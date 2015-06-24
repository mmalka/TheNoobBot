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
        };

        public enum ContainerFields
        {
            Slots = 0x52,
            NumSlots = 0xE2,
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
            Power = 0x3D,
            MaxHealth = 0x43,
            MaxPower = 0x44,
            PowerRegenFlatModifier = 0x4A,
            PowerRegenInterruptedFlatModifier = 0x50,
            Level = 0x56,
            EffectiveLevel = 0x57,
            FactionTemplate = 0x58,
            VirtualItems = 0x59,
            Flags = 0x5F,
            Flags2 = 0x60,
            Flags3 = 0x61,
            AuraState = 0x62,
            AttackRoundBaseTime = 0x63,
            RangedAttackRoundBaseTime = 0x65,
            BoundingRadius = 0x66,
            CombatReach = 0x67,
            DisplayID = 0x68,
            NativeDisplayID = 0x69,
            MountDisplayID = 0x6A,
            MinDamage = 0x6B,
            MaxDamage = 0x6C,
            MinOffHandDamage = 0x6D,
            MaxOffHandDamage = 0x6E,
            AnimTier = 0x6F,
            PetNumber = 0x70,
            PetNameTimestamp = 0x71,
            PetExperience = 0x72,
            PetNextLevelExperience = 0x73,
            ModCastingSpeed = 0x74,
            ModSpellHaste = 0x75,
            ModHaste = 0x76,
            ModRangedHaste = 0x77,
            ModHasteRegen = 0x78,
            CreatedBySpell = 0x79,
            NpcFlags = 0x7A,
            EmoteState = 0x7C,
            Stats = 0x7D,
            StatPosBuff = 0x82,
            StatNegBuff = 0x87,
            Resistances = 0x8C,
            ResistanceBuffModsPositive = 0x93,
            ResistanceBuffModsNegative = 0x9A,
            ModBonusArmor = 0xA1,
            BaseMana = 0xA2,
            BaseHealth = 0xA3,
            ShapeshiftForm = 0xA4,
            AttackPower = 0xA5,
            AttackPowerModPos = 0xA6,
            AttackPowerModNeg = 0xA7,
            AttackPowerMultiplier = 0xA8,
            RangedAttackPower = 0xA9,
            RangedAttackPowerModPos = 0xAA,
            RangedAttackPowerModNeg = 0xAB,
            RangedAttackPowerMultiplier = 0xAC,
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
            RestState = 0xE6,
            ArenaFaction = 0xE7,
            DuelTeam = 0xE8,
            GuildTimeStamp = 0xE9,
            QuestLog = 0xEA,
            VisibleItems = 0x3D8,
            PlayerTitle = 0x3FE,
            FakeInebriation = 0x3FF,
            VirtualPlayerRealm = 0x400,
            CurrentSpecID = 0x401,
            TaxiMountAnimKitID = 0x402,
            AvgItemLevel = 0x403,
            CurrentBattlePetBreedQuality = 0x407,
            InvSlots = 0x408,
            FarsightObject = 0x6E8,
            KnownTitles = 0x6EC,
            Coinage = 0x6F6,
            XP = 0x6F8,
            NextLevelXP = 0x6F9,
            Skill = 0x6FA,
            CharacterPoints = 0x8BA,
            MaxTalentTiers = 0x8BB,
            TrackCreatureMask = 0x8BC,
            TrackResourceMask = 0x8BD,
            MainhandExpertise = 0x8BE,
            OffhandExpertise = 0x8BF,
            RangedExpertise = 0x8C0,
            CombatRatingExpertise = 0x8C1,
            BlockPercentage = 0x8C2,
            DodgePercentage = 0x8C3,
            ParryPercentage = 0x8C4,
            CritPercentage = 0x8C5,
            RangedCritPercentage = 0x8C6,
            OffhandCritPercentage = 0x8C7,
            SpellCritPercentage = 0x8C8,
            ShieldBlock = 0x8CF,
            ShieldBlockCritPercentage = 0x8D0,
            Mastery = 0x8D1,
            Amplify = 0x8D2,
            Multistrike = 0x8D3,
            MultistrikeEffect = 0x8D4,
            Readiness = 0x8D5,
            Speed = 0x8D6,
            Lifesteal = 0x8D7,
            Avoidance = 0x8D8,
            Sturdiness = 0x8D9,
            Cleave = 0x8DA,
            Versatility = 0x8DB,
            VersatilityBonus = 0x8DC,
            PvpPowerDamage = 0x8DD,
            PvpPowerHealing = 0x8DE,
            ExploredZones = 0x8DF,
            RestStateBonusPool = 0x9DF,
            ModDamageDonePos = 0x9E0,
            ModDamageDoneNeg = 0x9E7,
            ModDamageDonePercent = 0x9EE,
            ModHealingDonePos = 0x9F5,
            ModHealingPercent = 0x9F6,
            ModHealingDonePercent = 0x9F7,
            ModPeriodicHealingDonePercent = 0x9F8,
            WeaponDmgMultipliers = 0x9F9,
            WeaponAtkSpeedMultipliers = 0x9FC,
            ModSpellPowerPercent = 0x9FF,
            ModResiliencePercent = 0xA00,
            OverrideSpellPowerByAPPercent = 0xA01,
            OverrideAPBySpellPowerPercent = 0xA02,
            ModTargetResistance = 0xA03,
            ModTargetPhysicalResistance = 0xA04,
            LocalFlags = 0xA05,
            LifetimeMaxRank = 0xA06,
            SelfResSpell = 0xA07,
            PvpMedals = 0xA08,
            BuybackPrice = 0xA09,
            BuybackTimestamp = 0xA15,
            YesterdayHonorableKills = 0xA21,
            LifetimeHonorableKills = 0xA22,
            WatchedFactionIndex = 0xA23,
            CombatRatings = 0xA24,
            PvpInfo = 0xA44,
            MaxLevel = 0xA68,
            RuneRegen = 0xA69,
            NoReagentCostMask = 0xA6D,
            GlyphSlots = 0xA71,
            Glyphs = 0xA77,
            GlyphSlotsEnabled = 0xA7D,
            PetSpellPower = 0xA7E,
            Researching = 0xA7F,
            ProfessionSkillLine = 0xA89,
            UiHitModifier = 0xA8B,
            UiSpellHitModifier = 0xA8C,
            HomeRealmTimeOffset = 0xA8D,
            ModPetHaste = 0xA8E,
            SummonedBattlePetGUID = 0xA8F,
            OverrideSpellsID = 0xA93,
            LfgBonusFactionID = 0xA94,
            LootSpecID = 0xA95,
            OverrideZonePVPType = 0xA96,
            ItemLevelDelta = 0xA97,
            BagSlotFlags = 0xA98,
            BankBagSlotFlags = 0xA9C,
            InsertItemsLeftToRight = 0xAA3,
            QuestCompleted = 0xAA4,
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
            StateAnimID = 0x1B,
            StateAnimKitID = 0x1C,
            StateWorldEffectID = 0x1D,
        };

        public enum DynamicObjectFields
        {
            Caster = 0xC,
            TypeAndVisualID = 0x10,
            SpellID = 0x11,
            Radius = 0x12,
            CastTime = 0x13,
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
        };

        public enum AreaTriggerFields
        {
            OverrideScaleCurve = 0xC,
            Caster = 0x13,
            Duration = 0x17,
            TimeToTargetScale = 0x18,
            SpellID = 0x19,
            SpellVisualID = 0x1A,
            BoundsRadius2D = 0x1B,
            ExplicitScale = 0x1C,
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
            Dummy = 0xC,
        };

        public enum ItemDynamicFields
        {
            Modifiers = 0xC,
        };

        public enum UnitDynamicFields
        {
        };

        public enum PlayerDynamicFields
        {
        };

        public enum ConversationDynamicData
        {
            Actors = 0xC,
        };
    }
}