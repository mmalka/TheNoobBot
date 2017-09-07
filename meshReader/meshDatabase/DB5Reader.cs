using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace meshDatabase
{
    public class ColumnMeta
    {
        public short Bits;
        public short Offset;
    }

    public class DB6Reader : IClientDBReader
    {
        private const int HeaderSize = 56;
        public const uint DB6FmtSig = 0x36424457; // WDB6
        private List<ColumnMeta> columnMeta;

        public int RecordsCount
        {
            get { return Lookup.Count; }
        }

        public static T ByteToType<T>(BinaryReader reader)
        {
            byte[] bytes = reader.ReadBytes(Marshal.SizeOf(typeof(T)));

            GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            T theStructure = (T) Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();

            return theStructure;
        }

        public int FieldsCount { get; private set; }
        public int RecordSize { get; private set; }
        public int StringTableSize { get; private set; }

        public Dictionary<int, string> StringTable { get; private set; }

        private SortedDictionary<int, byte[]> Lookup = new SortedDictionary<int, byte[]>();

        public List<ColumnMeta> Metaget
        {
            get { return columnMeta; }
        }

        public IEnumerable<BinaryReader> Rows
        {
            get
            {
                foreach (var row in Lookup)
                {
                    yield return new BinaryReader(new MemoryStream(row.Value), Encoding.UTF8);
                }
            }
        }

        public bool IsSparseTable { get; private set; }
        public bool HasIndexTable { get; private set; }
        public uint TableHash { get; private set; }
        public uint LayoutHash { get; private set; }
        public int MinId { get; private set; }
        public int MaxId { get; private set; }
        public int Locale { get; private set; }
        public int CopyTableSize { get; private set; }
        public int IdIndex { get; private set; }
        public int TotalFieldsCount { get; private set; }
        public int CommonDataSize { get; private set; }
        public string FileName { get; private set; }

        public DB6Reader(string fileName)
            : this(new FileStream(fileName, FileMode.Open))
        {
            FileName = fileName;
        }

        public DB6Reader(Stream stream)
            : this(new BinaryReader(stream, Encoding.UTF8))
        {
        }

        public DB6Reader(BinaryReader reader, bool headerOnly = false)
        {
            int recordsCount = ReadHeader(reader);

            if (headerOnly)
                return;

            using (reader)
            {
                long recordsOffset = HeaderSize + (HasIndexTable ? FieldsCount - 1 : FieldsCount) * 4;
                long eof = reader.BaseStream.Length;
                long copyTablePos = eof - CopyTableSize;
                long indexTablePos = copyTablePos - (HasIndexTable ? recordsCount * 4 : 0);
                long stringTablePos = indexTablePos - (IsSparseTable ? 0 : StringTableSize);

                // Index table
                int[] m_indexes = null;

                if (HasIndexTable)
                {
                    reader.BaseStream.Position = indexTablePos;

                    m_indexes = new int[recordsCount];

                    for (int i = 0; i < recordsCount; i++)
                        m_indexes[i] = reader.ReadInt32();
                }

                if (IsSparseTable)
                {
                    // Records table
                    reader.BaseStream.Position = StringTableSize;

                    int ofsTableSize = MaxId - MinId + 1;

                    for (int i = 0; i < ofsTableSize; i++)
                    {
                        int offset = reader.ReadInt32();
                        int length = reader.ReadInt16();

                        if (offset == 0 || length == 0)
                            continue;

                        int id = MinId + i;

                        long oldPos = reader.BaseStream.Position;

                        reader.BaseStream.Position = offset;

                        byte[] recordBytes = reader.ReadBytes(length);

                        byte[] newRecordBytes = new byte[recordBytes.Length + 4];

                        Array.Copy(BitConverter.GetBytes(id), newRecordBytes, 4);
                        Array.Copy(recordBytes, 0, newRecordBytes, 4, recordBytes.Length);

                        Lookup.Add(id, newRecordBytes);

                        reader.BaseStream.Position = oldPos;
                    }
                }
                else
                {
                    // Records table
                    reader.BaseStream.Position = recordsOffset;

                    for (int i = 0; i < recordsCount; i++)
                    {
                        reader.BaseStream.Position = recordsOffset + i * RecordSize;

                        byte[] recordBytes = reader.ReadBytes(RecordSize);

                        if (HasIndexTable)
                        {
                            byte[] newRecordBytes = new byte[RecordSize + 4];

                            Array.Copy(BitConverter.GetBytes(m_indexes[i]), newRecordBytes, 4);
                            Array.Copy(recordBytes, 0, newRecordBytes, 4, recordBytes.Length);

                            Lookup.Add(m_indexes[i], newRecordBytes);
                        }
                        else
                        {
                            int numBytes = (32 - columnMeta[IdIndex].Bits) >> 3;
                            int offset = columnMeta[IdIndex].Offset;
                            int id = 0;

                            for (int j = 0; j < numBytes; j++)
                                id |= (recordBytes[offset + j] << (j * 8));

                            Lookup.Add(id, recordBytes);
                        }
                    }

                    // Strings table
                    reader.BaseStream.Position = stringTablePos;

                    StringTable = new Dictionary<int, string>();

                    while (reader.BaseStream.Position != stringTablePos + StringTableSize)
                    {
                        int index = (int) (reader.BaseStream.Position - stringTablePos);
                        StringTable[index] = reader.ReadStringNull();
                    }
                }

                // Copy index table
                if (copyTablePos != reader.BaseStream.Length && CopyTableSize != 0)
                {
                    reader.BaseStream.Position = copyTablePos;

                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        int id = reader.ReadInt32();
                        int idcopy = reader.ReadInt32();

                        byte[] copyRow = Lookup[idcopy];
                        byte[] newRow = new byte[copyRow.Length];
                        Array.Copy(copyRow, newRow, newRow.Length);
                        Array.Copy(BitConverter.GetBytes(id), newRow, 4);

                        Lookup.Add(id, newRow);
                    }
                }
            }
        }

        public int ReadHeader(BinaryReader reader)
        {
            if (reader.BaseStream.Length < HeaderSize)
            {
                throw new InvalidDataException(string.Format("File {0} is corrupted!", FileName));
            }

            if (reader.ReadUInt32() != DB6FmtSig)
            {
                throw new InvalidDataException(string.Format("File {0} isn't valid DB2 file!", FileName));
            }

            int recordsCount = reader.ReadInt32();
            FieldsCount = reader.ReadInt32();
            RecordSize = reader.ReadInt32();
            StringTableSize = reader.ReadInt32(); // also offset for sparse table

            TableHash = reader.ReadUInt32();
            LayoutHash = reader.ReadUInt32(); // 21737: changed from build number to layoutHash

            MinId = reader.ReadInt32();
            MaxId = reader.ReadInt32();
            Locale = reader.ReadInt32();
            CopyTableSize = reader.ReadInt32();
            ushort flags = reader.ReadUInt16();
            IdIndex = reader.ReadUInt16();
            TotalFieldsCount = reader.ReadInt32();
            CommonDataSize = reader.ReadInt32();

            IsSparseTable = (flags & 0x1) != 0;
            HasIndexTable = (flags & 0x4) != 0;

            columnMeta = new List<ColumnMeta>();

            for (int i = 0; i < FieldsCount; i++)
            {
                columnMeta.Add(new ColumnMeta() {Bits = reader.ReadInt16(), Offset = (short) (reader.ReadInt16() + (HasIndexTable ? 4 : 0))});
            }

            if (HasIndexTable)
            {
                FieldsCount++;
                columnMeta.Insert(0, new ColumnMeta());
            }

            return recordsCount;
        }

        public void Save(DataTable table, Table def, string path)
        {

        }
    }
}