using System.Runtime.InteropServices;

namespace meshDatabase.Database
{
    public class TaxiPath
    {

        [StructLayout(LayoutKind.Sequential)]
        public struct TaxiPathDb2Record
        {
            public int To;
            public int From;
            public int Id;
            public int Cost;


            public bool IsValid()
            {
                return From > 0 && To > 0;
            }
        }
    }
}