/*
* CustomClass for TheNoobBot
* Credit : Rival, Geesus, Enelya, Marstor, Vesper, Neo2003, DreadLocks
* Thanks you !
*/

using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;
using Keybindings = nManager.Wow.Enums.Keybindings;
using Timer = nManager.Helpful.Timer;

public class Main : ICustomClass
{
    internal static float range = 5.0f;
    internal static bool loop = true;

    public float Range
    {
        get { return range; }
        set { range = value; }
    }

    public void Initialize()
    {
        Initialize(false);
    }

    public void Initialize(bool ConfigOnly)
    {
        try
        {
            Logging.WriteFight("Loading combat system.");

            switch (ObjectManager.Me.WowClass)
            {
                    #region ShamanSpecialisation checking

                case WoWClass.Shaman:
                    var Shaman_Enhancement_Spell = new Spell("Lava Lash");
                    var Shaman_Elemental_Spell = new Spell("Thunderstorm");
                    var Shaman_Restoration_Spell = new Spell("Riptide");

                    if (Shaman_Enhancement_Spell.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Shaman_Enhancement.xml";
                            Shaman_Enhancement.ShamanEnhancementSettings CurrentSetting;
                            CurrentSetting = new Shaman_Enhancement.ShamanEnhancementSettings();
                            if (System.IO.File.Exists(CurrentSettingsFile))
                            {
                                CurrentSetting =
                                    Settings.Load<Shaman_Enhancement.ShamanEnhancementSettings>(CurrentSettingsFile);
                            }
                            CurrentSetting.ToForm();
                            CurrentSetting.Save(CurrentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Shaman Enhancement class...");
                            new Shaman_Enhancement();
                        }
                        break;
                    }

                    else if (Shaman_Elemental_Spell.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Shaman_Elemental.xml";
                            Shaman_Elemental.ShamanElementalSettings CurrentSetting;
                            CurrentSetting = new Shaman_Elemental.ShamanElementalSettings();
                            if (System.IO.File.Exists(CurrentSettingsFile))
                            {
                                CurrentSetting =
                                    Settings.Load<Shaman_Elemental.ShamanElementalSettings>(CurrentSettingsFile);
                            }
                            CurrentSetting.ToForm();
                            CurrentSetting.Save(CurrentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Shaman Elemental class...");
                            range = 30.0f;
                            new Shaman_Elemental();
                        }
                        break;
                    }

                    else if (Shaman_Restoration_Spell.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Shaman_Restoration.xml";
                            Shaman_Restoration.ShamanRestorationSettings CurrentSetting;
                            CurrentSetting = new Shaman_Restoration.ShamanRestorationSettings();
                            if (System.IO.File.Exists(CurrentSettingsFile))
                            {
                                CurrentSetting =
                                    Settings.Load<Shaman_Restoration.ShamanRestorationSettings>(CurrentSettingsFile);
                            }
                            CurrentSetting.ToForm();
                            CurrentSetting.Save(CurrentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Shaman Restoration class...");
                            range = 30.0f;
                            new Shaman_Restoration();
                        }
                        break;
                    }

                    else
                    {
                        if (ConfigOnly)
                        {
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Shaman_Restoration.xml";
                            Shaman_Restoration.ShamanRestorationSettings CurrentSetting;
                            CurrentSetting = new Shaman_Restoration.ShamanRestorationSettings();
                            if (System.IO.File.Exists(CurrentSettingsFile))
                            {
                                CurrentSetting =
                                    Settings.Load<Shaman_Restoration.ShamanRestorationSettings>(CurrentSettingsFile);
                            }
                            CurrentSetting.ToForm();
                            CurrentSetting.Save(CurrentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Shaman without Spec");
                            range = 30.0f;
                            new Shaman_Restoration();
                        }
                        break;
                    }

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

    public void Dispose()
    {
        Logging.WriteFight("Combat system stopped.");
        loop = false;
    }

    public void ShowConfiguration()
    {
        Directory.CreateDirectory(Application.StartupPath + "\\CustomClasses\\Settings\\");
        Initialize(true);
    }
}

#region Shaman

public class Shaman_Enhancement
{
    [Serializable]
    public class ShamanEnhancementSettings : nManager.Helpful.Settings
    {
        /* Professions & Racials */
        public bool UseArcaneTorrent = true;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseLifeblood = true;
        public bool UseStoneform = true;
        public bool UseGiftoftheNaaru = true;
        public bool UseWarStomp = true;
        /* Shaman Buffs */
        public bool UseFlametongueWeapon = true;
        public bool UseFrostbrandWeapon = false;
        public bool UseGhostWolf = true;
        public bool UseLightningShield = true;
        public bool UseRockbiterWeapon = false;
        public bool UseSpiritwalkersGrace = true;
        public bool UseWaterShield = true;
        public bool UseWaterWalking = true;
        public bool UseWindfuryWeapon = true;
        /* Offensive Spell */
        public bool UseChainLightning = true;
        public bool UseEarthShock  = true;
        public bool UseFireNova = true;
        public bool UseFlameShock = true;
        public bool UseFrostShock = false;
        public bool UseLavaLash = true;
        public bool UseLightningBolt = true;
        public bool UseMagmaTotem = true;
        public bool UseSearingTotem = true;
        public bool UseStormstrike = true;
        /* Offensive Cooldown */
        public bool UseAncestralSwiftness = true;
        public bool UseAscendance = true;
        public bool UseBloodlustHeroism = true;
        public bool UseCalloftheElements = true;
        public bool UseEarthElementalTotem = true;
        public bool UseElementalBlast = true;
        public bool UseElementalMastery = true;
        public bool UseFeralSpirit = true;
        public bool UseFireElementalTotem = true;
        public bool UseStormlashTotem = true;
        public bool UseTotemicProjection = true;
        public bool UseUnleashElements = true;
        /* Defensive Cooldown */
        public bool UseAstralShift = true;
        public bool UseCapacitorTotem = true;
        public bool UseEarthbindTotem = false;
        public bool UseGroundingTotem = true;
        public bool UseShamanisticRage = true;
        public bool UseStoneBulwarkTotem = true;
        public bool UseWindShear = true;
        /* Healing Spell */
        public bool UseAncestralGuidance = true;
        public bool UseChainHeal = false;
        public bool UseHealingRain = true;
        public bool UseHealingSurge = true;
        public bool UseHealingStreamTotem = true;
        public bool UseHealingTideTotem = true;
        public bool UseTotemicRecall = true;
        /* Game Settings */
        public bool UseLowCombat = true;
        public bool UseTrinket = true;
        public bool UseEngGlove = true;
        public bool UseAlchFlask = true;

        public ShamanEnhancementSettings()
        {
            ConfigWinForm(new System.Drawing.Point(400, 400), "Shaman Enhancement Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");
            AddControlInWinForm("Use Lifeblood", "UseLifeblood", "Professions & Racials");
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
            AddControlInWinForm("Use Healing Stream Totem", "UseHealingStream_Totem", "Healing Spell");
            AddControlInWinForm("Use Healing Tide Totem", "UsHealingTide_Totem", "Healing Spell");
            AddControlInWinForm("Use Totemic Recall", "UseTotemicRecall", "Healing Spell");
            /* Game Settings */
            AddControlInWinForm("Use Low Combat Settings", "UseLowCombat", "Game Settings");
            AddControlInWinForm("Use Trinket", "UseTrinket", "Game Settings");
            AddControlInWinForm("Use Engineering Gloves", "UseEngGlove", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
        }

        public static ShamanEnhancementSettings CurrentSetting { get; set; }

        public static ShamanEnhancementSettings GetSettings()
        {
            string CurrentSettingsFile = Application.StartupPath + "\\CustomClasses\\Settings\\Shaman_Enhancement.xml";
            if (System.IO.File.Exists(CurrentSettingsFile))
            {
                return
                    CurrentSetting = Settings.Load<Shaman_Enhancement.ShamanEnhancementSettings>(CurrentSettingsFile);
            }
            else
            {
                return new Shaman_Enhancement.ShamanEnhancementSettings();
            }
        }
    }

    private readonly ShamanEnhancementSettings MySettings = ShamanEnhancementSettings.GetSettings();

    #region Professions & Racials

    private readonly Spell Arcane_Torrent = new Spell("Arcane Torrent");
    private readonly Spell Berserking = new Spell("Berserking");
    private readonly Spell Blood_Fury = new Spell("Blood Fury");
    private readonly Spell Lifeblood = new Spell("Lifeblood");
    private readonly Spell Stoneform = new Spell("Stoneform");
    private readonly Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    private readonly Spell War_Stomp = new Spell("War Stomp");
    private readonly Spell Engineering = new Spell("Engineering");
    private readonly Spell Alchemy = new Spell("Alchemy");

    #endregion

    #region Shaman Buffs

    private readonly Spell Flametongue_Weapon = new Spell("Flametongue Weapon");
    private readonly Spell Frostbrand_Weapon = new Spell("Frostbrand Weapon");
    private readonly Spell Ghost_Wolf = new Spell("Ghost Wolf");
    private readonly Spell Lightning_Shield = new Spell("Lightning Shield");
    private readonly Spell Rockbiter_Weapon = new Spell("Rockbiter Weapon");
    private readonly Spell Spiritwalkers_Grace = new Spell("Spiritwalker's Grace");
    private readonly Spell Water_Shield = new Spell("Water Shield");
    private readonly Spell Water_Walking = new Spell("Water Walking");
    private readonly Spell Windfury_Weapon = new Spell("Windfury Weapon");
    private Timer Water_Walking_Timer = new Timer(0);

    #endregion

    #region Offensive Spell

    private readonly Spell Chain_Lightning = new Spell("Chain Lightning");
    private readonly Spell Earth_Shock = new Spell("Earth Shock");
    private readonly Spell Fire_Nova = new Spell("Fire Nova");
    private readonly Spell Flame_Shock = new Spell("Flame Shock");
    private Timer Flame_Shock_Timer = new Timer(0);
    private readonly Spell Frost_Shock = new Spell("Frost Shock");
    private readonly Spell Lava_Lash = new Spell("Lava Lash");
    private readonly Spell Lightning_Bolt = new Spell("Lightning Bolt");
    private readonly Spell Magma_Totem = new Spell("Magma Totem");
    private readonly Spell Primal_Strike = new Spell("Primal Strike");
    private readonly Spell Searing_Totem = new Spell("Searing Totem");
    private readonly Spell Stormstrike = new Spell("Stormstrike");

    #endregion

    #region Offensive Cooldown

    private readonly Spell Ancestral_Swiftness = new Spell("Ancestral Swiftness");
    private readonly Spell Ascendance = new Spell("Ascendance");
    private readonly Spell Bloodlust = new Spell("Bloodlust");
    private readonly Spell Call_of_the_Elements = new Spell("Call of the Elements");
    private readonly Spell Earth_Elemental_Totem = new Spell("Earth Elemental Totem");
    private readonly Spell Elemental_Blast = new Spell("Elemental Blast");
    private readonly Spell Elemental_Mastery = new Spell("Elemental Mastery");
    private readonly Spell Feral_Spirit = new Spell("Feral Spirit");
    private readonly Spell Fire_Elemental_Totem = new Spell("Fire Elemental Totem");
    private readonly Spell Heroism = new Spell("Heroism");
    private readonly Spell Stormlash_Totem = new Spell("Stormlash Totem");
    private readonly Spell Totemic_Projection = new Spell("Totemic Projection");
    private readonly Spell Unleash_Elements = new Spell("Unleash Elements");
    private readonly Spell Unleashed_Fury = new Spell("Unleashed Fury");

    #endregion

    #region Defensive Cooldown

    private readonly Spell Astral_Shift = new Spell("Astral Shift");
    private readonly Spell Capacitor_Totem = new Spell("Capacitor Totem");
    private readonly Spell Earthbind_Totem = new Spell("Earthbind Totem");
    private readonly Spell Grounding_Totem = new Spell("Grounding Totem");
    private readonly Spell Shamanistic_Rage = new Spell("Shamanistic Rage");
    private readonly Spell Stone_Bulwark_Totem = new Spell("Stone Bulwark Totem");
    private readonly Spell Wind_Shear = new Spell("Wind Shear");

    #endregion

    #region Healing Spell

    private readonly Spell Ancestral_Guidance = new Spell("Ancestral Guidance");
    private readonly Spell Chain_Heal = new Spell("Chain Heal");
    private readonly Spell Healing_Rain = new Spell("Healing Rain");
    private readonly Spell Healing_Surge = new Spell("Healing Surge");
    private readonly Spell Healing_Stream_Totem = new Spell("Healing Stream Totem");
    private readonly Spell Healing_Tide_Totem = new Spell("Healing Tide Totem");
    private readonly Spell Totemic_Recall = new Spell("Totemic Recall");

    #endregion

    private Timer OnCD = new Timer(0);
    private Timer Trinket_Timer = new Timer(0);
    private Timer Engineering_Timer = new Timer(0);
    private Timer AlchFlask_Timer = new Timer(0);
    public int LC = 0;

    public Shaman_Enhancement()
    {
        Main.range = 5.0f;
        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            try
            {
                if (!ObjectManager.Me.IsMounted)
                {
                    if (Fight.InFight && ObjectManager.Me.Target > 0)
                    {
                        if (ObjectManager.Me.Target != lastTarget &&
                            (Flame_Shock.IsDistanceGood || Earth_Shock.IsDistanceGood))
                        {
                            Pull();
                            lastTarget = ObjectManager.Me.Target;
                        }

                        if (ObjectManager.Target.Level < 70 && ObjectManager.Me.Level > 84
                            && MySettings.UseLowCombat)
                        {
                            LC = 1;
                            LowCombat();
                        }
                        else
                        {
                            LC = 0;
                            Combat();
                        }
                    }
                    else
                        if (!ObjectManager.Me.IsCast)
                            Patrolling();
                }
            }
            catch
            {
            }
            Thread.Sleep(150);
        }
    }

    public void Pull()
    {
        if (Totemic_Projection.KnownSpell && Totemic_Projection.IsSpellUsable && MySettings.UseTotemicProjection)
            Totemic_Projection.Launch();

        if (Flame_Shock.KnownSpell && Flame_Shock.IsSpellUsable && Flame_Shock.IsDistanceGood
            && MySettings.UseFlameShock && LC != 1)
        {
            Flame_Shock.Launch();
            return;
        }
        else
        {
            if (Earth_Shock.KnownSpell && Earth_Shock.IsSpellUsable && Earth_Shock.IsDistanceGood
                && MySettings.UseEarthShock)
            {
                Earth_Shock.Launch();
                return;
            }
        }
    }

    public void Combat()
    {
        AvoidMelee();
        if (OnCD.IsReady)
            Defense_Cycle();
        Heal();
        Decast();
        Buff();
        DPS_Burst();
        DPS_Cycle();
    }

    public void LowCombat()
    {
        AvoidMelee();
        Heal();
        Defense_Cycle();
        Buff();

        if (Earth_Shock.KnownSpell && Earth_Shock.IsSpellUsable && Earth_Shock.IsDistanceGood
            && MySettings.UseEarthShock)
        {
            Earth_Shock.Launch();
            return;
        }
        // Blizzard API Calls for Stormstrike using Primal Strike Function
        else if (Primal_Strike.KnownSpell && Primal_Strike.IsSpellUsable && Primal_Strike.IsDistanceGood
            && MySettings.UseStormstrike)
        {
            Primal_Strike.Launch();
            return;
        }
        else if (Chain_Lightning.KnownSpell && Chain_Lightning.IsSpellUsable && Chain_Lightning.IsDistanceGood
            && MySettings.UseChainLightning)
        {
            Chain_Lightning.Launch();
            return;
        }
        else
        {
            if (Searing_Totem.KnownSpell && Searing_Totem.IsSpellUsable && FireTotemReady()
                && MySettings.UseSearingTotem)
            {
                if (!Searing_Totem.SummonedByMeBySpellName(5))
                    Searing_Totem.Launch();
                return;
            }
        }

        if (Magma_Totem.KnownSpell && Magma_Totem.IsSpellUsable && ObjectManager.Target.GetDistance < 8
            && MySettings.UseMagmaTotem && FireTotemReady())
        {
            Magma_Totem.Launch();
            return;
        }
    }

    public void DPS_Burst()
    {
        if (MySettings.UseTrinket && Trinket_Timer.IsReady && ObjectManager.Target.GetDistance < 30)
        {
            Logging.WriteFight("Use Trinket 1.");
            Lua.RunMacroText("/use 13");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Logging.WriteFight("Use Trinket 2.");
            Lua.RunMacroText("/use 14");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Trinket_Timer = new Timer(1000*60*2);
            return;
        }
        else if (Berserking.IsSpellUsable && Berserking.KnownSpell && MySettings.UseBerserking
            && ObjectManager.Target.GetDistance < 30)
        {
            Berserking.Launch();
            return;
        }
        else if (Blood_Fury.IsSpellUsable && Blood_Fury.KnownSpell && MySettings.UseBloodFury
            && ObjectManager.Target.GetDistance < 30)
        {
            Blood_Fury.Launch();
            return;
        }
        else if (Lifeblood.IsSpellUsable && Lifeblood.KnownSpell && MySettings.UseLifeblood
            && ObjectManager.Target.GetDistance < 30)
        {
            Lifeblood.Launch();
            return;
        }
        else if (MySettings.UseEngGlove && Engineering.KnownSpell && Engineering_Timer.IsReady
            && ObjectManager.Target.GetDistance < 30)
        {
            Logging.WriteFight("Use Engineering Gloves.");
            Lua.RunMacroText("/use 10");
            Engineering_Timer = new Timer(1000*60);
            return;
        }
        else if (Unleash_Elements.KnownSpell && Unleash_Elements.IsSpellUsable && Unleashed_Fury.KnownSpell
            && MySettings.UseUnleashElements && Unleash_Elements.IsDistanceGood)
        {
            Unleash_Elements.Launch();
            return;
        }
        else if (Elemental_Blast.KnownSpell && Elemental_Blast.IsSpellUsable
            && MySettings.UseElementalBlast && Elemental_Blast.IsDistanceGood)
        {
            Elemental_Blast.Launch();
            return;
        }
        else if (Ascendance.KnownSpell && Ascendance.IsSpellUsable
            && MySettings.UseAscendance && ObjectManager.Target.GetDistance < 30)
        {
            Ascendance.Launch();
            return;
        }
        else if (Fire_Elemental_Totem.KnownSpell && Fire_Elemental_Totem.IsSpellUsable
            && MySettings.UseFireElementalTotem && ObjectManager.Target.GetDistance < 30)
        {
            Fire_Elemental_Totem.Launch();
            return;
        }
        else if (Stormlash_Totem.KnownSpell && AirTotemReady()
            && MySettings.UseStormlashTotem && ObjectManager.Target.GetDistance < 30)
        {
            if (!Stormlash_Totem.IsSpellUsable && MySettings.UseCalloftheElements
                && Call_of_the_Elements.KnownSpell && Call_of_the_Elements.IsSpellUsable)
            {
                Call_of_the_Elements.Launch();
                Thread.Sleep(200);
            }

            if (Stormlash_Totem.IsSpellUsable)
                Stormlash_Totem.Launch();
            return;
        }
        else if (Feral_Spirit.KnownSpell && Feral_Spirit.IsSpellUsable
            && MySettings.UseFeralSpirit && ObjectManager.Target.GetDistance < 30)
        {
            Feral_Spirit.Launch();
            return;
        }
        else if (Bloodlust.KnownSpell && Bloodlust.IsSpellUsable && MySettings.UseBloodlustHeroism 
            && ObjectManager.Target.GetDistance < 30 && !ObjectManager.Me.HaveBuff(57724))
        {
            Bloodlust.Launch();
            return;
        }

        else if (Heroism.KnownSpell && Heroism.IsSpellUsable && MySettings.UseBloodlustHeroism 
            && ObjectManager.Target.GetDistance < 30 && !ObjectManager.Me.HaveBuff(57723))
        {
            Heroism.Launch();
            return;
        }
        else
        {
            if (Elemental_Mastery.KnownSpell && Elemental_Mastery.IsSpellUsable
                && !ObjectManager.Me.HaveBuff(2825) && MySettings.UseElementalMastery
                && !ObjectManager.Me.HaveBuff(32182))
            {
                Elemental_Mastery.Launch();
                return;
            }
        }
    }

    public void DPS_Cycle()
    {
        if (Earth_Elemental_Totem.KnownSpell && Earth_Elemental_Totem.IsSpellUsable
            && ObjectManager.GetNumberAttackPlayer() > 3 && MySettings.UseEarthElementalTotem)
        {
            Earth_Elemental_Totem.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 5 && Magma_Totem.KnownSpell
            && Magma_Totem.IsSpellUsable && MySettings.UseMagmaTotem
            && !Fire_Elemental_Totem.SummonedByMeBySpellName())
        {
            Magma_Totem.Launch();
            return;
        }
        else if (Searing_Totem.KnownSpell && Searing_Totem.IsSpellUsable && MySettings.UseSearingTotem
            && FireTotemReady() && !Searing_Totem.SummonedByMeBySpellName(25))
        {
            Searing_Totem.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 2 && Chain_Lightning.KnownSpell 
            && Chain_Lightning.IsSpellUsable && Chain_Lightning.IsDistanceGood 
            && MySettings.UseChainLightning && ObjectManager.Me.BuffStack(53817) == 5)
        {
            Chain_Lightning.Launch();
            return;
        }
        else if (Lightning_Bolt.IsDistanceGood && Lightning_Bolt.KnownSpell && Lightning_Bolt.IsSpellUsable
                && MySettings.UseLightningBolt && ObjectManager.Me.BuffStack(53817) == 5)
        {
            Lightning_Bolt.Launch();
            return;
        }
        else if (Flame_Shock.IsSpellUsable && Flame_Shock.IsDistanceGood && Flame_Shock.KnownSpell
            && MySettings.UseFlameShock && (!Flame_Shock.TargetHaveBuff || Flame_Shock_Timer.IsReady))
        {
            if (Unleash_Elements.KnownSpell && Unleash_Elements.IsSpellUsable && Unleash_Elements.IsDistanceGood
                && MySettings.UseUnleashElements)
            {
                Unleash_Elements.Launch();
                Thread.Sleep(200);
            }
            Flame_Shock.Launch();
            Flame_Shock_Timer = new Timer(1000 * 27);
            return;
        }
        else if (Fire_Nova.KnownSpell && Fire_Nova.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() > 2
            && MySettings.UseFireNova)
        {
            Fire_Nova.Launch();
            return;
        }
        // Blizzard API Calls for Stormstrike using Primal Strike Function
        else if (Primal_Strike.KnownSpell && Primal_Strike.IsSpellUsable && Primal_Strike.IsDistanceGood
            && MySettings.UseStormstrike)
        {
            Primal_Strike.Launch();
            return;
        }
        else if (Lava_Lash.KnownSpell && Lava_Lash.IsSpellUsable && Lava_Lash.IsDistanceGood
            && MySettings.UseLavaLash)
        {
            Lava_Lash.Launch();
            return;
        }
        else if (Unleash_Elements.KnownSpell && Unleash_Elements.IsSpellUsable && Unleash_Elements.IsDistanceGood
            && MySettings.UseUnleashElements)
        {
            Unleash_Elements.Launch();
            return;
        }
        else if (Earth_Shock.IsSpellUsable && Earth_Shock.KnownSpell && Earth_Shock.IsDistanceGood
            && Flame_Shock.TargetHaveBuff && MySettings.UseEarthShock)
        {
            Earth_Shock.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 2 && Chain_Lightning.KnownSpell 
            && Chain_Lightning.IsSpellUsable && Chain_Lightning.IsDistanceGood 
            && MySettings.UseChainLightning && ObjectManager.Me.BuffStack(53817) > 0)
        {
            if (Ancestral_Swiftness.KnownSpell && Ancestral_Swiftness.IsSpellUsable
                && MySettings.UseAncestralSwiftness)
            {
                Ancestral_Swiftness.Launch();
                Thread.Sleep(200);
            }
            Chain_Lightning.Launch();
            return;
        }
        else
        {
            if (Lightning_Bolt.IsDistanceGood && Lightning_Bolt.KnownSpell && Lightning_Bolt.IsSpellUsable
                && MySettings.UseLightningBolt && ObjectManager.Me.BuffStack(53817) > 0)
            {
                if (Ancestral_Swiftness.KnownSpell && Ancestral_Swiftness.IsSpellUsable
                    && MySettings.UseAncestralSwiftness)
                {
                    Ancestral_Swiftness.Launch();
                    Thread.Sleep(200);
                }
                Lightning_Bolt.Launch();
                return;
            }
        }
    }

    public void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            Heal();
            Buff();
        }
    }

    private void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (Water_Walking.IsSpellUsable && Water_Walking.KnownSpell && (!Water_Walking.HaveBuff || Water_Walking_Timer.IsReady)
            && ObjectManager.GetNumberAttackPlayer() == 0 && !Fight.InFight && MySettings.UseWaterWalking)
        {
            Water_Walking.Launch();
            Water_Walking_Timer = new Timer(1000*60*9);
            return;
        }
        else if ((ObjectManager.Me.ManaPercentage < 5 && Water_Shield.KnownSpell && Water_Shield.IsSpellUsable
            && MySettings.UseWaterShield && !Water_Shield.HaveBuff) || !MySettings.UseLightningShield)
        {
            Water_Shield.Launch();
            return;
        }
        else if (Lightning_Shield.KnownSpell && Lightning_Shield.IsSpellUsable && !Lightning_Shield.HaveBuff
            && MySettings.UseLightningShield && ObjectManager.Me.ManaPercentage > 15)
        {
            Lightning_Shield.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 0 && Spiritwalkers_Grace.IsSpellUsable 
            && Spiritwalkers_Grace.KnownSpell && MySettings.UseSpiritwalkersGrace && ObjectManager.Me.GetMove)
        {
            Spiritwalkers_Grace.Launch();
            return;
        }
        else
        {
            if (Windfury_Weapon.KnownSpell && Windfury_Weapon.IsSpellUsable && !ObjectManager.Me.HaveBuff(33757)
                && MySettings.UseWindfuryWeapon)
            {
                Windfury_Weapon.Launch();
                return;
            }
            else if (Frostbrand_Weapon.KnownSpell && Frostbrand_Weapon.IsSpellUsable && !ObjectManager.Me.HaveBuff(8034)
                && MySettings.UseFrostbrandWeapon && !MySettings.UseWindfuryWeapon)
            {
                Frostbrand_Weapon.Launch();
                return;
            }
            else
            {
                if (Rockbiter_Weapon.KnownSpell && Rockbiter_Weapon.IsSpellUsable && !ObjectManager.Me.HaveBuff(36494)
                    && MySettings.UseRockbiterWeapon && !MySettings.UseWindfuryWeapon 
                    && !MySettings.UseFrostbrandWeapon)
                {
                    Rockbiter_Weapon.Launch();
                    return;
                }
            }

            if (Flametongue_Weapon.KnownSpell && Flametongue_Weapon.IsSpellUsable && !ObjectManager.Me.HaveBuff(10400)
                    && MySettings.UseFlametongueWeapon && (ObjectManager.Me.HaveBuff(33757)
                    || ObjectManager.Me.HaveBuff(8034) || ObjectManager.Me.HaveBuff(36494)))
            {
                Flametongue_Weapon.Launch();
                return;
            }
        }

        if (ObjectManager.GetNumberAttackPlayer() == 0 && Ghost_Wolf.IsSpellUsable && Ghost_Wolf.KnownSpell
            && MySettings.UseGhostWolf && ObjectManager.Me.GetMove)
        {
            Ghost_Wolf.Launch();
            return;
        }
    }

    public bool FireTotemReady()
    {
        if (Fire_Elemental_Totem.SummonedByMeBySpellName() || Magma_Totem.SummonedByMeBySpellName())
            return false;
        return true;
    }

    public bool EarthTotemReady()
    {
        if (Earthbind_Totem.SummonedByMeBySpellName() || Earth_Elemental_Totem.SummonedByMeBySpellName()
            || Stone_Bulwark_Totem.SummonedByMeBySpellName())
            return false;
        return true;
    }

    public bool WaterTotemReady()
    {
        if (Healing_Stream_Totem.SummonedByMeBySpellName() || Healing_Tide_Totem.SummonedByMeBySpellName())
            return false;
        return true;
    }

    public bool AirTotemReady()
    {
        if (Capacitor_Totem.SummonedByMeBySpellName() || Grounding_Totem.SummonedByMeBySpellName() 
            || Stormlash_Totem.SummonedByMeBySpellName())
            return false;
        return true;
    }

    public bool TotemicRecallReady()
    {
        if (Fire_Elemental_Totem.SummonedByMeBySpellName())
            return false;
        else if (Earth_Elemental_Totem.SummonedByMeBySpellName())
            return false;
        else if (Searing_Totem.SummonedByMeBySpellName())
            return true;
        else if (FireTotemReady() && EarthTotemReady() && WaterTotemReady() && AirTotemReady())
            return false;
        else
            return true;
    }

    private void Heal()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (ObjectManager.Me.ManaPercentage < 50 && Totemic_Recall.KnownSpell && Totemic_Recall.IsSpellUsable
            && MySettings.UseTotemicRecall && ObjectManager.GetNumberAttackPlayer() == 0 && !Fight.InFight
            && TotemicRecallReady())
        {
            Totemic_Recall.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 95 && Healing_Surge.KnownSpell && Healing_Surge.IsSpellUsable
            && ObjectManager.GetNumberAttackPlayer() == 0 && !Fight.InFight && MySettings.UseHealingSurge)
        {
            Healing_Surge.Launch();
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(200);
            }
            return;
        }
        else if (Healing_Surge.KnownSpell && Healing_Surge.IsSpellUsable && ObjectManager.Me.HealthPercent < 50
                && MySettings.UseHealingSurge)
        {
            Healing_Surge.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && Gift_of_the_Naaru.KnownSpell && Gift_of_the_Naaru.IsSpellUsable
            && MySettings.UseGiftoftheNaaru)
        {
            Gift_of_the_Naaru.Launch();
            return;
        }
        else if (Healing_Tide_Totem.KnownSpell && Healing_Tide_Totem.IsSpellUsable && ObjectManager.Me.HealthPercent < 70
            && WaterTotemReady() && MySettings.UseHealingTideTotem)
        {
            Healing_Tide_Totem.Launch();
            return;
        }
        else if (Ancestral_Guidance.KnownSpell && Ancestral_Guidance.IsSpellUsable && ObjectManager.Me.HealthPercent < 70
            && MySettings.UseAncestralGuidance)
        {
            Ancestral_Guidance.Launch();
            return;
        }
        else if (Chain_Heal.KnownSpell && Chain_Heal.IsSpellUsable && ObjectManager.Me.HealthPercent < 80
            && MySettings.UseChainHeal)
        {
            Chain_Heal.Launch();
            return;
        }
        else
        {
            if (Healing_Stream_Totem.KnownSpell && Healing_Stream_Totem.IsSpellUsable && ObjectManager.Me.HealthPercent < 90
                && WaterTotemReady() && MySettings.UseHealingStreamTotem)
            {
                Healing_Stream_Totem.Launch();
                return;
            }
        }
    }

    private void Defense_Cycle()
    {
        if (ObjectManager.Me.HealthPercent < 50 && Capacitor_Totem.KnownSpell && Capacitor_Totem.IsSpellUsable
            && AirTotemReady() && MySettings.UseCapacitorTotem)
        {
            Capacitor_Totem.Launch();
            OnCD = new Timer(1000*5);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 50 && Stone_Bulwark_Totem.KnownSpell && Stone_Bulwark_Totem.IsSpellUsable
            && EarthTotemReady() && MySettings.UseStoneBulwarkTotem)
        {
            Stone_Bulwark_Totem.Launch();
            OnCD = new Timer(1000*10);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && War_Stomp.IsSpellUsable && War_Stomp.KnownSpell
            && MySettings.UseWarStomp)
        {
            War_Stomp.Launch();
            OnCD = new Timer(1000*2);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && Stoneform.IsSpellUsable && Stoneform.KnownSpell
            && MySettings.UseStoneform)
        {
            Stoneform.Launch();
            OnCD = new Timer(1000*8);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 90 && Shamanistic_Rage.IsSpellUsable 
            && Shamanistic_Rage.KnownSpell && MySettings.UseShamanisticRage)
        {
            Shamanistic_Rage.Launch();
            OnCD = new Timer(1000*15);
            return;
        }
        else
        {
            if (ObjectManager.Me.HealthPercent < 70 && Astral_Shift.KnownSpell && Astral_Shift.IsSpellUsable
                && MySettings.UseAstralShift)
            {
                Astral_Shift.Launch();
                OnCD = new Timer(1000*6);
                return;
            }
        }
    }

    private void Decast()
    {
        if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && MySettings.UseWindShear
            && Wind_Shear.KnownSpell && Wind_Shear.IsSpellUsable && Wind_Shear.IsDistanceGood)
        {
            Wind_Shear.Launch();
            return;
        }
        else if (ObjectManager.Target.IsCast && Arcane_Torrent.KnownSpell && Arcane_Torrent.IsSpellUsable
            && MySettings.UseArcaneTorrent && ObjectManager.Target.IsTargetingMe && ObjectManager.Target.GetDistance < 8)
        {
            Arcane_Torrent.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && MySettings.UseGroundingTotem
                && Grounding_Totem.KnownSpell && Grounding_Totem.IsSpellUsable && AirTotemReady())
            {
                Grounding_Totem.Launch();
                return;
            }
        }

        if (ObjectManager.Target.GetMove && !Frost_Shock.TargetHaveBuff && MySettings.UseFrostShock
            && Frost_Shock.KnownSpell && Frost_Shock.IsSpellUsable && Frost_Shock.IsDistanceGood)
        {
            Frost_Shock.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Target.GetMove && MySettings.UseEarthbindTotem && EarthTotemReady()
                && Earthbind_Totem.KnownSpell && Earthbind_Totem.IsSpellUsable && Earthbind_Totem.IsDistanceGood)
            {
                Earthbind_Totem.Launch();
                return;
            }
        }
    }

    private void AvoidMelee()
    {
        if (ObjectManager.Target.GetDistance < 3 && ObjectManager.Target.InCombat)
        {
            nManager.Wow.Helpers.Keybindings.PressKeybindings(nManager.Wow.Enums.Keybindings.MOVEBACKWARD);
        }
    }
}

