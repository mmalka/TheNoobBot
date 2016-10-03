using System.Collections.Generic;
using Quester.Tasks;
using nManager;
using nManager.FiniteStateMachine;
using nManager.Products;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;
using Quest = nManager.Wow.Helpers.Quest;

namespace Quester.Bot
{
    internal class QuesterState : State
    {
        public override string DisplayName
        {
            get { return "QuesterState"; }
        }

        public override int Priority { get; set; }

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

                return true;
            }
        }

        public override List<State> NextStates
        {
            get { return new List<State>(); }
        }

        public override List<State> BeforeStates
        {
            get { return new List<State>(); }
        }

        public override void Run()
        {
            // Get or set selected quest
            if (QuestingTask.CurrentQuest != null)
            {
                int id = QuestingTask.CurrentQuest.Id;
                if (Quest.GetQuestCompleted(id) || id == 0)
                {
                    QuestingTask.SelectQuest();
                    return;
                }
                if (QuestingTask.CurrentQuest.WorldQuestLocation != null && QuestingTask.CurrentQuest.WorldQuestLocation.IsValid && !Quest.GetLogQuestId().Contains(id))
                {
                    // World Quest that we don't have in our inventory
                    if (!QuestingTask.IsWorldQuestAvailable(id))
                    {
                        // The WorldQuest is no longer available, therefor, it's complete.
                        QuestingTask.SelectQuest();
                        return;
                    }
                }
            }
            else
            {
                QuestingTask.SelectQuest();
                return;
            }

            // Need PickUp or TurnIn
            if (QuestingTask.CurrentQuest.Id != -1 &&
                ((Quest.GetLogQuestIsComplete(QuestingTask.CurrentQuest.Id) && QuestingTask.AllForcedObjectiveComplete()) ||
                 QuestingTask.CurrentQuest.Objectives.Count <= 0) &&
                Quest.GetLogQuestId().Contains(QuestingTask.CurrentQuest.Id) &&
                QuestingTask.CurrentQuest.ScriptConditionIsFinish.Replace(" ", "").Length <= 0) // TurnIn
            {
                QuestingTask.TurnInQuest();
                return;
            }

            if (QuestingTask.CurrentQuest.AutoComplete != null && QuestingTask.CurrentQuest.AutoComplete.Count > 0)
            {
                EventsListener.HookEvent(nManager.Wow.Enums.WoWEventsType.QUEST_AUTOCOMPLETE, callback => Quest.AutoCompleteQuest(QuestingTask.CurrentQuest.AutoComplete), false, true);
            }

            if (QuestingTask.CurrentQuest.Id != -1 && !Quest.GetLogQuestId().Contains(QuestingTask.CurrentQuest.Id))
                // PickUp
            {
                QuestingTask.PickUpQuest();
                return;
            }

            QuestingTask._travelDisabled = false; // reset travel after PickUp worked. (code exclusive for WQ)

            if (Script.Run(QuestingTask.CurrentQuest.ScriptConditionIsFinish) &&
                QuestingTask.CurrentQuest.ScriptConditionIsFinish.Replace(" ", "").Length > 0)
            {
                QuestingTask.TurnInQuest();
                return;
            }

            // Quest Objective
            if (QuestingTask.CurrentQuestObjectiveIsFinish() || QuestingTask.CurrentQuestObjective == null)
            {
                QuestingTask.SelectNextQuestObjective();
                if (QuestingTask.CurrentQuestObjective == null && QuestingTask.CurrentQuest.Objectives.Count > 0)
                    QuestingTask.ResetQuestObjective();
                if (QuestingTask.CurrentQuestObjectiveIsFinish())
                    return;
                if (QuestingTask.CurrentQuestObjective == null)
                    QuestingTask.TurnInQuest();
                else
                    Script.Run(QuestingTask.CurrentQuestObjective.Script);
                return;
            }

            // Execute Objective
            QuestingTask.CurrentQuestObjectiveExecute();
        }
    }
}