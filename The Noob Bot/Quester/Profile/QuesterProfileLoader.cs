using System;
using System.Windows.Forms;
using nManager;
using nManager.Helpful;
using Quester.Bot;
using Quester.Properties;

namespace Quester.Profile
{
    public partial class QuesterProfileLoader : Form
    {
        public QuesterProfileLoader()
        {
            InitializeComponent();
            Translate();
            ShowQuesterProfileLoaderForm(true);
            TopMost = true;
        }

        private void Translate()
        {
            MainHeader.TitleText = nManager.Translate.Get(nManager.Translate.Id.QuesterProfileLoader) + " - " + Information.MainTitle;
            SimpleProfilesListLabel.Text = nManager.Translate.Get(nManager.Translate.Id.SimpleProfilesList).ToUpper();
            GroupedProfilesListLabel.Text = nManager.Translate.Get(nManager.Translate.Id.GroupedProfilesList).ToUpper();
            LoadSimpleProfile.Text = nManager.Translate.Get(nManager.Translate.Id.LoadSimpleProfile);
            LoadGroupedProfile.Text = nManager.Translate.Get(nManager.Translate.Id.LoadGroupedProfile);
            QuesterProfileManagementSystemButton.Text = nManager.Translate.Get(nManager.Translate.Id.QuesterProfileManagementSystem);
        }

        private void ShowQuesterProfileLoaderForm(bool load = false)
        {
            RefreshGroupedProfileList(load);
            RefreshSimpleProfileList(load);
        }

        private void RefreshGroupedProfileList(bool load = false)
        {
            try
            {
                int currentSelection = GroupedProfilesList.SelectedIndex;
                GroupedProfilesList.Items.Clear();
                foreach (string f in Others.GetFilesDirectory(Application.StartupPath + "\\Profiles\\Quester\\Grouped\\", "*.xml"))
                {
                    GroupedProfilesList.Items.Add(f);
                    if (load && !QuesterSettings.CurrentSettings.LastProfileSimple && QuesterSettings.CurrentSettings.LastProfile == f)
                        currentSelection = SimpleProfilesList.Items.Count - 1;
                }
                if (load && GroupedProfilesList.Items.Count > 0 && QuesterSettings.CurrentSettings.LastProfile == "")
                    GroupedProfilesList.SelectedIndex = 0;
                else if (GroupedProfilesList.Items.Count > 0 && currentSelection <= GroupedProfilesList.Items.Count - 1)
                    GroupedProfilesList.SelectedIndex = currentSelection;
                if (GroupedProfilesList.Items.Count > 0 && GroupedProfilesList.SelectedIndex == -1)
                    GroupedProfilesList.SelectedIndex = 0;
            }
            catch (Exception e)
            {
                Logging.WriteError("Quester > Profile > QuesterProfileLoader > RefreshGroupedProfileList(): " + e);
            }
        }

        private void RefreshSimpleProfileList(bool load = false)
        {
            try
            {
                int currentSelection = SimpleProfilesList.SelectedIndex;
                SimpleProfilesList.Items.Clear();
                foreach (string f in Others.GetFilesDirectory(Application.StartupPath + "\\Profiles\\Quester\\", "*.xml"))
                {
                    SimpleProfilesList.Items.Add(f);
                    if (load && QuesterSettings.CurrentSettings.LastProfileSimple && QuesterSettings.CurrentSettings.LastProfile == f)
                        currentSelection = SimpleProfilesList.Items.Count - 1;
                }
                if (load && SimpleProfilesList.Items.Count > 0 && QuesterSettings.CurrentSettings.LastProfile == "")
                    SimpleProfilesList.SelectedIndex = 0;
                else if (SimpleProfilesList.Items.Count > 0 && currentSelection <= SimpleProfilesList.Items.Count - 1)
                    SimpleProfilesList.SelectedIndex = currentSelection;
                if (SimpleProfilesList.Items.Count > 0 && SimpleProfilesList.SelectedIndex == -1)
                    SimpleProfilesList.SelectedIndex = 0;
            }
            catch (Exception e)
            {
                Logging.WriteError("Quester > Profile > QuesterProfileLoader > RefreshSimpleProfileList(): " + e);
            }
        }

        private void LoadGroupedProfile_MouseEnter(object sender, EventArgs e)
        {
            LoadGroupedProfile.Image = Resources.greenB_242;
        }

        private void LoadGroupedProfile_MouseLeave(object sender, EventArgs e)
        {
            LoadGroupedProfile.Image = Resources.blueB_242;
        }

        private void QuesterProfileManagementSystemButton_MouseEnter(object sender, EventArgs e)
        {
            QuesterProfileManagementSystemButton.Image = Resources.greenB_260;
        }

        private void QuesterProfileManagementSystemButton_MouseLeave(object sender, EventArgs e)
        {
            QuesterProfileManagementSystemButton.Image = Resources.blueB_260;
        }

        private void DoLoadSimpleProfile(object sender, EventArgs e)
        {
            if (SimpleProfilesList.Items.Count > 0)
            {
                QuesterSettings.CurrentSettings.LastProfile = SimpleProfilesList.Items[SimpleProfilesList.SelectedIndex].ToString();
                QuesterSettings.CurrentSettings.LastProfileSimple = true;
                QuesterSettings.CurrentSettings.Save();
                Dispose();
            }
            else
            {
                MessageBox.Show(nManager.Translate.Get(nManager.Translate.Id.NoSimpleProfileToLoad));
            }
        }

        private void DoLoadGroupedProfile(object sender, EventArgs e)
        {
            if (GroupedProfilesList.Items.Count > 0)
            {
                QuesterSettings.CurrentSettings.LastProfile = GroupedProfilesList.Items[GroupedProfilesList.SelectedIndex].ToString();
                QuesterSettings.CurrentSettings.LastProfileSimple = false;
                QuesterSettings.CurrentSettings.Save();
                Dispose();
            }
            else
            {
                MessageBox.Show(nManager.Translate.Get(nManager.Translate.Id.NoGroupedProfileToLoad));
            }
        }

        private void QuesterProfileManagementSystemButton_Click(object sender, EventArgs e)
        {
            var f = new ProfileManager();
            f.ShowDialog();
            ShowQuesterProfileLoaderForm();
        }

        private void QuesterProfileLoader_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (e.CloseReason != CloseReason.UserClosing) return;
                QuesterSettings.CurrentSettings.LastProfile = "";
                QuesterSettings.CurrentSettings.LastProfileSimple = true;
            }
            catch (Exception ec)
            {
                Logging.WriteError(
                    "Quester > Profile > QuesterProfileLoader > QuesterProfileLoader_FormClosing(object sender, FormClosingEventArgs e): " + ec);
            }
        }

        private void LoadSimpleProfile_MouseEnter(object sender, EventArgs e)
        {
            LoadSimpleProfile.Image = Resources.greenB_242;
        }

        private void LoadSimpleProfile_MouseLeave(object sender, EventArgs e)
        {
            LoadSimpleProfile.Image = Resources.blueB_242;
        }
    }
}