public class Shaman_Restoration
{
    [Serializable]
    public class ShamanRestorationSettings : nManager.Helpful.Settings
    {
        /* Professions & Racials */
        public bool UseArcaneTorrent = true;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseLifeblood = true;
        public bool UseStoneform = true;
        public bool UseGiftoftheNaaru = true;
        public bool UseWarStomp = true;
        /* Shaman Buffs */
        public bool UseEarthShield = true;
        public bool UseEarthlivingWeapon = true;
        public bool UseFlametongueWeapon = true;
        public bool UseFrostbrandWeapon = false;
        public bool UseGhostWolf = true;
        public bool UseLightningShield = true;
        public bool UseRockbiterWeapon = false;
        public bool UseSpiritwalkersGrace = true;
        public bool UseWaterShield = true;
        public bool UseWaterWalking = true;
        /* Offensive Spell */
        public bool UseChainLightning = true;
        public bool UseEarthShock  = true;
        public bool UseFlameShock = true;
        public bool UseFrostShock = false;
        public bool UseLavaBurst = true;
        public bool UseLightningBolt = true;
        public bool UseMagmaTotem = true;
        public bool UsePrimalStrike = true;
        public bool UseSearingTotem = true;
        /* Offensive Cooldown */
        public bool UseAncestralSwiftness = true;
        public bool UseAscendance = true;
        public bool UseBloodlustHeroism = true;
        public bool UseCalloftheElements = true;
        public bool UseEarthElementalTotem = true;
        public bool UseElementalBlast = true;
        public bool UseElementalMastery = true;
        public bool UseFireElementalTotem = true;
        public bool UseStormlashTotem = true;
        public bool UseTotemicProjection = true;
        public bool UseUnleashElements = true;
        /* Defensive Cooldown */
        public bool UseAstralShift = true;
        public bool UseCapacitorTotem = true;
        public bool UseEarthbindTotem = false;
        public bool UseGroundingTotem = true;
        public bool UseStoneBulwarkTotem = true;
        public bool UseWindShear = true;
        /* Healing Spell */
        public bool UseAncestralGuidance = true;
        public bool UseChainHeal = false;
        public bool UseGreaterHealingWave = true;
        public bool UseHealingRain = true;
        public bool UseHealingSurge = true;
        public bool UseHealingStreamTotem = true;
        public bool UseHealingWave = false;
        public bool UseHealingTideTotem = true;
        public bool UseManaTideTotem = true;
        public bool UseRiptide = true;
        public bool UseSpiritLinkTotem = true;
        public bool UseTotemicRecall = true;
        /* Game Settings */
        public bool UseLowCombat = true;
        public bool UseTrinket = true;
        public bool UseEngGlove = true;
        public bool UseAlchFlask = true;

