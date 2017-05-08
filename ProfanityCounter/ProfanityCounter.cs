using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ModuleInterface;
using System.Data;
using System.IO;

namespace ProfanityCounter
{
    public class ProfanityCounter : IModule
    {
        private Dictionary<User, Dictionary<string, int>> m_profanityTable;
        private Dictionary<User, Dictionary<FacebookMessage, List<string>>> m_flaggedMessages;
        private List<string> m_profaneWords = new List<string>();
        private bool m_censor;
        private bool m_showWordBreakdown;
        private bool m_saveFlaggedMessages;
        private string m_saveFolder;

        public ProfanityCounter()
        {
            m_profaneWords = new List<string>();
            m_profanityTable = new Dictionary<User, Dictionary<string, int>>();
            m_censor = true;
            m_showWordBreakdown = true;
            m_saveFlaggedMessages = false;
            m_saveFolder = "";
            m_flaggedMessages = new Dictionary<User, Dictionary<FacebookMessage, List<string>>>();
        }

        #region IModule Interface Methods

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
            lock (m_profanityTable)
            {
                if (!m_profanityTable.ContainsKey(fm.sender))
                {
                    m_profanityTable[fm.sender] = new Dictionary<string, int>();
                }
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

            foreach (string profane_word in m_profaneWords)
            {
                List<string> profaneWords = new List<string>();
                foreach (string message_word in message)
                {
                    if (message_word == profane_word || 
                        (message_word.Length > profane_word.Length && message_word.Substring(0, profane_word.Length) == profane_word)
                        )
                    {
                        lock (m_profanityTable[fm.sender])
                        {
                            if (!m_profanityTable[fm.sender].ContainsKey(profane_word))
                            {
                                m_profanityTable[fm.sender][profane_word] = 0;
                            }
                            m_profanityTable[fm.sender][profane_word]++;

                            if (!m_flaggedMessages.ContainsKey(fm.sender))
                            {
                                lock (m_flaggedMessages)
                                {
                                    m_flaggedMessages.Add(fm.sender, new Dictionary<FacebookMessage, List<string>>());
                                }
                            }
                            if (!m_flaggedMessages[fm.sender].ContainsKey(fm))
                            {
                                m_flaggedMessages[fm.sender].Add(fm, new List<string>());
                            }
                            m_flaggedMessages[fm.sender][fm].Add(profane_word);
                        }
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
            fillDataTable(dt);
            if (m_censor)
            {
                censorColumnNames(dt);
            }

            trimColumns(dt);

            if (m_saveFlaggedMessages)
            {
                saveFlaggedMessages();
            }

            return new ResultsScreen(dt);
        }

        public void savePreferences(Dictionary<string, object> newValues)
        {
            m_profaneWords = new List<string>(((string)newValues["wordFlags"]).Split(';'));
            m_showWordBreakdown = (bool)newValues["showBreakdown"];
            m_censor = (bool)newValues["innocent"];
            m_saveFlaggedMessages = (bool)newValues["saveChecked"];
            m_saveFolder = (string)newValues["saveLocation"];
        }

        #endregion

        #region Private Helper Methods
        private void fillDataTable(DataTable dt)
        {
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
        }

        private void censorColumnNames(DataTable dt)
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
        /// <summary>
        /// Remove profanity columns that are unused (0 count across all users)
        /// </summary>
        /// <param name="dt"></param>
        private void trimColumns(DataTable dt)
        {
            List<DataColumn> columnsToRemove = new List<DataColumn>();
            foreach (DataColumn column in dt.Columns)
            {
                if (column.ColumnName != "total" && column.ColumnName != "user")
                {
                    bool instanceFound = false;
                    foreach (DataRow row in dt.Rows)
                    {
                        if ((int.Parse((string)row[column]) != 0))
                        {
                            instanceFound = true;
                            break;
                        }
                    }
                    if (!instanceFound)
                    {
                        columnsToRemove.Add(column);
                    }
                }
            }
            foreach (DataColumn column in columnsToRemove)
            {
                dt.Columns.Remove(column);
            }
        }

        private void saveFlaggedMessages()
        {
            /********File Contents*******
             * Intro Message
             * Messages
             * {
             *      message id
             *      {
             *          flagged words
             *          [ 
             *              word1,
             *              word2,
             *              ...
             *          ]
             *          message time
             *          message text
             *      }
             * }
             */
            if (m_saveFolder == "")
            { m_saveFolder = "."; }
            Directory.CreateDirectory(m_saveFolder);
            foreach (KeyValuePair<User, Dictionary<FacebookMessage, List<string>>> kv in m_flaggedMessages)
            {
                if (m_saveFolder[m_saveFolder.Length - 1] != '\\' && m_saveFolder[m_saveFolder.Length - 1] != '/')
                { m_saveFolder += "\\"; }
                User user = kv.Key;
                string fileName = m_saveFolder + "_" + user.name + "_" + user.id + ".txt";
                
                string fileContents = "Flagged profane messages for " + user.name + "\n";
                fileContents += "Messages:\n{\n";

                Dictionary<FacebookMessage, List<string>> messages = kv.Value;
                foreach (KeyValuePair<FacebookMessage, List<string>> message in messages)
                {
                    fileContents += "\t" + message.Key.id + ":\n\t{\n";
                    fileContents += "\t\tFlaggedWords: \n\t\t[\n";
                    fileContents += "\t\t\t" + String.Join(",\n\t\t\t", message.Value);
                    fileContents += "\n\t\t]\n";
                    fileContents += "\t\tTimeSent: " + message.Key.timeSent.ToString() + "\n";
                    fileContents += "\t\tMessage: " + message.Key.message + "\n";
                    fileContents += "\t}\n";
                }
                fileContents += "}";
                File.WriteAllText(fileName, fileContents);
            }
        }
        #endregion
    }
}
