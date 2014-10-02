using System;
using System.Collections.Generic;
using nManager.Wow.Class;

namespace nManager.Wow.Helpers
{
    public class DBCReader<T> : IEnumerable<T> where T : struct
    {
        private readonly DBCStruct.WoWClientDB m_dbInfo;
        private readonly DBCStruct.DBCFile m_fileHdr;
        public int MinIndex { get { return m_dbInfo.MinIndex; } }
        public int MaxIndex { get { return m_dbInfo.MaxIndex; } }
        public int NumRows { get { return m_dbInfo.NumRows; } }
        public T this[int index] { get { return Memory.WowMemory.Memory.ReadT<T>((uint) GetRowPtr(index)); } }
        public bool HasRow(int index) { return GetRowPtr(index) != IntPtr.Zero; }
        /// <summary>
        /// Initializes a new instance of DBC class using specified memory address
        /// </summary>
        /// <param name="dbcBase">DBC memory address</param>
        public DBCReader(IntPtr dbcBase)
        {
            IntPtr addr = dbcBase + (int) Memory.WowProcess.WowModule; // Memory.WowMemory.Memory.Rebase(dbcBase);
            m_dbInfo = (DBCStruct.WoWClientDB) Memory.WowMemory.Memory.ReadObject((uint) addr, typeof (DBCStruct.WoWClientDB));
            m_fileHdr = (DBCStruct.DBCFile) Memory.WowMemory.Memory.ReadObject((uint) m_dbInfo.Data, typeof (DBCStruct.DBCFile));
        }
        private IntPtr GetRowPtr(int index)
        {
            if (index < MinIndex || index > MaxIndex)
                return IntPtr.Zero;
            var actualIndex = index - MinIndex;
            int v5 = Memory.WowMemory.Memory.ReadInt((uint) (m_dbInfo.Unk1 + (4 * (actualIndex >> 5))));
            int a2a = (int)(actualIndex & 0x1Fu);
            if (((1 << a2a) & v5) != 0)
            {
                int bitsSet = CountBitsSet(v5 << (31 - a2a));
                int entry = bitsSet + GetArrayEntryBySizeType(m_dbInfo.Unk3, actualIndex >> 5) - 1;
                if (m_dbInfo.Unk2 == 0)
                {
                    entry = GetArrayEntryBySizeType(m_dbInfo.Rows, entry);
                }
                return m_dbInfo.FirstRow + m_fileHdr.RecordSize * entry;
            }
            return IntPtr.Zero;
        }
        private int CountBitsSet(int a1)
        {
            return 0x1010101 *
            ((((a1 - ((a1 >> 1) & 0x55555555)) & 0x33333333) +
            (((a1 - ((a1 >> 1) & 0x55555555)) >> 2) & 0x33333333) +
            ((((a1 - ((a1 >> 1) & 0x55555555)) & 0x33333333) +
            (((a1 - ((a1 >> 1) & 0x55555555)) >> 2) & 0x33333333)) >> 4)) & 0x0F0F0F0F) >> 24;
        }
        private int GetArrayEntryBySizeType(IntPtr arrayPtr, int index)
        {
            if (m_dbInfo.RowEntrySize == 2)
                return Memory.WowMemory.Memory.ReadShort((uint) (arrayPtr + (2 * index)));
            else
                return Memory.WowMemory.Memory.ReadInt((uint) (arrayPtr + (4 * index)));
        }
        public IEnumerator<T> GetEnumerator()
        {
            for (uint i = 0; i < NumRows; ++i)
            {
                yield return Memory.WowMemory.Memory.ReadT<T>((uint) (m_dbInfo.FirstRow + (int) (i*m_fileHdr.RecordSize)));
            }
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}