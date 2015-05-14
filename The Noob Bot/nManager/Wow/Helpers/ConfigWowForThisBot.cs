using System;
using System.Threading;
using nManager.Helpful;
using nManager.Wow.Patchables;

namespace nManager.Wow.Helpers
{
    public class ConfigWowForThisBot
    {
        /// <summary>
        /// Config Key Bindings Wow in game.
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public static void ConfigWow()
        {
            try
            {
                Thread configThread = new Thread(ConfigWowThread) {Name = "config wow Thread"};
                configThread.Start();
            }
            catch (Exception exception)
            {
                Logging.WriteError("ConfigWow(): " + exception);
            }
        }

        public static void StartStopClickToMove(bool startStop = true)
        {
            uint autoInteract =
                Memory.WowMemory.Memory.ReadUInt(Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + (uint) Addresses.ActivateSettings.AutoInteract_Activate_Pointer) +
                                                 (uint) Addresses.ActivateSettings.Activate_Offset);
            if (startStop && autoInteract != 1)
            {
                Logging.WriteDebug("AutoInteract_Activate_Pointer was OFF, now activated.");
                Memory.WowMemory.Memory.WriteUInt(
                    Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + (uint) Addresses.ActivateSettings.AutoInteract_Activate_Pointer) + (uint) Addresses.ActivateSettings.Activate_Offset, 1);
            }
            else if (!startStop && autoInteract == 1)
            {
                Logging.WriteDebug("AutoInteract_Activate_Pointer was ON, now de-activated.");
                Memory.WowMemory.Memory.WriteUInt(
                    Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + (uint) Addresses.ActivateSettings.AutoInteract_Activate_Pointer) + (uint) Addresses.ActivateSettings.Activate_Offset, 0);
            }
        }

        private static void ConfigWowThread()
        {
            try
            {
                while (ObjectManager.ObjectManager.Me.InCombat)
                {
                    Thread.Sleep(10);
                }
                Thread.Sleep(50);

                StartStopClickToMove();
                uint autoDismount =
                    Memory.WowMemory.Memory.ReadUInt(Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + (uint) Addresses.ActivateSettings.AutoDismount_Activate_Pointer) +
                                                     (uint) Addresses.ActivateSettings.Activate_Offset);
                if (autoDismount != 1)
                {
                    Logging.WriteDebug("AutoDismount_Activate_Pointer was OFF, now activated.");
                    Memory.WowMemory.Memory.WriteUInt(
                        Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + (uint) Addresses.ActivateSettings.AutoDismount_Activate_Pointer) + (uint) Addresses.ActivateSettings.Activate_Offset, 1);
                }
                uint autoLoot =
                    Memory.WowMemory.Memory.ReadUInt(Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + (uint) Addresses.ActivateSettings.AutoLoot_Activate_Pointer) +
                                                     (uint) Addresses.ActivateSettings.Activate_Offset);
                if (autoLoot != 1)
                {
                    Logging.WriteDebug("AutoLoot_Activate_Pointer was OFF, now activated.");
                    Memory.WowMemory.Memory.WriteUInt(
                        Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + (uint) Addresses.ActivateSettings.AutoLoot_Activate_Pointer) + (uint) Addresses.ActivateSettings.Activate_Offset, 1);
                }
                uint autoSelfCast =
                    Memory.WowMemory.Memory.ReadUInt(Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + (uint) Addresses.ActivateSettings.AutoSelfCast_Activate_Pointer) +
                                                     (uint) Addresses.ActivateSettings.Activate_Offset);
                if (autoSelfCast != 1)
                {
                    Logging.WriteDebug("AutoSelfCast_Activate_Pointer was OFF, now activated.");
                    Memory.WowMemory.Memory.WriteUInt(
                        Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + (uint) Addresses.ActivateSettings.AutoSelfCast_Activate_Pointer) + (uint) Addresses.ActivateSettings.Activate_Offset, 1);
                }
                Lua.LuaDoString("SetCVar(\"ScriptErrors\", \"0\")");
                if (nManagerSetting.CurrentSetting.AllowTNBToSetYourMaxFps)
                {
                    Lua.LuaDoString("ConsoleExec(\"maxfpsbk 60\")");
                    Lua.LuaDoString("ConsoleExec(\"MaxFPS 60\")");
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("ConfigWowThread(): " + exception);
            }
        }
    }
}