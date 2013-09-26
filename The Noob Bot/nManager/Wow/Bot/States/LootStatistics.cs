using System;
using System.Collections.Generic;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;
using Timer = nManager.Helpful.Timer;

namespace nManager.Wow.Bot.States
{
    public class LootStatistics : State
    {
        public override string DisplayName
        {
            get { return "LootStatistics"; }
        }

        public override int Priority { get; set; }

        public override List<State> NextStates
        {
            get { return new List<State>(); }
        }

        public override List<State> BeforeStates
        {
            get { return new List<State>(); }
        }

        private readonly Timer _nextCheck = new Timer(6000);
        private Dictionary<int, int> _itemStock = new Dictionary<int, int>();
        public static List<WoWObject> TempList = new List<WoWObject>();

        public override bool NeedToRun
        {
            get
            {
                if (!Usefuls.InGame ||
                    Usefuls.IsLoadingOrConnecting ||
                    !ObjectManager.ObjectManager.Me.IsValid ||
                    ObjectManager.ObjectManager.Me.InCombat ||
                    !Products.Products.IsStarted)
                    return false;
                return ObjectManager.ObjectManager.ObjectListReloaded && _nextCheck.IsReady;
            }
        }

        public override void Run()
        {
            // we don't want to stay too long here, let's try to do our job as fast as possible.
            try // to be removed if no fails
            {
                Dictionary<int, int> newLoots = new Dictionary<int, int>();
                bool firstCheck = true;
                lock (TempList)
                {
                    foreach (WoWObject objects in TempList)
                    {
                        if (objects.Type != WoWObjectType.Item)
                            continue;
                        if (!objects.IsValid)
                            continue;
                        int localEntry = objects.Entry;
                        if (localEntry < 1)
                        {
                            localEntry = objects.Entry; // Gives a seconds chance.
                            if (localEntry < 1)
                            {
                                localEntry = objects.Entry; // Gives a last chance.
                                if (localEntry < 1)
                                    continue;
                            }
                        }
                        if (objects.ItemOwner != ObjectManager.ObjectManager.Me.Guid)
                            continue;
                        if (newLoots.ContainsKey(localEntry))
                            continue; // ObjectManager return an independant baseAdress for each stack, we just want ONE ItemEntry, not one per stack.
                        int count = ItemsManager.GetItemCount(localEntry);
                        if (!_itemStock.ContainsKey(localEntry))
                        {
                            newLoots.Add(localEntry, count);
                            _itemStock.Add(localEntry, count);
                            continue;
                        }
                        firstCheck = false;
                        if (_itemStock[localEntry] == count)
                            continue;
                        if (_itemStock[localEntry] < count)
                        {
                            newLoots.Add(localEntry, count - _itemStock[localEntry]);
                            _itemStock[localEntry] = count;
                            continue;
                        }
                        if (_itemStock[localEntry] > count)
                        {
                            // we lost some items, let's ignore this for now and just replace our internal stock
                            _itemStock[localEntry] = count;
                        }
                    }
                    TempList.Clear();
                    ObjectManager.ObjectManager.ObjectListReloaded = false;
                }

                if (!firstCheck)
                {
                    foreach (KeyValuePair<int, int> pair in newLoots)
                    {
                        Logging.Write("You recieve loot: " + ItemsManager.GetItemNameById(pair.Key) + "(" + pair.Key + ") x" + pair.Value);
                    }
                }
                newLoots.Clear();
                _nextCheck.Reset();
            }
            catch (Exception e)
            {
                Logging.WriteError("LootStatistics Internal Foreach: " + e);
            }
        }
    }
}