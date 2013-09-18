using System.Collections.Generic;
using System.Threading;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;

namespace nManager.Wow.Bot.States
{
    public class Trainers : State
    {
        private static uint _whishListSum;
        private static uint _lastPriceAddedToWhishList;
        private static uint _primarySkillsSlotOnWhishList;
        private static uint _lastPrimarySkillsWhishList;
        public static bool FakeSettingsOnlyTrainCurrentlyUsedSkills = true;
        public static bool FakeSettingsTrainMountingCapacity = true;
        public static bool FakeSettingsOnlyTrainIfWeHave2TimesMoreMoneyThanOurWishListSum = true;
        public static bool FakeSettingsBecomeApprenticeIfNeededByProduct = true;
        public static bool FakeSettingsBecomeApprenticeOfSecondarySkillsWhileQuesting = false;

        private static readonly Spell Mining = new Spell("Mining");
        private static readonly Spell Alchemy = new Spell("Alchemy");
        private static readonly Spell Skinning = new Spell("Skinning");
        private static readonly Spell Herbalism = new Spell("Herbalism");
        private static readonly Spell Tailoring = new Spell("Tailoring");
        private static readonly Spell Enchanting = new Spell("Enchanting");
        private static readonly Spell Engineering = new Spell("Engineering");
        private static readonly Spell Inscription = new Spell("Inscription");
        private static readonly Spell Blacksmithing = new Spell("Blacksmithing");
        private static readonly Spell Jewelcrafting = new Spell("Jewelcrafting");
        private static readonly Spell Leatherworking = new Spell("Leatherworking");
        private static readonly List<Npc> TeacherFoundNoSpam = new List<Npc>();
        private List<Npc> _listOfTeachers = new List<Npc>();
        private bool _rdy = true;

        private Npc _teacherOfAlchemy = new Npc();
        private Npc _teacherOfArchaeology = new Npc();
        private Npc _teacherOfBlacksmithing = new Npc();
        private Npc _teacherOfCooking = new Npc();
        private Npc _teacherOfEnchanting = new Npc();
        private Npc _teacherOfEngineering = new Npc();
        private Npc _teacherOfFirstAid = new Npc();
        private Npc _teacherOfFishing = new Npc();
        private Npc _teacherOfHerbalism = new Npc();
        private Npc _teacherOfInscription = new Npc();
        private Npc _teacherOfJewelcrafting = new Npc();
        private Npc _teacherOfLeatherworking = new Npc();
        private Npc _teacherOfMining = new Npc();
        private Npc _teacherOfRiding = new Npc();
        private Npc _teacherOfSkinning = new Npc();
        private Npc _teacherOfTailoring = new Npc();

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

                // Reset all vars that needs a reset.
                _whishListSum = 0;
                _primarySkillsSlotOnWhishList = 0;
                // Initiate all vars that needs to be iniated.
                SkillRank skillRank;
                int value;
                // We need to reset NPC found after each NeedToRun check
                _teacherOfMining = new Npc();
                _teacherOfRiding = new Npc();
                _teacherOfAlchemy = new Npc();
                _teacherOfCooking = new Npc();
                _teacherOfFishing = new Npc();
                _teacherOfFirstAid = new Npc();
                _teacherOfSkinning = new Npc();
                _teacherOfHerbalism = new Npc();
                _teacherOfTailoring = new Npc();
                _teacherOfEnchanting = new Npc();
                _teacherOfArchaeology = new Npc();
                _teacherOfEngineering = new Npc();
                _teacherOfInscription = new Npc();
                _teacherOfBlacksmithing = new Npc();
                _teacherOfJewelcrafting = new Npc();
                _teacherOfLeatherworking = new Npc();
                _listOfTeachers = new List<Npc>();

