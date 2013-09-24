using System.Collections.Generic;
using System.Threading;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Bot.Tasks;
using nManager.Wow.Class;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;
using Timer = nManager.Helpful.Timer;

namespace nManager.Wow.Bot.States
{
    public class Pause : State
    {
        public override string DisplayName
        {
            get { return "Pause"; }
        }

        public override int Priority { get; set; }
        private static Timer _nextBreakTime = new Timer(0);
        private static Timer _breakTime = new Timer(0);
        public bool FakeSettingsActivateBreakSystem = false;
        public int FakeSettingsMinimumTimeBetweenBreaks = 60;
        public int FakeSettingsMaximumTimeBetweenBreaks = 120;
        public int FakeSettingsMinimumBreakTime = 3;
        public int FakeSettingsMaximumBreakTime = 3;
        private uint _breaksTaken;
        private bool _onBreak;
        private bool _forceResetTimer = false;


        public override bool NeedToRun
        {
            get
            {
                if (Products.Products.IsStarted && Products.Products.InPause)
                {
                    _onBreak = false;
                    return true;
                }
                if (!FakeSettingsActivateBreakSystem)
                    return false; // If we deactivated the break system, we should not be in pause at all.
                if (_breaksTaken == 0 || _forceResetTimer)
                {
                    // we never took any break, we probably just started the product, so let's calculate the timer
                    _nextBreakTime = new Timer(Others.Random(FakeSettingsMinimumTimeBetweenBreaks*60*1000, FakeSettingsMaximumTimeBetweenBreaks*60*1000));
                    _breaksTaken++; // we can display it later then
                }
                if (_nextBreakTime.IsReady)
                {
                    // time to go on break, let's calculate our break timer
                    _breakTime = new Timer(Others.Random(FakeSettingsMinimumBreakTime*60*1000, FakeSettingsMaximumBreakTime*60*1000));
                    _onBreak = true;
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
            Logging.Write("Pause started");
            if (!_onBreak)
            {
                // That's the normal pausing system.
                Helpers.MovementManager.StopMove();
                while (Products.Products.IsStarted && Products.Products.InPause)
                {
                    Thread.Sleep(300);
                }
            }
            else
            {
                // We are on break, will we just pause their in the middle of nowhere ? No we wont !
                Point position = ObjectManager.ObjectManager.Me.Position;
                switch (Others.Random(1, 4))
                {
                    case 1:
                        position.X += Others.Random(100, 300);
                        break;
                    case 2:
                        position.X += Others.Random(-300, -100);
                        break;
                    case 3:
                        position.Y += Others.Random(100, 300);
                        break;
                    case 4:
                        position.Y += Others.Random(-300, -100);
                        break;
                }
                if (MountTask.GetMountCapacity() == MountCapacity.Fly)
                {
                    if (!ObjectManager.ObjectManager.Me.IsMounted)
                        MountTask.Mount(false);
                    // should be mounted here..
                    if (ObjectManager.ObjectManager.Me.IsMounted)
                    {
                        if (Usefuls.IsFlying)
                        {
                            position.Z += Others.Random(20, 50);
                        }
                        else
                        {
                            MountTask.Takeoff();
                            position.Z += Others.Random(50, 100);
                        }
                        MovementManager.MoveTo(position);
                    }
                }
                else
                {
                    // we are a fucking non flyer dude, so let's suppose we are under level 60 (ofc), so, that mean our NPC DB actually cover our zone, so let's just stuck our ass to the closest NPC ever
                    Npc target = new Npc();
                    target = NpcDB.GetNpcNearby(Npc.NpcType.Mailbox);
                    if (target.Entry <= 0)
                    {
                        target = NpcDB.GetNpcNearby(Npc.NpcType.Repair);
                        if (target.Entry <= 0)
                        {
                            target = NpcDB.GetNpcNearby(Npc.NpcType.Vendor);
                            if (target.Entry <= 0)
                                target = NpcDB.GetNpcNearby(Npc.NpcType.FlightMaster);
                        }
                    }
                    if (target.Entry > 0)
                    {
                        //Start target finding based on Seller.
                        uint baseAddress = MovementManager.FindTarget(ref target);
                        if (MovementManager.InMovement)
                            return;
                        // I need to handle this fucking possibility...
                        if (baseAddress == 0 && target.Position.DistanceTo(ObjectManager.ObjectManager.Me.Position) < 10)
                        {
                            NpcDB.DelNpc(target);
                            // The NPC is not found, let's remove it from the DB, but still we must be in a safe place right now.
                        }
                        else if (baseAddress > 0)
                        {
                            // We arrived at our target (either mailbox or repair/vendor or flight master), let's wait here, we're safe.
                        }
                    }
                    // We have no place to go... so just wait there.
                }
                while (!_breakTime.IsReady)
                {
                    Thread.Sleep(300);
                }
                _forceResetTimer = true; // force recalculate the "time between pause" timer when our break is done
            }
            Logging.Write("Pause stoped");
        }
    }
}