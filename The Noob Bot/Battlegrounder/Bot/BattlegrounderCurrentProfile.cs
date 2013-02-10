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

        private static readonly BattlegrounderProfileType ProfileTypeFile =
            XmlSerializer.Deserialize<BattlegrounderProfileType>(Application.StartupPath +
                                                                 "\\Profiles\\Battlegrounder\\ProfileType\\ProfileType.xml");

        public static bool ForceChecking;
        public static bool StopChecking;
        private bool _afkSomewhere;
        private List<Point> _afkSomewhereNextPosition;
        private Timer _afkSomewhereTimer = new Timer(-1);
        private bool _csharpProfile;
        private bool _xmlProfile;

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
                    if ((_afkSomewhere && _afkSomewhereTimer.IsReady) ||
                        (_currentBattlegroundId != null &&
                         _currentBattlegroundId != Battleground.GetCurrentBattleground().ToString())
                        || ForceChecking)
                    {
                        _currentBattlegroundId = null;
                        _currentProfileName = "";
                        ForceChecking = false;
                        if (_xmlProfile)
                        {
                            _currentProfile.BattlegrounderZones.Clear();
                            _xmlProfile = false;
                        }
                        if (_afkSomewhere)
                        {
                            _afkSomewhere = false;
                            _afkSomewhereTimer.Reset();
                        }
                        if (_csharpProfile)
                        {
                            if (CustomProfile.IsAliveCustomProfile)
                            {
                                CustomProfile.GetSetIgnoreFight = false;
                                CustomProfile.DisposeCustomProfile();
                            }
                            Bot.MovementLoop.IsProfileCSharp = false;
                            _csharpProfile = false;
                            StopChecking = false;
                        }
                    }
                    if (!_afkSomewhere && !_csharpProfile && !_xmlProfile)
                    {
                        _currentBattlegroundId = null;
                        if (_currentProfile.BattlegrounderZones != null)
                            _currentProfile.BattlegrounderZones.Clear();
                        StopChecking = false;
                    }
                    if (_currentBattlegroundId == null &&
                        (_currentProfile.BattlegrounderZones == null || _currentProfile.BattlegrounderZones.Count <= 0) &&
                        !StopChecking)
                    {
                        _xmlProfile = false;
                        _afkSomewhere = false;
                        _currentBattlegroundId = Battleground.GetCurrentBattleground().ToString();
                        foreach (
                            Profiletype.Battleground battleground in
                                ProfileTypeFile.Battlegrounds.Where(
                                    battleground =>
                                    battleground.BattlegroundId == Battleground.GetCurrentBattleground().ToString()))
                        {
                            if (battleground.BattlegroundId == BattlegroundId.AlteracValley.ToString())
                            {
                                foreach (
                                    ProfileType profileType in
                                        battleground.ProfileTypes.Where(
                                            profileType =>
                                            profileType.ProfileTypeId ==
                                            BattlegrounderSetting.CurrentSetting.AlteracValleyProfileType))
                                {
                                    switch (profileType.ProfileTypeId)
                                    {
                                        case "XMLProfile":
                                            if (BattlegrounderSetting.CurrentSetting.AlteracValleyXmlProfile == "")
                                                return false;
                                            _xmlProfile = true;
                                            _currentProfileName =
                                                BattlegrounderSetting.CurrentSetting.AlteracValleyXmlProfile;
                                            break;
                                        case "AfkSomewhere":
                                            _afkSomewhereTimer =
                                                new Timer(1000*(profileType.Timer1 + profileType.Timer2));
                                            _afkSomewhereNextPosition =
                                                AfkSomewhere.AfkSomewhereNow(battleground.BattlegroundId);
                                            _afkSomewhere = _afkSomewhereNextPosition != null;
                                            break;
                                        case "CSharpProfile":
                                            _csharpProfile =
                                                CSharpProfile.CSharpProfileNow(profileType.ProfileTypeScriptName);
                                            if (_csharpProfile)
                                                Logging.Write("ProfileType C# Profile detected. Starting script.");
                                            StopChecking = _csharpProfile;
                                            Bot.MovementLoop.IsProfileCSharp = StopChecking;
                                            return false; // Once the script is started, let him do the rest.
                                    }
                                }
                            }
                            else if (battleground.BattlegroundId == BattlegroundId.WarsongGulch.ToString())
                            {
                                foreach (
                                    ProfileType profileType in
                                        battleground.ProfileTypes.Where(
                                            profileType =>
                                            profileType.ProfileTypeId ==
                                            BattlegrounderSetting.CurrentSetting.WarsongGulchProfileType))
                                {
                                    switch (profileType.ProfileTypeId)
                                    {
                                        case "XMLProfile":
                                            if (BattlegrounderSetting.CurrentSetting.WarsongGulchXmlProfile == "")
                                                return false;
                                            _xmlProfile = true;
                                            _currentProfileName =
                                                BattlegrounderSetting.CurrentSetting.WarsongGulchXmlProfile;
                                            break;
                                        case "AfkSomewhere":
                                            _afkSomewhereTimer =
                                                new Timer(1000*(profileType.Timer1 + profileType.Timer2));
                                            _afkSomewhereNextPosition =
                                                AfkSomewhere.AfkSomewhereNow(battleground.BattlegroundId);
                                            _afkSomewhere = _afkSomewhereNextPosition != null;
                                            break;
                                        case "CSharpProfile":
                                            _csharpProfile =
                                                CSharpProfile.CSharpProfileNow(profileType.ProfileTypeScriptName);
                                            if (_csharpProfile)
                                                Logging.Write("ProfileType C# Profile detected. Starting script.");
                                            StopChecking = _csharpProfile;
                                            return false; // Once the script is started, let him do the rest.
                                    }
                                }
                            }
                            else if (battleground.BattlegroundId == BattlegroundId.ArathiBasin.ToString())
                            {
                                foreach (
                                    ProfileType profileType in
                                        battleground.ProfileTypes.Where(
                                            profileType =>
                                            profileType.ProfileTypeId ==
                                            BattlegrounderSetting.CurrentSetting.ArathiBasinProfileType))
                                {
                                    switch (profileType.ProfileTypeId)
                                    {
                                        case "XMLProfile":
                                            if (BattlegrounderSetting.CurrentSetting.ArathiBasinXmlProfile == "")
                                                return false;
                                            _xmlProfile = true;
                                            _currentProfileName =
                                                BattlegrounderSetting.CurrentSetting.ArathiBasinXmlProfile;
                                            break;
                                        case "AfkSomewhere":
                                            _afkSomewhereTimer =
                                                new Timer(1000*(profileType.Timer1 + profileType.Timer2));
                                            _afkSomewhereNextPosition =
                                                AfkSomewhere.AfkSomewhereNow(battleground.BattlegroundId);
                                            _afkSomewhere = _afkSomewhereNextPosition != null;
                                            break;
                                    }
                                }
                            }
                            else if (battleground.BattlegroundId == BattlegroundId.EyeoftheStorm.ToString())
                            {
                                foreach (
                                    ProfileType profileType in
                                        battleground.ProfileTypes.Where(
                                            profileType =>
                                            profileType.ProfileTypeId ==
                                            BattlegrounderSetting.CurrentSetting.EyeoftheStormProfileType))
                                {
                                    switch (profileType.ProfileTypeId)
                                    {
                                        case "XMLProfile":
                                            if (BattlegrounderSetting.CurrentSetting.EyeoftheStormXmlProfile == "")
                                                return false;
                                            _xmlProfile = true;
                                            _currentProfileName =
                                                BattlegrounderSetting.CurrentSetting.EyeoftheStormXmlProfile;
                                            break;
                                        case "AfkSomewhere":
                                            _afkSomewhereTimer =
                                                new Timer(1000*(profileType.Timer1 + profileType.Timer2));
                                            _afkSomewhereNextPosition =
                                                AfkSomewhere.AfkSomewhereNow(battleground.BattlegroundId);
                                            _afkSomewhere = _afkSomewhereNextPosition != null;
                                            break;
                                    }
                                }
                            }
                            else if (battleground.BattlegroundId == BattlegroundId.StrandoftheAncients.ToString())
                            {
                                foreach (
                                    ProfileType profileType in
                                        battleground.ProfileTypes.Where(
                                            profileType =>
                                            profileType.ProfileTypeId ==
                                            BattlegrounderSetting.CurrentSetting.StrandoftheAncientsProfileType))
                                {
                                    switch (profileType.ProfileTypeId)
                                    {
                                        case "XMLProfile":
                                            if (
                                                BattlegrounderSetting.CurrentSetting.StrandoftheAncientsXmlProfile ==
                                                "")
                                                return false;
                                            _xmlProfile = true;
                                            _currentProfileName =
                                                BattlegrounderSetting.CurrentSetting.StrandoftheAncientsXmlProfile;
                                            break;
                                        case "AfkSomewhere":
                                            _afkSomewhereTimer =
                                                new Timer(1000*(profileType.Timer1 + profileType.Timer2));
                                            _afkSomewhereNextPosition =
                                                AfkSomewhere.AfkSomewhereNow(battleground.BattlegroundId);
                                            _afkSomewhere = _afkSomewhereNextPosition != null;
                                            break;
                                    }
                                }
                            }
                            else if (battleground.BattlegroundId == BattlegroundId.IsleofConquest.ToString())
                            {
                                foreach (
                                    ProfileType profileType in
                                        battleground.ProfileTypes.Where(
                                            profileType =>
                                            profileType.ProfileTypeId ==
                                            BattlegrounderSetting.CurrentSetting.IsleofConquestProfileType))
                                {
                                    switch (profileType.ProfileTypeId)
                                    {
                                        case "XMLProfile":
                                            if (BattlegrounderSetting.CurrentSetting.IsleofConquestXmlProfile == "")
                                                return false;
                                            _xmlProfile = true;
                                            _currentProfileName =
                                                BattlegrounderSetting.CurrentSetting.IsleofConquestXmlProfile;
                                            break;
                                        case "AfkSomewhere":
                                            _afkSomewhereTimer =
                                                new Timer(1000*(profileType.Timer1 + profileType.Timer2));
                                            _afkSomewhereNextPosition =
                                                AfkSomewhere.AfkSomewhereNow(battleground.BattlegroundId);
                                            _afkSomewhere = _afkSomewhereNextPosition != null;
                                            break;
                                    }
                                }
                            }
                            else if (battleground.BattlegroundId == BattlegroundId.BattleforGilneas.ToString())
                            {
                                foreach (
                                    ProfileType profileType in
                                        battleground.ProfileTypes.Where(
                                            profileType =>
                                            profileType.ProfileTypeId ==
                                            BattlegrounderSetting.CurrentSetting.BattleforGilneasProfileType))
                                {
                                    switch (profileType.ProfileTypeId)
                                    {
                                        case "XMLProfile":
                                            if (BattlegrounderSetting.CurrentSetting.BattleforGilneasXmlProfile ==
                                                "")
                                                return false;
                                            _xmlProfile = true;
                                            _currentProfileName =
                                                BattlegrounderSetting.CurrentSetting.BattleforGilneasXmlProfile;
                                            break;
                                        case "AfkSomewhere":
                                            _afkSomewhereTimer =
                                                new Timer(1000*(profileType.Timer1 + profileType.Timer2));
                                            _afkSomewhereNextPosition =
                                                AfkSomewhere.AfkSomewhereNow(battleground.BattlegroundId);
                                            _afkSomewhere = _afkSomewhereNextPosition != null;
                                            break;
                                    }
                                }
                            }
                            else if (battleground.BattlegroundId == BattlegroundId.TwinPeaks.ToString())
                            {
                                foreach (
                                    ProfileType profileType in
                                        battleground.ProfileTypes.Where(
                                            profileType =>
                                            profileType.ProfileTypeId ==
                                            BattlegrounderSetting.CurrentSetting.TwinPeaksProfileType))
                                {
                                    switch (profileType.ProfileTypeId)
                                    {
                                        case "XMLProfile":
                                            if (BattlegrounderSetting.CurrentSetting.TwinPeaksXmlProfile == "")
                                                return false;
                                            _xmlProfile = true;
                                            _currentProfileName =
                                                BattlegrounderSetting.CurrentSetting.TwinPeaksXmlProfile;
                                            break;
                                        case "AfkSomewhere":
                                            _afkSomewhereTimer =
                                                new Timer(1000*(profileType.Timer1 + profileType.Timer2));
                                            _afkSomewhereNextPosition =
                                                AfkSomewhere.AfkSomewhereNow(battleground.BattlegroundId);
                                            _afkSomewhere = _afkSomewhereNextPosition != null;
                                            break;
                                    }
                                }
                            }
                            else if (battleground.BattlegroundId == BattlegroundId.TempleofKotmogu.ToString())
                            {
                                foreach (
                                    ProfileType profileType in
                                        battleground.ProfileTypes.Where(
                                            profileType =>
                                            profileType.ProfileTypeId ==
                                            BattlegrounderSetting.CurrentSetting.TempleofKotmoguProfileType))
                                {
                                    switch (profileType.ProfileTypeId)
                                    {
                                        case "XMLProfile":
                                            if (BattlegrounderSetting.CurrentSetting.TempleofKotmoguXmlProfile == "")
                                                return false;
                                            _xmlProfile = true;
                                            _currentProfileName =
                                                BattlegrounderSetting.CurrentSetting.TempleofKotmoguXmlProfile;
                                            break;
                                        case "AfkSomewhere":
                                            _afkSomewhereTimer =
                                                new Timer(1000*(profileType.Timer1 + profileType.Timer2));
                                            _afkSomewhereNextPosition =
                                                AfkSomewhere.AfkSomewhereNow(battleground.BattlegroundId);
                                            _afkSomewhere = _afkSomewhereNextPosition != null;
                                            break;
                                    }
                                }
                            }
                            else if (battleground.BattlegroundId == BattlegroundId.SilvershardMines.ToString())
                            {
                                foreach (
                                    ProfileType profileType in
                                        battleground.ProfileTypes.Where(
                                            profileType =>
                                            profileType.ProfileTypeId ==
                                            BattlegrounderSetting.CurrentSetting.SilvershardMinesProfileType))
                                {
                                    switch (profileType.ProfileTypeId)
                                    {
                                        case "XMLProfile":
                                            if (BattlegrounderSetting.CurrentSetting.SilvershardMinesXmlProfile ==
                                                "")
                                                return false;
                                            _xmlProfile = true;
                                            _currentProfileName =
                                                BattlegrounderSetting.CurrentSetting.SilvershardMinesXmlProfile;
                                            break;
                                        case "AfkSomewhere":
                                            _afkSomewhereTimer =
                                                new Timer(1000*(profileType.Timer1 + profileType.Timer2));
                                            _afkSomewhereNextPosition =
                                                AfkSomewhere.AfkSomewhereNow(battleground.BattlegroundId);
                                            _afkSomewhere = _afkSomewhereNextPosition != null;
                                            break;
                                    }
                                }
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
                                    if (_currentProfile.IsValid())
                                    {
                                        MovementManager.StopMove();
                                        return true;
                                    }
                                }
                                _xmlProfile = false;
                            }
                            else if (_afkSomewhere)
                                return true;
                            else
                                return false;
                        }
                    }
                }
                return false;
            }
        }

        public override List<
            State>
            NextStates
        {
            get { return new List<State>(); }
        }

        public override List<
            State>
            BeforeStates
        {
            get { return new List<State>(); }
        }

        public override void Run()
        {
            if (_xmlProfile)
            {
                if (Bot.MovementLoop.PathLoop != null && Bot.MovementLoop.PathLoop.Count > 0)
                    Bot.MovementLoop.PathLoop.Clear();
                SelectZone();
                Dictionary<Point, float> blackListDic =
                    _currentProfile.BattlegrounderZones.SelectMany(zone => zone.BlackListRadius)
                                   .ToDictionary(b => b.Position, b => b.Radius);
                nManagerSetting.AddRangeBlackListZone(blackListDic);
            }
            else if (_afkSomewhere)
            {
                if (Bot.MovementLoop.PathLoop != null && Bot.MovementLoop.PathLoop.Count > 0)
                    Bot.MovementLoop.PathLoop.Clear();
                Logging.Write("ProfileType AFK Somewhere detected. Going to a new zone to start AFK.");
                Bot.Battlegrounding.BattlegroundId = _currentBattlegroundId;
                Bot.MovementLoop.PathLoop = _afkSomewhereNextPosition;
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

            Bot.Battlegrounding.BattlegroundId = _currentProfile.BattlegrounderZones[_zoneIdProfile].BattlegroundId;

            Bot.MovementLoop.PathLoop = _currentProfile.BattlegrounderZones[_zoneIdProfile].Points;
        }
    }
}