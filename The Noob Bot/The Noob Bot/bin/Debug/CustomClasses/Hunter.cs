using System;
using System.Threading;
using System.Windows.Forms;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;

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
            case WoWClass.Hunter:
                Logging.WriteFight("Hunter CC selected.");
                Hunter ccHU = new Hunter();
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

public class Hunter
{
    #region InitializeSpell

    Spell Hunter_s_Mark = new Spell("Hunter's Mark");
    Spell Serpent_Sting = new Spell("Serpent Sting");
    Spell Arcane_Shot = new Spell("Arcane Shot");
    Spell Raptor_Strike = new Spell("Raptor Strike");
    Spell Aimed_Shot = new Spell("Aimed Shot");
    Spell Steady_Shot = new Spell("Steady Shot");
    Spell Call_Pet = new Spell("Call Pet 1");
    Spell Revive_Pet = new Spell("Revive Pet");
    Spell Aspect_of_the_Viper = new Spell("Aspect of the Viper");
    Spell Aspect_of_the_Hawk = new Spell("Aspect of the Hawk");
    Spell Mend_Pet = new Spell("Mend Pet");

    #endregion InitializeSpell

    public Hunter()
    {
        Main.range = 23.0f; // Range

        UInt64 lastTarget = 0;
        bool postCombatUsed = true;
        bool HaveBeenPooled = false;

        while (Main.loop)
        {
            if (!ObjectManager.Me.IsMounted)
            {
                Patrolling();
                if (!Fight.InFight)
                    HaveBeenPooled = false;

                if (!Fight.InFight && !postCombatUsed)
                {
                    postCombatUsed = true;
                    PostCombat();
                }


                if (Fight.InFight && ObjectManager.Me.Target > 0)
                {
                    if (ObjectManager.Me.Target != lastTarget && ObjectManager.Target.GetDistance <= Main.range)
                    {
                        Pull();
                        lastTarget = ObjectManager.Me.Target;
                        HaveBeenPooled = true;
                    }
                    if (HaveBeenPooled == true)
                        Combat();
                    postCombatUsed = false;
                }
            }
            Thread.Sleep(20);
        }
    }

    public void Pull()
    {

        //----------------------------------------------------
        // Summon Pet
        //----------------------------------------------------

        SummonPet();

        //----------------------------------------------------
        // Pet Pet Defensive
        //----------------------------------------------------

        if (ObjectManager.Pet.IsAlive)
        {
            Lua.LuaDoString("PetDefensiveMode();");
        }

        //----------------------------------------------------
        // Pet Attack
        //----------------------------------------------------

        if (ObjectManager.Pet.IsAlive)
        {
            Lua.RunMacroText("/petattack");
            Logging.WriteFight("Launch Pet Attack");
        }

        //----------------------------------------------------
        // Hunter's Mark
        //----------------------------------------------------

        if (!Hunter_s_Mark.HaveBuff &&
            Hunter_s_Mark.KnownSpell)
        {
            Hunter_s_Mark.Launch();
        }
        Lua.RunMacroText("/petattack");
        //----------------------------------------------------
        // Serpent Sting
        //----------------------------------------------------
        Serpent_Sting.Launch();
        if (
            Serpent_Sting.KnownSpell &&
            Serpent_Sting.IsSpellUsable &&
            Serpent_Sting.IsDistanceGood)
        {
            Serpent_Sting.Launch();
        }

        //----------------------------------------------------
        // Arcane Shot
        //----------------------------------------------------

        /*if (Arcane_Shot.KnownSpell &&
            Arcane_Shot.IsSpellUsable &&
            Arcane_Shot.IsDistanceGood)
        {
            Arcane_Shot.Launch();
        }*/
    }

