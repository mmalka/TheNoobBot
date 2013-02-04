using System.Collections.Generic;
using System.Threading;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;

namespace nManager.Wow.Bot.States
{
    public class Battlegrounding : State
    {
        public override string DisplayName
        {
            get { return "Battlegrounding"; }
        }

        public override int Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

        private int _priority;

        public override List<State> NextStates
        {
            get { return new List<State>(); }
        }

        public override List<State> BeforeStates
        {
            get { return new List<State>(); }
        }

        public uint MaxTargetLevel = ObjectManager.ObjectManager.Me.Level + 3;
        public string BattlegroundId;

        private WoWPlayer _unit;

        public override bool NeedToRun
        {
            get
            {
                if (!Battleground.IsInBattleground())
                    return false;
                if (nManagerSetting.CurrentSetting.DontPullMonsters)
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

                // Get unit:
                _unit = new WoWPlayer(0);
                var listUnit = new List<WoWPlayer>();
                listUnit.AddRange(ObjectManager.ObjectManager.Me.PlayerFaction.ToLower() == "horde"
                                      ? ObjectManager.ObjectManager.GetWoWUnitAlliance()
                                      : ObjectManager.ObjectManager.GetWoWUnitHorde());

                _unit = ObjectManager.ObjectManager.GetNearestWoWPlayer(listUnit);

                if (!_unit.IsValid)
                    return false;

                if (!nManagerSetting.IsBlackListedZone(_unit.Position) &&
                    _unit.GetDistance2D < nManagerSetting.CurrentSetting.GatheringSearchRadius &&
                    !nManagerSetting.IsBlackListed(_unit.Guid) && _unit.IsValid)
                    if (_unit.Target == ObjectManager.ObjectManager.Me.Target ||
                        _unit.Target == ObjectManager.ObjectManager.Pet.Target || _unit.Target == 0 ||
                        nManagerSetting.CurrentSetting.CanPullUnitsAlreadyInFight)
                        if (!_unit.UnitNearest)
                            if (_unit.Level <= MaxTargetLevel)
                                return true;

                _unit = new WoWPlayer(0);
                return false;
            }
        }

        public override void Run()
        {
            MovementManager.StopMove();
            MovementManager.StopMove();
            Logging.Write("Engage fight against player " + _unit.Name + " (lvl " + _unit.Level + ")");
            ulong UnkillableMob = Fight.StartFight(_unit.Guid);
            if (!_unit.IsDead && UnkillableMob != 0)
            {
                Logging.Write("Can't reach " + _unit.Name + ", blacklisting its position.");
                nManagerSetting.AddBlackList(UnkillableMob, 2*60*1000); // 2 minutes
            }
            else if (_unit.IsDead)
            {
                Statistics.Kills++;
                Thread.Sleep(Usefuls.Latency + 1000);
                while (!ObjectManager.ObjectManager.Me.IsMounted && ObjectManager.ObjectManager.Me.InCombat &&
                       ObjectManager.ObjectManager.GetUnitAttackPlayer().Count <= 0)
                {
                    Thread.Sleep(50);
                }
                Fight.StopFight();
            }
        }
    }
}