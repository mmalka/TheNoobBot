using System;
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
    #region Initialize_CustomClass

    internal static float range = 3.5f;
    public static bool loop = true;

    public float Range
    {
        get { return range; }
    }

    public void ShowConfiguration()
    {
        MessageBox.Show("No setting for this custom class.");
    }

    public void Initialize()
    {
        Logging.WriteFight("Loading The Noob Bot CC");

        switch (ObjectManager.Me.WowClass)
        {
            case WoWClass.Priest:

                Spell Mind_Flay = new Spell("Mind Flay");
                Spell Penance = new Spell("Penance");
                Spell Holy_Word_Chastise = new Spell("Holy Word: Chastise");

                if (Mind_Flay.KnownSpell)
                {
                    PriestSha ccPRSH = new PriestSha();
                }

                if (Penance.KnownSpell)
                {
                    PriestDisc ccPRDI = new PriestDisc();
                }

                /*if (Holy_Word_Chastise.KnownSpell)
                {
                    PriestHoly ccPRHO = new PriestHoly();
                }*/

                break;

            default:
                Dispose();
                break;
        }
        Logging.WriteFight("The Noob Bot CC closed.");
    }

    #endregion Initialize_CustomClass

    #region Dispose_CustomClass

    public void Dispose()
    {
        Logging.WriteFight("Closing The Noob Bot CC");
        loop = false;
    }

    #endregion Dispose_CustomClass
}

#region CustomClass

public class PriestDisc
{
    #region InitializeSpell

    Spell Mind_Flay = new Spell("Mind Flay");
    Spell Penance = new Spell("Penance");
    Spell Devouring_Plague = new Spell("Devouring Plague");
    Spell Vampiric_Touch = new Spell("Vampiric Touch");
    Spell Shadow_Word_Pain = new Spell("Shadow Word: Pain");
    Spell Archangel = new Spell("Archangel");
    Spell Mind_Blast = new Spell("Mind Blast");
    Spell Mind_Spike = new Spell("Mind Spike");
    Spell Shadow_Orbs = new Spell("Shadow Orbs");
    Spell Dispersion = new Spell("Dispersion");
    Spell Shadowform = new Spell("Shadowform");
    Spell Empowered_Shadow = new Spell("Empowered Shadow");
    Spell Power_Infusion = new Spell("Power Infusion");
    Spell Inner_Focus = new Spell("Inner Focus");
    Spell Power_Word_Shield = new Spell("Power Word: Shield");
    Spell Holy_Fire = new Spell("Holy Fire");
    Spell Smite = new Spell("Smite");
    Spell Pain_Suppression = new Spell("Pain Suppression");
    Spell Desperate_Prayer = new Spell("Desperate Prayer");
    Spell Shadowfiend = new Spell("Shadowfiend");
    Spell Shadow_Word_Death = new Spell("Shadow Word: Death");

    Spell Flash_Heal = new Spell("Flash Heal");
    Spell Renew = new Spell("Renew");
    Spell Greater_Heal = new Spell("Greater Heal");
    Spell Power_Word_Barrier = new Spell("Power Word: Barrier");

    Spell Power_Word_Fortitude = new Spell("Power Word: Fortitude");
    Spell Shadow_Protection = new Spell("Shadow Protection");
    Spell Inner_Fire = new Spell("Inner Fire");
    Spell Inner_Will = new Spell("Inner Will");
    Spell Fear_Ward = new Spell("Fear Ward");
    Spell Vampiric_Embrace = new Spell("Vampiric Embrace");
    Spell Psychic_Scream = new Spell("Psychic Scream");
    Spell Prayer_of_Mending = new Spell("Prayer of Mending");


    Timer Devouring_Plague_Timer = new Timer(0);
    Timer Vampiric_Touch_Timer = new Timer(0);
    Timer Inner_Fire_Timer = new Timer(0);
    Timer Inner_Will_Timer = new Timer(0);

    #endregion InitializeSpell

