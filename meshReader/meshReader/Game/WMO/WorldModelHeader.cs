using System.IO;
using meshReader.Helper;
using SlimDX;

namespace meshReader.Game.WMO
{
    public class WorldModelHeader
    {
        public uint CountMaterials;
        public uint CountGroups;
        public uint CountPortals;
        public uint CountLights;
        public uint CountModels;
        public uint CountDoodads;
        public uint CountSets;
        public uint AmbientColor;
        public uint WmoId;
        public Vector3[] BoundingBox;
        public uint LiquidTypeRelated;

        public static WorldModelHeader Read(Stream s)
        {
            var r = new BinaryReader(s);
            var ret = new WorldModelHeader();
            ret.CountMaterials = r.ReadUInt32();
            ret.CountGroups = r.ReadUInt32();
            ret.CountPortals = r.ReadUInt32();
            ret.CountLights = r.ReadUInt32();
            ret.CountModels = r.ReadUInt32();
            ret.CountDoodads = r.ReadUInt32();
            ret.CountSets = r.ReadUInt32();
            ret.AmbientColor = r.ReadUInt32();
            ret.WmoId = r.ReadUInt32();
            ret.BoundingBox = new Vector3[2];
            ret.BoundingBox[0] = Vector3Helper.Read(s);
            ret.BoundingBox[1] = Vector3Helper.Read(s);
            ret.LiquidTypeRelated = r.ReadUInt32();
            return ret;
        }
    }
}