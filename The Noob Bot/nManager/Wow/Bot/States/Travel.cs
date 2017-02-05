using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Bot.Tasks;
using nManager.Wow.Class;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;
using Math = nManager.Helpful.Math;

namespace nManager.Wow.Bot.States
{
    public class Travel : State
    {
        private Portals _availablePortals;
        private Transports _availableTransports;
        private CustomPaths _availableCustomPaths;
        private List<Taxi> _availableTaxis;
        private List<TaxiLink> _availableTaxiLinks;
        private List<Taxi> _unknownTaxis = new List<Taxi>();
        private List<Transport> _generatedRoutePath = new List<Transport>();
        private bool _unknownTaxisChecked = false;

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

        public Func<Point, bool> TargetValidationFct
        {
            get { return Products.Products.TargetValidationFct; }
            set { Products.Products.TargetValidationFct = value; }
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
                {
                    _availableTransports = XmlSerializer.Deserialize<Transports>(Application.StartupPath + @"\Data\TransportsDB.xml");
                    for (int i = _availableTransports.Items.Count - 1; i > 0; i--)
                    {
                        var transport = _availableTransports.Items[i];
                        if (transport.Faction != Npc.FactionType.Neutral && transport.Faction.ToString() != ObjectManager.ObjectManager.Me.PlayerFaction)
                        {
                            _availableTransports.Items.RemoveAt(i);
                        }
                    }
                }
                if (_availablePortals == null)
                {
                    _availablePortals = XmlSerializer.Deserialize<Portals>(Application.StartupPath + @"\Data\PortalsDB.xml");
                    for (int i = _availablePortals.Items.Count - 1; i >= 0; i--)
                    {
                        var portal = _availablePortals.Items[i];
                        if (portal.Faction != Npc.FactionType.Neutral && portal.Faction.ToString() != ObjectManager.ObjectManager.Me.PlayerFaction)
                        {
                            _availablePortals.Items.RemoveAt(i);
                            continue;
                        }
                        if (portal.RequireQuestId > 0 && !Quest.IsQuestFlaggedCompletedLUA(portal.RequireQuestId))
                        {
                            _availablePortals.Items.RemoveAt(i);
                            continue;
                        }

                        if (portal.RequireAchivementId > 0 && !Usefuls.IsCompletedAchievement(portal.RequireAchivementId, true))
                        {
                            _availablePortals.Items.RemoveAt(i);
                            continue; // in case I add more checks, I don't want to forget about this continue.
                        }

                        // We never serialize portals back, so it's all fine.
                    }
                }
                if (_availableCustomPaths == null)
                {
                    _availableCustomPaths = XmlSerializer.Deserialize<CustomPaths>(Application.StartupPath + @"\Data\CustomPathsDB.xml");
                    for (int i = _availableCustomPaths.Items.Count - 1; i >= 0; i--)
                    {
                        var customPath = _availableCustomPaths.Items[i];
                        if (customPath.Faction != Npc.FactionType.Neutral && customPath.Faction.ToString() != ObjectManager.ObjectManager.Me.PlayerFaction)
                        {
                            _availableCustomPaths.Items.RemoveAt(i);
                            continue;
                        }
                        if (customPath.RequireQuestId > 0 && !Quest.IsQuestFlaggedCompletedLUA(customPath.RequireQuestId))
                        {
                            _availableCustomPaths.Items.RemoveAt(i);
                            continue;
                        }

                        if (customPath.RequireAchivementId > 0 && !Usefuls.IsCompletedAchievement(customPath.RequireAchivementId, true))
                        {
                            _availableCustomPaths.Items.RemoveAt(i);
                            continue; // in case I add more checks, I don't want to forget about this continue.
                        }

                        // We never serialize CustomPath back, so it's all fine.
                    }
                }
                if (_availableTaxis == null)
                {
                    _availableTaxis = XmlSerializer.Deserialize<List<Taxi>>(Application.StartupPath + @"\Data\TaxiList.xml");
                    for (int i = _availableTaxis.Count - 1; i >= 0; i--)
                    {
                        var taxis = _availableTaxis[i];
                        if (taxis.Faction != Npc.FactionType.Neutral && taxis.Faction.ToString() != ObjectManager.ObjectManager.Me.PlayerFaction)
                        {
                            _availableTaxis.RemoveAt(i);
                        }
                    }
                }
                if (_availableTaxiLinks == null)
                {
                    _availableTaxiLinks = XmlSerializer.Deserialize<List<TaxiLink>>(Application.StartupPath + @"\Data\TaxiLinks.xml");
                    for (int i = _availableTaxiLinks.Count - 1; i >= 0; i--)
                    {
                        var taxiLink = _availableTaxiLinks[i];
                        if (taxiLink.PointB <= 0)
                            _availableTaxiLinks.RemoveAt(i);
                    }
                }
                if (_availableTransports == null || _availablePortals == null || _availableCustomPaths == null || _availableTaxis == null || _availableTaxiLinks == null)
                    return false;
                if (!Products.Products.IsStarted || ObjectManager.ObjectManager.Me.IsDeadMe || ObjectManager.ObjectManager.Me.InInevitableCombat || !NeedToTravel)
                    return false;
                _generatedRoutePath = GenerateRoutePath; // Automatically cancel TravelTo if not founds.
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

        private bool IsPointValidAsTarget(Point position)
        {
            if (TargetValidationFct != null)
                return TargetValidationFct(position);
            else
                return false;
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
                    if (success || (!success && way.Count >= 1 && IsPointValidAsTarget(way.Last()) && !(oneWayTravel.Key is CustomPath)))
                    {
                        if (oneWayTravel.Value > Math.DistanceListPoint(way))
                        {
                            TravelToContinentId = 9999999;
                            TravelTo = new Point();
                            TargetValidationFct = null;
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
                TargetValidationFct = null;
                Logging.Write("Travel: Couldn't find a travel path. Checked up to 2 way travel.");
                return new List<Transport>();
            }
        }

        public override void Run()
        {
            Logging.Write("Start travel from " + ObjectManager.ObjectManager.Me.Position + " " + Usefuls.ContinentNameMpqByContinentId(Usefuls.ContinentId) + " to " + TravelTo + " " +
                          Usefuls.ContinentNameMpqByContinentId(TravelToContinentId) + ".");
            MovementManager.StopMove();

            foreach (Transport transport in _generatedRoutePath)
            {
                GoToDepartureQuayOrPortal(transport);
                if (ObjectManager.ObjectManager.Me.InInevitableCombat || ObjectManager.ObjectManager.Me.IsDead)
                    return;
                if (!(transport is Portal) && !(transport is Taxi) && !(transport is CustomPath))
                {
                    WaitForTransport(transport);
                    if (ObjectManager.ObjectManager.Me.InInevitableCombat || ObjectManager.ObjectManager.Me.IsDead)
                        return;
                }
                EnterTransportOrTakePortal(transport);
                if (ObjectManager.ObjectManager.Me.InInevitableCombat || ObjectManager.ObjectManager.Me.IsDead)
                    return;
                if (transport is Taxi)
                {
                    TravelPatientlybyTaxiOrPortal();
                }
                else if (!(transport is Portal) && !(transport is CustomPath))
                {
                    TravelPatiently(transport);
                    if (ObjectManager.ObjectManager.Me.InInevitableCombat || ObjectManager.ObjectManager.Me.IsDead)
                        return;
                    LeaveTransport(transport);
                }
            }
            TravelToContinentId = 9999999;
            TravelTo = new Point();
            TargetValidationFct = null;
            Logging.Write("Travel is terminated, waiting for product to take the control back.");
        }

        private void GoToDepartureQuayOrPortal(Transport selectedTransport)
        {
            if (selectedTransport is Portal)
            {
                var portal = selectedTransport as Portal;
                Logging.Write("Going to portal " + portal.Name + " (" + portal.Id + ") to travel.");
                List<Point> pathToPortal = PathFinder.FindPath(portal.APoint);
                MovementManager.Go(pathToPortal);
                bool loop = true;
                while (loop)
                {
                    if (!Products.Products.IsStarted || ObjectManager.ObjectManager.Me.InInevitableCombat || ObjectManager.ObjectManager.Me.IsDead)
                        return;
                    if (ObjectManager.ObjectManager.Me.Position.DistanceTo(portal.APoint) < 2.0f)
                        loop = false;
                    Thread.Sleep(100);
                }
                MovementManager.StopMove();
            }
            else if (selectedTransport is CustomPath)
            {
                var customPath = selectedTransport as CustomPath;
                Logging.Write("Going to CustomPath " + customPath.Name + " (" + customPath.Id + ") to travel.");
                List<Point> pathToPortal = PathFinder.FindPath(customPath.ArrivalIsA ? customPath.BPoint : customPath.APoint);
                MovementManager.Go(pathToPortal);
                bool loop = true;
                while (loop)
                {
                    if (!Products.Products.IsStarted || ObjectManager.ObjectManager.Me.InInevitableCombat || ObjectManager.ObjectManager.Me.IsDead)
                        return;
                    if (ObjectManager.ObjectManager.Me.Position.DistanceTo(customPath.ArrivalIsA ? customPath.BPoint : customPath.APoint) < 2.0f)
                        loop = false;
                    Thread.Sleep(100);
                }
                MovementManager.StopMove();
            }
            else if (selectedTransport is Taxi)
            {
                var taxi = selectedTransport as Taxi;
                Logging.Write("Going to taxi " + taxi.Name + " to travel.");
                // 141605
                if (Usefuls.ContinentId == 1220 && Usefuls.AreaId != 7502 && Usefuls.IsOutdoors && taxi.APoint.DistanceTo(ObjectManager.ObjectManager.Me.Position) > 100f)
                {
                    // We want to uses the item Flight Master's Wistle as much as possible.
                    // We are in Broken Isles, not in Dalaran, outdoor, we should be able to uses it.
                    const int flightMasterWistleId = 141605;
                    if (ItemsManager.GetItemCount(flightMasterWistleId) > 0 && ItemsManager.IsItemUsable(flightMasterWistleId) && !ItemsManager.IsItemOnCooldown(flightMasterWistleId))
                    {
                        ItemsManager.UseItem(flightMasterWistleId);
                        Thread.Sleep(250);
                        while (ObjectManager.ObjectManager.Me.IsCasting)
                        {
                            Thread.Sleep(250);
                            if (!Products.Products.IsStarted || ObjectManager.ObjectManager.Me.InInevitableCombat || ObjectManager.ObjectManager.Me.IsDead)
                                return;
                        }
                        if (!Products.Products.IsStarted || ObjectManager.ObjectManager.Me.InInevitableCombat || ObjectManager.ObjectManager.Me.IsDead)
                            return;
                        Thread.Sleep(10000);
                        TravelToContinentId = 9999999;
                        TravelTo = new Point();
                        TargetValidationFct = null;
                        Logging.Write("We've used Flight Master Wistle, waiting for product to regenerate travel path.");
                        return;
                    }
                }
                List<Point> pathToTaxi = PathFinder.FindPath(taxi.APoint);
                MovementManager.Go(pathToTaxi);
                bool loop = true;
                while (loop)
                {
                    if (!Products.Products.IsStarted || ObjectManager.ObjectManager.Me.InInevitableCombat || ObjectManager.ObjectManager.Me.IsDead)
                        return;
                    if (ObjectManager.ObjectManager.Me.Position.DistanceTo(taxi.APoint) < 4.0f)
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
                    if (!Products.Products.IsStarted || ObjectManager.ObjectManager.Me.InInevitableCombat || ObjectManager.ObjectManager.Me.IsDead)
                        return;
                    if (ObjectManager.ObjectManager.Me.Position.DistanceTo(selectedTransport.ArrivalIsA ? selectedTransport.BOutsidePoint : selectedTransport.AOutsidePoint) < 2.0f)
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
            int i = 0;
            while (loop)
            {
                if (Usefuls.IsFlying)
                    MountTask.DismountMount();
                if (memoryTransport.IsValid)
                {
                    if ((selectedTransport.ArrivalIsA ? selectedTransport.BPoint : selectedTransport.APoint).DistanceTo(memoryTransport.Position) < 0.2f)
                    {
                        if (i > 5)
                            loop = false;
                        i++;
                        Thread.Sleep(300);
                    }
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

        public static event EventHandler AutomaticallyTookTaxi;

        public class TaxiEventArgs : EventArgs
        {
            public int From { get; set; }
            public int To { get; set; }
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
                        if (memoryPortal.GetDistance > 4.0f)
                        {
                            List<Point> path = PathFinder.FindPath(memoryPortal.Position);
                            MovementManager.Go(path);
                            while (memoryPortal.GetDistance > 4.0f)
                            {
                                if (ObjectManager.ObjectManager.Me.InInevitableCombat || ObjectManager.ObjectManager.Me.IsDead)
                                    return;
                                Thread.Sleep(150);
                            }
                        }
                        MountTask.DismountMount();
                        Thread.Sleep(500);
                        Interact.InteractWith(memoryPortal.GetBaseAddress);
                        Thread.Sleep(150);
                        Interact.InteractWith(memoryPortal.GetBaseAddress);
                        TravelPatientlybyTaxiOrPortal();
                        loop = false;
                    }
                    else
                    {
                        if (portal.APoint.DistanceTo(ObjectManager.ObjectManager.Me.Position) > 4.0f)
                        {
                            GoToDepartureQuayOrPortal(selectedTransport);
                            EnterTransportOrTakePortal(selectedTransport);
                            return;
                        }
                        memoryPortal = ObjectManager.ObjectManager.GetNearestWoWGameObject(ObjectManager.ObjectManager.GetWoWGameObjectByEntry((int) portal.Id), ObjectManager.ObjectManager.Me.Position);
                    }
                }
            }
            else if (selectedTransport is CustomPath)
            {
                var customPath = selectedTransport as CustomPath;
                bool loop = true;
                while (loop)
                {
                    if (Usefuls.IsFlying)
                        MountTask.DismountMount();

                    if (customPath.ArrivalIsA ? customPath.BPoint.DistanceTo(ObjectManager.ObjectManager.Me.Position) > 4.0f : customPath.APoint.DistanceTo(ObjectManager.ObjectManager.Me.Position) > 4.0f)
                    {
                        GoToDepartureQuayOrPortal(selectedTransport);
                        EnterTransportOrTakePortal(selectedTransport);
                        return;
                    }
                    // We are at the beginning of the path.
                    List<Point> path = customPath.Points;
                    if (customPath.ArrivalIsA)
                    {
                        List<Point> reversedPath = new List<Point>();
                        reversedPath.AddRange(customPath.Points);
                        reversedPath.Reverse(); // we don't want to mess up with the saved original path.
                        path = reversedPath;
                    }
                    MountTask.DismountMount();
                    Thread.Sleep(500);
                    MovementManager.Go(path);
                    while (MovementManager.InMovement)
                    {
                        if (!Products.Products.IsStarted || ObjectManager.ObjectManager.Me.InInevitableCombat || ObjectManager.ObjectManager.Me.IsDead)
                            return;
                        Thread.Sleep(150);
                    }
                    loop = false;
                }
            }
            else if (selectedTransport is Taxi)
            {
                var taxi = selectedTransport as Taxi;
                WoWUnit memoryTaxi = ObjectManager.ObjectManager.GetNearestWoWUnit(ObjectManager.ObjectManager.GetWoWUnitByEntry((int) taxi.Id));
                bool loop = true;
                while (loop)
                {
                    if (Usefuls.IsFlying)
                        MountTask.DismountMount();
                    if (memoryTaxi.IsValid)
                    {
                        if (memoryTaxi.GetDistance > 4.0f)
                        {
                            List<Point> path = PathFinder.FindPath(memoryTaxi.Position);
                            MovementManager.Go(path);
                            while (memoryTaxi.GetDistance > 4.0f)
                            {
                                if (ObjectManager.ObjectManager.Me.InInevitableCombat || ObjectManager.ObjectManager.Me.IsDead)
                                {
                                    return;
                                }
                                Thread.Sleep(150);
                            }
                        }
                        Taxi nextHop = FindNextTaxiHopFor(taxi, true);
                        if (nextHop == null)
                        {
                            Logging.Write("There is a problem with taxi links, some are missing to complete the minimal graph");
                            return;
                        }
                        MountTask.DismountMount();
                        Interact.InteractWith(memoryTaxi.GetBaseAddress, true);
                        Thread.Sleep(250 + Usefuls.Latency);
                        if (!Gossip.IsTaxiWindowOpen())
                        {
                            Gossip.SelectGossip(Gossip.GossipOption.Taxi);
                            Thread.Sleep(250 + Usefuls.Latency);
                        }
                        // We may just have learn the taxi, then retry
                        if (!Gossip.IsTaxiWindowOpen())
                        {
                            Interact.InteractWith(memoryTaxi.GetBaseAddress, true);
                            Thread.Sleep(250 + Usefuls.Latency);
                        }
                        if (!Gossip.IsTaxiWindowOpen())
                        {
                            Gossip.SelectGossip(Gossip.GossipOption.Taxi);
                            Thread.Sleep(250 + Usefuls.Latency);
                        }
                        if (!Gossip.IsTaxiWindowOpen())
                        {
                            Logging.Write("There is a problem with taxi master");
                            return;
                        }
                        // It's time to rethink the situation where the player does not know all taxis we need
                        // Taxi window is open, then we can get all taxis the player knowns
                        // Current one has just been learn if it was unknown
                        _unknownTaxis.Remove(taxi);
                        // Now update list of unknown taxi, they will be ignored in path next time, but used as start point
                        // and then will be learn
                        if (!_unknownTaxisChecked) // If we did it once, no need to redo (it takes several seconds)
                        {
                            _unknownTaxisChecked = true;
                            List<Taxi> knownList = Gossip.GetAllTaxisAvailable();
                            foreach (Taxi oneTaxi in _availableTaxis)
                            {
                                Taxi search = knownList.Find(x => x.Xcoord == oneTaxi.Xcoord && x.Ycoord == oneTaxi.Ycoord);
                                if (search == null || search.Xcoord == "")
                                {
                                    if (oneTaxi.Faction == Npc.FactionType.Neutral || oneTaxi.Faction.ToString() == ObjectManager.ObjectManager.Me.PlayerFaction)
                                        _unknownTaxis.Add(oneTaxi);
                                }
                            }
                        }
                        CombatClass.DisposeCombatClass();
                        Gossip.TakeTaxi(nextHop.Xcoord, nextHop.Ycoord);

                        if (AutomaticallyTookTaxi != null)
                            AutomaticallyTookTaxi(this, new TaxiEventArgs {From = memoryTaxi.Entry, To = (int) nextHop.Id}); // Fires event
                        //nextHop.Id;
                        Logging.Write("Flying to " + nextHop.Name);
                        loop = false;
                    }
                    else
                    {
                        if (taxi.APoint.DistanceTo(ObjectManager.ObjectManager.Me.Position) > 4.0f)
                        {
                            GoToDepartureQuayOrPortal(selectedTransport);
                            EnterTransportOrTakePortal(selectedTransport);
                            return;
                        }
                        memoryTaxi = ObjectManager.ObjectManager.GetNearestWoWUnit(ObjectManager.ObjectManager.GetWoWUnitByEntry((int) taxi.Id));
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
                        if (ObjectManager.ObjectManager.Me.InInevitableCombat || ObjectManager.ObjectManager.Me.IsDead)
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
                if (ObjectManager.ObjectManager.Me.InInevitableCombat || ObjectManager.ObjectManager.Me.IsDead)
                    return;
                if (!ObjectManager.ObjectManager.Me.InTransport)
                    loop = false;
                if (ObjectManager.ObjectManager.Me.Position.DistanceTo(selectedTransport.ArrivalIsA ? selectedTransport.AOutsidePoint : selectedTransport.BOutsidePoint) < 5)
                    loop = false;
                Thread.Sleep(500);
            }
        }

        // This function is a graph builder (A.I.), will build the graph with minimal number of hops to the target
        // After minimal(s) path are found, we select the shortest if several exists
        // Then we return only the next hop after the current
        private Taxi FindNextTaxiHopFor(Taxi taxi, bool display = false)
        {
            // Make a copy of the link list to be able to consume it
            List<TaxiLink> linksCopy = _availableTaxiLinks.ToList();
            uint startId = taxi.Id;
            uint endId = taxi.EndOfPath;
            bool progress = true;

            // The graph of hops
            List<List<uint>> graph = new List<List<uint>>();
            graph.Add(new List<uint> {startId});
            while (progress && linksCopy.Count > 0)
            {
                progress = false;
                bool pathFound = false;
                List<List<uint>> newGraph = new List<List<uint>>();
                foreach (List<uint> listOfCurrentHops in graph)
                {
                    uint currentHop = listOfCurrentHops.Last();
                    // Now find links from this hop
                    List<TaxiLink> nextLinks = linksCopy.FindAll(x => (x.PointA == currentHop || x.PointB == currentHop));
                    // Enumerate them
                    foreach (TaxiLink lnk in nextLinks)
                    {
                        // Consume this link from the copied links list
                        linksCopy.Remove(lnk);
                        uint target = lnk.PointA == currentHop ? lnk.PointB : lnk.PointA;
                        // Ignore if it's the wrong faction or taxi does not exist
                        Taxi targetTaxi = _availableTaxis.Find(x => x.Id == target);
                        if (targetTaxi == null || targetTaxi.Id == 0)
                            continue;
                        if (targetTaxi.Faction != Npc.FactionType.Neutral && targetTaxi.Faction.ToString() != ObjectManager.ObjectManager.Me.PlayerFaction)
                            continue;
                        // We ignore taxi that is marked unknown
                        if (_unknownTaxis.Contains(targetTaxi))
                            continue;
                        // If we found the target, then bingo, we can stop searching deeper
                        if (target == endId)
                            pathFound = true;
                        // We build a new list containing previous list + the new hop
                        List<uint> newHopsList = new List<uint>();
                        newHopsList.AddRange(listOfCurrentHops);
                        newHopsList.Add(target);
                        // now add the updated list in the graph
                        newGraph.Add(newHopsList);
                        progress = true; // we made a change, so there is a progress we can loop another time
                    }
                }
                // We found a path (or several) in the last iteration, time to take shortest
                if (pathFound)
                {
                    float bestDistance = float.MaxValue;
                    List<uint> bestPathFound = new List<uint>(); // useless to initialize but C# requires it
                    foreach (List<uint> onePath in newGraph)
                    {
                        if (onePath.Last() == endId)
                        {
                            Taxi lastTaxi = null;
                            float distance = 0;
                            foreach (uint id in onePath)
                            {
                                if (lastTaxi == null)
                                {
                                    lastTaxi = _availableTaxis.Find(x => x.Id == id);
                                    continue;
                                }
                                Taxi currentTaxi = _availableTaxis.Find(x => x.Id == id);
                                distance += lastTaxi.Position.DistanceTo(currentTaxi.Position);
                                lastTaxi = currentTaxi;
                            }
                            if (distance < bestDistance)
                            {
                                bestDistance = distance;
                                bestPathFound = onePath;
                            }
                        }
                    }
                    if (display)
                        Logging.Write("Taxi travel plan: " + string.Join<uint>(", ", bestPathFound));
                    if (bestPathFound.Count > 1)
                        return _availableTaxis.Find(x => x.Id == bestPathFound[1]);
                    else
                        return _availableTaxis.Find(x => x.Id == endId);
                }
                // We did not find target yet, so update graph and loop again
                graph = newGraph;
            }
            return null;
        }


        // This is for taxi near the target, we can't resolve it, so ensure it's linked to another taxi
        private bool IsTaxiLinkedFast(Taxi taxi)
        {
            List<TaxiLink> lnks = _availableTaxiLinks.FindAll(x => (x.PointA == taxi.Id || x.PointB == taxi.Id));
            foreach (TaxiLink lnk in lnks)
            {
                Taxi otherTaxi;
                if (lnk.PointA == taxi.Id)
                    otherTaxi = _availableTaxis.Find(x => x.Id == lnk.PointB);
                else
                    otherTaxi = _availableTaxis.Find(x => x.Id == lnk.PointA);
                if (otherTaxi == null || otherTaxi.Id == 0)
                    continue;
                if (otherTaxi.Faction != Npc.FactionType.Neutral && otherTaxi.Faction.ToString() != ObjectManager.ObjectManager.Me.PlayerFaction)
                    continue;
                return true;
            }
            return false;
        }

        // We resolve the path and check if there is a result
        private bool IsTaxiLinked(Taxi taxi)
        {
            return FindNextTaxiHopFor(taxi) != null;
        }

        public static void TravelPatientlybyTaxiOrPortal(bool ignoreCombatClass = false)
        {
            bool loop = true;
            Point refPoint = ObjectManager.ObjectManager.Me.Position;
            while (loop)
            {
                Thread.Sleep(1000);
                if (!Usefuls.InGame || Usefuls.IsLoading)
                    continue;
                if (!ObjectManager.ObjectManager.Me.OnTaxi &&
                    refPoint.DistanceTo(ObjectManager.ObjectManager.Me.Position) < 1.0f)
                    loop = false;
                else
                    refPoint = ObjectManager.ObjectManager.Me.Position;
            }
            if (!ignoreCombatClass)
                CombatClass.LoadCombatClass();
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
                if (Products.Products.InAutoPause)
                {
                    i = 0;
                    i2 = 0;
                    Thread.Sleep(500);
                    continue;
                }
                if (!ObjectManager.ObjectManager.Me.InTransport && Usefuls.InGame && !Usefuls.IsLoading)
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
            List<CustomPath> customPaths = GetCustomPathsThatGoesToDestination(travelTo, travelToContinentId);
            List<Portal> portals = GetPortalsThatGoesToDestination(travelTo, travelToContinentId);
            Taxi taxi = GetTaxiThatGoesToDestination(travelTo, travelToContinentId);
            allTransports.AddRange(transports);
            allTransports.AddRange(customPaths);
            allTransports.AddRange(portals);
            if (taxi != null)
                allTransports.Add(taxi);
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
                // toDo: Break reference to transport from _availableTransports. http://stackoverflow.com/questions/31603679/how-to-stop-reference-of-other-class-object
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
                        transport.ArrivalIsA = false;
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

        private Taxi GetTaxiThatGoesToDestination(Point travelTo, int travelToContinentId)
        {
            // Sort Taxis by distance to destination
            _availableTaxis.Sort(delegate(Taxi x, Taxi y) { return (travelTo.DistanceTo(x.Position) < travelTo.DistanceTo(y.Position) ? -1 : 1); });
            uint cnt = 0;
            foreach (Taxi taxi in _availableTaxis)
            {
                if (cnt >= 3) // We only check the 3 nearest taxis
                    break;
                if (taxi.Faction != Npc.FactionType.Neutral && taxi.Faction.ToString() != ObjectManager.ObjectManager.Me.PlayerFaction)
                    continue;
                if (taxi.ContinentId != travelToContinentId)
                    continue;
                if (!IsTaxiLinkedFast(taxi))
                    continue;
                if (_unknownTaxis.Contains(taxi))
                    continue;
                cnt++;
                bool success;
                List<Point> path = PathFinder.FindPath(taxi.Position, travelTo, Usefuls.ContinentNameMpqByContinentId(travelToContinentId), out success);
                if (success || (!success && path.Count >= 1 && IsPointValidAsTarget(path.Last())))
                {
                    // Return the closest taxi with a valid path from it to destination
                    return taxi;
                }
            }
            return null;
        }

        private Taxi GetTaxisThatDirectlyGoToDestination(Point travelTo, Point travelFrom, int travelToContinentId, int travelFromContinentId)
        {
            Taxi bestTaxi = GetTaxiThatGoesToDestination(travelTo, travelToContinentId);
            if (bestTaxi == null)
                return null;
            Point currentPosition = ObjectManager.ObjectManager.Me.Position;
            // Sort taxis by distance from our position
            _availableTaxis.Sort(delegate(Taxi x, Taxi y) { return (currentPosition.DistanceTo(x.Position) < currentPosition.DistanceTo(y.Position) ? -1 : 1); });
            uint cnt = 0;
            foreach (Taxi taxi in _availableTaxis)
            {
                if (cnt >= 3) // We only check the 3 nearest taxis
                    break;
                // Prevent going to self
                if (taxi.Position == bestTaxi.Position)
                    return null;
                if (taxi.Faction != Npc.FactionType.Neutral && taxi.Faction.ToString() != ObjectManager.ObjectManager.Me.PlayerFaction)
                    continue;
                if (taxi.ContinentId != travelFromContinentId)
                    continue;
                taxi.EndOfPath = bestTaxi.Id;
                if (!IsTaxiLinked(taxi))
                    continue;
                cnt++;
                bool success;
                PathFinder.FindPath(taxi.Position, travelFrom, Usefuls.ContinentNameMpqByContinentId(travelFromContinentId), out success);
                if (success)
                {
                    // Return the closest taxi with a valid path from our position to it
                    taxi.APoint = taxi.Position; // transfer values to transport object
                    taxi.BPoint = bestTaxi.Position;
                    return taxi;
                }
            }
            return null;
        }

        private List<Portal> GetPortalsThatGoesToDestination(Point travelTo, int travelToContinentId)
        {
            var listPortal = new List<Portal>();
            foreach (Portal portal in _availablePortals.Items)
            {
                if (portal.BContinentId != travelToContinentId)
                    continue;
                bool success;
                PathFinder.FindPath(portal.BPoint, travelTo, Usefuls.ContinentNameMpqByContinentId(travelToContinentId), out success);
                if (success || portal.BPoint.DistanceTo(travelTo) < 5f)
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

        private List<CustomPath> GetCustomPathsThatGoesToDestination(Point travelTo, int travelToContinentId)
        {
            var listCustomPath = new List<CustomPath>();
            foreach (CustomPath customPath in _availableCustomPaths.Items)
            {
                if (customPath.Faction != Npc.FactionType.Neutral && customPath.Faction.ToString() != ObjectManager.ObjectManager.Me.PlayerFaction)
                    continue;
                if (customPath.AContinentId != travelToContinentId && customPath.BContinentId != travelToContinentId)
                    continue;
                if (customPath.AContinentId == travelToContinentId && customPath.BContinentId != travelToContinentId)
                    continue;
                if (customPath.BContinentId == travelToContinentId && customPath.AContinentId != travelToContinentId)
                    continue;
                bool success;
                PathFinder.FindPath(customPath.APoint, travelTo, Usefuls.ContinentNameMpqByContinentId(travelToContinentId), out success);
                if (success && customPath.RoundTrip)
                {
                    customPath.ArrivalIsA = true;
                    listCustomPath.Add(customPath);
                }
                PathFinder.FindPath(customPath.BPoint, travelTo, Usefuls.ContinentNameMpqByContinentId(travelToContinentId), out success);
                if (success)
                {
                    customPath.ArrivalIsA = false;
                    listCustomPath.Add(customPath);
                }
            }
            return listCustomPath;
        }

        private List<CustomPath> GetCustomPathsThatDirectlyGoToDestination(Point travelTo, Point travelFrom, int travelToContinentId, int travelFromContinentId)
        {
            var listCustomPath = new List<CustomPath>();
            List<CustomPath> customPaths = GetCustomPathsThatGoesToDestination(travelTo, travelToContinentId);
            foreach (CustomPath customPath in customPaths)
            {
                if (customPath.ArrivalIsA)
                {
                    if (customPath.BContinentId != travelFromContinentId)
                        continue;
                    bool success;
                    PathFinder.FindPath(customPath.BPoint, travelFrom, Usefuls.ContinentNameMpqByContinentId(travelFromContinentId), out success);
                    if (success)
                    {
                        listCustomPath.Add(customPath);
                    }
                }
                else
                {
                    if (customPath.AContinentId != travelFromContinentId)
                        continue;
                    bool success;
                    PathFinder.FindPath(customPath.APoint, travelFrom, Usefuls.ContinentNameMpqByContinentId(travelFromContinentId), out success);
                    if (success)
                    {
                        listCustomPath.Add(customPath);
                    }
                }
            }
            return listCustomPath;
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
            List<CustomPath> customPaths = GetCustomPathsThatDirectlyGoToDestination(travelTo, travelFrom, travelToContinentId, travelFromContinentId);
            Taxi taxi = GetTaxisThatDirectlyGoToDestination(travelTo, travelFrom, travelToContinentId, travelFromContinentId);
            allTransports.AddRange(transports);
            allTransports.AddRange(customPaths);
            allTransports.AddRange(portals);
            if (taxi != null)
                allTransports.Add(taxi);
            return allTransports;
        }

        private KeyValuePair<Transport, float> GetBestDirectWayTransport(Point travelFrom, Point travelTo, int travelFromContinentId, int travelToContinentId)
        {
            var bestTransport = new Transport();
            float bestTransportDistance = float.MaxValue;
            List<Transport> allTransports = GetAllTransportsThatDirectlyGoToDestination(travelTo, travelFrom, travelToContinentId, travelFromContinentId);
            foreach (Transport transport in allTransports)
            {
                float currentTransportDistance = 0f;
                uint currentId = 0;
                if (transport is Portal)
                {
                    var portal = transport as Portal;
                    List<Point> wayIn = PathFinder.FindPath(travelFrom, portal.APoint, Usefuls.ContinentNameMpqByContinentId(travelFromContinentId));
                    List<Point> wayOff = PathFinder.FindPath(portal.BPoint, travelTo, Usefuls.ContinentNameMpqByContinentId(travelToContinentId));
                    currentTransportDistance = Math.DistanceListPoint(wayIn) + Math.DistanceListPoint(wayOff);
                    currentId = portal.Id;
                }
                else if (transport is CustomPath)
                {
                    var customPath = transport as CustomPath;
                    if (customPath.ArrivalIsA)
                    {
                        if (customPath.RoundTrip)
                        {
                            List<Point> wayIn = PathFinder.FindPath(travelFrom, customPath.BPoint, Usefuls.ContinentNameMpqByContinentId(travelFromContinentId));
                            List<Point> wayOff = PathFinder.FindPath(customPath.APoint, travelTo, Usefuls.ContinentNameMpqByContinentId(travelToContinentId));
                            currentTransportDistance = Math.DistanceListPoint(wayIn) + Math.DistanceListPoint(wayOff);
                            currentId = customPath.Id;
                        }
                    }
                    else
                    {
                        List<Point> wayIn = PathFinder.FindPath(travelFrom, customPath.APoint, Usefuls.ContinentNameMpqByContinentId(travelFromContinentId));
                        List<Point> wayOff = PathFinder.FindPath(customPath.BPoint, travelTo, Usefuls.ContinentNameMpqByContinentId(travelToContinentId));
                        currentTransportDistance = Math.DistanceListPoint(wayIn) + Math.DistanceListPoint(wayOff);
                        currentId = customPath.Id;
                    }
                }
                else if (transport is Taxi)
                {
                    var taxi = transport as Taxi;
                    List<Point> wayIn = PathFinder.FindPath(travelFrom, taxi.APoint, Usefuls.ContinentNameMpqByContinentId(travelFromContinentId));
                    List<Point> wayOff = PathFinder.FindPath(taxi.BPoint, travelTo, Usefuls.ContinentNameMpqByContinentId(travelToContinentId));
                    currentTransportDistance = Math.DistanceListPoint(wayIn) + Math.DistanceListPoint(wayOff);
                    if (travelFromContinentId == travelToContinentId)
                        currentTransportDistance += (taxi.APoint.DistanceTo(taxi.BPoint)/2.5f);
                    currentId = taxi.Id;
                }
                else
                {
                    if (transport.ArrivalIsA)
                    {
                        List<Point> wayIn = PathFinder.FindPath(travelFrom, transport.BOutsidePoint, Usefuls.ContinentNameMpqByContinentId(travelFromContinentId));
                        List<Point> wayOff = PathFinder.FindPath(transport.AOutsidePoint, travelTo, Usefuls.ContinentNameMpqByContinentId(travelToContinentId));
                        currentTransportDistance = Math.DistanceListPoint(wayIn) + Math.DistanceListPoint(wayOff);
                        currentId = transport.Id;
                    }
                    else
                    {
                        List<Point> wayIn = PathFinder.FindPath(travelFrom, transport.AOutsidePoint, Usefuls.ContinentNameMpqByContinentId(travelFromContinentId));
                        List<Point> wayOff = PathFinder.FindPath(transport.BOutsidePoint, travelTo, Usefuls.ContinentNameMpqByContinentId(travelToContinentId));
                        currentTransportDistance = Math.DistanceListPoint(wayIn) + Math.DistanceListPoint(wayOff);
                        currentId = transport.Id;
                    }
                }

                if (!(currentTransportDistance < bestTransportDistance)) continue;
                bestTransport = transport;
                bestTransport.Id = currentId;
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
                    currentTransportDistance = currentToIntermediate.Value + Math.DistanceListPoint(way);

                    if (currentTransportDistance < bestTransportDistance)
                    {
                        bestTransports = new List<Transport> {currentToIntermediate.Key, transport};
                        bestTransportDistance = currentTransportDistance;
                    }
                }
                else if (transport is CustomPath)
                {
                    var customPath = transport as CustomPath;
                    if (customPath.ArrivalIsA)
                    {
                        if (customPath.RoundTrip)
                        {
                            if (customPath.BContinentId != travelFromContinentId)
                                continue;
                            KeyValuePair<Transport, float> currentToIntermediate = GetBestDirectWayTransport(travelFrom, customPath.BOutsidePoint, travelFromContinentId, customPath.AContinentId);
                            List<Point> way = PathFinder.FindPath(customPath.AOutsidePoint, travelTo, Usefuls.ContinentNameMpqByContinentId(travelToContinentId));
                            currentTransportDistance = currentToIntermediate.Value + Math.DistanceListPoint(way);

                            if (currentTransportDistance < bestTransportDistance)
                            {
                                bestTransports = new List<Transport> {currentToIntermediate.Key, customPath};
                                bestTransportDistance = currentTransportDistance;
                            }
                        }
                    }
                    else
                    {
                        if (transport.AContinentId != travelFromContinentId)
                            continue;
                        KeyValuePair<Transport, float> currentToIntermediate = GetBestDirectWayTransport(travelFrom, customPath.AOutsidePoint, travelFromContinentId, customPath.BContinentId);
                        List<Point> way = PathFinder.FindPath(customPath.BOutsidePoint, travelTo, Usefuls.ContinentNameMpqByContinentId(travelToContinentId));
                        currentTransportDistance = currentToIntermediate.Value + Math.DistanceListPoint(way);

                        if (currentTransportDistance < bestTransportDistance)
                        {
                            bestTransports = new List<Transport> {currentToIntermediate.Key, transport};
                            bestTransportDistance = currentTransportDistance;
                        }
                    }
                }
                else if (transport is Taxi)
                {
                    // TODO: Support Taxi in 2way traveller.
                    /*var taxi = transport as Taxi;
                    List<Point> wayIn = PathFinder.FindPath(travelFrom, taxi.APoint, Usefuls.ContinentNameMpqByContinentId(travelFromContinentId));
                    List<Point> wayOff = PathFinder.FindPath(taxi.BPoint, travelTo, Usefuls.ContinentNameMpqByContinentId(travelToContinentId));
                    currentTransportDistance = Math.DistanceListPoint(wayIn) + Math.DistanceListPoint(wayOff);
                    if (travelFromContinentId == travelToContinentId)
                        currentTransportDistance += (taxi.APoint.DistanceTo(taxi.BPoint) / 1.5f);
                    currentId = taxi.Id;*/
                }
                else
                {
                    if (transport.ArrivalIsA)
                    {
                        if (transport.BContinentId != travelFromContinentId)
                            continue;
                        KeyValuePair<Transport, float> currentToIntermediate = GetBestDirectWayTransport(travelFrom, transport.BOutsidePoint, travelFromContinentId, transport.AContinentId);
                        List<Point> way = PathFinder.FindPath(transport.AOutsidePoint, travelTo, Usefuls.ContinentNameMpqByContinentId(travelToContinentId));
                        currentTransportDistance = currentToIntermediate.Value + Math.DistanceListPoint(way);

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
                        currentTransportDistance = currentToIntermediate.Value + Math.DistanceListPoint(way);

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