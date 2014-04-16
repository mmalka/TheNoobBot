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
            this.AutoCloseChatFrameLabel = new System.Windows.Forms.Label();
            this.AutoCloseChatFrame = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.AlwaysOnTopFeatureLabel = new System.Windows.Forms.Label();
            this.ActivateAlwaysOnTopFeature = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.labelX73 = new System.Windows.Forms.Label();
            this.AllowTNBToSetYourMaxFps = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.MaxDistanceToGoToMailboxesOrNPCs = new System.Windows.Forms.NumericUpDown();
            this.labelX60 = new System.Windows.Forms.Label();
            this.labelX42 = new System.Windows.Forms.Label();
            this.ActivatePathFindingFeature = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.SecuritySystemPanelName = new nManager.Helpful.Forms.UserControls.TnbExpendablePanel();
            this.UseHearthstoneLabel = new System.Windows.Forms.Label();
            this.ActiveStopTNBAfterXMinutes = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.ActiveStopTNBAfterXStucks = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.ActiveStopTNBIfReceivedAtMostXWhispers = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.ActiveStopTNBAfterXLevelup = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.UseHearthstone = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.labelX39 = new System.Windows.Forms.Label();
            this.labelX45 = new System.Windows.Forms.Label();
            this.PlayASongIfNewWhispReceived = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.labelX34 = new System.Windows.Forms.Label();
            this.labelX33 = new System.Windows.Forms.Label();
            this.labelX43 = new System.Windows.Forms.Label();
            this.labelX32 = new System.Windows.Forms.Label();
            this.labelX31 = new System.Windows.Forms.Label();
            this.RecordWhispsInLogFiles = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.labelX29 = new System.Windows.Forms.Label();
            this.StopTNBIfPlayerHaveBeenTeleported = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.labelX44 = new System.Windows.Forms.Label();
            this.labelX30 = new System.Windows.Forms.Label();
            this.PauseTNBIfNearByPlayer = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.StopTNBIfHonorPointsLimitReached = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.StopTNBAfterXMinutes = new System.Windows.Forms.NumericUpDown();
            this.labelX28 = new System.Windows.Forms.Label();
            this.StopTNBAfterXStucks = new System.Windows.Forms.NumericUpDown();
            this.labelX26 = new System.Windows.Forms.Label();
            this.StopTNBIfReceivedAtMostXWhispers = new System.Windows.Forms.NumericUpDown();
            this.labelX25 = new System.Windows.Forms.Label();
            this.StopTNBAfterXLevelup = new System.Windows.Forms.NumericUpDown();
            this.labelX24 = new System.Windows.Forms.Label();
            this.labelX27 = new System.Windows.Forms.Label();
            this.StopTNBIfBagAreFull = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
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
            this.labelX56 = new System.Windows.Forms.Label();
            this.MaillingFeatureSubject = new System.Windows.Forms.TextBox();
            this.ForceToMailTheseItems = new System.Windows.Forms.TextBox();
            this.labelX48 = new System.Windows.Forms.Label();
            this.labelX54 = new System.Windows.Forms.Label();
            this.labelX55 = new System.Windows.Forms.Label();
            this.ActivateAutoMaillingFeature = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.DontMailTheseItems = new System.Windows.Forms.TextBox();
            this.labelX58 = new System.Windows.Forms.Label();
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
            this.labelX53 = new System.Windows.Forms.Label();
            this.labelX52 = new System.Windows.Forms.Label();
            this.ActivateAutoSellingFeature = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.labelX51 = new System.Windows.Forms.Label();
            this.ActivateAutoRepairFeature = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.NumberOfFoodsWeGot = new System.Windows.Forms.NumericUpDown();
            this.labelX50 = new System.Windows.Forms.Label();
            this.NumberOfBeverageWeGot = new System.Windows.Forms.NumericUpDown();
            this.labelX41 = new System.Windows.Forms.Label();
            this.DontSellTheseItems = new System.Windows.Forms.TextBox();
            this.labelX46 = new System.Windows.Forms.Label();
            this.ReloggerManagementPanelName = new nManager.Helpful.Forms.UserControls.TnbExpendablePanel();
            this.BattleNetSubAccount = new System.Windows.Forms.TextBox();
            this.labelX67 = new System.Windows.Forms.Label();
            this.labelX38 = new System.Windows.Forms.Label();
            this.ActivateReloggerFeature = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.PasswordOfTheBattleNetAccount = new System.Windows.Forms.TextBox();
            this.labelX37 = new System.Windows.Forms.Label();
            this.EmailOfTheBattleNetAccount = new System.Windows.Forms.TextBox();
            this.labelX40 = new System.Windows.Forms.Label();
            this.LootingFarmingManagementPanelName = new nManager.Helpful.Forms.UserControls.TnbExpendablePanel();
            this.ActivateLootStatisticsLabel = new System.Windows.Forms.Label();
            this.ActivateLootStatistics = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.DontHarvestTheFollowingObjectsHelper = new System.Windows.Forms.Button();
            this.DontHarvestTheFollowingObjects = new System.Windows.Forms.TextBox();
            this.AutoConfirmOnBoPItems = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.AutoConfirmOnBoPItemsLabel = new System.Windows.Forms.Label();
            this.labelX69 = new System.Windows.Forms.Label();
            this.OnlyUseMillingInTown = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.TimeBetweenEachMillingAttempt = new System.Windows.Forms.NumericUpDown();
            this.labelX70 = new System.Windows.Forms.Label();
            this.labelX71 = new System.Windows.Forms.Label();
            this.ActivateAutoMilling = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.HerbsToBeMilled = new System.Windows.Forms.TextBox();
            this.labelX72 = new System.Windows.Forms.Label();
            this.labelX68 = new System.Windows.Forms.Label();
            this.MakeStackOfElementalsItems = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.labelX65 = new System.Windows.Forms.Label();
            this.OnlyUseProspectingInTown = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.TimeBetweenEachProspectingAttempt = new System.Windows.Forms.NumericUpDown();
            this.labelX64 = new System.Windows.Forms.Label();
            this.labelX63 = new System.Windows.Forms.Label();
            this.ActivateAutoProspecting = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.MineralsToProspect = new System.Windows.Forms.TextBox();
            this.labelX62 = new System.Windows.Forms.Label();
            this.labelX61 = new System.Windows.Forms.Label();
            this.ActivateAutoSmelting = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.labelX36 = new System.Windows.Forms.Label();
            this.addBlackListHarvest = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.labelX35 = new System.Windows.Forms.Label();
            this.HarvestDuringLongDistanceMovements = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.labelX23 = new System.Windows.Forms.Label();
            this.BeastNinjaSkinning = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.GatheringSearchRadius = new System.Windows.Forms.NumericUpDown();
            this.labelX22 = new System.Windows.Forms.Label();
            this.DontHarvestIfMoreThanXUnitInAggroRange = new System.Windows.Forms.NumericUpDown();
            this.labelX21 = new System.Windows.Forms.Label();
            this.labelX20 = new System.Windows.Forms.Label();
            this.ActivateHerbsHarvesting = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.labelX19 = new System.Windows.Forms.Label();
            this.ActivateVeinsHarvesting = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.labelX17 = new System.Windows.Forms.Label();
            this.ActivateBeastSkinning = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.labelX16 = new System.Windows.Forms.Label();
            this.ActivateChestLooting = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.DontHarvestIfPlayerNearRadius = new System.Windows.Forms.NumericUpDown();
            this.labelX12 = new System.Windows.Forms.Label();
            this.labelX18 = new System.Windows.Forms.Label();
            this.ActivateMonsterLooting = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.RegenerationManagementPanelName = new nManager.Helpful.Forms.UserControls.TnbExpendablePanel();
            this.DoRegenManaIfLow = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.labelX9 = new System.Windows.Forms.Label();
            this.labelX10 = new System.Windows.Forms.Label();
            this.DrinkBeverageWhenManaIsUnderXPercent = new System.Windows.Forms.NumericUpDown();
            this.BeverageName = new System.Windows.Forms.TextBox();
            this.labelX15 = new System.Windows.Forms.Label();
            this.labelX14 = new System.Windows.Forms.Label();
            this.labelX13 = new System.Windows.Forms.Label();
            this.EatFoodWhenHealthIsUnderXPercent = new System.Windows.Forms.NumericUpDown();
            this.FoodName = new System.Windows.Forms.TextBox();
            this.labelX11 = new System.Windows.Forms.Label();
            this.MountManagementPanelName = new nManager.Helpful.Forms.UserControls.TnbExpendablePanel();
            this.AquaticMountName = new System.Windows.Forms.TextBox();
            this.labelX66 = new System.Windows.Forms.Label();
            this.labelX57 = new System.Windows.Forms.Label();
            this.IgnoreFightIfMounted = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.MinimumDistanceToUseMount = new System.Windows.Forms.NumericUpDown();
            this.labelX8 = new System.Windows.Forms.Label();
            this.FlyingMountName = new System.Windows.Forms.TextBox();
            this.labelX7 = new System.Windows.Forms.Label();
            this.GroundMountName = new System.Windows.Forms.TextBox();
            this.labelX6 = new System.Windows.Forms.Label();
            this.labelX5 = new System.Windows.Forms.Label();
            this.UseGroundMount = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
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
            this.labelX59 = new System.Windows.Forms.Label();
            this.UseSpiritHealer = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.ActivateSkillsAutoTrainingLabel = new System.Windows.Forms.Label();
            this.ActivateSkillsAutoTraining = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.labelX4 = new System.Windows.Forms.Label();
            this.DontPullMonsters = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.labelX3 = new System.Windows.Forms.Label();
            this.CanPullUnitsAlreadyInFight = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.labelX2 = new System.Windows.Forms.Label();
            this.AutoAssignTalents = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.CombatClassLabel = new System.Windows.Forms.Label();
            this.CombatClassSettingsButton = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.MainHeader = new nManager.Helpful.Forms.UserControls.TnbControlMenu();
            this.MainPanel.SuspendLayout();
            this.MimesisBroadcasterSettingsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BroadcastingPort)).BeginInit();
            this.AdvancedSettingsPanelName.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MaxDistanceToGoToMailboxesOrNPCs)).BeginInit();
            this.SecuritySystemPanelName.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.StopTNBAfterXMinutes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StopTNBAfterXStucks)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StopTNBIfReceivedAtMostXWhispers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StopTNBAfterXLevelup)).BeginInit();
            this.MailsManagementPanelName.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SendMailWhenLessThanXSlotLeft)).BeginInit();
            this.NPCsRepairSellBuyPanelName.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SellItemsWhenLessThanXSlotLeft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RepairWhenDurabilityIsUnderPercent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumberOfFoodsWeGot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumberOfBeverageWeGot)).BeginInit();
            this.ReloggerManagementPanelName.SuspendLayout();
            this.LootingFarmingManagementPanelName.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TimeBetweenEachMillingAttempt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TimeBetweenEachProspectingAttempt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GatheringSearchRadius)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DontHarvestIfMoreThanXUnitInAggroRange)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DontHarvestIfPlayerNearRadius)).BeginInit();
            this.RegenerationManagementPanelName.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DrinkBeverageWhenManaIsUnderXPercent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EatFoodWhenHealthIsUnderXPercent)).BeginInit();
            this.MountManagementPanelName.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MinimumDistanceToUseMount)).BeginInit();
            this.SpellManagementSystemPanelName.SuspendLayout();
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
            this.MainPanel.Controls.Add(this.MimesisBroadcasterSettingsPanel);
            this.MainPanel.Controls.Add(this.AdvancedSettingsPanelName);
            this.MainPanel.Controls.Add(this.SecuritySystemPanelName);
            this.MainPanel.Controls.Add(this.MailsManagementPanelName);
            this.MainPanel.Controls.Add(this.NPCsRepairSellBuyPanelName);
            this.MainPanel.Controls.Add(this.ReloggerManagementPanelName);
            this.MainPanel.Controls.Add(this.LootingFarmingManagementPanelName);
            this.MainPanel.Controls.Add(this.RegenerationManagementPanelName);
            this.MainPanel.Controls.Add(this.MountManagementPanelName);
            this.MainPanel.Controls.Add(this.SpellManagementSystemPanelName);
            this.MainPanel.ForeColor = System.Drawing.Color.Black;
            this.MainPanel.Location = new System.Drawing.Point(1, 43);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(573, 360);
            this.MainPanel.TabIndex = 3;
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
            this.MimesisBroadcasterSettingsPanel.Location = new System.Drawing.Point(0, 288);
            this.MimesisBroadcasterSettingsPanel.Margin = new System.Windows.Forms.Padding(0);
            this.MimesisBroadcasterSettingsPanel.Name = "MimesisBroadcasterSettingsPanel";
            this.MimesisBroadcasterSettingsPanel.OrderIndex = 8;
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
            1,
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
            this.AdvancedSettingsPanelName.Controls.Add(this.AutoCloseChatFrameLabel);
            this.AdvancedSettingsPanelName.Controls.Add(this.AutoCloseChatFrame);
            this.AdvancedSettingsPanelName.Controls.Add(this.AlwaysOnTopFeatureLabel);
            this.AdvancedSettingsPanelName.Controls.Add(this.ActivateAlwaysOnTopFeature);
            this.AdvancedSettingsPanelName.Controls.Add(this.labelX73);
            this.AdvancedSettingsPanelName.Controls.Add(this.AllowTNBToSetYourMaxFps);
            this.AdvancedSettingsPanelName.Controls.Add(this.MaxDistanceToGoToMailboxesOrNPCs);
            this.AdvancedSettingsPanelName.Controls.Add(this.labelX60);
            this.AdvancedSettingsPanelName.Controls.Add(this.labelX42);
            this.AdvancedSettingsPanelName.Controls.Add(this.ActivatePathFindingFeature);
            this.AdvancedSettingsPanelName.Fold = true;
            this.AdvancedSettingsPanelName.FolderImage = ((System.Drawing.Image)(resources.GetObject("AdvancedSettingsPanelName.FolderImage")));
            this.AdvancedSettingsPanelName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.AdvancedSettingsPanelName.HeaderBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.AdvancedSettingsPanelName.HeaderImage = ((System.Drawing.Image)(resources.GetObject("AdvancedSettingsPanelName.HeaderImage")));
            this.AdvancedSettingsPanelName.HeaderSize = new System.Drawing.Size(556, 36);
            this.AdvancedSettingsPanelName.Location = new System.Drawing.Point(0, 324);
            this.AdvancedSettingsPanelName.Margin = new System.Windows.Forms.Padding(0);
            this.AdvancedSettingsPanelName.Name = "AdvancedSettingsPanelName";
            this.AdvancedSettingsPanelName.OrderIndex = 9;
            this.AdvancedSettingsPanelName.Padding = new System.Windows.Forms.Padding(0, 0, 0, 12);
            this.AdvancedSettingsPanelName.Size = new System.Drawing.Size(556, 36);
            this.AdvancedSettingsPanelName.TabIndex = 12;
            this.AdvancedSettingsPanelName.TitleFont = new System.Drawing.Font("Segoe UI", 7.65F, System.Drawing.FontStyle.Bold);
            this.AdvancedSettingsPanelName.TitleForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.AdvancedSettingsPanelName.TitleText = "Advanced Settings";
            this.AdvancedSettingsPanelName.UnfolderImage = ((System.Drawing.Image)(resources.GetObject("AdvancedSettingsPanelName.UnfolderImage")));
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
            // labelX73
            // 
            this.labelX73.BackColor = System.Drawing.Color.Transparent;
            this.labelX73.ForeColor = System.Drawing.Color.Black;
            this.labelX73.Location = new System.Drawing.Point(290, 48);
            this.labelX73.Name = "labelX73";
            this.labelX73.Size = new System.Drawing.Size(154, 22);
            this.labelX73.TabIndex = 28;
            this.labelX73.Text = "Uncap MaxFPS (recommended)";
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
            // labelX60
            // 
            this.labelX60.BackColor = System.Drawing.Color.Transparent;
            this.labelX60.ForeColor = System.Drawing.Color.Black;
            this.labelX60.Location = new System.Drawing.Point(3, 76);
            this.labelX60.Name = "labelX60";
            this.labelX60.Size = new System.Drawing.Size(154, 22);
            this.labelX60.TabIndex = 25;
            this.labelX60.Text = "Npc/Mailbox Search Radius";
            // 
            // labelX42
            // 
            this.labelX42.BackColor = System.Drawing.Color.Transparent;
            this.labelX42.ForeColor = System.Drawing.Color.Black;
            this.labelX42.Location = new System.Drawing.Point(3, 49);
            this.labelX42.Name = "labelX42";
            this.labelX42.Size = new System.Drawing.Size(154, 22);
            this.labelX42.TabIndex = 11;
            this.labelX42.Text = "Use Paths Finder";
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
            this.ActivatePathFindingFeature.Value = false;
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
            this.SecuritySystemPanelName.Controls.Add(this.labelX39);
            this.SecuritySystemPanelName.Controls.Add(this.labelX45);
            this.SecuritySystemPanelName.Controls.Add(this.PlayASongIfNewWhispReceived);
            this.SecuritySystemPanelName.Controls.Add(this.labelX34);
            this.SecuritySystemPanelName.Controls.Add(this.labelX33);
            this.SecuritySystemPanelName.Controls.Add(this.labelX43);
            this.SecuritySystemPanelName.Controls.Add(this.labelX32);
            this.SecuritySystemPanelName.Controls.Add(this.labelX31);
            this.SecuritySystemPanelName.Controls.Add(this.RecordWhispsInLogFiles);
            this.SecuritySystemPanelName.Controls.Add(this.labelX29);
            this.SecuritySystemPanelName.Controls.Add(this.StopTNBIfPlayerHaveBeenTeleported);
            this.SecuritySystemPanelName.Controls.Add(this.labelX44);
            this.SecuritySystemPanelName.Controls.Add(this.labelX30);
            this.SecuritySystemPanelName.Controls.Add(this.PauseTNBIfNearByPlayer);
            this.SecuritySystemPanelName.Controls.Add(this.StopTNBIfHonorPointsLimitReached);
            this.SecuritySystemPanelName.Controls.Add(this.StopTNBAfterXMinutes);
            this.SecuritySystemPanelName.Controls.Add(this.labelX28);
            this.SecuritySystemPanelName.Controls.Add(this.StopTNBAfterXStucks);
            this.SecuritySystemPanelName.Controls.Add(this.labelX26);
            this.SecuritySystemPanelName.Controls.Add(this.StopTNBIfReceivedAtMostXWhispers);
            this.SecuritySystemPanelName.Controls.Add(this.labelX25);
            this.SecuritySystemPanelName.Controls.Add(this.StopTNBAfterXLevelup);
            this.SecuritySystemPanelName.Controls.Add(this.labelX24);
            this.SecuritySystemPanelName.Controls.Add(this.labelX27);
            this.SecuritySystemPanelName.Controls.Add(this.StopTNBIfBagAreFull);
            this.SecuritySystemPanelName.Fold = true;
            this.SecuritySystemPanelName.FolderImage = ((System.Drawing.Image)(resources.GetObject("SecuritySystemPanelName.FolderImage")));
            this.SecuritySystemPanelName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.SecuritySystemPanelName.HeaderBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.SecuritySystemPanelName.HeaderImage = ((System.Drawing.Image)(resources.GetObject("SecuritySystemPanelName.HeaderImage")));
            this.SecuritySystemPanelName.HeaderSize = new System.Drawing.Size(556, 36);
            this.SecuritySystemPanelName.Location = new System.Drawing.Point(0, 252);
            this.SecuritySystemPanelName.Margin = new System.Windows.Forms.Padding(0);
            this.SecuritySystemPanelName.Name = "SecuritySystemPanelName";
            this.SecuritySystemPanelName.OrderIndex = 7;
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
            // labelX39
            // 
            this.labelX39.BackColor = System.Drawing.Color.Transparent;
            this.labelX39.ForeColor = System.Drawing.Color.Black;
            this.labelX39.Location = new System.Drawing.Point(3, 356);
            this.labelX39.Name = "labelX39";
            this.labelX39.Size = new System.Drawing.Size(154, 22);
            this.labelX39.TabIndex = 41;
            this.labelX39.Text = "Song if New Whisper";
            // 
            // labelX45
            // 
            this.labelX45.BackColor = System.Drawing.Color.Transparent;
            this.labelX45.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX45.ForeColor = System.Drawing.Color.Black;
            this.labelX45.Location = new System.Drawing.Point(3, 274);
            this.labelX45.Name = "labelX45";
            this.labelX45.Size = new System.Drawing.Size(154, 22);
            this.labelX45.TabIndex = 35;
            this.labelX45.Text = "Security:";
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
            // labelX34
            // 
            this.labelX34.BackColor = System.Drawing.Color.Transparent;
            this.labelX34.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX34.ForeColor = System.Drawing.Color.Black;
            this.labelX34.Location = new System.Drawing.Point(3, 52);
            this.labelX34.Name = "labelX34";
            this.labelX34.Size = new System.Drawing.Size(154, 22);
            this.labelX34.TabIndex = 34;
            this.labelX34.Text = "Close game:";
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
            // labelX43
            // 
            this.labelX43.BackColor = System.Drawing.Color.Transparent;
            this.labelX43.ForeColor = System.Drawing.Color.Black;
            this.labelX43.Location = new System.Drawing.Point(3, 330);
            this.labelX43.Name = "labelX43";
            this.labelX43.Size = new System.Drawing.Size(154, 22);
            this.labelX43.TabIndex = 39;
            this.labelX43.Text = "Record whisper in Log file";
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
            // labelX29
            // 
            this.labelX29.BackColor = System.Drawing.Color.Transparent;
            this.labelX29.ForeColor = System.Drawing.Color.Black;
            this.labelX29.Location = new System.Drawing.Point(3, 135);
            this.labelX29.Name = "labelX29";
            this.labelX29.Size = new System.Drawing.Size(154, 22);
            this.labelX29.TabIndex = 30;
            this.labelX29.Text = "If Player Teleported";
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
            // labelX44
            // 
            this.labelX44.BackColor = System.Drawing.Color.Transparent;
            this.labelX44.ForeColor = System.Drawing.Color.Black;
            this.labelX44.Location = new System.Drawing.Point(3, 302);
            this.labelX44.Name = "labelX44";
            this.labelX44.Size = new System.Drawing.Size(154, 22);
            this.labelX44.TabIndex = 37;
            this.labelX44.Text = "Pause Bot if Nearby Player";
            // 
            // labelX30
            // 
            this.labelX30.BackColor = System.Drawing.Color.Transparent;
            this.labelX30.ForeColor = System.Drawing.Color.Black;
            this.labelX30.Location = new System.Drawing.Point(3, 107);
            this.labelX30.Name = "labelX30";
            this.labelX30.Size = new System.Drawing.Size(154, 22);
            this.labelX30.TabIndex = 28;
            this.labelX30.Text = "If reached 4000 Honor Points";
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
            // labelX28
            // 
            this.labelX28.BackColor = System.Drawing.Color.Transparent;
            this.labelX28.ForeColor = System.Drawing.Color.Black;
            this.labelX28.Location = new System.Drawing.Point(3, 247);
            this.labelX28.Name = "labelX28";
            this.labelX28.Size = new System.Drawing.Size(154, 22);
            this.labelX28.TabIndex = 23;
            this.labelX28.Text = "After";
            // 
            // StopTNBAfterXStucks
            // 
            this.StopTNBAfterXStucks.ForeColor = System.Drawing.Color.Black;
            this.StopTNBAfterXStucks.Location = new System.Drawing.Point(162, 219);
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
            // labelX26
            // 
            this.labelX26.BackColor = System.Drawing.Color.Transparent;
            this.labelX26.ForeColor = System.Drawing.Color.Black;
            this.labelX26.Location = new System.Drawing.Point(3, 219);
            this.labelX26.Name = "labelX26";
            this.labelX26.Size = new System.Drawing.Size(154, 22);
            this.labelX26.TabIndex = 21;
            this.labelX26.Text = "After";
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
            // labelX25
            // 
            this.labelX25.BackColor = System.Drawing.Color.Transparent;
            this.labelX25.ForeColor = System.Drawing.Color.Black;
            this.labelX25.Location = new System.Drawing.Point(3, 191);
            this.labelX25.Name = "labelX25";
            this.labelX25.Size = new System.Drawing.Size(154, 22);
            this.labelX25.TabIndex = 19;
            this.labelX25.Text = "If Whisper bigger or equal to";
            // 
            // StopTNBAfterXLevelup
            // 
            this.StopTNBAfterXLevelup.ForeColor = System.Drawing.Color.Black;
            this.StopTNBAfterXLevelup.Location = new System.Drawing.Point(162, 163);
            this.StopTNBAfterXLevelup.Maximum = new decimal(new int[] {
            90,
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
            // labelX24
            // 
            this.labelX24.BackColor = System.Drawing.Color.Transparent;
            this.labelX24.ForeColor = System.Drawing.Color.Black;
            this.labelX24.Location = new System.Drawing.Point(3, 163);
            this.labelX24.Name = "labelX24";
            this.labelX24.Size = new System.Drawing.Size(154, 22);
            this.labelX24.TabIndex = 16;
            this.labelX24.Text = "After";
            // 
            // labelX27
            // 
            this.labelX27.BackColor = System.Drawing.Color.Transparent;
            this.labelX27.ForeColor = System.Drawing.Color.Black;
            this.labelX27.Location = new System.Drawing.Point(3, 80);
            this.labelX27.Name = "labelX27";
            this.labelX27.Size = new System.Drawing.Size(154, 22);
            this.labelX27.TabIndex = 11;
            this.labelX27.Text = "If full Bag";
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
            this.MailsManagementPanelName.Controls.Add(this.labelX56);
            this.MailsManagementPanelName.Controls.Add(this.MaillingFeatureSubject);
            this.MailsManagementPanelName.Controls.Add(this.ForceToMailTheseItems);
            this.MailsManagementPanelName.Controls.Add(this.labelX48);
            this.MailsManagementPanelName.Controls.Add(this.labelX54);
            this.MailsManagementPanelName.Controls.Add(this.labelX55);
            this.MailsManagementPanelName.Controls.Add(this.ActivateAutoMaillingFeature);
            this.MailsManagementPanelName.Controls.Add(this.DontMailTheseItems);
            this.MailsManagementPanelName.Controls.Add(this.labelX58);
            this.MailsManagementPanelName.Fold = true;
            this.MailsManagementPanelName.FolderImage = ((System.Drawing.Image)(resources.GetObject("MailsManagementPanelName.FolderImage")));
            this.MailsManagementPanelName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.MailsManagementPanelName.HeaderBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.MailsManagementPanelName.HeaderImage = ((System.Drawing.Image)(resources.GetObject("MailsManagementPanelName.HeaderImage")));
            this.MailsManagementPanelName.HeaderSize = new System.Drawing.Size(556, 36);
            this.MailsManagementPanelName.Location = new System.Drawing.Point(0, 216);
            this.MailsManagementPanelName.Margin = new System.Windows.Forms.Padding(0);
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
            this.MailPurple.Location = new System.Drawing.Point(162, 191);
            this.MailPurple.Name = "MailPurple";
            this.MailPurple.Size = new System.Drawing.Size(96, 23);
            this.MailPurple.TabIndex = 33;
            this.MailPurple.Text = "Mail Purple items";
            // 
            // MailBlue
            // 
            this.MailBlue.ForeColor = System.Drawing.Color.Black;
            this.MailBlue.Location = new System.Drawing.Point(11, 191);
            this.MailBlue.Name = "MailBlue";
            this.MailBlue.Size = new System.Drawing.Size(96, 23);
            this.MailBlue.TabIndex = 32;
            this.MailBlue.Text = "Mail Blue items";
            // 
            // MailGreen
            // 
            this.MailGreen.ForeColor = System.Drawing.Color.Black;
            this.MailGreen.Location = new System.Drawing.Point(302, 162);
            this.MailGreen.Name = "MailGreen";
            this.MailGreen.Size = new System.Drawing.Size(96, 23);
            this.MailGreen.TabIndex = 31;
            this.MailGreen.Text = "Mail Green items";
            // 
            // MailWhite
            // 
            this.MailWhite.ForeColor = System.Drawing.Color.Black;
            this.MailWhite.Location = new System.Drawing.Point(162, 161);
            this.MailWhite.Name = "MailWhite";
            this.MailWhite.Size = new System.Drawing.Size(96, 23);
            this.MailWhite.TabIndex = 30;
            this.MailWhite.Text = "Mail White items";
            // 
            // MailGray
            // 
            this.MailGray.ForeColor = System.Drawing.Color.Black;
            this.MailGray.Location = new System.Drawing.Point(11, 162);
            this.MailGray.Name = "MailGray";
            this.MailGray.Size = new System.Drawing.Size(96, 23);
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
            // labelX56
            // 
            this.labelX56.BackColor = System.Drawing.Color.Transparent;
            this.labelX56.ForeColor = System.Drawing.Color.Black;
            this.labelX56.Location = new System.Drawing.Point(3, 106);
            this.labelX56.Name = "labelX56";
            this.labelX56.Size = new System.Drawing.Size(154, 22);
            this.labelX56.TabIndex = 37;
            this.labelX56.Text = "Mail Recipient";
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
            // labelX48
            // 
            this.labelX48.BackColor = System.Drawing.Color.Transparent;
            this.labelX48.ForeColor = System.Drawing.Color.Black;
            this.labelX48.Location = new System.Drawing.Point(272, 223);
            this.labelX48.Name = "labelX48";
            this.labelX48.Size = new System.Drawing.Size(227, 22);
            this.labelX48.TabIndex = 34;
            this.labelX48.Text = "Force Mail List (one item by line)";
            // 
            // labelX54
            // 
            this.labelX54.BackColor = System.Drawing.Color.Transparent;
            this.labelX54.ForeColor = System.Drawing.Color.Black;
            this.labelX54.Location = new System.Drawing.Point(3, 134);
            this.labelX54.Name = "labelX54";
            this.labelX54.Size = new System.Drawing.Size(154, 22);
            this.labelX54.TabIndex = 28;
            this.labelX54.Text = "Subject";
            // 
            // labelX55
            // 
            this.labelX55.BackColor = System.Drawing.Color.Transparent;
            this.labelX55.ForeColor = System.Drawing.Color.Black;
            this.labelX55.Location = new System.Drawing.Point(3, 51);
            this.labelX55.Name = "labelX55";
            this.labelX55.Size = new System.Drawing.Size(154, 22);
            this.labelX55.TabIndex = 26;
            this.labelX55.Text = "Use Mail";
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
            // labelX58
            // 
            this.labelX58.BackColor = System.Drawing.Color.Transparent;
            this.labelX58.ForeColor = System.Drawing.Color.Black;
            this.labelX58.Location = new System.Drawing.Point(12, 223);
            this.labelX58.Name = "labelX58";
            this.labelX58.Size = new System.Drawing.Size(228, 22);
            this.labelX58.TabIndex = 14;
            this.labelX58.Text = "Do not Mail List (one item by line)";
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
            this.NPCsRepairSellBuyPanelName.Controls.Add(this.labelX53);
            this.NPCsRepairSellBuyPanelName.Controls.Add(this.labelX52);
            this.NPCsRepairSellBuyPanelName.Controls.Add(this.ActivateAutoSellingFeature);
            this.NPCsRepairSellBuyPanelName.Controls.Add(this.labelX51);
            this.NPCsRepairSellBuyPanelName.Controls.Add(this.ActivateAutoRepairFeature);
            this.NPCsRepairSellBuyPanelName.Controls.Add(this.NumberOfFoodsWeGot);
            this.NPCsRepairSellBuyPanelName.Controls.Add(this.labelX50);
            this.NPCsRepairSellBuyPanelName.Controls.Add(this.NumberOfBeverageWeGot);
            this.NPCsRepairSellBuyPanelName.Controls.Add(this.labelX41);
            this.NPCsRepairSellBuyPanelName.Controls.Add(this.DontSellTheseItems);
            this.NPCsRepairSellBuyPanelName.Controls.Add(this.labelX46);
            this.NPCsRepairSellBuyPanelName.Fold = true;
            this.NPCsRepairSellBuyPanelName.FolderImage = ((System.Drawing.Image)(resources.GetObject("NPCsRepairSellBuyPanelName.FolderImage")));
            this.NPCsRepairSellBuyPanelName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.NPCsRepairSellBuyPanelName.HeaderBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.NPCsRepairSellBuyPanelName.HeaderImage = ((System.Drawing.Image)(resources.GetObject("NPCsRepairSellBuyPanelName.HeaderImage")));
            this.NPCsRepairSellBuyPanelName.HeaderSize = new System.Drawing.Size(556, 36);
            this.NPCsRepairSellBuyPanelName.Location = new System.Drawing.Point(0, 180);
            this.NPCsRepairSellBuyPanelName.Margin = new System.Windows.Forms.Padding(0);
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
            this.SellPurple.Location = new System.Drawing.Point(162, 194);
            this.SellPurple.Name = "SellPurple";
            this.SellPurple.Size = new System.Drawing.Size(96, 23);
            this.SellPurple.TabIndex = 33;
            this.SellPurple.Text = "Sell Purple items";
            // 
            // SellBlue
            // 
            this.SellBlue.ForeColor = System.Drawing.Color.Black;
            this.SellBlue.Location = new System.Drawing.Point(11, 194);
            this.SellBlue.Name = "SellBlue";
            this.SellBlue.Size = new System.Drawing.Size(96, 23);
            this.SellBlue.TabIndex = 32;
            this.SellBlue.Text = "Sell Blue items";
            // 
            // SellGreen
            // 
            this.SellGreen.ForeColor = System.Drawing.Color.Black;
            this.SellGreen.Location = new System.Drawing.Point(302, 165);
            this.SellGreen.Name = "SellGreen";
            this.SellGreen.Size = new System.Drawing.Size(96, 23);
            this.SellGreen.TabIndex = 31;
            this.SellGreen.Text = "Sell Green items";
            // 
            // SellWhite
            // 
            this.SellWhite.ForeColor = System.Drawing.Color.Black;
            this.SellWhite.Location = new System.Drawing.Point(162, 164);
            this.SellWhite.Name = "SellWhite";
            this.SellWhite.Size = new System.Drawing.Size(96, 23);
            this.SellWhite.TabIndex = 30;
            this.SellWhite.Text = "Sell White items";
            // 
            // SellGray
            // 
            this.SellGray.ForeColor = System.Drawing.Color.Black;
            this.SellGray.Location = new System.Drawing.Point(11, 165);
            this.SellGray.Name = "SellGray";
            this.SellGray.Size = new System.Drawing.Size(96, 23);
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
            this.RepairWhenDurabilityIsUnderPercent.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
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
            // labelX53
            // 
            this.labelX53.BackColor = System.Drawing.Color.Transparent;
            this.labelX53.ForeColor = System.Drawing.Color.Black;
            this.labelX53.Location = new System.Drawing.Point(272, 226);
            this.labelX53.Name = "labelX53";
            this.labelX53.Size = new System.Drawing.Size(227, 22);
            this.labelX53.TabIndex = 34;
            this.labelX53.Text = "Force Sell List (one item by line)";
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
            // labelX51
            // 
            this.labelX51.BackColor = System.Drawing.Color.Transparent;
            this.labelX51.ForeColor = System.Drawing.Color.Black;
            this.labelX51.Location = new System.Drawing.Point(3, 109);
            this.labelX51.Name = "labelX51";
            this.labelX51.Size = new System.Drawing.Size(154, 22);
            this.labelX51.TabIndex = 26;
            this.labelX51.Text = "Repair";
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
            // labelX50
            // 
            this.labelX50.BackColor = System.Drawing.Color.Transparent;
            this.labelX50.ForeColor = System.Drawing.Color.Black;
            this.labelX50.Location = new System.Drawing.Point(280, 80);
            this.labelX50.Name = "labelX50";
            this.labelX50.Size = new System.Drawing.Size(154, 22);
            this.labelX50.TabIndex = 23;
            this.labelX50.Text = "Food Amount";
            // 
            // NumberOfBeverageWeGot
            // 
            this.NumberOfBeverageWeGot.ForeColor = System.Drawing.Color.Black;
            this.NumberOfBeverageWeGot.Location = new System.Drawing.Point(162, 81);
            this.NumberOfBeverageWeGot.Name = "NumberOfBeverageWeGot";
            this.NumberOfBeverageWeGot.Size = new System.Drawing.Size(63, 22);
            this.NumberOfBeverageWeGot.TabIndex = 18;
            // 
            // labelX41
            // 
            this.labelX41.BackColor = System.Drawing.Color.Transparent;
            this.labelX41.ForeColor = System.Drawing.Color.Black;
            this.labelX41.Location = new System.Drawing.Point(3, 81);
            this.labelX41.Name = "labelX41";
            this.labelX41.Size = new System.Drawing.Size(154, 22);
            this.labelX41.TabIndex = 16;
            this.labelX41.Text = "Drink Amount";
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
            // labelX46
            // 
            this.labelX46.BackColor = System.Drawing.Color.Transparent;
            this.labelX46.ForeColor = System.Drawing.Color.Black;
            this.labelX46.Location = new System.Drawing.Point(12, 226);
            this.labelX46.Name = "labelX46";
            this.labelX46.Size = new System.Drawing.Size(227, 22);
            this.labelX46.TabIndex = 14;
            this.labelX46.Text = "Do not Sell List (one item by line)";
            // 
            // ReloggerManagementPanelName
            // 
            this.ReloggerManagementPanelName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.ReloggerManagementPanelName.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.ReloggerManagementPanelName.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.ReloggerManagementPanelName.ContentSize = new System.Drawing.Size(556, 144);
            this.ReloggerManagementPanelName.Controls.Add(this.BattleNetSubAccount);
            this.ReloggerManagementPanelName.Controls.Add(this.labelX67);
            this.ReloggerManagementPanelName.Controls.Add(this.labelX38);
            this.ReloggerManagementPanelName.Controls.Add(this.ActivateReloggerFeature);
            this.ReloggerManagementPanelName.Controls.Add(this.PasswordOfTheBattleNetAccount);
            this.ReloggerManagementPanelName.Controls.Add(this.labelX37);
            this.ReloggerManagementPanelName.Controls.Add(this.EmailOfTheBattleNetAccount);
            this.ReloggerManagementPanelName.Controls.Add(this.labelX40);
            this.ReloggerManagementPanelName.Fold = true;
            this.ReloggerManagementPanelName.FolderImage = ((System.Drawing.Image)(resources.GetObject("ReloggerManagementPanelName.FolderImage")));
            this.ReloggerManagementPanelName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.ReloggerManagementPanelName.HeaderBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.ReloggerManagementPanelName.HeaderImage = ((System.Drawing.Image)(resources.GetObject("ReloggerManagementPanelName.HeaderImage")));
            this.ReloggerManagementPanelName.HeaderSize = new System.Drawing.Size(556, 36);
            this.ReloggerManagementPanelName.Location = new System.Drawing.Point(0, 144);
            this.ReloggerManagementPanelName.Margin = new System.Windows.Forms.Padding(0);
            this.ReloggerManagementPanelName.Name = "ReloggerManagementPanelName";
            this.ReloggerManagementPanelName.OrderIndex = 4;
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
            // labelX67
            // 
            this.labelX67.BackColor = System.Drawing.Color.Transparent;
            this.labelX67.ForeColor = System.Drawing.Color.Black;
            this.labelX67.Location = new System.Drawing.Point(3, 140);
            this.labelX67.Name = "labelX67";
            this.labelX67.Size = new System.Drawing.Size(154, 22);
            this.labelX67.TabIndex = 23;
            this.labelX67.Text = "BattleNet Account";
            // 
            // labelX38
            // 
            this.labelX38.BackColor = System.Drawing.Color.Transparent;
            this.labelX38.ForeColor = System.Drawing.Color.Black;
            this.labelX38.Location = new System.Drawing.Point(3, 52);
            this.labelX38.Name = "labelX38";
            this.labelX38.Size = new System.Drawing.Size(154, 22);
            this.labelX38.TabIndex = 13;
            this.labelX38.Text = "Relogger";
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
            // labelX37
            // 
            this.labelX37.BackColor = System.Drawing.Color.Transparent;
            this.labelX37.ForeColor = System.Drawing.Color.Black;
            this.labelX37.Location = new System.Drawing.Point(3, 108);
            this.labelX37.Name = "labelX37";
            this.labelX37.Size = new System.Drawing.Size(154, 22);
            this.labelX37.TabIndex = 21;
            this.labelX37.Text = "Account Password";
            // 
            // EmailOfTheBattleNetAccount
            // 
            this.EmailOfTheBattleNetAccount.ForeColor = System.Drawing.Color.Black;
            this.EmailOfTheBattleNetAccount.Location = new System.Drawing.Point(162, 83);
            this.EmailOfTheBattleNetAccount.Name = "EmailOfTheBattleNetAccount";
            this.EmailOfTheBattleNetAccount.Size = new System.Drawing.Size(175, 22);
            this.EmailOfTheBattleNetAccount.TabIndex = 13;
            // 
            // labelX40
            // 
            this.labelX40.BackColor = System.Drawing.Color.Transparent;
            this.labelX40.ForeColor = System.Drawing.Color.Black;
            this.labelX40.Location = new System.Drawing.Point(3, 80);
            this.labelX40.Name = "labelX40";
            this.labelX40.Size = new System.Drawing.Size(154, 22);
            this.labelX40.TabIndex = 12;
            this.labelX40.Text = "Account Email";
            // 
            // LootingFarmingManagementPanelName
            // 
            this.LootingFarmingManagementPanelName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.LootingFarmingManagementPanelName.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.LootingFarmingManagementPanelName.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.LootingFarmingManagementPanelName.ContentSize = new System.Drawing.Size(556, 518);
            this.LootingFarmingManagementPanelName.Controls.Add(this.ActivateLootStatisticsLabel);
            this.LootingFarmingManagementPanelName.Controls.Add(this.ActivateLootStatistics);
            this.LootingFarmingManagementPanelName.Controls.Add(this.DontHarvestTheFollowingObjectsHelper);
            this.LootingFarmingManagementPanelName.Controls.Add(this.DontHarvestTheFollowingObjects);
            this.LootingFarmingManagementPanelName.Controls.Add(this.AutoConfirmOnBoPItems);
            this.LootingFarmingManagementPanelName.Controls.Add(this.AutoConfirmOnBoPItemsLabel);
            this.LootingFarmingManagementPanelName.Controls.Add(this.labelX69);
            this.LootingFarmingManagementPanelName.Controls.Add(this.OnlyUseMillingInTown);
            this.LootingFarmingManagementPanelName.Controls.Add(this.TimeBetweenEachMillingAttempt);
            this.LootingFarmingManagementPanelName.Controls.Add(this.labelX70);
            this.LootingFarmingManagementPanelName.Controls.Add(this.labelX71);
            this.LootingFarmingManagementPanelName.Controls.Add(this.ActivateAutoMilling);
            this.LootingFarmingManagementPanelName.Controls.Add(this.HerbsToBeMilled);
            this.LootingFarmingManagementPanelName.Controls.Add(this.labelX72);
            this.LootingFarmingManagementPanelName.Controls.Add(this.labelX68);
            this.LootingFarmingManagementPanelName.Controls.Add(this.MakeStackOfElementalsItems);
            this.LootingFarmingManagementPanelName.Controls.Add(this.labelX65);
            this.LootingFarmingManagementPanelName.Controls.Add(this.OnlyUseProspectingInTown);
            this.LootingFarmingManagementPanelName.Controls.Add(this.TimeBetweenEachProspectingAttempt);
            this.LootingFarmingManagementPanelName.Controls.Add(this.labelX64);
            this.LootingFarmingManagementPanelName.Controls.Add(this.labelX63);
            this.LootingFarmingManagementPanelName.Controls.Add(this.ActivateAutoProspecting);
            this.LootingFarmingManagementPanelName.Controls.Add(this.MineralsToProspect);
            this.LootingFarmingManagementPanelName.Controls.Add(this.labelX62);
            this.LootingFarmingManagementPanelName.Controls.Add(this.labelX61);
            this.LootingFarmingManagementPanelName.Controls.Add(this.ActivateAutoSmelting);
            this.LootingFarmingManagementPanelName.Controls.Add(this.labelX36);
            this.LootingFarmingManagementPanelName.Controls.Add(this.addBlackListHarvest);
            this.LootingFarmingManagementPanelName.Controls.Add(this.labelX35);
            this.LootingFarmingManagementPanelName.Controls.Add(this.HarvestDuringLongDistanceMovements);
            this.LootingFarmingManagementPanelName.Controls.Add(this.labelX23);
            this.LootingFarmingManagementPanelName.Controls.Add(this.BeastNinjaSkinning);
            this.LootingFarmingManagementPanelName.Controls.Add(this.GatheringSearchRadius);
            this.LootingFarmingManagementPanelName.Controls.Add(this.labelX22);
            this.LootingFarmingManagementPanelName.Controls.Add(this.DontHarvestIfMoreThanXUnitInAggroRange);
            this.LootingFarmingManagementPanelName.Controls.Add(this.labelX21);
            this.LootingFarmingManagementPanelName.Controls.Add(this.labelX20);
            this.LootingFarmingManagementPanelName.Controls.Add(this.ActivateHerbsHarvesting);
            this.LootingFarmingManagementPanelName.Controls.Add(this.labelX19);
            this.LootingFarmingManagementPanelName.Controls.Add(this.ActivateVeinsHarvesting);
            this.LootingFarmingManagementPanelName.Controls.Add(this.labelX17);
            this.LootingFarmingManagementPanelName.Controls.Add(this.ActivateBeastSkinning);
            this.LootingFarmingManagementPanelName.Controls.Add(this.labelX16);
            this.LootingFarmingManagementPanelName.Controls.Add(this.ActivateChestLooting);
            this.LootingFarmingManagementPanelName.Controls.Add(this.DontHarvestIfPlayerNearRadius);
            this.LootingFarmingManagementPanelName.Controls.Add(this.labelX12);
            this.LootingFarmingManagementPanelName.Controls.Add(this.labelX18);
            this.LootingFarmingManagementPanelName.Controls.Add(this.ActivateMonsterLooting);
            this.LootingFarmingManagementPanelName.Fold = true;
            this.LootingFarmingManagementPanelName.FolderImage = ((System.Drawing.Image)(resources.GetObject("LootingFarmingManagementPanelName.FolderImage")));
            this.LootingFarmingManagementPanelName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.LootingFarmingManagementPanelName.HeaderBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.LootingFarmingManagementPanelName.HeaderImage = ((System.Drawing.Image)(resources.GetObject("LootingFarmingManagementPanelName.HeaderImage")));
            this.LootingFarmingManagementPanelName.HeaderSize = new System.Drawing.Size(556, 36);
            this.LootingFarmingManagementPanelName.Location = new System.Drawing.Point(0, 108);
            this.LootingFarmingManagementPanelName.Margin = new System.Windows.Forms.Padding(0);
            this.LootingFarmingManagementPanelName.Name = "LootingFarmingManagementPanelName";
            this.LootingFarmingManagementPanelName.OrderIndex = 3;
            this.LootingFarmingManagementPanelName.Padding = new System.Windows.Forms.Padding(0, 0, 0, 12);
            this.LootingFarmingManagementPanelName.Size = new System.Drawing.Size(556, 36);
            this.LootingFarmingManagementPanelName.TabIndex = 5;
            this.LootingFarmingManagementPanelName.TitleFont = new System.Drawing.Font("Segoe UI", 7.65F, System.Drawing.FontStyle.Bold);
            this.LootingFarmingManagementPanelName.TitleForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.LootingFarmingManagementPanelName.TitleText = "Looting/Farming Management";
            this.LootingFarmingManagementPanelName.UnfolderImage = ((System.Drawing.Image)(resources.GetObject("LootingFarmingManagementPanelName.UnfolderImage")));
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
            // labelX69
            // 
            this.labelX69.BackColor = System.Drawing.Color.Transparent;
            this.labelX69.ForeColor = System.Drawing.Color.Black;
            this.labelX69.Location = new System.Drawing.Point(4, 496);
            this.labelX69.Name = "labelX69";
            this.labelX69.Size = new System.Drawing.Size(154, 22);
            this.labelX69.TabIndex = 59;
            this.labelX69.Text = "Milling only in town";
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
            // labelX70
            // 
            this.labelX70.BackColor = System.Drawing.Color.Transparent;
            this.labelX70.ForeColor = System.Drawing.Color.Black;
            this.labelX70.Location = new System.Drawing.Point(4, 468);
            this.labelX70.Name = "labelX70";
            this.labelX70.Size = new System.Drawing.Size(154, 22);
            this.labelX70.TabIndex = 56;
            this.labelX70.Text = "Milling Every (in minute)";
            // 
            // labelX71
            // 
            this.labelX71.BackColor = System.Drawing.Color.Transparent;
            this.labelX71.ForeColor = System.Drawing.Color.Black;
            this.labelX71.Location = new System.Drawing.Point(4, 440);
            this.labelX71.Name = "labelX71";
            this.labelX71.Size = new System.Drawing.Size(154, 22);
            this.labelX71.TabIndex = 55;
            this.labelX71.Text = "Milling";
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
            // labelX72
            // 
            this.labelX72.BackColor = System.Drawing.Color.Transparent;
            this.labelX72.ForeColor = System.Drawing.Color.Black;
            this.labelX72.Location = new System.Drawing.Point(290, 413);
            this.labelX72.Name = "labelX72";
            this.labelX72.Size = new System.Drawing.Size(204, 22);
            this.labelX72.TabIndex = 52;
            this.labelX72.Text = "Milling list (one item by line)";
            // 
            // labelX68
            // 
            this.labelX68.BackColor = System.Drawing.Color.Transparent;
            this.labelX68.ForeColor = System.Drawing.Color.Black;
            this.labelX68.Location = new System.Drawing.Point(4, 519);
            this.labelX68.Name = "labelX68";
            this.labelX68.Size = new System.Drawing.Size(154, 22);
            this.labelX68.TabIndex = 51;
            this.labelX68.Text = "Auto Make Elemental";
            // 
            // MakeStackOfElementalsItems
            // 
            this.MakeStackOfElementalsItems.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.MakeStackOfElementalsItems.Location = new System.Drawing.Point(163, 519);
            this.MakeStackOfElementalsItems.MaximumSize = new System.Drawing.Size(60, 20);
            this.MakeStackOfElementalsItems.MinimumSize = new System.Drawing.Size(60, 20);
            this.MakeStackOfElementalsItems.Name = "MakeStackOfElementalsItems";
            this.MakeStackOfElementalsItems.OffText = "OFF";
            this.MakeStackOfElementalsItems.OnText = "ON";
            this.MakeStackOfElementalsItems.Size = new System.Drawing.Size(60, 20);
            this.MakeStackOfElementalsItems.TabIndex = 71;
            this.MakeStackOfElementalsItems.Value = false;
            // 
            // labelX65
            // 
            this.labelX65.BackColor = System.Drawing.Color.Transparent;
            this.labelX65.ForeColor = System.Drawing.Color.Black;
            this.labelX65.Location = new System.Drawing.Point(4, 382);
            this.labelX65.Name = "labelX65";
            this.labelX65.Size = new System.Drawing.Size(154, 22);
            this.labelX65.TabIndex = 49;
            this.labelX65.Text = "Prospecting only in town";
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
            this.TimeBetweenEachProspectingAttempt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(229)))), ((int)(((byte)(230)))));
            this.TimeBetweenEachProspectingAttempt.ForeColor = System.Drawing.Color.Black;
            this.TimeBetweenEachProspectingAttempt.Location = new System.Drawing.Point(163, 354);
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
            // labelX64
            // 
            this.labelX64.BackColor = System.Drawing.Color.Transparent;
            this.labelX64.ForeColor = System.Drawing.Color.Black;
            this.labelX64.Location = new System.Drawing.Point(4, 354);
            this.labelX64.Name = "labelX64";
            this.labelX64.Size = new System.Drawing.Size(154, 22);
            this.labelX64.TabIndex = 46;
            this.labelX64.Text = "Prospecting Every (in minute)";
            // 
            // labelX63
            // 
            this.labelX63.BackColor = System.Drawing.Color.Transparent;
            this.labelX63.ForeColor = System.Drawing.Color.Black;
            this.labelX63.Location = new System.Drawing.Point(4, 326);
            this.labelX63.Name = "labelX63";
            this.labelX63.Size = new System.Drawing.Size(154, 22);
            this.labelX63.TabIndex = 45;
            this.labelX63.Text = "Prospecting";
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
            // labelX62
            // 
            this.labelX62.BackColor = System.Drawing.Color.Transparent;
            this.labelX62.ForeColor = System.Drawing.Color.Black;
            this.labelX62.Location = new System.Drawing.Point(290, 298);
            this.labelX62.Name = "labelX62";
            this.labelX62.Size = new System.Drawing.Size(204, 22);
            this.labelX62.TabIndex = 42;
            this.labelX62.Text = "Prospecting list (one item by line)";
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
            // labelX36
            // 
            this.labelX36.BackColor = System.Drawing.Color.Transparent;
            this.labelX36.ForeColor = System.Drawing.Color.Black;
            this.labelX36.Location = new System.Drawing.Point(289, 160);
            this.labelX36.Name = "labelX36";
            this.labelX36.Size = new System.Drawing.Size(209, 22);
            this.labelX36.TabIndex = 38;
            this.labelX36.Text = "Don\'t harvest List (one id per line)";
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
            // labelX35
            // 
            this.labelX35.BackColor = System.Drawing.Color.Transparent;
            this.labelX35.ForeColor = System.Drawing.Color.Black;
            this.labelX35.Location = new System.Drawing.Point(4, 272);
            this.labelX35.Name = "labelX35";
            this.labelX35.Size = new System.Drawing.Size(154, 22);
            this.labelX35.TabIndex = 34;
            this.labelX35.Text = "Harvest During Long Move";
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
            5,
            0,
            0,
            0});
            this.GatheringSearchRadius.Name = "GatheringSearchRadius";
            this.GatheringSearchRadius.Size = new System.Drawing.Size(77, 22);
            this.GatheringSearchRadius.TabIndex = 30;
            this.GatheringSearchRadius.Value = new decimal(new int[] {
            35,
            0,
            0,
            0});
            // 
            // labelX22
            // 
            this.labelX22.BackColor = System.Drawing.Color.Transparent;
            this.labelX22.ForeColor = System.Drawing.Color.Black;
            this.labelX22.Location = new System.Drawing.Point(4, 244);
            this.labelX22.Name = "labelX22";
            this.labelX22.Size = new System.Drawing.Size(154, 22);
            this.labelX22.TabIndex = 29;
            this.labelX22.Text = "Search Radius";
            // 
            // DontHarvestIfMoreThanXUnitInAggroRange
            // 
            this.DontHarvestIfMoreThanXUnitInAggroRange.ForeColor = System.Drawing.Color.Black;
            this.DontHarvestIfMoreThanXUnitInAggroRange.Location = new System.Drawing.Point(163, 216);
            this.DontHarvestIfMoreThanXUnitInAggroRange.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.DontHarvestIfMoreThanXUnitInAggroRange.Name = "DontHarvestIfMoreThanXUnitInAggroRange";
            this.DontHarvestIfMoreThanXUnitInAggroRange.Size = new System.Drawing.Size(77, 22);
            this.DontHarvestIfMoreThanXUnitInAggroRange.TabIndex = 28;
            this.DontHarvestIfMoreThanXUnitInAggroRange.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // labelX21
            // 
            this.labelX21.BackColor = System.Drawing.Color.Transparent;
            this.labelX21.ForeColor = System.Drawing.Color.Black;
            this.labelX21.Location = new System.Drawing.Point(4, 216);
            this.labelX21.Name = "labelX21";
            this.labelX21.Size = new System.Drawing.Size(154, 22);
            this.labelX21.TabIndex = 27;
            this.labelX21.Text = "Max Units Near";
            // 
            // labelX20
            // 
            this.labelX20.BackColor = System.Drawing.Color.Transparent;
            this.labelX20.ForeColor = System.Drawing.Color.Black;
            this.labelX20.Location = new System.Drawing.Point(4, 160);
            this.labelX20.Name = "labelX20";
            this.labelX20.Size = new System.Drawing.Size(154, 22);
            this.labelX20.TabIndex = 26;
            this.labelX20.Text = "Harvest Herbs";
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
            // labelX19
            // 
            this.labelX19.BackColor = System.Drawing.Color.Transparent;
            this.labelX19.ForeColor = System.Drawing.Color.Black;
            this.labelX19.Location = new System.Drawing.Point(4, 132);
            this.labelX19.Name = "labelX19";
            this.labelX19.Size = new System.Drawing.Size(154, 22);
            this.labelX19.TabIndex = 24;
            this.labelX19.Text = "Harvest Minerals";
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
            // labelX17
            // 
            this.labelX17.BackColor = System.Drawing.Color.Transparent;
            this.labelX17.ForeColor = System.Drawing.Color.Black;
            this.labelX17.Location = new System.Drawing.Point(4, 104);
            this.labelX17.Name = "labelX17";
            this.labelX17.Size = new System.Drawing.Size(154, 22);
            this.labelX17.TabIndex = 22;
            this.labelX17.Text = "Skin Mobs";
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
            // labelX12
            // 
            this.labelX12.BackColor = System.Drawing.Color.Transparent;
            this.labelX12.ForeColor = System.Drawing.Color.Black;
            this.labelX12.Location = new System.Drawing.Point(4, 188);
            this.labelX12.Name = "labelX12";
            this.labelX12.Size = new System.Drawing.Size(154, 22);
            this.labelX12.TabIndex = 16;
            this.labelX12.Text = "Harvest Avoid Players Radius";
            // 
            // labelX18
            // 
            this.labelX18.BackColor = System.Drawing.Color.Transparent;
            this.labelX18.ForeColor = System.Drawing.Color.Black;
            this.labelX18.Location = new System.Drawing.Point(4, 49);
            this.labelX18.Name = "labelX18";
            this.labelX18.Size = new System.Drawing.Size(154, 22);
            this.labelX18.TabIndex = 11;
            this.labelX18.Text = "Loot Mobs";
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
            this.RegenerationManagementPanelName.Controls.Add(this.labelX15);
            this.RegenerationManagementPanelName.Controls.Add(this.labelX14);
            this.RegenerationManagementPanelName.Controls.Add(this.labelX13);
            this.RegenerationManagementPanelName.Controls.Add(this.EatFoodWhenHealthIsUnderXPercent);
            this.RegenerationManagementPanelName.Controls.Add(this.FoodName);
            this.RegenerationManagementPanelName.Controls.Add(this.labelX11);
            this.RegenerationManagementPanelName.Fold = true;
            this.RegenerationManagementPanelName.FolderImage = ((System.Drawing.Image)(resources.GetObject("RegenerationManagementPanelName.FolderImage")));
            this.RegenerationManagementPanelName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.RegenerationManagementPanelName.HeaderBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.RegenerationManagementPanelName.HeaderImage = ((System.Drawing.Image)(resources.GetObject("RegenerationManagementPanelName.HeaderImage")));
            this.RegenerationManagementPanelName.HeaderSize = new System.Drawing.Size(556, 36);
            this.RegenerationManagementPanelName.Location = new System.Drawing.Point(0, 72);
            this.RegenerationManagementPanelName.Margin = new System.Windows.Forms.Padding(0);
            this.RegenerationManagementPanelName.Name = "RegenerationManagementPanelName";
            this.RegenerationManagementPanelName.OrderIndex = 2;
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
            // labelX15
            // 
            this.labelX15.BackColor = System.Drawing.Color.Transparent;
            this.labelX15.ForeColor = System.Drawing.Color.Black;
            this.labelX15.Location = new System.Drawing.Point(3, 74);
            this.labelX15.Name = "labelX15";
            this.labelX15.Size = new System.Drawing.Size(154, 22);
            this.labelX15.TabIndex = 21;
            this.labelX15.Text = "Drink";
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
            // labelX11
            // 
            this.labelX11.BackColor = System.Drawing.Color.Transparent;
            this.labelX11.ForeColor = System.Drawing.Color.Black;
            this.labelX11.Location = new System.Drawing.Point(3, 46);
            this.labelX11.Name = "labelX11";
            this.labelX11.Size = new System.Drawing.Size(154, 22);
            this.labelX11.TabIndex = 12;
            this.labelX11.Text = "Food";
            // 
            // MountManagementPanelName
            // 
            this.MountManagementPanelName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.MountManagementPanelName.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.MountManagementPanelName.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.MountManagementPanelName.ContentSize = new System.Drawing.Size(556, 144);
            this.MountManagementPanelName.Controls.Add(this.AquaticMountName);
            this.MountManagementPanelName.Controls.Add(this.labelX66);
            this.MountManagementPanelName.Controls.Add(this.labelX57);
            this.MountManagementPanelName.Controls.Add(this.IgnoreFightIfMounted);
            this.MountManagementPanelName.Controls.Add(this.MinimumDistanceToUseMount);
            this.MountManagementPanelName.Controls.Add(this.labelX8);
            this.MountManagementPanelName.Controls.Add(this.FlyingMountName);
            this.MountManagementPanelName.Controls.Add(this.labelX7);
            this.MountManagementPanelName.Controls.Add(this.GroundMountName);
            this.MountManagementPanelName.Controls.Add(this.labelX6);
            this.MountManagementPanelName.Controls.Add(this.labelX5);
            this.MountManagementPanelName.Controls.Add(this.UseGroundMount);
            this.MountManagementPanelName.Fold = true;
            this.MountManagementPanelName.FolderImage = ((System.Drawing.Image)(resources.GetObject("MountManagementPanelName.FolderImage")));
            this.MountManagementPanelName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.MountManagementPanelName.HeaderBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.MountManagementPanelName.HeaderImage = ((System.Drawing.Image)(resources.GetObject("MountManagementPanelName.HeaderImage")));
            this.MountManagementPanelName.HeaderSize = new System.Drawing.Size(556, 36);
            this.MountManagementPanelName.Location = new System.Drawing.Point(0, 36);
            this.MountManagementPanelName.Margin = new System.Windows.Forms.Padding(0);
            this.MountManagementPanelName.Name = "MountManagementPanelName";
            this.MountManagementPanelName.OrderIndex = 1;
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
            this.AquaticMountName.Location = new System.Drawing.Point(161, 138);
            this.AquaticMountName.Name = "AquaticMountName";
            this.AquaticMountName.Size = new System.Drawing.Size(144, 22);
            this.AquaticMountName.TabIndex = 22;
            // 
            // labelX66
            // 
            this.labelX66.BackColor = System.Drawing.Color.Transparent;
            this.labelX66.ForeColor = System.Drawing.Color.Black;
            this.labelX66.Location = new System.Drawing.Point(2, 136);
            this.labelX66.Name = "labelX66";
            this.labelX66.Size = new System.Drawing.Size(154, 22);
            this.labelX66.TabIndex = 21;
            this.labelX66.Text = "Aquatic";
            // 
            // labelX57
            // 
            this.labelX57.BackColor = System.Drawing.Color.Transparent;
            this.labelX57.ForeColor = System.Drawing.Color.Black;
            this.labelX57.Location = new System.Drawing.Point(316, 85);
            this.labelX57.Name = "labelX57";
            this.labelX57.Size = new System.Drawing.Size(154, 22);
            this.labelX57.TabIndex = 20;
            this.labelX57.Text = "Ignore Fight if in Gound Mount";
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
            // labelX8
            // 
            this.labelX8.BackColor = System.Drawing.Color.Transparent;
            this.labelX8.ForeColor = System.Drawing.Color.Black;
            this.labelX8.Location = new System.Drawing.Point(2, 79);
            this.labelX8.Name = "labelX8";
            this.labelX8.Size = new System.Drawing.Size(154, 22);
            this.labelX8.TabIndex = 16;
            this.labelX8.Text = "Mount Distance";
            // 
            // FlyingMountName
            // 
            this.FlyingMountName.ForeColor = System.Drawing.Color.Black;
            this.FlyingMountName.Location = new System.Drawing.Point(161, 111);
            this.FlyingMountName.Name = "FlyingMountName";
            this.FlyingMountName.Size = new System.Drawing.Size(144, 22);
            this.FlyingMountName.TabIndex = 15;
            // 
            // labelX7
            // 
            this.labelX7.BackColor = System.Drawing.Color.Transparent;
            this.labelX7.ForeColor = System.Drawing.Color.Black;
            this.labelX7.Location = new System.Drawing.Point(2, 108);
            this.labelX7.Name = "labelX7";
            this.labelX7.Size = new System.Drawing.Size(154, 22);
            this.labelX7.TabIndex = 14;
            this.labelX7.Text = "Flying";
            // 
            // GroundMountName
            // 
            this.GroundMountName.ForeColor = System.Drawing.Color.Black;
            this.GroundMountName.Location = new System.Drawing.Point(161, 54);
            this.GroundMountName.Name = "GroundMountName";
            this.GroundMountName.Size = new System.Drawing.Size(144, 22);
            this.GroundMountName.TabIndex = 13;
            // 
            // labelX6
            // 
            this.labelX6.BackColor = System.Drawing.Color.Transparent;
            this.labelX6.ForeColor = System.Drawing.Color.Black;
            this.labelX6.Location = new System.Drawing.Point(2, 51);
            this.labelX6.Name = "labelX6";
            this.labelX6.Size = new System.Drawing.Size(154, 22);
            this.labelX6.TabIndex = 12;
            this.labelX6.Text = "Ground";
            // 
            // labelX5
            // 
            this.labelX5.BackColor = System.Drawing.Color.Transparent;
            this.labelX5.ForeColor = System.Drawing.Color.Black;
            this.labelX5.Location = new System.Drawing.Point(316, 54);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(154, 22);
            this.labelX5.TabIndex = 11;
            this.labelX5.Text = "Use Ground Mount";
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
            this.SpellManagementSystemPanelName.Controls.Add(this.labelX59);
            this.SpellManagementSystemPanelName.Controls.Add(this.UseSpiritHealer);
            this.SpellManagementSystemPanelName.Controls.Add(this.ActivateSkillsAutoTrainingLabel);
            this.SpellManagementSystemPanelName.Controls.Add(this.ActivateSkillsAutoTraining);
            this.SpellManagementSystemPanelName.Controls.Add(this.labelX4);
            this.SpellManagementSystemPanelName.Controls.Add(this.DontPullMonsters);
            this.SpellManagementSystemPanelName.Controls.Add(this.labelX3);
            this.SpellManagementSystemPanelName.Controls.Add(this.CanPullUnitsAlreadyInFight);
            this.SpellManagementSystemPanelName.Controls.Add(this.labelX2);
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
            this.SpellManagementSystemPanelName.Name = "SpellManagementSystemPanelName";
            this.SpellManagementSystemPanelName.OrderIndex = 0;
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
            // labelX59
            // 
            this.labelX59.BackColor = System.Drawing.Color.Transparent;
            this.labelX59.ForeColor = System.Drawing.Color.Black;
            this.labelX59.Location = new System.Drawing.Point(11, 123);
            this.labelX59.Name = "labelX59";
            this.labelX59.Size = new System.Drawing.Size(154, 22);
            this.labelX59.TabIndex = 28;
            this.labelX59.Text = "Use Spirit Healer";
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
            // labelX4
            // 
            this.labelX4.BackColor = System.Drawing.Color.Transparent;
            this.labelX4.ForeColor = System.Drawing.Color.Black;
            this.labelX4.Location = new System.Drawing.Point(300, 151);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(154, 22);
            this.labelX4.TabIndex = 9;
            this.labelX4.Text = "Don\'t start fighting";
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
            // labelX3
            // 
            this.labelX3.BackColor = System.Drawing.Color.Transparent;
            this.labelX3.ForeColor = System.Drawing.Color.Black;
            this.labelX3.Location = new System.Drawing.Point(300, 123);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(154, 22);
            this.labelX3.TabIndex = 7;
            this.labelX3.Text = "Can attack units already in fight";
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
            // labelX2
            // 
            this.labelX2.BackColor = System.Drawing.Color.Transparent;
            this.labelX2.ForeColor = System.Drawing.Color.Black;
            this.labelX2.Location = new System.Drawing.Point(11, 151);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(154, 22);
            this.labelX2.TabIndex = 5;
            this.labelX2.Text = "Assign Talents";
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
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
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
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "General Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GeneralSettings_FormClosing);
            this.MainPanel.ResumeLayout(false);
            this.MimesisBroadcasterSettingsPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.BroadcastingPort)).EndInit();
            this.AdvancedSettingsPanelName.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MaxDistanceToGoToMailboxesOrNPCs)).EndInit();
            this.SecuritySystemPanelName.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.StopTNBAfterXMinutes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StopTNBAfterXStucks)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StopTNBIfReceivedAtMostXWhispers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StopTNBAfterXLevelup)).EndInit();
            this.MailsManagementPanelName.ResumeLayout(false);
            this.MailsManagementPanelName.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SendMailWhenLessThanXSlotLeft)).EndInit();
            this.NPCsRepairSellBuyPanelName.ResumeLayout(false);
            this.NPCsRepairSellBuyPanelName.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SellItemsWhenLessThanXSlotLeft)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RepairWhenDurabilityIsUnderPercent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumberOfFoodsWeGot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumberOfBeverageWeGot)).EndInit();
            this.ReloggerManagementPanelName.ResumeLayout(false);
            this.ReloggerManagementPanelName.PerformLayout();
            this.LootingFarmingManagementPanelName.ResumeLayout(false);
            this.LootingFarmingManagementPanelName.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TimeBetweenEachMillingAttempt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TimeBetweenEachProspectingAttempt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GatheringSearchRadius)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DontHarvestIfMoreThanXUnitInAggroRange)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DontHarvestIfPlayerNearRadius)).EndInit();
            this.RegenerationManagementPanelName.ResumeLayout(false);
            this.RegenerationManagementPanelName.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DrinkBeverageWhenManaIsUnderXPercent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EatFoodWhenHealthIsUnderXPercent)).EndInit();
            this.MountManagementPanelName.ResumeLayout(false);
            this.MountManagementPanelName.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MinimumDistanceToUseMount)).EndInit();
            this.SpellManagementSystemPanelName.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TnbRibbonManager MainPanel;
        private TnbExpendablePanel MountManagementPanelName;
        private TnbExpendablePanel SpellManagementSystemPanelName;
        private Label labelX8;
        private TextBox FlyingMountName;
        private Label labelX7;
        private TextBox GroundMountName;
        private Label labelX6;
        private Label labelX5;
        private TnbSwitchButton UseGroundMount;
        private Label labelX4;
        private TnbSwitchButton DontPullMonsters;
        private Label labelX3;
        private TnbSwitchButton CanPullUnitsAlreadyInFight;
        private Label labelX2;
        private TnbSwitchButton AutoAssignTalents;
        private Label CombatClassLabel;
        private TnbComboBox CombatClass;
        private nManager.Helpful.Forms.UserControls.TnbButton CombatClassSettingsButton;
        private TnbExpendablePanel LootingFarmingManagementPanelName;
        private NumericUpDown DontHarvestIfPlayerNearRadius;
        private Label labelX12;
        private Label labelX18;
        private TnbSwitchButton ActivateMonsterLooting;
        private TnbExpendablePanel RegenerationManagementPanelName;
        private Label labelX9;
        private Label labelX10;
        private NumericUpDown DrinkBeverageWhenManaIsUnderXPercent;
        private TextBox BeverageName;
        private Label labelX15;
        private Label labelX14;
        private Label labelX13;
        private NumericUpDown EatFoodWhenHealthIsUnderXPercent;
        private TextBox FoodName;
        private Label labelX11;
        private NumericUpDown MinimumDistanceToUseMount;
        private Label labelX23;
        private TnbSwitchButton BeastNinjaSkinning;
        private NumericUpDown GatheringSearchRadius;
        private Label labelX22;
        private NumericUpDown DontHarvestIfMoreThanXUnitInAggroRange;
        private Label labelX21;
        private Label labelX20;
        private TnbSwitchButton ActivateHerbsHarvesting;
        private Label labelX19;
        private TnbSwitchButton ActivateVeinsHarvesting;
        private Label labelX17;
        private TnbSwitchButton ActivateBeastSkinning;
        private Label labelX16;
        private TnbSwitchButton ActivateChestLooting;
        private TnbExpendablePanel ReloggerManagementPanelName;
        private TextBox PasswordOfTheBattleNetAccount;
        private Label labelX37;
        private TextBox EmailOfTheBattleNetAccount;
        private Label labelX40;
        private Label labelX38;
        private TnbSwitchButton ActivateReloggerFeature;
        private TnbExpendablePanel NPCsRepairSellBuyPanelName;
        private NumericUpDown NumberOfBeverageWeGot;
        private Label labelX41;
        private TextBox DontSellTheseItems;
        private Label labelX46;
        private TnbExpendablePanel MailsManagementPanelName;
        private TextBox MaillingFeatureRecipient;
        private Label labelX56;
        private TextBox MaillingFeatureSubject;
        private TextBox ForceToMailTheseItems;
        private Label labelX48;
        private CheckBox MailPurple;
        private CheckBox MailBlue;
        private CheckBox MailGreen;
        private CheckBox MailWhite;
        private CheckBox MailGray;
        private Label labelX54;
        private Label labelX55;
        private TnbSwitchButton ActivateAutoMaillingFeature;
        private TextBox DontMailTheseItems;
        private Label labelX58;
        private TextBox ForceToSellTheseItems;
        private Label labelX53;
        private CheckBox SellPurple;
        private CheckBox SellBlue;
        private CheckBox SellGreen;
        private CheckBox SellWhite;
        private CheckBox SellGray;
        private Label labelX52;
        private TnbSwitchButton ActivateAutoSellingFeature;
        private Label labelX51;
        private TnbSwitchButton ActivateAutoRepairFeature;
        private NumericUpDown NumberOfFoodsWeGot;
        private Label labelX50;
        private Label ActivateSkillsAutoTrainingLabel;
        private TnbSwitchButton ActivateSkillsAutoTraining;
        private TnbExpendablePanel AdvancedSettingsPanelName;
        private Label labelX42;
        private TnbSwitchButton ActivatePathFindingFeature;
        private TnbExpendablePanel SecuritySystemPanelName;
        private Label labelX39;
        private Label labelX45;
        private TnbSwitchButton PlayASongIfNewWhispReceived;
        private Label labelX34;
        private Label labelX33;
        private Label labelX43;
        private Label labelX32;
        private Label labelX31;
        private TnbSwitchButton RecordWhispsInLogFiles;
        private Label labelX29;
        private TnbSwitchButton StopTNBIfPlayerHaveBeenTeleported;
        private Label labelX44;
        private Label labelX30;
        private TnbSwitchButton PauseTNBIfNearByPlayer;
        private TnbSwitchButton StopTNBIfHonorPointsLimitReached;
        private NumericUpDown StopTNBAfterXMinutes;
        private Label labelX28;
        private NumericUpDown StopTNBAfterXStucks;
        private Label labelX26;
        private NumericUpDown StopTNBIfReceivedAtMostXWhispers;
        private Label labelX25;
        private NumericUpDown StopTNBAfterXLevelup;
        private Label labelX24;
        private Label labelX27;
        private TnbSwitchButton StopTNBIfBagAreFull;
        private nManager.Helpful.Forms.UserControls.TnbButton saveAndCloseB;
        private nManager.Helpful.Forms.UserControls.TnbButton resetB;
        private nManager.Helpful.Forms.UserControls.TnbButton closeB;
        private Label labelX57;
        private TnbSwitchButton IgnoreFightIfMounted;
        private Label labelX59;
        private TnbSwitchButton UseSpiritHealer;
        private NumericUpDown MaxDistanceToGoToMailboxesOrNPCs;
        private Label labelX60;
        private Label labelX35;
        private TnbSwitchButton HarvestDuringLongDistanceMovements;
        private nManager.Helpful.Forms.UserControls.TnbButton addBlackListHarvest;
        private Label labelX36;
        private Label labelX61;
        private TnbSwitchButton ActivateAutoSmelting;
        private TnbSwitchButton DoRegenManaIfLow;
        private TextBox AquaticMountName;
        private Label labelX66;
        private TextBox BattleNetSubAccount;
        private Label labelX67;
        private Label labelX68;
        private TnbSwitchButton MakeStackOfElementalsItems;
        private System.Windows.Forms.ToolTip labelsToolTip;
        private Label labelX69;
        private TnbSwitchButton OnlyUseMillingInTown;
        private NumericUpDown TimeBetweenEachMillingAttempt;
        private Label labelX70;
        private Label labelX71;
        private TnbSwitchButton ActivateAutoMilling;
        private TextBox HerbsToBeMilled;
        private Label labelX72;
        private Label labelX65;
        private TnbSwitchButton OnlyUseProspectingInTown;
        private NumericUpDown TimeBetweenEachProspectingAttempt;
        private Label labelX64;
        private Label labelX63;
        private TnbSwitchButton ActivateAutoProspecting;
        private TextBox MineralsToProspect;
        private Label labelX62;
        private Label labelX73;
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
    }
}