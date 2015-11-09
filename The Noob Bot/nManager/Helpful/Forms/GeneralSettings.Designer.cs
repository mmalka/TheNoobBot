using System.Windows.Forms;
using nManager.Helpful.Forms.UserControls;

namespace nManager.Helpful.Forms
{
    partial class GeneralSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GeneralSettings));
            this.labelsToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.closeB = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.resetB = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.saveAndCloseB = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.MainPanel = new nManager.Helpful.Forms.UserControls.TnbRibbonManager();
            this.SpellManagementSystemPanelName = new nManager.Helpful.Forms.UserControls.TnbExpendablePanel();
            this.HealerClass = new nManager.Helpful.Forms.UserControls.TnbComboBox();
            this.CombatClass = new nManager.Helpful.Forms.UserControls.TnbComboBox();
            this.BecomeApprenticeOfSecondarySkillsWhileQuestingLabel = new System.Windows.Forms.Label();
            this.BecomeApprenticeOfSecondarySkillsWhileQuesting = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.BecomeApprenticeIfNeededByProductLabel = new System.Windows.Forms.Label();
            this.BecomeApprenticeIfNeededByProduct = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.TrainMountingCapacityLabel = new System.Windows.Forms.Label();
            this.TrainMountingCapacity = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.OnlyTrainIfWeHave2TimesMoreMoneyThanOurWishListSumLabel = new System.Windows.Forms.Label();
            this.OnlyTrainIfWeHave2TimesMoreMoneyThanOurWishListSum = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.OnlyTrainCurrentlyUsedSkillsLabel = new System.Windows.Forms.Label();
            this.OnlyTrainCurrentlyUsedSkills = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.HealerClassResetSettingsButton = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.HealerClassLabel = new System.Windows.Forms.Label();
            this.HealerClassSettingsButton = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.CombatClassResetSettingsButton = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.UseSpiritHealerLabel = new System.Windows.Forms.Label();
            this.UseSpiritHealer = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.ActivateSkillsAutoTrainingLabel = new System.Windows.Forms.Label();
            this.ActivateSkillsAutoTraining = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.DontPullMonstersLabel = new System.Windows.Forms.Label();
            this.DontPullMonsters = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.CanPullUnitsAlreadyInFightLabel = new System.Windows.Forms.Label();
            this.CanPullUnitsAlreadyInFight = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.AutoAssignTalentsLabel = new System.Windows.Forms.Label();
            this.AutoAssignTalents = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.CombatClassLabel = new System.Windows.Forms.Label();
            this.CombatClassSettingsButton = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.PluginsManagementSystemPanelName = new nManager.Helpful.Forms.UserControls.TnbExpendablePanel();
            this.ActivatedPluginResetSettings = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.ActivatedPluginSettings = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.DeactivatePlugin = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.ActivatePlugin = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.LaunchExpiredPlugins = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.ActivatePluginsSystem = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.LaunchExpiredPluginsLabel = new System.Windows.Forms.Label();
            this.ActivatePluginsSystemLabel = new System.Windows.Forms.Label();
            this.ActivatedPluginsListLabel = new System.Windows.Forms.Label();
            this.ActivatedPluginsList = new System.Windows.Forms.ListBox();
            this.AvailablePluginsListLabel = new System.Windows.Forms.Label();
            this.AvailablePluginsList = new System.Windows.Forms.ListBox();
            this.MountManagementPanelName = new nManager.Helpful.Forms.UserControls.TnbExpendablePanel();
            this.AquaticMountName = new System.Windows.Forms.TextBox();
            this.AquaticMountNameLabel = new System.Windows.Forms.Label();
            this.IgnoreFightIfMountedLabel = new System.Windows.Forms.Label();
            this.IgnoreFightIfMounted = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.MinimumDistanceToUseMount = new System.Windows.Forms.NumericUpDown();
            this.MinimumDistanceToUseMountLabel = new System.Windows.Forms.Label();
            this.FlyingMountName = new System.Windows.Forms.TextBox();
            this.FlyingMountNameLabel = new System.Windows.Forms.Label();
            this.GroundMountName = new System.Windows.Forms.TextBox();
            this.GroundMountNameLabel = new System.Windows.Forms.Label();
            this.UseGroundMountLabel = new System.Windows.Forms.Label();
            this.UseGroundMount = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.LootingFarmingManagementPanelName = new nManager.Helpful.Forms.UserControls.TnbExpendablePanel();
            this.UseLootARangeLabel = new System.Windows.Forms.Label();
            this.UseLootARange = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.ActivateLootStatisticsLabel = new System.Windows.Forms.Label();
            this.ActivateLootStatistics = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.DontHarvestTheFollowingObjectsHelper = new System.Windows.Forms.Button();
            this.DontHarvestTheFollowingObjects = new System.Windows.Forms.TextBox();
            this.AutoConfirmOnBoPItems = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.AutoConfirmOnBoPItemsLabel = new System.Windows.Forms.Label();
            this.OnlyUseMillingInTownLabel = new System.Windows.Forms.Label();
            this.OnlyUseMillingInTown = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.TimeBetweenEachMillingAttempt = new System.Windows.Forms.NumericUpDown();
            this.TimeBetweenEachMillingAttemptLabel = new System.Windows.Forms.Label();
            this.ActivateAutoMillingLabel = new System.Windows.Forms.Label();
            this.ActivateAutoMilling = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.HerbsToBeMilled = new System.Windows.Forms.TextBox();
            this.HerbsToBeMilledLabel = new System.Windows.Forms.Label();
            this.MakeStackOfElementalsItemsLabel = new System.Windows.Forms.Label();
            this.MakeStackOfElementalsItems = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.OnlyUseProspectingInTownLabel = new System.Windows.Forms.Label();
            this.OnlyUseProspectingInTown = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.TimeBetweenEachProspectingAttempt = new System.Windows.Forms.NumericUpDown();
            this.TimeBetweenEachProspectingAttemptLabel = new System.Windows.Forms.Label();
            this.ActivateAutoProspectingLabel = new System.Windows.Forms.Label();
            this.ActivateAutoProspecting = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.MineralsToProspect = new System.Windows.Forms.TextBox();
            this.MineralsToProspectLabel = new System.Windows.Forms.Label();
            this.labelX61 = new System.Windows.Forms.Label();
            this.ActivateAutoSmelting = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.DontHarvestTheFollowingObjectsLabel = new System.Windows.Forms.Label();
            this.addBlackListHarvest = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.HarvestDuringLongDistanceMovementsLabel = new System.Windows.Forms.Label();
            this.HarvestDuringLongDistanceMovements = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.labelX23 = new System.Windows.Forms.Label();
            this.BeastNinjaSkinning = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.GatheringSearchRadius = new System.Windows.Forms.NumericUpDown();
            this.GatheringSearchRadiusLabel = new System.Windows.Forms.Label();
            this.DontHarvestIfMoreThanXUnitInAggroRange = new System.Windows.Forms.NumericUpDown();
            this.DontHarvestIfMoreThanXUnitInAggroRangeLabel = new System.Windows.Forms.Label();
            this.ActivateHerbsHarvestingLabel = new System.Windows.Forms.Label();
            this.ActivateHerbsHarvesting = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.ActivateVeinsHarvestingLabel = new System.Windows.Forms.Label();
            this.ActivateVeinsHarvesting = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.ActivateBeastSkinningLabel = new System.Windows.Forms.Label();
            this.ActivateBeastSkinning = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.labelX16 = new System.Windows.Forms.Label();
            this.ActivateChestLooting = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.DontHarvestIfPlayerNearRadius = new System.Windows.Forms.NumericUpDown();
            this.DontHarvestIfPlayerNearRadiusLabel = new System.Windows.Forms.Label();
            this.ActivateMonsterLootingLabel = new System.Windows.Forms.Label();
            this.ActivateMonsterLooting = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.NPCsRepairSellBuyPanelName = new nManager.Helpful.Forms.UserControls.TnbExpendablePanel();
            this.SellPurple = new System.Windows.Forms.CheckBox();
            this.SellBlue = new System.Windows.Forms.CheckBox();
            this.SellGreen = new System.Windows.Forms.CheckBox();
            this.SellWhite = new System.Windows.Forms.CheckBox();
            this.SellGray = new System.Windows.Forms.CheckBox();
            this.UseRobotLabel = new System.Windows.Forms.Label();
            this.UseRobot = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.SellItemsWhenLessThanXSlotLeft = new System.Windows.Forms.NumericUpDown();
            this.RepairWhenDurabilityIsUnderPercent = new System.Windows.Forms.NumericUpDown();
            this.SellItemsWhenLessThanXSlotLeftLabel = new System.Windows.Forms.Label();
            this.RepairWhenDurabilityIsUnderPercentLabel = new System.Windows.Forms.Label();
            this.ForceToSellTheseItems = new System.Windows.Forms.TextBox();
            this.ForceToSellTheseItemsLabel = new System.Windows.Forms.Label();
            this.labelX52 = new System.Windows.Forms.Label();
            this.ActivateAutoSellingFeature = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.ActivateAutoRepairFeatureLabel = new System.Windows.Forms.Label();
            this.ActivateAutoRepairFeature = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.NumberOfFoodsWeGot = new System.Windows.Forms.NumericUpDown();
            this.NumberOfFoodsWeGotLabel = new System.Windows.Forms.Label();
            this.NumberOfBeverageWeGot = new System.Windows.Forms.NumericUpDown();
            this.NumberOfBeverageWeGotLabel = new System.Windows.Forms.Label();
            this.DontSellTheseItems = new System.Windows.Forms.TextBox();
            this.DontSellTheseItemsLabel = new System.Windows.Forms.Label();
            this.MailsManagementPanelName = new nManager.Helpful.Forms.UserControls.TnbExpendablePanel();
            this.MailPurple = new System.Windows.Forms.CheckBox();
            this.MailBlue = new System.Windows.Forms.CheckBox();
            this.MailGreen = new System.Windows.Forms.CheckBox();
            this.MailWhite = new System.Windows.Forms.CheckBox();
            this.MailGray = new System.Windows.Forms.CheckBox();
            this.UseMollELabel = new System.Windows.Forms.Label();
            this.UseMollE = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.SendMailWhenLessThanXSlotLeft = new System.Windows.Forms.NumericUpDown();
            this.SendMailWhenLessThanXSlotLeftLabel = new System.Windows.Forms.Label();
            this.MaillingFeatureRecipient = new System.Windows.Forms.TextBox();
            this.MaillingFeatureRecipientLabel = new System.Windows.Forms.Label();
            this.MaillingFeatureSubject = new System.Windows.Forms.TextBox();
            this.ForceToMailTheseItems = new System.Windows.Forms.TextBox();
            this.ForceToMailTheseItemsLabel = new System.Windows.Forms.Label();
            this.MaillingFeatureSubjectLabel = new System.Windows.Forms.Label();
            this.ActivateAutoMaillingFeatureLabel = new System.Windows.Forms.Label();
            this.ActivateAutoMaillingFeature = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.DontMailTheseItems = new System.Windows.Forms.TextBox();
            this.DontMailTheseItemsLabel = new System.Windows.Forms.Label();
            this.RegenerationManagementPanelName = new nManager.Helpful.Forms.UserControls.TnbExpendablePanel();
            this.DoRegenManaIfLow = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.labelX9 = new System.Windows.Forms.Label();
            this.labelX10 = new System.Windows.Forms.Label();
            this.DrinkBeverageWhenManaIsUnderXPercent = new System.Windows.Forms.NumericUpDown();
            this.BeverageName = new System.Windows.Forms.TextBox();
            this.BeverageNameLabel = new System.Windows.Forms.Label();
            this.labelX14 = new System.Windows.Forms.Label();
            this.labelX13 = new System.Windows.Forms.Label();
            this.EatFoodWhenHealthIsUnderXPercent = new System.Windows.Forms.NumericUpDown();
            this.FoodName = new System.Windows.Forms.TextBox();
            this.FoodNameLabel = new System.Windows.Forms.Label();
            this.SecuritySystemPanelName = new nManager.Helpful.Forms.UserControls.TnbExpendablePanel();
            this.UseHearthstoneLabel = new System.Windows.Forms.Label();
            this.ActiveStopTNBAfterXMinutes = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.ActiveStopTNBAfterXStucks = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.ActiveStopTNBIfReceivedAtMostXWhispers = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.ActiveStopTNBAfterXLevelup = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.UseHearthstone = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.SecuritySystemLabel = new System.Windows.Forms.Label();
            this.PlayASongIfNewWhispReceived = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.CloseGameLabel = new System.Windows.Forms.Label();
            this.labelX33 = new System.Windows.Forms.Label();
            this.RecordWhispsInLogFilesLabel = new System.Windows.Forms.Label();
            this.labelX32 = new System.Windows.Forms.Label();
            this.labelX31 = new System.Windows.Forms.Label();
            this.RecordWhispsInLogFiles = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.StopTNBIfPlayerHaveBeenTeleportedLabel = new System.Windows.Forms.Label();
            this.StopTNBIfPlayerHaveBeenTeleported = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.PauseTNBIfNearByPlayerLabel = new System.Windows.Forms.Label();
            this.StopTNBIfHonorPointsLimitReachedLabel = new System.Windows.Forms.Label();
            this.PauseTNBIfNearByPlayer = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.StopTNBIfHonorPointsLimitReached = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.StopTNBAfterXMinutes = new System.Windows.Forms.NumericUpDown();
            this.StopTNBAfterXMinutesLabel = new System.Windows.Forms.Label();
            this.StopTNBAfterXStucks = new System.Windows.Forms.NumericUpDown();
            this.StopTNBAfterXStucksLabel = new System.Windows.Forms.Label();
            this.StopTNBIfReceivedAtMostXWhispers = new System.Windows.Forms.NumericUpDown();
            this.StopTNBIfReceivedAtMostXWhispersLabel = new System.Windows.Forms.Label();
            this.StopTNBAfterXLevelup = new System.Windows.Forms.NumericUpDown();
            this.StopTNBAfterXLevelupLabel = new System.Windows.Forms.Label();
            this.StopTNBIfBagAreFullLabel = new System.Windows.Forms.Label();
            this.StopTNBIfBagAreFull = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.ReloggerManagementPanelName = new nManager.Helpful.Forms.UserControls.TnbExpendablePanel();
            this.BattleNetSubAccount = new System.Windows.Forms.TextBox();
            this.BattleNetSubAccountLabel = new System.Windows.Forms.Label();
            this.ActivateReloggerFeatureLabel = new System.Windows.Forms.Label();
            this.ActivateReloggerFeature = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.PasswordOfTheBattleNetAccount = new System.Windows.Forms.TextBox();
            this.PasswordOfTheBattleNetAccountLabel = new System.Windows.Forms.Label();
            this.EmailOfTheBattleNetAccount = new System.Windows.Forms.TextBox();
            this.EmailOfTheBattleNetAccountLabel = new System.Windows.Forms.Label();
            this.MimesisBroadcasterSettingsPanel = new nManager.Helpful.Forms.UserControls.TnbExpendablePanel();
            this.BroadcastingPort = new System.Windows.Forms.NumericUpDown();
            this.BroadcastingIPWan = new System.Windows.Forms.Label();
            this.BroadcastingIPLan = new System.Windows.Forms.Label();
            this.BroadcastingIPLocal = new System.Windows.Forms.Label();
            this.BroadcastingPortDefaultLabel = new System.Windows.Forms.Label();
            this.BroadcastingIPWanLabel = new System.Windows.Forms.Label();
            this.BroadcastingIPLanLabel = new System.Windows.Forms.Label();
            this.ActivateBroadcastingMimesisLabel = new System.Windows.Forms.Label();
            this.ActivateBroadcastingMimesis = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.BroadcastingIPLocalLabel = new System.Windows.Forms.Label();
            this.BroadcastingPortLabel = new System.Windows.Forms.Label();
            this.AdvancedSettingsPanelName = new nManager.Helpful.Forms.UserControls.TnbExpendablePanel();
            this.UseFrameLockLabel = new System.Windows.Forms.Label();
            this.UseFrameLock = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.HideSDKFilesLabel = new System.Windows.Forms.Label();
            this.HideSDKFiles = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.AutoCloseChatFrameLabel = new System.Windows.Forms.Label();
            this.AutoCloseChatFrame = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.AlwaysOnTopFeatureLabel = new System.Windows.Forms.Label();
            this.ActivateAlwaysOnTopFeature = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.AllowTNBToSetYourMaxFpsLabel = new System.Windows.Forms.Label();
            this.AllowTNBToSetYourMaxFps = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.MaxDistanceToGoToMailboxesOrNPCs = new System.Windows.Forms.NumericUpDown();
            this.MaxDistanceToGoToMailboxesOrNPCsLabel = new System.Windows.Forms.Label();
            this.ActivatePathFindingFeatureLabel = new System.Windows.Forms.Label();
            this.ActivatePathFindingFeature = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.MainHeader = new nManager.Helpful.Forms.UserControls.TnbControlMenu();
            this.MainPanel.SuspendLayout();
            this.SpellManagementSystemPanelName.SuspendLayout();
            this.PluginsManagementSystemPanelName.SuspendLayout();
            this.MountManagementPanelName.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MinimumDistanceToUseMount)).BeginInit();
            this.LootingFarmingManagementPanelName.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TimeBetweenEachMillingAttempt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TimeBetweenEachProspectingAttempt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GatheringSearchRadius)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DontHarvestIfMoreThanXUnitInAggroRange)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DontHarvestIfPlayerNearRadius)).BeginInit();
            this.NPCsRepairSellBuyPanelName.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SellItemsWhenLessThanXSlotLeft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RepairWhenDurabilityIsUnderPercent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumberOfFoodsWeGot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumberOfBeverageWeGot)).BeginInit();
            this.MailsManagementPanelName.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SendMailWhenLessThanXSlotLeft)).BeginInit();
            this.RegenerationManagementPanelName.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DrinkBeverageWhenManaIsUnderXPercent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EatFoodWhenHealthIsUnderXPercent)).BeginInit();
            this.SecuritySystemPanelName.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.StopTNBAfterXMinutes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StopTNBAfterXStucks)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StopTNBIfReceivedAtMostXWhispers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StopTNBAfterXLevelup)).BeginInit();
            this.ReloggerManagementPanelName.SuspendLayout();
            this.MimesisBroadcasterSettingsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BroadcastingPort)).BeginInit();
            this.AdvancedSettingsPanelName.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MaxDistanceToGoToMailboxesOrNPCs)).BeginInit();
            this.SuspendLayout();
            // 
            // closeB
            // 
            this.closeB.AutoEllipsis = true;
            this.closeB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.closeB.ForeColor = System.Drawing.Color.Snow;
            this.closeB.HooverImage = global::nManager.Properties.Resources.greenB_150;
            this.closeB.Image = global::nManager.Properties.Resources.blackB_150;
            this.closeB.Location = new System.Drawing.Point(93, 414);
            this.closeB.Name = "closeB";
            this.closeB.Size = new System.Drawing.Size(150, 29);
            this.closeB.TabIndex = 6;
            this.closeB.Text = "Close without save";
            this.closeB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.closeB.Click += new System.EventHandler(this.closeB_Click);
            // 
            // resetB
            // 
            this.resetB.AutoEllipsis = true;
            this.resetB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.resetB.ForeColor = System.Drawing.Color.Snow;
            this.resetB.HooverImage = global::nManager.Properties.Resources.greenB_150;
            this.resetB.Image = global::nManager.Properties.Resources.blackB_150;
            this.resetB.Location = new System.Drawing.Point(253, 414);
            this.resetB.Name = "resetB";
            this.resetB.Size = new System.Drawing.Size(150, 29);
            this.resetB.TabIndex = 5;
            this.resetB.Text = "Reset Settings";
            this.resetB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.resetB.Click += new System.EventHandler(this.resetB_Click);
            // 
            // saveAndCloseB
            // 
            this.saveAndCloseB.AutoEllipsis = true;
            this.saveAndCloseB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.saveAndCloseB.ForeColor = System.Drawing.Color.Snow;
            this.saveAndCloseB.HooverImage = global::nManager.Properties.Resources.greenB_150;
            this.saveAndCloseB.Image = global::nManager.Properties.Resources.blueB_150;
            this.saveAndCloseB.Location = new System.Drawing.Point(413, 414);
            this.saveAndCloseB.Name = "saveAndCloseB";
            this.saveAndCloseB.Size = new System.Drawing.Size(150, 29);
            this.saveAndCloseB.TabIndex = 4;
            this.saveAndCloseB.Text = "Save and Close";
            this.saveAndCloseB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.saveAndCloseB.Click += new System.EventHandler(this.saveAndCloseB_Click);
            // 
            // MainPanel
            // 
            this.MainPanel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.MainPanel.AutoScroll = true;
            this.MainPanel.AutoScrollMinSize = new System.Drawing.Size(0, 361);
            this.MainPanel.BackColor = System.Drawing.Color.Transparent;
            this.MainPanel.Controls.Add(this.SpellManagementSystemPanelName);
            this.MainPanel.Controls.Add(this.PluginsManagementSystemPanelName);
            this.MainPanel.Controls.Add(this.MountManagementPanelName);
            this.MainPanel.Controls.Add(this.LootingFarmingManagementPanelName);
            this.MainPanel.Controls.Add(this.NPCsRepairSellBuyPanelName);
            this.MainPanel.Controls.Add(this.MailsManagementPanelName);
            this.MainPanel.Controls.Add(this.RegenerationManagementPanelName);
            this.MainPanel.Controls.Add(this.SecuritySystemPanelName);
            this.MainPanel.Controls.Add(this.ReloggerManagementPanelName);
            this.MainPanel.Controls.Add(this.MimesisBroadcasterSettingsPanel);
            this.MainPanel.Controls.Add(this.AdvancedSettingsPanelName);
            this.MainPanel.ForeColor = System.Drawing.Color.Black;
            this.MainPanel.Location = new System.Drawing.Point(1, 43);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(573, 360);
            this.MainPanel.TabIndex = 3;
            // 
            // SpellManagementSystemPanelName
            // 
            this.SpellManagementSystemPanelName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.SpellManagementSystemPanelName.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.SpellManagementSystemPanelName.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.SpellManagementSystemPanelName.ContentSize = new System.Drawing.Size(556, 225);
            this.SpellManagementSystemPanelName.Controls.Add(this.HealerClass);
            this.SpellManagementSystemPanelName.Controls.Add(this.CombatClass);
            this.SpellManagementSystemPanelName.Controls.Add(this.BecomeApprenticeOfSecondarySkillsWhileQuestingLabel);
            this.SpellManagementSystemPanelName.Controls.Add(this.BecomeApprenticeOfSecondarySkillsWhileQuesting);
            this.SpellManagementSystemPanelName.Controls.Add(this.BecomeApprenticeIfNeededByProductLabel);
            this.SpellManagementSystemPanelName.Controls.Add(this.BecomeApprenticeIfNeededByProduct);
            this.SpellManagementSystemPanelName.Controls.Add(this.TrainMountingCapacityLabel);
            this.SpellManagementSystemPanelName.Controls.Add(this.TrainMountingCapacity);
            this.SpellManagementSystemPanelName.Controls.Add(this.OnlyTrainIfWeHave2TimesMoreMoneyThanOurWishListSumLabel);
            this.SpellManagementSystemPanelName.Controls.Add(this.OnlyTrainIfWeHave2TimesMoreMoneyThanOurWishListSum);
            this.SpellManagementSystemPanelName.Controls.Add(this.OnlyTrainCurrentlyUsedSkillsLabel);
            this.SpellManagementSystemPanelName.Controls.Add(this.OnlyTrainCurrentlyUsedSkills);
            this.SpellManagementSystemPanelName.Controls.Add(this.HealerClassResetSettingsButton);
            this.SpellManagementSystemPanelName.Controls.Add(this.HealerClassLabel);
            this.SpellManagementSystemPanelName.Controls.Add(this.HealerClassSettingsButton);
            this.SpellManagementSystemPanelName.Controls.Add(this.CombatClassResetSettingsButton);
            this.SpellManagementSystemPanelName.Controls.Add(this.UseSpiritHealerLabel);
            this.SpellManagementSystemPanelName.Controls.Add(this.UseSpiritHealer);
            this.SpellManagementSystemPanelName.Controls.Add(this.ActivateSkillsAutoTrainingLabel);
            this.SpellManagementSystemPanelName.Controls.Add(this.ActivateSkillsAutoTraining);
            this.SpellManagementSystemPanelName.Controls.Add(this.DontPullMonstersLabel);
            this.SpellManagementSystemPanelName.Controls.Add(this.DontPullMonsters);
            this.SpellManagementSystemPanelName.Controls.Add(this.CanPullUnitsAlreadyInFightLabel);
            this.SpellManagementSystemPanelName.Controls.Add(this.CanPullUnitsAlreadyInFight);
            this.SpellManagementSystemPanelName.Controls.Add(this.AutoAssignTalentsLabel);
            this.SpellManagementSystemPanelName.Controls.Add(this.AutoAssignTalents);
            this.SpellManagementSystemPanelName.Controls.Add(this.CombatClassLabel);
            this.SpellManagementSystemPanelName.Controls.Add(this.CombatClassSettingsButton);
            this.SpellManagementSystemPanelName.Fold = true;
            this.SpellManagementSystemPanelName.FolderImage = ((System.Drawing.Image)(resources.GetObject("SpellManagementSystemPanelName.FolderImage")));
            this.SpellManagementSystemPanelName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.SpellManagementSystemPanelName.HeaderBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.SpellManagementSystemPanelName.HeaderImage = ((System.Drawing.Image)(resources.GetObject("SpellManagementSystemPanelName.HeaderImage")));
            this.SpellManagementSystemPanelName.HeaderSize = new System.Drawing.Size(556, 36);
            this.SpellManagementSystemPanelName.Location = new System.Drawing.Point(0, 0);
            this.SpellManagementSystemPanelName.Margin = new System.Windows.Forms.Padding(0);
            this.SpellManagementSystemPanelName.MaximumSize = new System.Drawing.Size(556, 0);
            this.SpellManagementSystemPanelName.Name = "SpellManagementSystemPanelName";
            this.SpellManagementSystemPanelName.OrderIndex = 1;
            this.SpellManagementSystemPanelName.Padding = new System.Windows.Forms.Padding(0, 0, 0, 12);
            this.SpellManagementSystemPanelName.Size = new System.Drawing.Size(556, 36);
            this.SpellManagementSystemPanelName.TabIndex = 50;
            this.SpellManagementSystemPanelName.TitleFont = new System.Drawing.Font("Segoe UI", 7.65F, System.Drawing.FontStyle.Bold);
            this.SpellManagementSystemPanelName.TitleForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.SpellManagementSystemPanelName.TitleText = "Spell Management System - Combat/Healer Class";
            this.SpellManagementSystemPanelName.UnfolderImage = ((System.Drawing.Image)(resources.GetObject("SpellManagementSystemPanelName.UnfolderImage")));
            // 
            // HealerClass
            // 
            this.HealerClass.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.HealerClass.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(121)))), ((int)(((byte)(121)))));
            this.HealerClass.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.HealerClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.HealerClass.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.HealerClass.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(160)))), ((int)(((byte)(229)))));
            this.HealerClass.HighlightColor = System.Drawing.Color.Gainsboro;
            this.HealerClass.ItemHeight = 20;
            this.HealerClass.Location = new System.Drawing.Point(170, 89);
            this.HealerClass.Name = "HealerClass";
            this.HealerClass.SelectorBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(106)))), ((int)(((byte)(194)))));
            this.HealerClass.SelectorImage = ((System.Drawing.Image)(resources.GetObject("HealerClass.SelectorImage")));
            this.HealerClass.Size = new System.Drawing.Size(121, 26);
            this.HealerClass.TabIndex = 31;
            // 
            // CombatClass
            // 
            this.CombatClass.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.CombatClass.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(121)))), ((int)(((byte)(121)))));
            this.CombatClass.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.CombatClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CombatClass.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CombatClass.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(160)))), ((int)(((byte)(229)))));
            this.CombatClass.HighlightColor = System.Drawing.Color.Gainsboro;
            this.CombatClass.ItemHeight = 20;
            this.CombatClass.Location = new System.Drawing.Point(170, 53);
            this.CombatClass.Name = "CombatClass";
            this.CombatClass.SelectorBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(106)))), ((int)(((byte)(194)))));
            this.CombatClass.SelectorImage = ((System.Drawing.Image)(resources.GetObject("CombatClass.SelectorImage")));
            this.CombatClass.Size = new System.Drawing.Size(121, 26);
            this.CombatClass.TabIndex = 2;
            // 
            // BecomeApprenticeOfSecondarySkillsWhileQuestingLabel
            // 
            this.BecomeApprenticeOfSecondarySkillsWhileQuestingLabel.BackColor = System.Drawing.Color.Transparent;
            this.BecomeApprenticeOfSecondarySkillsWhileQuestingLabel.ForeColor = System.Drawing.Color.Black;
            this.BecomeApprenticeOfSecondarySkillsWhileQuestingLabel.Location = new System.Drawing.Point(300, 234);
            this.BecomeApprenticeOfSecondarySkillsWhileQuestingLabel.Name = "BecomeApprenticeOfSecondarySkillsWhileQuestingLabel";
            this.BecomeApprenticeOfSecondarySkillsWhileQuestingLabel.Size = new System.Drawing.Size(154, 22);
            this.BecomeApprenticeOfSecondarySkillsWhileQuestingLabel.TabIndex = 43;
            this.BecomeApprenticeOfSecondarySkillsWhileQuestingLabel.Text = "Become apprentice of secondary skills while questing";
            // 
            // BecomeApprenticeOfSecondarySkillsWhileQuesting
            // 
            this.BecomeApprenticeOfSecondarySkillsWhileQuesting.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.BecomeApprenticeOfSecondarySkillsWhileQuesting.Location = new System.Drawing.Point(459, 234);
            this.BecomeApprenticeOfSecondarySkillsWhileQuesting.MaximumSize = new System.Drawing.Size(60, 20);
            this.BecomeApprenticeOfSecondarySkillsWhileQuesting.MinimumSize = new System.Drawing.Size(60, 20);
            this.BecomeApprenticeOfSecondarySkillsWhileQuesting.Name = "BecomeApprenticeOfSecondarySkillsWhileQuesting";
            this.BecomeApprenticeOfSecondarySkillsWhileQuesting.OffText = "OFF";
            this.BecomeApprenticeOfSecondarySkillsWhileQuesting.OnText = "ON";
            this.BecomeApprenticeOfSecondarySkillsWhileQuesting.Size = new System.Drawing.Size(60, 20);
            this.BecomeApprenticeOfSecondarySkillsWhileQuesting.TabIndex = 44;
            this.BecomeApprenticeOfSecondarySkillsWhileQuesting.Value = false;
            // 
            // BecomeApprenticeIfNeededByProductLabel
            // 
            this.BecomeApprenticeIfNeededByProductLabel.BackColor = System.Drawing.Color.Transparent;
            this.BecomeApprenticeIfNeededByProductLabel.ForeColor = System.Drawing.Color.Black;
            this.BecomeApprenticeIfNeededByProductLabel.Location = new System.Drawing.Point(12, 235);
            this.BecomeApprenticeIfNeededByProductLabel.Name = "BecomeApprenticeIfNeededByProductLabel";
            this.BecomeApprenticeIfNeededByProductLabel.Size = new System.Drawing.Size(154, 22);
            this.BecomeApprenticeIfNeededByProductLabel.TabIndex = 41;
            this.BecomeApprenticeIfNeededByProductLabel.Text = "Become apprentice if needed by product";
            // 
            // BecomeApprenticeIfNeededByProduct
            // 
            this.BecomeApprenticeIfNeededByProduct.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.BecomeApprenticeIfNeededByProduct.Location = new System.Drawing.Point(171, 235);
            this.BecomeApprenticeIfNeededByProduct.MaximumSize = new System.Drawing.Size(60, 20);
            this.BecomeApprenticeIfNeededByProduct.MinimumSize = new System.Drawing.Size(60, 20);
            this.BecomeApprenticeIfNeededByProduct.Name = "BecomeApprenticeIfNeededByProduct";
            this.BecomeApprenticeIfNeededByProduct.OffText = "OFF";
            this.BecomeApprenticeIfNeededByProduct.OnText = "ON";
            this.BecomeApprenticeIfNeededByProduct.Size = new System.Drawing.Size(60, 20);
            this.BecomeApprenticeIfNeededByProduct.TabIndex = 45;
            this.BecomeApprenticeIfNeededByProduct.Value = false;
            // 
            // TrainMountingCapacityLabel
            // 
            this.TrainMountingCapacityLabel.BackColor = System.Drawing.Color.Transparent;
            this.TrainMountingCapacityLabel.ForeColor = System.Drawing.Color.Black;
            this.TrainMountingCapacityLabel.Location = new System.Drawing.Point(300, 207);
            this.TrainMountingCapacityLabel.Name = "TrainMountingCapacityLabel";
            this.TrainMountingCapacityLabel.Size = new System.Drawing.Size(154, 22);
            this.TrainMountingCapacityLabel.TabIndex = 39;
            this.TrainMountingCapacityLabel.Text = "Train mounting capacity";
            // 
            // TrainMountingCapacity
            // 
            this.TrainMountingCapacity.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.TrainMountingCapacity.Location = new System.Drawing.Point(459, 207);
            this.TrainMountingCapacity.MaximumSize = new System.Drawing.Size(60, 20);
            this.TrainMountingCapacity.MinimumSize = new System.Drawing.Size(60, 20);
            this.TrainMountingCapacity.Name = "TrainMountingCapacity";
            this.TrainMountingCapacity.OffText = "OFF";
            this.TrainMountingCapacity.OnText = "ON";
            this.TrainMountingCapacity.Size = new System.Drawing.Size(60, 20);
            this.TrainMountingCapacity.TabIndex = 46;
            this.TrainMountingCapacity.Value = false;
            // 
            // OnlyTrainIfWeHave2TimesMoreMoneyThanOurWishListSumLabel
            // 
            this.OnlyTrainIfWeHave2TimesMoreMoneyThanOurWishListSumLabel.BackColor = System.Drawing.Color.Transparent;
            this.OnlyTrainIfWeHave2TimesMoreMoneyThanOurWishListSumLabel.ForeColor = System.Drawing.Color.Black;
            this.OnlyTrainIfWeHave2TimesMoreMoneyThanOurWishListSumLabel.Location = new System.Drawing.Point(11, 207);
            this.OnlyTrainIfWeHave2TimesMoreMoneyThanOurWishListSumLabel.Name = "OnlyTrainIfWeHave2TimesMoreMoneyThanOurWishListSumLabel";
            this.OnlyTrainIfWeHave2TimesMoreMoneyThanOurWishListSumLabel.Size = new System.Drawing.Size(154, 22);
            this.OnlyTrainIfWeHave2TimesMoreMoneyThanOurWishListSumLabel.TabIndex = 37;
            this.OnlyTrainIfWeHave2TimesMoreMoneyThanOurWishListSumLabel.Text = "Only train if we have 2 times more money than required";
            // 
            // OnlyTrainIfWeHave2TimesMoreMoneyThanOurWishListSum
            // 
            this.OnlyTrainIfWeHave2TimesMoreMoneyThanOurWishListSum.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.OnlyTrainIfWeHave2TimesMoreMoneyThanOurWishListSum.Location = new System.Drawing.Point(170, 207);
            this.OnlyTrainIfWeHave2TimesMoreMoneyThanOurWishListSum.MaximumSize = new System.Drawing.Size(60, 20);
            this.OnlyTrainIfWeHave2TimesMoreMoneyThanOurWishListSum.MinimumSize = new System.Drawing.Size(60, 20);
            this.OnlyTrainIfWeHave2TimesMoreMoneyThanOurWishListSum.Name = "OnlyTrainIfWeHave2TimesMoreMoneyThanOurWishListSum";
            this.OnlyTrainIfWeHave2TimesMoreMoneyThanOurWishListSum.OffText = "OFF";
            this.OnlyTrainIfWeHave2TimesMoreMoneyThanOurWishListSum.OnText = "ON";
            this.OnlyTrainIfWeHave2TimesMoreMoneyThanOurWishListSum.Size = new System.Drawing.Size(60, 20);
            this.OnlyTrainIfWeHave2TimesMoreMoneyThanOurWishListSum.TabIndex = 47;
            this.OnlyTrainIfWeHave2TimesMoreMoneyThanOurWishListSum.Value = false;
            // 
            // OnlyTrainCurrentlyUsedSkillsLabel
            // 
            this.OnlyTrainCurrentlyUsedSkillsLabel.BackColor = System.Drawing.Color.Transparent;
            this.OnlyTrainCurrentlyUsedSkillsLabel.ForeColor = System.Drawing.Color.Black;
            this.OnlyTrainCurrentlyUsedSkillsLabel.Location = new System.Drawing.Point(300, 179);
            this.OnlyTrainCurrentlyUsedSkillsLabel.Name = "OnlyTrainCurrentlyUsedSkillsLabel";
            this.OnlyTrainCurrentlyUsedSkillsLabel.Size = new System.Drawing.Size(154, 22);
            this.OnlyTrainCurrentlyUsedSkillsLabel.TabIndex = 35;
            this.OnlyTrainCurrentlyUsedSkillsLabel.Text = "Only train currently used skills";
            // 
            // OnlyTrainCurrentlyUsedSkills
            // 
            this.OnlyTrainCurrentlyUsedSkills.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.OnlyTrainCurrentlyUsedSkills.Location = new System.Drawing.Point(459, 179);
            this.OnlyTrainCurrentlyUsedSkills.MaximumSize = new System.Drawing.Size(60, 20);
            this.OnlyTrainCurrentlyUsedSkills.MinimumSize = new System.Drawing.Size(60, 20);
            this.OnlyTrainCurrentlyUsedSkills.Name = "OnlyTrainCurrentlyUsedSkills";
            this.OnlyTrainCurrentlyUsedSkills.OffText = "OFF";
            this.OnlyTrainCurrentlyUsedSkills.OnText = "ON";
            this.OnlyTrainCurrentlyUsedSkills.Size = new System.Drawing.Size(60, 20);
            this.OnlyTrainCurrentlyUsedSkills.TabIndex = 48;
            this.OnlyTrainCurrentlyUsedSkills.Value = false;
            // 
            // HealerClassResetSettingsButton
            // 
            this.HealerClassResetSettingsButton.AutoEllipsis = true;
            this.HealerClassResetSettingsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.HealerClassResetSettingsButton.ForeColor = System.Drawing.Color.Snow;
            this.HealerClassResetSettingsButton.HooverImage = ((System.Drawing.Image)(resources.GetObject("HealerClassResetSettingsButton.HooverImage")));
            this.HealerClassResetSettingsButton.Image = ((System.Drawing.Image)(resources.GetObject("HealerClassResetSettingsButton.Image")));
            this.HealerClassResetSettingsButton.Location = new System.Drawing.Point(415, 85);
            this.HealerClassResetSettingsButton.Name = "HealerClassResetSettingsButton";
            this.HealerClassResetSettingsButton.Size = new System.Drawing.Size(106, 29);
            this.HealerClassResetSettingsButton.TabIndex = 49;
            this.HealerClassResetSettingsButton.Text = "Reset Settings";
            this.HealerClassResetSettingsButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.HealerClassResetSettingsButton.Click += new System.EventHandler(this.HealerClassResetSettingsButton_Click);
            // 
            // HealerClassLabel
            // 
            this.HealerClassLabel.BackColor = System.Drawing.Color.Transparent;
            this.HealerClassLabel.ForeColor = System.Drawing.Color.Black;
            this.HealerClassLabel.Location = new System.Drawing.Point(11, 88);
            this.HealerClassLabel.Name = "HealerClassLabel";
            this.HealerClassLabel.Size = new System.Drawing.Size(154, 22);
            this.HealerClassLabel.TabIndex = 32;
            this.HealerClassLabel.Text = "Healer Class";
            // 
            // HealerClassSettingsButton
            // 
            this.HealerClassSettingsButton.AutoEllipsis = true;
            this.HealerClassSettingsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.HealerClassSettingsButton.ForeColor = System.Drawing.Color.Snow;
            this.HealerClassSettingsButton.HooverImage = global::nManager.Properties.Resources.greenB_70;
            this.HealerClassSettingsButton.Image = global::nManager.Properties.Resources.blueB_70;
            this.HealerClassSettingsButton.Location = new System.Drawing.Point(328, 86);
            this.HealerClassSettingsButton.Name = "HealerClassSettingsButton";
            this.HealerClassSettingsButton.Size = new System.Drawing.Size(70, 29);
            this.HealerClassSettingsButton.TabIndex = 50;
            this.HealerClassSettingsButton.Text = "Settings";
            this.HealerClassSettingsButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.HealerClassSettingsButton.Click += new System.EventHandler(this.HealerClassSettingsButton_Click);
            // 
            // CombatClassResetSettingsButton
            // 
            this.CombatClassResetSettingsButton.AutoEllipsis = true;
            this.CombatClassResetSettingsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.CombatClassResetSettingsButton.ForeColor = System.Drawing.Color.Snow;
            this.CombatClassResetSettingsButton.HooverImage = ((System.Drawing.Image)(resources.GetObject("CombatClassResetSettingsButton.HooverImage")));
            this.CombatClassResetSettingsButton.Image = global::nManager.Properties.Resources.blackB;
            this.CombatClassResetSettingsButton.Location = new System.Drawing.Point(415, 51);
            this.CombatClassResetSettingsButton.Name = "CombatClassResetSettingsButton";
            this.CombatClassResetSettingsButton.Size = new System.Drawing.Size(106, 29);
            this.CombatClassResetSettingsButton.TabIndex = 51;
            this.CombatClassResetSettingsButton.Text = "Reset Settings";
            this.CombatClassResetSettingsButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.CombatClassResetSettingsButton.Click += new System.EventHandler(this.CombatClassResetSettingsButton_Click);
            // 
            // UseSpiritHealerLabel
            // 
            this.UseSpiritHealerLabel.BackColor = System.Drawing.Color.Transparent;
            this.UseSpiritHealerLabel.ForeColor = System.Drawing.Color.Black;
            this.UseSpiritHealerLabel.Location = new System.Drawing.Point(11, 123);
            this.UseSpiritHealerLabel.Name = "UseSpiritHealerLabel";
            this.UseSpiritHealerLabel.Size = new System.Drawing.Size(154, 22);
            this.UseSpiritHealerLabel.TabIndex = 28;
            this.UseSpiritHealerLabel.Text = "Use Spirit Healer";
            // 
            // UseSpiritHealer
            // 
            this.UseSpiritHealer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.UseSpiritHealer.Location = new System.Drawing.Point(170, 123);
            this.UseSpiritHealer.MaximumSize = new System.Drawing.Size(60, 20);
            this.UseSpiritHealer.MinimumSize = new System.Drawing.Size(60, 20);
            this.UseSpiritHealer.Name = "UseSpiritHealer";
            this.UseSpiritHealer.OffText = "OFF";
            this.UseSpiritHealer.OnText = "ON";
            this.UseSpiritHealer.Size = new System.Drawing.Size(60, 20);
            this.UseSpiritHealer.TabIndex = 49;
            this.UseSpiritHealer.Value = false;
            // 
            // ActivateSkillsAutoTrainingLabel
            // 
            this.ActivateSkillsAutoTrainingLabel.BackColor = System.Drawing.Color.Transparent;
            this.ActivateSkillsAutoTrainingLabel.ForeColor = System.Drawing.Color.Black;
            this.ActivateSkillsAutoTrainingLabel.Location = new System.Drawing.Point(12, 179);
            this.ActivateSkillsAutoTrainingLabel.Name = "ActivateSkillsAutoTrainingLabel";
            this.ActivateSkillsAutoTrainingLabel.Size = new System.Drawing.Size(154, 22);
            this.ActivateSkillsAutoTrainingLabel.TabIndex = 24;
            this.ActivateSkillsAutoTrainingLabel.Text = "Activate Skill Auto Training";
            // 
            // ActivateSkillsAutoTraining
            // 
            this.ActivateSkillsAutoTraining.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.ActivateSkillsAutoTraining.Location = new System.Drawing.Point(171, 179);
            this.ActivateSkillsAutoTraining.MaximumSize = new System.Drawing.Size(60, 20);
            this.ActivateSkillsAutoTraining.MinimumSize = new System.Drawing.Size(60, 20);
            this.ActivateSkillsAutoTraining.Name = "ActivateSkillsAutoTraining";
            this.ActivateSkillsAutoTraining.OffText = "OFF";
            this.ActivateSkillsAutoTraining.OnText = "ON";
            this.ActivateSkillsAutoTraining.Size = new System.Drawing.Size(60, 20);
            this.ActivateSkillsAutoTraining.TabIndex = 50;
            this.ActivateSkillsAutoTraining.Value = false;
            // 
            // DontPullMonstersLabel
            // 
            this.DontPullMonstersLabel.BackColor = System.Drawing.Color.Transparent;
            this.DontPullMonstersLabel.ForeColor = System.Drawing.Color.Black;
            this.DontPullMonstersLabel.Location = new System.Drawing.Point(300, 151);
            this.DontPullMonstersLabel.Name = "DontPullMonstersLabel";
            this.DontPullMonstersLabel.Size = new System.Drawing.Size(154, 22);
            this.DontPullMonstersLabel.TabIndex = 9;
            this.DontPullMonstersLabel.Text = "Don\'t start fighting";
            // 
            // DontPullMonsters
            // 
            this.DontPullMonsters.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.DontPullMonsters.Location = new System.Drawing.Point(459, 151);
            this.DontPullMonsters.MaximumSize = new System.Drawing.Size(60, 20);
            this.DontPullMonsters.MinimumSize = new System.Drawing.Size(60, 20);
            this.DontPullMonsters.Name = "DontPullMonsters";
            this.DontPullMonsters.OffText = "OFF";
            this.DontPullMonsters.OnText = "ON";
            this.DontPullMonsters.Size = new System.Drawing.Size(60, 20);
            this.DontPullMonsters.TabIndex = 51;
            this.DontPullMonsters.Value = false;
            // 
            // CanPullUnitsAlreadyInFightLabel
            // 
            this.CanPullUnitsAlreadyInFightLabel.BackColor = System.Drawing.Color.Transparent;
            this.CanPullUnitsAlreadyInFightLabel.ForeColor = System.Drawing.Color.Black;
            this.CanPullUnitsAlreadyInFightLabel.Location = new System.Drawing.Point(300, 123);
            this.CanPullUnitsAlreadyInFightLabel.Name = "CanPullUnitsAlreadyInFightLabel";
            this.CanPullUnitsAlreadyInFightLabel.Size = new System.Drawing.Size(154, 22);
            this.CanPullUnitsAlreadyInFightLabel.TabIndex = 7;
            this.CanPullUnitsAlreadyInFightLabel.Text = "Can attack units already in fight";
            // 
            // CanPullUnitsAlreadyInFight
            // 
            this.CanPullUnitsAlreadyInFight.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.CanPullUnitsAlreadyInFight.Location = new System.Drawing.Point(459, 123);
            this.CanPullUnitsAlreadyInFight.MaximumSize = new System.Drawing.Size(60, 20);
            this.CanPullUnitsAlreadyInFight.MinimumSize = new System.Drawing.Size(60, 20);
            this.CanPullUnitsAlreadyInFight.Name = "CanPullUnitsAlreadyInFight";
            this.CanPullUnitsAlreadyInFight.OffText = "OFF";
            this.CanPullUnitsAlreadyInFight.OnText = "ON";
            this.CanPullUnitsAlreadyInFight.Size = new System.Drawing.Size(60, 20);
            this.CanPullUnitsAlreadyInFight.TabIndex = 52;
            this.CanPullUnitsAlreadyInFight.Value = false;
            // 
            // AutoAssignTalentsLabel
            // 
            this.AutoAssignTalentsLabel.BackColor = System.Drawing.Color.Transparent;
            this.AutoAssignTalentsLabel.ForeColor = System.Drawing.Color.Black;
            this.AutoAssignTalentsLabel.Location = new System.Drawing.Point(11, 151);
            this.AutoAssignTalentsLabel.Name = "AutoAssignTalentsLabel";
            this.AutoAssignTalentsLabel.Size = new System.Drawing.Size(154, 22);
            this.AutoAssignTalentsLabel.TabIndex = 5;
            this.AutoAssignTalentsLabel.Text = "Assign Talents";
            // 
            // AutoAssignTalents
            // 
            this.AutoAssignTalents.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.AutoAssignTalents.Location = new System.Drawing.Point(170, 152);
            this.AutoAssignTalents.MaximumSize = new System.Drawing.Size(60, 20);
            this.AutoAssignTalents.MinimumSize = new System.Drawing.Size(60, 20);
            this.AutoAssignTalents.Name = "AutoAssignTalents";
            this.AutoAssignTalents.OffText = "OFF";
            this.AutoAssignTalents.OnText = "ON";
            this.AutoAssignTalents.Size = new System.Drawing.Size(60, 20);
            this.AutoAssignTalents.TabIndex = 53;
            this.AutoAssignTalents.Value = false;
            // 
            // CombatClassLabel
            // 
            this.CombatClassLabel.BackColor = System.Drawing.Color.Transparent;
            this.CombatClassLabel.ForeColor = System.Drawing.Color.Black;
            this.CombatClassLabel.Location = new System.Drawing.Point(11, 52);
            this.CombatClassLabel.Name = "CombatClassLabel";
            this.CombatClassLabel.Size = new System.Drawing.Size(154, 22);
            this.CombatClassLabel.TabIndex = 3;
            this.CombatClassLabel.Text = "Combat Class";
            // 
            // CombatClassSettingsButton
            // 
            this.CombatClassSettingsButton.AutoEllipsis = true;
            this.CombatClassSettingsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.CombatClassSettingsButton.ForeColor = System.Drawing.Color.Snow;
            this.CombatClassSettingsButton.HooverImage = global::nManager.Properties.Resources.greenB_70;
            this.CombatClassSettingsButton.Image = global::nManager.Properties.Resources.blueB_70;
            this.CombatClassSettingsButton.Location = new System.Drawing.Point(328, 51);
            this.CombatClassSettingsButton.Name = "CombatClassSettingsButton";
            this.CombatClassSettingsButton.Size = new System.Drawing.Size(70, 29);
            this.CombatClassSettingsButton.TabIndex = 54;
            this.CombatClassSettingsButton.Text = "Settings";
            this.CombatClassSettingsButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.CombatClassSettingsButton.Click += new System.EventHandler(this.CombatClassSettingsButton_Click);
            // 
            // PluginsManagementSystemPanelName
            // 
            this.PluginsManagementSystemPanelName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.PluginsManagementSystemPanelName.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.PluginsManagementSystemPanelName.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.PluginsManagementSystemPanelName.ContentSize = new System.Drawing.Size(556, 247);
            this.PluginsManagementSystemPanelName.Controls.Add(this.ActivatedPluginResetSettings);
            this.PluginsManagementSystemPanelName.Controls.Add(this.ActivatedPluginSettings);
            this.PluginsManagementSystemPanelName.Controls.Add(this.DeactivatePlugin);
            this.PluginsManagementSystemPanelName.Controls.Add(this.ActivatePlugin);
            this.PluginsManagementSystemPanelName.Controls.Add(this.LaunchExpiredPlugins);
            this.PluginsManagementSystemPanelName.Controls.Add(this.ActivatePluginsSystem);
            this.PluginsManagementSystemPanelName.Controls.Add(this.LaunchExpiredPluginsLabel);
            this.PluginsManagementSystemPanelName.Controls.Add(this.ActivatePluginsSystemLabel);
            this.PluginsManagementSystemPanelName.Controls.Add(this.ActivatedPluginsListLabel);
            this.PluginsManagementSystemPanelName.Controls.Add(this.ActivatedPluginsList);
            this.PluginsManagementSystemPanelName.Controls.Add(this.AvailablePluginsListLabel);
            this.PluginsManagementSystemPanelName.Controls.Add(this.AvailablePluginsList);
            this.PluginsManagementSystemPanelName.Fold = true;
            this.PluginsManagementSystemPanelName.FolderImage = ((System.Drawing.Image)(resources.GetObject("PluginsManagementSystemPanelName.FolderImage")));
            this.PluginsManagementSystemPanelName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.PluginsManagementSystemPanelName.HeaderBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.PluginsManagementSystemPanelName.HeaderImage = ((System.Drawing.Image)(resources.GetObject("PluginsManagementSystemPanelName.HeaderImage")));
            this.PluginsManagementSystemPanelName.HeaderSize = new System.Drawing.Size(556, 36);
            this.PluginsManagementSystemPanelName.Location = new System.Drawing.Point(0, 36);
            this.PluginsManagementSystemPanelName.MaximumSize = new System.Drawing.Size(556, 0);
            this.PluginsManagementSystemPanelName.Name = "PluginsManagementSystemPanelName";
            this.PluginsManagementSystemPanelName.OrderIndex = 2;
            this.PluginsManagementSystemPanelName.Padding = new System.Windows.Forms.Padding(0, 0, 0, 12);
            this.PluginsManagementSystemPanelName.Size = new System.Drawing.Size(556, 36);
            this.PluginsManagementSystemPanelName.TabIndex = 45;
            this.PluginsManagementSystemPanelName.TitleFont = new System.Drawing.Font("Segoe UI", 7.65F, System.Drawing.FontStyle.Bold);
            this.PluginsManagementSystemPanelName.TitleForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.PluginsManagementSystemPanelName.TitleText = "Plugins Management System - Load/UnLoad/Configure products";
            this.PluginsManagementSystemPanelName.UnfolderImage = ((System.Drawing.Image)(resources.GetObject("PluginsManagementSystemPanelName.UnfolderImage")));
            // 
            // ActivatedPluginResetSettings
            // 
            this.ActivatedPluginResetSettings.AutoEllipsis = true;
            this.ActivatedPluginResetSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.ActivatedPluginResetSettings.ForeColor = System.Drawing.Color.Snow;
            this.ActivatedPluginResetSettings.HooverImage = global::nManager.Properties.Resources.greenB_70;
            this.ActivatedPluginResetSettings.Image = global::nManager.Properties.Resources.blackB_70;
            this.ActivatedPluginResetSettings.Location = new System.Drawing.Point(477, 206);
            this.ActivatedPluginResetSettings.Margin = new System.Windows.Forms.Padding(0);
            this.ActivatedPluginResetSettings.Name = "ActivatedPluginResetSettings";
            this.ActivatedPluginResetSettings.Size = new System.Drawing.Size(70, 29);
            this.ActivatedPluginResetSettings.TabIndex = 14;
            this.ActivatedPluginResetSettings.Text = "Reset";
            this.ActivatedPluginResetSettings.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ActivatedPluginResetSettings.Click += new System.EventHandler(this.ResetPlugin);
            // 
            // ActivatedPluginSettings
            // 
            this.ActivatedPluginSettings.AutoEllipsis = true;
            this.ActivatedPluginSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.ActivatedPluginSettings.ForeColor = System.Drawing.Color.Snow;
            this.ActivatedPluginSettings.HooverImage = global::nManager.Properties.Resources.greenB_70;
            this.ActivatedPluginSettings.Image = global::nManager.Properties.Resources.blueB_70;
            this.ActivatedPluginSettings.Location = new System.Drawing.Point(402, 206);
            this.ActivatedPluginSettings.Margin = new System.Windows.Forms.Padding(0);
            this.ActivatedPluginSettings.Name = "ActivatedPluginSettings";
            this.ActivatedPluginSettings.Size = new System.Drawing.Size(70, 29);
            this.ActivatedPluginSettings.TabIndex = 13;
            this.ActivatedPluginSettings.Text = "Configure";
            this.ActivatedPluginSettings.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ActivatedPluginSettings.Click += new System.EventHandler(this.ConfigurePlugin);
            // 
            // DeactivatePlugin
            // 
            this.DeactivatePlugin.AutoEllipsis = true;
            this.DeactivatePlugin.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.DeactivatePlugin.ForeColor = System.Drawing.Color.Snow;
            this.DeactivatePlugin.HooverImage = global::nManager.Properties.Resources.greenB;
            this.DeactivatePlugin.Image = global::nManager.Properties.Resources.blackB;
            this.DeactivatePlugin.Location = new System.Drawing.Point(290, 206);
            this.DeactivatePlugin.Margin = new System.Windows.Forms.Padding(0);
            this.DeactivatePlugin.Name = "DeactivatePlugin";
            this.DeactivatePlugin.Size = new System.Drawing.Size(106, 29);
            this.DeactivatePlugin.TabIndex = 12;
            this.DeactivatePlugin.Text = "Deactivate";
            this.DeactivatePlugin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.DeactivatePlugin.Click += new System.EventHandler(this.UnLoadPlugin);
            // 
            // ActivatePlugin
            // 
            this.ActivatePlugin.AutoEllipsis = true;
            this.ActivatePlugin.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.ActivatePlugin.ForeColor = System.Drawing.Color.Snow;
            this.ActivatePlugin.HooverImage = global::nManager.Properties.Resources.greenB_150;
            this.ActivatePlugin.Image = global::nManager.Properties.Resources.blueB_150;
            this.ActivatePlugin.Location = new System.Drawing.Point(118, 206);
            this.ActivatePlugin.Margin = new System.Windows.Forms.Padding(0);
            this.ActivatePlugin.Name = "ActivatePlugin";
            this.ActivatePlugin.Size = new System.Drawing.Size(150, 29);
            this.ActivatePlugin.TabIndex = 11;
            this.ActivatePlugin.Text = "Activate Plugin";
            this.ActivatePlugin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ActivatePlugin.Click += new System.EventHandler(this.LoadPlugin);
            // 
            // LaunchExpiredPlugins
            // 
            this.LaunchExpiredPlugins.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.LaunchExpiredPlugins.Location = new System.Drawing.Point(291, 48);
            this.LaunchExpiredPlugins.MaximumSize = new System.Drawing.Size(60, 20);
            this.LaunchExpiredPlugins.MinimumSize = new System.Drawing.Size(60, 20);
            this.LaunchExpiredPlugins.Name = "LaunchExpiredPlugins";
            this.LaunchExpiredPlugins.OffText = "OFF";
            this.LaunchExpiredPlugins.OnText = "ON";
            this.LaunchExpiredPlugins.Size = new System.Drawing.Size(60, 20);
            this.LaunchExpiredPlugins.TabIndex = 10;
            this.LaunchExpiredPlugins.Value = false;
            // 
            // ActivatePluginsSystem
            // 
            this.ActivatePluginsSystem.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.ActivatePluginsSystem.Location = new System.Drawing.Point(12, 48);
            this.ActivatePluginsSystem.MaximumSize = new System.Drawing.Size(60, 20);
            this.ActivatePluginsSystem.MinimumSize = new System.Drawing.Size(60, 20);
            this.ActivatePluginsSystem.Name = "ActivatePluginsSystem";
            this.ActivatePluginsSystem.OffText = "OFF";
            this.ActivatePluginsSystem.OnText = "ON";
            this.ActivatePluginsSystem.Size = new System.Drawing.Size(60, 20);
            this.ActivatePluginsSystem.TabIndex = 8;
            this.ActivatePluginsSystem.Value = true;
            // 
            // LaunchExpiredPluginsLabel
            // 
            this.LaunchExpiredPluginsLabel.BackColor = System.Drawing.Color.Transparent;
            this.LaunchExpiredPluginsLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LaunchExpiredPluginsLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.LaunchExpiredPluginsLabel.Location = new System.Drawing.Point(359, 46);
            this.LaunchExpiredPluginsLabel.Name = "LaunchExpiredPluginsLabel";
            this.LaunchExpiredPluginsLabel.Size = new System.Drawing.Size(187, 22);
            this.LaunchExpiredPluginsLabel.TabIndex = 9;
            this.LaunchExpiredPluginsLabel.Text = "Launch expired plugins";
            this.LaunchExpiredPluginsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ActivatePluginsSystemLabel
            // 
            this.ActivatePluginsSystemLabel.BackColor = System.Drawing.Color.Transparent;
            this.ActivatePluginsSystemLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ActivatePluginsSystemLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.ActivatePluginsSystemLabel.Location = new System.Drawing.Point(80, 46);
            this.ActivatePluginsSystemLabel.Name = "ActivatePluginsSystemLabel";
            this.ActivatePluginsSystemLabel.Size = new System.Drawing.Size(187, 22);
            this.ActivatePluginsSystemLabel.TabIndex = 7;
            this.ActivatePluginsSystemLabel.Text = "Plugins System Status";
            this.ActivatePluginsSystemLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ActivatedPluginsListLabel
            // 
            this.ActivatedPluginsListLabel.BackColor = System.Drawing.Color.Transparent;
            this.ActivatedPluginsListLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ActivatedPluginsListLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.ActivatedPluginsListLabel.Location = new System.Drawing.Point(291, 68);
            this.ActivatedPluginsListLabel.Name = "ActivatedPluginsListLabel";
            this.ActivatedPluginsListLabel.Size = new System.Drawing.Size(154, 22);
            this.ActivatedPluginsListLabel.TabIndex = 6;
            this.ActivatedPluginsListLabel.Text = "Activated Plugins :";
            this.ActivatedPluginsListLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // ActivatedPluginsList
            // 
            this.ActivatedPluginsList.FormattingEnabled = true;
            this.ActivatedPluginsList.Location = new System.Drawing.Point(291, 100);
            this.ActivatedPluginsList.Name = "ActivatedPluginsList";
            this.ActivatedPluginsList.Size = new System.Drawing.Size(255, 95);
            this.ActivatedPluginsList.TabIndex = 5;
            this.ActivatedPluginsList.DoubleClick += new System.EventHandler(this.UnLoadPlugin);
            // 
            // AvailablePluginsListLabel
            // 
            this.AvailablePluginsListLabel.BackColor = System.Drawing.Color.Transparent;
            this.AvailablePluginsListLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AvailablePluginsListLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.AvailablePluginsListLabel.Location = new System.Drawing.Point(11, 68);
            this.AvailablePluginsListLabel.Name = "AvailablePluginsListLabel";
            this.AvailablePluginsListLabel.Size = new System.Drawing.Size(154, 22);
            this.AvailablePluginsListLabel.TabIndex = 4;
            this.AvailablePluginsListLabel.Text = "Available Plugins List :";
            this.AvailablePluginsListLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // AvailablePluginsList
            // 
            this.AvailablePluginsList.FormattingEnabled = true;
            this.AvailablePluginsList.Location = new System.Drawing.Point(12, 100);
            this.AvailablePluginsList.Name = "AvailablePluginsList";
            this.AvailablePluginsList.Size = new System.Drawing.Size(255, 95);
            this.AvailablePluginsList.TabIndex = 2;
            this.AvailablePluginsList.DoubleClick += new System.EventHandler(this.LoadPlugin);
            // 
            // MountManagementPanelName
            // 
            this.MountManagementPanelName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.MountManagementPanelName.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.MountManagementPanelName.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.MountManagementPanelName.ContentSize = new System.Drawing.Size(556, 180);
            this.MountManagementPanelName.Controls.Add(this.AquaticMountName);
            this.MountManagementPanelName.Controls.Add(this.AquaticMountNameLabel);
            this.MountManagementPanelName.Controls.Add(this.IgnoreFightIfMountedLabel);
            this.MountManagementPanelName.Controls.Add(this.IgnoreFightIfMounted);
            this.MountManagementPanelName.Controls.Add(this.MinimumDistanceToUseMount);
            this.MountManagementPanelName.Controls.Add(this.MinimumDistanceToUseMountLabel);
            this.MountManagementPanelName.Controls.Add(this.FlyingMountName);
            this.MountManagementPanelName.Controls.Add(this.FlyingMountNameLabel);
            this.MountManagementPanelName.Controls.Add(this.GroundMountName);
            this.MountManagementPanelName.Controls.Add(this.GroundMountNameLabel);
            this.MountManagementPanelName.Controls.Add(this.UseGroundMountLabel);
            this.MountManagementPanelName.Controls.Add(this.UseGroundMount);
            this.MountManagementPanelName.Fold = true;
            this.MountManagementPanelName.FolderImage = ((System.Drawing.Image)(resources.GetObject("MountManagementPanelName.FolderImage")));
            this.MountManagementPanelName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.MountManagementPanelName.HeaderBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.MountManagementPanelName.HeaderImage = ((System.Drawing.Image)(resources.GetObject("MountManagementPanelName.HeaderImage")));
            this.MountManagementPanelName.HeaderSize = new System.Drawing.Size(556, 36);
            this.MountManagementPanelName.Location = new System.Drawing.Point(0, 72);
            this.MountManagementPanelName.Margin = new System.Windows.Forms.Padding(0);
            this.MountManagementPanelName.MaximumSize = new System.Drawing.Size(556, 0);
            this.MountManagementPanelName.MinimumSize = new System.Drawing.Size(556, 36);
            this.MountManagementPanelName.Name = "MountManagementPanelName";
            this.MountManagementPanelName.OrderIndex = 3;
            this.MountManagementPanelName.Padding = new System.Windows.Forms.Padding(0, 0, 0, 12);
            this.MountManagementPanelName.Size = new System.Drawing.Size(556, 36);
            this.MountManagementPanelName.TabIndex = 3;
            this.MountManagementPanelName.TitleFont = new System.Drawing.Font("Segoe UI", 7.65F, System.Drawing.FontStyle.Bold);
            this.MountManagementPanelName.TitleForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.MountManagementPanelName.TitleText = "Ingame Mount Management - (Allow you to use Ground/Fly/Swim mount)";
            this.MountManagementPanelName.UnfolderImage = ((System.Drawing.Image)(resources.GetObject("MountManagementPanelName.UnfolderImage")));
            // 
            // AquaticMountName
            // 
            this.AquaticMountName.ForeColor = System.Drawing.Color.Black;
            this.AquaticMountName.Location = new System.Drawing.Point(161, 140);
            this.AquaticMountName.Name = "AquaticMountName";
            this.AquaticMountName.Size = new System.Drawing.Size(144, 22);
            this.AquaticMountName.TabIndex = 22;
            // 
            // AquaticMountNameLabel
            // 
            this.AquaticMountNameLabel.BackColor = System.Drawing.Color.Transparent;
            this.AquaticMountNameLabel.ForeColor = System.Drawing.Color.Black;
            this.AquaticMountNameLabel.Location = new System.Drawing.Point(2, 137);
            this.AquaticMountNameLabel.Name = "AquaticMountNameLabel";
            this.AquaticMountNameLabel.Size = new System.Drawing.Size(154, 22);
            this.AquaticMountNameLabel.TabIndex = 21;
            this.AquaticMountNameLabel.Text = "Aquatic Mount";
            // 
            // IgnoreFightIfMountedLabel
            // 
            this.IgnoreFightIfMountedLabel.BackColor = System.Drawing.Color.Transparent;
            this.IgnoreFightIfMountedLabel.ForeColor = System.Drawing.Color.Black;
            this.IgnoreFightIfMountedLabel.Location = new System.Drawing.Point(316, 85);
            this.IgnoreFightIfMountedLabel.Name = "IgnoreFightIfMountedLabel";
            this.IgnoreFightIfMountedLabel.Size = new System.Drawing.Size(154, 22);
            this.IgnoreFightIfMountedLabel.TabIndex = 20;
            this.IgnoreFightIfMountedLabel.Text = "Ignore Fight if in Gound Mount";
            this.IgnoreFightIfMountedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // IgnoreFightIfMounted
            // 
            this.IgnoreFightIfMounted.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.IgnoreFightIfMounted.Location = new System.Drawing.Point(475, 85);
            this.IgnoreFightIfMounted.MaximumSize = new System.Drawing.Size(60, 20);
            this.IgnoreFightIfMounted.MinimumSize = new System.Drawing.Size(60, 20);
            this.IgnoreFightIfMounted.Name = "IgnoreFightIfMounted";
            this.IgnoreFightIfMounted.OffText = "OFF";
            this.IgnoreFightIfMounted.OnText = "ON";
            this.IgnoreFightIfMounted.Size = new System.Drawing.Size(60, 20);
            this.IgnoreFightIfMounted.TabIndex = 23;
            this.IgnoreFightIfMounted.Value = false;
            // 
            // MinimumDistanceToUseMount
            // 
            this.MinimumDistanceToUseMount.ForeColor = System.Drawing.Color.Black;
            this.MinimumDistanceToUseMount.Location = new System.Drawing.Point(161, 79);
            this.MinimumDistanceToUseMount.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.MinimumDistanceToUseMount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.MinimumDistanceToUseMount.Name = "MinimumDistanceToUseMount";
            this.MinimumDistanceToUseMount.Size = new System.Drawing.Size(77, 22);
            this.MinimumDistanceToUseMount.TabIndex = 18;
            this.MinimumDistanceToUseMount.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            // 
            // MinimumDistanceToUseMountLabel
            // 
            this.MinimumDistanceToUseMountLabel.BackColor = System.Drawing.Color.Transparent;
            this.MinimumDistanceToUseMountLabel.ForeColor = System.Drawing.Color.Black;
            this.MinimumDistanceToUseMountLabel.Location = new System.Drawing.Point(2, 79);
            this.MinimumDistanceToUseMountLabel.Name = "MinimumDistanceToUseMountLabel";
            this.MinimumDistanceToUseMountLabel.Size = new System.Drawing.Size(154, 22);
            this.MinimumDistanceToUseMountLabel.TabIndex = 16;
            this.MinimumDistanceToUseMountLabel.Text = "Mount Distance";
            // 
            // FlyingMountName
            // 
            this.FlyingMountName.ForeColor = System.Drawing.Color.Black;
            this.FlyingMountName.Location = new System.Drawing.Point(161, 110);
            this.FlyingMountName.Name = "FlyingMountName";
            this.FlyingMountName.Size = new System.Drawing.Size(144, 22);
            this.FlyingMountName.TabIndex = 15;
            // 
            // FlyingMountNameLabel
            // 
            this.FlyingMountNameLabel.BackColor = System.Drawing.Color.Transparent;
            this.FlyingMountNameLabel.ForeColor = System.Drawing.Color.Black;
            this.FlyingMountNameLabel.Location = new System.Drawing.Point(2, 108);
            this.FlyingMountNameLabel.Name = "FlyingMountNameLabel";
            this.FlyingMountNameLabel.Size = new System.Drawing.Size(154, 22);
            this.FlyingMountNameLabel.TabIndex = 14;
            this.FlyingMountNameLabel.Text = "Flying Mount";
            // 
            // GroundMountName
            // 
            this.GroundMountName.ForeColor = System.Drawing.Color.Black;
            this.GroundMountName.Location = new System.Drawing.Point(161, 54);
            this.GroundMountName.Name = "GroundMountName";
            this.GroundMountName.Size = new System.Drawing.Size(144, 22);
            this.GroundMountName.TabIndex = 13;
            // 
            // GroundMountNameLabel
            // 
            this.GroundMountNameLabel.BackColor = System.Drawing.Color.Transparent;
            this.GroundMountNameLabel.ForeColor = System.Drawing.Color.Black;
            this.GroundMountNameLabel.Location = new System.Drawing.Point(2, 51);
            this.GroundMountNameLabel.Name = "GroundMountNameLabel";
            this.GroundMountNameLabel.Size = new System.Drawing.Size(154, 22);
            this.GroundMountNameLabel.TabIndex = 12;
            this.GroundMountNameLabel.Text = "Ground Mount";
            // 
            // UseGroundMountLabel
            // 
            this.UseGroundMountLabel.BackColor = System.Drawing.Color.Transparent;
            this.UseGroundMountLabel.ForeColor = System.Drawing.Color.Black;
            this.UseGroundMountLabel.Location = new System.Drawing.Point(316, 54);
            this.UseGroundMountLabel.Name = "UseGroundMountLabel";
            this.UseGroundMountLabel.Size = new System.Drawing.Size(154, 22);
            this.UseGroundMountLabel.TabIndex = 11;
            this.UseGroundMountLabel.Text = "Use Ground Mount";
            this.UseGroundMountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // UseGroundMount
            // 
            this.UseGroundMount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.UseGroundMount.Location = new System.Drawing.Point(475, 54);
            this.UseGroundMount.MaximumSize = new System.Drawing.Size(60, 20);
            this.UseGroundMount.MinimumSize = new System.Drawing.Size(60, 20);
            this.UseGroundMount.Name = "UseGroundMount";
            this.UseGroundMount.OffText = "OFF";
            this.UseGroundMount.OnText = "ON";
            this.UseGroundMount.Size = new System.Drawing.Size(60, 20);
            this.UseGroundMount.TabIndex = 24;
            this.UseGroundMount.Value = false;
            // 
            // LootingFarmingManagementPanelName
            // 
            this.LootingFarmingManagementPanelName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.LootingFarmingManagementPanelName.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.LootingFarmingManagementPanelName.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.LootingFarmingManagementPanelName.ContentSize = new System.Drawing.Size(556, 518);
            this.LootingFarmingManagementPanelName.Controls.Add(this.UseLootARangeLabel);
            this.LootingFarmingManagementPanelName.Controls.Add(this.UseLootARange);
            this.LootingFarmingManagementPanelName.Controls.Add(this.ActivateLootStatisticsLabel);
            this.LootingFarmingManagementPanelName.Controls.Add(this.ActivateLootStatistics);
            this.LootingFarmingManagementPanelName.Controls.Add(this.DontHarvestTheFollowingObjectsHelper);
            this.LootingFarmingManagementPanelName.Controls.Add(this.DontHarvestTheFollowingObjects);
            this.LootingFarmingManagementPanelName.Controls.Add(this.AutoConfirmOnBoPItems);
            this.LootingFarmingManagementPanelName.Controls.Add(this.AutoConfirmOnBoPItemsLabel);
            this.LootingFarmingManagementPanelName.Controls.Add(this.OnlyUseMillingInTownLabel);
            this.LootingFarmingManagementPanelName.Controls.Add(this.OnlyUseMillingInTown);
            this.LootingFarmingManagementPanelName.Controls.Add(this.TimeBetweenEachMillingAttempt);
            this.LootingFarmingManagementPanelName.Controls.Add(this.TimeBetweenEachMillingAttemptLabel);
            this.LootingFarmingManagementPanelName.Controls.Add(this.ActivateAutoMillingLabel);
            this.LootingFarmingManagementPanelName.Controls.Add(this.ActivateAutoMilling);
            this.LootingFarmingManagementPanelName.Controls.Add(this.HerbsToBeMilled);
            this.LootingFarmingManagementPanelName.Controls.Add(this.HerbsToBeMilledLabel);
            this.LootingFarmingManagementPanelName.Controls.Add(this.MakeStackOfElementalsItemsLabel);
            this.LootingFarmingManagementPanelName.Controls.Add(this.MakeStackOfElementalsItems);
            this.LootingFarmingManagementPanelName.Controls.Add(this.OnlyUseProspectingInTownLabel);
            this.LootingFarmingManagementPanelName.Controls.Add(this.OnlyUseProspectingInTown);
            this.LootingFarmingManagementPanelName.Controls.Add(this.TimeBetweenEachProspectingAttempt);
            this.LootingFarmingManagementPanelName.Controls.Add(this.TimeBetweenEachProspectingAttemptLabel);
            this.LootingFarmingManagementPanelName.Controls.Add(this.ActivateAutoProspectingLabel);
            this.LootingFarmingManagementPanelName.Controls.Add(this.ActivateAutoProspecting);
            this.LootingFarmingManagementPanelName.Controls.Add(this.MineralsToProspect);
            this.LootingFarmingManagementPanelName.Controls.Add(this.MineralsToProspectLabel);
            this.LootingFarmingManagementPanelName.Controls.Add(this.labelX61);
            this.LootingFarmingManagementPanelName.Controls.Add(this.ActivateAutoSmelting);
            this.LootingFarmingManagementPanelName.Controls.Add(this.DontHarvestTheFollowingObjectsLabel);
            this.LootingFarmingManagementPanelName.Controls.Add(this.addBlackListHarvest);
            this.LootingFarmingManagementPanelName.Controls.Add(this.HarvestDuringLongDistanceMovementsLabel);
            this.LootingFarmingManagementPanelName.Controls.Add(this.HarvestDuringLongDistanceMovements);
            this.LootingFarmingManagementPanelName.Controls.Add(this.labelX23);
            this.LootingFarmingManagementPanelName.Controls.Add(this.BeastNinjaSkinning);
            this.LootingFarmingManagementPanelName.Controls.Add(this.GatheringSearchRadius);
            this.LootingFarmingManagementPanelName.Controls.Add(this.GatheringSearchRadiusLabel);
            this.LootingFarmingManagementPanelName.Controls.Add(this.DontHarvestIfMoreThanXUnitInAggroRange);
            this.LootingFarmingManagementPanelName.Controls.Add(this.DontHarvestIfMoreThanXUnitInAggroRangeLabel);
            this.LootingFarmingManagementPanelName.Controls.Add(this.ActivateHerbsHarvestingLabel);
            this.LootingFarmingManagementPanelName.Controls.Add(this.ActivateHerbsHarvesting);
            this.LootingFarmingManagementPanelName.Controls.Add(this.ActivateVeinsHarvestingLabel);
            this.LootingFarmingManagementPanelName.Controls.Add(this.ActivateVeinsHarvesting);
            this.LootingFarmingManagementPanelName.Controls.Add(this.ActivateBeastSkinningLabel);
            this.LootingFarmingManagementPanelName.Controls.Add(this.ActivateBeastSkinning);
            this.LootingFarmingManagementPanelName.Controls.Add(this.labelX16);
            this.LootingFarmingManagementPanelName.Controls.Add(this.ActivateChestLooting);
            this.LootingFarmingManagementPanelName.Controls.Add(this.DontHarvestIfPlayerNearRadius);
            this.LootingFarmingManagementPanelName.Controls.Add(this.DontHarvestIfPlayerNearRadiusLabel);
            this.LootingFarmingManagementPanelName.Controls.Add(this.ActivateMonsterLootingLabel);
            this.LootingFarmingManagementPanelName.Controls.Add(this.ActivateMonsterLooting);
            this.LootingFarmingManagementPanelName.Fold = true;
            this.LootingFarmingManagementPanelName.FolderImage = ((System.Drawing.Image)(resources.GetObject("LootingFarmingManagementPanelName.FolderImage")));
            this.LootingFarmingManagementPanelName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.LootingFarmingManagementPanelName.HeaderBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.LootingFarmingManagementPanelName.HeaderImage = ((System.Drawing.Image)(resources.GetObject("LootingFarmingManagementPanelName.HeaderImage")));
            this.LootingFarmingManagementPanelName.HeaderSize = new System.Drawing.Size(556, 36);
            this.LootingFarmingManagementPanelName.Location = new System.Drawing.Point(0, 108);
            this.LootingFarmingManagementPanelName.Margin = new System.Windows.Forms.Padding(0);
            this.LootingFarmingManagementPanelName.MaximumSize = new System.Drawing.Size(556, 0);
            this.LootingFarmingManagementPanelName.MinimumSize = new System.Drawing.Size(556, 36);
            this.LootingFarmingManagementPanelName.Name = "LootingFarmingManagementPanelName";
            this.LootingFarmingManagementPanelName.OrderIndex = 4;
            this.LootingFarmingManagementPanelName.Padding = new System.Windows.Forms.Padding(0, 0, 0, 12);
            this.LootingFarmingManagementPanelName.Size = new System.Drawing.Size(556, 36);
            this.LootingFarmingManagementPanelName.TabIndex = 5;
            this.LootingFarmingManagementPanelName.TitleFont = new System.Drawing.Font("Segoe UI", 7.65F, System.Drawing.FontStyle.Bold);
            this.LootingFarmingManagementPanelName.TitleForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.LootingFarmingManagementPanelName.TitleText = "Looting/Farming Management";
            this.LootingFarmingManagementPanelName.UnfolderImage = ((System.Drawing.Image)(resources.GetObject("LootingFarmingManagementPanelName.UnfolderImage")));
            // 
            // UseLootARangeLabel
            // 
            this.UseLootARangeLabel.BackColor = System.Drawing.Color.Transparent;
            this.UseLootARangeLabel.ForeColor = System.Drawing.Color.Black;
            this.UseLootARangeLabel.Location = new System.Drawing.Point(4, 551);
            this.UseLootARangeLabel.Name = "UseLootARangeLabel";
            this.UseLootARangeLabel.Size = new System.Drawing.Size(154, 22);
            this.UseLootARangeLabel.TabIndex = 82;
            this.UseLootARangeLabel.Text = "Use Loot-A-Range items";
            // 
            // UseLootARange
            // 
            this.UseLootARange.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.UseLootARange.Location = new System.Drawing.Point(163, 551);
            this.UseLootARange.MaximumSize = new System.Drawing.Size(60, 20);
            this.UseLootARange.MinimumSize = new System.Drawing.Size(60, 20);
            this.UseLootARange.Name = "UseLootARange";
            this.UseLootARange.OffText = "OFF";
            this.UseLootARange.OnText = "ON";
            this.UseLootARange.Size = new System.Drawing.Size(60, 20);
            this.UseLootARange.TabIndex = 83;
            this.UseLootARange.Value = true;
            // 
            // ActivateLootStatisticsLabel
            // 
            this.ActivateLootStatisticsLabel.BackColor = System.Drawing.Color.Transparent;
            this.ActivateLootStatisticsLabel.ForeColor = System.Drawing.Color.Black;
            this.ActivateLootStatisticsLabel.Location = new System.Drawing.Point(4, 77);
            this.ActivateLootStatisticsLabel.Name = "ActivateLootStatisticsLabel";
            this.ActivateLootStatisticsLabel.Size = new System.Drawing.Size(154, 22);
            this.ActivateLootStatisticsLabel.TabIndex = 66;
            this.ActivateLootStatisticsLabel.Text = "Activate Loot Statistics";
            // 
            // ActivateLootStatistics
            // 
            this.ActivateLootStatistics.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.ActivateLootStatistics.Location = new System.Drawing.Point(163, 77);
            this.ActivateLootStatistics.MaximumSize = new System.Drawing.Size(60, 20);
            this.ActivateLootStatistics.MinimumSize = new System.Drawing.Size(60, 20);
            this.ActivateLootStatistics.Name = "ActivateLootStatistics";
            this.ActivateLootStatistics.OffText = "OFF";
            this.ActivateLootStatistics.OnText = "ON";
            this.ActivateLootStatistics.Size = new System.Drawing.Size(60, 20);
            this.ActivateLootStatistics.TabIndex = 67;
            this.ActivateLootStatistics.Value = false;
            // 
            // DontHarvestTheFollowingObjectsHelper
            // 
            this.DontHarvestTheFollowingObjectsHelper.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(229)))), ((int)(((byte)(230)))));
            this.DontHarvestTheFollowingObjectsHelper.ForeColor = System.Drawing.Color.Black;
            this.DontHarvestTheFollowingObjectsHelper.Location = new System.Drawing.Point(500, 160);
            this.DontHarvestTheFollowingObjectsHelper.Name = "DontHarvestTheFollowingObjectsHelper";
            this.DontHarvestTheFollowingObjectsHelper.Size = new System.Drawing.Size(18, 20);
            this.DontHarvestTheFollowingObjectsHelper.TabIndex = 64;
            this.DontHarvestTheFollowingObjectsHelper.Text = "?";
            this.DontHarvestTheFollowingObjectsHelper.UseVisualStyleBackColor = false;
            this.DontHarvestTheFollowingObjectsHelper.Click += new System.EventHandler(this.DontHaverstObjectsTutorial_Click);
            // 
            // DontHarvestTheFollowingObjects
            // 
            this.DontHarvestTheFollowingObjects.ForeColor = System.Drawing.Color.Black;
            this.DontHarvestTheFollowingObjects.Location = new System.Drawing.Point(290, 188);
            this.DontHarvestTheFollowingObjects.Multiline = true;
            this.DontHarvestTheFollowingObjects.Name = "DontHarvestTheFollowingObjects";
            this.DontHarvestTheFollowingObjects.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.DontHarvestTheFollowingObjects.Size = new System.Drawing.Size(226, 83);
            this.DontHarvestTheFollowingObjects.TabIndex = 63;
            // 
            // AutoConfirmOnBoPItems
            // 
            this.AutoConfirmOnBoPItems.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.AutoConfirmOnBoPItems.Location = new System.Drawing.Point(455, 49);
            this.AutoConfirmOnBoPItems.MaximumSize = new System.Drawing.Size(60, 20);
            this.AutoConfirmOnBoPItems.MinimumSize = new System.Drawing.Size(60, 20);
            this.AutoConfirmOnBoPItems.Name = "AutoConfirmOnBoPItems";
            this.AutoConfirmOnBoPItems.OffText = "OFF";
            this.AutoConfirmOnBoPItems.OnText = "ON";
            this.AutoConfirmOnBoPItems.Size = new System.Drawing.Size(60, 20);
            this.AutoConfirmOnBoPItems.TabIndex = 68;
            this.AutoConfirmOnBoPItems.Value = false;
            // 
            // AutoConfirmOnBoPItemsLabel
            // 
            this.AutoConfirmOnBoPItemsLabel.BackColor = System.Drawing.Color.Transparent;
            this.AutoConfirmOnBoPItemsLabel.ForeColor = System.Drawing.Color.Black;
            this.AutoConfirmOnBoPItemsLabel.Location = new System.Drawing.Point(289, 49);
            this.AutoConfirmOnBoPItemsLabel.Name = "AutoConfirmOnBoPItemsLabel";
            this.AutoConfirmOnBoPItemsLabel.Size = new System.Drawing.Size(161, 22);
            this.AutoConfirmOnBoPItemsLabel.TabIndex = 62;
            this.AutoConfirmOnBoPItemsLabel.Text = "Auto Confirm on BoP Items";
            // 
            // OnlyUseMillingInTownLabel
            // 
            this.OnlyUseMillingInTownLabel.BackColor = System.Drawing.Color.Transparent;
            this.OnlyUseMillingInTownLabel.ForeColor = System.Drawing.Color.Black;
            this.OnlyUseMillingInTownLabel.Location = new System.Drawing.Point(4, 496);
            this.OnlyUseMillingInTownLabel.Name = "OnlyUseMillingInTownLabel";
            this.OnlyUseMillingInTownLabel.Size = new System.Drawing.Size(154, 22);
            this.OnlyUseMillingInTownLabel.TabIndex = 59;
            this.OnlyUseMillingInTownLabel.Text = "Milling only in town";
            // 
            // OnlyUseMillingInTown
            // 
            this.OnlyUseMillingInTown.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.OnlyUseMillingInTown.Location = new System.Drawing.Point(163, 496);
            this.OnlyUseMillingInTown.MaximumSize = new System.Drawing.Size(60, 20);
            this.OnlyUseMillingInTown.MinimumSize = new System.Drawing.Size(60, 20);
            this.OnlyUseMillingInTown.Name = "OnlyUseMillingInTown";
            this.OnlyUseMillingInTown.OffText = "OFF";
            this.OnlyUseMillingInTown.OnText = "ON";
            this.OnlyUseMillingInTown.Size = new System.Drawing.Size(60, 20);
            this.OnlyUseMillingInTown.TabIndex = 69;
            this.OnlyUseMillingInTown.Value = false;
            // 
            // TimeBetweenEachMillingAttempt
            // 
            this.TimeBetweenEachMillingAttempt.ForeColor = System.Drawing.Color.Black;
            this.TimeBetweenEachMillingAttempt.Location = new System.Drawing.Point(163, 468);
            this.TimeBetweenEachMillingAttempt.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.TimeBetweenEachMillingAttempt.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.TimeBetweenEachMillingAttempt.Name = "TimeBetweenEachMillingAttempt";
            this.TimeBetweenEachMillingAttempt.Size = new System.Drawing.Size(77, 22);
            this.TimeBetweenEachMillingAttempt.TabIndex = 57;
            this.TimeBetweenEachMillingAttempt.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            // 
            // TimeBetweenEachMillingAttemptLabel
            // 
            this.TimeBetweenEachMillingAttemptLabel.BackColor = System.Drawing.Color.Transparent;
            this.TimeBetweenEachMillingAttemptLabel.ForeColor = System.Drawing.Color.Black;
            this.TimeBetweenEachMillingAttemptLabel.Location = new System.Drawing.Point(4, 468);
            this.TimeBetweenEachMillingAttemptLabel.Name = "TimeBetweenEachMillingAttemptLabel";
            this.TimeBetweenEachMillingAttemptLabel.Size = new System.Drawing.Size(154, 22);
            this.TimeBetweenEachMillingAttemptLabel.TabIndex = 56;
            this.TimeBetweenEachMillingAttemptLabel.Text = "Milling Every (in minute)";
            // 
            // ActivateAutoMillingLabel
            // 
            this.ActivateAutoMillingLabel.BackColor = System.Drawing.Color.Transparent;
            this.ActivateAutoMillingLabel.ForeColor = System.Drawing.Color.Black;
            this.ActivateAutoMillingLabel.Location = new System.Drawing.Point(4, 440);
            this.ActivateAutoMillingLabel.Name = "ActivateAutoMillingLabel";
            this.ActivateAutoMillingLabel.Size = new System.Drawing.Size(154, 22);
            this.ActivateAutoMillingLabel.TabIndex = 55;
            this.ActivateAutoMillingLabel.Text = "Milling";
            // 
            // ActivateAutoMilling
            // 
            this.ActivateAutoMilling.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.ActivateAutoMilling.Location = new System.Drawing.Point(163, 440);
            this.ActivateAutoMilling.MaximumSize = new System.Drawing.Size(60, 20);
            this.ActivateAutoMilling.MinimumSize = new System.Drawing.Size(60, 20);
            this.ActivateAutoMilling.Name = "ActivateAutoMilling";
            this.ActivateAutoMilling.OffText = "OFF";
            this.ActivateAutoMilling.OnText = "ON";
            this.ActivateAutoMilling.Size = new System.Drawing.Size(60, 20);
            this.ActivateAutoMilling.TabIndex = 70;
            this.ActivateAutoMilling.Value = false;
            // 
            // HerbsToBeMilled
            // 
            this.HerbsToBeMilled.ForeColor = System.Drawing.Color.Black;
            this.HerbsToBeMilled.Location = new System.Drawing.Point(290, 440);
            this.HerbsToBeMilled.Multiline = true;
            this.HerbsToBeMilled.Name = "HerbsToBeMilled";
            this.HerbsToBeMilled.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.HerbsToBeMilled.Size = new System.Drawing.Size(227, 83);
            this.HerbsToBeMilled.TabIndex = 53;
            // 
            // HerbsToBeMilledLabel
            // 
            this.HerbsToBeMilledLabel.BackColor = System.Drawing.Color.Transparent;
            this.HerbsToBeMilledLabel.ForeColor = System.Drawing.Color.Black;
            this.HerbsToBeMilledLabel.Location = new System.Drawing.Point(290, 413);
            this.HerbsToBeMilledLabel.Name = "HerbsToBeMilledLabel";
            this.HerbsToBeMilledLabel.Size = new System.Drawing.Size(204, 22);
            this.HerbsToBeMilledLabel.TabIndex = 52;
            this.HerbsToBeMilledLabel.Text = "Milling list (one item by line)";
            // 
            // MakeStackOfElementalsItemsLabel
            // 
            this.MakeStackOfElementalsItemsLabel.BackColor = System.Drawing.Color.Transparent;
            this.MakeStackOfElementalsItemsLabel.ForeColor = System.Drawing.Color.Black;
            this.MakeStackOfElementalsItemsLabel.Location = new System.Drawing.Point(4, 524);
            this.MakeStackOfElementalsItemsLabel.Name = "MakeStackOfElementalsItemsLabel";
            this.MakeStackOfElementalsItemsLabel.Size = new System.Drawing.Size(154, 22);
            this.MakeStackOfElementalsItemsLabel.TabIndex = 51;
            this.MakeStackOfElementalsItemsLabel.Text = "Auto Make Elemental";
            // 
            // MakeStackOfElementalsItems
            // 
            this.MakeStackOfElementalsItems.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.MakeStackOfElementalsItems.Location = new System.Drawing.Point(163, 524);
            this.MakeStackOfElementalsItems.MaximumSize = new System.Drawing.Size(60, 20);
            this.MakeStackOfElementalsItems.MinimumSize = new System.Drawing.Size(60, 20);
            this.MakeStackOfElementalsItems.Name = "MakeStackOfElementalsItems";
            this.MakeStackOfElementalsItems.OffText = "OFF";
            this.MakeStackOfElementalsItems.OnText = "ON";
            this.MakeStackOfElementalsItems.Size = new System.Drawing.Size(60, 20);
            this.MakeStackOfElementalsItems.TabIndex = 71;
            this.MakeStackOfElementalsItems.Value = true;
            // 
            // OnlyUseProspectingInTownLabel
            // 
            this.OnlyUseProspectingInTownLabel.BackColor = System.Drawing.Color.Transparent;
            this.OnlyUseProspectingInTownLabel.ForeColor = System.Drawing.Color.Black;
            this.OnlyUseProspectingInTownLabel.Location = new System.Drawing.Point(4, 382);
            this.OnlyUseProspectingInTownLabel.Name = "OnlyUseProspectingInTownLabel";
            this.OnlyUseProspectingInTownLabel.Size = new System.Drawing.Size(154, 22);
            this.OnlyUseProspectingInTownLabel.TabIndex = 49;
            this.OnlyUseProspectingInTownLabel.Text = "Prospecting only in town";
            // 
            // OnlyUseProspectingInTown
            // 
            this.OnlyUseProspectingInTown.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.OnlyUseProspectingInTown.Location = new System.Drawing.Point(163, 382);
            this.OnlyUseProspectingInTown.MaximumSize = new System.Drawing.Size(60, 20);
            this.OnlyUseProspectingInTown.MinimumSize = new System.Drawing.Size(60, 20);
            this.OnlyUseProspectingInTown.Name = "OnlyUseProspectingInTown";
            this.OnlyUseProspectingInTown.OffText = "OFF";
            this.OnlyUseProspectingInTown.OnText = "ON";
            this.OnlyUseProspectingInTown.Size = new System.Drawing.Size(60, 20);
            this.OnlyUseProspectingInTown.TabIndex = 72;
            this.OnlyUseProspectingInTown.Value = false;
            // 
            // TimeBetweenEachProspectingAttempt
            // 
            this.TimeBetweenEachProspectingAttempt.BackColor = System.Drawing.SystemColors.Window;
            this.TimeBetweenEachProspectingAttempt.ForeColor = System.Drawing.Color.Black;
            this.TimeBetweenEachProspectingAttempt.Location = new System.Drawing.Point(163, 354);
            this.TimeBetweenEachProspectingAttempt.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.TimeBetweenEachProspectingAttempt.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.TimeBetweenEachProspectingAttempt.Name = "TimeBetweenEachProspectingAttempt";
            this.TimeBetweenEachProspectingAttempt.Size = new System.Drawing.Size(77, 22);
            this.TimeBetweenEachProspectingAttempt.TabIndex = 47;
            this.TimeBetweenEachProspectingAttempt.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            // 
            // TimeBetweenEachProspectingAttemptLabel
            // 
            this.TimeBetweenEachProspectingAttemptLabel.BackColor = System.Drawing.Color.Transparent;
            this.TimeBetweenEachProspectingAttemptLabel.ForeColor = System.Drawing.Color.Black;
            this.TimeBetweenEachProspectingAttemptLabel.Location = new System.Drawing.Point(4, 354);
            this.TimeBetweenEachProspectingAttemptLabel.Name = "TimeBetweenEachProspectingAttemptLabel";
            this.TimeBetweenEachProspectingAttemptLabel.Size = new System.Drawing.Size(154, 22);
            this.TimeBetweenEachProspectingAttemptLabel.TabIndex = 46;
            this.TimeBetweenEachProspectingAttemptLabel.Text = "Prospecting Every (in minute)";
            // 
            // ActivateAutoProspectingLabel
            // 
            this.ActivateAutoProspectingLabel.BackColor = System.Drawing.Color.Transparent;
            this.ActivateAutoProspectingLabel.ForeColor = System.Drawing.Color.Black;
            this.ActivateAutoProspectingLabel.Location = new System.Drawing.Point(4, 326);
            this.ActivateAutoProspectingLabel.Name = "ActivateAutoProspectingLabel";
            this.ActivateAutoProspectingLabel.Size = new System.Drawing.Size(154, 22);
            this.ActivateAutoProspectingLabel.TabIndex = 45;
            this.ActivateAutoProspectingLabel.Text = "Prospecting";
            // 
            // ActivateAutoProspecting
            // 
            this.ActivateAutoProspecting.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.ActivateAutoProspecting.Location = new System.Drawing.Point(163, 326);
            this.ActivateAutoProspecting.MaximumSize = new System.Drawing.Size(60, 20);
            this.ActivateAutoProspecting.MinimumSize = new System.Drawing.Size(60, 20);
            this.ActivateAutoProspecting.Name = "ActivateAutoProspecting";
            this.ActivateAutoProspecting.OffText = "OFF";
            this.ActivateAutoProspecting.OnText = "ON";
            this.ActivateAutoProspecting.Size = new System.Drawing.Size(60, 20);
            this.ActivateAutoProspecting.TabIndex = 73;
            this.ActivateAutoProspecting.Value = false;
            // 
            // MineralsToProspect
            // 
            this.MineralsToProspect.ForeColor = System.Drawing.Color.Black;
            this.MineralsToProspect.Location = new System.Drawing.Point(290, 326);
            this.MineralsToProspect.Multiline = true;
            this.MineralsToProspect.Name = "MineralsToProspect";
            this.MineralsToProspect.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.MineralsToProspect.Size = new System.Drawing.Size(227, 83);
            this.MineralsToProspect.TabIndex = 43;
            // 
            // MineralsToProspectLabel
            // 
            this.MineralsToProspectLabel.BackColor = System.Drawing.Color.Transparent;
            this.MineralsToProspectLabel.ForeColor = System.Drawing.Color.Black;
            this.MineralsToProspectLabel.Location = new System.Drawing.Point(290, 298);
            this.MineralsToProspectLabel.Name = "MineralsToProspectLabel";
            this.MineralsToProspectLabel.Size = new System.Drawing.Size(204, 22);
            this.MineralsToProspectLabel.TabIndex = 42;
            this.MineralsToProspectLabel.Text = "Prospecting list (one item by line)";
            // 
            // labelX61
            // 
            this.labelX61.BackColor = System.Drawing.Color.Transparent;
            this.labelX61.ForeColor = System.Drawing.Color.Black;
            this.labelX61.Location = new System.Drawing.Point(290, 132);
            this.labelX61.Name = "labelX61";
            this.labelX61.Size = new System.Drawing.Size(160, 22);
            this.labelX61.TabIndex = 41;
            this.labelX61.Text = "Smelting";
            // 
            // ActivateAutoSmelting
            // 
            this.ActivateAutoSmelting.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.ActivateAutoSmelting.Location = new System.Drawing.Point(455, 132);
            this.ActivateAutoSmelting.MaximumSize = new System.Drawing.Size(60, 20);
            this.ActivateAutoSmelting.MinimumSize = new System.Drawing.Size(60, 20);
            this.ActivateAutoSmelting.Name = "ActivateAutoSmelting";
            this.ActivateAutoSmelting.OffText = "OFF";
            this.ActivateAutoSmelting.OnText = "ON";
            this.ActivateAutoSmelting.Size = new System.Drawing.Size(60, 20);
            this.ActivateAutoSmelting.TabIndex = 74;
            this.ActivateAutoSmelting.Value = false;
            // 
            // DontHarvestTheFollowingObjectsLabel
            // 
            this.DontHarvestTheFollowingObjectsLabel.BackColor = System.Drawing.Color.Transparent;
            this.DontHarvestTheFollowingObjectsLabel.ForeColor = System.Drawing.Color.Black;
            this.DontHarvestTheFollowingObjectsLabel.Location = new System.Drawing.Point(289, 160);
            this.DontHarvestTheFollowingObjectsLabel.Name = "DontHarvestTheFollowingObjectsLabel";
            this.DontHarvestTheFollowingObjectsLabel.Size = new System.Drawing.Size(209, 22);
            this.DontHarvestTheFollowingObjectsLabel.TabIndex = 38;
            this.DontHarvestTheFollowingObjectsLabel.Text = "Don\'t harvest List (one id per line)";
            // 
            // addBlackListHarvest
            // 
            this.addBlackListHarvest.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.addBlackListHarvest.AutoEllipsis = true;
            this.addBlackListHarvest.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.addBlackListHarvest.ForeColor = System.Drawing.Color.Snow;
            this.addBlackListHarvest.HooverImage = ((System.Drawing.Image)(resources.GetObject("addBlackListHarvest.HooverImage")));
            this.addBlackListHarvest.Image = ((System.Drawing.Image)(resources.GetObject("addBlackListHarvest.Image")));
            this.addBlackListHarvest.Location = new System.Drawing.Point(0, 0);
            this.addBlackListHarvest.Name = "addBlackListHarvest";
            this.addBlackListHarvest.Size = new System.Drawing.Size(106, 29);
            this.addBlackListHarvest.TabIndex = 60;
            this.addBlackListHarvest.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // HarvestDuringLongDistanceMovementsLabel
            // 
            this.HarvestDuringLongDistanceMovementsLabel.BackColor = System.Drawing.Color.Transparent;
            this.HarvestDuringLongDistanceMovementsLabel.ForeColor = System.Drawing.Color.Black;
            this.HarvestDuringLongDistanceMovementsLabel.Location = new System.Drawing.Point(4, 272);
            this.HarvestDuringLongDistanceMovementsLabel.Name = "HarvestDuringLongDistanceMovementsLabel";
            this.HarvestDuringLongDistanceMovementsLabel.Size = new System.Drawing.Size(154, 22);
            this.HarvestDuringLongDistanceMovementsLabel.TabIndex = 34;
            this.HarvestDuringLongDistanceMovementsLabel.Text = "Harvest During Long Move";
            // 
            // HarvestDuringLongDistanceMovements
            // 
            this.HarvestDuringLongDistanceMovements.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.HarvestDuringLongDistanceMovements.Location = new System.Drawing.Point(163, 272);
            this.HarvestDuringLongDistanceMovements.MaximumSize = new System.Drawing.Size(60, 20);
            this.HarvestDuringLongDistanceMovements.MinimumSize = new System.Drawing.Size(60, 20);
            this.HarvestDuringLongDistanceMovements.Name = "HarvestDuringLongDistanceMovements";
            this.HarvestDuringLongDistanceMovements.OffText = "OFF";
            this.HarvestDuringLongDistanceMovements.OnText = "ON";
            this.HarvestDuringLongDistanceMovements.Size = new System.Drawing.Size(60, 20);
            this.HarvestDuringLongDistanceMovements.TabIndex = 75;
            this.HarvestDuringLongDistanceMovements.Value = false;
            // 
            // labelX23
            // 
            this.labelX23.BackColor = System.Drawing.Color.Transparent;
            this.labelX23.ForeColor = System.Drawing.Color.Black;
            this.labelX23.Location = new System.Drawing.Point(289, 104);
            this.labelX23.Name = "labelX23";
            this.labelX23.Size = new System.Drawing.Size(161, 22);
            this.labelX23.TabIndex = 32;
            this.labelX23.Text = "Ninja";
            // 
            // BeastNinjaSkinning
            // 
            this.BeastNinjaSkinning.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.BeastNinjaSkinning.Location = new System.Drawing.Point(455, 104);
            this.BeastNinjaSkinning.MaximumSize = new System.Drawing.Size(60, 20);
            this.BeastNinjaSkinning.MinimumSize = new System.Drawing.Size(60, 20);
            this.BeastNinjaSkinning.Name = "BeastNinjaSkinning";
            this.BeastNinjaSkinning.OffText = "OFF";
            this.BeastNinjaSkinning.OnText = "ON";
            this.BeastNinjaSkinning.Size = new System.Drawing.Size(60, 20);
            this.BeastNinjaSkinning.TabIndex = 76;
            this.BeastNinjaSkinning.Value = false;
            // 
            // GatheringSearchRadius
            // 
            this.GatheringSearchRadius.ForeColor = System.Drawing.Color.Black;
            this.GatheringSearchRadius.Location = new System.Drawing.Point(163, 244);
            this.GatheringSearchRadius.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.GatheringSearchRadius.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.GatheringSearchRadius.Name = "GatheringSearchRadius";
            this.GatheringSearchRadius.Size = new System.Drawing.Size(77, 22);
            this.GatheringSearchRadius.TabIndex = 30;
            this.GatheringSearchRadius.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // GatheringSearchRadiusLabel
            // 
            this.GatheringSearchRadiusLabel.BackColor = System.Drawing.Color.Transparent;
            this.GatheringSearchRadiusLabel.ForeColor = System.Drawing.Color.Black;
            this.GatheringSearchRadiusLabel.Location = new System.Drawing.Point(4, 244);
            this.GatheringSearchRadiusLabel.Name = "GatheringSearchRadiusLabel";
            this.GatheringSearchRadiusLabel.Size = new System.Drawing.Size(154, 22);
            this.GatheringSearchRadiusLabel.TabIndex = 29;
            this.GatheringSearchRadiusLabel.Text = "Search Radius";
            // 
            // DontHarvestIfMoreThanXUnitInAggroRange
            // 
            this.DontHarvestIfMoreThanXUnitInAggroRange.ForeColor = System.Drawing.Color.Black;
            this.DontHarvestIfMoreThanXUnitInAggroRange.Location = new System.Drawing.Point(163, 216);
            this.DontHarvestIfMoreThanXUnitInAggroRange.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.DontHarvestIfMoreThanXUnitInAggroRange.Name = "DontHarvestIfMoreThanXUnitInAggroRange";
            this.DontHarvestIfMoreThanXUnitInAggroRange.Size = new System.Drawing.Size(77, 22);
            this.DontHarvestIfMoreThanXUnitInAggroRange.TabIndex = 28;
            this.DontHarvestIfMoreThanXUnitInAggroRange.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // DontHarvestIfMoreThanXUnitInAggroRangeLabel
            // 
            this.DontHarvestIfMoreThanXUnitInAggroRangeLabel.BackColor = System.Drawing.Color.Transparent;
            this.DontHarvestIfMoreThanXUnitInAggroRangeLabel.ForeColor = System.Drawing.Color.Black;
            this.DontHarvestIfMoreThanXUnitInAggroRangeLabel.Location = new System.Drawing.Point(4, 216);
            this.DontHarvestIfMoreThanXUnitInAggroRangeLabel.Name = "DontHarvestIfMoreThanXUnitInAggroRangeLabel";
            this.DontHarvestIfMoreThanXUnitInAggroRangeLabel.Size = new System.Drawing.Size(154, 22);
            this.DontHarvestIfMoreThanXUnitInAggroRangeLabel.TabIndex = 27;
            this.DontHarvestIfMoreThanXUnitInAggroRangeLabel.Text = "Max Units Near";
            // 
            // ActivateHerbsHarvestingLabel
            // 
            this.ActivateHerbsHarvestingLabel.BackColor = System.Drawing.Color.Transparent;
            this.ActivateHerbsHarvestingLabel.ForeColor = System.Drawing.Color.Black;
            this.ActivateHerbsHarvestingLabel.Location = new System.Drawing.Point(4, 160);
            this.ActivateHerbsHarvestingLabel.Name = "ActivateHerbsHarvestingLabel";
            this.ActivateHerbsHarvestingLabel.Size = new System.Drawing.Size(154, 22);
            this.ActivateHerbsHarvestingLabel.TabIndex = 26;
            this.ActivateHerbsHarvestingLabel.Text = "Harvest Herbs";
            // 
            // ActivateHerbsHarvesting
            // 
            this.ActivateHerbsHarvesting.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.ActivateHerbsHarvesting.Location = new System.Drawing.Point(163, 160);
            this.ActivateHerbsHarvesting.MaximumSize = new System.Drawing.Size(60, 20);
            this.ActivateHerbsHarvesting.MinimumSize = new System.Drawing.Size(60, 20);
            this.ActivateHerbsHarvesting.Name = "ActivateHerbsHarvesting";
            this.ActivateHerbsHarvesting.OffText = "OFF";
            this.ActivateHerbsHarvesting.OnText = "ON";
            this.ActivateHerbsHarvesting.Size = new System.Drawing.Size(60, 20);
            this.ActivateHerbsHarvesting.TabIndex = 77;
            this.ActivateHerbsHarvesting.Value = false;
            // 
            // ActivateVeinsHarvestingLabel
            // 
            this.ActivateVeinsHarvestingLabel.BackColor = System.Drawing.Color.Transparent;
            this.ActivateVeinsHarvestingLabel.ForeColor = System.Drawing.Color.Black;
            this.ActivateVeinsHarvestingLabel.Location = new System.Drawing.Point(4, 132);
            this.ActivateVeinsHarvestingLabel.Name = "ActivateVeinsHarvestingLabel";
            this.ActivateVeinsHarvestingLabel.Size = new System.Drawing.Size(154, 22);
            this.ActivateVeinsHarvestingLabel.TabIndex = 24;
            this.ActivateVeinsHarvestingLabel.Text = "Harvest Minerals";
            // 
            // ActivateVeinsHarvesting
            // 
            this.ActivateVeinsHarvesting.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.ActivateVeinsHarvesting.Location = new System.Drawing.Point(163, 132);
            this.ActivateVeinsHarvesting.MaximumSize = new System.Drawing.Size(60, 20);
            this.ActivateVeinsHarvesting.MinimumSize = new System.Drawing.Size(60, 20);
            this.ActivateVeinsHarvesting.Name = "ActivateVeinsHarvesting";
            this.ActivateVeinsHarvesting.OffText = "OFF";
            this.ActivateVeinsHarvesting.OnText = "ON";
            this.ActivateVeinsHarvesting.Size = new System.Drawing.Size(60, 20);
            this.ActivateVeinsHarvesting.TabIndex = 78;
            this.ActivateVeinsHarvesting.Value = false;
            // 
            // ActivateBeastSkinningLabel
            // 
            this.ActivateBeastSkinningLabel.BackColor = System.Drawing.Color.Transparent;
            this.ActivateBeastSkinningLabel.ForeColor = System.Drawing.Color.Black;
            this.ActivateBeastSkinningLabel.Location = new System.Drawing.Point(4, 104);
            this.ActivateBeastSkinningLabel.Name = "ActivateBeastSkinningLabel";
            this.ActivateBeastSkinningLabel.Size = new System.Drawing.Size(154, 22);
            this.ActivateBeastSkinningLabel.TabIndex = 22;
            this.ActivateBeastSkinningLabel.Text = "Skin Mobs";
            // 
            // ActivateBeastSkinning
            // 
            this.ActivateBeastSkinning.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.ActivateBeastSkinning.Location = new System.Drawing.Point(163, 104);
            this.ActivateBeastSkinning.MaximumSize = new System.Drawing.Size(60, 20);
            this.ActivateBeastSkinning.MinimumSize = new System.Drawing.Size(60, 20);
            this.ActivateBeastSkinning.Name = "ActivateBeastSkinning";
            this.ActivateBeastSkinning.OffText = "OFF";
            this.ActivateBeastSkinning.OnText = "ON";
            this.ActivateBeastSkinning.Size = new System.Drawing.Size(60, 20);
            this.ActivateBeastSkinning.TabIndex = 79;
            this.ActivateBeastSkinning.Value = false;
            // 
            // labelX16
            // 
            this.labelX16.BackColor = System.Drawing.Color.Transparent;
            this.labelX16.ForeColor = System.Drawing.Color.Black;
            this.labelX16.Location = new System.Drawing.Point(290, 77);
            this.labelX16.Name = "labelX16";
            this.labelX16.Size = new System.Drawing.Size(160, 22);
            this.labelX16.TabIndex = 20;
            this.labelX16.Text = "Loot Chests";
            // 
            // ActivateChestLooting
            // 
            this.ActivateChestLooting.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.ActivateChestLooting.Location = new System.Drawing.Point(455, 77);
            this.ActivateChestLooting.MaximumSize = new System.Drawing.Size(60, 20);
            this.ActivateChestLooting.MinimumSize = new System.Drawing.Size(60, 20);
            this.ActivateChestLooting.Name = "ActivateChestLooting";
            this.ActivateChestLooting.OffText = "OFF";
            this.ActivateChestLooting.OnText = "ON";
            this.ActivateChestLooting.Size = new System.Drawing.Size(60, 20);
            this.ActivateChestLooting.TabIndex = 80;
            this.ActivateChestLooting.Value = false;
            // 
            // DontHarvestIfPlayerNearRadius
            // 
            this.DontHarvestIfPlayerNearRadius.ForeColor = System.Drawing.Color.Black;
            this.DontHarvestIfPlayerNearRadius.Location = new System.Drawing.Point(163, 188);
            this.DontHarvestIfPlayerNearRadius.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.DontHarvestIfPlayerNearRadius.Name = "DontHarvestIfPlayerNearRadius";
            this.DontHarvestIfPlayerNearRadius.Size = new System.Drawing.Size(77, 22);
            this.DontHarvestIfPlayerNearRadius.TabIndex = 18;
            this.DontHarvestIfPlayerNearRadius.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // DontHarvestIfPlayerNearRadiusLabel
            // 
            this.DontHarvestIfPlayerNearRadiusLabel.BackColor = System.Drawing.Color.Transparent;
            this.DontHarvestIfPlayerNearRadiusLabel.ForeColor = System.Drawing.Color.Black;
            this.DontHarvestIfPlayerNearRadiusLabel.Location = new System.Drawing.Point(4, 188);
            this.DontHarvestIfPlayerNearRadiusLabel.Name = "DontHarvestIfPlayerNearRadiusLabel";
            this.DontHarvestIfPlayerNearRadiusLabel.Size = new System.Drawing.Size(154, 22);
            this.DontHarvestIfPlayerNearRadiusLabel.TabIndex = 16;
            this.DontHarvestIfPlayerNearRadiusLabel.Text = "Harvest Avoid Players Radius";
            // 
            // ActivateMonsterLootingLabel
            // 
            this.ActivateMonsterLootingLabel.BackColor = System.Drawing.Color.Transparent;
            this.ActivateMonsterLootingLabel.ForeColor = System.Drawing.Color.Black;
            this.ActivateMonsterLootingLabel.Location = new System.Drawing.Point(4, 49);
            this.ActivateMonsterLootingLabel.Name = "ActivateMonsterLootingLabel";
            this.ActivateMonsterLootingLabel.Size = new System.Drawing.Size(154, 22);
            this.ActivateMonsterLootingLabel.TabIndex = 11;
            this.ActivateMonsterLootingLabel.Text = "Loot Mobs";
            // 
            // ActivateMonsterLooting
            // 
            this.ActivateMonsterLooting.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.ActivateMonsterLooting.Location = new System.Drawing.Point(163, 49);
            this.ActivateMonsterLooting.MaximumSize = new System.Drawing.Size(60, 20);
            this.ActivateMonsterLooting.MinimumSize = new System.Drawing.Size(60, 20);
            this.ActivateMonsterLooting.Name = "ActivateMonsterLooting";
            this.ActivateMonsterLooting.OffText = "OFF";
            this.ActivateMonsterLooting.OnText = "ON";
            this.ActivateMonsterLooting.Size = new System.Drawing.Size(60, 20);
            this.ActivateMonsterLooting.TabIndex = 81;
            this.ActivateMonsterLooting.Value = false;
            // 
            // NPCsRepairSellBuyPanelName
            // 
            this.NPCsRepairSellBuyPanelName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.NPCsRepairSellBuyPanelName.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.NPCsRepairSellBuyPanelName.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.NPCsRepairSellBuyPanelName.ContentSize = new System.Drawing.Size(556, 316);
            this.NPCsRepairSellBuyPanelName.Controls.Add(this.SellPurple);
            this.NPCsRepairSellBuyPanelName.Controls.Add(this.SellBlue);
            this.NPCsRepairSellBuyPanelName.Controls.Add(this.SellGreen);
            this.NPCsRepairSellBuyPanelName.Controls.Add(this.SellWhite);
            this.NPCsRepairSellBuyPanelName.Controls.Add(this.SellGray);
            this.NPCsRepairSellBuyPanelName.Controls.Add(this.UseRobotLabel);
            this.NPCsRepairSellBuyPanelName.Controls.Add(this.UseRobot);
            this.NPCsRepairSellBuyPanelName.Controls.Add(this.SellItemsWhenLessThanXSlotLeft);
            this.NPCsRepairSellBuyPanelName.Controls.Add(this.RepairWhenDurabilityIsUnderPercent);
            this.NPCsRepairSellBuyPanelName.Controls.Add(this.SellItemsWhenLessThanXSlotLeftLabel);
            this.NPCsRepairSellBuyPanelName.Controls.Add(this.RepairWhenDurabilityIsUnderPercentLabel);
            this.NPCsRepairSellBuyPanelName.Controls.Add(this.ForceToSellTheseItems);
            this.NPCsRepairSellBuyPanelName.Controls.Add(this.ForceToSellTheseItemsLabel);
            this.NPCsRepairSellBuyPanelName.Controls.Add(this.labelX52);
            this.NPCsRepairSellBuyPanelName.Controls.Add(this.ActivateAutoSellingFeature);
            this.NPCsRepairSellBuyPanelName.Controls.Add(this.ActivateAutoRepairFeatureLabel);
            this.NPCsRepairSellBuyPanelName.Controls.Add(this.ActivateAutoRepairFeature);
            this.NPCsRepairSellBuyPanelName.Controls.Add(this.NumberOfFoodsWeGot);
            this.NPCsRepairSellBuyPanelName.Controls.Add(this.NumberOfFoodsWeGotLabel);
            this.NPCsRepairSellBuyPanelName.Controls.Add(this.NumberOfBeverageWeGot);
            this.NPCsRepairSellBuyPanelName.Controls.Add(this.NumberOfBeverageWeGotLabel);
            this.NPCsRepairSellBuyPanelName.Controls.Add(this.DontSellTheseItems);
            this.NPCsRepairSellBuyPanelName.Controls.Add(this.DontSellTheseItemsLabel);
            this.NPCsRepairSellBuyPanelName.Fold = true;
            this.NPCsRepairSellBuyPanelName.FolderImage = ((System.Drawing.Image)(resources.GetObject("NPCsRepairSellBuyPanelName.FolderImage")));
            this.NPCsRepairSellBuyPanelName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.NPCsRepairSellBuyPanelName.HeaderBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.NPCsRepairSellBuyPanelName.HeaderImage = ((System.Drawing.Image)(resources.GetObject("NPCsRepairSellBuyPanelName.HeaderImage")));
            this.NPCsRepairSellBuyPanelName.HeaderSize = new System.Drawing.Size(556, 36);
            this.NPCsRepairSellBuyPanelName.Location = new System.Drawing.Point(0, 144);
            this.NPCsRepairSellBuyPanelName.Margin = new System.Windows.Forms.Padding(0);
            this.NPCsRepairSellBuyPanelName.MaximumSize = new System.Drawing.Size(556, 0);
            this.NPCsRepairSellBuyPanelName.MinimumSize = new System.Drawing.Size(556, 36);
            this.NPCsRepairSellBuyPanelName.Name = "NPCsRepairSellBuyPanelName";
            this.NPCsRepairSellBuyPanelName.OrderIndex = 5;
            this.NPCsRepairSellBuyPanelName.Padding = new System.Windows.Forms.Padding(0, 0, 0, 12);
            this.NPCsRepairSellBuyPanelName.Size = new System.Drawing.Size(556, 36);
            this.NPCsRepairSellBuyPanelName.TabIndex = 9;
            this.NPCsRepairSellBuyPanelName.TitleFont = new System.Drawing.Font("Segoe UI", 7.65F, System.Drawing.FontStyle.Bold);
            this.NPCsRepairSellBuyPanelName.TitleForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.NPCsRepairSellBuyPanelName.TitleText = "NPCs Management - Repairer, Vendor (Selling or Buying)";
            this.NPCsRepairSellBuyPanelName.UnfolderImage = ((System.Drawing.Image)(resources.GetObject("NPCsRepairSellBuyPanelName.UnfolderImage")));
            // 
            // SellPurple
            // 
            this.SellPurple.ForeColor = System.Drawing.Color.Black;
            this.SellPurple.Location = new System.Drawing.Point(192, 194);
            this.SellPurple.Name = "SellPurple";
            this.SellPurple.Size = new System.Drawing.Size(169, 23);
            this.SellPurple.TabIndex = 33;
            this.SellPurple.Text = "Sell Purple items";
            // 
            // SellBlue
            // 
            this.SellBlue.ForeColor = System.Drawing.Color.Black;
            this.SellBlue.Location = new System.Drawing.Point(11, 194);
            this.SellBlue.Name = "SellBlue";
            this.SellBlue.Size = new System.Drawing.Size(175, 23);
            this.SellBlue.TabIndex = 32;
            this.SellBlue.Text = "Sell Blue items";
            // 
            // SellGreen
            // 
            this.SellGreen.ForeColor = System.Drawing.Color.Black;
            this.SellGreen.Location = new System.Drawing.Point(394, 165);
            this.SellGreen.Name = "SellGreen";
            this.SellGreen.Size = new System.Drawing.Size(135, 23);
            this.SellGreen.TabIndex = 31;
            this.SellGreen.Text = "Sell Green items";
            // 
            // SellWhite
            // 
            this.SellWhite.ForeColor = System.Drawing.Color.Black;
            this.SellWhite.Location = new System.Drawing.Point(192, 165);
            this.SellWhite.Name = "SellWhite";
            this.SellWhite.Size = new System.Drawing.Size(188, 23);
            this.SellWhite.TabIndex = 30;
            this.SellWhite.Text = "Sell White items";
            // 
            // SellGray
            // 
            this.SellGray.ForeColor = System.Drawing.Color.Black;
            this.SellGray.Location = new System.Drawing.Point(11, 165);
            this.SellGray.Name = "SellGray";
            this.SellGray.Size = new System.Drawing.Size(175, 23);
            this.SellGray.TabIndex = 29;
            this.SellGray.Text = "Sell Gray items";
            // 
            // UseRobotLabel
            // 
            this.UseRobotLabel.BackColor = System.Drawing.Color.Transparent;
            this.UseRobotLabel.ForeColor = System.Drawing.Color.Black;
            this.UseRobotLabel.Location = new System.Drawing.Point(4, 54);
            this.UseRobotLabel.Name = "UseRobotLabel";
            this.UseRobotLabel.Size = new System.Drawing.Size(154, 22);
            this.UseRobotLabel.TabIndex = 41;
            this.UseRobotLabel.Text = "Use robot when possible";
            // 
            // UseRobot
            // 
            this.UseRobot.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.UseRobot.Location = new System.Drawing.Point(163, 54);
            this.UseRobot.MaximumSize = new System.Drawing.Size(60, 20);
            this.UseRobot.MinimumSize = new System.Drawing.Size(60, 20);
            this.UseRobot.Name = "UseRobot";
            this.UseRobot.OffText = "OFF";
            this.UseRobot.OnText = "ON";
            this.UseRobot.Size = new System.Drawing.Size(60, 20);
            this.UseRobot.TabIndex = 42;
            this.UseRobot.Value = false;
            // 
            // SellItemsWhenLessThanXSlotLeft
            // 
            this.SellItemsWhenLessThanXSlotLeft.ForeColor = System.Drawing.Color.Black;
            this.SellItemsWhenLessThanXSlotLeft.Location = new System.Drawing.Point(488, 138);
            this.SellItemsWhenLessThanXSlotLeft.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.SellItemsWhenLessThanXSlotLeft.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SellItemsWhenLessThanXSlotLeft.Name = "SellItemsWhenLessThanXSlotLeft";
            this.SellItemsWhenLessThanXSlotLeft.Size = new System.Drawing.Size(41, 22);
            this.SellItemsWhenLessThanXSlotLeft.TabIndex = 39;
            this.SellItemsWhenLessThanXSlotLeft.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // RepairWhenDurabilityIsUnderPercent
            // 
            this.RepairWhenDurabilityIsUnderPercent.ForeColor = System.Drawing.Color.Black;
            this.RepairWhenDurabilityIsUnderPercent.Location = new System.Drawing.Point(488, 110);
            this.RepairWhenDurabilityIsUnderPercent.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.RepairWhenDurabilityIsUnderPercent.Name = "RepairWhenDurabilityIsUnderPercent";
            this.RepairWhenDurabilityIsUnderPercent.Size = new System.Drawing.Size(41, 22);
            this.RepairWhenDurabilityIsUnderPercent.TabIndex = 38;
            this.RepairWhenDurabilityIsUnderPercent.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // SellItemsWhenLessThanXSlotLeftLabel
            // 
            this.SellItemsWhenLessThanXSlotLeftLabel.BackColor = System.Drawing.Color.Transparent;
            this.SellItemsWhenLessThanXSlotLeftLabel.ForeColor = System.Drawing.Color.Black;
            this.SellItemsWhenLessThanXSlotLeftLabel.Location = new System.Drawing.Point(280, 136);
            this.SellItemsWhenLessThanXSlotLeftLabel.Name = "SellItemsWhenLessThanXSlotLeftLabel";
            this.SellItemsWhenLessThanXSlotLeftLabel.Size = new System.Drawing.Size(196, 23);
            this.SellItemsWhenLessThanXSlotLeftLabel.TabIndex = 37;
            this.SellItemsWhenLessThanXSlotLeftLabel.Text = "Sell items when less than X slot left";
            // 
            // RepairWhenDurabilityIsUnderPercentLabel
            // 
            this.RepairWhenDurabilityIsUnderPercentLabel.BackColor = System.Drawing.Color.Transparent;
            this.RepairWhenDurabilityIsUnderPercentLabel.ForeColor = System.Drawing.Color.Black;
            this.RepairWhenDurabilityIsUnderPercentLabel.Location = new System.Drawing.Point(279, 108);
            this.RepairWhenDurabilityIsUnderPercentLabel.Name = "RepairWhenDurabilityIsUnderPercentLabel";
            this.RepairWhenDurabilityIsUnderPercentLabel.Size = new System.Drawing.Size(197, 23);
            this.RepairWhenDurabilityIsUnderPercentLabel.TabIndex = 36;
            this.RepairWhenDurabilityIsUnderPercentLabel.Text = "Repair when Durability is under than X %";
            // 
            // ForceToSellTheseItems
            // 
            this.ForceToSellTheseItems.ForeColor = System.Drawing.Color.Black;
            this.ForceToSellTheseItems.Location = new System.Drawing.Point(272, 254);
            this.ForceToSellTheseItems.Multiline = true;
            this.ForceToSellTheseItems.Name = "ForceToSellTheseItems";
            this.ForceToSellTheseItems.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ForceToSellTheseItems.Size = new System.Drawing.Size(227, 83);
            this.ForceToSellTheseItems.TabIndex = 35;
            // 
            // ForceToSellTheseItemsLabel
            // 
            this.ForceToSellTheseItemsLabel.BackColor = System.Drawing.Color.Transparent;
            this.ForceToSellTheseItemsLabel.ForeColor = System.Drawing.Color.Black;
            this.ForceToSellTheseItemsLabel.Location = new System.Drawing.Point(272, 226);
            this.ForceToSellTheseItemsLabel.Name = "ForceToSellTheseItemsLabel";
            this.ForceToSellTheseItemsLabel.Size = new System.Drawing.Size(227, 22);
            this.ForceToSellTheseItemsLabel.TabIndex = 34;
            this.ForceToSellTheseItemsLabel.Text = "Force Sell List (one item by line)";
            // 
            // labelX52
            // 
            this.labelX52.BackColor = System.Drawing.Color.Transparent;
            this.labelX52.ForeColor = System.Drawing.Color.Black;
            this.labelX52.Location = new System.Drawing.Point(3, 137);
            this.labelX52.Name = "labelX52";
            this.labelX52.Size = new System.Drawing.Size(154, 22);
            this.labelX52.TabIndex = 28;
            this.labelX52.Text = "Selling";
            // 
            // ActivateAutoSellingFeature
            // 
            this.ActivateAutoSellingFeature.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.ActivateAutoSellingFeature.Location = new System.Drawing.Point(162, 137);
            this.ActivateAutoSellingFeature.MaximumSize = new System.Drawing.Size(60, 20);
            this.ActivateAutoSellingFeature.MinimumSize = new System.Drawing.Size(60, 20);
            this.ActivateAutoSellingFeature.Name = "ActivateAutoSellingFeature";
            this.ActivateAutoSellingFeature.OffText = "OFF";
            this.ActivateAutoSellingFeature.OnText = "ON";
            this.ActivateAutoSellingFeature.Size = new System.Drawing.Size(60, 20);
            this.ActivateAutoSellingFeature.TabIndex = 43;
            this.ActivateAutoSellingFeature.Value = false;
            // 
            // ActivateAutoRepairFeatureLabel
            // 
            this.ActivateAutoRepairFeatureLabel.BackColor = System.Drawing.Color.Transparent;
            this.ActivateAutoRepairFeatureLabel.ForeColor = System.Drawing.Color.Black;
            this.ActivateAutoRepairFeatureLabel.Location = new System.Drawing.Point(3, 109);
            this.ActivateAutoRepairFeatureLabel.Name = "ActivateAutoRepairFeatureLabel";
            this.ActivateAutoRepairFeatureLabel.Size = new System.Drawing.Size(154, 22);
            this.ActivateAutoRepairFeatureLabel.TabIndex = 26;
            this.ActivateAutoRepairFeatureLabel.Text = "Repair";
            // 
            // ActivateAutoRepairFeature
            // 
            this.ActivateAutoRepairFeature.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.ActivateAutoRepairFeature.Location = new System.Drawing.Point(162, 109);
            this.ActivateAutoRepairFeature.MaximumSize = new System.Drawing.Size(60, 20);
            this.ActivateAutoRepairFeature.MinimumSize = new System.Drawing.Size(60, 20);
            this.ActivateAutoRepairFeature.Name = "ActivateAutoRepairFeature";
            this.ActivateAutoRepairFeature.OffText = "OFF";
            this.ActivateAutoRepairFeature.OnText = "ON";
            this.ActivateAutoRepairFeature.Size = new System.Drawing.Size(60, 20);
            this.ActivateAutoRepairFeature.TabIndex = 44;
            this.ActivateAutoRepairFeature.Value = false;
            // 
            // NumberOfFoodsWeGot
            // 
            this.NumberOfFoodsWeGot.ForeColor = System.Drawing.Color.Black;
            this.NumberOfFoodsWeGot.Location = new System.Drawing.Point(466, 80);
            this.NumberOfFoodsWeGot.Name = "NumberOfFoodsWeGot";
            this.NumberOfFoodsWeGot.Size = new System.Drawing.Size(63, 22);
            this.NumberOfFoodsWeGot.TabIndex = 24;
            // 
            // NumberOfFoodsWeGotLabel
            // 
            this.NumberOfFoodsWeGotLabel.BackColor = System.Drawing.Color.Transparent;
            this.NumberOfFoodsWeGotLabel.ForeColor = System.Drawing.Color.Black;
            this.NumberOfFoodsWeGotLabel.Location = new System.Drawing.Point(280, 80);
            this.NumberOfFoodsWeGotLabel.Name = "NumberOfFoodsWeGotLabel";
            this.NumberOfFoodsWeGotLabel.Size = new System.Drawing.Size(154, 22);
            this.NumberOfFoodsWeGotLabel.TabIndex = 23;
            this.NumberOfFoodsWeGotLabel.Text = "Food Amount";
            // 
            // NumberOfBeverageWeGot
            // 
            this.NumberOfBeverageWeGot.ForeColor = System.Drawing.Color.Black;
            this.NumberOfBeverageWeGot.Location = new System.Drawing.Point(162, 81);
            this.NumberOfBeverageWeGot.Name = "NumberOfBeverageWeGot";
            this.NumberOfBeverageWeGot.Size = new System.Drawing.Size(63, 22);
            this.NumberOfBeverageWeGot.TabIndex = 18;
            // 
            // NumberOfBeverageWeGotLabel
            // 
            this.NumberOfBeverageWeGotLabel.BackColor = System.Drawing.Color.Transparent;
            this.NumberOfBeverageWeGotLabel.ForeColor = System.Drawing.Color.Black;
            this.NumberOfBeverageWeGotLabel.Location = new System.Drawing.Point(3, 81);
            this.NumberOfBeverageWeGotLabel.Name = "NumberOfBeverageWeGotLabel";
            this.NumberOfBeverageWeGotLabel.Size = new System.Drawing.Size(154, 22);
            this.NumberOfBeverageWeGotLabel.TabIndex = 16;
            this.NumberOfBeverageWeGotLabel.Text = "Drink Amount";
            // 
            // DontSellTheseItems
            // 
            this.DontSellTheseItems.ForeColor = System.Drawing.Color.Black;
            this.DontSellTheseItems.Location = new System.Drawing.Point(12, 254);
            this.DontSellTheseItems.Multiline = true;
            this.DontSellTheseItems.Name = "DontSellTheseItems";
            this.DontSellTheseItems.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.DontSellTheseItems.Size = new System.Drawing.Size(227, 83);
            this.DontSellTheseItems.TabIndex = 15;
            // 
            // DontSellTheseItemsLabel
            // 
            this.DontSellTheseItemsLabel.BackColor = System.Drawing.Color.Transparent;
            this.DontSellTheseItemsLabel.ForeColor = System.Drawing.Color.Black;
            this.DontSellTheseItemsLabel.Location = new System.Drawing.Point(12, 226);
            this.DontSellTheseItemsLabel.Name = "DontSellTheseItemsLabel";
            this.DontSellTheseItemsLabel.Size = new System.Drawing.Size(227, 22);
            this.DontSellTheseItemsLabel.TabIndex = 14;
            this.DontSellTheseItemsLabel.Text = "Do not Sell List (one item by line)";
            // 
            // MailsManagementPanelName
            // 
            this.MailsManagementPanelName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.MailsManagementPanelName.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.MailsManagementPanelName.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.MailsManagementPanelName.ContentSize = new System.Drawing.Size(556, 316);
            this.MailsManagementPanelName.Controls.Add(this.MailPurple);
            this.MailsManagementPanelName.Controls.Add(this.MailBlue);
            this.MailsManagementPanelName.Controls.Add(this.MailGreen);
            this.MailsManagementPanelName.Controls.Add(this.MailWhite);
            this.MailsManagementPanelName.Controls.Add(this.MailGray);
            this.MailsManagementPanelName.Controls.Add(this.UseMollELabel);
            this.MailsManagementPanelName.Controls.Add(this.UseMollE);
            this.MailsManagementPanelName.Controls.Add(this.SendMailWhenLessThanXSlotLeft);
            this.MailsManagementPanelName.Controls.Add(this.SendMailWhenLessThanXSlotLeftLabel);
            this.MailsManagementPanelName.Controls.Add(this.MaillingFeatureRecipient);
            this.MailsManagementPanelName.Controls.Add(this.MaillingFeatureRecipientLabel);
            this.MailsManagementPanelName.Controls.Add(this.MaillingFeatureSubject);
            this.MailsManagementPanelName.Controls.Add(this.ForceToMailTheseItems);
            this.MailsManagementPanelName.Controls.Add(this.ForceToMailTheseItemsLabel);
            this.MailsManagementPanelName.Controls.Add(this.MaillingFeatureSubjectLabel);
            this.MailsManagementPanelName.Controls.Add(this.ActivateAutoMaillingFeatureLabel);
            this.MailsManagementPanelName.Controls.Add(this.ActivateAutoMaillingFeature);
            this.MailsManagementPanelName.Controls.Add(this.DontMailTheseItems);
            this.MailsManagementPanelName.Controls.Add(this.DontMailTheseItemsLabel);
            this.MailsManagementPanelName.Fold = true;
            this.MailsManagementPanelName.FolderImage = ((System.Drawing.Image)(resources.GetObject("MailsManagementPanelName.FolderImage")));
            this.MailsManagementPanelName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.MailsManagementPanelName.HeaderBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.MailsManagementPanelName.HeaderImage = ((System.Drawing.Image)(resources.GetObject("MailsManagementPanelName.HeaderImage")));
            this.MailsManagementPanelName.HeaderSize = new System.Drawing.Size(556, 36);
            this.MailsManagementPanelName.Location = new System.Drawing.Point(0, 180);
            this.MailsManagementPanelName.Margin = new System.Windows.Forms.Padding(0);
            this.MailsManagementPanelName.MaximumSize = new System.Drawing.Size(556, 0);
            this.MailsManagementPanelName.MinimumSize = new System.Drawing.Size(556, 36);
            this.MailsManagementPanelName.Name = "MailsManagementPanelName";
            this.MailsManagementPanelName.OrderIndex = 6;
            this.MailsManagementPanelName.Padding = new System.Windows.Forms.Padding(0, 0, 0, 12);
            this.MailsManagementPanelName.Size = new System.Drawing.Size(556, 36);
            this.MailsManagementPanelName.TabIndex = 10;
            this.MailsManagementPanelName.TitleFont = new System.Drawing.Font("Segoe UI", 7.65F, System.Drawing.FontStyle.Bold);
            this.MailsManagementPanelName.TitleForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.MailsManagementPanelName.TitleText = "Mails Management (Allow you to send items to Alt. Char)";
            this.MailsManagementPanelName.UnfolderImage = ((System.Drawing.Image)(resources.GetObject("MailsManagementPanelName.UnfolderImage")));
            // 
            // MailPurple
            // 
            this.MailPurple.ForeColor = System.Drawing.Color.Black;
            this.MailPurple.Location = new System.Drawing.Point(194, 190);
            this.MailPurple.Name = "MailPurple";
            this.MailPurple.Size = new System.Drawing.Size(166, 23);
            this.MailPurple.TabIndex = 33;
            this.MailPurple.Text = "Mail Purple items";
            // 
            // MailBlue
            // 
            this.MailBlue.ForeColor = System.Drawing.Color.Black;
            this.MailBlue.Location = new System.Drawing.Point(11, 194);
            this.MailBlue.Name = "MailBlue";
            this.MailBlue.Size = new System.Drawing.Size(175, 23);
            this.MailBlue.TabIndex = 32;
            this.MailBlue.Text = "Mail Blue items";
            // 
            // MailGreen
            // 
            this.MailGreen.ForeColor = System.Drawing.Color.Black;
            this.MailGreen.Location = new System.Drawing.Point(380, 162);
            this.MailGreen.Name = "MailGreen";
            this.MailGreen.Size = new System.Drawing.Size(166, 23);
            this.MailGreen.TabIndex = 31;
            this.MailGreen.Text = "Mail Green items";
            // 
            // MailWhite
            // 
            this.MailWhite.ForeColor = System.Drawing.Color.Black;
            this.MailWhite.Location = new System.Drawing.Point(194, 163);
            this.MailWhite.Name = "MailWhite";
            this.MailWhite.Size = new System.Drawing.Size(166, 23);
            this.MailWhite.TabIndex = 30;
            this.MailWhite.Text = "Mail White items";
            // 
            // MailGray
            // 
            this.MailGray.ForeColor = System.Drawing.Color.Black;
            this.MailGray.Location = new System.Drawing.Point(11, 162);
            this.MailGray.Name = "MailGray";
            this.MailGray.Size = new System.Drawing.Size(175, 23);
            this.MailGray.TabIndex = 29;
            this.MailGray.Text = "Mail Gray items";
            // 
            // UseMollELabel
            // 
            this.UseMollELabel.BackColor = System.Drawing.Color.Transparent;
            this.UseMollELabel.ForeColor = System.Drawing.Color.Black;
            this.UseMollELabel.Location = new System.Drawing.Point(289, 50);
            this.UseMollELabel.Name = "UseMollELabel";
            this.UseMollELabel.Size = new System.Drawing.Size(154, 22);
            this.UseMollELabel.TabIndex = 43;
            this.UseMollELabel.Text = "Use MOLL-E if up";
            // 
            // UseMollE
            // 
            this.UseMollE.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.UseMollE.Location = new System.Drawing.Point(448, 50);
            this.UseMollE.MaximumSize = new System.Drawing.Size(60, 20);
            this.UseMollE.MinimumSize = new System.Drawing.Size(60, 20);
            this.UseMollE.Name = "UseMollE";
            this.UseMollE.OffText = "OFF";
            this.UseMollE.OnText = "ON";
            this.UseMollE.Size = new System.Drawing.Size(60, 20);
            this.UseMollE.TabIndex = 44;
            this.UseMollE.Value = false;
            // 
            // SendMailWhenLessThanXSlotLeft
            // 
            this.SendMailWhenLessThanXSlotLeft.ForeColor = System.Drawing.Color.Black;
            this.SendMailWhenLessThanXSlotLeft.Location = new System.Drawing.Point(211, 79);
            this.SendMailWhenLessThanXSlotLeft.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.SendMailWhenLessThanXSlotLeft.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SendMailWhenLessThanXSlotLeft.Name = "SendMailWhenLessThanXSlotLeft";
            this.SendMailWhenLessThanXSlotLeft.Size = new System.Drawing.Size(41, 22);
            this.SendMailWhenLessThanXSlotLeft.TabIndex = 41;
            this.SendMailWhenLessThanXSlotLeft.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // SendMailWhenLessThanXSlotLeftLabel
            // 
            this.SendMailWhenLessThanXSlotLeftLabel.BackColor = System.Drawing.Color.Transparent;
            this.SendMailWhenLessThanXSlotLeftLabel.ForeColor = System.Drawing.Color.Black;
            this.SendMailWhenLessThanXSlotLeftLabel.Location = new System.Drawing.Point(3, 77);
            this.SendMailWhenLessThanXSlotLeftLabel.Name = "SendMailWhenLessThanXSlotLeftLabel";
            this.SendMailWhenLessThanXSlotLeftLabel.Size = new System.Drawing.Size(196, 23);
            this.SendMailWhenLessThanXSlotLeftLabel.TabIndex = 40;
            this.SendMailWhenLessThanXSlotLeftLabel.Text = "Send Mail when less than X slot left";
            // 
            // MaillingFeatureRecipient
            // 
            this.MaillingFeatureRecipient.ForeColor = System.Drawing.Color.Black;
            this.MaillingFeatureRecipient.Location = new System.Drawing.Point(162, 109);
            this.MaillingFeatureRecipient.Name = "MaillingFeatureRecipient";
            this.MaillingFeatureRecipient.Size = new System.Drawing.Size(175, 22);
            this.MaillingFeatureRecipient.TabIndex = 38;
            // 
            // MaillingFeatureRecipientLabel
            // 
            this.MaillingFeatureRecipientLabel.BackColor = System.Drawing.Color.Transparent;
            this.MaillingFeatureRecipientLabel.ForeColor = System.Drawing.Color.Black;
            this.MaillingFeatureRecipientLabel.Location = new System.Drawing.Point(3, 106);
            this.MaillingFeatureRecipientLabel.Name = "MaillingFeatureRecipientLabel";
            this.MaillingFeatureRecipientLabel.Size = new System.Drawing.Size(154, 22);
            this.MaillingFeatureRecipientLabel.TabIndex = 37;
            this.MaillingFeatureRecipientLabel.Text = "Mail Recipient";
            // 
            // MaillingFeatureSubject
            // 
            this.MaillingFeatureSubject.ForeColor = System.Drawing.Color.Black;
            this.MaillingFeatureSubject.Location = new System.Drawing.Point(162, 137);
            this.MaillingFeatureSubject.Name = "MaillingFeatureSubject";
            this.MaillingFeatureSubject.Size = new System.Drawing.Size(175, 22);
            this.MaillingFeatureSubject.TabIndex = 36;
            // 
            // ForceToMailTheseItems
            // 
            this.ForceToMailTheseItems.ForeColor = System.Drawing.Color.Black;
            this.ForceToMailTheseItems.Location = new System.Drawing.Point(272, 251);
            this.ForceToMailTheseItems.Multiline = true;
            this.ForceToMailTheseItems.Name = "ForceToMailTheseItems";
            this.ForceToMailTheseItems.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ForceToMailTheseItems.Size = new System.Drawing.Size(227, 83);
            this.ForceToMailTheseItems.TabIndex = 35;
            // 
            // ForceToMailTheseItemsLabel
            // 
            this.ForceToMailTheseItemsLabel.BackColor = System.Drawing.Color.Transparent;
            this.ForceToMailTheseItemsLabel.ForeColor = System.Drawing.Color.Black;
            this.ForceToMailTheseItemsLabel.Location = new System.Drawing.Point(272, 223);
            this.ForceToMailTheseItemsLabel.Name = "ForceToMailTheseItemsLabel";
            this.ForceToMailTheseItemsLabel.Size = new System.Drawing.Size(227, 22);
            this.ForceToMailTheseItemsLabel.TabIndex = 34;
            this.ForceToMailTheseItemsLabel.Text = "Force Mail List (one item by line)";
            // 
            // MaillingFeatureSubjectLabel
            // 
            this.MaillingFeatureSubjectLabel.BackColor = System.Drawing.Color.Transparent;
            this.MaillingFeatureSubjectLabel.ForeColor = System.Drawing.Color.Black;
            this.MaillingFeatureSubjectLabel.Location = new System.Drawing.Point(3, 134);
            this.MaillingFeatureSubjectLabel.Name = "MaillingFeatureSubjectLabel";
            this.MaillingFeatureSubjectLabel.Size = new System.Drawing.Size(154, 22);
            this.MaillingFeatureSubjectLabel.TabIndex = 28;
            this.MaillingFeatureSubjectLabel.Text = "Subject";
            // 
            // ActivateAutoMaillingFeatureLabel
            // 
            this.ActivateAutoMaillingFeatureLabel.BackColor = System.Drawing.Color.Transparent;
            this.ActivateAutoMaillingFeatureLabel.ForeColor = System.Drawing.Color.Black;
            this.ActivateAutoMaillingFeatureLabel.Location = new System.Drawing.Point(3, 51);
            this.ActivateAutoMaillingFeatureLabel.Name = "ActivateAutoMaillingFeatureLabel";
            this.ActivateAutoMaillingFeatureLabel.Size = new System.Drawing.Size(154, 22);
            this.ActivateAutoMaillingFeatureLabel.TabIndex = 26;
            this.ActivateAutoMaillingFeatureLabel.Text = "Use Mail";
            // 
            // ActivateAutoMaillingFeature
            // 
            this.ActivateAutoMaillingFeature.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.ActivateAutoMaillingFeature.Location = new System.Drawing.Point(162, 51);
            this.ActivateAutoMaillingFeature.MaximumSize = new System.Drawing.Size(60, 20);
            this.ActivateAutoMaillingFeature.MinimumSize = new System.Drawing.Size(60, 20);
            this.ActivateAutoMaillingFeature.Name = "ActivateAutoMaillingFeature";
            this.ActivateAutoMaillingFeature.OffText = "OFF";
            this.ActivateAutoMaillingFeature.OnText = "ON";
            this.ActivateAutoMaillingFeature.Size = new System.Drawing.Size(60, 20);
            this.ActivateAutoMaillingFeature.TabIndex = 45;
            this.ActivateAutoMaillingFeature.Value = false;
            // 
            // DontMailTheseItems
            // 
            this.DontMailTheseItems.ForeColor = System.Drawing.Color.Black;
            this.DontMailTheseItems.Location = new System.Drawing.Point(12, 251);
            this.DontMailTheseItems.Multiline = true;
            this.DontMailTheseItems.Name = "DontMailTheseItems";
            this.DontMailTheseItems.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.DontMailTheseItems.Size = new System.Drawing.Size(227, 83);
            this.DontMailTheseItems.TabIndex = 15;
            // 
            // DontMailTheseItemsLabel
            // 
            this.DontMailTheseItemsLabel.BackColor = System.Drawing.Color.Transparent;
            this.DontMailTheseItemsLabel.ForeColor = System.Drawing.Color.Black;
            this.DontMailTheseItemsLabel.Location = new System.Drawing.Point(12, 223);
            this.DontMailTheseItemsLabel.Name = "DontMailTheseItemsLabel";
            this.DontMailTheseItemsLabel.Size = new System.Drawing.Size(228, 22);
            this.DontMailTheseItemsLabel.TabIndex = 14;
            this.DontMailTheseItemsLabel.Text = "Do not Mail List (one item by line)";
            // 
            // RegenerationManagementPanelName
            // 
            this.RegenerationManagementPanelName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.RegenerationManagementPanelName.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.RegenerationManagementPanelName.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.RegenerationManagementPanelName.ContentSize = new System.Drawing.Size(556, 78);
            this.RegenerationManagementPanelName.Controls.Add(this.DoRegenManaIfLow);
            this.RegenerationManagementPanelName.Controls.Add(this.labelX9);
            this.RegenerationManagementPanelName.Controls.Add(this.labelX10);
            this.RegenerationManagementPanelName.Controls.Add(this.DrinkBeverageWhenManaIsUnderXPercent);
            this.RegenerationManagementPanelName.Controls.Add(this.BeverageName);
            this.RegenerationManagementPanelName.Controls.Add(this.BeverageNameLabel);
            this.RegenerationManagementPanelName.Controls.Add(this.labelX14);
            this.RegenerationManagementPanelName.Controls.Add(this.labelX13);
            this.RegenerationManagementPanelName.Controls.Add(this.EatFoodWhenHealthIsUnderXPercent);
            this.RegenerationManagementPanelName.Controls.Add(this.FoodName);
            this.RegenerationManagementPanelName.Controls.Add(this.FoodNameLabel);
            this.RegenerationManagementPanelName.Fold = true;
            this.RegenerationManagementPanelName.FolderImage = ((System.Drawing.Image)(resources.GetObject("RegenerationManagementPanelName.FolderImage")));
            this.RegenerationManagementPanelName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.RegenerationManagementPanelName.HeaderBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.RegenerationManagementPanelName.HeaderImage = ((System.Drawing.Image)(resources.GetObject("RegenerationManagementPanelName.HeaderImage")));
            this.RegenerationManagementPanelName.HeaderSize = new System.Drawing.Size(556, 36);
            this.RegenerationManagementPanelName.Location = new System.Drawing.Point(0, 216);
            this.RegenerationManagementPanelName.Margin = new System.Windows.Forms.Padding(0);
            this.RegenerationManagementPanelName.MaximumSize = new System.Drawing.Size(556, 0);
            this.RegenerationManagementPanelName.Name = "RegenerationManagementPanelName";
            this.RegenerationManagementPanelName.OrderIndex = 7;
            this.RegenerationManagementPanelName.Padding = new System.Windows.Forms.Padding(0, 0, 0, 12);
            this.RegenerationManagementPanelName.Size = new System.Drawing.Size(556, 36);
            this.RegenerationManagementPanelName.TabIndex = 4;
            this.RegenerationManagementPanelName.TitleFont = new System.Drawing.Font("Segoe UI", 7.65F, System.Drawing.FontStyle.Bold);
            this.RegenerationManagementPanelName.TitleForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.RegenerationManagementPanelName.TitleText = "Regeneration Management - Food / Drink && percentage to be used";
            this.RegenerationManagementPanelName.UnfolderImage = ((System.Drawing.Image)(resources.GetObject("RegenerationManagementPanelName.UnfolderImage")));
            // 
            // DoRegenManaIfLow
            // 
            this.DoRegenManaIfLow.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.DoRegenManaIfLow.Location = new System.Drawing.Point(466, 75);
            this.DoRegenManaIfLow.MaximumSize = new System.Drawing.Size(60, 20);
            this.DoRegenManaIfLow.MinimumSize = new System.Drawing.Size(60, 20);
            this.DoRegenManaIfLow.Name = "DoRegenManaIfLow";
            this.DoRegenManaIfLow.OffText = "OFF";
            this.DoRegenManaIfLow.OnText = "ON";
            this.DoRegenManaIfLow.Size = new System.Drawing.Size(60, 20);
            this.DoRegenManaIfLow.TabIndex = 2;
            this.DoRegenManaIfLow.Value = false;
            // 
            // labelX9
            // 
            this.labelX9.BackColor = System.Drawing.Color.Transparent;
            this.labelX9.ForeColor = System.Drawing.Color.Black;
            this.labelX9.Location = new System.Drawing.Point(394, 74);
            this.labelX9.Name = "labelX9";
            this.labelX9.Size = new System.Drawing.Size(30, 22);
            this.labelX9.TabIndex = 25;
            this.labelX9.Text = "%";
            // 
            // labelX10
            // 
            this.labelX10.BackColor = System.Drawing.Color.Transparent;
            this.labelX10.ForeColor = System.Drawing.Color.Black;
            this.labelX10.Location = new System.Drawing.Point(312, 74);
            this.labelX10.Name = "labelX10";
            this.labelX10.Size = new System.Drawing.Size(17, 22);
            this.labelX10.TabIndex = 24;
            this.labelX10.Text = "on";
            // 
            // DrinkBeverageWhenManaIsUnderXPercent
            // 
            this.DrinkBeverageWhenManaIsUnderXPercent.ForeColor = System.Drawing.Color.Black;
            this.DrinkBeverageWhenManaIsUnderXPercent.Location = new System.Drawing.Point(335, 77);
            this.DrinkBeverageWhenManaIsUnderXPercent.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.DrinkBeverageWhenManaIsUnderXPercent.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.DrinkBeverageWhenManaIsUnderXPercent.Name = "DrinkBeverageWhenManaIsUnderXPercent";
            this.DrinkBeverageWhenManaIsUnderXPercent.Size = new System.Drawing.Size(53, 22);
            this.DrinkBeverageWhenManaIsUnderXPercent.TabIndex = 23;
            this.DrinkBeverageWhenManaIsUnderXPercent.Value = new decimal(new int[] {
            35,
            0,
            0,
            0});
            // 
            // BeverageName
            // 
            this.BeverageName.ForeColor = System.Drawing.Color.Black;
            this.BeverageName.Location = new System.Drawing.Point(162, 77);
            this.BeverageName.Name = "BeverageName";
            this.BeverageName.Size = new System.Drawing.Size(144, 22);
            this.BeverageName.TabIndex = 22;
            // 
            // BeverageNameLabel
            // 
            this.BeverageNameLabel.BackColor = System.Drawing.Color.Transparent;
            this.BeverageNameLabel.ForeColor = System.Drawing.Color.Black;
            this.BeverageNameLabel.Location = new System.Drawing.Point(3, 74);
            this.BeverageNameLabel.Name = "BeverageNameLabel";
            this.BeverageNameLabel.Size = new System.Drawing.Size(154, 22);
            this.BeverageNameLabel.TabIndex = 21;
            this.BeverageNameLabel.Text = "Drink";
            // 
            // labelX14
            // 
            this.labelX14.BackColor = System.Drawing.Color.Transparent;
            this.labelX14.ForeColor = System.Drawing.Color.Black;
            this.labelX14.Location = new System.Drawing.Point(394, 46);
            this.labelX14.Name = "labelX14";
            this.labelX14.Size = new System.Drawing.Size(30, 22);
            this.labelX14.TabIndex = 20;
            this.labelX14.Text = "%";
            // 
            // labelX13
            // 
            this.labelX13.BackColor = System.Drawing.Color.Transparent;
            this.labelX13.ForeColor = System.Drawing.Color.Black;
            this.labelX13.Location = new System.Drawing.Point(312, 46);
            this.labelX13.Name = "labelX13";
            this.labelX13.Size = new System.Drawing.Size(17, 22);
            this.labelX13.TabIndex = 19;
            this.labelX13.Text = "on";
            // 
            // EatFoodWhenHealthIsUnderXPercent
            // 
            this.EatFoodWhenHealthIsUnderXPercent.ForeColor = System.Drawing.Color.Black;
            this.EatFoodWhenHealthIsUnderXPercent.Location = new System.Drawing.Point(335, 49);
            this.EatFoodWhenHealthIsUnderXPercent.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.EatFoodWhenHealthIsUnderXPercent.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.EatFoodWhenHealthIsUnderXPercent.Name = "EatFoodWhenHealthIsUnderXPercent";
            this.EatFoodWhenHealthIsUnderXPercent.Size = new System.Drawing.Size(53, 22);
            this.EatFoodWhenHealthIsUnderXPercent.TabIndex = 18;
            this.EatFoodWhenHealthIsUnderXPercent.Value = new decimal(new int[] {
            35,
            0,
            0,
            0});
            // 
            // FoodName
            // 
            this.FoodName.ForeColor = System.Drawing.Color.Black;
            this.FoodName.Location = new System.Drawing.Point(162, 49);
            this.FoodName.Name = "FoodName";
            this.FoodName.Size = new System.Drawing.Size(144, 22);
            this.FoodName.TabIndex = 13;
            // 
            // FoodNameLabel
            // 
            this.FoodNameLabel.BackColor = System.Drawing.Color.Transparent;
            this.FoodNameLabel.ForeColor = System.Drawing.Color.Black;
            this.FoodNameLabel.Location = new System.Drawing.Point(3, 46);
            this.FoodNameLabel.Name = "FoodNameLabel";
            this.FoodNameLabel.Size = new System.Drawing.Size(154, 22);
            this.FoodNameLabel.TabIndex = 12;
            this.FoodNameLabel.Text = "Food";
            // 
            // SecuritySystemPanelName
            // 
            this.SecuritySystemPanelName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.SecuritySystemPanelName.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.SecuritySystemPanelName.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.SecuritySystemPanelName.ContentSize = new System.Drawing.Size(556, 354);
            this.SecuritySystemPanelName.Controls.Add(this.UseHearthstoneLabel);
            this.SecuritySystemPanelName.Controls.Add(this.ActiveStopTNBAfterXMinutes);
            this.SecuritySystemPanelName.Controls.Add(this.ActiveStopTNBAfterXStucks);
            this.SecuritySystemPanelName.Controls.Add(this.ActiveStopTNBIfReceivedAtMostXWhispers);
            this.SecuritySystemPanelName.Controls.Add(this.ActiveStopTNBAfterXLevelup);
            this.SecuritySystemPanelName.Controls.Add(this.UseHearthstone);
            this.SecuritySystemPanelName.Controls.Add(this.SecuritySystemLabel);
            this.SecuritySystemPanelName.Controls.Add(this.PlayASongIfNewWhispReceived);
            this.SecuritySystemPanelName.Controls.Add(this.CloseGameLabel);
            this.SecuritySystemPanelName.Controls.Add(this.labelX33);
            this.SecuritySystemPanelName.Controls.Add(this.RecordWhispsInLogFilesLabel);
            this.SecuritySystemPanelName.Controls.Add(this.labelX32);
            this.SecuritySystemPanelName.Controls.Add(this.labelX31);
            this.SecuritySystemPanelName.Controls.Add(this.RecordWhispsInLogFiles);
            this.SecuritySystemPanelName.Controls.Add(this.StopTNBIfPlayerHaveBeenTeleportedLabel);
            this.SecuritySystemPanelName.Controls.Add(this.StopTNBIfPlayerHaveBeenTeleported);
            this.SecuritySystemPanelName.Controls.Add(this.PauseTNBIfNearByPlayerLabel);
            this.SecuritySystemPanelName.Controls.Add(this.StopTNBIfHonorPointsLimitReachedLabel);
            this.SecuritySystemPanelName.Controls.Add(this.PauseTNBIfNearByPlayer);
            this.SecuritySystemPanelName.Controls.Add(this.StopTNBIfHonorPointsLimitReached);
            this.SecuritySystemPanelName.Controls.Add(this.StopTNBAfterXMinutes);
            this.SecuritySystemPanelName.Controls.Add(this.StopTNBAfterXMinutesLabel);
            this.SecuritySystemPanelName.Controls.Add(this.StopTNBAfterXStucks);
            this.SecuritySystemPanelName.Controls.Add(this.StopTNBAfterXStucksLabel);
            this.SecuritySystemPanelName.Controls.Add(this.StopTNBIfReceivedAtMostXWhispers);
            this.SecuritySystemPanelName.Controls.Add(this.StopTNBIfReceivedAtMostXWhispersLabel);
            this.SecuritySystemPanelName.Controls.Add(this.StopTNBAfterXLevelup);
            this.SecuritySystemPanelName.Controls.Add(this.StopTNBAfterXLevelupLabel);
            this.SecuritySystemPanelName.Controls.Add(this.StopTNBIfBagAreFullLabel);
            this.SecuritySystemPanelName.Controls.Add(this.StopTNBIfBagAreFull);
            this.SecuritySystemPanelName.Fold = true;
            this.SecuritySystemPanelName.FolderImage = ((System.Drawing.Image)(resources.GetObject("SecuritySystemPanelName.FolderImage")));
            this.SecuritySystemPanelName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.SecuritySystemPanelName.HeaderBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.SecuritySystemPanelName.HeaderImage = ((System.Drawing.Image)(resources.GetObject("SecuritySystemPanelName.HeaderImage")));
            this.SecuritySystemPanelName.HeaderSize = new System.Drawing.Size(556, 36);
            this.SecuritySystemPanelName.Location = new System.Drawing.Point(0, 252);
            this.SecuritySystemPanelName.Margin = new System.Windows.Forms.Padding(0);
            this.SecuritySystemPanelName.MaximumSize = new System.Drawing.Size(556, 0);
            this.SecuritySystemPanelName.Name = "SecuritySystemPanelName";
            this.SecuritySystemPanelName.OrderIndex = 8;
            this.SecuritySystemPanelName.Padding = new System.Windows.Forms.Padding(0, 0, 0, 12);
            this.SecuritySystemPanelName.Size = new System.Drawing.Size(556, 36);
            this.SecuritySystemPanelName.TabIndex = 11;
            this.SecuritySystemPanelName.TitleFont = new System.Drawing.Font("Segoe UI", 7.65F, System.Drawing.FontStyle.Bold);
            this.SecuritySystemPanelName.TitleForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.SecuritySystemPanelName.TitleText = "Security System Management (Allow to Pause, Stop Tnb, Close WoW)";
            this.SecuritySystemPanelName.UnfolderImage = ((System.Drawing.Image)(resources.GetObject("SecuritySystemPanelName.UnfolderImage")));
            // 
            // UseHearthstoneLabel
            // 
            this.UseHearthstoneLabel.BackColor = System.Drawing.Color.Transparent;
            this.UseHearthstoneLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UseHearthstoneLabel.ForeColor = System.Drawing.Color.Black;
            this.UseHearthstoneLabel.Location = new System.Drawing.Point(252, 52);
            this.UseHearthstoneLabel.Name = "UseHearthstoneLabel";
            this.UseHearthstoneLabel.Size = new System.Drawing.Size(154, 22);
            this.UseHearthstoneLabel.TabIndex = 48;
            this.UseHearthstoneLabel.Text = "Use Hearthstone";
            // 
            // ActiveStopTNBAfterXMinutes
            // 
            this.ActiveStopTNBAfterXMinutes.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.ActiveStopTNBAfterXMinutes.Location = new System.Drawing.Point(408, 248);
            this.ActiveStopTNBAfterXMinutes.MaximumSize = new System.Drawing.Size(60, 20);
            this.ActiveStopTNBAfterXMinutes.MinimumSize = new System.Drawing.Size(60, 20);
            this.ActiveStopTNBAfterXMinutes.Name = "ActiveStopTNBAfterXMinutes";
            this.ActiveStopTNBAfterXMinutes.OffText = "OFF";
            this.ActiveStopTNBAfterXMinutes.OnText = "ON";
            this.ActiveStopTNBAfterXMinutes.Size = new System.Drawing.Size(60, 20);
            this.ActiveStopTNBAfterXMinutes.TabIndex = 49;
            this.ActiveStopTNBAfterXMinutes.Value = false;
            // 
            // ActiveStopTNBAfterXStucks
            // 
            this.ActiveStopTNBAfterXStucks.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.ActiveStopTNBAfterXStucks.Location = new System.Drawing.Point(408, 219);
            this.ActiveStopTNBAfterXStucks.MaximumSize = new System.Drawing.Size(60, 20);
            this.ActiveStopTNBAfterXStucks.MinimumSize = new System.Drawing.Size(60, 20);
            this.ActiveStopTNBAfterXStucks.Name = "ActiveStopTNBAfterXStucks";
            this.ActiveStopTNBAfterXStucks.OffText = "OFF";
            this.ActiveStopTNBAfterXStucks.OnText = "ON";
            this.ActiveStopTNBAfterXStucks.Size = new System.Drawing.Size(60, 20);
            this.ActiveStopTNBAfterXStucks.TabIndex = 50;
            this.ActiveStopTNBAfterXStucks.Value = false;
            // 
            // ActiveStopTNBIfReceivedAtMostXWhispers
            // 
            this.ActiveStopTNBIfReceivedAtMostXWhispers.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.ActiveStopTNBIfReceivedAtMostXWhispers.Location = new System.Drawing.Point(408, 192);
            this.ActiveStopTNBIfReceivedAtMostXWhispers.MaximumSize = new System.Drawing.Size(60, 20);
            this.ActiveStopTNBIfReceivedAtMostXWhispers.MinimumSize = new System.Drawing.Size(60, 20);
            this.ActiveStopTNBIfReceivedAtMostXWhispers.Name = "ActiveStopTNBIfReceivedAtMostXWhispers";
            this.ActiveStopTNBIfReceivedAtMostXWhispers.OffText = "OFF";
            this.ActiveStopTNBIfReceivedAtMostXWhispers.OnText = "ON";
            this.ActiveStopTNBIfReceivedAtMostXWhispers.Size = new System.Drawing.Size(60, 20);
            this.ActiveStopTNBIfReceivedAtMostXWhispers.TabIndex = 51;
            this.ActiveStopTNBIfReceivedAtMostXWhispers.Value = false;
            // 
            // ActiveStopTNBAfterXLevelup
            // 
            this.ActiveStopTNBAfterXLevelup.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.ActiveStopTNBAfterXLevelup.Location = new System.Drawing.Point(408, 163);
            this.ActiveStopTNBAfterXLevelup.MaximumSize = new System.Drawing.Size(60, 20);
            this.ActiveStopTNBAfterXLevelup.MinimumSize = new System.Drawing.Size(60, 20);
            this.ActiveStopTNBAfterXLevelup.Name = "ActiveStopTNBAfterXLevelup";
            this.ActiveStopTNBAfterXLevelup.OffText = "OFF";
            this.ActiveStopTNBAfterXLevelup.OnText = "ON";
            this.ActiveStopTNBAfterXLevelup.Size = new System.Drawing.Size(60, 20);
            this.ActiveStopTNBAfterXLevelup.TabIndex = 52;
            this.ActiveStopTNBAfterXLevelup.Value = false;
            // 
            // UseHearthstone
            // 
            this.UseHearthstone.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.UseHearthstone.Location = new System.Drawing.Point(411, 53);
            this.UseHearthstone.MaximumSize = new System.Drawing.Size(60, 20);
            this.UseHearthstone.MinimumSize = new System.Drawing.Size(60, 20);
            this.UseHearthstone.Name = "UseHearthstone";
            this.UseHearthstone.OffText = "OFF";
            this.UseHearthstone.OnText = "ON";
            this.UseHearthstone.Size = new System.Drawing.Size(60, 20);
            this.UseHearthstone.TabIndex = 53;
            this.UseHearthstone.Value = false;
            // 
            // SecuritySystemLabel
            // 
            this.SecuritySystemLabel.BackColor = System.Drawing.Color.Transparent;
            this.SecuritySystemLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SecuritySystemLabel.ForeColor = System.Drawing.Color.Black;
            this.SecuritySystemLabel.Location = new System.Drawing.Point(3, 274);
            this.SecuritySystemLabel.Name = "SecuritySystemLabel";
            this.SecuritySystemLabel.Size = new System.Drawing.Size(154, 22);
            this.SecuritySystemLabel.TabIndex = 35;
            this.SecuritySystemLabel.Text = "Security:";
            // 
            // PlayASongIfNewWhispReceived
            // 
            this.PlayASongIfNewWhispReceived.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.PlayASongIfNewWhispReceived.Location = new System.Drawing.Point(0, 0);
            this.PlayASongIfNewWhispReceived.MaximumSize = new System.Drawing.Size(60, 20);
            this.PlayASongIfNewWhispReceived.MinimumSize = new System.Drawing.Size(60, 20);
            this.PlayASongIfNewWhispReceived.Name = "PlayASongIfNewWhispReceived";
            this.PlayASongIfNewWhispReceived.OffText = "OFF";
            this.PlayASongIfNewWhispReceived.OnText = "ON";
            this.PlayASongIfNewWhispReceived.Size = new System.Drawing.Size(60, 20);
            this.PlayASongIfNewWhispReceived.TabIndex = 54;
            this.PlayASongIfNewWhispReceived.Value = false;
            // 
            // CloseGameLabel
            // 
            this.CloseGameLabel.BackColor = System.Drawing.Color.Transparent;
            this.CloseGameLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CloseGameLabel.ForeColor = System.Drawing.Color.Black;
            this.CloseGameLabel.Location = new System.Drawing.Point(3, 52);
            this.CloseGameLabel.Name = "CloseGameLabel";
            this.CloseGameLabel.Size = new System.Drawing.Size(154, 22);
            this.CloseGameLabel.TabIndex = 34;
            this.CloseGameLabel.Text = "Close game:";
            // 
            // labelX33
            // 
            this.labelX33.BackColor = System.Drawing.Color.Transparent;
            this.labelX33.ForeColor = System.Drawing.Color.Black;
            this.labelX33.Location = new System.Drawing.Point(318, 246);
            this.labelX33.Name = "labelX33";
            this.labelX33.Size = new System.Drawing.Size(154, 22);
            this.labelX33.TabIndex = 33;
            this.labelX33.Text = "Min";
            // 
            // RecordWhispsInLogFilesLabel
            // 
            this.RecordWhispsInLogFilesLabel.BackColor = System.Drawing.Color.Transparent;
            this.RecordWhispsInLogFilesLabel.ForeColor = System.Drawing.Color.Black;
            this.RecordWhispsInLogFilesLabel.Location = new System.Drawing.Point(3, 330);
            this.RecordWhispsInLogFilesLabel.Name = "RecordWhispsInLogFilesLabel";
            this.RecordWhispsInLogFilesLabel.Size = new System.Drawing.Size(154, 22);
            this.RecordWhispsInLogFilesLabel.TabIndex = 39;
            this.RecordWhispsInLogFilesLabel.Text = "Record whisper in Log file";
            // 
            // labelX32
            // 
            this.labelX32.BackColor = System.Drawing.Color.Transparent;
            this.labelX32.ForeColor = System.Drawing.Color.Black;
            this.labelX32.Location = new System.Drawing.Point(318, 219);
            this.labelX32.Name = "labelX32";
            this.labelX32.Size = new System.Drawing.Size(154, 22);
            this.labelX32.TabIndex = 32;
            this.labelX32.Text = "Blockages";
            // 
            // labelX31
            // 
            this.labelX31.BackColor = System.Drawing.Color.Transparent;
            this.labelX31.ForeColor = System.Drawing.Color.Black;
            this.labelX31.Location = new System.Drawing.Point(318, 163);
            this.labelX31.Name = "labelX31";
            this.labelX31.Size = new System.Drawing.Size(154, 22);
            this.labelX31.TabIndex = 31;
            this.labelX31.Text = "Level";
            // 
            // RecordWhispsInLogFiles
            // 
            this.RecordWhispsInLogFiles.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.RecordWhispsInLogFiles.Location = new System.Drawing.Point(162, 330);
            this.RecordWhispsInLogFiles.MaximumSize = new System.Drawing.Size(60, 20);
            this.RecordWhispsInLogFiles.MinimumSize = new System.Drawing.Size(60, 20);
            this.RecordWhispsInLogFiles.Name = "RecordWhispsInLogFiles";
            this.RecordWhispsInLogFiles.OffText = "OFF";
            this.RecordWhispsInLogFiles.OnText = "ON";
            this.RecordWhispsInLogFiles.Size = new System.Drawing.Size(60, 20);
            this.RecordWhispsInLogFiles.TabIndex = 55;
            this.RecordWhispsInLogFiles.Value = false;
            // 
            // StopTNBIfPlayerHaveBeenTeleportedLabel
            // 
            this.StopTNBIfPlayerHaveBeenTeleportedLabel.BackColor = System.Drawing.Color.Transparent;
            this.StopTNBIfPlayerHaveBeenTeleportedLabel.ForeColor = System.Drawing.Color.Black;
            this.StopTNBIfPlayerHaveBeenTeleportedLabel.Location = new System.Drawing.Point(3, 135);
            this.StopTNBIfPlayerHaveBeenTeleportedLabel.Name = "StopTNBIfPlayerHaveBeenTeleportedLabel";
            this.StopTNBIfPlayerHaveBeenTeleportedLabel.Size = new System.Drawing.Size(154, 22);
            this.StopTNBIfPlayerHaveBeenTeleportedLabel.TabIndex = 30;
            this.StopTNBIfPlayerHaveBeenTeleportedLabel.Text = "If Player Teleported";
            // 
            // StopTNBIfPlayerHaveBeenTeleported
            // 
            this.StopTNBIfPlayerHaveBeenTeleported.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.StopTNBIfPlayerHaveBeenTeleported.Location = new System.Drawing.Point(162, 135);
            this.StopTNBIfPlayerHaveBeenTeleported.MaximumSize = new System.Drawing.Size(60, 20);
            this.StopTNBIfPlayerHaveBeenTeleported.MinimumSize = new System.Drawing.Size(60, 20);
            this.StopTNBIfPlayerHaveBeenTeleported.Name = "StopTNBIfPlayerHaveBeenTeleported";
            this.StopTNBIfPlayerHaveBeenTeleported.OffText = "OFF";
            this.StopTNBIfPlayerHaveBeenTeleported.OnText = "ON";
            this.StopTNBIfPlayerHaveBeenTeleported.Size = new System.Drawing.Size(60, 20);
            this.StopTNBIfPlayerHaveBeenTeleported.TabIndex = 56;
            this.StopTNBIfPlayerHaveBeenTeleported.Value = false;
            // 
            // PauseTNBIfNearByPlayerLabel
            // 
            this.PauseTNBIfNearByPlayerLabel.BackColor = System.Drawing.Color.Transparent;
            this.PauseTNBIfNearByPlayerLabel.ForeColor = System.Drawing.Color.Black;
            this.PauseTNBIfNearByPlayerLabel.Location = new System.Drawing.Point(3, 302);
            this.PauseTNBIfNearByPlayerLabel.Name = "PauseTNBIfNearByPlayerLabel";
            this.PauseTNBIfNearByPlayerLabel.Size = new System.Drawing.Size(154, 22);
            this.PauseTNBIfNearByPlayerLabel.TabIndex = 37;
            this.PauseTNBIfNearByPlayerLabel.Text = "Pause Bot if Nearby Player";
            // 
            // StopTNBIfHonorPointsLimitReachedLabel
            // 
            this.StopTNBIfHonorPointsLimitReachedLabel.BackColor = System.Drawing.Color.Transparent;
            this.StopTNBIfHonorPointsLimitReachedLabel.ForeColor = System.Drawing.Color.Black;
            this.StopTNBIfHonorPointsLimitReachedLabel.Location = new System.Drawing.Point(3, 107);
            this.StopTNBIfHonorPointsLimitReachedLabel.Name = "StopTNBIfHonorPointsLimitReachedLabel";
            this.StopTNBIfHonorPointsLimitReachedLabel.Size = new System.Drawing.Size(154, 22);
            this.StopTNBIfHonorPointsLimitReachedLabel.TabIndex = 28;
            this.StopTNBIfHonorPointsLimitReachedLabel.Text = "If reached 4000 Honor Points";
            // 
            // PauseTNBIfNearByPlayer
            // 
            this.PauseTNBIfNearByPlayer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.PauseTNBIfNearByPlayer.Location = new System.Drawing.Point(162, 302);
            this.PauseTNBIfNearByPlayer.MaximumSize = new System.Drawing.Size(60, 20);
            this.PauseTNBIfNearByPlayer.MinimumSize = new System.Drawing.Size(60, 20);
            this.PauseTNBIfNearByPlayer.Name = "PauseTNBIfNearByPlayer";
            this.PauseTNBIfNearByPlayer.OffText = "OFF";
            this.PauseTNBIfNearByPlayer.OnText = "ON";
            this.PauseTNBIfNearByPlayer.Size = new System.Drawing.Size(60, 20);
            this.PauseTNBIfNearByPlayer.TabIndex = 57;
            this.PauseTNBIfNearByPlayer.Value = false;
            // 
            // StopTNBIfHonorPointsLimitReached
            // 
            this.StopTNBIfHonorPointsLimitReached.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.StopTNBIfHonorPointsLimitReached.Location = new System.Drawing.Point(162, 107);
            this.StopTNBIfHonorPointsLimitReached.MaximumSize = new System.Drawing.Size(60, 20);
            this.StopTNBIfHonorPointsLimitReached.MinimumSize = new System.Drawing.Size(60, 20);
            this.StopTNBIfHonorPointsLimitReached.Name = "StopTNBIfHonorPointsLimitReached";
            this.StopTNBIfHonorPointsLimitReached.OffText = "OFF";
            this.StopTNBIfHonorPointsLimitReached.OnText = "ON";
            this.StopTNBIfHonorPointsLimitReached.Size = new System.Drawing.Size(60, 20);
            this.StopTNBIfHonorPointsLimitReached.TabIndex = 58;
            this.StopTNBIfHonorPointsLimitReached.Value = false;
            // 
            // StopTNBAfterXMinutes
            // 
            this.StopTNBAfterXMinutes.ForeColor = System.Drawing.Color.Black;
            this.StopTNBAfterXMinutes.Location = new System.Drawing.Point(162, 247);
            this.StopTNBAfterXMinutes.Maximum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.StopTNBAfterXMinutes.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.StopTNBAfterXMinutes.Name = "StopTNBAfterXMinutes";
            this.StopTNBAfterXMinutes.Size = new System.Drawing.Size(77, 22);
            this.StopTNBAfterXMinutes.TabIndex = 24;
            this.StopTNBAfterXMinutes.Value = new decimal(new int[] {
            1440,
            0,
            0,
            0});
            // 
            // StopTNBAfterXMinutesLabel
            // 
            this.StopTNBAfterXMinutesLabel.BackColor = System.Drawing.Color.Transparent;
            this.StopTNBAfterXMinutesLabel.ForeColor = System.Drawing.Color.Black;
            this.StopTNBAfterXMinutesLabel.Location = new System.Drawing.Point(3, 247);
            this.StopTNBAfterXMinutesLabel.Name = "StopTNBAfterXMinutesLabel";
            this.StopTNBAfterXMinutesLabel.Size = new System.Drawing.Size(154, 22);
            this.StopTNBAfterXMinutesLabel.TabIndex = 23;
            this.StopTNBAfterXMinutesLabel.Text = "After";
            // 
            // StopTNBAfterXStucks
            // 
            this.StopTNBAfterXStucks.ForeColor = System.Drawing.Color.Black;
            this.StopTNBAfterXStucks.Location = new System.Drawing.Point(162, 219);
            this.StopTNBAfterXStucks.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.StopTNBAfterXStucks.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.StopTNBAfterXStucks.Name = "StopTNBAfterXStucks";
            this.StopTNBAfterXStucks.Size = new System.Drawing.Size(77, 22);
            this.StopTNBAfterXStucks.TabIndex = 22;
            this.StopTNBAfterXStucks.Value = new decimal(new int[] {
            80,
            0,
            0,
            0});
            // 
            // StopTNBAfterXStucksLabel
            // 
            this.StopTNBAfterXStucksLabel.BackColor = System.Drawing.Color.Transparent;
            this.StopTNBAfterXStucksLabel.ForeColor = System.Drawing.Color.Black;
            this.StopTNBAfterXStucksLabel.Location = new System.Drawing.Point(3, 219);
            this.StopTNBAfterXStucksLabel.Name = "StopTNBAfterXStucksLabel";
            this.StopTNBAfterXStucksLabel.Size = new System.Drawing.Size(154, 22);
            this.StopTNBAfterXStucksLabel.TabIndex = 21;
            this.StopTNBAfterXStucksLabel.Text = "After";
            // 
            // StopTNBIfReceivedAtMostXWhispers
            // 
            this.StopTNBIfReceivedAtMostXWhispers.ForeColor = System.Drawing.Color.Black;
            this.StopTNBIfReceivedAtMostXWhispers.Location = new System.Drawing.Point(162, 191);
            this.StopTNBIfReceivedAtMostXWhispers.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.StopTNBIfReceivedAtMostXWhispers.Name = "StopTNBIfReceivedAtMostXWhispers";
            this.StopTNBIfReceivedAtMostXWhispers.Size = new System.Drawing.Size(77, 22);
            this.StopTNBIfReceivedAtMostXWhispers.TabIndex = 20;
            this.StopTNBIfReceivedAtMostXWhispers.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // StopTNBIfReceivedAtMostXWhispersLabel
            // 
            this.StopTNBIfReceivedAtMostXWhispersLabel.BackColor = System.Drawing.Color.Transparent;
            this.StopTNBIfReceivedAtMostXWhispersLabel.ForeColor = System.Drawing.Color.Black;
            this.StopTNBIfReceivedAtMostXWhispersLabel.Location = new System.Drawing.Point(3, 191);
            this.StopTNBIfReceivedAtMostXWhispersLabel.Name = "StopTNBIfReceivedAtMostXWhispersLabel";
            this.StopTNBIfReceivedAtMostXWhispersLabel.Size = new System.Drawing.Size(154, 22);
            this.StopTNBIfReceivedAtMostXWhispersLabel.TabIndex = 19;
            this.StopTNBIfReceivedAtMostXWhispersLabel.Text = "If Whisper bigger or equal to";
            // 
            // StopTNBAfterXLevelup
            // 
            this.StopTNBAfterXLevelup.ForeColor = System.Drawing.Color.Black;
            this.StopTNBAfterXLevelup.Location = new System.Drawing.Point(162, 163);
            this.StopTNBAfterXLevelup.Maximum = new decimal(new int[] {
            110,
            0,
            0,
            0});
            this.StopTNBAfterXLevelup.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.StopTNBAfterXLevelup.Name = "StopTNBAfterXLevelup";
            this.StopTNBAfterXLevelup.Size = new System.Drawing.Size(77, 22);
            this.StopTNBAfterXLevelup.TabIndex = 18;
            this.StopTNBAfterXLevelup.Value = new decimal(new int[] {
            90,
            0,
            0,
            0});
            // 
            // StopTNBAfterXLevelupLabel
            // 
            this.StopTNBAfterXLevelupLabel.BackColor = System.Drawing.Color.Transparent;
            this.StopTNBAfterXLevelupLabel.ForeColor = System.Drawing.Color.Black;
            this.StopTNBAfterXLevelupLabel.Location = new System.Drawing.Point(3, 163);
            this.StopTNBAfterXLevelupLabel.Name = "StopTNBAfterXLevelupLabel";
            this.StopTNBAfterXLevelupLabel.Size = new System.Drawing.Size(154, 22);
            this.StopTNBAfterXLevelupLabel.TabIndex = 16;
            this.StopTNBAfterXLevelupLabel.Text = "After";
            // 
            // StopTNBIfBagAreFullLabel
            // 
            this.StopTNBIfBagAreFullLabel.BackColor = System.Drawing.Color.Transparent;
            this.StopTNBIfBagAreFullLabel.ForeColor = System.Drawing.Color.Black;
            this.StopTNBIfBagAreFullLabel.Location = new System.Drawing.Point(3, 80);
            this.StopTNBIfBagAreFullLabel.Name = "StopTNBIfBagAreFullLabel";
            this.StopTNBIfBagAreFullLabel.Size = new System.Drawing.Size(154, 22);
            this.StopTNBIfBagAreFullLabel.TabIndex = 11;
            this.StopTNBIfBagAreFullLabel.Text = "If full Bag";
            // 
            // StopTNBIfBagAreFull
            // 
            this.StopTNBIfBagAreFull.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.StopTNBIfBagAreFull.Location = new System.Drawing.Point(162, 80);
            this.StopTNBIfBagAreFull.MaximumSize = new System.Drawing.Size(60, 20);
            this.StopTNBIfBagAreFull.MinimumSize = new System.Drawing.Size(60, 20);
            this.StopTNBIfBagAreFull.Name = "StopTNBIfBagAreFull";
            this.StopTNBIfBagAreFull.OffText = "OFF";
            this.StopTNBIfBagAreFull.OnText = "ON";
            this.StopTNBIfBagAreFull.Size = new System.Drawing.Size(60, 20);
            this.StopTNBIfBagAreFull.TabIndex = 59;
            this.StopTNBIfBagAreFull.Value = false;
            // 
            // ReloggerManagementPanelName
            // 
            this.ReloggerManagementPanelName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.ReloggerManagementPanelName.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.ReloggerManagementPanelName.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.ReloggerManagementPanelName.ContentSize = new System.Drawing.Size(556, 144);
            this.ReloggerManagementPanelName.Controls.Add(this.BattleNetSubAccount);
            this.ReloggerManagementPanelName.Controls.Add(this.BattleNetSubAccountLabel);
            this.ReloggerManagementPanelName.Controls.Add(this.ActivateReloggerFeatureLabel);
            this.ReloggerManagementPanelName.Controls.Add(this.ActivateReloggerFeature);
            this.ReloggerManagementPanelName.Controls.Add(this.PasswordOfTheBattleNetAccount);
            this.ReloggerManagementPanelName.Controls.Add(this.PasswordOfTheBattleNetAccountLabel);
            this.ReloggerManagementPanelName.Controls.Add(this.EmailOfTheBattleNetAccount);
            this.ReloggerManagementPanelName.Controls.Add(this.EmailOfTheBattleNetAccountLabel);
            this.ReloggerManagementPanelName.Fold = true;
            this.ReloggerManagementPanelName.FolderImage = ((System.Drawing.Image)(resources.GetObject("ReloggerManagementPanelName.FolderImage")));
            this.ReloggerManagementPanelName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.ReloggerManagementPanelName.HeaderBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.ReloggerManagementPanelName.HeaderImage = ((System.Drawing.Image)(resources.GetObject("ReloggerManagementPanelName.HeaderImage")));
            this.ReloggerManagementPanelName.HeaderSize = new System.Drawing.Size(556, 36);
            this.ReloggerManagementPanelName.Location = new System.Drawing.Point(0, 288);
            this.ReloggerManagementPanelName.Margin = new System.Windows.Forms.Padding(0);
            this.ReloggerManagementPanelName.MaximumSize = new System.Drawing.Size(556, 0);
            this.ReloggerManagementPanelName.Name = "ReloggerManagementPanelName";
            this.ReloggerManagementPanelName.OrderIndex = 9;
            this.ReloggerManagementPanelName.Padding = new System.Windows.Forms.Padding(0, 0, 0, 12);
            this.ReloggerManagementPanelName.Size = new System.Drawing.Size(556, 36);
            this.ReloggerManagementPanelName.TabIndex = 7;
            this.ReloggerManagementPanelName.TitleFont = new System.Drawing.Font("Segoe UI", 7.65F, System.Drawing.FontStyle.Bold);
            this.ReloggerManagementPanelName.TitleForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.ReloggerManagementPanelName.TitleText = "AutoRelogger - (Allow you to automaticly relog your char after a disconnection)";
            this.ReloggerManagementPanelName.UnfolderImage = ((System.Drawing.Image)(resources.GetObject("ReloggerManagementPanelName.UnfolderImage")));
            // 
            // BattleNetSubAccount
            // 
            this.BattleNetSubAccount.ForeColor = System.Drawing.Color.Black;
            this.BattleNetSubAccount.Location = new System.Drawing.Point(162, 143);
            this.BattleNetSubAccount.Name = "BattleNetSubAccount";
            this.BattleNetSubAccount.Size = new System.Drawing.Size(175, 22);
            this.BattleNetSubAccount.TabIndex = 24;
            // 
            // BattleNetSubAccountLabel
            // 
            this.BattleNetSubAccountLabel.BackColor = System.Drawing.Color.Transparent;
            this.BattleNetSubAccountLabel.ForeColor = System.Drawing.Color.Black;
            this.BattleNetSubAccountLabel.Location = new System.Drawing.Point(3, 140);
            this.BattleNetSubAccountLabel.Name = "BattleNetSubAccountLabel";
            this.BattleNetSubAccountLabel.Size = new System.Drawing.Size(154, 22);
            this.BattleNetSubAccountLabel.TabIndex = 23;
            this.BattleNetSubAccountLabel.Text = "BattleNet Account";
            // 
            // ActivateReloggerFeatureLabel
            // 
            this.ActivateReloggerFeatureLabel.BackColor = System.Drawing.Color.Transparent;
            this.ActivateReloggerFeatureLabel.ForeColor = System.Drawing.Color.Black;
            this.ActivateReloggerFeatureLabel.Location = new System.Drawing.Point(3, 52);
            this.ActivateReloggerFeatureLabel.Name = "ActivateReloggerFeatureLabel";
            this.ActivateReloggerFeatureLabel.Size = new System.Drawing.Size(154, 22);
            this.ActivateReloggerFeatureLabel.TabIndex = 13;
            this.ActivateReloggerFeatureLabel.Text = "Relogger";
            // 
            // ActivateReloggerFeature
            // 
            this.ActivateReloggerFeature.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.ActivateReloggerFeature.Location = new System.Drawing.Point(162, 52);
            this.ActivateReloggerFeature.MaximumSize = new System.Drawing.Size(60, 20);
            this.ActivateReloggerFeature.MinimumSize = new System.Drawing.Size(60, 20);
            this.ActivateReloggerFeature.Name = "ActivateReloggerFeature";
            this.ActivateReloggerFeature.OffText = "OFF";
            this.ActivateReloggerFeature.OnText = "ON";
            this.ActivateReloggerFeature.Size = new System.Drawing.Size(60, 20);
            this.ActivateReloggerFeature.TabIndex = 25;
            this.ActivateReloggerFeature.Value = false;
            // 
            // PasswordOfTheBattleNetAccount
            // 
            this.PasswordOfTheBattleNetAccount.ForeColor = System.Drawing.Color.Black;
            this.PasswordOfTheBattleNetAccount.Location = new System.Drawing.Point(162, 110);
            this.PasswordOfTheBattleNetAccount.Name = "PasswordOfTheBattleNetAccount";
            this.PasswordOfTheBattleNetAccount.Size = new System.Drawing.Size(175, 22);
            this.PasswordOfTheBattleNetAccount.TabIndex = 22;
            // 
            // PasswordOfTheBattleNetAccountLabel
            // 
            this.PasswordOfTheBattleNetAccountLabel.BackColor = System.Drawing.Color.Transparent;
            this.PasswordOfTheBattleNetAccountLabel.ForeColor = System.Drawing.Color.Black;
            this.PasswordOfTheBattleNetAccountLabel.Location = new System.Drawing.Point(3, 108);
            this.PasswordOfTheBattleNetAccountLabel.Name = "PasswordOfTheBattleNetAccountLabel";
            this.PasswordOfTheBattleNetAccountLabel.Size = new System.Drawing.Size(154, 22);
            this.PasswordOfTheBattleNetAccountLabel.TabIndex = 21;
            this.PasswordOfTheBattleNetAccountLabel.Text = "Account Password";
            // 
            // EmailOfTheBattleNetAccount
            // 
            this.EmailOfTheBattleNetAccount.ForeColor = System.Drawing.Color.Black;
            this.EmailOfTheBattleNetAccount.Location = new System.Drawing.Point(162, 83);
            this.EmailOfTheBattleNetAccount.Name = "EmailOfTheBattleNetAccount";
            this.EmailOfTheBattleNetAccount.Size = new System.Drawing.Size(175, 22);
            this.EmailOfTheBattleNetAccount.TabIndex = 13;
            // 
            // EmailOfTheBattleNetAccountLabel
            // 
            this.EmailOfTheBattleNetAccountLabel.BackColor = System.Drawing.Color.Transparent;
            this.EmailOfTheBattleNetAccountLabel.ForeColor = System.Drawing.Color.Black;
            this.EmailOfTheBattleNetAccountLabel.Location = new System.Drawing.Point(3, 80);
            this.EmailOfTheBattleNetAccountLabel.Name = "EmailOfTheBattleNetAccountLabel";
            this.EmailOfTheBattleNetAccountLabel.Size = new System.Drawing.Size(154, 22);
            this.EmailOfTheBattleNetAccountLabel.TabIndex = 12;
            this.EmailOfTheBattleNetAccountLabel.Text = "Account Email";
            // 
            // MimesisBroadcasterSettingsPanel
            // 
            this.MimesisBroadcasterSettingsPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.MimesisBroadcasterSettingsPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.MimesisBroadcasterSettingsPanel.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.MimesisBroadcasterSettingsPanel.ContentSize = new System.Drawing.Size(556, 158);
            this.MimesisBroadcasterSettingsPanel.Controls.Add(this.BroadcastingPort);
            this.MimesisBroadcasterSettingsPanel.Controls.Add(this.BroadcastingIPWan);
            this.MimesisBroadcasterSettingsPanel.Controls.Add(this.BroadcastingIPLan);
            this.MimesisBroadcasterSettingsPanel.Controls.Add(this.BroadcastingIPLocal);
            this.MimesisBroadcasterSettingsPanel.Controls.Add(this.BroadcastingPortDefaultLabel);
            this.MimesisBroadcasterSettingsPanel.Controls.Add(this.BroadcastingIPWanLabel);
            this.MimesisBroadcasterSettingsPanel.Controls.Add(this.BroadcastingIPLanLabel);
            this.MimesisBroadcasterSettingsPanel.Controls.Add(this.ActivateBroadcastingMimesisLabel);
            this.MimesisBroadcasterSettingsPanel.Controls.Add(this.ActivateBroadcastingMimesis);
            this.MimesisBroadcasterSettingsPanel.Controls.Add(this.BroadcastingIPLocalLabel);
            this.MimesisBroadcasterSettingsPanel.Controls.Add(this.BroadcastingPortLabel);
            this.MimesisBroadcasterSettingsPanel.Fold = true;
            this.MimesisBroadcasterSettingsPanel.FolderImage = ((System.Drawing.Image)(resources.GetObject("MimesisBroadcasterSettingsPanel.FolderImage")));
            this.MimesisBroadcasterSettingsPanel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.MimesisBroadcasterSettingsPanel.HeaderBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.MimesisBroadcasterSettingsPanel.HeaderImage = ((System.Drawing.Image)(resources.GetObject("MimesisBroadcasterSettingsPanel.HeaderImage")));
            this.MimesisBroadcasterSettingsPanel.HeaderSize = new System.Drawing.Size(556, 36);
            this.MimesisBroadcasterSettingsPanel.Location = new System.Drawing.Point(0, 324);
            this.MimesisBroadcasterSettingsPanel.Margin = new System.Windows.Forms.Padding(0);
            this.MimesisBroadcasterSettingsPanel.MaximumSize = new System.Drawing.Size(556, 0);
            this.MimesisBroadcasterSettingsPanel.Name = "MimesisBroadcasterSettingsPanel";
            this.MimesisBroadcasterSettingsPanel.OrderIndex = 10;
            this.MimesisBroadcasterSettingsPanel.Padding = new System.Windows.Forms.Padding(0, 0, 0, 12);
            this.MimesisBroadcasterSettingsPanel.Size = new System.Drawing.Size(556, 36);
            this.MimesisBroadcasterSettingsPanel.TabIndex = 13;
            this.MimesisBroadcasterSettingsPanel.TitleFont = new System.Drawing.Font("Segoe UI", 7.65F, System.Drawing.FontStyle.Bold);
            this.MimesisBroadcasterSettingsPanel.TitleForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.MimesisBroadcasterSettingsPanel.TitleText = "Mimesis Broadcaster - (Others sessions with a started Mimesis can teamplay with t" +
    "his session)";
            this.MimesisBroadcasterSettingsPanel.UnfolderImage = ((System.Drawing.Image)(resources.GetObject("MimesisBroadcasterSettingsPanel.UnfolderImage")));
            // 
            // BroadcastingPort
            // 
            this.BroadcastingPort.Location = new System.Drawing.Point(162, 78);
            this.BroadcastingPort.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.BroadcastingPort.Minimum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.BroadcastingPort.Name = "BroadcastingPort";
            this.BroadcastingPort.Size = new System.Drawing.Size(63, 22);
            this.BroadcastingPort.TabIndex = 32;
            this.BroadcastingPort.Value = new decimal(new int[] {
            6543,
            0,
            0,
            0});
            // 
            // BroadcastingIPWan
            // 
            this.BroadcastingIPWan.BackColor = System.Drawing.Color.Transparent;
            this.BroadcastingIPWan.ForeColor = System.Drawing.Color.Black;
            this.BroadcastingIPWan.Location = new System.Drawing.Point(160, 160);
            this.BroadcastingIPWan.Name = "BroadcastingIPWan";
            this.BroadcastingIPWan.Size = new System.Drawing.Size(154, 22);
            this.BroadcastingIPWan.TabIndex = 31;
            this.BroadcastingIPWan.Text = "0.0.0.0";
            // 
            // BroadcastingIPLan
            // 
            this.BroadcastingIPLan.BackColor = System.Drawing.Color.Transparent;
            this.BroadcastingIPLan.ForeColor = System.Drawing.Color.Black;
            this.BroadcastingIPLan.Location = new System.Drawing.Point(160, 132);
            this.BroadcastingIPLan.Name = "BroadcastingIPLan";
            this.BroadcastingIPLan.Size = new System.Drawing.Size(154, 22);
            this.BroadcastingIPLan.TabIndex = 30;
            this.BroadcastingIPLan.Text = "192.168.1.2";
            // 
            // BroadcastingIPLocal
            // 
            this.BroadcastingIPLocal.BackColor = System.Drawing.Color.Transparent;
            this.BroadcastingIPLocal.ForeColor = System.Drawing.Color.Black;
            this.BroadcastingIPLocal.Location = new System.Drawing.Point(160, 104);
            this.BroadcastingIPLocal.Name = "BroadcastingIPLocal";
            this.BroadcastingIPLocal.Size = new System.Drawing.Size(154, 22);
            this.BroadcastingIPLocal.TabIndex = 29;
            this.BroadcastingIPLocal.Text = "127.0.0.1";
            // 
            // BroadcastingPortDefaultLabel
            // 
            this.BroadcastingPortDefaultLabel.BackColor = System.Drawing.Color.Transparent;
            this.BroadcastingPortDefaultLabel.ForeColor = System.Drawing.Color.Black;
            this.BroadcastingPortDefaultLabel.Location = new System.Drawing.Point(231, 78);
            this.BroadcastingPortDefaultLabel.Name = "BroadcastingPortDefaultLabel";
            this.BroadcastingPortDefaultLabel.Size = new System.Drawing.Size(106, 22);
            this.BroadcastingPortDefaultLabel.TabIndex = 28;
            this.BroadcastingPortDefaultLabel.Text = "Default: 6543";
            // 
            // BroadcastingIPWanLabel
            // 
            this.BroadcastingIPWanLabel.BackColor = System.Drawing.Color.Transparent;
            this.BroadcastingIPWanLabel.ForeColor = System.Drawing.Color.Black;
            this.BroadcastingIPWanLabel.Location = new System.Drawing.Point(2, 160);
            this.BroadcastingIPWanLabel.Name = "BroadcastingIPWanLabel";
            this.BroadcastingIPWanLabel.Size = new System.Drawing.Size(154, 22);
            this.BroadcastingIPWanLabel.TabIndex = 26;
            this.BroadcastingIPWanLabel.Text = "IP Wan (internet)";
            // 
            // BroadcastingIPLanLabel
            // 
            this.BroadcastingIPLanLabel.BackColor = System.Drawing.Color.Transparent;
            this.BroadcastingIPLanLabel.ForeColor = System.Drawing.Color.Black;
            this.BroadcastingIPLanLabel.Location = new System.Drawing.Point(2, 132);
            this.BroadcastingIPLanLabel.Name = "BroadcastingIPLanLabel";
            this.BroadcastingIPLanLabel.Size = new System.Drawing.Size(154, 22);
            this.BroadcastingIPLanLabel.TabIndex = 24;
            this.BroadcastingIPLanLabel.Text = "IP Lan (network)";
            // 
            // ActivateBroadcastingMimesisLabel
            // 
            this.ActivateBroadcastingMimesisLabel.BackColor = System.Drawing.Color.Transparent;
            this.ActivateBroadcastingMimesisLabel.ForeColor = System.Drawing.Color.Black;
            this.ActivateBroadcastingMimesisLabel.Location = new System.Drawing.Point(2, 50);
            this.ActivateBroadcastingMimesisLabel.Name = "ActivateBroadcastingMimesisLabel";
            this.ActivateBroadcastingMimesisLabel.Size = new System.Drawing.Size(154, 22);
            this.ActivateBroadcastingMimesisLabel.TabIndex = 13;
            this.ActivateBroadcastingMimesisLabel.Text = "Activate Broadcasting";
            // 
            // ActivateBroadcastingMimesis
            // 
            this.ActivateBroadcastingMimesis.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(229)))), ((int)(((byte)(230)))));
            this.ActivateBroadcastingMimesis.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.ActivateBroadcastingMimesis.ForeColor = System.Drawing.Color.Black;
            this.ActivateBroadcastingMimesis.Location = new System.Drawing.Point(162, 51);
            this.ActivateBroadcastingMimesis.MaximumSize = new System.Drawing.Size(60, 20);
            this.ActivateBroadcastingMimesis.MinimumSize = new System.Drawing.Size(60, 20);
            this.ActivateBroadcastingMimesis.Name = "ActivateBroadcastingMimesis";
            this.ActivateBroadcastingMimesis.OffText = "OFF";
            this.ActivateBroadcastingMimesis.OnText = "ON";
            this.ActivateBroadcastingMimesis.Size = new System.Drawing.Size(60, 20);
            this.ActivateBroadcastingMimesis.TabIndex = 12;
            this.ActivateBroadcastingMimesis.Value = false;
            // 
            // BroadcastingIPLocalLabel
            // 
            this.BroadcastingIPLocalLabel.BackColor = System.Drawing.Color.Transparent;
            this.BroadcastingIPLocalLabel.ForeColor = System.Drawing.Color.Black;
            this.BroadcastingIPLocalLabel.Location = new System.Drawing.Point(2, 104);
            this.BroadcastingIPLocalLabel.Name = "BroadcastingIPLocalLabel";
            this.BroadcastingIPLocalLabel.Size = new System.Drawing.Size(154, 22);
            this.BroadcastingIPLocalLabel.TabIndex = 21;
            this.BroadcastingIPLocalLabel.Text = "IP Local (computer)";
            // 
            // BroadcastingPortLabel
            // 
            this.BroadcastingPortLabel.BackColor = System.Drawing.Color.Transparent;
            this.BroadcastingPortLabel.ForeColor = System.Drawing.Color.Black;
            this.BroadcastingPortLabel.Location = new System.Drawing.Point(2, 78);
            this.BroadcastingPortLabel.Name = "BroadcastingPortLabel";
            this.BroadcastingPortLabel.Size = new System.Drawing.Size(154, 22);
            this.BroadcastingPortLabel.TabIndex = 12;
            this.BroadcastingPortLabel.Text = "Broadcasting Port";
            // 
            // AdvancedSettingsPanelName
            // 
            this.AdvancedSettingsPanelName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.AdvancedSettingsPanelName.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.AdvancedSettingsPanelName.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.AdvancedSettingsPanelName.ContentSize = new System.Drawing.Size(556, 103);
            this.AdvancedSettingsPanelName.Controls.Add(this.UseFrameLockLabel);
            this.AdvancedSettingsPanelName.Controls.Add(this.UseFrameLock);
            this.AdvancedSettingsPanelName.Controls.Add(this.HideSDKFilesLabel);
            this.AdvancedSettingsPanelName.Controls.Add(this.HideSDKFiles);
            this.AdvancedSettingsPanelName.Controls.Add(this.AutoCloseChatFrameLabel);
            this.AdvancedSettingsPanelName.Controls.Add(this.AutoCloseChatFrame);
            this.AdvancedSettingsPanelName.Controls.Add(this.AlwaysOnTopFeatureLabel);
            this.AdvancedSettingsPanelName.Controls.Add(this.ActivateAlwaysOnTopFeature);
            this.AdvancedSettingsPanelName.Controls.Add(this.AllowTNBToSetYourMaxFpsLabel);
            this.AdvancedSettingsPanelName.Controls.Add(this.AllowTNBToSetYourMaxFps);
            this.AdvancedSettingsPanelName.Controls.Add(this.MaxDistanceToGoToMailboxesOrNPCs);
            this.AdvancedSettingsPanelName.Controls.Add(this.MaxDistanceToGoToMailboxesOrNPCsLabel);
            this.AdvancedSettingsPanelName.Controls.Add(this.ActivatePathFindingFeatureLabel);
            this.AdvancedSettingsPanelName.Controls.Add(this.ActivatePathFindingFeature);
            this.AdvancedSettingsPanelName.Fold = true;
            this.AdvancedSettingsPanelName.FolderImage = ((System.Drawing.Image)(resources.GetObject("AdvancedSettingsPanelName.FolderImage")));
            this.AdvancedSettingsPanelName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.AdvancedSettingsPanelName.HeaderBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.AdvancedSettingsPanelName.HeaderImage = ((System.Drawing.Image)(resources.GetObject("AdvancedSettingsPanelName.HeaderImage")));
            this.AdvancedSettingsPanelName.HeaderSize = new System.Drawing.Size(556, 36);
            this.AdvancedSettingsPanelName.Location = new System.Drawing.Point(0, 360);
            this.AdvancedSettingsPanelName.Margin = new System.Windows.Forms.Padding(0);
            this.AdvancedSettingsPanelName.MaximumSize = new System.Drawing.Size(556, 0);
            this.AdvancedSettingsPanelName.MinimumSize = new System.Drawing.Size(556, 36);
            this.AdvancedSettingsPanelName.Name = "AdvancedSettingsPanelName";
            this.AdvancedSettingsPanelName.OrderIndex = 11;
            this.AdvancedSettingsPanelName.Padding = new System.Windows.Forms.Padding(0, 0, 0, 12);
            this.AdvancedSettingsPanelName.Size = new System.Drawing.Size(556, 36);
            this.AdvancedSettingsPanelName.TabIndex = 12;
            this.AdvancedSettingsPanelName.TitleFont = new System.Drawing.Font("Segoe UI", 7.65F, System.Drawing.FontStyle.Bold);
            this.AdvancedSettingsPanelName.TitleForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.AdvancedSettingsPanelName.TitleText = "Advanced Settings";
            this.AdvancedSettingsPanelName.UnfolderImage = ((System.Drawing.Image)(resources.GetObject("AdvancedSettingsPanelName.UnfolderImage")));
            // 
            // UseFrameLockLabel
            // 
            this.UseFrameLockLabel.BackColor = System.Drawing.Color.Transparent;
            this.UseFrameLockLabel.ForeColor = System.Drawing.Color.Black;
            this.UseFrameLockLabel.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.UseFrameLockLabel.Location = new System.Drawing.Point(4, 133);
            this.UseFrameLockLabel.Name = "UseFrameLockLabel";
            this.UseFrameLockLabel.Size = new System.Drawing.Size(430, 22);
            this.UseFrameLockLabel.TabIndex = 37;
            this.UseFrameLockLabel.Text = "Allow Game FrameLocking to increase TheNoobBot performance";
            this.UseFrameLockLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // UseFrameLock
            // 
            this.UseFrameLock.BackColor = System.Drawing.Color.White;
            this.UseFrameLock.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.UseFrameLock.ForeColor = System.Drawing.Color.Black;
            this.UseFrameLock.Location = new System.Drawing.Point(448, 133);
            this.UseFrameLock.MaximumSize = new System.Drawing.Size(60, 20);
            this.UseFrameLock.MinimumSize = new System.Drawing.Size(60, 20);
            this.UseFrameLock.Name = "UseFrameLock";
            this.UseFrameLock.OffText = "OFF";
            this.UseFrameLock.OnText = "ON";
            this.UseFrameLock.Size = new System.Drawing.Size(60, 20);
            this.UseFrameLock.TabIndex = 36;
            this.UseFrameLock.Value = true;
            // 
            // HideSDKFilesLabel
            // 
            this.HideSDKFilesLabel.BackColor = System.Drawing.Color.Transparent;
            this.HideSDKFilesLabel.ForeColor = System.Drawing.Color.Black;
            this.HideSDKFilesLabel.Location = new System.Drawing.Point(4, 104);
            this.HideSDKFilesLabel.Name = "HideSDKFilesLabel";
            this.HideSDKFilesLabel.Size = new System.Drawing.Size(154, 22);
            this.HideSDKFilesLabel.TabIndex = 35;
            this.HideSDKFilesLabel.Text = "Hide SDK Files (.cs)";
            // 
            // HideSDKFiles
            // 
            this.HideSDKFiles.BackColor = System.Drawing.Color.White;
            this.HideSDKFiles.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.HideSDKFiles.ForeColor = System.Drawing.Color.Black;
            this.HideSDKFiles.Location = new System.Drawing.Point(163, 104);
            this.HideSDKFiles.MaximumSize = new System.Drawing.Size(60, 20);
            this.HideSDKFiles.MinimumSize = new System.Drawing.Size(60, 20);
            this.HideSDKFiles.Name = "HideSDKFiles";
            this.HideSDKFiles.OffText = "OFF";
            this.HideSDKFiles.OnText = "ON";
            this.HideSDKFiles.Size = new System.Drawing.Size(60, 20);
            this.HideSDKFiles.TabIndex = 34;
            this.HideSDKFiles.Value = true;
            // 
            // AutoCloseChatFrameLabel
            // 
            this.AutoCloseChatFrameLabel.BackColor = System.Drawing.Color.Transparent;
            this.AutoCloseChatFrameLabel.ForeColor = System.Drawing.Color.Black;
            this.AutoCloseChatFrameLabel.Location = new System.Drawing.Point(289, 104);
            this.AutoCloseChatFrameLabel.Name = "AutoCloseChatFrameLabel";
            this.AutoCloseChatFrameLabel.Size = new System.Drawing.Size(154, 22);
            this.AutoCloseChatFrameLabel.TabIndex = 32;
            this.AutoCloseChatFrameLabel.Text = "Auto Close Chat";
            // 
            // AutoCloseChatFrame
            // 
            this.AutoCloseChatFrame.BackColor = System.Drawing.Color.White;
            this.AutoCloseChatFrame.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.AutoCloseChatFrame.ForeColor = System.Drawing.Color.Black;
            this.AutoCloseChatFrame.Location = new System.Drawing.Point(448, 104);
            this.AutoCloseChatFrame.MaximumSize = new System.Drawing.Size(60, 20);
            this.AutoCloseChatFrame.MinimumSize = new System.Drawing.Size(60, 20);
            this.AutoCloseChatFrame.Name = "AutoCloseChatFrame";
            this.AutoCloseChatFrame.OffText = "OFF";
            this.AutoCloseChatFrame.OnText = "ON";
            this.AutoCloseChatFrame.Size = new System.Drawing.Size(60, 20);
            this.AutoCloseChatFrame.TabIndex = 31;
            this.AutoCloseChatFrame.Value = false;
            // 
            // AlwaysOnTopFeatureLabel
            // 
            this.AlwaysOnTopFeatureLabel.BackColor = System.Drawing.Color.Transparent;
            this.AlwaysOnTopFeatureLabel.ForeColor = System.Drawing.Color.Black;
            this.AlwaysOnTopFeatureLabel.Location = new System.Drawing.Point(290, 76);
            this.AlwaysOnTopFeatureLabel.Name = "AlwaysOnTopFeatureLabel";
            this.AlwaysOnTopFeatureLabel.Size = new System.Drawing.Size(154, 22);
            this.AlwaysOnTopFeatureLabel.TabIndex = 30;
            this.AlwaysOnTopFeatureLabel.Text = "Always On Top";
            // 
            // ActivateAlwaysOnTopFeature
            // 
            this.ActivateAlwaysOnTopFeature.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(229)))), ((int)(((byte)(230)))));
            this.ActivateAlwaysOnTopFeature.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.ActivateAlwaysOnTopFeature.Location = new System.Drawing.Point(449, 76);
            this.ActivateAlwaysOnTopFeature.MaximumSize = new System.Drawing.Size(60, 20);
            this.ActivateAlwaysOnTopFeature.MinimumSize = new System.Drawing.Size(60, 20);
            this.ActivateAlwaysOnTopFeature.Name = "ActivateAlwaysOnTopFeature";
            this.ActivateAlwaysOnTopFeature.OffText = "OFF";
            this.ActivateAlwaysOnTopFeature.OnText = "ON";
            this.ActivateAlwaysOnTopFeature.Size = new System.Drawing.Size(60, 20);
            this.ActivateAlwaysOnTopFeature.TabIndex = 29;
            this.ActivateAlwaysOnTopFeature.Value = false;
            // 
            // AllowTNBToSetYourMaxFpsLabel
            // 
            this.AllowTNBToSetYourMaxFpsLabel.BackColor = System.Drawing.Color.Transparent;
            this.AllowTNBToSetYourMaxFpsLabel.ForeColor = System.Drawing.Color.Black;
            this.AllowTNBToSetYourMaxFpsLabel.Location = new System.Drawing.Point(290, 48);
            this.AllowTNBToSetYourMaxFpsLabel.Name = "AllowTNBToSetYourMaxFpsLabel";
            this.AllowTNBToSetYourMaxFpsLabel.Size = new System.Drawing.Size(154, 22);
            this.AllowTNBToSetYourMaxFpsLabel.TabIndex = 28;
            this.AllowTNBToSetYourMaxFpsLabel.Text = "Uncap MaxFPS (recommended)";
            // 
            // AllowTNBToSetYourMaxFps
            // 
            this.AllowTNBToSetYourMaxFps.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(229)))), ((int)(((byte)(230)))));
            this.AllowTNBToSetYourMaxFps.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.AllowTNBToSetYourMaxFps.ForeColor = System.Drawing.Color.Black;
            this.AllowTNBToSetYourMaxFps.Location = new System.Drawing.Point(449, 48);
            this.AllowTNBToSetYourMaxFps.MaximumSize = new System.Drawing.Size(60, 20);
            this.AllowTNBToSetYourMaxFps.MinimumSize = new System.Drawing.Size(60, 20);
            this.AllowTNBToSetYourMaxFps.Name = "AllowTNBToSetYourMaxFps";
            this.AllowTNBToSetYourMaxFps.OffText = "OFF";
            this.AllowTNBToSetYourMaxFps.OnText = "ON";
            this.AllowTNBToSetYourMaxFps.Size = new System.Drawing.Size(60, 20);
            this.AllowTNBToSetYourMaxFps.TabIndex = 27;
            this.AllowTNBToSetYourMaxFps.Value = false;
            // 
            // MaxDistanceToGoToMailboxesOrNPCs
            // 
            this.MaxDistanceToGoToMailboxesOrNPCs.ForeColor = System.Drawing.Color.Black;
            this.MaxDistanceToGoToMailboxesOrNPCs.Location = new System.Drawing.Point(162, 76);
            this.MaxDistanceToGoToMailboxesOrNPCs.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.MaxDistanceToGoToMailboxesOrNPCs.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.MaxDistanceToGoToMailboxesOrNPCs.Name = "MaxDistanceToGoToMailboxesOrNPCs";
            this.MaxDistanceToGoToMailboxesOrNPCs.Size = new System.Drawing.Size(77, 22);
            this.MaxDistanceToGoToMailboxesOrNPCs.TabIndex = 26;
            this.MaxDistanceToGoToMailboxesOrNPCs.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // MaxDistanceToGoToMailboxesOrNPCsLabel
            // 
            this.MaxDistanceToGoToMailboxesOrNPCsLabel.BackColor = System.Drawing.Color.Transparent;
            this.MaxDistanceToGoToMailboxesOrNPCsLabel.ForeColor = System.Drawing.Color.Black;
            this.MaxDistanceToGoToMailboxesOrNPCsLabel.Location = new System.Drawing.Point(3, 76);
            this.MaxDistanceToGoToMailboxesOrNPCsLabel.Name = "MaxDistanceToGoToMailboxesOrNPCsLabel";
            this.MaxDistanceToGoToMailboxesOrNPCsLabel.Size = new System.Drawing.Size(154, 22);
            this.MaxDistanceToGoToMailboxesOrNPCsLabel.TabIndex = 25;
            this.MaxDistanceToGoToMailboxesOrNPCsLabel.Text = "Npc/Mailbox Search Radius";
            // 
            // ActivatePathFindingFeatureLabel
            // 
            this.ActivatePathFindingFeatureLabel.BackColor = System.Drawing.Color.Transparent;
            this.ActivatePathFindingFeatureLabel.ForeColor = System.Drawing.Color.Black;
            this.ActivatePathFindingFeatureLabel.Location = new System.Drawing.Point(3, 49);
            this.ActivatePathFindingFeatureLabel.Name = "ActivatePathFindingFeatureLabel";
            this.ActivatePathFindingFeatureLabel.Size = new System.Drawing.Size(154, 22);
            this.ActivatePathFindingFeatureLabel.TabIndex = 11;
            this.ActivatePathFindingFeatureLabel.Text = "Use Paths Finder";
            // 
            // ActivatePathFindingFeature
            // 
            this.ActivatePathFindingFeature.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(229)))), ((int)(((byte)(230)))));
            this.ActivatePathFindingFeature.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.ActivatePathFindingFeature.Location = new System.Drawing.Point(162, 49);
            this.ActivatePathFindingFeature.MaximumSize = new System.Drawing.Size(60, 20);
            this.ActivatePathFindingFeature.MinimumSize = new System.Drawing.Size(60, 20);
            this.ActivatePathFindingFeature.Name = "ActivatePathFindingFeature";
            this.ActivatePathFindingFeature.OffText = "OFF";
            this.ActivatePathFindingFeature.OnText = "ON";
            this.ActivatePathFindingFeature.Size = new System.Drawing.Size(60, 20);
            this.ActivatePathFindingFeature.TabIndex = 33;
            this.ActivatePathFindingFeature.Value = true;
            // 
            // MainHeader
            // 
            this.MainHeader.BackgroundImage = global::nManager.Properties.Resources._800x43_controlbar;
            this.MainHeader.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.MainHeader.Location = new System.Drawing.Point(0, 0);
            this.MainHeader.LogoImage = ((System.Drawing.Image)(resources.GetObject("MainHeader.LogoImage")));
            this.MainHeader.Margin = new System.Windows.Forms.Padding(0);
            this.MainHeader.Name = "MainHeader";
            this.MainHeader.Size = new System.Drawing.Size(575, 43);
            this.MainHeader.TabIndex = 7;
            this.MainHeader.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainHeader.TitleForeColor = System.Drawing.Color.White;
            this.MainHeader.TitleText = "General Settings";
            // 
            // GeneralSettings
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(575, 455);
            this.Controls.Add(this.closeB);
            this.Controls.Add(this.resetB);
            this.Controls.Add(this.saveAndCloseB);
            this.Controls.Add(this.MainPanel);
            this.Controls.Add(this.MainHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GeneralSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "General Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GeneralSettings_FormClosing);
            this.MainPanel.ResumeLayout(false);
            this.SpellManagementSystemPanelName.ResumeLayout(false);
            this.PluginsManagementSystemPanelName.ResumeLayout(false);
            this.MountManagementPanelName.ResumeLayout(false);
            this.MountManagementPanelName.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MinimumDistanceToUseMount)).EndInit();
            this.LootingFarmingManagementPanelName.ResumeLayout(false);
            this.LootingFarmingManagementPanelName.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TimeBetweenEachMillingAttempt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TimeBetweenEachProspectingAttempt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GatheringSearchRadius)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DontHarvestIfMoreThanXUnitInAggroRange)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DontHarvestIfPlayerNearRadius)).EndInit();
            this.NPCsRepairSellBuyPanelName.ResumeLayout(false);
            this.NPCsRepairSellBuyPanelName.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SellItemsWhenLessThanXSlotLeft)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RepairWhenDurabilityIsUnderPercent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumberOfFoodsWeGot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumberOfBeverageWeGot)).EndInit();
            this.MailsManagementPanelName.ResumeLayout(false);
            this.MailsManagementPanelName.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SendMailWhenLessThanXSlotLeft)).EndInit();
            this.RegenerationManagementPanelName.ResumeLayout(false);
            this.RegenerationManagementPanelName.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DrinkBeverageWhenManaIsUnderXPercent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EatFoodWhenHealthIsUnderXPercent)).EndInit();
            this.SecuritySystemPanelName.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.StopTNBAfterXMinutes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StopTNBAfterXStucks)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StopTNBIfReceivedAtMostXWhispers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StopTNBAfterXLevelup)).EndInit();
            this.ReloggerManagementPanelName.ResumeLayout(false);
            this.ReloggerManagementPanelName.PerformLayout();
            this.MimesisBroadcasterSettingsPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.BroadcastingPort)).EndInit();
            this.AdvancedSettingsPanelName.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MaxDistanceToGoToMailboxesOrNPCs)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private TnbRibbonManager MainPanel;
        private TnbExpendablePanel MountManagementPanelName;
        private TnbExpendablePanel SpellManagementSystemPanelName;
        private Label MinimumDistanceToUseMountLabel;
        private TextBox FlyingMountName;
        private Label FlyingMountNameLabel;
        private TextBox GroundMountName;
        private Label GroundMountNameLabel;
        private Label UseGroundMountLabel;
        private TnbSwitchButton UseGroundMount;
        private Label DontPullMonstersLabel;
        private TnbSwitchButton DontPullMonsters;
        private Label CanPullUnitsAlreadyInFightLabel;
        private TnbSwitchButton CanPullUnitsAlreadyInFight;
        private Label AutoAssignTalentsLabel;
        private TnbSwitchButton AutoAssignTalents;
        private Label CombatClassLabel;
        private TnbComboBox CombatClass;
        private nManager.Helpful.Forms.UserControls.TnbButton CombatClassSettingsButton;
        private TnbExpendablePanel LootingFarmingManagementPanelName;
        private NumericUpDown DontHarvestIfPlayerNearRadius;
        private Label DontHarvestIfPlayerNearRadiusLabel;
        private Label ActivateMonsterLootingLabel;
        private TnbSwitchButton ActivateMonsterLooting;
        private TnbExpendablePanel RegenerationManagementPanelName;
        private Label labelX9;
        private Label labelX10;
        private NumericUpDown DrinkBeverageWhenManaIsUnderXPercent;
        private TextBox BeverageName;
        private Label BeverageNameLabel;
        private Label labelX14;
        private Label labelX13;
        private NumericUpDown EatFoodWhenHealthIsUnderXPercent;
        private TextBox FoodName;
        private Label FoodNameLabel;
        private NumericUpDown MinimumDistanceToUseMount;
        private Label labelX23;
        private TnbSwitchButton BeastNinjaSkinning;
        private NumericUpDown GatheringSearchRadius;
        private Label GatheringSearchRadiusLabel;
        private NumericUpDown DontHarvestIfMoreThanXUnitInAggroRange;
        private Label DontHarvestIfMoreThanXUnitInAggroRangeLabel;
        private Label ActivateHerbsHarvestingLabel;
        private TnbSwitchButton ActivateHerbsHarvesting;
        private Label ActivateVeinsHarvestingLabel;
        private TnbSwitchButton ActivateVeinsHarvesting;
        private Label ActivateBeastSkinningLabel;
        private TnbSwitchButton ActivateBeastSkinning;
        private Label labelX16;
        private TnbSwitchButton ActivateChestLooting;
        private TnbExpendablePanel ReloggerManagementPanelName;
        private TextBox PasswordOfTheBattleNetAccount;
        private Label PasswordOfTheBattleNetAccountLabel;
        private TextBox EmailOfTheBattleNetAccount;
        private Label EmailOfTheBattleNetAccountLabel;
        private Label ActivateReloggerFeatureLabel;
        private TnbSwitchButton ActivateReloggerFeature;
        private TnbExpendablePanel NPCsRepairSellBuyPanelName;
        private NumericUpDown NumberOfBeverageWeGot;
        private Label NumberOfBeverageWeGotLabel;
        private TextBox DontSellTheseItems;
        private Label DontSellTheseItemsLabel;
        private TnbExpendablePanel MailsManagementPanelName;
        private TextBox MaillingFeatureRecipient;
        private Label MaillingFeatureRecipientLabel;
        private TextBox MaillingFeatureSubject;
        private TextBox ForceToMailTheseItems;
        private Label ForceToMailTheseItemsLabel;
        private CheckBox MailPurple;
        private CheckBox MailBlue;
        private CheckBox MailGreen;
        private CheckBox MailWhite;
        private CheckBox MailGray;
        private Label MaillingFeatureSubjectLabel;
        private Label ActivateAutoMaillingFeatureLabel;
        private TnbSwitchButton ActivateAutoMaillingFeature;
        private TextBox DontMailTheseItems;
        private Label DontMailTheseItemsLabel;
        private TextBox ForceToSellTheseItems;
        private Label ForceToSellTheseItemsLabel;
        private CheckBox SellPurple;
        private CheckBox SellBlue;
        private CheckBox SellGreen;
        private CheckBox SellWhite;
        private CheckBox SellGray;
        private Label labelX52;
        private TnbSwitchButton ActivateAutoSellingFeature;
        private Label ActivateAutoRepairFeatureLabel;
        private TnbSwitchButton ActivateAutoRepairFeature;
        private NumericUpDown NumberOfFoodsWeGot;
        private Label NumberOfFoodsWeGotLabel;
        private Label ActivateSkillsAutoTrainingLabel;
        private TnbSwitchButton ActivateSkillsAutoTraining;
        private TnbExpendablePanel AdvancedSettingsPanelName;
        private Label ActivatePathFindingFeatureLabel;
        private TnbSwitchButton ActivatePathFindingFeature;
        private TnbExpendablePanel SecuritySystemPanelName;
        private Label SecuritySystemLabel;
        private TnbSwitchButton PlayASongIfNewWhispReceived;
        private Label CloseGameLabel;
        private Label labelX33;
        private Label RecordWhispsInLogFilesLabel;
        private Label labelX32;
        private Label labelX31;
        private TnbSwitchButton RecordWhispsInLogFiles;
        private Label StopTNBIfPlayerHaveBeenTeleportedLabel;
        private TnbSwitchButton StopTNBIfPlayerHaveBeenTeleported;
        private Label PauseTNBIfNearByPlayerLabel;
        private Label StopTNBIfHonorPointsLimitReachedLabel;
        private TnbSwitchButton PauseTNBIfNearByPlayer;
        private TnbSwitchButton StopTNBIfHonorPointsLimitReached;
        private NumericUpDown StopTNBAfterXMinutes;
        private Label StopTNBAfterXMinutesLabel;
        private NumericUpDown StopTNBAfterXStucks;
        private Label StopTNBAfterXStucksLabel;
        private NumericUpDown StopTNBIfReceivedAtMostXWhispers;
        private Label StopTNBIfReceivedAtMostXWhispersLabel;
        private NumericUpDown StopTNBAfterXLevelup;
        private Label StopTNBAfterXLevelupLabel;
        private Label StopTNBIfBagAreFullLabel;
        private TnbSwitchButton StopTNBIfBagAreFull;
        private nManager.Helpful.Forms.UserControls.TnbButton saveAndCloseB;
        private nManager.Helpful.Forms.UserControls.TnbButton resetB;
        private nManager.Helpful.Forms.UserControls.TnbButton closeB;
        private Label IgnoreFightIfMountedLabel;
        private TnbSwitchButton IgnoreFightIfMounted;
        private Label UseSpiritHealerLabel;
        private TnbSwitchButton UseSpiritHealer;
        private NumericUpDown MaxDistanceToGoToMailboxesOrNPCs;
        private Label MaxDistanceToGoToMailboxesOrNPCsLabel;
        private Label HarvestDuringLongDistanceMovementsLabel;
        private TnbSwitchButton HarvestDuringLongDistanceMovements;
        private nManager.Helpful.Forms.UserControls.TnbButton addBlackListHarvest;
        private Label DontHarvestTheFollowingObjectsLabel;
        private Label labelX61;
        private TnbSwitchButton ActivateAutoSmelting;
        private TnbSwitchButton DoRegenManaIfLow;
        private TextBox AquaticMountName;
        private Label AquaticMountNameLabel;
        private TextBox BattleNetSubAccount;
        private Label BattleNetSubAccountLabel;
        private Label MakeStackOfElementalsItemsLabel;
        private TnbSwitchButton MakeStackOfElementalsItems;
        private System.Windows.Forms.ToolTip labelsToolTip;
        private Label OnlyUseMillingInTownLabel;
        private TnbSwitchButton OnlyUseMillingInTown;
        private NumericUpDown TimeBetweenEachMillingAttempt;
        private Label TimeBetweenEachMillingAttemptLabel;
        private Label ActivateAutoMillingLabel;
        private TnbSwitchButton ActivateAutoMilling;
        private TextBox HerbsToBeMilled;
        private Label HerbsToBeMilledLabel;
        private Label OnlyUseProspectingInTownLabel;
        private TnbSwitchButton OnlyUseProspectingInTown;
        private NumericUpDown TimeBetweenEachProspectingAttempt;
        private Label TimeBetweenEachProspectingAttemptLabel;
        private Label ActivateAutoProspectingLabel;
        private TnbSwitchButton ActivateAutoProspecting;
        private TextBox MineralsToProspect;
        private Label MineralsToProspectLabel;
        private Label AllowTNBToSetYourMaxFpsLabel;
        private TnbSwitchButton AllowTNBToSetYourMaxFps;
        private Label AutoConfirmOnBoPItemsLabel;
        private TnbSwitchButton AutoConfirmOnBoPItems;
        private Label AlwaysOnTopFeatureLabel;
        private TnbSwitchButton ActivateAlwaysOnTopFeature;
        private nManager.Helpful.Forms.UserControls.TnbButton CombatClassResetSettingsButton;
        private Label SellItemsWhenLessThanXSlotLeftLabel;
        private Label RepairWhenDurabilityIsUnderPercentLabel;
        private NumericUpDown SellItemsWhenLessThanXSlotLeft;
        private NumericUpDown RepairWhenDurabilityIsUnderPercent;
        private NumericUpDown SendMailWhenLessThanXSlotLeft;
        private Label SendMailWhenLessThanXSlotLeftLabel;
        private Label UseHearthstoneLabel;
        private TnbSwitchButton ActiveStopTNBAfterXMinutes;
        private TnbSwitchButton ActiveStopTNBAfterXStucks;
        private TnbSwitchButton ActiveStopTNBIfReceivedAtMostXWhispers;
        private TnbSwitchButton ActiveStopTNBAfterXLevelup;
        private TnbSwitchButton UseHearthstone;
        private Label UseMollELabel;
        private TnbSwitchButton UseMollE;
        private Label UseRobotLabel;
        private TnbSwitchButton UseRobot;
        private TextBox DontHarvestTheFollowingObjects;
        private System.Windows.Forms.Button DontHarvestTheFollowingObjectsHelper;
        private nManager.Helpful.Forms.UserControls.TnbButton HealerClassResetSettingsButton;
        private Label HealerClassLabel;
        private TnbComboBox HealerClass;
        private nManager.Helpful.Forms.UserControls.TnbButton HealerClassSettingsButton;
        private Label TrainMountingCapacityLabel;
        private TnbSwitchButton TrainMountingCapacity;
        private Label OnlyTrainIfWeHave2TimesMoreMoneyThanOurWishListSumLabel;
        private TnbSwitchButton OnlyTrainIfWeHave2TimesMoreMoneyThanOurWishListSum;
        private Label OnlyTrainCurrentlyUsedSkillsLabel;
        private TnbSwitchButton OnlyTrainCurrentlyUsedSkills;
        private Label BecomeApprenticeOfSecondarySkillsWhileQuestingLabel;
        private TnbSwitchButton BecomeApprenticeOfSecondarySkillsWhileQuesting;
        private Label BecomeApprenticeIfNeededByProductLabel;
        private TnbSwitchButton BecomeApprenticeIfNeededByProduct;
        private Label ActivateLootStatisticsLabel;
        private TnbSwitchButton ActivateLootStatistics;
        private TnbExpendablePanel MimesisBroadcasterSettingsPanel;
        private Label BroadcastingIPWan;
        private Label AutoCloseChatFrameLabel;
        private TnbSwitchButton AutoCloseChatFrame;
        private Label BroadcastingIPLan;
        private Label BroadcastingIPLocal;
        private Label BroadcastingPortDefaultLabel;
        private Label BroadcastingIPWanLabel;
        private Label BroadcastingIPLanLabel;
        private Label ActivateBroadcastingMimesisLabel;
        private TnbSwitchButton ActivateBroadcastingMimesis;
        private Label BroadcastingIPLocalLabel;
        private Label BroadcastingPortLabel;
        private NumericUpDown BroadcastingPort;
        private TnbControlMenu MainHeader;
        private TnbExpendablePanel PluginsManagementSystemPanelName;
        private TnbButton ActivatedPluginSettings;
        private TnbButton DeactivatePlugin;
        private TnbButton ActivatePlugin;
        private TnbSwitchButton LaunchExpiredPlugins;
        private TnbSwitchButton ActivatePluginsSystem;
        private Label LaunchExpiredPluginsLabel;
        private Label ActivatePluginsSystemLabel;
        private Label ActivatedPluginsListLabel;
        private ListBox ActivatedPluginsList;
        private Label AvailablePluginsListLabel;
        private ListBox AvailablePluginsList;
        private TnbButton ActivatedPluginResetSettings;
        private Label HideSDKFilesLabel;
        private TnbSwitchButton HideSDKFiles;
        private Label UseFrameLockLabel;
        private TnbSwitchButton UseFrameLock;
        private Label UseLootARangeLabel;
        private TnbSwitchButton UseLootARange;
    }
}