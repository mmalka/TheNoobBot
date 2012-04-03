using System.Collections.Generic;
using System.Threading;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.Helpers;

namespace nManager.Wow.Bot.States
{
    public class ArchaeologyStates : State
    {

        public override string DisplayName
        {
            get { return "Archaeology"; }
        }

        public override int Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

        private int _priority;

        List<string> BlackListDigsites = new List<string>();
        private string LastZone = "";
        Digsite digsitesZone = new Digsite();
        private int _nbTryFarmInThisZone;
        private Spell surveySpell;
        private Helpful.Timer timerAutoSolving;

        public int SolvingEveryXMin = 30;
        public int MaxTryByDigsite = 30;

        public override bool NeedToRun
        {
            get
            {
                if (!Usefuls.InGame ||
                    Usefuls.IsLoadingOrConnecting ||
                    ObjectManager.ObjectManager.Me.IsDeadMe ||
                    !ObjectManager.ObjectManager.Me.IsValid ||
                    (ObjectManager.ObjectManager.Me.InCombat && !(ObjectManager.ObjectManager.Me.IsMounted && (nManagerSetting.CurrentSetting.ignoreFightGoundMount || Usefuls.IsFlying))) ||
                    !Products.Products.IsStarted)
                    return false;

                if (!BlackListDigsites.Contains(digsitesZone.px + digsitesZone.py + digsitesZone.continentId) && Archaeology.DigsiteZoneIsAvailable(digsitesZone))
                    return true;

                var listDigsitesZone = Archaeology.GetDigsitesZoneAvailable();

                if (listDigsitesZone.Count > 0)
                {
                    var tDigsitesZone = new Digsite { name = "", px = "", py = "" };
                    float distance = 99999999999999999;
                    float priority = -999999999999;
                    foreach (var t in listDigsitesZone)
                    {
                        if (!BlackListDigsites.Contains(t.px + t.py + t.continentId) && t.Active)
                        {
                            if (t.PriorityDigsites >= priority)
                            {
                                if (t.position.DistanceTo(ObjectManager.ObjectManager.Me.Position) < distance)
                                {
                                    priority = t.PriorityDigsites;
                                    distance = t.position.DistanceTo(ObjectManager.ObjectManager.Me.Position);
                                    tDigsitesZone = t;
                                }
                            }
                        }
                    }
                    if (tDigsitesZone.px != "")
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
                // Get if this zone is last zone
                if (LastZone != digsitesZone.px + digsitesZone.py + digsitesZone.continentId)
                    _nbTryFarmInThisZone = 0; // Reset nb try farm if zone is not last zone
                LastZone = digsitesZone.px + digsitesZone.py + digsitesZone.continentId; // Set lastzone

                // Solving Every X Min
                if (timerAutoSolving == null)
                    timerAutoSolving = new Helpful.Timer(SolvingEveryXMin * 1000 * 60);
                if (timerAutoSolving.IsReady && !ObjectManager.ObjectManager.Me.IsDeadMe && !ObjectManager.ObjectManager.Me.InCombat)
                {
                    MovementManager.StopMove();
                    LongMove.StopLongMove();
                    Archaeology.SolveAllArtifact();
                    timerAutoSolving = new Helpful.Timer(SolvingEveryXMin*1000*60);
                }

                // Go To Zone
                if (MovementManager.InMovement)
                    return;
                if (digsitesZone.position.DistanceTo(ObjectManager.ObjectManager.Me.Position) > 1000)
                {
                    Logging.Write("Go to Digsite " + digsitesZone.name);
                    MovementManager.Go(new List<Point>(new[]{digsitesZone.position})); // MoveTo Digsite
                    return;
                }

                // Loop farm in zone
                int nbStuck = 0; // Nb of stuck direct
                int nbCastSurveyError = 0; // Nb max error cast survey (for try if in zone ou if zone is finish)
                while (Products.Products.IsStarted && !Products.Products.InPause && nbCastSurveyError <= 3 && !ObjectManager.ObjectManager.Me.IsDeadMe && !(ObjectManager.ObjectManager.Me.InCombat && !(ObjectManager.ObjectManager.Me.IsMounted && (nManagerSetting.CurrentSetting.ignoreFightGoundMount || Usefuls.IsFlying))))
                {
                    try
                    {
                        Tasks.MountTask.DismountMount();

                        var t = ObjectManager.ObjectManager.GetNearestWoWGameObject(ObjectManager.ObjectManager.GetWoWGameObjectByEntry(Archaeology.ArchaeologyItemsFindList));
                        if (t.GetBaseAddress > 0) // If found loot
                        {
                            var points = PathFinder.FindPath(t.Position);
                            MovementManager.Go(points);
                            Logging.Write("Loot " + t.Name);
                            var timer = new Helpful.Timer(1000 * Math.DistanceListPoint(points) / 3);
                            Thread.Sleep(300);
                            while (MovementManager.InMovement && !timer.IsReady && t.GetDistance > 3)
                            {
                                if ((ObjectManager.ObjectManager.Me.InCombat && !(ObjectManager.ObjectManager.Me.IsMounted && (nManagerSetting.CurrentSetting.ignoreFightGoundMount || Usefuls.IsFlying))))
                                {
                                    return;
                                }
                                Thread.Sleep(100);
                            }
                            if ((ObjectManager.ObjectManager.Me.InCombat && !(ObjectManager.ObjectManager.Me.IsMounted && (nManagerSetting.CurrentSetting.ignoreFightGoundMount || Usefuls.IsFlying))))
                            {
                                return;
                            }
                            Thread.Sleep(1000);
                            Interact.InteractGameObject(t.GetBaseAddress);
                            Thread.Sleep(Usefuls.Latency);
                            if ((ObjectManager.ObjectManager.Me.InCombat && !(ObjectManager.ObjectManager.Me.IsMounted && (nManagerSetting.CurrentSetting.ignoreFightGoundMount || Usefuls.IsFlying))))
                            {
                                return;
                            }
                            Thread.Sleep(1000);
                            while (ObjectManager.ObjectManager.Me.IsCast)
                            {
                                if (ObjectManager.ObjectManager.Me.InCombat)
                                {
                                    return;
                                }
                                Thread.Sleep(500);
                            }
                            Statistics.Farms++;
                            if (ObjectManager.ObjectManager.Me.InCombat)
                            {
                                return;
                            }
                            Thread.Sleep(2000);
                        }
                        else if (_nbTryFarmInThisZone > MaxTryByDigsite) // If try > config try black list
                        {
                            BlackListDigsites.Add(digsitesZone.px + digsitesZone.py + digsitesZone.continentId);
                            Logging.Write("Black List Digsite: " + digsitesZone.name);
                            return;
                        }
                        else // Find loot with Survey
                        {
                            if (!Archaeology.DigsiteZoneIsAvailable(digsitesZone))
                            {
                                return;
                            } // Check if the zone is available.
                            Thread.Sleep(200);
                            surveySpell.Launch(); // Cast Survey
                            _nbTryFarmInThisZone++;
                            if ((ObjectManager.ObjectManager.Me.InCombat && !(ObjectManager.ObjectManager.Me.IsMounted && (nManagerSetting.CurrentSetting.ignoreFightGoundMount || Usefuls.IsFlying))))
                            {
                                return;
                            }
                            Thread.Sleep(1500);
                            Thread.Sleep(Usefuls.Latency);
                            t = ObjectManager.ObjectManager.GetNearestWoWGameObject(ObjectManager.ObjectManager.GetWoWGameObjectByDisplayId(Archaeology.SurveyList));
                            if (t.GetBaseAddress > 0) // If find Survey
                            {
                                nbCastSurveyError = 0; // Reset try cast survey

                                ObjectManager.ObjectManager.Me.Rotation = CGUnit_C__GetFacing.GetFacing(t.GetBaseAddress);
                                // set my rotation to survey rotation

                                // Get Line to next cast survey
                                var p1 = ObjectManager.ObjectManager.Me.Position;
                                Thread.Sleep(50);
                                Keybindings.DownKeybindings(Enums.Keybindings.MOVEFORWARD);
                                Thread.Sleep(800);
                                Keybindings.UpKeybindings(Enums.Keybindings.MOVEFORWARD);
                                var p2 = ObjectManager.ObjectManager.Me.Position;

                                if (p1.X == p2.X && p1.Y == p2.Y)
                                {
                                    ObjectManager.ObjectManager.Me.Rotation = CGUnit_C__GetFacing.GetFacing(t.GetBaseAddress);
                                    p2 = ObjectManager.ObjectManager.Me.Position;
                                    Thread.Sleep(50);
                                    Keybindings.DownKeybindings(Enums.Keybindings.MOVEBACKWARD);
                                    Thread.Sleep(800);
                                    Keybindings.UpKeybindings(Enums.Keybindings.MOVEBACKWARD);
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
                                        p = Math.GetPostion2DOfLineByDistance(p1, p2, 100 - 15);
                                    else if (t.DisplayId == 10102) // Survey Tool (Yellow) 50 yard
                                        p = Math.GetPostion2DOfLineByDistance(p1, p2, 50 - 10);
                                    else // Survey Tool (Green) 25 yard
                                        p = Math.GetPostion2DOfLineByDistance(p1, p2, 13);

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
                                    if ((!resultB && p.DistanceTo(ObjectManager.ObjectManager.Me.Position) > 10) || nbStuck >= 2)
                                    // Use fly mount
                                    {
                                        p.Z = PathFinder.GetZPosition(p) + 10;

                                        if (p.Z == 0)
                                            p.Z = ObjectManager.ObjectManager.Me.Position.Z + 35;

                                        if ((ObjectManager.ObjectManager.Me.InCombat && !(ObjectManager.ObjectManager.Me.IsMounted && (nManagerSetting.CurrentSetting.ignoreFightGoundMount || Usefuls.IsFlying))))
                                        {
                                            return;
                                        }
                                        Tasks.MountTask.MountingFlyMount();
                                        LongMove.LongMoveByNewThread(p);
                                        var timer =
                                            new Helpful.Timer(1000 *
                                                      points[points.Count - 1].DistanceTo(ObjectManager.ObjectManager.Me.Position) / 3);
                                        Thread.Sleep(300);
                                        while (LongMove.IsLongMove && !timer.IsReady &&
                                               ObjectManager.ObjectManager.Me.Position.DistanceTo2D(p) > 10)
                                        {
                                            if ((ObjectManager.ObjectManager.Me.InCombat && !(ObjectManager.ObjectManager.Me.IsMounted && (nManagerSetting.CurrentSetting.ignoreFightGoundMount || Usefuls.IsFlying))))
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
                                        Thread.Sleep(800);
                                        MovementManager.StopMove();
                                        Tasks.MountTask.DismountMount();
                                        nbStuck = 0;
                                    }
                                    else //  walk to next position
                                    {
                                        MovementManager.Go(points);

                                        float d = Math.DistanceListPoint(points) / 3;
                                        if (d > 200)
                                            d = 200;
                                        var timer = new Helpful.Timer(1000 * d / 2 + 1500);

                                        Thread.Sleep(300);
                                        while (MovementManager.InMovement && !timer.IsReady &&
                                               ObjectManager.ObjectManager.Me.Position.DistanceTo2D(p) > 5)
                                        {
                                            if ((ObjectManager.ObjectManager.Me.InCombat && !(ObjectManager.ObjectManager.Me.IsMounted && (nManagerSetting.CurrentSetting.ignoreFightGoundMount || Usefuls.IsFlying))))
                                            {
                                                return;
                                            }
                                            Thread.Sleep(100);
                                        }
                                        Thread.Sleep(50);
                                        // incremente nbstuck if player is stuck
                                        if (ObjectManager.ObjectManager.Me.Position.DistanceTo(t.Position) < 5 ||
                                            (MovementManager.InMovement && !(ObjectManager.ObjectManager.Me.InCombat && !(ObjectManager.ObjectManager.Me.IsMounted && (nManagerSetting.CurrentSetting.ignoreFightGoundMount || Usefuls.IsFlying))) && timer.IsReady))
                                            nbStuck++;
                                        else
                                            nbStuck = 0;


                                        if ((ObjectManager.ObjectManager.Me.InCombat && !(ObjectManager.ObjectManager.Me.IsMounted && (nManagerSetting.CurrentSetting.ignoreFightGoundMount || Usefuls.IsFlying))))
                                        {
                                            return;
                                        }
                                        MovementManager.StopMove();
                                        Thread.Sleep(500);
                                        while (MovementManager.IsUnStuck)
                                        {
                                            Thread.Sleep(100);
                                            MovementManager.StopMove();
                                        }
                                    }
                                }
                            }
                            else // If cast survey error
                            {
                                nbCastSurveyError++;
                                if (nbCastSurveyError > 3)
                                {
                                    Logging.Write("Go to Digsite " + digsitesZone.name);
                                    MovementManager.Go(new List<Point>(new[] { digsitesZone.position })); // MoveTo Digsite
                                    return;
                                }
                            }
                        }
                    }
                    catch
                    {
                    }
                    Thread.Sleep(700);
                }
            }
            catch
            {
            }
        }
    }
}
