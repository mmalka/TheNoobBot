/*
* CombatClass for TheNoobBot
* Credit : Vesper, Neo2003, Ryuichiro
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
                    #region DemonHunter Specialisation checking

                case WoWClass.DemonHunter:

                    if (wowSpecialization == WoWSpecialization.DemonHunterVengeance || wowSpecialization == WoWSpecialization.None)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\DemonHunter_Vengeance.xml";
                            var currentSetting = new DemonHunterVengeance.DemonHunterVengeanceSettings();
                            if (File.Exists(currentSettingsFile) && !resetSettings)
                            {
                                currentSetting = Settings.Load<DemonHunterVengeance.DemonHunterVengeanceSettings>(currentSettingsFile);
                            }
                            currentSetting.ToForm();
                            currentSetting.Save(currentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading DemonHunter Vengeance Combat class...");
                            EquipmentAndStats.SetPlayerSpe(WoWSpecialization.DemonHunterVengeance);
                            new DemonHunterVengeance();
                        }
                        break;
                    }
                    if (wowSpecialization == WoWSpecialization.DemonHunterHavoc)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\DemonHunter_Havoc.xml";
                            var currentSetting = new DemonHunterHavoc.DemonHunterHavocSettings();
                            if (File.Exists(currentSettingsFile) && !resetSettings)
                            {
                                currentSetting = Settings.Load<DemonHunterHavoc.DemonHunterHavocSettings>(currentSettingsFile);
                            }
                            currentSetting.ToForm();
                            currentSetting.Save(currentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading DemonHunter Havoc Combat class...");
                            EquipmentAndStats.SetPlayerSpe(WoWSpecialization.DemonHunterHavoc);
                            new DemonHunterHavoc();
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

#region DemonHunter

public class DemonHunterHavoc
{
    private static DemonHunterHavocSettings MySettings = DemonHunterHavocSettings.GetSettings();

    #region General Timers & Variables

    private readonly WoWItem _firstTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET);
    private readonly WoWItem _secondTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET, 2);

    private bool CombatMode = true;

    #endregion

    #region Talents

    private readonly Spell Bloodlet = new Spell("Bloodlet");
    private readonly Spell DemonBlades = new Spell("Demon Blades");
    private readonly Spell Demonic = new Spell("Demonic");
    private readonly Spell ChaosCleave = new Spell("ChaosCleave");
    private readonly Spell FelMastery = new Spell("Fel Mastery");
    private readonly Spell FirstBlood = new Spell("First Blood");
    private readonly Spell MasteroftheGlaive = new Spell("Master of the Glaive");
    private readonly Spell Momentum = new Spell("Momentum");
    private readonly Spell Prepared = new Spell("Prepared");

    #endregion

    #region Professions & Racial

    //private readonly Spell ArcaneTorrent = new Spell("Arcane Torrent"); //No GCD
    private readonly Spell GiftoftheNaaru = new Spell("Gift of the Naaru"); //No GCD

    #endregion

    #region DemonHunter Seals & Buffs

    private readonly Spell DemonicTransformation = new Spell("Demonic Transformation");
    private readonly Spell MomentumBuff = new Spell(208628 /*"Momentum"*/);

    #endregion

    #region Offensive Spells

    private readonly Spell Annihilation = new Spell("Annihilation");
    private readonly Spell BladeDance = new Spell("Blade Dance");
    private readonly Spell ChaosStrike = new Spell("Chaos Strike");
    private readonly Spell DeathSweep = new Spell("Death Sweep");
    private readonly Spell DemonsBite = new Spell("Demon's Bite");
    private readonly Spell Felblade = new Spell("Felblade");
    private readonly Spell FelRush = new Spell("Fel Rush");
    private readonly Spell ThrowGlaive = new Spell("Throw Glaive");

    #endregion

    #region Legion Artifact

    private readonly Spell AnguishoftheDeceiver = new Spell("Anguish of the Deceiver"); //No GCD
    private readonly Spell FuryoftheIllidari = new Spell("Fury of the Illidari");

    #endregion

    #region Offensive Cooldowns

    private readonly Spell ChaosBlades = new Spell("Chaos Blades"); //No GCD
    private readonly Spell EyeBeam = new Spell("Eye Beam");
    private readonly Spell FelBarrage = new Spell("Fel Barrage");
    private readonly Spell FelEruption = new Spell("Fel Eruption");
    private readonly Spell InfernalStrike = new Spell("Infernal Strike");
    private readonly Spell Metamorphosis = new Spell("Metamorphosis");
    private readonly Spell Nemesis = new Spell("Nemesis");
    private readonly Spell VengefulRetreat = new Spell("Vengeful Retreat"); //No GCD

    #endregion

    #region Defensive Cooldown

    private readonly Spell Blur = new Spell("Blur"); //No GCD
    private readonly Spell Darkness = new Spell("Darkness"); //No GCD

    #endregion

    #region Utility Spells

    //private readonly Spell ConsumeMagic = new Spell("Consume Magic"); //No GCD
    //private readonly Spell Imprison = new Spell("Imprison");
    private readonly Spell Netherwalk = new Spell("Netherwalk"); //No GCD

    #endregion

    public DemonHunterHavoc()
    {
        Main.InternalRange = ObjectManager.Me.GetCombatReach;
        Main.InternalAggroRange = Main.InternalRange;
        Main.InternalLightHealingSpell = null;
        MySettings = DemonHunterHavocSettings.GetSettings();
        Main.DumpCurrentSettings<DemonHunterHavocSettings>(MySettings);
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
                //Movement Buffs
                if (MySettings.UseNetherwalkOOC && Netherwalk.IsSpellUsable && !Netherwalk.HaveBuff)
                {
                    Netherwalk.Cast();
                    return;
                }
            }
            else
            {
                //Self Heal for Damage Dealer
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
        Defensive();
        BurstBuffs();
        GCDCycle();
    }

    private void Heal()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            //Gift of the Naaru
            if (ObjectManager.Me.HealthPercent < MySettings.UseGiftoftheNaaruBelowPercentage && GiftoftheNaaru.IsSpellUsable)
            {
                GiftoftheNaaru.Cast();
                return;
            }
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

            //Mitigate Damage
            if (Blur.IsSpellUsable && ObjectManager.Me.HealthPercent < MySettings.UseBlurBelowPercentage)
            {
                Blur.Cast();
            }
            if (Darkness.IsSpellUsable && ObjectManager.Me.HealthPercent < MySettings.UseDarknessBelowPercentage)
            {
                Darkness.Cast();
            }
            if (Netherwalk.IsSpellUsable && ObjectManager.Me.HealthPercent < MySettings.UseNetherwalkBelowPercentage)
            {
                Netherwalk.Cast();
            }
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
            if (MySettings.UseMetamorphosis && Metamorphosis.IsSpellUsable && Metamorphosis.IsHostileDistanceGood)
            {
                Metamorphosis.CastAtPosition(ObjectManager.Target.Position);
            }
            if (MySettings.UseChaosBlades && ChaosBlades.IsSpellUsable)
            {
                ChaosBlades.Cast();
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

            if (MySettings.UseFelRush && FelRush.IsSpellUsable && CombatClass.InSpellRange(ObjectManager.Target, 0, 20 /*FelRush Range*/) &&
                //Keep Recharging
                (FelRush.GetSpellCharges == 2 ||
                 //Generate Fury
                 (FelMastery.HaveBuff && (ObjectManager.Me.MaxFury - ObjectManager.Me.Fury) < 30) ||
                 //Trigger Momentum
                 (Momentum.HaveBuff && !MomentumBuff.HaveBuff)))
            {
                FelRush.Cast();
                return;
            }
            if (MySettings.UseVengefulRetreat && VengefulRetreat.IsSpellUsable && CombatClass.InSpellRange(ObjectManager.Target, 0, 20 /*Vengeful Retreat Range*/) &&
                //Generate Fury
                ((Prepared.HaveBuff && (ObjectManager.Me.MaxFury - ObjectManager.Me.Fury) < 20) ||
                 //Trigger Momentum
                 (Momentum.HaveBuff && !MomentumBuff.HaveBuff)))
            {
                VengefulRetreat.Cast();
                return;
            }
            //Single Target 
            if (ObjectManager.GetUnitInSpellRange(5f) == 1)
            {
                if (MySettings.UseNemesis && Nemesis.IsSpellUsable && Nemesis.IsHostileDistanceGood)
                {
                    Nemesis.Cast();
                    return;
                }
                if (Demonic.HaveBuff && MySettings.UseEyeBeam && EyeBeam.IsSpellUsable && EyeBeam.IsHostileDistanceGood)
                {
                    EyeBeam.Cast();
                    return;
                }
                if (MySettings.UseFelEruption && FelEruption.IsSpellUsable && FelEruption.IsHostileDistanceGood)
                {
                    FelEruption.Cast();
                    return;
                }
                if (MySettings.UseFuryoftheIllidari && FuryoftheIllidari.IsSpellUsable && FuryoftheIllidari.IsHostileDistanceGood &&
                    //Only with Momentum
                    (!Momentum.HaveBuff || MomentumBuff.HaveBuff))
                {
                    FuryoftheIllidari.Cast();
                    return;
                }
                if (FirstBlood.HaveBuff && (!MomentumBuff.HaveBuff || MomentumBuff.HaveBuff))
                {
                    if (MySettings.UseBladeDance && BladeDance.IsSpellUsable && BladeDance.IsHostileDistanceGood)
                    {
                        BladeDance.Cast();
                        return;
                    }
                    if (MySettings.UseDeathSweep && DeathSweep.IsSpellUsable && DeathSweep.IsHostileDistanceGood)
                    {
                        DeathSweep.Cast();
                        return;
                    }
                }
                if (MySettings.UseFelblade && Felblade.IsSpellUsable && Felblade.IsHostileDistanceGood &&
                    //Generate Fury
                    (ObjectManager.Me.MaxFury - ObjectManager.Me.Fury) < 30)
                {
                    Felblade.Cast();
                    return;
                }
                if (Bloodlet.HaveBuff && MySettings.UseThrowGlaive && ThrowGlaive.IsSpellUsable && ThrowGlaive.IsHostileDistanceGood &&
                    //Only with Momentum
                    (!MomentumBuff.HaveBuff || MomentumBuff.HaveBuff))
                {
                    ThrowGlaive.Cast();
                    return;
                }
                if (MySettings.UseFelBarrage && FelBarrage.IsSpellUsable && FelBarrage.IsHostileDistanceGood &&
                    //Keep Recharging
                    FelBarrage.GetSpellCharges == 5 &&
                    //Only with Momentum
                    (!MomentumBuff.HaveBuff || MomentumBuff.HaveBuff))
                {
                    FelBarrage.Cast();
                    return;
                }
                if (MySettings.UseAnnihilation && Annihilation.IsSpellUsable && Annihilation.IsHostileDistanceGood &&
                    //Generate Fury
                    (ObjectManager.Me.MaxFury - ObjectManager.Me.Fury) < 30)
                {
                    Annihilation.Cast();
                    return;
                }
                if (AnguishoftheDeceiver.HaveBuff && MySettings.UseEyeBeam && EyeBeam.IsSpellUsable && EyeBeam.IsHostileDistanceGood)
                {
                    EyeBeam.Cast();
                    return;
                }
                if (MySettings.UseChaosStrike && ChaosStrike.IsSpellUsable && ChaosStrike.IsHostileDistanceGood &&
                    //Generate Fury
                    (ObjectManager.Me.MaxFury - ObjectManager.Me.Fury) < 30)
                {
                    ChaosStrike.Cast();
                    return;
                }
                if (MySettings.UseFelBarrage && FelBarrage.IsSpellUsable && FelBarrage.IsHostileDistanceGood &&
                    //Keep Recharging
                    FelBarrage.GetSpellCharges >= 4 &&
                    //Only with Momentum
                    (!MomentumBuff.HaveBuff || MomentumBuff.HaveBuff))
                {
                    FelBarrage.Cast();
                    return;
                }
                if (!DemonBlades.HaveBuff && MySettings.UseDemonsBite && DemonsBite.IsSpellUsable && DemonsBite.IsHostileDistanceGood)
                {
                    DemonsBite.Cast();
                    return;
                }
                if (MySettings.UseFelRush && FelRush.IsSpellUsable && CombatClass.InSpellRange(ObjectManager.Target, 6, 20 /*FelRush Range*/))
                {
                    FelRush.Cast();
                    return;
                }
                if (MySettings.UseThrowGlaive && ThrowGlaive.IsSpellUsable && (!CombatClass.InMeleeRange(ObjectManager.Target) ||
                                                                               DemonBlades.HaveBuff))
                {
                    ThrowGlaive.Cast();
                    return;
                }
            }
                //Multiple Target
            else
            {
                if (MySettings.UseFuryoftheIllidari && FuryoftheIllidari.IsSpellUsable && FuryoftheIllidari.IsHostileDistanceGood &&
                    //Only with Momentum
                    (!Momentum.HaveBuff || MomentumBuff.HaveBuff))
                {
                    FuryoftheIllidari.Cast();
                    return;
                }
                if (MySettings.UseFelBarrage && FelBarrage.IsSpellUsable && FelBarrage.IsHostileDistanceGood &&
                    //Keep Recharging
                    FelBarrage.GetSpellCharges >= 4 &&
                    //Only with Momentum
                    (!MomentumBuff.HaveBuff || MomentumBuff.HaveBuff))
                {
                    FelBarrage.Cast();
                    return;
                }
                if (MySettings.UseEyeBeam && EyeBeam.IsSpellUsable && EyeBeam.IsHostileDistanceGood &&
                    //Only with Momentum
                    (!MomentumBuff.HaveBuff || MomentumBuff.HaveBuff))
                {
                    EyeBeam.Cast();
                    return;
                }
                if (FirstBlood.HaveBuff && ObjectManager.GetUnitInSpellRange(5f) > 2 &&
                    (!MomentumBuff.HaveBuff || MomentumBuff.HaveBuff))
                {
                    if (MySettings.UseBladeDance && BladeDance.IsSpellUsable && BladeDance.IsHostileDistanceGood)
                    {
                        BladeDance.Cast();
                        return;
                    }
                    if (MySettings.UseDeathSweep && DeathSweep.IsSpellUsable && DeathSweep.IsHostileDistanceGood)
                    {
                        DeathSweep.Cast();
                        return;
                    }
                }
                if (Bloodlet.HaveBuff && MySettings.UseThrowGlaive && ThrowGlaive.IsSpellUsable && ThrowGlaive.IsHostileDistanceGood &&
                    //Only with Momentum
                    (!MomentumBuff.HaveBuff || MomentumBuff.HaveBuff))
                {
                    ThrowGlaive.Cast();
                    return;
                }
                if (MySettings.UseChaosStrike && ChaosStrike.IsSpellUsable &&
                    ChaosCleave.HaveBuff && ObjectManager.GetUnitInSpellRange(ChaosStrike.MaxRangeHostile) <= 3)
                {
                    ChaosStrike.Cast();
                    return;
                }
                if (MySettings.UseThrowGlaive && ThrowGlaive.IsSpellUsable && ThrowGlaive.IsHostileDistanceGood)
                {
                    ThrowGlaive.Cast();
                    return;
                }
                if (MySettings.UseChaosStrike && ChaosStrike.IsSpellUsable && ChaosStrike.IsHostileDistanceGood &&
                    //Generate Fury
                    (!DemonBlades.HaveBuff || (ObjectManager.Me.MaxFury - ObjectManager.Me.Fury) < 40) &&
                    (ObjectManager.Me.MaxFury - ObjectManager.Me.Fury) < 30)
                {
                    ChaosStrike.Cast();
                    return;
                }
                if (!DemonBlades.HaveBuff && MySettings.UseDemonsBite && DemonsBite.IsSpellUsable && DemonsBite.IsHostileDistanceGood)
                {
                    DemonsBite.Cast();
                    return;
                }
                if (MySettings.UseInfernalStrike && InfernalStrike.IsSpellUsable && InfernalStrike.IsHostileDistanceGood)
                {
                    InfernalStrike.CastAtPosition(ObjectManager.Target.Position);
                    return;
                }
            }
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    #region Nested type: DemonHunterHavocSettings

    [Serializable]
    public class DemonHunterHavocSettings : Settings
    {
        /* Professions & Racials */
        //public bool UseArcaneTorrent = true;
        public int UseGiftoftheNaaruBelowPercentage = 50;

        /* Offensive Spells */
        public bool UseAnnihilation = true;
        public bool UseBladeDance = true;
        public bool UseChaosStrike = true;
        public bool UseDeathSweep = true;
        public bool UseDemonsBite = true;
        public bool UseFelblade = true;
        public bool UseFelRush = true;
        public bool UseThrowGlaive = true;

        /* Artifact Spells */
        public bool UseFuryoftheIllidari = true;

        /* Offensive Cooldowns */
        public bool UseChaosBlades = true;
        public bool UseEyeBeam = true;
        public bool UseFelBarrage = true;
        public bool UseFelEruption = true;
        public bool UseInfernalStrike = true;
        public bool UseMetamorphosis = true;
        public bool UseNemesis = true;
        public bool UseVengefulRetreat = true;

        /* Defensive Cooldowns */
        public int UseBlurBelowPercentage = 40;
        public int UseDarknessBelowPercentage = 60;

        /* Utility Spells */
        //public bool UseConsumeMagic = true;
        //public bool UseImprison = true;
        public int UseNetherwalkBelowPercentage = 10;
        public bool UseNetherwalkOOC = true;

        /* Game Settings */
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;

        public DemonHunterHavocSettings()
        {
            ConfigWinForm("DemonHunter Havoc Settings");
            /* Professions & Racials */
            //AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaruBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            /* Offensive Spells */
            AddControlInWinForm("Use Annihilation", "UseAnnihilation", "Offensive Spells");
            AddControlInWinForm("Use Blade Dance", "UseBladeDance", "Offensive Spells");
            AddControlInWinForm("Use Chaos Strike", "UseChaosStrike", "Offensive Spells");
            AddControlInWinForm("Use Death Sweep", "UseDeathSweep", "Offensive Spells");
            AddControlInWinForm("Use Demons Bite", "UseDemonsBite", "Offensive Spells");
            AddControlInWinForm("Use Felblade", "UseFelblade", "Offensive Spells");
            AddControlInWinForm("Use Fel Rush", "UseFelRush", "Offensive Spells");
            AddControlInWinForm("Use Throw Glaive", "UseThrowGlaive", "Offensive Spells");
            /* Artifact Spells */
            AddControlInWinForm("Use Fury of the Illidari", "UseFuryoftheIllidari", "Artifact Spells");
            /* Offensive Cooldowns */
            AddControlInWinForm("Use Chaos Blades", "UseChaosBlades", "Offensive Spells");
            AddControlInWinForm("Use Eye Beam", "UseEyeBeam", "Offensive Spells");
            AddControlInWinForm("Use Fel Barrage", "UseFelBarrage", "Offensive Spells");
            AddControlInWinForm("Use Fel Eruption", "UseFelEruption", "Offensive Spells");
            AddControlInWinForm("Use Metamorphosis", "UseMetamorphosis", "Offensive Spells");
            AddControlInWinForm("Use Nemesis", "UseNemesis", "Offensive Spells");
            AddControlInWinForm("Use Vengeful Retreat", "UseVengefulRetreat", "Offensive Spells");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Blur", "UseBlur", "Defensive Cooldowns", "BelowPercentage", "Life");
            AddControlInWinForm("Use Darkness", "UseDarkness", "Defensive Cooldowns", "BelowPercentage", "Life");
            /* Utility Spells */
            AddControlInWinForm("Use Netherwalk", "UseNetherwalkBelowPercentage", "Utility Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Netherwalk out of Combat", "UseNetherwalkOOC", "Utility Spells");
            /* Game Settings */
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");
        }

        public static DemonHunterHavocSettings CurrentSetting { get; set; }

        public static DemonHunterHavocSettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\DemonHunter_Havoc.xml";
            if (File.Exists(currentSettingsFile))
            {
                return
                    CurrentSetting = Load<DemonHunterHavocSettings>(currentSettingsFile);
            }
            return new DemonHunterHavocSettings();
        }
    }

    #endregion
}

