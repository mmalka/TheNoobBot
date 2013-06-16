using System.Runtime.InteropServices;
using nManager.Wow.Patchables;

namespace nManager.Wow.Helpers
{
    public class WoWLock
    {
        private readonly LockDbcRecord _lockDBCRecord0;

        private static DBC<LockDbcRecord> _lockDBC;

        private WoWLock(uint id)
        {
            if (_lockDBC == null)
                _lockDBC = new DBC<LockDbcRecord>((int) Addresses.DBC.Lock);
            _lockDBCRecord0 = _lockDBC.GetRow((int) id);
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
            // 1 + 4*8 = 33 fields
            public uint Id;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)] public uint[] KeyType;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)] public uint[] LockType;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)] public uint[] Skill;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)] public uint[] Action;
        }
    }
}