using System.Collections.Generic;
using System.IO;
using System.Text;

namespace meshReader.Helper
{

    public static class StreamExtensions
    {
        
        public static string ReadCString(this Stream s)
        {
            var buf = new List<byte>(30);
            while (true)
            {
                var b = s.ReadByte();
                if (b == 0)
                    break;
                buf.Add((byte)b);
            }
            if (buf.Count <= 0)
                return null;
            return Encoding.ASCII.GetString(buf.ToArray());
        }

    }

}