using System.Threading;

namespace nManager.Wow.Helpers
{
    public class WDB5MemoryReader
    {
        private static int WowClientDB2__GetIndexAddressByIndex(int a1, int a2, ref short a3)
        {
            // this function read the Index list of the DB2 and return the address of the Index you wanted.
            // I'm not exactly sure why it needs that tho but we can use it to verify (standalone) if an index exists.
            int v3 = a1;
            int v4 = a2;
            int i = 0;
            while (v3 != v4)
            {
                if (i > 100000)
                    break; // security anti freeze
                i++;
                int v5 = v3 + 4*((v4 - v3) >> 2)/2;
                if (Memory.WowMemory.Memory.ReadShort((uint) v5) >= a3)
                    v4 = v3 + 4*((v4 - v3) >> 2)/2;
                else
                    v3 = v5 + 4;
            }
            return v3;
        }

        private static int WowClientDB2__GetIndexAddress2ByIndex(int a1, int a2, int a3)
        {
            // this function does a similar thing to the previous one, but I think for secondary indexes.
            // I have not seen any db2 that would use it.
            int v3 = a1;
            int v4 = a2;
            int i = 0;
            while (v3 != v4)
            {
                if (i > 100000)
                    break; // security anti freeze
                i++;
                int v5 = (v3 + 8*((v4 - v3) >> 3)/2);
                if (Memory.WowMemory.Memory.ReadInt((uint) v5) >= a3)
                    v4 = (v3 + 8*((v4 - v3) >> 3)/2);
                else
                    v3 = (v5 + 8);
            }
            return v3;
        }

        private static int WowClientDB2__GetRowPointerBySecondaryIndex(uint _this, int a2)
        {
            int v2; // esi@1
            int v3; // eax@1
            int result; // eax@3
            if (_this <= 0)
                return 0;
            v2 = (Memory.WowMemory.Memory.ReadInt(_this) + 8*(Memory.WowMemory.Memory.ReadInt(_this) + 1));
            v3 = WowClientDB2__GetIndexAddress2ByIndex(Memory.WowMemory.Memory.ReadInt(_this), v2, a2);
            if (v3 == v2 || Memory.WowMemory.Memory.ReadInt((uint) v3) != a2)
                result = 0;
            else
                result = Memory.WowMemory.Memory.ReadInt((uint) (v3 + 4));
            return result;

            /*
                int v2; // esi@1
                int v3; // eax@1
                int result; // eax@3

                v2 = *(_DWORD *)this + 8 * *((_DWORD *)this + 1);
                v3 = sub_214B30(*(_DWORD *)this, *(_DWORD *)this + 8 * *((_DWORD *)this + 1), &a2);
                if ( v3 == v2 || *(_DWORD *)v3 != a2 )
                result = 0;
                else
                result = *(_DWORD *)(v3 + 4);
                return result;
                }
            */
        }


