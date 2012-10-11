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
                    #region Druid Specialisation checking

                case WoWClass.Druid:
                    var Savage_Roar = new Spell("Savage Roar");
                    var Tigers_Fury = new Spell("Tiger's Fury");
                    if (Savage_Roar.KnownSpell || Tigers_Fury.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            MessageBox.Show("There is no settings available for your Class/Specialisation.");
                        }
                        else
                        {
                            Logging.WriteFight("Feral Druid Found");
                            new DruidFeral();
                        }
                        break;
                    }
                    var Starsurge = new Spell("Starsurge");
                    if (Starsurge.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            MessageBox.Show("There is no settings available for your Class/Specialisation.");
                        }
                        else
                        {
                            Logging.WriteFight("Balance Druid Found");
                            new Balance();
                        }
                        break;
                    }
                    if (!Starsurge.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            MessageBox.Show("There is no settings available for your Class/Specialisation.");
                        }
                        else
                        {
                            Logging.WriteFight("Druid without Spec");
                            new Balance();
                        }
                        break;
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

#region Druid

public class Balance
{
    private Int32 firemode;

    #region InitializeSpell

    // BALANCE ONLY
    private Spell Starfall = new Spell("Starfall");
    private Spell Typhoon = new Spell("Typhoon");
    private Spell Moonkin_Form = new Spell("Moonkin Form");
    private Spell Force_of_Nature = new Spell("Force of Nature");
    private Spell Solar_Beam = new Spell("Solar Beam");
    private Spell Starsurge = new Spell("Starsurge");
    private Spell Sunfire = new Spell("Sunfire");

    // DPS
    private Spell Insect_Swarm = new Spell("Insect Swarm");
    private Spell Starfire = new Spell("Starfire");
    private Spell Moonfire = new Spell("Moonfire");
    private Spell Wrath = new Spell("Wrath");

    // HEAL
    private Spell Regrowth = new Spell("Regrowth");
    private Spell Rejuvenation = new Spell("Rejuvenation");
    private Spell Nourish = new Spell("Nourish");
    private Spell Lifebloom = new Spell("Lifebloom");
    private Spell Healing_Touch = new Spell("Healing Touch");

    // BUFF & HELPING
    private Spell Innervate = new Spell("Innervate");
    private Spell Mark_of_the_Wild = new Spell("Mark of the Wild");
    private Spell Barkskin = new Spell("Barkskin");

    // TIMER
    private Timer look = new Timer(0);
    private Timer fighttimer = new Timer(0);
    private Timer slowbloom = new Timer(0);

    // profession & racials
    private Spell ArcaneTorrent = new Spell("Arcane Torrent");
    private Spell Lifeblood = new Spell("Lifeblood");
    private Spell Stoneform = new Spell("Stoneform");
    private Spell Tailoring = new Spell("Tailoring");
    private Spell Leatherworking = new Spell("Leatherworking");
    private Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    private Spell War_Stomp = new Spell("War Stomp");
    private Spell Berserking = new Spell("Berserking");

    #endregion InitializeSpell

