/*
* CombatClass template for TheNoobBot
* Credit : Vesper, Ryuichiro
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

public class Main : ICombatClass
{
    internal static float InternalRange = 5.0f;
    internal static float InternalAggroRange = 5.0f;
    internal static bool InternalLoop = true;
    internal static Spell InternalLightHealingSpell;

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
                #region TEMPLATE_CLASS Specialisation checking

                case WoWClass.TEMPLATE_CLASS:

                if (wowSpecialization == WoWSpecialization.TEMPLATE_SPEC || wowSpecialization == WoWSpecialization.None)
                {
                    if (configOnly)
                    {
                        string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\TEMPLATE_SPEC.xml";
                        var currentSetting = new TEMPLATE_SPEC.TEMPLATE_SPECSettings();
                        if (File.Exists(currentSettingsFile) && !resetSettings)
                        {
                            currentSetting = Settings.Load<TEMPLATE_SPEC.TEMPLATE_SPECSettings>(currentSettingsFile);
                        }
                        currentSetting.ToForm();
                        currentSetting.Save(currentSettingsFile);
                    }
                    else
                    {
                        Logging.WriteFight("Loading TEMPLATE_SPEC Combat class...");
                        EquipmentAndStats.SetPlayerSpe(WoWSpecialization.TEMPLATE_SPEC);
                        new TEMPLATE_CLASS();
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
        mySettings = mySettings is T ? (T)mySettings : default(T);
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

#region TEMPLATE_CLASS

public class TEMPLATE_SPEC
{
    private static TEMPLATE_SPECSettings MySettings = TEMPLATE_SPECSettings.GetSettings();

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

    private readonly Spell TEMPLATETALENT = new Spell("TEMPLATE_TALENT");

    #endregion

    #region Buffs

    private readonly Spell TEMPLATEBUFF = new Spell(TEMPLATE_BUFF_ID);

    #endregion

    #region Dots

    private readonly Spell TEMPLATEDOT = new Spell(TEMPLATE_DOT_ID);

    #endregion

    #region Artifact Spells

    private readonly Spell TEMPLATEARTIFACTSPELL = new Spell("TEMPLATE_ARTIFACT_SPELL");

    #endregion

    #region Offensive Spells

    private readonly Spell TEMPLATEOFFENSIVESPELL = new Spell("TEMPLATE_OFFENSIVE_SPELL");
    private Timer TEMPLATEOFFENSIVESPELLTimer = new Timer(0);

    #endregion

    #region Offensive Cooldowns

    private readonly Spell TEMPLATEOFFENSIVEBUFFSPELL = new Spell("TEMPLATE_OFFENSIVE_BUFF_SPELL");

    #endregion

    #region Defensive Spells

    private readonly Spell TEMPLATEDEFENSIVESPELL = new Spell("TEMPLATE_DEFENSIVE_SPELL");
    private readonly Spell TEMPLATEEMERGENCYDEFENSIVESPELL = new Spell("TEMPLATE_EMERGENCY_DEFENSIVE_SPELL");

    #endregion

    #region Healing Spell

    private readonly Spell TEMPLATEHEALINGSPELL = new Spell("TEMPLATE_HEALING_SPELL");

    #endregion

    #region Utility Spells

    private readonly Spell TEMPLATEAGGROSPELL = new Spell("TEMPLATE_AGGRO_SPELL");
    private readonly Spell TEMPLATESTUNSPELL = new Spell("TEMPLATE_STUN_SPELL");
    private readonly Spell TEMPLATEMOVEMENTSPELL = new Spell("TEMPLATE_MOVEMENT_SPELL");

    #endregion

    public TEMPLATE_SPEC()
    {
        Main.InternalRange = TEMPLATE_COMBAT_RANGE; // Max range of your combat spells (40f for most classes, ObjectManager.Me.GetCombatReach for melee)
        Main.InternalAggroRange = TEMPLATE_PULL_RANGE; // Range at which your RANGED class should engage enemies remove if your class is melee
        Main.InternalLightHealingSpell = null; /*new Spell("TEMPLATE_HEALING_SPELL");*/
        MySettings = TEMPLATE_SPECSettings.GetSettings();
        Main.DumpCurrentSettings<TEMPLATE_SPECSettings>(MySettings);
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

        if (ObjectManager.Me.GetMove)
        {
            //Movement Buffs
            if (!Darkflight.HaveBuff) // doesn't stack
            {
                if (MySettings.UseDarkflight && Darkflight.IsSpellUsable)
                {
                    Darkflight.Cast();
                    return;
                }
                if (MySettings.UseTEMPLATEMOVEMENTSPELL && TEMPLATEMOVEMENTSPELL.IsSpellUsable && !TEMPLATEMOVEMENTSPELL.HaveBuff)
                {
                    TEMPLATEMOVEMENTSPELL.Cast();
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
            //TEMPLATE_HEALING_SPELL
            if (ObjectManager.Me.HealthPercent < MySettings.UseTEMPLATEHEALINGSPELLBelowPercentage && TEMPLATEHEALINGSPELL.IsSpellUsable)
            {
                TEMPLATEHEALINGSPELL.CastOnSelf();
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
                        StunTimer = new Timer(1000 * 2.5);
                        return true;
                    }
                    //TEMPLATE_STUN_SPELL
                    if (ObjectManager.Me.HealthPercent < MySettings.UseTEMPLATESTUNSPELLBelowPercentage && TEMPLATESTUNSPELL.IsSpellUsable)
                    {
                        TEMPLATESTUNSPELL.Cast();
                        StunTimer = new Timer(1000 * TEMPLATE_STUN_TIME);// Time until the enemy will leave the Stun
                        return true;
                    }
                }
                //Mitigate Damage
                if (ObjectManager.Me.HealthPercent < MySettings.UseStoneformBelowPercentage && Stoneform.IsSpellUsable)
                {
                    Stoneform.Cast();
                    DefensiveTimer = new Timer(1000 * 8);
                    return true;
                }
                //TEMPLATE_DEFENSIVE_SPELL
                if (ObjectManager.Me.HealthPercent < MySettings.UseTEMPLATEDEFENSIVESPELLBelowPercentage && TEMPLATEDEFENSIVESPELL.IsSpellUsable)
                {
                    TEMPLATEDEFENSIVESPELL.Cast();
                    DefensiveTimer = new Timer(1000 * TEMPLATE_DEFENSIVE_BUFF_TIME);// Time until the Buff ends
                    return true;
                }
            }
            //Mitigate Damage in Emergency Situations
            //TEMPLATE_EMERGENCY_DEFENSIVE_SPELL
            if (ObjectManager.Me.HealthPercent < MySettings.UseTEMPLATEEMERGENCYDEFENSIVESPELLBelowPercentage && TEMPLATEEMERGENCYDEFENSIVESPELL.IsSpellUsable)
            {
                TEMPLATEEMERGENCYDEFENSIVESPELL.Cast();
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
            //TEMPLATE_OFFENSIVE_BUFF_SPELL
            if (MySettings.UseTEMPLATEOFFENSIVEBUFFSPELL && TEMPLATEOFFENSIVEBUFFSPELL.IsSpellUsable && !TEMPLATEOFFENSIVEBUFFSPELL.HaveBuff)
            {
                TEMPLATEOFFENSIVEBUFFSPELL.Cast();
                return true; // only return if Global Cooldown is triggered
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

            //TEMPLATE_AGGRO_SPELL
            if (MySettings.UseTEMPLATEAGGROSPELL && TEMPLATEAGGROSPELL.IsSpellUsable && TEMPLATEAGGROSPELL.IsHostileDistanceGood &&
                ObjectManager.Target.Target != ObjectManager.Me.Guid)
            {
                TEMPLATEAGGROSPELL.Cast();
                return true;
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

            // Independant of Target Count
            if (MySettings.UseTEMPLATEOFFENSIVESPELL && TEMPLATEOFFENSIVESPELL.IsSpellUsable && TEMPLATEOFFENSIVESPELL.IsHostileDistanceGood &&
                ObjectManager.Me.CLASS_RESOURCE > MySettings.UseTEMPLATEOFFENSIVESPELLAbovePercentage && // Resource Check
                TEMPLATEBUFF.HaveBuff && // Buff Check
                TEMPLATEDOT.TargetHaveBuff && // Buff Check
                (ObjectManager.Me.GetUnitInSpellRange(5f) > 1 || ObjectManager.Target.GetUnitInSpellRange(5f) > 1) && // Unit Count Check
                TEMPLATEOFFENSIVESPELLTimer.IsReady) // Timer Check
            {
                TEMPLATEOFFENSIVESPELL.Cast();
                TEMPLATEOFFENSIVESPELLTimer = new Timer(1000 * 10); // for timings that don't depend on a buff
                return;
            }
            if (MySettings.UseTEMPLATEARTIFACTSPELL && TEMPLATEARTIFACTSPELL.IsSpellUsable && TEMPLATEARTIFACTSPELL.IsHostileDistanceGood)
            {
                TEMPLATEARTIFACTSPELL.Cast();
                return;
            }

            //Single Target 
            if (ObjectManager.Target.GetUnitInSpellRange(5f) == 1)
            {
            }
            //Multiple Targets
            else
            {
            }
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    #region Nested type: TEMPLATE_SPECSettings

    [Serializable]
    public class TEMPLATE_SPECSettings : Settings
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
        public bool UseTEMPLATEARTIFACTSPELL = true;

        /* Offensive Spells */
        public bool UseTEMPLATEOFFENSIVESPELL = true;
        public int UseTEMPLATEOFFENSIVESPELLAbovePercentage = 50;

        /* Offensive Cooldowns */
        public bool UseTEMPLATEOFFENSIVEBUFFSPELL = true;

        /* Defensive Spells */
        public int UseTEMPLATEDEFENSIVESPELLBelowPercentage = 50;
        public int UseTEMPLATEEMERGENCYDEFENSIVESPELLBelowPercentage = 10;

        /* Healing Spells */
        public bool UseTEMPLATEHEALINGSPELLOOC = true;
        public int UseTEMPLATEHEALINGSPELLBelowPercentage = 25;

        /* Utility Spells */
        public bool UseTEMPLATEAGGROSPELL = true;
        public bool UseTEMPLATEMOVEMENTSPELL = true;

        /* Game Settings */
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;

        public TEMPLATE_SPECSettings()
        {
            ConfigWinForm("TEMPLATE_SPEC Settings");
            /* Professions & Racials */
            //AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Darkflight", "UseDarkflight", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaruBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            AddControlInWinForm("Use Stone Form", "UseStoneformBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            AddControlInWinForm("Use War Stomp", "UseWarStompBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            /* Artifact Spells */
            AddControlInWinForm("Use TEMPLATE_ARTIFACT_SPELL", "UseTEMPLATEARTIFACTSPELL", "Artifact Spells");
            /* Offensive Spells */
            AddControlInWinForm("Use TEMPLATE_OFFENSIVE_SPELL", "UseTEMPLATEOFFENSIVESPELL", "Offensive Spells");
            AddControlInWinForm("Use TEMPLATE_OFFENSIVE_SPELL", "UseTEMPLATEOFFENSIVESPELLAbovePercentage", "Offensive Spells", "AbovePercentage", "CLASS_RESOURCE");
            /* Offensive Cooldowns */
            AddControlInWinForm("Use TEMPLATE_OFFENSIVE_BUFF_SPELL", "UseTEMPLATEOFFENSIVEBUFFSPELL", "Offensive Cooldowns");
            /* Defensive Spells */
            AddControlInWinForm("Use TEMPLATE_DEFENSIVE_SPELL", "UseTEMPLATEDEFENSIVESPELLBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            /* Healing Spell */
            AddControlInWinForm("Use TEMPLATE_HEALING_SPELL out of Combat", "UseTEMPLATEHEALINGSPELLOOC", "Healing Spells");
            AddControlInWinForm("Use TEMPLATE_HEALING_SPELL", "UseTEMPLATEHEALINGSPELLBelowPercentage", "Healing Spells", "BelowPercentage", "Life");
            /* Utility Spells */
            AddControlInWinForm("Use TEMPLATE_AGGRO_SPELL", "UseTEMPLATEAGGROSPELL", "Utility Spells");
            AddControlInWinForm("Use TEMPLATE_MOVEMENT_SPELL", "UseTEMPLATEMOVEMENTSPELL", "Utility Spells");
            /* Game Settings */
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");
        }

        public static TEMPLATE_SPECSettings CurrentSetting { get; set; }

        public static TEMPLATE_SPECSettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\TEMPLATE_SPEC.xml";
            if (File.Exists(currentSettingsFile))
            {
                return
                    CurrentSetting = Load<TEMPLATE_SPECSettings>(currentSettingsFile);
            }
            return new TEMPLATE_SPECSettings();
        }
    }

    #endregion
}

#endregion