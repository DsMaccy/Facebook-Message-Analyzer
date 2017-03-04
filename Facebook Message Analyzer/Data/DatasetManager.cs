﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

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
        private string PATH;

        public const string DLL_LOCATIONS_TABLE_NAME = "dllLocations";
        public const string GENERIC_TABLE_NAME = "genericPreferences";
        public const string DLL_PATH_TAG = "dllPath";
        public const string CACHE_DATA_TAG = "cacheMessages";

        private DataSetManager()
        {
            PATH = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            m_savePaths = new Dictionary<DataSets, string>();
            m_savePaths[DataSets.Config] = "Config";
            m_savePaths[DataSets.Messages] = "Messages";

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

        public void addTable(DataSets ds, string tableName, Dictionary<string, Type> columns)
        {
            if (m_dataSets[ds].Tables.Contains(tableName))
            {
                throw new AlreadyInitializedError("Table already exists in this data set");
            }
            m_dataSets[ds].Tables.Add(tableName);
            foreach (KeyValuePair<string, Type> kvPair in columns)
            {
                m_dataSets[ds].Tables[tableName].Columns.Add(kvPair.Key, kvPair.Value);
            }
        }

        public void removeTable(DataSets ds, string tableName)
        {
            m_dataSets[ds].Tables.Remove(tableName);
        }

        public void clearTable(DataSets ds, string tableName)
        {
            m_dataSets[ds].Tables[tableName].Clear();
        }

        public void addValues(DataSets ds, string tableName, Dictionary<string, object> values)
        {
            object[] dataRow = new object[values.Count];
            foreach(KeyValuePair<string, object> pair in values)
            {
                int index = m_dataSets[ds].Tables[tableName].Columns.IndexOf(pair.Key);
                dataRow[index] = pair.Value;
            }
            m_dataSets[ds].Tables[tableName].Rows.Add(dataRow);
        }

        public System.Collections.IEnumerator getData(DataSets ds, string tableName)
        {
            return m_dataSets[ds].Tables[tableName].Rows.GetEnumerator();
        }

        public void saveDataSet(DataSets ds)
        {
            m_dataSets[ds].WriteXml(PATH + m_savePaths[ds], XmlWriteMode.WriteSchema);
        }

        #endregion
    }
}
