using System.Windows.Forms;
using Battlegrounder.Bot;
using nManager;
using nManager.Helpful;

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
            WarsongGulchLabel.Text = Translate.Get(Translate.Id.WarsongGulch);
            ArathiBasinLabel.Text = Translate.Get(Translate.Id.ArathiBasin);
            AlteracValleyLabel.Text = Translate.Get(Translate.Id.AlteracValley);
            EyeoftheStormLabel.Text = Translate.Get(Translate.Id.EyeoftheStorm);
            StrandoftheAncientsLabel.Text = Translate.Get(Translate.Id.StrandoftheAncients);
            IsleofConquestLabel.Text = Translate.Get(Translate.Id.IsleofConquest);
            BattleforGilneasLabel.Text = Translate.Get(Translate.Id.BattleforGilneas);
            TwinPeaksLabel.Text = Translate.Get(Translate.Id.TwinPeaks);
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
            BattlegrounderSetting.CurrentSetting.WarsongGulch = WarsongGulchSwitch.Value;
            BattlegrounderSetting.CurrentSetting.ArathiBasin = ArathiBasinSwitch.Value;
            BattlegrounderSetting.CurrentSetting.AlteracValley = AlteracValleySwitch.Value;
            BattlegrounderSetting.CurrentSetting.EyeoftheStorm = EyeoftheStormSwitch.Value;
            BattlegrounderSetting.CurrentSetting.StrandoftheAncients = StrandoftheAncientsSwitch.Value;
            BattlegrounderSetting.CurrentSetting.BattleforGilneas = BattleforGilneasSwitch.Value;
            BattlegrounderSetting.CurrentSetting.TwinPeaks = TwinPeaksSwitch.Value;
            BattlegrounderSetting.CurrentSetting.RandomBattleground = RandomBattlegroundSwitch.Value;
            BattlegrounderSetting.CurrentSetting.Save();
            Dispose();
        }

        private new void Load()
        {
            WarsongGulchSwitch.Value = BattlegrounderSetting.CurrentSetting.WarsongGulch;
            ArathiBasinSwitch.Value = BattlegrounderSetting.CurrentSetting.ArathiBasin;
            AlteracValleySwitch.Value = BattlegrounderSetting.CurrentSetting.AlteracValley;
            EyeoftheStormSwitch.Value = BattlegrounderSetting.CurrentSetting.EyeoftheStorm;
            StrandoftheAncientsSwitch.Value = BattlegrounderSetting.CurrentSetting.StrandoftheAncients;
            BattleforGilneasSwitch.Value = BattlegrounderSetting.CurrentSetting.BattleforGilneas;
            TwinPeaksSwitch.Value = BattlegrounderSetting.CurrentSetting.TwinPeaks;
            RandomBattlegroundSwitch.Value = BattlegrounderSetting.CurrentSetting.RandomBattleground;
        }
    }
}