public class DemonHunterVengeance
{
    private static DemonHunterVengeanceSettings MySettings = DemonHunterVengeanceSettings.GetSettings();

    #region General Timers & Variables

    private readonly WoWItem _firstTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET);
    private readonly WoWItem _secondTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET, 2);

    private bool CombatMode = true;

    private Timer DefensiveTimer = new Timer(0);

    #endregion

    #region Talents

    private readonly Spell BladeTurning = new Spell("Blade Turning");
    private readonly Spell FlameCrash = new Spell("Flame Crash");

    #endregion

    #region Professions & Racial

    //private readonly Spell ArcaneTorrent = new Spell("Arcane Torrent"); //No GCD
    private readonly Spell GiftoftheNaaru = new Spell("Gift of the Naaru"); //No GCD

    #endregion

    #region DemonHunter Seals & Buffs

    private readonly Spell BladeTurningBuff = new Spell(207709 /*"Blade Turning"*/);
    private readonly Spell SoulFragments = new Spell(203981 /*"Soul Fragments"*/);

    #endregion

    #region DemonHunter Dots

    private readonly Spell Frailty = new Spell("Frailty");

    #endregion

    #region Legion Artifact

    private readonly Spell SoulCarver = new Spell("Soul Carver");

    #endregion

    #region Offensive Spells

    private readonly Spell Felblade = new Spell("Felblade");
    private readonly Spell FelDevastation = new Spell("Fel Devastation");
    private readonly Spell FelEruption = new Spell("Fel Eruption");
    private readonly Spell Fracture = new Spell("Fracture");
    private readonly Spell ImmolationAura = new Spell("Immolation Aura");
    private readonly Spell InfernalStrike = new Spell("Infernal Strike");
    private readonly Spell Shear = new Spell("Shear");
    private readonly Spell SigilofFlame = new Spell("Sigil of Flame");
    private Timer SigilofFlameTimer = new Timer(0);
    private readonly Spell SoulCleave = new Spell("Soul Cleave");
    private readonly Spell SpiritBomb = new Spell("Spirit Bomb");

    #endregion

    #region Defensive Cooldown

    private readonly Spell DemonSpikes = new Spell("Demon Spikes");
    private readonly Spell FieryBrand = new Spell("Fiery Brand");
    private readonly Spell EmpowerWards = new Spell("Empower Wards");
    private readonly Spell Metamorphosis = new Spell("Metamorphosis");

    #endregion

    #region Healing Spell

    #endregion

    #region Utility Spells

    //private readonly Spell ConsumeMagic = new Spell("Consume Magic"); //No GCD
    //private readonly Spell Imprison = new Spell("Imprison");
    //private readonly Spell SigilofMisery = new Spell("Sigil of Misery");
    //private readonly Spell SigilofSilence = new Spell("Sigil of Silence");
    //private readonly Spell ThrowGlaive = new Spell("Throw Glaive");
    private readonly Spell Torment = new Spell("Torment"); //No GCD

    #endregion

    public DemonHunterVengeance()
    {
        Main.InternalRange = ObjectManager.Me.GetCombatReach;
        Main.InternalAggroRange = Main.InternalRange;
        Main.InternalLightHealingSpell = null;
        MySettings = DemonHunterVengeanceSettings.GetSettings();
        Main.DumpCurrentSettings<DemonHunterVengeanceSettings>(MySettings);
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
                //Movement Buffs
            }
            else
            {
                //Self Heal for Damage Dealer
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
        Defensive();
        AggroManagement();
        BurstBuffs();
        GCDCycle();
    }

    private void Heal()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            //Gift of the Naaru
            if (ObjectManager.Me.HealthPercent < MySettings.UseGiftoftheNaaruBelowPercentage && GiftoftheNaaru.IsSpellUsable)
            {
                GiftoftheNaaru.Cast();
                return;
            }
            //Soul Cleave
            if (MySettings.UseSoulCleave && ObjectManager.Me.HealthPercent < MySettings.UseSoulCleaveBelowPercentage && SoulCleave.IsSpellUsable)
            {
                SoulCleave.Cast();
                return;
            }
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
            if (DefensiveTimer.IsReady || ObjectManager.Me.HealthPercent < 25)
            {
                //Mitigate Damage
                if (DemonSpikes.IsSpellUsable && !DemonSpikes.HaveBuff && (DemonSpikes.GetSpellCharges == 2 ||
                                                                           ObjectManager.Me.HealthPercent < MySettings.UseDemonSpikesBelowPercentage))
                {
                    DemonSpikes.Cast();
                    DefensiveTimer = new Timer(1000*6);
                    return;
                }
                if ((ObjectManager.Me.HealthPercent < MySettings.UseFieryBrandBelowPercentage ||
                     (ObjectManager.Target.IsCast && !ObjectManager.Target.CanInterruptCurrentCast &&
                      ObjectManager.Target.CastEndsInMs < 1500)) && FieryBrand.IsSpellUsable)
                {
                    FieryBrand.Cast();
                    DefensiveTimer = new Timer(1000*8);
                    return;
                }
            }
            if (ObjectManager.Me.HealthPercent < MySettings.UseMetamorphosisBelowPercentage &&
                Metamorphosis.IsSpellUsable && Metamorphosis.IsHostileDistanceGood)
            {
                Metamorphosis.CastAtPosition(ObjectManager.Target.Position);
                return;
            }
            //Mitigate Magic Damage for Rage
            if (ObjectManager.Me.HealthPercent < MySettings.UseEmpowerWardsBelowPercentage &&
                EmpowerWards.IsSpellUsable && !EmpowerWards.HaveBuff)
            {
                EmpowerWards.Cast();
                DefensiveTimer = new Timer(1000*6);
                return;
            }
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
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    private void AggroManagement()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            //Torment
            //Cast Growl when you are in a party and the target of your target is a low health player
            if (MySettings.UseTormentBelowToTPercentage > 0 && Torment.IsSpellUsable &&
                Torment.IsHostileDistanceGood)
            {
                WoWObject obj = ObjectManager.GetObjectByGuid(ObjectManager.Target.Target);
                if (obj.IsValid && obj.Type == WoWObjectType.Player &&
                    new WoWPlayer(obj.GetBaseAddress).HealthPercent < MySettings.UseTormentBelowToTPercentage)
                {
                    Torment.Cast();
                    return;
                }
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

            if (MySettings.UseSoulCarver && SoulCarver.IsSpellUsable && SoulCarver.IsHostileDistanceGood)
            {
                SoulCarver.Cast();
                return;
            }
            if (MySettings.UseFelDevastation && FelDevastation.IsSpellUsable && FelDevastation.IsHostileDistanceGood)
            {
                FelDevastation.Cast();
                return;
            }
            if (MySettings.UseSoulCleave && SoulCleave.IsSpellUsable && SoulCleave.IsHostileDistanceGood &&
                //Spend Pain
                ObjectManager.Me.Pain >= 80)
            {
                SoulCleave.Cast();
                return;
            }
            if (MySettings.UseImmolationAura && ImmolationAura.IsSpellUsable && ImmolationAura.IsHostileDistanceGood)
            {
                ImmolationAura.Cast();
                return;
            }

            //Single Target 
            if (ObjectManager.GetUnitInSpellRange(5f) == 1)
            {
                if (MySettings.UseFelblade && Felblade.IsSpellUsable && Felblade.IsHostileDistanceGood)
                {
                    Felblade.Cast();
                    return;
                }
                if (MySettings.UseFelEruption && FelEruption.IsSpellUsable && FelEruption.IsHostileDistanceGood)
                {
                    FelEruption.Cast();
                    return;
                }
                if (MySettings.UseSpiritBomb && SpiritBomb.IsSpellUsable && SoulFragments.BuffStack >= 1 &&
                    SpiritBomb.IsHostileDistanceGood && !Frailty.TargetHaveBuff)
                {
                    SpiritBomb.Cast();
                    return;
                }
                if (MySettings.UseShear && Shear.IsSpellUsable && Shear.IsHostileDistanceGood &&
                    BladeTurningBuff.HaveBuff)
                {
                    Shear.Cast();
                    return;
                }
                if (MySettings.UseFracture && Fracture.IsSpellUsable && Fracture.IsHostileDistanceGood &&
                    //Spend Pain
                    ObjectManager.Me.Pain >= 60 && ObjectManager.Me.Health > 50)
                {
                    Fracture.Cast();
                    return;
                }
                if (SigilofFlameTimer.IsReady)
                {
                    if (MySettings.UseSigilofFlame && SigilofFlame.IsSpellUsable &&
                        SigilofFlame.IsHostileDistanceGood)
                    {
                        SigilofFlame.Cast();
                        SigilofFlameTimer = new Timer(1000*8);
                        return;
                    }
                    if (MySettings.UseInfernalStrike && InfernalStrike.IsSpellUsable &&
                        InfernalStrike.IsHostileDistanceGood && FlameCrash.HaveBuff)
                    {
                        InfernalStrike.Cast();
                        SigilofFlameTimer = new Timer(1000*8);
                        return;
                    }
                }
                if (MySettings.UseShear && Shear.IsSpellUsable && Shear.IsHostileDistanceGood)
                {
                    Shear.Cast();
                    return;
                }
            }
                //Multiple Target
            else
            {
                if (MySettings.UseSpiritBomb && SpiritBomb.IsSpellUsable && SoulFragments.BuffStack >= 1 &&
                    SpiritBomb.IsHostileDistanceGood && !Frailty.TargetHaveBuff)
                {
                    SpiritBomb.Cast();
                    return;
                }
                if (MySettings.UseFelblade && Felblade.IsSpellUsable && Felblade.IsHostileDistanceGood)
                {
                    Felblade.Cast();
                    return;
                }
                if (MySettings.UseShear && Shear.IsSpellUsable && Shear.IsHostileDistanceGood &&
                    BladeTurningBuff.HaveBuff)
                {
                    Shear.Cast();
                    return;
                }
                if (SigilofFlameTimer.IsReady)
                {
                    if (MySettings.UseSigilofFlame && SigilofFlame.IsSpellUsable && SigilofFlame.IsHostileDistanceGood)
                    {
                        SigilofFlame.CastAtPosition(ObjectManager.Target.Position);
                        SigilofFlameTimer = new Timer(1000*8);
                        return;
                    }
                    if (FlameCrash.HaveBuff && MySettings.UseInfernalStrike && InfernalStrike.IsSpellUsable && InfernalStrike.IsHostileDistanceGood)
                    {
                        InfernalStrike.CastAtPosition(ObjectManager.Target.Position);
                        SigilofFlameTimer = new Timer(1000*8);
                        return;
                    }
                }
                if (MySettings.UseFelEruption && FelEruption.IsSpellUsable && FelEruption.IsHostileDistanceGood)
                {
                    FelEruption.Cast();
                    return;
                }
                if (MySettings.UseShear && Shear.IsSpellUsable && Shear.IsHostileDistanceGood)
                {
                    Shear.Cast();
                    return;
                }
                if (MySettings.UseInfernalStrike && InfernalStrike.IsSpellUsable && InfernalStrike.IsHostileDistanceGood)
                {
                    InfernalStrike.CastAtPosition(ObjectManager.Target.Position);
                    if (FlameCrash.HaveBuff)
                        SigilofFlameTimer = new Timer(1000*8);
                    return;
                }
            }
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    #region Nested type: DemonHunterVengeanceSettings

    [Serializable]
    public class DemonHunterVengeanceSettings : Settings
    {
        /* Professions & Racials */
        //public bool UseArcaneTorrent = true;
        public int UseGiftoftheNaaruBelowPercentage = 50;

        /* Artifact Spells */
        public bool UseSoulCarver = true;

        /* Offensive Spells */
        public bool UseFelblade = true;
        public bool UseFelDevastation = true;
        public bool UseFelEruption = true;
        public bool UseFracture = true;
        public bool UseImmolationAura = true;
        public bool UseInfernalStrike = true;
        public bool UseShear = true;
        public bool UseSigilofFlame = true;
        public bool UseSoulCleave = true;
        public int UseSoulCleaveBelowPercentage = 40;
        public bool UseSpiritBomb = true;

        /* Defensive Cooldowns */
        public int UseDemonSpikesBelowPercentage = 50;
        public int UseFieryBrandBelowPercentage = 60;
        public int UseEmpowerWardsBelowPercentage = 0;
        public int UseMetamorphosisBelowPercentage = 25;

        /* Utility Spells */
        //public bool UseConsumeMagic = true;
        //public bool UseImprison = true;
        public int UseTormentBelowToTPercentage = 50;

        /* Game Settings */
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;

        public DemonHunterVengeanceSettings()
        {
            ConfigWinForm("DemonHunter Vengeance Settings");
            /* Professions & Racials */
            //AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaruBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            /* Artifact Spells */
            AddControlInWinForm("Use Soul Carver", "UseSoulCarver", "Artifact Spells");
            /* Offensive Spells */
            AddControlInWinForm("Use Felblade", "UseFelblade", "Offensive Spells");
            AddControlInWinForm("Use Fel Devastation", "UseFelDevastation", "Offensive Spells");
            AddControlInWinForm("Use Fel Eruption", "UseFelEruption", "Offensive Spells");
            AddControlInWinForm("Use Fracture", "UseFracture", "Offensive Spells");
            AddControlInWinForm("Use Immolation Aura", "UseImmolationAura", "Offensive Spells");
            AddControlInWinForm("Use Infernal Strike", "UseInfernalStrike", "Offensive Spells");
            AddControlInWinForm("Use Shear", "UseShear", "Offensive Spells");
            AddControlInWinForm("Use Sigil of Flame", "UseSigilofFlame", "Offensive Spells");
            AddControlInWinForm("Use Soul Cleave", "UseSoulCleave", "Offensive Spells");
            AddControlInWinForm("Use Soul Cleave", "UseSoulCleaveBelowPercentage", "Offensive Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Spirit Bomb", "UseSpiritBomb", "Offensive Spells");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Demon Spikes", "UseDemonSpikesBelowPercentage", "Defensive Cooldowns", "BelowPercentage", "Life");
            AddControlInWinForm("Use Fiery Brand", "UseFieryBrandBelowPercentage", "Defensive Cooldowns", "BelowPercentage", "Life");
            AddControlInWinForm("Use Empower Wards", "UseEmpowerWardsBelowPercentage", "Defensive Cooldowns", "BelowPercentage", "Life");
            AddControlInWinForm("Use Metamorphosis", "UseMetamorphosisBelowPercentage", "Defensive Cooldowns", "BelowPercentage", "Life");
            /* Utility Spells */
            AddControlInWinForm("Use Torment", "UseTormentBelowToTPercentage", "Utility Spells", "BelowPercentage", "Target Life");
            /* Game Settings */
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");
        }

        public static DemonHunterVengeanceSettings CurrentSetting { get; set; }

        public static DemonHunterVengeanceSettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\DemonHunter_Vengeance.xml";
            if (File.Exists(currentSettingsFile))
            {
                return
                    CurrentSetting = Load<DemonHunterVengeanceSettings>(currentSettingsFile);
            }
            return new DemonHunterVengeanceSettings();
        }
    }

    #endregion
}

#endregion

// ReSharper restore ObjectCreationAsStatement
// ReSharper restore EmptyGeneralCatchClause