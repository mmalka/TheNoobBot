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
            SummonedByHomeRealm = 0x37,
            Sex = 0x38,
            DisplayPower = 0x39,
            OverrideDisplayPowerID = 0x3A,
            Health = 0x3B,
            Power = 0x3C,
            MaxHealth = 0x42,
            MaxPower = 0x43,
            PowerRegenFlatModifier = 0x49,
            PowerRegenInterruptedFlatModifier = 0x4F,
            Level = 0x55,
            EffectiveLevel = 0x56,
            FactionTemplate = 0x57,
            VirtualItemID = 0x58,
            Flags = 0x5B,
            Flags2 = 0x5C,
            Flags3 = 0x5D,
            AuraState = 0x5E,
            AttackRoundBaseTime = 0x5F,
            RangedAttackRoundBaseTime = 0x61,
            BoundingRadius = 0x62,
            CombatReach = 0x63,
            DisplayID = 0x64,
            NativeDisplayID = 0x65,
            MountDisplayID = 0x66,
            MinDamage = 0x67,
            MaxDamage = 0x68,
            MinOffHandDamage = 0x69,
            MaxOffHandDamage = 0x6A,
            AnimTier = 0x6B,
            PetNumber = 0x6C,
            PetNameTimestamp = 0x6D,
            PetExperience = 0x6E,
            PetNextLevelExperience = 0x6F,
            ModCastingSpeed = 0x70,
            ModSpellHaste = 0x71,
            ModHaste = 0x72,
            ModRangedHaste = 0x73,
            ModHasteRegen = 0x74,
            CreatedBySpell = 0x75,
            NpcFlags = 0x76,
            EmoteState = 0x78,
            Stats = 0x79,
            StatPosBuff = 0x7E,
            StatNegBuff = 0x83,
            Resistances = 0x88,
            ResistanceBuffModsPositive = 0x8F,
            ResistanceBuffModsNegative = 0x96,
            ModBonusArmor = 0x9D,
            BaseMana = 0x9E,
            BaseHealth = 0x9F,
            ShapeshiftForm = 0xA0,
            AttackPower = 0xA1,
            AttackPowerModPos = 0xA2,
            AttackPowerModNeg = 0xA3,
            AttackPowerMultiplier = 0xA4,
            RangedAttackPower = 0xA5,
            RangedAttackPowerModPos = 0xA6,
            RangedAttackPowerModNeg = 0xA7,
            RangedAttackPowerMultiplier = 0xA8,
            MinRangedDamage = 0xA9,
            MaxRangedDamage = 0xAA,
            PowerCostModifier = 0xAB,
            PowerCostMultiplier = 0xB2,
            MaxHealthModifier = 0xB9,
            HoverHeight = 0xBA,
            MinItemLevelCutoff = 0xBB,
            MinItemLevel = 0xBC,
            MaxItemLevel = 0xBD,
            WildBattlePetLevel = 0xBE,
            BattlePetCompanionNameTimestamp = 0xBF,
            InteractSpellID = 0xC0,
            StateSpellVisualID = 0xC1,
            StateAnimID = 0xC2,
            StateAnimKitID = 0xC3,
            StateWorldEffectID = 0xC4,
            ScaleDuration = 0xC8,
            LooksLikeMountID = 0xC9,
            LooksLikeCreatureID = 0xCA,
            LookAtControllerID = 0xCB,
            LookAtControllerTarget = 0xCC,
        };

        public enum PlayerFields
        {
            DuelArbiter = 0xD0,
            WowAccount = 0xD4,
            LootTargetGUID = 0xD8,
            PlayerFlags = 0xDC,
            PlayerFlagsEx = 0xDD,
            GuildRankID = 0xDE,
            GuildDeleteDate = 0xDF,
            GuildLevel = 0xE0,
            HairColorID = 0xE1,
            RestState = 0xE2,
            ArenaFaction = 0xE3,
            DuelTeam = 0xE4,
            GuildTimeStamp = 0xE5,
            QuestLog = 0xE6,
            VisibleItems = 0x3D4,
            PlayerTitle = 0x40D,
            FakeInebriation = 0x40E,
            VirtualPlayerRealm = 0x40F,
            CurrentSpecID = 0x410,
            TaxiMountAnimKitID = 0x411,
            AvgItemLevelTotal = 0x412,
            AvgItemLevelEquipped = 0x413,
            CurrentBattlePetBreedQuality = 0x414,
            InvSlots = 0x415,
            FarsightObject = 0x6F5,
            KnownTitles = 0x6F9,
            Coinage = 0x703,
            XP = 0x705,
            NextLevelXP = 0x706,
            Skill = 0x707,
            CharacterPoints = 0x8C7,
            MaxTalentTiers = 0x8C8,
            TrackCreatureMask = 0x8C9,
            TrackResourceMask = 0x8CA,
            MainhandExpertise = 0x8CB,
            OffhandExpertise = 0x8CC,
            RangedExpertise = 0x8CD,
            CombatRatingExpertise = 0x8CE,
            BlockPercentage = 0x8CF,
            DodgePercentage = 0x8D0,
            ParryPercentage = 0x8D1,
            CritPercentage = 0x8D2,
            RangedCritPercentage = 0x8D3,
            OffhandCritPercentage = 0x8D4,
            SpellCritPercentage = 0x8D5,
            ShieldBlock = 0x8DC,
            ShieldBlockCritPercentage = 0x8DD,
            Mastery = 0x8DE,
            Amplify = 0x8DF,
            Multistrike = 0x8E0,
            MultistrikeEffect = 0x8E1,
            Readiness = 0x8E2,
            Speed = 0x8E3,
            Lifesteal = 0x8E4,
            Avoidance = 0x8E5,
            Sturdiness = 0x8E6,
            Cleave = 0x8E7,
            Versatility = 0x8E8,
            VersatilityBonus = 0x8E9,
            PvpPowerDamage = 0x8EA,
            PvpPowerHealing = 0x8EB,
            ExploredZones = 0x8EC,
            RestStateBonusPool = 0x9B4,
            ModDamageDonePos = 0x9B5,
            ModDamageDoneNeg = 0x9BC,
            ModDamageDonePercent = 0x9C3,
            ModHealingDonePos = 0x9CA,
            ModHealingPercent = 0x9CB,
            ModHealingDonePercent = 0x9CC,
            ModPeriodicHealingDonePercent = 0x9CD,
            WeaponDmgMultipliers = 0x9CE,
            WeaponAtkSpeedMultipliers = 0x9D1,
            ModSpellPowerPercent = 0x9D4,
            ModResiliencePercent = 0x9D5,
            OverrideSpellPowerByAPPercent = 0x9D6,
            OverrideAPBySpellPowerPercent = 0x9D7,
            ModTargetResistance = 0x9D8,
            ModTargetPhysicalResistance = 0x9D9,
            LocalFlags = 0x9DA,
            LifetimeMaxRank = 0x9DB,
            SelfResSpell = 0x9DC,
            PvpMedals = 0x9DD,
            BuybackPrice = 0x9DE,
            BuybackTimestamp = 0x9EA,
            YesterdayHonorableKills = 0x9F6,
            LifetimeHonorableKills = 0x9F7,
            WatchedFactionIndex = 0x9F8,
            CombatRatings = 0x9F9,
            PvpInfo = 0xA19,
            MaxLevel = 0xA3D,
            RuneRegen = 0xA3E,
            NoReagentCostMask = 0xA42,
            GlyphSlots = 0xA46,
            Glyphs = 0xA4C,
            GlyphSlotsEnabled = 0xA52,
            PetSpellPower = 0xA53,
            Researching = 0xA54,
            ProfessionSkillLine = 0xA5E,
            UiHitModifier = 0xA60,
            UiSpellHitModifier = 0xA61,
            HomeRealmTimeOffset = 0xA62,
            ModPetHaste = 0xA63,
            SummonedBattlePetGUID = 0xA64,
            OverrideSpellsID = 0xA68,
            LfgBonusFactionID = 0xA69,
            LootSpecID = 0xA6A,
            OverrideZonePVPType = 0xA6B,
            ItemLevelDelta = 0xA6C,
            BagSlotFlags = 0xA6D,
            BankBagSlotFlags = 0xA71,
            InsertItemsLeftToRight = 0xA78,
            QuestCompleted = 0xA79,
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
            Caster = 0xC,
            Duration = 0x10,
            SpellID = 0x11,
            SpellVisualID = 0x12,
            ExplicitScale = 0x13,
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