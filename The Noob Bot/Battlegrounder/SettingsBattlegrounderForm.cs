using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Battlegrounder.Bot;
using Battlegrounder.Profile;
using Battlegrounder.Profiletype;
using nManager;
using nManager.Helpful;
using nManager.Helpful.Forms.UserControls;
using Battleground = nManager.Wow.Helpers.Battleground;

namespace Battlegrounder
{
    public partial class SettingsBattlegrounderForm : Form
    {
        private static readonly BattlegrounderProfileType ProfileTypeFile =
            XmlSerializer.Deserialize<BattlegrounderProfileType>(Application.StartupPath +
                                                                 "\\Profiles\\Battlegrounder\\ProfileType\\ProfileType.xml");

        private readonly List<TnbSwitchButton> _listsb =
            new List<TnbSwitchButton>();

        public SettingsBattlegrounderForm()
        {
            InitializeComponent();
            Translate();
            if (nManagerSetting.CurrentSetting.ActivateAlwaysOnTopFeature)
                TopMost = true;
            Load();
        }

        private void Translate()
        {
            AlteracValleyLabel.Text = nManager.Translate.Get(nManager.Translate.Id.QueueAlteracValley);
            WarsongGulchLabel.Text = nManager.Translate.Get(nManager.Translate.Id.QueueWarsongGulch);
            ArathiBasinLabel.Text = nManager.Translate.Get(nManager.Translate.Id.QueueArathiBasin);
            EyeoftheStormLabel.Text = nManager.Translate.Get(nManager.Translate.Id.QueueEyeoftheStorm);
            StrandoftheAncientsLabel.Text = nManager.Translate.Get(nManager.Translate.Id.QueueStrandoftheAncients);
            IsleofConquestLabel.Text = nManager.Translate.Get(nManager.Translate.Id.QueueIsleofConquest);
            TwinPeaksLabel.Text = nManager.Translate.Get(nManager.Translate.Id.QueueTwinPeaks);
            BattleforGilneasLabel.Text = nManager.Translate.Get(nManager.Translate.Id.QueueBattleforGilneas);
            TempleOfKotmoguLabel.Text = nManager.Translate.Get(nManager.Translate.Id.QueueTempleofKotmogu);
            SilvershardMinesLabel.Text = nManager.Translate.Get(nManager.Translate.Id.QueueSilvershardMines);
            RandomBattlegroundLabel.Text = nManager.Translate.Get(nManager.Translate.Id.QueueRandomBattleground);
            AlteracValley.Text = nManager.Translate.Get(nManager.Translate.Id.AlteracValley);
            WarsongGulch.Text = nManager.Translate.Get(nManager.Translate.Id.WarsongGulch);
            ArathiBasin.Text = nManager.Translate.Get(nManager.Translate.Id.ArathiBasin);
            EyeoftheStorm.Text = nManager.Translate.Get(nManager.Translate.Id.EyeoftheStorm);
            StrandoftheAncients.Text = nManager.Translate.Get(nManager.Translate.Id.StrandoftheAncients);
            IsleofConquest.Text = nManager.Translate.Get(nManager.Translate.Id.IsleofConquest);
            TwinPeaks.Text = nManager.Translate.Get(nManager.Translate.Id.TwinPeaks);
            BattleforGilneas.Text = nManager.Translate.Get(nManager.Translate.Id.BattleforGilneas);
            TempleofKotmogu.Text = nManager.Translate.Get(nManager.Translate.Id.TempleofKotmogu);
            SilvershardMines.Text = nManager.Translate.Get(nManager.Translate.Id.SilvershardMines);
            RandomBattleground.Text = nManager.Translate.Get(nManager.Translate.Id.RandomBattleground);
            RequeueAfterXMinutesLabel.Text = nManager.Translate.Get(nManager.Translate.Id.RequeueAfterXMinutes);
            SaveButton.Text = nManager.Translate.Get(nManager.Translate.Id.Save_and_Close);
            MainHeader.TitleText = nManager.Translate.Get(nManager.Translate.Id.Settings_Battlegrounder) + " - " + Information.MainTitle;
            this.Text = MainHeader.TitleText;
        }

        private void SaveButtonClick(object sender,
            EventArgs e)
        {
            Save();
        }

