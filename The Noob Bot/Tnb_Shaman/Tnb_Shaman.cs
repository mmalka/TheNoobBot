/*
* CombatClass for TheNoobBot
* Credit : Vesper, Neo2003, Dreadlocks
* Thanks you !
*/

using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using nManager.Helpful;
using nManager.Wow;
using nManager.Wow.Bot.Tasks;
using nManager.Wow.Class;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;
using Timer = nManager.Helpful.Timer;

// ReSharper disable EmptyGeneralCatchClause
// ReSharper disable ObjectCreationAsStatement

public class Main : ICombatClass
{
    internal static float InternalRange = 5.0f;
    internal static float InternalAggroRange = 5.0f;
    internal static bool InternalLoop = true;

    #region ICombatClass Members

    public float AggroRange
    {
        get { return InternalAggroRange; }
    }

    public float Range
    {
        get { return InternalRange; }
        set { InternalRange = value; }
    }

    public void Initialize()
    {
        Initialize(false);
    }

    public void Dispose()
    {
        Logging.WriteFight("Combat system stopped.");
        InternalLoop = false;
    }

    public void ShowConfiguration()
    {
        Directory.CreateDirectory(Application.StartupPath + "\\CombatClasses\\Settings\\");
        Initialize(true);
    }

    public void ResetConfiguration()
    {
        Directory.CreateDirectory(Application.StartupPath + "\\CombatClasses\\Settings\\");
        Initialize(true, true);
    }

    #endregion

    public void Initialize(bool configOnly, bool resetSettings = false)
    {
        try
        {
            if (!InternalLoop)
                InternalLoop = true;
            Logging.WriteFight("Loading combat system.");
            WoWSpecialization wowSpecialization = ObjectManager.Me.WowSpecialization(true);
            switch (ObjectManager.Me.WowClass)
            {
                    #region Shaman Specialisation checking

                case WoWClass.Shaman:

                    if (wowSpecialization == WoWSpecialization.ShamanEnhancement)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Shaman_Enhancement.xml";
                            var currentSetting = new ShamanEnhancement.ShamanEnhancementSettings();
                            if (File.Exists(currentSettingsFile) && !resetSettings)
                            {
                                currentSetting = Settings.Load<ShamanEnhancement.ShamanEnhancementSettings>(currentSettingsFile);
                            }
                            currentSetting.ToForm();
                            currentSetting.Save(currentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Shaman Enhancement Combat class...");
                            EquipmentAndStats.SetPlayerSpe(WoWSpecialization.ShamanEnhancement);
                            new ShamanEnhancement();
                        }
                        break;
                    }
                    if (wowSpecialization == WoWSpecialization.ShamanElemental || wowSpecialization == WoWSpecialization.None)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Shaman_Elemental.xml";
                            var currentSetting = new ShamanElemental.ShamanElementalSettings();
                            if (File.Exists(currentSettingsFile) && !resetSettings)
                            {
                                currentSetting = Settings.Load<ShamanElemental.ShamanElementalSettings>(currentSettingsFile);
                            }
                            currentSetting.ToForm();
                            currentSetting.Save(currentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Shaman Elemental Combat class...");
                            InternalRange = 30.0f;
                            EquipmentAndStats.SetPlayerSpe(WoWSpecialization.ShamanElemental);
                            new ShamanElemental();
                        }
                        break;
                    }
                    if (wowSpecialization == WoWSpecialization.ShamanRestoration)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Shaman_Restoration.xml";
                            var currentSetting = new ShamanRestoration.ShamanRestorationSettings();
                            if (File.Exists(currentSettingsFile) && !resetSettings)
                            {
                                currentSetting = Settings.Load<ShamanRestoration.ShamanRestorationSettings>(currentSettingsFile);
                            }
                            currentSetting.ToForm();
                            currentSetting.Save(currentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Shaman Restoration Combat class...");
                            InternalRange = 30.0f;
                            EquipmentAndStats.SetPlayerSpe(WoWSpecialization.ShamanRestoration);
                            new ShamanRestoration();
                        }
                        break;
                    }
                    break;

                    #endregion

                default:
                    Dispose();
                    break;
            }
        }
        catch
        {
        }
        Logging.WriteFight("Combat system stopped.");
    }

    internal static void DumpCurrentSettings<T>(object mySettings)
    {
        mySettings = mySettings is T ? (T) mySettings : default(T);
        BindingFlags bindingFlags = BindingFlags.Public |
                                    BindingFlags.NonPublic |
                                    BindingFlags.Instance |
                                    BindingFlags.Static;
        for (int i = 0; i < mySettings.GetType().GetFields(bindingFlags).Length - 1; i++)
        {
            FieldInfo field = mySettings.GetType().GetFields(bindingFlags)[i];
            Logging.WriteDebug(field.Name + " = " + field.GetValue(mySettings));
        }

        // Last field is intentionnally ommited because it's a backing field.
    }
}

#region Shaman

public class ShamanEnhancement
{
    private static ShamanEnhancementSettings MySettings = ShamanEnhancementSettings.GetSettings();

    #region General Timers & Variables

