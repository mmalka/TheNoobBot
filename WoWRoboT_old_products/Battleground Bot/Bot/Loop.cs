using System.IO;
using System.Threading;
using System.Windows.Forms;
using WowManager;
using WowManager.MiscEnums;
using WowManager.WoW.Interface;
using WowManager.WoW.ObjectManager;
using WowManager.WoW.SpellManager;

namespace Battleground_Bot.Bot
{
    internal static class Loop
    {
        public static void Go()
        {
            while (Config.Bot.BotStarted)
            {
                try
                {
                    while (Config.Bot.BotStarted && Config.Bot.ForcePause)
                    {
                        Thread.Sleep(100);
                    }

                    if (Useful.InGame)
                    {
                        // Queue Bg:
                        QueueBg.Go();

                        if (Useful.ContinentId == (int) ContinentId.AV || Useful.ContinentId == (int) ContinentId.AB ||
                            Useful.ContinentId == (int) ContinentId.EOTS || Useful.ContinentId == (int) ContinentId.WSG ||
                            Useful.ContinentId == (int) ContinentId.SOTA || Useful.ContinentId == (int) ContinentId.IOC ||
                            Useful.ContinentId == (int) ContinentId.TP || Useful.ContinentId == (int) ContinentId.BFG)
                        {
                            // CustomClass:
                            if (!CustomClass.IsAliveCustomClass)
                                if (File.Exists(Application.StartupPath + "\\CustomClasses\\" + Config.Bot.CustomClass))
                                    CustomClass.LoadCustomClass(Application.StartupPath + "\\CustomClasses\\" + Config.Bot.CustomClass);

                            // GetProfil:
                            GetProfile.Go();

                            if (ObjectManager.Me.IsDeadMe && Config.Bot.BotStarted)
                            {
                                // Dead
                                Dead.Go();
                            }
                            else if (Fight.GetFight() > 0 && Config.Bot.BotStarted)
                            {
                                Fight.Attack(Fight.GetFight());
                            }
                                // LOOT
                            else if (Loot.GetLoot() > 0 && Config.Bot.BotStarted && Config.Bot.Loot)
                            {
                                Loot.LootUnit(Loot.GetLoot());
                            }
                            else if (Config.Bot.BotStarted)
                            {
                                if (!ObjectManager.Me.IsMount && Config.Bot.UseMount && Config.Bot.BotStarted)
                                {
                                    Mount.MountingMount();
                                }
                                if (BattleGround.BattlegroundIsStarted())
                                {
                                    if (Farm.GetFarm() > 0 && Config.Bot.BotStarted)
                                    {
                                        Farm.FarmNobe(Farm.GetFarm());
                                    }
                                    MovementManager.Go();
                                }
                                else
                                {
                                    WowManager.WoW.PlayerManager.Fight.StopFight();
                                    WowManager.Navigation.MovementManager.StopMove();
                                    Thread.Sleep(200);
                                }
                            }
                        }
                        else
                        {
                            if (!Config.Bot.ByApi)
                            {
                                WowManager.WoW.PlayerManager.Fight.StopFight();
                                WowManager.Navigation.MovementManager.StopMove();
                            }
                        }
                    }
                }
                catch
                {
                }
                Thread.Sleep(100);
            }

            // Dispose:
            WowManager.WoW.PlayerManager.Fight.StopFight();
            WowManager.Navigation.MovementManager.StopMove();
            Config.Bot.BotStarted = false;
            CustomClass.DisposeCustomClass();
            Config.Bot.BotStoped = true;

            Log.AddLog(Translation.GetText(Translation.Text.Bot_stoped));
        }
    }
}