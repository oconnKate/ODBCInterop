namespace ODBCNative
{
    public enum ODBCResult : short
    {
        Error = -1,
        InvalidHandle = -2,
        NoData = 100,
        Success = 0,
        SuccessWithInfo = 1,
    }
}