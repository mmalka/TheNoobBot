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
                    #region Monk Specialisation checking

                case WoWClass.Monk:

                    if (wowSpecialization == WoWSpecialization.MonkBrewmaster)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Monk_Brewmaster.xml";
                            var currentSetting = new MonkBrewmaster.MonkBrewmasterSettings();
                            if (File.Exists(currentSettingsFile) && !resetSettings)
                            {
                                currentSetting = Settings.Load<MonkBrewmaster.MonkBrewmasterSettings>(currentSettingsFile);
                            }
                            currentSetting.ToForm();
                            currentSetting.Save(currentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Monk Brewmaster Combat class...");
                            EquipmentAndStats.SetPlayerSpe(WoWSpecialization.MonkBrewmaster);
                            new MonkBrewmaster();
                        }
                        break;
                    }
                    if (wowSpecialization == WoWSpecialization.MonkWindwalker || wowSpecialization == WoWSpecialization.None)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Monk_Windwalker.xml";
                            var currentSetting = new MonkWindwalker.MonkWindwalkerSettings();
                            if (File.Exists(currentSettingsFile) && !resetSettings)
                            {
                                currentSetting = Settings.Load<MonkWindwalker.MonkWindwalkerSettings>(currentSettingsFile);
                            }
                            currentSetting.ToForm();
                            currentSetting.Save(currentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Monk Windwalker Combat class...");
                            EquipmentAndStats.SetPlayerSpe(WoWSpecialization.MonkWindwalker);
                            new MonkWindwalker();
                        }
                        break;
                    }
                    if (wowSpecialization == WoWSpecialization.MonkMistweaver)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Monk_Mistweaver.xml";
                            var currentSetting = new MonkMistweaver.MonkMistweaverSettings();
                            if (File.Exists(currentSettingsFile) && !resetSettings)
                            {
                                currentSetting = Settings.Load<MonkMistweaver.MonkMistweaverSettings>(currentSettingsFile);
                            }
                            currentSetting.ToForm();
                            currentSetting.Save(currentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Monk Mistweaver Combat class...");
                            InternalRange = 30.0f;
                            EquipmentAndStats.SetPlayerSpe(WoWSpecialization.MonkMistweaver);
                            new MonkMistweaver();
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

#region Monk

public class MonkBrewmaster
{
    private static MonkBrewmasterSettings MySettings = MonkBrewmasterSettings.GetSettings();

    #region General Timers & Variables

    private readonly WoWItem _firstTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET);
    private readonly WoWItem _secondTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET, 2);
    public int ElusiveBrewStack = 0;

    private Timer _grappleWeaponTimer = new Timer(0);
    private Timer _healingSphereTimer = new Timer(0);
    private Timer _onCd = new Timer(0);
