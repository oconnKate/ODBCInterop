using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ODBCConnection;

namespace TestDB
{
    class TestTableData
    {
        public Dictionary<string, IndexData> IndexesDescription;
        public List<PrimaryKeyData> PrimaryKeyDescription;
        public List<ColumnDescription> ColumnsDescription;
        public string TestTableName;
        public static TestTableData GetDataSet1()
        {
            TestTableData result = new TestTableData();
            result.TestTableName = "TESTTABLE1";
            result.IndexesDescription = new Dictionary<string, IndexData>();
            var idata = new IndexData();
            idata.SetData(0, "TESTINDEX1", "A");
            idata.AddColumnData(new ColumnData("SECONDFIELD", 1));
            idata.AddColumnData(new ColumnData("FIRSTFIELD", 2));
            result.IndexesDescription.Add("TESTINDEX1", idata);
            result.PrimaryKeyDescription = new List<PrimaryKeyData>() { new PrimaryKeyData("SECONDFIELD", 1) };
            result.ColumnsDescription = new List<ColumnDescription>()
            {
                new ColumnDescription("FIRSTFIELD",4,"N",0,0,0),
                new ColumnDescription("SECONDFIELD",1,"N",20,0,0),
                new ColumnDescription("THIRDFIELD",93,"N",0,0,1),
                new ColumnDescription("FOURTHFIELD",12,"N",10,0,1)
            };
            return result;
        }
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


    }
}
