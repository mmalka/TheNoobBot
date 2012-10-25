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
    internal static float range = 30;
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
                    #region Hunter Specialisation checking

                case WoWClass.Hunter:
                    var Hunter_Marksmanship_Spell = new Spell("Aimed Shot");
                    var Hunter_Survival_Spell = new Spell("Explosive Shot");
                    var Hunter_BeastMastery_Spell = new Spell("Kill Command");

                    if (Hunter_Marksmanship_Spell.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Hunter_Marksmanship.xml";
                            Hunter_Marksmanship.HunterMarksmanshipSettings CurrentSetting;
                            CurrentSetting = new Hunter_Marksmanship.HunterMarksmanshipSettings();
                            if (System.IO.File.Exists(CurrentSettingsFile))
                            {
                                CurrentSetting =
                                    Settings.Load<Hunter_Marksmanship.HunterMarksmanshipSettings>(CurrentSettingsFile);
                            }
                            CurrentSetting.ToForm();
                            CurrentSetting.Save(CurrentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Hunter Marksmanship class...");
                            new Hunter_Marksmanship();
                        }
                        break;
                    }

                    else if (Hunter_Survival_Spell.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Hunter_Survival.xml";
                            Hunter_Survival.HunterSurvivalSettings CurrentSetting;
                            CurrentSetting = new Hunter_Survival.HunterSurvivalSettings();
                            if (System.IO.File.Exists(CurrentSettingsFile))
                            {
                                CurrentSetting =
                                    Settings.Load<Hunter_Survival.HunterSurvivalSettings>(CurrentSettingsFile);
                            }
                            CurrentSetting.ToForm();
                            CurrentSetting.Save(CurrentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Hunter Survival class...");
                            new Hunter_Survival();
                        }
                        break;
                    }

                    else if (Hunter_BeastMastery_Spell.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Hunter_BeastMastery.xml";
                            Hunter_BeastMastery.HunterBeastMasterySettings CurrentSetting;
                            CurrentSetting = new Hunter_BeastMastery.HunterBeastMasterySettings();
                            if (System.IO.File.Exists(CurrentSettingsFile))
                            {
                                CurrentSetting =
                                    Settings.Load<Hunter_BeastMastery.HunterBeastMasterySettings>(CurrentSettingsFile);
                            }
                            CurrentSetting.ToForm();
                            CurrentSetting.Save(CurrentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Hunter BeastMastery class...");
                            new Hunter_BeastMastery();
                        }
                        break;
                    }

                    else
                    {
                        if (ConfigOnly)
                        {
                            MessageBox.Show("There is no settings available for your Class/Specialisation.");
                        }
                        else
                        {
                            Logging.WriteFight("Hunter without Spec");
                            new Hunter();
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

#region Hunter

public class Hunter_Marksmanship
{
    [Serializable]
    public class HunterMarksmanshipSettings : nManager.Helpful.Settings
    {
        /* Professions & Racials */
        public bool UseArcaneTorrent = true;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseLifeblood = true;
        public bool UseStoneform = true;
        public bool UseGiftoftheNaaru = true;
        public bool UseWarStomp = true;
        /* Hunter Buffs */
        public bool UseAspectoftheHawk = true;
        public bool UseCamouflage = false;
        public bool UseFeignDeath = true;
        public bool UseHuntersMark = true;
        public bool UseMisdirection = true;
        /* Offensive Spell */
        public bool UseAimedShot = true;
        public bool UseArcaneShot = true;
        public bool UsePet1 = true;
        public bool UsePet2 = false;
        public bool UsePet3 = false;
        public bool UsePet4 = false;
        public bool UsePet5 = false;
        public bool UseChimeraShot = true;
        public bool UseExplosiveTrap = true;
        public bool UseKillShot = true;
        public bool UseMultiShot = true;
        public bool UseSerpentSting = true;
        public bool UseSteadyShot = true;
        /* Offensive Cooldown */
        public bool UseAMurderofCrows = true;
        public bool UseBarrage = true;
        public bool UseBlinkStrike = true;
        public bool UseDireBeast = true;
        public bool UseFervor = true;
        public bool UseGlaiveToss = true;
        public bool UseLynxRush = true;
        public bool UsePowershot = true;
        public bool UseRapidFire = true;
        public bool UseReadiness = true;
        public bool UseStampede = true;
        /* Defensive Cooldown */
        public bool UseBindingShot = true;
        public bool UseConcussiveShot = true;
        public bool UseDeterrance = true;
        public bool UseDisengage = true;
        public bool UseIceTrap = true;
        public bool UseScatterShot = true;
        public bool UseSilencingShot = true;
        public bool UseWyvernSting = true;
        /* Healing Spell */
        public bool UseExhilaration = true;
        public bool UseFeedPet = true;
        public bool UseMendPet = true;
        public bool UseRevivePet = true;
        /* Game Settings */
        public bool UseLowCombat = true;
        public bool UseTrinket = true;
        public bool UseEngGlove = true;
        public bool UseAlchFlask = true;

        public HunterMarksmanshipSettings()
        {
            ConfigWinForm(new System.Drawing.Point(400, 400), "Hunter Marksmanship Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");
            AddControlInWinForm("Use Lifeblood", "UseLifeblood", "Professions & Racials");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            /* Hunter Buffs */
            AddControlInWinForm("Use Aspect of the Hawk", "UseAspectoftheHawk", "Hunter Buffs");
            AddControlInWinForm("Use Camouflage", "UseCamouflage", "Hunter Buffs");
            AddControlInWinForm("Use Feign Death", "UseFeignDeath", "Hunter Buffs");
            AddControlInWinForm("Use Hunter's Mark", "UseHuntersMark", "Hunter Buffs");
            AddControlInWinForm("Use Misdirection", "UseMisdirection", "Hunter Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Aimed Shot", "UseAimedShot", "Offensive Spell");
            AddControlInWinForm("Use Arcane Shot", "UseArcaneShot", "Offensive Spell");
            AddControlInWinForm("Use Pet in Slot 1", "UsePet1", "Offensive Spell");
            AddControlInWinForm("Use Pet in Slot 2", "UsePet2", "Offensive Spell");
            AddControlInWinForm("Use Pet in Slot 3", "UsePet3", "Offensive Spell");
            AddControlInWinForm("Use Pet in Slot 4", "UsePet4", "Offensive Spell");
            AddControlInWinForm("Use Pet in Slot 5", "UsePet5", "Offensive Spell");
            AddControlInWinForm("Use Chimera Shot", "UseChimeraShot", "Offensive Spell");
            AddControlInWinForm("Use Explosive Trap", "UseExplosiveTrap", "Offensive Spell");
            AddControlInWinForm("Use KillShot", "UseKillShot", "Offensive Spell");
            AddControlInWinForm("Use Multi-Shot", "UseMultiShot", "Offensive Spell");
            AddControlInWinForm("Use Serpent Sting", "UseSerpentSting", "Offensive Spell");
            AddControlInWinForm("Use Steady Shot", "UseSteadyShot", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use A Murder of Crows", "UseAMurderofCrows", "Offensive Cooldown");
            AddControlInWinForm("Use Barrage", "UseBarrage", "Offensive Cooldown");
            AddControlInWinForm("Use Blink Strike", "UseBlinkStrike", "Offensive Cooldown");
            AddControlInWinForm("Use Dire Beast", "UseDireBeast", "Offensive Cooldown");
            AddControlInWinForm("Use Fervor", "UseFervor", "Offensive Cooldown");
            AddControlInWinForm("Use Glaive Toss", "UseGlaiveToss", "Offensive Cooldown");
            AddControlInWinForm("Use Lynx Rush", "UseLynxRush", "Offensive Cooldown");
            AddControlInWinForm("Use Powershot", "UsePowershot", "Offensive Cooldown");
            AddControlInWinForm("Use Rapid Fire", "UseRapidFire", "Offensive Cooldown");
            AddControlInWinForm("Use Readiness", "UseReadiness", "Offensive Cooldown");
            AddControlInWinForm("Use Stampede", "UseStampede", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Binding Shot", "UseBindingShot", "Defensive Cooldown");
            AddControlInWinForm("Use Concussive Shot", "UseConcussiveShot", "Defensive Cooldown");
            AddControlInWinForm("Use Deterrance", "UseDeterrance", "Defensive Cooldown");
            AddControlInWinForm("Use Disengage", "UseDisengage", "Defensive Cooldown");
            AddControlInWinForm("Use Ice Trap", "UseIceTrap", "Defensive Cooldown");
            AddControlInWinForm("Use Scatter Shot", "UseScatterShot", "Defensive Cooldown");
            AddControlInWinForm("Use Silencing Shot", "UseSilencingShot", "Defensive Cooldown");
            AddControlInWinForm("Use Wyvern Sting ", "UseWyvernSting ", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Exhilaration", "UseExhilaration", "Healing Spell");
            AddControlInWinForm("Use Feed Pet", "UseFeedPet", "Healing Spell");
            AddControlInWinForm("Use Mend Pet", "UseMendPet", "Healing Spell");
            AddControlInWinForm("Use Revive Pet", "UseRevivePet", "Healing Spell");
            /* Game Settings */
            AddControlInWinForm("Use Low Combat Settings", "UseLowCombat", "Game Settings");
            AddControlInWinForm("Use Trinket", "UseTrinket", "Game Settings");
            AddControlInWinForm("Use Engineering Gloves", "UseEngGlove", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
        }

        public static HunterMarksmanshipSettings CurrentSetting { get; set; }

        public static HunterMarksmanshipSettings GetSettings()
        {
            string CurrentSettingsFile = Application.StartupPath + "\\CustomClasses\\Settings\\Hunter_Marksmanship.xml";
            if (System.IO.File.Exists(CurrentSettingsFile))
            {
                return
                    CurrentSetting = Settings.Load<Hunter_Marksmanship.HunterMarksmanshipSettings>(CurrentSettingsFile);
            }
            else
            {
                return new Hunter_Marksmanship.HunterMarksmanshipSettings();
            }
        }
    }

    private readonly HunterMarksmanshipSettings MySettings = HunterMarksmanshipSettings.GetSettings();

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

    #region Hunter Buffs

    private readonly Spell Aspect_of_the_Hawk = new Spell("Aspect of the Hawk");
    private readonly Spell Camouflage = new Spell("Camouflage");
    private readonly Spell Feign_Death = new Spell("Feign Death");
    private readonly Spell Hunters_Mark = new Spell("Hunter's Mark");
    private readonly Spell Misdirection = new Spell("Misdirection");

    #endregion

    #region Offensive Spell

    private readonly Spell Aimed_Shot = new Spell("Aimed Shot");
    private readonly Spell Arcane_Shot = new Spell("Arcane Shot");
    private readonly Spell Call_Pet_1 = new Spell("Call Pet 1");
    private readonly Spell Call_Pet_2 = new Spell("Call Pet 2");
    private readonly Spell Call_Pet_3 = new Spell("Call Pet 3");
    private readonly Spell Call_Pet_4 = new Spell("Call Pet 4");
    private readonly Spell Call_Pet_5 = new Spell("Call Pet 5");
    private readonly Spell Chimera_Shot = new Spell("Chimera Shot");
    private readonly Spell Explosive_Trap = new Spell("Explosive Trap");
    private readonly Spell Kill_Shot = new Spell("Kill Shot");
    private readonly Spell Multi_Shot = new Spell("Multi-Shot");
    private readonly Spell Serpent_Sting = new Spell("Serpent Sting");
    private Timer Serpent_Sting_Timer = new Timer(0);
    private readonly Spell Steady_Shot = new Spell("Steady Shot");

    #endregion

    #region Offensive Cooldown

    private readonly Spell A_Murder_of_Crows = new Spell("A Murder of Crows");
    private readonly Spell Barrage = new Spell("Barrage");
    private readonly Spell Blink_Strike = new Spell("Blink Strike");
    private readonly Spell Dire_Beast = new Spell("Dire Beast");
    private Timer Dire_Beast_Timer = new Timer(0);
    private readonly Spell Fervor = new Spell("Fervor");
    private readonly Spell Glaive_Toss = new Spell("Glaive Toss");
    private readonly Spell Lynx_Rush = new Spell("Lynx Rush");
    private readonly Spell Powershot = new Spell("Powershot");
    private readonly Spell Rapid_Fire = new Spell("Rapid Fire");
    private readonly Spell Readiness = new Spell("Readiness");
    private readonly Spell Stampede = new Spell("Stampede");

    #endregion

    #region Defensive Cooldown

    private readonly Spell Binding_Shot = new Spell("Binding Shot");
    private readonly Spell Concussive_Shot = new Spell("Concussive Shot");
    private readonly Spell Deterrance = new Spell("Deterrance");
    private readonly Spell Disengage = new Spell("Disengage");
    private readonly Spell Ice_Trap = new Spell("Ice Trap");
    private readonly Spell Scatter_Shot = new Spell("Scatter Shot");
    private readonly Spell Silencing_Shot = new Spell("Silencing Shot");
    private readonly Spell Wyvern_Sting = new Spell("Wyvern Sting");

    #endregion

    #region Healing Spell

    private readonly Spell Exhilaration = new Spell("Exhilaration");
    private readonly Spell Feed_Pet = new Spell("Feed Pet");
    private readonly Spell Mend_Pet = new Spell("Mend Pet");
    private Timer Mend_Pet_Timer = new Timer(0);
    private readonly Spell Revive_Pet = new Spell("Revive Pet");

    #endregion

    private Timer OnCD = new Timer(0);
    private Timer Trinket_Timer = new Timer(0);
    private Timer Engineering_Timer = new Timer(0);
    private Timer AlchFlask_Timer = new Timer(0);
    private Timer Steady_Focus_Timer = new Timer(0);
    public int LC = 0;

    public Hunter_Marksmanship()
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
                            (Hunters_Mark.IsDistanceGood || Serpent_Sting.IsDistanceGood))
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
        if (Hunters_Mark.KnownSpell && Hunters_Mark.IsSpellUsable && MySettings.UseHuntersMark
            && Hunters_Mark.IsDistanceGood && !Hunters_Mark.TargetHaveBuff && LC != 1)
            Hunters_Mark.Launch();

        if (ObjectManager.Pet.IsAlive)
        {
            Lua.RunMacroText("/petattack");
            Logging.WriteFight("Launch Pet Attack");
        }

        if (ObjectManager.Pet.IsAlive && MySettings.UseMisdirection && Misdirection.KnownSpell
            && Misdirection.IsSpellUsable)
        {
            Lua.RunMacroText("/target pet");
            Thread.Sleep(200);
            Misdirection.Launch();
            Thread.Sleep(200);
        }

        if (Serpent_Sting.KnownSpell && Serpent_Sting.IsSpellUsable && Serpent_Sting.IsDistanceGood
            && MySettings.UseSerpentSting)
        {
            Serpent_Sting.Launch();
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

    public void LowCombat()
    {
        AvoidMelee();
        Heal();
        Defense_Cycle();
        Buff();

        if (Glaive_Toss.KnownSpell && Glaive_Toss.IsSpellUsable && Glaive_Toss.IsDistanceGood
            && MySettings.UseGlaiveToss)
        {
            Glaive_Toss.Launch();
            return;
        }
        else if (Arcane_Shot.IsSpellUsable && Arcane_Shot.IsDistanceGood && Arcane_Shot.KnownSpell
            && MySettings.UseArcaneShot)
        {
            Arcane_Shot.Launch();
            return;
        }
        else
        {
            if (Steady_Shot.KnownSpell && Steady_Shot.IsSpellUsable && Steady_Shot.IsDistanceGood
                && MySettings.UseSteadyShot)
            {
                Steady_Shot.Launch();
                return;
            }
        }

        if (Explosive_Trap.KnownSpell && Explosive_Trap.IsSpellUsable && Explosive_Trap.IsDistanceGood
            && MySettings.UseExplosiveTrap)
        {
            Explosive_Trap.Launch();
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
        else if (A_Murder_of_Crows.KnownSpell && A_Murder_of_Crows.IsSpellUsable  && A_Murder_of_Crows.IsDistanceGood
            && MySettings.UseAMurderofCrows && !A_Murder_of_Crows.TargetHaveBuff)
        {
            A_Murder_of_Crows.Launch();
            return;
        }
        else if (Barrage.KnownSpell && Barrage.IsSpellUsable && MySettings.UseBarrage && Barrage.IsDistanceGood)
        {
            Barrage.Launch();
            return;
        }
        else if (Blink_Strike.KnownSpell && Blink_Strike.IsSpellUsable && ObjectManager.Pet.IsAlive
            && MySettings.UseBlinkStrike && ObjectManager.Target.GetDistance < 40)
        {
            Blink_Strike.Launch();
            return;
        }
        else if (Dire_Beast.KnownSpell && Dire_Beast.IsSpellUsable && MySettings.UseDireBeast
            && Dire_Beast.IsDistanceGood && Dire_Beast_Timer.IsReady)
        {
            Dire_Beast.Launch();
            Dire_Beast_Timer = new Timer(1000*15);
            return;
        }
        else if (Fervor.KnownSpell && Fervor.IsSpellUsable && ObjectManager.Me.Focus < 50
            && MySettings.UseFervor)
        {
            Fervor.Launch();
            return;
        }
        else if (Glaive_Toss.KnownSpell && Glaive_Toss.IsSpellUsable && MySettings.UseGlaiveToss && Glaive_Toss.IsDistanceGood)
        {
            Glaive_Toss.Launch();
            return;
        }
        else if (Lynx_Rush.KnownSpell && Lynx_Rush.IsSpellUsable && MySettings.UseLynxRush && ObjectManager.Target.GetDistance < 40)
        {
            Lynx_Rush.Launch();
            return;
        }
        else if (Powershot.KnownSpell && Powershot.IsSpellUsable && MySettings.UsePowershot && Powershot.IsDistanceGood)
        {
            Powershot.Launch();
            return;
        }
        else if (Stampede.KnownSpell && Stampede.IsSpellUsable && MySettings.UseStampede && Stampede.IsDistanceGood)
        {
            Stampede.Launch();
            return;
        }
        else if (Rapid_Fire.KnownSpell && Rapid_Fire.IsSpellUsable && MySettings.UseRapidFire 
            && ObjectManager.Target.GetDistance < 40)
        {
            Rapid_Fire.Launch();
            return;
        }
        else
        {
            if (Readiness.KnownSpell && Readiness.IsSpellUsable && MySettings.UseReadiness)
            {
                Readiness.Launch();
                return;
            }
        }
    }

    public void DPS_Cycle()
    {
        if (Serpent_Sting.IsSpellUsable && Serpent_Sting.IsDistanceGood && Serpent_Sting.KnownSpell
            && MySettings.UseSerpentSting && !Serpent_Sting.TargetHaveBuff)
        {
            Serpent_Sting.Launch();
            Serpent_Sting_Timer = new Timer(1000*12);
            return;
        }
        else if (Chimera_Shot.KnownSpell && Chimera_Shot.IsSpellUsable && Chimera_Shot.IsDistanceGood
            && MySettings.UseChimeraShot)
        {
            Chimera_Shot.Launch();
            Serpent_Sting_Timer = new Timer(1000*12);
            return;
        }
        else if (Kill_Shot.KnownSpell && Kill_Shot.IsSpellUsable && Kill_Shot.IsDistanceGood
            && MySettings.UseKillShot)
        {
            Kill_Shot.Launch();
            return;
        }
        else if (Aimed_Shot.KnownSpell && Aimed_Shot.IsSpellUsable && Aimed_Shot.IsDistanceGood
            && MySettings.UseAimedShot && ObjectManager.Me.HaveBuff(82926))
        {
            Aimed_Shot.Launch();
            return;
        }
        else if (Steady_Shot.KnownSpell && Steady_Shot.IsSpellUsable && Steady_Shot.IsDistanceGood
            && MySettings.UseSteadyShot && (!ObjectManager.Me.HaveBuff(53220) || Steady_Focus_Timer.IsReady))
        {
            Steady_Shot.Launch();
            Steady_Focus_Timer = new Timer(1000*6);
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 3 && MySettings.UseMultiShot && MySettings.UseSteadyShot)
        {
            if (Multi_Shot.KnownSpell && Multi_Shot.IsSpellUsable && Multi_Shot.IsDistanceGood)
            {
                Multi_Shot.Launch();
                return;
            }
            else
            {
                if (Steady_Shot.KnownSpell && Steady_Shot.IsSpellUsable && Steady_Shot.IsDistanceGood)
                {
                    Steady_Shot.Launch();
                    return;
                }
            }
        }
        else if (Arcane_Shot.KnownSpell && Arcane_Shot.IsSpellUsable && Arcane_Shot.IsDistanceGood
            && MySettings.UseArcaneShot && ObjectManager.Me.FocusPercentage > 64)
        {
            Arcane_Shot.Launch();
            return;
        }
        else
        {
            if (Steady_Shot.KnownSpell && Steady_Shot.IsSpellUsable && Steady_Shot.IsDistanceGood
                && MySettings.UseSteadyShot && ObjectManager.Me.FocusPercentage < 80)
            {
                Steady_Shot.Launch();
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

        Pet();

        if (MySettings.UseAspectoftheHawk && Aspect_of_the_Hawk.KnownSpell && Aspect_of_the_Hawk.IsSpellUsable 
            && !Aspect_of_the_Hawk.HaveBuff && !ObjectManager.Me.HaveBuff(109260))
        {
            Aspect_of_the_Hawk.Launch();
            return;
        }

        if (MySettings.UseCamouflage && Camouflage.KnownSpell && Camouflage.IsSpellUsable && !Camouflage.HaveBuff
            && !Fight.InFight && ObjectManager.GetNumberAttackPlayer() == 0)
        {
            Camouflage.Launch();
            return;
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (Exhilaration.KnownSpell && Exhilaration.IsSpellUsable
            && MySettings.UseExhilaration && ObjectManager.Me.HealthPercent < 70)
        {
            Exhilaration.Launch();
            return;
        }
        else if (ObjectManager.Pet.Health > 0 && ObjectManager.Pet.HealthPercent < 50
            && Feed_Pet.KnownSpell && Feed_Pet.IsSpellUsable && MySettings.UseFeedPet
            && !Fight.InFight && ObjectManager.GetNumberAttackPlayer() == 0)
        {
            Feed_Pet.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Pet.Health > 0 && ObjectManager.Pet.HealthPercent < 80
                && Mend_Pet.KnownSpell && Mend_Pet.IsSpellUsable && MySettings.UseMendPet
                && Mend_Pet_Timer.IsReady)
            {
                Mend_Pet.Launch();
                Mend_Pet_Timer = new Timer(1000*10);
                return;
            }
        }
    }

    private void Defense_Cycle()
    {
        if (ObjectManager.Me.HealthPercent < 20 && MySettings.UseFeignDeath
            && Feign_Death.KnownSpell && Feign_Death.IsSpellUsable)
        {
            Feign_Death.Launch();
            Thread.Sleep(5000);
            if (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0)
                return;
            else
                Thread.Sleep(5000);
        }
        else if (ObjectManager.Me.HealthPercent < 50 && MySettings.UseDeterrance
            && Deterrance.KnownSpell && Deterrance.IsSpellUsable)
        {
            Deterrance.Launch();
            Thread.Sleep(200);
        }
        else if (ObjectManager.Me.HealthPercent < 80 && MySettings.UseIceTrap
            && Ice_Trap.KnownSpell && Ice_Trap.IsSpellUsable && ObjectManager.Target.GetDistance < 10
            && Disengage.KnownSpell && Disengage.IsSpellUsable && MySettings.UseDisengage)
        {
            Ice_Trap.Launch();
            Thread.Sleep(1000);
            nManager.Wow.Helpers.Keybindings.PressKeybindings(nManager.Wow.Enums.Keybindings.JUMP);
            Disengage.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && MySettings.UseConcussiveShot 
            && Concussive_Shot.KnownSpell && Concussive_Shot.IsSpellUsable && Concussive_Shot.IsDistanceGood
            && Disengage.KnownSpell && Disengage.IsSpellUsable && MySettings.UseDisengage)
        {
            Concussive_Shot.Launch();
            Thread.Sleep(1000);
            nManager.Wow.Helpers.Keybindings.PressKeybindings(nManager.Wow.Enums.Keybindings.JUMP);
            Disengage.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && MySettings.UseBindingShot 
            && Binding_Shot.KnownSpell && Binding_Shot.IsSpellUsable && Binding_Shot.IsDistanceGood
            && Disengage.KnownSpell && Disengage.IsSpellUsable && MySettings.UseDisengage)
        {
            Binding_Shot.Launch();
            Thread.Sleep(1000);
            nManager.Wow.Helpers.Keybindings.PressKeybindings(nManager.Wow.Enums.Keybindings.JUMP);
            Disengage.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && War_Stomp.IsSpellUsable && War_Stomp.KnownSpell
            && MySettings.UseWarStomp)
        {
            War_Stomp.Launch();
            OnCD = new Timer(1000*2);
            return;
        }
        else
        {
            if (ObjectManager.Me.HealthPercent < 80 && Stoneform.IsSpellUsable && Stoneform.KnownSpell
                && MySettings.UseStoneform)
            {
                Stoneform.Launch();
                OnCD = new Timer(1000*8);
                return;
            }
        }
    }

    private void Decast()
    {
        if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && ObjectManager.Target.GetDistance < 8
            && Arcane_Torrent.KnownSpell && Arcane_Torrent.IsSpellUsable && MySettings.UseArcaneTorrent)
        {
            Arcane_Torrent.Launch();
            return;
        }
        else if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && Silencing_Shot.IsDistanceGood
            && Silencing_Shot.KnownSpell && Silencing_Shot.IsSpellUsable && MySettings.UseSilencingShot)
        {
            Silencing_Shot.Launch();
            return;
        }
        else if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && Scatter_Shot.IsDistanceGood
            && Scatter_Shot.KnownSpell && Scatter_Shot.IsSpellUsable && MySettings.UseScatterShot)
        {
            Scatter_Shot.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && MySettings.UseWyvernSting
                && Wyvern_Sting.KnownSpell && Wyvern_Sting.IsSpellUsable && Wyvern_Sting.IsDistanceGood)
            {
                Wyvern_Sting.Launch();
                return;
            }
        }
    }

    private void Pet()
    {
        if (!ObjectManager.Me.IsCast && (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0)
            && Call_Pet_1.KnownSpell && Call_Pet_1.IsSpellUsable && MySettings.UsePet1)
        {
            Call_Pet_1.Launch();
            Thread.Sleep(1000);
        }
        else if (!ObjectManager.Me.IsCast && (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0)
            && Call_Pet_2.KnownSpell && Call_Pet_2.IsSpellUsable && MySettings.UsePet2)
        {
            Call_Pet_2.Launch();
            Thread.Sleep(1000);
        }
        else if (!ObjectManager.Me.IsCast && (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0)
            && Call_Pet_3.KnownSpell && Call_Pet_3.IsSpellUsable && MySettings.UsePet3)
        {
            Call_Pet_3.Launch();
            Thread.Sleep(1000);
        }
        else if (!ObjectManager.Me.IsCast && (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0)
            && Call_Pet_4.KnownSpell && Call_Pet_4.IsSpellUsable && MySettings.UsePet4)
        {
            Call_Pet_4.Launch();
            Thread.Sleep(1000);
        }
        else
        {
            if (!ObjectManager.Me.IsCast && (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0)
                && Call_Pet_5.KnownSpell && Call_Pet_5.IsSpellUsable && MySettings.UsePet5)
            {
                Call_Pet_5.Launch();
                Thread.Sleep(1000);
            }
        }

        if (!ObjectManager.Me.IsCast && (!ObjectManager.Pet.IsAlive || ObjectManager.Pet.Guid == 0)
            && Revive_Pet.KnownSpell && Revive_Pet.IsSpellUsable && MySettings.UseRevivePet
            && !Fight.InFight && ObjectManager.GetNumberAttackPlayer() == 0)
        {
            Revive_Pet.Launch();
            Thread.Sleep(1000);
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

public class Hunter_BeastMastery
{
    [Serializable]
    public class HunterBeastMasterySettings : nManager.Helpful.Settings
    {
        /* Professions & Racials */
        public bool UseArcaneTorrent = true;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseLifeblood = true;
        public bool UseStoneform = true;
        public bool UseGiftoftheNaaru = true;
        public bool UseWarStomp = true;
        /* Hunter Buffs */
        public bool UseAspectoftheHawk = true;
        public bool UseCamouflage = false;
        public bool UseFeignDeath = true;
        public bool UseHuntersMark = true;
        public bool UseMisdirection = true;
        /* Offensive Spell */
        public bool UseArcaneShot = true;
        public bool UsePet1 = true;
        public bool UsePet2 = false;
        public bool UsePet3 = false;
        public bool UsePet4 = false;
        public bool UsePet5 = false;
        public bool UseCobraShot = true;
        public bool UseExplosiveTrap = true;
        public bool UseKillCommand = true;
        public bool UseKillShot = true;
        public bool UseMultiShot = true;
        public bool UseSerpentSting = true;
        /* Offensive Cooldown */
        public bool UseAMurderofCrows = true;
        public bool UseBarrage = true;
        public bool UseBestialWrath = true;
        public bool UseBlinkStrike = true;
        public bool UseDireBeast = true;
        public bool UseFervor = true;
        public bool UseFocusFire = false;
        public bool UseGlaiveToss = true;
        public bool UseLynxRush = true;
        public bool UsePowershot = true;
        public bool UseRapidFire = true;
        public bool UseReadiness = true;
        public bool UseStampede = true;
        /* Defensive Cooldown */
        public bool UseBindingShot = true;
        public bool UseConcussiveShot = true;
        public bool UseDeterrance = true;
        public bool UseDisengage = true;
        public bool UseIceTrap = true;
        public bool UseIntimidation = true;
        public bool UseScatterShot = true;
        public bool UseSilencingShot = true;
        public bool UseWyvernSting = true;
        /* Healing Spell */
        public bool UseExhilaration = true;
        public bool UseFeedPet = true;
        public bool UseMendPet = true;
        public bool UseRevivePet = true;
        /* Game Settings */
        public bool UseLowCombat = true;
        public bool UseTrinket = true;
        public bool UseEngGlove = true;
        public bool UseAlchFlask = true;
        public bool UseCoreHoundPet = false;
        public bool UseWormPet = false;
        public bool UseChimeraPet = false;
        public bool UseSpiritBeastPet = false;

        public HunterBeastMasterySettings()
        {
            ConfigWinForm(new System.Drawing.Point(400, 400), "Hunter BeastMastery Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");
            AddControlInWinForm("Use Lifeblood", "UseLifeblood", "Professions & Racials");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            /* Hunter Buffs */
            AddControlInWinForm("Use Aspect of the Hawk", "UseAspectoftheHawk", "Hunter Buffs");
            AddControlInWinForm("Use Camouflage", "UseCamouflage", "Hunter Buffs");
            AddControlInWinForm("Use Feign Death", "UseFeignDeath", "Hunter Buffs");
            AddControlInWinForm("Use Hunter's Mark", "UseHuntersMark", "Hunter Buffs");
            AddControlInWinForm("Use Misdirection", "UseMisdirection", "Hunter Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Arcane Shot", "UseArcaneShot", "Offensive Spell");
            AddControlInWinForm("Use Pet in Slot 1", "UsePet1", "Offensive Spell");
            AddControlInWinForm("Use Pet in Slot 2", "UsePet2", "Offensive Spell");
            AddControlInWinForm("Use Pet in Slot 3", "UsePet3", "Offensive Spell");
            AddControlInWinForm("Use Pet in Slot 4", "UsePet4", "Offensive Spell");
            AddControlInWinForm("Use Pet in Slot 5", "UsePet5", "Offensive Spell");
            AddControlInWinForm("Use Cobra Shot", "UseCobraShot", "Offensive Spell");
            AddControlInWinForm("Use Explosive Trap", "UseExplosiveTrap", "Offensive Spell");
            AddControlInWinForm("Use Kill Command", "UseKillCommand", "Offensive Spell");
            AddControlInWinForm("Use KillShot", "UseKillShot", "Offensive Spell");
            AddControlInWinForm("Use Multi-Shot", "UseMultiShot", "Offensive Spell");
            AddControlInWinForm("Use Serpent Sting", "UseSerpentSting", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use A Murder of Crows", "UseAMurderofCrows", "Offensive Cooldown");
            AddControlInWinForm("Use Barrage", "UseBarrage", "Offensive Cooldown");
            AddControlInWinForm("Use Bestial Wrath", "UseBestialWrath", "Offensive Cooldown");
            AddControlInWinForm("Use Blink Strike", "UseBlinkStrike", "Offensive Cooldown");
            AddControlInWinForm("Use Dire Beast", "UseDireBeast", "Offensive Cooldown");
            AddControlInWinForm("Use Fervor", "UseFervor", "Offensive Cooldown");
            AddControlInWinForm("Use Focus Fire", "UseFocusFire", "Offensive Cooldown");
            AddControlInWinForm("Use Glaive Toss", "UseGlaiveToss", "Offensive Cooldown");
            AddControlInWinForm("Use Lynx Rush", "UseLynxRush", "Offensive Cooldown");
            AddControlInWinForm("Use Powershot", "UsePowershot", "Offensive Cooldown");
            AddControlInWinForm("Use Rapid Fire", "UseRapidFire", "Offensive Cooldown");
            AddControlInWinForm("Use Readiness", "UseReadiness", "Offensive Cooldown");
            AddControlInWinForm("Use Stampede", "UseStampede", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Binding Shot", "UseBindingShot", "Defensive Cooldown");
            AddControlInWinForm("Use Concussive Shot", "UseConcussiveShot", "Defensive Cooldown");
            AddControlInWinForm("Use Deterrance", "UseDeterrance", "Defensive Cooldown");
            AddControlInWinForm("Use Disengage", "UseDisengage", "Defensive Cooldown");
            AddControlInWinForm("Use Ice Trap", "UseIceTrap", "Defensive Cooldown");
            AddControlInWinForm("Use Intimidation", "UseIntimidation", "Defensive Cooldown");
            AddControlInWinForm("Use Scatter Shot", "UseScatterShot", "Defensive Cooldown");
            AddControlInWinForm("Use Silencing Shot", "UseSilencingShot", "Defensive Cooldown");
            AddControlInWinForm("Use Wyvern Sting ", "UseWyvernSting ", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Exhilaration", "UseExhilaration", "Healing Spell");
            AddControlInWinForm("Use Feed Pet", "UseFeedPet", "Healing Spell");
            AddControlInWinForm("Use Mend Pet", "UseMendPet", "Healing Spell");
            AddControlInWinForm("Use Revive Pet", "UseRevivePet", "Healing Spell");
            /* Game Settings */
            AddControlInWinForm("Use Low Combat Settings", "UseLowCombat", "Game Settings");
            AddControlInWinForm("Use Trinket", "UseTrinket", "Game Settings");
            AddControlInWinForm("Use Engineering Gloves", "UseEngGlove", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
            AddControlInWinForm("Use Core Hound Pet", "UseCoreHoundPet", "Game Settings");
            AddControlInWinForm("Use Worm Pet", "UseWormPet", "Game Settings");
            AddControlInWinForm("Use Chimera Pet", "UseChimeraPet", "Game Settings");
            AddControlInWinForm("Use Spirit Beast Pet", "UseSpiritBeastPet", "Game Settings");
        }

        public static HunterBeastMasterySettings CurrentSetting { get; set; }

        public static HunterBeastMasterySettings GetSettings()
        {
            string CurrentSettingsFile = Application.StartupPath + "\\CustomClasses\\Settings\\Hunter_BeastMastery.xml";
            if (System.IO.File.Exists(CurrentSettingsFile))
            {
                return
                    CurrentSetting = Settings.Load<Hunter_BeastMastery.HunterBeastMasterySettings>(CurrentSettingsFile);
            }
            else
            {
                return new Hunter_BeastMastery.HunterBeastMasterySettings();
            }
        }
    }

    private readonly HunterBeastMasterySettings MySettings = HunterBeastMasterySettings.GetSettings();

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

    #region Hunter Buffs

    private readonly Spell Aspect_of_the_Hawk = new Spell("Aspect of the Hawk");
    private readonly Spell Camouflage = new Spell("Camouflage");
    private readonly Spell Feign_Death = new Spell("Feign Death");
    private readonly Spell Hunters_Mark = new Spell("Hunter's Mark");
    private readonly Spell Misdirection = new Spell("Misdirection");

    #endregion

    #region Offensive Spell

    private readonly Spell Arcane_Shot = new Spell("Arcane Shot");
    private readonly Spell Call_Pet_1 = new Spell("Call Pet 1");
    private readonly Spell Call_Pet_2 = new Spell("Call Pet 2");
    private readonly Spell Call_Pet_3 = new Spell("Call Pet 3");
    private readonly Spell Call_Pet_4 = new Spell("Call Pet 4");
    private readonly Spell Call_Pet_5 = new Spell("Call Pet 5");
    private readonly Spell Cobra_Shot = new Spell("Cobra Shot");
    private readonly Spell Explosive_Trap = new Spell("Explosive Trap");
    private readonly Spell Kill_Command = new Spell("Kill Command");
    private readonly Spell Kill_Shot = new Spell("Kill Shot");
    private readonly Spell Multi_Shot = new Spell("Multi-Shot");
    private readonly Spell Serpent_Sting = new Spell("Serpent Sting");
    private Timer Serpent_Sting_Timer = new Timer(0);

    #endregion

    #region Offensive Cooldown

    private readonly Spell A_Murder_of_Crows = new Spell("A Murder of Crows");
    private readonly Spell Barrage = new Spell("Barrage");
    private readonly Spell Bestial_Wrath = new Spell("Bestial Wrath");
    private readonly Spell Blink_Strike = new Spell("Blink Strike");
    private readonly Spell Dire_Beast = new Spell("Dire Beast");
    private Timer Dire_Beast_Timer = new Timer(0);
    private readonly Spell Fervor = new Spell("Fervor");
    private readonly Spell Focus_Fire = new Spell("Focus Fire");
    private readonly Spell Glaive_Toss = new Spell("Glaive Toss");
    private readonly Spell Lynx_Rush = new Spell("Lynx Rush");
    private readonly Spell Powershot = new Spell("Powershot");
    private readonly Spell Rapid_Fire = new Spell("Rapid Fire");
    private readonly Spell Readiness = new Spell("Readiness");
    private readonly Spell Stampede = new Spell("Stampede");

    #endregion

    #region Defensive Cooldown

    private readonly Spell Binding_Shot = new Spell("Binding Shot");
    private readonly Spell Concussive_Shot = new Spell("Concussive Shot");
    private readonly Spell Deterrance = new Spell("Deterrance");
    private readonly Spell Disengage = new Spell("Disengage");
    private readonly Spell Ice_Trap = new Spell("Ice Trap");
    private readonly Spell Intimidation = new Spell("Intimidation");
    private readonly Spell Scatter_Shot = new Spell("Scatter Shot");
    private readonly Spell Silencing_Shot = new Spell("Silencing Shot");
    private readonly Spell Wyvern_Sting = new Spell("Wyvern Sting");

    #endregion

    #region Healing Spell

    private readonly Spell Exhilaration = new Spell("Exhilaration");
    private readonly Spell Feed_Pet = new Spell("Feed Pet");
    private readonly Spell Mend_Pet = new Spell("Mend Pet");
    private Timer Mend_Pet_Timer = new Timer(0);
    private readonly Spell Revive_Pet = new Spell("Revive Pet");

    #endregion

    private Timer OnCD = new Timer(0);
    private Timer Trinket_Timer = new Timer(0);
    private Timer Engineering_Timer = new Timer(0);
    private Timer AlchFlask_Timer = new Timer(0);
    private Timer Ancient_Hysteria_Timer = new Timer(0);
    private Timer Burrow_Attack_Timer = new Timer(0);
    private Timer Froststorm_Breath_Timer = new Timer(0);
    private Timer Spirit_Mend_Timer = new Timer(0);
    public int LC = 0;

    public Hunter_BeastMastery()
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
                            (Hunters_Mark.IsDistanceGood || Serpent_Sting.IsDistanceGood))
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
        if (Hunters_Mark.KnownSpell && Hunters_Mark.IsSpellUsable && MySettings.UseHuntersMark
            && Hunters_Mark.IsDistanceGood && !Hunters_Mark.TargetHaveBuff && LC != 1)
            Hunters_Mark.Launch();

        if (ObjectManager.Pet.IsAlive)
        {
            Lua.RunMacroText("/petattack");
            Logging.WriteFight("Launch Pet Attack");
        }

        if (ObjectManager.Pet.IsAlive && MySettings.UseMisdirection && Misdirection.KnownSpell
            && Misdirection.IsSpellUsable)
        {
            Lua.RunMacroText("/target pet");
            Thread.Sleep(200);
            Misdirection.Launch();
            Thread.Sleep(200);
        }

        if (Serpent_Sting.KnownSpell && Serpent_Sting.IsSpellUsable && Serpent_Sting.IsDistanceGood
            && MySettings.UseSerpentSting)
        {
            Serpent_Sting.Launch();
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

    public void LowCombat()
    {
        AvoidMelee();
        Heal();
        Defense_Cycle();
        Buff();

        if (Glaive_Toss.KnownSpell && Glaive_Toss.IsSpellUsable && Glaive_Toss.IsDistanceGood
            && MySettings.UseGlaiveToss)
        {
            Glaive_Toss.Launch();
            return;
        }
        else if (Arcane_Shot.IsSpellUsable && Arcane_Shot.IsDistanceGood && Arcane_Shot.KnownSpell
            && MySettings.UseArcaneShot)
        {
            Arcane_Shot.Launch();
            return;
        }
        else
        {
            if (Cobra_Shot.KnownSpell && Cobra_Shot.IsSpellUsable && Cobra_Shot.IsDistanceGood
                && MySettings.UseCobraShot)
            {
                Cobra_Shot.Launch();
                return;
            }
        }

        if (Explosive_Trap.KnownSpell && Explosive_Trap.IsSpellUsable && Explosive_Trap.IsDistanceGood
            && MySettings.UseExplosiveTrap)
        {
            Explosive_Trap.Launch();
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
        else if (A_Murder_of_Crows.KnownSpell && A_Murder_of_Crows.IsSpellUsable  && A_Murder_of_Crows.IsDistanceGood
            && MySettings.UseAMurderofCrows && !A_Murder_of_Crows.TargetHaveBuff)
        {
            A_Murder_of_Crows.Launch();
            return;
        }
        else if (Barrage.KnownSpell && Barrage.IsSpellUsable && MySettings.UseBarrage && Barrage.IsDistanceGood)
        {
            Barrage.Launch();
            return;
        }
        else if (Blink_Strike.KnownSpell && Blink_Strike.IsSpellUsable && ObjectManager.Pet.IsAlive
            && MySettings.UseBlinkStrike && ObjectManager.Target.GetDistance < 40)
        {
            Blink_Strike.Launch();
            return;
        }
        else if (Dire_Beast.KnownSpell && Dire_Beast.IsSpellUsable && MySettings.UseDireBeast
            && Dire_Beast.IsDistanceGood && Dire_Beast_Timer.IsReady)
        {
            Dire_Beast.Launch();
            Dire_Beast_Timer = new Timer(1000*15);
            return;
        }
        else if (Fervor.KnownSpell && Fervor.IsSpellUsable && ObjectManager.Me.Focus < 50
            && MySettings.UseFervor)
        {
            Fervor.Launch();
            return;
        }
        else if (Glaive_Toss.KnownSpell && Glaive_Toss.IsSpellUsable && MySettings.UseGlaiveToss && Glaive_Toss.IsDistanceGood)
        {
            Glaive_Toss.Launch();
            return;
        }
        else if (Lynx_Rush.KnownSpell && Lynx_Rush.IsSpellUsable && MySettings.UseLynxRush && ObjectManager.Target.GetDistance < 40)
        {
            Lynx_Rush.Launch();
            return;
        }
        else if (Powershot.KnownSpell && Powershot.IsSpellUsable && MySettings.UsePowershot && Powershot.IsDistanceGood)
        {
            Powershot.Launch();
            return;
        }
        else if (Stampede.KnownSpell && Stampede.IsSpellUsable && MySettings.UseStampede && Stampede.IsDistanceGood)
        {
            Stampede.Launch();
            return;
        }
        else if (Bestial_Wrath.KnownSpell && Bestial_Wrath.IsSpellUsable && MySettings.UseBestialWrath 
            && ObjectManager.Target.GetDistance < 40)
        {
            Bestial_Wrath.Launch();
            return;
        }
        else if (Rapid_Fire.KnownSpell && Rapid_Fire.IsSpellUsable && MySettings.UseRapidFire 
            && ObjectManager.Target.GetDistance < 40 && !Bestial_Wrath.HaveBuff)
        {
            Rapid_Fire.Launch();
            return;
        }
        else if (MySettings.UseCoreHoundPet && ObjectManager.Target.GetDistance < 40
            && Ancient_Hysteria_Timer.IsReady && ObjectManager.Me.HaveBuff(95809)
            && ObjectManager.Pet.IsAlive && !Rapid_Fire.HaveBuff && !Bestial_Wrath.HaveBuff)
        {
            Lua.RunMacroText("/cast Ancient Hysteria");
            Logging.WriteFight("Launch Core Hound Pet Ancient Hysteria");
            Ancient_Hysteria_Timer = new Timer(1000*60*6);
            return;
        }
        else if (ObjectManager.Pet.BuffStack(19623) == 5 && Focus_Fire.IsSpellUsable && Focus_Fire.KnownSpell 
            && MySettings.UseFocusFire)
        {
            Focus_Fire.Launch();
            return;
        }
        else
        {
            if (Readiness.KnownSpell && Readiness.IsSpellUsable && MySettings.UseReadiness
                && !Rapid_Fire.IsSpellUsable && !Bestial_Wrath.IsSpellUsable)
            {
                Readiness.Launch();
                return;
            }
        }
    }

    public void DPS_Cycle()
    {
        if (Serpent_Sting.IsSpellUsable && Serpent_Sting.IsDistanceGood && Serpent_Sting.KnownSpell
            && MySettings.UseSerpentSting && !Serpent_Sting.TargetHaveBuff)
        {
            Serpent_Sting.Launch();
            Serpent_Sting_Timer = new Timer(1000*12);
            return;
        }
        else if (Cobra_Shot.KnownSpell && Cobra_Shot.IsSpellUsable && Cobra_Shot.IsDistanceGood
            && MySettings.UseCobraShot && Serpent_Sting_Timer.IsReady)
        {
            Cobra_Shot.Launch();
            Serpent_Sting_Timer = new Timer(1000*12);
            return;
        }
        else if (Kill_Shot.KnownSpell && Kill_Shot.IsSpellUsable && Kill_Shot.IsDistanceGood
            && MySettings.UseKillShot)
        {
            Kill_Shot.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 2 && MySettings.UseMultiShot)
        {
            if (Multi_Shot.KnownSpell && Multi_Shot.IsSpellUsable && Multi_Shot.IsDistanceGood)
            {
                Multi_Shot.Launch();
                return;
            }
            else if (MySettings.UseChimeraPet && ObjectManager.Target.GetDistance < 10
                && ObjectManager.Pet.Guid == 780 && ObjectManager.Pet.Focus > 29
                && Froststorm_Breath_Timer.IsReady && ObjectManager.Pet.IsAlive)
            {
                Lua.RunMacroText("/cast Froststorm Breath");
                Logging.WriteFight("Launch Chimera Pet AoE");
                Froststorm_Breath_Timer = new Timer(1000*8);
                return;
            }
            else
            {
                if (MySettings.UseWormPet && ObjectManager.Target.GetDistance < 10
                    && ObjectManager.Pet.Guid == 784 && ObjectManager.Pet.Focus > 29
                    && Burrow_Attack_Timer.IsReady && ObjectManager.Pet.IsAlive)
                {
                    Lua.RunMacroText("/cast Burrow Attack");
                    Logging.WriteFight("Launch Worm Pet AoE");
                    Burrow_Attack_Timer = new Timer(1000*20);
                    return;
                }
            }
        }
        else if (Kill_Command.KnownSpell && Kill_Command.IsSpellUsable && Kill_Command.IsDistanceGood
            && MySettings.UseKillCommand)
        {
            Kill_Command.Launch();
            return;
        }
        else if (Arcane_Shot.KnownSpell && Arcane_Shot.IsSpellUsable && Arcane_Shot.IsDistanceGood
            && MySettings.UseArcaneShot && ObjectManager.Me.FocusPercentage > 59)
        {
            Arcane_Shot.Launch();
            return;
        }
        else
        {
            if (Cobra_Shot.KnownSpell && Cobra_Shot.IsSpellUsable && Cobra_Shot.IsDistanceGood
                && MySettings.UseCobraShot && ObjectManager.Me.FocusPercentage < 60)
            {
                Cobra_Shot.Launch();
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

        Pet();

        if (MySettings.UseAspectoftheHawk && Aspect_of_the_Hawk.KnownSpell && Aspect_of_the_Hawk.IsSpellUsable 
            && !Aspect_of_the_Hawk.HaveBuff && !ObjectManager.Me.HaveBuff(109260))
        {
            Aspect_of_the_Hawk.Launch();
            return;
        }

        if (MySettings.UseCamouflage && Camouflage.KnownSpell && Camouflage.IsSpellUsable && !Camouflage.HaveBuff
            && !Fight.InFight && ObjectManager.GetNumberAttackPlayer() == 0)
        {
            Camouflage.Launch();
            return;
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (ObjectManager.Me.HealthPercent < 85 && ObjectManager.Pet.IsAlive //&& ObjectManager.Pet.Guid == 0)
            && MySettings.UseSpiritBeastPet && Spirit_Mend_Timer.IsReady)
        {
            Logging.WriteFight("Cast Spirit Mend.");
            Lua.RunMacroText("/target Player");
            Thread.Sleep(200);
            Lua.RunMacroText("/cast Spirit Mend");
            Spirit_Mend_Timer = new Timer(1000*40);
        }
        else if (Exhilaration.KnownSpell && Exhilaration.IsSpellUsable
            && MySettings.UseExhilaration && ObjectManager.Me.HealthPercent < 70)
        {
            Exhilaration.Launch();
            return;
        }
        else if (ObjectManager.Pet.Health > 0 && ObjectManager.Pet.HealthPercent < 50
            && Feed_Pet.KnownSpell && Feed_Pet.IsSpellUsable && MySettings.UseFeedPet
            && !Fight.InFight && ObjectManager.GetNumberAttackPlayer() == 0)
        {
            Feed_Pet.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Pet.Health > 0 && ObjectManager.Pet.HealthPercent < 80
                && Mend_Pet.KnownSpell && Mend_Pet.IsSpellUsable && MySettings.UseMendPet
                && Mend_Pet_Timer.IsReady)
            {
                Mend_Pet.Launch();
                Mend_Pet_Timer = new Timer(1000*10);
                return;
            }
        }
    }

    private void Defense_Cycle()
    {
        if (ObjectManager.Me.HealthPercent < 20 && MySettings.UseFeignDeath
            && Feign_Death.KnownSpell && Feign_Death.IsSpellUsable)
        {
            Feign_Death.Launch();
            Thread.Sleep(5000);
            if (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0)
                return;
            else
                Thread.Sleep(5000);
        }
        else if (ObjectManager.Me.HealthPercent < 50 && MySettings.UseDeterrance
            && Deterrance.KnownSpell && Deterrance.IsSpellUsable)
        {
            Deterrance.Launch();
            Thread.Sleep(200);
        }
        else if (ObjectManager.Me.HealthPercent < 80 && MySettings.UseIceTrap
            && Ice_Trap.KnownSpell && Ice_Trap.IsSpellUsable && ObjectManager.Target.GetDistance < 10
            && Disengage.KnownSpell && Disengage.IsSpellUsable && MySettings.UseDisengage)
        {
            Ice_Trap.Launch();
            Thread.Sleep(1000);
            nManager.Wow.Helpers.Keybindings.PressKeybindings(nManager.Wow.Enums.Keybindings.JUMP);
            Disengage.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && MySettings.UseConcussiveShot 
            && Concussive_Shot.KnownSpell && Concussive_Shot.IsSpellUsable && Concussive_Shot.IsDistanceGood
            && Disengage.KnownSpell && Disengage.IsSpellUsable && MySettings.UseDisengage)
        {
            Concussive_Shot.Launch();
            Thread.Sleep(1000);
            nManager.Wow.Helpers.Keybindings.PressKeybindings(nManager.Wow.Enums.Keybindings.JUMP);
            Disengage.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && MySettings.UseBindingShot 
            && Binding_Shot.KnownSpell && Binding_Shot.IsSpellUsable && Binding_Shot.IsDistanceGood
            && Disengage.KnownSpell && Disengage.IsSpellUsable && MySettings.UseDisengage)
        {
            Binding_Shot.Launch();
            Thread.Sleep(1000);
            nManager.Wow.Helpers.Keybindings.PressKeybindings(nManager.Wow.Enums.Keybindings.JUMP);
            Disengage.Launch();
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
            if (ObjectManager.Me.HealthPercent < 80 && Intimidation.IsSpellUsable && Intimidation.KnownSpell
                && MySettings.UseIntimidation)
            {
                Intimidation.Launch();
                OnCD = new Timer(1000*3);
                return;
            }
        }
    }

    private void Decast()
    {
        if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && ObjectManager.Target.GetDistance < 8
            && Arcane_Torrent.KnownSpell && Arcane_Torrent.IsSpellUsable && MySettings.UseArcaneTorrent)
        {
            Arcane_Torrent.Launch();
            return;
        }
        else if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && Silencing_Shot.IsDistanceGood
            && Silencing_Shot.KnownSpell && Silencing_Shot.IsSpellUsable && MySettings.UseSilencingShot)
        {
            Silencing_Shot.Launch();
            return;
        }
        else if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && Scatter_Shot.IsDistanceGood
            && Scatter_Shot.KnownSpell && Scatter_Shot.IsSpellUsable && MySettings.UseScatterShot)
        {
            Scatter_Shot.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && MySettings.UseWyvernSting
                && Wyvern_Sting.KnownSpell && Wyvern_Sting.IsSpellUsable && Wyvern_Sting.IsDistanceGood)
            {
                Wyvern_Sting.Launch();
                return;
            }
        }
    }

    private void Pet()
    {
        if (!ObjectManager.Me.IsCast && (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0)
            && Call_Pet_1.KnownSpell && Call_Pet_1.IsSpellUsable && MySettings.UsePet1)
        {
            Call_Pet_1.Launch();
            Thread.Sleep(1000);
        }
        else if (!ObjectManager.Me.IsCast && (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0)
            && Call_Pet_2.KnownSpell && Call_Pet_2.IsSpellUsable && MySettings.UsePet2)
        {
            Call_Pet_2.Launch();
            Thread.Sleep(1000);
        }
        else if (!ObjectManager.Me.IsCast && (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0)
            && Call_Pet_3.KnownSpell && Call_Pet_3.IsSpellUsable && MySettings.UsePet3)
        {
            Call_Pet_3.Launch();
            Thread.Sleep(1000);
        }
        else if (!ObjectManager.Me.IsCast && (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0)
            && Call_Pet_4.KnownSpell && Call_Pet_4.IsSpellUsable && MySettings.UsePet4)
        {
            Call_Pet_4.Launch();
            Thread.Sleep(1000);
        }
        else
        {
            if (!ObjectManager.Me.IsCast && (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0)
                && Call_Pet_5.KnownSpell && Call_Pet_5.IsSpellUsable && MySettings.UsePet5)
            {
                Call_Pet_5.Launch();
                Thread.Sleep(1000);
            }
        }

        if (!ObjectManager.Me.IsCast && (!ObjectManager.Pet.IsAlive || ObjectManager.Pet.Guid == 0)
            && Revive_Pet.KnownSpell && Revive_Pet.IsSpellUsable && MySettings.UseRevivePet
            && !Fight.InFight && ObjectManager.GetNumberAttackPlayer() == 0)
        {
            Revive_Pet.Launch();
            Thread.Sleep(1000);
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

public class Hunter_Survival
{
    [Serializable]
    public class HunterSurvivalSettings : nManager.Helpful.Settings
    {
        /* Professions & Racials */
        public bool UseArcaneTorrent = true;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseLifeblood = true;
        public bool UseStoneform = true;
        public bool UseGiftoftheNaaru = true;
        public bool UseWarStomp = true;
        /* Hunter Buffs */
        public bool UseAspectoftheHawk = true;
        public bool UseCamouflage = false;
        public bool UseFeignDeath = true;
        public bool UseHuntersMark = true;
        public bool UseMisdirection = true;
        /* Offensive Spell */
        public bool UseArcaneShot = true;
        public bool UseBlackArrow = true;
        public bool UsePet1 = true;
        public bool UsePet2 = false;
        public bool UsePet3 = false;
        public bool UsePet4 = false;
        public bool UsePet5 = false;
        public bool UseCobraShot = true;
        public bool UseExplosiveShot = true;
        public bool UseExplosiveTrap = true;
        public bool UseKillShot = true;
        public bool UseMultiShot = true;
        public bool UseSerpentSting = true;
        /* Offensive Cooldown */
        public bool UseAMurderofCrows = true;
        public bool UseBarrage = true;
        public bool UseBlinkStrike = true;
        public bool UseDireBeast = true;
        public bool UseFervor = true;
        public bool UseGlaiveToss = true;
        public bool UseLynxRush = true;
        public bool UsePowershot = true;
        public bool UseRapidFire = true;
        public bool UseReadiness = true;
        public bool UseStampede = true;
        /* Defensive Cooldown */
        public bool UseBindingShot = true;
        public bool UseConcussiveShot = true;
        public bool UseDeterrance = true;
        public bool UseDisengage = true;
        public bool UseIceTrap = true;
        public bool UseScatterShot = true;
        public bool UseSilencingShot = true;
        public bool UseWyvernSting = true;
        /* Healing Spell */
        public bool UseExhilaration = true;
        public bool UseFeedPet = true;
        public bool UseMendPet = true;
        public bool UseRevivePet = true;
        /* Game Settings */
        public bool UseLowCombat = true;
        public bool UseTrinket = true;
        public bool UseEngGlove = true;
        public bool UseAlchFlask = true;

        public HunterSurvivalSettings()
        {
            ConfigWinForm(new System.Drawing.Point(400, 400), "Hunter Survival Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");
            AddControlInWinForm("Use Lifeblood", "UseLifeblood", "Professions & Racials");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            /* Hunter Buffs */
            AddControlInWinForm("Use Aspect of the Hawk", "UseAspectoftheHawk", "Hunter Buffs");
            AddControlInWinForm("Use Camouflage", "UseCamouflage", "Hunter Buffs");
            AddControlInWinForm("Use Feign Death", "UseFeignDeath", "Hunter Buffs");
            AddControlInWinForm("Use Hunter's Mark", "UseHuntersMark", "Hunter Buffs");
            AddControlInWinForm("Use Misdirection", "UseMisdirection", "Hunter Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Arcane Shot", "UseArcaneShot", "Offensive Spell");
            AddControlInWinForm("Use Black Arrow", "UseBlackArrow", "Offensive Spell");
            AddControlInWinForm("Use Pet in Slot 1", "UsePet1", "Offensive Spell");
            AddControlInWinForm("Use Pet in Slot 2", "UsePet2", "Offensive Spell");
            AddControlInWinForm("Use Pet in Slot 3", "UsePet3", "Offensive Spell");
            AddControlInWinForm("Use Pet in Slot 4", "UsePet4", "Offensive Spell");
            AddControlInWinForm("Use Pet in Slot 5", "UsePet5", "Offensive Spell");
            AddControlInWinForm("Use Cobra Shot", "UseCobraShot", "Offensive Spell");
            AddControlInWinForm("Use Explosive Shot", "UseExplosiveShot", "Offensive Spell");
            AddControlInWinForm("Use Explosive Trap", "UseExplosiveTrap", "Offensive Spell");
            AddControlInWinForm("Use KillShot", "UseKillShot", "Offensive Spell");
            AddControlInWinForm("Use Multi-Shot", "UseMultiShot", "Offensive Spell");
            AddControlInWinForm("Use Serpent Sting", "UseSerpentSting", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use A Murder of Crows", "UseAMurderofCrows", "Offensive Cooldown");
            AddControlInWinForm("Use Barrage", "UseBarrage", "Offensive Cooldown");
            AddControlInWinForm("Use Blink Strike", "UseBlinkStrike", "Offensive Cooldown");
            AddControlInWinForm("Use Dire Beast", "UseDireBeast", "Offensive Cooldown");
            AddControlInWinForm("Use Fervor", "UseFervor", "Offensive Cooldown");
            AddControlInWinForm("Use Glaive Toss", "UseGlaiveToss", "Offensive Cooldown");
            AddControlInWinForm("Use Lynx Rush", "UseLynxRush", "Offensive Cooldown");
            AddControlInWinForm("Use Powershot", "UsePowershot", "Offensive Cooldown");
            AddControlInWinForm("Use Rapid Fire", "UseRapidFire", "Offensive Cooldown");
            AddControlInWinForm("Use Readiness", "UseReadiness", "Offensive Cooldown");
            AddControlInWinForm("Use Stampede", "UseStampede", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Binding Shot", "UseBindingShot", "Defensive Cooldown");
            AddControlInWinForm("Use Concussive Shot", "UseConcussiveShot", "Defensive Cooldown");
            AddControlInWinForm("Use Deterrance", "UseDeterrance", "Defensive Cooldown");
            AddControlInWinForm("Use Disengage", "UseDisengage", "Defensive Cooldown");
            AddControlInWinForm("Use Ice Trap", "UseIceTrap", "Defensive Cooldown");
            AddControlInWinForm("Use Scatter Shot", "UseScatterShot", "Defensive Cooldown");
            AddControlInWinForm("Use Silencing Shot", "UseSilencingShot", "Defensive Cooldown");
            AddControlInWinForm("Use Wyvern Sting ", "UseWyvernSting ", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Exhilaration", "UseExhilaration", "Healing Spell");
            AddControlInWinForm("Use Feed Pet", "UseFeedPet", "Healing Spell");
            AddControlInWinForm("Use Mend Pet", "UseMendPet", "Healing Spell");
            AddControlInWinForm("Use Revive Pet", "UseRevivePet", "Healing Spell");
            /* Game Settings */
            AddControlInWinForm("Use Low Combat Settings", "UseLowCombat", "Game Settings");
            AddControlInWinForm("Use Trinket", "UseTrinket", "Game Settings");
            AddControlInWinForm("Use Engineering Gloves", "UseEngGlove", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
        }

        public static HunterSurvivalSettings CurrentSetting { get; set; }

        public static HunterSurvivalSettings GetSettings()
        {
            string CurrentSettingsFile = Application.StartupPath + "\\CustomClasses\\Settings\\Hunter_Survival.xml";
            if (System.IO.File.Exists(CurrentSettingsFile))
            {
                return
                    CurrentSetting = Settings.Load<Hunter_Survival.HunterSurvivalSettings>(CurrentSettingsFile);
            }
            else
            {
                return new Hunter_Survival.HunterSurvivalSettings();
            }
        }
    }

    private readonly HunterSurvivalSettings MySettings = HunterSurvivalSettings.GetSettings();

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

    #region Hunter Buffs

    private readonly Spell Aspect_of_the_Hawk = new Spell("Aspect of the Hawk");
    private readonly Spell Camouflage = new Spell("Camouflage");
    private readonly Spell Feign_Death = new Spell("Feign Death");
    private readonly Spell Hunters_Mark = new Spell("Hunter's Mark");
    private readonly Spell Misdirection = new Spell("Misdirection");

    #endregion

    #region Offensive Spell

    private readonly Spell Arcane_Shot = new Spell("Arcane Shot");
    private readonly Spell Black_Arrow = new Spell("Black Arrow");
    private readonly Spell Call_Pet_1 = new Spell("Call Pet 1");
    private readonly Spell Call_Pet_2 = new Spell("Call Pet 2");
    private readonly Spell Call_Pet_3 = new Spell("Call Pet 3");
    private readonly Spell Call_Pet_4 = new Spell("Call Pet 4");
    private readonly Spell Call_Pet_5 = new Spell("Call Pet 5");
    private readonly Spell Cobra_Shot = new Spell("Cobra Shot");
    private readonly Spell Explosive_Shot = new Spell("Explosive Shot");
    private readonly Spell Explosive_Trap = new Spell("Explosive Trap");
    private readonly Spell Kill_Shot = new Spell("Kill Shot");
    private readonly Spell Multi_Shot = new Spell("Multi-Shot");
    private readonly Spell Serpent_Sting = new Spell("Serpent Sting");
    private Timer Serpent_Sting_Timer = new Timer(0);

    #endregion

    #region Offensive Cooldown

    private readonly Spell A_Murder_of_Crows = new Spell("A Murder of Crows");
    private readonly Spell Barrage = new Spell("Barrage");
    private readonly Spell Blink_Strike = new Spell("Blink Strike");
    private readonly Spell Dire_Beast = new Spell("Dire Beast");
    private Timer Dire_Beast_Timer = new Timer(0);
    private readonly Spell Fervor = new Spell("Fervor");
    private readonly Spell Glaive_Toss = new Spell("Glaive Toss");
    private readonly Spell Lynx_Rush = new Spell("Lynx Rush");
    private readonly Spell Powershot = new Spell("Powershot");
    private readonly Spell Rapid_Fire = new Spell("Rapid Fire");
    private readonly Spell Readiness = new Spell("Readiness");
    private readonly Spell Stampede = new Spell("Stampede");

    #endregion

    #region Defensive Cooldown

    private readonly Spell Binding_Shot = new Spell("Binding Shot");
    private readonly Spell Concussive_Shot = new Spell("Concussive Shot");
    private readonly Spell Deterrance = new Spell("Deterrance");
    private readonly Spell Disengage = new Spell("Disengage");
    private readonly Spell Freezing_Trap = new Spell("Freezing Trap");
    private readonly Spell Ice_Trap = new Spell("Ice Trap");
    private readonly Spell Scatter_Shot = new Spell("Scatter Shot");
    private readonly Spell Silencing_Shot = new Spell("Silencing Shot");
    private readonly Spell Wyvern_Sting = new Spell("Wyvern Sting");

    #endregion

    #region Healing Spell

    private readonly Spell Exhilaration = new Spell("Exhilaration");
    private readonly Spell Feed_Pet = new Spell("Feed Pet");
    private readonly Spell Mend_Pet = new Spell("Mend Pet");
    private Timer Mend_Pet_Timer = new Timer(0);
    private readonly Spell Revive_Pet = new Spell("Revive Pet");

    #endregion

    private Timer OnCD = new Timer(0);
    private Timer Trinket_Timer = new Timer(0);
    private Timer Engineering_Timer = new Timer(0);
    private Timer AlchFlask_Timer = new Timer(0);
    public int LC = 0;

    public Hunter_Survival()
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
                            (Hunters_Mark.IsDistanceGood || Serpent_Sting.IsDistanceGood))
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
                    {
                        if (!ObjectManager.Me.IsCast)
                            Patrolling();
                    }
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
        if (Hunters_Mark.KnownSpell && Hunters_Mark.IsSpellUsable && MySettings.UseHuntersMark
            && Hunters_Mark.IsDistanceGood && !Hunters_Mark.TargetHaveBuff && LC != 1)
            Hunters_Mark.Launch();

        if (ObjectManager.Pet.IsAlive)
        {
            Lua.RunMacroText("/petattack");
            Logging.WriteFight("Launch Pet Attack");
        }

        if (ObjectManager.Pet.IsAlive && MySettings.UseMisdirection && Misdirection.KnownSpell
            && Misdirection.IsSpellUsable)
        {
            Lua.RunMacroText("/target pet");
            Thread.Sleep(200);
            Misdirection.Launch();
            Thread.Sleep(200);
        }

        if (Serpent_Sting.KnownSpell && Serpent_Sting.IsSpellUsable && Serpent_Sting.IsDistanceGood
            && MySettings.UseSerpentSting)
        {
            Serpent_Sting.Launch();
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

    public void LowCombat()
    {
        AvoidMelee();
        Heal();
        Defense_Cycle();
        Buff();

        if (Glaive_Toss.KnownSpell && Glaive_Toss.IsSpellUsable && Glaive_Toss.IsDistanceGood
            && MySettings.UseGlaiveToss)
        {
            Glaive_Toss.Launch();
            return;
        }
        else if (Arcane_Shot.IsSpellUsable && Arcane_Shot.IsDistanceGood && Arcane_Shot.KnownSpell
            && MySettings.UseArcaneShot)
        {
            Arcane_Shot.Launch();
            return;
        }
        else
        {
            if (Cobra_Shot.KnownSpell && Cobra_Shot.IsSpellUsable && Cobra_Shot.IsDistanceGood
                && MySettings.UseCobraShot)
            {
                Cobra_Shot.Launch();
                return;
            }
        }

        if (Explosive_Trap.KnownSpell && Explosive_Trap.IsSpellUsable && Explosive_Trap.IsDistanceGood
            && MySettings.UseExplosiveTrap)
        {
            Explosive_Trap.Launch();
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
        else if (A_Murder_of_Crows.KnownSpell && A_Murder_of_Crows.IsSpellUsable  && A_Murder_of_Crows.IsDistanceGood
            && MySettings.UseAMurderofCrows && !A_Murder_of_Crows.TargetHaveBuff)
        {
            A_Murder_of_Crows.Launch();
            return;
        }
        else if (Barrage.KnownSpell && Barrage.IsSpellUsable && MySettings.UseBarrage && Barrage.IsDistanceGood)
        {
            Barrage.Launch();
            return;
        }
        else if (Blink_Strike.KnownSpell && Blink_Strike.IsSpellUsable && ObjectManager.Pet.IsAlive
            && MySettings.UseBlinkStrike && ObjectManager.Target.GetDistance < 40)
        {
            Blink_Strike.Launch();
            return;
        }
        else if (Dire_Beast.KnownSpell && Dire_Beast.IsSpellUsable && MySettings.UseDireBeast
            && Dire_Beast.IsDistanceGood && Dire_Beast_Timer.IsReady)
        {
            Dire_Beast.Launch();
            Dire_Beast_Timer = new Timer(1000*15);
            return;
        }
        else if (Fervor.KnownSpell && Fervor.IsSpellUsable && ObjectManager.Me.Focus < 50
            && MySettings.UseFervor)
        {
            Fervor.Launch();
            return;
        }
        else if (Glaive_Toss.KnownSpell && Glaive_Toss.IsSpellUsable && MySettings.UseGlaiveToss && Glaive_Toss.IsDistanceGood)
        {
            Glaive_Toss.Launch();
            return;
        }
        else if (Lynx_Rush.KnownSpell && Lynx_Rush.IsSpellUsable && MySettings.UseLynxRush && ObjectManager.Target.GetDistance < 40)
        {
            Lynx_Rush.Launch();
            return;
        }
        else if (Powershot.KnownSpell && Powershot.IsSpellUsable && MySettings.UsePowershot && Powershot.IsDistanceGood)
        {
            Powershot.Launch();
            return;
        }
        else if (Stampede.KnownSpell && Stampede.IsSpellUsable && MySettings.UseStampede && Stampede.IsDistanceGood)
        {
            Stampede.Launch();
            return;
        }
        else if (Rapid_Fire.KnownSpell && Rapid_Fire.IsSpellUsable && MySettings.UseRapidFire 
            && ObjectManager.Target.GetDistance < 40)
        {
            Rapid_Fire.Launch();
            return;
        }
        else
        {
            if (Readiness.KnownSpell && Readiness.IsSpellUsable && MySettings.UseReadiness)
            {
                Readiness.Launch();
                return;
            }
        }
    }

    public void DPS_Cycle()
    {
        if (Serpent_Sting.IsSpellUsable && Serpent_Sting.IsDistanceGood && Serpent_Sting.KnownSpell
            && MySettings.UseSerpentSting && !Serpent_Sting.TargetHaveBuff)
        {
            Serpent_Sting.Launch();
            Serpent_Sting_Timer = new Timer(1000*12);
            return;
        }
        else if (Cobra_Shot.KnownSpell && Cobra_Shot.IsSpellUsable && Cobra_Shot.IsDistanceGood
            && MySettings.UseCobraShot && Serpent_Sting_Timer.IsReady)
        {
            Cobra_Shot.Launch();
            Serpent_Sting_Timer = new Timer(1000*12);
            return;
        }
        else if (Kill_Shot.KnownSpell && Kill_Shot.IsSpellUsable && Kill_Shot.IsDistanceGood
            && MySettings.UseKillShot)
        {
            Kill_Shot.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 4 && MySettings.UseMultiShot && MySettings.UseExplosiveTrap
            && MySettings.UseExplosiveShot)
        {
            if (Multi_Shot.KnownSpell && Multi_Shot.IsSpellUsable && Multi_Shot.IsDistanceGood)
            {
                Multi_Shot.Launch();
                return;
            }
            else if (Explosive_Trap.KnownSpell && Explosive_Trap.IsSpellUsable && ObjectManager.Target.GetDistance < 10)
            {
                Explosive_Trap.Launch();
                return;
            }
            else
            {
                if (Explosive_Shot.KnownSpell && Explosive_Shot.IsSpellUsable && Explosive_Shot.IsDistanceGood)
                {
                    Explosive_Shot.Launch();
                    return;
                }
            }
        }
        else if (Explosive_Trap.KnownSpell && Explosive_Trap.IsSpellUsable && ObjectManager.Target.GetDistance < 10
            && MySettings.UseExplosiveTrap && ObjectManager.GetNumberAttackPlayer() < 4 && ObjectManager.GetNumberAttackPlayer() > 1)
        {
            Explosive_Trap.Launch();
            return;
        }
        else if (Black_Arrow.KnownSpell && Black_Arrow.IsSpellUsable && Black_Arrow.IsDistanceGood
            && MySettings.UseBlackArrow)
        {
            Black_Arrow.Launch();
            return;
        }
        else if (Explosive_Shot.KnownSpell && Explosive_Shot.IsSpellUsable && Explosive_Shot.IsDistanceGood
            && MySettings.UseExplosiveShot)
        {
            Explosive_Shot.Launch();
            return;
        }
        else if (Multi_Shot.KnownSpell && Multi_Shot.IsSpellUsable && Multi_Shot.IsDistanceGood
            && MySettings.UseMultiShot && ObjectManager.Me.FocusPercentage > 79 
            && ObjectManager.GetNumberAttackPlayer() < 4 && ObjectManager.GetNumberAttackPlayer() > 1)
        {
            Multi_Shot.Launch();
            return;
        }
        else if (Arcane_Shot.KnownSpell && Arcane_Shot.IsSpellUsable && Arcane_Shot.IsDistanceGood
            && MySettings.UseArcaneShot && ObjectManager.Me.FocusPercentage > 79)
        {
            Arcane_Shot.Launch();
            return;
        }
        else
        {
            if (Cobra_Shot.KnownSpell && Cobra_Shot.IsSpellUsable && Cobra_Shot.IsDistanceGood
                && MySettings.UseCobraShot && ObjectManager.Me.FocusPercentage < 80)
            {
                Cobra_Shot.Launch();
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

        Pet();

        if (MySettings.UseAspectoftheHawk && Aspect_of_the_Hawk.KnownSpell && Aspect_of_the_Hawk.IsSpellUsable 
            && !Aspect_of_the_Hawk.HaveBuff && !ObjectManager.Me.HaveBuff(109260))
        {
            Aspect_of_the_Hawk.Launch();
            return;
        }

        if (MySettings.UseCamouflage && Camouflage.KnownSpell && Camouflage.IsSpellUsable && !Camouflage.HaveBuff
            && !Fight.InFight && ObjectManager.GetNumberAttackPlayer() == 0)
        {
            Camouflage.Launch();
            return;
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (Exhilaration.KnownSpell && Exhilaration.IsSpellUsable
            && MySettings.UseExhilaration && ObjectManager.Me.HealthPercent < 70)
        {
            Exhilaration.Launch();
            return;
        }
        else if (ObjectManager.Pet.Health > 0 && ObjectManager.Pet.HealthPercent < 50
            && Feed_Pet.KnownSpell && Feed_Pet.IsSpellUsable && MySettings.UseFeedPet
            && !Fight.InFight && ObjectManager.GetNumberAttackPlayer() == 0)
        {
            Feed_Pet.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Pet.Health > 0 && ObjectManager.Pet.HealthPercent < 80
                && Mend_Pet.KnownSpell && Mend_Pet.IsSpellUsable && MySettings.UseMendPet
                && Mend_Pet_Timer.IsReady)
            {
                Mend_Pet.Launch();
                Mend_Pet_Timer = new Timer(1000*10);
                return;
            }
        }
    }

    private void Defense_Cycle()
    {
        if (ObjectManager.Me.HealthPercent < 20 && MySettings.UseFeignDeath
            && Feign_Death.KnownSpell && Feign_Death.IsSpellUsable)
        {
            Feign_Death.Launch();
            Thread.Sleep(5000);
            if (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0)
                return;
            else
                Thread.Sleep(5000);
        }
        else if (ObjectManager.Me.HealthPercent < 50 && MySettings.UseDeterrance
            && Deterrance.KnownSpell && Deterrance.IsSpellUsable)
        {
            Deterrance.Launch();
            Thread.Sleep(200);
        }
        else if (ObjectManager.Me.HealthPercent < 80 && MySettings.UseIceTrap
            && Ice_Trap.KnownSpell && Ice_Trap.IsSpellUsable && ObjectManager.Target.GetDistance < 10
            && Disengage.KnownSpell && Disengage.IsSpellUsable && MySettings.UseDisengage)
        {
            Ice_Trap.Launch();
            Thread.Sleep(1000);
            nManager.Wow.Helpers.Keybindings.PressKeybindings(nManager.Wow.Enums.Keybindings.JUMP);
            Disengage.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && MySettings.UseConcussiveShot 
            && Concussive_Shot.KnownSpell && Concussive_Shot.IsSpellUsable && Concussive_Shot.IsDistanceGood
            && Disengage.KnownSpell && Disengage.IsSpellUsable && MySettings.UseDisengage)
        {
            Concussive_Shot.Launch();
            Thread.Sleep(1000);
            nManager.Wow.Helpers.Keybindings.PressKeybindings(nManager.Wow.Enums.Keybindings.JUMP);
            Disengage.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && MySettings.UseBindingShot 
            && Binding_Shot.KnownSpell && Binding_Shot.IsSpellUsable && Binding_Shot.IsDistanceGood
            && Disengage.KnownSpell && Disengage.IsSpellUsable && MySettings.UseDisengage)
        {
            Binding_Shot.Launch();
            Thread.Sleep(1000);
            nManager.Wow.Helpers.Keybindings.PressKeybindings(nManager.Wow.Enums.Keybindings.JUMP);
            Disengage.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && War_Stomp.IsSpellUsable && War_Stomp.KnownSpell
            && MySettings.UseWarStomp)
        {
            War_Stomp.Launch();
            OnCD = new Timer(1000*2);
            return;
        }
        else
        {
            if (ObjectManager.Me.HealthPercent < 80 && Stoneform.IsSpellUsable && Stoneform.KnownSpell
                && MySettings.UseStoneform)
            {
                Stoneform.Launch();
                OnCD = new Timer(1000*8);
                return;
            }
        }
    }

    private void Decast()
    {
        if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && ObjectManager.Target.GetDistance < 8
            && Arcane_Torrent.KnownSpell && Arcane_Torrent.IsSpellUsable && MySettings.UseArcaneTorrent)
        {
            Arcane_Torrent.Launch();
            return;
        }
        else if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && Silencing_Shot.IsDistanceGood
            && Silencing_Shot.KnownSpell && Silencing_Shot.IsSpellUsable && MySettings.UseSilencingShot)
        {
            Silencing_Shot.Launch();
            return;
        }
        else if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && Scatter_Shot.IsDistanceGood
            && Scatter_Shot.KnownSpell && Scatter_Shot.IsSpellUsable && MySettings.UseScatterShot)
        {
            Scatter_Shot.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && MySettings.UseWyvernSting
                && Wyvern_Sting.KnownSpell && Wyvern_Sting.IsSpellUsable && Wyvern_Sting.IsDistanceGood)
            {
                Wyvern_Sting.Launch();
                return;
            }
        }
    }

    private void Pet()
    {
        if (!ObjectManager.Me.IsCast && (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0)
            && Call_Pet_1.KnownSpell && Call_Pet_1.IsSpellUsable && MySettings.UsePet1)
        {
            Call_Pet_1.Launch();
            Thread.Sleep(1000);
        }
        else if (!ObjectManager.Me.IsCast && (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0)
            && Call_Pet_2.KnownSpell && Call_Pet_2.IsSpellUsable && MySettings.UsePet2)
        {
            Call_Pet_2.Launch();
            Thread.Sleep(1000);
        }
        else if (!ObjectManager.Me.IsCast && (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0)
            && Call_Pet_3.KnownSpell && Call_Pet_3.IsSpellUsable && MySettings.UsePet3)
        {
            Call_Pet_3.Launch();
            Thread.Sleep(1000);
        }
        else if (!ObjectManager.Me.IsCast && (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0)
            && Call_Pet_4.KnownSpell && Call_Pet_4.IsSpellUsable && MySettings.UsePet4)
        {
            Call_Pet_4.Launch();
            Thread.Sleep(1000);
        }
        else
        {
            if (!ObjectManager.Me.IsCast && (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0)
                && Call_Pet_5.KnownSpell && Call_Pet_5.IsSpellUsable && MySettings.UsePet5)
            {
                Call_Pet_5.Launch();
                Thread.Sleep(1000);
            }
        }

        if (!ObjectManager.Me.IsCast && (!ObjectManager.Pet.IsAlive || ObjectManager.Pet.Guid == 0)
            && Revive_Pet.KnownSpell && Revive_Pet.IsSpellUsable && MySettings.UseRevivePet
            && !Fight.InFight && ObjectManager.GetNumberAttackPlayer() == 0)
        {
            Revive_Pet.Launch();
            Thread.Sleep(1000);
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

public class Hunter
{
    #region InitializeSpell

    private readonly Spell Arcane_Shot = new Spell("Arcane Shot");
    private readonly Spell Call_Pet_1 = new Spell("Call Pet 1");
    private readonly Spell Revive_Pet = new Spell("Revive Pet");
    private readonly Spell Serpent_Sting = new Spell("Serpent Sting");
    private readonly Spell Steady_Shot = new Spell("Steady Shot");

    #endregion InitializeSpell

    public Hunter()
    {
        Main.range = 30.0f;
        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            if (!ObjectManager.Me.IsMounted)
            {
                if (!ObjectManager.Me.IsCast)
                    Patrolling();

                if (Fight.InFight && ObjectManager.Me.Target > 0)
                {
                    if (ObjectManager.Me.Target != lastTarget && Arcane_Shot.IsDistanceGood)
                        lastTarget = ObjectManager.Me.Target;

                    Combat();
                }
            }
            Thread.Sleep(350);
        }
    }

    public void Combat()
    {
        AvoidMelee();

        if (Serpent_Sting.KnownSpell && Serpent_Sting.IsSpellUsable && Serpent_Sting.IsDistanceGood
            && !Serpent_Sting.TargetHaveBuff)
        {
            Serpent_Sting.Launch();
            return;
        }
        else if (Arcane_Shot.IsSpellUsable && Arcane_Shot.IsDistanceGood && Arcane_Shot.KnownSpell)
        {
            Arcane_Shot.Launch();
            return;
        }
        else
        {
            if (Steady_Shot.KnownSpell && Steady_Shot.IsSpellUsable && Steady_Shot.IsDistanceGood)
            {
                Steady_Shot.Launch();
                return;
            }
        }
    }


    private void Pet()
    {
        if (!ObjectManager.Me.IsCast && (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0)
            && Call_Pet_1.KnownSpell && Call_Pet_1.IsSpellUsable)
        {
            Call_Pet_1.Launch();
            Thread.Sleep(1000);
        }

        if (!ObjectManager.Me.IsCast && (!ObjectManager.Pet.IsAlive || ObjectManager.Pet.Guid == 0)
            && Revive_Pet.KnownSpell && Revive_Pet.IsSpellUsable
            && !Fight.InFight && ObjectManager.GetNumberAttackPlayer() == 0)
        {
            Revive_Pet.Launch();
            Thread.Sleep(1000);
        }
    }


    private void AvoidMelee()
    {
        if (ObjectManager.Target.GetDistance < 1)
        {
            Keyboard.DownKey(nManager.Wow.Memory.WowProcess.MainWindowHandle, "{DOWN}");
        }
    }

    public void Patrolling()
    {
    }
}

#endregion