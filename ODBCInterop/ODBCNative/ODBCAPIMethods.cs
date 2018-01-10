using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Linq;
using ODBCCHandles;
using ODBCErrors;
using ODBCConnection;
namespace ODBCNative
{
    internal static class ODBCMethods
    {


        private static class ODBCNativeMethods
        {
            #region external_methods


            [DllImport("odbc32.dll", SetLastError = false)]
            internal static extern ODBCResult SQLAllocHandle(
                 ODBCHType handleType,
                IntPtr inputHandle,
                out IntPtr outputHandle);

            [DllImport("odbc32.dll", SetLastError = false)]
            internal static extern ODBCResult SQLAllocHandle(
                ODBCHType handleType,
                ODBCHEnvironment inputHandle,
                out IntPtr outputHandle);

            [DllImport("odbc32.dll", SetLastError = false)]
            internal static extern ODBCResult SQLAllocHandle(
                ODBCHType handleType,
                ODBCHConnection inputHandle,
                out IntPtr outputHandle);


            [DllImport("odbc32.dll", SetLastError = false)]
            internal static extern ODBCResult SQLDisconnect(
                ODBCHConnection connectionHandle);


            [DllImport("odbc32.dll", CharSet = CharSet.Unicode, SetLastError = false)]
            internal static extern ODBCResult SQLDriverConnectW(
                 ODBCHConnection connectionHandle,
                 IntPtr windowHandle,
                 string inConnectionString,
                 short inConnectionStringLength,
                 StringBuilder outConnectionStringBuffer,
                 short bufferLength,
                 out short bufferLengthNeeded,
                 short fDriverCompletion);
            

            [DllImport("odbc32.dll", CharSet = CharSet.Unicode, SetLastError = false)]
            internal static extern ODBCResult SQLGetDiagRecW(
                ODBCHType handleType,
                ODBCHBase handle,
                short recordNumber,
                StringBuilder stateBuffer,
                out int nativeError,
                StringBuilder messageBuffer,
                short bufferLength,
                out short bufferLengthNeeded);


            [DllImport("odbc32.dll", SetLastError = false)]
            internal static extern ODBCResult SQLFreeHandle(
                ODBCHType handleType,
                IntPtr handle);

            //дополнительные параметры окружения - в текущем проекте ставим версию ODBC драйвера
            [DllImport("odbc32.dll", SetLastError = false)]
            internal static extern ODBCResult SQLSetEnvAttr(
                ODBCHEnvironment environmentHandle,
                ODBCEnvAttr attribute,
                ODBCVersion value,
                int valueLengthShouldBeZero);

            //привязка выделенных под данные буферов к результирующему набору
            [DllImport("odbc32.dll", SetLastError = false)]
            public static extern ODBCResult SQLBindCol(
                ODBCHStatement StatementHandle,
                short ColumnNumber,
                ODBCDataType TargetType,
                 IntPtr TargetValue,
                int BufferLength,
                out int StrLen_or_ind);
            //привязка выделенных под данные буферов к результирующему набору
            [DllImport("odbc32.dll", SetLastError = false)]
            public static extern ODBCResult SQLBindCol(
                ODBCHStatement StatementHandle,
                short ColumnNumber,
                ODBCDataType TargetType,
                 StringBuilder TargetValue,
                int BufferLength,
                out int StrLen_or_ind);

            // перечень колонок в таблице
            [DllImport("odbc32.dll", SetLastError = false)]
            public static extern ODBCResult SQLColumns(
           ODBCHStatement StatementHandle,
           string CatalogName,
           short NameLength1,
           string SchemaName,
           short NameLength2,
           string TableName,
           short NameLength3,
           string ColumnName,
           short NameLength4);

            //извлечение строки результирующего набора
            [DllImport("odbc32.dll", SetLastError = false)]
            public static extern ODBCResult SQLFetch(
                ODBCHStatement StatementHandle);

