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
using Math = System.Math;

public class Main : ICustomProfile
{
    internal static bool Loop = true;

    #region ICustomProfile Members

    internal static bool InternalIgnoreFight;
    public string BattlegroundId;

    public bool IgnoreFight
    {
        get { return InternalIgnoreFight; }
        set { InternalIgnoreFight = value; }
    }

    public void Initialize()
    {
        try
        {
            if (!Loop)
                Loop = true;
            InternalIgnoreFight = false;
            Logging.WriteFight("Loading TheNoobBot example Custom Profile.");
            BattlegroundId = Battleground.GetCurrentBattleground().ToString();
            if (BattlegroundId != null)
                if (BattlegroundId == "WarsongGulch")
                {
                    var ctf = new CaptureTheFlagWG();
                    Logging.WriteFight("CaptureTheFlagWG stopped, broken ?");
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
        MessageBox.Show(@"There is no settings available");
    }

    public void ResetConfiguration()
    {
        MessageBox.Show(@"There is no settings available");
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
                Main.InternalIgnoreFight = false;
                if (Usefuls.InGame && !Usefuls.IsLoadingOrConnecting && !ObjectManager.Me.IsDeadMe &&
                    ObjectManager.Me.IsValid && Products.IsStarted &&
                    Battleground.IsInBattleground() && !Battleground.IsFinishBattleground() &&
                    Battleground.BattlegroundIsStarted())
                {
                    if (ObjectManager.Me.IsHoldingWGFlag && !ObjectManager.IsSomeoneHoldingWGFlag())
                    {
                        // 1 possibility :
                        // Go to my base and capture it.
                        Logging.Write("Go to my base and capture it.");
                        Main.InternalIgnoreFight = true;
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
                        Main.InternalIgnoreFight = true;
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
                        Main.InternalIgnoreFight = true;
                        InternalGoTo(ObjectManager.Me.PlayerFaction.ToLower() == "horde"
                                         ? _allianceFlagPosition
                                         : _hordeFlagPosition, ObjectManager.Me.IsHoldingWGFlag,
                                     ObjectManager.IsSomeoneHoldingWGFlag(), ObjectManager.IsSomeoneHoldingWGFlag(false),
                                     "Take");
                    }
                    else if (ObjectManager.IsSomeoneHoldingWGFlag())
                    {
                        // 4 possibilities :
                        Logging.Write("Go to the ennemy base and wait until I can take it. #1");
                        Main.InternalIgnoreFight = false;
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
                        Logging.Write("Go to the ennemy base and wait until I can take it. #2");
                        Main.InternalIgnoreFight = false;
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
        Point initialPos = ObjectManager.Me.Position;
        if (CurrentInformationsHasChanged(Main.InternalIgnoreFight, isHoldingWGFlag, isSomeoneHoldingMyFlag,
                                          isSomeoneHoldingThemFlag))
            goto End;
        if (point.DistanceTo(ObjectManager.Me.Position) > 1.5)
        {
            if (!CheckPath(point, isHoldingWGFlag, isSomeoneHoldingMyFlag, isSomeoneHoldingThemFlag))
                goto End;
            List<Point> points = PathFinder.FindPathUnstuck(point);
            MovementManager.Go(points);
        }
        while (MovementManager.InMovement ||
               !MovementManager.InMovement && ObjectManager.Me.Position.DistanceTo(point) <= 1.5)
        {
            if (!CheckPath(point, isHoldingWGFlag, isSomeoneHoldingMyFlag, isSomeoneHoldingThemFlag))
                goto End;
            switch (goal)
            {
                case "Capture":
                    Main.InternalIgnoreFight = ObjectManager.Me.IsHoldingWGFlag;
                    break;
                case "Take":
                    if (ObjectManager.Me.Position.DistanceTo(point) <= 1.5)
                        InternalGoToGameObject(
                            ObjectManager.Me.PlayerFaction.ToLower() == "horde" ? _allianceFlagId : _hordeFlagId,
                            isHoldingWGFlag, isSomeoneHoldingMyFlag, isSomeoneHoldingThemFlag);
                    InternalGoToGameObject(
                        ObjectManager.Me.PlayerFaction.ToLower() == "horde"
                            ? _allianceFlagFloorId
                            : _hordeFlagFloorId,
                        isHoldingWGFlag, isSomeoneHoldingMyFlag, isSomeoneHoldingThemFlag);
                    break;
            }
            Thread.Sleep(100);
        }
        End:
        Logging.Write("InternalGoTo ended after traveling " + ObjectManager.Me.Position.DistanceTo(initialPos) +
                      " yards");
        Logging.Write("Initial Position: " + initialPos);
        Logging.Write("Current Position: " + ObjectManager.Me.Position);
        Logging.Write("Destination: " + point);
        Logging.Write("Last distance to destination: " + ObjectManager.Me.Position.DistanceTo(point));
    }

    public void InternalGoToGameObject(int entry, bool isHoldingWGFlag, bool isSomeoneHoldingMyFlag,
                                       bool isSomeoneHoldingThemFlag)
    {
        Point initialPos = ObjectManager.Me.Position;
        var obj = new WoWGameObject(0);
        if (CurrentInformationsHasChanged(Main.InternalIgnoreFight, isHoldingWGFlag, isSomeoneHoldingMyFlag,
                                          isSomeoneHoldingThemFlag))
            goto End;
        obj = ObjectManager.GetNearestWoWGameObject(ObjectManager.GetWoWGameObjectByEntry(entry));
        if (obj.GetBaseAddress <= 0) goto End;
        if (ObjectManager.Me.Position.DistanceTo(obj.Position) > 100)
        {
            Logging.Write("Found object " + obj.Name + ", but it's out of our radius.");
            goto End;
        }
        if (ObjectManager.Me.Position.DistanceTo(obj.Position) > 1.5)
        {
            if (!CheckPath(obj.Position, isHoldingWGFlag, isSomeoneHoldingMyFlag, isSomeoneHoldingThemFlag))
                goto End;
            Main.InternalIgnoreFight = ObjectManager.Me.Position.DistanceTo(obj.Position) < 15;
            List<Point> points = PathFinder.FindPathUnstuck(obj.Position);
            MovementManager.Go(points);
            Logging.Write("Found object " + obj.Name + " at coordonate: " + obj.Position.X + ", " + obj.Position.Y +
                          ", " + obj.Position.Z + "; Currently at: " + ObjectManager.Me.Position.X + ", " +
                          ObjectManager.Me.Position.Y + ", " + ObjectManager.Me.Position.Z);
        }
        while ((MovementManager.InMovement && ObjectManager.Me.Position.DistanceTo(obj.Position) <= 100) ||
               (!MovementManager.InMovement && ObjectManager.Me.Position.DistanceTo(obj.Position) <= 1.5))
        {
            if (!CheckPath(obj.Position, isHoldingWGFlag, isSomeoneHoldingMyFlag, isSomeoneHoldingThemFlag))
                goto End;
            Point newPosition =
                ObjectManager.GetNearestWoWGameObject(ObjectManager.GetWoWGameObjectByEntry(entry)).Position;
            if (Math.Abs(newPosition.X - obj.Position.X) > 0 || Math.Abs(newPosition.Y - obj.Position.Y) > 0 ||
                Math.Abs(newPosition.Z - obj.Position.Z) > 0)
            {
                Logging.Write("Object " + obj.Name + " has changed position To: " + newPosition.X + ", " +
                              newPosition.Y + ", " + newPosition.Z + "; From: " + obj.Position.X + ", " +
                              obj.Position.Y + ", " + obj.Position.Z);
                goto End;
            }
            if (ObjectManager.Me.Position.DistanceTo(obj.Position) <= 1.5)
            {
                while (true)
                {
                    Interact.InteractGameObject(obj.GetBaseAddress);
                    Thread.Sleep(Usefuls.Latency + 500);
                    if (CurrentInformationsHasChanged(Main.InternalIgnoreFight, isHoldingWGFlag, isSomeoneHoldingMyFlag,
                                                      isSomeoneHoldingThemFlag))
                        return;
                    Interact.InteractGameObject(obj.GetBaseAddress);
                    Thread.Sleep(Usefuls.Latency + 1000);
                }
            }
            Thread.Sleep(100);
        }
        End:
        Logging.Write("InternalGoToGameObject ended after traveling " + initialPos.DistanceTo(ObjectManager.Me.Position) +
                      " yards");
        Logging.Write("Initial Position: " + initialPos);
        Logging.Write("Current Position: " + ObjectManager.Me.Position);
        Logging.Write("Searching for object entry: " + entry);
        if (obj.IsValid)
        {
            Logging.Write("Object " + obj.Name + " found.");
            Logging.Write("Position: " + obj.Position);
            Logging.Write("Last distance to target: " + ObjectManager.Me.Position.DistanceTo(obj.Position));
        }
        else Logging.Write("Object not found.");
    }

    private static bool CurrentInformationsHasChanged(bool ignoreFight, bool isHoldingWGFlag,
                                                      bool isSomeoneHoldingMyFlag,
                                                      bool isSomeoneHoldingThemFlag)
    {
        if (!Usefuls.InGame || Usefuls.IsLoadingOrConnecting || ObjectManager.Me.IsDeadMe ||
            !ObjectManager.Me.IsValid || !Products.IsStarted ||
            !Battleground.IsInBattleground() || Battleground.IsFinishBattleground() ||
            !Battleground.BattlegroundIsStarted())
        {
            Logging.Write("Current Informations Has Changed. #1");
            return true;
        }
        if (isHoldingWGFlag && !isSomeoneHoldingMyFlag)
        {
            if (!ObjectManager.Me.IsHoldingWGFlag || ObjectManager.IsSomeoneHoldingWGFlag())
            {
                Logging.Write("Current Informations Has Changed. #2");
                return true;
            }
        }
        else
        {
            if (isHoldingWGFlag)
            {
                if (!ObjectManager.Me.IsHoldingWGFlag)
                {
                    Logging.Write("Current Informations Has Changed. #3");
                    return true;
                }
            }
            else
            {
                if (!isSomeoneHoldingThemFlag)
                {
                    if (ObjectManager.IsSomeoneHoldingWGFlag(false))
                    {
                        Logging.Write("Current Informations Has Changed. #4");
                        return true;
                    }
                }
                else
                {
                    if (isSomeoneHoldingMyFlag)
                    {
                        if (!ObjectManager.IsSomeoneHoldingWGFlag())
                        {
                            Logging.Write("Current Informations Has Changed. #5");
                            return true;
                        }
                    }
                    else
                    {
                        if (!ObjectManager.IsSomeoneHoldingWGFlag(false))
                        {
                            Logging.Write("Current Informations Has Changed. #6");
                            return true;
                        }
                    }
                }
            }
        }
        if (ObjectManager.Me.InCombat && !ignoreFight)
        {
            Logging.Write("Current Informations Has Changed. #7");
            return true;
        }
        return false;
    }

    private static bool CheckPath(Point destination, bool isHoldingWGFlag,
                                  bool isSomeoneHoldingMyFlag,
                                  bool isSomeoneHoldingThemFlag)
    {
        if (CurrentInformationsHasChanged(Main.InternalIgnoreFight, isHoldingWGFlag, isSomeoneHoldingMyFlag,
                                          isSomeoneHoldingThemFlag))
            return false;
        if (MovementManager.CurrentPath == null)
        {
            MovementManager.StopMove();
            Logging.Write("InternalGoTo : MovementManager.CurrentPath = null; InMovement = " +
                          MovementManager.InMovement);
        }
        else if (MovementManager.CurrentPath.Count <= 0)
        {
            MovementManager.StopMove();
            Logging.Write("InternalGoTo : MovementManager.CurrentPath.Count <= 0; InMovement = " +
                          MovementManager.InMovement);
        }
        else if (MovementManager.CurrentPath[MovementManager.CurrentPath.Count - 1].DistanceTo(destination) > 1.5)
        {
            MovementManager.StopMove();
            Logging.Write(
                "InternalGoTo : MovementManager.CurrentPath[MovementManager.CurrentPath.Count - 1].DistanceTo(point) = " +
                MovementManager.CurrentPath[MovementManager.CurrentPath.Count - 1].DistanceTo(destination) +
                "; InMovement = " + MovementManager.InMovement);
        }
        return true;
    }
}