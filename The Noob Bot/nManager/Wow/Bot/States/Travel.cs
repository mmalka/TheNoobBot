using System.Collections.Generic;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;

namespace nManager.Wow.Bot.States
{
    public class Travel : State
    {
        private Dictionary<ContinentId, ContinentId> knownDestination;
        // demo key, we need a proper class with a real structure from a XML DB
        // uint Travel.ID (Entry - our)
        // FactionType Travel.Faction (Horde/Alliance)
        // ContinentId Travel.FromContinent
        // ContinentId Travel.ToContinentContinent
        // uint Travel.TransportEntry (GameObject ID if applicable)
        // Point Travel.TransportOutsidePositionDeparture (A position outside of the Transport)
        // Point Travel.TransportOutsidePositionArrival (A position outside of the Transport)
        // Point Travel.TransportInsidePosition (A position inside of the Transport: go there then wait until map has changed or just stick to a NPC)
        // Travel.TransportDepartureNPCId (A NPC onBoard to check if the Transport is at the departure quay)
        // Travel.TransportDepartureNPCPosition (A NPC onBoard to check if the Transport is at the departure quay)
        // Travel.TransportArrivalNPCId (A NPC onBoard to detect if the Transport has arrived to next quay)
        // Travel.TransportArrivalNPCPosition (A NPC onBoard to detect if the Transport has arrived to next quay)

        public override string DisplayName
        {
            get { return "Travel"; }
        }

        public override int Priority { get; set; }

        private ContinentId TravelTo
        {
            get { return Products.Products.TravelTo; }
            set { Products.Products.TravelTo = value; }
        }

        private bool NeedToTravel
        {
            get { return Products.Products.NeedToTravel; }
            set { Products.Products.NeedToTravel = value; }
        }

        public override bool NeedToRun
        {
            get { return Products.Products.IsStarted && NeedToTravel && CanTravelTo(TravelTo); }
        }

        public override List<State> NextStates
        {
            get { return new List<State>(); }
        }

        public override List<State> BeforeStates
        {
            get { return new List<State>(); }
        }

        public bool CanTravelTo(ContinentId continentId)
        {
            foreach (var keyValuePair in knownDestination)
            {
                if (keyValuePair.Key != (ContinentId) Usefuls.ContinentId)
                    continue;
                if (keyValuePair.Value == continentId)
                    return true;
            }
            return false;
        }

        public override void Run()
        {
            Logging.Write("Start travel from " + (ContinentId) Usefuls.ContinentId + " to " + Products.Products.TravelTo + ".");
            /*
             handle it - Will select the best transport for our case, will calculate distance from where we are to Transport, 
                           then from where we wanna go to transport destination to take the smaller path.
            */
            NeedToTravel = false;
            TravelTo = ContinentId.None;
        }
    }
}