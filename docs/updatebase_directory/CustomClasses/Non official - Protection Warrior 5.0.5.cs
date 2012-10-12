/*
* CustomClass for TheNoobBot
* Credit : Rival, Geesus, Enelya, Marstor, Vesper, Neo2003, DreadLocks
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
                    #region Warrior Specialisation checking

                case WoWClass.Warrior:
                    var Mortal_Strike = new Spell("Mortal Strike");
                    if (Mortal_Strike.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            MessageBox.Show("There is no settings available for your Class/Specialisation.");
                        }
                        else
                        {
                            Logging.WriteFight("Arms Warrior Found");
                            new Arms();
                        }
                        break;
                    }
                    var Shield_Slam = new Spell("Shield Slam");
                    if (Shield_Slam.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            MessageBox.Show("There is no settings available for your Class/Specialisation.");
                        }
                        else
                        {
                            Logging.WriteFight("Def Warrior Found");
                            new WarriorProt();
                        }
                        break;
                    }
                    var Bloodthirst = new Spell("Bloodthirst");
                    if (Bloodthirst.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            MessageBox.Show("There is no settings available for your Class/Specialisation.");
                        }
                        else
                        {
                            Logging.WriteFight("Fury Warrior Found");
                            new WarriorFury();
                        }
                        break;
                    }
                    if (!Mortal_Strike.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            MessageBox.Show("There is no settings available for your Class/Specialisation.");
                        }
                        else
                        {
                            Logging.WriteFight("Warrior without Spec");
                            new Warrior();
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

#region Warrior

public class Arms
{
    #region InitializeSpell

    private Spell Battle_Shout = new Spell("Battle Shout");
    private Spell Battle_Stance = new Spell("Battle Stance");
    private Spell Berserker_Rage = new Spell("Berserker Rage");
    private Spell Berserker_Stance = new Spell("Berserker Stance");
    private Spell Bladestorm = new Spell("Bladestorm");
    private Spell Charge = new Spell("Charge");
    private Spell Cleave = new Spell("Cleave");
    private Spell Colossus_Smash = new Spell("Colossus Smash");
    private Spell Commanding_Shout = new Spell("Commanding Shout");
    private Spell Deadly_Calm = new Spell("Deadly Calm");
    private Spell Defensive_Stance = new Spell("Defensive Stance");
    private Spell Die_by_the_Sword = new Spell("Die by the Sword");
    private Spell Disarm = new Spell("Disarm");
    private Spell Disrupting_Shout = new Spell("Disrupting Shout");
    private Spell Dragon_Roar = new Spell("Dragon Roar");
    private Spell Enraged_Regeneration = new Spell("Enraged Regeneration");
    private Spell Execute = new Spell("Execute");
    private Spell Hamstring = new Spell("Hamstring");
    private Spell Heroic_Leap = new Spell("Heroic Leap");
    private Spell Heroic_Strike = new Spell("Heroic Strike");
    private Spell Heroic_Throw = new Spell("Heroic Throw");
    private Spell Impending_Victory = new Spell("Impending Victory");
    private Spell Intervene = new Spell("Intervene");
    private Spell Intimidating_Shout = new Spell("Intimidating Shout");
    private Spell Mass_Spell_Reflection = new Spell("Mass Spell Reflection");
    private Spell Mortal_Strike = new Spell("Mortal Strike");
    private Spell Overpower = new Spell("Overpower");
    private Spell Piercing_Howl = new Spell("Piercing Howl");
    private Spell Pummel = new Spell("Pummel");
    private Spell Rallying_Cry = new Spell("Rallying Cry");
    private Spell Recklessness = new Spell("Recklessness");
    private Spell Safeguard = new Spell("Safeguard");
    private Spell Shattering_Throw = new Spell("Shattering Throw");
    private Spell Shield_Wall = new Spell("Shield Wall");
    private Spell Shockwave = new Spell("Shockwave");
    private Spell Slam = new Spell("Slam");
    private Spell Spell_Reflection = new Spell("Spell Reflection");
    private Spell Sunder_Armor = new Spell("Sunder Armor");
    private Spell Staggering_Shout = new Spell("Staggering Shout");
    private Spell Sweeping_Strikes = new Spell("Sweeping Strikes");
    private Spell Taunt = new Spell("Taunt");
    private Spell Throw = new Spell("Throw");
    private Spell Thunder_Clap = new Spell("Thunder Clap");
    private Spell Vigilence = new Spell("Vigilence");
    private Spell Whirlwind = new Spell("Whirlwind");

    // TIMER
    private Timer look = new Timer(0);
    private Timer fighttimer = new Timer(0);
    private Timer rendchill = new Timer(0);


    // Profession & Racials
    private Spell Arcane_Torrent = new Spell("Arcane Torrent");
    private Spell Berserking = new Spell("Berserking");
    private Spell Blood_Fury = new Spell("Blood Fury");
    private Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    private Spell Lifeblood = new Spell("Lifeblood");
    private Spell Stoneform = new Spell("Stoneform");

    #endregion InitializeSpell

    public Arms()
    {
        Main.range = 3.6f;
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

                    if (fighttimer.IsReady && ObjectManager.Target.HealthPercent > 90 && ObjectManager.Me.Target > 0 &&
                        ObjectManager.GetNumberAttackPlayer() < 2)
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
        if (Charge.KnownSpell && Charge.IsSpellUsable && Charge.IsDistanceGood)
        {
            Charge.Launch();
        }
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

        if (Battle_Shout.KnownSpell && Battle_Shout.IsSpellUsable && !Battle_Shout.HaveBuff)
        {
            Battle_Shout.Launch();
        }
    }

    public void fight()
    {
        selfheal();
        buffinfight();
        if (ObjectManager.GetNumberAttackPlayer() > 1) fighttimer = new Timer(60000);

        if (Berserking.KnownSpell && Berserking.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(1454);
            // Berserking.Launch();
        }

        if (Berserker_Rage.KnownSpell && Berserker_Rage.IsSpellUsable && ObjectManager.Me.RagePercentage < 50)
        {
            Berserker_Rage.Launch();
        }

        if (Charge.KnownSpell && Charge.IsSpellUsable && Charge.IsDistanceGood)
        {
            Charge.Launch();
        }

        if (Execute.KnownSpell && Execute.IsSpellUsable && Execute.IsDistanceGood)
        {
            Execute.Launch();
        }

        if (Sweeping_Strikes.KnownSpell && Sweeping_Strikes.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() > 1)
        {
            Sweeping_Strikes.Launch();
        }

        if (Colossus_Smash.KnownSpell && Colossus_Smash.IsSpellUsable && Colossus_Smash.IsDistanceGood)
        {
            Colossus_Smash.Launch();
        }

        if (Heroic_Throw.KnownSpell && Heroic_Throw.IsSpellUsable && Heroic_Throw.IsDistanceGood)
        {
            Heroic_Throw.Launch();
        }

        if (Mortal_Strike.KnownSpell && Mortal_Strike.IsSpellUsable && Mortal_Strike.IsDistanceGood &&
            (ObjectManager.GetNumberAttackPlayer() < 2 ||
             (ObjectManager.GetNumberAttackPlayer() > 1 && !Cleave.KnownSpell)))
        {
            Mortal_Strike.Launch();
        }

        if (Overpower.KnownSpell && Overpower.IsSpellUsable && Overpower.IsDistanceGood &&
            (ObjectManager.GetNumberAttackPlayer() < 2 ||
             (ObjectManager.GetNumberAttackPlayer() > 1 && !Cleave.KnownSpell)))
        {
            Overpower.Launch();
        }

        if (Cleave.KnownSpell && Cleave.IsSpellUsable && Cleave.IsDistanceGood &&
            ObjectManager.GetNumberAttackPlayer() > 1)
        {
            Cleave.Launch();
        }

        if (Heroic_Strike.KnownSpell && Heroic_Strike.IsSpellUsable && Heroic_Strike.IsDistanceGood &&
            ObjectManager.Me.RagePercentage > 70)
        {
            Heroic_Strike.Launch();
        }


        if (Pummel.KnownSpell && Pummel.IsSpellUsable && Pummel.IsDistanceGood &&
            ObjectManager.Target.IsCast)
        {
            Pummel.Launch();
        }

        if (Slam.KnownSpell && Slam.IsSpellUsable && Slam.IsDistanceGood && !Overpower.IsSpellUsable &&
            !Mortal_Strike.IsSpellUsable &&
            (ObjectManager.GetNumberAttackPlayer() < 2 ||
             (ObjectManager.GetNumberAttackPlayer() > 1 && !Cleave.KnownSpell)))
        {
            Slam.Launch();
        }

        if (Arcane_Torrent.KnownSpell && Arcane_Torrent.IsSpellUsable &&
            ObjectManager.Target.IsCast && ObjectManager.Target.GetDistance < 8)
        {
            Arcane_Torrent.Launch();
        }
    }

    private void buffinfight()
    {
        if (!Fight.InFight) return;

        if ((ObjectManager.GetNumberAttackPlayer() > 1 || hardmob()) &&
            ObjectManager.Me.HealthPercent < 65 &&
            ObjectManager.Target.GetDistance < 5 &&
            Stoneform.KnownSpell && Stoneform.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(20594);
            // Stoneform.Launch();
        }

        if (!Battle_Stance.HaveBuff && Battle_Stance.IsSpellUsable)
        {
            Battle_Stance.Launch();
        }

        if (Thunder_Clap.KnownSpell && Thunder_Clap.IsSpellUsable && !Thunder_Clap.TargetHaveBuff && ObjectManager.Target.GetDistance < 9 &&
            (ObjectManager.Me.RagePercentage > 50 || hardmob() || ObjectManager.GetNumberAttackPlayer() > 1))
        {
            Thunder_Clap.Launch();
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
    }

    public bool hardmob()
    {
        if (((ObjectManager.Target.MaxHealth*100)/ObjectManager.Me.MaxHealth) > 100)
        {
            return true;
        }
        return false;
    }
}

public class WarriorProt
{
    #region InitializeSpell

    private Spell Battle_Shout = new Spell("Battle Shout");
    private Spell Battle_Stance = new Spell("Battle Stance");
    private Spell Berserker_Rage = new Spell("Berserker Rage");
    private Spell Berserker_Stance = new Spell("Berserker Stance");
    private Spell Bladestorm = new Spell("Bladestorm");
    private Spell Charge = new Spell("Charge");
    private Spell Cleave = new Spell("Cleave");
    private Spell Commanding_Shout = new Spell("Commanding Shout");
    private Spell Deadly_Calm = new Spell("Deadly Calm");
    private Spell Defensive_Stance = new Spell("Defensive Stance");
    private Spell Demoralizing_Shout = new Spell("Demoralizing Shout");
    private Spell Devastate = new Spell("Devastate");
    private Spell Disarm = new Spell("Disarm");
    private Spell Disrupting_Shout = new Spell("Disrupting Shout");
    private Spell Dragon_Roar = new Spell("Dragon Roar");
    private Spell Enraged_Regeneration = new Spell("Enraged Regeneration");
    private Spell Execute = new Spell("Execute");
    private Spell Hamstring = new Spell("Hamstring");
    private Spell Heroic_Leap = new Spell("Heroic Leap");
    private Spell Heroic_Strike = new Spell("Heroic Strike");
    private Spell Heroic_Throw = new Spell("Heroic Throw");
    private Spell Impending_Victory = new Spell("Impending Victory");
    private Spell Intervene = new Spell("Intervene");
    private Spell Intimidating_Shout = new Spell("Intimidating Shout");
    private Spell Last_Stand = new Spell("Last Stand");
    private Spell Mass_Spell_Reflection = new Spell("Mass Spell Reflection");
    private Spell Piercing_Howl = new Spell("Piercing Howl");
    private Spell Pummel = new Spell("Pummel");
    private Spell Rallying_Cry = new Spell("Rallying Cry");
    private Spell Recklessness = new Spell("Recklessness");
    private Spell Revenge = new Spell("Revenge");
    private Spell Safeguard = new Spell("Safeguard");
    private Spell Shattering_Throw = new Spell("Shattering Throw");
    private Spell Shield_Barrier = new Spell("Shield Barrier");
    private Spell Shield_Block = new Spell("Shield Block");
    private Spell Shield_Slam = new Spell("Shield Slam");
    private Spell Shield_Wall = new Spell("Shield Wall");
    private Spell Shockwave = new Spell("Shockwave");
    private Spell Spell_Reflection = new Spell("Spell Reflection");
    private Spell Staggering_Shout = new Spell("Staggering Shout");
    private Spell Sunder_Armor = new Spell("Sunder Armor");
    private Spell Taunt = new Spell("Taunt");
    private Spell Throw = new Spell("Throw");
    private Spell Thunder_Clap = new Spell("Thunder Clap");
    private Spell Victory_Rush = new Spell("Victory Rush");
    private Spell Vigilence = new Spell("Vigilance");

    private Timer Enraged_Regeneration_Timer = new Timer(0);

    // Profession & Racials
    private Spell Arcane_Torrent = new Spell("Arcane Torrent");
    private Spell Berserking = new Spell("Berserking");
    private Spell Blood_Fury = new Spell("Blood Fury");
    private Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    private Spell Lifeblood = new Spell("Lifeblood");
    private Spell Stoneform = new Spell("Stoneform");

    #endregion InitializeSpell

    public WarriorProt()
    {
        Main.range = 3.6f;

        UInt64 lastTarget = 0;

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

    public void Pull()
    {
        if (!Defensive_Stance.HaveBuff)
            Defensive_Stance.Launch();

        if (Heroic_Leap.KnownSpell && Heroic_Leap.IsSpellUsable && Heroic_Leap.IsDistanceGood)
        {
            SpellManager.CastSpellByIDAndPosition(6544, ObjectManager.Target.Position);
        }

        if (ObjectManager.Target.GetDistance > 15 && Heroic_Throw.KnownSpell && Heroic_Throw.IsSpellUsable && Heroic_Throw.IsDistanceGood)
        {
            Heroic_Throw.Launch();
        }

        if (ObjectManager.Target.GetDistance > 15 && Charge.KnownSpell && Charge.IsSpellUsable && Charge.IsDistanceGood)
        {
            Charge.Launch();
        }
    }

    public void LowCombat()
    {
        AvoidMelee();
        Heal();
        Resistance();
        Buff();

        if (Shield_Slam.KnownSpell && Shield_Slam.IsSpellUsable && Shield_Slam.IsDistanceGood && ObjectManager.Me.RagePercentage < 95)
        {
            Shield_Slam.Launch();
            return;
        }

        if (Revenge.KnownSpell && Revenge.IsDistanceGood && Revenge.IsSpellUsable && ObjectManager.Me.RagePercentage < 95)
        {
            Revenge.Launch();
            return;
        }

        if (Shockwave.KnownSpell && Shockwave.IsSpellUsable && Shockwave.IsDistanceGood)
        {
            Shockwave.Launch();
            return;
        }

        if (Dragon_Roar.KnownSpell && Dragon_Roar.IsSpellUsable && Dragon_Roar.IsDistanceGood)
        {
            Shockwave.Launch();
            return;
        }

        if (Bladestorm.KnownSpell && Bladestorm.IsSpellUsable && Bladestorm.IsDistanceGood)
        {
            Bladestorm.Launch();
            return;
        }

        if (Thunder_Clap.KnownSpell && Thunder_Clap.IsSpellUsable && Thunder_Clap.IsDistanceGood)
        {
            Thunder_Clap.Launch();
            return;
        }

        /*if (Devastate.KnownSpell && Devastate.IsSpellUsable && Devastate.IsDistanceGood)
        {
            Devastate.Launch();
            return;
        }*/
        if (Sunder_Armor.KnownSpell && Sunder_Armor.IsSpellUsable && Sunder_Armor.IsDistanceGood)
        {
            Sunder_Armor.Launch();
            return;
        }
    }

    public void Combat()
    {
        AvoidMelee();
        Decast();
        Heal();
        Resistance();
        Buff();
        Pull();

        if (Berserker_Rage.KnownSpell && Berserker_Rage.IsSpellUsable && Berserker_Rage.IsDistanceGood
            && ObjectManager.Me.HealthPercent > 75)
        {
            Logging.WriteFight("Use Trinket 1.");
            Lua.RunMacroText("/use 13");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Logging.WriteFight("Use Trinket 2.");
            Lua.RunMacroText("/use 14");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Berserker_Rage.Launch();
            return;
        }

        if (Recklessness.KnownSpell && Recklessness.IsSpellUsable && Recklessness.IsDistanceGood)
        {
            Logging.WriteFight("Use Trinket 1.");
            Lua.RunMacroText("/use 13");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Logging.WriteFight("Use Trinket 2.");
            Lua.RunMacroText("/use 14");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Recklessness.Launch();
            return;
        }

        if (ObjectManager.GetNumberAttackPlayer() > 2 && Thunder_Clap.KnownSpell && Thunder_Clap.IsSpellUsable 
            && Thunder_Clap.IsDistanceGood)
        {
            Thunder_Clap.Launch();
            return;
        }

        if (Cleave.KnownSpell && Cleave.IsSpellUsable && Cleave.IsDistanceGood && ObjectManager.GetNumberAttackPlayer() > 2 
            && (ObjectManager.Me.RagePercentage > 80 || ObjectManager.Me.HaveBuff(122510)))
        {
            if (ObjectManager.Me.HealthPercent > 80)
            {
                if (Deadly_Calm.KnownSpell && Deadly_Calm.IsSpellUsable)
                {
                    Deadly_Calm.Launch();
                    Thread.Sleep(200);
                }
                Cleave.Launch();
                return;
            }
        }

        else
        {
            if (Heroic_Strike.KnownSpell && Heroic_Strike.IsSpellUsable && Heroic_Strike.IsDistanceGood && (ObjectManager.Me.RagePercentage > 80
                || ObjectManager.Me.HaveBuff(122510)))
            {
                if (ObjectManager.Me.HealthPercent > 80)
                {
                    if (Deadly_Calm.KnownSpell && Deadly_Calm.IsSpellUsable)
                    {
                        Deadly_Calm.Launch();
                        Thread.Sleep(200);
                    }
                    Heroic_Strike.Launch();
                    return;
                }
            }
        }

        if (Victory_Rush.KnownSpell && Victory_Rush.IsSpellUsable && Victory_Rush.IsDistanceGood && ObjectManager.Me.HealthPercent < 90 )
        {
            Victory_Rush.Launch();
            return;
        }

        if (Shield_Slam.KnownSpell && Shield_Slam.IsSpellUsable && Shield_Slam.IsDistanceGood && ObjectManager.Me.RagePercentage < 95)
        {
            Shield_Slam.Launch();
            return;
        }

        if (Revenge.KnownSpell && Revenge.IsDistanceGood && Revenge.IsSpellUsable && ObjectManager.Me.RagePercentage < 95)
        {
            Revenge.Launch();
            return;
        }

        if (Shockwave.KnownSpell && Shockwave.IsSpellUsable && Shockwave.IsDistanceGood)
        {
            Shockwave.Launch();
            return;
        }

        if (Dragon_Roar.KnownSpell && Dragon_Roar.IsSpellUsable && Dragon_Roar.IsDistanceGood)
        {
            Shockwave.Launch();
            return;
        }

        if (Bladestorm.KnownSpell && Bladestorm.IsSpellUsable && Bladestorm.IsDistanceGood)
        {
            Bladestorm.Launch();
            return;
        }

        if (Thunder_Clap.KnownSpell && Thunder_Clap.IsSpellUsable && Thunder_Clap.IsDistanceGood && !ObjectManager.Target.HaveBuff(115798))
        {
            Thunder_Clap.Launch();
            return;
        }

        if (Battle_Shout.KnownSpell && Battle_Shout.IsSpellUsable)
        {
            Battle_Shout.Launch();
            return;
        }

        if (Shattering_Throw.KnownSpell && Shattering_Throw.IsSpellUsable && Shattering_Throw.IsDistanceGood)
        {
            Logging.WriteFight("Use Trinket 1.");
            Lua.RunMacroText("/use 13");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Logging.WriteFight("Use Trinket 2.");
            Lua.RunMacroText("/use 14");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Shattering_Throw.Launch();
            return;
        }

        /*if (Devastate.KnownSpell && Devastate.IsSpellUsable && Devastate.IsDistanceGood)
        {
            Devastate.Launch();
            return;
        }*/

        if (Sunder_Armor.KnownSpell && Sunder_Armor.IsSpellUsable && Sunder_Armor.IsDistanceGood)
        {
            Sunder_Armor.Launch();
            return;
        }
    }

    public void Patrolling()
    {
        if (!Defensive_Stance.HaveBuff)
            Defensive_Stance.Launch();

        if (!Battle_Shout.HaveBuff)
            Battle_Shout.Launch();
    }

    public void Heal()
    {
        if (ObjectManager.Me.HealthPercent < 90 && Victory_Rush.KnownSpell && Victory_Rush.IsSpellUsable)
        {
            Victory_Rush.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 75 && Enraged_Regeneration.KnownSpell && Enraged_Regeneration_Timer.IsReady
            && (Enraged_Regeneration.IsSpellUsable || Berserker_Rage.IsSpellUsable || ObjectManager.Me.HaveBuff(13046)))
        {
            if (Berserker_Rage.KnownSpell && Berserker_Rage.IsSpellUsable)
            {
                Berserker_Rage.Launch();
                Thread.Sleep(200);
            }

            Enraged_Regeneration.Launch();
            Enraged_Regeneration_Timer = new Timer(1000*60);
        }
    }

    public void Buff()
    {
        if (!Defensive_Stance.HaveBuff)
            Defensive_Stance.Launch();

        if (Battle_Shout.KnownSpell && Battle_Shout.IsSpellUsable && !Battle_Shout.HaveBuff)
            Battle_Shout.Launch();
    }

    private void Resistance()
    {
        if (Demoralizing_Shout.KnownSpell && Demoralizing_Shout.IsSpellUsable && ObjectManager.Target.GetDistance < 10)
        {
            Demoralizing_Shout.Launch();
        }

        if (ObjectManager.Me.HealthPercent < 80 && Shield_Block.KnownSpell && Shield_Block.IsSpellUsable)
        {
            Shield_Block.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 60 && Shield_Wall.KnownSpell && Shield_Wall.IsSpellUsable)
        {
            Shield_Wall.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 60 && Last_Stand.KnownSpell && Last_Stand.IsSpellUsable)
        {
            Last_Stand.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 60 && Rallying_Cry.KnownSpell && Rallying_Cry.IsSpellUsable)
        {
            Rallying_Cry.Launch();
            return;
        }
    }

    private void Decast()
    {
        if (ObjectManager.Target.IsCast && Spell_Reflection.KnownSpell && Spell_Reflection.IsSpellUsable)
        {
            Spell_Reflection.Launch();
            return;
        }

        if (ObjectManager.Target.IsCast && Pummel.KnownSpell && Pummel.IsSpellUsable && Pummel.IsDistanceGood)
        {
            Pummel.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 80 && Shield_Barrier.KnownSpell && Shield_Barrier.IsSpellUsable)
        {
            Shield_Barrier.Launch();
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

public class WarriorFury
{
    #region InitializeSpell

    private Spell Berserker_Stance = new Spell("Berserker Stance");
    private Spell Battle_Stance = new Spell("Battle Stance");

    private Spell Inner_Rage = new Spell("Inner Rage");
    private Spell Enraged_Regeneration = new Spell("Enraged Regeneration");
    private Spell Battle_Shout = new Spell("Battle Shout");
    private Spell Colossus_Smash = new Spell("Colossus Smash");
    private Spell Bloodthirst = new Spell("Bloodthirst");
    private Spell Raging_Blow = new Spell("Raging Blow");
    private Spell Slam = new Spell("Slam");
    private Spell Death_Wish = new Spell("Death Wish");
    private Spell Recklessness = new Spell("Recklessness");
    private Spell Execute = new Spell("Execute");
    private Spell Heroic_Strike = new Spell("Heroic Strike");
    private Spell Intercept = new Spell("Intercept");
    private Spell Enrage = new Spell("Enrage");
    private Spell Bloodsurge = new Spell("Bloodsurge");
    private Spell Heroic_Throw = new Spell("Heroic Throw");
    private Spell Heroic_Leap = new Spell("Heroic Leap");
    private Spell Cleave = new Spell("Cleave");
    private Spell Whirlwind = new Spell("Whirlwind");
    private Spell Victory_Rush = new Spell("Victory Rush");
    private Spell Hamstring = new Spell("Hamstring");
    private Spell Pummel = new Spell("Pummel");
    private Spell Rend = new Spell("Rend");
    private Spell Mortal_Strike = new Spell("Mortal Strike");
    private Spell Overpower = new Spell("Overpower");
    private Spell Deadly_Calm = new Spell("Deadly Calm");
    private Spell Bladestorm = new Spell("Bladestorm");
    private Spell Sweeping_Strikes = new Spell("Sweeping Strikes");
    private Spell Thunder_Clap = new Spell("Thunder Clap");
    private Spell Throwdown = new Spell("Throwdown");
    private Spell Charge = new Spell("Charge");
    private Spell Strike = new Spell("Strike");
    private Spell Intimidating_Shout = new Spell("Intimidating Shout");
    private Spell Commanding_Shout = new Spell("Commanding Shout");
    private Spell Piercing_Howl = new Spell("Piercing Howl");
    private Spell Taste_for_Blood = new Spell("Taste for Blood");
    private Spell Berserker_Rage = new Spell("Berserker Rage");
    private Spell Victorious = new Spell("Victorious");
    private Spell Retaliation = new Spell("Retaliation");

    private Timer Rend_Timer = new Timer(0);
    private Timer Charge_Timer = new Timer(0);
    private Timer Recklessness_Timer = new Timer(0);
    private Timer Enraged_Regeneration_Timer = new Timer(0);
    private Timer Death_Wish_Timer = new Timer(0);
    private Timer Inner_Rage_Timer = new Timer(0);
    private Timer Intercept_leap_Timer = new Timer(0);

    #endregion InitializeSpell

    public WarriorFury()
    {
        Main.range = 3.6f;

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
            Thread.Sleep(350);
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

            if ((ObjectManager.Me.RagePercentage > 70 || Inner_Rage.HaveBuff) &&
                Heroic_Strike.KnownSpell &&
                Heroic_Strike.IsSpellUsable)
            {
                Heroic_Strike.Launch();
                return;
            }
        }

        if (Colossus_Smash.KnownSpell &&
            Colossus_Smash.IsSpellUsable &&
            Colossus_Smash.IsDistanceGood)
        {
            Colossus_Smash.Launch();

            if ((ObjectManager.Me.RagePercentage > 70 || Inner_Rage.HaveBuff) &&
                Heroic_Strike.KnownSpell &&
                Heroic_Strike.IsSpellUsable)
            {
                Heroic_Strike.Launch();
                return;
            }
        }

        if (Bloodthirst.KnownSpell &&
            Bloodthirst.IsDistanceGood &&
            Bloodthirst.IsSpellUsable)
        {
            Bloodthirst.Launch();

            if ((ObjectManager.Me.RagePercentage > 70 || Inner_Rage.HaveBuff) &&
                Heroic_Strike.KnownSpell &&
                Heroic_Strike.IsSpellUsable)
            {
                Heroic_Strike.Launch();
                return;
            }
        }

        if (Raging_Blow.KnownSpell &&
            Raging_Blow.IsSpellUsable &&
            Raging_Blow.IsDistanceGood)
        {
            Raging_Blow.Launch();

            if ((ObjectManager.Me.RagePercentage > 70 || Inner_Rage.HaveBuff) &&
                Heroic_Strike.KnownSpell &&
                Heroic_Strike.IsSpellUsable)
            {
                Heroic_Strike.Launch();
                return;
            }
        }

        if (ObjectManager.Me.HaveBuff(46916) &&
            Slam.KnownSpell &&
            Slam.IsDistanceGood)
        {
            Slam.Launch();

            if ((ObjectManager.Me.RagePercentage > 70 || Inner_Rage.HaveBuff) &&
                Heroic_Strike.KnownSpell &&
                Heroic_Strike.IsSpellUsable)
            {
                Heroic_Strike.Launch();
                return;
            }
        }

        if ((ObjectManager.Me.RagePercentage > 60 || Inner_Rage.HaveBuff) &&
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
            Intercept_leap_Timer = new Timer(1000*3);
        }

        if (Heroic_Leap.KnownSpell && Heroic_Leap.IsSpellUsable && Heroic_Leap.IsDistanceGood &&
            Intercept_leap_Timer.IsReady)
        {
            SpellManager.CastSpellByIDAndPosition(6544, ObjectManager.Target.Position);
            Intercept_leap_Timer = new Timer(1000*3);
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
                    Enraged_Regeneration_Timer = new Timer(1000*60*3);

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
            ObjectManager.Me.RagePercentage < 70)
        {
            Battle_Shout.Launch();
        }

        if (Inner_Rage_Timer.IsReady &&
            Inner_Rage.KnownSpell &&
            Inner_Rage.IsSpellUsable)
        {
            Inner_Rage.Launch();
            Inner_Rage_Timer = new Timer(1000*40);
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
                    Recklessness_Timer = new Timer(1000*300);

                    Death_Wish.Launch();
                    Death_Wish_Timer = new Timer(1000*150);

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
                Death_Wish_Timer = new Timer(1000*150);
            }

            else if (!Death_Wish.KnownSpell && Recklessness.IsSpellUsable && Recklessness_Timer.IsReady)
            {
                Recklessness.Launch();
                Lua.RunMacroText("/use 13");
                Lua.RunMacroText("/script UIErrorsFrame:Clear()");
                Lua.RunMacroText("/use 14");
                Lua.RunMacroText("/script UIErrorsFrame:Clear()");
                Recklessness_Timer = new Timer(1000*300);
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

public class Warrior
{
    #region InitializeSpell

    private Spell Berserker_Stance = new Spell("Berserker Stance");
    private Spell Battle_Stance = new Spell("Battle Stance");

    private Spell Enraged_Regeneration = new Spell("Enraged Regeneration");
    private Spell Battle_Shout = new Spell("Battle Shout");
    private Spell Colossus_Smash = new Spell("Colossus Smash");
    private Spell Bloodthirst = new Spell("Bloodthirst");
    private Spell Raging_Blow = new Spell("Raging Blow");
    private Spell Slam = new Spell("Slam");
    private Spell Death_Wish = new Spell("Death Wish");
    private Spell Recklessness = new Spell("Recklessness");
    private Spell Execute = new Spell("Execute");
    private Spell Heroic_Strike = new Spell("Heroic Strike");
    private Spell Intercept = new Spell("Intercept");
    private Spell Enrage = new Spell("Enrage");
    private Spell Bloodsurge = new Spell("Bloodsurge");
    private Spell Heroic_Throw = new Spell("Heroic Throw");
    private Spell Heroic_Leap = new Spell("Heroic Leap");
    private Spell Cleave = new Spell("Cleave");
    private Spell Whirlwind = new Spell("Whirlwind");
    private Spell Victory_Rush = new Spell("Victory Rush");
    private Spell Hamstring = new Spell("Hamstring");
    private Spell Pummel = new Spell("Pummel");
    private Spell Rend = new Spell("Rend");
    private Spell Mortal_Strike = new Spell("Mortal Strike");
    private Spell Overpower = new Spell("Overpower");
    private Spell Deadly_Calm = new Spell("Deadly Calm");
    private Spell Bladestorm = new Spell("Bladestorm");
    private Spell Sweeping_Strikes = new Spell("Sweeping Strikes");
    private Spell Thunder_Clap = new Spell("Thunder Clap");
    private Spell Throwdown = new Spell("Throwdown");
    private Spell Charge = new Spell("Charge");
    private Spell Strike = new Spell("Strike");
    private Timer Rend_Timer = new Timer(0);

    #endregion InitializeSpell

    public Warrior()
    {
        Main.range = 3.6f;

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
            Thread.Sleep(350);
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
            Rend_Timer = new Timer(1000*10);
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

#endregion
