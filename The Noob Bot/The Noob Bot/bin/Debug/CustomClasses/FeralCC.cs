using System;
using System.Threading;
using System.Windows.Forms;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;

/// <summary>
/// Stuff marked with a NYI = Not yett implemented
/// </summary>
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
            #region Druid
            case WoWClass.Druid:

                Spell Mangle = new Spell("Mangle");
                if (Mangle.KnownSpell)
                    new DruidFeral();

                break;
            #endregion Druid

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

public class DruidFeral
{
    #region Inits

    float PullRange;
    UInt64 lastTarget;
    UInt64 Injured;
    UInt64 LowHealth;
    UInt64 CriticalHealth;

    Int64 interupt_cost;

    WoWPlayer Me = ObjectManager.Me;
    WoWUnit Target = ObjectManager.Target;
    bool haveFocus;

    #region balanceSpells
    Spell Barkskin = new Spell("Barkskin");
    Spell Cyclone = new Spell("Cyclone");
    Spell Entangling_Roots = new Spell("Entangling Roots");
    Spell Faerie_Fire = new Spell("Faerie Fire");
    Spell Hibernate = new Spell("Hibernate");
    Spell Hurricane = new Spell("Hurricane");
    Spell Innervate = new Spell("Innervate");
    Spell Insect_Swarm = new Spell("Insect Swarm");
    Spell Moonfire = new Spell("Moonfire");
    Spell Natures_Grasp = new Spell("Nature's Grasp");
    Spell Soothe = new Spell("Soothe");
    Spell Starfire = new Spell("Starfire");
    Spell Moonglade = new Spell("Teleport: Moonglade");
    Spell Thorns = new Spell("Thorns");
    Spell Wrath = new Spell("Wrath");
    Spell WildShroom = new Spell("Wild Mushroom");
    Spell WildShroom_deto = new Spell("Wild Mushroom: Detonate");
    #endregion balanceSpells
    #region feralSpells
    Spell Bash = new Spell("Bash");
    Spell Bear_Form = new Spell("Bear Form");
    Spell Berserk = new Spell("Berserk");
    Spell Cat_Form = new Spell("Cat Form");
    Spell Challenging_Roar = new Spell("Challenging Roar");
    Spell Claw = new Spell("Claw");
    Spell Cower = new Spell("Cower");
    Spell Dash = new Spell("Dash");
    Spell Demo_roar = new Spell("Demoralizing Roar");
    Spell Enrage = new Spell("Enrage");
    Spell FFF = new Spell("Faerie Fire (Feral)");
    Spell Flight_Form = new Spell("Flight Form");
    Spell Bear_Charge = new Spell(16979);
    Spell Ferocious_Bite = new Spell("Ferocious Bite");
    Spell Frenzied_Regen = new Spell("Frenzied Regeneration");
    Spell Taunt = new Spell("Growl");
    Spell Lacerate = new Spell("Lacerate");
    Spell Maim = new Spell("Maim");
    Spell Bear_Mangle = new Spell(33878);
    Spell Maul = new Spell("Maul");
    Spell Pounce = new Spell("Pounce");
    Spell Prowl = new Spell("Prowl");
    Spell Rake = new Spell("Rake");
    Spell Ravage = new Spell("Ravage");
    Spell Rip = new Spell("Rip");
    Spell Savage_Roar = new Spell("Savage Roar");
    Spell Shred = new Spell("Shred");
    Spell Cat_Skull_Bash = new Spell(80965);
    Spell Bear_Skull_Bash = new Spell(80964);
    Spell Bear_StampRoar = new Spell(77761);
    Spell Cat_StampRoar = new Spell(77764);
    Spell Swift_Flight_Form = new Spell("Swift Flight Form");
    Spell Bear_Swipe = new Spell(779);
    Spell Cat_Swipe = new Spell(62078);
    Spell Thrash = new Spell("Thrash");
    Spell Tigers_Fury = new Spell("Tiger's Fury");
    Spell Travel_Form = new Spell("Travel Form");

