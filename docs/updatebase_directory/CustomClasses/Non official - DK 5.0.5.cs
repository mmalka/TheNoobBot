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
                #region DeathKnight Specialisation checking

                case WoWClass.DeathKnight:
                    var Blood_Rites = new Spell("Blood Rites");
                    var Reaping = new Spell("Reaping");
                    var Howling_Blast = new Spell("Howling Blast");

                    if (Blood_Rites.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Deathknight_Blood.xml";
                            Deathknight_Blood.DeathknightBloodSettings CurrentSetting;
                            CurrentSetting = new Deathknight_Blood.DeathknightBloodSettings();
                            if (System.IO.File.Exists(CurrentSettingsFile))
                            {
                                CurrentSetting =
                                    Settings.Load<Deathknight_Blood.DeathknightBloodSettings>(CurrentSettingsFile);
                            }
                            CurrentSetting.ToForm();
                            CurrentSetting.Save(CurrentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Deathknight Blood class...");
                            new Deathknight_Blood();
                        }
                    }
                    else if (Reaping.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Deathknight_Unholy.xml";
                            Deathknight_Unholy.DeathknightUnholySettings CurrentSetting;
                            CurrentSetting = new Deathknight_Unholy.DeathknightUnholySettings();
                            if (System.IO.File.Exists(CurrentSettingsFile))
                            {
                                CurrentSetting =
                                    Settings.Load<Deathknight_Unholy.DeathknightUnholySettings>(CurrentSettingsFile);
                            }
                            CurrentSetting.ToForm();
                            CurrentSetting.Save(CurrentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Deathknight Unholy class...");
                            new Deathknight_Unholy();
                        }
                    }
                    else if (Howling_Blast.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Deathknight_Frost.xml";
                            Deathknight_Frost.DeathknightFrostSettings CurrentSetting;
                            CurrentSetting = new Deathknight_Frost.DeathknightFrostSettings();
                            if (System.IO.File.Exists(CurrentSettingsFile))
                            {
                                CurrentSetting =
                                    Settings.Load<Deathknight_Frost.DeathknightFrostSettings>(CurrentSettingsFile);
                            }
                            CurrentSetting.ToForm();
                            CurrentSetting.Save(CurrentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Deathknight Frost class...");
                            new Deathknight_Frost();
                        }
                    }
                    else
                    {
                        if (ConfigOnly)
                        {
                            System.Windows.Forms.MessageBox.Show(
                                "Your specification haven't be found, loading Deathknight Apprentice Settings");
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Deathknight_Apprentice.xml";
                            Deathknight_Apprentice.DeathknightApprenticeSettings CurrentSetting;
                            CurrentSetting = new Deathknight_Apprentice.DeathknightApprenticeSettings();
                            if (System.IO.File.Exists(CurrentSettingsFile))
                            {
                                CurrentSetting =
                                    Settings.Load<Deathknight_Apprentice.DeathknightApprenticeSettings>(CurrentSettingsFile);
                            }
                            CurrentSetting.ToForm();
                            CurrentSetting.Save(CurrentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("No specialisation detected.");
                            Logging.WriteFight("Loading Deathknight Apprentice class...");
                            new Deathknight_Apprentice();
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

#region Deathknight

public class Deathknight_Apprentice
{
    [Serializable]
    public class DeathknightApprenticeSettings : nManager.Helpful.Settings
    {
        /* Professions & Racials */
        public bool UseArcaneTorrent = true;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseLifeblood = true;
        public bool UseStoneform = true;
        public bool UseGiftoftheNaaru = true;
        public bool UseWarStomp = true;
        /* Deathknight Presence & Buffs */
        public bool UseFrostPresence = true;
        public bool UseBloodPresence = true;
        /* Offensive Spell */
        public bool UseBloodBoil = true;
        public bool UseBloodStrike = true;
        public bool UseDeathCoil = true;
        public bool UseIcyTouch = true;
        public bool UsePlagueStrike = true;
        /* Offensive Cooldown */
        public bool UseDeathGrip = true;
        public bool UsePestilence = true;
        public bool UseRaiseDead = true;
        /* Defensive Cooldown */
        public bool UseChainsofIce = true;
        public bool UseMindFreeze = true;
        /* Healing Spell */
        public bool UseDeathStrike = true;

        public DeathknightApprenticeSettings()
        {
            ConfigWinForm(new System.Drawing.Point(400, 400), "Deathknight Apprentice Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");
            AddControlInWinForm("Use Lifeblood", "UseLifeblood", "Professions & Racials");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            /* Deathknight Presence & Buffs */
            AddControlInWinForm("Use Frost Presence", "UseFrostPresence", "Deathknight Presence & Buffs");
            AddControlInWinForm("Use Blood Presence", "UseFrostPresence", "Deathknight Presence & Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Blood Boil", "UseBloodBoil", "Offensive Spell");
            AddControlInWinForm("Use Blood Strike", "UseBloodStrike", "Offensive Spell");
            AddControlInWinForm("Use Death Coil", "UseDeathCoil", "Offensive Spell");
            AddControlInWinForm("Use Icy Touch", "UseIcyTouch", "Offensive Spell");
            AddControlInWinForm("Use Plague Strike", "UsePlagueStrike", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use Death Grip", "UseDeathGrip", "Offensive Cooldown");
            AddControlInWinForm("Use Pestilence", "UsePestilence", "Offensive Cooldown");
            AddControlInWinForm("Use Raise Dead", "UseRaiseDead", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Chains of Ice", "UseChainsofIce", "Defensive Cooldown");
            AddControlInWinForm("Use Mind Freeze", "UseMindFreeze", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Death Strike", "UseDeathStrike", "Healing Spell");
        }

        public static DeathknightApprenticeSettings CurrentSetting { get; set; }

        public static DeathknightApprenticeSettings GetSettings()
        {
            string CurrentSettingsFile = Application.StartupPath + "\\CustomClasses\\Settings\\Deathknight_Apprentice.xml";
            if (System.IO.File.Exists(CurrentSettingsFile))
            {
                return
                    CurrentSetting = Settings.Load<Deathknight_Apprentice.DeathknightApprenticeSettings>(CurrentSettingsFile);
            }
            else
            {
                return new Deathknight_Apprentice.DeathknightApprenticeSettings();
            }
        }
    }

    private readonly DeathknightApprenticeSettings MySettings = DeathknightApprenticeSettings.GetSettings();

    #region Professions & Racials

    private readonly Spell Arcane_Torrent = new Spell("Arcane Torrent");
    private readonly Spell Berserking = new Spell("Berserking");
    private readonly Spell Blood_Fury = new Spell("Blood Fury");
    private readonly Spell Lifeblood = new Spell("Lifeblood");
    private readonly Spell Stoneform = new Spell("Stoneform");
    private readonly Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    private readonly Spell War_Stomp = new Spell("War Stomp");

    #endregion

    #region Deathknight Presence & Buffs

    private readonly Spell Blood_Plague = new Spell("Blood Plague");
    private Timer Blood_Plague_Timer = new Timer(0);
    private readonly Spell Blood_Presence = new Spell("Blood Presence");
    private readonly Spell Frost_Fever = new Spell("Frost Fever");
    private Timer Frost_Fever_Timer = new Timer(0);
    private readonly Spell Frost_Presence = new Spell("Frost Presence");

    #endregion

    #region Offensive Spell

    private readonly Spell Blood_Boil = new Spell("Blood Boil");
    private readonly Spell Blood_Strike = new Spell("Blood Strike");
    private readonly Spell Death_Coil = new Spell("Death Coil");
    private readonly Spell Icy_Touch = new Spell("Icy Touch");
    private readonly Spell Plague_Strike = new Spell("Plague Strike");

    #endregion

    #region Offensive Cooldown

    private readonly Spell Death_Grip = new Spell("Death Grip");
    private readonly Spell Pestilence = new Spell("Pestilence");
    private Timer Pestilence_Timer = new Timer(0);
    private readonly Spell Raise_Dead = new Spell("Raise Dead");

    #endregion

    #region Defensive Cooldown

    private readonly Spell Chains_of_Ice = new Spell("Chains of Ice");
    private readonly Spell Mind_Freeze = new Spell("Mind Freeze");

    #endregion

    #region Healing Spell

    private readonly Spell Death_Strike = new Spell("Death Strike");

    #endregion

    public Deathknight_Apprentice()
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
                            (Death_Grip.IsDistanceGood || Icy_Touch.IsDistanceGood))
                        {
                            Pull();
                            lastTarget = ObjectManager.Me.Target;
                        }

                        Combat();
                    }

                    else
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
        if (Death_Grip.IsSpellUsable && MySettings.UseDeathGrip && Death_Grip.IsDistanceGood)
            Grip();
        else
        {
            if (Icy_Touch.IsSpellUsable && MySettings.UseIcyTouch && Icy_Touch.IsDistanceGood)
                Icy_Touch.Launch();
        }
    }

    public void Grip()
    {
        if (ObjectManager.Target.GetDistance > 10 && Death_Grip.IsSpellUsable && MySettings.UseDeathGrip
            && Death_Grip.IsDistanceGood)
        {
            Death_Grip.Launch();
            MovementManager.StopMove();
        }
    }

    public void Combat()
    {
        Presence();
        Defense_Cycle();
        Heal();
        Decast();
        DPS_Burst();
        DPS_Cycle();
    }

    private void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            Presence();
            Heal();
        }
    }

    private void Presence()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (Blood_Presence.KnownSpell && ObjectManager.GetNumberAttackPlayer() >= 3 && MySettings.UseBloodPresence)
        {
            if (!Blood_Presence.HaveBuff && Blood_Presence.IsSpellUsable)
                Blood_Presence.Launch();
        }
        else
        {
            if (Frost_Presence.KnownSpell && MySettings.UseFrostPresence)
            {
                if (!Frost_Presence.HaveBuff && Frost_Presence.IsSpellUsable)
                    Frost_Presence.Launch();
            }
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.HealthPercent < 80 && Gift_of_the_Naaru.IsSpellUsable && Gift_of_the_Naaru.KnownSpell
            && MySettings.UseGiftoftheNaaru)
        {
            Gift_of_the_Naaru.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Me.HealthPercent > 0 && ObjectManager.Me.HealthPercent < 75)
            {
                if (Death_Strike.KnownSpell && Death_Strike.IsSpellUsable && MySettings.UseDeathStrike)
                {
                    Death_Strike.Launch();
                    return;
                }
            }
        }
    }

    private void DPS_Burst()
    {
        if (Raise_Dead.KnownSpell && MySettings.UseRaiseDead)
        {
            if (Raise_Dead.IsSpellUsable && ObjectManager.Target.GetDistance < 30)
                Raise_Dead.Launch();
        }
        else if (Lifeblood.KnownSpell && MySettings.UseLifeblood)
        {
            if (Lifeblood.IsSpellUsable && ObjectManager.Target.GetDistance < 10)
                Lifeblood.Launch();
        }
        else if (Berserking.KnownSpell && MySettings.UseBerserking)
        {
            if (Berserking.IsSpellUsable && ObjectManager.Target.GetDistance < 10)
                Berserking.Launch();
        }
        else
        {
            if (Blood_Fury.KnownSpell && MySettings.UseBloodFury)
            {
                if (Blood_Fury.IsSpellUsable && ObjectManager.Target.GetDistance < 10)
                    Blood_Fury.Launch();
            }
        }
    }

    private void Defense_Cycle()
    {
        if (Stoneform.KnownSpell && MySettings.UseStoneform)
        {
            if (ObjectManager.Me.HealthPercent < 80 && Stoneform.IsSpellUsable)
            {
                Stoneform.Launch();
                return;
            }
        }
        else
        {
            if (War_Stomp.KnownSpell && MySettings.UseWarStomp)
            {
                if (ObjectManager.Me.HealthPercent < 65 && ObjectManager.Target.GetDistance < 8
                    && War_Stomp.IsSpellUsable)
                {
                    War_Stomp.Launch();
                    return;
                }
            }
        }
    }

    private void DPS_Cycle()
    {
        Grip();

        if ((!Frost_Fever.TargetHaveBuff || Frost_Fever_Timer.IsReady) && MySettings.UseIcyTouch
            && Icy_Touch.IsSpellUsable && Icy_Touch.IsDistanceGood)
        {
            Icy_Touch.Launch();
            return;
        }
        else if ((!Blood_Plague.TargetHaveBuff || Blood_Plague_Timer.IsReady) && MySettings.UsePlagueStrike
            && Plague_Strike.IsSpellUsable && Plague_Strike.IsDistanceGood)
        {
            Plague_Strike.Launch();
            return;
        }
        else if (Blood_Plague.TargetHaveBuff && Frost_Fever.TargetHaveBuff && Pestilence.KnownSpell && MySettings.UsePestilence
            && Pestilence.IsSpellUsable && Pestilence.IsDistanceGood && ObjectManager.GetNumberAttackPlayer() > 1 &&
            Pestilence_Timer.IsReady)
        {
            Pestilence.Launch();
            Pestilence_Timer = new Timer(1000*30);
            return;
        }
        else if (Blood_Plague.TargetHaveBuff && Frost_Fever.TargetHaveBuff && Blood_Boil.KnownSpell && MySettings.UseBloodBoil
            && Blood_Boil.IsSpellUsable && ObjectManager.Target.GetDistance < 10 && ObjectManager.GetNumberAttackPlayer() > 2
            && !Pestilence_Timer.IsReady)
        {
            Blood_Boil.Launch();
            return;
        }
        else if (Death_Coil.IsDistanceGood && Death_Coil.IsSpellUsable && MySettings.UseDeathCoil)
        {
            Death_Coil.Launch();
            return;
        }
        else if (Blood_Strike.IsSpellUsable && Blood_Strike.IsDistanceGood && MySettings.UseBloodStrike)
        {
            Blood_Strike.Launch();
            return;
        }
        else if (Icy_Touch.IsSpellUsable && Icy_Touch.IsDistanceGood && MySettings.UseIcyTouch)
        {
            Icy_Touch.Launch();
            return;
        }
        else
        {
            if (Plague_Strike.IsSpellUsable && Plague_Strike.IsDistanceGood && MySettings.UsePlagueStrike)
            {
                Plague_Strike.Launch();
                return;
            }
        }
    }

    private void Decast()
    {
        if (Arcane_Torrent.KnownSpell && MySettings.UseArcaneTorrent)
        {
            if (Arcane_Torrent.IsSpellUsable && ObjectManager.Target.IsCast && ObjectManager.Target.GetDistance < 8)
            {
                Arcane_Torrent.Launch();
                return;
            }
        }
        else
        {
            if (Mind_Freeze.KnownSpell && MySettings.UseMindFreeze)
            {
                if (ObjectManager.Target.IsCast && Mind_Freeze.IsSpellUsable && Mind_Freeze.IsDistanceGood)
                {
                    Mind_Freeze.Launch();
                    return;
                }
            }
        }

        if (ObjectManager.Target.GetMove && !Chains_of_Ice.TargetHaveBuff && MySettings.UseChainsofIce
            && Chains_of_Ice.KnownSpell && Chains_of_Ice.IsSpellUsable && Chains_of_Ice.IsDistanceGood)
        {
            Chains_of_Ice.Launch();
            return;
        }
    }
}

