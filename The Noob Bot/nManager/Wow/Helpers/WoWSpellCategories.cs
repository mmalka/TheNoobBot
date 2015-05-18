using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using nManager.Wow.Enums;
using nManager.Wow.Patchables;

namespace nManager.Wow.Helpers
{
    public class WoWSpellCategories
    {
        [CompilerGenerated] private static SpellCategoriesDbcRecord spellCategoriesDbcRecord_0;
        [CompilerGenerated] private uint uint_0;

        private static DBC<SpellCategoriesDbcRecord> spellCategoriesDBC;

        private static void Init()
        {
            if (spellCategoriesDBC == null)
                spellCategoriesDBC = new DBC<SpellCategoriesDbcRecord>((int) Addresses.DBC.SpellCategories);
        }

        public static uint GetSpellCategoryBySpellId(uint spellid)
        {
            Init();
            for (int id = spellCategoriesDBC.MinIndex; id <= spellCategoriesDBC.MaxIndex; id++)
            {
                spellCategoriesDbcRecord_0 = spellCategoriesDBC.GetRow(id);
                if (spellCategoriesDbcRecord_0.m_spellID == spellid)
                {
                    return spellCategoriesDbcRecord_0.m_category;
                }
            }
            return 0;
        }

        public static uint GetSpellStartRecoverCategoryBySpellId(uint spellid)
        {
            for (int id = spellCategoriesDBC.MinIndex; id <= spellCategoriesDBC.MaxIndex; id++)
            {
                spellCategoriesDbcRecord_0 = spellCategoriesDBC.GetRow(id);
                if (spellCategoriesDbcRecord_0.m_spellID == spellid)
                {
                    return spellCategoriesDbcRecord_0.m_startRecoveryCategory;
                }
            }
            return 0;
        }

        public SpellCategoriesDbcRecord Record
        {
            get { return spellCategoriesDbcRecord_0; }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SpellCategoriesDbcRecord
        {
            public uint m_ID;
            public uint m_spellID;
            public uint m_difficultyID;
            public uint m_category;
            public uint m_defenseType;
            public uint m_dispelType;
            public uint m_mechanic;
            public uint m_preventionType;
            public uint m_startRecoveryCategory;
            public uint m_chargeCategory;
        }
    }
}