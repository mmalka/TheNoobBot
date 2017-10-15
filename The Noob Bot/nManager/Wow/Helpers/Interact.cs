using System;
using System.IO;
using System.Threading;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.Enums;
using nManager.Wow.Patchables;
using nManager.Wow.ObjectManager;

namespace nManager.Wow.Helpers
{
    public class Interact
    {
        private static Spell _stealth;
        private static bool _firstRun = true;

        public static void InteractWith(uint baseAddress, bool stopMove = false)
        {
            try
            {
                if (!Usefuls.InGame || Usefuls.IsLoading)
                    return;
                Helpful.Timer timer = new Helpful.Timer(2000);
                while (ObjectManager.ObjectManager.Me.GetBaseAddress == 0)
                {
                    if (timer.IsReady)
                    {
                        return;
                    }
                    Thread.Sleep(200);
                }
                if (_firstRun)
                {
                    if (ObjectManager.ObjectManager.Me.WowClass == WoWClass.Druid)
                        _stealth = new Spell("Prowl");
                    if (ObjectManager.ObjectManager.Me.WowClass == WoWClass.Rogue)
                        _stealth = new Spell("Stealth");
                    _firstRun = false;
                }
                if (_stealth != null && _stealth.KnownSpell)
                {
                    foreach (var aura in ObjectManager.ObjectManager.Me.UnitAuras.Auras)
                    {
                        if (_stealth.Ids.Contains(aura.AuraSpellId))
                        {
                            aura.TryCancel();
                            break;
                        }
                    }
                }
                Usefuls.UpdateLastHardwareAction();
                if (baseAddress > 0)
                {
                    WoWObject to = new WoWObject(baseAddress);
                    if (!to.IsValid)
                        return;
                    if (to.Guid <= 0)
                        return;

                    uint codecaveGUID = Memory.WowMemory.Memory.AllocateMemory(0x10);
                    Memory.WowMemory.Memory.WriteBytes(codecaveGUID, to.Guid.ToByteArray());

                    string[] asm = new[]
                    {
                        /*"call " +
                        (Memory.WowProcess.WowModule +
                         (uint) Addresses.FunctionWow.ClntObjMgrGetActivePlayer)
                        ,
                        "test eax, eax",
                        "je @out",*/
                        /*"call " +
                        (Memory.WowProcess.WowModule +
                         (uint) Addresses.FunctionWow.ClntObjMgrGetActivePlayerObj),
                        "test eax, eax",
                        "je @out",*/
                        "push " + codecaveGUID,
                        "mov ecx, " + ObjectManager.ObjectManager.Me.GetBaseAddress,
                        "call " + (Memory.WowProcess.WowModule + (uint) Addresses.FunctionWow.CGUnit_C__Interact),
                        "add esp, 4",
                        "@out:",
                        "retn"
                    };

                    Memory.WowMemory.InjectAndExecute(asm);

                    Memory.WowMemory.Memory.FreeMemory(codecaveGUID);
                    if (stopMove)
                        MovementManager.StopMove();
                    Thread.Sleep(Usefuls.Latency);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("InteractGameObject(uint baseAddress): " + exception);
            }
        }

        public static void InteractWithBeta(uint baseAddress)
        {
            Logging.WriteError("public static void InteractWithBeta(uint baseAddress) is depreciated.");
            InteractWith(baseAddress);
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