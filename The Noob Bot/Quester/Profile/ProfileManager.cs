using System;
using System.IO;
using System.Windows.Forms;
using nManager;
using nManager.Helpful;

namespace Quester.Profile
{
    public partial class ProfileManager : Form
    {
        public ProfileManager()
        {
            try
            {
                InitializeComponent();
                Translate();
                RefreshProfileManagerForm();
                if (nManagerSetting.CurrentSetting.ActivateAlwaysOnTopFeature)
                    TopMost = true;
            }
            catch (Exception e)
            {
                Logging.WriteError("Quester > Profile > ProfileManager > ProfileManager(): " + e);
            }
        }

        private void Translate()
        {
            MainHeader.TitleText = nManager.Translate.Get(nManager.Translate.Id.QuesterProfileManagementSystem) + Information.MainTitle;

            ProfileManagerGroupedLabel.Text = nManager.Translate.Get(nManager.Translate.Id.GroupedProfileManager);
            ProfileManagerSimpleLabel.Text = nManager.Translate.Get(nManager.Translate.Id.SimpleProfileManager);
            ProfileManagerAddGrouped.Text = nManager.Translate.Get(nManager.Translate.Id.AddGrouped);
            ProfileManagerEditGrouped.Text = nManager.Translate.Get(nManager.Translate.Id.EditGrouped);
            ProfileManagerGroupedDocumentation.Text = nManager.Translate.Get(nManager.Translate.Id.GroupedDocumentation);
            ProfileManagerRemoveGrouped.Text = nManager.Translate.Get(nManager.Translate.Id.RemoveGrouped);
            ProfileManagerAdd.Text = nManager.Translate.Get(nManager.Translate.Id.AddSimple);
            ProfileManagerEdit.Text = nManager.Translate.Get(nManager.Translate.Id.EditSimple);
            ProfileManagerSimpleDocumentation.Text = nManager.Translate.Get(nManager.Translate.Id.SimpleDocumentation);
            ProfileManagerRemove.Text = nManager.Translate.Get(nManager.Translate.Id.RemoveSimple);
        }

        private void RefreshProfileManagerForm()
        {
            RefreshGroupedProfileList(true);
            RefreshSimpleProfileList();
        }

