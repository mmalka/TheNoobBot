using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
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

        private Point TravelFrom
        {
            get { return Products.Products.TravelFrom; }
            set { Products.Products.TravelFrom = value; }
        }

        private bool ForceTravel
        {
            get { return Products.Products.ForceTravel; }
            set { Products.Products.ForceTravel = value; }
        }

        private int TravelFromContinentId
        {
            get { return Products.Products.TravelFromContinentId; }
            set { Products.Products.TravelFromContinentId = value; }
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
                if (!Products.Products.IsStarted || ObjectManager.ObjectManager.Me.IsDeadMe || ObjectManager.ObjectManager.Me.InInevitableCombat || !NeedToTravel)
                    return false;
                _availableTransports = XmlSerializer.Deserialize<Transports>(Application.StartupPath + @"\Data\TransportsDB.xml");
                for (int i = _availableTransports.Items.Count - 1; i > 0; i--)
                {
                    var transport = _availableTransports.Items[i];
                    if (transport.Faction != Npc.FactionType.Neutral && transport.Faction.ToString() != ObjectManager.ObjectManager.Me.PlayerFaction)
                    {
                        _availableTransports.Items.RemoveAt(i);
                    }
                }
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
                if (_generatedRoutePath.Count > 0)
                    return true;
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

        private Transport GetDepartureLift(Transport transport)
        {
            if (transport is Taxi)
            {
                // should never enter this check
                return new Transport();
            }
            bool success;
            if (!(transport is Portal) && !(transport is CustomPath) || (transport is CustomPath) && (transport as CustomPath).RoundTrip)
            {
                if (transport.ArrivalIsA)
                {
                    if (!transport.UseBLift)
                        return new Transport();
                    var bLift = GetTransportByTransportId(transport.BLift);
                    PathFinder.FindPath(TravelFrom, (bLift is CustomPath) ? bLift.APoint : bLift.AOutsidePoint, Usefuls.ContinentNameMpqByContinentId(TravelFromContinentId), out success);
                    if (!success)
                        bLift.ArrivalIsA = true;
                    // calculate the "IsArrivalIsA" then return the bLift;
                    return bLift;
                }
            }
            if (!transport.UseALift)
                return new Transport();
            var aLift = GetTransportByTransportId(transport.ALift);
            PathFinder.FindPath(TravelFrom, (aLift is CustomPath) || (aLift is Portal) ? aLift.APoint : aLift.AOutsidePoint, Usefuls.ContinentNameMpqByContinentId(TravelFromContinentId), out success);
            if (!success && (!(aLift is Portal) && !(aLift is CustomPath) || (aLift is CustomPath) && (aLift as CustomPath).RoundTrip))
                aLift.ArrivalIsA = true;
            // calculate the "IsArrivalIsA" then return the aLift;
            return aLift;
        }

        private Transport GetArrivalLift(Transport transport)
        {
            bool success;
            if (!(transport is Portal) && !(transport is CustomPath) || (transport is CustomPath) && (transport as CustomPath).RoundTrip)
            {
                if (transport.ArrivalIsA)
                {
                    if (!transport.UseALift)
                        return new Transport();
                    var aLift = GetTransportByTransportId(transport.ALift);
                    PathFinder.FindPath(TravelTo, (aLift is CustomPath) ? aLift.BPoint : aLift.BOutsidePoint, Usefuls.ContinentNameMpqByContinentId(TravelToContinentId), out success);
                    if (!success)
                        aLift.ArrivalIsA = true;
                    // calculate the "IsArrivalIsA" then return the aLift;
                    return aLift;
                }
            }
            if (!transport.UseBLift)
                return new Transport();
            var bLift = GetTransportByTransportId(transport.BLift);
            PathFinder.FindPath(TravelTo, (bLift is CustomPath) || (bLift is Portal) ? bLift.BPoint : bLift.BOutsidePoint, Usefuls.ContinentNameMpqByContinentId(TravelToContinentId), out success);
            if (!success && (!(bLift is Portal) && !(bLift is CustomPath) || (bLift is CustomPath) && (bLift as CustomPath).RoundTrip))
                bLift.ArrivalIsA = true;
            // calculate the "IsArrivalIsA" then return the bLift;
            return bLift;
        }

        private List<Transport> GenerateRoutePath
        {
            get
            {
                int travelToContinentId = TravelToContinentId;
                bool succes;

                if (travelToContinentId == TravelFromContinentId)
                {
                    Logging.Write("Generating Travel from " + TravelFrom + " " + Usefuls.ContinentNameMpqByContinentId(TravelFromContinentId) + " to " + TravelTo + " " +
                                  Usefuls.ContinentNameMpqByContinentId(TravelToContinentId) + ", Distance: " + TravelFrom.DistanceTo(TravelTo));
                    bool success;
                    var way = PathFinder.FindPath(TravelFrom, TravelTo, Usefuls.ContinentNameMpqByContinentId(TravelFromContinentId), out success);
                    if (success && Math.DistanceListPoint(way) <= 400f)
                    {
                        TravelToContinentId = 9999999;
                        TravelTo = new Point();
                        TravelFrom = new Point();
                        ForceTravel = false;
                        TargetValidationFct = null;
                        Logging.Write("Travel: We are close enough and we have a valid path. Cancelling Travel.");
                        return new List<Transport>();
                    }
                }
                else
                    Logging.Write("Generating Travel from " + TravelFrom + " " + Usefuls.ContinentNameMpqByContinentId(TravelFromContinentId) + " to " + TravelTo + " " +
                                  Usefuls.ContinentNameMpqByContinentId(TravelToContinentId) + ".");
                KeyValuePair<Transport, float> oneWayTravel = GetBestDirectWayTransport(TravelFrom, TravelTo, TravelFromContinentId, travelToContinentId);
                List<Transport> travelPlan = new List<Transport>();
                float travelCost = 0f;

                if (oneWayTravel.Key.Id != 0)
                {
                    Logging.Write("Travel: Found direct way travel.");
                    bool useTaxi = false;
                    float distWithoutTaxi = 0f;
                    var departureLift = GetDepartureLift(oneWayTravel.Key);

                    KeyValuePair<Transport, float> taxiToTravel = new KeyValuePair<Transport, float>(new Transport(), float.MaxValue);
                    bool taxiGoToLift = false;
                    if (!(oneWayTravel.Key is Taxi))
                    {
                        Point liftEntrance = null;
                        Point transportEntrance = null;
                        if (departureLift.Id > 0)
                        {
                            if (departureLift.ArrivalIsA && !(departureLift is Portal) && (!(departureLift is CustomPath) || (departureLift as CustomPath).RoundTrip))
                            {
                                liftEntrance = (departureLift is CustomPath) ? departureLift.BPoint : departureLift.BOutsidePoint;
                            }
                            else
                                liftEntrance = (departureLift is Portal) || (departureLift is CustomPath) ? departureLift.APoint : departureLift.AOutsidePoint;
                        }
                        if (oneWayTravel.Key.ArrivalIsA && !(oneWayTravel.Key is Portal) && (!(oneWayTravel.Key is CustomPath) || (oneWayTravel.Key as CustomPath).RoundTrip))
                        {
                            transportEntrance = (oneWayTravel.Key is CustomPath) ? oneWayTravel.Key.BPoint : oneWayTravel.Key.BOutsidePoint;
                        }
                        else
                            transportEntrance = (oneWayTravel.Key is Portal) || (oneWayTravel.Key is CustomPath) ? oneWayTravel.Key.APoint : oneWayTravel.Key.AOutsidePoint;


                        int liftEntranceContinentId = departureLift.ArrivalIsA ? departureLift.BContinentId : departureLift.AContinentId;
                        int transportEntranceContinentId = oneWayTravel.Key.ArrivalIsA ? oneWayTravel.Key.BContinentId : oneWayTravel.Key.AContinentId;
                        KeyValuePair<Transport, float> taxiToTransport = GetBestDirectWayTaxi(TravelFrom, transportEntrance, TravelFromContinentId, transportEntranceContinentId);
                        KeyValuePair<Transport, float> taxiToLift = new KeyValuePair<Transport, float>(new Transport(), float.MaxValue);
                        if (departureLift.Id > 0 && liftEntrance != null)
                        {
                            taxiToLift = GetBestDirectWayTaxi(TravelFrom, liftEntrance, TravelFromContinentId, liftEntranceContinentId);
                        }
                        List<Point> wayToEntrance = PathFinder.FindPath(TravelFrom, liftEntrance != null && liftEntrance.IsValid ? liftEntrance : transportEntrance,
                            Usefuls.ContinentNameMpqByContinentId(TravelFromContinentId), out succes);
                        if (succes)
                        {
                            distWithoutTaxi = Math.DistanceListPoint(wayToEntrance);
                            if (!(distWithoutTaxi < taxiToLift.Value) || !(distWithoutTaxi < taxiToTransport.Value))
                                useTaxi = true;
                        }
                        else
                            useTaxi = true;
                        if (taxiToLift.Key.Id != 0 && taxiToTransport.Key.Id != 0)
                        {
                            taxiToTravel = (taxiToLift.Value > taxiToTransport.Value) ? taxiToTransport : taxiToLift;
                            taxiGoToLift = !(taxiToLift.Value > taxiToTransport.Value);
                        }
                        else if (taxiToLift.Key.Id != 0)
                        {
                            taxiToTravel = taxiToLift;
                            taxiGoToLift = true;
                        }
                        else if (taxiToTransport.Key.Id != 0)
                        {
                            taxiToTravel = taxiToTransport;
                        }
                    }

                    if (useTaxi)
                    {
                        if (departureLift.Id > 0 && (taxiToTravel.Key.Id == 0 || taxiGoToLift))
                        {
                            if (taxiToTravel.Key.Id != 0 && taxiGoToLift)
                            {
                                travelPlan.Add(taxiToTravel.Key);
                                travelCost -= distWithoutTaxi;
                                travelCost += taxiToTravel.Value;
                            }
                            travelPlan.Add(departureLift);
                        }
                        else if (taxiToTravel.Key.Id != 0 && !taxiGoToLift)
                        {
                            travelPlan.Add(taxiToTravel.Key);
                            travelCost -= distWithoutTaxi;
                            travelCost += taxiToTravel.Value;
                        }
                    }
                    else if (departureLift.Id > 0)
                    {
                        travelPlan.Add(departureLift);
                    }

                    travelPlan.Add(oneWayTravel.Key);
                    travelCost += oneWayTravel.Value; // already contains the sum of both lift.

                    var arrivalLift = GetArrivalLift(oneWayTravel.Key);
                    if (arrivalLift.Id > 0)
                        travelPlan.Add(arrivalLift);
                }
                if (TravelFromContinentId == travelToContinentId && travelPlan.Count > 0)
                {
                    bool success;
                    List<Point> way = PathFinder.FindPath(TravelFrom, TravelTo, Usefuls.ContinentNameMpqByContinentId(TravelFromContinentId), out success);
                    if (success || (!success && way.Count >= 1 && IsPointValidAsTarget(way.Last()) && !(oneWayTravel.Key is CustomPath)))
                    {
                        if (travelCost > Math.DistanceListPoint(way) && !ForceTravel)
                        {
                            TravelToContinentId = 9999999;
                            TravelTo = new Point();
                            TravelFrom = new Point();
                            TargetValidationFct = null;
                            Logging.Write("Travel: Found a faster path without using Transports. Cancelling Travel.");
                            return new List<Transport>();
                        }
                        if (ForceTravel)
                        {
                            Logging.Write("Travel: Found a faster path without using Transports but ForceTravel is activated.");
                        }
                    }
                }
                if (travelPlan.Count > 0)
                    return travelPlan;

                TravelToContinentId = 9999999;
                TravelTo = new Point();
                TravelFrom = new Point();
                ForceTravel = false;
                TargetValidationFct = null;
                Logging.Write("Travel: Couldn't find a travel path.");
                return new List<Transport>();
            }
        }

        public override void Run()
        {
            MovementManager.StopMove();

            string s = "";
            string number = "one";
            if (_generatedRoutePath.Count == 2)
                number = "two";
            else if (_generatedRoutePath.Count == 3)
                number = "three";
            if (number != "one")
                s = "s";
            Logging.Write("Travel: Our travel plan consists of " + number + " transport" + s + " for today's journey.");
            foreach (Transport transport in _generatedRoutePath)
            {
                if (transport is Taxi)
                {
                    var taxi = transport as Taxi;
                    Logging.Write("Travel: Taxi " + taxi.Name + "(" + taxi.Id + "), to " + GetTaxiByTaxiId(taxi.EndOfPath).Name + "(" + taxi.EndOfPath + ")");
                }
                else if (transport is CustomPath || transport is Portal)
                {
                    if (transport is CustomPath && transport.ArrivalIsA)
                    {
                        Logging.Write("Travel: " + transport.Name + "(" + transport.Id + "), from " + Usefuls.ContinentNameMpqByContinentId(transport.BContinentId) + " (" +
                                      transport.BPoint + ") to " +
                                      Usefuls.ContinentNameMpqByContinentId(transport.AContinentId) + " (" + transport.APoint + ")");
                    }
                    else
                    {
                        Logging.Write("Travel: " + transport.Name + "(" + transport.Id + "), from " + Usefuls.ContinentNameMpqByContinentId(transport.AContinentId) + " (" +
                                      transport.APoint + ") to " +
                                      Usefuls.ContinentNameMpqByContinentId(transport.BContinentId) + " (" + transport.BPoint + ")");
                    }
                }
                else
                {
                    if (transport.ArrivalIsA)
                        Logging.Write("Travel: " + transport.Name + "(" + transport.Id + "), from " + Usefuls.ContinentNameMpqByContinentId(transport.BContinentId) + " (" +
                                      transport.BOutsidePoint + ") to " +
                                      Usefuls.ContinentNameMpqByContinentId(transport.AContinentId) + " (" + transport.AOutsidePoint + ")");
                    else
                        Logging.Write("Travel: " + transport.Name + "(" + transport.Id + "), from " + Usefuls.ContinentNameMpqByContinentId(transport.AContinentId) + " (" +
                                      transport.AOutsidePoint + ") to " +
                                      Usefuls.ContinentNameMpqByContinentId(transport.BContinentId) + " (" + transport.BOutsidePoint + ")");
                }
            }
            if (_generatedRoutePath.Count > 1)
                Logging.Write("Travel: We will recalculate travel path once arrived to make sure we are always on the fastest path.");
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
                bool restartTravel = EnterTransportOrTakePortal(transport);
                if (restartTravel)
                {
                    // Our travel ended because we didn't have the taxi we wanted, regenerate now that the list of known taxi is updated.
                    _generatedRoutePath = GenerateRoutePath;
                    if (!CombatClass.IsAliveCombatClass)
                        CombatClass.LoadCombatClass();
                    return;
                }
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
                break;
                // We didn't finish our travel plan, but we can generate the same route and this time perhaps includes a 
                // faster transport that became available. (a taxi that is only accessible before the elevator ?

                // This is based on the example: Brill to Somewhere on Kalimdor
                // We generate a road to Zeppelin + Elevator (to have a correct path to final destination)
                // But, when we arrive at Orgrimmar Zeppelin dock, we figure out that it would be wiser to use the taxi instead of going down.
                // If we take the elevator down, the Orgrimmar taxi wont be visible though travel as there is no direct path.
            }
            TravelToContinentId = 9999999;
            TravelTo = new Point();
            TravelFrom = new Point();
            ForceTravel = false;
            TargetValidationFct = null;
            _generatedRoutePath = new List<Transport>();
            if (!CombatClass.IsAliveCombatClass)
                CombatClass.LoadCombatClass();
            Logging.Write("Travel is terminated, waiting for product to take the control back.");
        }

        private void GoToDepartureQuayOrPortal(Transport selectedTransport, bool failed = false)
        {
            MovementManager.StopMove();
            if (selectedTransport is Portal)
            {
                var portal = selectedTransport as Portal;
                if (!failed)
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
                    if (!MovementManager.InMoveTo && !MovementManager.InMovement)
                        loop = false;
                    Thread.Sleep(100);
                }
                MovementManager.StopMove();
                if (ObjectManager.ObjectManager.Me.Position.DistanceTo(portal.APoint) >= 2.0f)
                    GoToDepartureQuayOrPortal(selectedTransport, true);
            }
            else if (selectedTransport is CustomPath)
            {
                var customPath = selectedTransport as CustomPath;
                if (!failed)
                    Logging.Write("Going to CustomPath " + customPath.Name + " (" + customPath.Id + ") to travel.");
                List<Point> pathToCustomPath = PathFinder.FindPath(customPath.ArrivalIsA ? customPath.BPoint : customPath.APoint);
                MovementManager.Go(pathToCustomPath);
                bool loop = true;
                while (loop)
                {
                    if (!Products.Products.IsStarted || ObjectManager.ObjectManager.Me.InInevitableCombat || ObjectManager.ObjectManager.Me.IsDead)
                        return;
                    if (ObjectManager.ObjectManager.Me.Position.DistanceTo(customPath.ArrivalIsA ? customPath.BPoint : customPath.APoint) < 2.0f)
                        loop = false;
                    if (!MovementManager.InMoveTo && !MovementManager.InMovement)
                        loop = false;
                    Thread.Sleep(100);
                }
                MovementManager.StopMove();
                if (ObjectManager.ObjectManager.Me.Position.DistanceTo(customPath.ArrivalIsA ? customPath.BPoint : customPath.APoint) >= 2.0f)
                    GoToDepartureQuayOrPortal(selectedTransport, true);
            }
            else if (selectedTransport is Taxi)
            {
                var taxi = selectedTransport as Taxi;
                if (!failed)
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
                        TravelFrom = new Point();
                        ForceTravel = false;
                        TargetValidationFct = null;
                        _generatedRoutePath = new List<Transport>();
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
                    if (!MovementManager.InMoveTo && !MovementManager.InMovement)
                        loop = false;
                    Thread.Sleep(100);
                }
                MovementManager.StopMove();
                if (ObjectManager.ObjectManager.Me.Position.DistanceTo(taxi.APoint) >= 4.0f)
                    GoToDepartureQuayOrPortal(selectedTransport, true);
            }
            else
            {
                List<Point> pathToDepartureQuay = selectedTransport.ArrivalIsA ? PathFinder.FindPath(selectedTransport.BOutsidePoint) : PathFinder.FindPath(selectedTransport.AOutsidePoint);
                MovementManager.Go(pathToDepartureQuay);
                if (!failed)
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
                if (ObjectManager.ObjectManager.Me.Position.DistanceTo(selectedTransport.ArrivalIsA ? selectedTransport.BOutsidePoint : selectedTransport.AOutsidePoint) < 2.0f)
                    Logging.Write("Arrived at departure quay of " + selectedTransport.Name + "(" + selectedTransport.Id + "), waiting for transport.");
                else
                    GoToDepartureQuayOrPortal(selectedTransport, true);
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

        private bool EnterTransportOrTakePortal(Transport selectedTransport)
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
                                    return false;
                                Thread.Sleep(150);
                            }
                        }
                        MountTask.DismountMount();
                        Thread.Sleep(500);
                        Interact.InteractWith(memoryPortal.GetBaseAddress);
                        Thread.Sleep(150);
                        Interact.InteractWith(memoryPortal.GetBaseAddress);
                        Thread.Sleep(300);
                        while (ObjectManager.ObjectManager.Me.IsCasting)
                        {
                            Thread.Sleep(100);
                        }
                        TravelPatientlybyTaxiOrPortal();
                        loop = false;
                    }
                    else
                    {
                        memoryPortal = ObjectManager.ObjectManager.GetNearestWoWGameObject(ObjectManager.ObjectManager.GetWoWGameObjectByEntry((int) portal.Id), ObjectManager.ObjectManager.Me.Position);

                        if (portal.AContinentId == portal.BContinentId)
                        {
                            if (portal.APoint.DistanceTo(ObjectManager.ObjectManager.Me.Position) < portal.BPoint.DistanceTo(ObjectManager.ObjectManager.Me.Position))
                            {
                                if (portal.APoint.DistanceTo(ObjectManager.ObjectManager.Me.Position) > 4.0f)
                                {
                                    GoToDepartureQuayOrPortal(selectedTransport);
                                    EnterTransportOrTakePortal(selectedTransport);
                                    return false;
                                }
                            }
                            else
                                return false; // arrived
                        }
                        else
                        {
                            if (Usefuls.ContinentId == portal.BContinentId)
                                return false; // arrived
                            if (portal.APoint.DistanceTo(ObjectManager.ObjectManager.Me.Position) > 4.0f)
                            {
                                GoToDepartureQuayOrPortal(selectedTransport);
                                EnterTransportOrTakePortal(selectedTransport);
                                return false;
                            }
                        }
                    }
                }
            }
            else if (selectedTransport is CustomPath)
            {
                var customPath = selectedTransport as CustomPath;
                bool loop = true;
                while (loop)
                {
                    if ((customPath.ArrivalIsA ? customPath.BPoint : customPath.APoint).DistanceTo(ObjectManager.ObjectManager.Me.Position) > 4.0f)
                    {
                        bool validArrivalPath;
                        bool validEntrancePath;
                        var pathToArrival = PathFinder.FindPath(customPath.ArrivalIsA ? customPath.APoint : customPath.BPoint, out validArrivalPath);
                        var pathToEntrance = PathFinder.FindPath(customPath.ArrivalIsA ? customPath.BPoint : customPath.APoint, out validEntrancePath);
                        if (validArrivalPath)
                        {
                            if (!validEntrancePath)
                                return false;
                            if (Math.DistanceListPoint(pathToArrival) <= Math.DistanceListPoint(pathToEntrance))
                                return false; // we are closer to arrival, return instead of going back to entrance.
                        }
                        GoToDepartureQuayOrPortal(selectedTransport);
                        EnterTransportOrTakePortal(selectedTransport);
                        return false;
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
                    if (path.Count > 0)
                    {
                        Point firstPoint = path[0];
                        if (customPath.UseMount && (customPath.ForceFlying || ((PathFinder.GetZPosition(firstPoint) + 20) < firstPoint.Z)))
                        {
                            // fly path?
                            if (MountTask.GetMountCapacity() >= MountCapacity.Fly)
                            {
                                MountTask.Mount(true, true);
                                MountTask.Takeoff();
                            }
                        }
                        else if (customPath.UseMount)
                        {
                            MountTask.Mount();
                        }
                    }
                    bool allowMounting = true;
                    if (!customPath.UseMount)
                    {
                        if (MountTask.AllowMounting)
                        {
                            MountTask.AllowMounting = false;
                        }
                        else
                        {
                            allowMounting = false;
                        }
                        MountTask.DismountMount();
                    }
                    if (customPath.ForceFlying && !Usefuls.IsFlying)
                    {
                        if (!MountTask.OnFlyMount())
                        {
                            if (MountTask.GetMountCapacity() >= MountCapacity.Fly)
                            {
                                MountTask.Mount(true, true);
                                MountTask.Takeoff();
                            }
                        }
                        else
                            MountTask.Takeoff();
                        Thread.Sleep(1000);
                        if (!Usefuls.IsFlying || ObjectManager.ObjectManager.Me.InCombatBlizzard)
                        {
                            Logging.Write("This CustomPath requires you to fly, but you can't at the moment, releasing travel.");
                            return false;
                        }
                    }
                    Thread.Sleep(500);
                    MovementManager.Go(path);
                    while (MovementManager.InMovement)
                    {
                        if (!Products.Products.IsStarted || ObjectManager.ObjectManager.Me.InInevitableCombat || ObjectManager.ObjectManager.Me.IsDead)
                        {
                            return false;
                        }
                        Thread.Sleep(150);
                    }
                    if (!customPath.UseMount && allowMounting)
                        MountTask.AllowMounting = true;
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
                                    return false;
                                }
                                Thread.Sleep(150);
                            }
                        }
                        Taxi nextHop = FindNextTaxiHopFor(taxi, true);
                        if (nextHop == null)
                        {
                            Logging.Write("There is a problem with taxi links, some are missing to complete the minimal graph");
                            return false;
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
                            return false;
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
                        var finalTaxi = GetTaxiByTaxiId(taxi.EndOfPath);
                        if (finalTaxi.Id != nextHop.Id && !_unknownTaxis.Contains(finalTaxi))
                        {
                            // Try to go directly to the final taxi instead of taking every hop one by one.
                            // Wont work if we don't know final taxi yet, so relay on normal hop to hop.
                            // Also wont work in some case where wow doesn't allow you to fly directly from somewhere and instead stop at a hub (rare?)
                            for (int i = 0; i < 3; i++)
                            {
                                Gossip.TakeTaxi(finalTaxi.Xcoord, finalTaxi.Ycoord);
                                Thread.Sleep(2000);
                            }
                            // Wow doesn't allow to go directly to the last taxi without calling the function a few time.
                            if (ObjectManager.ObjectManager.Me.OnTaxi)
                            {
                                if (AutomaticallyTookTaxi != null)
                                    AutomaticallyTookTaxi(this, new TaxiEventArgs {From = memoryTaxi.Entry, To = (int) finalTaxi.Id}); // Fires event
                                Logging.Write("Flying directly to " + nextHop.Name);
                                return false;
                            }
                        }
                        if (_unknownTaxis.Contains(nextHop))
                        {
                            Logging.Write("Cannot fly to " + nextHop.Name + " yet, releasing travel.");
                            return true;
                        }
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
                            return false;
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
                            Logging.Write("Failed to enter transport " + selectedTransport.Name + "(" + selectedTransport.Id + ") going back to the quay.");
                            GoToDepartureQuayOrPortal(selectedTransport);
                            EnterTransportOrTakePortal(selectedTransport);
                        }
                    }
                    MovementManager.MoveTo(selectedTransport.ArrivalIsA ? selectedTransport.BInsidePoint : selectedTransport.AInsidePoint);
                    bool loop = true;
                    while (loop)
                    {
                        if (ObjectManager.ObjectManager.Me.InInevitableCombat || ObjectManager.ObjectManager.Me.IsDead)
                            return false;
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
            return false;
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

        private Taxi GetTaxiByTaxiId(uint taxiId)
        {
            foreach (Taxi availableTaxi in _availableTaxis)
            {
                if (availableTaxi.Id == taxiId)
                    return availableTaxi;
            }
            return new Taxi();
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
                if (!ObjectManager.ObjectManager.Me.OnTaxi && refPoint.DistanceTo(ObjectManager.ObjectManager.Me.Position) < 1.0f)
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

        private List<Transport> GetAllTransportsThatGoesToDestination(Point travelTo, int travelToContinentId, Point travelFrom, int travelFromContinentId)
        {
            var allTransports = new List<Transport>();
            List<Transport> transports = GetTransportsThatGoesToDestination(travelTo, travelToContinentId);
            List<CustomPath> customPaths = GetCustomPathsThatGoesToDestination(travelTo, travelToContinentId, travelFrom, travelFromContinentId);
            List<Portal> portals = GetPortalsThatGoesToDestination(travelTo, travelToContinentId);
            Taxi taxi = GetTaxiThatGoesToDestination(travelTo, travelToContinentId);
            allTransports.AddRange(transports);
            allTransports.AddRange(customPaths);
            allTransports.AddRange(portals);
            if (taxi != null)
                allTransports.Add(taxi);
            return allTransports;
        }

        private Transport GetTransportByTransportId(uint id)
        {
            foreach (var transport in _availableTransports.Items)
            {
                if (transport.Id == id)
                    return transport;
            }
            return new Transport();
        }

        private List<Transport> GetTransportsThatGoesToDestination(Point travelTo, int travelToContinentId)
        {
            var listTransport = new List<Transport>();
            foreach (Transport transport in _availableTransports.Items)
            {
                Thread.Sleep(5);
                if (transport.Faction != Npc.FactionType.Neutral && transport.Faction.ToString() != ObjectManager.ObjectManager.Me.PlayerFaction)
                    continue;
                if (transport.AContinentId != travelToContinentId && transport.BContinentId != travelToContinentId)
                    continue;
                // toDo: Break reference to transport from _availableTransports. http://stackoverflow.com/questions/31603679/how-to-stop-reference-of-other-class-object
                if (transport.AContinentId == travelToContinentId && transport.BContinentId != travelToContinentId)
                {
                    bool success;
                    transport.ArrivalIsA = true;
                    PathFinder.FindPath(transport.AOutsidePoint, travelTo, Usefuls.ContinentNameMpqByContinentId(travelToContinentId), out success);
                    if (success)
                    {
                        listTransport.Add(transport);
                        continue;
                    }
                    if (transport.ALift <= 0)
                        continue;
                    var aLift = GetTransportByTransportId(transport.ALift);
                    if (aLift.Id <= 0)
                        continue;
                    transport.UseALift = true;
                    if (!(aLift is Portal) && (!(aLift is CustomPath) || (aLift as CustomPath).RoundTrip))
                    {
                        PathFinder.FindPath((aLift is CustomPath) ? aLift.APoint : aLift.AOutsidePoint, travelTo, Usefuls.ContinentNameMpqByContinentId(travelToContinentId), out success);
                        if (success)
                        {
                            listTransport.Add(transport);
                            continue;
                        }
                    }
                    PathFinder.FindPath((aLift is CustomPath) || (aLift is Portal) ? aLift.BPoint : aLift.BOutsidePoint, travelTo, Usefuls.ContinentNameMpqByContinentId(travelToContinentId), out success);
                    if (success)
                    {
                        listTransport.Add(transport);
                        continue;
                    }
                }
                else if (transport.BContinentId == travelToContinentId && transport.AContinentId != travelToContinentId)
                {
                    bool success;
                    transport.ArrivalIsA = false;
                    PathFinder.FindPath(transport.BOutsidePoint, travelTo, Usefuls.ContinentNameMpqByContinentId(travelToContinentId), out success);
                    if (success)
                    {
                        listTransport.Add(transport);
                        continue;
                    }
                    if (transport.BLift <= 0)
                        continue;
                    var bLift = GetTransportByTransportId(transport.BLift);
                    if (bLift.Id <= 0)
                        continue;
                    transport.UseBLift = true;
                    if (!(bLift is Portal) && (!(bLift is CustomPath) || (bLift as CustomPath).RoundTrip))
                    {
                        PathFinder.FindPath((bLift is CustomPath) ? bLift.APoint : bLift.AOutsidePoint, travelTo, Usefuls.ContinentNameMpqByContinentId(travelToContinentId), out success);
                        if (success)
                        {
                            listTransport.Add(transport);
                            continue;
                        }
                    }
                    PathFinder.FindPath((bLift is CustomPath) || (bLift is Portal) ? bLift.BPoint : bLift.BOutsidePoint, travelTo, Usefuls.ContinentNameMpqByContinentId(travelToContinentId), out success);
                    if (success)
                    {
                        listTransport.Add(transport);
                        continue;
                    }
                }
                else if (transport.AContinentId == travelToContinentId && transport.BContinentId == travelToContinentId)
                {
                    var distanceToTravel = ObjectManager.ObjectManager.Me.Position.DistanceTo(travelTo);
                    bool success;
                    if (transport.AOutsidePoint.DistanceTo(travelTo) < transport.BOutsidePoint.DistanceTo(travelTo))
                    {
                        if ((distanceToTravel + 1000f) < transport.AOutsidePoint.DistanceTo(travelTo))
                            continue;
                        transport.ArrivalIsA = true;
                        PathFinder.FindPath(transport.AOutsidePoint, travelTo, Usefuls.ContinentNameMpqByContinentId(travelToContinentId), out success);
                        if (success)
                        {
                            listTransport.Add(transport);
                        }
                        else if (transport.ALift > 0)
                        {
                            var aLift = GetTransportByTransportId(transport.ALift);
                            if (aLift.Id <= 0) continue;
                            transport.UseALift = true;
                            if (!(aLift is Portal) && (!(aLift is CustomPath) || (aLift as CustomPath).RoundTrip))
                            {
                                PathFinder.FindPath((aLift is CustomPath) ? aLift.APoint : aLift.AOutsidePoint, travelTo, Usefuls.ContinentNameMpqByContinentId(travelToContinentId), out success);
                                if (success)
                                {
                                    listTransport.Add(transport);
                                    continue;
                                }
                            }
                            PathFinder.FindPath((aLift is CustomPath) || (aLift is Portal) ? aLift.BPoint : aLift.BOutsidePoint, travelTo, Usefuls.ContinentNameMpqByContinentId(travelToContinentId), out success);
                            if (success)
                            {
                                listTransport.Add(transport);
                                continue;
                            }
                        }
                    }
                    else
                    {
                        if ((distanceToTravel + 1000f) < transport.BOutsidePoint.DistanceTo(travelTo))
                            continue;
                        transport.UseALift = false; // it's fine to set it false here as we are the bottom of the function.
                        transport.ArrivalIsA = false;
                        PathFinder.FindPath(transport.BOutsidePoint, travelTo, Usefuls.ContinentNameMpqByContinentId(travelToContinentId), out success);
                        if (success)
                        {
                            listTransport.Add(transport);
                        }
                        else if (transport.BLift > 0)
                        {
                            var bLift = GetTransportByTransportId(transport.BLift);
                            if (bLift.Id <= 0) continue;
                            transport.UseBLift = true;
                            if (!(bLift is Portal) && (!(bLift is CustomPath) || (bLift as CustomPath).RoundTrip))
                            {
                                PathFinder.FindPath((bLift is CustomPath) ? bLift.APoint : bLift.AOutsidePoint, travelTo, Usefuls.ContinentNameMpqByContinentId(travelToContinentId), out success);
                                if (success)
                                {
                                    listTransport.Add(transport);
                                    continue;
                                }
                            }
                            PathFinder.FindPath((bLift is CustomPath) || (bLift is Portal) ? bLift.BPoint : bLift.BOutsidePoint, travelTo, Usefuls.ContinentNameMpqByContinentId(travelToContinentId), out success);
                            if (success)
                            {
                                listTransport.Add(transport);
                                continue;
                            }
                        }
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
                Thread.Sleep(5);
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

        private Taxi CreateTaxiFromRef(Taxi taxi)
        {
            return new Taxi
            {
                Faction = taxi.Faction,
                Id = taxi.Id,
                Name = taxi.Name,
                ContinentId = taxi.ContinentId,
                Position = taxi.Position,
                Xcoord = taxi.Xcoord,
                Ycoord = taxi.Ycoord
            };
        }

        private Taxi GetTaxisThatDirectlyGoToDestination(Point travelTo, Point travelFrom, int travelToContinentId, int travelFromContinentId)
        {
            Taxi bestTaxi = GetTaxiThatGoesToDestination(travelTo, travelToContinentId);
            if (bestTaxi == null)
                return null;
            // Sort taxis by distance from our position
            _availableTaxis.Sort(delegate(Taxi x, Taxi y) { return (travelFrom.DistanceTo(x.Position) < travelFrom.DistanceTo(y.Position) ? -1 : 1); });
            uint cnt = 0;
            for (int i = 0; i < _availableTaxis.Count; i++)
            {
                Thread.Sleep(5);
                Taxi taxi = CreateTaxiFromRef(_availableTaxis[i]); // this breaks the reference, so we can safely modify "EndOfPath".
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
                Thread.Sleep(5);
                if (portal.BContinentId != travelToContinentId)
                    continue;
                if (portal.AContinentId == portal.BContinentId)
                {
                    var distanceToTravel = travelTo.DistanceTo(ObjectManager.ObjectManager.Me.Position);
                    // if we are on the same continent, don't even generate a path if it's way farther than us.
                    if ((distanceToTravel + 1000f) < portal.BPoint.DistanceTo(travelTo))
                        continue;
                }
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
                Thread.Sleep(5);
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

        private List<CustomPath> GetCustomPathsThatGoesToDestination(Point travelTo, int travelToContinentId, Point travelFrom, int travelFromContinentId)
        {
            var listCustomPath = new List<CustomPath>();
            foreach (CustomPath customPath in _availableCustomPaths.Items)
            {
                Thread.Sleep(5);
                if (customPath.Faction != Npc.FactionType.Neutral && customPath.Faction.ToString() != ObjectManager.ObjectManager.Me.PlayerFaction)
                    continue;
                if (customPath.AContinentId != travelToContinentId && customPath.BContinentId != travelToContinentId)
                    continue;
                if (customPath.AContinentId != travelFromContinentId && customPath.BContinentId != travelFromContinentId)
                    continue;
                if (customPath.AContinentId == travelToContinentId && customPath.BContinentId != travelFromContinentId)
                    continue;
                if (customPath.BContinentId == travelToContinentId && customPath.AContinentId != travelFromContinentId)
                    continue;
                if (customPath.AContinentId == travelToContinentId && !customPath.AllowFar && travelTo.DistanceTo(customPath.APoint) > 2000)
                    continue;
                if (customPath.BContinentId == travelToContinentId && !customPath.AllowFar && travelTo.DistanceTo(customPath.BPoint) > 2000)
                    continue; // Don't allow CustomPath too far away.
                var distanceToTravel = travelTo.DistanceTo(travelFrom);

                // if we are on the same continent, don't even generate a path if it's way farther than us.
                bool success;
                // Check for continent switch first (rare but faster than a distance to anyway).
                if (customPath.AContinentId == travelToContinentId && customPath.BContinentId == travelFromContinentId && customPath.APoint.DistanceTo(travelTo) < customPath.BPoint.DistanceTo(travelTo))
                {
                    if (customPath.AContinentId == customPath.BContinentId && travelTo.DistanceTo(customPath.APoint) > distanceToTravel + 1000f)
                        continue;
                    PathFinder.FindPath(customPath.APoint, travelTo, Usefuls.ContinentNameMpqByContinentId(travelToContinentId), out success);
                    if (success && customPath.RoundTrip)
                    {
                        customPath.ArrivalIsA = true;
                        listCustomPath.Add(customPath);
                    }
                }
                else if (customPath.BContinentId == travelToContinentId && customPath.AContinentId == travelFromContinentId)
                {
                    if (customPath.AContinentId == customPath.BContinentId && travelTo.DistanceTo(customPath.BPoint) > distanceToTravel + 1000f)
                        continue;
                    PathFinder.FindPath(customPath.BPoint, travelTo, Usefuls.ContinentNameMpqByContinentId(travelToContinentId), out success);
                    if (success)
                    {
                        customPath.ArrivalIsA = false;
                        listCustomPath.Add(customPath);
                    }
                }
            }
            return listCustomPath;
        }

        private List<CustomPath> GetCustomPathsThatDirectlyGoToDestination(Point travelTo, Point travelFrom, int travelToContinentId, int travelFromContinentId)
        {
            var listCustomPath = new List<CustomPath>();
            List<CustomPath> customPaths = GetCustomPathsThatGoesToDestination(travelTo, travelToContinentId, travelFrom, travelFromContinentId);
            foreach (CustomPath customPath in customPaths)
            {
                Thread.Sleep(5);
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
                Thread.Sleep(5);
                if (transport.ArrivalIsA)
                {
                    bool success;
                    if (transport.BContinentId == travelFromContinentId)
                    {
                        PathFinder.FindPath(transport.BOutsidePoint, travelFrom, Usefuls.ContinentNameMpqByContinentId(travelFromContinentId), out success);
                        if (success)
                        {
                            listTransport.Add(transport);
                            continue;
                        }
                    }
                    if (transport.BLift > 0)
                    {
                        transport.UseBLift = true;
                        Transport bLift = GetTransportByTransportId(transport.BLift);
                        if (bLift.AContinentId == travelFromContinentId)
                        {
                            PathFinder.FindPath((bLift is CustomPath) || (bLift is Portal) ? bLift.APoint : bLift.AOutsidePoint, travelFrom, Usefuls.ContinentNameMpqByContinentId(travelFromContinentId), out success);
                            if (success)
                            {
                                listTransport.Add(transport);
                                continue;
                            }
                        }
                        if ((bLift is Portal) || (bLift is CustomPath) && !(bLift as CustomPath).RoundTrip)
                            continue;
                        if (bLift.BContinentId == travelFromContinentId)
                        {
                            PathFinder.FindPath((bLift is CustomPath) ? bLift.BPoint : bLift.BOutsidePoint, travelFrom, Usefuls.ContinentNameMpqByContinentId(travelFromContinentId), out success);
                            if (success)
                            {
                                listTransport.Add(transport);
                                continue;
                            }
                        }
                    }
                }
                else
                {
                    bool success;
                    if (transport.AContinentId == travelFromContinentId)
                    {
                        PathFinder.FindPath(transport.AOutsidePoint, travelFrom, Usefuls.ContinentNameMpqByContinentId(travelFromContinentId), out success);
                        if (success)
                        {
                            listTransport.Add(transport);
                            continue;
                        }
                    }
                    if (transport.ALift > 0)
                    {
                        transport.UseALift = true;
                        Transport aLift = GetTransportByTransportId(transport.ALift);
                        if (aLift.AContinentId == travelFromContinentId)
                        {
                            PathFinder.FindPath((aLift is CustomPath) || (aLift is Portal) ? aLift.APoint : aLift.AOutsidePoint, travelFrom, Usefuls.ContinentNameMpqByContinentId(travelFromContinentId), out success);
                            if (success)
                            {
                                listTransport.Add(transport);
                                continue;
                            }
                        }
                        if ((aLift is Portal) || (aLift is CustomPath) && !(aLift as CustomPath).RoundTrip)
                            continue;
                        if (aLift.BContinentId == travelFromContinentId)
                        {
                            PathFinder.FindPath((aLift is CustomPath) ? aLift.BPoint : aLift.BOutsidePoint, travelFrom, Usefuls.ContinentNameMpqByContinentId(travelFromContinentId), out success);
                            if (success)
                            {
                                listTransport.Add(transport);
                                continue;
                            }
                        }
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

        private KeyValuePair<Transport, float> GetBestDirectWayTaxi(Point travelFrom, Point travelTo, int travelFromContinentId, int travelToContinentId)
        {
            bool success;
            Taxi taxi = GetTaxisThatDirectlyGoToDestination(travelTo, travelFrom, travelToContinentId, travelFromContinentId);
            if (taxi == null)
                return new KeyValuePair<Transport, float>(new Transport(), float.MaxValue);
            List<Point> wayIn = PathFinder.FindPath(travelFrom, taxi.APoint, Usefuls.ContinentNameMpqByContinentId(travelFromContinentId), out success);
            if (!success) return new KeyValuePair<Transport, float>(new Transport(), float.MaxValue);
            List<Point> wayOff = PathFinder.FindPath(taxi.BPoint, travelTo, Usefuls.ContinentNameMpqByContinentId(travelToContinentId), out success);
            if (!success) return new KeyValuePair<Transport, float>(new Transport(), float.MaxValue);
            float bestTransportDistance = Math.DistanceListPoint(wayIn) + Math.DistanceListPoint(wayOff);
            if (travelFromContinentId == travelToContinentId)
                bestTransportDistance += (taxi.APoint.DistanceTo(taxi.BPoint)/2.5f);
            Transport bestTransport = taxi;
            bestTransport.Id = taxi.Id;
            return bestTransport.Id != 0 ? new KeyValuePair<Transport, float>(bestTransport, bestTransportDistance) : new KeyValuePair<Transport, float>(new Transport(), float.MaxValue);
        }

        private KeyValuePair<Transport, float> GetBestDirectWayTransport(Point travelFrom, Point travelTo, int travelFromContinentId, int travelToContinentId)
        {
            bool success;
            var bestTransport = new Transport();
            float bestTransportDistance = float.MaxValue;
            List<Transport> allTransports = GetAllTransportsThatDirectlyGoToDestination(travelTo, travelFrom, travelToContinentId, travelFromContinentId);
            foreach (Transport transport in allTransports)
            {
                Thread.Sleep(5);
                float currentTransportDistance = 0f;
                uint currentId = 0;
                if (transport is Taxi)
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
                    List<Point> wayIn;
                    var wayInLift = new List<Point>();
                    List<Point> wayOff;
                    var wayOffLift = new List<Point>();
                    if (!(transport is Portal) && (!(transport is CustomPath) || (transport as CustomPath).RoundTrip) && transport.ArrivalIsA)
                    {
                        if (!transport.UseBLift)
                        {
                            wayIn = PathFinder.FindPath(travelFrom, (transport is CustomPath) ? transport.BPoint : transport.BOutsidePoint, Usefuls.ContinentNameMpqByContinentId(travelFromContinentId), out success);
                            if (!success)
                                continue;
                        }
                        else
                        {
                            // we need to find the right "ArrivalIsA" for that elevator.
                            Transport bLift = GetTransportByTransportId(transport.BLift);
                            wayIn = PathFinder.FindPath(travelFrom, (bLift is Portal) || (bLift is CustomPath) ? bLift.APoint : bLift.AOutsidePoint, Usefuls.ContinentNameMpqByContinentId(travelFromContinentId),
                                out success);
                            if (success && !(bLift is Portal) && (!(bLift is CustomPath) || (bLift as CustomPath).RoundTrip))
                            {
                                // bLift IsArrivalIsA=true
                                wayInLift = PathFinder.FindPath((transport is CustomPath) ? transport.BPoint : transport.BOutsidePoint, (bLift is CustomPath) ? bLift.BPoint : bLift.BOutsidePoint,
                                    Usefuls.ContinentNameMpqByContinentId(transport.BContinentId), out success);
                                if (!success)
                                    continue;
                            }
                            else
                            {
                                // bLift IsArrivalIsA=false
                                wayIn = PathFinder.FindPath(travelFrom, (bLift is Portal) || (bLift is CustomPath) ? bLift.BPoint : bLift.BOutsidePoint, Usefuls.ContinentNameMpqByContinentId(travelFromContinentId),
                                    out success);
                                if (!success)
                                    continue;
                                wayInLift = PathFinder.FindPath((transport is CustomPath) ? transport.BPoint : transport.BOutsidePoint, (bLift is Portal) || (bLift is CustomPath) ? bLift.APoint : bLift.AOutsidePoint,
                                    Usefuls.ContinentNameMpqByContinentId(transport.BContinentId), out success);
                                if (!success)
                                    continue;
                            }
                            // we already did the check prior to that, so let's assume it's correct.
                        }
                        if (!transport.UseALift)
                        {
                            wayOff = PathFinder.FindPath(travelTo, (transport is CustomPath) ? transport.APoint : transport.AOutsidePoint, Usefuls.ContinentNameMpqByContinentId(travelToContinentId), out success);
                            if (!success)
                                continue;
                        }
                        else
                        {
                            // we need to find the right "ArrivalIsA" for that elevator.
                            Transport aLift = GetTransportByTransportId(transport.ALift);
                            wayOff = PathFinder.FindPath(travelTo, (aLift is Portal) || (aLift is CustomPath) ? aLift.APoint : aLift.AOutsidePoint, Usefuls.ContinentNameMpqByContinentId(travelToContinentId),
                                out success);
                            if (success && !(aLift is Portal) && (!(aLift is CustomPath) || (aLift as CustomPath).RoundTrip))
                            {
                                // aLift IsArrivalIsA=true
                                wayOffLift = PathFinder.FindPath((transport is CustomPath) ? transport.APoint : transport.AOutsidePoint, (aLift is CustomPath) ? aLift.BPoint : aLift.BOutsidePoint,
                                    Usefuls.ContinentNameMpqByContinentId(transport.AContinentId), out success);
                                if (!success)
                                    continue;
                            }
                            else
                            {
                                // aLift IsArrivalIsA=false
                                wayOff = PathFinder.FindPath(travelTo, (aLift is Portal) || (aLift is CustomPath) ? aLift.BPoint : aLift.BOutsidePoint, Usefuls.ContinentNameMpqByContinentId(travelToContinentId),
                                    out success);
                                if (!success)
                                    continue;
                                wayOffLift = PathFinder.FindPath((transport is CustomPath) ? transport.APoint : transport.AOutsidePoint,
                                    (aLift is Portal) || (aLift is CustomPath) ? aLift.APoint : aLift.AOutsidePoint,
                                    Usefuls.ContinentNameMpqByContinentId(transport.AContinentId), out success);
                                if (!success)
                                    continue;
                            }
                            // we already did the check prior to that, so let's assume it's correct.
                        }
                    }
                    else
                    {
                        // transport.IsArrivalIsA = false.
                        if (!transport.UseALift)
                        {
                            wayIn = PathFinder.FindPath(travelFrom, (transport is CustomPath) || (transport is Portal) ? transport.APoint : transport.AOutsidePoint,
                                Usefuls.ContinentNameMpqByContinentId(travelFromContinentId), out success);
                            if (!success)
                                continue;
                        }
                        else
                        {
                            // we need to find the right "ArrivalIsA" for that elevator.
                            Transport aLift = GetTransportByTransportId(transport.ALift);
                            wayIn = PathFinder.FindPath(travelFrom, (aLift is Portal) || (aLift is CustomPath) ? aLift.APoint : aLift.AOutsidePoint, Usefuls.ContinentNameMpqByContinentId(travelFromContinentId),
                                out success);
                            if (success && !(aLift is Portal) && (!(aLift is CustomPath) || (aLift as CustomPath).RoundTrip))
                            {
                                // aLift IsArrivalIsA=true
                                wayInLift = PathFinder.FindPath((transport is CustomPath) || (transport is Portal) ? transport.APoint : transport.AOutsidePoint,
                                    (aLift is CustomPath) ? aLift.BPoint : aLift.BOutsidePoint, Usefuls.ContinentNameMpqByContinentId(transport.AContinentId), out success);
                                if (!success)
                                    continue;
                            }
                            else
                            {
                                // aLift IsArrivalIsA=false
                                wayIn = PathFinder.FindPath(travelFrom, (aLift is Portal) || (aLift is CustomPath) ? aLift.BPoint : aLift.BOutsidePoint, Usefuls.ContinentNameMpqByContinentId(travelFromContinentId),
                                    out success);
                                if (!success)
                                    continue;
                                wayInLift = PathFinder.FindPath((transport is CustomPath) || (transport is Portal) ? transport.APoint : transport.AOutsidePoint,
                                    (aLift is Portal) || (aLift is CustomPath) ? aLift.APoint : aLift.AOutsidePoint, Usefuls.ContinentNameMpqByContinentId(transport.AContinentId), out success);
                                if (!success)
                                    continue;
                            }
                            // we already did the check prior to that, so let's assume it's correct.
                        }
                        float currentTransportMidDistance = Math.DistanceListPoint(wayIn) + Math.DistanceListPoint(wayInLift);
                        if (currentTransportMidDistance > bestTransportDistance)
                            continue; // Do not waste time on parsing wayOff if wayIn is already longer.

                        if (!transport.UseBLift)
                        {
                            wayOff = PathFinder.FindPath(travelTo, (transport is CustomPath) || (transport is Portal) ? transport.BPoint : transport.BOutsidePoint,
                                Usefuls.ContinentNameMpqByContinentId(travelToContinentId), out success);
                            if (!success)
                                continue;
                        }
                        else
                        {
                            // we need to find the right "ArrivalIsA" for that elevator.
                            Transport bLift = GetTransportByTransportId(transport.BLift);
                            wayOff = PathFinder.FindPath(travelTo, (transport is CustomPath) || (transport is Portal) ? transport.APoint : transport.AOutsidePoint,
                                Usefuls.ContinentNameMpqByContinentId(travelToContinentId), out success);
                            if (success && !(bLift is Portal) && (!(bLift is CustomPath) || (bLift as CustomPath).RoundTrip))
                            {
                                // bLift IsArrivalIsA=true
                                wayOffLift = PathFinder.FindPath((transport is CustomPath) || (transport is Portal) ? transport.BPoint : transport.BOutsidePoint,
                                    (bLift is CustomPath) ? bLift.BPoint : bLift.BOutsidePoint, Usefuls.ContinentNameMpqByContinentId(transport.BContinentId), out success);
                                if (!success)
                                    continue;
                            }
                            else
                            {
                                // bLift IsArrivalIsA=false
                                wayOff = PathFinder.FindPath(travelTo, (bLift is Portal) || (bLift is CustomPath) ? bLift.BPoint : bLift.BOutsidePoint, Usefuls.ContinentNameMpqByContinentId(travelToContinentId),
                                    out success);
                                if (!success)
                                    continue;
                                wayOffLift = PathFinder.FindPath((transport is CustomPath) || (transport is Portal) ? transport.BPoint : transport.BOutsidePoint,
                                    (bLift is Portal) || (bLift is CustomPath) ? bLift.APoint : bLift.AOutsidePoint, Usefuls.ContinentNameMpqByContinentId(transport.BContinentId), out success);
                                if (!success)
                                    continue;
                            }
                            // we already did the check prior to that, so let's assume it's correct.
                        }
                    }
                    float extraDistance = 0f;
                    if (transport.Id == 190549)
                        extraDistance =
                            ((transport is CustomPath) || (transport is Portal) ? transport.APoint : transport.AOutsidePoint).DistanceTo2D((transport is CustomPath) || (transport is Portal)
                                ? transport.BPoint
                                : transport.BOutsidePoint);
                    currentTransportDistance = extraDistance + Math.DistanceListPoint(wayIn) + Math.DistanceListPoint(wayInLift) + Math.DistanceListPoint(wayOff) + Math.DistanceListPoint(wayOffLift);
                    currentId = transport.Id;
                }

                if (!(currentTransportDistance < bestTransportDistance)) continue;
                bestTransport = transport;
                bestTransport.Id = currentId;
                bestTransportDistance = currentTransportDistance;
            }
            return bestTransport.Id != 0 ? new KeyValuePair<Transport, float>(bestTransport, bestTransportDistance) : new KeyValuePair<Transport, float>(new Transport(), float.MaxValue);
        }
    }
}