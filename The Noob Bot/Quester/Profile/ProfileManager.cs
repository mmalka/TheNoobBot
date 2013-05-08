using System;
using System.Windows.Forms;
using DevComponents.DotNetBar.Metro;
using nManager;
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
            /*
             * this.Text = nManager.Translate.Get(nManager.Translate.Id.none); // Form Title
             * 
             * ProfileManagerGroupedLabel.Text = nManager.Translate.Get(nManager.Translate.Id.none);
             * ProfileManagerSimpleLabel.Text = nManager.Translate.Get(nManager.Translate.Id.none);
             * ProfileManagerAddGrouped.Text = nManager.Translate.Get(nManager.Translate.Id.none);
             * ProfileManagerEditGrouped.Text = nManager.Translate.Get(nManager.Translate.Id.none);
             * ProfileManagerGroupedDocumentation.Text = nManager.Translate.Get(nManager.Translate.Id.none);
             * ProfileManagerRemoveGrouped.Text = nManager.Translate.Get(nManager.Translate.Id.none);
             * ProfileManagerAdd.Text = nManager.Translate.Get(nManager.Translate.Id.none);
             * ProfileManagerEdit.Text = nManager.Translate.Get(nManager.Translate.Id.none);
             * ProfileManagerSimpleDocumentation.Text = nManager.Translate.Get(nManager.Translate.Id.none);
             * ProfileManagerRemove.Text = nManager.Translate.Get(nManager.Translate.Id.none);
             */
        }

        private void RefreshProfileManagerForm()
        {
            RefreshGroupedProfileList();
            RefreshSimpleProfileList();
        }

        private void RefreshGroupedProfileList()
        {
            try
            {
                int CurrentSelection = ExistingGroupedProfiles.SelectedIndex;
                ExistingGroupedProfiles.Items.Clear();
                foreach (string f in Others.GetFilesDirectory(Application.StartupPath + "\\Profiles\\Quester\\Grouped\\", "*.xml"))
                {
                    ExistingGroupedProfiles.Items.Add(f);
                }
                if (CurrentSelection > ExistingGroupedProfiles.Items.Count - 1)
                    CurrentSelection = ExistingGroupedProfiles.Items.Count - 1;
                ExistingGroupedProfiles.SelectedIndex = CurrentSelection;
            }
            catch (Exception e)
            {
                Logging.WriteError("Quester > Profile > ProfileManager > RefreshGroupedProfileList(): " + e);
            }
        }

        private void RefreshSimpleProfileList()
        {
            try
            {
                int CurrentSelection = ExistingSimpleProfiles.SelectedIndex;
                ExistingSimpleProfiles.Items.Clear();
                foreach (string f in Others.GetFilesDirectory(Application.StartupPath + "\\Profiles\\Quester\\", "*.xml"))
                {
                    ExistingSimpleProfiles.Items.Add(f);
                }
                if (CurrentSelection > ExistingSimpleProfiles.Items.Count - 1)
                    CurrentSelection = ExistingSimpleProfiles.Items.Count - 1;
                ExistingSimpleProfiles.SelectedIndex = CurrentSelection;
            }
            catch (Exception e)
            {
                Logging.WriteError("Quester > Profile > ProfileManager > RefreshSimpleProfileList(): " + e);
            }
        }

        private void ProfileManagerAddGrouped_Click(object sender, EventArgs e)
        {
            MessageBox.Show("The Grouped Profile system is not yet available, but you can read the Documentation.");
            RefreshProfileManagerForm();
        }

        private void ProfileManagerEditGrouped_Click(object sender, EventArgs e)
        {
            MessageBox.Show("The Grouped Profile system is not yet available, but you can read the Documentation.");
            RefreshProfileManagerForm();
        }

        private void ProfileManagerGroupedDocumentation_Click(object sender, EventArgs e)
        {
            Others.OpenWebBrowserOrApplication("http://thenoobbot.com/community/viewtopic.php?f=165&t=5983");
        }

        private void ProfileManagerRemoveGrouped_Click(object sender, EventArgs e)
        {
            MessageBox.Show("The Grouped Profile system is not yet available, but you can read the Documentation.");
            RefreshProfileManagerForm();
        }

        private void ProfileManagerAdd_Click(object sender, EventArgs e)
        {
            MessageBox.Show("The Simple Profile system is not yet available, but you can read the Documentation.");
            RefreshProfileManagerForm();
        }

        private void ProfileManageEdit_Click(object sender, EventArgs e)
        {
            MessageBox.Show("The Simple Profile system is not yet available, but you can read the Documentation.");
            RefreshProfileManagerForm();
        }

        private void ProfileManagerSimpleDocumentation_Click(object sender, EventArgs e)
        {
            Others.OpenWebBrowserOrApplication("http://thenoobbot.com/community/viewtopic.php?f=165&t=5986");
        }

        private void ProfileManagerRemove_Click(object sender, EventArgs e)
        {
            MessageBox.Show("The Simple Profile system is not yet available, but you can read the Documentation.");
            RefreshProfileManagerForm();
        }
    }
}