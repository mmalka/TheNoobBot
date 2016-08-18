using System;
using System.Windows.Forms;
using Heal_Bot.Bot;
using nManager;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;

namespace Heal_Bot
{
    public partial class HealBotSettingsForm : Form
    {
        public HealBotSettingsForm()
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
            HealBotSettings.CurrentSetting.ActivateAutoFacing = ActivateAutoFacing.Value;
            HealBotSettings.CurrentSetting.ActivateMovements = ActivateMovements.Value;
            HealBotSettings.CurrentSetting.Save();
            Dispose();
        }

        private new void Load()
        {
            ActivateMovements.Value = HealBotSettings.CurrentSetting.ActivateMovements;
            ActivateAutoFacing.Value = HealBotSettings.CurrentSetting.ActivateAutoFacing;
        }

        private void SpellSettingsShortcutButton_Click(object sender, EventArgs e)
        {
            string pathToCombatClassFile;
            if (nManagerSetting.CurrentSetting.HealerClass == "OfficialTnbClassSelector")
                pathToCombatClassFile = Application.StartupPath + "\\HealerClasses\\OfficialTnbClassSelector\\Tnb_" + ObjectManager.Me.WowClass + "Healing.dll";
            else
                pathToCombatClassFile = Application.StartupPath + "\\HealerClasses\\" + nManagerSetting.CurrentSetting.HealerClass;
            HealerClass.ShowConfigurationHealerClass(pathToCombatClassFile);
        }
    }
}