using System;
using System.IO;
using System.Windows.Forms;
using nManager;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.Enums;
using nManager.Wow.ObjectManager;
using Quester.Properties;
using Point = System.Drawing.Point;

namespace Quester.Profile
{
    public partial class SimpleProfileManager : Form
    {
        private readonly string _fullpath = "";
        private readonly QuesterProfile _profile = new QuesterProfile();
        private bool _flagClick;
        private int _positionInitialeX;
        private int _positionInitialeY;

        public SimpleProfileManager(string path = "")
        {
            try
            {
                InitializeComponent();
                Translate();
                _fullpath = Application.StartupPath + "\\Profiles\\Quester\\" + path;
                if (!string.IsNullOrWhiteSpace(path) && File.Exists(_fullpath))
                {
                    _profile = XmlSerializer.Deserialize<QuesterProfile>(_fullpath);
                    RefreshProfileQuestList();
                    RefreshProfileQuesterList();
                }
                else
                {
                    RefreshProfileQuestList();
                    RefreshProfileQuesterList();
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
            SimpleProfileManagerFormTitle.Text = nManager.Translate.Get(nManager.Translate.Id.SimpleProfileManager) + " - " + Information.MainTitle;
            AddNewQuestButton.Text = nManager.Translate.Get(nManager.Translate.Id.AddNewQuest);
            EditSelectedQuestButton.Text = nManager.Translate.Get(nManager.Translate.Id.EditSelectedQuest);
            DeleteSelectedQuestButton.Text = nManager.Translate.Get(nManager.Translate.Id.DeleteSelectedQuest);
            ProfileQuestListLabel.Text = nManager.Translate.Get(nManager.Translate.Id.ProfileQuestList);
            AddNewQuesterButton.Text = nManager.Translate.Get(nManager.Translate.Id.AddNewQuester);
            EditSelectedQuesterButton.Text = nManager.Translate.Get(nManager.Translate.Id.EditSelectedQuester);
            DeleteSelectedQuesterButton.Text = nManager.Translate.Get(nManager.Translate.Id.DeleteSelectedQuester);
            ProfileQuestersListLabel.Text = nManager.Translate.Get(nManager.Translate.Id.ProfileQuesterList);
            SaveSimpleProfileAs.Text = nManager.Translate.Get(nManager.Translate.Id.SaveAsAndClose);
            SaveSimpleProfile.Text = nManager.Translate.Get(nManager.Translate.Id.SaveAndClose);
            CancelSimpleProfileEdition.Text = nManager.Translate.Get(nManager.Translate.Id.CancelAndClose);
        }

        private void RefreshButtonsStatus()
        {
            bool showGlobalsButtons = false;
            if (ProfileQuestList.Items.Count > 0)
            {
                showGlobalsButtons = true;
                DeleteSelectedQuestButton.Show();
                EditSelectedQuestButton.Show();
            }
            else
            {
                DeleteSelectedQuestButton.Hide();
                EditSelectedQuestButton.Hide();
            }
            if (ProfileQuesterList.Items.Count > 0)
            {
                showGlobalsButtons = true;
                DeleteSelectedQuesterButton.Show();
                EditSelectedQuesterButton.Show();
            }
            else
            {
                DeleteSelectedQuesterButton.Hide();
                EditSelectedQuesterButton.Hide();
            }
            if (showGlobalsButtons)
                SaveSimpleProfile.Show();
            else
                SaveSimpleProfile.Hide();
        }

        private void RefreshProfileQuestList(int indexToSelect = 0)
        {
            try
            {
                if (_profile == null) return;
                ProfileQuestList.Items.Clear();
                foreach (Quest quest in _profile.Quests)
                {
                    string classSpecific = "";
                    foreach (object wowClass in Enum.GetValues(typeof (WoWClassMask)))
                    {
                        if ((int) wowClass != quest.ClassMask) continue;
                        classSpecific = wowClass.ToString();
                        break;
                    }
                    if (!string.IsNullOrEmpty(classSpecific))
                        classSpecific = " - Only " + classSpecific;
                    ProfileQuestList.Items.Add(quest.Id + " - " + quest.Name + " - MinLevel " + quest.MinLevel + classSpecific);
                }
                if (ProfileQuestList.Items.Count > 0)
                {
                    if (indexToSelect < ProfileQuestList.Items.Count)
                        ProfileQuestList.SelectedIndex = indexToSelect;
                    else ProfileQuestList.SelectedIndex = indexToSelect - 1;
                }
                RefreshButtonsStatus();
            }
            catch (Exception e)
            {
                Logging.WriteError("Quester > Profile > SimpleProfileManager > LoadSimpleProfileList(): " + e);
            }
        }

        private void RefreshProfileQuesterList(int indexToSelect = 0)
        {
            try
            {
                if (_profile == null) return;
                ProfileQuesterList.Items.Clear();
                foreach (Npc npc in _profile.Questers)
                {
                    ProfileQuesterList.Items.Add(npc.Entry + " - " + npc.Name + " - GPS:" + npc.Position);
                }
                if (ProfileQuesterList.Items.Count > 0)
                {
                    if (indexToSelect < ProfileQuesterList.Items.Count)
                        ProfileQuesterList.SelectedIndex = indexToSelect;
                    else ProfileQuesterList.SelectedIndex = indexToSelect - 1;
                }
                RefreshButtonsStatus();
            }
            catch (Exception e)
            {
                Logging.WriteError("Quester > Profile > SimpleProfileManager > LoadSimpleProfileList(): " + e);
            }
        }

        private void SaveSimpleProfileAs_Click(object sender, EventArgs e)
        {
            if (_profile.Quests.Count > 0 || _profile.Questers.Count > 0)
            {
                string fileToSaveAs = Others.DialogBoxSaveFile(Application.StartupPath + "\\Profiles\\Quester\\", nManager.Translate.Get(nManager.Translate.Id.SimpleQuestProfileFile) + " (*.xml)|*.xml");
                if (fileToSaveAs != "")
                    XmlSerializer.Serialize(fileToSaveAs, _profile);
                Close();
            }
            else
            {
                MessageBox.Show(nManager.Translate.Get(nManager.Translate.Id.CantSaveEmptySimpleNew));
            }
        }

        private void SaveSimpleProfile_Click(object sender, EventArgs e)
        {
            if (_profile.Quests.Count > 0 || _profile.Questers.Count > 0)
            {
                XmlSerializer.Serialize(_fullpath, _profile);
                Close();
            }
            else
            {
                MessageBox.Show(nManager.Translate.Get(nManager.Translate.Id.CantSaveEmptySimpleExisting));
            }
        }

        private void CancelSimpleProfileEdition_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void MainFormMouseDown(object sender, MouseEventArgs e)
        {
            _flagClick = true;
            _positionInitialeX = e.X;
            _positionInitialeY = e.Y;
        }

        private void MainFormMouseUp(object sender, MouseEventArgs e)
        {
            _flagClick = false;
        }


        private void MainFormMouseMove(object sender, MouseEventArgs e)
        {
            if (_flagClick)
            {
                Location = new Point(Left + (e.X - _positionInitialeX), Top + (e.Y - _positionInitialeY));
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ReduceButton_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void ReduceButton_MouseEnter(object sender, EventArgs e)
        {
            ReduceButton.Image = Resources.reduce_buttonG;
        }

        private void ReduceButton_MouseLeave(object sender, EventArgs e)
        {
            ReduceButton.Image = Resources.reduce_button;
        }

        private void CloseButton_MouseEnter(object sender, EventArgs e)
        {
            CloseButton.Image = Resources.close_buttonG;
        }

        private void CloseButton_MouseLeave(object sender, EventArgs e)
        {
            CloseButton.Image = Resources.close_button;
        }

        private void CancelSimpleProfileEdition_MouseEnter(object sender, EventArgs e)
        {
            CancelSimpleProfileEdition.Image = Resources.greenB;
        }

        private void CancelSimpleProfileEdition_MouseLeave(object sender, EventArgs e)
        {
            CancelSimpleProfileEdition.Image = Resources.blackB;
        }

        private void SaveSimpleProfile_MouseEnter(object sender, EventArgs e)
        {
            SaveSimpleProfile.Image = Resources.greenB;
        }

        private void SaveSimpleProfileAs_MouseEnter(object sender, EventArgs e)
        {
            SaveSimpleProfileAs.Image = Resources.greenB_150;
        }

        private void SaveSimpleProfileAs_MouseLeave(object sender, EventArgs e)
        {
            SaveSimpleProfileAs.Image = Resources.blueB_150;
        }

        private void SaveSimpleProfile_MouseLeave(object sender, EventArgs e)
        {
            SaveSimpleProfile.Image = Resources.blueB;
        }

        private void AddNewQuest(object sender, EventArgs e)
        {
            AddNewQuestButton.Enabled = false;
            MessageBox.Show(nManager.Translate.Get(nManager.Translate.Id.FeatureNotYetAvailable));
            RefreshProfileQuestList();
            AddNewQuestButton.Enabled = true;
        }

        private void EditSelectedQuest(object sender, EventArgs e)
        {
            EditSelectedQuestButton.Enabled = false;
            MessageBox.Show(nManager.Translate.Get(nManager.Translate.Id.FeatureNotYetAvailable));
            RefreshProfileQuestList(ProfileQuestList.SelectedIndex);
            EditSelectedQuestButton.Enabled = true;
        }

        private void DeleteSelectedQuest(object sender, EventArgs e)
        {
            DeleteSelectedQuestButton.Enabled = false;
            int selectedIndex = ProfileQuestList.SelectedIndex;
            if (ProfileQuestList.Items.Count > 0 && ProfileQuestList.Items.Count == _profile.Quests.Count)
            {
                _profile.Quests.RemoveAt(selectedIndex);
            }
            RefreshProfileQuestList(selectedIndex);
            DeleteSelectedQuestButton.Enabled = true;
        }

        private void AddNewQuester(object sender, EventArgs e)
        {
            AddNewQuesterButton.Enabled = false;
            if (ObjectManager.Target.Guid == 0 || ObjectManager.Target.IsNpcQuestGiver)
            {
                MessageBox.Show(@"The target is not a valid Npc Quest Giver");
                return;
            }
            var npc = new Npc {Entry = ObjectManager.Target.Entry, Name = ObjectManager.Target.Name, Position = ObjectManager.Target.Position};
            _profile.Questers.Add(npc);
            RefreshProfileQuesterList(ProfileQuesterList.Items.Count - 1);
            AddNewQuesterButton.Enabled = true;
        }

        private void EditSelectedQuester(object sender, EventArgs e)
        {
            EditSelectedQuesterButton.Enabled = false;
            int selectedIndex = ProfileQuesterList.SelectedIndex;
            if (ObjectManager.Target.Guid == 0 || ObjectManager.Target.IsNpcQuestGiver)
            {
                MessageBox.Show(@"The target is not a valid Npc Quest Giver");
                return;
            }
            var npc = new Npc {Entry = ObjectManager.Target.Entry, Name = ObjectManager.Target.Name, Position = ObjectManager.Target.Position};
            if (ProfileQuesterList.Items.Count <= 0 || ProfileQuesterList.Items.Count != _profile.Questers.Count) return;
            if (!ProfileQuesterList.SelectedItem.ToString().Contains(npc.Entry + " -") || _profile.Questers[selectedIndex].Entry != npc.Entry)
            {
                MessageBox.Show(@"The NPC selected do not share the same EntryId as the current target, you may prefer to delete it first, then add.");
            }
            else
            {
                _profile.Questers[selectedIndex].Name = npc.Name;
                _profile.Questers[selectedIndex].Position = npc.Position;
            }
            RefreshProfileQuesterList(selectedIndex);
            EditSelectedQuesterButton.Enabled = false;
        }

        private void DeleteSelectedQuester(object sender, EventArgs e)
        {
            DeleteSelectedQuestButton.Enabled = false;
            int selectedIndex = ProfileQuesterList.SelectedIndex;
            if (ProfileQuesterList.Items.Count > 0 && ProfileQuesterList.Items.Count == _profile.Questers.Count)
            {
                _profile.Questers.RemoveAt(selectedIndex);
            }
            RefreshProfileQuesterList(selectedIndex);
            DeleteSelectedQuestButton.Enabled = true;
        }

        private void AddNewQuestButton_MouseEnter(object sender, EventArgs e)
        {
            AddNewQuestButton.Image = Resources.greenB_200;
        }

        private void EditSelectedQuestButton_MouseEnter(object sender, EventArgs e)
        {
            EditSelectedQuestButton.Image = Resources.greenB_200;
        }

        private void DeleteSelectedQuestButton_MouseEnter(object sender, EventArgs e)
        {
            DeleteSelectedQuestButton.Image = Resources.greenB_200;
        }

        private void AddNewQuestButton_MouseLeave(object sender, EventArgs e)
        {
            AddNewQuestButton.Image = Resources.blueB_200;
        }

        private void EditSelectedQuestButton_MouseLeave(object sender, EventArgs e)
        {
            EditSelectedQuestButton.Image = Resources.blueB_200;
        }

        private void DeleteSelectedQuestButton_MouseLeave(object sender, EventArgs e)
        {
            DeleteSelectedQuestButton.Image = Resources.blackB_200;
        }

        private void AddNewQuesterButton_MouseEnter(object sender, EventArgs e)
        {
            AddNewQuesterButton.Image = Resources.greenB_200;
        }

        private void EditSelectedQuesterButton_MouseEnter(object sender, EventArgs e)
        {
            EditSelectedQuesterButton.Image = Resources.greenB_200;
        }

        private void EditSelectedQuesterButton_MouseLeave(object sender, EventArgs e)
        {
            EditSelectedQuesterButton.Image = Resources.blueB_200;
        }

        private void DeleteSelectedQuesterButton_MouseEnter(object sender, EventArgs e)
        {
            DeleteSelectedQuesterButton.Image = Resources.greenB_200;
        }

        private void DeleteSelectedQuesterButton_MouseLeave(object sender, EventArgs e)
        {
            DeleteSelectedQuesterButton.Image = Resources.blackB_200;
        }

        private void AddNewQuesterButton_MouseLeave(object sender, EventArgs e)
        {
            AddNewQuesterButton.Image = Resources.blueB_200;
        }
    }
}