        private void Save()
        {
            BattlegrounderSetting.CurrentSetting.AlteracValley = AlteracValleySwitch.Value;
            var alteracValleyProfileTypeInfo = AlteracValleyProfileType.SelectedItem as ComboboxItem;
            if (alteracValleyProfileTypeInfo != null)
                BattlegrounderSetting.CurrentSetting.AlteracValleyProfileType =
                    alteracValleyProfileTypeInfo.Value.ToString();
            BattlegrounderSetting.CurrentSetting.AlteracValleyXmlProfile = XMLProfileListAlteracValley.Text;

            BattlegrounderSetting.CurrentSetting.WarsongGulch = WarsongGulchSwitch.Value;
            var warsongGulchProfileTypeInfo = WarsongGulchProfileType.SelectedItem as ComboboxItem;
            if (warsongGulchProfileTypeInfo != null)
                BattlegrounderSetting.CurrentSetting.WarsongGulchProfileType =
                    warsongGulchProfileTypeInfo.Value.ToString();
            BattlegrounderSetting.CurrentSetting.WarsongGulchXmlProfile = XMLProfileListWarsongGulch.Text;

            BattlegrounderSetting.CurrentSetting.ArathiBasin = ArathiBasinSwitch.Value;
            var arathiBasinProfileTypeInfo = ArathiBasinProfileType.SelectedItem as ComboboxItem;
            if (arathiBasinProfileTypeInfo != null)
                BattlegrounderSetting.CurrentSetting.ArathiBasinProfileType =
                    arathiBasinProfileTypeInfo.Value.ToString();
            BattlegrounderSetting.CurrentSetting.ArathiBasinXmlProfile = XMLProfileListArathiBasin.Text;

            BattlegrounderSetting.CurrentSetting.EyeoftheStorm = EyeoftheStormSwitch.Value;
            var eyeoftheStormProfileTypeInfo = EyeoftheStormProfileType.SelectedItem as ComboboxItem;
            if (eyeoftheStormProfileTypeInfo != null)
                BattlegrounderSetting.CurrentSetting.EyeoftheStormProfileType =
                    eyeoftheStormProfileTypeInfo.Value.ToString();
            BattlegrounderSetting.CurrentSetting.EyeoftheStormXmlProfile = XMLProfileListEyeoftheStorm.Text;

            BattlegrounderSetting.CurrentSetting.StrandoftheAncients = StrandoftheAncientsSwitch.Value;
            var strandoftheAncientsProfileTypeInfo = StrandoftheAncientsProfileType.SelectedItem as ComboboxItem;
            if (strandoftheAncientsProfileTypeInfo != null)
                BattlegrounderSetting.CurrentSetting.StrandoftheAncientsProfileType =
                    strandoftheAncientsProfileTypeInfo.Value.ToString();
            BattlegrounderSetting.CurrentSetting.StrandoftheAncientsXmlProfile =
                XMLProfileListStrandoftheAncients.Text;

            BattlegrounderSetting.CurrentSetting.IsleofConquest = IsleofConquestSwitch.Value;
            var isleofConquestProfileTypeInfo = IsleofConquestProfileType.SelectedItem as ComboboxItem;
            if (isleofConquestProfileTypeInfo != null)
                BattlegrounderSetting.CurrentSetting.IsleofConquestProfileType =
                    isleofConquestProfileTypeInfo.Value.ToString();
            BattlegrounderSetting.CurrentSetting.IsleofConquestXmlProfile = XMLProfileListIsleofConquest.Text;

            BattlegrounderSetting.CurrentSetting.TwinPeaks = TwinPeaksSwitch.Value;
            var twinPeaksProfileTypeInfo = TwinPeaksProfileType.SelectedItem as ComboboxItem;
            if (twinPeaksProfileTypeInfo != null)
                BattlegrounderSetting.CurrentSetting.TwinPeaksProfileType = twinPeaksProfileTypeInfo.Value.ToString();
            BattlegrounderSetting.CurrentSetting.TwinPeaksXmlProfile = XMLProfileListTwinPeaks.Text;

            BattlegrounderSetting.CurrentSetting.BattleforGilneas = BattleforGilneasSwitch.Value;
            var battleforGilneasProfileTypeInfo = BattleforGilneasProfileType.SelectedItem as ComboboxItem;
            if (battleforGilneasProfileTypeInfo != null)
                BattlegrounderSetting.CurrentSetting.BattleforGilneasProfileType =
                    battleforGilneasProfileTypeInfo.Value.ToString();
            BattlegrounderSetting.CurrentSetting.BattleforGilneasXmlProfile = XMLProfileListBattleforGilneas.Text;

            BattlegrounderSetting.CurrentSetting.TempleofKotmogu = TempleOfKotmoguSwitch.Value;
            var templeofKotmoguProfileTypeInfo = TempleofKotmoguProfileType.SelectedItem as ComboboxItem;
            if (templeofKotmoguProfileTypeInfo != null)
                BattlegrounderSetting.CurrentSetting.TempleofKotmoguProfileType =
                    templeofKotmoguProfileTypeInfo.Value.ToString();
            BattlegrounderSetting.CurrentSetting.TempleofKotmoguXmlProfile = XMLProfileListTempleofKotmogu.Text;

            BattlegrounderSetting.CurrentSetting.SilvershardMines = SilvershardMinesSwitch.Value;
            var silvershardMinesProfileTypeInfo = SilvershardMinesProfileType.SelectedItem as ComboboxItem;
            if (silvershardMinesProfileTypeInfo != null)
                BattlegrounderSetting.CurrentSetting.SilvershardMinesProfileType =
                    silvershardMinesProfileTypeInfo.Value.ToString();
            BattlegrounderSetting.CurrentSetting.SilvershardMinesXmlProfile = XMLProfileListSilvershardMines.Text;

            BattlegrounderSetting.CurrentSetting.RandomBattleground = RandomBattlegroundSwitch.Value;
            BattlegrounderSetting.CurrentSetting.RequeueAfterXMinutes = RequeueAfterXMinutesSwitch.Value;
            BattlegrounderSetting.CurrentSetting.RequeueAfterXMinutesTimer = (int) RequeueAfterXMinutes.Value;
            BattlegrounderSetting.CurrentSetting.Save();
            Dispose();
        }