            //перечень таблиц
            [DllImport("odbc32.dll", SetLastError = false)]
            public static extern ODBCResult SQLTables(
                ODBCHStatement StatementHandle,
                string CatalogName,
                short NameLength1,
                string SchemaName,
                short NameLength2,
                string TableName,
                short NameLength3,
                string TableType,
                short NameLength4);

            //первичный ключ
            [DllImport("odbc32.dll", SetLastError = false)]
            public static extern ODBCResult SQLPrimaryKeys(
                ODBCHStatement StatementHandle,
                string CatalogName,
                short NameLength1,
                string SchemaName,
                short NameLength2,
                string TableName,
                short NameLength3);

            //информация об индексах
            [DllImport("odbc32.dll", SetLastError = false)]
            public static extern ODBCResult SQLStatistics(
                ODBCHStatement StatementHandle,
                string CatalogName,
                short NameLength1,
                string SchemaName,
                short NameLength2,
                string TableName,
                short NameLength3,
                ushort Unique,
                ushort Reserved);

            [DllImport("odbc32.dll", SetLastError = false)]
            //получение информации об источнике данных
            public static extern ODBCResult SQLGetInfo(
                 ODBCHConnection connectionHandle,
               ushort InfoType,
               IntPtr InfoValue,
               short BufferLength,
               out short StringLengthPtr);

            [DllImport("odbc32.dll", SetLastError = false)]
            //информация о драйвере и об источнике данных выбранного соединения
            //прототип для для строковой информации
            public static extern ODBCResult SQLGetInfo(
                ODBCHConnection connectionHandle,
              ushort InfoType,
              StringBuilder InfoValue,
              short BufferLength,
              out short StringLengthPtr);

            [DllImport("odbc32.dll", SetLastError = false)]
            //информация о драйвере и об источнике данных выбранного соединения
            //прототип для для целочисленной информации
            public static extern ODBCResult SQLGetInfo(
                ODBCHConnection connectionHandle,
              ushort InfoType,
              out  ushort InfoValue,
              short BufferLength,
              out short StringLengthPtr);

            [DllImport("odbc32.dll", SetLastError = false)]
            // перечень источников данных
            public static extern ODBCResult SQLDataSources(
                ODBCHEnvironment enviromentHandle,
                ushort Direction,
                StringBuilder ServerName,
                short BufferLength1,
                out short NameLength1Ptr,
                StringBuilder Description,
                short BufferLength2,
                out short NameLength2Ptr);

            [DllImport("odbc32.dll", SetLastError = false)]
            //выполнение sql-команды
            public static extern ODBCResult SQLExecDirectW(
                ODBCHStatement statementHandle,
          string statementText,
            int textLength
                );

            [DllImport("odbc32.dll", SetLastError = false)]
            //установка атрибутов соединения
            //целочисленное значение атрибута
            public static extern ODBCResult SQLSetConnectAttr(
              ODBCHConnection nStatementHandle,
               int Attribute,
               int ValuePtr,
               int eExpression
                );

            [DllImport("odbc32.dll", SetLastError = false)]
            //установка атрибутов соединения
            //строковое значение атрибута
            public static extern ODBCResult SQLSetConnectAttr(
              ODBCHConnection nStatementHandle,
               int Attribute,
               string ValuePtr,
               int eExpression
                );
            #endregion
        }

        #region internal_methods
        //оболочки для вызова внешних функций 
        internal static string ReadString(IntPtr pointer)
        {
            if (pointer == IntPtr.Zero) throw new ArgumentNullException("Null pointer in read string operation");
            return Marshal.PtrToStringAnsi(pointer);
        }

