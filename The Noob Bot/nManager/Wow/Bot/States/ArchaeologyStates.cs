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
            Solve,
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

        private LocState myState = LocState.Iddle;
        private Timer timerLooting;

        private int _bestPathId;
        private int _lastPathId;
        private bool _bestPathStatus;
        private bool _currentFindPathStatus;
        private readonly List<List<Point>> _pathFound = new List<List<Point>>();

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
                if (u.GetBaseAddress > 0)
                    return true;

                List<Digsite> listDigsitesZone = Archaeology.GetDigsitesZoneAvailable();

                if (listDigsitesZone.Count > 0)
                {
                    Digsite tDigsitesZone = new Digsite {id = 0, name = ""};
                    float distance = 99999999999999999f;
                    float priority = -999999999999f;
                    foreach (Digsite t in listDigsitesZone)
                    {
                        if (BlackListDigsites.Contains(t.id) || !t.Active) continue;
                        if (!(t.PriorityDigsites >= priority) &&
                            ((MountTask.GetMountCapacity() != MountCapacity.Feet && MountTask.GetMountCapacity() != MountCapacity.Ground) || _bestPathStatus))
                            continue;
                        WoWResearchSite OneSite = WoWResearchSite.FromName(t.name);
                        WoWQuestPOIPoint Polygon = WoWQuestPOIPoint.FromSetId(OneSite.Record.QuestIdPoint);
                        Point center = Polygon.Center;
                        float dist = center.DistanceTo(ObjectManager.ObjectManager.Me.Position);
                        if (!(dist < distance) && ((MountTask.GetMountCapacity() != MountCapacity.Feet && MountTask.GetMountCapacity() != MountCapacity.Ground) || _bestPathStatus))
                            continue;
                        if (MountTask.GetMountCapacity() == MountCapacity.Feet || MountTask.GetMountCapacity() == MountCapacity.Ground)
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
                    Archaeology.SolveAllArtifact();
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
                    if (t.GetBaseAddress > 0) // If found then loot
                    {
                        if (myState == LocState.Looting)
                            if (timerLooting != null && timerLooting.IsReady)
                            {
                                MovementsAction.Jump();
                                Thread.Sleep(2000);
                            }
                            else
                                return;

                        List<Point> points = PathFinder.FindPath(t.Position);
                        MovementManager.Go(points);
                        if (nbLootAttempt > 2)
                        {
                            MovementManager.StopMove();
                            LongMove.StopLongMove();
                            Archaeology.SolveAllArtifact();
                            nbLootAttempt = 0;
                            return;
                        }
                        Logging.Write("Loot " + t.Name);
                        Timer timer = new Timer(1000*Math.DistanceListPoint(points)/3);

                        while (MovementManager.InMovement && !timer.IsReady && t.GetDistance > 3)
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
                            Thread.Sleep(50);
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
                    if (qPOI != null && !qPOI.ValidPoint && qPOI.Center.DistanceTo2D(ObjectManager.ObjectManager.Me.Position) < 50.0f)
                    {
                        Point p = qPOI.MiddlePoint; // we are near enouth to compute it
                        if (Usefuls.IsFlying)
                            moreMovementNeeded = true;
                    }
                    // Go To Zone
                    if (qPOI != null && (moreMovementNeeded || !qPOI.IsInside(ObjectManager.ObjectManager.Me.Position)))
                    {
                        nbCastSurveyError = 0;
                        Logging.Write("Go to Digsite " + digsitesZone.name);
                        if (MountTask.GetMountCapacity() == MountCapacity.Feet || MountTask.GetMountCapacity() == MountCapacity.Ground)
                        {
                            if (_bestPathId == 0)
                                _bestPathId = _lastPathId;
                            MovementManager.Go(new List<Point>(_pathFound[_bestPathId]));
                        }
                        else if (qPOI.ValidPoint)
                        {
                            Point destination = qPOI.MiddlePoint;
                            destination.Type = "flying";
                            Logging.Write("Go to Digsite " + digsitesZone.name + "; X: " + destination.X + "; Y: " + destination.Y + "; Z: " + (int) destination.Z);
                            MovementManager.Go(new List<Point>(new[] {destination})); // MoveTo Digsite
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
                    t =
                        ObjectManager.ObjectManager.GetNearestWoWGameObject(
                            ObjectManager.ObjectManager.GetWoWGameObjectByDisplayId(Archaeology.SurveyList));
                    if (t.GetBaseAddress <= 0 || myState == LocState.GoingNextPoint ||
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
                        Thread.Sleep(250); // let's wait a bit
                        nbCastSurveyError++;
                        if (nbCastSurveyError > 3)
                        {
                            if (MountTask.GetMountCapacity() == MountCapacity.Feet || MountTask.GetMountCapacity() == MountCapacity.Ground)
                            {
                                if (_bestPathId == 0)
                                    _bestPathId = _lastPathId;
                                Logging.Write("Go to Digsite " + digsitesZone.name);
                                MovementManager.Go(new List<Point>(_pathFound[_bestPathId]));
                            }
                            else
                            {
                                if (qPOI != null)
                                {
                                    Point destination = qPOI.MiddlePoint;
                                    Logging.Write("Go to Digsite " + digsitesZone.name + "; X: " + destination.X + "; Y: " + destination.Y + "; Z: " + (int) destination.Z);
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

                    ObjectManager.ObjectManager.Me.Rotation = CGUnit_C__GetFacing.GetFacing(t.GetBaseAddress);
                    // set my rotation to survey rotation

                    // Get Line to next cast survey
                    Point p1 = ObjectManager.ObjectManager.Me.Position;
                    Thread.Sleep(50);
                    MovementsAction.MoveForward(true);
                    Thread.Sleep(200);
                    MovementsAction.MoveForward(false);
                    Point p2 = ObjectManager.ObjectManager.Me.Position;

                    if (p1.X == p2.X && p1.Y == p2.Y)
                    {
                        ObjectManager.ObjectManager.Me.Rotation = CGUnit_C__GetFacing.GetFacing(t.GetBaseAddress);
                        p2 = ObjectManager.ObjectManager.Me.Position;
                        Thread.Sleep(50);
                        MovementsAction.MoveBackward(true);
                        Thread.Sleep(200);
                        MovementsAction.MoveBackward(false);
                        p1 = ObjectManager.ObjectManager.Me.Position;
                    }
                    if (p1.X == p2.X && p1.Y == p2.Y) // Get if p1 != p2 (else wowerror)
                    {
                        MovementManager.UnStuck();
                    }
                    else // Get next cast survey position
                    {
                        Point p;

                        if (t.DisplayId == 10103) // Survey Tool (Red) 100 yard
                        {
                            int d = 90;
                            p = Math.GetPostion2DOfLineByDistance(p1, p2, d);
                            p.Z += 5.0f; // just so that the the GetZ don't find caves too easiely
                            p.Z = PathFinder.GetZPosition(p, true);
                            while (qPOI != null && (!qPOI.IsInside(p) || !IsPointOutOfWater(p) || p.Z == 0))
                            {
                                //Logging.Write("Point at " + d + " bad, testing " + (d + 5));
                                d += 5;
                                p = Math.GetPostion2DOfLineByDistance(p1, p2, d);
                                p.Z += 5.0f; // just so that the the GetZ don't find caves too easiely
                                p.Z = PathFinder.GetZPosition(p, true);
                                if (d >= 160)
                                    break;
                            }
                            d = 90;
                            while (qPOI != null && (!qPOI.IsInside(p) || !IsPointOutOfWater(p) || p.Z == 0))
                            {
                                //Logging.Write("Point at " + d + " bad, testing " + (d - 10));
                                d -= 10;
                                p = Math.GetPostion2DOfLineByDistance(p1, p2, d);
                                p.Z += 5.0f; // just so that the the GetZ don't find caves too easiely
                                p.Z = PathFinder.GetZPosition(p, true);
                                if (d <= 10)
                                    break;
                            }
                        }
                        else if (t.DisplayId == 10102) // Survey Tool (Yellow) 50 yard
                            p = Math.GetPostion2DOfLineByDistance(p1, p2, 50 - 10 + 6);
                        else // Survey Tool (Green) 25 yard
                            p = Math.GetPostion2DOfLineByDistance(p1, p2, 13 + 2.5f);

                        myState = LocState.GoingNextPoint;
                        p.Z += 5.0f; // just so that the the GetZ don't find caves too easiely
                        p.Z = PathFinder.GetZPosition(p, true);
                        if (p.Z == 0)
                            p.Z = ObjectManager.ObjectManager.Me.Position.Z;
                        // Find Path
                        bool resultB;
                        List<Point> points = PathFinder.FindPath(p, out resultB);

                        // If path not found find neawer
                        if (points.Count <= 0)
                        {
                            Point pt = Math.GetPostion2DOfLineByDistance(p1, p2, 15);
                            pt.Z = ObjectManager.ObjectManager.Me.Position.Z;
                            points = PathFinder.FindPath(pt, out resultB);
                            if (points.Count > 0 && resultB)
                                p = new Point(pt.X, pt.Y, pt.Z);
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
                            Timer timer =new Timer(1000*points[points.Count - 1].DistanceTo(ObjectManager.ObjectManager.Me.Position)/3);

                            while (LongMove.IsLongMove && !timer.IsReady &&
                                   ObjectManager.ObjectManager.Me.Position.DistanceTo2D(p) > 10)
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
                            nbStuck = 0;
                        }
                        else //  walk to next position
                        {
                            if (Usefuls.IsFlying)
                                for (int i = 0; i < points.Count; i++)
                                    points[i].Type = "flying";

                            MovementManager.Go(points);
                            float d = Math.DistanceListPoint(points)/3;
                            if (d > 200)
                                d = 200;
                            float tm_t = 1000*d/2 + 1200;
                            if (ObjectManager.ObjectManager.Me.Position.Type.ToLower() == "swimming")
                                tm_t /= 0.6f;
                            Timer timer = new Timer(tm_t);
                            while (MovementManager.InMovement && !timer.IsReady &&
                                   ObjectManager.ObjectManager.Me.Position.DistanceTo2D(p) > 5)
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
                                nbStuck++;
                            else
                                nbStuck = 0;

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