public class Deathknight_Blood
{
    [Serializable]
    public class DeathknightBloodSettings : nManager.Helpful.Settings
    {
        /* Professions & Racials */
        public bool UseArcaneTorrent = true;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseLifeblood = true;
        public bool UseStoneform = true;
        public bool UseGiftoftheNaaru = true;
        public bool UseWarStomp = true;
        /* Deathknight Presence & Buffs */
        public bool UseFrostPresence = true;
        public bool UseBloodPresence = true;
        public bool UseHornofWinter = true;
        public bool UsePathofFrost = true;
        public bool UseUnholyPresence = true;
        /* Offensive Spell */
        public bool UseBloodBoil = true;
        public bool UseDeathCoil = true;
        public bool UseDeathandDecay = true;
        public bool UseDeathStrike = true;
        public bool UseHeartStrike = true;
        public bool UseIcyTouch = true;
        public bool UsePlagueStrike = true;
        public bool UseRuneStrike = true;
        public bool UseSoulReaper = true;
        public bool UseUnholyBlight = true;
        /* Offensive Cooldown */
        public bool UseBloodTap = true;
        public bool UseDancingRuneWeapon = true;
        public bool UseDeathGrip = true;
        public bool UseEmpowerRuneWeapon = true;
        public bool UseOutbreak = true;
        public bool UsePestilence = true;
        public bool UseRaiseDead = true;
        /* Defensive Cooldown */
        public bool UseAntiMagicShell = true;
        public bool UseAntiMagicZone = true;
        public bool UseArmyoftheDead = true;
        public bool UseAsphyxiate = true;
        public bool UseBoneShield = true;
        public bool UseChainsofIce = true;
        public bool UseDeathsAdvance = true;
        public bool UseIceboundFortitude = true;
        public bool UseMindFreeze = true;
        public bool UseRemorselessWinter = true;
        public bool UseStrangulate = true;
        public bool UseVampiricBlood = true;
        /* Healing Spell */
        public bool UseConversion = true;
        public bool UseDeathPact = true;
        public bool UseDeathSiphon = true;
        public bool UseLichborne = true;
        public bool UseRuneTap = true;
        /* Game Settings */
        public bool UseLowCombat = true;
        public bool UseTrinket = true;
        public bool UseEngGlove = true;
        public bool UseAlchFlask = true;

