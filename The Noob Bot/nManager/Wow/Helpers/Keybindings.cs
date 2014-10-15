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
                foreach (KeybindingsStruct k in _keybindingsList)
                {
                    if (k.Key != "" && k.Action == action)
                        return k.Key;
                }

                // Search if exist ingame:
                Lua.LuaDoString("key1, key2 = GetBindingKey(\"" + action + "\");");
                string k1 = Lua.GetLocalizedText("key1");
                string k2 = Lua.GetLocalizedText("key2");
                if (k1 != "" && k1 != "BUTTON3")
                {
                    _keybindingsList.Add(new KeybindingsStruct {Action = action, Key = k1});
                    return k1;
                }
                if (k2 != "" && k2 != "BUTTON3")
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
                        if (k1 == "BUTTON3" || k2 == "BUTTON3")
                            Logging.WriteDebug(action + " were bind to a mouse button, as TheNoobBot does not support mouse button, currently trying to bind it with key: " + key + ".");
                        else
                            Logging.WriteDebug(action + " were not bind, currently trying to bind it with key: " + key + ".");
                        SetKeyByAction(action, key);
                        _keybindingsList.Add(new KeybindingsStruct {Action = action, Key = key});
                        return key;
                    }
                    Logging.WriteDebug(
                        "No free keys found on 236 possible bindings, if you got that line, you mainly have a problem with your WoW keybindings.");
                    return "";
                }
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
                    if (key.ToString() == Usefuls.AfkKeyPress)
                        continue;
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

        private struct KeybindingsStruct
        {
            public string Key;
            public Enums.Keybindings Action;
        }
    }
}