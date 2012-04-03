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

    public void Initialize() { HunterMark ccROA = new HunterMark(); }

    #endregion Initialize_CustomClass

    #region Dispose_CustomClass
    public void Dispose() { Logging.WriteFight("Closing The Noob Bot CC"); loop = false; }
    #endregion Dispose_CustomClass
}

#region CustomClass

public class HunterMark
{
    #region InitializeSpell

    //avoid
    Spell Hunter_s_Mark = new Spell("Hunter's Mark");
    Spell Misdirection = new Spell("Misdirection");
    Spell Feign_Death = new Spell("Feign Death");
    Spell Freezing_Trap = new Spell("Freezing Trap");
    Spell Snake_Trap = new Spell("Snake Trap");
    Spell Wing_Clip = new Spell("Wing Clip");
    Spell Disengage = new Spell("Disengage");
    Spell Concussive_Shot = new Spell("Concussive Shot");

    //kill					    
    Spell Serpent_Sting = new Spell("Serpent Sting");
    Spell Arcane_Shot = new Spell("Arcane Shot");
    Spell Chimera_Shot = new Spell("Chimera Shot");
    Spell Aimed_Shot = new Spell("Aimed Shot");
    Spell Steady_Shot = new Spell("Steady Shot");
    Spell Cobra_Shot = new Spell("Cobra Shot");
    Spell Kill_Shot = new Spell("Kill Shot");
    //BestialWrath
    Spell Bestial_Wrath = new Spell("Bestial Wrath");
    Spell Rapid_Fire = new Spell("Rapid Fire");
    Spell Intimidation = new Spell("Intimidation");

    Spell Focus_Fire = new Spell("Focus Fire");
    Spell Fervor = new Spell("Fervor");
    Spell Widow_Venom = new Spell("Widow Venom");
    Spell Deterrence = new Spell("Deterrence");
    Spell Camouflage = new Spell("Camouflage");
    Spell Stoneform = new Spell("Stoneform");
    Spell Silencing_Shot = new Spell("Silencing Shot");
    Spell Readiness = new Spell("Readiness");


    //moarkill
    Spell Raptor_Strike = new Spell("Raptor Strike");
    Spell Kill_Command = new Spell("Kill Command");

    Spell Call_Pet = new Spell("Call Pet 1");
    Spell Revive_Pet = new Spell("Revive Pet");
    Spell Mend_Pet = new Spell("Mend Pet");
    Spell Aspect_of_the_Fox = new Spell("Aspect of the Fox");
    Spell Aspect_of_the_Hawk = new Spell("Aspect of the Hawk");
    Spell Scatter_Shot = new Spell("Scatter Shot");

    Timer flee = new Timer(0);
    Timer pethealcd = new Timer(0);

    Spell Darkflight = new Spell("Darkflight");



    #endregion InitializeSpell

    public HunterMark()
    {
        Main.range = 40.0f; // Range

        UInt64 lastTarget = 0;

        while (Main.loop)
        {

            if (!ObjectManager.Me.IsMounted)
            {
                if (Darkflight.KnownSpell && Darkflight.IsSpellUsable)
                {
                    Darkflight.Launch();
                }

                Patrolling();

                if (Fight.InFight && ObjectManager.Me.Target > 0)
                {

                    if (ObjectManager.Me.Target != lastTarget && ObjectManager.Target.GetDistance <= Main.range)
                    {
                        if (Concussive_Shot.KnownSpell && Concussive_Shot.IsSpellUsable && Concussive_Shot.IsDistanceGood)
                        {
                            Concussive_Shot.Launch();
                        }
                        Pull();

                        lastTarget = ObjectManager.Me.Target;
                    }

                    Combat();
                }
            }
            Thread.Sleep(20);
        }
    }

    public void Pull()
    {
        Lua.RunMacroText("/petattack");

        if (!Hunter_s_Mark.HaveBuff && Hunter_s_Mark.KnownSpell)
        {
            Hunter_s_Mark.Launch();
        }

        Pet();
        Buff();
    }

