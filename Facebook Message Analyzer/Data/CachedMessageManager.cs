using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModuleInterface;

namespace Facebook_Message_Analyzer.Data
{
    /// <summary>
    /// Wrapper for DataSetManager/Database that deals specifically with saved Facebook Messages
    /// </summary>
    class CachedMessageManager
    {

        #region Singleton
        private static CachedMessageManager manager = null;

        public static CachedMessageManager Manager
        {
            get
            {
                if (CachedMessageManager.manager == null)
                {
                    CachedMessageManager.manager = new CachedMessageManager();
                }
                return CachedMessageManager.manager;
            }
        }
        #endregion


        /* Database Organization:
         *      User Table:
         *          Corresponding Conversation ID
         *          Name
         *      Link Table:
         *          Conversation ID
         *          Corresponding Last Message ID
         *          next URL
         *      Messages:
         *          FacebookMessage struct (id, User, message, date)
         *           
         */

        // Databases (for SQL iteration)
        public const string DB_NAME = "cachedMessages";
        private const string DB_CONN_STRING = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Data\CachedMessages.mdf;Integrated Security=True;";

        // Table Names
        private const string LINK_TABLE = "linkTable";
        private const string USER_TABLE = "userTable";

        // Column Names
        private const string LINK_COLUMN_ID = "id";
        private const string LINK_COLUMN_MESSAGE_ID = "messageID";
        private const string LINK_COLUMN_LINK = "next";
        private const string USER_COLUMN_CONVERSATION = "id";
        private const string USER_COLUMN_USER = "";

        // Save Data Heuristic
        private const int SAVE_PARAM = 2;
        private int m_saveCount;

        private string[] FB_MESSAGE_COLUMNS
        {
            get
            {
                return new string[] { "id", "message", "sender", "timeSent" };
            }

        }
        // Other tables referenced by conversation IDs

        private CachedMessageManager()
        {
            m_saveCount = 0;

            Dictionary<string, Type> userColumns = new Dictionary<string, Type>();
            Dictionary<string, Type> linkColumns = new Dictionary<string, Type>();

            userColumns.Add(USER_COLUMN_CONVERSATION, typeof(string));
            userColumns.Add(USER_COLUMN_USER, typeof(User));

            linkColumns.Add(LINK_COLUMN_ID, typeof(string));
            linkColumns.Add(LINK_COLUMN_MESSAGE_ID, typeof(int));
            linkColumns.Add(LINK_COLUMN_LINK, typeof(string));

            // TODO: Create columns
            try
            {
                DataSetManager.Manager.addTable(DataSets.Messages, USER_TABLE, userColumns);
            }
            catch (AlreadyInitializedError)
            { }
            try
            {
                DataSetManager.Manager.addTable(DataSets.Messages, LINK_TABLE, linkColumns, LINK_COLUMN_ID, LINK_COLUMN_MESSAGE_ID);
            }
            catch(AlreadyInitializedError) 
            { }
        }

        internal Dictionary<int, string> getNextURLs(string conversationID)
        {
            Dictionary<int, string> results = new Dictionary<int, string>();
            
            IEnumerator iterator = DataSetManager.Manager.getData(DataSets.Messages, LINK_TABLE);
            if (iterator != null)
            {
                while (iterator.MoveNext())
                {
                    DataRow row = iterator.Current as DataRow;
                    results[(int)row[LINK_COLUMN_MESSAGE_ID]] = row[LINK_COLUMN_LINK] as string;
                }
            }
            return results;
        }

        private void replaceLinkValue(string conversationID, int smallestID, string nextURL)
        {
            // TODO: This searches all values in the link table, consider if this needs to be more efficient
            IEnumerator iterator = DataSetManager.Manager.getData(DataSets.Messages, LINK_TABLE);
            int nextLargestMessageID = Int32.MaxValue;
            if (iterator != null)
            {
                while (iterator.MoveNext())
                {
                    string linkConvoID = ((DataRow)iterator.Current)[LINK_COLUMN_ID] as string;
                    int messageID = (int)((DataRow)iterator.Current)[LINK_COLUMN_MESSAGE_ID];
                    if (linkConvoID == conversationID && messageID > smallestID && messageID < nextLargestMessageID)
                    {
                        nextLargestMessageID = messageID;
                    }
                }
            }
            Dictionary<string, object> values = new Dictionary<string, object>();
            values.Add(LINK_COLUMN_ID, conversationID);
            values.Add(LINK_COLUMN_LINK, nextURL);
            values.Add(LINK_COLUMN_MESSAGE_ID, smallestID);
            if (nextLargestMessageID != Int32.MaxValue)
            {
                Dictionary<string, object> primaryKeyValues = new Dictionary<string, object>();
                primaryKeyValues[LINK_COLUMN_MESSAGE_ID] = nextLargestMessageID;
                primaryKeyValues[LINK_COLUMN_ID] = conversationID;
                DataSetManager.Manager.removeValues(DataSets.Messages, LINK_TABLE, primaryKeyValues);
            }
            else
            {
                DataSetManager.Manager.addValuesToEnd(DataSets.Messages, LINK_TABLE, values);
            }
        }

