namespace nManager.Wow.Patchables
{
    /// <summary>
    /// Descriptors
    /// </summary>
    public static class Descriptors
    {
        public const uint Multiplicator = 4; // 4 or 1
        public static readonly uint StartDescriptors = 0x4;

        public enum ObjectFields
        {
            Guid = 0x0,
            Data = 0x2,
            Type = 0x4,
            Entry = 0x5,
            DynamicFlags = 0x6,
            Scale = 0x7,
            End = 0x8, // CGObjectData_Size
        }

        public enum ItemFields
        {
            Owner = ObjectFields.End + 0,
            ContainedIn = Owner + 2,
            Creator = ContainedIn + 2,
            GiftCreator = Creator + 2,
            StackCount = GiftCreator + 2,
            Expiration = StackCount + 1,
            SpellCharges = Expiration + 1,
            DynamicFlags = SpellCharges + 5,
            Enchantment = DynamicFlags + 1,
            PropertySeed = Enchantment + 39,
            RandomPropertiesID = PropertySeed + 1,
            Durability = RandomPropertiesID + 1,
            MaxDurability = Durability + 1,
            CreatePlayedTime = MaxDurability + 1,
            ModifiersMask = CreatePlayedTime + 1,
            End = ModifiersMask + 1, // CGItemData_Size 
        }

        public enum ContainerFields
        {
            Slots = ItemFields.End + 0,
            NumSlots = Slots + 72,
            End = NumSlots + 1,
        }

        public enum UnitFields
        {
            Charm = ObjectFields.End + 0,
            Summon = Charm + 2,
            Critter = Summon + 2,
            CharmedBy = Critter + 2,
            SummonedBy = CharmedBy + 2,
            CreatedBy = SummonedBy + 2,
            DemonCreator = CreatedBy + 2,
            Target = DemonCreator + 2,
            BattlePetCompanionGUID = Target + 2,
            ChannelObject = BattlePetCompanionGUID + 2,
            ChannelSpell = ChannelObject + 2,
            SummonedByHomeRealm = ChannelSpell + 1,
            Sex = SummonedByHomeRealm + 1,
            DisplayPower = Sex + 1,
            OverrideDisplayPowerID = DisplayPower + 1,
            Health = OverrideDisplayPowerID + 1,
            Power = Health + 1,
            MaxHealth = Power + 5,
            MaxPower = MaxHealth + 1,
            PowerRegenFlatModifier = MaxPower + 5,
            PowerRegenInterruptedFlatModifier = PowerRegenFlatModifier + 5,
            Level = PowerRegenInterruptedFlatModifier + 5,
            EffectiveLevel = Level + 1,
            FactionTemplate = EffectiveLevel + 1,
            VirtualItemID = FactionTemplate + 1,
            Flags = VirtualItemID + 3,
            Flags2 = Flags + 1,
            AuraState = Flags2 + 1,
            AttackRoundBaseTime = AuraState + 1,
            RangedAttackRoundBaseTime = AttackRoundBaseTime + 2,
            BoundingRadius = RangedAttackRoundBaseTime + 1,
            CombatReach = BoundingRadius + 1,
            DisplayID = CombatReach + 1,
            NativeDisplayID = DisplayID + 1,
            MountDisplayID = NativeDisplayID + 1,
            MinDamage = MountDisplayID + 1,
            MaxDamage = MinDamage + 1,
            MinOffHandDamage = MaxDamage + 1,
            MaxOffHandDamage = MinOffHandDamage + 1,
            AnimTier = MaxOffHandDamage + 1,
            PetNumber = AnimTier + 1,
            PetNameTimestamp = PetNumber + 1,
            PetExperience = PetNameTimestamp + 1,
            PetNextLevelExperience = PetExperience + 1,
            ModCastingSpeed = PetNextLevelExperience + 1,
            ModSpellHaste = ModCastingSpeed + 1,
            ModHaste = ModSpellHaste + 1,
            ModRangedHaste = ModHaste + 1,
            ModHasteRegen = ModRangedHaste + 1,
            CreatedBySpell = ModHasteRegen + 1,
            NpcFlag = CreatedBySpell + 1,
            EmoteState = NpcFlag + 2,
            Stats = EmoteState + 1,
            StatPosBuff = Stats + 5,
            StatNegBuff = StatPosBuff + 5,
            Resistances = StatNegBuff + 5,
            ResistanceBuffModsPositive = Resistances + 7,
            ResistanceBuffModsNegative = ResistanceBuffModsPositive + 7,
            BaseMana = ResistanceBuffModsNegative + 7,
            BaseHealth = BaseMana + 1,
            ShapeshiftForm = BaseHealth + 1,
            AttackPower = ShapeshiftForm + 1,
            AttackPowerModPos = AttackPower + 1,
            AttackPowerModNeg = AttackPowerModPos + 1,
            AttackPowerMultiplier = AttackPowerModNeg + 1,
            RangedAttackPower = AttackPowerMultiplier + 1,
            RangedAttackPowerModPos = RangedAttackPower + 1,
            RangedAttackPowerModNeg = RangedAttackPowerModPos + 1,
            RangedAttackPowerMultiplier = RangedAttackPowerModNeg + 1,
            MinRangedDamage = RangedAttackPowerMultiplier + 1,
            MaxRangedDamage = MinRangedDamage + 1,
            PowerCostModifier = MaxRangedDamage + 1,
            PowerCostMultiplier = PowerCostModifier + 7,
            MaxHealthModifier = PowerCostMultiplier + 7,
            HoverHeight = MaxHealthModifier + 1,
            MinItemLevel = HoverHeight + 1,
            MaxItemLevel = MinItemLevel + 1,
            WildBattlePetLevel = MaxItemLevel + 1,
            BattlePetCompanionNameTimestamp = WildBattlePetLevel + 1,
            InteractSpellID = BattlePetCompanionNameTimestamp + 1,
            End = InteractSpellID + 1, // CGUnitData_Size
        }

