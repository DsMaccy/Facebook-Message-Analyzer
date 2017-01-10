using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Facebook_Message_Analyzer.Data;
using System.Collections;
using System.Collections.Generic;
using System.Data;


namespace DataSetTester
{
    [TestClass]
    public class DataSetTester
    {
        private string testTableName = "asdf";

        [TestMethod]
        public void TestDSInitialization()
        {
            try
            {
                Dictionary<string, Type> columns = new Dictionary<string, Type>();
                DataSetManager.Instance.addTable(DataSets.Config, testTableName, columns);
                DataSetManager.Instance.addTable(DataSets.Messages, testTableName, columns);

            }
            finally
            {
                DataSetManager.Instance.removeTable(DataSets.Config, testTableName);
                DataSetManager.Instance.removeTable(DataSets.Messages, testTableName);
            }
            try
            {
                DataSetManager.Instance.getData(DataSets.Config, testTableName);
                Assert.Fail();
            }
            catch (Exception) { }
            try
            {
                DataSetManager.Instance.getData(DataSets.Messages, testTableName);
                Assert.Fail();
            }
            catch (Exception) { }
        }

        [TestMethod]
        public void TestWritingAndReadingLargeData()
        {
            try
            {
                Dictionary<string, Type> columns = new Dictionary<string, Type>();
                columns.Add("c1", typeof(int));
                columns.Add("c2", typeof(string));
                columns.Add("c3", typeof(long));
                columns.Add("c4", typeof(double));

                DataSetManager.Instance.addTable(DataSets.Config, testTableName, columns);
                DataSetManager.Instance.addTable(DataSets.Messages, testTableName, columns);
                const int TRIALS = 1000000;
                for (int i = 0; i < TRIALS; i++)
                {
                    DataSetManager.Instance.addRow(DataSets.Config, testTableName, i, "test1", 1000 * i, 0.01 * i);
                    DataSetManager.Instance.addRow(DataSets.Messages, testTableName, 2 * i, "test2", 2000 * i, 0.02 * i);
                }
                DataSetManager.Instance.addRow(DataSets.Messages, testTableName, 2 * TRIALS, "test2", 2000 * TRIALS, 0.02 * TRIALS);

                IEnumerator ienum = DataSetManager.Instance.getData(DataSets.Config, testTableName);
                int count = 0;
                while (ienum.MoveNext())
                {
                    Assert.AreEqual(count, ((System.Data.DataRow)ienum.Current)["c1"]);
                    Assert.AreEqual("test1", ((System.Data.DataRow)ienum.Current)["c2"]);
                    Assert.AreEqual((long)(1000) * count, ((System.Data.DataRow)ienum.Current)["c3"]);
                    Assert.AreEqual((double)(0.01 * count), (double)(((System.Data.DataRow)ienum.Current)["c4"]), 0.0001);
                    count++;
                }
                Assert.AreEqual(TRIALS, count);

                ienum = DataSetManager.Instance.getData(DataSets.Messages, testTableName);
                count = 0;
                while (ienum.MoveNext())
                {
                    Assert.AreEqual(2 * count, ((System.Data.DataRow)ienum.Current)["c1"]);
                    Assert.AreEqual("test2", ((System.Data.DataRow)ienum.Current)["c2"]);
                    Assert.AreEqual((long)(2000) * count, ((System.Data.DataRow)ienum.Current)["c3"]);
                    Assert.AreEqual((double)(0.02 * count), (double)(((System.Data.DataRow)ienum.Current)["c4"]), 0.0001);
                    count++;
                }
                Assert.AreEqual(TRIALS + 1, count);
            }
            finally
            {
                DataSetManager.Instance.removeTable(DataSets.Config, testTableName);
                DataSetManager.Instance.removeTable(DataSets.Messages, testTableName);
            }
        }

        [TestMethod]
        public void TestReadingEmptyData()
        {
            try
            {
                Dictionary<string, Type> columns = new Dictionary<string, Type>();
                columns.Add("c1", typeof(int));
                columns.Add("c2", typeof(string));
                columns.Add("c3", typeof(long));
                columns.Add("c4", typeof(float));

                DataSetManager.Instance.addTable(DataSets.Config, testTableName, columns);
                DataSetManager.Instance.addTable(DataSets.Messages, testTableName, columns);

                IEnumerator ienum = DataSetManager.Instance.getData(DataSets.Config, testTableName);
                int count = 0;
                while (ienum.MoveNext()) { count++; }
                Assert.AreEqual(0, count);

                ienum = DataSetManager.Instance.getData(DataSets.Messages, testTableName);
                count = 0;
                while (ienum.MoveNext()) { count++; }
                Assert.AreEqual(0, count);
            }
            finally
            {
                DataSetManager.Instance.removeTable(DataSets.Config, testTableName);
                DataSetManager.Instance.removeTable(DataSets.Messages, testTableName);
            }
        }

        [TestMethod]
        public void TestRemovingData()
        {
            Assert.Fail("Test Not Yet Written");
        }

        [TestMethod]
        public void TestModifyingData()
        {
            Assert.Fail("Test Not Yet Written");
        }
    }
}
