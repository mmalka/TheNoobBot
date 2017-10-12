using System;
using System.IO;
using System.Windows.Forms;
using nManager;
using nManager.Helpful;
using Quester.Properties;

namespace Quester.Profile
{
    public partial class GroupedProfileManager : Form
    {
        private readonly string _fullpath = "";
        private readonly QuesterProfile _profile = new QuesterProfile();

        public GroupedProfileManager(string path = "")
        {
            try
            {
                InitializeComponent();
                Translate();
                _fullpath = Application.StartupPath + "\\Profiles\\Quester\\Grouped\\" + path;
                if (!string.IsNullOrWhiteSpace(path) && File.Exists(_fullpath))
                {
                    _profile = XmlSerializer.Deserialize<QuesterProfile>(_fullpath);
                    ShowGroupedProfileManagerForm();
                }
                else
                {
                    SaveGroupedProfile.Hide();
                    ShowGroupedProfileManagerForm();
                }
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
            MainHeader.TitleText = nManager.Translate.Get(nManager.Translate.Id.GroupedProfileManager) + " - " + Information.MainTitle;
            this.Text = Others.GetRandomString(Others.Random(4, 10));
            AvailableSimpleProfilesLabel.Text = nManager.Translate.Get(nManager.Translate.Id.AvailableSimpleProfiles).ToUpper();
            CurrentlyGroupedProfilesLabel.Text = nManager.Translate.Get(nManager.Translate.Id.CurrentlyGroupedProfiles).ToUpper();
            if (toolTip == null)
                toolTip = new ToolTip();
            toolTip.SetToolTip(GroupSelectedProfile, nManager.Translate.Get(nManager.Translate.Id.GroupSelectedProfile));
            toolTip.SetToolTip(UngroupSelectedProfile, nManager.Translate.Get(nManager.Translate.Id.UngroupSelectedProfile));
            toolTip.SetToolTip(UngroupAllProfiles, nManager.Translate.Get(nManager.Translate.Id.UngroupAllProfiles));
            SaveGroupedProfileAs.Text = nManager.Translate.Get(nManager.Translate.Id.SaveAsAndClose);
            SaveGroupedProfile.Text = nManager.Translate.Get(nManager.Translate.Id.SaveAndClose);
            CancelGroupedProfileEdition.Text = nManager.Translate.Get(nManager.Translate.Id.CancelAndClose);
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
                if (_profile == null) return;
                foreach (Include include in _profile.Includes)
                {
                    GroupedProfilesList.Items.Add(include.PathFile);
                }
                if (GroupedProfilesList.Items.Count > 0)
                    GroupedProfilesList.SelectedIndex = 0;
            }
            catch (Exception e)
            {
                Logging.WriteError("Quester > Profile > GroupedProfileManager > LoadGroupedProfileList(): " + e);
            }
        }

        private void RefreshSimpleProfileList(bool load = false)
        {
            try
            {
                int currentSelection = SimpleProfilesList.Items.Count;
                bool abort = false;
                SimpleProfilesList.Items.Clear();
                foreach (string f in Others.GetFilesDirectory(Application.StartupPath + "\\Profiles\\Quester\\", "*.xml"))
                {
                    foreach (string grouped in GroupedProfilesList.Items)
                    {
                        if (grouped == f)
                        {
                            abort = true;
                            break;
                        }
                    }
                    if (!abort)
                        SimpleProfilesList.Items.Add(f);
                    else
                        abort = false;
                }
                if (load && SimpleProfilesList.Items.Count > 0)
                    SimpleProfilesList.SelectedIndex = 0;
                else if (currentSelection <= SimpleProfilesList.Items.Count - 1 && SimpleProfilesList.Items.Count > 0)
                    SimpleProfilesList.SelectedIndex = currentSelection;
                else if (SimpleProfilesList.Items.Count > 0)
                    SimpleProfilesList.SelectedIndex = 0;
            }
            catch (Exception e)
            {
                Logging.WriteError("Quester > Profile > GroupedProfileManager > RefreshSimpleProfileList(): " + e);
            }
        }

        private void SaveGroupedProfileAs_Click(object sender, EventArgs e)
        {
            if (GroupedProfilesList.Items.Count > 0)
            {
                _profile.Includes.Clear();
                foreach (string pathFile in GroupedProfilesList.Items)
                {
                    string fullPathToFile = Application.StartupPath + "\\Profiles\\Quester\\" + pathFile;
                    if (!string.IsNullOrWhiteSpace(pathFile) && File.Exists(fullPathToFile))
                    {
                        _profile.Includes.Add(new Include {PathFile = pathFile, ScriptCondition = ""});
                    }
                }
                string fileToSaveAs = Others.DialogBoxSaveFile(Application.StartupPath + "\\Profiles\\Quester\\Grouped\\",
                    nManager.Translate.Get(nManager.Translate.Id.GroupedQuestProfileFile) + " (*.xml)|*.xml");
                if (fileToSaveAs == "") return;
                if (XmlSerializer.Serialize(fileToSaveAs, _profile))
                    Close();
            }
            else
            {
                MessageBox.Show(nManager.Translate.Get(nManager.Translate.Id.CantSaveEmptyGroupedNew));
            }
        }