    #endregion feralSpells
    #region restoSpells
    Spell Healing_Touch = new Spell("Healing Touch");
    Spell Lifebloom = new Spell("Lifebloom");
    Spell Mark_of_the_Wild = new Spell("Mark of the Wild");
    Spell Regrowth = new Spell("Regrowth");
    Spell Rejuvenation = new Spell("Rejuvenation");
    Spell Tranquility = new Spell("Tranquility");
    #endregion restoSpells
    #endregion Inits

    //private DruidConfig df = new DruidConfig(); //NYI

    public DruidFeral()
    {
        Main.range = 3.6f;  //Main ability range
        PullRange = 24.0f;  //Pull spell range
        lastTarget = 0;

        Injured = 90;
        LowHealth = 50;
        CriticalHealth = 30;
        interupt_cost = Cat_Skull_Bash.Cost;

        //std = new Thread(new ThreadStart(DoAllways)); //NYI
        //std.Start();                                  //NYI



        haveFocus = false;//HaveFocus();//NYI waiting for a working Config CC

        while (Main.loop)
        {
            if (!Mounted() && !InCombat())
            {
                Patrolling();
                if (haveFocus) //NYI
                    FollowFocus();
            }


            while (InFight() && HaveTarget())
            {
                if (haveFocus) //NYI
                    AssistFocus();

                DoAllways();

                if (!IsTarget(lastTarget) && DistanceToTarget() <= PullRange)
                    Pull();
                else
                    Combat();
            }

            DoSavage_Roar();
        }
    }


    #region PlayerState

    public void Pull()
    {
        Charge();
        lastTarget = Me.Target;
    }

    public void Patrolling()
    {
        CheckUseItems();

        if (Heal())
            return;

        Buff();
        Mana();
        CheckForm();
    }

    public void Combat()
    {
        CheckForm();
        if ((Cat_Form.HaveBuff && Me.BarTwoPercentage > (2 * interupt_cost)) || Bear_Form.HaveBuff)
        {
            DoFFF();

            if (Target.HealthPercent > Injured)
                DoBerserk();

            if (Me.HealthPercent < LowHealth)
                DoBarkskin();

            DoTigers_Fury();
            DoRavage();

            if (Me.ComboPoint > 3)
                if (!Rip.TargetHaveBuff && Target.HealthPercent > 30)
                    DoRip();
                else
                    DoFerocious_Bite();

            DoRake();

            if (8 <= Me.Level && Me.Level < 10)
                DoClaw();
            else if (10 <= Me.Level)
                DoCat_Mangle();
        }
    }

    #endregion PlayerState



