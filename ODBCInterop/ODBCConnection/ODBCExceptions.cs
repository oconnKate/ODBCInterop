using System;

namespace ODBCErrors
{
    public class ODBCErrorNotInitialized : Exception

    {
        public ODBCErrorNotInitialized(string message="Object has not been initialized!")
            : base(message)
        { }
    }

    public class ODBCAPIError : Exception
    { 
        
        public ODBCAPIError(string message = "ODBC API Error!")
            : base(message)
        { }
    }
}