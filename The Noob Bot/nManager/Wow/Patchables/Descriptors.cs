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
            ItemAppearanceModID = 0x53,
        };

        public enum ContainerFields
        {
            Slots = 0x54,
            NumSlots = 0xE4,
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
            ChannelObject = 0x32,
            ChannelSpell = 0x36,
            ChannelSpellXSpellVisual = 0x37,
            SummonedByHomeRealm = 0x38,
            Sex = 0x39,
            DisplayPower = 0x3A,
            OverrideDisplayPowerID = 0x3B,
            Health = 0x3C,
            Power = 0x3E,
            MaxHealth = 0x44,
            MaxPower = 0x46,
            PowerRegenFlatModifier = 0x4C,
            PowerRegenInterruptedFlatModifier = 0x52,
            Level = 0x58,
            EffectiveLevel = 0x59,
            ScalingLevelMin = 0x5A,
            ScalingLevelMax = 0x5B,
            ScalingLevelDelta = 0x5C,
            FactionTemplate = 0x5D,
            VirtualItems = 0x5E,
            Flags = 0x64,
            Flags2 = 0x65,
            Flags3 = 0x66,
            AuraState = 0x67,
            AttackRoundBaseTime = 0x68,
            RangedAttackRoundBaseTime = 0x6A,
            BoundingRadius = 0x6B,
            CombatReach = 0x6C,
            DisplayID = 0x6D,
            NativeDisplayID = 0x6E,
            MountDisplayID = 0x6F,
            MinDamage = 0x70,
            MaxDamage = 0x71,
            MinOffHandDamage = 0x72,
            MaxOffHandDamage = 0x73,
            AnimTier = 0x74,
            PetNumber = 0x75,
            PetNameTimestamp = 0x76,
            PetExperience = 0x77,
            PetNextLevelExperience = 0x78,
            ModCastingSpeed = 0x79,
            ModSpellHaste = 0x7A,
            ModHaste = 0x7B,
            ModRangedHaste = 0x7C,
            ModHasteRegen = 0x7D,
            ModTimeRate = 0x7E,
            CreatedBySpell = 0x7F,
            NpcFlags = 0x80,
            EmoteState = 0x82,
            Stats = 0x83,
            StatPosBuff = 0x87,
            StatNegBuff = 0x8B,
            Resistances = 0x8F,
            ResistanceBuffModsPositive = 0x96,
            ResistanceBuffModsNegative = 0x9D,
            ModBonusArmor = 0xA4,
            BaseMana = 0xA5,
            BaseHealth = 0xA6,
            ShapeshiftForm = 0xA7,
            AttackPower = 0xA8,
            AttackPowerModPos = 0xA9,
            AttackPowerModNeg = 0xAA,
            AttackPowerMultiplier = 0xAB,
            RangedAttackPower = 0xAC,
            RangedAttackPowerModPos = 0xAD,
            RangedAttackPowerModNeg = 0xAE,
            RangedAttackPowerMultiplier = 0xAF,
            SetAttackSpeedAura = 0xB0,
            MinRangedDamage = 0xB1,
            MaxRangedDamage = 0xB2,
            PowerCostModifier = 0xB3,
            PowerCostMultiplier = 0xBA,
            MaxHealthModifier = 0xC1,
            HoverHeight = 0xC2,
            MinItemLevelCutoff = 0xC3,
            MinItemLevel = 0xC4,
            MaxItemLevel = 0xC5,
            WildBattlePetLevel = 0xC6,
            BattlePetCompanionNameTimestamp = 0xC7,
            InteractSpellID = 0xC8,
            StateSpellVisualID = 0xC9,
            StateAnimID = 0xCA,
            StateAnimKitID = 0xCB,
            StateWorldEffectID = 0xCC,
            ScaleDuration = 0xD0,
            LooksLikeMountID = 0xD1,
            LooksLikeCreatureID = 0xD2,
            LookAtControllerID = 0xD3,
            LookAtControllerTarget = 0xD4,
        };

        public enum PlayerFields
        {
            DuelArbiter = 0xD8,
            WowAccount = 0xDC,
            LootTargetGUID = 0xE0,
            PlayerFlags = 0xE4,
            PlayerFlagsEx = 0xE5,
            GuildRankID = 0xE6,
            GuildDeleteDate = 0xE7,
            GuildLevel = 0xE8,
            HairColorID = 0xE9,
            CustomDisplayOption = 0xEA,
            Inebriation = 0xEB,
            ArenaFaction = 0xEC,
            DuelTeam = 0xED,
            GuildTimeStamp = 0xEE,
            QuestLog = 0xEF,
            VisibleItems = 0x40F,
            PlayerTitle = 0x435,
            FakeInebriation = 0x436,
            VirtualPlayerRealm = 0x437,
            CurrentSpecID = 0x438,
            TaxiMountAnimKitID = 0x439,
            AvgItemLevel = 0x43A,
            CurrentBattlePetBreedQuality = 0x43E,
            Prestige = 0x43F,
            HonorLevel = 0x440,
            InvSlots = 0x441,
            FarsightObject = 0x72D,
            SummonedBattlePetGUID = 0x731,
            KnownTitles = 0x735,
            Coinage = 0x741,
            XP = 0x743,
            NextLevelXP = 0x744,
            Skill = 0x745,
            CharacterPoints = 0x905,
            MaxTalentTiers = 0x906,
            TrackCreatureMask = 0x907,
            TrackResourceMask = 0x908,
            MainhandExpertise = 0x909,
            OffhandExpertise = 0x90A,
            RangedExpertise = 0x90B,
            CombatRatingExpertise = 0x90C,
            BlockPercentage = 0x90D,
            DodgePercentage = 0x90E,
            DodgePercentageFromAttribute = 0x90F,
            ParryPercentage = 0x910,
            ParryPercentageFromAttribute = 0x911,
            CritPercentage = 0x912,
            RangedCritPercentage = 0x913,
            OffhandCritPercentage = 0x914,
            SpellCritPercentage = 0x915,
            ShieldBlock = 0x916,
            ShieldBlockCritPercentage = 0x917,
            Mastery = 0x918,
            Speed = 0x919,
            Lifesteal = 0x91A,
            Avoidance = 0x91B,
            Sturdiness = 0x91C,
            Versatility = 0x91D,
            VersatilityBonus = 0x91E,
            PvpPowerDamage = 0x91F,
            PvpPowerHealing = 0x920,
            ExploredZones = 0x921,
            RestInfo = 0xA21,
            ModDamageDonePos = 0xA25,
            ModDamageDoneNeg = 0xA2C,
            ModDamageDonePercent = 0xA33,
            ModHealingDonePos = 0xA3A,
            ModHealingPercent = 0xA3B,
            ModHealingDonePercent = 0xA3C,
            ModPeriodicHealingDonePercent = 0xA3D,
            WeaponDmgMultipliers = 0xA3E,
            WeaponAtkSpeedMultipliers = 0xA41,
            ModSpellPowerPercent = 0xA44,
            ModResiliencePercent = 0xA45,
            OverrideSpellPowerByAPPercent = 0xA46,
            OverrideAPBySpellPowerPercent = 0xA47,
            ModTargetResistance = 0xA48,
            ModTargetPhysicalResistance = 0xA49,
            LocalFlags = 0xA4A,
            NumRespecs = 0xA4B,
            SelfResSpell = 0xA4C,
            PvpMedals = 0xA4D,
            BuybackPrice = 0xA4E,
            BuybackTimestamp = 0xA5A,
            YesterdayHonorableKills = 0xA66,
            LifetimeHonorableKills = 0xA67,
            WatchedFactionIndex = 0xA68,
            CombatRatings = 0xA69,
            PvpInfo = 0xA89,
            MaxLevel = 0xAB3,
            ScalingPlayerLevelDelta = 0xAB4,
            MaxCreatureScalingLevel = 0xAB5,
            NoReagentCostMask = 0xAB6,
            PetSpellPower = 0xABA,
            Researching = 0xABB,
            ProfessionSkillLine = 0xAC5,
            UiHitModifier = 0xAC7,
            UiSpellHitModifier = 0xAC8,
            HomeRealmTimeOffset = 0xAC9,
            ModPetHaste = 0xACA,
            AuraVision = 0xACB,
            OverrideSpellsID = 0xACC,
            LfgBonusFactionID = 0xACD,
            LootSpecID = 0xACE,
            OverrideZonePVPType = 0xACF,
            BagSlotFlags = 0xAD0,
            BankBagSlotFlags = 0xAD4,
            InsertItemsLeftToRight = 0xADB,
            QuestCompleted = 0xADC,
            Honor = 0xEC4,
            HonorNextLevel = 0xEC5,
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
            SpellXSpellVisualID = 0x15,
            BoundsRadius2D = 0x16,
            DecalPropertiesID = 0x17,
            CreatingEffectGUID = 0x18,
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