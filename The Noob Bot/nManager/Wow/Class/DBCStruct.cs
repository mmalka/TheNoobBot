using System.Runtime.InteropServices;

namespace nManager.Wow.Class
{
    public class DBCStruct
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct WoWClientDB
        {
            public uint vTable;         // pointer to vtable
            public int numRows;         // number of rows
            public int maxIndex;        // maximal row index
            public int minIndex;        // minimal row index
            public uint data;           // pointer to data block
            public uint FirstRow;       // pointer to first row
            public uint Rows;           // pointer to rows array - not anymore!
            public uint unk1; // ptr
            public uint unk2; // 1
            public uint unk3; // ptr
            public uint unk4; // 2
        };
        [StructLayout(LayoutKind.Sequential)]
        public struct SpellRec
        {
            public int SpellId;
            public uint Name;
            public uint RankDescription;
            public int dwordC;
            public int dword10;
            public int SpellRuneCostId;
            public int dword18;
            public int dword1C;
            public int dword20;
            public int SpellScalingId;
            public int SpellAuraOptionsId;
            public int SpellAuraRestrictionsId;
            public int SpellCastingRequirementsId;
            public int SpellCategoriesId;
            public int SpellClassOptionsId;
            public int SpellCooldownsID;
            public int SpellEquippedItemsId;
            public int SpellinterruptsId;
            public int SpellLevelId;
            public int SpellReagentsId;
            public int SpellShapeshiftId;
            public int SpellTargetRestrictionsId;
            public int SpellTotemsId;
            public int dword5C;
            public int SpellMiscId;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct SpellMiscRec
        {
            public int int0;
            public int int4;
            public int int8;
            public int intC;
            public int int10;
            public int int14;
            public int int18;
            public int int1C_Flags;
            public int int20;
            public int int24;
            public int int28;
            public int int2C;
            public int int30;
            public int int34;
            public int int38;
            public int SpellCastTimesId;
            public int SpellDurationId;
            public int SpellRangeId;
            public float float48_TimeOrSpeedRelated;
            public int SpellVisualId;
            public int SpellVisualId_OverrideMaybe;
            public int SpellIconId;
            public int int58;
            public int int5C_Flags;
        };
        [StructLayout(LayoutKind.Sequential)]
        public struct SpellCastTimesRec
        {
            public int Id;
            public int CastTime;
            public int SpellCastTimes;
            public int MinCastTime;
        };
        [StructLayout(LayoutKind.Sequential)]
        public struct SpellRangeRec
        {
            public int Id;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public float[] RangeMin;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public float[] RangeMax;
            public int Flags;
            public int DisplayName_lang;
            public int DisplayNameShort_lang;
        };
    }
}
