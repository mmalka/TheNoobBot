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
            case WoWClass.Warrior:

                Spell Bloodthirst = new Spell("Bloodthirst");
                Spell Mortal_Strike = new Spell("Mortal Strike");
                Spell Shield_Slam = new Spell("Shield Slam");

                if (Bloodthirst.KnownSpell)
                {
                    WarriorFury ccWAF = new WarriorFury();
                }

                if (Mortal_Strike.KnownSpell)
                {
                    WarriorArms ccWAA = new WarriorArms();
                }

                if (Shield_Slam.KnownSpell)
                {
                    WarriorProt ccWAP = new WarriorProt();
                }

                if (!Shield_Slam.KnownSpell && !Mortal_Strike.KnownSpell && !Bloodthirst.KnownSpell)
                {
                    Warrior ccWA = new Warrior();
                }

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

public class WarriorFury
{
    #region InitializeSpell

    Spell Berserker_Stance = new Spell("Berserker Stance");
    Spell Battle_Stance = new Spell("Battle Stance");

    Spell Inner_Rage = new Spell("Inner Rage");
    Spell Enraged_Regeneration = new Spell("Enraged Regeneration");
    Spell Battle_Shout = new Spell("Battle Shout");
    Spell Colossus_Smash = new Spell("Colossus Smash");
    Spell Bloodthirst = new Spell("Bloodthirst");
    Spell Raging_Blow = new Spell("Raging Blow");
    Spell Slam = new Spell("Slam");
    Spell Death_Wish = new Spell("Death Wish");
    Spell Recklessness = new Spell("Recklessness");
    Spell Execute = new Spell("Execute");
    Spell Heroic_Strike = new Spell("Heroic Strike");
    Spell Intercept = new Spell("Intercept");
    Spell Enrage = new Spell("Enrage");
    Spell Bloodsurge = new Spell("Bloodsurge");
    Spell Heroic_Throw = new Spell("Heroic Throw");
    Spell Heroic_Leap = new Spell("Heroic Leap");
    Spell Cleave = new Spell("Cleave");
    Spell Whirlwind = new Spell("Whirlwind");
    Spell Victory_Rush = new Spell("Victory Rush");
    Spell Hamstring = new Spell("Hamstring");
    Spell Pummel = new Spell("Pummel");
    Spell Rend = new Spell("Rend");
    Spell Mortal_Strike = new Spell("Mortal Strike");
    Spell Overpower = new Spell("Overpower");
    Spell Deadly_Calm = new Spell("Deadly Calm");
    Spell Bladestorm = new Spell("Bladestorm");
    Spell Sweeping_Strikes = new Spell("Sweeping Strikes");
    Spell Thunder_Clap = new Spell("Thunder Clap");
    Spell Throwdown = new Spell("Throwdown");
    Spell Charge = new Spell("Charge");
    Spell Strike = new Spell("Strike");
    Spell Intimidating_Shout = new Spell("Intimidating Shout");
    Spell Commanding_Shout = new Spell("Commanding Shout");
    Spell Piercing_Howl = new Spell("Piercing Howl");
    Spell Taste_for_Blood = new Spell("Taste for Blood");
    Spell Berserker_Rage = new Spell("Berserker Rage");
    Spell Victorious = new Spell("Victorious");
    Spell Retaliation = new Spell("Retaliation");

    Timer Rend_Timer = new Timer(0);
    Timer Charge_Timer = new Timer(0);
    Timer Recklessness_Timer = new Timer(0);
    Timer Enraged_Regeneration_Timer = new Timer(0);
    Timer Death_Wish_Timer = new Timer(0);
    Timer Inner_Rage_Timer = new Timer(0);
    Timer Intercept_leap_Timer = new Timer(0);

    #endregion InitializeSpell

    public WarriorFury()
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
                    if (ObjectManager.Me.Target != lastTarget && Intercept.IsDistanceGood)
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

    public void Pull()
    {
        if (!Berserker_Stance.HaveBuff)
            Berserker_Stance.Launch();

        Charges();
    }

