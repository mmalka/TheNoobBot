using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using nManager.Helpful;
using nManager.Wow.Class;

namespace nManager.Wow.Helpers
{
    public class DBC<T> where T : struct
    {
        private readonly DBCStruct.WoWClientDB m_header;
        private readonly Dictionary<int, T> m_rows;

        public int MinIndex
        {
            get { return m_header.minIndex; }
        }

        public int MaxIndex
        {
            get { return m_header.maxIndex; }
        }

        public int NumRows
        {
            get { return m_header.numRows; }
        }

        public string String(uint address)
        {
            return (string)Memory.WowMemory.Memory.ReadUTF8String(address);
        }

        public Dictionary<int, T> Rows
        {
            get { return m_rows; }
        }

        public T this[int index]
        {
            get { return m_rows[index]; }
        }

        public bool HasRow(int index)
        {
            return m_rows.ContainsKey(index);
        }

        /// <summary>
        /// Initializes a new instance of DBC class using specified memory address
        /// </summary>
        /// <param name="offset">DBC memory address</param>
        public DBC(uint offset)
        {
            try
            {
                m_header =
                    (DBCStruct.WoWClientDB)
                    Memory.WowMemory.Memory.ReadObject(Memory.WowProcess.WowModule + offset,
                                                       typeof (DBCStruct.WoWClientDB));

                m_rows = new Dictionary<int, T>(m_header.numRows);

                for (var i = 0; i < m_header.numRows; ++i)
                {
                    uint rowOffset = m_header.FirstRow + (uint) (i*Marshal.SizeOf(typeof (T)));

                    var index = Memory.WowMemory.Memory.ReadInt(rowOffset);
                    var row = (T) Memory.WowMemory.Memory.ReadObject(rowOffset, typeof (T));

                    m_rows.Add(index, row);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("DBC(uint offset): " + exception);
            }
        }

        /// <summary>
        /// Returns a specific row from DBC by it's index
        /// </summary>
        /// <param name="index">Row index</param>
        /// <returns>A row of type T</returns>
        public T GetRow(int index)
        {
            try
            {
                if (HasRow(index))
                    return m_rows[index];
                else
                    return default(T);
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetRow(int index): " + exception);
            }
            return default(T);
        }
    }
}