        public ShamanRestorationSettings()
        {
            ConfigWinForm(new System.Drawing.Point(400, 400), "Shaman Restoration Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");
            AddControlInWinForm("Use Lifeblood", "UseLifeblood", "Professions & Racials");
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
            AddControlInWinForm("Use Healing Stream Totem", "UseHealingStream_Totem", "Healing Spell");
            AddControlInWinForm("Use Healing Tide Totem", "UsHealingTideTotem", "Healing Spell");
            AddControlInWinForm("Use Healing Wave", "UseHealingWave", "Healing Spell");
            AddControlInWinForm("Use Mana Tide Totem", "UseManaTideTotem", "Healing Spell");
            AddControlInWinForm("Use Riptide", "UseRiptide", "Healing Spell");
            AddControlInWinForm("Use Spirit Link Totem", "UseSpiritLinkTotem", "Healing Spell");
            AddControlInWinForm("Use Totemic Recall", "UseTotemicRecall", "Healing Spell");
            /* Game Settings */
            AddControlInWinForm("Use Low Combat Settings", "UseLowCombat", "Game Settings");
            AddControlInWinForm("Use Trinket", "UseTrinket", "Game Settings");
            AddControlInWinForm("Use Engineering Gloves", "UseEngGlove", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
        }

        public static ShamanRestorationSettings CurrentSetting { get; set; }

        public static ShamanRestorationSettings GetSettings()
        {
            string CurrentSettingsFile = Application.StartupPath + "\\CustomClasses\\Settings\\Shaman_Restoration.xml";
            if (System.IO.File.Exists(CurrentSettingsFile))
            {
                return
                    CurrentSetting = Settings.Load<Shaman_Restoration.ShamanRestorationSettings>(CurrentSettingsFile);
            }
            else
            {
                return new Shaman_Restoration.ShamanRestorationSettings();
            }
        }
    }

