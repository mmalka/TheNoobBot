using System.IO;
using meshReader.Helper;
using SlimDX;

namespace meshReader.Game.MDX
{

    public class ModelHeader
    {
        public byte[] Magic;
        public uint Version;
        public uint LengthModelName;
        public uint OffsetName;
        public uint ModelFlags;
        public uint CountGlobalSequences;
        public uint OffsetGlobalSequences;
        public uint CountAnimations;
        public uint OffsetAnimations;
        public uint CountAnimationLookup;
        public uint OffsetAnimationLookup;
        public uint CountBones;
        public uint OffsetBones;
        public uint CountKeyBoneLookup;
        public uint OffsetKeyBoneLookup;
        public uint CountVertices;
        public uint OffsetVertices;
        public uint CountViews;
        public uint CountColors;
        public uint OffsetColors;
        public uint CountTextures;
        public uint OffsetTextures;
        public uint CountTransparency;
        public uint OffsetTransparency;
        public uint CountUvAnimation;
        public uint OffsetUvAnimation;
        public uint CountTexReplace;
        public uint OffsetTexReplace;
        public uint CountRenderFlags;
        public uint OffsetRenderFlags;
        public uint CountBoneLookup;
        public uint OffsetBoneLookup;
        public uint CountTexLookup;
        public uint OffsetTexLookup;
        public uint CountTexUnits;
        public uint OffsetTexUnits;
        public uint CountTransLookup;
        public uint OffsetTransLookup;
        public uint CountUvAnimLookup;
        public uint OffsetUvAnimLookup;
        public Vector3[] VertexBox;
        public float VertexRadius;
        public Vector3[] BoundingBox;
        public float BoundingRadius;
        public uint CountBoundingTriangles;
        public uint OffsetBoundingTriangles;
        public uint CountBoundingVertices;
        public uint OffsetBoundingVertices;
        public uint CountBoundingNormals;
        public uint OffsetBoundingNormals;

        public void Read(Stream s)
        {
            var r = new BinaryReader(s);
            Magic = r.ReadBytes(4);
            Version = r.ReadUInt32();
            LengthModelName = r.ReadUInt32();
            OffsetName = r.ReadUInt32();
            ModelFlags = r.ReadUInt32();
            CountGlobalSequences = r.ReadUInt32();
            OffsetGlobalSequences = r.ReadUInt32();
            CountAnimations = r.ReadUInt32();
            OffsetAnimations = r.ReadUInt32();
            CountAnimationLookup = r.ReadUInt32();
            OffsetAnimationLookup = r.ReadUInt32();
            CountBones = r.ReadUInt32();
            OffsetBones = r.ReadUInt32();
            CountKeyBoneLookup = r.ReadUInt32();
            OffsetKeyBoneLookup = r.ReadUInt32();
            CountVertices = r.ReadUInt32();
            OffsetVertices = r.ReadUInt32();
            CountViews = r.ReadUInt32();
            CountColors = r.ReadUInt32();
            OffsetColors = r.ReadUInt32();
            CountTextures = r.ReadUInt32();
            OffsetTextures = r.ReadUInt32();
            CountTransparency = r.ReadUInt32();
            OffsetTransparency = r.ReadUInt32();
            CountUvAnimation = r.ReadUInt32();
            OffsetUvAnimation = r.ReadUInt32();
            CountTexReplace = r.ReadUInt32();
            OffsetTexReplace = r.ReadUInt32();
            CountRenderFlags = r.ReadUInt32();
            OffsetRenderFlags = r.ReadUInt32();
            CountBoneLookup = r.ReadUInt32();
            OffsetBoneLookup = r.ReadUInt32();
            CountTexLookup = r.ReadUInt32();
            OffsetTexLookup = r.ReadUInt32();
            CountTexUnits = r.ReadUInt32();
            OffsetTexUnits = r.ReadUInt32();
            CountTransLookup = r.ReadUInt32();
            OffsetTransLookup = r.ReadUInt32();
            CountUvAnimLookup = r.ReadUInt32();
            OffsetUvAnimLookup = r.ReadUInt32();
            VertexBox = new Vector3[2];
            VertexBox[0] = Vector3Helper.Read(s);
            VertexBox[1] = Vector3Helper.Read(s);
            VertexRadius = r.ReadSingle();
            BoundingBox = new Vector3[2];
            BoundingBox[0] = Vector3Helper.Read(s);
            BoundingBox[1] = Vector3Helper.Read(s);
            BoundingRadius = r.ReadSingle();
            CountBoundingTriangles = r.ReadUInt32();
            OffsetBoundingTriangles = r.ReadUInt32();
            CountBoundingVertices = r.ReadUInt32();
            OffsetBoundingVertices = r.ReadUInt32();
            CountBoundingNormals = r.ReadUInt32();
            OffsetBoundingNormals = r.ReadUInt32();
        }
    }

}