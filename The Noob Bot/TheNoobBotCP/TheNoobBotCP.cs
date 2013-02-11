/*
* CustomProfile for TheNoobBot
* Credit : Vesper
*/

using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using nManager.Helpful;
using nManager.Products;
using nManager.Wow.Class;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;

public class Main : ICustomProfile
{
    internal static bool Loop = true;

    #region ICustomProfile Members

    internal static bool _ignoreFight;
    public string BattlegroundId;

    public bool IgnoreFight
    {
        get { return _ignoreFight; }
        set { _ignoreFight = value; }
    }

    public void Initialize()
    {
        try
        {
            if (!Loop)
                Loop = true;
            _ignoreFight = false;
            Logging.WriteFight("Loading TheNoobBot example Custom Profile.");
            BattlegroundId = Battleground.GetCurrentBattleground().ToString();
            if (BattlegroundId != null)
                if (BattlegroundId == "WarsongGulch")
                {
                    new CaptureTheFlagWG();
                }
        }
        catch (Exception exception)
        {
            Logging.WriteError("Initialize(): " + exception);
        }
        Logging.WriteFight("TheNoobBot example Custom Profile stopped. Loop shutdown.");
    }

    public void Dispose()
    {
        Logging.WriteFight("TheNoobBot example Custom Profile stopped.");
        Loop = false;
    }

    public void ShowConfiguration()
    {
        MessageBox.Show("There is no settings available");
    }

    public void ResetConfiguration()
    {
        MessageBox.Show("There is no settings available");
    }

    #endregion
}

public class CaptureTheFlagWG
{
    /**
      * Author : VesperCore
    **/

    private readonly int _allianceFlagFloorId;
    private readonly int _allianceFlagId;
    private readonly Point _allianceFlagPosition;

    private readonly int _hordeFlagFloorId;
    private readonly int _hordeFlagId;
    private readonly Point _hordeFlagPosition;

    public CaptureTheFlagWG()
    {
        _allianceFlagPosition = new Point((float) 1540.423, (float) 1481.325, (float) 351.8284);
        _allianceFlagId = 179830;
        _allianceFlagFloorId = 179785;
        _hordeFlagPosition = new Point((float) 918.0743, (float) 1433.238, (float) 346.3038);
        _hordeFlagId = _allianceFlagId + 1;
        _hordeFlagFloorId = _allianceFlagFloorId + 1;
        while (Main.Loop)
        {
            try
            {
                Main._ignoreFight = false;
                if (Usefuls.InGame && !Usefuls.IsLoadingOrConnecting && !ObjectManager.Me.IsDeadMe &&
                    ObjectManager.Me.IsValid && !ObjectManager.Me.InCombat && Products.IsStarted &&
                    Battleground.IsInBattleground() && !Battleground.IsFinishBattleground() &&
                    Battleground.BattlegroundIsStarted())
                {
                    if (ObjectManager.Me.IsHoldingWGFlag && !ObjectManager.IsSomeoneHoldingWGFlag())
                    {
                        // 1 possibility :
                        // Go to my base and capture it.
                        Logging.Write("Go to my base and capture it.");
                        InternalGoTo(ObjectManager.Me.PlayerFaction.ToLower() == "horde"
                                         ? _hordeFlagPosition
                                         : _allianceFlagPosition, ObjectManager.Me.IsHoldingWGFlag,
                                     ObjectManager.IsSomeoneHoldingWGFlag(), ObjectManager.IsSomeoneHoldingWGFlag(false));
                    }
                    else if (ObjectManager.Me.IsHoldingWGFlag)
                    {
                        // 4 possibilities :
                        // Go to my base and wait until I can capture it.
                        Logging.Write("Go to my base and wait until I can capture it.");
                        Main._ignoreFight = true;
                        InternalGoTo(ObjectManager.Me.PlayerFaction.ToLower() == "horde"
                                         ? _hordeFlagPosition
                                         : _allianceFlagPosition, ObjectManager.Me.IsHoldingWGFlag,
                                     ObjectManager.IsSomeoneHoldingWGFlag(), ObjectManager.IsSomeoneHoldingWGFlag(false));
                        // Go to my base and wait in a protected area until I can capture it.
                        // Search & Destroy the Hostile Flag holder if not so far from my base.
                        // Search & Destroy the Hostile Flag holder.
                    }
                    else if (!ObjectManager.IsSomeoneHoldingWGFlag(false))
                    {
                        // 1 possibility :
                        // Go to the ennemy base and take it.
                        Logging.Write("Go to the ennemy base and take it.");
                        Main._ignoreFight = true;
                        InternalGoTo(ObjectManager.Me.PlayerFaction.ToLower() == "horde"
                                         ? _allianceFlagPosition
                                         : _hordeFlagPosition, ObjectManager.Me.IsHoldingWGFlag,
                                     ObjectManager.IsSomeoneHoldingWGFlag(), ObjectManager.IsSomeoneHoldingWGFlag(false),
                                     "Take");
                    }
                    else if (ObjectManager.IsSomeoneHoldingWGFlag())
                    {
                        // 4 possibilities :
                        Logging.Write("Go to the ennemy base and wait until I can take it.");
                        Main._ignoreFight = false;
                        // Go to the ennemy base and wait until I can take it.
                        InternalGoTo(ObjectManager.Me.PlayerFaction.ToLower() == "horde"
                                         ? _allianceFlagPosition
                                         : _hordeFlagPosition, ObjectManager.Me.IsHoldingWGFlag,
                                     ObjectManager.IsSomeoneHoldingWGFlag(), ObjectManager.IsSomeoneHoldingWGFlag(false),
                                     "Take");
                        // Go to the ennemy base and wait in a protected area until I can take it.
                        // Protect the ally Flag holder.
                        // Search & Destroy the Hostile Flag holder.
                    }
                    else
                    {
                        // 3 possibilities :
                        Logging.Write("Go to the ennemy base and wait until I can take it.");
                        Main._ignoreFight = false;
                        // Go to the ennemy base and wait until I can take it.
                        InternalGoTo(ObjectManager.Me.PlayerFaction.ToLower() == "horde"
                                         ? _allianceFlagPosition
                                         : _hordeFlagPosition, ObjectManager.Me.IsHoldingWGFlag,
                                     ObjectManager.IsSomeoneHoldingWGFlag(), ObjectManager.IsSomeoneHoldingWGFlag(false),
                                     "Take");
                        // Go to the ennemy base and wait in a protected area until I can take it.
                        // Protect the ally Flag holder.
                    }
                }
                else
                    Thread.Sleep(500);
            }
            catch (Exception e)
            {
                Logging.WriteError("Custom Profile > TheNoobBotCP > CaptureTheFlagWG > Loop: " + e);
            }
            Thread.Sleep(150);
        }
    }

