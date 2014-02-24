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

namespace MimicryBot.Bot
{
    public class MimicryState : State
    {
        public override string DisplayName
        {
            get { return "Mimicry"; }
        }

        public override int Priority { get; set; }

        private Timer _positionCheckTimer;
        private Point _lastPoint;
        private ulong _masterGuid = 0;

        public override bool NeedToRun
        {
            get
            {
                if (_positionCheckTimer == null)
                    _positionCheckTimer = new Timer(500);
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
                if (_masterGuid == 0)
                    _masterGuid = MimicryClientCom.GetMasterGuid();
                if (_positionCheckTimer.IsReady)
                {
                    _positionCheckTimer.Reset();
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
            Point target = MimicryClientCom.GetMasterPosition();
            if (_lastPoint == target)
                return;
            _lastPoint = target;

            if (target.DistanceTo(ObjectManager.Me.Position) < 3.0f)
            {
                MovementManager.StopMove();
                return;
            }

            if (target.Type.ToLower() == "flying" || target.Type.ToLower() == "swimming")
            {
                MovementManager.MoveTo(target);
                return;
            }

            Npc Master = new Npc();
            Master.Position = target;
            Master.Guid = _masterGuid;
            uint baseAddress = MovementManager.FindTarget(ref Master, 3.0f);
        }


    }
}