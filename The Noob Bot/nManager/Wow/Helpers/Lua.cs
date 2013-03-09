using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nManager.Helpful;
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

        public static void LuaDoString(string command, bool notInGameMode = false)
        {
            try
            {
                Usefuls.UpdateLastHardwareAction();
                if (command.Replace(" ", "").Length <= 0)
                    return;

                // Allocate memory
                var doStringArgCodecave =
                    Memory.WowMemory.Memory.AllocateMemory(Encoding.UTF8.GetBytes(command).Length + 1 +
                                                           Others.Random(1, 25));
                if (doStringArgCodecave <= 0)
                    return;
                // Write value:
                Memory.WowMemory.Memory.WriteBytes(doStringArgCodecave, Encoding.UTF8.GetBytes(command));

                //Console.WriteLine("LuaDoString(" + command + ", " + notInGameMode + ")");

                // Write the asm stuff for Lua_DoString
                var asm = new[]
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
                    var tempsAsm = new List<string>
                        {
                            "call " +
                            (Memory.WowProcess.WowModule +
                             (uint) Addresses.FunctionWow.ClntObjMgrGetActivePlayer),
                            "test eax, eax",
                            "je @out",
                            "call " +
                            (Memory.WowProcess.WowModule +
                             (uint) Addresses.FunctionWow.ClntObjMgrGetActivePlayerObj),
                            "test eax, eax",
                            "je @out"
                        };

                    tempsAsm.AddRange(asm);

                    asm = tempsAsm.ToArray();
                }
                else
                {
                    var tempsAsm = new List<string>
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
                // Command to send using LUA
                var command = commandline;
                if (command.Replace(" ", "").Length <= 0)
                    return "";
                // Allocate memory for command
                var luaGetLocalizedTextSpace =
                    Memory.WowMemory.Memory.AllocateMemory(Encoding.UTF8.GetBytes(command).Length + 1 +
                                                           Others.Random(1, 25));
                if (luaGetLocalizedTextSpace <= 0)
                    return "";
                // Write command in the allocated memory
                Memory.WowMemory.Memory.WriteBytes(luaGetLocalizedTextSpace, Encoding.UTF8.GetBytes(command));

                // Console.WriteLine("GetLocalizedText(" + Commandline + ")");

                var asm = new[]
                    {
                        "call " +
                        (Memory.WowProcess.WowModule + (uint) Addresses.FunctionWow.ClntObjMgrGetActivePlayer)
                        ,
                        "test eax, eax",
                        "je @out",
                        "call " +
                        (Memory.WowProcess.WowModule +
                         (uint) Addresses.FunctionWow.ClntObjMgrGetActivePlayerObj),
                        "test eax, eax",
                        "je @out",
                        "call " +
                        (Memory.WowProcess.WowModule +
                         (uint) Addresses.FunctionWow.ClntObjMgrGetActivePlayerObj),
                        "mov ecx, eax",
                        "push -1",
                        "mov edx, " + luaGetLocalizedTextSpace + "",
                        "push edx",
                        "call " +
                        (Memory.WowProcess.WowModule +
                         (uint) Addresses.FunctionWow.FrameScript__GetLocalizedText),
                        "@out:",
                        "retn"
                    };

                // Inject the shit
                var sResult = Encoding.UTF8.GetString(Memory.WowMemory.InjectAndExecute(asm, true));

                // Free memory allocated for command
                Memory.WowMemory.Memory.FreeMemory(luaGetLocalizedTextSpace);

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