    public void Combat()
    {
        AvoidMelee();
        Heal();
        Buff();
        if (ObjectManager.Me.HaveBuff(82897))
        {
            Focus_Fire.Launch();
        }
        if (Bestial_Wrath.IsSpellUsable && Bestial_Wrath.IsDistanceGood)
            Bestial_Wrath.Launch();
        {
        }




        //----------------------------------------------------
        // Silencing Shot
        //----------------------------------------------------

        if (!Silencing_Shot.HaveBuff &&
            Silencing_Shot.KnownSpell)
        {
            Silencing_Shot.Launch();
        }
        //----------------------------------------------------
        // Readiness
        //----------------------------------------------------

        if (!Readiness.HaveBuff &&
            Readiness.KnownSpell)
        {
            Readiness.Launch();
        }
        //----------------------------------------------------
        //----------------------------------------------------
        // Stoneform
        //----------------------------------------------------

        if (!Stoneform.HaveBuff &&
            Stoneform.KnownSpell)
        {
            Stoneform.Launch();
        }
        //----------------------------------------------------
        // Camouflage
        //----------------------------------------------------

        if (!Camouflage.HaveBuff &&
            Camouflage.KnownSpell)
        {
            Camouflage.Launch();
        }
        //----------------------------------------------------
        //----------------------------------------------------
        // Rapid_Fire
        //----------------------------------------------------

        if (!Rapid_Fire.HaveBuff &&
            Rapid_Fire.KnownSpell)
        {
            Rapid_Fire.Launch();
        }
        //----------------------------------------------------
        // Focus_Fire
        //----------------------------------------------------

        if (!Focus_Fire.HaveBuff &&
            Focus_Fire.KnownSpell)
        {
            Focus_Fire.Launch();
        }
        //----------------------------------------------------
        // Fervor
        //----------------------------------------------------

        if (!Fervor.HaveBuff &&
            Fervor.KnownSpell)
        {
            Fervor.Launch();
        }
        //----------------------------------------------------
        // Intimidation
        //----------------------------------------------------

        if (!Intimidation.HaveBuff &&
            Intimidation.KnownSpell)
        {
            Intimidation.Launch();
        }
        //----------------------------------------------------
        // Widow_Venom
        //----------------------------------------------------

        if (!Widow_Venom.HaveBuff &&
            Widow_Venom.KnownSpell)
        {
            Widow_Venom.Launch();
        }
        //----------------------------------------------------
        // Freezing_Trap
        //----------------------------------------------------

        if (!Freezing_Trap.HaveBuff &&
            Freezing_Trap.KnownSpell)
        {
            Freezing_Trap.Launch();
        }
        //----------------------------------------------------
        // Snake_Trap
        //----------------------------------------------------

        if (!Snake_Trap.HaveBuff &&
            Snake_Trap.KnownSpell)
        {
            Snake_Trap.Launch();
        }

        //----------------------------------------------------------------------
        if (Misdirection.IsSpellUsable && Misdirection.IsDistanceGood)
        {
            Lua.RunMacroText("/cast [@pet,exists] kasusky");
        }
        if (ObjectManager.Me.HaveBuff(82926) && Aimed_Shot.IsSpellUsable && Aimed_Shot.IsDistanceGood)
        {
            Aimed_Shot.Launch();
        }
        if (ObjectManager.Me.HaveBuff(82897))
        {
            Kill_Command.Launch();
        }
        if (Kill_Shot.KnownSpell && Kill_Shot.IsSpellUsable && Kill_Shot.IsDistanceGood)
        {
            Kill_Shot.Launch();
        }
        if (!ObjectManager.Target.HaveBuff(1978))
        {
            Serpent_Sting.Launch();
        }
        if (Chimera_Shot.KnownSpell && Chimera_Shot.IsSpellUsable && Chimera_Shot.IsDistanceGood)
        {
            Chimera_Shot.Launch();
        }
        if (Aimed_Shot.IsSpellUsable && Aimed_Shot.IsDistanceGood && ObjectManager.Target.HealthPercent > 40)
        {
            Aimed_Shot.Launch();
        }
        if (Kill_Command.KnownSpell && Kill_Command.IsSpellUsable && Kill_Command.IsDistanceGood)
        {
            Kill_Command.Launch();
        }
        if (Raptor_Strike.KnownSpell && Raptor_Strike.IsSpellUsable && Raptor_Strike.IsDistanceGood)
        {
            Raptor_Strike.Launch();
        }
        if (Steady_Shot.KnownSpell && Steady_Shot.IsSpellUsable && Steady_Shot.IsDistanceGood)
        {
            Steady_Shot.Launch();
        }
    }

