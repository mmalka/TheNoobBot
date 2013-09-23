using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Helpers;

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

        private readonly Channel _lootChannel = new Channel();

        public override bool NeedToRun
        {
            get
            {
                if (!Usefuls.InGame ||
                    Usefuls.IsLoadingOrConnecting ||
                    ObjectManager.ObjectManager.Me.IsDeadMe ||
                    !ObjectManager.ObjectManager.Me.IsValid ||
                    ObjectManager.ObjectManager.Me.InCombat ||
                    !Products.Products.IsStarted)
                    return false;

                // Channel
                while (_lootChannel.CurrentMsg < _lootChannel.GetCurrentMsgInWow)
                {
                    string msg = _lootChannel.ReadAllChannel();
                    if (!String.IsNullOrWhiteSpace(msg))
                    {
                        Logging.Write(msg);
                    }
                }
                return false;
            }
        }

        public override void Run()
        {
            //
        }
    }
}