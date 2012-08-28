namespace nManager.Wow.Patchables
{
    /// <summary>
    /// Descriptors
    /// </summary>
    public class Descriptors
    {
        public const uint multiplicator = 4; // 4 or 1
        public static readonly uint startDescriptors = 0x8;

        public enum ObjectFields
        {
            guid = 0x0,
            data = 0x2,
            type = 0x4,
            entryID = 0x5,
            scale = 0x6,
            End = 0x7
        };

        public enum ItemFields
        {
            owner = ObjectFields.End + 0x0,
            containedIn = ObjectFields.End + 0x2,
            creator = ObjectFields.End + 0x4,
            giftCreator = ObjectFields.End + 0x6,
            stackCount = ObjectFields.End + 0x8,
            expiration = ObjectFields.End + 0x9,
            spellCharges = ObjectFields.End + 0xA,
            dynamicFlags = ObjectFields.End + 0xF,
            enchantment = ObjectFields.End + 0x10,
            propertySeed = ObjectFields.End + 0x37,
            randomPropertiesID = ObjectFields.End + 0x38,
            durability = ObjectFields.End + 0x39,
            maxDurability = ObjectFields.End + 0x3A,
            createPlayedTime = ObjectFields.End + 0x3B,
            modifiersMask = ObjectFields.End + 0x3C,
            End = ObjectFields.End + 0x3D
        };

        public enum ItemDynamicFields
        {
            modifiers = ObjectFields.End + 0x0,
            End = ObjectFields.End + 0x4
        }

        public enum ContainerFields
        {
            numSlots = ItemFields.End + 0x0,
            slots = ItemFields.End + 0x1,
            End = ItemFields.End + 0x49
        };

        public enum UnitFields
        {
            charm = ObjectFields.End + 0x0,
            summon = ObjectFields.End + 0x2,
            critter = ObjectFields.End + 0x4,
            charmedBy = ObjectFields.End + 0x6,
            summonedBy = ObjectFields.End + 0x8,
            createdBy = ObjectFields.End + 0xA,
            target = ObjectFields.End + 0xC,
            channelObject = ObjectFields.End + 0xE,
            summonedByHomeRealm = ObjectFields.End + 0x10,
            channelSpell = ObjectFields.End + 0x11,
            displayPower = ObjectFields.End + 0x12,
            overrideDisplayPowerID = ObjectFields.End + 0x13,
            health = ObjectFields.End + 0x14,
            power = ObjectFields.End + 0x15,
            maxHealth = ObjectFields.End + 0x1A,
            maxPower = ObjectFields.End + 0x1B,
            powerRegenFlatModifier = ObjectFields.End + 0x20,
            powerRegenInterruptedFlatModifier = ObjectFields.End + 0x25,
            level = ObjectFields.End + 0x2A,
            factionTemplate = ObjectFields.End + 0x2B,
            virtualItemID = ObjectFields.End + 0x2C,
            flags = ObjectFields.End + 0x2F,
            flags2 = ObjectFields.End + 0x30,
            auraState = ObjectFields.End + 0x31,
            attackRoundBaseTime = ObjectFields.End + 0x32,
            rangedAttackRoundBaseTime = ObjectFields.End + 0x34,
            boundingRadius = ObjectFields.End + 0x35,
            combatReach = ObjectFields.End + 0x36,
            displayID = ObjectFields.End + 0x37,
            nativeDisplayID = ObjectFields.End + 0x38,
            mountDisplayID = ObjectFields.End + 0x39,
            minDamage = ObjectFields.End + 0x3A,
            maxDamage = ObjectFields.End + 0x3B,
            minOffHandDamage = ObjectFields.End + 0x3C,
            maxOffHandDamage = ObjectFields.End + 0x3D,
            animTier = ObjectFields.End + 0x3E,
            petNumber = ObjectFields.End + 0x3F,
            petNameTimestamp = ObjectFields.End + 0x40,
            petExperience = ObjectFields.End + 0x41,
            petNextLevelExperience = ObjectFields.End + 0x42,
            dynamicFlags = ObjectFields.End + 0x43,
            modCastingSpeed = ObjectFields.End + 0x44,
            modSpellHaste = ObjectFields.End + 0x45,
            modHaste = ObjectFields.End + 0x46,
            modHasteRegen = ObjectFields.End + 0x47,
            createdBySpell = ObjectFields.End + 0x48,
            npcFlags = ObjectFields.End + 0x49,
            emoteState = ObjectFields.End + 0x4B,
            stats = ObjectFields.End + 0x4C,
            statPosBuff = ObjectFields.End + 0x51,
            statNegBuff = ObjectFields.End + 0x56,
            resistances = ObjectFields.End + 0x5B,
            resistanceBuffModsPositive = ObjectFields.End + 0x62,
            resistanceBuffModsNegative = ObjectFields.End + 0x69,
            baseMana = ObjectFields.End + 0x70,
            baseHealth = ObjectFields.End + 0x71,
            shapeshiftForm = ObjectFields.End + 0x72,
            attackPower = ObjectFields.End + 0x73,
            attackPowerModPos = ObjectFields.End + 0x74,
            attackPowerModNeg = ObjectFields.End + 0x75,
            attackPowerMultiplier = ObjectFields.End + 0x76,
            rangedAttackPower = ObjectFields.End + 0x77,
            rangedAttackPowerModPos = ObjectFields.End + 0x78,
            rangedAttackPowerModNeg = ObjectFields.End + 0x79,
            rangedAttackPowerMultiplier = ObjectFields.End + 0x7A,
            minRangedDamage = ObjectFields.End + 0x7B,
            maxRangedDamage = ObjectFields.End + 0x7C,
            powerCostModifier = ObjectFields.End + 0x7D,
            powerCostMultiplier = ObjectFields.End + 0x84,
            maxHealthModifier = ObjectFields.End + 0x8B,
            hoverHeight = ObjectFields.End + 0x8C,
            minItemLevel = ObjectFields.End + 0x8D,
            maxItemLevel = ObjectFields.End + 0x8E,
            wildBattlePetLevel = ObjectFields.End + 0x8F,
            battlePetCompanionID = ObjectFields.End + 0x90,
            battlePetCompanionNameTimestamp = ObjectFields.End + 0x91,
            End = ObjectFields.End + 0x92
        };

        public enum UnitDynamicFields
        {
            passiveSpells = ObjectFields.End + 0x0,
            End = ObjectFields.End + 0x101
        }

        public enum PlayerFields
        {
            duelArbiter = UnitFields.End + 0x0,
            playerFlags = UnitFields.End + 0x2,
            guildRankID = UnitFields.End + 0x3,
            guildDeleteDate = UnitFields.End + 0x4,
            guildLevel = UnitFields.End + 0x5,
            hairColorID = UnitFields.End + 0x6,
            restState = UnitFields.End + 0x7,
            arenaFaction = UnitFields.End + 0x8,
            duelTeam = UnitFields.End + 0x9,
            guildTimeStamp = UnitFields.End + 0xA,
            questLog = UnitFields.End + 0xB,
            visibleItems = UnitFields.End + 0x2F9,
            playerTitle = UnitFields.End + 0x31F,
            fakeInebriation = UnitFields.End + 0x320,
            homePlayerRealm = UnitFields.End + 0x321,
            currentSpecID = UnitFields.End + 0x322,
            taxiMountAnimKitID = UnitFields.End + 0x323,
            partyType = UnitFields.End + 0x324,
            invSlots = UnitFields.End + 0x325,
            farsightObject = UnitFields.End + 0x3D1,
            knownTitles = UnitFields.End + 0x3D3,
            local_XP = UnitFields.End + 0x3DB,
            local_nextLevelXP = UnitFields.End + 0x3DC,
            skill = UnitFields.End + 0x3DD,
            characterPoints = UnitFields.End + 0x59D,
            maxTalentTiers = UnitFields.End + 0x59E,
            local_trackCreatureMask = UnitFields.End + 0x59F,
            local_trackResourceMask = UnitFields.End + 0x5A0,
            expertise = UnitFields.End + 0x5A1,
            offhandExpertise = UnitFields.End + 0x5A2,
            rangedExpertise = UnitFields.End + 0x5A3,
            blockPercentage = UnitFields.End + 0x5A4,
            dodgePercentage = UnitFields.End + 0x5A5,
            parryPercentage = UnitFields.End + 0x5A6,
            critPercentage = UnitFields.End + 0x5A7,
            rangedCritPercentage = UnitFields.End + 0x5A8,
            offhandCritPercentage = UnitFields.End + 0x5A9,
            spellCritPercentage = UnitFields.End + 0x5AA,
            shieldBlock = UnitFields.End + 0x5B1,
            shieldBlockCritPercentage = UnitFields.End + 0x5B2,
            mastery = UnitFields.End + 0x5B3,
            pvpPower = UnitFields.End + 0x5B4,
            exploredZones = UnitFields.End + 0x5B5,
            restStateBonusPool = UnitFields.End + 0x67D,
            coinage = UnitFields.End + 0x67E,
            modDamageDonePos = UnitFields.End + 0x680,
            modDamageDoneNeg = UnitFields.End + 0x687,
            modDamageDonePercent = UnitFields.End + 0x68E,
            modHealingDonePos = UnitFields.End + 0x695,
            modHealingPercent = UnitFields.End + 0x696,
            modHealingDonePercent = UnitFields.End + 0x697,
            modPeriodicHealingDonePercent = UnitFields.End + 0x698,
            weaponDmgMultipliers = UnitFields.End + 0x699,
            modSpellPowerPercent = UnitFields.End + 0x69C,
            modResiliencePercent = UnitFields.End + 0x69D,
            overrideSpellPowerByAPPercent = UnitFields.End + 0x69E,
            overrideAPBySpellPowerPercent = UnitFields.End + 0x69F,
            modTargetResistance = UnitFields.End + 0x6A0,
            modTargetPhysicalResistance = UnitFields.End + 0x6A1,
            lifetimeMaxRank = UnitFields.End + 0x6A2,
            selfResSpell = UnitFields.End + 0x6A3,
            pvpMedals = UnitFields.End + 0x6A4,
            buybackPrice = UnitFields.End + 0x6A5,
            buybackTimestamp = UnitFields.End + 0x6B1,
            yesterdayHonorableKills = UnitFields.End + 0x6BD,
            lifetimeHonorableKills = UnitFields.End + 0x6BE,
            watchedFactionIndex = UnitFields.End + 0x6BF,
            combatRatings = UnitFields.End + 0x6C0,
            arenaTeams = UnitFields.End + 0x6DB,
            battlegroundRating = UnitFields.End + 0x6F0,
            maxLevel = UnitFields.End + 0x6F1,
            runeRegen = UnitFields.End + 0x6F2,
            noReagentCostMask = UnitFields.End + 0x6F6,
            glyphSlots = UnitFields.End + 0x6FA,
            glyphs = UnitFields.End + 0x700,
            glyphSlotsEnabled = UnitFields.End + 0x706,
            petSpellPower = UnitFields.End + 0x707,
            researching = UnitFields.End + 0x708,
            local_professionSkillLine = UnitFields.End + 0x710,
            uiHitModifier = UnitFields.End + 0x712,
            uiSpellHitModifier = UnitFields.End + 0x713,
            homeRealmTimeOffset = UnitFields.End + 0x714,
            modRangedHaste = UnitFields.End + 0x715,
            modPetHaste = UnitFields.End + 0x716,
            summonedBattlePetID = UnitFields.End + 0x717,
            auraVision = UnitFields.End + 0x718,
            overrideSpellsID = UnitFields.End + 0x719,
            End = UnitFields.End + 0x71A
        };

        public enum PlayerDynamicFields
        {
            researchSites = UnitFields.End + 0x0,
            dailyQuestsCompleted = UnitFields.End + 0x2,
            End = UnitFields.End + 0x4
        }

        public enum GameObjectFields
        {
            createdBy = ObjectFields.End + 0x0,
            displayID = ObjectFields.End + 0x2,
            flags = ObjectFields.End + 0x3,
            parentRotation = ObjectFields.End + 0x4,
            animProgress = ObjectFields.End + 0x8,
            factionTemplate = ObjectFields.End + 0x9,
            level = ObjectFields.End + 0xA,
            percentHealth = ObjectFields.End + 0xB,
            End = ObjectFields.End + 0xC
        };

        public enum DynamicObjectFields
        {
            caster = ObjectFields.End + 0x0,
            typeAndVisualID = ObjectFields.End + 0x2,
            spellId = ObjectFields.End + 0x3,
            radius = ObjectFields.End + 0x4,
            castTime = ObjectFields.End + 0x5,
            End = ObjectFields.End + 0x6
        };

        public enum CorpseFields
        {
            owner = ObjectFields.End + 0x0,
            partyGuid = ObjectFields.End + 0x2,
            displayId = ObjectFields.End + 0x4,
            items = ObjectFields.End + 0x5,
            skinId = ObjectFields.End + 0x18,
            facialHairStyleId = ObjectFields.End + 0x19,
            flags = ObjectFields.End + 0x1A,
            dynamicFlags = ObjectFields.End + 0x1B,
            End = ObjectFields.End + 0x1C
        };

        public enum AreaTriggerFields
        {
            caster = ObjectFields.End + 0x0,
            spellId = ObjectFields.End + 0x2,
            spellVisualId = ObjectFields.End + 0x3,
            duration = ObjectFields.End + 0x4,
            End = ObjectFields.End + 0x5
        };

        public enum SceneObjectFields
        {
            scriptPackageId = ObjectFields.End + 0x0,
            rndSeedVal = ObjectFields.End + 0x1,
            createdBy = ObjectFields.End + 0x2,
            End = ObjectFields.End + 0x4
        };

    }
}
