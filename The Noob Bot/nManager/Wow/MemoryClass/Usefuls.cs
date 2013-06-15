using System;
using System.Runtime.InteropServices;
using System.Threading;
using nManager.Wow.MemoryClass.Magic;

namespace nManager.Wow.MemoryClass
{
    public class Usefuls
    {
        public struct MEMORY_BASIC_INFORMATION
        {
            public int BaseAddress;
            public int AllocationBase;
            public int AllocationProtect;
            public int RegionSize;
            public int State;
            public int Protect;
            public int Type;
        }

        [DllImport("kernel32.dll")]
        public static extern int VirtualQueryEx(IntPtr hProcess, uint lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer,
                                                int dwLength);

        [DllImport("kernel32")]
        public static extern int LoadLibrary(string librayName);

        [DllImport("kernel32", CharSet = CharSet.Ansi)]
        public static extern int GetProcAddress(int hwnd, string procedureName);

        public class PatternResult
        {
            public uint AllocationBase;
            public uint dwAddress;
            public uint RegionSize;
        }

        public static PatternResult FindPattern(byte[] pattern, string mask)
        {
            const uint num = 0xfffffff;
            uint num2 = 0;
            do
            {
                MEMORY_BASIC_INFORMATION struct2 = new MEMORY_BASIC_INFORMATION();
                VirtualQueryEx(Memory.WowMemory.Memory.ProcessHandle, num2, out struct2, Marshal.SizeOf(struct2));
                try
                {
                    if ((struct2.AllocationBase != 0) && (struct2.RegionSize > 0x1000))
                    {
                        uint num3 = Memory.WowMemory.Memory.FindPattern((uint) struct2.AllocationBase,
                                                                        struct2.RegionSize, pattern, mask);
                        if (num3 != struct2.AllocationBase)
                        {
                            return new PatternResult
                                {
                                    AllocationBase = (uint) struct2.AllocationBase,
                                    dwAddress = num3,
                                    RegionSize = (uint) struct2.RegionSize
                                };
                        }
                    }
                }
                catch
                {
                }
                num2 = (uint) (struct2.BaseAddress + struct2.RegionSize);
                Thread.Sleep(3);
            } while (num2 <= num);
            return null;
        }
    }
}