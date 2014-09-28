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
        };

        public enum PlayerFields
        {
            DuelArbiter = 0xCB,
            WowAccount = 0xCF,
            LootTargetGUID = 0xD3,
            PlayerFlags = 0xD7,
            PlayerFlagsEx = 0xD8,
            GuildRankID = 0xD9,
            GuildDeleteDate = 0xDA,
            GuildLevel = 0xDB,
            HairColorID = 0xDC,
            RestState = 0xDD,
            ArenaFaction = 0xDE,
            DuelTeam = 0xDF,
            GuildTimeStamp = 0xE0,
            QuestLog = 0xE1,
            VisibleItems = 0x3CF,
            PlayerTitle = 0x408,
            FakeInebriation = 0x409,
            VirtualPlayerRealm = 0x40A,
            CurrentSpecID = 0x40B,
            TaxiMountAnimKitID = 0x40C,
            AvgItemLevelTotal = 0x40D,
            AvgItemLevelEquipped = 0x40E,
            CurrentBattlePetBreedQuality = 0x40F,
            InvSlots = 0x410,
            FarsightObject = 0x6F0,
            KnownTitles = 0x6F4,
            Coinage = 0x6FE,
            XP = 0x700,
            NextLevelXP = 0x701,
            Skill = 0x702,
            CharacterPoints = 0x8C2,
            MaxTalentTiers = 0x8C3,
            TrackCreatureMask = 0x8C4,
            TrackResourceMask = 0x8C5,
            MainhandExpertise = 0x8C6,
            OffhandExpertise = 0x8C7,
            RangedExpertise = 0x8C8,
            CombatRatingExpertise = 0x8C9,
            BlockPercentage = 0x8CA,
            DodgePercentage = 0x8CB,
            ParryPercentage = 0x8CC,
            CritPercentage = 0x8CD,
            RangedCritPercentage = 0x8CE,
            OffhandCritPercentage = 0x8CF,
            SpellCritPercentage = 0x8D0,
            ShieldBlock = 0x8D7,
            ShieldBlockCritPercentage = 0x8D8,
            Mastery = 0x8D9,
            Amplify = 0x8DA,
            Multistrike = 0x8DB,
            MultistrikeEffect = 0x8DC,
            Readiness = 0x8DD,
            Speed = 0x8DE,
            Lifesteal = 0x8DF,
            Avoidance = 0x8E0,
            Sturdiness = 0x8E1,
            Cleave = 0x8E2,
            Versatility = 0x8E3,
            VersatilityBonus = 0x8E4,
            PvpPowerDamage = 0x8E5,
            PvpPowerHealing = 0x8E6,
            ExploredZones = 0x8E7,
            RestStateBonusPool = 0x9AF,
            ModDamageDonePos = 0x9B0,
            ModDamageDoneNeg = 0x9B7,
            ModDamageDonePercent = 0x9BE,
            ModHealingDonePos = 0x9C5,
            ModHealingPercent = 0x9C6,
            ModHealingDonePercent = 0x9C7,
            ModPeriodicHealingDonePercent = 0x9C8,
            WeaponDmgMultipliers = 0x9C9,
            WeaponAtkSpeedMultipliers = 0x9CC,
            ModSpellPowerPercent = 0x9CF,
            ModResiliencePercent = 0x9D0,
            OverrideSpellPowerByAPPercent = 0x9D1,
            OverrideAPBySpellPowerPercent = 0x9D2,
            ModTargetResistance = 0x9D3,
            ModTargetPhysicalResistance = 0x9D4,
            LocalFlags = 0x9D5,
            LifetimeMaxRank = 0x9D6,
            SelfResSpell = 0x9D7,
            PvpMedals = 0x9D8,
            BuybackPrice = 0x9D9,
            BuybackTimestamp = 0x9E5,
            YesterdayHonorableKills = 0x9F1,
            LifetimeHonorableKills = 0x9F2,
            WatchedFactionIndex = 0x9F3,
            CombatRatings = 0x9F4,
            PvpInfo = 0xA14,
            MaxLevel = 0xA38,
            RuneRegen = 0xA39,
            NoReagentCostMask = 0xA3D,
            GlyphSlots = 0xA41,
            Glyphs = 0xA47,
            GlyphSlotsEnabled = 0xA4D,
            PetSpellPower = 0xA4E,
            Researching = 0xA4F,
            ProfessionSkillLine = 0xA59,
            UiHitModifier = 0xA5B,
            UiSpellHitModifier = 0xA5C,
            HomeRealmTimeOffset = 0xA5D,
            ModPetHaste = 0xA5E,
            SummonedBattlePetGUID = 0xA5F,
            OverrideSpellsID = 0xA63,
            LfgBonusFactionID = 0xA64,
            LootSpecID = 0xA65,
            OverrideZonePVPType = 0xA66,
            ItemLevelDelta = 0xA67,
            BagSlotFlags = 0xA68,
            BankBagSlotFlags = 0xA6C,
            InsertItemsLeftToRight = 0xA73,
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