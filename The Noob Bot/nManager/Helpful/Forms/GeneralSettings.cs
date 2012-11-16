using System;
using System.Windows.Forms;
using nManager.Wow.Helpers;

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
                    customClass.Items.Add(f);
                }
                foreach (var f in Others.GetFilesDirectory(Application.StartupPath + "\\CustomClasses\\", "*.cs"))
                {
                    customClass.Items.Add(f);
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("GeneralSettings > GeneralSettings(): " + e);

            }
        }

        void SetToolTypeIfNeeded(Control label)
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

        void TranslateForm()
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
            mailPurple.Text = Translate.Get(Translate.Id.Mail_Purple_items);
            SetToolTypeIfNeeded(mailPurple);
            mailBlue.Text = Translate.Get(Translate.Id.Mail_Blue_items);
            SetToolTypeIfNeeded(mailBlue);
            mailGreen.Text = Translate.Get(Translate.Id.Mail_Green_items);
            SetToolTypeIfNeeded(mailGreen);
            mailWhite.Text = Translate.Get(Translate.Id.Mail_White_items);
            SetToolTypeIfNeeded(mailWhite);
            mailGray.Text = Translate.Get(Translate.Id.Mail_Gray_items);
            SetToolTypeIfNeeded(mailGray);
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
            sellPurple.Text = Translate.Get(Translate.Id.Sell_Purple_items);
            SetToolTypeIfNeeded(sellPurple);
            sellBlue.Text = Translate.Get(Translate.Id.Sell_Blue_items);
            SetToolTypeIfNeeded(sellBlue);
            sellGreen.Text = Translate.Get(Translate.Id.Sell_Green_items);
            SetToolTypeIfNeeded(sellGreen);
            sellWhite.Text = Translate.Get(Translate.Id.Sell_White_items);
            SetToolTypeIfNeeded(sellWhite);
            sellGray.Text = Translate.Get(Translate.Id.Sell_Gray_items);
            SetToolTypeIfNeeded(sellGray);
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
            delBlackListHarvest.Text = Translate.Get(Translate.Id.Del);
            SetToolTypeIfNeeded(delBlackListHarvest);
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
            customClassConfigB.Text = Translate.Get(Translate.Id.Settings);
            SetToolTypeIfNeeded(customClassConfigB);
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
            AlwaysOnTop.Text = Translate.Get(Translate.Id.AlwaysOnTop);
            SetToolTypeIfNeeded(AlwaysOnTopLabel);
        }

        void SaveSetting()
        {
            try
            {
                nManagerSetting.CurrentSetting.CustomClass = customClass.Text;
                nManagerSetting.CurrentSetting.AutoAssignTalents = assignTalents.Value;
                nManagerSetting.CurrentSetting.TrainNewSkills = trainNewSkills.Value;
                nManagerSetting.CurrentSetting.LearnNewSpells = trainNewSpells.Value;
                nManagerSetting.CurrentSetting.CanPullUnitsAlreadyInFight = canAttackUnitsAlreadyInFight.Value;
                nManagerSetting.CurrentSetting.DontPullMonsters = dontStartFighting.Value;
                nManagerSetting.CurrentSetting.UseSpiritHealer = useSpiritHealer.Value;
                nManagerSetting.CurrentSetting.UseGroundMount = useGroundMount.Value;
                nManagerSetting.CurrentSetting.GroundMountName = GroundMountName.Text;
                nManagerSetting.CurrentSetting.MinimumDistanceToUseMount = mountDistance.Value;
                nManagerSetting.CurrentSetting.IgnoreFightIfMounted = ignoreFightGoundMount.Value;
                nManagerSetting.CurrentSetting.FlyingMountName = FlyingMountName.Text;
                nManagerSetting.CurrentSetting.AquaticMountName = AquaticMountName.Text;
                nManagerSetting.CurrentSetting.FoodName = foodName.Text;
                nManagerSetting.CurrentSetting.EatFoodWhenHealthIsUnderXPercent = foodPercent.Value;
                nManagerSetting.CurrentSetting.BeverageName = drinkName.Text;
                nManagerSetting.CurrentSetting.DrinkBeverageWhenManaIsUnderXPercent = drinkPercent.Value;
                nManagerSetting.CurrentSetting.DoRegenManaIfLow = restingMana.Value;
                nManagerSetting.CurrentSetting.ActivateMonsterLooting = lootMobs.Value;
                nManagerSetting.CurrentSetting.ActivateChestLooting = lootChests.Value;
                nManagerSetting.CurrentSetting.ActivateBeastSkinning = skinMobs.Value;
                nManagerSetting.CurrentSetting.BeastNinjaSkinning = skinNinja.Value;
                nManagerSetting.CurrentSetting.ActivateVeinsHarvesting = harvestMinerals.Value;
                nManagerSetting.CurrentSetting.ActivateHerbsHarvesting = harvestHerbs.Value;
                nManagerSetting.CurrentSetting.DontHarvestIfPlayerNearRadius = harvestAvoidPlayersRadius.Value;
                nManagerSetting.CurrentSetting.DontHarvestIfMoreThanOneUnitInAggroRange = maxUnitsNear.Value;
                nManagerSetting.CurrentSetting.GatheringSearchRadius = searchRadius.Value;
                nManagerSetting.CurrentSetting.HarvestDuringLongDistanceMovements = harvestDuringLongMove.Value;
                nManagerSetting.CurrentSetting.ActivateAutoSmelting = smelting.Value;
                nManagerSetting.CurrentSetting.ActivateAutoProspecting = prospecting.Value;
                nManagerSetting.CurrentSetting.TimeBetweenEachProspectingAttempt = prospectingTime.Value;
                nManagerSetting.CurrentSetting.OnlyUseProspectingInTown = prospectingInTown.Value;
                nManagerSetting.CurrentSetting.MineralsToProspect.Clear();
                nManagerSetting.CurrentSetting.MineralsToProspect.AddRange(Others.TextToArrayByLine(prospectingList.Text));
                nManagerSetting.CurrentSetting.ActivateAutoMilling = milling.Value;
                nManagerSetting.CurrentSetting.TimeBetweenEachMillingAttempt = millingTime.Value;
                nManagerSetting.CurrentSetting.OnlyUseMillingInTown = millingInTown.Value;
                nManagerSetting.CurrentSetting.HerbsToBeMilled.Clear();
                nManagerSetting.CurrentSetting.HerbsToBeMilled.AddRange(Others.TextToArrayByLine(millingList.Text));
                nManagerSetting.CurrentSetting.DontHarvestTheFollowingObjects.Clear();
                try
                {
                    foreach (string i in blackListHarvest.Items)
                    {
                        try
                        {
                            string[] result = i.Replace(" ", "").Split(Convert.ToChar("-"));
                            if (result.Length > 0)
                                nManagerSetting.CurrentSetting.DontHarvestTheFollowingObjects.Add(Convert.ToInt32(result[0]));
                        }
                        catch { }
                    }
                }
                catch { }
                nManagerSetting.CurrentSetting.MakeStackOfElementalsItems = autoMakeElemental.Value;
                nManagerSetting.CurrentSetting.ActivateReloggerFeature = relogger.Value;
                nManagerSetting.CurrentSetting.EmailOfTheBattleNetAccount = accountEmail.Text;
                nManagerSetting.CurrentSetting.PasswordOfTheBattleNetAccount = accountPassword.Text;
                nManagerSetting.CurrentSetting.BattleNetSubAccount = bNetName.Text;
                nManagerSetting.CurrentSetting.NumberOfFoodsWeGot = foodAmount.Value;
                nManagerSetting.CurrentSetting.NumberOfBeverageWeGot = drinkAmount.Value;
                nManagerSetting.CurrentSetting.ActivateAutoRepairFeature = repair.Value;
                nManagerSetting.CurrentSetting.ActivateAutoSellingFeature = selling.Value;
                nManagerSetting.CurrentSetting.SellGray = sellGray.Checked;
                nManagerSetting.CurrentSetting.SellWhite = sellWhite.Checked;
                nManagerSetting.CurrentSetting.SellGreen = sellGreen.Checked;
                nManagerSetting.CurrentSetting.SellBlue = sellBlue.Checked;
                nManagerSetting.CurrentSetting.SellPurple = sellPurple.Checked;
                nManagerSetting.CurrentSetting.DontSellTheseItems.Clear();
                nManagerSetting.CurrentSetting.DontSellTheseItems.AddRange(Others.TextToArrayByLine(doNotSellList.Text));
                nManagerSetting.CurrentSetting.ForceToSellTheseItems.Clear();
                nManagerSetting.CurrentSetting.ForceToSellTheseItems.AddRange(Others.TextToArrayByLine(forceSellList.Text));
                nManagerSetting.CurrentSetting.ActivateAutoMaillingFeature = useMail.Value;
                nManagerSetting.CurrentSetting.MaillingFeatureRecipient = mailRecipient.Text;
                nManagerSetting.CurrentSetting.MaillingFeatureSubject = mailSubject.Text;
                nManagerSetting.CurrentSetting.MailGray = mailGray.Checked;
                nManagerSetting.CurrentSetting.MailWhite = mailWhite.Checked;
                nManagerSetting.CurrentSetting.MailGreen = mailGreen.Checked;
                nManagerSetting.CurrentSetting.MailBlue = mailBlue.Checked;
                nManagerSetting.CurrentSetting.MailPurple = mailPurple.Checked;
                nManagerSetting.CurrentSetting.ForceToMailTheseItems.Clear();
                nManagerSetting.CurrentSetting.ForceToMailTheseItems.AddRange(Others.TextToArrayByLine(forceMailList.Text));
                nManagerSetting.CurrentSetting.DontMailTheseItems.Clear();
                nManagerSetting.CurrentSetting.DontMailTheseItems.AddRange(Others.TextToArrayByLine(doNotMailList.Text));
                nManagerSetting.CurrentSetting.StopTNBIfBagAreFull = closeIfFullBag.Value;
                nManagerSetting.CurrentSetting.StopTNBIfHonorPointsLimitReached = closeIfReached4000HonorPoints.Value;
                nManagerSetting.CurrentSetting.StopTNBIfPlayerHaveBeenTeleported = closeIfPlayerTeleported.Value;
                nManagerSetting.CurrentSetting.StopTNBAfterXLevelup = closeAfterXLevel.Value;
                nManagerSetting.CurrentSetting.StopTNBIfReceivedAtMostXWhispers = closeIfWhisperBiggerOrEgalAt.Value;
                nManagerSetting.CurrentSetting.StopTNBAfterXStucks = closeAfterXBlockages.Value;
                nManagerSetting.CurrentSetting.StopTNBAfterXMinutes = closeAfterXMin.Value;
                nManagerSetting.CurrentSetting.PauseTNBIfNearByPlayer = securityPauseBotIfNerbyPlayer.Value;
                nManagerSetting.CurrentSetting.RecordWhispsInLogFiles = securityRecordWhisperInLogFile.Value;
                nManagerSetting.CurrentSetting.PlayASongIfNewWhispReceived = securitySongIfNewWhisper.Value;
                nManagerSetting.CurrentSetting.ActivatePathFindingFeature = usePathsFinder.Value;
                nManagerSetting.CurrentSetting.AllowTNBToSetYourMapFps = MaxFPSSwitch.Value;
                nManagerSetting.CurrentSetting.MaxDistanceToGoToMailboxesOrNPCs = npcMailboxSearchRadius.Value;
                nManagerSetting.CurrentSetting.AutoConfirmOnBoPItems = AutoConfirmOnBoPItems.Value;
                nManagerSetting.CurrentSetting.ActivateAlwaysOnTopFeature = AlwaysOnTop.Value;
                nManagerSetting.CurrentSetting.Save();
            }
            catch (Exception e)
            {
                Logging.WriteError("GeneralSettings > SaveSetting(): " + e);

            }
        }

        void LoadSetting(nManagerSetting managerSetting)
        {
            try
            {
                customClass.Text = managerSetting.CustomClass;
                assignTalents.Value = managerSetting.AutoAssignTalents;
                trainNewSkills.Value = managerSetting.TrainNewSkills;
                trainNewSpells.Value = managerSetting.LearnNewSpells;
                canAttackUnitsAlreadyInFight.Value = managerSetting.CanPullUnitsAlreadyInFight;
                dontStartFighting.Value = managerSetting.DontPullMonsters;
                useSpiritHealer.Value = managerSetting.UseSpiritHealer;
                useGroundMount.Value = managerSetting.UseGroundMount;
                GroundMountName.Text = managerSetting.GroundMountName;
                mountDistance.Value = (int)managerSetting.MinimumDistanceToUseMount;
                ignoreFightGoundMount.Value = managerSetting.IgnoreFightIfMounted;
                FlyingMountName.Text = managerSetting.FlyingMountName;
                AquaticMountName.Text = managerSetting.AquaticMountName;
                foodName.Text = managerSetting.FoodName;
                foodPercent.Value = managerSetting.EatFoodWhenHealthIsUnderXPercent;
                drinkName.Text = managerSetting.BeverageName;
                drinkPercent.Value = managerSetting.DrinkBeverageWhenManaIsUnderXPercent;
                restingMana.Value = managerSetting.DoRegenManaIfLow;
                lootMobs.Value = managerSetting.ActivateMonsterLooting;
                lootChests.Value = managerSetting.ActivateChestLooting;
                skinMobs.Value = managerSetting.ActivateBeastSkinning;
                skinNinja.Value = managerSetting.BeastNinjaSkinning;
                harvestMinerals.Value = managerSetting.ActivateVeinsHarvesting;
                harvestHerbs.Value = managerSetting.ActivateHerbsHarvesting;
                harvestAvoidPlayersRadius.Value = (int)managerSetting.DontHarvestIfPlayerNearRadius;
                maxUnitsNear.Value = managerSetting.DontHarvestIfMoreThanOneUnitInAggroRange;
                searchRadius.Value = (int)managerSetting.GatheringSearchRadius;
                harvestDuringLongMove.Value = managerSetting.HarvestDuringLongDistanceMovements;
                smelting.Value = managerSetting.ActivateAutoSmelting;
                prospecting.Value = managerSetting.ActivateAutoProspecting;
                prospectingInTown.Value = managerSetting.OnlyUseProspectingInTown;
                prospectingTime.Value = managerSetting.TimeBetweenEachProspectingAttempt;
                prospectingList.Text = Others.ArrayToTextByLine(managerSetting.MineralsToProspect.ToArray());
                milling.Value = managerSetting.ActivateAutoMilling;
                millingInTown.Value = managerSetting.OnlyUseMillingInTown;
                millingTime.Value = managerSetting.TimeBetweenEachMillingAttempt;
                millingList.Text = Others.ArrayToTextByLine(managerSetting.HerbsToBeMilled.ToArray());
                blackListHarvest.Items.Clear();
                try
                {
                    foreach (var id in managerSetting.DontHarvestTheFollowingObjects)
                    {
                        if (id >= 0)
                            blackListHarvest.Items.Add(id);
                    }
                }
                catch{}
                autoMakeElemental.Value = managerSetting.MakeStackOfElementalsItems;
                relogger.Value = managerSetting.ActivateReloggerFeature;
                accountEmail.Text = managerSetting.EmailOfTheBattleNetAccount;
                accountPassword.Text = managerSetting.PasswordOfTheBattleNetAccount;
                bNetName.Text = managerSetting.BattleNetSubAccount;
                foodAmount.Value = managerSetting.NumberOfFoodsWeGot;
                drinkAmount.Value = managerSetting.NumberOfBeverageWeGot;
                repair.Value = managerSetting.ActivateAutoRepairFeature;
                selling.Value = managerSetting.ActivateAutoSellingFeature;
                sellGray.Checked = managerSetting.SellGray;
                sellWhite.Checked = managerSetting.SellWhite;
                sellGreen.Checked = managerSetting.SellGreen;
                sellBlue.Checked = managerSetting.SellBlue;
                sellPurple.Checked = managerSetting.SellPurple;
                doNotSellList.Text = Others.ArrayToTextByLine(managerSetting.DontSellTheseItems.ToArray());
                forceSellList.Text = Others.ArrayToTextByLine(managerSetting.ForceToSellTheseItems.ToArray());
                useMail.Value = managerSetting.ActivateAutoMaillingFeature;
                mailRecipient.Text = managerSetting.MaillingFeatureRecipient;
                mailSubject.Text = managerSetting.MaillingFeatureSubject;
                mailGray.Checked = managerSetting.MailGray;
                mailWhite.Checked = managerSetting.MailWhite;
                mailGreen.Checked = managerSetting.MailGreen;
                mailBlue.Checked = managerSetting.MailBlue;
                mailPurple.Checked = managerSetting.MailPurple;
                forceMailList.Text = Others.ArrayToTextByLine(managerSetting.ForceToMailTheseItems.ToArray());
                doNotMailList.Text = Others.ArrayToTextByLine(managerSetting.DontMailTheseItems.ToArray());
                closeIfFullBag.Value = managerSetting.StopTNBIfBagAreFull;
                closeIfReached4000HonorPoints.Value = managerSetting.StopTNBIfHonorPointsLimitReached;
                closeIfPlayerTeleported.Value = managerSetting.StopTNBIfPlayerHaveBeenTeleported;
                closeAfterXLevel.Value = managerSetting.StopTNBAfterXLevelup;
                closeIfWhisperBiggerOrEgalAt.Value = managerSetting.StopTNBIfReceivedAtMostXWhispers;
                closeAfterXBlockages.Value = managerSetting.StopTNBAfterXStucks;
                closeAfterXMin.Value = managerSetting.StopTNBAfterXMinutes;
                securityPauseBotIfNerbyPlayer.Value = managerSetting.PauseTNBIfNearByPlayer;
                securityRecordWhisperInLogFile.Value = managerSetting.RecordWhispsInLogFiles;
                securitySongIfNewWhisper.Value = managerSetting.PlayASongIfNewWhispReceived;
                usePathsFinder.Value = managerSetting.ActivatePathFindingFeature;
                MaxFPSSwitch.Value = managerSetting.AllowTNBToSetYourMapFps;
                npcMailboxSearchRadius.Value = (int)managerSetting.MaxDistanceToGoToMailboxesOrNPCs;
                AutoConfirmOnBoPItems.Value = managerSetting.AutoConfirmOnBoPItems;
                AlwaysOnTop.Value = managerSetting.ActivateAlwaysOnTopFeature;
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
                    var ret = MessageBox.Show(Translate.Get(Translate.Id.Do_you_want_save_this_setting) + "?", Translate.Get(Translate.Id.Save), MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Question);
                    if (ret == DialogResult.Yes)
                        SaveSetting();
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError("GeneralSettings > GeneralSettings_FormClosing(object sender, FormClosingEventArgs e): " + ex);

            }
        }

        private void customClassConfigB_Click(object sender, EventArgs e)
        {
            try
            {
                CustomClass.ShowConfigurationCustomClass(Application.StartupPath + "\\CustomClasses\\" + customClass.Text);
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
                if (blackListHarvest.SelectedIndex >= 0)
                    blackListHarvest.Items.RemoveAt(blackListHarvest.SelectedIndex);
            }
            catch (Exception ex)
            {
                Logging.WriteError("GeneralSettings > delBlackListHarvest_Click(object sender, EventArgs e): " + ex);

            }
        }
    }
}