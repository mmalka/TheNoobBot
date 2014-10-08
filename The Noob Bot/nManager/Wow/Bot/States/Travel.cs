using System.Collections.Generic;
using System.Windows.Forms;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;

namespace nManager.Wow.Bot.States
{
    public class Travel : State
    {
        private Transports _availableTransports;

        public override string DisplayName
        {
            get { return "Travel"; }
        }

        public override int Priority { get; set; }

        private Point TravelTo
        {
            get { return Products.Products.TravelTo; }
            set { Products.Products.TravelTo = value; }
        }

        private int TravelToContinentId
        {
            get { return Products.Products.TravelToContinentId; }
            set { Products.Products.TravelToContinentId = value; }
        }

        private bool NeedToTravel
        {
            get { return Products.Products.NeedToTravel; }
            set { Products.Products.NeedToTravel = value; }
        }

        public override bool NeedToRun
        {
            get
            {
                if (_availableTransports == null)
                    _availableTransports = XmlSerializer.Deserialize<Transports>(Application.StartupPath + @"\Data\TransportsDB.xml");
                if (_availableTransports == null)
                    return false;
                if (!Products.Products.IsStarted || !NeedToTravel)
                    return false;
                if (CanTravelTo(TravelToContinentId)) return true;
                NeedToTravel = false;
                TravelToContinentId = 9999999;
                TravelTo = new Point();
                // cannot reach this continent
                // notify => force product to either stop, use portals or cancel the action.
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

        public bool CanTravelTo(int toContinentId)
        {
            return CanTravelTo(toContinentId, Usefuls.ContinentId);
        }

        public bool CanTravelTo(int toContinentId, int fromContinentId)
        {
            return CanTravelToList(toContinentId, fromContinentId).Count > 0;
        }

        public List<Transport> CanTravelToList(int toContinentId)
        {
            return CanTravelToList(toContinentId, Usefuls.ContinentId);
        }

        public List<Transport> CanTravelToList(int toContinentId, int fromContinentId)
        {
            var listTransport = new List<Transport>();
            foreach (Transport transport in _availableTransports.Items)
            {
                if (transport.AContinentId != toContinentId && transport.BContinentId != toContinentId)
                    continue;
                if (transport.AContinentId == toContinentId && transport.BContinentId != fromContinentId)
                    continue;
                if (transport.BContinentId == toContinentId && transport.AContinentId != fromContinentId)
                    continue;
                listTransport.Add(transport);
            }
            return listTransport;
        }

        public override void Run()
        {
            Logging.Write("Start travel from " + (ContinentId) Usefuls.ContinentId + " to " + (ContinentId) Products.Products.TravelTo + ".");
            /*
             handle it - Will select the best transport for our case, will calculate distance from where we are to Transport, 
                           then from where we wanna go to transport destination to take the smaller path.
            */
            NeedToTravel = false;
            TravelToContinentId = 9999999;
            TravelTo = new Point();
        }
    }
}