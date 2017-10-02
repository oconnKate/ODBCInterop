namespace ODBCNative
{
    internal enum ODBCHType  // Defined in sql.h
    {
        Environment = 1,  // SQL_HANDLE_ENV
        Connection = 2,   // SQL_HANDLE_DBC
        Statement = 3     // SQL_HANDLE_STMT
    }

}