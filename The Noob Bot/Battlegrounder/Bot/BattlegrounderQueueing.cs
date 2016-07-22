using System.Collections.Generic;
using System.Threading;
using nManager;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Products;
using nManager.Wow.Class;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;
using Timer = nManager.Helpful.Timer;

namespace Battlegrounder.Bot
{
    public class BattlegrounderQueueing : State
    {
        private static Spell _deserter;
        private static Timer _requeuTimer;
        private readonly int _requeueingTime = 1000*60*BattlegrounderSetting.CurrentSetting.RequeueAfterXMinutesTimer;

        public override string DisplayName
        {
            get { return "BattlegrounderQueueing"; }
        }

        public override int Priority { get; set; }

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
                    Usefuls.IsLoading ||
                    ObjectManager.Me.IsDeadMe ||
                    !ObjectManager.Me.IsValid ||
                    (ObjectManager.Me.InCombat &&
                     !(ObjectManager.Me.IsMounted &&
                       (nManagerSetting.CurrentSetting.IgnoreFightIfMounted || Usefuls.IsFlying))) ||
                    !Products.IsStarted)
                    return false;

                if (!Usefuls.IsInBattleground || Battleground.IsFinishBattleground())
                    return true;

                return false;
            }
        }

        public override void Run()
        {
            int statPvp = Battleground.QueueingStatus();
            if (!Usefuls.IsInBattleground)
            {
                if (_deserter == null)
                    _deserter = new Spell(26013);
                if (_deserter.HaveBuff)
                    return;

                switch (statPvp)
                {
                    case 0:
                        Thread.Sleep(100);
                        Fight.StopFight();
                        MovementManager.StopMove();
                        if (BattlegrounderSetting.CurrentSetting.RandomBattleground)
                        {
                            Battleground.JoinBattlegroundQueue(
                                BattlegroundId.RandomBattleground);
                            Logging.Write(Translate.Get(Translate.Id.JoinQueue) + " " +
                                          Translate.Get(Translate.Id.RandomBattleground));
                            Thread.Sleep(500);
                        }
                        else
                        {
                            if (BattlegrounderSetting.CurrentSetting.AlteracValley)
                            {
                                Battleground.JoinBattlegroundQueue(
                                    BattlegroundId.AlteracValley);
                                Logging.Write(Translate.Get(Translate.Id.JoinQueue) + " " +
                                              Translate.Get(Translate.Id.AlteracValley) + ".");
                                Thread.Sleep(500);
                            }
                            if (BattlegrounderSetting.CurrentSetting.WarsongGulch)
                            {
                                Battleground.JoinBattlegroundQueue(
                                    BattlegroundId.WarsongGulch);
                                Logging.Write(Translate.Get(Translate.Id.JoinQueue) + " " +
                                              Translate.Get(Translate.Id.WarsongGulch) + ".");
                                Thread.Sleep(500);
                            }
                            if (BattlegrounderSetting.CurrentSetting.ArathiBasin)
                            {
                                Battleground.JoinBattlegroundQueue(
                                    BattlegroundId.ArathiBasin);
                                Logging.Write(Translate.Get(Translate.Id.JoinQueue) + " " +
                                              Translate.Get(Translate.Id.ArathiBasin) + ".");
                                Thread.Sleep(500);
                            }
                            if (BattlegrounderSetting.CurrentSetting.EyeoftheStorm)
                            {
                                Battleground.JoinBattlegroundQueue(
                                    BattlegroundId.EyeoftheStorm);
                                Logging.Write(Translate.Get(Translate.Id.JoinQueue) + " " +
                                              Translate.Get(Translate.Id.EyeoftheStorm) + ".");
                                Thread.Sleep(500);
                            }
                            if (BattlegrounderSetting.CurrentSetting.StrandoftheAncients)
                            {
                                Battleground.JoinBattlegroundQueue(
                                    BattlegroundId.StrandoftheAncients);
                                Logging.Write(Translate.Get(Translate.Id.JoinQueue) + " " +
                                              Translate.Get(Translate.Id.StrandoftheAncients) + ".");
                                Thread.Sleep(500);
                            }
                            if (BattlegrounderSetting.CurrentSetting.IsleofConquest)
                            {
                                Battleground.JoinBattlegroundQueue(
                                    BattlegroundId.IsleofConquest);
                                Logging.Write(Translate.Get(Translate.Id.JoinQueue) + " " +
                                              Translate.Get(Translate.Id.IsleofConquest) + ".");
                                Thread.Sleep(500);
                            }
                            if (BattlegrounderSetting.CurrentSetting.TwinPeaks)
                            {
                                Battleground.JoinBattlegroundQueue(
                                    BattlegroundId.TwinPeaks);
                                Logging.Write(Translate.Get(Translate.Id.JoinQueue) + " " +
                                              Translate.Get(Translate.Id.TwinPeaks) + ".");
                                Thread.Sleep(500);
                            }
                            if (BattlegrounderSetting.CurrentSetting.BattleforGilneas)
                            {
                                Battleground.JoinBattlegroundQueue(
                                    BattlegroundId.BattleforGilneas);
                                Logging.Write(Translate.Get(Translate.Id.JoinQueue) + " " +
                                              Translate.Get(Translate.Id.BattleforGilneas) + ".");
                                Thread.Sleep(500);
                            }
                            if (BattlegrounderSetting.CurrentSetting.TempleofKotmogu)
                            {
                                Battleground.JoinBattlegroundQueue(
                                    BattlegroundId.TempleofKotmogu);
                                Logging.Write(Translate.Get(Translate.Id.JoinQueue) + " " +
                                              Translate.Get(Translate.Id.TempleofKotmogu) + ".");
                                Thread.Sleep(500);
                            }
                            if (BattlegrounderSetting.CurrentSetting.SilvershardMines)
                            {
                                Battleground.JoinBattlegroundQueue(
                                    BattlegroundId.SilvershardMines);
                                Logging.Write(Translate.Get(Translate.Id.JoinQueue) + " " +
                                              Translate.Get(Translate.Id.SilvershardMines) + ".");
                                Thread.Sleep(500);
                            }
                            _requeuTimer = new Timer(_requeueingTime);
                        }
                        Thread.Sleep(1000);
                        Battleground.AcceptBattlefieldPortAll();
                        Thread.Sleep(1000);
                        break;
                    default:
                        if (BattlegrounderSetting.CurrentSetting.RequeueAfterXMinutes)
                        {
                            if (_requeuTimer == null)
                                _requeuTimer = new Timer(_requeueingTime);
                            if (_requeuTimer.IsReady)
                            {
                                int i = 2;
                                while (i > 0 && Battleground.QueueingStatus() != 0)
                                {
                                    Lua.RunMacroText("/click MiniMapBattlefieldFrame /click DropDownList1Button2");
                                    Lua.RunMacroText("/click MiniMapBattlefieldFrame");
                                    Lua.RunMacroText("/click DropDownList1Button2");
                                    i--;
                                }
                                _requeuTimer = new Timer(_requeueingTime);
                                Logging.Write(Translate.Get(Translate.Id.RequeueingInProcess));
                            }
                        }
                        Thread.Sleep(100);
                        Battleground.AcceptBattlefieldPortAll();
                        Thread.Sleep(3000);
                        break;
                }
                Thread.Sleep(300);
            }
            else
            {
                if (Battleground.IsFinishBattleground())
                {
                    Battleground.ExitBattleground();
                    Logging.Write(Translate.Get(Translate.Id.Battleground_Ended));
                    Thread.Sleep(1000);
                }
            }
        }
    }
}