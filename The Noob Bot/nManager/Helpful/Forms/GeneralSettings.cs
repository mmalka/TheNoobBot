using System;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using nManager.Plugins;
using nManager.Wow.Bot.Tasks;

namespace nManager.Helpful.Forms
{
    public partial class GeneralSettings : Form
    {
        public GeneralSettings()
        {
            try
            {
                InitializeComponent();
                TranslateForm();
                if (nManagerSetting.CurrentSetting.ActivateAlwaysOnTopFeature)
                    TopMost = true;
                Plugins.Plugins.DisposePlugins();
                foreach (string f in Others.GetFilesDirectory(Application.StartupPath + "\\CombatClasses\\", "*.dll"))
                {
                    CombatClass.Items.Add(f);
                }
                foreach (string f in Others.GetFilesDirectory(Application.StartupPath + "\\CombatClasses\\", "*.cs"))
                {
                    CombatClass.Items.Add(f);
                }
                foreach (string f in Others.GetFilesDirectory(Application.StartupPath + "\\HealerClasses\\", "*.dll"))
                {
                    HealerClass.Items.Add(f);
                }
                foreach (string f in Others.GetFilesDirectory(Application.StartupPath + "\\HealerClasses\\", "*.cs"))
                {
                    HealerClass.Items.Add(f);
                }
                foreach (string f in Others.GetFilesDirectory(Application.StartupPath + "\\Plugins\\", "*.dll"))
                {
                    AvailablePluginsList.Items.Add(f);
                }
                foreach (string f in Others.GetFilesDirectory(Application.StartupPath + "\\Plugins\\", "*.cs"))
                {
                    AvailablePluginsList.Items.Add(f);
                }
                LoadSetting(nManagerSetting.CurrentSetting);
                IPAddress firstInterfaceLanIPv4 = Dns.GetHostAddresses(Dns.GetHostName()).FirstOrDefault(test => test.AddressFamily == AddressFamily.InterNetwork);
                BroadcastingIPLan.Text = firstInterfaceLanIPv4 != null ? firstInterfaceLanIPv4.ToString() : "No Lan IPv4 found";
                BroadcastingIPWan.Text = Others.GetClientIPAddress;
            }
            catch (Exception e)
            {
                Logging.WriteError("GeneralSettings > GeneralSettings(): " + e);
            }
        }

        private void SetToolTypeIfNeeded(Control label)
        {
            using (Graphics g = CreateGraphics())
            {
                SizeF size = g.MeasureString(label.Text, label.Font);
                if (size.Width > label.Width)
                {
                    labelsToolTip.SetToolTip(label, label.Text);
                }
            }
        }

