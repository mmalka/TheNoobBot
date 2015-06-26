using System.Runtime.InteropServices;
using nManager.Wow.Patchables;

namespace nManager.Wow.Helpers
{
    public class WoWItemClass
    {
        private ItemClassDB2Record _rItemClassRec0;

        private static DB2<ItemClassDB2Record> _rItemClassDB2;

        private WoWItemClass(string name)
        {
            if (_rItemClassDB2 == null)
                _rItemClassDB2 = new DB2<ItemClassDB2Record>((uint) Addresses.DBC.ItemClass);
            for (int id = _rItemClassDB2.MinIndex; id <= _rItemClassDB2.MaxIndex; id++)
            {
                _rItemClassRec0 = _rItemClassDB2.GetRow(id);
                if (_rItemClassRec0.ClassId == id)
                {
                    if (Name == name)
                    {
                        return;
                    }
                }
            }
            _rItemClassRec0 = new ItemClassDB2Record();
        }

        // Factory function
        public static WoWItemClass FromName(string name)
        {
            return new WoWItemClass(name);
        }

        public ItemClassDB2Record Record
        {
            get { return _rItemClassRec0; }
        }

        public string Name
        {
            get
            {
                return _rItemClassDB2.String(_rItemClassDB2.GetRowOffset((int) _rItemClassRec0.ClassId) +
                                             _rItemClassRec0.ClassNameOffset +
                                             (uint) Marshal.OffsetOf(typeof (ItemClassDB2Record), "ClassNameOffset"));
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ItemClassDB2Record
        {
            public uint ClassId;
            public uint unk2;
            public uint unk3;
            public uint ClassNameOffset;
        }
    }
}