        internal static short ReadShort(IntPtr pointer)
        {
            if (pointer == IntPtr.Zero) throw new ArgumentNullException("Null pointer in read int operation"); return Marshal.ReadInt16(pointer);
        }
        internal static bool AllocateHandle(ODBCHType handleType, IntPtr inputHandle, out IntPtr outputHandle)
        {

            IntPtr handle = IntPtr.Zero;
            var result = ODBCNativeMethods.SQLAllocHandle(handleType, inputHandle, out handle);
            outputHandle = handle;

            if ((result != ODBCResult.Success) & (result != ODBCResult.SuccessWithInfo))
            {
                throw new ODBCAPIError("Can't allocate environment handle ");
            }

            return true;
        }
        internal static bool AllocateHandle(ODBCHType handleType, ODBCHEnvironment inputHandle, out IntPtr outputHandle)
        {

            IntPtr handle;
            var result = ODBCNativeMethods.SQLAllocHandle(handleType, inputHandle, out handle);
            outputHandle = handle;
            if ((result != ODBCResult.Success) & (result != ODBCResult.SuccessWithInfo))
            {
                throw GetException(inputHandle, "Error allocating connection handle");
            }
            return true;
        }
        internal static bool AllocateHandle(ODBCHType handleType, ODBCHConnection inputHandle, out IntPtr outputHandle)
        {

            IntPtr handle;
            var result = ODBCNativeMethods.SQLAllocHandle(handleType, inputHandle, out handle);
            outputHandle = handle;
            if ((result != ODBCResult.Success) & (result != ODBCResult.SuccessWithInfo))
            {
                throw GetException(inputHandle, "Error allocating statement handle");
            }

            return true;
        }
        internal static bool SQLBindColumn(ODBCHStatement statementHandle, ODBCData column)
        {
            var result = ODBCNativeMethods.SQLBindCol(statementHandle, column.ColumnNumber, column.ColumnType, column.ColumnData, column.ColumnDataLength, out column.ColumnDataRealLength);
            if ((result != ODBCResult.Success) & (result != ODBCResult.SuccessWithInfo))
            {
                throw GetException(statementHandle, "Error binding columns");
            }
            return true;
        }

        internal static bool SQLBindColumn(ODBCHStatement statementHandle, short ColumnNumber, ODBCDataType TargetType, StringBuilder TargetValue, int BufferLength, out int StrLen_or_ind)
        {
            var result = ODBCNativeMethods.SQLBindCol(statementHandle, ColumnNumber, TargetType, TargetValue, BufferLength, out StrLen_or_ind);
          if ((result != ODBCResult.Success) & (result != ODBCResult.SuccessWithInfo))
            {
                throw GetException(statementHandle, "Error binding columns");
            }
            return true;
        }
        internal static bool GetTableColumns(ODBCHStatement statementHandle, string schemaName, string tableName)
        {
            var result = ODBCNativeMethods.SQLColumns(statementHandle, string.Empty, 0, schemaName, (short)schemaName.Length, tableName, (short)tableName.Length, String.Empty, (short)0);
            if ((result != ODBCResult.Success) & (result != ODBCResult.SuccessWithInfo))
            {
                throw GetException(statementHandle, "Error getting column list");
            }
            return true;
        }
        internal static bool GetTables(ODBCHStatement statementHandle, string catalogName, string schemaName, string tableName)
        {
            var result = ODBCNativeMethods.SQLTables(statementHandle, catalogName, (short)(catalogName.Length), schemaName, (short)(schemaName.Length), tableName, (short)(tableName.Length), String.Empty, (short)0);
            if ((result != ODBCResult.Success) & (result != ODBCResult.SuccessWithInfo))
            {
                throw GetException(statementHandle, "Error getting table list");
            }
            return true;
        }

        internal static bool GetPrimKey(ODBCHStatement statementHandle, string schemaName, string tableName)
        {
            var result = ODBCNativeMethods.SQLPrimaryKeys(statementHandle, String.Empty, (short)0, schemaName, (short)schemaName.Length, tableName, (short)tableName.Length);
            if ((result != ODBCResult.Success) & (result != ODBCResult.SuccessWithInfo))
            {
                throw GetException(statementHandle, "Error getting primary key definition");
            }
            return true;
        }

