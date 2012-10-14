/*
* CustomClass for TheNoobBot
* Credit : Rival, Geesus, Enelya, Marstor, Vesper, Neo2003, Dreadlocks
* Thanks you !
* 10/13/2012
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
    internal static float range = 3.5f;
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
                    #region Priest Specialisation checking

                case WoWClass.Priest:
                    var Mind_Flay = new Spell("Mind Flay");
                    if (Mind_Flay.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Shadow_Priest.xml";
                            Shadow_Priest.ShadowPriestSettings CurrentSetting;
                            CurrentSetting = new Shadow_Priest.ShadowPriestSettings();
                            if (System.IO.File.Exists(CurrentSettingsFile))
                            {
                                CurrentSetting =
                                    Settings.Load<Shadow_Priest.ShadowPriestSettings>(CurrentSettingsFile);
                            }
                            CurrentSetting.ToForm();
                            CurrentSetting.Save(CurrentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Shadow Priest Found");
                            new Shadow_Priest();
                        }
                    }
                    if (!Mind_Flay.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Shadow_Priest.xml";
                            Shadow_Priest.ShadowPriestSettings CurrentSetting;
                            CurrentSetting = new Shadow_Priest.ShadowPriestSettings();
                            if (System.IO.File.Exists(CurrentSettingsFile))
                            {
                                CurrentSetting =
                                    Settings.Load<Shadow_Priest.ShadowPriestSettings>(CurrentSettingsFile);
                            }
                            CurrentSetting.ToForm();
                            CurrentSetting.Save(CurrentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Priest without Spec");
                            new Shadow_Priest();
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

#region Priest

public class Shadow_Priest
{
    [Serializable]
    public class ShadowPriestSettings : nManager.Helpful.Settings
    {
        /* Professions & Racials */
        public bool UseArcaneTorrent = true;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseLifeblood = true;
        public bool UseStoneform = true;
        public bool UseGiftoftheNaaru = true;
        public bool UseWarStomp = true;
        /* Priest Buffs */
        public bool UseInnerFire = true;
        public bool UseInnerWill = false;
        public bool UseLevitate = false;
        public bool UsePowerWordFortitude = true;
        public bool UseShadowform = true;
        /* Offensive Spell */
        public bool UseDevouringPlague = true;
        public bool UseMindBlast = true;
        public bool UseMindFlay = true;
        public bool UseMindSear = true;
        public bool UseMindSpike = true;
        public bool UseShadowWordDeath = true;
        public bool UseShadowWordInsanity = true;
        public bool UseShadowWordPain = true;
        public bool UseVampiricTouch = true;
        /* Offensive Cooldown */
        public bool UsePowerInfusion = true;
        public bool UseShadowfiend = true;
        /* Defensive Cooldown */
        public bool UseDispersion = true;
        public bool UsePowerWordShield = true;
        public bool UsePsychicHorror = true;
        public bool UsePsychicScream = true;
        public bool UsePsyfiend = true;
        public bool UseSilence = true;
        public bool UseSpectralGuise = true;
        public bool UseVoidTendrils = true;
        /* Healing Spell */
        public bool UseDesperatePrayer = true;
        public bool UseFlashHeal = true;
        public bool UseHymnofHope = true;
        public bool UsePrayerofMending = true;
        public bool UseRenew = true;
        public bool UseVampiricEmbrace = true;
        /* Game Settings */
        public bool UseLowCombat = true;
        public bool UseTrinket = true;
        public bool UseEngGlove = true;
        public bool UseAlchFlask = true;

        public ShadowPriestSettings()
        {
            ConfigWinForm(new System.Drawing.Point(400, 400), "Shadow Priest Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");
            AddControlInWinForm("Use Lifeblood", "UseLifeblood", "Professions & Racials");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            /* Priest Buffs */
            AddControlInWinForm("Use Inner Fire", "UseInnerFire", "Priest Buffs");
            AddControlInWinForm("Use Inner Will", "UseInnerWill", "Priest Buffs");
            AddControlInWinForm("Use Levitate", "UseLevitate", "Priest Buffs");
            AddControlInWinForm("Use Power Word: Fortitude", "UsePowerWordFortitude", "Priest Buffs");
            AddControlInWinForm("Use Shadowform", "UseShadowform", "Priest Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Devouring Plague", "UseDevoringPlague", "Offensive Spell");
            AddControlInWinForm("Use Mind Blast", "UseMindBlast", "Offensive Spell");
            AddControlInWinForm("Use Mind Flay", "UseMindFlay", "Offensive Spell");
            AddControlInWinForm("Use Mind Sear", "UseMindSear", "Offensive Spell");
            AddControlInWinForm("Use Mind Spike", "UseMindSpike", "Offensive Spell");
            AddControlInWinForm("Use Shadow Word: Death", "UseShadowWordDeath", "Offensive Spell");
            AddControlInWinForm("Use Shadow Word: Insanity", "UseShadowWordInsanity", "Offensive Spell");
            AddControlInWinForm("Use Shadow Word: Pain", "UseShadowWordPain", "Offensive Spell");
            AddControlInWinForm("Use Vampiric Touch", "UseVampiricTouch", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use Power Infusion", "UsePowerInfusion", "Offensive Cooldown");
            AddControlInWinForm("Use Shadowfiend", "UseShadowfiend", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Dispersion", "UseDispersion", "Defensive Cooldown");
            AddControlInWinForm("Use Power Word: Shield", "UsePowerWordShield", "Defensive Cooldown");
            AddControlInWinForm("Use Psychic Horror", "UsePsychicHorror", "Defensive Cooldown");
            AddControlInWinForm("Use Psychic Scream", "UsePsychicScream", "Defensive Cooldown");
            AddControlInWinForm("Use Psyfiend", "UsePsyfiend", "Defensive Cooldown");
            AddControlInWinForm("Use Silence", "UseSilence", "Defensive Cooldown");
            AddControlInWinForm("Use Spectral Guise", "UseSpectralGuise", "Defensive Cooldown");
            AddControlInWinForm("Use Void Tendrils", "UseVoidTendrils", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Desperate Prayer", "UseDesperatePrayer", "Healing Spell");
            AddControlInWinForm("Use Flash Heal", "UseFlashHeal", "Healing Spell");
            AddControlInWinForm("Use Hymn of Hope", "UseHymnofHope", "Healing Spell");
            AddControlInWinForm("Use Renew", "UseRenew", "Healing Spell");
            AddControlInWinForm("Use Vampiric Embrace", "UseVampiricEmbrace", "Healing Spell");
            /* Game Settings */
            AddControlInWinForm("Use Low Combat Settings", "UseLowCombat", "Game Settings");
            AddControlInWinForm("Use Trinket", "UseTrinket", "Game Settings");
            AddControlInWinForm("Use Engineering Gloves", "UseEngGlove", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
        }

        public static ShadowPriestSettings CurrentSetting { get; set; }

        public static ShadowPriestSettings GetSettings()
        {
            string CurrentSettingsFile = Application.StartupPath + "\\CustomClasses\\Settings\\Shadow_Priest.xml";
            if (System.IO.File.Exists(CurrentSettingsFile))
            {
                return
                    CurrentSetting = Settings.Load<Shadow_Priest.ShadowPriestSettings>(CurrentSettingsFile);
            }
            else
            {
                return new Shadow_Priest.ShadowPriestSettings();
            }
        }
    }

    private readonly ShadowPriestSettings MySettings = ShadowPriestSettings.GetSettings();

    #region Professions & Racials

    private readonly Spell Arcane_Torrent = new Spell("Arcane Torrent");
    private readonly Spell Berserking = new Spell("Berserking");
    private readonly Spell Blood_Fury = new Spell("Blood Fury");
    private readonly Spell Lifeblood = new Spell("Lifeblood");
    private readonly Spell Stoneform = new Spell("Stoneform");
    private readonly Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    private readonly Spell War_Stomp = new Spell("War Stomp");

    #endregion

    #region Priest Buffs

    private readonly Spell Inner_Fire = new Spell("Inner Fire");
    private readonly Spell Inner_Will = new Spell("Inner Will");
    private readonly Spell Levitate = new Spell("Levitate");
    private Timer Levitate_Timer = new Timer(0);
    private readonly Spell Power_Word_Fortitude = new Spell("Power Word: Fortitude");
    private readonly Spell Shadowform = new Spell("Shadowform");

    #endregion

    #region Offensive Spell

    private readonly Spell Devouring_Plague = new Spell("Devouring Plague");
    private Timer Devouring_Plague_Timer = new Timer(0);
    private readonly Spell Mind_Blast = new Spell("Mind Blast");
    private readonly Spell Mind_Flay = new Spell("Mind Flay");
    private readonly Spell Mind_Sear = new Spell("Mind Sear");
    private readonly Spell Mind_Spike = new Spell("Mind Spike");
    private readonly Spell Shadow_Word_Death = new Spell("Shadow Word: Death");
    private readonly Spell Shadow_Word_Insanity = new Spell("Shadow Word: Insanity");
    private readonly Spell Shadow_Word_Pain = new Spell("Shadow Word: Pain");
    private Timer Shadow_Word_Pain_Timer = new Timer(0);
    private readonly Spell Smite = new Spell("Smite");
    private readonly Spell Vampiric_Touch = new Spell("Vampiric Touch");
    private Timer Vampiric_Touch_Timer = new Timer(0);

    #endregion

    #region Offensive Cooldown

    private readonly Spell Power_Infusion = new Spell("Power Infusion");
    private readonly Spell Shadowfiend = new Spell("Shadowfiend");

    #endregion

    #region Defensive Cooldown

    private readonly Spell Dispersion = new Spell("Dispersion");
    private readonly Spell Power_Word_Shield = new Spell("Power Word: Shield");
    private readonly Spell Psychic_Horror = new Spell("Psychic Horror");
    private readonly Spell Psychic_Scream = new Spell("Psychic Scream");
    private readonly Spell Psyfiend = new Spell("Psyfiend");
    private readonly Spell Silence = new Spell("Silence");
    private readonly Spell Spectral_Guise = new Spell("Spectral Guise");
    private readonly Spell Void_Tendrils = new Spell("Void Tendrils");

    #endregion

    #region Healing Spell

    private readonly Spell Desperate_Prayer = new Spell("Desperate Prayer");
    private readonly Spell Flash_Heal = new Spell("Flash Heal");
    private readonly Spell Hymn_of_Hope = new Spell("Hymn of Hope");
    private readonly Spell Prayer_of_Mending = new Spell("Prayer of Mending");
    private readonly Spell Renew = new Spell("Renew");
    private Timer Renew_Timer = new Timer(0);
    private readonly Spell Vampiric_Embrace = new Spell("Vampiric Embrace");

    #endregion

    private Timer Trinket_Timer = new Timer(0);
    private Timer AlchFlask_Timer = new Timer(0);
    public int LC = 0;

    public Shadow_Priest()
    {
        Main.range = 30.0f;
        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            try
            {
                if (!ObjectManager.Me.IsMounted)
                {
                    Buff_Levitate();
                    if (Fight.InFight && ObjectManager.Me.Target > 0)
                    {
                        if (ObjectManager.Me.Target != lastTarget &&
                            (Mind_Spike.IsDistanceGood || Shadow_Word_Pain.IsDistanceGood))
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
        if (Devouring_Plague.IsSpellUsable && Devouring_Plague.KnownSpell && Devouring_Plague.IsDistanceGood
            && ObjectManager.Me.ShadowOrbs == 3 && MySettings.UseDevouringPlague)
        {
            Devouring_Plague.Launch();
            return;
        }

        else
        {
            if (Shadow_Word_Pain.IsSpellUsable && Shadow_Word_Pain.KnownSpell && Shadow_Word_Pain.IsDistanceGood
                && MySettings.UseShadowWordPain)
            {
                Shadow_Word_Pain.Launch();
                return;
            }
        }
    }

    private void Buff_Levitate()
    {
        if (!Fight.InFight && Levitate.KnownSpell && Levitate.IsSpellUsable && MySettings.UseLevitate
            && (!Levitate.HaveBuff || Levitate_Timer.IsReady))
        {
            Levitate.Launch();
            Levitate_Timer = new Timer(1000 * 60 * 9);
        }
    }

    public void Buff()
    {
        if (Power_Word_Fortitude.KnownSpell && Power_Word_Fortitude.IsSpellUsable &&
            !Power_Word_Fortitude.HaveBuff && MySettings.UsePowerWordFortitude)
        {
            Power_Word_Fortitude.Launch();
            return;
        }

        else if (Inner_Fire.KnownSpell && Inner_Fire.IsSpellUsable && !Inner_Fire.HaveBuff
            && MySettings.UseInnerFire)
        {
            Inner_Fire.Launch();
            return;
        }

        else if (Inner_Will.KnownSpell && Inner_Will.IsSpellUsable && !Inner_Will.HaveBuff
        && !MySettings.UseInnerFire && MySettings.UseInnerWill)
        {
            Inner_Will.Launch();
            return;
        }

        else if (AlchFlask_Timer.IsReady && MySettings.UseAlchFlask
            && ItemsManager.GetItemCountByIdLUA(75525) == 1)
        {
            Logging.WriteFight("Use Alchi Flask");
            Lua.RunMacroText("/use item:75525");
            AlchFlask_Timer = new Timer(1000 * 60 * 60 * 2);
        }

        else
        {
            if (!Shadowform.HaveBuff && Shadowform.KnownSpell && Shadowform.IsSpellUsable
                && MySettings.UseShadowform)
            {
                Shadowform.Launch();
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

        if (Devouring_Plague.IsSpellUsable && Devouring_Plague.KnownSpell && Devouring_Plague.IsDistanceGood
            && ObjectManager.Me.ShadowOrbs == 3 && MySettings.UseDevouringPlague)
        {
            Devouring_Plague.Launch();
            return;
        }

        else if (Mind_Spike.KnownSpell && Mind_Spike.IsSpellUsable && Mind_Spike.IsDistanceGood
            && MySettings.UseMindSpike)
        {
            Mind_Spike.Launch();
            if (ObjectManager.Target.HealthPercent < 50 && ObjectManager.Target.HealthPercent > 0)
            {
                Mind_Spike.Launch();
                return;
            }
            return;
        }

        else
        {
            if (Mind_Sear.KnownSpell && Mind_Sear.IsSpellUsable && Mind_Sear.IsDistanceGood
                    && MySettings.UseMindSear)
            {
                Mind_Sear.Launch();
                return;
            }
        }
    }

    public void Combat()
    {
        AvoidMelee();
        Defense_Cycle();
        Heal();
        Decast();
        Buff();
        DPS_Burst();
        DPS_Cycle();
    }

    public void DPS_Burst()
    {
        if (MySettings.UseTrinket && Trinket_Timer.IsReady)
        {
            Logging.WriteFight("Use Trinket 1.");
            Lua.RunMacroText("/use 13");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Logging.WriteFight("Use Trinket 2.");
            Lua.RunMacroText("/use 14");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Trinket_Timer = new Timer(1000 * 60 * 2);
        }

        if (Berserking.IsSpellUsable && Berserking.KnownSpell && MySettings.UseBerserking)
        {
            Berserking.Launch();
        }

        if (Blood_Fury.IsSpellUsable && Blood_Fury.KnownSpell && MySettings.UseBloodFury)
        {
            Blood_Fury.Launch();
        }

        if (Lifeblood.IsSpellUsable && Lifeblood.KnownSpell && MySettings.UseLifeblood)
        {
            Lifeblood.Launch();
        }

        if (MySettings.UseEngGlove)
        {
            Logging.WriteFight("Use Engineering Gloves.");
            Lua.RunMacroText("/use 10");
        }

        if (Power_Infusion.IsSpellUsable && Power_Infusion.KnownSpell && Power_Infusion.IsDistanceGood
            && MySettings.UsePowerInfusion)
        {
            Power_Infusion.Launch();
            return;
        }

        if (Shadowfiend.IsSpellUsable && Shadowfiend.KnownSpell && Shadowfiend.IsDistanceGood
            && MySettings.UseShadowfiend)
        {
            Shadowfiend.Launch();
            return;
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

        else if (ObjectManager.GetNumberAttackPlayer() > 4 && Mind_Sear.IsSpellUsable && Mind_Sear.KnownSpell
            && Mind_Sear.IsDistanceGood && !ObjectManager.Me.IsCast && MySettings.UseMindSear)
        {
            Mind_Sear.Launch();
            return;
        }

        else if (Shadow_Word_Pain.KnownSpell && Shadow_Word_Pain.IsSpellUsable 
            && Shadow_Word_Pain.IsDistanceGood && MySettings.UseShadowWordPain
            && (!Shadow_Word_Pain.TargetHaveBuff || Shadow_Word_Pain_Timer.IsReady))
        {
            Shadow_Word_Pain.Launch();
            Shadow_Word_Pain_Timer = new Timer(1000 * 14);
            return;
        }

        else if (Shadow_Word_Insanity.KnownSpell && Shadow_Word_Insanity.IsDistanceGood
            && Shadow_Word_Insanity.IsSpellUsable && MySettings.UseShadowWordInsanity)
        {
            Shadow_Word_Insanity.Launch();
            Shadow_Word_Pain_Timer = new Timer(0);
            return;
        }

        else if (Vampiric_Touch.KnownSpell && Vampiric_Touch.IsSpellUsable
            && Vampiric_Touch.IsDistanceGood && MySettings.UseVampiricTouch
            && (!Vampiric_Touch.TargetHaveBuff || Vampiric_Touch_Timer.IsReady))
        {
            Vampiric_Touch.Launch();
            Vampiric_Touch_Timer = new Timer(1000 * 11);
            return;
        }

        else if (Shadow_Word_Death.IsSpellUsable && Shadow_Word_Death.IsDistanceGood && Shadow_Word_Death.KnownSpell
            && ObjectManager.Target.HealthPercent < 20 && MySettings.UseShadowWordDeath)
        {
            Shadow_Word_Death.Launch();
            return;
        }

        else if (Mind_Spike.IsSpellUsable && Mind_Spike.IsDistanceGood && Mind_Spike.KnownSpell &&
            ObjectManager.Me.HaveBuff(87160) && MySettings.UseMindSpike)
        {
            Mind_Spike.Launch();
            return;
        }

        else if (Devouring_Plague.KnownSpell && Devouring_Plague.IsSpellUsable && Devouring_Plague.IsDistanceGood &&
            ObjectManager.Me.ShadowOrbs == 3 && MySettings.UseDevouringPlague
            && (!Devouring_Plague.TargetHaveBuff || Devouring_Plague_Timer.IsReady))
        {
            Devouring_Plague.Launch();
            Devouring_Plague_Timer = new Timer(1000 * 3);
            return;
        }

        else if (Mind_Blast.KnownSpell && Mind_Blast.IsSpellUsable && Mind_Blast.IsDistanceGood
            && ObjectManager.Me.ShadowOrbs < 3 && MySettings.UseMindBlast)
        {
            Mind_Blast.Launch();
            return;
        }

        else if (!ObjectManager.Me.IsCast && Mind_Flay.IsSpellUsable && Mind_Flay.KnownSpell && Mind_Flay.IsDistanceGood
            && MySettings.UseMindFlay)
        {
            Mind_Flay.Launch();
            return;
        }

        else if (!ObjectManager.Me.IsCast && Smite.IsSpellUsable && Smite.KnownSpell && Smite.IsDistanceGood
            && MySettings.UseMindFlay)
        {
            Smite.Launch();
            return;
        }

        else
        {
            if (!ObjectManager.Me.IsCast && Smite.IsSpellUsable && Smite.KnownSpell && Smite.IsDistanceGood)
            {
                Smite.Launch();
                return;
            }
        }
        return;
    }

    public void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            Heal();
            Buff();
        }
    }

    private void Heal()
    {
        if (!Fight.InFight && ObjectManager.Me.BarTwoPercentage < 40 && Hymn_of_Hope.KnownSpell && Hymn_of_Hope.IsSpellUsable
            && ObjectManager.Target.IsDead && MySettings.UseHymnofHope)
        {
            MovementManager.StopMove();
            Hymn_of_Hope.Launch();
            Thread.Sleep(8000);
        }

        if (!Fight.InFight && ObjectManager.Me.HealthPercent < 100 && Flash_Heal.KnownSpell && Flash_Heal.IsSpellUsable
            && ObjectManager.Target.IsDead && MySettings.UseFlashHeal)
        {
            MovementManager.StopMove();
            Flash_Heal.Launch();
            Thread.Sleep(1500);
        }

        if (ObjectManager.Me.HealthPercent < 80 && Gift_of_the_Naaru.IsSpellUsable && Gift_of_the_Naaru.KnownSpell
            && MySettings.UseGiftoftheNaaru)
        {
            Gift_of_the_Naaru.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 80 && War_Stomp.IsSpellUsable && War_Stomp.KnownSpell
            && MySettings.UseWarStomp && ObjectManager.Target.GetDistance < 8)
        {
            War_Stomp.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 80 && Vampiric_Embrace.IsSpellUsable && Vampiric_Embrace.KnownSpell
            && MySettings.UseVampiricEmbrace)
        {
            Vampiric_Embrace.Launch();
            return;
        }

        if (Renew.KnownSpell && Renew.IsSpellUsable && !Renew.HaveBuff && Renew_Timer.IsReady &&
            ObjectManager.Me.HealthPercent < 75 && MySettings.UseRenew)
        {
            Renew.Launch();
            Renew_Timer = new Timer(1000 * 12);
            return;
        }

        if (ObjectManager.Me.HealthPercent < 70 && Prayer_of_Mending.KnownSpell && Prayer_of_Mending.IsSpellUsable
            && MySettings.UsePrayerofMending)
        {
            Prayer_of_Mending.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 65 && Desperate_Prayer.KnownSpell && Desperate_Prayer.IsSpellUsable
            && MySettings.UseDesperatePrayer)
        {
            Desperate_Prayer.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 75 && Power_Word_Shield.KnownSpell && Power_Word_Shield.IsSpellUsable 
            && !Power_Word_Shield.HaveBuff && MySettings.UsePowerWordShield)
        {
            Power_Word_Shield.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 55 && Flash_Heal.KnownSpell && Flash_Heal.IsSpellUsable
            && MySettings.UseFlashHeal)
        {
            Flash_Heal.Launch();
            Thread.Sleep(1200);
            return;
        }
    }

    private void Defense_Cycle()
    {
        if (ObjectManager.Me.HealthPercent < 20 && Psychic_Scream.IsSpellUsable && Psychic_Scream.KnownSpell
            && MySettings.UsePsychicScream)
        {
            Psychic_Scream.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 20 && Void_Tendrils.IsSpellUsable && Void_Tendrils.KnownSpell
            && MySettings.UseVoidTendrils)
        {
            Void_Tendrils.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 20 && Dispersion.KnownSpell && Dispersion.IsSpellUsable
            && MySettings.UseDispersion)
        {
            if (Renew.KnownSpell && Renew.IsSpellUsable && MySettings.UseRenew)
            {
                Renew_Timer = new Timer(1000 * 12);
                Renew.Launch();
            }
            Thread.Sleep(200);
            Dispersion.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 20 && Psyfiend.IsSpellUsable && Psyfiend.KnownSpell
            && MySettings.UsePsyfiend)
        {
            Psyfiend.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 20 && Spectral_Guise.IsSpellUsable && Spectral_Guise.KnownSpell
            && MySettings.UseSpectralGuise)
        {
            Spectral_Guise.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 80 && Stoneform.IsSpellUsable && Stoneform.KnownSpell
            && MySettings.UseStoneform)
        {
            Stoneform.Launch();
            return;
        }
    }

    private void Decast()
    {
        if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && MySettings.UseSilence
            && Silence.KnownSpell && Silence.IsSpellUsable && Silence.IsDistanceGood)
        {
            Silence.Launch();
            return;
        }

        if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && MySettings.UsePsychicHorror
            && Psychic_Horror.KnownSpell && Psychic_Horror.IsSpellUsable && Psychic_Horror.IsDistanceGood)
        {
            Psychic_Horror.Launch();
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

#endregion
