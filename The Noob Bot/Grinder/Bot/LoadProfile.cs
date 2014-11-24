using System;
using System.Windows.Forms;
using Grinder.Profile;
using nManager;
using nManager.Helpful;

namespace Grinder.Bot
{
    public partial class LoadProfile : Form
    {
        public LoadProfile()
        {
            try
            {
                InitializeComponent();
                Translate();
                // Complete List Profiles
                RefreshProfileList();
                ProfileList.DropDownStyle = ComboBoxStyle.DropDownList;
                ProfileList.Text = GrinderSetting.CurrentSetting.ProfileName;
                if (nManagerSetting.CurrentSetting.ActivateAlwaysOnTopFeature)
                    TopMost = true;
            }
            catch (Exception e)
            {
                Logging.WriteError("Grinder > Bot > LoadProfile > LoadProfile(): " + e);
            }
        }

        private void Translate()
        {
            LoadProfileButton.Text = nManager.Translate.Get(nManager.Translate.Id.Load_Profile);
            SelectProfileLabel.Text = nManager.Translate.Get(nManager.Translate.Id.Profile) + ":";
            ProfileCreatorButton.Text = nManager.Translate.Get(nManager.Translate.Id.Profile_Creator);
            MainHeader.TitleText = nManager.Translate.Get(nManager.Translate.Id.Load_Profile) + " Grinder";
            this.Text = MainHeader.TitleText;
        }

        private void RefreshProfileList()
        {
            try
            {
                string profileName = ProfileList.Text;
                ProfileList.Items.Clear();
                foreach (string f in Others.GetFilesDirectory(Application.StartupPath + "\\Profiles\\Grinder\\", "*.xml"))
                {
                    ProfileList.Items.Add(f);
                }
                ProfileList.Text = profileName;
            }
            catch (Exception e)
            {
                Logging.WriteError("Grinder > Bot > LoadProfile > RefreshProfileList(): " + e);
            }
        }

        private void loadProfileB_Click(object sender, EventArgs ex)
        {
            try
            {
                GrinderSetting.CurrentSetting.ProfileName = ProfileList.Text;
                GrinderSetting.CurrentSetting.Save();
                Dispose();
            }
            catch (Exception e)
            {
                Logging.WriteError("Grinder > Bot > LoadProfile > loadProfileB_Click(object sender, EventArgs ex): " + e);
            }
        }

        private void createProfileB_Click(object sender, EventArgs ex)
        {
            try
            {
                var f = new ProfileCreator();
                f.ShowDialog();
                RefreshProfileList();
            }
            catch (Exception e)
            {
                Logging.WriteError("Grinder > Bot > LoadProfile > createProfileB_Click(object sender, EventArgs ex): " +
                                   e);
            }
        }

        private void LoadProfile_FormClosing(object sender, FormClosingEventArgs ex)
        {
            try
            {
                if (ex.CloseReason == CloseReason.UserClosing)
                {
                    GrinderSetting.CurrentSetting.ProfileName = "";
                }
            }
            catch (Exception e)
            {
                Logging.WriteError(
                    "Grinder > Bot > LoadProfile > LoadProfile_FormClosing(object sender, FormClosingEventArgs e): " + e);
            }
        }
    }
}