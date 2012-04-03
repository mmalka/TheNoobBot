using System.Collections.Generic;
using System.Linq;
using Questing_Bot.Profile;
using WowManager.MiscStructs;
using WowManager.Navigation;
using WowManager.Others;
using WowManager.WoW.ObjectManager;

namespace Questing_Bot.Bot.Tasks
{
    static class Functions
    {
        public static int CountVendorByType(TypeVendor typeVendor)
        {
            return Config.Bot.Profile.Vendors.Count(v => v.Type == typeVendor);
        }

        public static int CountTrainerByClass(TrainClass trainClass)
        {
            return Config.Bot.Profile.Vendors.Count(v => v.TrainClass == trainClass);
        }

        public static int CountMailBox()
        {
            return Config.Bot.Profile.Mailboxes.Count;
        }

        public static Vendor GetNearestVendor(TypeVendor typeVendor)
        {
            Vendor vendor = null;
            float lastDistance = 999999999999;

            foreach (var v in Config.Bot.Profile.Vendors)
            {
                if (v.Type != typeVendor)
                    continue;

                var distance = v.Position.DistanceTo(ObjectManager.Me.Position);

                if (distance < lastDistance)
                {
                    lastDistance = distance;
                    vendor = v;
                }
            }

            return vendor;
        }

        public static Vendor GetNearestTrainer(TrainClass trainClass)
        {
            Vendor vendor = null;
            float lastDistance = 999999999999;

            foreach (var v in Config.Bot.Profile.Vendors)
            {
                if (v.TrainClass != trainClass)
                    continue;

                var distance = v.Position.DistanceTo(ObjectManager.Me.Position);

                if (distance < lastDistance)
                {
                    lastDistance = distance;
                    vendor = v;
                }
            }

            return vendor;
        }

        public static Mailbox GetNearestMailbox()
        {
            Mailbox mailbox = null;
            float lastDistance = 999999999999;

            foreach (var v in Config.Bot.Profile.Mailboxes)
            {
                var distance = v.Position.DistanceTo(ObjectManager.Me.Position);

                if (distance < lastDistance)
                {
                    lastDistance = distance;
                    mailbox = v;
                }
            }

            return mailbox;
        }

        public static List<Buy> NeededBuy()
        {
            List<Buy> ret = new List<Buy>();
            foreach (var buy in Config.Bot.Profile.BuyItems)
            {
                if (ObjectManager.Me.Level >= buy.MinLevel && ObjectManager.Me.Level <= buy.MaxLevel && ((buy.TypeBuy == TypeBuy.Food && Config.Bot.FormConfig.RegenUseFood) || (buy.TypeBuy == TypeBuy.Water && Config.Bot.FormConfig.RegenUseWater)))
                    ret.Add(buy);
            }
            for (var i = ret.Count - 1 ; i >= 0 ; i--)
            {
                ret[i].Count -= WowManager.WoW.ItemManager.Item.GetItemCountByIdLUA((uint)ret[i].Id);
                if (ret[i].Count <= 0)
                    ret.RemoveAt(i);
            }
            return ret;
        }

        public static string FoodName()
        {
            foreach (var buy in Config.Bot.Profile.BuyItems)
            {
                if (buy.TypeBuy == TypeBuy.Food)
                    if (ObjectManager.Me.Level >= buy.MinLevel && ObjectManager.Me.Level <= buy.MaxLevel)
                        if (WowManager.WoW.ItemManager.Item.GetItemCountByIdLUA((uint)buy.Id) > 0)
                            return WowManager.WoW.ItemManager.Item.GetNameById((uint) buy.Id);
            }
            return string.Empty;
        }
        public static string WaterName()
        {
            foreach (var buy in Config.Bot.Profile.BuyItems)
            {
                if (buy.TypeBuy == TypeBuy.Water)
                    if (ObjectManager.Me.Level >= buy.MinLevel && ObjectManager.Me.Level <= buy.MaxLevel)
                        if (WowManager.WoW.ItemManager.Item.GetItemCountByIdLUA((uint)buy.Id) > 0)
                            return WowManager.WoW.ItemManager.Item.GetNameById((uint)buy.Id);
            }
            return string.Empty;
        }

