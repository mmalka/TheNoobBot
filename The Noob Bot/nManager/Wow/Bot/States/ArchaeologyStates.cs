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
    public class ArchaeologyStates : State
    {
        private enum LocState
        {
            Looting,
            GoingNextPoint,
            Survey,
            Iddle,
        }

        public override string DisplayName
        {
            get { return "Archaeology"; }
        }

        public override int Priority { get; set; }

        private readonly List<int> BlackListDigsites = new List<int>();
        private int LastZone;
        private Digsite digsitesZone = new Digsite();
        private WoWQuestPOIPoint qPOI;
        private int _nbTryFarmInThisZone;
        private Spell surveySpell;
        private Timer timerAutoSolving;

        public int SolvingEveryXMin = 20;
        private int nbLootAttempt;
        public int MaxTryByDigsite = 30;
        private int nbCastSurveyError;
        public bool UseKeystones = true;

        private LocState myState = LocState.Iddle;
        private Timer timerLooting;

        private int _bestPathId;
        private int _lastPathId;
        private bool _bestPathStatus;
        private bool _currentFindPathStatus;
        private readonly List<List<Point>> _pathFound = new List<List<Point>>();
        // This point is used to remember where the last green survey was, so that we can detect ping-pong between 2 points.
        private Point _lastGreenPosition = new Point();
        private bool _AntiPingPong = false;
        private int _greenCount = 0;

        private bool _inSecondDigSiteWithSameName = false;

        private bool IsPointOutOfWater(Point p)
        {
            return !TraceLine.TraceLineGo(new Point(p.X, p.Y, p.Z + 1000), p, Enums.CGWorldFrameHitFlags.HitTestLiquid);
        }

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
                _inSecondDigSiteWithSameName = false;

                if (listDigsitesZone.Count > 0)
                {
                    Digsite tDigsitesZone = new Digsite {id = 0, name = ""};
                    float distance = 99999999999999999f;
                    float priority = -999999999999f;
                    foreach (Digsite t in listDigsitesZone)
                    {
                        if (BlackListDigsites.Contains(t.id) || !t.Active) continue;
                        if (!(t.PriorityDigsites >= priority) &&
                            ((MountTask.GetMountCapacity() > MountCapacity.Ground) || _bestPathStatus))
                            continue;
                        WoWResearchSite OneSite = WoWResearchSite.FromName(t.name);
                        WoWQuestPOIPoint Polygon = WoWQuestPOIPoint.FromSetId(OneSite.Record.QuestIdPoint);
                        Point center = Polygon.Center;
                        float dist = center.DistanceTo(ObjectManager.ObjectManager.Me.Position);
                        if (!(dist < distance) && ((MountTask.GetMountCapacity() > MountCapacity.Ground) || _bestPathStatus))
                            continue;
                        if (MountTask.GetMountCapacity() <= MountCapacity.Ground)
                        {
                            _pathFound.AddRange(new[] {PathFinder.FindPath(ObjectManager.ObjectManager.Me.Position, center, Usefuls.ContinentNameMpq, out _currentFindPathStatus)});
                            _lastPathId = _pathFound.Count - 1;
                            _bestPathStatus = _currentFindPathStatus;
                            if (_bestPathStatus && !_currentFindPathStatus)
                                continue;
                            if (_bestPathStatus)
                                _bestPathId = _pathFound.Count - 1;
                        }
                        priority = t.PriorityDigsites;
                        distance = dist;
                        tDigsitesZone = t;
                        qPOI = Polygon;
                    }
                    if (tDigsitesZone.id != 0)
                    {
                        if (surveySpell == null)
                            surveySpell = new Spell("Survey");
                        digsitesZone = tDigsitesZone;
                        return true;
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

                // Get if this zone is last zone
                if (LastZone != digsitesZone.id)
                    _nbTryFarmInThisZone = 0; // Reset nb try farm if zone is not last zone
                LastZone = digsitesZone.id; // Set lastzone

                // Solving Every X Min
                if (timerAutoSolving == null)
                    timerAutoSolving = new Timer(SolvingEveryXMin*1000*60);
                if (timerAutoSolving.IsReady && !ObjectManager.ObjectManager.Me.IsDeadMe &&
                    !ObjectManager.ObjectManager.Me.InCombat)
                {
                    MovementManager.StopMove();
                    LongMove.StopLongMove();
                    Archaeology.SolveAllArtifact(UseKeystones);
                    timerAutoSolving = new Timer(SolvingEveryXMin*1000*60);
                }

                if (MovementManager.InMovement)
                    return;

                // Loop farm in zone // We must check Me.IsIndoor because no archeology is indoor
                int nbStuck = 0; // Nb of stuck direct
                try
                {
                    if (myState != LocState.Iddle)
                        MountTask.DismountMount();

                    ObjectManager.WoWGameObject t =
                        ObjectManager.ObjectManager.GetNearestWoWGameObject(
                            ObjectManager.ObjectManager.GetWoWGameObjectByEntry(Archaeology.ArchaeologyItemsFindList));
                    if (t.IsValid) // If found then loot
                    {
                        nbCastSurveyError = 0;
                        _lastGreenPosition = new Point();
                        _AntiPingPong = false;
                        _greenCount = 0;
                        if (myState == LocState.Looting)
                            if (timerLooting != null && timerLooting.IsReady)
                            {
                                MovementsAction.Jump();
                                Thread.Sleep(1500);
                            }
                            else
                                return;
                        bool ValidPath;
                        List<Point> points = PathFinder.FindPath(t.Position, out ValidPath, false);
                        if (!ValidPath)
                        {
                            MountTask.Mount();
                            Point p = new Point(t.Position);
                            p.Z += 5.0f;
                            points.Add(t.Position);
                        }
                        MovementManager.Go(points);
                        if (nbLootAttempt > 2)
                        {
                            MovementManager.StopMove();
                            LongMove.StopLongMove();
                            if (Archaeology.SolveAllArtifact(UseKeystones) == 0)
                            {
                                nManagerSetting.AddBlackList(t.Guid); // bugged artifacts not lootable
                                Logging.Write("Black-listing bugged artifact");
                            }
                            nbLootAttempt = 0;
                            return;
                        }
                        Logging.Write("Loot " + t.Name);
                        Timer timer = new Timer(1000*Math.DistanceListPoint(points)/3);

                        while (MovementManager.InMovement && !timer.IsReady && t.GetDistance > 3.5f)
                        {
                            Thread.Sleep(100);
                            if ((ObjectManager.ObjectManager.Me.InCombat &&
                                 !(ObjectManager.ObjectManager.Me.IsMounted &&
                                   (nManagerSetting.CurrentSetting.IgnoreFightIfMounted || Usefuls.IsFlying))))
                            {
                                return;
                            }
                        }
                        MovementManager.StopMove(); // avoid a red wow error
                        Thread.Sleep(150);
                        Interact.InteractWith(t.GetBaseAddress);
                        while (ObjectManager.ObjectManager.Me.IsCast)
                        {
                            Thread.Sleep(100);
                        }
                        if (ObjectManager.ObjectManager.Me.InCombat)
                        {
                            return;
                        }
                        Statistics.Farms++;
                        nbLootAttempt++;
                        myState = LocState.Looting;
                        if (timerLooting == null)
                            timerLooting = new Timer(1000*5);
                        else
                            timerLooting.Reset();
                        return;
                    }
                    if (_nbTryFarmInThisZone > MaxTryByDigsite) // If try > config try black list
                    {
                        nbLootAttempt = 0;
                        BlackListDigsites.Add(digsitesZone.id);
                        Logging.Write("Black List Digsite: " + digsitesZone.name);
                        myState = LocState.Iddle;
                        nbCastSurveyError = 0;
                        return;
                    }
                    bool moreMovementNeeded = false;
                    if (qPOI != null && !qPOI.ValidPoint && qPOI.Center.DistanceTo2D(ObjectManager.ObjectManager.Me.Position) < 40.0f)
                    {
#pragma warning disable 168
                        // We call qPOI.MiddlePoint to make the WoWQuestPOIPoint instance compute the middle point, but I don't care were it is
                        Point p = qPOI.MiddlePoint; // we are near enough to compute it
#pragma warning restore 168
                        if (Usefuls.IsFlying)
                        {
                            moreMovementNeeded = true;
                            MovementManager.StopMove();
                        }
                    }
                    // Go To Zone
                    if (qPOI != null && (moreMovementNeeded || !qPOI.IsInside(ObjectManager.ObjectManager.Me.Position)))
                    {
                        if (MountTask.GetMountCapacity() == MountCapacity.Feet || MountTask.GetMountCapacity() == MountCapacity.Ground)
                        {
                            Logging.Write("Not inside, then go to Digsite " + digsitesZone.name);
                            if (_bestPathId == 0)
                                _bestPathId = _lastPathId;
                            MovementManager.Go(new List<Point>(_pathFound[_bestPathId]));
                        }
                        else if (qPOI.ValidPoint)
                        {
                            if (moreMovementNeeded || !qPOI.IsInside(ObjectManager.ObjectManager.Me.Position))
                            {
                                Point destination = new Point(qPOI.MiddlePoint);
                                destination.Type = "flying";
                                if (moreMovementNeeded)
                                    Logging.Write("Landing on the digsite");
                                else
                                    Logging.Write("Not inside, then go to Digsite " + digsitesZone.name + "; X: " + destination.X + "; Y: " + destination.Y + "; Z: " + (int) destination.Z);
                                MovementManager.Go(new List<Point>(new[] {destination})); // MoveTo Digsite
                            }
                        }
                        else
                        {
                            // here we need to go to center, THEN compute middle point
                            Point destination = qPOI.Center;
                            destination.Type = "flying";
                            Logging.Write("Go to Digsite " + digsitesZone.name + "; X: " + destination.X + "; Y: " + destination.Y + "; Z: " + (int) destination.Z);
                            MovementManager.Go(new List<Point>(new[] {destination})); // MoveTo Digsite
                        }
                        myState = LocState.Iddle;
                        return;
                    }
                    // Find loot with Survey
                    nbLootAttempt = 0;
                    t = ObjectManager.ObjectManager.GetNearestWoWGameObject(
                        ObjectManager.ObjectManager.GetWoWGameObjectByDisplayId(Archaeology.SurveyList));
                    if (t.GetBaseAddress == 0 || myState == LocState.GoingNextPoint ||
                        // recast if we moved even if last is still spawned
                        myState == LocState.Looting)
                        // after we looted we need to recast survey spell, even if the previous one is still spawned
                    {
                        if (!Archaeology.DigsiteZoneIsAvailable(digsitesZone))
                            return;

                        if (myState == LocState.Iddle)
                            MountTask.DismountMount();

                        surveySpell.Launch();
                        myState = LocState.Survey;
                        if (ObjectManager.ObjectManager.Me.InCombat)
                            return;
                        Thread.Sleep(1750 + Usefuls.Latency); // let's wait a fair bit
                        nbCastSurveyError++;
                        if (nbCastSurveyError > 3)
                        {
                            if (ObjectManager.ObjectManager.Me.Position.DistanceTo2D(qPOI.MiddlePoint) < 5)
                            {
                                // This means we are in a wrong digsite
                                List<Digsite> listDigsitesZone;
                                if (!_inSecondDigSiteWithSameName)
                                    listDigsitesZone = Archaeology.GetDigsitesZoneAvailable(t.Name);
                                else // very very rare case I had: back to the first digsite with same name after the 2nd one
                                    listDigsitesZone = Archaeology.GetDigsitesZoneAvailable();
                                foreach (Digsite dg in listDigsitesZone)
                                    if (dg.name == t.Name)
                                        digsitesZone = dg;
                                WoWResearchSite OneSite;
                                if (!_inSecondDigSiteWithSameName)
                                    OneSite = WoWResearchSite.FromName(digsitesZone.name, true);
                                else
                                    OneSite = WoWResearchSite.FromName(digsitesZone.name, false);
                                qPOI = WoWQuestPOIPoint.FromSetId(OneSite.Record.QuestIdPoint);
                                _inSecondDigSiteWithSameName = !_inSecondDigSiteWithSameName;
                                nbCastSurveyError = 0;
                                return;
                            }
                            if (MountTask.GetMountCapacity() == MountCapacity.Feet || MountTask.GetMountCapacity() == MountCapacity.Ground)
                            {
                                if (_bestPathId == 0)
                                    _bestPathId = _lastPathId;
                                Logging.Write("Too many errors, then go to Digsite " + digsitesZone.name);
                                MovementManager.Go(new List<Point>(_pathFound[_bestPathId]));
                            }
                            else
                            {
                                if (qPOI != null)
                                {
                                    Point destination = qPOI.MiddlePoint;
                                    Logging.Write("Too many errors, then go to Digsite " + digsitesZone.name + "; X: " + destination.X + "; Y: " + destination.Y + "; Z: " + (int) destination.Z);
                                    MovementManager.Go(new List<Point>(new[] {destination})); // MoveTo Digsite
                                }
                            }
                            nbCastSurveyError = 0;
                            return;
                        }
                        _nbTryFarmInThisZone++;
                        return;
                    }
                    if (myState == LocState.GoingNextPoint)
                        return;
                    nbCastSurveyError = 0; // Reset try cast survey
                    if ((ObjectManager.ObjectManager.Me.InCombat &&
                         !(ObjectManager.ObjectManager.Me.IsMounted &&
                           (nManagerSetting.CurrentSetting.IgnoreFightIfMounted || Usefuls.IsFlying))))
                    {
                        return;
                    }
                    Point p0;
                    float angle;
                    {
                        Point p;
                        float distance, distanceMin, distanceMax, decrement;
                        if (t.DisplayId == 10103) // Survey Tool (Red) 100 yard
                        {
                            distance = 90f;
                            distanceMin = 20f;
                            distanceMax = 210f;
                            decrement = 10f;
                            _lastGreenPosition = new Point();
                            _AntiPingPong = false;
                            _greenCount = 0;
                        }
                        else if (t.DisplayId == 10102) // Survey Tool (Yellow) 50 yard
                        {
                            distance = 46f;
                            distanceMin = 20f;
                            distanceMax = 56f;
                            decrement = 6.5f;
                            _lastGreenPosition = new Point();
                            _AntiPingPong = false;
                            _greenCount = 0;
                        }
                        else // Survey Tool (Green) 25 yard (t.DisplayId == 10101)
                        {
                            _greenCount++;
                            if (_greenCount >= 10)
                            {
                                _greenCount = 0;
                                _lastGreenPosition = new Point();
                                Point destination = qPOI.MiddlePoint;
                                _AntiPingPong = false;
                                Logging.Write("Stuck, then go to Digsite " + digsitesZone.name + "; X: " + destination.X + "; Y: " + destination.Y + "; Z: " + (int) destination.Z);
                                MountTask.Mount();
                                MovementManager.Go(new List<Point>(new[] {destination})); // MoveTo Digsite
                                return;
                            }
                            if (_AntiPingPong)
                            {
                                Logging.Write("Ping-pong detected, shortening the distance");
                                distance = 10f;
                                distanceMin = 6f;
                                distanceMax = 15f;
                                decrement = 4f;
                            }
                            else
                            {
                                distance = 19f;
                                distanceMin = 7f;
                                distanceMax = 29f;
                                decrement = 3f;
                            }
                        }
                        {
                            float d = distance;
                            p0 = new Point(t.Position);
                            angle = t.Orientation;
                            p = Math.GetPosition2DOfAngleAndDistance(p0, angle, d);

                            p.Z += 5.0f; // just so that the the GetZ don't find caves too easiely
                            p.Z = PathFinder.GetZPosition(p, true);
                            bool valid;
                            PathFinder.FindPath(p, out valid);
                            if (qPOI != null)
                            {
                                bool IamOutOfWater = IsPointOutOfWater(ObjectManager.ObjectManager.Me.Position);
                                while (!valid || p.Z == 0 || !qPOI.IsInside(p) || !(!IamOutOfWater || IsPointOutOfWater(p)))
                                {
                                    if (d + 5 > distanceMax)
                                        break;
                                    d += 5;
                                    Point newone = Math.GetPosition2DOfAngleAndDistance(p0, angle, d);
                                    if (qPOI.IsInside(newone))
                                    {
                                        p = new Point(newone);
                                        p.Z += 5.0f; // just so that the the GetZ don't find caves too easiely
                                        p.Z = PathFinder.GetZPosition(p, true); //(t.DisplayId == 10101 ? false : true));
                                        if (p.Z == 0) // if p == 0 we don't care about the path
                                            valid = false;
                                        else if (Math.DistanceListPoint(PathFinder.FindPath(p, out valid)) > d*5 && d > 30)
                                            valid = false;
                                    }
                                }
                                if (!valid || p.Z == 0 || !qPOI.IsInside(p) || !(!IamOutOfWater || IsPointOutOfWater(p)))
                                {
                                    d = distance;
                                    while (!valid || p.Z == 0 || !qPOI.IsInside(p) || !(!IamOutOfWater || IsPointOutOfWater(p)))
                                    {
                                        if (d - decrement < distanceMin)
                                            break;
                                        d -= decrement;
                                        Point newone = Math.GetPosition2DOfAngleAndDistance(p0, angle, d);
                                        if (qPOI.IsInside(newone))
                                        {
                                            p = new Point(newone);
                                            p.Z += 5.0f; // just so that the the GetZ don't find caves too easiely
                                            p.Z = PathFinder.GetZPosition(p, true);
                                            if (p.Z == 0) // if p == 0 we don't care about the path
                                                valid = false;
                                            else if (Math.DistanceListPoint(PathFinder.FindPath(p, out valid)) > d*5 && d > 30)
                                                valid = false;
                                        }
                                    }
                                }
                            }
                            // check pingpong but not a second time
                            if (_AntiPingPong)
                                _AntiPingPong = false;
                            else if (t.DisplayId == 10101 && _lastGreenPosition.IsValid && p.DistanceTo2D(_lastGreenPosition) <= 6f)
                                _AntiPingPong = true;
                            // then remmember the last Green Position
                            if (t.DisplayId == 10101)
                                _lastGreenPosition = new Point(ObjectManager.ObjectManager.Me.Position);
                            if (_AntiPingPong)
                            {
                                myState = LocState.Iddle;
                                return;
                            }
                            Logging.Write("Distance " + d + " selected");
                        }
                        myState = LocState.GoingNextPoint;

                        // Find Path
                        bool resultB;
                        List<Point> points = PathFinder.FindPath(p, out resultB, false);

                        // If path not found find nearer
                        if (points.Count <= 0)
                        {
                            Point pt = Math.GetPosition2DOfAngleAndDistance(p0, angle, 15);
                            pt.Z = ObjectManager.ObjectManager.Me.Position.Z;
                            points = PathFinder.FindPath(pt, out resultB, false);
                            if (points.Count > 0 && resultB)
                                p = new Point(pt);
                        }

                        // Go to next position
                        if ((!resultB && p.DistanceTo(ObjectManager.ObjectManager.Me.Position) > 10) ||
                            nbStuck >= 2)
                            // Use fly mount
                        {
                            p.Z = PathFinder.GetZPosition(p);

                            if (p.Z == 0)
                                p.Z = ObjectManager.ObjectManager.Me.Position.Z + 35;
                            else
                                p.Z = p.Z + 5.0f;

                            if ((ObjectManager.ObjectManager.Me.InCombat &&
                                 !(ObjectManager.ObjectManager.Me.IsMounted &&
                                   (nManagerSetting.CurrentSetting.IgnoreFightIfMounted || Usefuls.IsFlying))))
                            {
                                return;
                            }
                            MountTask.Mount();
                            LongMove.LongMoveByNewThread(p);
                            Timer timer = new Timer(2000*points[points.Count - 1].DistanceTo(ObjectManager.ObjectManager.Me.Position)/3);

                            while (LongMove.IsLongMove && !timer.IsReady &&
                                   ObjectManager.ObjectManager.Me.Position.DistanceTo2D(p) > 0.5f)
                            {
                                if ((ObjectManager.ObjectManager.Me.InCombat &&
                                     !(ObjectManager.ObjectManager.Me.IsMounted &&
                                       (nManagerSetting.CurrentSetting.IgnoreFightIfMounted || Usefuls.IsFlying))))
                                {
                                    LongMove.StopLongMove();
                                    return;
                                }
                                Thread.Sleep(100);
                            }
                            LongMove.StopLongMove();
                            while (MovementManager.IsUnStuck)
                            {
                                Thread.Sleep(100);
                                LongMove.StopLongMove();
                            }
                            MovementManager.StopMove();
                            MountTask.DismountMount();
// ReSharper disable RedundantAssignment
                            nbStuck = 0;
// ReSharper restore RedundantAssignment
                        }
                        else //  walk to next position
                        {
                            float d1 = Math.DistanceListPoint(points);
                            float d2 = points[0].DistanceTo(points[points.Count - 1]);
                            // here we will try to shortcut the path using a fly mount
                            if (MountTask.GetMountCapacity() == MountCapacity.Fly && d1 > 80 && d1 > (d2*2))
                            {
                                Point startpoint = new Point(ObjectManager.ObjectManager.Me.Position);
                                Point endpoint = new Point(points[points.Count - 1]);
                                float z1 = startpoint.Z;
                                float z2 = endpoint.Z;
                                float zref = System.Math.Max(z1, z2) + 6.0f;
                                Point pref1 = new Point(startpoint);
                                pref1.Z = zref;
                                Point pref2 = new Point(endpoint);
                                pref2.Z = zref;
                                bool badres = TraceLine.TraceLineGo(startpoint, pref1) ||
                                              TraceLine.TraceLineGo(pref1, pref2) ||
                                              TraceLine.TraceLineGo(pref2, endpoint);
                                if (!badres)
                                {
                                    Logging.Write("Flying to shortcut the path");
                                    MountTask.Mount();
                                    if (Usefuls.IsFlying) // Failsafe: in case we are indoor don't try
                                    {
                                        points = new List<Point>();
                                        pref1.Z += 2f;
                                        pref2.Z += 2f;
                                        points.Add(pref1);
                                        points.Add(pref2);
                                        points.Add(endpoint);
                                    }
                                }
                            }
                            if (Usefuls.IsFlying)
                                for (int i = 0; i < points.Count; i++)
                                    points[i].Type = "flying";
                            // Disabled because this does not work fine
                            //else if (Usefuls.IsSwimming && !IsPointOutOfWater(points[points.Count - 1]))
                            //    for (int i = 0; i < points.Count; i++)
                            //        points[i].Type = "swimming";
                            MovementManager.Go(points);
                            float d = Math.DistanceListPoint(points)/3;
                            if (d > 200)
                                d = 200;
                            float tm_t = 1000*d/2 + 1500;
                            if (Usefuls.IsSwimming)
                                tm_t /= 0.6f;
                            Timer timer = new Timer(tm_t);
                            while (MovementManager.InMovement && !timer.IsReady &&
                                   ObjectManager.ObjectManager.Me.Position.DistanceTo2D(p) > 0.5f)
                            {
                                if ((ObjectManager.ObjectManager.Me.InCombat &&
                                     !(ObjectManager.ObjectManager.Me.IsMounted &&
                                       (nManagerSetting.CurrentSetting.IgnoreFightIfMounted || Usefuls.IsFlying))))
                                {
                                    return;
                                }
                                Thread.Sleep(100);
                            }
                            // incremente nbstuck if player is stuck
                            if (ObjectManager.ObjectManager.Me.Position.DistanceTo(t.Position) < 5 ||
                                (MovementManager.InMovement &&
                                 !(ObjectManager.ObjectManager.Me.InCombat &&
                                   !(ObjectManager.ObjectManager.Me.IsMounted &&
                                     (nManagerSetting.CurrentSetting.IgnoreFightIfMounted || Usefuls.IsFlying))) &&
                                 timer.IsReady))
// ReSharper disable RedundantAssignment
                                nbStuck++;
// ReSharper restore RedundantAssignment
                            else
// ReSharper disable RedundantAssignment
                                nbStuck = 0;
// ReSharper restore RedundantAssignment
                            if ((ObjectManager.ObjectManager.Me.InCombat &&
                                 !(ObjectManager.ObjectManager.Me.IsMounted &&
                                   (nManagerSetting.CurrentSetting.IgnoreFightIfMounted || Usefuls.IsFlying))))
                            {
                                return;
                            }
                            MovementManager.StopMove();
                            while (MovementManager.IsUnStuck)
                            {
                                Thread.Sleep(100);
                                MovementManager.StopMove();
                            }
                        }
                    }
                }
                catch
                {
                }
            }
            catch
            {
            }
        }
    }
}