    public void Combat()
    {


        SummonPet();
        HealPet();
        AvoidMelee();


        //----------------------------------------------------
        // Arcane Shot
        //----------------------------------------------------

        if (Arcane_Shot.KnownSpell &&
            Arcane_Shot.IsSpellUsable &&
            Arcane_Shot.IsDistanceGood)
        {
            Arcane_Shot.Launch();
            HealPet();
            AvoidMelee();
        }

        //----------------------------------------------------
        // Raptor Strike
        //----------------------------------------------------

        if (Raptor_Strike.KnownSpell &&
            Raptor_Strike.IsSpellUsable &&
            Raptor_Strike.IsDistanceGood)
        {
            Raptor_Strike.Launch();
            HealPet();
            AvoidMelee();
        }

        //----------------------------------------------------
        // Aimed Shot
        //----------------------------------------------------

        if (Aimed_Shot.KnownSpell &&
            Aimed_Shot.IsSpellUsable &&
            Aimed_Shot.IsDistanceGood)
        {
            Aimed_Shot.Launch();
            HealPet();
            AvoidMelee();
        }

        //----------------------------------------------------
        // Steady Shot
        //----------------------------------------------------

        if (Steady_Shot.KnownSpell &&
           Steady_Shot.IsSpellUsable &&
            Steady_Shot.IsDistanceGood)
        {
            Steady_Shot.Launch();
            HealPet();
            AvoidMelee();
        }

    }

    public void PostCombat()
    {

    }

    public void Patrolling()
    {

        if (!ObjectManager.Me.IsMounted)
        {

            //----------------------------------------------------
            // Summon Pet
            //----------------------------------------------------

            //SummonPet();

            //----------------------------------------------------
            // Check Aura
            //----------------------------------------------------

            CheckAura();

        }

    }

    ////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////
    ///////////					Private Function				  //////////
    ////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////	

    private void SummonPet()
    {
        if (!ObjectManager.Me.IsCast)
        {
            if (Call_Pet.KnownSpell &&
                Revive_Pet.KnownSpell &&
                !ObjectManager.Me.IsMounted)
            {

                if (!ObjectManager.Pet.IsAlive ||
                    ObjectManager.Pet.Guid == 0)
                {
                    Thread.Sleep(1000);
                    if (!ObjectManager.Me.IsCast && !ObjectManager.Me.IsMounted)
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

            }
        }
    }

    private void CheckAura()
    {

        if (Aspect_of_the_Viper.KnownSpell &&
            Aspect_of_the_Hawk.KnownSpell)
        {

            if (ObjectManager.Me.BarTwoPercentage >= 80 && !Aspect_of_the_Hawk.HaveBuff)
            {
                Aspect_of_the_Hawk.Launch();
            }
            else if (ObjectManager.Me.BarTwoPercentage <= 25 && !Aspect_of_the_Viper.HaveBuff)
            {
                Aspect_of_the_Viper.Launch();
            }
            else if (!Aspect_of_the_Viper.HaveBuff &&
                    !Aspect_of_the_Hawk.HaveBuff)
            {
                Aspect_of_the_Hawk.Launch();
            }

        }

    }

    private Int32 LastHealTick = 0;
    private void HealPet()
    {

        if (ObjectManager.Pet.Health > 0 &&
            ObjectManager.Pet.HealthPercent < 50 &&
            (Environment.TickCount - LastHealTick) > (15 * 1000))
        {
            LastHealTick = Environment.TickCount;
            Mend_Pet.Launch();
        }

    }

    private void AvoidMelee()
    {
        if (ObjectManager.Target.GetDistance < 5 &&
            ObjectManager.Pet.IsAlive &&
            ObjectManager.Pet.Guid != 0 &&
            ObjectManager.Target.Target == ObjectManager.Pet.Guid)
        {
            Keyboard.DownKey(nManager.Wow.Memory.WowProcess.MainWindowHandle, "{DOWN}");
            Thread.Sleep(1500);
            Keyboard.UpKey(nManager.Wow.Memory.WowProcess.MainWindowHandle, "{DOWN}");
        }
    }

}

#endregion CustomClass
