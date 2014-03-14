using System.Windows.Forms;
using Mimesis.Bot;
using nManager;
using nManager.Helpful;

namespace Mimesis
{
    public partial class SettingsMimesisForm : DevComponents.DotNetBar.Metro.MetroForm
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
            labelX6.Text = ""; // "* = " + nManager.Translate.Get(nManager.Translate.Id.If_special__If_empty__default_items_is_used);
            saveB.Text = nManager.Translate.Get(nManager.Translate.Id.Save_and_Close);
            Text = nManager.Translate.Get(nManager.Translate.Id.Settings_Mimesis);
            ActivatePartyMode.OnText = onText;
            ActivatePartyMode.OffText = offText;
        }

        private void saveB_Click(object sender, System.EventArgs e)
        {
            Save();
        }

        private void Save()
        {
            MimesisSettings.CurrentSetting.MasterIPAddress = MimesisMasterAddr.Text;
            MimesisSettings.CurrentSetting.MasterIPPort = MimesisMasterPort.Value;
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