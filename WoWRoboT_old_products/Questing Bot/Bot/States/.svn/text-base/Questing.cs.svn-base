using Questing_Bot.Bot.Tasks;
using WowManager.FiniteStateMachine;
using WowManager.WoW.Interface;
using WowManager.WoW.ObjectManager;

namespace Questing_Bot.Bot.States
{
    internal class Questing : State
    {
        public override string DisplayName
        {
            get { return nameState; }
        }

        private string nameState = "Questing";

        public override int Priority
        {
            get { return (int) States.Priority.Questing; }
        }

        public override bool NeedToRun
        {
            get
            {
                if (!Useful.InGame ||
                    Useful.isLoadingOrConnecting ||
                    ObjectManager.Me.IsDeadMe ||
                    !ObjectManager.Me.IsValid ||
                    !Config.Bot.BotIsActive)
                    return false;

                return true;
            }
        }

        public override void Run()
        {
            // Get or set selected quest
            if (Quest.GetQuestsCompleted(Config.Bot.CurrentQuest.Id) || Config.Bot.CurrentQuest.Id == 0 ||
                ObjectManager.Me.Level > Config.Bot.CurrentQuest.MaxLevel)
            {
                QuestingTask.SelectQuest();
                return;
            }
            if (Quest.GetQuestsCompleted(Config.Bot.CurrentQuest.Id) || Config.Bot.CurrentQuest.Id == 0)
                return;

            // Need PickUp or TurnIn
            if (Config.Bot.CurrentQuest.Id != -1 && !Quest.GetLogQuestId().Contains(Config.Bot.CurrentQuest.Id))
                // PickUp
            {
                QuestingTask.PickUpQuest();
                return;
            }
            if (Config.Bot.CurrentQuest.Id != -1 &&
                (Quest.GetLogQuestIsComplete(Config.Bot.CurrentQuest.Id) ||
                 Config.Bot.CurrentQuest.Objectives.Count <= 0) &&
                Quest.GetLogQuestId().Contains(Config.Bot.CurrentQuest.Id) &&
                Config.Bot.CurrentQuest.ScriptConditionIsFinish.Replace(" ", "").Length <= 0) // TurnIn
            {
                QuestingTask.TurnInQuest();
                return;
            }
            if (Script.Run(Config.Bot.CurrentQuest.ScriptConditionIsFinish) &&
                Config.Bot.CurrentQuest.ScriptConditionIsFinish.Replace(" ", "").Length > 0)
            {
                QuestingTask.TurnInQuest();
                return;
            }

            // Quest Objective
            if (QuestingTask.CurrentQuestObjectiveIsFinish() || Config.Bot.CurrentQuestObjective == null)
            {
                QuestingTask.SelectNextQuestObjective();
                if (Config.Bot.CurrentQuestObjective == null && Config.Bot.CurrentQuest.Objectives.Count > 0)
                    QuestingTask.ResetQuestObjective();
                if (QuestingTask.CurrentQuestObjectiveIsFinish())
                    QuestingTask.TurnInQuest();
                else if (Config.Bot.CurrentQuestObjective == null)
                    QuestingTask.TurnInQuest();
                else
                    Script.Run(Config.Bot.CurrentQuestObjective.Script);
                return;
            }

            // Execute Objective
            QuestingTask.CurrentQuestObjectiveExecute();

            //Config.Bot.QuestStat = "";
        }
    }
}