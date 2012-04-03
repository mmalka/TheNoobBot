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
            case WoWClass.Druid:

                Spell Mangle = new Spell("Mangle");
                Spell Starsurge = new Spell("Starsurge");

                if (Mangle.KnownSpell)
                {
                    DruidFeral ccDRF = new DruidFeral();
                }

                if (Starsurge.KnownSpell)
                {
                    DruidBalance ccDRB = new DruidBalance();
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

public class DruidFeral
{
    #region InitializeSpell

    Spell Faerie_Fire_Feral = new Spell("Faerie Fire (Feral)");
    Spell Ferocious_Bite = new Spell("Ferocious Bite");
    Spell Rip = new Spell("Rip");
    Spell ClawBashMaul = new Spell("ClawBashMaul");
    Spell Mark_of_the_Wild = new Spell("Mark of the Wild");
    Spell Thorns = new Spell("Thorns");
    Spell Cat_Form = new Spell("Cat Form");
    Spell Bear_Form = new Spell("Bear Form");
    Spell Dire_Bear_Form = new Spell("Dire Bear Form");
    Spell Maul = new Spell("Maul");
    Spell Bash = new Spell("Bash");
    Spell Claw = new Spell("Claw");
    Spell Prowl = new Spell("Prowl");
    Spell Feral_Charge = new Spell("Feral Charge");
    Spell Ravage = new Spell("Ravage");
    Spell Rake = new Spell("Rake");
    Spell Mangle = new Spell("Mangle");
    Spell Skull_Bash = new Spell("Skull Bash");

    Spell Regrowth = new Spell("Regrowth");
    Spell Lifebloom = new Spell("Lifebloom");
    Spell Wild_Growth = new Spell("Wild Growth");
    Spell Nourish = new Spell("Nourish");
    Spell Healing_Touch = new Spell("Healing Touch");
    Spell Rejuvenation = new Spell("Rejuvenation");

    #endregion InitializeSpell

    public DruidFeral()
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
                    if (ObjectManager.Me.Target != lastTarget && ObjectManager.Target.GetDistance <= 30.0f)
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
        if (Prowl.KnownSpell && Prowl.IsSpellUsable)
        {
            Prowl.Launch();
        }
    }

    public void Patrolling()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        Heal();
        Buff();
    }

    public void Combat()
    {
        CheckForm();
        Heal();
        Charge();

        if (ObjectManager.Me.ComboPoint >= 3 &&
            Ferocious_Bite.KnownSpell && Ferocious_Bite.IsDistanceGood && Ferocious_Bite.IsSpellUsable)
        {
            Ferocious_Bite.Launch();
            return;
        }

        if (ObjectManager.Me.ComboPoint >= 1 &&
            !ObjectManager.Target.HaveBuff(91565) &&
            Faerie_Fire_Feral.KnownSpell && Faerie_Fire_Feral.IsSpellUsable && Faerie_Fire_Feral.IsDistanceGood)
        {
            Faerie_Fire_Feral.Launch();
        }

        if (Ravage.KnownSpell && Ravage.IsDistanceGood && Ravage.IsSpellUsable)
        {
            Ravage.Launch();
            return;
        }

        if (!Rake.TargetHaveBuff && Rake.KnownSpell && Rake.IsDistanceGood && Rake.IsSpellUsable)
        {
            Rake.Launch();
        }

        if (Mangle.IsDistanceGood)
        {
            SpellManager.LaunchSpellById(33876);
            Logging.WriteFight("cast Mangle");
            return;
        }

        if (!Bear_Form.KnownSpell && Claw.KnownSpell && Claw.IsDistanceGood && Claw.IsSpellUsable)
        {
            Claw.Launch();
        }
    }

    private void Charge()
    {
        if (ObjectManager.Target.GetDistance > 8 &&
            ObjectManager.Target.GetDistance < 25 &&
            Feral_Charge.KnownSpell && Feral_Charge.IsSpellUsable)
        {
            Feral_Charge.Launch();
        }
    }

    public void Decast()
    {
        if (ObjectManager.Target.IsCast &&
            Skull_Bash.KnownSpell && Skull_Bash.IsDistanceGood && Skull_Bash.IsSpellUsable)
        {
            Skull_Bash.Launch();
        }
    }

    public void Buff()
    {
        if (!ObjectManager.Me.HaveBuff(79061) &&
            Mark_of_the_Wild.KnownSpell && Mark_of_the_Wild.IsSpellUsable)
        {
            Mark_of_the_Wild.Launch();
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.HealthPercent <= 20 &&
            Healing_Touch.KnownSpell && Healing_Touch.IsSpellUsable)
        {
            Healing_Touch.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent <= 35 &&
            !Regrowth.HaveBuff && Regrowth.KnownSpell && Regrowth.IsSpellUsable)
        {
            Regrowth.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent <= 55)
        {
            if (!Lifebloom.HaveBuff && Lifebloom.KnownSpell && Lifebloom.IsSpellUsable)
            {
                Lifebloom.Launch();
                return;
            }

            if (!Rejuvenation.HaveBuff && Rejuvenation.KnownSpell && Rejuvenation.IsSpellUsable &&
                !Lifebloom.KnownSpell)
            {
                Rejuvenation.Launch();
                return;
            }
        }
    }

    private void CheckForm()
    {
        if (!ObjectManager.Me.IsMounted && !ObjectManager.Me.IsDeadMe)
        {
            if (Cat_Form.KnownSpell)
            {
                if (!Cat_Form.HaveBuff)
                {
                    Cat_Form.Launch();
                }
            }
        }
    }
}

