/*
* CombatClass for TheNoobBot
* Credit : Vesper, Neo2003, Dreadlocks, Ryuichiro
* Thanks you !
*/

using System;
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

// ReSharper disable EmptyGeneralCatchClause
// ReSharper disable ObjectCreationAsStatement

public class Main : ICombatClass
{
    internal static float InternalRange = 5.0f;
    internal static float InternalAggroRange = 5.0f;
    internal static bool InternalLoop = true;
    internal static Spell InternalLightHealingSpell;
    internal static float Version = 0.51f;

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

#region Druid

public class DruidBalance
{
    private static DruidBalanceSettings MySettings = DruidBalanceSettings.GetSettings();

    #region General Timers & Variables

    private readonly WoWItem _firstTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET);
    private readonly WoWItem _secondTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET, 2);

    private bool CombatMode = true;

    private Timer StunTimer = new Timer(0);

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
    private readonly Spell MoonkinForm = new Spell("Moonkin Form");
    private readonly Spell TravelForm = new Spell("Travel Form");
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
        Main.InternalLightHealingSpell = HealingTouch;
        MySettings = DruidBalanceSettings.GetSettings();
        Main.DumpCurrentSettings<DruidBalanceSettings>(MySettings);
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
            //Healing Touch
            if (HealingTouch.IsSpellUsable && ObjectManager.Me.HealthPercent < MySettings.UseHealingTouchBelowPercentage)
            {
                HealingTouch.CastOnSelf();
                return;
            }

            //outside Moonkin Form
            if (!MoonkinForm.HaveBuff)
            {
                //Rejuvenation
                if (Rejuvenation.IsSpellUsable && ObjectManager.Me.HealthPercent < MySettings.UseRejuvenationBelowPercentage &&
                    ObjectManager.Me.UnitAura(774, ObjectManager.Me.Guid).AuraTimeLeftInMs < 5000)
                {
                    Rejuvenation.CastOnSelf();
                    return;
                }
                //Regrowth
                if (Regrowth.IsSpellUsable && ObjectManager.Me.HealthPercent < MySettings.UseRegrowthBelowPercentage &&
                    !Regrowth.HaveBuff)
                {
                    Regrowth.CastOnSelf();
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
            if (StunTimer.IsReady)
            {
                if (ObjectManager.Target.IsStunnable)
                {
                    if (MightyBash.IsSpellUsable && ObjectManager.Me.HealthPercent < MySettings.UseMightyBashBelowPercentage)
                    {
                        MightyBash.Cast();
                        StunTimer = new Timer(1000*5);
                        return true;
                    }
                    if (WarStomp.IsSpellUsable && ObjectManager.Me.HealthPercent < MySettings.UseWarStompBelowPercentage)
                    {
                        WarStomp.Cast();
                        StunTimer = new Timer(1000*2);
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

            //Change Form
            if (MySettings.UseIncarnation && Incarnation.IsSpellUsable && !Incarnation.HaveBuff)
            {
                Incarnation.Cast();
                return true;
            }
            //Moonkin Form
            if (MySettings.UseMoonkinForm && MoonkinForm.IsSpellUsable && !MoonkinForm.HaveBuff)
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
            if (MySettings.UseAstralCommunion && AstralCommunion.IsSpellUsable && ObjectManager.Me.Eclipse < 25) // AstralPower, LunarPower, Eclipse
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

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            //Pool Astral Power and New Moon charges for Fury of Elune
            if (MySettings.UseFuryofElune && FuryofElune.IsSpellUsable && ObjectManager.Target.GetUnitInSpellRange(3f) >= 2)
            {
                if (ObjectManager.Me.Eclipse == 100 && NewMoon.GetSpellCharges == 3)
                {
                    FuryofElune.CastAtPosition(ObjectManager.Target.Position);
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
                         (LunarEmpowerment.TargetBuffStack < 3 || SolarEmpowerment.TargetBuffStack < 3 || ObjectManager.Me.ManaPercentage == 100))
                {
                    Starsurge.Cast();
                    return;
                }

                //Don't cap on New Moon charges
                if (MySettings.UseNewMoon && NewMoon.IsSpellUsable && NewMoon.GetSpellCharges > 1)
                {
                    NewMoon.Cast();
                    return;
                }
            }


            //Keep Stellar Flare, Moonfire and Sunfire up
            if (MySettings.UseStellarFlare && StellarFlare.IsSpellUsable && StellarFlare.IsHostileDistanceGood && !StellarFlare.TargetHaveBuff)
            {
                StellarFlare.Cast();
                return;
            }
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

            //Consume Lunar & Solar Empowerment && Fill with Lunar Strike on 2+ targets
            if (MySettings.UseSolarWrath && SolarWrath.IsSpellUsable && SolarWrath.IsHostileDistanceGood && SolarEmpowerment.TargetBuffStack == 3)
            {
                SolarWrath.Cast();
                return;
            }
            if (MySettings.UseLunarStrike && LunarStrike.IsSpellUsable && LunarStrike.IsHostileDistanceGood &&
                (ObjectManager.GetUnitInSpellRange(8f) >= 2 || LunarEmpowerment.TargetBuffStack == 3))
            {
                if (MySettings.UseWarriorofElune && WarriorofElune.IsSpellUsable && LunarEmpowerment.TargetBuffStack >= 2)
                {
                    WarriorofElune.Cast();
                    LunarStrike.Cast();
                    Others.SafeSleep(1500 + Usefuls.Latency);
                }
                LunarStrike.Cast();
                return;
            }

            //Fill with Solar Wrath
            if (MySettings.UseSolarWrath && SolarWrath.IsSpellUsable && SolarWrath.IsHostileDistanceGood)
            {
                SolarWrath.Cast();
                return;
            }
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
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
        public bool UseFuryofElune = false;
        public bool UseWarriorofElune = true;
        public bool UseIncarnation = true;
        public bool UseAstralCommunion = true;

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
            //AddControlInWinForm("Use Fury of Elune", "UseFuryofElune", "Offensive Cooldowns");//Not properly implemented
            AddControlInWinForm("Use Warrior of Elune", "UseWarriorofElune", "Offensive Cooldowns");
            AddControlInWinForm("Use Incarnation: Chosen of Elune", "UseIncarnation", "Offensive Cooldowns");
            AddControlInWinForm("Use Astral Communion", "UseAstralCommunion", "Offensive Cooldowns");
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

    private Timer StunTimer = new Timer(0);

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

    private readonly Spell BrutalSlash = new Spell("Brutal Slash");
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

    private readonly Spell HealingTouch = new Spell("Healing Touch");
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
        Main.InternalLightHealingSpell = HealingTouch;
        MySettings = DruidFeralSettings.GetSettings();
        Main.DumpCurrentSettings<DruidFeralSettings>(MySettings);
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
        if (Prowl.HaveBuff)
            Stealth();
        else
        {
            Healing();
            if (Defensive() || Shapeshift() || Offensive())
                return;
            Rotation();
        }
    }

    private void Stealth()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            //1. Priority: Rake
            if (MySettings.UseRake && Rake.IsSpellUsable && Rake.IsHostileDistanceGood && ObjectManager.Target.IsStunnable)
            {
                Logging.WriteDebug("Rake IsHostileDistanceGood: " + Rake.IsHostileDistanceGood + ", Target Distance: " + ObjectManager.Target.GetDistance + ", CombatReach - Me: " + ObjectManager.Me.GetCombatReach +
                                   ", Target: " + ObjectManager.Target.GetCombatReach + ", Rake MaxRangeHostile: " + Rake.MaxRangeHostile + " (Tooltip: Melee)");
                Rake.Cast();
                return;
            }
            //2. Priority: Shred
            if (MySettings.UseShred && Shred.IsSpellUsable && Shred.IsHostileDistanceGood)
            {
                Shred.Cast();
                return;
            }
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    private bool Healing()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            //Cast Healing Touch when it would expire before you could use it or
            if (ObjectManager.Me.UnitAura(PredatorySwiftnessBuff.Id).AuraTimeLeftInMs < 2000 ||
                //you have less than 10% Health
                ObjectManager.Me.Health < 10)
            {
                if (CastHealingTouch())
                    return true;
            }
            //Renewal
            if (Renewal.IsSpellUsable && ObjectManager.Me.HealthPercent < 70)
            {
                Renewal.CastOnSelf();
                return false;
            }
            //Restoration Affinity
            if (ObjectManager.Me.HealthPercent < MySettings.UseRestorationAffinityBelowPercentage)
            {
                //Swiftmend
                if (Swiftmend.IsSpellUsable)
                {
                    Swiftmend.CastOnSelf();
                    return true;
                }
                //Rejuvenation
                if (Rejuvenation.IsSpellUsable && ObjectManager.Me.UnitAura(774, ObjectManager.Me.Guid).AuraTimeLeftInMs < 5000)
                {
                    Rejuvenation.CastOnSelf();
                    return true;
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
            if (StunTimer.IsReady)
            {
                //Stun
                if (ObjectManager.Target.IsStunnable)
                {
                    if (MightyBash.IsSpellUsable && ObjectManager.Me.HealthPercent < MySettings.UseMightyBashBelowPercentage)
                    {
                        MightyBash.Cast();
                        StunTimer = new Timer(1000*5);
                        return true;
                    }
                    if (WarStomp.IsSpellUsable && ObjectManager.Me.HealthPercent < MySettings.UseWarStompBelowPercentage)
                    {
                        WarStomp.Cast();
                        StunTimer = new Timer(1000*2);
                    }
                }
                //Mitigate Damage
                if (SurvivalInstincts.IsSpellUsable && (SurvivalInstincts.GetSpellCharges == 2 ||
                                                        ObjectManager.Me.HealthPercent < MySettings.UseSurvivalInstinctsBelowPercentage))
                {
                    SurvivalInstincts.Cast();
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

            //Cast Incarnation: King of the Jungle when you have the Tiger's Fury Buff
            if (MySettings.UseIncarnation && Incarnation.IsSpellUsable && !Incarnation.HaveBuff && TigersFury.HaveBuff)
            {
                Incarnation.Cast();
                return true;
            }
            //Cast Prowl
            if (MySettings.UseProwl && Prowl.IsSpellUsable && !Prowl.HaveBuff)
            {
                Prowl.Cast();
                return true;
            }
            //Cast Cat Form
            if (MySettings.UseCatForm && CatForm.IsSpellUsable && !CatForm.HaveBuff)
            {
                CatForm.Cast();
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
            //Apply Berserk when you have the Tiger's Fury Buff
            if (MySettings.UseBerserk && Berserk.IsSpellUsable && TigersFury.HaveBuff)
            {
                Berserk.Cast();
            }
            //Apply Elunes Guidance when
            if (MySettings.UseElunesGuidance && ElunesGuidance.IsSpellUsable &&
                //you have no Combo Point.
                ObjectManager.Me.ComboPoint == 0)
            {
                ElunesGuidance.Cast();
                return true;
            }
            //Apply Tiger's Fury when
            if (MySettings.UseTigersFury && TigersFury.IsSpellUsable &&
                //no Buff uptime will be lost and you have less than 30 Energy or
                ((ObjectManager.Me.UnitAura(TigersFury.Ids, ObjectManager.Me.Guid).AuraTimeLeftInMs < 2333 &&
                  ObjectManager.Me.Energy < 30) ||
                 //you can use the Predator Talent
                 (Predator.HaveBuff && ObjectManager.Target.HealthPercent < 20 && ObjectManager.Target.IsAlive &&
                  (Rake.HaveBuff || Rip.HaveBuff || AshamanesFrenzy.HaveBuff))))
            {
                TigersFury.Cast();
                return true;
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

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            //Logging
            if ((SavageRoar.KnownSpell && !ObjectManager.Me.UnitAura(SavageRoarBuff.Id, ObjectManager.Me.Guid).IsValid) ||
                (Bloodtalons.HaveBuff && ObjectManager.Me.UnitAura(BloodtalonsBuff.Id, ObjectManager.Me.Guid).IsValid) ||
                ObjectManager.Me.ComboPoint >= 5)
            {
                string log = "";
                log += (SavageRoar.KnownSpell && !ObjectManager.Me.UnitAura(SavageRoarBuff.Id, ObjectManager.Me.Guid).IsValid) ? "Savage Roar isn't active! " : "";
                log += (Bloodtalons.HaveBuff && ObjectManager.Me.UnitAura(BloodtalonsBuff.Id, ObjectManager.Me.Guid).IsValid) ? BloodtalonsBuff.BuffStack + " Bloodtalons Stack active! " : "";
                log += "Combo points: " + ObjectManager.Me.ComboPoint;
                Logging.Write(log);
            }

            //1. Cast Ferocious Bite when
            if (MySettings.UseFerociousBite && FerociousBite.IsSpellUsable && FerociousBite.IsHostileDistanceGood &&
                //it will refresh Rip and
                (Rip.TargetHaveBuff && (Sabertooth.HaveBuff || ObjectManager.Target.HealthPercent < 25)) &&
                //Rip is active and has less than 8 seconds remaining
                ObjectManager.Target.AuraIsActiveAndExpireInLessThanMs(Rip.Id, 8000))
            {
                if (CastHealingTouch() || ObjectManager.Me.Energy < 25)
                    return;
                FerociousBite.Cast();
                return;
            }
            //2. Maintain Savage Roar when you don't have the Buff or
            if (MySettings.UseSavageRoar && SavageRoar.IsSpellUsable && (!SavageRoar.HaveBuff ||
                                                                         //you have max Combo Points and less than 8 seconds remaining.
                                                                         (ObjectManager.Me.ComboPoint == 5 && ObjectManager.Me.UnitAura(SavageRoarBuff.Id, ObjectManager.Me.Guid).AuraTimeLeftInMs < 8000)))
            {
                if (CastHealingTouch() || ObjectManager.Me.Energy < 40)
                    return;
                SavageRoar.Cast();
                return;
            }
            //3. Cast Ashamane's Frenzy when
            if (MySettings.UseAshamanesFrenzy && AshamanesFrenzy.IsSpellUsable && AshamanesFrenzy.IsHostileDistanceGood &&
                //you have 2 or less Combo Points and
                ObjectManager.Me.ComboPoint <= 2 &&
                //you have the Savage Roar Buff (if talented).
                (!SavageRoar.KnownSpell || ObjectManager.Me.UnitAura(SavageRoarBuff.Id, ObjectManager.Me.Guid).IsValid))
            {
                if (CastHealingTouch())
                    return;
                //you have the Bloodtalons Buff (if talented) and
                if ((!Bloodtalons.HaveBuff || ObjectManager.Me.UnitAura(BloodtalonsBuff.Id, ObjectManager.Me.Guid).IsValid))
                {
                    //Apply Tigers Fury when
                    if (MySettings.UseTigersFury && TigersFury.IsSpellUsable &&
                        //no Buff uptime will be lost.
                        ObjectManager.Me.UnitAura(TigersFury.Ids, ObjectManager.Me.Guid).AuraTimeLeftInMs < 2333)
                    {
                        TigersFury.Cast();
                    }
                    AshamanesFrenzy.Cast();
                    return;
                }
            }
            //4. Maintain Thrash when
            if (MySettings.UseThrash && Thrash.IsSpellUsable &&
                //it has less than 5 seconds remaining and
                ObjectManager.Target.UnitAura(106830, ObjectManager.Me.Guid).AuraTimeLeftInMs < 5000 &&
                //there are multiple targets in range.
                ObjectManager.Me.GetUnitInSpellRange(Thrash.MaxRangeHostile) > 1)
            {
                if (ObjectManager.Me.Energy < 50)
                    return;
                Thrash.Cast();
                return;
            }
            //5. Maintain Rake when
            if (MySettings.UseRake && Rake.IsSpellUsable && Rake.IsHostileDistanceGood &&
                //it isn't on the target or
                (!Rake.TargetHaveBuffFromMe ||
                 //it has less than 5 seconds remaining and
                 (ObjectManager.Target.UnitAura(Rake.Ids, ObjectManager.Me.Guid).AuraTimeLeftInMs < 5000 &&
                  //you have the Blood Talons Buff.
                  ObjectManager.Me.UnitAura(BloodtalonsBuff.Id, ObjectManager.Me.Guid).IsValid)))
            {
                if (ObjectManager.Me.Energy < 35)
                    return;
                Rake.Cast();
                return;
            }
            //6. Maintain Moonfire when
            if (MySettings.UseMoonfire && Moonfire.IsSpellUsable && Moonfire.IsHostileDistanceGood &&
                //you don't leave Cat Form and
                (LunarInspiration.HaveBuff || !CatForm.HaveBuff) &&
                //it has less than 5 seconds remaining.
                ObjectManager.Target.UnitAura(Moonfire.Ids, ObjectManager.Me.Guid).AuraTimeLeftInMs < 5000)
            {
                if (ObjectManager.Me.Energy < 30)
                    return;
                Moonfire.Cast();
                return;
            }
            //Spend Combo Points on Finishers
            if (ObjectManager.Me.ComboPoint >= 5)
            {
                //7. Maintain Rip when
                if (MySettings.UseRip && Rip.IsSpellUsable && Rip.IsHostileDistanceGood &&
                    //it isn't on the target or
                    (!Rip.TargetHaveBuffFromMe ||
                     //it has less than 8 seconds remaining and
                     (ObjectManager.Target.UnitAura(Rip.Ids, ObjectManager.Me.Guid).AuraTimeLeftInMs < 8000 &&
                      //you have Blood Talons Buff.
                      ObjectManager.Me.UnitAura(BloodtalonsBuff.Id, ObjectManager.Me.Guid).IsValid)))
                {
                    if (CastHealingTouch() || ObjectManager.Me.Energy < 30)
                        return;
                    Rip.Cast();
                    return;
                }
                //8. Cast Ferocious Bite when
                if (MySettings.UseFerociousBite && FerociousBite.IsSpellUsable && FerociousBite.IsHostileDistanceGood &&
                    //your Savage Roar Buff has more than 10 seconds remaining (if talented) and
                    (!MySettings.UseSavageRoar || !SavageRoar.KnownSpell || ObjectManager.Target.UnitAura(SavageRoarBuff.Id, ObjectManager.Me.Guid).AuraTimeLeftInMs > 10000) &&
                    //your Rip Buff has more than 10 seconds remaining.
                    (!MySettings.UseRip || !Rip.KnownSpell || ObjectManager.Target.UnitAura(Rip.Id, ObjectManager.Me.Guid).AuraTimeLeftInMs > 10000))
                {
                    if (CastHealingTouch() || ObjectManager.Me.Energy < 25)
                        return;
                    FerociousBite.Cast();
                    return;
                }
            }
            //9. Cast Brutal Slash when (max Charges OR multiple Enemies)
            if (MySettings.UseBrutalSlash && BrutalSlash.IsSpellUsable &&
                //you have 3 charges or
                (BrutalSlash.GetSpellCharges == 3 ||
                 //there are 3 or more Targets in range.
                 ObjectManager.GetUnitInSpellRange(BrutalSlash.MaxRangeHostile) >= 3))
            {
                if (ObjectManager.Me.Energy < 20)
                    return;
                BrutalSlash.Cast();
                return;
            }
            //Spend Energy on Fillers
            if (ObjectManager.Me.EnergyPercentage > MySettings.PoolEnergy ||
                (MySettings.UseTigersFury && TigersFury.IsSpellUsable))
            {
                //10. Cast Swipe when
                if (MySettings.UseSwipe && Swipe.IsSpellUsable && Swipe.IsHostileDistanceGood &&
                    //there are 3 or more Targets in range.
                    ObjectManager.GetUnitInSpellRange(BrutalSlash.MaxRangeHostile) >= 3)
                {
                    if (ObjectManager.Me.Energy < 45)
                        return;
                    Swipe.Cast();
                    return;
                }
                //11. Cast Shred
                if (MySettings.UseShred && Shred.IsSpellUsable && Shred.IsHostileDistanceGood)
                {
                    if (ObjectManager.Me.Energy < 40)
                        return;
                    Shred.Cast();
                    return;
                }
                //12. Cast Moonfire when
                if (MySettings.UseMoonfire && Moonfire.IsSpellUsable && Moonfire.IsHostileDistanceGood &&
                    //you have the Lunar Inspiration Talent or have it forced in the Settings
                    (LunarInspiration.HaveBuff || MySettings.UseMoonfireWihtoutTalent))
                {
                    if (ObjectManager.Me.Energy < 30)
                        return;
                    Moonfire.Cast();
                    return;
                }
            }
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    private bool CastHealingTouch()
    {
        //Cast Healing Touch when
        if (MySettings.UseHealingTouch && HealingTouch.IsSpellUsable &&
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
                    if (currentPlayer.HealthPercent < lowestHpPlayer.HealthPercent && CombatClass.InSpellRange(currentPlayer, 0, HealingTouch.MaxRangeFriend))
                        lowestHpPlayer = currentPlayer;
                }
                if (lowestHpPlayer.Guid > 0)
                {
                    Logging.WriteFight("Healing " + lowestHpPlayer.Name);
                    HealingTouch.Cast(false, true, false, lowestHpPlayer.GetUnitId());
                    return true;
                }
            }
            HealingTouch.CastOnSelf();
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
        public bool UseHealingTouch = true;
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
            AddControlInWinForm("Use Healing Touch", "UseHealingTouch", "Healing Spells");
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
    public int DefenseHP = 0;
    public int HealHP = 0;
    public int HealMP = 0;

    private Timer _onCd = new Timer(0);

    #endregion

    #region Professions & Racials

    private readonly Spell Berserking = new Spell("Berserking");
    private readonly Spell Darkflight = new Spell("Darkflight");
    private readonly Spell WarStomp = new Spell("War Stomp");

    #endregion

    #region Druid Buffs

    private readonly Spell Dash = new Spell("Dash");
    private readonly Spell DisplacerBeast = new Spell("Displacer Beast");
    private readonly Spell MarkoftheWild = new Spell("Mark of the Wild");
    private readonly Spell MoonfireDebuff = new Spell(164812);
    private readonly Spell StampedingRoar = new Spell("Stampeding Roar");

    #endregion

    #region Offensive Spells

    private readonly Spell HeartoftheWild = new Spell("Heart of the Wild");
    private readonly Spell Hurricane = new Spell("Hurricane");
    private readonly Spell Moonfire = new Spell("Moonfire");
    private readonly Spell Wrath = new Spell("Wrath");

    #endregion

    #region Healing Cooldown

    private readonly Spell ForceofNature = new Spell("Force of Nature");
    private readonly Spell Incarnation = new Spell("Incarnation: Tree of Life");
    private readonly Spell NaturesSwiftness = new Spell("Nature's Swiftness");
    private readonly Spell Tranquility = new Spell("Tranquility");

    #endregion

    #region Defensive Cooldowns

    private readonly Spell Barkskin = new Spell("Barkskin");
    private readonly Spell IncapacitatingRoar = new Spell("Incapacitating Roar");
    private readonly Spell Ironbark = new Spell("Ironbark");
    private readonly Spell MassEntanglement = new Spell("Mass Entanglement");
    private readonly Spell MightyBash = new Spell("Mighty Bash");
    private readonly Spell Typhoon = new Spell("Typhoon");
    private readonly Spell UrsolsVortex = new Spell("Ursol's Vortex");

    #endregion

    #region Healing Spells

    private readonly Spell CenarionWard = new Spell("Cenarion Ward");
    private readonly Spell HealingTouch = new Spell("Healing Touch");
    private readonly Spell Lifebloom = new Spell("Lifebloom");
    private readonly Spell NaturesVigil = new Spell("Nature's Vigil");
    private readonly Spell Regrowth = new Spell("Regrowth");
    private readonly Spell Rejuvenation = new Spell("Rejuvenation");
    private readonly Spell Renewal = new Spell("Renewal");
    private readonly Spell Swiftmend = new Spell("Swiftmend");
    private readonly Spell WildGrowth = new Spell("Wild Growth");
    private readonly Spell WildMushroom = new Spell("Wild Mushroom");

    #endregion

    public DruidRestoration()
    {
        Main.InternalRange = 30.0f;
        Main.InternalAggroRange = 30f;
        Main.InternalLightHealingSpell = HealingTouch;
        MySettings = DruidRestorationSettings.GetSettings();
        Main.DumpCurrentSettings<DruidRestorationSettings>(MySettings);
        UInt128 lastTarget = 0;
        LowHP();

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
                                && (Moonfire.IsHostileDistanceGood || Wrath.IsHostileDistanceGood))
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

    private void LowHP()
    {
        if (MySettings.UseBarkskinBelowPercentage > DefenseHP)
            DefenseHP = MySettings.UseBarkskinBelowPercentage;

        if (MySettings.UseIncapacitatingRoarBelowPercentage > DefenseHP)
            DefenseHP = MySettings.UseIncapacitatingRoarBelowPercentage;

        if (MySettings.UseIronbarkBelowPercentage > DefenseHP)
            DefenseHP = MySettings.UseIronbarkBelowPercentage;

        if (MySettings.UseMassEntanglementBelowPercentage > DefenseHP)
            DefenseHP = MySettings.UseMassEntanglementBelowPercentage;

        if (MySettings.UseMightyBashBelowPercentage > DefenseHP)
            DefenseHP = MySettings.UseMightyBashBelowPercentage;

        if (MySettings.UseTyphoonBelowPercentage > DefenseHP)
            DefenseHP = MySettings.UseTyphoonBelowPercentage;

        if (MySettings.UseUrsolsVortexBelowPercentage > DefenseHP)
            DefenseHP = MySettings.UseUrsolsVortexBelowPercentage;

        if (MySettings.UseWarStompBelowPercentage > DefenseHP)
            DefenseHP = MySettings.UseWarStompBelowPercentage;

        if (MySettings.UseCenarionWardBelowPercentage > HealHP)
            HealHP = MySettings.UseCenarionWardBelowPercentage;

        if (MySettings.UseHealingTouchBelowPercentage > HealHP)
            HealHP = MySettings.UseHealingTouchBelowPercentage;

        if (MySettings.UseHeartoftheWildBelowPercentage > HealHP)
            HealHP = MySettings.UseHeartoftheWildBelowPercentage;

        if (MySettings.UseLifebloomBelowPercentage > HealHP)
            HealHP = MySettings.UseLifebloomBelowPercentage;

        if (MySettings.UseNaturesVigilBelowPercentage > HealHP)
            HealHP = MySettings.UseNaturesVigilBelowPercentage;

        if (MySettings.UseRegrowthBelowPercentage > HealHP)
            HealHP = MySettings.UseRegrowthBelowPercentage;

        if (MySettings.UseRejuvenationBelowPercentage > HealHP)
            HealHP = MySettings.UseRejuvenationBelowPercentage;

        if (MySettings.UseSwiftmendBelowPercentage > HealHP)
            HealHP = MySettings.UseSwiftmendBelowPercentage;

        if (MySettings.UseRenewalBelowPercentage > HealHP)
            HealHP = MySettings.UseRenewalBelowPercentage;

        if (MySettings.UseWildGrowthBelowPercentage > HealHP)
            HealHP = MySettings.UseWildGrowthBelowPercentage;

        if (MySettings.UseWildMushroomBelowPercentage > HealHP)
            HealHP = MySettings.UseWildMushroomBelowPercentage;
    }

    private void Pull()
    {
        if (MySettings.UseWrath && Wrath.IsSpellUsable && Wrath.IsHostileDistanceGood)
        {
            Wrath.Cast();
            return;
        }

        if (MySettings.UseMoonfire && Moonfire.IsSpellUsable && Moonfire.IsHostileDistanceGood)
            Moonfire.Cast();
    }

    private void Combat()
    {
        Buff();

        if (MySettings.DoAvoidMelee)
            AvoidMelee();

        DPSCycle();

        if (_onCd.IsReady && ObjectManager.Me.HealthPercent <= DefenseHP)
            DefenseCycle();

        if (ObjectManager.Me.HealthPercent <= HealHP || ObjectManager.Me.ManaPercentage <= HealMP)
            Heal();

        HealingBurst();
        DPSCycle();
    }

    private void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (MySettings.UseDarkflight && Darkflight.KnownSpell && !Dash.HaveBuff && !StampedingRoar.HaveBuff && ObjectManager.Me.GetMove && !ObjectManager.Target.InCombat && Darkflight.IsSpellUsable)
            Darkflight.Cast();

        if (MySettings.UseDash && !Darkflight.HaveBuff && !StampedingRoar.HaveBuff && ObjectManager.Me.GetMove && !ObjectManager.Target.InCombat && Dash.IsSpellUsable)
            Dash.Cast();

        if (MySettings.UseDisplacerBeast && !ObjectManager.Me.InCombat && ObjectManager.Me.GetMove && !Dash.HaveBuff && !StampedingRoar.HaveBuff
            && DisplacerBeast.IsSpellUsable)
            DisplacerBeast.Cast();

        if (MySettings.UseMarkoftheWild && !MarkoftheWild.HaveBuff && MarkoftheWild.IsSpellUsable)
            MarkoftheWild.Cast();

        if (MySettings.UseStampedingRoar && !Darkflight.HaveBuff && !Dash.HaveBuff && ObjectManager.Me.GetMove && !ObjectManager.Target.InCombat && StampedingRoar.IsSpellUsable)
            StampedingRoar.Cast();

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
        if (MySettings.UseBarkskin && ObjectManager.Me.HealthPercent <= MySettings.UseBarkskinBelowPercentage && Barkskin.IsSpellUsable)
        {
            if (ObjectManager.Me.IsCast)
                ObjectManager.Me.StopCast();

            Others.SafeSleep(1000);
            Barkskin.Cast();
            _onCd = new Timer(1000*12);
            return;
        }
        if (MySettings.UseIncapacitatingRoar && (ObjectManager.Me.HealthPercent <= MySettings.UseIncapacitatingRoarBelowPercentage
                                                 || ObjectManager.GetUnitInSpellRange(10 /*, ObjectManager.Me*/) > 2) && IncapacitatingRoar.IsSpellUsable)
        {
            if (ObjectManager.Me.IsCast)
                ObjectManager.Me.StopCast();

            Others.SafeSleep(1000);
            IncapacitatingRoar.Cast();
            _onCd = new Timer(1000*3);

            if (MySettings.UseDisplacerBeast && DisplacerBeast.IsSpellUsable && ObjectManager.GetUnitInSpellRange(5 /*, ObjectManager.Me*/) > 0)
                DisplacerBeast.Cast();
            else
            {
                Logging.WriteFight("Defensively Moving Away");
                var maxTimeTimer = new Timer(1000*3);
                MovementsAction.MoveBackward(true);
                if (ObjectManager.Target.InCombat && !maxTimeTimer.IsReady && IncapacitatingRoar.TargetHaveBuff && ObjectManager.GetUnitInSpellRange(5 /*, ObjectManager.Me*/) > 0)
                {
                    Logging.WriteFight("Defensively Moving Away");
                    Others.SafeSleep(3000);
                }
                MovementsAction.MoveBackward(false);
                if (maxTimeTimer.IsReady && ObjectManager.Target.InCombat && ObjectManager.GetUnitInSpellRange(5 /*, ObjectManager.Me*/) > 2)
                {
                    MovementsAction.MoveForward(true);
                    Others.SafeSleep(3000);
                    MovementsAction.MoveForward(false);
                    MovementManager.Face(ObjectManager.Target.Position);
                }
            }

            return;
        }
        if (MySettings.UseIronbark && ObjectManager.Me.HealthPercent <= MySettings.UseIronbarkBelowPercentage && Ironbark.IsSpellUsable)
        {
            Ironbark.Cast();
            _onCd = new Timer(1000*12);
            return;
        }
        if (MySettings.UseMassEntanglement && (ObjectManager.Me.HealthPercent <= MySettings.UseMassEntanglementBelowPercentage
                                               || ObjectManager.GetUnitInSpellRange(10 /*, ObjectManager.Me*/) > 2) && MassEntanglement.IsSpellUsable)
        {
            if (ObjectManager.Me.IsCast)
                ObjectManager.Me.StopCast();

            Others.SafeSleep(1000);
            MassEntanglement.Cast();
            _onCd = new Timer(1000*3);

            if (MySettings.UseDisplacerBeast && DisplacerBeast.IsSpellUsable && ObjectManager.GetUnitInSpellRange(5 /*, ObjectManager.Me*/) > 0)
                DisplacerBeast.Cast();
            else
            {
                Logging.WriteFight("Defensively Moving Away");
                var maxTimeTimer = new Timer(1000*3);
                MovementsAction.MoveBackward(true);
                if (ObjectManager.Target.GetDistance < 10 && ObjectManager.Target.InCombat && !maxTimeTimer.IsReady && MassEntanglement.TargetHaveBuff)
                {
                    Logging.WriteFight("Defensively Moving Away");
                    Others.SafeSleep(3000);
                }
                MovementsAction.MoveBackward(false);
                if (maxTimeTimer.IsReady && ObjectManager.Target.InCombat && ObjectManager.GetUnitInSpellRange(5 /*, ObjectManager.Me*/) > 2)
                {
                    MovementsAction.MoveForward(true);
                    Others.SafeSleep(3000);
                    MovementsAction.MoveForward(false);
                    MovementManager.Face(ObjectManager.Target.Position);
                }
            }

            return;
        }
        if (MySettings.UseMightyBash && ObjectManager.Me.HealthPercent <= MySettings.UseMightyBashBelowPercentage && MightyBash.IsSpellUsable && MightyBash.IsHostileDistanceGood)
        {
            if (ObjectManager.Me.IsCast)
                ObjectManager.Me.StopCast();

            Others.SafeSleep(1000);
            MightyBash.Cast();
            _onCd = new Timer(1000*5);
            return;
        }
        if (MySettings.UseTyphoon && ObjectManager.Me.HealthPercent <= MySettings.UseTyphoonBelowPercentage && Typhoon.IsSpellUsable && Typhoon.IsHostileDistanceGood)
        {
            if (ObjectManager.Me.IsCast)
                ObjectManager.Me.StopCast();

            Others.SafeSleep(1000);
            Typhoon.Cast();
            _onCd = new Timer(1000*6);
            return;
        }
        if (MySettings.UseUrsolsVortex && ObjectManager.Me.HealthPercent <= MySettings.UseUrsolsVortexBelowPercentage && UrsolsVortex.IsSpellUsable
            && UrsolsVortex.IsHostileDistanceGood)
        {
            if (ObjectManager.Me.IsCast)
                ObjectManager.Me.StopCast();

            Others.SafeSleep(1000);
            SpellManager.CastSpellByIDAndPosition(102793, ObjectManager.Me.Position);

            if (MySettings.UseDisplacerBeast && DisplacerBeast.IsSpellUsable && ObjectManager.GetUnitInSpellRange(5 /*, ObjectManager.Me*/) > 0)
                DisplacerBeast.Cast();
            else
            {
                Logging.WriteFight("Defensively Moving Away");
                var maxTimeTimer = new Timer(1000*4);
                MovementsAction.MoveBackward(true);
                while (ObjectManager.Target.GetDistance < 10 && ObjectManager.Target.InCombat && !maxTimeTimer.IsReady && UrsolsVortex.TargetHaveBuff)
                    Others.SafeSleep(300);
                MovementsAction.MoveBackward(false);
                if (maxTimeTimer.IsReady && ObjectManager.Target.GetDistance < 10 && ObjectManager.Target.InCombat && UrsolsVortex.TargetHaveBuff)
                {
                    MovementsAction.MoveForward(true);
                    Others.SafeSleep(3000);
                    MovementsAction.MoveForward(false);
                    MovementManager.Face(ObjectManager.Target.Position);
                }
            }

            return;
        }
        if (MySettings.UseWarStomp && ObjectManager.Me.HealthPercent <= MySettings.UseWarStompBelowPercentage && WarStomp.IsSpellUsable
            && ObjectManager.GetUnitInSpellRange(WarStomp.MaxRangeHostile) > 0)
        {
            if (ObjectManager.Me.IsCast)
                ObjectManager.Me.StopCast();

            Others.SafeSleep(1000);
            WarStomp.Cast();
            _onCd = new Timer(1000*2);
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (MySettings.UseCenarionWard && ObjectManager.Me.HealthPercent <= MySettings.UseCenarionWardBelowPercentage && CenarionWard.IsSpellUsable)
        {
            CenarionWard.Cast();
            return;
        }
        if (MySettings.UseHealingTouch && ObjectManager.Me.HealthPercent <= MySettings.UseHealingTouchBelowPercentage && HealingTouch.IsSpellUsable)
        {
            HealingTouch.Cast();
            return;
        }
        if (MySettings.UseNaturesSwiftness && ObjectManager.Me.HealthPercent <= MySettings.UseHealingTouchBelowPercentage && NaturesSwiftness.IsSpellUsable)
        {
            NaturesSwiftness.Cast();
            return;
        }
        if (MySettings.UseNaturesVigil && ObjectManager.Me.HealthPercent <= MySettings.UseNaturesVigilBelowPercentage && NaturesVigil.IsSpellUsable)
        {
            NaturesVigil.Cast();
            return;
        }
        if (MySettings.UseRegrowth && ObjectManager.Me.HealthPercent <= MySettings.UseRegrowthBelowPercentage && !Regrowth.HaveBuff && Regrowth.IsSpellUsable)
        {
            Regrowth.Cast();
            return;
        }
        if (MySettings.UseRejuvenation && ObjectManager.Me.HealthPercent <= MySettings.UseHealingTouchBelowPercentage && !Rejuvenation.HaveBuff && Rejuvenation.IsSpellUsable)
        {
            Rejuvenation.Cast();
            return;
        }
        if (MySettings.UseRenewal && ObjectManager.Me.HealthPercent <= MySettings.UseRenewalBelowPercentage && Renewal.IsSpellUsable)
        {
            Renewal.Cast();
            return;
        }
        if (MySettings.UseSwiftmend && ObjectManager.Me.HealthPercent <= MySettings.UseSwiftmendBelowPercentage && Rejuvenation.HaveBuff && Swiftmend.IsSpellUsable)
        {
            Swiftmend.Cast();
            return;
        }
        if (MySettings.UseTranquility && ObjectManager.Me.HealthPercent <= MySettings.UseTranquilityBelowPercentage && Tranquility.IsSpellUsable)
        {
            Tranquility.Cast(false, true);
            return;
        }
        if (MySettings.UseWildGrowth && ObjectManager.Me.HealthPercent <= MySettings.UseWildGrowthBelowPercentage && !WildGrowth.HaveBuff && WildGrowth.IsSpellUsable)
        {
            WildGrowth.Cast();
            return;
        }
        if (MySettings.UseWildMushroom && ObjectManager.Me.HealthPercent <= MySettings.UseWildMushroomBelowPercentage && !WildMushroom.HaveBuff && WildMushroom.IsSpellUsable)
        {
            SpellManager.CastSpellByIDAndPosition(145205, ObjectManager.Me.Position);
            return;
        }
    }

    public void HealingBurst()
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
        }
        if (MySettings.UseBerserking && Berserking.IsSpellUsable && Wrath.IsHostileDistanceGood)
            Berserking.Cast();

        if (MySettings.UseForceofNature && (ForceofNature.GetSpellCharges > 1 || (Incarnation.HaveBuff && ForceofNature.GetSpellCharges > 0))
            && ForceofNature.IsSpellUsable && ForceofNature.IsHostileDistanceGood)
            ForceofNature.Cast();

        if (MySettings.UseHeartoftheWild && HeartoftheWild.IsSpellUsable && Wrath.IsHostileDistanceGood)
            HeartoftheWild.Cast();

        if (MySettings.UseIncarnation && Incarnation.IsSpellUsable && Wrath.IsHostileDistanceGood)
            Incarnation.Cast();
    }

    private void DPSCycle()
    {
        Usefuls.SleepGlobalCooldown();
        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            if (MySettings.UseMoonfire && !MoonfireDebuff.TargetHaveBuff && Moonfire.IsSpellUsable && Moonfire.IsHostileDistanceGood)
            {
                Moonfire.Cast();
                return;
            }
            if (MySettings.UseHurricane && Hurricane.IsSpellUsable && ObjectManager.GetUnitInSpellRange(8 /*, ObjectManager.Target*/) > 3 && Hurricane.IsHostileDistanceGood)
            {
                SpellManager.CastSpellByIDAndPosition(16914, ObjectManager.Target.Position);
                return;
            }
            if (MySettings.UseWrath && MoonfireDebuff.TargetHaveBuff && Wrath.IsSpellUsable && Wrath.IsHostileDistanceGood)
                Wrath.Cast(true, false, true);
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    private void Patrolling()
    {
        if (ObjectManager.Me.IsMounted)
            return;
        Buff();
        Heal();
    }

    #region Nested type: DruidRestorationSettings

    [Serializable]
    public class DruidRestorationSettings : Settings
    {
        public bool DoAvoidMelee = false;
        public int DoAvoidMeleeDistance = 0;
        public bool UseAlchFlask = true;
        public bool UseBarkskin = true;
        public int UseBarkskinBelowPercentage = 75;
        public bool UseBerserking = true;
        public bool UseCenarionWard = true;
        public int UseCenarionWardBelowPercentage = 85;
        public bool UseDarkflight = true;
        public bool UseDash = true;
        public bool UseDisplacerBeast = true;
        public bool UseForceofNature = true;
        public bool UseHealingTouch = true;
        public int UseHealingTouchBelowPercentage = 60;
        public bool UseHeartoftheWild = true;
        public int UseHeartoftheWildBelowPercentage = 60;
        public bool UseHurricane = true;
        public bool UseIncapacitatingRoar = true;
        public int UseIncapacitatingRoarBelowPercentage = 50;
        public bool UseIncarnation = true;
        public bool UseIronbark = true;
        public int UseIronbarkBelowPercentage = 70;
        public bool UseLifebloom = true;
        public int UseLifebloomBelowPercentage = 70;
        public bool UseMarkoftheWild = true;
        public bool UseMassEntanglement = true;
        public int UseMassEntanglementBelowPercentage = 50;
        public bool UseMightyBash = true;
        public int UseMightyBashBelowPercentage = 80;
        public bool UseMoonfire = true;
        public bool UseNaturesSwiftness = true;
        public bool UseNaturesVigil = false;
        public int UseNaturesVigilBelowPercentage = 80;
        public bool UseRegrowth = true;
        public int UseRegrowthBelowPercentage = 65;
        public bool UseRejuvenation = true;
        public int UseRejuvenationBelowPercentage = 80;
        public bool UseRenewal = true;
        public int UseRenewalBelowPercentage = 65;
        public bool UseStampedingRoar = true;
        public bool UseSwiftmend = true;
        public int UseSwiftmendBelowPercentage = 75;
        public bool UseTranquility = true;
        public int UseTranquilityBelowPercentage = 30;
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;
        public bool UseTyphoon = true;
        public int UseTyphoonBelowPercentage = 50;
        public bool UseUrsolsVortex = true;
        public int UseUrsolsVortexBelowPercentage = 50;
        public bool UseWarStomp = true;
        public int UseWarStompBelowPercentage = 80;
        public bool UseWildGrowth = true;
        public int UseWildGrowthBelowPercentage = 45;
        public bool UseWildMushroom = true;
        public int UseWildMushroomBelowPercentage = 55;
        public bool UseWrath = true;

        public DruidRestorationSettings()
        {
            ConfigWinForm("Druid Restoration Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Darkflight", "UseDarkflight", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            /* Druid Buffs */
            AddControlInWinForm("Use Dash", "UseDash", "Druid Buffs");
            AddControlInWinForm("Use Displacer Beast", "UseDisplacerBeast", "Druid Buffs");
            AddControlInWinForm("Use Mark of the Wild", "UseMarkoftheWild", "Druid Buffs");
            AddControlInWinForm("Use Stampeding Roar", "UseStampedingRoar", "Druid Buffs");
            /* Offensive Spells */
            AddControlInWinForm("Use Heart of the Wild", "UseHeartoftheWild", "Offensive Spells");
            AddControlInWinForm("Use Hurricane", "UseHurricane", "Offensive Spells");
            AddControlInWinForm("Use Moonfire", "UseMoonfire", "Offensive Spells");
            AddControlInWinForm("Use Wrath", "UseWrath", "Offensive Spells");
            /* Healing Cooldown */
            AddControlInWinForm("Use Force of Nature", "UseForceofNature", "Healing Cooldown");
            AddControlInWinForm("Use Incarnation: Tree of Life", "UseIncarnation", "Healing Cooldown");
            AddControlInWinForm("Use Nature's Swiftness", "UseNaturesSwiftness", "Healing Cooldown");
            AddControlInWinForm("Use Tranquility", "UseTranquility", "Healing Cooldown", "BelowPercentage");
            /* Defensive Cooldowns */
            AddControlInWinForm("Use Barkskin", "UseBarkskin", "Defensive Cooldowns", "BelowPercentage");
            AddControlInWinForm("Use Incapacitating Roar", "UseIncapacitatingRoar", "Defensive Cooldowns", "BelowPercentage");
            AddControlInWinForm("Use Ironbark", "UseIronbark", "Defensive Cooldowns", "BelowPercentage");
            AddControlInWinForm("Use Mass Entanglement", "UseMassEntanglement", "Defensive Cooldowns", "BelowPercentage");
            AddControlInWinForm("Use Mighty Bash", "UseMightyBash", "Defensive Cooldowns", "BelowPercentage");
            AddControlInWinForm("Use Typhoon", "UseTyphoon", "Defensive Cooldowns", "BelowPercentage");
            AddControlInWinForm("Use Ursol's Vortex", "UseUrsolsVortex", "Defensive Cooldowns", "BelowPercentage");
            /* Healing Spells */
            AddControlInWinForm("Use Cenarion Ward", "UseCenarionWard", "Healing Spells", "BelowPercentage");
            AddControlInWinForm("Use Healing Touch", "UseHealingTouch", "Healing Spells", "BelowPercentage");
            AddControlInWinForm("Use Lifebloom", "UseLifebloom", "Offensive Spells", "BelowPercentage");
            AddControlInWinForm("Use Nature's Vigil", "UseNaturesVigil", "Offensive Cooldowns", "BelowPercentage");
            AddControlInWinForm("Use Regrowth", "UseRegrowth", "Offensive Spells", "BelowPercentage");
            AddControlInWinForm("Use Rejuvenation", "UseRejuvenation", "Healing Spells", "BelowPercentage");
            AddControlInWinForm("Use Renewal", "UseRenewal", "Healing Spells", "BelowPercentage");
            AddControlInWinForm("Use Swiftmend", "UseSwiftmend", "Healing Spells", "BelowPercentage");
            AddControlInWinForm("Use Wild Growth", "UseWildGrowth", "Healing Spells", "BelowPercentage");
            AddControlInWinForm("Use WildMushroom", "UseWildMushroom", "Healing Spells", "BelowPercentage");
            /* Game Settings */
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
            AddControlInWinForm("Do avoid melee (Off Advised!!)", "DoAvoidMelee", "Game Settings");
            AddControlInWinForm("Avoid melee distance (1 to 4)", "DoAvoidMeleeDistance", "Game Settings");
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
    private Timer StunTimer = new Timer(0);

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

    private readonly Spell RageoftheSleeper = new Spell("Rage of the Sleeper");

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
        Main.InternalLightHealingSpell = HealingTouch;
        MySettings = DruidGuardianSettings.GetSettings();
        Main.DumpCurrentSettings<DruidGuardianSettings>(MySettings);
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
        if (AgroManagement() || Defensive() || Shapeshift() || Offensive())
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
            if (StunTimer.IsReady && (DefensiveTimer.IsReady || ObjectManager.Me.HealthPercent < 20))
            {
                //Stun
                if (ObjectManager.Target.IsStunnable)
                {
                    if (MightyBash.IsSpellUsable && ObjectManager.Me.HealthPercent < MySettings.UseMightyBashBelowPercentage)
                    {
                        MightyBash.Cast();
                        StunTimer = new Timer(1000*5);
                        return true;
                    }
                    if (WarStomp.IsSpellUsable && ObjectManager.Me.HealthPercent < MySettings.UseWarStompBelowPercentage)
                    {
                        WarStomp.Cast();
                        StunTimer = new Timer(1000*2);
                    }
                }
                //Mitigate Damage
                if (SurvivalInstincts.IsSpellUsable && (SurvivalInstincts.GetSpellCharges == 2 ||
                                                        ObjectManager.Me.HealthPercent < MySettings.UseSurvivalInstinctsBelowPercentage))
                {
                    SurvivalInstincts.Cast();
                    DefensiveTimer = new Timer(1000*6);
                }
                if (Barkskin.IsSpellUsable && ObjectManager.Me.HealthPercent < MySettings.UseBarkskinBelowPercentage)
                {
                    Barkskin.Cast();
                    DefensiveTimer = new Timer(1000*12);
                }
                if (RageoftheSleeper.IsSpellUsable && ObjectManager.Me.HealthPercent < MySettings.UseRageoftheSleeperBelowPercentage)
                {
                    RageoftheSleeper.Cast();
                    DefensiveTimer = new Timer(1000*10);
                }
            }
            //Mitigate Magic Damage for Rage
            if (ObjectManager.Me.HealthPercent < MySettings.UseMarkofUrsolBelowPercentage &&
                ObjectManager.Me.Rage >= 45 && MarkofUrsol.IsSpellUsable &&
                !MarkofUrsol.HaveBuff)
            {
                MarkofUrsol.Cast();
                DefensiveTimer = new Timer(1000*6);
            }
            //Increase Armor for Rage
            if ((ObjectManager.Me.HealthPercent < MySettings.UseIronfurBelowHealthPercentage ||
                 ObjectManager.Me.Rage > MySettings.UseIronfurAboveRagePercentage) &&
                Ironfur.IsSpellUsable && ObjectManager.Me.Rage >= 45)
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
            if (MySettings.UseBearForm && BearForm.IsSpellUsable && !BearForm.HaveBuff)
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
        public int UseRageoftheSleeperBelowPercentage = 40;

        /* Offensive Cooldowns */
        public bool UseBristlingFur = true;
        public bool UseIncarnation = true;
        public bool UseLunarBeam = true;

        /* Defensive Cooldowns */
        public int UseBarkskinBelowPercentage = 75;
        public int UseIronfurBelowHealthPercentage = 0;
        public int UseIronfurAboveRagePercentage = 100;
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