        public enum PlayerFields
        {
            DuelArbiter = UnitFields.End + 0,
            PlayerFlags = DuelArbiter + 2,
            GuildRankID = PlayerFlags + 1,
            GuildDeleteDate = GuildRankID + 1,
            GuildLevel = GuildDeleteDate + 1,
            HairColorID = GuildLevel + 1,
            RestState = HairColorID + 1,
            ArenaFaction = RestState + 1,
            DuelTeam = ArenaFaction + 1,
            GuildTimeStamp = DuelTeam + 1,
            QuestLog = GuildTimeStamp + 1,
            VisibleItems = QuestLog + 750,
            PlayerTitle = VisibleItems + 38,
            FakeInebriation = PlayerTitle + 1,
            VirtualPlayerRealm = FakeInebriation + 1,
            CurrentSpecID = VirtualPlayerRealm + 1,
            TaxiMountAnimKitID = CurrentSpecID + 1,
            CurrentBattlePetBreedQuality = TaxiMountAnimKitID + 1,
            InvSlots = CurrentBattlePetBreedQuality + 1,
            FarsightObject = InvSlots + 172,
            KnownTitles = FarsightObject + 2,
            Coinage = KnownTitles + 8,
            XP = Coinage + 2,
            NextLevelXP = XP + 1,
            Skill = NextLevelXP + 1,
            CharacterPoints = Skill + 448,
            MaxTalentTiers = CharacterPoints + 1,
            TrackCreatureMask = MaxTalentTiers + 1,
            TrackResourceMask = TrackCreatureMask + 1,
            MainhandExpertise = TrackResourceMask + 1,
            OffhandExpertise = MainhandExpertise + 1,
            RangedExpertise = OffhandExpertise + 1,
            CombatRatingExpertise = OffhandExpertise + 1,
            BlockPercentage = CombatRatingExpertise + 1,
            DodgePercentage = BlockPercentage + 1,
            ParryPercentage = DodgePercentage + 1,
            CritPercentage = ParryPercentage + 1,
            RangedCritPercentage = CritPercentage + 1,
            OffhandCritPercentage = RangedCritPercentage + 1,
            SpellCritPercentage = OffhandCritPercentage + 1,
            ShieldBlock = SpellCritPercentage + 7,
            ShieldBlockCritPercentage = ShieldBlock + 1,
            Mastery = ShieldBlockCritPercentage + 1,
            PvpPowerDamage = Mastery + 1,
            PvpPowerHealing = PvpPowerDamage + 1,
            ExploredZones = PvpPowerHealing + 1,
            RestStateBonusPool = ExploredZones + 1,
            ModDamageDonePos = RestStateBonusPool + 1,
            ModDamageDoneNeg = ModDamageDonePos + 7,
            ModDamageDonePercent = ModDamageDoneNeg + 7,
            ModHealingDonePos = ModDamageDonePercent + 7,
            ModHealingPercent = ModHealingDonePos + 1,
            ModHealingDonePercent = ModHealingPercent + 1,
            ModPeriodicHealingDonePercent = ModHealingDonePercent + 1,
            WeaponDmgMultipliers = ModPeriodicHealingDonePercent + 1,
            ModSpellPowerPercent = WeaponDmgMultipliers + 3,
            ModResiliencePercent = ModSpellPowerPercent + 1,
            OverrideSpellPowerByAPPercent = ModResiliencePercent + 1,
            OverrideAPBySpellPowerPercent = OverrideSpellPowerByAPPercent + 1,
            ModTargetResistance = OverrideAPBySpellPowerPercent + 1,
            ModTargetPhysicalResistance = ModTargetResistance + 1,
            LifetimeMaxRank = ModTargetPhysicalResistance + 1,
            SelfResSpell = LifetimeMaxRank + 1,
            PvpMedals = SelfResSpell + 1,
            BuybackPrice = PvpMedals + 1,
            BuybackTimestamp = BuybackPrice + 12,
            YesterdayHonorableKills = BuybackTimestamp + 12,
            LifetimeHonorableKills = YesterdayHonorableKills + 1,
            WatchedFactionIndex = LifetimeHonorableKills + 1,
            CombatRatings = WatchedFactionIndex + 1,
            PvpInfo = CombatRatings + 108,
            MaxLevel = PvpInfo + 60,
            RuneRegen = MaxLevel + 1,
            NoReagentCostMask = RuneRegen + 4,
            GlyphSlots = NoReagentCostMask + 4,
            Glyphs = GlyphSlots + 6,
            GlyphSlotsEnabled = Glyphs + 6,
            PetSpellPower = GlyphSlotsEnabled + 1,
            Researching = PetSpellPower + 1,
            ProfessionSkillLine = Researching + 8,
            UiHitModifier = ProfessionSkillLine + 2,
            UiSpellHitModifier = UiHitModifier + 1,
            HomeRealmTimeOffset = UiSpellHitModifier + 1,
            ModPetHaste = HomeRealmTimeOffset + 1,
            SummonedBattlePetGUID = ModPetHaste + 1,
            OverrideSpellsID = SummonedBattlePetGUID + 2,
            LfgBonusFactionID = OverrideSpellsID + 1,
            LootSpecID = LfgBonusFactionID + 1,
            OverrideZonePVPType = LootSpecID + 1,
            ItemLevelDelta = OverrideZonePVPType + 1,
            End = ItemLevelDelta + 1, // CGPlayerData_Size
        }

