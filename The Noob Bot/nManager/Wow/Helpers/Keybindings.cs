using System;
using System.Collections.Generic;
using System.Threading;
using nManager.Helpful;
using nManager.Wow.Patchables;

namespace nManager.Wow.Helpers
{
    public static class Keybindings
    {
        static List<KeybindingsStruct> _keybindingsList = new List<KeybindingsStruct>();

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
                string kt = Lua.GetLocalizedText("key1");
                if (kt != "")
                {
                    _keybindingsList.Add(new KeybindingsStruct { Action = action, Key = kt });
                    return kt;
                }

                // Create if don't exist:
                SetKeyByAction(action, "VK_F13");
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetKeyByAction(Enums.Keybindings action, bool autoAssignKeyIfNull = true): " + exception);
            }
            return "k";
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
                lock (typeof(Keybindings))
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
                string[] keySlot = barAndSlot.Split(Convert.ToChar(";"));


                try
                {
                    lock (_pressBarAndSlotKeyLocker)
                    {
                        int lastBar =
                            Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule +
                                                            (uint)Addresses.BarManager.nbBar);
                        if (lastBar != Convert.ToInt32(keySlot[0]) - 1)
                        {
                            Memory.WowMemory.Memory.WriteInt(
                                Memory.WowProcess.WowModule + (uint)Addresses.BarManager.nbBar,
                                Convert.ToInt32(keySlot[0]) - 1);
                        }
                        Thread.Sleep(300);
                        PressSlotKey(Convert.ToInt32(keySlot[1]));
                        Thread.Sleep(10);

                        if (lastBar != Convert.ToInt32(keySlot[0]) - 1)
                        {
                            Memory.WowMemory.Memory.WriteInt(
                                Memory.WowProcess.WowModule + (uint)Addresses.BarManager.nbBar, lastBar);
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

        struct KeybindingsStruct
        {
            public string Key;
            public Enums.Keybindings Action;
        }
    }
}
