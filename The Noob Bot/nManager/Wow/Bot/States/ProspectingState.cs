using System.Collections.Generic;
using System.Threading;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Helpers;
using Timer = nManager.Helpful.Timer;

namespace nManager.Wow.Bot.States
{
    public class ProspectingState : State
    {
        public override string DisplayName
        {
            get { return "Prospecting"; }
        }

        public override int Priority { get; set; }

        private int _lastTimeRunning;

        public override bool NeedToRun
        {
            get
            {
                if (!nManagerSetting.CurrentSetting.ActivateAutoProspecting)
                    return false;

                if (nManagerSetting.CurrentSetting.OnlyUseProspectingInTown)
                    return false;

                if (nManagerSetting.CurrentSetting.MineralsToProspect.Count <= 0)
                    return false;

                if ((_lastTimeRunning + (nManagerSetting.CurrentSetting.TimeBetweenEachProspectingAttempt*60*1000)) >
                    Others.Times)
                    return false;

                if (Usefuls.BadBottingConditions || Usefuls.ShouldFight || ObjectManager.ObjectManager.Me.IsMounted || MovementManager.InMovement)
                    return false;


                if (Prospecting.NeedRun(nManagerSetting.CurrentSetting.MineralsToProspect))
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
            Logging.Write("Prospecting in progress");
            Timer timer = new Timer(15*60*1000);
            // Prospecting
            while (Prospecting.NeedRun(nManagerSetting.CurrentSetting.MineralsToProspect) && Products.Products.IsStarted &&
                   Usefuls.InGame &&
                   !ObjectManager.ObjectManager.Me.InCombat && !ObjectManager.ObjectManager.Me.IsDeadMe &&
                   !timer.IsReady)
            {
                Thread.Sleep(200);
                Prospecting.Pulse(nManagerSetting.CurrentSetting.MineralsToProspect);
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