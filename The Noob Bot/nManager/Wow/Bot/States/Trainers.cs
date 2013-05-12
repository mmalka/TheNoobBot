using System.Collections.Generic;
using System.Threading;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Bot.Tasks;
using nManager.Wow.Class;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;

namespace nManager.Wow.Bot.States
{
    public class Trainers : State
    {
        public override string DisplayName
        {
            get { return "Trainers"; }
        }

        public override int Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

        private int _priority;

        public override List<State> NextStates
        {
            get { return new List<State>(); }
        }

        public override List<State> BeforeStates
        {
            get { return new List<State>(); }
        }

        private uint _lastLevel;

        public override bool NeedToRun
        {
            get
            {
                if (!Usefuls.InGame ||
                    Usefuls.IsLoadingOrConnecting ||
                    ObjectManager.ObjectManager.Me.IsDeadMe ||
                    !ObjectManager.ObjectManager.Me.IsValid ||
                    (ObjectManager.ObjectManager.Me.InCombat &&
                     !(ObjectManager.ObjectManager.Me.IsMounted &&
                       (nManagerSetting.CurrentSetting.IgnoreFightIfMounted || Usefuls.IsFlying))) ||
                    !Products.Products.IsStarted)
                    return false;

                // Need Trainers
                if (_lastLevel == 0)
                    _lastLevel = ObjectManager.ObjectManager.Me.Level;

                // Herbalism:
                if (NpcDB.GetNpcNearby(Npc.NpcType.HerbalismTrainer).Entry > 0 &&
                    nManagerSetting.CurrentSetting.ActivateHerbsHarvesting &&
                    nManagerSetting.CurrentSetting.TrainNewSkills &&
                    (Skill.GetMaxValue(SkillLine.Herbalism) - Skill.GetValue(SkillLine.Herbalism)) <= 10 &&
                    Skill.GetValue(SkillLine.Herbalism) > 0)
                    return true;
                // Mining:
                if (NpcDB.GetNpcNearby(Npc.NpcType.MiningTrainer).Entry > 0 &&
                    nManagerSetting.CurrentSetting.ActivateVeinsHarvesting &&
                    nManagerSetting.CurrentSetting.TrainNewSkills &&
                    (Skill.GetMaxValue(SkillLine.Mining) - Skill.GetValue(SkillLine.Mining)) <= 10 &&
                    Skill.GetValue(SkillLine.Mining) > 0)
                    return true;

                return false;
            }
        }

        public override void Run()
        {
            Npc trainer = null;
            // Herbalism:
            if (NpcDB.GetNpcNearby(Npc.NpcType.HerbalismTrainer).Entry > 0 &&
                nManagerSetting.CurrentSetting.ActivateHerbsHarvesting && nManagerSetting.CurrentSetting.TrainNewSkills &&
                (Skill.GetMaxValue(SkillLine.Herbalism) - Skill.GetValue(SkillLine.Herbalism)) <= 10 &&
                Skill.GetValue(SkillLine.Herbalism) > 0)
                trainer = NpcDB.GetNpcNearby(Npc.NpcType.HerbalismTrainer);
            // Mining:))
            if (NpcDB.GetNpcNearby(Npc.NpcType.MiningTrainer).Entry > 0 &&
                nManagerSetting.CurrentSetting.ActivateVeinsHarvesting && nManagerSetting.CurrentSetting.TrainNewSkills &&
                (Skill.GetMaxValue(SkillLine.Mining) - Skill.GetValue(SkillLine.Mining)) <= 10 &&
                Skill.GetValue(SkillLine.Mining) > 0 && trainer == null)
                NpcDB.GetNpcNearby(Npc.NpcType.MiningTrainer);

            if (trainer == null)
                return;

            // Go To Pos Trainer:
            MovementManager.StopMove();
            Logging.Write("Go to trainer");
            // Mounting Mount
            MountTask.Mount();

            // Find path
            var points = new List<Point>();
            if ((trainer.Position.Type.ToLower() == "flying") && nManagerSetting.CurrentSetting.FlyingMountName != "")
            {
                points.Add(new Point(trainer.Position));
            }
            else if ((trainer.Position.Type.ToLower() == "swimming") &&
                     nManagerSetting.CurrentSetting.AquaticMountName != "" && Usefuls.IsSwimming)
            {
                points.Add(trainer.Position);
            }
            else
            {
                points = PathFinder.FindPath(trainer.Position);
            }

            MovementManager.Go(points);
            int timer = Others.Times + ((int) Math.DistanceListPoint(points)/3*1000) + 5000;
            while (MovementManager.InMovement && Products.Products.IsStarted && Usefuls.InGame &&
                   !(ObjectManager.ObjectManager.Me.InCombat &&
                     !(ObjectManager.ObjectManager.Me.IsMounted &&
                       (nManagerSetting.CurrentSetting.IgnoreFightIfMounted || Usefuls.IsFlying))) &&
                   !ObjectManager.ObjectManager.Me.IsDeadMe)
            {
                if (Others.Times > timer)
                    MovementManager.StopMove();
                if (trainer.Position.DistanceTo(ObjectManager.ObjectManager.Me.Position) <= 3.8f)
                    MovementManager.StopMove();
                Thread.Sleep(100);
            }

            if ((ObjectManager.ObjectManager.Me.InCombat &&
                 !(ObjectManager.ObjectManager.Me.IsMounted &&
                   (nManagerSetting.CurrentSetting.IgnoreFightIfMounted || Usefuls.IsFlying))))
                return;

            // GoTo trainer:
            List<WoWUnit> tListUnit = ObjectManager.ObjectManager.GetWoWUnitTrainer();
            if (tListUnit.Count > 0)
            {
                WoWUnit tTrainer = ObjectManager.ObjectManager.GetNearestWoWUnit(tListUnit, trainer.Position);

                if (tTrainer.Position.DistanceTo(ObjectManager.ObjectManager.Me.Position) > 4)
                {
                    points = PathFinder.FindPath(tTrainer.Position);
                    MovementManager.Go(points);
                    timer = Others.Times + ((int) Math.DistanceListPoint(points)/3*1000) + 5000;
                    while (MovementManager.InMovement && Products.Products.IsStarted && Usefuls.InGame &&
                           !(ObjectManager.ObjectManager.Me.InCombat &&
                             !(ObjectManager.ObjectManager.Me.IsMounted &&
                               (nManagerSetting.CurrentSetting.IgnoreFightIfMounted || Usefuls.IsFlying))) &&
                           !ObjectManager.ObjectManager.Me.IsDeadMe)
                    {
                        if (tTrainer.Position.DistanceTo(ObjectManager.ObjectManager.Me.Position) < 5)
                            MovementManager.StopMove();
                        if (Others.Times > timer)
                            MovementManager.StopMove();
                        Thread.Sleep(100);
                    }
                }
                if (tTrainer.Position.DistanceTo(ObjectManager.ObjectManager.Me.Position) < 6)
                {
                    Logging.Write("Training " + trainer.Type);

                    Interact.InteractGameObject(tTrainer.GetBaseAddress);
                    Thread.Sleep(5000);
                    Interact.InteractGameObject(tTrainer.GetBaseAddress);
                    Trainer.TrainingSpell();
                    Thread.Sleep(5000);
                    // Update spell list
                    SpellManager.UpdateSpellBook();
                }
            }
            else
            {
                Logging.Write("Trainer " + trainer.Type + " no found");
            }
        }
    }
}