    public Balance()
    {
        Main.range = 30.0f;
        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            if (!ObjectManager.Me.IsMounted)
            {
                buffoutfight();

                if (!Fight.InFight && look.IsReady)
                {
                    look = new Timer(5000);
                    Lua.RunMacroText("/targetfriendplayer");
                }

                if (Fight.InFight && ObjectManager.Me.Target > 0 && ObjectManager.Target.GetDistance > Main.range)
                {
                    fighttimer = new Timer(60000);
                }

                if (Fight.InFight && ObjectManager.Me.Target > 0)
                {
                    if (ObjectManager.Me.Target != lastTarget && ObjectManager.Target.GetDistance <= Main.range)
                    {
                        pull();
                        lastTarget = ObjectManager.Me.Target;
                    }
                    fight();
                    if (!Fight.InFight)
                    {
                        Logging.WriteFight(" - Target Down - ");
                        look = new Timer(5000);
                    }

                    if (fighttimer.IsReady && ObjectManager.Target.HealthPercent > 90 && ObjectManager.Me.Target > 0)
                    {
                        Logging.WriteFight(" - Target Evading - ");
                        break;
                    }
                }
            }
            Thread.Sleep(350);
        }
    }

    public void pull()
    {
        if (hardmob()) Logging.WriteFight(" -  Pull Hard Mob - ");
        if (!hardmob()) Logging.WriteFight(" -  Pull Easy Mob - ");
        fighttimer = new Timer(60000);
    }

    public void buffoutfight()
    {
        if (Fight.InFight || ObjectManager.Me.IsDeadMe) return;

        if (!ObjectManager.Me.HaveBuff(79640) &&
            ItemsManager.GetItemCountByIdLUA(58149) == 1)
        {
            Logging.WriteFight("Use Alchi Flask");
            Lua.RunMacroText("/use item:58149");
        }

        if (Mark_of_the_Wild.KnownSpell && Mark_of_the_Wild.IsSpellUsable && !Mark_of_the_Wild.HaveBuff)
        {
            Mark_of_the_Wild.Launch();
        }
    }

    public void fight()
    {
        selfheal();
        buffinfight();
        if (ObjectManager.GetNumberAttackPlayer() > 1) fighttimer = new Timer(60000);

        if (!ObjectManager.Target.IsTargetingMe && Wrath.IsDistanceGood)
        {
            if (wrathspam())
            {
                SpellManager.CastSpellByIdLUA(5176);
                // Wrath.Launch();
            }

            if (starfirespam())
            {
                SpellManager.CastSpellByIdLUA(2912);
                // Starfire.Launch();
            }
            return;
        }

        if (ObjectManager.Me.HaveBuff(93400) && Starsurge.IsDistanceGood && Starsurge.IsSpellUsable &&
            ObjectManager.Target.IsTargetingMe)
        {
            SpellManager.CastSpellByIdLUA(78674);
            // Starsurge.Launch();
        }

        if (Moonfire.KnownSpell && Moonfire.IsDistanceGood && Moonfire.IsSpellUsable && !Moonfire.TargetHaveBuff &&
            starfirespam())
        {
            SpellManager.CastSpellByIdLUA(8921);
            // Moonfire.Launch();
        }

        if (wrathspam() && !ObjectManager.Target.HaveBuff(93402) && !Moonfire.TargetHaveBuff)
        {
            if (Sunfire.KnownSpell && Sunfire.IsDistanceGood && Sunfire.IsSpellUsable)
            {
                SpellManager.CastSpellByIdLUA(93402);
                // Sunfire.Launch();
                return;
            }
            if (Moonfire.KnownSpell && Moonfire.IsDistanceGood && Moonfire.IsSpellUsable)
            {
                SpellManager.CastSpellByIdLUA(8921);
                // Moonfire.Launch();
            }
        }

        if (Starsurge.KnownSpell && Starsurge.IsDistanceGood && Starsurge.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(78674);
            // Starsurge.Launch();
        }

        if (Insect_Swarm.KnownSpell && Insect_Swarm.IsDistanceGood && Insect_Swarm.IsSpellUsable &&
            !Insect_Swarm.TargetHaveBuff && hardmob())
        {
            SpellManager.CastSpellByIdLUA(5570);
            // Insect_Swarm.Launch();
        }

        if (Wrath.KnownSpell && Wrath.IsSpellUsable && Wrath.IsDistanceGood && wrathspam() &&
            (ObjectManager.Target.HaveBuff(93402) || Moonfire.TargetHaveBuff))
        {
            SpellManager.CastSpellByIdLUA(5176);
            // Wrath.Launch();
        }

        if (Starfire.KnownSpell && Starfire.IsSpellUsable && Starfire.IsDistanceGood && starfirespam() &&
            (ObjectManager.Target.HaveBuff(93402) || Moonfire.TargetHaveBuff))
        {
            SpellManager.CastSpellByIdLUA(2912);
            // Starfire.Launch();
        }

        if (Berserking.KnownSpell && Berserking.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(1454);
            // Berserking.Launch();
        }

        if (Force_of_Nature.KnownSpell && Force_of_Nature.IsSpellUsable && Force_of_Nature.IsDistanceGood &&
            (hardmob() || ObjectManager.GetNumberAttackPlayer() > 1))
        {
            SpellManager.CastSpellByIDAndPosition(33831, ObjectManager.Target.Position);
        }

        if (Typhoon.KnownSpell && Typhoon.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() > 1 &&
            ObjectManager.Target.GetDistance < 40)
        {
            SpellManager.CastSpellByIdLUA(50516);
            // Typhoon.Launch();
        }

        if (Starfall.KnownSpell && Starfall.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() > 1 &&
            ObjectManager.Target.GetDistance < 40)
        {
            SpellManager.CastSpellByIdLUA(48505);
            // Starfall.Launch();
        }
    }

    private void buffinfight()
    {
        if ((ObjectManager.GetNumberAttackPlayer() > 1 || hardmob()) &&
            ObjectManager.Me.HealthPercent < 65 &&
            ObjectManager.Target.GetDistance < 5 &&
            Stoneform.KnownSpell && Stoneform.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(20594);
            // Stoneform.Launch();
        }

        if ((ObjectManager.GetNumberAttackPlayer() > 1 || hardmob()) &&
            ObjectManager.Me.HealthPercent < 65 &&
            ObjectManager.Target.GetDistance < 5 &&
            War_Stomp.KnownSpell && War_Stomp.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(20549);
            // War_Stomp.Launch();
        }

        if (Innervate.KnownSpell && Innervate.IsSpellUsable && ObjectManager.Me.ManaPercentage < 40)
        {
            SpellManager.CastSpellByIdLUA(29166);
            // Innervate.Launch();
        }

        if (Moonkin_Form.KnownSpell && !Moonkin_Form.HaveBuff && ObjectManager.Me.HealthPercent > 60)
        {
            SpellManager.CastSpellByIdLUA(24858);
            // Moonkin_Form.Launch();
        }
    }

    private void selfheal()
    {
        if (ObjectManager.Me.HealthPercent < 80 &&
            Lifeblood.KnownSpell && Lifeblood.IsSpellUsable)
        {
            Lifeblood.Launch();
        }

        if (ObjectManager.Me.HealthPercent < 80 &&
            Gift_of_the_Naaru.KnownSpell && Gift_of_the_Naaru.IsSpellUsable)
        {
            Gift_of_the_Naaru.Launch();
        }

        if (ArcaneTorrent.KnownSpell && ArcaneTorrent.IsSpellUsable &&
            ObjectManager.Target.IsCast && ObjectManager.Target.GetDistance < 8)
        {
            ArcaneTorrent.Launch();
        }

        if (Solar_Beam.KnownSpell && Solar_Beam.IsSpellUsable && Solar_Beam.IsDistanceGood &&
            ObjectManager.Target.IsCast)
        {
            SpellManager.CastSpellByIdLUA(78675);
            // Solar_Beam.Launch();
        }

        if (ObjectManager.Me.HealthPercent < 60 && ObjectManager.Me.ManaPercentage < 25 && Regrowth.KnownSpell &&
            Regrowth.IsSpellUsable && !Regrowth.HaveBuff)
        {
            if (Barkskin.KnownSpell && Barkskin.IsSpellUsable) Barkskin.Launch();
            SpellManager.CastSpellByIdLUA(8936);
            // Regrowth.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 50 && ObjectManager.Me.ManaPercentage > 25)
        {
            if (Barkskin.KnownSpell && Barkskin.IsSpellUsable) Barkskin.Launch();
            while (ObjectManager.Me.HealthPercent < 70)
            {
                if (Rejuvenation.KnownSpell && Rejuvenation.IsSpellUsable && !Rejuvenation.HaveBuff)
                {
                    SpellManager.CastSpellByIdLUA(774);
                    // Rejuvenation.Launch();
                }

                if (Regrowth.KnownSpell && Regrowth.IsSpellUsable && !Regrowth.HaveBuff)
                {
                    SpellManager.CastSpellByIdLUA(8936);
                    // Regrowth.Launch();
                }

                if (Regrowth.HaveBuff && Regrowth.KnownSpell && Rejuvenation.HaveBuff && Rejuvenation.KnownSpell &&
                    Nourish.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() < 2)
                {
                    SpellManager.CastSpellByIdLUA(50464);
                    // Nourish.Launch();
                }

                if (Regrowth.HaveBuff && Regrowth.KnownSpell && Rejuvenation.HaveBuff && Rejuvenation.KnownSpell &&
                    Healing_Touch.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() > 1)
                {
                    SpellManager.CastSpellByIdLUA(5185);
                    // Healing_Touch.Launch();
                }

                if (!Regrowth.KnownSpell && Nourish.KnownSpell)
                {
                    SpellManager.CastSpellByIdLUA(50464);
                    // Nourish.Launch();
                }

                if (Lifebloom.KnownSpell && Lifebloom.IsSpellUsable && slowbloom.IsReady)
                {
                    slowbloom = new Timer(4000);
                    SpellManager.CastSpellByIdLUA(33763);
                    // Lifebloom.Launch();
                }
                if (ObjectManager.Me.ManaPercentage < 10) return;
            }
        }
    }

    public bool wrathspam()
    {
        if (ObjectManager.Me.HaveBuff(48517))
        {
            firemode = 1;
        }

        if (ObjectManager.Me.HaveBuff(48518))
        {
            firemode = 2;
        }
        if (firemode == 0 || firemode == 1)
            return true;
        return false;
    }

    public bool starfirespam()
    {
        if (ObjectManager.Me.HaveBuff(48517))
        {
            firemode = 1;
        }

        if (ObjectManager.Me.HaveBuff(48518))
        {
            firemode = 2;
        }
        if (firemode == 2)
            return true;
        return false;
    }

    public bool hardmob()
    {
        if (((ObjectManager.Target.MaxHealth*100)/ObjectManager.Me.MaxHealth) > 110)
        {
            return true;
        }
        return false;
    }
}

