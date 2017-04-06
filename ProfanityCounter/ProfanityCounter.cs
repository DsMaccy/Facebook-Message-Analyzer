using System;
using System.Collections.Generic;
using System.Threading;
using ModuleInterface;
using System.Data;

namespace ProfanityCounter
{
    public class ProfanityCounter : IModule
    {

        private Dictionary<User, Dictionary<string, int>> m_profanityTable;
        private List<string> m_profaneWords = new List<string>();
        private bool m_censor;
        private bool m_showWordBreakdown;

        public ProfanityCounter()
        {
            m_profaneWords = new List<string>();
            m_profanityTable = new Dictionary<User, Dictionary<string, int>>();
            m_censor = true;
            m_showWordBreakdown = true;
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
            if (!m_profanityTable.ContainsKey(fm.sender))
            {
                m_profanityTable[fm.sender] = new Dictionary<string, int>();
            }
            if (fm.message == null)
            { return; }
            SortedSet<string> message = new SortedSet<string>(fm.message.ToLower().Split());
            foreach(string word in new List<string>(message))
            {
                string[] morewords = word.Split('-');
                for (int index = 0; index < morewords.Length; index++)
                {
                    if (morewords[index] != "")
                    {
                        message.Add(morewords[index]);
                    }
                }
            }
            foreach (string word in m_profaneWords)
            {
                if (message.Contains(word))
                {
                    lock (m_profanityTable[fm.sender])
                    {
                        if (!m_profanityTable[fm.sender].ContainsKey(word))
                        {
                            m_profanityTable[fm.sender][word] = 0;
                        }
                        m_profanityTable[fm.sender][word]++;
                    }
                }
            }
        }

        public void analyze(FacebookMessage fm)
        {
            throw new NotImplementedException("The Parallel Analysis should be called instead");
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
            DataTable dt = new DataTable();
            dt.Columns.Add("user");
            if (m_showWordBreakdown)
            {
                foreach (string word in m_profaneWords)
                {
                    dt.Columns.Add(word);
                }
            }
            dt.Columns.Add("total");
            dt.Rows.Clear();
            foreach (User user in m_profanityTable.Keys)
            {
                DataRow dr = dt.NewRow();
                int total = 0;
                dr["user"] = user.name;
                
                foreach (string word in m_profaneWords)
                {
                    if (m_showWordBreakdown)
                    { dr[word] = 0; }
                    if (m_profanityTable[user].ContainsKey(word))
                    {
                        total += m_profanityTable[user][word];
                        if (m_showWordBreakdown)
                        { dr[word] = m_profanityTable[user][word]; }
                    }
                }
                dr["total"] = total;
                dt.Rows.Add(dr);
            }

            if (m_censor)
            {
                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName != "total" && column.ColumnName != "user")
                    {
                        column.ColumnName = column.ColumnName.Replace('a', '*');
                        column.ColumnName = column.ColumnName.Replace('e', '*');
                        column.ColumnName = column.ColumnName.Replace('i', '*');
                        column.ColumnName = column.ColumnName.Replace('o', '*');
                        column.ColumnName = column.ColumnName.Replace('u', '*');
                    }
                }
            }

            return new ResultsScreen(dt);
        }

        public void savePreferences(Dictionary<string, object> newValues)
        {
            m_profaneWords = new List<string>(((string)newValues["wordFlags"]).Split(';'));
            m_showWordBreakdown = (bool)newValues["showBreakdown"];
            m_censor = (bool)newValues["innocent"];
        }
    }
}
