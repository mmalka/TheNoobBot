using System.Collections.Generic;
using System.Threading;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Bot.Tasks;
using nManager.Wow.Class;
using nManager.Wow.Helpers;
using Timer = nManager.Helpful.Timer;

namespace nManager.Wow.Bot.States
{
    public class DungeonFarming : State
    {
        public override string DisplayName
        {
            get { return "DungeonFarming"; }
        }

        public override int Priority { get; set; }

        private readonly List<int> BlackListDigsites = new List<int>();
        private int LastZone;
        private Digsite digsitesZone = new Digsite();


        private Point _travelLocation = null;
        private bool _travelDisabled = false;

        public override bool NeedToRun
        {
            get
            {
                if (!Usefuls.InGame ||
                    Usefuls.IsLoadingOrConnecting ||
                    ObjectManager.ObjectManager.Me.IsDeadMe ||
                    !ObjectManager.ObjectManager.Me.IsValid ||
                    (ObjectManager.ObjectManager.Me.InCombat &&
                     !(ObjectManager.ObjectManager.Me.IsMounted &&
                       (nManagerSetting.CurrentSetting.IgnoreFightIfMounted || Usefuls.IsFlying))) ||
                    !Products.Products.IsStarted)
                    return false;

                if (!BlackListDigsites.Contains(digsitesZone.id) &&
                    Archaeology.DigsiteZoneIsAvailable(digsitesZone))
                    return true;

                ObjectManager.WoWGameObject u =
                    ObjectManager.ObjectManager.GetNearestWoWGameObject(
                        ObjectManager.ObjectManager.GetWoWGameObjectByEntry(Archaeology.ArchaeologyItemsFindList));
                if (u.IsValid)
                    return true;

                List<Digsite> listDigsitesZone = Archaeology.GetDigsitesZoneAvailable();

                if (listDigsitesZone.Count > 0)
                {
                    Digsite tDigsitesZone = new Digsite {id = 0, name = ""};
                    float distance = System.Single.MaxValue;
                    float priority = System.Single.MinValue;
                    // Get the max priority in the list of available dig sites
                    foreach (Digsite t in listDigsitesZone)
                    {
                        if (t.PriorityDigsites > priority && !BlackListDigsites.Contains(t.id))
                            priority = t.PriorityDigsites;
                    }
                    // Now remove all digsites which are blacklisted or have lower priority
                    for (int digSiteIndex = listDigsitesZone.Count - 1; digSiteIndex >= 0; digSiteIndex--)
                    {
                        if (BlackListDigsites.Contains(listDigsitesZone[digSiteIndex].id) ||
                            listDigsitesZone[digSiteIndex].PriorityDigsites != priority)
                        {
                            listDigsitesZone.RemoveAt(digSiteIndex);
                        }
                    }
                    foreach (Digsite t in listDigsitesZone)
                    {
                        WoWResearchSite OneSite = WoWResearchSite.FromName(t.name);
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
            try
            {
                if (MovementManager.InMovement)
                    return;
            }
            catch
            {
            }
        }
    }
}