    public void InternalGoTo(Point point, bool isHoldingWGFlag, bool isSomeoneHoldingMyFlag,
                             bool isSomeoneHoldingThemFlag, string goal = "Capture")
    {
        if (CurrentInformationsHasChanged(Main._ignoreFight, isHoldingWGFlag, isSomeoneHoldingMyFlag,
                                          isSomeoneHoldingThemFlag))
            return;
        if (point.DistanceTo(ObjectManager.Me.Position) > 0)
        {
            if (MovementManager.CurrentPath == null || MovementManager.CurrentPath.Count <= 0 ||
                (MovementManager.CurrentPath[MovementManager.CurrentPath.Count - 1].DistanceTo(point) > 1))
            {
                MovementManager.StopMove();
                if (MovementManager.CurrentPath != null && MovementManager.CurrentPath.Count <= 0)
                    Logging.Write(
                        "InternalGoTo : StopMove : MovementManager.CurrentPath.Count <= 0");
                else if (MovementManager.CurrentPath != null)
                    Logging.Write(
                        "InternalGoTo : StopMove : MovementManager.CurrentPath[MovementManager.CurrentPath.Count - 1].DistanceTo(point) = " +
                        MovementManager.CurrentPath[MovementManager.CurrentPath.Count - 1].DistanceTo(point));
                else
                    Logging.Write(
                        "InternalGoTo : StopMove : MovementManager.CurrentPath = null");
            }
            List<Point> points = PathFinder.FindPath(point);
            MovementManager.Go(points);
        }
        while ((MovementManager.InMovement ||
                (!MovementManager.InMovement && ObjectManager.Me.Position.DistanceTo(point) < 3)))
        {
            if (CurrentInformationsHasChanged(Main._ignoreFight, isHoldingWGFlag, isSomeoneHoldingMyFlag,
                                              isSomeoneHoldingThemFlag))
                return;
            switch (goal)
            {
                case "Capture":
                    Main._ignoreFight = ObjectManager.Me.IsHoldingWGFlag;
                    break;
                case "Take":
                    InternalGoToGameObject(
                        ObjectManager.Me.PlayerFaction.ToLower() == "horde"
                            ? _allianceFlagFloorId
                            : _hordeFlagFloorId,
                        isHoldingWGFlag, isSomeoneHoldingMyFlag, isSomeoneHoldingThemFlag);
                    InternalGoToGameObject(
                        ObjectManager.Me.PlayerFaction.ToLower() == "horde" ? _allianceFlagId : _hordeFlagId,
                        isHoldingWGFlag, isSomeoneHoldingMyFlag, isSomeoneHoldingThemFlag);
                    break;
            }
            Thread.Sleep(100);
        }
    }

