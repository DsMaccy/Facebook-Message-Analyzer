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
        public static ConfigManager Manager = new ConfigManager();
        private SqlConnection m_sqlConnection;
        private Database m_db;

        private ConfigManager()
        {
            m_db = new Database("config");
        }

        public dynamic getValue (string table, string key)
        {
            dynamic value = null;

            // TODO: Get value

            return value;
        }
    }
}
