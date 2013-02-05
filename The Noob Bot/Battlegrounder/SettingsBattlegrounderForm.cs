using System;
using System.IO;
using Battlegrounder.Bot;
using Battlegrounder.Profile;
using Battlegrounder.Profiletype;
using DevComponents.DotNetBar;
using nManager;
using System.Windows.Forms;
using System.Collections.Generic;
using nManager.Helpful;
using Battleground = nManager.Wow.Helpers.Battleground;

namespace Battlegrounder
{
    public partial class SettingsBattlegrounderForm : DevComponents.DotNetBar.Metro.MetroForm
    {
        internal static BattlegrounderProfileType ProfileTypeFile = new BattlegrounderProfileType();

        private List<DevComponents.DotNetBar.Controls.SwitchButton> listsb =
            new List<DevComponents.DotNetBar.Controls.SwitchButton>();

        public SettingsBattlegrounderForm()
        {
            InitializeComponent();
            translate();
            if (nManagerSetting.CurrentSetting.ActivateAlwaysOnTopFeature)
                this.TopMost = true;
            Load();
        }

        private void translate()
        {
            AlteracValleyLabel.Text = Translate.Get(Translate.Id.AlteracValley);
            WarsongGulchLabel.Text = Translate.Get(Translate.Id.WarsongGulch);
            ArathiBasinLabel.Text = Translate.Get(Translate.Id.ArathiBasin);
            EyeoftheStormLabel.Text = Translate.Get(Translate.Id.EyeoftheStorm);
            StrandoftheAncientsLabel.Text = Translate.Get(Translate.Id.StrandoftheAncients);
            IsleofConquestLabel.Text = Translate.Get(Translate.Id.IsleofConquest);
            TwinPeaksLabel.Text = Translate.Get(Translate.Id.TwinPeaks);
            BattleforGilneasLabel.Text = Translate.Get(Translate.Id.BattleforGilneas);
            TempleOfKotmoguLabel.Text = Translate.Get(Translate.Id.TempleofKotmogu);
            SilvershardMinesLabel.Text = Translate.Get(Translate.Id.SilvershardMines);
            RandomBattlegroundLabel.Text = Translate.Get(Translate.Id.RandomBattleground);
            RequeueAfterXMinutesLabel.Text = Translate.Get(Translate.Id.RequeueAfterXMinutes);
            SaveButton.Text = Translate.Get(Translate.Id.Save_and_Close);
            Text = Translate.Get(Translate.Id.Settings_Battlegrounder);
        }

        private void SaveButton_Click(object sender, System.EventArgs e)
        {
            Save();
        }

