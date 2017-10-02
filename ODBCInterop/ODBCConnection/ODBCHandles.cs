
using System;
using System.Runtime.InteropServices;
using ODBCNative;

namespace ODBCCHandles
{
//освобождаем неуправляемы ресурсы
    internal class ODBCHBase : SafeHandle
    {

        internal ODBCHType HandleType { get; private set; }
        internal ODBCHBase(ODBCHType hType)
            : base(IntPtr.Zero, true)
        {

            handle = IntPtr.Zero;
            HandleType = hType;



        }

        public override bool IsInvalid
        {
            get { return (handle == IntPtr.Zero); }
        }
        protected override bool ReleaseHandle()
        {

            IntPtr lhandle = handle;
            if (lhandle == IntPtr.Zero)
            { return true; }
            return ODBCMethods.ReleaseHandle(HandleType, lhandle);

        }

    }

    internal class ODBCHEnvironment : ODBCHBase
    {
        internal ODBCHEnvironment(ODBCVersion version)
            : base(ODBCHType.Environment)
        {
            bool res = ODBCMethods.AllocateHandle(ODBCHType.Environment, IntPtr.Zero, out handle);
            if (res)
            {
                ODBCMethods.SetIntEnvironmentAttribute(this, ODBCEnvAttr.Version, version);
            }

        }

    }
    internal class ODBCHConnection : ODBCHBase
    {
        internal ODBCHConnection(ODBCHEnvironment eHandle)
            : base(ODBCHType.Connection)
        {
            bool res = ODBCMethods.AllocateHandle(ODBCHType.Connection, eHandle, out handle);

        }



    }
    internal class ODBCHStatement : ODBCHBase
    {
        internal ODBCHStatement(ODBCHConnection cHandle)
            : base(ODBCHType.Statement)
        {
            bool res = ODBCMethods.AllocateHandle(ODBCHType.Statement, cHandle, out handle);

        }
    }
}