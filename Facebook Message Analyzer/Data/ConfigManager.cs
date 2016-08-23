using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Facebook_Message_Analyzer.Data
{
    class ConfigManager
    {
        private static ConfigManager manager;

        private const string DB_NAME = "config";
        private const string DB_CONN_STRING = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\dsmac\Desktop\Facebook Message Analyzer\Facebook Message Analyzer\Facebook Message Analyzer\Data\ConfigDatabase.mdf;Integrated Security=True";
        public const string GENERIC_TABLE_NAME = "genericPreferences";
        public const string DLL_PATH_TAG = "dllPath";

        public static ConfigManager Manager
        {
            get
            {
                if (ConfigManager.manager == null)
                {
                    ConfigManager.manager = new ConfigManager();
                }
                return ConfigManager.manager;
            }
        }

        private ConfigManager()
        {
            //Database.createDatabase(DB_NAME, DB_CONN_STRING);
            createGenericTable();
            
        }

        public dynamic getValue (string table, string key)
        {
            List<List<dynamic>> values = Database.getValue(DB_CONN_STRING, GENERIC_TABLE_NAME, DLL_PATH_TAG);
            if (values.Count > 0)
            {
                return values[0][0];
            }

            return null;
        }

        private void createGenericTable()
        {
            Dictionary<string, Type>  columns = new Dictionary<string, Type>();
            columns.Add(DLL_PATH_TAG, typeof(string));
            Database.addTable(DB_CONN_STRING, GENERIC_TABLE_NAME, columns);
        }
    }
}
