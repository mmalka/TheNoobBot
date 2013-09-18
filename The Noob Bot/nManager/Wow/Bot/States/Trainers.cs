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

        public override int Priority { get; set; }

        public override List<State> NextStates
        {
            get { return new List<State>(); }
        }

        public override List<State> BeforeStates
        {
            get { return new List<State>(); }
        }

        private bool _rdy = true;
        public bool FakeSettingsOnlyTrainCurrentlyUsedSkills = false;

        private Npc _teacherOfMining = new Npc();
        private Npc _teacherOfRiding = new Npc();
        private Npc _teacherOfAlchemy = new Npc();
        private Npc _teacherOfCooking = new Npc();
        private Npc _teacherOfFishing = new Npc();
        private Npc _teacherOfFirstAid = new Npc();
        private Npc _teacherOfSkinning = new Npc();
        private Npc _teacherOfHerbalism = new Npc();
        private Npc _teacherOfTailoring = new Npc();
        private Npc _teacherOfEnchanting = new Npc();
        private Npc _teacherOfArchaeology = new Npc();
        private Npc _teacherOfEngineering = new Npc();
        private Npc _teacherOfInscription = new Npc();
        private Npc _teacherOfBlacksmithing = new Npc();
        private Npc _teacherOfJewelcrafting = new Npc();
        private Npc _teacherOfLeatherworking = new Npc();

        private static bool IsNewSkillAvailable(int value, int maxValue, SkillLine skillLine, bool hardCheck = false)
        {
            uint price = 0;
            uint minLevel = 0;
            uint maxLevelLeftBeforeLearn = 15;
            if (hardCheck)
                maxLevelLeftBeforeLearn = 5;
            // Note: Price and Levels of skills are hard-coded. But verified ingame with Friendly reputation to Orgrimmar.

            switch (maxValue)
            {
                case 0: // To Learn Apprentice
                    switch (skillLine)
                    {
                        case SkillLine.Herbalism:
                        case SkillLine.Mining:
                        case SkillLine.Skinning:
                            price = 10;
                            minLevel = 1;
                            break;
                        case SkillLine.Leatherworking:
                        case SkillLine.Alchemy:
                        case SkillLine.Tailoring:
                        case SkillLine.Enchanting:
                        case SkillLine.Engineering:
                        case SkillLine.Inscription:
                        case SkillLine.Blacksmithing:
                        case SkillLine.Jewelcrafting:
                            price = 10;
                            minLevel = 5;
                            break;
                        case SkillLine.Archaeology:
                            price = 1000;
                            minLevel = 20;
                            break;
                        case SkillLine.Cooking:
                        case SkillLine.FirstAid:
                            price = 95;
                            minLevel = 1;
                            break;
                        case SkillLine.Fishing:
                            price = 95;
                            minLevel = 5;
                            break;
                        case SkillLine.Riding:
                            price = 38000;
                            minLevel = 20;
                            break;
                    }
                    break;
                case 75: // To Learn Journeyman
                    switch (skillLine)
                    {
                        case SkillLine.Herbalism:
                        case SkillLine.Mining:
                        case SkillLine.Skinning:
                            price = 475;
                            minLevel = 1;
                            break;
                        case SkillLine.Leatherworking:
                        case SkillLine.Alchemy:
                        case SkillLine.Tailoring:
                        case SkillLine.Enchanting:
                        case SkillLine.Engineering:
                        case SkillLine.Inscription:
                        case SkillLine.Blacksmithing:
                        case SkillLine.Jewelcrafting:
                            price = 475;
                            minLevel = 10;
                            break;
                        case SkillLine.Archaeology:
                            price = 1000;
                            minLevel = 20;
                            break;
                        case SkillLine.Cooking:
                        case SkillLine.FirstAid:
                        case SkillLine.Fishing:
                            price = 475;
                            minLevel = 1;
                            break;
                        case SkillLine.Riding:
                            price = 475000;
                            minLevel = 40;
                            break;
                    }
                    break;
                case 150: // To Learn Expert
                    switch (skillLine)
                    {
                        case SkillLine.Herbalism:
                        case SkillLine.Mining:
                        case SkillLine.Skinning:
                            price = 4750;
                            minLevel = 10;
                            break;
                        case SkillLine.Leatherworking:
                        case SkillLine.Alchemy:
                        case SkillLine.Tailoring:
                        case SkillLine.Enchanting:
                        case SkillLine.Engineering:
                        case SkillLine.Inscription:
                        case SkillLine.Blacksmithing:
                        case SkillLine.Jewelcrafting:
                            price = 4750;
                            minLevel = 20;
                            break;
                        case SkillLine.Archaeology:
                            price = 1000;
                            minLevel = 20;
                            break;
                        case SkillLine.Cooking:
                        case SkillLine.FirstAid:
                        case SkillLine.Fishing:
                            price = 9500;
                            minLevel = 1;
                            break;
                        case SkillLine.Riding:
                            price = 2375000;
                            minLevel = 60;
                            break;
                    }
                    break;
                case 225: // To Learn Artisan
                    switch (skillLine)
                    {
                        case SkillLine.Herbalism:
                        case SkillLine.Mining:
                        case SkillLine.Skinning:
                            price = 47500;
                            minLevel = 25;
                            break;
                        case SkillLine.Leatherworking:
                        case SkillLine.Alchemy:
                        case SkillLine.Tailoring:
                        case SkillLine.Enchanting:
                        case SkillLine.Engineering:
                        case SkillLine.Inscription:
                        case SkillLine.Blacksmithing:
                        case SkillLine.Jewelcrafting:
                            price = 47500;
                            minLevel = 35;
                            break;
                        case SkillLine.Archaeology:
                            price = 25000;
                            minLevel = 35;
                            break;
                        case SkillLine.Cooking:
                            price = 23750;
                            minLevel = 1;
                            break;
                        case SkillLine.FirstAid:
                            price = 23750;
                            minLevel = 35;
                            break;
                        case SkillLine.Fishing:
                            price = 100;
                            minLevel = 10;
                            break;
                        case SkillLine.Riding:
                            // Todo: Check additionals Riding spells.
                            price = 47500000;
                            minLevel = 70;
                            break;
                    }
                    break;
                case 300: // To Learn Master
                    switch (skillLine)
                    {
                        case SkillLine.Herbalism:
                        case SkillLine.Mining:
                        case SkillLine.Skinning:
                            price = 95000;
                            minLevel = 40;
                            break;
                        case SkillLine.Leatherworking:
                        case SkillLine.Alchemy:
                        case SkillLine.Tailoring:
                        case SkillLine.Enchanting:
                        case SkillLine.Engineering:
                        case SkillLine.Inscription:
                        case SkillLine.Blacksmithing:
                        case SkillLine.Jewelcrafting:
                            price = 95000;
                            minLevel = 50;
                            break;
                        case SkillLine.Archaeology:
                            price = 100000;
                            minLevel = 50;
                            break;
                        case SkillLine.Cooking:
                        case SkillLine.Fishing:
                            price = 95000;
                            minLevel = 1;
                            break;
                        case SkillLine.FirstAid:
                            price = 95000;
                            minLevel = 50;
                            break;
                        case SkillLine.Riding:
                            // Todo: Check additionals Riding spells.
                            price = 47500000;
                            minLevel = 80;
                            break;
                    }
                    break;
                case 375: // To Learn Grand Master
                    switch (skillLine)
                    {
                        case SkillLine.Herbalism:
                        case SkillLine.Mining:
                        case SkillLine.Skinning:
                            price = 332500;
                            minLevel = 55;
                            break;
                        case SkillLine.Leatherworking:
                        case SkillLine.Alchemy:
                        case SkillLine.Tailoring:
                        case SkillLine.Enchanting:
                        case SkillLine.Engineering:
                        case SkillLine.Inscription:
                        case SkillLine.Blacksmithing:
                        case SkillLine.Jewelcrafting:
                            price = 332500;
                            minLevel = 65;
                            break;
                        case SkillLine.Archaeology:
                            price = 150000;
                            minLevel = 65;
                            break;
                        case SkillLine.Cooking:
                        case SkillLine.Fishing:
                            price = 332500;
                            minLevel = 1;
                            break;
                        case SkillLine.FirstAid:
                            price = 142500;
                            minLevel = 65;
                            break;
                        case SkillLine.Riding:
                            // Todo: Check additionals Riding spells.
                            return false;
                    }
                    break;
                case 450: // To Learn Illustrious Grand Master
                    switch (skillLine)
                    {
                        case SkillLine.Herbalism:
                        case SkillLine.Mining:
                        case SkillLine.Skinning:
                        case SkillLine.Leatherworking:
                        case SkillLine.Alchemy:
                        case SkillLine.Tailoring:
                        case SkillLine.Enchanting:
                        case SkillLine.Engineering:
                        case SkillLine.Inscription:
                        case SkillLine.Blacksmithing:
                        case SkillLine.Jewelcrafting:
                            price = 475000;
                            minLevel = 75;
                            break;
                        case SkillLine.Archaeology:
                            price = 250000;
                            minLevel = 75;
                            break;
                        case SkillLine.Cooking:
                            price = 475000;
                            minLevel = 1;
                            break;
                        case SkillLine.FirstAid:
                            price = 237500;
                            minLevel = 75;
                            break;
                        case SkillLine.Fishing:
                            return false;
                    }
                    break;
                case 525: // To Learn Zen Master
                    switch (skillLine)
                    {
                        case SkillLine.Herbalism:
                        case SkillLine.Mining:
                        case SkillLine.Skinning:
                        case SkillLine.Leatherworking:
                        case SkillLine.Alchemy:
                        case SkillLine.Tailoring:
                        case SkillLine.Enchanting:
                        case SkillLine.Engineering:
                        case SkillLine.Inscription:
                        case SkillLine.Blacksmithing:
                        case SkillLine.Jewelcrafting:
                            price = 570000;
                            minLevel = 80;
                            break;
                        case SkillLine.Archaeology:
                            price = 600000;
                            minLevel = 80;
                            break;
                        case SkillLine.Cooking:
                            price = 570000;
                            minLevel = 1;
                            break;
                        case SkillLine.FirstAid:
                            price = 285000;
                            minLevel = 80;
                            break;
                    }
                    break;
                case 600: // Nothing to learn.
                    return false;
            }
            return maxValue - maxLevelLeftBeforeLearn <= value && ObjectManager.ObjectManager.Me.Level >= minLevel && Usefuls.GetMoneyCopper >= price;
        }

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

                if (nManagerSetting.CurrentSetting.TrainNewSkills && _rdy)
                {
                    Logging.Write("You have activated Train New Skills feature, but it's not yet supported, will be added back soon.");
                    _rdy = false;
                }
                return false;

                //TODO: Please keep the function as is, it's still a work-in-progress, you can talk to me about it on the forum, but I have ideas about what to optimize/changes, etc yet, so better wait a pre-final version.

                // Real code start here:
                if (!nManagerSetting.CurrentSetting.TrainNewSkills)
                    return false;

                int maxValue; // Will be erased at each check.
                int value; // Will be erased at each check.

                // checks Archaeology
                if (!FakeSettingsOnlyTrainCurrentlyUsedSkills || Products.Products.ProductName == "Archaeologist")
                {
                    maxValue = Skill.GetMaxValue(SkillLine.Archaeology);
                    value = Skill.GetValue(SkillLine.Archaeology);
                    if (FakeSettingsOnlyTrainCurrentlyUsedSkills && IsNewSkillAvailable(value, maxValue, SkillLine.Archaeology, true))
                    {
                        // If we pass into this if and don't find a Npc with ignoreRadiusSettings true, we wont find one with the next if, 
                        // so we don't need to check 2 times the NpcDb for nothing.
                        _teacherOfArchaeology = NpcDB.GetNpcNearby(Npc.NpcType.ArchaeologyTrainer, ignoreRadiusSettings: true);
                    }
                    else if (IsNewSkillAvailable(value, maxValue, SkillLine.Archaeology))
                    {
                        _teacherOfArchaeology = NpcDB.GetNpcNearby(Npc.NpcType.ArchaeologyTrainer);
                    }
                }

                // checks Fishing
                if (!FakeSettingsOnlyTrainCurrentlyUsedSkills || Products.Products.ProductName == "Fisherbot")
                {
                    maxValue = Skill.GetMaxValue(SkillLine.Fishing);
                    value = Skill.GetValue(SkillLine.Fishing);
                    if (FakeSettingsOnlyTrainCurrentlyUsedSkills && IsNewSkillAvailable(value, maxValue, SkillLine.Fishing, true))
                    {
                        _teacherOfFishing = NpcDB.GetNpcNearby(Npc.NpcType.FishingTrainer, ignoreRadiusSettings: true);
                    }
                    else if (IsNewSkillAvailable(value, maxValue, SkillLine.Fishing))
                    {
                        _teacherOfFishing = NpcDB.GetNpcNearby(Npc.NpcType.FishingTrainer);
                    }
                }

                // checks Herbalism
                if (!FakeSettingsOnlyTrainCurrentlyUsedSkills ||
                    (nManagerSetting.CurrentSetting.ActivateHerbsHarvesting && (Products.Products.ProductName == "Gatherer" || Products.Products.ProductName == "Grinder")))
                {
                    maxValue = Skill.GetMaxValue(SkillLine.Herbalism);
                    value = Skill.GetValue(SkillLine.Herbalism);
                    if (FakeSettingsOnlyTrainCurrentlyUsedSkills && IsNewSkillAvailable(value, maxValue, SkillLine.Herbalism, true))
                    {
                        _teacherOfHerbalism = NpcDB.GetNpcNearby(Npc.NpcType.HerbalismTrainer, ignoreRadiusSettings: true);
                    }
                    else if (IsNewSkillAvailable(value, maxValue, SkillLine.Herbalism))
                    {
                        _teacherOfHerbalism = NpcDB.GetNpcNearby(Npc.NpcType.HerbalismTrainer);
                    }
                }

                // checks Mining
                if (!FakeSettingsOnlyTrainCurrentlyUsedSkills ||
                    (nManagerSetting.CurrentSetting.ActivateVeinsHarvesting && (Products.Products.ProductName == "Gatherer" || Products.Products.ProductName == "Grinder")))
                {
                    maxValue = Skill.GetMaxValue(SkillLine.Mining);
                    value = Skill.GetValue(SkillLine.Mining);
                    if (FakeSettingsOnlyTrainCurrentlyUsedSkills && IsNewSkillAvailable(value, maxValue, SkillLine.Mining, true))
                    {
                        _teacherOfMining = NpcDB.GetNpcNearby(Npc.NpcType.MiningTrainer, ignoreRadiusSettings: true);
                    }
                    else if (IsNewSkillAvailable(value, maxValue, SkillLine.Mining))
                    {
                        _teacherOfMining = NpcDB.GetNpcNearby(Npc.NpcType.MiningTrainer);
                    }
                }

                // checks Skinning
                if (!FakeSettingsOnlyTrainCurrentlyUsedSkills ||
                    (nManagerSetting.CurrentSetting.ActivateBeastSkinning && nManagerSetting.CurrentSetting.ActivateMonsterLooting))
                {
                    // This check is special, there is no product check, because we can do skinning in ALL products as long as we are looting monsters.
                    // Note: Product like DamageDealer (ie. Product not intended to be AFKable: DD, Heal, Track) does not load the TrainNewSkill class at all.
                    maxValue = Skill.GetMaxValue(SkillLine.Skinning);
                    value = Skill.GetValue(SkillLine.Skinning);
                    if (FakeSettingsOnlyTrainCurrentlyUsedSkills && IsNewSkillAvailable(value, maxValue, SkillLine.Skinning, true))
                    {
                        _teacherOfSkinning = NpcDB.GetNpcNearby(Npc.NpcType.SkinningTrainer, ignoreRadiusSettings: true);
                    }
                    else if (IsNewSkillAvailable(value, maxValue, SkillLine.Skinning))
                    {
                        _teacherOfSkinning = NpcDB.GetNpcNearby(Npc.NpcType.SkinningTrainer);
                    }
                }

                if (FakeSettingsOnlyTrainCurrentlyUsedSkills)
                    return _teacherOfArchaeology.Entry != 0 || _teacherOfFishing.Entry != 0 || _teacherOfHerbalism.Entry != 0 || _teacherOfMining.Entry != 0 ||
                           _teacherOfSkinning.Entry != 0;

                /* Skills to add */
                _teacherOfRiding = new Npc();
                _teacherOfAlchemy = new Npc();
                _teacherOfCooking = new Npc();
                _teacherOfFirstAid = new Npc();
                _teacherOfTailoring = new Npc();
                _teacherOfEnchanting = new Npc();
                _teacherOfEngineering = new Npc();
                _teacherOfInscription = new Npc();
                _teacherOfBlacksmithing = new Npc();
                _teacherOfJewelcrafting = new Npc();
                _teacherOfLeatherworking = new Npc();

                /* Model */
                // checks Mining
                maxValue = Skill.GetMaxValue(SkillLine.Mining);
                value = Skill.GetValue(SkillLine.Mining);
                if (IsNewSkillAvailable(value, maxValue, SkillLine.Mining))
                {
                    _teacherOfMining = NpcDB.GetNpcNearby(Npc.NpcType.MiningTrainer);
                }

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
            // Mining:
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
            List<Point> points = new List<Point>();
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

                    Interact.InteractWith(tTrainer.GetBaseAddress);
                    Thread.Sleep(5000);
                    Interact.InteractWith(tTrainer.GetBaseAddress);
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