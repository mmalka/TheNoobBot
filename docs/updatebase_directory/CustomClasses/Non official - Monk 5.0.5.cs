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
using nManager.Wow;
using nManager.Wow.Class;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;
using Keybindings = nManager.Wow.Helpers.Keybindings;
using Point = System.Drawing.Point;
using Timer = nManager.Helpful.Timer;

public class Main : ICustomClass
{
    internal static float range = 5.0f;
    internal static bool loop = true;

    #region ICustomClass Members

    public float Range
    {
        get { return range; }
        set { range = value; }
    }

    public void Initialize()
    {
        Initialize(false);
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

    public void ResetConfiguration()
    {
        Directory.CreateDirectory(Application.StartupPath + "\\CustomClasses\\Settings\\");
        Initialize(true, true);
    }

    #endregion

    public void Initialize(bool ConfigOnly, bool ResetSettings = false)
    {
        try
        {
            if (!loop)
                loop = true;
            Logging.WriteFight("Loading combat system.");
            switch (ObjectManager.Me.WowClass)
            {

                #region Monk Specialisation checking

                case WoWClass.Monk:
                    var Monk_Brewmaster_Spell = new Spell("Dizzying Haze");
                    var Monk_Windwalker_Spell = new Spell("Fists of Fury");
                    var Monk_Mistweaver_Spell = new Spell("Soothing Mist");
                    if (Monk_Brewmaster_Spell.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Monk_Brewmaster.xml";
                            Monk_Brewmaster.MonkBrewmasterSettings CurrentSetting;
                            CurrentSetting = new Monk_Brewmaster.MonkBrewmasterSettings();
                            if (File.Exists(CurrentSettingsFile) && !ResetSettings)
                            {
                                CurrentSetting =
                                    Settings.Load<Monk_Brewmaster.MonkBrewmasterSettings>(CurrentSettingsFile);
                            }
                            CurrentSetting.ToForm();
                            CurrentSetting.Save(CurrentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Monk Brewmaster class...");
                            new Monk_Brewmaster();
                        }
                    }
                    else if (Monk_Windwalker_Spell.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Monk_Windwalker.xml";
                            Monk_Windwalker.MonkWindwalkerSettings CurrentSetting;
                            CurrentSetting = new Monk_Windwalker.MonkWindwalkerSettings();
                            if (File.Exists(CurrentSettingsFile) && !ResetSettings)
                            {
                                CurrentSetting =
                                    Settings.Load<Monk_Windwalker.MonkWindwalkerSettings>(CurrentSettingsFile);
                            }
                            CurrentSetting.ToForm();
                            CurrentSetting.Save(CurrentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Monk Windwalker class...");
                            new Monk_Windwalker();
                        }
                    }
                    else if (Monk_Mistweaver_Spell.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Monk_Mistweaver.xml";
                            Monk_Mistweaver.MonkMistweaverSettings CurrentSetting;
                            CurrentSetting = new Monk_Mistweaver.MonkMistweaverSettings();
                            if (File.Exists(CurrentSettingsFile) && !ResetSettings)
                            {
                                CurrentSetting =
                                    Settings.Load<Monk_Mistweaver.MonkMistweaverSettings>(CurrentSettingsFile);
                            }
                            CurrentSetting.ToForm();
                            CurrentSetting.Save(CurrentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Monk Mistweaver class...");
                            range = 30.0f;
                            new Monk_Mistweaver();
                        }
                    }
                    else
                    {
                        if (ConfigOnly)
                        {
                            MessageBox.Show(
                                "Your specification haven't be found, loading Monk Brewmaster Settings");
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Monk_Brewmaster.xml";
                            Monk_Brewmaster.MonkBrewmasterSettings CurrentSetting;
                            CurrentSetting = new Monk_Brewmaster.MonkBrewmasterSettings();
                            if (File.Exists(CurrentSettingsFile) && !ResetSettings)
                            {
                                CurrentSetting =
                                    Settings.Load<Monk_Brewmaster.MonkBrewmasterSettings>(CurrentSettingsFile);
                            }
                            CurrentSetting.ToForm();
                            CurrentSetting.Save(CurrentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("No specialisation detected.");
                            Logging.WriteFight("Loading Monk Brewmaster class...");
                            new Monk_Brewmaster();
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
}

#region Monk

public class Monk_Brewmaster
{
    private readonly MonkBrewmasterSettings MySettings = MonkBrewmasterSettings.GetSettings();
    private readonly string MoveBackward = nManager.Wow.Helpers.Keybindings.GetKeyByAction(nManager.Wow.Enums.Keybindings.MOVEBACKWARD);
    private readonly string MoveForward = nManager.Wow.Helpers.Keybindings.GetKeyByAction(nManager.Wow.Enums.Keybindings.MOVEFORWARD);

    #region General Timers & Variables

    private Timer AlchFlask_Timer = new Timer(0);
    private Timer Engineering_Timer = new Timer(0);
    private Timer OnCD = new Timer(0);
    private Timer Trinket_Timer = new Timer(0);
    private Timer Grapple_Weapon_Timer = new Timer(0);
    private Timer Healing_Sphere_Timer = new Timer(0);
    private Timer Stagger_Timer = new Timer(0);
    public int Elusive_Brew_Stack = 0;

    #endregion

    #region Professions & Racials

    private readonly Spell Alchemy = new Spell("Alchemy");
    private readonly Spell Arcane_Torrent = new Spell("Arcane Torrent");
    private readonly Spell Berserking = new Spell("Berserking");
    private readonly Spell Blood_Fury = new Spell("Blood Fury");
    private readonly Spell Engineering = new Spell("Engineering");
    private readonly Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    private readonly Spell Lifeblood = new Spell("Lifeblood");
    private readonly Spell Stoneform = new Spell("Stoneform");
    private readonly Spell War_Stomp = new Spell("War Stomp");

    #endregion

    #region Monk Buffs

    private readonly Spell Disable = new Spell("Disable");
    private readonly Spell Legacy_of_the_Emperor = new Spell("Legacy of the Emperor");
    private readonly Spell Stance_of_the_Fierce_Tiger = new Spell("Stance of the Fierce Tiger");
    private readonly Spell Stance_of_the_Sturdy_Ox = new Spell("Stance of the Sturdy Ox");
    private readonly Spell Tigers_Lust = new Spell("Tiger's Lust");

    #endregion

    #region Offensive Spell

    private readonly Spell Blackout_Kick = new Spell("Blackout Kick");
    private readonly Spell Breath_of_Fire = new Spell("Breathe of Fire");
    private readonly Spell Clash = new Spell("Clash");
    private readonly Spell Crackling_Jade_Lightning = new Spell("Crackling Jade Lightning");
    private readonly Spell Dizzying_Haze = new Spell("Dizzying Haze");
    private readonly Spell Jab = new Spell("Jab");
    private readonly Spell Keg_Smash = new Spell("Keg Smash");
    private readonly Spell Provoke = new Spell("Provoke");
    private readonly Spell Roll = new Spell("Roll");
    private readonly Spell Spinning_Crane_Kick = new Spell("Spinning Crane Kick");
    private readonly Spell Tiger_Palm = new Spell("Tiger Palm");
    private readonly Spell Touch_of_Death = new Spell("Touch of Death");

    #endregion

    #region Offensive Cooldown

    private readonly Spell Chi_Brew = new Spell("Chi Brew");
    private readonly Spell Invoke_Xuen_the_White_Tiger = new Spell("Invoke Xuen, the White Tiger");
    private readonly Spell Rushing_Jade_Wind = new Spell("Rushing Jade Wind");

    #endregion

    #region Defensive Cooldown

    private readonly Spell Charging_Ox_Wave = new Spell("Charging Ox Wave");
    private readonly Spell Dampen_Harm = new Spell("Dampen Harm");
    private readonly Spell Diffuse_Magic = new Spell("Diffuse Magic");
    private readonly Spell Elusive_Brew = new Spell("Elusive Brew");
    private readonly Spell Fortifying_Brew = new Spell("Fortifying Brew");
    private readonly Spell Guard = new Spell("Guard");
    private readonly Spell Grapple_Weapon = new Spell("Grapple Weapon");
    private readonly Spell Leg_Sweep = new Spell("Leg Sweep");
    private readonly Spell Spear_Hand_Strike = new Spell("Spear Hand Strike");
    private readonly Spell Summon_Black_Ox_Statue = new Spell("Summon Black Ox Statue");
    private readonly Spell Zen_Meditation = new Spell("Zen Meditation");

    #endregion

    #region Healing Spell

    private readonly Spell Chi_Burst = new Spell("Chi Burst");
    private readonly Spell Chi_Wave = new Spell("Chi Wave");
    private readonly Spell Expel_Harm = new Spell("Expel Harm");
    private readonly Spell Healing_Sphere = new Spell("Healing Sphere");
    private readonly Spell Zen_Sphere = new Spell("Zen Sphere");

    #endregion

    public Monk_Brewmaster()
    {
        Main.range = 5.0f;
        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            try
            {
                if (!ObjectManager.Me.IsDead)
                {
                    if (!ObjectManager.Me.IsMounted)
                    {
                        if (Fight.InFight && ObjectManager.Me.Target > 0)
                        {
                            if (ObjectManager.Me.Target != lastTarget &&
                            Provoke.IsDistanceGood)
                            {
                                Pull();
                                lastTarget = ObjectManager.Me.Target;
                            }

                            if (ObjectManager.Target.GetDistance < 41)
                                Combat();
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
            Thread.Sleep(150);
        }
    }

    private void Pull()
    {
        if (Clash.IsSpellUsable && Clash.IsDistanceGood && MySettings.UseClash && Clash.KnownSpell)
            Clash.Launch();

        if (!ObjectManager.Target.InCombat && Provoke.IsSpellUsable && Provoke.IsDistanceGood
            && MySettings.UseProvoke && Provoke.KnownSpell)
        {
            Provoke.Launch();
            return;
        }
    }

    private void Combat()
    {
        Buff();
        AvoidMelee();
        if (OnCD.IsReady)
            Defense_Cycle();
        Heal();
        Decast();
        DPS_Burst();
        DPS_Cycle();
    }

    private void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (Legacy_of_the_Emperor.KnownSpell && Legacy_of_the_Emperor.IsSpellUsable &&
            !Legacy_of_the_Emperor.HaveBuff && MySettings.UseLegacyoftheEmperor)
        {
            Legacy_of_the_Emperor.Launch();
            return;
        }
        else if (Stance_of_the_Sturdy_Ox.KnownSpell && MySettings.UseStanceoftheSturdyOx 
            && !Stance_of_the_Sturdy_Ox.HaveBuff && Stance_of_the_Sturdy_Ox.IsSpellUsable)
        {
            Stance_of_the_Sturdy_Ox.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() == 0 && Tigers_Lust.IsSpellUsable && Tigers_Lust.KnownSpell
                && MySettings.UseTigersLust && ObjectManager.Me.GetMove)
        {
            Tigers_Lust.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() == 0 && Roll.IsSpellUsable && Roll.KnownSpell
                && MySettings.UseRoll && ObjectManager.Me.GetMove && !Tigers_Lust.HaveBuff
                && ObjectManager.Target.GetDistance > 14)
        {
            Roll.Launch();
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
            }
        }
    }

    private void AvoidMelee()
    {
        if (ObjectManager.Target.GetDistance < 2 && ObjectManager.Target.InCombat)
        {
            Logging.WriteFight("Too Close. Moving Back");
            var MaxTime_Timer = new Timer(1000 * 2);
            Keyboard.DownKey(Memory.WowProcess.MainWindowHandle, MoveBackward);
            while (ObjectManager.Target.GetDistance < 2 && ObjectManager.Target.InCombat && !MaxTime_Timer.IsReady)
                Thread.Sleep(300);
            Keyboard.UpKey(Memory.WowProcess.MainWindowHandle, MoveBackward);
            if (MaxTime_Timer.IsReady && ObjectManager.Target.GetDistance < 2 && ObjectManager.Target.InCombat)
            {
                Keyboard.DownKey(Memory.WowProcess.MainWindowHandle, MoveForward);
                Thread.Sleep(1000);
                Keyboard.UpKey(Memory.WowProcess.MainWindowHandle, MoveForward);
                MovementManager.Face(ObjectManager.Target.Position);
            }
        }
    }

    private void Defense_Cycle()
    {
        if (ObjectManager.Me.HealthPercent < 95 && MySettings.UseGrappleWeapon && Grapple_Weapon.IsDistanceGood
            && Grapple_Weapon.KnownSpell && Grapple_Weapon.IsSpellUsable && Grapple_Weapon_Timer.IsReady)
        {
            Grapple_Weapon.Launch();
            Grapple_Weapon_Timer = new Timer(1000*60);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 70 && Elusive_Brew.IsSpellUsable && Elusive_Brew.KnownSpell
            && MySettings.UseElusiveBrew && ObjectManager.Me.InCombat && ObjectManager.Me.BuffStack(128939) > 5)
        {
            Elusive_Brew_Stack = ObjectManager.Me.BuffStack(128939);
            Elusive_Brew.Launch();
            OnCD = new Timer(1000*Elusive_Brew_Stack);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 75 && Summon_Black_Ox_Statue.IsSpellUsable && Summon_Black_Ox_Statue.KnownSpell
            && MySettings.UseSummonBlackOxStatue && Summon_Black_Ox_Statue.IsDistanceGood && !Summon_Black_Ox_Statue.HaveBuff)
        {
            SpellManager.CastSpellByIDAndPosition(115315, ObjectManager.Target.Position);
            OnCD = new Timer(1000*5);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && Fortifying_Brew.IsSpellUsable && Fortifying_Brew.KnownSpell
            && MySettings.UseFortifyingBrew)
        {
            Fortifying_Brew.Launch();
            OnCD = new Timer(1000*20);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 90 && Charging_Ox_Wave.IsSpellUsable && Charging_Ox_Wave.KnownSpell
            && MySettings.UseChargingOxWave && Charging_Ox_Wave.IsDistanceGood)
        {
            Charging_Ox_Wave.Launch();
            OnCD = new Timer(1000*3);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 90 && Dampen_Harm.IsSpellUsable && Dampen_Harm.KnownSpell
            && MySettings.UseDampenHarm)
        {
            Dampen_Harm.Launch();
            OnCD = new Timer(1000*5);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 90 && Leg_Sweep.IsSpellUsable && Leg_Sweep.KnownSpell
            && MySettings.UseLegSweep && ObjectManager.Target.GetDistance < 6)
        {
            Leg_Sweep.Launch();
            OnCD = new Timer(1000*5);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 95 && Guard.IsSpellUsable && Guard.KnownSpell
            && MySettings.UseGuard && ObjectManager.Me.BuffStack(118636) > 2)
        {
            Guard.Launch();
            OnCD = new Timer(1000*5);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 95 && Elusive_Brew.IsSpellUsable && Elusive_Brew.KnownSpell
            && MySettings.UseElusiveBrew && ObjectManager.Me.BuffStack(128939) > 14
            && ObjectManager.Target.HealthPercent > 75)
        {
            Elusive_Brew_Stack = ObjectManager.Me.BuffStack(128939);
            Elusive_Brew.Launch();
            OnCD = new Timer(1000*Elusive_Brew_Stack);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && Zen_Meditation.IsSpellUsable && Zen_Meditation.KnownSpell
                 && MySettings.UseZenMeditation)
        {
            Zen_Meditation.Launch();
            OnCD = new Timer(1000*8);
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
            if (ObjectManager.Me.HealthPercent < 80 && War_Stomp.IsSpellUsable && War_Stomp.KnownSpell
                && MySettings.UseWarStomp)
            {
                War_Stomp.Launch();
                OnCD = new Timer(1000*2);
                return;
            }
        }
    }

    private void Heal()
    {
        if (Healing_Sphere.KnownSpell && Healing_Sphere.IsSpellUsable && ObjectManager.Me.Energy > 39 &&
            ObjectManager.Me.HealthPercent < 70 && MySettings.UseHealingSphere && Healing_Sphere_Timer.IsReady)
        {
            SpellManager.CastSpellByIDAndPosition(115460, ObjectManager.Me.Position);
            Healing_Sphere_Timer = new Timer(1000*5);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 85 && Chi_Wave.KnownSpell && Chi_Wave.IsSpellUsable 
                && MySettings.UseChiWave)
        {
            Chi_Wave.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 90 && Chi_Burst.KnownSpell && Chi_Burst.IsSpellUsable
                 && MySettings.UseChiBurst)
        {
            Chi_Burst.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 90 && Expel_Harm.KnownSpell && Expel_Harm.IsSpellUsable
                 && MySettings.UseExpelHarm && Expel_Harm.IsDistanceGood)
        {
            Expel_Harm.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Me.HealthPercent < 95 && Zen_Sphere.KnownSpell && Zen_Sphere.IsSpellUsable
                 && MySettings.UseZenSphere)
            {
                Zen_Sphere.Launch();
                return;
            }
        }
    }

    private void Decast()
    {
        if (Arcane_Torrent.KnownSpell && MySettings.UseArcaneTorrent && Arcane_Torrent.IsSpellUsable 
            && ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && ObjectManager.Target.GetDistance < 8)
        {
            Arcane_Torrent.Launch();
            return;
        }
        else if (Diffuse_Magic.KnownSpell && MySettings.UseDiffuseMagic && Diffuse_Magic.IsSpellUsable 
            && ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe)
        {
            Diffuse_Magic.Launch();
            return;
        }
        else
        {
            if (Spear_Hand_Strike.KnownSpell && MySettings.UseSpearHandStrike && ObjectManager.Target.IsCast 
                && Spear_Hand_Strike.IsSpellUsable && Spear_Hand_Strike.IsDistanceGood)
            {
                Spear_Hand_Strike.Launch();
                return;
            }
        }

        if (ObjectManager.Target.GetMove && !Disable.TargetHaveBuff && MySettings.UseDisable
            && Disable.KnownSpell && Disable.IsSpellUsable && Disable.IsDistanceGood)
        {
            Disable.Launch();
            return;
        }
    }

    private void DPS_Burst()
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
        }
        else if (Berserking.IsSpellUsable && Berserking.KnownSpell && ObjectManager.Target.GetDistance < 30
                 && MySettings.UseBerserking)
            Berserking.Launch();
        else if (Blood_Fury.IsSpellUsable && Blood_Fury.KnownSpell && ObjectManager.Target.GetDistance < 30
                 && MySettings.UseBloodFury)
            Blood_Fury.Launch();
        else if (Lifeblood.IsSpellUsable && Lifeblood.KnownSpell && ObjectManager.Target.GetDistance < 30
                 && MySettings.UseLifeblood)
            Lifeblood.Launch();
        else if (Engineering_Timer.IsReady && Engineering.KnownSpell && ObjectManager.Target.GetDistance < 30
                && MySettings.UseEngGlove)
        {
            Logging.WriteFight("Use Engineering Gloves.");
            Lua.RunMacroText("/use 10");
            Engineering_Timer = new Timer(1000*60);
        }
        else if (Chi_Brew.IsSpellUsable && Chi_Brew.KnownSpell
                 && MySettings.UseChiBrew && ObjectManager.Me.Chi == 0)
        {
            Chi_Brew.Launch();
            return;
        }
        else if (Touch_of_Death.IsSpellUsable && Touch_of_Death.KnownSpell && Touch_of_Death.IsDistanceGood
                 && MySettings.UseTouchofDeath)
        {
            Touch_of_Death.Launch();
            return;
        }
        else if (Invoke_Xuen_the_White_Tiger.IsSpellUsable && Invoke_Xuen_the_White_Tiger.KnownSpell
                 && MySettings.UseInvokeXuentheWhiteTiger && Invoke_Xuen_the_White_Tiger.IsDistanceGood)
        {
            Invoke_Xuen_the_White_Tiger.Launch();
            return;
        }
        else
        {
            if (Rushing_Jade_Wind.IsSpellUsable && Rushing_Jade_Wind.KnownSpell && Rushing_Jade_Wind.IsDistanceGood
                && MySettings.UseRushingJadeWind && ObjectManager.GetNumberAttackPlayer() > 3)
            {
                Rushing_Jade_Wind.Launch();
                return;
            }
        }
    }

    private void DPS_Cycle()
    {
        if (ObjectManager.GetNumberAttackPlayer() > 5 && Spinning_Crane_Kick.IsSpellUsable && Spinning_Crane_Kick.KnownSpell
                 && Spinning_Crane_Kick.IsDistanceGood && !ObjectManager.Me.IsCast && MySettings.UseSpinningCraneKick)
        {
            Spinning_Crane_Kick.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 2 && Dizzying_Haze.IsSpellUsable && Dizzying_Haze.KnownSpell
                 && Dizzying_Haze.IsDistanceGood && !Dizzying_Haze.TargetHaveBuff && MySettings.UseDizzyingHaze)
        {
            Dizzying_Haze.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 2 && Breath_of_Fire.IsSpellUsable && Breath_of_Fire.KnownSpell
                 && Breath_of_Fire.IsDistanceGood && !Breath_of_Fire.TargetHaveBuff && MySettings.UseBreathofFire)
        {
            Breath_of_Fire.Launch();
            return;
        }
        else if (Rushing_Jade_Wind.KnownSpell && Rushing_Jade_Wind.IsSpellUsable && Rushing_Jade_Wind.IsDistanceGood
                 && MySettings.UseRushingJadeWind && (!ObjectManager.Target.HaveBuff(115307)
                 || Stagger_Timer.IsReady))
        {
            Rushing_Jade_Wind.Launch();
            Stagger_Timer = new Timer(1000*4);
            return;
        }
        else if (Blackout_Kick.KnownSpell && Blackout_Kick.IsSpellUsable && Blackout_Kick.IsDistanceGood
                 && MySettings.UseBlackoutKick && !Rushing_Jade_Wind.KnownSpell && !ObjectManager.Me.HaveBuff(121125)
                 && (!ObjectManager.Target.HaveBuff(115307) || Stagger_Timer.IsReady))
        {
            Blackout_Kick.Launch();
            Stagger_Timer = new Timer(1000*4);
            return;
        }
        else if (Keg_Smash.KnownSpell && Keg_Smash.IsSpellUsable && Keg_Smash.IsDistanceGood
                 && MySettings.UseKegSmash && ObjectManager.Me.Chi < 3)
        {
            Keg_Smash.Launch();
            return;
        }
        else if (Tiger_Palm.KnownSpell && Tiger_Palm.IsSpellUsable && Tiger_Palm.IsDistanceGood && !ObjectManager.Me.HaveBuff(121125)
                 && MySettings.UseTigerPalm && ObjectManager.Target.BuffStack(125359) < 3)
        {
            Tiger_Palm.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 91 && Expel_Harm.KnownSpell && Expel_Harm.IsSpellUsable
                 && MySettings.UseExpelHarm && ObjectManager.Me.Chi < 4 && Expel_Harm.IsDistanceGood)
        {
            Expel_Harm.Launch();
            return;
        }
        else if (Jab.KnownSpell && Jab.IsSpellUsable && MySettings.UseJab && !ObjectManager.Me.HaveBuff(116768)
                && ObjectManager.Me.Chi < 4 && ObjectManager.Me.HealthPercent > 90 && !ObjectManager.Me.HaveBuff(118864)
                && Jab.IsDistanceGood)
        {
            Jab.Launch();
            return;
        }
        else
        {
            if (Tiger_Palm.KnownSpell && Tiger_Palm.IsSpellUsable && Tiger_Palm.IsDistanceGood && ObjectManager.Target.HaveBuff(115307)
                 && MySettings.UseTigerPalm && ObjectManager.Me.Chi < 2 && !ObjectManager.Me.HaveBuff(121125))
            {
                Tiger_Palm.Launch();
                return;
            }
        }
    }

    private void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            Buff();
            Heal();
        }
    }
    
    #region Nested type: MonkBrewmasterSettings

    [Serializable]
    public class MonkBrewmasterSettings : Settings
    {
        public bool UseAlchFlask = true;
        public bool UseArcaneTorrent = true;
        public bool UseBerserking = true;
        public bool UseBlackoutKick = true;
        public bool UseBloodFury = true;
        public bool UseBreathofFire = true;
        public bool UseChargingOxWave = true;
        public bool UseChiBrew = true;
        public bool UseChiBurst = true;
        public bool UseChiWave = true;
        public bool UseClash = true;
        public bool UseCracklingJadeLightning = true;
        public bool UseDampenHarm = true;
        public bool UseDiffuseMagic = true;
        public bool UseDisable = false;
        public bool UseDizzyingHaze = true;
        public bool UseElusiveBrew = true;
        public bool UseEngGlove = true;
        public bool UseExpelHarm = true;
        public bool UseFortifyingBrew = true;
        public bool UseGuard = true;
        public bool UseGiftoftheNaaru = true;
        public bool UseGrappleWeapon = true;
        public bool UseHealingSphere = true;
        public bool UseInvokeXuentheWhiteTiger = true;
        public bool UseJab = true;
        public bool UseKegSmash = true;
        public bool UseLegSweep = true;
        public bool UseLegacyoftheEmperor = true;
        public bool UseLifeblood = true;
        public bool UseProvoke = true;
        public bool UseRoll = true;
        public bool UseRushingJadeWind = true;
        public bool UseSpearHandStrike = true;
        public bool UseSpinningCraneKick = true;
        public bool UseStanceoftheFierceTiger = true;
        public bool UseStanceoftheSturdyOx = true;
        public bool UseStoneform = true;
        public bool UseSummonBlackOxStatue = true;
        public bool UseTigerPalm = true;
        public bool UseTigersLust= true;
        public bool UseTouchofDeath= true;
        public bool UseTrinket = true;
        public bool UseWarStomp = true;
        public bool UseZenMeditation = true;
        public bool UseZenSphere = true;

        public MonkBrewmasterSettings()
        {
            ConfigWinForm(new Point(500, 400), "Brewmaster Monk Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");
            AddControlInWinForm("Use Lifeblood", "UseLifeblood", "Professions & Racials");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            /* Monk Buffs */
            AddControlInWinForm("Use Disable", "UseDisable", "Monk Buffs");
            AddControlInWinForm("Use Legacy of the Emperor", "UseLegacyoftheEmperor", "Monk Buffs");
            AddControlInWinForm("Use Stance of the Fierce Tiger", "UseStanceoftheFierceTiger", "Monk Buffs");
            AddControlInWinForm("Use Tiger's Lust", "UseTigersLust", "Monk Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Chi Wave", "UseChiWave", "Offensive Spell");
            AddControlInWinForm("Use Blackout Kick", "UseBlackoutKick", "Offensive Spell");
            AddControlInWinForm("Use Breath of Fire", "UseBreathofFire", "Offensive Spell");
            AddControlInWinForm("Use Clash", "UseClash", "Offensive Spell");
            AddControlInWinForm("Use Crackling Jade Lightning", "UseCracklingJadeLightning", "Offensive Spell");
            AddControlInWinForm("Use Dizzying Haze", "UseDizzyingHaze", "Offensive Spell");
            AddControlInWinForm("Use Jab", "UseJab", "Offensive Spell");
            AddControlInWinForm("Use Keg Smash", "UseKegSmash", "Offensive Spell");
            AddControlInWinForm("Use Provoke", "UseProvoke", "Offensive Spell");
            AddControlInWinForm("Use Roll", "UseRoll", "Offensive Spell");
            AddControlInWinForm("Use Spinning Crane Kick", "UseSpinningCraneKick", "Offensive Spell");
            AddControlInWinForm("Use Tiger Palm", "UseTigerPalm", "Offensive Spell");
            AddControlInWinForm("Use Touch of Death", "UseTouchofDeath", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use Chi Brew", "UseChiBrew", "Offensive Cooldown");
            AddControlInWinForm("Use Invoke Xuen, the White Tiger", "UseInvokeXuentheWhiteTiger", "Offensive Cooldown");
            AddControlInWinForm("Use Rushing Jade Wind", "UseRushingJadeWind", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Charging Ox Wave", "UseChargingOxWave", "Defensive Cooldown");
            AddControlInWinForm("Use Dampen Harm ", "UseDampenHarm ", "Defensive Cooldown");
            AddControlInWinForm("Use Diffuse Magic", "UseDiffuseMagic", "Defensive Cooldown");
            AddControlInWinForm("Use Elusive Brew", "UseElusiveBrew", "Defensive Cooldown");
            AddControlInWinForm("Use Fortifying Brew", "UseFortifyingBrew", "Defensive Cooldown");
            AddControlInWinForm("Use Grapple Weapon", "UseGrappleWeapon", "Defensive Cooldown");
            AddControlInWinForm("Use Guard", "UseGuard", "Defensive Cooldown");
            AddControlInWinForm("Use Leg Sweep", "UseLegSweep", "Defensive Cooldown");
            AddControlInWinForm("Use Spear Hand Strike", "UseSpearHandStrike", "Defensive Cooldown");
            AddControlInWinForm("Use Summon Black Ox Statue", "UseSummonBlackOxStatue", "Defensive Cooldown");
            AddControlInWinForm("Use Zen Meditation", "UseZenMeditation", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Chi Burst", "UseChiBurst", "Healing Spell");
            AddControlInWinForm("Use Expel Harm", "UseExpelHarm", "Healing Spell");
            AddControlInWinForm("Use Healing Sphere", "UseHealingSphere", "Healing Spell");
            AddControlInWinForm("Use Zen Sphere", "UseZenSphere", "Healing Spell");
            /* Game Settings */
            AddControlInWinForm("Use Trinket", "UseTrinket", "Game Settings");
            AddControlInWinForm("Use Engineering Gloves", "UseEngGlove", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
        }

        public static MonkBrewmasterSettings CurrentSetting { get; set; }

        public static MonkBrewmasterSettings GetSettings()
        {
            string CurrentSettingsFile = Application.StartupPath + "\\CustomClasses\\Settings\\Monk_Brewmaster.xml";
            if (File.Exists(CurrentSettingsFile))
            {
                return
                    CurrentSetting = Load<MonkBrewmasterSettings>(CurrentSettingsFile);
            }
            else
            {
                return new MonkBrewmasterSettings();
            }
        }
    }

    #endregion
}

public class Monk_Windwalker
{
    private readonly MonkWindwalkerSettings MySettings = MonkWindwalkerSettings.GetSettings();
    private readonly string MoveBackward = nManager.Wow.Helpers.Keybindings.GetKeyByAction(nManager.Wow.Enums.Keybindings.MOVEBACKWARD);
    private readonly string MoveForward = nManager.Wow.Helpers.Keybindings.GetKeyByAction(nManager.Wow.Enums.Keybindings.MOVEFORWARD);

    #region General Timers & Variables

    private Timer AlchFlask_Timer = new Timer(0);
    private Timer Engineering_Timer = new Timer(0);
    private Timer OnCD = new Timer(0);
    private Timer Trinket_Timer = new Timer(0);
    private Timer Tiger_Power_Timer = new Timer(0);
    private Timer Rising_Sun_Kick_Timer = new Timer(0);
    private Timer Healing_Sphere_Timer = new Timer(0);
    private Timer Grapple_Weapon_Timer = new Timer(0);

    #endregion

    #region Professions & Racials

    private readonly Spell Alchemy = new Spell("Alchemy");
    private readonly Spell Arcane_Torrent = new Spell("Arcane Torrent");
    private readonly Spell Berserking = new Spell("Berserking");
    private readonly Spell Blood_Fury = new Spell("Blood Fury");
    private readonly Spell Engineering = new Spell("Engineering");
    private readonly Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    private readonly Spell Lifeblood = new Spell("Lifeblood");
    private readonly Spell Stoneform = new Spell("Stoneform");
    private readonly Spell War_Stomp = new Spell("War Stomp");

    #endregion

    #region Monk Buffs

    private readonly Spell Disable = new Spell("Disable");
    private readonly Spell Legacy_of_the_Emperor = new Spell("Legacy of the Emperor");
    private readonly Spell Legacy_of_the_White_Tiger = new Spell("Legacy of the White Tiger");
    private readonly Spell Stance_of_the_Fierce_Tiger = new Spell("Stance of the Fierce Tiger");
    private readonly Spell Tigereye_Brew = new Spell("Tigereye Brew");
    private readonly Spell Tigers_Lust = new Spell("Tiger's Lust");

    #endregion

    #region Offensive Spell

    private readonly Spell Blackout_Kick = new Spell("Blackout Kick");
    private readonly Spell Crackling_Jade_Lightning = new Spell("Crackling Jade Lightning");
    private readonly Spell Fists_of_Fury = new Spell("Fists of Fury");
    private readonly Spell Jab = new Spell("Jab");
    private readonly Spell Provoke = new Spell("Provoke");
    private readonly Spell Rising_Sun_Kick = new Spell("Rising Sun Kick");
    private readonly Spell Roll = new Spell("Roll");
    private readonly Spell Spinning_Crane_Kick = new Spell("Spinning Crane Kick");
    private readonly Spell Tiger_Palm = new Spell("Tiger Palm");
    private readonly Spell Touch_of_Death = new Spell("Touch of Death");


    #endregion

    #region Offensive Cooldown

    private readonly Spell Chi_Brew = new Spell("Chi Brew");
    private readonly Spell Energizing_Brew = new Spell("Energizing Brew");
    private readonly Spell Invoke_Xuen_the_White_Tiger = new Spell("Invoke Xuen, the White Tiger");
    private readonly Spell Rushing_Jade_Wind = new Spell("Rushing Jade Wind");

    #endregion

    #region Defensive Cooldown

    private readonly Spell Charging_Ox_Wave = new Spell("Charging Ox Wave");
    private readonly Spell Dampen_Harm = new Spell("Dampen Harm");
    private readonly Spell Diffuse_Magic = new Spell("Diffuse Magic");
    private readonly Spell Fortifying_Brew = new Spell("Fortifying Brew");
    private readonly Spell Grapple_Weapon = new Spell("Grapple Weapon");
    private readonly Spell Leg_Sweep = new Spell("Leg Sweep");
    private readonly Spell Spear_Hand_Strike = new Spell("Spear Hand Strike");
    private readonly Spell Touch_of_Karma = new Spell("Touch of Karma");
    private readonly Spell Zen_Meditation = new Spell("Zen Meditation");

    #endregion

    #region Healing Spell

    private readonly Spell Chi_Burst = new Spell("Chi Burst");
    private readonly Spell Chi_Wave = new Spell("Chi Wave");
    private readonly Spell Expel_Harm = new Spell("Expel Harm");
    private readonly Spell Healing_Sphere = new Spell("Healing Sphere");
    private readonly Spell Zen_Sphere = new Spell("Zen Sphere");

    #endregion

    public Monk_Windwalker()
    {
        Main.range = 5.0f;
        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            try
            {
                if (!ObjectManager.Me.IsDead)
                {
                    if (!ObjectManager.Me.IsMounted)
                    {
                        if (Fight.InFight && ObjectManager.Me.Target > 0)
                        {
                            if (ObjectManager.Me.Target != lastTarget &&
                            Provoke.IsDistanceGood)
                            {
                                Pull();
                                lastTarget = ObjectManager.Me.Target;
                            }

                            if (ObjectManager.Target.GetDistance < 41)
                                Combat();
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
            Thread.Sleep(150);
        }
    }

    private void Pull()
    {
        if (!ObjectManager.Target.InCombat && Provoke.IsSpellUsable && Provoke.IsDistanceGood
            && MySettings.UseProvoke && Provoke.KnownSpell)
        {
            Provoke.Launch();
            return;
        }
    }

    private void Combat()
    {
        Buff();
        AvoidMelee();
        if (OnCD.IsReady)
            Defense_Cycle();
        Heal();
        Decast();
        DPS_Burst();
        DPS_Cycle();
    }

    private void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (Legacy_of_the_Emperor.KnownSpell && Legacy_of_the_Emperor.IsSpellUsable &&
            !Legacy_of_the_Emperor.HaveBuff && MySettings.UseLegacyoftheEmperor)
        {
            Legacy_of_the_Emperor.Launch();
            return;
        }
        if (Legacy_of_the_White_Tiger.KnownSpell && Legacy_of_the_White_Tiger.IsSpellUsable &&
            !Legacy_of_the_White_Tiger.HaveBuff && MySettings.UseLegacyoftheWhiteTiger)
        {
            Legacy_of_the_White_Tiger.Launch();
            return;
        }
        else if (Stance_of_the_Fierce_Tiger.KnownSpell && Stance_of_the_Fierce_Tiger.IsSpellUsable && !Stance_of_the_Fierce_Tiger.HaveBuff
                 && MySettings.UseStanceoftheFierceTiger)
        {
            Stance_of_the_Fierce_Tiger.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() == 0 && Tigers_Lust.IsSpellUsable && Tigers_Lust.KnownSpell
                && MySettings.UseTigersLust && ObjectManager.Me.GetMove)
        {
            Tigers_Lust.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() == 0 && Roll.IsSpellUsable && Roll.KnownSpell
                && MySettings.UseRoll && ObjectManager.Me.GetMove && !Tigers_Lust.HaveBuff
                && ObjectManager.Target.GetDistance > 14)
        {
            Roll.Launch();
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
            }
        }
    }

    private void AvoidMelee()
    {
        if (ObjectManager.Target.GetDistance < 2 && ObjectManager.Target.InCombat)
        {
            Logging.WriteFight("Too Close. Moving Back");
            var MaxTime_Timer = new Timer(1000 * 2);
            Keyboard.DownKey(Memory.WowProcess.MainWindowHandle, MoveBackward);
            while (ObjectManager.Target.GetDistance < 2 && ObjectManager.Target.InCombat && !MaxTime_Timer.IsReady)
                Thread.Sleep(300);
            Keyboard.UpKey(Memory.WowProcess.MainWindowHandle, MoveBackward);
            if (MaxTime_Timer.IsReady && ObjectManager.Target.GetDistance < 2 && ObjectManager.Target.InCombat)
            {
                Keyboard.DownKey(Memory.WowProcess.MainWindowHandle, MoveForward);
                Thread.Sleep(1000);
                Keyboard.UpKey(Memory.WowProcess.MainWindowHandle, MoveForward);
                MovementManager.Face(ObjectManager.Target.Position);
            }
        }
    }

    private void Defense_Cycle()
    {
        if (ObjectManager.Me.HealthPercent < 95 && MySettings.UseGrappleWeapon && Grapple_Weapon.IsDistanceGood
            && Grapple_Weapon.KnownSpell && Grapple_Weapon.IsSpellUsable && Grapple_Weapon_Timer.IsReady)
        {
            Grapple_Weapon.Launch();
            Grapple_Weapon_Timer = new Timer(1000*60);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && Fortifying_Brew.IsSpellUsable && Fortifying_Brew.KnownSpell
            && MySettings.UseFortifyingBrew)
        {
            Fortifying_Brew.Launch();
            OnCD = new Timer(1000*20);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && Zen_Meditation.IsSpellUsable && Zen_Meditation.KnownSpell
                 && MySettings.UseZenMeditation)
        {
            Zen_Meditation.Launch();
            OnCD = new Timer(1000*8);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 90 && Charging_Ox_Wave.IsSpellUsable && Charging_Ox_Wave.KnownSpell
            && MySettings.UseChargingOxWave && Charging_Ox_Wave.IsDistanceGood)
        {
            Charging_Ox_Wave.Launch();
            OnCD = new Timer(1000*3);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 90 && Dampen_Harm.IsSpellUsable && Dampen_Harm.KnownSpell
            && MySettings.UseDampenHarm)
        {
            Dampen_Harm.Launch();
            OnCD = new Timer(1000*5);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 90 && Leg_Sweep.IsSpellUsable && Leg_Sweep.KnownSpell
            && MySettings.UseLegSweep && ObjectManager.Target.GetDistance < 6)
        {
            Leg_Sweep.Launch();
            OnCD = new Timer(1000*5);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 95 && Touch_of_Karma.IsSpellUsable && Touch_of_Karma.KnownSpell
            && MySettings.UseTouchofKarma && Touch_of_Karma.IsDistanceGood)
        {
            Touch_of_Karma.Launch();
            OnCD = new Timer(1000*6);
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
            if (ObjectManager.Me.HealthPercent < 80 && War_Stomp.IsSpellUsable && War_Stomp.KnownSpell
                && MySettings.UseWarStomp)
            {
                War_Stomp.Launch();
                OnCD = new Timer(1000*2);
                return;
            }
        }
    }
    
    private void Heal()
    {
        if (Healing_Sphere.KnownSpell && Healing_Sphere.IsSpellUsable && ObjectManager.Me.Energy > 39 &&
            ObjectManager.Me.HealthPercent < 70 && MySettings.UseHealingSphere && Healing_Sphere_Timer.IsReady)
        {
            SpellManager.CastSpellByIDAndPosition(115460, ObjectManager.Me.Position);
            Healing_Sphere_Timer = new Timer(1000*5);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 85 && Chi_Wave.KnownSpell && Chi_Wave.IsSpellUsable 
                && MySettings.UseChiWave)
        {
            Chi_Wave.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 90 && Chi_Burst.KnownSpell && Chi_Burst.IsSpellUsable
                 && MySettings.UseChiBurst)
        {
            Chi_Burst.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Me.HealthPercent < 95 && Zen_Sphere.KnownSpell && Zen_Sphere.IsSpellUsable
                 && MySettings.UseZenSphere)
            {
                Zen_Sphere.Launch();
                return;
            }
        }
    }

    private void Decast()
    {
        if (Arcane_Torrent.KnownSpell && MySettings.UseArcaneTorrent && Arcane_Torrent.IsSpellUsable 
            && ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && ObjectManager.Target.GetDistance < 8)
        {
            Arcane_Torrent.Launch();
            return;
        }
        else if (Diffuse_Magic.KnownSpell && MySettings.UseDiffuseMagic && Diffuse_Magic.IsSpellUsable 
            && ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe)
        {
            Diffuse_Magic.Launch();
            return;
        }
        else
        {
            if (Spear_Hand_Strike.KnownSpell && MySettings.UseSpearHandStrike && ObjectManager.Target.IsCast 
                && Spear_Hand_Strike.IsSpellUsable && Spear_Hand_Strike.IsDistanceGood)
            {
                Spear_Hand_Strike.Launch();
                return;
            }
        }

        if (ObjectManager.Target.GetMove && !Disable.TargetHaveBuff && MySettings.UseDisable
            && Disable.KnownSpell && Disable.IsSpellUsable && Disable.IsDistanceGood)
        {
            Disable.Launch();
            return;
        }
    }

    private void DPS_Burst()
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
        }
        else if (Berserking.IsSpellUsable && Berserking.KnownSpell && ObjectManager.Target.GetDistance < 30
                 && MySettings.UseBerserking)
            Berserking.Launch();
        else if (Blood_Fury.IsSpellUsable && Blood_Fury.KnownSpell && ObjectManager.Target.GetDistance < 30
                 && MySettings.UseBloodFury)
            Blood_Fury.Launch();
        else if (Lifeblood.IsSpellUsable && Lifeblood.KnownSpell && ObjectManager.Target.GetDistance < 30
                 && MySettings.UseLifeblood)
            Lifeblood.Launch();
        else if (Engineering_Timer.IsReady && Engineering.KnownSpell && ObjectManager.Target.GetDistance < 30
                && MySettings.UseEngGlove)
        {
            Logging.WriteFight("Use Engineering Gloves.");
            Lua.RunMacroText("/use 10");
            Engineering_Timer = new Timer(1000*60);
        }
        else if (Chi_Brew.IsSpellUsable && Chi_Brew.KnownSpell
                 && MySettings.UseChiBrew && ObjectManager.Me.Chi == 0)
        {
            Chi_Brew.Launch();
            return;
        }
        else if (Touch_of_Death.IsSpellUsable && Touch_of_Death.KnownSpell && Touch_of_Death.IsDistanceGood
                 && MySettings.UseTouchofDeath)
        {
            Touch_of_Death.Launch();
            return;
        }
        else if (Invoke_Xuen_the_White_Tiger.IsSpellUsable && Invoke_Xuen_the_White_Tiger.KnownSpell
                 && MySettings.UseInvokeXuentheWhiteTiger && Invoke_Xuen_the_White_Tiger.IsDistanceGood)
        {
            Invoke_Xuen_the_White_Tiger.Launch();
            return;
        }
        else if (Energizing_Brew.IsSpellUsable && Energizing_Brew.KnownSpell && ObjectManager.Me.Energy < 41
                 && MySettings.UseEnergizingBrew && ObjectManager.Target.GetDistance < 30)
        {
            Energizing_Brew.Launch();
            return;
        }
        else if (Tigereye_Brew.IsSpellUsable && Tigereye_Brew.KnownSpell && ObjectManager.Me.BuffStack(125195) > 9
                 && MySettings.UseTigereyeBrew && ObjectManager.Target.GetDistance < 30)
        {
            Tigereye_Brew.Launch();
            return;
        }
        else
        {
            if (Rushing_Jade_Wind.IsSpellUsable && Rushing_Jade_Wind.KnownSpell && Rushing_Jade_Wind.IsDistanceGood
                && MySettings.UseRushingJadeWind && ObjectManager.GetNumberAttackPlayer() > 3)
            {
                Rushing_Jade_Wind.Launch();
                return;
            }
        }
    }

    private void DPS_Cycle()
    {
        if (ObjectManager.GetNumberAttackPlayer() > 3 && Rising_Sun_Kick.IsSpellUsable && Rising_Sun_Kick.KnownSpell
                 && Rising_Sun_Kick.IsDistanceGood && !Rising_Sun_Kick.TargetHaveBuff && MySettings.UseRisingSunKick)
        {
            Rising_Sun_Kick.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 3 && Spinning_Crane_Kick.IsSpellUsable && Spinning_Crane_Kick.KnownSpell
                 && Spinning_Crane_Kick.IsDistanceGood && !ObjectManager.Me.IsCast && MySettings.UseSpinningCraneKick)
        {
            Spinning_Crane_Kick.Launch();
            return;
        }
        else if (Rising_Sun_Kick.KnownSpell && Rising_Sun_Kick.IsSpellUsable && Rising_Sun_Kick.IsDistanceGood
                 && MySettings.UseRisingSunKick)
        {
            Rising_Sun_Kick.Launch();
            Rising_Sun_Kick_Timer = new Timer(1000*4);
            return;
        }
        else if (Tiger_Palm.IsSpellUsable && Tiger_Palm.IsDistanceGood && Tiger_Palm.KnownSpell 
            && MySettings.UseTigerPalm && !ObjectManager.Me.HaveBuff(121125)
            && (Tiger_Power_Timer.IsReady || ObjectManager.Me.BuffStack(125359) != 3 || ObjectManager.Me.HaveBuff(118864)))
        {
            Tiger_Palm.Launch();
            Tiger_Power_Timer = new Timer(1000*15);
            return;
        }
        else if (Fists_of_Fury.KnownSpell && Fists_of_Fury.IsSpellUsable && Fists_of_Fury.IsDistanceGood && !ObjectManager.Me.HaveBuff(121125)
            && MySettings.UseFistsofFury && !Tiger_Power_Timer.IsReady && !Rising_Sun_Kick_Timer.IsReady
            && ObjectManager.Me.EnergyPercentage < 81 && ObjectManager.Me.BuffStack(125359) > 2)
        {
            Fists_of_Fury.Launch();
            return;
        }
        else if (Blackout_Kick.IsSpellUsable && Blackout_Kick.IsDistanceGood && Blackout_Kick.KnownSpell && !ObjectManager.Me.HaveBuff(121125)
            && MySettings.UseBlackoutKick && (ObjectManager.Me.HaveBuff(116768) || ObjectManager.Me.Chi > 2))
        {
            Blackout_Kick.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 91 && Expel_Harm.KnownSpell && Expel_Harm.IsSpellUsable
                 && MySettings.UseExpelHarm && ObjectManager.Me.Chi < 3 && Expel_Harm.IsDistanceGood)
        {
            Expel_Harm.Launch();
            return;
        }
        else
        {
            if (Jab.KnownSpell && Jab.IsSpellUsable && MySettings.UseJab && !ObjectManager.Me.HaveBuff(116768)
                && ObjectManager.Me.Chi < 3 && !ObjectManager.Me.HaveBuff(118864) && Jab.IsDistanceGood)
            {
                Jab.Launch();
                return;
            }
        }
    }

    private void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            Buff();
            Heal();
        }
    }

    #region Nested type: MonkWindwalkerSettings

    [Serializable]
    public class MonkWindwalkerSettings : Settings
    {
        public bool UseAlchFlask = true;
        public bool UseArcaneTorrent = true;
        public bool UseBerserking = true;
        public bool UseBlackoutKick = true;
        public bool UseBloodFury = true;
        public bool UseChargingOxWave = true;
        public bool UseChiBrew = true;
        public bool UseChiBurst = true;
        public bool UseChiWave = true;
        public bool UseDampenHarm = true;
        public bool UseDiffuseMagic = true;
        public bool UseDisable = false;
        public bool UseEngGlove = true;
        public bool UseEnergizingBrew = true;
        public bool UseExpelHarm = true;
        public bool UseFistsofFury = true;
        public bool UseFortifyingBrew = true;
        public bool UseGiftoftheNaaru = true;
        public bool UseGrappleWeapon = true;
        public bool UseHealingSphere = true;
        public bool UseInvokeXuentheWhiteTiger = true;
        public bool UseJab = true;
        public bool UseLegSweep = true;
        public bool UseLegacyoftheEmperor = true;
        public bool UseLegacyoftheWhiteTiger = true;
        public bool UseLifeblood = true;
        public bool UseProvoke = true;
        public bool UseRisingSunKick = true;
        public bool UseRoll = true;
        public bool UseRushingJadeWind = true;
        public bool UseSpearHandStrike = true;
        public bool UseSpinningCraneKick = true;
        public bool UseStanceoftheFierceTiger = true;
        public bool UseStoneform = true;
        public bool UseTigereyeBrew = true;
        public bool UseTigerPalm = true;
        public bool UseTigersLust= true;
        public bool UseTouchofDeath= true;
        public bool UseTouchofKarma = true;
        public bool UseTrinket = true;
        public bool UseWarStomp = true;
        public bool UseZenMeditation = true;
        public bool UseZenSphere = true;

        public MonkWindwalkerSettings()
        {
            ConfigWinForm(new Point(500, 400), "Windwalker Monk Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");
            AddControlInWinForm("Use Lifeblood", "UseLifeblood", "Professions & Racials");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            /* Monk Buffs */
            AddControlInWinForm("Use Disable", "UseDisable", "Monk Buffs");
            AddControlInWinForm("Use Legacy of the Emperor", "UseLegacyoftheEmperor", "Monk Buffs");
            AddControlInWinForm("Use Legacy of the White Tiger", "UseLegacyoftheWhiteTiger", "Monk Buffs");
            AddControlInWinForm("Use Stance of the Fierce Tiger", "UseStanceoftheFierceTiger", "Monk Buffs");
            AddControlInWinForm("Use Tigereye Brew", "UseTigereBrew", "Monk Buffs");
            AddControlInWinForm("Use Tiger's Lust", "UseTigersLust", "Monk Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Blackout Kick", "UseBlackoutKick", "Offensive Spell");
            AddControlInWinForm("Use Fists of Fury", "UseFistsofFury", "Offensive Spell");
            AddControlInWinForm("Use Jab", "UseJab", "Offensive Spell");
            AddControlInWinForm("Use Path of Blossoms", "UsePathofBlossoms", "Offensive Spell");
            AddControlInWinForm("Use Provoke", "UseProvoke", "Offensive Spell");
            AddControlInWinForm("Use Rising Sun Kick", "UseRisingSunKick", "Offensive Spell");
            AddControlInWinForm("Use Roll", "UseRoll", "Offensive Spell");
            AddControlInWinForm("Use Spinning Crane Kick", "UseSpinningCraneKick", "Offensive Spell");
            AddControlInWinForm("Use Tiger Palm", "UseTigerPalm", "Offensive Spell");
            AddControlInWinForm("Use Touch of Death", "UseTouchofDeath", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use Chi Brew", "UseChiBrew", "Offensive Cooldown");
            AddControlInWinForm("Use Energizing Brew", "UseEnergizingBrew", "Offensive Cooldown");
            AddControlInWinForm("Use Invoke Xuen, the White Tiger", "UseInvokeXuentheWhiteTiger", "Offensive Cooldown");
            AddControlInWinForm("Use Rushing Jade Wind", "UseRushingJadeWind", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Charging Ox Wave", "UseChargingOxWave", "Defensive Cooldown");
            AddControlInWinForm("Use Dampen Harm ", "UseDampenHarm ", "Defensive Cooldown");
            AddControlInWinForm("Use Diffuse Magic", "UseDiffuseMagic", "Defensive Cooldown");
            AddControlInWinForm("Use Fortifying Brew", "UseFortifyingBrew", "Defensive Cooldown");
            AddControlInWinForm("Use Grapple Weapon", "UseGrappleWeapon", "Defensive Cooldown");
            AddControlInWinForm("Use Leg Sweep", "UseLegSweep", "Defensive Cooldown");
            AddControlInWinForm("Use Spear Hand Strike", "UseSpearHandStrike", "Defensive Cooldown");
            AddControlInWinForm("Use Touch of Karma", "UseTouchofKarma", "Defensive Cooldown");
            AddControlInWinForm("Use Zen Meditation", "UseZenMeditation", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Chi Burst", "UseChiBurst", "Healing Spell");
            AddControlInWinForm("Use Chi Wave", "UseChiWave", "Healing Spell");
            AddControlInWinForm("Use Expel Harm", "UseExpelHarm", "Healing Spell");
            AddControlInWinForm("Use Healing Sphere", "UseHealingSphere", "Healing Spell");
            AddControlInWinForm("Use Zen Sphere", "UseZenSphere", "Healing Spell");
            /* Game Settings */
            AddControlInWinForm("Use Trinket", "UseTrinket", "Game Settings");
            AddControlInWinForm("Use Engineering Gloves", "UseEngGlove", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
        }

        public static MonkWindwalkerSettings CurrentSetting { get; set; }

        public static MonkWindwalkerSettings GetSettings()
        {
            string CurrentSettingsFile = Application.StartupPath + "\\CustomClasses\\Settings\\Monk_Windwalker.xml";
            if (File.Exists(CurrentSettingsFile))
            {
                return
                    CurrentSetting = Load<MonkWindwalkerSettings>(CurrentSettingsFile);
            }
            else
            {
                return new MonkWindwalkerSettings();
            }
        }
    }

    #endregion
}

public class Monk_Mistweaver
{
    private readonly MonkMistweaverSettings MySettings = MonkMistweaverSettings.GetSettings();
    private readonly string MoveBackward = nManager.Wow.Helpers.Keybindings.GetKeyByAction(nManager.Wow.Enums.Keybindings.MOVEBACKWARD);
    private readonly string MoveForward = nManager.Wow.Helpers.Keybindings.GetKeyByAction(nManager.Wow.Enums.Keybindings.MOVEFORWARD);

    #region General Timers & Variables

    private Timer AlchFlask_Timer = new Timer(0);
    private Timer Engineering_Timer = new Timer(0);
    private Timer OnCD = new Timer(0);
    private Timer Trinket_Timer = new Timer(0);
    private Timer Grapple_Weapon_Timer = new Timer(0);
    private Timer Healing_Sphere_Timer = new Timer(0);
    private Timer Serpents_Zeal_Timer = new Timer(0);

    #endregion

    #region Professions & Racials

    private readonly Spell Alchemy = new Spell("Alchemy");
    private readonly Spell Arcane_Torrent = new Spell("Arcane Torrent");
    private readonly Spell Berserking = new Spell("Berserking");
    private readonly Spell Blood_Fury = new Spell("Blood Fury");
    private readonly Spell Engineering = new Spell("Engineering");
    private readonly Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    private readonly Spell Lifeblood = new Spell("Lifeblood");
    private readonly Spell Stoneform = new Spell("Stoneform");
    private readonly Spell War_Stomp = new Spell("War Stomp");

    #endregion

    #region Monk Buffs

    private readonly Spell Disable = new Spell("Disable");
    private readonly Spell Legacy_of_the_Emperor = new Spell("Legacy of the Emperor");
    private readonly Spell Stance_of_the_Fierce_Tiger = new Spell("Stance of the Fierce Tiger");
    private readonly Spell Stance_of_the_Wise_Serpent = new Spell("Stance of the Wise Serpent");
    private readonly Spell Summon_Jade_Serpent_Statue = new Spell("Summon Jade Serpent Statue");
    private readonly Spell Tigers_Lust = new Spell("Tiger's Lust");

    #endregion

    #region Offensive Spell

    private readonly Spell Blackout_Kick = new Spell("Blackout Kick");
    private readonly Spell Crackling_Jade_Lightning = new Spell("Crackling Jade Lightning");
    private readonly Spell Jab = new Spell("Jab");
    private readonly Spell Path_of_Blossoms = new Spell("Path of Blossoms");
    private readonly Spell Provoke = new Spell("Provoke");
    private readonly Spell Roll = new Spell("Roll");
    private readonly Spell Spinning_Crane_Kick = new Spell("Spinning Crane Kick");
    private readonly Spell Tiger_Palm = new Spell("Tiger Palm");
    private readonly Spell Touch_of_Death = new Spell("Touch of Death");


    #endregion

    #region Healing Cooldown

    private readonly Spell Chi_Brew = new Spell("Chi Brew");
    private readonly Spell Invoke_Xuen_the_White_Tiger = new Spell("Invoke Xuen, the White Tiger");
    private readonly Spell Rushing_Jade_Wind = new Spell("Rushing Jade Wind");
    private readonly Spell Thunder_Focus_Tea = new Spell("Thunder Focus Tea");

    #endregion

    #region Defensive Cooldown

    private readonly Spell Charging_Ox_Wave = new Spell("Charging Ox Wave");
    private readonly Spell Dampen_Harm = new Spell("Dampen Harm");
    private readonly Spell Diffuse_Magic = new Spell("Diffuse Magic");
    private readonly Spell Fortifying_Brew = new Spell("Fortifying Brew");
    private readonly Spell Grapple_Weapon = new Spell("Grapple Weapon");
    private readonly Spell Leg_Sweep = new Spell("Leg Sweep");
    private readonly Spell Life_Cocoon = new Spell("Life Cocoon");
    private readonly Spell Spear_Hand_Strike = new Spell("Spear Hand Strike");
    private readonly Spell Zen_Meditation = new Spell("Zen Meditation");

    #endregion

    #region Healing Spell

    private readonly Spell Chi_Burst = new Spell("Chi Burst");
    private readonly Spell Chi_Wave = new Spell("Chi Wave");
    private readonly Spell Enveloping_Mist = new Spell("Enveloping Mist");
    private readonly Spell Expel_Harm = new Spell("Expel Harm");
    private readonly Spell Healing_Sphere = new Spell("Healing Sphere");
    private readonly Spell Mana_Tea = new Spell("Mana Tea");
    private readonly Spell Renewing_Mist = new Spell("Renewing Mist");
    private readonly Spell Revival = new Spell("Revival");
    private readonly Spell Soothing_Mist = new Spell("Soothing Mist");
    private readonly Spell Surging_Mist = new Spell("Surging Mist");
    private readonly Spell Uplift = new Spell("Uplift");
    private readonly Spell Zen_Sphere = new Spell("Zen Sphere");

    #endregion

    public Monk_Mistweaver()
    {
        Main.range = 30.0f;
        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            try
            {
                if (!ObjectManager.Me.IsDead)
                {
                    if (!ObjectManager.Me.IsMounted)
                    {
                        if (Fight.InFight && ObjectManager.Me.Target > 0)
                        {
                            if (ObjectManager.Me.Target != lastTarget &&
                            Provoke.IsDistanceGood)
                            {
                                Pull();
                                lastTarget = ObjectManager.Me.Target;
                            }

                            if (ObjectManager.Target.GetDistance < 41)
                                Combat();
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
            Thread.Sleep(150);
        }
    }

    private void Pull()
    {
        if (!ObjectManager.Target.InCombat && Provoke.IsSpellUsable && Provoke.IsDistanceGood
            && MySettings.UseProvoke && Provoke.KnownSpell)
        {
            Provoke.Launch();
            return;
        }
    }

    private void Combat()
    {
        Buff();
        AvoidMelee();
        if (OnCD.IsReady)
            Defense_Cycle();
        Heal();
        Decast();
        Healing_Burst();
        DPS_Cycle();
    }

    private void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (Legacy_of_the_Emperor.KnownSpell && Legacy_of_the_Emperor.IsSpellUsable &&
            !Legacy_of_the_Emperor.HaveBuff && MySettings.UseLegacyoftheEmperor)
        {
            Legacy_of_the_Emperor.Launch();
            return;
        }
        else if (Stance_of_the_Wise_Serpent.KnownSpell && Stance_of_the_Wise_Serpent.IsSpellUsable && !Stance_of_the_Wise_Serpent.HaveBuff
                 && MySettings.UseStanceoftheWiseSerpent)
        {
            Stance_of_the_Wise_Serpent.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() == 0 && Tigers_Lust.IsSpellUsable && Tigers_Lust.KnownSpell
                && MySettings.UseTigersLust && ObjectManager.Me.GetMove)
        {
            Tigers_Lust.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() == 0 && Roll.IsSpellUsable && Roll.KnownSpell
                && MySettings.UseRoll && ObjectManager.Me.GetMove && !Tigers_Lust.HaveBuff
                && ObjectManager.Target.GetDistance > 14)
        {
            Roll.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 0 && Summon_Jade_Serpent_Statue.IsSpellUsable && Summon_Jade_Serpent_Statue.KnownSpell
                && MySettings.UseSummonJadeSerpentStatue && !Summon_Jade_Serpent_Statue.HaveBuff && ObjectManager.Target.GetDistance < 40)
        {
            Summon_Jade_Serpent_Statue.Launch();
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
            }
        }
    }

    private void AvoidMelee()
    {
        if (ObjectManager.Target.GetDistance < 2 && ObjectManager.Target.InCombat)
        {
            Logging.WriteFight("Too Close. Moving Back");
            var MaxTime_Timer = new Timer(1000 * 2);
            Keyboard.DownKey(Memory.WowProcess.MainWindowHandle, MoveBackward);
            while (ObjectManager.Target.GetDistance < 2 && ObjectManager.Target.InCombat && !MaxTime_Timer.IsReady)
                Thread.Sleep(300);
            Keyboard.UpKey(Memory.WowProcess.MainWindowHandle, MoveBackward);
            if (MaxTime_Timer.IsReady && ObjectManager.Target.GetDistance < 2 && ObjectManager.Target.InCombat)
            {
                Keyboard.DownKey(Memory.WowProcess.MainWindowHandle, MoveForward);
                Thread.Sleep(1000);
                Keyboard.UpKey(Memory.WowProcess.MainWindowHandle, MoveForward);
                MovementManager.Face(ObjectManager.Target.Position);
            }
        }
    }

    private void Defense_Cycle()
    {
        if (ObjectManager.Me.HealthPercent < 95 && MySettings.UseGrappleWeapon && Grapple_Weapon.IsDistanceGood
            && Grapple_Weapon.KnownSpell && Grapple_Weapon.IsSpellUsable && Grapple_Weapon_Timer.IsReady)
        {
            Grapple_Weapon.Launch();
            Grapple_Weapon_Timer = new Timer(1000*60);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && Fortifying_Brew.IsSpellUsable && Fortifying_Brew.KnownSpell
            && MySettings.UseFortifyingBrew)
        {
            Fortifying_Brew.Launch();
            OnCD = new Timer(1000*20);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && Life_Cocoon.IsSpellUsable && Life_Cocoon.KnownSpell
            && MySettings.UseLifeCocoon)
        {
            Life_Cocoon.Launch();
            OnCD = new Timer(1000*12);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 90 && Charging_Ox_Wave.IsSpellUsable && Charging_Ox_Wave.KnownSpell
            && MySettings.UseChargingOxWave && Charging_Ox_Wave.IsDistanceGood)
        {
            Charging_Ox_Wave.Launch();
            OnCD = new Timer(1000*3);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 90 && Dampen_Harm.IsSpellUsable && Dampen_Harm.KnownSpell
            && MySettings.UseDampenHarm)
        {
            Dampen_Harm.Launch();
            OnCD = new Timer(1000*5);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 90 && Leg_Sweep.IsSpellUsable && Leg_Sweep.KnownSpell
            && MySettings.UseLegSweep && ObjectManager.Target.GetDistance < 6)
        {
            Leg_Sweep.Launch();
            OnCD = new Timer(1000*5);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && Zen_Meditation.IsSpellUsable && Zen_Meditation.KnownSpell
                 && MySettings.UseZenMeditation)
        {
            Zen_Meditation.Launch();
            OnCD = new Timer(1000*8);
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
            if (ObjectManager.Me.HealthPercent < 80 && War_Stomp.IsSpellUsable && War_Stomp.KnownSpell
                && MySettings.UseWarStomp)
            {
                War_Stomp.Launch();
                OnCD = new Timer(1000*2);
                return;
            }
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.ManaPercentage < 50 && Mana_Tea.KnownSpell && Mana_Tea.IsSpellUsable 
            && MySettings.UseManaTea && ObjectManager.Me.BuffStack(115867) > 4
            && ObjectManager.GetNumberAttackPlayer() == 0)
        {
            Mana_Tea.Launch();
            return;
        }
        else 
        {
            if (ObjectManager.Me.HealthPercent < 95 && Surging_Mist.KnownSpell && Surging_Mist.IsSpellUsable 
                && MySettings.UseSurgingMist && ObjectManager.Me.BuffStack(118674) > 4
                && ObjectManager.GetNumberAttackPlayer() == 0)
            {
                Surging_Mist.Launch();
                return;
            }
        }

        if (Healing_Sphere.KnownSpell && Healing_Sphere.IsSpellUsable && ObjectManager.Me.Energy > 39 &&
            ObjectManager.Me.HealthPercent < 60 && MySettings.UseHealingSphere && Healing_Sphere_Timer.IsReady)
        {
            SpellManager.CastSpellByIDAndPosition(115460, ObjectManager.Me.Position);
            Healing_Sphere_Timer = new Timer(1000*5);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 70 && Surging_Mist.KnownSpell && Surging_Mist.IsSpellUsable 
            && MySettings.UseSurgingMist)
        {
            Surging_Mist.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 85 && Uplift.KnownSpell && Uplift.IsSpellUsable 
            && MySettings.UseUplift && Renewing_Mist.HaveBuff)
        {
            Uplift.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 85 && Chi_Wave.KnownSpell && Chi_Wave.IsSpellUsable 
            && MySettings.UseChiWave)
        {
            Chi_Wave.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 90 && Chi_Burst.KnownSpell && Chi_Burst.IsSpellUsable
                 && MySettings.UseChiBurst)
        {
            Chi_Burst.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 90 && Expel_Harm.KnownSpell && Expel_Harm.IsSpellUsable
                 && MySettings.UseExpelHarm && Expel_Harm.IsDistanceGood)
        {
            Expel_Harm.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 90 && Enveloping_Mist.KnownSpell && Enveloping_Mist.IsSpellUsable
                 && MySettings.UseEnvelopingMist && !Enveloping_Mist.HaveBuff)
        {
            Enveloping_Mist.Launch();
            return;
        }
        if (ObjectManager.Me.HealthPercent < 95 && Surging_Mist.KnownSpell && Surging_Mist.IsSpellUsable 
            && MySettings.UseSurgingMist && ObjectManager.Me.BuffStack(118674) > 4)
        {
            Surging_Mist.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 95 && Soothing_Mist.KnownSpell && Soothing_Mist.IsSpellUsable
                 && MySettings.UseSoothingMist && !Soothing_Mist.HaveBuff && !ObjectManager.Me.IsCast)
        {
            Soothing_Mist.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 95 && Renewing_Mist.KnownSpell && Renewing_Mist.IsSpellUsable
                 && MySettings.UseRenewingMist && !Renewing_Mist.HaveBuff)
        {
            Renewing_Mist.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Me.HealthPercent < 95 && Zen_Sphere.KnownSpell && Zen_Sphere.IsSpellUsable
                 && MySettings.UseZenSphere)
            {
                Zen_Sphere.Launch();
                return;
            }
        }
    }

    private void Decast()
    {
        if (Arcane_Torrent.KnownSpell && MySettings.UseArcaneTorrent && Arcane_Torrent.IsSpellUsable 
            && ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && ObjectManager.Target.GetDistance < 8)
        {
            Arcane_Torrent.Launch();
            return;
        }
        else if (Diffuse_Magic.KnownSpell && MySettings.UseDiffuseMagic && Diffuse_Magic.IsSpellUsable 
            && ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe)
        {
            Diffuse_Magic.Launch();
            return;
        }
        else
        {
            if (Spear_Hand_Strike.KnownSpell && MySettings.UseSpearHandStrike && ObjectManager.Target.IsCast 
                && Spear_Hand_Strike.IsSpellUsable && Spear_Hand_Strike.IsDistanceGood)
            {
                Spear_Hand_Strike.Launch();
                return;
            }
        }

        if (ObjectManager.Target.GetMove && !Disable.TargetHaveBuff && MySettings.UseDisable
            && Disable.KnownSpell && Disable.IsSpellUsable && Disable.IsDistanceGood)
        {
            Disable.Launch();
            return;
        }
    }

    private void Healing_Burst()
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
        }
        else if (Berserking.IsSpellUsable && Berserking.KnownSpell && ObjectManager.Target.GetDistance < 30
                 && MySettings.UseBerserking)
            Berserking.Launch();
        else if (Blood_Fury.IsSpellUsable && Blood_Fury.KnownSpell && ObjectManager.Target.GetDistance < 30
                 && MySettings.UseBloodFury)
            Blood_Fury.Launch();
        else if (Lifeblood.IsSpellUsable && Lifeblood.KnownSpell && ObjectManager.Target.GetDistance < 30
                 && MySettings.UseLifeblood)
            Lifeblood.Launch();
        else if (Engineering_Timer.IsReady && Engineering.KnownSpell && ObjectManager.Target.GetDistance < 30
                && MySettings.UseEngGlove)
        {
            Logging.WriteFight("Use Engineering Gloves.");
            Lua.RunMacroText("/use 10");
            Engineering_Timer = new Timer(1000*60);
        }
        else if (Chi_Brew.IsSpellUsable && Chi_Brew.KnownSpell
                 && MySettings.UseChiBrew && ObjectManager.Me.Chi == 0)
        {
            Chi_Brew.Launch();
            return;
        }
        else if (Touch_of_Death.IsSpellUsable && Touch_of_Death.KnownSpell && Touch_of_Death.IsDistanceGood
                 && MySettings.UseTouchofDeath)
        {
            Touch_of_Death.Launch();
            return;
        }
        else if (Invoke_Xuen_the_White_Tiger.IsSpellUsable && Invoke_Xuen_the_White_Tiger.KnownSpell
                 && MySettings.UseInvokeXuentheWhiteTiger && Invoke_Xuen_the_White_Tiger.IsDistanceGood)
        {
            Invoke_Xuen_the_White_Tiger.Launch();
            return;
        }
        else if (Thunder_Focus_Tea.IsSpellUsable && Thunder_Focus_Tea.KnownSpell
                 && MySettings.UseThunderFocusTea && ObjectManager.Me.HealthPercent < 90)
        {
            Thunder_Focus_Tea.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && Revival.KnownSpell && Revival.IsSpellUsable 
            && MySettings.UseRevival)
        {
            Revival.Launch();
            return;
        }
        else
        {
            if (Rushing_Jade_Wind.IsSpellUsable && Rushing_Jade_Wind.KnownSpell && Rushing_Jade_Wind.IsDistanceGood
                && MySettings.UseRushingJadeWind && ObjectManager.GetNumberAttackPlayer() > 3)
            {
                Rushing_Jade_Wind.Launch();
                return;
            }
        }
    }

    private void DPS_Cycle()
    {
        if (ObjectManager.GetNumberAttackPlayer() > 2 && Spinning_Crane_Kick.IsSpellUsable && Spinning_Crane_Kick.KnownSpell
                 && Spinning_Crane_Kick.IsDistanceGood && !ObjectManager.Me.IsCast && MySettings.UseSpinningCraneKick)
        {
            Spinning_Crane_Kick.Launch();
            return;
        }
        else if (Crackling_Jade_Lightning.KnownSpell && Crackling_Jade_Lightning.IsSpellUsable
            && MySettings.UseCracklingJadeLightning && ObjectManager.Me.Chi < 4 && Crackling_Jade_Lightning.IsDistanceGood
            && !Expel_Harm.IsDistanceGood)
        {
            Crackling_Jade_Lightning.Launch();
            return;
        }
        else if (Blackout_Kick.KnownSpell && Blackout_Kick.IsSpellUsable && Blackout_Kick.IsDistanceGood
                 && MySettings.UseBlackoutKick && (!ObjectManager.Me.HaveBuff(127722) || Serpents_Zeal_Timer.IsReady))
        {
            Blackout_Kick.Launch();
            Serpents_Zeal_Timer = new Timer(1000*25);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 91 && Expel_Harm.KnownSpell && Expel_Harm.IsSpellUsable
                 && MySettings.UseExpelHarm && ObjectManager.Me.Chi < 4 && Expel_Harm.IsDistanceGood)
        {
            Expel_Harm.Launch();
            return;
        }
        else if (Jab.KnownSpell && Jab.IsSpellUsable && MySettings.UseJab && ObjectManager.Me.Chi < 4 
                 && Jab.IsDistanceGood)
        {
            Jab.Launch();
            return;
        }
        else
        {
            if (Tiger_Palm.KnownSpell && Tiger_Palm.IsSpellUsable && Tiger_Palm.IsDistanceGood
                 && MySettings.UseTigerPalm && ObjectManager.Me.HealthPercent > 90
                 && ObjectManager.Me.BuffStack(125359) < 3)
            {
                Tiger_Palm.Launch();
                return;
            }
        }
    }

    private void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            Buff();
            Heal();
        }
    }

    #region Nested type: MonkMistweaverSettings

    [Serializable]
    public class MonkMistweaverSettings : Settings
    {
        public bool UseAlchFlask = true;
        public bool UseArcaneTorrent = true;
        public bool UseBerserking = true;
        public bool UseBlackoutKick = true;
        public bool UseBloodFury = true;
        public bool UseChargingOxWave = true;
        public bool UseChiBrew = true;
        public bool UseChiBurst = true;
        public bool UseChiWave = true;
        public bool UseCracklingJadeLightning = true;
        public bool UseDampenHarm = true;
        public bool UseDiffuseMagic = true;
        public bool UseDisable = false;
        public bool UseEngGlove = true;
        public bool UseEnvelopingMist = true;
        public bool UseExpelHarm = true;
        public bool UseFortifyingBrew = true;
        public bool UseGiftoftheNaaru = true;
        public bool UseGrappleWeapon = true;
        public bool UseHealingSphere = true;
        public bool UseInvokeXuentheWhiteTiger = true;
        public bool UseJab = true;
        public bool UseLegSweep = true;
        public bool UseLegacyoftheEmperor = true;
        public bool UseLifeblood = true;
        public bool UseLifeCocoon = true;
        public bool UseManaTea = true;
        public bool UsePathofBlossoms = true;
        public bool UseProvoke = true;
        public bool UseRenewingMist = true;
        public bool UseRevival = true;
        public bool UseRoll = true;
        public bool UseRushingJadeWind = true;
        public bool UseSoothingMist = false;
        public bool UseSpearHandStrike = true;
        public bool UseSpinningCraneKick = true;
        public bool UseStanceoftheFierceTiger = true;
        public bool UseStanceoftheWiseSerpent = true;
        public bool UseStoneform = true;
        public bool UseSummonJadeSerpentStatue = true;
        public bool UseSurgingMist = true;
        public bool UseThunderFocusTea = true;
        public bool UseTigerPalm = true;
        public bool UseTigersLust= true;
        public bool UseTouchofDeath= true;
        public bool UseTrinket = true;
        public bool UseUplift = true;
        public bool UseWarStomp = true;
        public bool UseZenMeditation = true;
        public bool UseZenSphere = true;

        public MonkMistweaverSettings()
        {
            ConfigWinForm(new Point(500, 400), "Mistweaver Monk Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");
            AddControlInWinForm("Use Lifeblood", "UseLifeblood", "Professions & Racials");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            /* Monk Buffs */
            AddControlInWinForm("Use Disable", "UseDisable", "Monk Buffs");
            AddControlInWinForm("Use Legacy of the Emperor", "UseLegacyoftheEmperor", "Monk Buffs");
            AddControlInWinForm("Use Stance of the Fierce Tiger", "UseStanceoftheFierceTiger", "Monk Buffs");
            AddControlInWinForm("Use Summon Jade Serpent Statue", "UseSummonJadeSerpentStatue", "Monk Buffs");
            AddControlInWinForm("Use Tiger's Lust", "UseTigersLust", "Monk Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Chi Wave", "UseChiWave", "Offensive Spell");
            AddControlInWinForm("Use Blackout Kick", "UseBlackoutKick", "Offensive Spell");
            AddControlInWinForm("Use Crackling Jade Lightning", "UseCracklingJadeLightning", "Offensive Spell");
            AddControlInWinForm("Use Jab", "UseJab", "Offensive Spell");
            AddControlInWinForm("Use Path of Blossoms", "UsePathofBlossoms", "Offensive Spell");
            AddControlInWinForm("Use Provoke", "UseProvoke", "Offensive Spell");
            AddControlInWinForm("Use Roll", "UseRoll", "Offensive Spell");
            AddControlInWinForm("Use Spinning Crane Kick", "UseSpinningCraneKick", "Offensive Spell");
            AddControlInWinForm("Use Tiger Palm", "UseTigerPalm", "Offensive Spell");
            AddControlInWinForm("Use Touch of Death", "UseTouchofDeath", "Offensive Spell");
            /* Healing Cooldown */
            AddControlInWinForm("Use Chi Brew", "UseChiBrew", "Healing Cooldown");
            AddControlInWinForm("Use Invoke Xuen, the White Tiger", "UseInvokeXuentheWhiteTiger", "Healing Cooldown");
            AddControlInWinForm("Use Revival", "UseRevival", "Healing Cooldown");
            AddControlInWinForm("Use Rushing Jade Wind", "UseRushingJadeWind", "Healing Cooldown");
            AddControlInWinForm("Use Thunder Focus Tea", "UseThunderFocusTea", "Healing Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Charging Ox Wave", "UseChargingOxWave", "Defensive Cooldown");
            AddControlInWinForm("Use Dampen Harm ", "UseDampenHarm ", "Defensive Cooldown");
            AddControlInWinForm("Use Diffuse Magic", "UseDiffuseMagic", "Defensive Cooldown");
            AddControlInWinForm("Use Fortifying Brew", "UseFortifyingBrew", "Defensive Cooldown");
            AddControlInWinForm("Use Grapple Weapon", "UseGrappleWeapon", "Defensive Cooldown");
            AddControlInWinForm("Use Leg Sweep", "UseLegSweep", "Defensive Cooldown");
            AddControlInWinForm("Use Life Cocoon", "UseLifeCocoon", "Defensive Cooldown");
            AddControlInWinForm("Use Spear Hand Strike", "UseSpearHandStrike", "Defensive Cooldown");
            AddControlInWinForm("Use Zen Meditation", "UseZenMeditation", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Chi Burst", "UseChiBurst", "Healing Spell");
            AddControlInWinForm("Use Enveloping Mist", "UseEnvelopingMist", "Healing Spell");
            AddControlInWinForm("Use Expel Harm", "UseExpelHarm", "Healing Spell");
            AddControlInWinForm("Use Healing Sphere", "UseHealingSphere", "Healing Spell");
            AddControlInWinForm("Use Mana Tea", "UseManaTea", "Healing Spell");
            AddControlInWinForm("Use Renewing Mist", "UseRenewingMist", "Healing Spell");
            AddControlInWinForm("Use Soothing Mist", "UseSoothingMist", "Healing Spell");
            AddControlInWinForm("Use Surging Mist", "UseSurgingMist", "Healing Spell");
            AddControlInWinForm("Use Uplift", "UseUplift", "Healing Spell");
            AddControlInWinForm("Use Zen Sphere", "UseZenSphere", "Healing Spell");
            /* Game Settings */
            AddControlInWinForm("Use Trinket", "UseTrinket", "Game Settings");
            AddControlInWinForm("Use Engineering Gloves", "UseEngGlove", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
        }

        public static MonkMistweaverSettings CurrentSetting { get; set; }

        public static MonkMistweaverSettings GetSettings()
        {
            string CurrentSettingsFile = Application.StartupPath + "\\CustomClasses\\Settings\\Monk_Mistweaver.xml";
            if (File.Exists(CurrentSettingsFile))
            {
                return
                    CurrentSetting = Load<MonkMistweaverSettings>(CurrentSettingsFile);
            }
            else
            {
                return new MonkMistweaverSettings();
            }
        }
    }

    #endregion
}

#endregion
