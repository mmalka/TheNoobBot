using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar.Metro;
using nManager.Helpful;

namespace Quester.Profile
{
    public partial class GroupedProfileManager : MetroForm
    {
        private readonly QuesterProfile Profile = new QuesterProfile();
        private readonly QuesterProfile ProfileToSave = new QuesterProfile();
        private readonly string fullpath = "";

        public GroupedProfileManager(string path = "")
        {
            try
            {
                InitializeComponent();
                Translate();
                fullpath = Application.StartupPath + "\\Profiles\\Quester\\Grouped\\" + path;
                if (!string.IsNullOrWhiteSpace(path) && File.Exists(fullpath))
                {
                    Profile = XmlSerializer.Deserialize<QuesterProfile>(fullpath);
                    ShowGroupedProfileManagerForm();
                }
                else
                {
                    SaveGroupedProfile.Hide();
                    ShowGroupedProfileManagerForm();
                }
                TopMost = true;
            }
            catch (Exception e)
            {
                Logging.WriteError("Quester > Profile > ProfileManager > ProfileManager(): " + e);
            }
        }

        private void Translate()
        {
            
              this.Text = nManager.Translate.Get(nManager.Translate.Id.GroupedProfileManager); // Form Title

              AvailableSimpleProfilesLabel.Text = nManager.Translate.Get(nManager.Translate.Id.AvailableSimpleProfiles);
              CurrentlyGroupedProfilesLabel.Text = nManager.Translate.Get(nManager.Translate.Id.CurrentlyGroupedProfiles);
              GroupSelectedProfile.Tooltip = nManager.Translate.Get(nManager.Translate.Id.GroupSelectedProfile);
              UngroupSelectedProfile.Tooltip = nManager.Translate.Get(nManager.Translate.Id.UngroupSelectedProfile);
              UngroupAllProfiles.Tooltip = nManager.Translate.Get(nManager.Translate.Id.UngroupAllProfiles);
              SaveGroupedProfileAs.Text = nManager.Translate.Get(nManager.Translate.Id.SaveGroupedProfileAs);
              SaveGroupedProfile.Text = nManager.Translate.Get(nManager.Translate.Id.SaveGroupedProfile);
              CancelGroupedProfileEdition.Text = nManager.Translate.Get(nManager.Translate.Id.CancelGroupedProfileEdition);
              MoveUpButton.Text = nManager.Translate.Get(nManager.Translate.Id.MoveUp);
              MoveDownButton.Text = nManager.Translate.Get(nManager.Translate.Id.MoveDown);
             
        }

        private void ShowGroupedProfileManagerForm()
        {
            LoadGroupedProfileList();
            RefreshSimpleProfileList(true);
        }

        private void LoadGroupedProfileList()
        {
            try
            {
                if (Profile == null) return;
                foreach (Include include in Profile.Includes)
                {
                    CurrentlyGroupedProfiles.Items.Add(include.PathFile);
                }
                if (CurrentlyGroupedProfiles.Items.Count > 0)
                    CurrentlyGroupedProfiles.SelectedIndex = 0;
            }
            catch (Exception e)
            {
                Logging.WriteError("Quester > Profile > GroupedProfileManager > LoadGroupedProfileList(): " + e);
            }
        }

        private void RefreshSimpleProfileList(bool Load = false)
        {
            try
            {
                int CurrentSelection = AvailableSimpleProfiles.Items.Count;
                bool Abort = false;
                AvailableSimpleProfiles.Items.Clear();
                foreach (string f in Others.GetFilesDirectory(Application.StartupPath + "\\Profiles\\Quester\\", "*.xml"))
                {
                    if (CurrentlyGroupedProfiles.Items.Cast<string>().Any(Grouped => Grouped == f))
                        Abort = true;
                    if (!Abort)
                        AvailableSimpleProfiles.Items.Add(f);
                    else
                        Abort = false;
                }
                if (Load && AvailableSimpleProfiles.Items.Count > 0)
                    AvailableSimpleProfiles.SelectedIndex = 0;
                else if (CurrentSelection <= AvailableSimpleProfiles.Items.Count - 1 && AvailableSimpleProfiles.Items.Count > 0)
                    AvailableSimpleProfiles.SelectedIndex = CurrentSelection;
                else if (AvailableSimpleProfiles.Items.Count > 0)
                    AvailableSimpleProfiles.SelectedIndex = 0;
            }
            catch (Exception e)
            {
                Logging.WriteError("Quester > Profile > ProfileManager > RefreshSimpleProfileList(): " + e);
            }
        }

