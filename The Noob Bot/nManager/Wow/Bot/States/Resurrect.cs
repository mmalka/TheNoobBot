using System.Collections.Generic;
using System.Management.Instrumentation;
using System.Threading;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;
using nManager.Wow.Patchables;
using Timer = nManager.Helpful.Timer;

namespace nManager.Wow.Bot.States
{
    public class Resurrect : State
    {
        public override string DisplayName
        {
            get { return "Resurrect"; }
        }

        public override int Priority { get; set; }

        private Timer _battlegroundResurrect = new Timer(0);
        private readonly Spell _shamanReincarnation = new Spell("Reincarnation");
        private readonly Spell _warlockSoulstone = new Spell("Soulstone");
        private bool _forceSpiritHealer;

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
                    Usefuls.IsLoading)
                    return false;

                if (ObjectManager.ObjectManager.Me.IsDeadMe &&
                    ObjectManager.ObjectManager.Me.IsValid &&
                    Products.Products.IsStarted)
                    return true;

                return false;
            }
        }

        private bool _failed;

        public override void Run()
        {
            MovementManager.StopMove();
            MovementManager.StopMoveTo();
            Logging.Write("The player has died. Starting the resurrection process.");

            #region Reincarnation

            if (ObjectManager.ObjectManager.Me.WowClass == WoWClass.Shaman && _shamanReincarnation.KnownSpell && _shamanReincarnation.IsSpellUsable)
            {
                Thread.Sleep(3500); // Let our killers reset.
                Lua.RunMacroText("/click StaticPopup1Button2");
                Thread.Sleep(1000);
                if (!ObjectManager.ObjectManager.Me.IsDeadMe)
                {
                    _failed = false;
                    Logging.Write("The player have been resurrected using Shaman Reincarnation.");
                    Statistics.Deaths++;
                    return;
                }
            }

            #endregion

            #region Soulstone

            if (ObjectManager.ObjectManager.Me.WowClass == WoWClass.Warlock && _warlockSoulstone.KnownSpell && _warlockSoulstone.HaveBuff ||
                ObjectManager.ObjectManager.Me.HaveBuff(6203))
            {
                Thread.Sleep(3500); // Let our killers reset.
                Lua.RunMacroText("/click StaticPopup1Button2");
                Thread.Sleep(1000);
                if (!ObjectManager.ObjectManager.Me.IsDeadMe)
                {
                    _failed = false;
                    Logging.Write(ObjectManager.ObjectManager.Me.WowClass == WoWClass.Warlock
                        ? "The player have been resurrected using his Soulstone."
                        : "The player have been resurrected using a Soulstone offered by a Warlock.");
                    Statistics.Deaths++;
                    return;
                }
            }

            #endregion

            Interact.Repop();
            Thread.Sleep(1000);
            while (!ObjectManager.ObjectManager.Me.PositionCorpse.IsValid && ObjectManager.ObjectManager.Me.Health <= 0 && Products.Products.IsStarted && Usefuls.InGame)
            {
                Interact.Repop();
                Thread.Sleep(1000);
            }
            Thread.Sleep(1000);

            #region Battleground resurrection

            if (Usefuls.IsInBattleground)
            {
                _battlegroundResurrect = new Timer(1000*35);
                while (Usefuls.IsLoading && Products.Products.IsStarted && Usefuls.InGame)
                {
                    Thread.Sleep(100);
                }
                Thread.Sleep(4000);
                /*var factionBattlegroundSpiritHealer =
                    new WoWUnit(
                        ObjectManager.ObjectManager.GetNearestWoWUnit(
                            ObjectManager.ObjectManager.GetWoWUnitByName(ObjectManager.ObjectManager.Me.PlayerFaction +
                                                                         " Spirit Guide")).GetBaseAddress);
                if (!factionBattlegroundSpiritHealer.IsValid)
                {
                    Logging.Write("Faction Spirit Healer not found, teleport back to the cimetery.");
                    Interact.TeleportToSpiritHealer();
                    Thread.Sleep(5000);
                }
                else
                {
                    if (factionBattlegroundSpiritHealer.GetDistance > 25)
                    {
                        Interact.TeleportToSpiritHealer();
                        Thread.Sleep(5000);
                    }*/
                while (ObjectManager.ObjectManager.Me.IsDeadMe)
                {
                    if (_battlegroundResurrect.IsReady)
                    {
                        Interact.TeleportToSpiritHealer();
                        _battlegroundResurrect = new Timer(1000*35);
                        Logging.Write("The player have not been resurrected by any Battleground Spirit Healer in a reasonable time, Teleport back to the cimetary.");
                        Thread.Sleep(5000);
                    }
                    Thread.Sleep(1000);
                }
                _failed = false;
                Logging.Write("The player have been resurrected by the Battleground Spirit Healer.");
                Statistics.Deaths++;
                return;
                /*}*/
            }

            #endregion

            #region Go To Corpse resurrection

            if (ObjectManager.ObjectManager.Me.PositionCorpse.IsValid && !nManagerSetting.CurrentSetting.UseSpiritHealer && !_forceSpiritHealer)
            {
                while (Usefuls.IsLoading && Products.Products.IsStarted && Usefuls.InGame)
                {
                    Thread.Sleep(100);
                }
                Thread.Sleep(1000);
                Point tPointCorps;
                if (ObjectManager.ObjectManager.Me.IsMounted)
                {
                    MovementsAction.Ascend(true);
                    Thread.Sleep(500);
                    MovementsAction.Ascend(false);
                    tPointCorps = ObjectManager.ObjectManager.Me.PositionCorpse;
                    tPointCorps.Z = tPointCorps.Z + 10;
                    LongMove.LongMoveByNewThread(tPointCorps);
                }
                else
                {
                    tPointCorps = ObjectManager.ObjectManager.Me.PositionCorpse;
                    bool success;
                    List<Point> points = PathFinder.FindPath(tPointCorps, out success);
                    if (!success)
                    {
                        _forceSpiritHealer = true;
                        Logging.Write("There in no easy acces to the corpse, use Spirit Healer instead.");
                        return;
                    }
                    if (points.Count > 1 || (points.Count <= 1 && !nManagerSetting.CurrentSetting.UseSpiritHealer))
                        MovementManager.Go(points);
                }
                while ((MovementManager.InMovement || LongMove.IsLongMove) && Products.Products.IsStarted && Usefuls.InGame && ObjectManager.ObjectManager.Me.IsDeadMe)
                {
                    if ((tPointCorps.DistanceTo(ObjectManager.ObjectManager.Me.Position) < 25 && !_failed) ||
                        (Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule + (uint) Addresses.Player.RetrieveCorpseWindow) > 0 && !_failed) ||
                        ObjectManager.ObjectManager.Me.PositionCorpse.DistanceTo(ObjectManager.ObjectManager.Me.Position) < 5)
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

                if (Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule + (uint) Addresses.Player.RetrieveCorpseWindow) <= 0)
                {
                    _failed = true;
                }
                Point safeResPoint = Usefuls.GetSafeResPoint();

                if (safeResPoint.IsValid && nManagerSetting.CurrentSetting.ActivateSafeResurrectionSystem)
                {
                    MovementManager.StopMove();

                    bool success;
                    List<Point> points = PathFinder.FindPath(safeResPoint, out success);
                    if (!success)
                        return;
                    MovementManager.Go(points);
                    while (safeResPoint.DistanceTo(ObjectManager.ObjectManager.Me.Position) > 5)
                    {
                        Thread.Sleep(1000);
                    }

                    MovementManager.StopMove();
                    if ((Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule + (uint) Addresses.Player.RetrieveCorpseWindow) > 0) && ObjectManager.ObjectManager.Me.IsDeadMe && Products.Products.IsStarted &&
                        Usefuls.InGame)
                    {
                        Interact.RetrieveCorpse();
                        Thread.Sleep(1000);
                    }
                }
                else
                {
                    if (tPointCorps.DistanceTo(ObjectManager.ObjectManager.Me.Position) < 26 || Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule + (uint) Addresses.Player.RetrieveCorpseWindow) > 0)
                    {
                        while ((tPointCorps.DistanceTo(ObjectManager.ObjectManager.Me.Position) < 27 || Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule + (uint) Addresses.Player.RetrieveCorpseWindow) > 0) &&
                               ObjectManager.ObjectManager.Me.IsDeadMe && Products.Products.IsStarted && Usefuls.InGame)
                        {
                            Interact.RetrieveCorpse();
                            Thread.Sleep(1000);
                        }
                    }
                }
            }
            if (!ObjectManager.ObjectManager.Me.IsDeadMe)
            {
                _failed = false;
                Logging.Write("The player have been resurrected when retrieving his corpse.");
                Statistics.Deaths++;
                return;
            }

            #endregion GoToCorp

            #region Spirit Healer resurrection

            if (nManagerSetting.CurrentSetting.UseSpiritHealer || _forceSpiritHealer)
            {
                Thread.Sleep(4000);
                WoWUnit objectSpiritHealer = new WoWUnit(ObjectManager.ObjectManager.GetNearestWoWUnit(ObjectManager.ObjectManager.GetWoWUnitSpiritHealer()).GetBaseAddress);
                int stuckTemps = 5;

                if (!objectSpiritHealer.IsValid)
                {
                    Logging.Write("Spirit Healer not found, teleport back to the cimetery.");
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
                    Interact.InteractWith(objectSpiritHealer.GetBaseAddress);
                    Thread.Sleep(2000);
                    Interact.SpiritHealerAccept();
                    Thread.Sleep(1000);
                    if (!ObjectManager.ObjectManager.Me.IsDeadMe)
                    {
                        _forceSpiritHealer = false;
                        Logging.Write("The player have been resurrected by the Spirit Healer.");
                        Statistics.Deaths++;
                        if (ObjectManager.ObjectManager.Me.HaveBuff(15007))
                        {
                            Logging.Write("Resurrection Sickness detected, we will now wait its full duration to avoid dieing in chain.");
                            while (ObjectManager.ObjectManager.Me.HaveBuff(15007))
                            {
                                Thread.Sleep(1000);
                                // We don't need to return if we get in combat, we would die quickly anyway, and we will ressurect from our body this time.
                            }
                        }
                    }
                }
            }

            #endregion SpiritHealer
        }
    }
}