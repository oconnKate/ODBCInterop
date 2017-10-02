using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ODBCConnection;
namespace TestDB
{
    [TestClass]
    public class UnitTest1
    {
        private static List<string> tableList = new List<string>() {"TESTTABLE1","TESTTABLE2"};
        private static List<string> PK_TestTable1 = new List<string>() { "SECONDFIELD"};
        private static List<string> PK_TestTable2 = new List<string>() { "SECONDFIELD","FIRSTFIELD" };
       // private static List<ColumnDescription> Cols_TestTable1 = new List<ColumnDescription>() { {},{}};
        private static List<string> Cols_TestTable1 = new List<string>() { "SECONDFIELD", "FIRSTFIELD" };
        private static  string testConnection = "dsn=testDB;uid=SYSDBA; password=masterkey";
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
                    var records = connection.GetPrimaryKey("","TESTTABLE1");
                    var columnName = PK_TestTable1.Find(T => { return records.Find(TT => { return T == TT.ColumnName; }) != null; });
                    Assert.IsNotNull(columnName);
                     records = connection.GetPrimaryKey("", "TESTTABLE2");
                    columnName = PK_TestTable1.Find(T => { return records.Find(TT => { return T == TT.ColumnName; }) != null; });
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
                    var records = connection.GetColumns("", "TESTTABLE1");
                    var columnName = Cols_TestTable1.Find(T => { return records.Find(TT => { return T == TT.ColumnName; }) != null; });
                    Assert.IsNotNull(columnName);
                }
            }
        }
    }
}
