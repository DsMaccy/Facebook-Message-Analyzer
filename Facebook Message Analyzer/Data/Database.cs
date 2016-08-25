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

        public static void clearTable(string connectString, string tableName)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectString))
            {
                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand("DELETE FROM " + tableName, sqlConnection))
                {
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }

        public static void addValues(string connectString, string tableName, Dictionary<string, object> values)
        {
            // Conglomerate SQL column names and column values
            string columnNames = "";
            string columnValues = "";
            foreach (KeyValuePair<string, object> kvPair in values)
            {
                columnNames += kvPair.Key + ", ";
                columnValues += "\"" + kvPair.Value.ToString() + "\"" + ", ";     // TODO: Make this more general (work with numbers, dates, and possibly other types of objects
            }

            columnNames = columnNames.Substring(0, columnNames.Length - 2);
            columnValues = columnValues.Substring(0, columnValues.Length - 2);

            
            Console.WriteLine(columnNames);
            Console.WriteLine(columnValues);


            // TODO: Find out why exception is being thrown
            // Run insertion command through SQL
            using (SqlConnection sqlConnection = new SqlConnection(connectString))
            {
                sqlConnection.Open();
                string commandText = "INSERT INTO " + tableName + " (" + columnNames + ") VALUES (" + columnValues + ")";
                Console.WriteLine(commandText);
                using (SqlCommand sqlCommand = new SqlCommand(commandText, sqlConnection))
                {
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }
    }
}
