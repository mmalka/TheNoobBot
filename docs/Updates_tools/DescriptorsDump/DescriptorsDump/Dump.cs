using System;
using System.Diagnostics;

namespace DescriptorsDump
{
    class Dump
    {
        static Magic.BlackMagic _memory;

        internal static string Go(string prefixEnum) { return Go(prefixEnum, true); }
        internal static string Go(bool multiply) { return Go("", multiply); }
        internal static string Go(string prefixEnum, bool multiply)
        {
            try
            {
                
                _memory = new Magic.BlackMagic(Process.GetProcessesByName("WoW")[0].Id);
                if (!_memory.IsProcessOpen)
                    return "Process not open.";

                // Check function
                uint dwStartFunc;
                dwStartFunc = _memory.FindPattern("6A 54 68 00 00 00 00 68 00 00 00 00 E8 00 00 00 00 6A 54", "xxx????x????x????xx"); // Wow > 5.0.0
                if (dwStartFunc <= 0)
                {
                    _memory.Close();
                    return "Descriptors function not found.";
                }
                dwStartFunc = dwStartFunc - 0x46;

                // Get base adresse of all Field.
                uint s_objectDescriptors, s_unitDescriptors, s_itemDescriptors, s_playerDescriptors, s_containerDescriptors, s_gameobjectDescriptors, s_dynamicObjectDescriptors, s_corpseDescriptors, s_areaTriggerDescriptors, s_sceneObjectDescriptors;
                s_objectDescriptors = _memory.ReadUInt(dwStartFunc + 0x1 + 0x48);
                s_itemDescriptors = _memory.ReadUInt(dwStartFunc + 0x1 + 0x6D);
                s_containerDescriptors = _memory.ReadUInt(dwStartFunc + 0x1 + 0x95);
                s_unitDescriptors = _memory.ReadUInt(dwStartFunc + 0x1 + 0xBD);
                s_playerDescriptors = _memory.ReadUInt(dwStartFunc + 0x1 + 0xE5);
                s_gameobjectDescriptors = _memory.ReadUInt(dwStartFunc + 0x1 + 0x10A);
                s_dynamicObjectDescriptors = _memory.ReadUInt(dwStartFunc + 0x1 + 0x12F);
                s_corpseDescriptors = _memory.ReadUInt(dwStartFunc + 0x1 + 0x154);
                s_areaTriggerDescriptors = _memory.ReadUInt(dwStartFunc + 0x1 + 0x176);
                s_sceneObjectDescriptors = _memory.ReadUInt(dwStartFunc + 0x1 + 0x19B);
                if (s_objectDescriptors <= 0 || s_itemDescriptors <= 0 || s_containerDescriptors <= 0 || s_unitDescriptors <= 0 || s_playerDescriptors <= 0 || s_gameobjectDescriptors <= 0 || s_dynamicObjectDescriptors <= 0 || s_corpseDescriptors <= 0 || s_areaTriggerDescriptors <= 0 || s_sceneObjectDescriptors <= 0)
                {
                    _memory.Close();
                    return "Field not found.";
                }

                // Get Field
                string retVal = "";
                uint objectEnd;

                retVal = retVal + DumpField("ObjectFields", "CGObjectData::m_", s_objectDescriptors, 0, out objectEnd, prefixEnum, multiply);
                retVal = retVal + DumpField("ItemFields", "CGItemData::m_", s_itemDescriptors, objectEnd, out objectEnd, prefixEnum, multiply);
                retVal = retVal + DumpField("ContainerFields", "CGContainerData::m_", s_containerDescriptors, objectEnd, out objectEnd, prefixEnum, multiply);
                retVal = retVal + DumpField("UnitFields", "CGUnitData::", s_unitDescriptors, objectEnd, out objectEnd, prefixEnum, multiply);
                retVal = retVal + DumpField("PlayerFields", "CGPlayerData::", s_playerDescriptors, objectEnd, out objectEnd, prefixEnum, multiply);
                retVal = retVal + DumpField("GameObjectFields", "CGGameObjectData::m_", s_gameobjectDescriptors, objectEnd, out objectEnd, prefixEnum, multiply);
                retVal = retVal + DumpField("DynamicObjectFields", "CGDynamicObjectData::m_", s_dynamicObjectDescriptors, objectEnd, out objectEnd, prefixEnum, multiply);
                retVal = retVal + DumpField("CorpseFields", "CGCorpseData::m_", s_corpseDescriptors, objectEnd, out objectEnd, prefixEnum, multiply);
                retVal = retVal + DumpField("AreaTriggerFields", "CGAreaTriggerData::m_", s_areaTriggerDescriptors, objectEnd, out objectEnd, prefixEnum, multiply);
                retVal = retVal + DumpField("SceneObjecFields", "CGSceneObjectData::m_", s_sceneObjectDescriptors, objectEnd, out objectEnd, prefixEnum, multiply);

                // Dispose Process
                _memory.Close();
                // Return value
                return retVal;
            }
            catch (Exception e)
            {
                return e + Environment.NewLine + Environment.NewLine + " Exception caught.";
            }
        }

        static string DumpField(string szName, string sPrefix, uint dwPointer, uint lastIndex, out uint outLastIndex, string prefixEnum, bool multiply)
        {
            string valueReturn = "";

            valueReturn = valueReturn + "public enum " + prefixEnum + szName + Environment.NewLine + "{" + Environment.NewLine;

            while (true)
            {
                var descriptorStruct = (DescriptorStruct)_memory.ReadObject(dwPointer, typeof (DescriptorStruct));
                // Get name:
                string pszName;
                pszName = _memory.ReadASCIIString(descriptorStruct.pName, 100);
                uint multiplyNum = 1;
                if (multiply)
                    multiplyNum = 4;

                if (pszName == "")
                    break;

                if (!valueReturn.Contains(pszName.Replace(sPrefix, "").Replace(".", "_")) && pszName.Contains(sPrefix))
                {
                    valueReturn = valueReturn + "   " + pszName.Replace(sPrefix, "").Replace(".", "_") + " = 0x" + ((lastIndex) * multiplyNum).ToString("X") +
                                  "," + Environment.NewLine;
                    lastIndex = lastIndex + descriptorStruct.size;
                }

                dwPointer = dwPointer + 0xC;
            }

            valueReturn = valueReturn + "};" + Environment.NewLine + Environment.NewLine;

            outLastIndex = lastIndex;

            return valueReturn;
        }

        public struct DescriptorStruct
        {
            public uint pName;
            public uint size;
            public short unknown;
        }
    }
}
