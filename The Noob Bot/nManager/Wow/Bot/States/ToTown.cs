using System.Collections.Generic;
using System.Threading;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Bot.Tasks;
using nManager.Wow.Class;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;

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
        private readonly Spell _travelersTundraMammothAlliance = new Spell(61425);
        private readonly Spell _travelersTundraMammothHorde = new Spell(61447);
        private Spell _travelersTundraMammoth = new Spell(0);
        private bool _magicMountYak = false;
        private bool _magicMountMammoth;
        private bool _useMollE;
        private bool _use74A;
        private bool _use110G;
        private bool _useJeeves;
        private bool _suspendSelling = false;
        private bool _suspendMailing = false;

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
                    Usefuls.IsLoading ||
                    ObjectManager.ObjectManager.Me.IsDeadMe ||
                    !ObjectManager.ObjectManager.Me.IsValid ||
                    (ObjectManager.ObjectManager.Me.InCombat &&
                     !(ObjectManager.ObjectManager.Me.IsMounted &&
                       (nManagerSetting.CurrentSetting.IgnoreFightIfMounted || Usefuls.IsFlying))) ||
                    !Products.Products.IsStarted)
                    return false;

                /*if (_grandExpeditionYak != null &&
                    (SpellManager.ExistMountLUA(_grandExpeditionYak.NameInGame) || _grandExpeditionYak.KnownSpell))
                    _magicMountYak = true; */

                if (ObjectManager.ObjectManager.Me.PlayerFaction != "Horde")
                {
                    if (_travelersTundraMammothAlliance != null && (SpellManager.ExistMountLUA(_travelersTundraMammothAlliance.NameInGame) || _travelersTundraMammothAlliance.KnownSpell))
                    {
                        _magicMountMammoth = true;
                        _travelersTundraMammoth = _travelersTundraMammothAlliance;
                    }
                }
                else
                {
                    if (_travelersTundraMammothHorde != null && (SpellManager.ExistMountLUA(_travelersTundraMammothHorde.NameInGame) || _travelersTundraMammothHorde.KnownSpell))
                    {
                        _magicMountMammoth = true;
                        _travelersTundraMammoth = _travelersTundraMammothHorde;
                    }
                }

                _useMollE = false;
                WoWGameObject portableMailbox = ObjectManager.ObjectManager.GetNearestWoWGameObject(ObjectManager.ObjectManager.GetWoWGameObjectById(191605));
                if (portableMailbox.IsValid && portableMailbox.CreatedBy == ObjectManager.ObjectManager.Me.Guid)
                    _useMollE = true;
                else if ((nManagerSetting.CurrentSetting.UseMollE && ItemsManager.GetItemCount(40768) > 0 && !ItemsManager.IsItemOnCooldown(40768) &&
                          ItemsManager.IsItemUsable(40768)))
                    _useMollE = true;

                _use74A = false;
                _use110G = false;
                _useJeeves = false;
                if ((nManagerSetting.CurrentSetting.UseRobot &&
                     (DoSpawnRobot("74A", Npc.NpcType.Repair, true) != null ||
                      ItemsManager.GetItemCount(18232) > 0 && !ItemsManager.IsItemOnCooldown(18232) && ItemsManager.IsItemUsable(18232))))
                    _use74A = true;
                else if ((nManagerSetting.CurrentSetting.UseRobot &&
                          (DoSpawnRobot("110G", Npc.NpcType.Repair, true) != null ||
                           ItemsManager.GetItemCount(34113) > 0 && !ItemsManager.IsItemOnCooldown(34113) && ItemsManager.IsItemUsable(34113))))
                    _use110G = true;
                else if ((nManagerSetting.CurrentSetting.UseRobot &&
                          (DoSpawnRobot("Jeeves", Npc.NpcType.Repair, true) != null ||
                           ItemsManager.GetItemCount(49040) > 0 && !ItemsManager.IsItemOnCooldown(49040) && ItemsManager.IsItemUsable(49040))))
                    _useJeeves = true;

                if (Usefuls.GetContainerNumFreeSlots > nManagerSetting.CurrentSetting.SellItemsWhenLessThanXSlotLeft)
                    _suspendSelling = false;
                if (Usefuls.GetContainerNumFreeSlots > nManagerSetting.CurrentSetting.SendMailWhenLessThanXSlotLeft)
                    _suspendMailing = false;

                if (ObjectManager.ObjectManager.Me.GetDurability <=
                    nManagerSetting.CurrentSetting.RepairWhenDurabilityIsUnderPercent &&
                    (NpcDB.GetNpcNearby(Npc.NpcType.Repair).Entry > 0 || (MountTask.GetMountCapacity() >= MountCapacity.Ground && (_magicMountMammoth || _magicMountYak)) || _use74A ||
                     _use110G || _useJeeves) && nManagerSetting.CurrentSetting.ActivateAutoRepairFeature)
                    return true;

                if (Usefuls.GetContainerNumFreeSlots <= nManagerSetting.CurrentSetting.SellItemsWhenLessThanXSlotLeft && !_suspendSelling &&
                    (NpcDB.GetNpcNearby(Npc.NpcType.Vendor).Entry > 0 || (MountTask.GetMountCapacity() >= MountCapacity.Ground && (_magicMountMammoth || _magicMountYak)) || _use74A ||
                     _use110G || _useJeeves) && nManagerSetting.CurrentSetting.ActivateAutoSellingFeature)
                    return true;

                if (Usefuls.GetContainerNumFreeSlots <= nManagerSetting.CurrentSetting.SendMailWhenLessThanXSlotLeft && !_suspendMailing &&
                    (NpcDB.GetNpcNearby(Npc.NpcType.Mailbox).Entry > 0 || _useMollE) &&
                    nManagerSetting.CurrentSetting.ActivateAutoMaillingFeature &&
                    nManagerSetting.CurrentSetting.MaillingFeatureRecipient != string.Empty)
                    return true;

                return false;
            }
        }

        public override void Run()
        {
            List<Npc> listNPCs = new List<Npc>();
            Npc mailBox = null;
            // Stop fisher if needed
            if (Products.Products.ProductName == "Fisherbot" && FishingTask.IsLaunched)
            {
                FishingTask.StopLoopFish();
                // Then break the cast
                MovementsAction.MoveBackward(true);
                Thread.Sleep(50);
                MovementsAction.MoveBackward(false);
            }
            // If we need to send items.
            if (nManagerSetting.CurrentSetting.ActivateAutoMaillingFeature && !_suspendMailing &&
                nManagerSetting.CurrentSetting.MaillingFeatureRecipient != string.Empty &&
                Usefuls.GetContainerNumFreeSlots <= nManagerSetting.CurrentSetting.SendMailWhenLessThanXSlotLeft)
            {
                if (_useMollE)
                {
                    MountTask.DismountMount();
                    ItemsManager.UseItem(ItemsManager.GetItemNameById(40768));
                    Thread.Sleep(2000);
                    WoWGameObject portableMailbox = ObjectManager.ObjectManager.GetNearestWoWGameObject(
                        ObjectManager.ObjectManager.GetWoWGameObjectById(191605));
                    if (portableMailbox.IsValid &&
                        portableMailbox.CreatedBy == ObjectManager.ObjectManager.Me.Guid)
                    {
                        mailBox = new Npc
                        {
                            Entry = portableMailbox.Entry,
                            Position = portableMailbox.Position,
                            Name = portableMailbox.Name,
                            ContinentIdInt = Usefuls.ContinentId,
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
                listNPCs.Add(mailBox);
            }
            // If we need to repair.
            if (ObjectManager.ObjectManager.Me.GetDurability <=
                nManagerSetting.CurrentSetting.RepairWhenDurabilityIsUnderPercent &&
                nManagerSetting.CurrentSetting.ActivateAutoRepairFeature)
            {
                if (_magicMountMammoth && MountTask.GetMountCapacity() >= MountCapacity.Ground)
                {
                    if (_travelersTundraMammoth.HaveBuff || _travelersTundraMammoth.IsSpellUsable)
                    {
                        if (!_travelersTundraMammoth.HaveBuff)
                        {
                            MountTask.DismountMount();
                            _travelersTundraMammoth.Launch(true, true, true);
                            Thread.Sleep(2000);
                        }
                        if (ObjectManager.ObjectManager.Me.PlayerFaction.ToLower() == "horde")
                        {
                            WoWUnit drixBlackwrench =
                                ObjectManager.ObjectManager.GetNearestWoWUnit(
                                    ObjectManager.ObjectManager.GetWoWUnitByEntry(32641));
                            if (drixBlackwrench.IsValid && drixBlackwrench.IsAlive)
                            {
                                Npc drixBlackwrenchNpc = new Npc
                                {
                                    Entry = drixBlackwrench.Entry,
                                    Position = drixBlackwrench.Position,
                                    Name = drixBlackwrench.Name,
                                    ContinentIdInt = Usefuls.ContinentId,
                                    Faction = Npc.FactionType.Horde,
                                    SelectGossipOption = 0,
                                    Type = Npc.NpcType.Repair
                                };
                                listNPCs.Add(drixBlackwrenchNpc);
                            }
                        }
                        else
                        {
                            WoWUnit gnimo =
                                ObjectManager.ObjectManager.GetNearestWoWUnit(
                                    ObjectManager.ObjectManager.GetWoWUnitByEntry(32639));
                            if (gnimo.IsValid && gnimo.IsAlive)
                            {
                                Npc gnimoNpc = new Npc
                                {
                                    Entry = gnimo.Entry,
                                    Position = gnimo.Position,
                                    Name = gnimo.Name,
                                    ContinentIdInt = Usefuls.ContinentId,
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
                    if (_grandExpeditionYak.HaveBuff || _grandExpeditionYak.IsSpellUsable)
                    {
                        if (!_grandExpeditionYak.HaveBuff)
                        {
                            MountTask.DismountMount();
                            _grandExpeditionYak.Launch(true, true, true);
                            Thread.Sleep(2000);
                        }
                        WoWUnit cousinSlowhands =
                            ObjectManager.ObjectManager.GetNearestWoWUnit(
                                ObjectManager.ObjectManager.GetWoWUnitByEntry(62822));
                        if (cousinSlowhands.IsValid && cousinSlowhands.IsAlive)
                        {
                            Npc cousinSlowhandsNpc = new Npc
                            {
                                Entry = cousinSlowhands.Entry,
                                Position = cousinSlowhands.Position,
                                Name = cousinSlowhands.Name,
                                ContinentIdInt = Usefuls.ContinentId,
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
                    Npc npcA = DoSpawnRobot("74A", Npc.NpcType.Repair);
                    if (npcA != null)
                        listNPCs.Add(npcA);
                }
                else if (_use110G)
                {
                    Npc npcG = DoSpawnRobot("110G", Npc.NpcType.Repair);
                    if (npcG != null)
                        listNPCs.Add(npcG);
                }
                else if (_useJeeves)
                {
                    Npc npcJeeves = DoSpawnRobot("Jeeves", Npc.NpcType.Repair);
                    if (npcJeeves != null)
                        listNPCs.Add(npcJeeves);
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
                nManagerSetting.CurrentSetting.ActivateAutoSellingFeature && !_suspendSelling)
            {
                if (_magicMountMammoth && MountTask.GetMountCapacity() >= MountCapacity.Ground)
                {
                    if (_travelersTundraMammoth.HaveBuff || _travelersTundraMammoth.IsSpellUsable)
                    {
                        if (!_travelersTundraMammoth.HaveBuff)
                        {
                            MountTask.DismountMount();
                            _travelersTundraMammoth.Launch(true, true, true);
                            Thread.Sleep(2000);
                        }
                        if (ObjectManager.ObjectManager.Me.PlayerFaction.ToLower() == "horde")
                        {
                            WoWUnit mojodishu =
                                ObjectManager.ObjectManager.GetNearestWoWUnit(
                                    ObjectManager.ObjectManager.GetWoWUnitByEntry(32642));
                            if (mojodishu.IsValid && mojodishu.IsAlive)
                            {
                                Npc mojodishuNpc = new Npc
                                {
                                    Entry = mojodishu.Entry,
                                    Position = mojodishu.Position,
                                    Name = mojodishu.Name,
                                    ContinentIdInt = Usefuls.ContinentId,
                                    Faction = Npc.FactionType.Horde,
                                    SelectGossipOption = 0,
                                    Type = Npc.NpcType.Vendor
                                };
                                listNPCs.Add(mojodishuNpc);
                            }
                        }
                        else
                        {
                            WoWUnit hakmuddArgus =
                                ObjectManager.ObjectManager.GetNearestWoWUnit(
                                    ObjectManager.ObjectManager.GetWoWUnitByEntry(32638));
                            if (hakmuddArgus.IsValid && hakmuddArgus.IsAlive)
                            {
                                Npc hakmuddArgusNpc = new Npc
                                {
                                    Entry = hakmuddArgus.Entry,
                                    Position = hakmuddArgus.Position,
                                    Name = hakmuddArgus.Name,
                                    ContinentIdInt = Usefuls.ContinentId,
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
                    if (_grandExpeditionYak.HaveBuff || _grandExpeditionYak.IsSpellUsable)
                    {
                        if (!_grandExpeditionYak.HaveBuff)
                        {
                            MountTask.DismountMount();
                            _grandExpeditionYak.Launch(true, true, true);
                            Thread.Sleep(2000);
                        }
                        WoWUnit cousinSlowhands =
                            ObjectManager.ObjectManager.GetNearestWoWUnit(
                                ObjectManager.ObjectManager.GetWoWUnitByEntry(62822));
                        if (cousinSlowhands.IsValid && cousinSlowhands.IsAlive)
                        {
                            Npc cousinSlowhandsNpc = new Npc
                            {
                                Entry = cousinSlowhands.Entry,
                                Position = cousinSlowhands.Position,
                                Name = cousinSlowhands.Name,
                                ContinentIdInt = Usefuls.ContinentId,
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
                    Npc npcA = DoSpawnRobot("74A", Npc.NpcType.Vendor);
                    if (npcA != null)
                        listNPCs.Add(npcA);
                }
                else if (_use110G)
                {
                    Npc npcG = DoSpawnRobot("110G", Npc.NpcType.Vendor);
                    if (npcG != null)
                        listNPCs.Add(npcG);
                }
                else if (_useJeeves)
                {
                    Npc npcJeeves = DoSpawnRobot("Jeeves", Npc.NpcType.Vendor);
                    if (npcJeeves != null)
                        listNPCs.Add(npcJeeves);
                }
                else
                {
                    if (NpcDB.GetNpcNearby(Npc.NpcType.Vendor).Entry > 0)
                        listNPCs.Add(NpcDB.GetNpcNearby(Npc.NpcType.Vendor));
                }
            }

            #region Repairer, Seller/Buyer, MailBox

            if (listNPCs.Count > 0)
            {
                listNPCs.Sort(OnComparison);
                foreach (Npc npc in listNPCs)
                {
                    Npc target = npc;
                    //Start target finding based on Seller.
                    uint baseAddress = MovementManager.FindTarget(ref target, 0, !ObjectManager.ObjectManager.Me.IsMounted);
                    if (MovementManager.InMovement)
                        return;
                    if (baseAddress == 0 && target.Position.DistanceTo(ObjectManager.ObjectManager.Me.Position) < 10)
                        NpcDB.DelNpc(target);
                    else if (baseAddress > 0)
                    {
                        if (!_travelersTundraMammoth.HaveBuff)
                        {
                            DoProspectingInTown(target);
                            DoMillingInTown(target);
                        }
                        Interact.InteractWith(baseAddress);
                        Thread.Sleep(500);
                        if (target.SelectGossipOption != 0)
                        {
                            Lua.LuaDoString("SelectGossipOption(" + target.SelectGossipOption + ")");
                            Thread.Sleep(500);
                        }
                        else if (target.Type == Npc.NpcType.Repair || target.Type == Npc.NpcType.Vendor)
                        {
                            if (!Gossip.SelectGossip(Gossip.GossipOption.Vendor))
                            {
                                Logging.WriteError("Problem with NPC " + npc.Name + " Removing it for NpcDB");
                                NpcDB.DelNpc(npc);
                                return;
                            }
                        }
                        // NPC Repairer
                        if (target.Type == Npc.NpcType.Repair)
                        {
                            Logging.Write("Repair items from " + target.Name + " (" + target.Entry + ").");
                            Vendor.RepairAllItems();
                            Thread.Sleep(1000);
                        }
                        // End NPC Repairer

                        if (target.Type == Npc.NpcType.Vendor)
                        {
                            // NPC Buyer
                            Logging.Write("Selling items to " + target.Name + " (" + target.Entry + ").");
                            List<WoWItemQuality> vQuality = new List<WoWItemQuality>();
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
                            Thread.Sleep(3000);
                            if (Usefuls.GetContainerNumFreeSlots <= nManagerSetting.CurrentSetting.SellItemsWhenLessThanXSlotLeft)
                                _suspendSelling = true;
                            // End NPC Buyer

                            // NPC Seller
                            if (NeedFoodSupplies() || NeedDrinkSupplies())
                                Logging.Write("Buying beverages and food from " + target.Name + " (" + target.Entry + ").");
                            for (int i = 0; i < 10 && NeedFoodSupplies(); i++)
                            {
                                Vendor.BuyItem(nManagerSetting.CurrentSetting.FoodName, 1);
                            }
                            for (int i = 0; i < 10 && NeedDrinkSupplies(); i++)
                            {
                                Vendor.BuyItem(nManagerSetting.CurrentSetting.BeverageName, 1);
                            }
                            // End NPC Seller
                        }
                        if (target.Type == Npc.NpcType.Repair || target.Type == Npc.NpcType.Vendor)
                            Gossip.CloseGossip();

                        // MailBox
                        if (target.Type == Npc.NpcType.Mailbox)
                        {
                            List<WoWItemQuality> mQuality = new List<WoWItemQuality>();
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

                            bool mailSendingCompleted = false;
                            for (int i = 7; i > 0 && !mailSendingCompleted; i--)
                            {
                                Interact.InteractWith(baseAddress);
                                Thread.Sleep(1000);
                                Mail.SendMessage(nManagerSetting.CurrentSetting.MaillingFeatureRecipient,
                                    nManagerSetting.CurrentSetting.MaillingFeatureSubject, "",
                                    nManagerSetting.CurrentSetting.ForceToMailTheseItems,
                                    nManagerSetting.CurrentSetting.DontMailTheseItems, mQuality,
                                    out mailSendingCompleted);
                                Thread.Sleep(500);
                            }
                            if (mailSendingCompleted)
                                Logging.Write("Sending items to the player " + nManagerSetting.CurrentSetting.MaillingFeatureRecipient + " using " + target.Name + " (" +
                                              target.Entry + ").");
                            if (Usefuls.GetContainerNumFreeSlots <= nManagerSetting.CurrentSetting.SendMailWhenLessThanXSlotLeft)
                                _suspendMailing = true;
                            Lua.LuaDoString("CloseMail()");
                        }
                        // End MailBox
                    }
                    // still on the road, but not in movement for some reasons
                }
            }

            #endregion Repairer, Seller/Buyer, MailBox
        }

        private int OnComparison(Npc n1, Npc n2)
        {
            return n1.Position.DistanceTo(ObjectManager.ObjectManager.Me.Position).CompareTo(n1.Position.DistanceTo(ObjectManager.ObjectManager.Me.Position));
        }

        private bool NeedFoodSupplies()
        {
            if (nManagerSetting.CurrentSetting.FoodName != "" && nManagerSetting.CurrentSetting.NumberOfFoodsWeGot > 0)
            {
                if (ItemsManager.GetItemCount(nManagerSetting.CurrentSetting.FoodName) <
                    nManagerSetting.CurrentSetting.NumberOfFoodsWeGot)
                    return true;
            }
            return false;
        }

        private bool NeedDrinkSupplies()
        {
            if (nManagerSetting.CurrentSetting.BeverageName != "" && nManagerSetting.CurrentSetting.NumberOfBeverageWeGot > 0)
            {
                if (ItemsManager.GetItemCount(nManagerSetting.CurrentSetting.BeverageName) < nManagerSetting.CurrentSetting.NumberOfBeverageWeGot)
                    return true;
            }
            return false;
        }

        private void DoProspectingInTown(Npc npc)
        {
            if ((!_magicMountMammoth && !_magicMountYak || npc.Type != Npc.NpcType.Repair && npc.Type != Npc.NpcType.Vendor) &&
                nManagerSetting.CurrentSetting.OnlyUseProspectingInTown && nManagerSetting.CurrentSetting.ActivateAutoProspecting &&
                nManagerSetting.CurrentSetting.MineralsToProspect.Count > 0)
            {
                if (Prospecting.NeedRun(nManagerSetting.CurrentSetting.MineralsToProspect))
                {
                    ProspectingState prospectingState = new ProspectingState();
                    prospectingState.Run();
                }
            }
        }

        private void DoMillingInTown(Npc npc)
        {
            if ((!_magicMountMammoth && !_magicMountYak || npc.Type != Npc.NpcType.Repair && npc.Type != Npc.NpcType.Vendor) && nManagerSetting.CurrentSetting.OnlyUseMillingInTown &&
                nManagerSetting.CurrentSetting.ActivateAutoMilling &&
                nManagerSetting.CurrentSetting.HerbsToBeMilled.Count > 0)
            {
                if (Prospecting.NeedRun(nManagerSetting.CurrentSetting.HerbsToBeMilled))
                {
                    MillingState millingState = new MillingState();
                    millingState.Run();
                }
            }
        }

        private static Npc DoSpawnRobot(string robot, Npc.NpcType type, bool checkOnly = false)
        {
            int robotItemId;
            int robotEntryId;
            int gossipOption = 0;
            switch (robot)
            {
                case "74A":
                    robotItemId = 18232;
                    robotEntryId = 14337;
                    break;
                case "110G":
                    robotItemId = 34113;
                    robotEntryId = 24780;
                    break;
                case "Jeeves":
                    robotItemId = 49040;
                    robotEntryId = 35642;
                    gossipOption = 2;
                    break;
                default:
                    return null;
            }
            if (!checkOnly)
            {
                MountTask.DismountMount();
                ItemsManager.UseItem(ItemsManager.GetItemNameById(robotItemId));
                Thread.Sleep(2000);
            }
            WoWUnit unitRobot = ObjectManager.ObjectManager.GetNearestWoWUnit(ObjectManager.ObjectManager.GetWoWUnitByEntry(robotEntryId));
            if (!unitRobot.IsValid || !unitRobot.IsAlive)
                return null;
            Npc npcRobot = new Npc
            {
                Entry = unitRobot.Entry,
                Position = unitRobot.Position,
                Name = unitRobot.Name,
                ContinentIdInt = Usefuls.ContinentId,
                Faction = ObjectManager.ObjectManager.Me.PlayerFaction.ToLower() == "horde"
                    ? Npc.FactionType.Horde
                    : Npc.FactionType.Alliance,
                SelectGossipOption = gossipOption,
                Type = type
            };
            return npcRobot;
        }
    }
}