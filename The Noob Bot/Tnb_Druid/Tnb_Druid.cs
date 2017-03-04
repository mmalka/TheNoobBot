/*
* CombatClass for TheNoobBot
* Credit : Vesper, Neo2003, Dreadlocks, Ryuichiro
* Thanks you !
*/

using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Diagnostics;

// ReSharper disable EmptyGeneralCatchClause
// ReSharper disable ObjectCreationAsStatement

public class Main : ICombatClass
{
    internal static float InternalRange = 5.0f;
    internal static float InternalAggroRange = 5.0f;
    internal static bool InternalLoop = true;
    internal static Spell InternalLightHealingSpell;
    internal static float Version = 1.02f;

    #region ICombatClass Members

    public float AggroRange
    {
        get { return InternalAggroRange; }
    }

    public Spell LightHealingSpell
    {
        get { return InternalLightHealingSpell; }
        set { InternalLightHealingSpell = value; }
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
                    #region Druid Specialisation checking

                case WoWClass.Druid:

                    if (wowSpecialization == WoWSpecialization.DruidFeral || wowSpecialization == WoWSpecialization.None)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Druid_Feral.xml";
                            var currentSetting = new DruidFeral.DruidFeralSettings();
                            if (File.Exists(currentSettingsFile) && !resetSettings)
                            {
                                currentSetting = Settings.Load<DruidFeral.DruidFeralSettings>(currentSettingsFile);
                            }
                            currentSetting.ToForm();
                            currentSetting.Save(currentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Druid Feral Found");
                            EquipmentAndStats.SetPlayerSpe(WoWSpecialization.DruidFeral);
                            new DruidFeral();
                        }
                        break;
                    }
                    if (wowSpecialization == WoWSpecialization.DruidGuardian)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Druid_Guardian.xml";
                            var currentSetting = new DruidGuardian.DruidGuardianSettings();
                            if (File.Exists(currentSettingsFile) && !resetSettings)
                            {
                                currentSetting = Settings.Load<DruidGuardian.DruidGuardianSettings>(currentSettingsFile);
                            }
                            currentSetting.ToForm();
                            currentSetting.Save(currentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Feral Guardian Found");
                            EquipmentAndStats.SetPlayerSpe(WoWSpecialization.DruidGuardian);
                            new DruidGuardian();
                        }
                        break;
                    }
                    if (wowSpecialization == WoWSpecialization.DruidBalance)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Druid_Balance.xml";
                            var currentSetting = new DruidBalance.DruidBalanceSettings();
                            if (File.Exists(currentSettingsFile) && !resetSettings)
                            {
                                currentSetting = Settings.Load<DruidBalance.DruidBalanceSettings>(currentSettingsFile);
                            }
                            currentSetting.ToForm();
                            currentSetting.Save(currentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Druid Balance Found");
                            InternalRange = 30.0f;
                            EquipmentAndStats.SetPlayerSpe(WoWSpecialization.DruidBalance);
                            new DruidBalance();
                        }
                        break;
                    }
                    if (wowSpecialization == WoWSpecialization.DruidRestoration)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Druid_Restoration.xml";
                            var currentSetting = new DruidRestoration.DruidRestorationSettings();
                            if (File.Exists(currentSettingsFile) && !resetSettings)
                            {
                                currentSetting = Settings.Load<DruidRestoration.DruidRestorationSettings>(currentSettingsFile);
                            }
                            currentSetting.ToForm();
                            currentSetting.Save(currentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Druid Restoration Found");
                            InternalRange = 30.0f;
                            EquipmentAndStats.SetPlayerSpe(WoWSpecialization.DruidRestoration);
                            new DruidRestoration();
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
        Logging.WriteDebug("Loaded " + ObjectManager.Me.WowSpecialization() + " Combat Class " + Version.ToString("0.0###"));

        // Last field is intentionnally ommited because it's a backing field.
    }
}

#region Diagnostics

public static class Diagnostics
{
    private static Stopwatch stopwatch;
    private static string name;
    private static uint count;
    private static long timeMin;
    private static long timeMax;
    private static float timeAverage;

    public static void Init(string name)
    {
        Diagnostics.stopwatch = new Stopwatch();
        Diagnostics.name = name;
        Diagnostics.count = 0;
        Diagnostics.timeMin = long.MaxValue;
        Diagnostics.timeMax = long.MinValue;
        Diagnostics.timeAverage = 0;
        Logging.WriteDebug("Initialized Diagnostic Time for " + name);
    }

    public static void Summarize()
    {
        Logging.WriteDebug("Summarizing Diagnostics for '" + name + "' after " + count + " loops.");
        Logging.WriteDebug("Time = {Min: " + timeMin + ", Max: " + timeMax + ", Average: " + timeAverage + "}");
    }

    public static void Start()
    {
        stopwatch.Restart();
    }

    public static void Stop(bool log = true)
    {
        stopwatch.Stop();
        long ellapsedTime = stopwatch.ElapsedMilliseconds;
        timeMin = (ellapsedTime < timeMin) ? ellapsedTime : timeMin;
        timeMax = (ellapsedTime > timeMax) ? ellapsedTime : timeMax;
        timeAverage = ((timeAverage*count) + ellapsedTime)/++count;
        if (log)
        {
            Logging.WriteFileOnly("Time = {Ellapsed: " + ellapsedTime + ", Min: " + timeMin + ", Max: " + timeMax + ", Average: " + timeAverage + "} Loop: " + count);
        }
    }
}

#endregion

#region Druid

public class DruidBalance
{
    private static DruidBalanceSettings MySettings = DruidBalanceSettings.GetSettings();

    #region General Timers & Variables

