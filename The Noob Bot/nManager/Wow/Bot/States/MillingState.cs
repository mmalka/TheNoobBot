using System.Collections.Generic;
using System.Threading;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Helpers;
using Timer = nManager.Helpful.Timer;

namespace nManager.Wow.Bot.States
{
    public class MillingState : State
    {
        public override string DisplayName
        {
            get { return "Milling"; }
        }

        public override int Priority { get; set; }

        private int _lastTimeRunning;

        public override bool NeedToRun
        {
            get
            {
                if (!nManagerSetting.CurrentSetting.ActivateAutoMilling)
                    return false;

                if (nManagerSetting.CurrentSetting.OnlyUseMillingInTown)
                    return false;

                if (nManagerSetting.CurrentSetting.HerbsToBeMilled.Count <= 0)
                    return false;

                if ((_lastTimeRunning + (nManagerSetting.CurrentSetting.TimeBetweenEachMillingAttempt*60*1000)) >
                    Others.Times)
                    return false;

                if (!Usefuls.InGame ||
                    Usefuls.IsLoading ||
                    ObjectManager.ObjectManager.Me.IsDeadMe ||
                    !ObjectManager.ObjectManager.Me.IsValid ||
                    (ObjectManager.ObjectManager.Me.InCombat &&
                     !(ObjectManager.ObjectManager.Me.IsMounted &&
                       (nManagerSetting.CurrentSetting.IgnoreFightIfMounted || Usefuls.IsFlying))) ||
                    ObjectManager.ObjectManager.Me.IsMounted ||
                    MovementManager.InMovement ||
                    !Products.Products.IsStarted)
                    return false;


                if (Milling.NeedRun(nManagerSetting.CurrentSetting.HerbsToBeMilled))
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
            _lastTimeRunning = Others.Times;
            MovementManager.StopMove();
            Thread.Sleep(500);
            Tasks.MountTask.DismountMount();
            Logging.Write("Milling in progress");
            Timer timer = new Timer(15*60*1000);
            // Milling
            while (Milling.NeedRun(nManagerSetting.CurrentSetting.HerbsToBeMilled) && Products.Products.IsStarted &&
                   Usefuls.InGame &&
                   !ObjectManager.ObjectManager.Me.InCombat && !ObjectManager.ObjectManager.Me.IsDeadMe &&
                   !timer.IsReady)
            {
                Thread.Sleep(200);
                Milling.Pulse(nManagerSetting.CurrentSetting.HerbsToBeMilled);
                Thread.Sleep(750);
                while (ObjectManager.ObjectManager.Me.IsCast && Products.Products.IsStarted && Usefuls.InGame &&
                       !ObjectManager.ObjectManager.Me.InCombat && !ObjectManager.ObjectManager.Me.IsDeadMe &&
                       !timer.IsReady)
                {
                    Thread.Sleep(100);
                }

                Thread.Sleep(Others.Random(600, 1600) + Usefuls.Latency);
            }
        }
    }
}