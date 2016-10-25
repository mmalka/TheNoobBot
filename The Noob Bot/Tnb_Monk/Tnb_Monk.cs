/*
* CombatClass for TheNoobBot
* Credit : Vesper, Neo2003, Dreadlocks, Ryuichiro
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
    internal static Spell InternalLightHealingSpell;
    internal static float Version = 1.0f;

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
        Logging.WriteDebug("Loaded " + ObjectManager.Me.WowSpecialization() + " Combat Class " + Version.ToString("0.0###"));

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

    private bool CombatMode = true;

    private Timer DefensiveTimer = new Timer(0);
    private Timer StunTimer = new Timer(0);

    #endregion

    #region Professions & Racial

    //private readonly Spell ArcaneTorrent = new Spell("Arcane Torrent"); //No GCD
    private readonly Spell Berserking = new Spell("Berserking"); //No GCD
    private readonly Spell BloodFury = new Spell("Blood Fury"); //No GCD
    private readonly Spell Darkflight = new Spell("Darkflight"); //No GCD
    private readonly Spell GiftoftheNaaru = new Spell("Gift of the Naaru"); //No GCD
    private readonly Spell Stoneform = new Spell("Stoneform"); //No GCD
    private readonly Spell WarStomp = new Spell("War Stomp"); //No GCD

    #endregion

    #region Artifact Spells

    private readonly Spell ExplodingKeg = new Spell("Exploding Keg");

    #endregion

    #region Offensive Spells

    private readonly Spell BlackoutKick = new Spell("Blackout Kick");
    private readonly Spell BlackoutStrike = new Spell("Blackout Strike");
    private readonly Spell BreathofFire = new Spell("Breath of Fire");
    private readonly Spell ChiBurst = new Spell("Chi Burst");
    private readonly Spell ChiWave = new Spell("Chi Wave");
    private readonly Spell KegSmash = new Spell("Keg Smash");
    private readonly Spell RushingJadeWind = new Spell("Rushing Jade Wind");
    private readonly Spell TigerPalm = new Spell("Tiger Palm");

    #endregion

    #region Defensive Spells

    private readonly Spell DampenHarm = new Spell("Dampen Harm"); //No GCD
    private readonly Spell DiffuseMagic = new Spell("Diffuse Magic"); //No GCD
    private readonly Spell InvokeNiuzaotheBlackOx = new Spell("Invoke Niuzao, the Black Ox"); //No GCD
    private readonly Spell FortifyingBrew = new Spell("Fortifying Brew"); //No GCD
    private readonly Spell IronskinBrew = new Spell("Ironskin Brew"); //No GCD
    private readonly Spell PurifyingBrew = new Spell("Purifying Brew"); //No GCD
    private readonly Spell ZenMeditation = new Spell("Zen Meditation"); //No GCD

    #endregion

    #region Healing Spell

    private readonly Spell Effuse = new Spell("Effuse");
    private readonly Spell ExpelHarm = new Spell("Expel Harm");

    #endregion

    #region Utility Spells

    private readonly Spell BlackOxBrew = new Spell("Black Ox Brew");
    private readonly Spell Detox = new Spell("Detox");
    private readonly Spell LegSweep = new Spell("Leg Sweep");
    private readonly Spell Paralysis = new Spell("Paralysis");
    private readonly Spell Provoke = new Spell("Provoke"); //No GCD
    private readonly Spell Resuscitate = new Spell("Resuscitate");
    private readonly Spell Roll = new Spell("Roll"); //No GCD
    private readonly Spell SpearHandStrike = new Spell("Spear Hand Strike");
    private readonly Spell TigersLust = new Spell("Tiger's Lust");
    private readonly Spell Transcendence = new Spell("Transcendence");
    private readonly Spell TranscendenceTransfer = new Spell("Transcendence: Transfer");

    #endregion

    public MonkBrewmaster()
    {
        Main.InternalRange = ObjectManager.Me.GetCombatReach;
        Main.InternalAggroRange = Main.InternalRange;
        Main.InternalAggroRange = 25.0f;
        Main.InternalLightHealingSpell = Effuse;
        MySettings = MonkBrewmasterSettings.GetSettings();
        Main.DumpCurrentSettings<MonkBrewmasterSettings>(MySettings);
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
                                lastTarget = ObjectManager.Me.Target;

                            if (CombatClass.InSpellRange(ObjectManager.Target, 0, 40))
                                Combat();
                            else if (!ObjectManager.Me.IsCast)
                                Patrolling();
                        }
                        else if (!ObjectManager.Me.IsCast)
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

    // For Movement Spells (always return after Casting)
    private void Patrolling()
    {
        //Log
        if (CombatMode)
        {
            Logging.WriteFight("Patrolling:");
            CombatMode = false;
        }

        if (ObjectManager.Me.GetMove && !Usefuls.PlayerUsingVehicle)
        {
            //Movement Buffs
            if (!Darkflight.HaveBuff && !TigersLust.HaveBuff) // doesn't stack
            {
                if (MySettings.UseDarkflight && Darkflight.IsSpellUsable)
                {
                    Darkflight.Cast();
                    return;
                }
                if (MySettings.UseTigersLust && TigersLust.IsSpellUsable)
                {
                    TigersLust.CastOnSelf();
                    return;
                }
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

    // For Self-Healing Spells (always return after Casting)
    private bool Healing()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            //Gift of the Naaru
            if (ObjectManager.Me.HealthPercent < MySettings.UseGiftoftheNaaruBelowPercentage && GiftoftheNaaru.IsSpellUsable)
            {
                GiftoftheNaaru.CastOnSelf();
                return true;
            }
            //Expel Harm when
            if (ObjectManager.Me.HealthPercent < MySettings.UseExpelHarmBelowPercentage && ExpelHarm.IsSpellUsable &&
                //TODO: there are X or more Healing Spheres available
                false)
            {
                ExpelHarm.CastOnSelf();
                return true;
            }
            //Effuse
            if (ObjectManager.Me.HealthPercent < MySettings.UseEffuseBelowPercentage &&
                !ObjectManager.Me.GetMove && Effuse.IsSpellUsable)
            {
                Effuse.CastOnSelf();
                return true;
            }
            return false;
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    // For Defensive Buffs and Livesavers (always return after Casting)
    private bool Defensive()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            if (StunTimer.IsReady && (DefensiveTimer.IsReady || ObjectManager.Me.HealthPercent < 20))
            {
                //Stun
                if (ObjectManager.Target.IsStunnable)
                {
                    if (ObjectManager.Me.HealthPercent < MySettings.UseWarStompBelowPercentage && WarStomp.IsSpellUsable)
                    {
                        WarStomp.Cast();
                        StunTimer = new Timer(1000*2.5);
                        return true;
                    }
                    if (ObjectManager.Me.HealthPercent < MySettings.UseLegSweepBelowPercentage && LegSweep.IsSpellUsable &&
                        ObjectManager.Me.GetUnitInSpellRange(5f) >= 1)
                    {
                        LegSweep.Cast();
                        StunTimer = new Timer(1000*5);
                        return true;
                    }
                }
                //Mitigate Damage
                if (ObjectManager.Me.HealthPercent < MySettings.UseStoneformBelowPercentage && Stoneform.IsSpellUsable)
                {
                    Stoneform.Cast();
                    DefensiveTimer = new Timer(1000*8);
                    return true;
                }
                //Exploding Keg
                if (ObjectManager.Me.HealthPercent < MySettings.UseExplodingKegBelowPercentage &&
                    ExplodingKeg.IsSpellUsable && ExplodingKeg.IsHostileDistanceGood)
                {
                    ExplodingKeg.CastAtPosition(ObjectManager.Target.Position);
                    DefensiveTimer = new Timer(1000*3); // Time until the Buff ends
                    return true;
                }
                //Dampen Harm
                if (ObjectManager.Me.HealthPercent < MySettings.UseDampenHarmBelowPercentage &&
                    DampenHarm.IsSpellUsable)
                {
                    DampenHarm.Cast();
                    return true;
                }
            }
            //Mitigate Damage in Emergency Situations
            //Fortifying Brew
            if (ObjectManager.Me.HealthPercent < MySettings.UseFortifyingBrewBelowPercentage && FortifyingBrew.IsSpellUsable)
            {
                FortifyingBrew.Cast();
                return true;
            }
            //Diffuse Magic
            if (ObjectManager.Me.HealthPercent < MySettings.UseDiffuseMagicBelowPercentage && DiffuseMagic.IsSpellUsable)
            {
                DiffuseMagic.Cast();
                return true;
            }
            //Invoke Niuzao, the Black Ox
            if (ObjectManager.Me.HealthPercent < MySettings.UseInvokeNiuzaotheBlackOxBelowPercentage &&
                InvokeNiuzaotheBlackOx.IsSpellUsable && InvokeNiuzaotheBlackOx.IsHostileDistanceGood)
            {
                InvokeNiuzaotheBlackOx.Cast();
                return true;
            }
            //Zen Meditation
            /*if (ObjectManager.Me.HealthPercent < MySettings.UseZenMeditationBelowPercentage && ZenMeditation.IsSpellUsable)
            {
                ZenMeditation.Cast();
                return true;
            }*/
            return false;
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    // For Offensive Buffs (only return if a Cast triggered Global Cooldown)
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
            //Cast Black Ox Brew when
            if (MySettings.UseBlackOxBrew && BlackOxBrew.IsSpellUsable &&
                //it will generate 3 Brew Charges and atleast 70 energy
                IronskinBrew.GetSpellCharges == 0 && ObjectManager.Me.Energy <= 30)
            {
                BlackOxBrew.Cast();
            }
            return false;
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    // For the Ability Priority Logic
    private void Rotation()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            //Cast Provoke
            if (MySettings.UseProvoke && Provoke.IsSpellUsable && Provoke.IsHostileDistanceGood &&
                !ObjectManager.Target.IsTargetingMe)
            {
                Provoke.Cast();
                return;
            }
            //Cast Ironskin Brew when
            if (MySettings.UseIronskinBrew && IronskinBrew.IsSpellUsable && !IronskinBrew.HaveBuff &&
                // you have 3 Brew Charges
                IronskinBrew.GetSpellCharges == 3)
            {
                IronskinBrew.Cast();
                return;
            }
            //Cast Purifying Brew
            if (ObjectManager.Me.HealthPercent > MySettings.UsePurifyingBrewBelowPercentage &&
                PurifyingBrew.IsSpellUsable && !IronskinBrew.HaveBuff)
            {
                PurifyingBrew.Cast();
                return;
            }
            //Cast Keg Smash
            if (MySettings.UseKegSmash && KegSmash.IsSpellUsable && KegSmash.IsHostileDistanceGood)
            {
                KegSmash.Cast();
                return;
            }
            //Cast Chi Burst
            if (MySettings.UseChiBurst && ChiBurst.IsSpellUsable && ChiBurst.IsHostileDistanceGood)
            {
                ChiBurst.Cast();
                return;
            }
            //Cast Chi Wave
            if (MySettings.UseChiWave && ChiWave.IsSpellUsable && ChiWave.IsHostileDistanceGood)
            {
                ChiWave.Cast();
                return;
            }
            //Cast Rushing Jade Wind
            if (MySettings.UseRushingJadeWind && RushingJadeWind.IsSpellUsable &&
                ObjectManager.Me.GetUnitInSpellRange(5f) >= 1)
            {
                RushingJadeWind.Cast();
                return;
            }
            //Cast Breath of Fire when
            if (MySettings.UseBreathofFire && BreathofFire.IsSpellUsable && BreathofFire.IsHostileDistanceGood &&
                //you have 2 or more targets
                ObjectManager.Target.GetUnitInSpellRange(8f) >= 2)
            {
                BreathofFire.Cast();
                return;
            }
            //Cast Blackout Strike
            if (MySettings.UseBlackoutStrike && BlackoutStrike.IsSpellUsable && BlackoutStrike.IsHostileDistanceGood)
            {
                BlackoutStrike.Cast();
                return;
            }
            //Cast Breath of Fire when
            if (MySettings.UseBreathofFire && BreathofFire.IsSpellUsable && BreathofFire.IsHostileDistanceGood &&
                //the target has the Keg Smash Dot
                KegSmash.TargetHaveBuffFromMe)
            {
                BreathofFire.Cast();
                return;
            }
            //Cast Tiger Palm
            if (ObjectManager.Me.Energy > MySettings.UseTigerPalmAbovePercentage &&
                TigerPalm.IsSpellUsable && TigerPalm.IsHostileDistanceGood)
            {
                TigerPalm.Cast();
                return;
            }
            //Cast Blackout Kick
            if (MySettings.UseBlackoutKick && BlackoutKick.IsSpellUsable && BlackoutKick.IsHostileDistanceGood)
            {
                BlackoutKick.Cast();
                return;
            }
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    #region Nested type: MonkBrewmasterSettings

    [Serializable]
    public class MonkBrewmasterSettings : Settings
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
        public int UseExplodingKegBelowPercentage = 100;

        /* Offensive Spells */
        public bool UseBlackoutKick = true;
        public bool UseBlackoutStrike = true;
        public bool UseBreathofFire = true;
        public bool UseChiBurst = true;
        public bool UseChiWave = true;
        public bool UseKegSmash = true;
        public bool UseRushingJadeWind = true;
        public int UseTigerPalmAbovePercentage = 65;

        /* Offensive Cooldowns */
        public bool UseBlackOxBrew = true;

        /* Defensive Spells */
        public int UseDampenHarmBelowPercentage = 50;
        public int UseDiffuseMagicBelowPercentage = 30;
        public int UseInvokeNiuzaotheBlackOxBelowPercentage = 50;
        public bool UseIronskinBrew = true;
        public int UseFortifyingBrewBelowPercentage = 25;
        public int UseLegSweepBelowPercentage = 50;
        public int UsePurifyingBrewBelowPercentage = 40;

        /* Healing Spells */
        public int UseEffuseBelowPercentage = 0;
        public int UseExpelHarmBelowPercentage = 0;

        /* Utility Spells */
        public bool UseProvoke = false;
        public bool UseTigersLust = true;

        /* Game Settings */
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;

        public MonkBrewmasterSettings()
        {
            ConfigWinForm("Brewmaster Monk Settings");
            /* Professions & Racials */
            //AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Darkflight", "UseDarkflight", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaruBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            AddControlInWinForm("Use Stone Form", "UseStoneformBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            AddControlInWinForm("Use War Stomp", "UseWarStompBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            /* Artifact Spells */
            AddControlInWinForm("Use Exploding Keg", "UseExplodingKegBelowPercentage", "Artifact Spells", "BelowPercentage", "Life");
            /* Offensive Spells */
            AddControlInWinForm("Use Blackout Kick", "UseBlackoutKick", "Offensive Spells");
            AddControlInWinForm("Use Blackout Strike", "UseBlackoutStrike", "Offensive Spells");
            AddControlInWinForm("Use Breath of Fire", "UseBreathofFire", "Offensive Spells");
            AddControlInWinForm("Use Chi Burst", "UseChiBurst", "Offensive Spells");
            AddControlInWinForm("Use Chi Wave", "UseChiWave", "Offensive Spells");
            AddControlInWinForm("Use Keg Smash", "UseKegSmash", "Offensive Spells");
            AddControlInWinForm("Use Rushing Jade Wind", "UseRushingJadeWind", "Offensive Spells");
            AddControlInWinForm("Use Tiger Palm", "UseTigerPalmAbovePercentage", "Offensive Spells", "AbovePercentage", "Energy");
            /* Offensive Cooldowns */
            AddControlInWinForm("Use Black Ox Brew", "UseBlackOxBrew", "Offensive Cooldowns");
            /* Defensive Spells */
            AddControlInWinForm("Use Dampen Harm", "UseDampenHarmBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Diffuse Magic", "UseDiffuseMagicBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Invoke Niuzaom the Black Ox", "UseInvokeNiuzaotheBlackOxBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Ironskin Brew", "UseIronskinBrew", "Defensive Spells");
            AddControlInWinForm("Use Fortifying Brew", "UseFortifyingBrewBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Leg Sweep", "UseLegSweepBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Purifying Brew", "UsePurifyingBrewBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            /* Healing Spell */
            AddControlInWinForm("Use Effuse", "UseEffuseBelowPercentage", "Healing Spells", "BelowPercentage", "Life");
            //AddControlInWinForm("Use Expel Harm", "UseExpelHarmBelowPercentage", "Healing Spells", "BelowPercentage", "Life");
            /* Utility Spells */
            AddControlInWinForm("Use Provoke", "UseProvoke", "Utility Spells");
            AddControlInWinForm("Use Tiger's Lust", "UseTigersLust", "Utility Spells");
            /* Game Settings */
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");
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

    private bool CombatMode = true;

    private Timer DefensiveTimer = new Timer(0);
    private Timer StunTimer = new Timer(0);

    #endregion

    #region Professions & Racial

    //private readonly Spell ArcaneTorrent = new Spell("Arcane Torrent"); //No GCD
    private readonly Spell Berserking = new Spell("Berserking"); //No GCD
    private readonly Spell BloodFury = new Spell("Blood Fury"); //No GCD
    private readonly Spell Darkflight = new Spell("Darkflight"); //No GCD
    private readonly Spell GiftoftheNaaru = new Spell("Gift of the Naaru"); //No GCD
    private readonly Spell Stoneform = new Spell("Stoneform"); //No GCD
    private readonly Spell WarStomp = new Spell("War Stomp"); //No GCD

    #endregion

    #region Artifact Spells

    private readonly Spell StrikeoftheWindlord = new Spell("Strike of the Windlord");

    #endregion

    #region Offensive Spells

    private readonly Spell BlackoutKick = new Spell("Blackout Kick");
    private readonly Spell ChiBurst = new Spell("Chi Burst");
    private readonly Spell ChiWave = new Spell("Chi Wave");
    private readonly Spell FistsofFury = new Spell("Fists of Fury");
    private readonly Spell RisingSunKick = new Spell("Rising Sun Kick");
    private readonly Spell TigerPalm = new Spell("Tiger Palm");
    private readonly Spell SpinningCraneKick = new Spell("Spinning Crane Kick");
    private readonly Spell WhirlingDragonPunch = new Spell("Whirling Dragon Punch");

    #endregion

    #region Offensive Cooldowns

    private readonly Spell StormEarthandFire = new Spell("Storm, Earth, and Fire"); //No GCD
    private readonly Spell TouchofDeath = new Spell("Touch of Death");

    #endregion

    #region Defensive Spells

    private readonly Spell DampenHarm = new Spell("Dampen Harm"); //No GCD
    private readonly Spell DiffuseMagic = new Spell("Diffuse Magic"); //No GCD
    private readonly Spell TouchofKarma = new Spell("Touch of Karma"); //No GCD

    #endregion

    #region Healing Spell

    private readonly Spell Effuse = new Spell("Effuse");

    #endregion

    #region Utility Spells

    private readonly Spell Detox = new Spell("Detox");
    private readonly Spell EnergizingElixir = new Spell("Energizing Elixir");
    private readonly Spell LegSweep = new Spell("Leg Sweep");
    private readonly Spell Paralysis = new Spell("Paralysis");
    private readonly Spell Provoke = new Spell("Provoke"); //No GCD
    private readonly Spell Reawaken = new Spell("Reawaken");
    private readonly Spell Resuscitate = new Spell("Resuscitate");
    private readonly Spell Roll = new Spell("Roll"); //No GCD
    private readonly Spell SpearHandStrike = new Spell("Spear Hand Strike");
    private readonly Spell TigersLust = new Spell("Tiger's Lust");
    private readonly Spell Transcendence = new Spell("Transcendence");
    private readonly Spell TranscendenceTransfer = new Spell("Transcendence: Transfer");

    #endregion

    public MonkWindwalker()
    {
        Main.InternalRange = ObjectManager.Me.GetCombatReach;
        Main.InternalAggroRange = Main.InternalRange;
        Main.InternalLightHealingSpell = Effuse;
        MySettings = MonkWindwalkerSettings.GetSettings();
        Main.DumpCurrentSettings<MonkWindwalkerSettings>(MySettings);
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
                                lastTarget = ObjectManager.Me.Target;

                            if (CombatClass.InSpellRange(ObjectManager.Target, 0, 40))
                                Combat();
                            else if (!ObjectManager.Me.IsCast)
                                Patrolling();
                        }
                        else if (!ObjectManager.Me.IsCast)
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

    // For Movement Spells (always return after Casting)
    private void Patrolling()
    {
        //Log
        if (CombatMode)
        {
            Logging.WriteFight("Patrolling:");
            CombatMode = false;
        }

        if (ObjectManager.Me.GetMove && !Usefuls.PlayerUsingVehicle)
        {
            //Movement Buffs
            if (!Darkflight.HaveBuff && !TigersLust.HaveBuff) // doesn't stack
            {
                if (MySettings.UseDarkflight && Darkflight.IsSpellUsable)
                {
                    Darkflight.Cast();
                    return;
                }
                if (MySettings.UseTigersLust && TigersLust.IsSpellUsable)
                {
                    TigersLust.CastOnSelf();
                    return;
                }
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

    // For Self-Healing Spells (always return after Casting)
    private bool Healing()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            //Gift of the Naaru
            if (ObjectManager.Me.HealthPercent < MySettings.UseGiftoftheNaaruBelowPercentage && GiftoftheNaaru.IsSpellUsable)
            {
                GiftoftheNaaru.CastOnSelf();
                return true;
            }
            //Effuse
            if (ObjectManager.Me.HealthPercent < MySettings.UseEffuseBelowPercentage &&
                !ObjectManager.Me.GetMove && Effuse.IsSpellUsable)
            {
                Effuse.CastOnSelf();
                return true;
            }
            return false;
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    // For Defensive Buffs and Livesavers (always return after Casting)
    private bool Defensive()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            if (StunTimer.IsReady && (DefensiveTimer.IsReady || ObjectManager.Me.HealthPercent < 20))
            {
                //Stun
                if (ObjectManager.Target.IsStunnable)
                {
                    if (ObjectManager.Me.HealthPercent < MySettings.UseWarStompBelowPercentage && WarStomp.IsSpellUsable)
                    {
                        WarStomp.Cast();
                        StunTimer = new Timer(1000*2.5);
                        return true;
                    }
                    if (ObjectManager.Me.HealthPercent < MySettings.UseLegSweepBelowPercentage && LegSweep.IsSpellUsable &&
                        ObjectManager.Me.GetUnitInSpellRange(5f) >= 1)
                    {
                        LegSweep.Cast();
                        StunTimer = new Timer(1000*5);
                        return true;
                    }
                }
                //Mitigate Damage
                if (ObjectManager.Me.HealthPercent < MySettings.UseStoneformBelowPercentage && Stoneform.IsSpellUsable)
                {
                    Stoneform.Cast();
                    DefensiveTimer = new Timer(1000*8);
                    return true;
                }
                //Touch of Karma
                if (ObjectManager.Me.HealthPercent < MySettings.UseTouchofKarmaBelowPercentage &&
                    TouchofKarma.IsSpellUsable && TouchofKarma.IsHostileDistanceGood)
                {
                    TouchofKarma.Cast();
                    DefensiveTimer = new Timer(1000*10);
                    return true;
                }
                //Dampen Harm
                if (ObjectManager.Me.HealthPercent < MySettings.UseDampenHarmBelowPercentage &&
                    DampenHarm.IsSpellUsable)
                {
                    DampenHarm.Cast();
                    return true;
                }
            }
            //Mitigate Damage in Emergency Situations
            //Diffuse Magic
            if (ObjectManager.Me.HealthPercent < MySettings.UseDiffuseMagicBelowPercentage && DiffuseMagic.IsSpellUsable)
            {
                DiffuseMagic.Cast();
                return true;
            }
            return false;
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    // For Offensive Buffs (only return if a Cast triggered Global Cooldown)
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
            //Cast Storm, Earth and Fire when
            if (MySettings.UseStormEarthandFire && StormEarthandFire.IsSpellUsable &&
                //you have 2 or more Charges
                StormEarthandFire.GetSpellCharges >= 2)
            {
                StormEarthandFire.Cast();
            }
            //Cast Energizing Elixir when
            if (ObjectManager.Me.Energy < MySettings.UseEnergizingElixirBelowEnergyPecentage &&
                ObjectManager.Me.Chi < MySettings.UseEnergizingElixirBelowChiPecentage &&
                StormEarthandFire.IsSpellUsable)
            {
                EnergizingElixir.Cast();
            }
            return false;
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    // For the Ability Priority Logic
    private void Rotation()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            //Cast Provoke
            if (MySettings.UseProvoke && Provoke.IsSpellUsable && Provoke.IsHostileDistanceGood &&
                !ObjectManager.Target.IsTargetingMe)
            {
                Provoke.Cast();
                return;
            }
            //Cast Touch of Death
            if (MySettings.UseTouchofDeath && TouchofDeath.IsSpellUsable && TouchofDeath.IsHostileDistanceGood)
            {
                TouchofDeath.Cast();
                return;
            }
            //Cast Whirling Dragon Punch TESTING: Range Check
            if (MySettings.UseWhirlingDragonPunch && WhirlingDragonPunch.IsSpellUsable &&
                WhirlingDragonPunch.IsHostileDistanceGood)
            {
                WhirlingDragonPunch.Cast();
                return;
            }
            //Cast Fists of Fury
            if (MySettings.UseFistsofFury && FistsofFury.IsSpellUsable && FistsofFury.IsHostileDistanceGood)
            {
                FistsofFury.Cast();
                return;
            }
            //Cast Strike of the Windlord
            if (MySettings.UseStrikeoftheWindlord && StrikeoftheWindlord.IsSpellUsable && StrikeoftheWindlord.IsHostileDistanceGood)
            {
                StrikeoftheWindlord.Cast();
                return;
            }
            //Cast Chi Burst when
            if (MySettings.UseChiBurst && ChiBurst.IsSpellUsable && ChiBurst.IsHostileDistanceGood &&
                //you have multiple Targets
                ObjectManager.Target.GetUnitInSpellRange(5f) > 1)
            {
                ChiBurst.Cast();
                return;
            }
            //Single Target
            if (ObjectManager.Me.GetUnitInSpellRange(8f) == 1)
            {
                //Cast Tiger Palm when
                if (MySettings.UseTigerPalm && TigerPalm.IsSpellUsable && TigerPalm.IsHostileDistanceGood &&
                    //you have 4 or less Chi && over 90% Energy
                    ObjectManager.Me.Chi <= 4 && ObjectManager.Me.EnergyPercentage > 90)
                {
                    TigerPalm.Cast();
                    return;
                }
                //Cast Rising Sun Kick
                if (MySettings.UseRisingSunKick && RisingSunKick.IsSpellUsable && RisingSunKick.IsHostileDistanceGood)
                {
                    RisingSunKick.Cast();
                    return;
                }
                //Cast Chi Wave
                if (MySettings.UseChiWave && ChiWave.IsSpellUsable && ChiWave.IsHostileDistanceGood)
                {
                    ChiWave.Cast();
                    return;
                }
            }
                //Multiple Target
            else
            {
                //Cast Spinning Crane Kick
                if (MySettings.UseSpinningCraneKick && SpinningCraneKick.IsSpellUsable && SpinningCraneKick.IsHostileDistanceGood)
                {
                    SpinningCraneKick.Cast();
                    return;
                }
            }
            //Cast Blackout Kick
            if (MySettings.UseBlackoutKick && BlackoutKick.IsSpellUsable && BlackoutKick.IsHostileDistanceGood)
            {
                BlackoutKick.Cast();
                return;
            }
            //Cast Tiger Palm
            if (MySettings.UseTigerPalm && TigerPalm.IsSpellUsable && TigerPalm.IsHostileDistanceGood)
            {
                TigerPalm.Cast();
                return;
            }
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    #region Nested type: MonkWindwalkerSettings

    [Serializable]
    public class MonkWindwalkerSettings : Settings
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
        public bool UseStrikeoftheWindlord = true;

        /* Offensive Spells */
        public bool UseBlackoutKick = true;
        public bool UseChiBurst = true;
        public bool UseChiWave = true;
        public bool UseFistsofFury = true;
        public bool UseRisingSunKick = true;
        public bool UseSpinningCraneKick = true;
        public bool UseTigerPalm = true;
        public bool UseWhirlingDragonPunch = true;

        /* Offensive Cooldowns */
        public bool UseStormEarthandFire = true;
        public bool UseTouchofDeath = true;

        /* Defensive Spells */
        public int UseDampenHarmBelowPercentage = 50;
        public int UseDiffuseMagicBelowPercentage = 30;
        public int UseLegSweepBelowPercentage = 50;
        public int UseTouchofKarmaBelowPercentage = 50;

        /* Healing Spells */
        public int UseEffuseBelowPercentage = 0;

        /* Utility Spells */
        public int UseEnergizingElixirBelowEnergyPecentage = 20;
        public int UseEnergizingElixirBelowChiPecentage = 1;
        public bool UseProvoke = false;
        public bool UseTigersLust = true;

        /* Game Settings */
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;

        public MonkWindwalkerSettings()
        {
            ConfigWinForm("Windwalker Monk Settings");
            /* Professions & Racials */
            //AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Darkflight", "UseDarkflight", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaruBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            AddControlInWinForm("Use Stone Form", "UseStoneformBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            AddControlInWinForm("Use War Stomp", "UseWarStompBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            /* Artifact Spells */
            AddControlInWinForm("Use Strike of the Windlord", "UseStrikeoftheWindlord", "Artifact Spells");
            /* Offensive Spells */
            AddControlInWinForm("Use Blackout Kick", "UseBlackoutKick", "Offensive Spells");
            AddControlInWinForm("Use Breath of Fire", "UseBreathofFire", "Offensive Spells");
            AddControlInWinForm("Use Chi Burst", "UseChiBurst", "Offensive Spells");
            AddControlInWinForm("Use Chi Wave", "UseChiWave", "Offensive Spells");
            AddControlInWinForm("Use Fists of Fury", "UseFistsofFury", "Offensive Spells");
            AddControlInWinForm("Use Rising Sun Kick", "UseRisingSunKick", "Offensive Spells");
            AddControlInWinForm("Use Spinning Crane Kick", "UseSpinningCraneKick", "Offensive Spells");
            AddControlInWinForm("Use Tiger Palm", "UseTigerPalm", "Offensive Spells");
            AddControlInWinForm("Use Whirling Dragon Punch", "UseWhirlingDragonPunch", "Offensive Spells");
            /* Offensive Cooldowns */
            AddControlInWinForm("Use Storm, Earth and Fire", "UseStormEarthandFire", "Offensive Cooldowns");
            AddControlInWinForm("Use Touch of Death", "UseTouchofDeath", "Offensive Cooldowns");
            /* Defensive Spells */
            AddControlInWinForm("Use Dampen Harm", "UseDampenHarmBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Diffuse Magic", "UseDiffuseMagicBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Leg Sweep", "UseLegSweepBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Touch of Karma", "UseTouchofKarmaBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            /* Healing Spell */
            AddControlInWinForm("Use Effuse", "UseEffuseBelowPercentage", "Healing Spells", "BelowPercentage", "Life");
            /* Utility Spells */
            AddControlInWinForm("Use Energizing Elixir", "UseEnergizingElixirBelowEnergyPecentage", "Utility Spells", "BelowPercentage", "Energy");
            AddControlInWinForm("Use Energizing Elixir", "UseEnergizingElixirBelowChiPecentage", "Utility Spells", "BelowPercentage", "Chi");
            AddControlInWinForm("Use Provoke", "UseProvoke", "Utility Spells");
            AddControlInWinForm("Use Tiger's Lust", "UseTigersLust", "Utility Spells");
            /* Game Settings */
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");
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

    private bool CombatMode = true;

    private Timer DefensiveTimer = new Timer(0);
    private Timer StunTimer = new Timer(0);

    #endregion

    #region Professions & Racial

    //private readonly Spell ArcaneTorrent = new Spell("Arcane Torrent"); //No GCD
    private readonly Spell Berserking = new Spell("Berserking"); //No GCD
    private readonly Spell BloodFury = new Spell("Blood Fury"); //No GCD
    private readonly Spell Darkflight = new Spell("Darkflight"); //No GCD
    private readonly Spell GiftoftheNaaru = new Spell("Gift of the Naaru"); //No GCD
    private readonly Spell Stoneform = new Spell("Stoneform"); //No GCD
    private readonly Spell WarStomp = new Spell("War Stomp"); //No GCD

    #endregion

    #region Buffs

    private readonly Spell TeachingsoftheMonastery = new Spell("Teachings of the Monastery");

    #endregion

    #region Artifact Spells

    private readonly Spell SheilunsGift = new Spell("Sheilun's Gift");

    #endregion

    #region Offensive Spells

    private readonly Spell BlackoutKick = new Spell("Blackout Kick");
    private readonly Spell ChiBurst = new Spell("Chi Burst");
    private readonly Spell ChiWave = new Spell("Chi Wave");
    private readonly Spell RisingSunKick = new Spell("Rising Sun Kick");
    private readonly Spell SpinningCraneKick = new Spell("Spinning Crane Kick");
    private readonly Spell TigerPalm = new Spell("Tiger Palm");

    #endregion

    #region Offensive Cooldowns

    private readonly Spell ManaTea = new Spell("Mana Tea");

    #endregion

    #region Defensive Spells

    private readonly Spell DampenHarm = new Spell("Dampen Harm"); //No GCD
    private readonly Spell DiffuseMagic = new Spell("Diffuse Magic"); //No GCD

    #endregion

    #region Healing Spell

    private readonly Spell Effuse = new Spell("Effuse");
    private readonly Spell EnvelopingMist = new Spell("Enveloping Mist");
    private readonly Spell EssenceFont = new Spell("Essence Font");
    private readonly Spell InvokeChiJitheRedCrane = new Spell("Invoke Chi-Ji, the Red Crane"); //No GCD
    private readonly Spell LifeCocoon = new Spell("Life Cocoon");
    private readonly Spell Mistwalk = new Spell("Mistwalk");
    private readonly Spell RenewingMist = new Spell("Renewing Mist");
    private readonly Spell Revival = new Spell("Revival");
    private readonly Spell Vivify = new Spell("Vivify");

    #endregion

    #region Utility Spells

    private readonly Spell Detox = new Spell("Detox");
    private readonly Spell LegSweep = new Spell("Leg Sweep");
    private readonly Spell Paralysis = new Spell("Paralysis");
    private readonly Spell Provoke = new Spell("Provoke"); //No GCD
    private readonly Spell Reawaken = new Spell("Reawaken");
    private readonly Spell Resuscitate = new Spell("Resuscitate");
    private readonly Spell Roll = new Spell("Roll"); //No GCD
    private readonly Spell SpearHandStrike = new Spell("Spear Hand Strike");
    private readonly Spell TigersLust = new Spell("Tiger's Lust");
    private readonly Spell Transcendence = new Spell("Transcendence");
    private readonly Spell TranscendenceTransfer = new Spell("Transcendence: Transfer");

    #endregion

    public MonkMistweaver()
    {
        Main.InternalRange = 30.0f;
        Main.InternalLightHealingSpell = Effuse;
        MySettings = MonkMistweaverSettings.GetSettings();
        Main.DumpCurrentSettings<MonkMistweaverSettings>(MySettings);
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
                                lastTarget = ObjectManager.Me.Target;

                            if (CombatClass.InSpellRange(ObjectManager.Target, 0, 40))
                                Combat();
                            else if (!ObjectManager.Me.IsCast)
                                Patrolling();
                        }
                        else if (!ObjectManager.Me.IsCast)
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

    // For Movement Spells (always return after Casting)
    private void Patrolling()
    {
        //Log
        if (CombatMode)
        {
            Logging.WriteFight("Patrolling:");
            CombatMode = false;
        }

        if (ObjectManager.Me.GetMove && !Usefuls.PlayerUsingVehicle)
        {
            //Movement Buffs
            if (!Darkflight.HaveBuff && !TigersLust.HaveBuff) // doesn't stack
            {
                if (MySettings.UseDarkflight && Darkflight.IsSpellUsable)
                {
                    Darkflight.Cast();
                    return;
                }
                if (MySettings.UseTigersLust && TigersLust.IsSpellUsable)
                {
                    TigersLust.CastOnSelf();
                    return;
                }
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

    // For Self-Healing Spells (always return after Casting)
    private bool Healing()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            //Gift of the Naaru
            if (ObjectManager.Me.HealthPercent < MySettings.UseGiftoftheNaaruBelowPercentage && GiftoftheNaaru.IsSpellUsable)
            {
                GiftoftheNaaru.CastOnSelf();
                return true;
            }
            //Life Cocoon
            if (ObjectManager.Me.HealthPercent < MySettings.UseLifeCocoonBelowPercentage &&
                LifeCocoon.IsSpellUsable)
            {
                LifeCocoon.CastOnSelf();
                return true;
            }
            //Enveloping Mist
            if (ObjectManager.Me.HealthPercent < MySettings.UseEnvelopingMistBelowPercentage &&
                EnvelopingMist.IsSpellUsable && !EnvelopingMist.HaveBuff)
            {
                EnvelopingMist.CastOnSelf();
                return true;
            }
            //Sheiluns Gift
            if (ObjectManager.Me.HealthPercent < MySettings.UseSheilunsGiftBelowPercentage &&
                !ObjectManager.Me.GetMove && SheilunsGift.IsSpellUsable)
            {
                SheilunsGift.CastOnSelf();
                return true;
            }
            //Effuse
            if (ObjectManager.Me.HealthPercent < MySettings.UseEffuseBelowPercentage &&
                !ObjectManager.Me.GetMove && Effuse.IsSpellUsable)
            {
                Effuse.CastOnSelf();
                return true;
            }
            return false;
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    // For Defensive Buffs and Livesavers (always return after Casting)
    private bool Defensive()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            if (StunTimer.IsReady && (DefensiveTimer.IsReady || ObjectManager.Me.HealthPercent < 20))
            {
                //Stun
                if (ObjectManager.Target.IsStunnable)
                {
                    if (ObjectManager.Me.HealthPercent < MySettings.UseWarStompBelowPercentage && WarStomp.IsSpellUsable)
                    {
                        WarStomp.Cast();
                        StunTimer = new Timer(1000*2.5);
                        return true;
                    }
                    if (ObjectManager.Me.HealthPercent < MySettings.UseLegSweepBelowPercentage && LegSweep.IsSpellUsable &&
                        ObjectManager.Me.GetUnitInSpellRange(5f) >= 1)
                    {
                        LegSweep.Cast();
                        StunTimer = new Timer(1000*5);
                        return true;
                    }
                }
                //Mitigate Damage
                if (ObjectManager.Me.HealthPercent < MySettings.UseStoneformBelowPercentage && Stoneform.IsSpellUsable)
                {
                    Stoneform.Cast();
                    DefensiveTimer = new Timer(1000*8);
                    return true;
                }
                //Dampen Harm
                if (ObjectManager.Me.HealthPercent < MySettings.UseDampenHarmBelowPercentage &&
                    DampenHarm.IsSpellUsable)
                {
                    DampenHarm.Cast();
                    return true;
                }
            }
            //Mitigate Damage in Emergency Situations
            //Diffuse Magic
            if (ObjectManager.Me.HealthPercent < MySettings.UseDiffuseMagicBelowPercentage && DiffuseMagic.IsSpellUsable)
            {
                DiffuseMagic.Cast();
                return true;
            }
            return false;
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    // For Offensive Buffs (only return if a Cast triggered Global Cooldown)
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
            //Cast Mana Tea
            if (ObjectManager.Me.Mana <= MySettings.UseManaTeaBelowPercentage &&
                ManaTea.IsSpellUsable)
            {
                ManaTea.Cast();
            }
            return false;
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    // For the Ability Priority Logic
    private void Rotation()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            //Cast Provoke
            if (MySettings.UseProvoke && Provoke.IsSpellUsable && Provoke.IsHostileDistanceGood &&
                !ObjectManager.Target.IsTargetingMe)
            {
                Provoke.Cast();
                return;
            }
            //Cast Rising Sun Kick
            if (MySettings.UseRisingSunKick && RisingSunKick.IsSpellUsable && RisingSunKick.IsHostileDistanceGood)
            {
                RisingSunKick.Cast();
                return;
            }
            //Cast Chi Burst
            if (MySettings.UseChiBurst && ChiBurst.IsSpellUsable && ChiBurst.IsHostileDistanceGood)
            {
                ChiBurst.Cast();
                return;
            }
            //Cast Chi Wave
            if (MySettings.UseChiWave && ChiWave.IsSpellUsable && ChiWave.IsHostileDistanceGood)
            {
                ChiWave.Cast();
                return;
            }
            //Cast Tiger Palm when
            if (MySettings.UseTigerPalm && TigerPalm.IsSpellUsable && TigerPalm.IsHostileDistanceGood &&
                //you have less than 3 Teachings of the Monastery Stacks
                TeachingsoftheMonastery.BuffStack < 3)
            {
                TigerPalm.Cast();
                return;
            }
            //Cast Blackout Kick
            if (MySettings.UseBlackoutKick && BlackoutKick.IsSpellUsable && BlackoutKick.IsHostileDistanceGood)
            {
                BlackoutKick.Cast();
                return;
            }
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    #region Nested type: MonkMistweaverSettings

    [Serializable]
    public class MonkMistweaverSettings : Settings
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
        public int UseSheilunsGiftBelowPercentage = 25;

        /* Offensive Spells */
        public bool UseBlackoutKick = true;
        public bool UseBlackoutStrike = true;
        public bool UseChiBurst = true;
        public bool UseChiWave = true;
        public bool UseRisingSunKick = true;
        public bool UseTigerPalm = true;

        /* Offensive Cooldowns */
        public int UseManaTeaBelowPercentage = 50;

        /* Defensive Spells */
        public int UseDampenHarmBelowPercentage = 50;
        public int UseDiffuseMagicBelowPercentage = 30;
        public int UseLegSweepBelowPercentage = 50;

        /* Healing Spells */
        public int UseEffuseBelowPercentage = 50;
        public int UseEnvelopingMistBelowPercentage = 50;
        public int UseLifeCocoonBelowPercentage = 20;

        /* Utility Spells */
        public bool UseProvoke = false;
        public bool UseTigersLust = true;

        /* Game Settings */
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;

        public MonkMistweaverSettings()
        {
            ConfigWinForm("Mistweaver Monk Settings");
            /* Professions & Racials */
            //AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Darkflight", "UseDarkflight", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaruBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            AddControlInWinForm("Use Stone Form", "UseStoneformBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            AddControlInWinForm("Use War Stomp", "UseWarStompBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            /* Artifact Spells */
            AddControlInWinForm("Use Sheilun's Gift", "UseSheilunsGiftBelowPercentage", "Artifact Spells", "BelowPercentage", "Life");
            /* Offensive Spells */
            AddControlInWinForm("Use Blackout Kick", "UseBlackoutKick", "Offensive Spells");
            AddControlInWinForm("Use Blackout Strike", "UseBlackoutStrike", "Offensive Spells");
            AddControlInWinForm("Use Breath of Fire", "UseBreathofFire", "Offensive Spells");
            AddControlInWinForm("Use Chi Burst", "UseChiBurst", "Offensive Spells");
            AddControlInWinForm("Use Chi Wave", "UseChiWave", "Offensive Spells");
            AddControlInWinForm("Use Rising Sun Kick", "UseRisingSunKick", "Offensive Spells");
            AddControlInWinForm("Use Tiger Palm", "UseTigerPalm", "Offensive Spells");
            /* Offensive Cooldowns */
            AddControlInWinForm("Use Mana Tea", "UseManaTeaBelowPercentage", "Defensive Spells", "BelowPercentage", "Mana");
            /* Defensive Spells */
            AddControlInWinForm("Use Dampen Harm", "UseDampenHarmBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Diffuse Magic", "UseDiffuseMagicBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Leg Sweep", "UseLegSweepBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            /* Healing Spell */
            AddControlInWinForm("Use Effuse", "UseEffuseBelowPercentage", "Healing Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Enveloping Mist", "UseEnvelopingMistBelowPercentage", "Healing Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Life Cocoon", "UseLifeCocoonBelowPercentage", "Healing Spells", "BelowPercentage", "Life");
            /* Utility Spells */
            AddControlInWinForm("Use Provoke", "UseProvoke", "Utility Spells");
            AddControlInWinForm("Use Tiger's Lust", "UseTigersLust", "Utility Spells");
            /* Game Settings */
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");
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