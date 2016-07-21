using System.Collections.Generic;
using System.Threading;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Bot.Tasks;
using nManager.Wow.Class;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;

namespace nManager.Wow.Bot.States
{
    public class IsAttacked : State
    {
        private WoWUnit _unit;
        private WoWUnit _unitToPull;
        private Thread _StrikeBackThread;

        public override string DisplayName
        {
            get { return "IsAttacked"; }
        }

        public void StrikeBack()
        {
            while (Products.Products.IsStarted)
            {
                _unitToPull = ObjectManager.ObjectManager.GetUnitInAggroRange();
                Thread.Sleep(100);
            }
        }

        public override int Priority { get; set; }

        public override bool NeedToRun
        {
            get
            {
                if (!Usefuls.InGame ||
                    Usefuls.IsLoadingOrConnecting ||
                    ObjectManager.ObjectManager.Me.IsDeadMe ||
                    !ObjectManager.ObjectManager.Me.IsValid ||
                    (ObjectManager.ObjectManager.Me.IsMounted &&
                     (nManagerSetting.CurrentSetting.IgnoreFightIfMounted || Usefuls.IsFlying)) ||
                    !Products.Products.IsStarted)
                    return false;

                if (CustomProfile.GetSetIgnoreFight || Quest.GetSetIgnoreFight)
                    return false;

                // Get if is attacked:
                _unit = null;

                if (ObjectManager.ObjectManager.GetNumberAttackPlayer() > 0)
                    _unit = ObjectManager.ObjectManager.GetNearestWoWUnit(ObjectManager.ObjectManager.GetHostileUnitAttackingPlayer());

                if (_unit != null && _unit.IsValid)
                    return true;

                if (!nManagerSetting.CurrentSetting.DontPullMonsters)
                {
                    if (_StrikeBackThread == null || !_StrikeBackThread.IsAlive)
                    {
                        _StrikeBackThread = new Thread(StrikeBack);
                        _StrikeBackThread.Start();
                    }
                    _unit = _unitToPull;
                    if (_unit != null)
                    {
                        Logging.Write("Pulling " + _unit.Name);
                        return true;
                    }
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
            MountTask.DismountMount();
            Logging.Write("Player Attacked by " + _unit.Name + " (lvl " + _unit.Level + ")");
            UInt128 unkillableMob = Fight.StartFight(_unit.Guid);
            if (!_unit.IsDead && unkillableMob != 0 && _unit.HealthPercent == 100.0f)
            {
                Logging.Write("Blacklisting " + _unit.Name);
                nManagerSetting.AddBlackList(unkillableMob, 2*60*1000); // 2 minutes
            }
            else if (_unit.IsDead)
            {
                Statistics.Kills++;
                if (Products.Products.ProductName == "Quester" && (!_unit.IsTapped || (_unit.IsTapped && _unit.IsTappedByMe)))
                    Quest.KilledMobsToCount.Add(_unit.Entry); // we may update a quest requiring killing this unit

                if (ObjectManager.ObjectManager.GetNumberAttackPlayer() <= 0)
                    Thread.Sleep(Usefuls.Latency + 500);

                while (!ObjectManager.ObjectManager.Me.IsMounted && ObjectManager.ObjectManager.Me.InCombat && ObjectManager.ObjectManager.GetNumberAttackPlayer() <= 0)
                {
                    Thread.Sleep(150);
                }
                Fight.StopFight();
            }
        }
    }
}