using System;
using System.Collections.Generic;
using System.Threading;
using nManager.Helpful;
using nManager.Wow.Patchables;

namespace nManager.Wow.Helpers
{
    public static class Keybindings
    {
        private static List<KeybindingsStruct> _keybindingsList = new List<KeybindingsStruct>();

        public static string GetKeyByAction(Enums.Keybindings action, bool autoAssignKeyIfNull = true)
        {
            try
            {
                // Search if exist in list
                foreach (var k in _keybindingsList)
                {
                    if (k.Key != "" && k.Action == action)
                        return k.Key;
                }

                // Search if exist ingame:
                Lua.LuaDoString("key1, key2 = GetBindingKey(\"" + action + "\");");
                string k1 = Lua.GetLocalizedText("key1");
                string k2 = Lua.GetLocalizedText("key2");
                if (k1 != "")
                {
                    _keybindingsList.Add(new KeybindingsStruct {Action = action, Key = k1});
                    return k1;
                }
                if (k2 != "")
                {
                    _keybindingsList.Add(new KeybindingsStruct {Action = action, Key = k2});
                    return k2;
                }

                // Create if don't exist:
                if (autoAssignKeyIfNull)
                {
                    string key = GetAFreeKey();
                    if (!string.IsNullOrEmpty(key))
                    {
                        Logging.WriteDebug(action + " were not bind, currently trying to bind it with key: " + key + ".");
                        SetKeyByAction(action, key);
                        return key;
                    }
                    else
                    {
                        Logging.WriteDebug(
                            "No free keys found on 236 possible bindings, if you got that line, you mainly have a problem with your WoW keybindings.");
                        return "";
                    }
                }
                else
                    return "";
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetKeyByAction(Enums.Keybindings action, bool autoAssignKeyIfNull = true): " +
                                   exception);
                return "";
            }
        }

        public static string GetAFreeKey(bool easyonly = false)
        {
            try
            {
                foreach (Helpful.Win32.UnreservedVK key in Enum.GetValues(typeof (Helpful.Win32.UnreservedVK)))
                {
                    string randomStringResult = Others.GetRandomString(Others.Random(4, 10));
                    Lua.LuaDoString(randomStringResult + " = GetBindingAction(\"" + key + "\", true)", false, false);
                    string result = Lua.GetLocalizedText(randomStringResult);
                    if (string.IsNullOrEmpty(result))
                    {
                        return key.ToString();
                    }
                }
                if (easyonly)
                {
                    return ""; /* We did not found any key for our Anti-AFK, if we use a combination,
                                    * it will cast the Simple key as there is no binded action for the extended keybind.
                                    */
                }
                foreach (Helpful.Win32.UnreservedVK key in Enum.GetValues(typeof (Helpful.Win32.UnreservedVK)))
                {
                    string randomStringResult = Others.GetRandomString(Others.Random(4, 10));
                    Lua.LuaDoString(randomStringResult + " = GetBindingAction(\"CTRL-" + key + "\", true)");
                    if (string.IsNullOrEmpty(Lua.GetLocalizedText(randomStringResult)))
                    {
                        return "CTRL-" + key;
                    }
                }
                foreach (Helpful.Win32.UnreservedVK key in Enum.GetValues(typeof (Helpful.Win32.UnreservedVK)))
                {
                    string randomStringResult = Others.GetRandomString(Others.Random(4, 10));
                    Lua.LuaDoString(randomStringResult + " = GetBindingAction(\"SHIFT-" + key + "\", true)");
                    if (string.IsNullOrEmpty(Lua.GetLocalizedText(randomStringResult)))
                    {
                        return "SHIFT-" + key;
                    }
                }
                foreach (Helpful.Win32.UnreservedVK key in Enum.GetValues(typeof (Helpful.Win32.UnreservedVK)))
                {
                    string randomStringResult = Others.GetRandomString(Others.Random(4, 10));
                    Lua.LuaDoString(randomStringResult + " = GetBindingAction(\"CTRL-SHIFT-" + key + "\", true)");
                    if (string.IsNullOrEmpty(Lua.GetLocalizedText(randomStringResult)))
                    {
                        return "CTRL-SHIFT-" + key;
                    }
                }
                return ""; // No key found. Quite impossible since we try 236 bindings.
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetAFreeKey(): " + exception);
                return "";
            }
        }

        public static void SetKeyByAction(Enums.Keybindings action, string key)
        {
            try
            {
                Lua.LuaDoString("SetBinding(GetBindingKey(\"" + action + "\"));");
                Lua.LuaDoString("SetBinding(\"" + key.ToUpper() + "\", \"" + action + "\");");
                Lua.LuaDoString("SaveBindings(2)");

                ResetList();
            }
            catch (Exception exception)
            {
                Logging.WriteError("SetKeyByAction(Enums.Keybindings action, string key): " + exception);
            }
        }

