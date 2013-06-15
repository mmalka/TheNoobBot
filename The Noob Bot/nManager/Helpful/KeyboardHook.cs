using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace nManager.Helpful
{
    ///<summary>
    ///  Registers a hot key with Windows.
    ///</summary>
    public sealed class KeyboardHook : IDisposable
    {
        // Registers a hot key with Windows.

        private readonly Window _window = new Window();
        private int _currentId;

        public KeyboardHook()
        {
            try
            {
                // register the event of the inner native window.
                _window.KeyPressed += delegate(object sender, KeyPressedEventArgs args)
                    {
                        if (KeyPressed != null)
                            KeyPressed(this, args);
                    };
            }
            catch (Exception exception)
            {
                Logging.WriteError("KeyboardHook(): " + exception);
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            try
            {
                // unregister all the registered hot keys.
                for (int i = _currentId; i > 0; i--)
                {
                    UnregisterHotKey(_window.Handle, i);
                }

                // dispose the inner native window.
                _window.Dispose();
            }
            catch (Exception exception)
            {
                Logging.WriteError("KeyboardHook > Dispose(): " + exception);
            }
        }

        #endregion

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        // Unregisters the hot key with Windows.
        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        /// <summary>
        /// Registers a hot key in the system.
        /// </summary>
        /// <param name="modifier">The modifiers that are associated with the hot key.</param>
        /// <param name="key">The key itself that is associated with the hot key.</param>
        public void RegisterHotKey(ModifierKeys modifier, Keys key)
        {
            try
            {
                // increment the counter.
                _currentId = _currentId + 1;

                // register the hot key.

                RegisterHotKey(_window.Handle, _currentId, (uint) modifier, (uint) key);
            }
            catch (Exception exception)
            {
                Logging.WriteError("RegisterHotKey(ModifierKeys modifier, Keys key): " + exception);
            }
        }

        /// <summary>
        /// A hot key has been pressed.
        /// </summary>
        public event EventHandler<KeyPressedEventArgs> KeyPressed;

        #region Nested type: Window

        /// <summary>
        /// Represents the window that is used internally to get the messages.
        /// </summary>
        private sealed class Window : NativeWindow, IDisposable
        {
            private const int WmHotkey = 0x0312;

            public Window()
            {
                try
                {
                    // create the handle for the window.
                    CreateHandle(new CreateParams());
                }
                catch (Exception exception)
                {
                    Logging.WriteError("KeyboardHook > Window > Window(): " + exception);
                }
            }

            #region IDisposable Members

            public void Dispose()
            {
                try
                {
                    DestroyHandle();
                }
                catch (Exception exception)
                {
                    Logging.WriteError("KeyboardHook > Dispose(): " + exception);
                }
            }

            #endregion

            /// <summary>
            /// Overridden to get the notifications.
            /// </summary>
            /// <param name="m"></param>
            protected override void WndProc(ref Message m)
            {
                try
                {
                    base.WndProc(ref m);

                    // check if we got a hot key pressed.
                    if (m.Msg == WmHotkey)
                    {
                        // get the keys.
                        Keys key = (Keys) (((int) m.LParam >> 16) & 0xFFFF);
                        ModifierKeys modifier = (ModifierKeys) ((int) m.LParam & 0xFFFF);

                        // invoke the event to notify the parent.
                        if (KeyPressed != null)
                            KeyPressed(this, new KeyPressedEventArgs(modifier, key));
                    }
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WndProc(ref Message m): " + exception);
                }
            }

            public event EventHandler<KeyPressedEventArgs> KeyPressed;
        }

        #endregion

        /// <summary>
        /// Event Args for the event that is fired after the hot key has been pressed.
        /// </summary>
        public class KeyPressedEventArgs : EventArgs
        {
            private readonly Keys _key;
            private readonly ModifierKeys _modifier;

            internal KeyPressedEventArgs(ModifierKeys modifier, Keys key)
            {
                try
                {
                    _modifier = modifier;
                    _key = key;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("KeyPressedEventArgs(ModifierKeys modifier, Keys key): " + exception);
                }
            }

            public ModifierKeys Modifier
            {
                get
                {
                    try
                    {
                        return _modifier;
                    }
                    catch (Exception exception)
                    {
                        Logging.WriteError("ModifierKeys Modifier: " + exception);
                    }
                    return ModifierKeys.Alt;
                }
            }

            public Keys Key
            {
                get
                {
                    try
                    {
                        return _key;
                    }
                    catch (Exception exception)
                    {
                        Logging.WriteError("Keys Key: " + exception);
                    }
                    return Keys.None;
                }
            }
        }

        /// <summary>
        /// The enumeration of possible modifiers.
        /// </summary>
        [Flags]
        public enum ModifierKeys : uint
        {
            Alt = 1,
            Control = 2,
            Shift = 4,
            Win = 8
        }
    }
}