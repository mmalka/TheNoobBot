using System.Threading;
using WowManager;
using WowManager.MiscEnums;
using WowManager.MiscStructs;
using WowManager.WoW.Interface;
using Timer = WowManager.Others.Timer;

namespace Battleground_Bot.Bot
{
    public static class QueueBg
    {
        private static Spell _deserter;
        private static Timer _requeuTimer;
        private const int RequeuTime = 1000*60*4;

        internal static void Go()
        {
            int statPvp = BattleGround.StatBattleGround();
            if (Useful.ContinentId != (int) ContinentId.AV && Useful.ContinentId != (int) ContinentId.AB &&
                Useful.ContinentId != (int) ContinentId.EOTS && Useful.ContinentId != (int) ContinentId.WSG &&
                Useful.ContinentId != (int) ContinentId.SOTA && Useful.ContinentId != (int) ContinentId.IOC &&
                Useful.ContinentId != (int) ContinentId.TP && Useful.ContinentId != (int) ContinentId.BFG)
            {
                if (_deserter == null)
                    _deserter = new Spell(26013);
                if (_deserter.HaveBuff)
                    return;

                switch (statPvp)
                {
                    case 0: //joinQueue
                        Thread.Sleep(100);

                        WowManager.WoW.PlayerManager.Fight.StopFight();
                        WowManager.Navigation.MovementManager.StopMove();

                        if (Config.Bot.RandomBg)
                        {
                            BattleGround.JointBattleGroundQueue(
                                BattleGroundID.Random);
                            Log.AddLog(Translation.GetText(Translation.Text.Join_queue) + " " + Translation.GetText(Translation.Text.Random_BattleGround));
                            Thread.Sleep(500);
                        }
                        else
                        {
                            if (Config.Bot.ArathiBasin)
                            {
                                BattleGround.JointBattleGroundQueue(
                                    BattleGroundID.AB);
                                Log.AddLog(Translation.GetText(Translation.Text.Join_queue) + " Arathi Basin.");
                                Thread.Sleep(500);
                            }
                            if (Config.Bot.WarsongGuich)
                            {
                                BattleGround.JointBattleGroundQueue(
                                    BattleGroundID.WSG);
                                Log.AddLog(Translation.GetText(Translation.Text.Join_queue) + " WarSong Guich.");
                                Thread.Sleep(500);
                            }
                            if (Config.Bot.AlteracValley)
                            {
                                BattleGround.JointBattleGroundQueue(
                                    BattleGroundID.AV);
                                Log.AddLog(Translation.GetText(Translation.Text.Join_queue) + " Alterac Valley.");
                                Thread.Sleep(500);
                            }
                            if (Config.Bot.EyeOfTheStorm)
                            {
                                BattleGround.JointBattleGroundQueue(
                                    BattleGroundID.EOTS);
                                Log.AddLog(Translation.GetText(Translation.Text.Join_queue) + " Eye Of The Storm.");
                                Thread.Sleep(500);
                            }
                            if (Config.Bot.Sota)
                            {
                                BattleGround.JointBattleGroundQueue(
                                    BattleGroundID.SOTA);
                                Log.AddLog(Translation.GetText(Translation.Text.Join_queue) + " Strand of the Ancients.");
                                Thread.Sleep(500);
                            }
                            if (Config.Bot.Ioc)
                            {
                                BattleGround.JointBattleGroundQueue(
                                    BattleGroundID.IOC);
                                Log.AddLog(Translation.GetText(Translation.Text.Join_queue) + " Isle of Conquest.");
                                Thread.Sleep(500);
                            }
                            if (Config.Bot.Tp)
                            {
                                BattleGround.JointBattleGroundQueue(
                                    BattleGroundID.TP);
                                Log.AddLog(Translation.GetText(Translation.Text.Join_queue) + " Twin Peaks.");
                                Thread.Sleep(500);
                            }
                            if (Config.Bot.Bfg)
                            {
                                BattleGround.JointBattleGroundQueue(
                                    BattleGroundID.BFG);
                                Log.AddLog(Translation.GetText(Translation.Text.Join_queue) + " The Battle for Gilneas.");
                                Thread.Sleep(500);
                            }

                            _requeuTimer = new Timer(RequeuTime);
                        }
                        Thread.Sleep(1000);
                        BattleGround.AcceptBattlefield();
                        Thread.Sleep(1000);
                        break;
                    default: //idle
                        if (Config.Bot.Requeue)
                        {
                            if (_requeuTimer == null)
                                _requeuTimer = new Timer(RequeuTime);
                            if (_requeuTimer.isReady)
                            {
                                int i = 2;
                                while (i > 0 && BattleGround.StatBattleGround() != 0)
                                {
                                    Lua.RunMacroText("/click MiniMapBattlefieldFrame /click DropDownList1Button2");
                                    Lua.RunMacroText("/click MiniMapBattlefieldFrame");
                                    Lua.RunMacroText("/click DropDownList1Button2");
                                    i--;
                                }
                                _requeuTimer = new Timer(RequeuTime);
                                Log.AddLog(Translation.GetText(Translation.Text.Requeu));
                            }
                        }
                        Thread.Sleep(100);
                        BattleGround.AcceptBattlefield();
                        Thread.Sleep(3000);
                        break;
                }
                Thread.Sleep(300);
            }
            else // If bg finish exit
            {
                if (BattleGround.IsFinishBattleGround())
                {
                    BattleGround.ExitBattleGround();
                    Log.AddLog(Translation.GetText(Translation.Text.BattleGround_Finish));

                    Thread.Sleep(1000);
                }
            }
        }

        public static void ExitBgIfFinish()
        {
            while (Config.Bot.BotStarted)
            {
                if (BattleGround.IsFinishBattleGround())
                {
                    Thread.Sleep(7000);
                    try
                    {
                        WowManager.WoW.PlayerManager.Fight.StopFight();
                        WowManager.Navigation.MovementManager.StopMove();
                        WowManager.Navigation.MovementManager.Stop();
                    }
                    catch { }
                    BattleGround.ExitBattleGround();
                    Log.AddLog(Translation.GetText(Translation.Text.BattleGround_Finish));

                    Thread.Sleep(1000);
                }
                Thread.Sleep(1000);
            }
        }
    }
}