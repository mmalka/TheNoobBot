namespace nManager.Wow.Patchables
{
    /// <summary>
    /// Descriptors
    /// </summary>
    public static class Descriptors
    {
        public const uint multiplicator = 4; // 4 or 1
        public static readonly uint startDescriptors = 0x8;

        public enum ObjectFields
        {
            Guid = 0x0,
            Data = 0x2,
            Type = 0x4,
            Entry = 0x5,
            Scale = 0x6,
            End = 0x7
        };

        public enum DynamicObjectArrays
        {
            // Empty
            End = 0x0
        }

        public enum ItemFields
        {
            Owner = ObjectFields.End + 0x0,
            ContainedIn = ObjectFields.End + 0x2,
            Creator = ObjectFields.End + 0x4,
            GiftCreator = ObjectFields.End + 0x6,
            StackCount = ObjectFields.End + 0x8,
            Expiration = ObjectFields.End + 0x9,
            SpellCharges = ObjectFields.End + 0xA,
            DynamicFlags = ObjectFields.End + 0xF,
            Enchantment = ObjectFields.End + 0x10,
            PropertySeed = ObjectFields.End + 0x37,
            RandomPropertiesID = ObjectFields.End + 0x38,
            Durability = ObjectFields.End + 0x39,
            MaxDurability = ObjectFields.End + 0x3A,
            CreatePlayedTime = ObjectFields.End + 0x3B,
            ModifiersMask = ObjectFields.End + 0x3C,
            End = ObjectFields.End + 0x3D
        };

        public enum ItemDynamicArrays
        {
            // Empty
            End = DynamicObjectArrays.End
        }

        public enum ContainerFields
        {
            NumSlots = ItemFields.End + 0x0,
            Slots = ItemFields.End + 0x1,
            End = ItemFields.End + 0x49
        };

        public enum ContainerDynamicArrays
        {
            // Empty
            End = ItemDynamicArrays.End
        }

        public enum UnitFields
        {
            Charm = ObjectFields.End + 0x0,
            Summon = ObjectFields.End + 0x2,
            Critter = ObjectFields.End + 0x4,
            CharmedBy = ObjectFields.End + 0x6,
            SummonedBy = ObjectFields.End + 0x8,
            CreatedBy = ObjectFields.End + 0xA,
            Target = ObjectFields.End + 0xC,
            ChannelObject = ObjectFields.End + 0xE,
            SummonedByHomeRealm = ObjectFields.End + 0x10,
            ChannelSpell = ObjectFields.End + 0x11,
            DisplayPower = ObjectFields.End + 0x12,
            OverrideDisplayPowerID = ObjectFields.End + 0x13,
            Health = ObjectFields.End + 0x14,
            Power = ObjectFields.End + 0x15,
            MaxHealth = ObjectFields.End + 0x1A,
            MaxPower = ObjectFields.End + 0x1B,
            PowerRegenFlatModifier = ObjectFields.End + 0x20,
            PowerRegenInterruptedFlatModifier = ObjectFields.End + 0x25,
            Level = ObjectFields.End + 0x2A,
            FactionTemplate = ObjectFields.End + 0x2B,
            VirtualItemID = ObjectFields.End + 0x2C,
            Flags = ObjectFields.End + 0x2F,
            Flags2 = ObjectFields.End + 0x30,
            AuraState = ObjectFields.End + 0x31,
            AttackRoundBaseTime = ObjectFields.End + 0x32,
            RangedAttackRoundBaseTime = ObjectFields.End + 0x34,
            BoundingRadius = ObjectFields.End + 0x35,
            CombatReach = ObjectFields.End + 0x36,
            DisplayID = ObjectFields.End + 0x37,
            NativeDisplayID = ObjectFields.End + 0x38,
            MountDisplayID = ObjectFields.End + 0x39,
            MinDamage = ObjectFields.End + 0x3A,
            MaxDamage = ObjectFields.End + 0x3B,
            MinOffHandDamage = ObjectFields.End + 0x3C,
            MaxOffHandDamage = ObjectFields.End + 0x3D,
            AnimTier = ObjectFields.End + 0x3E,
            PetNumber = ObjectFields.End + 0x3F,
            PetNameTimestamp = ObjectFields.End + 0x40,
            PetExperience = ObjectFields.End + 0x41,
            PetNextLevelExperience = ObjectFields.End + 0x42,
            DynamicFlags = ObjectFields.End + 0x43,
            ModCastingSpeed = ObjectFields.End + 0x44,
            ModSpellHaste = ObjectFields.End + 0x45,
            ModHaste = ObjectFields.End + 0x46,
            ModHasteRegen = ObjectFields.End + 0x47,
            CreatedBySpell = ObjectFields.End + 0x48,
            NpcFlags = ObjectFields.End + 0x49,
            EmoteState = ObjectFields.End + 0x4B,
            Stats = ObjectFields.End + 0x4C,
            StatPosBuff = ObjectFields.End + 0x51,
            StatNegBuff = ObjectFields.End + 0x56,
            Resistances = ObjectFields.End + 0x5B,
            ResistanceBuffModsPositive = ObjectFields.End + 0x62,
            ResistanceBuffModsNegative = ObjectFields.End + 0x69,
            BaseMana = ObjectFields.End + 0x70,
            BaseHealth = ObjectFields.End + 0x71,
            ShapeshiftForm = ObjectFields.End + 0x72,
            AttackPower = ObjectFields.End + 0x73,
            AttackPowerModPos = ObjectFields.End + 0x74,
            AttackPowerModNeg = ObjectFields.End + 0x75,
            AttackPowerMultiplier = ObjectFields.End + 0x76,
            RangedAttackPower = ObjectFields.End + 0x77,
            RangedAttackPowerModPos = ObjectFields.End + 0x78,
            RangedAttackPowerModNeg = ObjectFields.End + 0x79,
            RangedAttackPowerMultiplier = ObjectFields.End + 0x7A,
            MinRangedDamage = ObjectFields.End + 0x7B,
            MaxRangedDamage = ObjectFields.End + 0x7C,
            PowerCostModifier = ObjectFields.End + 0x7D,
            PowerCostMultiplier = ObjectFields.End + 0x84,
            MaxHealthModifier = ObjectFields.End + 0x8B,
            HoverHeight = ObjectFields.End + 0x8C,
            MinItemLevel = ObjectFields.End + 0x8D,
            MaxItemLevel = ObjectFields.End + 0x8E,
            WildBattlePetLevel = ObjectFields.End + 0x8F,
            BattlePetCompanionGUID = ObjectFields.End + 0x90,
            BattlePetCompanionNameTimestamp = ObjectFields.End + 0x92,
            End = ObjectFields.End + 0x93
        };

        public enum UnitDynamicArrays
        {
            PassiveSpells = DynamicObjectArrays.End + 0x0,
            End = DynamicObjectArrays.End + 0x101
        }

        public enum PlayerFields
        {
            DuelArbiter = UnitFields.End + 0x0,
            PlayerFlags = UnitFields.End + 0x2,
            GuildRankID = UnitFields.End + 0x3,
            GuildDeleteDate = UnitFields.End + 0x4,
            GuildLevel = UnitFields.End + 0x5,
            HairColorID = UnitFields.End + 0x6,
            RestState = UnitFields.End + 0x7,
            ArenaFaction = UnitFields.End + 0x8,
            DuelTeam = UnitFields.End + 0x9,
            GuildTimeStamp = UnitFields.End + 0xA,
            QuestLog = UnitFields.End + 0xB,
            VisibleItems = UnitFields.End + 0x2F9,
            PlayerTitle = UnitFields.End + 0x31F,
            FakeInebriation = UnitFields.End + 0x320,
            HomePlayerRealm = UnitFields.End + 0x321,
            CurrentSpecID = UnitFields.End + 0x322,
            TaxiMountAnimKitID = UnitFields.End + 0x323,
            CurrentBattlePetBreedQuality = UnitFields.End + 0x324,
            InvSlots = UnitFields.End + 0x325,
            FarsightObject = UnitFields.End + 0x3D1,
            KnownTitles = UnitFields.End + 0x3D3,
            Coinage = UnitFields.End + 0x3DB,
            XP = UnitFields.End + 0x3DD,
            NextLevelXP = UnitFields.End + 0x3DE,
            Skill = UnitFields.End + 0x3DF,
            SpellCritPercentage = UnitFields.End + 0x59F,
            CharacterPoints = UnitFields.End + 0x5A6,
            MaxTalentTiers = UnitFields.End + 0x5A7,
            TrackCreatureMask = UnitFields.End + 0x5A8,
            TrackResourceMask = UnitFields.End + 0x5A9,
            Expertise = UnitFields.End + 0x5AA,
            OffhandExpertise = UnitFields.End + 0x5AB,
            RangedExpertise = UnitFields.End + 0x5AC,
            BlockPercentage = UnitFields.End + 0x5AD,
            DodgePercentage = UnitFields.End + 0x5AE,
            ParryPercentage = UnitFields.End + 0x5AF,
            CritPercentage = UnitFields.End + 0x5B0,
            RangedCritPercentage = UnitFields.End + 0x5B1,
            OffhandCritPercentage = UnitFields.End + 0x5B2,
            ShieldBlock = UnitFields.End + 0x5B3,
            ShieldBlockCritPercentage = UnitFields.End + 0x5B4,
            Mastery = UnitFields.End + 0x5B5,
            PvpPowerDamage = UnitFields.End + 0x5B6,
            PvpPowerHealing = UnitFields.End + 0x5B7,
            ExploredZones = UnitFields.End + 0x5B8,
            ModDamageDonePos = UnitFields.End + 0x680,
            ModDamageDoneNeg = UnitFields.End + 0x687,
            ModDamageDonePercent = UnitFields.End + 0x68E,
            RestStateBonusPool = UnitFields.End + 0x695,
            ModHealingDonePos = UnitFields.End + 0x696,
            ModHealingPercent = UnitFields.End + 0x697,
            ModHealingDonePercent = UnitFields.End + 0x698,
            WeaponDmgMultipliers = UnitFields.End + 0x699,
            ModPeriodicHealingDonePercent = UnitFields.End + 0x69C,
            ModSpellPowerPercent = UnitFields.End + 0x69D,
            ModResiliencePercent = UnitFields.End + 0x69E,
            OverrideSpellPowerByAPPercent = UnitFields.End + 0x69F,
            OverrideAPBySpellPowerPercent = UnitFields.End + 0x6A0,
            ModTargetResistance = UnitFields.End + 0x6A1,
            ModTargetPhysicalResistance = UnitFields.End + 0x6A2,
            LifetimeMaxRank = UnitFields.End + 0x6A3,
            SelfResSpell = UnitFields.End + 0x6A4,
            PvpMedals = UnitFields.End + 0x6A5,
            BuybackPrice = UnitFields.End + 0x6A6,
            BuybackTimestamp = UnitFields.End + 0x6B2,
            YesterdayHonorableKills = UnitFields.End + 0x6BE,
            LifetimeHonorableKills = UnitFields.End + 0x6BF,
            WatchedFactionIndex = UnitFields.End + 0x6C0,
            CombatRatings = UnitFields.End + 0x6C1,
            ArenaTeams = UnitFields.End + 0x6DC,
            RuneRegen = UnitFields.End + 0x6F1,
            NoReagentCostMask = UnitFields.End + 0x6F5,
            GlyphSlots = UnitFields.End + 0x6F9,
            Glyphs = UnitFields.End + 0x6FF,
            BattlegroundRating = UnitFields.End + 0x705,
            MaxLevel = UnitFields.End + 0x706,
            GlyphSlotsEnabled = UnitFields.End + 0x707,
            Researching = UnitFields.End + 0x708,
            ProfessionSkillLine = UnitFields.End + 0x710,
            PetSpellPower = UnitFields.End + 0x712,
            UiHitModifier = UnitFields.End + 0x713,
            UiSpellHitModifier = UnitFields.End + 0x714,
            HomeRealmTimeOffset = UnitFields.End + 0x715,
            ModRangedHaste = UnitFields.End + 0x716,
            ModPetHaste = UnitFields.End + 0x717,
            SummonedBattlePetGUID = UnitFields.End + 0x718,
            OverrideSpellsID = UnitFields.End + 0x71A,
            End = UnitFields.End + 0x71B
        };

        public enum PlayerDynamicArrays
        {
            ResearchSites = UnitDynamicArrays.End + 0x0,
            DailyQuestsCompleted = UnitDynamicArrays.End + 0x2,
            End = UnitDynamicArrays.End + 0x4
        }

        public enum GameObjectFields
        {
            CreatedBy = ObjectFields.End + 0x0,
            DisplayID = ObjectFields.End + 0x2,
            Flags = ObjectFields.End + 0x3,
            ParentRotation = ObjectFields.End + 0x4,
            AnimProgress = ObjectFields.End + 0x8,
            FactionTemplate = ObjectFields.End + 0x9,
            Level = ObjectFields.End + 0xA,
            PercentHealth = ObjectFields.End + 0xB,
            End = ObjectFields.End + 0xC
        };

        public enum GameObjectDynamicArrays
        {
            // One field, unknown
            UnknownField = DynamicObjectArrays.End + 0x0,
            End = DynamicObjectArrays.End + 0x1
        }

        public enum DynamicObjectFields
        {
            Caster = ObjectFields.End + 0x0,
            TypeAndVisualID = ObjectFields.End + 0x2,
            SpellId = ObjectFields.End + 0x3,
            Radius = ObjectFields.End + 0x4,
            CastTime = ObjectFields.End + 0x5,
            End = ObjectFields.End + 0x6
        };

        public enum DynamicObjectDynamicArrays
        {
            // Empty
            End = DynamicObjectArrays.End
        }

        public enum CorpseFields
        {
            Owner = ObjectFields.End + 0x0,
            PartyGuid = ObjectFields.End + 0x2,
            DisplayId = ObjectFields.End + 0x4,
            Items = ObjectFields.End + 0x5,
            SkinId = ObjectFields.End + 0x18,
            FacialHairStyleId = ObjectFields.End + 0x19,
            Flags = ObjectFields.End + 0x1A,
            DynamicFlags = ObjectFields.End + 0x1B,
            End = ObjectFields.End + 0x1C
        };

        public enum CorpseDynamicArrays
        {
            // Empty
            End = DynamicObjectArrays.End
        }

        public enum AreaTriggerFields
        {
            Caster = ObjectFields.End + 0x0,
            SpellId = ObjectFields.End + 0x2,
            SpellVisualId = ObjectFields.End + 0x3,
            Duration = ObjectFields.End + 0x4,
            End = ObjectFields.End + 0x5
        };

        public enum AreaTriggerDynamicArrays
        {
            // Empty
            End = DynamicObjectArrays.End
        }

        public enum SceneObjectFields
        {
            ScriptPackageId = ObjectFields.End + 0x0,
            RndSeedVal = ObjectFields.End + 0x1,
            CreatedBy = ObjectFields.End + 0x2,
            End = ObjectFields.End + 0x4
        };

        public enum SceneObjectDynamicArrays
        {
            // Empty
            End = DynamicObjectArrays.End
        }
    }
}
