using System;
using System.Collections.Generic;
using System.IO;
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

namespace Battlegrounder.Bot
{
    internal class BattlegrounderCurrentProfile : State
    {
        internal static BattlegrounderProfile CurrentProfile = new BattlegrounderProfile();
        public static int ZoneIdProfile;
        public static string CurrentBattlegroundName;
        public string CurrentProfileName;
        public bool XMLProfile;

        public override string DisplayName
        {
            get { return "BattlegrounderState"; }
        }

        public override int Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

        private int _priority;

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

                if (Battleground.IsInBattleground() && !Battleground.IsFinishBattleground() &&
                    Battleground.GetCurrentBattleground() != BattlegroundId.None)
                {
                    if (CurrentBattlegroundName == null && CurrentProfile.BattlegrounderZones.Count <= 0)
                    {
                        CurrentBattlegroundName = Battleground.GetCurrentBattleground().ToString();
                        if (CurrentBattlegroundName == BattlegroundId.AlteracValley.ToString())
                        {
                            if (
                                String.IsNullOrWhiteSpace(
                                    BattlegrounderSetting.CurrentSetting.AlteracValleyXmlProfile))
                                return false;
                            //if (BattlegrounderSetting.CurrentSetting.AlteracValleyProfileType == "XMLProfile")
                            //{
                            XMLProfile = true;
                            CurrentProfileName =
                                BattlegrounderSetting.CurrentSetting.AlteracValleyXmlProfile;
                            //}
                        }
                        else if (CurrentBattlegroundName == BattlegroundId.WarsongGulch.ToString())
                        {
                            switch (BattlegrounderSetting.CurrentSetting.WarsongGulchProfileType)
                            {
                                case "XMLProfile":
                                    if (
                                        String.IsNullOrWhiteSpace(
                                            BattlegrounderSetting.CurrentSetting.WarsongGulchXmlProfile))
                                        return false;
                                    XMLProfile = true;
                                    CurrentProfileName = BattlegrounderSetting.CurrentSetting.WarsongGulchXmlProfile;
                                    break;
                                case "AfkSomewhere":
                                    AfkSomewhere.AfkSomewhereNow(BattlegroundId.WarsongGulch);
                                    break;
                            }
                        }
                        else if (CurrentBattlegroundName == BattlegroundId.ArathiBasin.ToString())
                        {
                            if (
                                String.IsNullOrWhiteSpace(
                                    BattlegrounderSetting.CurrentSetting.ArathiBasinXmlProfile))
                                return false;
                            XMLProfile = true;
                            CurrentProfileName = BattlegrounderSetting.CurrentSetting.ArathiBasinXmlProfile;
                        }
                        else if (CurrentBattlegroundName == BattlegroundId.EyeoftheStorm.ToString())
                        {
                            if (
                                String.IsNullOrWhiteSpace(
                                    BattlegrounderSetting.CurrentSetting.EyeoftheStormXmlProfile))
                                return false;
                            XMLProfile = true;
                            CurrentProfileName = BattlegrounderSetting.CurrentSetting.EyeoftheStormXmlProfile;
                        }
                        else if (CurrentBattlegroundName == BattlegroundId.StrandoftheAncients.ToString())
                        {
                            if (
                                String.IsNullOrWhiteSpace(
                                    BattlegrounderSetting.CurrentSetting.StrandoftheAncientsXmlProfile))
                                return false;
                            XMLProfile = true;
                            CurrentProfileName =
                                BattlegrounderSetting.CurrentSetting.StrandoftheAncientsXmlProfile;
                        }
                        else if (CurrentBattlegroundName == BattlegroundId.IsleofConquest.ToString())
                        {
                            if (
                                String.IsNullOrWhiteSpace(
                                    BattlegrounderSetting.CurrentSetting.IsleofConquestXmlProfile))
                                return false;
                            XMLProfile = true;
                            CurrentProfileName = BattlegrounderSetting.CurrentSetting.IsleofConquestXmlProfile;
                        }
                        else if (CurrentBattlegroundName == BattlegroundId.BattleforGilneas.ToString())
                        {
                            if (
                                String.IsNullOrWhiteSpace(
                                    BattlegrounderSetting.CurrentSetting.BattleforGilneasXmlProfile))
                                return false;
                            XMLProfile = true;
                            CurrentProfileName = BattlegrounderSetting.CurrentSetting.BattleforGilneasXmlProfile;
                        }
                        else if (CurrentBattlegroundName == BattlegroundId.TwinPeaks.ToString())
                        {
                            if (
                                String.IsNullOrWhiteSpace(
                                    BattlegrounderSetting.CurrentSetting.TwinPeaksXmlProfile))
                                return false;
                            XMLProfile = true;
                            CurrentProfileName = BattlegrounderSetting.CurrentSetting.TwinPeaksXmlProfile;
                        }
                        else if (CurrentBattlegroundName == BattlegroundId.TempleofKotmogu.ToString())
                        {
                            if (
                                String.IsNullOrWhiteSpace(
                                    BattlegrounderSetting.CurrentSetting.TempleofKotmoguXmlProfile))
                                return false;
                            XMLProfile = true;
                            CurrentProfileName = BattlegrounderSetting.CurrentSetting.TempleofKotmoguXmlProfile;
                        }
                        else if (CurrentBattlegroundName == BattlegroundId.SilvershardMines.ToString())
                        {
                            if (
                                String.IsNullOrWhiteSpace(
                                    BattlegrounderSetting.CurrentSetting.SilvershardMinesXmlProfile))
                                return false;
                            XMLProfile = true;
                            CurrentProfileName = BattlegrounderSetting.CurrentSetting.SilvershardMinesXmlProfile;
                        }
                        if (XMLProfile)
                        {
                            CurrentProfile = new BattlegrounderProfile();
                            if (File.Exists(Application.StartupPath + "\\Profiles\\Battlegrounder\\" +
                                            CurrentProfileName))
                            {
                                CurrentProfile =
                                    XmlSerializer.Deserialize<BattlegrounderProfile>(Application.StartupPath +
                                                                                     "\\Profiles\\Battlegrounder\\" +
                                                                                     CurrentProfileName);
                                if (CurrentProfile.BattlegrounderZones.Count > 0)
                                {
                                    XMLProfile = false;
                                    return true;
                                }
                            }
                            XMLProfile = false;
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
            SelectZone();


            // Black List:
            var blackListDic = new Dictionary<Point, float>();
            foreach (var zone in CurrentProfile.BattlegrounderZones)
            {
                foreach (var b in zone.BlackListRadius)
                {
                    blackListDic.Add(b.Position, b.Radius);
                }
            }
            nManagerSetting.AddRangeBlackListZone(blackListDic);
        }

        internal static void SelectZone()
        {
            ZoneIdProfile = 1337;
            for (int i = 0; i <= CurrentProfile.BattlegrounderZones.Count - 1; i++)
            {
                if (CurrentProfile.BattlegrounderZones[i].BattlegroundId ==
                    Battleground.GetCurrentBattleground().ToString())
                {
                    ZoneIdProfile = i;
                    break;
                }
            }
            if (ZoneIdProfile == 1337)
            {
                return;
            }

            if (CurrentProfile.BattlegrounderZones[ZoneIdProfile].Hotspots)
            {
                var pointsTemps = new List<Point>();
                for (int i = 0; i <= CurrentProfile.BattlegrounderZones[ZoneIdProfile].Points.Count - 1; i++)
                {
                    if (i + 1 > CurrentProfile.BattlegrounderZones[ZoneIdProfile].Points.Count - 1)
                        pointsTemps.AddRange(
                            PathFinder.FindPath(CurrentProfile.BattlegrounderZones[ZoneIdProfile].Points[i],
                                                CurrentProfile.BattlegrounderZones[ZoneIdProfile].Points[0]));
                    else
                        pointsTemps.AddRange(
                            PathFinder.FindPath(CurrentProfile.BattlegrounderZones[ZoneIdProfile].Points[i],
                                                CurrentProfile.BattlegrounderZones[ZoneIdProfile].Points[i + 1
                                                    ]));
                }
                CurrentProfile.BattlegrounderZones[ZoneIdProfile].Hotspots = false;
                CurrentProfile.BattlegrounderZones[ZoneIdProfile].Points.Clear();
                CurrentProfile.BattlegrounderZones[ZoneIdProfile].Points.AddRange(pointsTemps);
            }

            Bot._battlegrounding.BattlegroundId = CurrentProfile.BattlegrounderZones[ZoneIdProfile].BattlegroundId;

            Bot._movementLoop.PathLoop = CurrentProfile.BattlegrounderZones[ZoneIdProfile].Points;
        }
    }
}