        private void TranslateForm()
        {
            string onText = Translate.Get(Translate.Id.YES);
            string offText = Translate.Get(Translate.Id.NO);
            SecuritySystemLabel.Text = string.Format("{0}:", Translate.Get(Translate.Id.Security));
            SetToolTypeIfNeeded(SecuritySystemLabel);
            CloseGameLabel.Text = string.Format("{0}:", Translate.Get(Translate.Id.Close_game));
            SetToolTypeIfNeeded(CloseGameLabel);
            StopTNBIfHonorPointsLimitReachedLabel.Text = Translate.Get(Translate.Id.If_reached______Honor_Points);
            SetToolTypeIfNeeded(StopTNBIfHonorPointsLimitReachedLabel);
            StopTNBAfterXMinutesLabel.Text = Translate.Get(Translate.Id.After);
            SetToolTypeIfNeeded(StopTNBAfterXMinutesLabel);
            StopTNBAfterXStucksLabel.Text = Translate.Get(Translate.Id.After);
            SetToolTypeIfNeeded(StopTNBAfterXStucksLabel);
            StopTNBAfterXLevelupLabel.Text = Translate.Get(Translate.Id.After);
            SetToolTypeIfNeeded(StopTNBAfterXLevelupLabel);
            addBlackListHarvest.Text = Translate.Get(Translate.Id.Add);
            SetToolTypeIfNeeded(addBlackListHarvest);
            AdvancedSettingsPanelName.TitleText = Translate.Get(Translate.Id.AdvancedSettingsPanelName);
            SetToolTypeIfNeeded(AdvancedSettingsPanelName);
            MaxDistanceToGoToMailboxesOrNPCsLabel.Text = Translate.Get(Translate.Id.Npc_Mailbox_Search_Radius);
            SetToolTypeIfNeeded(MaxDistanceToGoToMailboxesOrNPCsLabel);
            ActivatePathFindingFeatureLabel.Text = Translate.Get(Translate.Id.Use_Paths_Finder);
            SetToolTypeIfNeeded(ActivatePathFindingFeatureLabel);
            SecuritySystemPanelName.TitleText = Translate.Get(Translate.Id.SecuritySystemPanelName);
            SetToolTypeIfNeeded(SecuritySystemPanelName);
            labelX33.Text = Translate.Get(Translate.Id.Min);
            SetToolTypeIfNeeded(labelX33);
            RecordWhispsInLogFilesLabel.Text = Translate.Get(Translate.Id.Record_whisper_in_Log_file);
            SetToolTypeIfNeeded(RecordWhispsInLogFilesLabel);
            labelX32.Text = Translate.Get(Translate.Id.Blockages);
            SetToolTypeIfNeeded(labelX32);
            labelX31.Text = Translate.Get(Translate.Id.Level);
            SetToolTypeIfNeeded(labelX31);
            StopTNBIfPlayerHaveBeenTeleportedLabel.Text = Translate.Get(Translate.Id.If_Player_Teleported);
            SetToolTypeIfNeeded(StopTNBIfPlayerHaveBeenTeleportedLabel);
            PauseTNBIfNearByPlayerLabel.Text = Translate.Get(Translate.Id.Pause_tnb_if_Nearby_Player);
            SetToolTypeIfNeeded(PauseTNBIfNearByPlayerLabel);
            StopTNBIfReceivedAtMostXWhispersLabel.Text = Translate.Get(Translate.Id.If_Whisper_bigger_or_equal_to);
            SetToolTypeIfNeeded(StopTNBIfReceivedAtMostXWhispersLabel);
            StopTNBIfBagAreFullLabel.Text = Translate.Get(Translate.Id.If_full_Bag);
            SetToolTypeIfNeeded(StopTNBIfBagAreFullLabel);
            MailsManagementPanelName.TitleText = Translate.Get(Translate.Id.MailsManagementPanelName);
            SetToolTypeIfNeeded(MailsManagementPanelName);
            MaillingFeatureRecipientLabel.Text = Translate.Get(Translate.Id.Mail_Recipient);
            SetToolTypeIfNeeded(MaillingFeatureRecipientLabel);
            ForceToMailTheseItemsLabel.Text = Translate.Get(Translate.Id.Force_Mail_List__one_item_by_line);
            SetToolTypeIfNeeded(ForceToMailTheseItemsLabel);
            MailPurple.Text = Translate.Get(Translate.Id.Mail_Purple_items);
            SetToolTypeIfNeeded(MailPurple);
            MailBlue.Text = Translate.Get(Translate.Id.Mail_Blue_items);
            SetToolTypeIfNeeded(MailBlue);
            MailGreen.Text = Translate.Get(Translate.Id.Mail_Green_items);
            SetToolTypeIfNeeded(MailGreen);
            MailWhite.Text = Translate.Get(Translate.Id.Mail_White_items);
            SetToolTypeIfNeeded(MailWhite);
            MailGray.Text = Translate.Get(Translate.Id.Mail_Gray_items);
            SetToolTypeIfNeeded(MailGray);
            MaillingFeatureSubjectLabel.Text = Translate.Get(Translate.Id.Subject);
            SetToolTypeIfNeeded(MaillingFeatureSubjectLabel);
            ActivateAutoMaillingFeatureLabel.Text = Translate.Get(Translate.Id.Use_Mail);
            SetToolTypeIfNeeded(ActivateAutoMaillingFeatureLabel);
            DontMailTheseItemsLabel.Text = Translate.Get(Translate.Id.Do_not_Mail_List__one_item_by_line);
            SetToolTypeIfNeeded(DontMailTheseItemsLabel);
            NPCsRepairSellBuyPanelName.TitleText = Translate.Get(Translate.Id.NPCsRepairSellBuyPanelName);
            SetToolTypeIfNeeded(NPCsRepairSellBuyPanelName);
            ForceToSellTheseItemsLabel.Text = Translate.Get(Translate.Id.Force_Sell_List__one_item_by_line);
            SetToolTypeIfNeeded(ForceToSellTheseItemsLabel);
            SellPurple.Text = Translate.Get(Translate.Id.Sell_Purple_items);
            SetToolTypeIfNeeded(SellPurple);
            SellBlue.Text = Translate.Get(Translate.Id.Sell_Blue_items);
            SetToolTypeIfNeeded(SellBlue);
            SellGreen.Text = Translate.Get(Translate.Id.Sell_Green_items);
            SetToolTypeIfNeeded(SellGreen);
            SellWhite.Text = Translate.Get(Translate.Id.Sell_White_items);
            SetToolTypeIfNeeded(SellWhite);
            SellGray.Text = Translate.Get(Translate.Id.Sell_Gray_items);
            SetToolTypeIfNeeded(SellGray);
            labelX52.Text = Translate.Get(Translate.Id.Selling);
            SetToolTypeIfNeeded(labelX52);
            ActivateAutoRepairFeatureLabel.Text = Translate.Get(Translate.Id.Repair);
            SetToolTypeIfNeeded(ActivateAutoRepairFeatureLabel);
            NumberOfFoodsWeGotLabel.Text = Translate.Get(Translate.Id.Food_Amount);
            SetToolTypeIfNeeded(NumberOfFoodsWeGotLabel);
            NumberOfBeverageWeGotLabel.Text = Translate.Get(Translate.Id.Drink_Amount);
            SetToolTypeIfNeeded(NumberOfBeverageWeGotLabel);
            DontSellTheseItemsLabel.Text = Translate.Get(Translate.Id.Do_not_Sell_List__one_item_by_line);
            SetToolTypeIfNeeded(DontSellTheseItemsLabel);
            ReloggerManagementPanelName.TitleText = Translate.Get(Translate.Id.ReloggerManagementPanelName);
            SetToolTypeIfNeeded(ReloggerManagementPanelName);
            BattleNetSubAccountLabel.Text = Translate.Get(Translate.Id.BattleNet_Account);
            SetToolTypeIfNeeded(BattleNetSubAccountLabel);
            ActivateReloggerFeatureLabel.Text = Translate.Get(Translate.Id.Relogger);
            SetToolTypeIfNeeded(ActivateReloggerFeatureLabel);
            PasswordOfTheBattleNetAccountLabel.Text = Translate.Get(Translate.Id.Account_Password);
            SetToolTypeIfNeeded(PasswordOfTheBattleNetAccountLabel);
            EmailOfTheBattleNetAccountLabel.Text = Translate.Get(Translate.Id.Account_Email);
            SetToolTypeIfNeeded(EmailOfTheBattleNetAccountLabel);
            LootingFarmingManagementPanelName.TitleText = Translate.Get(Translate.Id.LootingFarmingManagementPanelName);
            SetToolTypeIfNeeded(LootingFarmingManagementPanelName);
            OnlyUseProspectingInTownLabel.Text = Translate.Get(Translate.Id.Prospecting_only_in_town);
            SetToolTypeIfNeeded(OnlyUseProspectingInTownLabel);
            TimeBetweenEachProspectingAttemptLabel.Text = Translate.Get(Translate.Id.Prospecting_Every__in_minute);
            SetToolTypeIfNeeded(TimeBetweenEachProspectingAttemptLabel);
            ActivateAutoProspectingLabel.Text = Translate.Get(Translate.Id.Prospecting);
            SetToolTypeIfNeeded(ActivateAutoProspectingLabel);
            MineralsToProspectLabel.Text = Translate.Get(Translate.Id.Prospecting_list__one_item_by_line);
            SetToolTypeIfNeeded(MineralsToProspectLabel);
            OnlyUseMillingInTownLabel.Text = Translate.Get(Translate.Id.Milling_only_in_town);
            SetToolTypeIfNeeded(OnlyUseMillingInTownLabel);
            TimeBetweenEachMillingAttemptLabel.Text = Translate.Get(Translate.Id.Milling_Every__in_minute);
            SetToolTypeIfNeeded(TimeBetweenEachMillingAttemptLabel);
            ActivateAutoMillingLabel.Text = Translate.Get(Translate.Id.Milling);
            SetToolTypeIfNeeded(ActivateAutoMillingLabel);
            HerbsToBeMilledLabel.Text = Translate.Get(Translate.Id.Milling_list__one_item_by_line);
            SetToolTypeIfNeeded(HerbsToBeMilledLabel);
            labelX61.Text = Translate.Get(Translate.Id.Smelting);
            SetToolTypeIfNeeded(labelX61);
            DontHarvestTheFollowingObjectsLabel.Text = Translate.Get(Translate.Id.Don_t_harvest);
            SetToolTypeIfNeeded(DontHarvestTheFollowingObjectsLabel);
            HarvestDuringLongDistanceMovementsLabel.Text = Translate.Get(Translate.Id.Harvest_During_Long_Move);
            SetToolTypeIfNeeded(HarvestDuringLongDistanceMovementsLabel);
            labelX23.Text = Translate.Get(Translate.Id.Ninja);
            SetToolTypeIfNeeded(labelX23);
            GatheringSearchRadiusLabel.Text = Translate.Get(Translate.Id.Search_Radius);
            SetToolTypeIfNeeded(GatheringSearchRadiusLabel);
            DontHarvestIfMoreThanXUnitInAggroRangeLabel.Text = Translate.Get(Translate.Id.Max_Units_Near);
            SetToolTypeIfNeeded(DontHarvestIfMoreThanXUnitInAggroRangeLabel);
            ActivateHerbsHarvestingLabel.Text = Translate.Get(Translate.Id.Harvest_Herbs);
            SetToolTypeIfNeeded(ActivateHerbsHarvestingLabel);
            ActivateVeinsHarvestingLabel.Text = Translate.Get(Translate.Id.Harvest_Minerals);
            SetToolTypeIfNeeded(ActivateVeinsHarvestingLabel);
            ActivateBeastSkinningLabel.Text = Translate.Get(Translate.Id.Skin_Mobs);
            SetToolTypeIfNeeded(ActivateBeastSkinningLabel);
            labelX16.Text = Translate.Get(Translate.Id.Loot_Chests);
            SetToolTypeIfNeeded(labelX16);
            DontHarvestIfPlayerNearRadiusLabel.Text = Translate.Get(Translate.Id.Harvest_Avoid_Players_Radius);
            SetToolTypeIfNeeded(DontHarvestIfPlayerNearRadiusLabel);
            ActivateMonsterLootingLabel.Text = Translate.Get(Translate.Id.Loot_Mobs);
            SetToolTypeIfNeeded(ActivateMonsterLootingLabel);
            ActivateLootStatisticsLabel.Text = Translate.Get(Translate.Id.ActivateLootStatistics);
            SetToolTypeIfNeeded(ActivateLootStatisticsLabel);
            RegenerationManagementPanelName.TitleText = Translate.Get(Translate.Id.RegenerationManagementPanelName);
            SetToolTypeIfNeeded(RegenerationManagementPanelName);
            labelX10.Text = Translate.Get(Translate.Id.on);
            SetToolTypeIfNeeded(labelX10);
            labelX13.Text = Translate.Get(Translate.Id.on);
            SetToolTypeIfNeeded(labelX13);
            BeverageNameLabel.Text = Translate.Get(Translate.Id.Drink);
            SetToolTypeIfNeeded(BeverageNameLabel);
            FoodNameLabel.Text = Translate.Get(Translate.Id.Food);
            SetToolTypeIfNeeded(FoodNameLabel);
            MountManagementPanelName.TitleText = Translate.Get(Translate.Id.MountManagementPanelName);
            SetToolTypeIfNeeded(MountManagementPanelName);
            AquaticMountNameLabel.Text = Translate.Get(Translate.Id.Aquatic);
            SetToolTypeIfNeeded(AquaticMountNameLabel);
            IgnoreFightIfMountedLabel.Text = Translate.Get(Translate.Id.Ignore_Fight_if_in_Gound_Mount);
            SetToolTypeIfNeeded(IgnoreFightIfMountedLabel);
            MinimumDistanceToUseMountLabel.Text = Translate.Get(Translate.Id.Mount_Distance);
            SetToolTypeIfNeeded(MinimumDistanceToUseMountLabel);
            FlyingMountNameLabel.Text = Translate.Get(Translate.Id.Flying);
            SetToolTypeIfNeeded(FlyingMountNameLabel);
            GroundMountNameLabel.Text = Translate.Get(Translate.Id.Ground);
            SetToolTypeIfNeeded(GroundMountNameLabel);
            UseGroundMountLabel.Text = Translate.Get(Translate.Id.Use_Ground_Mount);
            SetToolTypeIfNeeded(UseGroundMountLabel);
            SpellManagementSystemPanelName.TitleText = Translate.Get(Translate.Id.SpellManagementSystemPanelName);
            SetToolTypeIfNeeded(SpellManagementSystemPanelName);
            UseSpiritHealerLabel.Text = Translate.Get(Translate.Id.Use_Spirit_Healer);
            SetToolTypeIfNeeded(UseSpiritHealerLabel);
            ActivateSkillsAutoTrainingLabel.Text = Translate.Get(Translate.Id.ActivateSkillsAutoTraining);
            SetToolTypeIfNeeded(ActivateSkillsAutoTrainingLabel);
            OnlyTrainCurrentlyUsedSkillsLabel.Text = Translate.Get(Translate.Id.OnlyTrainCurrentlyUsedSkills);
            SetToolTypeIfNeeded(OnlyTrainCurrentlyUsedSkillsLabel);
            TrainMountingCapacityLabel.Text = Translate.Get(Translate.Id.TrainMountingCapacity);
            SetToolTypeIfNeeded(TrainMountingCapacityLabel);
            OnlyTrainIfWeHave2TimesMoreMoneyThanOurWishListSumLabel.Text = Translate.Get(Translate.Id.OnlyTrainIfWeHave2TimesMoreMoneyThanOurWishListSum);
            SetToolTypeIfNeeded(OnlyTrainIfWeHave2TimesMoreMoneyThanOurWishListSumLabel);
            BecomeApprenticeIfNeededByProductLabel.Text = Translate.Get(Translate.Id.BecomeApprenticeIfNeededByProduct);
            SetToolTypeIfNeeded(BecomeApprenticeIfNeededByProductLabel);
            BecomeApprenticeOfSecondarySkillsWhileQuestingLabel.Text = Translate.Get(Translate.Id.BecomeApprenticeOfSecondarySkillsWhileQuesting);
            SetToolTypeIfNeeded(BecomeApprenticeOfSecondarySkillsWhileQuestingLabel);
            DontPullMonstersLabel.Text = Translate.Get(Translate.Id.Don_t_start_fighting);
            SetToolTypeIfNeeded(DontPullMonstersLabel);
            CanPullUnitsAlreadyInFightLabel.Text = Translate.Get(Translate.Id.Can_attack_units_already_in_fight);
            SetToolTypeIfNeeded(CanPullUnitsAlreadyInFightLabel);
            AutoAssignTalentsLabel.Text = Translate.Get(Translate.Id.Assign_Talents);
            SetToolTypeIfNeeded(AutoAssignTalentsLabel);
            CombatClassLabel.Text = Translate.Get(Translate.Id.CombatClass);
            SetToolTypeIfNeeded(CombatClassLabel);
            HealerClassLabel.Text = Translate.Get(Translate.Id.HealerClass);
            SetToolTypeIfNeeded(HealerClassLabel);
            CombatClassSettingsButton.Text = Translate.Get(Translate.Id.Settings);
            SetToolTypeIfNeeded(CombatClassSettingsButton);
            HealerClassSettingsButton.Text = Translate.Get(Translate.Id.Settings);
            SetToolTypeIfNeeded(HealerClassSettingsButton);
            CombatClassResetSettingsButton.Text = Translate.Get(Translate.Id.ResetSettings);
            SetToolTypeIfNeeded(CombatClassResetSettingsButton);
            HealerClassResetSettingsButton.Text = Translate.Get(Translate.Id.ResetSettings);
            SetToolTypeIfNeeded(HealerClassResetSettingsButton);
            closeB.Text = Translate.Get(Translate.Id.CloseWithoutSaving);
            SetToolTypeIfNeeded(closeB);
            resetB.Text = Translate.Get(Translate.Id.Reset_Settings);
            SetToolTypeIfNeeded(resetB);
            saveAndCloseB.Text = Translate.Get(Translate.Id.Save_and_Close);
            SetToolTypeIfNeeded(saveAndCloseB);
            MakeStackOfElementalsItemsLabel.Text = Translate.Get(Translate.Id.Auto_Make_Elemental);
            SetToolTypeIfNeeded(MakeStackOfElementalsItemsLabel);
            MainHeader.TitleText = Translate.Get(Translate.Id.General_Settings) + " - " + Information.MainTitle;
            AllowTNBToSetYourMaxFpsLabel.Text = Translate.Get(Translate.Id.Uncap_MaxFPS);
            SetToolTypeIfNeeded(AllowTNBToSetYourMaxFpsLabel);
            AutoConfirmOnBoPItemsLabel.Text = Translate.Get(Translate.Id.AutoConfirmOnBoPItems);
            SetToolTypeIfNeeded(AutoConfirmOnBoPItemsLabel);
            AlwaysOnTopFeatureLabel.Text = Translate.Get(Translate.Id.AlwaysOnTop);
            SetToolTypeIfNeeded(AlwaysOnTopFeatureLabel);
            SendMailWhenLessThanXSlotLeftLabel.Text = Translate.Get(Translate.Id.SendMailWhenLessThanXSlotLeft);
            SetToolTypeIfNeeded(SendMailWhenLessThanXSlotLeftLabel);
            SellItemsWhenLessThanXSlotLeftLabel.Text = Translate.Get(Translate.Id.SellItemsWhenLessThanXSlotLeft);
            SetToolTypeIfNeeded(SellItemsWhenLessThanXSlotLeftLabel);
            RepairWhenDurabilityIsUnderPercentLabel.Text = Translate.Get(Translate.Id.RepairWhenDurabilityIsUnderPercent);
            SetToolTypeIfNeeded(RepairWhenDurabilityIsUnderPercentLabel);
            UseHearthstoneLabel.Text = Translate.Get(Translate.Id.UseHearthstone);
            SetToolTypeIfNeeded(UseHearthstoneLabel);
            UseMollELabel.Text = Translate.Get(Translate.Id.UseMollE);
            SetToolTypeIfNeeded(UseMollELabel);
            UseRobotLabel.Text = Translate.Get(Translate.Id.UseRobot);
            SetToolTypeIfNeeded(UseRobotLabel);
            MimesisBroadcasterSettingsPanel.TitleText = Translate.Get(Translate.Id.MimesisBroadcasterSettings);
            SetToolTypeIfNeeded(MimesisBroadcasterSettingsPanel);
            AutoCloseChatFrameLabel.Text = Translate.Get(Translate.Id.AutoCloseChatFrame);
            SetToolTypeIfNeeded(AutoCloseChatFrameLabel);
            BroadcastingPortDefaultLabel.Text = Translate.Get(Translate.Id.BroadcastingPortDefault);
            SetToolTypeIfNeeded(BroadcastingPortDefaultLabel);
            BroadcastingIPWanLabel.Text = Translate.Get(Translate.Id.BroadcastingIPWan);
            SetToolTypeIfNeeded(BroadcastingIPWanLabel);
            BroadcastingIPLanLabel.Text = Translate.Get(Translate.Id.BroadcastingIPLan);
            SetToolTypeIfNeeded(BroadcastingIPLanLabel);
            ActivateBroadcastingMimesisLabel.Text = Translate.Get(Translate.Id.ActivateBroadcastingMimesis);
            SetToolTypeIfNeeded(ActivateBroadcastingMimesisLabel);
            BroadcastingIPLocalLabel.Text = Translate.Get(Translate.Id.BroadcastingIPLocal);
            SetToolTypeIfNeeded(BroadcastingIPLocalLabel);
            BroadcastingPortLabel.Text = Translate.Get(Translate.Id.BroadcastingPort);
            SetToolTypeIfNeeded(BroadcastingPortLabel);
            ActivateAlwaysOnTopFeature.OffText = offText;
            ActivateAlwaysOnTopFeature.OnText = onText;
            AllowTNBToSetYourMaxFps.OffText = offText;
            AllowTNBToSetYourMaxFps.OnText = onText;
            ActivatePathFindingFeature.OffText = offText;
            ActivatePathFindingFeature.OnText = onText;
            ActiveStopTNBAfterXMinutes.OffText = offText;
            ActiveStopTNBAfterXMinutes.OnText = onText;
            ActiveStopTNBAfterXStucks.OffText = offText;
            ActiveStopTNBAfterXStucks.OnText = onText;
            ActiveStopTNBIfReceivedAtMostXWhispers.OffText = offText;
            ActiveStopTNBIfReceivedAtMostXWhispers.OnText = onText;
            ActiveStopTNBAfterXLevelup.OffText = offText;
            ActiveStopTNBAfterXLevelup.OnText = onText;
            UseHearthstone.OffText = offText;
            UseHearthstone.OnText = onText;
            PlayASongIfNewWhispReceived.OffText = offText;
            PlayASongIfNewWhispReceived.OnText = onText;
            RecordWhispsInLogFiles.OffText = offText;
            RecordWhispsInLogFiles.OnText = onText;
            StopTNBIfPlayerHaveBeenTeleported.OffText = offText;
            StopTNBIfPlayerHaveBeenTeleported.OnText = onText;
            PauseTNBIfNearByPlayer.OffText = offText;
            PauseTNBIfNearByPlayer.OnText = onText;
            StopTNBIfHonorPointsLimitReached.OffText = offText;
            StopTNBIfHonorPointsLimitReached.OnText = onText;
            StopTNBIfBagAreFull.OffText = offText;
            StopTNBIfBagAreFull.OnText = onText;
            UseMollE.OffText = offText;
            UseMollE.OnText = onText;
            ActivateAutoMaillingFeature.OffText = offText;
            ActivateAutoMaillingFeature.OnText = onText;
            UseRobot.OffText = offText;
            UseRobot.OnText = onText;
            ActivateAutoSellingFeature.OffText = offText;
            ActivateAutoSellingFeature.OnText = onText;
            ActivateAutoRepairFeature.OffText = offText;
            ActivateAutoRepairFeature.OnText = onText;
            AutoConfirmOnBoPItems.OffText = offText;
            AutoConfirmOnBoPItems.OnText = onText;
            OnlyUseMillingInTown.OffText = offText;
            OnlyUseMillingInTown.OnText = onText;
            ActivateAutoMilling.OffText = offText;
            ActivateAutoMilling.OnText = onText;
            MakeStackOfElementalsItems.OffText = offText;
            MakeStackOfElementalsItems.OnText = onText;
            OnlyUseProspectingInTown.OffText = offText;
            OnlyUseProspectingInTown.OnText = onText;
            ActivateAutoProspecting.OffText = offText;
            ActivateAutoProspecting.OnText = onText;
            ActivateAutoSmelting.OffText = offText;
            ActivateAutoSmelting.OnText = onText;
            HarvestDuringLongDistanceMovements.OffText = offText;
            HarvestDuringLongDistanceMovements.OnText = onText;
            BeastNinjaSkinning.OffText = offText;
            BeastNinjaSkinning.OnText = onText;
            ActivateHerbsHarvesting.OffText = offText;
            ActivateHerbsHarvesting.OnText = onText;
            ActivateVeinsHarvesting.OffText = offText;
            ActivateVeinsHarvesting.OnText = onText;
            ActivateBeastSkinning.OffText = offText;
            ActivateBeastSkinning.OnText = onText;
            ActivateChestLooting.OffText = offText;
            ActivateChestLooting.OnText = onText;
            ActivateMonsterLooting.OffText = offText;
            ActivateMonsterLooting.OnText = onText;
            ActivateLootStatistics.OffText = offText;
            ActivateLootStatistics.OnText = onText;
            DoRegenManaIfLow.OffText = offText;
            DoRegenManaIfLow.OnText = onText;
            IgnoreFightIfMounted.OffText = offText;
            IgnoreFightIfMounted.OnText = onText;
            UseGroundMount.OffText = offText;
            UseGroundMount.OnText = onText;
            UseSpiritHealer.OffText = offText;
            UseSpiritHealer.OnText = onText;
            ActivateSkillsAutoTraining.OffText = offText;
            ActivateSkillsAutoTraining.OnText = onText;
            OnlyTrainCurrentlyUsedSkills.OffText = offText;
            OnlyTrainCurrentlyUsedSkills.OnText = onText;
            TrainMountingCapacity.OffText = offText;
            TrainMountingCapacity.OnText = onText;
            OnlyTrainIfWeHave2TimesMoreMoneyThanOurWishListSum.OffText = offText;
            OnlyTrainIfWeHave2TimesMoreMoneyThanOurWishListSum.OnText = onText;
            BecomeApprenticeIfNeededByProduct.OffText = offText;
            BecomeApprenticeIfNeededByProduct.OnText = onText;
            BecomeApprenticeOfSecondarySkillsWhileQuesting.OffText = offText;
            BecomeApprenticeOfSecondarySkillsWhileQuesting.OnText = onText;
            DontPullMonsters.OffText = offText;
            DontPullMonsters.OnText = onText;
            CanPullUnitsAlreadyInFight.OffText = offText;
            CanPullUnitsAlreadyInFight.OnText = onText;
            AutoAssignTalents.OffText = offText;
            AutoAssignTalents.OnText = onText;
            ActivateBroadcastingMimesis.OffText = offText;
            ActivateBroadcastingMimesis.OnText = onText;
            AutoCloseChatFrame.OffText = offText;
            AutoCloseChatFrame.OnText = onText;
            ActivatePluginsSystem.OffText = offText;
            ActivatePluginsSystem.OnText = onText;
            LaunchExpiredPlugins.OffText = offText;
            LaunchExpiredPlugins.OnText = onText;
        }

