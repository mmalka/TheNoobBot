using System.Collections.Generic;
using WowManager.MiscStructs;
using WowManager.WoW.ObjectManager;

namespace Questing_Bot.Bot.Tasks
{
    public class Black
    {
        ulong guid = 0;
        int time = 0;
        Point position = new Point();
        float radius = 0.0f;

        public Black(ulong guid)
        {
            this.guid = guid;
        }

        public Black(ulong guid, int time)
        {
            this.guid = guid;
            this.time = WowManager.Others.Others.Times + time;
        }

        public Black(ulong guid, Point position, float radius)
        {
            this.guid = guid;
            this.position = position;
            this.radius = radius;
        }

        public ulong Guid { get { return guid; } }
        public int Time { get { return time; } }
        public Point Position { get { return position; } }
        public float Radius { get { return radius; } }

        public static List<ulong> ListGuidValid(List<Black> blacks)
        {
            var ret = new List<ulong>();
            foreach (var b in blacks)
            {
                if (b.Time >= WowManager.Others.Others.Times || b.Time == 0)
                {
                    if (b.Position.DistanceTo(ObjectManager.Me.Position) > b.Radius || b.Radius == 0.0f)
                    {
                        ret.Add(b.Guid);
                    }
                }
            }

            return ret;
        }
    }
}