        private void SaveGroupedProfileAs_Click(object sender, EventArgs e)
        {
            if (CurrentlyGroupedProfiles.Items.Count > 0)
            {
                foreach (string PathFile in CurrentlyGroupedProfiles.Items)
                {
                    string FullPathToFile = Application.StartupPath + "\\Profiles\\Quester\\" + PathFile;
                    if (!string.IsNullOrWhiteSpace(PathFile) && File.Exists(FullPathToFile))
                    {
                        ProfileToSave.Includes.Add(new Include {PathFile = PathFile, ScriptCondition = ""});
                    }
                }
                string FileToSaveAs = Others.DialogBoxSaveFile(Application.StartupPath + "\\Profiles\\Quester\\Grouped\\", nManager.Translate.Get(nManager.Translate.Id.GroupedQuestProfileFile) + " (*.xml)|*.xml");
                if (FileToSaveAs != "")
                    XmlSerializer.Serialize(FileToSaveAs, ProfileToSave);
                Close();
            }
            else
            {
                MessageBox.Show(nManager.Translate.Get(nManager.Translate.Id.CantSaveEmptyGroupedNew));
            }
        }

        private void SaveGroupedProfile_Click(object sender, EventArgs e)
        {
            if (CurrentlyGroupedProfiles.Items.Count > 0)
            {
                foreach (string PathFile in CurrentlyGroupedProfiles.Items)
                {
                    string FullPathToFile = Application.StartupPath + "\\Profiles\\Quester\\" + PathFile;
                    if (!string.IsNullOrWhiteSpace(PathFile) && File.Exists(FullPathToFile))
                    {
                        ProfileToSave.Includes.Add(new Include {PathFile = PathFile, ScriptCondition = ""});
                    }
                }
                XmlSerializer.Serialize(fullpath, ProfileToSave);
                Close();
            }
            else
            {
                MessageBox.Show(nManager.Translate.Get(nManager.Translate.Id.CantSaveEmptyGroupedExisting));
            }
        }

        private void DoGroupSelectedProfile(object sender, EventArgs e)
        {
            if (AvailableSimpleProfiles.SelectedItems.Count > 0)
            {
                for (int i = AvailableSimpleProfiles.SelectedIndices.Count - 1; i >= 0; i--)
                {
                    if (!CurrentlyGroupedProfiles.Items.Contains(AvailableSimpleProfiles.Items[AvailableSimpleProfiles.SelectedIndices[i]]))
                    {
                        CurrentlyGroupedProfiles.Items.Add(AvailableSimpleProfiles.Items[AvailableSimpleProfiles.SelectedIndices[i]]);
                        CurrentlyGroupedProfiles.SelectedIndex = CurrentlyGroupedProfiles.Items.Count - 1;
                        AvailableSimpleProfiles.Items.RemoveAt(AvailableSimpleProfiles.SelectedIndices[i]);
                        if (AvailableSimpleProfiles.Items.Count > 0)
                            AvailableSimpleProfiles.SelectedIndex = 0;
                    }
                }
            }
        }

        private void DoUngroupSelectedProfile(object sender, EventArgs e)
        {
            if (CurrentlyGroupedProfiles.SelectedItems.Count > 0)
            {
                for (int i = CurrentlyGroupedProfiles.SelectedIndices.Count - 1; i >= 0; i--)
                {
                    if (!AvailableSimpleProfiles.Items.Contains(CurrentlyGroupedProfiles.Items[CurrentlyGroupedProfiles.SelectedIndices[i]]))
                    {
                        AvailableSimpleProfiles.Items.Add(CurrentlyGroupedProfiles.Items[CurrentlyGroupedProfiles.SelectedIndices[i]]);
                        AvailableSimpleProfiles.SelectedIndex = AvailableSimpleProfiles.Items.Count - 1;
                        CurrentlyGroupedProfiles.Items.RemoveAt(CurrentlyGroupedProfiles.SelectedIndices[i]);
                        if (CurrentlyGroupedProfiles.Items.Count > 0)
                            CurrentlyGroupedProfiles.SelectedIndex = 0;
                    }
                }
            }
        }

        private void DoUngroupAllProfiles(object sender, EventArgs e)
        {
            if (CurrentlyGroupedProfiles.SelectedItems.Count > 0)
            {
                for (int i = CurrentlyGroupedProfiles.Items.Count - 1; i >= 0; i--)
                {
                    AvailableSimpleProfiles.Items.Add(CurrentlyGroupedProfiles.Items[i]);
                    AvailableSimpleProfiles.SelectedIndex = AvailableSimpleProfiles.Items.Count - 1;
                    CurrentlyGroupedProfiles.Items.RemoveAt(i);
                }
            }
        }

        private void CancelGroupedProfileEdition_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void MoveItem(int direction)
        {
            if (CurrentlyGroupedProfiles.SelectedItem == null || CurrentlyGroupedProfiles.SelectedIndex < 0)
                return;
            int newIndex = CurrentlyGroupedProfiles.SelectedIndex + direction;
            if (newIndex < 0 || newIndex >= CurrentlyGroupedProfiles.Items.Count)
                return;
            object selected = CurrentlyGroupedProfiles.SelectedItem;
            CurrentlyGroupedProfiles.Items.Remove(selected);
            CurrentlyGroupedProfiles.Items.Insert(newIndex, selected);
            CurrentlyGroupedProfiles.SetSelected(newIndex, true);
        }

        private void MoveUpButton_Click(object sender, EventArgs e)
        {
            MoveItem(-1);
        }

        private void MoveDownButton_Click(object sender, EventArgs e)
        {
            MoveItem(1);
        }


    }
}