using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ModuleInterface;

namespace GeneralInfoModule
{
    public class GeneralInfo : IModule
    {
        public void analyze(Message message)
        {
            throw new NotImplementedException();
        }

        public void analyze(FacebookMessage message)
        {
            throw new NotImplementedException();
        }

        public bool canParallelize()
        {
            return true;
        }

        public string description()
        {
            throw new NotImplementedException();
        }

        public bool formAvailable()
        {
            throw new NotImplementedException();
        }

        public System.Windows.Forms.UserControl getPreferenceForm()
        {
            throw new NotImplementedException();
        }

        public void parallelAnalyze(Message message)
        {
            throw new NotImplementedException();
        }

        public void parallelAnalyze(FacebookMessage message)
        {
            throw new NotImplementedException();
        }

        public bool preferencesAvailable()
        {
            throw new NotImplementedException();
        }

        public void savePreferences(Dictionary<string, dynamic> newValues)
        {
            throw new NotImplementedException();
        }

        System.Windows.Forms.UserControl IModule.getPreferenceForm()
        {
            throw new NotImplementedException();
        }
    }
}