    private readonly WoWItem _firstTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET);
    private readonly WoWItem _secondTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET, 2);

    private bool CombatMode = true;

    #endregion

    #region Professions & Racials

    private readonly Spell Berserking = new Spell("Berserking"); //No GCD
    private readonly Spell Darkflight = new Spell("Darkflight"); //No GCD
    private readonly Spell WarStomp = new Spell("War Stomp");

    #endregion

    #region Druid Buffs

    //private readonly Spell BearForm = new Spell("Bear Form");
    //private readonly Spell BlessingoftheAncients = new Spell("Blessing of the Ancients"); //No GCD //no need for the bot to switch the buff
    private readonly Spell CatForm = new Spell("Cat Form");
    private readonly Spell MoonkinForm = new Spell(24858);
    private readonly Spell TravelForm = new Spell("Travel Form");
    private readonly Spell OwlkinFrenzy = new Spell(157228);
    private readonly Spell LunarEmpowerment = new Spell(164547);
    private readonly Spell SolarEmpowerment = new Spell(164545);

    #endregion

    #region Druid DoTs

    private readonly Spell Moonfire = new Spell("Moonfire");
    private readonly Spell Sunfire = new Spell("Sunfire");
    private readonly Spell StellarFlare = new Spell("Stellar Flare");

    #endregion

    #region Offensive Spells

    private readonly Spell LunarStrike = new Spell("Lunar Strike");
    private readonly Spell SolarWrath = new Spell("Solar Wrath");
    private readonly Spell Starfall = new Spell("Starfall");
    private readonly Spell Starsurge = new Spell("Starsurge");

    #endregion

    #region Legion Artifact

    private readonly Spell NewMoon = new Spell(202767 /*"New Moon"*/);

    #endregion

    #region Offensive Cooldowns

    private readonly Spell AstralCommunion = new Spell("Astral Communion"); //No GCD
    private readonly Spell CelestialAlignment = new Spell("Celestial Alignment");
    private readonly Spell ForceofNature = new Spell("Force of Nature");
    private readonly Spell FuryofElune = new Spell("Fury of Elune");
    private readonly Spell Incarnation = new Spell("Incarnation: Chosen of Elune");
    private readonly Spell WarriorofElune = new Spell("Warrior of Elune"); //No GCD

    #endregion

    #region Defensive Cooldowns

    private readonly Spell Barkskin = new Spell("Barkskin"); //No GCD
    //private readonly Spell EntanglingRoots = new Spell("Entangling Roots");
    //private readonly Spell MassEntanglement = new Spell("Mass Entanglement");
    private readonly Spell MightyBash = new Spell("Mighty Bash");
    //private readonly Spell SolarBeam = new Spell("Solar Beam");
    //private readonly Spell Typhoon = new Spell("Typhoon");
    //private readonly Spell WildCharge = new Spell("Wild Charge"); //No GCD

    #endregion

    #region Healing Spells

    private readonly Spell HealingTouch = new Spell("Healing Touch");
    private readonly Spell Regrowth = new Spell("Regrowth");
    private readonly Spell Rejuvenation = new Spell("Rejuvenation");
    private readonly Spell Renewal = new Spell("Renewal"); //No GCD
    private readonly Spell Swiftmend = new Spell("Swiftmend");

    #endregion

    #region Utility Cooldowns

    private readonly Spell Dash = new Spell(1850 /*"Dash"*/); //No GCD
    //private readonly Spell DisplacerBeast = new Spell("Displacer Beast");
    //private readonly Spell Prowl = new Spell("Prowl"); //No GCD

    #endregion

    public DruidBalance()
    {
        Main.InternalRange = 45f;
        Main.InternalAggroRange = 45f;
        Main.InternalLightHealingSpell = Regrowth;
        MySettings = DruidBalanceSettings.GetSettings();
        Main.DumpCurrentSettings<DruidBalanceSettings>(MySettings);
        UInt128 lastTarget = 0;

        Diagnostics.Init("Balance Druid");

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
                            if (ObjectManager.Me.Target != lastTarget)
                            {
                                lastTarget = ObjectManager.Me.Target;
                            }

                            if (Moonfire.IsHostileDistanceGood)
                                Combat();
                            else
                                Patrolling();
                        }
                        else
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
        Diagnostics.Summarize();
    }

    private void Patrolling()
    {
        if (!ObjectManager.Me.IsCast)
        {
            //Log
            if (CombatMode)
            {
                Logging.WriteFight("Patrolling:");
                CombatMode = false;
            }

            if (ObjectManager.Me.GetMove && !Usefuls.PlayerUsingVehicle)
            {
                //Travel Form while swimming
                if (MySettings.UseAquaticTravelFormOOC && Usefuls.IsSwimming && TravelForm.IsSpellUsable)
                {
                    TravelForm.Cast();
                    return;
                }
                //Movement Buffs
                if (!Darkflight.HaveBuff && !Dash.HaveBuff)
                {
                    if (MySettings.UseDarkflight && Darkflight.IsSpellUsable) //they don't stack
                    {
                        Darkflight.Cast();
                    }
                    else if (MySettings.UseDash && Dash.IsSpellUsable && (MySettings.UseCatFormOOC || CatForm.HaveBuff))
                    {
                        Dash.Cast();
                    }
                }
                //Cat Form
                if (MySettings.UseCatFormOOC && CatForm.IsSpellUsable && !CatForm.HaveBuff)
                {
                    CatForm.Cast();
                    return;
                }
            }
            else
            {
                //Self Heal for Damage Dealer
                if (nManager.Products.Products.ProductName == "Damage Dealer" && Main.InternalLightHealingSpell.IsSpellUsable &&
                    ObjectManager.Me.HealthPercent < 90 && ObjectManager.Target.Guid == ObjectManager.Me.Guid)
                {
                    Main.InternalLightHealingSpell.CastOnSelf();
                    return;
                }
            }
        }
    }

    private void Combat()
    {
        //Log
        if (!CombatMode)
        {
            Logging.WriteFight("Combat:");
            CombatMode = true;
        }
        //Heal
        Heal();
        if (Defensive())
            return;
        //Tag
        if (MySettings.TagOnly && MySettings.UseSunfire)
            Tag();
        else
        {
            if (Shapeshift())
                return;
            BurstBuffs();
            GCDCycle();
        }
    }

    private void Heal()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            //Swiftmend
            if (Swiftmend.IsSpellUsable && ObjectManager.Me.HealthPercent < MySettings.UseSwiftmendBelowPercentage)
            {
                Swiftmend.CastOnSelf();
                return;
            }
            //Renewal
            if (Renewal.IsSpellUsable && ObjectManager.Me.HealthPercent < MySettings.UseRenewalBelowPercentage)
            {
                Renewal.CastOnSelf();
            }
            //Regrowth
            if (Regrowth.IsSpellUsable && ObjectManager.Me.HealthPercent < MySettings.UseRegrowthBelowPercentage &&
                !Regrowth.HaveBuff)
            {
                Regrowth.CastOnSelf();
                return;
            }

            //outside Moonkin Form
            if (!ObjectManager.Me.HaveBuff(MoonkinForm.Id))
            {
                //Rejuvenation
                if (Rejuvenation.IsSpellUsable && ObjectManager.Me.HealthPercent < MySettings.UseRejuvenationBelowPercentage &&
                    ObjectManager.Me.UnitAura(774, ObjectManager.Me.Guid).AuraTimeLeftInMs < 5000)
                {
                    Rejuvenation.CastOnSelf();
                    return;
                }
                //Healing Touch
                if (HealingTouch.IsSpellUsable && ObjectManager.Me.HealthPercent < MySettings.UseHealingTouchBelowPercentage)
                {
                    HealingTouch.CastOnSelf();
                    return;
                }
            }
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    private void Tag()
    {
        //No Blacklist, works with Sunfire Debuff
        Usefuls.SleepGlobalCooldown();
        try
        {
            Memory.WowMemory.GameFrameLock();

            //Offensive Spells
            if (MySettings.UseStarfall && Starfall.IsSpellUsable && Starfall.IsHostileDistanceGood &&
                ObjectManager.Target.GetUnitInSpellRange(15f) > 2)
            {
                Starfall.CastAtPosition(ObjectManager.Target.Position);
                return;
            }
            if (MySettings.UseSunfire && Sunfire.IsSpellUsable &&
                !ObjectManager.Target.UnitAura(Sunfire.Ids, ObjectManager.Me.Guid).IsValid && Sunfire.IsHostileDistanceGood)
            {
                Sunfire.Cast();
                return;
            }
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    private bool Defensive()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            //Defensive Cooldowns
            if (ObjectManager.Target.IsStunned)
            {
                if (ObjectManager.Target.IsStunnable)
                {
                    if (MightyBash.IsSpellUsable && ObjectManager.Me.HealthPercent < MySettings.UseMightyBashBelowPercentage)
                    {
                        MightyBash.Cast();
                        return true;
                    }
                    if (WarStomp.IsSpellUsable && ObjectManager.Me.HealthPercent < MySettings.UseWarStompBelowPercentage)
                    {
                        WarStomp.Cast();
                    }
                }
                if (Barkskin.IsSpellUsable && ObjectManager.Me.HealthPercent < MySettings.UseBarkskinBelowPercentage)
                {
                    Barkskin.Cast();
                }
            }
            return false;
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    private bool Shapeshift()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            //Moonkin Form
            if (MySettings.UseMoonkinForm && MoonkinForm.IsSpellUsable && !ObjectManager.Me.HaveBuff(MoonkinForm.Id))
            {
                MoonkinForm.Cast();
                return true;
            }

            return false;
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    private void BurstBuffs()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            //Only Burst when the main DotS are on the target
            if (!Moonfire.TargetHaveBuff && !Sunfire.TargetHaveBuff)
                return;

            //Burst Buffs
            if (MySettings.UseTrinketOne && !ItemsManager.IsItemOnCooldown(_firstTrinket.Entry) && ItemsManager.IsItemUsable(_firstTrinket.Entry))
            {
                ItemsManager.UseItem(_firstTrinket.Name);
                Logging.WriteFight("Use First Trinket Slot");
            }
            if (MySettings.UseTrinketTwo && !ItemsManager.IsItemOnCooldown(_secondTrinket.Entry) && ItemsManager.IsItemUsable(_secondTrinket.Entry))
            {
                ItemsManager.UseItem(_secondTrinket.Name);
                Logging.WriteFight("Use Second Trinket Slot");
            }
            if (MySettings.UseBerserking && Berserking.IsSpellUsable)
            {
                Berserking.Cast();
            }
            if (MySettings.UseCelestialAlignment && CelestialAlignment.IsSpellUsable) // && !CelestialAlignment.HaveBuff)
            {
                CelestialAlignment.Cast();
            }
            if (MySettings.UseIncarnation && Incarnation.IsSpellUsable && !Incarnation.HaveBuff)
            {
                Incarnation.Cast();
                return;
            }
            if (ObjectManager.Me.Eclipse < MySettings.UseAstralCommunionBelowPercentage && AstralCommunion.IsSpellUsable) // AstralPower, LunarPower, Eclipse
            {
                AstralCommunion.Cast();
            }
            if (MySettings.UseForceofNature && ForceofNature.IsSpellUsable)
            {
                ForceofNature.Cast();
                return;
            }
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    private void GCDCycle()
    {
        Usefuls.SleepGlobalCooldown();
        Diagnostics.Start();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            if (LunarEmpowerment.BuffStack == 3)
            {
                Logging.WriteFileOnly("Lunar Empowerment has 3 stacks!" + MySettings.UseLunarStrike + ", " + LunarStrike.IsSpellUsable + ", " + LunarStrike.IsHostileDistanceGood);
            }

            //Cast Solar Wrath when target isn't in combat
            if (MySettings.UseSolarWrath && SolarWrath.IsSpellUsable && SolarWrath.IsHostileDistanceGood &&
                !ObjectManager.Target.InCombat)
            {
                SolarWrath.Cast();
                return;
            }

            //Cast New Moon when you cap charges
            if (MySettings.UseNewMoon && NewMoon.IsSpellUsable && NewMoon.GetSpellCharges >= 3)
            {
                NewMoon.Cast();
                return;
            }

            //Apply Moonfire
            if (MySettings.UseMoonfire && Moonfire.IsSpellUsable && Moonfire.IsHostileDistanceGood && !Moonfire.TargetHaveBuff)
            {
                Moonfire.Cast();
                return;
            }

            //Apply Sunfire
            if (MySettings.UseSunfire && Sunfire.IsSpellUsable && Sunfire.IsHostileDistanceGood && !Sunfire.TargetHaveBuff)
            {
                Sunfire.Cast();
                return;
            }

            //Apply Stellar Flare
            if (MySettings.UseStellarFlare && StellarFlare.IsSpellUsable && StellarFlare.IsHostileDistanceGood && !StellarFlare.TargetHaveBuff)
            {
                StellarFlare.Cast();
                return;
            }

            //Cast LunarStrike when it's an Instant
            if (MySettings.UseLunarStrike && LunarStrike.IsSpellUsable && LunarStrike.IsHostileDistanceGood &&
                (OwlkinFrenzy.HaveBuff || WarriorofElune.HaveBuff))
            {
                LunarStrike.Cast();
                return;
            }

            //Pool Astral Power and New Moon charges for Fury of Elune when off cooldown
            if (MySettings.UseFuryofEluneAtPercentage > 0 && FuryofElune.IsSpellUsable && ObjectManager.Target.GetUnitInSpellRange(3f) >= 2)
            {
                //Cast Fury of Elune
                if (ObjectManager.Me.Eclipse >= MySettings.UseFuryofEluneAtPercentage && NewMoon.GetSpellCharges == 3 &&
                    !FuryofElune.HaveBuff)
                {
                    FuryofElune.CastAtPosition(ObjectManager.Target.Position);
                    return;
                }
                //Cast New Moon while Fury of Elune is active
                if (MySettings.UseNewMoon && NewMoon.IsSpellUsable && NewMoon.GetSpellCharges > 0 &&
                    FuryofElune.HaveBuff)
                {
                    NewMoon.Cast();
                    return;
                }
            }
            else
            {
                //Spend Astral Power on Starfall or Starsurge
                if (MySettings.UseStarfall && Starfall.KnownSpell && ObjectManager.Target.GetUnitInSpellRange(15f) >= 3)
                {
                    if (Starfall.IsSpellUsable && Starfall.IsHostileDistanceGood)
                    {
                        Starfall.CastAtPosition(ObjectManager.Target.Position);
                        return;
                    }
                }
                else if (MySettings.UseStarsurge && Starsurge.IsSpellUsable && Starsurge.IsHostileDistanceGood &&
                         (LunarEmpowerment.BuffStack < 3 || SolarEmpowerment.BuffStack < 3 || ObjectManager.Me.ManaPercentage == 100))
                {
                    Starsurge.Cast();
                    return;
                }

                //Cast New Moon when you have more then 1 charge
                if (MySettings.UseNewMoon && NewMoon.IsSpellUsable && NewMoon.GetSpellCharges > 1)
                {
                    NewMoon.Cast();
                    return;
                }
            }

            //Cast Solar Wrath when you cap Solar Empowerment and have less than 4 targets
            if (MySettings.UseSolarWrath && SolarWrath.IsSpellUsable && SolarWrath.IsHostileDistanceGood &&
                SolarEmpowerment.BuffStack == 3 && ObjectManager.GetUnitInSpellRange(8f) < 4)
            {
                SolarWrath.Cast();
                return;
            }

            //Cast Lunar Strike when you cap Lunar Empowerment
            if (MySettings.UseLunarStrike && LunarStrike.IsSpellUsable && LunarStrike.IsHostileDistanceGood &&
                LunarEmpowerment.BuffStack == 3)
            {
                //Cast Warrior of Elune is ready
                if (MySettings.UseWarriorofElune && WarriorofElune.IsSpellUsable)
                {
                    WarriorofElune.Cast();
                }
                LunarStrike.Cast();
            }

            //Maintain Moonfire (Pandemic Window)
            if (MySettings.UseMoonfire && Moonfire.IsSpellUsable && Moonfire.IsHostileDistanceGood &&
                ObjectManager.Target.UnitAura(Moonfire.Ids, ObjectManager.Me.Guid).AuraTimeLeftInMs < 5000)
            {
                Moonfire.Cast();
                return;
            }

            //Maintain Sunfire (Pandemic Window)
            if (MySettings.UseSunfire && Sunfire.IsSpellUsable && Sunfire.IsHostileDistanceGood &&
                ObjectManager.Target.UnitAura(Sunfire.Ids, ObjectManager.Me.Guid).AuraTimeLeftInMs < 4000)
            {
                Sunfire.Cast();
                return;
            }

            //Maintain Stellar Flare (Pandemic Window)
            if (MySettings.UseStellarFlare && StellarFlare.IsSpellUsable && StellarFlare.IsHostileDistanceGood &&
                ObjectManager.Target.UnitAura(StellarFlare.Ids, ObjectManager.Me.Guid).AuraTimeLeftInMs < 8000)
            {
                StellarFlare.Cast();
                return;
            }

            //Cast Lunar Strike when you will hit 2+ targets
            if (MySettings.UseLunarStrike && LunarStrike.IsSpellUsable && LunarStrike.IsHostileDistanceGood &&
                ObjectManager.GetUnitInSpellRange(8f) >= 2)
            {
                //Wait for 2 Lunar Empowerments when Warrior of Elune is off cooldown
                if (MySettings.UseWarriorofElune && WarriorofElune.IsSpellUsable &&
                    LunarEmpowerment.BuffStack >= 2)
                {
                    WarriorofElune.Cast();
                }
                LunarStrike.Cast();
            }

            //Cast Solar Wrath
            if (MySettings.UseSolarWrath && SolarWrath.IsSpellUsable && SolarWrath.IsHostileDistanceGood)
            {
                SolarWrath.Cast();
                return;
            }
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
            Diagnostics.Stop();
        }
    }

    #region Nested type: DruidBalanceSettings

    [Serializable]
    public class DruidBalanceSettings : Settings
    {
        /* Professions & Racials */
        public bool UseBerserking = true;
        public bool UseDarkflight = true;
        public int UseWarStompBelowPercentage = 25;

        /* Druid Buffs */
        public bool UseAquaticTravelFormOOC = true;
        public bool UseCatFormOOC = false;
        public bool UseMoonkinForm = true;
        //public bool UseBlessingoftheAncients  = true; //TEST THIS FIRST

        /* Druid DoTs */
        public bool UseMoonfire = true;
        public bool UseSunfire = true;
        public bool UseStellarFlare = true;

        /* Offensive Spells */
        public bool UseLunarStrike = true;
        public bool UseSolarWrath = true;
        public bool UseStarfall = true;
        public bool UseStarsurge = true;

        /* Artifact Spells */
        public bool UseNewMoon = true;

        /* Offensive Cooldowns */
        public bool UseCelestialAlignment = true;
        public bool UseForceofNature = true;
        public int UseFuryofEluneAtPercentage = 90;
        public bool UseWarriorofElune = true;
        public bool UseIncarnation = true;
        public int UseAstralCommunionBelowPercentage = 15;

        /* Defensive Cooldowns */
        public int UseBarkskinBelowPercentage = 75;
        //public bool UseEntanglingRoots = true;
        //public bool UseMassEntanglement = true;
        public int UseMightyBashBelowPercentage = 25;
        //public bool UseSolarBeam = true;
        //public bool UseTyphoon = true;
        //public bool UseWildCharge = true;

        /* Healing Spells */
        public int UseHealingTouchBelowPercentage = 25;
        public int UseRegrowthBelowPercentage = 0;
        public int UseRejuvenationBelowPercentage = 75;
        public int UseRenewalBelowPercentage = 65;
        public int UseSwiftmendBelowPercentage = 25;

        /* Utility Cooldowns */
        public bool UseDash = true;
        //public bool UseDisplacerBeast = true;
        //public bool UseProwl = true;

        /* Game Settings */
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;
        public bool TagOnly = false;

        /* Diagnostics */
        public bool LogDiagnostics = false;
        public bool LogDiagnosticsEachLoop = false;

        public DruidBalanceSettings()
        {
            ConfigWinForm("Druid Balance Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Darkflight", "UseDarkflight", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStompBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            /* Druid Buffs */
            AddControlInWinForm("Use Aquatic Travel Form out of Combat", "UseAquaticTravelFormOOC", "Druid Buffs");
            AddControlInWinForm("Use Cat Form out of Combat", "UseCatFormOOC", "Druid Buffs");
            AddControlInWinForm("Use Moonkin Form", "UseMoonkinForm", "Druid Buffs");
            /* Druid DoTs */
            AddControlInWinForm("Use Moonfire", "UseMoonfire", "Druid DoTs");
            AddControlInWinForm("Use Sunfire", "UseSunfire", "Druid DoTs");
            AddControlInWinForm("Use StellarFlare", "UseStellarFlare", "Druid DoTs");
            /* Offensive Spells */
            AddControlInWinForm("Use LunarStrike", "UseLunarStrike", "Offensive Spells");
            AddControlInWinForm("Use SolarWrath", "UseSolarWrath", "Offensive Spells");
            AddControlInWinForm("Use Starsurge", "UseStarsurge", "Offensive Spells");
            AddControlInWinForm("Use Starfall", "UseStarfall", "Offensive Spells");
            /* Artifact Spells */
            AddControlInWinForm("Use New/Half/Full Moon", "UseNewMoon", "Artifact Spells");
            /* Offensive Cooldowns */
            AddControlInWinForm("Use Celestial Alignment", "UseCelestialAlignment", "Offensive Cooldowns");
            AddControlInWinForm("Use Force of Nature", "UseForceofNature", "Offensive Cooldowns");
            AddControlInWinForm("Use Fury of Elune", "UseFuryofEluneAtPercentage", "Offensive Cooldowns", "AtPercentage", "Astral Power");
            AddControlInWinForm("Use Warrior of Elune", "UseWarriorofElune", "Offensive Cooldowns");
            AddControlInWinForm("Use Incarnation: Chosen of Elune", "UseIncarnation", "Offensive Cooldowns");
            AddControlInWinForm("Use Astral Communion", "UseAstralCommunionBelowPercentage", "Offensive Cooldowns", "BelowPercentage", "Astral Power");
            /* Defensive Cooldowns */
            AddControlInWinForm("Use Barkskin", "UseBarkskinBelowPercentage", "Defensive Cooldowns", "BelowPercentage", "Life");
            AddControlInWinForm("Use Mighty Bash", "UseMightyBashBelowPercentage", "Defensive Cooldowns", "BelowPercentage", "Life");
            /* Healing Spells */
            AddControlInWinForm("Use Healing Touch", "UseHealingTouchBelowPercentage", "Healing Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Regrowth", "UseRegrowthBelowPercentage", "Healing Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Rejuvenation", "UseRejuvenationBelowPercentage", "Healing Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Renewal", "UseRenewalBelowPercentage", "Healing Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Swiftmend", "UseSwiftmendBelowPercentage", "Healing Spells", "BelowPercentage", "Life");
            /* Utility Cooldowns */
            AddControlInWinForm("Use Dash", "UseDash", "Utility Cooldowns");
            /* Game Settings */
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");
            AddControlInWinForm("Only Tags Enemies ", "TagOnly", "Game Settings");
            /* Diagnostics */
            AddControlInWinForm("Log Diagnostics", "LogDiagnostics", "Logging");
            AddControlInWinForm("Log Diagnostics each Loop", "LogDiagnosticsEachLoop", "Logging");
        }

        public static DruidBalanceSettings CurrentSetting { get; set; }

        public static DruidBalanceSettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Druid_Balance.xml";
            if (File.Exists(currentSettingsFile))
            {
                return
                    CurrentSetting = Load<DruidBalanceSettings>(currentSettingsFile);
            }
            return new DruidBalanceSettings();
        }
    }

    #endregion
}

