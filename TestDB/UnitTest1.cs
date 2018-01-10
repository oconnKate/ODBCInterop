using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ODBCConnection;
using ODBCCHandles;
using ODBCNative;
using System.Linq;


namespace TestDB
{
    [TestClass]
    public class UnitTest1
    {

        private static List<string> tableList = new List<string>() { "TESTTABLE1", "TESTTABLE2" };
        //необходим тестовый dsn firebird
        private static string testConnection = "dsn=testDB;uid=SYSDBA; password=masterkey";
        private static string sybcon = "dsn=fs1;uid=dba;pwd=sql";
        [TestMethod]
        public void TestTables()
        {
            using (var connection = new ODBCConnection.ODBCConnection())
            {
                connection.Initialize();
                if (connection.Connect(testConnection))
                {
                    var records = connection.GetTableList();
                    var tableName = tableList.Find(T => { return records.Find(TT => { return T == TT.TableName; }) != null; });
                    Assert.IsNotNull(tableName);
                }
                else Assert.Fail("could not connect to testDB");
            }

        }
        [TestMethod]
        public void TestPrimaryKeys()
        {
            using (var connection = new ODBCConnection.ODBCConnection())
            {
                connection.Initialize();
                if (connection.Connect(testConnection))
                {
                    TestTableData testData = TestTableData.GetDataSet1();
                    var records = connection.GetPrimaryKey("", testData.TestTableName);
                    var columnName = testData.ColumnsDescription.Find(T => { return records.Find(TT => { return T.ColumnName == TT.ColumnName; }) != null; });
                    Assert.IsNotNull(columnName);

                }
                else Assert.Fail("could not connect to testDB");
            }
        }
        [TestMethod]
        public void TestColumns()
        {
            using (var connection = new ODBCConnection.ODBCConnection())
            {
                connection.Initialize();
                if (connection.Connect(testConnection))
                {
                    TestTableData testData = TestTableData.GetDataSet1();
                    var records = connection.GetColumns("", testData.TestTableName);

                    var columnName = testData.ColumnsDescription.Find(T => { return records.Find(TT => { return T.ColumnName == TT.ColumnName; }) != null; });
                    Assert.IsNotNull(columnName);
                }
            }
        }
        [TestMethod]
        public void TestColumnsDefinition()
        {
            var testData = TestTableData.GetDataSet1();
            using (var connection = new ODBCConnection.ODBCConnection())
            {
                connection.Initialize();
                if (connection.Connect(testConnection))
                {
                    List<ColumnDescription> records = connection.GetColumns("", "TESTTABLE1");
                    foreach (var data in testData.ColumnsDescription)
                    {
                        var res = records.Single(ress => ress.ColumnName == data.ColumnName);
                        Assert.IsNotNull(res);
                        if (res != null)
                        { Assert.AreEqual(res.DataType, data.DataType); }
                    }
                }
            }
        }
        [TestMethod]
        public void TestIndexes()
        {
            ColumnData cd = new ColumnData("FIRSTFIELD", 1);
            ColumnData cd1 = new ColumnData("SECONDFIELD", 2);
            IndexData id = new IndexData();
            id.SetData(0, "TESTINDEX1", "A");
            id.AddColumnData(cd);
            id.AddColumnData(cd1);
            Dictionary<string, IndexData> Indexes = new Dictionary<string, IndexData>();
            Indexes.Add("TESTINDEX1", id);
            using (var connection = new ODBCConnection.ODBCConnection())
            {
                connection.Initialize();
                if (connection.Connect(testConnection))
                {
                    var testData = TestTableData.GetDataSet1();

                    var DBIndexes = connection.GetIndexData("", testData.TestTableName);
                    Assert.AreNotEqual(DBIndexes.Count, 0);
                    foreach (var record in testData.IndexesDescription)
                    {
                        IndexData out_res;

                        var res = DBIndexes.TryGetValue(record.Key, out out_res);
                        Assert.IsTrue(res);
                        if (res)
                        {
                            foreach (var column in record.Value.NextColumn())
                            {
                                ColumnData out_column;
                                var found = out_res.ContainsField(column.ColumnName, out out_column);

                                Assert.IsTrue(found);
                            }
                        }
                    }



                }
                else Assert.Fail("could not connect to testDB");
            }
        }

        [TestMethod]
        public void alltypes()
        {
            using (var connection = new ODBCConnection.ODBCConnection())
            {
                connection.Initialize();
                if (connection.Connect(sybcon))
                {
                    var cols = connection.GetColumns("dba","alltypes");
                    Assert.IsNotNull(cols,"ColumnsWrong");
                }
            }
        
        }
    }
}
