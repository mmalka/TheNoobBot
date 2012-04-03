using System.Threading;
using WowManager;
using WowManager.FiniteStateMachine;
using WowManager.Navigation;
using WowManager.WoW.Interface;
using WowManager.WoW.ObjectManager;
using WowManager.WoW.WoWObject;

namespace Questing_Bot.Bot.States
{
    class IsAttacked : State
    {
        private WoWUnit _unit;

        public override string DisplayName
        {
            get { return "IsAttacked"; }
        }

        public override int Priority
        {
            get { return (int)States.Priority.IsAttacked; }
        }

        public override bool NeedToRun
        {
            get
            {
                if (!Useful.InGame ||
                    Useful.isLoadingOrConnecting || 
                    ObjectManager.Me.IsDeadMe ||
                    !ObjectManager.Me.IsValid ||
                    ObjectManager.Me.IsMount ||
                    !Config.Bot.BotIsActive)
                    return false;

                // Get if is attacked:
                _unit = null;

                if (ObjectManager.GetNumberAttackPlayer() > 0)
                    _unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetUnitAttackPlayer());

                if (_unit != null)
                    if (_unit.IsValid)
                        return true;

                return false;
            }
        }

        public override void Run()
        {
            MovementManager.StopMove();
            MovementManager.Stop();
            Log.AddLog(Translation.GetText(Translation.Text.Player_Attacked) + " Lvl " + _unit.Name);
            WowManager.WoW.PlayerManager.Fight.StartFight(_unit.Guid);
            if (_unit.IsDead)
            {
                Config.Bot.Kills++;
                Thread.Sleep(Useful.Latency + 1000);
                while (!ObjectManager.Me.IsMount && ObjectManager.Me.InCombat && ObjectManager.GetUnitAttackPlayer().Count <= 0)
                {
                    Thread.Sleep(10);
                }
                 WowManager.WoW.PlayerManager.Fight.StopFight();
            }
        }
    }
}