        private void Save()
        {
            BattlegrounderSetting.CurrentSetting.AlteracValley = AlteracValleySwitch.Value;
            BattlegrounderSetting.CurrentSetting.AlteracValleyProfileType = AlteracValleyProfileType.Text;
            BattlegrounderSetting.CurrentSetting.XMLProfileListAlteracValley = XMLProfileListAlteracValley.Text;

            BattlegrounderSetting.CurrentSetting.WarsongGulch = WarsongGulchSwitch.Value;
            BattlegrounderSetting.CurrentSetting.WarsongGulchProfileType = WarsongGulchProfileType.Text;
            BattlegrounderSetting.CurrentSetting.XMLProfileListWarsongGulch = XMLProfileListWarsongGulch.Text;

            BattlegrounderSetting.CurrentSetting.ArathiBasin = ArathiBasinSwitch.Value;
            BattlegrounderSetting.CurrentSetting.ArathiBasinProfileType = ArathiBasinProfileType.Text;
            BattlegrounderSetting.CurrentSetting.XMLProfileListArathiBasin = XMLProfileListArathiBasin.Text;

            BattlegrounderSetting.CurrentSetting.EyeoftheStorm = EyeoftheStormSwitch.Value;
            BattlegrounderSetting.CurrentSetting.EyeoftheStormProfileType = EyeoftheStormProfileType.Text;
            BattlegrounderSetting.CurrentSetting.XMLProfileListEyeoftheStorm = XMLProfileListEyeoftheStorm.Text;

            BattlegrounderSetting.CurrentSetting.StrandoftheAncients = StrandoftheAncientsSwitch.Value;
            BattlegrounderSetting.CurrentSetting.StrandoftheAncientsProfileType = StrandoftheAncientsProfileType.Text;
            BattlegrounderSetting.CurrentSetting.XMLProfileListStrandoftheAncients = XMLProfileListStrandoftheAncients.Text;

            BattlegrounderSetting.CurrentSetting.IsleofConquest = IsleofConquestSwitch.Value;
            BattlegrounderSetting.CurrentSetting.IsleofConquestProfileType = IsleofConquestProfileType.Text;
            BattlegrounderSetting.CurrentSetting.XMLProfileListIsleofConquest = XMLProfileListIsleofConquest.Text;

            BattlegrounderSetting.CurrentSetting.TwinPeaks = TwinPeaksSwitch.Value;
            BattlegrounderSetting.CurrentSetting.TwinPeaksProfileType = TwinPeaksProfileType.Text;
            BattlegrounderSetting.CurrentSetting.XMLProfileListTwinPeaks = XMLProfileListTwinPeaks.Text;

            BattlegrounderSetting.CurrentSetting.BattleforGilneas = BattleforGilneasSwitch.Value;
            BattlegrounderSetting.CurrentSetting.BattleforGilneasProfileType = BattleforGilneasProfileType.Text;
            BattlegrounderSetting.CurrentSetting.XMLProfileListBattleforGilneas = XMLProfileListBattleforGilneas.Text;

            BattlegrounderSetting.CurrentSetting.TempleofKotmogu = TempleOfKotmoguSwitch.Value;
            BattlegrounderSetting.CurrentSetting.TempleofKotmoguProfileType = TempleofKotmoguProfileType.Text;
            BattlegrounderSetting.CurrentSetting.XMLProfileListTempleofKotmogu = XMLProfileListTempleofKotmogu.Text;

            BattlegrounderSetting.CurrentSetting.SilvershardMines = SilvershardMinesSwitch.Value;
            BattlegrounderSetting.CurrentSetting.SilvershardMinesProfileType = SilvershardMinesProfileType.Text;
            BattlegrounderSetting.CurrentSetting.XMLProfileListSilvershardMines = XMLProfileListSilvershardMines.Text;

            BattlegrounderSetting.CurrentSetting.RandomBattleground = RandomBattlegroundSwitch.Value;
            BattlegrounderSetting.CurrentSetting.RequeueAfterXMinutes = RequeueAfterXMinutesSwitch.Value;
            BattlegrounderSetting.CurrentSetting.RequeueAfterXMinutesTimer = RequeueAfterXMinutes.Value;
            BattlegrounderSetting.CurrentSetting.Save();
            Dispose();
        }