public class DruidBalance
{
    #region InitializeSpell

    Spell Starsurge = new Spell("Starsurge");
    Spell Starfire = new Spell("Starfire");
    Spell Insect_Swarm = new Spell("Insect Swarm");
    Spell Earth_and_Moon = new Spell("Earth and Moon");
    Spell Moonfire = new Spell("Moonfire");
    Spell Sunfire = new Spell("Sunfire");
    Spell Barkskin = new Spell("Barkskin");
    Spell Natures_Grasp = new Spell("Nature's Grasp");
    Spell Thorns = new Spell("Thorns");
    Spell Moonkin_Form = new Spell("Moonkin Form");
    Spell Force_of_Nature = new Spell("Force of Nature");
    Spell Faerie_Fire = new Spell("Faerie Fire");
    Spell Sarments = new Spell("Sarments");
    Spell Innervate = new Spell("Innervate");
    Spell Mark_of_the_Wild = new Spell("Mark of the Wild");
    Spell Solar_Beam = new Spell("Solar Beam");
    Spell Typhoon = new Spell("Typhoon");
    Spell Starfall = new Spell("Starfall");
    Spell Wrath = new Spell("Wrath");

    Spell Regrowth = new Spell("Regrowth");
    Spell Lifebloom = new Spell("Lifebloom");
    Spell Wild_Growth = new Spell("Wild Growth");
    Spell Nourish = new Spell("Nourish");
    Spell Healing_Touch = new Spell("Healing Touch");
    Spell Rejuvenation = new Spell("Rejuvenation");

    #endregion InitializeSpell