/*
        private Timer _staggerTimer = new Timer(0);
*/

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

    #region Monk Buffs

    public readonly Spell Disable = new Spell("Disable");
    public readonly Spell LegacyoftheEmperor = new Spell("Legacy of the Emperor");
    public readonly Spell StanceoftheFierceTiger = new Spell("Stance of the Fierce Tiger");
    public readonly Spell StanceoftheSturdyOx = new Spell("Stance of the Sturdy Ox");
    public readonly Spell TigersLust = new Spell("Tiger's Lust");

    #endregion

    #region Offensive Spell

    public readonly Spell BlackoutKick = new Spell("Blackout Kick");
    public readonly Spell BreathofFire = new Spell("Breathe of Fire");
    public readonly Spell Clash = new Spell("Clash");
    public readonly Spell CracklingJadeLightning = new Spell("Crackling Jade Lightning");
    public readonly Spell DizzyingHaze = new Spell("Dizzying Haze");
    public readonly Spell Jab = new Spell("Jab");
    public readonly Spell KegSmash = new Spell("Keg Smash");
    public readonly Spell Provoke = new Spell("Provoke");
    public readonly Spell Roll = new Spell("Roll");
    public readonly Spell SpinningCraneKick = new Spell("Spinning Crane Kick");
    public readonly Spell TigerPalm = new Spell("Tiger Palm");
    public readonly Spell TouchofDeath = new Spell("Touch of Death");

    #endregion

    #region Offensive Cooldown

    public readonly Spell ChiBrew = new Spell("Chi Brew");
    public readonly Spell InvokeXuentheWhiteTiger = new Spell("Invoke Xuen, the White Tiger");
    public readonly Spell RushingJadeWind = new Spell("Rushing Jade Wind");

    #endregion

    #region Defensive Cooldown

    public readonly Spell ChargingOxWave = new Spell("Charging Ox Wave");
    public readonly Spell DampenHarm = new Spell("Dampen Harm");
    public readonly Spell DiffuseMagic = new Spell("Diffuse Magic");
    public readonly Spell ElusiveBrew = new Spell("Elusive Brew");
    public readonly Spell FortifyingBrew = new Spell("Fortifying Brew");
    public readonly Spell GrappleWeapon = new Spell("Grapple Weapon");
    public readonly Spell Guard = new Spell("Guard");
    public readonly Spell LegSweep = new Spell("Leg Sweep");
    public readonly Spell PurifyingBrew = new Spell("Purifying Brew");
    public readonly Spell SpearHandStrike = new Spell("Spear Hand Strike");
    public readonly Spell SummonBlackOxStatue = new Spell("Summon Black Ox Statue");
    public readonly Spell ZenMeditation = new Spell("Zen Meditation");
    private Timer _furifyingBrewTimer = new Timer(0);

    #endregion

    #region Healing Spell

    public readonly Spell ChiBurst = new Spell("Chi Burst");
    public readonly Spell ChiWave = new Spell("Chi Wave");
    public readonly Spell ExpelHarm = new Spell("Expel Harm");
    public readonly Spell HealingSphere = new Spell("Healing Sphere");
    public readonly Spell ZenSphere = new Spell("Zen Sphere");

    #endregion

    public MonkBrewmaster()
    {
        Main.InternalRange = ObjectManager.Me.GetCombatReach;
        Main.InternalAggroRange = 25.0f;
        MySettings = MonkBrewmasterSettings.GetSettings();
        Main.DumpCurrentSettings<MonkBrewmasterSettings>(MySettings);
        UInt128 lastTarget = 0;

        while (Main.InternalLoop)
        {
            try
            {
                if (!ObjectManager.Me.IsDead)
                {
                    if (!ObjectManager.Me.IsMounted)
                    {
                        if (Fight.InFight && ObjectManager.Me.Target > 0)
                        {
                            if (ObjectManager.Me.Target != lastTarget
                                && (Jab.IsHostileDistanceGood || Provoke.IsHostileDistanceGood))
                            {
                                Pull();
                                lastTarget = ObjectManager.Me.Target;
                            }

                            if (ObjectManager.Target.GetDistance < Main.InternalAggroRange)
                                Combat();
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
        if (MySettings.UseClash && Clash.KnownSpell && Clash.IsHostileDistanceGood && Clash.IsSpellUsable)
            Clash.Cast();

        if (MySettings.UseProvoke && Provoke.KnownSpell && !ObjectManager.Target.InCombat && Provoke.IsHostileDistanceGood && Provoke.IsSpellUsable)
        {
            Provoke.Cast();
        }
    }

    private void Combat()
    {
        Buff();
        if (MySettings.DoAvoidMelee)
            AvoidMelee();
        DPSCycle();
        if (_onCd.IsReady &&
            (ObjectManager.Me.HealthPercent <= MySettings.UseGrappleWeaponAtPercentage || ObjectManager.Me.HealthPercent <= MySettings.UseElusiveBrewAtPercentage
             || ObjectManager.Me.HealthPercent <= MySettings.UseFortifyingBrewAtPercentage ||
             ObjectManager.Me.HealthPercent <= MySettings.UseChargingOxWaveAtPercentage
             || ObjectManager.Me.HealthPercent <= MySettings.UseDampenHarmAtPercentage || ObjectManager.Me.HealthPercent <= MySettings.UseLegSweepAtPercentage
             || ObjectManager.Me.HealthPercent <= MySettings.UseGuardAtPercentage || ObjectManager.Me.HealthPercent <= MySettings.UseStoneformAtPercentage
             || ObjectManager.Me.HealthPercent <= MySettings.UseWarStompAtPercentage || ObjectManager.Me.HealthPercent <= MySettings.UsePurifyingBrewAtPercentage))
            DefenseCycle();
        Heal();
        Decast();
        DPSBurst();
        DPSCycle();
    }

    private void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (MySettings.UseLegacyoftheEmperor && LegacyoftheEmperor.KnownSpell && !LegacyoftheEmperor.HaveBuff && LegacyoftheEmperor.IsSpellUsable)
            LegacyoftheEmperor.Cast();

        if (MySettings.UseStanceoftheSturdyOx && StanceoftheSturdyOx.KnownSpell && !StanceoftheSturdyOx.HaveBuff && StanceoftheSturdyOx.IsSpellUsable)
            StanceoftheSturdyOx.Cast();

        if (MySettings.UseTigersLust && TigersLust.KnownSpell && !ObjectManager.Me.InCombat && ObjectManager.Me.GetMove && TigersLust.IsSpellUsable)
            TigersLust.Cast();

        if (MySettings.UseRoll && !ObjectManager.Me.InCombat && Roll.KnownSpell && ObjectManager.Me.GetMove
            && !TigersLust.HaveBuff && Roll.IsSpellUsable && ObjectManager.Target.GetDistance > 14)
            Roll.Cast();

        if (MySettings.UseSummonBlackOxStatue && SummonBlackOxStatue.KnownSpell && !ObjectManager.Me.HaveBuff(126119) && SummonBlackOxStatue.IsSpellUsable
            && ObjectManager.Target.GetDistance < 30 && ObjectManager.Target.InCombat)
            SpellManager.CastSpellByIDAndPosition(115315, ObjectManager.Target.Position);

        if (MySettings.UseAlchFlask && !ObjectManager.Me.HaveBuff(79638) && !ObjectManager.Me.HaveBuff(79640) && !ObjectManager.Me.HaveBuff(79639)
            && !ItemsManager.IsItemOnCooldown(75525) && ItemsManager.GetItemCount(75525) > 0)
            ItemsManager.UseItem(75525);
    }

    private void AvoidMelee()
    {
        if (ObjectManager.Target.GetDistance < 1.0f && ObjectManager.Target.GetDistance < ObjectManager.Target.GetBoundingRadius && ObjectManager.Target.InCombat)
        {
            Logging.WriteFight("Too Close. Moving forward");
            MovementsAction.MoveForward(true);
            Others.SafeSleep(1250);
            MovementsAction.MoveForward(false);
            MovementManager.Face(ObjectManager.Target.Position);
        }
    }

    private void DefenseCycle()
    {
        if (MySettings.UseGrappleWeapon && GrappleWeapon.KnownSpell && GrappleWeapon.IsHostileDistanceGood &&
            ObjectManager.Me.HealthPercent <= MySettings.UseGrappleWeaponAtPercentage
            && _grappleWeaponTimer.IsReady && GrappleWeapon.IsSpellUsable)
        {
            GrappleWeapon.Cast();
            _grappleWeaponTimer = new Timer(1000*60);
            return;
        }
        if (MySettings.UseElusiveBrew && ElusiveBrew.KnownSpell && ObjectManager.Me.InCombat && ObjectManager.Me.HealthPercent <= MySettings.UseElusiveBrewAtPercentage
            && ElusiveBrew.IsSpellUsable && ObjectManager.Me.BuffStack(128939) > 5)
        {
            ElusiveBrewStack = ObjectManager.Me.BuffStack(128939);
            ElusiveBrew.Cast();
            _onCd = new Timer(1000*ElusiveBrewStack);
            return;
        }
        if (MySettings.UseFortifyingBrew && FortifyingBrew.KnownSpell && ObjectManager.Me.HealthPercent <= MySettings.UseFortifyingBrewAtPercentage &&
            FortifyingBrew.IsSpellUsable)
        {
            FortifyingBrew.Cast();
            _onCd = new Timer(1000*20);
            return;
        }
        if (MySettings.UseChargingOxWave && ChargingOxWave.KnownSpell && ChargingOxWave.IsHostileDistanceGood &&
            ObjectManager.Me.HealthPercent <= MySettings.UseChargingOxWaveAtPercentage
            && ChargingOxWave.IsSpellUsable)
        {
            ChargingOxWave.Cast();
            _onCd = new Timer(1000*3);
            return;
        }
        if (MySettings.UseDampenHarm && DampenHarm.KnownSpell && DampenHarm.IsSpellUsable && ObjectManager.Me.HealthPercent <= MySettings.UseDampenHarmAtPercentage)
        {
            DampenHarm.Cast();
            _onCd = new Timer(1000*5);
            return;
        }
        if (MySettings.UseLegSweep && LegSweep.KnownSpell && ObjectManager.Target.GetDistance < 6 &&
            ObjectManager.Me.HealthPercent <= MySettings.UseLegSweepAtPercentage
            && LegSweep.IsSpellUsable)
        {
            LegSweep.Cast();
            _onCd = new Timer(1000*5);
            return;
        }
        if (MySettings.UseGuard && Guard.KnownSpell && ObjectManager.Me.HaveBuff(118636) && Guard.IsSpellUsable &&
            ObjectManager.Me.HealthPercent <= MySettings.UseGuardAtPercentage)
        {
            Guard.Cast();
            _onCd = new Timer(1000*5);
            return;
        }
        if (MySettings.UsePurifyingBrew && PurifyingBrew.KnownSpell && ObjectManager.Me.HaveBuff(124255) && _furifyingBrewTimer.IsReady &&
            PurifyingBrew.IsSpellUsable
            && ObjectManager.Me.HealthPercent <= MySettings.UsePurifyingBrewAtPercentage)
        {
            PurifyingBrew.Cast();
            _furifyingBrewTimer = new Timer(1000*10);
            return;
        }
        if (MySettings.UseStoneform && Stoneform.KnownSpell && Stoneform.IsSpellUsable &&
            ObjectManager.Me.HealthPercent <= MySettings.UseStoneformAtPercentage)
        {
            Stoneform.Cast();
            _onCd = new Timer(1000*8);
            return;
        }
        if (MySettings.UseWarStomp && WarStomp.KnownSpell && ObjectManager.Target.GetDistance < 8 && WarStomp.IsSpellUsable &&
            ObjectManager.Me.HealthPercent <= MySettings.UseWarStompAtPercentage)
        {
            WarStomp.Cast();
            _onCd = new Timer(1000*2);
        }
    }

    private void Heal()
    {
        if (MySettings.UseGiftoftheNaaru && GiftoftheNaaru.KnownSpell && GiftoftheNaaru.IsSpellUsable
            && ObjectManager.Me.HealthPercent <= MySettings.UseGiftoftheNaaruAtPercentage)
        {
            GiftoftheNaaru.Cast();
            return;
        }
        if (MySettings.UseHealingSphere && HealingSphere.KnownSpell && HealingSphere.IsSpellUsable && _healingSphereTimer.IsReady &&
            ObjectManager.Me.HealthPercent <= MySettings.UseHealingSphereAtPercentage)
        {
            SpellManager.CastSpellByIDAndPosition(115460, ObjectManager.Me.Position);
            _healingSphereTimer = new Timer(1000*8);
            return;
        }
        if (MySettings.UseChiWave && ChiWave.KnownSpell && ObjectManager.Me.HealthPercent <= MySettings.UseChiWaveAtPercentage && ChiWave.IsSpellUsable)
        {
            ChiWave.Cast();
            return;
        }
        if (MySettings.UseChiBurst && ChiBurst.KnownSpell && ObjectManager.Me.HealthPercent <= MySettings.UseChiBurstAtPercentage && ChiBurst.IsSpellUsable)
        {
            ChiBurst.Cast();
            return;
        }
        if (MySettings.UseExpelHarm && ExpelHarm.KnownSpell && ObjectManager.Me.HealthPercent <= MySettings.UseExpelHarmAtPercentage && ExpelHarm.IsSpellUsable)
        {
            ExpelHarm.Cast();
            return;
        }
        if (MySettings.UseZenSphere && ZenSphere.KnownSpell && ObjectManager.Me.HealthPercent <= MySettings.UseZenSphereAtPercentage
            && !ZenSphere.HaveBuff && ZenSphere.IsSpellUsable)
        {
            ZenSphere.Cast();
        }
    }

    private void Decast()
    {
        if (MySettings.UseArcaneTorrentForDecast && ArcaneTorrent.KnownSpell && ObjectManager.Me.HealthPercent <= MySettings.UseArcaneTorrentForDecastAtPercentage
            && ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && ObjectManager.Target.GetDistance < 8 && ArcaneTorrent.IsSpellUsable)
        {
            ArcaneTorrent.Cast();
            return;
        }
        if (MySettings.UseDiffuseMagic && DiffuseMagic.KnownSpell && ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && DiffuseMagic.IsSpellUsable
            && ObjectManager.Me.HealthPercent <= MySettings.UseDiffuseMagicAtPercentage)
        {
            DiffuseMagic.Cast();
            return;
        }
        if (MySettings.UseSpearHandStrike && SpearHandStrike.KnownSpell && ObjectManager.Target.IsCast && SpearHandStrike.IsHostileDistanceGood && SpearHandStrike.IsSpellUsable
            && ObjectManager.Me.HealthPercent <= MySettings.UseSpearHandStrikeAtPercentage)
        {
            SpearHandStrike.Cast();
            return;
        }
        if (MySettings.UseDisable && Disable.KnownSpell && ObjectManager.Target.GetMove && !Disable.TargetHaveBuff && Disable.IsHostileDistanceGood && Disable.IsSpellUsable)
        {
            Disable.Cast();
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
        if (MySettings.UseBerserking && Berserking.KnownSpell && ObjectManager.Target.GetDistance < 30 && Berserking.IsSpellUsable)
        {
            Berserking.Cast();
            return;
        }
        if (MySettings.UseBloodFury && BloodFury.KnownSpell && ObjectManager.Target.GetDistance < 30 && BloodFury.IsSpellUsable)
        {
            BloodFury.Cast();
            return;
        }
        if (MySettings.UseChiBrew && ChiBrew.KnownSpell && ObjectManager.Me.Chi == 0 && ChiBrew.IsSpellUsable)
        {
            ChiBrew.Cast();
        }

        if (MySettings.UseTouchofDeath && TouchofDeath.KnownSpell && TouchofDeath.IsHostileDistanceGood && TouchofDeath.IsSpellUsable)
        {
            TouchofDeath.Cast();
        }

        if (MySettings.UseInvokeXuentheWhiteTiger && InvokeXuentheWhiteTiger.KnownSpell && InvokeXuentheWhiteTiger.IsHostileDistanceGood &&
            InvokeXuentheWhiteTiger.IsSpellUsable)
        {
            InvokeXuentheWhiteTiger.Cast();
        }
        if (MySettings.UseRushingJadeWind && RushingJadeWind.KnownSpell && RushingJadeWind.IsHostileDistanceGood && RushingJadeWind.IsSpellUsable
            && ObjectManager.GetNumberAttackPlayer() > 3)
        {
            RushingJadeWind.Cast();
        }
    }

    private void DPSCycle()
    {
        Usefuls.SleepGlobalCooldown();
        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            if (ObjectManager.GetNumberAttackPlayer() > 2)
            {
                if (MySettings.UseSpinningCraneKick && SpinningCraneKick.KnownSpell && ObjectManager.GetNumberAttackPlayer() > 5 && !ObjectManager.Me.IsCast
                    && ObjectManager.Target.GetDistance < 8 && SpinningCraneKick.IsSpellUsable)
                {
                    SpinningCraneKick.Cast();
                    return;
                }
                if (MySettings.UseDizzyingHaze && DizzyingHaze.KnownSpell && !DizzyingHaze.TargetHaveBuff && DizzyingHaze.IsHostileDistanceGood && DizzyingHaze.IsSpellUsable)
                {
                    SpellManager.CastSpellByIDAndPosition(115180, ObjectManager.Target.Position);
                    return;
                }
                if (MySettings.UseBreathofFire && BreathofFire.KnownSpell && !BreathofFire.TargetHaveBuff && ObjectManager.Target.GetDistance < 8 && BreathofFire.IsSpellUsable)
                {
                    BreathofFire.Cast();
                    return;
                }
                if (MySettings.UseRushingJadeWind && RushingJadeWind.KnownSpell && ObjectManager.Target.GetDistance < 30 && RushingJadeWind.IsSpellUsable)
                {
                    RushingJadeWind.Cast();
                    return;
                }
                return;
            }
            if (MySettings.UseRushingJadeWind && RushingJadeWind.KnownSpell && ObjectManager.Target.GetDistance < 30 && !ObjectManager.Target.HaveBuff(115307) &&
                RushingJadeWind.IsSpellUsable)
            {
                RushingJadeWind.Cast();
                return;
            }
            if (MySettings.UseBlackoutKick && BlackoutKick.KnownSpell && BlackoutKick.IsHostileDistanceGood && BlackoutKick.IsSpellUsable &&
                (!ObjectManager.Me.HaveBuff(121125) || !MySettings.UseTouchofDeath) && (!ObjectManager.Me.HaveBuff(115307) || !StanceoftheSturdyOx.KnownSpell))
            {
                BlackoutKick.Cast();
                return;
            }
            if (MySettings.UseTigerPalm && TigerPalm.KnownSpell && TigerPalm.IsHostileDistanceGood && TigerPalm.IsSpellUsable &&
                (!ObjectManager.Me.HaveBuff(121125) || !MySettings.UseTouchofDeath)
                &&
                (!ObjectManager.Me.HaveBuff(125359) ||
                 (!ObjectManager.Me.HaveBuff(118636) && Guard.IsSpellUsable && ObjectManager.Me.HealthPercent <= MySettings.UseGuardAtPercentage)))
            {
                TigerPalm.Cast();
                return;
            }
            if (MySettings.UseKegSmash && KegSmash.KnownSpell && KegSmash.IsHostileDistanceGood && ObjectManager.Me.Chi < 3 && KegSmash.IsSpellUsable)
            {
                KegSmash.Cast();
                return;
            }
            if (MySettings.UseArcaneTorrentForResource && ArcaneTorrent.KnownSpell && ObjectManager.Me.EnergyPercentage < 40 && ArcaneTorrent.IsSpellUsable
                && ObjectManager.Me.HealthPercent <= MySettings.UseExpelHarmAtPercentage)
            {
                ArcaneTorrent.Cast();
                return;
            }
            if (MySettings.UseExpelHarm && ExpelHarm.KnownSpell && ObjectManager.Me.HealthPercent <= MySettings.UseExpelHarmAtPercentage && ObjectManager.Me.Chi < 4
                && ExpelHarm.IsHostileDistanceGood && ExpelHarm.IsSpellUsable)
            {
                ExpelHarm.Cast();
                return;
            }
            if (MySettings.UseJab && Jab.KnownSpell && ObjectManager.Me.Chi < 4 && Jab.IsHostileDistanceGood && Jab.IsSpellUsable)
            {
                Jab.Cast();
                return;
            }
            if (MySettings.UseTigerPalm && TigerPalm.KnownSpell && TigerPalm.IsHostileDistanceGood && TigerPalm.IsSpellUsable && !ObjectManager.Me.HaveBuff(121125)
                && (ObjectManager.Me.HaveBuff(115307) || !StanceoftheSturdyOx.KnownSpell))
            {
                TigerPalm.Cast();
            }
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    private void Patrolling()
    {
        if (ObjectManager.Me.IsMounted) return;
        Buff();
        Heal();
    }

    #region Nested type: MonkBrewmasterSettings

    [Serializable]
    public class MonkBrewmasterSettings : Settings
    {
        public bool DoAvoidMelee = true;
        public bool UseAlchFlask = true;
        public bool UseArcaneTorrentForDecast = true;
        public int UseArcaneTorrentForDecastAtPercentage = 100;
        public bool UseArcaneTorrentForResource = true;
        public bool UseBerserking = true;
        public bool UseBlackoutKick = true;
        public bool UseBloodFury = true;
        public bool UseBreathofFire = true;
        public bool UseChargingOxWave = true;
        public int UseChargingOxWaveAtPercentage = 90;
        public bool UseChiBrew = true;
        public bool UseChiBurst = true;
        public int UseChiBurstAtPercentage = 90;
        public bool UseChiWave = true;
        public int UseChiWaveAtPercentage = 85;
        public bool UseClash = true;
        public bool UseCracklingJadeLightning = true;
        public bool UseDampenHarm = true;
        public int UseDampenHarmAtPercentage = 90;
        public bool UseDiffuseMagic = true;
        public int UseDiffuseMagicAtPercentage = 90;
        public bool UseDisable = false;
        public bool UseDizzyingHaze = true;
        public bool UseElusiveBrew = true;
        public int UseElusiveBrewAtPercentage = 70;

        public bool UseExpelHarm = true;
        public int UseExpelHarmAtPercentage = 90;
        public bool UseFortifyingBrew = true;
        public int UseFortifyingBrewAtPercentage = 80;
        public bool UseGiftoftheNaaru = true;
        public int UseGiftoftheNaaruAtPercentage = 80;
        public bool UseGrappleWeapon = true;
        public int UseGrappleWeaponAtPercentage = 95;
        public bool UseGuard = true;
        public int UseGuardAtPercentage = 95;
        public bool UseHealingSphere = true;
        public int UseHealingSphereAtPercentage = 70;
        public bool UseInvokeXuentheWhiteTiger = true;
        public bool UseJab = true;
        public bool UseKegSmash = true;
        public bool UseLegSweep = true;
        public int UseLegSweepAtPercentage = 90;
        public bool UseLegacyoftheEmperor = true;

        public bool UseProvoke = true;
        public bool UsePurifyingBrew = true;
        public int UsePurifyingBrewAtPercentage = 90;
        public bool UseRoll = true;
        public bool UseRushingJadeWind = true;
        public bool UseSpearHandStrike = true;
        public int UseSpearHandStrikeAtPercentage = 100;
        public bool UseSpinningCraneKick = true;
        public bool UseStanceoftheFierceTiger = true;
        public bool UseStanceoftheSturdyOx = true;
        public bool UseStoneform = true;
        public int UseStoneformAtPercentage = 80;
        public bool UseSummonBlackOxStatue = true;
        public bool UseTigerPalm = true;
        public bool UseTigersLust = true;
        public bool UseTouchofDeath = true;
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;
        public bool UseWarStomp = true;
        public int UseWarStompAtPercentage = 80;
        public bool UseZenMeditation = true;
        public bool UseZenSphere = true;
        public int UseZenSphereAtPercentage = 90;

        public MonkBrewmasterSettings()
        {
            ConfigWinForm("Brewmaster Monk Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent For Decast", "UseArcaneTorrentForDecast", "Professions & Racials", "AtPercentage");
            AddControlInWinForm("Use Arcane Torrent For Resource", "UseArcaneTorrentForResource", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials", "AtPercentage");

            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials", "AtPercentage");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials", "AtPercentage");
            /* Monk Buffs */
            AddControlInWinForm("Use Disable", "UseDisable", "Monk Buffs");
            AddControlInWinForm("Use Legacy of the Emperor", "UseLegacyoftheEmperor", "Monk Buffs");
            AddControlInWinForm("Use Stance of the Fierce Tiger", "UseStanceoftheFierceTiger", "Monk Buffs");
            AddControlInWinForm("Use Tiger's Lust", "UseTigersLust", "Monk Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Chi Wave", "UseChiWave", "Offensive Spell", "AtPercentage");
            AddControlInWinForm("Use Blackout Kick", "UseBlackoutKick", "Offensive Spell");
            AddControlInWinForm("Use Breath of Fire", "UseBreathofFire", "Offensive Spell");
            AddControlInWinForm("Use Clash", "UseClash", "Offensive Spell");
            AddControlInWinForm("Use Crackling Jade Lightning", "UseCracklingJadeLightning", "Offensive Spell");
            AddControlInWinForm("Use Dizzying Haze", "UseDizzyingHaze", "Offensive Spell");
            AddControlInWinForm("Use Jab", "UseJab", "Offensive Spell");
            AddControlInWinForm("Use Keg Smash", "UseKegSmash", "Offensive Spell");
            AddControlInWinForm("Use Provoke", "UseProvoke", "Offensive Spell");
            AddControlInWinForm("Use Roll", "UseRoll", "Offensive Spell");
            AddControlInWinForm("Use Spinning Crane Kick", "UseSpinningCraneKick", "Offensive Spell");
            AddControlInWinForm("Use Tiger Palm", "UseTigerPalm", "Offensive Spell");
            AddControlInWinForm("Use Touch of Death", "UseTouchofDeath", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use Chi Brew", "UseChiBrew", "Offensive Cooldown");
            AddControlInWinForm("Use Invoke Xuen, the White Tiger", "UseInvokeXuentheWhiteTiger", "Offensive Cooldown");
            AddControlInWinForm("Use Rushing Jade Wind", "UseRushingJadeWind", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Charging Ox Wave", "UseChargingOxWave", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Dampen Harm ", "UseDampenHarm ", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Diffuse Magic", "UseDiffuseMagic", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Elusive Brew", "UseElusiveBrew", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Fortifying Brew", "UseFortifyingBrew", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Grapple Weapon", "UseGrappleWeapon", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Guard", "UseGuard", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Leg Sweep", "UseLegSweep", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Purifying Brew", "UsePurifyingBrew", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Spear Hand Strike", "UseSpearHandStrike", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Summon Black Ox Statue", "UseSummonBlackOxStatue", "Defensive Cooldown");
            AddControlInWinForm("Use Zen Meditation", "UseZenMeditation", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Chi Burst", "UseChiBurst", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Expel Harm", "UseExpelHarm", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Healing Sphere", "UseHealingSphere", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Zen Sphere", "UseZenSphere", "Healing Spell", "AtPercentage");
            /* Game Settings */
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");

            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
            AddControlInWinForm("Do avoid melee (Off Advised!!)", "DoAvoidMelee", "Game Settings");
            AddControlInWinForm("Avoid melee distance (1 to 4)", "DoAvoidMeleeDistance", "Game Settings");
        }

        public static MonkBrewmasterSettings CurrentSetting { get; set; }

        public static MonkBrewmasterSettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Monk_Brewmaster.xml";
            if (File.Exists(currentSettingsFile))
            {
                return
                    CurrentSetting = Load<MonkBrewmasterSettings>(currentSettingsFile);
            }
            return new MonkBrewmasterSettings();
        }
    }

    #endregion
}

public class MonkWindwalker
{
    private static MonkWindwalkerSettings MySettings = MonkWindwalkerSettings.GetSettings();

    #region General Timers & Variables

    private readonly WoWItem _firstTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET);
    private readonly WoWItem _secondTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET, 2);

    private Timer _grappleWeaponTimer = new Timer(0);
    private Timer _healingSphereTimer = new Timer(0);
    private Timer _onCd = new Timer(0);
    private Timer _risingSunKickTimer = new Timer(0);
    private Timer _tigerPowerTimer = new Timer(0);
    private Timer _rollTimer = new Timer(0);

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

    #region Monk Buffs

    public readonly Spell Disable = new Spell("Disable");
    public readonly Spell LegacyoftheEmperor = new Spell("Legacy of the Emperor");
    public readonly Spell LegacyoftheWhiteTiger = new Spell("Legacy of the White Tiger");
    public readonly Spell StanceoftheFierceTiger = new Spell("Stance of the Fierce Tiger");
    public readonly Spell TigereyeBrew = new Spell("Tigereye Brew");
    public readonly Spell TigersLust = new Spell("Tiger's Lust");

    #endregion

    #region Offensive Spell

    public readonly Spell BlackoutKick = new Spell("Blackout Kick");
    public readonly Spell CracklingJadeLightning = new Spell("Crackling Jade Lightning");
    public readonly Spell FistsofFury = new Spell("Fists of Fury");
    public readonly Spell Jab = new Spell("Jab");
    public readonly Spell Provoke = new Spell("Provoke");
    public readonly Spell RisingSunKick = new Spell("Rising Sun Kick");
    public readonly Spell Roll = new Spell("Roll");
    public readonly Spell SpinningCraneKick = new Spell("Spinning Crane Kick");
    public readonly Spell TigerPalm = new Spell("Tiger Palm");
    public readonly Spell TouchofDeath = new Spell("Touch of Death");

    #endregion

    #region Offensive Cooldown

    public readonly Spell ChiBrew = new Spell("Chi Brew");
    public readonly Spell EnergizingBrew = new Spell("Energizing Brew");
    public readonly Spell InvokeXuentheWhiteTiger = new Spell("Invoke Xuen, the White Tiger");
    public readonly Spell RushingJadeWind = new Spell("Rushing Jade Wind");

    #endregion

    #region Defensive Cooldown

    public readonly Spell ChargingOxWave = new Spell("Charging Ox Wave");
    public readonly Spell DampenHarm = new Spell("Dampen Harm");
    public readonly Spell DiffuseMagic = new Spell("Diffuse Magic");
    public readonly Spell FortifyingBrew = new Spell("Fortifying Brew");
    public readonly Spell GrappleWeapon = new Spell("Grapple Weapon");
    public readonly Spell LegSweep = new Spell("Leg Sweep");
    public readonly Spell SpearHandStrike = new Spell("Spear Hand Strike");
    public readonly Spell TouchofKarma = new Spell("Touch of Karma");
    public readonly Spell ZenMeditation = new Spell("Zen Meditation");

    #endregion

    #region Healing Spell

    public readonly Spell ChiBurst = new Spell("Chi Burst");
    public readonly Spell ChiWave = new Spell("Chi Wave");
    public readonly Spell ExpelHarm = new Spell("Expel Harm");
    public readonly Spell HealingSphere = new Spell("Healing Sphere");
    public readonly Spell ZenSphere = new Spell("Zen Sphere");

    #endregion

    public MonkWindwalker()
    {
        Main.InternalRange = ObjectManager.Me.GetCombatReach;
        MySettings = MonkWindwalkerSettings.GetSettings();
        Main.DumpCurrentSettings<MonkWindwalkerSettings>(MySettings);
        UInt128 lastTarget = 0;

        while (Main.InternalLoop)
        {
            try
            {
                if (!ObjectManager.Me.IsDead)
                {
                    if (!ObjectManager.Me.IsMounted)
                    {
                        if (Fight.InFight && ObjectManager.Me.Target > 0)
                        {
                            if (ObjectManager.Me.Target != lastTarget
                                && (Jab.IsHostileDistanceGood || Provoke.IsHostileDistanceGood))
                            {
                                Pull();
                                lastTarget = ObjectManager.Me.Target;
                            }

                            if (ObjectManager.Target.GetDistance < 30)
                                Combat();
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
        if (MySettings.UseProvoke && Provoke.KnownSpell && !ObjectManager.Target.InCombat && Provoke.IsHostileDistanceGood && Provoke.IsSpellUsable)
        {
            Provoke.Cast();
        }
    }

    private void Combat()
    {
        Buff();
        if (MySettings.DoAvoidMelee)
            AvoidMelee();
        DPSCycle();
        if (_onCd.IsReady &&
            (ObjectManager.Me.HealthPercent <= MySettings.UseGrappleWeaponAtPercentage || ObjectManager.Me.HealthPercent <= MySettings.UseFortifyingBrewAtPercentage
             || ObjectManager.Me.HealthPercent <= MySettings.UseChargingOxWaveAtPercentage ||
             ObjectManager.Me.HealthPercent <= MySettings.UseTouchofKarmaAtPercentage
             || ObjectManager.Me.HealthPercent <= MySettings.UseDampenHarmAtPercentage || ObjectManager.Me.HealthPercent <= MySettings.UseLegSweepAtPercentage
             || ObjectManager.Me.HealthPercent <= MySettings.UseStoneformAtPercentage || ObjectManager.Me.HealthPercent <= MySettings.UseWarStompAtPercentage))
            DefenseCycle();
        Heal();
        Decast();
        DPSBurst();
        DPSCycle();
    }

    private void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (MySettings.UseLegacyoftheEmperor && LegacyoftheEmperor.KnownSpell && !LegacyoftheEmperor.HaveBuff && LegacyoftheEmperor.IsSpellUsable)
            LegacyoftheEmperor.Cast();

        if (MySettings.UseLegacyoftheWhiteTiger && LegacyoftheWhiteTiger.KnownSpell && !LegacyoftheWhiteTiger.HaveBuff && LegacyoftheWhiteTiger.IsSpellUsable)
            LegacyoftheWhiteTiger.Cast();

        if (MySettings.UseStanceoftheFierceTiger && StanceoftheFierceTiger.KnownSpell && !StanceoftheFierceTiger.HaveBuff && StanceoftheFierceTiger.IsSpellUsable)
            StanceoftheFierceTiger.Cast();

        if (MySettings.UseTigersLust && TigersLust.KnownSpell && !ObjectManager.Me.InCombat && ObjectManager.Me.GetMove && TigersLust.IsSpellUsable)
            TigersLust.Cast();

        if (MySettings.UseRoll && !ObjectManager.Me.InCombat && Roll.KnownSpell && ObjectManager.Me.GetMove
            && !TigersLust.HaveBuff && _rollTimer.IsReady && Roll.IsSpellUsable && ObjectManager.Target.GetDistance > 14)
        {
            Roll.Cast();
            _rollTimer = new Timer(1000*15);
            return;
        }

        if (MySettings.UseAlchFlask && !ObjectManager.Me.HaveBuff(79638) && !ObjectManager.Me.HaveBuff(79640) && !ObjectManager.Me.HaveBuff(79639)
            && !ItemsManager.IsItemOnCooldown(75525) && ItemsManager.GetItemCount(75525) > 0)
            ItemsManager.UseItem(75525);
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
        if (MySettings.UseGrappleWeapon && GrappleWeapon.KnownSpell && GrappleWeapon.IsHostileDistanceGood &&
            ObjectManager.Me.HealthPercent <= MySettings.UseGrappleWeaponAtPercentage
            && _grappleWeaponTimer.IsReady && GrappleWeapon.IsSpellUsable)
        {
            GrappleWeapon.Cast();
            _grappleWeaponTimer = new Timer(1000*60);
            return;
        }
        if (MySettings.UseFortifyingBrew && FortifyingBrew.KnownSpell && ObjectManager.Me.HealthPercent <= MySettings.UseFortifyingBrewAtPercentage &&
            FortifyingBrew.IsSpellUsable)
        {
            FortifyingBrew.Cast();
            _onCd = new Timer(1000*20);
            return;
        }
        if (MySettings.UseChargingOxWave && ChargingOxWave.KnownSpell && ChargingOxWave.IsHostileDistanceGood &&
            ObjectManager.Me.HealthPercent <= MySettings.UseChargingOxWaveAtPercentage
            && ChargingOxWave.IsSpellUsable)
        {
            ChargingOxWave.Cast();
            _onCd = new Timer(1000*3);
            return;
        }
        if (MySettings.UseDampenHarm && DampenHarm.KnownSpell && DampenHarm.IsSpellUsable && ObjectManager.Me.HealthPercent <= MySettings.UseDampenHarmAtPercentage)
        {
            DampenHarm.Cast();
            _onCd = new Timer(1000*5);
            return;
        }
        if (MySettings.UseLegSweep && LegSweep.KnownSpell && ObjectManager.Target.GetDistance < 6 &&
            ObjectManager.Me.HealthPercent <= MySettings.UseLegSweepAtPercentage
            && LegSweep.IsSpellUsable)
        {
            LegSweep.Cast();
            _onCd = new Timer(1000*5);
            return;
        }
        if (MySettings.UseTouchofKarma && TouchofKarma.KnownSpell && ObjectManager.Me.HealthPercent <= MySettings.UseTouchofKarmaAtPercentage
            && TouchofKarma.IsHostileDistanceGood && TouchofKarma.IsSpellUsable)
        {
            TouchofKarma.Cast();
            _onCd = new Timer(1000*6);
            return;
        }
        if (MySettings.UseStoneform && Stoneform.KnownSpell && Stoneform.IsSpellUsable && ObjectManager.Me.HealthPercent <= MySettings.UseStoneformAtPercentage)
        {
            Stoneform.Cast();
            _onCd = new Timer(1000*8);
            return;
        }
        if (MySettings.UseWarStomp && WarStomp.KnownSpell && ObjectManager.Target.GetDistance < 8 && WarStomp.IsSpellUsable &&
            ObjectManager.Me.HealthPercent <= MySettings.UseWarStompAtPercentage)
        {
            WarStomp.Cast();
            _onCd = new Timer(1000*2);
        }
    }

    private void Heal()
    {
        if (MySettings.UseGiftoftheNaaru && GiftoftheNaaru.KnownSpell && GiftoftheNaaru.IsSpellUsable
            && ObjectManager.Me.HealthPercent <= MySettings.UseGiftoftheNaaruAtPercentage)
        {
            GiftoftheNaaru.Cast();
            return;
        }
        if (MySettings.UseHealingSphere && HealingSphere.KnownSpell && HealingSphere.IsSpellUsable && _healingSphereTimer.IsReady &&
            ObjectManager.Me.HealthPercent <= MySettings.UseHealingSphereAtPercentage)
        {
            SpellManager.CastSpellByIDAndPosition(115460, ObjectManager.Me.Position);
            _healingSphereTimer = new Timer(1000*8);
            return;
        }
        if (MySettings.UseChiWave && ChiWave.KnownSpell && ObjectManager.Me.HealthPercent <= MySettings.UseChiWaveAtPercentage && ChiWave.IsSpellUsable)
        {
            ChiWave.Cast();
            return;
        }
        if (MySettings.UseChiBurst && ChiBurst.KnownSpell && ObjectManager.Me.HealthPercent <= MySettings.UseChiBurstAtPercentage && ChiBurst.IsSpellUsable)
        {
            ChiBurst.Cast();
            return;
        }
        if (MySettings.UseExpelHarm && ExpelHarm.KnownSpell && ObjectManager.Me.HealthPercent <= MySettings.UseExpelHarmAtPercentage && ExpelHarm.IsSpellUsable)
        {
            ExpelHarm.Cast();
            return;
        }
        if (MySettings.UseZenSphere && ZenSphere.KnownSpell && ObjectManager.Me.HealthPercent <= MySettings.UseZenSphereAtPercentage
            && !ZenSphere.HaveBuff && ZenSphere.IsSpellUsable)
        {
            ZenSphere.Cast();
        }
    }

    private void Decast()
    {
        if (MySettings.UseArcaneTorrentForDecast && ArcaneTorrent.KnownSpell && ObjectManager.Me.HealthPercent <= MySettings.UseArcaneTorrentForDecastAtPercentage
            && ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && ObjectManager.Target.GetDistance < 8 && ArcaneTorrent.IsSpellUsable)
        {
            ArcaneTorrent.Cast();
            return;
        }
        if (MySettings.UseDiffuseMagic && DiffuseMagic.KnownSpell && ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && DiffuseMagic.IsSpellUsable
            && ObjectManager.Me.HealthPercent <= MySettings.UseDiffuseMagicAtPercentage)
        {
            DiffuseMagic.Cast();
            return;
        }
        if (MySettings.UseSpearHandStrike && SpearHandStrike.KnownSpell && ObjectManager.Target.IsCast && SpearHandStrike.IsHostileDistanceGood && SpearHandStrike.IsSpellUsable
            && ObjectManager.Me.HealthPercent <= MySettings.UseSpearHandStrikeAtPercentage)
        {
            SpearHandStrike.Cast();
            return;
        }
        if (MySettings.UseDisable && Disable.KnownSpell && ObjectManager.Target.GetMove && !Disable.TargetHaveBuff && Disable.IsHostileDistanceGood && Disable.IsSpellUsable)
        {
            Disable.Cast();
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
        if (MySettings.UseBerserking && Berserking.KnownSpell && ObjectManager.Target.GetDistance < 30 && Berserking.IsSpellUsable)
        {
            Berserking.Cast();
            return;
        }
        if (MySettings.UseBloodFury && BloodFury.KnownSpell && ObjectManager.Target.GetDistance < 30 && BloodFury.IsSpellUsable)
        {
            BloodFury.Cast();
            return;
        }
        if (MySettings.UseChiBrew && ChiBrew.KnownSpell && ObjectManager.Me.Chi == 0 && ChiBrew.IsSpellUsable)
        {
            ChiBrew.Cast();
        }
        if (MySettings.UseTouchofDeath && TouchofDeath.KnownSpell && TouchofDeath.IsHostileDistanceGood && TouchofDeath.IsSpellUsable)
        {
            TouchofDeath.Cast();
        }
        if (MySettings.UseInvokeXuentheWhiteTiger && InvokeXuentheWhiteTiger.KnownSpell && InvokeXuentheWhiteTiger.IsHostileDistanceGood &&
            InvokeXuentheWhiteTiger.IsSpellUsable)
        {
            InvokeXuentheWhiteTiger.Cast();
        }
        if (MySettings.UseEnergizingBrew && EnergizingBrew.KnownSpell && ObjectManager.Me.Energy <= 40f && ObjectManager.Target.GetDistance < 30 && EnergizingBrew.IsSpellUsable)
        {
            EnergizingBrew.Cast();
        }
        if (MySettings.UseTigereyeBrew && TigereyeBrew.KnownSpell && ObjectManager.Me.BuffStack(125195) > 9 && ObjectManager.Target.GetDistance < 30 &&
            TigereyeBrew.IsSpellUsable)
        {
            TigereyeBrew.Cast();
        }
        if (MySettings.UseChiWave && ChiWave.KnownSpell && ChiWave.IsHostileDistanceGood && ChiWave.IsSpellUsable)
        {
            ChiWave.Cast();
        }
        if (MySettings.UseRushingJadeWind && RushingJadeWind.KnownSpell && RushingJadeWind.IsHostileDistanceGood && RushingJadeWind.IsSpellUsable
            && ObjectManager.GetNumberAttackPlayer() > 3)
        {
            RushingJadeWind.Cast();
        }
    }

    private void DPSCycle()
    {
        Usefuls.SleepGlobalCooldown();
        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            if (ObjectManager.GetNumberAttackPlayer() > 3)
            {
                if (MySettings.UseTigerPalm && TigerPalm.KnownSpell && !ObjectManager.Me.HaveBuff(125359) && TigerPalm.IsHostileDistanceGood && TigerPalm.IsSpellUsable
                    && ObjectManager.Me.Chi > 0 && (_tigerPowerTimer.IsReady || !ObjectManager.Me.HaveBuff(125359)))
                {
                    TigerPalm.Cast();
                    _tigerPowerTimer = new Timer(1000*16);
                    return;
                }
                if (MySettings.UseRisingSunKick && RisingSunKick.KnownSpell && !ObjectManager.Target.HaveBuff(130320) && RisingSunKick.IsHostileDistanceGood && RisingSunKick.IsSpellUsable
                    && ObjectManager.Me.Chi > 1 && (_risingSunKickTimer.IsReady || !ObjectManager.Target.HaveBuff(130320)))
                {
                    RisingSunKick.Cast();
                    _risingSunKickTimer = new Timer(1000*6);
                    return;
                }
                if (MySettings.UseSpinningCraneKick && SpinningCraneKick.KnownSpell && SpinningCraneKick.IsHostileDistanceGood && !ObjectManager.Me.IsCast &&
                    SpinningCraneKick.IsSpellUsable)
                {
                    SpinningCraneKick.Cast();
                    return;
                }
            }

            if (MySettings.UseExpelHarm && ExpelHarm.KnownSpell && ObjectManager.Me.HealthPercent <= MySettings.UseExpelHarmAtPercentage && ExpelHarm.IsHostileDistanceGood &&
                ExpelHarm.IsSpellUsable
                && ObjectManager.Me.Chi < 4)
            {
                ExpelHarm.Cast();
                return;
            }
            if (MySettings.UseJab && Jab.KnownSpell && !ObjectManager.Me.HaveBuff(116768) && Jab.IsHostileDistanceGood && Jab.IsSpellUsable
                && ObjectManager.Me.Chi < 4 && !ObjectManager.Me.HaveBuff(118864))
            {
                Jab.Cast();
                return;
            }
            if (MySettings.UseTigerPalm && TigerPalm.KnownSpell && TigerPalm.IsHostileDistanceGood && TigerPalm.IsSpellUsable &&
                (!ObjectManager.Me.HaveBuff(121125) || !MySettings.UseTouchofDeath) && ObjectManager.Me.Chi > 0
                && (_tigerPowerTimer.IsReady || !ObjectManager.Me.HaveBuff(125359) || ObjectManager.Me.HaveBuff(118864)))
            {
                TigerPalm.Cast();
                _tigerPowerTimer = new Timer(1000*16);
                return;
            }
            if (MySettings.UseRisingSunKick && RisingSunKick.KnownSpell && RisingSunKick.IsHostileDistanceGood && RisingSunKick.IsSpellUsable
                && (!ObjectManager.Me.HaveBuff(121125) || !MySettings.UseTouchofDeath) && ObjectManager.Me.Chi > 1
                && (_risingSunKickTimer.IsReady || !ObjectManager.Target.HaveBuff(130320)))
            {
                RisingSunKick.Cast();
                _risingSunKickTimer = new Timer(1000*6);
                return;
            }
            if (MySettings.UseFistsofFury && FistsofFury.KnownSpell && FistsofFury.IsHostileDistanceGood && FistsofFury.IsSpellUsable &&
                (!ObjectManager.Me.HaveBuff(121125) || !MySettings.UseTouchofDeath) && ObjectManager.Me.Chi > 2
                && !_tigerPowerTimer.IsReady && !_risingSunKickTimer.IsReady && ObjectManager.Me.EnergyPercentage < 81)
            {
                FistsofFury.Cast();
                return;
            }
            if (MySettings.UseBlackoutKick && BlackoutKick.KnownSpell && BlackoutKick.IsHostileDistanceGood && BlackoutKick.IsSpellUsable
                && (!ObjectManager.Me.HaveBuff(121125) || !MySettings.UseTouchofDeath) && ObjectManager.Me.Chi > 1)
            {
                BlackoutKick.Cast();
                return;
            }
            if (MySettings.UseArcaneTorrentForResource && ArcaneTorrent.KnownSpell && ArcaneTorrent.IsSpellUsable && ObjectManager.Me.EnergyPercentage < 90)
            {
                ArcaneTorrent.Cast();
            }
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    private void Patrolling()
    {
        if (ObjectManager.Me.IsMounted) return;
        Buff();
        Heal();
    }

    #region Nested type: MonkWindwalkerSettings

    [Serializable]
    public class MonkWindwalkerSettings : Settings
    {
        public bool DoAvoidMelee = false;
        public int DoAvoidMeleeDistance = 0;
        public bool UseAlchFlask = true;
        public bool UseArcaneTorrentForDecast = true;
        public int UseArcaneTorrentForDecastAtPercentage = 100;
        public bool UseArcaneTorrentForResource = true;
        public bool UseBerserking = true;
        public bool UseBlackoutKick = true;
        public bool UseBloodFury = true;
        public bool UseChargingOxWave = true;
        public int UseChargingOxWaveAtPercentage = 90;
        public bool UseChiBrew = true;
        public bool UseChiBurst = true;
        public int UseChiBurstAtPercentage = 90;
        public bool UseChiWave = true;
        public int UseChiWaveAtPercentage = 85;
        public bool UseDampenHarm = true;
        public int UseDampenHarmAtPercentage = 90;
        public bool UseDiffuseMagic = true;
        public int UseDiffuseMagicAtPercentage = 90;
        public bool UseDisable = false;
        public bool UseEnergizingBrew = true;

        public bool UseExpelHarm = true;
        public int UseExpelHarmAtPercentage = 90;
        public bool UseFistsofFury = true;
        public bool UseFortifyingBrew = true;
        public int UseFortifyingBrewAtPercentage = 80;
        public bool UseGiftoftheNaaru = true;
        public int UseGiftoftheNaaruAtPercentage = 80;
        public bool UseGrappleWeapon = true;
        public int UseGrappleWeaponAtPercentage = 95;
        public bool UseHealingSphere = true;
        public int UseHealingSphereAtPercentage = 70;
        public bool UseInvokeXuentheWhiteTiger = true;
        public bool UseJab = true;
        public bool UseLegSweep = true;
        public int UseLegSweepAtPercentage = 90;
        public bool UseLegacyoftheEmperor = true;
        public bool UseLegacyoftheWhiteTiger = true;

        public bool UseProvoke = true;
        public bool UseRisingSunKick = true;
        public bool UseRoll = true;
        public bool UseRushingJadeWind = true;
        public bool UseSpearHandStrike = true;
        public int UseSpearHandStrikeAtPercentage = 100;
        public bool UseSpinningCraneKick = true;
        public bool UseStanceoftheFierceTiger = true;
        public bool UseStoneform = true;
        public int UseStoneformAtPercentage = 80;
        public bool UseTigerPalm = true;
        public bool UseTigereyeBrew = true;
        public bool UseTigersLust = true;
        public bool UseTouchofDeath = true;
        public bool UseTouchofKarma = true;
        public int UseTouchofKarmaAtPercentage = 95;
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;
        public bool UseWarStomp = true;
        public int UseWarStompAtPercentage = 80;
        public bool UseZenMeditation = true;
        public bool UseZenSphere = true;
        public int UseZenSphereAtPercentage = 90;

        public MonkWindwalkerSettings()
        {
            ConfigWinForm("Windwalker Monk Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent For Decast", "UseArcaneTorrentForDecast", "Professions & Racials", "AtPercentage");
            AddControlInWinForm("Use Arcane Torrent For Resource", "UseArcaneTorrentForResource", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials", "AtPercentage");

            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials", "AtPercentage");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials", "AtPercentage");
            /* Monk Buffs */
            AddControlInWinForm("Use Disable", "UseDisable", "Monk Buffs");
            AddControlInWinForm("Use Legacy of the Emperor", "UseLegacyoftheEmperor", "Monk Buffs");
            AddControlInWinForm("Use Legacy of the White Tiger", "UseLegacyoftheWhiteTiger", "Monk Buffs");
            AddControlInWinForm("Use Stance of the Fierce Tiger", "UseStanceoftheFierceTiger", "Monk Buffs");
            AddControlInWinForm("Use Tigereye Brew", "UseTigereBrew", "Monk Buffs");
            AddControlInWinForm("Use Tiger's Lust", "UseTigersLust", "Monk Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Blackout Kick", "UseBlackoutKick", "Offensive Spell");
            AddControlInWinForm("Use Fists of Fury", "UseFistsofFury", "Offensive Spell");
            AddControlInWinForm("Use Jab", "UseJab", "Offensive Spell");
            AddControlInWinForm("Use Path of Blossoms", "UsePathofBlossoms", "Offensive Spell");
            AddControlInWinForm("Use Provoke", "UseProvoke", "Offensive Spell");
            AddControlInWinForm("Use Rising Sun Kick", "UseRisingSunKick", "Offensive Spell");
            AddControlInWinForm("Use Roll", "UseRoll", "Offensive Spell");
            AddControlInWinForm("Use Spinning Crane Kick", "UseSpinningCraneKick", "Offensive Spell");
            AddControlInWinForm("Use Tiger Palm", "UseTigerPalm", "Offensive Spell");
            AddControlInWinForm("Use Touch of Death", "UseTouchofDeath", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use Chi Brew", "UseChiBrew", "Offensive Cooldown");
            AddControlInWinForm("Use Energizing Brew", "UseEnergizingBrew", "Offensive Cooldown");
            AddControlInWinForm("Use Invoke Xuen, the White Tiger", "UseInvokeXuentheWhiteTiger", "Offensive Cooldown");
            AddControlInWinForm("Use Rushing Jade Wind", "UseRushingJadeWind", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Charging Ox Wave", "UseChargingOxWave", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Dampen Harm ", "UseDampenHarm ", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Diffuse Magic", "UseDiffuseMagic", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Fortifying Brew", "UseFortifyingBrew", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Grapple Weapon", "UseGrappleWeapon", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Leg Sweep", "UseLegSweep", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Spear Hand Strike", "UseSpearHandStrike", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Touch of Karma", "UseTouchofKarma", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Zen Meditation", "UseZenMeditation", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Chi Burst", "UseChiBurst", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Chi Wave", "UseChiWave", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Expel Harm", "UseExpelHarm", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Healing Sphere", "UseHealingSphere", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Zen Sphere", "UseZenSphere", "Healing Spell", "AtPercentage");
            /* Game Settings */
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");

            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
            AddControlInWinForm("Do avoid melee (Off Advised!!)", "DoAvoidMelee", "Game Settings");
            AddControlInWinForm("Avoid melee distance (1 to 4)", "DoAvoidMeleeDistance", "Game Settings");
        }

        public static MonkWindwalkerSettings CurrentSetting { get; set; }

        public static MonkWindwalkerSettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Monk_Windwalker.xml";
            if (File.Exists(currentSettingsFile))
            {
                return
                    CurrentSetting = Load<MonkWindwalkerSettings>(currentSettingsFile);
            }
            return new MonkWindwalkerSettings();
        }
    }

    #endregion
}

public class MonkMistweaver
{
    private static MonkMistweaverSettings MySettings = MonkMistweaverSettings.GetSettings();

    #region General Timers & Variables

    private readonly WoWItem _firstTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET);
    private readonly WoWItem _secondTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET, 2);

    private Timer _grappleWeaponTimer = new Timer(0);
    private Timer _healingSphereTimer = new Timer(0);
    private Timer _onCd = new Timer(0);
    private Timer _serpentsZealTimer = new Timer(0);

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

    #region Monk Buffs

    public readonly Spell Disable = new Spell("Disable");
    public readonly Spell LegacyoftheEmperor = new Spell("Legacy of the Emperor");
    public readonly Spell StanceoftheFierceTiger = new Spell("Stance of the Fierce Tiger");
    public readonly Spell StanceoftheWiseSerpent = new Spell("Stance of the Wise Serpent");
    public readonly Spell SummonJadeSerpentStatue = new Spell("Summon Jade Serpent Statue");
    public readonly Spell TigersLust = new Spell("Tiger's Lust");

    #endregion

    #region Offensive Spell

    public readonly Spell BlackoutKick = new Spell("Blackout Kick");
    public readonly Spell CracklingJadeLightning = new Spell("Crackling Jade Lightning");
    public readonly Spell Jab = new Spell("Jab");
    public readonly Spell PathofBlossoms = new Spell("Path of Blossoms");
    public readonly Spell Provoke = new Spell("Provoke");
    public readonly Spell Roll = new Spell("Roll");
    public readonly Spell SpinningCraneKick = new Spell("Spinning Crane Kick");
    public readonly Spell TigerPalm = new Spell("Tiger Palm");
    public readonly Spell TouchofDeath = new Spell("Touch of Death");

    #endregion

    #region Healing Cooldown

    public readonly Spell ChiBrew = new Spell("Chi Brew");
    public readonly Spell InvokeXuentheWhiteTiger = new Spell("Invoke Xuen, the White Tiger");
    public readonly Spell RushingJadeWind = new Spell("Rushing Jade Wind");
    public readonly Spell ThunderFocusTea = new Spell("Thunder Focus Tea");

    #endregion

    #region Defensive Cooldown

    public readonly Spell ChargingOxWave = new Spell("Charging Ox Wave");
    public readonly Spell DampenHarm = new Spell("Dampen Harm");
    public readonly Spell DiffuseMagic = new Spell("Diffuse Magic");
    public readonly Spell FortifyingBrew = new Spell("Fortifying Brew");
    public readonly Spell GrappleWeapon = new Spell("Grapple Weapon");
    public readonly Spell LegSweep = new Spell("Leg Sweep");
    public readonly Spell LifeCocoon = new Spell("Life Cocoon");
    public readonly Spell SpearHandStrike = new Spell("Spear Hand Strike");
    public readonly Spell ZenMeditation = new Spell("Zen Meditation");

    #endregion

    #region Healing Spell

    public readonly Spell ChiBurst = new Spell("Chi Burst");
    public readonly Spell ChiWave = new Spell("Chi Wave");
    public readonly Spell EnvelopingMist = new Spell("Enveloping Mist");
    public readonly Spell ExpelHarm = new Spell("Expel Harm");
    public readonly Spell HealingSphere = new Spell("Healing Sphere");
    public readonly Spell ManaTea = new Spell("Mana Tea");
    public readonly Spell RenewingMist = new Spell("Renewing Mist");
    public readonly Spell Revival = new Spell("Revival");
    public readonly Spell SoothingMist = new Spell("Soothing Mist");
    public readonly Spell SurgingMist = new Spell("Surging Mist");
    public readonly Spell Uplift = new Spell("Uplift");
    public readonly Spell ZenSphere = new Spell("Zen Sphere");

    #endregion

    public MonkMistweaver()
    {
        Main.InternalRange = 30.0f;
        MySettings = MonkMistweaverSettings.GetSettings();
        Main.DumpCurrentSettings<MonkMistweaverSettings>(MySettings);
        UInt128 lastTarget = 0;

        while (Main.InternalLoop)
        {
            try
            {
                if (!ObjectManager.Me.IsDead)
                {
                    if (!ObjectManager.Me.IsMounted)
                    {
                        if (Fight.InFight && ObjectManager.Me.Target > 0)
                        {
                            if (ObjectManager.Me.Target != lastTarget
                                && (Jab.IsHostileDistanceGood || Provoke.IsHostileDistanceGood))
                            {
                                Pull();
                                lastTarget = ObjectManager.Me.Target;
                            }

                            if (ObjectManager.Target.GetDistance <= 40f)
                                Combat();
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
        if (!ObjectManager.Target.InCombat && Provoke.IsSpellUsable && Provoke.IsHostileDistanceGood
            && MySettings.UseProvoke && Provoke.KnownSpell)
        {
            Provoke.Cast();
        }
    }

    private void Combat()
    {
        Buff();
        if (MySettings.DoAvoidMelee)
            AvoidMelee();
        if (_onCd.IsReady)
            DefenseCycle();
        DPSCycle();
        Heal();
        Decast();
        DPSCycle();
        HealingBurst();
        DPSCycle();
    }

    private void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (LegacyoftheEmperor.KnownSpell && LegacyoftheEmperor.IsSpellUsable &&
            !LegacyoftheEmperor.HaveBuff && MySettings.UseLegacyoftheEmperor)
        {
            LegacyoftheEmperor.Cast();
            return;
        }
        if (StanceoftheWiseSerpent.KnownSpell && StanceoftheWiseSerpent.IsSpellUsable && !StanceoftheWiseSerpent.HaveBuff
            && MySettings.UseStanceoftheWiseSerpent)
        {
            StanceoftheWiseSerpent.Cast();
            return;
        }
        if (!ObjectManager.Me.InCombat && TigersLust.IsSpellUsable && TigersLust.KnownSpell
            && MySettings.UseTigersLust && ObjectManager.Me.GetMove)
        {
            TigersLust.Cast();
            return;
        }
        if (!ObjectManager.Me.InCombat && Roll.IsSpellUsable && Roll.KnownSpell
            && MySettings.UseRoll && ObjectManager.Me.GetMove && !TigersLust.HaveBuff
            && ObjectManager.Target.GetDistance > 14)
        {
            Roll.Cast();
            return;
        }
        if (ObjectManager.Me.InCombat && SummonJadeSerpentStatue.IsSpellUsable && SummonJadeSerpentStatue.KnownSpell
            && MySettings.UseSummonJadeSerpentStatue && !SummonJadeSerpentStatue.HaveBuff && ObjectManager.Target.GetDistance <= 40f)
        {
            SummonJadeSerpentStatue.Cast();
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
        if (ObjectManager.Me.HealthPercent < 95 && MySettings.UseGrappleWeapon && GrappleWeapon.IsHostileDistanceGood
            && GrappleWeapon.KnownSpell && GrappleWeapon.IsSpellUsable && _grappleWeaponTimer.IsReady)
        {
            GrappleWeapon.Cast();
            _grappleWeaponTimer = new Timer(1000*60);
            return;
        }
        if (ObjectManager.Me.HealthPercent < 80 && FortifyingBrew.IsSpellUsable && FortifyingBrew.KnownSpell
            && MySettings.UseFortifyingBrew)
        {
            FortifyingBrew.Cast();
            _onCd = new Timer(1000*20);
            return;
        }
        if (ObjectManager.Me.HealthPercent < 80 && LifeCocoon.IsSpellUsable && LifeCocoon.KnownSpell
            && MySettings.UseLifeCocoon)
        {
            LifeCocoon.Cast();
            _onCd = new Timer(1000*12);
            return;
        }
        if (ObjectManager.Me.HealthPercent < 90 && ChargingOxWave.IsSpellUsable && ChargingOxWave.KnownSpell
            && MySettings.UseChargingOxWave && ChargingOxWave.IsHostileDistanceGood)
        {
            ChargingOxWave.Cast();
            _onCd = new Timer(1000*3);
            return;
        }
        if (ObjectManager.Me.HealthPercent < 90 && DampenHarm.IsSpellUsable && DampenHarm.KnownSpell
            && MySettings.UseDampenHarm)
        {
            DampenHarm.Cast();
            _onCd = new Timer(1000*5);
            return;
        }
        if (ObjectManager.Me.HealthPercent < 90 && LegSweep.IsSpellUsable && LegSweep.KnownSpell
            && MySettings.UseLegSweep && ObjectManager.Target.GetDistance < 6)
        {
            LegSweep.Cast();
            _onCd = new Timer(1000*5);
            return;
        }
        if (ObjectManager.Me.HealthPercent < 80 && ZenMeditation.IsSpellUsable && ZenMeditation.KnownSpell
            && MySettings.UseZenMeditation)
        {
            ZenMeditation.Cast();
            _onCd = new Timer(1000*8);
            return;
        }
        if (ObjectManager.Me.HealthPercent < 80 && Stoneform.IsSpellUsable && Stoneform.KnownSpell
            && MySettings.UseStoneform)
        {
            Stoneform.Cast();
            _onCd = new Timer(1000*8);
            return;
        }
        if (ObjectManager.Me.HealthPercent < 80 && WarStomp.IsSpellUsable && WarStomp.KnownSpell
            && MySettings.UseWarStomp)
        {
            WarStomp.Cast();
            _onCd = new Timer(1000*2);
        }
    }

    private void Heal()
    {
        if (MySettings.UseArcaneTorrentForResource && ArcaneTorrent.KnownSpell && ArcaneTorrent.IsSpellUsable
            && ObjectManager.Me.ManaPercentage <= MySettings.UseArcaneTorrentForResourceAtPercentage)
        {
            ArcaneTorrent.Cast();
            return;
        }

        if (ObjectManager.Me.ManaPercentage < 50 && ManaTea.KnownSpell && ManaTea.IsSpellUsable
            && MySettings.UseManaTea && ObjectManager.Me.BuffStack(115867) > 4
            && !ObjectManager.Me.InCombat)
        {
            ManaTea.Cast();
            return;
        }
        if (ObjectManager.Me.HealthPercent < 95 && SurgingMist.KnownSpell && SurgingMist.IsSpellUsable
            && MySettings.UseSurgingMist && ObjectManager.Me.BuffStack(118674) > 4
            && !ObjectManager.Me.InCombat)
        {
            SurgingMist.Cast();
            return;
        }
        if (HealingSphere.KnownSpell && HealingSphere.IsSpellUsable && ObjectManager.Me.Energy > 39 &&
            ObjectManager.Me.HealthPercent < 60 && MySettings.UseHealingSphere && _healingSphereTimer.IsReady)
        {
            SpellManager.CastSpellByIDAndPosition(115460, ObjectManager.Me.Position);
            _healingSphereTimer = new Timer(1000*5);
            return;
        }
        if (ObjectManager.Me.HealthPercent < 70 && SurgingMist.KnownSpell && SurgingMist.IsSpellUsable
            && MySettings.UseSurgingMist)
        {
            SurgingMist.Cast();
            return;
        }
        if (ObjectManager.Me.HealthPercent < 85 && Uplift.KnownSpell && Uplift.IsSpellUsable
            && MySettings.UseUplift && RenewingMist.HaveBuff)
        {
            Uplift.Cast();
            return;
        }
        if (ObjectManager.Me.HealthPercent < 85 && ChiWave.KnownSpell && ChiWave.IsSpellUsable
            && MySettings.UseChiWave)
        {
            ChiWave.Cast();
            return;
        }
        if (ObjectManager.Me.HealthPercent < 90 && ChiBurst.KnownSpell && ChiBurst.IsSpellUsable
            && MySettings.UseChiBurst)
        {
            ChiBurst.Cast();
            return;
        }
        if (ObjectManager.Me.HealthPercent < 90 && ExpelHarm.KnownSpell && ExpelHarm.IsSpellUsable
            && MySettings.UseExpelHarm && ExpelHarm.IsHostileDistanceGood)
        {
            ExpelHarm.Cast();
            return;
        }
        if (ObjectManager.Me.HealthPercent < 90 && EnvelopingMist.KnownSpell && EnvelopingMist.IsSpellUsable
            && MySettings.UseEnvelopingMist && !EnvelopingMist.HaveBuff)
        {
            EnvelopingMist.Cast();
            return;
        }
        if (ObjectManager.Me.HealthPercent < 95 && SurgingMist.KnownSpell && SurgingMist.IsSpellUsable
            && MySettings.UseSurgingMist && ObjectManager.Me.BuffStack(118674) > 4)
        {
            SurgingMist.Cast();
            return;
        }
        if (ObjectManager.Me.HealthPercent < 95 && SoothingMist.KnownSpell && SoothingMist.IsSpellUsable
            && MySettings.UseSoothingMist && !SoothingMist.HaveBuff && !ObjectManager.Me.IsCast)
        {
            SoothingMist.Cast();
            return;
        }
        if (ObjectManager.Me.HealthPercent < 95 && RenewingMist.KnownSpell && RenewingMist.IsSpellUsable
            && MySettings.UseRenewingMist && !RenewingMist.HaveBuff)
        {
            RenewingMist.Cast();
            return;
        }
        if (ObjectManager.Me.HealthPercent < 95 && ZenSphere.KnownSpell && ZenSphere.IsSpellUsable
            && MySettings.UseZenSphere)
        {
            ZenSphere.Cast();
        }
    }

    private void Decast()
    {
        if (ArcaneTorrent.KnownSpell && MySettings.UseArcaneTorrentForDecast && ArcaneTorrent.IsSpellUsable &&
            ObjectManager.Me.HealthPercent <= MySettings.UseArcaneTorrentForDecastAtPercentage
            && ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && ObjectManager.Target.GetDistance < 8)
        {
            ArcaneTorrent.Cast();
            return;
        }
        if (DiffuseMagic.KnownSpell && MySettings.UseDiffuseMagic && DiffuseMagic.IsSpellUsable
            && ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe)
        {
            DiffuseMagic.Cast();
            return;
        }
        if (SpearHandStrike.KnownSpell && MySettings.UseSpearHandStrike && ObjectManager.Target.IsCast
            && SpearHandStrike.IsSpellUsable && SpearHandStrike.IsHostileDistanceGood)
        {
            SpearHandStrike.Cast();
            return;
        }
        if (ObjectManager.Target.GetMove && !Disable.TargetHaveBuff && MySettings.UseDisable
            && Disable.KnownSpell && Disable.IsSpellUsable && Disable.IsHostileDistanceGood)
        {
            Disable.Cast();
        }
    }

    private void HealingBurst()
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
        if (ChiBrew.IsSpellUsable && ChiBrew.KnownSpell
            && MySettings.UseChiBrew && ObjectManager.Me.Chi == 0)
        {
            ChiBrew.Cast();
            return;
        }
        if (TouchofDeath.IsSpellUsable && TouchofDeath.KnownSpell && TouchofDeath.IsHostileDistanceGood
            && MySettings.UseTouchofDeath)
        {
            TouchofDeath.Cast();
            return;
        }
        if (InvokeXuentheWhiteTiger.IsSpellUsable && InvokeXuentheWhiteTiger.KnownSpell
            && MySettings.UseInvokeXuentheWhiteTiger && InvokeXuentheWhiteTiger.IsHostileDistanceGood)
        {
            InvokeXuentheWhiteTiger.Cast();
            return;
        }
        if (ThunderFocusTea.IsSpellUsable && ThunderFocusTea.KnownSpell
            && MySettings.UseThunderFocusTea && ObjectManager.Me.HealthPercent < 90)
        {
            ThunderFocusTea.Cast();
            return;
        }
        if (ObjectManager.Me.HealthPercent < 80 && Revival.KnownSpell && Revival.IsSpellUsable
            && MySettings.UseRevival)
        {
            Revival.Cast();
            return;
        }
        if (RushingJadeWind.IsSpellUsable && RushingJadeWind.KnownSpell && RushingJadeWind.IsHostileDistanceGood
            && MySettings.UseRushingJadeWind && ObjectManager.GetNumberAttackPlayer() > 3)
        {
            RushingJadeWind.Cast();
        }
    }

    private void DPSCycle()
    {
        Usefuls.SleepGlobalCooldown();
        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            if (ObjectManager.GetNumberAttackPlayer() > 2 && SpinningCraneKick.IsSpellUsable && SpinningCraneKick.KnownSpell
                && SpinningCraneKick.IsHostileDistanceGood && !ObjectManager.Me.IsCast && MySettings.UseSpinningCraneKick)
            {
                SpinningCraneKick.Cast();
                return;
            }
            if (CracklingJadeLightning.KnownSpell && CracklingJadeLightning.IsSpellUsable
                && MySettings.UseCracklingJadeLightning && ObjectManager.Me.Chi < 4 && CracklingJadeLightning.IsHostileDistanceGood
                && !ExpelHarm.IsHostileDistanceGood)
            {
                CracklingJadeLightning.Cast();
                return;
            }
            if (BlackoutKick.KnownSpell && BlackoutKick.IsSpellUsable && BlackoutKick.IsHostileDistanceGood
                && MySettings.UseBlackoutKick && (!ObjectManager.Me.HaveBuff(127722) || _serpentsZealTimer.IsReady))
            {
                BlackoutKick.Cast();
                _serpentsZealTimer = new Timer(1000*25);
                return;
            }
            if (ObjectManager.Me.HealthPercent < 91 && ExpelHarm.KnownSpell && ExpelHarm.IsSpellUsable
                && MySettings.UseExpelHarm && ObjectManager.Me.Chi < 4 && ExpelHarm.IsHostileDistanceGood)
            {
                ExpelHarm.Cast();
                return;
            }
            if (Jab.KnownSpell && Jab.IsSpellUsable && MySettings.UseJab && ObjectManager.Me.Chi < 4
                && Jab.IsHostileDistanceGood)
            {
                Jab.Cast();
                return;
            }
            if (TigerPalm.KnownSpell && TigerPalm.IsSpellUsable && TigerPalm.IsHostileDistanceGood
                && MySettings.UseTigerPalm && ObjectManager.Me.HealthPercent > 90
                && ObjectManager.Me.BuffStack(125359) < 3)
            {
                TigerPalm.Cast();
            }
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    private void Patrolling()
    {
        if (ObjectManager.Me.IsMounted) return;
        Buff();
        Heal();
    }

    #region Nested type: MonkMistweaverSettings

    [Serializable]
    public class MonkMistweaverSettings : Settings
    {
        public bool DoAvoidMelee = false;
        public int DoAvoidMeleeDistance = 0;
        public bool UseAlchFlask = true;
        public bool UseArcaneTorrentForDecast = true;
        public int UseArcaneTorrentForDecastAtPercentage = 100;
        public bool UseArcaneTorrentForResource = true;
        public int UseArcaneTorrentForResourceAtPercentage = 80;
        public bool UseBerserking = true;
        public bool UseBlackoutKick = true;
        public bool UseBloodFury = true;
        public bool UseChargingOxWave = true;
        public bool UseChiBrew = true;
        public bool UseChiBurst = true;
        public bool UseChiWave = true;
        public bool UseCracklingJadeLightning = true;
        public bool UseDampenHarm = true;
        public bool UseDiffuseMagic = true;
        public bool UseDisable = false;

        public bool UseEnvelopingMist = true;
        public bool UseExpelHarm = true;
        public bool UseFortifyingBrew = true;
        public bool UseGiftoftheNaaru = true;
        public bool UseGrappleWeapon = true;
        public bool UseHealingSphere = true;
        public bool UseInvokeXuentheWhiteTiger = true;
        public bool UseJab = true;
        public bool UseLegSweep = true;
        public bool UseLegacyoftheEmperor = true;
        public bool UseLifeCocoon = true;

        public bool UseManaTea = true;
        public bool UsePathofBlossoms = true;
        public bool UseProvoke = true;
        public bool UseRenewingMist = true;
        public bool UseRevival = true;
        public bool UseRoll = true;
        public bool UseRushingJadeWind = true;
        public bool UseSoothingMist = false;
        public bool UseSpearHandStrike = true;
        public bool UseSpinningCraneKick = true;
        public bool UseStanceoftheFierceTiger = true;
        public bool UseStanceoftheWiseSerpent = true;
        public bool UseStoneform = true;
        public bool UseSummonJadeSerpentStatue = true;
        public bool UseSurgingMist = true;
        public bool UseThunderFocusTea = true;
        public bool UseTigerPalm = true;
        public bool UseTigersLust = true;
        public bool UseTouchofDeath = true;
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;
        public bool UseUplift = true;
        public bool UseWarStomp = true;
        public bool UseZenMeditation = true;
        public bool UseZenSphere = true;

        public MonkMistweaverSettings()
        {
            ConfigWinForm("Mistweaver Monk Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent For Decast", "UseArcaneTorrentForDecast", "Professions & Racials", "AtPercentage");
            AddControlInWinForm("Use Arcane Torrent For Resource", "UseArcaneTorrentForResource", "Professions & Racials", "AtPercentage");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");

            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            /* Monk Buffs */
            AddControlInWinForm("Use Disable", "UseDisable", "Monk Buffs");
            AddControlInWinForm("Use Legacy of the Emperor", "UseLegacyoftheEmperor", "Monk Buffs");
            AddControlInWinForm("Use Stance of the Fierce Tiger", "UseStanceoftheFierceTiger", "Monk Buffs");
            AddControlInWinForm("Use Summon Jade Serpent Statue", "UseSummonJadeSerpentStatue", "Monk Buffs");
            AddControlInWinForm("Use Tiger's Lust", "UseTigersLust", "Monk Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Chi Wave", "UseChiWave", "Offensive Spell");
            AddControlInWinForm("Use Blackout Kick", "UseBlackoutKick", "Offensive Spell");
            AddControlInWinForm("Use Crackling Jade Lightning", "UseCracklingJadeLightning", "Offensive Spell");
            AddControlInWinForm("Use Jab", "UseJab", "Offensive Spell");
            AddControlInWinForm("Use Path of Blossoms", "UsePathofBlossoms", "Offensive Spell");
            AddControlInWinForm("Use Provoke", "UseProvoke", "Offensive Spell");
            AddControlInWinForm("Use Roll", "UseRoll", "Offensive Spell");
            AddControlInWinForm("Use Spinning Crane Kick", "UseSpinningCraneKick", "Offensive Spell");
            AddControlInWinForm("Use Tiger Palm", "UseTigerPalm", "Offensive Spell");
            AddControlInWinForm("Use Touch of Death", "UseTouchofDeath", "Offensive Spell");
            /* Healing Cooldown */
            AddControlInWinForm("Use Chi Brew", "UseChiBrew", "Healing Cooldown");
            AddControlInWinForm("Use Invoke Xuen, the White Tiger", "UseInvokeXuentheWhiteTiger", "Healing Cooldown");
            AddControlInWinForm("Use Revival", "UseRevival", "Healing Cooldown");
            AddControlInWinForm("Use Rushing Jade Wind", "UseRushingJadeWind", "Healing Cooldown");
            AddControlInWinForm("Use Thunder Focus Tea", "UseThunderFocusTea", "Healing Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Charging Ox Wave", "UseChargingOxWave", "Defensive Cooldown");
            AddControlInWinForm("Use Dampen Harm ", "UseDampenHarm ", "Defensive Cooldown");
            AddControlInWinForm("Use Diffuse Magic", "UseDiffuseMagic", "Defensive Cooldown");
            AddControlInWinForm("Use Fortifying Brew", "UseFortifyingBrew", "Defensive Cooldown");
            AddControlInWinForm("Use Grapple Weapon", "UseGrappleWeapon", "Defensive Cooldown");
            AddControlInWinForm("Use Leg Sweep", "UseLegSweep", "Defensive Cooldown");
            AddControlInWinForm("Use Life Cocoon", "UseLifeCocoon", "Defensive Cooldown");
            AddControlInWinForm("Use Spear Hand Strike", "UseSpearHandStrike", "Defensive Cooldown");
            AddControlInWinForm("Use Zen Meditation", "UseZenMeditation", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Chi Burst", "UseChiBurst", "Healing Spell");
            AddControlInWinForm("Use Enveloping Mist", "UseEnvelopingMist", "Healing Spell");
            AddControlInWinForm("Use Expel Harm", "UseExpelHarm", "Healing Spell");
            AddControlInWinForm("Use Healing Sphere", "UseHealingSphere", "Healing Spell");
            AddControlInWinForm("Use Mana Tea", "UseManaTea", "Healing Spell");
            AddControlInWinForm("Use Renewing Mist", "UseRenewingMist", "Healing Spell");
            AddControlInWinForm("Use Soothing Mist", "UseSoothingMist", "Healing Spell");
            AddControlInWinForm("Use Surging Mist", "UseSurgingMist", "Healing Spell");
            AddControlInWinForm("Use Uplift", "UseUplift", "Healing Spell");
            AddControlInWinForm("Use Zen Sphere", "UseZenSphere", "Healing Spell");
            /* Game Settings */
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");

            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
            AddControlInWinForm("Do avoid melee (Off Advised!!)", "DoAvoidMelee", "Game Settings");
            AddControlInWinForm("Avoid melee distance (1 to 4)", "DoAvoidMeleeDistance", "Game Settings");
        }

        public static MonkMistweaverSettings CurrentSetting { get; set; }

        public static MonkMistweaverSettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Monk_Mistweaver.xml";
            if (File.Exists(currentSettingsFile))
            {
                return
                    CurrentSetting = Load<MonkMistweaverSettings>(currentSettingsFile);
            }
            return new MonkMistweaverSettings();
        }
    }

    #endregion
}

#endregion

// ReSharper restore ObjectCreationAsStatement
// ReSharper restore EmptyGeneralCatchClause