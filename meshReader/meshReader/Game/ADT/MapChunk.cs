using System;
using System.Collections.Generic;
using System.IO;
using meshReader.Helper;
using Microsoft.Xna.Framework;

namespace meshReader.Game.ADT
{

    public class MapChunk
    {
        public ADT ADT { get; private set; }
        public Chunk Source { get; private set; }
        public MapChunkHeader Header { get; private set; }
        public Vector3[] Vertices { get; private set; }
        public List<Triangle<byte>> Triangles { get; private set; }

        public int Index
        {
            get
            {
                return (int)(Header.IndexX + (16*Header.IndexY));
            }
        }

        public MapChunk(ADT adt, Chunk chunk)
        {
            ADT = adt;
            Source = chunk;
            var stream = chunk.GetStream();
            Header = new MapChunkHeader();
            Header.Read(stream);

            stream.Seek(chunk.Offset, SeekOrigin.Begin);
            GenerateVertices(stream);
        }

        public void GenerateTriangles()
        {
            Triangles = new List<Triangle<byte>>(64 * 4);
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    if (HasHole(Header.Holes, x / 2, y / 2))
                        continue;

                    var topLeft = (byte)((17*y) + x);
                    var topRight = (byte)((17*y) + x + 1);
                    var bottomLeft = (byte)((17*(y + 1)) + x);
                    var bottomRight = (byte)((17*(y + 1)) + x + 1);
                    var center = (byte)((17*y) + 9 + x);

                    var triangleType = TriangleType.Terrain;
                    if (ADT.LiquidHandler != null && ADT.LiquidHandler.MCNKData != null)
                    {
                        var data = ADT.LiquidHandler.MCNKData[Index];
                        var maxHeight = Math.Max(
                            Math.Max(
                                Math.Max(Math.Max(Vertices[topLeft].Z, Vertices[topRight].Z), Vertices[bottomLeft].Z),
                                Vertices[bottomRight].Z), Vertices[center].Z);
                        if (data != null && data.IsWater(x, y, maxHeight))
                            triangleType = TriangleType.Water;
                    }

                    Triangles.Add(new Triangle<byte>(triangleType, topRight, topLeft, center));
                    Triangles.Add(new Triangle<byte>(triangleType, topLeft, bottomLeft, center));
                    Triangles.Add(new Triangle<byte>(triangleType, bottomLeft, bottomRight, center));
                    Triangles.Add(new Triangle<byte>(triangleType, bottomRight, topRight, center));
                }
            }
        }

        private void GenerateVertices(Stream s)
        {
            s.Seek(Header.OffsetMCVT, SeekOrigin.Current);

            int vertIndex = 0;
            Vertices = new Vector3[145];
            var reader = new BinaryReader(s);
            for (int j = 0; j < 17; j++)
            {
                int values = j%2 != 0 ? 8 : 9;
                for (int i = 0; i < values; i++)
                {
                    var vertice = new Vector3
                                      {
                                          X = Header.Position.X - (j*(Constant.UnitSize*0.5f)),
                                          Y = Header.Position.Y - (i*Constant.UnitSize),
                                          Z = Header.Position.Z + reader.ReadSingle()
                                      };

                    if (values == 8)
                        vertice.Y -= Constant.UnitSize*0.5f;

                    Vertices[vertIndex++] = vertice;
                }
            }
        }

        private static bool HasHole(uint map, int x, int y)
        {
            return ((map & 0x0000FFFF) & ((1 << x) << (y << 2))) > 0;
        }

        public class MapChunkHeader
        {
            public uint Flags;
            public uint IndexX;
            public uint IndexY;
            public uint Layers;
            public uint DoodadRefs;
            public uint OffsetMCVT;
            public uint OffsetMCNR;
            public uint OffsetMCLY;
            public uint OffsetMCRF;
            public uint OffsetMCAL;
            public uint SizeMCAL;
            public uint OffsetMCSH;
            public uint SizeMCSH;
            public uint AreaId;
            public uint MapObjectRefs;
            public uint Holes;
            public uint[] LowQualityTextureMap;
            public uint PredTex;
            public uint NumberEffectDoodad;
            public uint OffsetMCSE;
            public uint SoundEmitters;
            public uint OffsetMCLQ;
            public uint SizeMCLQ;
            public Vector3 Position;
            public uint OffsetMCCV;

            public void Read(Stream s)
            {
                var r = new BinaryReader(s);
                Flags = r.ReadUInt32();
                IndexX = r.ReadUInt32();
                IndexY = r.ReadUInt32();
                Layers = r.ReadUInt32();
                DoodadRefs = r.ReadUInt32();
                OffsetMCVT = r.ReadUInt32();
                OffsetMCNR = r.ReadUInt32();
                OffsetMCLY = r.ReadUInt32();
                OffsetMCRF = r.ReadUInt32();
                OffsetMCAL = r.ReadUInt32();
                SizeMCAL = r.ReadUInt32();
                OffsetMCSH = r.ReadUInt32();
                SizeMCSH = r.ReadUInt32();
                AreaId = r.ReadUInt32();
                MapObjectRefs = r.ReadUInt32();
                Holes = r.ReadUInt32();
                LowQualityTextureMap = new uint[4];
                for (int i = 0; i < 4; i++)
                    LowQualityTextureMap[i] = r.ReadUInt32();
                PredTex = r.ReadUInt32();
                NumberEffectDoodad = r.ReadUInt32();
                OffsetMCSE = r.ReadUInt32();
                SoundEmitters = r.ReadUInt32();
                OffsetMCLQ = r.ReadUInt32();
                SizeMCLQ = r.ReadUInt32();
                Position = Vector3Helper.Read(s);
                OffsetMCCV = r.ReadUInt32();
            }
        }
    }

}