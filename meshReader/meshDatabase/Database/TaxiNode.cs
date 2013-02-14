using System.Linq;
using Microsoft.Xna.Framework;

namespace meshDatabase.Database
{
    
    public class TaxiNode
    {
        public static readonly int[] KnownAllianceMounts = new[] { 308, 541, 3837 };
        public static readonly int[] KnownHordeMounts = new[] { 2224, 3574 };

        public int Id { get; private set; }
        public string Name { get; private set; }
        public int MapId { get; private set; }
        public Vector3 Location { get; private set; }

        public bool IsHorde { get; private set; }
        public bool IsAlliance { get; private set; }

        public bool IsValid
        {
            get { return IsHorde || IsAlliance; }
        }

        public TaxiNode(Record src)
        {
            Id = src[0];
            Name = src.GetString(5);
            MapId = src[1];
            Location = new Vector3(src.GetFloat(2), src.GetFloat(3), src.GetFloat(4));

            IsHorde = KnownHordeMounts.Contains(src[6]) || KnownHordeMounts.Contains(src[7]);
            IsAlliance = KnownAllianceMounts.Contains(src[6]) || KnownAllianceMounts.Contains(src[7]);
        }
    }

}