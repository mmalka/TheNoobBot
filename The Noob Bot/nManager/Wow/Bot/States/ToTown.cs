using System.Collections.Generic;
using System.Threading;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Bot.Tasks;
using nManager.Wow.Class;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;
using Timer = nManager.Helpful.Timer;

namespace nManager.Wow.Bot.States
{
    public class ToTown : State
    {
        public override string DisplayName
        {
            get { return "To Town"; }
        }

        public override int Priority { get; set; }

        public override List<State> NextStates
        {
            get { return new List<State>(); }
        }

        public override List<State> BeforeStates
        {
            get { return new List<State> {new SmeltingState {Priority = 1}}; }
        }

        private readonly Spell _grandExpeditionYak = new Spell(122708);
        private readonly Spell _travelersTundraMammoth = new Spell(61425);
        private bool _magicMountYak;
        private bool _magicMountMammoth;
        private bool _useMollE;
        private bool _use74A;
        private bool _use110G;
        private bool _useJeeves;

        public override bool NeedToRun
        {
            get
            {
                if (!nManagerSetting.CurrentSetting.ActivateAutoRepairFeature &&
                    !nManagerSetting.CurrentSetting.ActivateAutoMaillingFeature &&
                    !nManagerSetting.CurrentSetting.ActivateAutoSellingFeature &&
                    (nManagerSetting.CurrentSetting.NumberOfBeverageWeGot == 0 ||
                     nManagerSetting.CurrentSetting.BeverageName == "") &&
                    (nManagerSetting.CurrentSetting.NumberOfFoodsWeGot == 0 ||
                     nManagerSetting.CurrentSetting.FoodName == ""))
                    return false;

                if (!Usefuls.InGame ||
                    Usefuls.IsLoadingOrConnecting ||
                    ObjectManager.ObjectManager.Me.IsDeadMe ||
                    !ObjectManager.ObjectManager.Me.IsValid ||
                    (ObjectManager.ObjectManager.Me.InCombat &&
                     !(ObjectManager.ObjectManager.Me.IsMounted &&
                       (nManagerSetting.CurrentSetting.IgnoreFightIfMounted || Usefuls.IsFlying))) ||
                    !Products.Products.IsStarted)
                    return false;

                if (_grandExpeditionYak != null &&
                    (SpellManager.ExistMountLUA(_grandExpeditionYak.NameInGame) || _grandExpeditionYak.KnownSpell))
                    _magicMountYak = true;

                if (_travelersTundraMammoth != null &&
                    (SpellManager.ExistMountLUA(_travelersTundraMammoth.NameInGame) ||
                     _travelersTundraMammoth.KnownSpell))
                    _magicMountMammoth = true;

                if ((nManagerSetting.CurrentSetting.UseMollE && ItemsManager.GetItemCountByIdLUA(40768) > 0 &&
                     !ItemsManager.IsItemOnCooldown(40768) && ItemsManager.IsUsableItemById(40768)))
                    _useMollE = true;
                else
                    _useMollE = false;

                if ((nManagerSetting.CurrentSetting.UseRobot && ItemsManager.GetItemCountByIdLUA(18232) > 0 &&
                     !ItemsManager.IsItemOnCooldown(18232) && ItemsManager.IsUsableItemById(18232)))
                {
                    _use74A = true;
                    _use110G = false;
                    _useJeeves = false;
                }
                else if ((nManagerSetting.CurrentSetting.UseRobot && ItemsManager.GetItemCountByIdLUA(34113) > 0 &&
                          !ItemsManager.IsItemOnCooldown(34113) && ItemsManager.IsUsableItemById(34113)))
                {
                    _use74A = false;
                    _use110G = true;
                    _useJeeves = false;
                }
                else if ((nManagerSetting.CurrentSetting.UseRobot && ItemsManager.GetItemCountByIdLUA(49040) > 0 &&
                          !ItemsManager.IsItemOnCooldown(49040) && ItemsManager.IsUsableItemById(49040)))
                {
                    _use74A = false;
                    _use110G = false;
                    _useJeeves = true;
                }
                else
                {
                    _use74A = false;
                    _use110G = false;
                    _useJeeves = false;
                }

                if (ObjectManager.ObjectManager.Me.GetDurability <=
                    nManagerSetting.CurrentSetting.RepairWhenDurabilityIsUnderPercent &&
                    (NpcDB.GetNpcNearby(Npc.NpcType.Repair).Entry > 0 || (MountTask.GetMountCapacity() >= MountCapacity.Ground && (_magicMountMammoth || _magicMountYak)) || _use74A ||
                     _use110G || _useJeeves) && nManagerSetting.CurrentSetting.ActivateAutoRepairFeature)
                    return true;

                if (Usefuls.GetContainerNumFreeSlots <= nManagerSetting.CurrentSetting.SellItemsWhenLessThanXSlotLeft &&
                    (NpcDB.GetNpcNearby(Npc.NpcType.Vendor).Entry > 0 || (MountTask.GetMountCapacity() >= MountCapacity.Ground && (_magicMountMammoth || _magicMountYak)) || _use74A ||
                     _use110G || _useJeeves) && nManagerSetting.CurrentSetting.ActivateAutoSellingFeature)
                    return true;

                if (Usefuls.GetContainerNumFreeSlots <= nManagerSetting.CurrentSetting.SendMailWhenLessThanXSlotLeft &&
                    (NpcDB.GetNpcNearby(Npc.NpcType.Mailbox).Entry > 0 || _useMollE) &&
                    nManagerSetting.CurrentSetting.ActivateAutoMaillingFeature &&
                    nManagerSetting.CurrentSetting.MaillingFeatureRecipient != string.Empty)
                    return true;

                return false;
            }
        }

