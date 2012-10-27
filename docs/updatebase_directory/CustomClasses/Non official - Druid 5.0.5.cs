/*
* CustomClass for TheNoobBot
* Credit : Rival, Geesus, Enelya, Marstor, Vesper, Neo2003, Dreadlocks
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
                    #region Druid Specialisation checking

                case WoWClass.Druid:
                    var Druid_Feral_Spell = new Spell("Tiger's Fury");
                    var Druid_Guardian_Spell = new Spell("Savage Defense");
                    var Druid_Balance_Spell = new Spell("Eclipse");
                    var Druid_Restoration_Spell = new Spell("Swiftmend");

                    if (Druid_Feral_Spell.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Druid_Feral.xml";
                            Druid_Feral.DruidFeralSettings CurrentSetting;
                            CurrentSetting = new Druid_Feral.DruidFeralSettings();
                            if (System.IO.File.Exists(CurrentSettingsFile))
                            {
                                CurrentSetting =
                                    Settings.Load<Druid_Feral.DruidFeralSettings>(CurrentSettingsFile);
                            }
                            CurrentSetting.ToForm();
                            CurrentSetting.Save(CurrentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Druid Feral Found");
                            new Druid_Feral();
                        }
                    }
                    else if (Druid_Guardian_Spell.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Druid_Guardian.xml";
                            Druid_Guardian.DruidGuardianSettings CurrentSetting;
                            CurrentSetting = new Druid_Guardian.DruidGuardianSettings();
                            if (System.IO.File.Exists(CurrentSettingsFile))
                            {
                                CurrentSetting =
                                    Settings.Load<Druid_Guardian.DruidGuardianSettings>(CurrentSettingsFile);
                            }
                            CurrentSetting.ToForm();
                            CurrentSetting.Save(CurrentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Feral Guardian Found");
                            new Druid_Guardian();
                        }
                    }
                    else if (Druid_Balance_Spell.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Druid_Balance.xml";
                            Druid_Balance.DruidBalanceSettings CurrentSetting;
                            CurrentSetting = new Druid_Balance.DruidBalanceSettings();
                            if (System.IO.File.Exists(CurrentSettingsFile))
                            {
                                CurrentSetting =
                                    Settings.Load<Druid_Balance.DruidBalanceSettings>(CurrentSettingsFile);
                            }
                            CurrentSetting.ToForm();
                            CurrentSetting.Save(CurrentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Druid Balance Found");
                            range = 30.0f;
                            new Druid_Balance();
                        }
                    }
                    else if (Druid_Restoration_Spell.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Druid_Restoration.xml";
                            Druid_Restoration.DruidRestorationSettings CurrentSetting;
                            CurrentSetting = new Druid_Restoration.DruidRestorationSettings();
                            if (System.IO.File.Exists(CurrentSettingsFile))
                            {
                                CurrentSetting =
                                    Settings.Load<Druid_Restoration.DruidRestorationSettings>(CurrentSettingsFile);
                            }
                            CurrentSetting.ToForm();
                            CurrentSetting.Save(CurrentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Druid Restoration Found");
                            range = 30.0f;
                            new Druid_Restoration();
                        }
                    }
                    else
                    {
                        if (ConfigOnly)
                        {
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Druid_Balance.xml";
                            Druid_Balance.DruidBalanceSettings CurrentSetting;
                            CurrentSetting = new Druid_Balance.DruidBalanceSettings();
                            if (System.IO.File.Exists(CurrentSettingsFile))
                            {
                                CurrentSetting =
                                    Settings.Load<Druid_Balance.DruidBalanceSettings>(CurrentSettingsFile);
                            }
                            CurrentSetting.ToForm();
                            CurrentSetting.Save(CurrentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("No specialisation detected.");
                            Logging.WriteFight("Loading Druid Balance class...");
                            range = 30.0f;
                            new Druid_Balance();
                        }
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

#region Druid

public class Druid_Balance
{
    [Serializable]
    public class DruidBalanceSettings : nManager.Helpful.Settings
    {
        /* Professions & Racials */
        public bool UseArcaneTorrent = true;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseLifeblood = true;
        public bool UseStoneform = true;
        public bool UseGiftoftheNaaru = true;
        public bool UseWarStomp = true;
        /* Druid Buffs */
        public bool UseDash = true;
        public bool UseFaerieFire = true;
        public bool UseMarkoftheWild = true;
        public bool UseMoonkinForm = true;
        public bool UseStampedingRoar = true;
        /* Offensive Spell */
        public bool UseHurricane = true;
        public bool UseMoonfire = true;
        public bool UseStarfall = true;
        public bool UseStarfire = true;
        public bool UseStarsurge = true;
        public bool UseSunfire = true;
        public bool UseWildMushroom = true;
        public bool UseWrath = true;
        /* Offensive Cooldown */
        public bool UseAstralCommunion = true;
        public bool UseCelestialAlignment = true;
        public bool UseForceofNature = true;
        public bool UseHeartoftheWild = true;
        public bool UseIncarnation = true;
        public bool UseNaturesVigil = true;
        /* Defensive Cooldown */
        public bool UseBarkskin = true;
        public bool UseDisorientingRoar = true;
        public bool UseEntanglingRoots = true;
        public bool UseMassEntanglement = true;
        public bool UseMightyBash = true;
        public bool UseNaturesGrasp = true;
        public bool UseSolarBeam = true;
        public bool UseTyphoon = true;
        public bool UseUrsolsVortex = true;
        public bool UseWildCharge = true;
        /* Healing Spell */
        public bool UseCenarionWard = true;
        public bool UseHealingTouch = true;
        public bool UseInnervate = true;
        public bool UseMightofUrsoc = true;
        public bool UseNaturesSwiftness = true;
        public bool UseRejuvenation = true;
        public bool UseRenewal = true;
        public bool UseTranquility = true;
        /* Game Settings */
        public bool UseLowCombat = true;
        public bool UseTrinket = true;
        public bool UseEngGlove = true;
        public bool UseAlchFlask = true;

        public DruidBalanceSettings()
        {
            ConfigWinForm(new System.Drawing.Point(400, 400), "Druid Balance Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");
            AddControlInWinForm("Use Lifeblood", "UseLifeblood", "Professions & Racials");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            /* Druid Buffs */
            AddControlInWinForm("Use Dash", "UseDash", "Druid Buffs");
            AddControlInWinForm("Use Faerie Fire", "UseFaerieFire", "Druid Buffs");
            AddControlInWinForm("Use Mark of the Wild", "UseMarkoftheWild", "Druid Buffs");
            AddControlInWinForm("Use Moonkin Form", "UseMoonkinForm", "Druid Buffs");
            AddControlInWinForm("Use Stampeding Roar", "UseStampedingRoar", "Druid Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Hurricane", "UseHurricane", "Offensive Spell");
            AddControlInWinForm("Use Moonfire", "UseMoonfire", "Offensive Spell");
            AddControlInWinForm("Use Starfall", "UseStarfall", "Offensive Spell");
            AddControlInWinForm("Use Starfire", "UseStarfire", "Offensive Spell");
            AddControlInWinForm("Use Starsurge", "UseStarsurge", "Offensive Spell");
            AddControlInWinForm("Use Sunfire", "UseSunfire", "Offensive Spell");
            AddControlInWinForm("Use WildMushroom", "UseWildMushroom", "Offensive Spell");
            AddControlInWinForm("Use Wrath", "UseWrath", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use Astral Communion", "UseAstralCommunion", "Offensive Cooldown");
            AddControlInWinForm("Use Celestial Alignment", "UseCelestialAlignment", "Offensive Cooldown");
            AddControlInWinForm("Use Force of Nature", "UseForceofNature", "Offensive Cooldown");
            AddControlInWinForm("Use Heart of the Wild", "UseHeartoftheWild", "Offensive Cooldown");
            AddControlInWinForm("Use Incarnation", "UseIncarnation", "Offensive Cooldown");
            AddControlInWinForm("Use Nature's Vigil", "UseNaturesVigil", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Barkskin", "UseBarkskin", "Defensive Cooldown");
            AddControlInWinForm("Use Disorienting Roar", "UseDisorientingRoar", "Defensive Cooldown");
            AddControlInWinForm("Use Entangling Roots", "UseEntanglingRoots", "Defensive Cooldown");
            AddControlInWinForm("Use Mass Entanglement", "UseMassEntanglement", "Defensive Cooldown");
            AddControlInWinForm("Use Mighty Bash", "UseMightyBash", "Defensive Cooldown");
            AddControlInWinForm("Use Nature's Grasp", "UseNaturesGrasp", "Defensive Cooldown");
            AddControlInWinForm("Use Solar Beam", "UseSolarBeam", "Defensive Cooldown");
            AddControlInWinForm("Use Typhoon", "UseTyphoon", "Defensive Cooldown");
            AddControlInWinForm("Use Ursol's Vortex", "UseUrsolsVortex", "Defensive Cooldown");
            AddControlInWinForm("Use Wild Charge", "UseWildCharge", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Cenarion Ward", "UseCenarionWard", "Healing Spell");
            AddControlInWinForm("Use Healing Touch", "UseHealingTouch", "Healing Spell");
            AddControlInWinForm("Use Innervate", "UseInnervate", "Healing Spell");
            AddControlInWinForm("Use Might of Ursoc", "UseMightofUrsoc", "Healing Spell");
            AddControlInWinForm("Use Nature's Swiftness", "UseNaturesSwiftness", "Healing Spell");
            AddControlInWinForm("Use Rejuvenation", "UseRejuvenation", "Healing Spell");
            AddControlInWinForm("Use Renewal", "UseRenewal", "Healing Spell");
            AddControlInWinForm("Use Tranquility", "UseTranquility", "Healing Spell");
            /* Game Settings */
            AddControlInWinForm("Use Low Combat Settings", "UseLowCombat", "Game Settings");
            AddControlInWinForm("Use Trinket", "UseTrinket", "Game Settings");
            AddControlInWinForm("Use Engineering Gloves", "UseEngGlove", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
        }

        public static DruidBalanceSettings CurrentSetting { get; set; }

        public static DruidBalanceSettings GetSettings()
        {
            string CurrentSettingsFile = Application.StartupPath + "\\CustomClasses\\Settings\\Druid_Balance.xml";
            if (System.IO.File.Exists(CurrentSettingsFile))
            {
                return
                    CurrentSetting = Settings.Load<Druid_Balance.DruidBalanceSettings>(CurrentSettingsFile);
            }
            else
            {
                return new Druid_Balance.DruidBalanceSettings();
            }
        }
    }

    private readonly DruidBalanceSettings MySettings = DruidBalanceSettings.GetSettings();

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

    #region Druid Buffs

    private readonly Spell Dash = new Spell("Dash");
    private readonly Spell Faerie_Fire = new Spell("Faerie Fire");
    private readonly Spell Mark_of_the_Wild = new Spell("Mark of the Wild");
    private readonly Spell Moonkin_Form = new Spell("Moonkin Form");
    private readonly Spell Stampeding_Roar = new Spell("Stampeding Roar");

    #endregion

    #region Offensive Spell

    private readonly Spell Hurricane = new Spell("Hurricane");
    private readonly Spell Moonfire = new Spell("Moonfire");
    private Timer Moonfire_Timer = new Timer(0);
    private readonly Spell Starfall = new Spell("Starfall");
    private readonly Spell Starfire = new Spell("Starfire");
    bool StarfireUse = false;
    private readonly Spell Starsurge = new Spell("Starsurge");
    private readonly Spell Sunfire = new Spell("Sunfire");
    private Timer Sunfire_Timer = new Timer(0);
    private readonly Spell Wild_Mushroom = new Spell("Wild Mushroom");
    private readonly Spell Wild_Mushroom_Detonate = new Spell("Wild Mushroom: Detonate");
    private readonly Spell Wrath = new Spell("Wrath");

    #endregion

    #region Offensive Cooldown

    private readonly Spell Astral_Communion = new Spell("Astral Communion");
    private readonly Spell Celestial_Alignment = new Spell("Celestial Alignment");
    private readonly Spell Force_of_Nature = new Spell("Force of Nature");
    private readonly Spell Heart_of_the_Wild = new Spell("Heart of the Wild");
    private readonly Spell Incarnation = new Spell("Incarnation");
    private readonly Spell Natures_Vigil = new Spell("Nature's Vigil");

    #endregion

    #region Defensive Cooldown

    private readonly Spell Barkskin = new Spell("Barkskin");
    private readonly Spell Disorienting_Roar = new Spell("Disorienting Roar");
    private readonly Spell Entangling_Roots = new Spell("Entangling Roots");
    private readonly Spell Mass_Entanglement = new Spell("Mass Entanglement");
    private readonly Spell Mighty_Bash = new Spell("Mighty Bash");
    private readonly Spell Natures_Grasp = new Spell("Nature's Grasp");
    private readonly Spell Solar_Beam = new Spell("Solar Beam");
    private readonly Spell Typhoon = new Spell("Typhoon");
    private readonly Spell Ursols_Vortex = new Spell("Ursol's Vortex");
    private readonly Spell Wild_Charge = new Spell("Wild Charge");

    #endregion

    #region Healing Spell

    private readonly Spell Cenarion_Ward = new Spell("Cenarion Ward");
    private readonly Spell Healing_Touch = new Spell("Healing Touch");
    private Timer Healing_Touch_Timer = new Timer(0);
    private readonly Spell Innervate = new Spell("Innervate");
    private readonly Spell Might_of_Ursoc = new Spell("Might of Ursoc");
    private readonly Spell Natures_Swiftness = new Spell("Nature's Swiftness");
    private readonly Spell Rejuvenation = new Spell("Rejuvenation");
    private readonly Spell Renewal = new Spell("Renewal");
    private readonly Spell Tranquility = new Spell("Tranquility");

    #endregion

    private Timer OnCD = new Timer(0);
    private Timer Trinket_Timer = new Timer(0);
    private Timer Engineering_Timer = new Timer(0);
    private Timer AlchFlask_Timer = new Timer(0);
    int LC = 0;

    public Druid_Balance()
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
                            (Moonfire.IsDistanceGood || Sunfire.IsDistanceGood))
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
            Thread.Sleep(250);
        }
    }

    public void Pull()
    {
        if (!ObjectManager.Me.HaveBuff(24858) && MySettings.UseMoonkinForm)
        {
            Moonkin_Form.Launch();
        }

        if (Moonfire.KnownSpell && Moonfire.IsDistanceGood && Moonfire.IsSpellUsable
            && MySettings.UseMoonfire)
        {
            Moonfire.Launch();
            Moonfire_Timer = new Timer(1000*11);
            return;
        }
        else
        {
            if (Sunfire.KnownSpell && Sunfire.IsDistanceGood && Sunfire.IsSpellUsable
                && MySettings.UseSunfire)
            {
                Sunfire.Launch();
                Sunfire_Timer = new Timer(1000*11);
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

    public void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (Astral_Communion.KnownSpell && Astral_Communion.IsSpellUsable && MySettings.UseAstralCommunion
            && !ObjectManager.Me.HaveBuff(48518) && !ObjectManager.Me.HaveBuff(48517)
            && !Fight.InFight && ObjectManager.GetNumberAttackPlayer() == 0)
        {
            Astral_Communion.Launch();
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(200);
            }
            return;
        }
        else if (AlchFlask_Timer.IsReady && MySettings.UseAlchFlask && Alchemy.KnownSpell 
            && ItemsManager.GetItemCountByIdLUA(75525) == 1)
        {
            Logging.WriteFight("Use Alchi Flask");
            Lua.RunMacroText("/use item:75525");
            AlchFlask_Timer = new Timer(1000*60*60*2);
            return;
        }
        else if (Mark_of_the_Wild.KnownSpell && Mark_of_the_Wild.IsSpellUsable && !Mark_of_the_Wild.HaveBuff
            && MySettings.UseMarkoftheWild)
        {
            Mark_of_the_Wild.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() == 0 && MySettings.UseDash
            && Dash.KnownSpell && Dash.IsSpellUsable && !Dash.HaveBuff && !Stampeding_Roar.HaveBuff
            && ObjectManager.Me.GetMove)
        {
            Dash.Launch();
            return;
        }
        else
        {
            if (ObjectManager.GetNumberAttackPlayer() == 0 && MySettings.UseStampedingRoar
                && Stampeding_Roar.KnownSpell && Stampeding_Roar.IsSpellUsable && !Dash.HaveBuff 
                && !Stampeding_Roar.HaveBuff && ObjectManager.Me.GetMove)
            {
                Stampeding_Roar.Launch();
                return;
            }
        }
    }

    public void LowCombat()
    {
        AvoidMelee();
        Heal();
        Defense_Cycle();
        Buff();

        if (ObjectManager.Me.HaveBuff(48518))
            StarfireUse = true;

        if (ObjectManager.Me.HaveBuff(48517))
            StarfireUse = false;

        if (!ObjectManager.Me.HaveBuff(24858) && MySettings.UseMoonkinForm)
        {
            Moonkin_Form.Launch();
            return;
        }

        if (Starsurge.KnownSpell && Starsurge.IsDistanceGood && Starsurge.IsSpellUsable
            && ObjectManager.Me.HaveBuff(93400) && MySettings.UseStarsurge)
        {
            Starsurge.Launch();
            return;
        }
        else if (Moonfire.KnownSpell && Moonfire.IsDistanceGood && Moonfire.IsSpellUsable
            && !Moonfire.TargetHaveBuff && MySettings.UseMoonfire)
        {
            Moonfire.Launch();
            return;
        }
        else if (Sunfire.KnownSpell && Sunfire.IsDistanceGood && Sunfire.IsSpellUsable
            && !Sunfire.TargetHaveBuff && MySettings.UseSunfire)
        {
            Sunfire.Launch();
            return;
        }
        else if (Starsurge.KnownSpell && Starsurge.IsDistanceGood && Starsurge.IsSpellUsable
            && MySettings.UseStarsurge)
        {
            Starsurge.Launch();
            return;
        }
        else if (Starfire.KnownSpell && Starfire.IsDistanceGood && Starfire.IsSpellUsable
            && StarfireUse && MySettings.UseStarfire)
        {
            Starfire.Launch();
            return;
        }
        else
        {
            if (Wrath.KnownSpell && Wrath.IsDistanceGood && Wrath.IsSpellUsable
                && MySettings.UseWrath)
            {
                Wrath.Launch();
                return;
            }
        }

        if (Hurricane.KnownSpell && Hurricane.IsDistanceGood && Hurricane.IsSpellUsable
            && MySettings.UseHurricane)
        {
            Hurricane.Launch();
            return;
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
        else if (Force_of_Nature.IsSpellUsable && Force_of_Nature.KnownSpell && Force_of_Nature.IsDistanceGood
            && MySettings.UseForceofNature)
        {
            Force_of_Nature.Launch();
            return;
        }
        else if (Incarnation.IsSpellUsable && Incarnation.KnownSpell && MySettings.UseIncarnation
            && ObjectManager.Target.GetDistance < 30)
        {
            Incarnation.Launch();
            return;
        }
        else if (Heart_of_the_Wild.IsSpellUsable && Heart_of_the_Wild.KnownSpell && MySettings.UseHeartoftheWild
            && ObjectManager.Target.GetDistance < 30)
        {
            Heart_of_the_Wild.Launch();
            return;
        }
        else if (Natures_Vigil.IsSpellUsable && Natures_Vigil.KnownSpell && MySettings.UseNaturesVigil
            && ObjectManager.Target.GetDistance < 30)
        {
            Natures_Vigil.Launch();
            return;
        }
        else 
        {
            if (Celestial_Alignment.KnownSpell && MySettings.UseCelestialAlignment && ObjectManager.Target.GetDistance < 30
                && (Celestial_Alignment.IsSpellUsable || ObjectManager.Me.HaveBuff(112071)))
            {
                if (!ObjectManager.Me.HaveBuff(112071))
                    Celestial_Alignment.Launch();
                Celestial_Alignment_Combat();
            }
        }
    }

    public void DPS_Cycle()
    {
        if (ObjectManager.Me.HaveBuff(48518))
            StarfireUse = true;

        if (ObjectManager.Me.HaveBuff(48517))
            StarfireUse = false;

        if (!ObjectManager.Me.HaveBuff(24858) && MySettings.UseMoonkinForm)
        {
            Moonkin_Form.Launch();
            return;
        }

        if (ObjectManager.Me.BarTwoPercentage < 80 && Arcane_Torrent.KnownSpell && Arcane_Torrent.IsSpellUsable
            && MySettings.UseArcaneTorrent)
        {
            Arcane_Torrent.Launch();
            return;
        }
        else if (Moonfire.KnownSpell && Moonfire.IsDistanceGood && Moonfire.IsSpellUsable
            && MySettings.UseMoonfire && (!Moonfire.TargetHaveBuff || Moonfire_Timer.IsReady))
        {
            Moonfire.Launch();
            Moonfire_Timer = new Timer(1000*11);
            return;
        }
        else if (Sunfire.KnownSpell && Sunfire.IsDistanceGood && Sunfire.IsSpellUsable
            && MySettings.UseSunfire && (!Sunfire.TargetHaveBuff || Sunfire_Timer.IsReady))
        {
            Sunfire.Launch();
            Sunfire_Timer = new Timer(1000*11);
            return;
        }
        else if (Starsurge.IsDistanceGood && Starsurge.IsSpellUsable
            && Starsurge.KnownSpell && MySettings.UseStarsurge)
        {
            Starsurge.Launch();
            return;
        }
        else if (Starfall.KnownSpell && Starfall.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() > 2 &&
            ObjectManager.Target.GetDistance < 40 && MySettings.UseStarfall)
        {
            Starfall.Launch();
            return;
        }
        else if (Wild_Mushroom.KnownSpell && Wild_Mushroom.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() > 3
            && ObjectManager.Target.GetDistance < 40 && Wild_Mushroom_Detonate.KnownSpell && Wild_Mushroom.IsSpellUsable
            && MySettings.UseWildMushroom)
        {
            for (int i = 0; i < 3; i++)
            {
                SpellManager.CastSpellByIDAndPosition(88747, ObjectManager.Target.Position);
                Thread.Sleep(200);
            }

            Wild_Mushroom_Detonate.Launch();
            return;
        }
        else if (Hurricane.KnownSpell && Hurricane.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() > 2 &&
            ObjectManager.Target.GetDistance < 30 && MySettings.UseHurricane)
        {
            SpellManager.CastSpellByIDAndPosition(16914, ObjectManager.Target.Position);
            return;
        }
        else if (Starfire.KnownSpell && Starfire.IsSpellUsable && Starfire.IsDistanceGood
            && StarfireUse && MySettings.UseStarfire)
        {
            Starfire.Launch();
            return;
        }
        else
        {
            if (Wrath.KnownSpell && Wrath.IsSpellUsable && Wrath.IsDistanceGood
                && MySettings.UseWrath)
            {
                Wrath.Launch();
                return;
            }
        }
    }

    public void Celestial_Alignment_Combat()
    {
        while (ObjectManager.Me.HaveBuff(112071))
        {
            if (!Moonfire.TargetHaveBuff || Moonfire_Timer.IsReady || !Sunfire.TargetHaveBuff || Sunfire_Timer.IsReady)
            {
                Moonfire.Launch();
                Moonfire_Timer = new Timer(1000*11);
                Sunfire_Timer = new Timer(1000*11);
            }

            if (Wrath.KnownSpell && Wrath.IsDistanceGood && Wrath.IsSpellUsable)
                Wrath.Launch();
        }
        return;
    }

    private void Heal()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (ObjectManager.Me.HealthPercent < 80 && Natures_Swiftness.IsSpellUsable && Natures_Swiftness.KnownSpell
            && MySettings.UseNaturesSwiftness && MySettings.UseHealingTouch)
        {
            Natures_Swiftness.Launch();
            Thread.Sleep(400);
            Healing_Touch.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 70 && Renewal.IsSpellUsable && Renewal.KnownSpell
            && MySettings.UseRenewal)
        {
            Renewal.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 95 && !Fight.InFight && ObjectManager.GetNumberAttackPlayer() == 0
            && Healing_Touch.IsSpellUsable && Healing_Touch.KnownSpell && MySettings.UseHealingTouch)
        {
            Healing_Touch.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 90 && Cenarion_Ward.IsSpellUsable && Cenarion_Ward.KnownSpell
            && MySettings.UseCenarionWard)
        {
            Cenarion_Ward.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && Gift_of_the_Naaru.IsSpellUsable && Gift_of_the_Naaru.KnownSpell
            && MySettings.UseGiftoftheNaaru)
        {
            Gift_of_the_Naaru.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 70 && Rejuvenation.IsSpellUsable && Rejuvenation.KnownSpell
            && !Rejuvenation.HaveBuff && MySettings.UseRejuvenation)
        {
            Rejuvenation.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 40 && Healing_Touch.IsSpellUsable && Healing_Touch.KnownSpell
            && Healing_Touch_Timer.IsReady && MySettings.UseHealingTouch)
        {
            Healing_Touch.Launch();
            Healing_Touch_Timer = new Timer(1000*15);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 35 && Might_of_Ursoc.IsSpellUsable && Might_of_Ursoc.KnownSpell
            && MySettings.UseMightofUrsoc)
        {
            Might_of_Ursoc.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Me.HealthPercent < 30 && Tranquility.IsSpellUsable && Tranquility.KnownSpell
            && MySettings.UseTranquility)
            {
                Tranquility.Launch();
                while (ObjectManager.Me.IsCast)
                {
                    Thread.Sleep(100);
                    Thread.Sleep(100);
                }
                return;
            }
        }

        if (ObjectManager.Me.ManaPercentage < 50 && MySettings.UseInnervate)
        {
            Innervate.Launch();
            return;
        }
    }

    public void Defense_Cycle()
    {
        if (!ObjectManager.Me.HaveBuff(24858) && MySettings.UseMoonkinForm)
        {
            Moonkin_Form.Launch();
        }

        if (ObjectManager.Me.HealthPercent < 80 && Stoneform.IsSpellUsable && Stoneform.KnownSpell
            && MySettings.UseStoneform)
        {
            Stoneform.Launch();
            OnCD = new Timer(1000*8);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && Barkskin.KnownSpell && Barkskin.IsSpellUsable
            && MySettings.UseBarkskin)
        {
            Barkskin.Launch();
            OnCD = new Timer(1000*12);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && Mighty_Bash.KnownSpell && Mighty_Bash.IsSpellUsable
            && MySettings.UseMightyBash && Mighty_Bash.IsDistanceGood)
        {
            Mighty_Bash.Launch();
            OnCD = new Timer(1000*5);
            return;
        }
        else if (Mass_Entanglement.KnownSpell && Mass_Entanglement.IsSpellUsable && Mass_Entanglement.IsDistanceGood
            && MySettings.UseMassEntanglement && ObjectManager.Me.HealthPercent < 80)
        {
            Mass_Entanglement.Launch();

            if (Wild_Charge.KnownSpell && Wild_Charge.IsDistanceGood && Wild_Charge.IsSpellUsable
                && MySettings.UseWildCharge)
            {
                Thread.Sleep(200);
                Wild_Charge.Launch();
            }
            return;
        }
        else if (Ursols_Vortex.KnownSpell && Ursols_Vortex.IsSpellUsable && Ursols_Vortex.IsDistanceGood
            && MySettings.UseUrsolsVortex && ObjectManager.Me.HealthPercent < 80)
        {
            Ursols_Vortex.Launch();

            if (Wild_Charge.KnownSpell && Wild_Charge.IsDistanceGood && Wild_Charge.IsSpellUsable
                && MySettings.UseWildCharge)
            {
                Thread.Sleep(200);
                Wild_Charge.Launch();
            }
            return;
        }
        else if (Natures_Grasp.KnownSpell && Natures_Grasp.IsSpellUsable
            && ObjectManager.Target.IsCast && MySettings.UseNaturesGrasp && ObjectManager.Me.HealthPercent < 80)
        {
            Natures_Grasp.Launch();

            if (Wild_Charge.KnownSpell && Wild_Charge.IsDistanceGood && Wild_Charge.IsSpellUsable
                && MySettings.UseWildCharge)
            {
                Thread.Sleep(200);
                Wild_Charge.Launch();
            }
            return;
        }
        else if (Typhoon.KnownSpell && Typhoon.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() > 2
            && ObjectManager.Target.GetDistance < 40 && ObjectManager.Me.HealthPercent < 70
            && MySettings.UseTyphoon)
        {
            Typhoon.Launch();
            return;
        }
        else if (Disorienting_Roar.KnownSpell && Disorienting_Roar.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() > 2
            && ObjectManager.Target.GetDistance < 10 && ObjectManager.Me.HealthPercent < 70
            && MySettings.UseDisorientingRoar)
        {
            Disorienting_Roar.Launch();
            OnCD = new Timer(1000*3);
            return;
        }
        else
        {
            if (ObjectManager.Me.HealthPercent < 80 && War_Stomp.IsSpellUsable && War_Stomp.KnownSpell
                && MySettings.UseWarStomp)
            {
                War_Stomp.Launch();
                OnCD = new Timer(1000*2);
                return;
            }
        }
    }

    private void Decast()
    {
        if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe
            && Solar_Beam.KnownSpell && Solar_Beam.IsSpellUsable && Solar_Beam.IsDistanceGood
            && MySettings.UseSolarBeam)
        {
            if (Entangling_Roots.KnownSpell && Entangling_Roots.IsDistanceGood && Entangling_Roots.IsSpellUsable
                && MySettings.UseEntanglingRoots)
            {
                Entangling_Roots.Launch();
                Thread.Sleep(200);
            }

            Solar_Beam.Launch();
            return;
        }
    }

    private void AvoidMelee()
    {
        if (ObjectManager.Target.GetDistance < 1)
        {
            Keyboard.DownKey(nManager.Wow.Memory.WowProcess.MainWindowHandle, "{DOWN}");
        }
    }
}

