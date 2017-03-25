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
                DataSetManager.Manager.addTable(DataSets.Config, testTableName, columns);
                DataSetManager.Manager.addTable(DataSets.Messages, testTableName, columns);

            }
            finally
            {
                DataSetManager.Manager.removeTable(DataSets.Config, testTableName);
                DataSetManager.Manager.removeTable(DataSets.Messages, testTableName);
            }
            try
            {
                DataSetManager.Manager.getData(DataSets.Config, testTableName);
                Assert.Fail();
            }
            catch (Exception) { }
            try
            {
                DataSetManager.Manager.getData(DataSets.Messages, testTableName);
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

                Dictionary<string, object> rowData;
                
                DataSetManager.Manager.addTable(DataSets.Config, testTableName, columns);
                DataSetManager.Manager.addTable(DataSets.Messages, testTableName, columns);
                const int TRIALS = 1000000;
                for (int i = 0; i < TRIALS; i++)
                {
                    rowData = new Dictionary<string, object>();
                    rowData["c1"] = i;
                    rowData["c2"] = "test1";
                    rowData["c3"] = 1000 * i;
                    rowData["c4"] = 0.01 * i;
                    DataSetManager.Manager.addValues(DataSets.Config, testTableName, rowData);

                    rowData = new Dictionary<string, object>();
                    rowData["c1"] = 2*i;
                    rowData["c2"] = "test2";
                    rowData["c3"] = 2000 * i;
                    rowData["c4"] = 0.02 * i;
                    DataSetManager.Manager.addValues(DataSets.Messages, testTableName, rowData);
                }
                rowData = new Dictionary<string, object>();
                rowData["c1"] = 2 * TRIALS;
                rowData["c2"] = "test2";
                rowData["c3"] = 2000 * TRIALS;
                rowData["c4"] = 0.02 * TRIALS;
                DataSetManager.Manager.addValues(DataSets.Messages, testTableName, rowData);

                IEnumerator ienum = DataSetManager.Manager.getData(DataSets.Config, testTableName);
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

                ienum = DataSetManager.Manager.getData(DataSets.Messages, testTableName);
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
                DataSetManager.Manager.removeTable(DataSets.Config, testTableName);
                DataSetManager.Manager.removeTable(DataSets.Messages, testTableName);
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

                DataSetManager.Manager.addTable(DataSets.Config, testTableName, columns);
                DataSetManager.Manager.addTable(DataSets.Messages, testTableName, columns);

                IEnumerator ienum = DataSetManager.Manager.getData(DataSets.Config, testTableName);
                int count = 0;
                while (ienum.MoveNext()) { count++; }
                Assert.AreEqual(0, count);

                ienum = DataSetManager.Manager.getData(DataSets.Messages, testTableName);
                count = 0;
                while (ienum.MoveNext()) { count++; }
                Assert.AreEqual(0, count);
            }
            finally
            {
                DataSetManager.Manager.removeTable(DataSets.Config, testTableName);
                DataSetManager.Manager.removeTable(DataSets.Messages, testTableName);
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
            try
            {
                // Set up preliminary values
                Dictionary<string, Type> columns = new Dictionary<string, Type>();
                columns.Add("c1", typeof(int));
                columns.Add("c2", typeof(string));
                columns.Add("c3", typeof(long));
                columns.Add("c4", typeof(double));

                Dictionary<string, object> rowData;

                DataSetManager.Manager.addTable(DataSets.Config, testTableName, columns, "c1");
                DataSetManager.Manager.addTable(DataSets.Messages, testTableName, columns, "c1");
                const int TRIALS = 1000000;
                for (int i = 0; i < TRIALS; i++)
                {
                    rowData = new Dictionary<string, object>();
                    rowData["c1"] = i;
                    rowData["c2"] = "test1";
                    rowData["c3"] = 1000 * i;
                    rowData["c4"] = 0.01 * i;
                    DataSetManager.Manager.addValues(DataSets.Config, testTableName, rowData);
                    DataSetManager.Manager.addValues(DataSets.Messages, testTableName, rowData);
                }

                // Check preliminary values before modifying
                IEnumerator ienum = DataSetManager.Manager.getData(DataSets.Config, testTableName);
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

                ienum = DataSetManager.Manager.getData(DataSets.Messages, testTableName);
                count = 0;
                while (ienum.MoveNext())
                {
                    Assert.AreEqual(count, ((System.Data.DataRow)ienum.Current)["c1"]);
                    Assert.AreEqual("test1", ((System.Data.DataRow)ienum.Current)["c2"]);
                    Assert.AreEqual((long)(1000) * count, ((System.Data.DataRow)ienum.Current)["c3"]);
                    Assert.AreEqual((double)(0.01 * count), (double)(((System.Data.DataRow)ienum.Current)["c4"]), 0.0001);
                    count++;
                }
                Assert.AreEqual(TRIALS, count);


                // Modify data
                for (int i = 0; i < TRIALS; i++)
                {
                    rowData = new Dictionary<string, object>();
                    rowData["c1"] = i;
                    rowData["c2"] = "test2";
                    rowData["c3"] = 2 * i;
                    rowData["c4"] = 0.02 * i;
                    Assert.IsFalse(DataSetManager.Manager.setValues(DataSets.Config, testTableName, rowData));
                    rowData["c1"] = i;
                    rowData["c2"] = "test3";
                    rowData["c3"] = 3 * i;
                    rowData["c4"] = 0.03 * i;
                    Assert.IsFalse(DataSetManager.Manager.setValues(DataSets.Messages, testTableName, rowData));
                }
                rowData = new Dictionary<string, object>();
                rowData["c1"] = TRIALS;
                rowData["c2"] = "test2";
                rowData["c3"] = 2 * TRIALS;
                rowData["c4"] = 0.02 * TRIALS;
                Assert.IsTrue(DataSetManager.Manager.setValues(DataSets.Config, testTableName, rowData));

                rowData = new Dictionary<string, object>();
                rowData["c1"] = TRIALS;
                rowData["c2"] = "test3";
                rowData["c3"] = 3 * TRIALS;
                rowData["c4"] = 0.03 * TRIALS;
                Assert.IsTrue(DataSetManager.Manager.setValues(DataSets.Messages, testTableName, rowData));

                // Check modified data
                ienum = DataSetManager.Manager.getData(DataSets.Config, testTableName);
                count = 0;
                while (ienum.MoveNext())
                {
                    Assert.AreEqual(count, ((System.Data.DataRow)ienum.Current)["c1"]);
                    Assert.AreEqual("test2", ((System.Data.DataRow)ienum.Current)["c2"]);
                    Assert.AreEqual((long)(2) * count, ((System.Data.DataRow)ienum.Current)["c3"]);
                    Assert.AreEqual((double)(0.02 * count), (double)(((System.Data.DataRow)ienum.Current)["c4"]), 0.0001);
                    count++;
                }
                Assert.AreEqual(TRIALS + 1, count);

                ienum = DataSetManager.Manager.getData(DataSets.Messages, testTableName);
                count = 0;
                while (ienum.MoveNext())
                {
                    Assert.AreEqual(count, ((System.Data.DataRow)ienum.Current)["c1"]);
                    Assert.AreEqual("test3", ((System.Data.DataRow)ienum.Current)["c2"]);
                    Assert.AreEqual((long)(3) * count, ((System.Data.DataRow)ienum.Current)["c3"]);
                    Assert.AreEqual((double)(0.03 * count), (double)(((System.Data.DataRow)ienum.Current)["c4"]), 0.0001);
                    count++;
                }
                Assert.AreEqual(TRIALS + 1, count);
            }
            finally
            {
                DataSetManager.Manager.removeTable(DataSets.Config, testTableName);
                DataSetManager.Manager.removeTable(DataSets.Messages, testTableName);
            }
        }
    }
}