        private void SaveGroupedProfile_Click(object sender, EventArgs e)
        {
            if (GroupedProfilesList.Items.Count > 0)
            {
                _profile.Includes.Clear();
                foreach (string pathFile in GroupedProfilesList.Items)
                {
                    string fullPathToFile = Application.StartupPath + "\\Profiles\\Quester\\" + pathFile;
                    if (!string.IsNullOrWhiteSpace(pathFile) && File.Exists(fullPathToFile))
                    {
                        _profile.Includes.Add(new Include {PathFile = pathFile, ScriptCondition = ""});
                    }
                }
                XmlSerializer.Serialize(_fullpath, _profile);
                Close();
            }
            else
            {
                MessageBox.Show(nManager.Translate.Get(nManager.Translate.Id.CantSaveEmptyGroupedExisting));
            }
        }

        private void DoGroupSelectedProfile(object sender, EventArgs e)
        {
            if (SimpleProfilesList.SelectedItems.Count > 0)
            {
                for (int i = SimpleProfilesList.SelectedIndices.Count - 1; i >= 0; i--)
                {
                    if (!GroupedProfilesList.Items.Contains(SimpleProfilesList.Items[SimpleProfilesList.SelectedIndices[i]]))
                    {
                        GroupedProfilesList.Items.Add(SimpleProfilesList.Items[SimpleProfilesList.SelectedIndices[i]]);
                        GroupedProfilesList.SelectedIndex = GroupedProfilesList.Items.Count - 1;
                        SimpleProfilesList.Items.RemoveAt(SimpleProfilesList.SelectedIndices[i]);
                    }
                }
            }
        }

        private void DoUngroupSelectedProfile(object sender, EventArgs e)
        {
            if (GroupedProfilesList.SelectedItems.Count > 0)
            {
                for (int i = GroupedProfilesList.SelectedIndices.Count - 1; i >= 0; i--)
                {
                    if (!SimpleProfilesList.Items.Contains(GroupedProfilesList.Items[GroupedProfilesList.SelectedIndices[i]]))
                    {
                        SimpleProfilesList.Items.Add(GroupedProfilesList.Items[GroupedProfilesList.SelectedIndices[i]]);
                        SimpleProfilesList.SelectedIndex = SimpleProfilesList.Items.Count - 1;
                        GroupedProfilesList.Items.RemoveAt(GroupedProfilesList.SelectedIndices[i]);
                        if (GroupedProfilesList.Items.Count > 0)
                            GroupedProfilesList.SelectedIndex = 0;
                    }
                }
            }
        }

        private void DoUngroupAllProfiles(object sender, EventArgs e)
        {
            if (GroupedProfilesList.SelectedItems.Count > 0)
            {
                for (int i = GroupedProfilesList.Items.Count - 1; i >= 0; i--)
                {
                    SimpleProfilesList.Items.Add(GroupedProfilesList.Items[i]);
                    SimpleProfilesList.SelectedIndex = SimpleProfilesList.Items.Count - 1;
                    GroupedProfilesList.Items.RemoveAt(i);
                }
            }
        }

        private void CancelGroupedProfileEdition_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void MoveItem(int direction)
        {
            if (GroupedProfilesList.SelectedItem == null || GroupedProfilesList.SelectedIndex < 0)
                return;
            int newIndex = GroupedProfilesList.SelectedIndex + direction;
            if (newIndex < 0 || newIndex >= GroupedProfilesList.Items.Count)
                return;
            object selected = GroupedProfilesList.SelectedItem;
            GroupedProfilesList.Items.Remove(selected);
            GroupedProfilesList.Items.Insert(newIndex, selected);
            GroupedProfilesList.SetSelected(newIndex, true);
        }

        private void MoveUpButton_Click(object sender, EventArgs e)
        {
            MoveItem(-1);
        }

        private void MoveDownButton_Click(object sender, EventArgs e)
        {
            MoveItem(1);
        }

        private void CancelGroupedProfileEdition_MouseEnter(object sender, EventArgs e)
        {
            CancelGroupedProfileEdition.Image = Resources.greenB;
        }

        private void CancelGroupedProfileEdition_MouseLeave(object sender, EventArgs e)
        {
            CancelGroupedProfileEdition.Image = Resources.blackB;
        }

        private void MoveUpButton_MouseEnter(object sender, EventArgs e)
        {
            MoveUpButton.Image = Resources.greenB;
        }

        private void MoveDownButton_MouseEnter(object sender, EventArgs e)
        {
            MoveDownButton.Image = Resources.greenB;
        }

        private void SaveGroupedProfile_MouseEnter(object sender, EventArgs e)
        {
            SaveGroupedProfile.Image = Resources.greenB;
        }

        private void SaveGroupedProfileAs_MouseEnter(object sender, EventArgs e)
        {
            SaveGroupedProfileAs.Image = Resources.greenB_150;
        }

        private void SaveGroupedProfileAs_MouseLeave(object sender, EventArgs e)
        {
            SaveGroupedProfileAs.Image = Resources.blueB_150;
        }

        private void SaveGroupedProfile_MouseLeave(object sender, EventArgs e)
        {
            SaveGroupedProfile.Image = Resources.blueB;
        }

        private void MoveDownButton_MouseLeave(object sender, EventArgs e)
        {
            MoveDownButton.Image = Resources.blackB;
        }

        private void MoveUpButton_MouseLeave(object sender, EventArgs e)
        {
            MoveUpButton.Image = Resources.blackB;
        }
    }
}