    #region DruidBalanceSpells
    public void DoBarkskin()
    {
        if (Barkskin.KnownSpell &&
            Barkskin.IsSpellUsable)
            Barkskin.Launch();
    }
    public void DoCyclone()
    {
        if (Cyclone.KnownSpell &&
            Cyclone.IsSpellUsable &&
            Cyclone.IsDistanceGood)
            Cyclone.Launch();
    }
    public void DoEntangling_Roots()
    {
        if (Entangling_Roots.KnownSpell &&
            Entangling_Roots.IsSpellUsable &&
            Entangling_Roots.IsDistanceGood)
            Entangling_Roots.Launch();
    }
    public void DoFaerie_Fire()
    {
        if (Faerie_Fire.KnownSpell &&
            Faerie_Fire.IsSpellUsable &&
            Faerie_Fire.IsDistanceGood)
            Faerie_Fire.Launch();
    }
    public void DoHibernate()
    {
        if (Hibernate.KnownSpell &&
            Hibernate.IsSpellUsable &&
            Hibernate.IsDistanceGood)
            Hibernate.Launch();
    }
    public void DoHurricane()
    {
        if (Hurricane.KnownSpell &&
            Hurricane.IsSpellUsable &&
            Hurricane.IsDistanceGood)
            Hurricane.Launch();
    }
    public void DoInnervate()
    {
        if (Innervate.KnownSpell && Innervate.IsSpellUsable)
            Innervate.Launch();
    }
    public void DoInsect_Swarm()
    {
        if (Insect_Swarm.KnownSpell &&
            Insect_Swarm.IsSpellUsable &&
            Insect_Swarm.IsDistanceGood)
            Insect_Swarm.Launch();
    }
    public void DoMoonfire()
    {
        if (Moonfire.KnownSpell &&
            Moonfire.IsSpellUsable &&
            Moonfire.IsDistanceGood)
            Moonfire.Launch();
    }
    public void DoNatures_Grasp()
    {
        if (Natures_Grasp.KnownSpell &&
            Natures_Grasp.IsSpellUsable)
            Natures_Grasp.Launch();
    }
    public void DoSoothe()
    {
        if (Soothe.KnownSpell &&
            Soothe.IsSpellUsable &&
            Soothe.IsDistanceGood)
            Soothe.Launch();
    }
    public void DoStarfire()
    {
        if (Starfire.KnownSpell &&
            Starfire.IsSpellUsable &&
            Starfire.IsDistanceGood)
            Starfire.Launch();
    }
    public void DoMoonglade()
    {
        if (Moonglade.KnownSpell &&
            Moonglade.IsSpellUsable)
            Moonglade.Launch();
    }
    public void DoThorns()
    {
        if (Thorns.KnownSpell &&
            Thorns.IsSpellUsable &&
            Thorns.IsDistanceGood)
            Thorns.Launch();
    }
    public void DoWrath()
    {
        if (Wrath.KnownSpell &&
            Wrath.IsSpellUsable &&
            Wrath.IsDistanceGood)
            Wrath.Launch();
    }
    public void DoWildShroom()
    {
        if (WildShroom.KnownSpell &&
            WildShroom.IsSpellUsable &&
            WildShroom.IsDistanceGood)
            WildShroom.Launch();
    }
    public void DoWildShroom_deto()
    {
        if (WildShroom_deto.KnownSpell &&
            WildShroom_deto.IsSpellUsable &&
            WildShroom_deto.IsDistanceGood)
            WildShroom_deto.Launch();
    }
    #endregion DruidBalanceSpells

