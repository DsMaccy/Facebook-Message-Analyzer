using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModuleInterface;

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
        public const string MESSAGE_TABLE = "messageTable";
        public const string MESSAGE_GROUP_TABLE = "messageGroupTable";
        public const string DLL_PATH_TAG = "dllPath";

        private CachedMessagesManager()
        {
            Database.createDatabase(DB_NAME, DB_CONN_STRING);
        }

        public void saveMessages(List<FacebookMessage> messageList)
        {
            // TODO: Fill
        }

        public List<FacebookMessage> getMessages()
        {
            List<FacebookMessage> messageList = new List<FacebookMessage>();



            return messageList;
        }
    }
}
