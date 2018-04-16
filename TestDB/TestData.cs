using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ODBCConnection;

namespace TestDB
{
  public  class  TestHelper
    {
        public static readonly List<string> tableList = new List<string>() { "TESTTABLE1", "TESTTABLE2" };
        public static readonly string testConnection = "dsn=testDB;uid=SYSDBA; password=masterkey"; 
        public readonly string  TableName ;
        public readonly List<IndexData> IndexData;
        public readonly IndexData PrimaryKey;
        public readonly List<ColumnDescription> ColumnsDescription;
        public static bool IsSameColumnDescription(ColumnDescription descr1, ColumnDescription descr2)
        
    {
            bool result = true;
            result = descr1.ColumnName == descr2.ColumnName;
            result = descr1.DataType == descr2.DataType;
            result = descr1.DataSize == descr2.DataSize;
            result = descr1.DecimalDigits == descr2.DecimalDigits;
            result = descr1.isNullableInt == descr2.isNullableInt;
            return result;
        }
        public TestHelper(string _tableName, List<IndexData> _indexes, IndexData _pk, List<ColumnDescription> _columns)
        {
            TableName = _tableName;
            IndexData = _indexes;
            PrimaryKey = _pk;
            ColumnsDescription = _columns;
        }
    }

  public class TestTableData1:TestHelper
    {
       public TestTableData1()
           : base(tableName, allIndexes, primKey, columnsDescription)
       {
           
       }
       
        private  static readonly string tableName = "TESTTABLE1";
        private static List<ColumnData> primColumnData = new List<ColumnData>()
        {
       
        new  ColumnData("SECONDFIELD",1,true)
        };
        private static List<ColumnData> index1ColumnData = new List<ColumnData>()
        {
             new  ColumnData("FIRSTFIELD",1,true),
       
        new  ColumnData("SECONDFIELD",2,true)
        };
        private static List<ColumnData> index2ColumnData = new List<ColumnData>()
        {
             new  ColumnData("FIRSTFIELD",1,true),
               new  ColumnData("SECONDFIELD",2,true),
               new  ColumnData("THIRDFIELD",3,true),
               new  ColumnData("FOURTHFIELD",4,true)
        };
        private static IndexData primKey = new IndexData(0, "", primColumnData);
        private static IndexData index1 = new IndexData(0, "TESTINDEX1", index1ColumnData);
        private static IndexData index2 = new IndexData(1, "TESTTABLE1_IDX", index2ColumnData);
        private static List<IndexData> allIndexes = new List<IndexData>()
        {
        index1,index2
        };
        private static List<ColumnDescription> columnsDescription = new List<ColumnDescription>()
            {
                new ColumnDescription("FIRSTFIELD",4,"N",0,0,0),
                new ColumnDescription("SECONDFIELD",1,"N",20,0,0),
                new ColumnDescription("THIRDFIELD",93,"N",0,0,1),
                new ColumnDescription("FOURTHFIELD",12,"N",10,0,1)
            };
     
       
       

    }
  public class TestTableData2 : TestHelper
    {
        public TestTableData2()
           : base(tableName, allIndexes, primKey, columnsDescription)
       {
           
       }
        private static string tableName = "TESTTABLE2";
        private static List<ColumnData> primColumnData = new List<ColumnData>()
        {
       
        new  ColumnData("FIRSTFIELD",1,true),
        new  ColumnData("SECONDFIELD",2,true)
        };
        private static List<ColumnData> index1ColumnData = new List<ColumnData>()
        {
             new  ColumnData("THIRDFIELD",1,true)
       
        
        };
        private static List<ColumnData> index2ColumnData = new List<ColumnData>()
        {
             new  ColumnData("FIRSTFIELD",1,true),
               new  ColumnData("SECONDFIELD",2,true),
               new  ColumnData("THIRDFIELD",3,true)
             
        };
        private static IndexData primKey = new IndexData(0, "", primColumnData);
        private static IndexData index1 = new IndexData(0, "TESTINDEX2", index1ColumnData);
        private static IndexData index2 = new IndexData(1, "TESTTABLE2_IDX", index2ColumnData);
        private static List<IndexData> allIndexes = new List<IndexData>()
        {
        index1,index2
        };
        private static List<ColumnDescription> columnsDescription = new List<ColumnDescription>()
            {
                new ColumnDescription("FIRSTFIELD",93,"N",0,0,0),
                new ColumnDescription("SECONDFIELD",1,"N",20,0,0),
                new ColumnDescription("THIRDFIELD",4,"N",0,0,1)
               
            };





    }
   public static class TestData
   {
       public static List<TestHelper> testData = new List<TestHelper>(){new TestTableData1(), new TestTableData2()};
   }
}
