using System;
using System.IO;
using System.Threading;
using nManager.Helpful;
using nManager.Wow.Enums;
using nManager.Wow.Patchables;
using nManager.Wow.ObjectManager;

namespace nManager.Wow.Helpers
{
    public class Interact
    {
        public static void InteractWith(uint baseAddress, bool stopMove = false)
        {
            try
            {
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
                        "call " +
                        (Memory.WowProcess.WowModule +
                         (uint) Addresses.FunctionWow.ClntObjMgrGetActivePlayerObj),
                        "test eax, eax",
                        "je @out",
                        "push " + codecaveGUID,
                        "mov ecx, eax",
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
            if (baseAddress > 0)
            {
                WoWObject to = new WoWObject(baseAddress);
                if (!to.IsValid)
                    return;
                if (to.Guid <= 0)
                    return;
                if (to.Type == WoWObjectType.Unit)
                    ClickToMove.CGPlayer_C__ClickToMove(ObjectManager.ObjectManager.Me.Position.X, ObjectManager.ObjectManager.Me.Position.Y,
                        ObjectManager.ObjectManager.Me.Position.Z, to.Guid, (Int32) ClickToMoveType.NpcInteract, 0.5f);
                else
                    InteractWith(baseAddress);
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