        private static int WowClientDB2__GetRowPointerByIndex(uint _this, int a2)
        {
            int result;
            uint v2 = _this;
            int v5 = 0;
            short v6 = 0;
            bool goToEnd = false;
            /*Memory.WowMemory.Memory.WriteUInt(Memory.WowMemory.Memory.ReadUInt(v2 + 56), Memory.WowMemory.Memory.ReadUInt(v2 + 56) + 1);
            if (a2 == Memory.WowMemory.Memory.ReadUInt(v2 + 64))
                Memory.WowMemory.Memory.WriteUInt(Memory.WowMemory.Memory.ReadUInt(v2 + 60), Memory.WowMemory.Memory.ReadUInt(v2 + 60) + 1);
            Memory.WowMemory.Memory.WriteInt(Memory.WowMemory.Memory.ReadUInt(v2 + 64), a2);*/

            if (a2 < Memory.WowMemory.Memory.ReadInt(v2 + 24) || a2 > Memory.WowMemory.Memory.ReadInt(v2 + 28))
                goToEnd = true;
            else
            {
                uint v3 = Memory.WowMemory.Memory.ReadUInt(v2 + 76);
                int v4 = a2 - Memory.WowMemory.Memory.ReadShort(v2 + 24);
                short v10 = (short) (a2 - Memory.WowMemory.Memory.ReadShort(v2 + 24));
                v5 = WowClientDB2__GetIndexAddressByIndex(Memory.WowMemory.Memory.ReadInt(v2 + 72), (int) (Memory.WowMemory.Memory.ReadInt(v2 + 72) + 4*v3), ref v10);
                if (v5 == Memory.WowMemory.Memory.ReadInt(v2 + 72) + 4*Memory.WowMemory.Memory.ReadInt(v2 + 76) || Memory.WowMemory.Memory.ReadShort((uint) v5) != v4)
                    goToEnd = true;
            }
            if (!goToEnd)
            {
                v6 = Memory.WowMemory.Memory.ReadShort(((uint) (v5 + 2)));
                if (v6 == -2)
                    return 0;
            }
            if (v6 == -1 || goToEnd)
            {
                result = WowClientDB2__GetRowPointerBySecondaryIndex(Memory.WowMemory.Memory.ReadUInt(_this + 84), a2);
            }
            else
            {
                short v8 = Memory.WowMemory.Memory.ReadShort(v2 + 32);
                if (v6 >= v8)
                {
                    short v9 = (short) (v6 - v8);
                    if ((ushort) v9 >= Memory.WowMemory.Memory.ReadInt(v2 + 44))
                        return 0;
                    result = (Memory.WowMemory.Memory.ReadInt(v2 + 8) + v9*Memory.WowMemory.Memory.ReadInt(v2 + 20));
                }
                else
                {
                    result = (Memory.WowMemory.Memory.ReadInt(v2 + 4) + Memory.WowMemory.Memory.ReadInt(v2 + 16)*v6);
                }
            }
            return result;

            /*
                v2 = this;
                ++*(_DWORD *)(v2 + 56);
                if ( a2 == *(_DWORD *)(this + 64) )
                    ++*(_DWORD *)(this + 60);
                *(_DWORD *)(this + 64) = a2;
               
                if ( a2 < *(_DWORD *)(this + 24)
                    || a2 > *(_DWORD *)(this + 28)
                    || (v3 = *(_DWORD *)(this + 76),
                        v4 = a2 - *(_WORD *)(v2 + 24),
                        v10 = (unsigned __int16)(a2 - *(_WORD *)(v2 + 24)),
                        v5 = sub_214AF6(*(_DWORD *)(v2 + 72), *(_DWORD *)(v2 + 72) + 4 * v3, &v10),
                        v5 == *(_DWORD *)(v2 + 72) + 4 * *(_DWORD *)(v2 + 76))
                    || *(_WORD *)v5 != v4 )
                    goto LABEL_17;
                v6 = *(_WORD *)(v5 + 2);
                if ( (_WORD)v6 == -2 )
                    return 0;
                if ( (_WORD)v6 == -1 )
                {
                LABEL_17:
                    result = sub_216ECB(a2);
                }
                else
                {
                    v8 = *(_WORD *)(v2 + 32);
                    if ( (unsigned __int16)v6 >= v8 )
                    {
                    v9 = v6 - v8;
                    if ( (unsigned int)v9 >= *(_DWORD *)(v2 + 44) )
                        return 0;
                    result = *(_DWORD *)(v2 + 8) + v9 * *(_DWORD *)(v2 + 20);
                    }
                    else
                    {
                    result = *(_DWORD *)(v2 + 4) + *(_DWORD *)(v2 + 16) * v6;
                    }
                }
                return result;
                }
            */
        }

        private static bool WowClientDB2__CheckRowPointer(uint addressToDB2, int rowPtr)
        {
            return rowPtr > 0 && rowPtr != Memory.WowMemory.Memory.ReadByte(addressToDB2 + 0xDC);
        }

        public static uint WowClientDB2__GetRowPointer(uint db2Offset, int reqId)
        {
            uint addressToDB2 = Memory.WowProcess.WowModule + db2Offset;

            if ((reqId >= 0 || Memory.WowMemory.Memory.ReadByte(Memory.WowMemory.Memory.ReadUInt(addressToDB2 + 0x98) + 0x58) == 1 && reqId != -1))
            {
                if (reqId >= Memory.WowMemory.Memory.ReadUInt(addressToDB2 + 0xC8) && reqId <= Memory.WowMemory.Memory.ReadUInt(addressToDB2 + 0xC4))
                {
                    int rowPtr = WowClientDB2__GetRowPointerByIndex(Memory.WowMemory.Memory.ReadUInt(addressToDB2 + 0xA8), reqId);
                    if (rowPtr > 0)
                    {
                        if (WowClientDB2__CheckRowPointer(Memory.WowMemory.Memory.ReadUInt(addressToDB2), rowPtr))
                            return (uint) rowPtr;
                    }
                }
            }
            else
            {
                // rare case where the first try at looking for ID fails
                // more things to dissect, but not yet interessting.
            }
            return 0;
        }
    }
}