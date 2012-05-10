using System.Collections.Generic;
using System.Threading;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Helpers;

namespace nManager.Wow.Bot.States
{
    public class ProspectingState : State
    {
        public override string DisplayName
        {
            get { return "Prospecting"; }
        }

        public override int Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

        private int _priority;
        private int lastTimeRunning;
        public override bool NeedToRun
        {
            get
            {
                if (!nManagerSetting.CurrentSetting.prospecting)
                    return false;

                if (nManagerSetting.CurrentSetting.prospectingInTown)
                    return false;

                if (nManagerSetting.CurrentSetting.prospectingList.Count <= 0)
                    return false;

                if ((lastTimeRunning + (nManagerSetting.CurrentSetting.prospectingTime * 60 * 1000)) > Others.Times)
                    return false;

                if (!Usefuls.InGame ||
                    Usefuls.IsLoadingOrConnecting ||
                    ObjectManager.ObjectManager.Me.IsDeadMe ||
                    !ObjectManager.ObjectManager.Me.IsValid ||
                    (ObjectManager.ObjectManager.Me.InCombat && !(ObjectManager.ObjectManager.Me.IsMounted && (nManagerSetting.CurrentSetting.ignoreFightGoundMount || Usefuls.IsFlying))) ||
                    ObjectManager.ObjectManager.Me.IsMounted ||
                    MovementManager.InMovement ||
                    !Products.Products.IsStarted)
                    return false;


                if (Prospecting.NeedRun(nManagerSetting.CurrentSetting.prospectingList))
                    return true;

                return false;
            }
        }

        public override List<State> NextStates
        {
            get { return new List<State>(); }
        }

        public override List<State> BeforeStates
        {
            get { return new List<State>(); }
        }

        public override void Run()
        {
            lastTimeRunning = Others.Times;
            MovementManager.StopMove();
            Thread.Sleep(500);
            Tasks.MountTask.DismountMount();
            Logging.Write("Prospecting in progress");
            var timer = new Helpful.Timer(15*60*1000);
            // Prospecting
            while (Prospecting.NeedRun(nManagerSetting.CurrentSetting.prospectingList) && Products.Products.IsStarted && Usefuls.InGame &&
                       !ObjectManager.ObjectManager.Me.InCombat && !ObjectManager.ObjectManager.Me.IsDeadMe && !timer.IsReady)
            {
                Thread.Sleep(200);
                Prospecting.Pulse(nManagerSetting.CurrentSetting.prospectingList);
                Thread.Sleep(750);
                while (ObjectManager.ObjectManager.Me.IsCast && Products.Products.IsStarted && Usefuls.InGame &&
                       !ObjectManager.ObjectManager.Me.InCombat && !ObjectManager.ObjectManager.Me.IsDeadMe && !timer.IsReady)
                {
                    Thread.Sleep(100);
                }

                Thread.Sleep(Others.Random(600, 1600) + Usefuls.Latency);
            }
        }
    }
}