                // checks Archaeology
                if (!FakeSettingsOnlyTrainCurrentlyUsedSkills || Products.Products.ProductName == "Archaeologist")
                {
                    skillRank = (SkillRank) Skill.GetMaxValue(SkillLine.Archaeology);
                    value = Skill.GetValue(SkillLine.Archaeology);
                    if (FakeSettingsOnlyTrainCurrentlyUsedSkills && IsNewSkillAvailable(value, skillRank, SkillLine.Archaeology, true))
                    {
                        // If we pass into this if and don't find a Npc with ignoreRadiusSettings true, we wont find one with the next if, 
                        // so we don't need to check 2 times the NpcDb for nothing.
                        _teacherOfArchaeology = NpcDB.GetNpcNearby(Npc.NpcType.ArchaeologyTrainer, ignoreRadiusSettings: true);
                    }
                    else if (IsNewSkillAvailable(value, skillRank, SkillLine.Archaeology))
                    {
                        _teacherOfArchaeology = NpcDB.GetNpcNearby(Npc.NpcType.ArchaeologyTrainer);
                    }
                    if (_teacherOfArchaeology.Entry > 0)
                    {
                        _listOfTeachers.Add(_teacherOfArchaeology);
                        TeacherFound(value, skillRank, SkillLine.Archaeology, _teacherOfArchaeology);
                    }
                    else
                        _whishListSum = _whishListSum - _lastPriceAddedToWhishList;
                    _lastPriceAddedToWhishList = 0;
                }

                // checks Fishing
                if (!FakeSettingsOnlyTrainCurrentlyUsedSkills || Products.Products.ProductName == "Fisherbot")
                {
                    skillRank = (SkillRank) Skill.GetMaxValue(SkillLine.Fishing);
                    value = Skill.GetValue(SkillLine.Fishing);
                    if (FakeSettingsOnlyTrainCurrentlyUsedSkills && IsNewSkillAvailable(value, skillRank, SkillLine.Fishing, true))
                    {
                        _teacherOfFishing = NpcDB.GetNpcNearby(Npc.NpcType.FishingTrainer, ignoreRadiusSettings: true);
                    }
                    else if (IsNewSkillAvailable(value, skillRank, SkillLine.Fishing))
                    {
                        _teacherOfFishing = NpcDB.GetNpcNearby(Npc.NpcType.FishingTrainer);
                    }
                    if (_teacherOfFishing.Entry > 0)
                    {
                        _listOfTeachers.Add(_teacherOfFishing);
                        TeacherFound(value, skillRank, SkillLine.Fishing, _teacherOfFishing);
                    }
                    else
                        _whishListSum = _whishListSum - _lastPriceAddedToWhishList;
                    _lastPriceAddedToWhishList = 0;
                }

                // checks Mining
                if (!FakeSettingsOnlyTrainCurrentlyUsedSkills ||
                    (nManagerSetting.CurrentSetting.ActivateVeinsHarvesting &&
                     (Products.Products.ProductName == "Gatherer" || Products.Products.ProductName == "Grinder" || Products.Products.ProductName == "Quester")))
                {
                    skillRank = (SkillRank) Skill.GetMaxValue(SkillLine.Mining);
                    value = Skill.GetValue(SkillLine.Mining);
                    if (FakeSettingsOnlyTrainCurrentlyUsedSkills && IsNewSkillAvailable(value, skillRank, SkillLine.Mining, true))
                    {
                        _teacherOfMining = NpcDB.GetNpcNearby(Npc.NpcType.MiningTrainer, ignoreRadiusSettings: true);
                    }
                    else if (IsNewSkillAvailable(value, skillRank, SkillLine.Mining))
                    {
                        _teacherOfMining = NpcDB.GetNpcNearby(Npc.NpcType.MiningTrainer);
                    }
                    if (_teacherOfMining.Entry > 0)
                    {
                        _listOfTeachers.Add(_teacherOfMining);
                        TeacherFound(value, skillRank, SkillLine.Mining, _teacherOfMining);
                    }
                    else
                    {
                        _primarySkillsSlotOnWhishList = _primarySkillsSlotOnWhishList - _lastPrimarySkillsWhishList;
                        _whishListSum = _whishListSum - _lastPriceAddedToWhishList;
                    }
                    _lastPriceAddedToWhishList = 0;
                    _lastPrimarySkillsWhishList = 0;
                }

