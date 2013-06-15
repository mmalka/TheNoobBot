using System;
using System.Windows.Forms;
using Grinder.Profile;
using nManager.Helpful;

namespace Grinder.Bot
{
    public partial class LoadProfile : DevComponents.DotNetBar.Metro.MetroForm
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
                listProfileCb.DropDownStyle = ComboBoxStyle.DropDownList;
                listProfileCb.Text = GrinderSetting.CurrentSetting.ProfileName;
                if (nManager.nManagerSetting.CurrentSetting.ActivateAlwaysOnTopFeature)
                    this.TopMost = true;
            }
            catch (Exception e)
            {
                Logging.WriteError("Grinder > Bot > LoadProfile > LoadProfile(): " + e);
            }
        }

        private void Translate()
        {
            loadProfileB.Text = nManager.Translate.Get(nManager.Translate.Id.Load_Profile);
            labelX1.Text = nManager.Translate.Get(nManager.Translate.Id.Profile) + ":";
            createProfileB.Text = nManager.Translate.Get(nManager.Translate.Id.Profile_Creator);
            Text = nManager.Translate.Get(nManager.Translate.Id.Load_Profile) + " Grinder";
        }

        private void RefreshProfileList()
        {
            try
            {
                string profileName = listProfileCb.Text;
                listProfileCb.Items.Clear();
                foreach (string f in Others.GetFilesDirectory(Application.StartupPath + "\\Profiles\\Grinder\\", "*.xml"))
                {
                    listProfileCb.Items.Add(f);
                }
                listProfileCb.Text = profileName;
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
                GrinderSetting.CurrentSetting.ProfileName = listProfileCb.Text;
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
                ProfileCreator f = new ProfileCreator();
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