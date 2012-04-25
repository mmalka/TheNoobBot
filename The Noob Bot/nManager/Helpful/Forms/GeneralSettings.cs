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

                LoadSetting(nManagerSetting.CurrentSetting);

                foreach (var f in Others.GetFilesDirectory(Application.StartupPath + "\\CustomClasses\\", "*.cs"))
                {
                    customClass.Items.Add(f);
                }
                listHarvestCb.DropDownStyle = ComboBoxStyle.DropDownList;
                foreach (var n in NodesList.LoadList())
                {
                    listHarvestCb.Items.Add(n.Id + " - " + n.Name);
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("GeneralSettings > GeneralSettings(): " + e);

            }
        }
        void TranslateForm()
        {
            labelX45.Text = Translate.Get(Translate.Id.Security) + ":";
            labelX34.Text = Translate.Get(Translate.Id.Close_game) + ":";
            labelX30.Text = Translate.Get(Translate.Id.If_reached______Honor_Points);
            labelX28.Text = Translate.Get(Translate.Id.After);
            labelX26.Text = Translate.Get(Translate.Id.After);
            labelX24.Text = Translate.Get(Translate.Id.After);
            addBlackListHarvest.Text = Translate.Get(Translate.Id.Add);
            expandablePanel7.TitleText = Translate.Get(Translate.Id.Other_options);
            labelX60.Text = Translate.Get(Translate.Id.Npc_Mailbox_Search_Radius);
            labelX42.Text = Translate.Get(Translate.Id.Use_Paths_Finder);
            expandablePanel5.TitleText = Translate.Get(Translate.Id.Stop_game___tnb___Security);
            labelX39.Text = Translate.Get(Translate.Id.Song_if_New_Whisper);
            labelX33.Text = Translate.Get(Translate.Id.Min);
            labelX43.Text = Translate.Get(Translate.Id.Record_whisper_in_Log_file);
            labelX32.Text = Translate.Get(Translate.Id.Blockages);
            labelX31.Text = Translate.Get(Translate.Id.Level);
            labelX29.Text = Translate.Get(Translate.Id.If_Player_Teleported);
            labelX44.Text = Translate.Get(Translate.Id.Pause_tnb_if_Nearby_Player);
            labelX25.Text = Translate.Get(Translate.Id.If_Whisper_bigger_or_equal_to);
            labelX27.Text = Translate.Get(Translate.Id.If_full_Bag);
            expandablePanel9.TitleText = Translate.Get(Translate.Id.Mail__Send_Items_to_alts);
            labelX56.Text = Translate.Get(Translate.Id.Mail_Recipient);
            labelX48.Text = Translate.Get(Translate.Id.Force_Mail_List__one_item_by_line);
            mailPurple.Text = Translate.Get(Translate.Id.Mail_Purple_items);
            mailBlue.Text = Translate.Get(Translate.Id.Mail_Blue_items);
            mailGreen.Text = Translate.Get(Translate.Id.Mail_Green_items);
            mailWhite.Text = Translate.Get(Translate.Id.Mail_White_items);
            mailGray.Text = Translate.Get(Translate.Id.Mail_Gray_items);
            labelX54.Text = Translate.Get(Translate.Id.Subject);
            labelX55.Text = Translate.Get(Translate.Id.Use_Mail);
            labelX58.Text = Translate.Get(Translate.Id.Do_not_Mail_List__one_item_by_line);
            expandablePanel8.TitleText = Translate.Get(Translate.Id.Vendor__Selling_or_Buying);
            labelX53.Text = Translate.Get(Translate.Id.Force_Sell_List__one_item_by_line);
            sellPurple.Text = Translate.Get(Translate.Id.Sell_Purple_items);
            sellBlue.Text = Translate.Get(Translate.Id.Sell_Blue_items);
            sellGreen.Text = Translate.Get(Translate.Id.Sell_Green_items);
            sellWhite.Text = Translate.Get(Translate.Id.Sell_White_items);
            sellGray.Text = Translate.Get(Translate.Id.Sell_Gray_items);
            labelX52.Text = Translate.Get(Translate.Id.Selling);
            labelX51.Text = Translate.Get(Translate.Id.Repair);
            labelX50.Text = Translate.Get(Translate.Id.Food_Amount);
            labelX41.Text = Translate.Get(Translate.Id.Drink_Amount);
            labelX46.Text = Translate.Get(Translate.Id.Do_not_Sell_List__one_item_by_line);
            expandablePanel6.TitleText = Translate.Get(Translate.Id.Relogger);
            labelX67.Text = Translate.Get(Translate.Id.BattleNet_Account);
            labelX38.Text = Translate.Get(Translate.Id.Relogger);
            labelX37.Text = Translate.Get(Translate.Id.Account_Password);
            labelX40.Text = Translate.Get(Translate.Id.Account_Email);
            expandablePanel4.TitleText = Translate.Get(Translate.Id.Looting____Farming_options);
            labelX65.Text = Translate.Get(Translate.Id.Prospecting_only_in_town);
            labelX64.Text = Translate.Get(Translate.Id.Prospecting_Every__in_minute);
            labelX63.Text = Translate.Get(Translate.Id.Prospecting);
            labelX62.Text = Translate.Get(Translate.Id.Prospecting_list__one_item_by_line);
            labelX61.Text = Translate.Get(Translate.Id.Smelting);
            delBlackListHarvest.Text = Translate.Get(Translate.Id.Del);
            labelX36.Text = Translate.Get(Translate.Id.Don_t_harvest);
            labelX35.Text = Translate.Get(Translate.Id.Harvest_During_Long_Move);
            labelX23.Text = Translate.Get(Translate.Id.Ninja);
            labelX22.Text = Translate.Get(Translate.Id.Search_Radius);
            labelX21.Text = Translate.Get(Translate.Id.Max_Units_Near);
            labelX20.Text = Translate.Get(Translate.Id.Harvest_Herbs);
            labelX19.Text = Translate.Get(Translate.Id.Harvest_Minerals);
            labelX17.Text = Translate.Get(Translate.Id.Skin_Mobs);
            labelX16.Text = Translate.Get(Translate.Id.Loot_Chests);
            labelX12.Text = Translate.Get(Translate.Id.Harvest_Avoid_Players_Radius);
            labelX18.Text = Translate.Get(Translate.Id.Loot_Mobs);
            expandablePanel3.TitleText = Translate.Get(Translate.Id.Food___Drink____percentage_to_be_used);
            labelX10.Text = Translate.Get(Translate.Id.on);
            labelX13.Text = Translate.Get(Translate.Id.on);
            labelX15.Text = Translate.Get(Translate.Id.Drink);
            labelX11.Text = Translate.Get(Translate.Id.Food);
            expandablePanel2.TitleText = Translate.Get(Translate.Id.Mount_options);
            labelX66.Text = Translate.Get(Translate.Id.Aquatic);
            labelX57.Text = Translate.Get(Translate.Id.Ignore_Fight_if_in_Gound_Mount);
            labelX8.Text = Translate.Get(Translate.Id.Mount_Distance);
            labelX7.Text = Translate.Get(Translate.Id.Flying);
            labelX6.Text = Translate.Get(Translate.Id.Ground);
            labelX5.Text = Translate.Get(Translate.Id.Use_Ground_Mount);
            expandablePanel1.TitleText = Translate.Get(Translate.Id.Class___Custom_spell_sequence_Settings);
            labelX59.Text = Translate.Get(Translate.Id.Use_Spirit_Healer);
            labelX49.Text = Translate.Get(Translate.Id.Train_New_Spells);
            labelX47.Text = Translate.Get(Translate.Id.Train_New_Skills);
            labelX4.Text = Translate.Get(Translate.Id.Don_t_start_fighting);
            labelX3.Text = Translate.Get(Translate.Id.Can_attack_units_already_in_fight);
            labelX2.Text = Translate.Get(Translate.Id.Assign_Talents);
            labelX1.Text = Translate.Get(Translate.Id.Custom_Class);
            customClassConfigB.Text = Translate.Get(Translate.Id.Settings);
            closeB.Text = Translate.Get(Translate.Id.Close_without_save);
            resetB.Text = Translate.Get(Translate.Id.Reset_Settings);
            saveAndCloseB.Text = Translate.Get(Translate.Id.Save_and_Close);
            labelX68.Text = Translate.Get(Translate.Id.Auto_Make_Elemental);
            Text = Translate.Get(Translate.Id.General_Settings);
            
        }

        void SaveSetting()
        {
            try
            {
                nManagerSetting.CurrentSetting.customClass = customClass.Text;
                nManagerSetting.CurrentSetting.assignTalents = assignTalents.Value;
                nManagerSetting.CurrentSetting.trainNewSkills = trainNewSkills.Value;
                nManagerSetting.CurrentSetting.trainNewSpells = trainNewSpells.Value;
                nManagerSetting.CurrentSetting.canAttackUnitsAlreadyInFight = canAttackUnitsAlreadyInFight.Value;
                nManagerSetting.CurrentSetting.dontStartFighting = dontStartFighting.Value;
                nManagerSetting.CurrentSetting.useSpiritHealer = useSpiritHealer.Value;
                nManagerSetting.CurrentSetting.useGroundMount = useGroundMount.Value;
                nManagerSetting.CurrentSetting.GroundMountName = GroundMountName.Text;
                nManagerSetting.CurrentSetting.mountDistance = mountDistance.Value;
                nManagerSetting.CurrentSetting.ignoreFightGoundMount = ignoreFightGoundMount.Value;
                nManagerSetting.CurrentSetting.FlyingMountName = FlyingMountName.Text;
                nManagerSetting.CurrentSetting.AquaticMountName = AquaticMountName.Text;
                nManagerSetting.CurrentSetting.foodName = foodName.Text;
                nManagerSetting.CurrentSetting.foodPercent = foodPercent.Value;
                nManagerSetting.CurrentSetting.drinkName = drinkName.Text;
                nManagerSetting.CurrentSetting.drinkPercent = drinkPercent.Value;
                nManagerSetting.CurrentSetting.restingMana = restingMana.Value;
                nManagerSetting.CurrentSetting.lootMobs = lootMobs.Value;
                nManagerSetting.CurrentSetting.lootChests = lootChests.Value;
                nManagerSetting.CurrentSetting.skinMobs = skinMobs.Value;
                nManagerSetting.CurrentSetting.skinNinja = skinNinja.Value;
                nManagerSetting.CurrentSetting.harvestMinerals = harvestMinerals.Value;
                nManagerSetting.CurrentSetting.harvestHerbs = harvestHerbs.Value;
                nManagerSetting.CurrentSetting.harvestAvoidPlayersRadius = harvestAvoidPlayersRadius.Value;
                nManagerSetting.CurrentSetting.maxUnitsNear = maxUnitsNear.Value;
                nManagerSetting.CurrentSetting.searchRadius = searchRadius.Value;
                nManagerSetting.CurrentSetting.harvestDuringLongMove = harvestDuringLongMove.Value;
                nManagerSetting.CurrentSetting.smelting = smelting.Value;
                nManagerSetting.CurrentSetting.prospecting = prospecting.Value;
                nManagerSetting.CurrentSetting.prospectingTime = prospectingTime.Value;
                nManagerSetting.CurrentSetting.prospectingInTown = prospectingInTown.Value;
                nManagerSetting.CurrentSetting.prospectingList.Clear();
                nManagerSetting.CurrentSetting.prospectingList.AddRange(Others.TextToArrayByLine(prospectingList.Text));
                nManagerSetting.CurrentSetting.blackListHarvest.Clear();
                try
                {
                    foreach (string i in blackListHarvest.Items)
                    {
                        try
                        {
                            string[] result = i.Replace(" ", "").Split(Convert.ToChar("-"));
                            if (result.Length > 0)
                                nManagerSetting.CurrentSetting.blackListHarvest.Add(Convert.ToInt32(result[0]));
                        }
                        catch { }
                    }
                }
                catch { }
                nManagerSetting.CurrentSetting.autoMakeElemental = autoMakeElemental.Value;
                nManagerSetting.CurrentSetting.reloger = reloger.Value;
                nManagerSetting.CurrentSetting.accountEmail = accountEmail.Text;
                nManagerSetting.CurrentSetting.accountPassword = accountPassword.Text;
                nManagerSetting.CurrentSetting.bNetName = bNetName.Text;
                //nManagerSetting.CurrentSetting.characterName = characterName.Text;
                //nManagerSetting.CurrentSetting.realmServer = realmServer.Text;
                nManagerSetting.CurrentSetting.foodAmount = foodAmount.Value;
                nManagerSetting.CurrentSetting.drinkAmount = drinkAmount.Value;
                nManagerSetting.CurrentSetting.repair = repair.Value;
                nManagerSetting.CurrentSetting.selling = selling.Value;
                nManagerSetting.CurrentSetting.sellGray = sellGray.Checked;
                nManagerSetting.CurrentSetting.sellWhite = sellWhite.Checked;
                nManagerSetting.CurrentSetting.sellGreen = sellGreen.Checked;
                nManagerSetting.CurrentSetting.sellBlue = sellBlue.Checked;
                nManagerSetting.CurrentSetting.sellPurple = sellPurple.Checked;
                nManagerSetting.CurrentSetting.doNotSellList.Clear();
                nManagerSetting.CurrentSetting.doNotSellList.AddRange(Others.TextToArrayByLine(doNotSellList.Text));
                nManagerSetting.CurrentSetting.forceSellList.Clear();
                nManagerSetting.CurrentSetting.forceSellList.AddRange(Others.TextToArrayByLine(forceSellList.Text));
                nManagerSetting.CurrentSetting.useMail = useMail.Value;
                nManagerSetting.CurrentSetting.mailRecipient = mailRecipient.Text;
                nManagerSetting.CurrentSetting.mailSubject = mailSubject.Text;
                nManagerSetting.CurrentSetting.mailGray = mailGray.Checked;
                nManagerSetting.CurrentSetting.mailWhite = mailWhite.Checked;
                nManagerSetting.CurrentSetting.mailGreen = mailGreen.Checked;
                nManagerSetting.CurrentSetting.mailBlue = mailBlue.Checked;
                nManagerSetting.CurrentSetting.mailPurple = mailPurple.Checked;
                nManagerSetting.CurrentSetting.forceMailList.Clear();
                nManagerSetting.CurrentSetting.forceMailList.AddRange(Others.TextToArrayByLine(forceMailList.Text));
                nManagerSetting.CurrentSetting.doNotMailList.Clear();
                nManagerSetting.CurrentSetting.doNotMailList.AddRange(Others.TextToArrayByLine(doNotMailList.Text));
                nManagerSetting.CurrentSetting.closeIfFullBag = closeIfFullBag.Value;
                nManagerSetting.CurrentSetting.closeIfReached4000HonorPoints = closeIfReached4000HonorPoints.Value;
                nManagerSetting.CurrentSetting.closeIfPlayerTeleported = closeIfPlayerTeleported.Value;
                nManagerSetting.CurrentSetting.closeAfterXLevel = closeAfterXLevel.Value;
                nManagerSetting.CurrentSetting.closeIfWhisperBiggerOrEgalAt = closeIfWhisperBiggerOrEgalAt.Value;
                nManagerSetting.CurrentSetting.closeAfterXBlockages = closeAfterXBlockages.Value;
                nManagerSetting.CurrentSetting.closeAfterXMin = closeAfterXMin.Value;
                nManagerSetting.CurrentSetting.securityPauseBotIfNerbyPlayer = securityPauseBotIfNerbyPlayer.Value;
                nManagerSetting.CurrentSetting.securityRecordWhisperInLogFile = securityRecordWhisperInLogFile.Value;
                nManagerSetting.CurrentSetting.securitySongIfNewWhisper = securitySongIfNewWhisper.Value;
                nManagerSetting.CurrentSetting.usePathsFinder = usePathsFinder.Value;
                nManagerSetting.CurrentSetting.npcMailboxSearchRadius = npcMailboxSearchRadius.Value;
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
                customClass.Text = managerSetting.customClass;
                assignTalents.Value = managerSetting.assignTalents;
                trainNewSkills.Value = managerSetting.trainNewSkills;
                trainNewSpells.Value = managerSetting.trainNewSpells;
                canAttackUnitsAlreadyInFight.Value = managerSetting.canAttackUnitsAlreadyInFight;
                dontStartFighting.Value = managerSetting.dontStartFighting;
                useSpiritHealer.Value = managerSetting.useSpiritHealer;
                useGroundMount.Value = managerSetting.useGroundMount;
                GroundMountName.Text = managerSetting.GroundMountName;
                mountDistance.Value = (int)managerSetting.mountDistance;
                ignoreFightGoundMount.Value = managerSetting.ignoreFightGoundMount;
                FlyingMountName.Text = managerSetting.FlyingMountName;
                AquaticMountName.Text = managerSetting.AquaticMountName;
                foodName.Text = managerSetting.foodName;
                foodPercent.Value = managerSetting.foodPercent;
                drinkName.Text = managerSetting.drinkName;
                drinkPercent.Value = managerSetting.drinkPercent;
                restingMana.Value = managerSetting.restingMana;
                lootMobs.Value = managerSetting.lootMobs;
                lootChests.Value = managerSetting.lootChests;
                skinMobs.Value = managerSetting.skinMobs;
                skinNinja.Value = managerSetting.skinNinja;
                harvestMinerals.Value = managerSetting.harvestMinerals;
                harvestHerbs.Value = managerSetting.harvestHerbs;
                harvestAvoidPlayersRadius.Value = (int)managerSetting.harvestAvoidPlayersRadius;
                maxUnitsNear.Value = managerSetting.maxUnitsNear;
                searchRadius.Value = (int)managerSetting.searchRadius;
                harvestDuringLongMove.Value = managerSetting.harvestDuringLongMove;
                smelting.Value = managerSetting.smelting;
                prospecting.Value = managerSetting.prospecting;
                prospectingInTown.Value = managerSetting.prospectingInTown;
                prospectingTime.Value = managerSetting.prospectingTime;
                prospectingList.Text = Others.ArrayToTextByLine(managerSetting.prospectingList.ToArray());
                blackListHarvest.Items.Clear();
                try
                {
                    foreach (var id in managerSetting.blackListHarvest)
                    {
                        if (id >= 0)
                            blackListHarvest.Items.Add(id + " - " + NodesList.GetNameById(id));
                    }
                }
                catch{}
                autoMakeElemental.Value = managerSetting.autoMakeElemental;
                reloger.Value = managerSetting.reloger;
                accountEmail.Text = managerSetting.accountEmail;
                accountPassword.Text = managerSetting.accountPassword;
                bNetName.Text = managerSetting.bNetName;
                //characterName.Text = managerSetting.characterName;
                //realmServer.Text = managerSetting.realmServer;
                foodAmount.Value = managerSetting.foodAmount;
                drinkAmount.Value = managerSetting.drinkAmount;
                repair.Value = managerSetting.repair;
                selling.Value = managerSetting.selling;
                sellGray.Checked = managerSetting.sellGray;
                sellWhite.Checked = managerSetting.sellWhite;
                sellGreen.Checked = managerSetting.sellGreen;
                sellBlue.Checked = managerSetting.sellBlue;
                sellPurple.Checked = managerSetting.sellPurple;
                doNotSellList.Text = Others.ArrayToTextByLine(managerSetting.doNotSellList.ToArray());
                forceSellList.Text = Others.ArrayToTextByLine(managerSetting.forceSellList.ToArray());
                useMail.Value = managerSetting.useMail;
                mailRecipient.Text = managerSetting.mailRecipient;
                mailSubject.Text = managerSetting.mailSubject;
                mailGray.Checked = managerSetting.mailGray;
                mailWhite.Checked = managerSetting.mailWhite;
                mailGreen.Checked = managerSetting.mailGreen;
                mailBlue.Checked = managerSetting.mailBlue;
                mailPurple.Checked = managerSetting.mailPurple;
                forceMailList.Text = Others.ArrayToTextByLine(managerSetting.forceMailList.ToArray());
                doNotMailList.Text = Others.ArrayToTextByLine(managerSetting.doNotMailList.ToArray());
                closeIfFullBag.Value = managerSetting.closeIfFullBag;
                closeIfReached4000HonorPoints.Value = managerSetting.closeIfReached4000HonorPoints;
                closeIfPlayerTeleported.Value = managerSetting.closeIfPlayerTeleported;
                closeAfterXLevel.Value = managerSetting.closeAfterXLevel;
                closeIfWhisperBiggerOrEgalAt.Value = managerSetting.closeIfWhisperBiggerOrEgalAt;
                closeAfterXBlockages.Value = managerSetting.closeAfterXBlockages;
                closeAfterXMin.Value = managerSetting.closeAfterXMin;
                securityPauseBotIfNerbyPlayer.Value = managerSetting.securityPauseBotIfNerbyPlayer;
                securityRecordWhisperInLogFile.Value = managerSetting.securityRecordWhisperInLogFile;
                securitySongIfNewWhisper.Value = managerSetting.securitySongIfNewWhisper;
                usePathsFinder.Value = managerSetting.usePathsFinder;
                npcMailboxSearchRadius.Value = (int)nManagerSetting.CurrentSetting.npcMailboxSearchRadius;
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

        private void addBlackListHarvest_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(listHarvestCb.Text))
                    blackListHarvest.Items.Add(listHarvestCb.Text);
            }
            catch (Exception ex)
            {
                Logging.WriteError("GeneralSettings > addBlackListHarvest_Click(object sender, EventArgs e): " + ex);

            }
        }
    }
}