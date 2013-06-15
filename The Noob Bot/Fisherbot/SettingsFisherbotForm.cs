using System.Windows.Forms;
using Fisherbot.Bot;
using Fisherbot.Profile;
using nManager;
using nManager.Helpful;

namespace Fisherbot
{
    public partial class SettingsFisherbotForm : DevComponents.DotNetBar.Metro.MetroForm
    {
        public SettingsFisherbotForm()
        {
            InitializeComponent();
            Translate();
            fishSchoolProfil.DropDownStyle = ComboBoxStyle.DropDownList;
            if (nManagerSetting.CurrentSetting.ActivateAlwaysOnTopFeature)
                this.TopMost = true;
            Load();
        }

        private void Translate()
        {
            labelX1.Text = nManager.Translate.Get(nManager.Translate.Id.Use_Lure);
            labelX2.Text = nManager.Translate.Get(nManager.Translate.Id.Fish_School);
            labelX3.Text = nManager.Translate.Get(nManager.Translate.Id.Lure_Name) + "*";
            labelX4.Text = nManager.Translate.Get(nManager.Translate.Id.Fishing_Pole_Name);
            labelX5.Text = nManager.Translate.Get(nManager.Translate.Id.Weapon_Name);
            labelX6.Text = "* = " + nManager.Translate.Get(nManager.Translate.Id.If_special__If_empty__default_items_is_used);
            saveB.Text = nManager.Translate.Get(nManager.Translate.Id.Save_and_Close);
            profileCreatorB.Text = nManager.Translate.Get(nManager.Translate.Id.Profile_Creator);
            labelX7.Text = nManager.Translate.Get(nManager.Translate.Id.Precision_Mode__fish_school);
            Text = nManager.Translate.Get(nManager.Translate.Id.Settings_Fisherbot);
        }

        private void saveB_Click(object sender, System.EventArgs e)
        {
            Save();
        }

        private void Save()
        {
            FisherbotSetting.CurrentSetting.UseLure = useLure.Value;
            FisherbotSetting.CurrentSetting.FishSchool = fishSchool.Value;
            FisherbotSetting.CurrentSetting.FishSchoolProfil = fishSchoolProfil.Text;
            FisherbotSetting.CurrentSetting.LureName = lureName.Text;
            FisherbotSetting.CurrentSetting.FishingPoleName = FisherbotPoolName.Text;
            FisherbotSetting.CurrentSetting.WeaponName = weaponName.Text;
            FisherbotSetting.CurrentSetting.PrecisionMode = precisionMode.Value;
            FisherbotSetting.CurrentSetting.Save();
            Dispose();
        }

        private new void Load()
        {
            RefreshProfilesList();

            useLure.Value = FisherbotSetting.CurrentSetting.UseLure;
            fishSchool.Value = FisherbotSetting.CurrentSetting.FishSchool;
            fishSchoolProfil.Text = FisherbotSetting.CurrentSetting.FishSchoolProfil;
            lureName.Text = FisherbotSetting.CurrentSetting.LureName;
            FisherbotPoolName.Text = FisherbotSetting.CurrentSetting.FishingPoleName;
            weaponName.Text = FisherbotSetting.CurrentSetting.WeaponName;
            precisionMode.Value = FisherbotSetting.CurrentSetting.PrecisionMode;
        }

        private void RefreshProfilesList()
        {
            fishSchoolProfil.Items.Clear();
            foreach (string f in Others.GetFilesDirectory(Application.StartupPath + "\\Profiles\\Fisherbot\\", "*.xml"))
            {
                fishSchoolProfil.Items.Add(f);
            }
        }

        private void profileCreatorB_Click(object sender, System.EventArgs e)
        {
            ProfileCreator f = new Profile.ProfileCreator();
            f.ShowDialog();
            RefreshProfilesList();
        }
    }
}