using System;
using System.Windows.Forms;
using nManager.Helpful;

namespace Gatherer.Bot
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
                listProfileCb.Text = GathererSetting.CurrentSetting.profileName;
            }
            catch (Exception e)
            {
                Logging.WriteError("Gatherer > Bot > LoadProfile > LoadProfile(): " + e);
            }
        }

        private void Translate()
        {
            loadProfileB.Text = nManager.Translate.Get(nManager.Translate.Id.Load_Profile);
            labelX1.Text = nManager.Translate.Get(nManager.Translate.Id.Profile) + ":";
            createProfileB.Text = nManager.Translate.Get(nManager.Translate.Id.Profile_Creator);
            Text = nManager.Translate.Get(nManager.Translate.Id.Load_Profile) + " Gatherer";
        }

        private void RefreshProfileList()
        {
            try
            {
                var profileName = listProfileCb.Text;
                listProfileCb.Items.Clear();
                foreach (var f in Others.GetFilesDirectory(Application.StartupPath + "\\Profiles\\Gatherer\\", "*.xml"))
                {
                    listProfileCb.Items.Add(f);
                }
                listProfileCb.Text = profileName;
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
                GathererSetting.CurrentSetting.profileName = listProfileCb.Text;
                GathererSetting.CurrentSetting.Save();
                Dispose();
            }
            catch (Exception e)
            {
                Logging.WriteError("Gatherer > Bot > LoadProfile > loadProfileB_Click(object sender, EventArgs ex): " +
                                   e);
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
                Logging.WriteError(
                    "Gatherer > Bot > LoadProfile > createProfileB_Click(object sender, EventArgs ex): " + e);
            }
        }

        private void LoadProfile_FormClosing(object sender, FormClosingEventArgs ex)
        {
            try
            {
                if (ex.CloseReason == CloseReason.UserClosing)
                {
                    GathererSetting.CurrentSetting.profileName = "";
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