    public PriestDisc()
    {
        Main.range = 28.0f; // Range

        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            if (!ObjectManager.Me.IsMounted)
            {
                Patrolling();

                if (Fight.InFight && ObjectManager.Me.Target > 0)
                {
                    if (ObjectManager.Me.Target != lastTarget && ObjectManager.Target.GetDistance <= Main.range)
                    {
                        Pull();
                        lastTarget = ObjectManager.Me.Target;
                    }

                    Combat();
                }
            }
            Thread.Sleep(700);
        }
    }

    public void Pull()
    {

    }

    public void Combat()
    {
        Fear();
        AvoidMelee();
        Heal();
        Mana();
        BuffCombat();

        if (Archangel.KnownSpell && Archangel.IsSpellUsable)
        {
            Archangel.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent > 50 &&
            ObjectManager.Target.HealthPercent < 25 &&
            Shadow_Word_Death.KnownSpell && Shadow_Word_Death.IsDistanceGood && Shadow_Word_Death.IsSpellUsable)
        {
            Shadow_Word_Death.Launch();
            return;
        }

        if (Penance.KnownSpell && Penance.IsDistanceGood && Penance.IsSpellUsable)
        {
            if (ObjectManager.Me.HealthPercent < 70)
            {
                Lua.RunMacroText("/target Player");
                Penance.Launch();
                return;
            }

            else
            {
                Penance.Launch();
                return;
            }
        }

        if (!Power_Word_Shield.HaveBuff && Power_Word_Shield.KnownSpell && Power_Word_Shield.IsSpellUsable)
        {
            Power_Word_Shield.Launch();
            return;
        }

        if (Holy_Fire.KnownSpell && Holy_Fire.IsDistanceGood && Holy_Fire.IsSpellUsable)
        {
            if (Mind_Spike.TargetHaveBuff && Mind_Blast.KnownSpell && Mind_Blast.IsDistanceGood && Mind_Blast.IsSpellUsable)
            {
                Lua.RunMacroText("/use 13");
                Lua.RunMacroText("/script UIErrorsFrame:Clear()");
                Lua.RunMacroText("/use 14");
                Lua.RunMacroText("/script UIErrorsFrame:Clear()");
                Mind_Blast.Launch();
                return;
            }

            else if (Mind_Spike.KnownSpell && Mind_Spike.IsDistanceGood && Mind_Spike.IsSpellUsable)
            {
                Mind_Spike.Launch();
                return;
            }

            else
            {
                Lua.RunMacroText("/use 13");
                Lua.RunMacroText("/script UIErrorsFrame:Clear()");
                Lua.RunMacroText("/use 14");
                Lua.RunMacroText("/script UIErrorsFrame:Clear()");
                Holy_Fire.Launch();
                return;
            }
        }
    }

    public void Patrolling()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        Buff();
        Heal();
    }

    private void Fear()
    {
        if (ObjectManager.Target.GetDistance < 8 &&
            Psychic_Scream.KnownSpell && Psychic_Scream.IsSpellUsable)
        {
            Psychic_Scream.Launch();
            return;
        }
    }

    private void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (!Power_Word_Fortitude.HaveBuff && Power_Word_Fortitude.KnownSpell && Power_Word_Fortitude.IsSpellUsable)
        {
            Power_Word_Fortitude.Launch();
            Thread.Sleep(1000);
            return;
        }

        if ((!Shadow_Protection.HaveBuff || ObjectManager.Me.HaveBuff(27683))
            && Shadow_Protection.KnownSpell && Shadow_Protection.IsSpellUsable)
        {
            Shadow_Protection.Launch();
            Thread.Sleep(1000);
            return;
        }

        if (!Fear_Ward.HaveBuff && Fear_Ward.KnownSpell && Fear_Ward.IsSpellUsable)
        {
            Fear_Ward.Launch();
            Thread.Sleep(1000);
            return;
        }

        if (Inner_Fire_Timer.IsReady && Inner_Fire.KnownSpell && Inner_Fire.IsSpellUsable)
        {
            Inner_Fire.Launch();
            Inner_Fire_Timer = new Timer(1000 * 60 * 30);
            Thread.Sleep(1000);
            return;
        }
    }

    private void BuffCombat()
    {
        if (Inner_Focus.KnownSpell && Inner_Focus.IsSpellUsable)
        {
            Inner_Focus.Launch();
            return;
        }

        if (!Fear_Ward.HaveBuff && Fear_Ward.KnownSpell && Fear_Ward.IsSpellUsable)
        {
            Fear_Ward.Launch();
            return;
        }

        if (Power_Infusion.KnownSpell && Power_Infusion.IsSpellUsable)
        {
            Power_Infusion.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 75 &&
            Pain_Suppression.KnownSpell && Pain_Suppression.IsSpellUsable)
        {
            Pain_Suppression.Launch();
            return;
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (ObjectManager.Me.HealthPercent < 75 &&
            ObjectManager.Me.BarTwoPercentage < 75 && Inner_Focus.HaveBuff &&
            Greater_Heal.KnownSpell && Greater_Heal.IsSpellUsable)
        {
            Greater_Heal.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 85 &&
            !Prayer_of_Mending.HaveBuff && Prayer_of_Mending.KnownSpell && Prayer_of_Mending.IsSpellUsable)
        {
            Prayer_of_Mending.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 85 &&
            !Renew.HaveBuff && Renew.KnownSpell && Renew.IsSpellUsable)
        {
            Renew.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 40 &&
            Flash_Heal.KnownSpell && Flash_Heal.IsDistanceGood && Flash_Heal.IsSpellUsable)
        {
            if (Inner_Focus.KnownSpell && Inner_Focus.IsSpellUsable)
            {
                Inner_Focus.Launch();
            }

            if (Power_Word_Barrier.KnownSpell && Power_Word_Barrier.IsSpellUsable)
            {
                SpellManager.CastSpellByIDAndPosition(62618, ObjectManager.Me.Position);
                Logging.WriteFight("cast Power Word: Barrier");
            }

            Flash_Heal.Launch();
        }

        if (ObjectManager.Me.HealthPercent < 75 &&
            Power_Word_Barrier.KnownSpell && Power_Word_Barrier.IsSpellUsable)
        {
            SpellManager.CastSpellByIDAndPosition(62618, ObjectManager.Me.Position);
            Logging.WriteFight("cast Power Word: Barrier");
        }
    }

    private void Mana()
    {
        if (ObjectManager.Me.BarTwoPercentage < 75 &&
            ObjectManager.Target.HealthPercent > 55 &&
            Shadowfiend.IsDistanceGood && Shadowfiend.KnownSpell && Shadowfiend.IsSpellUsable)
        {
            Shadowfiend.Launch();
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

public class PriestSha
{
    #region InitializeSpell

    Spell Mind_Flay = new Spell("Mind Flay");
    Spell Devouring_Plague = new Spell("Devouring Plague");
    Spell Vampiric_Touch = new Spell("Vampiric Touch");
    Spell Shadow_Word_Pain = new Spell("Shadow Word: Pain");
    Spell Archangel = new Spell("Archangel");
    Spell Mind_Blast = new Spell("Mind Blast");
    Spell Shadow_Orbs = new Spell("Shadow Orbs");
    Spell Dispersion = new Spell("Dispersion");
    Spell Shadowform = new Spell("Shadowform");
    Spell Empowered_Shadow = new Spell("Empowered Shadow");
    Spell Shadowfiend = new Spell("Shadowfiend");
    Spell Greater_Heal = new Spell("Greater Heal");
    Spell Power_Word_Shield = new Spell("Power Word: Shield");
    Spell Shadow_Word_Death = new Spell("Shadow Word: Death");
    Spell Psychic_Horror = new Spell("Psychic Horror");
    Spell Silence = new Spell("Silence");
    Spell Inner_Focus = new Spell("Inner Focus");
    Spell Hymn_of_Hope = new Spell("Hymn of Hope");

    Spell Flash_Heal = new Spell("Flash Heal");
    Spell Renew = new Spell("Renew");

    Spell Power_Word_Fortitude = new Spell("Power Word: Fortitude");
    Spell Shadow_Protection = new Spell("Shadow Protection");
    Spell Inner_Fire = new Spell("Inner Fire");
    Spell Fear_Ward = new Spell("Fear Ward");
    Spell Vampiric_Embrace = new Spell("Vampiric Embrace");
    Spell Psychic_Scream = new Spell("Psychic Scream");

    Timer Devouring_Plague_Timer = new Timer(0);
    Timer Vampiric_Touch_Timer = new Timer(0);

    #endregion InitializeSpell

    public PriestSha()
    {
        Main.range = 28.0f; // Range

        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            if (!ObjectManager.Me.IsMounted)
            {
                Patrolling();

                if (Fight.InFight && ObjectManager.Me.Target > 0)
                {
                    if (ObjectManager.Me.Target != lastTarget && Mind_Flay.IsDistanceGood)
                    {
                        Pull();
                        lastTarget = ObjectManager.Me.Target;
                    }

                    Combat();
                }
            }
            Thread.Sleep(700);
        }
    }

    public void Pull()
    {

    }

    public void Patrolling()
    {
        if (!Fight.InFight)
        {
            Heal();
        }

        Buff();
        Form();
    }

    public void Combat()
    {
        Mana();
        Fear();
        BuffCombat();
        AvoidMelee();
        Decast();

        if (!Shadowform.HaveBuff && Shadowform.KnownSpell && Shadowform.IsSpellUsable)
        {
            Shadowform.Launch();
            return;
        }

        if (!Power_Word_Shield.HaveBuff && Power_Word_Shield.KnownSpell && Power_Word_Shield.IsSpellUsable)
        {
            Power_Word_Shield.Launch();
            return;
        }

        if (!Vampiric_Touch.TargetHaveBuff && Vampiric_Touch.KnownSpell && Vampiric_Touch.IsDistanceGood && Vampiric_Touch.IsSpellUsable)
        {
            Vampiric_Touch.Launch();
            return;
        }

        if (!Devouring_Plague.TargetHaveBuff && Devouring_Plague.KnownSpell && Devouring_Plague.IsDistanceGood && Devouring_Plague.IsSpellUsable)
        {
            Devouring_Plague.Launch();
            return;
        }

        if (!Shadow_Word_Pain.TargetHaveBuff && Shadow_Word_Pain.KnownSpell && Shadow_Word_Pain.IsDistanceGood && Shadow_Word_Pain.IsSpellUsable)
        {
            Shadow_Word_Pain.Launch();
            return;
        }

        if (ObjectManager.Target.HealthPercent < 25 &&
            Shadow_Word_Death.KnownSpell && Shadow_Word_Death.IsDistanceGood && Shadow_Word_Death.IsSpellUsable)
        {
            Shadow_Word_Death.Launch();
            return;
        }

        if (!Empowered_Shadow.HaveBuff &&
            Mind_Blast.KnownSpell && Mind_Blast.IsDistanceGood && Mind_Blast.IsSpellUsable)
        {
            Lua.RunMacroText("/use 13");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Lua.RunMacroText("/use 14");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Mind_Blast.Launch();
            return;
        }

        if (Mind_Flay.KnownSpell && Mind_Flay.IsDistanceGood && Mind_Flay.IsSpellUsable)
        {
            Mind_Flay.Launch();
            return;
        }

        if (Archangel.KnownSpell && Archangel.IsSpellUsable)
        {
            Archangel.Launch();
            return;
        }
    }

    private void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (Inner_Focus.KnownSpell && Inner_Focus.IsSpellUsable)
        {
            Inner_Focus.Launch();
            return;
        }

        if (!Power_Word_Fortitude.HaveBuff && Power_Word_Fortitude.KnownSpell && Power_Word_Fortitude.IsSpellUsable)
        {
            Power_Word_Fortitude.Launch();
            return;
        }

        if ((!Shadow_Protection.HaveBuff || ObjectManager.Me.HaveBuff(27683))
            && Shadow_Protection.KnownSpell && Shadow_Protection.IsSpellUsable)
        {
            Shadow_Protection.Launch();
            return;
        }

        if (!Vampiric_Embrace.HaveBuff && Vampiric_Embrace.KnownSpell && Vampiric_Embrace.IsSpellUsable)
        {
            Vampiric_Embrace.Launch();
            return;
        }

        if (!Fear_Ward.HaveBuff && Fear_Ward.KnownSpell && Fear_Ward.IsSpellUsable)
        {
            Fear_Ward.Launch();
            return;
        }

        if (!Inner_Fire.HaveBuff && Inner_Fire.KnownSpell && Inner_Fire.IsSpellUsable)
        {
            Inner_Fire.Launch();
            return;
        }
    }

    private void Fear()
    {
        if ((!Psychic_Scream.TargetHaveBuff || !Psychic_Horror.TargetHaveBuff) &&
            ObjectManager.Target.GetDistance < 8 &&
            Psychic_Scream.KnownSpell && Psychic_Scream.IsSpellUsable)
        {
            Psychic_Scream.Launch();
            return;
        }

        if ((!Psychic_Scream.TargetHaveBuff || !Psychic_Horror.TargetHaveBuff) &&
            Psychic_Horror.IsDistanceGood && Psychic_Horror.KnownSpell && Psychic_Horror.IsSpellUsable)
        {
            Psychic_Horror.Launch();
            return;
        }
    }

    private void BuffCombat()
    {
        if (!Fear_Ward.HaveBuff && Fear_Ward.KnownSpell && Fear_Ward.IsSpellUsable)
        {
            Fear_Ward.Launch();
            return;
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (ObjectManager.Me.HealthPercent < 75 &&
            !Renew.HaveBuff && Renew.KnownSpell && Renew.IsSpellUsable)
        {
            Renew.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 65 &&
            Greater_Heal.KnownSpell && Greater_Heal.IsSpellUsable)
        {
            Greater_Heal.Launch();
            return;
        }
    }

    private void Form()
    {
        if (ObjectManager.Me.HealthPercent >= 75 &&
            !Shadowform.HaveBuff && Shadowform.KnownSpell && Shadowform.IsSpellUsable)
        {
            Shadowform.Launch();
            return;
        }
    }

    private void Mana()
    {
        if (ObjectManager.Me.BarTwoPercentage < 50 &&
            Dispersion.KnownSpell && Dispersion.IsSpellUsable)
        {
            Dispersion.Launch();
        }

        if (ObjectManager.Me.BarTwoPercentage < 75 &&
            ObjectManager.Target.HealthPercent > 50 &&
            Shadowfiend.IsDistanceGood && Shadowfiend.KnownSpell && Shadowfiend.IsSpellUsable)
        {
            Shadowfiend.Launch();
        }
    }

    private void Decast()
    {
        if (ObjectManager.Target.IsCast &&
            Silence.KnownSpell && Silence.IsSpellUsable && Silence.IsDistanceGood)
        {
            Silence.Launch();
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

#endregion CustomClass
