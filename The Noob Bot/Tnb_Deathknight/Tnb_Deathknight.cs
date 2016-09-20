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
                    #region DeathKnight Specialisation checking

                case WoWClass.DeathKnight:

                    if (wowSpecialization == WoWSpecialization.DeathknightBlood)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Deathknight_Blood.xml";
                            var currentSetting = new DeathknightBlood.DeathknightBloodSettings();
                            if (File.Exists(currentSettingsFile) && !resetSettings)
                            {
                                currentSetting = Settings.Load<DeathknightBlood.DeathknightBloodSettings>(currentSettingsFile);
                            }
                            currentSetting.ToForm();
                            currentSetting.Save(currentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Deathknight Blood Combat class...");
                            InternalRange = 5.0f;
                            EquipmentAndStats.SetPlayerSpe(WoWSpecialization.DeathknightBlood);
                            new DeathknightBlood();
                        }
                        break;
                    }
                    if (wowSpecialization == WoWSpecialization.DeathknightUnholy || wowSpecialization == WoWSpecialization.None)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Deathknight_Unholy.xml";
                            var currentSetting = new DeathknightUnholy.DeathknightUnholySettings();
                            if (File.Exists(currentSettingsFile) && !resetSettings)
                            {
                                currentSetting = Settings.Load<DeathknightUnholy.DeathknightUnholySettings>(currentSettingsFile);
                            }
                            currentSetting.ToForm();
                            currentSetting.Save(currentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Deathknight Unholy Combat class...");
                            InternalRange = 5.0f;
                            EquipmentAndStats.SetPlayerSpe(WoWSpecialization.DeathknightUnholy);
                            new DeathknightUnholy();
                        }
                        break;
                    }
                    if (wowSpecialization == WoWSpecialization.DeathknightFrost)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Deathknight_Frost.xml";
                            var currentSetting = new DeathknightFrost.DeathknightFrostSettings();
                            if (File.Exists(currentSettingsFile) && !resetSettings)
                            {
                                currentSetting = Settings.Load<DeathknightFrost.DeathknightFrostSettings>(currentSettingsFile);
                            }
                            currentSetting.ToForm();
                            currentSetting.Save(currentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Deathknight Frost Combat class...");
                            InternalRange = 5.0f;
                            EquipmentAndStats.SetPlayerSpe(WoWSpecialization.DeathknightFrost);
                            new DeathknightFrost();
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

#region Deathknight

public class DeathknightBlood
{
    private static DeathknightBloodSettings MySettings = DeathknightBloodSettings.GetSettings();

    #region General Timers & Variables

    private readonly WoWItem _firstTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET);
    private readonly WoWItem _secondTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET, 2);

    private bool CombatMode = true;

    private Timer DefensiveTimer = new Timer(0);
    private Timer StunTimer = new Timer(0);

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

    #region Buffs

    private readonly Spell BoneShield = new Spell(195181);
    private readonly Spell DarkSuccor = new Spell(101568);
    private readonly Spell CrimsonScourge = new Spell(81141);

    #endregion

    #region Dots

    private readonly Spell BloodPlague = new Spell(55078);

    #endregion

    #region Legion Artifact Spells

    private readonly Spell Consumption = new Spell("Consumption");

    #endregion

    #region Offensive Spell

    private readonly Spell BloodBoil = new Spell("Blood Boil");
    private readonly Spell DeathsCaress = new Spell("Death's Caress");
    private readonly Spell DeathandDecay = new Spell("Death and Decay");
    private readonly Spell DeathStrike = new Spell("Death Strike");
    private readonly Spell HearthStrike = new Spell("Hearth Strike");
    private readonly Spell Marrowrend = new Spell("Marrowrend");

    #endregion

    #region Defensive Spells

    private readonly Spell AntiMagicShell = new Spell("Anti-Magic Shell");
    private readonly Spell BloodMirror = new Spell("Blood Mirror");
    private readonly Spell DancingRuneWeapon = new Spell("Dancing Rune Weapon");
    private readonly Spell MarkofBlood = new Spell("Mark of Blood");
    private readonly Spell RuneTap = new Spell("Rune Tap");
    private readonly Spell Tombstone = new Spell("Tombstone");
    private readonly Spell VampiricBlood = new Spell("Vampiric Blood");

    #endregion

    #region Healing Spells

    private readonly Spell Blooddrinker = new Spell("Blooddrinker");
    private readonly Spell Bonestorm = new Spell("Bonestorm");

    #endregion

    #region Utility Spells

    private readonly Spell Asphyxiate = new Spell("Asphyxiate");
    private readonly Spell BloodTap = new Spell("Blood Tap");
    private readonly Spell DarkCommand = new Spell("Dark Command");
    private readonly Spell DeathGrip = new Spell("Death Grip");
    private readonly Spell GorefiendsGrasp = new Spell("Gorefriend's Grasp");
    //private readonly Spell MindFreeze = new Spell("Mind Freeze");
    private readonly Spell PathofFrost = new Spell("Path of Frost");
    private readonly Spell RaiseAlly = new Spell("Raise Ally");
    private readonly Spell Soulgorge = new Spell("Soulgorge");
    private readonly Spell WraithWalk = new Spell("Wraith Walk");

    #endregion

    public DeathknightBlood()
    {
        Main.InternalRange = ObjectManager.Me.GetCombatReach;
        Main.InternalLightHealingSpell = null;
        MySettings = DeathknightBloodSettings.GetSettings();
        Main.DumpCurrentSettings<DeathknightBloodSettings>(MySettings);
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
            if (!Darkflight.HaveBuff && !WraithWalk.HaveBuff) // doesn't stack
            {
                if (MySettings.UseDarkflight && Darkflight.IsSpellUsable)
                {
                    Darkflight.Cast();
                    return;
                }
                if (MySettings.UseWraithWalk && WraithWalk.IsSpellUsable)
                {
                    WraithWalk.Cast();
                    return;
                }
            }
            if (MySettings.UsePathofFrost && PathofFrost.IsSpellUsable && !PathofFrost.HaveBuff)
            {
                PathofFrost.Cast();
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

    // For general InFight Behavior (only touch if you want to add a new method like PetManagement())
    private void Combat()
    {
        //Log
        if (!CombatMode)
        {
            Logging.WriteFight("Combat:");
            CombatMode = true;
        }
        if (Healing() || Defensive() || AggroManagement() || Offensive())
            return;
        Rotation();
    }

    // For Healing Spells (always return after Casting)
    private bool Healing()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            //Gift of the Naaru
            if (ObjectManager.Me.HealthPercent < MySettings.UseGiftoftheNaaruBelowPercentage && GiftoftheNaaru.IsSpellUsable)
            {
                GiftoftheNaaru.Cast();
                return true;
            }
            //Blooddrinker
            if (ObjectManager.Me.HealthPercent < MySettings.UseBlooddrinkerBelowPercentage && Blooddrinker.IsSpellUsable && Blooddrinker.IsHostileDistanceGood)
            {
                Blooddrinker.Cast();
                return true;
            }
            //Blooddrinker
            if (ObjectManager.Me.HealthPercent < MySettings.UseBonestormBelowPercentage && Bonestorm.IsSpellUsable /*&& Bonestorm.IsHostileDistanceGood*/)
            {
                Bonestorm.Cast();
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
                    if (ObjectManager.Me.HealthPercent < MySettings.UseAsphyxiateBelowPercentage && Asphyxiate.IsSpellUsable)
                    {
                        Asphyxiate.Cast();
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
                if (ObjectManager.Me.HealthPercent < MySettings.UseBloodMirrorBelowPercentage && BloodMirror.IsSpellUsable && BloodMirror.IsHostileDistanceGood)
                {
                    BloodMirror.Cast();
                    DefensiveTimer = new Timer(1000*10);
                    return true;
                }
                if (ObjectManager.Me.HealthPercent < MySettings.UseDancingRuneWeaponBelowPercentage && DancingRuneWeapon.IsSpellUsable)
                {
                    DancingRuneWeapon.Cast();
                    DefensiveTimer = new Timer(1000*8);
                    return true;
                }
                if (ObjectManager.Me.HealthPercent < MySettings.UseMarkofBloodBelowPercentage && MarkofBlood.IsSpellUsable && MarkofBlood.IsHostileDistanceGood)
                {
                    MarkofBlood.Cast();
                    DefensiveTimer = new Timer(1000*25);
                    return true;
                }
                if (ObjectManager.Me.HealthPercent < MySettings.UseRuneTapBelowPercentage && RuneTap.IsSpellUsable)
                {
                    RuneTap.Cast();
                    DefensiveTimer = new Timer(1000*3);
                    return true;
                }
                if (ObjectManager.Me.HealthPercent < MySettings.UseTombstoneBelowPercentage && Tombstone.IsSpellUsable)
                {
                    Tombstone.Cast();
                    DefensiveTimer = new Timer(1000*8);
                    return true;
                }
                if (ObjectManager.Me.HealthPercent < MySettings.UseVampiricBloodBelowPercentage && VampiricBlood.IsSpellUsable)
                {
                    VampiricBlood.Cast();
                    DefensiveTimer = new Timer(1000*10);
                    return true;
                }
            }
            //Mitigate Damage in Emergency Situations
            if (ObjectManager.Me.HealthPercent < MySettings.UseAntiMagicShellBelowPercentage && AntiMagicShell.IsSpellUsable)
            {
                AntiMagicShell.Cast();
                DefensiveTimer = new Timer(1000*5);
                return true;
            }
            return false;
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    // For Offensive Buffs (only return if a Cast triggered Global Spells)
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

    // For Spots (always return after Casting)
    private bool AggroManagement()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            //Dark Command
            if (MySettings.UseDarkCommand && DarkCommand.IsSpellUsable && DarkCommand.IsHostileDistanceGood &&
                ObjectManager.Target.Target != ObjectManager.Me.Guid)
            {
                DarkCommand.Cast();
                return true;
            }
            //Death Grip
            if (MySettings.UseDeathGrip && DeathGrip.IsSpellUsable && DeathGrip.IsHostileDistanceGood &&
                ObjectManager.Target.Target != ObjectManager.Me.Guid)
            {
                DeathGrip.Cast();
                return true;
            }
            //Gorefiend's Grasp
            if (MySettings.UseGorefiendsGrasp && GorefiendsGrasp.IsSpellUsable && GorefiendsGrasp.IsHostileDistanceGood &&
                ObjectManager.Target.Target != ObjectManager.Me.Guid)
            {
                GorefiendsGrasp.Cast();
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

            if (MySettings.UseBloodBoil && BloodBoil.IsSpellUsable && BloodBoil.IsHostileDistanceGood && !BloodPlague.TargetHaveBuff)
            {
                BloodBoil.Cast();
                return;
            }
            if (MySettings.UseMarrowrend && Marrowrend.IsSpellUsable && Marrowrend.IsHostileDistanceGood && BoneShield.BuffStack == 0)
            {
                Marrowrend.Cast();
                return;
            }
            if (ObjectManager.Me.HealthPercent < MySettings.UseDeathStrikeBelowPercentage && DeathStrike.IsSpellUsable && DeathStrike.IsHostileDistanceGood &&
                DarkSuccor.HaveBuff)
            {
                DeathStrike.Cast();
                return;
            }
            if (MySettings.UseConsumption && Consumption.IsSpellUsable && Consumption.IsHostileDistanceGood)
            {
                Consumption.Cast();
                return;
            }

            //Single Target 
            if (ObjectManager.Target.GetUnitInSpellRange(5f) == 1)
            {
                if (MySettings.UseDeathandDecay && DeathandDecay.IsSpellUsable && DeathandDecay.IsHostileDistanceGood && CrimsonScourge.HaveBuff)
                {
                    DeathandDecay.CastAtPosition(ObjectManager.Target.Position);
                    return;
                }
                if (BoneShield.BuffStack < 6 || ObjectManager.Me.UnitAura(BoneShield.Id, ObjectManager.Me.Guid).AuraTimeLeftInMs < 2000)
                {
                    if (MySettings.UseMarrowrend && Marrowrend.IsSpellUsable && Marrowrend.IsHostileDistanceGood)
                    {
                        Marrowrend.Cast();
                        return;
                    }
                }
                else
                {
                    if (MySettings.UseDeathandDecay && DeathandDecay.IsSpellUsable && DeathandDecay.IsHostileDistanceGood)
                    {
                        DeathandDecay.CastAtPosition(ObjectManager.Target.Position);
                        return;
                    }
                    if (MySettings.UseHearthStrike && HearthStrike.IsSpellUsable && HearthStrike.IsHostileDistanceGood)
                    {
                        HearthStrike.Cast();
                        return;
                    }
                }
            }
                //Multiple Targets
            else
            {
                if (MySettings.UseDeathandDecay && DeathandDecay.IsSpellUsable && DeathandDecay.IsHostileDistanceGood)
                {
                    DeathandDecay.CastAtPosition(ObjectManager.Target.Position);
                    return;
                }
                if (BoneShield.BuffStack < 6 || ObjectManager.Me.UnitAura(BoneShield.Id, ObjectManager.Me.Guid).AuraTimeLeftInMs < 2000)
                {
                    if (MySettings.UseMarrowrend && Marrowrend.IsSpellUsable && Marrowrend.IsHostileDistanceGood)
                    {
                        Marrowrend.Cast();
                        return;
                    }
                }
                else
                {
                    if (MySettings.UseHearthStrike && HearthStrike.IsSpellUsable && HearthStrike.IsHostileDistanceGood)
                    {
                        HearthStrike.Cast();
                        return;
                    }
                }
            }

            if (ObjectManager.Me.HealthPercent < MySettings.UseDeathStrikeBelowPercentage && DeathStrike.IsSpellUsable && DeathStrike.IsHostileDistanceGood)
            {
                DeathStrike.Cast();
                return;
            }
            if (MySettings.UseBloodBoil && BloodBoil.IsSpellUsable && BloodBoil.IsHostileDistanceGood)
            {
                BloodBoil.Cast();
                return;
            }
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    #region Nested type: DeathknightBloodSettings

    [Serializable]
    public class DeathknightBloodSettings : Settings
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
        public bool UseConsumption = true;

        /* Offensive Spells */
        public bool UseBloodBoil = true;
        public bool UseDeathandDecay = true;
        public int UseDeathStrikeBelowPercentage = 100;
        public bool UseHearthStrike = true;
        public bool UseMarrowrend = true;

        /* Defensive Spells */
        public int UseAsphyxiateBelowPercentage = 0;
        public int UseBloodMirrorBelowPercentage = 0;
        public int UseDancingRuneWeaponBelowPercentage = 0;
        public int UseMarkofBloodBelowPercentage = 0;
        public int UseRuneTapBelowPercentage = 0;
        public int UseTombstoneBelowPercentage = 0;
        public int UseVampiricBloodBelowPercentage = 0;
        public int UseAntiMagicShellBelowPercentage = 0;

        /* Healing Spells */
        public int UseBlooddrinkerBelowPercentage = 60;
        public int UseBonestormBelowPercentage = 40;

        /* Utility Spells */
        public bool UseDarkCommand = true;
        public bool UseDeathGrip = true;
        public bool UseGorefiendsGrasp = true;
        public bool UsePathofFrost = true;
        public bool UseWraithWalk = true;

        /* Game Settings */
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;

        public DeathknightBloodSettings()
        {
            ConfigWinForm("Deathknight Blood Settings");
            /* Professions & Racials */
            //AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Darkflight", "UseDarkflight", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaruBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            AddControlInWinForm("Use Stone Form", "UseStoneformBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            AddControlInWinForm("Use War Stomp", "UseWarStompBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            /* Artifact Spells */
            AddControlInWinForm("Use Consumption", "UseConsumption", "Artifact Spells");
            /* Offensive Spells */
            AddControlInWinForm("Use BloodBoil", "UseBloodBoil", "Offensive Spells");
            AddControlInWinForm("Use Death and Decay", "UseDeathandDecay", "Offensive Spells");
            AddControlInWinForm("Use Death Strike", "UseDeathStrikeBelowPercentage", "Offensive Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use HeartStrike", "UseHearthStrike", "Offensive Spells");
            AddControlInWinForm("Use BloodBoil", "UseBloodBoil", "Offensive Spells");
            /* Defensive Spells */
            AddControlInWinForm("Use Asphyxiate", "UseAsphyxiateBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Blood Mirror", "UseBloodMirrorBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Dancing Rune Weapon", "UseDancingRuneWeaponBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Mark of Blood", "UseMarkofBloodBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Rune Tap", "UseRuneTapBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Tombstone", "UseTombstoneBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Vampiric Blood", "UseVampiricBloodBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Anti-Magic Shell", "UseAntiMagicShellBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            /* Healing Spells */
            AddControlInWinForm("Use Blooddrinker", "UseBlooddrinkerBelowPercentage", "Healing Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Bonestorm", "UseBonestormBelowPercentage", "Healing Spells", "BelowPercentage", "Life");
            /* Utility Spells */
            AddControlInWinForm("Use Dark Command", "UseDarkCommand", "Utility Spells");
            AddControlInWinForm("Use Death Grip", "UseDeathGrip", "Utility Spells");
            AddControlInWinForm("Use Gorefiend's Grasp", "UseGorefiendsGrasp", "Utility Spells");
            AddControlInWinForm("Use Path of Frost", "UsePathofFrost", "Utility Spells");
            AddControlInWinForm("Use Wraith Walk", "UseWraithWalk", "Utility Spells");
            /* Game Settings */
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");
        }

        public static DeathknightBloodSettings CurrentSetting { get; set; }

        public static DeathknightBloodSettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Deathknight_Blood.xml";
            if (File.Exists(currentSettingsFile))
            {
                return
                    CurrentSetting = Load<DeathknightBloodSettings>(currentSettingsFile);
            }
            return new DeathknightBloodSettings();
        }
    }

    #endregion
}

public class DeathknightFrost
{
    private static DeathknightFrostSettings MySettings = DeathknightFrostSettings.GetSettings();

    #region General Timers & Variables

    private readonly WoWItem _firstTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET);
    private readonly WoWItem _secondTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET, 2);

    private bool CombatMode = true;

    private Timer DefensiveTimer = new Timer(0);
    private Timer StunTimer = new Timer(0);

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

    #region Talents

    private readonly Spell Avalanche = new Spell("Avalanche");
    private readonly Spell IcyTalons = new Spell("Icy Talons");
    private readonly Spell FrozenPulse = new Spell("Frozen Pulse");
    private readonly Spell ShatteringStrikes = new Spell("Shattering Strikes");

    #endregion

    #region Buffs

    private readonly Spell DarkSuccor = new Spell(101568);
    private readonly Spell KillingMachine = new Spell(51128);
    private readonly Spell Rime = new Spell(59057);
    private readonly Spell Razorice = new Spell(51714);
    private readonly Spell RuneofRazorice = new Spell(53343);

    #endregion

    #region Dots

    private readonly Spell FrostFever = new Spell(55095);

    #endregion

    #region Legion Artifact Spells

    private readonly Spell SindragosasFury = new Spell("Sindragosa's Fury");

    #endregion

    #region Offensive Spell

    private readonly Spell BreathofSindragosa = new Spell("Breath of Sindragosa");
    private readonly Spell DeathStrike = new Spell("Death Strike");
    private readonly Spell EmpowerRuneWeapon = new Spell("Empower Rune Weapon");
    private readonly Spell FrostStrike = new Spell("Frost Strike");
    private readonly Spell Frostscythe = new Spell("Frostscythe");
    private readonly Spell GlacialAdvance = new Spell("Glacial Advance");
    private readonly Spell HornofWinter = new Spell("Horn of Winter");
    private readonly Spell HowlingBlast = new Spell("Howling Blast");
    private readonly Spell HungeringRuneWeapon = new Spell("Hungering Rune Weapon");
    private readonly Spell Obliterate = new Spell("Obliterate");
    private readonly Spell Obliteration = new Spell("Obliteration");
    private readonly Spell PillarofFrost = new Spell("Pillar of Frost");
    private readonly Spell RemorselessWinter = new Spell("Remorseless Winter");

    #endregion

    #region Defensive Spells

    private readonly Spell AntiMagicShell = new Spell("Anti-Magic Shell");

    #endregion

    #region Utility Spells

    //private readonly Spell BlindingSleet = new Spell("Blinding Sleet");
    private readonly Spell DeathGrip = new Spell("Death Grip");
    private readonly Spell PathofFrost = new Spell("Path of Frost");

    #endregion

    public DeathknightFrost()
    {
        Main.InternalRange = ObjectManager.Me.GetCombatReach;
        Main.InternalLightHealingSpell = null;
        MySettings = DeathknightFrostSettings.GetSettings();
        Main.DumpCurrentSettings<DeathknightFrostSettings>(MySettings);
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
            }
            if (MySettings.UsePathofFrost && PathofFrost.IsSpellUsable && !PathofFrost.HaveBuff)
            {
                PathofFrost.Cast();
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

    // For general InFight Behavior (only touch if you want to add a new method like PetManagement())
    private void Combat()
    {
        //Log
        if (!CombatMode)
        {
            Logging.WriteFight("Combat:");
            CombatMode = true;
        }
        if (Healing() || Defensive() || Offensive())
            return;
        Rotation();
    }

    // For Healing Spells (always return after Casting)
    private bool Healing()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            //Gift of the Naaru
            if (ObjectManager.Me.HealthPercent < MySettings.UseGiftoftheNaaruBelowPercentage && GiftoftheNaaru.IsSpellUsable)
            {
                GiftoftheNaaru.Cast();
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
                }
                //Mitigate Damage
                if (ObjectManager.Me.HealthPercent < MySettings.UseStoneformBelowPercentage && Stoneform.IsSpellUsable)
                {
                    Stoneform.Cast();
                    DefensiveTimer = new Timer(1000*8);
                    return true;
                }
            }
            //Mitigate Damage in Emergency Situations
            if (ObjectManager.Me.HealthPercent < MySettings.UseAntiMagicShellBelowPercentage && AntiMagicShell.IsSpellUsable)
            {
                AntiMagicShell.Cast();
                DefensiveTimer = new Timer(1000*5);
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
            //Pillar of Frost
            if (MySettings.UsePillarofFrost && PillarofFrost.IsSpellUsable && !PillarofFrost.HaveBuff)
            {
                PillarofFrost.Cast();
            }
            //Obliteration
            if (MySettings.UseObliteration && Obliteration.IsSpellUsable && !Obliteration.HaveBuff &&
                FrostStrike.IsSpellUsable)
            {
                Obliteration.Cast();
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

            if (MySettings.UseHowlingBlast && HowlingBlast.IsSpellUsable && HowlingBlast.IsHostileDistanceGood &&
                ObjectManager.Target.UnitAura(FrostFever.Id, ObjectManager.Me.Guid).AuraTimeLeftInMs < 1000)
            {
                HowlingBlast.Cast();
                return;
            }
            if (ObjectManager.Me.HealthPercent < MySettings.UseDeathStrikeBelowPercentage && DeathStrike.IsSpellUsable && DeathStrike.IsHostileDistanceGood &&
                DarkSuccor.HaveBuff)
            {
                DeathStrike.Cast();
                return;
            }
            if (MySettings.UseSindragosasFury && SindragosasFury.IsSpellUsable && SindragosasFury.IsHostileDistanceGood &&
                Razorice.BuffStack == 5 && PillarofFrost.HaveBuff)
            {
                SindragosasFury.Cast();
                return;
            }
            if (MySettings.UseHowlingBlast && HowlingBlast.IsSpellUsable && HowlingBlast.IsHostileDistanceGood &&
                Rime.HaveBuff)
            {
                HowlingBlast.Cast();
                return;
            }
            if (MySettings.UseFrostStrike && FrostStrike.IsSpellUsable && FrostStrike.IsHostileDistanceGood &&
                (ObjectManager.Me.RunicPower >= 80 || Obliteration.HaveBuff ||
                 (ShatteringStrikes.HaveBuff && Razorice.BuffStack == 5)))
            {
                FrostStrike.Cast();
                return;
            }
            if (MySettings.UseGlacialAdvance && GlacialAdvance.IsSpellUsable && GlacialAdvance.IsHostileDistanceGood)
            {
                GlacialAdvance.Cast();
                return;
            }
            if (MySettings.UseFrostscythe && Frostscythe.IsSpellUsable && Frostscythe.IsHostileDistanceGood &&
                KillingMachine.HaveBuff)
            {
                Frostscythe.Cast();
                return;
            }
            if (MySettings.UseRemorselessWinter && RemorselessWinter.IsSpellUsable && RemorselessWinter.IsHostileDistanceGood)
            {
                RemorselessWinter.Cast();
                return;
            }

            //Single Target 
            if (ObjectManager.Target.GetUnitInSpellRange(5f) == 1)
            {
                if (MySettings.UseObliterate && Obliterate.IsSpellUsable && Obliterate.IsHostileDistanceGood &&
                    (!Rime.HaveBuff || ObjectManager.Me.Runes <= 3))
                {
                    Obliterate.Cast();
                    return;
                }
            }
                //Multiple Targets
            else
            {
                if (MySettings.UseFrostscythe && Frostscythe.IsSpellUsable && Frostscythe.IsHostileDistanceGood)
                {
                    Frostscythe.Cast();
                    return;
                }
            }

            if (MySettings.UseFrostStrike && FrostStrike.IsSpellUsable && FrostStrike.IsHostileDistanceGood &&
                (ObjectManager.Me.Runes <= 3 && Razorice.BuffStack == 5))
            {
                FrostStrike.Cast();
                return;
            }
            if (MySettings.UseHornofWinter && HornofWinter.IsSpellUsable && HornofWinter.IsHostileDistanceGood &&
                (ObjectManager.Me.Runes <= 1 && (ObjectManager.Me.MaxRunicPower - ObjectManager.Me.RunicPower) > 10))
            {
                HornofWinter.Cast();
                return;
            }
            if (ObjectManager.Me.HealthPercent < MySettings.UseDeathStrikeBelowPercentage && !MySettings.UseDeathStrikeOnlyWithDarkSuccor &&
                DeathStrike.IsSpellUsable && DeathStrike.IsHostileDistanceGood)
            {
                DeathStrike.Cast();
                return;
            }
            if (MySettings.UseFrostStrike && FrostStrike.IsSpellUsable && FrostStrike.IsHostileDistanceGood)
            {
                FrostStrike.Cast();
                return;
            }
            if (MySettings.UseEmpowerRuneWeapon && EmpowerRuneWeapon.IsSpellUsable)
            {
                EmpowerRuneWeapon.Cast();
            }
            if (MySettings.UseDeathGrip && DeathGrip.IsSpellUsable && DeathGrip.IsHostileDistanceGood)
            {
                DeathGrip.Cast();
                return;
            }
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    #region Nested type: DeathknightFrostSettings

    [Serializable]
    public class DeathknightFrostSettings : Settings
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
        public bool UseSindragosasFury = true;

        /* Offensive Spells */
        public int UseDeathStrikeBelowPercentage = 100;
        public bool UseDeathStrikeOnlyWithDarkSuccor = true;
        public bool UseHowlingBlast = true;
        public bool UseEmpowerRuneWeapon = true;
        public bool UseFrostStrike = true;
        public bool UseHornofWinter = true;
        public bool UseFrostscythe = true;
        public bool UseObliterate = true;
        public bool UseRemorselessWinter = true;
        public bool UseGlacialAdvance = true;
        public bool UsePillarofFrost = true;
        public bool UseObliteration = true;

        /* Utility Spells */
        public bool UseDeathGrip = true;
        public bool UsePathofFrost = true;

        /* Defensive Spells */
        public int UseAntiMagicShellBelowPercentage = 0;

        /* Game Settings */
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;

        public DeathknightFrostSettings()
        {
            ConfigWinForm("Deathknight Frost Settings");
            /* Professions & Racials */
            //AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Darkflight", "UseDarkflight", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaruBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            AddControlInWinForm("Use Stone Form", "UseStoneformBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            AddControlInWinForm("Use War Stomp", "UseWarStompBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            /* Artifact Spells */
            AddControlInWinForm("Use Sindragosa's Fury", "UseSindragosasFury", "Artifact Spells");
            /* Offensive Spells */
            AddControlInWinForm("Use Death Strike", "UseDeathStrikeBelowPercentage", "Offensive Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Death Strike only with Dark Succor Proc", "UseDeathStrikeOnlyWithDarkSuccor", "Offensive Spells");
            AddControlInWinForm("Use Howling Blast", "UseHowlingBlast", "Offensive Spells");
            AddControlInWinForm("Use Empower Rune Weapon", "UseEmpowerRuneWeapon", "Offensive Spells");
            AddControlInWinForm("Use Frost Strike", "UseFrostStrike", "Offensive Spells");
            AddControlInWinForm("Use Horn of Winter", "UseHornofWinter", "Offensive Spells");
            AddControlInWinForm("Use Frostscythe", "UseFrostscythe", "Offensive Spells");
            AddControlInWinForm("Use Obliterate", "UseObliterate", "Offensive Spells");
            AddControlInWinForm("Use Remorseless Winter", "UseRemorselessWinter", "Offensive Spells");
            AddControlInWinForm("Use Glacial Advance", "UseGlacialAdvance", "Offensive Spells");
            AddControlInWinForm("Use Pillar of Frost", "UsePillarofFrost", "Offensive Spells");
            AddControlInWinForm("Use Obliteration", "UseObliteration", "Offensive Spells");
            /* Defensive Spells */
            AddControlInWinForm("Use Anti-Magic Shell", "UseAntiMagicShellBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            /* Utility Spells */
            AddControlInWinForm("Use Death Grip", "UseDeathGrip", "Utility Spells");
            AddControlInWinForm("Use Path of Frost", "UsePathofFrost", "Utility Spells");
            /* Game Settings */
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");
        }

        public static DeathknightFrostSettings CurrentSetting { get; set; }

        public static DeathknightFrostSettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Deathknight_Frost.xml";
            if (File.Exists(currentSettingsFile))
            {
                return
                    CurrentSetting = Load<DeathknightFrostSettings>(currentSettingsFile);
            }
            return new DeathknightFrostSettings();
        }
    }

    #endregion
}

public class DeathknightUnholy
{
    private static DeathknightUnholySettings MySettings = DeathknightUnholySettings.GetSettings();

    #region General Timers & Variables

    private readonly WoWItem _firstTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET);
    private readonly WoWItem _secondTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET, 2);

    private bool CombatMode = true;

    private Timer DefensiveTimer = new Timer(0);
    private Timer StunTimer = new Timer(0);

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

    #region Talents

    private readonly Spell Castigator = new Spell("Castigator");
    private readonly Spell ShadowInfusion = new Spell("Shadow Infusion");

    #endregion

    #region Buffs

    private readonly Spell DarkSuccor = new Spell(101568);

    #endregion

    #region Dots

    private readonly Spell FesteringWound = new Spell(197147);
    private readonly Spell SuddenDoom = new Spell(49530);
    private readonly Spell VirulentPlague = new Spell(191587);

    #endregion

    #region Legion Artifact Spells

    private readonly Spell Apocalypse = new Spell("Apocalypse");
    private readonly Spell ScourgeofWorldsBuff = new Spell(191748);

    #endregion

    #region Offensive Spells

    private readonly Spell BlightedRuneWeapon = new Spell("Blighted Rune Weapon");
    private readonly Spell ClawingShadows = new Spell("Clawing Shadows");
    private readonly Spell DarkArbiter = new Spell("Dark Arbiter");
    private readonly Spell DarkTransformation = new Spell("Dark Transformation");
    private readonly Spell DeathandDecay = new Spell("Death and Decay");
    private readonly Spell DeathCoil = new Spell("Death Coil");
    private readonly Spell DeathStrike = new Spell("Death Strike");
    private readonly Spell Defile = new Spell("Defile");
    private readonly Spell Epidemic = new Spell("Epidemic");
    private readonly Spell FesteringStrike = new Spell("Festering Strike");
    private readonly Spell Outbreak = new Spell("Outbreak");
    private readonly Spell ScourgeStrike = new Spell("Scourge Strike");
    private readonly Spell SoulReaper = new Spell("Soul Reaper");
    private readonly Spell SummonGargoyle = new Spell("Summon Gargoyle");
    private readonly Spell ArmyoftheDead = new Spell("Army of the Dead");

    #endregion

    #region Defensive Spells

    private readonly Spell AntiMagicShell = new Spell("Anti-Magic Shell");
    private readonly Spell Asphyxiate = new Spell("Asphyxiate");

    #endregion

    #region Utility Spells

    private readonly Spell DeathGrip = new Spell("Death Grip");
    private readonly Spell RaiseDead = new Spell("Raise Dead");
    private readonly Spell PathofFrost = new Spell("Path of Frost");

    #endregion

    public DeathknightUnholy()
    {
        Main.InternalRange = ObjectManager.Me.GetCombatReach;
        Main.InternalLightHealingSpell = null;
        MySettings = DeathknightUnholySettings.GetSettings();
        Main.DumpCurrentSettings<DeathknightUnholySettings>(MySettings);
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
            }
            if (MySettings.UsePathofFrost && PathofFrost.IsSpellUsable && !PathofFrost.HaveBuff)
            {
                PathofFrost.Cast();
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

    // For general InFight Behavior (only touch if you want to add a new method like PetManagement())
    private void Combat()
    {
        //Log
        if (!CombatMode)
        {
            Logging.WriteFight("Combat:");
            CombatMode = true;
        }
        if (Healing() || Defensive() || Offensive())
            return;
        Rotation();
    }

    // For Healing Spells (always return after Casting)
    private bool Healing()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            //Gift of the Naaru
            if (ObjectManager.Me.HealthPercent < MySettings.UseGiftoftheNaaruBelowPercentage && GiftoftheNaaru.IsSpellUsable)
            {
                GiftoftheNaaru.Cast();
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
                    if (ObjectManager.Me.HealthPercent < MySettings.UseAsphyxiateBelowPercentage && Asphyxiate.IsSpellUsable)
                    {
                        Asphyxiate.Cast();
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
            }
            //Mitigate Damage in Emergency Situations
            if (ObjectManager.Me.HealthPercent < MySettings.UseAntiMagicShellBelowPercentage && AntiMagicShell.IsSpellUsable)
            {
                AntiMagicShell.Cast();
                DefensiveTimer = new Timer(1000*5);
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
            if (MySettings.UseArmyoftheDead && ArmyoftheDead.IsSpellUsable)
            {
                ArmyoftheDead.Cast();
            }
            if (MySettings.UseSummonGargoyle && SummonGargoyle.IsSpellUsable)
            {
                SummonGargoyle.Cast();
            }
            if (MySettings.UseRaiseDead && RaiseDead.IsSpellUsable &&
                ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0 || !ObjectManager.Pet.IsValid)
            {
                Logging.WriteDebug("Pet: Health == " + ObjectManager.Pet.Health + ", Guid == " + ObjectManager.Pet.Guid + ", IsValid == " + ObjectManager.Pet.IsValid);
                RaiseDead.Cast();
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

            if (MySettings.UseScourgeStrike && ScourgeStrike.IsSpellUsable && ScourgeStrike.IsHostileDistanceGood && ScourgeofWorldsBuff.HaveBuff &&
                ((!Castigator.HaveBuff && FesteringWound.TargetBuffStack >= 1) || FesteringWound.TargetBuffStack >= 3))
            {
                ScourgeStrike.Cast();
                return;
            }
            if (MySettings.UseOutbreak && Outbreak.IsSpellUsable && Outbreak.IsHostileDistanceGood && ObjectManager.Target.UnitAura(VirulentPlague.Id, ObjectManager.Me.Guid).AuraTimeLeftInMs < 1000)
            {
                Outbreak.Cast();
                return;
            }
            if (ObjectManager.Me.HealthPercent < MySettings.UseDeathStrikeBelowPercentage && DeathStrike.IsSpellUsable && DeathStrike.IsHostileDistanceGood &&
                DarkSuccor.HaveBuff)
            {
                DeathStrike.Cast();
                return;
            }
            if (MySettings.UseDarkTransformation && DarkTransformation.IsSpellUsable)
            {
                DarkTransformation.Cast();
                return;
            }
            if (MySettings.UseDarkArbiter && DarkArbiter.IsSpellUsable && ObjectManager.Me.RunicPowerPercentage > 90)
            {
                DarkArbiter.Cast();
                return;
            }
            if (MySettings.UseFesteringStrike && FesteringStrike.IsSpellUsable && FesteringStrike.IsHostileDistanceGood &&
                (FesteringWound.TargetBuffStack < 5 || (FesteringWound.TargetBuffStack < 8 && Apocalypse.IsSpellUsable)))
            {
                FesteringStrike.Cast();
                return;
            }
            if (MySettings.UseSoulReaper && SoulReaper.IsSpellUsable && SoulReaper.IsHostileDistanceGood &&
                FesteringWound.TargetBuffStack >= 3)
            {
                SoulReaper.Cast();
                //TODO Utilize Soul Reaper by bursting 3 wounds with Sourge Strike or Clawing Shadow
                return;
            }
            if (MySettings.UseApocalypse && Apocalypse.IsSpellUsable && Apocalypse.IsHostileDistanceGood &&
                FesteringWound.TargetBuffStack >= 7)
            {
                Apocalypse.Cast();
                return;
            }
            if (ObjectManager.Target.GetUnitInSpellRange(5f) > 1)
            {
                if (MySettings.UseDefile && Defile.IsSpellUsable && Defile.IsHostileDistanceGood)
                {
                    Defile.Cast();
                    return;
                }
                if (MySettings.UseDeathandDecay && DeathandDecay.IsSpellUsable && DeathandDecay.IsHostileDistanceGood)
                {
                    DeathandDecay.CastAtPosition(ObjectManager.Target.Position);
                    return;
                }
            }
            if (MySettings.UseClawingShadows && ClawingShadows.IsSpellUsable && ClawingShadows.IsHostileDistanceGood)
            {
                ClawingShadows.Cast();
                return;
            }
            else if (MySettings.UseScourgeStrike && ScourgeStrike.IsSpellUsable && ScourgeStrike.IsHostileDistanceGood &&
                     ((!Castigator.HaveBuff && FesteringWound.TargetBuffStack >= 1) || FesteringWound.TargetBuffStack >= 3))
            {
                ScourgeStrike.Cast();
                return;
            }
            if (ObjectManager.Me.HealthPercent < MySettings.UseDeathStrikeBelowPercentage && !MySettings.UseDeathStrikeOnlyWithDarkSuccor &&
                DeathStrike.IsSpellUsable && DeathStrike.IsHostileDistanceGood)
            {
                DeathStrike.Cast();
                return;
            }
            if (MySettings.UseDeathCoil && DeathCoil.IsSpellUsable && DeathCoil.IsHostileDistanceGood &&
                (!ShadowInfusion.HaveBuff || ObjectManager.Pet.HaveBuff(DarkTransformation.Ids) ||
                 SuddenDoom.HaveBuff || ObjectManager.Me.RunicPowerPercentage > 90))
            {
                DeathCoil.Cast();
                return;
            }
            if (MySettings.UseDeathGrip && DeathGrip.IsSpellUsable && DeathGrip.IsHostileDistanceGood)
            {
                DeathGrip.Cast();
                return;
            }
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    #region Nested type: DeathknightUnholySettings

    [Serializable]
    public class DeathknightUnholySettings : Settings
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
        public bool UseApocalypse = true;

        /* Offensive Spells */
        public bool UseArmyoftheDead = true;
        public bool UseClawingShadows = true;
        public bool UseDarkArbiter = true;
        public bool UseDarkTransformation = true;
        public bool UseDeathandDecay = true;
        public bool UseDeathCoil = true;
        public int UseDeathStrikeBelowPercentage = 100;
        public bool UseDeathStrikeOnlyWithDarkSuccor = true;
        public bool UseDefile = true;
        public bool UseFesteringStrike = true;
        public bool UseOutbreak = true;
        public bool UseScourgeStrike = true;
        public bool UseSoulReaper = true;
        public bool UseSummonGargoyle = true;

        /* Defensive Spells */
        public int UseAntiMagicShellBelowPercentage = 50;
        public int UseAsphyxiateBelowPercentage = 50;

        /* Utility Spells */
        public bool UseDeathGrip = true;
        public bool UsePathofFrost = true;
        public bool UseRaiseDead = true;

        /* Game Settings */
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;

        public DeathknightUnholySettings()
        {
            ConfigWinForm("Deathknight Unholy Settings");
            /* Professions & Racials */
            //AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Darkflight", "UseDarkflight", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaruBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            AddControlInWinForm("Use Stone Form", "UseStoneformBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            AddControlInWinForm("Use War Stomp", "UseWarStompBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            /* Artifact Spells */
            AddControlInWinForm("Use Apocalypse", "UseApocalypse", "Artifact Spells");
            /* Offensive Spells */
            AddControlInWinForm("Use Army of the Dead", "UseArmyoftheDead", "Offensive Spells");
            AddControlInWinForm("Use Clawing Shadows", "UseClawingShadows", "Offensive Spells");
            AddControlInWinForm("Use Dark Arbiter", "UseDarkArbiter", "Offensive Spells");
            AddControlInWinForm("Use Dark Transformation", "UseDarkTransformation", "Offensive Spells");
            AddControlInWinForm("Use Death and Decay", "UseDeathandDecay", "Offensive Spells");
            AddControlInWinForm("Use Death Coil", "UseDeathCoil", "Offensive Spells");
            AddControlInWinForm("Use Death Strike", "UseDeathStrikeBelowPercentage", "Offensive Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Death Strike only with Dark Succor Proc", "UseDeathStrikeOnlyWithDarkSuccor", "Offensive Spells");
            AddControlInWinForm("Use Defile", "UseDefile", "Offensive Spells");
            AddControlInWinForm("Use Festering Strike", "UseFesteringStrike", "Offensive Spells");
            AddControlInWinForm("Use Outbreak", "UseOutbreak", "Offensive Spells");
            AddControlInWinForm("Use Scourge Strike", "UseScourgeStrike", "Offensive Spells");
            AddControlInWinForm("Use Soul Reaper", "UseSoulReaper", "Offensive Spells");
            AddControlInWinForm("Use Summon Gargoyle", "UseSummonGargoyle", "Offensive Spells");
            /* Defensive Spells */
            AddControlInWinForm("Use Anti-Magic Shell", "UseAntiMagicShellBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Asphyxiate", "UseAsphyxiateBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            /* Utility Spells */
            AddControlInWinForm("Use Death Grip", "UseDeathGrip", "Utility Spells");
            AddControlInWinForm("Use Path of Frost", "UsePathofFrost", "Utility Spells");
            AddControlInWinForm("Use Raise Dead", "UseRaiseDead", "Utility Spells");
            /* Game Settings */
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");
        }

        public static DeathknightUnholySettings CurrentSetting { get; set; }

        public static DeathknightUnholySettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Deathknight_Unholy.xml";
            if (File.Exists(currentSettingsFile))
            {
                return
                    CurrentSetting = Load<DeathknightUnholySettings>(currentSettingsFile);
            }
            return new DeathknightUnholySettings();
        }
    }

    #endregion
}

#endregion

// ReSharper restore ObjectCreationAsStatement
// ReSharper restore EmptyGeneralCatchClause