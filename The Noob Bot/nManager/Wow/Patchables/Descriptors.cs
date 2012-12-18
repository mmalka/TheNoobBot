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
            Guid = 0,
            Data = 2,
            Type = 4,
            Entry = 5,
            Scale = 6,
            End = 7,
        }

        public enum ItemFields
        {
            Owner = ObjectFields.End + 0,
            ContainedIn = ObjectFields.End + 2,
            Creator = ObjectFields.End + 4,
            GiftCreator = ObjectFields.End + 6,
            StackCount = ObjectFields.End + 8,
            Expiration = ObjectFields.End + 9,
            SpellCharges = ObjectFields.End + 10,
            DynamicFlags = ObjectFields.End + 15,
            Enchantment = ObjectFields.End + 16,
            PropertySeed = ObjectFields.End + 55,
            RandomPropertiesID = ObjectFields.End + 56,
            Durability = ObjectFields.End + 57,
            MaxDurability = ObjectFields.End + 58,
            CreatePlayedTime = ObjectFields.End + 59,
            ModifiersMask = ObjectFields.End + 60,
            End = ObjectFields.End + 61,
        }

        public enum ContainerFields
        {
            Slots = ItemFields.End + 0,
            NumSlots = ItemFields.End + 72,
            End = ItemFields.End + 73,
        }

        public enum UnitFields
        {
            Charm = ObjectFields.End + 0,
            Summon = ObjectFields.End + 2,
            Critter = ObjectFields.End + 4,
            CharmedBy = ObjectFields.End + 6,
            SummonedBy = ObjectFields.End + 8,
            CreatedBy = ObjectFields.End + 10,
            Target = ObjectFields.End + 12,
            ChannelObject = ObjectFields.End + 14,
            ChannelSpell = ObjectFields.End + 16,
            SummonedByHomeRealm = ObjectFields.End + 17,
            DisplayPower = ObjectFields.End + 18,
            OverrideDisplayPowerID = ObjectFields.End + 19,
            Health = ObjectFields.End + 20,
            Power = ObjectFields.End + 21,
            MaxHealth = ObjectFields.End + 26,
            MaxPower = ObjectFields.End + 27,
            PowerRegenFlatModifier = ObjectFields.End + 32,
            PowerRegenInterruptedFlatModifier = ObjectFields.End + 37,
            Level = ObjectFields.End + 42,
            FactionTemplate = ObjectFields.End + 43,
            VirtualItemID = ObjectFields.End + 44,
            Flags = ObjectFields.End + 47,
            Flags2 = ObjectFields.End + 48,
            AuraState = ObjectFields.End + 49,
            AttackRoundBaseTime = ObjectFields.End + 50,
            RangedAttackRoundBaseTime = ObjectFields.End + 52,
            BoundingRadius = ObjectFields.End + 53,
            CombatReach = ObjectFields.End + 54,
            DisplayID = ObjectFields.End + 55,
            NativeDisplayID = ObjectFields.End + 56,
            MountDisplayID = ObjectFields.End + 57,
            MinDamage = ObjectFields.End + 58,
            MaxDamage = ObjectFields.End + 59,
            MinOffHandDamage = ObjectFields.End + 60,
            MaxOffHandDamage = ObjectFields.End + 61,
            AnimTier = ObjectFields.End + 62,
            PetNumber = ObjectFields.End + 63,
            PetNameTimestamp = ObjectFields.End + 64,
            PetExperience = ObjectFields.End + 65,
            PetNextLevelExperience = ObjectFields.End + 66,
            DynamicFlags = ObjectFields.End + 67,
            ModCastingSpeed = ObjectFields.End + 68,
            ModSpellHaste = ObjectFields.End + 69,
            ModHaste = ObjectFields.End + 70,
            ModHasteRegen = ObjectFields.End + 71,
            CreatedBySpell = ObjectFields.End + 72,
            NpcFlags = ObjectFields.End + 73,
            EmoteState = ObjectFields.End + 75,
            Stats = ObjectFields.End + 76,
            StatPosBuff = ObjectFields.End + 81,
            StatNegBuff = ObjectFields.End + 86,
            Resistances = ObjectFields.End + 91,
            ResistanceBuffModsPositive = ObjectFields.End + 98,
            ResistanceBuffModsNegative = ObjectFields.End + 105,
            BaseMana = ObjectFields.End + 112,
            BaseHealth = ObjectFields.End + 113,
            ShapeshiftForm = ObjectFields.End + 114,
            AttackPower = ObjectFields.End + 115,
            AttackPowerModPos = ObjectFields.End + 116,
            AttackPowerModNeg = ObjectFields.End + 117,
            AttackPowerMultiplier = ObjectFields.End + 118,
            RangedAttackPower = ObjectFields.End + 119,
            RangedAttackPowerModPos = ObjectFields.End + 120,
            RangedAttackPowerModNeg = ObjectFields.End + 121,
            RangedAttackPowerMultiplier = ObjectFields.End + 122,
            MinRangedDamage = ObjectFields.End + 123,
            MaxRangedDamage = ObjectFields.End + 124,
            PowerCostModifier = ObjectFields.End + 125,
            PowerCostMultiplier = ObjectFields.End + 132,
            MaxHealthModifier = ObjectFields.End + 139,
            HoverHeight = ObjectFields.End + 140,
            MinItemLevel = ObjectFields.End + 141,
            MaxItemLevel = ObjectFields.End + 142,
            WildBattlePetLevel = ObjectFields.End + 143,
            BattlePetCompanionGUID = ObjectFields.End + 144,
            BattlePetCompanionNameTimestamp = ObjectFields.End + 146,
            End = ObjectFields.End + 147,
        }

        public enum PlayerFields
        {
            DuelArbiter = UnitFields.End + 0,
            PlayerFlags = UnitFields.End + 2,
            GuildRankID = UnitFields.End + 3,
            GuildDeleteDate = UnitFields.End + 4,
            GuildLevel = UnitFields.End + 5,
            HairColorID = UnitFields.End + 6,
            RestState = UnitFields.End + 7,
            ArenaFaction = UnitFields.End + 8,
            DuelTeam = UnitFields.End + 9,
            GuildTimeStamp = UnitFields.End + 10,
            QuestLog = UnitFields.End + 11,
            VisibleItems = UnitFields.End + 761,
            PlayerTitle = UnitFields.End + 799,
            FakeInebriation = UnitFields.End + 800,
            HomePlayerRealm = UnitFields.End + 801,
            CurrentSpecID = UnitFields.End + 802,
            TaxiMountAnimKitID = UnitFields.End + 803,
            CurrentBattlePetBreedQuality = UnitFields.End + 804,
            InvSlots = UnitFields.End + 805,
            FarsightObject = UnitFields.End + 977,
            KnownTitles = UnitFields.End + 979,
            Coinage = UnitFields.End + 987,
            XP = UnitFields.End + 989,
            NextLevelXP = UnitFields.End + 990,
            Skill = UnitFields.End + 991,
            CharacterPoints = UnitFields.End + 1439,
            MaxTalentTiers = UnitFields.End + 1440,
            TrackCreatureMask = UnitFields.End + 1441,
            TrackResourceMask = UnitFields.End + 1442,
            Expertise = UnitFields.End + 1443,
            OffhandExpertise = UnitFields.End + 1444,
            RangedExpertise = UnitFields.End + 1445,
            BlockPercentage = UnitFields.End + 1446,
            DodgePercentage = UnitFields.End + 1447,
            ParryPercentage = UnitFields.End + 1448,
            CritPercentage = UnitFields.End + 1449,
            RangedCritPercentage = UnitFields.End + 1450,
            OffhandCritPercentage = UnitFields.End + 1451,
            SpellCritPercentage = UnitFields.End + 1452,
            ShieldBlock = UnitFields.End + 1459,
            ShieldBlockCritPercentage = UnitFields.End + 1460,
            Mastery = UnitFields.End + 1461,
            PvpPowerDamage = UnitFields.End + 1462,
            PvpPowerHealing = UnitFields.End + 1463,
            ExploredZones = UnitFields.End + 1464,
            RestStateBonusPool = UnitFields.End + 1664,
            ModDamageDonePos = UnitFields.End + 1665,
            ModDamageDoneNeg = UnitFields.End + 1672,
            ModDamageDonePercent = UnitFields.End + 1679,
            ModHealingDonePos = UnitFields.End + 1686,
            ModHealingPercent = UnitFields.End + 1687,
            ModHealingDonePercent = UnitFields.End + 1688,
            ModPeriodicHealingDonePercent = UnitFields.End + 1689,
            WeaponDmgMultipliers = UnitFields.End + 1690,
            ModSpellPowerPercent = UnitFields.End + 1693,
            ModResiliencePercent = UnitFields.End + 1694,
            OverrideSpellPowerByAPPercent = UnitFields.End + 1695,
            OverrideAPBySpellPowerPercent = UnitFields.End + 1696,
            ModTargetResistance = UnitFields.End + 1697,
            ModTargetPhysicalResistance = UnitFields.End + 1698,
            LifetimeMaxRank = UnitFields.End + 1699,
            SelfResSpell = UnitFields.End + 1700,
            PvpMedals = UnitFields.End + 1701,
            BuybackPrice = UnitFields.End + 1702,
            BuybackTimestamp = UnitFields.End + 1714,
            YesterdayHonorableKills = UnitFields.End + 1726,
            LifetimeHonorableKills = UnitFields.End + 1727,
            WatchedFactionIndex = UnitFields.End + 1728,
            CombatRatings = UnitFields.End + 1729,
            ArenaTeams = UnitFields.End + 1756,
            BattlegroundRating = UnitFields.End + 1777,
            MaxLevel = UnitFields.End + 1778,
            RuneRegen = UnitFields.End + 1779,
            NoReagentCostMask = UnitFields.End + 1783,
            GlyphSlots = UnitFields.End + 1787,
            Glyphs = UnitFields.End + 1793,
            GlyphSlotsEnabled = UnitFields.End + 1799,
            PetSpellPower = UnitFields.End + 1800,
            Researching = UnitFields.End + 1801,
            ProfessionSkillLine = UnitFields.End + 1809,
            UiHitModifier = UnitFields.End + 1811,
            UiSpellHitModifier = UnitFields.End + 1812,
            HomeRealmTimeOffset = UnitFields.End + 1813,
            ModRangedHaste = UnitFields.End + 1814,
            ModPetHaste = UnitFields.End + 1815,
            SummonedBattlePetGUID = UnitFields.End + 1816,
            OverrideSpellsID = UnitFields.End + 1818,
            End = UnitFields.End + 1819,
        }

        public enum GameObjectFields
        {
            CreatedBy = ObjectFields.End + 0,
            DisplayID = ObjectFields.End + 2,
            Flags = ObjectFields.End + 3,
            ParentRotation = ObjectFields.End + 4,
            AnimProgress = ObjectFields.End + 8,
            FactionTemplate = ObjectFields.End + 9,
            Level = ObjectFields.End + 10,
            PercentHealth = ObjectFields.End + 11,
            End = ObjectFields.End + 12,
        }

        public enum DynamicObjectFields
        {
            Caster = ObjectFields.End + 0,
            TypeAndVisualID = ObjectFields.End + 2,
            SpellID = ObjectFields.End + 3,
            Radius = ObjectFields.End + 4,
            CastTime = ObjectFields.End + 5,
            End = ObjectFields.End + 6,
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
            End = ObjectFields.End + 5,
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
            Modifiers,
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
