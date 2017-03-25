using System.IO;
using meshReader.Helper;
using SlimDX;

namespace meshReader.Game.WMO
{
    public class DoodadInstance
    {
        public uint FileOffset;
        public ushort Flags;
        public string File;
        public Vector3 Position;
        public float QuatW;
        public float QuatX;
        public float QuatY;
        public float QuatZ;
        public float Scale;
        public uint LightColor;

        public static DoodadInstance Read(Stream s)
        {
            var r = new BinaryReader(s);
            var ret = new DoodadInstance();
            uint OffsetAndFlag = r.ReadUInt32();
            ret.FileOffset = OffsetAndFlag & 0x00FFFFFF;
            ret.Flags = (ushort) (OffsetAndFlag >> 24);
            ret.Position = Vector3Helper.Read(s);
            ret.QuatX = r.ReadSingle();
            ret.QuatY = r.ReadSingle();
            ret.QuatZ = r.ReadSingle();
            ret.QuatW = r.ReadSingle();
            ret.Scale = r.ReadSingle();
            ret.LightColor = r.ReadUInt32();
            return ret;
        }
    }
}