using System.IO;
using meshReader.Helper;
using Microsoft.Xna.Framework;

namespace meshReader.Game.WMO
{
    
    public class DoodadInstance
    {
        public uint FileOffset;
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
            ret.FileOffset = r.ReadUInt32();
            ret.Position = Vector3Helper.Read(s);
            ret.QuatW = r.ReadSingle();
            ret.QuatX = r.ReadSingle();
            ret.QuatY = r.ReadSingle();
            ret.QuatZ = r.ReadSingle();
            ret.Scale = r.ReadSingle();
            ret.LightColor = r.ReadUInt32();
            return ret;
        }
    }

}