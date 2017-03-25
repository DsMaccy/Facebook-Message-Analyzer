﻿using System;
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

        public const string DLL_LOCATIONS_TABLE_NAME = "dllLocations";
        public const string GENERIC_TABLE_NAME = "genericPreferences";
        public const string DLL_PATH_TAG = "dllPath";
        public const string CACHE_DATA_TAG = "cacheMessages";

        private DataSetManager()
        {
            m_writeSems = new Dictionary<DataSets, Semaphore>();
            m_writeSems[DataSets.Config] = new Semaphore(1, 1);
            m_writeSems[DataSets.Messages] = new Semaphore(1, 1);

            System.Reflection.Assembly assemblyObj = System.Reflection.Assembly.GetExecutingAssembly();

            System.Reflection.AssemblyCompanyAttribute  companyAttr = System.Reflection.AssemblyCompanyAttribute.GetCustomAttribute(assemblyObj, typeof(System.Reflection.AssemblyCompanyAttribute)) as System.Reflection.AssemblyCompanyAttribute;
            string companyName = companyAttr.Company;

            System.Reflection.AssemblyTitleAttribute titleAttr = System.Reflection.AssemblyTitleAttribute.GetCustomAttribute(assemblyObj, typeof(System.Reflection.AssemblyTitleAttribute)) as System.Reflection.AssemblyTitleAttribute;
            string programTitle = titleAttr.Title;

            // ... AppData/Local/Solace Inc./Facebook Message Analyzer/
            PATH = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\" + companyName + "\\" + programTitle + "\\";
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

            foreach (KeyValuePair<DataSets, string> kvPair in m_savePaths)
            {
                // TODO: Test that this filename is correct (BackSlashes and all)
                string filename = PATH + kvPair.Key;
                if (File.Exists(filename))
                {
                    m_dataSets[kvPair.Key].ReadXml(filename);
                }
            }
        }

        #endregion

        #region Data Access Methods

        public void addTable(DataSets ds, string tableName, Dictionary<string, Type> columns, string primaryKey = null)
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
                    if (kvPair.Key == primaryKey)
                    {
                        m_dataSets[ds].Tables[tableName].PrimaryKey = new DataColumn[1] { m_dataSets[ds].Tables[tableName].Columns[kvPair.Key] };
                    }
                }
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

        public void addValues(DataSets ds, string tableName, Dictionary<string, object> values)
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
                addValues(ds, tableName, values);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ds">Which dataset to query</param>
        /// <param name="tableName">The table being queried</param>
        /// <returns>System.Data.DataRowCollection enumerator whose elements are of type System.Data.DataRow or <code>null</code> if there is no data for the table</returns>
        public System.Collections.IEnumerator getData(DataSets ds, string tableName)
        {
            DataTable table = m_dataSets[ds].Tables[tableName];
            if (table == null || table.Rows.Count == 0)
            {
                return null;
            }
            return table.Rows.GetEnumerator();
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

        public void saveDataSet(DataSets ds)
        {
            // Guard with semaphore
            m_writeSems[ds].WaitOne();
            m_dataSets[ds].WriteXml(PATH + m_savePaths[ds], XmlWriteMode.WriteSchema);
            m_writeSems[ds].Release();
        }

        #endregion
    }
}