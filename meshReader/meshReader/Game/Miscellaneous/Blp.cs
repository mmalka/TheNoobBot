using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using meshDatabase;

namespace meshReader.Game.Miscellaneous
{
    public class BlpData
    {
        public string Magic;
        public uint Version;
        public byte Compression;
        public byte AlphaDepth;
        public byte OtherAlpha;
        public byte MipLevel;
        public uint Width;
        public uint Height;

        public uint MipCount;
        public uint[] MipOffsets;
        public uint[] MipSizes;

        public byte[][] RawData;
        public uint[] Pallette;

        public BlpData(BinaryReader reader)
        {
            Magic = Encoding.ASCII.GetString(reader.ReadBytes(4));
            Version = reader.ReadUInt32();
            Compression = reader.ReadByte();
            AlphaDepth = reader.ReadByte();
            OtherAlpha = reader.ReadByte();
            MipLevel = reader.ReadByte();
            Width = reader.ReadUInt32();
            Height = reader.ReadUInt32();

            MipOffsets = new uint[0x10];
            for (int i = 0; i < 0x10; i++)
                MipOffsets[i] = reader.ReadUInt32();

            MipCount = 0;
            MipSizes = new uint[0x10];
            for (int i = 0; i < 0x10; i++)
            {
                MipSizes[i] = reader.ReadUInt32();
                if (MipSizes[i] > 0)
                    MipCount++;
            }

            RawData = new byte[MipCount][];
            if (Compression == 1)
            {
                Pallette = new uint[0x100];
                for (int i = 0; i < 0x100; i++)
                    Pallette[i] = reader.ReadUInt32();
            }

            for (int i = 0; i < MipCount; i++)
            {
                RawData[i] = new byte[MipSizes[i]];
                reader.BaseStream.Seek(MipOffsets[i], SeekOrigin.Begin);
                for (int n = 0; n < MipSizes[i]; n++)
                    RawData[i][n] = reader.ReadByte();
            }
        }
    }

    public class Blp
    {
        public BlpData Data { get; private set; }

        private readonly Image[] _cache;

        public Blp(string path)
        {
            using (var reader = new BinaryReader(MpqManager.GetFile(path)))
                Data = new BlpData(reader);
            _cache = new Image[Data.MipCount];
        }

