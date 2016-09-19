using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ModuleInterface;
using System.Data;

namespace GeneralInfoModule
{
    public class GeneralInfo : IModule
    {

        private List<User> m_users;
        private Dictionary<User, int> m_numMessages;
        private Dictionary<User, DateTime> m_earliestMessages;
        private Dictionary<User, DateTime> m_latestMessages;
        
        public GeneralInfo()
        {
            m_users = new List<User>();
            m_numMessages = new Dictionary<User, int>();
            m_earliestMessages = new Dictionary<User, DateTime>();
            m_latestMessages = new Dictionary<User, DateTime>();
        }
        
        #region IModule Methods

        public void analyze(FacebookMessage message)
        {
            // Add User to user list if not already in it
            if (!m_users.Contains(message.sender))
            {
                m_users.Add(message.sender);
                m_numMessages.Add(message.sender, 0);
                m_earliestMessages.Add(message.sender, message.timeSent);
                m_latestMessages.Add(message.sender, message.timeSent);
            }

            // Increase message count by 1 for this user
            m_numMessages[message.sender]++; 

            // Set earliest message time if applicable
            if (m_earliestMessages[message.sender].CompareTo(message.timeSent) > 0)
            {
                m_earliestMessages[message.sender] = message.timeSent;
            }

            // Set latest message time if applicable
            if (m_latestMessages[message.sender].CompareTo(message.timeSent) < 0)
            {
                m_latestMessages[message.sender] = message.timeSent;
            }
            
        }

        public bool canParallelize()
        {
            return true;
        }

        public void parallelAnalyze(FacebookMessage message)
        {
            throw new NotImplementedException();
        }

        public string description()
        {
            return "Basic statistics and information for message group";
        }

        public bool formAvailable()
        {
            return true;
        }

        public Form getResultForm()
        {
            DataTable data = new DataTable("General Info");
            data.Columns.Add("User", typeof(string));
            data.Columns.Add("Number of Messages", typeof(int));
            data.Columns.Add("First Message", typeof(DateTime));
            data.Columns.Add("Last Message", typeof(DateTime));

            foreach (User user in m_users)
            {
                data.Rows.Add(user.name, m_numMessages[user], m_earliestMessages[user], m_latestMessages[user]);
            }

            return new ResultForm(data);
        }

        public bool preferencesAvailable()
        {
            throw new NotImplementedException();
        }

        public System.Windows.Forms.UserControl getPreferenceForm()
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, dynamic> getSavedProperties()
        {
            throw new NotImplementedException();
        }

        public void savePreferences(Dictionary<string, dynamic> newValues)
        {
            throw new NotImplementedException();
        }

        public void setSavedProperties(Dictionary<string, dynamic> dbValues)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
