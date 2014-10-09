using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Bot.Tasks;
using nManager.Wow.Class;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;

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
            get { return TravelToContinentId != 9999999; }
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

        private float GetPathDistance(List<Point> points)
        {
            if (points.Count == 0)
                return 100000;
            Point oldPoint = null;
            float pathDistance = 0;
            foreach (Point point in points)
            {
                if (oldPoint == null)
                {
                    oldPoint = point;
                    continue;
                }
                pathDistance += point.DistanceTo(oldPoint);
            }
            return pathDistance;
        }

        public override void Run()
        {
            Logging.Write("Start travel from " + ObjectManager.ObjectManager.Me.Position + " " + (ContinentId) Usefuls.ContinentId + " to " + TravelTo + " " + (ContinentId) TravelToContinentId + ".");
            MovementManager.StopMove();
            StartTravel(TravelTo, TravelToContinentId);
        }

        private void StartTravel(Point travelTo, int travelToContinentId)
        {
            Transport selectedTransport;
            bool selectedTransportIdArrivalIsA;
            /* step : Select a transport to go to the destination */
            SelectTransport(travelTo, travelToContinentId, out selectedTransport, out selectedTransportIdArrivalIsA);
            if (selectedTransport.Id == 0)
                return;

            /* step : Go departure quay */
            GoToDepartureQuay(selectedTransport, selectedTransportIdArrivalIsA);

            /* step : wait for transport */
            WaitForTransport(selectedTransport, selectedTransportIdArrivalIsA);

            /* step : Enter the transport */
            EnterTransport(selectedTransport, selectedTransportIdArrivalIsA);

            /* step : travel */
            TravelPatiently(selectedTransport, selectedTransportIdArrivalIsA);

            /* step : Go out of the transport */
            LeaveTransport(selectedTransport, selectedTransportIdArrivalIsA);

            /* step : release the product */
            Logging.Write("Travel accomplished, sent notification to product to take the control back.");
            TravelToContinentId = 9999999;
            TravelTo = new Point();
        }

        private void SelectTransport(Point travelTo, int travelToContinentId, out Transport selectedTransport, out bool selectedTransportIdArrivalIsA)
        {
            List<Transport> listTransport = CanTravelToList(travelToContinentId, Usefuls.ContinentId);
            Logging.Write(listTransport.Count + " transports can travel to this destination.");
            var bestTransport = new Transport();
            bool bestTransportIdArrivalIsA = false;
            float bestTransportIdDistance = float.MaxValue;
            foreach (Transport t in listTransport)
            {
                bool path1ResultSuccess = false;
                var path1 = new List<Point>();
                bool path2ResultSuccess = false;
                var path2 = new List<Point>();
                if (t.AContinentId == t.BContinentId)
                {
                    // we don't change continent, so we must find the closer point and/or accessible one.
                    path1 = PathFinder.FindPath(ObjectManager.ObjectManager.Me.Position, t.OutsideAPoint, Usefuls.ContinentNameMpq, out path1ResultSuccess);
                    path2 = PathFinder.FindPath(ObjectManager.ObjectManager.Me.Position, t.OutsideBPoint, Usefuls.ContinentNameMpq, out path2ResultSuccess);
                }
                else if (t.AContinentId == Usefuls.ContinentId)
                {
                    path1 = PathFinder.FindPath(ObjectManager.ObjectManager.Me.Position, t.OutsideAPoint, Usefuls.ContinentNameMpq, out path1ResultSuccess);
                }
                else if (t.BContinentId == Usefuls.ContinentId)
                {
                    path2 = PathFinder.FindPath(ObjectManager.ObjectManager.Me.Position, t.OutsideBPoint, Usefuls.ContinentNameMpq, out path2ResultSuccess);
                }
                if (path1ResultSuccess)
                {
                    float path1Distance = GetPathDistance(path1);
                    if (bestTransportIdDistance > path1Distance)
                    {
                        bestTransport = t;
                        bestTransportIdArrivalIsA = false;
                        bestTransportIdDistance = path1Distance;
                    }
                }
                if (path2ResultSuccess)
                {
                    float path2Distance = GetPathDistance(path2);
                    if (bestTransportIdDistance > path2Distance)
                    {
                        bestTransport = t;
                        bestTransportIdArrivalIsA = true;
                        bestTransportIdDistance = path2Distance;
                    }
                }
            }

            if (bestTransport.Id == 0)
            {
                TravelToContinentId = 9999999;
                TravelTo = new Point();
                Logging.Write("No transports selected due to incapacity to reach the quay with PathFinder.");
                selectedTransport = new Transport();
                selectedTransportIdArrivalIsA = false;
                return;
            }
            selectedTransport = bestTransport; // Define which transport we will be using.
            selectedTransportIdArrivalIsA = bestTransportIdArrivalIsA; // Define if we must use A or B as departure.
            Logging.Write("Selected transport: " + selectedTransport.Name + "(" + selectedTransport.Id + "), departure quay at " +
                          (selectedTransportIdArrivalIsA ? selectedTransport.BPoint : selectedTransport.APoint) + ".");
        }

        private void GoToDepartureQuay(Transport selectedTransport, bool selectedTransportIdArrivalIsA)
        {
            List<Point> pathToDepartureQuay = selectedTransportIdArrivalIsA ? PathFinder.FindPath(selectedTransport.OutsideBPoint) : PathFinder.FindPath(selectedTransport.OutsideAPoint);
            MovementManager.Go(pathToDepartureQuay);
            Logging.Write("Going to departure quay of " + selectedTransport.Name + "(" + selectedTransport.Id + ").");
            bool loop = true;
            while (loop)
            {
                if (ObjectManager.ObjectManager.Me.InCombat || ObjectManager.ObjectManager.Me.IsDead)
                    return;
                if (ObjectManager.ObjectManager.Me.Position.Equals(pathToDepartureQuay[pathToDepartureQuay.Count - 1]))
                    loop = false;
                if (!MovementManager.InMoveTo && !MovementManager.InMovement)
                    loop = false;
                Thread.Sleep(100);
            }
            /* step : wait for the transport */
            Logging.Write("Arrived at departure quay of " + selectedTransport.Name + "(" + selectedTransport.Id + "), waiting for transport.");
        }

        private void WaitForTransport(Transport selectedTransport, bool selectedTransportIdArrivalIsA)
        {
            //List<WoWUnit> listWoWUnit = ObjectManager.ObjectManager.GetWoWUnitByEntry((int) selectedTransport.Id);
            //WoWUnit TargetIsNPC = ObjectManager.ObjectManager.GetNearestWoWUnit(listWoWUnit, ObjectManager.ObjectManager.Me.Position);
            WoWGameObject TargetIsGameObject = ObjectManager.ObjectManager.GetNearestWoWGameObject(ObjectManager.ObjectManager.GetWoWGameObjectByEntry((int) selectedTransport.Id),
                ObjectManager.ObjectManager.Me.Position);
            /*if (TargetIsNPC.IsValid)
            {
                // portal support ?
            }*/
            bool loop = true;
            while (loop)
            {
                if (Usefuls.IsFlying)
                    MountTask.DismountMount();
                if (TargetIsGameObject.IsValid)
                {
                    if ((selectedTransportIdArrivalIsA ? selectedTransport.BPoint : selectedTransport.APoint).Equals(TargetIsGameObject.Position))
                        loop = false;
                    Thread.Sleep(50); // Wait for transport to get into position.
                }
                else
                {
                    TargetIsGameObject = ObjectManager.ObjectManager.GetNearestWoWGameObject(ObjectManager.ObjectManager.GetWoWGameObjectByEntry((int) selectedTransport.Id), ObjectManager.ObjectManager.Me.Position);
                }
            }
        }

        private void EnterTransport(Transport selectedTransport, bool selectedTransportIdArrivalIsA)
        {
            Logging.Write("Transport " + selectedTransport.Name + "(" + selectedTransport.Id + ") arrived at the quay, entering transport.");
            while (!ObjectManager.ObjectManager.Me.InTransport)
            {
                //continue;
                if (ObjectManager.ObjectManager.Me.Position.Z + 30 < (selectedTransportIdArrivalIsA ? selectedTransport.BPoint : selectedTransport.APoint).Z
                    || ObjectManager.ObjectManager.Me.Position.Z - 30 > (selectedTransportIdArrivalIsA ? selectedTransport.BPoint : selectedTransport.APoint).Z)
                {
                    Logging.Write("Failed to enter transport " + selectedTransport.Name + "(" + selectedTransport.Id + ") going back to the quay.");
                    GoToDepartureQuay(selectedTransport, selectedTransportIdArrivalIsA);
                    EnterTransport(selectedTransport, selectedTransportIdArrivalIsA);
                    return;
                }
                Point goTo = selectedTransportIdArrivalIsA ? selectedTransport.BPoint : selectedTransport.APoint;
                goTo.Z = ObjectManager.ObjectManager.Me.Position.Z; // we don't wanna go to the middle point of a zeppelin for example
                MovementManager.Go(new List<Point> {ObjectManager.ObjectManager.Me.Position, goTo});
                bool loop = true;
                while (loop)
                {
                    if (ObjectManager.ObjectManager.Me.InCombat || ObjectManager.ObjectManager.Me.IsDead)
                        return;
                    if (ObjectManager.ObjectManager.Me.Position.Equals(goTo))
                        loop = false;
                    if (!MovementManager.InMoveTo && !MovementManager.InMovement)
                        loop = false;
                    if (ObjectManager.ObjectManager.Me.InTransport)
                        loop = false;
                    Thread.Sleep(10);
                }
                MovementManager.StopMove();
                MovementsAction.Jump();
                Thread.Sleep(20);
            }
            MovementManager.StopMove(); // The middle point of the transport is probably bad for us, so stop ASAP we are inside.
            Logging.Write("Successfuly entered transport " + selectedTransport.Name + "(" + selectedTransport.Id + "), waiting to arrive at destination.");
        }

        private void LeaveTransport(Transport selectedTransport, bool selectedTransportIdArrivalIsA)
        {
            Logging.Write("Transport " + selectedTransport.Name + "(" + selectedTransport.Id + ") arrived at destination, leaving to the arrival quay.");
            MovementManager.Go(new List<Point> {selectedTransportIdArrivalIsA ? selectedTransport.OutsideAPoint : selectedTransport.OutsideBPoint});
            bool loop = true;
            while (loop)
            {
                if (ObjectManager.ObjectManager.Me.InCombat || ObjectManager.ObjectManager.Me.IsDead)
                    return;
                if (ObjectManager.ObjectManager.Me.Position.Equals(selectedTransportIdArrivalIsA ? selectedTransport.OutsideAPoint : selectedTransport.OutsideBPoint))
                    loop = false;
                if (!MovementManager.InMoveTo && !MovementManager.InMovement)
                    loop = false;
                if (!ObjectManager.ObjectManager.Me.InTransport)
                    loop = false;
                Thread.Sleep(20);
            }
        }

        private void TravelPatiently(Transport selectedTransport, bool selectedTransportIdArrivalIsA)
        {
            bool loop = true;
            while (loop)
            {
                if (!ObjectManager.ObjectManager.Me.InTransport && Usefuls.InGame && !Usefuls.IsLoadingOrConnecting)
                    loop = false;
                if (selectedTransportIdArrivalIsA && ObjectManager.ObjectManager.Me.Position.Equals(selectedTransport.APoint))
                    loop = false;
                if (!selectedTransportIdArrivalIsA && ObjectManager.ObjectManager.Me.Position.Equals(selectedTransport.BPoint))
                    loop = false;
                Thread.Sleep(100);
            }
        }
    }
}