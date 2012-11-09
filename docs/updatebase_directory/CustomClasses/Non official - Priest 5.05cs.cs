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
    internal static float range = 30.0f;
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

    #endregion

    public void Initialize(bool ConfigOnly)
    {
        try
        {
            if (!loop)
                loop = true;
            Logging.WriteFight("Loading combat system.");
            switch (ObjectManager.Me.WowClass)
            {

                #region Priest Specialisation checking

                case WoWClass.Priest:
                    var Priest_Shadow_Spell = new Spell("Mind Flay");
                    var Priest_Discipline_Spell = new Spell("Penance");
                    var Priest_Holy_Spell = new Spell("Holy Word: Chastise");
                    if (Priest_Shadow_Spell.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Priest_Shadow.xml";
                            Priest_Shadow.PriestShadowSettings CurrentSetting;
                            CurrentSetting = new Priest_Shadow.PriestShadowSettings();
                            if (File.Exists(CurrentSettingsFile))
                            {
                                CurrentSetting =
                                    Settings.Load<Priest_Shadow.PriestShadowSettings>(CurrentSettingsFile);
                            }
                            CurrentSetting.ToForm();
                            CurrentSetting.Save(CurrentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Priest Shadow class...");
                            range = 30.0f;
                            new Priest_Shadow();
                        }
                    }
                    else if (Priest_Discipline_Spell.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Priest_Discipline.xml";
                            Priest_Discipline.PriestDisciplineSettings CurrentSetting;
                            CurrentSetting = new Priest_Discipline.PriestDisciplineSettings();
                            if (File.Exists(CurrentSettingsFile))
                            {
                                CurrentSetting =
                                    Settings.Load<Priest_Discipline.PriestDisciplineSettings>(CurrentSettingsFile);
                            }
                            CurrentSetting.ToForm();
                            CurrentSetting.Save(CurrentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Priest Discipline class...");
                            range = 30.0f;
                            new Priest_Discipline();
                        }
                    }
                    else if (Priest_Holy_Spell.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Priest_Holy.xml";
                            Priest_Holy.PriestHolySettings CurrentSetting;
                            CurrentSetting = new Priest_Holy.PriestHolySettings();
                            if (File.Exists(CurrentSettingsFile))
                            {
                                CurrentSetting =
                                    Settings.Load<Priest_Holy.PriestHolySettings>(CurrentSettingsFile);
                            }
                            CurrentSetting.ToForm();
                            CurrentSetting.Save(CurrentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Priest Holy class...");
                            range = 30.0f;
                            new Priest_Holy();
                        }
                    }
                    else
                    {
                        if (ConfigOnly)
                        {
                            MessageBox.Show(
                                "Your specification haven't be found, loading Priest Shadow Settings");
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Priest_Shadow.xml";
                            Priest_Shadow.PriestShadowSettings CurrentSetting;
                            CurrentSetting = new Priest_Shadow.PriestShadowSettings();
                            if (File.Exists(CurrentSettingsFile))
                            {
                                CurrentSetting =
                                    Settings.Load<Priest_Shadow.PriestShadowSettings>(CurrentSettingsFile);
                            }
                            CurrentSetting.ToForm();
                            CurrentSetting.Save(CurrentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("No specialisation detected.");
                            Logging.WriteFight("Loading Priest Shadow class...");
                            range = 30.0f;
                            new Priest_Shadow();
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


#region Priest

public class Priest_Shadow
{
    private readonly PriestShadowSettings MySettings = PriestShadowSettings.GetSettings();

    private Timer AlchFlask_Timer = new Timer(0);
    private Timer Engineering_Timer = new Timer(0);
    public int LC = 0;
    private Timer OnCD = new Timer(0);
    private Timer Trinket_Timer = new Timer(0);

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

    #region Priest Buffs

    private readonly Spell Inner_Fire = new Spell("Inner Fire");
    private readonly Spell Inner_Will = new Spell("Inner Will");
    private readonly Spell Levitate = new Spell("Levitate");
    private readonly Spell Power_Word_Fortitude = new Spell("Power Word: Fortitude");
    private readonly Spell Shadowform = new Spell("Shadowform");
    private Timer Levitate_Timer = new Timer(0);

    #endregion

    #region Offensive Spell

    private readonly Spell Cascade = new Spell("Cascade");
    private readonly Spell Devouring_Plague = new Spell("Devouring Plague");
    private readonly Spell Divine_Star = new Spell("Divine Star");
    private readonly Spell Halo = new Spell("Halo");
    private readonly Spell Mind_Blast = new Spell("Mind Blast");
    private readonly Spell Mind_Flay = new Spell("Mind Flay");
    private readonly Spell Mind_Sear = new Spell("Mind Sear");
    private readonly Spell Mind_Spike = new Spell("Mind Spike");
    private readonly Spell Shadow_Word_Death = new Spell("Shadow Word: Death");
    private readonly Spell Shadow_Word_Insanity = new Spell("Shadow Word: Insanity");
    private readonly Spell Shadow_Word_Pain = new Spell("Shadow Word: Pain");
    private readonly Spell Smite = new Spell("Smite");
    private readonly Spell Vampiric_Touch = new Spell("Vampiric Touch");
    private Timer Devouring_Plague_Timer = new Timer(0);
    private Timer Shadow_Word_Pain_Timer = new Timer(0);
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
    private readonly Spell Vampiric_Embrace = new Spell("Vampiric Embrace");
    private Timer Renew_Timer = new Timer(0);

    #endregion

    public Priest_Shadow()
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
                    else if (!ObjectManager.Me.IsCast)
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
                Shadow_Word_Pain_Timer = new Timer(1000*14);
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

    private void Buff_Levitate()
    {
        if (!Fight.InFight && Levitate.KnownSpell && Levitate.IsSpellUsable && MySettings.UseLevitate
            && (!Levitate.HaveBuff || Levitate_Timer.IsReady))
        {
            Levitate.Launch();
            Levitate_Timer = new Timer(1000*60*9);
        }
    }

    public void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

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
            AlchFlask_Timer = new Timer(1000*60*60*2);
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

    public void DPS_Burst()
    {
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
        else if (Power_Infusion.IsSpellUsable && Power_Infusion.KnownSpell
                 && MySettings.UsePowerInfusion && ObjectManager.Target.GetDistance < 40)
        {
            Power_Infusion.Launch();
            return;
        }
        else
        {
            if (Shadowfiend.IsSpellUsable && Shadowfiend.KnownSpell && Shadowfiend.IsDistanceGood
                && MySettings.UseShadowfiend)
            {
                Shadowfiend.Launch();
                return;
            }
        }
    }

    public void DPS_Cycle()
    {
        if (ObjectManager.Me.ManaPercentage < 80 && Arcane_Torrent.KnownSpell && Arcane_Torrent.IsSpellUsable
            && MySettings.UseArcaneTorrent)
        {
            Arcane_Torrent.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 2 && Cascade.IsSpellUsable && Cascade.KnownSpell
                 && Cascade.IsDistanceGood && MySettings.UseCascade)
        {
            Cascade.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 2 && Divine_Star.IsSpellUsable && Divine_Star.KnownSpell
                 && Divine_Star.IsDistanceGood && MySettings.UseDivineStar)
        {
            Divine_Star.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 2 && Halo.IsSpellUsable && Halo.KnownSpell
                 && Halo.IsDistanceGood && MySettings.UseHalo)
        {
            Halo.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 4 && Mind_Sear.IsSpellUsable && Mind_Sear.KnownSpell
                 && Mind_Sear.IsDistanceGood && !ObjectManager.Me.IsCast && MySettings.UseMindSear)
        {
            Mind_Sear.Launch();
            return;
        }
        else if (Shadow_Word_Death.IsSpellUsable && Shadow_Word_Death.IsDistanceGood && Shadow_Word_Death.KnownSpell
                 && ObjectManager.Target.HealthPercent < 20 && MySettings.UseShadowWordDeath)
        {
            Shadow_Word_Death.Launch();
            return;
        }
        else if (Shadow_Word_Pain.KnownSpell && Shadow_Word_Pain.IsSpellUsable
                 && Shadow_Word_Pain.IsDistanceGood && MySettings.UseShadowWordPain
                 && (!Shadow_Word_Pain.TargetHaveBuff || Shadow_Word_Pain_Timer.IsReady))
        {
            Shadow_Word_Pain.Launch();
            Shadow_Word_Pain_Timer = new Timer(1000*14);
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
            Vampiric_Touch_Timer = new Timer(1000*11);
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
            Devouring_Plague_Timer = new Timer(1000*3);
            return;
        }
        else if (Mind_Blast.KnownSpell && Mind_Blast.IsSpellUsable && Mind_Blast.IsDistanceGood
                 && ObjectManager.Me.ShadowOrbs < 3 && MySettings.UseMindBlast)
        {
            Mind_Blast.Launch();
            return;
        }
        else if (!ObjectManager.Me.IsCast && Mind_Flay.IsSpellUsable && Mind_Flay.KnownSpell && Mind_Flay.IsDistanceGood
                 && MySettings.UseMindFlay && Shadow_Word_Pain.TargetHaveBuff && Vampiric_Touch.TargetHaveBuff
                 && !ObjectManager.Me.HaveBuff(87160) && ObjectManager.GetNumberAttackPlayer() < 5
                 && ObjectManager.Me.ShadowOrbs != 3)
        {
            Mind_Flay.Launch();
            return;
        }
        // Blizzard API Calls for Mind Flay using Smite Function
        else if (!ObjectManager.Me.IsCast && Smite.IsSpellUsable && Smite.KnownSpell && Smite.IsDistanceGood
                 && MySettings.UseMindFlay && Shadow_Word_Pain.TargetHaveBuff && Vampiric_Touch.TargetHaveBuff
                 && !ObjectManager.Me.HaveBuff(87160) && ObjectManager.GetNumberAttackPlayer() < 5
                 && ObjectManager.Me.ShadowOrbs != 3)
        {
            Smite.Launch();
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

    private void Heal()
    {
        if (ObjectManager.Me.HealthPercent < 95 && !Fight.InFight && ObjectManager.GetNumberAttackPlayer() == 0)
        {
            if (Flash_Heal.KnownSpell && Flash_Heal.IsSpellUsable && MySettings.UseFlashHeal)
            {
                Flash_Heal.Launch(false);
                return;
            }
        }
        else if (!Fight.InFight && ObjectManager.Me.ManaPercentage < 40 && Hymn_of_Hope.KnownSpell &&
                 Hymn_of_Hope.IsSpellUsable
                 && ObjectManager.GetNumberAttackPlayer() == 0 && MySettings.UseHymnofHope)
        {
            Hymn_of_Hope.Launch(false);
            return;
        }
        else if (!Fight.InFight && ObjectManager.Me.ManaPercentage < 60 && ObjectManager.GetNumberAttackPlayer() == 0
                 && Dispersion.KnownSpell && Dispersion.IsSpellUsable && MySettings.UseDispersion)
        {
            Dispersion.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 65 && Desperate_Prayer.KnownSpell && Desperate_Prayer.IsSpellUsable
                 && MySettings.UseDesperatePrayer)
        {
            Desperate_Prayer.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 60 && Flash_Heal.KnownSpell && Flash_Heal.IsSpellUsable
                 && MySettings.UseFlashHeal)
        {
            Flash_Heal.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && Gift_of_the_Naaru.IsSpellUsable && Gift_of_the_Naaru.KnownSpell
                 && MySettings.UseGiftoftheNaaru)
        {
            Gift_of_the_Naaru.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && War_Stomp.IsSpellUsable && War_Stomp.KnownSpell
                 && MySettings.UseWarStomp && ObjectManager.Target.GetDistance < 8)
        {
            War_Stomp.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && Vampiric_Embrace.IsSpellUsable && Vampiric_Embrace.KnownSpell
                 && MySettings.UseVampiricEmbrace)
        {
            Vampiric_Embrace.Launch();
            return;
        }
        else if (Power_Word_Shield.KnownSpell && Power_Word_Shield.IsSpellUsable
                 && !Power_Word_Shield.HaveBuff && MySettings.UsePowerWordShield
                 && !ObjectManager.Me.HaveBuff(6788) && (ObjectManager.GetNumberAttackPlayer() > 0
                                                         || ObjectManager.Me.GetMove))
        {
            Power_Word_Shield.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 50 && Prayer_of_Mending.KnownSpell && Prayer_of_Mending.IsSpellUsable
                 && MySettings.UsePrayerofMending)
        {
            Prayer_of_Mending.Launch();
            return;
        }
        else
        {
            if (Renew.KnownSpell && Renew.IsSpellUsable && !Renew.HaveBuff &&
                ObjectManager.Me.HealthPercent < 90 && MySettings.UseRenew)
            {
                Renew.Launch();
                return;
            }
        }
    }

    private void Defense_Cycle()
    {
        if (ObjectManager.Me.HealthPercent < 20 && Psychic_Scream.IsSpellUsable && Psychic_Scream.KnownSpell
            && MySettings.UsePsychicScream)
        {
            Psychic_Scream.Launch();
            OnCD = new Timer(1000*8);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 20 && Dispersion.KnownSpell && Dispersion.IsSpellUsable
                 && MySettings.UseDispersion)
        {
            if (Renew.KnownSpell && Renew.IsSpellUsable && MySettings.UseRenew)
            {
                Renew.Launch();
                Thread.Sleep(1500);
            }
            Dispersion.Launch();
            OnCD = new Timer(1000*6);
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() >= 2 && ObjectManager.Me.HealthPercent < 35 &&
                 Void_Tendrils.IsSpellUsable && Void_Tendrils.KnownSpell
                 && MySettings.UseVoidTendrils)
        {
            Void_Tendrils.Launch();
            OnCD = new Timer(1000*10);
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() >= 2 && ObjectManager.Me.HealthPercent < 35 &&
                 Psyfiend.IsSpellUsable &&
                 Psyfiend.KnownSpell
                 && MySettings.UsePsyfiend)
        {
            Psyfiend.Launch();
            OnCD = new Timer(1000*10);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 70 && Spectral_Guise.IsSpellUsable && Spectral_Guise.KnownSpell
                 && MySettings.UseSpectralGuise)
        {
            if (Renew.KnownSpell && Renew.IsSpellUsable && MySettings.UseRenew)
            {
                Renew.Launch();
                Thread.Sleep(1500);
            }
            Spectral_Guise.Launch();
            OnCD = new Timer(1000*3);
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

    private void Decast()
    {
        if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && MySettings.UseSilence
            && Silence.KnownSpell && Silence.IsSpellUsable && Silence.IsDistanceGood)
        {
            Silence.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && MySettings.UsePsychicHorror
                && Psychic_Horror.KnownSpell && Psychic_Horror.IsSpellUsable && Psychic_Horror.IsDistanceGood)
            {
                Psychic_Horror.Launch();
                return;
            }
        }
    }

    private void AvoidMelee()
    {
        while (ObjectManager.Target.GetDistance < 3 && ObjectManager.Target.InCombat)
        {
            Keybindings.PressKeybindings(nManager.Wow.Enums.Keybindings.MOVEBACKWARD);
        }
    }

    #region Nested type: PriestShadowSettings

    [Serializable]
    public class PriestShadowSettings : Settings
    {
        public bool UseAlchFlask = true;
        public bool UseArcaneTorrent = true;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseCascade = true;
        public bool UseDesperatePrayer = true;
        public bool UseDevouringPlague = true;
        public bool UseDispersion = true;
        public bool UseDivineStar = true;
        public bool UseEngGlove = true;
        public bool UseFlashHeal = true;
        public bool UseGiftoftheNaaru = true;
        public bool UseHalo = true;
        public bool UseHymnofHope = true;
        public bool UseInnerFire = true;
        public bool UseInnerWill = false;
        public bool UseLevitate = false;
        public bool UseLifeblood = true;
        public bool UseLowCombat = true;
        public bool UseMindBlast = true;
        public bool UseMindFlay = true;
        public bool UseMindSear = true;
        public bool UseMindSpike = true;
        public bool UsePowerInfusion = true;
        public bool UsePowerWordFortitude = true;
        public bool UsePowerWordShield = true;
        public bool UsePrayerofMending = true;
        public bool UsePsychicHorror = true;
        public bool UsePsychicScream = true;
        public bool UsePsyfiend = true;
        public bool UseRenew = true;
        public bool UseShadowWordDeath = true;
        public bool UseShadowWordInsanity = true;
        public bool UseShadowWordPain = true;
        public bool UseShadowfiend = true;
        public bool UseShadowform = true;
        public bool UseSilence = true;
        public bool UseSpectralGuise = true;
        public bool UseStoneform = true;
        public bool UseTrinket = true;
        public bool UseVampiricEmbrace = true;
        public bool UseVampiricTouch = true;
        public bool UseVoidTendrils = true;
        public bool UseWarStomp = true;

        public PriestShadowSettings()
        {
            ConfigWinForm(new Point(400, 400), "Shadow Priest Settings");
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
            AddControlInWinForm("Use Cascade", "UseCascade", "Offensive Spell");
            AddControlInWinForm("Use Devouring Plague", "UseDevoringPlague", "Offensive Spell");
            AddControlInWinForm("Use DivineStar", "UseDivineStar", "Offensive Spell");
            AddControlInWinForm("Use Halo", "UseHalo", "Offensive Spell");
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
            AddControlInWinForm("Use Prayer of Mending", "UsePrayerofMending", "Healing Spell");
            AddControlInWinForm("Use Renew", "UseRenew", "Healing Spell");
            AddControlInWinForm("Use Vampiric Embrace", "UseVampiricEmbrace", "Healing Spell");
            /* Game Settings */
            AddControlInWinForm("Use Low Combat Settings", "UseLowCombat", "Game Settings");
            AddControlInWinForm("Use Trinket", "UseTrinket", "Game Settings");
            AddControlInWinForm("Use Engineering Gloves", "UseEngGlove", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
        }

        public static PriestShadowSettings CurrentSetting { get; set; }

        public static PriestShadowSettings GetSettings()
        {
            string CurrentSettingsFile = Application.StartupPath + "\\CustomClasses\\Settings\\Priest_Shadow.xml";
            if (File.Exists(CurrentSettingsFile))
            {
                return
                    CurrentSetting = Load<PriestShadowSettings>(CurrentSettingsFile);
            }
            else
            {
                return new PriestShadowSettings();
            }
        }
    }

    #endregion
}

public class Priest_Discipline
{
    private readonly PriestDisciplineSettings MySettings = PriestDisciplineSettings.GetSettings();

    private Timer AlchFlask_Timer = new Timer(0);
    private Timer Engineering_Timer = new Timer(0);
    private Timer OnCD = new Timer(0);
    private Timer Trinket_Timer = new Timer(0);

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

    #region Priest Buffs

    private readonly Spell Inner_Fire = new Spell("Inner Fire");
    private readonly Spell Inner_Will = new Spell("Inner Will");
    private readonly Spell Levitate = new Spell("Levitate");
    private readonly Spell Power_Word_Fortitude = new Spell("Power Word: Fortitude");
    private Timer Levitate_Timer = new Timer(0);

    #endregion

    #region Offensive Spell

    private readonly Spell Cascade = new Spell("Cascade");
    private readonly Spell Divine_Star = new Spell("Divine Star");
    private readonly Spell Halo = new Spell("Halo");
    private readonly Spell Mind_Sear = new Spell("Mind Sear");
    private readonly Spell Power_Word_Solace = new Spell("Power Word: Solace");
    private readonly Spell Shadow_Word_Death = new Spell("Shadow Word: Death");
    private readonly Spell Shadow_Word_Pain = new Spell("Shadow Word: Pain");
    private readonly Spell Smite = new Spell("Smite");
    private Timer Shadow_Word_Pain_Timer = new Timer(0);

    #endregion

    #region Healing Cooldown

    private readonly Spell Archangel = new Spell("Archangel");
    private readonly Spell Inner_Focus = new Spell("Inner Focus");
    private readonly Spell Power_Infusion = new Spell("Power Infusion");
    private readonly Spell Shadowfiend = new Spell("Shadowfiend");

    #endregion

    #region Defensive Cooldown

    private readonly Spell Pain_Suppression = new Spell("Pain Suppression");
    private readonly Spell Power_Word_Barrier = new Spell("Power Word: Barrier");
    private readonly Spell Power_Word_Shield = new Spell("Power Word: Shield");
    private readonly Spell Psychic_Scream = new Spell("Psychic Scream");
    private readonly Spell Psyfiend = new Spell("Psyfiend");
    private readonly Spell Spectral_Guise = new Spell("Spectral Guise");
    private readonly Spell Void_Tendrils = new Spell("Void Tendrils");

    #endregion

    #region Healing Spell

    private readonly Spell Desperate_Prayer = new Spell("Desperate Prayer");
    private readonly Spell Flash_Heal = new Spell("Flash Heal");
    private readonly Spell Greater_Heal = new Spell("Greater Heal");
    private readonly Spell Heal_Spell = new Spell("Heal");
    private readonly Spell Holy_Fire = new Spell("Holy Fire");
    private readonly Spell Hymn_of_Hope = new Spell("Hymn of Hope");
    private readonly Spell Penance = new Spell("Penance");
    private readonly Spell Prayer_of_Healing = new Spell("Prayer of Healing");
    private readonly Spell Prayer_of_Mending = new Spell("Prayer of Mending");
    private readonly Spell Renew = new Spell("Renew");
    private readonly Spell Spirit_Shell = new Spell("Spirit Shell");
    private Timer Renew_Timer = new Timer(0);

    #endregion

    public Priest_Discipline()
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
                            (Holy_Fire.IsDistanceGood || Shadow_Word_Pain.IsDistanceGood))
                        {
                            Pull();
                            lastTarget = ObjectManager.Me.Target;
                        }
                        else
                            Combat();
                    }
                    else if (!ObjectManager.Me.IsCast)
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
        if (Holy_Fire.IsSpellUsable && Holy_Fire.KnownSpell && Holy_Fire.IsDistanceGood
            && MySettings.UseHolyFire)
        {
            Holy_Fire.Launch();
            return;
        }
        else
        {
            if (Shadow_Word_Pain.IsSpellUsable && Shadow_Word_Pain.KnownSpell && Shadow_Word_Pain.IsDistanceGood
                && MySettings.UseShadowWordPain)
            {
                Shadow_Word_Pain.Launch();
                Shadow_Word_Pain_Timer = new Timer(1000*14);
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

    private void Buff_Levitate()
    {
        if (!Fight.InFight && Levitate.KnownSpell && Levitate.IsSpellUsable && MySettings.UseLevitate
            && (!Levitate.HaveBuff || Levitate_Timer.IsReady))
        {
            Levitate.Launch();
            Levitate_Timer = new Timer(1000*60*9);
        }
    }

    public void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

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

    public void Healing_Burst()
    {
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
        else if (Power_Infusion.IsSpellUsable && Power_Infusion.KnownSpell
                 && MySettings.UsePowerInfusion && ObjectManager.Target.GetDistance < 40)
        {
            Power_Infusion.Launch();
            return;
        }
        else if (Archangel.IsSpellUsable && Archangel.KnownSpell && ObjectManager.Me.BuffStack(81661) > 4
                 && MySettings.UseArchangel && ObjectManager.Target.GetDistance < 40)
        {
            Archangel.Launch();
            return;
        }
        else if (Spirit_Shell.IsSpellUsable && Spirit_Shell.KnownSpell && ObjectManager.Me.HealthPercent > 80
                 && MySettings.UseSpiritShell && ObjectManager.Target.InCombat)
        {
            Spirit_Shell.Launch();
            return;
        }
        else
        {
            if (Shadowfiend.IsSpellUsable && Shadowfiend.KnownSpell && Shadowfiend.IsDistanceGood
                && MySettings.UseShadowfiend)
            {
                Shadowfiend.Launch();
                return;
            }
        }
    }

    public void DPS_Cycle()
    {
        if (ObjectManager.Me.ManaPercentage < 80 && Arcane_Torrent.KnownSpell && Arcane_Torrent.IsSpellUsable
            && MySettings.UseArcaneTorrent)
        {
            Arcane_Torrent.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 2 && Cascade.IsSpellUsable && Cascade.KnownSpell
                 && Cascade.IsDistanceGood && MySettings.UseCascade)
        {
            Cascade.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 2 && Divine_Star.IsSpellUsable && Divine_Star.KnownSpell
                 && Divine_Star.IsDistanceGood && MySettings.UseDivineStar)
        {
            Divine_Star.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 2 && Halo.IsSpellUsable && Halo.KnownSpell
                 && Halo.IsDistanceGood && MySettings.UseHalo)
        {
            Halo.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 4 && Mind_Sear.IsSpellUsable && Mind_Sear.KnownSpell
                 && Mind_Sear.IsDistanceGood && !ObjectManager.Me.IsCast && MySettings.UseMindSear)
        {
            Mind_Sear.Launch();
            return;
        }
        else if (Shadow_Word_Death.IsSpellUsable && Shadow_Word_Death.IsDistanceGood && Shadow_Word_Death.KnownSpell
                 && ObjectManager.Target.HealthPercent < 20 && MySettings.UseShadowWordDeath)
        {
            Shadow_Word_Death.Launch();
            return;
        }
        else if (Shadow_Word_Pain.KnownSpell && Shadow_Word_Pain.IsSpellUsable
                 && Shadow_Word_Pain.IsDistanceGood && MySettings.UseShadowWordPain
                 && (!Shadow_Word_Pain.TargetHaveBuff || Shadow_Word_Pain_Timer.IsReady))
        {
            Shadow_Word_Pain.Launch();
            Shadow_Word_Pain_Timer = new Timer(1000*14);
            return;
        }
        else if (Power_Word_Solace.KnownSpell && Power_Word_Solace.IsDistanceGood
                 && Power_Word_Solace.IsSpellUsable && MySettings.UsePowerWordSolace
                 && ObjectManager.Me.ManaPercentage < 50)
        {
            Power_Word_Solace.Launch();
            return;
        }
        else if (Penance.IsSpellUsable && Penance.IsDistanceGood && Penance.KnownSpell
                 && MySettings.UsePenance)
        {
            Penance.Launch();
            return;
        }
        else if (Holy_Fire.IsSpellUsable && Holy_Fire.IsDistanceGood && Holy_Fire.KnownSpell
                 && MySettings.UseHolyFire)
        {
            Holy_Fire.Launch();
            return;
        }
        else if (Smite.IsSpellUsable && Smite.KnownSpell && Smite.IsDistanceGood
                 && MySettings.UseSmite && Shadow_Word_Pain.TargetHaveBuff
                 && ObjectManager.GetNumberAttackPlayer() < 5)
        {
            Smite.Launch();
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

    private void Heal()
    {
        if (ObjectManager.Me.HealthPercent < 95 && !Fight.InFight && ObjectManager.GetNumberAttackPlayer() == 0)
        {
            if (Flash_Heal.KnownSpell && Flash_Heal.IsSpellUsable && MySettings.UseFlashHeal)
            {
                Flash_Heal.Launch(false);
                return;
            }
        }
        else if (ObjectManager.Me.HealthPercent < 90 && Inner_Focus.KnownSpell && Inner_Focus.IsSpellUsable
                 && MySettings.UseInnerFocus && !Inner_Focus.HaveBuff)
        {
            Inner_Focus.Launch();
            return;
        }
        else if (!Fight.InFight && ObjectManager.Me.ManaPercentage < 40 && Hymn_of_Hope.KnownSpell &&
                 Hymn_of_Hope.IsSpellUsable
                 && ObjectManager.GetNumberAttackPlayer() == 0 && MySettings.UseHymnofHope)
        {
            Hymn_of_Hope.Launch(false);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 65 && Desperate_Prayer.KnownSpell && Desperate_Prayer.IsSpellUsable
                 && MySettings.UseDesperatePrayer)
        {
            Desperate_Prayer.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 60 && Flash_Heal.KnownSpell && Flash_Heal.IsSpellUsable
                 && MySettings.UseFlashHeal)
        {
            Flash_Heal.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 70 && Greater_Heal.KnownSpell && Greater_Heal.IsSpellUsable
                 && MySettings.UseGreaterHeal)
        {
            Greater_Heal.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && Gift_of_the_Naaru.IsSpellUsable && Gift_of_the_Naaru.KnownSpell
                 && MySettings.UseGiftoftheNaaru)
        {
            Gift_of_the_Naaru.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && War_Stomp.IsSpellUsable && War_Stomp.KnownSpell
                 && MySettings.UseWarStomp && ObjectManager.Target.GetDistance < 8)
        {
            War_Stomp.Launch();
            return;
        }
        else if (Power_Word_Shield.KnownSpell && Power_Word_Shield.IsSpellUsable
                 && !Power_Word_Shield.HaveBuff && MySettings.UsePowerWordShield
                 && !ObjectManager.Me.HaveBuff(6788) && (ObjectManager.GetNumberAttackPlayer() > 0
                                                         || ObjectManager.Me.GetMove))
        {
            Power_Word_Shield.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 50 && Prayer_of_Healing.KnownSpell && Prayer_of_Healing.IsSpellUsable
                 && MySettings.UsePrayerofHealing)
        {
            Prayer_of_Healing.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 50 && Prayer_of_Mending.KnownSpell && Prayer_of_Mending.IsSpellUsable
                 && MySettings.UsePrayerofMending)
        {
            Prayer_of_Mending.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 70 && Heal_Spell.KnownSpell && Heal_Spell.IsSpellUsable
                 && (MySettings.UseHeal || !Greater_Heal.KnownSpell))
        {
            Heal_Spell.Launch();
            return;
        }
        else
        {
            if (Renew.KnownSpell && Renew.IsSpellUsable && !Renew.HaveBuff &&
                ObjectManager.Me.HealthPercent < 90 && MySettings.UseRenew)
            {
                Renew.Launch();
                return;
            }
        }
    }

    private void Defense_Cycle()
    {
        if (ObjectManager.Me.HealthPercent < 20 && Psychic_Scream.IsSpellUsable && Psychic_Scream.KnownSpell
            && MySettings.UsePsychicScream)
        {
            Psychic_Scream.Launch();
            OnCD = new Timer(1000*8);
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() >= 2 && ObjectManager.Me.HealthPercent < 35 &&
                 Void_Tendrils.IsSpellUsable && Void_Tendrils.KnownSpell
                 && MySettings.UseVoidTendrils)
        {
            Void_Tendrils.Launch();
            OnCD = new Timer(1000*10);
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() >= 2 && ObjectManager.Me.HealthPercent < 35 &&
                 Psyfiend.IsSpellUsable &&
                 Psyfiend.KnownSpell
                 && MySettings.UsePsyfiend)
        {
            Psyfiend.Launch();
            OnCD = new Timer(1000*10);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 70 && Spectral_Guise.IsSpellUsable && Spectral_Guise.KnownSpell
                 && MySettings.UseSpectralGuise)
        {
            if (Renew.KnownSpell && Renew.IsSpellUsable && MySettings.UseRenew)
            {
                Renew.Launch();
                Thread.Sleep(1500);
            }
            Spectral_Guise.Launch();
            OnCD = new Timer(1000*3);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 60 && Power_Word_Barrier.IsSpellUsable && Power_Word_Barrier.KnownSpell
                 && MySettings.UsePowerWordBarrier)
        {
            SpellManager.CastSpellByIDAndPosition(62618, ObjectManager.Me.Position);
            OnCD = new Timer(1000*10);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 70 && Pain_Suppression.IsSpellUsable && Pain_Suppression.KnownSpell
                 && MySettings.UsePainSuppression)
        {
            Pain_Suppression.Launch();
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

    private void AvoidMelee()
    {
        while (ObjectManager.Target.GetDistance < 3 && ObjectManager.Target.InCombat)
        {
            Keybindings.PressKeybindings(nManager.Wow.Enums.Keybindings.MOVEBACKWARD);
        }
    }

    #region Nested type: PriestDisciplineSettings

    [Serializable]
    public class PriestDisciplineSettings : Settings
    {
        public bool UseAlchFlask = true;
        public bool UseArcaneTorrent = true;
        public bool UseArchangel = true;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseCascade = true;
        public bool UseDesperatePrayer = true;
        public bool UseDivineStar = true;
        public bool UseEngGlove = true;
        public bool UseFlashHeal = true;
        public bool UseGiftoftheNaaru = true;
        public bool UseGreaterHeal = true;
        public bool UseHalo = true;
        public bool UseHeal = true;
        public bool UseHolyFire = true;
        public bool UseHymnofHope = true;
        public bool UseInnerFire = true;
        public bool UseInnerFocus = true;
        public bool UseInnerWill = false;
        public bool UseLevitate = false;
        public bool UseLifeblood = true;
        public bool UseMindSear = true;
        public bool UsePainSuppression = true;
        public bool UsePenance = true;
        public bool UsePowerInfusion = true;
        public bool UsePowerWordBarrier = true;
        public bool UsePowerWordFortitude = true;
        public bool UsePowerWordShield = true;
        public bool UsePowerWordSolace = true;
        public bool UsePrayerofHealing = false;
        public bool UsePrayerofMending = true;
        public bool UsePsychicScream = true;
        public bool UsePsyfiend = true;
        public bool UseRenew = true;
        public bool UseShadowWordDeath = true;
        public bool UseShadowWordPain = true;
        public bool UseShadowfiend = true;
        public bool UseSpectralGuise = true;
        public bool UseSpiritShell = true;
        public bool UseSmite = true;
        public bool UseStoneform = true;
        public bool UseTrinket = true;
        public bool UseVoidTendrils = true;
        public bool UseWarStomp = true;

        public PriestDisciplineSettings()
        {
            ConfigWinForm(new Point(400, 400), "Discipline Priest Settings");
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
            /* Offensive Spell */
            AddControlInWinForm("Use Cascade", "UseCascade", "Offensive Spell");
            AddControlInWinForm("Use Divine Star", "Use Divine Star", "Offensive Spell");
            AddControlInWinForm("Use Halo", "UseHalo", "Offensive Spell");
            AddControlInWinForm("Use Holy Fire", "UseHolyFire", "Offensive Spell");
            AddControlInWinForm("Use Mind Sear", "UseMindSear", "Offensive Spell");
            AddControlInWinForm("Use Penance", "UsePenance", "Offensive Spell");
            AddControlInWinForm("Use Shadow Word: Death", "UseShadowWordDeath", "Offensive Spell");
            AddControlInWinForm("Use Shadow Word: Pain", "UseShadowWordPain", "Offensive Spell");
            AddControlInWinForm("Use Smite", "UseSmite", "Offensive Spell");
            /* Healing Cooldown */
            AddControlInWinForm("Use Archangel", "UseArchangel", "Healing Cooldown");
            AddControlInWinForm("Use Power Infusion", "UsePowerInfusion", "Healing Cooldown");
            AddControlInWinForm("Use Shadowfiend", "UseShadowfiend", "Healing Cooldown");
            AddControlInWinForm("Use Spirit Shell", "UseSpiritShell", "Healing Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Pain Suppression", "UsePainSuppression", "Defensive Cooldown");
            AddControlInWinForm("Use Power Word: Barrier", "UsePowerWordBarrier", "Defensive Cooldown");
            AddControlInWinForm("Use Power Word: Shield", "UsePowerWordShield", "Defensive Cooldown");
            AddControlInWinForm("Use Psychic Scream", "UsePsychicScream", "Defensive Cooldown");
            AddControlInWinForm("Use Psyfiend", "UsePsyfiend", "Defensive Cooldown");
            AddControlInWinForm("Use Spectral Guise", "UseSpectralGuise", "Defensive Cooldown");
            AddControlInWinForm("Use Void Tendrils", "UseVoidTendrils", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Desperate Prayer", "UseDesperatePrayer", "Healing Spell");
            AddControlInWinForm("Use Flash Heal", "UseFlashHeal", "Healing Spell");
            AddControlInWinForm("Use Greater Heal", "UseGreaterHeal", "Healing Spell");
            AddControlInWinForm("Use Heal", "UseHeal", "Healing Spell");
            AddControlInWinForm("Use Hymn of Hope", "UseHymnofHope", "Healing Spell");
            AddControlInWinForm("Use Inner Focus", "UseInnerFocus", "Healing Spell");
            AddControlInWinForm("Use Prayer of Mending", "UsePrayerofMending", "Healing Spell");
            AddControlInWinForm("Use Renew", "UseRenew", "Healing Spell");
            /* Game Settings */
            AddControlInWinForm("Use Trinket", "UseTrinket", "Game Settings");
            AddControlInWinForm("Use Engineering Gloves", "UseEngGlove", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
        }

        public static PriestDisciplineSettings CurrentSetting { get; set; }

        public static PriestDisciplineSettings GetSettings()
        {
            string CurrentSettingsFile = Application.StartupPath + "\\CustomClasses\\Settings\\Priest_Discipline.xml";
            if (File.Exists(CurrentSettingsFile))
            {
                return
                    CurrentSetting = Load<PriestDisciplineSettings>(CurrentSettingsFile);
            }
            else
            {
                return new PriestDisciplineSettings();
            }
        }
    }

    #endregion
}

public class Priest_Holy
{
    private readonly PriestHolySettings MySettings = PriestHolySettings.GetSettings();

    private Timer AlchFlask_Timer = new Timer(0);
    private Timer Engineering_Timer = new Timer(0);
    private Timer OnCD = new Timer(0);
    private Timer Trinket_Timer = new Timer(0);

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

    #region Priest Buffs

    private readonly Spell Chakra_Chastise = new Spell("Chakra: Chastise");
    private readonly Spell Chakra_Sanctuary = new Spell("Chakra: Sanctuary");
    private readonly Spell Chakra_Serenity = new Spell("Chakra: Serenity");
    private readonly Spell Inner_Fire = new Spell("Inner Fire");
    private readonly Spell Inner_Will = new Spell("Inner Will");
    private readonly Spell Levitate = new Spell("Levitate");
    private readonly Spell Power_Word_Fortitude = new Spell("Power Word: Fortitude");
    private Timer Levitate_Timer = new Timer(0);

    #endregion

    #region Offensive Spell

    private readonly Spell Cascade = new Spell("Cascade");
    private readonly Spell Divine_Star = new Spell("Divine Star");
    private readonly Spell Halo = new Spell("Halo");
    private readonly Spell Holy_Word_Chastise = new Spell("Holy Word: Chastise");
    private readonly Spell Mind_Sear = new Spell("Mind Sear");
    private readonly Spell Power_Word_Solace = new Spell("Power Word: Solace");
    private readonly Spell Shadow_Word_Death = new Spell("Shadow Word: Death");
    private readonly Spell Shadow_Word_Pain = new Spell("Shadow Word: Pain");
    private readonly Spell Smite = new Spell("Smite");
    private Timer Shadow_Word_Pain_Timer = new Timer(0);

    #endregion

    #region Healing Cooldown

    private readonly Spell Divine_Hymn = new Spell("Divine Hymn");
    private readonly Spell Light_Well = new Spell("Light Well");
    private readonly Spell Power_Infusion = new Spell("Power Infusion");
    private readonly Spell Shadowfiend = new Spell("Shadowfiend");

    #endregion

    #region Defensive Cooldown

    private readonly Spell Guardian_Spirit = new Spell("Guardian Spirit");
    private readonly Spell Power_Word_Shield = new Spell("Power Word: Shield");
    private readonly Spell Psychic_Scream = new Spell("Psychic Scream");
    private readonly Spell Psyfiend = new Spell("Psyfiend");
    private readonly Spell Spectral_Guise = new Spell("Spectral Guise");
    private readonly Spell Void_Tendrils = new Spell("Void Tendrils");

    #endregion

    #region Healing Spell

    private readonly Spell Circle_of_Healing = new Spell("Circle of Healing");
    private readonly Spell Desperate_Prayer = new Spell("Desperate Prayer");
    private readonly Spell Flash_Heal = new Spell("Flash Heal");
    private readonly Spell Greater_Heal = new Spell("Greater Heal");
    private readonly Spell Heal_Spell = new Spell("Heal");
    private readonly Spell Holy_Fire = new Spell("Holy Fire");
    private readonly Spell Hymn_of_Hope = new Spell("Hymn of Hope");
    private readonly Spell Prayer_of_Healing = new Spell("Prayer of Healing");
    private readonly Spell Prayer_of_Mending = new Spell("Prayer of Mending");
    private readonly Spell Renew = new Spell("Renew");
    private Timer Renew_Timer = new Timer(0);

    #endregion

    public Priest_Holy()
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
                            (Holy_Fire.IsDistanceGood || Shadow_Word_Pain.IsDistanceGood))
                        {
                            Pull();
                            lastTarget = ObjectManager.Me.Target;
                        }
                        else
                            Combat();
                    }
                    else if (!ObjectManager.Me.IsCast)
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
        if (Holy_Fire.IsSpellUsable && Holy_Fire.KnownSpell && Holy_Fire.IsDistanceGood
            && MySettings.UseHolyFire)
        {
            Holy_Fire.Launch();
            return;
        }
        else
        {
            if (Shadow_Word_Pain.IsSpellUsable && Shadow_Word_Pain.KnownSpell && Shadow_Word_Pain.IsDistanceGood
                && MySettings.UseShadowWordPain)
            {
                Shadow_Word_Pain.Launch();
                Shadow_Word_Pain_Timer = new Timer(1000*14);
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

    private void Buff_Levitate()
    {
        if (!Fight.InFight && Levitate.KnownSpell && Levitate.IsSpellUsable && MySettings.UseLevitate
            && (!Levitate.HaveBuff || Levitate_Timer.IsReady))
        {
            Levitate.Launch();
            Levitate_Timer = new Timer(1000*60*9);
        }
    }

    public void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

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
        else if (Chakra_Chastise.KnownSpell && Chakra_Chastise.IsSpellUsable && !Chakra_Chastise.HaveBuff
                 && MySettings.UseChakraChastise)
        {
            Chakra_Chastise.Launch();
            return;
        }
        else if (Chakra_Sanctuary.KnownSpell && Chakra_Sanctuary.IsSpellUsable && !Chakra_Sanctuary.HaveBuff
                 && !MySettings.UseChakraChastise && MySettings.UseChakraSanctuary)
        {
            Chakra_Sanctuary.Launch();
            return;
        }
        else if (Chakra_Serenity.KnownSpell && Chakra_Serenity.IsSpellUsable && !Chakra_Serenity.HaveBuff
                 && !MySettings.UseChakraChastise && !MySettings.UseChakraSanctuary && MySettings.UseChakraSerenity)
        {
            Chakra_Serenity.Launch();
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

    public void Healing_Burst()
    {
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
        else if (Power_Infusion.IsSpellUsable && Power_Infusion.KnownSpell
                 && MySettings.UsePowerInfusion && ObjectManager.Target.GetDistance < 40)
        {
            Power_Infusion.Launch();
            return;
        }
        else
        {
            if (Shadowfiend.IsSpellUsable && Shadowfiend.KnownSpell && Shadowfiend.IsDistanceGood
                && MySettings.UseShadowfiend)
            {
                Shadowfiend.Launch();
                return;
            }
        }
    }

    public void DPS_Cycle()
    {
        if (ObjectManager.Me.ManaPercentage < 80 && Arcane_Torrent.KnownSpell && Arcane_Torrent.IsSpellUsable
            && MySettings.UseArcaneTorrent)
        {
            Arcane_Torrent.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 2 && Cascade.IsSpellUsable && Cascade.KnownSpell
                 && Cascade.IsDistanceGood && MySettings.UseCascade)
        {
            Cascade.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 2 && Divine_Star.IsSpellUsable && Divine_Star.KnownSpell
                 && Divine_Star.IsDistanceGood && MySettings.UseDivineStar)
        {
            Divine_Star.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 2 && Halo.IsSpellUsable && Halo.KnownSpell
                 && Halo.IsDistanceGood && MySettings.UseHalo)
        {
            Halo.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 4 && Mind_Sear.IsSpellUsable && Mind_Sear.KnownSpell
                 && Mind_Sear.IsDistanceGood && !ObjectManager.Me.IsCast && MySettings.UseMindSear)
        {
            Mind_Sear.Launch();
            return;
        }
        else if (Shadow_Word_Death.IsSpellUsable && Shadow_Word_Death.IsDistanceGood && Shadow_Word_Death.KnownSpell
                 && ObjectManager.Target.HealthPercent < 20 && MySettings.UseShadowWordDeath)
        {
            Shadow_Word_Death.Launch();
            return;
        }
        else if (Shadow_Word_Pain.KnownSpell && Shadow_Word_Pain.IsSpellUsable
                 && Shadow_Word_Pain.IsDistanceGood && MySettings.UseShadowWordPain
                 && (!Shadow_Word_Pain.TargetHaveBuff || Shadow_Word_Pain_Timer.IsReady))
        {
            Shadow_Word_Pain.Launch();
            Shadow_Word_Pain_Timer = new Timer(1000*14);
            return;
        }
        else if (Power_Word_Solace.KnownSpell && Power_Word_Solace.IsDistanceGood
                 && Power_Word_Solace.IsSpellUsable && MySettings.UsePowerWordSolace
                 && ObjectManager.Me.ManaPercentage < 50)
        {
            Power_Word_Solace.Launch();
            return;
        }
        else if (Holy_Word_Chastise.IsSpellUsable && Holy_Word_Chastise.IsDistanceGood && Holy_Word_Chastise.KnownSpell
                 && MySettings.UseHolyWordChastise)
        {
            Holy_Word_Chastise.Launch();
            return;
        }
        else if (Holy_Fire.IsSpellUsable && Holy_Fire.IsDistanceGood && Holy_Fire.KnownSpell
                 && MySettings.UseHolyFire)
        {
            Holy_Fire.Launch();
            return;
        }
        else if (Smite.IsSpellUsable && Smite.KnownSpell && Smite.IsDistanceGood
                 && MySettings.UseSmite && Shadow_Word_Pain.TargetHaveBuff
                 && ObjectManager.GetNumberAttackPlayer() < 5)
        {
            Smite.Launch();
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

    private void Heal()
    {
        if (ObjectManager.Me.HealthPercent < 95 && !Fight.InFight && ObjectManager.GetNumberAttackPlayer() == 0)
        {
            if (Flash_Heal.KnownSpell && Flash_Heal.IsSpellUsable && MySettings.UseFlashHeal)
            {
                Flash_Heal.Launch(false);
                return;
            }
        }
        else if (ObjectManager.Me.HealthPercent < 30 && Divine_Hymn.KnownSpell && Divine_Hymn.IsSpellUsable
                 && MySettings.UseDivineHymn)
        {
            Divine_Hymn.Launch();
            return;
        }
        else if (!Fight.InFight && ObjectManager.Me.ManaPercentage < 40 && Hymn_of_Hope.KnownSpell &&
                 Hymn_of_Hope.IsSpellUsable
                 && ObjectManager.GetNumberAttackPlayer() == 0 && MySettings.UseHymnofHope)
        {
            Hymn_of_Hope.Launch(false);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 65 && Desperate_Prayer.KnownSpell && Desperate_Prayer.IsSpellUsable
                 && MySettings.UseDesperatePrayer)
        {
            Desperate_Prayer.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 60 && Flash_Heal.KnownSpell && Flash_Heal.IsSpellUsable
                 && MySettings.UseFlashHeal)
        {
            Flash_Heal.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 70 && Greater_Heal.KnownSpell && Greater_Heal.IsSpellUsable
                 && MySettings.UseGreaterHeal)
        {
            Greater_Heal.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && Gift_of_the_Naaru.IsSpellUsable && Gift_of_the_Naaru.KnownSpell
                 && MySettings.UseGiftoftheNaaru)
        {
            Gift_of_the_Naaru.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && War_Stomp.IsSpellUsable && War_Stomp.KnownSpell
                 && MySettings.UseWarStomp && ObjectManager.Target.GetDistance < 8)
        {
            War_Stomp.Launch();
            return;
        }
        else if (Power_Word_Shield.KnownSpell && Power_Word_Shield.IsSpellUsable
                 && !Power_Word_Shield.HaveBuff && MySettings.UsePowerWordShield
                 && !ObjectManager.Me.HaveBuff(6788) && (ObjectManager.GetNumberAttackPlayer() > 0
                                                         || ObjectManager.Me.GetMove))
        {
            Power_Word_Shield.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 50 && Prayer_of_Healing.KnownSpell && Prayer_of_Healing.IsSpellUsable
                 && MySettings.UsePrayerofHealing)
        {
            Prayer_of_Healing.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 50 && Circle_of_Healing.KnownSpell && Circle_of_Healing.IsSpellUsable
                 && MySettings.UseCircleofHealing)
        {
            SpellManager.CastSpellByIDAndPosition(34861, ObjectManager.Me.Position);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 50 && Prayer_of_Mending.KnownSpell && Prayer_of_Mending.IsSpellUsable
                 && MySettings.UsePrayerofMending)
        {
            Prayer_of_Mending.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 70 && Heal_Spell.KnownSpell && Heal_Spell.IsSpellUsable
                 && (MySettings.UseHeal || !Greater_Heal.KnownSpell))
        {
            Heal_Spell.Launch();
            return;
        }
        else if (Light_Well.KnownSpell && Light_Well.IsSpellUsable && MySettings.UseGlyphofLightspring
                 && ObjectManager.Me.HealthPercent < 95 && MySettings.UseLightWell)
        {
            SpellManager.CastSpellByIDAndPosition(724, ObjectManager.Target.Position);
            return;
        }
        else
        {
            if (Renew.KnownSpell && Renew.IsSpellUsable && !Renew.HaveBuff &&
                ObjectManager.Me.HealthPercent < 90 && MySettings.UseRenew)
            {
                Renew.Launch();
                return;
            }
        }
    }

    private void Defense_Cycle()
    {
        if (ObjectManager.Me.HealthPercent < 20 && Psychic_Scream.IsSpellUsable && Psychic_Scream.KnownSpell
            && MySettings.UsePsychicScream)
        {
            Psychic_Scream.Launch();
            OnCD = new Timer(1000*8);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 20 && Guardian_Spirit.KnownSpell && Guardian_Spirit.IsSpellUsable
                 && MySettings.UseGuardianSpirit)
        {
            Guardian_Spirit.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() >= 2 && ObjectManager.Me.HealthPercent < 35 &&
                 Void_Tendrils.IsSpellUsable && Void_Tendrils.KnownSpell
                 && MySettings.UseVoidTendrils)
        {
            Void_Tendrils.Launch();
            OnCD = new Timer(1000*10);
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() >= 2 && ObjectManager.Me.HealthPercent < 35 &&
                 Psyfiend.IsSpellUsable &&
                 Psyfiend.KnownSpell
                 && MySettings.UsePsyfiend)
        {
            Psyfiend.Launch();
            OnCD = new Timer(1000*10);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 70 && Spectral_Guise.IsSpellUsable && Spectral_Guise.KnownSpell
                 && MySettings.UseSpectralGuise)
        {
            if (Renew.KnownSpell && Renew.IsSpellUsable && MySettings.UseRenew)
            {
                Renew.Launch();
                Thread.Sleep(1500);
            }
            Spectral_Guise.Launch();
            OnCD = new Timer(1000*3);
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

    private void AvoidMelee()
    {
        while (ObjectManager.Target.GetDistance < 3 && ObjectManager.Target.InCombat)
        {
            Keybindings.PressKeybindings(nManager.Wow.Enums.Keybindings.MOVEBACKWARD);
        }
    }

    #region Nested type: PriestHolySettings

    [Serializable]
    public class PriestHolySettings : Settings
    {
        public bool UseAlchFlask = true;
        public bool UseArcaneTorrent = true;
        public bool UseArchangel = true;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseCascade = true;
        public bool UseChakraChastise = true;
        public bool UseChakraSanctuary = false;
        public bool UseChakraSerenity = false;
        public bool UseCircleofHealing = false;
        public bool UseDesperatePrayer = true;
        public bool UseDivineHymn = true;
        public bool UseDivineStar = true;
        public bool UseEngGlove = true;
        public bool UseFlashHeal = true;
        public bool UseGiftoftheNaaru = true;
        public bool UseGlyphofLightspring = false;
        public bool UseGreaterHeal = true;
        public bool UseGuardianSpirit = true;
        public bool UseHalo = true;
        public bool UseHeal = true;
        public bool UseHolyFire = true;
        public bool UseHolyWordChastise = true;
        public bool UseHymnofHope = true;
        public bool UseInnerFire = true;
        public bool UseInnerWill = false;
        public bool UseLevitate = false;
        public bool UseLifeblood = true;
        public bool UseLightWell = true;
        public bool UseMindSear = true;
        public bool UsePowerInfusion = true;
        public bool UsePowerWordFortitude = true;
        public bool UsePowerWordShield = true;
        public bool UsePowerWordSolace = true;
        public bool UsePrayerofHealing = false;
        public bool UsePrayerofMending = true;
        public bool UsePsychicScream = true;
        public bool UsePsyfiend = true;
        public bool UseRenew = true;
        public bool UseShadowWordDeath = true;
        public bool UseShadowWordPain = true;
        public bool UseShadowfiend = true;
        public bool UseSpectralGuise = true;
        public bool UseSmite = true;
        public bool UseStoneform = true;
        public bool UseTrinket = true;
        public bool UseVoidTendrils = true;
        public bool UseWarStomp = true;

        public PriestHolySettings()
        {
            ConfigWinForm(new Point(400, 400), "Holy Priest Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");
            AddControlInWinForm("Use Lifeblood", "UseLifeblood", "Professions & Racials");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            /* Priest Buffs */
            AddControlInWinForm("Use Chakra: Chastise", "UseChakraChastise", "Priest Buffs");
            AddControlInWinForm("Use Chakra: Sanctuary", "UseChakraSanctuary", "Priest Buffs");
            AddControlInWinForm("Use Chakra: Serenity", "UseChakraSerenity", "Priest Buffs");
            AddControlInWinForm("Use Inner Fire", "UseInnerFire", "Priest Buffs");
            AddControlInWinForm("Use Inner Will", "UseInnerWill", "Priest Buffs");
            AddControlInWinForm("Use Levitate", "UseLevitate", "Priest Buffs");
            AddControlInWinForm("Use Power Word: Fortitude", "UsePowerWordFortitude", "Priest Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Cascade", "UseCascade", "Offensive Spell");
            AddControlInWinForm("Use Divine Star", "Use Divine Star", "Offensive Spell");
            AddControlInWinForm("Use Halo", "UseHalo", "Offensive Spell");
            AddControlInWinForm("Use Holy Fire", "UseHolyFire", "Offensive Spell");
            AddControlInWinForm("Use Holy Word: Chastise", "UseHolyWordChastise", "Offensive Spell");
            AddControlInWinForm("Use Mind Sear", "UseMindSear", "Offensive Spell");
            AddControlInWinForm("Use Shadow Word: Death", "UseShadowWordDeath", "Offensive Spell");
            AddControlInWinForm("Use Shadow Word: Pain", "UseShadowWordPain", "Offensive Spell");
            AddControlInWinForm("Use Smite", "UseSmite", "Offensive Spell");
            /* Healing Cooldown */
            AddControlInWinForm("Use Divine Hymn", "UseDivineHymn", "Healing Cooldown");
            AddControlInWinForm("Use Light Well", "UseLightWell", "Healing Cooldown");
            AddControlInWinForm("Use Power Infusion", "UsePowerInfusion", "Healing Cooldown");
            AddControlInWinForm("Use Shadowfiend", "UseShadowfiend", "Healing Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Guardian Spirit", "UseGuardianSpirit", "Defensive Cooldown");
            AddControlInWinForm("Use Power Word: Shield", "UsePowerWordShield", "Defensive Cooldown");
            AddControlInWinForm("Use Psychic Scream", "UsePsychicScream", "Defensive Cooldown");
            AddControlInWinForm("Use Psyfiend", "UsePsyfiend", "Defensive Cooldown");
            AddControlInWinForm("Use Spectral Guise", "UseSpectralGuise", "Defensive Cooldown");
            AddControlInWinForm("Use Void Tendrils", "UseVoidTendrils", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Circle of Healing", "UseCircleofHealing", "Healing Spell");
            AddControlInWinForm("Use Desperate Prayer", "UseDesperatePrayer", "Healing Spell");
            AddControlInWinForm("Use Flash Heal", "UseFlashHeal", "Healing Spell");
            AddControlInWinForm("Use Greater Heal", "UseGreaterHeal", "Healing Spell");
            AddControlInWinForm("Use Heal", "UseHeal", "Healing Spell");
            AddControlInWinForm("Use Hymn of Hope", "UseHymnofHope", "Healing Spell");
            AddControlInWinForm("Use Prayer of Mending", "UsePrayerofMending", "Healing Spell");
            AddControlInWinForm("Use Renew", "UseRenew", "Healing Spell");
            /* Game Settings */
            AddControlInWinForm("Use Trinket", "UseTrinket", "Game Settings");
            AddControlInWinForm("Use Engineering Gloves", "UseEngGlove", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
            AddControlInWinForm("Use Glyph of Lightspring", "UseGlyphofLightspring", "Game Settings");
        }

        public static PriestHolySettings CurrentSetting { get; set; }

        public static PriestHolySettings GetSettings()
        {
            string CurrentSettingsFile = Application.StartupPath + "\\CustomClasses\\Settings\\Priest_Holy.xml";
            if (File.Exists(CurrentSettingsFile))
            {
                return
                    CurrentSetting = Load<PriestHolySettings>(CurrentSettingsFile);
            }
            else
            {
                return new PriestHolySettings();
            }
        }
    }

    #endregion
}

#endregion
