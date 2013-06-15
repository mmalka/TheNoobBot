using System;
using System.IO;
using System.Windows.Forms;
using DevComponents.DotNetBar.Metro;
using nManager.Helpful;

namespace Quester.Profile
{
    public partial class ProfileManager : MetroForm
    {
        public ProfileManager()
        {
            try
            {
                InitializeComponent();
                Translate();
                RefreshProfileManagerForm();
                TopMost = true;
            }
            catch (Exception e)
            {
                Logging.WriteError("Quester > Profile > ProfileManager > ProfileManager(): " + e);
            }
        }

        private void Translate()
        {
            this.Text = nManager.Translate.Get(nManager.Translate.Id.QuesterProfileManagementSystem); // Form Title

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

        private void RefreshGroupedProfileList(bool Load = false)
        {
            try
            {
                int CurrentSelection = ExistingGroupedProfiles.SelectedIndex;
                ExistingGroupedProfiles.Items.Clear();
                foreach (string f in Others.GetFilesDirectory(Application.StartupPath + "\\Profiles\\Quester\\Grouped\\", "*.xml"))
                {
                    ExistingGroupedProfiles.Items.Add(f);
                }
                if (Load && ExistingGroupedProfiles.Items.Count > 0)
                    ExistingGroupedProfiles.SelectedIndex = 0;
                else if (CurrentSelection <= ExistingGroupedProfiles.Items.Count - 1 && ExistingGroupedProfiles.Items.Count > 0)
                    ExistingGroupedProfiles.SelectedIndex = CurrentSelection;
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

        private void RefreshSimpleProfileList(bool Load = false)
        {
            try
            {
                int CurrentSelection = ExistingSimpleProfiles.SelectedIndex;
                ExistingSimpleProfiles.Items.Clear();
                foreach (string f in Others.GetFilesDirectory(Application.StartupPath + "\\Profiles\\Quester\\", "*.xml"))
                {
                    ExistingSimpleProfiles.Items.Add(f);
                }
                if (Load && ExistingSimpleProfiles.Items.Count > 0)
                    ExistingSimpleProfiles.SelectedIndex = 0;
                else if (CurrentSelection <= ExistingSimpleProfiles.Items.Count - 1 && ExistingSimpleProfiles.Items.Count > 0)
                    ExistingSimpleProfiles.SelectedIndex = CurrentSelection;
                else if (ExistingSimpleProfiles.Items.Count > 0)
                    ExistingSimpleProfiles.SelectedIndex = 0;
            }
            catch (Exception e)
            {
                Logging.WriteError("Quester > Profile > ProfileManager > RefreshSimpleProfileList(): " + e);
            }
        }

        private void ProfileManagerAddGrouped_Click(object sender, EventArgs e)
        {
            GroupedProfileManager f = new GroupedProfileManager();
            f.ShowDialog();
            RefreshGroupedProfileList();
        }

        private void DoProfileManagerEditGrouped(object sender, EventArgs e)
        {
            if (ExistingGroupedProfiles.Items.Count > 0)
            {
                GroupedProfileManager f = new GroupedProfileManager(ExistingGroupedProfiles.Items[ExistingGroupedProfiles.SelectedIndex].ToString());
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
                else
                {
                    MessageBox.Show(nManager.Translate.Get(nManager.Translate.Id.NoGroupedProfileToDelete));
                }
            }
            else
            {
                MessageBox.Show(nManager.Translate.Get(nManager.Translate.Id.NoGroupedProfileToDelete));
            }
            RefreshGroupedProfileList();
        }

        private void ProfileManagerAdd_Click(object sender, EventArgs e)
        {
            MessageBox.Show(nManager.Translate.Get(nManager.Translate.Id.FeatureNotYetAvailable));
            RefreshProfileManagerForm();
        }

        private void ProfileManageEdit_Click(object sender, EventArgs e)
        {
            MessageBox.Show(nManager.Translate.Get(nManager.Translate.Id.FeatureNotYetAvailable));
            RefreshProfileManagerForm();
        }

        private void ProfileManagerSimpleDocumentation_Click(object sender, EventArgs e)
        {
            Others.OpenWebBrowserOrApplication("http://thenoobbot.com/community/viewtopic.php?f=165&t=5986");
        }

        private void ProfileManagerRemove_Click(object sender, EventArgs e)
        {
            MessageBox.Show(nManager.Translate.Get(nManager.Translate.Id.FeatureNotYetAvailable));
            RefreshProfileManagerForm();
        }
    }
}