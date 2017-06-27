using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.Enums;
using nManager.Wow.Patchables;

namespace nManager.Wow.Helpers
{
    public class WoWSpellCategories
    {
        [CompilerGenerated] private static SpellCategoriesDbcRecord _spellCategoriesDB2Record0;

        private static DB6Reader _spellCategoriesDb2;
        private static SpellCategoriesDbcRecord[] _cachedRecords;

        private static void Init() // todo make DBC loading shared accross all others reads with a better loading class.
        {
            if (_spellCategoriesDb2 == null)
            {
                var mDefinitions = DBFilesClient.Load(Application.StartupPath + @"\Data\DBFilesClient\dblayout.xml");
                var definitions = mDefinitions.Tables.Where(t => t.Name == "SpellCategories");

                if (!definitions.Any())
                {
                    definitions = mDefinitions.Tables.Where(t => t.Name == Path.GetFileName("SpellCategories"));
                }
                if (definitions.Count() == 1)
                {
                    var table = definitions.First();
                    _spellCategoriesDb2 = DBReaderFactory.GetReader(Application.StartupPath + @"\Data\DBFilesClient\SpellCategories.db2", table) as DB6Reader;
                    if (_cachedSpellCategoriesRows == null || _cachedRecords == null)
                    {
                        if (_spellCategoriesDb2 != null)
                        {
                            _cachedSpellCategoriesRows = _spellCategoriesDb2.Rows.ToArray();
                            _cachedRecords = new SpellCategoriesDbcRecord[_cachedSpellCategoriesRows.Length];
                            for (int i = 0; i < _cachedSpellCategoriesRows.Length - 1; i++)
                            {
                                _cachedRecords[i] = DB6Reader.ByteToType<SpellCategoriesDbcRecord>(_cachedSpellCategoriesRows[i]);
                            }
                        }
                    }
                    if (_spellCategoriesDb2 != null) Logging.Write(_spellCategoriesDb2.FileName + " loaded with " + _spellCategoriesDb2.RecordsCount + " entries.");
                }
                else
                {
                    Logging.Write("DB2 SpellCategories not read-able.");
                }
            }
        }

        private static BinaryReader[] _cachedSpellCategoriesRows;

        public static uint GetSpellCategoryBySpellId(uint spellid)
        {
            Init();
            for (int id = 0; id <= _spellCategoriesDb2.RecordsCount - 1; id++)
            {
                _spellCategoriesDB2Record0 = _cachedRecords[id];
                if (_spellCategoriesDB2Record0.m_spellID == spellid || _spellCategoriesDB2Record0.m_ID == spellid)
                {
                    return _spellCategoriesDB2Record0.m_spellCategoryID;
                }
            }
            return 0;
        }

        public static uint GetSpellStartRecoverCategoryBySpellId(uint spellid)
        {
            Init();
            for (int id = 0; id <= _spellCategoriesDb2.RecordsCount - 1; id++)
            {
                _spellCategoriesDB2Record0 = _cachedRecords[id];
                if (_spellCategoriesDB2Record0.m_spellID == spellid)
                {
                    return _spellCategoriesDB2Record0.m_StartRecoveryCategory;
                }
            }
            return 0;
        }

        public SpellCategoriesDbcRecord Record
        {
            get { return _spellCategoriesDB2Record0; }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SpellCategoriesDbcRecord
        {
            public uint m_ID;
            public uint m_spellID;
            public uint m_spellCategoryID;
            public ushort m_StartRecoveryCategory;
            public ushort field0A;
            public ushort m_SpellDifficultyID;
            public byte m_DefenseType;
            public byte m_DispelType;
            public byte m_SpellMechanic;
            public byte m_PreventionType;
            public byte field12_0;
            public byte field12_1;
        }
    }
}