        private new void Load()
        {
            _listsb.Add(ArathiBasinSwitch);
            _listsb.Add(AlteracValleySwitch);
            _listsb.Add(WarsongGulchSwitch);
            _listsb.Add(EyeoftheStormSwitch);
            _listsb.Add(StrandoftheAncientsSwitch);
            _listsb.Add(IsleofConquestSwitch);
            _listsb.Add(TwinPeaksSwitch);
            _listsb.Add(BattleforGilneasSwitch);
            _listsb.Add(TempleOfKotmoguSwitch);
            _listsb.Add(SilvershardMinesSwitch);

            AlteracValleySwitch.Value = BattlegrounderSetting.CurrentSetting.AlteracValley;
            WarsongGulchSwitch.Value = BattlegrounderSetting.CurrentSetting.WarsongGulch;
            ArathiBasinSwitch.Value = BattlegrounderSetting.CurrentSetting.ArathiBasin;
            EyeoftheStormSwitch.Value = BattlegrounderSetting.CurrentSetting.EyeoftheStorm;
            StrandoftheAncientsSwitch.Value = BattlegrounderSetting.CurrentSetting.StrandoftheAncients;
            IsleofConquestSwitch.Value = BattlegrounderSetting.CurrentSetting.IsleofConquest;
            TwinPeaksSwitch.Value = BattlegrounderSetting.CurrentSetting.TwinPeaks;
            BattleforGilneasSwitch.Value = BattlegrounderSetting.CurrentSetting.BattleforGilneas;
            TempleOfKotmoguSwitch.Value = BattlegrounderSetting.CurrentSetting.TempleofKotmogu;
            SilvershardMinesSwitch.Value = BattlegrounderSetting.CurrentSetting.SilvershardMines;
            RandomBattlegroundSwitch.Value = BattlegrounderSetting.CurrentSetting.RandomBattleground;
            RequeueAfterXMinutesSwitch.Value = BattlegrounderSetting.CurrentSetting.RequeueAfterXMinutes;
            RequeueAfterXMinutes.Value = BattlegrounderSetting.CurrentSetting.RequeueAfterXMinutesTimer;

            if (File.Exists(Application.StartupPath + "\\Profiles\\Battlegrounder\\ProfileType\\ProfileType.xml"))
            {
                if (ProfileTypeFile.Battlegrounds.Count > 0)
                    RefreshProfileListType();
                else
                    MessageBox.Show(@"Cannot load Profile Types list.");
            }
            else
                MessageBox.Show(@"Cannot load Profile Types list.");
            RefreshProfileList();
            XMLProfileListAlteracValley.Text = BattlegrounderSetting.CurrentSetting.AlteracValleyXmlProfile;
            XMLProfileListWarsongGulch.Text = BattlegrounderSetting.CurrentSetting.WarsongGulchXmlProfile;
            XMLProfileListArathiBasin.Text = BattlegrounderSetting.CurrentSetting.ArathiBasinXmlProfile;
            XMLProfileListEyeoftheStorm.Text = BattlegrounderSetting.CurrentSetting.EyeoftheStormXmlProfile;
            XMLProfileListStrandoftheAncients.Text =
                BattlegrounderSetting.CurrentSetting.StrandoftheAncientsXmlProfile;
            XMLProfileListIsleofConquest.Text = BattlegrounderSetting.CurrentSetting.IsleofConquestXmlProfile;
            XMLProfileListTwinPeaks.Text = BattlegrounderSetting.CurrentSetting.TwinPeaksXmlProfile;
            XMLProfileListBattleforGilneas.Text = BattlegrounderSetting.CurrentSetting.BattleforGilneasXmlProfile;
            XMLProfileListTempleofKotmogu.Text = BattlegrounderSetting.CurrentSetting.TempleofKotmoguXmlProfile;
            XMLProfileListSilvershardMines.Text = BattlegrounderSetting.CurrentSetting.SilvershardMinesXmlProfile;
        }

