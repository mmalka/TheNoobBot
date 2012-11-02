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
                    #region Mage Specialisation checking

                case WoWClass.Mage:
                    var Mage_Arcane_Spell = new Spell("Arcane Blast");
                    var Mage_Fire_Spell = new Spell("Pyroblast");
                    var Mage_Frost_Spell = new Spell("Summon Water Elemental");

                    if (Mage_Arcane_Spell.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Mage_Arcane.xml";
                            Mage_Arcane.MageArcaneSettings CurrentSetting;
                            CurrentSetting = new Mage_Arcane.MageArcaneSettings();
                            if (System.IO.File.Exists(CurrentSettingsFile))
                            {
                                CurrentSetting =
                                    Settings.Load<Mage_Arcane.MageArcaneSettings>(CurrentSettingsFile);
                            }
                            CurrentSetting.ToForm();
                            CurrentSetting.Save(CurrentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Mage Arcane class...");
                            new Mage_Arcane();
                        }
                        break;
                    }

                    else if (Mage_Fire_Spell.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Mage_Fire.xml";
                            Mage_Fire.MageFireSettings CurrentSetting;
                            CurrentSetting = new Mage_Fire.MageFireSettings();
                            if (System.IO.File.Exists(CurrentSettingsFile))
                            {
                                CurrentSetting =
                                    Settings.Load<Mage_Fire.MageFireSettings>(CurrentSettingsFile);
                            }
                            CurrentSetting.ToForm();
                            CurrentSetting.Save(CurrentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Mage Fire class...");
                            new Mage_Fire();
                        }
                        break;
                    }

                    else if (Mage_Frost_Spell.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Mage_Frost.xml";
                            Mage_Frost.MageFrostSettings CurrentSetting;
                            CurrentSetting = new Mage_Frost.MageFrostSettings();
                            if (System.IO.File.Exists(CurrentSettingsFile))
                            {
                                CurrentSetting =
                                    Settings.Load<Mage_Frost.MageFrostSettings>(CurrentSettingsFile);
                            }
                            CurrentSetting.ToForm();
                            CurrentSetting.Save(CurrentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Mage Frost class...");
                            new Mage_Frost();
                        }
                        break;
                    }

                    else
                    {
                        if (ConfigOnly)
                        {
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Mage_Frost.xml";
                            Mage_Frost.MageFrostSettings CurrentSetting;
                            CurrentSetting = new Mage_Frost.MageFrostSettings();
                            if (System.IO.File.Exists(CurrentSettingsFile))
                            {
                                CurrentSetting =
                                    Settings.Load<Mage_Frost.MageFrostSettings>(CurrentSettingsFile);
                            }
                            CurrentSetting.ToForm();
                            CurrentSetting.Save(CurrentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Mage without Spec");
                            Logging.WriteFight("Loading Mage Frost class...");
                            new Mage_Frost();
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

#region Mage

public class Mage_Arcane
{
    [Serializable]
    public class MageArcaneSettings : nManager.Helpful.Settings
    {
        /* Professions & Racials */
        public bool UseArcaneTorrent = true;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseLifeblood = true;
        public bool UseStoneform = true;
        public bool UseGiftoftheNaaru = true;
        public bool UseWarStomp = true;
        /* Mage Buffs */
        public bool UseArcaneBrilliance = true;
        public bool UseBlazingSpeed = true;
        public bool UseFrostArmor = false;
        public bool UseIceFloes = true;
        public bool UseMageArmor = true;
        public bool UseMoltenArmor = false;
        /* Offensive Spell */
        public bool UseArcaneBarrage = true;
        public bool UseArcaneBlast = true;
        public bool UseArcaneExplosion = true;
        public bool UseArcaneMissiles = true;
        public bool UseFlamestrike = true;
        public bool UseScorch = true;
        /* Offensive Cooldown */
        public bool UseAlterTime = true;
        public bool UseArcanePower = true;
        public bool UseMirrorImage = true;
        public bool UsePresenceofMind = true;
        public bool UseTierFive = true;
        public bool UseTimeWarp = true;
        /* Defensive Cooldown */
        public bool UseBlink = true;
        public bool UseColdSnap = true;
        public bool UseConeofCold = true;
        public bool UseCounterspell = true;
        public bool UseDeepFreeze = true;
        public bool UseFrostJaw = true;
        public bool UseFrostNova = true;
        public bool UseIceBarrier = true;
        public bool UseIceBlock = true;
        public bool UseIceWard = true;
        public bool UseIncantersWard = true;
        public bool UseInvisibility = true;
        public bool UseRingofFrost = true;
        public bool UseSlow = false;
        public bool UseTemporalShield  = true;
        /* Healing Spell */
        public bool UseConjureManaGem = true;
        public bool UseConjureRefreshment = true;
        public bool UseEvocation = true;
        /* Game Settings */
        public bool UseLowCombat = true;
        public bool UseTrinket = true;
        public bool UseEngGlove = true;
        public bool UseAlchFlask = true;
        public bool UseEvocationGlyph = false;
        public bool UseInvocationTalent = false;
        public bool UseRuneofPowerTalent = false;

        public MageArcaneSettings()
        {
            ConfigWinForm(new System.Drawing.Point(400, 400), "Mage Arcane Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");
            AddControlInWinForm("Use Lifeblood", "UseLifeblood", "Professions & Racials");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            /* Mage Buffs */
            AddControlInWinForm("Use Arcane Brilliance", "UseArcaneBrilliance", "Mage Buffs");
            AddControlInWinForm("Use Blazing Speed", "UseBlazingSpeed", "Mage Buffs");
            AddControlInWinForm("Use Frost Armor", "UseFrostArmor", "Mage Buffs");
            AddControlInWinForm("Use Ice Floes", "UseIceFloes", "Mage Buffs");
            AddControlInWinForm("Use Mage Armor", "UseMageArmor", "Mage Buffs");
            AddControlInWinForm("Use Molten Armor", "UseMoltenArmor", "Mage Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Arcane Barrage", "UseArcaneBarrage", "Offensive Spell");
            AddControlInWinForm("Use Arcane Blast", "UseArcaneBlast", "Offensive Spell");
            AddControlInWinForm("Use Arcane Explosion", "UseArcaneExplosion", "Offensive Spell");
            AddControlInWinForm("Use Arcane Missiles", "UseArcaneMissiles", "Offensive Spell");
            AddControlInWinForm("Use Flamestrike", "UseFlamestrike", "Offensive Spell");
            AddControlInWinForm("Use Scorch", "UseScorch", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use Alter Time", "UseAlterTime", "Offensive Cooldown");
            AddControlInWinForm("Use Arcane Power", "UseArcanePower", "Offensive Cooldown");
            AddControlInWinForm("Use Mirror Image", "UseMirrorImage", "Offensive Cooldown");
            AddControlInWinForm("Use Presence of Mind", "UsePresenceofMind", "Offensive Cooldown");
            AddControlInWinForm("Use Tier Five Talent", "UseTierFive", "Offensive Cooldown");
            AddControlInWinForm("Use Time Warp", "UseTimeWarp", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Blink", "UseBlink", "Defensive Cooldown");
            AddControlInWinForm("Use Cold Snap", "UseColdSnap", "Defensive Cooldown");
            AddControlInWinForm("Use Cone of Cold", "UseConeofCold", "Defensive Cooldown");
            AddControlInWinForm("Use Counterspell", "UseCounterspell", "Defensive Cooldown");
            AddControlInWinForm("Use Deep Freeze", "UseDeepFreeze", "Defensive Cooldown");
            AddControlInWinForm("Use Frost Jaw", "UseFrostJaw", "Defensive Cooldown");
            AddControlInWinForm("Use Frost Nova", "UseFrostNova", "Defensive Cooldown");
            AddControlInWinForm("Use Ice Barrier", "UseIceBarrier", "Defensive Cooldown");
            AddControlInWinForm("Use Ice Block", "UseIceBlock", "Defensive Cooldown");
            AddControlInWinForm("Use Ice Ward", "UseIceWard", "Defensive Cooldown");
            AddControlInWinForm("Use Incanter's Ward", "UseIncantersWard", "Defensive Cooldown");
            AddControlInWinForm("Use Invisibility", "UseInvisibility", "Defensive Cooldown");
            AddControlInWinForm("Use Ring of Frost", "UseRingofFrost", "Defensive Cooldown");
            AddControlInWinForm("Use Slow", "UseSlow", "Defensive Cooldown");
            AddControlInWinForm("Use Temporal Shield", "UseTemporalShield", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Conjure Mana Gem", "UseConjureManaGem", "Healing Spell");
            AddControlInWinForm("Use Conjure Refreshment", "UseConjureRefreshment", "Healing Spell");
            AddControlInWinForm("Use Evocation", "UseEvocation", "Healing Spell");
            /* Game Settings */
            AddControlInWinForm("Use Low Combat Settings", "UseLowCombat", "Game Settings");
            AddControlInWinForm("Use Trinket", "UseTrinket", "Game Settings");
            AddControlInWinForm("Use Engineering Gloves", "UseEngGlove", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
            AddControlInWinForm("Use Evocation Glyph", "UseEvocationGlyph", "Game Settings");
            AddControlInWinForm("Use Invocation Talent", "UseInvocationTalent", "Game Settings");
            AddControlInWinForm("Use Rune of Power Talent", "UseRuneofPowerTalent", "Game Settings");
        }

        public static MageArcaneSettings CurrentSetting { get; set; }

        public static MageArcaneSettings GetSettings()
        {
            string CurrentSettingsFile = Application.StartupPath + "\\CustomClasses\\Settings\\Mage_Arcane.xml";
            if (System.IO.File.Exists(CurrentSettingsFile))
            {
                return
                    CurrentSetting = Settings.Load<Mage_Arcane.MageArcaneSettings>(CurrentSettingsFile);
            }
            else
            {
                return new Mage_Arcane.MageArcaneSettings();
            }
        }
    }

    private readonly MageArcaneSettings MySettings = MageArcaneSettings.GetSettings();

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

    #region Mage Buffs

    private readonly Spell Arcane_Brilliance = new Spell("Arcane Brilliance");
    private readonly Spell Blazing_Speed = new Spell("Blazing Speed");
    private readonly Spell Frost_Armor = new Spell("Frost Armor");
    private readonly Spell Ice_Floes = new Spell("Ice Floes");
    private readonly Spell Mage_Armor = new Spell("Mage Armor");
    private readonly Spell Molten_Armor = new Spell("Molten Armor");

    #endregion

    #region Offensive Spell

    private readonly Spell Arcane_Barrage = new Spell("Arcane Barrage");
    private readonly Spell Arcane_Blast = new Spell("Arcane Blast");
    private readonly Spell Arcane_Explosion = new Spell("Arcane Explosion");
    private readonly Spell Arcane_Missiles = new Spell("Arcane Missiles");
    private readonly Spell Flamestrike = new Spell("Flamestrike");
    private Timer Flamestrike_Timer = new Timer(0);
    private readonly Spell Scorch = new Spell("Scorch");

    #endregion

    #region Offensive Cooldown

    private readonly Spell Alter_Time = new Spell("Alter Time");
    private readonly Spell Arcane_Power = new Spell("Arcane Power");
    private readonly Spell Frozen_Orb = new Spell("Frozen Orb");
    private readonly Spell Mage_Bomb = new Spell("Mage Bomb");
    private readonly Spell Mirror_Image = new Spell("Mirror Image");
    private readonly Spell Presence_of_Mind = new Spell("Presence of Mind");
    private readonly Spell Time_Warp = new Spell("Time Warp");

    #endregion

    #region Defensive Cooldown

    private readonly Spell Blink = new Spell("Blink");
    private readonly Spell Cold_Snap = new Spell("Cold Snap");
    private readonly Spell Cone_of_Cold = new Spell("Cone of Cold");
    private readonly Spell Counterspell = new Spell("Counterspell");
    private readonly Spell Deep_Freeze = new Spell("Deep Freeze");
    private readonly Spell Frost_Jaw = new Spell("Frost Jaw");
    private readonly Spell Frost_Nova = new Spell("Frost Nova");
    private readonly Spell Ice_Barrier = new Spell("Ice Barrier");
    private readonly Spell Ice_Block = new Spell("Ice Block");
    private readonly Spell Ice_Ward = new Spell("Ice Ward");
    private readonly Spell Incanters_Ward = new Spell("Incanter's Ward");
    private readonly Spell Invisibility = new Spell("Invisibility");
    private readonly Spell Ring_of_Frost = new Spell("Ring of Frost");
    private readonly Spell Slow = new Spell("Slow");
    private readonly Spell Temporal_Shield = new Spell("Temporal Shield");

    #endregion

    #region Healing Spell

    private readonly Spell Conjure_Mana_Gem = new Spell("Conjure Mana Gem");
    private readonly Spell Conjure_Refreshment = new Spell("Conjure Refreshment");
    private readonly Spell Evocation = new Spell("Evocation");

    #endregion

    private Timer OnCD = new Timer(0);
    private Timer Trinket_Timer = new Timer(0);
    private Timer Engineering_Timer = new Timer(0);
    private Timer AlchFlask_Timer = new Timer(0);
    private Timer Steady_Focus_Timer = new Timer(0);
    public int LC = 0;

    public Mage_Arcane()
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
                            (Arcane_Barrage.IsDistanceGood || Scorch.IsDistanceGood))
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
        if (Scorch.IsDistanceGood && Scorch.KnownSpell && Scorch.IsSpellUsable && MySettings.UseScorch)
            Scorch.Launch();
        else
        {
            if (Arcane_Barrage.IsDistanceGood && Arcane_Barrage.KnownSpell && Arcane_Barrage.IsSpellUsable 
                && MySettings.UseArcaneBarrage)
                Arcane_Barrage.Launch();
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

        if (Arcane_Barrage.IsDistanceGood && Arcane_Barrage.KnownSpell && Arcane_Barrage.IsSpellUsable
            && MySettings.UseArcaneBarrage)
        {
            Arcane_Barrage.Launch();
            return;
        }
        else
        {
            if (Arcane_Blast.KnownSpell && Arcane_Blast.IsSpellUsable && Arcane_Blast.IsDistanceGood
                && MySettings.UseArcaneBlast)
            {
                Arcane_Blast.Launch();
                return;
            }
        }

        if (Arcane_Explosion.KnownSpell && Arcane_Explosion.IsSpellUsable && Arcane_Explosion.IsDistanceGood
            && MySettings.UseArcaneExplosion)
        {
            Arcane_Explosion.Launch();
            return;
        }
    }

    public void DPS_Burst()
    {
        if (Alter_Time.IsSpellUsable && Alter_Time.KnownSpell && MySettings.UseAlterTime
            && ObjectManager.Target.GetDistance < 30 && ObjectManager.Target.InCombat)
            Alter_Time.Launch();

        if (MySettings.UseTrinket && Trinket_Timer.IsReady && ObjectManager.Target.GetDistance < 40)
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
            && ObjectManager.Target.GetDistance < 40)
        {
            Berserking.Launch();
            return;
        }
        else if (Blood_Fury.IsSpellUsable && Blood_Fury.KnownSpell && MySettings.UseBloodFury
            && ObjectManager.Target.GetDistance < 40)
        {
            Blood_Fury.Launch();
            return;
        }
        else if (Lifeblood.IsSpellUsable && Lifeblood.KnownSpell && MySettings.UseLifeblood
            && ObjectManager.Target.GetDistance < 40)
        {
            Lifeblood.Launch();
            return;
        }
        else if (MySettings.UseEngGlove && Engineering.KnownSpell && Engineering_Timer.IsReady
            && ObjectManager.Target.GetDistance < 40)
        {
            Logging.WriteFight("Use Engineering Gloves.");
            Lua.RunMacroText("/use 10");
            Engineering_Timer = new Timer(1000*60);
            return;
        }
        else if (Mage_Bomb.KnownSpell && Mage_Bomb.IsSpellUsable && Mage_Bomb.IsDistanceGood
            && MySettings.UseTierFive)
        {
            Mage_Bomb.Launch();
            return;
        }
        else if (Evocation.KnownSpell && Evocation.IsSpellUsable && ObjectManager.Target.GetDistance < 40
            && MySettings.UseInvocationTalent && !ObjectManager.Me.HaveBuff(114003))
        {
            Evocation.Launch();
            return;
        }
        else if (Evocation.KnownSpell && Evocation.IsSpellUsable && ObjectManager.Target.GetDistance < 40
            && MySettings.UseRuneofPowerTalent && !ObjectManager.Me.HaveBuff(116011))
        {
            SpellManager.CastSpellByIDAndPosition(116011, ObjectManager.Target.Position);
            return;
        }
        else if (Arcane_Power.KnownSpell && Arcane_Power.IsSpellUsable && ObjectManager.Target.GetDistance < 40
            && MySettings.UseArcanePower)
        {
            Arcane_Power.Launch();
            return;
        }
        else if (Mirror_Image.KnownSpell && Mirror_Image.IsSpellUsable && ObjectManager.Target.GetDistance < 40
            && MySettings.UseMirrorImage)
        {
            Mirror_Image.Launch();
            return;
        }
        else
        {
            if (Time_Warp.KnownSpell && Time_Warp.IsSpellUsable && MySettings.UseTimeWarp
                && !ObjectManager.Me.HaveBuff(80354) && ObjectManager.Target.GetDistance < 40)
            {
                Time_Warp.Launch();
                return;
            }
        }
    }

    public void DPS_Cycle()
    {
        if (Scorch.IsSpellUsable && MySettings.UseScorch && Scorch.IsDistanceGood
            && Scorch.KnownSpell && ObjectManager.Me.GetMove && !Ice_Floes.HaveBuff)
        {
            Scorch.Launch();
            return;
        }

        if (ObjectManager.GetNumberAttackPlayer() > 1 && Flamestrike.IsSpellUsable && Flamestrike.KnownSpell
            && Flamestrike.IsDistanceGood && Flamestrike_Timer.IsReady && MySettings.UseFlamestrike)
        {
            SpellManager.CastSpellByIDAndPosition(2120, ObjectManager.Target.Position);
            Flamestrike_Timer = new Timer(1000*8);
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 4 && Arcane_Explosion.IsSpellUsable && Arcane_Explosion.KnownSpell
            && Arcane_Explosion.IsDistanceGood && MySettings.UseArcaneExplosion)
        {
            Arcane_Explosion.Launch();
            return;
        }
        else if (Arcane_Missiles.KnownSpell && Arcane_Missiles.IsSpellUsable && Arcane_Missiles.IsDistanceGood
            && MySettings.UseArcaneMissiles)
        {
            Arcane_Missiles.Launch();
            return;
        }
        else if (Arcane_Barrage.IsDistanceGood && Arcane_Barrage.KnownSpell && Arcane_Barrage.IsSpellUsable
            && ObjectManager.Me.BuffStack(114664) > 3 && MySettings.UseArcaneBarrage)
        {
            Arcane_Barrage.Launch();
            return;
        }
        else if (Arcane_Blast.KnownSpell && Arcane_Blast.IsSpellUsable && Arcane_Blast.IsDistanceGood
            && Presence_of_Mind.KnownSpell && Presence_of_Mind.IsSpellUsable
            && ObjectManager.Me.BuffStack(114664) < 6)
        {
            Presence_of_Mind.Launch();
            Thread.Sleep(400);
            Arcane_Blast.Launch();
            return;
        }
        else
        {
            if (Arcane_Blast.KnownSpell && Arcane_Blast.IsSpellUsable && Arcane_Blast.IsDistanceGood
                && MySettings.UseArcaneBlast && ObjectManager.Me.BuffStack(114664) < 6)
            {
                Arcane_Blast.Launch();
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

        if (ObjectManager.Me.HaveBuff(87023) && Ice_Block.KnownSpell && MySettings.UseIceBlock
            && !ObjectManager.Me.HaveBuff(41425))
        {
            if (!Ice_Block.IsSpellUsable && Cold_Snap.KnownSpell && Cold_Snap.IsSpellUsable
                && MySettings.UseColdSnap)
            {
                Cold_Snap.Launch();
                Thread.Sleep(400);
            }
            Ice_Block.Launch();
            OnCD = new Timer(1000*10);
            return;
        }

        if (MySettings.UseArcaneBrilliance && Arcane_Brilliance.KnownSpell && Arcane_Brilliance.IsSpellUsable 
            && !Arcane_Brilliance.HaveBuff && !ObjectManager.Me.HaveBuff(61316))
        {
            Arcane_Brilliance.Launch();
            return;
        }
        else if (MySettings.UseMageArmor && Mage_Armor.KnownSpell && Mage_Armor.IsSpellUsable 
            && !Mage_Armor.HaveBuff)
        {
            Mage_Armor.Launch();
            return;
        }
        else if (MySettings.UseFrostArmor && Frost_Armor.KnownSpell && Frost_Armor.IsSpellUsable 
            && !Frost_Armor.HaveBuff && !MySettings.UseMageArmor)
        {
            Frost_Armor.Launch();
            return;
        }
        else if (MySettings.UseMoltenArmor && Molten_Armor.KnownSpell && Molten_Armor.IsSpellUsable 
            && !Molten_Armor.HaveBuff && !MySettings.UseFrostArmor && !MySettings.UseMageArmor)
        {
            Molten_Armor.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 0 && Ice_Floes.IsSpellUsable 
            && Ice_Floes.KnownSpell && MySettings.UseIceFloes && ObjectManager.Me.GetMove)
        {
            Ice_Floes.Launch();
            return;
        }
        else
        {
            if (ObjectManager.GetNumberAttackPlayer() == 0 && Blazing_Speed.IsSpellUsable && Blazing_Speed.KnownSpell
                && MySettings.UseBlazingSpeed && ObjectManager.Me.GetMove)
            {
                Blazing_Speed.Launch();
                return;
            }
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (ObjectManager.Me.ManaPercentage < 80 && Arcane_Torrent.KnownSpell && Arcane_Torrent.IsSpellUsable
            && MySettings.UseArcaneTorrent)
        {
            Arcane_Torrent.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && Gift_of_the_Naaru.KnownSpell && Gift_of_the_Naaru.IsSpellUsable
            && MySettings.UseGiftoftheNaaru)
        {
            Gift_of_the_Naaru.Launch();
            return;
        }
        else if (ObjectManager.Me.ManaPercentage < 40 && ItemsManager.GetItemCountByIdLUA(36799) > 0
            && MySettings.UseConjureManaGem)
        {
            Logging.WriteFight("Use Mana Gem.");
            Lua.RunMacroText("/use item:36799");
            return;
        }
        else if ((ObjectManager.Me.HealthPercent < 40 || ObjectManager.Me.ManaPercentage < 60)
            && MySettings.UseEvocation && Evocation.IsSpellUsable && !MySettings.UseInvocationTalent
            && !MySettings.UseRuneofPowerTalent && MySettings.UseEvocationGlyph)
        {
            Evocation.Launch();
            return;
        }
        else if (Conjure_Mana_Gem.KnownSpell && ItemsManager.GetItemCountByIdLUA(36799) == 0
            && MySettings.UseConjureManaGem)
        {
            Conjure_Mana_Gem.Launch();
            return;
        }
        else
        {
            if (Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(80610) == 0 // 90
                && Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(65499) == 0 // 85-89
                && Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(43523) == 0 // 84-80
                && Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(43518) == 0 // 79-74
                && Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(65517) == 0 // 73-64
                && Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(65516) == 0 // 63-54
                && Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(65515) == 0 // 53-44
                && Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(65500) == 0 // 43-38
                && MySettings.UseConjureRefreshment)
            {
                Conjure_Refreshment.Launch();
                return;
            }
        }
    }

    private void Defense_Cycle()
    {
        if (ObjectManager.Me.HealthPercent < 100 && Incanters_Ward.IsSpellUsable && Incanters_Ward.KnownSpell
            && MySettings.UseIncantersWard && !Incanters_Ward.HaveBuff && ObjectManager.GetNumberAttackPlayer() > 0)
        {
            Incanters_Ward.Launch();
            return;
        }

        if (Ring_of_Frost.KnownSpell && Ring_of_Frost.IsSpellUsable
            && ObjectManager.GetNumberAttackPlayer() > 2
            && ObjectManager.Target.GetDistance < 10 && MySettings.UseRingofFrost)
        {
            SpellManager.CastSpellByIDAndPosition(113724, ObjectManager.Target.Position);
            return;
        }
        else if (Frost_Nova.KnownSpell && ObjectManager.Target.GetDistance < 12
            && ObjectManager.Me.HealthPercent < 50 && MySettings.UseFrostNova)
        {
            if (!Frost_Nova.IsSpellUsable && Cold_Snap.KnownSpell && Cold_Snap.IsSpellUsable
                && MySettings.UseColdSnap)
            {
                Cold_Snap.Launch();
                Thread.Sleep(200);
            }

            if (Frost_Nova.IsSpellUsable)
            {
                Frost_Nova.Launch();
                return;
            }
        }
        else if (Ice_Ward.KnownSpell && Ice_Ward.IsSpellUsable && ObjectManager.Target.GetDistance < 10
            && ObjectManager.Me.HealthPercent < 45 && MySettings.UseIceWard && !Frost_Nova.IsSpellUsable)
        {
            Ice_Ward.Launch();
            return;
        }
        else if (Cone_of_Cold.KnownSpell && Cone_of_Cold.IsSpellUsable && ObjectManager.Target.GetDistance < 10
            && ObjectManager.Me.HealthPercent < 45 && MySettings.UseConeofCold && !Frost_Nova.IsSpellUsable
            && !Ice_Ward.IsSpellUsable)
        {
            Cone_of_Cold.Launch();
            return;
        }
        else if (Blink.KnownSpell && Blink.IsSpellUsable && ObjectManager.Target.GetDistance < 11
            && (Frost_Nova.TargetHaveBuff || Cone_of_Cold.TargetHaveBuff || Ice_Ward.TargetHaveBuff))
        {
            Blink.Launch();
            return;
        }
        else if (Deep_Freeze.KnownSpell && Deep_Freeze.IsSpellUsable && Deep_Freeze.IsDistanceGood
            && MySettings.UseDeepFreeze && ObjectManager.Me.HealthPercent < 50)
        {
            Deep_Freeze.Launch();
            OnCD = new Timer(1000*5);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 95 && Ice_Barrier.IsSpellUsable && Ice_Barrier.KnownSpell
            && MySettings.UseIceBarrier && !Ice_Barrier.HaveBuff && !Incanters_Ward.HaveBuff)
        {
            Ice_Barrier.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 95 && Temporal_Shield.IsSpellUsable && Temporal_Shield.KnownSpell
            && MySettings.UseTemporalShield && !Temporal_Shield.HaveBuff && ObjectManager.GetNumberAttackPlayer() > 0)
        {
            Temporal_Shield.Launch();
            OnCD = new Timer(1000*4);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 40 && Frost_Jaw.KnownSpell && Frost_Jaw.IsSpellUsable 
            && MySettings.UseFrostJaw && Frost_Jaw.IsDistanceGood)
        {
            Frost_Jaw.Launch();
            OnCD = new Timer(1000*8);
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
            if (ObjectManager.GetNumberAttackPlayer() > 3 && Invisibility.KnownSpell && Invisibility.IsSpellUsable
                && MySettings.UseInvisibility)
            {
                Invisibility.Launch();
                Thread.Sleep(5000);
                return;
            }
        }
    }

    private void Decast()
    {
        if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe
            && Counterspell.KnownSpell && Counterspell.IsSpellUsable && Counterspell.IsDistanceGood)
        {
            Counterspell.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe
                && Frost_Jaw.KnownSpell && Frost_Jaw.IsSpellUsable && Frost_Jaw.IsDistanceGood)
            {
                Frost_Jaw.Launch();
                OnCD = new Timer(1000*8);
                return;
            }
        }

        if (ObjectManager.Target.GetMove && !Slow.TargetHaveBuff && MySettings.UseSlow
            && Slow.KnownSpell && Slow.IsSpellUsable && Slow.IsDistanceGood)
        {
            Slow.Launch();
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

public class Mage_Frost
{
    [Serializable]
    public class MageFrostSettings : nManager.Helpful.Settings
    {
        /* Professions & Racials */
        public bool UseArcaneTorrent = true;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseLifeblood = true;
        public bool UseStoneform = true;
        public bool UseGiftoftheNaaru = true;
        public bool UseWarStomp = true;
        /* Mage Buffs */
        public bool UseArcaneBrilliance = true;
        public bool UseBlazingSpeed = true;
        public bool UseFrostArmor = true;
        public bool UseIceFloes = true;
        public bool UseMageArmor = false;
        public bool UseMoltenArmor = false;
        /* Offensive Spell */
        public bool UseArcaneExplosion = true;
        public bool UseBlizzard = true;
        public bool UseFlamestrike = true;
        public bool UseFreeze = true;
        public bool UseFrostbolt = true;
        public bool UseFrostfireBolt = true;
        public bool UseIceLance = true;
        public bool UseScorch = true;
        public bool UseSummonWaterElemental = true;
        /* Offensive Cooldown */
        public bool UseAlterTime = true;
        public bool UseFrozenOrb = true;
        public bool UseIcyVeins = true;
        public bool UseTierFive = true;
        public bool UseMirrorImage = true;
        public bool UsePresenceofMind = true;
        public bool UseTimeWarp = true;
        /* Defensive Cooldown */
        public bool UseBlink = true;
        public bool UseColdSnap = true;
        public bool UseConeofCold = true;
        public bool UseCounterspell = true;
        public bool UseDeepFreeze = true;
        public bool UseFrostJaw = true;
        public bool UseFrostNova = true;
        public bool UseIceBarrier = true;
        public bool UseIceBlock = true;
        public bool UseIceWard = true;
        public bool UseIncantersWard = true;
        public bool UseInvisibility = true;
        public bool UseRingofFrost = true;
        public bool UseTemporalShield  = true;
        /* Healing Spell */
        public bool UseConjureManaGem = true;
        public bool UseConjureRefreshment = true;
        public bool UseEvocation = true;
        /* Game Settings */
        public bool UseLowCombat = true;
        public bool UseTrinket = true;
        public bool UseEngGlove = true;
        public bool UseAlchFlask = true;
        public bool UseEvocationGlyph = false;
        public bool UseInvocationTalent = false;
        public bool UseRuneofPowerTalent = false;

        public MageFrostSettings()
        {
            ConfigWinForm(new System.Drawing.Point(400, 400), "Mage Frost Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");
            AddControlInWinForm("Use Lifeblood", "UseLifeblood", "Professions & Racials");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            /* Mage Buffs */
            AddControlInWinForm("Use Arcane Brilliance", "UseArcaneBrilliance", "Mage Buffs");
            AddControlInWinForm("Use Blazing Speed", "UseBlazingSpeed", "Mage Buffs");
            AddControlInWinForm("Use Frost Armor", "UseFrostArmor", "Mage Buffs");
            AddControlInWinForm("Use Ice Floes", "UseIceFloes", "Mage Buffs");
            AddControlInWinForm("Use Mage Armor", "UseMageArmor", "Mage Buffs");
            AddControlInWinForm("Use Molten Armor", "UseMoltenArmor", "Mage Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Arcane Explosion", "UseArcaneExplosion", "Offensive Spell");
            AddControlInWinForm("Use Blizzard", "UseBlizzard", "Offensive Spell");
            AddControlInWinForm("Use Flamestrike", "UseFlamestrike", "Offensive Spell");
            AddControlInWinForm("Use Pet Freeze Ability", "UseFreeze", "Offensive Spell");
            AddControlInWinForm("Use Frostbolt", "UseFrostbolt", "Offensive Spell");
            AddControlInWinForm("Use Frostfire Bolt", "UseFrostfireBolt", "Offensive Spell");
            AddControlInWinForm("Use Ice Lance", "UseIceLance", "Offensive Spell");
            AddControlInWinForm("Use Scorch", "UseScorch", "Offensive Spell");
            AddControlInWinForm("Use Summon Water Elemental", "UseSummonWaterElemental", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use Alter Time", "UseAlterTime", "Offensive Cooldown");
            AddControlInWinForm("Use Frozen Orb", "UseFrozenOrb", "Offensive Cooldown");
            AddControlInWinForm("Use Icy Veins", "UseIcyVeins", "Offensive Cooldown");
            AddControlInWinForm("Use Mirror Image", "UseMirrorImage", "Offensive Cooldown");
            AddControlInWinForm("Use Presence of Mind", "UsePresenceofMind", "Offensive Cooldown");
            AddControlInWinForm("Use Tier Five Ability", "UseTierFive", "Offensive Cooldown");
            AddControlInWinForm("Use Time Warp", "UseTimeWarp", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Blink", "UseBlink", "Defensive Cooldown");
            AddControlInWinForm("Use Cold Snap", "UseColdSnap", "Defensive Cooldown");
            AddControlInWinForm("Use Cone of Cold", "UseConeofCold", "Defensive Cooldown");
            AddControlInWinForm("Use Counterspell", "UseCounterspell", "Defensive Cooldown");
            AddControlInWinForm("Use Deep Freeze", "UseDeepFreeze", "Defensive Cooldown");
            AddControlInWinForm("Use Frost Jaw", "UseFrostJaw", "Defensive Cooldown");
            AddControlInWinForm("Use Frost Nova", "UseFrostNova", "Defensive Cooldown");
            AddControlInWinForm("Use Ice Barrier", "UseIceBarrier", "Defensive Cooldown");
            AddControlInWinForm("Use Ice Block", "UseIceBlock", "Defensive Cooldown");
            AddControlInWinForm("Use Ice Ward", "UseIceWard", "Defensive Cooldown");
            AddControlInWinForm("Use Incanter's Ward", "UseIncantersWard", "Defensive Cooldown");
            AddControlInWinForm("Use Invisibility", "UseInvisibility", "Defensive Cooldown");
            AddControlInWinForm("Use Ring of Frost", "UseRingofFrost", "Defensive Cooldown");
            AddControlInWinForm("Use Temporal Shield", "UseTemporalShield", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Conjure Mana Gem", "UseConjureManaGem", "Healing Spell");
            AddControlInWinForm("Use Conjure Refreshment", "UseConjureRefreshment", "Healing Spell");
            AddControlInWinForm("Use Evocation", "UseEvocation", "Healing Spell");
            /* Game Settings */
            AddControlInWinForm("Use Low Combat Settings", "UseLowCombat", "Game Settings");
            AddControlInWinForm("Use Trinket", "UseTrinket", "Game Settings");
            AddControlInWinForm("Use Engineering Gloves", "UseEngGlove", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
            AddControlInWinForm("Use Evocation Glyph", "UseEvocationGlyph", "Game Settings");
            AddControlInWinForm("Use Invocation Talent", "UseInvocationTalent", "Game Settings");
            AddControlInWinForm("Use Rune of Power Talent", "UseRuneofPowerTalent", "Game Settings");
        }

        public static MageFrostSettings CurrentSetting { get; set; }

        public static MageFrostSettings GetSettings()
        {
            string CurrentSettingsFile = Application.StartupPath + "\\CustomClasses\\Settings\\Mage_Frost.xml";
            if (System.IO.File.Exists(CurrentSettingsFile))
            {
                return
                    CurrentSetting = Settings.Load<Mage_Frost.MageFrostSettings>(CurrentSettingsFile);
            }
            else
            {
                return new Mage_Frost.MageFrostSettings();
            }
        }
    }

    private readonly MageFrostSettings MySettings = MageFrostSettings.GetSettings();

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

    #region Mage Buffs

    private readonly Spell Arcane_Brilliance = new Spell("Arcane Brilliance");
    private readonly Spell Blazing_Speed = new Spell("Blazing Speed");
    private readonly Spell Frost_Armor = new Spell("Frost Armor");
    private readonly Spell Ice_Floes = new Spell("Ice Floes");
    private readonly Spell Mage_Armor = new Spell("Mage Armor");
    private readonly Spell Molten_Armor = new Spell("Molten Armor");

    #endregion

    #region Offensive Spell

    private readonly Spell Arcane_Explosion = new Spell("Arcane Explosion");
    private readonly Spell Blizzard = new Spell("Blizzard");
    private readonly Spell Cone_of_Cold = new Spell("Cone of Cold");
    private readonly Spell Fire_Blast = new Spell("Fire Blast");
    private readonly Spell Flamestrike = new Spell("Flamestrike");
    private Timer Flamestrike_Timer = new Timer(0);
    private readonly Spell Frostbolt = new Spell("Frostbolt");
    private readonly Spell Frostfire_Bolt = new Spell("Frostfire Bolt");
    private readonly Spell Ice_Lance = new Spell("Ice Lance");
    private readonly Spell Scorch = new Spell("Scorch");
    private readonly Spell Summon_Water_Elemental = new Spell("Summon Water Elemental");

    #endregion

    #region Offensive Cooldown

    private readonly Spell Alter_Time = new Spell("Alter Time");
    private readonly Spell Frozen_Orb = new Spell("Frozen Orb");
    private readonly Spell Icy_Veins = new Spell("Icy Veins");
    private readonly Spell Mage_Bomb = new Spell("Mage Bomb");
    private readonly Spell Mirror_Image = new Spell("Mirror Image");
    private readonly Spell Presence_of_Mind = new Spell("Presence of Mind");
    private readonly Spell Time_Warp = new Spell("Time Warp");

    #endregion

    #region Defensive Cooldown

    private readonly Spell Blink = new Spell("Blink");
    private readonly Spell Cold_Snap = new Spell("Cold Snap");
    private readonly Spell Counterspell = new Spell("Counterspell");
    private readonly Spell Deep_Freeze = new Spell("Deep Freeze");
    private readonly Spell Frost_Jaw = new Spell("Frost Jaw");
    private readonly Spell Frost_Nova = new Spell("Frost Nova");
    private readonly Spell Ice_Barrier = new Spell("Ice Barrier");
    private readonly Spell Ice_Block = new Spell("Ice Block");
    private readonly Spell Ice_Ward = new Spell("Ice Ward");
    private readonly Spell Incanters_Ward = new Spell("Incanter's Ward");
    private readonly Spell Invisibility = new Spell("Invisibility");
    private readonly Spell Ring_of_Frost = new Spell("Ring of Frost");
    private readonly Spell Temporal_Shield = new Spell("Temporal Shield");

    #endregion

    #region Healing Spell

    private readonly Spell Conjure_Mana_Gem = new Spell("Conjure Mana Gem");
    private readonly Spell Conjure_Refreshment = new Spell("Conjure Refreshment");
    private readonly Spell Evocation = new Spell("Evocation");

    #endregion

    private Timer OnCD = new Timer(0);
    private Timer Trinket_Timer = new Timer(0);
    private Timer Engineering_Timer = new Timer(0);
    private Timer AlchFlask_Timer = new Timer(0);
    private Timer Freeze_Timer = new Timer(0);
    public int LC = 0;

    public Mage_Frost()
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
                            (Frostbolt.IsDistanceGood || Ice_Lance.IsDistanceGood))
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
        if (ObjectManager.Pet.IsAlive)
        {
            Lua.RunMacroText("/petattack");
            Logging.WriteFight("Launch Pet Attack");
        }

        if (Scorch.IsDistanceGood && Scorch.KnownSpell && Scorch.IsSpellUsable && MySettings.UseScorch)
            Scorch.Launch();
        else
        {
            if (Ice_Lance.IsDistanceGood && Ice_Lance.KnownSpell && Ice_Lance.IsSpellUsable && MySettings.UseIceLance)
                Ice_Lance.Launch();
        }

        if (ObjectManager.Me.Level > 9 && Freeze_Timer.IsReady && MySettings.UseFreeze
            && (ObjectManager.Pet.Health != 0 || ObjectManager.Pet.Guid != 0))
        {
            SpellManager.CastSpellByIDAndPosition(33395, ObjectManager.Target.Position);
            Freeze_Timer = new Timer(1000 * 25);
            Thread.Sleep(400);
            if (Deep_Freeze.IsSpellUsable && Deep_Freeze.KnownSpell && Deep_Freeze.IsDistanceGood)
            {
                Deep_Freeze.Launch();
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

        if (Ice_Lance.IsDistanceGood && Ice_Lance.KnownSpell && Ice_Lance.IsSpellUsable
            && ObjectManager.Me.HaveBuff(44544) && MySettings.UseIceLance)
        {
            Ice_Lance.Launch();
            return;
        }
        else
        {
            if (Frostbolt.KnownSpell && Frostbolt.IsSpellUsable && Frostbolt.IsDistanceGood
                && MySettings.UseFrostbolt)
            {
                Frostbolt.Launch();
                return;
            }
        }

        if (ObjectManager.Target.HealthPercent > 90)
        {
            if (Arcane_Explosion.KnownSpell && Arcane_Explosion.IsSpellUsable && Arcane_Explosion.IsDistanceGood
                && MySettings.UseArcaneExplosion)
            {
                Arcane_Explosion.Launch();
                return;
            }
        }
    }

    public void DPS_Burst()
    {
        if (Alter_Time.IsSpellUsable && Alter_Time.KnownSpell && MySettings.UseAlterTime
            && ObjectManager.Target.GetDistance < 30 && ObjectManager.Target.InCombat)
            Alter_Time.Launch();

        if (MySettings.UseTrinket && Trinket_Timer.IsReady && ObjectManager.Target.GetDistance < 40)
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
            && ObjectManager.Target.GetDistance < 40)
        {
            Berserking.Launch();
            return;
        }
        else if (Blood_Fury.IsSpellUsable && Blood_Fury.KnownSpell && MySettings.UseBloodFury
            && ObjectManager.Target.GetDistance < 40)
        {
            Blood_Fury.Launch();
            return;
        }
        else if (Lifeblood.IsSpellUsable && Lifeblood.KnownSpell && MySettings.UseLifeblood
            && ObjectManager.Target.GetDistance < 40)
        {
            Lifeblood.Launch();
            return;
        }
        else if (MySettings.UseEngGlove && Engineering.KnownSpell && Engineering_Timer.IsReady
            && ObjectManager.Target.GetDistance < 40)
        {
            Logging.WriteFight("Use Engineering Gloves.");
            Lua.RunMacroText("/use 10");
            Engineering_Timer = new Timer(1000*60);
            return;
        }
        else if (Mage_Bomb.KnownSpell && Mage_Bomb.IsSpellUsable && Mage_Bomb.IsDistanceGood
            && MySettings.UseTierFive)
        {
            Mage_Bomb.Launch();
            return;
        }
        else if (Frozen_Orb.KnownSpell && Frozen_Orb.IsSpellUsable && Frozen_Orb.IsDistanceGood
            && MySettings.UseFrozenOrb)
        {
            Frozen_Orb.Launch();
            return;
        }
        else if (Evocation.KnownSpell && Evocation.IsSpellUsable && ObjectManager.Target.GetDistance < 40
            && MySettings.UseInvocationTalent && !ObjectManager.Me.HaveBuff(114003))
        {
            Evocation.Launch();
            return;
        }
        else if (Evocation.KnownSpell && Evocation.IsSpellUsable && ObjectManager.Target.GetDistance < 40
            && MySettings.UseRuneofPowerTalent && !ObjectManager.Me.HaveBuff(116011))
        {
            SpellManager.CastSpellByIDAndPosition(116011, ObjectManager.Target.Position);
            return;
        }
        else if (Icy_Veins.KnownSpell && Icy_Veins.IsSpellUsable && ObjectManager.Target.GetDistance < 40
            && MySettings.UseIcyVeins && !Time_Warp.HaveBuff)
        {
            Icy_Veins.Launch();
            return;
        }
        else if (Mirror_Image.KnownSpell && Mirror_Image.IsSpellUsable && ObjectManager.Target.GetDistance < 40
            && MySettings.UseMirrorImage)
        {
            Mirror_Image.Launch();
            return;
        }
        else
        {
            if (Time_Warp.KnownSpell && Time_Warp.IsSpellUsable && MySettings.UseTimeWarp
                && !ObjectManager.Me.HaveBuff(80354) && ObjectManager.Target.GetDistance < 40
                && !Icy_Veins.HaveBuff)
            {
                Time_Warp.Launch();
                return;
            }
        }
    }

    public void DPS_Cycle()
    {
        if (Scorch.IsSpellUsable && MySettings.UseScorch && Scorch.IsDistanceGood
            && Scorch.KnownSpell && ObjectManager.Me.GetMove && !Ice_Floes.HaveBuff)
        {
            Scorch.Launch();
            return;
        }

        if (ObjectManager.GetNumberAttackPlayer() > 4 && Flamestrike.IsSpellUsable && Flamestrike.KnownSpell
            && Flamestrike.IsDistanceGood && Flamestrike_Timer.IsReady && MySettings.UseFlamestrike)
        {
            SpellManager.CastSpellByIDAndPosition(2120, ObjectManager.Target.Position);
            Flamestrike_Timer = new Timer(1000*8);
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 4 && Arcane_Explosion.IsSpellUsable && Arcane_Explosion.KnownSpell
            && Arcane_Explosion.IsDistanceGood && MySettings.UseArcaneExplosion)
        {
            Arcane_Explosion.Launch();
            return;
        }
        else if (Frozen_Orb.KnownSpell && Frozen_Orb.IsSpellUsable && Frozen_Orb.IsDistanceGood
            && MySettings.UseFrozenOrb)
        {
            Frozen_Orb.Launch();
            return;
        }
        else if (ObjectManager.Me.Level > 9 && Freeze_Timer.IsReady && MySettings.UseFreeze
            && (ObjectManager.Pet.Health != 0 || ObjectManager.Pet.Guid != 0) && ObjectManager.Target.GetDistance < 35)
        {
            SpellManager.CastSpellByIDAndPosition(33395, ObjectManager.Target.Position);
            Freeze_Timer = new Timer(1000 * 25);
            return;
        }
        else if (Frostfire_Bolt.IsDistanceGood && Frostfire_Bolt.KnownSpell && Frostfire_Bolt.IsSpellUsable
            && ObjectManager.Me.HaveBuff(57761) && MySettings.UseFrostfireBolt)
        {
            Frostfire_Bolt.Launch();
            return;
        }
        else if (Ice_Lance.IsDistanceGood && Ice_Lance.KnownSpell && Ice_Lance.IsSpellUsable
            && ObjectManager.Me.HaveBuff(44544) && MySettings.UseIceLance)
        {
            Ice_Lance.Launch();
            return;
        }
        else if (Frostbolt.KnownSpell && Frostbolt.IsSpellUsable && Frostbolt.IsDistanceGood
            && Presence_of_Mind.KnownSpell && Presence_of_Mind.IsSpellUsable)
        {
            Presence_of_Mind.Launch();
            Thread.Sleep(400);
            Frostbolt.Launch();
            return;
        }
        else
        {
            if (Frostbolt.KnownSpell && Frostbolt.IsSpellUsable && Frostbolt.IsDistanceGood)
            {
                Frostbolt.Launch();
                return;
            }
        }

        if (ObjectManager.Me.Level < 10 && Frostfire_Bolt.KnownSpell && Frostfire_Bolt.IsSpellUsable
            && Frostfire_Bolt.IsDistanceGood)
        {
            Frostfire_Bolt.Launch();
            return;
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

        if (ObjectManager.Me.HaveBuff(87023) && Ice_Block.KnownSpell && MySettings.UseIceBlock
            && !ObjectManager.Me.HaveBuff(41425))
        {
            if (!Ice_Block.IsSpellUsable && Cold_Snap.KnownSpell && Cold_Snap.IsSpellUsable
                && MySettings.UseColdSnap)
            {
                Cold_Snap.Launch();
                Thread.Sleep(400);
            }
            Ice_Block.Launch();
            OnCD = new Timer(1000*10);
            return;
        }

        if (MySettings.UseArcaneBrilliance && Arcane_Brilliance.KnownSpell && Arcane_Brilliance.IsSpellUsable 
            && !Arcane_Brilliance.HaveBuff && !ObjectManager.Me.HaveBuff(61316))
        {
            Arcane_Brilliance.Launch();
            return;
        }
        else if (MySettings.UseFrostArmor && Frost_Armor.KnownSpell && Frost_Armor.IsSpellUsable 
            && !Frost_Armor.HaveBuff)
        {
            Frost_Armor.Launch();
            return;
        }
        else if (MySettings.UseMoltenArmor && Molten_Armor.KnownSpell && Molten_Armor.IsSpellUsable 
            && !Molten_Armor.HaveBuff && !MySettings.UseFrostArmor)
        {
            Molten_Armor.Launch();
            return;
        }
        else if (MySettings.UseMageArmor && Mage_Armor.KnownSpell && Mage_Armor.IsSpellUsable 
            && !Mage_Armor.HaveBuff && !MySettings.UseFrostArmor && !MySettings.UseMoltenArmor)
        {
            Mage_Armor.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 0 && Ice_Floes.IsSpellUsable 
            && Ice_Floes.KnownSpell && MySettings.UseIceFloes && ObjectManager.Me.GetMove)
        {
            Ice_Floes.Launch();
            return;
        }
        else
        {
            if (ObjectManager.GetNumberAttackPlayer() == 0 && Blazing_Speed.IsSpellUsable && Blazing_Speed.KnownSpell
                && MySettings.UseBlazingSpeed && ObjectManager.Me.GetMove)
            {
                Blazing_Speed.Launch();
                return;
            }
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (ObjectManager.Me.ManaPercentage < 80 && Arcane_Torrent.KnownSpell && Arcane_Torrent.IsSpellUsable
            && MySettings.UseArcaneTorrent)
        {
            Arcane_Torrent.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && Gift_of_the_Naaru.KnownSpell && Gift_of_the_Naaru.IsSpellUsable
            && MySettings.UseGiftoftheNaaru)
        {
            Gift_of_the_Naaru.Launch();
            return;
        }
        else if (ObjectManager.Me.ManaPercentage < 40 && ItemsManager.GetItemCountByIdLUA(36799) > 0
            && MySettings.UseConjureManaGem)
        {
            Logging.WriteFight("Use Mana Gem.");
            Lua.RunMacroText("/use item:36799");
            return;
        }
        else if ((ObjectManager.Me.HealthPercent < 40 || ObjectManager.Me.ManaPercentage < 60)
            && MySettings.UseEvocation && Evocation.IsSpellUsable && !MySettings.UseInvocationTalent
            && !MySettings.UseRuneofPowerTalent && MySettings.UseEvocationGlyph)
        {
            Evocation.Launch();
            return;
        }
        else if (Conjure_Mana_Gem.KnownSpell && ItemsManager.GetItemCountByIdLUA(36799) == 0
            && MySettings.UseConjureManaGem)
        {
            Conjure_Mana_Gem.Launch();
            return;
        }
        else
        {
            if (Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(80610) == 0 // 90
                && Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(65499) == 0 // 85-89
                && Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(43523) == 0 // 84-80
                && Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(43518) == 0 // 79-74
                && Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(65517) == 0 // 73-64
                && Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(65516) == 0 // 63-54
                && Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(65515) == 0 // 53-44
                && Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(65500) == 0 // 43-38
                && MySettings.UseConjureRefreshment)
            {
                Conjure_Refreshment.Launch();
                return;
            }
        }
    }

    private void Defense_Cycle()
    {
        if (ObjectManager.Me.HealthPercent < 100 && Incanters_Ward.IsSpellUsable && Incanters_Ward.KnownSpell
            && MySettings.UseIncantersWard && !Incanters_Ward.HaveBuff && ObjectManager.GetNumberAttackPlayer() > 0)
        {
            Incanters_Ward.Launch();
            return;
        }

        if (Ring_of_Frost.KnownSpell && Ring_of_Frost.IsSpellUsable
            && ObjectManager.GetNumberAttackPlayer() > 2
            && ObjectManager.Target.GetDistance < 10 && MySettings.UseRingofFrost)
        {
            SpellManager.CastSpellByIDAndPosition(113724, ObjectManager.Target.Position);
            return;
        }
        else if (Frost_Nova.KnownSpell && ObjectManager.Target.GetDistance < 12
            && ObjectManager.Me.HealthPercent < 50 && MySettings.UseFrostNova)
        {
            if (!Frost_Nova.IsSpellUsable && Cold_Snap.KnownSpell && Cold_Snap.IsSpellUsable
                && MySettings.UseColdSnap)
            {
                Cold_Snap.Launch();
                Thread.Sleep(200);
            }

            if (Frost_Nova.IsSpellUsable)
            {
                Frost_Nova.Launch();
                return;
            }
        }
        else if (Ice_Ward.KnownSpell && Ice_Ward.IsSpellUsable && ObjectManager.Target.GetDistance < 10
            && ObjectManager.Me.HealthPercent < 45 && MySettings.UseIceWard && !Frost_Nova.IsSpellUsable)
        {
            Ice_Ward.Launch();
            return;
        }
        else if (Cone_of_Cold.KnownSpell && Cone_of_Cold.IsSpellUsable && ObjectManager.Target.GetDistance < 10
            && ObjectManager.Me.HealthPercent < 45 && MySettings.UseConeofCold && !Frost_Nova.IsSpellUsable
            && !Ice_Ward.IsSpellUsable)
        {
            Cone_of_Cold.Launch();
            return;
        }
        else if (Blink.KnownSpell && Blink.IsSpellUsable && ObjectManager.Target.GetDistance < 11
            && (Frost_Nova.TargetHaveBuff || Cone_of_Cold.TargetHaveBuff || Ice_Ward.TargetHaveBuff))
        {
            Blink.Launch();
            return;
        }
        else if (Deep_Freeze.KnownSpell && Deep_Freeze.IsSpellUsable && Deep_Freeze.IsDistanceGood
            && MySettings.UseDeepFreeze && ObjectManager.Me.HealthPercent < 50)
        {
            Deep_Freeze.Launch();
            OnCD = new Timer(1000*5);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 95 && Ice_Barrier.IsSpellUsable && Ice_Barrier.KnownSpell
            && MySettings.UseIceBarrier && !Ice_Barrier.HaveBuff && !Incanters_Ward.HaveBuff)
        {
            Ice_Barrier.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 95 && Temporal_Shield.IsSpellUsable && Temporal_Shield.KnownSpell
            && MySettings.UseTemporalShield && !Temporal_Shield.HaveBuff && ObjectManager.GetNumberAttackPlayer() > 0)
        {
            Temporal_Shield.Launch();
            OnCD = new Timer(1000*4);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 40 && Frost_Jaw.KnownSpell && Frost_Jaw.IsSpellUsable 
            && MySettings.UseFrostJaw && Frost_Jaw.IsDistanceGood)
        {
            Frost_Jaw.Launch();
            OnCD = new Timer(1000*8);
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
            if (ObjectManager.GetNumberAttackPlayer() > 3 && Invisibility.KnownSpell && Invisibility.IsSpellUsable
                && MySettings.UseInvisibility)
            {
                Invisibility.Launch();
                Thread.Sleep(5000);
                return;
            }
        }
    }

    private void Decast()
    {
        if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe
            && Counterspell.KnownSpell && Counterspell.IsSpellUsable && Counterspell.IsDistanceGood)
        {
            Counterspell.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe
                && Frost_Jaw.KnownSpell && Frost_Jaw.IsSpellUsable && Frost_Jaw.IsDistanceGood)
            {
                Frost_Jaw.Launch();
                OnCD = new Timer(1000*8);
                return;
            }
        }
    }

    private void Pet()
    {
        if (Summon_Water_Elemental.IsSpellUsable && Summon_Water_Elemental.KnownSpell 
            && (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0)
            && MySettings.UseSummonWaterElemental)
        {
            Logging.WriteFight(" - PET DEAD - ");
            Summon_Water_Elemental.Launch();
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

public class Mage_Fire
{
    [Serializable]
    public class MageFireSettings : nManager.Helpful.Settings
    {
        /* Professions & Racials */
        public bool UseArcaneTorrent = true;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseLifeblood = true;
        public bool UseStoneform = true;
        public bool UseGiftoftheNaaru = true;
        public bool UseWarStomp = true;
        /* Mage Buffs */
        public bool UseArcaneBrilliance = true;
        public bool UseBlazingSpeed = true;
        public bool UseFrostArmor = true;
        public bool UseIceFloes = false;
        public bool UseMageArmor = true;
        public bool UseMoltenArmor = false;
        /* Offensive Spell */
        public bool UseArcaneExplosion = true;
        public bool UseDragonsBreath = true;
        public bool UseFireball = true;
        public bool UseFlamestrike = true;
        public bool UseInfernoBlast = true;
        public bool UsePyroblast = true;
        public bool UseScorch = true;
        /* Offensive Cooldown */
        public bool UseAlterTime = true;
        public bool UseCombustion = true;
        public bool UseFrozenOrb = true;
        public bool UseMirrorImage = true;
        public bool UsePresenceofMind = true;
        public bool UseTierFive = true;
        public bool UseTimeWarp = true;
        /* Defensive Cooldown */
        public bool UseBlink = true;
        public bool UseColdSnap = true;
        public bool UseConeofCold = true;
        public bool UseCounterspell = true;
        public bool UseDeepFreeze = true;
        public bool UseFrostJaw = true;
        public bool UseFrostNova = true;
        public bool UseIceBarrier = true;
        public bool UseIceBlock = true;
        public bool UseIceWard = true;
        public bool UseIncantersWard = true;
        public bool UseInvisibility = true;
        public bool UseRingofFrost = true;
        public bool UseTemporalShield  = true;
        /* Healing Spell */
        public bool UseConjureManaGem = true;
        public bool UseConjureRefreshment = true;
        public bool UseEvocation = true;
        /* Game Settings */
        public bool UseLowCombat = true;
        public bool UseTrinket = true;
        public bool UseEngGlove = true;
        public bool UseAlchFlask = true;
        public bool UseEvocationGlyph = false;
        public bool UseInvocationTalent = false;
        public bool UseRuneofPowerTalent = false;

        public MageFireSettings()
        {
            ConfigWinForm(new System.Drawing.Point(400, 400), "Mage Fire Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");
            AddControlInWinForm("Use Lifeblood", "UseLifeblood", "Professions & Racials");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            /* Mage Buffs */
            AddControlInWinForm("Use Arcane Brilliance", "UseArcaneBrilliance", "Mage Buffs");
            AddControlInWinForm("Use Blazing Speed", "UseBlazingSpeed", "Mage Buffs");
            AddControlInWinForm("Use Frost Armor", "UseFrostArmor", "Mage Buffs");
            AddControlInWinForm("Use Ice Floes", "UseIceFloes", "Mage Buffs");
            AddControlInWinForm("Use Mage Armor", "UseMageArmor", "Mage Buffs");
            AddControlInWinForm("Use Molten Armor", "UseMoltenArmor", "Mage Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Arcane Explosion", "UseArcaneExplosion", "Offensive Spell");
            AddControlInWinForm("Use Dragon's Breath", "UseDragonsBreath", "Offensive Spell");
            AddControlInWinForm("Use Fireball", "UseFireball", "Offensive Spell");
            AddControlInWinForm("Use Flamestrike", "UseFlamestrike", "Offensive Spell");
            AddControlInWinForm("Use Inferno Blast", "UseInfernoBlast", "Offensive Spell");
            AddControlInWinForm("Use Pyroblast", "UsePyroblast", "Offensive Spell");
            AddControlInWinForm("Use Scorch", "UseScorch", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use Alter Time", "UseAlterTime", "Offensive Cooldown");
            AddControlInWinForm("Use Combustion", "UseCombustion", "Offensive Cooldown");
            AddControlInWinForm("Use Frozen Orb", "UseFrozenOrb", "Offensive Cooldown");
            AddControlInWinForm("Use Mirror Image", "UseMirrorImage", "Offensive Cooldown");
            AddControlInWinForm("Use Presence of Mind", "UsePresenceofMind", "Offensive Cooldown");
            AddControlInWinForm("Use Tier Five Talent", "UseTierFive", "Offensive Cooldown");
            AddControlInWinForm("Use Time Warp", "UseTimeWarp", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Blink", "UseBlink", "Defensive Cooldown");
            AddControlInWinForm("Use Cold Snap", "UseColdSnap", "Defensive Cooldown");
            AddControlInWinForm("Use Cone of Cold", "UseConeofCold", "Defensive Cooldown");
            AddControlInWinForm("Use Counterspell", "UseCounterspell", "Defensive Cooldown");
            AddControlInWinForm("Use DeepFreeze", "UseDeepFreeze", "Defensive Cooldown");
            AddControlInWinForm("Use Frost Jaw", "UseFrostJaw", "Defensive Cooldown");
            AddControlInWinForm("Use Fros Nova", "UseFrostNova", "Defensive Cooldown");
            AddControlInWinForm("Use Ice Barrier", "UseIceBarrier", "Defensive Cooldown");
            AddControlInWinForm("Use Ice Block", "UseIceBlock", "Defensive Cooldown");
            AddControlInWinForm("Use Ice Ward", "UseIceWard", "Defensive Cooldown");
            AddControlInWinForm("Use Incanter's Ward", "UseIncantersWard", "Defensive Cooldown");
            AddControlInWinForm("Use Invisibility", "UseInvisibility", "Defensive Cooldown");
            AddControlInWinForm("Use Ring of Frost", "UseRingofFrost", "Defensive Cooldown");
            AddControlInWinForm("Use Temporal Shield", "UseTemporalShield", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Conjure Mana Gem", "UseConjureManaGem", "Healing Spell");
            AddControlInWinForm("Use Conjure Refreshment", "UseConjureRefreshment", "Healing Spell");
            AddControlInWinForm("Use Evocation", "UseEvocation", "Healing Spell");
            /* Game Settings */
            AddControlInWinForm("Use Low Combat Settings", "UseLowCombat", "Game Settings");
            AddControlInWinForm("Use Trinket", "UseTrinket", "Game Settings");
            AddControlInWinForm("Use Engineering Gloves", "UseEngGlove", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
            AddControlInWinForm("Use Evocation Glyph", "UseEvocationGlyph", "Game Settings");
            AddControlInWinForm("Use Invocation Talent", "UseInvocationTalent", "Game Settings");
            AddControlInWinForm("Use Rune of Power Talent", "UseRuneofPowerTalent", "Game Settings");
        }

        public static MageFireSettings CurrentSetting { get; set; }

        public static MageFireSettings GetSettings()
        {
            string CurrentSettingsFile = Application.StartupPath + "\\CustomClasses\\Settings\\Mage_Fire.xml";
            if (System.IO.File.Exists(CurrentSettingsFile))
            {
                return
                    CurrentSetting = Settings.Load<Mage_Fire.MageFireSettings>(CurrentSettingsFile);
            }
            else
            {
                return new Mage_Fire.MageFireSettings();
            }
        }
    }

    private readonly MageFireSettings MySettings = MageFireSettings.GetSettings();

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

    #region Mage Buffs

    private readonly Spell Arcane_Brilliance = new Spell("Arcane Brilliance");
    private readonly Spell Blazing_Speed = new Spell("Blazing Speed");
    private readonly Spell Frost_Armor = new Spell("Frost Armor");
    private readonly Spell Ice_Floes = new Spell("Ice Floes");
    private readonly Spell Mage_Armor = new Spell("Mage Armor");
    private readonly Spell Molten_Armor = new Spell("Molten Armor");

    #endregion

    #region Offensive Spell

    private readonly Spell Arcane_Explosion = new Spell("Arcane Explosion");
    private readonly Spell DragonsBreath = new Spell("Dragon's Breath");
    private readonly Spell Fireball = new Spell("Fireball");
    private readonly Spell Fire_Blast = new Spell("Fire Blast");
    private readonly Spell Flamestrike = new Spell("Flamestrike");
    private Timer Flamestrike_Timer = new Timer(0);
    private readonly Spell InfernoBlast = new Spell("Inferno Blast");
    private readonly Spell Pyroblast = new Spell("Pyroblast");
    private readonly Spell Scorch = new Spell("Scorch");

    #endregion

    #region Offensive Cooldown

    private readonly Spell Alter_Time = new Spell("Alter Time");
    private readonly Spell Combustion = new Spell("Combustion");
    private readonly Spell Frost_Bomb = new Spell("Frost Bomb");
    private readonly Spell Mage_Bomb = new Spell("Mage Bomb");
    private readonly Spell Mirror_Image = new Spell("Mirror Image");
    private readonly Spell Presence_of_Mind = new Spell("Presence of Mind");
    private readonly Spell Time_Warp = new Spell("Time Warp");

    #endregion

    #region Defensive Cooldown

    private readonly Spell Blink = new Spell("Blink");
    private readonly Spell Cold_Snap = new Spell("Cold Snap");
    private readonly Spell Cone_of_Cold = new Spell("Cone of Cold");
    private readonly Spell Counterspell = new Spell("Counterspell");
    private readonly Spell Deep_Freeze = new Spell("Deep Freeze");
    private readonly Spell Frost_Jaw = new Spell("Frost Jaw");
    private readonly Spell Frost_Nova = new Spell("Frost Nova");
    private readonly Spell Ice_Barrier = new Spell("Ice Barrier");
    private readonly Spell Ice_Block = new Spell("Ice Block");
    private readonly Spell Ice_Ward = new Spell("Ice Ward");
    private readonly Spell Incanters_Ward = new Spell("Incanter's Ward");
    private readonly Spell Invisibility = new Spell("Invisibility");
    private readonly Spell Ring_of_Frost = new Spell("Ring of Frost");
    private readonly Spell Temporal_Shield = new Spell("Temporal Shield");

    #endregion

    #region Healing Spell

    private readonly Spell Conjure_Mana_Gem = new Spell("Conjure Mana Gem");
    private readonly Spell Conjure_Refreshment = new Spell("Conjure Refreshment");
    private readonly Spell Evocation = new Spell("Evocation");

    #endregion

    private Timer OnCD = new Timer(0);
    private Timer Trinket_Timer = new Timer(0);
    private Timer Engineering_Timer = new Timer(0);
    private Timer AlchFlask_Timer = new Timer(0);
    public int LC = 0;

    public Mage_Fire()
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
                            (Scorch.IsDistanceGood || Fireball.IsDistanceGood))
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
            Thread.Sleep(150);
        }
    }

    public void Pull()
    {
        if (Pyroblast.KnownSpell && Pyroblast.IsSpellUsable && Pyroblast.IsDistanceGood
            && ObjectManager.Me.HaveBuff(48108) && MySettings.UsePyroblast)
        {
            Pyroblast.Launch();
            return;
        }
        else if (Scorch.IsDistanceGood && Scorch.KnownSpell && Scorch.IsSpellUsable && MySettings.UseScorch)
        {
            Scorch.Launch();
            return;
        }
        else
        {
            if (Fireball.IsDistanceGood && Fireball.KnownSpell && Fireball.IsSpellUsable && MySettings.UseFireball)
            {
                Fireball.Launch();
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

        if (Pyroblast.KnownSpell && Pyroblast.IsSpellUsable && Pyroblast.IsDistanceGood
            && ObjectManager.Me.HaveBuff(48108))
        {
            Pyroblast.Launch();
            return;
        }
        //Blizzard API calls for Inferno Blast using the Fire Blast function.
        else if (Fire_Blast.IsDistanceGood && Fire_Blast.IsSpellUsable && Fire_Blast.KnownSpell
            && MySettings.UseInfernoBlast)
        {
            Fire_Blast.Launch();
            return;
        }
        else
        {
            if (Fireball.KnownSpell && Fireball.IsSpellUsable && Fireball.IsDistanceGood
                && MySettings.UseFireball)
            {
                Fireball.Launch();
                return;
            }
        }

        if (Arcane_Explosion.KnownSpell && Arcane_Explosion.IsSpellUsable && Arcane_Explosion.IsDistanceGood
            && MySettings.UseArcaneExplosion)
        {
            Arcane_Explosion.Launch();
            return;
        }
    }

    public void DPS_Burst()
    {
        if (Alter_Time.IsSpellUsable && Alter_Time.KnownSpell && MySettings.UseAlterTime
            && ObjectManager.Target.GetDistance < 30 && ObjectManager.Target.InCombat)
            Alter_Time.Launch();

        if (MySettings.UseTrinket && Trinket_Timer.IsReady && ObjectManager.Target.GetDistance < 40)
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
            && ObjectManager.Target.GetDistance < 40)
        {
            Berserking.Launch();
            return;
        }
        else if (Blood_Fury.IsSpellUsable && Blood_Fury.KnownSpell && MySettings.UseBloodFury
            && ObjectManager.Target.GetDistance < 40)
        {
            Blood_Fury.Launch();
            return;
        }
        else if (Lifeblood.IsSpellUsable && Lifeblood.KnownSpell && MySettings.UseLifeblood
            && ObjectManager.Target.GetDistance < 40)
        {
            Lifeblood.Launch();
            return;
        }
        else if (MySettings.UseEngGlove && Engineering.KnownSpell && Engineering_Timer.IsReady
            && ObjectManager.Target.GetDistance < 40)
        {
            Logging.WriteFight("Use Engineering Gloves.");
            Lua.RunMacroText("/use 10");
            Engineering_Timer = new Timer(1000*60);
            return;
        }
        else if (Mage_Bomb.KnownSpell && Mage_Bomb.IsSpellUsable && Mage_Bomb.IsDistanceGood
            && MySettings.UseTierFive)
        {
            Mage_Bomb.Launch();
            return;
        }
        else if (Evocation.KnownSpell && Evocation.IsSpellUsable && ObjectManager.Target.GetDistance < 40
            && MySettings.UseInvocationTalent && !ObjectManager.Me.HaveBuff(114003))
        {
            Evocation.Launch();
            return;
        }
        else if (Evocation.KnownSpell && Evocation.IsSpellUsable && ObjectManager.Target.GetDistance < 40
            && MySettings.UseRuneofPowerTalent && !ObjectManager.Me.HaveBuff(116011))
        {
            SpellManager.CastSpellByIDAndPosition(116011, ObjectManager.Target.Position);
            return;
        }
        else if (Combustion.KnownSpell && Combustion.IsSpellUsable && Combustion.IsDistanceGood
            && MySettings.UseCombustion && ObjectManager.Target.HaveBuff(12654) 
            && ObjectManager.Target.HaveBuff(11366))
        {
            Combustion.Launch();
            return;
        }
        else if (Mirror_Image.KnownSpell && Mirror_Image.IsSpellUsable && ObjectManager.Target.GetDistance < 40
            && MySettings.UseMirrorImage)
        {
            Mirror_Image.Launch();
            return;
        }
        else
        {
            if (Time_Warp.KnownSpell && Time_Warp.IsSpellUsable && MySettings.UseTimeWarp
                && !ObjectManager.Me.HaveBuff(80354) && ObjectManager.Target.GetDistance < 40)
            {
                Time_Warp.Launch();
                return;
            }
        }
    }

    public void DPS_Cycle()
    {
        if (Scorch.IsSpellUsable && MySettings.UseScorch && Scorch.IsDistanceGood
            && Scorch.KnownSpell && ObjectManager.Me.GetMove && !Ice_Floes.HaveBuff)
        {
            Scorch.Launch();
            return;
        }

        if (ObjectManager.GetNumberAttackPlayer() > 4 && Flamestrike.IsSpellUsable && Flamestrike.KnownSpell
            && Flamestrike.IsDistanceGood && Flamestrike_Timer.IsReady && MySettings.UseFlamestrike)
        {
            SpellManager.CastSpellByIDAndPosition(2120, ObjectManager.Target.Position);
            Flamestrike_Timer = new Timer(1000*8);
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 4 && Arcane_Explosion.IsSpellUsable && Arcane_Explosion.KnownSpell
            && Arcane_Explosion.IsDistanceGood && MySettings.UseArcaneExplosion)
        {
            Arcane_Explosion.Launch();
            return;
        }
        else if (Pyroblast.KnownSpell && Pyroblast.IsSpellUsable && Pyroblast.IsDistanceGood
            && ObjectManager.Me.HaveBuff(48108) && MySettings.UsePyroblast)
        {
            Pyroblast.Launch();
            return;
        }
        else if (Pyroblast.KnownSpell && Pyroblast.IsSpellUsable && Pyroblast.IsDistanceGood
            && Presence_of_Mind.KnownSpell && Presence_of_Mind.IsSpellUsable
            && !ObjectManager.Me.HaveBuff(48108) && MySettings.UsePyroblast
            && MySettings.UsePresenceofMind)
        {
            Presence_of_Mind.Launch();
            Thread.Sleep(400);
            Pyroblast.Launch();
            return;
        }
        //Blizzard API calls for Inferno Blast using the Frostfire Bolt function.
        else if (Fire_Blast.IsDistanceGood && Fire_Blast.IsSpellUsable && Fire_Blast.KnownSpell
            && MySettings.UseInfernoBlast && ObjectManager.Me.HaveBuff(48107))
        {
            Fire_Blast.Launch();
            return;
        }
        else
        {
            if (Fireball.KnownSpell && Fireball.IsSpellUsable && Fireball.IsDistanceGood
                && MySettings.UseFireball)
            {
                Fireball.Launch();
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

        if (ObjectManager.Me.HaveBuff(87023) && Ice_Block.KnownSpell && MySettings.UseIceBlock
            && !ObjectManager.Me.HaveBuff(41425))
        {
            if (!Ice_Block.IsSpellUsable && Cold_Snap.KnownSpell && Cold_Snap.IsSpellUsable
                && MySettings.UseColdSnap)
            {
                Cold_Snap.Launch();
                Thread.Sleep(400);
            }
            Ice_Block.Launch();
            OnCD = new Timer(1000*10);
            return;
        }

        if (MySettings.UseArcaneBrilliance && Arcane_Brilliance.KnownSpell && Arcane_Brilliance.IsSpellUsable 
            && !Arcane_Brilliance.HaveBuff && !ObjectManager.Me.HaveBuff(61316))
        {
            Arcane_Brilliance.Launch();
            return;
        }
        else if (MySettings.UseMoltenArmor && Molten_Armor.KnownSpell && Molten_Armor.IsSpellUsable 
            && !Molten_Armor.HaveBuff)
        {
            Molten_Armor.Launch();
            return;
        }
        else if (MySettings.UseFrostArmor && Frost_Armor.KnownSpell && Frost_Armor.IsSpellUsable 
            && !Frost_Armor.HaveBuff && !MySettings.UseMoltenArmor)
        {
            Frost_Armor.Launch();
            return;
        }
        else if (MySettings.UseMageArmor && Mage_Armor.KnownSpell && Mage_Armor.IsSpellUsable 
            && !Mage_Armor.HaveBuff && !MySettings.UseFrostArmor && !MySettings.UseMoltenArmor)
        {
            Mage_Armor.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 0 && Ice_Floes.IsSpellUsable 
            && Ice_Floes.KnownSpell && MySettings.UseIceFloes && ObjectManager.Me.GetMove)
        {
            Ice_Floes.Launch();
            return;
        }
        else
        {
            if (ObjectManager.GetNumberAttackPlayer() == 0 && Blazing_Speed.IsSpellUsable && Blazing_Speed.KnownSpell
                && MySettings.UseBlazingSpeed && ObjectManager.Me.GetMove)
            {
                Blazing_Speed.Launch();
                return;
            }
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (ObjectManager.Me.ManaPercentage < 80 && Arcane_Torrent.KnownSpell && Arcane_Torrent.IsSpellUsable
            && MySettings.UseArcaneTorrent)
        {
            Arcane_Torrent.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && Gift_of_the_Naaru.KnownSpell && Gift_of_the_Naaru.IsSpellUsable
            && MySettings.UseGiftoftheNaaru)
        {
            Gift_of_the_Naaru.Launch();
            return;
        }
        else if (ObjectManager.Me.ManaPercentage < 40 && ItemsManager.GetItemCountByIdLUA(36799) > 0
            && MySettings.UseConjureManaGem)
        {
            Logging.WriteFight("Use Mana Gem.");
            Lua.RunMacroText("/use item:36799");
            return;
        }
        else if ((ObjectManager.Me.HealthPercent < 40 || ObjectManager.Me.ManaPercentage < 60)
            && MySettings.UseEvocation && Evocation.IsSpellUsable && !MySettings.UseInvocationTalent
            && !MySettings.UseRuneofPowerTalent && MySettings.UseEvocationGlyph)
        {
            Evocation.Launch();
            return;
        }
        else if (Conjure_Mana_Gem.KnownSpell && ItemsManager.GetItemCountByIdLUA(36799) == 0
            && MySettings.UseConjureManaGem)
        {
            Conjure_Mana_Gem.Launch();
            return;
        }
        else
        {
            if (Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(80610) == 0 // 90
                && Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(65499) == 0 // 85-89
                && Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(43523) == 0 // 84-80
                && Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(43518) == 0 // 79-74
                && Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(65517) == 0 // 73-64
                && Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(65516) == 0 // 63-54
                && Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(65515) == 0 // 53-44
                && Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(65500) == 0 // 43-38
                && MySettings.UseConjureRefreshment)
            {
                Conjure_Refreshment.Launch();
                return;
            }
        }
    }

    private void Defense_Cycle()
    {
        if (ObjectManager.Me.HealthPercent < 100 && Incanters_Ward.IsSpellUsable && Incanters_Ward.KnownSpell
            && MySettings.UseIncantersWard && !Incanters_Ward.HaveBuff && ObjectManager.GetNumberAttackPlayer() > 0)
        {
            Incanters_Ward.Launch();
            return;
        }

        if (Ring_of_Frost.KnownSpell && Ring_of_Frost.IsSpellUsable
            && ObjectManager.GetNumberAttackPlayer() > 2
            && ObjectManager.Target.GetDistance < 10 && MySettings.UseRingofFrost)
        {
            SpellManager.CastSpellByIDAndPosition(113724, ObjectManager.Target.Position);
            return;
        }
        else if (Frost_Nova.KnownSpell && ObjectManager.Target.GetDistance < 12
            && ObjectManager.Me.HealthPercent < 50 && MySettings.UseFrostNova)
        {
            if (!Frost_Nova.IsSpellUsable && Cold_Snap.KnownSpell && Cold_Snap.IsSpellUsable
                && MySettings.UseColdSnap)
            {
                Cold_Snap.Launch();
                Thread.Sleep(200);
            }

            if (Frost_Nova.IsSpellUsable)
            {
                Frost_Nova.Launch();
                return;
            }
        }
        else if (Ice_Ward.KnownSpell && Ice_Ward.IsSpellUsable && ObjectManager.Target.GetDistance < 10
            && ObjectManager.Me.HealthPercent < 45 && MySettings.UseIceWard && !Frost_Nova.IsSpellUsable)
        {
            Ice_Ward.Launch();
            return;
        }
        else if (Cone_of_Cold.KnownSpell && Cone_of_Cold.IsSpellUsable && ObjectManager.Target.GetDistance < 10
            && ObjectManager.Me.HealthPercent < 45 && MySettings.UseConeofCold && !Frost_Nova.IsSpellUsable
            && !Ice_Ward.IsSpellUsable)
        {
            Cone_of_Cold.Launch();
            return;
        }
        else if (Blink.KnownSpell && Blink.IsSpellUsable && ObjectManager.Target.GetDistance < 11
            && (Frost_Nova.TargetHaveBuff || Cone_of_Cold.TargetHaveBuff || Ice_Ward.TargetHaveBuff))
        {
            Blink.Launch();
            return;
        }
        else if (Deep_Freeze.KnownSpell && Deep_Freeze.IsSpellUsable && Deep_Freeze.IsDistanceGood
            && MySettings.UseDeepFreeze && ObjectManager.Me.HealthPercent < 50)
        {
            Deep_Freeze.Launch();
            OnCD = new Timer(1000*5);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 95 && Ice_Barrier.IsSpellUsable && Ice_Barrier.KnownSpell
            && MySettings.UseIceBarrier && !Ice_Barrier.HaveBuff && !Incanters_Ward.HaveBuff)
        {
            Ice_Barrier.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 95 && Temporal_Shield.IsSpellUsable && Temporal_Shield.KnownSpell
            && MySettings.UseTemporalShield && !Temporal_Shield.HaveBuff && ObjectManager.GetNumberAttackPlayer() > 0)
        {
            Temporal_Shield.Launch();
            OnCD = new Timer(1000*4);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 40 && Frost_Jaw.KnownSpell && Frost_Jaw.IsSpellUsable 
            && MySettings.UseFrostJaw && Frost_Jaw.IsDistanceGood)
        {
            Frost_Jaw.Launch();
            OnCD = new Timer(1000*8);
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
            if (ObjectManager.GetNumberAttackPlayer() > 3 && Invisibility.KnownSpell && Invisibility.IsSpellUsable
                && MySettings.UseInvisibility)
            {
                Invisibility.Launch();
                Thread.Sleep(5000);
                return;
            }
        }
    }

    private void Decast()
    {
        if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe
            && Counterspell.KnownSpell && Counterspell.IsSpellUsable && Counterspell.IsDistanceGood)
        {
            Counterspell.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe
                && Frost_Jaw.KnownSpell && Frost_Jaw.IsSpellUsable && Frost_Jaw.IsDistanceGood)
            {
                Frost_Jaw.Launch();
                OnCD = new Timer(1000*8);
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