    #region DruidFeralSpells
    public void DoBash()
    {
        if (Bash.KnownSpell &&
            Bash.IsSpellUsable &&
            Bash.IsDistanceGood)
            Bash.Launch();
    }
    public void DoBear_Form()
    {
        if (Bear_Form.KnownSpell &&
            Bear_Form.IsSpellUsable)
            Bear_Form.Launch();
    }
    public void DoBerserk()
    {
        if (Berserk.KnownSpell &&
            Berserk.IsSpellUsable)
            Berserk.Launch();
    }
    public void DoCat_Form()
    {
        if (Cat_Form.KnownSpell &&
            Cat_Form.IsSpellUsable)
            Cat_Form.Launch();
    }
    public void DoChallenging_Roar()
    {
        if (Challenging_Roar.KnownSpell &&
            Challenging_Roar.IsSpellUsable)
            Challenging_Roar.Launch();
    }
    public void DoClaw()
    {
        if (Claw.KnownSpell &&
            Claw.IsSpellUsable &&
            Claw.IsDistanceGood)
            Claw.Launch();
    }
    public void DoCower()
    {
        if (Cower.KnownSpell &&
            Cower.IsSpellUsable &&
            Cower.IsDistanceGood)
            Cower.Launch();
    }
    public void DoDash()
    {
        if (Dash.KnownSpell &&
            Dash.IsSpellUsable)
            Dash.Launch();
    }
    public void DoDemo_roar()
    {
        if (Demo_roar.KnownSpell &&
            Demo_roar.IsSpellUsable &&
            Demo_roar.IsDistanceGood)
            Demo_roar.Launch();
    }
    public void DoEnrage()
    {
        if (Enrage.KnownSpell &&
            Enrage.IsSpellUsable)
            Enrage.Launch();
    }
    public void DoFFF()
    {
        if (FFF.KnownSpell &&
            FFF.IsSpellUsable &&
            FFF.IsDistanceGood &&
            (!FFF.TargetHaveBuff || FFF.TargetBuffStack < 3))
            FFF.Launch();
    }
    public void DoFlight_Form()
    {
        if (Flight_Form.KnownSpell &&
            Flight_Form.IsSpellUsable)
            Flight_Form.Launch();
    }
    public void DoBear_Charge()
    {
        if (Bear_Charge.KnownSpell &&
            Bear_Charge.IsSpellUsable &&
            Bear_Charge.IsDistanceGood)
            Bear_Charge.Launch();
    }
    public void DoCat_Charge()
    {
        if (DistanceToTarget() < 25.0f
            /*Cat_Charge.KnownSpell &&
            Cat_Charge.IsSpellUsable &&
            Cat_Charge.IsDistanceGood*/)
        {
            //Cat_Charge.Launch();
            Macro("/cast Feral Charge(Cat form)");
        }
    }
    public void DoFerocious_Bite()
    {
        if (Ferocious_Bite.KnownSpell &&
            Ferocious_Bite.IsSpellUsable &&
            Ferocious_Bite.IsDistanceGood)
            Ferocious_Bite.Launch();
    }
    public void DoFrenzied_Regen()
    {
        if (Frenzied_Regen.KnownSpell &&
            Frenzied_Regen.IsSpellUsable)
            Frenzied_Regen.Launch();
    }
    public void DoTaunt()
    {
        if (Taunt.KnownSpell &&
            Taunt.IsSpellUsable &&
            Taunt.IsDistanceGood)
            Taunt.Launch();
    }
    public void DoLacerate()
    {
        if (Lacerate.KnownSpell &&
            Lacerate.IsSpellUsable &
            Lacerate.IsDistanceGood)
            Lacerate.Launch();
    }
    public void DoMaim()
    {
        if (Maim.KnownSpell &&
            Maim.IsSpellUsable &&
            Maim.IsDistanceGood)
            Maim.Launch();
    }
    public void DoCat_Mangle()
    {
        //if ()
        //Cat_Mangle.Launch();
        Macro("/cast Mangle(Cat form)");
    }
    public void DoBear_Mangle()
    {
        if (Bear_Mangle.KnownSpell &&
            Bear_Mangle.IsSpellUsable &&
            Bear_Mangle.IsDistanceGood)
            Bear_Mangle.Launch();
    }
    public void DoMaul()
    {
        if (Maul.KnownSpell &&
            Maul.IsSpellUsable &&
            Maul.IsDistanceGood)
            Maul.Launch();
    }
    public void DoPounce()
    {
        if (Pounce.KnownSpell &&
            Pounce.IsSpellUsable &&
            Pounce.IsDistanceGood)
            Pounce.Launch();
    }
    public void DoProwl()
    {
        if (Prowl.KnownSpell &&
            Prowl.IsSpellUsable)
            Prowl.Launch();
    }
    public void DoRake()
    {
        if (Rake.KnownSpell &&
            Rake.IsSpellUsable &&
            Rake.IsDistanceGood &&
            !Rake.TargetHaveBuff)
            Rake.Launch();
    }
    public void DoRavage()
    {
        Ravage.Launch();
    }
    public void DoRip()
    {
        if (Rip.KnownSpell &&
            Rip.IsSpellUsable &&
            Rip.IsDistanceGood)
            Rip.Launch();
    }
    public void DoSavage_Roar()
    {
        if (Savage_Roar.KnownSpell &&
            Savage_Roar.IsSpellUsable &&
            Me.ComboPoint > 0)
            Savage_Roar.Launch();
    }
    public void DoShred()
    {
        if (Shred.KnownSpell &&
            Shred.IsSpellUsable &&
            Shred.IsDistanceGood)
            Shred.Launch();
    }
    public void DoCat_Skull_Bash()
    {
        if (Cat_Skull_Bash.KnownSpell &&
            Cat_Skull_Bash.IsSpellUsable &&
            Cat_Skull_Bash.IsDistanceGood)
        {
            Cat_Skull_Bash.Launch();
            Macro("/cast Skull Bash(Cat form)");
        }
    }
    public void DoBear_Skull_Bash()
    {
        if (Bear_Skull_Bash.KnownSpell &&
            Bear_Skull_Bash.IsSpellUsable &&
            Bear_Skull_Bash.IsDistanceGood)
            Bear_Skull_Bash.Launch();
    }
    public void DoBear_StampRoar()
    {
        if (Bear_StampRoar.KnownSpell &&
            Bear_StampRoar.IsSpellUsable)
            Bear_StampRoar.Launch();
    }
    public void DoCat_StampRoar()
    {
        if (Cat_StampRoar.KnownSpell &&
            Cat_StampRoar.IsSpellUsable)
            Cat_StampRoar.Launch();
    }
    public void DoSwift_Flight_Form()
    {
        if (Swift_Flight_Form.KnownSpell &&
            Swift_Flight_Form.IsSpellUsable)
            Swift_Flight_Form.Launch();
    }
    public void DoBear_Swipe()
    {
        if (Bear_Swipe.KnownSpell &&
            Bear_Swipe.IsSpellUsable &&
            Bear_Swipe.IsDistanceGood)
            Bear_Swipe.Launch();
    }
    public void DoCat_Swipe()
    {
        if (Cat_Swipe.KnownSpell &&
            Cat_Swipe.IsSpellUsable &&
            Cat_Swipe.IsDistanceGood)
            Cat_Swipe.Launch();
    }
    public void DoThrash()
    {
        if (Thrash.KnownSpell &&
            Thrash.IsSpellUsable &&
            Thrash.IsDistanceGood)
            Thrash.Launch();
    }
    public void DoTigers_Fury()
    {
        if (Tigers_Fury.KnownSpell &&
            Tigers_Fury.IsSpellUsable &&
            !Tigers_Fury.HaveBuff &&
            (DistanceToTarget() <= 10.0f))
            Tigers_Fury.Launch();
    }
    public void DoTravel_Form()
    {
        if (Travel_Form.KnownSpell &&
            Travel_Form.IsSpellUsable)
            Travel_Form.Launch();
    }
    #endregion DruidFeralSpells

