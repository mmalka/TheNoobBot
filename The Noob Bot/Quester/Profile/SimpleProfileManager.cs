using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar.Metro;
using nManager.Helpful;
using nManager.Wow.Enums;

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
                    RefreshSimpleProfileList();
                }
                else
                {
                    RefreshSimpleProfileList();
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
            SaveSimpleProfileAs.Text = nManager.Translate.Get(nManager.Translate.Id.SaveAsAndClose);
            SaveSimpleProfile.Text = nManager.Translate.Get(nManager.Translate.Id.SaveAndClose);
            CancelSimpleProfileEdition.Text = nManager.Translate.Get(nManager.Translate.Id.CancelAndClose);
        }

        private void RefreshSimpleProfileList()
        {
            try
            {
                if (Profile == null) return;
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
                    SaveSimpleProfile.Show();
                    DeleteSelectedQuestButton.Show();
                    EditSelectedQuestButton.Show();
                    ProfileQuestList.SelectedIndex = 0;
                }
                else
                {
                    SaveSimpleProfile.Hide();
                    DeleteSelectedQuestButton.Hide();
                    EditSelectedQuestButton.Hide();
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("Quester > Profile > SimpleProfileManager > LoadSimpleProfileList(): " + e);
            }
        }

        private void SaveSimpleProfileAs_Click(object sender, EventArgs e)
        {
            if (Profile.Quests.Count > 0)
            {
                string FileToSaveAs = Others.DialogBoxSaveFile(Application.StartupPath + "\\Profiles\\Quester\\Simple\\", nManager.Translate.Get(nManager.Translate.Id.SimpleQuestProfileFile) + " (*.xml)|*.xml");
                if (FileToSaveAs != "")
                    XmlSerializer.Serialize(FileToSaveAs, Profile);
                Close();
            }
            else
            {
                MessageBox.Show(nManager.Translate.Get(nManager.Translate.Id.CantSaveEmptySimpleNew));
            }
        }

        private void SaveSimpleProfile_Click(object sender, EventArgs e)
        {
            if (Profile.Quests.Count > 0)
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
            RefreshSimpleProfileList();
        }

        private void EditSelectedQuest(object sender, EventArgs e)
        {
            MessageBox.Show(nManager.Translate.Get(nManager.Translate.Id.FeatureNotYetAvailable));
            RefreshSimpleProfileList();
        }

        private void DeleteSelectedQuest(object sender, EventArgs e)
        {
            if (ProfileQuestList.Items.Count > 0 && ProfileQuestList.Items.Count == Profile.Quests.Count)
            {
                ProfileQuestList.Items.Remove(ProfileQuestList.Items[ProfileQuestList.SelectedIndex]);
                Profile.Quests.RemoveAt(ProfileQuestList.SelectedIndex);
            }
            RefreshSimpleProfileList();
        }
    }
}