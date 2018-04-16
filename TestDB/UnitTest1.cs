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


        [TestMethod]
        public void TestTables()
        {
            using (var connection = new ODBCConnection.ODBCConnection())
            {
                connection.Initialize();
                if (connection.Connect(TestTableData1.testConnection))
                {
                    var records = connection.GetTableList();
                    foreach (var table in TestTableData1.tableList)
                    {
                        var tableName = records.Find(T => { return T.TableName == table; });
                        Assert.IsNotNull(tableName);
                    }
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
                if (connection.Connect(TestTableData1.testConnection))
                {

                    foreach (var table in TestData.testData)
                    {
                        var records = connection.GetPrimaryKey("", table.TableName);
                        foreach (var column in records.NextColumn())
                        {
                            var columnName = table.PrimaryKey.NextColumn().First(T => { return T.ColumnName == column.ColumnName; });
                            Assert.IsNotNull(columnName);
                        }

                    }


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
                if (connection.Connect(TestTableData1.testConnection))
                {
                    foreach (var table in TestData.testData)
                    {
                        var records = connection.GetColumns("", table.TableName);

                        var columnName = table.ColumnsDescription.Find(T => { return records.Find(TT => { return T.ColumnName == TT.ColumnName; }) != null; });
                        Assert.IsNotNull(columnName);
                    }
                }
            }
        }

        [TestMethod]
        public void TestIndexes()
        {

            using (var connection = new ODBCConnection.ODBCConnection())
            {
                connection.Initialize();
                if (connection.Connect(TestTableData1.testConnection))
                {

                    foreach (var table in TestData.testData)
                    {
                        var DBIndexes = connection.GetIndexData("", table.TableName);
                        Assert.AreNotEqual(DBIndexes.Count, 0);
                        foreach (var record in table.IndexData)
                        {

                            var found = DBIndexes.Find(T => { return T.IndexName == record.IndexName; });

                            Assert.IsNotNull(found);
                            foreach (var column in found.NextColumn())
                            {
                                var dest = record.NextColumn().First(T => { return T.ColumnName == column.ColumnName; });
                                Assert.AreEqual(column.ColumnName, dest.ColumnName);
                            }



                        }



                    }

                }
                else Assert.Fail("could not connect to testDB");
            }
        }
    }
}
        
