using System.Runtime.InteropServices;
using nManager.Wow.Patchables;

namespace nManager.Wow.Helpers
{
    public class WoWItemSubClass
    {
        private ItemSubClassDbcRecord _rItemSubClassRec0;

        private static DBC<ItemSubClassDbcRecord> _rItemSubClassDBC;

        private WoWItemSubClass(string name, uint iClass)
        {
            if (_rItemSubClassDBC == null)
                _rItemSubClassDBC = new DBC<ItemSubClassDbcRecord>((int) Addresses.DBC.ItemSubClass);
            for (int id = _rItemSubClassDBC.MinIndex; id <= _rItemSubClassDBC.MaxIndex; id++)
            {
                _rItemSubClassRec0 = _rItemSubClassDBC.GetRow(id);
                if (_rItemSubClassRec0.Index == id)
                {
                    if (_rItemSubClassRec0.ClassId == iClass)
                    {
                        if (LongName == name)
                        {
                            return;
                        }
                    }
                }
            }
            _rItemSubClassRec0 = new ItemSubClassDbcRecord();
        }

        // Factory function
        public static WoWItemSubClass FromNameAndClass(string name, uint iClass)
        {
            return new WoWItemSubClass(name, iClass);
        }

        public ItemSubClassDbcRecord Record
        {
            get { return _rItemSubClassRec0; }
        }

        public string LongName
        {
            get
            {
                return _rItemSubClassDBC.String(_rItemSubClassDBC.GetRowOffset((int) _rItemSubClassRec0.Index) +
                                                _rItemSubClassRec0.SubClassLongNameOffset +
                                                (uint) Marshal.OffsetOf(typeof (ItemSubClassDbcRecord), "SubClassLongNameOffset"));
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ItemSubClassDbcRecord
        {
            public uint Index;
            public uint ClassId;
            public uint SubClassId;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)] public uint[] Unk;
            public uint SubClassNameOffset;
            public uint SubClassLongNameOffset;
        }
    }
}