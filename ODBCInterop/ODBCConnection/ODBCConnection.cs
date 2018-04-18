using System;
using ODBCNative;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ODBCCHandles;
using ODBCErrors;
using System.Runtime.InteropServices;

namespace ODBCConnection

{
    public interface IODBCConnection
    {
        void Initialize(ODBCVersion version);
        bool Connect(string connectionString);
        List<TableData> GetTableList(string catalogName = "", string schemaName = "", string tableNamePattern = "");
        List<ColumnDescription> GetColumns(string schemaName, string tableName);
        IndexData GetPrimaryKey(string schemaName, string tableName);
        List<IndexData> GetIndexData(string schemaName, string tableName);
    }

    //класс для получения информации об источнике данных(таблицы, их структура, индексы и пр.)
    public class ODBCConnection : IDisposable, IODBCConnection
    {
        internal ODBCHConnection _connectionHandle;
        internal ODBCHEnvironment _enviromentHandle;
        private bool _initialized = false;
        private bool _connected = false;
        #region additional data
        private static List<ODBCData> BindBuffer(BaseDefinition baseDef)//привязка выделенной памяти к определенной  команде. В зависимости от команды результирующий набор содержит различное количество колонок различных типов
        {
            var resList = new List<ODBCData>();
            for (int i = 0; i < baseDef.Indexes.Count(); i++)
            {
                var col = new ODBCData(baseDef.Indexes[i], baseDef.Types[i], baseDef.Lengths[i]);
                col.ColumnIndex = i;
                resList.Add(col);
            }

            return resList;
        }

