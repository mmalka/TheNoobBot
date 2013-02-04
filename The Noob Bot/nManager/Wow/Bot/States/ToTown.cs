using System.Collections.Generic;
using System.Threading;
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

                if (ObjectManager.ObjectManager.Me.GetDurability <=
                    nManagerSetting.CurrentSetting.RepairWhenDurabilityIsUnderPercent &&
                    NpcDB.GetNpcNearby(Npc.NpcType.Repair).Entry > 0)
                    return true;

                if (Usefuls.GetContainerNumFreeSlots <= nManagerSetting.CurrentSetting.SellItemsWhenLessThanXSlotLeft &&
                    (NpcDB.GetNpcNearby(Npc.NpcType.Repair).Entry + NpcDB.GetNpcNearby(Npc.NpcType.Vendor).Entry) > 0
                    && nManagerSetting.CurrentSetting.ActivateAutoSellingFeature)
                    return true;

                if (Usefuls.GetContainerNumFreeSlots <= nManagerSetting.CurrentSetting.SendMailWhenLessThanXSlotLeft &&
                    NpcDB.GetNpcNearby(Npc.NpcType.Mailbox).Entry > 0 &&
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
                nManagerSetting.CurrentSetting.MaillingFeatureRecipient != string.Empty &&
                NpcDB.GetNpcNearby(Npc.NpcType.Mailbox).Entry > 0)
                mailBox = NpcDB.GetNpcNearby(Npc.NpcType.Mailbox);
            // If need repair
            if ((ObjectManager.ObjectManager.Me.GetDurability <= 85 ||
                 (Usefuls.GetContainerNumFreeSlots <= 2 && NpcDB.GetNpcNearby(Npc.NpcType.Vendor).Entry <= 0)) &&
                NpcDB.GetNpcNearby(Npc.NpcType.Repair).Entry > 0)
                listVendor.Add(NpcDB.GetNpcNearby(Npc.NpcType.Repair));
            // If need sell
            if (NpcDB.GetNpcNearby(Npc.NpcType.Vendor).Entry > 0 &&
                (listVendor.Count <= 0 || NeededBuyFood() || NeededBuyDrink() ||
                 (Usefuls.GetContainerNumFreeSlots <= 2 && listVendor.Count <= 0)))
                listVendor.Add(NpcDB.GetNpcNearby(Npc.NpcType.Vendor));

            #region Vendor

            if (listVendor.Count > 0)
            {
                foreach (var vendor in listVendor)
                {
                    Logging.Write("Go to vendor");
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

                    var vendorObj =
                        ObjectManager.ObjectManager.GetNearestWoWUnit(
                            ObjectManager.ObjectManager.GetWoWUnitByEntry(vendor.Entry));
                    if (vendorObj.IsValid)
                    {
                        Logging.Write("Vendor named " + vendorObj.Name);

                        Thread.Sleep(500);
                        MovementManager.StopMoveTo();
                        Thread.Sleep(1000);
                        List<Point> listPoint = PathFinder.FindPath(vendorObj.Position);

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
                            if (vendorObj.Position.DistanceTo(ObjectManager.ObjectManager.Me.Position) < 3.7f)
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


                        // Milling
                        if (nManagerSetting.CurrentSetting.OnlyUseMillingInTown &&
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
                            Thread.Sleep(1500);

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