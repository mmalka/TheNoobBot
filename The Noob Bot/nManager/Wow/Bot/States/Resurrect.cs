using System.Collections.Generic;
using System.Threading;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;
using nManager.Wow.Patchables;

namespace nManager.Wow.Bot.States
{
    public class Resurrect : State
    {
        public override string DisplayName
        {
            get { return "Resurrect"; }
        }

        public override int Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }
        private int _priority;

        public override List<State> NextStates
        {
            get { return new List<State>(); }
        }

        public override List<State> BeforeStates
        {
            get { return new List<State>(); }
        }

        public override bool NeedToRun
        {
            get
            {
                if (!Usefuls.InGame ||
                    Usefuls.IsLoadingOrConnecting)
                    return false;

                if (ObjectManager.ObjectManager.Me.IsDeadMe &&
                    ObjectManager.ObjectManager.Me.IsValid &&
                    Products.Products.IsStarted)
                    return true;

                return false;
            }
        }

        private bool _failled;
        public override void Run()
        {
            MovementManager.StopMove();
            MovementManager.StopMoveTo();
            Logging.Write("Player dead");
            Interact.Repop();
            Thread.Sleep(1000);
            while (ObjectManager.ObjectManager.Me.PositionCorpse.X == 0 && ObjectManager.ObjectManager.Me.PositionCorpse.Y == 0 &&
                   ObjectManager.ObjectManager.Me.Health <= 0 && Products.Products.IsStarted && Usefuls.InGame)
            {
                Interact.Repop();
                Thread.Sleep(1000);
            }
            Thread.Sleep(1000);

            #region GoToCorp

            if (ObjectManager.ObjectManager.Me.PositionCorpse.X != 0 && ObjectManager.ObjectManager.Me.PositionCorpse.Y != 0 && !nManagerSetting.CurrentSetting.UseSpiritHealer)
            {
                while (Usefuls.IsLoadingOrConnecting && Products.Products.IsStarted && Usefuls.InGame)
                {
                    Thread.Sleep(100);
                }
                Thread.Sleep(1000);
                Point tPointCorps;
                if (ObjectManager.ObjectManager.Me.IsMounted)
                {
                    Keybindings.DownKeybindings(Enums.Keybindings.JUMP);
                    Thread.Sleep(500);
                    Keybindings.UpKeybindings(Enums.Keybindings.JUMP);
                    tPointCorps = ObjectManager.ObjectManager.Me.PositionCorpse;
                    tPointCorps.Z = tPointCorps.Z + 10;
                    LongMove.LongMoveByNewThread(tPointCorps);
                }
                else
                {
                    tPointCorps = ObjectManager.ObjectManager.Me.PositionCorpse;
                    List<Point> points = PathFinder.FindPath(tPointCorps);
                    if (points.Count > 1 || (points.Count <= 1 && !nManagerSetting.CurrentSetting.UseSpiritHealer))
                        MovementManager.Go(points);
                }
                while ((MovementManager.InMovement || LongMove.IsLongMove) &&
                       Products.Products.IsStarted &&
                       Usefuls.InGame && ObjectManager.ObjectManager.Me.IsDeadMe)
                {
                    if ((tPointCorps.DistanceTo(ObjectManager.ObjectManager.Me.Position) < 25 && !_failled) ||
                        (Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule +
                                                     (uint)Addresses.Player.RetrieveCorpseWindow) > 0 && !_failled) || ObjectManager.ObjectManager.Me.PositionCorpse.DistanceTo(ObjectManager.ObjectManager.Me.Position) < 5)
                    {
                        LongMove.StopLongMove();
                        MovementManager.StopMove();
                    }
                    Thread.Sleep(100);
                }

                if (Usefuls.IsFlying)
                {
                    Tasks.MountTask.Land();
                }

                if (Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule +
                                                     (uint)Addresses.Player.RetrieveCorpseWindow) <= 0)
                {
                    _failled = true;
                }

                if (tPointCorps.DistanceTo(ObjectManager.ObjectManager.Me.Position) < 26 ||
                    Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule + (uint)Addresses.Player.RetrieveCorpseWindow) >
                    0)
                {
                    while ((tPointCorps.DistanceTo(ObjectManager.ObjectManager.Me.Position) < 27 ||
                            Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule +
                                                         (uint)Addresses.Player.RetrieveCorpseWindow) > 0) &&
                           ObjectManager.ObjectManager.Me.IsDeadMe && Products.Products.IsStarted && Usefuls.InGame)
                    {
                        Interact.RetrieveCorpse();
                        Thread.Sleep(1000);
                    }
                }
            }
            if (!ObjectManager.ObjectManager.Me.IsDeadMe)
            {
                _failled = false;
                Logging.Write("Player retrieve corpse");
                Statistics.Deaths++;
                return;
            }

            #endregion GoToCorp

            #region SpiritHealer

            if (nManagerSetting.CurrentSetting.UseSpiritHealer)
            {
                Thread.Sleep(4000);
                var objectSpiritHealer =
                    new WoWUnit(ObjectManager.ObjectManager.GetNearestWoWUnit(ObjectManager.ObjectManager.GetWoWUnitSpiritHealer()).GetBaseAddress);
                int stuckTemps = 5;

                if (!objectSpiritHealer.IsValid)
                {
                    Logging.Write("Not found Spirit Healer");
                    Interact.TeleportToSpiritHealer();
                    Thread.Sleep(5000);
                }
                else
                {
                    if (objectSpiritHealer.GetDistance > 25)
                    {
                        Interact.TeleportToSpiritHealer();
                        Thread.Sleep(5000);
                    }
                    MovementManager.MoveTo(objectSpiritHealer.Position);
                    while (objectSpiritHealer.GetDistance > 5 && Products.Products.IsStarted && stuckTemps >= 0 && Usefuls.InGame)
                    {
                        Thread.Sleep(300);
                        if (!ObjectManager.ObjectManager.Me.GetMove && objectSpiritHealer.GetDistance > 5)
                        {
                            MovementManager.MoveTo(objectSpiritHealer.Position);
                            stuckTemps--;
                        }
                    }
                    Interact.InteractGameObject(objectSpiritHealer.GetBaseAddress);
                    Thread.Sleep(2000);
                    Interact.SpiritHealerAccept();
                    Thread.Sleep(1000);
                    if (!ObjectManager.ObjectManager.Me.IsDeadMe)
                    {
                        Logging.Write("Player retrieve corpse");
                        Statistics.Deaths++;
                    }
                }

            }
            #endregion SpiritHealer

        }
    }
}
