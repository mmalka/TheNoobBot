using System;
using System.Collections.Generic;
using System.IO;
using meshReader.Helper;
using SlimDX;
using System.Text;

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
                    if (Header.HasHole(x, y))
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
            s.Seek(Header.MCVTDataOffsetComputed, SeekOrigin.Current);
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

        /*private static bool HasHole(uint map, int x, int y)
        {
            return ((map & 0x0000FFFF) & ((1 << x) << (y << 2))) > 0;
        }*/

        [Flags]
        public enum MapChunkHeaderFlags : uint
        {
            HighResHoleMap = 0x10000
        }

        public class MapChunkHeader
        {
            public MapChunkHeaderFlags Flags;
            public uint IndexX;
            public uint IndexY;
            public uint Layers;
            public uint DoodadRefs;
            public uint HighResHoleL;
            public uint HighResHoleH;
            public uint OffsetMCLY;
            public uint OffsetMCRF;
            public uint OffsetMCAL;
            public uint SizeMCAL;
            public uint OffsetMCSH;
            public uint SizeMCSH;
            public uint AreaId;
            public uint MapObjectRefs;
            public ushort LowResHoles;
            public ushort HolesAlign;
            public uint[] LowQualityTextureMap;
            public uint PredTex;
            public uint NumberEffectDoodad;
            public uint OffsetMCSE;
            public uint SoundEmitters;
            public uint OffsetMCLQ;
            public uint SizeMCLQ;
            public Vector3 Position;
            public uint OffsetMCCV;
            public uint OffsetMCLV;
            public uint unused;
            // Computed data
            public byte[] HighResHoles { get { return BitConverter.GetBytes(HighResHoleL + ((ulong)HighResHoleH << 32)); } } // 0x14
            public uint MCVTDataOffsetComputed;

            public void Read(Stream s)
            {
                var r = new BinaryReader(s);
                var startingOffset = s.Position;
                Flags = (MapChunkHeaderFlags)r.ReadUInt32();
                IndexX = r.ReadUInt32();
                IndexY = r.ReadUInt32();
                Layers = r.ReadUInt32();
                DoodadRefs = r.ReadUInt32();
                HighResHoleL = r.ReadUInt32();
                HighResHoleH = r.ReadUInt32();
                OffsetMCLY = r.ReadUInt32();
                OffsetMCRF = r.ReadUInt32();
                OffsetMCAL = r.ReadUInt32();
                SizeMCAL = r.ReadUInt32();
                OffsetMCSH = r.ReadUInt32();
                SizeMCSH = r.ReadUInt32();
                AreaId = r.ReadUInt32();
                MapObjectRefs = r.ReadUInt32();
                LowResHoles = r.ReadUInt16();
                HolesAlign = r.ReadUInt16();
                LowQualityTextureMap = new uint[4];
                for (int i = 0; i < 4; i++)
                    LowQualityTextureMap[i] = r.ReadUInt32();
                PredTex = r.ReadUInt32();
                NumberEffectDoodad = r.ReadUInt32();
                OffsetMCSE = r.ReadUInt32();
                SoundEmitters = r.ReadUInt32();
                OffsetMCLQ = r.ReadUInt32();
                SizeMCLQ = r.ReadUInt32();
                Position = Vector3Helper.Read(r.BaseStream);
                OffsetMCCV = r.ReadUInt32();
                OffsetMCLV = r.ReadUInt32();
                unused = r.ReadUInt32();

                //string sigString = "MCVT";
                //var arr = Encoding.ASCII.GetBytes(sigString);
                //Array.Reverse(arr);
                //uint sigInt = BitConverter.ToUInt32(arr, 0);

                long currentPos = s.Position;
                var sig = r.ReadUInt32();
                var size = r.ReadUInt32();
                while (sig != 0x4D435654 && s.CanRead) // 0x4D435654 = MCVT reversed
                {
                    Console.WriteLine("I had to read more data");
                    s.Position = currentPos + size;
                    currentPos = s.Position;
                    sig = r.ReadUInt32();
                    size = r.ReadUInt32();
                }
                MCVTDataOffsetComputed = (uint)(s.Position - startingOffset);
            }
            public byte[] Holes
            {
                get
                {
                    if (Flags.HasFlag(MapChunkHeaderFlags.HighResHoleMap))
                        return HighResHoles;
                    else
                        return TransformToHighRes(LowResHoles);
                }
            }
            private static byte[] TransformToHighRes(ushort holes)
            {
                var ret = new byte[8];
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        int holeIdxL = (i / 2) * 4 + (j / 2);
                        if (((holes >> holeIdxL) & 1) == 1)
                            ret[i] |= (byte)(1 << j);
                    }
                }
                return ret;
            }
            public bool HasHole(int col, int row)
            {
                return ((Holes[row] >> col) & 1) == 1;
            }
        }
    }

}