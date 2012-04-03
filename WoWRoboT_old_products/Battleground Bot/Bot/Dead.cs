using System.Threading;
using WowManager;
using WowManager.WoW.Interface;
using WowManager.WoW.ObjectManager;
using WowManager.WoW.PlayerManager;

namespace Battleground_Bot.Bot
{
    internal static class Dead
    {
        internal static void Go()
        {
            if (ObjectManager.Me.IsDeadMe)
            {
                WowManager.Navigation.MovementManager.StopMove();
                WowManager.Navigation.MovementManager.Stop();
                Log.AddLog(Translation.GetText(Translation.Text.Player_dead));
                Thread.Sleep(1000);
                WowManager.Navigation.MovementManager.StopMove();
                WowManager.Navigation.MovementManager.Stop();
                Thread.Sleep(500);
                Interact.Repop();
                Thread.Sleep(1000);
                while (Config.Bot.BotStarted && Useful.InGame && ObjectManager.Me.IsDeadMe)
                {
                    if (ObjectManager.Me.GetMove)
                        WowManager.Navigation.MovementManager.Stop();

                    Interact.RetrieveCorpse();
                    Thread.Sleep(1000);
                }
                Thread.Sleep(1000);
            }
        }
    }
}