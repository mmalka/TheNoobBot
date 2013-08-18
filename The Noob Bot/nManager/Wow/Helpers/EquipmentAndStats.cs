using System;
using System.Collections.Generic;
using nManager.Wow.Enums;

namespace nManager.Wow.Helpers
{
    public class EquipmentAndStats
    {
        internal static WowItemSubClassArmor InternalEquipableArmorItemType = WowItemSubClassArmor.Cloth;
        internal static List<WowItemSubClassWeapon> InternalEquipableWeapons = new List<WowItemSubClassWeapon>();
        internal static List<WoWStatistic> InternalEquipementStats = new List<WoWStatistic>();
        internal static bool InternalHasShield = false;

        public static WowItemSubClassArmor EquipableArmorItemType
        {
            get { return InternalEquipableArmorItemType; }
        }
        public static List<WowItemSubClassWeapon> EquipableWeapons
        {
            get { return InternalEquipableWeapons; }
        }
        public static List<WoWStatistic> EquipementStats
        {
            get { return InternalEquipementStats; }
        }
        public static bool HasShield
        {
            get { return InternalHasShield; }
        }

        public static void SetPlayerSpe(WoWSpecialization spe)
        {
            InternalEquipableWeapons.Clear();
            InternalEquipementStats.Clear();

            // Template lists
            List<WoWStatistic> IntelBased = new List<WoWStatistic>() {
                WoWStatistic.INTELLECT,
                WoWStatistic.STAMINA,
                WoWStatistic.HASTE_RATING,
                WoWStatistic.CRIT_RATING,
                WoWStatistic.SPELL_POWER,
                WoWStatistic.MASTERY_RATING };
            List<WoWStatistic> Heal = new List<WoWStatistic>() {
                WoWStatistic.SPIRIT };
            Heal.AddRange(IntelBased);
            List<WoWStatistic> MagicDPS = new List<WoWStatistic>() {
                WoWStatistic.HIT_RATING };
            MagicDPS.AddRange(IntelBased);
            List<WoWStatistic> AgilityBased = new List<WoWStatistic>() {
                WoWStatistic.AGILITY,
                WoWStatistic.STAMINA,
                WoWStatistic.HASTE_RATING,
                WoWStatistic.CRIT_RATING,
                WoWStatistic.ATTACK_POWER,
                WoWStatistic.MASTERY_RATING,
                WoWStatistic.HIT_RATING,
                WoWStatistic.EXPERTISE_RATING };
            List<WoWStatistic> StrenghBased = new List<WoWStatistic>() {
                WoWStatistic.STRENGTH,
                WoWStatistic.STAMINA,
                WoWStatistic.HASTE_RATING,
                WoWStatistic.MASTERY_RATING,
                WoWStatistic.HIT_RATING,
                WoWStatistic.EXPERTISE_RATING };
            List<WoWStatistic> StrenghDPS = new List<WoWStatistic>() {
                WoWStatistic.CRIT_RATING,
                WoWStatistic.ATTACK_POWER };
            StrenghDPS.AddRange(StrenghBased);

            // Armor and weapons
            switch (spe)
            {
                case WoWSpecialization.DeathknightBlood:
                case WoWSpecialization.DeathknightUnholy:
                case WoWSpecialization.DeathknightFrost:
                    InternalEquipableArmorItemType = WowItemSubClassArmor.Plate;
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Axe2);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Sword2);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Mace2);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Polearm);
                    break;
                case WoWSpecialization.MageArcane:
                case WoWSpecialization.MageFire:
                case WoWSpecialization.MageFrost:
                    InternalEquipableArmorItemType = WowItemSubClassArmor.Cloth;
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Staff);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Wand);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Sword);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Dagger); // + OffHand
                    break;
                case WoWSpecialization.WarlockDemonology:
                case WoWSpecialization.WarlockAffliction:
                case WoWSpecialization.WarlockDestruction:
                    InternalEquipableArmorItemType = WowItemSubClassArmor.Cloth;
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Staff);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Wand);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Sword);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Dagger); // + OffHand
                    break;
                case WoWSpecialization.DruidFeral:
                case WoWSpecialization.DruidGuardian:
                case WoWSpecialization.DruidBalance:
                case WoWSpecialization.DruidRestoration:
                    InternalEquipableArmorItemType = WowItemSubClassArmor.Leather;
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Staff);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Polearm);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Mace);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Dagger);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Fist); // + OffHand
                    break;
                case WoWSpecialization.PaladinRetribution:
                    if (ObjectManager.ObjectManager.Me.Level < 40)
                        InternalEquipableArmorItemType = WowItemSubClassArmor.Mail;
                    else
                        InternalEquipableArmorItemType = WowItemSubClassArmor.Plate;
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Axe2);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Sword2);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Mace2);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Polearm);
                    break;
                case WoWSpecialization.PaladinProtection:
                case WoWSpecialization.PaladinHoly:
                    if (ObjectManager.ObjectManager.Me.Level < 40)
                        InternalEquipableArmorItemType = WowItemSubClassArmor.Mail;
                    else
                        InternalEquipableArmorItemType = WowItemSubClassArmor.Plate;
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Mace);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Axe);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Sword);
                    InternalHasShield = true;
                    break;
                case WoWSpecialization.ShamanEnhancement:
                case WoWSpecialization.ShamanElemental:
                case WoWSpecialization.ShamanRestoration:
                    if (ObjectManager.ObjectManager.Me.Level < 40)
                        InternalEquipableArmorItemType = WowItemSubClassArmor.Leather;
                    else
                        InternalEquipableArmorItemType = WowItemSubClassArmor.Mail;
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Axe2);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Mace2);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Polearm);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Staff);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Axe);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Mace);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Dagger);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Fist);
                    InternalHasShield = true;
                    break;
                case WoWSpecialization.PriestShadow:
                case WoWSpecialization.PriestDiscipline:
                case WoWSpecialization.PriestHoly:
                    InternalEquipableArmorItemType = WowItemSubClassArmor.Cloth;
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Staff);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Wand);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Dagger);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Mace); // + OffHand
                    break;
                case WoWSpecialization.RogueCombat:
                case WoWSpecialization.RogueAssassination:
                case WoWSpecialization.RogueSubtlety:
                    InternalEquipableArmorItemType = WowItemSubClassArmor.Leather;
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Axe);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Mace);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Sword);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Dagger);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Fist); // + Bows, Crossbows, Guns, Thrown
                    break;
                case WoWSpecialization.WarriorArms:
                case WoWSpecialization.WarriorFury:
                    if (ObjectManager.ObjectManager.Me.Level < 40)
                        InternalEquipableArmorItemType = WowItemSubClassArmor.Mail;
                    else
                        InternalEquipableArmorItemType = WowItemSubClassArmor.Plate;
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Axe2);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Sword2);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Mace2);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Polearm); // + Staf, Dagger, Fist, Bow, Crossbow, Gun, Thrown 
                    break;
                case WoWSpecialization.WarriorProtection:
                    if (ObjectManager.ObjectManager.Me.Level < 40)
                        InternalEquipableArmorItemType = WowItemSubClassArmor.Mail;
                    else
                        InternalEquipableArmorItemType = WowItemSubClassArmor.Plate;
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Mace);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Axe);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Sword);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Dagger);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Fist);
                    InternalHasShield = true;
                    break;
                case WoWSpecialization.HunterMarksmanship:
                case WoWSpecialization.HunterSurvival:
                case WoWSpecialization.HunterBeastMastery:
                    if (ObjectManager.ObjectManager.Me.Level < 40)
                        InternalEquipableArmorItemType = WowItemSubClassArmor.Leather;
                    else
                        InternalEquipableArmorItemType = WowItemSubClassArmor.Mail;
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Gun);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Bow);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Crossbow); // + lots of useless weapons
                    break;
                case WoWSpecialization.MonkBrewmaster:
                case WoWSpecialization.MonkWindwalker:
                case WoWSpecialization.MonkMistweaver:
                    InternalEquipableArmorItemType = WowItemSubClassArmor.Leather;
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Polearm);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Staff);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Axe);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Sword);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Mace);
                    break;
                default:
                    InternalEquipableArmorItemType = WowItemSubClassArmor.Cloth;
                    break;
            }

            // Statistics
            switch (spe)
            {
                case WoWSpecialization.DeathknightBlood:
                    InternalEquipementStats.AddRange(StrenghBased);
                    InternalEquipementStats.Add(WoWStatistic.PARRY_RATING);
                    InternalEquipementStats.Add(WoWStatistic.DODGE_RATING);
                    break;
                case WoWSpecialization.DeathknightUnholy:
                case WoWSpecialization.DeathknightFrost:
                    InternalEquipementStats.AddRange(StrenghDPS);
                    break;
                case WoWSpecialization.MageArcane:
                case WoWSpecialization.MageFire:
                case WoWSpecialization.MageFrost:
                    InternalEquipementStats.AddRange(MagicDPS);
                    break;
                case WoWSpecialization.WarlockDemonology:
                case WoWSpecialization.WarlockAffliction:
                case WoWSpecialization.WarlockDestruction:
                    InternalEquipementStats.AddRange(MagicDPS);
                    break;
                case WoWSpecialization.DruidFeral:
                    InternalEquipementStats.AddRange(AgilityBased);
                    break;
                case WoWSpecialization.DruidGuardian:
                    InternalEquipementStats.AddRange(AgilityBased);
                    InternalEquipementStats.Add(WoWStatistic.DODGE_RATING);
                    break;
                case WoWSpecialization.DruidBalance:
                    InternalEquipementStats.AddRange(MagicDPS);
                    break;
                case WoWSpecialization.DruidRestoration:
                    InternalEquipementStats.AddRange(Heal);
                    break;
                case WoWSpecialization.PaladinRetribution:
                    InternalEquipementStats.AddRange(StrenghDPS);
                    break;
                case WoWSpecialization.PaladinProtection:
                    InternalEquipementStats.AddRange(StrenghBased);
                    InternalEquipementStats.Add(WoWStatistic.PARRY_RATING);
                    InternalEquipementStats.Add(WoWStatistic.DODGE_RATING);
                    break;
                case WoWSpecialization.PaladinHoly:
                    InternalEquipementStats.AddRange(Heal);
                    break;
                case WoWSpecialization.ShamanEnhancement:
                    InternalEquipementStats.AddRange(AgilityBased);
                    break;
                case WoWSpecialization.ShamanElemental:
                    InternalEquipementStats.AddRange(MagicDPS);
                    InternalEquipementStats.Add(WoWStatistic.SPIRIT);
                    break;
                case WoWSpecialization.ShamanRestoration:
                    InternalEquipementStats.AddRange(Heal);
                    break;
                case WoWSpecialization.PriestShadow:
                    InternalEquipementStats.AddRange(MagicDPS);
                    InternalEquipementStats.Add(WoWStatistic.SPIRIT);
                    break;
                case WoWSpecialization.PriestDiscipline:
                case WoWSpecialization.PriestHoly:
                    InternalEquipementStats.AddRange(Heal);
                    break;
                case WoWSpecialization.RogueCombat:
                case WoWSpecialization.RogueAssassination:
                case WoWSpecialization.RogueSubtlety:
                    InternalEquipementStats.AddRange(AgilityBased);
                    break;
                case WoWSpecialization.WarriorArms:
                case WoWSpecialization.WarriorFury:
                    InternalEquipementStats.AddRange(StrenghDPS);
                    break;
                case WoWSpecialization.WarriorProtection:
                    InternalEquipementStats.AddRange(StrenghBased);
                    InternalEquipementStats.Add(WoWStatistic.PARRY_RATING);
                    InternalEquipementStats.Add(WoWStatistic.DODGE_RATING);
                    InternalEquipementStats.Add(WoWStatistic.CRIT_RATING);
                    break;
                case WoWSpecialization.HunterMarksmanship:
                case WoWSpecialization.HunterSurvival:
                case WoWSpecialization.HunterBeastMastery:
                    InternalEquipementStats.AddRange(AgilityBased);
                    break;
                case WoWSpecialization.MonkBrewmaster:
                    InternalEquipementStats.AddRange(AgilityBased);
                    InternalEquipementStats.Add(WoWStatistic.DODGE_RATING);
                    break;
                case WoWSpecialization.MonkWindwalker:
                    InternalEquipementStats.AddRange(AgilityBased);
                    break;
                case WoWSpecialization.MonkMistweaver:
                    InternalEquipementStats.AddRange(Heal);
                    break;
                default:
                    break;
            }
        }
    }
}
