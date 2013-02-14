using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using System.Linq;

namespace meshReader.Game.ADT
{

    public class LiquidHandler
    {
        public ADT Source { get; private set; }
        public List<Vector3> Vertices { get; private set; }
        public List<Triangle<uint>> Triangles { get; private set; }
        public MCNKLiquidData[] MCNKData { get; private set; }
 
        public LiquidHandler(ADT source)
        {
            Source = source;

            HandleNewLiquid();
        }

        private void HandleNewLiquid()
        {
            var chunk = Source.Data.GetChunkByName("MH2O");
            if (chunk == null)
                return;

            Vertices = new List<Vector3>(1000);
            Triangles = new List<Triangle<uint>>(1000);

            var stream = chunk.GetStream();
            var header = new H2OHeader[256];
            MCNKData = new MCNKLiquidData[256];
            for (int i = 0; i < header.Length; i++)
                header[i] = H2OHeader.Read(stream);

            for (int i = 0; i < header.Length; i++)
            {
                var h = header[i];
                if (h.LayerCount == 0)
                    continue;

                stream.Seek(chunk.Offset + h.OffsetInformation, SeekOrigin.Begin);
                var information = H2OInformation.Read(stream);

                #region Get RenderMask and set heights
                var heights = new float[9, 9];
                H2ORenderMask renderMask;
                if (information.LiquidType != 2)
                {
                    stream.Seek(chunk.Offset + h.OffsetRender, SeekOrigin.Begin);
                    renderMask = H2ORenderMask.Read(stream);

                    if ((renderMask.Mask.All(b => b == 0) || (information.Width == 8 && information.Height == 8)) && information.OffsetMask2 != 0)
                    {
                        stream.Seek(chunk.Offset + information.OffsetMask2, SeekOrigin.Begin);
                        var altMask = new byte[(int)Math.Ceiling(information.Width * information.Height / 8.0f)];
                        stream.Read(altMask, 0, altMask.Length);

                        for (int mi = 0; mi < altMask.Length; mi++)
                            renderMask.Mask[mi + information.OffsetY] |= altMask[mi];
                    }

                    stream.Seek(chunk.Offset + information.OffsetHeightmap, SeekOrigin.Begin);
                    var reader = new BinaryReader(stream);
                    for (int y = information.OffsetY; y < (information.OffsetY + information.Height); y++)
                    {
                        for (int x = information.OffsetX; x < (information.OffsetX + information.Width); x++)
                        {
                            heights[x, y] = reader.ReadSingle();
                        }
                    }
                }
                else
                {
                    // ocean
                    renderMask = new H2ORenderMask
                                     {
                                         Mask = new byte[] {0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF}
                                     };

                    for (int y = 0; y < 9; y++)
                        for (int x = 0; x < 9; x++)
                            heights[x, y] = information.HeightLevel1;
                }
                #endregion
                
                MCNKData[i] = new MCNKLiquidData {Heights = heights, Mask = renderMask};

                #region Create Vertices + Triangles
                for (int y = information.OffsetY; y < (information.OffsetY + information.Height); y++)
                {
                    for (int x = information.OffsetX; x < (information.OffsetX + information.Width); x++)
                    {
                        if (!renderMask.ShouldRender(x, y))
                            continue;

                        var mapChunk = Source.MapChunks[i];
                        var location = mapChunk.Header.Position;
                        location.Y = location.Y - (x*Constant.UnitSize);
                        location.X = location.X - (y*Constant.UnitSize);
                        location.Z = heights[x, y];

                        var vertOffset = (uint)Vertices.Count;
                        Vertices.Add(location);
                        Vertices.Add(new Vector3(location.X - Constant.UnitSize, location.Y, location.Z));
                        Vertices.Add(new Vector3(location.X, location.Y - Constant.UnitSize, location.Z));
                        Vertices.Add(new Vector3(location.X - Constant.UnitSize, location.Y - Constant.UnitSize, location.Z));

                        Triangles.Add(new Triangle<uint>(TriangleType.Water, vertOffset, vertOffset+2, vertOffset+1));
                        Triangles.Add(new Triangle<uint>(TriangleType.Water, vertOffset + 2, vertOffset + 3, vertOffset + 1));
                    }
                }
                #endregion
            }
        }

        private class H2OHeader
        {
            public uint OffsetInformation;
            public uint LayerCount;
            public uint OffsetRender;

            public static H2OHeader Read(Stream s)
            {
                var r = new BinaryReader(s);
                var ret = new H2OHeader
                              {
                                  OffsetInformation = r.ReadUInt32(),
                                  LayerCount = r.ReadUInt32(),
                                  OffsetRender = r.ReadUInt32()
                              };
                return ret;
            }
        }

        public class H2ORenderMask
        {
            public byte[] Mask;

            public bool ShouldRender(int x, int y)
            {
                return (Mask[y] >> x & 1) != 0;
            }

            public static H2ORenderMask Read(Stream s)
            {
                var r = new BinaryReader(s);
                var ret = new H2ORenderMask {Mask = r.ReadBytes(8)};
                return ret;
            }
        }

        private class H2OInformation
        {
            public ushort LiquidType;
            public ushort Flags;
            public float HeightLevel1;
            public float HeightLevel2;
            public byte OffsetX;
            public byte OffsetY;
            public byte Width;
            public byte Height;
            public uint OffsetMask2;
            public uint OffsetHeightmap;

            public static H2OInformation Read(Stream s)
            {
                var r = new BinaryReader(s);
                var ret = new H2OInformation
                              {
                                  LiquidType = r.ReadUInt16(),
                                  Flags = r.ReadUInt16(),
                                  HeightLevel1 = r.ReadSingle(),
                                  HeightLevel2 = r.ReadSingle(),
                                  OffsetX = r.ReadByte(),
                                  OffsetY = r.ReadByte(),
                                  Width = r.ReadByte(),
                                  Height = r.ReadByte(),
                                  OffsetMask2 = r.ReadUInt32(),
                                  OffsetHeightmap = r.ReadUInt32()
                              };
                return ret;
            }
        }

        public class MCNKLiquidData
        {
            public const float MaxStandableHeight = 1.5f;

            public float[,] Heights;
            public H2ORenderMask Mask;

            public bool IsWater(int x, int y, float height)
            {
                if (Heights == null || Mask == null)
                    return false;
                if (!Mask.ShouldRender(x, y))
                    return false;
                var diff = Heights[x, y] - height;
                if (diff > MaxStandableHeight)
                    return true;
                return false;
            }
        }

    }

}