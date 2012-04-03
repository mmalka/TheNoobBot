using System;
using System.Runtime.InteropServices;

namespace nManager.Helpful.Win32
{
    public static class Native
    {
        #region Process

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer,
                                                      int dwSize, out int lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize,
                                                       out int lpNumberOfBytesWritten);

        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr hHandle);

        #endregion Process

        #region Hardware

        // Mouse
        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int x, int y);
        [DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        // Display
        [DllImport(@"user32.dll", EntryPoint = "SetWindowPos", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern int SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);
        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowRect(IntPtr hWnd, ref RECT rect);
        [DllImport("gdi32.dll")]
        public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest,
        int nWidth, int nHeight, IntPtr hObjectSource,
        int nXSrc, int nYSrc, int dwRop);
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth,
        int nHeight);
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
        [DllImport("gdi32.dll")]
        public static extern bool DeleteDC(IntPtr hDC);
        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);
        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }
        [DllImport("User32.DLL")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        public const int SwShow = 5;
        public const int SwMinimize = 6;
        public const int SwRestore = 9;
        [DllImport("User32")]
        public static extern int SetForegroundWindow(IntPtr hwnd);

        // Keyboard
        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(IntPtr hwnd, uint wMsg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, long dwFlags, long dwExtraInfo);
        [DllImport("user32.dll")]
        public static extern short VkKeyScan(char ch);
        public const uint DownKeyVk = 0x100;
        public const uint UpKeyVk = 0x101;

        #endregion Hardeware

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool FreeConsole();
    }
}
