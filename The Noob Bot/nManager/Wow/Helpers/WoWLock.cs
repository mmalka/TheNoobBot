using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using nManager.Wow.Enums;
using nManager.Wow.Patchables;

namespace nManager.Wow.Helpers
{
    public class WoWLock
    {
        private LockDbcRecord lockDbcRecord_0;

        private static DBC<LockDbcRecord> lockDBC;

        private WoWLock(uint id)
        {
            if (lockDBC == null)
                lockDBC = new DBC<LockDbcRecord>((int)Addresses.DBC.Lock);
            lockDbcRecord_0 = lockDBC.GetRow((int)id);
        }

        // Factory function
        public static WoWLock FromId(uint id)
        {
            return new WoWLock(id);
        }

        public LockDbcRecord Record
        {
            get
            {
                return this.lockDbcRecord_0;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct LockDbcRecord
        { // 1 + 4*8 = 33 fields
            public uint Id;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public uint[] KeyType;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public uint[] LockType;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public uint[] Skill;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public uint[] Action;
        }

    }
}
