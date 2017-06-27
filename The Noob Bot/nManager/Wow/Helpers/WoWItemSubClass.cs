using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using nManager.Helpful;

namespace nManager.Wow.Helpers
{
    public class WoWItemSubClass
    {
        private ItemSubClassDbcRecord _rItemSubClassRec0;

        private static DB6Reader _itemSubClassDB2;

        private static void Init() // todo make DBC loading shared accross all others reads with a better loading class.
        {
            if (_itemSubClassDB2 == null)
            {
                var mDefinitions = DBFilesClient.Load(Application.StartupPath + @"\Data\DBFilesClient\dblayout.xml");
                var definitions = mDefinitions.Tables.Where(t => t.Name == "ItemSubClass");

                if (!definitions.Any())
                {
                    definitions = mDefinitions.Tables.Where(t => t.Name == Path.GetFileName("ItemSubClass"));
                }
                if (definitions.Count() == 1)
                {
                    var table = definitions.First();
                    _itemSubClassDB2 = DBReaderFactory.GetReader(Application.StartupPath + @"\Data\DBFilesClient\ItemSubClass.db2", table) as DB6Reader;
                    Logging.Write(_itemSubClassDB2.FileName + " loaded with " + _itemSubClassDB2.RecordsCount + " entries.");
                }
                else
                {
                    Logging.Write("DB2 ItemSubClass not read-able.");
                }
            }
        }

        private WoWItemSubClass(string name, uint iClass)
        {
            Init();
            ItemSubClassDbcRecord tempItemSubClassDb2Record = new ItemSubClassDbcRecord();
            bool found = false;
            for (int i = 0; i < _itemSubClassDB2.RecordsCount - 1; i++)
            {
                tempItemSubClassDb2Record = DB6Reader.ByteToType<ItemSubClassDbcRecord>(_itemSubClassDB2.Rows.ElementAt(i));
                if (tempItemSubClassDb2Record.LongName() == name && tempItemSubClassDb2Record.ClassId == iClass)
                {
                    found = true;
                    break;
                }
            }
            _rItemSubClassRec0 = found ? tempItemSubClassDb2Record : new ItemSubClassDbcRecord();
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

        public string Name
        {
            get { return _rItemSubClassRec0.Name(); }
        }

        public string LongName
        {
            get { return _rItemSubClassRec0.LongName(); }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ItemSubClassDbcRecord
        {
            public uint Index;
            public uint SubClassNameOffset;
            public uint SubClassLongNameOffset;
            public ushort field0C;
            public byte ClassId;
            public byte SubClassId;
            public byte field10;
            public byte field11;
            public byte field12;
            public byte field13;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)] public byte[] field14;

            public string Name()
            {
                string strValue;
                if (_itemSubClassDB2.StringTable != null && _itemSubClassDB2.StringTable.TryGetValue((int) SubClassNameOffset, out strValue))
                {
                    return strValue;
                }
                return "";
            }

            public string LongName()
            {
                string strValue;
                if (_itemSubClassDB2.StringTable != null && _itemSubClassDB2.StringTable.TryGetValue((int) SubClassLongNameOffset, out strValue))
                {
                    return strValue;
                }
                return "";
            }
        }
    }
}