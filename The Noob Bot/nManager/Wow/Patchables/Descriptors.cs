namespace nManager.Wow.Patchables
{
    /// <summary>
    /// Descriptors
    /// </summary>
    public static class Descriptors
    {
        public const uint multiplicator = 4; // 4 or 1
        public static readonly uint startDescriptors = 0x4;

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
            End = ObjectFields.End + 61, // CGItemData_Size 
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
            BattlePetCompanionGUID = ObjectFields.End + 14,
            ChannelObject = ObjectFields.End + 16,
            ChannelSpell = ObjectFields.End + 18,
            SummonedByHomeRealm = ObjectFields.End + 19,
            DisplayPower = ObjectFields.End + 20,
            OverrideDisplayPowerID = ObjectFields.End + 21,
            Health = ObjectFields.End + 22,
            Power = ObjectFields.End + 23,
            MaxHealth = ObjectFields.End + 28,
            MaxPower = ObjectFields.End + 29,
            PowerRegenFlatModifier = ObjectFields.End + 34,
            PowerRegenInterruptedFlatModifier = ObjectFields.End + 39,
            Level = ObjectFields.End + 44,
            EffectiveLevel = ObjectFields.End + 45,
            FactionTemplate = ObjectFields.End + 46,
            VirtualItemID = ObjectFields.End + 47,
            Flags = ObjectFields.End + 50,
            Flags2 = ObjectFields.End + 51,
            AuraState = ObjectFields.End + 52,
            AttackRoundBaseTime = ObjectFields.End + 53,
            RangedAttackRoundBaseTime = ObjectFields.End + 55,
            BoundingRadius = ObjectFields.End + 56,
            CombatReach = ObjectFields.End + 57,
            DisplayID = ObjectFields.End + 58,
            NativeDisplayID = ObjectFields.End + 59,
            MountDisplayID = ObjectFields.End + 60,
            MinDamage = ObjectFields.End + 61,
            MaxDamage = ObjectFields.End + 62,
            MinOffHandDamage = ObjectFields.End + 63,
            MaxOffHandDamage = ObjectFields.End + 64,
            AnimTier = ObjectFields.End + 65,
            PetNumber = ObjectFields.End + 66,
            PetNameTimestamp = ObjectFields.End + 67,
            PetExperience = ObjectFields.End + 68,
            PetNextLevelExperience = ObjectFields.End + 69,
            ModCastingSpeed = ObjectFields.End + 70,
            ModSpellHaste = ObjectFields.End + 71,
            ModHaste = ObjectFields.End + 72,
            ModHasteRegen = ObjectFields.End + 73,
            CreatedBySpell = ObjectFields.End + 74,
            NpcFlagUMNW0 = ObjectFields.End + 75,
            NpcFlags = ObjectFields.End + 76,
            EmoteState = ObjectFields.End + 77,
            Stats = ObjectFields.End + 78,
            StatPosBuff = ObjectFields.End + 83,
            StatNegBuff = ObjectFields.End + 88,
            Resistances = ObjectFields.End + 93,
            ResistanceBuffModsPositive = ObjectFields.End + 100,
            ResistanceBuffModsNegative = ObjectFields.End + 107,
            BaseMana = ObjectFields.End + 114,
            BaseHealth = ObjectFields.End + 115,
            ShapeshiftForm = ObjectFields.End + 116,
            AttackPower = ObjectFields.End + 117,
            AttackPowerModPos = ObjectFields.End + 118,
            AttackPowerModNeg = ObjectFields.End + 119,
            AttackPowerMultiplier = ObjectFields.End + 120,
            RangedAttackPower = ObjectFields.End + 121,
            RangedAttackPowerModPos = ObjectFields.End + 122,
            RangedAttackPowerModNeg = ObjectFields.End + 123,
            RangedAttackPowerMultiplier = ObjectFields.End + 124,
            MinRangedDamage = ObjectFields.End + 125,
            MaxRangedDamage = ObjectFields.End + 126,
            PowerCostModifier = ObjectFields.End + 127,
            PowerCostMultiplier = ObjectFields.End + 134,
            MaxHealthModifier = ObjectFields.End + 141,
            HoverHeight = ObjectFields.End + 142,
            MinItemLevel = ObjectFields.End + 143,
            MaxItemLevel = ObjectFields.End + 144,
            WildBattlePetLevel = ObjectFields.End + 145,
            BattlePetCompanionNameTimestamp = ObjectFields.End + 146,
            End = ObjectFields.End + 147, // CGUnitData_Size
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
            VirtualPlayerRealm = UnitFields.End + 801,
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
            MainhandExpertise = UnitFields.End + 1443,
            RangedExpertise = UnitFields.End + 1444,
            OffhandExpertise = UnitFields.End + 1445,
            CombatRatingExpertise = UnitFields.End + 1446,
            BlockPercentage = UnitFields.End + 1447,
            DodgePercentage = UnitFields.End + 1448,
            ParryPercentage = UnitFields.End + 1449,
            CritPercentage = UnitFields.End + 1450,
            RangedCritPercentage = UnitFields.End + 1451,
            OffhandCritPercentage = UnitFields.End + 1452,
            SpellCritPercentage = UnitFields.End + 1453,
            ShieldBlock = UnitFields.End + 1460,
            ShieldBlockCritPercentage = UnitFields.End + 1461,
            Mastery = UnitFields.End + 1462,
            PvpPowerDamage = UnitFields.End + 1463,
            PvpPowerHealing = UnitFields.End + 1464,
            ExploredZones = UnitFields.End + 1465,
            RestStateBonusPool = UnitFields.End + 1665,
            ModDamageDonePos = UnitFields.End + 1666,
            ModDamageDoneNeg = UnitFields.End + 1673,
            ModDamageDonePercent = UnitFields.End + 1680,
            ModHealingDonePos = UnitFields.End + 1687,
            ModHealingPercent = UnitFields.End + 1688,
            ModHealingDonePercent = UnitFields.End + 1689,
            ModPeriodicHealingDonePercent = UnitFields.End + 1690,
            WeaponDmgMultipliers = UnitFields.End + 1691,
            ModSpellPowerPercent = UnitFields.End + 1694,
            ModResiliencePercent = UnitFields.End + 1695,
            OverrideSpellPowerByAPPercent = UnitFields.End + 1696,
            OverrideAPBySpellPowerPercent = UnitFields.End + 1697,
            ModTargetResistance = UnitFields.End + 1698,
            ModTargetPhysicalResistance = UnitFields.End + 1699,
            LifetimeMaxRank = UnitFields.End + 1700,
            SelfResSpell = UnitFields.End + 1701,
            PvpMedals = UnitFields.End + 1702,
            BuybackPrice = UnitFields.End + 1703,
            BuybackTimestamp = UnitFields.End + 1715,
            YesterdayHonorableKills = UnitFields.End + 1727,
            LifetimeHonorableKills = UnitFields.End + 1728,
            WatchedFactionIndex = UnitFields.End + 1729,
            CombatRatings = UnitFields.End + 1730,
            ArenaTeams = UnitFields.End + 1757,
            BattlegroundRating = UnitFields.End + 1778,
            MaxLevel = UnitFields.End + 1779,
            RuneRegen = UnitFields.End + 1780,
            NoReagentCostMask = UnitFields.End + 1784,
            GlyphSlots = UnitFields.End + 1788,
            Glyphs = UnitFields.End + 1794,
            GlyphSlotsEnabled = UnitFields.End + 1800,
            PetSpellPower = UnitFields.End + 1801,
            Researching = UnitFields.End + 1802,
            ProfessionSkillLine = UnitFields.End + 1810,
            UiHitModifier = UnitFields.End + 1812,
            UiSpellHitModifier = UnitFields.End + 1813,
            HomeRealmTimeOffset = UnitFields.End + 1814,
            ModRangedHaste = UnitFields.End + 1815,
            ModPetHaste = UnitFields.End + 1816,
            SummonedBattlePetGUID = UnitFields.End + 1817,
            OverrideSpellsID = UnitFields.End + 1819,
            LfgBonusFactionID = UnitFields.End + 1820,
            LootSpecID = UnitFields.End + 1821,
            OverrideZonePVPType = UnitFields.End + 1822,
            ItemLevelDelta = UnitFields.End + 1823,
            End = UnitFields.End + 1824, // CGPlayerData_Size
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
            Caster2 = ObjectFields.End + 5,
            Duration2 = ObjectFields.End + 7,
            SpellID2 = ObjectFields.End + 8,
            SpellVisualID2 = ObjectFields.End + 9,
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