public class Druid_Feral
{
    [Serializable]
    public class DruidFeralSettings : nManager.Helpful.Settings
    {
        /* Professions & Racials */
        public bool UseArcaneTorrent = true;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseLifeblood = true;
        public bool UseStoneform = true;
        public bool UseGiftoftheNaaru = true;
        public bool UseWarStomp = true;
        /* Druid Buffs */
        public bool UseCatForm = true;
        public bool UseDash = true;
        public bool UseDisplacerBeast = false;
        public bool UseFaerieFire = true;
        public bool UseMarkoftheWild = true;
        public bool UseProwl = false;
        public bool UseSavageRoar = true;
        public bool UseStampedingRoar = true;
        /* Offensive Spell */
        public bool UseFerociousBite = true;
        public bool UseMaim = true;
        public bool UseMangle = true;
        public bool UsePounce = true;
        public bool UseRake = true;
        public bool UseRavage = true;
        public bool UseRip = true;
        public bool UseShred = true;
        public bool UseSwipe = true;
        public bool UseThrash = true;
        /* Offensive Cooldown */
        public bool UseBerserk = true;
        public bool UseForceofNature = true;
        public bool UseHeartoftheWild = true;
        public bool UseIncarnation = true;
        public bool UseNaturesVigil = true;
        public bool UseTigersFury = true;
        /* Defensive Cooldown */
        public bool UseBarkskin = true;
        public bool UseDisorientingRoar = true;
        public bool UseMassEntanglement = true;
        public bool UseMightyBash = true;
        public bool UseNaturesGrasp = true;
        public bool UseSkullBash = true;
        public bool UseSurvivalInstincts = true;
        public bool UseTyphoon = true;
        public bool UseUrsolsVortex = true;
        public bool UseWildCharge = true;
        /* Healing Spell */
        public bool UseCenarionWard = true;
        public bool UseHealingTouch = true;
        public bool UseInnervate = true;
        public bool UseMightofUrsoc = true;
        public bool UseNaturesSwiftness = true;
        public bool UseRejuvenation = true;
        public bool UseRenewal = true;
        public bool UseTranquility = true;
        /* Game Settings */
        public bool UseGlyphofShred = false;
        public bool UseLowCombat = true;
        public bool UseTrinket = true;
        public bool UseEngGlove = true;
        public bool UseAlchFlask = true;