        public override void Run()
        {
            MovementManager.StopMove();
            var listNPCs = new List<Npc>();
            Npc mailBox = null;

            // If we need to send items.
            if (nManagerSetting.CurrentSetting.ActivateAutoMaillingFeature &&
                nManagerSetting.CurrentSetting.MaillingFeatureRecipient != string.Empty)
            {
                if (_useMollE)
                {
                    MountTask.DismountMount();
                    ItemsManager.UseItem(ItemsManager.GetNameById(40768));
                    Thread.Sleep(2000);
                    var portableMailbox = ObjectManager.ObjectManager.GetNearestWoWGameObject(
                        ObjectManager.ObjectManager.GetWoWGameObjectById(191605));
                    if (portableMailbox.IsValid &&
                        portableMailbox.CreatedBy == ObjectManager.ObjectManager.Me.GetBaseAddress)
                    {
                        mailBox = new Npc
                            {
                                Entry = portableMailbox.Entry,
                                Position = portableMailbox.Position,
                                Name = portableMailbox.Name,
                                ContinentId = (ContinentId) Usefuls.ContinentId,
                                Faction = ObjectManager.ObjectManager.Me.PlayerFaction.ToLower() == "horde"
                                              ? Npc.FactionType.Horde
                                              : Npc.FactionType.Alliance,
                                SelectGossipOption = 0,
                                Type = Npc.NpcType.Mailbox
                            };
                    }
                }
                if (mailBox == null && NpcDB.GetNpcNearby(Npc.NpcType.Mailbox).Entry > 0)
                    mailBox = NpcDB.GetNpcNearby(Npc.NpcType.Mailbox);
            }
            // If we need to repair.
            if (ObjectManager.ObjectManager.Me.GetDurability <=
                nManagerSetting.CurrentSetting.RepairWhenDurabilityIsUnderPercent &&
                nManagerSetting.CurrentSetting.ActivateAutoRepairFeature)
            {
                if (_magicMountMammoth && MountTask.GetMountCapacity() >= MountCapacity.Ground)
                {
                    if (_travelersTundraMammoth.IsSpellUsable)
                    {
                        MountTask.DismountMount();
                        _travelersTundraMammoth.Launch(true, true, true);
                        Thread.Sleep(2000);
                        if (ObjectManager.ObjectManager.Me.PlayerFaction.ToLower() == "horde")
                        {
                            var drixBlackwrench =
                                ObjectManager.ObjectManager.GetNearestWoWUnit(
                                    ObjectManager.ObjectManager.GetWoWUnitByEntry(32641));
                            if (drixBlackwrench.IsValid && drixBlackwrench.IsAlive)
                            {
                                var drixBlackwrenchNpc = new Npc
                                    {
                                        Entry = drixBlackwrench.Entry,
                                        Position = drixBlackwrench.Position,
                                        Name = drixBlackwrench.Name,
                                        ContinentId = (ContinentId) Usefuls.ContinentId,
                                        Faction = Npc.FactionType.Horde,
                                        SelectGossipOption = 0,
                                        Type = Npc.NpcType.Repair
                                    };
                                listNPCs.Add(drixBlackwrenchNpc);
                            }
                        }
                        else
                        {
                            var gnimo =
                                ObjectManager.ObjectManager.GetNearestWoWUnit(
                                    ObjectManager.ObjectManager.GetWoWUnitByEntry(32639));
                            if (gnimo.IsValid && gnimo.IsAlive)
                            {
                                var gnimoNpc = new Npc
                                    {
                                        Entry = gnimo.Entry,
                                        Position = gnimo.Position,
                                        Name = gnimo.Name,
                                        ContinentId = (ContinentId) Usefuls.ContinentId,
                                        Faction = Npc.FactionType.Alliance,
                                        SelectGossipOption = 0,
                                        Type = Npc.NpcType.Repair
                                    };
                                listNPCs.Add(gnimoNpc);
                            }
                        }
                    }
                }
                else if (_magicMountYak && MountTask.GetMountCapacity() >= MountCapacity.Ground)
                {
                    if (_grandExpeditionYak.IsSpellUsable)
                    {
                        MountTask.DismountMount();
                        _grandExpeditionYak.Launch(true, true, true);
                        Thread.Sleep(2000);
                        var cousinSlowhands =
                            ObjectManager.ObjectManager.GetNearestWoWUnit(
                                ObjectManager.ObjectManager.GetWoWUnitByEntry(62822));
                        if (cousinSlowhands.IsValid && cousinSlowhands.IsAlive)
                        {
                            var cousinSlowhandsNpc = new Npc
                                {
                                    Entry = cousinSlowhands.Entry,
                                    Position = cousinSlowhands.Position,
                                    Name = cousinSlowhands.Name,
                                    ContinentId = (ContinentId) Usefuls.ContinentId,
                                    Faction = ObjectManager.ObjectManager.Me.PlayerFaction.ToLower() == "horde"
                                                  ? Npc.FactionType.Horde
                                                  : Npc.FactionType.Alliance,
                                    SelectGossipOption = 0,
                                    Type = Npc.NpcType.Repair
                                };
                            listNPCs.Add(cousinSlowhandsNpc);
                        }
                    }
                }
                else if (_use74A)
                {
                    MountTask.DismountMount();
                    ItemsManager.UseItem(ItemsManager.GetNameById(18232));
                    Thread.Sleep(2000);
                    var unitA =
                        ObjectManager.ObjectManager.GetNearestWoWUnit(
                            ObjectManager.ObjectManager.GetWoWUnitByEntry(14337));
                    if (unitA.IsValid && unitA.IsAlive)
                    {
                        var npcA = new Npc
                            {
                                Entry = unitA.Entry,
                                Position = unitA.Position,
                                Name = unitA.Name,
                                ContinentId = (ContinentId) Usefuls.ContinentId,
                                Faction = ObjectManager.ObjectManager.Me.PlayerFaction.ToLower() == "horde"
                                              ? Npc.FactionType.Horde
                                              : Npc.FactionType.Alliance,
                                SelectGossipOption = 0,
                                Type = Npc.NpcType.Repair
                            };
                        listNPCs.Add(npcA);
                    }
                }
                else if (_use110G)
                {
                    MountTask.DismountMount();
                    ItemsManager.UseItem(ItemsManager.GetNameById(34113));
                    Thread.Sleep(2000);
                    var unitG =
                        ObjectManager.ObjectManager.GetNearestWoWUnit(
                            ObjectManager.ObjectManager.GetWoWUnitByEntry(24780));
                    if (unitG.IsValid && unitG.IsAlive)
                    {
                        var npcG = new Npc
                            {
                                Entry = unitG.Entry,
                                Position = unitG.Position,
                                Name = unitG.Name,
                                ContinentId = (ContinentId) Usefuls.ContinentId,
                                Faction = ObjectManager.ObjectManager.Me.PlayerFaction.ToLower() == "horde"
                                              ? Npc.FactionType.Horde
                                              : Npc.FactionType.Alliance,
                                SelectGossipOption = 0,
                                Type = Npc.NpcType.Repair
                            };
                        listNPCs.Add(npcG);
                    }
                }
                else if (_useJeeves)
                {
                    MountTask.DismountMount();
                    ItemsManager.UseItem(ItemsManager.GetNameById(49040));
                    Thread.Sleep(2000);
                    var unitJeeves =
                        ObjectManager.ObjectManager.GetNearestWoWUnit(
                            ObjectManager.ObjectManager.GetWoWUnitByEntry(35642));
                    if (unitJeeves.IsValid && unitJeeves.IsAlive)
                    {
                        var npcJeeves = new Npc
                            {
                                Entry = unitJeeves.Entry,
                                Position = unitJeeves.Position,
                                Name = unitJeeves.Name,
                                ContinentId = (ContinentId) Usefuls.ContinentId,
                                Faction = ObjectManager.ObjectManager.Me.PlayerFaction.ToLower() == "horde"
                                              ? Npc.FactionType.Horde
                                              : Npc.FactionType.Alliance,
                                SelectGossipOption = 2,
                                Type = Npc.NpcType.Repair
                            };
                        listNPCs.Add(npcJeeves);
                    }
                }
                else
                {
                    if (NpcDB.GetNpcNearby(Npc.NpcType.Repair).Entry > 0)
                        listNPCs.Add(NpcDB.GetNpcNearby(Npc.NpcType.Repair));
                }
            }

            // If we need to sell.
            if (NeedFoodSupplies() || NeedDrinkSupplies() ||
                Usefuls.GetContainerNumFreeSlots <= nManagerSetting.CurrentSetting.SellItemsWhenLessThanXSlotLeft &&
                nManagerSetting.CurrentSetting.ActivateAutoSellingFeature)
            {
                if (_magicMountMammoth && MountTask.GetMountCapacity() >= MountCapacity.Ground)
                {
                    if (_travelersTundraMammoth.IsSpellUsable)
                    {
                        MountTask.DismountMount();
                        _travelersTundraMammoth.Launch(true, true, true);
                        Thread.Sleep(2000);
                        if (ObjectManager.ObjectManager.Me.PlayerFaction.ToLower() == "horde")
                        {
                            var mojodishu =
                                ObjectManager.ObjectManager.GetNearestWoWUnit(
                                    ObjectManager.ObjectManager.GetWoWUnitByEntry(32642));
                            if (mojodishu.IsValid && mojodishu.IsAlive)
                            {
                                var mojodishuNpc = new Npc
                                    {
                                        Entry = mojodishu.Entry,
                                        Position = mojodishu.Position,
                                        Name = mojodishu.Name,
                                        ContinentId = (ContinentId) Usefuls.ContinentId,
                                        Faction = Npc.FactionType.Horde,
                                        SelectGossipOption = 0,
                                        Type = Npc.NpcType.Vendor
                                    };
                                listNPCs.Add(mojodishuNpc);
                            }
                        }
                        else
                        {
                            var hakmuddArgus =
                                ObjectManager.ObjectManager.GetNearestWoWUnit(
                                    ObjectManager.ObjectManager.GetWoWUnitByEntry(32638));
                            if (hakmuddArgus.IsValid && hakmuddArgus.IsAlive)
                            {
                                var hakmuddArgusNpc = new Npc
                                    {
                                        Entry = hakmuddArgus.Entry,
                                        Position = hakmuddArgus.Position,
                                        Name = hakmuddArgus.Name,
                                        ContinentId = (ContinentId) Usefuls.ContinentId,
                                        Faction = Npc.FactionType.Alliance,
                                        SelectGossipOption = 0,
                                        Type = Npc.NpcType.Vendor
                                    };
                                listNPCs.Add(hakmuddArgusNpc);
                            }
                        }
                    }
                }
                else if (_magicMountYak && MountTask.GetMountCapacity() >= MountCapacity.Ground)
                {
                    if (_grandExpeditionYak.IsSpellUsable)
                    {
                        MountTask.DismountMount();
                        _grandExpeditionYak.Launch(true, true, true);
                        Thread.Sleep(2000);
                        var cousinSlowhands =
                            ObjectManager.ObjectManager.GetNearestWoWUnit(
                                ObjectManager.ObjectManager.GetWoWUnitByEntry(62822));
                        if (cousinSlowhands.IsValid && cousinSlowhands.IsAlive)
                        {
                            var cousinSlowhandsNpc = new Npc
                                {
                                    Entry = cousinSlowhands.Entry,
                                    Position = cousinSlowhands.Position,
                                    Name = cousinSlowhands.Name,
                                    ContinentId = (ContinentId) Usefuls.ContinentId,
                                    Faction = Npc.FactionType.Neutral,
                                    SelectGossipOption = 0,
                                    Type = Npc.NpcType.Vendor
                                };
                            listNPCs.Add(cousinSlowhandsNpc);
                        }
                    }
                }
                else if (_use74A)
                {
                    MountTask.DismountMount();
                    ItemsManager.UseItem(ItemsManager.GetNameById(18232));
                    Thread.Sleep(2000);
                    var unitA =
                        ObjectManager.ObjectManager.GetNearestWoWUnit(
                            ObjectManager.ObjectManager.GetWoWUnitByEntry(14337));
                    if (unitA.IsValid && unitA.IsAlive)
                    {
                        var npcA = new Npc
                            {
                                Entry = unitA.Entry,
                                Position = unitA.Position,
                                Name = unitA.Name,
                                ContinentId = (ContinentId) Usefuls.ContinentId,
                                Faction = ObjectManager.ObjectManager.Me.PlayerFaction.ToLower() == "horde"
                                              ? Npc.FactionType.Horde
                                              : Npc.FactionType.Alliance,
                                SelectGossipOption = 0,
                                Type = Npc.NpcType.Vendor
                            };
                        listNPCs.Add(npcA);
                    }
                }
                else if (_use110G)
                {
                    MountTask.DismountMount();
                    ItemsManager.UseItem(ItemsManager.GetNameById(31113));
                    Thread.Sleep(2000);
                    var unitG =
                        ObjectManager.ObjectManager.GetNearestWoWUnit(
                            ObjectManager.ObjectManager.GetWoWUnitByEntry(24780));
                    if (unitG.IsValid && unitG.IsAlive)
                    {
                        var npcG = new Npc
                            {
                                Entry = unitG.Entry,
                                Position = unitG.Position,
                                Name = unitG.Name,
                                ContinentId = (ContinentId) Usefuls.ContinentId,
                                Faction = ObjectManager.ObjectManager.Me.PlayerFaction.ToLower() == "horde"
                                              ? Npc.FactionType.Horde
                                              : Npc.FactionType.Alliance,
                                SelectGossipOption = 0,
                                Type = Npc.NpcType.Vendor
                            };
                        listNPCs.Add(npcG);
                    }
                }
                else if (_useJeeves)
                {
                    MountTask.DismountMount();
                    ItemsManager.UseItem(ItemsManager.GetNameById(49040));
                    Thread.Sleep(2000);
                    var unitJeeves =
                        ObjectManager.ObjectManager.GetNearestWoWUnit(
                            ObjectManager.ObjectManager.GetWoWUnitByEntry(35642));
                    if (unitJeeves.IsValid && unitJeeves.IsAlive)
                    {
                        var npcJeeves = new Npc
                            {
                                Entry = unitJeeves.Entry,
                                Position = unitJeeves.Position,
                                Name = unitJeeves.Name,
                                ContinentId = (ContinentId) Usefuls.ContinentId,
                                Faction = ObjectManager.ObjectManager.Me.PlayerFaction.ToLower() == "horde"
                                              ? Npc.FactionType.Horde
                                              : Npc.FactionType.Alliance,
                                SelectGossipOption = 2,
                                Type = Npc.NpcType.Vendor
                            };
                        listNPCs.Add(npcJeeves);
                    }
                }
                else
                {
                    if (NpcDB.GetNpcNearby(Npc.NpcType.Vendor).Entry > 0)
                        listNPCs.Add(NpcDB.GetNpcNearby(Npc.NpcType.Vendor));
                }
            }

            #region Repairer, Seller/Buyer

            if (listNPCs.Count > 0)
            {
                foreach (var npc in listNPCs)
                {
                    //Start target finding based on Seller.
                    WoWUnit TargetIsNPC;
                    WoWObject TargetIsObject;
                    Npc Target = MovementManager.FindTarget(npc, out TargetIsNPC, out TargetIsObject);
                    //End target finding based on Seller.
                    if (!TargetIsNPC.IsValid && !TargetIsObject.IsValid)
                    {
                        // ToDo: Dynamically remove this Target from the NPC Db.
                    }
                    else
                    {
                        uint baseAddress = TargetIsNPC.IsValid ? TargetIsNPC.GetBaseAddress : TargetIsObject.GetBaseAddress;
                        DoProspectingInTown();
                        DoMillingInTown();
                        Interact.InteractGameObject(baseAddress);
                        Thread.Sleep(500);
                        if (Target.SelectGossipOption != 0)
                            Lua.LuaDoString("SelectGossipOption(" + Target.SelectGossipOption + ")");
                        Thread.Sleep(1000);

                        // NPC Repairer
                        if (Target.Type == Npc.NpcType.Repair)
                        {
                            Logging.Write("Repair items from " + Target.Name + " (" + Target.Entry + ").");
                            Vendor.RepairAllItems();
                            Thread.Sleep(1000);
                        }
                        // End NPC Repairer

                        if (Target.Type == Npc.NpcType.Vendor)
                        {
                            // NPC Buyer
                            var vQuality = new List<WoWItemQuality>();
                            if (nManagerSetting.CurrentSetting.SellGray)
                                vQuality.Add(WoWItemQuality.Poor);
                            if (nManagerSetting.CurrentSetting.SellWhite)
                                vQuality.Add(WoWItemQuality.Common);
                            if (nManagerSetting.CurrentSetting.SellGreen)
                                vQuality.Add(WoWItemQuality.Uncommon);
                            if (nManagerSetting.CurrentSetting.SellBlue)
                                vQuality.Add(WoWItemQuality.Rare);
                            if (nManagerSetting.CurrentSetting.SellPurple)
                                vQuality.Add(WoWItemQuality.Epic);
                            Vendor.SellItems(nManagerSetting.CurrentSetting.ForceToSellTheseItems,
                                             nManagerSetting.CurrentSetting.DontSellTheseItems, vQuality);
                            Logging.Write("Selling items to " + Target.Name + " (" + Target.Entry + ").");
                            Thread.Sleep(3000);
                            // End NPC Buyer

                            // NPC Seller
                            Logging.Write("Buying beverages and food from " + Target.Name + " (" + Target.Entry + ").");
                            for (int i = 0; i < 10 && NeedFoodSupplies(); i++)
                            {
                                Vendor.BuyItem(nManagerSetting.CurrentSetting.FoodName, 1);
                            }
                            for (int i = 0; i < 10 && NeedDrinkSupplies(); i++)
                            {
                                Vendor.BuyItem(nManagerSetting.CurrentSetting.BeverageName, 1);
                            }
                        }
                        // End NPC Seller
                        Lua.LuaDoString("CloseMerchant()");
                    }
                }
            }

            #endregion Repairer, Seller/Buyer

            #region Mailbox

            if (mailBox != null)
            {
                //Start target finding based on Mailbox.
                WoWUnit TargetIsNPC;
                WoWObject TargetIsObject;
                Npc Target = MovementManager.FindTarget(mailBox, out TargetIsNPC, out TargetIsObject);
                //End target finding based on Mailbox.
                if (!TargetIsNPC.IsValid && !TargetIsObject.IsValid)
                {
                    // ToDo: Dynamically remove this Target from the NPC Db.
                }
                else
                {
                    uint baseAddress = TargetIsNPC.IsValid ? TargetIsNPC.GetBaseAddress : TargetIsObject.GetBaseAddress;
                    DoProspectingInTown();
                    DoMillingInTown();
                    Interact.InteractGameObject(baseAddress);
                    Thread.Sleep(500);
                    var mQuality = new List<WoWItemQuality>();
                    if (nManagerSetting.CurrentSetting.MailGray)
                        mQuality.Add(WoWItemQuality.Poor);
                    if (nManagerSetting.CurrentSetting.MailWhite)
                        mQuality.Add(WoWItemQuality.Common);
                    if (nManagerSetting.CurrentSetting.MailGreen)
                        mQuality.Add(WoWItemQuality.Uncommon);
                    if (nManagerSetting.CurrentSetting.MailBlue)
                        mQuality.Add(WoWItemQuality.Rare);
                    if (nManagerSetting.CurrentSetting.MailPurple)
                        mQuality.Add(WoWItemQuality.Epic);

                    var MailSendingCompleted = false;
                    for (var i = 7; i > 0 && !MailSendingCompleted; i--)
                    {
                        Interact.InteractGameObject(baseAddress);
                        Thread.Sleep(1000);
                        Mail.SendMessage(nManagerSetting.CurrentSetting.MaillingFeatureRecipient,
                                         nManagerSetting.CurrentSetting.MaillingFeatureSubject, "",
                                         nManagerSetting.CurrentSetting.ForceToMailTheseItems,
                                         nManagerSetting.CurrentSetting.DontMailTheseItems, mQuality,
                                         out MailSendingCompleted);
                        Thread.Sleep(500);
                    }
                    Logging.Write("Sending items to the player " + nManagerSetting.CurrentSetting.MaillingFeatureRecipient + " using " + Target.Name + " (" + Target.Entry + ").");
                }
            }

            #endregion Mailbox
        }