        private new void Load()
        {
            listsb.Add(ArathiBasinSwitch);
            listsb.Add(AlteracValleySwitch);
            listsb.Add(WarsongGulchSwitch);
            listsb.Add(EyeoftheStormSwitch);
            listsb.Add(StrandoftheAncientsSwitch);
            listsb.Add(IsleofConquestSwitch);
            listsb.Add(TwinPeaksSwitch);
            listsb.Add(BattleforGilneasSwitch);
            listsb.Add(TempleOfKotmoguSwitch);
            listsb.Add(SilvershardMinesSwitch);

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
                ProfileTypeFile = new BattlegrounderProfileType();
                ProfileTypeFile =
                    XmlSerializer.Deserialize<BattlegrounderProfileType>(Application.StartupPath +
                                                                         "\\Profiles\\Battlegrounder\\ProfileType\\ProfileType.xml");
                if (ProfileTypeFile.Battlegrounds.Count > 0)
                    RefreshProfileListType();
                else
                    MessageBox.Show("Cannot load Profile Types list.");
            }
            else
                MessageBox.Show("Cannot load Profile Types list.");
            AlteracValleyProfileType.Text = BattlegrounderSetting.CurrentSetting.AlteracValleyProfileType;
            XMLProfileListAlteracValley.Text = BattlegrounderSetting.CurrentSetting.XMLProfileListAlteracValley;
            WarsongGulchProfileType.Text = BattlegrounderSetting.CurrentSetting.WarsongGulchProfileType;
            XMLProfileListWarsongGulch.Text = BattlegrounderSetting.CurrentSetting.XMLProfileListWarsongGulch;
            ArathiBasinProfileType.Text = BattlegrounderSetting.CurrentSetting.ArathiBasinProfileType;
            XMLProfileListArathiBasin.Text = BattlegrounderSetting.CurrentSetting.XMLProfileListArathiBasin;
            EyeoftheStormProfileType.Text = BattlegrounderSetting.CurrentSetting.EyeoftheStormProfileType;
            XMLProfileListEyeoftheStorm.Text = BattlegrounderSetting.CurrentSetting.XMLProfileListEyeoftheStorm;
            StrandoftheAncientsProfileType.Text = BattlegrounderSetting.CurrentSetting.StrandoftheAncientsProfileType;
            XMLProfileListStrandoftheAncients.Text =
                BattlegrounderSetting.CurrentSetting.XMLProfileListStrandoftheAncients;
            IsleofConquestProfileType.Text = BattlegrounderSetting.CurrentSetting.IsleofConquestProfileType;
            XMLProfileListIsleofConquest.Text = BattlegrounderSetting.CurrentSetting.XMLProfileListIsleofConquest;
            TwinPeaksProfileType.Text = BattlegrounderSetting.CurrentSetting.TwinPeaksProfileType;
            XMLProfileListTwinPeaks.Text = BattlegrounderSetting.CurrentSetting.XMLProfileListTwinPeaks;
            BattleforGilneasProfileType.Text = BattlegrounderSetting.CurrentSetting.BattleforGilneasProfileType;
            XMLProfileListBattleforGilneas.Text = BattlegrounderSetting.CurrentSetting.XMLProfileListBattleforGilneas;
            TempleofKotmoguProfileType.Text = BattlegrounderSetting.CurrentSetting.TempleofKotmoguProfileType;
            XMLProfileListTempleofKotmogu.Text = BattlegrounderSetting.CurrentSetting.XMLProfileListTempleofKotmogu;
            SilvershardMinesProfileType.Text = BattlegrounderSetting.CurrentSetting.SilvershardMinesProfileType;
            XMLProfileListSilvershardMines.Text = BattlegrounderSetting.CurrentSetting.XMLProfileListSilvershardMines;
        }

