using System;
using System.Collections.Generic;
namespace ODBCConnection
{
    //описание интересующих колонок в результирующем наборе
    internal abstract class BaseDefinition
    {
        public short[] Indexes;
        public ODBCDataType[] Types;
        public int[] Lengths;
        public string[] Names;

        abstract public string GetListOfFields();
    }
    // для SQLColumns
    internal class ColumnsStatementDefinition : BaseDefinition
    {
        public ColumnsStatementDefinition()
        {
            Indexes = new short[] { 4, 5, 7, 9, 18,11 };
            Types = new ODBCDataType[] { ODBCDataType.Char, ODBCDataType.Integer, ODBCDataType.Integer, ODBCDataType.Integer, ODBCDataType.Char, ODBCDataType.Integer };
            Lengths = new int[] { 150, 4, 4, 4, 150,4 };
        }
        public override string GetListOfFields()
        {
            return string.Format(" 0 - column name, 1 - data type, 2 - column size, 3 - decimal digits, 4 - is nullable ");
        }

    }
    // для SQLTables
    internal class TablesStatementDefinition : BaseDefinition
    {
        public TablesStatementDefinition()
        {
            Indexes = new short[] { 2, 3, 4 };
            Types = new ODBCDataType[] { ODBCDataType.Char, ODBCDataType.Char, ODBCDataType.Char };
            Lengths = new int[] { 80, 80, 80 };
        }
        public override string GetListOfFields()
        {
            return string.Format(" 0 - schema, 1 - table name, 2 - table type");
        }

    }
    // для SQLPrimaryKeys
    internal class PrimaryKeyStatementDefinition : BaseDefinition
    {
        public PrimaryKeyStatementDefinition()
        {
            Indexes = new short[] { 4, 5 };
            Types = new ODBCDataType[] { ODBCDataType.Char, ODBCDataType.Integer };
            Lengths = new int[] { 80, 4 };
        }
        public override string GetListOfFields()
        {
            return string.Format(" 0 - column name, 1 - column sequence");
        }
    }
    // для SQLStatisctics
    internal class IndexStatementDefinition : BaseDefinition
    {
        public IndexStatementDefinition()
        {
            Indexes = new short[] { 4, 6, 8, 9, 10 };
            Types = new ODBCDataType[] { ODBCDataType.Integer, ODBCDataType.Char, ODBCDataType.Integer, ODBCDataType.Char, ODBCDataType.Char };
            Lengths = new int[] { 4, 80, 4, 80, 4 };
            Names = new string[] { "isUnique", "indexName", "orderNumber", "ColumnName", "sortOrder" };
        }
        public override string GetListOfFields()
        {
            return string.Format(" 0 - is index unique, 1 - index name,  2 - column order in index, 3 - column name, 4 - asc or desc");
        }
    }
    //вспомогательный класс для сохранения данных, считанных из функций ODBC
    internal class BufferData
    {
        internal int intVal;
        internal string strVal = String.Empty;
    }

    //класс с описанием структуры индекса(SQLStatistics)
    public class IndexData
    {
        private int _isUnique;//уникальный или нет
        private string _indexName;//имя индекса
        private string _isAsc;//строки идут по возрастанию
        private List<ColumnData> _data;//список с именами колонок в индексе
        public bool IsUnique { get { return (_isUnique == 0); } }
        public string IndexName { get { return _indexName; } }
        public bool IsAsc { get { return _isAsc[0] == 'A'; } }
        public int ColumnCount { get { return _data.Count; } }
        internal Dictionary<string, BufferData> Values = new Dictionary<string, BufferData>();
        public IEnumerable<ColumnData> NextColumn()
        {
            foreach (var record in _data)
            {
                yield return record;
            }

        }

        internal void SetData(int isUnique, string indexName, string isAsc)
        {
            _isUnique = isUnique;
            _indexName = indexName;
            _isAsc = isAsc;
        }
        internal void AddColumnData(ColumnData columnData)
        {
            if (_data == null) { _data = new List<ColumnData>(); }
            _data.Add(columnData);
        }

    }
    // ообщая информация о таблице
    public class TableData
    {
        private string _tableName;
        private string _tableType;
        private string _tableSchema;
        public string TableName { get { return _tableName; } }
        public string TableType { get { return _tableType; } }
        public string TableSchema { get { return _tableSchema; } }
        public TableData(string tableSchema, string tableName, string tableType)
        {
            _tableName = tableName;
            _tableType = tableType;
            _tableSchema = tableSchema;
        }

    }
    public class PrimaryKeyData
    {
        private string _columnName;
        private int _columnSequence;
        public string ColumnName { get { return _columnName; } }
        public int ColumnSequence { get { return _columnSequence; } }//starting with 1

        public PrimaryKeyData(string columnName, int columnSequence)
        {
            _columnName = columnName;
            _columnSequence = columnSequence;

        }

    }

    public class ColumnData
    {
        private string _columnName;
        private int _columnSequence;
        public string ColumnName {  get{return _columnName;} }//имя колонки
        public int ColumnSequence {  get { return _columnSequence; }  }//последовательность в индексе
        public ColumnData(string columnName, int columnSequence)
        {
            _columnName = columnName;
            _columnSequence = columnSequence;
        }
    }
    public class ColumnDescription
    {
        private int _dataType;
        private int _dataSize;
        private int _decimalDigits;
        private string _isNullable;
        private string _columnName;
        private int _isNullableInt;
        public string ColumnName { get { return _columnName; } }
        public NullableValue isNullable { get { switch (_isNullable) { case "YES":return NullableValue.Yes;  case "NO": return NullableValue.No; default: return NullableValue.Unknown; } } }
        public string isNullableStr { get { return _isNullable; } }
        public int isNullableInt { get { return _isNullableInt; } set { _isNullableInt = value; } }
        public string DataType
        {
            get
            {
                string value = string.Empty;
                if (!DataTypes.DataTypeByNumber.TryGetValue(_dataType, out value))
                { throw new Exception("Unknown data type int value - " + _dataType.ToString()); }
                return value;
            }
        }
        public int DataSize { get { return _dataSize; } }
        public int DecimalDigits { get { return _decimalDigits; } }
        internal ColumnDescription(string columnName, int dataType, string isNullable, int dataSize = 0, int decimalDigits = 0, int isNullableInt=1)
        {
            string value = string.Empty;
            if (!DataTypes.DataTypeByNumber.TryGetValue(dataType, out value))
            { throw new Exception("Unknown data type int value - " + _dataType.ToString()); }
           _columnName = columnName;
            _dataType = dataType;
            _isNullable = isNullable;
            _dataSize = dataSize;
            _decimalDigits = decimalDigits;
            _isNullableInt = isNullableInt;
        }

    }
    public class ODBCSources
    { 
    private string _serverName;
    private string _description;
    public string ServerName { get { return _serverName; } }
    public string Description { get { return _description; } }
        public  ODBCSources(string serverName, string description)
        {
            _serverName = serverName;
            _description = description;
        }
    }
}