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
using Math = System.Math;

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
                if (TransportAvailableForContinent(TravelToContinentId)) return true;
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

        public bool TransportAvailableForContinent(int toContinentId)
        {
            return TransportAvailableForContinent(toContinentId, Usefuls.ContinentId);
        }

        public bool TransportAvailableForContinent(int toContinentId, int fromContinentId)
        {
            return ListTransportForContinent(toContinentId, fromContinentId).Count > 0;
        }

        public List<Transport> ListTransportForContinent(int toContinentId)
        {
            return ListTransportForContinent(toContinentId, Usefuls.ContinentId);
        }

        public List<Transport> ListTransportForContinent(int toContinentId, int fromContinentId)
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
                return float.MaxValue;
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

        private void StartTravel(Point travelTo, int travelToContinentId, bool mainFunction = true)
        {
            Transport selectedTransport;
            bool selectedTransportIdArrivalIsA;
            float selectedDistanceTransportId;
            MovementManager.StopMove();
            /* step : Select a transport to go to the destination */
            SelectTransport(ObjectManager.ObjectManager.Me.Position, travelTo, travelToContinentId, out selectedTransport, out selectedTransportIdArrivalIsA, out selectedDistanceTransportId, mainFunction);
            MovementManager.StopMove(); 
            if (selectedTransport.Id == 0)
                return;

            /* step : Go departure quay */
            GoToDepartureQuay(selectedTransport, selectedTransportIdArrivalIsA, mainFunction);
            MovementManager.StopMove();

            /* step : wait for transport */
            WaitForTransport(selectedTransport, selectedTransportIdArrivalIsA, mainFunction);
            MovementManager.StopMove();

            /* step : Enter the transport */
            EnterTransport(selectedTransport, selectedTransportIdArrivalIsA, mainFunction);
            MovementManager.StopMove();

            /* step : travel */
            TravelPatiently(selectedTransport, selectedTransportIdArrivalIsA);
            MovementManager.StopMove();

            bool pathResultSuccess;
            bool path2ResultSuccess;
            List<Point> pts = PathFinder.FindPath(ObjectManager.ObjectManager.Me.Position, selectedTransportIdArrivalIsA ? selectedTransport.OutsideBPoint : selectedTransport.OutsideAPoint, Usefuls.ContinentNameMpq,
                out pathResultSuccess);
            List<Point> pts2 = PathFinder.FindPath(ObjectManager.ObjectManager.Me.Position, selectedTransportIdArrivalIsA ? selectedTransport.OutsideAPoint : selectedTransport.OutsideBPoint,
                Usefuls.ContinentNameMpqByContinentId(selectedTransportIdArrivalIsA ? selectedTransport.AContinentId : selectedTransport.BContinentId), out path2ResultSuccess);
            if (pathResultSuccess && !path2ResultSuccess)
                StartTravel(travelTo, travelToContinentId, mainFunction);
            else if (pathResultSuccess)
            {
                if (GetPathDistance(pts) < GetPathDistance(pts2))
                    StartTravel(travelTo, travelToContinentId, mainFunction);
            }
            /* step : Go out of the transport */
            LeaveTransport(selectedTransport, selectedTransportIdArrivalIsA, mainFunction);
            MovementManager.StopMove();

            if (!mainFunction)
                return;
            /* step : release the product */
            Logging.Write("Travel accomplished, sent notification to product to take the control back.");
            TravelToContinentId = 9999999;
            TravelTo = new Point();
        }

        private void SelectTransport(Point travelFrom, Point travelTo, int travelToContinentId, out Transport selectedTransport, out bool selectedTransportIdArrivalIsA, out float selectedDistanceTransportId,
            bool mainFunction = true, bool infoOnly = false)
        {
            List<Transport> listTransport = ListTransportForContinent(travelToContinentId, Usefuls.ContinentId);
            if (mainFunction)
                Logging.Write(listTransport.Count + " transports can travel to this destination.");
            else
                Logging.Write(listTransport.Count + " intermediate transports can travel to this destination.");
            var bestTransport = new Transport();
            bool bestTransportIdArrivalIsA = false;
            float bestTransportIdDistance = float.MaxValue;
            bool pathResultSuccess;
            var path = new List<Point>();
            if (travelToContinentId == Usefuls.ContinentId)
            {
                path = PathFinder.FindPath(ObjectManager.ObjectManager.Me.Position, travelTo, Usefuls.ContinentNameMpq, out pathResultSuccess);
                if (pathResultSuccess)
                {
                    bestTransportIdDistance = GetPathDistance(path);
                }
            }
            bool hasTravelled;
            SelectTransport2(listTransport, travelFrom, travelTo, ref bestTransport, ref bestTransportIdArrivalIsA, ref bestTransportIdDistance, out hasTravelled, mainFunction);

            if (hasTravelled)
            {
                SelectTransport(ObjectManager.ObjectManager.Me.Position, travelTo, travelToContinentId, out selectedTransport, out selectedTransportIdArrivalIsA, out selectedDistanceTransportId);
                bestTransport = selectedTransport;
                bestTransportIdArrivalIsA = selectedTransportIdArrivalIsA;
                bestTransportIdDistance = selectedDistanceTransportId;
            }
            if (bestTransport.Id == 0)
            {
                if (mainFunction)
                {
                    TravelToContinentId = 9999999;
                    TravelTo = new Point();
                    Logging.Write(Math.Abs(bestTransportIdDistance - float.MaxValue) > 0.1
                        ? "No transports selected because it will be faster to go there by ourself."
                        : "No transports selected due to incapacity to reach the quay with PathFinder.");
                }
                selectedTransport = new Transport();
                selectedTransportIdArrivalIsA = false;
                selectedDistanceTransportId = float.MaxValue;
                return;
            }
            selectedTransport = bestTransport; // Define which transport we will be using.
            selectedTransportIdArrivalIsA = bestTransportIdArrivalIsA; // Define if we must use A or B as departure.
            selectedDistanceTransportId = bestTransportIdDistance;
            if (mainFunction)
                Logging.Write("Selected transport: " + selectedTransport.Name + "(" + selectedTransport.Id + "), departure quay at " +
                              (selectedTransportIdArrivalIsA ? selectedTransport.BPoint : selectedTransport.APoint) + ".");
            else
                Logging.Write("Selected intermediate transport: " + selectedTransport.Name + "(" + selectedTransport.Id + "), departure quay at " +
                              (selectedTransportIdArrivalIsA ? selectedTransport.BPoint : selectedTransport.APoint) + ".");
        }

        private void SelectTransport2(List<Transport> listTransport, Point startingPoint, Point destinationPoint, ref Transport bestTransport, ref bool bestTransportIdArrivalIsA, ref float bestTransportIdDistance,
            out bool hasTravelled, bool mainFunction = true)
        {
            Transport secondarySelectedTransport;
            bool secondarySelectedTransportIdArrivalIsA;
            float secondarySelectedDistanceTransportId;
            var secondaryBestTransport = new Transport();
            bool secondaryBestTransportIdArrivalIsA = false;
            float secondaryBestTransportIdDistance = float.MaxValue;
            hasTravelled = false;
            foreach (Transport t in listTransport)
            {
                bool path1ResultSuccess = false;
                var path1 = new List<Point>();
                bool path1BisResultSuccess = false;
                var path1Bis = new List<Point>();
                bool path2ResultSuccess = false;
                var path2 = new List<Point>();
                bool path2BisResultSuccess = false;
                var path2Bis = new List<Point>();
                if (t.AContinentId == t.BContinentId)
                {
                    // we don't change continent, so we must find the closer point and/or accessible one.
                    path1 = PathFinder.FindPath(startingPoint, t.OutsideAPoint, Usefuls.ContinentNameMpq, out path1ResultSuccess);
                    path1Bis = PathFinder.FindPath(t.OutsideBPoint, destinationPoint, Usefuls.ContinentNameMpq, out path1BisResultSuccess);
                    path2 = PathFinder.FindPath(startingPoint, t.OutsideBPoint, Usefuls.ContinentNameMpq, out path2ResultSuccess);
                    path2Bis = PathFinder.FindPath(t.OutsideAPoint, destinationPoint, Usefuls.ContinentNameMpq, out path2BisResultSuccess);
                }
                else if (t.AContinentId == Usefuls.ContinentId)
                {
                    path1 = PathFinder.FindPath(startingPoint, t.OutsideAPoint, Usefuls.ContinentNameMpq, out path1ResultSuccess);
                    path1Bis = PathFinder.FindPath(t.OutsideBPoint, destinationPoint, Usefuls.ContinentNameMpqByContinentId(t.BContinentId), out path1BisResultSuccess);
                }
                else if (t.BContinentId == Usefuls.ContinentId)
                {
                    path2 = PathFinder.FindPath(startingPoint, t.OutsideBPoint, Usefuls.ContinentNameMpq, out path2ResultSuccess);
                    path2Bis = PathFinder.FindPath(t.OutsideAPoint, destinationPoint, Usefuls.ContinentNameMpqByContinentId(t.AContinentId), out path2BisResultSuccess);
                }
                if (path1ResultSuccess && path1BisResultSuccess && (t.AContinentId == Usefuls.ContinentId || t.AContinentId == t.BContinentId))
                {
                    float path1Distance = GetPathDistance(path1);
                    float path1BisDistance = GetPathDistance(path1Bis);
                    if (bestTransportIdDistance > (path1Distance + path1BisDistance))
                    {
                        bestTransport = t;
                        bestTransportIdArrivalIsA = false;
                        bestTransportIdDistance = path1Distance;
                    }
                }
                else if (!path1ResultSuccess && path1BisResultSuccess && (t.AContinentId == Usefuls.ContinentId || t.AContinentId == t.BContinentId))
                {
                    SelectTransport(ObjectManager.ObjectManager.Me.Position, t.OutsideAPoint, t.AContinentId, out secondarySelectedTransport, out secondarySelectedTransportIdArrivalIsA,
                        out secondarySelectedDistanceTransportId, false);
                    if (secondarySelectedTransport.Id != 0)
                    {
                        if (secondaryBestTransportIdDistance > secondarySelectedDistanceTransportId)
                        {
                            secondaryBestTransport = secondarySelectedTransport;
                            secondaryBestTransportIdArrivalIsA = true;
                            secondaryBestTransportIdDistance = secondarySelectedDistanceTransportId;
                        }
                    }
                }
                if (path2ResultSuccess && path2BisResultSuccess && (t.BContinentId == Usefuls.ContinentId || t.AContinentId == t.BContinentId))
                {
                    float path2Distance = GetPathDistance(path2);
                    float path2BisDistance = GetPathDistance(path2Bis);
                    if (bestTransportIdDistance > (path2Distance + path2BisDistance))
                    {
                        bestTransport = t;
                        bestTransportIdArrivalIsA = true;
                        bestTransportIdDistance = path2Distance;
                    }
                }
                else if (!path2ResultSuccess && path2BisResultSuccess && (t.BContinentId == Usefuls.ContinentId || t.AContinentId == t.BContinentId))
                {
                    SelectTransport(ObjectManager.ObjectManager.Me.Position, t.OutsideBPoint, t.BContinentId, out secondarySelectedTransport, out secondarySelectedTransportIdArrivalIsA,
                        out secondarySelectedDistanceTransportId, false);
                    if (secondarySelectedTransport.Id != 0)
                    {
                        if (secondaryBestTransportIdDistance > secondarySelectedDistanceTransportId)
                        {
                            secondaryBestTransport = secondarySelectedTransport;
                            secondaryBestTransportIdArrivalIsA = true;
                            secondaryBestTransportIdDistance = secondarySelectedDistanceTransportId;
                        }
                    }
                }
            }
            if (bestTransport.Id == 0 && secondaryBestTransport.Id != 0)
            {
                // we cannot go to a direct transport to the destination, but we can go to a transport that go to another transport
                // it should check recursivly
                StartTravel((secondaryBestTransportIdArrivalIsA ? secondaryBestTransport.OutsideBPoint : secondaryBestTransport.OutsideAPoint),
                    (secondaryBestTransportIdArrivalIsA ? secondaryBestTransport.BContinentId : secondaryBestTransport.AContinentId), false);
                hasTravelled = true;
            }
        }

        private void GoToDepartureQuay(Transport selectedTransport, bool selectedTransportIdArrivalIsA, bool mainFunction = true)
        {
            List<Point> pathToDepartureQuay = selectedTransportIdArrivalIsA ? PathFinder.FindPath(selectedTransport.OutsideBPoint) : PathFinder.FindPath(selectedTransport.OutsideAPoint);
            MovementManager.Go(pathToDepartureQuay);
            if (mainFunction)
                Logging.Write("Going to departure quay of " + selectedTransport.Name + "(" + selectedTransport.Id + ").");
            else
                Logging.Write("Going to intermediate departure quay of " + selectedTransport.Name + "(" + selectedTransport.Id + ").");
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
            if (mainFunction)
                Logging.Write("Arrived at departure quay of " + selectedTransport.Name + "(" + selectedTransport.Id + "), waiting for transport.");
            else
                Logging.Write("Arrived at intermediate departure quay of " + selectedTransport.Name + "(" + selectedTransport.Id + "), waiting for transport.");
        }

        private void WaitForTransport(Transport selectedTransport, bool selectedTransportIdArrivalIsA, bool mainFunction = true)
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
                    if (ObjectManager.ObjectManager.Me.Position.DistanceTo((selectedTransportIdArrivalIsA ? selectedTransport.OutsideBPoint : selectedTransport.OutsideAPoint)) > 10)
                        GoToDepartureQuay(selectedTransport, selectedTransportIdArrivalIsA, mainFunction); // wrong quay because we felt and it tryed to find a transport while in the air
                    Thread.Sleep(50); // Wait for transport to get into position.
                }
                else
                {
                    TargetIsGameObject = ObjectManager.ObjectManager.GetNearestWoWGameObject(ObjectManager.ObjectManager.GetWoWGameObjectByEntry((int) selectedTransport.Id), ObjectManager.ObjectManager.Me.Position);
                }
            }
        }

        private void EnterTransport(Transport selectedTransport, bool selectedTransportIdArrivalIsA, bool mainFunction = true)
        {
            if (mainFunction)
                Logging.Write("Transport " + selectedTransport.Name + "(" + selectedTransport.Id + ") arrived at the quay, entering transport.");
            else
                Logging.Write("Intermediate Transport " + selectedTransport.Name + "(" + selectedTransport.Id + ") arrived at the quay, entering transport.");
            while (!ObjectManager.ObjectManager.Me.InTransport)
            {
                //continue;
                if (ObjectManager.ObjectManager.Me.Position.Z + 30 < (selectedTransportIdArrivalIsA ? selectedTransport.BPoint : selectedTransport.APoint).Z
                    || ObjectManager.ObjectManager.Me.Position.Z - 30 > (selectedTransportIdArrivalIsA ? selectedTransport.BPoint : selectedTransport.APoint).Z)
                {
                    if (mainFunction)
                        Logging.Write("Failed to enter transport " + selectedTransport.Name + "(" + selectedTransport.Id + ") going back to the quay.");
                    else
                        Logging.Write("Failed to enter intermediate transport " + selectedTransport.Name + "(" + selectedTransport.Id + ") going back to the quay.");
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
            if (mainFunction)
                Logging.Write("Successfuly entered transport " + selectedTransport.Name + "(" + selectedTransport.Id + "), waiting to arrive at destination.");
            else
                Logging.Write("Successfuly entered intermediate transport " + selectedTransport.Name + "(" + selectedTransport.Id + "), waiting to arrive at destination.");
        }

        private void LeaveTransport(Transport selectedTransport, bool selectedTransportIdArrivalIsA, bool mainFunction = true)
        {
            if (mainFunction)
                Logging.Write("Transport " + selectedTransport.Name + "(" + selectedTransport.Id + ") arrived at destination, leaving to the arrival quay.");
            else
                Logging.Write("Intermediate Transport " + selectedTransport.Name + "(" + selectedTransport.Id + ") arrived at destination, leaving to the arrival quay.");
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
            int i =0;
            while (loop)
            {
                if (!ObjectManager.ObjectManager.Me.InTransport && Usefuls.InGame && !Usefuls.IsLoadingOrConnecting)
                {
                    if (i > 5)
                        loop = false;
                    i++;
                    Thread.Sleep(300); // let time to the game to detect out transport
                }
                if (selectedTransportIdArrivalIsA && ObjectManager.ObjectManager.Me.Position.Equals(selectedTransport.APoint))
                    loop = false;
                if (!selectedTransportIdArrivalIsA && ObjectManager.ObjectManager.Me.Position.Equals(selectedTransport.BPoint))
                    loop = false;
                Thread.Sleep(100);
            }
        }
    }
}