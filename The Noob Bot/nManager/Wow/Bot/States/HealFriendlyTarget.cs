using System.Collections.Generic;
using System.Linq;
using System.Threading;
using nManager.FiniteStateMachine;
using nManager.Wow.Bot.Tasks;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;

namespace nManager.Wow.Bot.States
{
    public class HealFriendlyTarget : State
    {
        public override string DisplayName
        {
            get { return "HealFriendlyTarget"; }
        }

        public override int Priority { get; set; }

        public override List<State> NextStates
        {
            get { return new List<State>(); }
        }

        public override List<State> BeforeStates
        {
            get { return new List<State>(); }
        }

        public override bool NeedToRun
        {
            get
            {
                if (!Usefuls.InGame || Usefuls.IsLoading || ObjectManager.ObjectManager.Me.IsDeadMe ||
                    !ObjectManager.ObjectManager.Me.IsValid || !Products.Products.IsStarted)
                    return false;

                if (ObjectManager.ObjectManager.Me.HealthPercent < 100)
                    return true;

                return Party.IsInGroup() &&
                       Party.GetPartyPlayersGUID()
                           .Any(playerInMyParty => new WoWUnit((uint) playerInMyParty).HealthPercent < 100);
            }
        }

        public override void Run()
        {
            MountTask.DismountMount();
            Heal.StartHealBot();
            while (ObjectManager.ObjectManager.Me.HealthPercent < 100 ||
                   (Party.IsInGroup() &&
                    Party.GetPartyPlayersGUID()
                        .Any(playerInMyParty => new WoWUnit((uint) playerInMyParty).HealthPercent < 100)))
            {
                Thread.Sleep(200);
            }
            Heal.StopHeal();
        }
    }
}