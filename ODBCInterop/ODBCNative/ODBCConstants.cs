namespace ODBCNative
{
    //часть констант из sql.h,sqlext.h
    public static class ODBCConstants
    {
        #region handles
        public const int SQL_HANDLE_DBC = 2;
        public const int SQL_HANDLE_ENV = 1;
        public const int SQL_HANDLE_STMT = 3;
        #endregion
        #region enviroment_attributes
        public const int SQL_ATTR_ODBC_VERSION = 200;
        public const int SQL_ATTR_CONNECTION_POOLING = 201;
         public const int SQL_ATTR_CP_MATCH = 202;
        #endregion
         #region versions
         public const int SQL_OV_ODBC3 = 3;
        public const int SQL_OV_ODBC2 = 2;
#endregion
        #region results
        public const int SQL_SUCCESS = 0;
        public const int SQL_SUCCESS_WITH_INFO = 1;
        public const int SQL_NO_DATA = 100;
        public const int SQL_ERROR = -1;
        public const int SQL_INVALID_HANDLE = -2;
        public const int SQL_NEED_DATA = 99;
 public const int SQL_NULL_DATA = -1;
        public const int SQL_NO_TOTAL = -4;
        #endregion
        #region types
        public const int SQL_C_CHAR = 1;
        public const int SQL_NUMERIC = 2;
        public const int SQL_DECIMAL = 3;
        public const int SQL_INTEGER = 4;
        public const int SQL_SMALLINT = 5;
        public const int SQL_FLOAT = 6;
        public const int SQL_REAL = 7;
        public const int SQL_DOUBLE = 8;
        public const int SQL_DATETIME = 9;
        public const int SQL_VARCHAR = 12;
        public const int SQL_TYPE_DATE = 91;
        public const int SQL_TYPE_TIME = 92;
        public const int SQL_TYPE_TIMESTAMP = 93;
        #endregion
        #region sizes
        public const int TAB_LEN = 129;
        public const int COL_LEN = 129;
        public const int SMINT = 5;
        public const int MAXSMINT = 65535;
        public const ushort SQL_INDEX_UNIQUE = 0;
        public const ushort SQL_INDEX_ALL = 1;
        public const ushort SQL_SHORT_LEN = 2;
        public const ushort SQL_STATE_LEN = 5;
        public const int SQL_FETCH_NEXT = 1;
        public const int SQL_FETCH_FIRST = 2;
        public const int SQL_FETCH_FIRST_USER =31;
        public const int SQL_FETCH_FIRST_SYSTEM = 32;
        public const int MAX_DSN_LENGTH = 32;
        #endregion
        #region information
        public const int SQL_TABLE_STAT = 0;
        public const int SQL_TRUE = 0;
        public const int SQL_FALSE = 1;
        public const char SQL_ASC = 'A';
        public const char SQL_DESC = 'D';
        public const int SQL_IS_INDEX_UNIQUE = 0;
        public const int SQL_IS_INDEX_DUPLICATE = 1;
        public const int SQL_CP_OFF = 0;
        #endregion


    }
       
}