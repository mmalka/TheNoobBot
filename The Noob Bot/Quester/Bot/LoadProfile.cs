using System;
using System.Windows.Forms;
using Quester.Profile;
using nManager.Helpful;

namespace Quester.Bot
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
                listProfileCb.Text = QuesterSetting.CurrentSetting.profileName;
                if (nManager.nManagerSetting.CurrentSetting.ActivateAlwaysOnTopFeature)
                    this.TopMost = true;
            }
            catch (Exception e)
            {
                Logging.WriteError("Quester > Bot > LoadProfile > LoadProfile(): " + e);
            }
        }

        private void Translate()
        {
            loadProfileB.Text = nManager.Translate.Get(nManager.Translate.Id.Load_Profile);
            labelX1.Text = nManager.Translate.Get(nManager.Translate.Id.Profile) + ":";
            createProfileB.Text = nManager.Translate.Get(nManager.Translate.Id.Profile_Creator);
            Text = nManager.Translate.Get(nManager.Translate.Id.Load_Profile) + " Quester";
        }

        private void RefreshProfileList()
        {
            try
            {
                var profileName = listProfileCb.Text;
                listProfileCb.Items.Clear();
                foreach (var f in Others.GetFilesDirectory(Application.StartupPath + "\\Profiles\\Quester\\", "*.xml"))
                {
                    listProfileCb.Items.Add(f);
                }
                listProfileCb.Text = profileName;
            }
            catch (Exception e)
            {
                Logging.WriteError("Quester > Bot > LoadProfile > RefreshProfileList(): " + e);
            }
        }

        private void loadProfileB_Click(object sender, EventArgs ex)
        {
            try
            {
                QuesterSetting.CurrentSetting.profileName = listProfileCb.Text;
                QuesterSetting.CurrentSetting.Save();
                Dispose();
            }
            catch (Exception e)
            {
                Logging.WriteError("Quester > Bot > LoadProfile > loadProfileB_Click(object sender, EventArgs ex): " + e);
            }
        }

        private void createProfileB_Click(object sender, EventArgs ex)
        {
            try
            {
                var f = new ProfileManager();
                f.ShowDialog();
                RefreshProfileList();
            }
            catch (Exception e)
            {
                Logging.WriteError("Quester > Bot > LoadProfile > createProfileB_Click(object sender, EventArgs ex): " +
                                   e);
            }
        }

        private void LoadProfile_FormClosing(object sender, FormClosingEventArgs ex)
        {
            try
            {
                if (ex.CloseReason == CloseReason.UserClosing)
                {
                    QuesterSetting.CurrentSetting.profileName = "";
                }
            }
            catch (Exception e)
            {
                Logging.WriteError(
                    "Quester > Bot > LoadProfile > LoadProfile_FormClosing(object sender, FormClosingEventArgs e): " + e);
            }
        }
    }
}