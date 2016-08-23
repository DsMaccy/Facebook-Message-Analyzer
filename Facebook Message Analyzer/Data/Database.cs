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
        private static List<string> previouslyUsedNames = new List<string>();

        public static bool createDatabase(string dbName, string connectString)
        {
            if (previouslyUsedNames.Contains(dbName))
            {
                return false;
            }

            Console.WriteLine(DatabaseConstants.PATH);
            string command = "create DATABASE if not exists " + dbName;
            using (SqlConnection sqlConnection = new SqlConnection(connectString))
            {
                sqlConnection.Open();
                using (SqlCommand sc = new SqlCommand(command, sqlConnection))
                {
                    sc.ExecuteNonQuery();
                }
            }

            previouslyUsedNames.Add(dbName);
            return true;
        }

        public static bool addTable(string connectString, string tableName, Dictionary<string, Type> columns)
        {
            string command = "IF NOT EXISTS (select * from sysobjects where name=\'"+ tableName + "\') CREATE TABLE " + tableName + "(";

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
                    return false;
                }
                
            }
            command += ");";
            using (SqlConnection sqlConnection = new SqlConnection(connectString))
            {
                sqlConnection.Open();
                using (SqlCommand sc = new SqlCommand(command, sqlConnection))
                {
                    sc.ExecuteNonQuery();
                }
            }
            return true;
        }

        public static List<List<dynamic>> getValue(string connectString, string tableName, string table, params string[] keys)
        {

            // Create SQL command string by conglomerating data values
            List<List<dynamic>> value = null;
            //SqlConnection sqlConnection = new SqlConnection(connectString);

            string command = "SELECT ";
            if (keys.Length == 0)
            {
                command += "* ";
            }
            else
            {
                for (int index = 0; index < keys.Length; index++)
                {
                    command += keys[index];
                    if (index < keys.Length - 1)
                    {
                        command += ",";
                    }
                }
            }
            command += "FROM " + tableName + ";";
            // TODO: Check this value
            Console.WriteLine(command);

            // Run Command in SQL, retrieve and parse data


            value = new List<List<dynamic>>();
            using (SqlConnection sqlConnection = new SqlConnection(connectString))
            {
                sqlConnection.Open();
                using (SqlCommand sc = new SqlCommand(command, sqlConnection))
                {
                    using (SqlDataReader reader = sc.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // TODO: Check this value
                            Console.WriteLine(reader);
                            List<dynamic> newRow = new List<dynamic>();
                            for (int index = 0; index < keys.Length; index++)
                            {
                                newRow.Add(reader[keys[index]]);
                            }

                            value.Add(newRow);
                        }
                    }
                }


            }

            // TODO: Check this value
            Console.WriteLine(value);
            return value;
        }
    }
}
