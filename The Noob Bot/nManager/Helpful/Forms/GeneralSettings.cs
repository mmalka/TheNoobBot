using System;
using System.Windows.Forms;

namespace nManager.Helpful.Forms
{
    public partial class GeneralSettings : DevComponents.DotNetBar.Metro.MetroForm
    {
        public GeneralSettings()
        {
            try
            {
                InitializeComponent();
                TranslateForm();
                if (nManagerSetting.CurrentSetting.ActivateAlwaysOnTopFeature)
                    this.TopMost = true;

                LoadSetting(nManagerSetting.CurrentSetting);
                foreach (var f in Others.GetFilesDirectory(Application.StartupPath + "\\CustomClasses\\", "*.dll"))
                {
                    CustomClass.Items.Add(f);
                }
                foreach (var f in Others.GetFilesDirectory(Application.StartupPath + "\\CustomClasses\\", "*.cs"))
                {
                    CustomClass.Items.Add(f);
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("GeneralSettings > GeneralSettings(): " + e);
            }
        }

        private void SetToolTypeIfNeeded(Control label)
        {
            using (System.Drawing.Graphics g = CreateGraphics())
            {
                System.Drawing.SizeF size = g.MeasureString(label.Text, label.Font);
                if (size.Width > label.Width)
                {
                    this.labelsToolTip.SetToolTip(label, label.Text);
                }
            }
        }

        private void TranslateForm()
        {
            labelX45.Text = string.Format("{0}:", Translate.Get(Translate.Id.Security));
            SetToolTypeIfNeeded(labelX45);
            labelX34.Text = string.Format("{0}:", Translate.Get(Translate.Id.Close_game));
            SetToolTypeIfNeeded(labelX34);
            labelX30.Text = Translate.Get(Translate.Id.If_reached______Honor_Points);
            SetToolTypeIfNeeded(labelX30);
            labelX28.Text = Translate.Get(Translate.Id.After);
            SetToolTypeIfNeeded(labelX28);
            labelX26.Text = Translate.Get(Translate.Id.After);
            SetToolTypeIfNeeded(labelX26);
            labelX24.Text = Translate.Get(Translate.Id.After);
            SetToolTypeIfNeeded(labelX24);
            addBlackListHarvest.Text = Translate.Get(Translate.Id.Add);
            SetToolTypeIfNeeded(addBlackListHarvest);
            expandablePanel7.TitleText = Translate.Get(Translate.Id.Other_options);
            SetToolTypeIfNeeded(expandablePanel7);
            labelX60.Text = Translate.Get(Translate.Id.Npc_Mailbox_Search_Radius);
            SetToolTypeIfNeeded(labelX60);
            labelX42.Text = Translate.Get(Translate.Id.Use_Paths_Finder);
            SetToolTypeIfNeeded(labelX42);
            expandablePanel5.TitleText = Translate.Get(Translate.Id.Stop_game___tnb___Security);
            SetToolTypeIfNeeded(expandablePanel5);
            labelX39.Text = Translate.Get(Translate.Id.Song_if_New_Whisper);
            SetToolTypeIfNeeded(labelX39);
            labelX33.Text = Translate.Get(Translate.Id.Min);
            SetToolTypeIfNeeded(labelX33);
            labelX43.Text = Translate.Get(Translate.Id.Record_whisper_in_Log_file);
            SetToolTypeIfNeeded(labelX43);
            labelX32.Text = Translate.Get(Translate.Id.Blockages);
            SetToolTypeIfNeeded(labelX32);
            labelX31.Text = Translate.Get(Translate.Id.Level);
            SetToolTypeIfNeeded(labelX31);
            labelX29.Text = Translate.Get(Translate.Id.If_Player_Teleported);
            SetToolTypeIfNeeded(labelX29);
            labelX44.Text = Translate.Get(Translate.Id.Pause_tnb_if_Nearby_Player);
            SetToolTypeIfNeeded(labelX44);
            labelX25.Text = Translate.Get(Translate.Id.If_Whisper_bigger_or_equal_to);
            SetToolTypeIfNeeded(labelX25);
            labelX27.Text = Translate.Get(Translate.Id.If_full_Bag);
            SetToolTypeIfNeeded(labelX27);
            expandablePanel9.TitleText = Translate.Get(Translate.Id.Mail__Send_Items_to_alts);
            SetToolTypeIfNeeded(expandablePanel9);
            labelX56.Text = Translate.Get(Translate.Id.Mail_Recipient);
            SetToolTypeIfNeeded(labelX56);
            labelX48.Text = Translate.Get(Translate.Id.Force_Mail_List__one_item_by_line);
            SetToolTypeIfNeeded(labelX48);
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
            labelX54.Text = Translate.Get(Translate.Id.Subject);
            SetToolTypeIfNeeded(labelX54);
            labelX55.Text = Translate.Get(Translate.Id.Use_Mail);
            SetToolTypeIfNeeded(labelX55);
            labelX58.Text = Translate.Get(Translate.Id.Do_not_Mail_List__one_item_by_line);
            SetToolTypeIfNeeded(labelX58);
            expandablePanel8.TitleText = Translate.Get(Translate.Id.Vendor__Selling_or_Buying);
            SetToolTypeIfNeeded(expandablePanel8);
            labelX53.Text = Translate.Get(Translate.Id.Force_Sell_List__one_item_by_line);
            SetToolTypeIfNeeded(labelX53);
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
            labelX51.Text = Translate.Get(Translate.Id.Repair);
            SetToolTypeIfNeeded(labelX51);
            labelX50.Text = Translate.Get(Translate.Id.Food_Amount);
            SetToolTypeIfNeeded(labelX50);
            labelX41.Text = Translate.Get(Translate.Id.Drink_Amount);
            SetToolTypeIfNeeded(labelX41);
            labelX46.Text = Translate.Get(Translate.Id.Do_not_Sell_List__one_item_by_line);
            SetToolTypeIfNeeded(labelX46);
            expandablePanel6.TitleText = Translate.Get(Translate.Id.Relogger);
            SetToolTypeIfNeeded(expandablePanel6);
            labelX67.Text = Translate.Get(Translate.Id.BattleNet_Account);
            SetToolTypeIfNeeded(labelX67);
            labelX38.Text = Translate.Get(Translate.Id.Relogger);
            SetToolTypeIfNeeded(labelX38);
            labelX37.Text = Translate.Get(Translate.Id.Account_Password);
            SetToolTypeIfNeeded(labelX37);
            labelX40.Text = Translate.Get(Translate.Id.Account_Email);
            SetToolTypeIfNeeded(labelX40);
            expandablePanel4.TitleText = Translate.Get(Translate.Id.Looting____Farming_options);
            SetToolTypeIfNeeded(expandablePanel4);
            labelX65.Text = Translate.Get(Translate.Id.Prospecting_only_in_town);
            SetToolTypeIfNeeded(labelX65);
            labelX64.Text = Translate.Get(Translate.Id.Prospecting_Every__in_minute);
            SetToolTypeIfNeeded(labelX64);
            labelX63.Text = Translate.Get(Translate.Id.Prospecting);
            SetToolTypeIfNeeded(labelX63);
            labelX62.Text = Translate.Get(Translate.Id.Prospecting_list__one_item_by_line);
            SetToolTypeIfNeeded(labelX62);
            labelX69.Text = Translate.Get(Translate.Id.Milling_only_in_town);
            SetToolTypeIfNeeded(labelX69);
            labelX70.Text = Translate.Get(Translate.Id.Milling_Every__in_minute);
            SetToolTypeIfNeeded(labelX70);
            labelX71.Text = Translate.Get(Translate.Id.Milling);
            SetToolTypeIfNeeded(labelX71);
            labelX72.Text = Translate.Get(Translate.Id.Milling_list__one_item_by_line);
            SetToolTypeIfNeeded(labelX72);
            labelX61.Text = Translate.Get(Translate.Id.Smelting);
            SetToolTypeIfNeeded(labelX61);
            ResetDontHarvestTheFollowingObjectsButton.Text = Translate.Get(Translate.Id.Del);
            SetToolTypeIfNeeded(ResetDontHarvestTheFollowingObjectsButton);
            labelX36.Text = Translate.Get(Translate.Id.Don_t_harvest);
            SetToolTypeIfNeeded(labelX36);
            labelX35.Text = Translate.Get(Translate.Id.Harvest_During_Long_Move);
            SetToolTypeIfNeeded(labelX35);
            labelX23.Text = Translate.Get(Translate.Id.Ninja);
            SetToolTypeIfNeeded(labelX23);
            labelX22.Text = Translate.Get(Translate.Id.Search_Radius);
            SetToolTypeIfNeeded(labelX22);
            labelX21.Text = Translate.Get(Translate.Id.Max_Units_Near);
            SetToolTypeIfNeeded(labelX21);
            labelX20.Text = Translate.Get(Translate.Id.Harvest_Herbs);
            SetToolTypeIfNeeded(labelX20);
            labelX19.Text = Translate.Get(Translate.Id.Harvest_Minerals);
            SetToolTypeIfNeeded(labelX19);
            labelX17.Text = Translate.Get(Translate.Id.Skin_Mobs);
            SetToolTypeIfNeeded(labelX17);
            labelX16.Text = Translate.Get(Translate.Id.Loot_Chests);
            SetToolTypeIfNeeded(labelX16);
            labelX12.Text = Translate.Get(Translate.Id.Harvest_Avoid_Players_Radius);
            SetToolTypeIfNeeded(labelX12);
            labelX18.Text = Translate.Get(Translate.Id.Loot_Mobs);
            SetToolTypeIfNeeded(labelX18);
            expandablePanel3.TitleText = Translate.Get(Translate.Id.Food___Drink____percentage_to_be_used);
            SetToolTypeIfNeeded(expandablePanel3);
            labelX10.Text = Translate.Get(Translate.Id.on);
            SetToolTypeIfNeeded(labelX10);
            labelX13.Text = Translate.Get(Translate.Id.on);
            SetToolTypeIfNeeded(labelX13);
            labelX15.Text = Translate.Get(Translate.Id.Drink);
            SetToolTypeIfNeeded(labelX15);
            labelX11.Text = Translate.Get(Translate.Id.Food);
            SetToolTypeIfNeeded(labelX11);
            expandablePanel2.TitleText = Translate.Get(Translate.Id.Mount_options);
            SetToolTypeIfNeeded(expandablePanel2);
            labelX66.Text = Translate.Get(Translate.Id.Aquatic);
            SetToolTypeIfNeeded(labelX66);
            labelX57.Text = Translate.Get(Translate.Id.Ignore_Fight_if_in_Gound_Mount);
            SetToolTypeIfNeeded(labelX57);
            labelX8.Text = Translate.Get(Translate.Id.Mount_Distance);
            SetToolTypeIfNeeded(labelX8);
            labelX7.Text = Translate.Get(Translate.Id.Flying);
            SetToolTypeIfNeeded(labelX7);
            labelX6.Text = Translate.Get(Translate.Id.Ground);
            SetToolTypeIfNeeded(labelX6);
            labelX5.Text = Translate.Get(Translate.Id.Use_Ground_Mount);
            SetToolTypeIfNeeded(labelX5);
            expandablePanel1.TitleText = Translate.Get(Translate.Id.Class___Custom_spell_sequence_Settings);
            SetToolTypeIfNeeded(expandablePanel1);
            labelX59.Text = Translate.Get(Translate.Id.Use_Spirit_Healer);
            SetToolTypeIfNeeded(labelX59);
            labelX49.Text = Translate.Get(Translate.Id.Train_New_Spells);
            SetToolTypeIfNeeded(labelX49);
            labelX47.Text = Translate.Get(Translate.Id.Train_New_Skills);
            SetToolTypeIfNeeded(labelX47);
            labelX4.Text = Translate.Get(Translate.Id.Don_t_start_fighting);
            SetToolTypeIfNeeded(labelX4);
            labelX3.Text = Translate.Get(Translate.Id.Can_attack_units_already_in_fight);
            SetToolTypeIfNeeded(labelX3);
            labelX2.Text = Translate.Get(Translate.Id.Assign_Talents);
            SetToolTypeIfNeeded(labelX2);
            labelX1.Text = Translate.Get(Translate.Id.Custom_Class);
            SetToolTypeIfNeeded(labelX1);
            CustomClassSettingsButton.Text = Translate.Get(Translate.Id.Settings);
            SetToolTypeIfNeeded(CustomClassSettingsButton);
            CustomClassResetSettingsButton.Text = Translate.Get(Translate.Id.ResetSettings);
            SetToolTypeIfNeeded(CustomClassResetSettingsButton);
            closeB.Text = Translate.Get(Translate.Id.Close_without_save);
            SetToolTypeIfNeeded(closeB);
            resetB.Text = Translate.Get(Translate.Id.Reset_Settings);
            SetToolTypeIfNeeded(resetB);
            saveAndCloseB.Text = Translate.Get(Translate.Id.Save_and_Close);
            SetToolTypeIfNeeded(saveAndCloseB);
            labelX68.Text = Translate.Get(Translate.Id.Auto_Make_Elemental);
            SetToolTypeIfNeeded(labelX68);
            Text = Translate.Get(Translate.Id.General_Settings);
            labelX73.Text = Translate.Get(Translate.Id.Uncap_MaxFPS);
            SetToolTypeIfNeeded(labelX73);
            AutoConfirmOnBoPItemsLabel.Text = Translate.Get(Translate.Id.AutoConfirmOnBoPItems);
            SetToolTypeIfNeeded(AutoConfirmOnBoPItemsLabel);
            ActivateAlwaysOnTopFeature.Text = Translate.Get(Translate.Id.AlwaysOnTop);
            SendMailWhenLessThanXSlotLeftLabel.Text = Translate.Get(Translate.Id.SendMailWhenLessThanXSlotLeft);
            SellItemsWhenLessThanXSlotLeftLabel.Text = Translate.Get(Translate.Id.SellItemsWhenLessThanXSlotLeft);
            RepairWhenDurabilityIsUnderPercentLabel.Text = Translate.Get(Translate.Id.RepairWhenDurabilityIsUnderPercent);
            UseHearthstoneLabel.Text = Translate.Get(Translate.Id.UseHearthstone);
            SetToolTypeIfNeeded(AlwaysOnTopFeatureLabel);
        }

        private void SaveSetting()
        {
            try
            {
                nManagerSetting.CurrentSetting.CustomClass = CustomClass.Text;
                nManagerSetting.CurrentSetting.AutoAssignTalents = AutoAssignTalents.Value;
                nManagerSetting.CurrentSetting.TrainNewSkills = TrainNewSkills.Value;
                nManagerSetting.CurrentSetting.LearnNewSpells = LearnNewSpells.Value;
                nManagerSetting.CurrentSetting.CanPullUnitsAlreadyInFight = CanPullUnitsAlreadyInFight.Value;
                nManagerSetting.CurrentSetting.DontPullMonsters = DontPullMonsters.Value;
                nManagerSetting.CurrentSetting.UseSpiritHealer = UseSpiritHealer.Value;
                nManagerSetting.CurrentSetting.UseGroundMount = UseGroundMount.Value;
                nManagerSetting.CurrentSetting.GroundMountName = GroundMountName.Text;
                nManagerSetting.CurrentSetting.MinimumDistanceToUseMount = MinimumDistanceToUseMount.Value;
                nManagerSetting.CurrentSetting.IgnoreFightIfMounted = IgnoreFightIfMounted.Value;
                nManagerSetting.CurrentSetting.FlyingMountName = FlyingMountName.Text;
                nManagerSetting.CurrentSetting.AquaticMountName = AquaticMountName.Text;
                nManagerSetting.CurrentSetting.FoodName = FoodName.Text;
                nManagerSetting.CurrentSetting.EatFoodWhenHealthIsUnderXPercent = EatFoodWhenHealthIsUnderXPercent.Value;
                nManagerSetting.CurrentSetting.BeverageName = BeverageName.Text;
                nManagerSetting.CurrentSetting.DrinkBeverageWhenManaIsUnderXPercent =
                    DrinkBeverageWhenManaIsUnderXPercent.Value;
                nManagerSetting.CurrentSetting.DoRegenManaIfLow = DoRegenManaIfLow.Value;
                nManagerSetting.CurrentSetting.ActivateMonsterLooting = ActivateMonsterLooting.Value;
                nManagerSetting.CurrentSetting.ActivateChestLooting = ActivateChestLooting.Value;
                nManagerSetting.CurrentSetting.ActivateBeastSkinning = ActivateBeastSkinning.Value;
                nManagerSetting.CurrentSetting.BeastNinjaSkinning = BeastNinjaSkinning.Value;
                nManagerSetting.CurrentSetting.ActivateVeinsHarvesting = ActivateVeinsHarvesting.Value;
                nManagerSetting.CurrentSetting.ActivateHerbsHarvesting = ActivateHerbsHarvesting.Value;
                nManagerSetting.CurrentSetting.DontHarvestIfPlayerNearRadius = DontHarvestIfPlayerNearRadius.Value;
                nManagerSetting.CurrentSetting.DontHarvestIfMoreThanXUnitInAggroRange =
                    DontHarvestIfMoreThanXUnitInAggroRange.Value;
                nManagerSetting.CurrentSetting.GatheringSearchRadius = GatheringSearchRadius.Value;
                nManagerSetting.CurrentSetting.HarvestDuringLongDistanceMovements =
                    HarvestDuringLongDistanceMovements.Value;
                nManagerSetting.CurrentSetting.ActivateAutoSmelting = ActivateAutoSmelting.Value;
                nManagerSetting.CurrentSetting.ActivateAutoProspecting = ActivateAutoProspecting.Value;
                nManagerSetting.CurrentSetting.TimeBetweenEachProspectingAttempt =
                    TimeBetweenEachProspectingAttempt.Value;
                nManagerSetting.CurrentSetting.OnlyUseProspectingInTown = OnlyUseProspectingInTown.Value;
                nManagerSetting.CurrentSetting.MineralsToProspect.Clear();
                nManagerSetting.CurrentSetting.MineralsToProspect.AddRange(
                    Others.TextToArrayByLine(MineralsToProspect.Text));
                nManagerSetting.CurrentSetting.ActivateAutoMilling = ActivateAutoMilling.Value;
                nManagerSetting.CurrentSetting.TimeBetweenEachMillingAttempt = TimeBetweenEachMillingAttempt.Value;
                nManagerSetting.CurrentSetting.OnlyUseMillingInTown = OnlyUseMillingInTown.Value;
                nManagerSetting.CurrentSetting.HerbsToBeMilled.Clear();
                nManagerSetting.CurrentSetting.HerbsToBeMilled.AddRange(Others.TextToArrayByLine(HerbsToBeMilled.Text));
                nManagerSetting.CurrentSetting.DontHarvestTheFollowingObjects.Clear();
                try
                {
                    foreach (string i in DontHarvestTheFollowingObjects.Items)
                    {
                        try
                        {
                            string[] result = i.Replace(" ", "").Split(Convert.ToChar("-"));
                            if (result.Length > 0)
                                nManagerSetting.CurrentSetting.DontHarvestTheFollowingObjects.Add(
                                    Convert.ToInt32(result[0]));
                        }
                        catch
                        {
                        }
                    }
                }
                catch
                {
                }
                nManagerSetting.CurrentSetting.MakeStackOfElementalsItems = MakeStackOfElementalsItems.Value;
                nManagerSetting.CurrentSetting.ActivateReloggerFeature = ActivateReloggerFeature.Value;
                nManagerSetting.CurrentSetting.EmailOfTheBattleNetAccount = EmailOfTheBattleNetAccount.Text;
                nManagerSetting.CurrentSetting.PasswordOfTheBattleNetAccount = PasswordOfTheBattleNetAccount.Text;
                nManagerSetting.CurrentSetting.BattleNetSubAccount = BattleNetSubAccount.Text;
                nManagerSetting.CurrentSetting.NumberOfFoodsWeGot = NumberOfFoodsWeGot.Value;
                nManagerSetting.CurrentSetting.NumberOfBeverageWeGot = NumberOfBeverageWeGot.Value;
                nManagerSetting.CurrentSetting.ActivateAutoRepairFeature = ActivateAutoRepairFeature.Value;
                nManagerSetting.CurrentSetting.ActivateAutoSellingFeature = ActivateAutoSellingFeature.Value;
                nManagerSetting.CurrentSetting.SellGray = SellGray.Checked;
                nManagerSetting.CurrentSetting.SellWhite = SellWhite.Checked;
                nManagerSetting.CurrentSetting.SellGreen = SellGreen.Checked;
                nManagerSetting.CurrentSetting.SellBlue = SellBlue.Checked;
                nManagerSetting.CurrentSetting.SellPurple = SellPurple.Checked;
                nManagerSetting.CurrentSetting.DontSellTheseItems.Clear();
                nManagerSetting.CurrentSetting.DontSellTheseItems.AddRange(
                    Others.TextToArrayByLine(DontSellTheseItems.Text));
                nManagerSetting.CurrentSetting.ForceToSellTheseItems.Clear();
                nManagerSetting.CurrentSetting.ForceToSellTheseItems.AddRange(
                    Others.TextToArrayByLine(ForceToSellTheseItems.Text));
                nManagerSetting.CurrentSetting.ActivateAutoMaillingFeature = ActivateAutoMaillingFeature.Value;
                nManagerSetting.CurrentSetting.MaillingFeatureRecipient = MaillingFeatureRecipient.Text;
                nManagerSetting.CurrentSetting.MaillingFeatureSubject = MaillingFeatureSubject.Text;
                nManagerSetting.CurrentSetting.MailGray = MailGray.Checked;
                nManagerSetting.CurrentSetting.MailWhite = MailWhite.Checked;
                nManagerSetting.CurrentSetting.MailGreen = MailGreen.Checked;
                nManagerSetting.CurrentSetting.MailBlue = MailBlue.Checked;
                nManagerSetting.CurrentSetting.MailPurple = MailPurple.Checked;
                nManagerSetting.CurrentSetting.ForceToMailTheseItems.Clear();
                nManagerSetting.CurrentSetting.ForceToMailTheseItems.AddRange(
                    Others.TextToArrayByLine(ForceToMailTheseItems.Text));
                nManagerSetting.CurrentSetting.DontMailTheseItems.Clear();
                nManagerSetting.CurrentSetting.DontMailTheseItems.AddRange(
                    Others.TextToArrayByLine(DontMailTheseItems.Text));
                nManagerSetting.CurrentSetting.StopTNBIfBagAreFull = StopTNBIfBagAreFull.Value;
                nManagerSetting.CurrentSetting.StopTNBIfHonorPointsLimitReached = StopTNBIfHonorPointsLimitReached.Value;
                nManagerSetting.CurrentSetting.StopTNBIfPlayerHaveBeenTeleported =
                    StopTNBIfPlayerHaveBeenTeleported.Value;
                nManagerSetting.CurrentSetting.StopTNBAfterXLevelup = StopTNBAfterXLevelup.Value;
                nManagerSetting.CurrentSetting.StopTNBIfReceivedAtMostXWhispers = StopTNBIfReceivedAtMostXWhispers.Value;
                nManagerSetting.CurrentSetting.StopTNBAfterXStucks = StopTNBAfterXStucks.Value;
                nManagerSetting.CurrentSetting.StopTNBAfterXMinutes = StopTNBAfterXMinutes.Value;
                nManagerSetting.CurrentSetting.PauseTNBIfNearByPlayer = PauseTNBIfNearByPlayer.Value;
                nManagerSetting.CurrentSetting.RecordWhispsInLogFiles = RecordWhispsInLogFiles.Value;
                nManagerSetting.CurrentSetting.PlayASongIfNewWhispReceived = PlayASongIfNewWhispReceived.Value;
                nManagerSetting.CurrentSetting.ActivatePathFindingFeature = ActivatePathFindingFeature.Value;
                nManagerSetting.CurrentSetting.AllowTNBToSetYourMaxFps = AllowTNBToSetYourMaxFps.Value;
                nManagerSetting.CurrentSetting.MaxDistanceToGoToMailboxesOrNPCs = MaxDistanceToGoToMailboxesOrNPCs.Value;
                nManagerSetting.CurrentSetting.AutoConfirmOnBoPItems = AutoConfirmOnBoPItems.Value;
                nManagerSetting.CurrentSetting.ActivateAlwaysOnTopFeature = ActivateAlwaysOnTopFeature.Value;
                nManagerSetting.CurrentSetting.RepairWhenDurabilityIsUnderPercent =
                    RepairWhenDurabilityIsUnderPercent.Value;
                nManagerSetting.CurrentSetting.SellItemsWhenLessThanXSlotLeft = SellItemsWhenLessThanXSlotLeft.Value;
                nManagerSetting.CurrentSetting.SendMailWhenLessThanXSlotLeft = SendMailWhenLessThanXSlotLeft.Value;
                nManagerSetting.CurrentSetting.UseHearthstone = UseHearthstone.Value;
                nManagerSetting.CurrentSetting.ActiveStopTNBAfterXLevelup = ActiveStopTNBAfterXLevelup.Value;
                nManagerSetting.CurrentSetting.ActiveStopTNBAfterXMinutes = ActiveStopTNBAfterXMinutes.Value;
                nManagerSetting.CurrentSetting.ActiveStopTNBAfterXStucks = ActiveStopTNBAfterXStucks.Value;
                nManagerSetting.CurrentSetting.ActiveStopTNBIfReceivedAtMostXWhispers =
                    ActiveStopTNBIfReceivedAtMostXWhispers.Value;
                nManagerSetting.CurrentSetting.Save();
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
                CustomClass.Text = managerSetting.CustomClass;
                AutoAssignTalents.Value = managerSetting.AutoAssignTalents;
                TrainNewSkills.Value = managerSetting.TrainNewSkills;
                LearnNewSpells.Value = managerSetting.LearnNewSpells;
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
                ActivateChestLooting.Value = managerSetting.ActivateChestLooting;
                ActivateBeastSkinning.Value = managerSetting.ActivateBeastSkinning;
                BeastNinjaSkinning.Value = managerSetting.BeastNinjaSkinning;
                ActivateVeinsHarvesting.Value = managerSetting.ActivateVeinsHarvesting;
                ActivateHerbsHarvesting.Value = managerSetting.ActivateHerbsHarvesting;
                DontHarvestIfPlayerNearRadius.Value = (int) managerSetting.DontHarvestIfPlayerNearRadius;
                DontHarvestIfMoreThanXUnitInAggroRange.Value = managerSetting.DontHarvestIfMoreThanXUnitInAggroRange;
                GatheringSearchRadius.Value = (int) managerSetting.GatheringSearchRadius;
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
                DontHarvestTheFollowingObjects.Items.Clear();
                try
                {
                    foreach (var id in managerSetting.DontHarvestTheFollowingObjects)
                    {
                        if (id >= 0)
                            DontHarvestTheFollowingObjects.Items.Add(id);
                    }
                }
                catch
                {
                }
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
                MaxDistanceToGoToMailboxesOrNPCs.Value = (int) managerSetting.MaxDistanceToGoToMailboxesOrNPCs;
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
                LoadSetting(new nManagerSetting());
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
                Dispose();
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
                    var ret = MessageBox.Show(Translate.Get(Translate.Id.Do_you_want_save_this_setting) + "?",
                                              Translate.Get(Translate.Id.Save), MessageBoxButtons.YesNo,
                                              MessageBoxIcon.Question);
                    if (ret == DialogResult.Yes)
                        SaveSetting();
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError(
                    "GeneralSettings > GeneralSettings_FormClosing(object sender, FormClosingEventArgs e): " + ex);
            }
        }

        private void customClassConfigB_Click(object sender, EventArgs e)
        {
            try
            {
                nManager.Wow.Helpers.CustomClass.ShowConfigurationCustomClass(Application.StartupPath +
                                                                              "\\CustomClasses\\" + CustomClass.Text);
            }
            catch (Exception ex)
            {
                Logging.WriteError("GeneralSettings > customClassConfigB_Click(object sender, EventArgs e): " + ex);
            }
        }

        private void delBlackListHarvest_Click(object sender, EventArgs e)
        {
            try
            {
                if (DontHarvestTheFollowingObjects.SelectedIndex >= 0)
                    DontHarvestTheFollowingObjects.Items.RemoveAt(DontHarvestTheFollowingObjects.SelectedIndex);
            }
            catch (Exception ex)
            {
                Logging.WriteError("GeneralSettings > delBlackListHarvest_Click(object sender, EventArgs e): " + ex);
            }
        }

        private void CustomClassResetSettingsButton_Click(object sender, EventArgs e)
        {
            nManager.Wow.Helpers.CustomClass.ResetConfigurationCustomClass(Application.StartupPath + "\\CustomClasses\\" +
                                                                           CustomClass.Text);
        }
    }
}