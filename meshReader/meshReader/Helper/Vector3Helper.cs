using System.IO;
using Microsoft.Xna.Framework;

namespace meshReader.Helper
{

    public static class Vector3Helper
    {
        
        public static Vector3 Read(Stream s)
        {
            var r = new BinaryReader(s);
            return new Vector3(r.ReadSingle(), r.ReadSingle(), r.ReadSingle());
        }

    }

}