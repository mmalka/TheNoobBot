using System.Threading;
using WowManager;
using WowManager.FiniteStateMachine;
using WowManager.WoW.Interface;

namespace Questing_Bot.Bot.States
{
    class Pause : State
    {
        public override string DisplayName
        {
            get { return "Pause"; }
        }

        public override int Priority
        {
            get { return (int)States.Priority.Pause; }
        }

        public override bool NeedToRun
        {
            get
            {
                if (Config.Bot.Pause || Config.Bot.ForcePause)
                    return true;
                return false;
            }
        }

        public override void Run()
        {
            Log.AddLog(Translation.GetText(Translation.Text.Pause_Started));
            while ((Config.Bot.Pause || Config.Bot.ForcePause) && Useful.InGame || !Useful.isLoadingOrConnecting)
            {
                Thread.Sleep(300);
            }
            Log.AddLog(Translation.GetText(Translation.Text.Pause_Stoped));
        }
    }
}
