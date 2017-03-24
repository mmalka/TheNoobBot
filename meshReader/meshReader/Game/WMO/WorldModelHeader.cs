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
        public uint ExtraFlag;
        public uint numLod;

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
            ret.ExtraFlag = r.ReadUInt16();
            /* 
             * _t flag_attenuate_vertices_based_on_distance_to_portal : 1;
             * uint16_t flag_skip_base_color : 1; // do not add base (ambient) color (of MOHD) to MOCVs. apparently does more, e.g. required for multiple MOCVs
             * uint16_t flag_liquid_related : 1; // fills the whole WMO with water (used for underwater WMOs). (possibly - LiquidType related, see below in the MLIQ).
             * uint16_t flag_has_some_outdoor_group : 1; // possibly - has some group that is outdoors
             * uint16_t Flag_Lod : 1; 
             */
            ret.numLod = r.ReadUInt16();
            return ret;
        }
    }
}