using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;
using Math = nManager.Helpful.Math;
using Timer = nManager.Helpful.Timer;

namespace nManager.Wow.Bot.Tasks
{
    public static class LootingTask
    {
        public static void Pulse(IEnumerable<WoWUnit> woWUnits)
        {
            try
            {
                woWUnits = woWUnits.OrderBy(x => x.GetDistance);
                foreach (WoWUnit wowUnit in woWUnits)
                {
                    try
                    {
                        if (Products.Products.IsStarted)
                        {
                            if (nManagerSetting.IsBlackListed(wowUnit.Guid))
                                continue;

                            MovementManager.StopMove();
                            MovementManager.StopMove();
                            Thread.Sleep(250 + Usefuls.Latency);
                            while (ObjectManager.ObjectManager.Me.IsCast)
                                Thread.Sleep(200);
                            if (wowUnit.IsValid)
                            {
                                bool looted = false;
                                if (wowUnit.IsLootable)
                                    Logging.Write("Loot " + wowUnit.Name);
                                else if (wowUnit.IsSkinnable && nManagerSetting.CurrentSetting.ActivateBeastSkinning)
                                    Logging.Write("Skin " + wowUnit.Name);
                                else
                                    continue;

                                List<Point> points = new List<Point>();
                                if (ObjectManager.ObjectManager.Me.Position.DistanceTo(wowUnit.Position) > 4.0f)
                                {
                                    points = PathFinder.FindPath(wowUnit.Position);
                                    MovementManager.Go(points);
                                }
                                if (points.Count <= 0)
                                {
                                    points.Add(ObjectManager.ObjectManager.Me.Position);
                                    points.Add(wowUnit.Position);
                                }
                                Timer timer = new Timer((int) (Math.DistanceListPoint(points)/3*1000) + 5000);
                                while (!ObjectManager.ObjectManager.Me.IsDeadMe && wowUnit.IsValid &&
                                       Products.Products.IsStarted &&
                                       ObjectManager.ObjectManager.GetNumberAttackPlayer() == 0 &&
                                       !(ObjectManager.ObjectManager.Me.InCombat &&
                                         !(ObjectManager.ObjectManager.Me.IsMounted &&
                                           (nManagerSetting.CurrentSetting.IgnoreFightIfMounted || Usefuls.IsFlying))) &&
                                       !timer.IsReady)
                                {
                                    if (ObjectManager.ObjectManager.Me.Position.DistanceTo(wowUnit.Position) <= 4.0f)
                                    {
                                        MovementManager.StopMove();
                                        MovementManager.StopMove();
                                        MountTask.DismountMount();
                                        Thread.Sleep(250);
                                        while (ObjectManager.ObjectManager.Me.GetMove)
                                        {
                                            Thread.Sleep(50);
                                        }

                                        if (wowUnit.IsLootable)
                                        {
                                            Interact.InteractWith(wowUnit.GetBaseAddress);
                                            if ((ObjectManager.ObjectManager.Me.InCombat &&
                                                 !(ObjectManager.ObjectManager.Me.IsMounted &&
                                                   (nManagerSetting.CurrentSetting.IgnoreFightIfMounted ||
                                                    Usefuls.IsFlying))))
                                            {
                                                return;
                                            }
                                            Thread.Sleep(500 + Usefuls.Latency);
                                            if (nManagerSetting.CurrentSetting.ActivateBeastSkinning &&
                                                ObjectManager.ObjectManager.GetNumberAttackPlayer() > 0)
                                                return;
                                            Statistics.Loots++;
                                            if (nManagerSetting.CurrentSetting.MakeStackOfElementalsItems &&
                                                !ObjectManager.ObjectManager.Me.InCombat)
                                                Elemental.AutoMakeElemental();
                                            //Thread.Sleep(250 + Usefuls.Latency);
                                            if (nManagerSetting.CurrentSetting.ActivateBeastSkinning)
                                            {
                                                Thread.Sleep(2000 + Usefuls.Latency); // let the client react to unit flag change
                                                looted = true;
                                            }
                                            else
                                            {
                                                WoWUnit unit = wowUnit;
                                                // we blacklist all unit around for a short time to be sure we loot then
                                                foreach (WoWUnit u in woWUnits.Where(u => u != unit).Where(u => u.Position.DistanceTo(unit.Position) <= 20f))
                                                {
                                                    nManagerSetting.AddBlackList(u.Guid, 2600);
                                                }
                                                nManagerSetting.AddBlackList(wowUnit.Guid, 1000*60*5);
                                                break;
                                            }
                                        }
                                        if (looted && !wowUnit.IsSkinnable)
                                            break;
                                        // From here we are sure the unit is skinnable
                                        // if this is the unit we just looted, we need to redo check for extra loot
                                        // if this is NOT the unit we just looted, then the check is already done at list building time
                                        if (nManagerSetting.CurrentSetting.ActivateBeastSkinning &&
                                            ObjectManager.ObjectManager.GetNumberAttackPlayer() == 0)
                                        {
                                            if (looted)
                                            {
                                                if (wowUnit.ExtraLootType.HasFlag(Enums.TypeFlag.HERB_LOOT))
                                                {
                                                    int myHerbalismLevel = Skill.GetValue(Enums.SkillLine.Herbalism);
                                                    if (myHerbalismLevel > 0)
                                                        myHerbalismLevel += Skill.GetSkillBonus(Enums.SkillLine.Herbalism);
                                                    if (wowUnit.GetSkillLevelRequired > myHerbalismLevel)
                                                    {
                                                        nManagerSetting.AddBlackList(wowUnit.Guid, 1000*60*5);
                                                        break;
                                                    }
                                                }
                                                else if (wowUnit.ExtraLootType.HasFlag(Enums.TypeFlag.MINING_LOOT))
                                                {
                                                    int myMiningLevel = Skill.GetValue(Enums.SkillLine.Mining);
                                                    if (myMiningLevel > 0)
                                                        myMiningLevel += Skill.GetSkillBonus(Enums.SkillLine.Mining);
                                                    if (wowUnit.GetSkillLevelRequired > myMiningLevel)
                                                    {
                                                        nManagerSetting.AddBlackList(wowUnit.Guid, 1000*60*5);
                                                        break;
                                                    }
                                                }
                                                else if (wowUnit.ExtraLootType.HasFlag(Enums.TypeFlag.ENGENEERING_LOOT))
                                                {
                                                    int myEngeneeringLevel = Skill.GetValue(Enums.SkillLine.Engineering);
                                                    if (myEngeneeringLevel > 0)
                                                        myEngeneeringLevel += Skill.GetSkillBonus(Enums.SkillLine.Engineering);
                                                    if (wowUnit.GetSkillLevelRequired > myEngeneeringLevel)
                                                    {
                                                        nManagerSetting.AddBlackList(wowUnit.Guid, 1000*60*5);
                                                        break;
                                                    }
                                                }
                                                else
                                                {
                                                    int mySkinningLevel = Skill.GetValue(Enums.SkillLine.Skinning);
                                                    if (mySkinningLevel <= 0)
                                                    {
                                                        nManagerSetting.AddBlackList(wowUnit.Guid, 1000*60*5);
                                                        break;
                                                    }
                                                }
                                            }
                                            Logging.Write("Skin " + wowUnit.Name);
                                            Interact.InteractWith(wowUnit.GetBaseAddress);
                                            Thread.Sleep(200 + Usefuls.Latency);
                                            while (ObjectManager.ObjectManager.Me.IsCast)
                                            {
                                                Thread.Sleep(100);
                                            }
                                            if ((ObjectManager.ObjectManager.Me.InCombat &&
                                                 !(ObjectManager.ObjectManager.Me.IsMounted &&
                                                   (nManagerSetting.CurrentSetting.IgnoreFightIfMounted ||
                                                    Usefuls.IsFlying))))
                                            {
                                                return;
                                            }
                                            Thread.Sleep(400 + Usefuls.Latency);
                                            if (nManagerSetting.CurrentSetting.ActivateBeastSkinning &&
                                                ObjectManager.ObjectManager.GetNumberAttackPlayer() > 0)
                                                return;
                                            Statistics.Farms++;
                                            nManagerSetting.AddBlackList(wowUnit.Guid, 1000*60*5);
                                            break;
                                        }
                                    }
                                    Thread.Sleep(30);
                                }
                                if (timer.IsReady)
                                    nManagerSetting.AddBlackList(wowUnit.Guid, 1000*60*20);
                            }
                            MovementManager.StopMove();
                            MovementManager.StopMove();
                        }
                    }
                    catch
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError("LootingTask > Pulse(IEnumerable<WoWUnit> woWUnits): " + ex);
            }
        }

        public static void LootAndConfirmBoPForAllItems(bool forceBoP)
        {
            try
            {
                Thread.Sleep(Usefuls.Latency);
                string forcedBoP = forceBoP ? " ConfirmLootSlot(slot) " : "";
                Lua.LuaDoString(
                    "for slot = 1, GetNumLootItems() do " +
                    "    LootSlot(slot) " +
                    forcedBoP +
                    "end");
                Thread.Sleep(Usefuls.Latency);
            }
            catch (Exception ex)
            {
                Logging.WriteError("LootingTask > ConfirmOnBoPItems(): " + ex);
            }
        }
    }
}