        public enum GameObjectFields
        {
            CreatedBy = ObjectFields.End + 0,
            DisplayID = ObjectFields.End + 2,
            Flags = ObjectFields.End + 3,
            ParentRotation = ObjectFields.End + 4,
            FactionTemplate = ObjectFields.End + 8,
            Level = ObjectFields.End + 9,
            PercentHealth = ObjectFields.End + 10,
            StateSpellVisualID = ObjectFields.End + 11,
            End = ObjectFields.End + 12, // CGGameObjectData_Size
        }

        public enum DynamicObjectFields
        {
            Caster = ObjectFields.End + 0,
            TypeAndVisualID = ObjectFields.End + 2,
            SpellID = ObjectFields.End + 3,
            Radius = ObjectFields.End + 4,
            CastTime = ObjectFields.End + 5,
            End = ObjectFields.End + 6, // CGDynamicObjectData_Size
        }

        public enum CorpseFields
        {
            Owner = ObjectFields.End + 0,
            PartyGUID = ObjectFields.End + 2,
            DisplayID = ObjectFields.End + 4,
            Items = ObjectFields.End + 5,
            SkinID = ObjectFields.End + 24,
            FacialHairStyleID = ObjectFields.End + 25,
            Flags = ObjectFields.End + 26,
            DynamicFlags = ObjectFields.End + 27,
            End = ObjectFields.End + 28,
        }

        public enum AreaTriggerFields
        {
            Caster = ObjectFields.End + 0,
            Duration = ObjectFields.End + 2,
            SpellID = ObjectFields.End + 3,
            SpellVisualID = ObjectFields.End + 4,
            ExplicitScale = ObjectFields.End + 5,
            End = ObjectFields.End + 10,
        }

        public enum SceneObjectFields
        {
            ScriptPackageID = ObjectFields.End + 0,
            RndSeedVal = ObjectFields.End + 1,
            CreatedBy = ObjectFields.End + 2,
            End = ObjectFields.End + 4,
        }

        public enum ItemDynamicFields
        {
            Modifiers = ObjectFields.End + 0,
        }

        public enum UnitDynamicFields
        {
            PassiveSpells,
        }

        public enum PlayerDynamicFields
        {
            ResearchSites,
            DailyQuestsCompleted,
        }
    }
}