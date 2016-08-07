using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Facebook_Message_Analyzer.Data
{
    static class Database
    {
        private static List<string> previouslyUsedNames;

        public static bool createDatabase(string dbName)
        {
            if (previouslyUsedNames.Contains(dbName))
            {
                return false;
            }

            Console.WriteLine(DatabaseConstants.PATH);

            string command = "create DATABASE if not exists " + dbName;
            SqlConnection sqlConnection = new SqlConnection();
            SqlCommand sc = new SqlCommand(command, sqlConnection);

            sqlConnection.BeginTransaction();
            sc.ExecuteNonQuery();
            sqlConnection.Close();
            return true;
        }

        public static bool addTable(string dbName, string tableName)
        {
            return false;
            // TODO: Implement
        }

        public static dynamic getValue(string dbName, string tableName, string table, string key)
        {
            dynamic value = null;

            // TODO: Get value

            return value;
        }
    }
}
