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
            get { return new List<State> { new SmeltingState { Priority = 1 } }; }
        }


        public override bool NeedToRun
        {
            get
            {
                if (!nManagerSetting.CurrentSetting.repair && !nManagerSetting.CurrentSetting.useMail && !nManagerSetting.CurrentSetting.selling && (nManagerSetting.CurrentSetting.drinkAmount == 0 || nManagerSetting.CurrentSetting.drinkName == "") && (nManagerSetting.CurrentSetting.foodAmount == 0 || nManagerSetting.CurrentSetting.foodName == ""))
                    return false;

                if (!Usefuls.InGame ||
                    Usefuls.IsLoadingOrConnecting ||
                    ObjectManager.ObjectManager.Me.IsDeadMe ||
                    !ObjectManager.ObjectManager.Me.IsValid ||
                     (ObjectManager.ObjectManager.Me.InCombat && !(ObjectManager.ObjectManager.Me.IsMounted && (nManagerSetting.CurrentSetting.ignoreFightGoundMount || Usefuls.IsFlying))) ||
                    !Products.Products.IsStarted)
                        return false;

                if (ObjectManager.ObjectManager.Me.GetDurability <= 30 &&
                    NpcDB.GetNpcNearby(Npc.NpcType.Repair).Entry > 0)
                    return true;

                if (Usefuls.GetContainerNumFreeSlots <= 2 &&
                    (NpcDB.GetNpcNearby(Npc.NpcType.Repair).Entry + NpcDB.GetNpcNearby(Npc.NpcType.Vendor).Entry) > 0
                    && nManagerSetting.CurrentSetting.selling)
                    return true;

                if (Usefuls.GetContainerNumFreeSlots <= 2 &&
                     NpcDB.GetNpcNearby(Npc.NpcType.Mailbox).Entry > 0 &&
                     nManagerSetting.CurrentSetting.useMail && nManagerSetting.CurrentSetting.mailRecipient != string.Empty)
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
            if (nManagerSetting.CurrentSetting.useMail &&
                    nManagerSetting.CurrentSetting.mailRecipient != string.Empty &&
                    NpcDB.GetNpcNearby(Npc.NpcType.Mailbox).Entry > 0)
                mailBox = NpcDB.GetNpcNearby(Npc.NpcType.Mailbox);
            // If need repair
            if ((ObjectManager.ObjectManager.Me.GetDurability <= 85 || (Usefuls.GetContainerNumFreeSlots <= 2 && NpcDB.GetNpcNearby(Npc.NpcType.Vendor).Entry <= 0)) &&
                    NpcDB.GetNpcNearby(Npc.NpcType.Repair).Entry > 0)
                listVendor.Add(NpcDB.GetNpcNearby(Npc.NpcType.Repair));
            // If need sell
            if (NpcDB.GetNpcNearby(Npc.NpcType.Vendor).Entry > 0 &&
                    (listVendor.Count <= 0 || NeededBuyFood() || NeededBuyDrink() || (Usefuls.GetContainerNumFreeSlots <= 2 && listVendor.Count <= 0)))
                listVendor.Add(NpcDB.GetNpcNearby(Npc.NpcType.Vendor));

            #region Mail

            if (mailBox != null)
            {
                Logging.Write("Go to mailbox");
                var pointsMail = new List<Point>();
                if ((mailBox.Position.Type.ToLower() == "flying") && nManagerSetting.CurrentSetting.flyingMountName != "")
                {
                    pointsMail.Add(mailBox.Position);
                }
                else if (nManagerSetting.CurrentSetting.aquaticName != "" && Usefuls.IsSwimming)
                {
                    mailBox.Position.Type = "Swimming";
                    pointsMail.Add(mailBox.Position);
                }
                else
                {
                    pointsMail = PathFinder.FindPath(mailBox.Position);
                }
                

                MovementManager.Go(pointsMail);
                var timer = new Timer(((int)Math.DistanceListPoint(pointsMail) / 3 * 1000) + 5000);
                Thread.Sleep(700);
                while (MovementManager.InMovement && Products.Products.IsStarted && Usefuls.InGame &&
                       !(ObjectManager.ObjectManager.Me.InCombat && !(ObjectManager.ObjectManager.Me.IsMounted && (nManagerSetting.CurrentSetting.ignoreFightGoundMount || Usefuls.IsFlying))) && !ObjectManager.ObjectManager.Me.IsDeadMe)
                {
                    if (timer.IsReady)
                        MovementManager.StopMove();
                    if (mailBox.Position.DistanceTo(ObjectManager.ObjectManager.Me.Position) < 3.7f)
                        MovementManager.StopMove();
                    Thread.Sleep(100);
                }

                // Prospection
                if (nManagerSetting.CurrentSetting.prospectingInTown && nManagerSetting.CurrentSetting.prospecting && nManagerSetting.CurrentSetting.prospectingList.Count > 0)
                {
                    if (Prospecting.NeedRun(nManagerSetting.CurrentSetting.prospectingList))
                    {
                        var prospectingState = new ProspectingState();
                        prospectingState.Run();
                    }
                }
                // End Prospection

                WoWGameObject mailBoxObj = ObjectManager.ObjectManager.GetNearestWoWGameObject(ObjectManager.ObjectManager.GetWoWGameObjectByEntry(mailBox.Entry));
                if (mailBoxObj.IsValid)
                {
                    Thread.Sleep(500);
                    MovementManager.StopMoveTo();
                    Thread.Sleep(1000);
                    List<Point> listPoint = PathFinder.FindPath(mailBoxObj.Position);

                    Logging.Write("MailBox found");

                    MovementManager.Go(listPoint);
                    timer = new Timer(((int)Math.DistanceListPoint(listPoint) / 3 * 1000) + 5000);
                    while (MovementManager.InMovement && Products.Products.IsStarted && Usefuls.InGame &&
                           !(ObjectManager.ObjectManager.Me.InCombat && !(ObjectManager.ObjectManager.Me.IsMounted && (nManagerSetting.CurrentSetting.ignoreFightGoundMount || Usefuls.IsFlying))) && !ObjectManager.ObjectManager.Me.IsDeadMe)
                    {
                        if (timer.IsReady)
                            MovementManager.StopMove();
                        if (mailBoxObj.Position.DistanceTo(ObjectManager.ObjectManager.Me.Position) < 3.7f)
                            MovementManager.StopMove();
                        Thread.Sleep(100);
                    }

                    if (ObjectManager.ObjectManager.Me.Position.DistanceTo(mailBoxObj.Position) < 5 && Products.Products.IsStarted &&
                        !(ObjectManager.ObjectManager.Me.InCombat && !(ObjectManager.ObjectManager.Me.IsMounted && (nManagerSetting.CurrentSetting.ignoreFightGoundMount || Usefuls.IsFlying))))
                    {
                        Interact.InteractGameObject(mailBoxObj.GetBaseAddress);
                        Thread.Sleep(500);
                        var mQuality = new List<WoWItemQuality>();
                        if (nManagerSetting.CurrentSetting.mailGray)
                            mQuality.Add(WoWItemQuality.Poor);
                        if (nManagerSetting.CurrentSetting.mailWhite)
                            mQuality.Add(WoWItemQuality.Common);
                        if (nManagerSetting.CurrentSetting.mailGreen)
                            mQuality.Add(WoWItemQuality.Uncommon);
                        if (nManagerSetting.CurrentSetting.mailBlue)
                            mQuality.Add(WoWItemQuality.Rare);
                        if (nManagerSetting.CurrentSetting.mailPurple)
                            mQuality.Add(WoWItemQuality.Epic);

                        var needRunAgain = true;
                        for (var i = 7; i > 0 && needRunAgain; i--)
                        {
                            Interact.InteractGameObject(mailBoxObj.GetBaseAddress);
                            Thread.Sleep(1000);
                            Mail.SendMessage(nManagerSetting.CurrentSetting.mailRecipient, nManagerSetting.CurrentSetting.mailSubject, "",
                                             nManagerSetting.CurrentSetting.forceMailList, nManagerSetting.CurrentSetting.doNotMailList, mQuality, out needRunAgain);
                            Thread.Sleep(500);
                        }
                        Logging.Write("Mail sending at " + nManagerSetting.CurrentSetting.mailRecipient);
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


            #region Vendor

            if (listVendor.Count > 0)
            {
                foreach (var vendor in listVendor)
                {
                    Logging.Write("Go to vendor");
                    var pointsVendor = new List<Point>();
                    if ((vendor.Position.Type.ToLower() == "flying") && nManagerSetting.CurrentSetting.flyingMountName != "")
                    {
                        pointsVendor.Add(new Point(vendor.Position));
                    }
                    else if (nManagerSetting.CurrentSetting.aquaticName != "" && Usefuls.IsSwimming)
                    {
                        vendor.Position.Type = "Swimming";
                        pointsVendor.Add(vendor.Position);
                    }
                    else
                    {
                        pointsVendor = PathFinder.FindPath(vendor.Position);
                    }

                    MovementManager.Go(pointsVendor);
                    var timer = new Timer(((int)Math.DistanceListPoint(pointsVendor) / 3 * 1000) + 5000);

                    while (MovementManager.InMovement && Products.Products.IsStarted && Usefuls.InGame &&
                           !(ObjectManager.ObjectManager.Me.InCombat && !(ObjectManager.ObjectManager.Me.IsMounted && (nManagerSetting.CurrentSetting.ignoreFightGoundMount || Usefuls.IsFlying))) && !ObjectManager.ObjectManager.Me.IsDeadMe)
                    {
                        if (timer.IsReady)
                            MovementManager.StopMove();
                        if (vendor.Position.DistanceTo(ObjectManager.ObjectManager.Me.Position) < 3.7f)
                            MovementManager.StopMove();
                        Thread.Sleep(100);
                    }

                    var vendorObj = ObjectManager.ObjectManager.GetNearestWoWUnit(ObjectManager.ObjectManager.GetWoWUnitByEntry(vendor.Entry));
                    if (vendorObj.IsValid)
                    {
                        Logging.Write("Vendor named " + vendorObj.Name);

                        Thread.Sleep(500);
                        MovementManager.StopMoveTo();
                        Thread.Sleep(1000);
                        List<Point> listPoint = PathFinder.FindPath(vendorObj.Position);

                        MovementManager.Go(listPoint);
                        timer = new Timer(((int)Math.DistanceListPoint(listPoint) / 3 * 1000) + 5000);
                        while (MovementManager.InMovement && Products.Products.IsStarted && Usefuls.InGame &&
                               !(ObjectManager.ObjectManager.Me.InCombat && !(ObjectManager.ObjectManager.Me.IsMounted && (nManagerSetting.CurrentSetting.ignoreFightGoundMount || Usefuls.IsFlying))) &&
                               !ObjectManager.ObjectManager.Me.IsDeadMe)
                        {
                            if (timer.IsReady)
                                MovementManager.StopMove();
                            if (vendorObj.Position.DistanceTo(ObjectManager.ObjectManager.Me.Position) < 3.7f)
                                MovementManager.StopMove();
                            Thread.Sleep(100);
                        }

                        // Prospection
                        if (nManagerSetting.CurrentSetting.prospectingInTown && nManagerSetting.CurrentSetting.prospecting && nManagerSetting.CurrentSetting.prospectingList.Count > 0)
                        {
                            if (Prospecting.NeedRun(nManagerSetting.CurrentSetting.prospectingList))
                            {
                                var prospectingState = new ProspectingState();
                                prospectingState.Run();
                            }
                        }
                        // End Prospection

                        if (ObjectManager.ObjectManager.Me.Position.DistanceTo(vendorObj.Position) < 5 && Products.Products.IsStarted &&
                            !(ObjectManager.ObjectManager.Me.InCombat && !(ObjectManager.ObjectManager.Me.IsMounted && (nManagerSetting.CurrentSetting.ignoreFightGoundMount || Usefuls.IsFlying))))
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
                            if (nManagerSetting.CurrentSetting.sellGray)
                                vQuality.Add(WoWItemQuality.Poor);
                            if (nManagerSetting.CurrentSetting.sellWhite)
                                vQuality.Add(WoWItemQuality.Common);
                            if (nManagerSetting.CurrentSetting.sellGreen)
                                vQuality.Add(WoWItemQuality.Uncommon);
                            if (nManagerSetting.CurrentSetting.sellBlue)
                                vQuality.Add(WoWItemQuality.Rare);
                            if (nManagerSetting.CurrentSetting.sellPurple)
                                vQuality.Add(WoWItemQuality.Epic);
                            Vendor.SellItems(nManagerSetting.CurrentSetting.forceSellList, nManagerSetting.CurrentSetting.doNotSellList, vQuality);
                            Logging.Write("Sell items");
                            Thread.Sleep(3000);


                            // Buy:
                            if (vendor.Type == Npc.NpcType.Vendor)
                            {
                                Logging.Write("Buy drink and food");
                                for (int i = 0; i < 10 && NeededBuyFood(); i++)
                                {
                                    Vendor.BuyItem(nManagerSetting.CurrentSetting.foodName, 1);
                                }
                                for (int i = 0; i < 10 && NeededBuyDrink(); i++)
                                {
                                    Vendor.BuyItem(nManagerSetting.CurrentSetting.drinkName, 1);
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

            #endregion Vendor


            }
        }

        bool NeededBuyFood()
        {
            // food
            if (nManagerSetting.CurrentSetting.foodName != "" && nManagerSetting.CurrentSetting.foodAmount > 0)
            {
                if (ItemsManager.GetItemCountByNameLUA(nManagerSetting.CurrentSetting.foodName) < nManagerSetting.CurrentSetting.foodAmount)
                    return true;
            }
            return false;
        }

        bool NeededBuyDrink()
        {
            // Drink
            if (nManagerSetting.CurrentSetting.drinkName != "" && nManagerSetting.CurrentSetting.drinkAmount > 0)
            {
                if (ItemsManager.GetItemCountByNameLUA(nManagerSetting.CurrentSetting.drinkName) < nManagerSetting.CurrentSetting.drinkAmount)
                    return true;
            }
            return false;
        }
    }
}