    public DruidBalance()
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
                    if (ObjectManager.Me.Target != lastTarget && ObjectManager.Target.GetDistance <= 30.0f)
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
        if (Faerie_Fire.TargetHaveBuff &&
            Faerie_Fire.KnownSpell && Faerie_Fire.IsDistanceGood && Faerie_Fire.IsSpellUsable)
        {
            Faerie_Fire.Launch();
        }
    }

    public void Patrolling()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        Heal();
        Buff();
    }

    public void Combat()
    {
        CheckForm();
        Decast();
        Heal();
        Buff();
        BuffCombat();

        if (ObjectManager.Me.HaveBuff(48518) &&
            Starfall.KnownSpell && Starfall.IsSpellUsable)
        {
            Starfall.Launch();
        }

        if (!Insect_Swarm.TargetHaveBuff &&
            Insect_Swarm.KnownSpell && Insect_Swarm.IsDistanceGood && Insect_Swarm.IsSpellUsable)
        {
            if (Earth_and_Moon.TargetHaveBuff && Earth_and_Moon.KnownSpell)
            {
                Insect_Swarm.Launch();
                return;
            }

            else if (!Earth_and_Moon.KnownSpell)
            {
                Insect_Swarm.Launch();
                return;
            }
        }

        if ((!Moonfire.TargetHaveBuff && Moonfire.KnownSpell && Moonfire.IsDistanceGood && Moonfire.IsSpellUsable) ||
            (!Sunfire.TargetHaveBuff && Sunfire.KnownSpell && Sunfire.IsDistanceGood && Sunfire.IsSpellUsable))
        {
            if (Earth_and_Moon.TargetHaveBuff && Earth_and_Moon.KnownSpell)
            {
                Moonfire.Launch();
                return;
            }
        }

        if (Starsurge.KnownSpell && Starsurge.IsDistanceGood && Starsurge.IsSpellUsable)
        {
            Starsurge.Launch();
        }

        if (//ObjectManager.Me.HaveBuff(48518) && //Eclipse Lunaire
            Starfire.KnownSpell && Starfire.IsSpellUsable && Starfire.IsDistanceGood)
        {
            Starfire.Launch();
        }

        if (//ObjectManager.Me.HaveBuff(48517) && //Eclipse Solaire
            Wrath.KnownSpell && Wrath.IsSpellUsable && Wrath.IsDistanceGood)
        {
            Wrath.Launch();
        }

        if (ObjectManager.Target.GetDistance <= 30 &&
            Typhoon.KnownSpell && Typhoon.IsSpellUsable)
        {
            Typhoon.Launch();
        }
    }

    public void Decast()
    {
        if (ObjectManager.Target.IsCast &&
            Solar_Beam.KnownSpell && Solar_Beam.IsSpellUsable && Solar_Beam.IsDistanceGood)
        {
            Solar_Beam.Launch();
        }
    }

    public void Buff()
    {
        if (!ObjectManager.Me.HaveBuff(79061) &&
            Mark_of_the_Wild.KnownSpell && Mark_of_the_Wild.IsSpellUsable)
        {
            Mark_of_the_Wild.Launch();
        }
    }

    public void BuffCombat()
    {
        if (ObjectManager.Me.HealthPercent <= 75 &&
            Barkskin.KnownSpell && Barkskin.IsSpellUsable)
        {
            Barkskin.Launch();
        }

        if (Natures_Grasp.KnownSpell && Natures_Grasp.IsSpellUsable)
        {
            Natures_Grasp.Launch();
        }

        if (Thorns.KnownSpell && Thorns.IsSpellUsable)
        {
            Thorns.Launch();
        }

        if (Force_of_Nature.KnownSpell && Force_of_Nature.IsSpellUsable && Force_of_Nature.IsDistanceGood)
        {
            SpellManager.CastSpellByIDAndPosition(33831, ObjectManager.Target.Position);
        }

        if (!Sarments.TargetHaveBuff &&
            (ObjectManager.Target.GetDistance >= 5 || ObjectManager.Target.GetMove) &&
            Sarments.KnownSpell && Sarments.IsSpellUsable && Sarments.IsDistanceGood)
        {
            Sarments.Launch();
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.BarTwoPercentage < 50 &&
            Innervate.KnownSpell && Innervate.IsSpellUsable)
        {
            Innervate.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent <= 20 &&
            Healing_Touch.KnownSpell && Healing_Touch.IsSpellUsable)
        {
            Healing_Touch.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent <= 35 &&
            !Regrowth.HaveBuff && Regrowth.KnownSpell && Regrowth.IsSpellUsable)
        {
            Regrowth.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent <= 55)
        {
            if (!Lifebloom.HaveBuff && Lifebloom.KnownSpell && Lifebloom.IsSpellUsable)
            {
                Lifebloom.Launch();
                return;
            }

            if (!Rejuvenation.HaveBuff && Rejuvenation.KnownSpell && Rejuvenation.IsSpellUsable &&
                !Lifebloom.KnownSpell)
            {
                Rejuvenation.Launch();
                return;
            }
        }
    }

    private void CheckForm()
    {
        if (!ObjectManager.Me.IsMounted && !ObjectManager.Me.IsDeadMe)
        {
            if (Moonkin_Form.KnownSpell)
            {
                if (!Moonkin_Form.HaveBuff)
                {
                    Moonkin_Form.Launch();
                }
            }
        }
    }
}

#endregion CustomClass