        private void RandomBattlegroundSwitchValueChanged(object sender,
            EventArgs e)
        {
            var sbi = sender as TnbSwitchButton;
            if (sbi != null && sbi.Value)
            {
                if (CountSwitchActive() > 0)
                {
                    sbi.Value = false;
                    MessageBox.Show(nManager.Translate.Get(nManager.Translate.Id.ErrorRandomQueue), "",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
        }

        private int CountSwitchActive()
        {
            return _listsb.Count(i => i.Value);
        }

        private void CheckAllSwitchEvent(object sender,
            EventArgs e)
        {
            var sbi = sender as TnbSwitchButton;
            if (RandomBattlegroundSwitch.Value)
            {
                if (sbi != null) sbi.Value = false;
                MessageBox.Show(nManager.Translate.Get(nManager.Translate.Id.ErrorSingleRandomQueue), "",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            if (CountSwitchActive() > 2)
            {
                if (sbi != null) sbi.Value = false;
                MessageBox.Show(nManager.Translate.Get(nManager.Translate.Id.ErrorMultipleQueue), "",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void CloseNoSaveButtonClick(object sender,
            EventArgs e)
        {
            Dispose();
        }

        private void CreateProfileBClick(object sender,
            EventArgs e)
        {
            try
            {
                if (Usefuls.IsInBattleground)
                {
                    var f = new ProfileCreator();
                    f.ShowDialog();
                }
                else
                {
                    MessageBox.Show(nManager.Translate.Get(nManager.Translate.Id.NotInBg));
                }
                RefreshProfileList();
            }
            catch (Exception ex)
            {
                Logging.WriteError(
                    "Battlegrounder > Bot > LoadProfile > createProfileB_Click(object sender, EventArgs ex): " +
                    ex);
            }
        }

        private void RefreshProfileListType()
        {
            try
            {
                AlteracValleyProfileType.Items.Clear();
                WarsongGulchProfileType.Items.Clear();
                ArathiBasinProfileType.Items.Clear();
                EyeoftheStormProfileType.Items.Clear();
                StrandoftheAncientsProfileType.Items.Clear();
                IsleofConquestProfileType.Items.Clear();
                BattleforGilneasProfileType.Items.Clear();
                TwinPeaksProfileType.Items.Clear();
                TempleofKotmoguProfileType.Items.Clear();
                SilvershardMinesProfileType.Items.Clear();
                foreach (Profiletype.Battleground battleground in ProfileTypeFile.Battlegrounds)
                {
                    for (int i = 0; i <= battleground.ProfileTypes.Count - 1; i++)
                    {
                        var item = new ComboboxItem
                        {
                            Text = battleground.ProfileTypes[i].ProfileTypeName,
                            Value = battleground.ProfileTypes[i].ProfileTypeId
                        };
                        if (battleground.BattlegroundId == AlteracValley.Name)
                        {
                            AlteracValleyProfileType.Items.Add(item);
                            if (battleground.ProfileTypes[i].ProfileTypeId ==
                                BattlegrounderSetting.CurrentSetting.AlteracValleyProfileType ||
                                AlteracValleyProfileType.SelectedIndex == -1)
                            {
                                AlteracValleyProfileType.SelectedIndex = i;
                            }
                        }
                        else if (battleground.BattlegroundId == WarsongGulch.Name)
                        {
                            WarsongGulchProfileType.Items.Add(item);
                            if (battleground.ProfileTypes[i].ProfileTypeId ==
                                BattlegrounderSetting.CurrentSetting.WarsongGulchProfileType ||
                                WarsongGulchProfileType.SelectedIndex == -1)
                            {
                                WarsongGulchProfileType.SelectedIndex = i;
                            }
                        }
                        else if (battleground.BattlegroundId == ArathiBasin.Name)
                        {
                            ArathiBasinProfileType.Items.Add(item);
                            if (battleground.ProfileTypes[i].ProfileTypeId ==
                                BattlegrounderSetting.CurrentSetting.ArathiBasinProfileType ||
                                ArathiBasinProfileType.SelectedIndex == -1)
                            {
                                ArathiBasinProfileType.SelectedIndex = i;
                            }
                        }
                        else if (battleground.BattlegroundId == EyeoftheStorm.Name)
                        {
                            EyeoftheStormProfileType.Items.Add(item);
                            if (battleground.ProfileTypes[i].ProfileTypeId ==
                                BattlegrounderSetting.CurrentSetting.EyeoftheStormProfileType ||
                                EyeoftheStormProfileType.SelectedIndex == -1)
                            {
                                EyeoftheStormProfileType.SelectedIndex = i;
                            }
                        }
                        else if (battleground.BattlegroundId == StrandoftheAncients.Name)
                        {
                            StrandoftheAncientsProfileType.Items.Add(item);
                            if (battleground.ProfileTypes[i].ProfileTypeId ==
                                BattlegrounderSetting.CurrentSetting.StrandoftheAncientsProfileType ||
                                StrandoftheAncientsProfileType.SelectedIndex == -1)
                            {
                                StrandoftheAncientsProfileType.SelectedIndex = i;
                            }
                        }
                        else if (battleground.BattlegroundId == IsleofConquest.Name)
                        {
                            IsleofConquestProfileType.Items.Add(item);
                            if (battleground.ProfileTypes[i].ProfileTypeId ==
                                BattlegrounderSetting.CurrentSetting.IsleofConquestProfileType ||
                                IsleofConquestProfileType.SelectedIndex == -1)
                            {
                                IsleofConquestProfileType.SelectedIndex = i;
                            }
                        }
                        else if (battleground.BattlegroundId == BattleforGilneas.Name)
                        {
                            BattleforGilneasProfileType.Items.Add(item);
                            if (battleground.ProfileTypes[i].ProfileTypeId ==
                                BattlegrounderSetting.CurrentSetting.BattleforGilneasProfileType ||
                                BattleforGilneasProfileType.SelectedIndex == -1)
                            {
                                BattleforGilneasProfileType.SelectedIndex = i;
                            }
                        }
                        else if (battleground.BattlegroundId == TwinPeaks.Name)
                        {
                            TwinPeaksProfileType.Items.Add(item);
                            if (battleground.ProfileTypes[i].ProfileTypeId ==
                                BattlegrounderSetting.CurrentSetting.TwinPeaksProfileType ||
                                TwinPeaksProfileType.SelectedIndex == -1)
                            {
                                TwinPeaksProfileType.SelectedIndex = i;
                            }
                        }
                        else if (battleground.BattlegroundId == TempleofKotmogu.Name)
                        {
                            TempleofKotmoguProfileType.Items.Add(item);
                            if (battleground.ProfileTypes[i].ProfileTypeId ==
                                BattlegrounderSetting.CurrentSetting.TempleofKotmoguProfileType ||
                                TempleofKotmoguProfileType.SelectedIndex == -1)
                            {
                                TempleofKotmoguProfileType.SelectedIndex = i;
                            }
                        }
                        else if (battleground.BattlegroundId == SilvershardMines.Name)
                        {
                            SilvershardMinesProfileType.Items.Add(item);
                            if (battleground.ProfileTypes[i].ProfileTypeId ==
                                BattlegrounderSetting.CurrentSetting.SilvershardMinesProfileType ||
                                SilvershardMinesProfileType.SelectedIndex == -1)
                            {
                                SilvershardMinesProfileType.SelectedIndex = i;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("Battlegrounder > Bot > LoadProfile > RefreshProfileList(): " + e);
            }
        }

        private void RefreshProfileList()
        {
            try
            {
                string alteracValley = XMLProfileListAlteracValley.Text;
                XMLProfileListAlteracValley.Items.Clear();
                string warsongGulch = XMLProfileListWarsongGulch.Text;
                XMLProfileListWarsongGulch.Items.Clear();
                string arathiBasin = XMLProfileListArathiBasin.Text;
                XMLProfileListArathiBasin.Items.Clear();
                string eyeoftheStorm = XMLProfileListEyeoftheStorm.Text;
                XMLProfileListEyeoftheStorm.Items.Clear();
                string strandoftheAncients = XMLProfileListStrandoftheAncients.Text;
                XMLProfileListStrandoftheAncients.Items.Clear();
                string isleofConquest = XMLProfileListIsleofConquest.Text;
                XMLProfileListIsleofConquest.Items.Clear();
                string battleforGilneas = XMLProfileListBattleforGilneas.Text;
                XMLProfileListBattleforGilneas.Items.Clear();
                string twinPeaks = XMLProfileListTwinPeaks.Text;
                XMLProfileListTwinPeaks.Items.Clear();
                string templeofKotmogu = XMLProfileListTempleofKotmogu.Text;
                XMLProfileListTempleofKotmogu.Items.Clear();
                string silvershardMines = XMLProfileListSilvershardMines.Text;
                XMLProfileListSilvershardMines.Items.Clear();
                foreach (
                    string f in
                        Others.GetFilesDirectory(Application.StartupPath + "\\Profiles\\Battlegrounder\\", "*.xml")
                    )
                {
                    XMLProfileListAlteracValley.Items.Add(f);
                    XMLProfileListWarsongGulch.Items.Add(f);
                    XMLProfileListArathiBasin.Items.Add(f);
                    XMLProfileListEyeoftheStorm.Items.Add(f);
                    XMLProfileListStrandoftheAncients.Items.Add(f);
                    XMLProfileListIsleofConquest.Items.Add(f);
                    XMLProfileListBattleforGilneas.Items.Add(f);
                    XMLProfileListTwinPeaks.Items.Add(f);
                    XMLProfileListTempleofKotmogu.Items.Add(f);
                    XMLProfileListSilvershardMines.Items.Add(f);
                }
                XMLProfileListAlteracValley.Text = alteracValley;
                XMLProfileListWarsongGulch.Text = warsongGulch;
                XMLProfileListArathiBasin.Text = arathiBasin;
                XMLProfileListEyeoftheStorm.Text = eyeoftheStorm;
                XMLProfileListStrandoftheAncients.Text = strandoftheAncients;
                XMLProfileListIsleofConquest.Text = isleofConquest;
                XMLProfileListBattleforGilneas.Text = battleforGilneas;
                XMLProfileListTwinPeaks.Text = twinPeaks;
                XMLProfileListTempleofKotmogu.Text = templeofKotmogu;
                XMLProfileListSilvershardMines.Text = silvershardMines;
            }
            catch (Exception e)
            {
                Logging.WriteError("Battlegrounder > Bot > LoadProfile > RefreshProfileList(): " + e);
            }
        }
    }
}