using System;
using System.Threading;
using System.Windows.Forms;
using nManager.Helpful.Win32;
using nManager.Wow.Helpers;

namespace nManager.Helpful
{
    /// <summary>
    /// Wow Keyboard Manager
    /// </summary>
    public static class Keyboard
    {
        private static int VkKeyScan(string key)
        {
            try
            {
                if (key == "1")
                    return (int) VK.KEY_1;
                if (key == "2")
                    return (int) VK.KEY_2;
                if (key == "3")
                    return (int) VK.KEY_3;
                if (key == "4")
                    return (int) VK.KEY_4;
                if (key == "5")
                    return (int) VK.KEY_5;
                if (key == "6")
                    return (int) VK.KEY_6;
                if (key == "7")
                    return (int) VK.KEY_7;
                if (key == "8")
                    return (int) VK.KEY_8;
                if (key == "9")
                    return (int) VK.KEY_9;
                if (key == "0")
                    return (int) VK.KEY_0;
                if (key == ")")
                    return (int) Keys.OemOpenBrackets;
                if (key == "-")
                    return (int) Keys.OemMinus;
                if (key == "=")
                    return (int) Keys.Oemplus;

                key = key.ToLower();
                char[] test = key.ToCharArray();
                if (key.Length > 1)
                {
                    if (key == "{CTRL}".ToLower())
                        return (int) Keys.ControlKey;
                    if (key == "{ALT}".ToLower())
                        return (int) Keys.Alt;
                    if (key == "{SHIFT}".ToLower())
                        return (int) Keys.Shift;
                    if (key == "{SPACE}".ToLower())
                        return (int) Keys.Space;
                    if (key == "{UP}".ToLower())
                        return (int) Keys.Up;
                    if (key == "{DOWN}".ToLower())
                        return (int) Keys.Down;
                    if (key == "{LEFT}".ToLower())
                        return (int) Keys.Left;
                    if (key == "{RIGHT}".ToLower())
                        return (int) Keys.Right;

                    if (key == "f1".ToLower())
                        return (int) Keys.F1;
                    if (key == "shift".ToLower())
                        return (int) Keys.Shift;
                    if (key == "f2".ToLower())
                        return (int) Keys.F2;
                    if (key == "space".ToLower())
                        return (int) Keys.Space;
                    if (key == "ctrl".ToLower())
                        return (int) Keys.ControlKey;
                    if (key == "f3".ToLower())
                        return (int) Keys.F3;
                    if (key == "f4".ToLower())
                        return (int) Keys.F4;
                    if (key == "f5".ToLower())
                        return (int) Keys.F5;
                    if (key == "f8".ToLower())
                        return (int) Keys.F8;
                    if (key == "numpad1".ToLower())
                        return (int) Keys.NumPad1;
                    if (key == "f9".ToLower())
                        return (int) Keys.F9;
                    if (key == "numpad4".ToLower())
                        return (int) Keys.NumPad4;
                    if (key == "numpad7".ToLower())
                        return (int) Keys.NumPad7;
                    if (key == "f7".ToLower())
                        return (int) Keys.F7;
                    if (key == "alt".ToLower())
                        return (int) Keys.Alt;
                    if (key == "numpadplus".ToLower())
                        return (int) Keys.Add;
                    if (key == "pagedown".ToLower())
                        return (int) Keys.PageDown;
                    if (key == "numlock".ToLower())
                        return (int) Keys.NumLock;
                    if (key == "up".ToLower())
                        return (int) Keys.Up;
                    if (key == "numpad2".ToLower())
                        return (int) Keys.NumPad2;
                    if (key == "numpad5".ToLower())
                        return (int) Keys.NumPad5;
                    if (key == "numpad8".ToLower())
                        return (int) Keys.NumPad8;
                    if (key == "end".ToLower())
                        return (int) Keys.End;
                    if (key == "tab".ToLower())
                        return (int) Keys.Tab;
                    if (key == "down".ToLower())
                        return (int) Keys.Down;
                    if (key == "numpaddivide".ToLower())
                        return (int) Keys.Divide;
                    if (key == "backspace".ToLower())
                        return (int) Keys.Back;
                    if (key == "delete".ToLower())
                        return (int) Keys.Delete;
                    if (key == "numpad0".ToLower())
                        return (int) Keys.NumPad0;
                    if (key == "numpad3".ToLower())
                        return (int) Keys.NumPad3;
                    if (key == "enter".ToLower())
                        return (int) Keys.Enter;
                    if (key == "numpad6".ToLower())
                        return (int) Keys.NumPad6;
                    if (key == "numpad9".ToLower())
                        return (int) Keys.NumPad9;
                    if (key == "f6".ToLower())
                        return (int) Keys.F6;
                    if (key == "pageup".ToLower())
                        return (int) Keys.PageUp;
                    if (key == "home".ToLower())
                        return (int) Keys.Home;
                    if (key == "escape".ToLower())
                        return (int) Keys.Space;
                    if (key == "numpadminus".ToLower())
                        return (int) Keys.Subtract;
                    if (key == "left".ToLower())
                        return (int) Keys.Left;
                    if (key == "f10".ToLower())
                        return (int) Keys.F10;
                    if (key == "f11".ToLower())
                        return (int) Keys.F11;
                    if (key == "right".ToLower())
                        return (int) Keys.Right;
                    if (key == "f12".ToLower())
                        return (int) Keys.F12;
                    if (key == "printscreen".ToLower())
                        return (int) Keys.PrintScreen;
                    if (key == "f14".ToLower())
                        return (int) Keys.F14;
                    if (key == "f15".ToLower())
                        return (int) Keys.F15;
                    if (key == "f16".ToLower())
                        return (int) Keys.F16;
                    if (key == "f17".ToLower())
                        return (int) Keys.F17;
                    if (key == "f18".ToLower())
                        return (int) Keys.F18;
                    if (key == "f19".ToLower())
                        return (int) Keys.F19;
                    if (key == "numpadequals".ToLower())
                        return (int) Keys.None;
                    if (key == "-".ToLower())
                        return (int) Keys.Subtract;
                }

                return Native.VkKeyScan(test[0]);
            }
            catch (Exception exception)
            {
                Logging.WriteError("VkKeyScan(string key): " + exception);
            }
            return 0;
        }

