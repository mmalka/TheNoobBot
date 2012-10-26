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
                    #region Warrior Specialisation checking

                case WoWClass.Warrior:
                    var Warrior_Arms_Spell = new Spell("Mortal Strike");
                    var Warrior_Fury_Spell = new Spell("Bloodthirst");
                    var Warrior_Protection_Spell = new Spell("Shield Slam");

                    if (Warrior_Arms_Spell.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Warrior_Arms.xml";
                            Warrior_Arms.WarriorArmsSettings CurrentSetting;
                            CurrentSetting = new Warrior_Arms.WarriorArmsSettings();
                            if (System.IO.File.Exists(CurrentSettingsFile))
                            {
                                CurrentSetting =
                                    Settings.Load<Warrior_Arms.WarriorArmsSettings>(CurrentSettingsFile);
                            }
                            CurrentSetting.ToForm();
                            CurrentSetting.Save(CurrentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Warrior Arms class...");
                            new Warrior_Arms();
                        }
                    }
                    else if (Warrior_Fury_Spell.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Warrior_Fury.xml";
                            Warrior_Fury.WarriorFurySettings CurrentSetting;
                            CurrentSetting = new Warrior_Fury.WarriorFurySettings();
                            if (System.IO.File.Exists(CurrentSettingsFile))
                            {
                                CurrentSetting =
                                    Settings.Load<Warrior_Fury.WarriorFurySettings>(CurrentSettingsFile);
                            }
                            CurrentSetting.ToForm();
                            CurrentSetting.Save(CurrentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Warrior Fury class...");
                            new Warrior_Fury();
                        }
                    }
                    else if (Warrior_Protection_Spell.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Warrior_Protection.xml";
                            Warrior_Protection.WarriorProtectionSettings CurrentSetting;
                            CurrentSetting = new Warrior_Protection.WarriorProtectionSettings();
                            if (System.IO.File.Exists(CurrentSettingsFile))
                            {
                                CurrentSetting =
                                    Settings.Load<Warrior_Protection.WarriorProtectionSettings>(CurrentSettingsFile);
                            }
                            CurrentSetting.ToForm();
                            CurrentSetting.Save(CurrentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Warrior Protection class...");
                            new Warrior_Protection();
                        }
                    }
                    else
                    {
                        if (ConfigOnly)
                        {
                            MessageBox.Show(
                                "Your specification haven't be found, loading Warrior Arms Settings");
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Warrior_Arms.xml";
                            Warrior_Arms.WarriorArmsSettings CurrentSetting;
                            CurrentSetting = new Warrior_Arms.WarriorArmsSettings();
                            if (File.Exists(CurrentSettingsFile))
                            {
                                CurrentSetting =
                                    Settings.Load<Warrior_Arms.WarriorArmsSettings>(CurrentSettingsFile);
                            }
                            CurrentSetting.ToForm();
                            CurrentSetting.Save(CurrentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("No specialisation detected.");
                            Logging.WriteFight("Loading Warrior Arms class...");
                            new Warrior_Arms();
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

#region Warrior

public class Warrior_Arms
{
    [Serializable]
    public class WarriorArmsSettings : nManager.Helpful.Settings
    {
        /* Professions & Racials */
        public bool UseArcaneTorrent = true;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseLifeblood = true;
        public bool UseStoneform = true;
        public bool UseGiftoftheNaaru = true;
        public bool UseWarStomp = true;
        /* Warrior Buffs */
        public bool UseBattleShout = true;
        public bool UseBattleStance = true;
        public bool UseBerserkerStance = false;
        public bool UseCommandingShout = false;
        public bool UseDefensiveStance = true;
        /* Offensive Spell */
        public bool UseAvatar = true;
        public bool UseBladestorm = true;
        public bool UseBloodbath = true;
        public bool UseCharge = true;
        public bool UseCleave = true;
        public bool UseColossusSmash = true;
        public bool UseDragonRoar = true;
        public bool UseExecute = true;
        public bool UseHeroicLeap = true;
        public bool UseHeroicStrike = true;
        public bool UseHeroicThrow = true;
        public bool UseMortalStrike = true;
        public bool UseOverpower = true;
        public bool UseShockwave = true;
        public bool UseSlam = true;
        public bool UseStormBolt = true;
        public bool UseTaunt = true;
        public bool UseThunderClap = true;
        public bool UseWhirlwind = true;
        /* Offensive Cooldown */
        public bool UseBerserkerRage = true;
        public bool UseDeadlyCalm = true;
        public bool UseRecklessness = true;
        public bool UseShatteringThrow = true;
        public bool UseSweepingStrikes = true;
        public bool UseSkullBanner = true;
        /* Defensive Cooldown */
        public bool UseDemoralizingBanner = true;
        public bool UseDiebytheSword = true;
        public bool UseDisarm = true;
        public bool UseDisruptingShout = true;
        public bool UseHamstring = false;
        public bool UseIntimidatingShout = true;
        public bool UseMassSpellReflection = true;
        public bool UsePiercingHowl = false;
        public bool UsePummel = true;
        public bool UseStaggeringShout = true;
        /* Healing Spell */
        public bool UseEnragedRegeneration = true;
        public bool UseRallyingCry = true;
        public bool UseVictoryRush = true;
        /* Game Settings */
        public bool UseLowCombat = true;
        public bool UseTrinket = true;
        public bool UseEngGlove = true;
        public bool UseAlchFlask = true;

        public WarriorArmsSettings()
        {
            ConfigWinForm(new System.Drawing.Point(400, 400), "Warrior Arms Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");
            AddControlInWinForm("Use Lifeblood", "UseLifeblood", "Professions & Racials");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            /* Warrior Buffs */
            AddControlInWinForm("Use Battle Shout", "UseBattleShout", "Warrior Buffs");
            AddControlInWinForm("Use Battle Stance", "UseBattleStance", "Warrior Buffs");
            AddControlInWinForm("Use Berserker Stance", "UseBerserkerStance", "Warrior Buffs");
            AddControlInWinForm("Use Commanding Shout", "UseCommandingShout", "Warrior Buffs");
            AddControlInWinForm("Use Defensive Stance", "UseDefensiveStance", "Warrior Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Avatar", "UseAvatar", "Offensive Spell");
            AddControlInWinForm("Use Bladestorm", "UseBladestorm", "Offensive Spell");
            AddControlInWinForm("Use Bloodbath", "UseBloodbath", "Offensive Spell");
            AddControlInWinForm("Use Charge", "UseCharge", "Offensive Spell");
            AddControlInWinForm("Use Cleave", "UseCleave", "Offensive Spell");
            AddControlInWinForm("Use Colossus Smash", "UseColossusSmash", "Offensive Spell");
            AddControlInWinForm("Use Dragon Roar", "UseDragonRoar", "Offensive Spell");
            AddControlInWinForm("Use Exectue", "UseExecute", "Offensive Spell");
            AddControlInWinForm("Use Heroic Leap", "UseHeroicLeap", "Offensive Spell");
            AddControlInWinForm("Use Heroic Strike", "UseHeroicStrike", "Offensive Spell");
            AddControlInWinForm("Use Heroic Throw", "UseHeroicThrow", "Offensive Spell");
            AddControlInWinForm("Use Mortal Strike", "UseMortalStrike", "Offensive Spell");
            AddControlInWinForm("Use Overpower", "UseOverpower", "Offensive Spell");
            AddControlInWinForm("Use Shockwave", "UseShockwave", "Offensive Spell");
            AddControlInWinForm("Use Slam", "UseSlam", "Offensive Spell");
            AddControlInWinForm("Use Storm Bolt", "UseStormBolt", "Offensive Spell");
            AddControlInWinForm("Use Taunt", "UseTaunt", "Offensive Spell");
            AddControlInWinForm("Use Thunder Clap", "UseThunderClap", "Offensive Spell");
            AddControlInWinForm("Use Whirlwind", "UseWhirlwind", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use Berserker Rage", "UseBerserkerRage", "Offensive Cooldown");
            AddControlInWinForm("Use Deadly Calm", "UseDeadlyCalm", "Offensive Cooldown");
            AddControlInWinForm("Use Recklessness", "UseRecklessness", "Offensive Cooldown");
            AddControlInWinForm("Use Shattering Throw", "UseShatteringThrow", "Offensive Cooldown");
            AddControlInWinForm("Use Sweeping Strikes", "UseSweepingStrikes", "Offensive Cooldown");
            AddControlInWinForm("Use Skull Banner", "UseSkullBanner", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Demoralizing Banner", "UseDemoralizingBanner", "Defensive Cooldown");
            AddControlInWinForm("Use Die by the Sword", "UseDiebytheSword", "Defensive Cooldown");
            AddControlInWinForm("Use Disarm", "UseDisarm", "Defensive Cooldown");
            AddControlInWinForm("Use Disrupting Shout", "UseDisruptingShout", "Defensive Cooldown");
            AddControlInWinForm("Use Hamstring", "UseHamstring", "Defensive Cooldown");
            AddControlInWinForm("Use Intimidating Shout", "UseIntimidatingShout", "Defensive Cooldown");
            AddControlInWinForm("Use Mass Spell Reflection", "UseMassSpellReflection", "Defensive Cooldown");
            AddControlInWinForm("Use Piercing Howl", "UsePiercingHowl", "Defensive Cooldown");
            AddControlInWinForm("Use Pummel", "UsePummel", "Defensive Cooldown");
            AddControlInWinForm("Use Staggering Shout", "UseStaggeringShout", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Enraged Regeneration", "UseEnragedRegeneration", "Healing Spell");
            AddControlInWinForm("Use Rallying Cry", "UseRallyingCry", "Healing Spell");
            AddControlInWinForm("Use Victory Rush", "UseVictoryRush", "Healing Spell");
            /* Game Settings */
            AddControlInWinForm("Use Low Combat Settings", "UseLowCombat", "Game Settings");
            AddControlInWinForm("Use Trinket", "UseTrinket", "Game Settings");
            AddControlInWinForm("Use Engineering Gloves", "UseEngGlove", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
        }

        public static WarriorArmsSettings CurrentSetting { get; set; }

        public static WarriorArmsSettings GetSettings()
        {
            string CurrentSettingsFile = Application.StartupPath + "\\CustomClasses\\Settings\\Warrior_Arms.xml";
            if (System.IO.File.Exists(CurrentSettingsFile))
            {
                return
                    CurrentSetting = Settings.Load<Warrior_Arms.WarriorArmsSettings>(CurrentSettingsFile);
            }
            else
            {
                return new Warrior_Arms.WarriorArmsSettings();
            }
        }
    }

    private readonly WarriorArmsSettings MySettings = WarriorArmsSettings.GetSettings();

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

    #region Warrior Buffs

    private readonly Spell Battle_Shout = new Spell("Battle Shout");
    private readonly Spell Battle_Stance = new Spell("Battle Stance");
    private readonly Spell Berserker_Stance = new Spell("Berserker Stance");
    private readonly Spell Commanding_Shout = new Spell("Commanding Shout");
    private readonly Spell Defensive_Stance = new Spell("Defensive Stance");

    #endregion

    #region Offensive Spell

    private readonly Spell Avatar = new Spell("Avatar");
    private readonly Spell Bladestorm = new Spell("Bladestorm");
    private readonly Spell Bloodbath = new Spell("Bloodbath");
    private readonly Spell Charge = new Spell("Charge");
    private readonly Spell Cleave = new Spell("Cleave");
    private readonly Spell Colossus_Smash = new Spell("Colossus Smash");
    private readonly Spell Dragon_Roar = new Spell("Dragon Roar");
    private readonly Spell Execute = new Spell("Execute");
    private readonly Spell Heroic_Leap = new Spell("Heroic Leap");
    private readonly Spell Heroic_Strike = new Spell("Heroic Strike");
    private readonly Spell Heroic_Throw = new Spell("Heroic Throw");
    private readonly Spell Impending_Victory = new Spell("Impending Victory");
    private readonly Spell Mortal_Strike = new Spell("Mortal Strike");
    private readonly Spell Overpower = new Spell("Overpower");
    private readonly Spell Shockwave = new Spell("Shockwave");
    private readonly Spell Slam = new Spell("Slam");
    private readonly Spell Storm_Bolt = new Spell("Storm Bolt");
    private readonly Spell Taunt = new Spell("Taunt");
    private readonly Spell Thunder_Clap = new Spell("Thunder Clap");
    private readonly Spell Whirlwind = new Spell("Whirlwind");

    #endregion

    #region Offensive Cooldown

    private readonly Spell Berserker_Rage = new Spell("Berserker Rage");
    private readonly Spell Deadly_Calm = new Spell("Deadly Calm");
    private readonly Spell Recklessness = new Spell("Recklessness");
    private readonly Spell Shattering_Throw = new Spell("Shattering Throw");
    private readonly Spell Sweeping_Strikes = new Spell("Sweeping Strikes");
    private readonly Spell Skull_Banner = new Spell("Skull Banner");

    #endregion

    #region Defensive Cooldown

    private readonly Spell Die_by_the_Sword = new Spell("Die by the Sword");
    private readonly Spell Disarm = new Spell("Disarm");
    private Timer Disarm_Timer = new Timer(0);
    private readonly Spell Disrupting_Shout = new Spell("Disrupting Shout");
    private readonly Spell Hamstring = new Spell("Hamstring");
    private readonly Spell Intimidating_Shout = new Spell("Intimidating Shout");
    private readonly Spell Mass_Spell_Reflection = new Spell("Mass Spell Reflection");
    private readonly Spell Piercing_Howl = new Spell("Piercing Howl");
    private readonly Spell Pummel = new Spell("Pummel");
    private readonly Spell Staggering_Shout = new Spell("Staggering Shout");
    private readonly Spell Demoralizing_Banner = new Spell("Demoralizing Banner");

    #endregion

    #region Healing Spell

    private readonly Spell Enraged_Regeneration = new Spell("Enraged Regeneration");
    private readonly Spell Rallying_Cry = new Spell("Rallying Cry");
    private readonly Spell Victory_Rush = new Spell("Victory Rush");

    #endregion

    private Timer OnCD = new Timer(0);
    private Timer Trinket_Timer = new Timer(0);
    private Timer Engineering_Timer = new Timer(0);
    private Timer AlchFlask_Timer = new Timer(0);
    public int LC = 0;

    public Warrior_Arms()
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
                            (Charge.IsDistanceGood || Heroic_Throw.IsDistanceGood))
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
                    else if (ObjectManager.Me.IsCast)
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
        if (ObjectManager.Target.GetDistance > 15 && Charge.KnownSpell && Charge.IsSpellUsable)
        {
            Charge.Launch();
            return;
        }
        else if (Heroic_Throw.KnownSpell && Heroic_Throw.IsSpellUsable && Heroic_Throw.IsDistanceGood)
        {
            Heroic_Throw.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Target.GetDistance > 15 && Heroic_Leap.KnownSpell && Heroic_Leap.IsSpellUsable)
            {
                SpellManager.CastSpellByIDAndPosition(6544, ObjectManager.Target.Position);
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

        if (Taunt.IsSpellUsable && Taunt.IsDistanceGood && Taunt.KnownSpell && !ObjectManager.Target.InCombat
            && MySettings.UseTaunt)
        {
            Taunt.Launch();
            return;
        }

        if (Mortal_Strike.KnownSpell && Mortal_Strike.IsSpellUsable && Mortal_Strike.IsDistanceGood
            && MySettings.UseMortalStrike)
        {
            Mortal_Strike.Launch();
            return;
        }
        else if (Colossus_Smash.KnownSpell && Colossus_Smash.IsDistanceGood && Colossus_Smash.IsSpellUsable
                 && MySettings.UseColossusSmash)
        {
            Colossus_Smash.Launch();
            return;
        }
        else if (Heroic_Strike.KnownSpell && Heroic_Strike.IsSpellUsable && Heroic_Strike.IsDistanceGood
                 && MySettings.UseHeroicStrike && ObjectManager.GetNumberAttackPlayer() < 3
                 && (ObjectManager.Me.RagePercentage > 90 || ObjectManager.Me.HaveBuff(125831)))
        {
            if (Deadly_Calm.KnownSpell && Deadly_Calm.IsSpellUsable && MySettings.UseDeadlyCalm)
            {
                Deadly_Calm.Launch();
                Thread.Sleep(200);
            }

            Heroic_Strike.Launch();
            return;
        }
        else if (Shockwave.KnownSpell && Shockwave.IsSpellUsable && ObjectManager.Target.GetDistance < 10
                 && MySettings.UseShockwave)
        {
            Shockwave.Launch();
            return;
        }
        else if (Dragon_Roar.KnownSpell && Dragon_Roar.IsSpellUsable && ObjectManager.Target.GetDistance < 8
                 && MySettings.UseDragonRoar)
        {
            Dragon_Roar.Launch();
            return;
        }
        else
        {
            if (Bladestorm.KnownSpell && Bladestorm.IsSpellUsable && ObjectManager.Target.GetDistance < 8
                && MySettings.UseBladestorm)
            {
                Bladestorm.Launch();
                return;
            }
        }

        if (Thunder_Clap.KnownSpell && Thunder_Clap.IsSpellUsable && Thunder_Clap.IsDistanceGood
            && MySettings.UseThunderClap)
        {
            Thunder_Clap.Launch();
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
        else if (Berserker_Rage.KnownSpell && Berserker_Rage.IsSpellUsable && ObjectManager.Me.RagePercentage < 50
                 && MySettings.UseBerserkerRage && ObjectManager.Target.GetDistance < 30)
        {
            Berserker_Rage.Launch();
            return;
        }
        else if (Battle_Shout.KnownSpell && Battle_Shout.IsSpellUsable && ObjectManager.Me.RagePercentage < 80
                 && MySettings.UseBattleShout && ObjectManager.Target.GetDistance < 30)
        {
            Battle_Shout.Launch();
            return;
        }
        else if (Commanding_Shout.KnownSpell && Commanding_Shout.IsSpellUsable && ObjectManager.Me.RagePercentage < 80
                 && MySettings.UseCommandingShout && !MySettings.UseBattleShout && ObjectManager.Target.GetDistance < 30)
        {
            Commanding_Shout.Launch();
            return;
        }
        else if (Recklessness.KnownSpell && Recklessness.IsSpellUsable && MySettings.UseRecklessness
                 && ObjectManager.Target.GetDistance < 30)
        {
            Recklessness.Launch();
            return;
        }
        else if (Shattering_Throw.KnownSpell && Shattering_Throw.IsSpellUsable && Shattering_Throw.IsDistanceGood
                 && MySettings.UseShatteringThrow)
        {
            Shattering_Throw.Launch();
            return;
        }
        else if (Skull_Banner.KnownSpell && Skull_Banner.IsSpellUsable
                 && MySettings.UseSkullBanner && ObjectManager.Target.GetDistance < 30)
        {
            Skull_Banner.Launch();
            return;
        }
        else if (Avatar.KnownSpell && Avatar.IsSpellUsable
                 && MySettings.UseAvatar && ObjectManager.Target.GetDistance < 30)
        {
            Avatar.Launch();
            return;
        }
        else if (Bloodbath.KnownSpell && Bloodbath.IsSpellUsable
                 && MySettings.UseBloodbath && ObjectManager.Target.GetDistance < 30)
        {
            Bloodbath.Launch();
            return;
        }
        else
        {
            if (Storm_Bolt.KnownSpell && Storm_Bolt.IsSpellUsable
                && MySettings.UseStormBolt && Storm_Bolt.IsDistanceGood)
            {
                Storm_Bolt.Launch();
                return;
            }
        }
    }

    public void DPS_Cycle()
    {
        if (Taunt.IsSpellUsable && Taunt.IsDistanceGood && Taunt.KnownSpell && !ObjectManager.Target.InCombat
            && MySettings.UseTaunt)
        {
            Taunt.Launch();
            return;
        }

        if (Charge.KnownSpell && Charge.IsSpellUsable && Charge.IsDistanceGood && ObjectManager.Target.GetDistance > 8
            && MySettings.UseCharge)
        {
            Charge.Launch();
            return;
        }

        if (Victory_Rush.KnownSpell && Victory_Rush.IsSpellUsable && Victory_Rush.IsDistanceGood
            && MySettings.UseVictoryRush && ObjectManager.Me.HealthPercent < 90)
        {
            Victory_Rush.Launch();
            return;
        }
        else if (Sweeping_Strikes.KnownSpell && Sweeping_Strikes.IsSpellUsable &&
                 ObjectManager.GetNumberAttackPlayer() > 1
                 && MySettings.UseSweepingStrikes)
        {
            Sweeping_Strikes.Launch();
            return;
        }
        else if (Thunder_Clap.KnownSpell && Thunder_Clap.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() > 2
                 && MySettings.UseThunderClap)
        {
            Thunder_Clap.Launch();
            return;
        }
        else if (Whirlwind.KnownSpell && Whirlwind.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() > 3
                 && MySettings.UseWhirlwind)
        {
            Whirlwind.Launch();
            return;
        }
        else if (Cleave.KnownSpell && Cleave.IsSpellUsable && Cleave.IsDistanceGood
                 && ObjectManager.GetNumberAttackPlayer() == 3 && MySettings.UseCleave)
        {
            if (Deadly_Calm.KnownSpell && Deadly_Calm.IsSpellUsable && MySettings.UseDeadlyCalm)
            {
                Deadly_Calm.Launch();
                Thread.Sleep(200);
            }

            Cleave.Launch();
            return;
        }
        else if (Heroic_Strike.KnownSpell && Heroic_Strike.IsSpellUsable && Heroic_Strike.IsDistanceGood
                 && MySettings.UseHeroicStrike && ObjectManager.GetNumberAttackPlayer() < 3
                 && ObjectManager.Me.HaveBuff(125831))
        {
            if (Deadly_Calm.KnownSpell && Deadly_Calm.IsSpellUsable && MySettings.UseDeadlyCalm)
            {
                Deadly_Calm.Launch();
                Thread.Sleep(200);
            }

            Heroic_Strike.Launch();
            return;
        }
        else if (Shockwave.KnownSpell && Shockwave.IsSpellUsable && ObjectManager.Target.GetDistance < 10
                 && MySettings.UseShockwave)
        {
            Shockwave.Launch();
            return;
        }
        else if (Dragon_Roar.KnownSpell && Dragon_Roar.IsSpellUsable && ObjectManager.Target.GetDistance < 8
                 && MySettings.UseDragonRoar)
        {
            Dragon_Roar.Launch();
            return;
        }
        else if (Bladestorm.KnownSpell && Bladestorm.IsSpellUsable && ObjectManager.Target.GetDistance < 8
                 && MySettings.UseBladestorm)
        {
            Bladestorm.Launch();
            return;
        }
        else if (Mortal_Strike.KnownSpell && Mortal_Strike.IsSpellUsable && Mortal_Strike.IsDistanceGood
                 && MySettings.UseMortalStrike && ObjectManager.Me.RagePercentage < 100)
        {
            Mortal_Strike.Launch();
            return;
        }
        else if (Colossus_Smash.KnownSpell && Colossus_Smash.IsSpellUsable && Colossus_Smash.IsDistanceGood
                 && MySettings.UseColossusSmash)
        {
            Colossus_Smash.Launch();
            return;
        }
        else if (Execute.KnownSpell && Execute.IsSpellUsable && Execute.IsDistanceGood
                 && MySettings.UseExecute && ObjectManager.GetNumberAttackPlayer() < 4)
        {
            Execute.Launch();
            return;
        }
        else if (Overpower.KnownSpell && Overpower.IsSpellUsable && Overpower.IsDistanceGood
                 && MySettings.UseOverpower && ObjectManager.Me.RagePercentage < 100)
        {
            Overpower.Launch();
            return;
        }
        else
        {
            if (Slam.KnownSpell && Slam.IsSpellUsable && Slam.IsDistanceGood && MySettings.UseSlam
                && ObjectManager.GetNumberAttackPlayer() < 4 && ObjectManager.Target.HealthPercent > 20)
            {
                Slam.Launch();
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

        if (ObjectManager.Me.HealthPercent < 30 && MySettings.UseDefensiveStance
            && Defensive_Stance.KnownSpell && Defensive_Stance.IsSpellUsable && !Defensive_Stance.HaveBuff)
        {
            Defensive_Stance.Launch();
            return;
        }
        else if (!Battle_Stance.HaveBuff && Battle_Stance.KnownSpell && Battle_Stance.IsSpellUsable
                 && MySettings.UseBattleStance && ObjectManager.Me.HealthPercent > 50)
        {
            Battle_Stance.Launch();
            return;
        }
        else if (!Berserker_Stance.HaveBuff && Berserker_Stance.KnownSpell && Berserker_Stance.IsSpellUsable
                 && MySettings.UseBerserkerStance && !MySettings.UseBattleStance && ObjectManager.Me.HealthPercent > 50)
        {
            Berserker_Stance.Launch();
            return;
        }
        else if (Battle_Shout.KnownSpell && Battle_Shout.IsSpellUsable && !Battle_Shout.HaveBuff
                 && MySettings.UseBattleShout)
        {
            Battle_Shout.Launch();
            return;
        }
        else
        {
            if (Commanding_Shout.KnownSpell && Commanding_Shout.IsSpellUsable && !Commanding_Shout.HaveBuff
                && MySettings.UseCommandingShout && !MySettings.UseBattleShout)
            {
                Commanding_Shout.Launch();
                return;
            }
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (Victory_Rush.KnownSpell && Victory_Rush.IsSpellUsable && Victory_Rush.IsDistanceGood
            && MySettings.UseVictoryRush && ObjectManager.Me.HealthPercent < 90)
        {
            Victory_Rush.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 30 && Rallying_Cry.IsSpellUsable && Rallying_Cry.KnownSpell
                 && MySettings.UseRallyingCry && Fight.InFight)
        {
            Rallying_Cry.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && Gift_of_the_Naaru.IsSpellUsable && Gift_of_the_Naaru.KnownSpell
                 && MySettings.UseGiftoftheNaaru)
        {
            Gift_of_the_Naaru.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Me.HealthPercent < 80 && Enraged_Regeneration.IsSpellUsable &&
                Enraged_Regeneration.KnownSpell
                && MySettings.UseEnragedRegeneration)
            {
                Enraged_Regeneration.Launch();
                return;
            }
        }
    }

    private void Defense_Cycle()
    {
        if (ObjectManager.Me.HealthPercent < 95 && MySettings.UseDisarm && Disarm.IsDistanceGood
            && Disarm.KnownSpell && Disarm.IsSpellUsable && Disarm_Timer.IsReady)
        {
            Disarm.Launch();
            Disarm_Timer = new Timer(1000*60);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 20 && MySettings.UseIntimidatingShout
                 && Intimidating_Shout.KnownSpell && Intimidating_Shout.IsSpellUsable &&
                 ObjectManager.Target.GetDistance < 8)
        {
            Intimidating_Shout.Launch();
            OnCD = new Timer(1000*8);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && MySettings.UseDiebytheSword
                 && Die_by_the_Sword.KnownSpell && Die_by_the_Sword.IsSpellUsable)
        {
            Die_by_the_Sword.Launch();
            OnCD = new Timer(1000*8);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && MySettings.UseDemoralizingBanner
                 && Demoralizing_Banner.KnownSpell && Demoralizing_Banner.IsSpellUsable &&
                 ObjectManager.Target.GetDistance < 30)
        {
            SpellManager.CastSpellByIDAndPosition(114203, ObjectManager.Target.Position);
            OnCD = new Timer(1000*15);
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
        else if (!Hamstring.TargetHaveBuff && MySettings.UseHamstring && Hamstring.KnownSpell
                 && Hamstring.IsSpellUsable && Hamstring.IsDistanceGood)
        {
            Hamstring.Launch();
            return;
        }
        else if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && Pummel.IsDistanceGood
                 && Pummel.KnownSpell && Pummel.IsSpellUsable && MySettings.UsePummel)
        {
            Pummel.Launch();
            return;
        }
        else if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe &&
                 ObjectManager.Target.GetDistance < 10
                 && Disrupting_Shout.KnownSpell && Disrupting_Shout.IsSpellUsable && MySettings.UseDisruptingShout)
        {
            Disrupting_Shout.Launch();
            return;
        }
        else if (ObjectManager.Target.GetMove && !Piercing_Howl.TargetHaveBuff && MySettings.UsePiercingHowl
                 && Piercing_Howl.KnownSpell && Piercing_Howl.IsSpellUsable && ObjectManager.Target.GetDistance < 15)
        {
            Piercing_Howl.Launch();
            return;
        }
        else if (Hamstring.TargetHaveBuff && MySettings.UseStaggeringShout && Staggering_Shout.KnownSpell
                 && Staggering_Shout.IsSpellUsable && ObjectManager.Target.GetDistance < 20)
        {
            Staggering_Shout.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe &&
                MySettings.UseMassSpellReflection
                && Mass_Spell_Reflection.KnownSpell && Mass_Spell_Reflection.IsSpellUsable)
            {
                Mass_Spell_Reflection.Launch();
                return;
            }
        }
    }

    private void AvoidMelee()
    {
        while (ObjectManager.Target.GetDistance < 3 && ObjectManager.Target.InCombat)
        {
            nManager.Wow.Helpers.Keybindings.PressKeybindings(Keybindings.MOVEBACKWARD);
        }
    }
}

public class Warrior_Protection
{
    [Serializable]
    public class WarriorProtectionSettings : nManager.Helpful.Settings
    {
        /* Professions & Racials */
        public bool UseArcaneTorrent = true;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseLifeblood = true;
        public bool UseStoneform = true;
        public bool UseGiftoftheNaaru = true;
        public bool UseWarStomp = true;
        /* Warrior Buffs */
        public bool UseBattleShout = true;
        public bool UseBattleStance = true;
        public bool UseBerserkerStance = false;
        public bool UseCommandingShout = false;
        public bool UseDefensiveStance = true;
        /* Offensive Spell */
        public bool UseAvatar = true;
        public bool UseBladestorm = true;
        public bool UseBloodbath = true;
        public bool UseCharge = true;
        public bool UseCleave = true;
        public bool UseDevastate = true;
        public bool UseDragonRoar = true;
        public bool UseExecute = true;
        public bool UseHeroicLeap = true;
        public bool UseHeroicStrike = true;
        public bool UseHeroicThrow = true;
        public bool UseRevenge = true;
        public bool UseShieldSlam = true;
        public bool UseShockwave = true;
        public bool UseStormBolt = true;
        public bool UseTaunt = true;
        public bool UseThunderClap = true;
        /* Offensive Cooldown */
        public bool UseBerserkerRage = true;
        public bool UseDeadlyCalm = true;
        public bool UseRecklessness = true;
        public bool UseShatteringThrow = true;
        public bool UseSweepingStrikes = true;
        public bool UseSkullBanner = true;
        /* Defensive Cooldown */
        public bool UseDemoralizingBanner = true;
        public bool UseDemoralizingShout = true;
        public bool UseDisarm = true;
        public bool UseDisruptingShout = true;
        public bool UseHamstring = false;
        public bool UseIntimidatingShout = true;
        public bool UseMassSpellReflection = true;
        public bool UsePiercingHowl = false;
        public bool UsePummel = true;
        public bool UseShieldBarrier = true;
        public bool UseShieldBlock = true;
        public bool UseShieldWall = true;
        public bool UseSpellReflection = true;
        public bool UseStaggeringShout = true;
        /* Healing Spell */
        public bool UseEnragedRegeneration = true;
        public bool UseLastStand = true;
        public bool UseRallyingCry = true;
        public bool UseVictoryRush = true;
        /* Game Settings */
        public bool UseLowCombat = true;
        public bool UseTrinket = true;
        public bool UseEngGlove = true;
        public bool UseAlchFlask = true;

        public WarriorProtectionSettings()
        {
            ConfigWinForm(new System.Drawing.Point(400, 400), "Warrior Protection Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");
            AddControlInWinForm("Use Lifeblood", "UseLifeblood", "Professions & Racials");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            /* Warrior Buffs */
            AddControlInWinForm("Use Battle Shout", "UseBattleShout", "Warrior Buffs");
            AddControlInWinForm("Use Battle Stance", "UseBattleStance", "Warrior Buffs");
            AddControlInWinForm("Use Berserker Stance", "UseBerserkerStance", "Warrior Buffs");
            AddControlInWinForm("Use Commanding Shout", "UseCommandingShout", "Warrior Buffs");
            AddControlInWinForm("Use Defensive Stance", "UseDefensiveStance", "Warrior Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Avatar", "UseAvatar", "Offensive Spell");
            AddControlInWinForm("Use Bladestorm", "UseBladestorm", "Offensive Spell");
            AddControlInWinForm("Use Bloodbath", "UseBloodbath", "Offensive Spell");
            AddControlInWinForm("Use Charge", "UseCharge", "Offensive Spell");
            AddControlInWinForm("Use Cleave", "UseCleave", "Offensive Spell");
            AddControlInWinForm("Use Devastate", "UseDevastate", "Offensive Spell");
            AddControlInWinForm("Use Dragon Roar", "UseDragonRoar", "Offensive Spell");
            AddControlInWinForm("Use Exectue", "UseExecute", "Offensive Spell");
            AddControlInWinForm("Use Heroic Leap", "UseHeroicLeap", "Offensive Spell");
            AddControlInWinForm("Use Heroic Strike", "UseHeroicStrike", "Offensive Spell");
            AddControlInWinForm("Use Heroic Throw", "UseHeroicThrow", "Offensive Spell");
            AddControlInWinForm("Use Revenge", "UseRevenge", "Offensive Spell");
            AddControlInWinForm("Use Shield Slam", "UseShieldSlam", "Offensive Spell");
            AddControlInWinForm("Use Shockwave", "UseShockwave", "Offensive Spell");
            AddControlInWinForm("Use Storm Bolt", "UseStormBolt", "Offensive Spell");
            AddControlInWinForm("Use Taunt", "UseTaunt", "Offensive Spell");
            AddControlInWinForm("Use Thunder Clap", "UseThunderClap", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use Berserker Rage", "UseBerserkerRage", "Offensive Cooldown");
            AddControlInWinForm("Use Deadly Calm", "UseDeadlyCalm", "Offensive Cooldown");
            AddControlInWinForm("Use Recklessness", "UseRecklessness", "Offensive Cooldown");
            AddControlInWinForm("Use Shattering Throw", "UseShatteringThrow", "Offensive Cooldown");
            AddControlInWinForm("Use Sweeping Strikes", "UseSweepingStrikes", "Offensive Cooldown");
            AddControlInWinForm("Use Skull Banner", "UseSkullBanner", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Demoralizing Banner", "UseDemoralizingBanner", "Defensive Cooldown");
            AddControlInWinForm("Use Demoralizing Shout", "UseDemoralizingShout", "Defensive Cooldown");
            AddControlInWinForm("Use Disarm", "UseDisarm", "Defensive Cooldown");
            AddControlInWinForm("Use Disrupting Shout", "UseDisruptingShout", "Defensive Cooldown");
            AddControlInWinForm("Use Hamstring", "UseHamstring", "Defensive Cooldown");
            AddControlInWinForm("Use Intimidating Shout", "UseIntimidatingShout", "Defensive Cooldown");
            AddControlInWinForm("Use Mass Spell Reflection", "UseMassSpellReflection", "Defensive Cooldown");
            AddControlInWinForm("Use Piercing Howl", "UsePiercingHowl", "Defensive Cooldown");
            AddControlInWinForm("Use Pummel", "UsePummel", "Defensive Cooldown");
            AddControlInWinForm("Use Shield Barrier", "UseShieldBarrier", "Defensive Cooldown");
            AddControlInWinForm("Use Shield Block", "UseShieldBlock", "Defensive Cooldown");
            AddControlInWinForm("Use Shield Wall", "UseShieldWall", "Defensive Cooldown");
            AddControlInWinForm("Use Spell Reflection", "UseSpellReflection", "Defensive Cooldown");
            AddControlInWinForm("Use Staggering Shout", "UseStaggeringShout", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Enraged Regeneration", "UseEnragedRegeneration", "Healing Spell");
            AddControlInWinForm("Use Last Stand", "UseLastStand", "Healing Spell");
            AddControlInWinForm("Use Rallying Cry", "UseRallyingCry", "Healing Spell");
            AddControlInWinForm("Use Victory Rush", "UseVictoryRush", "Healing Spell");
            /* Game Settings */
            AddControlInWinForm("Use Low Combat Settings", "UseLowCombat", "Game Settings");
            AddControlInWinForm("Use Trinket", "UseTrinket", "Game Settings");
            AddControlInWinForm("Use Engineering Gloves", "UseEngGlove", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
        }

        public static WarriorProtectionSettings CurrentSetting { get; set; }

        public static WarriorProtectionSettings GetSettings()
        {
            string CurrentSettingsFile = Application.StartupPath + "\\CustomClasses\\Settings\\Warrior_Protection.xml";
            if (System.IO.File.Exists(CurrentSettingsFile))
            {
                return
                    CurrentSetting = Settings.Load<Warrior_Protection.WarriorProtectionSettings>(CurrentSettingsFile);
            }
            else
            {
                return new Warrior_Protection.WarriorProtectionSettings();
            }
        }
    }

    private readonly WarriorProtectionSettings MySettings = WarriorProtectionSettings.GetSettings();

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

    #region Warrior Buffs

    private readonly Spell Battle_Shout = new Spell("Battle Shout");
    private readonly Spell Battle_Stance = new Spell("Battle Stance");
    private readonly Spell Berserker_Stance = new Spell("Berserker Stance");
    private readonly Spell Commanding_Shout = new Spell("Commanding Shout");
    private readonly Spell Defensive_Stance = new Spell("Defensive Stance");

    #endregion

    #region Offensive Spell

    private readonly Spell Avatar = new Spell("Avatar");
    private readonly Spell Bladestorm = new Spell("Bladestorm");
    private readonly Spell Bloodbath = new Spell("Bloodbath");
    private readonly Spell Charge = new Spell("Charge");
    private readonly Spell Cleave = new Spell("Cleave");
    private readonly Spell Devastate = new Spell("Devastate");
    private readonly Spell Dragon_Roar = new Spell("Dragon Roar");
    private readonly Spell Execute = new Spell("Execute");
    private readonly Spell Heroic_Leap = new Spell("Heroic Leap");
    private readonly Spell Heroic_Strike = new Spell("Heroic Strike");
    private readonly Spell Heroic_Throw = new Spell("Heroic Throw");
    private readonly Spell Revenge = new Spell("Revenge");
    private readonly Spell Shield_Slam = new Spell("Shield Slam");
    private readonly Spell Shockwave = new Spell("Shockwave");
    private readonly Spell Storm_Bolt = new Spell("Storm Bolt");
    private readonly Spell Sunder_Armor = new Spell("Sunder Armor");
    private readonly Spell Taunt = new Spell("Taunt");
    private readonly Spell Thunder_Clap = new Spell("Thunder Clap");

    #endregion

    #region Offensive Cooldown

    private readonly Spell Berserker_Rage = new Spell("Berserker Rage");
    private readonly Spell Deadly_Calm = new Spell("Deadly Calm");
    private readonly Spell Recklessness = new Spell("Recklessness");
    private readonly Spell Shattering_Throw = new Spell("Shattering Throw");
    private readonly Spell Sweeping_Strikes = new Spell("Sweeping Strikes");
    private readonly Spell Skull_Banner = new Spell("Skull Banner");

    #endregion

    #region Defensive Cooldown

    private readonly Spell Demoralizing_Banner = new Spell("Demoralizing Banner");
    private readonly Spell Demoralizing_Shout = new Spell("Demoralizing Shout");
    private readonly Spell Disarm = new Spell("Disarm");
    private Timer Disarm_Timer = new Timer(0);
    private readonly Spell Disrupting_Shout = new Spell("Disrupting Shout");
    private readonly Spell Hamstring = new Spell("Hamstring");
    private readonly Spell Intimidating_Shout = new Spell("Intimidating Shout");
    private readonly Spell Mass_Spell_Reflection = new Spell("Mass Spell Reflection");
    private readonly Spell Piercing_Howl = new Spell("Piercing Howl");
    private readonly Spell Pummel = new Spell("Pummel");
    private readonly Spell Shield_Barrier = new Spell("Shield Barrier");
    private Timer Shield_Barrier_Timer = new Timer(0);
    private readonly Spell Shield_Block = new Spell("Shield Block");
    private readonly Spell Shield_Wall = new Spell("Shield Wall");
    private readonly Spell Spell_Reflection = new Spell("Spell Reflection");
    private readonly Spell Staggering_Shout = new Spell("Staggering Shout");

    #endregion

    #region Healing Spell

    private readonly Spell Enraged_Regeneration = new Spell("Enraged Regeneration");
    private readonly Spell Last_Stand = new Spell("Last Stand");
    private readonly Spell Rallying_Cry = new Spell("Rallying Cry");
    private readonly Spell Victory_Rush = new Spell("Victory Rush");

    #endregion

    private Timer OnCD = new Timer(0);
    private Timer Trinket_Timer = new Timer(0);
    private Timer Engineering_Timer = new Timer(0);
    private Timer AlchFlask_Timer = new Timer(0);
    public int LC = 0;

    public Warrior_Protection()
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
                            (Charge.IsDistanceGood || Heroic_Throw.IsDistanceGood))
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
                    else if (ObjectManager.Me.IsCast)
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
        if (ObjectManager.Target.GetDistance > 15 && Charge.KnownSpell && Charge.IsSpellUsable)
        {
            Charge.Launch();
            return;
        }
        else if (Heroic_Throw.KnownSpell && Heroic_Throw.IsSpellUsable && Heroic_Throw.IsDistanceGood)
        {
            Heroic_Throw.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Target.GetDistance > 15 && Heroic_Leap.KnownSpell && Heroic_Leap.IsSpellUsable)
            {
                SpellManager.CastSpellByIDAndPosition(6544, ObjectManager.Target.Position);
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

        if (Taunt.IsSpellUsable && Taunt.IsDistanceGood && Taunt.KnownSpell && !ObjectManager.Target.InCombat
            && MySettings.UseTaunt)
        {
            Taunt.Launch();
            return;
        }

        if (Shield_Slam.KnownSpell && Shield_Slam.IsSpellUsable && Shield_Slam.IsDistanceGood
            && ObjectManager.Me.RagePercentage < 95 && MySettings.UseShieldSlam)
        {
            Shield_Slam.Launch();
            return;
        }
        else if (Heroic_Strike.KnownSpell && Heroic_Strike.IsSpellUsable && Heroic_Strike.IsDistanceGood
                 && MySettings.UseHeroicStrike &&
                 (ObjectManager.Me.RagePercentage > 80 || ObjectManager.Me.HaveBuff(122510)))
        {
            if (ObjectManager.Me.HealthPercent > 80)
            {
                if (Deadly_Calm.KnownSpell && Deadly_Calm.IsSpellUsable && MySettings.UseDeadlyCalm)
                {
                    Deadly_Calm.Launch();
                    Thread.Sleep(200);
                }
                Heroic_Strike.Launch();
                return;
            }
        }
        else if (Revenge.KnownSpell && Revenge.IsDistanceGood && Revenge.IsSpellUsable
                 && ObjectManager.Me.RagePercentage < 95 && MySettings.UseRevenge)
        {
            Revenge.Launch();
            return;
        }
        else if (Shockwave.KnownSpell && Shockwave.IsSpellUsable && Shockwave.IsDistanceGood
                 && MySettings.UseShockwave)
        {
            Shockwave.Launch();
            return;
        }
        else if (Dragon_Roar.KnownSpell && Dragon_Roar.IsSpellUsable && Dragon_Roar.IsDistanceGood
                 && MySettings.UseDragonRoar)
        {
            Dragon_Roar.Launch();
            return;
        }
        else if (Bladestorm.KnownSpell && Bladestorm.IsSpellUsable && Bladestorm.IsDistanceGood
                 && MySettings.UseBladestorm)
        {
            Bladestorm.Launch();
            return;
        }
        else
        {
            // Blizzard API Calls for Devastate using Sunder Armor Function
            if (Sunder_Armor.KnownSpell && Sunder_Armor.IsSpellUsable && Sunder_Armor.IsDistanceGood
                && MySettings.UseDevastate)
            {
                Sunder_Armor.Launch();
                return;
            }
        }

        if (Thunder_Clap.KnownSpell && Thunder_Clap.IsSpellUsable && Thunder_Clap.IsDistanceGood
            && MySettings.UseThunderClap)
        {
            Thunder_Clap.Launch();
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
        else if (Berserker_Rage.KnownSpell && Berserker_Rage.IsSpellUsable && ObjectManager.Me.RagePercentage < 50
                 && MySettings.UseBerserkerRage && ObjectManager.Target.GetDistance < 30)
        {
            Berserker_Rage.Launch();
            return;
        }
        else if (Battle_Shout.KnownSpell && Battle_Shout.IsSpellUsable && ObjectManager.Me.RagePercentage < 80
                 && MySettings.UseBattleShout && ObjectManager.Target.GetDistance < 30)
        {
            Battle_Shout.Launch();
            return;
        }
        else if (Commanding_Shout.KnownSpell && Commanding_Shout.IsSpellUsable && ObjectManager.Me.RagePercentage < 80
                 && MySettings.UseCommandingShout && !MySettings.UseBattleShout && ObjectManager.Target.GetDistance < 30)
        {
            Commanding_Shout.Launch();
            return;
        }
        else if (Recklessness.KnownSpell && Recklessness.IsSpellUsable && MySettings.UseRecklessness
                 && ObjectManager.Target.GetDistance < 30)
        {
            Recklessness.Launch();
            return;
        }
        else if (Shattering_Throw.KnownSpell && Shattering_Throw.IsSpellUsable && Shattering_Throw.IsDistanceGood
                 && MySettings.UseShatteringThrow)
        {
            Shattering_Throw.Launch();
            return;
        }
        else if (Skull_Banner.KnownSpell && Skull_Banner.IsSpellUsable
                 && MySettings.UseSkullBanner && ObjectManager.Target.GetDistance < 30)
        {
            Skull_Banner.Launch();
            return;
        }
        else if (Avatar.KnownSpell && Avatar.IsSpellUsable
                 && MySettings.UseAvatar && ObjectManager.Target.GetDistance < 30)
        {
            Avatar.Launch();
            return;
        }
        else if (Bloodbath.KnownSpell && Bloodbath.IsSpellUsable
                 && MySettings.UseBloodbath && ObjectManager.Target.GetDistance < 30)
        {
            Bloodbath.Launch();
            return;
        }
        else
        {
            if (Storm_Bolt.KnownSpell && Storm_Bolt.IsSpellUsable
                && MySettings.UseStormBolt && Storm_Bolt.IsDistanceGood)
            {
                Storm_Bolt.Launch();
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

    public void DPS_Cycle()
    {
        if (Taunt.IsSpellUsable && Taunt.IsDistanceGood && Taunt.KnownSpell && !ObjectManager.Target.InCombat
            && MySettings.UseTaunt)
        {
            Taunt.Launch();
            return;
        }

        if (Victory_Rush.KnownSpell && Victory_Rush.IsSpellUsable && Victory_Rush.IsDistanceGood
            && MySettings.UseVictoryRush && ObjectManager.Me.HealthPercent < 90)
        {
            Victory_Rush.Launch();
            return;
        }

        if (ObjectManager.GetNumberAttackPlayer() > 2 && Thunder_Clap.KnownSpell && Thunder_Clap.IsSpellUsable
            && Thunder_Clap.IsDistanceGood && MySettings.UseThunderClap)
        {
            Thunder_Clap.Launch();
            return;
        }

        if (Cleave.KnownSpell && Cleave.IsSpellUsable && Cleave.IsDistanceGood &&
            ObjectManager.GetNumberAttackPlayer() > 2
            && MySettings.UseCleave && (ObjectManager.Me.RagePercentage > 80 || ObjectManager.Me.HaveBuff(122510)))
        {
            if (ObjectManager.Me.HealthPercent > 80)
            {
                if (Deadly_Calm.KnownSpell && Deadly_Calm.IsSpellUsable && MySettings.UseDeadlyCalm)
                {
                    Deadly_Calm.Launch();
                    Thread.Sleep(200);
                }
                Cleave.Launch();
                return;
            }
        }

        else
        {
            if (Heroic_Strike.KnownSpell && Heroic_Strike.IsSpellUsable && Heroic_Strike.IsDistanceGood
                && MySettings.UseHeroicStrike &&
                (ObjectManager.Me.RagePercentage > 80 || ObjectManager.Me.HaveBuff(122510)))
            {
                if (ObjectManager.Me.HealthPercent > 80)
                {
                    if (Deadly_Calm.KnownSpell && Deadly_Calm.IsSpellUsable && MySettings.UseDeadlyCalm)
                    {
                        Deadly_Calm.Launch();
                        Thread.Sleep(200);
                    }
                    Heroic_Strike.Launch();
                    return;
                }
            }
        }

        if (Shield_Slam.KnownSpell && Shield_Slam.IsSpellUsable && Shield_Slam.IsDistanceGood
            && MySettings.UseShieldSlam && ObjectManager.Me.RagePercentage < 95)
        {
            Shield_Slam.Launch();
            return;
        }
        else if (Revenge.KnownSpell && Revenge.IsDistanceGood && Revenge.IsSpellUsable
                 && MySettings.UseRevenge && ObjectManager.Me.RagePercentage < 95)
        {
            Revenge.Launch();
            return;
        }
        else if (Shockwave.KnownSpell && Shockwave.IsSpellUsable && Shockwave.IsDistanceGood
                 && MySettings.UseShockwave)
        {
            Shockwave.Launch();
            return;
        }
        else if (Dragon_Roar.KnownSpell && Dragon_Roar.IsSpellUsable && Dragon_Roar.IsDistanceGood
                 && MySettings.UseDragonRoar)
        {
            Shockwave.Launch();
            return;
        }
        else if (Bladestorm.KnownSpell && Bladestorm.IsSpellUsable && Bladestorm.IsDistanceGood
                 && MySettings.UseBladestorm)
        {
            Bladestorm.Launch();
            return;
        }
        else if (Thunder_Clap.KnownSpell && Thunder_Clap.IsSpellUsable && Thunder_Clap.IsDistanceGood
                 && MySettings.UseThunderClap && !ObjectManager.Target.HaveBuff(115798))
        {
            Thunder_Clap.Launch();
            return;
        }
        else if (Battle_Shout.KnownSpell && Battle_Shout.IsSpellUsable && MySettings.UseBattleShout)
        {
            Battle_Shout.Launch();
            return;
        }
        else if (Commanding_Shout.KnownSpell && Commanding_Shout.IsSpellUsable && MySettings.UseCommandingShout
                 && !MySettings.UseBattleShout)
        {
            Commanding_Shout.Launch();
            return;
        }
        else
        {
            if (Sunder_Armor.KnownSpell && Sunder_Armor.IsSpellUsable && Sunder_Armor.IsDistanceGood
                && MySettings.UseDevastate)
            {
                Sunder_Armor.Launch();
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

        if (MySettings.UseDefensiveStance && Defensive_Stance.KnownSpell && Defensive_Stance.IsSpellUsable
            && !Defensive_Stance.HaveBuff && LC != 1)
        {
            Defensive_Stance.Launch();
            return;
        }
        else if (!Battle_Stance.HaveBuff && Battle_Stance.KnownSpell && Battle_Stance.IsSpellUsable
                 && MySettings.UseBattleStance && LC == 1)
        {
            Battle_Stance.Launch();
            return;
        }
        else if (!Berserker_Stance.HaveBuff && Berserker_Stance.KnownSpell && Berserker_Stance.IsSpellUsable
                 && MySettings.UseBerserkerStance && !MySettings.UseBattleStance && !MySettings.UseDefensiveStance)
        {
            Berserker_Stance.Launch();
            return;
        }
        else if (Battle_Shout.KnownSpell && Battle_Shout.IsSpellUsable && !Battle_Shout.HaveBuff
                 && MySettings.UseBattleShout)
        {
            Battle_Shout.Launch();
            return;
        }
        else
        {
            if (Commanding_Shout.KnownSpell && Commanding_Shout.IsSpellUsable && !Commanding_Shout.HaveBuff
                && MySettings.UseCommandingShout && !MySettings.UseBattleShout)
            {
                Commanding_Shout.Launch();
                return;
            }
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (Victory_Rush.KnownSpell && Victory_Rush.IsSpellUsable && Victory_Rush.IsDistanceGood
            && MySettings.UseVictoryRush && ObjectManager.Me.HealthPercent < 90)
        {
            Victory_Rush.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 30 && Last_Stand.IsSpellUsable && Last_Stand.KnownSpell
                 && MySettings.UseLastStand && Fight.InFight)
        {
            Last_Stand.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 30 && Rallying_Cry.IsSpellUsable && Rallying_Cry.KnownSpell
                 && MySettings.UseRallyingCry && Fight.InFight && !Last_Stand.HaveBuff)
        {
            Rallying_Cry.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && Gift_of_the_Naaru.IsSpellUsable && Gift_of_the_Naaru.KnownSpell
                 && MySettings.UseGiftoftheNaaru)
        {
            Gift_of_the_Naaru.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Me.HealthPercent < 80 && Enraged_Regeneration.IsSpellUsable &&
                Enraged_Regeneration.KnownSpell
                && MySettings.UseEnragedRegeneration)
            {
                Enraged_Regeneration.Launch();
                return;
            }
        }
    }

    private void Defense_Cycle()
    {
        if (ObjectManager.Me.HealthPercent < 95 && MySettings.UseDisarm && Disarm.IsDistanceGood
            && Disarm.KnownSpell && Disarm.IsSpellUsable && Disarm_Timer.IsReady)
        {
            Disarm.Launch();
            Disarm_Timer = new Timer(1000*60);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 20 && MySettings.UseIntimidatingShout
                 && Intimidating_Shout.KnownSpell && Intimidating_Shout.IsSpellUsable &&
                 ObjectManager.Target.GetDistance < 8)
        {
            Intimidating_Shout.Launch();
            OnCD = new Timer(1000*8);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 60 && Shield_Wall.KnownSpell && Shield_Wall.IsSpellUsable
                 && MySettings.UseShieldWall)
        {
            Shield_Wall.Launch();
            OnCD = new Timer(1000*12);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && MySettings.UseDemoralizingBanner
                 && Demoralizing_Banner.KnownSpell && Demoralizing_Banner.IsSpellUsable &&
                 ObjectManager.Target.GetDistance < 30)
        {
            SpellManager.CastSpellByIDAndPosition(114203, ObjectManager.Target.Position);
            OnCD = new Timer(1000*15);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 90 && MySettings.UseDemoralizingShout
                 && Demoralizing_Shout.KnownSpell && Demoralizing_Shout.IsSpellUsable &&
                 ObjectManager.Target.GetDistance < 30)
        {
            Demoralizing_Shout.Launch();
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
            if (ObjectManager.Me.HealthPercent < 80 && Shield_Block.KnownSpell && Shield_Block.IsSpellUsable
                && MySettings.UseShieldBlock)
            {
                Shield_Block.Launch();
                OnCD = new Timer(1000*6);
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
        else if (!Hamstring.TargetHaveBuff && MySettings.UseHamstring && Hamstring.KnownSpell
                 && Hamstring.IsSpellUsable && Hamstring.IsDistanceGood)
        {
            Hamstring.Launch();
            return;
        }
        else if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && Pummel.IsDistanceGood
                 && Pummel.KnownSpell && Pummel.IsSpellUsable && MySettings.UsePummel)
        {
            Pummel.Launch();
            return;
        }
        else if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe &&
                 ObjectManager.Target.GetDistance < 10
                 && Disrupting_Shout.KnownSpell && Disrupting_Shout.IsSpellUsable && MySettings.UseDisruptingShout)
        {
            Disrupting_Shout.Launch();
            return;
        }
        else if (ObjectManager.Target.GetMove && !Piercing_Howl.TargetHaveBuff && MySettings.UsePiercingHowl
                 && Piercing_Howl.KnownSpell && Piercing_Howl.IsSpellUsable && ObjectManager.Target.GetDistance < 15)
        {
            Piercing_Howl.Launch();
            return;
        }
        else if (Hamstring.TargetHaveBuff && MySettings.UseStaggeringShout && Staggering_Shout.KnownSpell
                 && Staggering_Shout.IsSpellUsable && ObjectManager.Target.GetDistance < 20)
        {
            Staggering_Shout.Launch();
            return;
        }
        else if (ObjectManager.Target.IsCast && Spell_Reflection.KnownSpell && Spell_Reflection.IsSpellUsable
                 && MySettings.UseSpellReflection)
        {
            Spell_Reflection.Launch();
            return;
        }
        else if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe &&
                 MySettings.UseMassSpellReflection
                 && Mass_Spell_Reflection.KnownSpell && Mass_Spell_Reflection.IsSpellUsable)
        {
            Mass_Spell_Reflection.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe
                && ObjectManager.Me.HealthPercent < 80 && Shield_Barrier.KnownSpell
                && Shield_Barrier.IsSpellUsable && MySettings.UseShieldBarrier && Shield_Barrier_Timer.IsReady)
            {
                Shield_Barrier.Launch();
                Shield_Barrier_Timer = new Timer(1000*6);
                return;
            }
        }
    }

    private void AvoidMelee()
    {
        while (ObjectManager.Target.GetDistance < 3 && ObjectManager.Target.InCombat)
        {
            nManager.Wow.Helpers.Keybindings.PressKeybindings(Keybindings.MOVEBACKWARD);
        }
    }
}

public class Warrior_Fury
{
    [Serializable]
    public class WarriorFurySettings : nManager.Helpful.Settings
    {
        /* Professions & Racials */
        public bool UseArcaneTorrent = true;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseLifeblood = true;
        public bool UseStoneform = true;
        public bool UseGiftoftheNaaru = true;
        public bool UseWarStomp = true;
        /* Warrior Buffs */
        public bool UseBattleShout = true;
        public bool UseBattleStance = true;
        public bool UseBerserkerStance = false;
        public bool UseCommandingShout = false;
        public bool UseDefensiveStance = true;
        /* Offensive Spell */
        public bool UseAvatar = true;
        public bool UseBladestorm = true;
        public bool UseBloodbath = true;
        public bool UseBloodthirst = true;
        public bool UseCharge = true;
        public bool UseCleave = true;
        public bool UseColossusSmash = true;
        public bool UseDragonRoar = true;
        public bool UseExecute = true;
        public bool UseHeroicLeap = true;
        public bool UseHeroicStrike = true;
        public bool UseHeroicThrow = true;
        public bool UseRagingBlow = true;
        public bool UseShockwave = true;
        public bool UseStormBolt = true;
        public bool UseTaunt = true;
        public bool UseThunderClap = true;
        public bool UseWhirlwind = true;
        public bool UseWildStrike = true;
        /* Offensive Cooldown */
        public bool UseBerserkerRage = true;
        public bool UseDeadlyCalm = true;
        public bool UseRecklessness = true;
        public bool UseShatteringThrow = true;
        public bool UseSweepingStrikes = true;
        public bool UseSkullBanner = true;
        /* Defensive Cooldown */
        public bool UseDemoralizingBanner = true;
        public bool UseDiebytheSword = true;
        public bool UseDisarm = true;
        public bool UseDisruptingShout = true;
        public bool UseHamstring = false;
        public bool UseIntimidatingShout = true;
        public bool UseMassSpellReflection = true;
        public bool UsePiercingHowl = false;
        public bool UsePummel = true;
        public bool UseStaggeringShout = true;
        /* Healing Spell */
        public bool UseEnragedRegeneration = true;
        public bool UseRallyingCry = true;
        public bool UseVictoryRush = true;
        /* Game Settings */
        public bool UseLowCombat = true;
        public bool UseTrinket = true;
        public bool UseEngGlove = true;
        public bool UseAlchFlask = true;

        public WarriorFurySettings()
        {
            ConfigWinForm(new System.Drawing.Point(400, 400), "Warrior Fury Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");
            AddControlInWinForm("Use Lifeblood", "UseLifeblood", "Professions & Racials");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            /* Warrior Buffs */
            AddControlInWinForm("Use Battle Shout", "UseBattleShout", "Warrior Buffs");
            AddControlInWinForm("Use Battle Stance", "UseBattleStance", "Warrior Buffs");
            AddControlInWinForm("Use Berserker Stance", "UseBerserkerStance", "Warrior Buffs");
            AddControlInWinForm("Use Commanding Shout", "UseCommandingShout", "Warrior Buffs");
            AddControlInWinForm("Use Defensive Stance", "UseDefensiveStance", "Warrior Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Avatar", "UseAvatar", "Offensive Spell");
            AddControlInWinForm("Use Bladestorm", "UseBladestorm", "Offensive Spell");
            AddControlInWinForm("Use Bloodbath", "UseBloodbath", "Offensive Spell");
            AddControlInWinForm("Use Bloodthirst", "UseBloodthirst", "Offensive Spell");
            AddControlInWinForm("Use Charge", "UseCharge", "Offensive Spell");
            AddControlInWinForm("Use Cleave", "UseCleave", "Offensive Spell");
            AddControlInWinForm("Use Colossus Smash", "UseColossusSmash", "Offensive Spell");
            AddControlInWinForm("Use Dragon Roar", "UseDragonRoar", "Offensive Spell");
            AddControlInWinForm("Use Exectue", "UseExecute", "Offensive Spell");
            AddControlInWinForm("Use Heroic Leap", "UseHeroicLeap", "Offensive Spell");
            AddControlInWinForm("Use Heroic Strike", "UseHeroicStrike", "Offensive Spell");
            AddControlInWinForm("Use Heroic Throw", "UseHeroicThrow", "Offensive Spell");
            AddControlInWinForm("Use Raging Blow", "UseRagingBlow", "Offensive Spell");
            AddControlInWinForm("Use Shockwave", "UseShockwave", "Offensive Spell");
            AddControlInWinForm("Use Storm Bolt", "UseStormBolt", "Offensive Spell");
            AddControlInWinForm("Use Taunt", "UseTaunt", "Offensive Spell");
            AddControlInWinForm("Use Thunder Clap", "UseThunderClap", "Offensive Spell");
            AddControlInWinForm("Use Whirlwind", "UseWhirlwind", "Offensive Spell");
            AddControlInWinForm("Use Wild Strike", "UseWildStrike", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use Berserker Rage", "UseBerserkerRage", "Offensive Cooldown");
            AddControlInWinForm("Use Deadly Calm", "UseDeadlyCalm", "Offensive Cooldown");
            AddControlInWinForm("Use Recklessness", "UseRecklessness", "Offensive Cooldown");
            AddControlInWinForm("Use Shattering Throw", "UseShatteringThrow", "Offensive Cooldown");
            AddControlInWinForm("Use Sweeping Strikes", "UseSweepingStrikes", "Offensive Cooldown");
            AddControlInWinForm("Use Skull Banner", "UseSkullBanner", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Demoralizing Banner", "UseDemoralizingBanner", "Defensive Cooldown");
            AddControlInWinForm("Use Die by the Sword", "UseDiebytheSword", "Defensive Cooldown");
            AddControlInWinForm("Use Disarm", "UseDisarm", "Defensive Cooldown");
            AddControlInWinForm("Use Disrupting Shout", "UseDisruptingShout", "Defensive Cooldown");
            AddControlInWinForm("Use Hamstring", "UseHamstring", "Defensive Cooldown");
            AddControlInWinForm("Use Intimidating Shout", "UseIntimidatingShout", "Defensive Cooldown");
            AddControlInWinForm("Use Mass Spell Reflection", "UseMassSpellReflection", "Defensive Cooldown");
            AddControlInWinForm("Use Piercing Howl", "UsePiercingHowl", "Defensive Cooldown");
            AddControlInWinForm("Use Pummel", "UsePummel", "Defensive Cooldown");
            AddControlInWinForm("Use Staggering Shout", "UseStaggeringShout", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Enraged Regeneration", "UseEnragedRegeneration", "Healing Spell");
            AddControlInWinForm("Use Rallying Cry", "UseRallyingCry", "Healing Spell");
            AddControlInWinForm("Use Victory Rush", "UseVictoryRush", "Healing Spell");
            /* Game Settings */
            AddControlInWinForm("Use Low Combat Settings", "UseLowCombat", "Game Settings");
            AddControlInWinForm("Use Trinket", "UseTrinket", "Game Settings");
            AddControlInWinForm("Use Engineering Gloves", "UseEngGlove", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
        }

        public static WarriorFurySettings CurrentSetting { get; set; }

        public static WarriorFurySettings GetSettings()
        {
            string CurrentSettingsFile = Application.StartupPath + "\\CustomClasses\\Settings\\Warrior_Fury.xml";
            if (System.IO.File.Exists(CurrentSettingsFile))
            {
                return
                    CurrentSetting = Settings.Load<Warrior_Fury.WarriorFurySettings>(CurrentSettingsFile);
            }
            else
            {
                return new Warrior_Fury.WarriorFurySettings();
            }
        }
    }

    private readonly WarriorFurySettings MySettings = WarriorFurySettings.GetSettings();

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

    #region Warrior Buffs

    private readonly Spell Battle_Shout = new Spell("Battle Shout");
    private readonly Spell Battle_Stance = new Spell("Battle Stance");
    private readonly Spell Berserker_Stance = new Spell("Berserker Stance");
    private readonly Spell Commanding_Shout = new Spell("Commanding Shout");
    private readonly Spell Defensive_Stance = new Spell("Defensive Stance");

    #endregion

    #region Offensive Spell

    private readonly Spell Avatar = new Spell("Avatar");
    private readonly Spell Bladestorm = new Spell("Bladestorm");
    private readonly Spell Bloodbath = new Spell("Bloodbath");
    private readonly Spell Bloodthirst = new Spell("Bloodthirst");
    private readonly Spell Charge = new Spell("Charge");
    private readonly Spell Cleave = new Spell("Cleave");
    private readonly Spell Colossus_Smash = new Spell("Colossus Smash");
    private readonly Spell Dragon_Roar = new Spell("Dragon Roar");
    private readonly Spell Execute = new Spell("Execute");
    private readonly Spell Heroic_Leap = new Spell("Heroic Leap");
    private readonly Spell Heroic_Strike = new Spell("Heroic Strike");
    private readonly Spell Heroic_Throw = new Spell("Heroic Throw");
    private readonly Spell Impending_Victory = new Spell("Impending Victory");
    private readonly Spell Raging_Blow = new Spell("Raging Blow");
    private readonly Spell Shockwave = new Spell("Shockwave");
    private readonly Spell Storm_Bolt = new Spell("Storm Bolt");
    private readonly Spell Taunt = new Spell("Taunt");
    private readonly Spell Thunder_Clap = new Spell("Thunder Clap");
    private readonly Spell Whirlwind = new Spell("Whirlwind");
    private readonly Spell Wild_Strike = new Spell("Wild Strike");

    #endregion

    #region Offensive Cooldown

    private readonly Spell Berserker_Rage = new Spell("Berserker Rage");
    private readonly Spell Deadly_Calm = new Spell("Deadly Calm");
    private readonly Spell Recklessness = new Spell("Recklessness");
    private readonly Spell Shattering_Throw = new Spell("Shattering Throw");
    private readonly Spell Sweeping_Strikes = new Spell("Sweeping Strikes");
    private readonly Spell Skull_Banner = new Spell("Skull Banner");

    #endregion

    #region Defensive Cooldown

    private readonly Spell Die_by_the_Sword = new Spell("Die by the Sword");
    private readonly Spell Disarm = new Spell("Disarm");
    private Timer Disarm_Timer = new Timer(0);
    private readonly Spell Disrupting_Shout = new Spell("Disrupting Shout");
    private readonly Spell Hamstring = new Spell("Hamstring");
    private readonly Spell Intimidating_Shout = new Spell("Intimidating Shout");
    private readonly Spell Mass_Spell_Reflection = new Spell("Mass Spell Reflection");
    private readonly Spell Piercing_Howl = new Spell("Piercing Howl");
    private readonly Spell Pummel = new Spell("Pummel");
    private readonly Spell Staggering_Shout = new Spell("Staggering Shout");
    private readonly Spell Demoralizing_Banner = new Spell("Demoralizing Banner");

    #endregion

    #region Healing Spell

    private readonly Spell Enraged_Regeneration = new Spell("Enraged Regeneration");
    private readonly Spell Rallying_Cry = new Spell("Rallying Cry");
    private readonly Spell Victory_Rush = new Spell("Victory Rush");

    #endregion

    private Timer OnCD = new Timer(0);
    private Timer Trinket_Timer = new Timer(0);
    private Timer Engineering_Timer = new Timer(0);
    private Timer AlchFlask_Timer = new Timer(0);
    public int LC = 0;

    public Warrior_Fury()
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
                            (Charge.IsDistanceGood || Heroic_Throw.IsDistanceGood))
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
                    else if (ObjectManager.Me.IsCast)
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
        if (ObjectManager.Target.GetDistance > 15 && Charge.KnownSpell && Charge.IsSpellUsable)
        {
            Charge.Launch();
            return;
        }
        else if (Heroic_Throw.KnownSpell && Heroic_Throw.IsSpellUsable && Heroic_Throw.IsDistanceGood)
        {
            Heroic_Throw.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Target.GetDistance > 15 && Heroic_Leap.KnownSpell && Heroic_Leap.IsSpellUsable)
            {
                SpellManager.CastSpellByIDAndPosition(6544, ObjectManager.Target.Position);
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

        if (Taunt.IsSpellUsable && Taunt.IsDistanceGood && Taunt.KnownSpell && !ObjectManager.Target.InCombat
            && MySettings.UseTaunt)
        {
            Taunt.Launch();
            return;
        }

        if (Bloodthirst.KnownSpell && Bloodthirst.IsSpellUsable && Bloodthirst.IsDistanceGood
            && MySettings.UseBloodthirst)
        {
            Bloodthirst.Launch();
            return;
        }
        else if (Colossus_Smash.KnownSpell && Colossus_Smash.IsDistanceGood && Colossus_Smash.IsSpellUsable
                 && MySettings.UseColossusSmash)
        {
            Colossus_Smash.Launch();
            return;
        }
        else if (Heroic_Strike.KnownSpell && Heroic_Strike.IsSpellUsable && Heroic_Strike.IsDistanceGood
                 && MySettings.UseHeroicStrike && ObjectManager.GetNumberAttackPlayer() < 3
                 && ObjectManager.Me.RagePercentage > 80)
        {
            if (Deadly_Calm.KnownSpell && Deadly_Calm.IsSpellUsable && MySettings.UseDeadlyCalm)
            {
                Deadly_Calm.Launch();
                Thread.Sleep(200);
            }

            Heroic_Strike.Launch();
            return;
        }
        else if (Shockwave.KnownSpell && Shockwave.IsSpellUsable && ObjectManager.Target.GetDistance < 10
                 && MySettings.UseShockwave)
        {
            Shockwave.Launch();
            return;
        }
        else if (Dragon_Roar.KnownSpell && Dragon_Roar.IsSpellUsable && ObjectManager.Target.GetDistance < 8
                 && MySettings.UseDragonRoar)
        {
            Dragon_Roar.Launch();
            return;
        }
        else
        {
            if (Bladestorm.KnownSpell && Bladestorm.IsSpellUsable && ObjectManager.Target.GetDistance < 8
                && MySettings.UseBladestorm)
            {
                Bladestorm.Launch();
                return;
            }
        }

        if (Thunder_Clap.KnownSpell && Thunder_Clap.IsSpellUsable && Thunder_Clap.IsDistanceGood
            && MySettings.UseThunderClap)
        {
            Thunder_Clap.Launch();
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
        else if (Berserker_Rage.KnownSpell && Berserker_Rage.IsSpellUsable && ObjectManager.Me.RagePercentage < 50
                 && MySettings.UseBerserkerRage && ObjectManager.Target.GetDistance < 30)
        {
            Berserker_Rage.Launch();
            return;
        }
        else if (Battle_Shout.KnownSpell && Battle_Shout.IsSpellUsable && ObjectManager.Me.RagePercentage < 80
                 && MySettings.UseBattleShout && ObjectManager.Target.GetDistance < 30)
        {
            Battle_Shout.Launch();
            return;
        }
        else if (Commanding_Shout.KnownSpell && Commanding_Shout.IsSpellUsable && ObjectManager.Me.RagePercentage < 80
                 && MySettings.UseCommandingShout && !MySettings.UseBattleShout && ObjectManager.Target.GetDistance < 30)
        {
            Commanding_Shout.Launch();
            return;
        }
        else if (Recklessness.KnownSpell && Recklessness.IsSpellUsable && MySettings.UseRecklessness
                 && ObjectManager.Target.GetDistance < 30)
        {
            Recklessness.Launch();
            return;
        }
        else if (Shattering_Throw.KnownSpell && Shattering_Throw.IsSpellUsable && Shattering_Throw.IsDistanceGood
                 && MySettings.UseShatteringThrow)
        {
            Shattering_Throw.Launch();
            return;
        }
        else if (Skull_Banner.KnownSpell && Skull_Banner.IsSpellUsable
                 && MySettings.UseSkullBanner && ObjectManager.Target.GetDistance < 30)
        {
            Skull_Banner.Launch();
            return;
        }
        else if (Avatar.KnownSpell && Avatar.IsSpellUsable
                 && MySettings.UseAvatar && ObjectManager.Target.GetDistance < 30)
        {
            Avatar.Launch();
            return;
        }
        else if (Bloodbath.KnownSpell && Bloodbath.IsSpellUsable
                 && MySettings.UseBloodbath && ObjectManager.Target.GetDistance < 30)
        {
            Bloodbath.Launch();
            return;
        }
        else
        {
            if (Storm_Bolt.KnownSpell && Storm_Bolt.IsSpellUsable
                && MySettings.UseStormBolt && Storm_Bolt.IsDistanceGood)
            {
                Storm_Bolt.Launch();
                return;
            }
        }
    }

    public void DPS_Cycle()
    {
        if (Taunt.IsSpellUsable && Taunt.IsDistanceGood && Taunt.KnownSpell && !ObjectManager.Target.InCombat
            && MySettings.UseTaunt)
        {
            Taunt.Launch();
            return;
        }

        if (Charge.KnownSpell && Charge.IsSpellUsable && Charge.IsDistanceGood && ObjectManager.Target.GetDistance > 8
            && MySettings.UseCharge)
        {
            Charge.Launch();
            return;
        }

        if (Victory_Rush.KnownSpell && Victory_Rush.IsSpellUsable && Victory_Rush.IsDistanceGood
            && MySettings.UseVictoryRush && ObjectManager.Me.HealthPercent < 90)
        {
            Victory_Rush.Launch();
            return;
        }
        else if (Whirlwind.KnownSpell && Whirlwind.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() > 3
                 && MySettings.UseWhirlwind)
        {
            Whirlwind.Launch();
            return;
        }
        else if (Cleave.KnownSpell && Cleave.IsSpellUsable && Cleave.IsDistanceGood && MySettings.UseCleave
                 && ObjectManager.GetNumberAttackPlayer() > 1 && ObjectManager.GetNumberAttackPlayer() < 4)
        {
            if (Deadly_Calm.KnownSpell && Deadly_Calm.IsSpellUsable && MySettings.UseDeadlyCalm)
            {
                Deadly_Calm.Launch();
                Thread.Sleep(200);
            }

            Cleave.Launch();
            return;
        }
        else if (Heroic_Strike.KnownSpell && Heroic_Strike.IsSpellUsable && Heroic_Strike.IsDistanceGood
                 && MySettings.UseHeroicStrike && ObjectManager.GetNumberAttackPlayer() < 3
                 && ObjectManager.Me.RagePercentage > 80)
        {
            if (Deadly_Calm.KnownSpell && Deadly_Calm.IsSpellUsable && MySettings.UseDeadlyCalm)
            {
                Deadly_Calm.Launch();
                Thread.Sleep(200);
            }

            Heroic_Strike.Launch();
            return;
        }
        else if (Shockwave.KnownSpell && Shockwave.IsSpellUsable && ObjectManager.Target.GetDistance < 10
                 && MySettings.UseShockwave)
        {
            Shockwave.Launch();
            return;
        }
        else if (Dragon_Roar.KnownSpell && Dragon_Roar.IsSpellUsable && ObjectManager.Target.GetDistance < 8
                 && MySettings.UseDragonRoar)
        {
            Dragon_Roar.Launch();
            return;
        }
        else if (Bladestorm.KnownSpell && Bladestorm.IsSpellUsable && ObjectManager.Target.GetDistance < 8
                 && MySettings.UseBladestorm)
        {
            Bladestorm.Launch();
            return;
        }
        else if (Bloodthirst.KnownSpell && Bloodthirst.IsSpellUsable && Bloodthirst.IsDistanceGood
                 && MySettings.UseBloodthirst)
        {
            Bloodthirst.Launch();
            return;
        }
        else if (Colossus_Smash.KnownSpell && Colossus_Smash.IsSpellUsable && Colossus_Smash.IsDistanceGood
                 && MySettings.UseColossusSmash)
        {
            Colossus_Smash.Launch();
            return;
        }
        else if (Execute.KnownSpell && Execute.IsSpellUsable && Execute.IsDistanceGood
                 && MySettings.UseExecute && ObjectManager.GetNumberAttackPlayer() < 4)
        {
            Execute.Launch();
            return;
        }
        else if (Raging_Blow.KnownSpell && Raging_Blow.IsSpellUsable && Raging_Blow.IsDistanceGood
                 && MySettings.UseRagingBlow)
        {
            Raging_Blow.Launch();
            return;
        }
        else if (Wild_Strike.KnownSpell && Wild_Strike.IsSpellUsable && Wild_Strike.IsDistanceGood
                 && MySettings.UseWildStrike && ObjectManager.Me.HaveBuff(46915))
        {
            Wild_Strike.Launch();
            return;
        }
        else
        {
            if (Heroic_Throw.KnownSpell && Heroic_Throw.IsSpellUsable && Heroic_Throw.IsDistanceGood
                && MySettings.UseHeroicThrow)
            {
                Heroic_Throw.Launch();
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

        if (ObjectManager.Me.HealthPercent < 30 && MySettings.UseDefensiveStance
            && Defensive_Stance.KnownSpell && Defensive_Stance.IsSpellUsable && !Defensive_Stance.HaveBuff)
        {
            Defensive_Stance.Launch();
            return;
        }
        else if (!Battle_Stance.HaveBuff && Battle_Stance.KnownSpell && Battle_Stance.IsSpellUsable
                 && MySettings.UseBattleStance && ObjectManager.Me.HealthPercent > 50)
        {
            Battle_Stance.Launch();
            return;
        }
        else if (!Berserker_Stance.HaveBuff && Berserker_Stance.KnownSpell && Berserker_Stance.IsSpellUsable
                 && MySettings.UseBerserkerStance && !MySettings.UseBattleStance && ObjectManager.Me.HealthPercent > 50)
        {
            Berserker_Stance.Launch();
            return;
        }
        else if (Battle_Shout.KnownSpell && Battle_Shout.IsSpellUsable && !Battle_Shout.HaveBuff
                 && MySettings.UseBattleShout)
        {
            Battle_Shout.Launch();
            return;
        }
        else
        {
            if (Commanding_Shout.KnownSpell && Commanding_Shout.IsSpellUsable && !Commanding_Shout.HaveBuff
                && MySettings.UseCommandingShout && !MySettings.UseBattleShout)
            {
                Commanding_Shout.Launch();
                return;
            }
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (Victory_Rush.KnownSpell && Victory_Rush.IsSpellUsable && Victory_Rush.IsDistanceGood
            && MySettings.UseVictoryRush && ObjectManager.Me.HealthPercent < 90)
        {
            Victory_Rush.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 30 && Rallying_Cry.IsSpellUsable && Rallying_Cry.KnownSpell
                 && MySettings.UseRallyingCry && Fight.InFight)
        {
            Rallying_Cry.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && Gift_of_the_Naaru.IsSpellUsable && Gift_of_the_Naaru.KnownSpell
                 && MySettings.UseGiftoftheNaaru)
        {
            Gift_of_the_Naaru.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Me.HealthPercent < 80 && Enraged_Regeneration.IsSpellUsable &&
                Enraged_Regeneration.KnownSpell
                && MySettings.UseEnragedRegeneration)
            {
                Enraged_Regeneration.Launch();
                return;
            }
        }
    }

    private void Defense_Cycle()
    {
        if (ObjectManager.Me.HealthPercent < 95 && MySettings.UseDisarm && Disarm.IsDistanceGood
            && Disarm.KnownSpell && Disarm.IsSpellUsable && Disarm_Timer.IsReady)
        {
            Disarm.Launch();
            Disarm_Timer = new Timer(1000*60);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 20 && MySettings.UseIntimidatingShout
                 && Intimidating_Shout.KnownSpell && Intimidating_Shout.IsSpellUsable &&
                 ObjectManager.Target.GetDistance < 8)
        {
            Intimidating_Shout.Launch();
            OnCD = new Timer(1000*8);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && MySettings.UseDiebytheSword
                 && Die_by_the_Sword.KnownSpell && Die_by_the_Sword.IsSpellUsable)
        {
            Die_by_the_Sword.Launch();
            OnCD = new Timer(1000*8);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && MySettings.UseDemoralizingBanner
                 && Demoralizing_Banner.KnownSpell && Demoralizing_Banner.IsSpellUsable &&
                 ObjectManager.Target.GetDistance < 30)
        {
            SpellManager.CastSpellByIDAndPosition(114203, ObjectManager.Target.Position);
            OnCD = new Timer(1000*15);
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
        else if (!Hamstring.TargetHaveBuff && MySettings.UseHamstring && Hamstring.KnownSpell
                 && Hamstring.IsSpellUsable && Hamstring.IsDistanceGood)
        {
            Hamstring.Launch();
            return;
        }
        else if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && Pummel.IsDistanceGood
                 && Pummel.KnownSpell && Pummel.IsSpellUsable && MySettings.UsePummel)
        {
            Pummel.Launch();
            return;
        }
        else if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe &&
                 ObjectManager.Target.GetDistance < 10
                 && Disrupting_Shout.KnownSpell && Disrupting_Shout.IsSpellUsable && MySettings.UseDisruptingShout)
        {
            Disrupting_Shout.Launch();
            return;
        }
        else if (ObjectManager.Target.GetMove && !Piercing_Howl.TargetHaveBuff && MySettings.UsePiercingHowl
                 && Piercing_Howl.KnownSpell && Piercing_Howl.IsSpellUsable && ObjectManager.Target.GetDistance < 15)
        {
            Piercing_Howl.Launch();
            return;
        }
        else if (Hamstring.TargetHaveBuff && MySettings.UseStaggeringShout && Staggering_Shout.KnownSpell
                 && Staggering_Shout.IsSpellUsable && ObjectManager.Target.GetDistance < 20)
        {
            Staggering_Shout.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe &&
                MySettings.UseMassSpellReflection
                && Mass_Spell_Reflection.KnownSpell && Mass_Spell_Reflection.IsSpellUsable)
            {
                Mass_Spell_Reflection.Launch();
                return;
            }
        }
    }

    private void AvoidMelee()
    {
        while (ObjectManager.Target.GetDistance < 3 && ObjectManager.Target.InCombat)
        {
            nManager.Wow.Helpers.Keybindings.PressKeybindings(Keybindings.MOVEBACKWARD);
        }
    }
}

#endregion