public class DruidFeral
{
    private float PullRange;
    private UInt64 lastTarget;
    private UInt64 Injured;
    private UInt64 LowHealth;
    private UInt64 CriticalHealth;

    private Int64 interupt_cost;

    private WoWPlayer Me = ObjectManager.Me;
    private WoWUnit Target = ObjectManager.Target;
    private bool haveFocus;

    #region InitializeSpell

    private Spell Aquatic_Form = new Spell("Aquatic Form");
    private Spell Barkskin = new Spell("Barkskin");
    private Spell Bear_Form = new Spell("Bear Form");
    private Spell Berserk = new Spell("Berserk");
    private Spell Cat_Form = new Spell("Cat Form");
    private Spell Cenarion_Ward = new Spell("Cenarion Ward");
    private Spell Cyclone = new Spell("Cyclone");
    private Spell Dash = new Spell("Dash");
    private Spell Displacer_Beast = new Spell("Displacer Beast");
    private Spell Disorienting_Roar = new Spell("Disorienting Roar");
    private Spell Entangling_Roots = new Spell("Entangling Roots");
    private Spell Faerie_Fire = new Spell("Faerie Fire");
    private Spell Ferocious_Bite = new Spell("Ferocious Bite");
    private Spell Flight_Form = new Spell("Flight Form");
    private Spell Force_of_Nature = new Spell("Force of Nature");
    private Spell Frenzied_Regeneration = new Spell("Frenzied Regeneration");
    private Spell Growl = new Spell("Growl");
    private Spell Healing_Touch = new Spell("Healing Touch");
    private Spell Hibernate = new Spell("Hibernate");
    private Spell Hurricane = new Spell("Hurricane");
    private Spell Incarnation = new Spell("Incarnation");
    private Spell Innervate = new Spell("Innervate");
    private Spell Lacerate = new Spell("Lacerate");
    private Spell Maim = new Spell("Maim");
    private Spell Mangle = new Spell("Mangle");
    private Spell Mark_of_the_Wild = new Spell("Mark of the Wild");
    private Spell Maul = new Spell("Maul");
    private Spell Might_of_Ursoc = new Spell("Might of Ursoc");
    private Spell Mighty_Bash = new Spell("Mighty Bash");
    private Spell Moonfire = new Spell("Moonfire");
    private Spell Natures_Grasp = new Spell("Nature's Grasp");
    private Spell Natures_Swiftness = new Spell("Nature's Swiftness");
    private Spell Pounce = new Spell("Pounce");
    private Spell Prowl = new Spell("Prowl");
    private Spell Rake = new Spell("Rake");
    private Spell Ravage = new Spell("Ravage");
    private Spell Rebirth = new Spell("Rebirth");
    private Spell Rejuvenation = new Spell("Rejuvenation");
    private Spell Remove_Corruption = new Spell("Remove Corruption");
    private Spell Renewal = new Spell("Renewal");
    private Spell Revive = new Spell("Revive");
    private Spell Rip = new Spell("Rip");
    private Spell Savage_Roar = new Spell("Savage Roar");
    private Spell Shred = new Spell("Shred");
    private Spell Skull_Bash = new Spell("Skull Bash");
    private Spell Soothe = new Spell("Soothe");
    private Spell Stampeding_Roar = new Spell("Stampeding Roar");
    private Spell Survival_Instincts = new Spell("Survival Instincts");
    private Spell Swift_Flight_Form = new Spell("Swift Flight Form");
    private Spell Swipe = new Spell("Swipe");
    private Spell Teleport_Moonglade = new Spell("Teleport: Moonglade");
    private Spell Thrash = new Spell("Thrash");
    private Spell Tigers_Fury = new Spell("Tiger's Fury");
    private Spell Track_Humanoids = new Spell("Track Humanoids");
    private Spell Tranquility = new Spell("Tranquility");
    private Spell Travel_Form = new Spell("Travel Form");
    private Spell Typhoon = new Spell("Typhoon");
    private Spell Ursols_Vortex = new Spell("Ursol's Vortex");
    private Spell Wild_Charge = new Spell("Wild Charge");
    private Spell Wrath = new Spell("Wrath");