    private readonly ShamanRestorationSettings MySettings = ShamanRestorationSettings.GetSettings();

    #region Professions & Racials

    private readonly Spell Arcane_Torrent = new Spell("Arcane Torrent");
    private readonly Spell Berserking = new Spell("Berserking");
    private readonly Spell Blood_Fury = new Spell("Blood Fury");
    private readonly Spell Lifeblood = new Spell("Lifeblood");
    private readonly Spell Stoneform = new Spell("Stoneform");
    private readonly Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    private readonly Spell War_Stomp = new Spell("War Stomp");
    private readonly Spell Engineering = new Spell("Engineering");
    private readonly Spell Alchemy = new Spell("Alchemy");

    #endregion

    #region Shaman Buffs

    private readonly Spell Earth_Shield = new Spell("Earth Shield");
    private readonly Spell Earthliving_Weapon = new Spell("Earthliving Weapon");
    private readonly Spell Flametongue_Weapon = new Spell("Flametongue Weapon");
    private readonly Spell Frostbrand_Weapon = new Spell("Frostbrand Weapon");
    private readonly Spell Ghost_Wolf = new Spell("Ghost Wolf");
    private readonly Spell Lightning_Shield = new Spell("Lightning Shield");
    private readonly Spell Rockbiter_Weapon = new Spell("Rockbiter Weapon");
    private readonly Spell Spiritwalkers_Grace = new Spell("Spiritwalker's Grace");
    private readonly Spell Water_Shield = new Spell("Water Shield");
    private readonly Spell Water_Walking = new Spell("Water Walking");
    private Timer Water_Walking_Timer = new Timer(0);

    #endregion

    #region Offensive Spell

    private readonly Spell Chain_Lightning = new Spell("Chain Lightning");
    private readonly Spell Earth_Shock = new Spell("Earth Shock");
    private readonly Spell Flame_Shock = new Spell("Flame Shock");
    private Timer Flame_Shock_Timer = new Timer(0);
    private readonly Spell Frost_Shock = new Spell("Frost Shock");
    private readonly Spell Lava_Burst = new Spell("Lava Burst");
    private readonly Spell Lightning_Bolt = new Spell("Lightning Bolt");
    private readonly Spell Magma_Totem = new Spell("Magma Totem");
    private readonly Spell Primal_Strike = new Spell("Primal Strike");
    private readonly Spell Searing_Totem = new Spell("Searing Totem");

    #endregion

    #region Offensive Cooldown

    private readonly Spell Ancestral_Swiftness = new Spell("Ancestral Swiftness");
    private readonly Spell Ascendance = new Spell("Ascendance");
    private readonly Spell Bloodlust = new Spell("Bloodlust");
    private readonly Spell Call_of_the_Elements = new Spell("Call of the Elements");
    private readonly Spell Earth_Elemental_Totem = new Spell("Earth Elemental Totem");
    private readonly Spell Elemental_Blast = new Spell("Elemental Blast");
    private readonly Spell Elemental_Mastery = new Spell("Elemental Mastery");
    private readonly Spell Fire_Elemental_Totem = new Spell("Fire Elemental Totem");
    private readonly Spell Heroism = new Spell("Heroism");
    private readonly Spell Stormlash_Totem = new Spell("Stormlash Totem");
    private readonly Spell Totemic_Projection = new Spell("Totemic Projection");
    private readonly Spell Unleash_Elements = new Spell("Unleash Elements");
    private readonly Spell Unleashed_Fury = new Spell("Unleashed Fury");

    #endregion

    #region Defensive Cooldown

    private readonly Spell Astral_Shift = new Spell("Astral Shift");
    private readonly Spell Capacitor_Totem = new Spell("Capacitor Totem");
    private readonly Spell Earthbind_Totem = new Spell("Earthbind Totem");
    private readonly Spell Grounding_Totem = new Spell("Grounding Totem");
    private readonly Spell Stone_Bulwark_Totem = new Spell("Stone Bulwark Totem");
    private readonly Spell Wind_Shear = new Spell("Wind Shear");

    #endregion

    #region Healing Spell

    private readonly Spell Ancestral_Guidance = new Spell("Ancestral Guidance");
    private readonly Spell Chain_Heal = new Spell("Chain Heal");
    private readonly Spell Greater_Healing_Wave = new Spell("Greater Healing Wave");
    private readonly Spell Healing_Rain = new Spell("Healing Rain");
    private readonly Spell Healing_Surge = new Spell("Healing Surge");
    private readonly Spell Healing_Stream_Totem = new Spell("Healing Stream Totem");
    private readonly Spell Healing_Tide_Totem = new Spell("Healing Tide Totem");
    private readonly Spell Healing_Wave = new Spell("Healing_Wave");
    private readonly Spell Mana_Tide_Totem = new Spell("Mana Tide Totem");
    private readonly Spell Riptide = new Spell("Riptide");
    private readonly Spell Spirit_Link_Totem = new Spell("Spirit Link Totem");
    private readonly Spell Totemic_Recall = new Spell("Totemic Recall");

    #endregion

    private Timer OnCD = new Timer(0);
    private Timer Trinket_Timer = new Timer(0);
    private Timer Engineering_Timer = new Timer(0);
    private Timer AlchFlask_Timer = new Timer(0);
    public int LC = 0;

    public Shaman_Restoration()
    {
        Main.range = 30.0f;
        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            try
            {
                if (!ObjectManager.Me.IsMounted)
                {
                    if (Fight.InFight && ObjectManager.Me.Target > 0)
                    {
                        if (ObjectManager.Me.Target != lastTarget &&
                            (Flame_Shock.IsDistanceGood || Earth_Shock.IsDistanceGood))
                        {
                            Pull();
                            lastTarget = ObjectManager.Me.Target;
                        }

                        if (ObjectManager.Target.Level < 70 && ObjectManager.Me.Level > 84
                            && MySettings.UseLowCombat)
                        {
                            LC = 1;
                            LowCombat();
                        }
                        else
                        {
                            LC = 0;
                            Combat();
                        }
                    }
                    else
                        if (!ObjectManager.Me.IsCast)
                            Patrolling();
                }
            }
            catch
            {
            }
            Thread.Sleep(150);
        }
    }

    public void Pull()
    {
        if (Totemic_Projection.KnownSpell && Totemic_Projection.IsSpellUsable && MySettings.UseTotemicProjection)
            Totemic_Projection.Launch();

        if (Flame_Shock.KnownSpell && Flame_Shock.IsSpellUsable && Flame_Shock.IsDistanceGood
            && MySettings.UseFlameShock && LC != 1)
        {
            Flame_Shock.Launch();
            return;
        }
        else
        {
            if (Earth_Shock.KnownSpell && Earth_Shock.IsSpellUsable && Earth_Shock.IsDistanceGood
                && MySettings.UseEarthShock)
            {
                Earth_Shock.Launch();
                return;
            }
        }
    }

    public void Combat()
    {
        AvoidMelee();
        if (OnCD.IsReady)
            Defense_Cycle();
        Heal();
        Decast();
        Buff();
        DPS_Burst();
        DPS_Cycle();
    }

    public void LowCombat()
    {
        AvoidMelee();
        Heal();
        Defense_Cycle();
        Buff();

        if (Earth_Shock.KnownSpell && Earth_Shock.IsSpellUsable && Earth_Shock.IsDistanceGood
            && MySettings.UseEarthShock)
        {
            Earth_Shock.Launch();
            return;
        }
        else if (Lava_Burst.KnownSpell && Lava_Burst.IsSpellUsable && Lava_Burst.IsDistanceGood
            && MySettings.UseLavaBurst)
        {
            Lava_Burst.Launch();
            return;
        }
        else if (Chain_Lightning.KnownSpell && Chain_Lightning.IsSpellUsable && Chain_Lightning.IsDistanceGood
            && MySettings.UseChainLightning)
        {
            Chain_Lightning.Launch();
            return;
        }
        else
        {
            if (Searing_Totem.KnownSpell && Searing_Totem.IsSpellUsable && FireTotemReady()
                && MySettings.UseSearingTotem)
            {
                if (!Searing_Totem.SummonedByMeBySpellName(5))
                    Searing_Totem.Launch();
                return;
            }
        }

        if (Magma_Totem.KnownSpell && Magma_Totem.IsSpellUsable && ObjectManager.Target.GetDistance < 8
            && MySettings.UseMagmaTotem && FireTotemReady())
        {
            Magma_Totem.Launch();
            return;
        }
    }

