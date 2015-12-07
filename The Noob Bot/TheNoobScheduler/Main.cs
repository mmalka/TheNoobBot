using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using nManager;
using nManager.Helpful;

namespace TheNoobScheduler
{
    public partial class Main : Form
    {
        private static SchedulerSettings _profile;

        public string SchedulerSettingsPath = Application.StartupPath + "\\Settings\\SchedulerSettings.xml";

        public Main()
        {
            InitializeComponent();
            Translate();
            InitializeSettings();
        }

        public void InitializeSettings()
        {
            _profile = new SchedulerSettings();
            if (File.Exists(SchedulerSettingsPath))
                _profile = XmlSerializer.Deserialize<SchedulerSettings>(Application.StartupPath + "\\Settings\\SchedulerSettings.xml");
            else
            {
                XmlSerializer.Serialize(SchedulerSettingsPath, _profile);
            }
            Logging.Write("Loading settings...");
            int iAccounts = 0;
            int iChars = 0;
            RefreshAccountList();
            foreach (BattleNetAccounts setting in _profile.Accounts)
            {
                Logging.Write("Load data for BattleNet account: " + setting.AccountEmail + "...");
                int i = 0;
                iAccounts++;
                foreach (Character character in setting.Characters)
                {
                    i++;
                    iChars++;
                    Logging.Write("Character " + character.CharacterName + "'s data loaded. ");
                }
                Logging.Write("Parsing complete, found " + i + " characters linked to this account.");
            }
            Logging.Write("Settings loaded. " + iAccounts + " accounts and " + iChars + " characters loaded.");
        }

        public void RefreshAccountList()
        {
            int selectedIndex = AccountList.SelectedIndex;
            AccountList.Items.Clear();
            foreach (BattleNetAccounts setting in _profile.Accounts)
            {
                AccountList.Items.Add(setting.AccountEmail + " - " + setting.AccountName);
            }
            if (selectedIndex == -1 && AccountList.Items.Count > 0)
                AccountList.SelectedIndex = 0;
            else if (AccountList.Items.Count > 0)
            {
                AccountList.SelectedIndex = selectedIndex <= AccountList.Items.Count - 1 ? selectedIndex : 0;
            }

            RefreshCharactersList();
        }

        public void RefreshCharactersList()
        {
            CharactersList.Items.Clear();
            CharacterNote.Clear();
            if (string.IsNullOrEmpty(AccountList.Text))
                return;
            foreach (BattleNetAccounts bnetAccount in _profile.Accounts)
            {
                if (AccountList.Text == bnetAccount.AccountEmail + " - " + bnetAccount.AccountName)
                {
                    foreach (Character character in bnetAccount.Characters)
                    {
                        CharactersList.Items.Add(character.CharacterName + " - " + character.CharacterRealm + " - " + character.CharacterFaction + " - " + (character.IsActive ? "Active" : "Inactive"));
                    }
                    if (CharactersList.Items.Count > 0)
                        CharactersList.SelectedIndex = 0;
                }
            }
            RefreshCharacterNote();
        }

        public void RefreshCharacterNote()
        {
            CharacterNote.Text = "";
            foreach (BattleNetAccounts bnetAccount in _profile.Accounts)
            {
                if (AccountList.Text == bnetAccount.AccountEmail + " - " + bnetAccount.AccountName)
                {
                    foreach (Character character in bnetAccount.Characters)
                    {
                        if (CharactersList.Text == character.CharacterName + " - " + character.CharacterRealm + " - " + character.CharacterFaction + " - " + (character.IsActive ? "Active" : "Inactive"))
                        {
                            CharacterNote.Text = character.CharacterNote;
                            break;
                        }
                    }
                    break;
                }
            }
        }

