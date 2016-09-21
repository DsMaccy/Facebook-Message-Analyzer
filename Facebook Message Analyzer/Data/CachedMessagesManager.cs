using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModuleInterface;
using System.Data.Sql;
using System.Data.SqlClient;

namespace Facebook_Message_Analyzer.Data
{
    class CachedMessagesManager
    {

        #region Singleton
        private static CachedMessagesManager manager = null;

        public static CachedMessagesManager Manager
        {
            get
            {
                if (CachedMessagesManager.manager == null)
                {
                    CachedMessagesManager.manager = new CachedMessagesManager();
                }
                return CachedMessagesManager.manager;
            }
        }
        #endregion


        /* Database Organization:
         *      Message Table: 
         *          Conversation ID
         *          next
         *      Messages:
         *           
         */

        public const string DB_NAME = "cachedMessages";
        private const string DB_CONN_STRING = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Data\CachedMessages.mdf;Integrated Security=True;";
        private const string LINK_TABLE = "linkTable";
        private const string USER_TABLE = "userTable";
        private string[] FB_MESSAGE_ROWS
        {
            get
            {
                return new string[] { "id", "message", "sender", "timeSent"};
            }
            
        }
        // Other tables referenced by conversation IDs

        private CachedMessagesManager()
        {
            Database.createDatabase(DB_NAME, DB_CONN_STRING);
        }

        internal string getNextURL(string conversationID)
        {
            Dictionary<string, object> whereClause = new Dictionary<string, object>();
            whereClause.Add("id", conversationID);
            return Database.getValue(DB_CONN_STRING, LINK_TABLE, "next", whereClause) as string;
        }

        public void saveMessages(string conversationID, List<FacebookMessage> messageList, string nextURL)
        {
            Dictionary<string, object> values;
            foreach (FacebookMessage message in messageList)
            {
                values = new Dictionary<string, object>();
                foreach (string tag in FB_MESSAGE_ROWS)
                {
                    values.Add(tag, message[tag]);
                }
                Database.addValues(DB_CONN_STRING, conversationID, values);
            }

            values = new Dictionary<string, object>();
            values.Add("id", conversationID);
            values.Add("next", nextURL);
            Database.addValues(DB_CONN_STRING, LINK_TABLE, values);
        }

        public List<FacebookMessage> getMessages(string conversationID)
        {
            List<FacebookMessage> messageList = new List<FacebookMessage>();
            List<List<dynamic>> table = Database.getTable(DB_CONN_STRING, conversationID, FB_MESSAGE_ROWS);


            foreach (List<dynamic>row in table)
            {
                FacebookMessage fm = new FacebookMessage();
                int index = 0;
                foreach (string tag in FB_MESSAGE_ROWS)
                {
                    fm[tag] = row[index++];
                }
                messageList.Add(fm);
            }

            return messageList;
        }

        public List<FacebookMessage> getEarliestEntries(string conversationID)
        {
            List<FacebookMessage> messages = new List<FacebookMessage>();
            using (SqlConnection sqlConnection = new SqlConnection(DB_CONN_STRING))
            {
                sqlConnection.Open();
                DateTime minDT;
                using (SqlCommand command = new SqlCommand())
                {
                    command.CommandText = "Select MIN(date) AS MinDate FROM @conversationID";
                    command.Parameters.AddWithValue("@conversationID", conversationID);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        minDT = (DateTime)reader.GetValue(0);
                    }
                }
                using (SqlCommand command = new SqlCommand())
                {
                    command.CommandText = "Select * from @conversationID where date=@minDate";
                    command.Parameters.AddWithValue("@conversationID", minDT);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            FacebookMessage fm = new FacebookMessage();
                            fm.id = reader["id"] as string;
                            fm.message = reader["message"] as string;
                            fm.timeSent = (DateTime)reader["timeSent"];
                            fm.sender = new User();
                            fm.sender.id = reader["sender"] as string;
                            using (SqlCommand getUser = new SqlCommand())
                            {
                                getUser.CommandText = "Select name from " + USER_TABLE + "where id=@id";
                                getUser.Parameters.AddWithValue("@id", fm.sender.id);
                                using (SqlDataReader userReader = getUser.ExecuteReader())
                                {
                                    while (userReader.Read())
                                    {
                                        fm.sender.name = userReader.GetString(0);
                                    }
                                }
                            }
                            messages.Add(fm);
                        }
                    }
                }
            }
            return messages;
        }

        public List<FacebookMessage> getLastestEntries(string conversationID)
        {
            throw new NotImplementedException();
        }
    }
}