    public void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            Buff();
            Heal();

        }
    }

    private void Pet()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if ((ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0) &&
            !ObjectManager.Me.IsMounted && !ObjectManager.Me.IsDeadMe)
        {
            Call_Pet.Launch();
            Thread.Sleep(1000);

            if (!ObjectManager.Pet.IsAlive)
            {
                Revive_Pet.Launch();
                Thread.Sleep(1000);
            }
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.HealthPercent < 50 &&
            ObjectManager.Pet.Health > 0 &&
            Scatter_Shot.KnownSpell &&
            Scatter_Shot.IsSpellUsable &&
            Scatter_Shot.IsDistanceGood)
        {
            Scatter_Shot.Launch();
            // Verband
            // Thread.Sleep (6000);
        }

        if (ObjectManager.Me.HealthPercent < 30 &&
            ObjectManager.Pet.Health > 0 &&
            Scatter_Shot.KnownSpell &&
            Scatter_Shot.IsSpellUsable &&
            Scatter_Shot.IsDistanceGood)
        {
            Deterrence.Launch();
            // Verband
            // Thread.Sleep (2000);
        }
        if (ObjectManager.Me.HealthPercent < 50 &&
            !ObjectManager.Pet.IsAlive &&
            Feign_Death.KnownSpell &&
            Feign_Death.IsSpellUsable)
        {
            Feign_Death.Launch();
            Thread.Sleep(6000);
        }

        if (ObjectManager.Pet.Health > 0 &&
                        ObjectManager.Pet.HealthPercent < 66 &&
            Mend_Pet.KnownSpell &&
            Mend_Pet.IsSpellUsable &&
            pethealcd.IsReady)
        {
            pethealcd = new Timer(1000 * 8);
            Mend_Pet.Launch();
        }
    }

    private void Buff()
    {
        if (!Aspect_of_the_Hawk.HaveBuff)
        {
            Aspect_of_the_Hawk.Launch();
        }
    }

    private void AvoidMelee()
    {
        if (ObjectManager.Target.GetDistance < 5 && Disengage.KnownSpell && Disengage.IsSpellUsable && Disengage.IsDistanceGood && ObjectManager.Target.HealthPercent > 25)
        {
            if (Wing_Clip.KnownSpell && Wing_Clip.IsSpellUsable && Wing_Clip.IsDistanceGood)
            {
                Wing_Clip.Launch();
            }
            Disengage.Launch();
        }

        if (ObjectManager.Target.GetDistance < 15)
        {
            flee = new Timer(1000 * 6);
            while (ObjectManager.Target.GetDistance < 15 && !flee.IsReady)
            {
                nManager.Wow.Helpers.Keybindings.DownKeybindings(Keybindings.MOVEBACKWARD);
                Thread.Sleep(100);

                if (Wing_Clip.KnownSpell && Wing_Clip.IsSpellUsable && Wing_Clip.IsDistanceGood)
                {
                    Wing_Clip.Launch();
                }
                if (Kill_Command.KnownSpell && Kill_Command.IsSpellUsable && Kill_Command.IsDistanceGood)
                {
                    Kill_Command.Launch();
                    //Keyboard.DownKey(nManager.Wow.Memory.WowProcess.MainWindowHandle, "{SPACE}");
                }
                if (Arcane_Shot.KnownSpell && Arcane_Shot.IsSpellUsable && Arcane_Shot.IsDistanceGood)
                {
                    Arcane_Shot.Launch();
                    Keyboard.DownKey(nManager.Wow.Memory.WowProcess.MainWindowHandle, "{SPACE}");
                }
                if (ObjectManager.Me.HealthPercent < 60 && Feign_Death.KnownSpell && Feign_Death.IsSpellUsable)
                {
                    Feign_Death.Launch();
                    Thread.Sleep(6000);
                }
                //if (case worldtimer=flee10timesinarow)
                // {break;}
            }
            nManager.Wow.Helpers.Keybindings.UpKeybindings(Keybindings.MOVEBACKWARD);
            return;
        }
    }

}
#endregion CustomClass			