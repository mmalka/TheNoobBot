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
                    #region RogueSpecialisation checking

                case WoWClass.Rogue:
                    var Rogue_Combat_Spell = new Spell("Blade Flurry");
                    var Rogue_Assassination_Spell = new Spell("Mutilate");
                    var Rogue_Subtlety_Spell = new Spell("Master of Subtlety");

                    if (Rogue_Combat_Spell.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Rogue_Combat.xml";
                            Rogue_Combat.RogueCombatSettings CurrentSetting;
                            CurrentSetting = new Rogue_Combat.RogueCombatSettings();
                            if (System.IO.File.Exists(CurrentSettingsFile))
                            {
                                CurrentSetting =
                                    Settings.Load<Rogue_Combat.RogueCombatSettings>(CurrentSettingsFile);
                            }
                            CurrentSetting.ToForm();
                            CurrentSetting.Save(CurrentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Rogue Combat class...");
                            new Rogue_Combat();
                        }
                        break;
                    }

                    else if (Rogue_Assassination_Spell.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Rogue_Assassination.xml";
                            Rogue_Assassination.RogueAssassinationSettings CurrentSetting;
                            CurrentSetting = new Rogue_Assassination.RogueAssassinationSettings();
                            if (System.IO.File.Exists(CurrentSettingsFile))
                            {
                                CurrentSetting =
                                    Settings.Load<Rogue_Assassination.RogueAssassinationSettings>(CurrentSettingsFile);
                            }
                            CurrentSetting.ToForm();
                            CurrentSetting.Save(CurrentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Rogue Assassination class...");
                            new Rogue_Assassination();
                        }
                        break;
                    }

                    else if (Rogue_Subtlety_Spell.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Rogue_Subtlety.xml";
                            Rogue_Subtlety.RogueSubtletySettings CurrentSetting;
                            CurrentSetting = new Rogue_Subtlety.RogueSubtletySettings();
                            if (System.IO.File.Exists(CurrentSettingsFile))
                            {
                                CurrentSetting =
                                    Settings.Load<Rogue_Subtlety.RogueSubtletySettings>(CurrentSettingsFile);
                            }
                            CurrentSetting.ToForm();
                            CurrentSetting.Save(CurrentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Rogue Subtlety class...");
                            new Rogue_Subtlety();
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
                            Logging.WriteFight("Rogue without Spec");
                            new Rogue_Combat();
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

#region Rogue

public class Rogue_Combat
{
    [Serializable]
    public class RogueCombatSettings : nManager.Helpful.Settings
    {
        /* Professions & Racials */
        public bool UseArcaneTorrent = true;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseLifeblood = true;
        public bool UseStoneform = true;
        public bool UseGiftoftheNaaru = true;
        public bool UseWarStomp = true;
        /* Rogue Buffs */
        public bool UseBladeFlurry = true;
        public bool UseBurstofSpeed = true;
        public bool UseCripplingPoison = false;
        public bool UseDeadlyPoison = true;
        public bool UseLeechingPoison = true;
        public bool UseMindnumbingPoison = true;
        public bool UseParalyticPoison = false;
        public bool UseSliceandDice = true;
        public bool UseSprint = true;
        public bool UseStealth = false;
        public bool UseWoundPoison = false;
        /* Offensive Spell */
        public bool UseAmbush = true;
        public bool UseCrimsonTempest = true;
        public bool UseDeadlyThrow  = true;
        public bool UseEviscerate = true;
        public bool UseExposeArmor = false;
        public bool UseFanofKnives = true;
        public bool UseGarrote = true;
        public bool UseRevealingStrike = true;
        public bool UseRupture = true;
        public bool UseShiv = true;
        public bool UseShurikenToss = true;
        public bool UseSinisterStrike = true;
        public bool UseThrow = true;
        /* Offensive Cooldown */
        public bool UseAdrenalineRush = true;
        public bool UseKillingSpree = true;
        public bool UseRedirect = true;
        public bool UseShadowBlades = true;
        public bool UseShadowStep = true;
        /* Defensive Cooldown */
        public bool UseCheapShot = true;
        public bool UseCloakofShadows= true;
        public bool UseCombatReadiness = true;
        public bool UseDismantle = true;
        public bool UseEvasion = true;
        public bool UseKick = true;
        public bool UseKidneyShot = true;
        public bool UsePreparation = true;
        public bool UseSmokeBomb = true;
        public bool UseVanish = true;
        /* Healing Spell */
        public bool UseRecuperate = true;
        /* Game Settings */
        public bool UseLowCombat = true;
        public bool UseTrinket = true;
        public bool UseEngGlove = true;
        public bool UseAlchFlask = true;

        public RogueCombatSettings()
        {
            ConfigWinForm(new System.Drawing.Point(400, 400), "Rogue Combat Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");
            AddControlInWinForm("Use Lifeblood", "UseLifeblood", "Professions & Racials");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            /* Rogue Buffs */
            AddControlInWinForm("Use Blade Flurry", "UseBladeFlurry", "Rogue Buffs");
            AddControlInWinForm("Use Burst of Speed", "UseBurstofSpeed", "Rogue Buffs");
            AddControlInWinForm("Use Crippling Poison", "UseCripplingPoison", "Rogue Buffs");
            AddControlInWinForm("Use Deadly Poison", "UseDeadlyPoison", "Rogue Buffs");
            AddControlInWinForm("Use Leeching Poison", "UseLeechingPoison", "Rogue Buffs");
            AddControlInWinForm("Use Mindnumbing Poison", "UseMindnumbingPoison", "Rogue Buffs");
            AddControlInWinForm("Use Paralytic Poison", "UseParalyticPoison", "Rogue Buffs");
            AddControlInWinForm("Use Slice and Dice", "UseSliceandDice", "Rogue Buffs");
            AddControlInWinForm("Use Sprint", "UseSprint", "Rogue Buffs");
            AddControlInWinForm("Use Stealth", "UseStealth", "Rogue Buffs");
            AddControlInWinForm("Use Wound Poison", "UseWoundPoison", "Rogue Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Ambush", "UseAmbush", "Offensive Spell");
            AddControlInWinForm("Use Crimson Tempest", "UseCrimsonTempest", "Offensive Spell");
            AddControlInWinForm("Use Deadly Throw", "UseDeadlyThrow", "Offensive Spell");
            AddControlInWinForm("Use Eviscerate", "UseEviscerate", "Offensive Spell");
            AddControlInWinForm("Use Expose Armor", "UseExposeArmor", "Offensive Spell");
            AddControlInWinForm("Use Fan of Knives", "UseFanofKnives", "Offensive Spell");
            AddControlInWinForm("Use Garrote", "UseGarrote", "Offensive Spell");
            AddControlInWinForm("Use Revealing Strike", "UseRevealingStrike", "Offensive Spell");
            AddControlInWinForm("Use Rupture", "UseRupture", "Offensive Spell");
            AddControlInWinForm("Use Shiv", "UseShiv", "Offensive Spell");
            AddControlInWinForm("Use Shuriken Toss", "UseShurikenToss", "Offensive Spell");
            AddControlInWinForm("Use Sinister Strike", "UseSinisterStrike", "Offensive Spell");
            AddControlInWinForm("Use Throw", "UseThrow", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use Adrenaline Rush", "UseAdrenalineRush", "Offensive Cooldown");
            AddControlInWinForm("Use Killing Spree", "UseKillingSpree", "Offensive Cooldown");
            AddControlInWinForm("Use Redirect", "UseRedirect", "Offensive Cooldown");
            AddControlInWinForm("Use Shadow Blades", "UseShadowBlades", "Offensive Cooldown");
            AddControlInWinForm("Use Shadow Step", "UseShadowStep", "Offensive Cooldown");
            AddControlInWinForm("Use Vendetta", "UseVendetta", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use CheapShot", "UseCheapShot", "Defensive Cooldown");
            AddControlInWinForm("Use CloakofShadows", "UseCloakofShadows", "Defensive Cooldown");
            AddControlInWinForm("Use CombatReadiness", "UseCombatReadiness", "Defensive Cooldown");
            AddControlInWinForm("Use Dismantle", "UseDismantle", "Defensive Cooldown");
            AddControlInWinForm("Use Evasion", "UseEvasion", "Defensive Cooldown");
            AddControlInWinForm("Use Kick", "UseKick", "Defensive Cooldown");
            AddControlInWinForm("Use KidneyShot", "UseKidneyShot", "Defensive Cooldown");
            AddControlInWinForm("Use Preparation", "UsePreparation", "Defensive Cooldown");
            AddControlInWinForm("Use SmokeBomb", "UseSmokeBomb", "Defensive Cooldown");
            AddControlInWinForm("Use Vanish", "UseVanish", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Recuperate", "UseRecuperate", "Healing Spell");
            /* Game Settings */
            AddControlInWinForm("Use Low Combat Settings", "UseLowCombat", "Game Settings");
            AddControlInWinForm("Use Trinket", "UseTrinket", "Game Settings");
            AddControlInWinForm("Use Engineering Gloves", "UseEngGlove", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
        }

        public static RogueCombatSettings CurrentSetting { get; set; }

        public static RogueCombatSettings GetSettings()
        {
            string CurrentSettingsFile = Application.StartupPath + "\\CustomClasses\\Settings\\Rogue_Combat.xml";
            if (System.IO.File.Exists(CurrentSettingsFile))
            {
                return
                    CurrentSetting = Settings.Load<Rogue_Combat.RogueCombatSettings>(CurrentSettingsFile);
            }
            else
            {
                return new Rogue_Combat.RogueCombatSettings();
            }
        }
    }

    private readonly RogueCombatSettings MySettings = RogueCombatSettings.GetSettings();
    
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

    #region Rogue Buffs

    private readonly Spell Blade_Flurry = new Spell("Blade Flurry");
    private readonly Spell Burst_of_Speed = new Spell("Burst of Speed");
    private readonly Spell Crippling_Poison = new Spell("Crippling Poison");
    private readonly Spell Deadly_Poison = new Spell("Deadly Poison");
    private readonly Spell Leeching_Poison = new Spell("Leeching Poison");
    private readonly Spell Mindnumbing_Poison = new Spell("Mind-numbing Poison");
    private readonly Spell Paralytic_Poison = new Spell("Paralytic Poison");
    private readonly Spell Slice_and_Dice = new Spell("Slice and Dice");
    private Timer Slice_and_Dice_Timer = new Timer(0);
    private readonly Spell Sprint = new Spell("Sprint");
    private readonly Spell Stealth = new Spell("Stealth");
    private readonly Spell Wound_Poison = new Spell("Wound Poison");

    #endregion

    #region Offensive Spell

    private readonly Spell Ambush = new Spell("Ambush");
    private readonly Spell Crimson_Tempest = new Spell("Crimson Tempest");
    private readonly Spell Deadly_Throw = new Spell("Deadly Throw");
    private readonly Spell Eviscerate = new Spell("Eviscerate");
    private readonly Spell Expose_Armor = new Spell("Expose Armor");
    private readonly Spell Fan_of_Knives = new Spell("Fan of Knives");
    private readonly Spell Garrote = new Spell("Garrote");
    private readonly Spell Revealing_Strike = new Spell("Revealing Strike");
    private readonly Spell Rupture = new Spell("Rupture");
    private Timer Rupture_Timer = new Timer(0);
    private readonly Spell Shiv = new Spell("Shiv");
    private readonly Spell Shuriken_Toss = new Spell("Shuriken Toss");
    private readonly Spell Sinister_Strike = new Spell("Sinister Strike");
    private readonly Spell Throw = new Spell("Throw");

    #endregion

    #region Offensive Cooldown

    private readonly Spell Adrenaline_Rush = new Spell("Adrenaline Rush");
    private readonly Spell Killing_Spree = new Spell("Killing Spree");
    private readonly Spell Redirect = new Spell("Redirect");
    private readonly Spell Shadow_Blades = new Spell("Shadow Blades");
    private readonly Spell Shadow_Step = new Spell("Shadow Step");
    private readonly Spell Vendetta = new Spell("Vendetta");

    #endregion

    #region Defensive Cooldown

    private readonly Spell Cheap_Shot = new Spell("Cheap Shot");
    private readonly Spell Cloak_of_Shadows = new Spell("Cloak of Shadows");
    private readonly Spell Combat_Readiness = new Spell("Combat Readiness");
    private readonly Spell Dismantle = new Spell("Dismantle");
    private Timer Dismantle_Timer = new Timer(0);
    private readonly Spell Evasion = new Spell("Evasion");
    private readonly Spell Kick = new Spell("Kick");
    private readonly Spell Kidney_Shot = new Spell("Kidney Shot");
    private readonly Spell Preparation = new Spell("Preparation");
    private readonly Spell Smoke_Bomb = new Spell("Smoke Bomb");
    private readonly Spell Vanish = new Spell("Vanish");

    #endregion

    #region Healing Spell

    private readonly Spell Recuperate = new Spell("Recuperate");

    #endregion

    private Timer OnCD = new Timer(0);
    private Timer Trinket_Timer = new Timer(0);
    private Timer Engineering_Timer = new Timer(0);
    private Timer AlchFlask_Timer = new Timer(0);
    public int LC = 0;
    public int CP = 0;

    public Rogue_Combat()
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
                            (Throw.IsDistanceGood || Cheap_Shot.IsDistanceGood))
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
                        if (ObjectManager.Me.IsCast)
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
        if (Redirect.IsSpellUsable && Redirect.IsDistanceGood && Redirect.KnownSpell
            && MySettings.UseRedirect && ObjectManager.Me.ComboPoint > 0)
        {
            Redirect.Launch();
            Thread.Sleep(200);
        }

        if (((Stealth.KnownSpell && Stealth.IsSpellUsable && !Stealth.HaveBuff && MySettings.UseStealth)
            || Stealth.HaveBuff) && LC != 1)
        {
            if (!Stealth.HaveBuff)
            {
                Stealth.Launch();
                Thread.Sleep(200);
            }

            if (Shadow_Step.IsSpellUsable && Shadow_Step.IsDistanceGood && Shadow_Step.KnownSpell
                && MySettings.UseShadowStep)
            {
                Shadow_Step.Launch();
                Thread.Sleep(200);
            }

            if (Garrote.IsSpellUsable && Garrote.IsDistanceGood && Garrote.KnownSpell
                && MySettings.UseGarrote)
            {
                Garrote.Launch();
                return;
            }
            else
            {
                if (Cheap_Shot.IsSpellUsable && Cheap_Shot.IsDistanceGood && Cheap_Shot.KnownSpell
                    && MySettings.UseCheapShot)
                {
                    Cheap_Shot.Launch();
                    return;
                }
            }
        }
        else if (Shuriken_Toss.IsSpellUsable && Shuriken_Toss.IsDistanceGood && Shuriken_Toss.KnownSpell
                && MySettings.UseShurikenToss && !MySettings.UseStealth)
        {
            Shuriken_Toss.Launch();
            return;
        }
        else
        {
            if (Throw.IsSpellUsable && Throw.IsDistanceGood && Throw.KnownSpell
                && MySettings.UseThrow && !MySettings.UseStealth)
            {
                MovementManager.StopMove();
                Throw.Launch();
                Thread.Sleep(1000);
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

        if (Throw.IsSpellUsable && Throw.IsDistanceGood && Throw.KnownSpell && !ObjectManager.Target.InCombat
            && MySettings.UseThrow)
        {
            Throw.Launch();
            return;
        }

        if (Eviscerate.KnownSpell && Eviscerate.IsSpellUsable && Eviscerate.IsDistanceGood
            && MySettings.UseEviscerate && ObjectManager.Me.ComboPoint > 4)
        {
            Eviscerate.Launch();
            return;
        }
        else if (Revealing_Strike.KnownSpell && Revealing_Strike.IsSpellUsable && Revealing_Strike.IsDistanceGood
            && MySettings.UseRevealingStrike)
        {
            Revealing_Strike.Launch();
            return;
        }
        else if (Sinister_Strike.KnownSpell && Sinister_Strike.IsSpellUsable && Sinister_Strike.IsDistanceGood
            && MySettings.UseSinisterStrike)
        {
            Sinister_Strike.Launch();
            return;
        }
        else
        {
            if (Slice_and_Dice.KnownSpell && Slice_and_Dice.IsSpellUsable && Slice_and_Dice.IsDistanceGood
                && MySettings.UseSliceandDice && !Slice_and_Dice.HaveBuff)
            {
                CP = ObjectManager.Me.ComboPoint;
                Slice_and_Dice.Launch();
                Slice_and_Dice_Timer = new Timer(1000*(6+(CP*6)));
                return;
            }
        }

        if (Fan_of_Knives.KnownSpell && Fan_of_Knives.IsSpellUsable && Fan_of_Knives.IsDistanceGood
            && MySettings.UseFanofKnives)
        {
            Fan_of_Knives.Launch();
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
        else if (Adrenaline_Rush.KnownSpell && Adrenaline_Rush.IsSpellUsable
            && MySettings.UseAdrenalineRush && ObjectManager.Target.GetDistance < 30)
        {
            Adrenaline_Rush.Launch();
            return;
        }
        else if (Killing_Spree.KnownSpell && Killing_Spree.IsSpellUsable
            && MySettings.UseKillingSpree && ObjectManager.Target.GetDistance < 10
            && ObjectManager.Me.EnergyPercentage < 35)
        {
            Killing_Spree.Launch();
            return;
        }
        else
        {
            if (Shadow_Blades.KnownSpell && Shadow_Blades.IsSpellUsable
                && MySettings.UseShadowBlades && ObjectManager.Target.GetDistance < 30)
            {
                Shadow_Blades.Launch();
                return;
            }
        }
    }

    public void DPS_Cycle()
    {
        if (Garrote.IsSpellUsable && Garrote.IsDistanceGood && Garrote.KnownSpell
            && MySettings.UseGarrote && ObjectManager.Me.HaveBuff(115192))
        {
            Garrote.Launch();
            return;
        }

        if (Throw.IsSpellUsable && Throw.IsDistanceGood && Throw.KnownSpell && !ObjectManager.Target.InCombat
            && MySettings.UseThrow)
        {
            Throw.Launch();
            return;
        }

        if (Blade_Flurry.KnownSpell && Blade_Flurry.IsSpellUsable && ObjectManager.Target.GetDistance < 10
            && MySettings.UseBladeFlurry && !Blade_Flurry.HaveBuff && ObjectManager.GetNumberAttackPlayer() > 1)
        {
            Blade_Flurry.Launch();
            return;
        }
        else
        {
            if (Blade_Flurry.KnownSpell && Blade_Flurry.IsSpellUsable && Sinister_Strike.IsDistanceGood
                && Blade_Flurry.HaveBuff && ObjectManager.GetNumberAttackPlayer() < 2)
            {
                Blade_Flurry.Launch();
                return;
            }
        }

        if (Eviscerate.KnownSpell && Eviscerate.IsSpellUsable && Eviscerate.IsDistanceGood
            && MySettings.UseEviscerate && ObjectManager.Me.ComboPoint > 4)
        {
            Eviscerate.Launch();
            return;
        }
        else if (Revealing_Strike.KnownSpell && Revealing_Strike.IsSpellUsable && Revealing_Strike.IsDistanceGood
            && MySettings.UseRevealingStrike && !Revealing_Strike.TargetHaveBuff)
        {
            Revealing_Strike.Launch();
            return;
        }
        else if (Slice_and_Dice.KnownSpell && Slice_and_Dice.IsSpellUsable && Slice_and_Dice.IsDistanceGood
            && MySettings.UseSliceandDice && !Slice_and_Dice.HaveBuff)
        {
            CP = ObjectManager.Me.ComboPoint;
            Slice_and_Dice.Launch();
            Slice_and_Dice_Timer = new Timer(1000*(6+(CP*6)));
            return;
        }
        else if (Rupture.KnownSpell && Rupture.IsDistanceGood && Rupture.IsSpellUsable
            && MySettings.UseRupture && (!Rupture.TargetHaveBuff || Rupture_Timer.IsReady))
        {
            CP = ObjectManager.Me.ComboPoint;
            Rupture.Launch();
            Rupture_Timer = new Timer(1000*(4+(CP*4)));
            return;
        }
        else if (Expose_Armor.IsSpellUsable && Expose_Armor.IsDistanceGood && Expose_Armor.KnownSpell
            && MySettings.UseExposeArmor && !ObjectManager.Target.HaveBuff(113746))
        {
            Expose_Armor.Launch();
            return;
        }
        else
        {
            if (Sinister_Strike.KnownSpell && Sinister_Strike.IsSpellUsable && Sinister_Strike.IsDistanceGood
                && MySettings.UseSinisterStrike)
            {
                Sinister_Strike.Launch();
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

        if (MySettings.UseDeadlyPoison && Deadly_Poison.KnownSpell && Deadly_Poison.IsSpellUsable
            && !Deadly_Poison.HaveBuff)
        {
            Deadly_Poison.Launch();
            return;
        }
        else
        {
            if (!Wound_Poison.HaveBuff && Wound_Poison.KnownSpell && Wound_Poison.IsSpellUsable
                && MySettings.UseWoundPoison && !Deadly_Poison.HaveBuff)
            {
                Wound_Poison.Launch();
                return;
            }
        }

        if (!Leeching_Poison.HaveBuff && Leeching_Poison.KnownSpell && Leeching_Poison.IsSpellUsable
            && MySettings.UseLeechingPoison)
        {
            Leeching_Poison.Launch();
            return;
        }
        else if (!Paralytic_Poison.HaveBuff && Paralytic_Poison.KnownSpell && Paralytic_Poison.IsSpellUsable
            && MySettings.UseParalyticPoison && !Leeching_Poison.HaveBuff)
        {
            Paralytic_Poison.Launch();
            return;
        }
        else if (!Crippling_Poison.HaveBuff && Crippling_Poison.KnownSpell && Crippling_Poison.IsSpellUsable
            && MySettings.UseCripplingPoison && !Leeching_Poison.HaveBuff && Paralytic_Poison.HaveBuff)
        {
            Crippling_Poison.Launch();
            return;
        }
        else
        {
            if (!Mindnumbing_Poison.HaveBuff && Mindnumbing_Poison.KnownSpell && Mindnumbing_Poison.IsSpellUsable
                && MySettings.UseMindnumbingPoison && !Crippling_Poison.HaveBuff && !Paralytic_Poison.HaveBuff
                && !Leeching_Poison.HaveBuff)
            {
                Mindnumbing_Poison.Launch();
                return;
            }
        }

        if (ObjectManager.GetNumberAttackPlayer() == 0 && Burst_of_Speed.IsSpellUsable && Burst_of_Speed.KnownSpell
            && MySettings.UseBurstofSpeed && !ObjectManager.Target.IsLootable
            && ObjectManager.Me.GetMove)
        {
            Burst_of_Speed.Launch();
            return;
        }
        else
        {
            if (ObjectManager.GetNumberAttackPlayer() == 0 && Sprint.IsSpellUsable && Sprint.KnownSpell
                && MySettings.UseSprint && !ObjectManager.Target.IsLootable
                && ObjectManager.Me.GetMove)
            {
                Sprint.Launch();
                return;
            }
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (!Recuperate.HaveBuff && ObjectManager.Me.ComboPoint > 1 && MySettings.UseRecuperate
            && ObjectManager.Me.HealthPercent <= 90 && Recuperate.KnownSpell && Recuperate.IsSpellUsable)
        {
            Recuperate.Launch();
            return;
        }
    }

    private void Defense_Cycle()
    {
        if (ObjectManager.Me.HealthPercent <= 10 && Vanish.KnownSpell && Vanish.IsSpellUsable
            && MySettings.UseVanish)
        {
            Vanish.Launch();
            Thread.Sleep(5000);
            OnCD = new Timer(1000*20);
            return;
        }
        else if (ObjectManager.Me.HealthPercent <= 80 && !Kidney_Shot.TargetHaveBuff && Kidney_Shot.KnownSpell
            && Kidney_Shot.IsSpellUsable && Kidney_Shot.IsDistanceGood && ObjectManager.Me.ComboPoint <= 3
            && Recuperate.HaveBuff && MySettings.UseKidneyShot)
        {
            CP = ObjectManager.Me.ComboPoint;
            Kidney_Shot.Launch();
            OnCD = new Timer(1000*CP);
            return;
        }
        else if (ObjectManager.Me.HealthPercent <= 80 && Evasion.KnownSpell && Evasion.IsSpellUsable
            && MySettings.UseEvasion)
        {
            Evasion.Launch();
            OnCD = new Timer(1000*15);
            return;
        }
        else if (ObjectManager.Me.HealthPercent <= 90 && Combat_Readiness.KnownSpell && Combat_Readiness.IsSpellUsable
            && MySettings.UseCombatReadiness)
        {
            Combat_Readiness.Launch();
            OnCD = new Timer(1000*20);
            return;
        }
        else if (ObjectManager.Me.HealthPercent <= 95 && Dismantle.KnownSpell && Dismantle.IsSpellUsable
            && MySettings.UseDismantle)
        {
            Dismantle.Launch();
            Dismantle_Timer = new Timer(1000*60);
            return;
        }
        else
        {
            if (ObjectManager.GetNumberAttackPlayer() >= 3 && Vanish.KnownSpell && Vanish.IsSpellUsable
                && MySettings.UseVanish)
            {
                Vanish.Launch();
                Thread.Sleep(5000);
                return;
            }
        }

        if (ObjectManager.Me.HealthPercent <= 70 && Preparation.KnownSpell && Preparation.IsSpellUsable
            && MySettings.UsePreparation && !Evasion.IsSpellUsable)
        {
            Preparation.Launch();
            return;
        }
    }

    private void Decast()
    {
        if (ObjectManager.Target.IsCast && Kick.KnownSpell && Kick.IsSpellUsable 
            && Kick.IsDistanceGood && MySettings.UseKick && ObjectManager.Target.IsTargetingMe)
        {
            Kick.Launch();
            return;
        }
        else if (ObjectManager.Target.IsCast && Cloak_of_Shadows.KnownSpell && Cloak_of_Shadows.IsSpellUsable
            && ObjectManager.Target.IsTargetingMe && MySettings.UseCloakofShadows)
        {
            Cloak_of_Shadows.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Target.IsCast && Smoke_Bomb.KnownSpell && Smoke_Bomb.IsSpellUsable
                && ObjectManager.Target.IsTargetingMe && MySettings.UseSmokeBomb
                && !Cloak_of_Shadows.HaveBuff)
            {
                Smoke_Bomb.Launch();
                return;
            }
        }

        if (ObjectManager.Me.HealthPercent <= 70 && Preparation.KnownSpell && Preparation.IsSpellUsable
            && MySettings.UsePreparation && !Cloak_of_Shadows.IsSpellUsable && ObjectManager.Target.IsCast
            && ObjectManager.Target.IsTargetingMe)
        {
            Preparation.Launch();
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

public class Rogue_Subtlety
{
    [Serializable]
    public class RogueSubtletySettings : nManager.Helpful.Settings
    {
        /* Professions & Racials */
        public bool UseArcaneTorrent = true;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseLifeblood = true;
        public bool UseStoneform = true;
        public bool UseGiftoftheNaaru = true;
        public bool UseWarStomp = true;
        /* Rogue Buffs */
        public bool UseBurstofSpeed = true;
        public bool UseCripplingPoison = false;
        public bool UseDeadlyPoison = true;
        public bool UseLeechingPoison = true;
        public bool UseMindnumbingPoison = true;
        public bool UseParalyticPoison = false;
        public bool UseSliceandDice = true;
        public bool UseSprint = true;
        public bool UseStealth = false;
        public bool UseWoundPoison = false;
        /* Offensive Spell */
        public bool UseAmbush = true;
        public bool UseCrimsonTempest = true;
        public bool UseDeadlyThrow  = true;
        public bool UseExposeArmor = false;
        public bool UseFanofKnives = true;
        public bool UseEviscerate = true;
        public bool UseGarrote = true;
        public bool UseHemorrhage = true;
        public bool UseRupture = true;
        public bool UseShiv = true;
        public bool UseShurikenToss = true;
        public bool UseThrow = true;
        /* Offensive Cooldown */
        public bool UsePremeditation = true;
        public bool UseRedirect = true;
        public bool UseShadowBlades = true;
        public bool UseShadowDance = true;
        public bool UseShadowStep = true;
        /* Defensive Cooldown */
        public bool UseCheapShot = true;
        public bool UseCloakofShadows= true;
        public bool UseCombatReadiness = true;
        public bool UseDismantle = true;
        public bool UseEvasion = true;
        public bool UseKick = true;
        public bool UseKidneyShot = true;
        public bool UsePreparation = true;
        public bool UseSmokeBomb = true;
        public bool UseVanish = true;
        /* Healing Spell */
        public bool UseRecuperate = true;
        /* Game Settings */
        public bool UseLowCombat = true;
        public bool UseTrinket = true;
        public bool UseEngGlove = true;
        public bool UseAlchFlask = true;

        public RogueSubtletySettings()
        {
            ConfigWinForm(new System.Drawing.Point(400, 400), "Rogue Subtlety Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");
            AddControlInWinForm("Use Lifeblood", "UseLifeblood", "Professions & Racials");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            /* Rogue Buffs */
            AddControlInWinForm("Use Burst of Speed", "UseBurstofSpeed", "Rogue Buffs");
            AddControlInWinForm("Use Crippling Poison", "UseCripplingPoison", "Rogue Buffs");
            AddControlInWinForm("Use Deadly Poison", "UseDeadlyPoison", "Rogue Buffs");
            AddControlInWinForm("Use Leeching Poison", "UseLeechingPoison", "Rogue Buffs");
            AddControlInWinForm("Use Mindnumbing Poison", "UseMindnumbingPoison", "Rogue Buffs");
            AddControlInWinForm("Use Paralytic Poison", "UseParalyticPoison", "Rogue Buffs");
            AddControlInWinForm("Use Slice and Dice", "UseSliceandDice", "Rogue Buffs");
            AddControlInWinForm("Use Sprint", "UseSprint", "Rogue Buffs");
            AddControlInWinForm("Use Stealth", "UseStealth", "Rogue Buffs");
            AddControlInWinForm("Use Wound Poison", "UseWoundPoison", "Rogue Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Ambush", "UseAmbush", "Offensive Spell");
            AddControlInWinForm("Use Crimson Tempest", "UseCrimsonTempest", "Offensive Spell");
            AddControlInWinForm("Use Deadly Throw", "UseDeadlyThrow", "Offensive Spell");
            AddControlInWinForm("Use Expose Armor", "UseExposeArmor", "Offensive Spell");
            AddControlInWinForm("Use Fan of Knives", "UseFanofKnives", "Offensive Spell");
            AddControlInWinForm("Use Eviscerate", "UseEviscerate", "Offensive Spell");
            AddControlInWinForm("Use Garrote", "UseGarrote", "Offensive Spell");
            AddControlInWinForm("Use Hemorrhage", "UseHemorrhage", "Offensive Spell");
            AddControlInWinForm("Use Rupture", "UseRupture", "Offensive Spell");
            AddControlInWinForm("Use Shiv", "UseShiv", "Offensive Spell");
            AddControlInWinForm("Use Shuriken Toss", "UseShurikenToss", "Offensive Spell");
            AddControlInWinForm("Use Throw", "UseThrow", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use Premeditation", "UsePremeditation", "Offensive Cooldown");
            AddControlInWinForm("Use Redirect", "UseRedirect", "Offensive Cooldown");
            AddControlInWinForm("Use Shadow Blades", "UseShadowBlades", "Offensive Cooldown");
            AddControlInWinForm("Use Shadow Dance", "UseShadowDance", "Offensive Cooldown");
            AddControlInWinForm("Use Shadow Step", "UseShadowStep", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use CheapShot", "UseCheapShot", "Defensive Cooldown");
            AddControlInWinForm("Use CloakofShadows", "UseCloakofShadows", "Defensive Cooldown");
            AddControlInWinForm("Use CombatReadiness", "UseCombatReadiness", "Defensive Cooldown");
            AddControlInWinForm("Use Dismantle", "UseDismantle", "Defensive Cooldown");
            AddControlInWinForm("Use Evasion", "UseEvasion", "Defensive Cooldown");
            AddControlInWinForm("Use Kick", "UseKick", "Defensive Cooldown");
            AddControlInWinForm("Use KidneyShot", "UseKidneyShot", "Defensive Cooldown");
            AddControlInWinForm("Use Preparation", "UsePreparation", "Defensive Cooldown");
            AddControlInWinForm("Use SmokeBomb", "UseSmokeBomb", "Defensive Cooldown");
            AddControlInWinForm("Use Vanish", "UseVanish", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Recuperate", "UseRecuperate", "Healing Spell");
            /* Game Settings */
            AddControlInWinForm("Use Low Combat Settings", "UseLowCombat", "Game Settings");
            AddControlInWinForm("Use Trinket", "UseTrinket", "Game Settings");
            AddControlInWinForm("Use Engineering Gloves", "UseEngGlove", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
        }

        public static RogueSubtletySettings CurrentSetting { get; set; }

        public static RogueSubtletySettings GetSettings()
        {
            string CurrentSettingsFile = Application.StartupPath + "\\CustomClasses\\Settings\\Rogue_Subtlety.xml";
            if (System.IO.File.Exists(CurrentSettingsFile))
            {
                return
                    CurrentSetting = Settings.Load<Rogue_Subtlety.RogueSubtletySettings>(CurrentSettingsFile);
            }
            else
            {
                return new Rogue_Subtlety.RogueSubtletySettings();
            }
        }
    }

    private readonly RogueSubtletySettings MySettings = RogueSubtletySettings.GetSettings();
    
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

    #region Rogue Buffs

    private readonly Spell Burst_of_Speed = new Spell("Burst of Speed");
    private readonly Spell Crippling_Poison = new Spell("Crippling Poison");
    private readonly Spell Deadly_Poison = new Spell("Deadly Poison");
    private readonly Spell Leeching_Poison = new Spell("Leeching Poison");
    private readonly Spell Mindnumbing_Poison = new Spell("Mind-numbing Poison");
    private readonly Spell Paralytic_Poison = new Spell("Paralytic Poison");
    private readonly Spell Slice_and_Dice = new Spell("Slice and Dice");
    private Timer Slice_and_Dice_Timer = new Timer(0);
    private readonly Spell Sprint = new Spell("Sprint");
    private readonly Spell Stealth = new Spell("Stealth");
    private readonly Spell Wound_Poison = new Spell("Wound Poison");

    #endregion

    #region Offensive Spell

    private readonly Spell Ambush = new Spell("Ambush");
    private readonly Spell Crimson_Tempest = new Spell("Crimson Tempest");
    private readonly Spell Deadly_Throw = new Spell("Deadly Throw");
    private readonly Spell Eviscerate = new Spell("Eviscerate");
    private readonly Spell Expose_Armor = new Spell("Expose Armor");
    private readonly Spell Fan_of_Knives = new Spell("Fan of Knives");
    private readonly Spell Garrote = new Spell("Garrote");
    private readonly Spell Hemorrhage = new Spell("Hemorrhage");
    private readonly Spell Rupture = new Spell("Rupture");
    private Timer Rupture_Timer = new Timer(0);
    private readonly Spell Shiv = new Spell("Shiv");
    private readonly Spell Shuriken_Toss = new Spell("Shuriken Toss");
    private readonly Spell Sinister_Strike = new Spell("Sinister Strike");
    private readonly Spell Throw = new Spell("Throw");

    #endregion

    #region Offensive Cooldown

    private readonly Spell Premeditation = new Spell("Premeditation");
    private readonly Spell Redirect = new Spell("Redirect");
    private readonly Spell Shadow_Blades = new Spell("Shadow Blades");
    private readonly Spell Shadow_Dance = new Spell("Shadow Dance");
    private readonly Spell Shadow_Step = new Spell("Shadow Step");

    #endregion

    #region Defensive Cooldown

    private readonly Spell Cheap_Shot = new Spell("Cheap Shot");
    private readonly Spell Cloak_of_Shadows = new Spell("Cloak of Shadows");
    private readonly Spell Combat_Readiness = new Spell("Combat Readiness");
    private readonly Spell Dismantle = new Spell("Dismantle");
    private Timer Dismantle_Timer = new Timer(0);
    private readonly Spell Evasion = new Spell("Evasion");
    private readonly Spell Kick = new Spell("Kick");
    private readonly Spell Kidney_Shot = new Spell("Kidney Shot");
    private readonly Spell Preparation = new Spell("Preparation");
    private readonly Spell Smoke_Bomb = new Spell("Smoke Bomb");
    private readonly Spell Vanish = new Spell("Vanish");

    #endregion

    #region Healing Spell

    private readonly Spell Recuperate = new Spell("Recuperate");

    #endregion

    private Timer OnCD = new Timer(0);
    private Timer Trinket_Timer = new Timer(0);
    private Timer Engineering_Timer = new Timer(0);
    private Timer AlchFlask_Timer = new Timer(0);
    public int LC = 0;
    public int CP = 0;

    public Rogue_Subtlety()
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
                            (Throw.IsDistanceGood || Cheap_Shot.IsDistanceGood))
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
                        if (ObjectManager.Me.IsCast)
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
        if (Redirect.IsSpellUsable && Redirect.IsDistanceGood && Redirect.KnownSpell
            && MySettings.UseRedirect && ObjectManager.Me.ComboPoint > 0)
        {
            Redirect.Launch();
            Thread.Sleep(200);
        }

        if (((Stealth.KnownSpell && Stealth.IsSpellUsable && !Stealth.HaveBuff && MySettings.UseStealth)
            || Stealth.HaveBuff) && LC != 1)
        {
            if (!Stealth.HaveBuff)
            {
                Stealth.Launch();
                Thread.Sleep(200);
            }

            if (Premeditation.IsSpellUsable && Premeditation.IsDistanceGood && Premeditation.KnownSpell
                && MySettings.UsePremeditation && ObjectManager.Me.ComboPoint == 0)
            {
                Premeditation.Launch();
                Thread.Sleep(200);
            }

            if (Shadow_Step.IsSpellUsable && Shadow_Step.IsDistanceGood && Shadow_Step.KnownSpell
                && MySettings.UseShadowStep)
            {
                Shadow_Step.Launch();
                Thread.Sleep(200);
            }

            if (Garrote.IsSpellUsable && Garrote.IsDistanceGood && Garrote.KnownSpell
                && MySettings.UseGarrote)
            {
                Garrote.Launch();
                return;
            }
            else
            {
                if (Cheap_Shot.IsSpellUsable && Cheap_Shot.IsDistanceGood && Cheap_Shot.KnownSpell
                    && MySettings.UseCheapShot)
                {
                    Cheap_Shot.Launch();
                    return;
                }
            }
        }
        else if (Shuriken_Toss.IsSpellUsable && Shuriken_Toss.IsDistanceGood && Shuriken_Toss.KnownSpell
                && MySettings.UseShurikenToss && !MySettings.UseStealth)
        {
            Shuriken_Toss.Launch();
            return;
        }
        else
        {
            if (Throw.IsSpellUsable && Throw.IsDistanceGood && Throw.KnownSpell
                && MySettings.UseThrow && !MySettings.UseStealth)
            {
                MovementManager.StopMove();
                Throw.Launch();
                Thread.Sleep(1000);
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

        if (Throw.IsSpellUsable && Throw.IsDistanceGood && Throw.KnownSpell && !ObjectManager.Target.InCombat
            && MySettings.UseThrow)
        {
            Throw.Launch();
            return;
        }

        if (Eviscerate.KnownSpell && Eviscerate.IsSpellUsable && Eviscerate.IsDistanceGood
            && MySettings.UseEviscerate && ObjectManager.Me.ComboPoint > 4)
        {
            Eviscerate.Launch();
            return;
        }
        else if (Slice_and_Dice.KnownSpell && Slice_and_Dice.IsSpellUsable && Slice_and_Dice.IsDistanceGood
            && MySettings.UseSliceandDice && !Slice_and_Dice.HaveBuff)
        {
            CP = ObjectManager.Me.ComboPoint;
            Slice_and_Dice.Launch();
            Slice_and_Dice_Timer = new Timer(1000*(6+(CP*6)));
            return;
        }
        else
        {
            // Blizzard API Calls for Hemorrhage using Sinister Strike Function
            if (Sinister_Strike.KnownSpell && Sinister_Strike.IsSpellUsable && Sinister_Strike.IsDistanceGood
                && MySettings.UseHemorrhage)
            {
                Sinister_Strike.Launch();
                return;
            }
        }

        if (Fan_of_Knives.KnownSpell && Fan_of_Knives.IsSpellUsable && Fan_of_Knives.IsDistanceGood
            && MySettings.UseFanofKnives)
        {
            Fan_of_Knives.Launch();
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
        else if (Shadow_Dance.KnownSpell && Shadow_Dance.IsSpellUsable
            && MySettings.UseShadowDance && ObjectManager.Target.GetDistance < 10)
        {
            Shadow_Dance.Launch();
            return;
        }
        else
        {
            if (Shadow_Blades.KnownSpell && Shadow_Blades.IsSpellUsable
                && MySettings.UseShadowBlades && ObjectManager.Target.GetDistance < 30)
            {
                Shadow_Blades.Launch();
                return;
            }
        }
    }

    public void DPS_Cycle()
    {
        if (ObjectManager.Me.HaveBuff(115192) || ObjectManager.Me.HaveBuff(51713))
        {
            if (Garrote.IsSpellUsable && Garrote.IsDistanceGood && Garrote.KnownSpell
                && MySettings.UseGarrote && !ObjectManager.Target.HaveBuff(703))
            {
                Garrote.Launch();
                return;
            }
        }

        if (Throw.IsSpellUsable && Throw.IsDistanceGood && Throw.KnownSpell && !ObjectManager.Target.InCombat
            && MySettings.UseThrow)
        {
            Throw.Launch();
            return;
        }

        if (Eviscerate.KnownSpell && Eviscerate.IsSpellUsable && Eviscerate.IsDistanceGood
            && MySettings.UseEviscerate && ObjectManager.Me.ComboPoint > 4)
        {
            Eviscerate.Launch();
            return;
        }
        else if (Slice_and_Dice.KnownSpell && Slice_and_Dice.IsSpellUsable && Slice_and_Dice.IsDistanceGood
            && MySettings.UseSliceandDice && !Slice_and_Dice.HaveBuff)
        {
            CP = ObjectManager.Me.ComboPoint;
            Slice_and_Dice.Launch();
            Slice_and_Dice_Timer = new Timer(1000*(6+(CP*6)));
            return;
        }
        else if (Rupture.KnownSpell && Rupture.IsDistanceGood && Rupture.IsSpellUsable
            && MySettings.UseRupture && (!Rupture.TargetHaveBuff || Rupture_Timer.IsReady))
        {
            CP = ObjectManager.Me.ComboPoint;
            Rupture.Launch();
            Rupture_Timer = new Timer(1000*(4+(CP*4)));
            return;
        }
        else if (Expose_Armor.IsSpellUsable && Expose_Armor.IsDistanceGood && Expose_Armor.KnownSpell
            && MySettings.UseExposeArmor && !ObjectManager.Target.HaveBuff(113746))
        {
            Expose_Armor.Launch();
            return;
        }
        else
        {
            if (Sinister_Strike.KnownSpell && Sinister_Strike.IsSpellUsable && Sinister_Strike.IsDistanceGood
                && MySettings.UseHemorrhage)
            {
                Sinister_Strike.Launch();
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

        if (MySettings.UseDeadlyPoison && Deadly_Poison.KnownSpell && Deadly_Poison.IsSpellUsable
            && !Deadly_Poison.HaveBuff)
        {
            Deadly_Poison.Launch();
            return;
        }
        else
        {
            if (!Wound_Poison.HaveBuff && Wound_Poison.KnownSpell && Wound_Poison.IsSpellUsable
                && MySettings.UseWoundPoison && !Deadly_Poison.HaveBuff)
            {
                Wound_Poison.Launch();
                return;
            }
        }

        if (!Leeching_Poison.HaveBuff && Leeching_Poison.KnownSpell && Leeching_Poison.IsSpellUsable
            && MySettings.UseLeechingPoison)
        {
            Leeching_Poison.Launch();
            return;
        }
        else if (!Paralytic_Poison.HaveBuff && Paralytic_Poison.KnownSpell && Paralytic_Poison.IsSpellUsable
            && MySettings.UseParalyticPoison && !Leeching_Poison.HaveBuff)
        {
            Paralytic_Poison.Launch();
            return;
        }
        else if (!Crippling_Poison.HaveBuff && Crippling_Poison.KnownSpell && Crippling_Poison.IsSpellUsable
            && MySettings.UseCripplingPoison && !Leeching_Poison.HaveBuff && Paralytic_Poison.HaveBuff)
        {
            Crippling_Poison.Launch();
            return;
        }
        else
        {
            if (!Mindnumbing_Poison.HaveBuff && Mindnumbing_Poison.KnownSpell && Mindnumbing_Poison.IsSpellUsable
                && MySettings.UseMindnumbingPoison && !Crippling_Poison.HaveBuff && !Paralytic_Poison.HaveBuff
                && !Leeching_Poison.HaveBuff)
            {
                Mindnumbing_Poison.Launch();
                return;
            }
        }

        if (ObjectManager.GetNumberAttackPlayer() == 0 && Burst_of_Speed.IsSpellUsable && Burst_of_Speed.KnownSpell
            && MySettings.UseBurstofSpeed && !ObjectManager.Target.IsLootable
            && ObjectManager.Me.GetMove)
        {
            Burst_of_Speed.Launch();
            return;
        }
        else
        {
            if (ObjectManager.GetNumberAttackPlayer() == 0 && Sprint.IsSpellUsable && Sprint.KnownSpell
                && MySettings.UseSprint && !ObjectManager.Target.IsLootable
                && ObjectManager.Me.GetMove)
            {
                Sprint.Launch();
                return;
            }
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (!Recuperate.HaveBuff && ObjectManager.Me.ComboPoint > 1 && MySettings.UseRecuperate
            && ObjectManager.Me.HealthPercent <= 90 && Recuperate.KnownSpell && Recuperate.IsSpellUsable)
        {
            Recuperate.Launch();
            return;
        }
    }

    private void Defense_Cycle()
    {
        if (ObjectManager.Me.HealthPercent <= 10 && Vanish.KnownSpell && Vanish.IsSpellUsable
            && MySettings.UseVanish)
        {
            Vanish.Launch();
            Thread.Sleep(5000);
            OnCD = new Timer(1000*20);
            return;
        }
        else if (ObjectManager.Me.HealthPercent <= 80 && !Kidney_Shot.TargetHaveBuff && Kidney_Shot.KnownSpell
            && Kidney_Shot.IsSpellUsable && Kidney_Shot.IsDistanceGood && ObjectManager.Me.ComboPoint <= 3
            && Recuperate.HaveBuff && MySettings.UseKidneyShot)
        {
            CP = ObjectManager.Me.ComboPoint;
            Kidney_Shot.Launch();
            OnCD = new Timer(1000*CP);
            return;
        }
        else if (ObjectManager.Me.HealthPercent <= 80 && Evasion.KnownSpell && Evasion.IsSpellUsable
            && MySettings.UseEvasion)
        {
            Evasion.Launch();
            OnCD = new Timer(1000*15);
            return;
        }
        else if (ObjectManager.Me.HealthPercent <= 90 && Combat_Readiness.KnownSpell && Combat_Readiness.IsSpellUsable
            && MySettings.UseCombatReadiness)
        {
            Combat_Readiness.Launch();
            OnCD = new Timer(1000*20);
            return;
        }
        else if (ObjectManager.Me.HealthPercent <= 95 && Dismantle.KnownSpell && Dismantle.IsSpellUsable
            && MySettings.UseDismantle)
        {
            Dismantle.Launch();
            Dismantle_Timer = new Timer(1000*60);
            return;
        }
        else
        {
            if (ObjectManager.GetNumberAttackPlayer() >= 3 && Vanish.KnownSpell && Vanish.IsSpellUsable
                && MySettings.UseVanish)
            {
                Vanish.Launch();
                Thread.Sleep(5000);
                return;
            }
        }

        if (ObjectManager.Me.HealthPercent <= 70 && Preparation.KnownSpell && Preparation.IsSpellUsable
            && MySettings.UsePreparation && !Evasion.IsSpellUsable)
        {
            Preparation.Launch();
            return;
        }
    }

    private void Decast()
    {
        if (ObjectManager.Target.IsCast && Kick.KnownSpell && Kick.IsSpellUsable 
            && Kick.IsDistanceGood && MySettings.UseKick && ObjectManager.Target.IsTargetingMe)
        {
            Kick.Launch();
            return;
        }
        else if (ObjectManager.Target.IsCast && Cloak_of_Shadows.KnownSpell && Cloak_of_Shadows.IsSpellUsable
            && ObjectManager.Target.IsTargetingMe && MySettings.UseCloakofShadows)
        {
            Cloak_of_Shadows.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Target.IsCast && Smoke_Bomb.KnownSpell && Smoke_Bomb.IsSpellUsable
                && ObjectManager.Target.IsTargetingMe && MySettings.UseSmokeBomb
                && !Cloak_of_Shadows.HaveBuff)
            {
                Smoke_Bomb.Launch();
                return;
            }
        }

        if (ObjectManager.Me.HealthPercent <= 70 && Preparation.KnownSpell && Preparation.IsSpellUsable
            && MySettings.UsePreparation && !Cloak_of_Shadows.IsSpellUsable && ObjectManager.Target.IsCast
            && ObjectManager.Target.IsTargetingMe)
        {
            Preparation.Launch();
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

public class Rogue_Assassination
{
    [Serializable]
    public class RogueAssassinationSettings : nManager.Helpful.Settings
    {
        /* Professions & Racials */
        public bool UseArcaneTorrent = true;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseLifeblood = true;
        public bool UseStoneform = true;
        public bool UseGiftoftheNaaru = true;
        public bool UseWarStomp = true;
        /* Rogue Buffs */
        public bool UseBurstofSpeed = true;
        public bool UseCripplingPoison = false;
        public bool UseDeadlyPoison = true;
        public bool UseLeechingPoison = true;
        public bool UseMindnumbingPoison = true;
        public bool UseParalyticPoison = false;
        public bool UseSliceandDice = true;
        public bool UseSprint = true;
        public bool UseStealth = false;
        public bool UseWoundPoison = false;
        /* Offensive Spell */
        public bool UseAmbush = true;
        public bool UseCrimsonTempest = true;
        public bool UseDeadlyThrow  = true;
        public bool UseDispatch = true;
        public bool UseEnvenom = true;
        public bool UseExposeArmor = false;
        public bool UseFanofKnives = true;
        public bool UseGarrote = true;
        public bool UseMutilate = true;
        public bool UseRupture = true;
        public bool UseShiv = true;
        public bool UseShurikenToss = true;
        public bool UseThrow = true;
        /* Offensive Cooldown */
        public bool UseRedirect = true;
        public bool UseShadowBlades = true;
        public bool UseShadowStep = true;
        public bool UseVendetta = true;
        /* Defensive Cooldown */
        public bool UseCheapShot = true;
        public bool UseCloakofShadows= true;
        public bool UseCombatReadiness = true;
        public bool UseDismantle = true;
        public bool UseEvasion = true;
        public bool UseKick = true;
        public bool UseKidneyShot = true;
        public bool UsePreparation = true;
        public bool UseSmokeBomb = true;
        public bool UseVanish = true;
        /* Healing Spell */
        public bool UseRecuperate = true;
        /* Game Settings */
        public bool UseLowCombat = true;
        public bool UseTrinket = true;
        public bool UseEngGlove = true;
        public bool UseAlchFlask = true;
        public bool UseShadowFocus = false;

        public RogueAssassinationSettings()
        {
            ConfigWinForm(new System.Drawing.Point(400, 400), "Rogue Assassination Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");
            AddControlInWinForm("Use Lifeblood", "UseLifeblood", "Professions & Racials");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            /* Rogue Buffs */
            AddControlInWinForm("Use Burst of Speed", "UseBurstofSpeed", "Rogue Buffs");
            AddControlInWinForm("Use Crippling Poison", "UseCripplingPoison", "Rogue Buffs");
            AddControlInWinForm("Use Deadly Poison", "UseDeadlyPoison", "Rogue Buffs");
            AddControlInWinForm("Use Leeching Poison", "UseLeechingPoison", "Rogue Buffs");
            AddControlInWinForm("Use Mindnumbing Poison", "UseMindnumbingPoison", "Rogue Buffs");
            AddControlInWinForm("Use Paralytic Poison", "UseParalyticPoison", "Rogue Buffs");
            AddControlInWinForm("Use Slice and Dice", "UseSliceandDice", "Rogue Buffs");
            AddControlInWinForm("Use Sprint", "UseSprint", "Rogue Buffs");
            AddControlInWinForm("Use Stealth", "UseStealth", "Rogue Buffs");
            AddControlInWinForm("Use Wound Poison", "UseWoundPoison", "Rogue Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Ambush", "UseAmbush", "Offensive Spell");
            AddControlInWinForm("Use Crimson Tempest", "UseCrimsonTempest", "Offensive Spell");
            AddControlInWinForm("Use Deadly Throw", "UseDeadlyThrow", "Offensive Spell");
            AddControlInWinForm("Use Dispatch", "UseDispatch", "Offensive Spell");
            AddControlInWinForm("Use Envenom", "UseEnvenom", "Offensive Spell");
            AddControlInWinForm("Use Expose Armor", "UseExposeArmor", "Offensive Spell");
            AddControlInWinForm("Use Fan of Knives", "UseFanofKnives", "Offensive Spell");
            AddControlInWinForm("Use Garrote", "UseGarrote", "Offensive Spell");
            AddControlInWinForm("Use Mutilate", "UseMutilate", "Offensive Spell");
            AddControlInWinForm("Use Rupture", "UseRupture", "Offensive Spell");
            AddControlInWinForm("Use Shiv", "UseShiv", "Offensive Spell");
            AddControlInWinForm("Use Shuriken Toss", "UseShurikenToss", "Offensive Spell");
            AddControlInWinForm("Use Throw", "UseThrow", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use UseRedirect", "UseRedirect", "Offensive Cooldown");
            AddControlInWinForm("Use Shadow Blades", "UseShadowBlades", "Offensive Cooldown");
            AddControlInWinForm("Use Shadow Step", "UseShadowStep", "Offensive Cooldown");
            AddControlInWinForm("Use Vendetta", "UseVendetta", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use CheapShot", "UseCheapShot", "Defensive Cooldown");
            AddControlInWinForm("Use CloakofShadows", "UseCloakofShadows", "Defensive Cooldown");
            AddControlInWinForm("Use CombatReadiness", "UseCombatReadiness", "Defensive Cooldown");
            AddControlInWinForm("Use Dismantle", "UseDismantle", "Defensive Cooldown");
            AddControlInWinForm("Use Evasion", "UseEvasion", "Defensive Cooldown");
            AddControlInWinForm("Use Kick", "UseKick", "Defensive Cooldown");
            AddControlInWinForm("Use KidneyShot", "UseKidneyShot", "Defensive Cooldown");
            AddControlInWinForm("Use Preparation", "UsePreparation", "Defensive Cooldown");
            AddControlInWinForm("Use SmokeBomb", "UseSmokeBomb", "Defensive Cooldown");
            AddControlInWinForm("Use Vanish", "UseVanish", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Recuperate", "UseRecuperate", "Healing Spell");
            /* Game Settings */
            AddControlInWinForm("Use Low Combat Settings", "UseLowCombat", "Game Settings");
            AddControlInWinForm("Use Trinket", "UseTrinket", "Game Settings");
            AddControlInWinForm("Use Engineering Gloves", "UseEngGlove", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
            AddControlInWinForm("Use Shadow Focus Talent?", "UseShadowFocus", "Game Settings");
        }

        public static RogueAssassinationSettings CurrentSetting { get; set; }

        public static RogueAssassinationSettings GetSettings()
        {
            string CurrentSettingsFile = Application.StartupPath + "\\CustomClasses\\Settings\\Rogue_Assassination.xml";
            if (System.IO.File.Exists(CurrentSettingsFile))
            {
                return
                    CurrentSetting = Settings.Load<Rogue_Assassination.RogueAssassinationSettings>(CurrentSettingsFile);
            }
            else
            {
                return new Rogue_Assassination.RogueAssassinationSettings();
            }
        }
    }

    private readonly RogueAssassinationSettings MySettings = RogueAssassinationSettings.GetSettings();

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

    #region Rogue Buffs

    private readonly Spell Burst_of_Speed = new Spell("Burst of Speed");
    private readonly Spell Crippling_Poison = new Spell("Crippling Poison");
    private readonly Spell Deadly_Poison = new Spell("Deadly Poison");
    private readonly Spell Leeching_Poison = new Spell("Leeching Poison");
    private readonly Spell Mindnumbing_Poison = new Spell("Mind-numbing Poison");
    private readonly Spell Paralytic_Poison = new Spell("Paralytic Poison");
    private readonly Spell Slice_and_Dice = new Spell("Slice and Dice");
    private Timer Slice_and_Dice_Timer = new Timer(0);
    private readonly Spell Sprint = new Spell("Sprint");
    private readonly Spell Stealth = new Spell("Stealth");
    private readonly Spell Wound_Poison = new Spell("Wound Poison");

    #endregion

    #region Offensive Spell

    private readonly Spell Ambush = new Spell("Ambush");
    private readonly Spell Crimson_Tempest = new Spell("Crimson Tempest");
    private readonly Spell Deadly_Throw = new Spell("Deadly Throw");
    private readonly Spell Dispatch = new Spell("Dispatch");
    private readonly Spell Envenom = new Spell("Envenom");
    private readonly Spell Eviscerate = new Spell("Eviscerate");
    private readonly Spell Expose_Armor = new Spell("Expose Armor");
    private readonly Spell Fan_of_Knives = new Spell("Fan of Knives");
    private readonly Spell Garrote = new Spell("Garrote");
    private readonly Spell Mutilate = new Spell("Mutilate");
    private readonly Spell Rupture = new Spell("Rupture");
    private Timer Rupture_Timer = new Timer(0);
    private readonly Spell Shiv = new Spell("Shiv");
    private readonly Spell Shuriken_Toss = new Spell("Shuriken Toss");
    private readonly Spell Sinister_Strike = new Spell("Sinister Strike");
    private readonly Spell Throw = new Spell("Throw");

    #endregion

    #region Offensive Cooldown

    private readonly Spell Redirect = new Spell("Redirect");
    private readonly Spell Shadow_Blades = new Spell("Shadow Blades");
    private readonly Spell Shadow_Step = new Spell("Shadow Step");
    private readonly Spell Vendetta = new Spell("Vendetta");

    #endregion

    #region Defensive Cooldown

    private readonly Spell Cheap_Shot = new Spell("Cheap Shot");
    private readonly Spell Cloak_of_Shadows = new Spell("Cloak of Shadows");
    private readonly Spell Combat_Readiness = new Spell("Combat Readiness");
    private readonly Spell Dismantle = new Spell("Dismantle");
    private Timer Dismantle_Timer = new Timer(0);
    private readonly Spell Evasion = new Spell("Evasion");
    private readonly Spell Kick = new Spell("Kick");
    private readonly Spell Kidney_Shot = new Spell("Kidney Shot");
    private readonly Spell Preparation = new Spell("Preparation");
    private readonly Spell Smoke_Bomb = new Spell("Smoke Bomb");
    private readonly Spell Vanish = new Spell("Vanish");

    #endregion

    #region Healing Spell

    private readonly Spell Recuperate = new Spell("Recuperate");

    #endregion

    private Timer OnCD = new Timer(0);
    private Timer Trinket_Timer = new Timer(0);
    private Timer Engineering_Timer = new Timer(0);
    private Timer AlchFlask_Timer = new Timer(0);
    public int LC = 0;
    public int CP = 0;

    public Rogue_Assassination()
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
                            (Throw.IsDistanceGood || Cheap_Shot.IsDistanceGood))
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
                        if (ObjectManager.Me.IsCast)
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
        if (Redirect.IsSpellUsable && Redirect.IsDistanceGood && Redirect.KnownSpell
            && MySettings.UseRedirect && ObjectManager.Me.ComboPoint > 0)
        {
            Redirect.Launch();
            Thread.Sleep(200);
        }

        if (((Stealth.KnownSpell && Stealth.IsSpellUsable && !Stealth.HaveBuff && MySettings.UseStealth)
            || Stealth.HaveBuff) && LC != 1)
        {
            if (!Stealth.HaveBuff)
            {
                Stealth.Launch();
                Thread.Sleep(200);
            }

            if (Shadow_Step.IsSpellUsable && Shadow_Step.IsDistanceGood && Shadow_Step.KnownSpell
                && MySettings.UseShadowStep)
            {
                Shadow_Step.Launch();
                Thread.Sleep(200);
            }

            if (Garrote.IsSpellUsable && Garrote.IsDistanceGood && Garrote.KnownSpell
                && MySettings.UseGarrote)
            {
                Garrote.Launch();
                return;
            }
            else
            {
                if (Cheap_Shot.IsSpellUsable && Cheap_Shot.IsDistanceGood && Cheap_Shot.KnownSpell
                    && MySettings.UseCheapShot)
                {
                    Cheap_Shot.Launch();
                    return;
                }
            }
        }
        else if (Shuriken_Toss.IsSpellUsable && Shuriken_Toss.IsDistanceGood && Shuriken_Toss.KnownSpell
                && MySettings.UseShurikenToss && !MySettings.UseStealth)
        {
            Shuriken_Toss.Launch();
            return;
        }
        else
        {
            if (Throw.IsSpellUsable && Throw.IsDistanceGood && Throw.KnownSpell
                && MySettings.UseThrow && !MySettings.UseStealth)
            {
                MovementManager.StopMove();
                Throw.Launch();
                Thread.Sleep(1000);
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

        if (Throw.IsSpellUsable && Throw.IsDistanceGood && Throw.KnownSpell && !ObjectManager.Target.InCombat
            && MySettings.UseThrow)
        {
            Throw.Launch();
            return;
        }
        // Blizzard API Calls for Envenom using Eviscerate Function
        if (Eviscerate.KnownSpell && Eviscerate.IsSpellUsable && Eviscerate.IsDistanceGood
            && MySettings.UseEnvenom && (ObjectManager.Me.ComboPoint > 4
            || (Slice_and_Dice.HaveBuff && Slice_and_Dice_Timer.IsReady)))
        {
            Eviscerate.Launch();
            if (Slice_and_Dice.HaveBuff)
                Slice_and_Dice_Timer = new Timer(1000*(6+(5*6)));
            return;
        }
        // Blizzard API Calls for Dispatch using Sinister Strike Function
        else if (Sinister_Strike.KnownSpell && Sinister_Strike.IsSpellUsable && Sinister_Strike.IsDistanceGood
            && MySettings.UseDispatch)
        {
            Sinister_Strike.Launch();
            return;
        }
        else if (Slice_and_Dice.KnownSpell && Slice_and_Dice.IsSpellUsable && Slice_and_Dice.IsDistanceGood
            && MySettings.UseSliceandDice && !Slice_and_Dice.HaveBuff)
        {
            CP = ObjectManager.Me.ComboPoint;
            Slice_and_Dice.Launch();
            Slice_and_Dice_Timer = new Timer(1000*(6+(CP*6)));
            return;
        }
        else
        {
            if (Mutilate.KnownSpell && Mutilate.IsSpellUsable && ObjectManager.Target.HealthPercent > 35
                && MySettings.UseMutilate && Mutilate.IsDistanceGood)
            {
                Mutilate.Launch();
                return;
            }
        }

        if (Fan_of_Knives.KnownSpell && Fan_of_Knives.IsSpellUsable && Fan_of_Knives.IsDistanceGood
            && MySettings.UseFanofKnives)
        {
            Fan_of_Knives.Launch();
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
        else if (Vendetta.KnownSpell && Vendetta.IsSpellUsable
            && MySettings.UseVendetta && Vendetta.IsDistanceGood)
        {
            Vendetta.Launch();
            return;
        }
        else
        {
            if (Shadow_Blades.KnownSpell && Shadow_Blades.IsSpellUsable
                && MySettings.UseShadowBlades && ObjectManager.Target.GetDistance < 30)
            {
                Shadow_Blades.Launch();
                return;
            }
        }
    }

    public void DPS_Cycle()
    {
        if (Mutilate.KnownSpell && Mutilate.IsSpellUsable && MySettings.UseMutilate 
            && Mutilate.IsDistanceGood && MySettings.UseShadowFocus && !ObjectManager.Target.InCombat
            && (Stealth.HaveBuff || ObjectManager.Me.HaveBuff(115192)))
        {
            Mutilate.Launch();
            return;
        }

        if (Garrote.IsSpellUsable && Garrote.IsDistanceGood && Garrote.KnownSpell
            && MySettings.UseGarrote && ObjectManager.Me.HaveBuff(115192))
        {
            Garrote.Launch();
            return;
        }

        if (Throw.IsSpellUsable && Throw.IsDistanceGood && Throw.KnownSpell && !ObjectManager.Target.InCombat
            && MySettings.UseThrow)
        {
            Throw.Launch();
            return;
        }

        if (Eviscerate.KnownSpell && Eviscerate.IsSpellUsable && Eviscerate.IsDistanceGood
            && MySettings.UseEnvenom && (ObjectManager.Me.ComboPoint > 4 
            || (Slice_and_Dice.HaveBuff && Slice_and_Dice_Timer.IsReady)))
        {
            Eviscerate.Launch();
            if (Slice_and_Dice.HaveBuff)
                Slice_and_Dice_Timer = new Timer(1000*(6+(5*6)));
            return;
        }
        else if (Sinister_Strike.KnownSpell && Sinister_Strike.IsSpellUsable && Sinister_Strike.IsDistanceGood
            && MySettings.UseDispatch)
        {
            Sinister_Strike.Launch();
            return;
        }
        else if (Slice_and_Dice.KnownSpell && Slice_and_Dice.IsSpellUsable && Slice_and_Dice.IsDistanceGood
            && MySettings.UseSliceandDice && !Slice_and_Dice.HaveBuff)
        {
            CP = ObjectManager.Me.ComboPoint;
            Slice_and_Dice.Launch();
            Slice_and_Dice_Timer = new Timer(1000*(6+(CP*6)));
            return;
        }
        else if (Rupture.KnownSpell && Rupture.IsDistanceGood && Rupture.IsSpellUsable
            && MySettings.UseRupture && (!Rupture.TargetHaveBuff || Rupture_Timer.IsReady))
        {
            CP = ObjectManager.Me.ComboPoint;
            Rupture.Launch();
            Rupture_Timer = new Timer(1000*(4+(CP*4)));
            return;
        }
        else if (Expose_Armor.IsSpellUsable && Expose_Armor.IsDistanceGood && Expose_Armor.KnownSpell
            && MySettings.UseExposeArmor && !ObjectManager.Target.HaveBuff(113746))
        {
            Expose_Armor.Launch();
            return;
        }
        else
        {
            if (Mutilate.KnownSpell && Mutilate.IsSpellUsable && ObjectManager.Target.HealthPercent > 35
                && MySettings.UseMutilate && Mutilate.IsDistanceGood)
            {
                Mutilate.Launch();
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

        if (MySettings.UseDeadlyPoison && Deadly_Poison.KnownSpell && Deadly_Poison.IsSpellUsable
            && !Deadly_Poison.HaveBuff)
        {
            Deadly_Poison.Launch();
            return;
        }
        else 
        {
            if (!Wound_Poison.HaveBuff && Wound_Poison.KnownSpell && Wound_Poison.IsSpellUsable
                && MySettings.UseWoundPoison && !Deadly_Poison.HaveBuff)
            {
                Wound_Poison.Launch();
                return;
            }
        }

        if (!Leeching_Poison.HaveBuff && Leeching_Poison.KnownSpell && Leeching_Poison.IsSpellUsable
            && MySettings.UseLeechingPoison)
        {
            Leeching_Poison.Launch();
            return;
        }
        else if (!Paralytic_Poison.HaveBuff && Paralytic_Poison.KnownSpell && Paralytic_Poison.IsSpellUsable
            && MySettings.UseParalyticPoison && !Leeching_Poison.HaveBuff)
        {
            Paralytic_Poison.Launch();
            return;
        }
        else if (!Crippling_Poison.HaveBuff && Crippling_Poison.KnownSpell && Crippling_Poison.IsSpellUsable
            && MySettings.UseCripplingPoison && !Leeching_Poison.HaveBuff && Paralytic_Poison.HaveBuff)
        {
            Crippling_Poison.Launch();
            return;
        }
        else
        {
            if (!Mindnumbing_Poison.HaveBuff && Mindnumbing_Poison.KnownSpell && Mindnumbing_Poison.IsSpellUsable
                && MySettings.UseMindnumbingPoison && !Crippling_Poison.HaveBuff && !Paralytic_Poison.HaveBuff
                && !Leeching_Poison.HaveBuff)
            {
                Mindnumbing_Poison.Launch();
                return;
            }
        }

        if (ObjectManager.GetNumberAttackPlayer() == 0 && Burst_of_Speed.IsSpellUsable && Burst_of_Speed.KnownSpell
            && MySettings.UseBurstofSpeed && !ObjectManager.Target.IsLootable
            && ObjectManager.Me.GetMove)
        {
            Burst_of_Speed.Launch();
            return;
        }
        else
        {
            if (ObjectManager.GetNumberAttackPlayer() == 0 && Sprint.IsSpellUsable && Sprint.KnownSpell
                && MySettings.UseSprint && !ObjectManager.Target.IsLootable
                && ObjectManager.Me.GetMove)
            {
                Sprint.Launch();
                return;
            }
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (!Recuperate.HaveBuff && ObjectManager.Me.ComboPoint > 1 && MySettings.UseRecuperate
            && ObjectManager.Me.HealthPercent <= 90 && Recuperate.KnownSpell && Recuperate.IsSpellUsable)
        {
            Recuperate.Launch();
            return;
        }
    }

    private void Defense_Cycle()
    {
        if (ObjectManager.Me.HealthPercent <= 10 && Vanish.KnownSpell && Vanish.IsSpellUsable
            && MySettings.UseVanish)
        {
            Vanish.Launch();
            Thread.Sleep(5000);
            OnCD = new Timer(1000*20);
            return;
        }
        else if (ObjectManager.Me.HealthPercent <= 80 && !Kidney_Shot.TargetHaveBuff && Kidney_Shot.KnownSpell
            && Kidney_Shot.IsSpellUsable && Kidney_Shot.IsDistanceGood && ObjectManager.Me.ComboPoint <= 3
            && Recuperate.HaveBuff && MySettings.UseKidneyShot)
        {
            CP = ObjectManager.Me.ComboPoint;
            Kidney_Shot.Launch();
            OnCD = new Timer(1000*CP);
            return;
        }
        else if (ObjectManager.Me.HealthPercent <= 80 && Evasion.KnownSpell && Evasion.IsSpellUsable
            && MySettings.UseEvasion)
        {
            Evasion.Launch();
            OnCD = new Timer(1000*15);
            return;
        }
        else if (ObjectManager.Me.HealthPercent <= 90 && Combat_Readiness.KnownSpell && Combat_Readiness.IsSpellUsable
            && MySettings.UseCombatReadiness)
        {
            Combat_Readiness.Launch();
            OnCD = new Timer(1000*20);
            return;
        }
        else if (ObjectManager.Me.HealthPercent <= 95 && Dismantle.KnownSpell && Dismantle.IsSpellUsable
            && MySettings.UseDismantle)
        {
            Dismantle.Launch();
            Dismantle_Timer = new Timer(1000*60);
            return;
        }
        else
        {
            if (ObjectManager.GetNumberAttackPlayer() >= 3 && Vanish.KnownSpell && Vanish.IsSpellUsable
                && MySettings.UseVanish)
            {
                Vanish.Launch();
                Thread.Sleep(5000);
                return;
            }
        }

        if (ObjectManager.Me.HealthPercent <= 70 && Preparation.KnownSpell && Preparation.IsSpellUsable
            && MySettings.UsePreparation && !Evasion.IsSpellUsable)
        {
            Preparation.Launch();
            return;
        }
    }

    private void Decast()
    {
        if (ObjectManager.Target.IsCast && Kick.KnownSpell && Kick.IsSpellUsable 
            && Kick.IsDistanceGood && MySettings.UseKick && ObjectManager.Target.IsTargetingMe)
        {
            Kick.Launch();
            return;
        }
        else if (ObjectManager.Target.IsCast && Cloak_of_Shadows.KnownSpell && Cloak_of_Shadows.IsSpellUsable
            && ObjectManager.Target.IsTargetingMe && MySettings.UseCloakofShadows)
        {
            Cloak_of_Shadows.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Target.IsCast && Smoke_Bomb.KnownSpell && Smoke_Bomb.IsSpellUsable
                && ObjectManager.Target.IsTargetingMe && MySettings.UseSmokeBomb
                && !Cloak_of_Shadows.HaveBuff)
            {
                Smoke_Bomb.Launch();
                return;
            }
        }

        if (ObjectManager.Me.HealthPercent <= 70 && Preparation.KnownSpell && Preparation.IsSpellUsable
            && MySettings.UsePreparation && !Cloak_of_Shadows.IsSpellUsable && ObjectManager.Target.IsCast
            && ObjectManager.Target.IsTargetingMe)
        {
            Preparation.Launch();
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
