using System;
using System.Windows.Forms;
using DevComponents.DotNetBar.Metro;
using Quester.Bot;
using nManager.Helpful;

namespace Quester.Profile
{
    public partial class LoadQuesterProfile : MetroForm
    {
        public LoadQuesterProfile()
        {
            try
            {
                InitializeComponent();
                Translate();
                ShowLoadQuesterProfileForm(true);
                TopMost = true;
            }
            catch (Exception e)
            {
                Logging.WriteError("Quester > Profile > LoadQuesterProfile > LoadQuesterProfile(): " + e);
            }
        }

        private void Translate()
        {
            Text = nManager.Translate.Get(nManager.Translate.Id.LoadQuesterProfile); // Form Title
            SimpleProfilesListLabel.Text = nManager.Translate.Get(nManager.Translate.Id.SimpleProfilesList);
            GroupedProfileListLabel.Text = nManager.Translate.Get(nManager.Translate.Id.GroupedProfilesList);
            LoadSimpleProfile.Text = nManager.Translate.Get(nManager.Translate.Id.LoadSimpleProfile);
            LoadGroupedProfile.Text = nManager.Translate.Get(nManager.Translate.Id.LoadGroupedProfile);
            QuesterProfileManagementSystemButton.Text = nManager.Translate.Get(nManager.Translate.Id.QuesterProfileManagementSystem);
        }

        private void ShowLoadQuesterProfileForm(bool load = false)
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
                Logging.WriteError("Quester > Profile > LoadQuesterProfile > RefreshGroupedProfileList(): " + e);
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
                Logging.WriteError("Quester > Profile > LoadQuesterProfile > RefreshSimpleProfileList(): " + e);
            }
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
            ShowLoadQuesterProfileForm();
        }

        private void LoadQuesterProfile_FormClosing(object sender, FormClosingEventArgs ex)
        {
            try
            {
                if (ex.CloseReason != CloseReason.UserClosing) return;
                QuesterSettings.CurrentSettings.LastProfile = "";
                QuesterSettings.CurrentSettings.LastProfileSimple = true;
            }
            catch (Exception e)
            {
                Logging.WriteError(
                    "Quester > Profile > LoadQuesterProfile > LoadQuesterProfile_FormClosing(object sender, FormClosingEventArgs e): " + e);
            }
        }
    }
}