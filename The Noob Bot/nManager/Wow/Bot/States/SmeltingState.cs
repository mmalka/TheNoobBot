using System.Collections.Generic;
using System.Threading;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.Helpers;

namespace nManager.Wow.Bot.States
{
    public class SmeltingState : State
    {
        public override string DisplayName
        {
            get { return "Smelting"; }
        }

        public override int Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

        private int _priority;

        public bool IgnoreSmeltingZone;

        public override bool NeedToRun
        {
            get
            {
                if (!nManagerSetting.CurrentSetting.ActivateAutoSmelting)
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


                if (NpcDB.GetNpcNearby(Npc.NpcType.Smelting).Entry > 0)
                {
                    if (Smelting.NeedRun())
                        return true;
                }

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
            if (!IgnoreSmeltingZone)
            {
                Logging.Write("Smelting in progress");
                var smeltingZone = NpcDB.GetNpcNearby(Npc.NpcType.Smelting);
                if (smeltingZone.Entry <= 0)
                    return;
                if (smeltingZone.Position.DistanceTo(ObjectManager.ObjectManager.Me.Position) > 10)
                {
                    var pointssmelting = new List<Point>();
                    if ((smeltingZone.Position.Type.ToLower() == "flying") &&
                        nManagerSetting.CurrentSetting.FlyingMountName != "")
                    {
                        pointssmelting.Add(smeltingZone.Position);
                    }
                    else
                    {
                        pointssmelting = PathFinder.FindPath(smeltingZone.Position);
                    }


                    MovementManager.Go(pointssmelting);
                    var timer = new Helpful.Timer(((int) Math.DistanceListPoint(pointssmelting)/3*1000) + 5000);
                    Thread.Sleep(700);
                    while (MovementManager.InMovement && Products.Products.IsStarted && Usefuls.InGame &&
                           !(ObjectManager.ObjectManager.Me.InCombat &&
                             !(ObjectManager.ObjectManager.Me.IsMounted &&
                               (nManagerSetting.CurrentSetting.IgnoreFightIfMounted || Usefuls.IsFlying))) &&
                           !ObjectManager.ObjectManager.Me.IsDeadMe)
                    {
                        if (timer.IsReady)
                            MovementManager.StopMove();
                        if (smeltingZone.Position.DistanceTo(ObjectManager.ObjectManager.Me.Position) < 3.7f)
                            MovementManager.StopMove();
                        Thread.Sleep(100);
                    }
                }

                MovementManager.StopMove();
                Tasks.MountTask.DismountMount();
                Thread.Sleep(500);

                if (smeltingZone.Position.DistanceTo(ObjectManager.ObjectManager.Me.Position) > 15)
                    return;
            }

            // Smelting
            Smelting.OpenSmeltingWindow();
            var timer2 = new Helpful.Timer(15*60*1000);
            while (Smelting.NeedRun(false) && Products.Products.IsStarted && Usefuls.InGame &&
                   !ObjectManager.ObjectManager.Me.InCombat && !ObjectManager.ObjectManager.Me.IsDeadMe &&
                   !timer2.IsReady)
            {
                Smelting.Pulse();
                Thread.Sleep(1500);
                while (ObjectManager.ObjectManager.Me.IsCast && Products.Products.IsStarted && Usefuls.InGame &&
                       !ObjectManager.ObjectManager.Me.InCombat && !ObjectManager.ObjectManager.Me.IsDeadMe &&
                       !timer2.IsReady)
                {
                    Thread.Sleep(700);
                    if (!ObjectManager.ObjectManager.Me.IsCast)
                        Thread.Sleep(700);
                }

                Thread.Sleep(Usefuls.Latency);
            }
            Smelting.CloseSmeltingWindow();
        }
    }
}