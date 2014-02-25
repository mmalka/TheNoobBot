using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System;
using nManager;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Products;
using nManager.Wow.Bot.Tasks;
using nManager.Wow.Class;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;
using Timer = nManager.Helpful.Timer;
using Math = nManager.Helpful.Math;

namespace Mimesis.Bot
{
    public class MimesisState : State
    {
        public override string DisplayName
        {
            get { return "Mimesis"; }
        }

        public override int Priority { get; set; }

        private Timer _positionCheckTimer, _runTimer;
        private Npc _master = null;

        public override bool NeedToRun
        {
            get
            {
                if (_runTimer == null)
                {
                    _runTimer = new Timer(200);
                    _positionCheckTimer = new Timer(5000);
                }
                if (_master == null)
                {
                    _master = new Npc();
                    _master.Guid = MimesisClientCom.GetMasterGuid();
                    _master.Position = MimesisClientCom.GetMasterPosition();
                }
                if (!Usefuls.InGame ||
                    Usefuls.IsLoadingOrConnecting ||
                    ObjectManager.Me.IsDeadMe ||
                    !ObjectManager.Me.IsValid ||
                    (ObjectManager.Me.InCombat &&
                     !(ObjectManager.Me.IsMounted &&
                       (nManagerSetting.CurrentSetting.IgnoreFightIfMounted || Usefuls.IsFlying))) ||
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


    }
}