        public DeathknightBloodSettings()
        {
            ConfigWinForm(new System.Drawing.Point(400, 400), "Deathknight Blood Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");
            AddControlInWinForm("Use Lifeblood", "UseLifeblood", "Professions & Racials");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            /* Deathknight Presence & Buffs */
            AddControlInWinForm("Use Frost Presence", "UseFrostPresence", "Deathknight Presence & Buffs");
            AddControlInWinForm("Use Blood Presence", "UseBloodPresence", "Deathknight Presence & Buffs");
            AddControlInWinForm("Use Horn of Winter", "UseHornofWinter", "Deathknight Presence & Buffs");
            AddControlInWinForm("Use Path of Frost", "UsePathofFrost", "Deathknight Presence & Buffs");
            AddControlInWinForm("Use Unholy Presence", "UseUnholyPresence", "Deathknight Presence & Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Blood Boil", "UseBloodBoil", "Offensive Spell");
            AddControlInWinForm("Use Death Coil", "UseDeathCoil", "Offensive Spell");
            AddControlInWinForm("Use Death and Decay", "UseDeathandDecay", "Offensive Spell");
            AddControlInWinForm("Use Death Strike", "UseDeathStrike", "Offensive Spell");
            AddControlInWinForm("Use Heart Strike", "UseHeartStrike", "Offensive Spell");
            AddControlInWinForm("Use Icy Touch", "UseIcyTouch", "Offensive Spell");
            AddControlInWinForm("Use Plague Strike", "UsePlagueStrike", "Offensive Spell");
            AddControlInWinForm("Use Rune Strike", "UseRuneStrike", "Offensive Spell");
            AddControlInWinForm("Use Soul Reaper", "UseSoulReaper", "Offensive Spell");
            AddControlInWinForm("Use Unholy Blight", "UseUnholyBlight", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use Blood Tap", "UseBloodTap", "Offensive Cooldown");
            AddControlInWinForm("Use Dancing Rune Weapon", "UseDancingRuneWeapon", "Offensive Cooldown");
            AddControlInWinForm("Use Death Grip", "UseDeathGrip", "Offensive Cooldown");
            AddControlInWinForm("Use Empower Rune Weapon", "UseEmpowerRuneWeapon", "Offensive Cooldown");
            AddControlInWinForm("Use Outbreak", "UseOutbreak", "Offensive Cooldown");
            AddControlInWinForm("Use Pestilence", "UsePestilence", "Offensive Cooldown");
            AddControlInWinForm("Use Raise Dead", "UseRaiseDead", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Anti-Magic Shell", "UseAntiMagicShell", "Defensive Cooldown");
            AddControlInWinForm("Use Anti-Magic Zone", "UseAntiMagicZone", "Defensive Cooldown");
            AddControlInWinForm("Use Army of the Dead", "UseArmyoftheDead", "Defensive Cooldown");
            AddControlInWinForm("Use Asphyxiate", "UseAsphyxiate", "Defensive Cooldown");
            AddControlInWinForm("Use Bone Shield", "UseBoneShield", "Defensive Cooldown");
            AddControlInWinForm("Use Chains of Ice", "UseChainsofIce", "Defensive Cooldown");
            AddControlInWinForm("Use Death's Advance", "UseDeathsAdvance", "Defensive Cooldown");
            AddControlInWinForm("Use Icebound Fortitude", "UseIceboundFortitude", "Defensive Cooldown");
            AddControlInWinForm("Use Mind Freeze", "UseMindFreeze", "Defensive Cooldown");
            AddControlInWinForm("Use Remorseless Winter", "UseRemorseless Winter", "Defensive Cooldown");
            AddControlInWinForm("Use Strangulate", "UseStrangulate", "Defensive Cooldown");
            AddControlInWinForm("Use Vampiric Blood", "UseVampiricBlood", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Conversion", "UseConversion", "Healing Spell");
            AddControlInWinForm("Use Death Pact", "UseDeathPact", "Healing Spell");
            AddControlInWinForm("Use Death Siphon", "UseDeathSiphon", "Healing Spell");
            AddControlInWinForm("Use Lichborne", "UseLichborne", "Healing Spell");
            AddControlInWinForm("Use Rune Tap", "UseRuneTap", "Healing Spell");
            /* Game Settings */
            AddControlInWinForm("Use Low Combat Settings", "UseLowCombat", "Game Settings");
            AddControlInWinForm("Use Trinket", "UseTrinket", "Game Settings");
            AddControlInWinForm("Use Engineer Gloves", "UseEngGlove", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
        }

        public static DeathknightBloodSettings CurrentSetting { get; set; }

        public static DeathknightBloodSettings GetSettings()
        {
            string CurrentSettingsFile = Application.StartupPath + "\\CustomClasses\\Settings\\Deathknight_Blood.xml";
            if (System.IO.File.Exists(CurrentSettingsFile))
            {
                return
                    CurrentSetting = Settings.Load<Deathknight_Blood.DeathknightBloodSettings>(CurrentSettingsFile);
            }
            else
            {
                return new Deathknight_Blood.DeathknightBloodSettings();
            }
        }
    }

    private readonly DeathknightBloodSettings MySettings = DeathknightBloodSettings.GetSettings();

    #region Professions & Racials

    private readonly Spell Arcane_Torrent = new Spell("Arcane Torrent");
    private readonly Spell Berserking = new Spell("Berserking");
    private readonly Spell Blood_Fury = new Spell("Blood Fury");
    private readonly Spell Engineering = new Spell("Engineering");
    private readonly Spell Lifeblood = new Spell("Lifeblood");
    private readonly Spell Stoneform = new Spell("Stoneform");
    private readonly Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    private readonly Spell War_Stomp = new Spell("War Stomp");

    #endregion

    #region Deathknight Presence & Buffs

    private readonly Spell Blood_Plague = new Spell("Blood Plague");
    private Timer Blood_Plague_Timer = new Timer(0);
    private readonly Spell Blood_Presence = new Spell("Blood Presence");
    private readonly Spell Frost_Fever = new Spell("Frost Fever");
    private Timer Frost_Fever_Timer = new Timer(0);
    private readonly Spell Frost_Presence = new Spell("Frost Presence");
    private readonly Spell Horn_of_Winter = new Spell("Horn of Winter");
    private readonly Spell Path_of_Frost = new Spell("Path of Frost");
    private Timer Path_of_Frost_Timer = new Timer(0);
    private readonly Spell Roiling_Blood = new Spell("Roiling Blood");
    private readonly Spell Unholy_Presence = new Spell("Unholy Presence");

    #endregion

    #region Offensive Spell

    private readonly Spell Blood_Boil = new Spell("Blood Boil");
    private readonly Spell Blood_Strike = new Spell("Blood Strike");
    private readonly Spell Death_and_Decay = new Spell("Death and Decay");
    private readonly Spell Death_Coil = new Spell("Death Coil");
    private readonly Spell Death_Strike = new Spell("Death Strike");
    private readonly Spell Heart_Strike = new Spell("Heart Strike");
    private readonly Spell Icy_Touch = new Spell("Icy Touch");
    private readonly Spell Plague_Strike = new Spell("Plague Strike");
    private readonly Spell Rune_Strike = new Spell("Rune Strike");
    private readonly Spell Soul_Reaper = new Spell("Soul Reaper");
    private readonly Spell Unholy_Blight = new Spell("Unholy Blight");

    #endregion

    #region Offensive Cooldown

    private readonly Spell Blood_Tap = new Spell("Blood Tap");
    private readonly Spell Dancing_Rune_Weapon = new Spell("Dancing Rune Weapon");
    private Timer Dancing_Rune_Weapon_Timer = new Timer(0);
    private readonly Spell Death_Grip = new Spell("Death Grip");
    private readonly Spell Empower_Rune_Weapon = new Spell("Empower Rune Weapon");
    private readonly Spell Outbreak = new Spell("Outbreak");
    private readonly Spell Pestilence = new Spell("Pestilence");
    private Timer Pestilence_Timer = new Timer(0);
    private readonly Spell Raise_Dead = new Spell("Raise Dead");

    #endregion

    #region Defensive Cooldown

    private readonly Spell AntiMagic_Shell = new Spell("Anti-Magic Shell");
    private readonly Spell AntiMagic_Zone = new Spell("Anti-Magic Zone");
    private readonly Spell Army_of_the_Dead = new Spell("Army of the Dead");
    private readonly Spell Asphyxiate = new Spell("Asphyxiate");
    private readonly Spell Bone_Shield = new Spell("Bone Shield");
    private readonly Spell Chains_of_Ice = new Spell("Chains of Ice");
    private readonly Spell Deaths_Advance = new Spell("Death's Advance");
    private readonly Spell Icebound_Fortitude = new Spell("Icebound Fortitude");
    private readonly Spell Mind_Freeze = new Spell("Mind Freeze");
    private readonly Spell Remorseless_Winter = new Spell("Remorseless Winter");
    private readonly Spell Strangulate = new Spell("Strangulate");
    private readonly Spell Vampiric_Blood = new Spell("Vampiric Blood");

    #endregion

    #region Healing Spell

    private readonly Spell Conversion = new Spell("Conversion");
    private readonly Spell Death_Pact = new Spell("Death Pact");
    private readonly Spell Death_Siphon = new Spell("Death Siphon");
    private readonly Spell Lichborne = new Spell("Lichborne");
    private readonly Spell Rune_Tap = new Spell("Rune Tap");

    #endregion

    private Timer Trinket_Timer = new Timer(0);
    private Timer Engineering_Timer = new Timer(0);
    private Timer AlchFlask_Timer = new Timer(0);
    private Timer OnCD = new Timer(0);
    public int DRW = 1;
    public int LC = 0;

    public Deathknight_Blood()
    {
        Main.range = 5.0f;
        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            try
            {
                Buff_Path();
                if (!ObjectManager.Me.IsMounted)
                {
                    if (Fight.InFight && ObjectManager.Me.Target > 0)
                    {
                        if (ObjectManager.Me.Target != lastTarget &&
                            (Death_Grip.IsDistanceGood || Icy_Touch.IsDistanceGood))
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
            Thread.Sleep(150);
        }
    }

    public void Pull()
    {
        if (Death_Grip.IsSpellUsable && MySettings.UseDeathGrip && Death_Grip.IsDistanceGood)
        {
            Grip();
        }
        else
        {
            if (Icy_Touch.IsSpellUsable && MySettings.UseIcyTouch && Icy_Touch.IsDistanceGood)
                Icy_Touch.Launch();
        }
    }

    private void Buff_Path()
    {
        if (!Fight.InFight && Path_of_Frost.KnownSpell && Path_of_Frost.IsSpellUsable && MySettings.UsePathofFrost
            && (!Path_of_Frost.HaveBuff || Path_of_Frost_Timer.IsReady))
        {
            Path_of_Frost.Launch();
            Path_of_Frost_Timer = new Timer(1000*60*9);
        }
    }

    public void LowCombat()
    {
        Presence();
        AvoidMelee();
        Defense_Cycle();
        Heal();
        Decast();
        Buff();
        Grip();

        if (Icy_Touch.KnownSpell && Icy_Touch.IsSpellUsable && Icy_Touch.IsDistanceGood && MySettings.UseIcyTouch)
        {
            Icy_Touch.Launch();
            if (ObjectManager.Target.HealthPercent < 50 && ObjectManager.Target.HealthPercent > 0)
            {
                Icy_Touch.Launch();
                return;
            }
            return;
        }
        else if (Death_Coil.KnownSpell && Death_Coil.IsSpellUsable && Death_Coil.IsDistanceGood && MySettings.UseDeathCoil)
        {
            Death_Coil.Launch();
            return;
        }
        else
        {
            if (Blood_Boil.IsSpellUsable && Blood_Boil.IsSpellUsable && Blood_Boil.IsDistanceGood && MySettings.UseBloodBoil)
            {
                Blood_Boil.Launch();
                return;
            }
        }
    }

    public void Combat()
    {
        Presence();
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
        else
        {
            if (MySettings.UseEngGlove && Engineering.KnownSpell && Engineering_Timer.IsReady
                && ObjectManager.Target.GetDistance < 30)
            {
                Logging.WriteFight("Use Engineering Gloves.");
                Lua.RunMacroText("/use 10");
                Engineering_Timer = new Timer(1000*60);
                return;
            }
        }

        if (Dancing_Rune_Weapon_Timer.IsReady && DRW == 0)
            DRW = 1;

        if (DRW == 1 && Dancing_Rune_Weapon.IsSpellUsable && Dancing_Rune_Weapon.KnownSpell
            && Dancing_Rune_Weapon.IsDistanceGood && MySettings.UseDancingRuneWeapon)
        {
            Dancing_Rune_Weapon.Launch();
            Dancing_Rune_Weapon_Timer = new Timer(1000*60*1.5);
            DRW = 0;
            return;
        }
    }

    public void DPS_Cycle()
    {
        Grip();

        if (ObjectManager.Me.HealthPercent < 85 && MySettings.UseDeathCoil
            && Lichborne.HaveBuff && Death_Coil.KnownSpell && Death_Coil.IsSpellUsable)
        {
            Lua.RunMacroText("/target Player");
            Death_Coil.Launch();
            return;
        }
        else if (Unholy_Blight.KnownSpell && Unholy_Blight.IsSpellUsable && ObjectManager.Target.GetDistance < 9
            && MySettings.UseUnholyBlight)
        {
            Unholy_Blight.Launch();
            Blood_Plague_Timer = new Timer(1000*27);
            Frost_Fever_Timer = new Timer(1000*27);
            return;
        }
        else if (Outbreak.KnownSpell && Outbreak.IsSpellUsable && Outbreak.IsDistanceGood && MySettings.UseOutbreak
            && (Blood_Plague_Timer.IsReady || Frost_Fever_Timer.IsReady || !Blood_Plague.TargetHaveBuff || !Frost_Fever.TargetHaveBuff))
        {
            Outbreak.Launch();
            Blood_Plague_Timer = new Timer(1000*27);
            Frost_Fever_Timer = new Timer(1000*27);
            return;
        }
        else if (Blood_Plague_Timer.IsReady && Frost_Fever_Timer.IsReady && Blood_Plague.TargetHaveBuff && Frost_Fever.TargetHaveBuff
            && Blood_Boil.IsSpellUsable && Blood_Boil.KnownSpell && ObjectManager.Target.GetDistance < 9
            && MySettings.UseBloodBoil && Roiling_Blood.KnownSpell)
        {
            Blood_Boil.Launch();
            Blood_Plague_Timer = new Timer(1000*27);
            Frost_Fever_Timer = new Timer(1000*27);
            return;
        }
        else if (Plague_Strike.KnownSpell && Plague_Strike.IsDistanceGood && !Outbreak.IsSpellUsable
            && !Unholy_Blight.IsSpellUsable && MySettings.UsePlagueStrike
            && (Blood_Plague_Timer.IsReady || !Blood_Plague.TargetHaveBuff))
        {
            if (!Plague_Strike.IsSpellUsable && Blood_Tap.KnownSpell && Blood_Tap.IsSpellUsable
                && MySettings.UseBloodTap)
            {
                Blood_Tap.Launch();
                Thread.Sleep(200);
            }

            if (Plague_Strike.IsSpellUsable)
            {
                Plague_Strike.Launch();
                Blood_Plague_Timer = new Timer(1000*27);
                return;
            }
        }
        else if (Icy_Touch.KnownSpell && Icy_Touch.IsDistanceGood && MySettings.UseIcyTouch
            && !Outbreak.IsSpellUsable && !Unholy_Blight.IsSpellUsable
            && (Frost_Fever_Timer.IsReady || !Frost_Fever.TargetHaveBuff))
        {
            if (!Icy_Touch.IsSpellUsable && Blood_Tap.KnownSpell && Blood_Tap.IsSpellUsable
                && MySettings.UseBloodTap)
            {
                Blood_Tap.Launch();
                Thread.Sleep(200);
            }

            if (Icy_Touch.IsSpellUsable)
            {
                Icy_Touch.Launch();
                Frost_Fever_Timer = new Timer(1000*27);
                return;
            }
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 3 && Blood_Boil.IsSpellUsable && Blood_Boil.IsDistanceGood && Blood_Boil.KnownSpell
            && MySettings.UseBloodBoil)
        {
            Blood_Boil.Launch();
            if (Blood_Plague.TargetHaveBuff && Frost_Fever.TargetHaveBuff && Roiling_Blood.KnownSpell)
            {
                Blood_Plague_Timer = new Timer(1000*27);
                Frost_Fever_Timer = new Timer(1000*27);
            }
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 2 && Pestilence.IsSpellUsable && Pestilence.IsDistanceGood
            && Pestilence.KnownSpell && MySettings.UsePestilence && !Roiling_Blood.KnownSpell)
        {
            Pestilence.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 2 && Death_and_Decay.KnownSpell && MySettings.UseDeathandDecay
            && Death_and_Decay.IsSpellUsable && Death_and_Decay.IsDistanceGood)
        {
            SpellManager.CastSpellByIDAndPosition(43265, ObjectManager.Target.Position);
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() < 4 && ObjectManager.GetNumberAttackPlayer() > 1 && Heart_Strike.IsSpellUsable
            && Heart_Strike.IsDistanceGood && Heart_Strike.KnownSpell && MySettings.UseHeartStrike)
        {
            Heart_Strike.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 4 && Army_of_the_Dead.IsSpellUsable && Army_of_the_Dead.IsDistanceGood
            && Army_of_the_Dead.KnownSpell && MySettings.UseArmyoftheDead)
        {
            Army_of_the_Dead.Launch();
            Thread.Sleep(4000);
            return;
        }
        else if (ObjectManager.Me.HaveBuff(81141) && MySettings.UseBloodBoil)
        {
            Blood_Boil.Launch();
            if (Blood_Plague.TargetHaveBuff && Frost_Fever.TargetHaveBuff)
            {
                Blood_Plague_Timer = new Timer(1000*27);
                Frost_Fever_Timer = new Timer(1000*27);
            }
            return;
        }
        else if (Soul_Reaper.KnownSpell && Soul_Reaper.IsDistanceGood && Soul_Reaper.IsSpellUsable
            && ObjectManager.Target.HealthPercent < 35 && ObjectManager.Me.HealthPercent > 90
            && MySettings.UseSoulReaper)
        {
            Soul_Reaper.Launch();
            return;
        }
        else if (Death_Strike.IsSpellUsable && Death_Strike.IsDistanceGood && Death_Strike.KnownSpell
             && MySettings.UseDeathStrike)
        {
            Death_Strike.Launch();
            return;
        }
        else if (Rune_Strike.IsSpellUsable && Rune_Strike.IsDistanceGood && Rune_Strike.KnownSpell && DRW == 0
            && MySettings.UseRuneStrike)
        {
            if (ObjectManager.Me.HealthPercent < 80 && ((Lichborne.KnownSpell && MySettings.UseLichborne)
                || (Conversion.KnownSpell && MySettings.UseConversion)))
                return;
            else
            {
                Rune_Strike.Launch();
                return;
            }
        }
        // Blizzard API Calls for Heart Strike using Blood Strike Function
        else if (Blood_Strike.IsSpellUsable && Blood_Strike.IsDistanceGood && Blood_Strike.KnownSpell
            && MySettings.UseHeartStrike)
        {
            Blood_Strike.Launch();
            return;
        }
        else if (Empower_Rune_Weapon.IsSpellUsable && Empower_Rune_Weapon.KnownSpell && MySettings.UseEmpowerRuneWeapon)
        {
            Empower_Rune_Weapon.Launch();
            return;
        }
        else if (Horn_of_Winter.KnownSpell && Horn_of_Winter.IsSpellUsable && MySettings.UseHornofWinter)
        {
            Horn_of_Winter.Launch();
            return;
        }
        else
        {
            if (Arcane_Torrent.IsSpellUsable && Arcane_Torrent.KnownSpell && MySettings.UseArcaneTorrent)
            {
                Arcane_Torrent.Launch();
                return;
            }
        }
    }

    public void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            Presence();
            Heal();
            Buff();
        }
    }

    private void Presence()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (Unholy_Presence.KnownSpell && MySettings.UseLowCombat && MySettings.UseUnholyPresence)
        {
            if (!Unholy_Presence.HaveBuff && Unholy_Presence.IsSpellUsable && LC == 1)
                Unholy_Presence.Launch();
        }

        else if (Blood_Presence.KnownSpell && MySettings.UseBloodPresence)
        {
            if (!Blood_Presence.HaveBuff && Blood_Presence.IsSpellUsable)
                Blood_Presence.Launch();
        }

        else
        {
            if (Frost_Presence.KnownSpell && MySettings.UseFrostPresence && !MySettings.UseBloodPresence)
            {
                if (!Frost_Presence.HaveBuff && Frost_Presence.IsSpellUsable)
                    Frost_Presence.Launch();
            }
        }
    }

    private void Grip()
    {
        if (ObjectManager.Target.GetDistance > 5 &&
            Death_Grip.KnownSpell && Death_Grip.IsSpellUsable && Death_Grip.IsDistanceGood
            && MySettings.UseDeathGrip)
        {
            Death_Grip.Launch();
            MovementManager.StopMove();
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (ObjectManager.Me.HealthPercent < 80 && Gift_of_the_Naaru.IsSpellUsable && Gift_of_the_Naaru.KnownSpell
            && MySettings.UseGiftoftheNaaru)
        {
            Gift_of_the_Naaru.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 55 && MySettings.UseDeathPact
            && Raise_Dead.KnownSpell && Raise_Dead.IsSpellUsable
            && Death_Pact.KnownSpell && Death_Pact.IsSpellUsable)
        {
            int i = 0;
            while (i < 3)
            {
                i++;
                Raise_Dead.Launch();
                Death_Pact.Launch();

                if (!Death_Pact.IsSpellUsable)
                {
                    break;
                }
            }
        }
        else if (ObjectManager.Me.HealthPercent < 45 && MySettings.UseLichborne
            && Lichborne.KnownSpell && Lichborne.IsSpellUsable
            && Death_Coil.KnownSpell && Death_Coil.IsSpellUsable)
        {
            if (Lichborne.IsSpellUsable)
            {
                Lichborne.Launch();
                return;
            }
        }
        else if (ObjectManager.Me.HealthPercent < 45 && MySettings.UseConversion
            && Conversion.KnownSpell && Conversion.IsSpellUsable
            && ObjectManager.Me.RunicPower > 10)
        {
            if (Conversion.IsSpellUsable)
            {
                Conversion.Launch();
                while (ObjectManager.Me.RunicPower > 0)
                    Thread.Sleep(200);
                return;
            }
        }
        else if (ObjectManager.Me.HealthPercent < 80 && Death_Siphon.KnownSpell && Death_Siphon.IsSpellUsable
            && MySettings.UseDeathSiphon)
        {
            Death_Siphon.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Me.HealthPercent < 70)
            {
                if (Vampiric_Blood.KnownSpell && Vampiric_Blood.IsSpellUsable && MySettings.UseVampiricBlood)
                {
                    Vampiric_Blood.Launch();
                    Thread.Sleep(200);
                }

                if (!Rune_Tap.IsSpellUsable && Blood_Tap.KnownSpell && Blood_Tap.IsSpellUsable
                    && MySettings.UseBloodTap)
                {
                    Blood_Tap.Launch();
                    Thread.Sleep(200);
                }

                if (Rune_Tap.KnownSpell && Rune_Tap.IsSpellUsable && MySettings.UseRuneTap
                    && (Vampiric_Blood.HaveBuff || ObjectManager.Me.HaveBuff(81164)))
                    Rune_Tap.Launch();
            }
        }
    }

    private void Defense_Cycle()
    {
        if (!Bone_Shield.HaveBuff && Bone_Shield.KnownSpell && Bone_Shield.IsSpellUsable
            && MySettings.UseBoneShield)
        {
            Bone_Shield.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && MySettings.UseIceboundFortitude
            && Icebound_Fortitude.KnownSpell && Icebound_Fortitude.IsSpellUsable)
        {
            Icebound_Fortitude.Launch();
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
        else
        {
            if (ObjectManager.Target.GetDistance < 8 && MySettings.UseRemorselessWinter
            && (ObjectManager.Me.HealthPercent < 70 || ObjectManager.GetNumberAttackPlayer() > 1))
            {
                if (Remorseless_Winter.KnownSpell && Remorseless_Winter.IsSpellUsable)
                {
                    Remorseless_Winter.Launch();
                    OnCD = new Timer(1000*8);
                    return;
                }
            }
        }
    }

    private void Decast()
    {
        if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && MySettings.UseMindFreeze
            && Mind_Freeze.KnownSpell && Mind_Freeze.IsSpellUsable && Mind_Freeze.IsDistanceGood)
        {
            Mind_Freeze.Launch();
            return;
        }
        else if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && MySettings.UseAntiMagicShell
            && AntiMagic_Shell.KnownSpell && AntiMagic_Shell.IsSpellUsable)
        {
            AntiMagic_Shell.Launch();
            return;
        }
        else if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe
            && Strangulate.KnownSpell && Strangulate.IsSpellUsable && Strangulate.IsDistanceGood
            && (MySettings.UseStrangulate || MySettings.UseAsphyxiate))
        {
            Strangulate.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && AntiMagic_Zone.KnownSpell
                && MySettings.UseAntiMagicZone && AntiMagic_Zone.IsSpellUsable)
            {
                SpellManager.CastSpellByIDAndPosition(51052, ObjectManager.Me.Position);
                return;
            }
        }