    #region DruidRestoSpells
    public void DoMark_Of_The_Wild()
    {
        if (!Mark_of_the_Wild.HaveBuff &&
            Mark_of_the_Wild.IsSpellUsable &&
            Mark_of_the_Wild.KnownSpell)
            Mark_of_the_Wild.Launch();
    }
    public void DoNourish()
    {
    }
    public void DoRebirth()
    {
    }
    public void DoRemove_Corruption()
    {
    }
    public void DoRevive()
    {
    }
    public bool DoTranquility()
    {
        if (Tranquility.KnownSpell &&
            Tranquility.IsSpellUsable)
        {
            Fight.StopFight();
            MovementManager.StopMove();
            Healing_Touch.Launch();
            Thread.Sleep(5000);
            return true;
        }
        return false;
    }

    public bool DoHealing_Touch()
    {
        if (Healing_Touch.KnownSpell &&
            Healing_Touch.IsSpellUsable &&
            Healing_Touch.IsDistanceGood)
        {
            Fight.StopFight();
            MovementManager.StopMove();
            Healing_Touch.Launch();
            Thread.Sleep(2800);
            return true;
        }
        return false;
    }

    public bool DoRegrowth()
    {
        if (Me.HealthPercent <= 95 &&
            !Regrowth.HaveBuff &&
             Regrowth.KnownSpell &&
             Regrowth.IsSpellUsable)
        {
            Fight.StopFight();
            MovementManager.StopMove();
            Regrowth.Launch();
            return true;
        }
        return false;
    }

    public bool DoLifebloom()
    {
        if (Lifebloom.KnownSpell &&
             Lifebloom.IsSpellUsable &&
             Lifebloom.IsDistanceGood &&
             (!Lifebloom.HaveBuff || Lifebloom.BuffStack < 3))
        {
            Fight.StopFight();
            MovementManager.StopMove();
            Lifebloom.Launch();
            return true;
        }
        return false;
    }

    public bool DoRejuvenation()
    {
        if (!Rejuvenation.HaveBuff && Rejuvenation.KnownSpell && Rejuvenation.IsSpellUsable &&
                !Lifebloom.KnownSpell)
        {
            Fight.StopFight();
            MovementManager.StopMove();
            Rejuvenation.Launch();
            Thread.Sleep(1400);
            return true;
        }
        return false;
    }

    #endregion DruidRestoSpells



    #region SpecialMoves

    public void DoAllways()
    {
        Charge();
        Interupt();
    }

    public void Charge()
    {
        CheckForm();
        if (Cat_Form.HaveBuff)
            DoCat_Charge();
        if (Bear_Form.HaveBuff)
            DoBear_Charge();
    }

