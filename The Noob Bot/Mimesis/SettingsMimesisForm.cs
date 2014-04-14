using System;
using System.Windows.Forms;
using Mimesis.Bot;
using nManager;

namespace Mimesis
{
    public partial class SettingsMimesisForm : Form
    {
        public SettingsMimesisForm()
        {
            InitializeComponent();
            Translate();
            if (nManagerSetting.CurrentSetting.ActivateAlwaysOnTopFeature)
                TopMost = true;
            Load();
        }

        private void Translate()
        {
            string onText = nManager.Translate.Get(nManager.Translate.Id.YES);
            string offText = nManager.Translate.Get(nManager.Translate.Id.NO);
            MasterBotIPAddressLabel.Text = nManager.Translate.Get(nManager.Translate.Id.MasterBotIPAddress);
            MasterBotIPPortLabel.Text = nManager.Translate.Get(nManager.Translate.Id.MasterBotIPPort);
            ActivatePartyModeLabel.Text = nManager.Translate.Get(nManager.Translate.Id.ActivatePartyMode);
            SaveAndCloseButton.Text = nManager.Translate.Get(nManager.Translate.Id.Save_and_Close);
            MainHeader.TitleText = nManager.Translate.Get(nManager.Translate.Id.Settings_Mimesis);
            ActivatePartyMode.OnText = onText;
            ActivatePartyMode.OffText = offText;
        }

        private void SaveAndCloseButton_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void Save()
        {
            MimesisSettings.CurrentSetting.MasterIPAddress = MimesisMasterAddr.Text;
            MimesisSettings.CurrentSetting.MasterIPPort = (int) MimesisMasterPort.Value;
            MimesisSettings.CurrentSetting.ActivatePartyMode = ActivatePartyMode.Value;
            MimesisSettings.CurrentSetting.Save();
            Dispose();
        }

        private new void Load()
        {
            ActivatePartyMode.Value = MimesisSettings.CurrentSetting.ActivatePartyMode;
            MimesisMasterAddr.Text = MimesisSettings.CurrentSetting.MasterIPAddress;
            MimesisMasterPort.Text = MimesisSettings.CurrentSetting.MasterIPPort.ToString();
        }
    }
}