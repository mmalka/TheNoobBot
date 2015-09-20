using System.IO;
using meshReader.Helper;
using SlimDX;

namespace meshReader.Game.WMO
{
    public class WorldModelMaterialTexture // 16 * 32bits = 16 * 4 bytes = 64 bytes
    {
        public uint MaterialId;
        public uint Shader;
        public uint BlendMode;
        public uint Texture1;
        public uint Color1;
        public uint Flags1;
        public uint Texture2;
        public uint Color2;
        public uint Flags2;
        public uint Texture3;
        public uint Color3;
        public uint Flags3;
        public float Diffcolor1;
        public float Diffcolor2;
        public float Diffcolor3;
        public uint RunTimeData;
        public string Texture1Name;

        public static WorldModelMaterialTexture Read(Stream s)
        {
            var r = new BinaryReader(s);
            var ret = new WorldModelMaterialTexture();
            ret.MaterialId = r.ReadUInt32();
            ret.Shader = r.ReadUInt32();
            ret.BlendMode = r.ReadUInt32();
            ret.Texture1 = r.ReadUInt32();
            ret.Color1 = r.ReadUInt32();
            ret.Flags1 = r.ReadUInt32();
            ret.Texture2 = r.ReadUInt32();
            ret.Color2 = r.ReadUInt32();
            ret.Flags2 = r.ReadUInt32();
            ret.Texture3 = r.ReadUInt32();
            ret.Color3 = r.ReadUInt32();
            ret.Flags3 = r.ReadUInt32();
            ret.Diffcolor1 = r.ReadSingle();
            ret.Diffcolor2 = r.ReadSingle();
            ret.Diffcolor3 = r.ReadSingle();
            ret.RunTimeData = r.ReadUInt32();
            return ret;
        }
    }
}
