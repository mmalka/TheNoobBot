using System;
using System.IO;
using System.Windows.Forms;
using DevComponents.DotNetBar.Metro;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.Enums;
using nManager.Wow.ObjectManager;

namespace Quester.Profile
{
    public partial class SimpleProfileManager : MetroForm
    {
        private readonly QuesterProfile Profile = new QuesterProfile();
        private readonly string fullpath = "";

        public SimpleProfileManager(string path = "")
        {
            try
            {
                InitializeComponent();
                Translate();
                fullpath = Application.StartupPath + "\\Profiles\\Quester\\" + path;
                if (!string.IsNullOrWhiteSpace(path) && File.Exists(fullpath))
                {
                    Profile = XmlSerializer.Deserialize<QuesterProfile>(fullpath);
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
            Text = nManager.Translate.Get(nManager.Translate.Id.SimpleProfileManager); // Form Title
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
                if (Profile == null) return;
                ProfileQuestList.Items.Clear();
                foreach (Quest quest in Profile.Quests)
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
                if (Profile == null) return;
                ProfileQuesterList.Items.Clear();
                foreach (Npc npc in Profile.Questers)
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
            if (Profile.Quests.Count > 0 || Profile.Questers.Count > 0)
            {
                string fileToSaveAs = Others.DialogBoxSaveFile(Application.StartupPath + "\\Profiles\\Quester\\Simple\\", nManager.Translate.Get(nManager.Translate.Id.SimpleQuestProfileFile) + " (*.xml)|*.xml");
                if (fileToSaveAs != "")
                    XmlSerializer.Serialize(fileToSaveAs, Profile);
                Close();
            }
            else
            {
                MessageBox.Show(nManager.Translate.Get(nManager.Translate.Id.CantSaveEmptySimpleNew));
            }
        }

        private void SaveSimpleProfile_Click(object sender, EventArgs e)
        {
            if (Profile.Quests.Count > 0 || Profile.Questers.Count > 0)
            {
                XmlSerializer.Serialize(fullpath, Profile);
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

        private void AddNewQuest(object sender, EventArgs e)
        {
            MessageBox.Show(nManager.Translate.Get(nManager.Translate.Id.FeatureNotYetAvailable));
            RefreshProfileQuestList();
        }

        private void EditSelectedQuest(object sender, EventArgs e)
        {
            MessageBox.Show(nManager.Translate.Get(nManager.Translate.Id.FeatureNotYetAvailable));
            RefreshProfileQuestList(ProfileQuestList.SelectedIndex);
        }

        private void DeleteSelectedQuest(object sender, EventArgs e)
        {
            int selectedIndex = ProfileQuestList.SelectedIndex;
            if (ProfileQuestList.Items.Count > 0 && ProfileQuestList.Items.Count == Profile.Quests.Count)
            {
                Profile.Quests.RemoveAt(selectedIndex);
            }
            RefreshProfileQuestList(selectedIndex);
        }

        private void AddNewQuester(object sender, EventArgs e)
        {
            if (ObjectManager.Target.Guid == 0 || ObjectManager.Target.IsNpcQuestGiver)
            {
                MessageBox.Show(@"The target is not a valid Npc Quest Giver");
                return;
            }
            Npc npc = new Npc {Entry = ObjectManager.Target.Entry, Name = ObjectManager.Target.Name, Position = ObjectManager.Target.Position};
            Profile.Questers.Add(npc);
            RefreshProfileQuesterList(ProfileQuesterList.Items.Count - 1);
        }

        private void EditSelectedQuester(object sender, EventArgs e)
        {
            int selectedIndex = ProfileQuesterList.SelectedIndex;
            if (ObjectManager.Target.Guid == 0 || ObjectManager.Target.IsNpcQuestGiver)
            {
                MessageBox.Show(@"The target is not a valid Npc Quest Giver");
                return;
            }
            Npc npc = new Npc {Entry = ObjectManager.Target.Entry, Name = ObjectManager.Target.Name, Position = ObjectManager.Target.Position};
            if (ProfileQuesterList.Items.Count <= 0 || ProfileQuesterList.Items.Count != Profile.Questers.Count) return;
            if (!ProfileQuesterList.SelectedItem.ToString().Contains(npc.Entry + " -") || Profile.Questers[selectedIndex].Entry != npc.Entry)
            {
                MessageBox.Show(@"The NPC selected do not share the same EntryId as the current target, you may prefer to delete it first, then add.");
            }
            else
            {
                Profile.Questers[selectedIndex].Name = npc.Name;
                Profile.Questers[selectedIndex].Position = npc.Position;
            }
            RefreshProfileQuesterList(selectedIndex);
        }

        private void DeleteSelectedQuester(object sender, EventArgs e)
        {
            int selectedIndex = ProfileQuesterList.SelectedIndex;
            if (ProfileQuesterList.Items.Count > 0 && ProfileQuesterList.Items.Count == Profile.Questers.Count)
            {
                Profile.Questers.RemoveAt(selectedIndex);
            }
            RefreshProfileQuesterList(selectedIndex);
        }
    }
}