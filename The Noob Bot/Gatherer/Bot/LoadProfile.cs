using System;
using System.Windows.Forms;
using nManager;
using nManager.Helpful;

namespace Gatherer.Bot
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
                TopMost = true;
                ProfileList.DropDownStyle = ComboBoxStyle.DropDownList;
                ProfileList.Text = GathererSetting.CurrentSetting.ProfileName;
                if (nManagerSetting.CurrentSetting.ActivateAlwaysOnTopFeature)
                    TopMost = true;
            }
            catch (Exception e)
            {
                Logging.WriteError("Gatherer > Bot > LoadProfile > LoadProfile(): " + e);
            }
        }

        private void Translate()
        {
            LoadProfileButton.Text = nManager.Translate.Get(nManager.Translate.Id.Load_Profile);
            SelectProfileLabel.Text = nManager.Translate.Get(nManager.Translate.Id.Profile) + ":";
            ProfileCreatorButton.Text = nManager.Translate.Get(nManager.Translate.Id.Profile_Creator);
            MainHeader.TitleText = nManager.Translate.Get(nManager.Translate.Id.Load_Profile) + " Gatherer";
        }

        private void RefreshProfileList()
        {
            try
            {
                string profileName = ProfileList.Text;
                ProfileList.Items.Clear();
                foreach (string f in Others.GetFilesDirectory(Application.StartupPath + "\\Profiles\\Gatherer\\", "*.xml"))
                {
                    ProfileList.Items.Add(f);
                }
                ProfileList.Text = profileName;
            }
            catch (Exception e)
            {
                Logging.WriteError("Gatherer > Bot > LoadProfile > RefreshProfileList(): " + e);
            }
        }

        private void loadProfileB_Click(object sender, EventArgs ex)
        {
            try
            {
                GathererSetting.CurrentSetting.ProfileName = ProfileList.Text;
                GathererSetting.CurrentSetting.Save();
                Dispose();
            }
            catch (Exception e)
            {
                Logging.WriteError("Gatherer > Bot > LoadProfile > loadProfileB_Click(object sender, EventArgs ex): " +
                                   e);
            }
        }

        private void ProfileCreator_Click(object sender, EventArgs ex)
        {
            try
            {
                var f = new ProfileCreator();
                f.ShowDialog();
                RefreshProfileList();
            }
            catch (Exception e)
            {
                Logging.WriteError(
                    "Gatherer > Bot > LoadProfile > ProfileCreator_Click(object sender, EventArgs ex): " + e);
            }
        }

        private void LoadProfile_FormClosing(object sender, FormClosingEventArgs ex)
        {
            try
            {
                if (ex.CloseReason == CloseReason.UserClosing)
                {
                    GathererSetting.CurrentSetting.ProfileName = "";
                }
            }
            catch (Exception e)
            {
                Logging.WriteError(
                    "Gatherer > Bot > LoadProfile > LoadProfile_FormClosing(object sender, FormClosingEventArgs e): " +
                    e);
            }
        }
    }
}