        private void RandomBattlegroundSwitch_ValueChanged(object sender, System.EventArgs e)
        {
            DevComponents.DotNetBar.Controls.SwitchButton sbi = sender as DevComponents.DotNetBar.Controls.SwitchButton;
            if (sbi.Value == true)
            {
                if (CountSwitchActive() > 0)
                {
                    sbi.Value = false;
                    MessageBox.Show(Translate.Get(Translate.Id.ErrorRandomQueue), "", MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                }
            }
        }

        private int CountSwitchActive()
        {
            int cnt = 0;
            foreach (DevComponents.DotNetBar.Controls.SwitchButton i in listsb)
                if (i.Value == true)
                    cnt++;

            return cnt;
        }

        private void CheckAll_Switch_Event(object sender, System.EventArgs e)
        {
            DevComponents.DotNetBar.Controls.SwitchButton sbi = sender as DevComponents.DotNetBar.Controls.SwitchButton;
            if (RandomBattlegroundSwitch.Value == true)
            {
                sbi.Value = false;
                MessageBox.Show(Translate.Get(Translate.Id.ErrorSingleRandomQueue), "", MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                return;
            }

            if (CountSwitchActive() > 2)
            {
                sbi.Value = false;
                MessageBox.Show(Translate.Get(Translate.Id.ErrorMultipleQueue), "", MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
            }
        }

        private void CloseNoSaveButton_Click(object sender, System.EventArgs e)
        {
            Dispose();
        }

        private void createProfileB_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (Battleground.IsInBattleground())
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
                var AlteracValleyb = AlteracValleyProfileType.Text;
                AlteracValleyProfileType.Items.Clear();
                var WarsongGulchb = WarsongGulchProfileType.Text;
                WarsongGulchProfileType.Items.Clear();
                var ArathiBasinb = ArathiBasinProfileType.Text;
                ArathiBasinProfileType.Items.Clear();
                var EyeoftheStormb = EyeoftheStormProfileType.Text;
                EyeoftheStormProfileType.Items.Clear();
                var StrandoftheAncientsb = StrandoftheAncientsProfileType.Text;
                StrandoftheAncientsProfileType.Items.Clear();
                var IsleofConquestb = IsleofConquestProfileType.Text;
                IsleofConquestProfileType.Items.Clear();
                var BattleforGilneasb = BattleforGilneasProfileType.Text;
                BattleforGilneasProfileType.Items.Clear();
                var TwinPeaksb = TwinPeaksProfileType.Text;
                TwinPeaksProfileType.Items.Clear();
                var TempleOfKotmogub = TempleofKotmoguProfileType.Text;
                TempleofKotmoguProfileType.Items.Clear();
                var SilvershardMinesb = SilvershardMinesProfileType.Text;
                SilvershardMinesProfileType.Items.Clear();
                Battlegrounder.Profiletype.ComboboxItem item = new Battlegrounder.Profiletype.ComboboxItem();
                for (int x = 0; x <= ProfileTypeFile.Battlegrounds.Count - 1; x++)
                {
                    if (ProfileTypeFile.Battlegrounds[x].BattlegroundId == AlteracValley.Name)
                    {
                        for (int y = 0; y <= ProfileTypeFile.Battlegrounds[x].ProfileTypes.Count - 1; y++)
                        {
                            item = new Battlegrounder.Profiletype.ComboboxItem();
                            item.Text = ProfileTypeFile.Battlegrounds[x].ProfileTypes[y].ProfileTypeName;
                            item.Value = ProfileTypeFile.Battlegrounds[x].ProfileTypes[y].ProfileTypeId;
                            AlteracValleyProfileType.Items.Add(item);
                        }
                    }
                    if (ProfileTypeFile.Battlegrounds[x].BattlegroundId == WarsongGulch.Name)
                    {
                        for (int y = 0; y <= ProfileTypeFile.Battlegrounds[x].ProfileTypes.Count - 1; y++)
                        {
                            item = new Battlegrounder.Profiletype.ComboboxItem();
                            item.Text = ProfileTypeFile.Battlegrounds[x].ProfileTypes[y].ProfileTypeName;
                            item.Value = ProfileTypeFile.Battlegrounds[x].ProfileTypes[y].ProfileTypeId;
                            WarsongGulchProfileType.Items.Add(item);
                        }
                    }
                    if (ProfileTypeFile.Battlegrounds[x].BattlegroundId == ArathiBasin.Name)
                    {
                        for (int y = 0; y <= ProfileTypeFile.Battlegrounds[x].ProfileTypes.Count - 1; y++)
                        {
                            item = new Battlegrounder.Profiletype.ComboboxItem();
                            item.Text = ProfileTypeFile.Battlegrounds[x].ProfileTypes[y].ProfileTypeName;
                            item.Value = ProfileTypeFile.Battlegrounds[x].ProfileTypes[y].ProfileTypeId;
                            ArathiBasinProfileType.Items.Add(item);
                        }
                    }
                    if (ProfileTypeFile.Battlegrounds[x].BattlegroundId == EyeoftheStorm.Name)
                    {
                        for (int y = 0; y <= ProfileTypeFile.Battlegrounds[x].ProfileTypes.Count - 1; y++)
                        {
                            item = new Battlegrounder.Profiletype.ComboboxItem();
                            item.Text = ProfileTypeFile.Battlegrounds[x].ProfileTypes[y].ProfileTypeName;
                            item.Value = ProfileTypeFile.Battlegrounds[x].ProfileTypes[y].ProfileTypeId;
                            EyeoftheStormProfileType.Items.Add(item);
                        }
                    }
                    if (ProfileTypeFile.Battlegrounds[x].BattlegroundId == StrandoftheAncients.Name)
                    {
                        for (int y = 0; y <= ProfileTypeFile.Battlegrounds[x].ProfileTypes.Count - 1; y++)
                        {
                            item = new Battlegrounder.Profiletype.ComboboxItem();
                            item.Text = ProfileTypeFile.Battlegrounds[x].ProfileTypes[y].ProfileTypeName;
                            item.Value = ProfileTypeFile.Battlegrounds[x].ProfileTypes[y].ProfileTypeId;
                            StrandoftheAncientsProfileType.Items.Add(item);
                        }
                    }
                    if (ProfileTypeFile.Battlegrounds[x].BattlegroundId == IsleofConquest.Name)
                    {
                        for (int y = 0; y <= ProfileTypeFile.Battlegrounds[x].ProfileTypes.Count - 1; y++)
                        {
                            item = new Battlegrounder.Profiletype.ComboboxItem();
                            item.Text = ProfileTypeFile.Battlegrounds[x].ProfileTypes[y].ProfileTypeName;
                            item.Value = ProfileTypeFile.Battlegrounds[x].ProfileTypes[y].ProfileTypeId;
                            IsleofConquestProfileType.Items.Add(item);
                        }
                    }
                    if (ProfileTypeFile.Battlegrounds[x].BattlegroundId == BattleforGilneas.Name)
                    {
                        for (int y = 0; y <= ProfileTypeFile.Battlegrounds[x].ProfileTypes.Count - 1; y++)
                        {
                            item = new Battlegrounder.Profiletype.ComboboxItem();
                            item.Text = ProfileTypeFile.Battlegrounds[x].ProfileTypes[y].ProfileTypeName;
                            item.Value = ProfileTypeFile.Battlegrounds[x].ProfileTypes[y].ProfileTypeId;
                            BattleforGilneasProfileType.Items.Add(item);
                        }
                    }
                    if (ProfileTypeFile.Battlegrounds[x].BattlegroundId == TwinPeaks.Name)
                    {
                        for (int y = 0; y <= ProfileTypeFile.Battlegrounds[x].ProfileTypes.Count - 1; y++)
                        {
                            item = new Battlegrounder.Profiletype.ComboboxItem();
                            item.Text = ProfileTypeFile.Battlegrounds[x].ProfileTypes[y].ProfileTypeName;
                            item.Value = ProfileTypeFile.Battlegrounds[x].ProfileTypes[y].ProfileTypeId;
                            TwinPeaksProfileType.Items.Add(item);
                        }
                    }
                    if (ProfileTypeFile.Battlegrounds[x].BattlegroundId == TempleofKotmogu.Name)
                    {
                        for (int y = 0; y <= ProfileTypeFile.Battlegrounds[x].ProfileTypes.Count - 1; y++)
                        {
                            item = new Battlegrounder.Profiletype.ComboboxItem();
                            item.Text = ProfileTypeFile.Battlegrounds[x].ProfileTypes[y].ProfileTypeName;
                            item.Value = ProfileTypeFile.Battlegrounds[x].ProfileTypes[y].ProfileTypeId;
                            TempleofKotmoguProfileType.Items.Add(item);
                        }
                    }
                    if (ProfileTypeFile.Battlegrounds[x].BattlegroundId == SilvershardMines.Name)
                    {
                        for (int y = 0; y <= ProfileTypeFile.Battlegrounds[x].ProfileTypes.Count - 1; y++)
                        {
                            item = new Battlegrounder.Profiletype.ComboboxItem();
                            item.Text = ProfileTypeFile.Battlegrounds[x].ProfileTypes[y].ProfileTypeName;
                            item.Value = ProfileTypeFile.Battlegrounds[x].ProfileTypes[y].ProfileTypeId;
                            SilvershardMinesProfileType.Items.Add(item);
                        }
                    }
                }
                /*for (int x = 0; x <= ProfileTypeFile.Battlegrounds.Count - 1; x++)
                {
                    for (int y = 0; y <= ProfileTypeFile.Battlegrounds[x].ProfileTypes.Count - 1; y++)
                    {
                        item = new Battlegrounder.Profiletype.ComboboxItem();
                        item.Text = ProfileTypeFile.Battlegrounds[x].ProfileTypes[y].ProfileTypeName;
                        item.Value = ProfileTypeFile.Battlegrounds[x].ProfileTypes[y].ProfileTypeId;
                        this[ProfileTypeFile.Battlegrounds[x].ProfileTypes[y].ProfileTypeId].Items.Add(item);
                    }
                }*/
                AlteracValleyProfileType.Text = AlteracValleyb;
                WarsongGulchProfileType.Text = WarsongGulchb;
                ArathiBasinProfileType.Text = ArathiBasinb;
                EyeoftheStormProfileType.Text = EyeoftheStormb;
                StrandoftheAncientsProfileType.Text = StrandoftheAncientsb;
                IsleofConquestProfileType.Text = IsleofConquestb;
                BattleforGilneasProfileType.Text = BattleforGilneasb;
                TwinPeaksProfileType.Text = TwinPeaksb;
                TempleofKotmoguProfileType.Text = TempleOfKotmogub;
                SilvershardMinesProfileType.Text = SilvershardMinesb;
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
                var AlteracValley = XMLProfileListAlteracValley.Text;
                XMLProfileListAlteracValley.Items.Clear();
                var WarsongGulch = XMLProfileListWarsongGulch.Text;
                XMLProfileListWarsongGulch.Items.Clear();
                var ArathiBasin = XMLProfileListArathiBasin.Text;
                XMLProfileListArathiBasin.Items.Clear();
                var EyeoftheStorm = XMLProfileListEyeoftheStorm.Text;
                XMLProfileListEyeoftheStorm.Items.Clear();
                var StrandoftheAncients = XMLProfileListStrandoftheAncients.Text;
                XMLProfileListStrandoftheAncients.Items.Clear();
                var IsleofConquest = XMLProfileListIsleofConquest.Text;
                XMLProfileListIsleofConquest.Items.Clear();
                var BattleforGilneas = XMLProfileListBattleforGilneas.Text;
                XMLProfileListBattleforGilneas.Items.Clear();
                var TwinPeaks = XMLProfileListTwinPeaks.Text;
                XMLProfileListTwinPeaks.Items.Clear();
                var TempleofKotmogu = XMLProfileListTempleofKotmogu.Text;
                XMLProfileListTempleofKotmogu.Items.Clear();
                var SilvershardMines = XMLProfileListSilvershardMines.Text;
                XMLProfileListSilvershardMines.Items.Clear();
                foreach (
                    var f in Others.GetFilesDirectory(Application.StartupPath + "\\Profiles\\Battlegrounder\\", "*.xml")
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
                XMLProfileListAlteracValley.Text = AlteracValley;
                XMLProfileListWarsongGulch.Text = WarsongGulch;
                XMLProfileListArathiBasin.Text = ArathiBasin;
                XMLProfileListEyeoftheStorm.Text = EyeoftheStorm;
                XMLProfileListStrandoftheAncients.Text = StrandoftheAncients;
                XMLProfileListIsleofConquest.Text = IsleofConquest;
                XMLProfileListBattleforGilneas.Text = BattleforGilneas;
                XMLProfileListTwinPeaks.Text = TwinPeaks;
                XMLProfileListTempleofKotmogu.Text = TempleofKotmogu;
                XMLProfileListSilvershardMines.Text = SilvershardMines;
            }
            catch (Exception e)
            {
                Logging.WriteError("Battlegrounder > Bot > LoadProfile > RefreshProfileList(): " + e);
            }
        }
    }
}