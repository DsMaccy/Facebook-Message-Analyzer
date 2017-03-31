using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading;

namespace Facebook_Message_Analyzer.Data
{
    public enum DataSets
    {
        Config = 0,
        Messages = 1,
    }
    class AlreadyInitializedError : Exception
    {
        public AlreadyInitializedError() : base() { }
        public AlreadyInitializedError(string message) : base(message) { }
    }

    /// <summary>
    /// Thread-Safe DataSet Management Class
    /// </summary>
    public class DataSetManager
    {
        #region Singleton
        private static DataSetManager manager = null;
        
        public static DataSetManager Manager
        {
            get
            {
                if (manager == null)
                {
                    manager = new DataSetManager();
                }
                return manager;
            }
        }
        #endregion

        #region Instance Variables + Constructor
        private Dictionary<DataSets, DataSet> m_dataSets;
        private Dictionary<DataSets, string> m_savePaths;
        private Dictionary<DataSets, Semaphore> m_writeSems;
        private string PATH;

        public const string GENERIC_TABLE_NAME = "genericPreferences";
        public const string SELECTED_MODULES_TABLE_NAME = "activeModules";
        public const string SELECTED_MODULES_NAME_COLUMN_TAG = "name";

        private DataSetManager()
        {
            m_writeSems = new Dictionary<DataSets, Semaphore>();
            m_writeSems[DataSets.Config] = new Semaphore(1, 1);
            m_writeSems[DataSets.Messages] = new Semaphore(1, 1);

            PATH = Business.StateMaster.getPath();
            if (!Directory.Exists(PATH))
            {
                Directory.CreateDirectory(PATH);
            }

            m_savePaths = new Dictionary<DataSets, string>();
            m_savePaths[DataSets.Config] = "Config.xml";
            m_savePaths[DataSets.Messages] = "Messages.xml";

            m_dataSets = new Dictionary<DataSets, DataSet>();
            m_dataSets[DataSets.Config] = new Configurations();
            m_dataSets[DataSets.Messages] = new CachedMessages();
            //loadFiles();
            
        }

        #endregion

        #region FileStorage and Access

        internal void loadFiles()
        {
            
            foreach (KeyValuePair<DataSets, string> kvPair in m_savePaths)
            {
                try
                {
                    m_writeSems[kvPair.Key].WaitOne();
                    // TODO: Test that this filename is correct (BackSlashes and all)
                    string filename = PATH + FBQueryManager.Manager.getMe() + "\\" + kvPair.Value;
                    if (File.Exists(filename))
                    {
                        m_dataSets[kvPair.Key].ReadXml(filename);
                    }
                }
                finally
                {
                    m_writeSems[kvPair.Key].Release();
                }
            }
        }

        public void saveDataSet(DataSets ds)
        {
            try
            {
                m_writeSems[ds].WaitOne();
                Directory.CreateDirectory(PATH + FBQueryManager.Manager.getMe());
                m_dataSets[ds].WriteXml(PATH + FBQueryManager.Manager.getMe() + "\\" + m_savePaths[ds], XmlWriteMode.WriteSchema);
            }
            finally
            {
                m_writeSems[ds].Release();
            }
        }
        #endregion

        #region Data Access Methods

        public void addTable(DataSets ds, string tableName, Dictionary<string, Type> columns, params string[] primaryKeys)
        {
            
            if (m_dataSets[ds].Tables.Contains(tableName))
            {
                throw new AlreadyInitializedError("Table already exists in this data set");
            }

            try
            {
                m_writeSems[ds].WaitOne();
                m_dataSets[ds].Tables.Add(tableName);
                foreach (KeyValuePair<string, Type> kvPair in columns)
                {
                    m_dataSets[ds].Tables[tableName].Columns.Add(kvPair.Key, kvPair.Value);
                }

                DataColumn[] primaryKeyColumnsList = new DataColumn[primaryKeys.Length];
                for (int index = 0; index < primaryKeys.Length; index++)
                {
                    primaryKeyColumnsList[index] = m_dataSets[ds].Tables[tableName].Columns[primaryKeys[index]];
                }
                m_dataSets[ds].Tables[tableName].PrimaryKey = primaryKeyColumnsList;
            }
            finally
            {
                m_writeSems[ds].Release();
            }
        }

