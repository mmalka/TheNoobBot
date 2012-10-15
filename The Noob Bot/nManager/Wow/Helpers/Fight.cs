using System;
using System.Collections.Generic;
using System.Threading;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.ObjectManager;
using Math = nManager.Helpful.Math;

namespace nManager.Wow.Helpers
{
    public class Fight
    {
        private static bool _fightLoop;

        public static bool InFight { get { return _fightLoop; } set { _fightLoop = value; } }

        public static ulong StartFight(ulong guid = 0, bool inBg = false)
        {
            MovementManager.StopMove();
            if (ObjectManager.ObjectManager.Me.IsMounted)
                Bot.Tasks.MountTask.DismountMount();
            WoWUnit targetNpc = null;
            try
            {
                if (guid == 0)
                {
                    targetNpc =
                        new WoWUnit(
                            ObjectManager.ObjectManager.GetNearestWoWUnit(
                                ObjectManager.ObjectManager.GetUnitAttackPlayer()).GetBaseAddress);
                }
                else
                {
                    targetNpc =
                        new WoWUnit(ObjectManager.ObjectManager.GetObjectByGuid(guid).GetBaseAddress);
                }

                _fightLoop = true;

                if (!ObjectManager.ObjectManager.Me.IsCast)
                    Interact.InteractGameObject(targetNpc.GetBaseAddress);
                Thread.Sleep(100);
                if (ObjectManager.ObjectManager.Me.GetMove && !ObjectManager.ObjectManager.Me.IsCast)
                {
                    MovementManager.StopMoveTo();
                }

                Point positionStartTarget = targetNpc.Position;

                int timer = 0;
            figthStart:
                // If pos start is very different
                if (targetNpc.Position.DistanceTo(positionStartTarget) > 50)
                    return 0;
                if (inBg)
                {
                    List<WoWUnit> tLUnit = ObjectManager.ObjectManager.GetUnitAttackPlayer();
                    if (tLUnit.Count > 0)
                    {
                        if (ObjectManager.ObjectManager.GetNearestWoWUnit(tLUnit).GetDistance < targetNpc.GetDistance &&
                            ObjectManager.ObjectManager.GetNearestWoWUnit(tLUnit).SummonedBy == 0)
                            return 0;
                    }
                }
                if (TraceLine.TraceLineGo(targetNpc.Position)) // If obstacle
                {
                    List<Point> points = PathFinder.FindPath(targetNpc.Position);
                    MovementManager.Go(points);
                    timer = Others.Times + ((int)Math.DistanceListPoint(points) / 3 * 1000) + 15000;

                    while (!ObjectManager.ObjectManager.Me.IsDeadMe && !targetNpc.IsDead && !targetNpc.IsLootable && targetNpc.Health > 0 && targetNpc.IsValid &&
                            MovementManager.InMovement && _fightLoop && Usefuls.InGame &&
                            (TraceLine.TraceLineGo(targetNpc.Position) ||
                             targetNpc.GetDistance > (CustomClass.GetRange - 1))
                            )
                    {
                        // Mob already in fight
                        if (targetNpc.Type != Enums.WoWObjectType.Player &&
                            !(targetNpc.IsTargetingMe || targetNpc.Target == 0 ||
                                ((WoWUnit)ObjectManager.ObjectManager.GetObjectByGuid(targetNpc.Target)).
                                    SummonedBy == ObjectManager.ObjectManager.Me.Guid ||
                                targetNpc.Target == ObjectManager.ObjectManager.Pet.Guid))
                            return targetNpc.Guid;

                        // Timer
                        if (Others.Times > timer && TraceLine.TraceLineGo(targetNpc.Position))
                            return targetNpc.Guid;

                        // Target Pos Verif
                        if (targetNpc.Position.X == 0.0f && targetNpc.Position.Z == 0.0f)
                            return targetNpc.Guid;

                        // If pos start is very different
                        if (targetNpc.Position.DistanceTo(positionStartTarget) > 50)
                            return 0;

                        // Return if player attacked and this target not attack player
                        if (ObjectManager.ObjectManager.GetNumberAttackPlayer() > 0 && !targetNpc.IsTargetingMe &&
                            !(targetNpc.Target == ObjectManager.ObjectManager.Pet.Guid && targetNpc.Target > 0))
                            return 0;
                        Thread.Sleep(50);
                    }
                }
                if (timer == 0)
                    timer = Others.Times +
                            ((int)ObjectManager.ObjectManager.Me.Position.DistanceTo(targetNpc.Position) / 3 * 1000) +
                            15000;
                if (MovementManager.InMovement)
                {
                    MovementManager.StopMove();
                    MovementManager.StopMove();
                }
                if (!ObjectManager.ObjectManager.Me.IsCast && ObjectManager.ObjectManager.Me.Target != targetNpc.Guid)
                    Interact.InteractGameObject(targetNpc.GetBaseAddress);

                _fightLoop = true;
                Thread.Sleep(500);
                if (targetNpc.GetDistance < (CustomClass.GetRange - 1) && ObjectManager.ObjectManager.Me.GetMove && !ObjectManager.ObjectManager.Me.IsCast)
                {
                    MovementManager.StopMoveTo();
                }
                while (!ObjectManager.ObjectManager.Me.IsDeadMe && !targetNpc.IsDead && targetNpc.IsValid && _fightLoop && targetNpc.GetBaseAddress > 0 && !ObjectManager.ObjectManager.Me.InTransport)
                {
                    // Return if player attacked and this target not attack player
                    if (targetNpc.Type != Enums.WoWObjectType.Player && !targetNpc.IsTargetingMe && !(targetNpc.Target == ObjectManager.ObjectManager.Pet.Guid && targetNpc.Target > 0) && ObjectManager.ObjectManager.GetNumberAttackPlayer() > 0)
                        return 0;

                    // Target Pos Verif
                    if (targetNpc.Position.X == 0.0f && targetNpc.Position.Z == 0.0f)
                        return targetNpc.Guid;

                    // Target mob if not target
                    if ((ObjectManager.ObjectManager.Me.Target != targetNpc.Guid) && !targetNpc.IsDead && !ObjectManager.ObjectManager.Me.IsCast)
                    {
                        Interact.InteractGameObject(targetNpc.GetBaseAddress);
                    }

                    // Move to target if out of range
                    if (targetNpc.GetDistance > CustomClass.GetRange && !ObjectManager.ObjectManager.Me.IsCast)
                    {
                        int rJump = Others.Random(1, 30);
                        if (rJump == 5)
                            Keybindings.PressKeybindings(Enums.Keybindings.JUMP);
                        MovementManager.MoveTo(targetNpc);
                    }
                    // Create path if the mob is out of sight or out of range
                    if ((targetNpc.GetDistance > CustomClass.GetRange && !ObjectManager.ObjectManager.Me.IsCast) || TraceLine.TraceLineGo(targetNpc.Position))
                    {
                        goto figthStart;
                    }
                    // Stop move if in range
                    if (targetNpc.GetDistance < (CustomClass.GetRange - 1) && ObjectManager.ObjectManager.Me.GetMove && !ObjectManager.ObjectManager.Me.IsCast)
                    {
                        MovementManager.StopMoveTo();
                    }

                    // Face player to mob
                    MovementManager.Face(targetNpc);

                    // Timer
                    if (Others.Times > timer && TraceLine.TraceLineGo(targetNpc.Position) && targetNpc.HealthPercent > 90)
                        return targetNpc.Guid;

                    if (ObjectManager.ObjectManager.Me.IsMounted)
                        Bot.Tasks.MountTask.DismountMount();

                    Thread.Sleep(50);
                }

                MovementManager.StopMoveTo();
                _fightLoop = false;
            }
            catch (Exception exception)
            {
                Logging.WriteError("StartFight(ulong guid = 0, bool inBg = false): " + exception); _fightLoop = false;
            }
            try
            {
                if (targetNpc != null) 
                    return targetNpc.Guid;
            }
            catch
            {
                return 0;
            }
            return 0;
        }

        public static void StopFight()
        {
            try
            {
                _fightLoop = false;
            }
            catch (Exception exception)
            {
                Logging.WriteError("StopFight(): " + exception); _fightLoop = false;
            }
        }
    }
}