                // checks Herbalism
                if (!FakeSettingsOnlyTrainCurrentlyUsedSkills ||
                    (nManagerSetting.CurrentSetting.ActivateHerbsHarvesting &&
                     (Products.Products.ProductName == "Gatherer" || Products.Products.ProductName == "Grinder" || Products.Products.ProductName == "Quester")))
                {
                    skillRank = (SkillRank) Skill.GetMaxValue(SkillLine.Herbalism);
                    value = Skill.GetValue(SkillLine.Herbalism);
                    if (FakeSettingsOnlyTrainCurrentlyUsedSkills && IsNewSkillAvailable(value, skillRank, SkillLine.Herbalism, true))
                    {
                        _teacherOfHerbalism = NpcDB.GetNpcNearby(Npc.NpcType.HerbalismTrainer, ignoreRadiusSettings: true);
                    }
                    else if (IsNewSkillAvailable(value, skillRank, SkillLine.Herbalism))
                    {
                        _teacherOfHerbalism = NpcDB.GetNpcNearby(Npc.NpcType.HerbalismTrainer);
                    }
                    if (_teacherOfHerbalism.Entry > 0)
                    {
                        _listOfTeachers.Add(_teacherOfHerbalism);
                        TeacherFound(value, skillRank, SkillLine.Herbalism, _teacherOfHerbalism);
                    }
                    else
                    {
                        _primarySkillsSlotOnWhishList = _primarySkillsSlotOnWhishList - _lastPrimarySkillsWhishList;
                        _whishListSum = _whishListSum - _lastPriceAddedToWhishList;
                    }
                    _lastPriceAddedToWhishList = 0;
                    _lastPrimarySkillsWhishList = 0;
                }

                // checks Skinning
                if (!FakeSettingsOnlyTrainCurrentlyUsedSkills ||
                    (nManagerSetting.CurrentSetting.ActivateBeastSkinning && nManagerSetting.CurrentSetting.ActivateMonsterLooting))
                {
                    // This check is special, there is no product check, because all the products that run the States.Trainings can do Skinning.
                    skillRank = (SkillRank) Skill.GetMaxValue(SkillLine.Skinning);
                    value = Skill.GetValue(SkillLine.Skinning);
                    if (FakeSettingsOnlyTrainCurrentlyUsedSkills && IsNewSkillAvailable(value, skillRank, SkillLine.Skinning, true))
                    {
                        _teacherOfSkinning = NpcDB.GetNpcNearby(Npc.NpcType.SkinningTrainer, ignoreRadiusSettings: true);
                    }
                    else if (IsNewSkillAvailable(value, skillRank, SkillLine.Skinning))
                    {
                        _teacherOfSkinning = NpcDB.GetNpcNearby(Npc.NpcType.SkinningTrainer);
                    }
                    if (_teacherOfSkinning.Entry > 0)
                    {
                        _listOfTeachers.Add(_teacherOfSkinning);
                        TeacherFound(value, skillRank, SkillLine.Skinning, _teacherOfSkinning);
                    }
                    else
                    {
                        _primarySkillsSlotOnWhishList = _primarySkillsSlotOnWhishList - _lastPrimarySkillsWhishList;
                        _whishListSum = _whishListSum - _lastPriceAddedToWhishList;
                    }
                    _lastPriceAddedToWhishList = 0;
                    _lastPrimarySkillsWhishList = 0;
                }


                // checks Ridings
                if (FakeSettingsTrainMountingCapacity)
                {
                    // This check is shorter because all the products that run the States.Trainings uses mount.
                    skillRank = (SkillRank) Skill.GetMaxValue(SkillLine.Skinning);
                    value = Skill.GetValue(SkillLine.Skinning);
                    if (IsNewSkillAvailable(value, skillRank, SkillLine.Riding))
                    {
                        _teacherOfRiding = NpcDB.GetNpcNearby(Npc.NpcType.RidingTrainer);
                    }
                    if (_teacherOfRiding.Entry > 0)
                    {
                        _listOfTeachers.Add(_teacherOfRiding);
                        TeacherFound(value, skillRank, SkillLine.Riding, _teacherOfRiding);
                    }
                    else
                        _whishListSum = _whishListSum - _lastPriceAddedToWhishList;
                    _lastPriceAddedToWhishList = 0;
                }

                // if (FakeSettingsOnlyTrainCurrentlyUsedSkills)
                return _listOfTeachers.Count > 0;

                // Addings those skills will request much more work on cost calculcation as they all require to learn new recipes,
                // also, TheNoobBot is not able -yet- to use these product. I have an idea of a Profession bot, where you just ask "level this skill to 600" and it will do it, keeping all the craft, so it will use them in next recipe,
                // and go AH if more recipes are needed. A kind of bot can work with a Database of "to do" list to grow 600, like an export of wowprofessions.com.
                // For the moment, you know the code is here !
                return false;
                /* Skills to add */
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
                // Todo: Copy paste the check below for every skills from the list above.

