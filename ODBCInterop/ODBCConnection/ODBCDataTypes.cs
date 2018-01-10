using System.Collections.Generic;
namespace ODBCConnection
{
    public enum ODBCDataType : short
    {
        Char = 1,
        Numeric = 2,
        Decimal = 3,
        Integer = 4,
        Smallint = 5,
        Float = 6,
        Real = 7,
        Double = 8,
        DateTime = 9,
        Varchar = 12,
        TimeStamp = 11,
        Date = 91,
        Time = 92,
        Timestamp = 93

    }
    public enum NullableValue : short
    {
        Yes = 1,
        No = 2,
        Unknown = 3
    }
    // для возможности строкового возвращения типа данных
    public static class DataTypes
    {
        public static Dictionary<int, string> DataTypeByNumber
            = new Dictionary<int, string>
            { { 1, "Char" },
                { 2, "Numeric" },
                { 3, "Decimal" },
                {4,"Integer"},
                {5,"Smallint"},
                {6,"Float"},
                {7, "Real"},
                {8, "Double"},
                {9, "DateTime"},
                {10,"SybaseTime"},
                {11, "Timestamp"},
                {12, "Varchar"},
                {91, "Date"},
                {92, "Time"}, 
                {93, "Timestamp"},
                {-2,"SybaseBinary"},
                {-4,"SybaseLongBinary"},
                {-1, "SybaseLongVarChar"}
             };
        public static Dictionary<string, int> DataNumberByType
           = new Dictionary<string, int>
            { {  "Char",1 },
                {  "Numeric",2 },
                {  "Decimal",3 },
                {"Integer",4},
                {"Smallint",5},
                {"Float",6},
                {"Real",7},
                {"Double",8},
                { "DateTime",9},
                { "Varchar",12},
                { "Date",91},
                {"Time",92}, 
                {"Timestamp",93},
                {"Binary",-2},
                {"SybaseLongBinary",-4},
                {"SybaseLongVarChar",-1},
                {"SybaseTime",10}
              
            };
    }
}