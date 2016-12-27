using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Facebook_Message_Analyzer.Data;
using System.Collections;
using System.Collections.Generic;

namespace DataSetTester
{
    [TestClass]
    public class DataSetTester
    {
        private string testTableName = "asdf";

        [TestMethod]
        public void TestDSInitialization()
        {
            Dictionary<string, Type>  columns = new Dictionary<string, Type>();
            DataSetManager.Instance.addTable(DataSets.Config, testTableName, columns);
            DataSetManager.Instance.addTable(DataSets.Messages, testTableName, columns);


            DataSetManager.Instance.removeTable(DataSets.Config, testTableName);
            DataSetManager.Instance.removeTable(DataSets.Messages, testTableName);
        }

        [TestMethod]
        public void TestWritingAndReadingLargeData()
        {
            Dictionary<string, Type> columns = new Dictionary<string, Type>();
            //TODO: Add Columns

            DataSetManager.Instance.addTable(DataSets.Config, testTableName, columns);
            DataSetManager.Instance.addTable(DataSets.Messages, testTableName, columns);

            // TODO: Write several rows
            // TODO: Read rows and check w/ created data

            DataSetManager.Instance.removeTable(DataSets.Config, testTableName);
            DataSetManager.Instance.removeTable(DataSets.Messages, testTableName);
        }

        [TestMethod]
        public void TestReadingEmptyData()
        {
            Dictionary<string, Type> columns = new Dictionary<string, Type>();
            //TODO: Add Columns

            DataSetManager.Instance.addTable(DataSets.Config, testTableName, columns);
            DataSetManager.Instance.addTable(DataSets.Messages, testTableName, columns);

            // TODO: Read rows and check w/ created data

            DataSetManager.Instance.removeTable(DataSets.Config, testTableName);
            DataSetManager.Instance.removeTable(DataSets.Messages, testTableName);
        }

        [TestMethod]
        public void TestSavingData()
        {
            Dictionary<string, Type> columns = new Dictionary<string, Type>();
            //TODO: Add Columns

            DataSetManager.Instance.addTable(DataSets.Config, testTableName, columns);
            DataSetManager.Instance.addTable(DataSets.Messages, testTableName, columns);
            // TODO: Write several rows
            // TODO: Read rows and check w/ created data

            // TODO: Save Data
            // TODO: Stop and Check DataSets/Files


            DataSetManager.Instance.removeTable(DataSets.Config, testTableName);
            DataSetManager.Instance.removeTable(DataSets.Messages, testTableName);

            // TODO: Stop and Check DataSets/Files
        }
    }
}
