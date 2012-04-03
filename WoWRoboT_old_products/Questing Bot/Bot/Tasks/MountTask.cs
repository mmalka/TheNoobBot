using System.Threading;
using WowManager;
using WowManager.Hardware;
using WowManager.WoW.Interface;
using WowManager.WoW.ObjectManager;
using WowManager.WoW.SpellManager;

namespace Questing_Bot.Bot.Tasks
{
    class MountTask
    {
        private static int _nbTry;

        public static void MountingMount()
        {
            if (!ObjectManager.Me.IsMount && SpellManager.SlotIsEnable(Config.Bot.FormConfig.MountBar + ";" + Config.Bot.FormConfig.MountSlot) && Config.Bot.FormConfig.UseMount)
            {
                WowManager.Navigation.MovementManager.StopMove();
                Log.AddLog(Translation.GetText(Translation.Text.Mounting_mount));

                Thread.Sleep(500);
                Keyboard.PressBarAndSlotKey(Config.Bot.FormConfig.MountBar + ";" + Config.Bot.FormConfig.MountSlot);
                Thread.Sleep(1000);
                Thread.Sleep(Useful.Latency);
                while (ObjectManager.Me.IsCast)
                {
                    Thread.Sleep(50);
                }
                Thread.Sleep(500);

                if (!ObjectManager.Me.IsMount && Config.Bot.BotIsActive)
                {
                    if (_nbTry > 0)
                    {
                        WowManager.Navigation.MovementManager.UnStuck();
                    }
                    _nbTry++;
                }
                else
                {
                    _nbTry = 0;
                }
            }
        }

        public static void DismountMount()
        {
            if (ObjectManager.Me.IsMount && Config.Bot.BotIsActive)
            {
                Log.AddLog(Translation.GetText(Translation.Text.Dismount_mount));
                WowManager.Navigation.MovementManager.StopMove();
                Thread.Sleep(200);
                Useful.DisMount();
                Thread.Sleep(500);
                Thread.Sleep(Useful.Latency);
            }
        }
    }
}
