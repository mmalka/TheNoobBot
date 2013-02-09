using System;
using System.IO;
using System.Threading;
using nManager.Helpful;
using nManager.Wow.Patchables;
using nManager.Wow.ObjectManager;

namespace nManager.Wow.Helpers
{
    public class Interact
    {
        public static void InteractGameObject(uint baseAddress)
        {
            try
            {
                if (baseAddress > 0)
                {
                    var to = new WoWObject(baseAddress);
                    if (!to.IsValid)
                        return;
                    if (to.Guid <= 0)
                        return;

                    // GUID (uint64) to uint32 + uint32
                    Stream t = new MemoryStream(8);
                    var ta = BitConverter.GetBytes(to.Guid);
                    t.Write(ta, 0, ta.Length);
                    t.Position = 0;
                    var b = new BinaryReader(t);
                    var p1 = b.ReadUInt32();
                    var p2 = b.ReadUInt32();

                    var asm = new[]
                        {
                            "call " +
                            (Memory.WowProcess.WowModule +
                             (uint) Addresses.FunctionWow.ClntObjMgrGetActivePlayer)
                            ,
                            "test eax, eax",
                            "je @out",
                            "call " +
                            (Memory.WowProcess.WowModule +
                             (uint) Addresses.FunctionWow.ClntObjMgrGetActivePlayerObj),
                            "test eax, eax",
                            "je @out",
                            "push " + p2,
                            "push " + p1,
                            "mov ecx, eax",
                            "call " + (Memory.WowProcess.WowModule + (uint) Addresses.FunctionWow.Interact),
                            "add esp, 8",
                            "@out:",
                            "retn"
                        };

                    Memory.WowMemory.InjectAndExecute(asm);
                    Thread.Sleep(Usefuls.Latency);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("InteractGameObject(uint baseAddress): " + exception);
            }
        }

        public static void Repop()
        {
            try
            {
                Lua.LuaDoString("RepopMe()");
            }
            catch (Exception exception)
            {
                Logging.WriteError("Repop(): " + exception);
            }
        }

        public static void RetrieveCorpse()
        {
            try
            {
                Lua.LuaDoString("RetrieveCorpse()");
            }
            catch (Exception exception)
            {
                Logging.WriteError("RetrieveCorpse(): " + exception);
            }
        }

        public static void SpiritHealerAccept()
        {
            try
            {
                Lua.LuaDoString(
                    "AcceptXPLoss() local f = StaticPopup_Visible local s = f(\"XP_LOSS\") if s then _G[s]:Hide() end s = f(\"XP_LOSS_NO_SICKNESS\") if s then _G[s]:Hide() end");
            }
            catch (Exception exception)
            {
                Logging.WriteError("SpiritHealerAccept(): " + exception);
            }
        }

        public static void TeleportToSpiritHealer()
        {
            try
            {
                Lua.RunMacroText("/click GhostFrame");
            }
            catch (Exception exception)
            {
                Logging.WriteError("TeleportToSpiritHealer(): " + exception);
            }
        }
    }
}