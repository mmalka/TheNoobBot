using System.Diagnostics;
using System.IO;
using meshReader.Helper;
using SlimDX;

namespace meshReader.Game.WMO
{
    public class WorldModelGroup
    {
        public ChunkedData Data { get; private set; }
        public ChunkedData SubData { get; private set; }

        public int GroupIndex { get; private set; }
        public Vector3[] BoundingBox;
        public uint Flags { get; private set; }
        public byte[] TriangleFlags { get; private set; }
        public byte[] TriangleMaterials { get; private set; }
        public Triangle<ushort>[] Triangles { get; private set; }
        public Vector3[] Vertices { get; private set; }
        public Vector3[] Normals { get; private set; }

        public bool HasLiquidData { get; private set; }
        public LiquidHeader LiquidDataHeader { get; private set; }
        public LiquidData LiquidDataGeometry { get; private set; }
        public static System.Collections.Generic.List<uint> groupFileIds { get; private set; }

        public WorldModelGroup(string path, int groupIndex)
        {
            uint fileId;
            uint.TryParse(path, out fileId);
            if (fileId <= 0)
            {
                Data = new ChunkedData(path);
            }
            else
                Data = new ChunkedData(fileId);
            GroupIndex = groupIndex;

            var mainChunk = Data.GetChunkByName("MOGP");
            var firstSub = mainChunk.FindSubChunkOffset("MOPY");
            if (firstSub == -1)
                return;
            var stream = mainChunk.GetStream();
            stream.Seek(firstSub, SeekOrigin.Begin);
            SubData = new ChunkedData(stream, (int) (mainChunk.Length - firstSub));

            ReadBoundingBox();
            ReadMaterials();
            ReadTriangles();
            ReadVertices();
            ReadNormals();
            ReadLiquid();
        }

        private void ReadNormals()
        {
            var chunk = SubData.GetChunkByName("MONR");
            if (chunk == null)
                return;

            var normalCount = (int) (chunk.Length / 12);
            Debug.Assert(normalCount == Vertices.Length);
            Normals = new Vector3[normalCount];
            var stream = chunk.GetStream();
            for (int i = 0; i < normalCount; i++)
                Normals[i] = Vector3Helper.Read(stream);
        }

        private void ReadLiquid()
        {
            var chunk = SubData.GetChunkByName("MLIQ");
            if (chunk == null)
                return;

            HasLiquidData = true;
            var stream = chunk.GetStream();
            LiquidDataHeader = LiquidHeader.Read(stream);
            LiquidDataGeometry = LiquidData.Read(stream, LiquidDataHeader);
        }

        public class LiquidData
        {
            public float[,] HeightMap;
            public byte[,] RenderFlags;

            public bool ShouldRender(int x, int y)
            {
                return RenderFlags[x, y] != 0x0F;
            }

            public static LiquidData Read(Stream s, LiquidHeader header)
            {
                var r = new BinaryReader(s);
                var ret = new LiquidData
                {
                    HeightMap = new float[header.CountXVertices, header.CountYVertices],
                    RenderFlags = new byte[header.Width, header.Height]
                };

                for (int y = 0; y < header.CountYVertices; y++)
                {
                    for (int x = 0; x < header.CountXVertices; x++)
                    {
                        r.ReadUInt32();
                        ret.HeightMap[x, y] = r.ReadSingle();
                    }
                }

                for (int y = 0; y < header.Height; y++)
                {
                    for (int x = 0; x < header.Width; x++)
                    {
                        ret.RenderFlags[x, y] = r.ReadByte();
                    }
                }

                return ret;
            }
        }

        public class LiquidHeader
        {
            public uint CountXVertices;
            public uint CountYVertices;
            public uint Width;
            public uint Height;
            public Vector3 BaseLocation;
            public ushort MaterialId;

            public static LiquidHeader Read(Stream s)
            {
                var ret = new LiquidHeader();
                var r = new BinaryReader(s);
                ret.CountXVertices = r.ReadUInt32();
                ret.CountYVertices = r.ReadUInt32();
                ret.Width = r.ReadUInt32();
                ret.Height = r.ReadUInt32();
                ret.BaseLocation = Vector3Helper.Read(s);
                ret.MaterialId = r.ReadUInt16();
                return ret;
            }
        }

        private void ReadVertices()
        {
            var chunk = SubData.GetChunkByName("MOVT");
            if (chunk == null)
                return;

            var verticeCount = (int) (chunk.Length / 12);
            Vertices = new Vector3[verticeCount];
            var stream = chunk.GetStream();
            for (int i = 0; i < verticeCount; i++)
                Vertices[i] = Vector3Helper.Read(stream);
        }

        private void ReadTriangles()
        {
            var chunk = SubData.GetChunkByName("MOVI");
            if (chunk == null)
                return;

            var triangleCount = (int) (chunk.Length / 6);
            Debug.Assert(triangleCount == TriangleFlags.Length);
            var r = new BinaryReader(chunk.GetStream());
            Triangles = new Triangle<ushort>[triangleCount];
            for (int i = 0; i < triangleCount; i++)
                Triangles[i] = new Triangle<ushort>(TriangleType.Wmo, r.ReadUInt16(), r.ReadUInt16(), r.ReadUInt16());
        }

        private void ReadMaterials()
        {
            var chunk = SubData.GetChunkByName("MOPY");
            if (chunk == null)
                return;

            var stream = chunk.GetStream();
            var triangleCount = (int) (chunk.Length / 2);
            TriangleFlags = new byte[triangleCount];
            TriangleMaterials = new byte[triangleCount];
            for (int i = 0; i < triangleCount; i++)
            {
                TriangleFlags[i] = (byte) stream.ReadByte();
                TriangleMaterials[i] = (byte) stream.ReadByte();
            }
        }

        private void ReadBoundingBox()
        {
            var chunk = Data.GetChunkByName("MOGP");
            if (chunk == null)
                return;

            var stream = chunk.GetStream();
            stream.Seek(8, SeekOrigin.Current);
            var r = new BinaryReader(stream);
            Flags = r.ReadUInt32();
            BoundingBox = new Vector3[2];
            BoundingBox[0] = Vector3Helper.Read(stream);
            BoundingBox[1] = Vector3Helper.Read(stream);
        }
    }
}