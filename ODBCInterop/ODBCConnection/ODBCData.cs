using System.Runtime.InteropServices;
using System;
using System.Text;
namespace ODBCConnection
{
    /*odbc api возвращает данные о структуре таблиц в виде набора строк
  где для каждой интересующей колонке предварительно должен быть выделен буфер в памяти
  класс ODBCData содержит в себе описание колонки  для записи данных
  * из результирующего набора
  * 
  */
    #region odbc data

    public class ODBCData : IDisposable
    {

        public short ColumnNumber { get; private set; } //номер колонки в  наборе, возвращенном ODBC
        public ODBCDataType ColumnType { get; private set; } //тип содерж. данных
        public int ColumnDataLength { get; private set; }//размер буфера, кот. необходим функции ODBC для передачи данных
        internal int ColumnDataRealLength; // сюда ODBC функции возвращают размер данных
        public IntPtr ColumnData;// указатель на буфер с данными ODBC
        public StringBuilder ColumnStringData;
        internal BufferData InternalData; //данные, извлеченные из буфера ODBC
        public int ColumnIndex;//индекс колонки в сокращенном локальном наборе 
        public ODBCData(short columnNumber, ODBCDataType columnType, int columnDataLength)
        {
            ColumnNumber = columnNumber;
            ColumnType = columnType;
            ColumnDataLength = columnDataLength;
            ColumnData = Marshal.AllocHGlobal(ColumnDataLength);
            ColumnStringData = new StringBuilder(ColumnDataLength);
            InternalData = new BufferData();
        }
        private void CloseHandle()
        {
            if (ColumnData != null)
            {

                Marshal.FreeHGlobal(ColumnData);

                ColumnData = IntPtr.Zero;
            }
        }

        public void Dispose()
        {
            CloseHandle();
            GC.SuppressFinalize(this);
        }

        ~ODBCData()
        {
            CloseHandle();
        }
    }
    #endregion
}