        private bool NeedFoodSupplies()
        {
            if (nManagerSetting.CurrentSetting.FoodName != "" && nManagerSetting.CurrentSetting.NumberOfFoodsWeGot > 0)
            {
                if (ItemsManager.GetItemCountByNameLUA(nManagerSetting.CurrentSetting.FoodName) <
                    nManagerSetting.CurrentSetting.NumberOfFoodsWeGot)
                    return true;
            }
            return false;
        }

        private bool NeedDrinkSupplies()
        {
            if (nManagerSetting.CurrentSetting.BeverageName != "" &&
                nManagerSetting.CurrentSetting.NumberOfBeverageWeGot > 0)
            {
                if (ItemsManager.GetItemCountByNameLUA(nManagerSetting.CurrentSetting.BeverageName) <
                    nManagerSetting.CurrentSetting.NumberOfBeverageWeGot)
                    return true;
            }
            return false;
        }

        private void DoProspectingInTown()
        {
            if (!_magicMountMammoth && !_magicMountYak &&
                nManagerSetting.CurrentSetting.OnlyUseProspectingInTown &&
                nManagerSetting.CurrentSetting.ActivateAutoProspecting &&
                nManagerSetting.CurrentSetting.MineralsToProspect.Count > 0)
            {
                if (Prospecting.NeedRun(nManagerSetting.CurrentSetting.MineralsToProspect))
                {
                    var prospectingState = new ProspectingState();
                    prospectingState.Run();
                }
            }
        }

        private void DoMillingInTown()
        {
            if (!_magicMountMammoth && !_magicMountYak &&
                nManagerSetting.CurrentSetting.OnlyUseMillingInTown &&
                nManagerSetting.CurrentSetting.ActivateAutoMilling &&
                nManagerSetting.CurrentSetting.HerbsToBeMilled.Count > 0)
            {
                if (Prospecting.NeedRun(nManagerSetting.CurrentSetting.HerbsToBeMilled))
                {
                    var millingState = new MillingState();
                    millingState.Run();
                }
            }
        }
    }
}