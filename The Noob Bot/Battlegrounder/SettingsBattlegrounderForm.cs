using Battlegrounder.Bot;
using nManager;

namespace Battlegrounder
{
    public partial class SettingsBattlegrounderForm : DevComponents.DotNetBar.Metro.MetroForm
    {
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
            BattlegrounderSetting.CurrentSetting.WarsongGulch = WarsongGulchSwitch.Value;
            BattlegrounderSetting.CurrentSetting.ArathiBasin = ArathiBasinSwitch.Value;
            BattlegrounderSetting.CurrentSetting.EyeoftheStorm = EyeoftheStormSwitch.Value;
            BattlegrounderSetting.CurrentSetting.StrandoftheAncients = StrandoftheAncientsSwitch.Value;
            BattlegrounderSetting.CurrentSetting.IsleofConquest = IsleofConquestSwitch.Value;
            BattlegrounderSetting.CurrentSetting.TwinPeaks = TwinPeaksSwitch.Value;
            BattlegrounderSetting.CurrentSetting.BattleforGilneas = BattleforGilneasSwitch.Value;
            BattlegrounderSetting.CurrentSetting.TempleofKotmogu = TempleOfKotmoguSwitch.Value;
            BattlegrounderSetting.CurrentSetting.SilvershardMines = SilvershardMinesSwitch.Value;
            BattlegrounderSetting.CurrentSetting.RandomBattleground = RandomBattlegroundSwitch.Value;
            BattlegrounderSetting.CurrentSetting.Save();
            Dispose();
        }

        private new void Load()
        {
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
        }
    }
}