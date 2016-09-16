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


        private const string DB_NAME = "config";
        private const string DB_CONN_STRING = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Data\ConfigDatabase.mdf;Integrated Security=True;";
        public const string GENERIC_TABLE_NAME = "genericPreferences";
        public const string DLL_PATH_TAG = "dllPath";

        private CachedMessagesManager()
        {
        }

        private List<FacebookMessage> getNextMessages()
        {
            List<FacebookMessage> messageList = new List<FacebookMessage>();

            return messageList;
        }
    }
}
