using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using nManager.Helpful;

namespace nManager.Wow.Helpers
{
    public class WoWItemClass
    {
        private ItemClassDB2Record _rItemClassRec0;
        private static DB5Reader _itemClassDB2;

        private static void Init() // todo make DBC loading shared accross all others reads with a better loading class.
        {
            if (_itemClassDB2 == null)
            {
                var mDefinitions = DBFilesClient.Load(Application.StartupPath + @"\Data\DBFilesClient\dblayout.xml");
                var definitions = mDefinitions.Tables.Where(t => t.Name == "ItemClass");

                if (!definitions.Any())
                {
                    definitions = mDefinitions.Tables.Where(t => t.Name == Path.GetFileName("ItemClass"));
                }
                if (definitions.Count() == 1)
                {
                    var table = definitions.First();
                    _itemClassDB2 = DBReaderFactory.GetReader(Application.StartupPath + @"\Data\DBFilesClient\ItemClass.db2", table) as DB5Reader;
                    Logging.Write(_itemClassDB2.FileName + " loaded with " + _itemClassDB2.RecordsCount + " entries.");
                }
                else
                {
                    Logging.Write("DBC ItemClass not read-able.");
                }
            }
        }

        private WoWItemClass(string name)
        {
            Init();
            ItemClassDB2Record tempItemClassDb2Record = new ItemClassDB2Record();
            bool found = false;
            for (int i = 0; i < _itemClassDB2.RecordsCount; i++)
            {
                tempItemClassDb2Record = DB5Reader.ByteToType<ItemClassDB2Record>(_itemClassDB2.Rows.ElementAt(i));
                if (tempItemClassDb2Record.Name() == name)
                {
                    found = true;
                    break;
                }
            }
            _rItemClassRec0 = found ? tempItemClassDb2Record : new ItemClassDB2Record();
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
            get { return _rItemClassRec0.Name(); }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ItemClassDB2Record
        {
            public uint ClassId;
            public uint PointerToStringTable;
            public uint ClassNameOffset;
            public uint lastByte;


            public string Name()
            {
                string strValue;
                if (_itemClassDB2.StringTable != null && _itemClassDB2.StringTable.TryGetValue((int) ClassNameOffset, out strValue))
                {
                    return strValue;
                }
                return "";
            }
        }
    }
}