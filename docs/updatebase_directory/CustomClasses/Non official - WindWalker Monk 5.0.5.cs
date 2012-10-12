/*
* CustomClass for TheNoobBot
* Credit :  Marcellolo
* Thanks you !
*/
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
        Logging.WriteFight("Loading combat system.");

        switch (ObjectManager.Me.WowClass)
        {
            case WoWClass.Monk:

                Spell Jab = new Spell("Jab");
 
                if (Jab.KnownSpell)
                {
                    MonkWW ccMWW = new MonkWW();
                }

                                break;

            default:
                Dispose();
                break;
        }
        Logging.WriteFight("WoWRoboT CC closed.");
    }

    #endregion Initialize_CustomClass

    #region Dispose_CustomClass

    public void Dispose()
    {
        Logging.WriteFight("Closing WoW-RoboT CC");
        loop = false;
    }

    #endregion Dispose_CustomClass
}

#region CustomClass

public class MonkWW
{
    #region InitializeSpell


    Spell Stance_of_the_Fierce_Tiger = new Spell("Stance of the Fierce Tiger");

    Spell Jab = new Spell("Jab");
    Spell Tiger_Palm = new Spell("Tiger Palm");
    Spell Roll = new Spell("Roll");
    Spell Blackout_Kick = new Spell("Blackout Kick");
    Spell Provoke = new Spell("Provoke");
    Spell Detox = new Spell("Detox");
    Spell Legacy_of_the_Emperor = new Spell("Legacy of the Emperor");
    Spell Touch_Of_Death = new Spell("Touch Of Death");
    Spell Fortifying_Brew = new Spell("Fortifying Brew");
    Spell Expel_Harm = new Spell("Expel Harm");
    Spell Disable = new Spell("Disable");
    Spell Spear_Hand_Strike = new Spell("Spear Hand Strike");
    Spell Paralysis = new Spell("Paralysis");
    Spell Spinning_Crane_Kick = new Spell("Spinning Crane Kick");
    Spell Crackling_Jade_Lightning = new Spell("Crackling Jade Lightning");
    Spell Path_of_Blossoms = new Spell("Path of Blossoms");
    Spell Grapple_Weapon = new Spell("Grapple Weapon");
    Spell Zen_Meditation = new Spell("Zen Meditation");

    Spell Fists_of_Fury = new Spell("Fists of Fury");
    Spell Flying_Serpent_Kick = new Spell("Flying Serpent Kick");
    Spell Touch_of_Karma = new Spell("Touch of Karma");
    Spell Energizing_Brew = new Spell("Energizing Brew");
    Spell Spinning_Fire_Blossom = new Spell("Spinning Fire Blossom");
    Spell Rising_Sun_Kick = new Spell("Rising Sun Kick");
    Spell Tigereye_Brew = new Spell("Tigereye Brew");
    Spell Legacy_of_the_White_Tiger = new Spell("Legacy of the White Tiger");

    Spell Tigers_Lust = new Spell("Tiger's Lust");
    Spell Momentum = new Spell("Momentum");

    Spell Chi_Wave = new Spell("Chi Wave");
    Spell Zen_Sphere = new Spell("Zen Sphere");
    Spell Chi_Burst = new Spell("Chi Burst");

    Spell Chi_Brew = new Spell("Chi Brew");

    Spell Charging_Ox_Wave = new Spell("Charging Ox Wave");
    Spell Leg_Sweep = new Spell("Leg Sweep");

    Spell Dampen_Harm = new Spell("Dampen Harm");
    Spell Diffuse_Magic = new Spell("Diffuse Magic");

    Spell Rushing_Jade_Wind = new Spell("Rushing Jade Wind");
    Spell Invoke_Xuen_the_White_Tiger = new Spell("Invoke Xuen, the White Tiger");
    Spell Chi_Torpedo = new Spell("Chi Torpedo");

    #endregion InitializeSpell

