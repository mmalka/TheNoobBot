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
                var configThread = new Thread(ConfigWowThread) { Name = "config wow Thread" };
                configThread.Start();
            }
            catch (Exception exception)
            {
                Logging.WriteError("ConfigWow(): " + exception);
            }
        }
        static void ConfigWowThread()
        {
            try
            {
                while (ObjectManager.ObjectManager.Me.InCombat)
                {
                    Thread.Sleep(10);
                }
                Thread.Sleep(50);
                Memory.WowMemory.Memory.WriteInt(
                    Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule +
                                                     (uint) Addresses.AutoInteract.AutoInteract_Activate_Pointer) +
                    (uint) Addresses.AutoInteract.AutoInteract_Activate_Offset, 1);
                Memory.WowMemory.Memory.WriteInt(
                    Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule +
                                                     (uint) Addresses.AutoSelfCast.AutoSelfCast_Activate_Pointer) +
                    (uint) Addresses.AutoSelfCast.AutoSelfCast_Activate_Offset, 1);
                Memory.WowMemory.Memory.WriteInt(
                    Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule +
                                                     (uint) Addresses.AutoLoot.AutoLoot_Activate_Pointer) +
                    (uint) Addresses.AutoLoot.AutoLoot_Activate_Offset, 1);
                Lua.LuaDoString("SetCVar(\"ScriptErrors\", \"0\")");
                if (nManagerSetting.CurrentSetting.AllowTNBToSetYourMaxFps)
                {
                    Lua.LuaDoString("ConsoleExec(\"maxfpsbk 100\")");
                    Lua.LuaDoString("ConsoleExec(\"MaxFPS 100\")");
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("ConfigWowThread(): " + exception);
            }
        }
    }
}