    private Timer Savage_Roar_Timer = new Timer(0);
    private Timer Rake_Timer = new Timer(0);
    private Timer Rip_Timer = new Timer(0);
    private Timer Rejuvenation_Timer = new Timer(0);
    private Timer Healing_Touch_Timer = new Timer(0);

    int StartFight = 1;

    // Profession & Racials
    private Spell Arcane_Torrent = new Spell("Arcane Torrent");
    private Spell Berserking = new Spell("Berserking");
    private Spell Blood_Fury = new Spell("Blood Fury");
    private Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    private Spell Lifeblood = new Spell("Lifeblood");
    private Spell Stoneform = new Spell("Stoneform");

    int CP = 0;
    bool FivePtRip = false;
    bool FivePtSav = false;
    bool FivePtFer = false;

    #endregion InitializeSpell

    //private DruidConfig df = new DruidConfig(); //NYI

    public DruidFeral()
    {
        Main.range = 3.6f; //Main ability range
        PullRange = 24.0f; //Pull spell range
        lastTarget = 0;

        Injured = 90;
        LowHealth = 50;
        CriticalHealth = 30;

        //std = new Thread(new ThreadStart(DoAllways)); //NYI
        //std.Start();                                  //NYI


        haveFocus = false; //HaveFocus();//NYI waiting for a working Config CC

        while (Main.loop)
        {
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
            Thread.Sleep(350);
        }
    }

