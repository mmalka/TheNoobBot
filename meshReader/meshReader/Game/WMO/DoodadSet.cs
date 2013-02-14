using System.IO;
using System.Text;

namespace meshReader.Game.WMO
{

    public class DoodadSet
    {
        public string Name;
        public uint FirstInstanceIndex;
        public uint CountInstances;
        public uint UnknownZero;

        public static DoodadSet Read(Stream s)
        {
            var r = new BinaryReader(s);
            var ret = new DoodadSet();
            ret.Name = Encoding.ASCII.GetString(r.ReadBytes(20));
            ret.FirstInstanceIndex = r.ReadUInt32();
            ret.CountInstances = r.ReadUInt32();
            ret.UnknownZero = r.ReadUInt32();
            return ret;
        }
    }

}