        public DruidFeralSettings()
        {
            ConfigWinForm(new System.Drawing.Point(400, 400), "Druid Feral Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");
            AddControlInWinForm("Use Lifeblood", "UseLifeblood", "Professions & Racials");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            /* Druid Buffs */
            AddControlInWinForm("Use Cat Form", "UseCatForm", "Druid Buffs");
            AddControlInWinForm("Use Dash", "UseDash", "Druid Buffs");
            AddControlInWinForm("Use Displacer Beast", "UseDisplacerBeast", "Druid Buffs");
            AddControlInWinForm("Use Faerie Fire", "UseFaerieFire", "Druid Buffs");
            AddControlInWinForm("Use Mark of the Wild", "UseMarkoftheWild", "Druid Buffs");
            AddControlInWinForm("Use Prowl", "UseProwl", "Druid Buffs");
            AddControlInWinForm("Use Savage Roar", "UseSavageRoar", "Druid Buffs");
            AddControlInWinForm("Use Stampeding Roar", "UseStampedingRoar", "Druid Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Ferocious Bite", "UseFerociousBite", "Offensive Spell");
            AddControlInWinForm("Use Maim ", "UseMaim ", "Offensive Spell");
            AddControlInWinForm("Use Mangle", "UseMangle", "Offensive Spell");
            AddControlInWinForm("Use Pounce", "UsePounce", "Offensive Spell");
            AddControlInWinForm("Use Rake", "UseRake", "Offensive Spell");
            AddControlInWinForm("Use Ravage", "UseRavage", "Offensive Spell");
            AddControlInWinForm("Use Rip", "UseRip", "Offensive Spell");
            AddControlInWinForm("Use Shred", "UseShred", "Offensive Spell");
            AddControlInWinForm("Use Swipe", "UseSwipe", "Offensive Spell");
            AddControlInWinForm("Use Thrash", "UseThrash", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use Berserk", "UseBerserk", "Offensive Cooldown");
            AddControlInWinForm("Use Force of Nature", "UseForceofNature", "Offensive Cooldown");
            AddControlInWinForm("Use Heart of the Wild", "UseHeartoftheWild", "Offensive Cooldown");
            AddControlInWinForm("Use Incarnation", "UseIncarnation", "Offensive Cooldown");
            AddControlInWinForm("Use Nature's Vigil", "UseNaturesVigil", "Offensive Cooldown");
            AddControlInWinForm("Use Tiger's Fury", "UseTigersFury", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Barkskin", "UseBarkskin", "Defensive Cooldown");
            AddControlInWinForm("Use Disorienting Roar", "UseDisorientingRoar", "Defensive Cooldown");
            AddControlInWinForm("Use Mass Entanglement", "UseMassEntanglement", "Defensive Cooldown");
            AddControlInWinForm("Use Mighty Bash", "UseMightyBash", "Defensive Cooldown");
            AddControlInWinForm("Use Nature's Grasp", "UseNaturesGrasp", "Defensive Cooldown");
            AddControlInWinForm("Use Skull Bash", "UseSkullBash", "Defensive Cooldown");
            AddControlInWinForm("Use Survival Instincts", "UseSurvivalInstincts", "Defensive Cooldown");
            AddControlInWinForm("Use Typhoon", "UseTyphoon", "Defensive Cooldown");
            AddControlInWinForm("Use Ursol's Vortex", "UseUrsolsVortex", "Defensive Cooldown");
            AddControlInWinForm("Use Wild Charge", "UseWildCharge", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Cenarion Ward", "UseCenarionWard", "Healing Spell");
            AddControlInWinForm("Use Healing Touch", "UseHealingTouch", "Healing Spell");
            AddControlInWinForm("Use Innervate", "UseInnervate", "Healing Spell");
            AddControlInWinForm("Use Might of Ursoc", "UseMightofUrsoc", "Healing Spell");
            AddControlInWinForm("Use Nature's Swiftness", "UseNaturesSwiftness", "Healing Spell");
            AddControlInWinForm("Use Rejuvenation", "UseRejuvenation", "Healing Spell");
            AddControlInWinForm("Use Renewal", "UseRenewal", "Healing Spell");
            AddControlInWinForm("Use Tranquility", "UseTranquility", "Healing Spell");
            /* Game Settings */
            AddControlInWinForm("Using Glyph of Shred?", "UseGlyphofShred", "Game Settings");
            AddControlInWinForm("Use Low Combat Settings", "UseLowCombat", "Game Settings");
            AddControlInWinForm("Use Trinket", "UseTrinket", "Game Settings");
            AddControlInWinForm("Use Engineering Gloves", "UseEngGlove", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
        }

        public static DruidFeralSettings CurrentSetting { get; set; }

        public static DruidFeralSettings GetSettings()
        {
            string CurrentSettingsFile = Application.StartupPath + "\\CustomClasses\\Settings\\Druid_Feral.xml";
            if (System.IO.File.Exists(CurrentSettingsFile))
            {
                return
                    CurrentSetting = Settings.Load<Druid_Feral.DruidFeralSettings>(CurrentSettingsFile);
            }
            else
            {
                return new Druid_Feral.DruidFeralSettings();
            }
        }
    }

    private readonly DruidFeralSettings MySettings = DruidFeralSettings.GetSettings();
    
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

    #region Druid Buffs

    private readonly Spell Cat_Form = new Spell("Cat Form");
    private readonly Spell Dash = new Spell("Dash");
    private readonly Spell Displacer_Beast = new Spell("Displacer Beast");
    private readonly Spell Faerie_Fire = new Spell("Faerie Fire");
    private readonly Spell Mark_of_the_Wild = new Spell("Mark of the Wild");
    private readonly Spell Prowl = new Spell("Prowl");
    private readonly Spell Savage_Roar = new Spell("Savage Roar");
    private Timer Savage_Roar_Timer = new Timer(0);
    bool FivePtSav = false;
    private readonly Spell Stampeding_Roar = new Spell("Stampeding Roar");

    #endregion

    #region Offensive Spell

    private readonly Spell Ferocious_Bite = new Spell("Ferocious Bite");
    bool FivePtFer = false;
    private readonly Spell Maim = new Spell("Maim");
    private readonly Spell Mangle = new Spell("Mangle");
    private readonly Spell Pounce = new Spell("Pounce");
    private readonly Spell Rake = new Spell("Rake");
    private Timer Rake_Timer = new Timer(0);
    private readonly Spell Ravage = new Spell("Ravage");
    private readonly Spell Rip = new Spell("Rip");
    private Timer Rip_Timer = new Timer(0);
    bool FivePtRip = false;
    private readonly Spell Shred = new Spell("Shred");
    private readonly Spell Swipe = new Spell("Swipe");
    private readonly Spell Thrash = new Spell("Thrash");

    #endregion

    #region Offensive Cooldown

    private readonly Spell Berserk = new Spell("Berserk"); 
    private readonly Spell Force_of_Nature = new Spell("Force of Nature");
    private readonly Spell Heart_of_the_Wild = new Spell("Heart of the Wild");
    private readonly Spell Incarnation = new Spell("Incarnation");
    private readonly Spell Natures_Vigil = new Spell("Nature's Vigil");
    private readonly Spell Tigers_Fury = new Spell("Tiger's Fury");

    #endregion

    #region Defensive Cooldown

    private readonly Spell Barkskin = new Spell("Barkskin");
    private readonly Spell Disorienting_Roar = new Spell("Disorienting Roar");
    private readonly Spell Mass_Entanglement = new Spell("Mass Entanglement");
    private readonly Spell Mighty_Bash = new Spell("Mighty Bash");
    private readonly Spell Natures_Grasp = new Spell("Nature's Grasp");
    private readonly Spell Skull_Bash = new Spell("Skull Bash");
    private readonly Spell Survival_Instincts = new Spell("Survival Instincts");
    private readonly Spell Typhoon = new Spell("Typhoon");
    private readonly Spell Ursols_Vortex = new Spell("Ursol's Vortex");
    private readonly Spell Wild_Charge = new Spell("Wild Charge");

    #endregion

    #region Healing Spell

    private readonly Spell Cenarion_Ward = new Spell("Cenarion Ward");
    private readonly Spell Healing_Touch = new Spell("Healing Touch");
    private Timer Healing_Touch_Timer = new Timer(0);
    private readonly Spell Innervate = new Spell("Innervate");
    private readonly Spell Might_of_Ursoc = new Spell("Might of Ursoc");
    private readonly Spell Natures_Swiftness = new Spell("Nature's Swiftness");
    private readonly Spell Rejuvenation = new Spell("Rejuvenation");
    private readonly Spell Renewal = new Spell("Renewal");
    private readonly Spell Tranquility = new Spell("Tranquility");

    #endregion

    private Timer OnCD = new Timer(0);
    private Timer Trinket_Timer = new Timer(0);
    private Timer Engineering_Timer = new Timer(0);
    private Timer AlchFlask_Timer = new Timer(0);
    int CP = 0;
    int LC = 0;

    public Druid_Feral()
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
                            (Faerie_Fire.IsDistanceGood || Wild_Charge.IsDistanceGood))
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

    public void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            Heal();
            Buff();
        }
    }

