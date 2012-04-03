using System.Collections.Generic;
using System.Threading;
using Questing_Bot.Bot.Tasks;
using Questing_Bot.Profile;
using WowManager;
using WowManager.FiniteStateMachine;
using WowManager.MiscEnums;
using WowManager.MiscStructs;
using WowManager.Navigation;
using WowManager.WoW.Interface;
using WowManager.WoW.ObjectManager;
using WowManager.WoW.PlayerManager;
using WowManager.WoW.WoWObject;
using Timer = WowManager.Others.Timer;
using Vendor = WowManager.WoW.Interface.Vendor;

namespace Questing_Bot.Bot.States
{
    class ToTown : State
    {
        public override string DisplayName
        {
            get { return "To Town"; }
        }

        public override int Priority
        {
            get { return (int)States.Priority.ToTown; }
        }

        public override bool NeedToRun
        {
            get
            {
                if (Config.Bot.DisableRepairAndVendor)
                    return false;

                if (!Useful.InGame ||
                    Useful.isLoadingOrConnecting || 
                    ObjectManager.Me.IsDeadMe ||
                    !ObjectManager.Me.IsValid ||
                    (ObjectManager.Me.InCombat && !ObjectManager.Me.IsMount) ||
                    !Config.Bot.BotIsActive)
                    return false;

                if (Config.Bot.ForceGoToTown)
                    return true;

                if (ObjectManager.Me.GetDurability <= Config.Bot.Profile.MinDurability * 100 &&
                    Functions.CountVendorByType(TypeVendor.Repair) > 0)
                    return true;

                if (Useful.GetContainerNumFreeSlots <= Config.Bot.Profile.MinFreeBagSlots &&
                    Functions.CountVendorByType(TypeVendor.Repair & TypeVendor.Food) > 0)
                    return true;

                return false;
            }
        }

