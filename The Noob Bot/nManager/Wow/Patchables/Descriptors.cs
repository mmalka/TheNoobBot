namespace nManager.Wow.Patchables
{
    /// <summary>
    /// Descriptors
    /// </summary>
    public static class Descriptors
    {
        public const uint Multiplicator = 4; // 4 or 1
        public static readonly uint StartDescriptors = 0x8;

        public enum ObjectFields
        {
            Guid = 0x0,
            Data = 0x4,
            Type = 0x8,
            EntryID = 0x9,
            DynamicFlags = 0xA,
            Scale = 0xB,
        };

        public enum ItemFields
        {
            Owner = 0xC,
            ContainedIn = 0x10,
            Creator = 0x14,
            GiftCreator = 0x18,
            StackCount = 0x1C,
            Expiration = 0x1D,
            SpellCharges = 0x1E,
            DynamicFlags = 0x23,
            Enchantment = 0x24,
            PropertySeed = 0x4B,
            RandomPropertiesID = 0x4C,
            Durability = 0x4D,
            MaxDurability = 0x4E,
            CreatePlayedTime = 0x4F,
            ModifiersMask = 0x50,
            Context = 0x51,
            ArtifactXP = 0x52,
            ItemAppearanceModID = 0x54,
        };

        public enum ContainerFields
        {
            Slots = 0x55,
            NumSlots = 0xE5,
        };

        public enum UnitFields
        {
            Charm = 0xC,
            Summon = 0x10,
            Critter = 0x14,
            CharmedBy = 0x18,
            SummonedBy = 0x1C,
            CreatedBy = 0x20,
            DemonCreator = 0x24,
            Target = 0x28,
            BattlePetCompanionGUID = 0x2C,
            BattlePetDBID = 0x30,
            ChannelSpell = 0x32,
            ChannelSpellXSpellVisual = 0x33,
            SummonedByHomeRealm = 0x34,
            Sex = 0x35,
            DisplayPower = 0x36,
            OverrideDisplayPowerID = 0x37,
            Health = 0x38,
            Power = 0x3A,
            MaxHealth = 0x40,
            MaxPower = 0x42,
            PowerRegenFlatModifier = 0x48,
            PowerRegenInterruptedFlatModifier = 0x4E,
            Level = 0x54,
            EffectiveLevel = 0x55,
            ScalingLevelMin = 0x56,
            ScalingLevelMax = 0x57,
            ScalingLevelDelta = 0x58,
            FactionTemplate = 0x59,
            VirtualItems = 0x5A,
            Flags = 0x60,
            Flags2 = 0x61,
            Flags3 = 0x62,
            AuraState = 0x63,
            AttackRoundBaseTime = 0x64,
            RangedAttackRoundBaseTime = 0x66,
            BoundingRadius = 0x67,
            CombatReach = 0x68,
            DisplayID = 0x69,
            NativeDisplayID = 0x6A,
            MountDisplayID = 0x6B,
            MinDamage = 0x6C,
            MaxDamage = 0x6D,
            MinOffHandDamage = 0x6E,
            MaxOffHandDamage = 0x6F,
            AnimTier = 0x70,
            PetNumber = 0x71,
            PetNameTimestamp = 0x72,
            PetExperience = 0x73,
            PetNextLevelExperience = 0x74,
            ModCastingSpeed = 0x75,
            ModSpellHaste = 0x76,
            ModHaste = 0x77,
            ModRangedHaste = 0x78,
            ModHasteRegen = 0x79,
            ModTimeRate = 0x7A,
            CreatedBySpell = 0x7B,
            NpcFlags = 0x7C,
            EmoteState = 0x7E,
            Stats = 0x7F,
            StatPosBuff = 0x83,
            StatNegBuff = 0x87,
            Resistances = 0x8B,
            ResistanceBuffModsPositive = 0x92,
            ResistanceBuffModsNegative = 0x99,
            ModBonusArmor = 0xA0,
            BaseMana = 0xA1,
            BaseHealth = 0xA2,
            ShapeshiftForm = 0xA3,
            AttackPower = 0xA4,
            AttackPowerModPos = 0xA5,
            AttackPowerModNeg = 0xA6,
            AttackPowerMultiplier = 0xA7,
            RangedAttackPower = 0xA8,
            RangedAttackPowerModPos = 0xA9,
            RangedAttackPowerModNeg = 0xAA,
            RangedAttackPowerMultiplier = 0xAB,
            SetAttackSpeedAura = 0xAC,
            MinRangedDamage = 0xAD,
            MaxRangedDamage = 0xAE,
            PowerCostModifier = 0xAF,
            PowerCostMultiplier = 0xB6,
            MaxHealthModifier = 0xBD,
            HoverHeight = 0xBE,
            MinItemLevelCutoff = 0xBF,
            MinItemLevel = 0xC0,
            MaxItemLevel = 0xC1,
            WildBattlePetLevel = 0xC2,
            BattlePetCompanionNameTimestamp = 0xC3,
            InteractSpellID = 0xC4,
            StateSpellVisualID = 0xC5,
            StateAnimID = 0xC6,
            StateAnimKitID = 0xC7,
            StateWorldEffectID = 0xC8,
            ScaleDuration = 0xCC,
            LooksLikeMountID = 0xCD,
            LooksLikeCreatureID = 0xCE,
            LookAtControllerID = 0xCF,
            LookAtControllerTarget = 0xD0,
        };

        public enum PlayerFields
        {
            DuelArbiter = 0xD4,
            WowAccount = 0xD8,
            LootTargetGUID = 0xDC,
            PlayerFlags = 0xE0,
            PlayerFlagsEx = 0xE1,
            GuildRankID = 0xE2,
            GuildDeleteDate = 0xE3,
            GuildLevel = 0xE4,
            HairColorID = 0xE5,
            CustomDisplayOption = 0xE6,
            Inebriation = 0xE7,
            ArenaFaction = 0xE8,
            DuelTeam = 0xE9,
            GuildTimeStamp = 0xEA,
            QuestLog = 0xEB,
            VisibleItems = 0x40B,
            PlayerTitle = 0x431,
            FakeInebriation = 0x432,
            VirtualPlayerRealm = 0x433,
            CurrentSpecID = 0x434,
            TaxiMountAnimKitID = 0x435,
            AvgItemLevel = 0x436,
            CurrentBattlePetBreedQuality = 0x43A,
            Prestige = 0x43B,
            HonorLevel = 0x43C,
            InvSlots = 0x43D,
            FarsightObject = 0x729,
            SummonedBattlePetGUID = 0x72D,
            KnownTitles = 0x731,
            Coinage = 0x73D,
            XP = 0x73F,
            NextLevelXP = 0x740,
            Skill = 0x741,
            CharacterPoints = 0x901,
            MaxTalentTiers = 0x902,
            TrackCreatureMask = 0x903,
            TrackResourceMask = 0x904,
            MainhandExpertise = 0x905,
            OffhandExpertise = 0x906,
            RangedExpertise = 0x907,
            CombatRatingExpertise = 0x908,
            BlockPercentage = 0x909,
            DodgePercentage = 0x90A,
            DodgePercentageFromAttribute = 0x90B,
            ParryPercentage = 0x90C,
            ParryPercentageFromAttribute = 0x90D,
            CritPercentage = 0x90E,
            RangedCritPercentage = 0x90F,
            OffhandCritPercentage = 0x910,
            SpellCritPercentage = 0x911,
            ShieldBlock = 0x912,
            ShieldBlockCritPercentage = 0x913,
            Mastery = 0x914,
            Speed = 0x915,
            Lifesteal = 0x916,
            Avoidance = 0x917,
            Sturdiness = 0x918,
            Versatility = 0x919,
            VersatilityBonus = 0x91A,
            PvpPowerDamage = 0x91B,
            PvpPowerHealing = 0x91C,
            ExploredZones = 0x91D,
            RestInfo = 0xA1D,
            ModDamageDonePos = 0xA21,
            ModDamageDoneNeg = 0xA28,
            ModDamageDonePercent = 0xA2F,
            ModHealingDonePos = 0xA36,
            ModHealingPercent = 0xA37,
            ModHealingDonePercent = 0xA38,
            ModPeriodicHealingDonePercent = 0xA39,
            WeaponDmgMultipliers = 0xA3A,
            WeaponAtkSpeedMultipliers = 0xA3D,
            ModSpellPowerPercent = 0xA40,
            ModResiliencePercent = 0xA41,
            OverrideSpellPowerByAPPercent = 0xA42,
            OverrideAPBySpellPowerPercent = 0xA43,
            ModTargetResistance = 0xA44,
            ModTargetPhysicalResistance = 0xA45,
            LocalFlags = 0xA46,
            NumRespecs = 0xA47,
            SelfResSpell = 0xA48,
            PvpMedals = 0xA49,
            BuybackPrice = 0xA4A,
            BuybackTimestamp = 0xA56,
            YesterdayHonorableKills = 0xA62,
            LifetimeHonorableKills = 0xA63,
            WatchedFactionIndex = 0xA64,
            CombatRatings = 0xA65,
            PvpInfo = 0xA85,
            MaxLevel = 0xAAF,
            ScalingPlayerLevelDelta = 0xAB0,
            MaxCreatureScalingLevel = 0xAB1,
            NoReagentCostMask = 0xAB2,
            PetSpellPower = 0xAB6,
            Researching = 0xAB7,
            ProfessionSkillLine = 0xAC1,
            UiHitModifier = 0xAC3,
            UiSpellHitModifier = 0xAC4,
            HomeRealmTimeOffset = 0xAC5,
            ModPetHaste = 0xAC6,
            AuraVision = 0xAC7,
            OverrideSpellsID = 0xAC8,
            LfgBonusFactionID = 0xAC9,
            LootSpecID = 0xACA,
            OverrideZonePVPType = 0xACB,
            BagSlotFlags = 0xACC,
            BankBagSlotFlags = 0xAD0,
            InsertItemsLeftToRight = 0xAD7,
            QuestCompleted = 0xAD8,
            Honor = 0x11AE,
            HonorNextLevel = 0x11AF,
        };

        public enum GameObjectFields
        {
            CreatedBy = 0xC,
            DisplayID = 0x10,
            Flags = 0x11,
            ParentRotation = 0x12,
            FactionTemplate = 0x16,
            Level = 0x17,
            PercentHealth = 0x18,
            SpellVisualID = 0x19,
            StateSpellVisualID = 0x1A,
            SpawnTrackingStateAnimID = 0x1B,
            SpawnTrackingStateAnimKitID = 0x1C,
            StateWorldEffectID = 0x1D,
        };

        public enum DynamicObjectFields
        {
            Caster = 0xC,
            Type = 0x10,
            SpellXSpellVisualID = 0x11,
            SpellID = 0x12,
            Radius = 0x13,
            CastTime = 0x14,
        };

        public enum CorpseFields
        {
            Owner = 0xC,
            PartyGUID = 0x10,
            DisplayID = 0x14,
            Items = 0x15,
            SkinID = 0x28,
            FacialHairStyleID = 0x29,
            Flags = 0x2A,
            DynamicFlags = 0x2B,
            FactionTemplate = 0x2C,
            CustomDisplayOption = 0x2D,
        };

        public enum AreaTriggerFields
        {
            Caster = 0xC,
            Duration = 0x10,
            TimeToTarget = 0x11,
            TimeToTargetScale = 0x12,
            TimeToTargetExtraScale = 0x13,
            SpellID = 0x14,
            SpellForVisuals = 0x15,
            SpellXSpellVisualID = 0x16,
            BoundsRadius2D = 0x17,
            DecalPropertiesID = 0x18,
            CreatingEffectGUID = 0x19,
        };

        public enum SceneObjectFields
        {
            ScriptPackageID = 0xC,
            RndSeedVal = 0xD,
            CreatedBy = 0xE,
            SceneType = 0x12,
        };

        public enum ConversationData
        {
        };

        public enum ItemDynamicFields
        {
        };

        public enum UnitDynamicFields
        {
        };

        public enum PlayerDynamicFields
        {
        };

        public enum ConversationDynamicData
        {
        };
    }
}