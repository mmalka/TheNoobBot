using System.Collections.Generic;
using System.Threading;
using Questing_Bot.Bot.Tasks;
using WowManager;
using WowManager.Addresses;
using WowManager.FiniteStateMachine;
using WowManager.Memory;
using WowManager.Memory.Process;
using WowManager.MiscEnums;
using WowManager.MiscStructs;
using WowManager.Navigation;
using WowManager.WoW.Interface;
using WowManager.WoW.ObjectManager;
using WowManager.WoW.PlayerManager;
using WowManager.WoW.WoWObject;

namespace Questing_Bot.Bot.States
{
    internal class Resurrect : State
    {
        public override string DisplayName
        {
            get { return "Resurrect"; }
        }

        public override int Priority
        {
            get { return (int)States.Priority.Resurrect; }
        }

        public override bool NeedToRun
        {
            get
            {
                if (!Useful.InGame ||
                    Useful.isLoadingOrConnecting)
                    return false;

                if (ObjectManager.Me.IsDeadMe &&
                    ObjectManager.Me.IsValid &&
                    Config.Bot.BotIsActive)
                    return true;

                return false;
            }
        }

        public override void Run()
        {
            MovementManager.StopMove();
            MovementManager.Stop();
            Log.AddLog(Translation.GetText(Translation.Text.Player_dead));
            Interact.Repop();
            Thread.Sleep(1000);
            while (ObjectManager.Me.PositionCorpse.X == 0 && ObjectManager.Me.PositionCorpse.Y == 0 &&
                   ObjectManager.Me.Health <= 0 && Config.Bot.BotIsActive && Useful.InGame)
            {
                Interact.Repop();
                Thread.Sleep(1000);
            }
            Thread.Sleep(1000);

            #region GoToCorp

            if (ObjectManager.Me.PositionCorpse.X != 0 && ObjectManager.Me.PositionCorpse.Y != 0)
            {
                while (Useful.isLoadingOrConnecting && Config.Bot.BotIsActive && Useful.InGame)
                {
                    Thread.Sleep(10);
                }
                Thread.Sleep(1000);
                Point tPointCorps;
                if (ObjectManager.Me.IsMount)
                {
                    WowManager.WoW.Useful.Keybindings.DownKeybindings(Keybindings.JUMP);
                    Thread.Sleep(1000);
                    WowManager.WoW.Useful.Keybindings.UpKeybindings(Keybindings.JUMP);
                    tPointCorps = ObjectManager.Me.PositionCorpse;
                    tPointCorps.X = tPointCorps.X + 20;
                    LongMove.LongMoveByNewThread(tPointCorps);
                }
                tPointCorps = ObjectManager.Me.PositionCorpse;
                List<Point> points = Functions.GoToPathFind(tPointCorps);
                if (points.Count > 1 || (points.Count <= 1 && !Config.Bot.FormConfig.UseSpiritHealer))
                    MovementManager.Go(points);

                while ((MovementManager.InMovement || LongMove.IsLongMove) &&
                       Config.Bot.BotIsActive &&
                       Useful.InGame && ObjectManager.Me.IsDeadMe)
                {
                    if (tPointCorps.DistanceTo(ObjectManager.Me.Position) < 25 ||
                        Memory.MyHook.Memory.ReadInt(Process.WowModule +
                                                     (uint)Addresses.Player.RetrieveCorpseWindow) > 0)
                    {
                        LongMove.StopLongMove();
                        MovementManager.StopMove();
                    }
                    Thread.Sleep(100);
                }

                if (Useful.IsFlying)
                {
                    WowManager.WoW.Useful.Keybindings.DownKeybindings(Keybindings.SITORSTAND);
                    Thread.Sleep(2500);
                    WowManager.WoW.Useful.Keybindings.UpKeybindings(Keybindings.SITORSTAND);
                }

                if (tPointCorps.DistanceTo(ObjectManager.Me.Position) < 26 ||
                    Memory.MyHook.Memory.ReadInt(Process.WowModule + (uint)Addresses.Player.RetrieveCorpseWindow) >
                    0)
                {
                    while ((tPointCorps.DistanceTo(ObjectManager.Me.Position) < 27 ||
                            Memory.MyHook.Memory.ReadInt(Process.WowModule +
                                                         (uint)Addresses.Player.RetrieveCorpseWindow) > 0) &&
                           ObjectManager.Me.IsDeadMe && Config.Bot.BotIsActive && Useful.InGame)
                    {
                        Interact.RetrieveCorpse();
                        Thread.Sleep(1000);
                    }
                }
            }
            if (!ObjectManager.Me.IsDeadMe)
            {
                Log.AddLog(Translation.GetText(Translation.Text.Player_retrieve_corpse));
                Config.Bot.Deaths++;
                return;
            }

            #endregion GoToCorp

            #region SpiritHealer

            if (Config.Bot.FormConfig.UseSpiritHealer)
            {
                Thread.Sleep(4000);
                List<WoWUnit> listSpiritHealer = ObjectManager.GetWoWUnitSpiritHealer();
                if (listSpiritHealer.Count > 0)
                {
                    var objectSpiritHealer =
                        new WoWUnit(ObjectManager.GetNearestWoWUnit(listSpiritHealer).GetBaseAddress);
                    int stuckTemps = 5;
                    if (objectSpiritHealer.Guid > 0)
                    {
                        MovementManager.MoveTo(objectSpiritHealer.Position);

                        while (objectSpiritHealer.GetDistance > 5 && Config.Bot.BotIsActive && stuckTemps >= 0 && Useful.InGame)
                        {
                            Thread.Sleep(300);
                            if (!ObjectManager.Me.GetMove && objectSpiritHealer.GetDistance > 5)
                            {
                                MovementManager.MoveTo(objectSpiritHealer.Position);
                                stuckTemps--;
                            }
                        }
                        Interact.InteractGameObject(objectSpiritHealer.GetBaseAddress);
                        Thread.Sleep(2000);
                        Interact.SpiritHealerAccept();
                        Thread.Sleep(1000);
                        if (!ObjectManager.Me.IsDeadMe)
                        {
                            Log.AddLog(Translation.GetText(Translation.Text.Player_retrieve_corpse));
                            Config.Bot.Deaths++;
                            return;
                        }
                    }
                    else
                    {
                        Log.AddLog(Translation.GetText(Translation.Text.No_find_SpiritHealer));
                    }
                }
                else
                {
                    Log.AddLog(Translation.GetText(Translation.Text.No_find_SpiritHealer));
                }

            }
            #endregion SpiritHealer
           
        }
    }
}