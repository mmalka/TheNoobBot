using System.Linq;
using System.Runtime.InteropServices;
using SlimDX;

namespace meshDatabase.Database
{
    public class TaxiNode
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct TaxiNodesDb2Record
        {
            public Vector3 Location;
            public int NameOffset;
            public int MountCreatureId1;
            public int MountCreatureId2;
            public int Unk1; // big numbers
            public int Unk2;
            public int MapId;
            public int Unk3; // 0 or 62xx or 59xx, mostly 0
            public int Unk4; // Similar to NodeId, except it's most of the time 0 and the rest of the time = to NodeId.
            public int Unk5; // 0, 1, 2, 3, 5, 6, 7, 9, 10, 11, 19
            public int Id;

            public bool IsHorde()
            {
                return KnownHordeMounts.Contains(MountCreatureId1) || KnownHordeMounts.Contains(MountCreatureId2);
            }

            public bool IsAlliance()
            {
                return KnownHordeMounts.Contains(MountCreatureId1) || KnownHordeMounts.Contains(MountCreatureId2);
            }

            public string Name()
            {
                string strValue;
                if (PhaseHelper._map.StringTable != null && PhaseHelper._map.StringTable.TryGetValue(NameOffset, out strValue))
                {
                    return strValue;
                }
                return "";
            }

            public bool IsValid()
            {
                return IsHorde() || IsAlliance();
            }
        }

        public static readonly int[] KnownAllianceMounts = new[] {308, 541, 3837};
        public static readonly int[] KnownHordeMounts = new[] {2224, 3574};
    }
}