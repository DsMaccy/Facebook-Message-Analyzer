using System;
using System.Collections.Generic;
using System.Threading;
using ModuleInterface;
using System.Data;

namespace ProfanityCounter
{
    public class ProfanityCounter : IModule
    {
        List<string> m_profaneWords = new List<string>();
        DataTable m_dataTable;

        public ProfanityCounter()
        {
            m_dataTable = new DataTable();
            m_profaneWords = new List<string>();
        }

        public string description()
        {
            return "Count how many times each user has used profanity";
        }

        public bool canParallelize()
        {
            return true;
        }

        public void parallelAnalyze(FacebookMessage fm)
        {
            throw new NotImplementedException();
        }

        public void analyze(FacebookMessage fm)
        {
            throw new NotImplementedException();
        }

        public bool preferencesAvailable()
        {
            return true;
        }

        public PreferenceControl getPreferenceControl()
        {
            return new ProfanityOptions();
        }

        public bool formAvailable()
        {
            return true;
        }

        public System.Windows.Forms.Form getResultForm()
        {
            return new ResultsScreen(m_dataTable);
        }

        public void savePreferences(Dictionary<string, object> newValues)
        {
            throw new NotImplementedException();
        }
    }
}
