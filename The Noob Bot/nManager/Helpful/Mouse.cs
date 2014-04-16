using System;
using nManager.Helpful.Win32;

namespace nManager.Helpful
{
    public class Mouse
    {
        #region Fields

        private const int MOUSEEVENTF_ABSOLUTE = 0x8000; //  absolute move
        private const int MOUSEEVENTF_LEFTDOWN = 0x2; //  left button down
        private const int MOUSEEVENTF_LEFTUP = 0x4; //  left button up
        private const int MOUSEEVENTF_MIDDLEDOWN = 0x20; //  middle button down
        private const int MOUSEEVENTF_MIDDLEUP = 0x40; //  middle button up
        private const int MOUSEEVENTF_RIGHTDOWN = 0x8; //  right button down
        private const int MOUSEEVENTF_RIGHTUP = 0x10; //  right button up

        #endregion Fields

        #region Methods

        /// <summary>
        /// Click Left.
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public static void ClickLeft()
        {
            try
            {
                Native.mouse_event(MOUSEEVENTF_LEFTDOWN + MOUSEEVENTF_ABSOLUTE, 0, 0, 0, 0);
                Native.mouse_event(MOUSEEVENTF_LEFTUP + MOUSEEVENTF_ABSOLUTE, 0, 0, 0, 0);
            }
            catch (Exception exception)
            {
                Logging.WriteError("ClickLeft(): " + exception);
            }
        }

        /// <summary>
        /// Click Right.
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public static void ClickRight()
        {
            try
            {
                Native.mouse_event(MOUSEEVENTF_RIGHTDOWN + MOUSEEVENTF_ABSOLUTE, 0, 0, 0, 0);
                Native.mouse_event(MOUSEEVENTF_RIGHTUP + MOUSEEVENTF_ABSOLUTE, 0, 0, 0, 0);
            }
            catch (Exception exception)
            {
                Logging.WriteError("ClickRight(): " + exception);
            }
        }

        /// <summary>
        /// Click Roller.
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public static void ClickRoller()
        {
            try
            {
                Native.mouse_event(MOUSEEVENTF_MIDDLEDOWN + MOUSEEVENTF_ABSOLUTE, 0, 0, 0, 0);
                Native.mouse_event(MOUSEEVENTF_MIDDLEUP + MOUSEEVENTF_ABSOLUTE, 0, 0, 0, 0);
            }
            catch (Exception exception)
            {
                Logging.WriteError("ClickRoller(): " + exception);
            }
        }

        /// <summary>
        /// Position Curseur.
        /// </summary>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        /// <returns></returns>
        public static void CurseurPosition(int posX, int posY)
        {
            try
            {
                Native.SetCursorPos(posX, posY);
            }
            catch (Exception exception)
            {
                Logging.WriteError("CurseurPosition(int posX, int posY): " + exception);
            }
        }

        /// <summary>
        /// Position Curseur in window.
        /// </summary>
        /// <param name="mainWindowHandle"> </param>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        /// <returns></returns>
        public static void CurseurWindowPosition(IntPtr mainWindowHandle, int posX, int posY)
        {
            try
            {
                Native.SetCursorPos(Display.GetWindowPosX(mainWindowHandle) + posX,
                    Display.GetWindowPosY(mainWindowHandle) + posY);
            }
            catch (Exception exception)
            {
                Logging.WriteError("CurseurWindowPosition(IntPtr mainWindowHandle, int posX, int posY): " + exception);
            }
        }

        /// <summary>
        /// Position Curseur in window.
        /// </summary>
        /// <param name="mainWindowHandle"> </param>
        /// <param name="percentageX"></param>
        /// <param name="percentageY"></param>
        /// <returns></returns>
        public static void CurseurWindowPercentagePosition(IntPtr mainWindowHandle, int percentageX, int percentageY)
        {
            try
            {
                Native.SetCursorPos(
                    Display.GetWindowPosX(mainWindowHandle) +
                    ((percentageX*Display.GetWindowWidth(mainWindowHandle))/100),
                    Display.GetWindowPosY(mainWindowHandle) +
                    ((percentageY*Display.GetWindowHeight(mainWindowHandle))/100));
            }
            catch (Exception exception)
            {
                Logging.WriteError(
                    "CurseurWindowPercentagePosition(IntPtr mainWindowHandle, int percentageX, int percentageY): " +
                    exception);
            }
        }

        #endregion Methods
    }
}