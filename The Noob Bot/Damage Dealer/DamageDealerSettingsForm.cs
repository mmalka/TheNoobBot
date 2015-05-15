using System;
using System.Windows.Forms;
using Damage_Dealer.Bot;
using nManager;
using nManager.Wow.Helpers;

namespace Damage_Dealer
{
    public partial class DamageDealerSettingsForm : Form
    {
        public DamageDealerSettingsForm()
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
            /*ActivateAutoFacingLabel.Text = nManager.Translate.Get(nManager.Translate.Id.MasterBotIPAddress);
            ActivateMovementsLabel.Text = nManager.Translate.Get(nManager.Translate.Id.ActivatePartyMode);*/
            SaveAndCloseButton.Text = nManager.Translate.Get(nManager.Translate.Id.Save_and_Close);
            /*MainHeader.TitleText = nManager.Translate.Get(nManager.Translate.Id.Settings_Mimesis);*/
            Text = MainHeader.TitleText;
            ActivateMovements.OnText = onText;
            ActivateMovements.OffText = offText;
            ActivateAutoFacing.OnText = onText;
            ActivateAutoFacing.OffText = offText;
        }

        private void SaveAndCloseButton_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void Save()
        {
            DamageDealerSettings.CurrentSetting.ActivateAutoFacing = ActivateAutoFacing.Value;
            DamageDealerSettings.CurrentSetting.ActivateMovements = ActivateMovements.Value;
            DamageDealerSettings.CurrentSetting.Save();
            Dispose();
        }

        private new void Load()
        {
            ActivateMovements.Value = DamageDealerSettings.CurrentSetting.ActivateMovements;
            ActivateAutoFacing.Value = DamageDealerSettings.CurrentSetting.ActivateAutoFacing;
        }

        private void SpellSettingsShortcutButton_Click(object sender, EventArgs e)
        {
            CombatClass.ShowConfigurationCombatClass(Application.StartupPath + "\\CombatClasses\\" + nManagerSetting.CurrentSetting.CombatClass);
        }
    }
}