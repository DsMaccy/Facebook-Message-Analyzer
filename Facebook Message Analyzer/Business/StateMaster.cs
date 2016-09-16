/* TODO
 * Add Message Analysis
 * Check m_next values and make sure query works for FBQueryManager
 * Add saving and querying to CachedMessagesManager
 * Get ConversationIterator to work
 * Create Threads to run analysis
 * Create modules for: points, profanity, and general statistics
 * Find way to Parse DLL files in path (preferences) and add modules to application
 * Create Constants for Width and Heigth offsets
 * Create installer...?
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Facebook_Message_Analyzer.Business;
using Facebook_Message_Analyzer.Presentation;
using Facebook_Message_Analyzer.Data;
using ModuleInterface;
using System.IO;
using System.Reflection;
using GeneralInfoModule;
using System.Threading;


namespace Facebook_Message_Analyzer.Business
{
    internal static class StateMaster
    {
        private static Form m_activeForm = null;
        private static bool m_loggedIn = false;
        private static bool m_restart = false;
        private static List<Type> m_activeModules = new List<Type>();
        private static List<List<Thread>> m_threads = null;
        private static Thread masterThread = null;
        private static List<IModule> m_runningModules = null;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            m_activeModules.Add(typeof(GeneralInfo));

            do
            {
                m_restart = false;
                // Set the Active Form to the welcome screen
                m_activeForm = new WelcomeForm();
                Application.Run(m_activeForm);

                if (!m_loggedIn)
                {
                    return;
                }

                m_activeForm = new ConversationSelectionForm();
                Application.Run(m_activeForm);
            } while (m_restart);
        }

        public static void login()
        {
            AuthenticationForm loginScreen = new AuthenticationForm(FBQueryManager.Manager.getLoginURL());
            loginScreen.ShowDialog();
            if (m_loggedIn)
            {
                m_activeForm.Close();
            }
        }

        public static void setOAuthToken(string token)
        {
            if (token != null && token != "")
            {
                FBQueryManager.Manager.setToken(token);
                //m_activeForm.Close();
                m_loggedIn = true;
            }
            else
            {
                m_loggedIn = false;
            }
        }

        public static void logout()
        {
            AuthenticationForm loginScreen = new AuthenticationForm(FBQueryManager.Manager.getLogoutURL());
            loginScreen.ShowDialog();
            

            // Reset states and start over from the Welcome Screen
            m_loggedIn = false;
            m_activeForm.Close();
            m_restart = true;
        }

        public static string getLogoutRedirectUrl()
        {
            return Data.FBQueryManager.LOGOUT_URL;
        }

        public static dynamic getConversations()
        {
            return FBQueryManager.Manager.getConversations();
        }

        public static void setConversation()
        {
            FBQueryManager.Manager.setConversation();
        }

        public static void selectAnalysisModules()
        {
            SelectModulesForm smf = new SelectModulesForm();
            smf.ShowDialog();
        }

        public static void runAnalysisModules(int conversationIndex)
        {

            if (m_activeModules.Count == 0)
            {
                ErrorMessages.NoAnalysisModuleSelected();
                return;
            }
            masterThread = new Thread(new ThreadStart(
                () =>
                {
                    bool analysisAborted = false;
                    try
                    {
                        // Find out optimal number of threads
                        int idealNumThreads = System.Environment.ProcessorCount / 2;
                        m_threads = new List<List<Thread>>();
                        m_runningModules = new List<IModule>();
                        int numParallelizeableModules = 0;
                        int numThreads = 0;

                        Console.WriteLine("Number of Processors / 2 ( == ideal # of cores) = " + idealNumThreads);

                        // Setup the various threads
                        foreach (Type module in m_activeModules)
                        {
                            IModule moduleInstance = Activator.CreateInstance(module) as IModule;
                            m_runningModules.Add(moduleInstance);

                            if (moduleInstance.canParallelize())
                            {
                                numParallelizeableModules++;
                            }

                            List<Thread> threadContainer = new List<Thread>();
                            threadContainer.Add(
                                new Thread
                                (
                                    new System.Threading.ThreadStart
                                    (
                                        () =>
                                        {
                                            // For all message values
                                            //      If parallelizeable
                                            //          Create children threads to run analysis based on the number of 
                                            //      Else
                                            //          Analyze the next message either within this thread
                                        }
                                    )
                                )
                            );
                        }
                    }
                    catch (Exception)
                    {
                        analysisAborted = true;
                    }
                    finally
                    {
                        // Cleanup
                        foreach (List<Thread> threadList in m_threads)
                        {
                            foreach (Thread thread in threadList)
                            {
                                thread.Abort();
                            }
                        }
                        if (analysisAborted)
                        {
                            ErrorMessages.AnalysisAborted();
                        }
                    }

                }));

            masterThread.Start();

            AnalyzingForm af = new AnalyzingForm();
            af.ShowDialog();
        }

        public static void showPreferences()
        {
            ModulePreferencesForm mpf = new ModulePreferencesForm();
            mpf.ShowDialog();
        }

        public static Dictionary<string, string> getPreferenceData(string tag)
        {
            Dictionary<string, string> values = new Dictionary<string, string>();
            if (tag != "General")
            {
                Dictionary<string, Type> modules = getModules();
                Type[] types = new Type[0];
                object[] parameters = new object[0];
                IModule moduleObject = (modules[tag]).GetConstructor(types).Invoke(parameters) as IModule;
                Dictionary<string, dynamic> preferenceValues = moduleObject.getSavedProperties();
                foreach (KeyValuePair<string, dynamic> kvPair in preferenceValues)
                {
                    values[kvPair.Key] = kvPair.Value.ToString();
                }
                return values;
            }
            else
            {
                dynamic value = ConfigManager.Manager.getValue(ConfigManager.GENERIC_TABLE_NAME, ConfigManager.DLL_PATH_TAG);
                values.Add("modulePath", value.ToString());
                return values;
            }
        }

        public static Dictionary<string, Type> getModules()
        {
            Dictionary<string, Type> modules = new Dictionary<string, Type>();
            modules.Add("General Info Module", typeof(GeneralInfoModule.GeneralInfo));

            string dllPath = ConfigManager.Manager.getValue(ConfigManager.GENERIC_TABLE_NAME, ConfigManager.DLL_PATH_TAG);
            if (dllPath != null)
            {
                foreach (string file in Directory.EnumerateFiles(dllPath, "*.dll"))
                {
                    Assembly assembly = Assembly.LoadFrom("dllPath");
                    if (assembly.GetType() is IModule)
                    {
                        AppDomain.CurrentDomain.Load(assembly.GetName());
                        modules.Add(assembly.GetName() + "Module", assembly.GetType());
                    }
                }
            }

            return modules;
        }

        public static bool isModuleActive(Type module)
        {
            for (int i = 0; i < m_activeModules.Count; i++)
            {
                if (module.Equals(m_activeModules[i]))
                {
                    return true;
                }
            }
            return false;
        }

        public static void setActiveModules(params Type [] activeModules)
        {
            m_activeModules.Clear();
            for (int i = 0; i < activeModules.Length; i++)
            {
                m_activeModules.Add(activeModules[i]);
            }
        }

        public static void setDllLocations(params string[] filePaths)
        {
            ConfigManager.Manager.clearTable(ConfigManager.GENERIC_TABLE_NAME);
            for (int i = 0; i < filePaths.Length; i++)
            {
                Dictionary<string, object> value = new Dictionary<string, object>();
                value.Add(ConfigManager.DLL_PATH_TAG, filePaths[i]);
                ConfigManager.Manager.addValues(ConfigManager.GENERIC_TABLE_NAME, value);
            }

        }

        public static void Exit()
        {
            m_activeForm.Close();
            m_activeForm = null;
        }
    }
}