                // checks Mining
                skillRank = (SkillRank) Skill.GetMaxValue(SkillLine.Mining);
                value = Skill.GetValue(SkillLine.Mining);
                if (IsNewSkillAvailable(value, skillRank, SkillLine.Mining))
                {
                    _teacherOfMining = NpcDB.GetNpcNearby(Npc.NpcType.MiningTrainer);
                }
                if (_teacherOfMining.Entry > 0)
                {
                    _listOfTeachers.Add(_teacherOfMining);
                    TeacherFound(value, skillRank, SkillLine.Mining, _teacherOfMining);
                }
                else
                {
                    _primarySkillsSlotOnWhishList = _primarySkillsSlotOnWhishList - _lastPrimarySkillsWhishList;
                    _whishListSum = _whishListSum - _lastPriceAddedToWhishList;
                }
                _lastPriceAddedToWhishList = 0;
                _lastPrimarySkillsWhishList = 0;

                return _listOfTeachers.Count > 0;
            }
        }

        public override void Run()
        {
            bool needUpdate = false;
            if (_listOfTeachers.Count > 0)
            {
                Npc teacher = _listOfTeachers[0];
                uint baseAddress = MovementManager.FindTarget(ref teacher);
                if (MovementManager.InMovement)
                    return;
                if (baseAddress == 0 && teacher.Position.DistanceTo(ObjectManager.ObjectManager.Me.Position) < 10)
                    NpcDB.DelNpc(teacher);
                else
                {
                    Interact.InteractWith(baseAddress);
                    Thread.Sleep(500);
                    Trainer.TrainingSpell();
                    Thread.Sleep(1000);
                    needUpdate = true;
                    _listOfTeachers.Remove(teacher);
                    TeacherFoundNoSpam.Remove(teacher);
                }
            }
            if (needUpdate)
                SpellManager.UpdateSpellBook();

            // Todo: Calcul the best way to reach all the target in the less distance.
            // Note: Not sure about how works the "InMovement => Return" stuff with a state. Will it ask the bot to continue others tasks .. ?
        }

        private static void TeacherFound(int value, SkillRank skillRank, SkillLine skillLine, Npc teacher)
        {
            if (TeacherFoundNoSpam.Contains(teacher))
                return;
            TeacherFoundNoSpam.Add(teacher);
            SkillRank nextRank = skillRank + 75;
            string current = "You don't know this skill yet";
            if (skillRank > SkillRank.None)
                current = "You are currently " + skillRank + " of " + skillLine.ToString() + ". Level " + value + "/" + (int) skillRank;
            Logging.Write("Teacher of " + skillLine.ToString() + " found. " + current + ". You will become " + nextRank + " of " + skillLine.ToString() + ".");
            Logging.Write("Informations about the teacher of " + skillLine.ToString() + ". Id: " + teacher.Entry + ", Name: " + teacher.Name + ", Distance: " +
                          teacher.Position.DistanceTo(ObjectManager.ObjectManager.Me.Position) + ", Coords: " + teacher.Position);
        }

        private static uint PrimarySkillSlotAvailable()
        {
            uint slots = 2;
            if (Mining.KnownSpell)
                slots = slots - 1;
            if (Alchemy.KnownSpell)
                slots = slots - 1;
            if (slots > 0 && Skinning.KnownSpell)
                slots = slots - 1;
            if (slots > 0 && Herbalism.KnownSpell)
                slots = slots - 1;
            if (slots > 0 && Tailoring.KnownSpell)
                slots = slots - 1;
            if (slots > 0 && Enchanting.KnownSpell)
                slots = slots - 1;
            if (slots > 0 && Engineering.KnownSpell)
                slots = slots - 1;
            if (slots > 0 && Inscription.KnownSpell)
                slots = slots - 1;
            if (slots > 0 && Blacksmithing.KnownSpell)
                slots = slots - 1;
            if (slots > 0 && Jewelcrafting.KnownSpell)
                slots = slots - 1;
            if (slots > 0 && Leatherworking.KnownSpell)
                slots = slots - 1;
            return slots;
        }

        private static bool IsNewSkillAvailable(int value, SkillRank maxValue, SkillLine skillLine, bool hardCheck = false)
        {
            uint price = 0;
            uint minLevel = 0;
            int maxLevelLeftBeforeLearn = 15;
            if (hardCheck)
                maxLevelLeftBeforeLearn = 5;
            bool primarySkillToLearn = false;
            // Note: Price and Levels of skills are hard-coded. But verified ingame with Friendly reputation to Orgrimmar.

            switch (maxValue)
            {
                case SkillRank.None: // To Learn Apprentice
                    switch (skillLine)
                    {
                        case SkillLine.Herbalism:
                        case SkillLine.Mining:
                        case SkillLine.Skinning:
                            if (!FakeSettingsBecomeApprenticeIfNeededByProduct || PrimarySkillSlotAvailable() == 0)
                                return false;
                            if ((skillLine == SkillLine.Mining &&
                                 (!nManagerSetting.CurrentSetting.ActivateVeinsHarvesting &&
                                  (Products.Products.ProductName != "Quester" || Products.Products.ProductName != "Gatherer"))) ||
                                (skillLine == SkillLine.Herbalism &&
                                 (!nManagerSetting.CurrentSetting.ActivateHerbsHarvesting &&
                                  (Products.Products.ProductName == "Quester" || Products.Products.ProductName == "Gatherer"))) ||
                                (skillLine == SkillLine.Skinning &&
                                 (!nManagerSetting.CurrentSetting.ActivateBeastSkinning &&
                                  (Products.Products.ProductName == "Quester" || Products.Products.ProductName == "Grinder"))))
                                return false;
                            primarySkillToLearn = true;
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
                            return false;
                            // We wont learn Primary skills that are not gathering skills.
                            /*if (PrimarySkillSlotAvailable() == 0)
                        return false;
                    primarySkillToLearn = true;
                    price = 10;
                    minLevel = 5;
                    break;*/
                        case SkillLine.Archaeology:
                            if (!FakeSettingsBecomeApprenticeOfSecondarySkillsWhileQuesting || Products.Products.ProductName != "Quester")
                                if (!FakeSettingsBecomeApprenticeIfNeededByProduct || Products.Products.ProductName != "Archaeologist")
                                    return false;
                            price = 1000;
                            minLevel = 20;
                            break;
                        case SkillLine.Cooking:
                        case SkillLine.FirstAid:
                            if (!FakeSettingsBecomeApprenticeOfSecondarySkillsWhileQuesting || Products.Products.ProductName != "Quester")
                                return false;
                            price = 95;
                            minLevel = 1;
                            break;
                        case SkillLine.Fishing:
                            if (!FakeSettingsBecomeApprenticeOfSecondarySkillsWhileQuesting || Products.Products.ProductName != "Quester")
                                if (!FakeSettingsBecomeApprenticeIfNeededByProduct || Products.Products.ProductName != "Fisherbot")
                                    return false;
                            price = 95;
                            minLevel = 5;
                            break;
                        case SkillLine.Riding:
                            price = 38000;
                            minLevel = 20;
                            break;
                    }
                    break;
                case SkillRank.Apprentice: // To Learn Journeyman
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
                case SkillRank.Journeyman: // To Learn Expert
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
                case SkillRank.Expert: // To Learn Artisan
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
                        case SkillLine.Fishing:
                            price = 23750;
                            minLevel = 1;
                            break;
                        case SkillLine.FirstAid:
                            price = 23750;
                            minLevel = 35;
                            break;
                        case SkillLine.Riding:
                            // Todo: Check additionals Riding spells.
                            price = 47500000;
                            minLevel = 70;
                            break;
                    }
                    break;
                case SkillRank.Artisan: // To Learn Master
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
                case SkillRank.Master: // To Learn Grand Master
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
                case SkillRank.GrandMaster: // To Learn Illustrious Grand Master
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
                case SkillRank.IllustriousGrandMaster: // To Learn Zen Master
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
                case SkillRank.ZenMaster: // Nothing to learn.
                    return false;
            }
            if (FakeSettingsOnlyTrainIfWeHave2TimesMoreMoneyThanOurWishListSum)
                price = price*2;
            if (skillLine == SkillLine.Riding && ObjectManager.ObjectManager.Me.Level >= minLevel && (Usefuls.GetMoneyCopper - _whishListSum) >= price)
            {
                _whishListSum = _whishListSum + price;
                _lastPriceAddedToWhishList = price;
                return true;
            }
            if ((int) maxValue - maxLevelLeftBeforeLearn <= value && ObjectManager.ObjectManager.Me.Level >= minLevel && (Usefuls.GetMoneyCopper - _whishListSum) >= price)
            {
                if (primarySkillToLearn && PrimarySkillSlotAvailable() - _primarySkillsSlotOnWhishList >= 1)
                {
                    _primarySkillsSlotOnWhishList = _primarySkillsSlotOnWhishList + 1;
                    _lastPrimarySkillsWhishList = 1;
                }
                else
                    return false;
                _whishListSum = _whishListSum + price;
                _lastPriceAddedToWhishList = price;
                return true;
            }
            return false;
        }
    }
}