    public void Interupt()
    {
        if (Target.IsCast)
            DoCat_Skull_Bash();
    }

    public void Buff()
    {
        DoMark_Of_The_Wild();
    }

    public bool Heal()
    {
        if (Me.HealthPercent < CriticalHealth && Tranquility.IsSpellUsable)
            return DoTranquility();
        if (Me.HealthPercent < LowHealth)
            return DoHealing_Touch();
        if (Me.HealthPercent < Injured)
            return DoRegrowth();

        if (Me.HealthPercent < Injured)
        {
            if (DoLifebloom())
                return true;
            if (DoRejuvenation())
                return true;
        }
        return false;
    }

    public void Mana()
    {
        if (!Cat_Form.HaveBuff && !Bear_Form.HaveBuff && Me.BarTwoPercentage < 50.0f)
            DoInnervate();
    }

    public void CheckForm()
    {
        if (!ObjectManager.Me.IsMounted && !ObjectManager.Me.IsDeadMe)
        {
            if (!Cat_Form.HaveBuff)
            {
                Cat_Form.Launch();
            }
        }
    }

    public void CheckUseItems()
    {
        if (HaveItem("Strange Bloated Stomach"))
            Macro("/use Strange Bloated Stomach");
    }

    public void FollowFocus()
    {
        Macro("/follow focus");
    }

    public void AssistFocus()
    {
        Macro("/assist focus");
    }

    #endregion SpecialMoves



    #region Shortcuts

    /// <summary>
    /// Checks if you're mounted
    /// </summary>
    /// <returns>True if mounted</returns>
    public bool Mounted()
    {
        return ObjectManager.Me.IsMounted;
    }

    /// <summary>
    /// Checks if you're in combat
    /// </summary>
    /// <returns>True if in combat</returns>
    public bool InCombat()
    {
        return ObjectManager.Me.InCombat;
        //This is for if you are initiating a fight. Aka have an attack qued for the target.
        //return WowManager.WoW.PlayerManager.Fight.InFight;
    }

    /// <summary>
    /// Checks if you're initating a fight(!?)
    /// </summary>
    /// <returns></returns>
    public bool InFight()
    {
        return Fight.InFight;
    }

    /// <summary>
    /// Checks if you have a target
    /// </summary>
    /// <returns>True if you have a target</returns>
    public bool HaveTarget()
    {
        return ObjectManager.Me.Target > 0;
    }

    /// <summary>
    /// Checks if "a" is your target
    /// </summary>
    /// <param name="a">unitidentifyer as UInt64</param>
    /// <returns>True if the unit is your target</returns>
    public bool IsTarget(UInt64 a)
    {
        return ObjectManager.Me.Target == a;
    }

    /// <summary>
    /// Gets the distance to your target in yards (float)
    /// </summary>
    /// <returns>Distance to your target in yards</returns>
    public float DistanceToTarget()
    {
        return ObjectManager.Target.GetDistance;
    }

    /// <summary>
    /// Does command in wow, stated in the param
    /// </summary>
    /// <param name="s">The text command to do inGame</param>
    public void Macro(String s)
    {
        Lua.RunMacroText(s);
    }

    /// <summary>
    /// Checks if the item is in players inventory
    /// </summary>
    /// <param name="s">The item to check for (string)</param>
    /// <returns>True if item is in players inventory</returns>
    public bool HaveItem(String s)
    {
        return ItemsManager.GetItemCountByNameLUA(s) > 0;
    }

    /// <summary>
    /// Checks to see if player has focused a target
    /// </summary>
    /// <returns>True if player has focustarget</returns>
    public bool HaveFocus()
    {
        Fight.StopFight();
        Macro("/cleartarget");
        Thread.Sleep(Usefuls.Latency + 200);
        Macro("/target focus");
        Thread.Sleep(Usefuls.Latency + 1000);
        Fight.StartFight();
        return Me.Target != 0;
    }

    /// <summary>
    /// Write specified text to the BotLog
    /// </summary>
    /// <param name="s">The test you wish to </param>
    public void Log(String s)
    {
        Logging.WriteFight(s);
    }

    #endregion Shortcuts
}


#endregion CustomClass