    private readonly WoWItem _firstTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET);
    private readonly WoWItem _secondTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET, 2);
    public int LC = 0;

    private Timer _onCd = new Timer(0);

    #endregion

    #region Professions & Racials

    public readonly Spell Alchemy = new Spell("Alchemy");
    public readonly Spell ArcaneTorrent = new Spell("Arcane Torrent");
    public readonly Spell Berserking = new Spell("Berserking");
    public readonly Spell BloodFury = new Spell("Blood Fury");

    public readonly Spell GiftoftheNaaru = new Spell("Gift of the Naaru");

    public readonly Spell Stoneform = new Spell("Stoneform");
    public readonly Spell WarStomp = new Spell("War Stomp");

    #endregion

    #region Shaman Buffs

    public readonly Spell FlametongueWeapon = new Spell("Flametongue Weapon");
    public readonly Spell FrostbrandWeapon = new Spell("Frostbrand Weapon");
    public readonly Spell GhostWolf = new Spell("Ghost Wolf");
    public readonly Spell LightningShield = new Spell("Lightning Shield");
    public readonly Spell MaelstromWeapon = new Spell("Maelstrom Weapon");
    public readonly Spell RockbiterWeapon = new Spell("Rockbiter Weapon");
    public readonly Spell SpiritwalkersGrace = new Spell("Spiritwalker's Grace");
    public readonly Spell WaterShield = new Spell("Water Shield");
    public readonly Spell WaterWalking = new Spell("Water Walking");
    public readonly Spell WindfuryWeapon = new Spell("Windfury Weapon");
    private Timer _waterWalkingTimer = new Timer(0);

    #endregion

    #region Offensive Spell

    public readonly Spell ChainLightning = new Spell("Chain Lightning");
    public readonly Spell EarthShock = new Spell("Earth Shock");
    public readonly Spell FireNova = new Spell("Fire Nova");
    public readonly Spell FlameShock = new Spell("Flame Shock");
    public readonly Spell FrostShock = new Spell("Frost Shock");
    public readonly Spell LavaLash = new Spell("Lava Lash");
    public readonly Spell LightningBolt = new Spell("Lightning Bolt");
    public readonly Spell MagmaTotem = new Spell("Magma Totem");
    public readonly Spell SearingTotem = new Spell("Searing Totem");
    public readonly Timer FireTotemTimer = new Timer(40000);
    public readonly Spell Stormstrike = new Spell("Stormstrike");

    #endregion

    #region Offensive Cooldown

    public readonly Spell AncestralSwiftness = new Spell("Ancestral Swiftness");
    public readonly Spell Ascendance = new Spell("Ascendance");
    public readonly Spell Bloodlust = new Spell("Bloodlust");
    public readonly Spell CalloftheElements = new Spell("Call of the Elements");
    public readonly Spell EarthElementalTotem = new Spell("Earth Elemental Totem");
    public readonly Spell ElementalBlast = new Spell("Elemental Blast");
    public readonly Spell ElementalMastery = new Spell("Elemental Mastery");
    public readonly Spell FeralSpirit = new Spell("Feral Spirit");
    public readonly Spell FireElementalTotem = new Spell("Fire Elemental Totem");
    public readonly Spell Heroism = new Spell("Heroism");
    public readonly Spell StormlashTotem = new Spell("Stormlash Totem");
    public readonly Spell TotemicProjection = new Spell("Totemic Projection");
    public readonly Spell UnleashElements = new Spell("Unleash Elements");
    public readonly Spell UnleashedFury = new Spell("Unleashed Fury");

    #endregion

    #region Defensive Cooldown

    public readonly Spell AstralShift = new Spell("Astral Shift");
    public readonly Spell CapacitorTotem = new Spell("Capacitor Totem");
    public readonly Spell EarthbindTotem = new Spell("Earthbind Totem");
    public readonly Spell GroundingTotem = new Spell("Grounding Totem");
    public readonly Spell ShamanisticRage = new Spell("Shamanistic Rage");
    public readonly Spell StoneBulwarkTotem = new Spell("Stone Bulwark Totem");
    public readonly Spell WindShear = new Spell("Wind Shear");

    #endregion

    #region Healing Spell

    public readonly Spell AncestralGuidance = new Spell("Ancestral Guidance");
    public readonly Spell ChainHeal = new Spell("Chain Heal");
    public readonly Spell HealingRain = new Spell("Healing Rain");
    public readonly Spell HealingStreamTotem = new Spell("Healing Stream Totem");
    public readonly Spell HealingSurge = new Spell("Healing Surge");
    public readonly Spell HealingTideTotem = new Spell("Healing Tide Totem");
    public readonly Spell TotemicRecall = new Spell("Totemic Recall");

    #endregion

    public ShamanEnhancement()
    {
        Main.InternalRange = ObjectManager.Me.GetCombatReach;
        MySettings = ShamanEnhancementSettings.GetSettings();
        Main.DumpCurrentSettings<ShamanEnhancementSettings>(MySettings);
        UInt128 lastTarget = 0;

        while (Main.InternalLoop)
        {
            try
            {
                if (!ObjectManager.Me.IsDeadMe)
                {
                    if (!ObjectManager.Me.IsMounted)
                    {
                        if (Fight.InFight && ObjectManager.Me.Target > 0)
                        {
                            if (ObjectManager.Me.Target != lastTarget
                                && (LavaLash.IsHostileDistanceGood || EarthShock.IsHostileDistanceGood))
                            {
                                Pull();
                                lastTarget = ObjectManager.Me.Target;
                            }

                            if (ObjectManager.Target.Level < 70 && ObjectManager.Me.Level > 84
                                && MySettings.UseLowCombat)
                            {
                                LC = 1;
                                if (ObjectManager.Target.GetDistance <= 40f)
                                    LowCombat();
                            }
                            else
                            {
                                LC = 0;
                                if (ObjectManager.Target.GetDistance <= 40f)
                                    Combat();
                            }
                        }
                        if (!ObjectManager.Me.IsCast)
                            Patrolling();
                    }
                }
                else
                    Thread.Sleep(500);
            }
            catch
            {
            }
            Thread.Sleep(100);
        }
    }

    private void Pull()
    {
        if (TotemicProjection.KnownSpell && TotemicProjection.IsSpellUsable && MySettings.UseTotemicProjection)
            TotemicProjection.Cast();

        if (FlameShock.KnownSpell && FlameShock.IsSpellUsable && FlameShock.IsHostileDistanceGood
            && MySettings.UseFlameShock && LC != 1)
        {
            FlameShock.Cast();
            return;
        }
        if (EarthShock.KnownSpell && EarthShock.IsSpellUsable && EarthShock.IsHostileDistanceGood
            && MySettings.UseEarthShock)
        {
            EarthShock.Cast();
        }
    }

    private void LowCombat()
    {
        Buff();
        if (MySettings.DoAvoidMelee)
            AvoidMelee();
        DefenseCycle();
        Heal();

        if (EarthShock.KnownSpell && EarthShock.IsSpellUsable && EarthShock.IsHostileDistanceGood
            && MySettings.UseEarthShock)
        {
            EarthShock.Cast();
            return;
        }
        if (Stormstrike.KnownSpell && Stormstrike.IsSpellUsable && Stormstrike.IsHostileDistanceGood && MySettings.UseStormstrike)
        {
            Stormstrike.Cast();
            return;
        }
        if (ChainLightning.KnownSpell && ChainLightning.IsSpellUsable && ChainLightning.IsHostileDistanceGood
            && MySettings.UseChainLightning)
        {
            ChainLightning.Cast();
            return;
        }
        if (SearingTotem.KnownSpell && SearingTotem.IsSpellUsable && MySettings.UseSearingTotem
            && FireTotemReady() && !SearingTotem.CreatedBySpellInRange(25) && ObjectManager.Target.GetDistance < 31)
        {
            SearingTotem.Cast();
            return;
        }
        if (MagmaTotem.KnownSpell && MagmaTotem.IsSpellUsable && ObjectManager.Target.GetDistance < 8
            && MySettings.UseMagmaTotem && FireTotemReady())
        {
            MagmaTotem.Cast();
        }
    }

    private void Combat()
    {
        Buff();
        DPSBurst();
        if (MySettings.DoAvoidMelee)
            AvoidMelee();
        DPSCycle();
        Decast();
        if (_onCd.IsReady)
            DefenseCycle();
        Heal();
    }

    private void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (WaterWalking.IsSpellUsable && WaterWalking.KnownSpell &&
            (!WaterWalking.HaveBuff || _waterWalkingTimer.IsReady)
            && !ObjectManager.Me.InCombat && MySettings.UseWaterWalking)
        {
            WaterWalking.CastOnSelf();
            _waterWalkingTimer = new Timer(1000*60*9);
            return;
        }
        if (MySettings.UseWaterShield && !WaterShield.HaveBuff && WaterShield.KnownSpell && WaterShield.IsSpellUsable && (!MySettings.UseLightningShield || ObjectManager.Me.ManaPercentage < 5))
        {
            WaterShield.CastOnSelf();
            return;
        }
        if (MySettings.UseLightningShield && (ObjectManager.Me.ManaPercentage > 10 || !MySettings.UseWaterShield) && LightningShield.KnownSpell && LightningShield.IsSpellUsable && !LightningShield.HaveBuff)
        {
            LightningShield.CastOnSelf();
            return;
        }
        if (ObjectManager.Me.InCombat && SpiritwalkersGrace.IsSpellUsable
            && SpiritwalkersGrace.KnownSpell && MySettings.UseSpiritwalkersGrace && ObjectManager.Me.GetMove)
        {
            SpiritwalkersGrace.Cast();
            return;
        }
        if (WindfuryWeapon.KnownSpell && WindfuryWeapon.IsSpellUsable && !ObjectManager.Me.HaveBuff(33757)
            && MySettings.UseWindfuryWeapon)
        {
            WindfuryWeapon.Cast();
            return;
        }
        if (FrostbrandWeapon.KnownSpell && FrostbrandWeapon.IsSpellUsable && !ObjectManager.Me.HaveBuff(8034)
            && MySettings.UseFrostbrandWeapon && !MySettings.UseWindfuryWeapon)
        {
            FrostbrandWeapon.Cast();
            return;
        }
        if (RockbiterWeapon.KnownSpell && RockbiterWeapon.IsSpellUsable && !ObjectManager.Me.HaveBuff(36494)
            && MySettings.UseRockbiterWeapon && !MySettings.UseWindfuryWeapon
            && !MySettings.UseFrostbrandWeapon)
        {
            RockbiterWeapon.Cast();
            return;
        }
        if (FlametongueWeapon.KnownSpell && FlametongueWeapon.IsSpellUsable && !ObjectManager.Me.HaveBuff(10400)
            && MySettings.UseFlametongueWeapon && (ObjectManager.Me.HaveBuff(33757)
                                                   || ObjectManager.Me.HaveBuff(8034) ||
                                                   ObjectManager.Me.HaveBuff(36494)))
        {
            FlametongueWeapon.Cast();
            return;
        }
        if (MountTask.GetMountCapacity() == MountCapacity.Ground && !ObjectManager.Me.InCombat && GhostWolf.IsSpellUsable && GhostWolf.KnownSpell
            && MySettings.UseGhostWolf && ObjectManager.Me.GetMove && !GhostWolf.HaveBuff
            && ObjectManager.Target.GetDistance > 10)
        {
            GhostWolf.Cast();
            return;
        }
        if (MySettings.UseAlchFlask && !ObjectManager.Me.HaveBuff(79638) && !ObjectManager.Me.HaveBuff(79640) && !ObjectManager.Me.HaveBuff(79639)
            && !ItemsManager.IsItemOnCooldown(75525) && ItemsManager.GetItemCount(75525) > 0)
        {
            ItemsManager.UseItem(75525);
        }
    }

    private void AvoidMelee()
    {
        if (ObjectManager.Target.GetDistance < MySettings.DoAvoidMeleeDistance && ObjectManager.Target.InCombat)
        {
            Logging.WriteFight("Too Close. Moving Back");
            var maxTimeTimer = new Timer(1000*2);
            MovementsAction.MoveBackward(true);
            while (ObjectManager.Target.GetDistance < 2 && ObjectManager.Target.InCombat && !maxTimeTimer.IsReady)
                Others.SafeSleep(300);
            MovementsAction.MoveBackward(false);
            if (maxTimeTimer.IsReady && ObjectManager.Target.GetDistance < 2 && ObjectManager.Target.InCombat)
            {
                MovementsAction.MoveForward(true);
                Others.SafeSleep(1000);
                MovementsAction.MoveForward(false);
                MovementManager.Face(ObjectManager.Target.Position);
            }
        }
    }

    private void DefenseCycle()
    {
        if (MySettings.UseHealingRain && HealingRain.IsSpellUsable)
        {
            SpellManager.CastSpellByIDAndPosition(HealingRain.Id, ObjectManager.Me.Position);
            return;
        }
        if (ObjectManager.Me.HealthPercent < 50 && CapacitorTotem.KnownSpell && CapacitorTotem.IsSpellUsable
            && AirTotemReady() && MySettings.UseCapacitorTotem)
        {
            CapacitorTotem.Cast();
            _onCd = new Timer(1000*5);
            return;
        }
        if (ObjectManager.Me.HealthPercent < 50 && StoneBulwarkTotem.KnownSpell &&
            StoneBulwarkTotem.IsSpellUsable
            && EarthTotemReady() && MySettings.UseStoneBulwarkTotem)
        {
            StoneBulwarkTotem.Cast();
            _onCd = new Timer(1000*10);
            return;
        }
        if (ObjectManager.Me.HealthPercent <= MySettings.UseWarStompAtPercentage && WarStomp.IsSpellUsable &&
            WarStomp.KnownSpell
            && MySettings.UseWarStomp)
        {
            WarStomp.Cast();
            _onCd = new Timer(1000*2);
            return;
        }
        if (ObjectManager.Me.HealthPercent <= MySettings.UseStoneformAtPercentage && Stoneform.IsSpellUsable &&
            Stoneform.KnownSpell
            && MySettings.UseStoneform)
        {
            Stoneform.Cast();
            _onCd = new Timer(1000*8);
            return;
        }
        if (ObjectManager.Me.HealthPercent < 90 && ShamanisticRage.IsSpellUsable
            && ShamanisticRage.KnownSpell && MySettings.UseShamanisticRage)
        {
            ShamanisticRage.Cast();
            _onCd = new Timer(1000*15);
            return;
        }
        if (ObjectManager.Me.HealthPercent < 70 && AstralShift.KnownSpell && AstralShift.IsSpellUsable
            && MySettings.UseAstralShift)
        {
            AstralShift.Cast();
            _onCd = new Timer(1000*6);
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (ObjectManager.Me.ManaPercentage < 50 && TotemicRecall.KnownSpell && TotemicRecall.IsSpellUsable
            && MySettings.UseTotemicRecall && !ObjectManager.Me.InCombat
            && TotemicRecallReady())
        {
            TotemicRecall.Cast();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 95 && HealingSurge.KnownSpell && HealingSurge.IsSpellUsable
            && !ObjectManager.Me.InCombat && MySettings.UseHealingSurge)
        {
            HealingSurge.Cast();
            while (ObjectManager.Me.IsCast)
            {
                Others.SafeSleep(200);
            }
            return;
        }
        if (HealingSurge.KnownSpell && HealingSurge.IsSpellUsable && ObjectManager.Me.HealthPercent < 50
            && MySettings.UseHealingSurge)
        {
            HealingSurge.Cast();
            return;
        }
        if (ObjectManager.Me.HealthPercent <= MySettings.UseGiftoftheNaaruAtPercentage &&
            GiftoftheNaaru.KnownSpell && GiftoftheNaaru.IsSpellUsable
            && MySettings.UseGiftoftheNaaru)
        {
            GiftoftheNaaru.Cast();
            return;
        }
        if (HealingTideTotem.KnownSpell && HealingTideTotem.IsSpellUsable &&
            ObjectManager.Me.HealthPercent < 70
            && WaterTotemReady() && MySettings.UseHealingTideTotem)
        {
            HealingTideTotem.Cast();
            return;
        }
        if (AncestralGuidance.KnownSpell && AncestralGuidance.IsSpellUsable &&
            ObjectManager.Me.HealthPercent < 70
            && MySettings.UseAncestralGuidance)
        {
            AncestralGuidance.Cast();
            return;
        }
        if (ChainHeal.KnownSpell && ChainHeal.IsSpellUsable && ObjectManager.Me.HealthPercent < 80
            && MySettings.UseChainHeal)
        {
            ChainHeal.Cast();
            return;
        }
        if (HealingStreamTotem.KnownSpell && HealingStreamTotem.IsSpellUsable &&
            ObjectManager.Me.HealthPercent < 90
            && WaterTotemReady() && MySettings.UseHealingStreamTotem)
        {
            HealingStreamTotem.Cast();
        }
    }

    private void Decast()
    {
        if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && MySettings.UseWindShear
            && WindShear.KnownSpell && WindShear.IsSpellUsable && WindShear.IsHostileDistanceGood)
        {
            WindShear.Cast();
            return;
        }
        if (ArcaneTorrent.IsSpellUsable && ArcaneTorrent.KnownSpell && ObjectManager.Target.GetDistance < 8
            && ObjectManager.Me.HealthPercent <= MySettings.UseArcaneTorrentForDecastAtPercentage
            && MySettings.UseArcaneTorrentForDecast && ObjectManager.Target.IsCast &&
            ObjectManager.Target.IsTargetingMe)
        {
            ArcaneTorrent.Cast();
            return;
        }
        if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && MySettings.UseGroundingTotem
            && GroundingTotem.KnownSpell && GroundingTotem.IsSpellUsable && AirTotemReady())
        {
            GroundingTotem.Cast();
            return;
        }
        if (ObjectManager.Target.GetMove && !FrostShock.TargetHaveBuff && MySettings.UseFrostShock
            && FrostShock.KnownSpell && FrostShock.IsSpellUsable && FrostShock.IsHostileDistanceGood)
        {
            FrostShock.Cast();
            return;
        }
        if (ObjectManager.Target.GetMove && MySettings.UseEarthbindTotem && EarthTotemReady()
            && EarthbindTotem.KnownSpell && EarthbindTotem.IsSpellUsable && EarthbindTotem.IsHostileDistanceGood)
        {
            EarthbindTotem.Cast();
        }
    }

    private void DPSBurst()
    {
        if (MySettings.UseTrinketOne && !ItemsManager.IsItemOnCooldown(_firstTrinket.Entry) && ItemsManager.IsItemUsable(_firstTrinket.Entry))
        {
            ItemsManager.UseItem(_firstTrinket.Name);
            Logging.WriteFight("Use First Trinket Slot");
        }

        if (MySettings.UseTrinketTwo && !ItemsManager.IsItemOnCooldown(_secondTrinket.Entry) && ItemsManager.IsItemUsable(_secondTrinket.Entry))
        {
            ItemsManager.UseItem(_secondTrinket.Name);
            Logging.WriteFight("Use Second Trinket Slot");
            return;
        }
        if (Berserking.IsSpellUsable && Berserking.KnownSpell && ObjectManager.Target.GetDistance < 30
            && MySettings.UseBerserking)
        {
            Berserking.Cast();
            return;
        }
        if (BloodFury.IsSpellUsable && BloodFury.KnownSpell && ObjectManager.Target.GetDistance < 30
            && MySettings.UseBloodFury)
        {
            BloodFury.Cast();
            return;
        }
        if (UnleashElements.KnownSpell && UnleashElements.IsSpellUsable && UnleashedFury.KnownSpell
            && MySettings.UseUnleashElements && UnleashElements.IsHostileDistanceGood)
        {
            UnleashElements.Cast();
            return;
        }
        if (ElementalBlast.KnownSpell && ElementalBlast.IsSpellUsable
            && MySettings.UseElementalBlast && ElementalBlast.IsHostileDistanceGood)
        {
            ElementalBlast.Cast();
            return;
        }
        if (Ascendance.KnownSpell && Ascendance.IsSpellUsable
            && MySettings.UseAscendance && ObjectManager.Target.GetDistance < 30)
        {
            Ascendance.Cast();
            return;
        }
        if (FireElementalTotem.KnownSpell && FireElementalTotem.IsSpellUsable
            && MySettings.UseFireElementalTotem && ObjectManager.Target.GetDistance < 30)
        {
            FireElementalTotem.Cast();
            return;
        }
        if (StormlashTotem.KnownSpell && AirTotemReady()
            && MySettings.UseStormlashTotem && ObjectManager.Target.GetDistance < 30)
        {
            if (!StormlashTotem.IsSpellUsable && MySettings.UseCalloftheElements
                && CalloftheElements.KnownSpell && CalloftheElements.IsSpellUsable)
            {
                CalloftheElements.Cast();
                Others.SafeSleep(200);
            }

            if (StormlashTotem.IsSpellUsable)
                StormlashTotem.Cast();
            return;
        }
        if (FeralSpirit.KnownSpell && FeralSpirit.IsSpellUsable
            && MySettings.UseFeralSpirit && ObjectManager.Target.GetDistance < 30)
        {
            FeralSpirit.Cast();
            return;
        }
        if (Bloodlust.KnownSpell && Bloodlust.IsSpellUsable && MySettings.UseBloodlustHeroism
            && ObjectManager.Target.GetDistance < 30 && !ObjectManager.Me.HaveBuff(57724))
        {
            Bloodlust.Cast();
            return;
        }
        if (Heroism.KnownSpell && Heroism.IsSpellUsable && MySettings.UseBloodlustHeroism
            && ObjectManager.Target.GetDistance < 30 && !ObjectManager.Me.HaveBuff(57723))
        {
            Heroism.Cast();
            return;
        }
        if (ElementalMastery.KnownSpell && ElementalMastery.IsSpellUsable
            && !ObjectManager.Me.HaveBuff(2825) && MySettings.UseElementalMastery
            && !ObjectManager.Me.HaveBuff(32182))
        {
            ElementalMastery.Cast();
        }
    }

    private void DPSCycle()
    {
        Usefuls.SleepGlobalCooldown();
        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            if (MySettings.UseMagmaTotem && MagmaTotem.IsSpellUsable && ObjectManager.GetUnitInSpellRange(8f) > 1 && FireTotemReady() && !MagmaTotem.CreatedBySpellInRange(8))
            {
                MagmaTotem.Cast();
                FireTotemTimer.Reset();
                return;
            }
            if (MySettings.UseSearingTotem && SearingTotem.IsSpellUsable && ObjectManager.GetUnitInSpellRange(8f) <= 1 && !SearingTotem.CreatedBySpellInRange(25) && FireTotemReady())
            {
                SearingTotem.Cast();
                FireTotemTimer.Reset();
                return;
            }
            if (MySettings.UseChainLightning && ObjectManager.GetUnitInSpellRange(30f) > 1 && ChainLightning.IsSpellUsable && ChainLightning.IsHostileDistanceGood && ObjectManager.Me.BuffStack(53817) == 5)
            {
                ChainLightning.Cast();
                return;
            }
            else if (MySettings.UseLightningBolt && LightningBolt.IsHostileDistanceGood && LightningBolt.IsSpellUsable && ObjectManager.Me.BuffStack(53817) == 5)
            {
                LightningBolt.Cast();
                return;
            }
            if (MySettings.UseStormstrike && Ascendance.HaveBuff && Stormstrike.IsSpellUsable && Stormstrike.IsHostileDistanceGood)
            {
                Stormstrike.Cast();
                return;
            }
            if (MySettings.UseLavaLash && LavaLash.IsSpellUsable && LavaLash.IsHostileDistanceGood)
            {
                LavaLash.Cast();
                return;
            }
            if (MySettings.UseUnleashElements && UnleashElements.IsSpellUsable)
            {
                UnleashElements.Cast();
                return;
            }
            if (MySettings.UseFlameShock && (!FlameShock.HaveBuff || ObjectManager.Me.AuraIsActiveAndExpireInLessThanMs(FlameShock.Id, 9000)) && (ObjectManager.Me.HaveBuff(73683) || !UnleashElements.KnownSpell) &&
                FlameShock.IsSpellUsable && FlameShock.IsHostileDistanceGood)
            {
                FlameShock.Cast();
                return;
            }
            if (MySettings.UseFrostShock && FrostShock.IsSpellUsable && FrostShock.IsHostileDistanceGood)
            {
                FrostShock.Cast();
                return;
            }
            if (MySettings.UseChainLightning && ObjectManager.GetUnitInSpellRange(30f) > 1 && ChainLightning.IsSpellUsable && ChainLightning.IsHostileDistanceGood && ObjectManager.Me.BuffStack(53817) >= 1)
            {
                ChainLightning.Cast();
                return;
            }
            else if (MySettings.UseLightningBolt && LightningBolt.IsHostileDistanceGood && LightningBolt.IsSpellUsable && ObjectManager.Me.BuffStack(53817) >= 1)
            {
                LightningBolt.Cast();
                return;
            }
            if (MySettings.UseMagmaTotem && MagmaTotem.IsSpellUsable && (FireTotemTimer.IsReady || !MagmaTotem.CreatedBySpellInRange(8)) && ObjectManager.GetUnitInSpellRange(8f) > 1 && FireTotemReady())
            {
                MagmaTotem.Cast();
                FireTotemTimer.Reset();
                return;
            }
            if (MySettings.UseSearingTotem && SearingTotem.IsSpellUsable && ObjectManager.GetUnitInSpellRange(8f) <= 1 && (FireTotemTimer.IsReady || !SearingTotem.CreatedBySpellInRange(25)) && FireTotemReady())
            {
                SearingTotem.Cast();
                FireTotemTimer.Reset();
                return;
            }
            if (MySettings.UseLightningBolt && !MaelstromWeapon.KnownSpell && !FrostShock.IsSpellUsable && LightningBolt.IsHostileDistanceGood && LightningBolt.IsSpellUsable)
            {
                LightningBolt.Cast();
                return;
            }
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    private bool FireTotemReady()
    {
        if (FireElementalTotem.CreatedBySpell || MagmaTotem.CreatedBySpell)
            return false;
        return true;
    }

    private bool EarthTotemReady()
    {
        if (EarthbindTotem.CreatedBySpell || EarthElementalTotem.CreatedBySpell
            || StoneBulwarkTotem.CreatedBySpell)
            return false;
        return true;
    }

    private bool WaterTotemReady()
    {
        if (HealingStreamTotem.CreatedBySpell || HealingTideTotem.CreatedBySpell)
            return false;
        return true;
    }

    private bool AirTotemReady()
    {
        if (CapacitorTotem.CreatedBySpell || GroundingTotem.CreatedBySpell
            || StormlashTotem.CreatedBySpell)
            return false;
        return true;
    }

    private bool TotemicRecallReady()
    {
        if (FireElementalTotem.CreatedBySpell)
            return false;
        if (EarthElementalTotem.CreatedBySpell)
            return false;
        if (SearingTotem.CreatedBySpell)
            return true;
        if (FireTotemReady() && EarthTotemReady() && WaterTotemReady() && AirTotemReady())
            return false;
        return true;
    }

    private void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            Buff();
            Heal();
        }
    }

    #region Nested type: ShamanEnhancementSettings

    [Serializable]
    public class ShamanEnhancementSettings : Settings
    {
        public bool DoAvoidMelee = false;
        public int DoAvoidMeleeDistance = 0;
        public bool UseAlchFlask = true;
        public bool UseAncestralGuidance = true;
        public bool UseAncestralSwiftness = true;
        public bool UseArcaneTorrentForDecast = true;
        public int UseArcaneTorrentForDecastAtPercentage = 100;
        public bool UseArcaneTorrentForResource = true;
        public int UseArcaneTorrentForResourceAtPercentage = 80;
        public bool UseAscendance = true;
        public bool UseAstralShift = true;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseBloodlustHeroism = true;
        public bool UseCalloftheElements = true;
        public bool UseCapacitorTotem = true;
        public bool UseChainHeal = false;
        public bool UseChainLightning = true;
        public bool UseEarthElementalTotem = true;
        public bool UseEarthShock = true;
        public bool UseEarthbindTotem = false;
        public bool UseElementalBlast = true;
        public bool UseElementalMastery = true;

        public bool UseFeralSpirit = true;
        public bool UseFireElementalTotem = true;
        public bool UseFireNova = true;
        public bool UseFlameShock = true;
        public bool UseFlametongueWeapon = true;
        public bool UseFrostShock = false;
        public bool UseFrostbrandWeapon = false;
        public bool UseGhostWolf = true;
        public bool UseGiftoftheNaaru = true;
        public int UseGiftoftheNaaruAtPercentage = 80;
        public bool UseGroundingTotem = true;
        public bool UseHealingRain = true;
        public bool UseHealingStreamTotem = true;
        public bool UseHealingSurge = true;
        public bool UseHealingTideTotem = true;
        public bool UseLavaLash = true;

        public bool UseLightningBolt = true;
        public bool UseLightningShield = true;
        public bool UseLowCombat = true;
        public bool UseMagmaTotem = true;
        public bool UseRockbiterWeapon = false;
        public bool UseSearingTotem = true;
        public bool UseShamanisticRage = true;
        public bool UseSpiritwalkersGrace = true;
        public bool UseStoneBulwarkTotem = true;
        public bool UseStoneform = true;
        public int UseStoneformAtPercentage = 80;
        public bool UseStormlashTotem = true;
        public bool UseStormstrike = true;
        public bool UseTotemicProjection = true;
        public bool UseTotemicRecall = true;
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;
        public bool UseUnleashElements = true;
        public bool UseWarStomp = true;
        public int UseWarStompAtPercentage = 80;
        public bool UseWaterShield = true;
        public bool UseWaterWalking = true;
        public bool UseWindShear = true;
        public bool UseWindfuryWeapon = true;

        public ShamanEnhancementSettings()
        {
            ConfigWinForm("Shaman Enhancement Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent for Interrupt", "UseArcaneTorrentForDecast", "Professions & Racials", "AtPercentage");
            AddControlInWinForm("Use Arcane Torrent for Resource", "UseArcaneTorrentForResource", "Professions & Racials", "AtPercentage");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");

            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            /* Shaman Buffs */
            AddControlInWinForm("Use Flametongue Weapon", "UseFlametongueWeapon", "Shaman Buffs");
            AddControlInWinForm("Use Frostbrand Weapon", "UseFrostbrandWeapon", "Shaman Buffs");
            AddControlInWinForm("Use Ghost Wolf", "UseGhostWolf", "Shaman Buffs");
            AddControlInWinForm("Use Lightning Shield", "UseLightningShield", "Shaman Buffs");
            AddControlInWinForm("Use Rockbiter Weapon", "UseRockbiterWeapon", "Shaman Buffs");
            AddControlInWinForm("Use Spiritwalker's Grace", "UseSpiritwalkersGrace", "Shaman Buffs");
            AddControlInWinForm("Use Water Shield", "UseWaterShield", "Shaman Buffs");
            AddControlInWinForm("Use Water Walking", "UseWaterWalking", "Shaman Buffs");
            AddControlInWinForm("Use Windfury Weapon", "UseWindfuryWeapon", "Shaman Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Chain Lightning", "UseChainLightning", "Offensive Spell");
            AddControlInWinForm("Use Earth Shock", "UseEarthShock", "Offensive Spell");
            AddControlInWinForm("Use Fire Nova", "UseFireNova", "Offensive Spell");
            AddControlInWinForm("Use Flame Shock", "UseFlameShock", "Offensive Spell");
            AddControlInWinForm("Use Frost Shock", "UseFrostShock", "Offensive Spell");
            AddControlInWinForm("Use Lava Lash", "UseLavaLash", "Offensive Spell");
            AddControlInWinForm("Use Lightning Bolt", "UseLightningBolt", "Offensive Spell");
            AddControlInWinForm("Use Magma Totem", "UseMagmaTotem", "Offensive Spell");
            AddControlInWinForm("Use Searing Totem", "UseSearingTotem", "Offensive Spell");
            AddControlInWinForm("Use Stormstrike", "UseStormstrike", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use Ancestral Swiftness", "UseAncestralSwiftness", "Offensive Cooldown");
            AddControlInWinForm("Use Ascendance", "UseAscendance", "Offensive Cooldown");
            AddControlInWinForm("Use Bloodlust / Heroism", "UseBloodlustHeroism", "Offensive Cooldown");
            AddControlInWinForm("Use Call of the Elements", "UseCalloftheElements", "Offensive Cooldown");
            AddControlInWinForm("Use Earth Elemental Totem", "UseEarthElementalTotem", "Offensive Cooldown");
            AddControlInWinForm("Use Elemental Blast", "UseElementalBlast", "Offensive Cooldown");
            AddControlInWinForm("Use Elemental Mastery", "UseElementalMastery", "Offensive Cooldown");
            AddControlInWinForm("Use Feral Spirit", "UseFeralSpirit", "Offensive Cooldown");
            AddControlInWinForm("Use Fire Elemental Totem", "UseFireElementalTotem", "Offensive Cooldown");
            AddControlInWinForm("Use Stormlash Totem", "UseStormlashTotem", "Offensive Cooldown");
            AddControlInWinForm("Use Totemic Projection", "UseTotemicProjection", "Offensive Cooldown");
            AddControlInWinForm("Use Unleash Elements", "UseUnleashElements", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Astral Shift", "UseAstralShift", "Defensive Cooldown");
            AddControlInWinForm("Use Capacitor Totem", "UseCapacitorTotem", "Defensive Cooldown");
            AddControlInWinForm("Use Earthbind Totem", "UseEarthbindTotem", "Defensive Cooldown");
            AddControlInWinForm("Use Grounding Totem", "UseGroundingTotem", "Defensive Cooldown");
            AddControlInWinForm("Use Shamanistic Rage", "UseShamanisticRage", "Defensive Cooldown");
            AddControlInWinForm("Use StoneBulwark Totem", "UseStoneBulwarkTotem", "Defensive Cooldown");
            AddControlInWinForm("Use Wind Shear", "UseWindShear", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Ancestral Guidance", "UseAncestralGuidance", "Healing Spell");
            AddControlInWinForm("Use Chain Heal", "UseChainHeal", "Healing Spell");
            AddControlInWinForm("Use Healing Rain", "UseHealingRain", "Healing Spell");
            AddControlInWinForm("Use Healing Surge", "UseHealingSurge", "Healing Spell");
            AddControlInWinForm("Use Healing Stream Totem", "UseHealingStreamTotem", "Healing Spell");
            AddControlInWinForm("Use Healing Tide Totem", "UsHealingTideTotem", "Healing Spell");
            AddControlInWinForm("Use Totemic Recall", "UseTotemicRecall", "Healing Spell");
            /* Game Settings */
            AddControlInWinForm("Use Low Combat Settings", "UseLowCombat", "Game Settings");
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");

            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
            AddControlInWinForm("Do avoid melee (Off Advised!!)", "DoAvoidMelee", "Game Settings");
            AddControlInWinForm("Avoid melee distance (1 to 4)", "DoAvoidMeleeDistance", "Game Settings");
        }

        public static ShamanEnhancementSettings CurrentSetting { get; set; }

        public static ShamanEnhancementSettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Shaman_Enhancement.xml";
            if (File.Exists(currentSettingsFile))
            {
                return
                    CurrentSetting = Load<ShamanEnhancementSettings>(currentSettingsFile);
            }
            return new ShamanEnhancementSettings();
        }
    }

    #endregion
}

public class ShamanRestoration
{
    private static ShamanRestorationSettings MySettings = ShamanRestorationSettings.GetSettings();

    #region General Timers & Variables

    private readonly WoWItem _firstTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET);
    private readonly WoWItem _secondTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET, 2);
    public int LC = 0;

    private Timer _onCd = new Timer(0);

    #endregion

    #region Professions & Racials

    public readonly Spell Alchemy = new Spell("Alchemy");
    public readonly Spell ArcaneTorrent = new Spell("Arcane Torrent");
    public readonly Spell Berserking = new Spell("Berserking");
    public readonly Spell BloodFury = new Spell("Blood Fury");
    public readonly Spell GiftoftheNaaru = new Spell("Gift of the Naaru");
    public readonly Spell Stoneform = new Spell("Stoneform");
    public readonly Spell WarStomp = new Spell("War Stomp");

    #endregion

    #region Shaman Buffs

    public readonly Spell EarthShield = new Spell("Earth Shield");
    public readonly Spell EarthlivingWeapon = new Spell("Earthliving Weapon");
    public readonly Spell FlametongueWeapon = new Spell("Flametongue Weapon");
    public readonly Spell FrostbrandWeapon = new Spell("Frostbrand Weapon");
    public readonly Spell GhostWolf = new Spell("Ghost Wolf");
    public readonly Spell LightningShield = new Spell("Lightning Shield");
    public readonly Spell RockbiterWeapon = new Spell("Rockbiter Weapon");
    public readonly Spell SpiritwalkersGrace = new Spell("Spiritwalker's Grace");
    public readonly Spell WaterShield = new Spell("Water Shield");
    public readonly Spell WaterWalking = new Spell("Water Walking");
    private Timer _waterWalkingTimer = new Timer(0);

    #endregion

    #region Offensive Spell

    public readonly Spell ChainLightning = new Spell("Chain Lightning");
    public readonly Spell EarthShock = new Spell("Earth Shock");
    public readonly Spell FlameShock = new Spell("Flame Shock");
    public readonly Spell FrostShock = new Spell("Frost Shock");
    public readonly Spell LavaBurst = new Spell("Lava Burst");
    public readonly Spell LightningBolt = new Spell("Lightning Bolt");
    public readonly Spell MagmaTotem = new Spell("Magma Totem");
    public readonly Spell PrimalStrike = new Spell("Primal Strike");
    public readonly Spell SearingTotem = new Spell("Searing Totem");
    private Timer _flameShockTimer = new Timer(0);

    #endregion

    #region Offensive Cooldown

    public readonly Spell AncestralSwiftness = new Spell("Ancestral Swiftness");
    public readonly Spell Ascendance = new Spell("Ascendance");
    public readonly Spell Bloodlust = new Spell("Bloodlust");
    public readonly Spell CalloftheElements = new Spell("Call of the Elements");
    public readonly Spell EarthElementalTotem = new Spell("Earth Elemental Totem");
    public readonly Spell ElementalBlast = new Spell("Elemental Blast");
    public readonly Spell ElementalMastery = new Spell("Elemental Mastery");
    public readonly Spell FireElementalTotem = new Spell("Fire Elemental Totem");
    public readonly Spell Heroism = new Spell("Heroism");
    public readonly Spell StormlashTotem = new Spell("Stormlash Totem");
    public readonly Spell TotemicProjection = new Spell("Totemic Projection");
    public readonly Spell UnleashElements = new Spell("Unleash Elements");
    public readonly Spell UnleashedFury = new Spell("Unleashed Fury");

    #endregion

    #region Defensive Cooldown

    public readonly Spell AstralShift = new Spell("Astral Shift");
    public readonly Spell CapacitorTotem = new Spell("Capacitor Totem");
    public readonly Spell EarthbindTotem = new Spell("Earthbind Totem");
    public readonly Spell GroundingTotem = new Spell("Grounding Totem");
    public readonly Spell StoneBulwarkTotem = new Spell("Stone Bulwark Totem");
    public readonly Spell WindShear = new Spell("Wind Shear");

    #endregion

    #region Healing Spell

    public readonly Spell AncestralGuidance = new Spell("Ancestral Guidance");
    public readonly Spell ChainHeal = new Spell("Chain Heal");
    public readonly Spell GreaterHealingWave = new Spell("Greater Healing Wave");
    public readonly Spell HealingRain = new Spell("Healing Rain");
    public readonly Spell HealingStreamTotem = new Spell("Healing Stream Totem");
    public readonly Spell HealingSurge = new Spell("Healing Surge");
    public readonly Spell HealingTideTotem = new Spell("Healing Tide Totem");
    public readonly Spell HealingWave = new Spell("HealingWave");
    public readonly Spell ManaTideTotem = new Spell("Mana Tide Totem");
    public readonly Spell Riptide = new Spell("Riptide");
    public readonly Spell SpiritLinkTotem = new Spell("Spirit Link Totem");
    public readonly Spell TotemicRecall = new Spell("Totemic Recall");

    #endregion

    public ShamanRestoration()
    {
        Main.InternalRange = 30.0f;
        Main.InternalAggroRange = 30f;
        MySettings = ShamanRestorationSettings.GetSettings();
        Main.DumpCurrentSettings<ShamanRestorationSettings>(MySettings);
        UInt128 lastTarget = 0;

        while (Main.InternalLoop)
        {
            try
            {
                if (!ObjectManager.Me.IsDeadMe)
                {
                    if (!ObjectManager.Me.IsMounted)
                    {
                        if (Fight.InFight && ObjectManager.Me.Target > 0)
                        {
                            if (ObjectManager.Me.Target != lastTarget
                                && (FlameShock.IsHostileDistanceGood || LightningBolt.IsHostileDistanceGood))
                            {
                                Pull();
                                lastTarget = ObjectManager.Me.Target;
                            }

                            if (ObjectManager.Target.Level < 70 && ObjectManager.Me.Level > 84
                                && MySettings.UseLowCombat)
                            {
                                LC = 1;
                                if (ObjectManager.Target.GetDistance <= 40f)
                                    LowCombat();
                            }
                            else
                            {
                                LC = 0;
                                if (ObjectManager.Target.GetDistance <= 40f)
                                    Combat();
                            }
                        }
                        if (!ObjectManager.Me.IsCast)
                            Patrolling();
                    }
                }
                else
                    Thread.Sleep(500);
            }
            catch
            {
            }
            Thread.Sleep(100);
        }
    }

    private void Pull()
    {
        if (TotemicProjection.KnownSpell && TotemicProjection.IsSpellUsable && MySettings.UseTotemicProjection)
            TotemicProjection.Cast();

        if (FlameShock.KnownSpell && FlameShock.IsSpellUsable && FlameShock.IsHostileDistanceGood
            && MySettings.UseFlameShock && LC != 1)
        {
            FlameShock.Cast();
            return;
        }
        if (EarthShock.KnownSpell && EarthShock.IsSpellUsable && EarthShock.IsHostileDistanceGood
            && MySettings.UseEarthShock)
        {
            EarthShock.Cast();
        }
    }

    private void LowCombat()
    {
        Buff();
        if (MySettings.DoAvoidMelee)
            AvoidMelee();
        DefenseCycle();
        Heal();

        if (EarthShock.KnownSpell && EarthShock.IsSpellUsable && EarthShock.IsHostileDistanceGood
            && MySettings.UseEarthShock)
        {
            EarthShock.Cast();
            return;
        }
        if (LavaBurst.KnownSpell && LavaBurst.IsSpellUsable && LavaBurst.IsHostileDistanceGood
            && MySettings.UseLavaBurst)
        {
            LavaBurst.Cast();
            return;
        }
        if (ChainLightning.KnownSpell && ChainLightning.IsSpellUsable && ChainLightning.IsHostileDistanceGood
            && MySettings.UseChainLightning)
        {
            ChainLightning.Cast();
            return;
        }
        if (SearingTotem.KnownSpell && SearingTotem.IsSpellUsable && MySettings.UseSearingTotem
            && FireTotemReady() && !SearingTotem.CreatedBySpellInRange(25) && ObjectManager.Target.GetDistance < 31)
        {
            SearingTotem.Cast();
            return;
        }
        if (MagmaTotem.KnownSpell && MagmaTotem.IsSpellUsable && ObjectManager.Target.GetDistance < 8
            && MySettings.UseMagmaTotem && FireTotemReady())
        {
            MagmaTotem.Cast();
        }
    }

    private void Combat()
    {
        Buff();
        DPSBurst();
        if (MySettings.DoAvoidMelee)
            AvoidMelee();
        DPSCycle();
        Decast();
        if (_onCd.IsReady)
            DefenseCycle();
        Heal();
    }

    private void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (WaterWalking.IsSpellUsable && WaterWalking.KnownSpell &&
            (!WaterWalking.HaveBuff || _waterWalkingTimer.IsReady)
            && !ObjectManager.Me.InCombat && MySettings.UseWaterWalking)
        {
            WaterWalking.CastOnSelf();
            _waterWalkingTimer = new Timer(1000*60*9);
            return;
        }
        if (MySettings.UseWaterShield && !WaterShield.HaveBuff && WaterShield.KnownSpell && WaterShield.IsSpellUsable &&
            (!MySettings.UseLightningShield && !MySettings.UseEarthShield || ObjectManager.Me.ManaPercentage < 5))
        {
            WaterShield.CastOnSelf();
            return;
        }
        if (MySettings.UseEarthShield && !MySettings.UseLightningShield && !EarthShield.HaveBuff && EarthShield.KnownSpell && ObjectManager.Me.HealthPercent < 50 && EarthShield.IsSpellUsable)
        {
            EarthShield.Cast();
            return;
        }
        if (MySettings.UseLightningShield && !MySettings.UseEarthShield && (ObjectManager.Me.ManaPercentage > 10 || !MySettings.UseWaterShield) && LightningShield.KnownSpell && LightningShield.IsSpellUsable &&
            !LightningShield.HaveBuff)
        {
            LightningShield.CastOnSelf();
            return;
        }
        if (ObjectManager.Me.InCombat && SpiritwalkersGrace.IsSpellUsable
            && SpiritwalkersGrace.KnownSpell && MySettings.UseSpiritwalkersGrace && ObjectManager.Me.GetMove)
        {
            SpiritwalkersGrace.Cast();
            return;
        }
        if (FlametongueWeapon.KnownSpell && FlametongueWeapon.IsSpellUsable && !ObjectManager.Me.HaveBuff(10400)
            && MySettings.UseFlametongueWeapon)
        {
            FlametongueWeapon.Cast();
            return;
        }
        if (EarthlivingWeapon.KnownSpell && EarthlivingWeapon.IsSpellUsable &&
            !ObjectManager.Me.HaveBuff(52007)
            && MySettings.UseEarthlivingWeapon && !MySettings.UseFlametongueWeapon)
        {
            EarthlivingWeapon.Cast();
            return;
        }
        if (FrostbrandWeapon.KnownSpell && FrostbrandWeapon.IsSpellUsable &&
            !ObjectManager.Me.HaveBuff(8034)
            && MySettings.UseFrostbrandWeapon && !MySettings.UseFlametongueWeapon &&
            !MySettings.UseEarthlivingWeapon)
        {
            FrostbrandWeapon.Cast();
            return;
        }
        if (RockbiterWeapon.KnownSpell && RockbiterWeapon.IsSpellUsable &&
            !ObjectManager.Me.HaveBuff(36494)
            && MySettings.UseRockbiterWeapon && !MySettings.UseFlametongueWeapon
            && !MySettings.UseFrostbrandWeapon && !MySettings.UseEarthlivingWeapon)
        {
            RockbiterWeapon.Cast();
            return;
        }

        if (MountTask.GetMountCapacity() == MountCapacity.Ground && !ObjectManager.Me.InCombat && GhostWolf.IsSpellUsable && GhostWolf.KnownSpell
            && MySettings.UseGhostWolf && ObjectManager.Me.GetMove && !GhostWolf.HaveBuff
            && ObjectManager.Target.GetDistance > 50)
        {
            GhostWolf.Cast();
            return;
        }

        if (MySettings.UseAlchFlask && !ObjectManager.Me.HaveBuff(79638) && !ObjectManager.Me.HaveBuff(79640) && !ObjectManager.Me.HaveBuff(79639)
            && !ItemsManager.IsItemOnCooldown(75525) && ItemsManager.GetItemCount(75525) > 0)
        {
            ItemsManager.UseItem(75525);
        }
    }

    private void AvoidMelee()
    {
        if (ObjectManager.Target.GetDistance < MySettings.DoAvoidMeleeDistance && ObjectManager.Target.InCombat)
        {
            Logging.WriteFight("Too Close. Moving Back");
            var maxTimeTimer = new Timer(1000*2);
            MovementsAction.MoveBackward(true);
            while (ObjectManager.Target.GetDistance < 2 && ObjectManager.Target.InCombat && !maxTimeTimer.IsReady)
                Others.SafeSleep(300);
            MovementsAction.MoveBackward(false);
            if (maxTimeTimer.IsReady && ObjectManager.Target.GetDistance < 2 && ObjectManager.Target.InCombat)
            {
                MovementsAction.MoveForward(true);
                Others.SafeSleep(1000);
                MovementsAction.MoveForward(false);
                MovementManager.Face(ObjectManager.Target.Position);
            }
        }
    }

    private void DefenseCycle()
    {
        if (ObjectManager.Me.HealthPercent < 50 && CapacitorTotem.KnownSpell && CapacitorTotem.IsSpellUsable
            && AirTotemReady() && MySettings.UseCapacitorTotem)
        {
            CapacitorTotem.Cast();
            _onCd = new Timer(1000*5);
            return;
        }
        if (ObjectManager.Me.HealthPercent < 50 && StoneBulwarkTotem.KnownSpell &&
            StoneBulwarkTotem.IsSpellUsable
            && EarthTotemReady() && MySettings.UseStoneBulwarkTotem)
        {
            StoneBulwarkTotem.Cast();
            _onCd = new Timer(1000*10);
            return;
        }
        if (ObjectManager.Me.HealthPercent < 70 && SpiritLinkTotem.KnownSpell &&
            SpiritLinkTotem.IsSpellUsable
            && AirTotemReady() && MySettings.UseSpiritLinkTotem)
        {
            SpiritLinkTotem.Cast();
            _onCd = new Timer(1000*6);
            return;
        }
        if (ObjectManager.Me.HealthPercent <= MySettings.UseWarStompAtPercentage && WarStomp.IsSpellUsable &&
            WarStomp.KnownSpell
            && MySettings.UseWarStomp)
        {
            WarStomp.Cast();
            _onCd = new Timer(1000*2);
            return;
        }
        if (ObjectManager.Me.HealthPercent <= MySettings.UseStoneformAtPercentage && Stoneform.IsSpellUsable &&
            Stoneform.KnownSpell
            && MySettings.UseStoneform)
        {
            Stoneform.Cast();
            _onCd = new Timer(1000*8);
            return;
        }
        if (ObjectManager.Me.HealthPercent < 70 && AstralShift.KnownSpell && AstralShift.IsSpellUsable
            && MySettings.UseAstralShift)
        {
            AstralShift.Cast();
            _onCd = new Timer(1000*6);
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (ArcaneTorrent.IsSpellUsable && ArcaneTorrent.KnownSpell &&
            ObjectManager.Me.ManaPercentage <= MySettings.UseArcaneTorrentForResourceAtPercentage
            && MySettings.UseArcaneTorrentForResource)
        {
            ArcaneTorrent.Cast();
            return;
        }
        if (ObjectManager.Me.ManaPercentage < 50 && TotemicRecall.KnownSpell && TotemicRecall.IsSpellUsable
            && MySettings.UseTotemicRecall && !ObjectManager.Me.InCombat
            && TotemicRecallReady())
        {
            TotemicRecall.Cast();
            return;
        }
        if (ObjectManager.Me.ManaPercentage < 80 && ManaTideTotem.KnownSpell && ManaTideTotem.IsSpellUsable
            && MySettings.UseManaTideTotem && WaterTotemReady())
        {
            ManaTideTotem.Cast();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 95 && HealingSurge.KnownSpell && HealingSurge.IsSpellUsable
            && !ObjectManager.Me.InCombat && MySettings.UseHealingSurge)
        {
            HealingSurge.Cast();
            while (ObjectManager.Me.IsCast)
            {
                Others.SafeSleep(200);
            }
            return;
        }
        if (HealingSurge.KnownSpell && HealingSurge.IsSpellUsable && ObjectManager.Me.HealthPercent < 50
            && MySettings.UseHealingSurge)
        {
            HealingSurge.Cast();
            return;
        }
        if (GreaterHealingWave.KnownSpell && GreaterHealingWave.IsSpellUsable
            && ObjectManager.Me.HealthPercent < 60 && MySettings.UseGreaterHealingWave)
        {
            GreaterHealingWave.Cast();
            return;
        }
        if (ObjectManager.Me.HealthPercent <= MySettings.UseGiftoftheNaaruAtPercentage &&
            GiftoftheNaaru.KnownSpell && GiftoftheNaaru.IsSpellUsable
            && MySettings.UseGiftoftheNaaru)
        {
            GiftoftheNaaru.Cast();
            return;
        }
        if (HealingTideTotem.KnownSpell && HealingTideTotem.IsSpellUsable &&
            ObjectManager.Me.HealthPercent < 70
            && WaterTotemReady() && MySettings.UseHealingTideTotem)
        {
            HealingTideTotem.Cast();
            return;
        }
        if (AncestralGuidance.KnownSpell && AncestralGuidance.IsSpellUsable &&
            ObjectManager.Me.HealthPercent < 70
            && MySettings.UseAncestralGuidance)
        {
            AncestralGuidance.Cast();
            return;
        }
        if (ChainHeal.KnownSpell && ChainHeal.IsSpellUsable && ObjectManager.Me.HealthPercent < 80
            && MySettings.UseChainHeal)
        {
            ChainHeal.Cast();
            return;
        }
        if (HealingStreamTotem.KnownSpell && HealingStreamTotem.IsSpellUsable &&
            ObjectManager.Me.HealthPercent < 90
            && WaterTotemReady() && MySettings.UseHealingStreamTotem)
        {
            HealingStreamTotem.Cast();
            return;
        }
        if (Riptide.KnownSpell && Riptide.IsSpellUsable && ObjectManager.Me.HealthPercent < 90
            && MySettings.UseRiptide && !Riptide.HaveBuff)
        {
            Riptide.Cast();
            return;
        }
        if (HealingWave.KnownSpell && HealingWave.IsSpellUsable && ObjectManager.Me.HealthPercent < 80
            && MySettings.UseHealingWave)
        {
            HealingWave.Cast();
        }
    }

    private void Decast()
    {
        if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && MySettings.UseWindShear
            && WindShear.KnownSpell && WindShear.IsSpellUsable && WindShear.IsHostileDistanceGood)
        {
            WindShear.Cast();
            return;
        }
        if (ArcaneTorrent.IsSpellUsable && ArcaneTorrent.KnownSpell && ObjectManager.Target.GetDistance < 8
            && ObjectManager.Me.HealthPercent <= MySettings.UseArcaneTorrentForDecastAtPercentage
            && MySettings.UseArcaneTorrentForDecast && ObjectManager.Target.IsCast &&
            ObjectManager.Target.IsTargetingMe)
        {
            ArcaneTorrent.Cast();
            return;
        }
        if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && MySettings.UseGroundingTotem
            && GroundingTotem.KnownSpell && GroundingTotem.IsSpellUsable && AirTotemReady())
        {
            GroundingTotem.Cast();
            return;
        }

        if (ObjectManager.Target.GetMove && !FrostShock.TargetHaveBuff && MySettings.UseFrostShock
            && FrostShock.KnownSpell && FrostShock.IsSpellUsable && FrostShock.IsHostileDistanceGood)
        {
            FrostShock.Cast();
            return;
        }
        if (ObjectManager.Target.GetMove && MySettings.UseEarthbindTotem && EarthTotemReady()
            && EarthbindTotem.KnownSpell && EarthbindTotem.IsSpellUsable && EarthbindTotem.IsHostileDistanceGood)
        {
            EarthbindTotem.Cast();
        }
    }

    private void DPSBurst()
    {
        if (MySettings.UseTrinketOne && !ItemsManager.IsItemOnCooldown(_firstTrinket.Entry) && ItemsManager.IsItemUsable(_firstTrinket.Entry))
        {
            ItemsManager.UseItem(_firstTrinket.Name);
            Logging.WriteFight("Use First Trinket Slot");
        }

        if (MySettings.UseTrinketTwo && !ItemsManager.IsItemOnCooldown(_secondTrinket.Entry) && ItemsManager.IsItemUsable(_secondTrinket.Entry))
        {
            ItemsManager.UseItem(_secondTrinket.Name);
            Logging.WriteFight("Use Second Trinket Slot");
            return;
        }
        if (Berserking.IsSpellUsable && Berserking.KnownSpell && ObjectManager.Target.GetDistance <= 40f
            && MySettings.UseBerserking)
        {
            Berserking.Cast();
            return;
        }
        if (BloodFury.IsSpellUsable && BloodFury.KnownSpell && ObjectManager.Target.GetDistance <= 40f
            && MySettings.UseBloodFury)
        {
            BloodFury.Cast();
            return;
        }
        if (UnleashElements.KnownSpell && UnleashElements.IsSpellUsable && UnleashedFury.KnownSpell
            && MySettings.UseUnleashElements && UnleashElements.IsHostileDistanceGood)
        {
            UnleashElements.Cast();
            return;
        }
        if (ElementalBlast.KnownSpell && ElementalBlast.IsSpellUsable
            && MySettings.UseElementalBlast && ElementalBlast.IsHostileDistanceGood)
        {
            ElementalBlast.Cast();
            return;
        }
        if (Ascendance.KnownSpell && Ascendance.IsSpellUsable && ObjectManager.Me.HealthPercent < 80
            && MySettings.UseAscendance && ObjectManager.Target.GetDistance <= 40f)
        {
            Ascendance.Cast();
            return;
        }
        if (FireElementalTotem.KnownSpell && FireElementalTotem.IsSpellUsable
            && MySettings.UseFireElementalTotem && ObjectManager.Target.GetDistance <= 40f)
        {
            FireElementalTotem.Cast();
            return;
        }
        if (StormlashTotem.KnownSpell && AirTotemReady()
            && MySettings.UseStormlashTotem && ObjectManager.Target.GetDistance <= 40f)
        {
            if (!StormlashTotem.IsSpellUsable && MySettings.UseCalloftheElements
                && CalloftheElements.KnownSpell && CalloftheElements.IsSpellUsable)
            {
                CalloftheElements.Cast();
                Others.SafeSleep(200);
            }

            if (StormlashTotem.IsSpellUsable)
                StormlashTotem.Cast();
            return;
        }
        if (Bloodlust.KnownSpell && Bloodlust.IsSpellUsable && MySettings.UseBloodlustHeroism
            && ObjectManager.Target.GetDistance <= 40f && !ObjectManager.Me.HaveBuff(57724))
        {
            Bloodlust.Cast();
            return;
        }
        if (Heroism.KnownSpell && Heroism.IsSpellUsable && MySettings.UseBloodlustHeroism
            && ObjectManager.Target.GetDistance <= 40f && !ObjectManager.Me.HaveBuff(57723))
        {
            Heroism.Cast();
            return;
        }
        if (ElementalMastery.KnownSpell && ElementalMastery.IsSpellUsable
            && !ObjectManager.Me.HaveBuff(2825) && MySettings.UseElementalMastery
            && !ObjectManager.Me.HaveBuff(32182))
        {
            ElementalMastery.Cast();
        }
    }

    private void DPSCycle()
    {
        Usefuls.SleepGlobalCooldown();
        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            if (PrimalStrike.KnownSpell && PrimalStrike.IsSpellUsable && PrimalStrike.IsHostileDistanceGood
                && MySettings.UsePrimalStrike && ObjectManager.Me.Level < 11)
            {
                PrimalStrike.Cast();
                return;
            }

            if (EarthElementalTotem.KnownSpell && EarthElementalTotem.IsSpellUsable
                && ObjectManager.GetNumberAttackPlayer() > 3 && MySettings.UseEarthElementalTotem)
            {
                EarthElementalTotem.Cast();
                return;
            }
            if (FlameShock.IsSpellUsable && FlameShock.IsHostileDistanceGood && FlameShock.KnownSpell
                && MySettings.UseFlameShock && (!FlameShock.TargetHaveBuff || _flameShockTimer.IsReady))
            {
                FlameShock.Cast();
                _flameShockTimer = new Timer(1000*27);
                return;
            }
            if (LavaBurst.KnownSpell && LavaBurst.IsSpellUsable && LavaBurst.IsHostileDistanceGood
                && MySettings.UseLavaBurst && FlameShock.TargetHaveBuff)
            {
                LavaBurst.Cast();
                return;
            }
            if (EarthShock.IsSpellUsable && EarthShock.KnownSpell && EarthShock.IsHostileDistanceGood
                && MySettings.UseEarthShock && FlameShock.TargetHaveBuff)
            {
                EarthShock.Cast();
                return;
            }
            if (ObjectManager.GetNumberAttackPlayer() > 1 && MagmaTotem.KnownSpell
                && MagmaTotem.IsSpellUsable && MySettings.UseMagmaTotem
                && !FireElementalTotem.CreatedBySpell)
            {
                MagmaTotem.Cast();
                return;
            }
            if (SearingTotem.KnownSpell && SearingTotem.IsSpellUsable && MySettings.UseSearingTotem
                && FireTotemReady() && !SearingTotem.CreatedBySpellInRange(25) && ObjectManager.Target.GetDistance < 31)
            {
                SearingTotem.Cast();
                return;
            }
            if (ObjectManager.GetNumberAttackPlayer() > 1 && ChainLightning.KnownSpell
                && ChainLightning.IsSpellUsable && ChainLightning.IsHostileDistanceGood
                && MySettings.UseChainLightning && !ObjectManager.Me.HaveBuff(77762))
            {
                if (AncestralSwiftness.KnownSpell && AncestralSwiftness.IsSpellUsable
                    && MySettings.UseAncestralSwiftness)
                {
                    AncestralSwiftness.Cast();
                    Others.SafeSleep(200);
                }
                ChainLightning.Cast();
                return;
            }
            if (LightningBolt.IsHostileDistanceGood && LightningBolt.KnownSpell && LightningBolt.IsSpellUsable
                && MySettings.UseLightningBolt && !ObjectManager.Me.HaveBuff(77762))
            {
                if (AncestralSwiftness.KnownSpell && AncestralSwiftness.IsSpellUsable
                    && MySettings.UseAncestralSwiftness)
                {
                    AncestralSwiftness.Cast();
                    Others.SafeSleep(200);
                }
                LightningBolt.Cast();
            }
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    private bool FireTotemReady()
    {
        return !FireElementalTotem.CreatedBySpell && !MagmaTotem.CreatedBySpell;
    }

    private bool EarthTotemReady()
    {
        return !EarthbindTotem.CreatedBySpell && !EarthElementalTotem.CreatedBySpell && !StoneBulwarkTotem.CreatedBySpell;
    }

    private bool WaterTotemReady()
    {
        return !HealingStreamTotem.CreatedBySpell && !HealingTideTotem.CreatedBySpell && !ManaTideTotem.CreatedBySpell;
    }

    private bool AirTotemReady()
    {
        return !CapacitorTotem.CreatedBySpell && !GroundingTotem.CreatedBySpell && !StormlashTotem.CreatedBySpell && !SpiritLinkTotem.CreatedBySpell;
    }

    private bool TotemicRecallReady()
    {
        if (FireElementalTotem.CreatedBySpell)
            return false;
        if (EarthElementalTotem.CreatedBySpell)
            return false;
        if (SearingTotem.CreatedBySpell)
            return true;
        if (FireTotemReady() && EarthTotemReady() && WaterTotemReady() && AirTotemReady())
            return false;
        return true;
    }

    private void Patrolling()
    {
        if (ObjectManager.Me.IsMounted) return;
        Buff();
        Heal();
    }

    #region Nested type: ShamanRestorationSettings

    [Serializable]
    public class ShamanRestorationSettings : Settings
    {
        public bool DoAvoidMelee = false;
        public int DoAvoidMeleeDistance = 0;
        public bool UseAlchFlask = true;
        public bool UseAncestralGuidance = true;
        public bool UseAncestralSwiftness = true;
        public bool UseArcaneTorrentForDecast = true;
        public int UseArcaneTorrentForDecastAtPercentage = 100;
        public bool UseArcaneTorrentForResource = true;
        public int UseArcaneTorrentForResourceAtPercentage = 80;
        public bool UseAscendance = true;
        public bool UseAstralShift = true;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseBloodlustHeroism = true;
        public bool UseCalloftheElements = true;
        public bool UseCapacitorTotem = true;
        public bool UseChainHeal = false;
        public bool UseChainLightning = true;
        public bool UseEarthElementalTotem = true;
        public bool UseEarthShield = true;
        public bool UseEarthShock = true;
        public bool UseEarthbindTotem = false;
        public bool UseEarthlivingWeapon = true;
        public bool UseElementalBlast = true;
        public bool UseElementalMastery = true;

        public bool UseFireElementalTotem = true;
        public bool UseFlameShock = true;
        public bool UseFlametongueWeapon = true;
        public bool UseFrostShock = false;
        public bool UseFrostbrandWeapon = false;
        public bool UseGhostWolf = true;
        public bool UseGiftoftheNaaru = true;
        public int UseGiftoftheNaaruAtPercentage = 80;
        public bool UseGreaterHealingWave = true;
        public bool UseGroundingTotem = true;
        public bool UseHealingRain = true;
        public bool UseHealingStreamTotem = true;
        public bool UseHealingSurge = true;
        public bool UseHealingTideTotem = true;
        public bool UseHealingWave = false;
        public bool UseLavaBurst = true;

        public bool UseLightningBolt = true;
        public bool UseLightningShield = true;
        public bool UseLowCombat = true;
        public bool UseMagmaTotem = true;
        public bool UseManaTideTotem = true;
        public bool UsePrimalStrike = true;
        public bool UseRiptide = true;
        public bool UseRockbiterWeapon = false;
        public bool UseSearingTotem = true;
        public bool UseSpiritLinkTotem = true;
        public bool UseSpiritwalkersGrace = true;
        public bool UseStoneBulwarkTotem = true;
        public bool UseStoneform = true;
        public int UseStoneformAtPercentage = 80;
        public bool UseStormlashTotem = true;
        public bool UseTotemicProjection = true;
        public bool UseTotemicRecall = true;
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;
        public bool UseUnleashElements = true;
        public bool UseWarStomp = true;
        public int UseWarStompAtPercentage = 80;
        public bool UseWaterShield = true;
        public bool UseWaterWalking = true;
        public bool UseWindShear = true;

        public ShamanRestorationSettings()
        {
            ConfigWinForm("Shaman Restoration Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent for Interrupt", "UseArcaneTorrentForDecast", "Professions & Racials", "AtPercentage");
            AddControlInWinForm("Use Arcane Torrent for Resource", "UseArcaneTorrentForResource", "Professions & Racials", "AtPercentage");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");

            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            /* Shaman Buffs */
            AddControlInWinForm("Use Earth Shield", "UseEarthShield", "Shaman Buffs");
            AddControlInWinForm("Use Earthliving Weapon", "UseEarthlivingWeapon", "Shaman Buffs");
            AddControlInWinForm("Use Flametongue Weapon", "UseFlametongueWeapon", "Shaman Buffs");
            AddControlInWinForm("Use Frostbrand Weapon", "UseFrostbrandWeapon", "Shaman Buffs");
            AddControlInWinForm("Use Ghost Wolf", "UseGhostWolf", "Shaman Buffs");
            AddControlInWinForm("Use Lightning Shield", "UseLightningShield", "Shaman Buffs");
            AddControlInWinForm("Use Rockbiter Weapon", "UseRockbiterWeapon", "Shaman Buffs");
            AddControlInWinForm("Use Spiritwalker's Grace", "UseSpiritwalkersGrace", "Shaman Buffs");
            AddControlInWinForm("Use Water Shield", "UseWaterShield", "Shaman Buffs");
            AddControlInWinForm("Use Water Walking", "UseWaterWalking", "Shaman Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Chain Lightning", "UseChainLightning", "Offensive Spell");
            AddControlInWinForm("Use Earth Shock", "UseEarthShock", "Offensive Spell");
            AddControlInWinForm("Use Flame Shock", "UseFlameShock", "Offensive Spell");
            AddControlInWinForm("Use Frost Shock", "UseFrostShock", "Offensive Spell");
            AddControlInWinForm("Use Lava Burst", "UseLavaBurst", "Offensive Spell");
            AddControlInWinForm("Use Lightning Bolt", "UseLightningBolt", "Offensive Spell");
            AddControlInWinForm("Use Magma Totem", "UseMagmaTotem", "Offensive Spell");
            AddControlInWinForm("Use Searing Totem", "UseSearingTotem", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use Ancestral Swiftness", "UseAncestralSwiftness", "Offensive Cooldown");
            AddControlInWinForm("Use Ascendance", "UseAscendance", "Offensive Cooldown");
            AddControlInWinForm("Use Bloodlust / Heroism", "UseBloodlustHeroism", "Offensive Cooldown");
            AddControlInWinForm("Use Call of the Elements", "UseCalloftheElements", "Offensive Cooldown");
            AddControlInWinForm("Use Earth Elemental Totem", "UseEarthElementalTotem", "Offensive Cooldown");
            AddControlInWinForm("Use Elemental Blast", "UseElementalBlast", "Offensive Cooldown");
            AddControlInWinForm("Use Elemental Mastery", "UseElementalMastery", "Offensive Cooldown");
            AddControlInWinForm("Use Fire Elemental Totem", "UseFireElementalTotem", "Offensive Cooldown");
            AddControlInWinForm("Use Stormlash Totem", "UseStormlashTotem", "Offensive Cooldown");
            AddControlInWinForm("Use Totemic Projection", "UseTotemicProjection", "Offensive Cooldown");
            AddControlInWinForm("Use Unleash Elements", "UseUnleashElements", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Astral Shift", "UseAstralShift", "Defensive Cooldown");
            AddControlInWinForm("Use Capacitor Totem", "UseCapacitorTotem", "Defensive Cooldown");
            AddControlInWinForm("Use Earthbind Totem", "UseEarthbindTotem", "Defensive Cooldown");
            AddControlInWinForm("Use Grounding Totem", "UseGroundingTotem", "Defensive Cooldown");
            AddControlInWinForm("Use StoneBulwark Totem", "UseStoneBulwarkTotem", "Defensive Cooldown");
            AddControlInWinForm("Use Wind Shear", "UseWindShear", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Ancestral Guidance", "UseAncestralGuidance", "Healing Spell");
            AddControlInWinForm("Use Chain Heal", "UseChainHeal", "Healing Spell");
            AddControlInWinForm("Use Greater Healing Wave", "UseGreaterHealingWave", "Healing Spell");
            AddControlInWinForm("Use Healing Rain", "UseHealingRain", "Healing Spell");
            AddControlInWinForm("Use Healing Surge", "UseHealingSurge", "Healing Spell");
            AddControlInWinForm("Use Healing Stream Totem", "UseHealingStreamTotem", "Healing Spell");
            AddControlInWinForm("Use Healing Tide Totem", "UsHealingTideTotem", "Healing Spell");
            AddControlInWinForm("Use Healing Wave", "UseHealingWave", "Healing Spell");
            AddControlInWinForm("Use Mana Tide Totem", "UseManaTideTotem", "Healing Spell");
            AddControlInWinForm("Use Riptide", "UseRiptide", "Healing Spell");
            AddControlInWinForm("Use Spirit Link Totem", "UseSpiritLinkTotem", "Healing Spell");
            AddControlInWinForm("Use Totemic Recall", "UseTotemicRecall", "Healing Spell");
            /* Game Settings */
            AddControlInWinForm("Use Low Combat Settings", "UseLowCombat", "Game Settings");
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");

            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
            AddControlInWinForm("Do avoid melee (Off Advised!!)", "DoAvoidMelee", "Game Settings");
            AddControlInWinForm("Avoid melee distance (1 to 4)", "DoAvoidMeleeDistance", "Game Settings");
        }

        public static ShamanRestorationSettings CurrentSetting { get; set; }

        public static ShamanRestorationSettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Shaman_Restoration.xml";
            if (File.Exists(currentSettingsFile))
            {
                return
                    CurrentSetting = Load<ShamanRestorationSettings>(currentSettingsFile);
            }
            return new ShamanRestorationSettings();
        }
    }

    #endregion
}

public class ShamanElemental
{
    private static ShamanElementalSettings MySettings = ShamanElementalSettings.GetSettings();

    #region General Timers & Variables

    private readonly WoWItem _firstTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET);
    private readonly WoWItem _secondTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET, 2);
    public int LC = 0;

    private Timer _onCd = new Timer(0);

    #endregion

    #region Professions & Racials

    public readonly Spell Alchemy = new Spell("Alchemy");
    public readonly Spell ArcaneTorrent = new Spell("Arcane Torrent");
    public readonly Spell Berserking = new Spell("Berserking");
    public readonly Spell BloodFury = new Spell("Blood Fury");

    public readonly Spell GiftoftheNaaru = new Spell("Gift of the Naaru");

    public readonly Spell Stoneform = new Spell("Stoneform");
    public readonly Spell WarStomp = new Spell("War Stomp");

    #endregion

    #region Shaman Buffs

    public readonly Spell FlametongueWeapon = new Spell("Flametongue Weapon");
    public readonly Spell FrostbrandWeapon = new Spell("Frostbrand Weapon");
    public readonly Spell GhostWolf = new Spell("Ghost Wolf");
    public readonly Spell LightningShield = new Spell("Lightning Shield");
    public readonly Spell ImprovedLightningShield = new Spell("Improved Lightning Shield");
    public readonly Spell RockbiterWeapon = new Spell("Rockbiter Weapon");
    public readonly Spell SpiritwalkersGrace = new Spell("Spiritwalker's Grace");
    public readonly Spell WaterShield = new Spell("Water Shield");
    public readonly Spell WaterWalking = new Spell("Water Walking");
    private Timer _waterWalkingTimer = new Timer(0);

    #endregion

    #region Offensive Spell

    public readonly Spell ChainLightning = new Spell("Chain Lightning");
    public readonly Spell EarthShock = new Spell("Earth Shock");
    public readonly Spell FlameShock = new Spell("Flame Shock");
    public readonly Spell FrostShock = new Spell("Frost Shock");
    public readonly Spell LavaBurst = new Spell("Lava Burst");
    public readonly Spell LightningBolt = new Spell("Lightning Bolt");
    public readonly Spell SearingTotem = new Spell("Searing Totem");
    public readonly Spell Thunderstorm = new Spell("Thunderstorm");

    #endregion

    #region Offensive Cooldown

    public readonly Spell AncestralSwiftness = new Spell("Ancestral Swiftness");
    public readonly Spell Ascendance = new Spell("Ascendance");
    public readonly Spell Bloodlust = new Spell("Bloodlust");
    public readonly Spell CalloftheElements = new Spell("Call of the Elements");
    public readonly Spell EarthElementalTotem = new Spell("Earth Elemental Totem");
    public readonly Spell ElementalBlast = new Spell("Elemental Blast");
    public readonly Spell ElementalMastery = new Spell("Elemental Mastery");
    public readonly Spell FireElementalTotem = new Spell("Fire Elemental Totem");
    public readonly Spell Heroism = new Spell("Heroism");
    public readonly Spell StormlashTotem = new Spell("Stormlash Totem");
    public readonly Spell TotemicProjection = new Spell("Totemic Projection");
    public readonly Spell UnleashElements = new Spell("Unleash Elements");
    public readonly Spell UnleashedFury = new Spell("Unleashed Fury");

    #endregion

    #region Defensive Cooldown

    public readonly Spell AstralShift = new Spell("Astral Shift");
    public readonly Spell CapacitorTotem = new Spell("Capacitor Totem");
    public readonly Spell EarthbindTotem = new Spell("Earthbind Totem");
    public readonly Spell GroundingTotem = new Spell("Grounding Totem");
    public readonly Spell StoneBulwarkTotem = new Spell("Stone Bulwark Totem");
    public readonly Spell WindShear = new Spell("Wind Shear");

    #endregion

    #region Healing Spell

    public readonly Spell AncestralGuidance = new Spell("Ancestral Guidance");
    public readonly Spell ChainHeal = new Spell("Chain Heal");
    public readonly Spell HealingRain = new Spell("Healing Rain");
    public readonly Spell HealingStreamTotem = new Spell("Healing Stream Totem");
    public readonly Spell HealingSurge = new Spell("Healing Surge");
    public readonly Spell HealingTideTotem = new Spell("Healing Tide Totem");
    public readonly Spell TotemicRecall = new Spell("Totemic Recall");

    #endregion

    #region Talent

    public readonly Spell Conductivity = new Spell("Conductivity");

    #endregion

    public ShamanElemental()
    {
        Main.InternalRange = 39f;
        Main.InternalAggroRange = 39f;
        MySettings = ShamanElementalSettings.GetSettings();
        Main.DumpCurrentSettings<ShamanElementalSettings>(MySettings);
        UInt128 lastTarget = 0;

        while (Main.InternalLoop)
        {
            try
            {
                if (!ObjectManager.Me.IsDeadMe)
                {
                    if (!ObjectManager.Me.IsMounted)
                    {
                        if (Fight.InFight && ObjectManager.Me.Target > 0)
                        {
                            if (ObjectManager.Me.Target != lastTarget && EarthShock.IsHostileDistanceGood)
                            {
                                Pull();
                                lastTarget = ObjectManager.Me.Target;
                            }

                            if (ObjectManager.Target.Level < 70 && ObjectManager.Me.Level > 84
                                && MySettings.UseLowCombat)
                            {
                                LC = 1;
                                if (CombatClass.InSpellRange(ObjectManager.Target, 0, Main.InternalRange))
                                    LowCombat();
                            }
                            else
                            {
                                LC = 0;
                                if (CombatClass.InSpellRange(ObjectManager.Target, 0, Main.InternalRange))
                                    Combat();
                            }
                        }
                        if (!ObjectManager.Me.IsCast)
                            Patrolling();
                    }
                }
                else
                    Thread.Sleep(500);
            }
            catch
            {
            }
            Thread.Sleep(100);
        }
    }

    private void Pull()
    {
        if (TotemicProjection.KnownSpell && TotemicProjection.IsSpellUsable && MySettings.UseTotemicProjection)
            TotemicProjection.Cast();

        if (FlameShock.KnownSpell && FlameShock.IsSpellUsable && FlameShock.IsHostileDistanceGood
            && MySettings.UseFlameShock && LC != 1)
        {
            FlameShock.Cast();
            return;
        }
        if (EarthShock.KnownSpell && EarthShock.IsSpellUsable && EarthShock.IsHostileDistanceGood
            && MySettings.UseEarthShock)
        {
            EarthShock.Cast();
        }
    }

    private void LowCombat()
    {
        Buff();
        if (MySettings.DoAvoidMelee)
            AvoidMelee();
        DefenseCycle();
        Heal();

        if (EarthShock.KnownSpell && EarthShock.IsSpellUsable && EarthShock.IsHostileDistanceGood
            && MySettings.UseEarthShock)
        {
            EarthShock.Cast();
            return;
        }
        if (LavaBurst.KnownSpell && LavaBurst.IsSpellUsable && LavaBurst.IsHostileDistanceGood
            && MySettings.UseLavaBurst)
        {
            LavaBurst.Cast();
            return;
        }
        if (ChainLightning.KnownSpell && ChainLightning.IsSpellUsable && ChainLightning.IsHostileDistanceGood
            && MySettings.UseChainLightning)
        {
            ChainLightning.Cast();
            return;
        }
        if (SearingTotem.KnownSpell && SearingTotem.IsSpellUsable && MySettings.UseSearingTotem && !SearingTotem.CreatedBySpellInRange(25) && ObjectManager.GetUnitInSpellRange(30) > 0)
        {
            SearingTotem.Cast();
            return;
        }
    }

    private void Combat()
    {
        Buff();
        DPSBurst();
        if (MySettings.DoAvoidMelee)
            AvoidMelee();
        DPSCycle();
        Decast();
        if (_onCd.IsReady)
            DefenseCycle();
        Heal();
    }

    private void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (WaterWalking.IsSpellUsable && WaterWalking.KnownSpell &&
            (!WaterWalking.HaveBuff || _waterWalkingTimer.IsReady)
            && !ObjectManager.Me.InCombat && MySettings.UseWaterWalking)
        {
            WaterWalking.CastOnSelf();
            _waterWalkingTimer = new Timer(1000*60*9);
            return;
        }
        if (MySettings.UseWaterShield && !WaterShield.HaveBuff && WaterShield.KnownSpell && WaterShield.IsSpellUsable && (!MySettings.UseLightningShield || ObjectManager.Me.ManaPercentage < 5))
        {
            WaterShield.CastOnSelf();
            return;
        }
        if (MySettings.UseLightningShield && (ObjectManager.Me.ManaPercentage > 10 || !MySettings.UseWaterShield) && LightningShield.KnownSpell && LightningShield.IsSpellUsable && !LightningShield.HaveBuff)
        {
            LightningShield.CastOnSelf();
            return;
        }
        if (ObjectManager.Me.InCombat && SpiritwalkersGrace.IsSpellUsable
            && SpiritwalkersGrace.KnownSpell && MySettings.UseSpiritwalkersGrace && ObjectManager.Me.GetMove)
        {
            SpiritwalkersGrace.Cast();
            return;
        }
        if (FlametongueWeapon.KnownSpell && FlametongueWeapon.IsSpellUsable && !ObjectManager.Me.HaveBuff(10400)
            && MySettings.UseFlametongueWeapon)
        {
            FlametongueWeapon.Cast();
            return;
        }
        if (FrostbrandWeapon.KnownSpell && FrostbrandWeapon.IsSpellUsable && !ObjectManager.Me.HaveBuff(8034)
            && MySettings.UseFrostbrandWeapon && !MySettings.UseFlametongueWeapon)
        {
            FrostbrandWeapon.Cast();
            return;
        }
        if (RockbiterWeapon.KnownSpell && RockbiterWeapon.IsSpellUsable && !ObjectManager.Me.HaveBuff(36494)
            && MySettings.UseRockbiterWeapon && !MySettings.UseFlametongueWeapon
            && !MySettings.UseFrostbrandWeapon)
        {
            RockbiterWeapon.Cast();
            return;
        }
        if (MountTask.GetMountCapacity() == MountCapacity.Ground && !ObjectManager.Me.InCombat && GhostWolf.IsSpellUsable && GhostWolf.KnownSpell
            && MySettings.UseGhostWolf && ObjectManager.Me.GetMove && !GhostWolf.HaveBuff
            && ObjectManager.Target.GetDistance > 50)
        {
            GhostWolf.Cast();
            return;
        }
        if (MySettings.UseAlchFlask && !ObjectManager.Me.HaveBuff(79638) && !ObjectManager.Me.HaveBuff(79640) && !ObjectManager.Me.HaveBuff(79639)
            && !ItemsManager.IsItemOnCooldown(75525) && ItemsManager.GetItemCount(75525) > 0)
        {
            ItemsManager.UseItem(75525);
        }
    }

    private void AvoidMelee()
    {
        if (ObjectManager.Target.GetDistance < MySettings.DoAvoidMeleeDistance && ObjectManager.Target.InCombat)
        {
            Logging.WriteFight("Too Close. Moving Back");
            var maxTimeTimer = new Timer(1000*2);
            MovementsAction.MoveBackward(true);
            while (ObjectManager.Target.GetDistance < 2 && ObjectManager.Target.InCombat && !maxTimeTimer.IsReady)
                Others.SafeSleep(300);
            MovementsAction.MoveBackward(false);
            if (maxTimeTimer.IsReady && ObjectManager.Target.GetDistance < 2 && ObjectManager.Target.InCombat)
            {
                MovementsAction.MoveForward(true);
                Others.SafeSleep(1000);
                MovementsAction.MoveForward(false);
                MovementManager.Face(ObjectManager.Target.Position);
            }
        }
    }

    private void DefenseCycle()
    {
        if (MySettings.UseHealingRain && HealingRain.IsSpellUsable)
        {
            SpellManager.CastSpellByIDAndPosition(HealingRain.Id, ObjectManager.Me.Position);
            return;
        }
        if (ObjectManager.Me.HealthPercent < 50 && CapacitorTotem.KnownSpell && CapacitorTotem.IsSpellUsable && AirTotemReady() && MySettings.UseCapacitorTotem)
        {
            CapacitorTotem.Cast();
            _onCd = new Timer(1000*5);
            return;
        }
        if (ObjectManager.Me.HealthPercent < 50 && StoneBulwarkTotem.KnownSpell && StoneBulwarkTotem.IsSpellUsable && EarthTotemReady() && MySettings.UseStoneBulwarkTotem)
        {
            StoneBulwarkTotem.Cast();
            _onCd = new Timer(1000*10);
            return;
        }
        if (ObjectManager.Me.HealthPercent <= MySettings.UseWarStompAtPercentage && WarStomp.IsSpellUsable && WarStomp.KnownSpell
            && MySettings.UseWarStomp)
        {
            WarStomp.Cast();
            _onCd = new Timer(1000*2);
            return;
        }
        if (ObjectManager.Me.HealthPercent <= MySettings.UseStoneformAtPercentage && Stoneform.IsSpellUsable && Stoneform.KnownSpell
            && MySettings.UseStoneform)
        {
            Stoneform.Cast();
            _onCd = new Timer(1000*8);
            return;
        }
        if (ObjectManager.Me.HealthPercent < 70 && AstralShift.KnownSpell && AstralShift.IsSpellUsable && MySettings.UseAstralShift)
        {
            AstralShift.Cast();
            _onCd = new Timer(1000*6);
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.IsMounted)
            return;
        if (ArcaneTorrent.IsSpellUsable && ArcaneTorrent.KnownSpell &&
            ObjectManager.Me.ManaPercentage <= MySettings.UseArcaneTorrentForResourceAtPercentage
            && MySettings.UseArcaneTorrentForResource)
        {
            ArcaneTorrent.Cast();
            return;
        }
        if (ObjectManager.Me.ManaPercentage < 50 && TotemicRecall.KnownSpell && TotemicRecall.IsSpellUsable
            && MySettings.UseTotemicRecall && !ObjectManager.Me.InCombat
            && TotemicRecallReady())
        {
            TotemicRecall.Cast();
            return;
        }
        if (Conductivity.KnownSpell && HealingRain.KnownSpell && MySettings.UseHealingRain
            && HealingRain.IsSpellUsable && ObjectManager.Me.InCombat
            && (ObjectManager.GetNumberAttackPlayer() > 1 || ObjectManager.Target.Health > ObjectManager.Me.MaxHealth))
        {
            SpellManager.CastSpellByIDAndPosition(73920, ObjectManager.Me.Position);
            while (ObjectManager.Me.IsCast)
            {
                Others.SafeSleep(200);
            }
            return;
        }
        if (ObjectManager.Me.HealthPercent < 95 && HealingSurge.KnownSpell && HealingSurge.IsSpellUsable
            && !ObjectManager.Me.InCombat && MySettings.UseHealingSurge)
        {
            HealingSurge.Cast();
            while (ObjectManager.Me.IsCast)
            {
                Others.SafeSleep(200);
            }
            return;
        }
        if (HealingSurge.KnownSpell && HealingSurge.IsSpellUsable && ObjectManager.Me.HealthPercent < 50
            && MySettings.UseHealingSurge)
        {
            HealingSurge.Cast();
            return;
        }
        if (ObjectManager.Me.HealthPercent <= MySettings.UseGiftoftheNaaruAtPercentage &&
            GiftoftheNaaru.KnownSpell && GiftoftheNaaru.IsSpellUsable
            && MySettings.UseGiftoftheNaaru)
        {
            GiftoftheNaaru.Cast();
            return;
        }
        if (HealingTideTotem.KnownSpell && HealingTideTotem.IsSpellUsable &&
            ObjectManager.Me.HealthPercent < 70
            && WaterTotemReady() && MySettings.UseHealingTideTotem)
        {
            HealingTideTotem.Cast();
            return;
        }
        if (AncestralGuidance.KnownSpell && AncestralGuidance.IsSpellUsable &&
            ObjectManager.Me.HealthPercent < 70
            && MySettings.UseAncestralGuidance)
        {
            AncestralGuidance.Cast();
            return;
        }
        if (ChainHeal.KnownSpell && ChainHeal.IsSpellUsable && ObjectManager.Me.HealthPercent < 80
            && MySettings.UseChainHeal)
        {
            ChainHeal.Cast();
            return;
        }
        if (HealingStreamTotem.KnownSpell && HealingStreamTotem.IsSpellUsable &&
            ObjectManager.Me.HealthPercent < 90
            && WaterTotemReady() && MySettings.UseHealingStreamTotem)
        {
            HealingStreamTotem.Cast();
        }
    }

    private void Decast()
    {
        if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && MySettings.UseWindShear
            && WindShear.KnownSpell && WindShear.IsSpellUsable && WindShear.IsHostileDistanceGood)
        {
            WindShear.Cast();
            return;
        }
        if (ArcaneTorrent.IsSpellUsable && ArcaneTorrent.KnownSpell && ObjectManager.Target.GetDistance < 8
            && ObjectManager.Me.HealthPercent <= MySettings.UseArcaneTorrentForDecastAtPercentage
            && MySettings.UseArcaneTorrentForDecast && ObjectManager.Target.IsCast &&
            ObjectManager.Target.IsTargetingMe)
        {
            ArcaneTorrent.Cast();
            return;
        }
        if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && MySettings.UseGroundingTotem
            && GroundingTotem.KnownSpell && GroundingTotem.IsSpellUsable && AirTotemReady())
        {
            GroundingTotem.Cast();
            return;
        }
        if (ObjectManager.Target.GetMove && !FrostShock.TargetHaveBuff && MySettings.UseFrostShock
            && FrostShock.KnownSpell && FrostShock.IsSpellUsable && FrostShock.IsHostileDistanceGood)
        {
            FrostShock.Cast();
            return;
        }
        if (ObjectManager.Target.GetMove && MySettings.UseEarthbindTotem && EarthTotemReady()
            && EarthbindTotem.KnownSpell && EarthbindTotem.IsSpellUsable && EarthbindTotem.IsHostileDistanceGood)
        {
            EarthbindTotem.Cast();
        }
    }

    private void DPSBurst()
    {
        if (MySettings.UseTrinketOne && !ItemsManager.IsItemOnCooldown(_firstTrinket.Entry) && ItemsManager.IsItemUsable(_firstTrinket.Entry))
        {
            ItemsManager.UseItem(_firstTrinket.Name);
            Logging.WriteFight("Use First Trinket Slot");
        }

        if (MySettings.UseTrinketTwo && !ItemsManager.IsItemOnCooldown(_secondTrinket.Entry) && ItemsManager.IsItemUsable(_secondTrinket.Entry))
        {
            ItemsManager.UseItem(_secondTrinket.Name);
            Logging.WriteFight("Use Second Trinket Slot");
            return;
        }
        if (Berserking.IsSpellUsable && Berserking.KnownSpell && ObjectManager.Target.GetDistance <= 40f
            && MySettings.UseBerserking)
        {
            Berserking.Cast();
            return;
        }
        if (BloodFury.IsSpellUsable && BloodFury.KnownSpell && ObjectManager.Target.GetDistance <= 40f
            && MySettings.UseBloodFury)
        {
            BloodFury.Cast();
            return;
        }
        if (UnleashElements.KnownSpell && UnleashElements.IsSpellUsable && UnleashedFury.KnownSpell
            && MySettings.UseUnleashElements && UnleashElements.IsHostileDistanceGood)
        {
            UnleashElements.Cast();
            return;
        }
        if (ElementalBlast.KnownSpell && ElementalBlast.IsSpellUsable
            && MySettings.UseElementalBlast && ElementalBlast.IsHostileDistanceGood)
        {
            ElementalBlast.Cast();
            return;
        }
        if (MySettings.UseAscendance && Ascendance.IsSpellUsable && ObjectManager.GetUnitInSpellRange(Main.InternalRange) > 0)
        {
            Ascendance.Cast();
            return;
        }
        if (FireElementalTotem.KnownSpell && FireElementalTotem.IsSpellUsable
            && MySettings.UseFireElementalTotem && ObjectManager.Target.GetDistance <= 40f)
        {
            FireElementalTotem.Cast();
            return;
        }
        if (StormlashTotem.KnownSpell && AirTotemReady()
            && MySettings.UseStormlashTotem && ObjectManager.Target.GetDistance <= 40f)
        {
            if (!StormlashTotem.IsSpellUsable && MySettings.UseCalloftheElements
                && CalloftheElements.KnownSpell && CalloftheElements.IsSpellUsable)
            {
                CalloftheElements.Cast();
                Others.SafeSleep(200);
            }

            if (StormlashTotem.IsSpellUsable)
                StormlashTotem.Cast();
            return;
        }
        if (Bloodlust.KnownSpell && Bloodlust.IsSpellUsable && MySettings.UseBloodlustHeroism
            && ObjectManager.Target.GetDistance <= 40f && !ObjectManager.Me.HaveBuff(57724))
        {
            Bloodlust.Cast();
            return;
        }

        if (Heroism.KnownSpell && Heroism.IsSpellUsable && MySettings.UseBloodlustHeroism
            && ObjectManager.Target.GetDistance <= 40f && !ObjectManager.Me.HaveBuff(57723))
        {
            Heroism.Cast();
            return;
        }
        if (ElementalMastery.KnownSpell && ElementalMastery.IsSpellUsable
            && !ObjectManager.Me.HaveBuff(2825) && MySettings.UseElementalMastery
            && !ObjectManager.Me.HaveBuff(32182))
        {
            ElementalMastery.Cast();
        }
    }

    private void DPSCycle()
    {
        Usefuls.SleepGlobalCooldown();
        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            if (MySettings.UseLavaBurst && LavaBurst.IsSpellUsable && LavaBurst.IsHostileDistanceGood && (FlameShock.TargetHaveBuff || !FlameShock.KnownSpell || !MySettings.UseFlameShock))
            {
                LavaBurst.Cast();
                return;
            }
            if (MySettings.UseFlameShock && FlameShock.IsSpellUsable && FlameShock.IsHostileDistanceGood && (!FlameShock.TargetHaveBuff || ObjectManager.Target.AuraIsActiveAndExpireInLessThanMs(FlameShock.Id, 9000)))
            {
                FlameShock.Cast();
                return;
            }
            if (MySettings.UseEarthShock && EarthShock.IsHostileDistanceGood && EarthShock.IsSpellUsable &&
                (LightningShield.BuffStack >= 17 || (ImprovedLightningShield.KnownSpell && LightningShield.BuffStack >= 15) || !MySettings.UseLightningShield || !LightningShield.KnownSpell))
            {
                EarthShock.Cast();
                return;
            }
            if (MySettings.UseElementalBlast && ElementalBlast.IsSpellUsable && ElementalBlast.IsHostileDistanceGood)
            {
                ElementalBlast.Cast();
                return;
            }
            if (MySettings.UseLavaBurst && LavaBurst.IsSpellUsable && LavaBurst.IsHostileDistanceGood)
            {
                LavaBurst.Cast();
                return;
            }
            if (MySettings.UseSearingTotem && SearingTotem.IsSpellUsable && FireTotemReady() && !SearingTotem.CreatedBySpellInRange(25) && ObjectManager.Target.GetDistance < 31)
            {
                SearingTotem.Cast();
                return;
            }
            if (MySettings.UseChainLightning && ChainLightning.IsSpellUsable && ChainLightning.IsHostileDistanceGood && ObjectManager.GetUnitInSpellRange(ChainLightning.MaxRangeHostile) > 1)
            {
                if (MySettings.UseAncestralSwiftness && AncestralSwiftness.IsSpellUsable)
                {
                    AncestralSwiftness.Cast();
                    Others.SafeSleep(200);
                }
                ChainLightning.Cast();
                return;
            }
            if (MySettings.UseLightningBolt && LightningBolt.IsSpellUsable && LightningBolt.IsHostileDistanceGood)
            {
                if (MySettings.UseAncestralSwiftness && AncestralSwiftness.IsSpellUsable)
                {
                    AncestralSwiftness.Cast();
                    Others.SafeSleep(200);
                }
                LightningBolt.Cast();
                return;
            }
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    private bool FireTotemReady()
    {
        return !FireElementalTotem.CreatedBySpell;
    }

    private bool EarthTotemReady()
    {
        return !EarthbindTotem.CreatedBySpell && !EarthElementalTotem.CreatedBySpell && !StoneBulwarkTotem.CreatedBySpell;
    }

    private bool WaterTotemReady()
    {
        return !HealingStreamTotem.CreatedBySpell && !HealingTideTotem.CreatedBySpell;
    }

    private bool AirTotemReady()
    {
        return !CapacitorTotem.CreatedBySpell && !GroundingTotem.CreatedBySpell && !StormlashTotem.CreatedBySpell;
    }

    private bool TotemicRecallReady()
    {
        if (FireElementalTotem.CreatedBySpell)
            return false;
        if (EarthElementalTotem.CreatedBySpell)
            return false;
        if (SearingTotem.CreatedBySpell)
            return true;
        if (FireTotemReady() && EarthTotemReady() && WaterTotemReady() && AirTotemReady())
            return false;
        return true;
    }

    private void Patrolling()
    {
        if (ObjectManager.Me.IsMounted) return;
        Buff();
        Heal();
    }

    #region Nested type: ShamanElementalSettings

    [Serializable]
    public class ShamanElementalSettings : Settings
    {
        public bool DoAvoidMelee = false;
        public int DoAvoidMeleeDistance = 0;
        public bool UseAlchFlask = true;
        public bool UseAncestralGuidance = true;
        public bool UseAncestralSwiftness = true;
        public bool UseArcaneTorrentForDecast = true;
        public int UseArcaneTorrentForDecastAtPercentage = 100;
        public bool UseArcaneTorrentForResource = true;
        public int UseArcaneTorrentForResourceAtPercentage = 80;
        public bool UseAscendance = true;
        public bool UseAstralShift = true;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseBloodlustHeroism = true;
        public bool UseCalloftheElements = true;
        public bool UseCapacitorTotem = true;
        public bool UseChainHeal = false;
        public bool UseChainLightning = true;
        public bool UseEarthElementalTotem = true;
        public bool UseEarthShock = true;
        public bool UseEarthbindTotem = false;
        public bool UseEarthquake = true;
        public bool UseElementalBlast = true;
        public bool UseElementalMastery = true;

        public bool UseFireElementalTotem = true;
        public bool UseFlameShock = true;
        public bool UseFlametongueWeapon = true;
        public bool UseFrostShock = false;
        public bool UseFrostbrandWeapon = false;
        public bool UseGhostWolf = true;
        public bool UseGiftoftheNaaru = true;
        public int UseGiftoftheNaaruAtPercentage = 80;
        public bool UseGroundingTotem = true;
        public bool UseHealingRain = true;
        public bool UseHealingStreamTotem = true;
        public bool UseHealingSurge = true;
        public bool UseHealingTideTotem = true;
        public bool UseLavaBurst = true;

        public bool UseLightningBolt = true;
        public bool UseLightningShield = true;
        public bool UseLowCombat = true;
        public bool UseMagmaTotem = true;
        public bool UseRockbiterWeapon = false;
        public bool UseSearingTotem = true;
        public bool UseSpiritwalkersGrace = true;
        public bool UseStoneBulwarkTotem = true;
        public bool UseStoneform = true;
        public int UseStoneformAtPercentage = 80;
        public bool UseStormlashTotem = true;
        public bool UseThunderstorm = true;
        public bool UseTotemicProjection = true;
        public bool UseTotemicRecall = true;
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;
        public bool UseUnleashElements = true;
        public bool UseWarStomp = true;
        public int UseWarStompAtPercentage = 70;
        public bool UseWaterShield = true;
        public bool UseWaterWalking = true;
        public bool UseWindShear = true;

        public ShamanElementalSettings()
        {
            ConfigWinForm("Shaman Elemental Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent for Interrupt", "UseArcaneTorrentForDecast", "Professions & Racials", "AtPercentage");
            AddControlInWinForm("Use Arcane Torrent for Resource", "UseArcaneTorrentForResource", "Professions & Racials", "AtPercentage");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");

            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            /* Shaman Buffs */
            AddControlInWinForm("Use Flametongue Weapon", "UseFlametongueWeapon", "Shaman Buffs");
            AddControlInWinForm("Use Frostbrand Weapon", "UseFrostbrandWeapon", "Shaman Buffs");
            AddControlInWinForm("Use Ghost Wolf", "UseGhostWolf", "Shaman Buffs");
            AddControlInWinForm("Use Lightning Shield", "UseLightningShield", "Shaman Buffs");
            AddControlInWinForm("Use Rockbiter Weapon", "UseRockbiterWeapon", "Shaman Buffs");
            AddControlInWinForm("Use Spiritwalker's Grace", "UseSpiritwalkersGrace", "Shaman Buffs");
            AddControlInWinForm("Use Water Shield", "UseWaterShield", "Shaman Buffs");
            AddControlInWinForm("Use Water Walking", "UseWaterWalking", "Shaman Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Chain Lightning", "UseChainLightning", "Offensive Spell");
            AddControlInWinForm("Use Earthquake", "UseEarthquake", "Offensive Spell");
            AddControlInWinForm("Use Earth Shock", "UseEarthShock", "Offensive Spell");
            AddControlInWinForm("Use Flame Shock", "UseFlameShock", "Offensive Spell");
            AddControlInWinForm("Use Frost Shock", "UseFrostShock", "Offensive Spell");
            AddControlInWinForm("Use Lava Burst", "UseLavaBurst", "Offensive Spell");
            AddControlInWinForm("Use Lightning Bolt", "UseLightningBolt", "Offensive Spell");
            AddControlInWinForm("Use Magma Totem", "UseMagmaTotem", "Offensive Spell");
            AddControlInWinForm("Use Searing Totem", "UseSearingTotem", "Offensive Spell");
            AddControlInWinForm("Use Thunderstorm", "UseThunderstorm", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use Ancestral Swiftness", "UseAncestralSwiftness", "Offensive Cooldown");
            AddControlInWinForm("Use Ascendance", "UseAscendance", "Offensive Cooldown");
            AddControlInWinForm("Use Bloodlust / Heroism", "UseBloodlustHeroism", "Offensive Cooldown");
            AddControlInWinForm("Use Call of the Elements", "UseCalloftheElements", "Offensive Cooldown");
            AddControlInWinForm("Use Earth Elemental Totem", "UseEarthElementalTotem", "Offensive Cooldown");
            AddControlInWinForm("Use Elemental Blast", "UseElementalBlast", "Offensive Cooldown");
            AddControlInWinForm("Use Elemental Mastery", "UseElementalMastery", "Offensive Cooldown");
            AddControlInWinForm("Use Fire Elemental Totem", "UseFireElementalTotem", "Offensive Cooldown");
            AddControlInWinForm("Use Stormlash Totem", "UseStormlashTotem", "Offensive Cooldown");
            AddControlInWinForm("Use Totemic Projection", "UseTotemicProjection", "Offensive Cooldown");
            AddControlInWinForm("Use Unleash Elements", "UseUnleashElements", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Astral Shift", "UseAstralShift", "Defensive Cooldown");
            AddControlInWinForm("Use Capacitor Totem", "UseCapacitorTotem", "Defensive Cooldown");
            AddControlInWinForm("Use Earthbind Totem", "UseEarthbindTotem", "Defensive Cooldown");
            AddControlInWinForm("Use Grounding Totem", "UseGroundingTotem", "Defensive Cooldown");
            AddControlInWinForm("Use StoneBulwark Totem", "UseStoneBulwarkTotem", "Defensive Cooldown");
            AddControlInWinForm("Use Wind Shear", "UseWindShear", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Ancestral Guidance", "UseAncestralGuidance", "Healing Spell");
            AddControlInWinForm("Use Chain Heal", "UseChainHeal", "Healing Spell");
            AddControlInWinForm("Use Healing Rain", "UseHealingRain", "Healing Spell");
            AddControlInWinForm("Use Healing Surge", "UseHealingSurge", "Healing Spell");
            AddControlInWinForm("Use Healing Stream Totem", "UseHealingStreamTotem", "Healing Spell");
            AddControlInWinForm("Use Healing Tide Totem", "UsHealingTideTotem", "Healing Spell");
            AddControlInWinForm("Use Totemic Recall", "UseTotemicRecall", "Healing Spell");
            /* Game Settings */
            AddControlInWinForm("Use Low Combat Settings", "UseLowCombat", "Game Settings");
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");

            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
            AddControlInWinForm("Do avoid melee (Off Advised!!)", "DoAvoidMelee", "Game Settings");
            AddControlInWinForm("Avoid melee distance (1 to 4)", "DoAvoidMeleeDistance", "Game Settings");
        }

        public static ShamanElementalSettings CurrentSetting { get; set; }

        public static ShamanElementalSettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Shaman_Elemental.xml";
            if (File.Exists(currentSettingsFile))
            {
                return
                    CurrentSetting = Load<ShamanElementalSettings>(currentSettingsFile);
            }
            return new ShamanElementalSettings();
        }
    }

    #endregion
}

#endregion

// ReSharper restore ObjectCreationAsStatement
// ReSharper restore EmptyGeneralCatchClause