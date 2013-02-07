using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Battlegrounder.Profile;
using Battlegrounder.Profiletype;
using nManager;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Products;
using nManager.Wow.Class;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;
using Battleground = nManager.Wow.Helpers.Battleground;
using Timer = nManager.Helpful.Timer;

namespace Battlegrounder.Bot
{
    internal class BattlegrounderCurrentProfile : State
    {
        private static BattlegrounderProfile _currentProfile = new BattlegrounderProfile();
        private static int _zoneIdProfile;
        private static string _currentBattlegroundId;
        private static string _currentProfileName;
        private bool _xmlProfile;
        private bool _afkSomewhere;
        private List<Point> _afkSomewherePosition;
        private Timer _afkSomewhereTimer = new Timer(-1);

        public override string DisplayName
        {
            get { return "BattlegrounderState"; }
        }

        public override int Priority { get; set; }

        public override bool NeedToRun
        {
            get
            {
                if (!Usefuls.InGame ||
                    Usefuls.IsLoadingOrConnecting ||
                    ObjectManager.Me.IsDeadMe ||
                    !ObjectManager.Me.IsValid ||
                    !Products.IsStarted)
                    return false;

                if (Battleground.IsInBattleground() && !Battleground.IsFinishBattleground())
                {
                    if (_afkSomewhere && _afkSomewhereTimer.IsReady)
                    {
                        _currentBattlegroundId = null;
                        _currentProfileName = "";
                        _afkSomewhereTimer.Reset();
                    }
                    if (_currentBattlegroundId != null &&
                        _currentBattlegroundId != Battleground.GetCurrentBattleground().ToString())
                    {
                        _currentBattlegroundId = null;
                        _currentProfileName = "";
                    }
                    if (_currentBattlegroundId == null && _currentProfile.BattlegrounderZones.Count <= 0)
                    {
                        _xmlProfile = false;
                        _afkSomewhere = false;
                        _currentBattlegroundId = Battleground.GetCurrentBattleground().ToString();
                        if (_currentBattlegroundId == BattlegroundId.AlteracValley.ToString())
                        {
                            if (
                                String.IsNullOrWhiteSpace(
                                    BattlegrounderSetting.CurrentSetting.AlteracValleyXmlProfile))
                                return false;
                            //if (BattlegrounderSetting.CurrentSetting.AlteracValleyProfileType == "XMLProfile")
                            //{
                            _xmlProfile = true;
                            _currentProfileName =
                                BattlegrounderSetting.CurrentSetting.AlteracValleyXmlProfile;
                            //}
                        }
                        else if (_currentBattlegroundId == BattlegroundId.WarsongGulch.ToString())
                        {
                            switch (BattlegrounderSetting.CurrentSetting.WarsongGulchProfileType)
                            {
                                case "XMLProfile":
                                    if (
                                        String.IsNullOrWhiteSpace(
                                            BattlegrounderSetting.CurrentSetting.WarsongGulchXmlProfile))
                                        return false;
                                    _xmlProfile = true;
                                    _currentProfileName = BattlegrounderSetting.CurrentSetting.WarsongGulchXmlProfile;
                                    break;
                                case "AfkSomewhere":
                                    foreach (
                                        var profiletype in
                                            BattlegrounderProfileType.Battlegrounds.Where(
                                                battleground => battleground.BattlegroundId == _currentBattlegroundId)
                                                                     .SelectMany(
                                                                         battleground =>
                                                                         battleground.ProfileTypes.Where(
                                                                             profiletype =>
                                                                             profiletype.ProfileTypeId ==
                                                                             BattlegrounderSetting.CurrentSetting
                                                                                                  .WarsongGulchProfileType))
                                        )
                                    {
                                        _xmlProfile = false;
                                        _afkSomewhereTimer = new Timer(1000*(profiletype.Timer1 + profiletype.Timer2));
                                        _afkSomewherePosition = AfkSomewhere.AfkSomewhereNow(BattlegroundId.WarsongGulch);
                                        _afkSomewhere = _afkSomewherePosition != null;
                                    }
                                    break;
                            }
                        }
                        else if (_currentBattlegroundId == BattlegroundId.ArathiBasin.ToString())
                        {
                            if (
                                String.IsNullOrWhiteSpace(
                                    BattlegrounderSetting.CurrentSetting.ArathiBasinXmlProfile))
                                return false;
                            _xmlProfile = true;
                            _currentProfileName = BattlegrounderSetting.CurrentSetting.ArathiBasinXmlProfile;
                        }
                        else if (_currentBattlegroundId == BattlegroundId.EyeoftheStorm.ToString())
                        {
                            if (
                                String.IsNullOrWhiteSpace(
                                    BattlegrounderSetting.CurrentSetting.EyeoftheStormXmlProfile))
                                return false;
                            _xmlProfile = true;
                            _currentProfileName = BattlegrounderSetting.CurrentSetting.EyeoftheStormXmlProfile;
                        }
                        else if (_currentBattlegroundId == BattlegroundId.StrandoftheAncients.ToString())
                        {
                            if (
                                String.IsNullOrWhiteSpace(
                                    BattlegrounderSetting.CurrentSetting.StrandoftheAncientsXmlProfile))
                                return false;
                            _xmlProfile = true;
                            _currentProfileName =
                                BattlegrounderSetting.CurrentSetting.StrandoftheAncientsXmlProfile;
                        }
                        else if (_currentBattlegroundId == BattlegroundId.IsleofConquest.ToString())
                        {
                            if (
                                String.IsNullOrWhiteSpace(
                                    BattlegrounderSetting.CurrentSetting.IsleofConquestXmlProfile))
                                return false;
                            _xmlProfile = true;
                            _currentProfileName = BattlegrounderSetting.CurrentSetting.IsleofConquestXmlProfile;
                        }
                        else if (_currentBattlegroundId == BattlegroundId.BattleforGilneas.ToString())
                        {
                            if (
                                String.IsNullOrWhiteSpace(
                                    BattlegrounderSetting.CurrentSetting.BattleforGilneasXmlProfile))
                                return false;
                            _xmlProfile = true;
                            _currentProfileName = BattlegrounderSetting.CurrentSetting.BattleforGilneasXmlProfile;
                        }
                        else if (_currentBattlegroundId == BattlegroundId.TwinPeaks.ToString())
                        {
                            if (
                                String.IsNullOrWhiteSpace(
                                    BattlegrounderSetting.CurrentSetting.TwinPeaksXmlProfile))
                                return false;
                            _xmlProfile = true;
                            _currentProfileName = BattlegrounderSetting.CurrentSetting.TwinPeaksXmlProfile;
                        }
                        else if (_currentBattlegroundId == BattlegroundId.TempleofKotmogu.ToString())
                        {
                            if (
                                String.IsNullOrWhiteSpace(
                                    BattlegrounderSetting.CurrentSetting.TempleofKotmoguXmlProfile))
                                return false;
                            _xmlProfile = true;
                            _currentProfileName = BattlegrounderSetting.CurrentSetting.TempleofKotmoguXmlProfile;
                        }
                        else if (_currentBattlegroundId == BattlegroundId.SilvershardMines.ToString())
                        {
                            if (
                                String.IsNullOrWhiteSpace(
                                    BattlegrounderSetting.CurrentSetting.SilvershardMinesXmlProfile))
                                return false;
                            _xmlProfile = true;
                            _currentProfileName = BattlegrounderSetting.CurrentSetting.SilvershardMinesXmlProfile;
                        }
                        if (_xmlProfile)
                        {
                            _currentProfile = new BattlegrounderProfile();
                            if (File.Exists(Application.StartupPath + "\\Profiles\\Battlegrounder\\" +
                                            _currentProfileName))
                            {
                                _currentProfile =
                                    XmlSerializer.Deserialize<BattlegrounderProfile>(Application.StartupPath +
                                                                                     "\\Profiles\\Battlegrounder\\" +
                                                                                     _currentProfileName);
                                if (_currentProfile.BattlegrounderZones.Count > 0)
                                {
                                    _xmlProfile = false;
                                    MovementManager.StopMove();
                                    return true;
                                }
                            }
                            _xmlProfile = false;
                        }
                        else if (_afkSomewhere)
                        {
                            MovementManager.StopMove();
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        public override List<State> NextStates
        {
            get { return new List<State>(); }
        }

        public override List<State> BeforeStates
        {
            get { return new List<State>(); }
        }

        public override void Run()
        {
            if (_xmlProfile)
            {
                SelectZone();

                // Black List:
                var blackListDic =
                    _currentProfile.BattlegrounderZones.SelectMany(zone => zone.BlackListRadius)
                                   .ToDictionary(b => b.Position, b => b.Radius);
                nManagerSetting.AddRangeBlackListZone(blackListDic);
            }
            else if (_afkSomewhere)
            {
                Logging.Write("ProfileType AFK Somewhere detected. Going to a new zone.");
                Bot._battlegrounding.BattlegroundId = _currentBattlegroundId;
                Bot._movementLoop.PathLoop = _afkSomewherePosition;
            }
        }

        private static void SelectZone()
        {
            _zoneIdProfile = -1;
            for (int i = 0; i <= _currentProfile.BattlegrounderZones.Count - 1; i++)
            {
                if (_currentProfile.BattlegrounderZones[i].BattlegroundId ==
                    Battleground.GetCurrentBattleground().ToString())
                {
                    _zoneIdProfile = i;
                    break;
                }
            }
            if (_zoneIdProfile == -1 || !_currentProfile.BattlegrounderZones[_zoneIdProfile].IsValid())
            {
                return;
            }

            Logging.Write("ProfileType XMLProfile detected. Starting profile : " + _currentProfileName);

            if (_currentProfile.BattlegrounderZones[_zoneIdProfile].Hotspots)
            {
                var pointsTemps = new List<Point>();
                for (int i = 0; i <= _currentProfile.BattlegrounderZones[_zoneIdProfile].Points.Count - 1; i++)
                {
                    if (i + 1 > _currentProfile.BattlegrounderZones[_zoneIdProfile].Points.Count - 1)
                        pointsTemps.AddRange(
                            PathFinder.FindPath(_currentProfile.BattlegrounderZones[_zoneIdProfile].Points[i],
                                                _currentProfile.BattlegrounderZones[_zoneIdProfile].Points[0]));
                    else
                        pointsTemps.AddRange(
                            PathFinder.FindPath(_currentProfile.BattlegrounderZones[_zoneIdProfile].Points[i],
                                                _currentProfile.BattlegrounderZones[_zoneIdProfile].Points[i + 1
                                                    ]));
                }
                _currentProfile.BattlegrounderZones[_zoneIdProfile].Hotspots = false;
                _currentProfile.BattlegrounderZones[_zoneIdProfile].Points.Clear();
                _currentProfile.BattlegrounderZones[_zoneIdProfile].Points.AddRange(pointsTemps);
            }

            Bot._battlegrounding.BattlegroundId = _currentProfile.BattlegrounderZones[_zoneIdProfile].BattlegroundId;

            Bot._movementLoop.PathLoop = _currentProfile.BattlegrounderZones[_zoneIdProfile].Points;
        }
    }
}