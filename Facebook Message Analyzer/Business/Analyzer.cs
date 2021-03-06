﻿using System;
using System.Collections.Generic;
using System.Collections;
using Facebook_Message_Analyzer.Data;
using System.Data;
using System.Threading.Tasks;
using System.Threading;
using ModuleInterface;

namespace Facebook_Message_Analyzer.Business
{
    class Analyzer
    {
        private List<IModule> m_analysisModules;
        private int m_idealThreadCount;
        private Semaphore m_sem;
        private Semaphore m_counterSem;
        private List<Thread> m_threads;
        private string m_conversationID;
        private Thread m_endThread;
        private bool m_empty;
        

        public Analyzer(string conversationID)
        {
            m_conversationID = conversationID;
            m_analysisModules = new List<IModule>();
            m_idealThreadCount = System.Environment.ProcessorCount / 2;
            m_sem = new Semaphore(m_idealThreadCount, m_idealThreadCount);
            m_counterSem = null;
            m_threads = new List<Thread>();
            m_endThread = null;
            m_empty = true;
        }

        public void setModules(List<Type> modules)
        {
            foreach (Type module in modules)
            {
                m_analysisModules.Add((IModule)Activator.CreateInstance(module));
            }
        }
        
        public void abort()
        {
            foreach (Thread thread in m_threads)
            {
                thread.Abort();
            }
        }

        public void runAnalysisAsync()
        {
            if (m_analysisModules.Count < 1)
            {
                ErrorMessages.NoAnalysisModuleSelected();
            }

            m_counterSem = new Semaphore(0, m_analysisModules.Count);

            m_endThread = new Thread(new ThreadStart(finishAnalysis));
            m_endThread.Start();

            Dictionary<string, Type> modules = StateMaster.getModules();
            Dictionary<Type, string> moduleTags = new Dictionary<Type, string>();
            foreach (KeyValuePair<string, Type> kvPair in modules)
            {
                moduleTags[kvPair.Value] = kvPair.Key;
            }

            foreach (IModule module in m_analysisModules)
            {
                m_threads.Add(new Thread(new ParameterizedThreadStart(analysisThread)));
                if (module.preferencesAvailable())
                {
                    module.savePreferences(StateMaster.getPreferenceData(moduleTags[module.GetType()]));
                }
                m_threads[m_threads.Count - 1].Start(module);
            }
        }
        
        private void finishAnalysis()
        {
            for (int i = 0; i < m_analysisModules.Count; i++)
            {
                m_counterSem.WaitOne();
            }
            Console.WriteLine("Finished waiting for analysis threads");

            StateMaster.closeAnalysisForm();

            Console.WriteLine("Analysis Form Closed");

            if (m_empty)
            {
                ErrorMessages.NoMessages();
                return;
            }
            foreach (IModule module in m_analysisModules)
            {
                if (module.formAvailable())
                {
                   StateMaster.displayAnalysisResult(module.getResultForm());
                }
            }
        } 

        private void analysisThread(object moduleObj)
        {
            lock (this)
            {
                IModule module = moduleObj as IModule;
                if (module == null)
                {
                    throw new ArgumentException("Object needs to inherit from IModule");
                }

                List<Thread> threads = new List<Thread>();
                try
                {
                    bool saveMessages = StateMaster.getCacheMessages();

                    ConversationIterator messages = new ConversationIterator(m_conversationID, saveMessages);

                    if (!module.canParallelize())
                    {
                        m_sem.WaitOne();
                        foreach (FacebookMessage message in messages)
                        {
                            m_empty = false;
                            module.analyze(message);
                        }
                        m_sem.Release(1);
                    }
                    else
                    {
                        ParallelOptions po = new ParallelOptions();
                        Parallel.ForEach<FacebookMessage>(messages,
                            new Action<FacebookMessage>((FacebookMessage message) =>
                       {
                            m_empty = false;
                            module.parallelAnalyze(message);
                        }));
                    }

                    m_counterSem.Release(1);
                }
                catch (System.Threading.ThreadInterruptedException)
                {
                    if (module.canParallelize())
                    {
                        // TODO: Cleanup threads created in parallel version
                    }
                }
            }
        }
    }
}