    public void InternalGoToGameObject(int entry, bool isHoldingWGFlag, bool isSomeoneHoldingMyFlag,
                                       bool isSomeoneHoldingThemFlag)
    {
        if (CurrentInformationsHasChanged(Main._ignoreFight, isHoldingWGFlag, isSomeoneHoldingMyFlag,
                                          isSomeoneHoldingThemFlag))
            return;
        WoWGameObject obj = ObjectManager.GetNearestWoWGameObject(ObjectManager.GetWoWGameObjectByEntry(entry));
        if (obj.GetBaseAddress <= 0) return;
        if (ObjectManager.Me.Position.DistanceTo(obj.Position) > 50)
            return;
        if (MovementManager.CurrentPath == null || MovementManager.CurrentPath.Count <= 0 ||
            (MovementManager.CurrentPath[MovementManager.CurrentPath.Count - 1].DistanceTo(obj.Position) > 1))
        {
            MovementManager.StopMove();
            if (MovementManager.CurrentPath != null && MovementManager.CurrentPath.Count <= 0)
                Logging.Write(
                    "InternalGoToGameObject : StopMove : MovementManager.CurrentPath.Count <= 0");
            else if (MovementManager.CurrentPath != null)
                Logging.Write(
                    "InternalGoToGameObject : StopMove : MovementManager.CurrentPath[MovementManager.CurrentPath.Count - 1].DistanceTo(point) = " +
                    MovementManager.CurrentPath[MovementManager.CurrentPath.Count - 1].DistanceTo(obj.Position));
            else
                Logging.Write(
                    "InternalGoToGameObject : StopMove : MovementManager.CurrentPath = null");
            if (CurrentInformationsHasChanged(Main._ignoreFight, isHoldingWGFlag, isSomeoneHoldingMyFlag,
                                              isSomeoneHoldingThemFlag))
                return;
        }
        Main._ignoreFight = true;
        List<Point> points = PathFinder.FindPath(obj.Position);
        MovementManager.Go(points);
        Logging.Write("Going to Object: " + obj.Name + "@" + obj.Position + " From: " + ObjectManager.Me.Position);
        Thread.Sleep(300);
        while ((MovementManager.InMovement && ObjectManager.Me.Position.DistanceTo(obj.Position) <= 50) ||
               (!MovementManager.InMovement && ObjectManager.Me.Position.DistanceTo(obj.Position) <= 1))
        {
            if (CurrentInformationsHasChanged(Main._ignoreFight, isHoldingWGFlag, isSomeoneHoldingMyFlag,
                                              isSomeoneHoldingThemFlag))
                return;
            Point newPosition =
                ObjectManager.GetNearestWoWGameObject(ObjectManager.GetWoWGameObjectByEntry(entry)).Position;
            if (newPosition != obj.Position)
            {
                Logging.Write("Object: " + obj.Name + "@" + obj.Position + " has changed position to : " + newPosition);
                return;
            }
            if (ObjectManager.Me.Position.DistanceTo(obj.Position) < 3)
            {
                while (MovementManager.InMovement)
                {
                    if (CurrentInformationsHasChanged(Main._ignoreFight, isHoldingWGFlag, isSomeoneHoldingMyFlag,
                                                      isSomeoneHoldingThemFlag))
                        return;
                    Thread.Sleep(500);
                }
                while (
                    !CurrentInformationsHasChanged(Main._ignoreFight, isHoldingWGFlag, isSomeoneHoldingMyFlag,
                                                   isSomeoneHoldingThemFlag))
                {
                    Interact.InteractGameObject(obj.GetBaseAddress);
                    Thread.Sleep(Usefuls.Latency + 500);
                }
            }
            Thread.Sleep(100);
        }
    }

    private static bool CurrentInformationsHasChanged(bool ignoreFight, bool isHoldingWGFlag,
                                                      bool isSomeoneHoldingMyFlag,
                                                      bool isSomeoneHoldingThemFlag)
    {
        if (!Usefuls.InGame || Usefuls.IsLoadingOrConnecting || ObjectManager.Me.IsDeadMe ||
            !ObjectManager.Me.IsValid || (ObjectManager.Me.InCombat && !ignoreFight) || !Products.IsStarted ||
            !Battleground.IsInBattleground() || Battleground.IsFinishBattleground() ||
            !Battleground.BattlegroundIsStarted())
            return true;
        if (isHoldingWGFlag && !isSomeoneHoldingMyFlag)
        {
            if (!ObjectManager.Me.IsHoldingWGFlag || ObjectManager.IsSomeoneHoldingWGFlag())
                return true;
        }
        else
        {
            if (isHoldingWGFlag)
            {
                if (!ObjectManager.Me.IsHoldingWGFlag)
                    return true;
            }
            else
            {
                if (!isSomeoneHoldingThemFlag)
                {
                    if (ObjectManager.IsSomeoneHoldingWGFlag(false))
                        return true;
                }
                else
                {
                    if (isSomeoneHoldingMyFlag)
                    {
                        if (!ObjectManager.IsSomeoneHoldingWGFlag())
                            return true;
                    }
                    else
                    {
                        if (ObjectManager.IsSomeoneHoldingWGFlag(false))
                            return true;
                    }
                }
            }
        }
        return false;
    }
}