using System.Collections.Generic;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;

namespace nManager.Wow.Bot.States
{
    public class LootStatistics : State
    {
        private bool _firstCheck = true;

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

                return _firstCheck;
            }
        }

        public override void Run()
        {
            Logging.Write("Initializing LootStatistics module, may take few seconds.");
            Others.CheckInventoryForLatestLoot(0); // Generate the initial _stockList.
            EventsListener.HookEvent(WoWEventsType.CHAT_MSG_LOOT, callBack => Others.CheckInventoryForLatestLoot((int) callBack)); // Hook
            _firstCheck = false;
        }
    }
}