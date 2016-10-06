using System.Collections.Generic;
using System.Threading;
using nManager;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Products;
using nManager.Wow.Class;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;
using Timer = nManager.Helpful.Timer;

namespace Mimesis.Bot
{
    public class MimesisState : State
    {
        public override string DisplayName
        {
            get { return "Mimesis"; }
        }

        public override int Priority { get; set; }

        private Timer _positionCheckTimer, _runTimer, _eventQueryTimer;
        private Npc _master = null;
        private int _groupInvitCount = 0;

        public override bool NeedToRun
        {
            get
            {
                if (_runTimer == null)
                {
                    _runTimer = new Timer(200);
                    _positionCheckTimer = new Timer(5000);
                    _eventQueryTimer = new Timer(1000);
                }
                if (_master == null)
                {
                    _master = new Npc();
                    _master.Guid = MimesisClientCom.GetMasterGuid();
                    _master.Position = MimesisClientCom.GetMasterPosition();
                }
                if (!Usefuls.InGame ||
                    Usefuls.IsLoading ||
                    ObjectManager.Me.IsDeadMe ||
                    !ObjectManager.Me.IsValid ||
                    ObjectManager.Me.InInevitableCombat ||
                    Usefuls.IsFlying ||
                    !Products.IsStarted)
                    return false;
                if (_runTimer.IsReady)
                {
                    _runTimer.Reset();
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
            WoWPlayer TargetPlayer = ObjectManager.GetObjectWoWPlayer(_master.Guid);
            if (TargetPlayer == null)
            {
                if (_positionCheckTimer.IsReady)
                {
                    _master.Position = MimesisClientCom.GetMasterPosition();
                    _positionCheckTimer.Reset();
                }
            }
            else
                _master.Position = TargetPlayer.Position;
            if (MimesisSettings.CurrentSetting.ActivatePartyMode && _groupInvitCount < 3 && !Party.IsInGroup())
            {
                MimesisClientCom.JoinGroup();
                _groupInvitCount++;
                Thread.Sleep(500 + Usefuls.Latency);
            }
            if (!_master.Position.IsValid)
            {
                return;
            }
            else if (MimesisClientCom.HasTaskToDo())
            {
                MovementManager.Chasing = false;
            }
            else
            {
                if (_master.Position.DistanceTo(ObjectManager.Me.Position) < 3.0f)
                {
                    MovementManager.Chasing = false;
                    MovementManager.StopMove();
                }
                else if (_master.Position.Type.ToLower() == "flying" || _master.Position.Type.ToLower() == "swimming")
                {
                    MovementManager.MoveTo(_master.Position);
                    Logging.Write("Flying or swimming");
                }
                else
                {
                    MovementManager.Chasing = true;
                    uint baseAddress = MovementManager.FindTarget(ref _master, 3.0f);
                }
            }
            // now we should query for events
            if (_eventQueryTimer.IsReady)
            {
                MimesisClientCom.ProcessEvents();
                _eventQueryTimer.Reset();
            }
            MimesisClientCom.DoTasks();
            // on event START_LOOT_ROLL lookup if item is an update and roll need/cupidity
        }
    }
}