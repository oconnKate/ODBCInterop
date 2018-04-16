using System;
using System.Collections.Generic;
namespace ODBCConnection
{
    //описание интересующих колонок в результирующем наборе
    public abstract class BaseDefinition
    {
        public short[] Indexes;
        public ODBCDataType[] Types;
        public int[] Lengths;
        public string[] Names;

        abstract public string GetListOfFields();
    }
    // для SQLColumns
    public class ColumnsStatementDefinition : BaseDefinition
    {
        public ColumnsStatementDefinition()
        {
            Indexes = new short[] { 4, 5, 7, 9, 18, 11 };
            Types = new ODBCDataType[] { ODBCDataType.Char, ODBCDataType.Integer, ODBCDataType.Integer, ODBCDataType.Integer, ODBCDataType.Char, ODBCDataType.Integer };
            Lengths = new int[] { 150, 4, 4, 4, 150, 4 };
        }
        public override string GetListOfFields()
        {
            return string.Format(" 0 - column name, 1 - data type, 2 - column size, 3 - decimal digits, 4 - is nullable ");
        }

    }
    // для SQLTables
    public class TablesStatementDefinition : BaseDefinition
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
    public class PrimaryKeyStatementDefinition : BaseDefinition
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
    public class IndexStatementDefinition : BaseDefinition
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
    //для SQLForeignKeys
    public class ForeignKeysStatementDefinition : BaseDefinition
    {
        public ForeignKeysStatementDefinition()
        {
            Indexes = new short[] { 1, 2, 3, 4, 5, 6,7,8,9,10,11,12,13 };
            Types = new ODBCDataType[] { ODBCDataType.Char, ODBCDataType.Char, ODBCDataType.Char, ODBCDataType.Char, ODBCDataType.Char, ODBCDataType.Char, ODBCDataType.Char, ODBCDataType.Char, ODBCDataType.Integer, ODBCDataType.Integer, ODBCDataType.Integer, ODBCDataType.Char, ODBCDataType.Char };
            Lengths = new int[] { 50,50, 50, 50, 50, 50,50 ,50,4,4,4,50,50};
        }
        public override string GetListOfFields()
        {
            return string.Format(" 0 -pk catalog, 1 - pk schema, 2 - pk table name, 3 - pk column name, 4 - fk catalog, 5 - fk schema, 6 - fk table name, 7 - fk column name, 8 - column number, 9 - update rule,10 - delete rule,11 - fk name, 12 - pk name ");
        }

    }
    //вспомогательный класс для сохранения данных, считанных из функций ODBC
    public class BufferData
    {
        internal int intVal;
        internal string strVal = String.Empty;
    }

    //класс с описанием структуры индекса(SQLStatistics)
    public class IndexData
    {
        private int _isUnique;//уникальный или нет
        private string _indexName;//имя индекса
        private List<ColumnData> _data;//список с именами колонок в индексе
        public bool IsUnique { get { return (_isUnique == 0); } }
        public string IndexName { get { return _indexName; } }
        public int Count { get { if (_data != null) { return _data.Count; } else return 0; } }
        public bool ContainsField(string Name, out ColumnData out_res)
        {
            out_res = _data.Find(T => { return T.ColumnName == Name; });
            return out_res != null;
        }
        public IEnumerable<ColumnData> NextColumn()
        {
            if (_data != null)
            {
                foreach (var record in _data)
                {
                    yield return record;
                }
            }
            else throw new ArgumentOutOfRangeException("Column number cannot be null");
        }

        public void SetData(int isUnique, string indexName)
        {
            _isUnique = isUnique;
            _indexName = indexName;
           
        }
        public void AddColumnData(ColumnData columnData)
        {
            if (_data == null) { _data = new List<ColumnData>(); }
            _data.Add(columnData);
        }
        public  IndexData(int isUnique, string indexName, List<ColumnData> columnData)
        {

            SetData(isUnique,indexName);
            _data = columnData;
        }
        public IndexData()
        {
            if (_data == null) { _data = new List<ColumnData>(); }
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
        private bool _isAsc;
        public string ColumnName { get { return _columnName; } }//имя колонки
        public int ColumnSequence { get { return _columnSequence; } }//последовательность в индексе
        public bool isAsc { get { return _isAsc; } }
        public ColumnData(string columnName, int columnSequence, bool isAscp)
        {
            _columnName = columnName;
            _columnSequence = columnSequence;
            _isAsc = isAscp;
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
        public NullableValue isNullable { get { switch (_isNullable) { case "YES":return NullableValue.Yes; case "NO": return NullableValue.No; default: return NullableValue.Unknown; } } }
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
        public int DataTypeIntVal { get { return _dataType; } }
        public int DataSize { get { return _dataSize; } }
        public int DecimalDigits { get { return _decimalDigits; } }

        public ColumnDescription(string columnName, int dataType, string isNullable, int dataSize = 0, int decimalDigits = 0, int isNullableInt = 1)
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
    public class ForeignKeyDescription
    {
        private string fk_name;
        private string pk_name;
        private List<string> pkColumns;
        private List<string> fkColumns;
        private int update_rule;
        private int delete_rule;
        private string RuleToStr(int _rule)
        {
            switch (_rule)
            {
                case ODBCNative.ODBCConstants.SQL_CASCADE: return "CASCADE";
                case ODBCNative.ODBCConstants.SQL_NO_ACTION: return "NO ACTION";
                case ODBCNative.ODBCConstants.SQL_SET_NULL: return "SET NULL";
                case ODBCNative.ODBCConstants.SQL_RESTRICT: return "RESTRICT";
                case ODBCNative.ODBCConstants.SQL_SET_DEFAULT: return "SET DEFAULT";
                default: throw new ArgumentException("unknown rule type/");

            }
        }
        public string UpdateRule()
        { return RuleToStr(update_rule); }
        public string DeleteRule()
        { return RuleToStr(delete_rule); }
        public int PKCount() { return pkColumns.Count; }
        public int FKCount() { return fkColumns.Count; }
        public IEnumerable<string> FKColumns()
        {
            foreach (var key in fkColumns)
            { yield return key; }
        }
        public IEnumerable<string> PKColumns()
        {
            foreach (var key in pkColumns)
            { yield return key; }
        }
        public ForeignKeyDescription(string _fk_name, string _pk_name, List<string> _pkColumns, List<string> _fkColumns, int _update_rule, int _delete_rule)
        { 
        fk_name = _fk_name;
            pk_name = _pk_name;
            pkColumns = _pkColumns;
            fkColumns =  _fkColumns;
            update_rule= _update_rule;
            delete_rule= _delete_rule;
                }
    }
    public class ODBCSources
    {
        private string _serverName;
        private string _description;
        public string ServerName { get { return _serverName; } }
        public string Description { get { return _description; } }
        public ODBCSources(string serverName, string description)
        {
            _serverName = serverName;
            _description = description;
        }
    }
}