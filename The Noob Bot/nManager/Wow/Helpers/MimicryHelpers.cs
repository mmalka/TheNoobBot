using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace nManager.Wow.Helpers
{
    public class MimicryHelpers
    {
        public enum opCodes
        {
            QueryPosition = 1
        }

        public static byte[] StructToBytes(Object str)
        {
            int size = Marshal.SizeOf(str);
            byte[] arr = new byte[size];
            IntPtr ptr = Marshal.AllocHGlobal(size);

            Marshal.StructureToPtr(str, ptr, true);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);

            return arr;
        }

        public static T BytesToStruct<T>(byte[] arr)
        {
            T str = (T)Activator.CreateInstance(typeof(T));
            int size = Marshal.SizeOf(str);
            IntPtr ptr = Marshal.AllocHGlobal(size);

            Marshal.Copy(arr, 0, ptr, size);
            str = (T)Marshal.PtrToStructure(ptr, str.GetType());
            Marshal.FreeHGlobal(ptr);

            return str;
        }
    }
}