    public void Patrolling()
    {
        /*if (!ObjectManager.Me.IsMounted || !ObjectManager.Me.InCombat || !Fight.InFight)
        {
            if (ObjectManager.Me.HealthPercent < 95 && ObjectManager.Me.ManaPercentage > 33 && Healing_Touch.IsSpellUsable 
                && Healing_Touch.KnownSpell)
            {
                Healing_Touch.Launch();
                while (ObjectManager.Me.IsCast)
                {
                    Thread.Sleep(200);
                }
            }*/
            Buff();
        //}
    }

    public void Buff()
    {
        if (Mark_of_the_Wild.KnownSpell && Mark_of_the_Wild.IsSpellUsable && !Mark_of_the_Wild.HaveBuff)
        {
            Mark_of_the_Wild.Launch();
            return;
        }
    }

    public void Pull()
    {
        if (!ObjectManager.Me.HaveBuff(768))
        {
            Cat_Form.Launch();
            return;
        }

        if (Wild_Charge.KnownSpell && Wild_Charge.IsSpellUsable && Wild_Charge.IsDistanceGood)
        {
            Mark_of_the_Wild.Launch();
        }
        else
        {
            if (Faerie_Fire.KnownSpell && Faerie_Fire.IsSpellUsable && Faerie_Fire.IsDistanceGood)
            {
                Faerie_Fire.Launch();
            }
        }
        StartFight--;
        return;
    }