        public override void Run()
        {
            var listVendor = new List<Profile.Vendor>();
            Mailbox mailBox = null;

            Config.Bot.ForceGoToTown = false;

            // MailBox
            if (Config.Bot.FormConfig.MailAt &&
                    Config.Bot.FormConfig.MailAtName != string.Empty &&
                    Functions.CountMailBox() > 0)
                mailBox = Functions.GetNearestMailbox();
            // If need repair
            if (ObjectManager.Me.GetDurability <= 90 &&
                    Functions.CountVendorByType(TypeVendor.Repair) > 0)
                listVendor.Add(Functions.GetNearestVendor(TypeVendor.Repair));
            // If need sell
            if (Functions.CountVendorByType(TypeVendor.Food) > 0 &&
                    (listVendor.Count <= 0 || Functions.NeededBuy().Count > 0))
                listVendor.Add(Functions.GetNearestVendor(TypeVendor.Food));

            #region Mail

            if (mailBox != null)
            {
                Log.AddLog(Translation.GetText(Translation.Text.Go_to_mailbox));
                var pointsMail = Functions.GoToPathFind(mailBox.Position);

                MovementManager.Go(pointsMail);
                var timer = new Timer(((int)Point.SizeListPoint(pointsMail) / 3 * 1000) + 5000);

                while (MovementManager.InMovement && Config.Bot.BotIsActive && Useful.InGame &&
                       !ObjectManager.Me.InCombat && !ObjectManager.Me.IsDeadMe)
                {
                    if (timer.isReady)
                        MovementManager.StopMove();
                    if (mailBox.Position.DistanceTo(ObjectManager.Me.Position) < 3.7f)
                        MovementManager.StopMove();
                    Thread.Sleep(100);
                }

                var listNameMailBox = new List<string>
                                                  {
                                                      "почтового ящика",
                                                      "boîte aux lettres",
                                                      "buzón",
                                                      "Briefkasten",
                                                      "mailbox"
                                                  };
                listNameMailBox.AddRange(Config.Bot.Profile.MailBoxNames);
                WoWGameObject mailBoxObj = ObjectManager.GetNearestWoWGameObject(ObjectManager.GetWoWGameObjectByName(listNameMailBox));
                if (mailBoxObj.IsValid)
                {
                    Log.AddLog(Translation.GetText(Translation.Text.MailBox_no_found));

                    Thread.Sleep(500);
                    MovementManager.Stop();
                    Thread.Sleep(1000);
                    List<Point> listPoint = Functions.GoToPathFind(mailBoxObj.Position);

                    Log.AddLog(Translation.GetText(Translation.Text.Go_to_mailbox));

                    MovementManager.Go(listPoint);
                    timer = new Timer(((int)Point.SizeListPoint(listPoint) / 3 * 1000) + 5000);
                    while (MovementManager.InMovement && Config.Bot.BotIsActive && Useful.InGame &&
                           !ObjectManager.Me.InCombat && !ObjectManager.Me.IsDeadMe)
                    {
                        if (timer.isReady)
                            MovementManager.StopMove();
                        if (mailBoxObj.Position.DistanceTo(ObjectManager.Me.Position) < 3.7f)
                            MovementManager.StopMove();
                        Thread.Sleep(100);
                    }

                    if (ObjectManager.Me.Position.DistanceTo(mailBoxObj.Position) < 5 && Config.Bot.BotIsActive &&
                        !ObjectManager.Me.InCombat)
                    {
                        Interact.InteractGameObject(mailBoxObj.GetBaseAddress);
                        Thread.Sleep(500);
                        var mQuality = new List<WoWItemQuality>();
                        if (Config.Bot.Profile.MailGrey)
                            mQuality.Add(WoWItemQuality.Poor);
                        if (Config.Bot.Profile.MailWhite)
                            mQuality.Add(WoWItemQuality.Common);
                        if (Config.Bot.Profile.MailGreen)
                            mQuality.Add(WoWItemQuality.Uncommon);
                        if (Config.Bot.Profile.MailBlue)
                            mQuality.Add(WoWItemQuality.Rare);
                        if (Config.Bot.Profile.MailPurple)
                            mQuality.Add(WoWItemQuality.Epic);

                        for (var i = 3; i > 0; i--)
                        {
                            Interact.InteractGameObject(mailBoxObj.GetBaseAddress);
                            Thread.Sleep(1000);
                            Mail.SendMessage(Config.Bot.FormConfig.MailAtName, "Hey", "",
                                             new List<string>(), Functions.KeepItemsList(), mQuality);
                            Thread.Sleep(500);
                        }
                        Log.AddLog(Translation.GetText(Translation.Text.Mail_sending_at) + " " + Config.Bot.FormConfig.MailAtName);
                    }
                    else
                    {
                        Log.AddLog(Translation.GetText(Translation.Text.Unable_to_reach_the_message_box));
                    }
                }
                else
                {
                    Log.AddLog(Translation.GetText(Translation.Text.MailBox_no_found));
                }
            }

            #endregion Mail


            #region Vendor

            if (listVendor.Count > 0)
            {
                foreach (var vendor in listVendor)
                {
                    Log.AddLog(Translation.GetText(Translation.Text.Go_to_vendor));
                    var pointsVendor = Functions.GoToPathFind(vendor.Position);

                    MovementManager.Go(pointsVendor);
                    var timer = new Timer(((int)Point.SizeListPoint(pointsVendor) / 3 * 1000) + 5000);

                    while (MovementManager.InMovement && Config.Bot.BotIsActive && Useful.InGame &&
                           !ObjectManager.Me.InCombat && !ObjectManager.Me.IsDeadMe)
                    {
                        if (timer.isReady)
                            MovementManager.StopMove();
                        if (vendor.Position.DistanceTo(ObjectManager.Me.Position) < 3.7f)
                            MovementManager.StopMove();
                        Thread.Sleep(100);
                    }

                    var vendorObj = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitByEntry(vendor.Entry));
                    if (vendorObj.IsValid)
                    {
                        Log.AddLog(Translation.GetText(Translation.Text.Vendor_named) + " " + vendorObj.Name);

                        Thread.Sleep(500);
                        MovementManager.Stop();
                        Thread.Sleep(1000);
                        List<Point> listPoint = Functions.GoToPathFind(vendorObj.Position);

                        MovementManager.Go(listPoint);
                        timer = new Timer(((int)Point.SizeListPoint(listPoint) / 3 * 1000) + 5000);
                        while (MovementManager.InMovement && Config.Bot.BotIsActive && Useful.InGame &&
                               !ObjectManager.Me.InCombat && !ObjectManager.Me.IsDeadMe)
                        {
                            if (timer.isReady)
                                MovementManager.StopMove();
                            if (vendorObj.Position.DistanceTo(ObjectManager.Me.Position) < 3.7f)
                                MovementManager.StopMove();
                            Thread.Sleep(100);
                        }

                        if (ObjectManager.Me.Position.DistanceTo(vendorObj.Position) < 5 && Config.Bot.BotIsActive &&
                            !ObjectManager.Me.InCombat)
                        {
                            Interact.InteractGameObject(vendorObj.GetBaseAddress);
                            Thread.Sleep(1500);

                            // Repair:
                            if (vendor.Type == TypeVendor.Repair)
                            {
                                Log.AddLog(Translation.GetText(Translation.Text.Repair_items));
                                Vendor.RepairAllItems();
                                Thread.Sleep(1000);
                            }

                            // Sell:
                            var vQuality = new List<WoWItemQuality>();
                            if (Config.Bot.Profile.SellGrey)
                                vQuality.Add(WoWItemQuality.Poor);
                            if (Config.Bot.Profile.SellWhite)
                                vQuality.Add(WoWItemQuality.Common);
                            if (Config.Bot.Profile.SellGreen)
                                vQuality.Add(WoWItemQuality.Uncommon);
                            if (Config.Bot.Profile.SellBlue)
                                vQuality.Add(WoWItemQuality.Rare);
                            if (Config.Bot.Profile.SellPurple)
                                vQuality.Add(WoWItemQuality.Epic);
                            Vendor.SellItems(new List<string>(), Functions.KeepItemsList(), vQuality);
                            Log.AddLog(Translation.GetText(Translation.Text.Sell_items));
                            Thread.Sleep(3000);
                            

                            // Buy:
                            if (vendor.Type == TypeVendor.Food)
                            {
                                Log.AddLog(Translation.GetText(Translation.Text.Buy_item));
                                Vendor.RepairAllItems();
                                Thread.Sleep(1000);

                                foreach (var b in Functions.NeededBuy())
                                {
                                    Vendor.BuyItem(WowManager.WoW.ItemManager.Item.GetNameById((uint)b.Id), 1);    
                                }
                            }

                            
                            Lua.LuaDoString("CloseMerchant()");
                        }
                        else
                        {
                            Log.AddLog(Translation.GetText(Translation.Text.Unable_to_reach_the_vendor));
                        }
                    }
                }

            #endregion Vendor


            }
        }
    }
}