        public Image GetImage(int mipLevel)
        {
            if (_cache[mipLevel] != null)
                return _cache[mipLevel];

            var width = (int) (Data.Width / Math.Pow(2.0, mipLevel));
            var height = (int) (Data.Height / Math.Pow(2.0, mipLevel));
            var source = new byte[(height * width) * 4];
            if (Data.Compression == 1)
            {
                for (int i = 0; i < (height * width); i++)
                {
                    uint num4 = Data.Pallette[Data.RawData[mipLevel][i]];
                    source[i * 4] = (byte) (num4 & 0xff);
                    source[(i * 4) + 1] = (byte) ((num4 & 0xff00) >> 8);
                    source[(i * 4) + 2] = (byte) ((num4 & 0xff0000) >> 0x10);
                    if (Data.AlphaDepth == 8)
                    {
                        source[(i * 4) + 3] = Data.RawData[mipLevel][(width * height) + i];
                    }
                    if (Data.AlphaDepth == 0)
                    {
                        source[(i * 4) + 3] = 0xff;
                    }
                }
                if (Data.AlphaDepth == 1)
                {
                    for (int j = 0; j < ((width * height) / 8); j++)
                    {
                        int num6 = (j * 4) * 8;
                        byte num7 = Data.RawData[mipLevel][(width * height) + j];
                        source[num6 + 3] = (byte) (num7 & 1);
                        source[num6 + 7] = (byte) (num7 & 2);
                        source[num6 + 11] = (byte) (num7 & 4);
                        source[num6 + 15] = (byte) (num7 & 8);
                        source[num6 + 0x13] = (byte) (num7 & 0x10);
                        source[num6 + 0x17] = (byte) (num7 & 0x20);
                        source[num6 + 0x1b] = (byte) (num7 & 0x40);
                        source[num6 + 0x1f] = (byte) (num7 & 0x80);
                        for (int k = 3; k < 0x20; k += 4)
                        {
                            if (source[num6 + k] > 0)
                            {
                                source[num6 + k] = 0xff;
                            }
                            else
                            {
                                source[num6 + k] = 0;
                            }
                        }
                    }
                }
            }
            if (Data.Compression == 2)
            {
                for (int m = 0; m < (height / 4); m++)
                {
                    for (int n = 0; n < (width / 4); n++)
                    {
                        int num15;
                        var numArray = new uint[4];
                        var numArray2 = new uint[4];
                        var numArray3 = new uint[4];
                        var numArray4 = new uint[8];
                        ulong num13 = 0L;
                        ulong num14 = 7L;
                        if (Data.AlphaDepth == 8)
                        {
                            num15 = (((m * (width / 4)) + n) * 0x10) + 8;
                        }
                        else
                        {
                            num15 = ((m * (width / 4)) + n) * 8;
                        }
                        uint num16 = Data.RawData[mipLevel][num15];
                        uint num17 = Data.RawData[mipLevel][num15 + 1];
                        uint num11 = num16 + (num17 << 8);
                        numArray[0] = (num11 & 0xf800) >> 8;
                        numArray2[0] = (num11 & 0x7e0) >> 3;
                        numArray3[0] = (num11 & 0x1f) << 3;
                        uint num18 = Data.RawData[mipLevel][num15 + 2];
                        uint num19 = Data.RawData[mipLevel][num15 + 3];
                        uint num12 = num18 + (num19 << 8);
                        numArray[1] = (num12 & 0xf800) >> 8;
                        numArray2[1] = (num12 & 0x7e0) >> 3;
                        numArray3[1] = (num12 & 0x1f) << 3;
                        if ((num11 > num12) | ((Data.AlphaDepth == 8) && (Data.OtherAlpha == 7)))
                        {
                            numArray[2] = ((2 * numArray[0]) + numArray[1]) / 3;
                            numArray[3] = (numArray[0] + (2 * numArray[1])) / 3;
                            numArray2[2] = ((2 * numArray2[0]) + numArray2[1]) / 3;
                            numArray2[3] = (numArray2[0] + (2 * numArray2[1])) / 3;
                            numArray3[2] = ((2 * numArray3[0]) + numArray3[1]) / 3;
                            numArray3[3] = (numArray3[0] + (2 * numArray3[1])) / 3;
                        }
                        else
                        {
                            numArray[2] = (numArray[0] + numArray[1]) >> 1;
                            numArray3[2] = (numArray3[0] + numArray3[1]) >> 1;
                            numArray2[2] = (numArray2[0] + numArray2[1]) >> 1;
                            numArray[3] = 0xff;
                            numArray2[3] = 0xff;
                            numArray3[3] = 0xff;
                        }
                        if ((Data.AlphaDepth == 8) && (Data.OtherAlpha == 7))
                        {
                            numArray4[0] = Data.RawData[mipLevel][num15 - 8];
                            numArray4[1] = Data.RawData[mipLevel][num15 - 7];
                            for (int num20 = 0; num20 < 7; num20++)
                            {
                                num13 = (num13 << 8) + Data.RawData[mipLevel][num15 - num20];
                            }
                            if (numArray4[0] > numArray4[1])
                            {
                                numArray4[2] = ((6 * numArray4[0]) + numArray4[1]) / 7;
                                numArray4[3] = ((5 * numArray4[0]) + (2 * numArray4[1])) / 7;
                                numArray4[4] = ((4 * numArray4[0]) + (3 * numArray4[1])) / 7;
                                numArray4[5] = ((3 * numArray4[0]) + (4 * numArray4[1])) / 7;
                                numArray4[6] = ((2 * numArray4[0]) + (5 * numArray4[1])) / 7;
                                numArray4[7] = (numArray4[0] + (6 * numArray4[1])) / 7;
                            }
                            else
                            {
                                numArray4[2] = ((4 * numArray4[0]) + numArray4[1]) / 5;
                                numArray4[3] = ((3 * numArray4[0]) + (2 * numArray4[1])) / 5;
                                numArray4[4] = ((2 * numArray4[0]) + (3 * numArray4[1])) / 5;
                                numArray4[5] = (numArray4[0] + (4 * numArray4[1])) / 5;
                                numArray4[6] = 0;
                                numArray4[7] = 0xff;
                            }
                        }
                        for (int num21 = 0; num21 < 4; num21++)
                        {
                            int index = ((((m * 4) + num21) * width) * 4) + (n * 0x10);
                            var buffer2 = new[]
                            {
                                (byte) (Data.RawData[mipLevel][(num15 + 4) + num21] & 3), (byte) ((Data.RawData[mipLevel][(num15 + 4) + num21] & 12) >> 2),
                                (byte) ((Data.RawData[mipLevel][(num15 + 4) + num21] & 0x30) >> 4), (byte) ((Data.RawData[mipLevel][(num15 + 4) + num21] & 0xc0) >> 6)
                            };
                            source[index] = (byte) numArray3[buffer2[0]];
                            source[index + 1] = (byte) numArray2[buffer2[0]];
                            source[index + 2] = (byte) numArray[buffer2[0]];
                            source[index + 3] = 0xff;
                            source[index + 4] = (byte) numArray3[buffer2[1]];
                            source[index + 5] = (byte) numArray2[buffer2[1]];
                            source[index + 6] = (byte) numArray[buffer2[1]];
                            source[index + 7] = 0xff;
                            source[index + 8] = (byte) numArray3[buffer2[2]];
                            source[index + 9] = (byte) numArray2[buffer2[2]];
                            source[index + 10] = (byte) numArray[buffer2[2]];
                            source[index + 11] = 0xff;
                            source[index + 12] = (byte) numArray3[buffer2[3]];
                            source[index + 13] = (byte) numArray2[buffer2[3]];
                            source[index + 14] = (byte) numArray[buffer2[3]];
                            source[index + 15] = 0xff;
                            if ((Data.AlphaDepth == 1) && (num11 <= num12))
                            {
                                if (buffer2[0] == 3)
                                {
                                    source[index + 3] = 0;
                                }
                                if (buffer2[1] == 3)
                                {
                                    source[index + 7] = 0;
                                }
                                if (buffer2[2] == 3)
                                {
                                    source[index + 11] = 0;
                                }
                                if (buffer2[3] == 3)
                                {
                                    source[index + 15] = 0;
                                }
                            }
                            if (Data.AlphaDepth == 8)
                            {
                                if (Data.OtherAlpha != 7)
                                {
                                    numArray4[0] = (byte) (Data.RawData[mipLevel][num15 - (8 - (num21 * 2))] & 15);
                                    numArray4[1] = (byte) (Data.RawData[mipLevel][num15 - (8 - (num21 * 2))] & 240);
                                    numArray4[2] = (byte) (Data.RawData[mipLevel][num15 - (7 - (num21 * 2))] & 15);
                                    numArray4[3] = (byte) (Data.RawData[mipLevel][num15 - (7 - (num21 * 2))] & 240);
                                    source[index + 3] = (byte) (numArray4[0] | (numArray4[0] << 4));
                                    source[index + 7] = (byte) (numArray4[1] | (numArray4[1] >> 4));
                                    source[index + 11] = (byte) (numArray4[2] | (numArray4[2] << 4));
                                    source[index + 15] = (byte) (numArray4[3] | (numArray4[3] >> 4));
                                }
                                else
                                {
                                    for (int num23 = 0; num23 < 4; num23++)
                                    {
                                        ulong num24 = (num13 & num14) >> (((num21 * 4) + num23) * 3);
                                        source[(index + 3) + (num23 * 4)] = (byte) numArray4[(int) ((IntPtr) (num24 & 7L))];
                                        num14 = num14 << 3;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            var bitmap = new Bitmap(width, height);
            const PixelFormat format = PixelFormat.Format32bppArgb;
            var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            BitmapData bitmapdata = bitmap.LockBits(rect, ImageLockMode.ReadWrite, format);
            IntPtr destination = bitmapdata.Scan0;
            Marshal.Copy(source, 0, destination, source.Length);
            bitmap.UnlockBits(bitmapdata);
            _cache[mipLevel] = bitmap;
            return bitmap;
        }
    }
}