public class DruidFeral
{
    private static DruidFeralSettings MySettings = DruidFeralSettings.GetSettings();

    #region General Timers & Variables

    private readonly WoWItem _firstTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET);
    private readonly WoWItem _secondTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET, 2);

    private bool CombatMode = true;
    private bool EmptyLoop = false;
    private int ProwlTime = 0;

    #endregion

    #region Talents

    private readonly Spell Bloodtalons = new Spell(155672);
    private readonly Spell LunarInspiration = new Spell("Lunar Inspiration");
    private readonly Spell Predator = new Spell("Predator");
    private readonly Spell Sabertooth = new Spell("Sabertooth");

    #endregion

    #region Professions & Racials

    private readonly Spell Berserking = new Spell("Berserking"); //No GCD
    private readonly Spell Darkflight = new Spell("Darkflight"); //No GCD
    private readonly Spell WarStomp = new Spell("War Stomp");

    #endregion

    #region Druid Buffs

    //private readonly Spell BearForm = new Spell("Bear Form");
    private readonly Spell BloodtalonsBuff = new Spell(145152);
    private readonly Spell ClearcastingBuff = new Spell(135700);
    private readonly Spell CatForm = new Spell("Cat Form");
    //private readonly Spell MoonkinForm = new Spell("Moonkin Form");
    private readonly Spell TravelForm = new Spell("Travel Form");
    private readonly Spell PredatorySwiftnessBuff = new Spell(69369);
    private readonly Spell SavageRoarBuff = new Spell(52610);

    #endregion

    #region Druid DoTs

    private readonly Spell Moonfire = new Spell("Moonfire");
    private readonly Spell Rake = new Spell("Rake");
    private readonly Spell Rip = new Spell("Rip");
    private readonly Spell Thrash = new Spell("Thrash");

    #endregion

    #region Offensive Spells

    private readonly Spell BrutalSlash = new Spell(202028); //There is a SpellId that blocks the Talent init by name: 187191
    private readonly Spell FerociousBite = new Spell("Ferocious Bite");
    private readonly Spell SavageRoar = new Spell("Savage Roar");
    private readonly Spell Shred = new Spell("Shred");
    private readonly Spell Swipe = new Spell("Swipe");

    #endregion

    #region Legion Artifact

    private readonly Spell AshamanesFrenzy = new Spell("Ashamane's Frenzy");

    #endregion

    #region Offensive Cooldowns

    private readonly Spell Berserk = new Spell("Berserk"); //No GCD
    private readonly Spell ElunesGuidance = new Spell("Elune's Guidance"); //No GCD
    private readonly Spell Incarnation = new Spell("Incarnation: King of the Jungle");
    private readonly Spell TigersFury = new Spell("Tiger's Fury"); //No GCD

    #endregion

    #region Defensive Cooldowns

    //private readonly Spell EntanglingRoots = new Spell("Entangling Roots");
    //private readonly Spell Maim = new Spell("Maim");
    //private readonly Spell MassEntanglement = new Spell("Mass Entanglement");
    private readonly Spell MightyBash = new Spell("Mighty Bash");
    //private readonly Spell SkullBash = new Spell("Skull Bash"); //No GCD
    private readonly Spell SurvivalInstincts = new Spell("Survival Instincts"); //No GCD
    //private readonly Spell Typhoon = new Spell("Typhoon");
    //private readonly Spell WildCharge = new Spell("Wild Charge"); //No GCD

    #endregion

    #region Healing Spells

    private readonly Spell Regrowth = new Spell("Regrowth");
    private readonly Spell Rejuvenation = new Spell("Rejuvenation");
    private readonly Spell Renewal = new Spell("Renewal"); //No GCD
    private readonly Spell Swiftmend = new Spell("Swiftmend");

    #endregion

    #region Utility Cooldowns

    private readonly Spell Dash = new Spell(1850 /*"Dash"*/); //No GCD
    //private readonly Spell DisplacerBeast = new Spell("Displacer Beast");
    private readonly Spell Prowl = new Spell("Prowl"); //No GCD
    private readonly Spell StampedingRoar = new Spell("Stampeding Roar");

    #endregion

    public DruidFeral()
    {
        Main.InternalRange = ObjectManager.Me.GetCombatReach; // 1.5
        Main.InternalAggroRange = Main.InternalRange;
        Main.InternalLightHealingSpell = Regrowth;
        MySettings = DruidFeralSettings.GetSettings();
        Main.DumpCurrentSettings<DruidFeralSettings>(MySettings);
        UInt128 lastTarget = 0;

        Diagnostics.Init("Feral Druid");

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
                            if (ObjectManager.Me.Target != lastTarget)
                            {
                                lastTarget = ObjectManager.Me.Target;
                            }

                            if (Moonfire.IsHostileDistanceGood)
                                Combat();
                            else
                                Patrolling();
                        }
                        else
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
        Diagnostics.Summarize();
    }

    private void Patrolling()
    {
        if (!ObjectManager.Me.IsCast)
        {
            //Log
            if (CombatMode)
            {
                Logging.WriteFight("Patrolling:");
                CombatMode = false;
            }

            if (ObjectManager.Me.GetMove && !Usefuls.PlayerUsingVehicle)
            {
                //Travel Form while swimming
                if (MySettings.UseAquaticTravelFormOOC && Usefuls.IsSwimming && TravelForm.IsSpellUsable)
                {
                    TravelForm.Cast();
                    return;
                }
                //Movement Buffs
                if (!Darkflight.HaveBuff && !Dash.HaveBuff && !StampedingRoar.HaveBuff) //they don't stack
                {
                    if (MySettings.UseDarkflight && Darkflight.IsSpellUsable)
                    {
                        Darkflight.Cast();
                        return;
                    }
                    if (MySettings.UseDash && Dash.IsSpellUsable && (MySettings.UseCatFormOOC || CatForm.HaveBuff))
                    {
                        Dash.Cast();
                        return;
                    }
                    if (MySettings.UseStampedingRoar && StampedingRoar.IsSpellUsable && CatForm.HaveBuff)
                    {
                        StampedingRoar.Cast();
                        return;
                    }
                }
                //Stealth
                if (MySettings.UseProwlOOC && Prowl.IsSpellUsable && !Prowl.HaveBuff &&
                    !ObjectManager.Target.IsLootable)
                {
                    Prowl.Cast();
                    return;
                }
                //Cat Form
                if (MySettings.UseCatFormOOC && CatForm.IsSpellUsable && !CatForm.HaveBuff)
                {
                    CatForm.Cast();
                    return;
                }
            }
            else
            {
                //Self Heal for Damage Dealer
                if (nManager.Products.Products.ProductName == "Damage Dealer" && Main.InternalLightHealingSpell.IsSpellUsable &&
                    ObjectManager.Me.HealthPercent < 90 && ObjectManager.Target.Guid == ObjectManager.Me.Guid)
                {
                    Main.InternalLightHealingSpell.CastOnSelf();
                    return;
                }
            }
        }
    }

    private void Combat()
    {
        //Log
        if (!CombatMode)
        {
            Logging.WriteFight("Combat:");
            CombatMode = true;
        }
        Stealth();
        if (Prowl.HaveBuff)
            return;
        Shapeshift();
        Healing();
        Defensive();
        Offensive();
        Rotation();
    }

    private void Stealth()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            if (!Prowl.HaveBuff)
            {
                ProwlTime = 0;
                //Cast Prowl
                if (MySettings.UseProwl && Prowl.IsSpellUsable && !Prowl.HaveBuff)
                {
                    Prowl.Cast();
                    return;
                }
            }
            else
            {
                ProwlTime = ProwlTime != 0 ? ProwlTime : Usefuls.GetWoWTime;
                //1. Priority: Rake
                if (MySettings.UseRake && Rake.IsSpellUsable && Rake.IsHostileDistanceGood && ObjectManager.Target.IsStunnable)
                {
                    Logging.WriteDebug("Rake IsHostileDistanceGood: " + Rake.IsHostileDistanceGood + ", Target Distance: " + ObjectManager.Target.GetDistance + ", CombatReach - Me: " +
                                       ObjectManager.Me.GetCombatReach + ", Target: " + ObjectManager.Target.GetCombatReach + ", Rake MaxRangeHostile: " + Rake.MaxRangeHostile + " (Tooltip: Melee)");
                    Rake.Cast();
                    return;
                }
                //2. Priority: Shred
                if (MySettings.UseShred && Shred.IsSpellUsable && Shred.IsHostileDistanceGood)
                {
                    Shred.Cast();
                    return;
                }
                //3. Priority: Moonfire
                if (MySettings.UseMoonfire && Usefuls.GetWoWTime - ProwlTime > 5000 &&
                    Moonfire.IsSpellUsable && Moonfire.IsHostileDistanceGood)
                {
                    Shred.Cast();
                    return;
                }
            }
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    private void Healing()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            //Cast Regrowth when it would expire before you could use it or
            if (ObjectManager.Me.UnitAura(PredatorySwiftnessBuff.Id).AuraTimeLeftInMs < 2000 ||
                //you have less than 10% Health
                ObjectManager.Me.Health < 10)
            {
                if (CastRegrowth())
                    return;
            }
            //Renewal
            if (Renewal.IsSpellUsable && ObjectManager.Me.HealthPercent < 70)
            {
                Renewal.CastOnSelf();
                return;
            }
            //Restoration Affinity
            if (ObjectManager.Me.HealthPercent < MySettings.UseRestorationAffinityBelowPercentage)
            {
                //Swiftmend
                if (Swiftmend.IsSpellUsable)
                {
                    Swiftmend.CastOnSelf();
                    return;
                }
                //Rejuvenation
                if (Rejuvenation.IsSpellUsable && ObjectManager.Me.UnitAura(774, ObjectManager.Me.Guid).AuraTimeLeftInMs < 5000)
                {
                    Rejuvenation.CastOnSelf();
                    return;
                }
            }
            return;
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    private void Defensive()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            //Defensive Cooldowns
            if (ObjectManager.Target.IsStunned)
            {
                //Stun
                if (ObjectManager.Target.IsStunnable)
                {
                    if (MightyBash.IsSpellUsable && ObjectManager.Me.HealthPercent < MySettings.UseMightyBashBelowPercentage)
                    {
                        MightyBash.Cast();
                        return;
                    }
                    if (WarStomp.IsSpellUsable && ObjectManager.Me.HealthPercent < MySettings.UseWarStompBelowPercentage)
                    {
                        WarStomp.Cast();
                        return;
                    }
                }
                //Mitigate Damage
                if (SurvivalInstincts.IsSpellUsable && (SurvivalInstincts.GetSpellCharges == 2 ||
                                                        ObjectManager.Me.HealthPercent < MySettings.UseSurvivalInstinctsBelowPercentage))
                {
                    SurvivalInstincts.Cast();
                    return;
                }
            }
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    private void Shapeshift()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            if (!CatForm.HaveBuff)
            {
                //Cast Incarnation: King of the Jungle when you have the Tiger's Fury Buff
                if (MySettings.UseIncarnation && Incarnation.IsSpellUsable && !Incarnation.HaveBuff && TigersFury.HaveBuff)
                {
                    Incarnation.Cast();
                    return;
                }
                //Cast Prowl
                if (MySettings.UseDash && Dash.IsSpellUsable)
                {
                    Dash.Cast();
                    return;
                }
                //Cast Cat Form
                if (MySettings.UseCatForm && CatForm.IsSpellUsable)
                {
                    CatForm.Cast();
                    return;
                }
            }
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    private void Offensive()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            WoWPlayer me = ObjectManager.Me;

            if (MySettings.UseTrinketOne && !ItemsManager.IsItemOnCooldown(_firstTrinket.Entry) && ItemsManager.IsItemUsable(_firstTrinket.Entry))
            {
                ItemsManager.UseItem(_firstTrinket.Name);
                Logging.WriteFight("Use First Trinket Slot");
            }
            if (MySettings.UseTrinketTwo && !ItemsManager.IsItemOnCooldown(_secondTrinket.Entry) && ItemsManager.IsItemUsable(_secondTrinket.Entry))
            {
                ItemsManager.UseItem(_secondTrinket.Name);
                Logging.WriteFight("Use Second Trinket Slot");
            }
            if (MySettings.UseBerserking && Berserking.IsSpellUsable)
            {
                Berserking.Cast();
            }
            //Apply Berserk when you have the Tiger's Fury Buff
            if (MySettings.UseBerserk && Berserk.IsSpellUsable && TigersFury.HaveBuff)
            {
                Berserk.Cast();
            }
            //Apply Elunes Guidance when
            if (MySettings.UseElunesGuidance && ElunesGuidance.IsSpellUsable &&
                //you have no Combo Point.
                me.ComboPoint == 0)
            {
                ElunesGuidance.Cast();
                return;
            }
            //Apply Tiger's Fury when
            if (MySettings.UseTigersFury && TigersFury.IsSpellUsable && (me.EnergyPercentage < 20 ||
                                                                         me.EnergyPercentage < 40 && !me.UnitAura(ClearcastingBuff.Id).IsValid))
            {
                TigersFury.Cast();
                return;
            }
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    private void Rotation()
    {
        Usefuls.SleepGlobalCooldown();
        Diagnostics.Start();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            //Logging
            if (MySettings.LogBuffs)
            {
                string log = "";
                log += ObjectManager.Me.UnitAura(SavageRoarBuff.Id).IsValid ? "Savage Roar is active and has " + ObjectManager.Me.UnitAura(SavageRoarBuff.Id).AuraTimeLeftInMs/1000 + " seconds remaining. " : "";
                log += ObjectManager.Me.UnitAura(TigersFury.Id).IsValid ? "Tiger's Fury is active and has " + ObjectManager.Me.UnitAura(TigersFury.Id).AuraTimeLeftInMs/1000 + " seconds remaining. " : "";
                log += ObjectManager.Me.UnitAura(BloodtalonsBuff.Id).IsValid
                    ? ObjectManager.Me.UnitAura(BloodtalonsBuff.Id).AuraCount + " Bloodtalons Stack active, which have " + ObjectManager.Me.UnitAura(SavageRoarBuff.Id).AuraTimeLeftInMs/1000 + " seconds remaining. "
                    : "";
                log += ObjectManager.Me.UnitAura(PredatorySwiftnessBuff.Id).IsValid
                    ? "Predatory Swiftness is active and has " + ObjectManager.Me.UnitAura(PredatorySwiftnessBuff.Id).AuraTimeLeftInMs/1000 + " seconds remaining. "
                    : "";
                log += ObjectManager.Me.UnitAura(Berserk.Id).IsValid ? "Berserk is active and has " + ObjectManager.Me.UnitAura(Berserk.Id).AuraTimeLeftInMs/1000 + " seconds remaining. " : "";
                log += "Combo points: " + ObjectManager.Me.ComboPoint + "/" + ObjectManager.Me.MaxComboPoint + ", Energy: " + ObjectManager.Me.Energy + "/" + ObjectManager.Me.MaxEnergy + " Mana: " +
                       ObjectManager.Me.Mana + "/" + ObjectManager.Me.MaxMana;
                Logging.WriteFileOnly(log);
            }
            EmptyLoop = false;

            //1. Maintain Savage Roar when you don't have the Buff or
            if (MySettings.UseSavageRoar && (!SavageRoar.HaveBuff ||
                                             //you have max Combo Points and less than 8 seconds remaining.
                                             (ObjectManager.Me.ComboPoint == 5 && ObjectManager.Me.UnitAura(SavageRoarBuff.Id, ObjectManager.Me.Guid).AuraTimeLeftInMs < 8000)))
            {
                if (CastRegrowth() || ObjectManager.Me.Energy < 40)
                    return;
                if (SavageRoar.IsSpellUsable)
                {
                    SavageRoar.Cast();
                    return;
                }
            }
            //2. Cast Ferocious Bite when
            if (MySettings.UseFerociousBite && FerociousBite.IsHostileDistanceGood &&
                //it will refresh Rip and
                (Rip.TargetHaveBuff && (Sabertooth.HaveBuff || ObjectManager.Target.HealthPercent < 25)) &&
                //Rip is active and has less than 8 seconds remaining
                ObjectManager.Target.AuraIsActiveAndExpireInLessThanMs(Rip.Id, 8000))
            {
                if (CastRegrowth() || ObjectManager.Me.Energy < 25)
                    return;
                if (FerociousBite.IsSpellUsable)
                {
                    FerociousBite.Cast();
                    return;
                }
            }
            //3. Maintain Rip when
            if (MySettings.UseRip && Rip.IsHostileDistanceGood &&
                //it isn't on the target or
                ObjectManager.Me.ComboPoint >= 5 && (!Rip.TargetHaveBuffFromMe ||
                                                     //it has less than 8 seconds remaining and
                                                     (ObjectManager.Target.UnitAura(Rip.Ids, ObjectManager.Me.Guid).AuraTimeLeftInMs < 8000 &&
                                                      //you have Blood Talons Buff.
                                                      ObjectManager.Me.UnitAura(BloodtalonsBuff.Id, ObjectManager.Me.Guid).IsValid)))
            {
                if (CastRegrowth() || ObjectManager.Me.Energy < 30)
                    return;
                if (Rip.IsSpellUsable)
                {
                    Rip.Cast();
                    return;
                }
            }
            //4. Cast Ashamane's Frenzy when
            if (MySettings.UseAshamanesFrenzy && AshamanesFrenzy.IsSpellUsable && AshamanesFrenzy.IsHostileDistanceGood &&
                //you have 2 or less Combo Points and
                ObjectManager.Me.ComboPoint <= 2 &&
                //you have the Savage Roar Buff (if talented).
                (!SavageRoar.KnownSpell || ObjectManager.Me.UnitAura(SavageRoarBuff.Id, ObjectManager.Me.Guid).IsValid) &&
                TigersFury.HaveBuff)
            {
                if (CastRegrowth())
                    return;
                //you have the Bloodtalons Buff (if talented) and
                if (!Bloodtalons.HaveBuff || ObjectManager.Me.UnitAura(BloodtalonsBuff.Id, ObjectManager.Me.Guid).IsValid)
                {
                    AshamanesFrenzy.Cast();
                    return;
                }
            }
            //5. Maintain Thrash when
            if (MySettings.UseThrash &&
                //it has less than 5 seconds remaining and
                ObjectManager.Target.UnitAura(106830, ObjectManager.Me.Guid).AuraTimeLeftInMs < 5000 &&
                //there are multiple targets in range.
                ObjectManager.Me.GetUnitInSpellRange(Thrash.MaxRangeHostile) > 1)
            {
                if (ObjectManager.Me.Energy < 50)
                    return;
                if (Thrash.IsSpellUsable)
                {
                    Thrash.Cast();
                    return;
                }
            }
            //6. Maintain Rake when
            if (MySettings.UseRake && Rake.IsHostileDistanceGood &&
                //it isn't on the target or
                (!Rake.TargetHaveBuffFromMe ||
                 //it has less than 5 seconds remaining and
                 (ObjectManager.Target.UnitAura(Rake.Ids, ObjectManager.Me.Guid).AuraTimeLeftInMs < 5000 &&
                  //you have the Blood Talons Buff.
                  ObjectManager.Me.UnitAura(BloodtalonsBuff.Id, ObjectManager.Me.Guid).IsValid)))
            {
                if (ObjectManager.Me.Energy < 35)
                    return;
                if (Rake.IsSpellUsable)
                {
                    Rake.Cast();
                    return;
                }
            }
            //7. Maintain Moonfire when
            if (MySettings.UseMoonfire && Moonfire.IsHostileDistanceGood &&
                //you don't leave Cat Form and
                (LunarInspiration.HaveBuff || !CatForm.HaveBuff) &&
                //it has less than 5 seconds remaining.
                ObjectManager.Target.UnitAura(Moonfire.Ids, ObjectManager.Me.Guid).AuraTimeLeftInMs < 5000)
            {
                if (ObjectManager.Me.Energy < 30)
                    return;
                if (Moonfire.IsSpellUsable)
                {
                    Moonfire.Cast();
                    return;
                }
            }
            //8. Cast Ferocious Bite when
            if (MySettings.UseFerociousBite &&
                FerociousBite.IsHostileDistanceGood && ObjectManager.Me.ComboPoint >= 5 &&
                //your Savage Roar Buff has more than 10 seconds remaining (if talented) and
                (!MySettings.UseSavageRoar || !SavageRoar.KnownSpell || ObjectManager.Target.UnitAura(SavageRoarBuff.Id, ObjectManager.Me.Guid).AuraTimeLeftInMs > 10000) &&
                //your Rip Buff has more than 10 seconds remaining.
                (!MySettings.UseRip || !Rip.KnownSpell || ObjectManager.Target.UnitAura(Rip.Id, ObjectManager.Me.Guid).AuraTimeLeftInMs > 10000))
            {
                if (CastRegrowth() || ObjectManager.Me.Energy < 25)
                    return;
                if (FerociousBite.IsSpellUsable)
                {
                    FerociousBite.Cast();
                    return;
                }
            }
            //9. Cast Brutal Slash when (max Charges OR multiple Enemies)
            if (MySettings.UseBrutalSlash &&
                //you have 3 charges or
                (BrutalSlash.GetSpellCharges == 3 ||
                 //there are 3 or more Targets in range.
                 ObjectManager.GetUnitInSpellRange(BrutalSlash.MaxRangeHostile) >= 3))
            {
                if (ObjectManager.Me.Energy < 20)
                    return;
                if (BrutalSlash.IsSpellUsable)
                {
                    BrutalSlash.Cast();
                    return;
                }
            }
            //Spend Energy on Fillers
            if (ObjectManager.Me.EnergyPercentage > MySettings.PoolEnergy ||
                (MySettings.UseTigersFury && TigersFury.IsSpellUsable))
            {
                //10. Cast Swipe when
                if (MySettings.UseSwipe && Swipe.IsHostileDistanceGood &&
                    //there are 3 or more Targets in range.
                    ObjectManager.GetUnitInSpellRange(BrutalSlash.MaxRangeHostile) >= 3)
                {
                    if (ObjectManager.Me.Energy < 45)
                        return;
                    if (Swipe.IsSpellUsable)
                    {
                        Swipe.Cast();
                        return;
                    }
                }
                //11. Cast Shred
                if (MySettings.UseShred && Shred.IsHostileDistanceGood)
                {
                    if (ObjectManager.Me.Energy < 40)
                        return;
                    if (Shred.IsSpellUsable)
                    {
                        Shred.Cast();
                        return;
                    }
                }
                //12. Cast Moonfire when
                if (MySettings.UseMoonfire && ObjectManager.Me.EnergyPercentage == 100 &&
                    Moonfire.IsHostileDistanceGood &&
                    //you have the Lunar Inspiration Talent or have it forced in the Settings
                    (LunarInspiration.HaveBuff || MySettings.UseMoonfireWihtoutTalent))
                {
                    if (ObjectManager.Me.Energy < 30)
                        return;
                    if (Moonfire.IsSpellUsable)
                    {
                        Moonfire.Cast();
                        return;
                    }
                }
            }
            EmptyLoop = true;
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
            Diagnostics.Stop();
        }
    }

    private bool CastRegrowth()
    {
        //Cast Healing Touch when
        if (MySettings.UseRegrowth && Regrowth.IsSpellUsable &&
            //you have the Predatory Swiftness Buff
            ObjectManager.Me.UnitAura(PredatorySwiftnessBuff.Id, ObjectManager.Me.Guid).IsValid)
        {
            if (ObjectManager.Me.HealthPercent > 90 && Party.IsInGroup())
            {
                var lowestHpPlayer = ObjectManager.Me;
                foreach (UInt128 playerInMyParty in Party.GetPartyPlayersGUID())
                {
                    if (playerInMyParty <= 0)
                        continue;
                    WoWObject obj = ObjectManager.GetObjectByGuid(playerInMyParty);
                    if (!obj.IsValid || obj.Type != WoWObjectType.Player)
                        continue;
                    var currentPlayer = new WoWPlayer(obj.GetBaseAddress);
                    if (!currentPlayer.IsValid || !currentPlayer.IsAlive)
                        continue;
                    if (currentPlayer.HealthPercent < lowestHpPlayer.HealthPercent && CombatClass.InSpellRange(currentPlayer, 0, Regrowth.MaxRangeFriend))
                        lowestHpPlayer = currentPlayer;
                }
                if (lowestHpPlayer.Guid > 0)
                {
                    Logging.WriteFight("Healing " + lowestHpPlayer.Name + "(" + lowestHpPlayer.GetUnitRole + ")");
                    Regrowth.Cast(false, true, false, lowestHpPlayer.GetUnitId());
                    return true;
                }
            }
            Regrowth.CastOnSelf();
            return true;
        }
        return false;
    }

    #region Nested type: DruidFeralSettings

    [Serializable]
    public class DruidFeralSettings : Settings
    {
        /* Professions & Racials */
        public bool UseBerserking = true;
        public bool UseDarkflight = true;
        public int UseWarStompBelowPercentage = 25;

        /* Druid Buffs */
        public bool UseAquaticTravelFormOOC = true;
        public bool UseCatForm = true;
        public bool UseCatFormOOC = true;
        public bool UseSavageRoar = true;

        /* Druid DoTs */
        public bool UseMoonfire = true;
        public bool UseMoonfireWihtoutTalent = false;
        public bool UseRake = true;
        public bool UseRip = true;
        public bool UseThrash = true;

        /* Offensive Spells */
        public bool UseBrutalSlash = true;
        public bool UseFerociousBite = true;
        public bool UseShred = true;
        public bool UseSwipe = true;

        /* Artifact Spells */
        public bool UseAshamanesFrenzy = true;

        /* Offensive Cooldowns */
        public bool UseBerserk = true;
        public bool UseElunesGuidance = true;
        public bool UseIncarnation = true;
        public bool UseTigersFury = true;

        /* Defensive Cooldowns */
        //public bool UseEntanglingRoots = true;
        //public bool UseMaim = true;
        //public bool UseMassEntanglement = true;
        public int UseMightyBashBelowPercentage = 25;
        //public bool UseSkullBash = true;
        public int UseSurvivalInstinctsBelowPercentage = 25;
        //public bool UseTyphoon = true;
        //public bool UseWildCharge = true;

        /* Healing Spells */
        public bool UseRegrowth = true;
        public int UseRestorationAffinityBelowPercentage = 25;

        /* Utility Cooldowns */
        public bool UseDash = true;
        //public bool UseDisplacerBeast = true;
        public bool UseProwl = true;
        public bool UseProwlOOC = false;
        public bool UseStampedingRoar = true;

        /* Game Settings */
        public int PoolEnergy = 0;
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;

        /* Diagnostics */
        public bool LogBuffs = false;
        public bool LogDiagnostics = false;
        public bool LogDiagnosticsEachLoop = false;

        public DruidFeralSettings()
        {
            ConfigWinForm("Druid Feral Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Darkflight", "UseDarkflight", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStompBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            /* Druid Buffs */
            AddControlInWinForm("Use Aquatic Travel Form out of Combat", "UseAquaticTravelFormOOC", "Druid Buffs");
            AddControlInWinForm("Use Cat Form", "UseCatForm", "Druid Buffs");
            AddControlInWinForm("Use Cat Form out of Combat", "UseCatFormOOC", "Druid Buffs");
            AddControlInWinForm("Use Savage Roar", "UseSavageRoar", "Druid Buffs");
            /* Druid DoTs */
            AddControlInWinForm("Use Moonfire", "UseMoonfire", "Druid DoTs");
            AddControlInWinForm("Use Moonfire regardless of Talents", "UseMoonfireWihtoutTalent", "Druid DoTs");
            AddControlInWinForm("Use Rake", "UseRake", "Druid DoTs");
            AddControlInWinForm("Use Rip", "UseRip", "Druid DoTs");
            AddControlInWinForm("Use Thrash", "UseThrash", "Druid DoTs");
            /* Offensive Spells */
            AddControlInWinForm("Use Brutal Slash", "UseBrutalSlash", "Offensive Spells");
            AddControlInWinForm("Use Ferocious Bite", "UseFerociousBite", "Offensive Spells");
            AddControlInWinForm("Use Shred", "UseShred", "Offensive Spells");
            AddControlInWinForm("Use Swipe", "UseSwipe", "Offensive Spells");
            /* Artifact Spells */
            AddControlInWinForm("Use Ashamanes Frenzy", "UseAshamanesFrenzy", "Artifact Spells");
            /* Offensive Cooldowns */
            AddControlInWinForm("Use Berserk", "UseBerserk", "Offensive Cooldowns");
            AddControlInWinForm("Use Elunes Guidance", "UseElunesGuidance", "Offensive Cooldowns");
            AddControlInWinForm("Use Incarnation: King of the Jungle", "UseIncarnation", "Offensive Cooldowns");
            AddControlInWinForm("Use Tiger's Fury", "UseTigersFury", "Offensive Cooldowns");
            /* Defensive Cooldowns */
            AddControlInWinForm("Use Mighty Bash", "UseMightyBashBelowPercentage", "Defensive Cooldowns", "BelowPercentage", "Life");
            AddControlInWinForm("Use SurvivalInstincts", "UseSurvivalInstinctsBelowPercentage", "Defensive Cooldowns", "BelowPercentage", "Life");
            /* Healing Spells */
            AddControlInWinForm("Use Regrowth", "UseRegrowth", "Healing Spells");
            AddControlInWinForm("Use Restoration Affinity", "UseRestorationAffinityBelowPercentage", "Healing Spells", "BelowPercentage", "Life");
            /* Utility Cooldowns */
            AddControlInWinForm("Use Dash", "UseDash", "Utility Cooldowns");
            AddControlInWinForm("Use Prowl", "UseProwl", "Utility Cooldowns");
            AddControlInWinForm("Use Prowl out of Combat", "UseProwlOOC", "Utility Cooldowns");
            AddControlInWinForm("Use Stampeding Roar", "UseStampedingRoar", "Utility Cooldowns");
            /* Game Settings */
            AddControlInWinForm("Use Fillers", "PoolEnergy", "Game Settings", "AbovePercentage", "Energy");
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");
            /* Diagnostics */
            AddControlInWinForm("Log Buffs", "LogBuffs", "Logging");
            AddControlInWinForm("Log Diagnostics", "LogDiagnostics", "Logging");
            AddControlInWinForm("Log Diagnostics each Loop", "LogDiagnosticsEachLoop", "Logging");
        }

        public static DruidFeralSettings CurrentSetting { get; set; }

        public static DruidFeralSettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Druid_Feral.xml";
            if (File.Exists(currentSettingsFile))
            {
                return
                    CurrentSetting = Load<DruidFeralSettings>(currentSettingsFile);
            }
            return new DruidFeralSettings();
        }
    }

    #endregion
}