    public void LowCombat()
    {
        AvoidMelee();
        Heal();
        Resistance();
        Buff();

        if (!ObjectManager.Me.HaveBuff(768))
        {
            Cat_Form.Launch();
            return;
        }

        if (Mangle.IsSpellUsable && Mangle.KnownSpell && Mangle.IsDistanceGood)
        {
            Mangle.Launch();
            if (ObjectManager.Target.HealthPercent < 50 && ObjectManager.Target.HealthPercent > 0)
            {
                Mangle.Launch();
                return;
            }
        }

        if (ObjectManager.Target.HealthPercent > 90)
        {
            if (Swipe.IsSpellUsable && Swipe.KnownSpell && Swipe.IsDistanceGood)
            {
                Swipe.Launch();
                return;
            }
        }
        return;
    }

    public void Combat()
    {
        AvoidMelee();
        Decast();
        Heal();
        Resistance();
        Buff();
        if (StartFight == 1)
            Pull();

        if (!ObjectManager.Me.HaveBuff(768))
        {
            Cat_Form.Launch();
            return;
        }

        if (Faerie_Fire.KnownSpell && Faerie_Fire.IsSpellUsable && Faerie_Fire.IsDistanceGood 
            && (!Faerie_Fire.TargetHaveBuff || !ObjectManager.Target.HaveBuff(113746)))
        {
            Faerie_Fire.Launch();
            return;
        }

        if (Tigers_Fury.KnownSpell && Tigers_Fury.IsSpellUsable && ObjectManager.Me.Energy < 35)
        {
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

            Logging.WriteFight("Use Trinket 1.");
            Lua.RunMacroText("/use 13");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Logging.WriteFight("Use Trinket 2.");
            Lua.RunMacroText("/use 14");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");

            Tigers_Fury.Launch();
            return;
        }

        if (Force_of_Nature.KnownSpell && Force_of_Nature.IsSpellUsable 
            && Force_of_Nature.IsDistanceGood)
        {
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
            Logging.WriteFight("Use Trinket 1.");
            Lua.RunMacroText("/use 13");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Logging.WriteFight("Use Trinket 2.");
            Lua.RunMacroText("/use 14");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");

            Force_of_Nature.Launch();
            return;
        }

        if (Berserk.KnownSpell && Berserk.IsSpellUsable)
        {
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
            Logging.WriteFight("Use Trinket 1.");
            Lua.RunMacroText("/use 13");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Logging.WriteFight("Use Trinket 2.");
            Lua.RunMacroText("/use 14");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");

            Berserk.Launch();
            return;
        }

        if (ObjectManager.GetNumberAttackPlayer() > 2 && Thrash.IsSpellUsable && Thrash.KnownSpell
            && Thrash.IsDistanceGood && !Thrash.TargetHaveBuff)
        {
            Thrash.Launch();
            return;
        }

        if (ObjectManager.GetNumberAttackPlayer() > 2 && Swipe.IsSpellUsable && Swipe.KnownSpell
            && Swipe.IsDistanceGood)
        {
            Swipe.Launch();
            return;
        }

        if (Savage_Roar.IsSpellUsable && Savage_Roar.KnownSpell && Savage_Roar.IsDistanceGood && !FivePtSav
            && ObjectManager.Me.ComboPoint == 5)
        {
            CP = ObjectManager.Me.ComboPoint;
            Savage_Roar.Launch();
            Savage_Roar_Timer = new Timer(1000 * (9 + (6 * CP)));
            FivePtSav = true;
            return;
        }

        if (Savage_Roar.IsSpellUsable && Savage_Roar.KnownSpell && Savage_Roar.IsDistanceGood
            && (!ObjectManager.Me.HaveBuff(127538) || Savage_Roar_Timer.IsReady))
        {
            CP = ObjectManager.Me.ComboPoint;
            Savage_Roar.Launch();
            Savage_Roar_Timer = new Timer(1000 * (9 + (6 * CP)));
            FivePtSav = false;
            return;
        }

        if (Rake.IsSpellUsable && Rake.KnownSpell && Rake.IsDistanceGood && !Rake.TargetHaveBuff)
        {
            Rake.Launch();
            return;
        }

        if (ObjectManager.Target.HealthPercent > 24)
        {
            if (Rip.IsSpellUsable && Rip.KnownSpell && Rip.IsDistanceGood && !FivePtRip && ObjectManager.Me.ComboPoint == 5)
            {
                Rip.Launch();
                Rip_Timer = new Timer(1000 * 13);
                FivePtRip = true;
                return;
            }

            if (Rip.IsSpellUsable && Rip.KnownSpell && Rip.IsDistanceGood && (!Rip.TargetHaveBuff || Rip_Timer.IsReady))
            {
                Rip.Launch();
                Rip_Timer = new Timer(1000 * 13);
                FivePtRip = false;
                return;
            }
        }

        else
        {
            if (Rip.IsSpellUsable && Rip.KnownSpell && Rip.IsDistanceGood && !Rip.TargetHaveBuff)
            {
                CP = ObjectManager.Me.ComboPoint;
                Rip.Launch();
                Rip_Timer = new Timer(1000 * 13);
                if (CP == 5)
                    FivePtFer = true;
                else
                    FivePtFer = false;
                return;
            }

            if (Ferocious_Bite.IsSpellUsable && Ferocious_Bite.KnownSpell && Ferocious_Bite.IsDistanceGood 
                && !FivePtFer && ObjectManager.Me.ComboPoint == 5)
            {
                Ferocious_Bite.Launch();
                Rip_Timer = new Timer(1000 * 13);
                FivePtFer = true;
                return;
            }

            if (Ferocious_Bite.IsSpellUsable && Ferocious_Bite.KnownSpell && Ferocious_Bite.IsDistanceGood 
                && (!Rip.TargetHaveBuff || Rip_Timer.IsReady))
            {
                Ferocious_Bite.Launch();
                Rip_Timer = new Timer(1000 * 13);
                FivePtFer = false;
                return;
            }
        }

        if (Incarnation.KnownSpell && Incarnation.IsSpellUsable)
        {
            Incarnation.Launch();
            return;
        }

        if (ObjectManager.Me.HaveBuff(102543))
        {
            if (Ravage.KnownSpell && Ravage.IsSpellUsable && Ravage.IsDistanceGood)
            {
                Ravage.Launch();
                return;
            }
        }

        else
        {
            if (Mangle.KnownSpell && Mangle.IsSpellUsable && Mangle.IsDistanceGood)
            {
                Mangle.Launch();
                return;
            }
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.ManaPercentage < 10)
        {
            Innervate.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 80 && Gift_of_the_Naaru.IsSpellUsable && Gift_of_the_Naaru.KnownSpell)
        {
            Gift_of_the_Naaru.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 80 && Natures_Swiftness.IsSpellUsable && Natures_Swiftness.KnownSpell)
        {
            Natures_Swiftness.Launch();
            Thread.Sleep(400);
            Healing_Touch.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 50 && Rejuvenation.IsSpellUsable && Rejuvenation.KnownSpell
            && Rejuvenation_Timer.IsReady)
        {
            Rejuvenation.Launch();
            Rejuvenation_Timer = new Timer(1000 * 12);
            return;
        }

        if (ObjectManager.Me.HealthPercent < 40 && Healing_Touch.IsSpellUsable && Healing_Touch.KnownSpell
            && Healing_Touch_Timer.IsReady)
        {
            Healing_Touch.Launch();
            Healing_Touch_Timer = new Timer(1000 * 15);
            return;
        }

        if (ObjectManager.Me.HealthPercent < 30 && Tranquility.IsSpellUsable && Tranquility.KnownSpell)
        {
            Tranquility.Launch();
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(100);
                Thread.Sleep(100);
            }
            return;
        }
    }

    private void Resistance()
    {
        if (ObjectManager.Me.HealthPercent < 70 &&
            Barkskin.KnownSpell && Barkskin.IsSpellUsable)
        {
            Barkskin.Launch();
        }

        if (ObjectManager.Me.HealthPercent < 40 &&
            Survival_Instincts.KnownSpell && Survival_Instincts.IsSpellUsable)
        {
            Survival_Instincts.Launch();
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
            Skull_Bash.KnownSpell && Skull_Bash.IsSpellUsable && Skull_Bash.IsDistanceGood)
        {
            Skull_Bash.Launch();
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