        public static List<string> KeepItemsList()
        {
            var ret = new List<string>();
            foreach (var keep in Config.Bot.Profile.KeepItems)
            {
                ret.Add(WowManager.WoW.ItemManager.Item.GetNameById((uint)keep.Id));
            }
            return ret;
        }

        public static bool IsInBlackListZone(Point position)
        {
            foreach (var bs in Config.Bot.Profile.Blackspots)
            {
                if (bs.Position.DistanceTo2D(position) <= bs.Radius)
                    return true;
            }
            return false;
        }

        public static bool IsInAvoidMobsList(WowManager.WoW.WoWObject.WoWUnit woWUnit)
        {
            foreach (var npc in Config.Bot.Profile.AvoidMobs)
            {
                if ((npc.Position.X == 0.0f || npc.Position.DistanceTo(woWUnit.Position) <= 40) && npc.Entry == woWUnit.Entry)
                return true;
            }

            return false;
        }

        public static List<Point> GoToPathFind(Point to)
        {
            GoTo bestGoTo = new GoTo();
            // Find Best GoTo:
            float distanceToMe = 999999999;
            float distanceToTo = 999999999;
            foreach (var goTo in Config.Bot.Profile.GoToList)
            {
                if (goTo.Points.Count <= 0)
                    continue;

                float dMe = goTo.Points[Others.NearestPointOfListPoints(goTo.Points, ObjectManager.Me.Position)].DistanceTo(ObjectManager.Me.Position);
                float dTo = goTo.Points[Others.NearestPointOfListPoints(goTo.Points, to)].DistanceTo(to);
                float dMeZ = goTo.Points[Others.NearestPointOfListPoints(goTo.Points, ObjectManager.Me.Position)].DistanceZ(ObjectManager.Me.Position);
                float dToZ = goTo.Points[Others.NearestPointOfListPoints(goTo.Points, to)].DistanceZ((to));

                if (dMeZ <= 35 && dToZ <= 35)
                    if ((dMe + dTo) < (distanceToMe + distanceToTo))
                    {
                        distanceToMe = dMe;
                        distanceToTo = dTo;
                        bestGoTo = goTo;
                    }
            }

            // If GoTo find:
            if (bestGoTo.Points.Count > 0)
            {
                if (distanceToMe < to.DistanceTo(ObjectManager.Me.Position) && distanceToTo < to.DistanceTo(ObjectManager.Me.Position) && distanceToMe <= Config.Bot.Profile.GoToSearchDistance && distanceToTo <= Config.Bot.Profile.GoToSearchDistance) // Distance
                {
                    var iMe = Others.NearestPointOfListPoints(bestGoTo.Points, ObjectManager.Me.Position);
                    var iTo = Others.NearestPointOfListPoints(bestGoTo.Points, to);


                    if (iTo < iMe)
                    { // Reverse list points
                        bestGoTo.Points.Reverse();
                        iMe = Others.NearestPointOfListPoints(bestGoTo.Points, ObjectManager.Me.Position);
                        iTo = Others.NearestPointOfListPoints(bestGoTo.Points, to);
                    }

                    // Delete useless points
                    var ret = new List<Point>();
                    for (var i = iMe; i <= iTo; i++)
                    {
                        ret.Add(bestGoTo.Points[i]);
                    }

                    if (distanceToMe > 4)
                    {
                        var t = new List<Point>();
                        t.AddRange(ret);
                        ret.Clear();
                        ret.AddRange(PathFinderManager.FindPath(t[0]));
                        ret.AddRange(t);
                    }

                    if (distanceToTo > 4)
                    {
                        ret.AddRange(PathFinderManager.FindPath(ret[ret.Count-1], to));
                    }

                    return ret;
                }
            }

            // If not goto:
            return PathFinderManager.FindPath(to);
        }
    }
}
