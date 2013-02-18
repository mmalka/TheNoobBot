using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using nManager.FiniteStateMachine;
using nManager.Helpful;
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

        public override int Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

        private int _priority;

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
                    (NpcDB.GetNpcNearby(Npc.NpcType.Repair).Entry > 0 || _magicMountMammoth || _magicMountYak || _use74A ||
                     _use110G || _useJeeves) && nManagerSetting.CurrentSetting.ActivateAutoRepairFeature)
                    return true;

                if (Usefuls.GetContainerNumFreeSlots <= nManagerSetting.CurrentSetting.SellItemsWhenLessThanXSlotLeft &&
                    (NpcDB.GetNpcNearby(Npc.NpcType.Vendor).Entry > 0 || _magicMountMammoth || _magicMountYak || _use74A ||
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
            var listVendor = new List<Npc>();
            Npc mailBox = null;

            // MailBox
            if (nManagerSetting.CurrentSetting.ActivateAutoMaillingFeature &&
                nManagerSetting.CurrentSetting.MaillingFeatureRecipient != string.Empty)
            {
                if (_useMollE)
                {
                    var mollE = new WoWItem(40768);
                    ItemsManager.UseItem(mollE.GetItemInfo.ItemName);
                    Thread.Sleep(500);
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
            // If need repair
            if (ObjectManager.ObjectManager.Me.GetDurability <=
                nManagerSetting.CurrentSetting.RepairWhenDurabilityIsUnderPercent &&
                nManagerSetting.CurrentSetting.ActivateAutoRepairFeature)
            {
                if (_magicMountMammoth)
                {
                    if (_travelersTundraMammoth.IsSpellUsable)
                    {
                        _travelersTundraMammoth.Launch(true, true, true);
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
                                    Faction = ObjectManager.ObjectManager.Me.PlayerFaction.ToLower() == "horde"
                                                  ? Npc.FactionType.Horde
                                                  : Npc.FactionType.Alliance,
                                    SelectGossipOption = 0,
                                    Type = Npc.NpcType.Repair
                                };
                            listVendor.Add(gnimoNpc);
                        }
                    }
                }
                else if (_magicMountYak)
                {
                    if (_grandExpeditionYak.IsSpellUsable)
                    {
                        _grandExpeditionYak.Launch(true, true, true);
                        var cousinSlowhands =
                            ObjectManager.ObjectManager.GetNearestWoWUnit(
                                ObjectManager.ObjectManager.GetWoWUnitByEntry(32639));
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
                            listVendor.Add(cousinSlowhandsNpc);
                        }
                    }
                }
                else if (_use74A)
                {
                    var a = new WoWItem(18232);
                    ItemsManager.UseItem(a.GetItemInfo.ItemName);
                    Thread.Sleep(500);
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
                        listVendor.Add(npcA);
                    }
                }
                else if (_use110G)
                {
                    var g = new WoWItem(34113);
                    ItemsManager.UseItem(g.GetItemInfo.ItemName);
                    Thread.Sleep(500);
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
                        listVendor.Add(npcG);
                    }
                }
                else if (_useJeeves)
                {
                    var jeeves = new WoWItem(49040);
                    ItemsManager.UseItem(jeeves.GetItemInfo.ItemName);
                    Thread.Sleep(500);
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
                        listVendor.Add(npcJeeves);
                    }
                }
                else
                {
                    if (NpcDB.GetNpcNearby(Npc.NpcType.Repair).Entry > 0)
                        listVendor.Add(NpcDB.GetNpcNearby(Npc.NpcType.Repair));
                }
            }

            // If need sell
            if (NeededBuyFood() || NeededBuyDrink() ||
                Usefuls.GetContainerNumFreeSlots <= nManagerSetting.CurrentSetting.SellItemsWhenLessThanXSlotLeft &&
                nManagerSetting.CurrentSetting.ActivateAutoSellingFeature)
            {
                if (_magicMountMammoth)
                {
                    if (_travelersTundraMammoth.IsSpellUsable)
                    {
                        _travelersTundraMammoth.Launch(true, true, true);
                        var hakmuddArgus =
                            ObjectManager.ObjectManager.GetNearestWoWUnit(
                                ObjectManager.ObjectManager.GetWoWUnitByEntry(32639));
                        if (hakmuddArgus.IsValid && hakmuddArgus.IsAlive)
                        {
                            var hakmuddArgusNpc = new Npc
                                {
                                    Entry = hakmuddArgus.Entry,
                                    Position = hakmuddArgus.Position,
                                    Name = hakmuddArgus.Name,
                                    ContinentId = (ContinentId) Usefuls.ContinentId,
                                    Faction = ObjectManager.ObjectManager.Me.PlayerFaction.ToLower() == "horde"
                                                  ? Npc.FactionType.Horde
                                                  : Npc.FactionType.Alliance,
                                    SelectGossipOption = 0,
                                    Type = Npc.NpcType.Vendor
                                };
                            listVendor.Add(hakmuddArgusNpc);
                        }
                    }
                }
                else if (_magicMountYak)
                {
                    if (_grandExpeditionYak.IsSpellUsable)
                    {
                        _grandExpeditionYak.Launch(true, true, true);
                        var cousinSlowhands =
                            ObjectManager.ObjectManager.GetNearestWoWUnit(
                                ObjectManager.ObjectManager.GetWoWUnitByEntry(32639));
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
                                    Type = Npc.NpcType.Vendor
                                };
                            listVendor.Add(cousinSlowhandsNpc);
                        }
                    }
                }
                else if (_use74A)
                {
                    var a = new WoWItem(18232);
                    ItemsManager.UseItem(a.GetItemInfo.ItemName);
                    Thread.Sleep(500);
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
                        listVendor.Add(npcA);
                    }
                }
                else if (_use110G)
                {
                    var g = new WoWItem(34113);
                    ItemsManager.UseItem(g.GetItemInfo.ItemName);
                    Thread.Sleep(500);
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
                        listVendor.Add(npcG);
                    }
                }
                else if (_useJeeves)
                {
                    var jeeves = new WoWItem(49040);
                    ItemsManager.UseItem(jeeves.GetItemInfo.ItemName);
                    Thread.Sleep(500);
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
                        listVendor.Add(npcJeeves);
                    }
                }
                else
                {
                    if (NpcDB.GetNpcNearby(Npc.NpcType.Vendor).Entry > 0)
                        listVendor.Add(NpcDB.GetNpcNearby(Npc.NpcType.Vendor));
                }
            }

            #region Vendor

            if (listVendor.Count > 0)
            {
                foreach (var vendor in listVendor)
                {
                    Logging.Write("Go to vendor");
                    if (vendor.Position.DistanceTo(ObjectManager.ObjectManager.Me.Position) > 3.7f)
                    {
                        var pointsVendor = new List<Point>();
                        if ((vendor.Position.Type.ToLower() == "flying") &&
                            nManagerSetting.CurrentSetting.FlyingMountName != "")
                        {
                            pointsVendor.Add(new Point(vendor.Position));
                        }
                        else if (nManagerSetting.CurrentSetting.AquaticMountName != "" && Usefuls.IsSwimming)
                        {
                            vendor.Position.Type = "Swimming";
                            pointsVendor.Add(vendor.Position);
                        }
                        else
                        {
                            pointsVendor = PathFinder.FindPath(vendor.Position);
                        }

                        MovementManager.Go(pointsVendor);
                        var timer = new Timer(((int) Math.DistanceListPoint(pointsVendor)/3*1000) + 5000);

                        while (MovementManager.InMovement && Products.Products.IsStarted && Usefuls.InGame &&
                               !(ObjectManager.ObjectManager.Me.InCombat &&
                                 !(ObjectManager.ObjectManager.Me.IsMounted &&
                                   (nManagerSetting.CurrentSetting.IgnoreFightIfMounted || Usefuls.IsFlying))) &&
                               !ObjectManager.ObjectManager.Me.IsDeadMe)
                        {
                            if (timer.IsReady)
                                MovementManager.StopMove();
                            if (vendor.Position.DistanceTo(ObjectManager.ObjectManager.Me.Position) < 3.7f)
                                MovementManager.StopMove();
                            Thread.Sleep(100);
                        }
                    }

                    var vendorObj =
                        ObjectManager.ObjectManager.GetNearestWoWUnit(
                            ObjectManager.ObjectManager.GetWoWUnitByEntry(vendor.Entry));
                    if (vendorObj.IsValid)
                    {
                        Logging.Write("Vendor named " + vendorObj.Name);
                        if (vendorObj.Position.DistanceTo(ObjectManager.ObjectManager.Me.Position) > 3.7f)
                        {
                            Thread.Sleep(500);
                            MovementManager.StopMoveTo();
                            Thread.Sleep(1000);
                            List<Point> listPoint = PathFinder.FindPath(vendorObj.Position);

                            MovementManager.Go(listPoint);
                            var timer = new Timer(((int) Math.DistanceListPoint(listPoint)/3*1000) + 5000);
                            while (MovementManager.InMovement && Products.Products.IsStarted && Usefuls.InGame &&
                                   !(ObjectManager.ObjectManager.Me.InCombat &&
                                     !(ObjectManager.ObjectManager.Me.IsMounted &&
                                       (nManagerSetting.CurrentSetting.IgnoreFightIfMounted || Usefuls.IsFlying))) &&
                                   !ObjectManager.ObjectManager.Me.IsDeadMe)
                            {
                                if (timer.IsReady)
                                    MovementManager.StopMove();
                                if (vendorObj.Position.DistanceTo(ObjectManager.ObjectManager.Me.Position) < 3.7f)
                                    MovementManager.StopMove();
                                Thread.Sleep(100);
                            }
                        }

                        // Prospection
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
                        // End Prospection


                        // Milling
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
                        // End Milling

                        if (ObjectManager.ObjectManager.Me.Position.DistanceTo(vendorObj.Position) < 5 &&
                            Products.Products.IsStarted &&
                            !(ObjectManager.ObjectManager.Me.InCombat &&
                              !(ObjectManager.ObjectManager.Me.IsMounted &&
                                (nManagerSetting.CurrentSetting.IgnoreFightIfMounted || Usefuls.IsFlying))))
                        {
                            Interact.InteractGameObject(vendorObj.GetBaseAddress);
                            Thread.Sleep(500);
                            if (vendor.SelectGossipOption != 0)
                                Lua.LuaDoString("SelectGossipOption(" + vendor.SelectGossipOption + ")");
                            Thread.Sleep(1000);

                            // Repair:
                            if (vendor.Type == Npc.NpcType.Repair)
                            {
                                Logging.Write("Repair items");
                                Vendor.RepairAllItems();
                                Thread.Sleep(1000);
                            }

                            // Sell:
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
                            Logging.Write("Sell items");
                            Thread.Sleep(3000);


                            // Buy:
                            if (vendor.Type == Npc.NpcType.Vendor)
                            {
                                Logging.Write("Buy drink and food");
                                for (int i = 0; i < 10 && NeededBuyFood(); i++)
                                {
                                    Vendor.BuyItem(nManagerSetting.CurrentSetting.FoodName, 1);
                                }
                                for (int i = 0; i < 10 && NeededBuyDrink(); i++)
                                {
                                    Vendor.BuyItem(nManagerSetting.CurrentSetting.BeverageName, 1);
                                }
                            }


                            Lua.LuaDoString("CloseMerchant()");
                        }
                        else
                        {
                            Logging.Write("Unable to reach the vendor");
                        }
                    }
                }
            }

            #endregion Vendor

            #region Mail

            if (mailBox != null)
            {
                Logging.Write("Go to mailbox");
                var pointsMail = new List<Point>();
                if ((mailBox.Position.Type.ToLower() == "flying") &&
                    nManagerSetting.CurrentSetting.FlyingMountName != "")
                {
                    pointsMail.Add(mailBox.Position);
                }
                else if (nManagerSetting.CurrentSetting.AquaticMountName != "" && Usefuls.IsSwimming)
                {
                    mailBox.Position.Type = "Swimming";
                    pointsMail.Add(mailBox.Position);
                }
                else
                {
                    pointsMail = PathFinder.FindPath(mailBox.Position);
                }


                MovementManager.Go(pointsMail);
                var timer = new Timer(((int) Math.DistanceListPoint(pointsMail)/3*1000) + 5000);
                Thread.Sleep(700);
                while (MovementManager.InMovement && Products.Products.IsStarted && Usefuls.InGame &&
                       !(ObjectManager.ObjectManager.Me.InCombat &&
                         !(ObjectManager.ObjectManager.Me.IsMounted &&
                           (nManagerSetting.CurrentSetting.IgnoreFightIfMounted || Usefuls.IsFlying))) &&
                       !ObjectManager.ObjectManager.Me.IsDeadMe)
                {
                    if (timer.IsReady)
                        MovementManager.StopMove();
                    if (mailBox.Position.DistanceTo(ObjectManager.ObjectManager.Me.Position) < 3.7f)
                        MovementManager.StopMove();
                    Thread.Sleep(100);
                }

                // Prospection
                if (nManagerSetting.CurrentSetting.OnlyUseProspectingInTown &&
                    nManagerSetting.CurrentSetting.ActivateAutoProspecting &&
                    nManagerSetting.CurrentSetting.MineralsToProspect.Count > 0)
                {
                    if (Prospecting.NeedRun(nManagerSetting.CurrentSetting.MineralsToProspect))
                    {
                        var prospectingState = new ProspectingState();
                        prospectingState.Run();
                    }
                }
                // End Prospection

                WoWGameObject mailBoxObj =
                    ObjectManager.ObjectManager.GetNearestWoWGameObject(
                        ObjectManager.ObjectManager.GetWoWGameObjectByEntry(mailBox.Entry));
                if (mailBoxObj.IsValid)
                {
                    Thread.Sleep(500);
                    MovementManager.StopMoveTo();
                    Thread.Sleep(1000);
                    List<Point> listPoint = PathFinder.FindPath(mailBoxObj.Position);

                    Logging.Write("MailBox found");

                    MovementManager.Go(listPoint);
                    timer = new Timer(((int) Math.DistanceListPoint(listPoint)/3*1000) + 5000);
                    while (MovementManager.InMovement && Products.Products.IsStarted && Usefuls.InGame &&
                           !(ObjectManager.ObjectManager.Me.InCombat &&
                             !(ObjectManager.ObjectManager.Me.IsMounted &&
                               (nManagerSetting.CurrentSetting.IgnoreFightIfMounted || Usefuls.IsFlying))) &&
                           !ObjectManager.ObjectManager.Me.IsDeadMe)
                    {
                        if (timer.IsReady)
                            MovementManager.StopMove();
                        if (mailBoxObj.Position.DistanceTo(ObjectManager.ObjectManager.Me.Position) < 3.7f)
                            MovementManager.StopMove();
                        Thread.Sleep(100);
                    }

                    if (ObjectManager.ObjectManager.Me.Position.DistanceTo(mailBoxObj.Position) < 5 &&
                        Products.Products.IsStarted &&
                        !(ObjectManager.ObjectManager.Me.InCombat &&
                          !(ObjectManager.ObjectManager.Me.IsMounted &&
                            (nManagerSetting.CurrentSetting.IgnoreFightIfMounted || Usefuls.IsFlying))))
                    {
                        Interact.InteractGameObject(mailBoxObj.GetBaseAddress);
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

                        var needRunAgain = true;
                        for (var i = 7; i > 0 && needRunAgain; i--)
                        {
                            Interact.InteractGameObject(mailBoxObj.GetBaseAddress);
                            Thread.Sleep(1000);
                            Mail.SendMessage(nManagerSetting.CurrentSetting.MaillingFeatureRecipient,
                                             nManagerSetting.CurrentSetting.MaillingFeatureSubject, "",
                                             nManagerSetting.CurrentSetting.ForceToMailTheseItems,
                                             nManagerSetting.CurrentSetting.DontMailTheseItems, mQuality,
                                             out needRunAgain);
                            Thread.Sleep(500);
                        }
                        Logging.Write("Mail sending at " + nManagerSetting.CurrentSetting.MaillingFeatureRecipient);
                    }
                    else
                    {
                        Logging.Write("Unable to reach the mail box");
                    }
                }
                else
                {
                    Logging.Write("MailBox not found");
                }
            }

            #endregion Mail
        }

        private bool NeededBuyFood()
        {
            // food
            if (nManagerSetting.CurrentSetting.FoodName != "" && nManagerSetting.CurrentSetting.NumberOfFoodsWeGot > 0)
            {
                if (ItemsManager.GetItemCountByNameLUA(nManagerSetting.CurrentSetting.FoodName) <
                    nManagerSetting.CurrentSetting.NumberOfFoodsWeGot)
                    return true;
            }
            return false;
        }

        private bool NeededBuyDrink()
        {
            // Drink
            if (nManagerSetting.CurrentSetting.BeverageName != "" &&
                nManagerSetting.CurrentSetting.NumberOfBeverageWeGot > 0)
            {
                if (ItemsManager.GetItemCountByNameLUA(nManagerSetting.CurrentSetting.BeverageName) <
                    nManagerSetting.CurrentSetting.NumberOfBeverageWeGot)
                    return true;
            }
            return false;
        }
    }
}