        private void RefreshGroupedProfileList(bool load = false)
        {
            try
            {
                int currentSelection = ExistingGroupedProfiles.SelectedIndex;
                ExistingGroupedProfiles.Items.Clear();
                foreach (string f in Others.GetFilesDirectory(Application.StartupPath + "\\Profiles\\Quester\\Grouped\\", "*.xml"))
                {
                    ExistingGroupedProfiles.Items.Add(f);
                }
                if (load && ExistingGroupedProfiles.Items.Count > 0)
                    ExistingGroupedProfiles.SelectedIndex = 0;
                else if (currentSelection <= ExistingGroupedProfiles.Items.Count - 1 && ExistingGroupedProfiles.Items.Count > 0)
                    ExistingGroupedProfiles.SelectedIndex = currentSelection;
                else if (ExistingGroupedProfiles.Items.Count > 0)
                    ExistingGroupedProfiles.SelectedIndex = 0;
                if (ExistingGroupedProfiles.Items.Count > 0)
                {
                    ProfileManagerEditGrouped.Show();
                    ProfileManagerRemoveGrouped.Show();
                }
                else
                {
                    ProfileManagerEditGrouped.Hide();
                    ProfileManagerRemoveGrouped.Hide();
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("Quester > Profile > ProfileManager > RefreshGroupedProfileList(): " + e);
            }
        }

        private void RefreshSimpleProfileList(bool load = false)
        {
            try
            {
                int currentSelection = ExistingSimpleProfiles.SelectedIndex;
                ExistingSimpleProfiles.Items.Clear();
                foreach (string f in Others.GetFilesDirectory(Application.StartupPath + "\\Profiles\\Quester\\", "*.xml"))
                {
                    ExistingSimpleProfiles.Items.Add(f);
                }
                if (load && ExistingSimpleProfiles.Items.Count > 0)
                    ExistingSimpleProfiles.SelectedIndex = 0;
                else if (currentSelection <= ExistingSimpleProfiles.Items.Count - 1 && ExistingSimpleProfiles.Items.Count > 0)
                    ExistingSimpleProfiles.SelectedIndex = currentSelection;
                else if (ExistingSimpleProfiles.Items.Count > 0)
                    ExistingSimpleProfiles.SelectedIndex = 0;
                if (ExistingSimpleProfiles.Items.Count > 0)
                {
                    ProfileManagerEdit.Show();
                    ProfileManagerRemove.Show();
                }
                else
                {
                    ProfileManagerEdit.Hide();
                    ProfileManagerRemove.Hide();
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("Quester > Profile > ProfileManager > RefreshSimpleProfileList(): " + e);
            }
        }

        private void ProfileManagerAddGrouped_Click(object sender, EventArgs e)
        {
            var f = new GroupedProfileManager();
            f.ShowDialog();
            RefreshGroupedProfileList();
        }

        private void DoProfileManagerEditGrouped(object sender, EventArgs e)
        {
            if (ExistingGroupedProfiles.Items.Count > 0)
            {
                var f = new GroupedProfileManager(ExistingGroupedProfiles.Items[ExistingGroupedProfiles.SelectedIndex].ToString());
                f.ShowDialog();
            }
            else
                MessageBox.Show(nManager.Translate.Get(nManager.Translate.Id.NoGroupedProfileToEdit));
            RefreshGroupedProfileList();
        }

        private void ProfileManagerGroupedDocumentation_Click(object sender, EventArgs e)
        {
            Others.OpenWebBrowserOrApplication("http://thenoobbot.com/community/viewtopic.php?f=165&t=5983");
        }

        private void ProfileManagerRemoveGrouped_Click(object sender, EventArgs e)
        {
            if (ExistingGroupedProfiles.Items.Count > 0)
            {
                string path = ExistingGroupedProfiles.Items[ExistingGroupedProfiles.SelectedIndex].ToString();
                string fullpath = Application.StartupPath + "\\Profiles\\Quester\\Grouped\\" + path;
                if (!string.IsNullOrWhiteSpace(path) && File.Exists(fullpath))
                {
                    DialogResult check = MessageBox.Show(string.Format("{0}", nManager.Translate.Get(nManager.Translate.Id.RemoveGroupedProfile) + path + " ?"),
                        nManager.Translate.Get(nManager.Translate.Id.Confirm), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (check == DialogResult.Yes)
                        File.Delete(fullpath);
                }
            }
            RefreshGroupedProfileList();
        }

        private void ProfileManagerAdd_Click(object sender, EventArgs e)
        {
            var f = new SimpleProfileManager();
            f.ShowDialog();
            RefreshProfileManagerForm();
        }

        private void DoProfileManagerEditSimple(object sender, EventArgs e)
        {
            if (ExistingSimpleProfiles.Items.Count > 0)
            {
                var f = new SimpleProfileManager(ExistingSimpleProfiles.Items[ExistingSimpleProfiles.SelectedIndex].ToString());
                f.ShowDialog();
            }
            else
                MessageBox.Show(nManager.Translate.Get(nManager.Translate.Id.NoSimpleProfileToEdit));
            RefreshProfileManagerForm();
        }

        private void ProfileManagerSimpleDocumentation_Click(object sender, EventArgs e)
        {
            Others.OpenWebBrowserOrApplication("http://thenoobbot.com/community/viewtopic.php?f=165&t=5986");
        }

        private void ProfileManagerRemove_Click(object sender, EventArgs e)
        {
            if (ExistingSimpleProfiles.Items.Count > 0)
            {
                string path = ExistingSimpleProfiles.Items[ExistingSimpleProfiles.SelectedIndex].ToString();
                string fullpath = Application.StartupPath + "\\Profiles\\Quester\\" + path;
                if (!string.IsNullOrWhiteSpace(path) && File.Exists(fullpath))
                {
                    DialogResult check = MessageBox.Show(string.Format("{0}", nManager.Translate.Get(nManager.Translate.Id.RemoveSimple) + path + " ?"),
                        nManager.Translate.Get(nManager.Translate.Id.Confirm), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (check == DialogResult.Yes)
                        File.Delete(fullpath);
                }
            }
            RefreshProfileManagerForm();
        }
    }
}