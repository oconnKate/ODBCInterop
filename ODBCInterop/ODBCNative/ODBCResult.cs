namespace ODBCNative
{
    //standart odbc functions result 
    public enum ODBCResult : short //Defined in sql.h
    {
        Error = -1,
        InvalidHandle = -2,
        NoData = 100,
        Success = 0,
        SuccessWithInfo = 1,
    }
}