    public void DPS_Burst()
    {
        if (MySettings.UseTrinket && Trinket_Timer.IsReady && ObjectManager.Target.GetDistance < 30)
        {
            Logging.WriteFight("Use Trinket 1.");
            Lua.RunMacroText("/use 13");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Logging.WriteFight("Use Trinket 2.");
            Lua.RunMacroText("/use 14");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Trinket_Timer = new Timer(1000*60*2);
            return;
        }
        else if (Berserking.IsSpellUsable && Berserking.KnownSpell && MySettings.UseBerserking
            && ObjectManager.Target.GetDistance < 30)
        {
            Berserking.Launch();
            return;
        }
        else if (Blood_Fury.IsSpellUsable && Blood_Fury.KnownSpell && MySettings.UseBloodFury
            && ObjectManager.Target.GetDistance < 30)
        {
            Blood_Fury.Launch();
            return;
        }
        else if (Lifeblood.IsSpellUsable && Lifeblood.KnownSpell && MySettings.UseLifeblood
            && ObjectManager.Target.GetDistance < 30)
        {
            Lifeblood.Launch();
            return;
        }
        else if (MySettings.UseEngGlove && Engineering.KnownSpell && Engineering_Timer.IsReady
            && ObjectManager.Target.GetDistance < 30)
        {
            Logging.WriteFight("Use Engineering Gloves.");
            Lua.RunMacroText("/use 10");
            Engineering_Timer = new Timer(1000*60);
            return;
        }
        else if (Unleash_Elements.KnownSpell && Unleash_Elements.IsSpellUsable && Unleashed_Fury.KnownSpell
            && MySettings.UseUnleashElements && Unleash_Elements.IsDistanceGood)
        {
            Unleash_Elements.Launch();
            return;
        }
        else if (Elemental_Blast.KnownSpell && Elemental_Blast.IsSpellUsable
            && MySettings.UseElementalBlast && Elemental_Blast.IsDistanceGood)
        {
            Elemental_Blast.Launch();
            return;
        }
        else if (Ascendance.KnownSpell && Ascendance.IsSpellUsable && ObjectManager.Me.HealthPercent < 80
            && MySettings.UseAscendance && ObjectManager.Target.GetDistance < 40)
        {
            Ascendance.Launch();
            return;
        }
        else if (Fire_Elemental_Totem.KnownSpell && Fire_Elemental_Totem.IsSpellUsable
            && MySettings.UseFireElementalTotem && ObjectManager.Target.GetDistance < 40)
        {
            Fire_Elemental_Totem.Launch();
            return;
        }
        else if (Stormlash_Totem.KnownSpell && AirTotemReady()
            && MySettings.UseStormlashTotem && ObjectManager.Target.GetDistance < 40)
        {
            if (!Stormlash_Totem.IsSpellUsable && MySettings.UseCalloftheElements
                && Call_of_the_Elements.KnownSpell && Call_of_the_Elements.IsSpellUsable)
            {
                Call_of_the_Elements.Launch();
                Thread.Sleep(200);
            }

            if (Stormlash_Totem.IsSpellUsable)
                Stormlash_Totem.Launch();
            return;
        }
        else if (Bloodlust.KnownSpell && Bloodlust.IsSpellUsable && MySettings.UseBloodlustHeroism 
            && ObjectManager.Target.GetDistance < 40 && !ObjectManager.Me.HaveBuff(57724))
        {
            Bloodlust.Launch();
            return;
        }

        else if (Heroism.KnownSpell && Heroism.IsSpellUsable && MySettings.UseBloodlustHeroism 
            && ObjectManager.Target.GetDistance < 40 && !ObjectManager.Me.HaveBuff(57723))
        {
            Heroism.Launch();
            return;
        }
        else
        {
            if (Elemental_Mastery.KnownSpell && Elemental_Mastery.IsSpellUsable
                && !ObjectManager.Me.HaveBuff(2825) && MySettings.UseElementalMastery
                && !ObjectManager.Me.HaveBuff(32182))
            {
                Elemental_Mastery.Launch();
                return;
            }
        }
    }

    public void DPS_Cycle()
    {
        if (Primal_Strike.KnownSpell && Primal_Strike.IsSpellUsable && Primal_Strike.IsDistanceGood 
            && MySettings.UsePrimalStrike && ObjectManager.Me.Level < 11)
        {
            Primal_Strike.Launch();
            return;
        }

        if (ObjectManager.Me.ManaPercentage < 70 && Arcane_Torrent.KnownSpell && Arcane_Torrent.IsSpellUsable
            && MySettings.UseArcaneTorrent)
        {
            Arcane_Torrent.Launch();
            return;
        }
        else if (Earth_Elemental_Totem.KnownSpell && Earth_Elemental_Totem.IsSpellUsable
            && ObjectManager.GetNumberAttackPlayer() > 3 && MySettings.UseEarthElementalTotem)
        {
            Earth_Elemental_Totem.Launch();
            return;
        }
        else if (Flame_Shock.IsSpellUsable && Flame_Shock.IsDistanceGood && Flame_Shock.KnownSpell
            && MySettings.UseFlameShock && (!Flame_Shock.TargetHaveBuff || Flame_Shock_Timer.IsReady))
        {
            Flame_Shock.Launch();
            Flame_Shock_Timer = new Timer(1000 * 27);
            return;
        }
        else if (Lava_Burst.KnownSpell && Lava_Burst.IsSpellUsable && Lava_Burst.IsDistanceGood 
            && MySettings.UseLavaBurst && Flame_Shock.TargetHaveBuff)
        {
            Lava_Burst.Launch();
            return;
        }
        else if (Earth_Shock.IsSpellUsable && Earth_Shock.KnownSpell && Earth_Shock.IsDistanceGood
            && MySettings.UseEarthShock && Flame_Shock.TargetHaveBuff)
        {
            Earth_Shock.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 1 && Magma_Totem.KnownSpell
            && Magma_Totem.IsSpellUsable && MySettings.UseMagmaTotem
            && !Fire_Elemental_Totem.SummonedByMeBySpellName())
        {
            Magma_Totem.Launch();
            return;
        }
        else if (Searing_Totem.KnownSpell && Searing_Totem.IsSpellUsable && MySettings.UseSearingTotem
            && FireTotemReady() && !Searing_Totem.SummonedByMeBySpellName(25))
        {
            Searing_Totem.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 1 && Chain_Lightning.KnownSpell 
            && Chain_Lightning.IsSpellUsable && Chain_Lightning.IsDistanceGood && MySettings.UseChainLightning)
        {
            if (Ancestral_Swiftness.KnownSpell && Ancestral_Swiftness.IsSpellUsable 
                && MySettings.UseAncestralSwiftness)
            {
                Ancestral_Swiftness.Launch();
                Thread.Sleep(200);
            }
            Chain_Lightning.Launch();
            return;
        }
        else
        {
            if (Lightning_Bolt.IsDistanceGood && Lightning_Bolt.KnownSpell && Lightning_Bolt.IsSpellUsable
                && MySettings.UseLightningBolt)
            {
                if (Ancestral_Swiftness.KnownSpell && Ancestral_Swiftness.IsSpellUsable
                    && MySettings.UseAncestralSwiftness)
                {
                    Ancestral_Swiftness.Launch();
                    Thread.Sleep(200);
                }
                Lightning_Bolt.Launch();
                return;
            }
        }
    }