        public static void UpKeybindings(Enums.Keybindings action)
        {
            try
            {
                string k = GetKeyByAction(action);
                if (k != "")
                    Keyboard.UpKey(Memory.WowProcess.MainWindowHandle, k);
            }
            catch (Exception exception)
            {
                Logging.WriteError("UpKeybindings(Enums.Keybindings action): " + exception);
            }
        }

        public static void DownKeybindings(Enums.Keybindings action)
        {
            try
            {
                string k = GetKeyByAction(action);
                if (k != "")
                    Keyboard.DownKey(Memory.WowProcess.MainWindowHandle, k);
            }
            catch (Exception exception)
            {
                Logging.WriteError("DownKeybindings(Enums.Keybindings action): " + exception);
            }
        }

        public static void PressKeybindings(Enums.Keybindings action)
        {
            try
            {
                string k = GetKeyByAction(action);
                if (k != "")
                {
                    Keyboard.DownKey(Memory.WowProcess.MainWindowHandle, k);
                    Keyboard.UpKey(Memory.WowProcess.MainWindowHandle, k);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("PressKeybindings(Enums.Keybindings action): " + exception);
            }
        }

        public static void ResetList()
        {
            try
            {
                lock (typeof (Keybindings))
                {
                    _keybindingsList = new List<KeybindingsStruct>();
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("ResetList(): " + exception);
            }
        }

#pragma warning disable 649
        private static object _pressBarAndSlotKeyLocker;
#pragma warning restore 649
        /// <summary>
        /// Pres Bar and Slot.
        /// </summary>
        /// <param name="barAndSlot">The bar and slot (barAndSlot = 1;2 for press bar 1 slot 2).</param>
        public static void PressBarAndSlotKey(string barAndSlot)
        {
            try
            {
                barAndSlot = barAndSlot.Replace("{", "");
                barAndSlot = barAndSlot.Replace("}", "");
                barAndSlot = barAndSlot.Replace(" ", "");
                string[] keySlot = barAndSlot.Split(';');


                try
                {
                    lock (_pressBarAndSlotKeyLocker)
                    {
                        int lastBar =
                            Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule +
                                                            (uint) Addresses.BarManager.nbBar);
                        if (lastBar != Others.ToInt32(keySlot[0]) - 1)
                        {
                            Memory.WowMemory.Memory.WriteInt(
                                Memory.WowProcess.WowModule + (uint) Addresses.BarManager.nbBar,
                                Others.ToInt32(keySlot[0]) - 1);
                        }
                        Thread.Sleep(300);
                        PressSlotKey(Others.ToInt32(keySlot[1]));
                        Thread.Sleep(10);

                        if (lastBar != Others.ToInt32(keySlot[0]) - 1)
                        {
                            Memory.WowMemory.Memory.WriteInt(
                                Memory.WowProcess.WowModule + (uint) Addresses.BarManager.nbBar, lastBar);
                        }
                    }
                }
                catch (Exception exception)
                {
                    Logging.WriteError("PressBarAndSlotKey(string barAndSlot) #1: " + exception);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("PressBarAndSlotKey(string barAndSlot) #2: " + exception);
            }
        }

        /// <summary>
        /// Press Slot Key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static void PressSlotKey(int key)
        {
            try
            {
                switch (key)
                {
                    case 1:
                        PressKeybindings(Enums.Keybindings.ACTIONBUTTON1);
                        break;
                    case 2:
                        PressKeybindings(Enums.Keybindings.ACTIONBUTTON2);
                        break;
                    case 3:
                        PressKeybindings(Enums.Keybindings.ACTIONBUTTON3);
                        break;
                    case 4:
                        PressKeybindings(Enums.Keybindings.ACTIONBUTTON4);
                        break;
                    case 5:
                        PressKeybindings(Enums.Keybindings.ACTIONBUTTON5);
                        break;
                    case 6:
                        PressKeybindings(Enums.Keybindings.ACTIONBUTTON6);
                        break;
                    case 7:
                        PressKeybindings(Enums.Keybindings.ACTIONBUTTON7);
                        break;
                    case 8:
                        PressKeybindings(Enums.Keybindings.ACTIONBUTTON8);
                        break;
                    case 9:
                        PressKeybindings(Enums.Keybindings.ACTIONBUTTON9);
                        break;
                    case 10:
                        PressKeybindings(Enums.Keybindings.ACTIONBUTTON10);
                        break;
                    case 11:
                        PressKeybindings(Enums.Keybindings.ACTIONBUTTON11);
                        break;
                    case 12:
                        PressKeybindings(Enums.Keybindings.ACTIONBUTTON12);
                        break;
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("PressSlotKey(int key): " + exception);
            }
        }

        private struct KeybindingsStruct
        {
            public string Key;
            public Enums.Keybindings Action;
        }
    }
}