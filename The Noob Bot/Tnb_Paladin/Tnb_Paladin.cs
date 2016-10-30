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
    internal static float Version = 0.5f;

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
                    #region Paladin Specialisation checking

                case WoWClass.Paladin:

                    if (wowSpecialization == WoWSpecialization.PaladinRetribution || wowSpecialization == WoWSpecialization.None)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Paladin_Retribution.xml";
                            var currentSetting = new PaladinRetribution.PaladinRetributionSettings();
                            if (File.Exists(currentSettingsFile) && !resetSettings)
                            {
                                currentSetting = Settings.Load<PaladinRetribution.PaladinRetributionSettings>(currentSettingsFile);
                            }
                            currentSetting.ToForm();
                            currentSetting.Save(currentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Paladin Retribution Combat class...");
                            EquipmentAndStats.SetPlayerSpe(WoWSpecialization.PaladinRetribution);
                            new PaladinRetribution();
                        }
                        break;
                    }
                    if (wowSpecialization == WoWSpecialization.PaladinProtection)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Paladin_Protection.xml";
                            var currentSetting = new PaladinProtection.PaladinProtectionSettings();
                            if (File.Exists(currentSettingsFile) && !resetSettings)
                            {
                                currentSetting = Settings.Load<PaladinProtection.PaladinProtectionSettings>(currentSettingsFile);
                            }
                            currentSetting.ToForm();
                            currentSetting.Save(currentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Paladin Protection Combat class...");
                            EquipmentAndStats.SetPlayerSpe(WoWSpecialization.PaladinProtection);
                            new PaladinProtection();
                        }
                        break;
                    }
                    if (wowSpecialization == WoWSpecialization.PaladinHoly)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Paladin_Holy.xml";
                            var currentSetting = new PaladinHoly.PaladinHolySettings();
                            if (File.Exists(currentSettingsFile) && !resetSettings)
                            {
                                currentSetting = Settings.Load<PaladinHoly.PaladinHolySettings>(currentSettingsFile);
                            }
                            currentSetting.ToForm();
                            currentSetting.Save(currentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Paladin Holy Combat class...");
                            InternalRange = 30.0f;
                            EquipmentAndStats.SetPlayerSpe(WoWSpecialization.PaladinHoly);
                            new PaladinHoly();
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

#region Paladin

public class PaladinHoly
{
    private static PaladinHolySettings MySettings = PaladinHolySettings.GetSettings();

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

    #region Talents

    private readonly Spell AuraofMercy = new Spell("Aura of Mercy"); //No GCD
    private readonly Spell AuraofSacrifice = new Spell("Aura of Sacrifice"); //No GCD
    private readonly Spell DevotionAura = new Spell("Devotion Aura"); //No GCD
    private readonly Spell JudgmentofLight = new Spell(183778);

    #endregion

    #region Buffs

    private readonly Spell Forbearance = new Spell(25771);

    #endregion

    #region Dot

    private readonly Spell JudgmentofLightDebuff = new Spell(196941);

    #endregion

    #region Artifact Spells

    private readonly Spell TyrsDeliverance = new Spell("Tyr's Deliverance");

    #endregion

    #region Offensive Spells

    private readonly Spell CrusaderStrike = new Spell("Crusader Strike");
    private readonly Spell Judgment = new Spell("Judgment");

    #endregion

    #region Defensive Spells

    private readonly Spell BlessingofProtection = new Spell("Blessing of Protection");
    private readonly Spell BlessingofSacrifice = new Spell("Blessing of Sacrifice"); //No GCD
    private readonly Spell DivineProtection = new Spell("Divine Protection");
    private readonly Spell DivineShield = new Spell("Divine Shield");

    #endregion

    #region Healing Buffs

    private readonly Spell BeaconofFaith = new Spell("Beacon of Faith");
    private readonly Spell BeaconofLight = new Spell("Beacon of Light");
    private readonly Spell BeaconofVirtue = new Spell("Beacon of Virtue");

    #endregion

    #region Healing Spells

    private readonly Spell BestowFaith = new Spell("Bestow Faith");
    private readonly Spell FlashofLight = new Spell("Flash of Light");
    private readonly Spell HolyLight = new Spell("Holy Light");
    private readonly Spell HolyPrism = new Spell("Holy Prism");
    private readonly Spell HolyShock = new Spell("Holy Shock");
    private readonly Spell LightofDawn = new Spell("Light of Dawn");
    private readonly Spell LightoftheMartyr = new Spell("Light of the Martyr");

    #endregion

    #region Healing Cooldowns

    private readonly Spell AvengingWrath = new Spell("Avenging Wrath"); //No GCD
    private readonly Spell AuraMastery = new Spell("Aura Mastery"); //No GCD
    private readonly Spell HolyAvenger = new Spell("Holy Avenger");
    private readonly Spell LayonHands = new Spell("Lay on Hands"); //No GCD
    private readonly Spell LightsHammer = new Spell("Light's Hammer");

    #endregion

    #region Utility Spells

    private readonly Spell BlessingofFreedom = new Spell("Blessing of Freedom");
    private readonly Spell DivineSteed = new Spell("Divine Steed");
    private readonly Spell HammerofJustice = new Spell("Hammer of Justice");
    private readonly Spell Repentance = new Spell("Repentance");
    private readonly Spell RuleofLaw = new Spell("Rule of Law"); //No GCD

    #endregion

    public PaladinHoly()
    {
        Main.InternalRange = 30f;
        Main.InternalAggroRange = 30f;
        Main.InternalLightHealingSpell = FlashofLight;
        MySettings = PaladinHolySettings.GetSettings();
        Main.DumpCurrentSettings<PaladinHolySettings>(MySettings);
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
            if (!Darkflight.HaveBuff) // doesn't stack
            {
                if (MySettings.UseDarkflight && Darkflight.IsSpellUsable)
                {
                    Darkflight.Cast();
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

            //Maintain Beacon of Light
            if (MySettings.UseBeaconofLight && BeaconofLight.IsSpellUsable && !BeaconofLight.HaveBuff)
            {
                BeaconofLight.CastOnSelf();
                return true;
            }
            //Gift of the Naaru
            if (ObjectManager.Me.HealthPercent < MySettings.UseGiftoftheNaaruBelowPercentage && GiftoftheNaaru.IsSpellUsable)
            {
                GiftoftheNaaru.CastOnSelf();
                return true;
            }
            //Lay on Hands
            if (ObjectManager.Me.HealthPercent < MySettings.UseLayonHandsBelowPercentage &&
                LayonHands.IsSpellUsable && !Forbearance.HaveBuff)
            {
                LayonHands.CastOnSelf();
                return true;
            }
            //Use Tyr's Deliverance
            if (ObjectManager.Me.HealthPercent < MySettings.UseTyrsDeliveranceBelowPartyPercentage &&
                TyrsDeliverance.IsSpellUsable && !ObjectManager.Me.GetMove)
            {
                TyrsDeliverance.Cast();
                return true;
            }
            //Use Bestow Faith
            if (ObjectManager.Me.HealthPercent < MySettings.UseBestowFaithBelowPercentage && BestowFaith.IsSpellUsable)
            {
                BestowFaith.CastOnSelf();
                return true;
            }
            //Flash of Light
            if (ObjectManager.Me.HealthPercent < MySettings.UseFlashofLightBelowPercentage &&
                FlashofLight.IsSpellUsable && !Forbearance.HaveBuff)
            {
                FlashofLight.CastOnSelf();
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
                //Divine Shield
                if (ObjectManager.Me.HealthPercent < MySettings.UseDivineShieldBelowPercentage &&
                    DivineShield.IsSpellUsable)
                {
                    DivineShield.Cast();
                    DefensiveTimer = new Timer(1000*8);
                    return true;
                }
                //Stun
                if (ObjectManager.Target.IsStunnable)
                {
                    if (ObjectManager.Me.HealthPercent < MySettings.UseWarStompBelowPercentage && WarStomp.IsSpellUsable)
                    {
                        WarStomp.Cast();
                        StunTimer = new Timer(1000*2.5);
                        return true;
                    }
                    if (ObjectManager.Me.HealthPercent < MySettings.UseHammerofJusticeBelowPercentage &&
                        HammerofJustice.IsSpellUsable && HammerofJustice.IsHostileDistanceGood)
                    {
                        HammerofJustice.Cast();
                        StunTimer = new Timer(1000*6);
                        return true;
                    }
                }
                //Stoneform
                if (ObjectManager.Me.HealthPercent < MySettings.UseStoneformBelowPercentage && Stoneform.IsSpellUsable)
                {
                    Stoneform.Cast();
                    DefensiveTimer = new Timer(1000*8);
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
            //Avenging Wrath
            if (MySettings.UseAvengingWrath && AvengingWrath.IsSpellUsable && !AvengingWrath.HaveBuff)
            {
                AvengingWrath.Cast();
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

            //Use Light's Hammer when it hits multiple enemies
            if (LightsHammer.IsSpellUsable && ObjectManager.Target.GetUnitInSpellRange(10f) > 1)
            {
                LightsHammer.CastAtPosition(ObjectManager.Target.Position);
                return;
            }
            //Cast Judgment
            if (MySettings.UseJudgment && Judgment.IsSpellUsable && Judgment.IsHostileDistanceGood)
            {
                Judgment.Cast();
                return;
            }
            //Cast Holy Shock
            if (MySettings.UseHolyShock && HolyShock.IsSpellUsable && HolyShock.IsHostileDistanceGood)
            {
                HolyShock.Cast();
                return;
            }
            //Cast Crusader Strike
            if (MySettings.UseCrusaderStrike && CrusaderStrike.IsSpellUsable && CrusaderStrike.IsHostileDistanceGood)
            {
                CrusaderStrike.Cast();
                return;
            }
            //Cast Holy Shock
            if (MySettings.UseHolyShock && HolyShock.IsSpellUsable && HolyShock.IsHostileDistanceGood)
            {
                HolyShock.Cast();
                return;
            }
            //Cast Holy Light
            if (MySettings.UseHolyLight && HolyLight.IsSpellUsable && HolyLight.IsHostileDistanceGood)
            {
                HolyLight.Cast();
                return;
            }
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    #region Nested type: PaladinHolySettings

    [Serializable]
    public class PaladinHolySettings : Settings
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
        public int UseTyrsDeliveranceBelowPartyPercentage = 0;

        /* Offensive Spells */
        public bool UseCrusaderStrike = true;
        public bool UseJudgment = true;
        public bool UseHolyShock = true;
        public bool UseHolyLight = true;

        /* Offensive Cooldowns */
        public bool UseAvengingWrath = true;

        /* Defensive Cooldowns */
        public int UseDivineShieldBelowPercentage = 0;
        public int UseHammerofJusticeBelowPercentage = 50;

        /* Healing Spells */
        public bool UseBeaconofLight = true;
        public int UseFlashofLightBelowPercentage = 25;
        public int UseLayonHandsBelowPercentage = 10;
        public int UseBestowFaithBelowPercentage = 40;

        /* Game Settings */
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;

        public PaladinHolySettings()
        {
            ConfigWinForm("Paladin Protection Settings");
            /* Professions & Racials */
            //AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Darkflight", "UseDarkflight", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaruBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            AddControlInWinForm("Use Stone Form", "UseStoneformBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            AddControlInWinForm("Use War Stomp", "UseWarStompBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            /* Artifact Spells */
            AddControlInWinForm("Use Tyr's Deliverance", "UseTyrsDeliveranceBelowPartyPercentage", "Artifact Spells", "BelowPercentage", "Life");
            /* Offensive Spells */
            AddControlInWinForm("Use Crusader Strike", "UseCrusaderStrike", "Offensive Spells");
            AddControlInWinForm("Use Judgment", "UseJudgment", "Offensive Spells");
            AddControlInWinForm("Use Holy Shock", "UseHolyShock", "Offensive Spells");
            AddControlInWinForm("Use Holy Light", "UseHolyLight", "Offensive Spells");
            /* Offensive Cooldowns */
            AddControlInWinForm("Use Avenging Wrath", "UseAvengingWrath", "Offensive Cooldowns");
            /* Defensive Cooldowns */
            AddControlInWinForm("Use Divine Shield", "UseDivineShieldBelowPercentage", "Defensive Cooldowns", "BelowPercentage", "Life");
            AddControlInWinForm("Use Hammer of Justice", "UseHammerofJusticeBelowPercentage", "Defensive Cooldowns", "BelowPercentage", "Life");
            /* Healing Spells */
            AddControlInWinForm("Use Beacon of Light", "UseBeaconofLight", "Healing Spells");
            AddControlInWinForm("Use Flash of Light", "UseFlashofLightBelowPercentage", "Healing Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Lay on Hands", "UseLayonHandsBelowPercentage", "Healing Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Bestow Faith", "UseBestowFaithBelowPercentage", "Healing Spells", "BelowPercentage", "Life");
            /* Game Settings */
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");
        }

        public static PaladinHolySettings CurrentSetting { get; set; }

        public static PaladinHolySettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Paladin_Holy.xml";
            if (File.Exists(currentSettingsFile))
            {
                return
                    CurrentSetting = Load<PaladinHolySettings>(currentSettingsFile);
            }
            return new PaladinHolySettings();
        }
    }

    #endregion
}

public class PaladinProtection
{
    private static PaladinProtectionSettings MySettings = PaladinProtectionSettings.GetSettings();

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

    private readonly Spell Forbearance = new Spell(25771);

    #endregion

    #region Artifact Spells

    private readonly Spell EyeofTyr = new Spell("Eye of Tyr");

    #endregion

    #region Offensive Spells

    public readonly Spell AvengersShield = new Spell("Avenger's Shield");
    public readonly Spell BlessedHammer = new Spell("Blessed Hammer");
    public readonly Spell Consecration = new Spell("Consecration");
    public readonly Spell CrusaderStrike = new Spell("Crusader Strike");
    public readonly Spell HammerOfTheRighteous = new Spell("Hammer of the Righteous");
    public readonly Spell HammerOfWrath = new Spell("Hammer of Wrath");
    public readonly Spell Judgment = new Spell("Judgment");

    #endregion

    #region Offensive Cooldowns

    public readonly Spell AvengingWrath = new Spell("Avenging Wrath"); //No GCD

    #endregion

    #region Defensive Spells

    public readonly Spell ShieldOfTheRighteous = new Spell("Shield of the Righteous"); //No GCD

    #endregion

    #region Defensive Cooldowns

    public readonly Spell ArdentDefender = new Spell("Ardent Defender"); //No GCD
    private readonly Spell BlessingofProtection = new Spell("Blessing of Protection");
    private readonly Spell BlessingofSpellwarding = new Spell("Blessing of Spellwarding");
    public readonly Spell DivineShield = new Spell("Divine Shield");
    public readonly Spell GuardianOfAncientKings = new Spell("Guardian of Ancient Kings"); //No GCD

    #endregion

    #region Healing Spells

    public readonly Spell FlashofLight = new Spell("Flash of Light");
    public readonly Spell HandoftheProtector = new Spell("Hand of the Protector"); //No GCD
    public readonly Spell LayonHands = new Spell("Lay on Hands"); //No GCD
    public readonly Spell LightoftheProtector = new Spell("Light of the Protector"); //No GCD

    #endregion

    #region Utility Spells

    private readonly Spell BlessingofSacrifice = new Spell("Blessing of Sacrifice"); //No GCD
    private readonly Spell BlessingofSalvation = new Spell("Blessing of Salvation");
    public readonly Spell HammerofJustice = new Spell("Hammer of Justice");
    public readonly Spell HandofReckoning = new Spell("Hand of Reckoning"); //No GCD

    #endregion

    public PaladinProtection()
    {
        Main.InternalRange = ObjectManager.Me.GetCombatReach;
        Main.InternalAggroRange = Main.InternalRange;
        Main.InternalLightHealingSpell = FlashofLight;
        MySettings = PaladinProtectionSettings.GetSettings();
        Main.DumpCurrentSettings<PaladinProtectionSettings>(MySettings);
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
            if (!Darkflight.HaveBuff) // doesn't stack
            {
                if (MySettings.UseDarkflight && Darkflight.IsSpellUsable)
                {
                    Darkflight.Cast();
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
        if (Defensive() || AggroManagement() || Offensive())
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
            //Lay on Hands
            if (ObjectManager.Me.HealthPercent < MySettings.UseLayonHandsBelowPercentage &&
                LayonHands.IsSpellUsable && !Forbearance.HaveBuff)
            {
                LayonHands.CastOnSelf();
                return true;
            }
            //Light of the Protector
            if (ObjectManager.Me.HealthPercent < MySettings.UseLightoftheProtectorBelowPercentage &&
                LightoftheProtector.IsSpellUsable)
            {
                LightoftheProtector.CastOnSelf();
                return true;
            }
            //Flash of Light
            if (ObjectManager.Me.HealthPercent < MySettings.UseFlashofLightBelowPercentage &&
                FlashofLight.IsSpellUsable && !Forbearance.HaveBuff)
            {
                FlashofLight.CastOnSelf();
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
                //Ardent Defender
                if (ObjectManager.Me.HealthPercent < MySettings.UseArdentDefenderBelowPercentage &&
                    ArdentDefender.IsSpellUsable)
                {
                    ArdentDefender.Cast();
                    DefensiveTimer = new Timer(1000*8);
                    return true;
                }
                //Divine Shield
                if (ObjectManager.Me.HealthPercent < MySettings.UseDivineShieldBelowPercentage &&
                    DivineShield.IsSpellUsable)
                {
                    DivineShield.Cast();
                    DefensiveTimer = new Timer(1000*8);
                    return true;
                }
                //Stun
                if (ObjectManager.Target.IsStunnable)
                {
                    if (ObjectManager.Me.HealthPercent < MySettings.UseWarStompBelowPercentage && WarStomp.IsSpellUsable)
                    {
                        WarStomp.Cast();
                        StunTimer = new Timer(1000*2.5);
                        return true;
                    }
                    if (ObjectManager.Me.HealthPercent < MySettings.UseHammerofJusticeBelowPercentage &&
                        HammerofJustice.IsSpellUsable && HammerofJustice.IsHostileDistanceGood)
                    {
                        HammerofJustice.Cast();
                        StunTimer = new Timer(1000*6);
                        return true;
                    }
                }
                //Guardian of Ancient Kings
                if (ObjectManager.Me.HealthPercent < MySettings.UseGuardianOfAncientKingsBelowPercentage &&
                    GuardianOfAncientKings.IsSpellUsable)
                {
                    GuardianOfAncientKings.Cast();
                    DefensiveTimer = new Timer(1000*8);
                    return true;
                }
                //Stoneform
                if (ObjectManager.Me.HealthPercent < MySettings.UseStoneformBelowPercentage && Stoneform.IsSpellUsable)
                {
                    Stoneform.Cast();
                    DefensiveTimer = new Timer(1000*8);
                    return true;
                }
                //Shield of the Righteous
                if (ObjectManager.Me.HealthPercent < MySettings.UseShieldOfTheRighteousBelowPercentage &&
                    ShieldOfTheRighteous.IsSpellUsable && ShieldOfTheRighteous.IsHostileDistanceGood &&
                    !ShieldOfTheRighteous.HaveBuff)
                {
                    ShieldOfTheRighteous.Cast();
                    DefensiveTimer = new Timer(1000*4.5);
                    return true;
                }
            }
            //Eye of Tyr
            if (ObjectManager.Me.HealthPercent < MySettings.UseEyeofTyrBelowPercentage &&
                EyeofTyr.IsSpellUsable && ObjectManager.Me.GetUnitInSpellRange(25f) > 0)
            {
                EyeofTyr.Cast();
                DefensiveTimer = new Timer(1000*8);
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
            //Avenging Wrath
            if (MySettings.UseAvengingWrath && AvengingWrath.IsSpellUsable && !AvengingWrath.HaveBuff)
            {
                AvengingWrath.Cast();
            }
            return false;
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    // For Spots (always return after Casting)
    private bool AggroManagement()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            //Cast Taunt when you are in a party and the target of your target is a low health player
            if (MySettings.UseHandofReckoningBelowToTPercentage > 0 && HandofReckoning.IsSpellUsable &&
                HandofReckoning.IsHostileDistanceGood)
            {
                WoWObject obj = ObjectManager.GetObjectByGuid(ObjectManager.Target.Target);
                if (obj.IsValid && obj.Type == WoWObjectType.Player &&
                    new WoWPlayer(obj.GetBaseAddress).HealthPercent < MySettings.UseHandofReckoningBelowToTPercentage)
                {
                    HandofReckoning.Cast();
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

    // For the Ability Priority Logic
    private void Rotation()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            //Cast Avenger's Shield when you have multiple target
            if (MySettings.UseAvengersShield && AvengersShield.IsSpellUsable &&
                AvengersShield.IsHostileDistanceGood && ObjectManager.Target.GetUnitInSpellRange(8f) > 1)
            {
                AvengersShield.Cast();
                return;
            }
            //Cast Consecration when you have multiple target
            if (MySettings.UseConsecration && Consecration.IsSpellUsable &&
                ObjectManager.Me.GetUnitInSpellRange(8f) > 1)
            {
                Consecration.Cast();
                return;
            }
            //Cast Blessed Hammer
            if (MySettings.UseBlessedHammer && BlessedHammer.IsSpellUsable && BlessedHammer.IsHostileDistanceGood)
            {
                BlessedHammer.Cast();
                return;
            }
            //Cast Hammer of the Righteous when you have multiple target
            if (MySettings.UseHammerOfTheRighteous && HammerOfTheRighteous.IsSpellUsable &&
                ObjectManager.Me.GetUnitInSpellRange(8f) > 1)
            {
                HammerOfTheRighteous.Cast();
                return;
            }
            //Cast Judgment
            if (MySettings.UseJudgment && Judgment.IsSpellUsable && Judgment.IsHostileDistanceGood)
            {
                Judgment.Cast();
                return;
            }
            //Cast Consecration
            if (MySettings.UseConsecration && Consecration.IsSpellUsable &&
                ObjectManager.Me.GetUnitInSpellRange(8f) >= 1)
            {
                Consecration.Cast();
                return;
            }
            //Cast Avenger's Shield
            if (MySettings.UseAvengersShield && AvengersShield.IsSpellUsable && AvengersShield.IsHostileDistanceGood)
            {
                AvengersShield.Cast();
                return;
            }
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    #region Nested type: PaladinProtectionSettings

    [Serializable]
    public class PaladinProtectionSettings : Settings
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
        public int UseEyeofTyrBelowPercentage = 90;

        /* Offensive Spells */
        public bool UseAvengersShield = true;
        public bool UseConsecration = true;
        public bool UseJudgment = true;
        public bool UseHammerOfTheRighteous = true;
        public bool UseBlessedHammer = true;

        /* Offensive Cooldowns */
        public bool UseAvengingWrath = true;

        /* Defensive Spells */
        public int UseShieldOfTheRighteousBelowPercentage = 10;

        /* Defensive Cooldowns */
        public int UseArdentDefenderBelowPercentage = 10;
        public int UseDivineShieldBelowPercentage = 0;
        public int UseGuardianOfAncientKingsBelowPercentage = 25;
        public int UseHammerofJusticeBelowPercentage = 50;

        /* Healing Spells */
        public int UseFlashofLightBelowPercentage = 25;
        public int UseLayonHandsBelowPercentage = 10;
        public int UseLightoftheProtectorBelowPercentage = 60;

        /* Utility Spells */
        public int UseHandofReckoningBelowToTPercentage = 20;

        /* Game Settings */
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;

        public PaladinProtectionSettings()
        {
            ConfigWinForm("Paladin Protection Settings");
            /* Professions & Racials */
            //AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Darkflight", "UseDarkflight", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaruBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            AddControlInWinForm("Use Stone Form", "UseStoneformBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            AddControlInWinForm("Use War Stomp", "UseWarStompBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            /* Artifact Spells */
            AddControlInWinForm("Use Eye of Tyr", "UseEyeofTyrBelowPercentage", "Artifact Spells", "BelowPercentage", "Life");
            /* Offensive Spells */
            AddControlInWinForm("Use Avenger's Shield", "UseAvengersShield", "Offensive Spells");
            AddControlInWinForm("Use Consecration", "UseConsecration", "Offensive Spells");
            AddControlInWinForm("Use Judgment", "UseJudgment", "Offensive Spells");
            AddControlInWinForm("Use Hammer of the Righteous", "UseHammerOfTheRighteous", "Offensive Spells");
            AddControlInWinForm("Use Blessed Hammer", "UseBlessedHammer", "Offensive Spells");
            /* Offensive Cooldowns */
            AddControlInWinForm("Use Avenging Wrath", "UseAvengingWrath", "Offensive Cooldowns");
            /* Defensive Spells */
            AddControlInWinForm("Use Shield of the Righteous", "UseShieldOfTheRighteousBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            /* Defensive Cooldowns */
            AddControlInWinForm("Use Ardent Defender", "UseArdentDefenderBelowPercentage", "Defensive Cooldowns", "BelowPercentage", "Life");
            AddControlInWinForm("Use Divine Shield", "UseDivineShieldBelowPercentage", "Defensive Cooldowns", "BelowPercentage", "Life");
            AddControlInWinForm("Use Guardian of Ancient Kings", "UseGuardianOfAncientKingsBelowPercentage", "Defensive Cooldowns", "BelowPercentage", "Life");
            AddControlInWinForm("Use Hammer of Justice", "UseHammerofJusticeBelowPercentage", "Defensive Cooldowns", "BelowPercentage", "Life");
            /* Healing Spells */
            AddControlInWinForm("Use Flash of Light", "UseFlashofLightBelowPercentage", "Healing Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Lay on Hands", "UseLayonHandsBelowPercentage", "Healing Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Light of the Protector", "UseLightoftheProtectorBelowPercentage", "Healing Spells", "BelowPercentage", "Life");
            /* Utility Spells */
            AddControlInWinForm("Use Hand of Reckoning", "UseHandofReckoningBelowToTPercentage", "Utility Spells", "BelowPercentage", "Target of Target Life");
            /* Game Settings */
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");
        }

        public static PaladinProtectionSettings CurrentSetting { get; set; }

        public static PaladinProtectionSettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Paladin_Protection.xml";
            if (File.Exists(currentSettingsFile))
            {
                return
                    CurrentSetting = Load<PaladinProtectionSettings>(currentSettingsFile);
            }
            return new PaladinProtectionSettings();
        }
    }

    #endregion
}

public class PaladinRetribution
{
    private static PaladinRetributionSettings MySettings = PaladinRetributionSettings.GetSettings();

    #region Professions & Racials

    public readonly Spell ArcaneTorrent = new Spell("Arcane Torrent");
    public readonly Spell Berserking = new Spell("Berserking");
    public readonly Spell GiftoftheNaaru = new Spell("Gift of the Naaru");

    public readonly Spell Stoneform = new Spell("Stoneform");
    public readonly Spell WarStomp = new Spell("War Stomp");

    public readonly Spell SanctifiedWrath = new Spell(53376);
    public Timer AvengingWrathTimer = new Timer(0);

    #endregion

    #region Paladin Seals & Buffs

    public readonly Spell GreaterBlessingOfKings = new Spell("Greater Blessing of Kings");
    public readonly Spell GreaterBlessingOfMight = new Spell("Greater Blessing of Might");
    public readonly Spell GreaterBlessingOfWisdom = new Spell("Greater Blessing of Wisdom");

    #endregion

    #region Offensive Spell

    public readonly Spell CrusaderStrike = new Spell("Crusader Strike");
    public readonly Spell BladeOfJustice = new Spell("Blade of Justice");
    public readonly Spell BladeOfWrath = new Spell("Blade of Wrath");
    public readonly Spell DivineHammer = new Spell("Divine Hammer");
    public readonly uint DivinePurposeBuff = 223819;
    public readonly Spell DivineStorm = new Spell("Divine Storm");
    public readonly Spell ExecutionSentence = new Spell("Execution Sentence");
    public readonly Spell JusticarsVengeance = new Spell("Justicar's Vengeance");
    public readonly Spell HammerOfJustice = new Spell("Hammer of Justice");
    public readonly Spell Judgment = new Spell("Judgment");
    public readonly Spell TemplarsVerdict = new Spell("Templar's Verdict");
    public readonly Spell WakeOfAshes = new Spell("Wake of Ashes");
    public readonly Spell AshesToAshes = new Spell("Ashes to Ashes");

    #endregion

    #region Offensive Cooldown

    public readonly Spell AvengingWrath = new Spell("Avenging Wrath");

    #endregion

    #region Defensive Cooldown

    public readonly Spell DivineProtection = new Spell("Divine Protection");
    public readonly Spell DivineShield = new Spell("Divine Shield");
    public readonly Spell HandOfProtection = new Spell("Hand of Protection");
    public readonly Spell Reckoning = new Spell("Reckoning");
    public readonly Spell SacredShield = new Spell("Sacred Shield");

    #endregion

    #region Healing Spell

    public readonly Spell FlashOfLight = new Spell("Flash of Light");
    public readonly Spell LayOnHands = new Spell("Lay on Hands");
    public readonly Spell WordOfGlory = new Spell("Word of Glory");

    #endregion

    #region Flask & Potion Management

    /*
            private readonly int _combatPotion = ItemsManager.GetIdByName(MySettings.CombatPotion);
    */
    private readonly WoWItem _firstTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET);
    private readonly int _flaskOrBattleElixir = ItemsManager.GetItemIdByName(MySettings.FlaskOrBattleElixir);
    private readonly int _guardianElixir = ItemsManager.GetItemIdByName(MySettings.GuardianElixir);

    /*
            private readonly WoWItem _hands = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_HAND);
    */
    private readonly WoWItem _secondTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET, 2);
    /*
            private readonly int _teasureFindingPotion = ItemsManager.GetIdByName(MySettings.TeasureFindingPotion);
    */
    /*
            private readonly int _wellFedBuff = ItemsManager.GetIdByName(MySettings.WellFedBuff);
    */

    #endregion

    public PaladinRetribution()
    {
        Main.InternalRange = ObjectManager.Me.GetCombatReach;
        Main.InternalAggroRange = Main.InternalRange;
        Main.InternalLightHealingSpell = FlashOfLight;
        MySettings = PaladinRetributionSettings.GetSettings();
        Main.DumpCurrentSettings<PaladinRetributionSettings>(MySettings);
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
                            if (ObjectManager.Me.Target != lastTarget && Reckoning.IsHostileDistanceGood)
                            {
                                //Pull();
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
        if (MySettings.UseCrusaderStrike && CrusaderStrike.IsSpellUsable && CrusaderStrike.IsHostileDistanceGood)
        {
            CrusaderStrike.Cast();
            return;
        }
        if (MySettings.UseReckoning && Reckoning.IsSpellUsable && Reckoning.IsHostileDistanceGood)
        {
            Reckoning.Cast();
        }
    }

    private void Combat()
    {
        Buffs();
        DPSBurst();
        if (MySettings.DoAvoidMelee)
            AvoidMelee();
        DPSCycle();
        Heal();
    }

    private void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            if (MySettings.UseFlaskOrBattleElixir && MySettings.FlaskOrBattleElixir != string.Empty)
                if (!SpellManager.HaveBuffLua(ItemsManager.GetItemSpell(MySettings.FlaskOrBattleElixir)) &&
                    !ItemsManager.IsItemOnCooldown(_flaskOrBattleElixir) &&
                    ItemsManager.IsItemUsable(_flaskOrBattleElixir))
                    ItemsManager.UseItem(MySettings.FlaskOrBattleElixir);
            if (MySettings.UseGuardianElixir && MySettings.GuardianElixir != string.Empty)
                if (!SpellManager.HaveBuffLua(ItemsManager.GetItemSpell(MySettings.GuardianElixir)) &&
                    !ItemsManager.IsItemOnCooldown(_guardianElixir) && ItemsManager.IsItemUsable(_guardianElixir))
                    ItemsManager.UseItem(MySettings.GuardianElixir);
            Blessing();
        }
    }

    private void Buffs()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            if (MySettings.UseFlaskOrBattleElixir && MySettings.FlaskOrBattleElixir != string.Empty)
                if (!SpellManager.HaveBuffLua(ItemsManager.GetItemSpell(MySettings.FlaskOrBattleElixir)) &&
                    !ItemsManager.IsItemOnCooldown(_flaskOrBattleElixir) &&
                    ItemsManager.IsItemUsable(_flaskOrBattleElixir))
                    ItemsManager.UseItem(MySettings.FlaskOrBattleElixir);
            if (MySettings.UseGuardianElixir && MySettings.GuardianElixir != string.Empty)
                if (!SpellManager.HaveBuffLua(ItemsManager.GetItemSpell(MySettings.GuardianElixir)) &&
                    !ItemsManager.IsItemOnCooldown(_guardianElixir) && ItemsManager.IsItemUsable(_guardianElixir))
                    ItemsManager.UseItem(MySettings.GuardianElixir);
            Blessing();

            if (MySettings.UseAlchFlask && !ObjectManager.Me.HaveBuff(79638) && !ObjectManager.Me.HaveBuff(79640) && !ObjectManager.Me.HaveBuff(79639)
                && !ItemsManager.IsItemOnCooldown(75525) && ItemsManager.GetItemCount(75525) > 0)
                ItemsManager.UseItem(75525);
        }
    }

    private void Blessing()
    {
        if (ObjectManager.Me.IsMounted || ObjectManager.Me.InCombatBlizzard) // No longer can bless in combat
            return;
        Usefuls.SleepGlobalCooldown();

        if (MySettings.UseGreaterBlessingOfKings && GreaterBlessingOfKings.KnownSpell && !GreaterBlessingOfKings.HaveBuff && GreaterBlessingOfKings.IsSpellUsable)
        {
            GreaterBlessingOfKings.Cast();
            return;
        }
        if (MySettings.UseGreaterBlessingOfMight && GreaterBlessingOfMight.KnownSpell && !GreaterBlessingOfMight.HaveBuff && GreaterBlessingOfMight.IsSpellUsable)
        {
            Logging.Write("If for raiding reasons you need to bless certains party member, disable Greater Blessings in settings and do it manually.");
            GreaterBlessingOfMight.Cast();
            return;
        }
        if (MySettings.UseGreaterBlessingOfWisdom && GreaterBlessingOfWisdom.KnownSpell && !GreaterBlessingOfWisdom.HaveBuff && GreaterBlessingOfWisdom.IsSpellUsable)
        {
            GreaterBlessingOfWisdom.Cast();
            return;
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.HealthPercent < 75 && !ObjectManager.Me.InCombat && !ObjectManager.Me.GetMove && !ObjectManager.Me.IsCast)
        {
            if (FlashOfLight.KnownSpell && FlashOfLight.IsSpellUsable && MySettings.UseFlashOfLight)
            {
                FlashOfLight.CastOnSelf(true, true, true);
                return;
            }
        }
        if (DivineShield.KnownSpell && MySettings.UseDivineShield && ObjectManager.Me.HealthPercent > 0 &&
            ObjectManager.Me.HealthPercent <= 20 && !ObjectManager.Me.HaveBuff(25771) && DivineShield.IsSpellUsable)
        {
            DivineShield.Cast();
            return;
        }
        if (LayOnHands.KnownSpell && MySettings.UseLayOnHands && ObjectManager.Me.HealthPercent > 0 &&
            ObjectManager.Me.HealthPercent <= 20 && !ObjectManager.Me.HaveBuff(25771) && LayOnHands.IsSpellUsable)
        {
            LayOnHands.CastOnSelf();
            return;
        }
        if (HandOfProtection.KnownSpell && MySettings.UseHandOfProtection && ObjectManager.Me.HealthPercent > 0 &&
            ObjectManager.Me.HealthPercent <= 20 && !ObjectManager.Me.HaveBuff(25771) && HandOfProtection.IsSpellUsable)
        {
            HandOfProtection.CastOnSelf();
            return;
        }
        if (ObjectManager.Me.ManaPercentage < 10)
        {
            if (ArcaneTorrent.KnownSpell && MySettings.UseArcaneTorrentForResource && ArcaneTorrent.IsSpellUsable)
            {
                ArcaneTorrent.Cast();
                return;
            }
        }
        if (ObjectManager.Me.HealthPercent > 0 && ObjectManager.Me.HealthPercent < 50)
        {
            if (WordOfGlory.KnownSpell && MySettings.UseWordOfGlory && WordOfGlory.IsSpellUsable && ObjectManager.Me.HolyPower >= 3)
                WordOfGlory.Cast();
            if (FlashOfLight.KnownSpell && MySettings.UseFlashOfLight && FlashOfLight.IsSpellUsable)
            {
                FlashOfLight.CastOnSelf();
                return;
            }
        }
        if (ObjectManager.Me.HealthPercent >= 0 && ObjectManager.Me.HealthPercent < 30)
        {
            if (WordOfGlory.KnownSpell && MySettings.UseWordOfGlory && WordOfGlory.IsSpellUsable && ObjectManager.Me.HolyPower >= 3)
                WordOfGlory.Cast();
            if (DivineProtection.KnownSpell && MySettings.UseDivineProtection && DivineProtection.IsSpellUsable)
                DivineProtection.Cast();
            if (FlashOfLight.KnownSpell && MySettings.UseFlashOfLight && FlashOfLight.IsSpellUsable)
            {
                FlashOfLight.CastOnSelf();
            }
        }
    }

    private void DPSBurst()
    {
        if (MySettings.UseAvengingWrath && Judgment.TargetHaveBuff && !AvengingWrath.HaveBuff && AvengingWrath.IsSpellUsable)
        {
            AvengingWrath.Cast();
        }
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
    }

    private void DPSCycle()
    {
        Usefuls.SleepGlobalCooldown();
        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            if (WakeOfAshes.KnownSpell && (ObjectManager.Me.HolyPower == 0 || !AshesToAshes.HaveBuff) && WakeOfAshes.IsSpellUsable && WakeOfAshes.IsHostileDistanceGood)
            {
                WakeOfAshes.Cast(); // Artefact spell.
                return;
            }
            if (MySettings.UseJusticarsVengeance && ObjectManager.Me.HaveBuff(DivinePurposeBuff) &&
                (!MySettings.UseDivineStorm || !DivineStorm.IsSpellUsable || ObjectManager.GetUnitInSpellRange(DivineStorm.MaxRangeHostile) < 3) &&
                JusticarsVengeance.IsSpellUsable && JusticarsVengeance.IsHostileDistanceGood)
            {
                JusticarsVengeance.Cast();
                return;
            }
            if (MySettings.UseExecutionSentence && (!MySettings.UseJudgment || !Judgment.TargetHaveBuff) && (ObjectManager.Me.HaveBuff(DivinePurposeBuff) ||
                                                                                                             ObjectManager.Me.HolyPower >= 3) && ExecutionSentence.IsSpellUsable &&
                ExecutionSentence.IsHostileDistanceGood)
            {
                // don't cast if target have judgment buff because it's mean it will be expired when Sentence hit. If Judgement just faded, is no issues since we can recast it before the end of Sentence.
                ExecutionSentence.Cast();
                return;
            }
            if (MySettings.UseJudgment && (ObjectManager.Target.AuraIsActiveAndExpireInLessThanMs(ExecutionSentence.Id, 2000) || ObjectManager.Me.HolyPower == 5) && Judgment.IsSpellUsable &&
                Judgment.IsHostileDistanceGood)
            {
                // We cast judgment before ExecutionSentence deals its damages for 50% more damages.
                // We do 3 Holy Power worth of generation.
                Judgment.Cast();
                return;
            }
            if (((!MySettings.UseDivineStorm && MySettings.UseTemplarsVerdict && TemplarsVerdict.IsSpellUsable) ||
                 (TemplarsVerdict.IsSpellUsable && ObjectManager.GetUnitInSpellRange(DivineStorm.MaxRangeHostile) <= 2)) &&
                ((ObjectManager.Me.HaveBuff(DivinePurposeBuff) || (ObjectManager.Me.HolyPower == 5 || (Judgment.TargetHaveBuff && ObjectManager.Me.HolyPower >= 3))) &&
                 TemplarsVerdict.IsHostileDistanceGood))
            {
                TemplarsVerdict.Cast();
                return;
            }
            if (((MySettings.UseDivineStorm && !MySettings.UseTemplarsVerdict && DivineStorm.IsSpellUsable) || (DivineStorm.IsSpellUsable && ObjectManager.GetUnitInSpellRange(DivineStorm.MaxRangeHostile) >= 3)) &&
                ((ObjectManager.Me.HaveBuff(DivinePurposeBuff) || (ObjectManager.Me.HolyPower == 5 || Judgment.TargetHaveBuff && ObjectManager.Me.HolyPower >= 3))))
            {
                DivineStorm.Cast();
                return;
            }
            if (MySettings.UseCrusaderStrike && ObjectManager.Me.HolyPower < 5 && CrusaderStrike.IsSpellUsable && CrusaderStrike.IsHostileDistanceGood && CrusaderStrike.GetSpellCharges == 2)
            {
                // burn first CS Charge before any blade.
                CrusaderStrike.Cast();
                return;
            }
            if (MySettings.UseBladeOfJustice && ObjectManager.Me.HolyPower < 4 && BladeOfJustice.IsSpellUsable && BladeOfJustice.IsHostileDistanceGood)
            {
                BladeOfJustice.Cast();
                return;
            }
            if (MySettings.UseBladeOfWrath && ObjectManager.Me.HolyPower < 4 && BladeOfWrath.IsSpellUsable && BladeOfWrath.IsHostileDistanceGood)
            {
                BladeOfWrath.Cast();
                return;
            }
            if (MySettings.UseDivineHammer && ObjectManager.Me.HolyPower < 4 && DivineHammer.IsSpellUsable && DivineHammer.IsHostileDistanceGood)
            {
                DivineHammer.Cast();
                return;
            }
            if (MySettings.UseCrusaderStrike && ObjectManager.Me.HolyPower < 5 && CrusaderStrike.IsSpellUsable && CrusaderStrike.IsHostileDistanceGood)
            {
                CrusaderStrike.Cast();
                return;
            }
            if ((!MySettings.UseDivineStorm && MySettings.UseTemplarsVerdict && TemplarsVerdict.IsSpellUsable) ||
                (TemplarsVerdict.IsSpellUsable && ObjectManager.GetUnitInSpellRange(DivineStorm.MaxRangeHostile) <= 2) && TemplarsVerdict.IsHostileDistanceGood)
            {
                TemplarsVerdict.Cast();
                return;
            }
            if ((MySettings.UseDivineStorm && !MySettings.UseTemplarsVerdict && DivineStorm.IsSpellUsable) ||
                (DivineStorm.IsSpellUsable && ObjectManager.GetUnitInSpellRange(DivineStorm.MaxRangeHostile) > 2) && DivineStorm.IsHostileDistanceGood)
            {
                DivineStorm.Cast();
                return;
            }
            if (MySettings.UseJudgment && Judgment.IsSpellUsable && Judgment.IsHostileDistanceGood)
            {
                Judgment.Cast(); // fill with Judgment for low level paladins.
                return;
            }
            if (MySettings.UseHammerOfJustice && HammerOfJustice.IsSpellUsable && ObjectManager.Target.IsStunnable && HammerOfJustice.IsHostileDistanceGood)
            {
                HammerOfJustice.Cast();
                return;
            }
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
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

    #region Nested type: PaladinRetributionSettings

    [Serializable]
    public class PaladinRetributionSettings : Settings
    {
        public string CombatPotion = "Potion of Mogu Power";
        public bool DoAvoidMelee = false;
        public int DoAvoidMeleeDistance = 0;
        public string FlaskOrBattleElixir = "Flask of Winter's Bite";
        public string GuardianElixir = "";
        public bool RefreshWeakenedBlows = true;
        public string TeasureFindingPotion = "Potion of Luck";
        public bool UseAlchFlask = true;
        public bool UseArcaneTorrentForDecast = true;
        public int UseArcaneTorrentForDecastAtPercentage = 100;
        public bool UseArcaneTorrentForResource = true;
        public int UseArcaneTorrentForResourceAtPercentage = 80;
        public bool UseAvengingWrath = true;
        public bool UseBerserking = true;
        public bool UseGreaterBlessingOfKings = true;
        public bool UseGreaterBlessingOfMight = true;
        public bool UseGreaterBlessingOfWisdom = true;
        public bool UseCombatPotion = false;
        public bool UseCrusaderStrike = true;
        public bool UseBladeOfWrath = true;
        public bool UseBladeOfJustice = true;
        public bool UseDivineHammer = true;
        public bool UseDivineProtection = true;
        public bool UseDivineShield = true;
        public bool UseDivineStorm = true;
        public bool UseExecutionSentence = true;
        public bool UseJusticarsVengeance = true;
        public bool UseFlashOfLight = true;
        public bool UseFlaskOrBattleElixir = false;
        public bool UseGiftoftheNaaru = true;
        public int UseGiftoftheNaaruAtPercentage = 80;
        public bool UseGuardianElixir = false;
        public bool UseHammerOfJustice = true;
        public bool UseHandOfProtection = false;
        public bool UseHolyAvenger = true;
        public bool UseJudgment = true;
        public bool UseLayOnHands = true;

        public bool UseReckoning = true;
        public bool UseSacredShield = true;
        public bool UseStoneform = true;
        public int UseStoneformAtPercentage = 80;
        public bool UseTeasureFindingPotion = false;
        public bool UseTemplarsVerdict = true;
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;
        public bool UseWarStomp = true;
        public int UseWarStompAtPercentage = 80;
        public bool UseWellFedBuff = false;
        public bool UseWordOfGlory = true;

        public string WellFedBuff = "Sleeper Sushi";

        public PaladinRetributionSettings()
        {
            ConfigWinForm("Paladin Retribution Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent for Interrupt", "UseArcaneTorrentForDecast", "Professions & Racials", "AtPercentage");
            AddControlInWinForm("Use Arcane Torrent for Resource", "UseArcaneTorrentForResource", "Professions & Racials", "AtPercentage");

            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            /* Paladin Seals & Buffs */
            AddControlInWinForm("Use Greater Blessing of Might", "UseGreaterBlessingOfMight", "Paladin Blessings");
            AddControlInWinForm("Use Greater Blessing of Kings", "UseGreaterBlessingOfKings", "Paladin Blessings");
            AddControlInWinForm("Use Greater Blessing of Wisdom", "UseGreaterBlessingOfWisdom", "Paladin Blessings");
            /* Offensive Spell */
            AddControlInWinForm("Use Templar's Verdict", "UseTemplarsVerdict", "Offensive Spell");
            AddControlInWinForm("Use Justicar's Vengeance", "UseJusticarsVengeance", "Offensive Spell");
            AddControlInWinForm("Use Divine Storm", "UseDivineStorm", "Offensive Spell");
            AddControlInWinForm("Use Crusader Strike", "UseCrusaderStrike", "Offensive Spell");
            AddControlInWinForm("Use Blade of Wrath", "UseBladeOfWrath", "Offensive Spell");
            AddControlInWinForm("Use Blade of Justice", "UseBladeOfJustice", "Offensive Spell");
            AddControlInWinForm("Use Divine Hammer", "UseDivineHammer", "Offensive Spell");
            AddControlInWinForm("Use Judgment", "UseJudgment", "Offensive Spell");
            AddControlInWinForm("Use Hammer of Justice", "UseHammerOfJustice", "Offensive Spell");
            AddControlInWinForm("Use Execution Sentence", "UseExecutionSentence", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use Avenging Wrath", "UseAvengingWrath", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Reckoning", "UseReckoning", "Defensive Cooldown");
            AddControlInWinForm("Use Divine Protection", "UseDivineProtection", "Defensive Cooldown");
            AddControlInWinForm("Use Sacred Shield", "UseSacredShield", "Defensive Cooldown");
            AddControlInWinForm("Use Divine Shield", "UseDivineShield", "Defensive Cooldown");
            AddControlInWinForm("Use Hand of Protection", "UseHandOfProtection", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Flash of Light", "UseFlashOfLight", "Healing Spell");
            AddControlInWinForm("Use Lay on Hands", "UseLayOnHands", "Healing Spell");
            AddControlInWinForm("Use Word of Glory", "UseWordOfGlory", "Healing Spell");
            /* Flask & Potion Management */
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");
            AddControlInWinForm("Use Flask or Battle Elixir", "UseFlaskOrBattleElixir", "Flask & Potion Management");
            AddControlInWinForm("Flask or Battle Elixir Name", "FlaskOrBattleElixir", "Flask & Potion Management");
            AddControlInWinForm("Use Guardian Elixir", "UseGuardianElixir", "Flask & Potion Management");
            AddControlInWinForm("Guardian Elixir Name", "GuardianElixir", "Flask & Potion Management");
            AddControlInWinForm("Use Combat Potion", "UseCombatPotion", "Flask & Potion Management");
            AddControlInWinForm("Combat Potion Name", "CombatPotion", "Flask & Potion Management");
            AddControlInWinForm("Use Teasure Finding Potion", "UseTeasureFindingPotion", "Flask & Potion Management");
            AddControlInWinForm("Teasure Finding Potion Name", "TeasureFindingPotion", "Flask & Potion Management");
            AddControlInWinForm("Use Well Fed Buff", "UseWellFedBuff", "Flask & Potion Management");
            AddControlInWinForm("Well Fed Buff Name", "WellFedBuff", "Flask & Potion Management");
            AddControlInWinForm("Do avoid melee (Off Advised!!)", "DoAvoidMelee", "Game Settings");
            AddControlInWinForm("Avoid melee distance (1 to 4)", "DoAvoidMeleeDistance", "Game Settings");
        }

        public static PaladinRetributionSettings CurrentSetting { get; set; }

        public static PaladinRetributionSettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Paladin_Retribution.xml";
            if (File.Exists(currentSettingsFile))
            {
                return
                    CurrentSetting = Load<PaladinRetributionSettings>(currentSettingsFile);
            }
            return new PaladinRetributionSettings();
        }
    }

    #endregion
}

#endregion

// ReSharper restore ObjectCreationAsStatement
// ReSharper restore EmptyGeneralCatchClause