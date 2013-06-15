/*
* CustomProfile for TheNoobBot
* Credit : Vesper
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using nManager.Helpful;
using nManager.Products;
using nManager.Wow.Class;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;
using Math = System.Math;

public class Main : ICustomProfile
{
    internal static bool Loop = true;

    #region ICustomProfile Members

    internal static bool InternalIgnoreFight;
    internal static bool InternalDontStartFights;
    public BattlegroundId CurrentBattlegroundId;

    public bool IgnoreFight
    {
        get { return InternalIgnoreFight; }
        set { InternalIgnoreFight = value; }
    }

    public bool DontStartFights
    {
        get { return InternalDontStartFights; }
        set { InternalDontStartFights = value; }
    }

    public void Initialize()
    {
        Initialize(false);
    }

    public void ShowConfiguration()
    {
        Directory.CreateDirectory(Application.StartupPath +
                                  "\\Profiles\\Battlegrounder\\ProfileType\\CSharpProfile\\Settings\\");
        Initialize(true);
    }

    public void ResetConfiguration()
    {
        Directory.CreateDirectory(Application.StartupPath +
                                  "\\Profiles\\Battlegrounder\\ProfileType\\CSharpProfile\\Settings\\");
        Initialize(true, true);
    }

    public void Dispose()
    {
        Logging.WriteFight("TheNoobBot example Custom Profile stopped.");
        Loop = false;
    }

    public void Initialize(bool configOnly, bool resetSettings = false)
    {
        try
        {
            if (!Loop)
                Loop = true;
            InternalIgnoreFight = false;
            InternalDontStartFights = false;
            Logging.Write("Loading TheNoobBot ProfileType C# Profile system.");
            CurrentBattlegroundId = Battleground.GetCurrentBattleground();
            if (CurrentBattlegroundId != BattlegroundId.None)
                if (CurrentBattlegroundId == BattlegroundId.WarsongGulch ||
                    CurrentBattlegroundId == BattlegroundId.TwinPeaks)
                {
                    if (configOnly)
                    {
                        string currentSettingsFile = Application.StartupPath +
                                                     "\\Profiles\\Battlegrounder\\ProfileType\\CSharpProfile\\Settings\\CaptureTheFlag.xml";
                        CaptureTheFlag.CaptureTheFlagSettings currentSetting = new CaptureTheFlag.CaptureTheFlagSettings();
                        if (File.Exists(currentSettingsFile) && !resetSettings)
                        {
                            currentSetting =
                                Settings.Load<CaptureTheFlag.CaptureTheFlagSettings>(currentSettingsFile);
                        }
                        currentSetting.ToForm();
                        currentSetting.Save(currentSettingsFile);
                    }
                    else
                    {
                        Logging.Write("Loading Capture The Flag module.");
                        new CaptureTheFlag();
                    }
                    Logging.WriteFight("CaptureTheFlag stopped, broken ?");
                }
        }
        catch (Exception exception)
        {
            Logging.WriteError("Initialize(): " + exception);
        }
        Logging.WriteFight("TheNoobBot example Custom Profile stopped. Loop shutdown.");
    }

    #endregion
}

public class CaptureTheFlag
{
    /**
      * Author : VesperCore
    **/
    private static Point _allianceFlagPositionInCTFModule;
    private static Point _allianceFlagPositionTwinPeaks;
    private static Point _allianceFlagPositionWarsongGulch;
    private static Point _hordeFlagPositionInCTFModule;
    private static Point _hordeFlagPositionInTwinPeaks;
    private static Point _hordeFlagPositionInWarsongGulch;

    private readonly int _allianceFlagFloorId;
    private readonly int _allianceFlagId;
    private readonly int _hordeFlagFloorId;
    private readonly int _hordeFlagId;

    public CaptureTheFlag()
    {
        _allianceFlagPositionTwinPeaks = new Point((float) 2117.637, (float) 191.6823, (float) 44.05199);
        _allianceFlagPositionWarsongGulch = new Point((float) 1540.423, (float) 1481.325, (float) 351.8284);
        _allianceFlagId = 179830;
        _allianceFlagFloorId = 179785;
        _hordeFlagPositionInTwinPeaks = new Point((float) 1578.337, (float) 344.0451, (float) 2.418409);
        _hordeFlagPositionInWarsongGulch = new Point((float) 916.5073, (float) 1433.826, (float) 346.3796);
        _hordeFlagId = _allianceFlagId + 1;
        _hordeFlagFloorId = _allianceFlagFloorId + 1;
        while (Main.Loop)
        {
            try
            {
                if (Battleground.GetCurrentBattleground() == BattlegroundId.WarsongGulch)
                {
                    _allianceFlagPositionInCTFModule = _allianceFlagPositionWarsongGulch;
                    _hordeFlagPositionInCTFModule = _hordeFlagPositionInWarsongGulch;
                }
                else
                {
                    _allianceFlagPositionInCTFModule = _allianceFlagPositionTwinPeaks;
                    _hordeFlagPositionInCTFModule = _hordeFlagPositionInTwinPeaks;
                }
                Main.InternalIgnoreFight = false;
                Main.InternalDontStartFights = false;
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
                        Main.InternalDontStartFights = true;
                        Main.InternalIgnoreFight = false;
                        InternalGoTo(ObjectManager.Me.PlayerFaction.ToLower() == "horde"
                                         ? _hordeFlagPositionInCTFModule
                                         : _allianceFlagPositionInCTFModule, ObjectManager.Me.IsHoldingWGFlag,
                                     ObjectManager.IsSomeoneHoldingWGFlag(), ObjectManager.IsSomeoneHoldingWGFlag(false),
                                     ObjectManager.Me.InCombat);
                    }
                    else if (ObjectManager.Me.IsHoldingWGFlag)
                    {
                        // 4 possibilities :
                        if (CaptureTheFlagSettings.CurrentSetting.PlayTheFlag)
                        {
                            // Go to my base and wait until I can capture it.
                            Logging.Write("Go to my base and wait until I can capture it.");
                            Main.InternalDontStartFights = true;
                            Main.InternalIgnoreFight = false;
                            InternalGoTo(ObjectManager.Me.PlayerFaction.ToLower() == "horde"
                                             ? _hordeFlagPositionInCTFModule
                                             : _allianceFlagPositionInCTFModule, ObjectManager.Me.IsHoldingWGFlag,
                                         ObjectManager.IsSomeoneHoldingWGFlag(),
                                         ObjectManager.IsSomeoneHoldingWGFlag(false),
                                         ObjectManager.Me.InCombat);
                        }
                        // Go to my base and wait in a protected area until I can capture it.
                        // Search & Destroy the Hostile Flag holder if not so far from my base.
                        // Search & Destroy the Hostile Flag holder.
                    }
                    else if (!ObjectManager.IsSomeoneHoldingWGFlag(false))
                    {
                        // 2 possibilities :
                        // Go to the ennemy base and take it.
                        Logging.Write("Go to the ennemy base and take it.");
                        Main.InternalDontStartFights = true;
                        Main.InternalIgnoreFight = false;
                        InternalGoTo(ObjectManager.Me.PlayerFaction.ToLower() == "horde"
                                         ? _allianceFlagPositionInCTFModule
                                         : _hordeFlagPositionInCTFModule, ObjectManager.Me.IsHoldingWGFlag,
                                     ObjectManager.IsSomeoneHoldingWGFlag(),
                                     ObjectManager.IsSomeoneHoldingWGFlag(false),
                                     ObjectManager.Me.InCombat, "Take");
                        // Search & Destroy the Hostile Flag holder.
                    }

                    else if (ObjectManager.IsSomeoneHoldingWGFlag())
                    {
                        // 4 possibilities :
                        Logging.Write("Go to the ennemy base and wait until I can take it. #1");
                        Main.InternalIgnoreFight = false;
                        Main.InternalDontStartFights = false;
                        // Go to the ennemy base and wait until I can take it.
                        InternalGoTo(ObjectManager.Me.PlayerFaction.ToLower() == "horde"
                                         ? _allianceFlagPositionInCTFModule
                                         : _hordeFlagPositionInCTFModule, ObjectManager.Me.IsHoldingWGFlag,
                                     ObjectManager.IsSomeoneHoldingWGFlag(),
                                     ObjectManager.IsSomeoneHoldingWGFlag(false),
                                     ObjectManager.Me.InCombat, "Take");
                        // Go to the ennemy base and wait in a protected area until I can take it.
                        // Protect the ally Flag holder.
                        // Search & Destroy the Hostile Flag holder.
                    }
                    else
                    {
                        // 3 possibilities :
                        Logging.Write("Go to the ennemy base and wait until I can take it. #2");
                        Main.InternalIgnoreFight = false;
                        Main.InternalDontStartFights = false;
                        // Go to the ennemy base and wait until I can take it.
                        InternalGoTo(ObjectManager.Me.PlayerFaction.ToLower() == "horde"
                                         ? _allianceFlagPositionInCTFModule
                                         : _hordeFlagPositionInCTFModule, ObjectManager.Me.IsHoldingWGFlag,
                                     ObjectManager.IsSomeoneHoldingWGFlag(),
                                     ObjectManager.IsSomeoneHoldingWGFlag(false),
                                     ObjectManager.Me.InCombat, "Take");
                        // Go to the ennemy base and wait in a protected area until I can take it.
                        // Protect the ally Flag holder.
                    }
                }
                else
                    Thread.Sleep(100);
            }
            catch (Exception e)
            {
                Logging.WriteError("Custom Profile > TheNoobBotCP > CaptureTheFlagWG > Loop: " + e);
            }
            Thread.Sleep(100);
        }
    }

    public void InternalGoTo(Point point, bool isHoldingWGFlag, bool isSomeoneHoldingMyFlag,
                             bool isSomeoneHoldingThemFlag, bool inCombat, string goal = "Capture")
    {
        while (inCombat && !Main.InternalIgnoreFight)
        {
            if (CurrentInformationsHasChanged(Main.InternalIgnoreFight, isHoldingWGFlag, isSomeoneHoldingMyFlag,
                                              isSomeoneHoldingThemFlag, true))
                return;
            Thread.Sleep(10);
        }
        Point initialPos = ObjectManager.Me.Position;
        if (CurrentInformationsHasChanged(Main.InternalIgnoreFight, isHoldingWGFlag, isSomeoneHoldingMyFlag,
                                          isSomeoneHoldingThemFlag, inCombat))
            goto End;
        if (point.DistanceTo(ObjectManager.Me.Position) > 1.5)
        {
            if (!CheckPath(point, isHoldingWGFlag, isSomeoneHoldingMyFlag, isSomeoneHoldingThemFlag, inCombat))
                goto End;
            List<Point> points = PathFinder.FindPathUnstuck(point);
            MovementManager.Go(points);
        }
        while (MovementManager.InMovement ||
               !MovementManager.InMovement && ObjectManager.Me.Position.DistanceTo(point) <= 1.5)
        {
            if (!CheckPath(point, isHoldingWGFlag, isSomeoneHoldingMyFlag, isSomeoneHoldingThemFlag, inCombat))
                goto End;
            switch (goal)
            {
                case "Capture":
                    Main.InternalIgnoreFight = !ObjectManager.IsSomeoneHoldingWGFlag();
                    Main.InternalDontStartFights = ObjectManager.Me.IsHoldingWGFlag;
                    break;
                case "Take":
                    if (ObjectManager.Me.Position.DistanceTo(point) <= 10)
                        InternalGoToGameObject(
                            ObjectManager.Me.PlayerFaction.ToLower() == "horde" ? _allianceFlagId : _hordeFlagId,
                            isHoldingWGFlag, isSomeoneHoldingMyFlag, isSomeoneHoldingThemFlag, inCombat);
                    InternalGoToGameObject(
                        ObjectManager.Me.PlayerFaction.ToLower() == "horde"
                            ? _allianceFlagFloorId
                            : _hordeFlagFloorId,
                        isHoldingWGFlag, isSomeoneHoldingMyFlag, isSomeoneHoldingThemFlag, inCombat);
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
                                       bool isSomeoneHoldingThemFlag, bool inCombat)
    {
        while (inCombat && !Main.InternalIgnoreFight)
        {
            if (CurrentInformationsHasChanged(Main.InternalIgnoreFight, isHoldingWGFlag, isSomeoneHoldingMyFlag,
                                              isSomeoneHoldingThemFlag, true))
                return;
            Thread.Sleep(10);
        }
        Point initialPos = ObjectManager.Me.Position;
        if (CurrentInformationsHasChanged(Main.InternalIgnoreFight, isHoldingWGFlag, isSomeoneHoldingMyFlag,
                                          isSomeoneHoldingThemFlag, inCombat))
            return;
        WoWGameObject obj = ObjectManager.GetNearestWoWGameObject(ObjectManager.GetWoWGameObjectByEntry(entry));
        if (obj.GetBaseAddress <= 0) return;
        if (ObjectManager.Me.Position.DistanceTo(obj.Position) > 100)
        {
            //Logging.Write("Found object " + obj.Name + ", but it's out of our radius. Distance: " + ObjectManager.Me.Position.DistanceTo(obj.Position) + " yards.");
            return;
        }
        if (ObjectManager.Me.Position.DistanceTo(obj.Position) > 1.5)
        {
            if (!CheckPath(obj.Position, isHoldingWGFlag, isSomeoneHoldingMyFlag, isSomeoneHoldingThemFlag, inCombat))
                return;
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
            if (!CheckPath(obj.Position, isHoldingWGFlag, isSomeoneHoldingMyFlag, isSomeoneHoldingThemFlag, inCombat))
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
                    Interact.InteractWith(obj.GetBaseAddress);
                    Thread.Sleep(Usefuls.Latency + 100);
                    if (CurrentInformationsHasChanged(Main.InternalIgnoreFight, isHoldingWGFlag,
                                                      isSomeoneHoldingMyFlag,
                                                      isSomeoneHoldingThemFlag, inCombat))
                        return;
                    Interact.InteractWith(obj.GetBaseAddress);
                    Thread.Sleep(Usefuls.Latency + 100);
                }
            }
            Thread.Sleep(100);
        }
        End:
        if (!obj.IsValid) return;
        Logging.Write("InternalGoToGameObject ended after traveling " +
                      initialPos.DistanceTo(ObjectManager.Me.Position) +
                      " yards");
        Logging.Write("Initial Position: " + initialPos);
        Logging.Write("Current Position: " + ObjectManager.Me.Position);
        Logging.Write("Searching for object entry: " + entry);
        Logging.Write("Object " + obj.Name + " found.");
        Logging.Write("Position: " + obj.Position);
        Logging.Write("Last distance to target: " + ObjectManager.Me.Position.DistanceTo(obj.Position));
    }

    private static bool CurrentInformationsHasChanged(bool ignoreFight, bool isHoldingWGFlag,
                                                      bool isSomeoneHoldingMyFlag, bool isSomeoneHoldingThemFlag,
                                                      bool inCombat)
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
            Main.InternalIgnoreFight = ObjectManager.Me.Position.DistanceTo(
                ObjectManager.Me.PlayerFaction.ToLower() == "horde"
                    ? _hordeFlagPositionInCTFModule
                    : _allianceFlagPositionInCTFModule) <= 20;
        }
        else if (isHoldingWGFlag)
        {
            if (!ObjectManager.Me.IsHoldingWGFlag || !ObjectManager.IsSomeoneHoldingWGFlag())
            {
                Logging.Write("Current Informations Has Changed. #3");
                return true;
            }
            Main.InternalDontStartFights = ObjectManager.Me.Position.DistanceTo(
                ObjectManager.Me.PlayerFaction.ToLower() == "horde"
                    ? _hordeFlagPositionInCTFModule
                    : _allianceFlagPositionInCTFModule) > 20;
        }
        else if (!isSomeoneHoldingThemFlag)
        {
            if (ObjectManager.Me.IsHoldingWGFlag || ObjectManager.IsSomeoneHoldingWGFlag(false))
            {
                Logging.Write("Current Informations Has Changed. #4");
                return true;
            }
            Main.InternalIgnoreFight = ObjectManager.IsSomeoneHoldingWGFlag() &&
                                       ObjectManager.Me.Position.DistanceTo(ObjectManager.Me.PlayerFaction.ToLower() ==
                                                                            "horde"
                                                                                ? _allianceFlagPositionInCTFModule
                                                                                : _hordeFlagPositionInCTFModule) <= 20;
        }
        else if (isSomeoneHoldingMyFlag)
        {
            if (ObjectManager.Me.IsHoldingWGFlag || !ObjectManager.IsSomeoneHoldingWGFlag() ||
                !ObjectManager.IsSomeoneHoldingWGFlag(false))
            {
                Logging.Write("Current Informations Has Changed. #5");
                return true;
            }
        }
        else
        {
            if (ObjectManager.Me.IsHoldingWGFlag || ObjectManager.IsSomeoneHoldingWGFlag() ||
                !ObjectManager.IsSomeoneHoldingWGFlag(false))
            {
                Logging.Write("Current Informations Has Changed. #6");
                return true;
            }
        }

        if (inCombat && !ignoreFight && !ObjectManager.Me.InCombat)
        {
            Logging.Write("Current Informations Has Changed. #7");
            return true;
        }
        if (!inCombat && ObjectManager.Me.InCombat && !ignoreFight)
        {
            Logging.Write("Current Informations Has Changed. #8");
            return true;
        }
        return false;
    }

    private static bool CheckPath(Point destination, bool isHoldingWGFlag, bool isSomeoneHoldingMyFlag,
                                  bool isSomeoneHoldingThemFlag, bool inCombat)
    {
        if (CurrentInformationsHasChanged(Main.InternalIgnoreFight, isHoldingWGFlag, isSomeoneHoldingMyFlag,
                                          isSomeoneHoldingThemFlag, inCombat))
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
        else if ((!inCombat || Main.InternalIgnoreFight) &&
                 MovementManager.CurrentPath[MovementManager.CurrentPath.Count - 1].DistanceTo(destination) > 1.5)
        {
            MovementManager.StopMove();
            Logging.Write(
                "InternalGoTo : MovementManager.CurrentPath[MovementManager.CurrentPath.Count - 1].DistanceTo(point) = " +
                MovementManager.CurrentPath[MovementManager.CurrentPath.Count - 1].DistanceTo(destination) +
                "; InMovement = " + MovementManager.InMovement);
        }
        return true;
    }

    [Serializable]
    public class CaptureTheFlagSettings : Settings
    {
        public readonly CaptureTheFlagSettings MySettings = GetSettings();
        public bool PlayTheFlag = true;

        public CaptureTheFlagSettings()
        {
            ConfigWinForm(new System.Drawing.Point(500, 400), "Capture The Flag module Settings");
            AddControlInWinForm("Play The Flag", "PlayTheFlag", "Main settings", "bool");
        }

        public static CaptureTheFlagSettings CurrentSetting { get; set; }

        public static CaptureTheFlagSettings GetSettings()
        {
            string CurrentSettingsFile = Application.StartupPath +
                                         "\\CombatClasses\\Settings\\Deathknight_Apprentice.xml";
            if (File.Exists(CurrentSettingsFile))
                return
                    CurrentSetting =
                    Load<CaptureTheFlagSettings>(CurrentSettingsFile);
            return new CaptureTheFlagSettings();
        }
    }
}