    public void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (Mark_of_the_Wild.KnownSpell && Mark_of_the_Wild.IsSpellUsable && !Mark_of_the_Wild.HaveBuff)
        {
            Mark_of_the_Wild.Launch();
            return;
        }
        else if (AlchFlask_Timer.IsReady && MySettings.UseAlchFlask && Alchemy.KnownSpell
            && ItemsManager.GetItemCountByIdLUA(75525) == 1)
        {
            Logging.WriteFight("Use Alchi Flask");
            Lua.RunMacroText("/use item:75525");
            AlchFlask_Timer = new Timer(1000*60*60*2);
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() == 0 && MySettings.UseDash
            && Dash.KnownSpell && Dash.IsSpellUsable && !Dash.HaveBuff && !Stampeding_Roar.HaveBuff
            && ObjectManager.Me.GetMove)
        {
            Dash.Launch();
            return;
        }
        else
        {
            if (ObjectManager.GetNumberAttackPlayer() == 0 && MySettings.UseStampedingRoar
                && Stampeding_Roar.KnownSpell && Stampeding_Roar.IsSpellUsable && !Dash.HaveBuff 
                && !Stampeding_Roar.HaveBuff && ObjectManager.Me.GetMove)
            {
                Stampeding_Roar.Launch();
                return;
            }
        }
    }

    public void Pull()
    {
        if (!ObjectManager.Me.HaveBuff(768) && MySettings.UseCatForm)
            Cat_Form.Launch();

        if (Prowl.IsSpellUsable && Prowl.KnownSpell && Prowl.IsDistanceGood
            && MySettings.UseProwl && !ObjectManager.Me.InCombat)
        {
            if (Displacer_Beast.IsSpellUsable && Displacer_Beast.KnownSpell && Displacer_Beast.IsDistanceGood
                && MySettings.UseDisplacerBeast)
            {
                Displacer_Beast.Launch();
                Thread.Sleep(200);
            }

            if (Pounce.IsSpellUsable && Pounce.KnownSpell && Pounce.IsDistanceGood
                && MySettings.UsePounce)
            {
                Pounce.Launch();
                return;
            }
        }
        else
        {
            if (Wild_Charge.KnownSpell && Wild_Charge.IsSpellUsable && Wild_Charge.IsDistanceGood
            && MySettings.UseWildCharge)
            {
                Wild_Charge.Launch();
                return;
            }

            else
            {
                if (Faerie_Fire.KnownSpell && Faerie_Fire.IsSpellUsable && Faerie_Fire.IsDistanceGood
                    && MySettings.UseFaerieFire)
                {
                    Faerie_Fire.Launch();
                    return;
                }
            }
        }
    }

