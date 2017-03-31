using System;
using System.Windows.Forms;
using System.Collections.Generic;
using ModuleInterface;

namespace TestModule
{
    public class TestModule : IModule
    {
        private FacebookMessage m_previousMessage;
        private Dictionary<string, object> m_previousValues;
        public TestModule()
        {
            m_previousMessage = new FacebookMessage();
            m_previousValues = new Dictionary<string, object>();
        }

        public string description()
        {
            return "This is used to make sure that the analysis works correctly.";
        }

        public bool canParallelize()
        {
            return false;
        }

        public void parallelAnalyze(FacebookMessage fm)
        {
            throw new NotImplementedException("This should not be called");
        }

        public void analyze(FacebookMessage fm)
        {

            if ((m_previousMessage.id > fm.id && m_previousMessage.timeSent > fm.timeSent) || m_previousMessage.id == 0)
            {
                m_previousMessage = fm;
            }
            else
            {
                throw new ArgumentException("Inconsistent Results");
            }
        }

        public bool preferencesAvailable()
        {
            return true;
        }

        public PreferenceControl getPreferenceControl()
        {
            return new TestControl();
        }

        public bool formAvailable()
        {
            return false;
        }

        public Form getResultForm()
        {
            throw new NotImplementedException("This should not be called");
        }

        public void savePreferences(Dictionary<string, object> newValues)
        {
            List<string> changedControls = new List<string>();

            foreach (KeyValuePair<string, object> kv in newValues)
            {
                if (kv.Value != m_previousValues[kv.Key])
                {
                    changedControls.Add(kv.Key);
                }
            }


            MessageBox.Show("The following controls have been changed" + changedControls.ToString());
        }
    }
}
