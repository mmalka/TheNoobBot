/*
* CustomClass for TheNoobBot
* Credit : Rival, Geesus, Enelya, Marstor, Vesper, Neo2003
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
                            MessageBox.Show("There is no settings available for your Class/Specialisation.");
                        }
                        else
                        {
                            Logging.WriteFight("Shadow Priest Found");
                            new Shadow();
                        }
                    }
                    if (!Mind_Flay.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            MessageBox.Show("There is no settings available for your Class/Specialisation.");
                        }
                        else
                        {
                            Logging.WriteFight("Priest without Spec");
                            new Shadow();
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

public class Shadow
{
    #region InitializeSpell

    private Spell Angelic_Feather = new Spell("Angelic Feather");
    private Spell Binding_Heal = new Spell("Binding Heal");
    private Spell Desperate_Prayer = new Spell("Desperate Prayer");
    private Spell Devouring_Plague = new Spell("Devouring Plague");
    private Spell Dispel_Magic = new Spell("Dispel Magic");
    private Spell Dispersion = new Spell("Dispersion");
    private Spell Dominate_Mind = new Spell("Dominate Mind");
    private Spell Fade = new Spell("Fade");
    private Spell Fear_Ward = new Spell("Fear Ward");
    private Spell Flash_Heal = new Spell("Flash Heal");
    private Spell Hymn_of_Hope = new Spell("Hymn of Hope");
    private Spell Inner_Fire = new Spell("Inner Fire");
    private Spell Inner_Will = new Spell("Inner Will");
    private Spell Leap_of_Faith = new Spell("Leap of Faith");
    private Spell Levitate = new Spell("Levitate");
    private Spell Mass_Dispel = new Spell("Mass Dispel");
    private Spell Mind_Blast = new Spell("Mind Blast");
    private Spell Mind_Flay = new Spell("Mind Flay");
    private Spell Mind_Sear = new Spell("Mind Sear");
    private Spell Mind_Spike = new Spell("Mind Spike");
    private Spell Mind_Vision = new Spell("Mind Vision");
    private Spell Power_Infusion = new Spell("Power Infusion");
    private Spell Power_Word_Fortitude = new Spell("Power Word: Fortitude");
    private Spell Power_Word_Shield = new Spell("Power Word: Shield");
    private Spell Power_Word_Solace = new Spell("Power Word: Solace");
    private Spell Prayer_of_Mending = new Spell("Prayer of Mending");
    private Spell Psychic_Horror = new Spell("Psychic Horror");
    private Spell Psychic_Scream = new Spell("Psychic Scream");
    private Spell Psyfiend = new Spell("Psyfiend");
    private Spell Renew = new Spell("Renew");
    private Spell Resurrection = new Spell("Resurrection");
    private Spell Shackle_Undead = new Spell("Shackle Undead");
    private Spell Shadow_Word_Death = new Spell("Shadow Word: Death");
    private Spell Shadow_Word_Insanity = new Spell("Shadow Word: Insanity");
    private Spell Shadow_Word_Pain = new Spell("Shadow Word: Pain");
    private Spell Shadowfiend = new Spell("Shadowfiend");
    private Spell Shadowform = new Spell("Shadowform");
    private Spell Silence = new Spell("Silence");
    private Spell Smite = new Spell("Smite");
    private Spell Spectral_Guise = new Spell("Spectral Guise");
    private Spell Vampiric_Embrace = new Spell("Vampiric Embrace");
    private Spell Vampiric_Touch = new Spell("Vampiric Touch");
    private Spell Void_Tendrils = new Spell("Void Tendrils");

    // TIMER
    private Timer Devouring_Plague_Timer = new Timer(0);
    private Timer Renew_Timer = new Timer(0);
    private Timer Shadow_Word_Pain_Timer = new Timer(0);
    private Timer Vampiric_Touch_Timer = new Timer(0);

    // Profession & Racials
    private Spell Arcane_Torrent = new Spell("Arcane Torrent");
    private Spell Berserking = new Spell("Berserking");
    private Spell Blood_Fury = new Spell("Blood Fury");
    private Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    private Spell Lifeblood = new Spell("Lifeblood");
    private Spell Stoneform = new Spell("Stoneform");

    #endregion InitializeSpell

    public Shadow()
    {
        Main.range = 30.0f;
        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            OutCombatBuff();
            if (!ObjectManager.Me.IsMounted)
            {
                Patrolling();

                if (Fight.InFight && ObjectManager.Me.Target > 0)
                {
                    if (ObjectManager.Me.Target != lastTarget)
                    {
                        lastTarget = ObjectManager.Me.Target;
                    }
                    if (ObjectManager.Target.Level < 70 && ObjectManager.Me.Level > 84)
                    {
                        LowCombat();
                    }
                    else
                    {
                        Combat();
                    }
                }
            }
            Thread.Sleep(400);
        }
    }

    public void OutCombatBuff()
    {
        if (ObjectManager.Me.BarTwoPercentage < 40 && Hymn_of_Hope.KnownSpell && Hymn_of_Hope.IsSpellUsable
            && ObjectManager.Target.IsDead)
        {
            Hymn_of_Hope.Launch();
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(200);
            }
        }

        if (ObjectManager.Me.HealthPercent < 100 && Flash_Heal.KnownSpell && Flash_Heal.IsSpellUsable
            && ObjectManager.Target.IsDead)
        {
            Flash_Heal.Launch();
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(200);
            }
        }
    }

    public void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            Buff();
        }
    }

    public void Buff()
    {
        if (Power_Word_Fortitude.KnownSpell && Power_Word_Fortitude.IsSpellUsable &&
            !Power_Word_Fortitude.HaveBuff)
        {
            Power_Word_Fortitude.Launch();
        }

        if (Inner_Fire.KnownSpell && Inner_Fire.IsSpellUsable && !Inner_Fire.HaveBuff)
        {
            Inner_Fire.Launch();
        }

        if (!Shadowform.HaveBuff && Shadowform.KnownSpell && Shadowform.IsSpellUsable)
        {
            Shadowform.Launch();
        }
    }

    public void LowCombat()
    {
        AvoidMelee();
        Heal();
        Resistance();
        Buff();

        if (Mind_Spike.KnownSpell && Mind_Spike.IsSpellUsable && Mind_Spike.IsDistanceGood)
        {
            Mind_Spike.Launch();
            if (ObjectManager.Target.HealthPercent < 50 && ObjectManager.Target.HealthPercent > 0)
            {
                Mind_Spike.Launch();
                return;
            }
            return;
        }

        if (ObjectManager.Target.HealthPercent > 90)
        {
            if (Mind_Sear.KnownSpell && Mind_Sear.IsSpellUsable && Mind_Sear.IsDistanceGood)
            {
                Mind_Sear.Launch();
                return;
            }
        }
    }

    public void Combat()
    {
        AvoidMelee();
        Decast();
        Heal();
        Resistance();
        Buff();

        if (ObjectManager.Me.BarTwoPercentage < 10 && Arcane_Torrent.KnownSpell && Arcane_Torrent.IsSpellUsable)
        {
            Arcane_Torrent.Launch();
            return;
        }

        if (ObjectManager.GetNumberAttackPlayer() > 4 && Mind_Sear.IsSpellUsable && Mind_Sear.KnownSpell
            && Mind_Sear.IsDistanceGood)
        {
            Mind_Sear.Launch();
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(200);
            }
            return;
        }

        if (Shadow_Word_Pain.KnownSpell && Shadow_Word_Pain.IsSpellUsable && Shadow_Word_Pain.IsDistanceGood &&
            (!Shadow_Word_Pain.TargetHaveBuff || Shadow_Word_Pain_Timer.IsReady))
        {
            Shadow_Word_Pain.Launch();
            Shadow_Word_Pain_Timer = new Timer(1000 * 14);
            //Thread.Sleep(700);
            return;
            //SpellManager.CastSpellByIdLUA(589);
        }

        if (Vampiric_Touch.KnownSpell && Vampiric_Touch.IsSpellUsable && Vampiric_Touch.IsDistanceGood &&
            (!Vampiric_Touch.TargetHaveBuff || Vampiric_Touch_Timer.IsReady))
        {
            Vampiric_Touch.Launch();
            Vampiric_Touch_Timer = new Timer(1000 * 11);
            //Thread.Sleep(700);
            return;
            //SpellManager.CastSpellByIdLUA(34914);
        }

        if (Shadow_Word_Death.IsSpellUsable && Shadow_Word_Death.IsDistanceGood && Shadow_Word_Death.KnownSpell
            && ObjectManager.Target.HealthPercent < 20)
        {
            //SpellManager.CastSpellByIdLUA(32379);
            Shadow_Word_Death.Launch();
            //Thread.Sleep(700);
            return;
        }

        if (Mind_Spike.IsSpellUsable && Mind_Spike.IsDistanceGood && Mind_Spike.KnownSpell &&
            ObjectManager.Me.HaveBuff(87160))
        {
            //SpellManager.CastSpellByIdLUA(32379);
            Mind_Spike.Launch();
            //Thread.Sleep(700);
            return;
        }

        if (Shadowfiend.IsSpellUsable && Shadowfiend.KnownSpell && Shadowfiend.IsDistanceGood)
        {
            Logging.WriteFight("Use Trinket 1.");
            Lua.RunMacroText("/use 13");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Logging.WriteFight("Use Trinket 2.");
            Lua.RunMacroText("/use 14");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");

            if (Berserking.IsSpellUsable && Berserking.KnownSpell)
            {
                Berserking.Launch();
            }

            if (Blood_Fury.IsSpellUsable && Blood_Fury.KnownSpell)
            {
                Blood_Fury.Launch();
            }

            if (Lifeblood.IsSpellUsable && Lifeblood.KnownSpell)
            {
                Lifeblood.Launch();
            }

            Shadowfiend.Launch();
            //Thread.Sleep(700);
            return;
        }

        if (Devouring_Plague.KnownSpell && Devouring_Plague.IsSpellUsable && Devouring_Plague.IsDistanceGood &&
            ObjectManager.Me.ShadowOrbs == 3 && (!Devouring_Plague.TargetHaveBuff || Devouring_Plague_Timer.IsReady))
        {
            Devouring_Plague.Launch();
            Devouring_Plague_Timer = new Timer(1000 * 3);
            //Thread.Sleep(700);
            return;
            //SpellManager.CastSpellByIdLUA(2944);
        }

        if (Mind_Blast.KnownSpell && Mind_Blast.IsSpellUsable && Mind_Blast.IsDistanceGood
            && ObjectManager.Me.ShadowOrbs < 3)
        {
            //SpellManager.CastSpellByIdLUA(8092);
            Mind_Blast.Launch();
            //Thread.Sleep(700);
            return;
        }

        if (!ObjectManager.Me.IsCast && Mind_Flay.IsSpellUsable && Mind_Flay.KnownSpell && Mind_Flay.IsDistanceGood)
        {
            Mind_Flay.Launch();
            //Thread.Sleep(700);
            return;
        }

        /*if (!ObjectManager.Me.IsCast && ObjectManager.Me.Level > 9 )
        {
            Logging.WriteFight("Cast Mind Flay.");
            Lua.RunMacroText("/cast Mind Flay");
            //Thread.Sleep(700);
            return;
        }*/
        else
        {
            if (!ObjectManager.Me.IsCast && Smite.IsSpellUsable && Smite.KnownSpell && Smite.IsDistanceGood)
            {
                Smite.Launch();
                //Thread.Sleep(700);
                return;
            }
        }
        return;
    }

    private void Heal()
    {
        if (ObjectManager.Me.HealthPercent < 80 && Gift_of_the_Naaru.IsSpellUsable && Gift_of_the_Naaru.KnownSpell)
        {
            Gift_of_the_Naaru.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 80 && Vampiric_Embrace.IsSpellUsable && Vampiric_Embrace.KnownSpell)
        {
            Vampiric_Embrace.Launch();
            return;
        }

        if (Renew.KnownSpell && Renew.IsSpellUsable && !Renew.HaveBuff && Renew_Timer.IsReady &&
            ObjectManager.Me.HealthPercent < 75)
        {
            Renew_Timer = new Timer(1000 * 12);
            //SpellManager.CastSpellByIdLUA(139);
            Renew.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 70 && Prayer_of_Mending.KnownSpell && Prayer_of_Mending.IsSpellUsable)
        {
            Prayer_of_Mending.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 65 && Desperate_Prayer.KnownSpell && Desperate_Prayer.IsSpellUsable)
        {
            Desperate_Prayer.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 75 && Power_Word_Shield.KnownSpell && Power_Word_Shield.IsSpellUsable && !Power_Word_Shield.HaveBuff)
        {
            //SpellManager.CastSpellByIdLUA(17);
            Power_Word_Shield.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 55 && Flash_Heal.KnownSpell && Flash_Heal.IsSpellUsable)
        {
            //SpellManager.CastSpellByIdLUA(2061);
            Flash_Heal.Launch();
            Thread.Sleep(1200);
            return;
        }
    }

    private void Resistance()
    {
        if (ObjectManager.Me.HealthPercent < 20 && Dispersion.KnownSpell && Dispersion.IsSpellUsable)
        {
            if (Renew.KnownSpell && Renew.IsSpellUsable)
            {
                Renew_Timer = new Timer(1000 * 12);
                //SpellManager.CastSpellByIdLUA(139);
                Renew.Launch();
            }
            Thread.Sleep(200);
            Dispersion.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 80 && Stoneform.IsSpellUsable && Stoneform.KnownSpell)
        {
            Stoneform.Launch();
            return;
        }
    }

    private void Decast()
    {
        if (ObjectManager.Target.IsCast &&
            ObjectManager.Target.IsTargetingMe &&
            Silence.KnownSpell && Silence.IsSpellUsable && Silence.IsDistanceGood)
        {
            Silence.Launch();
            return;
        }

        if (ObjectManager.Target.IsCast &&
            ObjectManager.Target.IsTargetingMe &&
            Psychic_Horror.KnownSpell && Psychic_Horror.IsSpellUsable && Psychic_Horror.IsDistanceGood)
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