    public void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            Heal();
            Buff();
        }
    }

    private void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (Water_Walking.IsSpellUsable && Water_Walking.KnownSpell && (!Water_Walking.HaveBuff || Water_Walking_Timer.IsReady)
            && ObjectManager.GetNumberAttackPlayer() == 0 && !Fight.InFight && MySettings.UseWaterWalking)
        {
            Water_Walking.Launch();
            Water_Walking_Timer = new Timer(1000*60*9);
            return;
        }
        else if ((ObjectManager.Me.ManaPercentage < 5 && Water_Shield.KnownSpell && Water_Shield.IsSpellUsable
            && MySettings.UseWaterShield && !Water_Shield.HaveBuff) 
            || (!MySettings.UseLightningShield && !MySettings.UseEarthShield))
        {
            Water_Shield.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 50 && Earth_Shield.KnownSpell && Earth_Shield.IsSpellUsable
            && MySettings.UseEarthShield && !Earth_Shield.HaveBuff && ObjectManager.Me.ManaPercentage > 15
            || !MySettings.UseLightningShield)
        {
            Earth_Shield.Launch();
            return;
        }
        else if (Lightning_Shield.KnownSpell && Lightning_Shield.IsSpellUsable && !Lightning_Shield.HaveBuff
            && MySettings.UseLightningShield && ObjectManager.Me.ManaPercentage > 15 
            && ObjectManager.Me.HealthPercent > 70)
        {
            Lightning_Shield.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 0 && Spiritwalkers_Grace.IsSpellUsable 
            && Spiritwalkers_Grace.KnownSpell && MySettings.UseSpiritwalkersGrace && ObjectManager.Me.GetMove)
        {
            Spiritwalkers_Grace.Launch();
            return;
        }
        else
        {
            if (Flametongue_Weapon.KnownSpell && Flametongue_Weapon.IsSpellUsable && !ObjectManager.Me.HaveBuff(10400)
                && MySettings.UseFlametongueWeapon)
            {
                Flametongue_Weapon.Launch();
                return;
            }
            else if (Earthliving_Weapon.KnownSpell && Earthliving_Weapon.IsSpellUsable && !ObjectManager.Me.HaveBuff(52007)
                && MySettings.UseEarthlivingWeapon && !MySettings.UseFlametongueWeapon)
            {
                Earthliving_Weapon.Launch();
                return;
            }
            else if (Frostbrand_Weapon.KnownSpell && Frostbrand_Weapon.IsSpellUsable && !ObjectManager.Me.HaveBuff(8034)
                && MySettings.UseFrostbrandWeapon && !MySettings.UseFlametongueWeapon && !MySettings.UseEarthlivingWeapon)
            {
                Frostbrand_Weapon.Launch();
                return;
            }
            else
            {
                if (Rockbiter_Weapon.KnownSpell && Rockbiter_Weapon.IsSpellUsable && !ObjectManager.Me.HaveBuff(36494)
                    && MySettings.UseRockbiterWeapon && !MySettings.UseFlametongueWeapon 
                    && !MySettings.UseFrostbrandWeapon && !MySettings.UseEarthlivingWeapon)
                {
                    Rockbiter_Weapon.Launch();
                    return;
                }
            }
        }

        if (ObjectManager.GetNumberAttackPlayer() == 0 && Ghost_Wolf.IsSpellUsable && Ghost_Wolf.KnownSpell
            && MySettings.UseGhostWolf && ObjectManager.Me.GetMove)
        {
            Ghost_Wolf.Launch();
            return;
        }
    }

    public bool FireTotemReady()
    {
        if (Fire_Elemental_Totem.SummonedByMeBySpellName() || Magma_Totem.SummonedByMeBySpellName())
            return false;
        return true;
    }

    public bool EarthTotemReady()
    {
        if (Earthbind_Totem.SummonedByMeBySpellName() || Earth_Elemental_Totem.SummonedByMeBySpellName()
            || Stone_Bulwark_Totem.SummonedByMeBySpellName())
            return false;
        return true;
    }

    public bool WaterTotemReady()
    {
        if (Healing_Stream_Totem.SummonedByMeBySpellName() || Healing_Tide_Totem.SummonedByMeBySpellName()
            || Mana_Tide_Totem.SummonedByMeBySpellName())
            return false;
        return true;
    }

    public bool AirTotemReady()
    {
        if (Capacitor_Totem.SummonedByMeBySpellName() || Grounding_Totem.SummonedByMeBySpellName() 
            || Stormlash_Totem.SummonedByMeBySpellName() || Spirit_Link_Totem.SummonedByMeBySpellName())
            return false;
        return true;
    }

    public bool TotemicRecallReady()
    {
        if (Fire_Elemental_Totem.SummonedByMeBySpellName())
            return false;
        else if (Earth_Elemental_Totem.SummonedByMeBySpellName())
            return false;
        else if (Searing_Totem.SummonedByMeBySpellName())
            return true;
        else if (FireTotemReady() && EarthTotemReady() && WaterTotemReady() && AirTotemReady())
            return false;
        else
            return true;
    }

    private void Heal()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (ObjectManager.Me.ManaPercentage < 50 && Totemic_Recall.KnownSpell && Totemic_Recall.IsSpellUsable
            && MySettings.UseTotemicRecall && ObjectManager.GetNumberAttackPlayer() == 0 && !Fight.InFight
            && TotemicRecallReady())
        {
            Totemic_Recall.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Me.ManaPercentage < 80 && Mana_Tide_Totem.KnownSpell && Mana_Tide_Totem.IsSpellUsable
                && MySettings.UseManaTideTotem && WaterTotemReady())
            {
                Mana_Tide_Totem.Launch();
                return;
            }
        }

        if (ObjectManager.Me.HealthPercent < 95 && Healing_Surge.KnownSpell && Healing_Surge.IsSpellUsable
            && ObjectManager.GetNumberAttackPlayer() == 0 && !Fight.InFight && MySettings.UseHealingSurge)
        {
            Healing_Surge.Launch();
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(200);
            }
            return;
        }
        else if (Healing_Surge.KnownSpell && Healing_Surge.IsSpellUsable && ObjectManager.Me.HealthPercent < 50
            && MySettings.UseHealingSurge)
        {
            Healing_Surge.Launch();
            return;
        }
        else if (Greater_Healing_Wave.KnownSpell && Greater_Healing_Wave.IsSpellUsable 
            && ObjectManager.Me.HealthPercent < 60 && MySettings.UseGreaterHealingWave)
        {
            Greater_Healing_Wave.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && Gift_of_the_Naaru.KnownSpell && Gift_of_the_Naaru.IsSpellUsable
            && MySettings.UseGiftoftheNaaru)
        {
            Gift_of_the_Naaru.Launch();
            return;
        }
        else if (Healing_Tide_Totem.KnownSpell && Healing_Tide_Totem.IsSpellUsable && ObjectManager.Me.HealthPercent < 70
            && WaterTotemReady() && MySettings.UseHealingTideTotem)
        {
            Healing_Tide_Totem.Launch();
            return;
        }
        else if (Ancestral_Guidance.KnownSpell && Ancestral_Guidance.IsSpellUsable && ObjectManager.Me.HealthPercent < 70
            && MySettings.UseAncestralGuidance)
        {
            Ancestral_Guidance.Launch();
            return;
        }
        else if (Chain_Heal.KnownSpell && Chain_Heal.IsSpellUsable && ObjectManager.Me.HealthPercent < 80
            && MySettings.UseChainHeal)
        {
            Chain_Heal.Launch();
            return;
        }
        else if (Healing_Stream_Totem.KnownSpell && Healing_Stream_Totem.IsSpellUsable && ObjectManager.Me.HealthPercent < 90
            && WaterTotemReady() && MySettings.UseHealingStreamTotem)
        {
            Healing_Stream_Totem.Launch();
            return;
        }
        else if (Riptide.KnownSpell && Riptide.IsSpellUsable && ObjectManager.Me.HealthPercent < 90
            && MySettings.UseRiptide && !Riptide.HaveBuff)
        {
            Riptide.Launch();
            return;
        }
        else
        {
            if (Healing_Wave.KnownSpell && Healing_Wave.IsSpellUsable && ObjectManager.Me.HealthPercent < 80
                && MySettings.UseHealingWave)
            {
                Healing_Wave.Launch();
                return;
            }
        }
    }

    private void Defense_Cycle()
    {
        if (ObjectManager.Me.HealthPercent < 50 && Capacitor_Totem.KnownSpell && Capacitor_Totem.IsSpellUsable
            && AirTotemReady() && MySettings.UseCapacitorTotem)
        {
            Capacitor_Totem.Launch();
            OnCD = new Timer(1000*5);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 50 && Stone_Bulwark_Totem.KnownSpell && Stone_Bulwark_Totem.IsSpellUsable
            && EarthTotemReady() && MySettings.UseStoneBulwarkTotem)
        {
            Stone_Bulwark_Totem.Launch();
            OnCD = new Timer(1000*10);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 70 && Spirit_Link_Totem.KnownSpell && Spirit_Link_Totem.IsSpellUsable
            && AirTotemReady() && MySettings.UseSpiritLinkTotem)
        {
            Spirit_Link_Totem.Launch();
            OnCD = new Timer(1000*6);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && War_Stomp.IsSpellUsable && War_Stomp.KnownSpell
            && MySettings.UseWarStomp)
        {
            War_Stomp.Launch();
            OnCD = new Timer(1000*2);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && Stoneform.IsSpellUsable && Stoneform.KnownSpell
            && MySettings.UseStoneform)
        {
            Stoneform.Launch();
            OnCD = new Timer(1000*8);
            return;
        }
        else
        {
            if (ObjectManager.Me.HealthPercent < 70 && Astral_Shift.KnownSpell && Astral_Shift.IsSpellUsable
                && MySettings.UseAstralShift)
            {
                Astral_Shift.Launch();
                OnCD = new Timer(1000*6);
                return;
            }
        }
    }

    private void Decast()
    {
        if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && MySettings.UseWindShear
            && Wind_Shear.KnownSpell && Wind_Shear.IsSpellUsable && Wind_Shear.IsDistanceGood)
        {
            Wind_Shear.Launch();
            return;
        }
        else if (ObjectManager.Target.IsCast && Arcane_Torrent.KnownSpell && Arcane_Torrent.IsSpellUsable
            && MySettings.UseArcaneTorrent && ObjectManager.Target.IsTargetingMe && ObjectManager.Target.GetDistance < 8)
        {
            Arcane_Torrent.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && MySettings.UseGroundingTotem
                && Grounding_Totem.KnownSpell && Grounding_Totem.IsSpellUsable && AirTotemReady())
            {
                Grounding_Totem.Launch();
                return;
            }
        }

        if (ObjectManager.Target.GetMove && !Frost_Shock.TargetHaveBuff && MySettings.UseFrostShock
            && Frost_Shock.KnownSpell && Frost_Shock.IsSpellUsable && Frost_Shock.IsDistanceGood)
        {
            Frost_Shock.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Target.GetMove && MySettings.UseEarthbindTotem && EarthTotemReady()
                && Earthbind_Totem.KnownSpell && Earthbind_Totem.IsSpellUsable && Earthbind_Totem.IsDistanceGood)
            {
                Earthbind_Totem.Launch();
                return;
            }
        }
    }

    private void AvoidMelee()
    {
        if (ObjectManager.Target.GetDistance < 3 && ObjectManager.Target.InCombat)
        {
            nManager.Wow.Helpers.Keybindings.PressKeybindings(nManager.Wow.Enums.Keybindings.MOVEBACKWARD);
        }
    }
}

public class Shaman_Elemental
{
    [Serializable]
    public class ShamanElementalSettings : nManager.Helpful.Settings
    {
        /* Professions & Racials */
        public bool UseArcaneTorrent = true;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseLifeblood = true;
        public bool UseStoneform = true;
        public bool UseGiftoftheNaaru = true;
        public bool UseWarStomp = true;
        /* Shaman Buffs */
        public bool UseFlametongueWeapon = true;
        public bool UseFrostbrandWeapon = false;
        public bool UseGhostWolf = true;
        public bool UseLightningShield = true;
        public bool UseRockbiterWeapon = false;
        public bool UseSpiritwalkersGrace = true;
        public bool UseWaterShield = true;
        public bool UseWaterWalking = true;
        /* Offensive Spell */
        public bool UseChainLightning = true;
        public bool UseEarthquake = true;
        public bool UseEarthShock  = true;
        public bool UseFlameShock = true;
        public bool UseFrostShock = false;
        public bool UseLavaBurst = true;
        public bool UseLightningBolt = true;
        public bool UseMagmaTotem = true;
        public bool UseSearingTotem = true;
        public bool UseThunderstorm = true;
        /* Offensive Cooldown */
        public bool UseAncestralSwiftness = true;
        public bool UseAscendance = true;
        public bool UseBloodlustHeroism = true;
        public bool UseCalloftheElements = true;
        public bool UseEarthElementalTotem = true;
        public bool UseElementalBlast = true;
        public bool UseElementalMastery = true;
        public bool UseFireElementalTotem = true;
        public bool UseStormlashTotem = true;
        public bool UseTotemicProjection = true;
        public bool UseUnleashElements = true;
        /* Defensive Cooldown */
        public bool UseAstralShift = true;
        public bool UseCapacitorTotem = true;
        public bool UseEarthbindTotem = false;
        public bool UseGroundingTotem = true;
        public bool UseStoneBulwarkTotem = true;
        public bool UseWindShear = true;
        /* Healing Spell */
        public bool UseAncestralGuidance = true;
        public bool UseChainHeal = false;
        public bool UseHealingRain = true;
        public bool UseHealingSurge = true;
        public bool UseHealingStreamTotem = true;
        public bool UseHealingTideTotem = true;
        public bool UseTotemicRecall = true;
        /* Game Settings */
        public bool UseLowCombat = true;
        public bool UseTrinket = true;
        public bool UseEngGlove = true;
        public bool UseAlchFlask = true;

        public ShamanElementalSettings()
        {
            ConfigWinForm(new System.Drawing.Point(400, 400), "Shaman Elemental Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");
            AddControlInWinForm("Use Lifeblood", "UseLifeblood", "Professions & Racials");
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
            AddControlInWinForm("Use Healing Stream Totem", "UseHealingStream_Totem", "Healing Spell");
            AddControlInWinForm("Use Healing Tide Totem", "UsHealingTide_Totem", "Healing Spell");
            AddControlInWinForm("Use Totemic Recall", "UseTotemicRecall", "Healing Spell");
            /* Game Settings */
            AddControlInWinForm("Use Low Combat Settings", "UseLowCombat", "Game Settings");
            AddControlInWinForm("Use Trinket", "UseTrinket", "Game Settings");
            AddControlInWinForm("Use Engineering Gloves", "UseEngGlove", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
        }

        public static ShamanElementalSettings CurrentSetting { get; set; }

        public static ShamanElementalSettings GetSettings()
        {
            string CurrentSettingsFile = Application.StartupPath + "\\CustomClasses\\Settings\\Shaman_Elemental.xml";
            if (System.IO.File.Exists(CurrentSettingsFile))
            {
                return
                    CurrentSetting = Settings.Load<Shaman_Elemental.ShamanElementalSettings>(CurrentSettingsFile);
            }
            else
            {
                return new Shaman_Elemental.ShamanElementalSettings();
            }
        }
    }

    private readonly ShamanElementalSettings MySettings = ShamanElementalSettings.GetSettings();

    #region Professions & Racials

    private readonly Spell Arcane_Torrent = new Spell("Arcane Torrent");
    private readonly Spell Berserking = new Spell("Berserking");
    private readonly Spell Blood_Fury = new Spell("Blood Fury");
    private readonly Spell Lifeblood = new Spell("Lifeblood");
    private readonly Spell Stoneform = new Spell("Stoneform");
    private readonly Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    private readonly Spell War_Stomp = new Spell("War Stomp");
    private readonly Spell Engineering = new Spell("Engineering");
    private readonly Spell Alchemy = new Spell("Alchemy");

    #endregion

    #region Shaman Buffs

    private readonly Spell Flametongue_Weapon = new Spell("Flametongue Weapon");
    private readonly Spell Frostbrand_Weapon = new Spell("Frostbrand Weapon");
    private readonly Spell Ghost_Wolf = new Spell("Ghost Wolf");
    private readonly Spell Lightning_Shield = new Spell("Lightning Shield");
    private readonly Spell Rockbiter_Weapon = new Spell("Rockbiter Weapon");
    private readonly Spell Spiritwalkers_Grace = new Spell("Spiritwalker's Grace");
    private readonly Spell Water_Shield = new Spell("Water Shield");
    private readonly Spell Water_Walking = new Spell("Water Walking");
    private Timer Water_Walking_Timer = new Timer(0);

