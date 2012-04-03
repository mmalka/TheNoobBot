using System;
using System.Collections.Generic;
using System.Threading;
using Questing_Bot.Bot.Tasks;
using Questing_Bot.Profile;
using WowManager;
using WowManager.FiniteStateMachine;
using WowManager.MiscEnums;
using WowManager.MiscStructs;
using WowManager.Navigation;
using WowManager.Others;
using WowManager.WoW.Interface;
using WowManager.WoW.ObjectManager;
using WowManager.WoW.PlayerManager;
using WowManager.WoW.SpellManager;
using WowManager.WoW.WoWObject;
using Vendor = Questing_Bot.Profile.Vendor;

namespace Questing_Bot.Bot.States
{
    internal class Trainers : State
    {
        public override string DisplayName
        {
            get { return "Trainers"; }
        }

        public override int Priority
        {
            get { return (int) States.Priority.Trainers; }
        }

        private uint _lastLevel;
        public override bool NeedToRun
        {
            get
            {
                if (!Useful.InGame ||
                    Useful.isLoadingOrConnecting || 
                    ObjectManager.Me.IsDeadMe ||
                    !ObjectManager.Me.IsValid ||
                    (ObjectManager.Me.InCombat && !ObjectManager.Me.IsMount) ||
                    !Config.Bot.BotIsActive)
                    return false;

                if (Config.Bot.ForceGoToTrainers)
                    return true;

                // Need Trainers
                if (_lastLevel == 0)
                    _lastLevel = ObjectManager.Me.Level;

                // Herbalism:
                if (Functions.CountTrainerByClass(TrainClass.Herbalism) > 0 && Config.Bot.FormConfig.FarmHerb &&
                    (Skill.GetMaxValue(SkillLine.Herbalism) - Skill.GetValue(SkillLine.Herbalism)) <= 10 &&
                    Skill.GetValue(SkillLine.Herbalism) > 0)
                    return true;
                // Mining:
                if (Functions.CountTrainerByClass(TrainClass.Mining) > 0 && Config.Bot.FormConfig.FarmMine &&
                    (Skill.GetMaxValue(SkillLine.Mining) - Skill.GetValue(SkillLine.Mining)) <= 10 &&
                    Skill.GetValue(SkillLine.Mining) > 0)
                    return true;
                // Spell
                if (ObjectManager.Me.Level >= 3 && _lastLevel != ObjectManager.Me.Level)
                    if (Functions.CountTrainerByClass(
                        (TrainClass)Enum.Parse(typeof(TrainClass), ObjectManager.Me.WowClass.ToString())) > 0 &&
                            SpellManager.SpellAvailable() > 0)
                            return true;

                return false;
            }
        }

        public override void Run()
        {
            Config.Bot.ForceGoToTrainers = false;
            Vendor trainer = null;
            // Herbalism:
            if (Functions.CountTrainerByClass(TrainClass.Herbalism) > 0 && Config.Bot.FormConfig.FarmHerb &&
                (Skill.GetMaxValue(SkillLine.Herbalism) - Skill.GetValue(SkillLine.Herbalism)) <= 10 &&
                Skill.GetValue(SkillLine.Herbalism) > 0)
                trainer = Functions.GetNearestTrainer(TrainClass.Herbalism);
            // Mining:))
            if (Functions.CountTrainerByClass(TrainClass.Mining) > 0 && Config.Bot.FormConfig.FarmMine &&
                (Skill.GetMaxValue(SkillLine.Mining) - Skill.GetValue(SkillLine.Mining)) <= 10 &&
                Skill.GetValue(SkillLine.Mining) > 0 && trainer == null)
                trainer = Functions.GetNearestTrainer(TrainClass.Mining);
            // Spell
            if (
                Functions.CountTrainerByClass(
                    (TrainClass) Enum.Parse(typeof (TrainClass), ObjectManager.Me.WowClass.ToString())) > 0 &&
                SpellManager.SpellAvailable() > 0 && trainer == null)
                trainer =
                    Functions.GetNearestTrainer(
                        (TrainClass) Enum.Parse(typeof (TrainClass), ObjectManager.Me.WowClass.ToString()));

            if (trainer == null)
                return;

            _lastLevel = ObjectManager.Me.Level;

            // Go To Pos Trainer:
            MovementManager.StopMove();
            Log.AddLog(Translation.GetText(Translation.Text.Go_to_Trainer) + " " + trainer.Name + " - " +
                       trainer.TrainClass);
            if (trainer.TrainClass != TrainClass.Herbalism && trainer.TrainClass != TrainClass.Mining)
                Log.AddLog(SpellManager.SpellAvailable() + " " + Translation.GetText(Translation.Text.Spell_Available));
            // Mounting Mount
            MountTask.MountingMount();

            // Find path
            List<Point> points = Functions.GoToPathFind(trainer.Position);

            MovementManager.Go(points);
            int timer = Others.Times + ((int) Point.SizeListPoint(points)/3*1000) + 5000;
            while (MovementManager.InMovement && Config.Bot.BotIsActive && Useful.InGame &&
                   !ObjectManager.Me.InCombat && !ObjectManager.Me.IsDeadMe)
            {
                if (Others.Times > timer)
                    MovementManager.StopMove();
                if (trainer.Position.DistanceTo(ObjectManager.Me.Position) <= 3.8f)
                    MovementManager.StopMove();
                Thread.Sleep(100);
            }

            if (ObjectManager.Me.InCombat)
                return;

            // GoTo trainer:
            List<WoWUnit> tListUnit = ObjectManager.GetWoWUnitTrainer();
            if (tListUnit.Count > 0)
            {
                WoWUnit tTrainer = ObjectManager.GetNearestWoWUnit(tListUnit, trainer.Position);

                if (tTrainer.Position.DistanceTo(ObjectManager.Me.Position) > 4)
                {
                    points = Functions.GoToPathFind(tTrainer.Position);
                    MovementManager.Go(points);
                    timer = Others.Times + ((int) Point.SizeListPoint(points)/3*1000) + 5000;
                    while (MovementManager.InMovement && Config.Bot.BotIsActive && Useful.InGame &&
                           !ObjectManager.Me.InCombat && !ObjectManager.Me.IsDeadMe)
                    {
                        if (tTrainer.Position.DistanceTo(ObjectManager.Me.Position) < 5)
                            MovementManager.StopMove();
                        if (Others.Times > timer)
                            MovementManager.StopMove();
                        Thread.Sleep(100);
                    }
                }
                if (tTrainer.Position.DistanceTo(ObjectManager.Me.Position) < 6)
                {
                    Log.AddLog(Translation.GetText(Translation.Text.Training) + " " + trainer.TrainClass);

                    Interact.InteractGameObject(tTrainer.GetBaseAddress);
                    Thread.Sleep(5000);
                    Interact.InteractGameObject(tTrainer.GetBaseAddress);
                    Trainer.TrainingSpell();
                    Thread.Sleep(5000);
                    // Update spell list
                    if (trainer.TrainClass != TrainClass.Herbalism && trainer.TrainClass != TrainClass.Mining)
                        SpellManager.UpdateSpellBook();
                }
            }
            else
            {
                Log.AddLog(Translation.GetText(Translation.Text.Trainer) + " " + trainer.TrainClass + " " +
                           Translation.GetText(Translation.Text.no_found) + ".");
            }
        }
    }
}