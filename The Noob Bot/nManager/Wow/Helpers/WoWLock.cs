using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using nManager.Helpful;
using nManager.Wow.Patchables;

namespace nManager.Wow.Helpers
{
    public class WoWLock
    {
        private readonly LockDbcRecord _lockDBCRecord0;

        private static DB6Reader _lockDB2;

        private static void Init()
        {
            if (_lockDB2 == null)
            {
                var mDefinitions = DBFilesClient.Load(Application.StartupPath + @"\Data\DBFilesClient\dblayout.xml");
                var definitions = mDefinitions.Tables.Where(t => t.Name == "Lock");

                if (!definitions.Any())
                {
                    definitions = mDefinitions.Tables.Where(t => t.Name == Path.GetFileName("Lock"));
                }
                if (definitions.Count() == 1)
                {
                    var table = definitions.First();
                    _lockDB2 = DBReaderFactory.GetReader(Application.StartupPath + @"\Data\DBFilesClient\Lock.db2", table) as DB6Reader;
                    Logging.Write(_lockDB2.FileName + " loaded with " + _lockDB2.RecordsCount + " entries.");
                }
                else
                {
                    Logging.Write("DB2 Lock not read-able.");
                }
            }
        }

        private WoWLock(uint id)
        {
            Init();
            LockDbcRecord tempLockDbcRecord = new LockDbcRecord();
            bool found = false;
            for (int i = 0; i < _lockDB2.RecordsCount - 1; i++)
            {
                tempLockDbcRecord = DB6Reader.ByteToType<LockDbcRecord>(_lockDB2.Rows.ElementAt(i));
                if (tempLockDbcRecord.Id == id)
                {
                    found = true;
                    break;
                }
            }
            _lockDBCRecord0 = found ? tempLockDbcRecord : new LockDbcRecord();
        }

        // Factory function
        public static WoWLock FromId(uint id)
        {
            return new WoWLock(id);
        }

        public LockDbcRecord Record
        {
            get { return _lockDBCRecord0; }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct LockDbcRecord
        {
            public uint Id;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)] public uint[] LockType;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)] public ushort[] Skill;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)] public byte[] KeyType;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)] public byte[] Action;
        }
    }
}