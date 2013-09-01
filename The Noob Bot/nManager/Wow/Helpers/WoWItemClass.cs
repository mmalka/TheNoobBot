using System.Runtime.InteropServices;
using nManager.Wow.Patchables;

namespace nManager.Wow.Helpers
{
    internal class WoWItemClass
    {
        private ItemClassDbcRecord _rItemClassRec0;

        private static DBC<ItemClassDbcRecord> _rItemClassDBC;

        private WoWItemClass(string name)
        {
            if (_rItemClassDBC == null)
                _rItemClassDBC = new DBC<ItemClassDbcRecord>((int) Addresses.DBC.ItemClass);
            for (int id = _rItemClassDBC.MinIndex; id <= _rItemClassDBC.MaxIndex; id++)
            {
                _rItemClassRec0 = _rItemClassDBC.GetRow(id);
                if (_rItemClassRec0.ClassId == id)
                {
                    string temp = Name;
                    if (temp == name)
                    {
                        return;
                    }
                }
            }
        }

        // Factory function
        public static WoWItemClass FromName(string name)
        {
            return new WoWItemClass(name);
        }

        public ItemClassDbcRecord Record
        {
            get { return _rItemClassRec0; }
        }

        public string Name
        {
            get { return _rItemClassDBC.String(_rItemClassRec0.ClassNameOffset); }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ItemClassDbcRecord
        {
            public uint ClassId;
            public uint unk2;
            public uint unk3;
            public uint ClassNameOffset;
        }
    }
}