    public MonkWW()
    {
        Main.range = 3.6f; // Range

        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            if (!ObjectManager.Me.IsMounted)
            {
                Patrolling();

                if (Fight.InFight && ObjectManager.Me.Target > 0)
                {
                    if (ObjectManager.Me.Target != lastTarget && Provoke.IsDistanceGood)
                    {
                        Pull();
                        lastTarget = ObjectManager.Me.Target;
                    }

                    Combat();
                }
            }
            Thread.Sleep(20);
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

    public void Pull()
    {
        Charges();
    }

    public void Combat()
    {
        Charges();
        BuffCombat();
        AvoidMelee();
        Shield();
        Heal();
        TargetMoving();
        Decast();


        if (Touch_Of_Death.KnownSpell &&
            Touch_Of_Death.IsSpellUsable &&
            Touch_Of_Death.IsDistanceGood)
        {
            Touch_Of_Death.Launch();
            return;            

        }

        if (Jab.KnownSpell &&
            Jab.IsSpellUsable &&
            Jab.IsDistanceGood)
        {
            Jab.Launch();
            return;
        }

        if (ObjectManager.GetNumberAttackPlayer() > 2 &&
            Leg_Sweep.KnownSpell &&
            Leg_Sweep.IsSpellUsable &&
            Leg_Sweep.IsDistanceGood)
        {
            Leg_Sweep.Launch();
            return;
        }

        if (ObjectManager.GetNumberAttackPlayer() > 2 &&
            Spinning_Crane_Kick.KnownSpell &&
            Spinning_Crane_Kick.IsSpellUsable &&
            Spinning_Crane_Kick.IsDistanceGood)
        {
            Spinning_Crane_Kick.Launch();
            return;
        }


        if (Rising_Sun_Kick.KnownSpell &&
            Rising_Sun_Kick.IsSpellUsable &&
            Rising_Sun_Kick.IsDistanceGood)
        {
            Rising_Sun_Kick.Launch();
            return;

        }


        if (Chi_Wave.KnownSpell &&
            Chi_Wave.IsSpellUsable &&
            Chi_Wave.IsDistanceGood)
        {
            Chi_Wave.Launch();
            return;

        }




        if (Jab.KnownSpell &&
            Jab.IsSpellUsable &&
            Jab.IsDistanceGood)
        {
            Jab.Launch();
            return;
        }


        if (Blackout_Kick.KnownSpell &&
            Blackout_Kick.IsSpellUsable &&
            Blackout_Kick.IsDistanceGood)
        {
            Blackout_Kick.Launch();
            return;

        }


        if (Tiger_Palm.KnownSpell &&
            Tiger_Palm.IsSpellUsable &&
            Tiger_Palm.IsDistanceGood)
        {
            Tiger_Palm.Launch();
            return;

        }

        if (Jab.KnownSpell &&
            Jab.IsSpellUsable &&
            Jab.IsDistanceGood)
        {
            Jab.Launch();
            return;
        }


        if (Path_of_Blossoms.KnownSpell &&
            Path_of_Blossoms.IsSpellUsable &&
            Path_of_Blossoms.IsDistanceGood)
        {
            Path_of_Blossoms.Launch();
            return;
        }

    }
    private void Charges()
    {

        if (Provoke.KnownSpell &&
            Provoke.IsSpellUsable &&
            Provoke.IsDistanceGood)
        {
            Provoke.Launch();
            return;

        }



    }



    private void BuffCombat()
    {
        if (Tigereye_Brew.KnownSpell && Tigereye_Brew.IsSpellUsable)
            Tigereye_Brew.Launch();
    }


    private void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (Legacy_of_the_Emperor.KnownSpell && !Legacy_of_the_Emperor.HaveBuff && Legacy_of_the_Emperor.IsSpellUsable)
            Legacy_of_the_Emperor.Launch();

    }


    private void Shield()
    {
        if (ObjectManager.GetNumberAttackPlayer() > 2 && Touch_of_Karma.KnownSpell && ObjectManager.Me.HealthPercent > 0 && ObjectManager.Me.HealthPercent < 50 && Touch_of_Karma.IsSpellUsable)
            Touch_of_Karma.Launch();

        if (ObjectManager.GetNumberAttackPlayer() > 2 &&
            Touch_Of_Death.KnownSpell &&
            Touch_Of_Death.IsSpellUsable &&
            Touch_Of_Death.IsDistanceGood)
            Touch_Of_Death.Launch();

    }


    private void Heal()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (Chi_Wave.KnownSpell && ObjectManager.Me.HealthPercent > 0 && ObjectManager.Me.HealthPercent < 50 && Chi_Wave.IsSpellUsable)
            Chi_Wave.Launch();

        if (Expel_Harm.KnownSpell && ObjectManager.Me.HealthPercent > 0 && ObjectManager.Me.HealthPercent < 50 && Expel_Harm.IsSpellUsable)
            Expel_Harm.Launch();

        if (Fortifying_Brew.KnownSpell && ObjectManager.Me.HealthPercent > 0 && ObjectManager.Me.HealthPercent < 20 && Fortifying_Brew.IsSpellUsable)
            Fortifying_Brew.Launch();



    }

    private void TargetMoving()
    {

        if (ObjectManager.Target.GetMove &&
            !Disable.TargetHaveBuff &&
            Disable.KnownSpell &&
            Disable.IsDistanceGood &&
            Disable.IsSpellUsable)
        {
            Disable.Launch();

            if (Spinning_Fire_Blossom.KnownSpell &&
                Spinning_Fire_Blossom.IsSpellUsable &&
                Spinning_Fire_Blossom.IsDistanceGood)
            {
                Spinning_Fire_Blossom.Launch();
                return;
            }

            else
            {
                return;
            }

        }

    }

    private void Decast()
    {
         if (ObjectManager.Target.IsCast &&
            Spear_Hand_Strike.KnownSpell &&
            Spear_Hand_Strike.IsDistanceGood &&
            Spear_Hand_Strike.IsSpellUsable)
        {
            Spear_Hand_Strike.Launch();

            if (Zen_Meditation.KnownSpell &&
                Zen_Meditation.IsSpellUsable &&
                Zen_Meditation.IsDistanceGood)
            {
                Zen_Meditation.Launch();
                return;
            }

            else
            {
                return;
            }

        }

    }
    private void AvoidMelee()
    {
        if (ObjectManager.Target.GetDistance < 1)
            Keyboard.DownKey(nManager.Wow.Memory.WowProcess.MainWindowHandle, "{DOWN}");
    }
}

#endregion CustomClass