        private void SaveSetting()
        {
            try
            {
                nManagerSetting.CurrentSetting.CombatClass = CombatClass.Text;
                nManagerSetting.CurrentSetting.HealerClass = HealerClass.Text;
                nManagerSetting.CurrentSetting.AutoAssignTalents = AutoAssignTalents.Value;
                nManagerSetting.CurrentSetting.ActivateSkillsAutoTraining = ActivateSkillsAutoTraining.Value;
                nManagerSetting.CurrentSetting.OnlyTrainCurrentlyUsedSkills = OnlyTrainCurrentlyUsedSkills.Value;
                nManagerSetting.CurrentSetting.TrainMountingCapacity = TrainMountingCapacity.Value;
                nManagerSetting.CurrentSetting.OnlyTrainIfWeHave2TimesMoreMoneyThanOurWishListSum = OnlyTrainIfWeHave2TimesMoreMoneyThanOurWishListSum.Value;
                nManagerSetting.CurrentSetting.BecomeApprenticeIfNeededByProduct = BecomeApprenticeIfNeededByProduct.Value;
                nManagerSetting.CurrentSetting.BecomeApprenticeOfSecondarySkillsWhileQuesting = BecomeApprenticeOfSecondarySkillsWhileQuesting.Value;
                nManagerSetting.CurrentSetting.CanPullUnitsAlreadyInFight = CanPullUnitsAlreadyInFight.Value;
                nManagerSetting.CurrentSetting.DontPullMonsters = DontPullMonsters.Value;
                nManagerSetting.CurrentSetting.UseSpiritHealer = UseSpiritHealer.Value;
                nManagerSetting.CurrentSetting.UseGroundMount = UseGroundMount.Value;
                nManagerSetting.CurrentSetting.GroundMountName = GroundMountName.Text;
                nManagerSetting.CurrentSetting.MinimumDistanceToUseMount = (uint) MinimumDistanceToUseMount.Value;
                nManagerSetting.CurrentSetting.IgnoreFightIfMounted = IgnoreFightIfMounted.Value;
                nManagerSetting.CurrentSetting.FlyingMountName = FlyingMountName.Text;
                nManagerSetting.CurrentSetting.AquaticMountName = AquaticMountName.Text;
                nManagerSetting.CurrentSetting.FoodName = FoodName.Text;
                nManagerSetting.CurrentSetting.EatFoodWhenHealthIsUnderXPercent = (int) EatFoodWhenHealthIsUnderXPercent.Value;
                nManagerSetting.CurrentSetting.BeverageName = BeverageName.Text;
                nManagerSetting.CurrentSetting.DrinkBeverageWhenManaIsUnderXPercent = (int) DrinkBeverageWhenManaIsUnderXPercent.Value;
                nManagerSetting.CurrentSetting.DoRegenManaIfLow = DoRegenManaIfLow.Value;
                nManagerSetting.CurrentSetting.ActivateMonsterLooting = ActivateMonsterLooting.Value;
                nManagerSetting.CurrentSetting.ActivateChestLooting = ActivateChestLooting.Value;
                nManagerSetting.CurrentSetting.ActivateBeastSkinning = ActivateBeastSkinning.Value;
                nManagerSetting.CurrentSetting.BeastNinjaSkinning = BeastNinjaSkinning.Value;
                nManagerSetting.CurrentSetting.ActivateVeinsHarvesting = ActivateVeinsHarvesting.Value;
                nManagerSetting.CurrentSetting.ActivateHerbsHarvesting = ActivateHerbsHarvesting.Value;
                nManagerSetting.CurrentSetting.DontHarvestIfPlayerNearRadius = (int) DontHarvestIfPlayerNearRadius.Value;
                nManagerSetting.CurrentSetting.DontHarvestIfMoreThanXUnitInAggroRange = (int) DontHarvestIfMoreThanXUnitInAggroRange.Value;
                nManagerSetting.CurrentSetting.GatheringSearchRadius = (int) GatheringSearchRadius.Value;
                nManagerSetting.CurrentSetting.HarvestDuringLongDistanceMovements = HarvestDuringLongDistanceMovements.Value;
                nManagerSetting.CurrentSetting.ActivateAutoSmelting = ActivateAutoSmelting.Value;
                nManagerSetting.CurrentSetting.ActivateAutoProspecting = ActivateAutoProspecting.Value;
                nManagerSetting.CurrentSetting.TimeBetweenEachProspectingAttempt = (int) TimeBetweenEachProspectingAttempt.Value;
                nManagerSetting.CurrentSetting.OnlyUseProspectingInTown = OnlyUseProspectingInTown.Value;
                nManagerSetting.CurrentSetting.MineralsToProspect.Clear();
                nManagerSetting.CurrentSetting.MineralsToProspect.AddRange(Others.TextToArrayByLine(MineralsToProspect.Text));
                nManagerSetting.CurrentSetting.ActivateAutoMilling = ActivateAutoMilling.Value;
                nManagerSetting.CurrentSetting.TimeBetweenEachMillingAttempt = (int) TimeBetweenEachMillingAttempt.Value;
                nManagerSetting.CurrentSetting.OnlyUseMillingInTown = OnlyUseMillingInTown.Value;
                nManagerSetting.CurrentSetting.HerbsToBeMilled.Clear();
                nManagerSetting.CurrentSetting.HerbsToBeMilled.AddRange(Others.TextToArrayByLine(HerbsToBeMilled.Text));
                nManagerSetting.CurrentSetting.DontHarvestTheFollowingObjects.Clear();
                nManagerSetting.CurrentSetting.DontHarvestTheFollowingObjects.AddRange(Others.TextToArrayByLine(DontHarvestTheFollowingObjects.Text));
                nManagerSetting.CurrentSetting.MakeStackOfElementalsItems = MakeStackOfElementalsItems.Value;
                nManagerSetting.CurrentSetting.ActivateReloggerFeature = ActivateReloggerFeature.Value;
                nManagerSetting.CurrentSetting.EmailOfTheBattleNetAccount = EmailOfTheBattleNetAccount.Text;
                nManagerSetting.CurrentSetting.PasswordOfTheBattleNetAccount = PasswordOfTheBattleNetAccount.Text;
                nManagerSetting.CurrentSetting.BattleNetSubAccount = BattleNetSubAccount.Text;
                nManagerSetting.CurrentSetting.NumberOfFoodsWeGot = (int) NumberOfFoodsWeGot.Value;
                nManagerSetting.CurrentSetting.NumberOfBeverageWeGot = (int) NumberOfBeverageWeGot.Value;
                nManagerSetting.CurrentSetting.ActivateAutoRepairFeature = ActivateAutoRepairFeature.Value;
                nManagerSetting.CurrentSetting.ActivateAutoSellingFeature = ActivateAutoSellingFeature.Value;
                nManagerSetting.CurrentSetting.SellGray = SellGray.Checked;
                nManagerSetting.CurrentSetting.SellWhite = SellWhite.Checked;
                nManagerSetting.CurrentSetting.SellGreen = SellGreen.Checked;
                nManagerSetting.CurrentSetting.SellBlue = SellBlue.Checked;
                nManagerSetting.CurrentSetting.SellPurple = SellPurple.Checked;
                nManagerSetting.CurrentSetting.DontSellTheseItems.Clear();
                nManagerSetting.CurrentSetting.DontSellTheseItems.AddRange(Others.TextToArrayByLine(DontSellTheseItems.Text));
                nManagerSetting.CurrentSetting.ForceToSellTheseItems.Clear();
                nManagerSetting.CurrentSetting.ForceToSellTheseItems.AddRange(Others.TextToArrayByLine(ForceToSellTheseItems.Text));
                nManagerSetting.CurrentSetting.ActivateAutoMaillingFeature = ActivateAutoMaillingFeature.Value;
                nManagerSetting.CurrentSetting.MaillingFeatureRecipient = MaillingFeatureRecipient.Text;
                nManagerSetting.CurrentSetting.MaillingFeatureSubject = MaillingFeatureSubject.Text;
                nManagerSetting.CurrentSetting.MailGray = MailGray.Checked;
                nManagerSetting.CurrentSetting.MailWhite = MailWhite.Checked;
                nManagerSetting.CurrentSetting.MailGreen = MailGreen.Checked;
                nManagerSetting.CurrentSetting.MailBlue = MailBlue.Checked;
                nManagerSetting.CurrentSetting.MailPurple = MailPurple.Checked;
                nManagerSetting.CurrentSetting.ForceToMailTheseItems.Clear();
                nManagerSetting.CurrentSetting.ForceToMailTheseItems.AddRange(Others.TextToArrayByLine(ForceToMailTheseItems.Text));
                nManagerSetting.CurrentSetting.DontMailTheseItems.Clear();
                nManagerSetting.CurrentSetting.DontMailTheseItems.AddRange(Others.TextToArrayByLine(DontMailTheseItems.Text));
                nManagerSetting.CurrentSetting.StopTNBIfBagAreFull = StopTNBIfBagAreFull.Value;
                nManagerSetting.CurrentSetting.StopTNBIfHonorPointsLimitReached = StopTNBIfHonorPointsLimitReached.Value;
                nManagerSetting.CurrentSetting.StopTNBIfPlayerHaveBeenTeleported = StopTNBIfPlayerHaveBeenTeleported.Value;
                nManagerSetting.CurrentSetting.StopTNBAfterXLevelup = (int) StopTNBAfterXLevelup.Value;
                nManagerSetting.CurrentSetting.StopTNBIfReceivedAtMostXWhispers = (int) StopTNBIfReceivedAtMostXWhispers.Value;
                nManagerSetting.CurrentSetting.StopTNBAfterXStucks = (int) StopTNBAfterXStucks.Value;
                nManagerSetting.CurrentSetting.StopTNBAfterXMinutes = (int) StopTNBAfterXMinutes.Value;
                nManagerSetting.CurrentSetting.PauseTNBIfNearByPlayer = PauseTNBIfNearByPlayer.Value;
                nManagerSetting.CurrentSetting.RecordWhispsInLogFiles = RecordWhispsInLogFiles.Value;
                nManagerSetting.CurrentSetting.PlayASongIfNewWhispReceived = PlayASongIfNewWhispReceived.Value;
                nManagerSetting.CurrentSetting.ActivatePathFindingFeature = ActivatePathFindingFeature.Value;
                nManagerSetting.CurrentSetting.AllowTNBToSetYourMaxFps = AllowTNBToSetYourMaxFps.Value;
                nManagerSetting.CurrentSetting.MaxDistanceToGoToMailboxesOrNPCs = (int) MaxDistanceToGoToMailboxesOrNPCs.Value;
                nManagerSetting.CurrentSetting.AutoConfirmOnBoPItems = AutoConfirmOnBoPItems.Value;
                nManagerSetting.CurrentSetting.ActivateAlwaysOnTopFeature = ActivateAlwaysOnTopFeature.Value;
                nManagerSetting.CurrentSetting.RepairWhenDurabilityIsUnderPercent = (int) RepairWhenDurabilityIsUnderPercent.Value;
                nManagerSetting.CurrentSetting.SellItemsWhenLessThanXSlotLeft = (int) SellItemsWhenLessThanXSlotLeft.Value;
                nManagerSetting.CurrentSetting.SendMailWhenLessThanXSlotLeft = (int) SendMailWhenLessThanXSlotLeft.Value;
                nManagerSetting.CurrentSetting.UseHearthstone = UseHearthstone.Value;
                nManagerSetting.CurrentSetting.ActiveStopTNBAfterXLevelup = ActiveStopTNBAfterXLevelup.Value;
                nManagerSetting.CurrentSetting.ActiveStopTNBAfterXMinutes = ActiveStopTNBAfterXMinutes.Value;
                nManagerSetting.CurrentSetting.ActiveStopTNBAfterXStucks = ActiveStopTNBAfterXStucks.Value;
                nManagerSetting.CurrentSetting.ActiveStopTNBIfReceivedAtMostXWhispers = ActiveStopTNBIfReceivedAtMostXWhispers.Value;
                nManagerSetting.CurrentSetting.UseMollE = UseMollE.Value;
                nManagerSetting.CurrentSetting.UseRobot = UseRobot.Value;
                nManagerSetting.CurrentSetting.AutoCloseChatFrame = AutoCloseChatFrame.Value;
                if (nManagerSetting.CurrentSetting.ActivateLootStatistics && !ActivateLootStatistics.Value)
                    Others.LootStatistics(false);
                else if (!nManagerSetting.CurrentSetting.ActivateLootStatistics && ActivateLootStatistics.Value)
                    Others.LootStatistics();
                nManagerSetting.CurrentSetting.ActivateLootStatistics = ActivateLootStatistics.Value;
                int oldPort = nManagerSetting.CurrentSetting.BroadcastingPort;
                bool oldStatus = nManagerSetting.CurrentSetting.ActivateBroadcastingMimesis;
                nManagerSetting.CurrentSetting.ActivateBroadcastingMimesis = ActivateBroadcastingMimesis.Value;
                nManagerSetting.CurrentSetting.BroadcastingPort = (int) BroadcastingPort.Value;
                nManagerSetting.CurrentSetting.ActivatePluginsSystem = ActivatePluginsSystem.Value;
                nManagerSetting.CurrentSetting.LaunchExpiredPlugins = LaunchExpiredPlugins.Value;
                nManagerSetting.CurrentSetting.ActivatedPluginsList.Clear();
                foreach (string s in ActivatedPluginsList.Items)
                {
                    nManagerSetting.CurrentSetting.ActivatedPluginsList.Add(s);
                }
                if (nManagerSetting.CurrentSetting.ActivatePluginsSystem)
                    Plugins.Plugins.ReLoadPlugins();
                nManagerSetting.CurrentSetting.Save();
                if (oldStatus && !ActivateBroadcastingMimesis.Value)
                    Communication.Shutdown(oldPort); // Display the port used before the settings edition.
                else if (!oldStatus && ActivateBroadcastingMimesis.Value)
                    Communication.Listen();
                else if (oldStatus && ActivateBroadcastingMimesis.Value && BroadcastingPort.Value != oldPort)
                {
                    Communication.Shutdown(oldPort); // Display the port used before the settings edition.
                    Communication.Listen();
                }
                MountTask.SettingsHasChanged = true;
            }
            catch (Exception e)
            {
                Logging.WriteError("GeneralSettings > SaveSetting(): " + e);
            }
        }