    public void LowCombat()
    {
        AvoidMelee();
        Heal();
        Defense_Cycle();
        Buff();

        if (!ObjectManager.Me.HaveBuff(768) && MySettings.UseCatForm)
        {
            Cat_Form.Launch();
            return;
        }

        if (Mangle.IsSpellUsable && Mangle.KnownSpell && Mangle.IsDistanceGood
            && MySettings.UseMangle)
        {
            Mangle.Launch();
            if (ObjectManager.Target.HealthPercent < 50 && ObjectManager.Target.HealthPercent > 0)
            {
                Mangle.Launch();
                return;
            }
        }
        else
        {
            if (Swipe.IsSpellUsable && Swipe.KnownSpell && Swipe.IsDistanceGood
            && MySettings.UseSwipe)
            {
                Swipe.Launch();
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
        else if (Force_of_Nature.IsSpellUsable && Force_of_Nature.KnownSpell && Force_of_Nature.IsDistanceGood
            && MySettings.UseForceofNature)
        {
            Force_of_Nature.Launch();
            return;
        }
        else if (Incarnation.IsSpellUsable && Incarnation.KnownSpell && MySettings.UseIncarnation
            && ObjectManager.Target.GetDistance < 30)
        {
            Incarnation.Launch();
            return;
        }
        else if (Heart_of_the_Wild.IsSpellUsable && Heart_of_the_Wild.KnownSpell && MySettings.UseHeartoftheWild
            && ObjectManager.Target.GetDistance < 30)
        {
            Heart_of_the_Wild.Launch();
            return;
        }
        else if (Natures_Vigil.IsSpellUsable && Natures_Vigil.KnownSpell && MySettings.UseNaturesVigil
            && ObjectManager.Target.GetDistance < 30)
        {
            Natures_Vigil.Launch();
            return;
        }
        else if (Tigers_Fury.KnownSpell && Tigers_Fury.IsSpellUsable && ObjectManager.Me.Energy < 35
            && !Berserk.HaveBuff && MySettings.UseTigersFury && ObjectManager.Target.GetDistance < 30)
        {
            Tigers_Fury.Launch();
            return;
        }
        else
        {
            if (Berserk.KnownSpell && Berserk.IsSpellUsable && MySettings.UseBerserk
                && ObjectManager.Target.GetDistance < 30)
            {
                Berserk.Launch();
                return;
            }
        }
    }

    public void DPS_Cycle()
    {
        if (!ObjectManager.Me.HaveBuff(768) && MySettings.UseCatForm)
        {
            Cat_Form.Launch();
            return;
        }

        if (Faerie_Fire.KnownSpell && Faerie_Fire.IsSpellUsable && Faerie_Fire.IsDistanceGood 
            && MySettings.UseFaerieFire && (!Faerie_Fire.TargetHaveBuff || !ObjectManager.Target.HaveBuff(113746)))
        {
            Faerie_Fire.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 2 && Thrash.IsSpellUsable && Thrash.KnownSpell
            && Thrash.IsDistanceGood && !Thrash.TargetHaveBuff && MySettings.UseThrash)
        {
            Thrash.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 2 && Swipe.IsSpellUsable && Swipe.KnownSpell
            && Swipe.IsDistanceGood && MySettings.UseSwipe)
        {
            Swipe.Launch();
            return;
        }
        else if (Savage_Roar.IsSpellUsable && Savage_Roar.KnownSpell && Savage_Roar.IsDistanceGood && !FivePtSav
            && ObjectManager.Me.ComboPoint == 5 && MySettings.UseSavageRoar)
        {
            CP = ObjectManager.Me.ComboPoint;
            Savage_Roar.Launch();
            Savage_Roar_Timer = new Timer(1000*(12 + (6*CP)));
            FivePtSav = true;
            return;
        }
        else if (Savage_Roar.IsSpellUsable && Savage_Roar.KnownSpell && Savage_Roar.IsDistanceGood
            && (!ObjectManager.Me.HaveBuff(127538) || Savage_Roar_Timer.IsReady) && MySettings.UseSavageRoar)
        {
            CP = ObjectManager.Me.ComboPoint;
            Savage_Roar.Launch();
            Savage_Roar_Timer = new Timer(1000*(9 + (6*CP)));
            FivePtSav = false;
            return;
        }
        else
        {
            if (Rake.IsSpellUsable && Rake.KnownSpell && Rake.IsDistanceGood && !Rake.TargetHaveBuff
                && MySettings.UseRake)
            {
                Rake.Launch();
                return;
            }
        }

        if (ObjectManager.Target.HealthPercent > 24)
        {
            if (Rip.IsSpellUsable && Rip.KnownSpell && Rip.IsDistanceGood && !FivePtRip 
                && ObjectManager.Me.ComboPoint == 5 && MySettings.UseRip)
            {
                Rip.Launch();
                Rip_Timer = new Timer(1000*13);
                FivePtRip = true;
                return;
            }

            if (Rip.IsSpellUsable && Rip.KnownSpell && Rip.IsDistanceGood && MySettings.UseRip
                && (!Rip.TargetHaveBuff || Rip_Timer.IsReady))
            {
                Rip.Launch();
                Rip_Timer = new Timer(1000*13);
                FivePtRip = false;
                return;
            }
        }
        else
        {
            if (Rip.IsSpellUsable && Rip.KnownSpell && Rip.IsDistanceGood && !Rip.TargetHaveBuff
                && MySettings.UseRip)
            {
                CP = ObjectManager.Me.ComboPoint;
                Rip.Launch();
                Rip_Timer = new Timer(1000*13);
                if (CP == 5)
                    FivePtFer = true;
                else
                    FivePtFer = false;
                return;
            }

            if (Ferocious_Bite.IsSpellUsable && Ferocious_Bite.KnownSpell && Ferocious_Bite.IsDistanceGood 
                && !FivePtFer && ObjectManager.Me.ComboPoint == 5 && MySettings.UseFerociousBite)
            {
                Ferocious_Bite.Launch();
                Rip_Timer = new Timer(1000*13);
                FivePtFer = true;
                return;
            }

            if (Ferocious_Bite.IsSpellUsable && Ferocious_Bite.KnownSpell && Ferocious_Bite.IsDistanceGood 
                && MySettings.UseFerociousBite && (!Rip.TargetHaveBuff || Rip_Timer.IsReady))
            {
                Ferocious_Bite.Launch();
                Rip_Timer = new Timer(1000*13);
                FivePtFer = false;
                return;
            }
        }

        if (ObjectManager.Me.HaveBuff(102543))
        {
            if (Ravage.KnownSpell && Ravage.IsSpellUsable && Ravage.IsDistanceGood
                && MySettings.UseRavage)
            {
                Ravage.Launch();
                return;
            }
        }
        else if (Shred.KnownSpell && Shred.IsSpellUsable && Shred.IsDistanceGood
            && MySettings.UseShred && MySettings.UseGlyphofShred 
            && (Tigers_Fury.HaveBuff || Berserk.HaveBuff))
        {
            Shred.Launch();
            return;
        }
        else
        {
            if (Mangle.KnownSpell && Mangle.IsSpellUsable && Mangle.IsDistanceGood
                && MySettings.UseMangle)
            {
                Mangle.Launch();
                return;
            }
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (ObjectManager.Me.HealthPercent < 80 && Natures_Swiftness.IsSpellUsable && Natures_Swiftness.KnownSpell
            && MySettings.UseNaturesSwiftness && MySettings.UseHealingTouch)
        {
            Natures_Swiftness.Launch();
            Thread.Sleep(400);
            Healing_Touch.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 70 && Renewal.IsSpellUsable && Renewal.KnownSpell
            && MySettings.UseRenewal)
        {
            Renewal.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 90 && Cenarion_Ward.IsSpellUsable && Cenarion_Ward.KnownSpell
            && MySettings.UseCenarionWard)
        {
            Cenarion_Ward.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 50 && !Fight.InFight && ObjectManager.GetNumberAttackPlayer() == 0
            && Healing_Touch.IsSpellUsable && Healing_Touch.KnownSpell && MySettings.UseHealingTouch)
        {
            while (ObjectManager.Me.HealthPercent < 95 && Healing_Touch.IsSpellUsable)
            {
                Healing_Touch.Launch();
                Thread.Sleep(1500);
            }
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && Gift_of_the_Naaru.IsSpellUsable && Gift_of_the_Naaru.KnownSpell)
        {
            Gift_of_the_Naaru.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 70 && Healing_Touch.IsSpellUsable && Healing_Touch.KnownSpell
            && ObjectManager.Me.HaveBuff(69369) && MySettings.UseHealingTouch)
        {
            Healing_Touch.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 50 && Rejuvenation.IsSpellUsable && Rejuvenation.KnownSpell
            && !Rejuvenation.HaveBuff && MySettings.UseRejuvenation)
        {
            Rejuvenation.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 40 && Healing_Touch.IsSpellUsable && Healing_Touch.KnownSpell
            && Healing_Touch_Timer.IsReady && MySettings.UseHealingTouch)
        {
            Healing_Touch.Launch();
            Healing_Touch_Timer = new Timer(1000*15);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 35 && Might_of_Ursoc.IsSpellUsable && Might_of_Ursoc.KnownSpell
            && MySettings.UseMightofUrsoc)
        {
            Might_of_Ursoc.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 30 && Tranquility.IsSpellUsable && Tranquility.KnownSpell
            && MySettings.UseTranquility)
        {
            Tranquility.Launch();
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(100);
                Thread.Sleep(100);
            }
            return;
        }
        else
        {
            if (ObjectManager.Me.ManaPercentage < 10 && MySettings.UseInnervate)
            {
                Innervate.Launch();
                return;
            }
        }
    }

    private void Defense_Cycle()
    {
        if (ObjectManager.Me.HealthPercent < 80 && MySettings.UseBarkskin
            && Barkskin.KnownSpell && Barkskin.IsSpellUsable)
        {
            Barkskin.Launch();
            OnCD = new Timer(1000*12);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 90 && Mighty_Bash.IsDistanceGood
            && Mighty_Bash.KnownSpell && Mighty_Bash.IsSpellUsable && MySettings.UseMightyBash)
        {
            Mighty_Bash.Launch();
            OnCD = new Timer(1000*5);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 70 && ObjectManager.Me.ComboPoint > 2
            && Maim.KnownSpell && Maim.IsSpellUsable && MySettings.UseMaim && Maim.IsDistanceGood)
        {
            CP = ObjectManager.Me.ComboPoint;
            Maim.Launch();
            OnCD = new Timer(1000*CP);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 70 && MySettings.UseSurvivalInstincts
            && Survival_Instincts.KnownSpell && Survival_Instincts.IsSpellUsable)
        {
            Survival_Instincts.Launch();
            OnCD = new Timer(1000*12);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && Stoneform.IsSpellUsable && Stoneform.KnownSpell
            && MySettings.UseStoneform)
        {
            Stoneform.Launch();
            OnCD = new Timer(1000*8);
            return;
        }
        else if (Mass_Entanglement.KnownSpell && Mass_Entanglement.IsSpellUsable && Mass_Entanglement.IsDistanceGood
            && ObjectManager.Target.IsCast && MySettings.UseMassEntanglement && ObjectManager.GetNumberAttackPlayer() > 2
            && ObjectManager.Me.HealthPercent < 70)
        {
            if (Typhoon.KnownSpell && Typhoon.IsSpellUsable && MySettings.UseTyphoon 
                && ObjectManager.Target.GetDistance < 40 && MySettings.UseTyphoon)
            {
                Typhoon.Launch();
                Thread.Sleep(200);
            }

            Mass_Entanglement.Launch();
            return;
        }
        else if (Ursols_Vortex.KnownSpell && Ursols_Vortex.IsSpellUsable && Ursols_Vortex.IsDistanceGood
            && MySettings.UseUrsolsVortex && ObjectManager.Me.HealthPercent < 80 
            && ObjectManager.GetNumberAttackPlayer() > 2)
        {
            Ursols_Vortex.Launch();

            if (Wild_Charge.KnownSpell && Wild_Charge.IsDistanceGood && Wild_Charge.IsSpellUsable
                && MySettings.UseWildCharge)
            {
                Thread.Sleep(200);
                Wild_Charge.Launch();
            }
            return;
        }
        else if (Natures_Grasp.KnownSpell && Natures_Grasp.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() > 2
            && ObjectManager.Target.IsCast && MySettings.UseNaturesGrasp && ObjectManager.Me.HealthPercent < 80)
        {
            Natures_Grasp.Launch();
            return;
        }
        else if (Typhoon.KnownSpell && Typhoon.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() > 2
            && ObjectManager.Target.GetDistance < 40 && ObjectManager.Me.HealthPercent < 70
            && MySettings.UseTyphoon)
        {
            Typhoon.Launch();
            return;
        }
        else if (Disorienting_Roar.KnownSpell && Disorienting_Roar.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() > 2
            && ObjectManager.Target.GetDistance < 10 && ObjectManager.Me.HealthPercent < 70
            && MySettings.UseDisorientingRoar)
        {
            Disorienting_Roar.Launch();
            OnCD = new Timer(1000*3);
            return;
        }
        else 
        {
            if (ObjectManager.Me.HealthPercent < 80 && War_Stomp.IsSpellUsable && War_Stomp.KnownSpell
                && MySettings.UseWarStomp)
            {
                War_Stomp.Launch();
                OnCD = new Timer(1000*2);
                return;
            }
        }
    }

    private void Decast()
    {
        if (ObjectManager.Target.IsCast && MySettings.UseSkullBash
            && ObjectManager.Target.IsTargetingMe
            && Skull_Bash.KnownSpell && Skull_Bash.IsSpellUsable && Skull_Bash.IsDistanceGood)
        {
            Skull_Bash.Launch();
            return;
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

public class Druid_Restoration
{
    [Serializable]
    public class DruidRestorationSettings : nManager.Helpful.Settings
    {
        /* Professions & Racials */
        public bool UseArcaneTorrent = true;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseLifeblood = true;
        public bool UseStoneform = true;
        public bool UseGiftoftheNaaru = true;
        public bool UseWarStomp = true;
        /* Druid Buffs */
        public bool UseDash = true;
        public bool UseFaerieFire = true;
        public bool UseMarkoftheWild = true;
        public bool UseStampedingRoar = true;
        /* Offensive Spell */
        public bool UseHurricane = true;
        public bool UseMoonfire = true;
        public bool UseWrath = true;
        /* Healing Cooldown */
        public bool UseForceofNature = true;
        public bool UseIncarnation = true;
        /* Defensive Cooldown */
        public bool UseBarkskin = true;
        public bool UseDisorientingRoar = true;
        public bool UseEntanglingRoots = true;
        public bool UseIronbark = true;
        public bool UseMassEntanglement = true;
        public bool UseMightyBash = true;
        public bool UseNaturesGrasp = true;
        public bool UseSolarBeam = true;
        public bool UseTyphoon = true;
        public bool UseUrsolsVortex = true;
        public bool UseWildCharge = true;
        /* Healing Spell */
        public bool UseCenarionWard = true;
        public bool UseHealingTouch = true;
        public bool UseInnervate = true;
        public bool UseLifebloom = true;
        public bool UseMightofUrsoc = true;
        public bool UseNaturesSwiftness = true;
        public bool UseNourish = false;
        public bool UseRegrowth = true;
        public bool UseRejuvenation = true;
        public bool UseRenewal = true;
        public bool UseSwiftmend = true;
        public bool UseTranquility = true;
        public bool UseWildGrowth = true;
        public bool UseWildMushroom = false;
        /* Game Settings */
        public bool UseLowCombat = true;
        public bool UseTrinket = true;
        public bool UseEngGlove = true;
        public bool UseAlchFlask = true;

        public DruidRestorationSettings()
        {
            ConfigWinForm(new System.Drawing.Point(400, 400), "Druid Restoration Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");
            AddControlInWinForm("Use Lifeblood", "UseLifeblood", "Professions & Racials");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            /* Druid Buffs */
            AddControlInWinForm("Use Dash", "UseDash", "Druid Buffs");
            AddControlInWinForm("Use Faerie Fire", "UseFaerieFire", "Druid Buffs");
            AddControlInWinForm("Use Mark of the Wild", "UseMarkoftheWild", "Druid Buffs");
            AddControlInWinForm("Use Stampeding Roar", "UseStampedingRoar", "Druid Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Hurricane", "UseHurricane", "Offensive Spell");
            AddControlInWinForm("Use Moonfire", "UseMoonfire", "Offensive Spell");
            AddControlInWinForm("Use Wrath", "UseWrath", "Offensive Spell");
            /* Healing Cooldown */
            AddControlInWinForm("Use Force of Nature", "UseForceofNature", "Offensive Cooldown");
            AddControlInWinForm("Use Incarnation", "UseIncarnation", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Barkskin", "UseBarkskin", "Defensive Cooldown");
            AddControlInWinForm("Use Disorienting Roar", "UseDisorientingRoar", "Defensive Cooldown");
            AddControlInWinForm("Use Entangling Roots", "UseEntanglingRoots", "Defensive Cooldown");
            AddControlInWinForm("Use Ironbark", "UseIronbark", "Defensive Cooldown");
            AddControlInWinForm("Use Mass Entanglement", "UseMassEntanglement", "Defensive Cooldown");
            AddControlInWinForm("Use Mighty Bash", "UseMightyBash", "Defensive Cooldown");
            AddControlInWinForm("Use Nature's Grasp", "UseNaturesGrasp", "Defensive Cooldown");
            AddControlInWinForm("Use Solar Beam", "UseSolarBeam", "Defensive Cooldown");
            AddControlInWinForm("Use Typhoon", "UseTyphoon", "Defensive Cooldown");
            AddControlInWinForm("Use Ursol's Vortex", "UseUrsolsVortex", "Defensive Cooldown");
            AddControlInWinForm("Use Wild Charge", "UseWildCharge", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Cenarion Ward", "UseCenarionWard", "Healing Spell");
            AddControlInWinForm("Use Healing Touch", "UseHealingTouch", "Healing Spell");
            AddControlInWinForm("Use Innervate", "UseInnervate", "Healing Spell");
            AddControlInWinForm("Use Lifebloom", "UseLifebloom", "Offensive Spell");
            AddControlInWinForm("Use Might of Ursoc", "UseMightofUrsoc", "Healing Spell");
            AddControlInWinForm("Use Nature's Swiftness", "UseNaturesSwiftness", "Healing Spell");
            AddControlInWinForm("Use Nourish", "UseNourish", "Offensive Spell");
            AddControlInWinForm("Use Regrowth", "UseRegrowth", "Offensive Spell");
            AddControlInWinForm("Use Rejuvenation", "UseRejuvenation", "Healing Spell");
            AddControlInWinForm("Use Renewal", "UseRenewal", "Healing Spell");
            AddControlInWinForm("Use Swiftmend", "UseSwiftmend", "Offensive Spell");
            AddControlInWinForm("Use Tranquility", "UseTranquility", "Healing Spell");
            AddControlInWinForm("Use Wild Growth", "UseWildGrowth", "Offensive Spell");
            AddControlInWinForm("Use WildMushroom", "UseWildMushroom", "Offensive Spell");
            /* Game Settings */
            AddControlInWinForm("Use Low Combat Settings", "UseLowCombat", "Game Settings");
            AddControlInWinForm("Use Trinket", "UseTrinket", "Game Settings");
            AddControlInWinForm("Use Engineering Gloves", "UseEngGlove", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
        }

        public static DruidRestorationSettings CurrentSetting { get; set; }

        public static DruidRestorationSettings GetSettings()
        {
            string CurrentSettingsFile = Application.StartupPath + "\\CustomClasses\\Settings\\Druid_Restoration.xml";
            if (System.IO.File.Exists(CurrentSettingsFile))
            {
                return
                    CurrentSetting = Settings.Load<Druid_Restoration.DruidRestorationSettings>(CurrentSettingsFile);
            }
            else
            {
                return new Druid_Restoration.DruidRestorationSettings();
            }
        }
    }

    private readonly DruidRestorationSettings MySettings = DruidRestorationSettings.GetSettings();

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

    #region Druid Buffs

    private readonly Spell Dash = new Spell("Dash");
    private readonly Spell Faerie_Fire = new Spell("Faerie Fire");
    private readonly Spell Mark_of_the_Wild = new Spell("Mark of the Wild");
    private readonly Spell Stampeding_Roar = new Spell("Stampeding Roar");

    #endregion

    #region Offensive Spell

    private readonly Spell Hurricane = new Spell("Hurricane");
    private readonly Spell Moonfire = new Spell("Moonfire");
    private Timer Moonfire_Timer = new Timer(0);
    private readonly Spell Wrath = new Spell("Wrath");

    #endregion

    #region Healing Cooldown

    private readonly Spell Force_of_Nature = new Spell("Force of Nature");
    private readonly Spell Incarnation = new Spell("Incarnation");

    #endregion

    #region Defensive Cooldown

    private readonly Spell Barkskin = new Spell("Barkskin");
    private readonly Spell Disorienting_Roar = new Spell("Disorienting Roar");
    private readonly Spell Entangling_Roots = new Spell("Entangling Roots");
    private readonly Spell Ironbark = new Spell("Ironbark");
    private readonly Spell Mass_Entanglement = new Spell("Mass Entanglement");
    private readonly Spell Mighty_Bash = new Spell("Mighty Bash");
    private readonly Spell Natures_Grasp = new Spell("Nature's Grasp");
    private readonly Spell Solar_Beam = new Spell("Solar Beam");
    private readonly Spell Typhoon = new Spell("Typhoon");
    private readonly Spell Ursols_Vortex = new Spell("Ursol's Vortex");
    private readonly Spell Wild_Charge = new Spell("Wild Charge");

    #endregion

    #region Healing Spell

    private readonly Spell Cenarion_Ward = new Spell("Cenarion Ward");
    private readonly Spell Healing_Touch = new Spell("Healing Touch");
    private Timer Healing_Touch_Timer = new Timer(0);
    private readonly Spell Innervate = new Spell("Innervate");
    private readonly Spell Lifebloom = new Spell("Lifebloom");
    private readonly Spell Might_of_Ursoc = new Spell("Might of Ursoc");
    private readonly Spell Natures_Swiftness = new Spell("Nature's Swiftness");
    private readonly Spell Nourish = new Spell("Nourish");
    private Timer Nourish_Timer = new Timer(0);
    private readonly Spell Regrowth = new Spell("Regrowth");
    private readonly Spell Rejuvenation = new Spell("Rejuvenation");
    private readonly Spell Renewal = new Spell("Renewal");
    private readonly Spell Swiftmend = new Spell("Swiftmend");
    private readonly Spell Tranquility = new Spell("Tranquility");
    private readonly Spell Wild_Growth = new Spell("Wild Growth");
    private readonly Spell Wild_Mushroom = new Spell("Wild Mushroom");
    private readonly Spell Wild_Mushroom_Bloom = new Spell("Wild Mushroom: Bloom");

    #endregion

    private Timer OnCD = new Timer(0);
    private Timer Trinket_Timer = new Timer(0);
    private Timer Engineering_Timer = new Timer(0);
    private Timer AlchFlask_Timer = new Timer(0);

    public Druid_Restoration()
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
                            (Moonfire.IsDistanceGood || Wrath.IsDistanceGood))
                        {
                            Pull();
                            lastTarget = ObjectManager.Me.Target;
                        }

                        Combat();
                    }
                    else
                        if (!ObjectManager.Me.IsCast)
                            Patrolling();
                }
            }
            catch
            {
            }
            Thread.Sleep(250);
        }
    }

    public void Pull()
    {
        if (Moonfire.KnownSpell && Moonfire.IsDistanceGood && Moonfire.IsSpellUsable
            && MySettings.UseMoonfire)
        {
            Moonfire.Launch();
            Moonfire_Timer = new Timer(1000*11);
            return;
        }
        else
        {
            if (Wrath.KnownSpell && Wrath.IsDistanceGood && Wrath.IsSpellUsable
                && MySettings.UseWrath)
            {
                Wrath.Launch();
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

    public void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (AlchFlask_Timer.IsReady && MySettings.UseAlchFlask && Alchemy.KnownSpell 
            && ItemsManager.GetItemCountByIdLUA(75525) == 1)
        {
            Logging.WriteFight("Use Alchi Flask");
            Lua.RunMacroText("/use item:75525");
            AlchFlask_Timer = new Timer(1000*60*60*2);
            return;
        }
        else if (Mark_of_the_Wild.KnownSpell && Mark_of_the_Wild.IsSpellUsable && !Mark_of_the_Wild.HaveBuff
            && MySettings.UseMarkoftheWild)
        {
            Mark_of_the_Wild.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() == 0 && MySettings.UseDash
            && Dash.KnownSpell && Dash.IsSpellUsable && !Dash.HaveBuff && !Stampeding_Roar.HaveBuff
            && ObjectManager.Me.GetMove)
        {
            Dash.Launch();
            return;
        }
        else
        {
            if (ObjectManager.GetNumberAttackPlayer() == 0 && MySettings.UseStampedingRoar
                && Stampeding_Roar.KnownSpell && Stampeding_Roar.IsSpellUsable && !Dash.HaveBuff 
                && !Stampeding_Roar.HaveBuff && ObjectManager.Me.GetMove)
            {
                Stampeding_Roar.Launch();
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
        Buff();
        Healing_Burst();
        DPS_Cycle();
    }

    public void Healing_Burst()
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
        else if (Force_of_Nature.IsSpellUsable && Force_of_Nature.KnownSpell && Force_of_Nature.IsDistanceGood
            && MySettings.UseForceofNature)
        {
            Force_of_Nature.Launch();
            return;
        }
        else
        {
            if (Incarnation.IsSpellUsable && Incarnation.KnownSpell && MySettings.UseIncarnation
                && ObjectManager.Target.GetDistance < 30)
            {
                Incarnation.Launch();
                return;
            }
        }
    }

    public void DPS_Cycle()
    {
        if (ObjectManager.Me.BarTwoPercentage < 80 && Arcane_Torrent.KnownSpell && Arcane_Torrent.IsSpellUsable
            && MySettings.UseArcaneTorrent)
        {
            Arcane_Torrent.Launch();
            return;
        }
        else if (Moonfire.KnownSpell && Moonfire.IsDistanceGood && Moonfire.IsSpellUsable
            && MySettings.UseMoonfire && (!Moonfire.TargetHaveBuff || Moonfire_Timer.IsReady))
        {
            Moonfire.Launch();
            Moonfire_Timer = new Timer(1000*11);
            return;
        }
        else if (Hurricane.KnownSpell && Hurricane.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() > 2 &&
            ObjectManager.Target.GetDistance < 30 && MySettings.UseHurricane)
        {
            SpellManager.CastSpellByIDAndPosition(16914, ObjectManager.Target.Position);
            return;
        }
        else
        {
            if (Wrath.KnownSpell && Wrath.IsSpellUsable && Wrath.IsDistanceGood
                && MySettings.UseWrath)
            {
                Wrath.Launch();
                return;
            }
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (ObjectManager.Me.HealthPercent < 80 && Natures_Swiftness.IsSpellUsable && Natures_Swiftness.KnownSpell
            && MySettings.UseNaturesSwiftness && MySettings.UseHealingTouch)
        {
            Natures_Swiftness.Launch();
            Thread.Sleep(400);
            Healing_Touch.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 70 && Renewal.IsSpellUsable && Renewal.KnownSpell
            && MySettings.UseRenewal)
        {
            Renewal.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 95 && !Fight.InFight && ObjectManager.GetNumberAttackPlayer() == 0
            && Healing_Touch.IsSpellUsable && Healing_Touch.KnownSpell && MySettings.UseHealingTouch)
        {
            Healing_Touch.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 90 && Cenarion_Ward.IsSpellUsable && Cenarion_Ward.KnownSpell
            && MySettings.UseCenarionWard)
        {
            Cenarion_Ward.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && Gift_of_the_Naaru.IsSpellUsable && Gift_of_the_Naaru.KnownSpell
            && MySettings.UseGiftoftheNaaru)
        {
            Gift_of_the_Naaru.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 90 && Rejuvenation.IsSpellUsable && Rejuvenation.KnownSpell
            && !Rejuvenation.HaveBuff && MySettings.UseRejuvenation)
        {
            Rejuvenation.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 60 && Regrowth.IsSpellUsable && Regrowth.KnownSpell
            && !Regrowth.HaveBuff && MySettings.UseRegrowth)
        {
            Regrowth.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && Swiftmend.IsSpellUsable && Swiftmend.KnownSpell
            && MySettings.UseSwiftmend)
        {
            Swiftmend.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 50 && Wild_Growth.IsSpellUsable && Wild_Growth.KnownSpell
            && MySettings.UseWildGrowth)
        {
            Wild_Growth.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 40 && Healing_Touch.IsSpellUsable && Healing_Touch.KnownSpell
            && Healing_Touch_Timer.IsReady && MySettings.UseHealingTouch)
        {
            Healing_Touch.Launch();
            Healing_Touch_Timer = new Timer(1000*15);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 40 && Nourish.IsSpellUsable && Nourish.KnownSpell
            && Nourish_Timer.IsReady && MySettings.UseNourish && !MySettings.UseHealingTouch)
        {
            Nourish.Launch();
            Nourish_Timer = new Timer(1000*15);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 35 && Might_of_Ursoc.IsSpellUsable && Might_of_Ursoc.KnownSpell
            && MySettings.UseMightofUrsoc)
        {
            Might_of_Ursoc.Launch();
            return;
        }
        else if (Wild_Mushroom.KnownSpell && Wild_Mushroom.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() > 3
            && Wild_Mushroom_Bloom.KnownSpell && Wild_Mushroom.IsSpellUsable && MySettings.UseWildMushroom
            && ObjectManager.Me.HealthPercent < 80)
        {
            for (int i = 0; i < 3; i++)
            {
                SpellManager.CastSpellByIDAndPosition(88747, ObjectManager.Target.Position);
                Thread.Sleep(200);
            }

            Wild_Mushroom_Bloom.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Me.HealthPercent < 30 && Tranquility.IsSpellUsable && Tranquility.KnownSpell
            && MySettings.UseTranquility)
            {
                Tranquility.Launch();
                while (ObjectManager.Me.IsCast)
                {
                    Thread.Sleep(100);
                    Thread.Sleep(100);
                }
                return;
            }
        }

        if (ObjectManager.Me.ManaPercentage < 50 && MySettings.UseInnervate)
        {
            Innervate.Launch();
            return;
        }
    }

    public void Defense_Cycle()
    {
        if (ObjectManager.Me.HealthPercent < 80 && Stoneform.IsSpellUsable && Stoneform.KnownSpell
            && MySettings.UseStoneform)
        {
            Stoneform.Launch();
            OnCD = new Timer(1000*8);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && Barkskin.KnownSpell && Barkskin.IsSpellUsable
            && MySettings.UseBarkskin)
        {
            Barkskin.Launch();
            OnCD = new Timer(1000*12);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && Ironbark.KnownSpell && Ironbark.IsSpellUsable
            && MySettings.UseIronbark)
        {
            Ironbark.Launch();
            OnCD = new Timer(1000*12);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && Mighty_Bash.KnownSpell && Mighty_Bash.IsSpellUsable
            && MySettings.UseMightyBash && Mighty_Bash.IsDistanceGood)
        {
            Mighty_Bash.Launch();
            OnCD = new Timer(1000*5);
            return;
        }
        else if (Mass_Entanglement.KnownSpell && Mass_Entanglement.IsSpellUsable && Mass_Entanglement.IsDistanceGood
            && MySettings.UseMassEntanglement && ObjectManager.Me.HealthPercent < 80)
        {
            Mass_Entanglement.Launch();

            if (Wild_Charge.KnownSpell && Wild_Charge.IsDistanceGood && Wild_Charge.IsSpellUsable
                && MySettings.UseWildCharge)
            {
                Thread.Sleep(200);
                Wild_Charge.Launch();
            }
            return;
        }
        else if (Ursols_Vortex.KnownSpell && Ursols_Vortex.IsSpellUsable && Ursols_Vortex.IsDistanceGood
            && MySettings.UseUrsolsVortex && ObjectManager.Me.HealthPercent < 80)
        {
            Ursols_Vortex.Launch();

            if (Wild_Charge.KnownSpell && Wild_Charge.IsDistanceGood && Wild_Charge.IsSpellUsable
                && MySettings.UseWildCharge)
            {
                Thread.Sleep(200);
                Wild_Charge.Launch();
            }
            return;
        }
        else if (Natures_Grasp.KnownSpell && Natures_Grasp.IsSpellUsable
            && ObjectManager.Target.IsCast && MySettings.UseNaturesGrasp && ObjectManager.Me.HealthPercent < 80)
        {
            Natures_Grasp.Launch();

            if (Wild_Charge.KnownSpell && Wild_Charge.IsDistanceGood && Wild_Charge.IsSpellUsable
                && MySettings.UseWildCharge)
            {
                Thread.Sleep(200);
                Wild_Charge.Launch();
            }
            return;
        }
        else if (Typhoon.KnownSpell && Typhoon.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() > 2
            && ObjectManager.Target.GetDistance < 40 && ObjectManager.Me.HealthPercent < 70
            && MySettings.UseTyphoon)
        {
            Typhoon.Launch();
            return;
        }
        else if (Disorienting_Roar.KnownSpell && Disorienting_Roar.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() > 2
            && ObjectManager.Target.GetDistance < 10 && ObjectManager.Me.HealthPercent < 70
            && MySettings.UseDisorientingRoar)
        {
            Disorienting_Roar.Launch();
            OnCD = new Timer(1000*3);
            return;
        }
        else
        {
            if (ObjectManager.Me.HealthPercent < 80 && War_Stomp.IsSpellUsable && War_Stomp.KnownSpell
                && MySettings.UseWarStomp)
            {
                War_Stomp.Launch();
                OnCD = new Timer(1000*2);
                return;
            }
        }
    }

    private void AvoidMelee()
    {
        if (ObjectManager.Target.GetDistance < 1)
        {
            Keyboard.DownKey(nManager.Wow.Memory.WowProcess.MainWindowHandle, "{DOWN}");
        }
    }
}

public class Druid_Guardian
{
    [Serializable]
    public class DruidGuardianSettings : nManager.Helpful.Settings
    {
        /* Professions & Racials */
        public bool UseArcaneTorrent = true;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseLifeblood = true;
        public bool UseStoneform = true;
        public bool UseGiftoftheNaaru = true;
        public bool UseWarStomp = true;
        /* Druid Buffs */
        public bool UseBearForm = true;
        public bool UseDash = true;
        public bool UseFaerieFire = true;
        public bool UseMarkoftheWild = true;
        public bool UseStampedingRoar = true;
        /* Offensive Spell */
        public bool UseGrowl = true;
        public bool UseLacerate = true;
        public bool UseMangle = true;
        public bool UseMaul = true;
        public bool UseSwipe = true;
        public bool UseThrash = true;
        /* Offensive Cooldown */
        public bool UseBerserk = true;
        public bool UseEnrage = true;
        public bool UseForceofNature = true;
        public bool UseHeartoftheWild = true;
        public bool UseIncarnation = true;
        public bool UseNaturesVigil = true;
        /* Defensive Cooldown */
        public bool UseBarkskin = true;
        public bool UseBearHug = true;
        public bool UseDisorientingRoar = true;
        public bool UseMassEntanglement = true;
        public bool UseMightyBash = true;
        public bool UseNaturesGrasp = true;
        public bool UseSavageDefense = true;
        public bool UseSkullBash = true;
        public bool UseSurvivalInstincts = true;
        public bool UseTyphoon = true;
        public bool UseUrsolsVortex = true;
        public bool UseWildCharge = true;
        /* Healing Spell */
        public bool UseCenarionWard = true;
        public bool UseFrenziedRegeneration = true;
        public bool UseHealingTouch = true;
        public bool UseInnervate = true;
        public bool UseMightofUrsoc = true;
        public bool UseNaturesSwiftness = true;
        public bool UseRejuvenation = true;
        public bool UseRenewal = true;
        public bool UseTranquility = true;
        /* Game Settings */
        public bool UseLowCombat = true;
        public bool UseTrinket = true;
        public bool UseEngGlove = true;
        public bool UseAlchFlask = true;

        public DruidGuardianSettings()
        {
            ConfigWinForm(new System.Drawing.Point(400, 400), "Druid Guardian Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");
            AddControlInWinForm("Use Lifeblood", "UseLifeblood", "Professions & Racials");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            /* Druid Buffs */
            AddControlInWinForm("Use Bear Form", "UseBearForm", "Druid Buffs");
            AddControlInWinForm("Use Dash", "UseDash", "Druid Buffs");
            AddControlInWinForm("Use Faerie Fire", "UseFaerieFire", "Druid Buffs");
            AddControlInWinForm("Use Mark of the Wild", "UseMarkoftheWild", "Druid Buffs");
            AddControlInWinForm("Use Stampeding Roar", "UseStampedingRoar", "Druid Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Growl", "UseGrowl", "Offensive Spell");
            AddControlInWinForm("Use Lacerate", "UseLacerate", "Offensive Spell");
            AddControlInWinForm("Use Mangle", "UseMangle", "Offensive Spell");
            AddControlInWinForm("Use Maul", "UseMaul", "Offensive Spell");
            AddControlInWinForm("Use Swipe", "UseSwipe", "Offensive Spell");
            AddControlInWinForm("Use Thrash", "UseThrash", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use Berserk", "UseBerserk", "Offensive Cooldown");
            AddControlInWinForm("Use Enrage", "UseEnrage", "Offensive Cooldown");
            AddControlInWinForm("Use Force of Nature", "UseForceofNature", "Offensive Cooldown");
            AddControlInWinForm("Use Heart of the Wild", "UseHeartoftheWild", "Offensive Cooldown");
            AddControlInWinForm("Use Incarnation", "UseIncarnation", "Offensive Cooldown");
            AddControlInWinForm("Use Nature's Vigil", "UseNaturesVigil", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Bear Hug", "UseBearHug", "Defensive Cooldown");
            AddControlInWinForm("Use Barkskin", "UseBarkskin", "Defensive Cooldown");
            AddControlInWinForm("Use Disorienting Roar", "UseDisorientingRoar", "Defensive Cooldown");
            AddControlInWinForm("Use Mass Entanglement", "UseMassEntanglement", "Defensive Cooldown");
            AddControlInWinForm("Use Mighty Bash", "UseMightyBash", "Defensive Cooldown");
            AddControlInWinForm("Use Nature's Grasp", "UseNaturesGrasp", "Defensive Cooldown");
            AddControlInWinForm("Use Savage Defense", "UseSavageDefense", "Defensive Cooldown");
            AddControlInWinForm("Use Skull Bash", "UseSkullBash", "Defensive Cooldown");
            AddControlInWinForm("Use Survival Instincts", "UseSurvivalInstincts", "Defensive Cooldown");
            AddControlInWinForm("Use Typhoon", "UseTyphoon", "Defensive Cooldown");
            AddControlInWinForm("Use Ursol's Vortex", "UseUrsolsVortex", "Defensive Cooldown");
            AddControlInWinForm("Use Wild Charge", "UseWildCharge", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Cenarion Ward", "UseCenarionWard", "Healing Spell");
            AddControlInWinForm("Use Frenzied Regeneration", "UseFrenziedRegeneration", "Healing Spell");
            AddControlInWinForm("Use Healing Touch", "UseHealingTouch", "Healing Spell");
            AddControlInWinForm("Use Innervate", "UseInnervate", "Healing Spell");
            AddControlInWinForm("Use Might of Ursoc", "UseMightofUrsoc", "Healing Spell");
            AddControlInWinForm("Use Nature's Swiftness", "UseNaturesSwiftness", "Healing Spell");
            AddControlInWinForm("Use Rejuvenation", "UseRejuvenation", "Healing Spell");
            AddControlInWinForm("Use Renewal", "UseRenewal", "Healing Spell");
            AddControlInWinForm("Use Tranquility", "UseTranquility", "Healing Spell");
            /* Game Settings */
            AddControlInWinForm("Using Glyph of Shred?", "UseGlyphofShred", "Game Settings");
            AddControlInWinForm("Use Low Combat Settings", "UseLowCombat", "Game Settings");
            AddControlInWinForm("Use Trinket", "UseTrinket", "Game Settings");
            AddControlInWinForm("Use Engineering Gloves", "UseEngGlove", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
        }

        public static DruidGuardianSettings CurrentSetting { get; set; }

        public static DruidGuardianSettings GetSettings()
        {
            string CurrentSettingsFile = Application.StartupPath + "\\CustomClasses\\Settings\\Druid_Guardian.xml";
            if (System.IO.File.Exists(CurrentSettingsFile))
            {
                return
                    CurrentSetting = Settings.Load<Druid_Guardian.DruidGuardianSettings>(CurrentSettingsFile);
            }
            else
            {
                return new Druid_Guardian.DruidGuardianSettings();
            }
        }
    }

    private readonly DruidGuardianSettings MySettings = DruidGuardianSettings.GetSettings();

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

    #region Druid Buffs

    private readonly Spell Bear_Form = new Spell("Bear Form");
    private readonly Spell Dash = new Spell("Dash");
    private readonly Spell Faerie_Fire = new Spell("Faerie Fire");
    private readonly Spell Mark_of_the_Wild = new Spell("Mark of the Wild");
    private readonly Spell Stampeding_Roar = new Spell("Stampeding Roar");

    #endregion

    #region Offensive Spell

    private readonly Spell Growl = new Spell("Growl");
    private readonly Spell Lacerate = new Spell("Lacerate");
    private readonly Spell Maul = new Spell("Maul");
    private readonly Spell Mangle = new Spell("Mangle");
    private readonly Spell Swipe = new Spell("Swipe");
    private readonly Spell Thrash = new Spell("Thrash");

    #endregion

    #region Offensive Cooldown

    private readonly Spell Berserk = new Spell("Berserk");
    private readonly Spell Enrage = new Spell("Enrage");
    private readonly Spell Force_of_Nature = new Spell("Force of Nature");
    private readonly Spell Heart_of_the_Wild = new Spell("Heart of the Wild");
    private readonly Spell Incarnation = new Spell("Incarnation");
    private readonly Spell Natures_Vigil = new Spell("Nature's Vigil");

    #endregion

    #region Defensive Cooldown

    private readonly Spell Bear_Hug = new Spell("Bear Hug");
    private readonly Spell Barkskin = new Spell("Barkskin");
    private readonly Spell Disorienting_Roar = new Spell("Disorienting Roar");
    private readonly Spell Mass_Entanglement = new Spell("Mass Entanglement");
    private readonly Spell Mighty_Bash = new Spell("Mighty Bash");
    private readonly Spell Natures_Grasp = new Spell("Nature's Grasp");
    private readonly Spell Savage_Defense = new Spell("Savage Defense");
    private readonly Spell Skull_Bash = new Spell("Skull Bash");
    private readonly Spell Survival_Instincts = new Spell("Survival Instincts");
    private readonly Spell Typhoon = new Spell("Typhoon");
    private readonly Spell Ursols_Vortex = new Spell("Ursol's Vortex");
    private readonly Spell Wild_Charge = new Spell("Wild Charge");

    #endregion

    #region Healing Spell

    private readonly Spell Cenarion_Ward = new Spell("Cenarion Ward");
    private readonly Spell Frenzied_Regeneration = new Spell("Frenzied_Regeneration");
    private readonly Spell Healing_Touch = new Spell("Healing Touch");
    private Timer Healing_Touch_Timer = new Timer(0);
    private readonly Spell Innervate = new Spell("Innervate");
    private readonly Spell Might_of_Ursoc = new Spell("Might of Ursoc");
    private readonly Spell Natures_Swiftness = new Spell("Nature's Swiftness");
    private readonly Spell Rejuvenation = new Spell("Rejuvenation");
    private readonly Spell Renewal = new Spell("Renewal");
    private readonly Spell Tranquility = new Spell("Tranquility");

    #endregion

    private Timer OnCD = new Timer(0);
    private Timer Trinket_Timer = new Timer(0);
    private Timer Engineering_Timer = new Timer(0);
    private Timer AlchFlask_Timer = new Timer(0);
    int LC = 0;

    public Druid_Guardian()
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
                            (Faerie_Fire.IsDistanceGood || Wild_Charge.IsDistanceGood))
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

    public void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            Heal();
            Buff();
        }
    }

    public void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (Mark_of_the_Wild.KnownSpell && Mark_of_the_Wild.IsSpellUsable && !Mark_of_the_Wild.HaveBuff)
        {
            Mark_of_the_Wild.Launch();
            return;
        }
        else if (AlchFlask_Timer.IsReady && MySettings.UseAlchFlask && Alchemy.KnownSpell
            && ItemsManager.GetItemCountByIdLUA(75525) == 1)
        {
            Logging.WriteFight("Use Alchi Flask");
            Lua.RunMacroText("/use item:75525");
            AlchFlask_Timer = new Timer(1000*60*60*2);
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() == 0 && MySettings.UseDash
            && Dash.KnownSpell && Dash.IsSpellUsable && !Dash.HaveBuff && !Stampeding_Roar.HaveBuff
            && ObjectManager.Me.GetMove)
        {
            Dash.Launch();
            return;
        }
        else
        {
            if (ObjectManager.GetNumberAttackPlayer() == 0 && MySettings.UseStampedingRoar
                && Stampeding_Roar.KnownSpell && Stampeding_Roar.IsSpellUsable && !Dash.HaveBuff 
                && !Stampeding_Roar.HaveBuff && ObjectManager.Me.GetMove)
            {
                Stampeding_Roar.Launch();
                return;
            }
        }
    }

    public void Pull()
    {
        if (!ObjectManager.Me.HaveBuff(5487) && MySettings.UseBearForm)
            Bear_Form.Launch();

        if (Wild_Charge.KnownSpell && Wild_Charge.IsSpellUsable && Wild_Charge.IsDistanceGood
            && MySettings.UseWildCharge)
        {
            Wild_Charge.Launch();
            return;
        }
        else
        {
            if (Faerie_Fire.KnownSpell && Faerie_Fire.IsSpellUsable && Faerie_Fire.IsDistanceGood
                && MySettings.UseFaerieFire)
            {
                Faerie_Fire.Launch();
                return;
            }
        }
    }

    public void LowCombat()
    {
        AvoidMelee();
        Heal();
        Defense_Cycle();
        Buff();

        if (!ObjectManager.Me.HaveBuff(5487) && MySettings.UseBearForm)
        {
            Bear_Form.Launch();
            return;
        }

        if (Mangle.IsSpellUsable && Mangle.KnownSpell && Mangle.IsDistanceGood
            && MySettings.UseMangle)
        {
            Mangle.Launch();
            if (ObjectManager.Target.HealthPercent < 50 && ObjectManager.Target.HealthPercent > 0)
            {
                Mangle.Launch();
                return;
            }
        }
        else if (Maul.IsSpellUsable && Maul.KnownSpell && Maul.IsDistanceGood
            && MySettings.UseMaul)
        {
            Maul.Launch();
            return;
        }
        else
        {
            if (Swipe.IsSpellUsable && Swipe.KnownSpell && Swipe.IsDistanceGood
            && MySettings.UseSwipe)
            {
                Swipe.Launch();
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
        else if (Enrage.IsSpellUsable && Enrage.KnownSpell && Enrage.IsDistanceGood
            && MySettings.UseEnrage && ObjectManager.Me.RagePercentage < 70)
        {
            Enrage.Launch();
            return;
        }
        else if (Force_of_Nature.IsSpellUsable && Force_of_Nature.KnownSpell && Force_of_Nature.IsDistanceGood
            && MySettings.UseForceofNature)
        {
            Force_of_Nature.Launch();
            return;
        }
        else if (Incarnation.IsSpellUsable && Incarnation.KnownSpell && MySettings.UseIncarnation
            && ObjectManager.Target.GetDistance < 30)
        {
            Incarnation.Launch();
            return;
        }
        else if (Heart_of_the_Wild.IsSpellUsable && Heart_of_the_Wild.KnownSpell && MySettings.UseHeartoftheWild
            && ObjectManager.Target.GetDistance < 30)
        {
            Heart_of_the_Wild.Launch();
            return;
        }
        else if (Natures_Vigil.IsSpellUsable && Natures_Vigil.KnownSpell && MySettings.UseNaturesVigil
            && ObjectManager.Target.GetDistance < 30)
        {
            Natures_Vigil.Launch();
            return;
        }
        else
        {
            if (Berserk.KnownSpell && Berserk.IsSpellUsable && MySettings.UseBerserk
                && ObjectManager.Target.GetDistance < 30)
            {
                Berserk.Launch();
                return;
            }
        }
    }

    public void DPS_Cycle()
    {
        if (!ObjectManager.Me.HaveBuff(5487) && MySettings.UseBearForm)
        {
            Bear_Form.Launch();
            return;
        }

        if (Faerie_Fire.KnownSpell && Faerie_Fire.IsSpellUsable && Faerie_Fire.IsDistanceGood 
            && MySettings.UseFaerieFire && (!Faerie_Fire.TargetHaveBuff || !ObjectManager.Target.HaveBuff(113746)))
        {
            Faerie_Fire.Launch();
            return;
        }
        else if (Growl.KnownSpell && Growl.IsSpellUsable && Growl.IsDistanceGood 
            && MySettings.UseGrowl && !ObjectManager.Target.InCombat)
        {
            Growl.Launch();
            return;
        }
        else if (Mangle.KnownSpell && Mangle.IsSpellUsable && Mangle.IsDistanceGood
            && MySettings.UseMangle)
        {
            Mangle.Launch();
            return;
        }
        else if (Thrash.IsSpellUsable && Thrash.KnownSpell
            && Thrash.IsDistanceGood && !Thrash.TargetHaveBuff && MySettings.UseThrash)
        {
            Thrash.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 1 && Swipe.IsSpellUsable && Swipe.KnownSpell
            && Swipe.IsDistanceGood && MySettings.UseSwipe)
        {
            Swipe.Launch();
            return;
        }
        else if (Lacerate.KnownSpell && Lacerate.IsSpellUsable && Lacerate.IsDistanceGood
            && MySettings.UseLacerate)
        {
            Lacerate.Launch();
            return;
        }
        else
        {
            if (Maul.KnownSpell && Maul.IsSpellUsable && Maul.IsDistanceGood
                && MySettings.UseMaul && ObjectManager.Me.RagePercentage > 90
                && ObjectManager.Me.HealthPercent > 90)
            {
                Maul.Launch();
                return;
            }
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (ObjectManager.Me.HealthPercent < 80 && Natures_Swiftness.IsSpellUsable && Natures_Swiftness.KnownSpell
            && MySettings.UseNaturesSwiftness && MySettings.UseHealingTouch)
        {
            Natures_Swiftness.Launch();
            Thread.Sleep(400);
            Healing_Touch.Launch();
            return;
        }
        if (ObjectManager.Me.HealthPercent < 75 && Frenzied_Regeneration.IsSpellUsable && Frenzied_Regeneration.KnownSpell
            && MySettings.UseFrenziedRegeneration)
        {
            Frenzied_Regeneration.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 70 && Renewal.IsSpellUsable && Renewal.KnownSpell
            && MySettings.UseRenewal)
        {
            Renewal.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 90 && Cenarion_Ward.IsSpellUsable && Cenarion_Ward.KnownSpell
            && MySettings.UseCenarionWard)
        {
            Cenarion_Ward.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 50 && !Fight.InFight && ObjectManager.GetNumberAttackPlayer() == 0
            && Healing_Touch.IsSpellUsable && Healing_Touch.KnownSpell && MySettings.UseHealingTouch)
        {
            while (ObjectManager.Me.HealthPercent < 95 && Healing_Touch.IsSpellUsable)
            {
                Healing_Touch.Launch();
                Thread.Sleep(1500);
            }
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && Gift_of_the_Naaru.IsSpellUsable && Gift_of_the_Naaru.KnownSpell)
        {
            Gift_of_the_Naaru.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 50 && Rejuvenation.IsSpellUsable && Rejuvenation.KnownSpell
            && !Rejuvenation.HaveBuff && MySettings.UseRejuvenation)
        {
            Rejuvenation.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 40 && Healing_Touch.IsSpellUsable && Healing_Touch.KnownSpell
            && Healing_Touch_Timer.IsReady && MySettings.UseHealingTouch)
        {
            Healing_Touch.Launch();
            Healing_Touch_Timer = new Timer(1000*15);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 35 && Might_of_Ursoc.IsSpellUsable && Might_of_Ursoc.KnownSpell
            && MySettings.UseMightofUrsoc)
        {
            Might_of_Ursoc.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 30 && Tranquility.IsSpellUsable && Tranquility.KnownSpell
            && MySettings.UseTranquility)
        {
            Tranquility.Launch();
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(100);
                Thread.Sleep(100);
            }
            return;
        }
        else
        {
            if (ObjectManager.Me.ManaPercentage < 10 && MySettings.UseInnervate)
            {
                Innervate.Launch();
                return;
            }
        }
    }

    private void Defense_Cycle()
    {
        if (ObjectManager.Me.HealthPercent < 80 && MySettings.UseBarkskin
            && Barkskin.KnownSpell && Barkskin.IsSpellUsable)
        {
            Barkskin.Launch();
            OnCD = new Timer(1000*12);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 90 && MySettings.UseSavageDefense
            && Savage_Defense.KnownSpell && Savage_Defense.IsSpellUsable)
        {
            Savage_Defense.Launch();
            OnCD = new Timer(1000*6);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 90 && Mighty_Bash.IsDistanceGood
            && Mighty_Bash.KnownSpell && Mighty_Bash.IsSpellUsable && MySettings.UseMightyBash)
        {
            Mighty_Bash.Launch();
            OnCD = new Timer(1000*5);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 70 && Bear_Hug.KnownSpell && Bear_Hug.IsSpellUsable 
            && MySettings.UseBearHug && Bear_Hug.IsDistanceGood)
        {
            Bear_Hug.Launch();
            OnCD = new Timer(1000*3);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 70 && MySettings.UseSurvivalInstincts
            && Survival_Instincts.KnownSpell && Survival_Instincts.IsSpellUsable)
        {
            Survival_Instincts.Launch();
            OnCD = new Timer(1000*12);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && Stoneform.IsSpellUsable && Stoneform.KnownSpell
            && MySettings.UseStoneform)
        {
            Stoneform.Launch();
            OnCD = new Timer(1000*8);
            return;
        }
        else if (Mass_Entanglement.KnownSpell && Mass_Entanglement.IsSpellUsable && Mass_Entanglement.IsDistanceGood
            && ObjectManager.Target.IsCast && MySettings.UseMassEntanglement && ObjectManager.GetNumberAttackPlayer() > 2
            && ObjectManager.Me.HealthPercent < 70)
        {
            if (Typhoon.KnownSpell && Typhoon.IsSpellUsable && MySettings.UseTyphoon 
                && ObjectManager.Target.GetDistance < 40 && MySettings.UseTyphoon)
            {
                Typhoon.Launch();
                Thread.Sleep(200);
            }

            Mass_Entanglement.Launch();
            return;
        }
        else if (Ursols_Vortex.KnownSpell && Ursols_Vortex.IsSpellUsable && Ursols_Vortex.IsDistanceGood
            && MySettings.UseUrsolsVortex && ObjectManager.Me.HealthPercent < 80 
            && ObjectManager.GetNumberAttackPlayer() > 2)
        {
            Ursols_Vortex.Launch();

            if (Wild_Charge.KnownSpell && Wild_Charge.IsDistanceGood && Wild_Charge.IsSpellUsable
                && MySettings.UseWildCharge)
            {
                Thread.Sleep(200);
                Wild_Charge.Launch();
            }
            return;
        }
        else if (Natures_Grasp.KnownSpell && Natures_Grasp.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() > 2
            && ObjectManager.Target.IsCast && MySettings.UseNaturesGrasp && ObjectManager.Me.HealthPercent < 80)
        {
            Natures_Grasp.Launch();
            return;
        }
        else if (Typhoon.KnownSpell && Typhoon.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() > 2
            && ObjectManager.Target.GetDistance < 40 && ObjectManager.Me.HealthPercent < 70
            && MySettings.UseTyphoon)
        {
            Typhoon.Launch();
            return;
        }
        else if (Disorienting_Roar.KnownSpell && Disorienting_Roar.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() > 2
            && ObjectManager.Target.GetDistance < 10 && ObjectManager.Me.HealthPercent < 70
            && MySettings.UseDisorientingRoar)
        {
            Disorienting_Roar.Launch();
            OnCD = new Timer(1000*3);
            return;
        }
        else
        {
            if (ObjectManager.Me.HealthPercent < 80 && War_Stomp.IsSpellUsable && War_Stomp.KnownSpell
                && MySettings.UseWarStomp)
            {
                War_Stomp.Launch();
                OnCD = new Timer(1000*2);
                return;
            }
        }
    }

    private void Decast()
    {
        if (ObjectManager.Target.IsCast && MySettings.UseSkullBash
            && ObjectManager.Target.IsTargetingMe
            && Skull_Bash.KnownSpell && Skull_Bash.IsSpellUsable && Skull_Bash.IsDistanceGood)
        {
            Skull_Bash.Launch();
            return;
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