        internal static bool GetIndexInfo(ODBCHStatement statementHandle, string schemaName, string tableName, ushort indexType)
        {
            var result = ODBCNativeMethods.SQLStatistics(statementHandle, String.Empty, (short)0, schemaName, (short)schemaName.Length, tableName, (short)tableName.Length, indexType, 0);
            if ((result != ODBCResult.Success) & (result != ODBCResult.SuccessWithInfo))
            {
                throw GetException(statementHandle, "Error getting index definition");
            }
            return true;

        }
        internal static bool Fetch(ODBCHStatement statementHandle)
        {
            var result = ODBCNativeMethods.SQLFetch(statementHandle);
            if ((result != ODBCResult.Success) & (result != ODBCResult.SuccessWithInfo) & (result != ODBCResult.NoData))
            {
                throw GetException(statementHandle, "Error fetching data");
            }
            return ((result == ODBCResult.Success) || (result == ODBCResult.SuccessWithInfo));
        }



        internal static IntPtr AllocateEnvironmentHandle()
        {
            IntPtr handle;
            var result = ODBCNativeMethods.SQLAllocHandle(ODBCHType.Environment, IntPtr.Zero, out handle);
            if ((result != ODBCResult.Success) && (result != ODBCResult.SuccessWithInfo))
            {
                throw new ODBCAPIError("Unable to allocate ODBC environment handle.");
            }

            return handle;
        }

        internal static IntPtr AllocateStatementHandle(ODBCHConnection connectionHandle)
        {
            if (connectionHandle == null) throw new ArgumentNullException("connectionHandle");
            IntPtr handle;
            var result = ODBCNativeMethods.SQLAllocHandle(ODBCHType.Statement, connectionHandle, out handle);
            if ((result == ODBCResult.Success) || (result == ODBCResult.SuccessWithInfo)) return handle;
            var ex = GetException(connectionHandle, "Unable to allocate ODBC statement handle.");
            throw ex;
        }


        internal static bool ConnectTo(ODBCHConnection connectionHandle, string connectionString, short driverParam = 0)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("Connection string is empty!");
            }