    #endregion

    #region Offensive Spell

    private readonly Spell Chain_Lightning = new Spell("Chain Lightning");
    private readonly Spell Earthquake = new Spell("Earthquake");
    private readonly Spell Earth_Shock = new Spell("Earth Shock");
    private readonly Spell Flame_Shock = new Spell("Flame Shock");
    private Timer Flame_Shock_Timer = new Timer(0);
    private readonly Spell Frost_Shock = new Spell("Frost Shock");
    private readonly Spell Lava_Burst = new Spell("Lava Burst");
    private readonly Spell Lightning_Bolt = new Spell("Lightning Bolt");
    private readonly Spell Magma_Totem = new Spell("Magma Totem");
    private readonly Spell Searing_Totem = new Spell("Searing Totem");
    private readonly Spell Thunderstorm = new Spell("Thunderstorm");

    #endregion

    #region Offensive Cooldown

    private readonly Spell Ancestral_Swiftness = new Spell("Ancestral Swiftness");
    private readonly Spell Ascendance = new Spell("Ascendance");
    private readonly Spell Bloodlust = new Spell("Bloodlust");
    private readonly Spell Call_of_the_Elements = new Spell("Call of the Elements");
    private readonly Spell Earth_Elemental_Totem = new Spell("Earth Elemental Totem");
    private readonly Spell Elemental_Blast = new Spell("Elemental Blast");
    private readonly Spell Elemental_Mastery = new Spell("Elemental Mastery");
    private readonly Spell Fire_Elemental_Totem = new Spell("Fire Elemental Totem");
    private readonly Spell Heroism = new Spell("Heroism");
    private readonly Spell Stormlash_Totem = new Spell("Stormlash Totem");
    private readonly Spell Totemic_Projection = new Spell("Totemic Projection");
    private readonly Spell Unleash_Elements = new Spell("Unleash Elements");
    private readonly Spell Unleashed_Fury = new Spell("Unleashed Fury");

    #endregion

    #region Defensive Cooldown

    private readonly Spell Astral_Shift = new Spell("Astral Shift");
    private readonly Spell Capacitor_Totem = new Spell("Capacitor Totem");
    private readonly Spell Earthbind_Totem = new Spell("Earthbind Totem");
    private readonly Spell Grounding_Totem = new Spell("Grounding Totem");
    private readonly Spell Stone_Bulwark_Totem = new Spell("Stone Bulwark Totem");
    private readonly Spell Wind_Shear = new Spell("Wind Shear");

    #endregion

    #region Healing Spell

    private readonly Spell Ancestral_Guidance = new Spell("Ancestral Guidance");
    private readonly Spell Chain_Heal = new Spell("Chain Heal");
    private readonly Spell Healing_Rain = new Spell("Healing Rain");
    private readonly Spell Healing_Surge = new Spell("Healing Surge");
    private readonly Spell Healing_Stream_Totem = new Spell("Healing Stream Totem");
    private readonly Spell Healing_Tide_Totem = new Spell("Healing Tide Totem");
    private readonly Spell Totemic_Recall = new Spell("Totemic Recall");

    #endregion

    private Timer OnCD = new Timer(0);
    private Timer Trinket_Timer = new Timer(0);
    private Timer Engineering_Timer = new Timer(0);
    private Timer AlchFlask_Timer = new Timer(0);
    public int LC = 0;

    public Shaman_Elemental()
    {
        Main.range = 30.0f;
        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            try
            {
                if (!ObjectManager.Me.IsMounted)
                {
                    if (Fight.InFight && ObjectManager.Me.Target > 0)
                    {
                        if (ObjectManager.Me.Target != lastTarget && Flame_Shock.IsDistanceGood)
                        {
                            Pull();
                            lastTarget = ObjectManager.Me.Target;
                        }

                        if (ObjectManager.Target.Level < 70 && ObjectManager.Me.Level > 84
                            && MySettings.UseLowCombat)
                        {
                            LC = 1;
                            LowCombat();
                        }
                        else
                        {
                            LC = 0;
                            Combat();
                        }
                    }
                    else
                        if (!ObjectManager.Me.IsCast)
                            Patrolling();
                }
            }
            catch
            {
            }
            Thread.Sleep(150);
        }
    }

    public void Pull()
    {
        if (Totemic_Projection.KnownSpell && Totemic_Projection.IsSpellUsable && MySettings.UseTotemicProjection)
            Totemic_Projection.Launch();

        if (Flame_Shock.KnownSpell && Flame_Shock.IsSpellUsable && Flame_Shock.IsDistanceGood
            && MySettings.UseFlameShock && LC != 1)
        {
            Flame_Shock.Launch();
            return;
        }
        else
        {
            if (Earth_Shock.KnownSpell && Earth_Shock.IsSpellUsable && Earth_Shock.IsDistanceGood
                && MySettings.UseEarthShock)
            {
                Earth_Shock.Launch();
                return;
            }
        }
    }

    public void Combat()
    {
        AvoidMelee();
        if (OnCD.IsReady)
            Defense_Cycle();
        Heal();
        Decast();
        Buff();
        DPS_Burst();
        DPS_Cycle();
    }

    public void LowCombat()
    {
        AvoidMelee();
        Heal();
        Defense_Cycle();
        Buff();

        if (Earth_Shock.KnownSpell && Earth_Shock.IsSpellUsable && Earth_Shock.IsDistanceGood
            && MySettings.UseEarthShock)
        {
            Earth_Shock.Launch();
            return;
        }
        else if (Lava_Burst.KnownSpell && Lava_Burst.IsSpellUsable && Lava_Burst.IsDistanceGood
            && MySettings.UseLavaBurst)
        {
            Lava_Burst.Launch();
            return;
        }
        else if (Chain_Lightning.KnownSpell && Chain_Lightning.IsSpellUsable && Chain_Lightning.IsDistanceGood
            && MySettings.UseChainLightning)
        {
            Chain_Lightning.Launch();
            return;
        }
        else
        {
            if (Searing_Totem.KnownSpell && Searing_Totem.IsSpellUsable && FireTotemReady()
                && MySettings.UseSearingTotem)
            {
                if (!Searing_Totem.SummonedByMeBySpellName(5))
                    Searing_Totem.Launch();
                return;
            }
        }

        if (Magma_Totem.KnownSpell && Magma_Totem.IsSpellUsable && ObjectManager.Target.GetDistance < 8
            && MySettings.UseMagmaTotem && FireTotemReady())
        {
            Magma_Totem.Launch();
            return;
        }
    }

    public void DPS_Burst()
    {
        if (MySettings.UseTrinket && Trinket_Timer.IsReady && ObjectManager.Target.GetDistance < 30)
        {
            Logging.WriteFight("Use Trinket 1.");
            Lua.RunMacroText("/use 13");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Logging.WriteFight("Use Trinket 2.");
            Lua.RunMacroText("/use 14");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Trinket_Timer = new Timer(1000*60*2);
            return;
        }
        else if (Berserking.IsSpellUsable && Berserking.KnownSpell && MySettings.UseBerserking
            && ObjectManager.Target.GetDistance < 30)
        {
            Berserking.Launch();
            return;
        }
        else if (Blood_Fury.IsSpellUsable && Blood_Fury.KnownSpell && MySettings.UseBloodFury
            && ObjectManager.Target.GetDistance < 30)
        {
            Blood_Fury.Launch();
            return;
        }
        else if (Lifeblood.IsSpellUsable && Lifeblood.KnownSpell && MySettings.UseLifeblood
            && ObjectManager.Target.GetDistance < 30)
        {
            Lifeblood.Launch();
            return;
        }
        else if (MySettings.UseEngGlove && Engineering.KnownSpell && Engineering_Timer.IsReady
            && ObjectManager.Target.GetDistance < 30)
        {
            Logging.WriteFight("Use Engineering Gloves.");
            Lua.RunMacroText("/use 10");
            Engineering_Timer = new Timer(1000*60);
            return;
        }
        else if (Unleash_Elements.KnownSpell && Unleash_Elements.IsSpellUsable && Unleashed_Fury.KnownSpell
            && MySettings.UseUnleashElements && Unleash_Elements.IsDistanceGood)
        {
            Unleash_Elements.Launch();
            return;
        }
        else if (Elemental_Blast.KnownSpell && Elemental_Blast.IsSpellUsable
            && MySettings.UseElementalBlast && Elemental_Blast.IsDistanceGood)
        {
            Elemental_Blast.Launch();
            return;
        }
        else if (Ascendance.KnownSpell && Ascendance.IsSpellUsable
            && MySettings.UseAscendance && ObjectManager.Target.GetDistance < 40)
        {
            Ascendance.Launch();
            return;
        }
        else if (Fire_Elemental_Totem.KnownSpell && Fire_Elemental_Totem.IsSpellUsable
            && MySettings.UseFireElementalTotem && ObjectManager.Target.GetDistance < 40)
        {
            Fire_Elemental_Totem.Launch();
            return;
        }
        else if (Stormlash_Totem.KnownSpell && AirTotemReady()
            && MySettings.UseStormlashTotem && ObjectManager.Target.GetDistance < 40)
        {
            if (!Stormlash_Totem.IsSpellUsable && MySettings.UseCalloftheElements
                && Call_of_the_Elements.KnownSpell && Call_of_the_Elements.IsSpellUsable)
            {
                Call_of_the_Elements.Launch();
                Thread.Sleep(200);
            }

            if (Stormlash_Totem.IsSpellUsable)
                Stormlash_Totem.Launch();
            return;
        }
        else if (Bloodlust.KnownSpell && Bloodlust.IsSpellUsable && MySettings.UseBloodlustHeroism 
            && ObjectManager.Target.GetDistance < 40 && !ObjectManager.Me.HaveBuff(57724))
            {
                Bloodlust.Launch();
                return;
            }

        else if (Heroism.KnownSpell && Heroism.IsSpellUsable && MySettings.UseBloodlustHeroism 
            && ObjectManager.Target.GetDistance < 40 && !ObjectManager.Me.HaveBuff(57723))
        {
            Heroism.Launch();
            return;
        }
        else
        {
            if (Elemental_Mastery.KnownSpell && Elemental_Mastery.IsSpellUsable
                && !ObjectManager.Me.HaveBuff(2825) && MySettings.UseElementalMastery
                && !ObjectManager.Me.HaveBuff(32182))
            {
                Elemental_Mastery.Launch();
                return;
            }
        }
    }

