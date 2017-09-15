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
        private static ulong _whishListSum;
        private static uint _lastPriceAddedToWhishList;
        private static uint _primarySkillsSlotOnWhishList;
        private static uint _lastPrimarySkillsWhishList;

        public static List<KeyValuePair<string, int>> SkillList = new List<KeyValuePair<string, int>>()
        {
            new KeyValuePair<string, int>("Archaeology", Skill.GetValue(SkillLine.Archaeology)),
            new KeyValuePair<string, int>("Fishing", Skill.GetValue(SkillLine.Fishing)),
            new KeyValuePair<string, int>("Mining", Skill.GetValue(SkillLine.Mining)),
            new KeyValuePair<string, int>("Herbalism", Skill.GetValue(SkillLine.Herbalism)),
            new KeyValuePair<string, int>("Skinning", Skill.GetValue(SkillLine.Skinning)),
            new KeyValuePair<string, int>("Riding", Skill.GetValue(SkillLine.Riding)),
            // Addings those skills will request much more work on cost calculcation as they all require to learn new recipes, also, TheNoobBot is not able -yet- to use these product.
            // I have an idea of a ProfessionsManager product, where you just ask "level this skill to 600" and it will do it.
            // A kind of bot can work with a Database of "to do" list to grow 600, like an export of wowprofessions.com. (buy list (from AH or Merchant) then craft list)
            // For the moment, the code stays here, ready to use anytime we need it.
            /*new KeyValuePair<string, int>("Alchemy", Skill.GetValue(SkillLine.Alchemy)),
                new KeyValuePair<string, int>("Cooking", Skill.GetValue(SkillLine.Cooking)),
                new KeyValuePair<string, int>("FirstAid", Skill.GetValue(SkillLine.FirstAid)),
                new KeyValuePair<string, int>("Tailoring", Skill.GetValue(SkillLine.Tailoring)),
                new KeyValuePair<string, int>("Enchanting", Skill.GetValue(SkillLine.Enchanting)),
                new KeyValuePair<string, int>("Engineering", Skill.GetValue(SkillLine.Engineering)),
                new KeyValuePair<string, int>("Inscription", Skill.GetValue(SkillLine.Inscription)),
                new KeyValuePair<string, int>("Blacksmithing", Skill.GetValue(SkillLine.Blacksmithing)),
                new KeyValuePair<string, int>("Jewelcrafting", Skill.GetValue(SkillLine.Jewelcrafting)),
                new KeyValuePair<string, int>("Leatherworking", Skill.GetValue(SkillLine.Leatherworking)),*/
        };

        private static readonly Spell Mining = new Spell("Smelting"); // Mining is not part of the SpellBook
        private static readonly Spell Alchemy = new Spell("Alchemy");
        private static readonly Spell Skinning = new Spell("Skinning");
        private static readonly Spell Herbalism = new Spell("Herb Gathering");
        private static readonly Spell Tailoring = new Spell("Tailoring");
        private static readonly Spell Enchanting = new Spell("Enchanting");
        private static readonly Spell Engineering = new Spell("Engineering");
        private static readonly Spell Inscription = new Spell("Inscription");
        private static readonly Spell Blacksmithing = new Spell("Blacksmithing");
        private static readonly Spell Jewelcrafting = new Spell("Jewelcrafting");
        private static readonly Spell Leatherworking = new Spell("Leatherworking");
        private static readonly List<Npc> TeacherFoundNoSpam = new List<Npc>();
        private List<Npc> _listOfTeachers = new List<Npc>();
        private Npc _teacher = new Npc();
        private bool _doTravel = true;

        public override string DisplayName
        {
            get { return "Learning skills"; }
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
                    Usefuls.IsLoading ||
                    ObjectManager.ObjectManager.Me.IsDeadMe ||
                    !ObjectManager.ObjectManager.Me.IsValid ||
                    ObjectManager.ObjectManager.Me.InInevitableCombat ||
                    !Products.Products.IsStarted)
                    return false;

                // Reset all vars that needs a reset.
                _whishListSum = 0;
                _primarySkillsSlotOnWhishList = 0;
                _listOfTeachers = new List<Npc>();

                foreach (KeyValuePair<string, int> currSkill in SkillList)
                {
                    _lastPriceAddedToWhishList = 0;
                    _lastPrimarySkillsWhishList = 0;
                    _teacher = new Npc();
                    SkillLine skillLine = SkillLine.None;
                    Npc.NpcType trainer = Npc.NpcType.None;
                    bool isPrimarySkill = false;
                    bool isRiding = false;
                    bool doIgnoreCheck = false;
                    switch (currSkill.Key)
                    {
                        case "Archaeology":
                            if (nManagerSetting.CurrentSetting.OnlyTrainCurrentlyUsedSkills && Products.Products.ProductName != "Archaeologist")
                                doIgnoreCheck = true;
                            skillLine = SkillLine.Archaeology;
                            trainer = Npc.NpcType.ArchaeologyTrainer;
                            break;
                        case "Fishing":
                            if (nManagerSetting.CurrentSetting.OnlyTrainCurrentlyUsedSkills && Products.Products.ProductName != "Fisherbot")
                                doIgnoreCheck = true;
                            skillLine = SkillLine.Fishing;
                            trainer = Npc.NpcType.FishingTrainer;
                            break;
                        case "Mining":
                            if (nManagerSetting.CurrentSetting.OnlyTrainCurrentlyUsedSkills &&
                                (!nManagerSetting.CurrentSetting.ActivateVeinsHarvesting ||
                                 nManagerSetting.CurrentSetting.ActivateVeinsHarvesting && Products.Products.ProductName != "Gatherer" && Products.Products.ProductName != "Grinder" &&
                                 Products.Products.ProductName != "Quester"))
                                doIgnoreCheck = true;
                            skillLine = SkillLine.Mining;
                            trainer = Npc.NpcType.MiningTrainer;
                            isPrimarySkill = true;
                            break;
                        case "Herbalism":
                            if (nManagerSetting.CurrentSetting.OnlyTrainCurrentlyUsedSkills &&
                                (!nManagerSetting.CurrentSetting.ActivateHerbsHarvesting ||
                                 nManagerSetting.CurrentSetting.ActivateHerbsHarvesting && Products.Products.ProductName != "Gatherer" && Products.Products.ProductName != "Grinder" &&
                                 Products.Products.ProductName != "Quester"))
                                doIgnoreCheck = true;
                            skillLine = SkillLine.Herbalism;
                            trainer = Npc.NpcType.HerbalismTrainer;
                            isPrimarySkill = true;
                            break;
                        case "Skinning":
                            if (nManagerSetting.CurrentSetting.OnlyTrainCurrentlyUsedSkills &&
                                (!nManagerSetting.CurrentSetting.ActivateBeastSkinning ||
                                 !nManagerSetting.CurrentSetting.ActivateMonsterLooting && !nManagerSetting.CurrentSetting.BeastNinjaSkinning))
                                doIgnoreCheck = true;
                            skillLine = SkillLine.Skinning;
                            trainer = Npc.NpcType.SkinningTrainer;
                            isPrimarySkill = true;
                            break;
                        case "Riding":
                            if (!nManagerSetting.CurrentSetting.TrainMountingCapacity)
                                doIgnoreCheck = true;
                            skillLine = SkillLine.Riding;
                            trainer = Npc.NpcType.RidingTrainer;
                            isRiding = true;
                            break;
                        case "Alchemy":
                            if (nManagerSetting.CurrentSetting.OnlyTrainCurrentlyUsedSkills)
                                doIgnoreCheck = true;
                            skillLine = SkillLine.Alchemy;
                            trainer = Npc.NpcType.AlchemyTrainer;
                            isPrimarySkill = true;
                            break;
                        case "Cooking":
                            if (nManagerSetting.CurrentSetting.OnlyTrainCurrentlyUsedSkills)
                                doIgnoreCheck = true;
                            skillLine = SkillLine.Cooking;
                            trainer = Npc.NpcType.CookingTrainer;
                            break;
                        case "FirstAid":
                            if (nManagerSetting.CurrentSetting.OnlyTrainCurrentlyUsedSkills)
                                doIgnoreCheck = true;
                            skillLine = SkillLine.FirstAid;
                            trainer = Npc.NpcType.FirstAidTrainer;
                            break;
                        case "Tailoring":
                            if (nManagerSetting.CurrentSetting.OnlyTrainCurrentlyUsedSkills)
                                doIgnoreCheck = true;
                            skillLine = SkillLine.Tailoring;
                            trainer = Npc.NpcType.TailoringTrainer;
                            isPrimarySkill = true;
                            break;
                        case "Enchanting":
                            if (nManagerSetting.CurrentSetting.OnlyTrainCurrentlyUsedSkills)
                                doIgnoreCheck = true;
                            skillLine = SkillLine.Enchanting;
                            trainer = Npc.NpcType.EnchantingTrainer;
                            isPrimarySkill = true;
                            break;
                        case "Engineering":
                            if (nManagerSetting.CurrentSetting.OnlyTrainCurrentlyUsedSkills)
                                doIgnoreCheck = true;
                            skillLine = SkillLine.Engineering;
                            trainer = Npc.NpcType.EngineeringTrainer;
                            isPrimarySkill = true;
                            break;
                        case "Inscription":
                            if (nManagerSetting.CurrentSetting.OnlyTrainCurrentlyUsedSkills)
                                doIgnoreCheck = true;
                            skillLine = SkillLine.Inscription;
                            trainer = Npc.NpcType.InscriptionTrainer;
                            isPrimarySkill = true;
                            break;
                        case "Blacksmithing":
                            if (nManagerSetting.CurrentSetting.OnlyTrainCurrentlyUsedSkills)
                                doIgnoreCheck = true;
                            skillLine = SkillLine.Blacksmithing;
                            trainer = Npc.NpcType.BlacksmithingTrainer;
                            isPrimarySkill = true;
                            break;
                        case "Jewelcrafting":
                            if (nManagerSetting.CurrentSetting.OnlyTrainCurrentlyUsedSkills)
                                doIgnoreCheck = true;
                            skillLine = SkillLine.Jewelcrafting;
                            trainer = Npc.NpcType.JewelcraftingTrainer;
                            isPrimarySkill = true;
                            break;
                        case "Leatherworking":
                            if (nManagerSetting.CurrentSetting.OnlyTrainCurrentlyUsedSkills)
                                doIgnoreCheck = true;
                            skillLine = SkillLine.Leatherworking;
                            trainer = Npc.NpcType.LeatherworkingTrainer;
                            isPrimarySkill = true;
                            break;
                        default:
                            continue;
                    }

                    SkillRank skillRank = (SkillRank) Skill.GetMaxValue(skillLine);
                    int value = Skill.GetValue(skillLine);
                    if (currSkill.Key.Contains(skillLine.ToString()) && currSkill.Value != value)
                    {
                        KeyValuePair<string, int> tempSkill = new KeyValuePair<string, int>(currSkill.Key, value);
                        SkillList.Remove(currSkill);
                        SkillList.Add(tempSkill);
                        value += Skill.GetSkillBonus(skillLine);
                        Logging.Write("Your skill in " + skillLine + " has increased to " + value + ".");
                        return false; // We need to get out of the foreach to avoid causing a System.InvalidOperationException.
                    }
                    if (!nManagerSetting.CurrentSetting.ActivateSkillsAutoTraining || doIgnoreCheck)
                        continue;
                    if (!isRiding && nManagerSetting.CurrentSetting.OnlyTrainCurrentlyUsedSkills && IsNewSkillAvailable(value, skillRank, skillLine, true))
                    {
                        _teacher = NpcDB.GetNpcNearby(trainer, ignoreRadiusSettings: true);
                    }
                    else if (IsNewSkillAvailable(value, skillRank, skillLine))
                    {
                        _teacher = NpcDB.GetNpcNearby(trainer);
                    }
                    if (_teacher.Entry > 0)
                    {
                        _teacher.InternalData = (int) skillLine + "," + (int) skillRank;
                        _listOfTeachers.Add(_teacher);
                        TeacherFound(value, skillRank, skillLine, _teacher);
                    }
                    else
                    {
                        if (isPrimarySkill)
                            _primarySkillsSlotOnWhishList = _primarySkillsSlotOnWhishList - _lastPrimarySkillsWhishList;
                        _whishListSum = _whishListSum - _lastPriceAddedToWhishList;
                    }
                }

                return _listOfTeachers.Count > 0;
            }
        }

        public override void Run()
        {
            if (_listOfTeachers.Count <= 0) return;
            Npc bestTeacher = new Npc();
            for (int i = 0; i < _listOfTeachers.Count; i++)
            {
                Npc teacher = _listOfTeachers[i];
                if (bestTeacher.Entry > 0)
                {
                    // priority checks first
                    switch (Products.Products.ProductName)
                    {
                        case "Gatherer":
                            if (bestTeacher.Type == Npc.NpcType.MiningTrainer && teacher.Type != Npc.NpcType.RidingTrainer)
                                continue;
                            if (bestTeacher.Type == Npc.NpcType.HerbalismTrainer && teacher.Type != Npc.NpcType.RidingTrainer && teacher.Type != Npc.NpcType.MiningTrainer)
                                continue;
                            if (bestTeacher.Type == Npc.NpcType.SkinningTrainer && teacher.Type != Npc.NpcType.RidingTrainer && teacher.Type != Npc.NpcType.MiningTrainer &&
                                teacher.Type != Npc.NpcType.HerbalismTrainer)
                                continue;
                            if (teacher.Type == Npc.NpcType.RidingTrainer)
                                bestTeacher = teacher;
                            else if (teacher.Type == Npc.NpcType.MiningTrainer && bestTeacher.Type != Npc.NpcType.RidingTrainer)
                                bestTeacher = teacher;
                            else if (teacher.Type == Npc.NpcType.HerbalismTrainer && bestTeacher.Type != Npc.NpcType.RidingTrainer && bestTeacher.Type != Npc.NpcType.MiningTrainer)
                                bestTeacher = teacher;
                            else if (teacher.Type == Npc.NpcType.SkinningTrainer && bestTeacher.Type != Npc.NpcType.RidingTrainer && bestTeacher.Type != Npc.NpcType.MiningTrainer &&
                                     bestTeacher.Type != Npc.NpcType.HerbalismTrainer)
                                bestTeacher = teacher;
                            break;
                        case "Quester":
                        case "Grinder":
                            if (bestTeacher.Type == Npc.NpcType.SkinningTrainer && teacher.Type != Npc.NpcType.RidingTrainer)
                                continue;
                            if (bestTeacher.Type == Npc.NpcType.MiningTrainer && teacher.Type != Npc.NpcType.RidingTrainer && teacher.Type != Npc.NpcType.SkinningTrainer)
                                continue;
                            if (bestTeacher.Type == Npc.NpcType.HerbalismTrainer && teacher.Type != Npc.NpcType.RidingTrainer && teacher.Type != Npc.NpcType.SkinningTrainer &&
                                teacher.Type != Npc.NpcType.MiningTrainer)
                                continue;
                            if (teacher.Type == Npc.NpcType.RidingTrainer)
                                bestTeacher = teacher;
                            else if (teacher.Type == Npc.NpcType.SkinningTrainer && bestTeacher.Type != Npc.NpcType.RidingTrainer)
                                bestTeacher = teacher;
                            else if (teacher.Type == Npc.NpcType.MiningTrainer && bestTeacher.Type != Npc.NpcType.RidingTrainer && bestTeacher.Type != Npc.NpcType.SkinningTrainer)
                                bestTeacher = teacher;
                            else if (teacher.Type == Npc.NpcType.HerbalismTrainer && bestTeacher.Type != Npc.NpcType.RidingTrainer && bestTeacher.Type != Npc.NpcType.SkinningTrainer &&
                                     bestTeacher.Type != Npc.NpcType.MiningTrainer)
                                bestTeacher = teacher;
                            break;
                    }
                    if (bestTeacher == teacher)
                        continue; // We just set a best from priority checks, so distance check is not important anymore
                    if (ObjectManager.ObjectManager.Me.Position.DistanceTo(teacher.Position) < ObjectManager.ObjectManager.Me.Position.DistanceTo(bestTeacher.Position))
                        bestTeacher = teacher; // We do not have priority between teacher and the actual bestTeacher, so we use distance instead
                }
                else
                    bestTeacher = teacher;
            }

            if (bestTeacher.Position.DistanceTo(ObjectManager.ObjectManager.Me.Position) > 800)
                if (Quest.TravelToQuestZone(bestTeacher.Position, ref _doTravel, bestTeacher.ContinentIdInt, false, bestTeacher.Type.ToString()))
                    return;
            uint baseAddress = MovementManager.FindTarget(ref bestTeacher);
            if (MovementManager.InMovement)
                return;
            if (baseAddress == 0 && bestTeacher.Position.DistanceTo(ObjectManager.ObjectManager.Me.Position) < 10)
                NpcDB.DelNpc(bestTeacher);
            else if (baseAddress > 0)
            {
                if (bestTeacher.Position.DistanceTo(ObjectManager.ObjectManager.Me.Position) > 5f)
                    return;
                string[] skillInfo = bestTeacher.InternalData.Split(',');
                if (skillInfo.Length == 2)
                {
                    SkillLine skillLine = (SkillLine) Others.ToInt32(skillInfo[0]);
                    SkillRank skillRank = (SkillRank) Others.ToInt32(skillInfo[1]);
                    SkillRank nextRank;
                    if (skillRank == SkillRank.ZenMaster)
                    {
                        nextRank = skillRank + 100;
                    }
                    else
                    {
                        nextRank = skillRank + 75;
                    }
                    string oldRank = "";
                    if (skillRank != SkillRank.None)
                        oldRank = " We were only " + skillRank.ToString() + " of " + skillLine.ToString() + ".";
                    Logging.Write("We have just reached the Teacher of " + skillLine.ToString() + ", " + bestTeacher.Name + ". We are now going to learn " + nextRank.ToString() +
                                  " of " + skillLine.ToString() + "." + oldRank);
                }
                Interact.InteractWith(baseAddress);
                Thread.Sleep(500 + Usefuls.Latency);
                Quest.CompleteQuest();
                Gossip.TrainAllAvailableSpells();
                TeacherFoundNoSpam.Remove(bestTeacher);
                SpellManager.UpdateSpellBook();
                _doTravel = true;
            }
            // still on the road, but not in movement for some reasons
        }

        private static void TeacherFound(int value, SkillRank skillRank, SkillLine skillLine, Npc teacher)
        {
            if (TeacherFoundNoSpam.Contains(teacher))
                return;
            TeacherFoundNoSpam.Add(teacher);
            SkillRank nextRank;
            if (skillRank == SkillRank.ZenMaster)
            {
                nextRank = skillRank + 100;
            }
            else
            {
                nextRank = skillRank + 75;
            }
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
                            if (!nManagerSetting.CurrentSetting.BecomeApprenticeIfNeededByProduct || PrimarySkillSlotAvailable() == 0)
                                return false;
                            if ((skillLine == SkillLine.Mining &&
                                 (!nManagerSetting.CurrentSetting.ActivateVeinsHarvesting &&
                                  (Products.Products.ProductName != "Quester" || Products.Products.ProductName != "Gatherer" || Products.Products.ProductName == "Grinder"))) ||
                                (skillLine == SkillLine.Herbalism &&
                                 (!nManagerSetting.CurrentSetting.ActivateHerbsHarvesting &&
                                  (Products.Products.ProductName == "Quester" || Products.Products.ProductName == "Gatherer" || Products.Products.ProductName == "Grinder"))) ||
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
                            if (Products.Products.ProductName != "ProfessionsManager" || PrimarySkillSlotAvailable() == 0)
                                return false;
                            const string professionToLevelup = "Profession"; // Example
                            if (professionToLevelup != skillLine.ToString())
                                return false;
                            primarySkillToLearn = true;
                            price = 10;
                            minLevel = 5;
                            break;
                        case SkillLine.Archaeology:
                            if (!nManagerSetting.CurrentSetting.BecomeApprenticeOfSecondarySkillsWhileQuesting || Products.Products.ProductName != "Quester")
                                if (!nManagerSetting.CurrentSetting.BecomeApprenticeIfNeededByProduct || Products.Products.ProductName != "Archaeologist")
                                    return false;
                            price = 1000;
                            minLevel = 20;
                            break;
                        case SkillLine.Cooking:
                        case SkillLine.FirstAid:
                            if (!nManagerSetting.CurrentSetting.BecomeApprenticeOfSecondarySkillsWhileQuesting || Products.Products.ProductName != "Quester")
                                return false;
                            price = 95;
                            minLevel = 1;
                            break;
                        case SkillLine.Fishing:
                            if (!nManagerSetting.CurrentSetting.BecomeApprenticeOfSecondarySkillsWhileQuesting || Products.Products.ProductName != "Quester")
                                if (!nManagerSetting.CurrentSetting.BecomeApprenticeIfNeededByProduct || Products.Products.ProductName != "Fisherbot")
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
                            return false;
                            /*case SkillLine.Riding:
                            // Todo: Check additionals Riding spells.
                            price = 47500000;
                            minLevel = 70;
                            break;*/
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
                            return false;
                            /*case SkillLine.Riding:
                            // Todo: Check additionals Riding spells.
                            price = 47500000;
                            minLevel = 80;
                            break;*/
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
                case SkillRank.ZenMaster: // To Learn DraenorMaster. Complex.
                case SkillRank.DraenorMaster: // To Learn Legion. Complex.
                case SkillRank.Legion: // Nothing to learn anymore.
                    return false;
                    // Secondary skills should be easier to learn for Zen to Draenor to Legion (Fishing, Archaeology, etc)
            }
            if (nManagerSetting.CurrentSetting.OnlyTrainIfWeHave2TimesMoreMoneyThanOurWishListSum)
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
                else if (primarySkillToLearn)
                    return false;
                _whishListSum = _whishListSum + price;
                _lastPriceAddedToWhishList = price;
                return true;
            }
            return false;
        }
    }
}