    public void Combat()
    {
        Charges();
        AvoidMelee();
        Heal();
        BuffCombat();
        TargetMoving();
        Decast();
        Fear();

        if (Victory_Rush.KnownSpell &&
            Victory_Rush.IsSpellUsable &&
            Victory_Rush.IsDistanceGood)
        {
            Victory_Rush.Launch();

            if ((ObjectManager.Me.BarTwoPercentage > 70 || Inner_Rage.HaveBuff) &&
                Heroic_Strike.KnownSpell &&
                Heroic_Strike.IsSpellUsable)
            {
                Heroic_Strike.Launch();
                return;
            }

            else
            {
                return;
            }

        }

        if (Colossus_Smash.KnownSpell &&
            Colossus_Smash.IsSpellUsable &&
            Colossus_Smash.IsDistanceGood)
        {
            Colossus_Smash.Launch();

            if ((ObjectManager.Me.BarTwoPercentage > 70 || Inner_Rage.HaveBuff) &&
                Heroic_Strike.KnownSpell &&
                Heroic_Strike.IsSpellUsable)
            {
                Heroic_Strike.Launch();
                return;
            }

            else
            {
                return;
            }

        }

        if (Bloodthirst.KnownSpell &&
            Bloodthirst.IsDistanceGood &&
            Bloodthirst.IsSpellUsable)
        {
            Bloodthirst.Launch();

            if ((ObjectManager.Me.BarTwoPercentage > 70 || Inner_Rage.HaveBuff) &&
                Heroic_Strike.KnownSpell &&
                Heroic_Strike.IsSpellUsable)
            {
                Heroic_Strike.Launch();
                return;
            }

            else
            {
                return;
            }
        }

        if (Raging_Blow.KnownSpell &&
            Raging_Blow.IsSpellUsable &&
            Raging_Blow.IsDistanceGood)
        {
            Raging_Blow.Launch();

            if ((ObjectManager.Me.BarTwoPercentage > 70 || Inner_Rage.HaveBuff) &&
                Heroic_Strike.KnownSpell &&
                Heroic_Strike.IsSpellUsable)
            {
                Heroic_Strike.Launch();
                return;
            }

            else
            {
                return;
            }

        }

        if (ObjectManager.Me.HaveBuff(46916) &&
            Slam.KnownSpell &&
            Slam.IsDistanceGood)
        {

            Slam.Launch();

            if ((ObjectManager.Me.BarTwoPercentage > 70 || Inner_Rage.HaveBuff) &&
                Heroic_Strike.KnownSpell &&
                Heroic_Strike.IsSpellUsable)
            {
                Heroic_Strike.Launch();
                return;
            }

            else
            {
                return;
            }

        }

        if ((ObjectManager.Me.BarTwoPercentage > 60 || Inner_Rage.HaveBuff) &&
            Heroic_Strike.KnownSpell &&
            Heroic_Strike.IsSpellUsable)
        {
            Heroic_Strike.Launch();
            return;
        }

        if (Heroic_Throw.KnownSpell &&
            Heroic_Throw.IsSpellUsable &&
            Heroic_Throw.IsDistanceGood)
        {
            Heroic_Throw.Launch();
        }

    }

    public void Patrolling()
    {
        if (!Berserker_Stance.HaveBuff)
            Berserker_Stance.Launch();
    }

    private void Charges()
    {
        if (Intercept.KnownSpell && Intercept.IsSpellUsable && Intercept.IsDistanceGood && Intercept_leap_Timer.IsReady)
        {
            Intercept.Launch();
            Intercept_leap_Timer = new Timer(1000 * 3);
        }

        if (Heroic_Leap.KnownSpell && Heroic_Leap.IsSpellUsable && Heroic_Leap.IsDistanceGood && Intercept_leap_Timer.IsReady)
        {
            SpellManager.CastSpellByIDAndPosition(6544, ObjectManager.Target.Position);
            Intercept_leap_Timer = new Timer(1000 * 3);
        }
    }

    public void Heal()
    {
        if (ObjectManager.Me.HealthPercent < 75 &&
            Berserker_Rage.KnownSpell && Enraged_Regeneration_Timer.IsReady &&
            Berserker_Rage.IsSpellUsable && Enraged_Regeneration.KnownSpell)
        {
            Berserker_Rage.Launch();
            Thread.Sleep(200);

            if (Berserker_Rage.HaveBuff)
            {

                int i = 0;
                while (i < 3)
                {
                    i++;
                    Enraged_Regeneration.Launch();
                    Enraged_Regeneration_Timer = new Timer(1000 * 60 * 3);

                    if (Enraged_Regeneration.HaveBuff)
                    {
                        break;
                    }
                }
            }
        }
    }

    public void BuffCombat()
    {

        if (!Berserker_Stance.HaveBuff)
            Berserker_Stance.Launch();

        if (!Berserker_Stance.KnownSpell)
        {
            Battle_Stance.Launch();
        }

        if (Battle_Shout.KnownSpell &&
            Battle_Shout.IsSpellUsable &&
            ObjectManager.Me.BarTwoPercentage < 70)
        {
            Battle_Shout.Launch();
        }

        if (Inner_Rage_Timer.IsReady &&
            Inner_Rage.KnownSpell &&
            Inner_Rage.IsSpellUsable)
        {
            Inner_Rage.Launch();
            Inner_Rage_Timer = new Timer(1000 * 40);
        }

        if (Recklessness.KnownSpell || Death_Wish.KnownSpell)
        {
            if (Recklessness.KnownSpell && Death_Wish.KnownSpell &&
                Recklessness.IsSpellUsable && Recklessness_Timer.IsReady &&
                Death_Wish.IsSpellUsable && Death_Wish_Timer.IsReady)
            {
                int j = 0;
                while (j < 3)
                {
                    j++;
                    Recklessness.Launch();
                    Recklessness_Timer = new Timer(1000 * 300);

                    Death_Wish.Launch();
                    Death_Wish_Timer = new Timer(1000 * 150);

                    Lua.RunMacroText("/use 13");
                    Lua.RunMacroText("/script UIErrorsFrame:Clear()");
                    Lua.RunMacroText("/use 14");
                    Lua.RunMacroText("/script UIErrorsFrame:Clear()");

                    if (!Recklessness.IsSpellUsable &&
                        !Death_Wish.IsSpellUsable)
                    {
                        break;
                    }
                }
            }

            else if (Death_Wish.KnownSpell && Death_Wish.IsSpellUsable && Death_Wish_Timer.IsReady)
            {
                Death_Wish.Launch();
                Lua.RunMacroText("/use 13");
                Lua.RunMacroText("/script UIErrorsFrame:Clear()");
                Lua.RunMacroText("/use 14");
                Lua.RunMacroText("/script UIErrorsFrame:Clear()");
                Death_Wish_Timer = new Timer(1000 * 150);
            }

            else if (!Death_Wish.KnownSpell && Recklessness.IsSpellUsable && Recklessness_Timer.IsReady)
            {
                Recklessness.Launch();
                Lua.RunMacroText("/use 13");
                Lua.RunMacroText("/script UIErrorsFrame:Clear()");
                Lua.RunMacroText("/use 14");
                Lua.RunMacroText("/script UIErrorsFrame:Clear()");
                Recklessness_Timer = new Timer(1000 * 300);
            }
        }
    }

