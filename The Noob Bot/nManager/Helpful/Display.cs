using System;
using System.Drawing;
using System.Drawing.Imaging;
using nManager.Helpful.Win32;

namespace nManager.Helpful
{
    /// <summary>
    /// Window Display Manager
    /// </summary>
    public static class Display
    {
        /// <summary>
        /// Gets the width of the  window.
        /// </summary>
        /// <value>
        /// The width of the  window.
        /// </value>
        /// <param name="mainWindowHandle"> </param>
        public static int GetWindowWidth(IntPtr mainWindowHandle)
        {
            try
            {
                Native.RECT windowRect = new Native.RECT();
                Native.GetWindowRect(mainWindowHandle, ref windowRect);
                return windowRect.right - windowRect.left;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWindowWidth(IntPtr mainWindowHandle): " + exception);
                return 0;
            }
        }

        /// <summary>
        /// Gets the height window.
        /// </summary>
        /// <value>
        /// The height of the window.
        /// </value>
        /// <param name="mainWindowHandle"> </param>
        public static int GetWindowHeight(IntPtr mainWindowHandle)
        {
            try
            {
                Native.RECT windowRect = new Native.RECT();
                Native.GetWindowRect(mainWindowHandle, ref windowRect);
                return windowRect.bottom - windowRect.top;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWindowHeight(IntPtr mainWindowHandle): " + exception);
                return 0;
            }
        }

        /// <summary>
        /// Gets window pos X.
        /// </summary>
        /// <param name="mainWindowHandle"> </param>
        public static int GetWindowPosX(IntPtr mainWindowHandle)
        {
            try
            {
                Native.RECT windowRect = new Native.RECT();
                Native.GetWindowRect(mainWindowHandle, ref windowRect);
                return windowRect.left;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWindowPosX(IntPtr mainWindowHandle): " + exception);
                return 0;
            }
        }

        /// <summary>
        /// Gets window pos Y.
        /// </summary>
        /// <param name="mainWindowHandle"> </param>
        public static int GetWindowPosY(IntPtr mainWindowHandle)
        {
            try
            {
                Native.RECT windowRect = new Native.RECT();
                Native.GetWindowRect(mainWindowHandle, ref windowRect);
                return windowRect.top;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWindowPosY(IntPtr mainWindowHandle): " + exception);
                return 0;
            }
        }

        public static bool WindowInTaskBarre(IntPtr mainWindowHandle)
        {
            try
            {
                int t = GetWindowPosX(mainWindowHandle);
                if (GetWindowPosY(mainWindowHandle) < -100 && GetWindowPosX(mainWindowHandle) < -100)
                    return true;
                return false;
            }
            catch (Exception exception)
            {
                Logging.WriteError("WindowInTaskBarre(IntPtr mainWindowHandle): " + exception);
                return false;
            }
        }

        /// <summary>
        /// Move and resize Windows.
        /// </summary>
        /// <param name="mainWindowHandle"> </param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static void SetWindowPositionSize(IntPtr mainWindowHandle, int x, int y, int width, int height)
        {
            try
            {
                Native.SetWindowPos(mainWindowHandle, IntPtr.Zero, x, y, width, height, 0u);
            }
            catch (Exception exception)
            {
                Logging.WriteError(
                    "SetWindowPositionSize(IntPtr mainWindowHandle, int x, int y, int width, int height): " + exception);
            }
        }

        /// <summary>
        /// Screenshot Window.
        /// </summary>
        /// <param name="mainWindowHandle"> </param>
        /// <param name="filename"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static void ScreenshotWindow(IntPtr mainWindowHandle, string filename, ImageFormat format)
        {
            try
            {
                Image img = ScreenshotWindow(mainWindowHandle);
                img.Save(filename, format);
            }
            catch (Exception exception)
            {
                Logging.WriteError("ScreenshotWindow(IntPtr mainWindowHandle, string filename, ImageFormat format): " +
                                   exception);
            }
        }

        /// <summary>
        /// Capture Wow Window to Image.
        /// </summary>
        /// <param name="mainWindowHandle"> </param>
        /// <returns></returns>
        public static Image ScreenshotWindow(IntPtr mainWindowHandle)
        {
            try
            {
                IntPtr handle = mainWindowHandle;
                // get te hDC of the target window
                IntPtr hdcSrc = Native.GetWindowDC(handle);

                // get the size
                Native.RECT windowRect = new Native.RECT();
                Native.GetWindowRect(handle, ref windowRect);
                int width = windowRect.right - windowRect.left;
                int height = windowRect.bottom - windowRect.top;

                // create a device context we can copy to
                IntPtr hdcDest = Native.CreateCompatibleDC(hdcSrc);

                // create a bitmap we can copy it to,
                // using GetDeviceCaps to get the width/height
                IntPtr hBitmap = Native.CreateCompatibleBitmap(hdcSrc, width, height);

                // select the bitmap object
                IntPtr hOld = Native.SelectObject(hdcDest, hBitmap);

                // bitblt over
                Native.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, 0x00CC0020);

                // restore selection
                Native.SelectObject(hdcDest, hOld);

                // clean up
                Native.DeleteDC(hdcDest);
                Native.ReleaseDC(handle, hdcSrc);

                // get a .NET image object for it
                Image img = Image.FromHbitmap(hBitmap);

                // free up the Bitmap object
                Native.DeleteObject(hBitmap);
                //img.Save(bitmapFilePath);
                return img;
            }
            catch (Exception exception)
            {
                Logging.WriteError("ScreenshotWindow(IntPtr mainWindowHandle): " + exception);
            }
            return new Bitmap(1, 1);
        }

        /// <summary>
        /// Show Window.
        /// </summary>
        /// <param name="mainWindowHandle"> </param>
        /// <returns></returns>
        public static void ShowWindow(IntPtr mainWindowHandle)
        {
            try
            {
                Native.ShowWindow(mainWindowHandle, Native.SwRestore);
                Native.SetForegroundWindow(mainWindowHandle);
            }
            catch (Exception exception)
            {
                Logging.WriteError("ShowWindow(IntPtr mainWindowHandle): " + exception);
            }
        }
    }
}