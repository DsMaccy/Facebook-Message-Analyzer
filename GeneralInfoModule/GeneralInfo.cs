using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModuleInterface;
using System.Windows.Form;

namespace GeneralInfoModule
{
    public class GeneralInfo : IModule
    {
        public void analyze(Message message)
        {
            throw new NotImplementedException();
        }

        public bool canParallelize()
        {
            return true;
        }

        public bool formAvailable()
        {
            throw new NotImplementedException();
        }

        public UserControl getPreferenceForm()
        {
            throw new NotImplementedException();
        }

        public void parallelAnalyze(Message message)
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
    }
}
