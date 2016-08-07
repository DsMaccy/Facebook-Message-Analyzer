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

            string command = "create DATABASE if not exists " + dbName + ";";
            SqlConnection sqlConnection = new SqlConnection();
            SqlCommand sc = new SqlCommand(command, sqlConnection);

            sqlConnection.BeginTransaction();
            sc.ExecuteNonQuery();
            sqlConnection.Close();
            previouslyUsedNames.Add(dbName);
            return true;
        }

        public static bool addTable(string dbName, string tableName, Dictionary<string, Type> columns)
        {
            string command = "create Table if not exists " + tableName + "(";

            foreach (KeyValuePair<string, Type> kvPair in columns)
            {
                if (kvPair.Value == typeof(int))
                {
                    command += kvPair.Key + "integer not null,";
                }
                else if (kvPair.Value == typeof(string))
                {
                    command += kvPair.Key + " varchar not null,";
                }
                else
                {
                    Console.Write("Invalid type for key: \'" + kvPair.Key + "\'");
                }
                
            }
            command += ");";
            SqlConnection sqlConnection = new SqlConnection();
            SqlCommand sc = new SqlCommand(command, sqlConnection);

            sqlConnection.BeginTransaction();
            sc.ExecuteNonQuery();
            sqlConnection.Close();
            return true;
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
