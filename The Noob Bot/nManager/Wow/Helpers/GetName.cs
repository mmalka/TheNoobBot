using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.Enums;
using nManager.Wow.Patchables;

namespace nManager.Wow.Helpers
{
    internal class GetName
    {
        public static class GetPlayerName
        {
            private static readonly Dictionary<UInt128, string> CachePlayerName = new Dictionary<UInt128, string>();

            public static string GetPlayerNameByGuid(UInt128 guid)
            {
                try
                {
                    if (!Usefuls.InGame || Usefuls.IsLoading)
                        return string.Empty;

                    lock (CachePlayerName)
                    {
                        if (CachePlayerName.ContainsKey(guid))
                            return CachePlayerName[guid];

                        if (CachePlayerName.Count > 30)
                            CachePlayerName.Clear();

                        var pObj = ObjectManager.ObjectManager.GetObjectByGuid(guid);

                        if (pObj.IsValid && pObj.Type == WoWObjectType.Player)
                        {
                            uint pString = Memory.WowMemory.Memory.AllocateMemory(4);
                            var asm = new[]
                            {
                                "push 1",
                                "push 0",
                                "mov ecx, " + pObj.GetBaseAddress,
                                "call " + (Memory.WowProcess.WowModule + (uint) Addresses.FunctionWow.CGUnit_C__GetUnitName),
                                "mov [" + pString + "], eax",
                                "test eax, eax",
                                "je @out",
                                "push eax",
                                "call " +
                                (Memory.WowProcess.WowModule + (uint) Addresses.FunctionWow.strlen),
                                "add esp, 4",
                                "@out:",
                                "retn"
                            };

                            uint stringLength = Memory.WowMemory.InjectAndExecute(asm);
                            uint retPtr = Memory.WowMemory.Memory.ReadUInt(pString);
                            string sResult = "";
                            if (retPtr > 0)
                                sResult = Memory.WowMemory.Memory.ReadUTF8String(retPtr, (int) stringLength);
                            // Free memory allocated for command
                            Memory.WowMemory.Memory.FreeMemory(pString);

                            if (string.IsNullOrWhiteSpace(sResult))
                                return "";
                            CachePlayerName.Add(guid, sResult);
                            return sResult;
                        }
                    }
                }
                catch (Exception e)
                {
                    Logging.WriteError("GetPlayerNameByGuid(UInt128 guid): " + e);
                }
                return "";
            }
        }
    }
}