        public void saveMessages(string conversationID, List<FacebookMessage> messageList, string nextURL, int saveIndex=-1)
        {
            Dictionary<string, object> values;
            int count = 0;
            int smallestID = messageList[0].id;
            foreach (FacebookMessage message in messageList)
            {
                if (message.id < smallestID)
                {
                    smallestID = message.id;
                }
                values = new Dictionary<string, object>();
                foreach (string tag in FB_MESSAGE_COLUMNS)
                {
                    values.Add(tag, message[tag]);
                }
                if (saveIndex < 0)
                {
                    DataSetManager.Manager.addValuesToEnd(DataSets.Messages, conversationID, values);
                }
                else
                {
                    DataSetManager.Manager.addValuesToIndex(DataSets.Messages, conversationID, values, saveIndex + count);
                }
                m_saveCount += 1;
                if (m_saveCount == SAVE_PARAM)
                {
                    m_saveCount = 0;
                    DataSetManager.Manager.saveDataSet(DataSets.Messages);
                }
            }
            replaceLinkValue(conversationID, smallestID, nextURL);
        }

        public void addConversation(string conversationID)
        {
            Dictionary<string, Type> columns = new Dictionary<string, Type>();
            foreach (string columnName in FB_MESSAGE_COLUMNS)
            {
                if (columnName == "id")
                {
                    columns.Add(columnName, typeof(int));
                }
                else if (columnName == "message")
                {
                    columns.Add(columnName, typeof(string));
                }
                else if (columnName == "sender")
                {
                    columns.Add(columnName, typeof(User));
                }
                else if (columnName == "timeSent")
                {
                    columns.Add(columnName, typeof(DateTime));
                }
                else
                {
                    throw new TypeInitializationException("Unkown Field being added to message table", new Exception());
                }
            }

            try
            {
                DataSetManager.Manager.addTable(DataSets.Messages, conversationID, columns, FB_MESSAGE_COLUMNS[0]);
            }
            catch (AlreadyInitializedError)
            { }
        }

        /// <summary>
        /// Retrieves the messages associated with a specific conversation
        /// </summary>
        /// <param name="conversationID">The conversation tag to get the messages for</param>
        /// <returns>The list of saved messages which is empty if no data is saved for the specific conversation</returns>
        public List<FacebookMessage> getMessages(string conversationID)
        {
            List<FacebookMessage> messageList = new List<FacebookMessage>();
            IEnumerator iterator = DataSetManager.Manager.getData(DataSets.Messages, conversationID);
            //List<List<dynamic>> table = Database.getTable(DB_CONN_STRING, conversationID, FB_MESSAGE_ROWS);
            if (iterator != null)
            {
                while (iterator.MoveNext())
                {
                    System.Data.DataRow row = iterator.Current as System.Data.DataRow;
                    FacebookMessage fm = new FacebookMessage();
                    foreach (string tag in FB_MESSAGE_COLUMNS)
                    {
                        fm[tag] = row[tag];
                    }
                    messageList.Add(fm);
                }
            }

            return messageList;
        }
        /*
        public List<FacebookMessage> getEarliestEntries(string conversationID)
        {
            List<FacebookMessage> messages = new List<FacebookMessage>();
            using (SqlConnection sqlConnection = new SqlConnection(DB_CONN_STRING))
            {
                sqlConnection.Open();
                DateTime minDT;
                using (SqlCommand command = new SqlCommand("", sqlConnection))
                {
                    command.CommandText = "Select MIN(date) AS MinDate FROM @conversationID";
                    command.Parameters.AddWithValue("@conversationID", conversationID);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        minDT = (DateTime)reader.GetValue(0);
                    }
                }
                using (SqlCommand command = new SqlCommand("", sqlConnection))
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
        }*/

        public FacebookMessage getLastestEntry(string conversationID)
        {
            IEnumerator iterator = DataSetManager.Manager.getData(DataSets.Messages, conversationID);
            if (iterator == null)
            {
                return new FacebookMessage();
            }
            iterator.MoveNext();
            FacebookMessage fm = new FacebookMessage();
            DataRow row = iterator.Current as DataRow;
            foreach (string tag in FB_MESSAGE_COLUMNS)
            {
                fm[tag] = row[tag];
            }

            return fm;
        }
    }
}