        public void removeTable(DataSets ds, string tableName)
        {
            try
            {
                m_writeSems[ds].WaitOne();
                m_dataSets[ds].Tables.Remove(tableName);
            }
            finally
            {
                m_writeSems[ds].Release();
            }
        }

        public void clearTable(DataSets ds, string tableName)
        {
            try
            {
                m_writeSems[ds].WaitOne();
                m_dataSets[ds].Tables[tableName].Clear();
            }
            finally
            {
                m_writeSems[ds].Release();
            }
        }

        public void addValuesToIndex(DataSets ds, string tableName, Dictionary<string, object> values, int saveIndex)
        {
            DataRow dataRow = m_dataSets[ds].Tables[tableName].NewRow();
            foreach (KeyValuePair<string, object> pair in values)
            {
                dataRow[pair.Key] = pair.Value;
            }

            try
            {
                m_writeSems[ds].WaitOne();
                m_dataSets[ds].Tables[tableName].Rows.Add(dataRow);
            }
            finally
            {
                m_writeSems[ds].Release();
            }
        }

        public void addValuesToEnd(DataSets ds, string tableName, Dictionary<string, object> values)
        {
            object[] dataRow = new object[values.Count];
            foreach (KeyValuePair<string, object> pair in values)
            {
                int index = m_dataSets[ds].Tables[tableName].Columns.IndexOf(pair.Key);
                dataRow[index] = pair.Value;
            }

            try
            {
                m_writeSems[ds].WaitOne();
                m_dataSets[ds].Tables[tableName].Rows.Add(dataRow);
            }
            finally
            {
                m_writeSems[ds].Release();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>true if the value needed to be added to the table</returns>
        public bool setValues(DataSets ds, string tableName, Dictionary<string, object> values)
        {
            string primaryKey = m_dataSets[ds].Tables[tableName].PrimaryKey[0].ColumnName;
            DataRow row = m_dataSets[ds].Tables[tableName].Rows.Find(values[primaryKey]);
            if (row == null)
            {
                addValuesToEnd(ds, tableName, values);
                return true;
            }

            try
            {
                m_writeSems[ds].WaitOne();
                foreach (KeyValuePair<string, object> pair in values)
                {
                    row[pair.Key] = pair.Value;
                }
                return false;
            }
            finally
            {
                m_writeSems[ds].Release();
            }
        }

        public void removeValues(DataSets ds, string tableName, Dictionary<string, object> primaryKeyValues)
        {
            DataTable table = m_dataSets[ds].Tables[tableName];
            object[] lookup = new object[primaryKeyValues.Count];
            foreach (KeyValuePair<string, object> pair in primaryKeyValues)
            {
                lookup[table.Columns.IndexOf(pair.Key)] = pair.Value;
            }

            try
            {
                m_writeSems[ds].WaitOne();
                DataRow rowToRemove = table.Rows.Find(lookup);
                table.Rows.Remove(rowToRemove);
            }
            finally
            {
                m_writeSems[ds].Release();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ds">Which dataset to query</param>
        /// <param name="tableName">The table being queried</param>
        /// <returns>System.Data.DataRowCollection enumerator whose elements are of type System.Data.DataRow or <code>null</code> if there is no data for the table</returns>
        public System.Collections.IEnumerator getData(DataSets ds, string tableName)
        {
            DataTable table = m_dataSets[ds].Tables[tableName];
            if (table == null)
            {
                return null;
            }
            DataRow[] dataRows;
            if (ds == DataSets.Messages)
            {
                dataRows = table.Select("", "id");
            }
            else
            {
                dataRows = table.Select();
            }
            return dataRows.GetEnumerator(); //table.Rows.GetEnumerator();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ds">Which dataset to query</param>
        /// <param name="tableName">The table being queried</param>
        /// <param name="primaryKeyLookupValue">The value used to search the Data Set</param>
        /// <returns>System.Data.DataRow that contains the specified primary key value or null if no row found</returns>
        public DataRow getData(DataSets ds, string tableName, string primaryKeyLookupValue)
        {
            return m_dataSets[ds].Tables[tableName].Rows.Find(primaryKeyLookupValue);
        }

        #endregion
    }
}