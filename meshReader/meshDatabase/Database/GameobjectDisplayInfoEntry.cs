using System.Runtime.InteropServices;
using SlimDX;

namespace meshDatabase.Database
{
    public class GameobjectDisplayInfoEntry
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct GameobjectDisplayInfoDb2Record
        {
            public int DisplayId;
            public int DataId;
            public Vector3 LowerBound;
            public Vector3 HigherBound;
            public int unk1;
            public int unk2;
            public ushort unk3;
            public ushort unk4;
        }

        public int DisplayId { get; private set; }
        public int DataId { get; private set; }
        public Vector3 LowerBound { get; private set; }
        public Vector3 HigherBound { get; private set; }

        public GameobjectDisplayInfoEntry(Record rec)
        {
            DisplayId = rec[0];
            DataId = rec[1];
            LowerBound = new Vector3(rec.GetFloat(12), rec.GetFloat(13), rec.GetFloat(14));
            HigherBound = new Vector3(rec.GetFloat(15), rec.GetFloat(16), rec.GetFloat(17));
        }
    }
}