public class DruidRestoration
{
    private static DruidRestorationSettings MySettings = DruidRestorationSettings.GetSettings();

    #region General Timers & Variables

    private readonly WoWItem _firstTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET);
    private readonly WoWItem _secondTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET, 2);

    private bool CombatMode = true;

    private Timer DefensiveTimer = new Timer(0);

    #endregion

    #region Professions & Racials

    //private readonly Spell ArcaneTorrent = new Spell("Arcane Torrent"); //No GCD
    private readonly Spell Berserking = new Spell("Berserking"); //No GCD
    private readonly Spell BloodFury = new Spell("Blood Fury"); //No GCD
    private readonly Spell Darkflight = new Spell("Darkflight"); //No GCD
    private readonly Spell GiftoftheNaaru = new Spell("Gift of the Naaru"); //No GCD
    private readonly Spell Stoneform = new Spell("Stoneform"); //No GCD
    private readonly Spell WarStomp = new Spell("War Stomp"); //No GCD

    #endregion

    #region Druid Buffs

    private readonly Spell CatForm = new Spell("Cat Form");
    private readonly Spell Flourish = new Spell("Flourish");
    private readonly Spell Incarnation = new Spell("Incarnation: Tree of Life");
    private readonly Spell TravelForm = new Spell("Travel Form");

    #endregion

    #region Offensive Spells

    private readonly Spell Moonfire = new Spell("Moonfire");
    private readonly Spell SolarWrath = new Spell("Solar Wrath");
    private readonly Spell Sunfire = new Spell("Sunfire");

    #endregion

    #region Artifact Spells

    private readonly Spell EssenceofGHanir = new Spell("Essence of G'Hanir");

    #endregion

    #region Defensive Spells

    private readonly Spell Barkskin = new Spell("Barkskin");
    //private readonly Spell EntanglingRoots = new Spell("Entangling Roots");
    private readonly Spell Ironbark = new Spell("Ironbark"); //No GCD
    //private readonly Spell MassEntanglement = new Spell("Mass Entanglement");
    private readonly Spell MightyBash = new Spell("Mighty Bash");
    //private readonly Spell Typhoon = new Spell("Typhoon");
    //private readonly Spell WildCharge = new Spell("Wild Charge"); //No GCD

    #endregion

    #region Healing Spells

    private readonly Spell CenarionWard = new Spell("Cenarion Ward");
    private readonly Spell Efflorescence = new Spell("Efflorescence");
    private Timer EfflorescenceTimer = new Timer(0);
    private readonly Spell FrenziedRegeneration = new Spell("Frenzied Regeneration");
    private readonly Spell HealingTouch = new Spell("Healing Touch");
    private readonly Spell Lifebloom = new Spell("Lifebloom");
    private readonly Spell Regrowth = new Spell("Regrowth");
    private readonly Spell Rejuvenation = new Spell("Rejuvenation");
    private readonly Spell Renewal = new Spell("Renewal"); //No GCD
    private readonly Spell Swiftmend = new Spell("Swiftmend");
    private readonly Spell Tranquility = new Spell("Tranquility");
    private readonly Spell WildGrowth = new Spell("Wild Growth");

    #endregion

    #region Utility Spells

    private readonly Spell Dash = new Spell(1850 /*"Dash"*/); //No GCD
    //private readonly Spell DisplacerBeast = new Spell("Displacer Beast");
    //private readonly Spell Prowl = new Spell("Prowl"); //No GCD

    #endregion

    public DruidRestoration()
    {
        Main.InternalRange = 39f;
        Main.InternalAggroRange = 39f;
        Main.InternalLightHealingSpell = HealingTouch;
        MySettings = DruidRestorationSettings.GetSettings();
        Main.DumpCurrentSettings<DruidRestorationSettings>(MySettings);
        UInt128 lastTarget = 0;

        Diagnostics.Init("Restoration Druid");

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
                            if (ObjectManager.Me.Target != lastTarget)
                            {
                                lastTarget = ObjectManager.Me.Target;
                            }

                            if (Moonfire.IsHostileDistanceGood)
                                Combat();
                            else
                                Patrolling();
                        }
                        else
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
        Diagnostics.Summarize();
    }

    private void Patrolling()
    {
        if (!ObjectManager.Me.IsCast)
        {
            //Log
            if (CombatMode)
            {
                Logging.WriteFight("Patrolling:");
                CombatMode = false;
            }

            if (ObjectManager.Me.GetMove && !Usefuls.PlayerUsingVehicle)
            {
                //Travel Form while swimming
                if (MySettings.UseAquaticTravelFormOOC && Usefuls.IsSwimming && TravelForm.IsSpellUsable)
                {
                    TravelForm.Cast();
                    return;
                }
                //Movement Buffs
                if (!Darkflight.HaveBuff && !Dash.HaveBuff) //they don't stack
                {
                    if (MySettings.UseDarkflight && Darkflight.IsSpellUsable)
                    {
                        Darkflight.Cast();
                        return;
                    }
                    if (MySettings.UseDash && Dash.IsSpellUsable && (MySettings.UseCatFormOOC || CatForm.HaveBuff))
                    {
                        Dash.Cast();
                        return;
                    }
                }
                if (MySettings.UseCatFormOOC && CatForm.IsSpellUsable && !CatForm.HaveBuff)
                {
                    CatForm.Cast();
                    return;
                }
            }
            else
            {
                //Self Heal for Damage Dealer
                if (nManager.Products.Products.ProductName == "Damage Dealer" && Main.InternalLightHealingSpell.IsSpellUsable &&
                    ObjectManager.Me.HealthPercent < 90 && ObjectManager.Target.Guid == ObjectManager.Me.Guid)
                {
                    Main.InternalLightHealingSpell.CastOnSelf();
                    return;
                }
            }
        }
    }

    // For general InFight Behavior (only touch if you want to add a new method like PetManagement())
    private void Combat()
    {
        //Log
        if (!CombatMode)
        {
            Logging.WriteFight("Combat:");
            CombatMode = true;
        }
        Healing();
        if (Defensive() || Offensive())
            return;
        Rotation();
    }

    private bool Healing()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            //Gift of the Naaru
            if (ObjectManager.Me.HealthPercent < MySettings.UseGiftoftheNaaruBelowPercentage &&
                GiftoftheNaaru.IsSpellUsable)
            {
                GiftoftheNaaru.CastOnSelf();
                return true;
            }
            //Renewal 
            if (ObjectManager.Me.HealthPercent < MySettings.UseRenewalBelowPercentage &&
                Renewal.IsSpellUsable)
            {
                Renewal.CastOnSelf();
            }
            //Swiftmend 
            if (ObjectManager.Me.HealthPercent < MySettings.UseSwiftmendBelowPercentage &&
                Swiftmend.IsSpellUsable)
            {
                Swiftmend.CastOnSelf();
            }
            //Cenarion Ward
            if (ObjectManager.Me.HealthPercent < MySettings.UseCenarionWardBelowPercentage &&
                CenarionWard.IsSpellUsable)
            {
                CenarionWard.CastOnSelf();
            }
            //Lifebloom
            if (ObjectManager.Me.HealthPercent < MySettings.UseLifebloomBelowPercentage &&
                Lifebloom.IsSpellUsable && !Lifebloom.HaveBuff)
            {
                Lifebloom.CastOnSelf();
            }
            //Rejuvenation 
            if (ObjectManager.Me.HealthPercent < MySettings.UseRejuvenationBelowPercentage &&
                Rejuvenation.IsSpellUsable && !Rejuvenation.HaveBuff)
            {
                Rejuvenation.CastOnSelf();
            }
            //Healing Touch 
            if (ObjectManager.Me.HealthPercent < MySettings.UseHealingTouchBelowPercentage &&
                HealingTouch.IsSpellUsable)
            {
                HealingTouch.CastOnSelf();
            }
            return false;
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    private bool Defensive()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            //Defensive Cooldowns
            if (ObjectManager.Target.IsStunned)
            {
                //Stun
                if (ObjectManager.Target.IsStunnable)
                {
                    if (MightyBash.IsSpellUsable && ObjectManager.Me.HealthPercent < MySettings.UseMightyBashBelowPercentage)
                    {
                        MightyBash.Cast();
                        return true;
                    }
                    if (WarStomp.IsSpellUsable && ObjectManager.Me.HealthPercent < MySettings.UseWarStompBelowPercentage)
                    {
                        WarStomp.Cast();
                        return true;
                    }
                }
            }
            //Stoneform
            if (ObjectManager.Me.HealthPercent < MySettings.UseStoneformBelowPercentage && Stoneform.IsSpellUsable)
            {
                Stoneform.Cast();
                DefensiveTimer = new Timer(1000*8);
                return true;
            }
            //Barkskin
            if (Barkskin.IsSpellUsable && ObjectManager.Me.HealthPercent < MySettings.UseBarkskinBelowPercentage)
            {
                Barkskin.Cast();
                DefensiveTimer = new Timer(1000*12);
                return true;
            }
            //Ironbark 
            if (ObjectManager.Me.HealthPercent < MySettings.UseIronbarkBelowPercentage &&
                Ironbark.IsSpellUsable && ObjectManager.Me.UnitAura(Ironbark.Id, ObjectManager.Me.Guid).AuraTimeLeftInMs < 4000)
            {
                Ironbark.CastOnSelf();
                DefensiveTimer = new Timer(1000*12);
                return true;
            }
            return false;
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    private bool Offensive()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            if (MySettings.UseTrinketOne && !ItemsManager.IsItemOnCooldown(_firstTrinket.Entry) && ItemsManager.IsItemUsable(_firstTrinket.Entry))
            {
                ItemsManager.UseItem(_firstTrinket.Name);
                Logging.WriteFight("Use First Trinket Slot");
            }
            if (MySettings.UseTrinketTwo && !ItemsManager.IsItemOnCooldown(_secondTrinket.Entry) && ItemsManager.IsItemUsable(_secondTrinket.Entry))
            {
                ItemsManager.UseItem(_secondTrinket.Name);
                Logging.WriteFight("Use Second Trinket Slot");
            }
            if (MySettings.UseBerserking && Berserking.IsSpellUsable)
            {
                Berserking.Cast();
            }
            if (MySettings.UseBloodFury && BloodFury.IsSpellUsable)
            {
                BloodFury.Cast();
            }
            return false;
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    private void Rotation()
    {
        Usefuls.SleepGlobalCooldown();
        Diagnostics.Start();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            //Maintain Moonfire and Sunfire Dots
            if (MySettings.UseMoonfire && Moonfire.IsSpellUsable && Moonfire.IsHostileDistanceGood && !Moonfire.TargetHaveBuff)
            {
                Moonfire.Cast();
                return;
            }
            if (MySettings.UseSunfire && Sunfire.IsSpellUsable && Sunfire.IsHostileDistanceGood && !Sunfire.TargetHaveBuff)
            {
                Sunfire.Cast();
                return;
            }

            //Cast Solar Wrath
            if (MySettings.UseSolarWrath && SolarWrath.IsSpellUsable && SolarWrath.IsHostileDistanceGood)
            {
                SolarWrath.Cast();
                return;
            }
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
            Diagnostics.Stop();
        }
    }

    #region Nested type: DruidRestorationSettings

    [Serializable]
    public class DruidRestorationSettings : Settings
    {
        /* Professions & Racials */
        //public bool UseArcaneTorrent = true;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseDarkflight = true;
        public int UseGiftoftheNaaruBelowPercentage = 50;
        public int UseStoneformBelowPercentage = 50;
        public int UseWarStompBelowPercentage = 50;

        /* Artifact Spells */
        public int UseEssenceofGHanirBelowPercentage = 50;

        /* Offensive Spells */
        public bool UseMoonfire = true;
        public bool UseSolarWrath = true;
        public bool UseSunfire = true;

        /* Defensive Cooldowns */
        public int UseBarkskinBelowPercentage = 40;
        //public bool UseEntanglingRoots = true;
        public int UseIronbarkBelowPercentage = 60;
        //public bool UseMassEntanglement = true;
        public int UseMightyBashBelowPercentage = 25;
        //public bool UseTyphoon = true;

        /* Healing Spells */
        public int UseCenarionWardBelowPercentage = 100;
        public int UseHealingTouchBelowPercentage = 25;
        public int UseRejuvenationBelowPercentage = 80;
        public int UseRenewalBelowPercentage = 40;
        public int UseLifebloomBelowPercentage = 40;
        public int UseSwiftmendBelowPercentage = 40;

        /* Utility Cooldowns */
        public bool UseAquaticTravelFormOOC = true;
        public bool UseCatFormOOC = true;
        public bool UseDash = true;

        /* Game Settings */
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;

        /* Diagnostics */
        public bool LogDiagnostics = false;
        public bool LogDiagnosticsEachLoop = false;

        public DruidRestorationSettings()
        {
            ConfigWinForm("Druid Restoration Settings");
            /* Professions & Racials */
            //AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Darkflight", "UseDarkflight", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaruBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            AddControlInWinForm("Use Stone Form", "UseStoneformBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            AddControlInWinForm("Use War Stomp", "UseWarStompBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            /* Artifact Spells */
            AddControlInWinForm("Use Essence of GHanir", "UseEssenceofGHanirBelowPercentage", "Artifact Spells", "BelowPercentage", "Life");
            /* Offensive Spells */
            AddControlInWinForm("Use Moonfire", "UseMoonfire", "Offensive Spells");
            AddControlInWinForm("Use Solar Wrath", "UseSolarWrath", "Offensive Spells");
            AddControlInWinForm("Use Sunfire", "UseSunfire", "Offensive Spells");
            /* Defensive Cooldowns */
            AddControlInWinForm("Use Barkskin", "UseBarkskinBelowPercentage", "Defensive Cooldowns", "BelowPercentage", "Life");
            AddControlInWinForm("Use Ironbark", "UseIronbarkBelowPercentage", "Defensive Cooldowns", "BelowPercentage", "Life");
            AddControlInWinForm("Use Mighty Bash", "UseMightyBashBelowPercentage", "Defensive Cooldowns", "BelowPercentage", "Life");
            /* Healing Spells */
            AddControlInWinForm("Use Cenarion Ward", "UseCenarionWardBelowPercentage", "Healing Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Healing Touch", "UseHealingTouchBelowPercentage", "Healing Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Rejuvenation", "UseRejuvenationBelowPercentage", "Healing Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Renewal", "UseRenewalBelowPercentage", "Healing Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Lifebloom", "UseLifebloomBelowPercentage", "Healing Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Swiftmend", "UseSwiftmendBelowPercentage", "Healing Spells", "BelowPercentage", "Life");
            /* Utility Spells */
            AddControlInWinForm("Use Aquatic Travel Form out of Combat", "UseAquaticTravelFormOOC", "Utility Spells");
            AddControlInWinForm("Use Cat Form out of Combat", "UseCatFormOOC", "Utility Spells");
            AddControlInWinForm("Use Dash", "UseDash", "Utility Spells");
            /* Game Settings */
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");
            /* Diagnostics */
            AddControlInWinForm("Log Diagnostics", "LogDiagnostics", "Logging");
            AddControlInWinForm("Log Diagnostics each Loop", "LogDiagnosticsEachLoop", "Logging");
        }

        public static DruidRestorationSettings CurrentSetting { get; set; }

        public static DruidRestorationSettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Druid_Restoration.xml";
            if (File.Exists(currentSettingsFile))
            {
                return
                    CurrentSetting = Load<DruidRestorationSettings>(currentSettingsFile);
            }
            return new DruidRestorationSettings();
        }
    }

    #endregion
}

