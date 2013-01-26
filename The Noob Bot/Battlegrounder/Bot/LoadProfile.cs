using System;
using System.Windows.Forms;
using Battlegrounder.Profile;
using nManager.Helpful;
using nManager.Wow.Helpers;

namespace Battlegrounder.Bot
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
                listProfileCb.Text = BattlegrounderSetting.CurrentSetting.profileName;
                if (nManager.nManagerSetting.CurrentSetting.ActivateAlwaysOnTopFeature)
                    this.TopMost = true;
            }
            catch (Exception e)
            {
                Logging.WriteError("Battlegrounder > Bot > LoadProfile > LoadProfile(): " + e);
            }
        }

        private void Translate()
        {
            loadProfileB.Text = nManager.Translate.Get(nManager.Translate.Id.Load_Profile);
            labelX1.Text = nManager.Translate.Get(nManager.Translate.Id.Profile) + ":";
            createProfileB.Text = nManager.Translate.Get(nManager.Translate.Id.Profile_Creator);
            Text = nManager.Translate.Get(nManager.Translate.Id.Load_Profile) + " Battlegrounder";
        }

        private void RefreshProfileList()
        {
            try
            {
                var profileName = listProfileCb.Text;
                listProfileCb.Items.Clear();
                foreach (var f in Others.GetFilesDirectory(Application.StartupPath + "\\Profiles\\Battlegrounder\\", "*.xml"))
                {
                    listProfileCb.Items.Add(f);
                }
                listProfileCb.Text = profileName;
            }
            catch (Exception e)
            {
                Logging.WriteError("Battlegrounder > Bot > LoadProfile > RefreshProfileList(): " + e);
            }
        }

        private void loadProfileB_Click(object sender, EventArgs ex)
        {
            try
            {
                BattlegrounderSetting.CurrentSetting.profileName = listProfileCb.Text;
                BattlegrounderSetting.CurrentSetting.Save();
                Dispose();
            }
            catch (Exception e)
            {
                Logging.WriteError("Battlegrounder > Bot > LoadProfile > loadProfileB_Click(object sender, EventArgs ex): " + e);
            }
        }

        private void createProfileB_Click(object sender, EventArgs ex)
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
            catch (Exception e)
            {
                Logging.WriteError("Battlegrounder > Bot > LoadProfile > createProfileB_Click(object sender, EventArgs ex): " +
                                   e);
            }
        }

        private void LoadProfile_FormClosing(object sender, FormClosingEventArgs ex)
        {
            try
            {
                if (ex.CloseReason == CloseReason.UserClosing)
                {
                    BattlegrounderSetting.CurrentSetting.profileName = "";
                }
            }
            catch (Exception e)
            {
                Logging.WriteError(
                    "Battlegrounder > Bot > LoadProfile > LoadProfile_FormClosing(object sender, FormClosingEventArgs e): " + e);
            }
        }
    }
}