    public void DPS_Cycle()
    {
        if (ObjectManager.Me.ManaPercentage < 70 && Arcane_Torrent.KnownSpell && Arcane_Torrent.IsSpellUsable
            && MySettings.UseArcaneTorrent)
        {
            Arcane_Torrent.Launch();
            return;
        }
        else if (ObjectManager.Me.ManaPercentage < 80 && Thunderstorm.KnownSpell && Thunderstorm.IsSpellUsable
            && MySettings.UseThunderstorm)
        {
            Thunderstorm.Launch();
            return;
        }
        else if (Earth_Elemental_Totem.KnownSpell && Earth_Elemental_Totem.IsSpellUsable
            && ObjectManager.GetNumberAttackPlayer() > 3 && MySettings.UseEarthElementalTotem)
        {
            Earth_Elemental_Totem.Launch();
            return;
        }
        else if (Thunderstorm.KnownSpell && Thunderstorm.IsSpellUsable && ObjectManager.Target.GetDistance < 10
            && ObjectManager.GetNumberAttackPlayer() > 5 && MySettings.UseThunderstorm)
        {
            Thunderstorm.Launch();
            return;
        }
        else if (Earthquake.KnownSpell && Earthquake.IsSpellUsable && Earthquake.IsDistanceGood
            && ObjectManager.GetNumberAttackPlayer() > 5 && MySettings.UseEarthquake)
        {
            SpellManager.CastSpellByIDAndPosition(61882, ObjectManager.Target.Position);
            return;
        }
        else if (Flame_Shock.IsSpellUsable && Flame_Shock.IsDistanceGood && Flame_Shock.KnownSpell
            && MySettings.UseFlameShock && (!Flame_Shock.TargetHaveBuff || Flame_Shock_Timer.IsReady))
        {
            Flame_Shock.Launch();
            Flame_Shock_Timer = new Timer(1000 * 27);
            return;
        }
        else if (Lava_Burst.KnownSpell && Lava_Burst.IsSpellUsable && Lava_Burst.IsDistanceGood 
            && MySettings.UseLavaBurst && Flame_Shock.TargetHaveBuff)
        {
            Lava_Burst.Launch();
            return;
        }
        else if (Earth_Shock.IsSpellUsable && Earth_Shock.KnownSpell && Earth_Shock.IsDistanceGood
            && MySettings.UseEarthShock && Lightning_Shield.BuffStack > 4)
        {
            Earth_Shock.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 1 && Magma_Totem.KnownSpell
            && Magma_Totem.IsSpellUsable && MySettings.UseMagmaTotem
            && !Fire_Elemental_Totem.SummonedByMeBySpellName())
        {
            Magma_Totem.Launch();
            return;
        }
        else if (Searing_Totem.KnownSpell && Searing_Totem.IsSpellUsable && MySettings.UseSearingTotem
            && FireTotemReady() && !Searing_Totem.SummonedByMeBySpellName(25))
        {
            Searing_Totem.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 1 && Chain_Lightning.KnownSpell 
            && Chain_Lightning.IsSpellUsable && Chain_Lightning.IsDistanceGood && MySettings.UseChainLightning)
        {
            if (Ancestral_Swiftness.KnownSpell && Ancestral_Swiftness.IsSpellUsable 
                && MySettings.UseAncestralSwiftness)
            {
                Ancestral_Swiftness.Launch();
                Thread.Sleep(200);
            }
            Chain_Lightning.Launch();
            return;
        }
        else
        {
            if (Lightning_Bolt.IsDistanceGood && Lightning_Bolt.KnownSpell && Lightning_Bolt.IsSpellUsable
                && MySettings.UseLightningBolt)
            {
                if (Ancestral_Swiftness.KnownSpell && Ancestral_Swiftness.IsSpellUsable
                    && MySettings.UseAncestralSwiftness)
                {
                    Ancestral_Swiftness.Launch();
                    Thread.Sleep(200);
                }
                Lightning_Bolt.Launch();
                return;
            }
        }
    }

    public void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            Heal();
            Buff();
        }
    }

    private void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (Water_Walking.IsSpellUsable && Water_Walking.KnownSpell && (!Water_Walking.HaveBuff || Water_Walking_Timer.IsReady)
            && ObjectManager.GetNumberAttackPlayer() == 0 && !Fight.InFight && MySettings.UseWaterWalking)
        {
            Water_Walking.Launch();
            Water_Walking_Timer = new Timer(1000*60*9);
            return;
        }
        else if ((ObjectManager.Me.ManaPercentage < 5 && Water_Shield.KnownSpell && Water_Shield.IsSpellUsable
            && MySettings.UseWaterShield && !Water_Shield.HaveBuff) || !MySettings.UseLightningShield)
        {
            Water_Shield.Launch();
            return;
        }
        else if (Lightning_Shield.KnownSpell && Lightning_Shield.IsSpellUsable && !Lightning_Shield.HaveBuff
            && MySettings.UseLightningShield && ObjectManager.Me.ManaPercentage > 15)
        {
            Lightning_Shield.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 0 && Spiritwalkers_Grace.IsSpellUsable 
            && Spiritwalkers_Grace.KnownSpell && MySettings.UseSpiritwalkersGrace && ObjectManager.Me.GetMove)
        {
            Spiritwalkers_Grace.Launch();
            return;
        }
        else 
        {
            if (Flametongue_Weapon.KnownSpell && Flametongue_Weapon.IsSpellUsable && !ObjectManager.Me.HaveBuff(10400)
                && MySettings.UseFlametongueWeapon)
            {
                Flametongue_Weapon.Launch();
                return;
            }
            else if (Frostbrand_Weapon.KnownSpell && Frostbrand_Weapon.IsSpellUsable && !ObjectManager.Me.HaveBuff(8034)
                && MySettings.UseFrostbrandWeapon && !MySettings.UseFlametongueWeapon)
            {
                Frostbrand_Weapon.Launch();
                return;
            }
            else
            {
                if (Rockbiter_Weapon.KnownSpell && Rockbiter_Weapon.IsSpellUsable && !ObjectManager.Me.HaveBuff(36494)
                    && MySettings.UseRockbiterWeapon && !MySettings.UseFlametongueWeapon 
                    && !MySettings.UseFrostbrandWeapon)
                {
                    Rockbiter_Weapon.Launch();
                    return;
                }
            }
        }

        if (ObjectManager.GetNumberAttackPlayer() == 0 && Ghost_Wolf.IsSpellUsable && Ghost_Wolf.KnownSpell
            && MySettings.UseGhostWolf && ObjectManager.Me.GetMove)
        {
            Ghost_Wolf.Launch();
            return;
        }
    }

    public bool FireTotemReady()
    {
        if (Fire_Elemental_Totem.SummonedByMeBySpellName() || Magma_Totem.SummonedByMeBySpellName())
            return false;
        return true;
    }

    public bool EarthTotemReady()
    {
        if (Earthbind_Totem.SummonedByMeBySpellName() || Earth_Elemental_Totem.SummonedByMeBySpellName()
            || Stone_Bulwark_Totem.SummonedByMeBySpellName())
            return false;
        return true;
    }

    public bool WaterTotemReady()
    {
        if (Healing_Stream_Totem.SummonedByMeBySpellName() || Healing_Tide_Totem.SummonedByMeBySpellName())
            return false;
        return true;
    }

    public bool AirTotemReady()
    {
        if (Capacitor_Totem.SummonedByMeBySpellName() || Grounding_Totem.SummonedByMeBySpellName() 
            || Stormlash_Totem.SummonedByMeBySpellName())
            return false;
        return true;
    }

    public bool TotemicRecallReady()
    {
        if (Fire_Elemental_Totem.SummonedByMeBySpellName())
            return false;
        else if (Earth_Elemental_Totem.SummonedByMeBySpellName())
            return false;
        else if (Searing_Totem.SummonedByMeBySpellName())
            return true;
        else if (FireTotemReady() && EarthTotemReady() && WaterTotemReady() && AirTotemReady())
            return false;
        else
            return true;
    }

    private void Heal()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (ObjectManager.Me.ManaPercentage < 50 && Totemic_Recall.KnownSpell && Totemic_Recall.IsSpellUsable
            && MySettings.UseTotemicRecall && ObjectManager.GetNumberAttackPlayer() == 0 && !Fight.InFight
            && TotemicRecallReady())
        {
            Totemic_Recall.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 95 && Healing_Surge.KnownSpell && Healing_Surge.IsSpellUsable
            && ObjectManager.GetNumberAttackPlayer() == 0 && !Fight.InFight && MySettings.UseHealingSurge)
        {
            Healing_Surge.Launch();
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(200);
            }
            return;
        }
        else if (Healing_Surge.KnownSpell && Healing_Surge.IsSpellUsable && ObjectManager.Me.HealthPercent < 50
            && MySettings.UseHealingSurge)
        {
            Healing_Surge.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && Gift_of_the_Naaru.KnownSpell && Gift_of_the_Naaru.IsSpellUsable
            && MySettings.UseGiftoftheNaaru)
        {
            Gift_of_the_Naaru.Launch();
            return;
        }
        else if (Healing_Tide_Totem.KnownSpell && Healing_Tide_Totem.IsSpellUsable && ObjectManager.Me.HealthPercent < 70
            && WaterTotemReady() && MySettings.UseHealingTideTotem)
        {
            Healing_Tide_Totem.Launch();
            return;
        }
        else if (Ancestral_Guidance.KnownSpell && Ancestral_Guidance.IsSpellUsable && ObjectManager.Me.HealthPercent < 70
            && MySettings.UseAncestralGuidance)
        {
            Ancestral_Guidance.Launch();
            return;
        }
        else if (Chain_Heal.KnownSpell && Chain_Heal.IsSpellUsable && ObjectManager.Me.HealthPercent < 80
            && MySettings.UseChainHeal)
        {
            Chain_Heal.Launch();
            return;
        }
        else
        {
            if (Healing_Stream_Totem.KnownSpell && Healing_Stream_Totem.IsSpellUsable && ObjectManager.Me.HealthPercent < 90
                && WaterTotemReady() && MySettings.UseHealingStreamTotem)
            {
                Healing_Stream_Totem.Launch();
                return;
            }
        }
    }

    private void Defense_Cycle()
    {
        if (ObjectManager.Me.HealthPercent < 50 && Capacitor_Totem.KnownSpell && Capacitor_Totem.IsSpellUsable
            && AirTotemReady() && MySettings.UseCapacitorTotem)
        {
            Capacitor_Totem.Launch();
            OnCD = new Timer(1000*5);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 50 && Stone_Bulwark_Totem.KnownSpell && Stone_Bulwark_Totem.IsSpellUsable
            && EarthTotemReady() && MySettings.UseStoneBulwarkTotem)
        {
            Stone_Bulwark_Totem.Launch();
            OnCD = new Timer(1000*10);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && War_Stomp.IsSpellUsable && War_Stomp.KnownSpell
            && MySettings.UseWarStomp)
        {
            War_Stomp.Launch();
            OnCD = new Timer(1000*2);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && Stoneform.IsSpellUsable && Stoneform.KnownSpell
            && MySettings.UseStoneform)
        {
            Stoneform.Launch();
            OnCD = new Timer(1000*8);
            return;
        }
        else
        {
            if (ObjectManager.Me.HealthPercent < 70 && Astral_Shift.KnownSpell && Astral_Shift.IsSpellUsable
                && MySettings.UseAstralShift)
            {
                Astral_Shift.Launch();
                OnCD = new Timer(1000*6);
                return;
            }
        }
    }

    private void Decast()
    {
        if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && MySettings.UseWindShear
            && Wind_Shear.KnownSpell && Wind_Shear.IsSpellUsable && Wind_Shear.IsDistanceGood)
        {
            Wind_Shear.Launch();
            return;
        }
        else if (ObjectManager.Target.IsCast && Arcane_Torrent.KnownSpell && Arcane_Torrent.IsSpellUsable
            && MySettings.UseArcaneTorrent && ObjectManager.Target.IsTargetingMe && ObjectManager.Target.GetDistance < 8)
        {
            Arcane_Torrent.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && MySettings.UseGroundingTotem
                && Grounding_Totem.KnownSpell && Grounding_Totem.IsSpellUsable && AirTotemReady())
            {
                Grounding_Totem.Launch();
                return;
            }
        }

        if (ObjectManager.Target.GetMove && !Frost_Shock.TargetHaveBuff && MySettings.UseFrostShock
            && Frost_Shock.KnownSpell && Frost_Shock.IsSpellUsable && Frost_Shock.IsDistanceGood)
        {
            Frost_Shock.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Target.GetMove && MySettings.UseEarthbindTotem && EarthTotemReady()
                && Earthbind_Totem.KnownSpell && Earthbind_Totem.IsSpellUsable && Earthbind_Totem.IsDistanceGood)
            {
                Earthbind_Totem.Launch();
                return;
            }
        }
    }

    private void AvoidMelee()
    {
        if (ObjectManager.Target.GetDistance < 3 && ObjectManager.Target.InCombat)
        {
            nManager.Wow.Helpers.Keybindings.PressKeybindings(nManager.Wow.Enums.Keybindings.MOVEBACKWARD);
        }
    }
}

#endregion