        private static bool BindColumns(List<ODBCData> columns, ODBCHStatement statement)//привязка выделенной памяти к определенным колонкам в результирующем наборе
        {
            bool res = true;
            foreach (var col in columns)
            {
                if (!ODBCMethods.SQLBindColumn(statement, col))
                {
                    res = false;
                    break;
                }
            }
            return res;
        }
        private static void FillInternalData(ref  List<ODBCData> data)// считываем данные из неуправляемой памяти в локальный буфер 
        {
            foreach (var locData in data)
            {
                if (locData.ColumnType == ODBCDataType.Char)
                {
                    locData.InternalData.strVal = ODBCMethods.ReadString(data[locData.ColumnIndex].ColumnData);
                    locData.InternalData.intVal = -1;
                }
                if (locData.ColumnType == ODBCDataType.Integer)
                {

                    locData.InternalData.intVal = ODBCMethods.ReadShort(data[locData.ColumnIndex].ColumnData);
                }
            }
        }
        #endregion
        #region free_resources
        private void Close()
        {

            if (_connected)
            {
                try
                {

                    ODBCNative.ODBCMethods.Disconnect(_connectionHandle);
                }
                catch (ODBCAPIError e )
                {
                    _connected = false;

#if DEBUG
                    System.Diagnostics.Trace.Write(e.Message);

#endif
                }

            }

            try
            {
                if (_connectionHandle != null) { _connectionHandle.Dispose(); }
                if (_enviromentHandle != null) { _enviromentHandle.Dispose(); }
            }
            catch
            {
                _initialized = false;
            }
            _connectionHandle = null;
            _enviromentHandle = null;
            _connected = false;
            _initialized = false;
        }
        public void Dispose()
        {
            Close();
            GC.SuppressFinalize(this);
        }
        ~ODBCConnection()
        {
            Close();
        }
        #endregion
        #region initialize
        public void Initialize(ODBCVersion version = ODBCVersion.Version2)//по умолчанию используем ODBC 2.0, т.к. работаем с устаревшими БД
        {
            Close();
            _enviromentHandle = new ODBCHEnvironment(version);
            _connectionHandle = new ODBCHConnection(_enviromentHandle);
            _initialized = true;

        }
        public bool Connect(string connectionString)
        {
            if (!_initialized) { Initialize(); }

            if (ODBCNative.ODBCMethods.ConnectTo(_connectionHandle, connectionString))
            {
                _connected = true;
                return true;
            }
            return false;
        }
        public void Disconnect()
        {
            if (_initialized)
            {
                ODBCNative.ODBCMethods.Disconnect(_connectionHandle);
            }
            else { throw new ODBCErrorNotInitialized("You must connect to database!"); }
        }
        #endregion
        #region data_source_information
        //получение списка таблиц
        public List<TableData> GetTableList(string catalogName = "", string schemaName = "", string tableNamePattern = "")
        {

            var _tablesData = new List<TableData>();

            if (_initialized)
            {
                List<ODBCData> data = null;

                using (ODBCHStatement statement = new ODBCHStatement(_connectionHandle))
                {

                    var tablesDefinition = new TablesStatementDefinition();
                    data = BindBuffer(tablesDefinition);

                    try
                    {

                        if (ODBCNative.ODBCMethods.GetTables(statement, catalogName, schemaName, tableNamePattern))
                        {
                            if (BindColumns(data, statement))
                            {
                                while (ODBCNative.ODBCMethods.Fetch(statement))
                                {

                                    FillInternalData(ref data);
                                    TableData table = new TableData(data[0].InternalData.strVal, data[1].InternalData.strVal, data[2].InternalData.strVal);
                                    _tablesData.Add(table);
                                }

                            }
                        }

                    }

                    finally
                    {
                        foreach (var record in data)
                        {

                            record.Dispose();
                        }
                        //  statement.Close();
                    }
                }
            }
            else { throw new ODBCErrorNotInitialized("You must connect to database!"); }
            return _tablesData;
        }
        //получение перечня полей и их типов для таблицы
        public List<ColumnDescription> GetColumns(string schemaName, string tableName)
        {
            var _primData = new List<ColumnDescription>();
            if (_initialized)
            {
                using (ODBCHStatement statement = new ODBCHStatement(_connectionHandle))
                {
                    List<ODBCData> data = null;
                    ColumnsStatementDefinition columnDefinition = new ColumnsStatementDefinition();
                    try
                    {
                        data = BindBuffer(columnDefinition);

                        if (ODBCNative.ODBCMethods.GetTableColumns(statement, schemaName, tableName))
                        {
                            if (BindColumns(data, statement))
                            {

                                while (ODBCNative.ODBCMethods.Fetch(statement))
                                {
                                    FillInternalData(ref data);


                                    //ODBC values only
                                    if (data[1].InternalData.intVal > 0)
                                    {
                                        var prim = new ColumnDescription(data[0].InternalData.strVal, data[1].InternalData.intVal, data[4].InternalData.strVal, data[2].InternalData.intVal, data[3].InternalData.intVal, data[5].InternalData.intVal);
                                        _primData.Add(prim);
                                    }

                                }


                            }
                        }

                    }

                    finally
                    {
                        foreach (var record in data)
                        {
                            record.Dispose();
                        }
                        //  statement.Close();
                    }
                }
                return _primData;
            }
            else { throw new ODBCErrorNotInitialized("You must connect to database!"); }
        }
        //получение полей первичного ключа
        public IndexData GetPrimaryKey(string schemaName, string tableName)
        {
            var _primData = new IndexData();
            if (_initialized)
            {
                using (ODBCHStatement statement = new ODBCHStatement(_connectionHandle))
                {
                    PrimaryKeyStatementDefinition primaryKeyDefinition = new PrimaryKeyStatementDefinition();
                    List<ODBCData> data = null;
                    try
                    {
                        data = BindBuffer(primaryKeyDefinition);
                        if (ODBCMethods.GetPrimKey(statement, schemaName, tableName))
                        {
                            if (BindColumns(data, statement))
                            {
                                while (ODBCMethods.Fetch(statement))
                                {
                                    FillInternalData(ref data);
                                    _primData.SetData(0,"");
                                    ColumnData _cd = new ColumnData(data[0].InternalData.strVal, data[1].InternalData.intVal, true);
                                    _primData.AddColumnData(_cd);
                                    //PrimaryKeyData prim = new PrimaryKeyData(data[0].InternalData.strVal, data[1].InternalData.intVal);
                                   // _primData.Add(prim);
                                }


                            }
                        }

                    }
                    finally
                    {
                        foreach (var record in data)
                        {

                            record.Dispose();
                        }
                        //  statement.Close();
                    }
                }
                return _primData;
            }
            else { throw new ODBCErrorNotInitialized("You must connect to database!"); }
        }
        //получение информации об индексах, полях, входящих в них
        public List<IndexData> GetIndexData(string schemaName, string tableName)
        {

            var _indexesData = new List<IndexData>();
            const ushort _indexType = 1; //all indexes;
            if (_initialized)
            {
                using (ODBCHStatement statement = new ODBCHStatement(_connectionHandle))
                {
                    List<ODBCData> data = null;
                    IndexStatementDefinition indexStatementDefinition = new IndexStatementDefinition();
                    try
                    {
                        data = BindBuffer(indexStatementDefinition);

                        if (ODBCNative.ODBCMethods.GetIndexInfo(statement, schemaName, tableName, _indexType))
                        {
                            if (BindColumns(data, statement))
                            {
                                bool fetched = ODBCNative.ODBCMethods.Fetch(statement);
                                while (fetched)
                                {
                                    FillInternalData(ref data);
                                    string indexName = data[1].InternalData.strVal;
                                    if (indexName.Length > 1)
                                    {
                                        IndexData _indexData = new IndexData();
                                        _indexData.SetData(data[0].InternalData.intVal, data[1].InternalData.strVal);

                                        while ((fetched) & (indexName.Equals(data[1].InternalData.strVal)))
                                        {
                                          
                                            int _unique = ODBCMethods.ReadShort(data[0].ColumnData);
                                            if ((_unique == ODBCNative.ODBCConstants.SQL_IS_INDEX_UNIQUE) | (_unique == ODBCNative.ODBCConstants.SQL_IS_INDEX_DUPLICATE))
                                            {
                                                bool asc = (data[4].InternalData.strVal == "A");
                                                ColumnData cd = new ColumnData(data[3].InternalData.strVal, data[2].InternalData.intVal, asc);
                                                _indexData.AddColumnData(cd);

                                            }
                                            fetched = ODBCNative.ODBCMethods.Fetch(statement);
                                           if (fetched) FillInternalData(ref data);
                                        }

                                        _indexesData.Add(_indexData);
                                    }
                                    else fetched = ODBCNative.ODBCMethods.Fetch(statement);
                                }
                            }

                        }
                    }

                    finally
                    {
                        foreach (var record in data)
                        {

                            record.Dispose();
                        }

                    }
                }
                return _indexesData;
            }
            else { throw new ODBCErrorNotInitialized("You must connect to database!"); }
        }
        //получение информации о внешних ключах
        public List<ForeignKeyDescription> GetForeingKeys(string pkCatalogName, string pkSchemaName, string pkTableName, string fkCatalogName, string fkSchemaName, string fkTableName)
        {
            var foreignKeyData = new List<ForeignKeyDescription>();
            if (_initialized)
            {
                using (ODBCHStatement statement = new ODBCHStatement(_connectionHandle))
                {
                    List<ODBCData> data = null;
                    ForeignKeysStatementDefinition foreignStatementDefinition = new ForeignKeysStatementDefinition();

                    try
                    {
                        data = BindBuffer(foreignStatementDefinition);
                        if (ODBCNative.ODBCMethods.GetForeignKeys(statement, pkCatalogName, pkSchemaName, pkTableName, fkCatalogName, fkSchemaName, fkTableName))
                        {
                            if (BindColumns(data, statement))
                            {
                                bool fetched = ODBCNative.ODBCMethods.Fetch(statement);

                               while (fetched)
                                {
                                    List<string> _fkColumns = new List<string>();
                                    List<string> _pkColumns = new List<string>();
                                    string _fkName = ODBCMethods.ReadString(data[11].ColumnData);
                                    string _pkName = ODBCMethods.ReadString(data[12].ColumnData);
                                    short _updateRule = ODBCMethods.ReadShort(data[9].ColumnData);
                                    short _deleteRule = ODBCMethods.ReadShort(data[10].ColumnData);

                                    while ((fetched) & (_fkName == ODBCMethods.ReadString(data[11].ColumnData)))
                                    {
                                        string _pkcolumnName = ODBCMethods.ReadString(data[3].ColumnData);
                                        string _fkcolumnName = ODBCMethods.ReadString(data[7].ColumnData);
                                        _fkColumns.Add(_fkcolumnName);
                                        _pkColumns.Add(_pkcolumnName);
                                        fetched = ODBCNative.ODBCMethods.Fetch(statement);
                                    }
                                    ForeignKeyDescription _fkd = new ForeignKeyDescription(_fkName, _pkName, _pkColumns, _fkColumns, _updateRule, _deleteRule);
                                    foreignKeyData.Add(_fkd);

                                }
                            }



                        }
                    }
                    finally
                    {
                        foreach (var record in data)
                        {
                            record.Dispose();
                        }
                    }
                }
            }
            return foreignKeyData;
        }
        public bool ExecDirect(string sqlStatement)
        {
            if (_initialized)
            {
                using (ODBCHStatement statement = new ODBCHStatement(_connectionHandle))
                {
                    string out_info;
                    bool result = ODBCNative.ODBCMethods.ExecDirect(statement, sqlStatement, sqlStatement.Length, out out_info);
                    return result;
                }

            }
            return false;
        }
        public bool SetConnectionProperty(ODBCHConnection nStatementHandle, short prop, int val)
        {
            if (_initialized)
            {
                return ODBCNative.ODBCMethods.SetConnectionProp(nStatementHandle, prop, val);
            }
            return false;
        }
        //оболочка для вызова SQLGetInfo, получение строковой информации об источнике данных   
        public string GetStringInfo(ushort type)
        {
            if (_connected)
            {
                StringBuilder catalog = new StringBuilder();
                ODBCNative.ODBCMethods.GetConnectionInfoString(_connectionHandle, type, out  catalog);
                return catalog.ToString();
            }
            else { throw new ODBCErrorNotInitialized("You must connect to database!"); }
        }
        //оболочка для вызова SQLGetInfo, получение численной информации об источнике данных   
        public ushort GetIntInfo(ushort type)
        {
            if (_connected)
            {
                ushort res = 0;
                ODBCNative.ODBCMethods.GetConnectionInfoShort(_connectionHandle, type, out res);
                return res;
            }
            else { throw new ODBCErrorNotInitialized("You must connect to database!"); }
        }
        //получение информации об источниках ODBC
        public List<ODBCSources> GetODBCSources()
        {
            var list = new List<ODBCSources>();
            if (!_initialized)
            {
                Initialize(ODBCVersion.Version2);
            }
            StringBuilder serverName = new StringBuilder();
            StringBuilder description = new StringBuilder();
            var result = ODBCMethods.GetDataSources(_enviromentHandle, ODBCConstants.SQL_FETCH_FIRST, out serverName, out description);
            if ((result != ODBCResult.Error) && (result != ODBCResult.InvalidHandle))
            {
                while (result != ODBCResult.NoData)
                {
                    var source = new ODBCSources(serverName.ToString(), description.ToString());
                    list.Add(source);
                    result = ODBCMethods.GetDataSources(_enviromentHandle, ODBCConstants.SQL_FETCH_NEXT, out serverName, out description);
                }
            }
            return list;
        }
        #endregion
    }
}