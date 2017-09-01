using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using nManager.Helpful;
using nManager.Wow.MemoryClass;
using nManager.Wow.Patchables;

namespace nManager.Wow.Helpers
{
    public static class Lua
    {
        public static void LuaDoString(List<string> command, bool notInGameMode = false)
        {
            try
            {
                string val = command.Aggregate("", (current, line) => current + (" " + line));
                LuaDoString(val, notInGameMode);
            }
            catch (Exception exception)
            {
                Logging.WriteError("LuaDoString(List<string> command, bool notInGameMode = false): " + exception);
            }
        }

        public static string LuaDoString(string command, string returnArgument, bool notInGameMode = false)
        {
            try
            {
                lock ("LuaWithReturnValue")
                {
                    LuaDoString(command, notInGameMode);
                    return GetLocalizedText(returnArgument);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("LuaDoString(string command, string returnArgument, bool notInGameMode = false): " +
                                   exception);
                return "";
            }
        }

        public static void LuaDoString(string command, bool notInGameMode = false, bool doAntiAfk = true)
        {
            try
            {
                if (!notInGameMode && !Usefuls.InGame && !Usefuls.IsLoading)
                {
                    Memory.WowMemory.GameFrameUnLock();
                    return;
                }
                while (!notInGameMode && !Usefuls.InGame || Usefuls.IsLoading)
                {
                    Memory.WowMemory.GameFrameUnLock();
                    Thread.Sleep(200);
                }
                if (!notInGameMode && doAntiAfk) // Avoid loop while retrieving the AFK key to press.
                    Usefuls.UpdateLastHardwareAction();
                if (command.Replace(" ", "").Length <= 0)
                    return;

                // Allocate memory
                uint doStringArgCodecave = Memory.WowMemory.Memory.AllocateMemory(Encoding.UTF8.GetBytes(command).Length + 1 + Others.Random(1, 25));
                if (doStringArgCodecave <= 0)
                    return;
                // Write value:
                Memory.WowMemory.Memory.WriteBytes(doStringArgCodecave, Encoding.UTF8.GetBytes(command));

                //Console.WriteLine("LuaDoString(" + command + ", " + notInGameMode + ")");

                // Write the asm stuff for Lua_DoString
                string[] asm = new[]
                {
                    "mov eax, " + doStringArgCodecave,
                    "push 0",
                    "push eax",
                    "push eax",
                    "mov eax, " +
                    (Memory.WowProcess.WowModule + (uint) Addresses.FunctionWow.FrameScript_ExecuteBuffer)
                    , // Lua_DoString
                    "call eax",
                    "add esp, 0xC",
                    "@out:",
                    "retn"
                };

                if (!notInGameMode)
                {
                    List<string> tempsAsm = new List<string>
                    {
                        /*"call " +
                        (Memory.WowProcess.WowModule +
                         (uint) Addresses.FunctionWow.ClntObjMgrGetActivePlayer),
                        "test eax, eax",
                        "je @out",*/
                        /*"call " +
                        (Memory.WowProcess.WowModule +
                         (uint) Addresses.FunctionWow.ClntObjMgrGetActivePlayerObj),
                        "test eax, eax",
                        "je @out"*/
                    };

                    tempsAsm.AddRange(asm);

                    asm = tempsAsm.ToArray();
                }
                else
                {
                    List<string> tempsAsm = new List<string>
// ReSharper disable RedundantEmptyObjectOrCollectionInitializer
                    {
                        //"call " + ( Memory.WowProcess.WowModule +  (uint) Addresses.FunctionWow.ClntObjMgrGetActivePlayer),
                        //"test eax, eax",
                        //"jne @out",
                        //"call " + (Memory.WowProcess.WowModule + (uint)Addresses.FunctionWow.ClntObjMgrGetActivePlayerObj),
                        //"test eax, eax",
                        //"jne @out",
                        //"mov eax, [" + ( Memory.WowProcess.WowModule + (uint) Addresses.GameInfo.isLoadingOrConnecting + "]"),
                        //"test eax, eax",
                        //"jne @out"
                    };
// ReSharper restore RedundantEmptyObjectOrCollectionInitializer

                    tempsAsm.AddRange(asm);
                    asm = tempsAsm.ToArray();
                }
                // Inject
                Memory.WowMemory.InjectAndExecute(asm);
                // Free memory allocated 
                Memory.WowMemory.Memory.FreeMemory(doStringArgCodecave);
            }
            catch (Exception exception)
            {
                Logging.WriteError("LuaDoString(string command, bool notInGameMode = false): " + exception);
            }
        }

        public static string GetLocalizedText(string commandline)
        {
            try
            {
                while (!Usefuls.InGame && Usefuls.IsLoading)
                {
                    // if we are not loading, avoid freezing wow => relog for example.
                    Memory.WowMemory.GameFrameUnLock();
                    Thread.Sleep(200);
                }
                if (!Usefuls.InGame && !Usefuls.IsLoading)
                    return "NOT_CONNECTED";
                // Command to send using LUA
                string command = commandline;
                if (command.Replace(" ", "").Length <= 0)
                    return "";
                // Allocate memory for command
                uint luaGetLocalizedTextSpace =
                    Memory.WowMemory.Memory.AllocateMemory(Encoding.UTF8.GetBytes(command).Length + 1 +
                                                           Others.Random(1, 25));
                if (luaGetLocalizedTextSpace <= 0)
                    return "";
                // Write command in the allocated memory
                Memory.WowMemory.Memory.WriteBytes(luaGetLocalizedTextSpace, Encoding.UTF8.GetBytes(command));

                // Console.WriteLine("GetLocalizedText(" + Commandline + ")");

                uint pString = Memory.WowMemory.Memory.AllocateMemory(4);

                string[] asm = new[]
                {
                    /*"call " +
                    (Memory.WowProcess.WowModule + (uint) Addresses.FunctionWow.ClntObjMgrGetActivePlayer)
                    ,
                    "test eax, eax",
                    "je @out",*/
                    /*"call " +
                    (
                     * ),
                    "test eax, eax",
                    "je @out",*/
                    "mov ecx, " + ObjectManager.ObjectManager.Me.GetBaseAddress,
                    "push -1",
                    "mov edx, " + luaGetLocalizedTextSpace + "",
                    "push edx",
                    "call " +
                    (Memory.WowProcess.WowModule + (uint) Addresses.FunctionWow.FrameScript__GetLocalizedText),
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

                // Inject the shit
                uint stringLength = Memory.WowMemory.InjectAndExecute(asm);
                uint retPtr = Memory.WowMemory.Memory.ReadUInt(pString);
                string sResult = "";
                if (retPtr > 0)
                    sResult = Memory.WowMemory.Memory.ReadUTF8String(retPtr, (int) stringLength);
                // Free memory allocated for command
                Memory.WowMemory.Memory.FreeMemory(luaGetLocalizedTextSpace);
                Memory.WowMemory.Memory.FreeMemory(pString);

                // Remove the LUA variable
                //LuaDoString(commandline + " = nil");

                // Uninstall the hook
                return sResult;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetLocalizedText(string commandline): " + exception);
                return "";
            }
        }

        public static void RunMacroText(string macro)
        {
            try
            {
                LuaDoString("RunMacroText(\"" + macro + "\")");
            }
            catch (Exception exception)
            {
                Logging.WriteError("RunMacroText(string macro): " + exception);
            }
        }
    }
}