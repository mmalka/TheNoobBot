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
        private Portals _availablePortals;
        private Transports _availableTransports;
        private List<Transport> _generatedRoutePath = new List<Transport>();

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
                if (_availablePortals == null)
                    _availablePortals = XmlSerializer.Deserialize<Portals>(Application.StartupPath + @"\Data\PortalsDB.xml");
                if (_availableTransports == null || _availablePortals == null)
                    return false;
                if (!Products.Products.IsStarted || !NeedToTravel)
                    return false;


                _generatedRoutePath = GenerateRoutePath; // Automatically cancel TravelTo if no founds.
                return _generatedRoutePath.Count > 0;
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

        private List<Transport> GenerateRoutePath
        {
            get
            {
                Point travelTo = TravelTo;
                int travelToContinentId = TravelToContinentId;
                int currentContinentId = Usefuls.ContinentId;
                Point currentPosition = ObjectManager.ObjectManager.Me.Position;

                KeyValuePair<Transport, float> oneWayTravel = GetBestDirectWayTransport(currentPosition, travelTo, currentContinentId, travelToContinentId);

                if (currentContinentId == travelToContinentId)
                {
                    bool success;
                    List<Point> way = PathFinder.FindPath(currentPosition, travelTo, Usefuls.ContinentNameMpq, out success);
                    if (success)
                    {
                        if (oneWayTravel.Value > GetPathDistance(way))
                        {
                            TravelToContinentId = 9999999;
                            TravelTo = new Point();
                            Logging.Write("Travel: Found a faster path without using Transports. Cancelling Travel.");
                            return new List<Transport>();
                        }
                    }
                }
                if (oneWayTravel.Key.Id != 0)
                {
                    Logging.Write("Travel: Found direct way travel.");
                    return new List<Transport> {oneWayTravel.Key};
                }

                KeyValuePair<List<Transport>, float> twoWayTravel = GetBestTwoWayTransport(currentPosition, travelTo, currentContinentId, travelToContinentId);

                if (oneWayTravel.Key.Id != 0 && twoWayTravel.Key.Count == 2 && oneWayTravel.Value <= twoWayTravel.Value)
                {
                    Logging.Write("Travel: Found a direct way travel that is faster than a 2-way travel.");
                    return new List<Transport> {oneWayTravel.Key};
                }
                if (oneWayTravel.Key.Id != 0 && twoWayTravel.Key.Count == 2 && oneWayTravel.Value > twoWayTravel.Value)
                {
                    Logging.Write("Travel: Found a 2-way travel that is faster than a direct way travel.");
                    return twoWayTravel.Key;
                }

                //KeyValuePair<List<Transport>, float> threeWayTravel = GetBestThreeWayTransport(currentPosition, travelTo, currentContinentId, travelToContinentId);
                //KeyValuePair<List<Transport>, float> threeWayTravel = GetBestFourthWayTransport(currentPosition, travelTo, currentContinentId, travelToContinentId);
                //KeyValuePair<List<Transport>, float> threeWayTravel = GetBestFifthWayTransport(currentPosition, travelTo, currentContinentId, travelToContinentId);
                // todo: support up to 5 way travel and check the fastest every 2 ways. (1-2, 2-3, 3-4, 4-5)
                TravelToContinentId = 9999999;
                TravelTo = new Point();
                Logging.Write("Travel: Couldn't find a travel path. Checked up to 2 way travel.");
                return new List<Transport>();
            }
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

            foreach (Transport transport in _generatedRoutePath)
            {
                GoToDepartureQuayOrPortal(transport);
                if (ObjectManager.ObjectManager.Me.InCombat || ObjectManager.ObjectManager.Me.IsDead)
                    return;
                if (!(transport is Portal))
                {
                    WaitForTransport(transport);
                    if (ObjectManager.ObjectManager.Me.InCombat || ObjectManager.ObjectManager.Me.IsDead)
                        return;
                }
                EnterTransportOrTakePortal(transport);
                if (ObjectManager.ObjectManager.Me.InCombat || ObjectManager.ObjectManager.Me.IsDead)
                    return;
                if (!(transport is Portal))
                {
                    TravelPatiently(transport);
                    if (ObjectManager.ObjectManager.Me.InCombat || ObjectManager.ObjectManager.Me.IsDead)
                        return;
                    LeaveTransport(transport);
                }
            }
            TravelToContinentId = 9999999;
            TravelTo = new Point();
            Logging.Write("Travel is terminated, waiting for product to take the control back.");
        }

        private void GoToDepartureQuayOrPortal(Transport selectedTransport)
        {
            if (selectedTransport is Portal)
            {
                var portal = selectedTransport as Portal;
                Logging.Write("Going to portal " + portal.Name + "(" + portal.Id + ") to travel.");
                List<Point> pathToPortal = PathFinder.FindPath(portal.APoint);
                MovementManager.Go(pathToPortal);
                bool loop = true;
                while (loop)
                {
                    if (ObjectManager.ObjectManager.Me.InCombat || ObjectManager.ObjectManager.Me.IsDead)
                        return;
                    if (ObjectManager.ObjectManager.Me.Position.DistanceTo(portal.APoint) < 2)
                        loop = false;
                    Thread.Sleep(100);
                }
                MovementManager.StopMove();
            }
            else
            {
                List<Point> pathToDepartureQuay = selectedTransport.ArrivalIsA ? PathFinder.FindPath(selectedTransport.BOutsidePoint) : PathFinder.FindPath(selectedTransport.AOutsidePoint);
                MovementManager.Go(pathToDepartureQuay);
                Logging.Write("Going to departure quay of " + selectedTransport.Name + "(" + selectedTransport.Id + ") to travel.");
                bool loop = true;
                while (loop)
                {
                    if (ObjectManager.ObjectManager.Me.InCombat || ObjectManager.ObjectManager.Me.IsDead)
                        return;
                    if (ObjectManager.ObjectManager.Me.Position.DistanceTo(selectedTransport.ArrivalIsA ? selectedTransport.BOutsidePoint : selectedTransport.AOutsidePoint) < 2)
                        loop = false;
                    if (!MovementManager.InMoveTo && !MovementManager.InMovement)
                        loop = false;
                    Thread.Sleep(100);
                }
                MovementManager.StopMove();
                Logging.Write("Arrived at departure quay of " + selectedTransport.Name + "(" + selectedTransport.Id + "), waiting for transport.");
            }
        }

        private void WaitForTransport(Transport selectedTransport)
        {
            WoWGameObject memoryTransport = ObjectManager.ObjectManager.GetNearestWoWGameObject(ObjectManager.ObjectManager.GetWoWGameObjectByEntry((int) selectedTransport.Id),
                ObjectManager.ObjectManager.Me.Position);
            bool loop = true;
            while (loop)
            {
                if (Usefuls.IsFlying)
                    MountTask.DismountMount();
                if (memoryTransport.IsValid)
                {
                    if ((selectedTransport.ArrivalIsA ? selectedTransport.BPoint : selectedTransport.APoint).Equals(memoryTransport.Position))
                        loop = false;
                    if (ObjectManager.ObjectManager.Me.Position.DistanceTo((selectedTransport.ArrivalIsA ? selectedTransport.BOutsidePoint : selectedTransport.AOutsidePoint)) > 5)
                    {
                        GoToDepartureQuayOrPortal(selectedTransport); // wrong quay because we felt and it tryed to find a transport while in the air
                        return;
                    }
                    Thread.Sleep(100); // Wait for transport to get into position.
                }
                else
                {
                    memoryTransport = ObjectManager.ObjectManager.GetNearestWoWGameObject(ObjectManager.ObjectManager.GetWoWGameObjectByEntry((int) selectedTransport.Id), ObjectManager.ObjectManager.Me.Position);
                }
            }
        }

        private void EnterTransportOrTakePortal(Transport selectedTransport)
        {
            if (selectedTransport is Portal)
            {
                var portal = selectedTransport as Portal;
                WoWGameObject memoryPortal = ObjectManager.ObjectManager.GetNearestWoWGameObject(ObjectManager.ObjectManager.GetWoWGameObjectByEntry((int) portal.Id), ObjectManager.ObjectManager.Me.Position);
                bool loop = true;
                while (loop)
                {
                    if (Usefuls.IsFlying)
                        MountTask.DismountMount();
                    if (memoryPortal.IsValid)
                    {
                        if (memoryPortal.GetDistance > 4)
                        {
                            List<Point> path = PathFinder.FindPath(memoryPortal.Position);
                            MovementManager.Go(path);
                            while (memoryPortal.GetDistance > 4)
                            {
                                if (ObjectManager.ObjectManager.Me.InCombat || ObjectManager.ObjectManager.Me.IsDead)
                                {
                                    return;
                                }
                            }
                            Interact.InteractWith(memoryPortal.GetBaseAddress, true);
                            Thread.Sleep(3000);
                            if (memoryPortal.IsValid)
                            {
                                Interact.InteractWith(memoryPortal.GetBaseAddress, true);
                                Thread.Sleep(6000);
                            }
                            loop = false;
                        }
                    }
                    else
                    {
                        if (portal.APoint.DistanceTo(ObjectManager.ObjectManager.Me.Position) > 5)
                        {
                            GoToDepartureQuayOrPortal(selectedTransport);
                            EnterTransportOrTakePortal(selectedTransport);
                            return;
                        }
                        memoryPortal = ObjectManager.ObjectManager.GetNearestWoWGameObject(ObjectManager.ObjectManager.GetWoWGameObjectByEntry((int) portal.Id), ObjectManager.ObjectManager.Me.Position);
                    }
                }
            }
            else
            {
                Logging.Write("Transport " + selectedTransport.Name + "(" + selectedTransport.Id + ") arrived at the quay, entering transport.");
                while (!ObjectManager.ObjectManager.Me.InTransport)
                {
                    if (ObjectManager.ObjectManager.Me.Position.DistanceTo((selectedTransport.ArrivalIsA ? selectedTransport.BOutsidePoint : selectedTransport.AOutsidePoint)) > 10)
                    {
                        if (ObjectManager.ObjectManager.Me.Position.DistanceTo((selectedTransport.ArrivalIsA ? selectedTransport.BInsidePoint : selectedTransport.AInsidePoint)) >
                            ObjectManager.ObjectManager.Me.Position.DistanceTo((selectedTransport.ArrivalIsA ? selectedTransport.BOutsidePoint : selectedTransport.AOutsidePoint)))
                        {
                            GoToDepartureQuayOrPortal(selectedTransport);
                            EnterTransportOrTakePortal(selectedTransport);
                            Logging.Write("Failed to enter transport " + selectedTransport.Name + "(" + selectedTransport.Id + ") going back to the quay.");
                        }
                    }
                    MovementManager.MoveTo(selectedTransport.ArrivalIsA ? selectedTransport.BInsidePoint : selectedTransport.AInsidePoint);
                    bool loop = true;
                    while (loop)
                    {
                        if (ObjectManager.ObjectManager.Me.InCombat || ObjectManager.ObjectManager.Me.IsDead)
                            return;
                        if (ObjectManager.ObjectManager.Me.InTransport)
                        {
                            loop = false;
                            Thread.Sleep(1000);
                        }
                        if (ObjectManager.ObjectManager.Me.Position.DistanceTo(selectedTransport.ArrivalIsA ? selectedTransport.BInsidePoint : selectedTransport.AInsidePoint) <= 2)
                        {
                            loop = false;
                            MovementManager.StopMove();
                            Thread.Sleep(100);
                            if (!ObjectManager.ObjectManager.Me.InTransport)
                                MovementsAction.Jump(); // We sometimes need to jump at the bottom of an elevator to be "inside" of it.
                        }
                        Thread.Sleep(500);
                    }
                }
                Thread.Sleep(100);
                MovementManager.StopMove();
                Logging.Write("Successfuly entered transport " + selectedTransport.Name + "(" + selectedTransport.Id + "), waiting to arrive at destination.");
            }
        }

        private void LeaveTransport(Transport selectedTransport)
        {
            Logging.Write("Transport " + selectedTransport.Name + "(" + selectedTransport.Id + ") arrived at destination, leaving to the arrival quay.");
            MovementManager.MoveTo(selectedTransport.ArrivalIsA ? selectedTransport.AOutsidePoint : selectedTransport.BOutsidePoint);
            bool loop = true;
            while (loop)
            {
                if (ObjectManager.ObjectManager.Me.InCombat || ObjectManager.ObjectManager.Me.IsDead)
                    return;
                if (!ObjectManager.ObjectManager.Me.InTransport)
                    loop = false;
                if (ObjectManager.ObjectManager.Me.Position.DistanceTo(selectedTransport.ArrivalIsA ? selectedTransport.AOutsidePoint : selectedTransport.BOutsidePoint) < 5)
                    loop = false;
                Thread.Sleep(500);
            }
        }

        private void TravelPatiently(Transport selectedTransport)
        {
            WoWGameObject memoryTransport = ObjectManager.ObjectManager.GetNearestWoWGameObject(ObjectManager.ObjectManager.GetWoWGameObjectByEntry((int) selectedTransport.Id),
                ObjectManager.ObjectManager.Me.Position);
            bool loop = true;
            int i = 0;
            int i2 = 0;
            while (loop)
            {
                if (!ObjectManager.ObjectManager.Me.InTransport && Usefuls.InGame && !Usefuls.IsLoadingOrConnecting)
                {
                    if (i > 5)
                        loop = false;
                    i++;
                    Thread.Sleep(300); // Shortly after loading, we are not yet "InTransport".
                }
                if (selectedTransport.ArrivalIsA && memoryTransport.Position.Equals(selectedTransport.APoint))
                    loop = false;
                if (!selectedTransport.ArrivalIsA && memoryTransport.Position.Equals(selectedTransport.BPoint))
                    loop = false;
                if (!memoryTransport.IsValid && i2 < 5)
                {
                    memoryTransport = ObjectManager.ObjectManager.GetNearestWoWGameObject(ObjectManager.ObjectManager.GetWoWGameObjectByEntry((int) selectedTransport.Id),
                        ObjectManager.ObjectManager.Me.Position);
                    i2++;
                }
                else if (!memoryTransport.IsValid && i2 >= 5)
                {
                    loop = false;
                }
                Thread.Sleep(500);
            }
        }

        private List<Transport> GetAllTransportsThatGoesToDestination(Point travelTo, int travelToContinentId)
        {
            var allTransports = new List<Transport>();
            List<Transport> transports = GetTransportsThatGoesToDestination(travelTo, travelToContinentId);
            List<Portal> portals = GetPortalsThatGoesToDestination(travelTo, travelToContinentId);
            allTransports.AddRange(transports);
            allTransports.AddRange(portals);
            return allTransports;
        }

        private List<Transport> GetTransportsThatGoesToDestination(Point travelTo, int travelToContinentId)
        {
            var listTransport = new List<Transport>();
            foreach (Transport transport in _availableTransports.Items)
            {
                if (transport.Faction != Npc.FactionType.Neutral && transport.Faction.ToString() != ObjectManager.ObjectManager.Me.PlayerFaction)
                    continue;
                if (transport.AContinentId != travelToContinentId && transport.BContinentId != travelToContinentId)
                    continue;
                if (transport.AContinentId == travelToContinentId && transport.BContinentId != travelToContinentId)
                {
                    bool success;
                    PathFinder.FindPath(transport.AOutsidePoint, travelTo, Usefuls.ContinentNameMpqByContinentId(travelToContinentId), out success);
                    if (success)
                    {
                        transport.ArrivalIsA = true;
                        listTransport.Add(transport);
                    }
                }
                else if (transport.BContinentId == travelToContinentId && transport.AContinentId != travelToContinentId)
                {
                    bool success;
                    PathFinder.FindPath(transport.BOutsidePoint, travelTo, Usefuls.ContinentNameMpqByContinentId(travelToContinentId), out success);
                    if (success)
                    {
                        listTransport.Add(transport);
                    }
                }
                else if (transport.AContinentId == travelToContinentId && transport.BContinentId == travelToContinentId)
                {
                    bool success;
                    PathFinder.FindPath(transport.AOutsidePoint, travelTo, Usefuls.ContinentNameMpqByContinentId(travelToContinentId), out success);
                    if (success)
                    {
                        transport.ArrivalIsA = true;
                        listTransport.Add(transport);
                    }
                    PathFinder.FindPath(transport.BOutsidePoint, travelTo, Usefuls.ContinentNameMpqByContinentId(travelToContinentId), out success);
                    if (success)
                    {
                        transport.ArrivalIsA = false;
                        listTransport.Add(transport);
                    }
                }
            }
            return listTransport;
        }

        private List<Portal> GetPortalsThatGoesToDestination(Point travelTo, int travelToContinentId)
        {
            var listPortal = new List<Portal>();
            foreach (Portal portal in _availablePortals.Items)
            {
                if (portal.Faction != Npc.FactionType.Neutral && portal.Faction.ToString() != ObjectManager.ObjectManager.Me.PlayerFaction)
                    continue;
                if (portal.BContinentId != travelToContinentId)
                    continue;
                bool success;
                PathFinder.FindPath(portal.BPoint, travelTo, Usefuls.ContinentNameMpqByContinentId(travelToContinentId), out success);
                if (success)
                {
                    listPortal.Add(portal);
                }
            }
            return listPortal;
        }

        private List<Portal> GetPortalsThatDirectlyGoToDestination(Point travelTo, Point travelFrom, int travelToContinentId, int travelFromContinentId)
        {
            var listPortal = new List<Portal>();
            List<Portal> portals = GetPortalsThatGoesToDestination(travelTo, travelToContinentId);
            foreach (Portal portal in portals)
            {
                if (portal.AContinentId != travelFromContinentId)
                    continue;
                bool success;
                PathFinder.FindPath(portal.APoint, travelFrom, Usefuls.ContinentNameMpqByContinentId(travelFromContinentId), out success);
                if (success)
                {
                    listPortal.Add(portal);
                }
            }
            return listPortal;
        }

        private List<Transport> GetTransportsThatDirectlyGoToDestination(Point travelTo, Point travelFrom, int travelToContinentId, int travelFromContinentId)
        {
            var listTransport = new List<Transport>();
            List<Transport> transports = GetTransportsThatGoesToDestination(travelTo, travelToContinentId);
            foreach (Transport transport in transports)
            {
                if (transport.ArrivalIsA)
                {
                    if (transport.BContinentId != travelFromContinentId)
                        continue;
                    bool success;
                    PathFinder.FindPath(transport.BOutsidePoint, travelFrom, Usefuls.ContinentNameMpqByContinentId(travelFromContinentId), out success);
                    if (success)
                    {
                        listTransport.Add(transport);
                    }
                }
                else
                {
                    if (transport.AContinentId != travelFromContinentId)
                        continue;
                    bool success;
                    PathFinder.FindPath(transport.AOutsidePoint, travelFrom, Usefuls.ContinentNameMpqByContinentId(travelFromContinentId), out success);
                    if (success)
                    {
                        listTransport.Add(transport);
                    }
                }
            }
            return listTransport;
        }

        private List<Transport> GetAllTransportsThatDirectlyGoToDestination(Point travelTo, Point travelFrom, int travelToContinentId, int travelFromContinentId)
        {
            var allTransports = new List<Transport>();
            List<Transport> transports = GetTransportsThatDirectlyGoToDestination(travelTo, travelFrom, travelToContinentId, travelFromContinentId);
            List<Portal> portals = GetPortalsThatDirectlyGoToDestination(travelTo, travelFrom, travelToContinentId, travelFromContinentId);
            allTransports.AddRange(transports);
            allTransports.AddRange(portals);
            return allTransports;
        }

        private KeyValuePair<Transport, float> GetBestDirectWayTransport(Point travelFrom, Point travelTo, int travelFromContinentId, int travelToContinentId)
        {
            var bestTransport = new Transport();
            float bestTransportDistance = float.MaxValue;
            List<Transport> allTransports = GetAllTransportsThatDirectlyGoToDestination(travelTo, travelFrom, travelToContinentId, travelFromContinentId);
            foreach (Transport transport in allTransports)
            {
                float currentTransportDistance;
                if (transport is Portal)
                {
                    var portal = transport as Portal;
                    List<Point> wayIn = PathFinder.FindPath(travelFrom, portal.APoint, Usefuls.ContinentNameMpqByContinentId(travelFromContinentId));
                    List<Point> wayOff = PathFinder.FindPath(portal.BPoint, travelTo, Usefuls.ContinentNameMpqByContinentId(travelToContinentId));
                    currentTransportDistance = GetPathDistance(wayIn) + GetPathDistance(wayOff);
                }
                else
                {
                    if (transport.ArrivalIsA)
                    {
                        List<Point> wayIn = PathFinder.FindPath(travelFrom, transport.BOutsidePoint, Usefuls.ContinentNameMpqByContinentId(travelFromContinentId));
                        List<Point> wayOff = PathFinder.FindPath(transport.AOutsidePoint, travelTo, Usefuls.ContinentNameMpqByContinentId(travelToContinentId));
                        currentTransportDistance = GetPathDistance(wayIn) + GetPathDistance(wayOff);
                    }
                    else
                    {
                        List<Point> wayIn = PathFinder.FindPath(travelFrom, transport.AOutsidePoint, Usefuls.ContinentNameMpqByContinentId(travelFromContinentId));
                        List<Point> wayOff = PathFinder.FindPath(transport.BOutsidePoint, travelTo, Usefuls.ContinentNameMpqByContinentId(travelToContinentId));
                        currentTransportDistance = GetPathDistance(wayIn) + GetPathDistance(wayOff);
                    }
                }

                if (!(currentTransportDistance < bestTransportDistance)) continue;
                bestTransport = transport;
                bestTransportDistance = currentTransportDistance;
            }
            return bestTransport.Id != 0 ? new KeyValuePair<Transport, float>(bestTransport, bestTransportDistance) : new KeyValuePair<Transport, float>(new Transport(), float.MaxValue);
        }

        private KeyValuePair<List<Transport>, float> GetBestTwoWayTransport(Point travelFrom, Point travelTo, int travelFromContinentId, int travelToContinentId)
        {
            var bestTransports = new List<Transport>();
            float bestTransportDistance = float.MaxValue;
            List<Transport> allTransports = GetAllTransportsThatGoesToDestination(travelTo, travelToContinentId);
            foreach (Transport transport in allTransports)
            {
                float currentTransportDistance;
                if (transport is Portal)
                {
                    var portal = transport as Portal;
                    if (portal.AContinentId != travelFromContinentId)
                        continue;
                    KeyValuePair<Transport, float> currentToIntermediate = GetBestDirectWayTransport(travelFrom, portal.APoint, travelFromContinentId, portal.BContinentId);
                    List<Point> way = PathFinder.FindPath(portal.BPoint, travelTo, Usefuls.ContinentNameMpqByContinentId(travelToContinentId));
                    currentTransportDistance = currentToIntermediate.Value + GetPathDistance(way);

                    if (currentTransportDistance < bestTransportDistance)
                    {
                        bestTransports = new List<Transport> {currentToIntermediate.Key, transport};
                        bestTransportDistance = currentTransportDistance;
                    }
                }
                else
                {
                    if (transport.ArrivalIsA)
                    {
                        if (transport.BContinentId != travelFromContinentId)
                            continue;
                        KeyValuePair<Transport, float> currentToIntermediate = GetBestDirectWayTransport(travelFrom, transport.BOutsidePoint, travelFromContinentId, transport.AContinentId);
                        List<Point> way = PathFinder.FindPath(transport.AOutsidePoint, travelTo, Usefuls.ContinentNameMpqByContinentId(travelToContinentId));
                        currentTransportDistance = currentToIntermediate.Value + GetPathDistance(way);

                        if (currentTransportDistance < bestTransportDistance)
                        {
                            bestTransports = new List<Transport> {currentToIntermediate.Key, transport};
                            bestTransportDistance = currentTransportDistance;
                        }
                    }
                    else
                    {
                        if (transport.AContinentId != travelFromContinentId)
                            continue;
                        KeyValuePair<Transport, float> currentToIntermediate = GetBestDirectWayTransport(travelFrom, transport.AOutsidePoint, travelFromContinentId, transport.BContinentId);
                        List<Point> way = PathFinder.FindPath(transport.BOutsidePoint, travelTo, Usefuls.ContinentNameMpqByContinentId(travelToContinentId));
                        currentTransportDistance = currentToIntermediate.Value + GetPathDistance(way);

                        if (currentTransportDistance < bestTransportDistance)
                        {
                            bestTransports = new List<Transport> {currentToIntermediate.Key, transport};
                            bestTransportDistance = currentTransportDistance;
                        }
                    }
                }
            }
            if (bestTransports.Count == 2)
                return new KeyValuePair<List<Transport>, float>(bestTransports, bestTransportDistance);
            return new KeyValuePair<List<Transport>, float>(new List<Transport>(), float.MaxValue);
        }
    }
}