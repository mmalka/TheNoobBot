using System;
using nManager.Helpful;

namespace nManager.Wow.Helpers
{
    public class DBCReading
    {
        public static WoWClientDB GetWoWClientDBByAddress(uint address)
        {
            try
            {
                return (WoWClientDB)Memory.WowMemory.Memory.ReadObject(Memory.WowProcess.WowModule + address, typeof(WoWClientDB));
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWClientDBByAddress(uint address): " + exception);
                return new WoWClientDB();
            }
        }

        public static uint GetAddressByIndex(int index, WoWClientDB woWClientDb)
        {
            try
            {
                if (index >= woWClientDb.MinIndex && index <= woWClientDb.MaxIndex)
                {
                    return Memory.WowMemory.Memory.ReadUInt((uint)woWClientDb.Rows + 4 * (uint)(index - woWClientDb.MinIndex));
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetAddressByIndex(int index, WoWClientDB woWClientDb): " + exception);
            }
            return 0;
        }

        public struct WoWClientDB
        {
            public int VTable;
            public int NumRows;
            public int MaxIndex;
            public int MinIndex;
            public int Data;
            public int FirstRow;
            public int Rows;
        }
    }


}
