using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using WowManager;
using WowManager.Navigation;

namespace Battleground_Bot.Bot
{
    public class Bot
    {
        public readonly string CustomClass = "";
        private readonly bool _pathFinder = true;
        public readonly bool AlteracValley;
        public readonly bool ArathiBasin;
        public readonly bool Bfg;
        public readonly bool EyeOfTheStorm;
        public readonly bool Ioc;
        public readonly string KeyMount = "";
        public readonly bool Loot = true;
        public readonly bool RandomBg;
        public readonly bool Requeue;
        public readonly bool Sota;
        public readonly bool Tp;
        public readonly bool UseMount;
        public readonly bool WarsongGuich;
        public readonly bool ByApi = false;
        public List<ulong> BlackList = new List<ulong>();
        public readonly List<ulong> BlackListLoot = new List<ulong>();
        public bool BotStarted;
        public bool BotStoped = true;
        internal bool ForcePause;
        public int IdProfil = -1;
        public int Kills;
        public Profile.Profile Profile = new Profile.Profile();


        public Bot()
        {
        }

        public Bot(bool byApi, bool randomBg, bool warsongGuich, bool eyeOfTheStorm, bool arathiBasin, bool alteracValley, bool sota,
                   bool ioc, bool tp, bool bfg, string customClass, bool pathFinder, bool useMount, string keyMount,
                   bool loot, bool requeue)
        {
            Log.AddLog(Translation.GetText(Translation.Text.Launch_Bot));
            RandomBg = randomBg;
            WarsongGuich = warsongGuich;
            EyeOfTheStorm = eyeOfTheStorm;
            ArathiBasin = arathiBasin;
            AlteracValley = alteracValley;
            Sota = sota;
            Ioc = ioc;
            Tp = tp;
            Bfg = bfg;
            CustomClass = customClass;
            _pathFinder = pathFinder;
            UseMount = useMount;
            KeyMount = keyMount;
            Loot = loot;
            Requeue = requeue;
            ByApi = byApi;
        }

        public void InisializeBotConfig()
        {
            // Active bot
            BotStarted = true;
            BotStoped = false;

            // Launch CC
            if (File.Exists(Application.StartupPath + "\\CustomClasses\\" + CustomClass))
                WowManager.WoW.SpellManager.CustomClass.LoadCustomClass(Application.StartupPath + "\\CustomClasses\\" + CustomClass);
            else
                Log.AddLog(Translation.GetText(Translation.Text.Custom_Class_no_found));

            // Launch Thread Movement
            PathFinderManager.UsePatherFind = _pathFinder;

            // Launch Thread Loop
            Log.AddLog(Translation.GetText(Translation.Text.Launch_Bot));
            var worker2 = new Thread(Loop.Go) {IsBackground = true, Name = "LoopBot"};
            worker2.Start();

            // Launch Thread ExitBgIfFinish
            Log.AddLog(Translation.GetText(Translation.Text.Launch_Bot));
            var worker3 = new Thread(QueueBg.ExitBgIfFinish) {IsBackground = true, Name = "ExitBgIfFinish"};
            worker3.Start();
        }
    }
}