        public void CharacterToggleActive()
        {
            foreach (BattleNetAccounts bnetAccount in _profile.Accounts)
            {
                if (AccountList.Text == bnetAccount.AccountEmail + " - " + bnetAccount.AccountName)
                {
                    foreach (Character character in bnetAccount.Characters)
                    {
                        if (CharactersList.Text == character.CharacterName + " - " + character.CharacterRealm + " - " + character.CharacterFaction + " - " + (character.IsActive ? "Active" : "Inactive"))
                        {
                            character.IsActive = !character.IsActive;
                            CharactersList.Items[CharactersList.SelectedIndex] = character.CharacterName + " - " + character.CharacterRealm + " - " + character.CharacterFaction + " - " +
                                                                                 (character.IsActive ? "Active" : "Inactive");
                            XmlSerializer.Serialize(SchedulerSettingsPath, _profile);
                            break;
                        }
                    }
                    break;
                }
            }
        }

        public void SaveCharacterNote()
        {
            foreach (BattleNetAccounts bnetAccount in _profile.Accounts)
            {
                if (AccountList.Text == bnetAccount.AccountEmail + " - " + bnetAccount.AccountName)
                {
                    foreach (Character character in bnetAccount.Characters)
                    {
                        if (CharactersList.Text == character.CharacterName + " - " + character.CharacterRealm + " - " + character.CharacterFaction + " - " + (character.IsActive ? "Active" : "Inactive"))
                        {
                            character.CharacterNote = CharacterNote.Text;
                            XmlSerializer.Serialize(SchedulerSettingsPath, _profile);
                            break;
                        }
                    }
                    break;
                }
            }
        }

        public void Translate()
        {
            MainHeader.TitleText = Information.SchedulerTitle;
            if (LoginServer.IsFreeVersion)
                MainHeader.TitleText += " - Trial";
        }

        private void CloseButton_Click(object sender, FormClosedEventArgs e)
        {
            Pulsator.Dispose(true);
        }

        private void AccountList_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshCharactersList();
        }

        private void CharactersList_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshCharacterNote();
        }

        private void CharacterNoteSaveButton_Click(object sender, EventArgs e)
        {
            CharacterNoteSaveButton.Enabled = false;
            Thread.Sleep(250);
            SaveCharacterNote();
            CharacterNoteSaveButton.Enabled = true;
        }

        private void CharactersList_DoubleClick(object sender, EventArgs e)
        {
            CharactersList.Enabled = false;
            Thread.Sleep(250);
            CharacterToggleActive();
            CharactersList.Enabled = true;
        }

        private void CharDelButton_Click(object sender, EventArgs e)
        {
            CharDelButton.Enabled = false;
            DialogResult result = MessageBox.Show("Do you really want to delete this character ?", "Warning", MessageBoxButtons.YesNo);
            if (result == DialogResult.No)
            {
                CharDelButton.Enabled = true;
                return;
            }
            foreach (BattleNetAccounts bnetAccount in _profile.Accounts)
            {
                if (AccountList.Text == bnetAccount.AccountEmail + " - " + bnetAccount.AccountName)
                {
                    foreach (Character character in bnetAccount.Characters)
                    {
                        if (CharactersList.Text == character.CharacterName + " - " + character.CharacterRealm + " - " + character.CharacterFaction + " - " + (character.IsActive ? "Active" : "Inactive"))
                        {
                            bnetAccount.Characters.Remove(character);
                            XmlSerializer.Serialize(SchedulerSettingsPath, _profile);
                            RefreshCharactersList();
                            break;
                        }
                    }
                    break;
                }
            }
            CharDelButton.Enabled = true;
        }

        private void AccountDelButton_Click(object sender, EventArgs e)
        {
            AccountDelButton.Enabled = false;
            DialogResult result = MessageBox.Show("Do you really want to delete this account ?", "Warning", MessageBoxButtons.YesNo);
            if (result == DialogResult.No)
            {
                AccountDelButton.Enabled = true;
                return;
            }
            foreach (BattleNetAccounts bnetAccount in _profile.Accounts)
            {
                if (AccountList.Text == bnetAccount.AccountEmail + " - " + bnetAccount.AccountName)
                {
                    _profile.Accounts.Remove(bnetAccount);
                    XmlSerializer.Serialize(SchedulerSettingsPath, _profile);
                    RefreshAccountList();
                    break;
                }
            }
            AccountDelButton.Enabled = true;
        }
    }
}