        private void LoadSetting(nManagerSetting managerSetting)
        {
            try
            {
                CombatClass.Text = managerSetting.CombatClass;
                HealerClass.Text = managerSetting.HealerClass;
                AutoAssignTalents.Value = managerSetting.AutoAssignTalents;
                ActivateSkillsAutoTraining.Value = managerSetting.ActivateSkillsAutoTraining;
                OnlyTrainCurrentlyUsedSkills.Value = managerSetting.OnlyTrainCurrentlyUsedSkills;
                TrainMountingCapacity.Value = managerSetting.TrainMountingCapacity;
                OnlyTrainIfWeHave2TimesMoreMoneyThanOurWishListSum.Value = managerSetting.OnlyTrainIfWeHave2TimesMoreMoneyThanOurWishListSum;
                BecomeApprenticeIfNeededByProduct.Value = managerSetting.BecomeApprenticeIfNeededByProduct;
                BecomeApprenticeOfSecondarySkillsWhileQuesting.Value = managerSetting.BecomeApprenticeOfSecondarySkillsWhileQuesting;
                CanPullUnitsAlreadyInFight.Value = managerSetting.CanPullUnitsAlreadyInFight;
                DontPullMonsters.Value = managerSetting.DontPullMonsters;
                UseSpiritHealer.Value = managerSetting.UseSpiritHealer;
                UseGroundMount.Value = managerSetting.UseGroundMount;
                GroundMountName.Text = managerSetting.GroundMountName;
                MinimumDistanceToUseMount.Value = (int) managerSetting.MinimumDistanceToUseMount;
                IgnoreFightIfMounted.Value = managerSetting.IgnoreFightIfMounted;
                FlyingMountName.Text = managerSetting.FlyingMountName;
                AquaticMountName.Text = managerSetting.AquaticMountName;
                FoodName.Text = managerSetting.FoodName;
                EatFoodWhenHealthIsUnderXPercent.Value = managerSetting.EatFoodWhenHealthIsUnderXPercent;
                BeverageName.Text = managerSetting.BeverageName;
                DrinkBeverageWhenManaIsUnderXPercent.Value = managerSetting.DrinkBeverageWhenManaIsUnderXPercent;
                DoRegenManaIfLow.Value = managerSetting.DoRegenManaIfLow;
                ActivateMonsterLooting.Value = managerSetting.ActivateMonsterLooting;
                ActivateLootStatistics.Value = managerSetting.ActivateLootStatistics;
                ActivateChestLooting.Value = managerSetting.ActivateChestLooting;
                ActivateBeastSkinning.Value = managerSetting.ActivateBeastSkinning;
                BeastNinjaSkinning.Value = managerSetting.BeastNinjaSkinning;
                ActivateVeinsHarvesting.Value = managerSetting.ActivateVeinsHarvesting;
                ActivateHerbsHarvesting.Value = managerSetting.ActivateHerbsHarvesting;
                DontHarvestIfPlayerNearRadius.Value = (decimal) managerSetting.DontHarvestIfPlayerNearRadius;
                DontHarvestIfMoreThanXUnitInAggroRange.Value = managerSetting.DontHarvestIfMoreThanXUnitInAggroRange;
                GatheringSearchRadius.Value = (decimal) managerSetting.GatheringSearchRadius;
                HarvestDuringLongDistanceMovements.Value = managerSetting.HarvestDuringLongDistanceMovements;
                ActivateAutoSmelting.Value = managerSetting.ActivateAutoSmelting;
                ActivateAutoProspecting.Value = managerSetting.ActivateAutoProspecting;
                OnlyUseProspectingInTown.Value = managerSetting.OnlyUseProspectingInTown;
                TimeBetweenEachProspectingAttempt.Value = managerSetting.TimeBetweenEachProspectingAttempt;
                MineralsToProspect.Text = Others.ArrayToTextByLine(managerSetting.MineralsToProspect.ToArray());
                ActivateAutoMilling.Value = managerSetting.ActivateAutoMilling;
                OnlyUseMillingInTown.Value = managerSetting.OnlyUseMillingInTown;
                TimeBetweenEachMillingAttempt.Value = managerSetting.TimeBetweenEachMillingAttempt;
                HerbsToBeMilled.Text = Others.ArrayToTextByLine(managerSetting.HerbsToBeMilled.ToArray());
                DontHarvestTheFollowingObjects.Text = Others.ArrayToTextByLine(managerSetting.DontHarvestTheFollowingObjects.ToArray());
                MakeStackOfElementalsItems.Value = managerSetting.MakeStackOfElementalsItems;
                ActivateReloggerFeature.Value = managerSetting.ActivateReloggerFeature;
                EmailOfTheBattleNetAccount.Text = managerSetting.EmailOfTheBattleNetAccount;
                PasswordOfTheBattleNetAccount.Text = managerSetting.PasswordOfTheBattleNetAccount;
                BattleNetSubAccount.Text = managerSetting.BattleNetSubAccount;
                NumberOfFoodsWeGot.Value = managerSetting.NumberOfFoodsWeGot;
                NumberOfBeverageWeGot.Value = managerSetting.NumberOfBeverageWeGot;
                ActivateAutoRepairFeature.Value = managerSetting.ActivateAutoRepairFeature;
                ActivateAutoSellingFeature.Value = managerSetting.ActivateAutoSellingFeature;
                SellGray.Checked = managerSetting.SellGray;
                SellWhite.Checked = managerSetting.SellWhite;
                SellGreen.Checked = managerSetting.SellGreen;
                SellBlue.Checked = managerSetting.SellBlue;
                SellPurple.Checked = managerSetting.SellPurple;
                DontSellTheseItems.Text = Others.ArrayToTextByLine(managerSetting.DontSellTheseItems.ToArray());
                ForceToSellTheseItems.Text = Others.ArrayToTextByLine(managerSetting.ForceToSellTheseItems.ToArray());
                ActivateAutoMaillingFeature.Value = managerSetting.ActivateAutoMaillingFeature;
                MaillingFeatureRecipient.Text = managerSetting.MaillingFeatureRecipient;
                MaillingFeatureSubject.Text = managerSetting.MaillingFeatureSubject;
                MailGray.Checked = managerSetting.MailGray;
                MailWhite.Checked = managerSetting.MailWhite;
                MailGreen.Checked = managerSetting.MailGreen;
                MailBlue.Checked = managerSetting.MailBlue;
                MailPurple.Checked = managerSetting.MailPurple;
                ForceToMailTheseItems.Text = Others.ArrayToTextByLine(managerSetting.ForceToMailTheseItems.ToArray());
                DontMailTheseItems.Text = Others.ArrayToTextByLine(managerSetting.DontMailTheseItems.ToArray());
                StopTNBIfBagAreFull.Value = managerSetting.StopTNBIfBagAreFull;
                StopTNBIfHonorPointsLimitReached.Value = managerSetting.StopTNBIfHonorPointsLimitReached;
                StopTNBIfPlayerHaveBeenTeleported.Value = managerSetting.StopTNBIfPlayerHaveBeenTeleported;
                StopTNBAfterXLevelup.Value = managerSetting.StopTNBAfterXLevelup;
                StopTNBIfReceivedAtMostXWhispers.Value = managerSetting.StopTNBIfReceivedAtMostXWhispers;
                StopTNBAfterXStucks.Value = managerSetting.StopTNBAfterXStucks;
                StopTNBAfterXMinutes.Value = managerSetting.StopTNBAfterXMinutes;
                PauseTNBIfNearByPlayer.Value = managerSetting.PauseTNBIfNearByPlayer;
                RecordWhispsInLogFiles.Value = managerSetting.RecordWhispsInLogFiles;
                PlayASongIfNewWhispReceived.Value = managerSetting.PlayASongIfNewWhispReceived;
                ActivatePathFindingFeature.Value = managerSetting.ActivatePathFindingFeature;
                AllowTNBToSetYourMaxFps.Value = managerSetting.AllowTNBToSetYourMaxFps;
                MaxDistanceToGoToMailboxesOrNPCs.Value = (decimal) managerSetting.MaxDistanceToGoToMailboxesOrNPCs;
                AutoConfirmOnBoPItems.Value = managerSetting.AutoConfirmOnBoPItems;
                ActivateAlwaysOnTopFeature.Value = managerSetting.ActivateAlwaysOnTopFeature;
                RepairWhenDurabilityIsUnderPercent.Value = managerSetting.RepairWhenDurabilityIsUnderPercent;
                SellItemsWhenLessThanXSlotLeft.Value = managerSetting.SellItemsWhenLessThanXSlotLeft;
                SendMailWhenLessThanXSlotLeft.Value = managerSetting.SendMailWhenLessThanXSlotLeft;
                UseHearthstone.Value = managerSetting.UseHearthstone;
                ActiveStopTNBAfterXLevelup.Value = managerSetting.ActiveStopTNBAfterXLevelup;
                ActiveStopTNBAfterXMinutes.Value = managerSetting.ActiveStopTNBAfterXMinutes;
                ActiveStopTNBAfterXStucks.Value = managerSetting.ActiveStopTNBAfterXStucks;
                ActiveStopTNBIfReceivedAtMostXWhispers.Value = managerSetting.ActiveStopTNBIfReceivedAtMostXWhispers;
                UseMollE.Value = managerSetting.UseMollE;
                UseRobot.Value = managerSetting.UseRobot;
                AutoCloseChatFrame.Value = managerSetting.AutoCloseChatFrame;
                ActivateBroadcastingMimesis.Value = managerSetting.ActivateBroadcastingMimesis;
                BroadcastingPort.Value = managerSetting.BroadcastingPort;
                ActivatePluginsSystem.Value = managerSetting.ActivatePluginsSystem;
                LaunchExpiredPlugins.Value = managerSetting.LaunchExpiredPlugins;
                ActivatedPluginsList.Items.Clear();
                foreach (string s in managerSetting.ActivatedPluginsList)
                {
                    if (AvailablePluginsList.Items.Contains(s))
                    {
                        AvailablePluginsList.Items.Remove(s);
                        ActivatedPluginsList.Items.Add(s);
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError("GeneralSettings > LoadSetting(nManagerSetting managerSetting): " + ex);
            }
        }

        private void saveAndCloseB_Click(object sender, EventArgs e)
        {
            try
            {
                SaveSetting();
                Dispose();
            }
            catch (Exception ex)
            {
                Logging.WriteError("GeneralSettings >  saveAndCloseB_Click(object sender, EventArgs e): " + ex);
            }
        }

        private void resetB_Click(object sender, EventArgs e)
        {
            try
            {
                resetB.Enabled = false;
                LoadSetting(new nManagerSetting());
                resetB.Enabled = true;
            }
            catch (Exception ex)
            {
                Logging.WriteError("GeneralSettings >  resetB_Click(object sender, EventArgs e): " + ex);
            }
        }

        private void closeB_Click(object sender, EventArgs e)
        {
            try
            {
                Close();
            }
            catch (Exception ex)
            {
                Logging.WriteError("GeneralSettings > closeB_Click(object sender, EventArgs e): " + ex);
            }
        }

        private void GeneralSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (e.CloseReason == CloseReason.UserClosing)
                {
                    DialogResult ret = MessageBox.Show(string.Format("{0}?", Translate.Get(Translate.Id.Do_you_want_save_this_setting)),
                        Translate.Get(Translate.Id.Save), MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);
                    if (ret == DialogResult.Yes)
                    {
                        SaveSetting();
                    }
                    else if (nManagerSetting.CurrentSetting.ActivatePluginsSystem)
                        Plugins.Plugins.ReLoadPlugins();
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError(
                    "GeneralSettings > GeneralSettings_FormClosing(object sender, FormClosingEventArgs e): " + ex);
            }
        }

        private void CombatClassSettingsButton_Click(object sender, EventArgs e)
        {
            Wow.Helpers.CombatClass.ShowConfigurationCombatClass(Application.StartupPath + "\\CombatClasses\\" + CombatClass.Text);
        }

        private void CombatClassResetSettingsButton_Click(object sender, EventArgs e)
        {
            Wow.Helpers.CombatClass.ResetConfigurationCombatClass(Application.StartupPath + "\\CombatClasses\\" + CombatClass.Text);
        }

        private void DontHaverstObjectsTutorial_Click(object sender, EventArgs e)
        {
            Others.OpenWebBrowserOrApplication("http://thenoobbot.com/community/viewtopic.php?f=43&t=5612");
        }

        private void HealerClassSettingsButton_Click(object sender, EventArgs e)
        {
            Wow.Helpers.HealerClass.ShowConfigurationHealerClass(Application.StartupPath + "\\HealerClass\\" + HealerClass.Text);
        }

        private void HealerClassResetSettingsButton_Click(object sender, EventArgs e)
        {
            Wow.Helpers.HealerClass.ResetConfigurationHealerClass(Application.StartupPath + "\\HealerClass\\" + HealerClass.Text);
        }

        private void LoadPlugin(object sender, EventArgs e)
        {
            ActivatePlugin.Enabled = false;
            if (AvailablePluginsList.Items.Count <= 0 || AvailablePluginsList.SelectedIndex < 0)
            {
                MessageBox.Show(@"There is no plugins available or selected.");
                ActivatePlugin.Enabled = true;
                return;
            }
            var p = new Plugin {PathToPluginFile = Application.StartupPath + "\\Plugins\\" + AvailablePluginsList.SelectedItem};
            if (!p.IsAlive)
            {
                MessageBox.Show(@"This plugin cannot be activated, read error logs for more informations.");
            }
            else if (!LaunchExpiredPlugins.Value && p.IsExpired)
            {
                MessageBox.Show(@"This plugin is expired and LaunchExpiredPlugins is OFF.");
            }
            else
            {
                ActivatedPluginsList.Items.Add(AvailablePluginsList.SelectedItem);
                AvailablePluginsList.Items.Remove(AvailablePluginsList.SelectedItem);
            }
            Plugins.Plugins.DisposePlugins();
            ActivatePlugin.Enabled = true;
        }

        private void UnLoadPlugin(object sender, EventArgs e)
        {
            DeactivatePlugin.Enabled = false;
            if (ActivatedPluginsList.Items.Count <= 0 || ActivatedPluginsList.SelectedIndex < 0)
            {
                MessageBox.Show(@"There is no plugins activated or selected.");
                DeactivatePlugin.Enabled = true;
                return;
            }
            AvailablePluginsList.Items.Add(ActivatedPluginsList.SelectedItem);
            ActivatedPluginsList.Items.Remove(ActivatedPluginsList.SelectedItem);
            Plugins.Plugins.DisposePlugins();
            DeactivatePlugin.Enabled = true;
        }

        private void ConfigurePlugin(object sender, EventArgs e)
        {
            ActivatedPluginSettings.Enabled = false;
            if (ActivatedPluginsList.Items.Count <= 0 || ActivatedPluginsList.SelectedIndex < 0)
            {
                MessageBox.Show(@"There is no plugins activated or selected.");
                ActivatedPluginSettings.Enabled = true;
                return;
            }
            var p = new Plugin {PathToPluginFile = Application.StartupPath + "\\Plugins\\" + ActivatedPluginsList.SelectedItem};
            p.ShowConfigurationPlugin();
            ActivatedPluginSettings.Enabled = true;
        }

        private void ResetPlugin(object sender, EventArgs e)
        {
            ActivatedPluginResetSettings.Enabled = false;
            if (ActivatedPluginsList.Items.Count <= 0 || ActivatedPluginsList.SelectedIndex < 0)
            {
                MessageBox.Show(@"There is no plugins activated or selected.");
                ActivatedPluginResetSettings.Enabled = true;
                return;
            }
            var p = new Plugin {PathToPluginFile = Application.StartupPath + "\\Plugins\\" + ActivatedPluginsList.SelectedItem};
            p.ResetConfigurationPlugin();
            ActivatedPluginResetSettings.Enabled = true;
        }
    }
}