    private void TargetMoving()
    {
        if (ObjectManager.Target.GetMove &&
            !Hamstring.TargetHaveBuff &&
            Hamstring.KnownSpell &&
            Hamstring.IsDistanceGood &&
            Hamstring.IsSpellUsable)
        {
            Hamstring.Launch();
        }

        if (ObjectManager.Target.GetDistance <= 15 &&
            (ObjectManager.GetNumberAttackPlayer() >= 2 ||
            ObjectManager.Target.GetMove) &&
            !Piercing_Howl.TargetHaveBuff &&
            !Intimidating_Shout.TargetHaveBuff &&
            Piercing_Howl.KnownSpell &&
            Piercing_Howl.IsDistanceGood &&
            Piercing_Howl.IsSpellUsable)
        {
            Piercing_Howl.Launch();
        }

    }

    private void Decast()
    {
        if (ObjectManager.Target.IsCast &&
            Pummel.KnownSpell &&
            Pummel.IsSpellUsable &&
            Pummel.IsDistanceGood)
        {
            Pummel.Launch();
        }
    }

    private void Fear()
    {
        if (ObjectManager.Target.GetMove &&
            ObjectManager.Target.GetDistance <= 8 &&
            !Throwdown.TargetHaveBuff &&
            !Piercing_Howl.TargetHaveBuff &&
            Intimidating_Shout.KnownSpell &&
            Intimidating_Shout.IsSpellUsable)
        {
            Intimidating_Shout.Launch();
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

public class WarriorArms
{
    #region InitializeSpell

    Spell Berserker_Stance = new Spell("Berserker Stance");
    Spell Battle_Stance = new Spell("Battle Stance");

    Spell Inner_Rage = new Spell("Inner Rage");
    Spell Enraged_Regeneration = new Spell("Enraged Regeneration");
    Spell Battle_Shout = new Spell("Battle Shout");
    Spell Colossus_Smash = new Spell("Colossus Smash");
    Spell Bloodthirst = new Spell("Bloodthirst");
    Spell Raging_Blow = new Spell("Raging Blow");
    Spell Slam = new Spell("Slam");
    Spell Death_Wish = new Spell("Death Wish");
    Spell Recklessness = new Spell("Recklessness");
    Spell Execute = new Spell("Execute");
    Spell Heroic_Strike = new Spell("Heroic Strike");
    Spell Intercept = new Spell("Intercept");
    Spell Enrage = new Spell("Enrage");
    Spell Bloodsurge = new Spell("Bloodsurge");
    Spell Heroic_Throw = new Spell("Heroic Throw");
    Spell Heroic_Leap = new Spell("Heroic Leap");
    Spell Cleave = new Spell("Cleave");
    Spell Whirlwind = new Spell("Whirlwind");
    Spell Victory_Rush = new Spell("Victory Rush");
    Spell Hamstring = new Spell("Hamstring");
    Spell Pummel = new Spell("Pummel");
    Spell Rend = new Spell("Rend");
    Spell Mortal_Strike = new Spell("Mortal Strike");
    Spell Overpower = new Spell("Overpower");
    Spell Deadly_Calm = new Spell("Deadly Calm");
    Spell Bladestorm = new Spell("Bladestorm");
    Spell Sweeping_Strikes = new Spell("Sweeping Strikes");
    Spell Thunder_Clap = new Spell("Thunder Clap");
    Spell Throwdown = new Spell("Throwdown");
    Spell Charge = new Spell("Charge");
    Spell Strike = new Spell("Strike");
    Spell Intimidating_Shout = new Spell("Intimidating Shout");
    Spell Commanding_Shout = new Spell("Commanding Shout");
    Spell Piercing_Howl = new Spell("Piercing Howl");
    Spell Taste_for_Blood = new Spell("Taste for Blood");
    Spell Berserker_Rage = new Spell("Berserker Rage");
    Spell Victorious = new Spell("Victorious");
    Spell Retaliation = new Spell("Retaliation");
    Spell Improved_Slam = new Spell("Improved Slam");

    Timer Rend_Timer = new Timer(0);
    Timer Intercept_Timer = new Timer(0);
    Timer Recklessness_Timer = new Timer(0);
    Timer Enraged_Regeneration_Timer = new Timer(0);
    Timer Inner_Rage_Timer = new Timer(0);

    #endregion InitializeSpell

    public WarriorArms()
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
                    if (ObjectManager.Me.Target != lastTarget && Charge.IsDistanceGood)
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

    public void Pull()
    {
        if (!Battle_Stance.HaveBuff)
            Battle_Stance.Launch();

        Charges();
    }

    public void Combat()
    {
        Charges();
        AvoidMelee();
        Heal();
        BuffCombat();
        TargetMoving();
        Decast();
        Fear();
        Stunned();

        if (!Battle_Stance.HaveBuff)
            Battle_Stance.Launch();

        //////////////////////////////////////////

        if (Victory_Rush.KnownSpell && Victory_Rush.IsSpellUsable && Victory_Rush.IsDistanceGood)
        {
            Victory_Rush.Launch();

            if ((ObjectManager.Me.BarTwoPercentage > 70 || Inner_Rage.HaveBuff) &&
                Heroic_Strike.KnownSpell &&
                Heroic_Strike.IsSpellUsable)
            {
                Heroic_Strike.Launch();
                return;
            }

            else
            {
                return;
            }

        }

        //////////////////////////////////////////

        if (ObjectManager.Target.HealthPercent <= 20 &&
            Execute.KnownSpell &&
            Execute.IsSpellUsable &&
            Execute.IsDistanceGood)
        {
            Execute.Launch();

            if ((ObjectManager.Me.BarTwoPercentage > 85 ||
                 Inner_Rage.HaveBuff) &&
                 Heroic_Strike.IsDistanceGood &&
                 Heroic_Strike.KnownSpell &&
                 Heroic_Strike.IsSpellUsable)
            {
                Heroic_Strike.Launch();
                return;
            }

            else
            {
                return;
            }
        }

        //////////////////////////////////////////

        if (ObjectManager.GetNumberAttackPlayer() >= 2 &&
            Inner_Rage.HaveBuff &&
            Bladestorm.KnownSpell &&
            Bladestorm.IsSpellUsable &&
            Bladestorm.IsDistanceGood)
        {
            Bladestorm.Launch();
            return;
        }

        //////////////////////////////////////////

        if (Mortal_Strike.IsDistanceGood &&
            Sweeping_Strikes.KnownSpell &&
            Sweeping_Strikes.IsSpellUsable)
        {
            Sweeping_Strikes.Launch();
            return;
        }

        //////////////////////////////////////////

        if (Rend_Timer.IsReady &&
            Rend.KnownSpell &&
            Rend.IsDistanceGood &&
            Rend.IsSpellUsable)
        {

            Rend.Launch();
            Rend_Timer = new Timer(1000 * 13);
            return;
        }

        //////////////////////////////////////////

        if (Colossus_Smash.KnownSpell &&
            Colossus_Smash.IsSpellUsable &&
            Colossus_Smash.IsDistanceGood)
        {
            Colossus_Smash.Launch();

            if ((ObjectManager.Me.BarTwoPercentage > 85 ||
                Inner_Rage.HaveBuff) &&
                Heroic_Strike.IsDistanceGood &&
                Heroic_Strike.KnownSpell &&
                Heroic_Strike.IsSpellUsable)
            {
                Heroic_Strike.Launch();
                return;
            }

            else
            {
                return;
            }
        }

        //////////////////////////////////////////

        if (Mortal_Strike.KnownSpell &&
             Mortal_Strike.IsSpellUsable &&
             Mortal_Strike.IsDistanceGood)
        {

            Mortal_Strike.Launch();

            if ((ObjectManager.Me.BarTwoPercentage > 85 ||
                Inner_Rage.HaveBuff) &&
                Heroic_Strike.IsDistanceGood &&
                Heroic_Strike.KnownSpell &&
                Heroic_Strike.IsSpellUsable)
            {
                Heroic_Strike.Launch();
                return;
            }

            else
            {
                return;
            }
        }

        //////////////////////////////////////////

        if ((ObjectManager.Me.HaveBuff(56636) ||
           ObjectManager.Me.HaveBuff(56637) ||
           ObjectManager.Me.HaveBuff(56638)) &&
           Overpower.KnownSpell &&
           Overpower.IsSpellUsable &&
           Overpower.IsDistanceGood)
        {

            Overpower.Launch();

            if ((ObjectManager.Me.BarTwoPercentage > 85 ||
                Inner_Rage.HaveBuff) &&
                Heroic_Strike.IsDistanceGood &&
                Heroic_Strike.KnownSpell &&
                Heroic_Strike.IsSpellUsable)
            {
                Heroic_Strike.Launch();
                return;
            }

            else
            {
                return;
            }
        }

        //////////////////////////////////////////

        if (Overpower.KnownSpell &&
            Overpower.IsSpellUsable &&
            Overpower.IsDistanceGood)
        {

            Overpower.Launch();

            if ((ObjectManager.Me.BarTwoPercentage > 85 ||
                Inner_Rage.HaveBuff) &&
                Heroic_Strike.IsDistanceGood &&
                Heroic_Strike.KnownSpell &&
                Heroic_Strike.IsSpellUsable)
            {
                Heroic_Strike.Launch();
                return;
            }

            else
            {
                return;
            }
        }

        if (Retaliation.IsDistanceGood &&
             Retaliation.KnownSpell &&
             Retaliation.IsSpellUsable)
        {
            Retaliation.Launch();
            return;
        }

        if (Heroic_Throw.KnownSpell &&
            ObjectManager.Target.GetDistance < 30 &&
            Heroic_Throw.IsSpellUsable &&
            Heroic_Throw.IsDistanceGood)
        {
            Heroic_Throw.Launch();
            return;
        }

        if (Slam.KnownSpell && Slam.IsSpellUsable && Slam.IsDistanceGood)
        {
            Slam.Launch();

            if (ObjectManager.Me.BarTwoPercentage > 60)
            {
                Heroic_Strike.Launch();
                return;
            }
        }

    }

    public void Patrolling()
    {
        if (!Battle_Stance.HaveBuff)
            Battle_Stance.Launch();
    }

    public void Heal()
    {
        if (ObjectManager.Me.HealthPercent < 75 &&
            Berserker_Rage.KnownSpell && Enraged_Regeneration_Timer.IsReady &&
            Berserker_Rage.IsSpellUsable && Enraged_Regeneration.KnownSpell)
        {
            Berserker_Rage.Launch();
            Thread.Sleep(200);

            if (Berserker_Rage.HaveBuff)
            {

                int i = 0;
                while (i < 3)
                {
                    i++;
                    Enraged_Regeneration.Launch();
                    Enraged_Regeneration_Timer = new Timer(1000 * 60 * 3);

                    if (Enraged_Regeneration.HaveBuff)
                    {
                        break;
                    }
                }
            }
        }
    }

    public void BuffCombat()
    {

        if (!Battle_Stance.HaveBuff)
            Battle_Stance.Launch();

        if (Battle_Shout.KnownSpell &&
            Battle_Shout.IsSpellUsable &&
            (ObjectManager.Me.BarTwoPercentage < 70 || !Battle_Shout.HaveBuff))
        {
            Battle_Shout.Launch();
        }

        if (Rend.TargetHaveBuff && ObjectManager.GetNumberAttackPlayer() <= 2 &&
            ObjectManager.Me.HealthPercent > 70 &&
            (Deadly_Calm.KnownSpell || Inner_Rage.KnownSpell))
        {
            if (Deadly_Calm.KnownSpell && Deadly_Calm.IsSpellUsable && Inner_Rage_Timer.IsReady)
            {
                int i = 0;
                while (i < 3)
                {
                    i++;
                    Deadly_Calm.Launch();
                    Inner_Rage.Launch();
                    Inner_Rage_Timer = new Timer(1000 * 40);
                }

                Burst();
            }

            else if (Inner_Rage_Timer.IsReady && Inner_Rage.KnownSpell && Inner_Rage.IsSpellUsable)
            {
                Inner_Rage.Launch();
                Inner_Rage_Timer = new Timer(1000 * 40);
                return;
            }
        }

        if (Rend.TargetHaveBuff && Inner_Rage.HaveBuff &&
            ObjectManager.Me.HealthPercent > 50 &&
            Recklessness.KnownSpell && Recklessness_Timer.IsReady)
        {
            if (!Berserker_Stance.HaveBuff)
            {
                int i = 0;
                while (i < 3)
                {
                    i++;
                    Berserker_Stance.Launch();

                    if (Berserker_Stance.HaveBuff)
                    {
                        break;
                    }
                }
            }

            if (Berserker_Stance.HaveBuff)
            {
                if (!Recklessness.HaveBuff)
                {
                    int j = 0;
                    while (j < 3)
                    {
                        j++;
                        Recklessness.Launch();
                        Lua.RunMacroText("/use 13");
                        Lua.RunMacroText("/script UIErrorsFrame:Clear()");
                        Lua.RunMacroText("/use 14");
                        Lua.RunMacroText("/script UIErrorsFrame:Clear()");
                        Recklessness_Timer = new Timer(1000 * 60 * 5);

                        if (!Recklessness.IsSpellUsable)
                        {
                            break;
                        }
                    }
                }
            }

            if (!Battle_Stance.HaveBuff)
            {
                int k = 0;
                while (k < 0)
                {
                    k++;
                    Battle_Stance.Launch();

                    if (Battle_Stance.HaveBuff)
                    {
                        break;
                    }
                }
            }
        }
    }

    public void Burst()
    {
        if (Recklessness.KnownSpell && Recklessness_Timer.IsReady)
        {
            if (!Berserker_Stance.HaveBuff)
            {
                int i = 0;
                while (i < 3)
                {
                    i++;
                    Berserker_Stance.Launch();

                    if (Berserker_Stance.HaveBuff)
                    {
                        break;
                    }
                }
            }

            if (Berserker_Stance.HaveBuff)
            {
                if (!Recklessness.HaveBuff)
                {
                    int j = 0;
                    while (j < 3)
                    {
                        j++;
                        Recklessness.Launch();
                        Lua.RunMacroText("/use 13");
                        Lua.RunMacroText("/script UIErrorsFrame:Clear()");
                        Lua.RunMacroText("/use 14");
                        Lua.RunMacroText("/script UIErrorsFrame:Clear()");
                        Recklessness_Timer = new Timer(1000 * 60 * 5);

                        if (!Recklessness.IsSpellUsable)
                        {
                            break;
                        }
                    }
                }
            }

            if (!Battle_Stance.HaveBuff)
            {
                int k = 0;
                while (k < 0)
                {
                    k++;
                    Battle_Stance.Launch();

                    if (Battle_Stance.HaveBuff)
                    {
                        break;
                    }
                }
            }
        }
    }

    private void TargetMoving()
    {
        if (ObjectManager.Target.GetMove &&
            !Hamstring.TargetHaveBuff &&
            Hamstring.KnownSpell &&
            Hamstring.IsDistanceGood &&
            Hamstring.IsSpellUsable)
        {
            Hamstring.Launch();
        }

        if (ObjectManager.Target.GetDistance <= 15 &&
            (ObjectManager.GetNumberAttackPlayer() >= 2 ||
            ObjectManager.Target.GetMove) &&
            !Piercing_Howl.TargetHaveBuff &&
            !Intimidating_Shout.TargetHaveBuff &&
            Piercing_Howl.KnownSpell &&
            Piercing_Howl.IsDistanceGood &&
            Piercing_Howl.IsSpellUsable)
        {
            Piercing_Howl.Launch();
        }

    }

    private void Stunned()
    {
        if (!Intimidating_Shout.TargetHaveBuff &&
            Throwdown.KnownSpell &&
            Throwdown.IsDistanceGood &&
            Throwdown.IsSpellUsable)
        {
            Throwdown.Launch();
        }

    }

    private void Fear()
    {
        if (ObjectManager.Target.GetMove &&
            ObjectManager.Target.GetDistance <= 8 &&
            !Throwdown.TargetHaveBuff &&
            !Piercing_Howl.TargetHaveBuff &&
            Intimidating_Shout.KnownSpell &&
            Intimidating_Shout.IsSpellUsable)
        {
            Intimidating_Shout.Launch();
        }
    }

    private void Charges()
    {
        if (Charge.KnownSpell &&
            Charge.IsSpellUsable &&
            Charge.IsDistanceGood)
        {
            Charge.Launch();
        }

        if (Heroic_Leap.KnownSpell && Heroic_Leap.IsSpellUsable && Heroic_Leap.IsDistanceGood)
        {
            SpellManager.CastSpellByIDAndPosition(6544, ObjectManager.Target.Position);
        }
    }

    private void Decast()
    {
        if (ObjectManager.Target.IsCast &&
            Pummel.KnownSpell &&
            Pummel.IsSpellUsable &&
            Pummel.IsDistanceGood)
        {
            Pummel.Launch();
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

public class WarriorProt
{
    #region InitializeSpell

    Spell Berserker_Stance = new Spell("Berserker Stance");
    Spell Enraged_Regeneration = new Spell("Enraged Regeneration");
    Spell Battle_Shout = new Spell("Battle Shout");
    Spell Colossus_Smash = new Spell("Colossus Smash");
    Spell Bloodthirst = new Spell("Bloodthirst");
    Spell Raging_Blow = new Spell("Raging Blow");
    Spell Slam = new Spell("Slam");
    Spell Death_Wish = new Spell("Death Wish");
    Spell Recklessness = new Spell("Recklessness");
    Spell Execute = new Spell("Execute");
    Spell Heroic_Strike = new Spell("Heroic Strike");
    Spell Intercept = new Spell("Intercept");
    Spell Enrage = new Spell("Enrage");
    Spell Bloodsurge = new Spell("Bloodsurge");
    Spell Heroic_Throw = new Spell("Heroic Throw");
    Spell Heroic_Leap = new Spell("Heroic Leap");
    Spell Cleave = new Spell("Cleave");
    Spell Whirlwind = new Spell("Whirlwind");
    Spell Victory_Rush = new Spell("Victory Rush");
    Spell Hamstring = new Spell("Hamstring");
    Spell Pummel = new Spell("Pummel");
    Spell Shield_Slam = new Spell("Shield Slam");
    Spell Concussion_Blow = new Spell("Concussion Blow");
    Timer Rend_Timer = new Timer(0);
    Timer Recklessness_Timer = new Timer(0);
    Timer Enraged_Regeneration_Timer = new Timer(0);
    Spell Rend = new Spell("Rend");
    Spell Shockwave = new Spell("Shockwave");
    Spell Devastate = new Spell("Devastate");
    Spell Shield_Bash = new Spell("Shield Bash");
    Spell Last_Stand = new Spell("Last Stand");
    Spell Shield_Block = new Spell("Shield Block");
    Spell Shield_Wall = new Spell("Shield Wall");
    Spell Spell_Reflection = new Spell("Spell Reflection");
    Spell Charge = new Spell("Charge");
    Spell Revenge = new Spell("Revenge");
    Spell Thunder_Clap = new Spell("Thunder Clap");
    Spell Defensive_Stance = new Spell("Defensive Stance");
    Spell Sunder_Armor = new Spell("Sunder Armor");
    Spell Strike = new Spell("Strike");
    Spell Battle_Stance = new Spell("Battle Stance");
    Spell Intimidating_Shout = new Spell("Intimidating_Shout");
    Spell Piercing_Howl = new Spell("Piercing_Howl");
    Spell Berserker_Rage = new Spell("Berserker_Rage");

    #endregion InitializeSpell

    public WarriorProt()
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
                    if (ObjectManager.Me.Target != lastTarget && Intercept.IsDistanceGood)
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

    public void Pull()
    {

        if (!Defensive_Stance.HaveBuff)
            Defensive_Stance.Launch();

        if (Heroic_Throw.KnownSpell &&
            ObjectManager.Target.GetDistance < 30 &&
            Heroic_Throw.IsSpellUsable &&
            Heroic_Throw.IsDistanceGood)
        {
            Heroic_Throw.Launch();
        }
    }

    public void Combat()
    {
        Charges();
        Def();
        AvoidMelee();
        Heal();
        BuffCombat();
        Decast();
        Fear();
        Burst();

        if ((ObjectManager.Me.HaveBuff(46951) ||
            ObjectManager.Me.HaveBuff(46952) ||
            ObjectManager.Me.HaveBuff(46953)) &&
            Shield_Slam.KnownSpell && Shield_Slam.IsSpellUsable && Shield_Slam.IsDistanceGood)
        {
            Shield_Slam.Launch();
            return;
        }

        if (Rend_Timer.IsReady && Rend.KnownSpell && Rend.IsDistanceGood && Rend.IsSpellUsable)
        {
            Rend.Launch();
            Rend_Timer = new Timer(1000 * 10);
            return;
        }

        if (ObjectManager.GetNumberAttackPlayer() >= 2 &&
            Cleave.KnownSpell &&
            Cleave.IsSpellUsable &&
            Cleave.IsDistanceGood)
        {
            Cleave.Launch();
            return;
        }

        if (Victory_Rush.KnownSpell && Victory_Rush.IsSpellUsable && Victory_Rush.IsDistanceGood)
        {
            Victory_Rush.Launch();
            return;
        }

        if (Shield_Slam.KnownSpell && Shield_Slam.IsSpellUsable && Shield_Slam.IsDistanceGood)
        {
            Shield_Slam.Launch();
            return;
        }

        if (Revenge.KnownSpell && Revenge.IsDistanceGood && Revenge.IsSpellUsable)
        {
            Revenge.Launch();
            return;
        }

        if (!Recklessness_Timer.IsReady &&
            Concussion_Blow.KnownSpell && Concussion_Blow.IsSpellUsable && Concussion_Blow.IsDistanceGood)
        {

            Concussion_Blow.Launch();
            return;
        }

        if (Shockwave.KnownSpell && Shockwave.IsSpellUsable && Shockwave.IsDistanceGood)
        {
            Shockwave.Launch();
            return;
        }

        if (Devastate.KnownSpell && Devastate.IsSpellUsable && Devastate.IsDistanceGood)
        {
            if (Sunder_Armor.TargetHaveBuff)
            {
                Heroic_Strike.Launch();
                return;
            }

            else
            {
                Devastate.Launch();
                return;
            }
        }

    }

    public void Patrolling()
    {
        if (!Defensive_Stance.HaveBuff)
            Defensive_Stance.Launch();
    }

    public void Heal()
    {
        if (ObjectManager.Me.HealthPercent < 75 &&
            Berserker_Rage.KnownSpell && Enraged_Regeneration_Timer.IsReady &&
            Berserker_Rage.IsSpellUsable && Enraged_Regeneration.KnownSpell)
        {
            Berserker_Rage.Launch();
            Thread.Sleep(200);

            if (Berserker_Rage.HaveBuff)
            {

                int i = 0;
                while (i < 3)
                {
                    i++;
                    Enraged_Regeneration.Launch();
                    Enraged_Regeneration_Timer = new Timer(1000 * 60 * 3);

                    if (Enraged_Regeneration.HaveBuff)
                    {
                        break;
                    }
                }
            }
        }
    }

    public void Burst()
    {
        if (Rend.TargetHaveBuff && Shield_Block.HaveBuff && Concussion_Blow.KnownSpell &&
            Concussion_Blow.IsSpellUsable && Colossus_Smash.KnownSpell &&
            ObjectManager.Me.HealthPercent > 50 &&
            Recklessness.KnownSpell && Recklessness_Timer.IsReady)
        {
            if (!Berserker_Stance.HaveBuff)
            {
                int i = 0;
                while (i < 3)
                {
                    i++;
                    Berserker_Stance.Launch();

                    if (Berserker_Stance.HaveBuff)
                    {
                        break;
                    }
                }
            }

            if (Berserker_Stance.HaveBuff &&
                Concussion_Blow.KnownSpell &&
                Concussion_Blow.IsSpellUsable &&
                Concussion_Blow.IsDistanceGood)
            {
                int l = 0;
                while (l < 0)
                {
                    l++;
                    Concussion_Blow.Launch();

                    if (!Concussion_Blow.IsSpellUsable)
                    {
                        break;
                    }
                }
            }

            if (Berserker_Stance.HaveBuff)
            {
                if (!Recklessness.HaveBuff)
                {
                    int j = 0;
                    while (j < 3)
                    {
                        j++;
                        Recklessness.Launch();
                        Lua.RunMacroText("/use 13");
                        Lua.RunMacroText("/script UIErrorsFrame:Clear()");
                        Lua.RunMacroText("/use 14");
                        Lua.RunMacroText("/script UIErrorsFrame:Clear()");
                        Recklessness_Timer = new Timer(1000 * 60 * 5);

                        if (!Recklessness.IsSpellUsable)
                        {
                            break;
                        }
                    }
                }
            }

            if (Berserker_Stance.HaveBuff &&
                Colossus_Smash.KnownSpell && Colossus_Smash.IsSpellUsable && Colossus_Smash.IsDistanceGood)
            {
                int m = 0;
                while (m < 0)
                {
                    m++;
                    Colossus_Smash.Launch();

                    if (!Colossus_Smash.IsSpellUsable)
                    {
                        break;
                    }
                }

            }

            if (!Defensive_Stance.HaveBuff)
            {
                int k = 0;
                while (k < 0)
                {
                    k++;
                    Defensive_Stance.Launch();

                    if (Defensive_Stance.HaveBuff)
                    {
                        break;
                    }
                }
            }
        }
    }

    public void BuffCombat()
    {
        if (!Defensive_Stance.HaveBuff)
            Defensive_Stance.Launch();

        if (Shield_Block.KnownSpell &&
            Shield_Block.IsSpellUsable)
        {
            Shield_Block.Launch();
        }

        if (Battle_Shout.KnownSpell &&
            Battle_Shout.IsSpellUsable &&
            ObjectManager.Me.BarTwoPercentage < 70)
        {
            Battle_Shout.Launch();
        }
    }

    private void Def()
    {
        if (ObjectManager.Me.HealthPercent < 60 &&
            Shield_Wall.KnownSpell && Shield_Wall.IsSpellUsable)
        {
            Shield_Wall.Launch();
        }

        if (ObjectManager.Me.HealthPercent < 60 &&
            Last_Stand.KnownSpell && Last_Stand.IsSpellUsable)
        {
            Last_Stand.Launch();
        }
    }

    private void Decast()
    {
        if (ObjectManager.Target.IsCast &&
            Spell_Reflection.KnownSpell &&
            Spell_Reflection.IsSpellUsable &&
            Spell_Reflection.IsDistanceGood)
        {
            Spell_Reflection.Launch();
        }

        if (ObjectManager.Target.IsCast &&
            Shield_Bash.KnownSpell &&
            Shield_Bash.IsSpellUsable &&
            Shield_Bash.IsDistanceGood)
        {
            Shield_Bash.Launch();
        }
    }

    private void Fear()
    {
        if (ObjectManager.Target.GetMove &&
            ObjectManager.Target.GetDistance <= 8 &&
            !Piercing_Howl.TargetHaveBuff &&
            Intimidating_Shout.KnownSpell &&
            Intimidating_Shout.IsSpellUsable)
        {
            Intimidating_Shout.Launch();
        }
    }

    private void Charges()
    {
        if (Intercept.KnownSpell && Intercept.IsSpellUsable && Intercept.IsDistanceGood)
        {
            Intercept.Launch();
        }

        if (Heroic_Leap.KnownSpell && Heroic_Leap.IsSpellUsable && Heroic_Leap.IsDistanceGood)
        {
            SpellManager.CastSpellByIDAndPosition(6544, ObjectManager.Target.Position);
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

public class Warrior
{
    #region InitializeSpell

    Spell Berserker_Stance = new Spell("Berserker Stance");
    Spell Battle_Stance = new Spell("Battle Stance");

    Spell Enraged_Regeneration = new Spell("Enraged Regeneration");
    Spell Battle_Shout = new Spell("Battle Shout");
    Spell Colossus_Smash = new Spell("Colossus Smash");
    Spell Bloodthirst = new Spell("Bloodthirst");
    Spell Raging_Blow = new Spell("Raging Blow");
    Spell Slam = new Spell("Slam");
    Spell Death_Wish = new Spell("Death Wish");
    Spell Recklessness = new Spell("Recklessness");
    Spell Execute = new Spell("Execute");
    Spell Heroic_Strike = new Spell("Heroic Strike");
    Spell Intercept = new Spell("Intercept");
    Spell Enrage = new Spell("Enrage");
    Spell Bloodsurge = new Spell("Bloodsurge");
    Spell Heroic_Throw = new Spell("Heroic Throw");
    Spell Heroic_Leap = new Spell("Heroic Leap");
    Spell Cleave = new Spell("Cleave");
    Spell Whirlwind = new Spell("Whirlwind");
    Spell Victory_Rush = new Spell("Victory Rush");
    Spell Hamstring = new Spell("Hamstring");
    Spell Pummel = new Spell("Pummel");
    Spell Rend = new Spell("Rend");
    Spell Mortal_Strike = new Spell("Mortal Strike");
    Spell Overpower = new Spell("Overpower");
    Spell Deadly_Calm = new Spell("Deadly Calm");
    Spell Bladestorm = new Spell("Bladestorm");
    Spell Sweeping_Strikes = new Spell("Sweeping Strikes");
    Spell Thunder_Clap = new Spell("Thunder Clap");
    Spell Throwdown = new Spell("Throwdown");
    Spell Charge = new Spell("Charge");
    Spell Strike = new Spell("Strike");
    Timer Rend_Timer = new Timer(0);

    #endregion InitializeSpell

    public Warrior()
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
                    if (ObjectManager.Me.Target != lastTarget && Charge.IsDistanceGood)
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

    public void Pull()
    {
        if (!Battle_Stance.HaveBuff)
            Battle_Stance.Launch();

        if (Charge.KnownSpell &&
            ObjectManager.Target.GetDistance > 8 &&
            Charge.IsSpellUsable &&
            Charge.IsDistanceGood)
        {
            Charge.Launch();
        }

    }

    public void Combat()
    {
        AvoidMelee();

        if (Strike.KnownSpell &&
            Strike.IsSpellUsable &&
            Strike.IsDistanceGood)
        {
            Strike.Launch();
            return;
        }

        if (Victory_Rush.KnownSpell &&
            Victory_Rush.IsSpellUsable &&
            Victory_Rush.IsDistanceGood)
        {
            Victory_Rush.Launch();
            return;
        }

        if (Rend_Timer.IsReady &&
            Rend.KnownSpell &&
            Rend.IsDistanceGood &&
            Rend.IsSpellUsable)
        {
            Rend.Launch();
            Rend_Timer = new Timer(1000 * 10);
            return;
        }

        if (ObjectManager.GetNumberAttackPlayer() > 3 &&
            Thunder_Clap.KnownSpell &&
            Thunder_Clap.IsSpellUsable &&
            Thunder_Clap.IsDistanceGood)
        {
            Thunder_Clap.Launch();
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

    public void Patrolling()
    {

    }

}

#endregion CustomClass
