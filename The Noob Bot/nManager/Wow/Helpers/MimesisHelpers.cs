using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using nManager.Helpful;

namespace nManager.Wow.Helpers
{
    public class MimesisHelpers
    {
        public enum opCodes
        {
            QueryPosition = 1,
            ReplyPosition = 11,
            QueryEvent = 2,
            ReplyEvent = 21,
            QueryGuid = 3,
            ReplyGuid = 31,
            RequestGrouping = 4,
            ReplyGrouping = 51,
            Disconnect = 9,
        }

        public enum eventType
        {
            none = 0,
            pickupQuest = 1,
            turninQuest = 2,
            interactObject = 3,
            mount = 4,
        }

        [Serializable]
        public struct MimesisEvent
        {
            public uint SerialNumber;
            public eventType eType;
            public int EventValue1;
            public int EventValue2;
            public string EventString1;
        }

        public static T BytesToObject<T>(byte[] arrBytes)
        {
            System.IO.MemoryStream memStream = new System.IO.MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, System.IO.SeekOrigin.Begin);
            T obj = (T) binForm.Deserialize(memStream);
            return obj;
        }

        public static byte[] ObjectToBytes(Object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            bf.Serialize(ms, obj);
            return ms.ToArray();
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
            T str = (T) Activator.CreateInstance(typeof (T));
            int size = Marshal.SizeOf(str);
            IntPtr ptr = Marshal.AllocHGlobal(size);

            Marshal.Copy(arr, 0, ptr, size);
            str = (T) Marshal.PtrToStructure(ptr, str.GetType());
            Marshal.FreeHGlobal(ptr);

            return str;
        }

        public static byte[] StringToBytes(string str)
        {
            byte[] bytes = new byte[str.Length*sizeof (char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static string BytesToString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length/sizeof (char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }
    }
}