        /// <summary>
        /// Down a Keyboard Key.
        /// </summary>
        /// <param name="mainWindowHandle"> </param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static void DownKey(IntPtr mainWindowHandle, Keys key)
        {
            try
            {
                Native.SendMessage(mainWindowHandle, Native.DownKeyVk, (int) key, 0);
            }
            catch (Exception exception)
            {
                Logging.WriteError("DownKey(IntPtr mainWindowHandle, Keys key): " + exception);
            }
        }

        /// <summary>
        /// Up a Keyboard Key.
        /// </summary>
        /// <param name="mainWindowHandle"> </param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static void UpKey(IntPtr mainWindowHandle, Keys key)
        {
            try
            {
                Native.SendMessage(mainWindowHandle, Native.UpKeyVk, (int) key, 0);
            }
            catch (Exception exception)
            {
                Logging.WriteError("UpKey(IntPtr mainWindowHandle, Keys key): " + exception);
            }
        }

        /// <summary>
        /// Press a Keyboard Key.
        /// </summary>
        /// <param name="mainWindowHandle"> </param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static void PressKey(IntPtr mainWindowHandle, Keys key)
        {
            try
            {
                Native.SendMessage(mainWindowHandle, Native.DownKeyVk, (int) key, 0);
                Thread.Sleep(100);
                Native.SendMessage(mainWindowHandle, Native.UpKeyVk, (int) key, 0);
            }
            catch (Exception exception)
            {
                Logging.WriteError("PressKey(IntPtr mainWindowHandle, Keys key): " + exception);
            }
        }

        /// <summary>
        /// Down a Keyboard Key.
        /// </summary>
        /// <param name="mainWindowHandle"> </param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static void DownKey(IntPtr mainWindowHandle, string key)
        {
            try
            {
                DownKey(mainWindowHandle, (Keys) VkKeyScan(key));
            }
            catch (Exception exception)
            {
                Logging.WriteError("DownKey(IntPtr mainWindowHandle, string key): " + exception);
            }
        }

        /// <summary>
        /// Up a Keyboard Key.
        /// </summary>
        /// <param name="mainWindowHandle"> </param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static void UpKey(IntPtr mainWindowHandle, string key)
        {
            try
            {
                UpKey(mainWindowHandle, (Keys) VkKeyScan(key));
            }
            catch (Exception exception)
            {
                Logging.WriteError("UpKey(IntPtr mainWindowHandle, string key): " + exception);
            }
        }

        /// <summary>
        /// Press a Keyboard Key.
        /// </summary>
        /// <param name="mainWindowHandle"> </param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static void PressKey(IntPtr mainWindowHandle, string key)
        {
            try
            {
                if (key == "/")
                {
                    PressKey(mainWindowHandle, (Keys) VK.DIVIDE);
                }
                else if (key.Length > 1 && key.Contains(";"))
                {
                    Keybindings.PressBarAndSlotKey(key);
                }
                else
                {
                    PressKey(mainWindowHandle, (Keys) VkKeyScan(key));
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("PressKey(IntPtr mainWindowHandle, string key): " + exception);
            }
        }

        /// <summary>
        /// Sends the text in wow windows.
        /// </summary>
        /// <param name="mainWindowHandle"> </param>
        /// <param name="text">The text.</param>
        public static void SendText(IntPtr mainWindowHandle, string text)
        {
            try
            {
                try
                {
                    Clipboard.SetText(text);
                }
                catch
                {
                    return;
                }

                Thread.Sleep(10);

                Native.SendMessage(mainWindowHandle, 0x100, 0xa2, 0);
                Native.SendMessage(mainWindowHandle, 0x100, (int) Keys.V, 0);
                Thread.Sleep(10);
                Native.SendMessage(mainWindowHandle, 0x101, 0xa2, 0);
                Native.SendMessage(mainWindowHandle, 0x101, (int) Keys.V, 0);

                Thread.Sleep(10);
            }
            catch (Exception exception)
            {
                Logging.WriteError("SendText(IntPtr mainWindowHandle, string text): " + exception);
            }
        }
    }
}