        if (ObjectManager.Target.GetMove && !Chains_of_Ice.TargetHaveBuff && MySettings.UseChainsofIce
            && Chains_of_Ice.KnownSpell && Chains_of_Ice.IsSpellUsable && Chains_of_Ice.IsDistanceGood)
        {
            Chains_of_Ice.Launch();
            return;
        }
    }

    private void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (!Horn_of_Winter.HaveBuff && Horn_of_Winter.KnownSpell && Horn_of_Winter.IsSpellUsable
            && MySettings.UseHornofWinter)
        {
            Horn_of_Winter.Launch();
            return;
        }
        else if (!Fight.InFight && Deaths_Advance.KnownSpell && Deaths_Advance.IsSpellUsable
            && MySettings.UseDeathsAdvance)
        {
            Deaths_Advance.Launch();
            return;
        }
        else
        {
            if (AlchFlask_Timer.IsReady && MySettings.UseAlchFlask
            && ItemsManager.GetItemCountByIdLUA(75525) == 1)
            {
                Logging.WriteFight("Use Alchi Flask");
                Lua.RunMacroText("/use item:75525");
                AlchFlask_Timer = new Timer(1000*60*60*2);
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

public class Deathknight_Unholy
{
    [Serializable]
    public class DeathknightUnholySettings : nManager.Helpful.Settings
    {
        /* Professions & Racials */
        public bool UseArcaneTorrent = true;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseLifeblood = true;
        public bool UseStoneform = true;
        public bool UseGiftoftheNaaru = true;
        public bool UseWarStomp = true;
        /* Deathknight Presence & Buffs */
        public bool UseFrostPresence = true;
        public bool UseBloodPresence = true;
        public bool UseHornofWinter = true;
        public bool UsePathofFrost = true;
        public bool UseUnholyPresence = true;
        /* Offensive Spell */
        public bool UseBloodBoil = true;
        public bool UseDarkTransformation = true;
        public bool UseDeathCoil = true;
        public bool UseDeathandDecay = true;
        public bool UseFesteringStrike = true;
        public bool UseIcyTouch = true;
        public bool UsePlagueStrike = true;
        public bool UseSoulReaper = true;
        public bool UseScourgeStrike = true;
        public bool UseUnholyBlight = true;
        /* Offensive Cooldown */
        public bool UseBloodTap = true;
        public bool UseDeathGrip = true;
        public bool UseEmpowerRuneWeapon = true;
        public bool UseOutbreak = true;
        public bool UsePestilence = true;
        public bool UseRaiseDead = true;
        public bool UseSummonGargoyle = true;
        public bool UseUnholyFrenzy = true;
        /* Defensive Cooldown */
        public bool UseAntiMagicShell = true;
        public bool UseAntiMagicZone = true;
        public bool UseArmyoftheDead = true;
        public bool UseAsphyxiate = true;
        public bool UseChainsofIce = true;
        public bool UseDeathsAdvance = true;
        public bool UseIceboundFortitude = true;
        public bool UseMindFreeze = true;
        public bool UseRemorselessWinter = true;
        public bool UseStrangulate = true;
        /* Healing Spell */
        public bool UseConversion = true;
        public bool UseDeathPact = true;
        public bool UseDeathSiphon = true;
        public bool UseDeathStrike = true;
        public bool UseLichborne = true;
        /* Game Settings */
        public bool UseLowCombat = true;
        public bool UseTrinket = true;
        public bool UseEngGlove = true;
        public bool UseAlchFlask = true;

        public DeathknightUnholySettings()
        {
            ConfigWinForm(new System.Drawing.Point(400, 400), "Deathknight Unholy Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");
            AddControlInWinForm("Use Lifeblood", "UseLifeblood", "Professions & Racials");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            /* Deathknight Presence & Buffs */
            AddControlInWinForm("Use Frost Presence", "UseFrostPresence", "Deathknight Presence & Buffs");
            AddControlInWinForm("Use Blood Presence", "UseBloodPresence", "Deathknight Presence & Buffs");
            AddControlInWinForm("Use Horn of Winter", "UseHornofWinter", "Deathknight Presence & Buffs");
            AddControlInWinForm("Use Path of Frost", "UsePathofFrost", "Deathknight Presence & Buffs");
            AddControlInWinForm("Use Unholy Presence", "UseUnholyPresence", "Deathknight Presence & Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Blood Boil", "UseBloodBoil", "Offensive Spell");
            AddControlInWinForm("Use Dark Transformation", "UseDarkTransformation", "Offensive Spell");
            AddControlInWinForm("Use Death Coil", "UseDeathCoil", "Offensive Spell");
            AddControlInWinForm("Use Death and Decay", "UseDeathandDecay", "Offensive Spell");
            AddControlInWinForm("Use Festering Strike", "UseFesteringStrike", "Offensive Spell");
            AddControlInWinForm("Use Icy Touch", "UseIcyTouch", "Offensive Spell");
            AddControlInWinForm("Use Plague Strike", "UsePlagueStrike", "Offensive Spell");
            AddControlInWinForm("Use Soul Reaper", "UseSoulReaper", "Offensive Spell");
            AddControlInWinForm("Use Scourge Strike", "UseScourgeStrike", "Offensive Spell");
            AddControlInWinForm("Use Unholy Blight", "UseUnholyBlight", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use Blood Tap", "UseBloodTap", "Offensive Cooldown");
            AddControlInWinForm("Use Death Grip", "UseDeathGrip", "Offensive Cooldown");
            AddControlInWinForm("Use Empower Rune Weapon", "UseEmpowerRuneWeapon", "Offensive Cooldown");
            AddControlInWinForm("Use Outbreak", "UseOutbreak", "Offensive Cooldown");
            AddControlInWinForm("Use Pestilence", "UsePestilence", "Offensive Cooldown");
            AddControlInWinForm("Use Raise Dead", "UseRaiseDead", "Offensive Cooldown");
            AddControlInWinForm("Use Summon Gargoyle", "UseSummonGargoyle", "Offensive Cooldown");
            AddControlInWinForm("Use Unholy Frenzy", "UseUnholyFrenzy", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Anti-Magic Shell", "UseAntiMagicShell", "Defensive Cooldown");
            AddControlInWinForm("Use Anti-Magic Zone", "UseAntiMagicZone", "Defensive Cooldown");
            AddControlInWinForm("Use Army of the Dead", "UseArmyoftheDead", "Defensive Cooldown");
            AddControlInWinForm("Use Asphyxiate", "UseAsphyxiate", "Defensive Cooldown");
            AddControlInWinForm("Use Chains of Ice", "UseChainsofIce", "Defensive Cooldown");
            AddControlInWinForm("Use Death's Advance", "UseDeathsAdvance", "Defensive Cooldown");
            AddControlInWinForm("Use Icebound Fortitude", "UseIceboundFortitude", "Defensive Cooldown");
            AddControlInWinForm("Use Mind Freeze", "UseMindFreeze", "Defensive Cooldown");
            AddControlInWinForm("Use Remorseless Winter", "UseRemorseless Winter", "Defensive Cooldown");
            AddControlInWinForm("Use Strangulate", "UseStrangulate", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Conversion", "UseConversion", "Healing Spell");
            AddControlInWinForm("Use Death Pact", "UseDeathPact", "Healing Spell");
            AddControlInWinForm("Use Death Siphon", "UseDeathSiphon", "Healing Spell");
            AddControlInWinForm("Use Death Strike", "UseDeathStrike", "Healing Spell");
            AddControlInWinForm("Use Lichborne", "UseLichborne", "Healing Spell");
            /* Game Settings */
            AddControlInWinForm("Use Low Combat Settings", "UseLowCombat", "Game Settings");
            AddControlInWinForm("Use Trinket", "UseTrinket", "Game Settings");
            AddControlInWinForm("Use Engineering Gloves", "UseEngGlove", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
        }

        public static DeathknightUnholySettings CurrentSetting { get; set; }

        public static DeathknightUnholySettings GetSettings()
        {
            string CurrentSettingsFile = Application.StartupPath + "\\CustomClasses\\Settings\\Deathknight_Unholy.xml";
            if (System.IO.File.Exists(CurrentSettingsFile))
            {
                return
                    CurrentSetting = Settings.Load<Deathknight_Unholy.DeathknightUnholySettings>(CurrentSettingsFile);
            }
            else
            {
                return new Deathknight_Unholy.DeathknightUnholySettings();
            }
        }
    }

    private readonly DeathknightUnholySettings MySettings = DeathknightUnholySettings.GetSettings();

    #region Professions & Racials

    private readonly Spell Arcane_Torrent = new Spell("Arcane Torrent");
    private readonly Spell Berserking = new Spell("Berserking");
    private readonly Spell Blood_Fury = new Spell("Blood Fury");
    private readonly Spell Engineering = new Spell("Engineering");
    private readonly Spell Lifeblood = new Spell("Lifeblood");
    private readonly Spell Stoneform = new Spell("Stoneform");
    private readonly Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    private readonly Spell War_Stomp = new Spell("War Stomp");

    #endregion

    #region Deathknight Presence & Buffs

    private readonly Spell Blood_Plague = new Spell("Blood Plague");
    private Timer Blood_Plague_Timer = new Timer(0);
    private readonly Spell Blood_Presence = new Spell("Blood Presence");
    private readonly Spell Frost_Fever = new Spell("Frost Fever");
    private Timer Frost_Fever_Timer = new Timer(0);
    private readonly Spell Frost_Presence = new Spell("Frost Presence");
    private readonly Spell Horn_of_Winter = new Spell("Horn of Winter");
    private readonly Spell Path_of_Frost = new Spell("Path of Frost");
    private Timer Path_of_Frost_Timer = new Timer(0);
    private readonly Spell Roiling_Blood = new Spell("Roiling Blood");
    private readonly Spell Unholy_Presence = new Spell("Unholy Presence");

    #endregion

    #region Offensive Spell

    private readonly Spell Blood_Boil = new Spell("Blood Boil");
    private readonly Spell Blood_Strike = new Spell("Blood Strike");
    private readonly Spell Dark_Transformation = new Spell("Dark Transformation");
    private Timer Dark_Transformation_Timer = new Timer(0);
    private readonly Spell Death_and_Decay = new Spell("Death and Decay");
    private readonly Spell Death_Coil = new Spell("Death Coil");
    private readonly Spell Festering_Strike = new Spell("Festering Strike");
    private readonly Spell Icy_Touch = new Spell("Icy Touch");
    private readonly Spell Plague_Strike = new Spell("Plague Strike");
    private readonly Spell Soul_Reaper = new Spell("Soul Reaper");
    private readonly Spell Scourge_Strike = new Spell("Scourge Strike");
    private readonly Spell Unholy_Blight = new Spell("Unholy Blight");

    #endregion

    #region Offensive Cooldown

    private readonly Spell Blood_Tap = new Spell("Blood Tap");
    private readonly Spell Death_Grip = new Spell("Death Grip");
    private readonly Spell Empower_Rune_Weapon = new Spell("Empower Rune Weapon");
    private readonly Spell Outbreak = new Spell("Outbreak");
    private readonly Spell Pestilence = new Spell("Pestilence");
    private Timer Pestilence_Timer = new Timer(0);
    private readonly Spell Raise_Dead = new Spell("Raise Dead");
    private readonly Spell Summon_Gargoyle = new Spell("Summon Gargoyle");
    private Timer Summon_Gargoyle_Timer = new Timer(0);
    private readonly Spell Unholy_Frenzy = new Spell("Unholy Frenzy");

    #endregion

    #region Defensive Cooldown

    private readonly Spell AntiMagic_Shell = new Spell("Anti-Magic Shell");
    private readonly Spell AntiMagic_Zone = new Spell("Anti-Magic Zone");
    private readonly Spell Army_of_the_Dead = new Spell("Army of the Dead");
    private readonly Spell Asphyxiate = new Spell("Asphyxiate");
    private readonly Spell Chains_of_Ice = new Spell("Chains of Ice");
    private readonly Spell Deaths_Advance = new Spell("Death's Advance");
    private readonly Spell Icebound_Fortitude = new Spell("Icebound Fortitude");
    private readonly Spell Mind_Freeze = new Spell("Mind Freeze");
    private readonly Spell Remorseless_Winter = new Spell("Remorseless Winter");
    private readonly Spell Strangulate = new Spell("Strangulate");

    #endregion

    #region Healing Spell

    private readonly Spell Conversion = new Spell("Conversion");
    private readonly Spell Death_Pact = new Spell("Death Pact");
    private readonly Spell Death_Siphon = new Spell("Death Siphon");
    private readonly Spell Death_Strike = new Spell("Death Strike");
    private readonly Spell Lichborne = new Spell("Lichborne");

    #endregion

    private Timer Trinket_Timer = new Timer(0);
    private Timer Engineering_Timer = new Timer(0);
    private Timer AlchFlask_Timer = new Timer(0);
    private Timer OnCD = new Timer(0);
    public int SG = 1;
    public int DT = 1;
    public int LC = 0;

    public Deathknight_Unholy()
    {
        Main.range = 5.0f;
        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            try
            {
                Buff_Path();
                if (!ObjectManager.Me.IsMounted)
                {
                    if (Fight.InFight && ObjectManager.Me.Target > 0)
                    {
                        if (ObjectManager.Me.Target != lastTarget &&
                            (Death_Grip.IsDistanceGood || Icy_Touch.IsDistanceGood))
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
            Thread.Sleep(150);
        }
    }

    public void Pull()
    {
        if (Death_Grip.IsSpellUsable && MySettings.UseDeathGrip && Death_Grip.IsDistanceGood)
            Grip();
        else
        {
            if (Icy_Touch.IsSpellUsable && MySettings.UseIcyTouch && Icy_Touch.IsDistanceGood)
                Icy_Touch.Launch();
        }
    }

    private void Buff_Path()
    {
        if (!Fight.InFight && Path_of_Frost.KnownSpell && Path_of_Frost.IsSpellUsable && MySettings.UsePathofFrost
            && (!Path_of_Frost.HaveBuff || Path_of_Frost_Timer.IsReady))
        {
            Path_of_Frost.Launch();
            Path_of_Frost_Timer = new Timer(1000*60*9);
        }
    }

    public void LowCombat()
    {
        Presence();
        AvoidMelee();
        Defense_Cycle();
        Heal();
        Decast();
        Buff();
        Grip();

        if (Icy_Touch.KnownSpell && Icy_Touch.IsSpellUsable && Icy_Touch.IsDistanceGood && MySettings.UseIcyTouch)
        {
            Icy_Touch.Launch();
            if (ObjectManager.Target.HealthPercent < 50 && ObjectManager.Target.HealthPercent > 0)
            {
                Icy_Touch.Launch();
                return;
            }
            return;
        }
        else if (Death_Coil.KnownSpell && Death_Coil.IsSpellUsable && Death_Coil.IsDistanceGood
            && MySettings.UseDeathCoil)
        {
            Death_Coil.Launch();
            return;
        }
        else
        {
            if (Blood_Boil.KnownSpell && Blood_Boil.IsSpellUsable && Blood_Boil.IsDistanceGood
                && MySettings.UseBloodBoil)
            {
                Blood_Boil.Launch();
                return;
            }
        }
    }

    public void Combat()
    {
        Presence();
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
        else
        {
            if (Unholy_Frenzy.IsSpellUsable && Unholy_Frenzy.KnownSpell
                && ObjectManager.Target.GetDistance < 30)
            {
                Unholy_Frenzy.Launch();
                return;
            }
        }

        if (Summon_Gargoyle_Timer.IsReady && SG == 0)
            SG++;

        if (SG == 1 && Summon_Gargoyle.IsSpellUsable && Summon_Gargoyle.KnownSpell && Summon_Gargoyle.IsDistanceGood
            && MySettings.UseSummonGargoyle)
        {
            Summon_Gargoyle.Launch();
            Summon_Gargoyle_Timer = new Timer(180000);
            SG--;
            return;
        }

        if (Dark_Transformation_Timer.IsReady && DT == 0)
            DT++;

        if (DT == 1 && Dark_Transformation.IsSpellUsable && Dark_Transformation.KnownSpell
            && MySettings.UseDarkTransformation)
        {
            Dark_Transformation.Launch();
            Dark_Transformation_Timer = new Timer(1000*30);
            DT--;
            return;
        }
    }

    public void DPS_Cycle()
    {
        Grip();

        if (ObjectManager.Me.HealthPercent < 85 && MySettings.UseDeathCoil
            && Lichborne.HaveBuff && Death_Coil.KnownSpell && Death_Coil.IsSpellUsable)
        {
            Lua.RunMacroText("/target Player");
            Death_Coil.Launch();
            return;
        }
        else if (Unholy_Blight.KnownSpell && Unholy_Blight.IsSpellUsable && ObjectManager.Target.GetDistance < 9
            && MySettings.UseUnholyBlight)
        {
            Unholy_Blight.Launch();
            Blood_Plague_Timer = new Timer(1000*27);
            Frost_Fever_Timer = new Timer(1000*27);
            return;
        }
        else if (Outbreak.KnownSpell && Outbreak.IsSpellUsable && Outbreak.IsDistanceGood && MySettings.UseOutbreak
            && (Blood_Plague_Timer.IsReady || Frost_Fever_Timer.IsReady || !Blood_Plague.TargetHaveBuff || !Frost_Fever.TargetHaveBuff))
        {
            Outbreak.Launch();
            Blood_Plague_Timer = new Timer(1000*27);
            Frost_Fever_Timer = new Timer(1000*27);
            return;
        }
        else if (Blood_Plague_Timer.IsReady && Frost_Fever_Timer.IsReady && Blood_Plague.TargetHaveBuff && Frost_Fever.TargetHaveBuff
            && Blood_Boil.IsSpellUsable && Blood_Boil.KnownSpell && ObjectManager.Target.GetDistance < 9
            && MySettings.UseBloodBoil && Roiling_Blood.KnownSpell)
        {
            Blood_Boil.Launch();
            Blood_Plague_Timer = new Timer(1000*27);
            Frost_Fever_Timer = new Timer(1000*27);
            return;
        }
        else if (Plague_Strike.KnownSpell && Plague_Strike.IsDistanceGood && !Outbreak.IsSpellUsable
            && !Unholy_Blight.IsSpellUsable && MySettings.UsePlagueStrike
            && (Blood_Plague_Timer.IsReady || !Blood_Plague.TargetHaveBuff))
        {
            if (!Plague_Strike.IsSpellUsable && Blood_Tap.KnownSpell && Blood_Tap.IsSpellUsable
                && MySettings.UseBloodTap)
            {
                Blood_Tap.Launch();
                Thread.Sleep(200);
            }

            if (Plague_Strike.IsSpellUsable)
            {
                Plague_Strike.Launch();
                Blood_Plague_Timer = new Timer(1000*27);
                return;
            }
        }
        else if (Icy_Touch.KnownSpell && Icy_Touch.IsDistanceGood && MySettings.UseIcyTouch
            && !Outbreak.IsSpellUsable && !Unholy_Blight.IsSpellUsable
            && (Frost_Fever_Timer.IsReady || !Frost_Fever.TargetHaveBuff))
        {
            if (!Icy_Touch.IsSpellUsable && Blood_Tap.KnownSpell && Blood_Tap.IsSpellUsable
                && MySettings.UseBloodTap)
            {
                Blood_Tap.Launch();
                Thread.Sleep(200);
            }

            if (Icy_Touch.IsSpellUsable)
            {
                Icy_Touch.Launch();
                Frost_Fever_Timer = new Timer(1000*27);
                return;
            }
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 2 && Pestilence.IsSpellUsable && Pestilence.IsDistanceGood
            && Pestilence.KnownSpell && MySettings.UsePestilence && !Roiling_Blood.KnownSpell)
        {
            Pestilence.Launch();
            return;
        }
        else if (Dark_Transformation_Timer.IsReady && DT == 1 && SG == 0 && MySettings.UseDarkTransformation)
        {
            if (Death_Coil.KnownSpell && Death_Coil.IsSpellUsable && Death_Coil.IsDistanceGood
                && MySettings.UseDeathCoil)
            {
                if (ObjectManager.Me.HealthPercent < 80 && ((Lichborne.KnownSpell && MySettings.UseLichborne)
                    || (Conversion.KnownSpell && MySettings.UseConversion)))
                    return;
                else
                {
                    Death_Coil.Launch();
                    return;
                }
            }
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 2 && MySettings.UseDeathandDecay
            && Death_and_Decay.KnownSpell && Death_and_Decay.IsSpellUsable && Death_and_Decay.IsDistanceGood)
        {
            SpellManager.CastSpellByIDAndPosition(43265, ObjectManager.Target.Position);
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 4 && Army_of_the_Dead.IsSpellUsable && Army_of_the_Dead.IsDistanceGood
            && Army_of_the_Dead.KnownSpell && MySettings.UseArmyoftheDead)
        {
            Army_of_the_Dead.Launch();
            Thread.Sleep(4000);
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 2 && Blood_Boil.IsSpellUsable && Blood_Boil.IsDistanceGood && Blood_Boil.KnownSpell
            && MySettings.UseBloodBoil)
        {
            Blood_Boil.Launch();
            if (Blood_Plague.TargetHaveBuff && Frost_Fever.TargetHaveBuff && Roiling_Blood.KnownSpell)
            {
                Blood_Plague_Timer = new Timer(1000*27);
                Frost_Fever_Timer = new Timer(1000*27);
            }
            return;
        }

        else if (Soul_Reaper.KnownSpell && Soul_Reaper.IsDistanceGood && Soul_Reaper.IsSpellUsable
            && ObjectManager.Target.HealthPercent < 35 && ObjectManager.Me.HealthPercent > 80
            && MySettings.UseSoulReaper)
        {
            Soul_Reaper.Launch();
            return;
        }
        else if (ObjectManager.Me.RunicPowerPercentage < 90 && MySettings.UseScourgeStrike
            && Scourge_Strike.KnownSpell && Scourge_Strike.IsSpellUsable && Scourge_Strike.IsDistanceGood)
        {
            Scourge_Strike.Launch();
            return;
        }
        else if (ObjectManager.Me.RunicPowerPercentage < 90 && MySettings.UseFesteringStrike
            && Festering_Strike.KnownSpell && Festering_Strike.IsSpellUsable && Festering_Strike.IsDistanceGood)
        {
            Festering_Strike.Launch();
            return;
        }
        else if (ObjectManager.Me.RunicPowerPercentage >= 90 || ObjectManager.Me.HaveBuff(81340)
            && Death_Coil.KnownSpell && Death_Coil.IsSpellUsable && Death_Coil.IsDistanceGood
            && MySettings.UseDeathCoil)
        {
            if (Blood_Tap.IsSpellUsable && Blood_Tap.KnownSpell)
            {
                Blood_Tap.Launch();
                Thread.Sleep(200);
            }
            Death_Coil.Launch();
            return;
        }
        else if (Horn_of_Winter.KnownSpell && Horn_of_Winter.IsSpellUsable && MySettings.UseHornofWinter)
        {
            Horn_of_Winter.Launch();
            return;
        }
        else if (Empower_Rune_Weapon.IsSpellUsable && Empower_Rune_Weapon.KnownSpell && MySettings.UseEmpowerRuneWeapon)
        {
            Empower_Rune_Weapon.Launch();
            return;
        }
        else
        {
            if (Arcane_Torrent.IsSpellUsable && Arcane_Torrent.KnownSpell && MySettings.UseArcaneTorrent)
            {
                Arcane_Torrent.Launch();
                return;
            }
        }
    }

    public void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            Presence();
            Heal();
            Buff();
        }
    }

    private void Presence()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (Unholy_Presence.KnownSpell && MySettings.UseUnholyPresence && ObjectManager.Me.HealthPercent > 50)
        {
            if (!Unholy_Presence.HaveBuff && Unholy_Presence.IsSpellUsable)
                Unholy_Presence.Launch();
        }
        else if (Blood_Presence.KnownSpell && MySettings.UseBloodPresence && ObjectManager.Me.HealthPercent < 30)
        {
            if (!Blood_Presence.HaveBuff && Blood_Presence.IsSpellUsable)
                Blood_Presence.Launch();
        }
        else
        {
            if (Frost_Presence.KnownSpell && MySettings.UseFrostPresence && !MySettings.UseUnholyPresence)
            {
                if (!Frost_Presence.HaveBuff && Frost_Presence.IsSpellUsable)
                    Frost_Presence.Launch();
            }
        }
    }

    private void Grip()
    {
        if (ObjectManager.Target.GetDistance > 5 &&
            Death_Grip.KnownSpell && Death_Grip.IsSpellUsable && Death_Grip.IsDistanceGood
            && MySettings.UseDeathGrip)
        {
            Death_Grip.Launch();
            MovementManager.StopMove();
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (ObjectManager.Me.HealthPercent < 80 && Gift_of_the_Naaru.IsSpellUsable && Gift_of_the_Naaru.KnownSpell
            && MySettings.UseGiftoftheNaaru)
        {
            Gift_of_the_Naaru.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 55 && MySettings.UseDeathPact
            && Raise_Dead.KnownSpell && Raise_Dead.IsSpellUsable
            && Death_Pact.KnownSpell && Death_Pact.IsSpellUsable)
        {
            int i = 0;
            while (i < 3)
            {
                i++;
                Raise_Dead.Launch();
                Death_Pact.Launch();

                if (!Death_Pact.IsSpellUsable)
                {
                    break;
                }
            }
        }
        else if (ObjectManager.Me.HealthPercent < 45 && MySettings.UseLichborne
            && Lichborne.KnownSpell && Lichborne.IsSpellUsable
            && Death_Coil.KnownSpell && Death_Coil.IsSpellUsable)
        {
            if (Lichborne.IsSpellUsable)
            {
                Lichborne.Launch();
                return;
            }
        }
        else if (ObjectManager.Me.HealthPercent < 45 && MySettings.UseConversion
            && Conversion.KnownSpell && Conversion.IsSpellUsable
            && ObjectManager.Me.RunicPower > 10)
        {
            if (Conversion.IsSpellUsable)
            {
                Conversion.Launch();
                while (ObjectManager.Me.RunicPower > 0)
                    Thread.Sleep(200);
                return;
            }
        }
        else if (ObjectManager.Me.HealthPercent < 80 && Death_Siphon.KnownSpell && Death_Siphon.IsSpellUsable
            && MySettings.UseDeathSiphon)
        {
            Death_Siphon.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Me.HealthPercent < 80 && Death_Strike.IsSpellUsable && Death_Strike.KnownSpell
                && MySettings.UseDeathStrike)
            {
                Death_Strike.Launch();
                return;
            }
        }
    }

    private void Defense_Cycle()
    {
        if (ObjectManager.Me.HealthPercent < 80 && Stoneform.IsSpellUsable && Stoneform.KnownSpell)
        {
            Stoneform.Launch();
            OnCD = new Timer(1000*8);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && MySettings.UseIceboundFortitude
            && Icebound_Fortitude.KnownSpell && Icebound_Fortitude.IsSpellUsable)
        {
            Icebound_Fortitude.Launch();
            OnCD = new Timer(1000*12);
            return;
        }
        else
        {
            if (ObjectManager.Target.GetDistance < 8 && MySettings.UseRemorselessWinter
            && (ObjectManager.Me.HealthPercent < 70 || ObjectManager.GetNumberAttackPlayer() > 1))
            {
                if (Remorseless_Winter.KnownSpell && Remorseless_Winter.IsSpellUsable)
                {
                    Remorseless_Winter.Launch();
                    OnCD = new Timer(1000*8);
                    return;
                }
            }
        }
    }

    private void Decast()
    {
        if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && MySettings.UseMindFreeze
            && Mind_Freeze.KnownSpell && Mind_Freeze.IsSpellUsable && Mind_Freeze.IsDistanceGood)
        {
            Mind_Freeze.Launch();
            return;
        }
        else if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && MySettings.UseAntiMagicShell
            && AntiMagic_Shell.KnownSpell && AntiMagic_Shell.IsSpellUsable)
        {
            AntiMagic_Shell.Launch();
            return;
        }
        else if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe
            && Strangulate.KnownSpell && Strangulate.IsSpellUsable && Strangulate.IsDistanceGood
            && (MySettings.UseStrangulate || MySettings.UseAsphyxiate))
        {
            Strangulate.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && AntiMagic_Zone.KnownSpell
                && MySettings.UseAntiMagicZone && AntiMagic_Zone.IsSpellUsable)
            {
                SpellManager.CastSpellByIDAndPosition(51052, ObjectManager.Me.Position);
                return;
            }
        }

        if (ObjectManager.Target.GetMove && !Chains_of_Ice.TargetHaveBuff && MySettings.UseChainsofIce
            && Chains_of_Ice.KnownSpell && Chains_of_Ice.IsSpellUsable && Chains_of_Ice.IsDistanceGood)
        {
            Chains_of_Ice.Launch();
            return;
        }
    }

    private void Ghoul()
    {
        if ((ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0) &&
            !ObjectManager.Me.IsMounted && !ObjectManager.Me.IsDeadMe && MySettings.UseRaiseDead)
        {
            Logging.WriteFight(" - PET DEAD - ");
            if (Raise_Dead.KnownSpell && Raise_Dead.IsSpellUsable)
            {
                Logging.WriteFight(" - SUMMONING PET - ");
                Raise_Dead.Launch();
            }
        }
    }

    private void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        Ghoul();

        if (!Horn_of_Winter.HaveBuff && Horn_of_Winter.KnownSpell && Horn_of_Winter.IsSpellUsable
            && MySettings.UseHornofWinter)
        {
            Horn_of_Winter.Launch();
            return;
        }
        else if (!Fight.InFight && Deaths_Advance.KnownSpell && Deaths_Advance.IsSpellUsable
            && MySettings.UseDeathsAdvance)
        {
            Deaths_Advance.Launch();
            return;
        }
        else
        {
            if (AlchFlask_Timer.IsReady && MySettings.UseAlchFlask
            && ItemsManager.GetItemCountByIdLUA(75525) == 1)
            {
                Logging.WriteFight("Use Alchi Flask");
                Lua.RunMacroText("/use item:75525");
                AlchFlask_Timer = new Timer(1000*60*60*2);
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

public class Deathknight_Frost
{
    [Serializable]
    public class DeathknightFrostSettings : nManager.Helpful.Settings
    {
        /* Professions & Racials */
        public bool UseArcaneTorrent = true;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseLifeblood = true;
        public bool UseStoneform = true;
        public bool UseGiftoftheNaaru = true;
        public bool UseWarStomp = true;
        /* Deathknight Presence & Buffs */
        public bool UseFrostPresence = true;
        public bool UseBloodPresence = true;
        public bool UseHornofWinter = true;
        public bool UsePathofFrost = true;
        public bool UseUnholyPresence = true;
        /* Offensive Spell */
        public bool UseBloodBoil = true;
        public bool UseDeathCoil = true;
        public bool UseDeathandDecay = true;
        public bool UseFrostStrike = true;
        public bool UseHowlingBlast = true;
        public bool UseIcyTouch = true;
        public bool UsePlagueStrike = true;
        public bool UseObliterate = true;
        public bool UseSoulReaper = true;
        public bool UseUnholyBlight = true;
        /* Offensive Cooldown */
        public bool UseBloodTap = true;
        public bool UseDeathGrip = true;
        public bool UseEmpowerRuneWeapon = true;
        public bool UseOutbreak = true;
        public bool UsePestilence = true;
        public bool UsePillarofFrost = true;
        public bool UseRaiseDead = true;
        /* Defensive Cooldown */
        public bool UseAntiMagicShell = true;
        public bool UseAntiMagicZone = true;
        public bool UseArmyoftheDead = true;
        public bool UseAsphyxiate = true;
        public bool UseChainsofIce = true;
        public bool UseDeathsAdvance = true;
        public bool UseIceboundFortitude = true;
        public bool UseMindFreeze = true;
        public bool UseRemorselessWinter = true;
        public bool UseStrangulate = true;
        /* Healing Spell */
        public bool UseConversion = true;
        public bool UseDeathPact = true;
        public bool UseDeathSiphon = true;
        public bool UseDeathStrike = true;
        public bool UseLichborne = true;
        /* Game Settings */
        public bool UseLowCombat = true;
        public bool UseTrinket = true;
        public bool UseEngGlove = true;
        public bool UseAlchFlask = true;
        public bool UseDuelWield = false;
        public bool UseTwoHander = true;

        public DeathknightFrostSettings()
        {
            ConfigWinForm(new System.Drawing.Point(400, 400), "Deathknight Frost Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");
            AddControlInWinForm("Use Lifeblood", "UseLifeblood", "Professions & Racials");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            /* Deathknight Presence & Buffs */
            AddControlInWinForm("Use Frost Presence", "UseFrostPresence", "Deathknight Presence & Buffs");
            AddControlInWinForm("Use Blood Presence", "UseBloodPresence", "Deathknight Presence & Buffs");
            AddControlInWinForm("Use Horn of Winter", "UseHornofWinter", "Deathknight Presence & Buffs");
            AddControlInWinForm("Use Path of Frost", "UsePathofFrost", "Deathknight Presence & Buffs");
            AddControlInWinForm("Use Unholy Presence", "UseUnholyPresence", "Deathknight Presence & Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Blood Boil", "UseBloodBoil", "Offensive Spell");
            AddControlInWinForm("Use Dark Transformation", "UseDarkTransformation", "Offensive Spell");
            AddControlInWinForm("Use Death Coil", "UseDeathCoil", "Offensive Spell");
            AddControlInWinForm("Use Death and Decay", "UseDeathandDecay", "Offensive Spell");
            AddControlInWinForm("Use Frost Strike", "UseFrostStrike", "Offensive Spell");
            AddControlInWinForm("Use Howling Blast", "UseHowlingBlast", "Offensive Spell");
            AddControlInWinForm("Use Icy Touch", "UseIcyTouch", "Offensive Spell");
            AddControlInWinForm("Use Plague Strike", "UsePlagueStrike", "Offensive Spell");
            AddControlInWinForm("Use Obliterate", "UseObliterate", "Offensive Spell");
            AddControlInWinForm("Use Soul Reaper", "UseSoulReaper", "Offensive Spell");
            AddControlInWinForm("Use Unholy Blight", "UseUnholyBlight", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use Blood Tap", "UseBloodTap", "Offensive Cooldown");
            AddControlInWinForm("Use Death Grip", "UseDeathGrip", "Offensive Cooldown");
            AddControlInWinForm("Use Empower Rune Weapon", "UseEmpowerRuneWeapon", "Offensive Cooldown");
            AddControlInWinForm("Use Outbreak", "UseOutbreak", "Offensive Cooldown");
            AddControlInWinForm("Use Pestilence", "UsePestilence", "Offensive Cooldown");
            AddControlInWinForm("Use Pillar of Frost", "UsePillarofFrost", "Offensive Cooldown");
            AddControlInWinForm("Use Raise Dead", "UseRaiseDead", "Offensive Cooldown");
            AddControlInWinForm("Use Summon Gargoyle", "UseSummonGargoyle", "Offensive Cooldown");
            AddControlInWinForm("Use Unholy Frenzy", "UseUnholyFrenzy", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Anti-Magic Shell", "UseAntiMagicShell", "Defensive Cooldown");
            AddControlInWinForm("Use Anti-Magic Zone", "UseAntiMagicZone", "Defensive Cooldown");
            AddControlInWinForm("Use Army of the Dead", "UseArmyoftheDead", "Defensive Cooldown");
            AddControlInWinForm("Use Asphyxiate", "UseAsphyxiate", "Defensive Cooldown");
            AddControlInWinForm("Use Chains of Ice", "UseChainsofIce", "Defensive Cooldown");
            AddControlInWinForm("Use Death's Advance", "UseDeathsAdvance", "Defensive Cooldown");
            AddControlInWinForm("Use Icebound Fortitude", "UseIceboundFortitude", "Defensive Cooldown");
            AddControlInWinForm("Use Mind Freeze", "UseMindFreeze", "Defensive Cooldown");
            AddControlInWinForm("Use Remorseless Winter", "UseRemorseless Winter", "Defensive Cooldown");
            AddControlInWinForm("Use Strangulate", "UseStrangulate", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Conversion", "UseConversion", "Healing Spell");
            AddControlInWinForm("Use Death Pact", "UseDeathPact", "Healing Spell");
            AddControlInWinForm("Use Death Siphon", "UseDeathSiphon", "Healing Spell");
            AddControlInWinForm("Use Death Strike", "UseDeathStrike", "Healing Spell");
            AddControlInWinForm("Use Lichborne", "UseLichborne", "Healing Spell");
            /* Game Settings */
            AddControlInWinForm("Use Low Combat Settings", "UseLowCombat", "Game Settings");
            AddControlInWinForm("Use Trinket", "UseTrinket", "Game Settings");
            AddControlInWinForm("Use Engineering Gloves", "UseEngGlove", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
            AddControlInWinForm("Use Duel Wield", "UseDuelWield", "Game Settings");
            AddControlInWinForm("Use Two Hander", "UseTwoHander", "Game Settings");
        }

        public static DeathknightFrostSettings CurrentSetting { get; set; }

        public static DeathknightFrostSettings GetSettings()
        {
            string CurrentSettingsFile = Application.StartupPath + "\\CustomClasses\\Settings\\Deathknight_Frost.xml";
            if (System.IO.File.Exists(CurrentSettingsFile))
            {
                return
                    CurrentSetting = Settings.Load<Deathknight_Frost.DeathknightFrostSettings>(CurrentSettingsFile);
            }
            else
            {
                return new Deathknight_Frost.DeathknightFrostSettings();
            }
        }
    }

    private readonly DeathknightFrostSettings MySettings = DeathknightFrostSettings.GetSettings();

    #region Professions & Racials

    private readonly Spell Arcane_Torrent = new Spell("Arcane Torrent");
    private readonly Spell Berserking = new Spell("Berserking");
    private readonly Spell Blood_Fury = new Spell("Blood Fury");
    private readonly Spell Engineering = new Spell("Engineering");
    private readonly Spell Lifeblood = new Spell("Lifeblood");
    private readonly Spell Stoneform = new Spell("Stoneform");
    private readonly Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    private readonly Spell War_Stomp = new Spell("War Stomp");

    #endregion

    #region Deathknight Presence & Buffs

    private readonly Spell Blood_Plague = new Spell("Blood Plague");
    private Timer Blood_Plague_Timer = new Timer(0);
    private readonly Spell Blood_Presence = new Spell("Blood Presence");
    private readonly Spell Freezing_Fog = new Spell(59052);
    private readonly Spell Frost_Fever = new Spell("Frost Fever");
    private Timer Frost_Fever_Timer = new Timer(0);
    private readonly Spell Frost_Presence = new Spell("Frost Presence");
    private readonly Spell Horn_of_Winter = new Spell("Horn of Winter");
    private readonly Spell Path_of_Frost = new Spell("Path of Frost");
    private Timer Path_of_Frost_Timer = new Timer(0);
    private readonly Spell Roiling_Blood = new Spell("Roiling Blood");
    private readonly Spell Unholy_Presence = new Spell("Unholy Presence");

    #endregion

    #region Offensive Spell

    private readonly Spell Blood_Boil = new Spell("Blood Boil");
    private readonly Spell Blood_Strike = new Spell("Blood Strike");
    private readonly Spell Death_and_Decay = new Spell("Death and Decay");
    private readonly Spell Death_Coil = new Spell("Death Coil");
    private readonly Spell Frost_Strike = new Spell("Frost Strike");
    private readonly Spell Howling_Blast = new Spell("Howling Blast");
    private readonly Spell Icy_Touch = new Spell("Icy Touch");
    private readonly Spell Plague_Strike = new Spell("Plague Strike");
    private readonly Spell Obliterate = new Spell("Obliterate");
    private readonly Spell Soul_Reaper = new Spell("Soul Reaper");
    private readonly Spell Unholy_Blight = new Spell("Unholy Blight");

    #endregion

    #region Offensive Cooldown

    private readonly Spell Blood_Tap = new Spell("Blood Tap");
    private readonly Spell Death_Grip = new Spell("Death Grip");
    private readonly Spell Empower_Rune_Weapon = new Spell("Empower Rune Weapon");
    private readonly Spell Pillar_of_Frost = new Spell("Pillar of Frost");
    private readonly Spell Outbreak = new Spell("Outbreak");
    private readonly Spell Pestilence = new Spell("Pestilence");
    private Timer Pestilence_Timer = new Timer(0);
    private readonly Spell Raise_Dead = new Spell("Raise Dead");

    #endregion

    #region Defensive Cooldown

    private readonly Spell AntiMagic_Shell = new Spell("Anti-Magic Shell");
    private readonly Spell AntiMagic_Zone = new Spell("Anti-Magic Zone");
    private readonly Spell Army_of_the_Dead = new Spell("Army of the Dead");
    private readonly Spell Asphyxiate = new Spell("Asphyxiate");
    private readonly Spell Chains_of_Ice = new Spell("Chains of Ice");
    private readonly Spell Deaths_Advance = new Spell("Death's Advance");
    private readonly Spell Icebound_Fortitude = new Spell("Icebound Fortitude");
    private readonly Spell Mind_Freeze = new Spell("Mind Freeze");
    private readonly Spell Remorseless_Winter = new Spell("Remorseless Winter");
    private readonly Spell Strangulate = new Spell("Strangulate");

    #endregion

    #region Healing Spell

    private readonly Spell Conversion = new Spell("Conversion");
    private readonly Spell Death_Pact = new Spell("Death Pact");
    private readonly Spell Death_Siphon = new Spell("Death Siphon");
    private readonly Spell Death_Strike = new Spell("Death Strike");
    private readonly Spell Lichborne = new Spell("Lichborne");

    #endregion

    private Timer Trinket_Timer = new Timer(0);
    private Timer Engineering_Timer = new Timer(0);
    private Timer AlchFlask_Timer = new Timer(0);
    private Timer OnCD = new Timer(0);
    public int LC = 0;

    public Deathknight_Frost()
    {
        Main.range = 5.0f;
        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            try
            {
                Buff_Path();
                if (!ObjectManager.Me.IsMounted)
                {
                    if (Fight.InFight && ObjectManager.Me.Target > 0)
                    {
                        if (ObjectManager.Me.Target != lastTarget &&
                            (Death_Grip.IsDistanceGood || Icy_Touch.IsDistanceGood))
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
        if (Death_Grip.IsSpellUsable && MySettings.UseDeathGrip && Death_Grip.IsDistanceGood)
            Grip();
        else
        {
            if (Icy_Touch.IsSpellUsable && MySettings.UseIcyTouch && Icy_Touch.IsDistanceGood)
                Icy_Touch.Launch();
        }
    }

    private void Buff_Path()
    {
        if (!Fight.InFight && Path_of_Frost.KnownSpell && Path_of_Frost.IsSpellUsable && MySettings.UsePathofFrost
            && (!Path_of_Frost.HaveBuff || Path_of_Frost_Timer.IsReady))
        {
            Path_of_Frost.Launch();
            Path_of_Frost_Timer = new Timer(1000*60*9);
        }
    }

    public void LowCombat()
    {
        Presence();
        AvoidMelee();
        Defense_Cycle();
        Heal();
        Decast();
        Buff();
        Grip();

        if (Howling_Blast.KnownSpell && Howling_Blast.IsSpellUsable && Howling_Blast.IsDistanceGood
            && MySettings.UseHowlingBlast)
        {
            Howling_Blast.Launch();
            if (ObjectManager.Target.HealthPercent < 50 && ObjectManager.Target.HealthPercent > 0)
            {
                Howling_Blast.Launch();
                return;
            }
            return;
        }
        else if (Death_Coil.KnownSpell && Death_Coil.IsSpellUsable && Death_Coil.IsDistanceGood
            && MySettings.UseDeathCoil)
        {
            Death_Coil.Launch();
            return;
        }
        else
        {
            if (Blood_Boil.KnownSpell && Blood_Boil.IsSpellUsable && Blood_Boil.IsDistanceGood
                && MySettings.UseBloodBoil)
            {
                Blood_Boil.Launch();
                return;
            }
        }
    }

    public void Combat()
    {
        Presence();
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
        else 
        {
            if (Pillar_of_Frost.IsSpellUsable && Pillar_of_Frost.KnownSpell && MySettings.UsePillarofFrost
                && ObjectManager.Target.GetDistance < 30)
            {
                Pillar_of_Frost.Launch();
                return;
            }
        }
    }

    public void DPS_Cycle()
    {
        Grip();

        if (ObjectManager.Me.HealthPercent < 85 && MySettings.UseDeathCoil
            && Lichborne.HaveBuff && Death_Coil.KnownSpell && Death_Coil.IsSpellUsable)
        {
            Lua.RunMacroText("/target Player");
            Death_Coil.Launch();
            return;
        }
        else if (Unholy_Blight.KnownSpell && Unholy_Blight.IsSpellUsable && ObjectManager.Target.GetDistance < 9
            && MySettings.UseUnholyBlight)
        {
            Unholy_Blight.Launch();
            Blood_Plague_Timer = new Timer(1000*27);
            Frost_Fever_Timer = new Timer(1000*27);
            return;
        }
        else if (Outbreak.KnownSpell && Outbreak.IsSpellUsable && Outbreak.IsDistanceGood && MySettings.UseOutbreak
            && (Blood_Plague_Timer.IsReady || Frost_Fever_Timer.IsReady || !Blood_Plague.TargetHaveBuff || !Frost_Fever.TargetHaveBuff))
        {
            Outbreak.Launch();
            Blood_Plague_Timer = new Timer(1000*27);
            Frost_Fever_Timer = new Timer(1000*27);
            return;
        }
        else if (Blood_Plague_Timer.IsReady && Frost_Fever_Timer.IsReady && Blood_Plague.TargetHaveBuff && Frost_Fever.TargetHaveBuff
            && Blood_Boil.IsSpellUsable && Blood_Boil.KnownSpell && ObjectManager.Target.GetDistance < 9
            && MySettings.UseBloodBoil && Roiling_Blood.KnownSpell)
        {
            Blood_Boil.Launch();
            Blood_Plague_Timer = new Timer(1000*27);
            Frost_Fever_Timer = new Timer(1000*27);
            return;
        }
        else if (Plague_Strike.KnownSpell && Plague_Strike.IsDistanceGood && !Outbreak.IsSpellUsable
            && !Unholy_Blight.IsSpellUsable && MySettings.UsePlagueStrike
            && (Blood_Plague_Timer.IsReady || !Blood_Plague.TargetHaveBuff))
        {
            if (!Plague_Strike.IsSpellUsable && Blood_Tap.KnownSpell && Blood_Tap.IsSpellUsable
                && MySettings.UseBloodTap)
            {
                Blood_Tap.Launch();
                Thread.Sleep(200);
            }

            if (Plague_Strike.IsSpellUsable)
            {
                Plague_Strike.Launch();
                Blood_Plague_Timer = new Timer(1000*27);
                return;
            }
        }
        else if (Icy_Touch.KnownSpell && Howling_Blast.IsDistanceGood
            && !Outbreak.IsSpellUsable && !Unholy_Blight.IsSpellUsable
            && (Frost_Fever_Timer.IsReady || !Frost_Fever.TargetHaveBuff))
        {
            if (!Howling_Blast.IsSpellUsable && Blood_Tap.KnownSpell && Blood_Tap.IsSpellUsable
                && MySettings.UseBloodTap)
            {
                Blood_Tap.Launch();
                Thread.Sleep(200);
            }

            if (Howling_Blast.IsSpellUsable && MySettings.UseHowlingBlast)
            {
                Howling_Blast.Launch();
                Frost_Fever_Timer = new Timer(1000*27);
                return;
            }

            else
            {
                if (Icy_Touch.IsSpellUsable && MySettings.UseIcyTouch)
                {
                    Icy_Touch.Launch();
                    Frost_Fever_Timer = new Timer(1000*27);
                    return;
                }
            }
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 2 && Pestilence.IsSpellUsable && Pestilence.IsDistanceGood
            && Pestilence.KnownSpell && MySettings.UsePestilence && !Roiling_Blood.KnownSpell)
        {
            Pestilence.Launch();
            return;
        }
        else if (Freezing_Fog.HaveBuff && Howling_Blast.KnownSpell && Howling_Blast.IsDistanceGood
            && MySettings.UseHowlingBlast)
        {
            Howling_Blast.Launch();
            Frost_Fever_Timer = new Timer(1000*27);
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 2 && MySettings.UseDeathandDecay
            && Death_and_Decay.KnownSpell && Death_and_Decay.IsSpellUsable && Death_and_Decay.IsDistanceGood)
        {
            SpellManager.CastSpellByIDAndPosition(43265, ObjectManager.Target.Position);
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 4 && Army_of_the_Dead.IsSpellUsable && Army_of_the_Dead.IsDistanceGood
            && Army_of_the_Dead.KnownSpell && MySettings.UseArmyoftheDead)
        {
            Army_of_the_Dead.Launch();
            Thread.Sleep(4000);
            return;
        }
        // Blizzard API Calls for Frost Strike using Blood Strike Function
        else if (ObjectManager.Me.RunicPowerPercentage >= 90 && MySettings.UseFrostStrike
            && Blood_Strike.KnownSpell && Blood_Strike.IsSpellUsable && Blood_Strike.IsDistanceGood)
        {
            Blood_Strike.Launch();
            return;
        }
        else 
        {
            if (Soul_Reaper.KnownSpell && Soul_Reaper.IsDistanceGood && Soul_Reaper.IsSpellUsable
            && ObjectManager.Target.HealthPercent < 35 && ObjectManager.Me.HealthPercent > 80
            && MySettings.UseSoulReaper)
            {
                Soul_Reaper.Launch();
                return;
            }
        }

        if (MySettings.UseDuelWield)
        {
            if (ObjectManager.Me.HaveBuff(51124) && MySettings.UseFrostStrike
                && Blood_Strike.KnownSpell && Blood_Strike.IsSpellUsable && Blood_Strike.IsDistanceGood)
            {
                if (ObjectManager.Me.HealthPercent < 80 && ((Lichborne.KnownSpell && MySettings.UseLichborne)
                    || (Conversion.KnownSpell && MySettings.UseConversion)))
                    return;
                else
                {
                    Blood_Strike.Launch();
                    return;
                }
            }

            else
            {
                if (ObjectManager.Me.HaveBuff(51124) && Obliterate.KnownSpell && Obliterate.IsSpellUsable && Obliterate.IsDistanceGood
                    && MySettings.UseObliterate)
                {
                    if (ObjectManager.Me.HealthPercent < 80 && MySettings.UseDeathStrike
                        && Death_Strike.KnownSpell && Death_Strike.IsSpellUsable && Death_Strike.IsDistanceGood)
                    {
                        Death_Strike.Launch();
                        return;
                    }
                    Obliterate.Launch();
                    return;
                }
            }
        }
        else 
        {
            if (MySettings.UseTwoHander)
            {
                if (ObjectManager.Me.HaveBuff(51124) && Obliterate.KnownSpell && Obliterate.IsSpellUsable && Obliterate.IsDistanceGood
                    && MySettings.UseObliterate)
                {
                    if (ObjectManager.Me.HealthPercent < 80 && MySettings.UseDeathStrike
                        && Death_Strike.KnownSpell && Death_Strike.IsSpellUsable && Death_Strike.IsDistanceGood)
                    {
                        Death_Strike.Launch();
                        return;
                    }
                    Obliterate.Launch();
                    return;
                }

                else
                {
                    if (ObjectManager.Me.HaveBuff(51124) && MySettings.UseFrostStrike
                        && Blood_Strike.KnownSpell && Blood_Strike.IsSpellUsable && Blood_Strike.IsDistanceGood)
                    {
                        if (ObjectManager.Me.HealthPercent < 80 && ((Lichborne.KnownSpell && MySettings.UseLichborne)
                            || (Conversion.KnownSpell && MySettings.UseConversion)))
                            return;
                        else
                        {
                            Blood_Strike.Launch();
                            return;
                        }
                    }
                }
            }
        }

        if (Obliterate.KnownSpell && Obliterate.IsSpellUsable && Obliterate.IsDistanceGood
            && MySettings.UseObliterate)
        {
            if (ObjectManager.Me.HealthPercent < 80 && MySettings.UseDeathStrike
                && Death_Strike.KnownSpell && Death_Strike.IsSpellUsable && Death_Strike.IsDistanceGood)
            {
                Death_Strike.Launch();
                return;
            }
            Obliterate.Launch();
            return;
        }
        else if (Blood_Strike.KnownSpell && Blood_Strike.IsSpellUsable && Blood_Strike.IsDistanceGood
            && MySettings.UseFrostStrike)
        {
            if (ObjectManager.Me.HealthPercent < 80 && ((Lichborne.KnownSpell && MySettings.UseLichborne)
                || (Conversion.KnownSpell && MySettings.UseConversion)))
                return;
            else
            {
                Blood_Strike.Launch();
                return;
            }
        }
        else if (Horn_of_Winter.KnownSpell && Horn_of_Winter.IsSpellUsable && MySettings.UseHornofWinter)
        {
            Horn_of_Winter.Launch();
            return;
        }
        else if (Empower_Rune_Weapon.IsSpellUsable && Empower_Rune_Weapon.KnownSpell && MySettings.UseEmpowerRuneWeapon)
        {
            Empower_Rune_Weapon.Launch();
            return;
        }
        else
        {
            if (Arcane_Torrent.IsSpellUsable && Arcane_Torrent.KnownSpell && MySettings.UseArcaneTorrent)
            {
                Arcane_Torrent.Launch();
                return;
            }
        }
    }

    public void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            Presence();
            Heal();
            Buff();
        }
    }

    private void Presence()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (Unholy_Presence.KnownSpell && MySettings.UseUnholyPresence && MySettings.UseLowCombat)
        {
            if (!Unholy_Presence.HaveBuff && Unholy_Presence.IsSpellUsable && LC == 1)
                Unholy_Presence.Launch();
        }
        else if (Blood_Presence.KnownSpell && MySettings.UseBloodPresence && ObjectManager.Me.HealthPercent < 30)
        {
            if (!Blood_Presence.HaveBuff && Blood_Presence.IsSpellUsable)
                Blood_Presence.Launch();
        }
        else
        {
            if (Frost_Presence.KnownSpell && MySettings.UseFrostPresence && ObjectManager.Me.HealthPercent > 50)
            {
                if (!Frost_Presence.HaveBuff && Frost_Presence.IsSpellUsable)
                    Frost_Presence.Launch();
            }
        }
    }

    private void Grip()
    {
        if (ObjectManager.Target.GetDistance > 5 &&
            Death_Grip.KnownSpell && Death_Grip.IsSpellUsable && Death_Grip.IsDistanceGood
            && MySettings.UseDeathGrip)
        {
            Death_Grip.Launch();
            MovementManager.StopMove();
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (ObjectManager.Me.HealthPercent < 80 && Gift_of_the_Naaru.IsSpellUsable && Gift_of_the_Naaru.KnownSpell
            && MySettings.UseGiftoftheNaaru)
        {
            Gift_of_the_Naaru.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 55 && MySettings.UseDeathPact
            && Raise_Dead.KnownSpell && Raise_Dead.IsSpellUsable
            && Death_Pact.KnownSpell && Death_Pact.IsSpellUsable)
        {
            int i = 0;
            while (i < 3)
            {
                i++;
                Raise_Dead.Launch();
                Death_Pact.Launch();

                if (!Death_Pact.IsSpellUsable)
                {
                    break;
                }
            }
        }
        else if (ObjectManager.Me.HealthPercent < 45 && MySettings.UseLichborne
            && Lichborne.KnownSpell && Lichborne.IsSpellUsable
            && Death_Coil.KnownSpell && Death_Coil.IsSpellUsable)
        {
            if (Lichborne.IsSpellUsable)
            {
                Lichborne.Launch();
                return;
            }
        }
        else if (ObjectManager.Me.HealthPercent < 45 && MySettings.UseConversion
            && Conversion.KnownSpell && Conversion.IsSpellUsable
            && ObjectManager.Me.RunicPower > 10)
        {
            if (Conversion.IsSpellUsable)
            {
                Conversion.Launch();
                while (ObjectManager.Me.RunicPower > 0)
                    Thread.Sleep(200);
                return;
            }
        }
        else
        {
            if (ObjectManager.Me.HealthPercent < 80 && Death_Siphon.KnownSpell && Death_Siphon.IsSpellUsable
                && MySettings.UseDeathSiphon)
            {
                Death_Siphon.Launch();
                return;
            }
        }
    }

    private void Defense_Cycle()
    {
        if (ObjectManager.Me.HealthPercent < 80 && Stoneform.IsSpellUsable && Stoneform.KnownSpell)
        {
            Stoneform.Launch();
            OnCD = new Timer(1000*8);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && MySettings.UseIceboundFortitude
            && Icebound_Fortitude.KnownSpell && Icebound_Fortitude.IsSpellUsable)
        {
            Icebound_Fortitude.Launch();
            OnCD = new Timer(1000*12);
            return;
        }
        else
        {
            if (ObjectManager.Target.GetDistance < 8 && MySettings.UseRemorselessWinter
                && (ObjectManager.Me.HealthPercent < 70 || ObjectManager.GetNumberAttackPlayer() > 1))
            {
                if (Remorseless_Winter.KnownSpell && Remorseless_Winter.IsSpellUsable)
                {
                    Remorseless_Winter.Launch();
                    OnCD = new Timer(1000*8);
                    return;
                }
            }
        }
    }

    private void Decast()
    {
        if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && MySettings.UseMindFreeze
            && Mind_Freeze.KnownSpell && Mind_Freeze.IsSpellUsable && Mind_Freeze.IsDistanceGood)
        {
            Mind_Freeze.Launch();
            return;
        }
        else if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && MySettings.UseAntiMagicShell
            && AntiMagic_Shell.KnownSpell && AntiMagic_Shell.IsSpellUsable)
        {
            AntiMagic_Shell.Launch();
            return;
        }
        else if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe
            && Strangulate.KnownSpell && Strangulate.IsSpellUsable && Strangulate.IsDistanceGood
            && (MySettings.UseStrangulate || MySettings.UseAsphyxiate))
        {
            Strangulate.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && AntiMagic_Zone.KnownSpell
                && MySettings.UseAntiMagicZone && AntiMagic_Zone.IsSpellUsable)
            {
                SpellManager.CastSpellByIDAndPosition(51052, ObjectManager.Me.Position);
                return;
            }
        }

        if (ObjectManager.Target.GetMove && !Chains_of_Ice.TargetHaveBuff && MySettings.UseChainsofIce
            && Chains_of_Ice.KnownSpell && Chains_of_Ice.IsSpellUsable && Chains_of_Ice.IsDistanceGood)
        {
            Chains_of_Ice.Launch();
            return;
        }
    }

    private void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (!Horn_of_Winter.HaveBuff && Horn_of_Winter.KnownSpell && Horn_of_Winter.IsSpellUsable
            && MySettings.UseHornofWinter)
        {
            Horn_of_Winter.Launch();
            return;
        }
        else if (!Fight.InFight && Deaths_Advance.KnownSpell && Deaths_Advance.IsSpellUsable
            && MySettings.UseDeathsAdvance)
        {
            Deaths_Advance.Launch();
            return;
        }
        else
        {
            if (AlchFlask_Timer.IsReady && MySettings.UseAlchFlask
            && ItemsManager.GetItemCountByIdLUA(75525) == 1)
            {
                Logging.WriteFight("Use Alchi Flask");
                Lua.RunMacroText("/use item:75525");
                AlchFlask_Timer = new Timer(1000*60*60*2);
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