using System.Windows.Forms;
using Fisherbot.Bot;
using nManager;
using nManager.Helpful;

namespace Fisherbot
{
    public partial class SettingsFisherbotForm : DevComponents.DotNetBar.Metro.MetroForm
    {
        public SettingsFisherbotForm()
        {
            InitializeComponent();
            translate();
            fishSchoolProfil.DropDownStyle = ComboBoxStyle.DropDownList;
            if (nManagerSetting.CurrentSetting.ActivateAlwaysOnTopFeature)
                this.TopMost = true;
            Load();
        }

        private void translate()
        {
            labelX1.Text = Translate.Get(Translate.Id.Use_Lure);
            labelX2.Text = Translate.Get(Translate.Id.Fish_School);
            labelX3.Text = Translate.Get(Translate.Id.Lure_Name) + "*";
            labelX4.Text = Translate.Get(Translate.Id.Fishing_Pole_Name);
            labelX5.Text = Translate.Get(Translate.Id.Weapon_Name);
            labelX6.Text = "* = " + Translate.Get(Translate.Id.If_special__If_empty__default_items_is_used);
            saveB.Text = Translate.Get(Translate.Id.Save_and_Close);
            profileCreatorB.Text = Translate.Get(Translate.Id.Profile_Creator);
            labelX7.Text = Translate.Get(Translate.Id.Precision_Mode__fish_school);
            Text = Translate.Get(Translate.Id.Settings_Fisherbot);
        }

        private void saveB_Click(object sender, System.EventArgs e)
        {
            Save();
        }

        private void Save()
        {
            FisherbotSetting.CurrentSetting.useLure = useLure.Value;
            FisherbotSetting.CurrentSetting.fishSchool = fishSchool.Value;
            FisherbotSetting.CurrentSetting.fishSchoolProfil = fishSchoolProfil.Text;
            FisherbotSetting.CurrentSetting.lureName = lureName.Text;
            FisherbotSetting.CurrentSetting.FishingPoleName = FisherbotPoolName.Text;
            FisherbotSetting.CurrentSetting.weaponName = weaponName.Text;
            FisherbotSetting.CurrentSetting.precisionMode = precisionMode.Value;
            FisherbotSetting.CurrentSetting.Save();
            Dispose();
        }

        private new void Load()
        {
            RefreshProfilesList();

            useLure.Value = FisherbotSetting.CurrentSetting.useLure;
            fishSchool.Value = FisherbotSetting.CurrentSetting.fishSchool;
            fishSchoolProfil.Text = FisherbotSetting.CurrentSetting.fishSchoolProfil;
            lureName.Text = FisherbotSetting.CurrentSetting.lureName;
            FisherbotPoolName.Text = FisherbotSetting.CurrentSetting.FishingPoleName;
            weaponName.Text = FisherbotSetting.CurrentSetting.weaponName;
            precisionMode.Value = FisherbotSetting.CurrentSetting.precisionMode;
        }

        private void RefreshProfilesList()
        {
            fishSchoolProfil.Items.Clear();
            foreach (var f in Others.GetFilesDirectory(Application.StartupPath + "\\Profiles\\Fisherbot\\", "*.xml"))
            {
                fishSchoolProfil.Items.Add(f);
            }
        }

        private void profileCreatorB_Click(object sender, System.EventArgs e)
        {
            var f = new Profile.ProfileCreator();
            f.ShowDialog();
            RefreshProfilesList();
        }
    }
}