            short bufferLengthNeeded;
            StringBuilder outString = new StringBuilder(1024);
            var result = ODBCNativeMethods.SQLDriverConnectW(connectionHandle, IntPtr.Zero, connectionString, (short)connectionString.Length, outString, (short)outString.Capacity, out bufferLengthNeeded, driverParam);
            if ((result != ODBCResult.Success) & (result != ODBCResult.SuccessWithInfo))
            {
                throw GetException(connectionHandle, "Unnable to connect using connection string " + connectionString);
            }
            return true;
        }

        internal static void Disconnect(ODBCHConnection connectionHandle)
        {
            if (connectionHandle != null)
            {
                var result = ODBCNativeMethods.SQLDisconnect(connectionHandle);
                if ((result == ODBCResult.Success) || (result == ODBCResult.SuccessWithInfo)) return;
                var ex = GetException(connectionHandle, "Unable to disconnect the database.");
                throw ex;
            }
        }


        internal static bool ReleaseHandle(ODBCHType handleType, IntPtr handle)
        {
            return (ODBCNativeMethods.SQLFreeHandle(handleType, handle) == ODBCResult.Success);
        }

        internal static void SetIntEnvironmentAttribute(ODBCHEnvironment environmentHandle, ODBCEnvAttr attribute, ODBCVersion value)
        {
            if (environmentHandle != null)
            {
                var result = ODBCNativeMethods.SQLSetEnvAttr(environmentHandle, attribute, value, 0);
                if ((result != ODBCResult.Success) && (result != ODBCResult.SuccessWithInfo))
                {
                    var ex = GetException(environmentHandle, string.Format("Unable to set ODBC environment attribute '{0:G}'.", attribute));
                    throw ex;
                }
            }
        }

        internal static bool GetConnectionInfoString(ODBCHConnection connectionHandle, ushort infoType, out StringBuilder resultStr)
        {
            resultStr = new StringBuilder();
            short _bufferLength = 0;
            short _stringLengthPtr = 0;
            var result = ODBCNativeMethods.SQLGetInfo(connectionHandle, infoType, null, _bufferLength, out _stringLengthPtr);
            if ((result == ODBCResult.Success) || (result == ODBCResult.SuccessWithInfo))
            {
                if (_stringLengthPtr > 0)
                {
                    resultStr.Capacity = _stringLengthPtr;
                    _bufferLength = _stringLengthPtr;
                    result = ODBCNativeMethods.SQLGetInfo(connectionHandle, infoType, resultStr, _bufferLength, out _stringLengthPtr);
                    if ((result != ODBCResult.Success) && (result != ODBCResult.SuccessWithInfo))
                    {
                        var ex = GetException(connectionHandle, string.Format("Unable to get information  about data source code '{0}'.", infoType.ToString()));
                        throw ex;
                    }


                }
            }
            else
            {
                var ex = GetException(connectionHandle, string.Format("Unable to get information  about data source code '{0}'.", infoType.ToString()));
                throw ex;
            }
            return true;
        }
        internal static bool GetConnectionInfoShort(ODBCHConnection connectionHandle, ushort infoType, out ushort resultShort)
        {
            resultShort = 0;
            short _bufferLength = 4;
            short _stringLengthPtr = 0;
            var result = ODBCNativeMethods.SQLGetInfo(connectionHandle, infoType, out resultShort, _bufferLength, out _stringLengthPtr);
            if ((result != ODBCResult.Success) && (result != ODBCResult.SuccessWithInfo))
            {
                var ex = GetException(connectionHandle, string.Format("Unable to get information  about data source code '{0}'.", infoType.ToString()));
                throw ex;
            }
            return true;
        }
        internal static ODBCResult GetDataSources(ODBCHEnvironment enviromentHandle, ushort direction, out StringBuilder serverName, out StringBuilder description)
        {
            serverName = new StringBuilder();
            description = new StringBuilder();
            short nameLength1Ptr = 0;
            short bufferLength1 = ODBCConstants.MAX_DSN_LENGTH + 1;
            short bufferLength2 = 0;
            short nameLength2Ptr = 0;
            var res = ODBCNativeMethods.SQLDataSources(enviromentHandle, direction, null, bufferLength1, out nameLength1Ptr, null, bufferLength2, out nameLength2Ptr);
            if ((res == ODBCResult.Success) || (res == ODBCResult.SuccessWithInfo))
            {
                if (nameLength1Ptr > 0)
                {
                    serverName.Capacity = nameLength1Ptr + 2;
                    bufferLength1 = (short)(nameLength1Ptr + 2);
                    if (nameLength2Ptr > 0)
                    {
                        description.Capacity = nameLength2Ptr + 2;
                        bufferLength2 = (short)(nameLength2Ptr + 2);
                    }
                    res = ODBCNativeMethods.SQLDataSources(enviromentHandle, direction, serverName, bufferLength1, out nameLength1Ptr, description, bufferLength2, out nameLength2Ptr);
                    if ((res != ODBCResult.Success) && (res != ODBCResult.SuccessWithInfo) && (res != ODBCResult.NoData))
                    {
                        var ex = GetException(enviromentHandle, "Unable to get ODBC data sources list ");
                        throw ex;
                    }
                }
            }
            return res;
        }
        internal static bool GetConnectionInfo(ODBCHConnection connectionHandle, ushort infoType, out String resultStr)
        {

            short _bufferLength = 0;
            short _stringLengthPtr = 0;
            IntPtr Value = IntPtr.Zero;
            resultStr = String.Empty;
            var result = ODBCNativeMethods.SQLGetInfo(connectionHandle, infoType, Value, _bufferLength, out _stringLengthPtr);
            if ((result == ODBCResult.Success) || (result == ODBCResult.SuccessWithInfo))
            {
                if (_stringLengthPtr > 0)
                {
                    _bufferLength = _stringLengthPtr;
                    Value = Marshal.AllocHGlobal(_stringLengthPtr);
                    result = ODBCNativeMethods.SQLGetInfo(connectionHandle, infoType, Value, _bufferLength, out _stringLengthPtr);
                    if ((result == ODBCResult.Success) || (result == ODBCResult.SuccessWithInfo))
                    {
                        resultStr = Marshal.PtrToStringAnsi(Value);
                    }
                    else
                    {
                        Marshal.FreeHGlobal(Value);
                        var exerr = GetException(connectionHandle, string.Format("Unable to get information  about data source code '{0}'.", infoType.ToString()));
                        throw exerr;
                    }
                    Marshal.FreeHGlobal(Value);
                }

                return true;
            }
            var ex = GetException(connectionHandle, string.Format("Unable to get information  about data source code '{0}'.", infoType.ToString()));
            throw ex;
        }
        public static bool ExecDirect(ODBCHStatement statementHandle, string statementText, int textLength, out string info)
        {
            info = string.Empty;
            if (statementText == null)
            { throw new ArgumentNullException("Command string is empty"); }
            if (statementText.Trim() == "")
            {
                throw new ArgumentException("Command string is empty");
            }

            var result = ODBCNativeMethods.SQLExecDirectW(statementHandle, statementText, textLength);
            if ((result != ODBCResult.Success) & (result != ODBCResult.SuccessWithInfo))
            {
                throw GetException(statementHandle, "Unnable to execute sql command " + statementText);
            }
            if (result == ODBCResult.SuccessWithInfo)
            {
                var list_info = GetErrorInfo(statementHandle);
                if (list_info.Count > 0)
                {
                    info = list_info[0].EMessage;
                }
            }

            return true;
        }

        public static bool SetConnectionProp(ODBCHConnection nStatementHandle, short cSetting, int eExpression)
        {
            var result = ODBCNativeMethods.SQLSetConnectAttr(nStatementHandle, cSetting, eExpression, 0);

            if (result != ODBCResult.Success)
            {
                throw GetException(nStatementHandle, "Unnable to set connection property ");
            }
            return true;
        }

        #endregion

        #region exceptions

        private class ODBCErrorInfo
        {
            public string ECode { get; private set; }
            public string EMessage { get; private set; }
            public string ENativeCode { get; private set; }
            public ODBCErrorInfo(string eCode, string eNCode, string eMessage)
            {
                ECode = eCode;
                ENativeCode = eNCode;
                EMessage = eMessage;
            }

        }
        private static List<ODBCErrorInfo> GetErrorInfo(ODBCHBase handle)
        {
            var eList = new List<ODBCErrorInfo>();
            short start = 1;
            var messageBuffer = new StringBuilder(256);
            var stateBuffer = new StringBuilder(5);
            int nativeError;
            short bufferLengthNeeded;
            var resultString = new StringBuilder();
            while (true)
            {
                var result = ODBCNativeMethods.SQLGetDiagRecW(handle.HandleType, handle, start, stateBuffer, out nativeError, messageBuffer, (short)messageBuffer.Capacity, out bufferLengthNeeded);
                if (result == ODBCResult.SuccessWithInfo)
                {
                    bufferLengthNeeded = (short)((int)bufferLengthNeeded + 1);
                    if (messageBuffer.Capacity < bufferLengthNeeded)
                    {
                        messageBuffer.Capacity = bufferLengthNeeded;
                        result = ODBCNativeMethods.SQLGetDiagRecW(handle.HandleType, handle, start, stateBuffer, out nativeError, messageBuffer, (short)messageBuffer.Capacity, out bufferLengthNeeded);
                    }
                }
                if ((result != ODBCResult.Success) && (result != ODBCResult.SuccessWithInfo))
                {
                    break;
                }
                eList.Add(new ODBCErrorInfo(stateBuffer.ToString(), nativeError.ToString(), messageBuffer.ToString()));
                start++;
            }
            return eList;
        }
        internal static ODBCAPIError GetException(ODBCHBase handle, string additionalMessage)
        {
            var eList = GetErrorInfo(handle);
            var resultString = new StringBuilder();
            resultString.Append(additionalMessage);
            foreach (var eMes in eList)
            {
                resultString.Append(Environment.NewLine);
                resultString.Append(string.Format("[{0}] [{1}] {2}", eMes.ECode, eMes.ENativeCode, eMes.EMessage));
            }

            return new ODBCAPIError(resultString.ToString());
        }

        #endregion
    }
}