public class DruidGuardian
{
    private static DruidGuardianSettings MySettings = DruidGuardianSettings.GetSettings();

    #region General Timers & Variables

    private readonly WoWItem _firstTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET);
    private readonly WoWItem _secondTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET, 2);

    private bool CombatMode = true;

    private Timer DefensiveTimer = new Timer(0);

    #endregion

    #region Professions & Racials

    private readonly Spell Berserking = new Spell("Berserking"); //No GCD
    private readonly Spell Darkflight = new Spell("Darkflight"); //No GCD
    private readonly Spell WarStomp = new Spell("War Stomp");

    #endregion

    #region Druid Buffs

    private readonly Spell BearForm = new Spell("Bear Form");
    private readonly Spell CatForm = new Spell("Cat Form");
    private readonly Spell TravelForm = new Spell("Travel Form");
    private readonly Spell GalacticGuardianBuff = new Spell(213708);

    #endregion

    #region Offensive Spells

    private readonly Spell Mangle = new Spell("Mangle");
    private readonly Spell Maul = new Spell("Maul");
    private readonly Spell Moonfire = new Spell("Moonfire");
    private readonly Spell Pulverize = new Spell("Pulverize");
    private readonly Spell Swipe = new Spell("Swipe");
    private readonly Spell Thrash = new Spell("Thrash");

    #endregion

    #region Legion Artifact

    private readonly Spell RageoftheSleeper = new Spell(200851);

    #endregion

    #region Offensive Cooldowns

    private readonly Spell BristlingFur = new Spell("Bristling Fur");
    private readonly Spell Incarnation = new Spell("Incarnation: Guardian of Ursoc");
    private readonly Spell LunarBeam = new Spell("Lunar Beam");

    #endregion

    #region Defensive Cooldowns

    private readonly Spell Barkskin = new Spell("Barkskin");
    //private readonly Spell IncapacitatingRoar = new Spell("Incapacitating Roar");
    private readonly Spell Ironfur = new Spell("Ironfur");
    private readonly Spell MarkofUrsol = new Spell("Mark of Ursol");
    //private readonly Spell MassEntanglement = new Spell("Mass Entanglement");
    private readonly Spell MightyBash = new Spell("Mighty Bash");
    //private readonly Spell SkullBash = new Spell("Skull Bash");
    private readonly Spell SurvivalInstincts = new Spell("Survival Instincts");
    //private readonly Spell Typhoon = new Spell("Typhoon");

    #endregion

    #region Healing Spells

    private readonly Spell FrenziedRegeneration = new Spell("Frenzied Regeneration");
    private readonly Spell HealingTouch = new Spell("Healing Touch");
    private readonly Spell Regrowth = new Spell("Regrowth");
    private readonly Spell Rejuvenation = new Spell("Rejuvenation");
    private readonly Spell Swiftmend = new Spell("Swiftmend");

    #endregion

    #region Utility Cooldowns

    private readonly Spell Dash = new Spell(1850 /*"Dash"*/); //No GCD
    //private readonly Spell DisplacerBeast = new Spell("Displacer Beast");
    private readonly Spell Growl = new Spell("Growl");
    private readonly Spell Prowl = new Spell("Prowl"); //No GCD
    private readonly Spell StampedingRoar = new Spell("Stampeding Roar");
    //private readonly Spell WildCharge = new Spell("Wild Charge);

    #endregion

    public DruidGuardian()
    {
        Main.InternalRange = ObjectManager.Me.GetCombatReach;
        Main.InternalAggroRange = Main.InternalRange;
        Main.InternalLightHealingSpell = Regrowth;
        MySettings = DruidGuardianSettings.GetSettings();
        Main.DumpCurrentSettings<DruidGuardianSettings>(MySettings);
        UInt128 lastTarget = 0;

        Diagnostics.Init("Guardian Druid");

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
                            if (ObjectManager.Me.Target != lastTarget)
                            {
                                lastTarget = ObjectManager.Me.Target;
                            }

                            if (Moonfire.IsHostileDistanceGood)
                                Combat();
                            else
                                Patrolling();
                        }
                        else
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
        Diagnostics.Summarize();
    }

    private void Patrolling()
    {
        if (!ObjectManager.Me.IsCast)
        {
            //Log
            if (CombatMode)
            {
                Logging.WriteFight("Patrolling:");
                CombatMode = false;
            }

            if (ObjectManager.Me.GetMove && !Usefuls.PlayerUsingVehicle)
            {
                //Travel Form while swimming
                if (MySettings.UseAquaticTravelFormOOC && Usefuls.IsSwimming && TravelForm.IsSpellUsable)
                {
                    TravelForm.Cast();
                    return;
                }
                //Movement Buffs
                if (!Darkflight.HaveBuff && !Dash.HaveBuff && !StampedingRoar.HaveBuff) //they don't stack
                {
                    if (MySettings.UseDarkflight && Darkflight.IsSpellUsable)
                    {
                        Darkflight.Cast();
                        return;
                    }
                    if (MySettings.UseDash && Dash.IsSpellUsable && (MySettings.UseCatFormOOC || CatForm.HaveBuff))
                    {
                        Dash.Cast();
                        return;
                    }
                    if (MySettings.UseStampedingRoar && StampedingRoar.IsSpellUsable && CatForm.HaveBuff)
                    {
                        StampedingRoar.Cast();
                        return;
                    }
                }
                //Cat Form
                if (MySettings.UseCatFormOOC && CatForm.IsSpellUsable && !CatForm.HaveBuff)
                {
                    CatForm.Cast();
                    return;
                }
            }
            else
            {
                //Self Heal for Damage Dealer
                if (nManager.Products.Products.ProductName == "Damage Dealer" && Main.InternalLightHealingSpell.IsSpellUsable &&
                    ObjectManager.Me.HealthPercent < 90 && ObjectManager.Target.Guid == ObjectManager.Me.Guid)
                {
                    Main.InternalLightHealingSpell.CastOnSelf();
                    return;
                }
            }
        }
    }

    private void Combat()
    {
        //Log
        if (!CombatMode)
        {
            Logging.WriteFight("Combat:");
            CombatMode = true;
        }
        Heal();
        if (Shapeshift() || Defensive() || AgroManagement() || Offensive())
            return;
        Rotation();
    }

    private void Heal()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            //Frenzied Regeneration
            if (FrenziedRegeneration.IsSpellUsable && DefensiveTimer.IsReady && ObjectManager.Me.Rage >= 10 &&
                (ObjectManager.Me.HealthPercent < MySettings.UseFrenziedRegenerationBelowPercentage ||
                 (FrenziedRegeneration.GetSpellCharges == 2 && ObjectManager.Me.HealthPercent < 90)))
            {
                FrenziedRegeneration.CastOnSelf();
            }
            //Restoration Affinity
            if (ObjectManager.Me.HealthPercent < MySettings.UseRestorationAffinityBelowPercentage)
            {
                //Swiftmend
                if (Swiftmend.IsSpellUsable)
                {
                    Swiftmend.CastOnSelf();
                }
                //Rejuvenation
                if (Rejuvenation.IsSpellUsable && ObjectManager.Me.UnitAura(774, ObjectManager.Me.Guid).AuraTimeLeftInMs < 5000)
                {
                    Rejuvenation.CastOnSelf();
                }
            }
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    private bool AgroManagement()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            //Cast Growl when you are in a party and the target of your target is a low health player
            if (MySettings.UseGrowlBelowToTPercentage > 0 && Growl.IsSpellUsable &&
                Growl.IsHostileDistanceGood)
            {
                WoWObject obj = ObjectManager.GetObjectByGuid(ObjectManager.Target.Target);
                if (obj.IsValid && obj.Type == WoWObjectType.Player &&
                    new WoWPlayer(obj.GetBaseAddress).HealthPercent < MySettings.UseGrowlBelowToTPercentage)
                {
                    Growl.Cast();
                    return false;
                }
            }
            return false;
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    private bool Defensive()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            //Defensive Cooldowns
            if (ObjectManager.Target.IsStunned && (DefensiveTimer.IsReady || ObjectManager.Me.HealthPercent < 20))
            {
                //Stun
                if (ObjectManager.Target.IsStunnable)
                {
                    if (MightyBash.IsSpellUsable && ObjectManager.Me.HealthPercent < MySettings.UseMightyBashBelowPercentage)
                    {
                        MightyBash.Cast();
                        return true;
                    }
                    if (WarStomp.IsSpellUsable && ObjectManager.Me.HealthPercent < MySettings.UseWarStompBelowPercentage)
                    {
                        WarStomp.Cast();
                    }
                }
                //Mitigate Damage
                if (SurvivalInstincts.IsSpellUsable && (SurvivalInstincts.GetSpellCharges == 2 ||
                                                        ObjectManager.Me.HealthPercent < MySettings.UseSurvivalInstinctsBelowPercentage))
                {
                    SurvivalInstincts.Cast();
                    DefensiveTimer = new Timer(1000*6);
                }
                if (ObjectManager.Me.HealthPercent < MySettings.UseRageoftheSleeperBelowPercentage && RageoftheSleeper.IsSpellUsable)
                {
                    RageoftheSleeper.Cast();
                    DefensiveTimer = new Timer(1000*10);
                }
                if (ObjectManager.Me.HealthPercent < MySettings.UseBarkskinBelowPercentage && Barkskin.IsSpellUsable)
                {
                    Barkskin.Cast();
                    DefensiveTimer = new Timer(1000*12);
                }
            }
            //Mitigate Magic Damage for Rage
            if (ObjectManager.Me.HealthPercent < MySettings.UseMarkofUrsolBelowPercentage &&
                MarkofUrsol.IsSpellUsable && !MarkofUrsol.HaveBuff)
            {
                MarkofUrsol.Cast();
            }
            //Increase Armor for Rage
            if ((ObjectManager.Me.HealthPercent < MySettings.UseIronfurBelowHealthPercentage ||
                 ObjectManager.Me.Rage > MySettings.UseIronfurAboveRagePercentage) &&
                Ironfur.IsSpellUsable)
            {
                Ironfur.Cast();
            }
            return false;
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    private bool Shapeshift()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            //Change Form
            if (MySettings.UseIncarnation && Incarnation.IsSpellUsable && !Incarnation.HaveBuff)
            {
                Incarnation.Cast();
                return true;
            }
            if (MySettings.UseBearForm && /*BearForm.IsSpellUsable && */ !BearForm.HaveBuff) //IsSpellUsable sometimes false for no reason
            {
                BearForm.Cast();
                return true;
            }

            return false;
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    private bool Offensive()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            //Burst Buffs
            if (MySettings.UseTrinketOne && !ItemsManager.IsItemOnCooldown(_firstTrinket.Entry) && ItemsManager.IsItemUsable(_firstTrinket.Entry))
            {
                ItemsManager.UseItem(_firstTrinket.Name);
                Logging.WriteFight("Use First Trinket Slot");
            }
            if (MySettings.UseTrinketTwo && !ItemsManager.IsItemOnCooldown(_secondTrinket.Entry) && ItemsManager.IsItemUsable(_secondTrinket.Entry))
            {
                ItemsManager.UseItem(_secondTrinket.Name);
                Logging.WriteFight("Use Second Trinket Slot");
            }
            if (MySettings.UseBerserking && Berserking.IsSpellUsable)
            {
                Berserking.Cast();
            }
            if (MySettings.UseBristlingFur && BristlingFur.IsSpellUsable && !DefensiveTimer.IsReady)
            {
                BristlingFur.Cast();
                DefensiveTimer = new Timer(1000*8);
            }
            return false;
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    private void Rotation()
    {
        Usefuls.SleepGlobalCooldown();
        Diagnostics.Start();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            //Use Moonfire if you have a Galactic Guardian proc (if you have chosen this talent).
            if (MySettings.UseMoonfire && Moonfire.IsSpellUsable && Moonfire.IsHostileDistanceGood &&
                ObjectManager.Me.UnitAura(GalacticGuardianBuff.Id).IsValid)
            {
                Moonfire.Cast();
                return;
            }
            //Use Thrash on cooldown when you have multiple targets.
            if (MySettings.UseThrash && Thrash.IsSpellUsable && ObjectManager.Me.GetUnitInSpellRange(8f) > 1)
            {
                Thrash.Cast();
                return;
            }
            //Use Mangle on cooldown.
            if (MySettings.UseMangle && Mangle.IsSpellUsable && Mangle.IsHostileDistanceGood)
            {
                Mangle.Cast();
                return;
            }
            //Use Thrash on cooldown.
            if (MySettings.UseThrash && Thrash.IsSpellUsable && Thrash.IsHostileDistanceGood)
            {
                Thrash.Cast();
                return;
            }
            //Maintain Moonfire on the target.
            if (MySettings.UseMoonfire && Moonfire.IsSpellUsable && Moonfire.IsHostileDistanceGood &&
                ObjectManager.Target.UnitAura(Moonfire.Ids, ObjectManager.Me.Guid).AuraTimeLeftInMs < 5000)
            {
                Moonfire.Cast();
                return;
            }
            //Cast Pulverize when possible.
            if (MySettings.UsePulverize && Thrash.TargetBuffStack >= 2 &&
                Pulverize.IsSpellUsable && Pulverize.IsHostileDistanceGood)
            {
                Pulverize.Cast();
                return;
            }
            //Use Swipe.
            if (MySettings.UseSwipe && Swipe.IsSpellUsable && Swipe.IsHostileDistanceGood)
            {
                Swipe.Cast();
                return;
            }
            //Use Maul when you are close to maximum Rage.
            if (MySettings.UseMaul && Maul.IsSpellUsable &&
                ObjectManager.Me.RagePercentage > 90 && Maul.IsHostileDistanceGood)
            {
                Maul.Cast();
                return;
            }
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
            Diagnostics.Stop();
        }
    }

    #region Nested type: DruidGuardianSettings

    [Serializable]
    public class DruidGuardianSettings : Settings
    {
        /* Professions & Racials */
        public bool UseBerserking = true;
        public bool UseDarkflight = true;
        public int UseWarStompBelowPercentage = 25;

        /* Druid Buffs */
        public bool UseAquaticTravelFormOOC = true;
        public bool UseBearForm = true;
        public bool UseCatFormOOC = true;

        /* Offensive Spells */
        public bool UseMangle = true;
        public bool UseMaul = true;
        public bool UseMoonfire = true;
        public bool UsePulverize = true;
        public bool UseSwipe = true;
        public bool UseThrash = true;

        /* Artifact Spells */
        public int UseRageoftheSleeperBelowPercentage = 60;

        /* Offensive Cooldowns */
        public bool UseBristlingFur = true;
        public bool UseIncarnation = true;
        public bool UseLunarBeam = true;

        /* Defensive Cooldowns */
        public int UseBarkskinBelowPercentage = 60;
        public int UseIronfurBelowHealthPercentage = 60;
        public int UseIronfurAboveRagePercentage = 80;
        public int UseMarkofUrsolBelowPercentage = 0;
        //public bool UseEntanglingRoots = true;
        //public bool UseMassEntanglement = true;
        public int UseMightyBashBelowPercentage = 25;
        //public bool UseSkullBash = true;
        public int UseSurvivalInstinctsBelowPercentage = 50;
        //public bool UseTyphoon = true;
        //public bool UseWildCharge = true;

        /* Healing Spells */
        public int UseFrenziedRegenerationBelowPercentage = 25;
        public int UseRestorationAffinityBelowPercentage = 10;

        /* Utility Cooldowns */
        public bool UseDash = true;
        //public bool UseDisplacerBeast = true;
        public int UseGrowlBelowToTPercentage = 20;
        public bool UseStampedingRoar = true;

        /* Game Settings */
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;

        /* Diagnostics */
        public bool LogDiagnostics = false;
        public bool LogDiagnosticsEachLoop = false;

        public DruidGuardianSettings()
        {
            ConfigWinForm("Druid Guardian Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Darkflight", "UseDarkflight", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStompBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            /* Druid Buffs */
            AddControlInWinForm("Use Aquatic Travel Form out of Combat", "UseAquaticTravelFormOOC", "Druid Buffs");
            AddControlInWinForm("Use Bear Form", "UseBearForm", "Druid Buffs");
            AddControlInWinForm("Use Cat Form out of Combat", "UseCatFormOOC", "Druid Buffs");
            /* Offensive Spells */
            AddControlInWinForm("Use Mangle", "UseMangle", "Offensive Spells");
            AddControlInWinForm("Use Maul", "UseMaul", "Offensive Spells");
            AddControlInWinForm("Use Moonfire", "UseMoonfire", "Offensive Spells");
            AddControlInWinForm("Use Pulverize", "UsePulverize", "Offensive Spells");
            AddControlInWinForm("Use Swipe", "UseSwipe", "Offensive Spells");
            AddControlInWinForm("Use Thrash", "UseThrash", "Offensive Spells");
            /* Artifact Spells */
            AddControlInWinForm("Use Rage of the Sleeper", "UseRageoftheSleeperBelowPercentage", "Artifact Spells", "BelowPercentage", "Life");
            /* Offensive Cooldowns */
            AddControlInWinForm("Use Bristling Fur", "UseBristlingFur", "Offensive Cooldowns");
            AddControlInWinForm("Use Incarnation", "UseIncarnation", "Offensive Cooldowns");
            AddControlInWinForm("Use Lunar Beam", "UseLunarBeam", "Offensive Cooldowns");
            /* Defensive Cooldowns */
            AddControlInWinForm("Use Barkskin", "UseBarkskinBelowPercentage", "Defensive Cooldowns", "BelowPercentage", "Life");
            AddControlInWinForm("Use Ironfur", "UseIronfurBelowHealthPercentage", "Defensive Cooldowns", "BelowPercentage", "Life");
            AddControlInWinForm("Use Ironfur", "UseIronfurAboveRagePercentage", "Defensive Cooldowns", "AbovePercentage", "Rage");
            AddControlInWinForm("Use Mark of Ursol", "UseMarkofUrsolBelowPercentage", "Defensive Cooldowns", "BelowPercentage", "Life");
            AddControlInWinForm("Use Mighty Bash", "UseMightyBashBelowPercentage", "Defensive Cooldowns", "BelowPercentage", "Life");
            AddControlInWinForm("Use SurvivalInstincts", "UseSurvivalInstinctsBelowPercentage", "Defensive Cooldowns", "BelowPercentage", "Life");
            /* Healing Spells */
            AddControlInWinForm("Use Frenzied Regeneration", "UseFrenziedRegenerationBelowPercentage", "Healing Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Restoration Affinity", "UseRestorationAffinityBelowPercentage", "Healing Spells", "BelowPercentage", "Life");
            /* Utility Cooldowns */
            AddControlInWinForm("Use Dash", "UseDash", "Utility Cooldowns");
            AddControlInWinForm("Use Growl", "UseGrowlBelowToTPercentage", "Utility Spells", "BelowPercentage", "Target of Target Life");
            AddControlInWinForm("Use Stampeding Roar", "UseStampedingRoar", "Utility Cooldowns");
            /* Game Settings */
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");
            /* Diagnostics */
            AddControlInWinForm("Log Diagnostics", "LogDiagnostics", "Logging");
            AddControlInWinForm("Log Diagnostics each Loop", "LogDiagnosticsEachLoop", "Logging");
        }

        public static DruidGuardianSettings CurrentSetting { get; set; }

        public static DruidGuardianSettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Druid_Guardian.xml";
            if (File.Exists(currentSettingsFile))
            {
                return
                    CurrentSetting = Load<DruidGuardianSettings>(currentSettingsFile);
            }
            return new DruidGuardianSettings();
        }
    }

    #endregion
}

#endregion

// ReSharper restore ObjectCreationAsStatement
// ReSharper restore EmptyGeneralCatchClause