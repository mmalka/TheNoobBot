using System.IO;

namespace meshReader.Game.ADT
{

    public class MHDR
    {
        public uint Flags;
        public uint OffsetMCIN;
        public uint OffsetMTEX;
        public uint OffsetMMDX;
        public uint OffsetMMID;
        public uint OffsetMWMO;
        public uint OffsetMWID;
        public uint OffsetMDDF;
        public uint OffsetMODF;
        public uint OffsetMFBO;
        public uint OffsetMH2O;
        public uint OffsetMTFX;

        public void Read(Stream s)
        {
            var r = new BinaryReader(s);
            Flags = r.ReadUInt32();
            OffsetMCIN = r.ReadUInt32();
            OffsetMTEX = r.ReadUInt32();
            OffsetMMDX = r.ReadUInt32();
            OffsetMMID = r.ReadUInt32();
            OffsetMWMO = r.ReadUInt32();
            OffsetMWID = r.ReadUInt32();
            OffsetMDDF = r.ReadUInt32();
            OffsetMODF = r.ReadUInt32();
            OffsetMFBO = r.ReadUInt32();
            OffsetMH2O = r.ReadUInt32();
            OffsetMTFX = r.ReadUInt32();

        }
    }

}