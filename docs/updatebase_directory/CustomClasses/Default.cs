/*
* CustomClass for TheNoobBot
* Credit : Rival, Geesus, Enelya, Marstor, Vesper, Neo2003
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
    internal static float range = 3.5f;
    internal static bool loop = true;

    public float Range
    {
        get { return range; }
        set { range = value; }
    }

    public void Initialize()
    {
        try
        {
            Logging.WriteFight("Loading combat system.");

            switch (ObjectManager.Me.WowClass)
            {
                #region DeathKnight Specialisation checking
                case WoWClass.DeathKnight:
                    var Heart_Strike = new Spell("Heart Strike");
                    var Scourge_Strike = new Spell("Scourge Strike");
                    var Frost_Strike = new Spell("Frost Strike");
                    if (Heart_Strike.KnownSpell)
                    {
                        Logging.WriteFight("Loading Deathknight Blood class...");
                        new Deathknight_Blood();
                    }
                    if (Scourge_Strike.KnownSpell)
                    {
                        Logging.WriteFight("Loading Deathknight Unholy class...");
                        new Deathknight_Unholy();
                    }
                    if (Frost_Strike.KnownSpell)
                    {
                        Logging.WriteFight("Loading Deathknight Frost class...");
                        new Deathknight_Frost();
                    }
                    if (!Heart_Strike.KnownSpell && !Scourge_Strike.KnownSpell && !Frost_Strike.KnownSpell)
                    {
                        Logging.WriteFight("No specialisation detected.");
                        Logging.WriteFight("Loading Deathknight Apprentice class...");
                        new Deathknight_Apprentice();
                    }
                    break;
                #endregion

                #region Mage Specialisation checking
                case WoWClass.Mage:
                    var Summon_Water_Elemental = new Spell("Summon Water Elemental");
                    var Arcane_Barrage = new Spell("Arcane Barrage");
                    var Pyroblast = new Spell("Pyroblast");
                    if (Summon_Water_Elemental.KnownSpell)
                    {
                        Logging.WriteFight("Loading Mage Frost class...");
                        new Mage_Frost();
                    }
                    if (Arcane_Barrage.KnownSpell)
                    {
                        Logging.WriteFight("Loading Arcane Mage class...");
                        new Mage_Arcane();
                    }
                    if (Pyroblast.KnownSpell)
                    {
                        Logging.WriteFight("Loading Mage Fire class...");
                        new Mage_Fire();
                    }
                    if (!Summon_Water_Elemental.KnownSpell && !Arcane_Barrage.KnownSpell && !Pyroblast.KnownSpell)
                    {
                        Logging.WriteFight("No specialisation detected.");
                        Logging.WriteFight("Loading Mage frost class...");
                        new Mage_Frost();
                    }
                    break;
                #endregion

                #region Warlock Specialisation checking
                case WoWClass.Warlock:
                    var Summon_Felguard = new Spell("Summon Felguard");
                    var Unstable_Affliction = new Spell("Unstable Affliction");
                    if (Unstable_Affliction.KnownSpell)
                    {
                        Logging.WriteFight("Loading Affliction Warlock class...");
                        new Affli();
                    }
                    if (Summon_Felguard.KnownSpell)
                    {
                        Logging.WriteFight("Loading Demonology Warlock class...");
                        new Demo();
                    }
                    if (!Unstable_Affliction.KnownSpell && !Summon_Felguard.KnownSpell)
                    {
                        Logging.WriteFight("No specialisation detected.");
                        Logging.WriteFight("Loading Demonology Warlock class...");
                        new Demo();
                    }
                    break;
                #endregion

                #region Druid Specialisation checking
                case WoWClass.Druid:
                    var Mangle = new Spell("Mangle");
                    if (Mangle.KnownSpell)
                    {
                        Logging.WriteFight("Feral Druid Found");
                        new DruidFeral();
                        break;
                    }
                    var Starsurge = new Spell("Starsurge");
                    if (Starsurge.KnownSpell)
                    {
                        Logging.WriteFight("Balance Dudu Found");
                        new Balance();
                        break;
                    }
                    if (!Starsurge.KnownSpell)
                    {
                        Logging.WriteFight("Dudu without Spec");
                        new Balance();
                        break;
                    }
                    break;
                #endregion

                #region Paladin Specialisation checking
                case WoWClass.Paladin:
                    var Retribution_Spell = new Spell("Templar's Verdict");
                    var Protection_Spell = new Spell("Avenger's Shield");
                    var Holy_Spell = new Spell("Holy Shock");
                    if (Retribution_Spell.KnownSpell)
                    {
                        Logging.WriteFight("Loading Paladin Retribution class...");
                        new Paladin_Retribution();
                        break;
                    }
                    else if (Protection_Spell.KnownSpell)
                    {
                        Logging.WriteFight("Loading Paladin Protection class...");
                        new Paladin_Protection();
                        break;
                    }
                    else if (Holy_Spell.KnownSpell)
                    {
                        Logging.WriteFight("Loading Paladin Holy class...");
                        new Paladin_Holy();
                        break;
                    }
                    else
                    {
                        Logging.WriteFight("No specialisation detected.");
                        Logging.WriteFight("Loading Paladin Retribution class...");
                        new Paladin_Retribution();
                        break;
                    }             
                #endregion

                #region Shaman Specialisation checking
                case WoWClass.Shaman:
                    var Thunderstorm = new Spell("Thunderstorm");
                    if (Thunderstorm.KnownSpell)
                    {
                        Logging.WriteFight("Ele Shaman Found");
                        new Ele();
                    }
                    if (!Thunderstorm.KnownSpell)
                    {
                        Logging.WriteFight("Shaman without Spec");
                        new Ele();
                    }
                    break;
                #endregion

                #region Priest Specialisation checking
                case WoWClass.Priest:
                    var Mind_Flay = new Spell("Mind Flay");
                    if (Mind_Flay.KnownSpell)
                    {
                        Logging.WriteFight("Shadow Priest Found");
                        new Shadow();
                    }
                    if (!Mind_Flay.KnownSpell)
                    {
                        Logging.WriteFight("Priest without Spec");
                        new Shadow();
                    }
                    break;
                #endregion

                #region Rogue Specialisation checking
                case WoWClass.Rogue:
                var Blade_Flurry = new Spell("Blade Flurry");
                var Mutilate = new Spell("Mutilate");
                if (Blade_Flurry.KnownSpell)
                {
                    Logging.WriteFight("Combat Rogue found");
                    new RogueCom();
                }
                if (Mutilate.KnownSpell)
                {
                    Logging.WriteFight("Assassination Rogue found");
                    new RogueAssa();
                }
                else
                {
                    Logging.WriteFight("Rogue without Spec");
                    new Rogue();
                }
                    break;
                #endregion

                #region Warrior Specialisation checking
                case WoWClass.Warrior:
                    var Mortal_Strike = new Spell("Mortal Strike");
                    if (Mortal_Strike.KnownSpell)
                    {
                        Logging.WriteFight("Arms Warrior Found");
                        new Arms();
                        break;
                    }
                    var Shield_Slam = new Spell("Shield Slam");
                    if (Shield_Slam.KnownSpell)
                    {
                        Logging.WriteFight("Def Warrior Found");
                        new WarriorProt();
                        break;
                    }
                    var Bloodthirst = new Spell("Bloodthirst");
                    if (Bloodthirst.KnownSpell)
                    {
                        Logging.WriteFight("Fury Warrior Found");
                        new WarriorFury();
                        break;
                    }
                    if (!Mortal_Strike.KnownSpell)
                    {
                        Logging.WriteFight("Warrior without Spec");
                        new Warrior();
                        break;
                    }
                    break;
                #endregion

                #region Hunter Specialisation checking
                case WoWClass.Hunter:
                    var Explosive_Shot = new Spell("Explosive Shot");
                    if (Explosive_Shot.KnownSpell)
                    {
                        Logging.WriteFight("Survival Hunter Found");
                        new Survival();
                    }
                    var Aimed_Shot = new Spell("Aimed Shot");
                    if (Aimed_Shot.KnownSpell)
                    {
                        Logging.WriteFight("Marksman Hunter Found");
                        new Marks();
                    }
                    var FocusFire = new Spell("Focus Fire");
                    if (FocusFire.KnownSpell)
                    {
                        Logging.WriteFight("Beast Master Hunter Found");
                        new BeastMaster();
                    }
                    if (!Explosive_Shot.KnownSpell && !Aimed_Shot.KnownSpell && !FocusFire.KnownSpell)
                    {
                        Logging.WriteFight("Hunter without Spec");
                        new BeastMaster();
                    }
                    break;
                #endregion
                default:
                    Dispose();
                    break;
            }
        }
        catch { }
        Logging.WriteFight("Combat system stopped.");
    }

    public void Dispose()
    {
        Logging.WriteFight("Combat system stopped.");
        loop = false;
    }

    public void ShowConfiguration()
    {
        MessageBox.Show("This CustomClass has no settings available.");
    }
}

#region Deathknight


public class Deathknight_Apprentice
{
    /**
      * Author : VesperCore
      * Utility : Apprentice Deathkight 55-58 / Starting Area
    **/
    #region InitializeSpell

    // Default Presence
    private readonly Spell Frost_Presence = new Spell("Frost Presence");
    
    // Default Spells
    private readonly Spell Death_Grip = new Spell("Death Grip");
    private readonly Spell Icy_Touch = new Spell("Icy Touch");
    private readonly Spell Plague_Strike = new Spell("Plague Strike");
    private readonly Spell Blood_Strike = new Spell("Blood Strike");
    private readonly Spell Death_Coil = new Spell("Death Coil");
    
    // Level 56 Spells
    private readonly Spell Blood_Boil = new Spell("Blood Boil");
    private readonly Spell Death_Strike = new Spell("Death Strike");
    private readonly Spell Pestilence = new Spell("Pestilence");
    private Timer Timer_Pestilence = new Timer(0);
    private readonly Spell Raise_Dead = new Spell("Raise Dead");
    
    // Level 57 Spells
    private readonly Spell Mind_Freeze = new Spell("Mind Freeze");
    
    // Level 58 Spells
    private readonly Spell Strangulate = new Spell("Strangulate");
    
    // Racials
    private readonly Spell Every_Man_for_Himself = new Spell("Every Man for Himself");
    private readonly Spell Arcane_Torrent = new Spell("Arcane Torrent");
    private readonly Spell Stoneform = new Spell("Stoneform");
    private readonly Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    private readonly Spell War_Stomp = new Spell("War Stomp");
    private readonly Spell Berserking = new Spell("Berserking");

    #endregion InitializeSpell

    public Deathknight_Apprentice()
    {
        Main.range = 3.6f;
        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            try
            {
                if (!ObjectManager.Me.IsMounted)
                {
                    if (Fight.InFight && ObjectManager.Me.Target > 0)
                    {
                        if (ObjectManager.Me.Target != lastTarget && Death_Grip.IsDistanceGood)
                        {
                            Pull();
                            lastTarget = ObjectManager.Me.Target;
                        }
                        Combat();
                    }
                }
            }
            catch
            {
                // Bug
            }
            Thread.Sleep(50);
        }
    }

    public void Pull()
    {
        if (Death_Grip.IsSpellUsable && Death_Grip.IsDistanceGood)
        {
            Death_Grip.Launch();
            MovementManager.StopMove();
        }
    }
    public void Combat()
    {
        Defense_Cycle();
        DPS_Cooldown();
        DPS_Cycle();
        InFightBuffs();
        Spell_Interrupt();
    }
    private void Defense_Cycle()
    {
        if (ObjectManager.Me.HealthPercent < 80 &&
            Gift_of_the_Naaru.KnownSpell && Gift_of_the_Naaru.IsSpellUsable)
        {
            Gift_of_the_Naaru.Launch();
            return;
        }

        if (ObjectManager.Me.HaveBuff(44572) 
            || ObjectManager.Me.HaveBuff(5782) 
            || ObjectManager.Me.HaveBuff(8122)
            || ObjectManager.Me.HaveBuff(853)
            || ObjectManager.Me.HaveBuff(1833))
        {
            if (Every_Man_for_Himself.KnownSpell && Every_Man_for_Himself.IsSpellUsable)
            {
                Every_Man_for_Himself.Launch();
                return;
            }
        }
        if ((ObjectManager.GetNumberAttackPlayer() > 1) &&
            ObjectManager.Me.HealthPercent < 65 &&
            ObjectManager.Target.GetDistance < 5 &&
            Stoneform.KnownSpell && Stoneform.IsSpellUsable)
        {
            Stoneform.Launch();
            return;
        }

        if ((ObjectManager.GetNumberAttackPlayer() > 1) &&
            ObjectManager.Me.HealthPercent < 65 &&
            ObjectManager.Target.GetDistance < 5 &&
            War_Stomp.KnownSpell && War_Stomp.IsSpellUsable)
        {
            War_Stomp.Launch();
            return;
        }
    }
    private void DPS_Cooldown()
    {
        if (Berserking.KnownSpell && Berserking.IsSpellUsable)
        {
            Berserking.Launch();
        }
        if (Raise_Dead.KnownSpell && Raise_Dead.IsSpellUsable)
        {
            Raise_Dead.Launch();
        }
    }
    private void DPS_Cycle()
    {
        if (ObjectManager.Target.GetDistance > 10 && Death_Grip.IsSpellUsable && Death_Grip.IsDistanceGood)
        {
            Death_Grip.Launch();
            MovementManager.StopMove();
        }
        if (!ObjectManager.Target.HaveBuff(55095) && Icy_Touch.IsSpellUsable && Icy_Touch.IsDistanceGood)
        {
            Icy_Touch.Launch();
            return;
        }
        if (!ObjectManager.Target.HaveBuff(55078) && Plague_Strike.IsSpellUsable && Plague_Strike.IsDistanceGood)
        {
            Plague_Strike.Launch();
            return;
        }
        if(ObjectManager.Target.HaveBuff(55078) && ObjectManager.Target.HaveBuff(55095) && Pestilence.KnownSpell 
          && Pestilence.IsSpellUsable && Pestilence.IsDistanceGood && ObjectManager.GetNumberAttackPlayer() > 1 && Timer_Pestilence.IsReady)
        {
            Pestilence.Launch();
            Timer_Pestilence = new Timer(1000 * 30);
            return;
        }
        if(ObjectManager.Target.HaveBuff(55078) && ObjectManager.Target.HaveBuff(55095) && Blood_Boil.IsSpellUsable && Blood_Boil.IsDistanceGood && ObjectManager.GetNumberAttackPlayer() > 1 && !Timer_Pestilence.IsReady)
        {
            Blood_Boil.Launch();
            return;
        }
        if (Death_Coil.IsDistanceGood && Death_Coil.IsSpellUsable)
        {
            Death_Coil.Launch();
            return;
        }
        if (Blood_Strike.IsSpellUsable && Blood_Strike.IsDistanceGood)
        {
            Blood_Strike.Launch();
            return;
        }
    }
    private void InFightBuffs()
    {
        if (!Frost_Presence.HaveBuff && Frost_Presence.IsSpellUsable)
        {
            Frost_Presence.Launch();
        }
    }
    private void Spell_Interrupt()
    {
        if (Arcane_Torrent.KnownSpell && Arcane_Torrent.IsSpellUsable &&
            ObjectManager.Target.IsCast && ObjectManager.Target.GetDistance < 8)
        {
            Arcane_Torrent.Launch();
            return;
        }
        if (ObjectManager.Target.IsCast && Mind_Freeze.KnownSpell && Mind_Freeze.IsSpellUsable && Mind_Freeze.IsDistanceGood)
        {
            Mind_Freeze.Launch();
            return;
        }
        if (ObjectManager.Target.IsCast && Strangulate.KnownSpell && Strangulate.IsSpellUsable && Strangulate.IsDistanceGood)
        {
            Strangulate.Launch();
            return;
        }

    }
}

public class Deathknight_Blood
{
    #region InitializeSpell

    // PRESENCE
    Spell Frost_Presence = new Spell("Frost Presence");
    Spell Blood_Presence = new Spell("Blood Presence");
    Spell Unholy_Presence = new Spell("Unholy Presence");

    // BLOOD
    Spell Rune_Tap = new Spell("Rune Tap");
    Spell Vampiric_Blood = new Spell("Vampiric Blood");
    Spell Dancing_Rune_Weapon = new Spell("Dancing Rune Weapon");
    Spell Heart_Strike = new Spell("Heart Strike");
    Spell Rune_Strike = new Spell("Rune Strike");

    // AOE
    Spell Pestilence = new Spell("Pestilence");
    Spell Death_and_Decay = new Spell("Death and Decay");
    Spell Blood_Boil = new Spell("Blood Boil");

    // BUFF & HELPING
    Spell Death_Grip = new Spell("Death Grip");
    Spell Blood_Tap = new Spell("Blood Tap");
    Spell Empower_Rune_Weapon = new Spell("Empower Rune Weapon");
    Spell Mind_Freeze = new Spell("Mind Freeze");
    Spell Strangulate = new Spell("Strangulate");
    Spell Horn_of_Winter = new Spell("Horn of Winter");
    Spell Lichborne = new Spell("Lichborne");

    // DPS
    Spell Icy_Touch = new Spell("Icy Touch");
    Spell Plague_Strike = new Spell("Plague Strike");
    Spell Outbreak = new Spell("Outbreak");
    Spell Death_Strike = new Spell("Death Strike");
    Spell Death_Coil = new Spell("Death Coil");
    Spell Raise_Dead = new Spell("Raise Dead");
    Spell Death_Pact = new Spell("Death Pact");

    // DEFENCE
    Spell AntiMagic_Shell = new Spell("Anti-Magic Shell");
    Spell Icebound_Fortitude = new Spell("Icebound Fortitude");

    // TIMER
    Timer Pest = new Timer(0);
    Timer look = new Timer(0);
    Timer fighttimer = new Timer(0);
    Timer waitfordebuff1 = new Timer(0);
    Timer waitfordebuff2 = new Timer(0);
    Timer gcd = new Timer(0);

    // Profession & Racials
    Spell Every_Man_for_Himself = new Spell("Every Man for Himself");
    Spell Arcane_Torrent = new Spell("Arcane Torrent");
    Spell Lifeblood = new Spell("Lifeblood");
    Spell Stoneform = new Spell("Stoneform");
    Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    Spell War_Stomp = new Spell("War Stomp");
    Spell Berserking = new Spell("Berserking");

    #endregion InitializeSpell

    public Deathknight_Blood()
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

                    if (fighttimer.IsReady && ObjectManager.Target.HealthPercent > 90 && ObjectManager.Me.Target > 0 && ObjectManager.GetNumberAttackPlayer() < 2)
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
        if (ObjectManager.Target.GetDistance > 10 && Death_Grip.IsSpellUsable && Death_Grip.IsDistanceGood)
        {
            gcd = new Timer(2000);
            SpellManager.CastSpellByIdLUA(49576);
            // Death_Grip.Launch();
            MovementManager.StopMove();
            Thread.Sleep(1000);
        }
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

    }

    public void fight()
    {
        selfheal();
        buffinfight();
        interrupt();
        if (ObjectManager.GetNumberAttackPlayer() > 1) fighttimer = new Timer(60000);

        if (ObjectManager.Target.GetDistance > 10 && Death_Grip.IsSpellUsable && Death_Grip.IsDistanceGood)
        {
            gcd = new Timer(2000);
            SpellManager.CastSpellByIdLUA(49576);
            // Death_Grip.Launch();
            MovementManager.StopMove();
            Thread.Sleep(1000);
        }

        if (ObjectManager.GetNumberAttackPlayer() >= 2 &&
            ObjectManager.Target.HaveBuff(55078) &&
            ObjectManager.Target.HaveBuff(55095) &&
            Pestilence.IsSpellUsable && Pestilence.IsDistanceGood && Pestilence.KnownSpell && Pest.IsReady)
        {
            Pest = new Timer(1000 * 8);
            gcd = new Timer(2000);
            SpellManager.CastSpellByIdLUA(50842);
            // Pestilence.Launch();
            return;
        }

        if (ObjectManager.GetNumberAttackPlayer() > 2 &&
            ObjectManager.Target.HaveBuff(55078) &&
            ObjectManager.Target.HaveBuff(55095) &&
            Blood_Boil.IsSpellUsable)
        {
            gcd = new Timer(2000);
            SpellManager.CastSpellByIdLUA(48721);
            // Blood_Boil.Launch();
            return;
        }

        if (ObjectManager.GetNumberAttackPlayer() > 2 &&
            ObjectManager.Target.HaveBuff(55078) &&
            ObjectManager.Target.HaveBuff(55095) &&
            Death_and_Decay.IsSpellUsable && Death_and_Decay.KnownSpell &&
            ObjectManager.Me.HealthPercent > 60)
        {
            gcd = new Timer(2000);
            SpellManager.CastSpellByIDAndPosition(43265, ObjectManager.Me.Position);
            return;
        }

        if (!Heart_Strike.IsSpellUsable && Heart_Strike.IsDistanceGood && !Death_Strike.IsSpellUsable && Death_Strike.IsDistanceGood && Blood_Tap.IsSpellUsable && gcd.IsReady)
        {
            gcd = new Timer(2000);
            SpellManager.CastSpellByIdLUA(45529);
            // Blood_Tap.Launch();
            return;
        }

        if (!Heart_Strike.IsSpellUsable && Heart_Strike.IsDistanceGood && !Death_Strike.IsSpellUsable && Death_Strike.IsDistanceGood && Empower_Rune_Weapon.IsSpellUsable && gcd.IsReady)
        {
            gcd = new Timer(2000);
            SpellManager.CastSpellByIdLUA(47568);
            // Empower_Rune_Weapon.Launch();
            return;
        }

        if (Rune_Tap.IsSpellUsable && ObjectManager.Me.HealthPercent < 85)
        {
            gcd = new Timer(2000);
            SpellManager.CastSpellByIdLUA(48982);
            // Rune_Tap.Launch();
            return;
        }

        if (!ObjectManager.Target.HaveBuff(55078) && !ObjectManager.Target.HaveBuff(55095) && Outbreak.IsSpellUsable && Outbreak.IsDistanceGood && waitfordebuff1.IsReady)
        {
            gcd = new Timer(2000);
            SpellManager.CastSpellByIdLUA(77575);
            // Outbreak.Launch();
            return;
        }

        if (Berserking.KnownSpell && Berserking.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(1454);
            // Berserking.Launch();
        }

        if (!ObjectManager.Target.HaveBuff(55078) && Plague_Strike.IsSpellUsable && Plague_Strike.IsDistanceGood && waitfordebuff1.IsReady)
        {
            waitfordebuff1 = new Timer(1000);
            gcd = new Timer(2000);
            SpellManager.CastSpellByIdLUA(45462);
            // Plague_Strike.Launch();
        }

        if (!ObjectManager.Target.HaveBuff(55095) && Icy_Touch.IsSpellUsable && Icy_Touch.IsDistanceGood && waitfordebuff2.IsReady)
        {
            waitfordebuff2 = new Timer(1000);
            gcd = new Timer(2000);
            SpellManager.CastSpellByIdLUA(45477);
            // Icy_Touch.Launch();
        }

        if (Death_Strike.IsDistanceGood && Death_Strike.IsSpellUsable)
        {
            gcd = new Timer(2000);
            SpellManager.CastSpellByIdLUA(49998);
            // Death_Strike.Launch();
            return;
        }

        if (ObjectManager.Me.HaveBuff(81141))
        {
            gcd = new Timer(2000);
            SpellManager.CastSpellByIdLUA(48721);
            // Blood_Boil.Launch();
            return;
        }

        if (ObjectManager.Target.GetDistance > 10 && Death_Coil.IsDistanceGood && Death_Coil.IsSpellUsable)
        {
            gcd = new Timer(2000);
            SpellManager.CastSpellByIdLUA(47541);
            // Death_Coil.Launch();
            return;
        }

        if (!Rune_Strike.KnownSpell && Death_Coil.IsDistanceGood && Death_Coil.IsSpellUsable)
        {
            gcd = new Timer(2000);
            SpellManager.CastSpellByIdLUA(47541);
            // Death_Coil.Launch();
            return;
        }

        if (Dancing_Rune_Weapon.KnownSpell && Dancing_Rune_Weapon.IsSpellUsable && ObjectManager.Me.HealthPercent > 80)
        {
            gcd = new Timer(2000);
            SpellManager.CastSpellByIdLUA(49028);
            // Dancing_Rune_Weapon.Launch();
        }

        if (ObjectManager.Me.BarTwoPercentage > 63 && Rune_Strike.IsSpellUsable)
        {
            if (!Lichborne.KnownSpell)
            {
                gcd = new Timer(2000);
                SpellManager.CastSpellByIdLUA(56815);
                // Rune_Strike.Launch();
                return;
            }
            if (ObjectManager.Me.HealthPercent > 63 || !Lichborne.IsSpellUsable)
            {
                gcd = new Timer(2000);
                SpellManager.CastSpellByIdLUA(56815);
                // Rune_Strike.Launch();
                return;
            }
            return;
        }

        if (Heart_Strike.IsDistanceGood && Heart_Strike.IsSpellUsable)
        {
            gcd = new Timer(2000);
            SpellManager.CastSpellByIdLUA(55050);
            // Heart_Strike.Launch();
            return;
        }

        if (Horn_of_Winter.IsSpellUsable)
        {
            gcd = new Timer(2000);
            SpellManager.CastSpellByIdLUA(57330);
            // Horn_of_Winter.Launch();
            return;
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

        if (Lichborne.HaveBuff && Death_Coil.IsSpellUsable)
        {
            gcd = new Timer(2000);
            Lua.RunMacroText("/cast [@player] Death Coil");
            Lua.RunMacroText("/cast [@player] Todesmantel");
            Lua.RunMacroText("/cast [@player] Voile mortel");
            Lua.RunMacroText("/cast [@player] Лик смерти");
            Lua.RunMacroText("/cast [@player] Espiral de la muerte");
            Lua.RunMacroText("/cast [@player] Espiral da Morte");
        }

        if (Lichborne.KnownSpell && Lichborne.IsSpellUsable &&
            ObjectManager.Me.HealthPercent < 40 &&
            ObjectManager.Me.BarTwoPercentage > 63)
        {
            gcd = new Timer(2000);
            Lichborne.Launch();
            Lua.RunMacroText("/cast [@player] Death Coil");
            Lua.RunMacroText("/cast [@player] Todesmantel");
            Lua.RunMacroText("/cast [@player] Voile mortel");
            Lua.RunMacroText("/cast [@player] Лик смерти");
            Lua.RunMacroText("/cast [@player] Espiral de la muerte");
            Lua.RunMacroText("/cast [@player] Espiral da Morte");
        }

        if (Raise_Dead.KnownSpell && Raise_Dead.IsSpellUsable && ObjectManager.Me.HealthPercent < 35)
        {
            gcd = new Timer(2000);
            SpellManager.CastSpellByIdLUA(46584);
            // Raise_Dead.Launch();
        }

        if (ObjectManager.Me.HealthPercent < 35 && Death_Pact.KnownSpell && Death_Pact.IsSpellUsable)
        {
            Logging.WriteFight(" - Very Low Health - Killing Pet -");
            gcd = new Timer(2000);
            SpellManager.CastSpellByIdLUA(48743);
            // Death_Pact.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 45 && Vampiric_Blood.KnownSpell && Vampiric_Blood.IsSpellUsable)
        {
            gcd = new Timer(2000);
            SpellManager.CastSpellByIdLUA(55233);
            // Vampiric_Blood.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 60 &&
            ObjectManager.Target.IsTargetingMe &&
            Icebound_Fortitude.KnownSpell && Icebound_Fortitude.IsSpellUsable)
        {
            gcd = new Timer(2000);
            SpellManager.CastSpellByIdLUA(48792);
            // Icebound_Fortitude.Launch();
        }

        if (ObjectManager.Me.HaveBuff(44572) ||
            ObjectManager.Me.HaveBuff(5782) ||
            ObjectManager.Me.HaveBuff(8122) ||
            ObjectManager.Me.HaveBuff(853) ||
            ObjectManager.Me.HaveBuff(1833))
        {
            if (Every_Man_for_Himself.KnownSpell && Every_Man_for_Himself.IsSpellUsable)
            {
                Every_Man_for_Himself.Launch();
                return;
            }
            if (!Every_Man_for_Himself.KnownSpell)
            {
                Lua.RunMacroText("/use 13");
            }
        }

        if (ObjectManager.Me.HaveBuff(172) ||
            ObjectManager.Me.HaveBuff(13729) ||
            ObjectManager.Me.HaveBuff(55078) ||
            ObjectManager.Me.HaveBuff(8921) ||
            ObjectManager.Me.HaveBuff(44457) ||
            ObjectManager.Me.HaveBuff(116) ||
            ObjectManager.Me.HaveBuff(122) ||
            ObjectManager.Me.HaveBuff(55095) ||
            ObjectManager.Me.HaveBuff(1978) ||
            ObjectManager.Me.HaveBuff(8050) ||
            ObjectManager.Me.HaveBuff(879)
            && AntiMagic_Shell.KnownSpell && AntiMagic_Shell.IsSpellUsable)
        {
            gcd = new Timer(2000);
            SpellManager.CastSpellByIdLUA(48707);
            // AntiMagic_Shell.Launch();
        }

    }

    private void interrupt()
    {

        if (ObjectManager.Target.IsCast &&
            Mind_Freeze.KnownSpell && Mind_Freeze.IsSpellUsable && Mind_Freeze.IsDistanceGood)
        {
            SpellManager.CastSpellByIdLUA(47528);
            // Mind_Freeze.Launch();
            return;
        }

        if (ObjectManager.Target.IsCast &&
            ObjectManager.Target.IsTargetingMe &&
            AntiMagic_Shell.KnownSpell && AntiMagic_Shell.IsSpellUsable)
        {
            gcd = new Timer(2000);
            SpellManager.CastSpellByIdLUA(48707);
            // AntiMagic_Shell.Launch();
            return;
        }

        if (ObjectManager.Target.IsCast &&
            Strangulate.KnownSpell && Strangulate.IsSpellUsable && Strangulate.IsDistanceGood)
        {
            gcd = new Timer(2000);
            SpellManager.CastSpellByIdLUA(47476);
            // Strangulate.Launch();
            return;
        }

        if (Arcane_Torrent.KnownSpell && Arcane_Torrent.IsSpellUsable &&
            ObjectManager.Target.IsCast && ObjectManager.Target.GetDistance < 8)
        {
            Arcane_Torrent.Launch();
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

        if ((Unholy_Presence.HaveBuff || Frost_Presence.HaveBuff && Blood_Presence.KnownSpell) || (!ObjectManager.Me.HaveBuff(48263) && Blood_Presence.KnownSpell))
        {
            Blood_Presence.Launch();
        }
        else if (!Frost_Presence.HaveBuff && !Blood_Presence.KnownSpell)
        {
            Frost_Presence.Launch();
        }

        if (!Horn_of_Winter.HaveBuff && Horn_of_Winter.KnownSpell && Horn_of_Winter.IsSpellUsable)
        {
            gcd = new Timer(2000);
            SpellManager.CastSpellByIdLUA(57330);
            // Horn_of_Winter.Launch();
        }

    }

    public bool hardmob()
    {
        if (((ObjectManager.Target.MaxHealth * 100) / ObjectManager.Me.MaxHealth) > 110)
        {
            return true;
        }
        return false;
    }
}

public class Deathknight_Unholy
{
    #region InitializeSpell

    // PRESENCE
    Spell Frost_Presence = new Spell("Frost Presence");
    Spell Blood_Presence = new Spell("Blood Presence");
    Spell Unholy_Presence = new Spell("Unholy Presence");

    // AOE
    Spell Pestilence = new Spell("Pestilence");
    Spell Death_and_Decay = new Spell("Death and Decay");
    Spell Blood_Boil = new Spell("Blood Boil");

    // UNHOLY
    Spell Bone_Shield = new Spell("Bone Shield");
    Spell Unholy_Frenzy = new Spell("Unholy Frenzy");
    Spell Festering_Strike = new Spell("Festering Strike");
    Spell Scourge_Strike = new Spell("Scourge Strike");
    Spell Sudden_Doom = new Spell("Sudden Doom");
    Spell Summon_Gargoyle = new Spell("Summon Gargoyle");
    Spell AntiMagic_Zone = new Spell("Anti-Magic Zone");
    Spell Dark_Transformation = new Spell("Dark Transformation");

    // BUFF & HELPING
    Spell Death_Grip = new Spell("Death Grip");
    Spell Blood_Tap = new Spell("Blood Tap");
    Spell Empower_Rune_Weapon = new Spell("Empower Rune Weapon");
    Spell Mind_Freeze = new Spell("Mind Freeze");
    Spell Chains_of_Ice = new Spell("Chains of Ice");
    Spell Dark_Simulacrum = new Spell("Dark Simulacrum");
    Spell Strangulate = new Spell("Strangulate");
    Spell Horn_of_Winter = new Spell("Horn of Winter");
    Spell Lichborne = new Spell("Lichborne");

    // DPS
    Spell Icy_Touch = new Spell("Icy Touch");
    Spell Plague_Strike = new Spell("Plague Strike");
    Spell Frost_Fever = new Spell("Frost Fever");
    Spell Blood_Plague = new Spell("Blood Plague");
    Spell Outbreak = new Spell("Outbreak");
    Spell Death_Strike = new Spell("Death Strike");
    Spell Blood_Strike = new Spell("Blood Strike");
    Spell Death_Coil = new Spell("Death Coil");
    Spell Raise_Dead = new Spell("Raise Dead");
    Spell Death_Pact = new Spell("Death Pact");
    Spell Heart_Strike = new Spell("Heart Strike");
    Spell Frost_Strike = new Spell("Frost Strike");

    // DEFENCE
    Spell AntiMagic_Shell = new Spell("Anti-Magic Shell");
    Spell Icebound_Fortitude = new Spell("Icebound Fortitude");

    // TIMER
    Timer Pest = new Timer(0);
    Timer look = new Timer(0);
    Timer fighttimer = new Timer(0);
    Timer waitfordebuff1 = new Timer(0);
    Timer waitfordebuff2 = new Timer(0);
    Timer gcd = new Timer(0);
    Timer mountchill = new Timer(0);

    // Profession & Racials
    Spell Every_Man_for_Himself = new Spell("Every Man for Himself");
    Spell Arcane_Torrent = new Spell("Arcane Torrent");
    Spell Lifeblood = new Spell("Lifeblood");
    Spell Stoneform = new Spell("Stoneform");
    Spell Tailoring = new Spell("Tailoring");
    Spell Leatherworking = new Spell("Leatherworking");
    Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    Spell War_Stomp = new Spell("War Stomp");
    Spell Berserking = new Spell("Berserking");

    #endregion InitializeSpell

    public Deathknight_Unholy()
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

                    if (fighttimer.IsReady && ObjectManager.Target.HealthPercent > 90 && ObjectManager.Me.Target > 0 && ObjectManager.GetNumberAttackPlayer() < 2)
                    {
                        Logging.WriteFight(" - Target Evading - ");
                        break;
                    }
                }
            }
            if (ObjectManager.Me.IsMounted) mountchill = new Timer(2000);
            Thread.Sleep(350);
        }
    }

    public void pull()
    {

        if (hardmob()) Logging.WriteFight(" -  Pull Hard Mob - ");
        if (!hardmob()) Logging.WriteFight(" -  Pull Easy Mob - ");
        pet();
        if (ObjectManager.Target.GetDistance > 10 && Death_Grip.IsSpellUsable && Death_Grip.IsDistanceGood)
        {
            gcd = new Timer(2000);
            SpellManager.CastSpellByIdLUA(49576);
            // Death_Grip.Launch();
            MovementManager.StopMove();
            Thread.Sleep(1000);
        }
        fighttimer = new Timer(60000);

    }

    public void buffoutfight()
    {

        if (Fight.InFight || ObjectManager.Me.IsDeadMe) return;

        pet();

        if (!ObjectManager.Me.HaveBuff(79640) &&
            ItemsManager.GetItemCountByIdLUA(58149) == 1)
        {
            Logging.WriteFight("Use Alchi Flask");
            Lua.RunMacroText("/use item:58149");
        }

    }

    public void fight()
    {
        pet();
        interrupt();
        selfheal();
        buffinfight();
        if (ObjectManager.GetNumberAttackPlayer() > 1) fighttimer = new Timer(60000);

        if (ObjectManager.Target.GetDistance > 10 && Death_Grip.IsSpellUsable && Death_Grip.IsDistanceGood)
        {
            gcd = new Timer(2000);
            SpellManager.CastSpellByIdLUA(49576);
            // Death_Grip.Launch();
            MovementManager.StopMove();
            Thread.Sleep(1000);
        }

        if (ObjectManager.GetNumberAttackPlayer() >= 2 &&
            ObjectManager.Target.HaveBuff(55078) &&
            ObjectManager.Target.HaveBuff(55095) &&
            Pestilence.IsSpellUsable && Pestilence.IsDistanceGood && Pestilence.KnownSpell && Pest.IsReady)
        {
            Pest = new Timer(1000 * 8);
            gcd = new Timer(2000);
            SpellManager.CastSpellByIdLUA(50842);
            // Pestilence.Launch();
            return;
        }

        if (ObjectManager.GetNumberAttackPlayer() > 2 &&
            ObjectManager.Target.HaveBuff(55078) &&
            ObjectManager.Target.HaveBuff(55095) &&
            Blood_Boil.IsSpellUsable)
        {
            gcd = new Timer(2000);
            SpellManager.CastSpellByIdLUA(48721);
            // Blood_Boil.Launch();
            return;
        }

        if (ObjectManager.GetNumberAttackPlayer() > 2 &&
            ObjectManager.Target.HaveBuff(55078) &&
            ObjectManager.Target.HaveBuff(55095) &&
            Death_and_Decay.IsSpellUsable && Death_and_Decay.KnownSpell &&
            ObjectManager.Me.HealthPercent > 60)
        {
            gcd = new Timer(2000);
            SpellManager.CastSpellByIDAndPosition(43265, ObjectManager.Me.Position);
            return;
        }

        if (!Festering_Strike.IsSpellUsable && Festering_Strike.IsDistanceGood && !Scourge_Strike.IsSpellUsable && Scourge_Strike.IsDistanceGood && Blood_Tap.IsSpellUsable && gcd.IsReady)
        {
            gcd = new Timer(2000);
            SpellManager.CastSpellByIdLUA(45529);
            // Blood_Tap.Launch();
            return;
        }

        if (!Festering_Strike.IsSpellUsable && Festering_Strike.IsDistanceGood && !Scourge_Strike.IsSpellUsable && Scourge_Strike.IsDistanceGood && Empower_Rune_Weapon.IsSpellUsable && gcd.IsReady)
        {
            gcd = new Timer(2000);
            SpellManager.CastSpellByIdLUA(47568);
            // Empower_Rune_Weapon.Launch();
            return;
        }

        if (Dark_Transformation.KnownSpell && Dark_Transformation.IsSpellUsable)
        {
            gcd = new Timer(2000);
            SpellManager.CastSpellByIdLUA(63560);
            // Dark_Transformation.Launch();
            return;
        }

        if (!ObjectManager.Target.HaveBuff(55078) && !ObjectManager.Target.HaveBuff(55095) && Outbreak.IsSpellUsable && Outbreak.IsDistanceGood && waitfordebuff1.IsReady)
        {
            gcd = new Timer(2000);
            waitfordebuff1 = new Timer(1000);
            waitfordebuff2 = new Timer(1000);
            SpellManager.CastSpellByIdLUA(77575);
            // Outbreak.Launch();
            return;
        }

        if (Berserking.KnownSpell && Berserking.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(1454);
            // Berserking.Launch();
        }

        if (!ObjectManager.Target.HaveBuff(55078) && Plague_Strike.IsSpellUsable && Plague_Strike.IsDistanceGood && waitfordebuff1.IsReady)
        {
            waitfordebuff1 = new Timer(1000);
            gcd = new Timer(2000);
            SpellManager.CastSpellByIdLUA(45462);
            // Plague_Strike.Launch();
        }

        if (!ObjectManager.Target.HaveBuff(55095) && Icy_Touch.IsSpellUsable && Icy_Touch.IsDistanceGood && waitfordebuff2.IsReady)
        {
            waitfordebuff2 = new Timer(1000);
            gcd = new Timer(2000);
            SpellManager.CastSpellByIdLUA(45477);
            // Icy_Touch.Launch();
        }

        if (ObjectManager.Me.HaveBuff(81340) && Death_Coil.IsDistanceGood)
        {
            gcd = new Timer(2000);
            SpellManager.CastSpellByIdLUA(47541);
            // Death_Coil.Launch();
            return;
        }

        if (ObjectManager.Me.BarTwoPercentage > 63)
        {
            if (Summon_Gargoyle.KnownSpell && Summon_Gargoyle.IsSpellUsable && Summon_Gargoyle.IsDistanceGood && (hardmob() || ObjectManager.GetNumberAttackPlayer() > 2))
            {
                Lua.RunMacroText("/cast [@player] Unholy Frenzy");
                Lua.RunMacroText("/cast [@player] Unheilige Raserei");
                Lua.RunMacroText("/cast [@player] Frénésie impie");
                Lua.RunMacroText("/cast [@player] ???????Лик смерти???");
                Lua.RunMacroText("/cast [@player] Frenesí profano");
                Lua.RunMacroText("/cast [@player] Frenesi Profano");
                Summon_Gargoyle.Launch();
                return;
            }
            if (Death_Coil.IsSpellUsable && Death_Coil.IsDistanceGood)
            {
                gcd = new Timer(2000);
                SpellManager.CastSpellByIdLUA(47541);
                // Death_Coil.Launch();
                return;
            }
            return;
        }

        if (Scourge_Strike.IsDistanceGood && Scourge_Strike.IsSpellUsable)
        {
            gcd = new Timer(2000);
            SpellManager.CastSpellByIdLUA(55090);
            // Scourge_Strike.Launch();
            return;
        }

        if (Festering_Strike.IsDistanceGood && Festering_Strike.IsSpellUsable)
        {
            gcd = new Timer(2000);
            SpellManager.CastSpellByIdLUA(85948);
            // Festering_Strike.Launch();
            return;
        }

        if (Horn_of_Winter.IsSpellUsable)
        {
            gcd = new Timer(2000);
            SpellManager.CastSpellByIdLUA(57330);
            // Horn_of_Winter.Launch();
            return;
        }

        if (!Festering_Strike.KnownSpell && !Scourge_Strike.KnownSpell && Blood_Strike.IsSpellUsable)
        {
            gcd = new Timer(2000);
            SpellManager.CastSpellByIdLUA(45902);
            // Blood_Strike.Launch();
            return;
        }

    }

    private void selfheal()
    {

        if (ObjectManager.Me.HealthPercent < 25 && Death_Pact.KnownSpell && Death_Pact.IsSpellUsable)
        {
            Logging.WriteFight(" - Very Low Health - Killing Pet -");
            gcd = new Timer(2000);
            SpellManager.CastSpellByIdLUA(48743);
            // Death_Pact.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 45)
        {
            gcd = new Timer(2000);
            SpellManager.CastSpellByIdLUA(49998);
            // Death_Strike.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 45 && !Death_Strike.IsSpellUsable && Death_Strike.IsDistanceGood && Death_Coil.IsSpellUsable && Death_Coil.IsDistanceGood)
        {
            gcd = new Timer(2000);
            SpellManager.CastSpellByIdLUA(47541);
            // Death_Coil.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 45 && !Death_Strike.IsSpellUsable && Death_Strike.IsDistanceGood && Blood_Boil.IsSpellUsable)
        {
            gcd = new Timer(2000);
            SpellManager.CastSpellByIdLUA(48721);
            // Blood_Boil.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 65 &&
            Icebound_Fortitude.KnownSpell && Icebound_Fortitude.IsSpellUsable &&
            ObjectManager.Target.IsTargetingMe)
        {
            gcd = new Timer(2000);
            SpellManager.CastSpellByIdLUA(48792);
            // Icebound_Fortitude.Launch();
        }

        if (ObjectManager.Target.IsCast &&
            ObjectManager.Target.IsTargetingMe &&
            AntiMagic_Zone.KnownSpell && AntiMagic_Zone.IsSpellUsable && !AntiMagic_Shell.IsSpellUsable)
        {
            gcd = new Timer(2000);
            SpellManager.CastSpellByIdLUA(51052);
            // AntiMagic_Zone.Launch();
        }

        if (ObjectManager.Me.HaveBuff(44572) ||
            ObjectManager.Me.HaveBuff(5782) ||
            ObjectManager.Me.HaveBuff(8122) ||
            ObjectManager.Me.HaveBuff(853) ||
            ObjectManager.Me.HaveBuff(1833))
        {
            if (Every_Man_for_Himself.KnownSpell && Every_Man_for_Himself.IsSpellUsable)
            {
                Every_Man_for_Himself.Launch();
                return;
            }
            if (!Every_Man_for_Himself.KnownSpell)
            {
                Lua.RunMacroText("/use 13");
            }
        }

        if (ObjectManager.Me.HaveBuff(172) ||
            ObjectManager.Me.HaveBuff(13729) ||
            ObjectManager.Me.HaveBuff(55078) ||
            ObjectManager.Me.HaveBuff(8921) ||
            ObjectManager.Me.HaveBuff(44457) ||
            ObjectManager.Me.HaveBuff(116) ||
            ObjectManager.Me.HaveBuff(122) ||
            ObjectManager.Me.HaveBuff(55095) ||
            ObjectManager.Me.HaveBuff(1978) ||
            ObjectManager.Me.HaveBuff(8050) ||
            ObjectManager.Me.HaveBuff(879)
            && AntiMagic_Shell.KnownSpell && AntiMagic_Shell.IsSpellUsable)
        {
            gcd = new Timer(2000);
            SpellManager.CastSpellByIdLUA(48707);
            // AntiMagic_Shell.Launch();
        }

    }

    private void interrupt()
    {

        if (ObjectManager.Target.IsCast &&
            Mind_Freeze.KnownSpell && Mind_Freeze.IsSpellUsable && Mind_Freeze.IsDistanceGood)
        {
            SpellManager.CastSpellByIdLUA(47528);
            // Mind_Freeze.Launch();
            return;
        }

        if (ObjectManager.Target.IsCast &&
            ObjectManager.Target.IsTargetingMe &&
            AntiMagic_Shell.KnownSpell && AntiMagic_Shell.IsSpellUsable)
        {
            gcd = new Timer(2000);
            SpellManager.CastSpellByIdLUA(48707);
            // AntiMagic_Shell.Launch();
            return;
        }

        if (ObjectManager.Target.IsCast &&
            Strangulate.KnownSpell && Strangulate.IsSpellUsable && Strangulate.IsDistanceGood)
        {
            gcd = new Timer(2000);
            SpellManager.CastSpellByIdLUA(47476);
            // Strangulate.Launch();
            return;
        }

        if (Arcane_Torrent.KnownSpell && Arcane_Torrent.IsSpellUsable &&
            ObjectManager.Target.IsCast && ObjectManager.Target.GetDistance < 8)
        {
            Arcane_Torrent.Launch();
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

        if (!Heart_Strike.KnownSpell && !Scourge_Strike.KnownSpell && !Frost_Strike.KnownSpell && !Frost_Presence.HaveBuff)
        {
            Frost_Presence.Launch();
            return;
        }

        if (!Heart_Strike.KnownSpell && !Scourge_Strike.KnownSpell && !Frost_Strike.KnownSpell && Frost_Presence.HaveBuff)
        {
            return;
        }

        if ((Unholy_Presence.KnownSpell || Frost_Presence.KnownSpell || Blood_Presence.KnownSpell) && ObjectManager.Me.PVP)
        {
            if (!Unholy_Presence.HaveBuff && Unholy_Presence.KnownSpell)
            {
                Unholy_Presence.Launch();
                return;
            }
        }

        if (!Horn_of_Winter.HaveBuff && Horn_of_Winter.KnownSpell && Horn_of_Winter.IsSpellUsable)
        {
            gcd = new Timer(2000);
            SpellManager.CastSpellByIdLUA(57330);
            // Horn_of_Winter.Launch();
        }

    }

    private void pet()
    {

        if (ObjectManager.Me.IsMounted || !mountchill.IsReady) return;
        if ((ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0) &&
            !ObjectManager.Me.IsMounted && !ObjectManager.Me.IsDeadMe)
        {
            Logging.WriteFight(" - PET DEAD - ");
            if (Raise_Dead.KnownSpell && Raise_Dead.IsSpellUsable)
            {
                Logging.WriteFight(" - SUMMONING PET - ");
                gcd = new Timer(2000);
                SpellManager.CastSpellByIdLUA(46584);
                // Raise_Dead.Launch();
            }
        }

    }

    public bool hardmob()
    {
        if (((ObjectManager.Target.MaxHealth * 100) / ObjectManager.Me.MaxHealth) > 110)
        {
            return true;
        }
        return false;
    }
}

public class Deathknight_Frost 
{
    #region InitializeSpell

    Spell Frost_Presence = new Spell("Frost Presence");
    Spell Blood_Presence = new Spell("Blood Presence");
    Spell Unholy_Presence = new Spell("Unholy Presence");
    Spell Death_Grip = new Spell("Death Grip");
    Spell Icy_Touch = new Spell("Icy Touch");
    Spell Frost_Fever = new Spell("Frost Fever");
    Spell Blood_Plague = new Spell("Blood Plague");
    Spell Plague_Strike = new Spell("Plague Strike");
    Spell Death_Strike = new Spell("Death Strike");
    Spell Blood_Strike = new Spell("Blood Strike");
    Spell Horn_of_Winter = new Spell("Horn of Winter");
    Spell Rune_Strike = new Spell("Rune Strike");
    Spell Outbreak = new Spell("Outbreak");
    Spell Obliterate = new Spell("Obliterate");
    Spell Frost_Strike = new Spell("Frost Strike");
    Spell Pillar_of_Frost = new Spell("Pillar of Frost");
    Spell Raise_Dead = new Spell("Raise Dead");
    Spell Death_Pact = new Spell("Death Pact");
    Spell Threat_of_Thassarian = new Spell("Threat of Thassarian");
    Spell Death_Coil = new Spell("Death Coil");
    Spell Howling_Blast = new Spell("Howling Blast");
    Spell Death_and_Decay = new Spell("Death and Decay");
    Spell Blood_Boil = new Spell("Blood Boil");
    Spell Bone_Shield = new Spell("Bone Shield");
    Spell Dancing_Rune_Weapon = new Spell("Dancing Rune Weapon");
    Spell Heart_Strike = new Spell("Heart Strike");
    Spell Rune_Tap = new Spell("Rune Tap");
    Spell Blood_Tap = new Spell("Blood Tap");
    Spell Empower_Rune_Weapon = new Spell("Empower Rune Weapon");
    Spell Dark_Command = new Spell("Dark Command");
    Spell Icebound_Fortitude = new Spell("Icebound Fortitude");
    Spell Vampiric_Blood = new Spell("Vampiric Blood");
    Spell Mind_Freeze = new Spell("Mind Freeze");
    Spell AntiMagic_Shell = new Spell("Anti-Magic Shell");
    Spell Strangulate = new Spell("Strangulate");
    Spell Dark_Simulacrum = new Spell("Dark Simulacrum");
    Timer Pestilence_Timer = new Timer(0);
    Spell Pestilence = new Spell("Pestilence");
    Spell Chains_of_Ice = new Spell("Chains of Ice");
    Spell Hungering_Cold = new Spell("Hungering Cold");
    Spell Freezing_Fog = new Spell("Freezing Fog");
    Spell Killing_Machine = new Spell("Killing Machine");
    Spell Lichborne = new Spell("Lichborne");
    Spell Path_of_Frost = new Spell("Path of Frost");

    Timer Plague_Strike_Timer = new Timer(0);

    #endregion InitializeSpell

    public Deathknight_Frost()
    {
        Main.range = 3.6f;

        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            Buff_path();

            if (!ObjectManager.Me.IsMounted)
            {
                Patrolling();

                if (Fight.InFight && ObjectManager.Me.Target > 0)
                {
                    if (ObjectManager.Me.Target != lastTarget && Death_Grip.IsDistanceGood)
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
        Grip();
    }

    private void Buff_path()
    {
        if (!Fight.InFight && !Path_of_Frost.HaveBuff && Path_of_Frost.KnownSpell && Path_of_Frost.IsSpellUsable)
        {
            Path_of_Frost.Launch();
        }
    }

    public void Combat()
    {
        Presence();
        AvoidMelee();
        Grip();
        Decast();
        TargetMoving();
        Heal();
        Resistance();
        Buff();

        if (ObjectManager.Me.HealthPercent < 85 &&
            Lichborne.HaveBuff && Death_Coil.KnownSpell && Death_Coil.IsSpellUsable)
        {
            Lua.RunMacroText("/target Player");
            Death_Coil.Launch();
            return;
        }

        if (ObjectManager.GetNumberAttackPlayer() > 2 &&
            Death_and_Decay.KnownSpell && Death_and_Decay.IsSpellUsable && Death_and_Decay.IsDistanceGood)
        {
            SpellManager.CastSpellByIDAndPosition(43265, ObjectManager.Target.Position);
        }

        if (Plague_Strike_Timer.IsReady && Plague_Strike.KnownSpell && Plague_Strike.IsDistanceGood && Outbreak.KnownSpell && !Outbreak.IsSpellUsable)
        {
            if (!Plague_Strike.IsSpellUsable && Blood_Tap.KnownSpell && Blood_Tap.IsSpellUsable)
            {
                Blood_Tap.Launch();
                Thread.Sleep(500);
            }

            if (Plague_Strike.IsSpellUsable)
            {
                Plague_Strike.Launch();
                Plague_Strike_Timer = new Timer(1000 * 14);
                return;
            }
        }

        if (Outbreak.KnownSpell && Outbreak.IsSpellUsable && Outbreak.IsDistanceGood)
        {
            Outbreak.Launch();
            Plague_Strike_Timer = new Timer(1000 * 14);
            return;
        }

        if (!Obliterate.IsSpellUsable && !Blood_Strike.IsSpellUsable &&
            Frost_Strike.KnownSpell && Frost_Strike.IsSpellUsable && Frost_Strike.IsDistanceGood)
        {
            if (Howling_Blast.KnownSpell && Howling_Blast.IsSpellUsable && Howling_Blast.IsDistanceGood)
            {
                Howling_Blast.Launch();
                return;
            }

            if (Blood_Tap.KnownSpell && Blood_Tap.IsSpellUsable)
            {
                Blood_Tap.Launch();
                return;
            }

            if (Empower_Rune_Weapon.KnownSpell && Empower_Rune_Weapon.IsSpellUsable)
            {
                Empower_Rune_Weapon.Launch();
                return;
            }

            if (!Lichborne.HaveBuff && Rune_Strike.KnownSpell && Rune_Strike.IsSpellUsable && Rune_Strike.IsDistanceGood)
            {
                Rune_Strike.Launch();
                return;
            }

            if (!Lichborne.HaveBuff)
            {
                Frost_Strike.Launch();
                return;
            }
        }

        if (!Outbreak.KnownSpell)
        {
            if (!Blood_Plague.TargetHaveBuff &&
                Plague_Strike.KnownSpell && Plague_Strike.IsSpellUsable && Plague_Strike.IsDistanceGood)
            {
                Plague_Strike.Launch();
                return;
            }

            if (!Frost_Fever.TargetHaveBuff &&
                Icy_Touch.KnownSpell && Icy_Touch.IsSpellUsable && Icy_Touch.IsDistanceGood)
            {
                Icy_Touch.Launch();
                return;
            }
        }

        if (Obliterate.KnownSpell && Obliterate.IsSpellUsable && Obliterate.IsDistanceGood)
        {
            if (ObjectManager.Me.HealthPercent < 55 &&
                Death_Strike.KnownSpell && Death_Strike.IsSpellUsable && Death_Strike.IsDistanceGood)
            {
                Lua.RunMacroText("/use 13");
                Lua.RunMacroText("/script UIErrorsFrame:Clear()");
                Lua.RunMacroText("/use 14");
                Lua.RunMacroText("/script UIErrorsFrame:Clear()");
                Death_Strike.Launch();
                return;
            }

            Lua.RunMacroText("/use 13");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Lua.RunMacroText("/use 14");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Obliterate.Launch();
            return;
        }

        if (ObjectManager.Me.BarTwoPercentage == 100 &&
            Frost_Strike.KnownSpell && Frost_Strike.IsSpellUsable && Frost_Strike.IsDistanceGood)
        {
            if (!Lichborne.HaveBuff && Rune_Strike.KnownSpell && Rune_Strike.IsSpellUsable && Rune_Strike.IsDistanceGood)
            {
                Rune_Strike.Launch();
                return;
            }

            if (!Lichborne.HaveBuff)
            {
                Frost_Strike.Launch();
                return;
            }
        }

        if ((ObjectManager.Me.HaveBuff(59052) || ObjectManager.Me.HaveBuff(49188) ||
            ObjectManager.Me.HaveBuff(56822) || ObjectManager.Me.HaveBuff(59057)) &&
            Howling_Blast.KnownSpell && Howling_Blast.IsSpellUsable && Howling_Blast.IsDistanceGood)
        {
            Howling_Blast.Launch();
            return;
        }

        if (Blood_Strike.KnownSpell && Blood_Strike.IsSpellUsable && Blood_Strike.IsDistanceGood)
        {
            Blood_Strike.Launch();
            return;
        }
    }

    public void Patrolling()
    {
        if (!Fight.InFight && !Path_of_Frost.HaveBuff && Path_of_Frost.KnownSpell && Path_of_Frost.IsSpellUsable)
        {
            Path_of_Frost.Launch();
        }

        if (!ObjectManager.Me.IsMounted)
        {
            Buff();
        }

        Presence();
    }

    private void Presence()
    {
        if ((Unholy_Presence.KnownSpell || Frost_Presence.KnownSpell || Blood_Presence.KnownSpell) && ObjectManager.Me.PVP)
        {
            if (!Unholy_Presence.HaveBuff && Unholy_Presence.KnownSpell)
            {
                Unholy_Presence.Launch();
                return;
            }

            if (!Unholy_Presence.KnownSpell && !Frost_Presence.HaveBuff && Frost_Presence.KnownSpell)
            {
                Frost_Presence.Launch();
                return;
            }

            /*else if (!Unholy_Presence.KnownSpell && !Frost_Presence.KnownSpell && !Blood_Presence.HaveBuff)
            {
                Blood_Presence.Launch();
                return;
            }*/
        }

        else if (Unholy_Presence.KnownSpell || Frost_Presence.KnownSpell || Blood_Presence.KnownSpell)
        {
            if (!Frost_Presence.HaveBuff && Frost_Presence.KnownSpell)
            {
                Frost_Presence.Launch();
                return;
            }

            if (!Frost_Presence.KnownSpell && !Blood_Presence.HaveBuff)
            {
                Blood_Presence.Launch();
                return;
            }
        }
    }

    private void Grip()
    {
        if (ObjectManager.Target.GetDistance > 5 &&
            Death_Grip.KnownSpell && Death_Grip.IsSpellUsable && Death_Grip.IsDistanceGood)
        {
            Death_Grip.Launch();
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.HealthPercent < 75 &&
            Raise_Dead.KnownSpell && Raise_Dead.IsSpellUsable &&
            Death_Pact.KnownSpell && Death_Pact.IsSpellUsable)
        {
            int i = 0;
            while (i < 3)
            {
                i++;
                Raise_Dead.Launch();
                Death_Pact.Launch();

                if (!Death_Pact.IsSpellUsable)
                {
                    break;
                }
            }
        }

        if (ObjectManager.Me.HealthPercent < 65 &&
            Lichborne.KnownSpell && Lichborne.IsSpellUsable &&
            Death_Coil.KnownSpell && Death_Coil.IsSpellUsable)
        {
            if (Lichborne.IsSpellUsable)
            {
                Lichborne.Launch();
                return;
            }
        }
    }

    private void Resistance()
    {
        if (ObjectManager.Me.HealthPercent < 75 &&
            Icebound_Fortitude.KnownSpell && Icebound_Fortitude.IsSpellUsable)
        {
            Icebound_Fortitude.Launch();
        }
    }

    private void Decast()
    {
        if (ObjectManager.Target.IsCast &&
            ObjectManager.Target.IsTargetingMe &&
            Mind_Freeze.KnownSpell && Mind_Freeze.IsSpellUsable && Mind_Freeze.IsDistanceGood)
        {
            Mind_Freeze.Launch();
        }

        if (ObjectManager.Target.IsCast &&
            ObjectManager.Target.IsTargetingMe &&
            AntiMagic_Shell.KnownSpell && AntiMagic_Shell.IsSpellUsable)
        {
            AntiMagic_Shell.Launch();
        }

        if (ObjectManager.Target.IsCast &&
            ObjectManager.Target.IsTargetingMe &&
            Strangulate.KnownSpell && Strangulate.IsSpellUsable && Strangulate.IsDistanceGood)
        {
            Strangulate.Launch();
        }

        if (ObjectManager.Target.IsCast &&
            Dark_Simulacrum.KnownSpell && Dark_Simulacrum.IsSpellUsable && Dark_Simulacrum.IsDistanceGood)
        {
            Dark_Simulacrum.Launch();
        }
    }

    private void TargetMoving()
    {
        if (ObjectManager.Target.GetDistance >= 10 &&
            Hungering_Cold.KnownSpell && Hungering_Cold.IsSpellUsable)
        {
            Hungering_Cold.Launch();
        }

        if (ObjectManager.Target.GetMove && !Chains_of_Ice.TargetHaveBuff &&
            Chains_of_Ice.KnownSpell && Chains_of_Ice.IsSpellUsable && Chains_of_Ice.IsDistanceGood)
        {
            Chains_of_Ice.Launch();
        }
    }

    private void Buff()
    {
        if (!Horn_of_Winter.HaveBuff && Horn_of_Winter.KnownSpell && Horn_of_Winter.IsSpellUsable)
        {
            Horn_of_Winter.Launch();
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

#region Mage
public class Mage_Frost
{
    #region InitializeSpell

    Spell Summon_Water_Elemental = new Spell("Summon Water Elemental");
    Spell Ice_Barrier = new Spell("Ice Barrier");
    Spell Mana_Shield = new Spell("Mana Shield");
    Spell Mage_Ward = new Spell("Mage Ward");
    Spell Frostbolt = new Spell("Frostbolt");
    Spell Frostfire_Bolt = new Spell("Frostfire Bolt");
    Spell Ice_Lance = new Spell("Ice Lance");
    Spell Deep_Freeze = new Spell("Deep Freeze");
    Spell Frost_Nova = new Spell("Frost Nova");
    Spell Cone_of_Cold = new Spell("Cone of Cold");
    Spell Flame_Orb = new Spell("Flame Orb");
    Spell Freeze = new Spell("Freeze");
    Spell Ring_of_Frost = new Spell("Ring of Frost");
    Spell Mirror_Image = new Spell("Mirror Image");
    Spell Invisibility = new Spell("Invisibility");
    Spell Fireblast = new Spell("Fireblast");
    Spell Icy_Veins = new Spell("Icy Veins");
    Spell Cold_Snap = new Spell("Cold_Snap");
    Spell Polymorph = new Spell("Polymorph");
    Spell Ice_Block = new Spell("Ice Block");
    Spell Counterspell = new Spell("Counterspell");
    Spell Blink = new Spell("Blink");
    Spell Evocate = new Spell("Evocate");
    Spell Arcane_Blast = new Spell("Arcane Blast");
    Spell Arcane_Explosion = new Spell("Arcane Explosion");
    Spell Remove_Curse = new Spell("Remove Curse");
    Spell Mage_Armor = new Spell("Mage Armor");
    Spell Arcane_Brilliance = new Spell("Arcane Brilliance");
    Spell Time_Warp = new Spell("Time Warp");
    Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    Spell Arcane_Torrent = new Spell("Arcane Torrent");
    Spell Escape_Artist = new Spell("Escape Artist");
    Spell Heroism = new Spell("Heroism");
    Spell Dalaran_Brilliance = new Spell("Dalaran Brilliance");
    Spell Improved_Cone_of_Cold = new Spell("Improved Cone of Cold");

    #endregion InitializeSpell

    public Mage_Frost()
    {
        Main.range = 28.0f;

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
            Thread.Sleep(350);
        }
    }

    public void Pull()
    {
        Pet();
        Buff();
    }

    public void Combat()
    {

        AvoidMelee();
        Blinking();
        Heal();
        Buff();
        Interrupt();
        BuffCombat();

        if (ObjectManager.Me.HaveBuff(44544) || Frost_Nova.TargetHaveBuff || Freeze.TargetHaveBuff || Deep_Freeze.TargetHaveBuff || Improved_Cone_of_Cold.TargetHaveBuff)
        {
            if (Deep_Freeze.KnownSpell && Deep_Freeze.IsSpellUsable && Deep_Freeze.IsDistanceGood)
            {
                Deep_Freeze.Launch();
            }

            else
            {
                if (Ice_Lance.KnownSpell && Ice_Lance.IsSpellUsable && Deep_Freeze.IsDistanceGood)
                {
                    Ice_Lance.Launch();
                    return;
                }
            }
        }



        if (ObjectManager.Me.HaveBuff(57761) &&
           Frostfire_Bolt.KnownSpell &&
           Frostfire_Bolt.IsSpellUsable &&
           Frostfire_Bolt.IsDistanceGood)
        {
            Frostfire_Bolt.Launch();
            return;
        }

        if (Freeze.KnownSpell && Freeze.IsSpellUsable && Freeze.IsDistanceGood)
        {
            SpellManager.CastSpellByIDAndPosition(33395, ObjectManager.Target.Position);
            return;
        }

        if (ObjectManager.Me.HealthPercent < 10 &&
            Ice_Lance.KnownSpell &&
            Ice_Lance.IsSpellUsable &&
            Ice_Lance.IsDistanceGood)
        {
            Ice_Lance.Launch();
            return;
        }

        if (ObjectManager.Target.GetDistance < 8 &&
            Cone_of_Cold.KnownSpell &&
            Cone_of_Cold.IsDistanceGood &&
            Cone_of_Cold.IsSpellUsable)
        {
            Cone_of_Cold.Launch();
            return;
        }

        if (ObjectManager.Target.GetDistance < 15 &&
            Flame_Orb.KnownSpell &&
            Flame_Orb.IsDistanceGood &&
            Flame_Orb.IsSpellUsable)
        {
            Flame_Orb.Launch();
        }

        if (Frostbolt.KnownSpell &&
            Frostbolt.IsDistanceGood &&
            Frostbolt.IsSpellUsable)
        {
            Frostbolt.Launch();
            return;
        }

        if (Fireblast.KnownSpell &&
            Fireblast.IsDistanceGood &&
            Fireblast.IsSpellUsable)
        {
            Fireblast.Launch();
            return;
        }

        if (Arcane_Blast.KnownSpell &&
            Arcane_Blast.IsDistanceGood &&
            Arcane_Blast.IsSpellUsable)
        {
            Arcane_Blast.Launch();
            return;
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
            if (Summon_Water_Elemental.KnownSpell &&
                Summon_Water_Elemental.IsSpellUsable)
            {
                Summon_Water_Elemental.Launch();
            }
        }
    }

    private void Buff()
    {

        if (!Arcane_Brilliance.HaveBuff &&
            !Dalaran_Brilliance.HaveBuff &&
            Arcane_Brilliance.KnownSpell &&
            Arcane_Brilliance.IsSpellUsable)
        {
            Arcane_Brilliance.Launch();
        }

        if (!Mage_Armor.HaveBuff &&
            Mage_Armor.KnownSpell &&
            Mage_Armor.IsSpellUsable)
        {
            Mage_Armor.Launch();
        }

        if (!Ice_Barrier.HaveBuff &&
            Ice_Barrier.KnownSpell &&
            Ice_Barrier.IsSpellUsable)
        {
            Ice_Barrier.Launch();
        }

        if (!Mana_Shield.HaveBuff &&
            Mana_Shield.KnownSpell &&
            Mana_Shield.IsSpellUsable)
        {
            Mana_Shield.Launch();
        }

        if (!Mage_Ward.HaveBuff &&
            Mage_Ward.KnownSpell &&
            Mage_Ward.IsSpellUsable)
        {
            Mage_Ward.Launch();
        }

    }

    private void TargetMoving()
    {
        if (ObjectManager.Target.GetDistance >= 11 &&
            ObjectManager.Target.GetMove &&
            !Frostbolt.TargetHaveBuff &&
            !Cone_of_Cold.TargetHaveBuff &&
            Frostbolt.KnownSpell &&
            Frostbolt.IsDistanceGood &&
            Frostbolt.IsSpellUsable)
        {
            Frostbolt.Launch();
        }

        if (ObjectManager.Target.GetDistance <= 11 &&
            ObjectManager.Target.GetMove &&
            !Frostbolt.TargetHaveBuff &&
            !Cone_of_Cold.TargetHaveBuff &&
            Cone_of_Cold.KnownSpell &&
            Cone_of_Cold.IsDistanceGood &&
            Cone_of_Cold.IsSpellUsable)
        {
            Cone_of_Cold.Launch();
        }

        if (ObjectManager.Target.GetDistance <= 13 &&
            (ObjectManager.GetNumberAttackPlayer() >= 2 ||
            ObjectManager.Target.GetMove) &&
            !Frost_Nova.TargetHaveBuff &&
            Frost_Nova.KnownSpell &&
            Frost_Nova.IsDistanceGood &&
            Frost_Nova.IsSpellUsable)
        {
            Frost_Nova.Launch();
        }
    }

    private void Heal()
    {
        if (Ice_Block.KnownSpell &&
            ObjectManager.Me.HealthPercent > 0 &&
            ObjectManager.Me.HealthPercent < 25 &&
            ObjectManager.GetNumberAttackPlayer() >= 1 &&
            Ice_Block.IsSpellUsable)
        {
            Ice_Block.Launch();
        }

        if (Gift_of_the_Naaru.KnownSpell &&
            ObjectManager.Me.HealthPercent > 0 &&
            ObjectManager.Me.HealthPercent < 50 &&
            Gift_of_the_Naaru.IsSpellUsable)
        {
            Gift_of_the_Naaru.Launch();
        }

        if (Evocate.KnownSpell &&
            ObjectManager.Me.HealthPercent > 0 &&
            ObjectManager.Me.HealthPercent < 35 &&
            (ObjectManager.Me.HaveBuff(11426) || ObjectManager.Me.HaveBuff(1463) ||
            ObjectManager.GetNumberAttackPlayer() <= 2) &&
            Evocate.IsSpellUsable)
        {
            Evocate.Launch();
        }

        if (Ring_of_Frost.KnownSpell && Ring_of_Frost.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() >= 2 &&
         ObjectManager.Target.GetDistance <= 15)
        {
            SpellManager.CastSpellByIDAndPosition(82676, ObjectManager.Me.Position);
        }
    }

    private void BuffCombat()
    {

        if (ObjectManager.GetNumberAttackPlayer() <= 3 &&
            Icy_Veins.KnownSpell &&
            Icy_Veins.IsSpellUsable &&
            Icy_Veins.IsDistanceGood)
        {
            Icy_Veins.Launch();
        }

        if (!Time_Warp.HaveBuff &&
            !Heroism.HaveBuff &&
            ObjectManager.GetNumberAttackPlayer() <= 3 &&
            Time_Warp.KnownSpell &&
            Time_Warp.IsSpellUsable &&
            Time_Warp.IsDistanceGood)
        {
            Time_Warp.Launch();
        }

    }

    private void Interrupt()
    {
        if (ObjectManager.Target.IsCast &&
            Counterspell.KnownSpell &&
            Counterspell.IsSpellUsable &&
            Counterspell.IsDistanceGood)
        {
            Counterspell.Launch();
        }

    }

    private void Blinking()
    {
        if ((ObjectManager.Me.HaveBuff(44572) || ObjectManager.Me.HaveBuff(408) || ObjectManager.Me.HaveBuff(65929) ||
             ObjectManager.Me.HaveBuff(85388) ||
             ObjectManager.Me.HaveBuff(30283)) &&
             Blink.KnownSpell &&
             Blink.IsSpellUsable)
        {
            Blink.Launch();
        }

        if (ObjectManager.Target.GetDistance > 35 &&
            Blink.KnownSpell &&
            Blink.IsSpellUsable)
        {
            Blink.Launch();
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

public class Mage_Arcane
{
    #region InitializeSpell

    // Arcane Only
    Spell Arcane_Barrage = new Spell("Arcane Barrage");
    Spell Arcane_Blast = new Spell("Arcane Blast");
    Spell Arcane_Explosion = new Spell("Arcane Explosion");
    Spell Presence_of_Mind = new Spell("Presence of Mind");
    Spell Slow = new Spell("Slow");

    // Survive
    Spell Mana_Shield = new Spell("Mana Shield");
    Spell Mage_Ward = new Spell("Mage Ward");
    Spell Ring_of_Frost = new Spell("Ring of Frost");
    Spell Frost_Nova = new Spell("Frost Nova");
    Spell Blink = new Spell("Blink");
    Spell Counterspell = new Spell("Counterspell");
    Spell Frostbolt = new Spell("Frostbolt");

    // DPS
    Spell Frostfire_Bolt = new Spell("Frostfire Bolt");
    Spell Fireball = new Spell("Fireball");
    Spell Flame_Orb = new Spell("Flame Orb");
    Spell Fire_Blast = new Spell("Fire Blast");
    Spell Arcane_Missiles = new Spell("Arcane Missiles");

    // BUFF & HELPING
    Spell Evocation = new Spell("Evocation");
    Spell Conjure_Refreshment = new Spell("Conjure Refreshment");
    Spell Arcane_Brilliance = new Spell("Arcane Brilliance");
    Spell Remove_Curse = new Spell("Remove Curse");
    Spell Mage_Armor = new Spell("Mage Armor");
    Spell Molten_Armor = new Spell("Molten Armor");
    Spell Conjure_Mana_Gem = new Spell("Conjure Mana Gem");

    // BIG CD
    Spell Mirror_Image = new Spell("Mirror Image");
    Spell Time_Warp = new Spell("Time Warp");
    Spell Invisibility = new Spell("Invisibility");
    Spell Ice_Block = new Spell("Ice Block");
    Spell Cold_Snap = new Spell("Cold Snap");

    // TIMER
    Timer freeze = new Timer(0);
    Timer look = new Timer(0);
    Timer fighttimer = new Timer(0);
    Timer waitfordebuff = new Timer(0);

    // Profession & Racials
    Spell Arcane_Torrent = new Spell("Arcane Torrent");
    Spell Lifeblood = new Spell("Lifeblood");
    Spell Stoneform = new Spell("Stoneform");
    Spell Tailoring = new Spell("Tailoring");
    Spell Leatherworking = new Spell("Leatherworking");
    Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    Spell War_Stomp = new Spell("War Stomp");
    Spell Berserking = new Spell("Berserking");

    #endregion InitializeSpell

    public Mage_Arcane()
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

                    if (fighttimer.IsReady && ObjectManager.Target.HealthPercent > 90 && ObjectManager.Me.Target > 0 && ObjectManager.GetNumberAttackPlayer() < 2)
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
        if (ObjectManager.Target.GetDistance > 25 && !Slow.KnownSpell) SpellManager.CastSpellByIdLUA(116); //Frostbolt.Launch();

    }

    public void buffoutfight()
    {

        if (Fight.InFight || ObjectManager.Me.IsDeadMe) return;

        if (Evocation.KnownSpell && Evocation.IsSpellUsable &&
            ObjectManager.Me.HealthPercent < 40 ||
            ObjectManager.Me.BarTwoPercentage < 40)
        {
            SpellManager.CastSpellByIdLUA(12051);
            // Evocation.Launch();
        }

        if (!ObjectManager.Me.HaveBuff(79640) &&
            ItemsManager.GetItemCountByIdLUA(58149) == 1)
        {
            Logging.WriteFight("Use Alchi Flask");
            Lua.RunMacroText("/use item:58149");
        }

        if (Arcane_Brilliance.KnownSpell && Arcane_Brilliance.IsSpellUsable &&
            !Arcane_Brilliance.HaveBuff)
        {
            Arcane_Brilliance.Launch();
        }

        if (Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(65499) == 0 &&	// 85
            Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(43523) == 0 &&	// 84-80
            Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(43518) == 0 &&	// 79-74
            Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(65517) == 0 &&	// 73-64
            Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(65516) == 0 &&	// 63-54
            Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(65515) == 0 &&	// 53-44
            Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(65500) == 0)		// 43-38
        {
            Thread.Sleep(100);
            Conjure_Refreshment.Launch();
            Thread.Sleep(200);
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(200);
            }
            Thread.Sleep(100);
        }

        if (Conjure_Mana_Gem.KnownSpell && ItemsManager.GetItemCountByIdLUA(36799) == 0)
        {
            Thread.Sleep(100);
            Conjure_Mana_Gem.Launch();
            Thread.Sleep(200);
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(200);
            }
            Thread.Sleep(100);
        }

        if (Mage_Armor.KnownSpell && Mage_Armor.IsSpellUsable && !Mage_Armor.HaveBuff)
        {
            Mage_Armor.Launch();
        }

        if (!Mage_Armor.KnownSpell && Molten_Armor.KnownSpell && !Molten_Armor.HaveBuff)
        {
            Molten_Armor.Launch();
        }

    }

    public void fight()
    {
        selfheal();
        buffinfight();
        if (ObjectManager.GetNumberAttackPlayer() > 1) fighttimer = new Timer(60000);

        if (Mirror_Image.KnownSpell && Mirror_Image.IsSpellUsable && hardmob() || ObjectManager.GetNumberAttackPlayer() > 1)
        {
            SpellManager.CastSpellByIdLUA(55342);
            // Mirror_Image.Launch();
        }

        if (Berserking.KnownSpell && Berserking.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(1454);
            // Berserking.Launch();
        }

        if (Arcane_Blast.KnownSpell && Arcane_Blast.IsDistanceGood && Arcane_Blast.IsSpellUsable && ObjectManager.Target.GetDistance > 8)
        {
            SpellManager.CastSpellByIdLUA(30451);
            // Arcane_Blast.Launch();
            return;
        }

        if (Arcane_Blast.KnownSpell && Arcane_Blast.IsDistanceGood && Arcane_Blast.IsSpellUsable && !Arcane_Missiles.IsSpellUsable && !Arcane_Barrage.IsSpellUsable && !Fire_Blast.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(30451);
            // Arcane_Blast.Launch();
            return;
        }

        if (!Arcane_Blast.KnownSpell && Frostbolt.KnownSpell && Frostbolt.IsDistanceGood && Frostbolt.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(116);
            // Frostbolt.Launch();
        }

        if (!Frostbolt.KnownSpell && Fireball.IsSpellUsable && Fireball.IsDistanceGood)
        {
            SpellManager.CastSpellByIdLUA(133);
            // Fireball.Launch();
        }

        if (Arcane_Missiles.KnownSpell &&
            Arcane_Missiles.IsSpellUsable &&
            Arcane_Missiles.IsDistanceGood)
        {
            SpellManager.CastSpellByIdLUA(5143);
            // Arcane_Missiles.Launch();
            Thread.Sleep(200);
            while (ObjectManager.Me.IsCast)
            {
                Fight.StopFight();
                MovementManager.StopMove();
                Thread.Sleep(200);
            }
        }

        if (Slow.KnownSpell && Slow.IsDistanceGood && Slow.IsSpellUsable && !Slow.TargetHaveBuff && ObjectManager.Target.IsTargetingMe)
        {
            SpellManager.CastSpellByIdLUA(31589);
            // Slow.Launch();
        }

        if (Arcane_Barrage.KnownSpell && Arcane_Barrage.IsSpellUsable && Arcane_Barrage.IsDistanceGood)
        {
            SpellManager.CastSpellByIdLUA(44425);
            // Arcane_Barrage.Launch();	
        }

        if (ObjectManager.Target.GetDistance < 8 &&
            Fire_Blast.KnownSpell &&
            Fire_Blast.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(2136);
            // Fire_Blast.Launch();
        }

        if (Counterspell.KnownSpell && Counterspell.IsSpellUsable && Counterspell.IsDistanceGood &&
            ObjectManager.Target.IsCast && ObjectManager.Target.HealthPercent > 30)
        {
            SpellManager.CastSpellByIdLUA(2139);
            // Counterspell.Launch();
        }

        if (Arcane_Torrent.KnownSpell && Arcane_Torrent.IsSpellUsable &&
            ObjectManager.Target.IsCast && ObjectManager.Target.GetDistance < 8)
        {
            Arcane_Torrent.Launch();
        }

        if (Arcane_Explosion.KnownSpell && Arcane_Explosion.IsSpellUsable &&
            ObjectManager.Target.GetDistance < 8 &&
            ObjectManager.GetNumberAttackPlayer() > 3)
        {
            SpellManager.CastSpellByIdLUA(1449);
            // Arcane_Explosion.Launch();
        }

        if (Flame_Orb.KnownSpell && Flame_Orb.IsDistanceGood && Flame_Orb.IsSpellUsable && hardmob())
        {
            SpellManager.CastSpellByIdLUA(82731);
            // Flame_Orb.Launch();
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

        if ((ObjectManager.GetNumberAttackPlayer() > 1 || hardmob()) &&
            ObjectManager.Me.HealthPercent < 65 &&
            ObjectManager.Target.GetDistance < 5 &&
            War_Stomp.KnownSpell && War_Stomp.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(20549);
            // War_Stomp.Launch();
        }

        if (Presence_of_Mind.KnownSpell && Presence_of_Mind.IsSpellUsable && ObjectManager.Target.IsTargetingMe)
        {
            SpellManager.CastSpellByIdLUA(12043);
            // Presence_of_Mind.Launch();
        }

        if (Time_Warp.KnownSpell && Time_Warp.IsSpellUsable &&
            !Time_Warp.HaveBuff && hardmob() && ObjectManager.GetNumberAttackPlayer() > 2)
        {
            SpellManager.CastSpellByIdLUA(80353);
            // Time_Warp.Launch();
        }

        if (Frost_Nova.KnownSpell && Frost_Nova.IsSpellUsable && ObjectManager.Target.GetDistance < 6)
        {
            SpellManager.CastSpellByIdLUA(122);
            // Frost_Nova.Launch();
        }

        if (Blink.KnownSpell && Blink.IsSpellUsable && Fight.InFight && !Frost_Nova.IsSpellUsable &&
            ObjectManager.Target.GetDistance < 6 && ObjectManager.Target.HealthPercent > 30)
        {
            SpellManager.CastSpellByIdLUA(1953);
            // Blink.Launch();	
        }

        if (ObjectManager.Target.GetDistance > 55 && Blink.KnownSpell && Blink.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(1953);
            // Blink.Launch();	
        }

    }

    private void selfheal()
    {

        if (Mage_Ward.KnownSpell && Mage_Ward.IsSpellUsable && ObjectManager.Target.IsCast &&
            ObjectManager.Target.IsTargetingMe && !Counterspell.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(543);
            // Mage_Ward.Launch();
        }

        if (ObjectManager.Me.BarTwoPercentage < 40 &&
            ItemsManager.GetItemCountByIdLUA(36799) > 0)
        {
            Lua.RunMacroText("/use item:36799");
        }


        if (!Mana_Shield.HaveBuff && ObjectManager.Me.HealthPercent < 85 &&
            Mana_Shield.KnownSpell && Mana_Shield.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(1463);
            // Mana_Shield.Launch();
        }

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

        if (Ice_Block.KnownSpell && Ice_Block.IsSpellUsable && ObjectManager.Me.HealthPercent > 0 &&
            ObjectManager.Me.HealthPercent < 25 && ObjectManager.GetNumberAttackPlayer() > 1 &&
            Fight.InFight)
        {
            SpellManager.CastSpellByIdLUA(45438);
            // Ice_Block.Launch();
        }

        if (Evocation.KnownSpell && Evocation.IsSpellUsable &&
            ObjectManager.Me.HealthPercent > 0 &&
            ObjectManager.Me.HealthPercent < 30 ||
            ObjectManager.Me.BarTwoPercentage < 20)
        {
            SpellManager.CastSpellByIdLUA(12051);
            // Evocation.Launch();
        }

        if (Ring_of_Frost.KnownSpell && Ring_of_Frost.IsSpellUsable &&
            ObjectManager.GetNumberAttackPlayer() > 2 &&
            ObjectManager.Target.GetDistance < 10 && hardmob())
        {
            SpellManager.CastSpellByIDAndPosition(82676, ObjectManager.Target.Position);
        }

    }

    public bool hardmob()
    {
        if (((ObjectManager.Target.MaxHealth * 100) / ObjectManager.Me.MaxHealth) > 140)
        {
            return true;
        }
        return false;
    }
}

public class Mage_Fire
{
    #region InitializeSpell

    // Fire Only
    Spell Pyroblast = new Spell("Pyroblast");
    Spell Blast_Wave = new Spell("Blast Wave");
    Spell Combustion = new Spell("Combustion");
    Spell Dragons_Breath = new Spell("Dragon's Breath");
    Spell Living_Bomb = new Spell("Living Bomb");
    Spell Hot_Streak = new Spell("Hot Streak");
    Spell Flamestrike = new Spell("Flamestrike");
    Spell Scorch = new Spell("Scorch");

    // Survive
    Spell Mana_Shield = new Spell("Mana Shield");
    Spell Mage_Ward = new Spell("Mage Ward");
    Spell Ring_of_Frost = new Spell("Ring of Frost");
    Spell Frost_Nova = new Spell("Frost Nova");
    Spell Blink = new Spell("Blink");
    Spell Counterspell = new Spell("Counterspell");
    Spell Frostbolt = new Spell("Frostbolt");

    // DPS
    Spell Frostfire_Bolt = new Spell("Frostfire Bolt");
    Spell Fireball = new Spell("Fireball");
    Spell Flame_Orb = new Spell("Flame Orb");
    Spell Fire_Blast = new Spell("Fire Blast");
    Spell Arcane_Missiles = new Spell("Arcane Missiles");

    // BUFF & HELPING
    Spell Evocation = new Spell("Evocation");
    Spell Conjure_Refreshment = new Spell("Conjure Refreshment");
    Spell Arcane_Brilliance = new Spell("Arcane Brilliance");
    Spell Remove_Curse = new Spell("Remove Curse");
    Spell Mage_Armor = new Spell("Mage Armor");
    Spell Molten_Armor = new Spell("Molten Armor");
    Spell Conjure_Mana_Gem = new Spell("Conjure Mana Gem");

    // BIG CD
    Spell Mirror_Image = new Spell("Mirror Image");
    Spell Time_Warp = new Spell("Time Warp");
    Spell Invisibility = new Spell("Invisibility");
    Spell Ice_Block = new Spell("Ice Block");
    Spell Cold_Snap = new Spell("Cold Snap");

    // TIMER
    Timer freeze = new Timer(0);
    Timer look = new Timer(0);
    Timer fighttimer = new Timer(0);
    Timer waitfordebuff = new Timer(0);

    // Profession & Racials
    Spell Arcane_Torrent = new Spell("Arcane Torrent");
    Spell Lifeblood = new Spell("Lifeblood");
    Spell Stoneform = new Spell("Stoneform");
    Spell Tailoring = new Spell("Tailoring");
    Spell Leatherworking = new Spell("Leatherworking");
    Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    Spell War_Stomp = new Spell("War Stomp");
    Spell Berserking = new Spell("Berserking");

    #endregion InitializeSpell

    public Mage_Fire()
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

                    if (fighttimer.IsReady && ObjectManager.Target.HealthPercent > 90 && ObjectManager.Me.Target > 0 && ObjectManager.GetNumberAttackPlayer() < 2)
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
        if (ObjectManager.Target.GetDistance > 25 && !Frostfire_Bolt.KnownSpell) SpellManager.CastSpellByIdLUA(116); //Frostbolt.Launch();
        if (ObjectManager.Target.GetDistance > 25) SpellManager.CastSpellByIdLUA(44614); //Frostfirebolt.Launch();

    }

    public void buffoutfight()
    {

        if (Fight.InFight || ObjectManager.Me.IsDeadMe) return;

        if (Evocation.KnownSpell && Evocation.IsSpellUsable &&
            ObjectManager.Me.HealthPercent < 40 ||
            ObjectManager.Me.BarTwoPercentage < 40)
        {
            SpellManager.CastSpellByIdLUA(12051);
            // Evocation.Launch();
        }

        if (!ObjectManager.Me.HaveBuff(79640) &&
            ItemsManager.GetItemCountByIdLUA(58149) == 1)
        {
            Logging.WriteFight("Use Alchi Flask");
            Lua.RunMacroText("/use item:58149");
        }

        if (Arcane_Brilliance.KnownSpell && Arcane_Brilliance.IsSpellUsable &&
            !Arcane_Brilliance.HaveBuff)
        {
            Arcane_Brilliance.Launch();
        }

        if (Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(65499) == 0 &&	// 85
            Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(43523) == 0 &&	// 84-80
            Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(43518) == 0 &&	// 79-74
            Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(65517) == 0 &&	// 73-64
            Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(65516) == 0 &&	// 63-54
            Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(65515) == 0 &&	// 53-44
            Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(65500) == 0)		// 43-38
        {
            Thread.Sleep(100);
            Conjure_Refreshment.Launch();
            Thread.Sleep(200);
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(200);
            }
            Thread.Sleep(100);
        }

        if (Conjure_Mana_Gem.KnownSpell && ItemsManager.GetItemCountByIdLUA(36799) == 0)
        {
            Thread.Sleep(100);
            Conjure_Mana_Gem.Launch();
            Thread.Sleep(200);
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(200);
            }
            Thread.Sleep(100);
        }

        if (Mage_Armor.KnownSpell && Mage_Armor.IsSpellUsable && !Mage_Armor.HaveBuff)
        {
            Mage_Armor.Launch();
        }

        if (!Mage_Armor.KnownSpell && Molten_Armor.KnownSpell && !Molten_Armor.HaveBuff)
        {
            Molten_Armor.Launch();
        }

    }

    public void fight()
    {
        selfheal();
        buffinfight();
        if (ObjectManager.GetNumberAttackPlayer() > 1) fighttimer = new Timer(60000);

        if (Mirror_Image.KnownSpell && Mirror_Image.IsSpellUsable && hardmob() || ObjectManager.GetNumberAttackPlayer() > 1)
        {
            SpellManager.CastSpellByIdLUA(55342);
            // Mirror_Image.Launch();
        }

        if (Berserking.KnownSpell && Berserking.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(1454);
            // Berserking.Launch();
        }

        if (Fireball.IsDistanceGood && Fireball.IsSpellUsable && !ObjectManager.Target.IsTargetingMe)
        {
            SpellManager.CastSpellByIdLUA(133);
            // Fireball.Launch();
            return;
        }

        if (Pyroblast.IsDistanceGood && Pyroblast.IsSpellUsable && ObjectManager.Me.HaveBuff(48108))
        {
            SpellManager.CastSpellByIdLUA(92315);
            // Pyroblast.Launch();
            return;
        }

        if (Fire_Blast.KnownSpell && Fire_Blast.IsSpellUsable && Fire_Blast.IsDistanceGood)
        {
            SpellManager.CastSpellByIdLUA(2136);
            // Fire_Blast.Launch();
        }

        if (Dragons_Breath.KnownSpell && Dragons_Breath.IsSpellUsable && Dragons_Breath.IsDistanceGood && ObjectManager.Target.GetDistance < 6)
        {
            SpellManager.CastSpellByIdLUA(31661);
            // Dragons_Breath.Launch();
        }

        if (Fireball.IsSpellUsable && Fireball.IsDistanceGood && ObjectManager.Target.GetDistance > 5)
        {
            SpellManager.CastSpellByIdLUA(133);
            // Fireball.Launch();
        }

        if (Scorch.KnownSpell && Scorch.IsSpellUsable && Scorch.IsDistanceGood && ObjectManager.Target.GetDistance < 6)
        {
            SpellManager.CastSpellByIdLUA(2948);
            // Scorch.Launch();
        }

        if (Living_Bomb.KnownSpell && Living_Bomb.IsSpellUsable && Living_Bomb.IsDistanceGood && !Living_Bomb.TargetHaveBuff && hardmob())
        {
            SpellManager.CastSpellByIdLUA(44457);
            // Living_Bomb.Launch();
        }

        if (Arcane_Missiles.KnownSpell &&
            Arcane_Missiles.IsSpellUsable &&
            Arcane_Missiles.IsDistanceGood)
        {
            SpellManager.CastSpellByIdLUA(5143);
            // Arcane_Missiles.Launch();
            Thread.Sleep(200);
            while (ObjectManager.Me.IsCast)
            {
                Fight.StopFight();
                MovementManager.StopMove();
                Thread.Sleep(200);
            }
        }

        if (Counterspell.KnownSpell && Counterspell.IsSpellUsable && Counterspell.IsDistanceGood &&
            ObjectManager.Target.IsCast && ObjectManager.Target.HealthPercent > 30)
        {
            SpellManager.CastSpellByIdLUA(2139);
            // Counterspell.Launch();
        }

        if (Arcane_Torrent.KnownSpell && Arcane_Torrent.IsSpellUsable &&
            ObjectManager.Target.IsCast && ObjectManager.Target.GetDistance < 8)
        {
            Arcane_Torrent.Launch();
        }

        if (Flamestrike.KnownSpell && Flamestrike.IsSpellUsable &&
            ObjectManager.Target.GetDistance < 8 &&
            !ObjectManager.Target.HaveBuff(2120) &&
            ObjectManager.GetNumberAttackPlayer() > 2)
        {
            SpellManager.CastSpellByIDAndPosition(2120, ObjectManager.Target.Position);
        }

        if (Flame_Orb.KnownSpell && Flame_Orb.IsDistanceGood && Flame_Orb.IsSpellUsable && hardmob())
        {
            SpellManager.CastSpellByIdLUA(82731);
            // Flame_Orb.Launch();
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

        if ((ObjectManager.GetNumberAttackPlayer() > 1 || hardmob()) &&
            ObjectManager.Me.HealthPercent < 65 &&
            ObjectManager.Target.GetDistance < 5 &&
            War_Stomp.KnownSpell && War_Stomp.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(20549);
            // War_Stomp.Launch();
        }

        if (ObjectManager.Target.HaveBuff(12846) &&
            Combustion.KnownSpell && Combustion.IsSpellUsable && hardmob())
        {
            SpellManager.CastSpellByIdLUA(11129);
            // Combustion.Launch();
        }

        if (Time_Warp.KnownSpell && Time_Warp.IsSpellUsable &&
            !Time_Warp.HaveBuff && hardmob() && ObjectManager.GetNumberAttackPlayer() > 2)
        {
            SpellManager.CastSpellByIdLUA(80353);
            // Time_Warp.Launch();
        }

        if (Blast_Wave.KnownSpell && Blast_Wave.IsSpellUsable && ObjectManager.Target.IsTargetingMe)
        {
            SpellManager.CastSpellByIDAndPosition(11113, ObjectManager.Target.Position);
            return;
        }

        if (Frost_Nova.KnownSpell && Frost_Nova.IsSpellUsable && ObjectManager.Target.GetDistance < 6)
        {
            SpellManager.CastSpellByIdLUA(122);
            // Frost_Nova.Launch();
        }

        if (Blink.KnownSpell && Blink.IsSpellUsable && Fight.InFight && !Frost_Nova.IsSpellUsable &&
            ObjectManager.Target.GetDistance < 6 && ObjectManager.Target.HealthPercent > 30)
        {
            SpellManager.CastSpellByIdLUA(1953);
            // Blink.Launch();	
        }

        if (ObjectManager.Target.GetDistance > 55 && Blink.KnownSpell && Blink.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(1953);
            // Blink.Launch();	
        }

    }

    private void selfheal()
    {

        if (Mage_Ward.KnownSpell && Mage_Ward.IsSpellUsable && ObjectManager.Target.IsCast &&
            ObjectManager.Target.IsTargetingMe && !Counterspell.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(543);
            // Mage_Ward.Launch();
        }

        if (ObjectManager.Me.BarTwoPercentage < 40 &&
            ItemsManager.GetItemCountByIdLUA(36799) > 0)
        {
            Lua.RunMacroText("/use item:36799");
        }


        if (!Mana_Shield.HaveBuff && ObjectManager.Me.HealthPercent < 85 &&
            Mana_Shield.KnownSpell && Mana_Shield.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(1463);
            // Mana_Shield.Launch();
        }

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

        if (Ice_Block.KnownSpell && Ice_Block.IsSpellUsable && ObjectManager.Me.HealthPercent > 0 &&
            ObjectManager.Me.HealthPercent < 25 && ObjectManager.GetNumberAttackPlayer() > 1 &&
            Fight.InFight)
        {
            SpellManager.CastSpellByIdLUA(45438);
            // Ice_Block.Launch();
        }

        if (Evocation.KnownSpell && Evocation.IsSpellUsable &&
            ObjectManager.Me.HealthPercent > 0 &&
            ObjectManager.Me.HealthPercent < 30 ||
            ObjectManager.Me.BarTwoPercentage < 20)
        {
            SpellManager.CastSpellByIdLUA(12051);
            // Evocation.Launch();
        }

        if (Ring_of_Frost.KnownSpell && Ring_of_Frost.IsSpellUsable &&
            ObjectManager.GetNumberAttackPlayer() > 2 &&
            ObjectManager.Target.GetDistance < 10 && hardmob())
        {
            SpellManager.CastSpellByIDAndPosition(82676, ObjectManager.Target.Position);
        }

    }

    public bool hardmob()
    {
        if (((ObjectManager.Target.MaxHealth * 100) / ObjectManager.Me.MaxHealth) > 120)
        {
            return true;
        }
        return false;
    }
}
#endregion

#region Warlock
public class Demo
{
    #region InitializeSpell

    Spell Immolate = new Spell("Immolate");
    Spell Soul_Fire = new Spell("Soul Fire");
    Spell Bane_of_Doom = new Spell("Bane of Doom");
    Spell Shadow_Bolt = new Spell("Shadow Bolt");
    Spell Shadowflame = new Spell("Shadowflame");
    Spell Incinerate = new Spell("Incinerate");
    Spell Health_Funnel = new Spell("Health Funnel");
    Spell Life_Tap = new Spell("Life Tap");
    Spell Drain_Soul = new Spell("Drain Soul");
    Spell Corruption = new Spell("Corruption");
    Spell Curse_of_the_Elements = new Spell("Curse of the Elements");
    Spell Drain_Life = new Spell("Drain Life");
    Spell Metamorphosis = new Spell("Metamorphosis");
    Spell Immolation_Aura = new Spell("Immolation Aura");
    Spell Demon_Soul = new Spell("Demon Soul");
    Spell Demon_Leap = new Spell("Demon Leap");
    Spell Summon_Imp = new Spell("Summon Imp");
    Spell Summon_Felguard = new Spell("Summon Felguard");
    Spell Summon_Infernal = new Spell("Summon Infernal");
    Spell Death_Coil = new Spell("Death Coil");
    Spell Soul_Link = new Spell("Soul Link");
    Spell Curse_of_Weakness = new Spell("Curse of Weakness");
    Spell Curse_of_Tongues = new Spell("Curse of Tongues");
    Spell Hand_of_Guldan = new Spell("Hand of Gul'dan");
    Spell Curse_of_Guldan = new Spell("Curse of Gul'dan");
    Spell Fel_Domination = new Spell("Fel Domination");
    Spell Soul_Harvest = new Spell("Soul Harvest");
    Spell Create_Healthstone = new Spell("Create Healthstone");
    Spell Fel_Armor = new Spell("Fel Armor");
    Spell Demon_Armor = new Spell("Demon Armor");
    Spell Molten_Core = new Spell("Molten Core");
    Spell Soulburn = new Spell("Soulburn");
    Spell Dark_Intent = new Spell("Dark Intent");
    Timer look = new Timer(0);
    Timer petchill = new Timer(0);
    Timer fighttimer = new Timer(0);
    Timer waitfordebuff = new Timer(0);
    Timer mountchill = new Timer(0);

    // profession & racials
    Spell Arcane_Torrent = new Spell("Arcane Torrent");
    Spell Lifeblood = new Spell("Lifeblood");
    Spell Stoneform = new Spell("Stoneform");
    Spell Tailoring = new Spell("Tailoring");
    Spell Leatherworking = new Spell("Leatherworking");
    Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    Spell War_Stomp = new Spell("War Stomp");
    Spell Berserking = new Spell("Berserking");

    #endregion InitializeSpell

    public Demo()
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
            if (ObjectManager.Me.IsMounted) mountchill = new Timer(2000);
            Thread.Sleep(350);
        }

    }

    public void pull()
    {
        if (hardmob()) Logging.WriteFight(" -  Pull Hard Mob - ");
        if (!hardmob()) Logging.WriteFight(" -  Pull Easy Mob - ");
        pet();
        Lua.RunMacroText("/petattack");
        petchill = new Timer(3000);
        fighttimer = new Timer(60000);
    }

    public void buffoutfight()
    {

        if (Fight.InFight || ObjectManager.Me.IsDeadMe) return;

        pet();

        if (ObjectManager.Pet.HealthPercent > 0 && !Dark_Intent.HaveBuff && Dark_Intent.KnownSpell)
        {
            Logging.WriteFight("Buff Dark Intent");
            Lua.RunMacroText("/cast [@pet] Dark Intent");
            Lua.RunMacroText("/cast [@pet] Finstere Absichten");
            Lua.RunMacroText("/cast [@pet] Sombre intention");
            Lua.RunMacroText("/cast [@pet] Узы Тьмы");
            Lua.RunMacroText("/cast [@pet] Propósito oscuro");
            Lua.RunMacroText("/cast [@pet] Intenção Sombria");
        }

        if (!ObjectManager.Me.HaveBuff(79640) &&
            ItemsManager.GetItemCountByIdLUA(58149) == 1)
        {
            Logging.WriteFight("Use Alchi Flask");
            Lua.RunMacroText("/use item:58149");
        }

        if (ItemsManager.GetItemCountByIdLUA(5512) == 0 && Create_Healthstone.KnownSpell && Create_Healthstone.IsSpellUsable)
        {
            Logging.WriteFight("Create Healthstone");
            Thread.Sleep(200);
            Create_Healthstone.Launch();
            Thread.Sleep(200);
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(100);
                Thread.Sleep(100);
            }
        }

        if (ObjectManager.Me.HealthPercent < 65 && Soul_Harvest.IsSpellUsable && Soul_Harvest.KnownSpell)
        {
            Thread.Sleep(200);
            Fight.StopFight();
            MovementManager.StopMove();
            if (ObjectManager.Me.BarTwoPercentage < 50) SpellManager.CastSpellByIdLUA(1454);
            Soul_Harvest.Launch();
            Thread.Sleep(200);
            Fight.StopFight();
            MovementManager.StopMove();
            while (ObjectManager.Me.IsCast)
            {
                Fight.StopFight();
                MovementManager.StopMove();
                Thread.Sleep(200);
            }
        }

        if (Demon_Leap.IsSpellUsable && Demon_Leap.KnownSpell)
        {
            SpellManager.CastSpellByIdLUA(54785);
            // Demon_Leap.Launch();
            return;
        }

    }

    public void fight()
    {
        selfheal();
        pet();
        buffinfight();
        if (ObjectManager.GetNumberAttackPlayer() > 1) fighttimer = new Timer(60000);

        if (ObjectManager.Target.IsTargetingMe)
        {
            SpellManager.CastSpellByIdLUA(89766);
        }

        if (petchill.IsReady)
        {
            SpellManager.CastSpellByIdLUA(89751);
        }

        if (ObjectManager.Me.HealthPercent < 20 &&
            ItemsManager.GetItemCountByIdLUA(5512) == 1)
        {
            Lua.RunMacroText("/use item:5512");
            Logging.WriteFight(" - Healthstone Used - ");
            return;
        }

        if (ObjectManager.Me.HaveBuff(63167))
        {
            SpellManager.CastSpellByIdLUA(6353);
            // Soul_Fire.Launch();
            return;
        }

        if (!ObjectManager.Me.HaveBuff(63167) &&
            ObjectManager.Me.HealthPercent > 50 &&
            ObjectManager.Target.HealthPercent < 26 &&
            ObjectManager.GetNumberAttackPlayer() < 2 &&
            Drain_Soul.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(1120);
            // Drain_Soul.Launch();
            return;
        }

        if (Soulburn.KnownSpell && Soulburn.IsSpellUsable && Soul_Fire.IsSpellUsable && Soul_Fire.KnownSpell && Soul_Fire.IsDistanceGood)
        {
            SpellManager.CastSpellByIdLUA(74434);
            // Soulburn.Launch();
            SpellManager.CastSpellByIdLUA(6353);
            // Soul_Fire.Launch();
        }

        if (!Curse_of_the_Elements.TargetHaveBuff && hardmob() &&
            ObjectManager.Target.HealthPercent < 100 &&
            ObjectManager.Target.HealthPercent > 40 &&
            Curse_of_the_Elements.KnownSpell &&
            Curse_of_the_Elements.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(1490);
            // Curse_of_the_Elements.Launch();
            return;
        }

        selfheal();

        if (ObjectManager.Me.HealthPercent < 85 &&
            Death_Coil.KnownSpell &&
            Death_Coil.IsDistanceGood &&
            Death_Coil.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(6789);
            // Death_Coil.Launch();
            return;
        }

        if (ObjectManager.Target.GetDistance < 4 && Shadowflame.KnownSpell && Shadowflame.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(47897);
            // Shadowflame.Launch();
            nManager.Wow.Helpers.Keybindings.DownKeybindings(Keybindings.JUMP);
            Thread.Sleep(100);
            nManager.Wow.Helpers.Keybindings.DownKeybindings(Keybindings.MOVEBACKWARD);
            Thread.Sleep(1000);
            nManager.Wow.Helpers.Keybindings.UpKeybindings(Keybindings.JUMP);
            nManager.Wow.Helpers.Keybindings.UpKeybindings(Keybindings.MOVEBACKWARD);
            return;
        }

        if (ObjectManager.Me.HealthPercent > 79 && ObjectManager.Me.BarTwoPercentage < 50 && Life_Tap.KnownSpell)
        {
            SpellManager.CastSpellByIdLUA(1454);
            // Life_Tap.Launch();
            return;
        }

        if (Berserking.KnownSpell && Berserking.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(1454);
            // Berserking.Launch();
        }

        if (!Incinerate.KnownSpell && Shadow_Bolt.IsSpellUsable) Shadow_Bolt.Launch();

        if (!Incinerate.KnownSpell && !Corruption.TargetHaveBuff && Corruption.IsSpellUsable && ObjectManager.Target.HealthPercent > 40) Corruption.Launch();

        if (!Incinerate.KnownSpell && !Bane_of_Doom.TargetHaveBuff && Bane_of_Doom.IsSpellUsable && ObjectManager.Target.HealthPercent > 40) Bane_of_Doom.Launch();

        if (!Curse_of_Guldan.TargetHaveBuff &&
            Hand_of_Guldan.KnownSpell &&
            Hand_of_Guldan.IsDistanceGood &&
            Hand_of_Guldan.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(71521);
            // Hand_of_Guldan.Launch();
            return;
        }

        if (ObjectManager.Target.HaveBuff(348) &&
            Incinerate.KnownSpell &&
            Incinerate.IsDistanceGood &&
            Incinerate.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(29722);
            // Incinerate.Launch();
            return;
        }

        if (!Immolate.TargetHaveBuff &&
            Immolate.KnownSpell &&
            Immolate.IsDistanceGood &&
            Immolate.IsSpellUsable &&
            waitfordebuff.IsReady &&
            ObjectManager.Target.HealthPercent > 40)
        {
            SpellManager.CastSpellByIdLUA(348);
            // Immolate.Launch();
            waitfordebuff = new Timer(2000);
            return;
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

        if (Fel_Armor.KnownSpell && !Fel_Armor.HaveBuff && Fel_Armor.IsSpellUsable)
        {
            Fel_Armor.Launch();
        }

        if (Soul_Link.KnownSpell && !Soul_Link.HaveBuff && Soul_Link.IsSpellUsable)
        {
            Soul_Link.Launch();
        }
        else if (Demon_Armor.KnownSpell && !Fel_Armor.KnownSpell && !Demon_Armor.HaveBuff && Demon_Armor.IsSpellUsable)
        {
            Demon_Armor.Launch();
        }

        if (Summon_Infernal.KnownSpell && Summon_Infernal.IsDistanceGood && Summon_Infernal.IsSpellUsable && hardmob())
        {
            SpellManager.CastSpellByIDAndPosition(1122, ObjectManager.Target.Position);
        }

        if (Metamorphosis.KnownSpell && Metamorphosis.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(59672);
            // Metamorphosis.Launch();
        }

        if (!Metamorphosis.HaveBuff && Demon_Soul.KnownSpell && Demon_Soul.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(77801);
            // Demon_Soul.Launch();
        }

        if (ObjectManager.GetNumberAttackPlayer() > 2 &&
            Immolation_Aura.KnownSpell && Immolation_Aura.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(50589);
            // Immolation_Aura.Launch();
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

        if (Arcane_Torrent.KnownSpell && Arcane_Torrent.IsSpellUsable &&
            ObjectManager.Target.IsCast && ObjectManager.Target.GetDistance < 8)
        {
            Arcane_Torrent.Launch();
        }

        if (ObjectManager.Me.HealthPercent < 50 &&
            Drain_Life.KnownSpell &&
            Drain_Life.IsDistanceGood &&
            Drain_Life.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(689);
            // Drain_Life.Launch();
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(100);
                Thread.Sleep(100);
            }
            return;
        }

    }

    private void pet()
    {
        if (ObjectManager.Me.IsMounted || !mountchill.IsReady) return;

        if (Health_Funnel.KnownSpell && Health_Funnel.IsSpellUsable && ObjectManager.Pet.HealthPercent > 0 && ObjectManager.Pet.HealthPercent < 50)
        {
            SpellManager.CastSpellByIdLUA(755);
            // Health_Funnel.Launch();
            while (ObjectManager.Me.IsCast)
            {
                if (ObjectManager.Pet.HealthPercent > 80 || ObjectManager.Pet.IsDead) break;
                Thread.Sleep(100);
            }
        }

        if ((ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0) &&
            !ObjectManager.Me.IsMounted && !ObjectManager.Me.IsDeadMe)
        {
            Logging.WriteFight(" - PET DEAD - ");
            if (Summon_Felguard.KnownSpell && Summon_Felguard.IsSpellUsable)
            {
                if (Soulburn.KnownSpell && Soulburn.IsSpellUsable)
                {
                    SpellManager.CastSpellByIdLUA(74434);
                    // Soulburn.Launch();
                }
                Summon_Felguard.Launch();
            }
            if (!Summon_Felguard.KnownSpell) Summon_Imp.Launch();
        }
    }

    public bool hardmob()
    {
        if (((ObjectManager.Target.MaxHealth * 100) / ObjectManager.Me.MaxHealth) > 120)
        {
            return true;
        }
        return false;
    }
}
public class Affli
{
    #region InitializeSpell

    Spell Immolate = new Spell("Immolate");
    Spell Soul_Fire = new Spell("Soul Fire");
    Spell Bane_of_Doom = new Spell("Bane of Doom");
    Spell Shadow_Bolt = new Spell("Shadow Bolt");
    Spell Shadowflame = new Spell("Shadowflame");
    Spell Incinerate = new Spell("Incinerate");
    Spell Health_Funnel = new Spell("Health Funnel");
    Spell Life_Tap = new Spell("Life Tap");
    Spell Drain_Soul = new Spell("Drain Soul");
    Spell Corruption = new Spell("Corruption");
    Spell Curse_of_the_Elements = new Spell("Curse of the Elements");
    Spell Drain_Life = new Spell("Drain Life");
    Spell Demon_Soul = new Spell("Demon Soul");
    Spell Soul_Swap = new Spell("Soul Swap");
    Spell Summon_Imp = new Spell("Summon Imp");
    Spell Summon_Felhunter = new Spell("Summon Felhunter");
    Spell Summon_Infernal = new Spell("Summon Infernal");
    Spell Death_Coil = new Spell("Death Coil");
    Spell Curse_of_Weakness = new Spell("Curse of Weakness");
    Spell Curse_of_Tongues = new Spell("Curse of Tongues");
    Spell Hand_of_Guldan = new Spell("Hand of Gul'dan");
    Spell Curse_of_Guldan = new Spell("Curse of Gul'dan");
    Spell Soul_Harvest = new Spell("Soul Harvest");
    Spell Create_Healthstone = new Spell("Create Healthstone");
    Spell Fel_Armor = new Spell("Fel Armor");
    Spell Demon_Armor = new Spell("Demon Armor");
    Spell Soulburn = new Spell("Soulburn");
    Spell Dark_Intent = new Spell("Dark Intent");
    Spell Haunt = new Spell("Haunt");
    Spell Unstable_Affliction = new Spell("Unstable Affliction");
    Spell Bane_of_Agony = new Spell("Bane of Agony");
    Spell Shadow_Trance = new Spell("Shadow Trance");
    Timer look = new Timer(0);
    Timer fighttimer = new Timer(0);
    Timer waitfordebuff = new Timer(0);
    Timer mountchill = new Timer(0);

    // profession & racials
    Spell Arcane_Torrent = new Spell("Arcane Torrent");
    Spell Lifeblood = new Spell("Lifeblood");
    Spell Stoneform = new Spell("Stoneform");
    Spell Tailoring = new Spell("Tailoring");
    Spell Leatherworking = new Spell("Leatherworking");
    Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    Spell War_Stomp = new Spell("War Stomp");
    Spell Berserking = new Spell("Berserking");

    #endregion InitializeSpell

    public Affli()
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
            if (ObjectManager.Me.IsMounted) mountchill = new Timer(2000);
            Thread.Sleep(350);
        }
    }

    public void pull()
    {
        if (hardmob()) Logging.WriteFight(" -  Pull Hard Mob - ");
        if (!hardmob()) Logging.WriteFight(" -  Pull Easy Mob - ");
        pet();
        fighttimer = new Timer(60000);
        Lua.RunMacroText("/petattack");
    }

    public void buffoutfight()
    {

        if (Fight.InFight || ObjectManager.Me.IsDeadMe) return;

        pet();

        if (ObjectManager.Pet.HealthPercent > 0 && !Dark_Intent.HaveBuff && Dark_Intent.KnownSpell)
        {
            Logging.WriteFight("Buff Dark Intent");
            Lua.RunMacroText("/cast [@pet] Dark Intent");
            Lua.RunMacroText("/cast [@pet] Finstere Absichten");
            Lua.RunMacroText("/cast [@pet] Sombre intention");
            Lua.RunMacroText("/cast [@pet] Узы Тьмы");
            Lua.RunMacroText("/cast [@pet] Propósito oscuro");
            Lua.RunMacroText("/cast [@pet] Intenção Sombria");
        }

        if (!ObjectManager.Me.HaveBuff(79640) &&
            ItemsManager.GetItemCountByIdLUA(58149) == 1)
        {
            Logging.WriteFight("Use Alchi Flask");
            Lua.RunMacroText("/use item:58149");
        }

        if (ItemsManager.GetItemCountByIdLUA(5512) == 0 && Create_Healthstone.KnownSpell && Create_Healthstone.IsSpellUsable)
        {
            Logging.WriteFight(" - Create Healthstone - ");
            Thread.Sleep(200);
            Create_Healthstone.Launch();
            Thread.Sleep(200);
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(200);
            }
        }

        if (ObjectManager.Me.HealthPercent < 65 && Soul_Harvest.IsSpellUsable && Soul_Harvest.KnownSpell)
        {
            Thread.Sleep(200);
            Fight.StopFight();
            MovementManager.StopMove();
            if (ObjectManager.Me.BarTwoPercentage < 50) SpellManager.CastSpellByIdLUA(1454);
            Soul_Harvest.Launch();
            Thread.Sleep(200);
            Fight.StopFight();
            MovementManager.StopMove();
            while (ObjectManager.Me.IsCast)
            {
                Fight.StopFight();
                MovementManager.StopMove();
                Thread.Sleep(200);
            }
        }

    }

    public void fight()
    {
        selfheal();
        pet();
        buffinfight();
        if (ObjectManager.GetNumberAttackPlayer() > 1) fighttimer = new Timer(60000);

        if (ObjectManager.Me.HealthPercent < 20 &&
            ItemsManager.GetItemCountByIdLUA(5512) == 1)
        {
            Lua.RunMacroText("/use item:5512");
            Logging.WriteFight(" - Healthstone Used - ");
            return;
        }

        if (Soul_Swap.HaveBuff &&
            Soul_Swap.IsDistanceGood)
        {
            SpellManager.CastSpellByIdLUA(86213);
        }

        if (ObjectManager.Me.HaveBuff(17941) && Shadow_Bolt.IsDistanceGood && Shadow_Bolt.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(686);
            // Shadow_Bolt.Launch();
            return;
        }

        if (!ObjectManager.Me.HaveBuff(63167) &&
            ObjectManager.Me.HealthPercent > 50 &&
            ObjectManager.Target.HealthPercent < 26 &&
            ObjectManager.GetNumberAttackPlayer() < 2 &&
            Drain_Soul.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(1120);
            // Drain_Soul.Launch();
            return;
        }

        if (!Curse_of_the_Elements.TargetHaveBuff && hardmob() &&
            ObjectManager.Target.HealthPercent < 100 &&
            ObjectManager.Target.HealthPercent > 40 &&
            Curse_of_the_Elements.KnownSpell &&
            Curse_of_the_Elements.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(1490);
            // Curse_of_the_Elements.Launch();
            return;
        }

        selfheal();

        if (ObjectManager.Me.HealthPercent < 85 &&
            Death_Coil.KnownSpell &&
            Death_Coil.IsDistanceGood &&
            Death_Coil.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(6789);
            // Death_Coil.Launch();
            return;
        }

        if (ObjectManager.Target.GetDistance < 4 && Shadowflame.KnownSpell && Shadowflame.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(47897);
            // Shadowflame.Launch();
            nManager.Wow.Helpers.Keybindings.DownKeybindings(Keybindings.JUMP);
            Thread.Sleep(100);
            nManager.Wow.Helpers.Keybindings.DownKeybindings(Keybindings.MOVEBACKWARD);
            Thread.Sleep(1000);
            nManager.Wow.Helpers.Keybindings.UpKeybindings(Keybindings.JUMP);
            nManager.Wow.Helpers.Keybindings.UpKeybindings(Keybindings.MOVEBACKWARD);
            return;
        }

        if (ObjectManager.Me.HealthPercent > 79 && ObjectManager.Me.BarTwoPercentage < 50 && Life_Tap.KnownSpell)
        {
            SpellManager.CastSpellByIdLUA(1454);
            // Life_Tap.Launch();
            return;
        }

        if (Berserking.KnownSpell && Berserking.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(1454);
            // Berserking.Launch();
        }

        if (!Haunt.TargetHaveBuff && Haunt.KnownSpell && Haunt.IsDistanceGood && Haunt.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(48181);
            // Haunt.Launch();
            return;
        }

        if (Soul_Swap.KnownSpell &&
            Soul_Swap.IsDistanceGood &&
            Soul_Swap.IsSpellUsable &&
            (ObjectManager.Target.HealthPercent < 35 && !hardmob())
            || hardmob())
        {
            SpellManager.CastSpellByIdLUA(86121);
            // Soul_Swap.Launch();
        }

        if (!Corruption.TargetHaveBuff && Corruption.KnownSpell && Corruption.IsDistanceGood && Corruption.IsSpellUsable)
        {
            Corruption.Launch();
            return;
        }

        if (!Bane_of_Agony.TargetHaveBuff && Bane_of_Agony.KnownSpell && Bane_of_Agony.IsDistanceGood && hardmob() && Bane_of_Agony.IsSpellUsable)
        {
            Bane_of_Agony.Launch();
            return;
        }

        if (!Unstable_Affliction.TargetHaveBuff && Unstable_Affliction.KnownSpell && Unstable_Affliction.IsDistanceGood && Unstable_Affliction.IsSpellUsable && waitfordebuff.IsReady)
        {
            waitfordebuff = new Timer(2500);
            SpellManager.CastSpellByIdLUA(30108);
            // Unstable_Affliction.Launch();
            return;
        }

        if (Drain_Life.KnownSpell &&
            Drain_Life.IsDistanceGood &&
            Drain_Life.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(689);
            // Drain_Life.Launch();
            Thread.Sleep(200);
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(100);
                Thread.Sleep(100);
            }
            return;
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

        if (Fel_Armor.KnownSpell && !Fel_Armor.HaveBuff && Fel_Armor.IsSpellUsable)
        {
            Fel_Armor.Launch();
        }

        if (Demon_Armor.KnownSpell && !Fel_Armor.KnownSpell && !Demon_Armor.HaveBuff && Demon_Armor.IsSpellUsable)
        {
            Demon_Armor.Launch();
        }

        if (Summon_Infernal.KnownSpell && Summon_Infernal.IsDistanceGood && Summon_Infernal.IsSpellUsable && hardmob())
        {
            SpellManager.CastSpellByIDAndPosition(1122, ObjectManager.Target.Position);
        }

        if (Demon_Soul.KnownSpell && Demon_Soul.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(77801);
            // Demon_Soul.Launch();
        }

    }

    private void selfheal()
    {

        if (ObjectManager.Me.HealthPercent < 60 &&
            Lifeblood.KnownSpell && Lifeblood.IsSpellUsable)
        {
            Lifeblood.Launch();
        }

        if (ObjectManager.Me.HealthPercent < 60 &&
            Gift_of_the_Naaru.KnownSpell && Gift_of_the_Naaru.IsSpellUsable)
        {
            Gift_of_the_Naaru.Launch();
        }

        if (Arcane_Torrent.KnownSpell && Arcane_Torrent.IsSpellUsable &&
            ObjectManager.Target.IsCast && ObjectManager.Target.GetDistance < 8)
        {
            Arcane_Torrent.Launch();
        }

        if (ObjectManager.Me.HealthPercent < 50 &&
            Drain_Life.KnownSpell &&
            Drain_Life.IsDistanceGood &&
            Drain_Life.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(689);
            // Drain_Life.Launch();
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(200);
            }
            return;
        }

    }

    private void pet()
    {

        if (ObjectManager.Me.IsMounted || !mountchill.IsReady) return;

        if (Health_Funnel.KnownSpell)
            if (ObjectManager.Pet.HealthPercent > 0 && ObjectManager.Pet.HealthPercent < 50 &&
                Health_Funnel.IsSpellUsable)
            {
                SpellManager.CastSpellByIdLUA(755);
                // Health_Funnel.Launch();
                while (ObjectManager.Me.IsCast)
                {
                    if (ObjectManager.Pet.HealthPercent > 85 || ObjectManager.Pet.IsDead)
                        break;
                    Thread.Sleep(100);
                }
            }

        if ((ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0) &&
            !ObjectManager.Me.IsMounted && !ObjectManager.Me.IsDeadMe)
        {
            Logging.WriteFight(" - PET DEAD - ");
            if (Soulburn.KnownSpell && Soulburn.IsSpellUsable)
            {
                SpellManager.CastSpellByIdLUA(74434);
                // Soulburn.Launch();
            }
            Summon_Felhunter.Launch();
        }

    }

    public bool hardmob()
    {
        if (((ObjectManager.Target.MaxHealth * 100) / ObjectManager.Me.MaxHealth) > 120)
        {
            return true;
        }
        return false;
    }
}
#endregion

#region Druid
public class Balance
{
    Int32 firemode;
    #region InitializeSpell

    // BALANCE ONLY
    Spell Starfall = new Spell("Starfall");
    Spell Typhoon = new Spell("Typhoon");
    Spell Moonkin_Form = new Spell("Moonkin Form");
    Spell Force_of_Nature = new Spell("Force of Nature");
    Spell Solar_Beam = new Spell("Solar Beam");
    Spell Starsurge = new Spell("Starsurge");
    Spell Sunfire = new Spell("Sunfire");

    // DPS
    Spell Insect_Swarm = new Spell("Insect Swarm");
    Spell Starfire = new Spell("Starfire");
    Spell Moonfire = new Spell("Moonfire");
    Spell Wrath = new Spell("Wrath");

    // HEAL
    Spell Regrowth = new Spell("Regrowth");
    Spell Rejuvenation = new Spell("Rejuvenation");
    Spell Nourish = new Spell("Nourish");
    Spell Lifebloom = new Spell("Lifebloom");
    Spell Healing_Touch = new Spell("Healing Touch");

    // BUFF & HELPING
    Spell Innervate = new Spell("Innervate");
    Spell Mark_of_the_Wild = new Spell("Mark of the Wild");
    Spell Barkskin = new Spell("Barkskin");

    // TIMER
    Timer look = new Timer(0);
    Timer fighttimer = new Timer(0);
    Timer slowbloom = new Timer(0);

    // profession & racials
    Spell Arcane_Torrent = new Spell("Arcane Torrent");
    Spell Lifeblood = new Spell("Lifeblood");
    Spell Stoneform = new Spell("Stoneform");
    Spell Tailoring = new Spell("Tailoring");
    Spell Leatherworking = new Spell("Leatherworking");
    Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    Spell War_Stomp = new Spell("War Stomp");
    Spell Berserking = new Spell("Berserking");

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

        if (ObjectManager.Me.HaveBuff(93400) && Starsurge.IsDistanceGood && Starsurge.IsSpellUsable && ObjectManager.Target.IsTargetingMe)
        {
            SpellManager.CastSpellByIdLUA(78674);
            // Starsurge.Launch();
        }

        if (Moonfire.KnownSpell && Moonfire.IsDistanceGood && Moonfire.IsSpellUsable && !Moonfire.TargetHaveBuff && starfirespam())
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

        if (Insect_Swarm.KnownSpell && Insect_Swarm.IsDistanceGood && Insect_Swarm.IsSpellUsable && !Insect_Swarm.TargetHaveBuff && hardmob())
        {
            SpellManager.CastSpellByIdLUA(5570);
            // Insect_Swarm.Launch();
        }

        if (Wrath.KnownSpell && Wrath.IsSpellUsable && Wrath.IsDistanceGood && wrathspam() && (ObjectManager.Target.HaveBuff(93402) || Moonfire.TargetHaveBuff))
        {
            SpellManager.CastSpellByIdLUA(5176);
            // Wrath.Launch();
        }

        if (Starfire.KnownSpell && Starfire.IsSpellUsable && Starfire.IsDistanceGood && starfirespam() && (ObjectManager.Target.HaveBuff(93402) || Moonfire.TargetHaveBuff))
        {
            SpellManager.CastSpellByIdLUA(2912);
            // Starfire.Launch();
        }

        if (Berserking.KnownSpell && Berserking.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(1454);
            // Berserking.Launch();
        }

        if (Force_of_Nature.KnownSpell && Force_of_Nature.IsSpellUsable && Force_of_Nature.IsDistanceGood && (hardmob() || ObjectManager.GetNumberAttackPlayer() > 1))
        {
            SpellManager.CastSpellByIDAndPosition(33831, ObjectManager.Target.Position);
        }

        if (Typhoon.KnownSpell && Typhoon.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() > 1 && ObjectManager.Target.GetDistance < 40)
        {
            SpellManager.CastSpellByIdLUA(50516);
            // Typhoon.Launch();
        }

        if (Starfall.KnownSpell && Starfall.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() > 1 && ObjectManager.Target.GetDistance < 40)
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

        if (Innervate.KnownSpell && Innervate.IsSpellUsable && ObjectManager.Me.BarTwoPercentage < 40)
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

        if (Arcane_Torrent.KnownSpell && Arcane_Torrent.IsSpellUsable &&
            ObjectManager.Target.IsCast && ObjectManager.Target.GetDistance < 8)
        {
            Arcane_Torrent.Launch();
        }

        if (Solar_Beam.KnownSpell && Solar_Beam.IsSpellUsable && Solar_Beam.IsDistanceGood && ObjectManager.Target.IsCast)
        {
            SpellManager.CastSpellByIdLUA(78675);
            // Solar_Beam.Launch();
        }

        if (ObjectManager.Me.HealthPercent < 60 && ObjectManager.Me.BarTwoPercentage < 25 && Regrowth.KnownSpell && Regrowth.IsSpellUsable && !Regrowth.HaveBuff)
        {
            if (Barkskin.KnownSpell && Barkskin.IsSpellUsable) Barkskin.Launch();
            SpellManager.CastSpellByIdLUA(8936);
            // Regrowth.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 50 && ObjectManager.Me.BarTwoPercentage > 25)
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

                if (Regrowth.HaveBuff && Regrowth.KnownSpell && Rejuvenation.HaveBuff && Rejuvenation.KnownSpell && Nourish.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() < 2)
                {
                    SpellManager.CastSpellByIdLUA(50464);
                    // Nourish.Launch();
                }

                if (Regrowth.HaveBuff && Regrowth.KnownSpell && Rejuvenation.HaveBuff && Rejuvenation.KnownSpell && Healing_Touch.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() > 1)
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
                if (ObjectManager.Me.BarTwoPercentage < 10) return;
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
        if (((ObjectManager.Target.MaxHealth * 100) / ObjectManager.Me.MaxHealth) > 110)
        {
            return true;
        }
        return false;
    }
}
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
        // CheckForm();
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
#endregion

#region Paladin
public class Paladin_Holy
{
    #region Professions & Racial
    private readonly Spell Arcane_Torrent = new Spell("Arcane Torrent");
    private readonly Spell Lifeblood = new Spell("Lifeblood");
    private readonly Spell Stoneform = new Spell("Stoneform");
    private readonly Spell Tailoring = new Spell("Tailoring");
    private readonly Spell Leatherworking = new Spell("Leatherworking");
    private readonly Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    private readonly Spell War_Stomp = new Spell("War Stomp");
    private readonly Spell Berserking = new Spell("Berserking");
    #endregion

    #region Paladin Seals & Buffs
    private readonly Spell SealOfTheRighteousness = new Spell("Seal of Righteousness");
    private readonly Spell SealOfTruth = new Spell("Seal of Truth");
    private readonly Spell SealOfInsight = new Spell("Seal of Insight");
    private readonly Spell SealOfJustice = new Spell("Seal of Justice");
    private readonly Spell BlessingOfMight = new Spell("Blessing of Might");
    private readonly Spell BlessingOfKings = new Spell("Blessing of Kings");
    #endregion

    #region Offensive Spell
    private readonly Spell HammerOfJustice = new Spell("Hammer of Justice");
    private readonly Spell HammerOfWrath = new Spell("Hammer of Wrath");
    private readonly Spell Judgment = new Spell("Judgment");
    private readonly Spell HolyShock = new Spell("Holy Shock");
    private readonly Spell Exorcism = new Spell("Exorcism");
    #endregion

    #region Offensive Cooldown
    private readonly Spell Inquisition = new Spell("Inquisition");
    private readonly Spell GuardianOfAncientKings = new Spell("Guardian of Ancient Kings");
    Timer BurstTime = new Timer(0);
    private readonly Spell HolyAvenger = new Spell("HolyAvenger"); 
    private readonly Spell AvengingWrath = new Spell("Avenging Wrath");
    #endregion

    #region Defensive Cooldown
	private readonly Spell DevotionAura = new Spell("Devotion Aura");
    private readonly Spell DivineProtection = new Spell("Divine Protection");
    private readonly Spell DivineShield = new Spell("Divine Shield");
    private readonly Spell HandOfProtection = new Spell("Hand Of Protection");
    // 25771 = Forbearance
    #endregion
        
    #region Healing Spell
    private readonly Spell DivinePlea = new Spell("Divine Plea");
    private readonly Spell DivineLight = new Spell("Divine Light");
    private readonly Spell HolyRadiance = new Spell("Holy Radiance");
    private readonly Spell FlashOfLight = new Spell("Flash of Light");
    private readonly Spell HolyLight = new Spell("Holy Light");
    private readonly Spell LayOnHands = new Spell("Lay on Hands");
    private readonly Spell WorldOfGlory = new Spell("Word of Glory");
    #endregion

    public Paladin_Holy()
    {
        Main.range = 20f;

        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            try
            {
                if (!ObjectManager.Me.IsMounted)
                {
                    Patrolling();

                    if (Fight.InFight && ObjectManager.Me.Target > 0)
                    {
                        if (ObjectManager.Me.Target != lastTarget && Judgment.IsDistanceGood)
                        {
                            Pull();
                            lastTarget = ObjectManager.Me.Target;
                        }

                        Combat();
                    }
                }
            }
            catch
            {
            }
            Thread.Sleep(50);
        }
    }

    private void Pull()
    {
        if (Judgment.KnownSpell && Judgment.IsDistanceGood && Judgment.IsSpellUsable)
        {
            Judgment.Launch();
        }
    }

    private void Combat()
    {
        DPS_Cycle();

        Heal();

        DPS_Burst();
    }

    private void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            Seal();
            Blessing();
        }
    }

    private void Seal()
    {
        if (ObjectManager.Me.IsMounted)
            return;
        if (SealOfInsight.KnownSpell)
        {
            if (!SealOfInsight.HaveBuff && SealOfInsight.IsSpellUsable)
            {
                SealOfInsight.Launch();
            }
        }
        else if (SealOfTruth.KnownSpell)
        {
            if (!SealOfTruth.HaveBuff && SealOfTruth.IsSpellUsable)
            {
                SealOfTruth.Launch();
            }
        }
        else if (SealOfTheRighteousness.KnownSpell)
            if (!SealOfTheRighteousness.HaveBuff && SealOfTheRighteousness.IsSpellUsable)
            {
                {
                    SealOfTheRighteousness.Launch();
                }
            }
    }

    private void Blessing()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (BlessingOfMight.KnownSpell && !BlessingOfMight.HaveBuff && BlessingOfMight.IsSpellUsable)
        {
            BlessingOfMight.Launch();
        }
    }

    private void Heal()
    {
        if (DivineShield.KnownSpell && ObjectManager.Me.HealthPercent > 0 && ObjectManager.Me.HealthPercent <= 5 && !ObjectManager.Me.HaveBuff(25771) && DivineShield.IsSpellUsable)
        {
            DivineShield.Launch();
            return;
        }
        if (LayOnHands.KnownSpell && ObjectManager.Me.HealthPercent > 0 && ObjectManager.Me.HealthPercent <= 20 && !ObjectManager.Me.HaveBuff(25771) && LayOnHands.IsSpellUsable)
        {
            LayOnHands.Launch();
            return;
        }
        if (ObjectManager.Me.BarTwoPercentage < 10)
        {
            if (Arcane_Torrent.KnownSpell && Arcane_Torrent.IsSpellUsable)
                Arcane_Torrent.Launch();
            if (DivinePlea.KnownSpell && DivinePlea.IsSpellUsable)
            {
                DivinePlea.Launch();
                return;
            }
        }
        if (ObjectManager.Me.HealthPercent > 0 && ObjectManager.Me.HealthPercent < 50)
        {
            if (WorldOfGlory.KnownSpell && WorldOfGlory.IsSpellUsable)
                WorldOfGlory.Launch();
            if (DivineLight.KnownSpell && DivineLight.IsSpellUsable)
            {
                DivineLight.Launch();
                return;
            }
            if (FlashOfLight.KnownSpell && FlashOfLight.IsSpellUsable)
            {
                FlashOfLight.Launch();
                return;
            }
            if (HolyLight.KnownSpell && HolyLight.IsSpellUsable)
            {
                HolyLight.Launch();
                return;
            }
        }
        if (ObjectManager.Me.HealthPercent >= 0 && ObjectManager.Me.HealthPercent < 30)
        {
            if (WorldOfGlory.KnownSpell && WorldOfGlory.IsSpellUsable)
                WorldOfGlory.Launch();
            if (DivineProtection.KnownSpell && DivineProtection.IsSpellUsable)
                DivineProtection.Launch();
            if (FlashOfLight.KnownSpell && FlashOfLight.IsSpellUsable)
            {
                FlashOfLight.Launch();
                return;
            }
            if (HolyLight.KnownSpell && HolyLight.IsSpellUsable)
            {
                HolyLight.Launch();
                return;
            }
            if (DivineLight.KnownSpell && DivineLight.IsSpellUsable)
            {
                DivineLight.Launch();
                return;
            }
        }
    }
    private void DPS_Burst()
    {
        if (!Inquisition.HaveBuff && Inquisition.KnownSpell && Inquisition.IsSpellUsable && ObjectManager.Me.HolyPower >= 3)
        {
            Inquisition.Launch();
        }
        if (AvengingWrath.KnownSpell && AvengingWrath.IsSpellUsable)
        {
            AvengingWrath.Launch();
        }
        return;
    }
    private void DPS_Cycle()
    {
        /*if (HammerOfJustice.KnownSpell && HammerOfJustice.IsDistanceGood && HammerOfJustice.IsSpellUsable)
        {
           // TODO : If target can be stun, if not, it will be a pure loss of DPS.
            HammerOfJustice.Launch();
            return;
        }*/
        if (HolyShock.KnownSpell && HolyShock.IsDistanceGood && HolyShock.IsSpellUsable)
        {
            HolyShock.Launch();
            return;
        }
        if (Exorcism.KnownSpell && Exorcism.IsDistanceGood && Exorcism.IsSpellUsable)
        {
            Exorcism.Launch();
            return;
        }
    }
}

public class Paladin_Protection
{
    #region Professions & Racial
    private readonly Spell Arcane_Torrent = new Spell("Arcane Torrent");
    private readonly Spell Lifeblood = new Spell("Lifeblood");
    private readonly Spell Stoneform = new Spell("Stoneform");
    private readonly Spell Tailoring = new Spell("Tailoring");
    private readonly Spell Leatherworking = new Spell("Leatherworking");
    private readonly Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    private readonly Spell War_Stomp = new Spell("War Stomp");
    private readonly Spell Berserking = new Spell("Berserking");
    #endregion

    #region Paladin Seals & Buffs
    private readonly Spell SealOfTheRighteousness = new Spell("Seal of Righteousness");
    private readonly Spell SealOfTruth = new Spell("Seal of Truth");
    private readonly Spell SealOfInsight = new Spell("Seal of Insight");
    private readonly Spell BlessingOfMight = new Spell("Blessing of Might");
    private readonly Spell BlessingOfKings = new Spell("Blessing of Kings");
    #endregion

    #region Offensive Spell
    private readonly Spell HammerOfTheRighteous = new Spell("Hammer of the Righteous"); // 115798 = Weakened Blows
    private readonly Spell ShieldOfTheRighteous = new Spell("Shield of the Righteous");
    private readonly Spell AvengersShield = new Spell("Avenger's Shield");
    private readonly Spell CrusaderStrike = new Spell("Crusader Strike");
    private readonly Spell Consecration = new Spell("Consecration");
    private readonly Spell HolyWrath = new Spell("Holy Wrath");
    private readonly Spell Judgment = new Spell("Judgment");
    private readonly Spell HammerOfJustice = new Spell("Hammer of Justice");
    private readonly Spell HammerOfWrath = new Spell("Hammer of Wrath");
    #endregion

    #region Offensive Cooldown
	private readonly Spell HolyAvenger = new Spell("Holy Avenger");
    private readonly Spell AvengingWrath = new Spell("Avenging Wrath");
    #endregion

    #region Defensive Cooldown
    Timer OnCD = new Timer(0);
    private readonly Spell GuardianOfAncientKings = new Spell("Guardian of Ancient Kings");
    private readonly Spell HolyShield = new Spell("Holy Shield");
    private readonly Spell ArdentDefender = new Spell("Ardent Defender");
	private readonly Spell SacredShield = new Spell("Sacred Shield");
	private readonly Spell HandOfPurity = new Spell("Hand Of Purity");
    private readonly Spell DevotionAura = new Spell("Devotion Aura");
    private readonly Spell DivineProtection = new Spell("Divine Protection");
    private readonly Spell DivineShield = new Spell("Divine Shield");
    private readonly Spell HandOfProtection = new Spell("Hand Of Protection");
    #endregion
    
    #region Healing Spell
    private readonly Spell FlashOfLight = new Spell("Flash of Light");
    private readonly Spell LayOnHands = new Spell("Lay on Hands");
    private readonly Spell WorldOfGlory = new Spell("Word of Glory");
    #endregion

    public Paladin_Protection()
    {
        Main.range = 3.6f;

        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            try
            {
                if (!ObjectManager.Me.IsMounted)
                {
                    Patrolling();

                    if (Fight.InFight && ObjectManager.Me.Target > 0)
                    {
                        if (ObjectManager.Me.Target != lastTarget && Judgment.IsDistanceGood)
                        {
                            Pull();
                            lastTarget = ObjectManager.Me.Target;
                        }

                        Combat();
                    }
                }
            }
            catch
            {
            }
            Thread.Sleep(250);
        }
    }

    private void Pull()
    {
		DPS_Burst();
        if (Judgment.KnownSpell && Judgment.IsDistanceGood && Judgment.IsSpellUsable)
        {
            Judgment.Launch();
            Thread.Sleep(1000);
        }
        if (AvengersShield.KnownSpell && AvengersShield.IsDistanceGood && AvengersShield.IsSpellUsable)
        {
            AvengersShield.Launch();
        }
    }

    private void Combat()
    {
        if (OnCD.IsReady)
            Defense_Cycle();

        DPS_Cycle();

        Heal();

        DPS_Burst();
    }

    private void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            Seal();
            Blessing();
        }
    }

    private void Seal()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (SealOfTruth.KnownSpell)
        {
            if (!SealOfTruth.HaveBuff && SealOfTruth.IsSpellUsable)
            {
                SealOfTruth.Launch();
            }
        }
        else if (SealOfTheRighteousness.KnownSpell)
            if (!SealOfTheRighteousness.HaveBuff && SealOfTheRighteousness.IsSpellUsable)
            {
                {
                    SealOfTheRighteousness.Launch();
                }
            }
    }

    private void Blessing()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (BlessingOfMight.KnownSpell && !BlessingOfMight.HaveBuff && BlessingOfMight.IsSpellUsable)
        {
            BlessingOfMight.Launch();
        }
    }

    private void Heal()
    {
        if (DivineShield.KnownSpell && ObjectManager.Me.HealthPercent > 0 && ObjectManager.Me.HealthPercent <= 5 && DivineShield.IsSpellUsable && !ObjectManager.Me.HaveBuff(25771))
        {
            DivineShield.Launch();
            return;
        }
        if (LayOnHands.KnownSpell && ObjectManager.Me.HealthPercent > 0 && ObjectManager.Me.HealthPercent <= 20 && LayOnHands.IsSpellUsable && !ObjectManager.Me.HaveBuff(25771))
        {
            LayOnHands.Launch();
            return;
        }
        if (ObjectManager.Me.BarTwoPercentage < 10)
        {
            if (Arcane_Torrent.KnownSpell && Arcane_Torrent.IsSpellUsable)
                Arcane_Torrent.Launch();
				return;
        }
        if (ObjectManager.Me.HealthPercent > 0 && ObjectManager.Me.HealthPercent < 50)
        {
            if (WorldOfGlory.KnownSpell && WorldOfGlory.IsSpellUsable)
                WorldOfGlory.Launch();
            if (FlashOfLight.KnownSpell && FlashOfLight.IsSpellUsable)
            {
                FlashOfLight.Launch();
                return;
            }
        }
        if (ObjectManager.Me.HealthPercent >= 0 && ObjectManager.Me.HealthPercent < 30)
        {
            if (WorldOfGlory.KnownSpell && WorldOfGlory.IsSpellUsable)
                WorldOfGlory.Launch();
            if (DivineProtection.KnownSpell && DivineProtection.IsSpellUsable)
                DivineProtection.Launch();
            if (FlashOfLight.KnownSpell && FlashOfLight.IsSpellUsable)
            {
                FlashOfLight.Launch();
                return;
            }
        }
    }
    private void DPS_Burst()
    {
		if(HolyAvenger.KnownSpell && HolyAvenger.IsSpellUsable)
		{
			HolyAvenger.Launch();
			if (AvengingWrath.KnownSpell && AvengingWrath.IsSpellUsable)
			{
				AvengingWrath.Launch();
				return;
			}
		}
        else if (AvengingWrath.KnownSpell && AvengingWrath.IsSpellUsable)
        {
            AvengingWrath.Launch();
            return;
        }
    }
    private void Defense_Cycle()
    {
		if (SacredShield.KnownSpell && SacredShield.IsSpellUsable && !SacredShield.HaveBuff)
		{
			SacredShield.Launch();
			OnCD = new Timer(0);
		}
        else if (HolyShield.KnownSpell && HolyShield.IsSpellUsable)
        {
            HolyShield.Launch();
            OnCD = new Timer(1000 * 10);
            return;
        }
		else if (HandOfPurity.KnownSpell && HandOfPurity.IsSpellUsable && !HandOfPurity.HaveBuff)
		{
			HandOfPurity.Launch();
			OnCD = new Timer(1000 * 6);
		}
        else if (HammerOfJustice.KnownSpell && HammerOfJustice.IsSpellUsable)
        {
            HammerOfJustice.Launch();
            OnCD = new Timer(1000 * 6);
            return;
        }
        else if (DivineShield.KnownSpell && DivineShield.IsSpellUsable && !ObjectManager.Me.HaveBuff(25771))
        {
            DivineShield.Launch();
            OnCD = new Timer(1000 * 8);
            return;
        }
        else if (DivineProtection.KnownSpell && DivineProtection.IsSpellUsable)
        {
            DivineProtection.Launch();
            OnCD = new Timer(1000 * 10);
            return;
        }
        else if (DevotionAura.KnownSpell && DevotionAura.IsSpellUsable)
        {
            DevotionAura.Launch();
            OnCD = new Timer(1000 * 6);
            return;
        }
        else if (GuardianOfAncientKings.KnownSpell && GuardianOfAncientKings.IsSpellUsable)
        {
            GuardianOfAncientKings.Launch();
            OnCD = new Timer(1000 * 12);
            return;
        }
        else if (ArdentDefender.KnownSpell && ArdentDefender.IsSpellUsable)
        {
            ArdentDefender.Launch();
            OnCD = new Timer(1000 * 10);
            return;
        }
        else if (LayOnHands.KnownSpell && LayOnHands.IsSpellUsable && !ObjectManager.Me.HaveBuff(25771))
        {
            LayOnHands.Launch();
            OnCD = new Timer(1000 * 5);
            return;
        }
        else if (WorldOfGlory.KnownSpell && WorldOfGlory.IsSpellUsable)
        {
            WorldOfGlory.Launch();
            OnCD = new Timer(1000 * 5);
            return;
        }
        else if (HandOfProtection.KnownSpell && HandOfProtection.IsSpellUsable && !ObjectManager.Me.HaveBuff(25771))
        {
            HandOfProtection.Launch();
            OnCD = new Timer(1000 * 8);
            return;
        }
    }
    private void DPS_Cycle()
    {
        if (ShieldOfTheRighteous.KnownSpell && ShieldOfTheRighteous.IsSpellUsable && ShieldOfTheRighteous.IsDistanceGood && (ObjectManager.Me.HaveBuff(90174) || ObjectManager.Me.HolyPower >= 3))
        {
            ShieldOfTheRighteous.Launch();
            return;
        }
        if ((ObjectManager.GetNumberAttackPlayer() >= 3  || !ObjectManager.Target.HaveBuff(115798)) && !ObjectManager.Me.HaveBuff(90174) && ObjectManager.Me.HolyPower < 3)
        {
            if (HammerOfTheRighteous.KnownSpell && HammerOfTheRighteous.IsDistanceGood && HammerOfTheRighteous.IsSpellUsable)
            {
                HammerOfTheRighteous.Launch();
                return;
            }
        }
        else
        {
            if (CrusaderStrike.KnownSpell && CrusaderStrike.IsDistanceGood && CrusaderStrike.IsSpellUsable && !ObjectManager.Me.HaveBuff(90174) && ObjectManager.Me.HolyPower < 3)
            {
                CrusaderStrike.Launch();
                return;
            }
        }
        if (AvengersShield.KnownSpell && AvengersShield.IsDistanceGood && AvengersShield.IsSpellUsable && !ObjectManager.Me.HaveBuff(90174) && ObjectManager.Me.HolyPower < 3)
        {
            AvengersShield.Launch();
            return;
        }
        if (HammerOfWrath.KnownSpell && HammerOfWrath.IsDistanceGood && HammerOfWrath.IsSpellUsable && !ObjectManager.Me.HaveBuff(90174) && ObjectManager.Me.HolyPower < 3)
        {
            HammerOfWrath.Launch();
            return;
        }
        if (Judgment.KnownSpell && Judgment.IsDistanceGood && Judgment.IsSpellUsable && !ObjectManager.Me.HaveBuff(90174) && ObjectManager.Me.HolyPower < 3)
        {
            Judgment.Launch();
            return;
        }
        if (Consecration.KnownSpell && Consecration.IsSpellUsable && !ObjectManager.Me.HaveBuff(90174) && ObjectManager.Me.HolyPower < 3)
        {
            // Consecration.Launch(); // If the glyph is activated, it's a huge loss of time to have this .Launch() here "2-3 sec" without doing anything.
			SpellManager.CastSpellByIDAndPosition(26573, ObjectManager.Target.Position);
            return;
        }
        if (HolyWrath.KnownSpell && HolyWrath.IsSpellUsable && !ObjectManager.Me.HaveBuff(90174) && ObjectManager.Me.HolyPower < 3 && !Judgment.IsSpellUsable && !CrusaderStrike.IsSpellUsable && !Consecration.IsSpellUsable)
        {
            HolyWrath.Launch();
            return;
        }
    }
}

public class Paladin_Retribution
{
    #region Professions & Racial
    private readonly Spell Arcane_Torrent = new Spell("Arcane Torrent");
    private readonly Spell Lifeblood = new Spell("Lifeblood");
    private readonly Spell Stoneform = new Spell("Stoneform");
    private readonly Spell Tailoring = new Spell("Tailoring");
    private readonly Spell Leatherworking = new Spell("Leatherworking");
    private readonly Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    private readonly Spell War_Stomp = new Spell("War Stomp");
    private readonly Spell Berserking = new Spell("Berserking");
    #endregion

    #region Paladin Seals & Buffs
    private readonly Spell SealOfTheRighteousness = new Spell("Seal of Righteousness");
    private readonly Spell SealOfTruth = new Spell("Seal of Truth");
    private readonly Spell SealOfJustice = new Spell("Seal of Justice");
    private readonly Spell BlessingOfMight = new Spell("Blessing of Might");
    private readonly Spell BlessingOfKings = new Spell("Blessing of Kings");
    #endregion

    #region Offensive Spell
    private readonly Spell TemplarsVerdict = new Spell("Templar's Verdict");
	private readonly Spell DivineStorm = new Spell("Divine Storm");
	private readonly Spell Exorcism = new Spell("Exorcism");
    private readonly Spell HammerOfWrath = new Spell("Hammer of Wrath");
    private readonly Spell CrusaderStrike = new Spell("Crusader Strike");
	private readonly Spell HammerOfTheRighteous = new Spell("Hammer of the Righteous");
	private readonly Spell Judgment = new Spell("Judgment");	
	private readonly Spell HammerOfJustice = new Spell("Hammer of Justice");
    
    #endregion

    #region Offensive Cooldown
    private readonly Spell Inquisition = new Spell("Inquisition");
    private Timer InquisitionToUseInPriotiy = new Timer(0);
    private readonly Spell GuardianOfAncientKings = new Spell("Guardian of Ancient Kings");
	private Timer BurstTime = new Timer(0);
    private readonly Spell HolyAvenger = new Spell("Holy Avenger"); 
    private readonly Spell AvengingWrath = new Spell("Avenging Wrath");
    #endregion

    #region Defensive Cooldown
    private readonly Spell DivineProtection = new Spell("Divine Protection");
    private readonly Spell DevotionAura = new Spell("Devotion Aura");
	private readonly Spell SacredShield = new Spell("Sacred Shield");
    private readonly Spell DivineShield = new Spell("Divine Shield");
    private readonly Spell HandOfProtection = new Spell("Hand Of Protection");
    #endregion
        
    #region Healing Spell
    private readonly Spell FlashOfLight = new Spell("Flash of Light");
    private readonly Spell LayOnHands = new Spell("Lay on Hands");
    private readonly Spell WorldOfGlory = new Spell("Word of Glory");
    #endregion

    public Paladin_Retribution()
    {
        Main.range = 3.6f;

        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            try
            {
                if (!ObjectManager.Me.IsMounted)
                {
                    Patrolling();

                    if (Fight.InFight && ObjectManager.Me.Target > 0)
                    {
                        if (ObjectManager.Me.Target != lastTarget && Judgment.IsDistanceGood)
                        {
                            Pull();
                            lastTarget = ObjectManager.Me.Target;
                        }

                        Combat();
                    }
                }
            }
            catch
            {
            }
            Thread.Sleep(250);
        }
    }

    private void Pull()
    {
        if (Judgment.KnownSpell && Judgment.IsDistanceGood && Judgment.IsSpellUsable)
        {
            Judgment.Launch();
        }
    }

    private void Combat()
    {
        DPS_Cycle();

        Heal();

        DPS_Burst();
    }

    private void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            Seal();
            Blessing();
        }
    }

    private void Seal()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (SealOfTruth.KnownSpell)
        {
            if (!SealOfTruth.HaveBuff && SealOfTruth.IsSpellUsable)
                SealOfTruth.Launch();
        }
        else if (SealOfTheRighteousness.KnownSpell)
        {
			if (!SealOfTheRighteousness.HaveBuff && SealOfTheRighteousness.IsSpellUsable)
				SealOfTheRighteousness.Launch();
        }
    }

    private void Blessing()
    {
        if (ObjectManager.Me.IsMounted)
            return;
		else if (BlessingOfMight.KnownSpell && !BlessingOfMight.HaveBuff && BlessingOfMight.IsSpellUsable)
            BlessingOfMight.Launch();
    }

    private void Heal()
    {
        if (DivineShield.KnownSpell && ObjectManager.Me.HealthPercent > 0 && ObjectManager.Me.HealthPercent <= 5 && !ObjectManager.Me.HaveBuff(25771) && DivineShield.IsSpellUsable)
        {
            DivineShield.Launch();
            return;
        }
        if (LayOnHands.KnownSpell && ObjectManager.Me.HealthPercent > 0 && ObjectManager.Me.HealthPercent <= 20 && !ObjectManager.Me.HaveBuff(25771) && LayOnHands.IsSpellUsable)
        {
            LayOnHands.Launch();
            return;
        }
        if (ObjectManager.Me.BarTwoPercentage < 10)
        {
            if (Arcane_Torrent.KnownSpell && Arcane_Torrent.IsSpellUsable)
            {
				Arcane_Torrent.Launch();
				return;
			}
        }
        if (ObjectManager.Me.HealthPercent > 0 && ObjectManager.Me.HealthPercent < 50)
        {
            if (WorldOfGlory.KnownSpell && WorldOfGlory.IsSpellUsable)
                WorldOfGlory.Launch();
            if (FlashOfLight.KnownSpell && FlashOfLight.IsSpellUsable)
            {
                FlashOfLight.Launch();
                return;
            }
        }
        if (ObjectManager.Me.HealthPercent >= 0 && ObjectManager.Me.HealthPercent < 30)
        {
            if (WorldOfGlory.KnownSpell && WorldOfGlory.IsSpellUsable)
                WorldOfGlory.Launch();
            if (DivineProtection.KnownSpell && DivineProtection.IsSpellUsable)
                DivineProtection.Launch();
            if (FlashOfLight.KnownSpell && FlashOfLight.IsSpellUsable)
            {
                FlashOfLight.Launch();
                return;
            }
        }
    }
    private void DPS_Burst()
    {
        if (GuardianOfAncientKings.HaveBuff || !GuardianOfAncientKings.IsSpellUsable)
        {
            if (BurstTime.IsReady && HolyAvenger.KnownSpell && HolyAvenger.IsSpellUsable)
            {
                HolyAvenger.Launch();
                if ((!Inquisition.HaveBuff || InquisitionToUseInPriotiy.IsReady) && Inquisition.KnownSpell && Inquisition.IsSpellUsable)
                {
                    Inquisition.Launch();
                    InquisitionToUseInPriotiy = new Timer(1000 * (10 * 3 - 6));
                }
				if (AvengingWrath.KnownSpell && AvengingWrath.IsSpellUsable)
                AvengingWrath.Launch();
                return;
            }
			else if (!HolyAvenger.KnownSpell && AvengingWrath.KnownSpell && AvengingWrath.IsSpellUsable)
            {
                if ((!Inquisition.HaveBuff || InquisitionToUseInPriotiy.IsReady) && Inquisition.KnownSpell && Inquisition.IsSpellUsable)
                {
                    Inquisition.Launch();
                    InquisitionToUseInPriotiy = new Timer(1000 * (10 * 3 - 6));
                }
                AvengingWrath.Launch();
                return;
            }
        }
        else
            if (GuardianOfAncientKings.KnownSpell && GuardianOfAncientKings.IsSpellUsable && HolyAvenger.IsSpellUsable)
            {
                GuardianOfAncientKings.Launch();
                BurstTime = new Timer(1000 * 6);
                return;
            }
    }
    private void DPS_Cycle()
    {
        /*if (HammerOfJustice.KnownSpell && HammerOfJustice.IsDistanceGood && HammerOfJustice.IsSpellUsable)
        {
           // TODO : If target can be stun, if not, it will be a pure loss of DPS.
            HammerOfJustice.Launch();
            return;
        }*/
        if (Inquisition.KnownSpell && (!Inquisition.HaveBuff || InquisitionToUseInPriotiy.IsReady) && Inquisition.IsSpellUsable && (ObjectManager.Me.HaveBuff(90174) || ObjectManager.Me.HolyPower >= 3))
        {
            if (HolyAvenger.IsSpellUsable && (GuardianOfAncientKings.HaveBuff || !GuardianOfAncientKings.IsSpellUsable))
            {
                DPS_Burst();
                return;
            }
            else
                Inquisition.Launch();
            InquisitionToUseInPriotiy = new Timer(1000 * (10 * 3 - 6));
            return;
        }
        if (ObjectManager.GetNumberAttackPlayer() <= 2 && TemplarsVerdict.KnownSpell && Inquisition.HaveBuff && TemplarsVerdict.IsSpellUsable && TemplarsVerdict.IsDistanceGood && (ObjectManager.Me.HaveBuff(90174) || ObjectManager.Me.HolyPower >= 3))
        {
            TemplarsVerdict.Launch();
            return;
        }
		else if (ObjectManager.GetNumberAttackPlayer() >= 3 && DivineStorm.KnownSpell && Inquisition.HaveBuff && DivineStorm.IsSpellUsable && DivineStorm.IsDistanceGood && (ObjectManager.Me.HaveBuff(90174) || ObjectManager.Me.HolyPower >= 3))
        {
			DivineStorm.Launch();
			return;
        }
		if ((ObjectManager.GetNumberAttackPlayer() <= 2 || ObjectManager.Target.HaveBuff(115798)) && CrusaderStrike.KnownSpell && CrusaderStrike.IsDistanceGood && CrusaderStrike.IsSpellUsable)
		{
			CrusaderStrike.Launch();
			return;
		}
		else if ((ObjectManager.GetNumberAttackPlayer() >= 3 || !ObjectManager.Target.HaveBuff(115798)) && HammerOfTheRighteous.KnownSpell && HammerOfTheRighteous.IsDistanceGood && HammerOfTheRighteous.IsSpellUsable && !ObjectManager.Me.HaveBuff(90174))
		{
			HammerOfTheRighteous.Launch();
			return;
		}
        if (Exorcism.KnownSpell && Exorcism.IsDistanceGood && Exorcism.IsSpellUsable && Inquisition.HaveBuff)
        {
            Exorcism.Launch();
            return;
        }
        if (HammerOfWrath.KnownSpell && HammerOfWrath.IsDistanceGood && HammerOfWrath.IsSpellUsable && Inquisition.HaveBuff)
        {
            HammerOfWrath.Launch();
            return;
        }
        if (Judgment.KnownSpell && Judgment.IsDistanceGood && Judgment.IsSpellUsable)
        {
            Judgment.Launch();
            return;
        }
    }
}
#endregion

#region Shaman
public class Ele
{
    #region InitializeSpell

    // ELE ONLY
    Spell Thunderstorm = new Spell("Thunderstorm");
    Spell Lava_Burst = new Spell("Lava Burst");
    Spell Elemental_Mastery = new Spell("Elemental Mastery");
    Spell Flametongue_Weapon = new Spell("Flametongue Weapon");

    // SKILL
    Spell Lightning_Bolt = new Spell("Lightning Bolt");
    Spell Chain_Lightning = new Spell("Chain Lightning");
    Spell Flame_Shock = new Spell("Flame Shock");
    Spell Earth_Shock = new Spell("Earth Shock");
    Spell Lightning_Shield = new Spell("Lightning Shield");
    Spell Water_Shield = new Spell("Water Shield");
    Spell Unleash_Elements = new Spell("Unleash Elements");

    // TIMER
    Timer look = new Timer(0);
    Timer fighttimer = new Timer(0);
    Timer weaponbuff = new Timer(30000);

    // BUFF & HELPING
    Spell Stoneclaw_Totem = new Spell("Stoneclaw Totem");
    Spell Call_of_the_Elements = new Spell("Call of the Elements");
    Spell Bloodlust = new Spell("Bloodlust");
    Spell Heroism = new Spell("Heroism");
    Spell Wind_Shear = new Spell("Wind Shear");
    Spell Healing_Wave = new Spell("Healing Wave");
    Spell Healing_Surge = new Spell("Healing Surge");
    Spell Earth_Elemental_Totem = new Spell("Earth Elemental Totem");

    // profession & racials
    Spell Arcane_Torrent = new Spell("Arcane Torrent");
    Spell Lifeblood = new Spell("Lifeblood");
    Spell Stoneform = new Spell("Stoneform");
    Spell Tailoring = new Spell("Tailoring");
    Spell Leatherworking = new Spell("Leatherworking");
    Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    Spell War_Stomp = new Spell("War Stomp");
    Spell Berserking = new Spell("Berserking");

    #endregion InitializeSpell

    public Ele()
    {
        Main.range = 30.0f;
        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            if (!ObjectManager.Me.IsMounted)
            {
                buffoutfight();

                if (ItemsManager.GetItemCountByIdLUA(67495) > 0) Logging.WriteFight("/use item:67495");

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
        Lua.RunMacroText("/petattack");
        fighttimer = new Timer(60000);
    }

    public void buffoutfight()
    {

        selfheal();

        if (Fight.InFight || ObjectManager.Me.IsDeadMe) return;

        if (!ObjectManager.Me.HaveBuff(79640) &&
            ItemsManager.GetItemCountByIdLUA(58149) == 1)
        {
            Logging.WriteFight("Use Alchi Flask");
            Lua.RunMacroText("/use item:58149");
        }

        if (!Call_of_the_Elements.KnownSpell && !ObjectManager.Me.HaveBuff(77747))
        {
            SpellManager.CastSpellByIdLUA(8227);
            // Flametongue_Totem.Launch();
        }

    }

    public void fight()
    {
        selfheal();
        buffinfight();
        if (ObjectManager.GetNumberAttackPlayer() > 1) fighttimer = new Timer(60000);

        if (!ObjectManager.Target.IsTargetingMe && Lightning_Bolt.IsDistanceGood)
        {
            SpellManager.CastSpellByIdLUA(403);
            // Lightning_Bolt.Launch();
            return;
        }

        if (Thunderstorm.KnownSpell && Thunderstorm.IsSpellUsable && ObjectManager.Me.BarTwoPercentage < 80 &&
            ObjectManager.Target.GetDistance < 10)
        {
            SpellManager.CastSpellByIdLUA(51490);
            // Thunderstorm.Launch();
        }

        if (!Flame_Shock.TargetHaveBuff && Flame_Shock.IsSpellUsable && Flame_Shock.IsDistanceGood)
        {
            SpellManager.CastSpellByIdLUA(8050);
            Thread.Sleep(50);
            SpellManager.CastSpellByIdLUA(51505);
            // Flame_Shock.Launch();
        }

        if (Lava_Burst.KnownSpell && Lava_Burst.IsSpellUsable && Lava_Burst.IsDistanceGood && Flame_Shock.TargetHaveBuff)
        {
            SpellManager.CastSpellByIdLUA(51505);
            // Lava_Burst.Launch();
            return;
        }

        if (Chain_Lightning.KnownSpell && Chain_Lightning.IsSpellUsable && Chain_Lightning.IsDistanceGood &&
            ObjectManager.GetNumberAttackPlayer() > 1)
        {
            SpellManager.CastSpellByIdLUA(421);
            // Chain_Lightning.Launch();
        }

        if (ObjectManager.Target.IsCast && Wind_Shear.KnownSpell && Wind_Shear.IsSpellUsable && Wind_Shear.IsDistanceGood)
        {
            SpellManager.CastSpellByIdLUA(57994);
            // Wind_Shear.Launch();
        }

        if (Earth_Shock.IsSpellUsable && Earth_Shock.KnownSpell && Earth_Shock.IsDistanceGood)
        {
            if (!Flame_Shock.TargetHaveBuff && Flame_Shock.KnownSpell) return;
            SpellManager.CastSpellByIdLUA(8042);
            // Earth_Shock.Launch();
        }

        if (Berserking.KnownSpell && Berserking.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(1454);
            // Berserking.Launch();
        }

        if (Unleash_Elements.KnownSpell && Unleash_Elements.IsSpellUsable && Unleash_Elements.IsDistanceGood)
        {
            SpellManager.CastSpellByIdLUA(73680);
            //Unleash_Elements.Launch();
        }

        if (Lightning_Bolt.IsSpellUsable && Lightning_Bolt.IsDistanceGood && !Lava_Burst.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(403);
            // Lightning_Bolt.Launch();
        }

    }

    private void buffinfight()
    {

        if (Flametongue_Weapon.KnownSpell && Flametongue_Weapon.IsSpellUsable && weaponbuff.IsReady)
        {
            Flametongue_Weapon.Launch();
            weaponbuff = new Timer(1800000);

        }

        if (Call_of_the_Elements.KnownSpell && Call_of_the_Elements.IsSpellUsable && !ObjectManager.Me.HaveBuff(77747))
        {
            SpellManager.CastSpellByIdLUA(66842);
            // Call_of_the_Elements.Launch();
        }

        if (ObjectManager.GetNumberAttackPlayer() > 3 || hardmob())
        {
            if (Elemental_Mastery.KnownSpell && Elemental_Mastery.IsSpellUsable)
            {
                SpellManager.CastSpellByIdLUA(16166);
                // Elemental_Mastery.Launch();
            }

            if (Heroism.KnownSpell && Heroism.IsSpellUsable)
            {
                SpellManager.CastSpellByIdLUA(32182);
                // Heroism.Launch();
            }

            if (Bloodlust.KnownSpell && Bloodlust.IsSpellUsable)
            {
                SpellManager.CastSpellByIdLUA(2825);
                // Bloodlust.Launch();
            }

            if (Earth_Elemental_Totem.KnownSpell && Earth_Elemental_Totem.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() > 4)
            {
                SpellManager.CastSpellByIdLUA(2062);
                // Earth_Elemental_Totem.Launch();
            }
        }

        if (!Lightning_Shield.HaveBuff && Lightning_Shield.KnownSpell && Lightning_Shield.IsSpellUsable && Water_Shield.IsSpellUsable && ObjectManager.Me.BarTwoPercentage > 50)
        {
            SpellManager.CastSpellByIdLUA(324);
            // Lightning_Shield.Launch();
        }

        if (!Water_Shield.HaveBuff && Water_Shield.KnownSpell && Water_Shield.IsSpellUsable && ObjectManager.Me.BarTwoPercentage < 20)
        {
            SpellManager.CastSpellByIdLUA(52127);
            // Water_Shield.Launch();
        }

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

        if (Arcane_Torrent.KnownSpell && Arcane_Torrent.IsSpellUsable &&
            ObjectManager.Target.IsCast && ObjectManager.Target.GetDistance < 8)
        {
            Arcane_Torrent.Launch();
        }

        if (Stoneclaw_Totem.KnownSpell && Stoneclaw_Totem.IsSpellUsable && ObjectManager.Me.HealthPercent < 50 &&
            ObjectManager.GetNumberAttackPlayer() > 1)
        {
            SpellManager.CastSpellByIdLUA(5730);
            // Stoneclaw_Totem.Launch();
        }

        if (Healing_Surge.KnownSpell && Healing_Surge.IsSpellUsable && ObjectManager.Me.HealthPercent < 75)
        {
            SpellManager.CastSpellByIdLUA(8004);
            // Healing_Surge.Launch();
            return;
        }

        if (!Healing_Surge.KnownSpell && Healing_Wave.KnownSpell && Healing_Wave.IsSpellUsable && ObjectManager.Me.HealthPercent < 75)
        {
            SpellManager.CastSpellByIdLUA(331);
            // Healing_Wave.Launch();
        }

    }

    public bool hardmob()
    {
        if (((ObjectManager.Target.MaxHealth * 100) / ObjectManager.Me.MaxHealth) > 110)
        {
            return true;
        }
        return false;
    }
}
#endregion

#region Priest
public class Shadow
{
    #region InitializeSpell

    // Shadow Only
    Spell Mind_Flay = new Spell("Mind Flay");
    Spell Shadowform = new Spell("Shadowform");
    Spell Devouring_Plague = new Spell("Devouring Plague");
    Spell Vampiric_Touch = new Spell("Vampiric Touch");
    Spell Vampiric_Embrace = new Spell("Vampiric Embrace");
    Spell Silence = new Spell("Silence");
    Spell Dispersion = new Spell("Dispersion");

    // HEAL
    Spell Renew = new Spell("Renew");
    Spell Flash_Heal = new Spell("Flash Heal");
    Spell Greater_Heal = new Spell("Greater Heal");
    Spell Inner_Focus = new Spell("Inner Focus");
    Spell Power_Word_Shield = new Spell("Power Word: Shield");
    Spell Prayer_of_Mending = new Spell("Prayer of Mending");

    // DPS
    Spell Mind_Blast = new Spell("Mind Blast");
    Spell Shadow_Word_Pain = new Spell("Shadow Word: Pain");
    Spell Shadow_Word_Death = new Spell("Shadow Word: Death");
    Spell Mind_Spike = new Spell("Mind Spike");
    Spell Smite = new Spell("Smite");

    // BUFF & HELPING
    Spell Inner_Fire = new Spell("Inner Fire");
    Spell Psychic_Scream = new Spell("Psychic Scream");
    Spell Psychic_Horror = new Spell("Psychic Horror");
    Spell Shadowfiend = new Spell("Shadowfiend");
    Spell Fade = new Spell("Fade");
    Spell Power_Word_Fortitude = new Spell("Power Word: Fortitude");

    // TIMER
    Timer look = new Timer(0);
    Timer fighttimer = new Timer(0);
    Timer shadowfiendrota = new Timer(0);
    Timer renewchill = new Timer(0);
    Timer vtouchchill = new Timer(0);
    Timer painchill = new Timer(0);
    Timer plaguechill = new Timer(0);

    // Profession & Racials
    Spell Arcane_Torrent = new Spell("Arcane Torrent");
    Spell Lifeblood = new Spell("Lifeblood");
    Spell Stoneform = new Spell("Stoneform");
    Spell Tailoring = new Spell("Tailoring");
    Spell Leatherworking = new Spell("Leatherworking");
    Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    Spell War_Stomp = new Spell("War Stomp");
    Spell Berserking = new Spell("Berserking");

    #endregion InitializeSpell

    public Shadow()
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

                    if (fighttimer.IsReady && ObjectManager.Target.HealthPercent > 90 && ObjectManager.Me.Target > 0 && ObjectManager.GetNumberAttackPlayer() < 2)
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

        if (Power_Word_Fortitude.KnownSpell && Power_Word_Fortitude.IsSpellUsable &&
            !Power_Word_Fortitude.HaveBuff)
        {
            Power_Word_Fortitude.Launch();
        }

        if (Inner_Fire.KnownSpell && Inner_Fire.IsSpellUsable && !Inner_Fire.HaveBuff)
        {
            Inner_Fire.Launch();
        }

        if (Vampiric_Embrace.KnownSpell && Vampiric_Embrace.IsSpellUsable && !Vampiric_Embrace.HaveBuff)
        {
            Vampiric_Embrace.Launch();
        }

        if (!Shadowform.HaveBuff && Shadowform.KnownSpell && Shadowform.IsSpellUsable)
        {
            Shadowform.Launch();
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

        if (Mind_Spike.KnownSpell && Mind_Spike.IsSpellUsable && Mind_Spike.IsDistanceGood && (Vampiric_Touch.TargetHaveBuff || Shadow_Word_Pain.TargetHaveBuff) &&
            ObjectManager.Target.HealthPercent < 30)
        {
            SpellManager.CastSpellByIdLUA(73510);
            // Mind_Spike.Launch();
        }

        if (Shadow_Word_Death.IsSpellUsable && Shadow_Word_Death.IsDistanceGood && !Mind_Blast.IsSpellUsable && ObjectManager.Target.HealthPercent < 26)
        {
            SpellManager.CastSpellByIdLUA(32379);
            // Shadow_Word_Death.Launch();
        }

        if (Vampiric_Touch.KnownSpell && Vampiric_Touch.IsSpellUsable && Vampiric_Touch.IsDistanceGood && !Vampiric_Touch.TargetHaveBuff && ObjectManager.Target.HealthPercent > 30 && vtouchchill.IsReady)
        {
            vtouchchill = new Timer(2500);
            SpellManager.CastSpellByIdLUA(34914);
            // Vampiric_Touch.Launch();
        }

        if (Shadow_Word_Pain.KnownSpell && Shadow_Word_Pain.IsSpellUsable && Shadow_Word_Pain.IsDistanceGood && !Shadow_Word_Pain.TargetHaveBuff && ObjectManager.Target.HealthPercent > 30 && painchill.IsReady)
        {
            painchill = new Timer(2500);
            SpellManager.CastSpellByIdLUA(589);
            // Shadow_Word_Pain.Launch();
        }

        if (Mind_Blast.KnownSpell && Mind_Blast.IsSpellUsable && Mind_Blast.IsDistanceGood)
        {
            SpellManager.CastSpellByIdLUA(8092);
            // Mind_Blast.Launch();
        }

        if (Smite.KnownSpell && Smite.IsSpellUsable && Smite.IsDistanceGood && !Mind_Blast.KnownSpell)
        {
            SpellManager.CastSpellByIdLUA(585);
            // Smite.Launch();
        }

        if (Mind_Flay.IsSpellUsable && Mind_Flay.IsDistanceGood && !Mind_Blast.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(15407);
            // Mind_Flay.Launch();
            Thread.Sleep(200);
            while (ObjectManager.Me.IsCast)
            {
                Fight.StopFight();
                MovementManager.StopMove();
                Thread.Sleep(200);
            }
        }

        if (Devouring_Plague.KnownSpell && Devouring_Plague.IsSpellUsable && Devouring_Plague.IsDistanceGood && !Devouring_Plague.TargetHaveBuff &&
            ObjectManager.Target.HealthPercent > 30 && hardmob() && plaguechill.IsReady)
        {
            plaguechill = new Timer(2500);
            SpellManager.CastSpellByIdLUA(2944);
            // Devouring_Plague.Launch();
        }

        if (Mind_Blast.KnownSpell && Mind_Blast.IsSpellUsable && Mind_Blast.IsDistanceGood)
        {
            SpellManager.CastSpellByIdLUA(8092);
            // Mind_Blast.Launch();
        }

        if (Smite.KnownSpell && Smite.IsSpellUsable && Smite.IsDistanceGood && !Mind_Blast.KnownSpell)
        {
            SpellManager.CastSpellByIdLUA(585);
            // Smite.Launch();
        }

        if (Mind_Flay.IsSpellUsable && Mind_Flay.IsDistanceGood && !Mind_Blast.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(15407);
            // Mind_Flay.Launch();
            Thread.Sleep(200);
            while (ObjectManager.Me.IsCast)
            {
                Fight.StopFight();
                MovementManager.StopMove();
                Thread.Sleep(200);
            }
        }

        if (Silence.KnownSpell && Silence.IsSpellUsable && Silence.IsDistanceGood &&
            ObjectManager.Target.IsCast && ObjectManager.Target.HealthPercent > 30)
        {
            SpellManager.CastSpellByIdLUA(15487);
            // Silence.Launch();
        }

        if (Arcane_Torrent.KnownSpell && Arcane_Torrent.IsSpellUsable &&
            ObjectManager.Target.IsCast && ObjectManager.Target.GetDistance < 8)
        {
            Arcane_Torrent.Launch();
        }

        if (Psychic_Horror.KnownSpell && Psychic_Horror.IsSpellUsable && Psychic_Horror.IsDistanceGood &&
            ObjectManager.Target.HealthPercent > 30 && ObjectManager.Me.HealthPercent < 80)
        {
            SpellManager.CastSpellByIdLUA(64044);
            // Psychic_Horror.Launch();
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

        if ((ObjectManager.GetNumberAttackPlayer() > 1 || hardmob()) &&
            ObjectManager.Me.HealthPercent < 65 &&
            ObjectManager.Target.GetDistance < 5 &&
            War_Stomp.KnownSpell && War_Stomp.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(20549);
            // War_Stomp.Launch();
        }

        if (!Shadowform.HaveBuff && Shadowform.KnownSpell && Shadowform.IsSpellUsable && ObjectManager.Me.BarTwoPercentage > 20)
        {
            Shadowform.Launch();
        }

        if (Power_Word_Shield.KnownSpell && Power_Word_Shield.IsSpellUsable && !Power_Word_Shield.HaveBuff &&
            (ObjectManager.Me.HealthPercent < 80 || hardmob() || ObjectManager.GetNumberAttackPlayer() > 1))
        {
            SpellManager.CastSpellByIdLUA(17);
            // Power_Word_Shield.Launch();
        }

        if (Dispersion.KnownSpell && Dispersion.IsSpellUsable && ObjectManager.Me.BarTwoPercentage < 50 &&
            !hardmob() && ObjectManager.GetNumberAttackPlayer() < 2)
        {
            SpellManager.CastSpellByIdLUA(47585);
            // Dispersion.Launch();
        }

        if (Shadowfiend.KnownSpell && Shadowfiend.IsSpellUsable && !Dispersion.IsSpellUsable && ObjectManager.Me.BarTwoPercentage < 50 && ObjectManager.Target.HealthPercent > 60)
        {
            shadowfiendrota = new Timer(2200);
            while (!shadowfiendrota.IsReady)
            {
                Shadowfiend.Launch();
                Thread.Sleep(1000);
                if (Fade.KnownSpell && Fade.IsSpellUsable)
                {
                    Fade.Launch();
                }
            }
        }

    }

    private void selfheal()
    {

        if (Shadowfiend.KnownSpell && Shadowfiend.IsSpellUsable && ObjectManager.Me.HealthPercent < 25 && ObjectManager.Target.HealthPercent > 50 &&
            (hardmob() || ObjectManager.GetNumberAttackPlayer() > 1))
        {
            shadowfiendrota = new Timer(3000);
            while (!shadowfiendrota.IsReady)
            {
                Shadowfiend.Launch();
                Thread.Sleep(1000);
                if (Prayer_of_Mending.KnownSpell && Prayer_of_Mending.IsSpellUsable)
                {
                    Lua.RunMacroText("/cast [@player] Prayer of Mending");
                    Lua.RunMacroText("/cast [@player] Gebet der Besserung");
                    Lua.RunMacroText("/cast [@player] Prière de guérison");
                    Lua.RunMacroText("/cast [@player] Молитва восстановления");
                    Lua.RunMacroText("/cast [@player] Prece da Recomposição");
                    Lua.RunMacroText("/cast [@player] Rezo de alivio");
                    // Prayer_of_Mending.Launch();
                }
                if (Fade.KnownSpell && Fade.IsSpellUsable)
                {
                    Fade.Launch();
                }
            }
        }

        if (Dispersion.KnownSpell && Dispersion.IsSpellUsable && ObjectManager.Me.HealthPercent > 0 &&
            ObjectManager.Me.HealthPercent < 35 && ObjectManager.GetNumberAttackPlayer() > 1)
        {
            SpellManager.CastSpellByIdLUA(47585);
            // Dispersion.Launch();
        }

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

        if (Renew.KnownSpell && Renew.IsSpellUsable && !Renew.HaveBuff && renewchill.IsReady &&
            ObjectManager.Me.HealthPercent < 45 && ObjectManager.Me.BarTwoPercentage < 20)
        {
            renewchill = new Timer(2500);
            SpellManager.CastSpellByIdLUA(139);
            // Renew.Launch();
        }

        if (ObjectManager.Me.HealthPercent < 45 && ObjectManager.Me.BarTwoPercentage > 25)
        {
            if (Inner_Focus.KnownSpell && Inner_Focus.IsSpellUsable)
            {
                SpellManager.CastSpellByIdLUA(89485);
                // Inner_Focus.Launch();
            }

            while (ObjectManager.Me.HealthPercent < 70)
            {
                if (Renew.KnownSpell && Renew.IsSpellUsable && !Renew.HaveBuff && renewchill.IsReady)
                {
                    renewchill = new Timer(2500);
                    SpellManager.CastSpellByIdLUA(139);
                    // Renew.Launch();
                }

                if (Power_Word_Shield.KnownSpell && Power_Word_Shield.IsSpellUsable && !Power_Word_Shield.HaveBuff)
                {
                    SpellManager.CastSpellByIdLUA(17);
                    // Power_Word_Shield.Launch();
                }

                if (Renew.HaveBuff && ObjectManager.GetNumberAttackPlayer() < 2)
                {
                    if (Greater_Heal.KnownSpell && Greater_Heal.IsSpellUsable)
                    {
                        SpellManager.CastSpellByIdLUA(2060);
                        // Greater_Heal.Launch();
                        Thread.Sleep(2000);
                    }

                    if (Flash_Heal.KnownSpell && Flash_Heal.IsSpellUsable && !Greater_Heal.KnownSpell)
                    {
                        SpellManager.CastSpellByIdLUA(2061);
                        // Flash_Heal.Launch();
                        Thread.Sleep(1200);
                    }
                }

                if (Renew.HaveBuff && Flash_Heal.KnownSpell && Flash_Heal.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() > 1)
                {
                    SpellManager.CastSpellByIdLUA(2061);
                    // Flash_Heal.Launch();
                    Thread.Sleep(1200);
                }

                if (ObjectManager.Me.BarTwoPercentage < 10) return;
            }
        }

    }

    public bool hardmob()
    {
        if (((ObjectManager.Target.MaxHealth * 100) / ObjectManager.Me.MaxHealth) > 100)
        {
            return true;
        }
        return false;
    }
}
#endregion

#region Rogue
public class RogueCom
{
    #region InitializeSpell

    Spell Blade_Flurry = new Spell("Blade Flurry");
    Spell Sinister_Strike = new Spell("Sinister Strike");
    Spell Slice_and_Dice = new Spell("Slice and Dice");
    Spell Premeditation = new Spell("Premeditation");
    Spell Revealing_Strike = new Spell("Revealing Strike");
    Spell Rupture = new Spell("Rupture");
    Spell Eviscerate = new Spell("Eviscerate");
    Spell Adrenaline_Rush = new Spell("Adrenaline Rush");
    Spell Killing_Spree = new Spell("Killing Spree");
    Spell Kick = new Spell("Kick");
    Spell Evasion = new Spell("Evasion");
    Spell Sprint = new Spell("Sprint");
    Spell Combat_Readiness = new Spell("Combat Readiness");
    Spell Cloak_of_Shadows = new Spell("Cloak of Shadows");
    Spell Stealth = new Spell("Stealth");
    Spell Sap = new Spell("Sap");
    Spell Pick_Pocket = new Spell("Pick Pocket");
    Spell Recuperate = new Spell("Recuperate");
    Spell Vanish = new Spell("Vanish");
    Spell Kidney_Shot = new Spell("Kidney Shot");
    Spell Cheap_Shot = new Spell("Cheap Shot");

    Spell Deadly_Poison = new Spell("Deadly Poison");
    Spell Wound_Poison = new Spell("Wound Poison");
    Spell Instant_Poison = new Spell("Instant Poison");
    Spell Crippling_Poison = new Spell("Crippling Poison");
    Spell MindNumbing_Poison = new Spell("Mind-Numbing Poison");

    Timer Main_Poison_Timer = new Timer(0);
    Timer Off_Poison_Timer = new Timer(0);
    #endregion InitializeSpell

    public RogueCom()
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
                    if (ObjectManager.Me.Target != lastTarget && ObjectManager.Target.GetDistance <= 50.0f)
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
        if (!Stealth.HaveBuff && Stealth.KnownSpell && Stealth.IsSpellUsable)
            Stealth.Launch();
    }

    public void Patrolling()
    {
        if (Off_Poison_Timer.IsReady && Main_Poison_Timer.IsReady)
        {
            if (Usefuls.InGame && !Usefuls.IsLoadingOrConnecting)
            {
                if (ItemsManager.GetItemCountByIdLUA(2892) > 0 || ItemsManager.GetItemCountByIdLUA(6947) > 0 || ItemsManager.GetItemCountByIdLUA(10918) > 0)
                    Poisons();
            }
        }
    }

    public void Combat()
    {
        AvoidMelee();
        Heal();
        BuffCombat();
        Decast();

        while (Stealth.HaveBuff && Cheap_Shot.KnownSpell && !Cheap_Shot.TargetHaveBuff)
        {
            if (!Sap.TargetHaveBuff && Sap.KnownSpell && Sap.IsSpellUsable && Sap.IsDistanceGood)
                Sap.Launch();

            if (Cheap_Shot.IsSpellUsable && Cheap_Shot.IsDistanceGood)
            {
                Cheap_Shot.Launch();
                return;
            }
        }

        if (ObjectManager.GetNumberAttackPlayer() >= 2 && Blade_Flurry.KnownSpell && Blade_Flurry.IsSpellUsable && Blade_Flurry.IsDistanceGood)
            Blade_Flurry.Launch();

        if (ObjectManager.Me.HealthPercent <= 35 && !Kidney_Shot.TargetHaveBuff && Kidney_Shot.KnownSpell && Kidney_Shot.IsSpellUsable && Kidney_Shot.IsDistanceGood)
            Kidney_Shot.Launch();

        if (ObjectManager.Me.ComboPoint > 1 && ObjectManager.Me.ComboPoint <= 3 && Slice_and_Dice.KnownSpell && !Slice_and_Dice.HaveBuff)
        {
            if (Slice_and_Dice.IsSpellUsable && Slice_and_Dice.IsDistanceGood)
            {
                Slice_and_Dice.Launch();
                return;
            }
        }

        if (ObjectManager.Me.ComboPoint >= 4 && Eviscerate.KnownSpell)
        {
            if (Eviscerate.IsSpellUsable && Eviscerate.IsDistanceGood)
            {
                Eviscerate.Launch();
                return;
            }
        }

        if (Sinister_Strike.KnownSpell && Sinister_Strike.IsSpellUsable && Sinister_Strike.IsDistanceGood)
        {
            Sinister_Strike.Launch();
            return;
        }
    }

    public void Heal()
    {

        if (!Recuperate.HaveBuff && ObjectManager.Me.ComboPoint <= 3 && ObjectManager.Me.ComboPoint > 0 &&
            ObjectManager.Me.HealthPercent <= 50 && Recuperate.KnownSpell && Recuperate.IsSpellUsable && Recuperate.IsDistanceGood)
        {
            Recuperate.Launch();
        }

        if (ObjectManager.Me.HealthPercent <= 10 && Vanish.KnownSpell && Vanish.IsSpellUsable && Vanish.IsDistanceGood)
        {
            Vanish.Launch();
            Thread.Sleep(5000);
        }

        if (ObjectManager.GetNumberAttackPlayer() > 3 && Vanish.KnownSpell && Vanish.IsSpellUsable && Vanish.IsDistanceGood)
        {
            Vanish.Launch();
            Thread.Sleep(5000);
            return;
        }

    }

    public void Poisons()
    {
        if (Off_Poison_Timer.IsReady)
        {
            ObjectManager.Me.forceIsCast = true;

            if (ObjectManager.Me.GetMove)
            {
                MovementManager.StopMove();
                Thread.Sleep(2000);
            }

            if (ItemsManager.GetItemCountByIdLUA(2892) > 0)
            {
                Lua.RunMacroText("/use item:2892");
                Thread.Sleep(20);
                Lua.RunMacroText("/use 17");
                Logging.WriteFight("Deadly Poison");
                Thread.Sleep(5000);
                Off_Poison_Timer = new Timer(1000 * 3600);
            }

            else if (ItemsManager.GetItemCountByIdLUA(6947) > 0)
            {
                Lua.RunMacroText("/use item:6947");
                Thread.Sleep(20);
                Lua.RunMacroText("/use 17");
                Logging.WriteFight("Instant Poison");
                Thread.Sleep(5000);
                Off_Poison_Timer = new Timer(1000 * 3600);
            }

            else if (ItemsManager.GetItemCountByIdLUA(10918) > 0)
            {
                Lua.RunMacroText("/use item:10918");
                Thread.Sleep(20);
                Lua.RunMacroText("/use 17");
                Logging.WriteFight("Wound Poison");
                Thread.Sleep(5000);
                Off_Poison_Timer = new Timer(1000 * 3600);
            }

            ObjectManager.Me.forceIsCast = false;
        }

        if (Main_Poison_Timer.IsReady)
        {
            ObjectManager.Me.forceIsCast = true;

            if (ObjectManager.Me.GetMove)
            {
                MovementManager.StopMove();
                Thread.Sleep(2000);
            }

            if (ItemsManager.GetItemCountByIdLUA(6947) > 0)
            {
                Lua.RunMacroText("/use item:6947");
                Thread.Sleep(20);
                Lua.RunMacroText("/use 16");
                Logging.WriteFight("Instant Poison");
                Thread.Sleep(5000);
                Main_Poison_Timer = new Timer(1000 * 3600);
            }

            else if (ItemsManager.GetItemCountByIdLUA(2892) > 0)
            {
                Lua.RunMacroText("/use item:2892");
                Thread.Sleep(20);
                Lua.RunMacroText("/use 16");
                Logging.WriteFight("Deadly Poison");
                Thread.Sleep(5000);
                Main_Poison_Timer = new Timer(1000 * 3600);
            }

            else if (ItemsManager.GetItemCountByIdLUA(10918) > 0)
            {
                Lua.RunMacroText("/use item:10918");
                Thread.Sleep(20);
                Lua.RunMacroText("/use 16");
                Logging.WriteFight("Wound Poison");
                Thread.Sleep(5000);
                Main_Poison_Timer = new Timer(1000 * 3600);
            }

            ObjectManager.Me.forceIsCast = false;
        }
    }

    public void BuffCombat()
    {
        if (ObjectManager.Target.GetDistance < 5 && Adrenaline_Rush.KnownSpell && Adrenaline_Rush.IsSpellUsable)
            Adrenaline_Rush.Launch();

        if (ObjectManager.Target.GetDistance < 5 && Evasion.KnownSpell && Evasion.IsSpellUsable)
            Evasion.Launch();

        if (Combat_Readiness.KnownSpell && Combat_Readiness.IsSpellUsable)
            Combat_Readiness.Launch();

        Lua.RunMacroText("/use 13");
        Lua.RunMacroText("/use 14");
    }

    private void Decast()
    {
        if (ObjectManager.Target.IsCast && Kick.KnownSpell && Kick.IsSpellUsable && Kick.IsDistanceGood)
            Kick.Launch();

        if (ObjectManager.Target.IsCast && Cloak_of_Shadows.KnownSpell && Cloak_of_Shadows.IsSpellUsable)
            Cloak_of_Shadows.Launch();
    }

    private void AvoidMelee()
    {
        if (ObjectManager.Target.GetDistance < 1)
            Keyboard.DownKey(nManager.Wow.Memory.WowProcess.MainWindowHandle, "{DOWN}");
    }
}
public class RogueAssa
{
    #region InitializeSpell

    Spell Blade_Flurry = new Spell("Blade Flurry");
    Spell Sinister_Strike = new Spell("Sinister Strike");
    Spell Slice_and_Dice = new Spell("Slice and Dice");
    Spell Premeditation = new Spell("Premeditation");
    Spell Revealing_Strike = new Spell("Revealing Strike");
    Spell Rupture = new Spell("Rupture");
    Spell Eviscerate = new Spell("Eviscerate");
    Spell Adrenaline_Rush = new Spell("Adrenaline Rush");
    Spell Killing_Spree = new Spell("Killing Spree");
    Spell Kick = new Spell("Kick");
    Spell Evasion = new Spell("Evasion");
    Spell Sprint = new Spell("Sprint");
    Spell Combat_Readiness = new Spell("Combat Readiness");
    Spell Cloak_of_Shadows = new Spell("Cloak of Shadows");
    Spell Stealth = new Spell("Stealth");
    Spell Sap = new Spell("Sap");
    Spell Pick_Pocket = new Spell("Pick Pocket");
    Spell Recuperate = new Spell("Recuperate");
    Spell Vanish = new Spell("Vanish");
    Spell Kidney_Shot = new Spell("Kidney Shot");
    Spell Cheap_Shot = new Spell("Cheap Shot");
    Spell Mutilate = new Spell("Mutilate");
    Spell Envenom = new Spell("Envenom");
    Spell Vendetta = new Spell("Vendetta");
    Spell Cold_Blood = new Spell("Cold Blood");

    Spell Deadly_Poison = new Spell("Deadly Poison");
    Spell Wound_Poison = new Spell("Wound Poison");
    Spell Instant_Poison = new Spell("Instant Poison");
    Spell Crippling_Poison = new Spell("Crippling Poison");
    Spell MindNumbing_Poison = new Spell("Mind-Numbing Poison");

    Timer Main_Poison_Timer = new Timer(0);
    Timer Off_Poison_Timer = new Timer(0);

    #endregion InitializeSpell

    public RogueAssa()
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
                    if (ObjectManager.Me.Target != lastTarget && ObjectManager.Target.GetDistance <= 50.0f)
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
        if (!Stealth.HaveBuff && Stealth.KnownSpell && Stealth.IsSpellUsable)
            Stealth.Launch();
    }

    public void Patrolling()
    {
        if (Off_Poison_Timer.IsReady && Main_Poison_Timer.IsReady)
        {
            if (Usefuls.InGame && !Usefuls.IsLoadingOrConnecting)
            {
                if (ItemsManager.GetItemCountByIdLUA(2892) > 0 || ItemsManager.GetItemCountByIdLUA(6947) > 0 || ItemsManager.GetItemCountByIdLUA(10918) > 0)
                    Poisons();
            }
        }
    }

    public void Combat()
    {
        AvoidMelee();
        Heal();
        BuffCombat();
        Decast();

        if ((Stealth.HaveBuff || Vanish.HaveBuff) && Cheap_Shot.KnownSpell && !Cheap_Shot.TargetHaveBuff)
        {
            if (!Sap.TargetHaveBuff && Sap.KnownSpell && Sap.IsSpellUsable && Sap.IsDistanceGood)
                Sap.Launch();

            if (Cheap_Shot.IsSpellUsable && Cheap_Shot.IsDistanceGood)
            {
                Cheap_Shot.Launch();
                return;
            }
        }

        if (ObjectManager.Me.HealthPercent <= 30 && !Kidney_Shot.TargetHaveBuff && Kidney_Shot.KnownSpell && Kidney_Shot.IsSpellUsable && Kidney_Shot.IsDistanceGood)
            Kidney_Shot.Launch();

        if (ObjectManager.Me.ComboPoint <= 3 && ObjectManager.Me.ComboPoint > 1 && Slice_and_Dice.KnownSpell && !Slice_and_Dice.HaveBuff)
        {
            if (Slice_and_Dice.IsSpellUsable && Slice_and_Dice.IsDistanceGood)
            {
                Slice_and_Dice.Launch();
                return;
            }
        }

        if (ObjectManager.Me.ComboPoint <= 3 && ObjectManager.Me.ComboPoint > 1 && Rupture.KnownSpell && Slice_and_Dice.HaveBuff && !Rupture.TargetHaveBuff)
        {
            if (Rupture.IsSpellUsable && Rupture.IsDistanceGood)
            {
                Rupture.Launch();
                return;
            }
        }

        if (ObjectManager.Me.ComboPoint >= 4 && Envenom.KnownSpell)
        {
            if (Envenom.IsSpellUsable && Envenom.IsDistanceGood)
            {
                Envenom.Launch();
                return;
            }

            if (ObjectManager.Me.BarTwoPercentage > 35 && !Envenom.IsSpellUsable && Envenom.IsDistanceGood)
            {
                Eviscerate.Launch();
                return;
            }
        }

        if (ObjectManager.Me.ComboPoint >= 4 && !Envenom.KnownSpell && Eviscerate.KnownSpell)
        {
            if (Eviscerate.IsSpellUsable && Eviscerate.IsDistanceGood)
            {
                Eviscerate.Launch();
                return;
            }
        }

        if (Mutilate.KnownSpell && Mutilate.IsSpellUsable && Mutilate.IsDistanceGood)
            Mutilate.Launch();
    }

    public void Heal()
    {
        if (!Recuperate.HaveBuff && ObjectManager.Me.ComboPoint <= 3 && ObjectManager.Me.ComboPoint > 1 &&
            ObjectManager.Me.HealthPercent <= 50 && Recuperate.KnownSpell && Recuperate.IsSpellUsable && Recuperate.IsDistanceGood)
        {
            Recuperate.Launch();
        }

        if (ObjectManager.Me.HealthPercent <= 10 && Vanish.KnownSpell && Vanish.IsSpellUsable && Vanish.IsDistanceGood)
        {
            Vanish.Launch();
            Thread.Sleep(5000);
            return;
        }

        if (ObjectManager.GetNumberAttackPlayer() >= 3 && Vanish.KnownSpell && Vanish.IsSpellUsable && Vanish.IsDistanceGood)
        {
            Vanish.Launch();
            Thread.Sleep(5000);
            return;
        }
    }

    public void Poisons()
    {
        if (Off_Poison_Timer.IsReady)
        {
            ObjectManager.Me.forceIsCast = true;

            if (ObjectManager.Me.GetMove)
            {
                MovementManager.StopMove();
                Thread.Sleep(2000);
            }

            if (ItemsManager.GetItemCountByIdLUA(2892) > 0)
            {
                Lua.RunMacroText("/use item:2892");
                Thread.Sleep(20);
                Lua.RunMacroText("/use 17");
                Logging.WriteFight("Deadly Poison");
                Thread.Sleep(5000);
                Off_Poison_Timer = new Timer(1000 * 3600);
            }

            else if (ItemsManager.GetItemCountByIdLUA(6947) > 0)
            {
                Lua.RunMacroText("/use item:6947");
                Thread.Sleep(20);
                Lua.RunMacroText("/use 17");
                Logging.WriteFight("Instant Poison");
                Thread.Sleep(5000);
                Off_Poison_Timer = new Timer(1000 * 3600);
            }

            else if (ItemsManager.GetItemCountByIdLUA(10918) > 0)
            {
                Lua.RunMacroText("/use item:10918");
                Thread.Sleep(20);
                Lua.RunMacroText("/use 17");
                Logging.WriteFight("Wound Poison");
                Thread.Sleep(5000);
                Off_Poison_Timer = new Timer(1000 * 3600);
            }

            ObjectManager.Me.forceIsCast = false;
        }

        if (Main_Poison_Timer.IsReady)
        {
            ObjectManager.Me.forceIsCast = true;

            if (ObjectManager.Me.GetMove)
            {
                MovementManager.StopMove();
                Thread.Sleep(2000);
            }

            if (ItemsManager.GetItemCountByIdLUA(6947) > 0)
            {
                Lua.RunMacroText("/use item:6947");
                Thread.Sleep(20);
                Lua.RunMacroText("/use 16");
                Logging.WriteFight("Instant Poison");
                Thread.Sleep(5000);
                Main_Poison_Timer = new Timer(1000 * 3600);
            }

            else if (ItemsManager.GetItemCountByIdLUA(2892) > 0)
            {
                Lua.RunMacroText("/use item:2892");
                Thread.Sleep(20);
                Lua.RunMacroText("/use 16");
                Logging.WriteFight("Deadly Poison");
                Thread.Sleep(5000);
                Main_Poison_Timer = new Timer(1000 * 3600);
            }

            else if (ItemsManager.GetItemCountByIdLUA(10918) > 0)
            {
                Lua.RunMacroText("/use item:10918");
                Thread.Sleep(20);
                Lua.RunMacroText("/use 16");
                Logging.WriteFight("Wound Poison");
                Thread.Sleep(5000);
                Main_Poison_Timer = new Timer(1000 * 3600);
            }

            ObjectManager.Me.forceIsCast = false;
        }
    }

    public void BuffCombat()
    {
        if (ObjectManager.Me.BarTwoPercentage < 75 && Cold_Blood.KnownSpell && !Cold_Blood.HaveBuff && Cold_Blood.IsSpellUsable)
            Cold_Blood.Launch();

        if (ObjectManager.Target.GetDistance < 5 && Adrenaline_Rush.KnownSpell && Adrenaline_Rush.IsSpellUsable)
            Adrenaline_Rush.Launch();

        if (ObjectManager.Target.GetDistance < 5 && Evasion.KnownSpell && Evasion.IsSpellUsable)
            Evasion.Launch();

        if (Combat_Readiness.KnownSpell && Combat_Readiness.IsSpellUsable)
            Combat_Readiness.Launch();

        if (Vendetta.KnownSpell && Vendetta.IsSpellUsable)
            Vendetta.Launch();
    }

    private void Decast()
    {
        if (ObjectManager.Target.IsCast && Kick.KnownSpell && Kick.IsSpellUsable && Kick.IsDistanceGood)
            Kick.Launch();

        if (ObjectManager.Target.IsCast && Cloak_of_Shadows.KnownSpell && Cloak_of_Shadows.IsSpellUsable)
            Cloak_of_Shadows.Launch();
    }

    private void AvoidMelee()
    {
        if (ObjectManager.Target.GetDistance < 1)
            Keyboard.DownKey(nManager.Wow.Memory.WowProcess.MainWindowHandle, "{DOWN}");
    }
}
public class Rogue
{
    #region InitializeSpell

    Spell Blade_Flurry = new Spell("Blade Flurry");
    Spell Sinister_Strike = new Spell("Sinister Strike");
    Spell Slice_and_Dice = new Spell("Slice and Dice");
    Spell Premeditation = new Spell("Premeditation");
    Spell Revealing_Strike = new Spell("Revealing Strike");
    Spell Rupture = new Spell("Rupture");
    Spell Eviscerate = new Spell("Eviscerate");
    Spell Adrenaline_Rush = new Spell("Adrenaline Rush");
    Spell Killing_Spree = new Spell("Killing Spree");
    Spell Kick = new Spell("Kick");
    Spell Evasion = new Spell("Evasion");
    Spell Sprint = new Spell("Sprint");
    Spell Combat_Readiness = new Spell("Combat Readiness");
    Spell Cloak_of_Shadows = new Spell("Cloak of Shadows");
    Spell Stealth = new Spell("Stealth");
    Spell Sap = new Spell("Sap");
    Spell Pick_Pocket = new Spell("Pick Pocket");
    Spell Recuperate = new Spell("Recuperate");
    Spell Vanish = new Spell("Vanish");
    Spell Kidney_Shot = new Spell("Kidney Shot");
    Spell Cheap_Shot = new Spell("Cheap Shot");

    #endregion InitializeSpell

    public Rogue()
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
                    if (ObjectManager.Me.Target != lastTarget && Sap.IsDistanceGood)
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
        if (ObjectManager.Target.GetDistance <= 20 && !Stealth.HaveBuff && Stealth.KnownSpell && Stealth.IsSpellUsable)
        {
            Stealth.Launch();
            Thread.Sleep(1500);
        }
    }

    public void Combat()
    {
        AvoidMelee();
        Heal();
        BuffCombat();
        Decast();

        while (ObjectManager.Me.ComboPoint >= 4 && Eviscerate.KnownSpell)
        {
            if (Eviscerate.IsSpellUsable && Eviscerate.IsDistanceGood)
            {
                Eviscerate.Launch();
                return;
            }
        }

        if (Sinister_Strike.KnownSpell && Sinister_Strike.IsSpellUsable && Sinister_Strike.IsDistanceGood)
        {
            Sinister_Strike.Launch();
            return;
        }

    }

    public void Patrolling()
    {

    }

    public void Heal()
    {

    }

    public void BuffCombat()
    {
        if (ObjectManager.Target.GetDistance < 5 && Evasion.KnownSpell && Evasion.IsSpellUsable)
            Evasion.Launch();
    }

    private void Decast()
    {

    }

    private void AvoidMelee()
    {
        if (ObjectManager.Target.GetDistance < 1)
            Keyboard.DownKey(nManager.Wow.Memory.WowProcess.MainWindowHandle, "{DOWN}");
    }

}
#endregion

#region Warrior
public class Arms
{
    #region InitializeSpell

    // ARMS ONLY
    Spell Mortal_Strike = new Spell("Mortal Strike");
    Spell Sweeping_Strikes = new Spell("Sweeping Strikes");
    Spell Deadly_Calm = new Spell("Deadly Calm");
    Spell Juggernaut = new Spell("Juggernaut");
    Spell Throwdown = new Spell("Throwdown");
    Spell Bladestorm = new Spell("Bladestorm");

    // DPS
    Spell Strike = new Spell("Strike");
    Spell Rend = new Spell("Rend");
    Spell Victory_Rush = new Spell("Victory Rush");
    Spell Heroic_Strike = new Spell("Heroic Strike");
    Spell Overpower = new Spell("Overpower");
    Spell Heroic_Throw = new Spell("Heroic Throw");
    Spell Execute = new Spell("Execute");
    Spell Cleave = new Spell("Cleave");
    Spell Slam = new Spell("Slam");


    // BUFF & HELPING
    Spell Battle_Stance = new Spell("Battle Stance");
    Spell Defensive_Stance = new Spell("Defensive Stance");
    Spell Berserker_Stance = new Spell("Berserker Stance");
    Spell Battle_Shout = new Spell("Battle Shout");
    Spell Demoralizing_Shout = new Spell("Demoralizing Shout");
    Spell Commanding_Shout = new Spell("Commanding Shout");
    Spell Thunder_Clap = new Spell("Thunder Clap");
    Spell Charge = new Spell("Charge");
    Spell Pummel = new Spell("Pummel");
    Spell Berserker_Rage = new Spell("Berserker Rage");
    Spell Inner_Rage = new Spell("Inner Rage");
    Spell Colossus_Smash = new Spell("Colossus Smash");

    // TIMER
    Timer look = new Timer(0);
    Timer fighttimer = new Timer(0);
    Timer rendchill = new Timer(0);


    // Profession & Racials
    Spell Arcane_Torrent = new Spell("Arcane Torrent");
    Spell Lifeblood = new Spell("Lifeblood");
    Spell Stoneform = new Spell("Stoneform");
    Spell Tailoring = new Spell("Tailoring");
    Spell Leatherworking = new Spell("Leatherworking");
    Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    Spell War_Stomp = new Spell("War Stomp");
    Spell Berserking = new Spell("Berserking");

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

                    if (fighttimer.IsReady && ObjectManager.Target.HealthPercent > 90 && ObjectManager.Me.Target > 0 && ObjectManager.GetNumberAttackPlayer() < 2)
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

        if (Berserker_Rage.KnownSpell && Berserker_Rage.IsSpellUsable && ObjectManager.Me.BarTwoPercentage < 50)
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

        if (Rend.KnownSpell && Rend.IsSpellUsable && Rend.IsDistanceGood && !Rend.TargetHaveBuff && rendchill.IsReady)
        {
            rendchill = new Timer(2500);

            Rend.Launch();
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

        if (Mortal_Strike.KnownSpell && Mortal_Strike.IsSpellUsable && Mortal_Strike.IsDistanceGood && (ObjectManager.GetNumberAttackPlayer() < 2 || (ObjectManager.GetNumberAttackPlayer() > 1 && !Cleave.KnownSpell)))
        {

            Mortal_Strike.Launch();
        }

        if (Overpower.KnownSpell && Overpower.IsSpellUsable && Overpower.IsDistanceGood && (ObjectManager.GetNumberAttackPlayer() < 2 || (ObjectManager.GetNumberAttackPlayer() > 1 && !Cleave.KnownSpell)))
        {

            Overpower.Launch();
        }

        if (!Mortal_Strike.KnownSpell && Strike.IsSpellUsable && Strike.IsDistanceGood && (ObjectManager.GetNumberAttackPlayer() < 2 || (ObjectManager.GetNumberAttackPlayer() > 1 && !Cleave.KnownSpell)))
        {

            Strike.Launch();
        }

        if (Cleave.KnownSpell && Cleave.IsSpellUsable && Cleave.IsDistanceGood && ObjectManager.GetNumberAttackPlayer() > 1)
        {

            Cleave.Launch();
        }

        if (Heroic_Strike.KnownSpell && Heroic_Strike.IsSpellUsable && Heroic_Strike.IsDistanceGood && ObjectManager.Me.BarTwoPercentage > 70)
        {

            Heroic_Strike.Launch();
        }


        if (Pummel.KnownSpell && Pummel.IsSpellUsable && Pummel.IsDistanceGood &&
            ObjectManager.Target.IsCast)
        {

            Pummel.Launch();
        }

        if (Slam.KnownSpell && Slam.IsSpellUsable && Slam.IsDistanceGood && !Overpower.IsSpellUsable && !Mortal_Strike.IsSpellUsable &&
            (ObjectManager.GetNumberAttackPlayer() < 2 || (ObjectManager.GetNumberAttackPlayer() > 1 && !Cleave.KnownSpell)))
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

        if ((ObjectManager.GetNumberAttackPlayer() > 1 || hardmob()) &&
            ObjectManager.Me.HealthPercent < 65 &&
            ObjectManager.Target.GetDistance < 5 &&
            War_Stomp.KnownSpell && War_Stomp.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(20549);
            // War_Stomp.Launch();
        }

        if (!Battle_Stance.HaveBuff && Battle_Stance.IsSpellUsable)
        {
            Battle_Stance.Launch();
        }

        if (Thunder_Clap.KnownSpell && Thunder_Clap.IsSpellUsable && !Thunder_Clap.TargetHaveBuff && !Strike.IsSpellUsable && ObjectManager.Target.GetDistance < 9 &&
            (ObjectManager.Me.BarTwoPercentage > 50 || hardmob() || ObjectManager.GetNumberAttackPlayer() > 1))
        {

            Thunder_Clap.Launch();
        }

        if (Demoralizing_Shout.KnownSpell && Demoralizing_Shout.IsSpellUsable && !Demoralizing_Shout.TargetHaveBuff && !Strike.IsSpellUsable && ObjectManager.Target.GetDistance < 9 &&
            (ObjectManager.Me.BarTwoPercentage > 50 || hardmob() || ObjectManager.GetNumberAttackPlayer() > 1))
        {

            Demoralizing_Shout.Launch();
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

        if (Victory_Rush.KnownSpell && Victory_Rush.IsSpellUsable && Victory_Rush.IsDistanceGood)
        {
            Victory_Rush.Launch();
        }

    }

    public bool hardmob()
    {
        if (((ObjectManager.Target.MaxHealth * 100) / ObjectManager.Me.MaxHealth) > 100)
        {
            return true;
        }
        return false;
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

            Devastate.Launch();
            return;
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

            if ((ObjectManager.Me.BarTwoPercentage > 70 || Inner_Rage.HaveBuff) &&
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

            if ((ObjectManager.Me.BarTwoPercentage > 70 || Inner_Rage.HaveBuff) &&
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

            if ((ObjectManager.Me.BarTwoPercentage > 70 || Inner_Rage.HaveBuff) &&
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

            if ((ObjectManager.Me.BarTwoPercentage > 70 || Inner_Rage.HaveBuff) &&
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

            if ((ObjectManager.Me.BarTwoPercentage > 70 || Inner_Rage.HaveBuff) &&
                Heroic_Strike.KnownSpell &&
                Heroic_Strike.IsSpellUsable)
            {
                Heroic_Strike.Launch();
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
#endregion

#region Hunter
public class Survival
{
    #region InitializeSpell

    // Survival Only
    Spell Explosive_Shot = new Spell("Explosive Shot");
    Spell Counterattack = new Spell("Counterattack");
    Spell Black_Arrow = new Spell("Black Arrow");

    // DPS
    Spell Raptor_Strike = new Spell("Raptor Strike");
    Spell Arcane_Shot = new Spell("Arcane Shot");
    Spell Steady_Shot = new Spell("Steady Shot");
    Spell Serpent_Sting = new Spell("Serpent Sting");
    Spell Multi_Shot = new Spell("Multi-Shot");
    Spell Kill_Shot = new Spell("Kill Shot");
    Spell Explosive_Trap = new Spell("Explosive Trap");
    Spell Cobra_Shot = new Spell("Cobra Shot");
    Spell Immolation_Trap = new Spell("Immolation Trap");

    // BUFF & HELPING
    Spell Concussive_Shot = new Spell("Concussive Shot");
    Spell Aspect_of_the_Hawk = new Spell("Aspect of the Hawk");
    Spell Disengage = new Spell("Disengage");
    Spell Hunters_Mark = new Spell("Hunter's Mark");
    Spell Scatter_Shot = new Spell("Scatter Shot");	// 19503
    Spell Feign_Death = new Spell("Feign Death");	//	5384
    Spell Snake_Trap = new Spell("Snake Trap");
    Spell Ice_Trap = new Spell("Ice Trap");
    Spell Freezing_Trap = new Spell("Freezing Trap");
    Spell Trap_Launcher = new Spell("Trap Launcher");	//	77769
    Spell Rapid_Fire = new Spell("Rapid Fire");	//	3045
    Spell Misdirection = new Spell("Misdirection");
    Spell Deterrence = new Spell("Deterrence");	//	19263
    Spell Wing_Clip = new Spell("Wing Clip");

    // PET
    Spell Kill_Command = new Spell("Kill Command");
    Spell Mend_Pet = new Spell("Mend Pet");	//	136
    Spell Revive_Pet = new Spell("Revive Pet");	//	982
    Spell Call_Pet = new Spell("Call Pet 1");	//	883

    // TIMER
    Timer look = new Timer(0);
    Timer fighttimer = new Timer(0);
    Timer petheal = new Timer(0);
    Timer traplaunchertimer = new Timer(0);
    Timer disengagetimer = new Timer(0);
    Timer Serpent_Sting_debuff = new Timer(0);
    Timer mountchill = new Timer(0);

    // Profession & Racials
    Spell Arcane_Torrent = new Spell("Arcane Torrent");
    Spell Lifeblood = new Spell("Lifeblood");
    Spell Stoneform = new Spell("Stoneform");
    Spell Tailoring = new Spell("Tailoring");
    Spell Leatherworking = new Spell("Leatherworking");
    Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    Spell War_Stomp = new Spell("War Stomp");
    Spell Berserking = new Spell("Berserking");

    #endregion InitializeSpell

    public Survival()
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

                    if (fighttimer.IsReady && ObjectManager.Target.HealthPercent > 90 && ObjectManager.Me.Target > 0 && ObjectManager.GetNumberAttackPlayer() < 2)
                    {
                        Logging.WriteFight(" - Target Evading - ");
                        break;
                    }
                }
            }
            if (ObjectManager.Me.IsMounted) mountchill = new Timer(2000);
            Thread.Sleep(350);
        }
    }

    public void pull()
    {

        if (hardmob()) Logging.WriteFight(" -  Pull Hard Mob - ");
        if (!hardmob()) Logging.WriteFight(" -  Pull Easy Mob - ");
        fighttimer = new Timer(60000);

        if (Concussive_Shot.KnownSpell && Concussive_Shot.IsSpellUsable && Concussive_Shot.IsDistanceGood)
        {
            SpellManager.CastSpellByIdLUA(5116);
            // Concussive_Shot.Launch();
        }
    }

    public void buffoutfight()
    {

        if (Fight.InFight || ObjectManager.Me.IsDeadMe) return;

        pet();

        if (!ObjectManager.Me.HaveBuff(79640) &&
            ItemsManager.GetItemCountByIdLUA(58149) == 1)
        {
            Logging.WriteFight("Use Alchi Flask");
            Lua.RunMacroText("/use item:58149");
        }

        if (Aspect_of_the_Hawk.KnownSpell && Aspect_of_the_Hawk.IsSpellUsable &&
            !Aspect_of_the_Hawk.HaveBuff)
        {
            SpellManager.CastSpellByIdLUA(13165);
            // Aspect_of_the_Hawk.Launch();
        }

    }

    public void fight()
    {
        pet();
        selfheal();
        buffinfight();
        if (ObjectManager.GetNumberAttackPlayer() > 1) fighttimer = new Timer(60000);

        if (ObjectManager.GetNumberAttackPlayer() > 2 && Explosive_Trap.IsSpellUsable && Trap_Launcher.IsSpellUsable && Arcane_Shot.IsDistanceGood)
        {
            traplaunchertimer = new Timer(1100);
            Trap_Launcher.Launch();
            while (!traplaunchertimer.IsReady)
            {
                if (Explosive_Trap.KnownSpell && Explosive_Trap.IsSpellUsable)
                {
                    SpellManager.CastSpellByIDAndPosition(13813, ObjectManager.Target.Position);
                }
            }
        }

        if (ObjectManager.GetNumberAttackPlayer() < 2 && Immolation_Trap.IsSpellUsable && Trap_Launcher.IsSpellUsable && Arcane_Shot.IsDistanceGood &&
            !ObjectManager.Target.IsTargetingMe && ObjectManager.Target.HealthPercent > 70)
        {
            traplaunchertimer = new Timer(1100);
            Trap_Launcher.Launch();
            while (!traplaunchertimer.IsReady)
            {
                if (Immolation_Trap.KnownSpell && Immolation_Trap.IsSpellUsable)
                {
                    SpellManager.CastSpellByIDAndPosition(13795, ObjectManager.Target.Position);
                }
            }
        }

        if (ObjectManager.GetNumberAttackPlayer() < 2 && Immolation_Trap.IsSpellUsable && !Trap_Launcher.KnownSpell && Arcane_Shot.IsDistanceGood)
        {

            Immolation_Trap.Launch();
        }



        if (Kill_Shot.KnownSpell && Kill_Shot.IsSpellUsable && Kill_Shot.IsDistanceGood)
        {
            SpellManager.CastSpellByIdLUA(53351);
            // Kill_Shot.Launch();
        }

        if (Hunters_Mark.KnownSpell && Hunters_Mark.IsSpellUsable && Hunters_Mark.IsDistanceGood && !Hunters_Mark.TargetHaveBuff)
        {
            SpellManager.CastSpellByIdLUA(1130);
            // Hunters_Mark.Launch();
        }

        if ((ObjectManager.GetNumberAttackPlayer() > 2 || hardmob()) && Misdirection.KnownSpell && Misdirection.IsSpellUsable)
        {
            Lua.RunMacroText("/cast [@pet] Misdirection");
            Lua.RunMacroText("/cast [@pet] Irreführung");
            Lua.RunMacroText("/cast [@pet] Détournement");
            Lua.RunMacroText("/cast [@pet] Перенаправление");
            Lua.RunMacroText("/cast [@pet] Redirección");
            Lua.RunMacroText("/cast [@pet] Redirecionar");
        }

        if (ObjectManager.Me.HaveBuff(56453))
        {
            if (Explosive_Shot.KnownSpell && Explosive_Shot.IsSpellUsable && Explosive_Shot.IsDistanceGood)
            {
                SpellManager.CastSpellByIdLUA(53301);
                // Explosive_Shot.Launch();
            }
            if (Arcane_Shot.KnownSpell && Arcane_Shot.IsSpellUsable && Arcane_Shot.IsDistanceGood)
            {
                SpellManager.CastSpellByIdLUA(3044);
                // Arcane_Shot.Launch();
            }
            return;
        }

        if (Concussive_Shot.KnownSpell && Concussive_Shot.IsSpellUsable && Concussive_Shot.IsDistanceGood && !ObjectManager.Target.HaveBuff(1978))
        {
            SpellManager.CastSpellByIdLUA(5116);
            // Concussive_Shot.Launch();
        }

        if (!ObjectManager.Target.HaveBuff(1978) && Serpent_Sting_debuff.IsReady && Arcane_Shot.IsDistanceGood)
        {
            Serpent_Sting_debuff = new Timer(2500);
            Serpent_Sting.Launch();
        }

        if (!ObjectManager.Target.HaveBuff(1978) && !Serpent_Sting_debuff.IsReady)
        {
            if (Kill_Shot.KnownSpell && Kill_Shot.IsSpellUsable && Kill_Shot.IsDistanceGood)
            {
                Kill_Shot.Launch();
            }
        }

        if ((ObjectManager.GetNumberAttackPlayer() > 1 || hardmob()) && Snake_Trap.IsSpellUsable && Arcane_Shot.IsDistanceGood && !ObjectManager.Target.GetMove && Trap_Launcher.KnownSpell)
        {
            traplaunchertimer = new Timer(1100);
            Trap_Launcher.Launch();
            while (!traplaunchertimer.IsReady)
            {
                if (Snake_Trap.KnownSpell && Snake_Trap.IsSpellUsable && Trap_Launcher.HaveBuff)
                {
                    SpellManager.CastSpellByIDAndPosition(34600, ObjectManager.Target.Position);
                }
            }
        }

        if (Freezing_Trap.KnownSpell && Freezing_Trap.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() > 1)
        {
            SpellManager.CastSpellByIdLUA(1499);
            // Freezing_Trap.Launch();
        }

        if (ObjectManager.Target.HaveBuff(1978))
        {
            if (Multi_Shot.KnownSpell && Multi_Shot.IsSpellUsable && Multi_Shot.IsDistanceGood && ObjectManager.GetNumberAttackPlayer() > 1)
            {
                SpellManager.CastSpellByIdLUA(2643);
                // Multi_Shot.Launch();
            }
            if (Explosive_Shot.KnownSpell && Explosive_Shot.IsSpellUsable && Explosive_Shot.IsDistanceGood && ObjectManager.Me.BarTwoPercentage > 70)
            {
                SpellManager.CastSpellByIdLUA(53301);
                // Explosive_Shot.Launch();
            }

            if (Black_Arrow.KnownSpell && Black_Arrow.IsSpellUsable && Black_Arrow.IsDistanceGood && !Explosive_Shot.IsSpellUsable)
            {
                SpellManager.CastSpellByIdLUA(3674);
                // Black_Arrow.Launch();
            }

            if (Arcane_Shot.KnownSpell && Arcane_Shot.IsSpellUsable && Arcane_Shot.IsDistanceGood && !Explosive_Shot.IsSpellUsable && !Black_Arrow.IsSpellUsable)
            {
                SpellManager.CastSpellByIdLUA(3044);
                // Arcane_Shot.Launch();
            }
        }

        if (ObjectManager.Me.BarTwoPercentage < 70 && ObjectManager.Target.HaveBuff(1978))
        {
            if (Steady_Shot.KnownSpell && Steady_Shot.IsSpellUsable && Steady_Shot.IsDistanceGood && !Cobra_Shot.KnownSpell)
            {
                SpellManager.CastSpellByIdLUA(56641);
                // Steady_Shot.Launch();
            }
            else if (Cobra_Shot.KnownSpell && Cobra_Shot.IsSpellUsable && Cobra_Shot.IsDistanceGood && ObjectManager.Target.HaveBuff(1978))
            {
                SpellManager.CastSpellByIdLUA(77767);
                // Cobra_Shot.Launch();
            }
        }

        if (Arcane_Torrent.KnownSpell && Arcane_Torrent.IsSpellUsable &&
            ObjectManager.Target.IsCast && ObjectManager.Target.GetDistance < 8)
        {
            Arcane_Torrent.Launch();
        }

    }

    private void pet()
    {

        if (ObjectManager.Me.IsMounted || !mountchill.IsReady) return;

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

        if (Mend_Pet.KnownSpell && Mend_Pet.IsSpellUsable && petheal.IsReady &&
            ObjectManager.Pet.Health > 0 && ObjectManager.Pet.HealthPercent < 70)
        {
            petheal = new Timer(9000);
            Mend_Pet.Launch();
        }

        if (Fight.InFight) Lua.RunMacroText("/petattack");

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

        if ((ObjectManager.GetNumberAttackPlayer() > 1 || hardmob()) &&
            ObjectManager.Me.HealthPercent < 65 &&
            ObjectManager.Target.GetDistance < 5 &&
            War_Stomp.KnownSpell && War_Stomp.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(20549);
            // War_Stomp.Launch();
        }

        if (Berserking.KnownSpell && Berserking.IsSpellUsable && Arcane_Shot.IsDistanceGood)
        {
            SpellManager.CastSpellByIdLUA(1454);
            // Berserking.Launch();
        }

        if (Rapid_Fire.KnownSpell && Rapid_Fire.IsSpellUsable && (hardmob() || ObjectManager.GetNumberAttackPlayer() > 2) && Arcane_Shot.IsDistanceGood)
        {
            Rapid_Fire.Launch();
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

        if (Disengage.KnownSpell && Disengage.IsSpellUsable && Disengage.IsDistanceGood &&
            ObjectManager.Target.HealthPercent > 30 && ObjectManager.Target.GetDistance < 5)
        {
            disengagetimer = new Timer(2000);
            while (ObjectManager.Target.GetDistance < 5 && !disengagetimer.IsReady)
                if (Wing_Clip.KnownSpell && Wing_Clip.IsSpellUsable && Wing_Clip.IsDistanceGood && !Wing_Clip.TargetHaveBuff)
                {
                    SpellManager.CastSpellByIdLUA(2974);
                    // Wing_Clip.Launch();
                }
            SpellManager.CastSpellByIdLUA(781);
            // Disengage.Launch();
            if (Concussive_Shot.KnownSpell && Concussive_Shot.IsSpellUsable && Concussive_Shot.IsDistanceGood)
            {
                SpellManager.CastSpellByIdLUA(5116);
                // Concussive_Shot.Launch();
            }
            return;
        }

        if (ObjectManager.Target.GetDistance < 10 && ((Disengage.KnownSpell && !Disengage.IsSpellUsable) || !Disengage.KnownSpell))
        {
            disengagetimer = new Timer(5000);
            while (ObjectManager.Target.GetDistance < 10 || !disengagetimer.IsReady)
            {
                if (!Fight.InFight) return;
                Keyboard.DownKey(nManager.Wow.Memory.WowProcess.MainWindowHandle, "S");
                Thread.Sleep(100);

                if (Mend_Pet.KnownSpell && Mend_Pet.IsSpellUsable && petheal.IsReady &&
                    ObjectManager.Pet.Health > 0 && ObjectManager.Pet.HealthPercent < 70)
                {
                    petheal = new Timer(9000);
                    Mend_Pet.Launch();
                }

                if (Wing_Clip.KnownSpell && Wing_Clip.IsSpellUsable && Wing_Clip.IsDistanceGood && !Wing_Clip.TargetHaveBuff)
                {
                    SpellManager.CastSpellByIdLUA(2974);
                    // Wing_Clip.Launch();
                }

                if (Counterattack.KnownSpell && Counterattack.IsSpellUsable && Counterattack.IsDistanceGood)
                {
                    SpellManager.CastSpellByIdLUA(19306);
                    // Counterattack.Launch();
                }

                if (Raptor_Strike.KnownSpell && Raptor_Strike.IsSpellUsable && Raptor_Strike.IsDistanceGood)
                {
                    SpellManager.CastSpellByIdLUA(2973);
                    // Raptor_Strike.Launch();
                }

                if (Kill_Command.KnownSpell && Kill_Command.IsSpellUsable && Kill_Command.IsDistanceGood)
                {
                    SpellManager.CastSpellByIdLUA(34026);
                    // Kill_Command.Launch();
                }

                if (Kill_Shot.KnownSpell && Kill_Shot.IsSpellUsable && Kill_Shot.IsDistanceGood)
                {
                    SpellManager.CastSpellByIdLUA(53351);
                    // Kill_Shot.Launch();
                }

                if (Explosive_Shot.KnownSpell && Explosive_Shot.IsSpellUsable && Explosive_Shot.IsDistanceGood && ObjectManager.Me.BarTwoPercentage > 70)
                {
                    SpellManager.CastSpellByIdLUA(53301);
                    // Explosive_Shot.Launch();
                }

                if (Black_Arrow.KnownSpell && Black_Arrow.IsSpellUsable && Black_Arrow.IsDistanceGood && !Explosive_Shot.IsSpellUsable)
                {
                    SpellManager.CastSpellByIdLUA(3674);
                    // Black_Arrow.Launch();
                }

                if (Arcane_Shot.KnownSpell && Arcane_Shot.IsSpellUsable && Arcane_Shot.IsDistanceGood)
                {
                    SpellManager.CastSpellByIdLUA(3044);
                    // Arcane_Shot.Launch();
                }

                if (Feign_Death.KnownSpell && Feign_Death.IsSpellUsable)
                {
                    Feign_Death.Launch();
                    Thread.Sleep(3000);
                }

                if (Freezing_Trap.KnownSpell && Freezing_Trap.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() > 1)
                {
                    SpellManager.CastSpellByIdLUA(1499);
                    // Freezing_Trap.Launch();
                }

                if (Scatter_Shot.KnownSpell && Scatter_Shot.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() > 1)
                {
                    Scatter_Shot.Launch();
                }

                if (ObjectManager.Me.HealthPercent < 30 && ObjectManager.Target.IsTargetingMe)
                {
                    if (!Feign_Death.IsSpellUsable && !Scatter_Shot.IsSpellUsable && Deterrence.KnownSpell && Deterrence.KnownSpell)
                    {
                        Deterrence.Launch();
                    }
                }

                Keyboard.DownKey(nManager.Wow.Memory.WowProcess.MainWindowHandle, "{SPACE}");
            }
        }

        if (Feign_Death.KnownSpell && Feign_Death.IsSpellUsable && ObjectManager.Me.HealthPercent < 15 && ObjectManager.Pet.Health > 10)
        {
            Feign_Death.Launch();
            Thread.Sleep(3000);
        }

        if (Feign_Death.KnownSpell && Feign_Death.IsSpellUsable && ObjectManager.Me.HealthPercent < 15 && (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0))
        {
            Feign_Death.Launch();
            Lua.RunMacroText("/cleartarget");
            Thread.Sleep(30000);
        }

    }

    public bool hardmob()
    {
        if (((ObjectManager.Target.MaxHealth * 100) / ObjectManager.Me.MaxHealth) > 110)
        {
            return true;
        }
        return false;
    }
}
public class Marks
{
    #region InitializeSpell

    // Marks Only
    Spell Aimed_Shot = new Spell("Aimed Shot");
    Spell Silencing_Shot = new Spell("Silencing Shot");
    Spell Readiness = new Spell("Readiness");
    Spell Chimera_Shot = new Spell("Chimera Shot");

    // DPS
    Spell Raptor_Strike = new Spell("Raptor Strike");
    Spell Arcane_Shot = new Spell("Arcane Shot");
    Spell Steady_Shot = new Spell("Steady Shot");
    Spell Serpent_Sting = new Spell("Serpent Sting");
    Spell Multi_Shot = new Spell("Multi-Shot");
    Spell Kill_Shot = new Spell("Kill Shot");
    Spell Explosive_Trap = new Spell("Explosive Trap");
    Spell Cobra_Shot = new Spell("Cobra Shot");
    Spell Immolation_Trap = new Spell("Immolation Trap");

    // BUFF & HELPING
    Spell Concussive_Shot = new Spell("Concussive Shot");
    Spell Aspect_of_the_Hawk = new Spell("Aspect of the Hawk");
    Spell Disengage = new Spell("Disengage");
    Spell Hunters_Mark = new Spell("Hunter's Mark");
    Spell Scatter_Shot = new Spell("Scatter Shot");	// 19503
    Spell Feign_Death = new Spell("Feign Death");	//	5384
    Spell Snake_Trap = new Spell("Snake Trap");
    Spell Ice_Trap = new Spell("Ice Trap");
    Spell Freezing_Trap = new Spell("Freezing Trap");
    Spell Trap_Launcher = new Spell("Trap Launcher");	//	77769
    Spell Rapid_Fire = new Spell("Rapid Fire");	//	3045
    Spell Misdirection = new Spell("Misdirection");
    Spell Deterrence = new Spell("Deterrence");	//	19263
    Spell Wing_Clip = new Spell("Wing Clip");

    // PET
    Spell Kill_Command = new Spell("Kill Command");
    Spell Mend_Pet = new Spell("Mend Pet");	//	136
    Spell Revive_Pet = new Spell("Revive Pet");	//	982
    Spell Call_Pet = new Spell("Call Pet 1");	//	883

    // TIMER
    Timer look = new Timer(0);
    Timer fighttimer = new Timer(0);
    Timer petheal = new Timer(0);
    Timer traplaunchertimer = new Timer(0);
    Timer disengagetimer = new Timer(0);
    Timer Serpent_Sting_debuff = new Timer(0);
    Timer mountchill = new Timer(0);

    // Profession & Racials
    Spell Arcane_Torrent = new Spell("Arcane Torrent");
    Spell Lifeblood = new Spell("Lifeblood");
    Spell Stoneform = new Spell("Stoneform");
    Spell Tailoring = new Spell("Tailoring");
    Spell Leatherworking = new Spell("Leatherworking");
    Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    Spell War_Stomp = new Spell("War Stomp");
    Spell Berserking = new Spell("Berserking");

    #endregion InitializeSpell

    public Marks()
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

                    if (fighttimer.IsReady && ObjectManager.Target.HealthPercent > 90 && ObjectManager.Me.Target > 0 && ObjectManager.GetNumberAttackPlayer() < 2)
                    {
                        Logging.WriteFight(" - Target Evading - ");
                        break;
                    }
                }
            }
            if (ObjectManager.Me.IsMounted) mountchill = new Timer(2000);
            Thread.Sleep(350);
        }
    }

    public void pull()
    {

        if (hardmob()) Logging.WriteFight(" -  Pull Hard Mob - ");
        if (!hardmob()) Logging.WriteFight(" -  Pull Easy Mob - ");
        fighttimer = new Timer(60000);

        if (Concussive_Shot.KnownSpell && Concussive_Shot.IsSpellUsable && Concussive_Shot.IsDistanceGood)
        {
            SpellManager.CastSpellByIdLUA(5116);
            // Concussive_Shot.Launch();
        }
    }

    public void buffoutfight()
    {

        if (Fight.InFight || ObjectManager.Me.IsDeadMe) return;

        pet();

        if (!ObjectManager.Me.HaveBuff(79640) &&
            ItemsManager.GetItemCountByIdLUA(58149) == 1)
        {
            Logging.WriteFight("Use Alchi Flask");
            Lua.RunMacroText("/use item:58149");
        }

        if (Aspect_of_the_Hawk.KnownSpell && Aspect_of_the_Hawk.IsSpellUsable &&
            !Aspect_of_the_Hawk.HaveBuff)
        {
            SpellManager.CastSpellByIdLUA(13165);
            // Aspect_of_the_Hawk.Launch();
        }

    }

    public void fight()
    {
        pet();
        selfheal();
        buffinfight();
        if (ObjectManager.GetNumberAttackPlayer() > 1) fighttimer = new Timer(60000);

        if (ObjectManager.GetNumberAttackPlayer() > 2 && Explosive_Trap.IsSpellUsable && Trap_Launcher.IsSpellUsable && Arcane_Shot.IsDistanceGood)
        {
            traplaunchertimer = new Timer(1000);
            Trap_Launcher.Launch();
            while (!traplaunchertimer.IsReady)
            {
                if (Explosive_Trap.KnownSpell && Explosive_Trap.IsSpellUsable)
                {
                    SpellManager.CastSpellByIDAndPosition(13813, ObjectManager.Target.Position);
                }
            }
        }

        if (ObjectManager.GetNumberAttackPlayer() < 2 && Immolation_Trap.IsSpellUsable && Trap_Launcher.IsSpellUsable && Arcane_Shot.IsDistanceGood &&
            !ObjectManager.Target.IsTargetingMe && ObjectManager.Target.HealthPercent > 70)
        {
            traplaunchertimer = new Timer(1000);
            Trap_Launcher.Launch();
            while (!traplaunchertimer.IsReady)
            {
                if (Immolation_Trap.KnownSpell && Immolation_Trap.IsSpellUsable)
                {
                    SpellManager.CastSpellByIDAndPosition(13795, ObjectManager.Target.Position);
                }
            }
        }

        if (ObjectManager.GetNumberAttackPlayer() < 2 && Immolation_Trap.IsSpellUsable && !Trap_Launcher.KnownSpell && Arcane_Shot.IsDistanceGood)
        {

            Immolation_Trap.Launch();
        }



        if (Kill_Shot.KnownSpell && Kill_Shot.IsSpellUsable && Kill_Shot.IsDistanceGood)
        {
            SpellManager.CastSpellByIdLUA(53351);
            // Kill_Shot.Launch();
        }

        if (Hunters_Mark.KnownSpell && Hunters_Mark.IsSpellUsable && Hunters_Mark.IsDistanceGood && !Hunters_Mark.TargetHaveBuff && !Chimera_Shot.KnownSpell)
        {
            SpellManager.CastSpellByIdLUA(1130);
            // Hunters_Mark.Launch();
        }

        if ((ObjectManager.GetNumberAttackPlayer() > 2 || hardmob()) && Misdirection.KnownSpell && Misdirection.IsSpellUsable)
        {
            Lua.RunMacroText("/cast [@pet] Misdirection");
            Lua.RunMacroText("/cast [@pet] Irreführung");
            Lua.RunMacroText("/cast [@pet] Détournement");
            Lua.RunMacroText("/cast [@pet] Перенаправление");
            Lua.RunMacroText("/cast [@pet] Redirección");
            Lua.RunMacroText("/cast [@pet] Redirecionar");
        }

        if (ObjectManager.Me.HaveBuff(82926) && Aimed_Shot.IsSpellUsable && Aimed_Shot.IsDistanceGood)
        {
            SpellManager.CastSpellByIdLUA(19434);
            // Aimed_Shot.Launch();
        }

        if (ObjectManager.Me.HaveBuff(82897))
        {
            Kill_Command.Launch();
        }

        if (Concussive_Shot.KnownSpell && Concussive_Shot.IsSpellUsable && Concussive_Shot.IsDistanceGood && !ObjectManager.Target.HaveBuff(1978))
        {
            SpellManager.CastSpellByIdLUA(5116);
            // Concussive_Shot.Launch();
        }

        if (!ObjectManager.Target.HaveBuff(1978) && Serpent_Sting_debuff.IsReady && Arcane_Shot.IsDistanceGood)
        {
            Serpent_Sting_debuff = new Timer(2000);
            Serpent_Sting.Launch();
        }

        if (!ObjectManager.Target.HaveBuff(1978) && !Serpent_Sting_debuff.IsReady)
        {
            if (Kill_Shot.KnownSpell && Kill_Shot.IsSpellUsable && Kill_Shot.IsDistanceGood)
            {
                Kill_Shot.Launch();
            }
            if (Kill_Shot.KnownSpell && Kill_Shot.IsSpellUsable && Kill_Shot.IsDistanceGood)
            {
                Kill_Command.Launch();
            }
        }

        if ((ObjectManager.GetNumberAttackPlayer() > 1 || hardmob()) && Snake_Trap.IsSpellUsable && Arcane_Shot.IsDistanceGood && !ObjectManager.Target.GetMove && Trap_Launcher.KnownSpell && look.IsReady)
        {
            traplaunchertimer = new Timer(1000);
            Trap_Launcher.Launch();
            while (!traplaunchertimer.IsReady)
            {
                if (Snake_Trap.KnownSpell && Snake_Trap.IsSpellUsable && Trap_Launcher.HaveBuff)
                {
                    SpellManager.CastSpellByIDAndPosition(34600, ObjectManager.Target.Position);
                }
            }
        }

        if (Freezing_Trap.KnownSpell && Freezing_Trap.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() > 1)
        {
            SpellManager.CastSpellByIdLUA(1499);
            // Freezing_Trap.Launch();
        }

        if (Scatter_Shot.KnownSpell && Scatter_Shot.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() > 2)
        {
            Scatter_Shot.Launch();
        }

        if (ObjectManager.Me.BarTwoPercentage > 44 && ObjectManager.Target.HaveBuff(1978))
        {
            if (Multi_Shot.KnownSpell && Multi_Shot.IsSpellUsable && Multi_Shot.IsDistanceGood && ObjectManager.GetNumberAttackPlayer() > 1)
            {
                SpellManager.CastSpellByIdLUA(2643);
                // Multi_Shot.Launch();
            }
            if (Chimera_Shot.KnownSpell && Chimera_Shot.IsSpellUsable && Chimera_Shot.IsDistanceGood)
            {
                SpellManager.CastSpellByIdLUA(53209);
                // Chimera_Shot.Launch();
            }

            if (Aimed_Shot.KnownSpell && Aimed_Shot.IsSpellUsable && Aimed_Shot.IsDistanceGood && !Chimera_Shot.IsSpellUsable)
            {
                SpellManager.CastSpellByIdLUA(19434);
                // Aimed_Shot.Launch();
            }

            if (Arcane_Shot.KnownSpell && Arcane_Shot.IsSpellUsable && Arcane_Shot.IsDistanceGood && !Chimera_Shot.IsSpellUsable && !Aimed_Shot.IsSpellUsable)
            {
                SpellManager.CastSpellByIdLUA(3044);
                // Arcane_Shot.Launch();
            }
        }

        if (ObjectManager.Me.BarTwoPercentage < 50 && ObjectManager.Target.HaveBuff(1978))
        {
            if (Steady_Shot.KnownSpell && Steady_Shot.IsSpellUsable && Steady_Shot.IsDistanceGood)
            {
                SpellManager.CastSpellByIdLUA(56641);
                // Steady_Shot.Launch();
            }

        }

        if (Arcane_Torrent.KnownSpell && Arcane_Torrent.IsSpellUsable &&
            ObjectManager.Target.IsCast && ObjectManager.Target.GetDistance < 8)
        {
            Arcane_Torrent.Launch();
        }

        if (Silencing_Shot.KnownSpell && Silencing_Shot.IsSpellUsable && Silencing_Shot.IsDistanceGood &&
            ObjectManager.Target.IsCast)
        {
            Silencing_Shot.Launch();
        }

    }

    private void pet()
    {

        if (ObjectManager.Me.IsMounted || !mountchill.IsReady) return;

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

        if (Mend_Pet.KnownSpell && Mend_Pet.IsSpellUsable && petheal.IsReady &&
            ObjectManager.Pet.Health > 0 && ObjectManager.Pet.HealthPercent < 70)
        {
            petheal = new Timer(9000);
            Mend_Pet.Launch();
        }

        if (Fight.InFight) Lua.RunMacroText("/petattack");

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

        if ((ObjectManager.GetNumberAttackPlayer() > 1 || hardmob()) &&
            ObjectManager.Me.HealthPercent < 65 &&
            ObjectManager.Target.GetDistance < 5 &&
            War_Stomp.KnownSpell && War_Stomp.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(20549);
            // War_Stomp.Launch();
        }

        if (Berserking.KnownSpell && Berserking.IsSpellUsable && Arcane_Shot.IsDistanceGood)
        {
            SpellManager.CastSpellByIdLUA(1454);
            // Berserking.Launch();
        }

        if (Rapid_Fire.KnownSpell && Rapid_Fire.IsSpellUsable && (hardmob() || ObjectManager.GetNumberAttackPlayer() > 2) && Arcane_Shot.IsDistanceGood)
        {
            Rapid_Fire.Launch();
        }

        if (Readiness.KnownSpell && Readiness.IsSpellUsable && (hardmob() || ObjectManager.GetNumberAttackPlayer() > 2) && Arcane_Shot.IsDistanceGood &&
            !Rapid_Fire.IsSpellUsable && !Chimera_Shot.IsSpellUsable)
        {
            Readiness.Launch();
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

        if (Disengage.KnownSpell && Disengage.IsSpellUsable && Disengage.IsDistanceGood &&
            ObjectManager.Target.HealthPercent > 30 && ObjectManager.Target.GetDistance < 5)
        {
            disengagetimer = new Timer(2000);
            while (ObjectManager.Target.GetDistance < 5 && !disengagetimer.IsReady)
                if (Wing_Clip.KnownSpell && Wing_Clip.IsSpellUsable && Wing_Clip.IsDistanceGood && !Wing_Clip.TargetHaveBuff)
                {
                    SpellManager.CastSpellByIdLUA(2974);
                    // Wing_Clip.Launch();
                }
            SpellManager.CastSpellByIdLUA(781);
            // Disengage.Launch();
            if (Concussive_Shot.KnownSpell && Concussive_Shot.IsSpellUsable && Concussive_Shot.IsDistanceGood)
            {
                SpellManager.CastSpellByIdLUA(5116);
                // Concussive_Shot.Launch();
            }
            return;
        }

        if (ObjectManager.Target.GetDistance < 10 && ((Disengage.KnownSpell && !Disengage.IsSpellUsable) || !Disengage.KnownSpell))
        {
            disengagetimer = new Timer(5000);
            while (ObjectManager.Target.GetDistance < 10 || !disengagetimer.IsReady)
            {
                if (!Fight.InFight) return;
                Keyboard.DownKey(nManager.Wow.Memory.WowProcess.MainWindowHandle, "S");
                Thread.Sleep(100);

                if (Mend_Pet.KnownSpell && Mend_Pet.IsSpellUsable && petheal.IsReady &&
                    ObjectManager.Pet.Health > 0 && ObjectManager.Pet.HealthPercent < 70)
                {
                    petheal = new Timer(9000);
                    Mend_Pet.Launch();
                }

                if (Wing_Clip.KnownSpell && Wing_Clip.IsSpellUsable && Wing_Clip.IsDistanceGood && !Wing_Clip.TargetHaveBuff)
                {
                    SpellManager.CastSpellByIdLUA(2974);
                    // Wing_Clip.Launch();
                }

                if (ObjectManager.Me.HaveBuff(82926) && Aimed_Shot.IsSpellUsable && Aimed_Shot.IsDistanceGood)
                {
                    SpellManager.CastSpellByIdLUA(19434);
                    // Aimed_Shot.Launch();
                }

                if (Raptor_Strike.KnownSpell && Raptor_Strike.IsSpellUsable && Raptor_Strike.IsDistanceGood)
                {
                    SpellManager.CastSpellByIdLUA(2973);
                    // Raptor_Strike.Launch();
                }

                if (Kill_Command.KnownSpell && Kill_Command.IsSpellUsable && Kill_Command.IsDistanceGood)
                {
                    SpellManager.CastSpellByIdLUA(34026);
                    // Kill_Command.Launch();
                }

                if (Kill_Shot.KnownSpell && Kill_Shot.IsSpellUsable && Kill_Shot.IsDistanceGood)
                {
                    SpellManager.CastSpellByIdLUA(53351);
                    // Kill_Shot.Launch();
                }

                if (Chimera_Shot.KnownSpell && Chimera_Shot.IsSpellUsable && Chimera_Shot.IsDistanceGood && ObjectManager.Me.BarTwoPercentage > 70)
                {
                    SpellManager.CastSpellByIdLUA(53209);
                    // Chimera_Shot.Launch();
                }

                if (Arcane_Shot.KnownSpell && Arcane_Shot.IsSpellUsable && Arcane_Shot.IsDistanceGood && !Chimera_Shot.IsSpellUsable && ObjectManager.Me.BarTwoPercentage > 70)
                {
                    SpellManager.CastSpellByIdLUA(3044);
                    // Arcane_Shot.Launch();
                }

                if (Feign_Death.KnownSpell && Feign_Death.IsSpellUsable)
                {
                    Feign_Death.Launch();
                    Thread.Sleep(3000);
                }

                if (Freezing_Trap.KnownSpell && Freezing_Trap.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() > 1)
                {
                    SpellManager.CastSpellByIdLUA(1499);
                    // Freezing_Trap.Launch();
                }

                if (Scatter_Shot.KnownSpell && Scatter_Shot.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() > 1)
                {
                    Scatter_Shot.Launch();
                }

                if (ObjectManager.Me.HealthPercent < 30 && ObjectManager.Target.IsTargetingMe)
                {
                    if (!Feign_Death.IsSpellUsable && !Scatter_Shot.IsSpellUsable && Deterrence.KnownSpell && Deterrence.KnownSpell)
                    {
                        Deterrence.Launch();
                    }
                }

                Keyboard.DownKey(nManager.Wow.Memory.WowProcess.MainWindowHandle, "{SPACE}");
            }
        }

        if (Feign_Death.KnownSpell && Feign_Death.IsSpellUsable && ObjectManager.Me.HealthPercent < 30 && ObjectManager.Pet.Health > 10)
        {
            Feign_Death.Launch();
            Thread.Sleep(2500);
        }

        if (Feign_Death.KnownSpell && Feign_Death.IsSpellUsable && ObjectManager.Me.HealthPercent < 15 && (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0))
        {
            Feign_Death.Launch();
            Lua.RunMacroText("/cleartarget");
            Thread.Sleep(30000);
        }

    }

    public bool hardmob()
    {
        if (((ObjectManager.Target.MaxHealth * 100) / ObjectManager.Me.MaxHealth) > 110)
        {
            return true;
        }
        return false;
    }
}
public class BeastMaster
{
    #region InitializeSpell

    // Beast Mastery only
    Spell Beastial_Wrath = new Spell("Beastial Wrath");
    Spell Focus_Fire = new Spell("Focus Fire");
    Spell Intimidation = new Spell("Intimidation");
    // Beast master with a Spirit Beast only
    Spell Spirit_Mend = new Spell("Spirit Mend");

    // DPS
    Spell Raptor_Strike = new Spell("Raptor Strike");
    Spell Arcane_Shot = new Spell("Arcane Shot");
    Spell Steady_Shot = new Spell("Steady Shot");
    Spell Serpent_Sting = new Spell("Serpent Sting");
    Spell Multi_Shot = new Spell("Multi-Shot");
    Spell Kill_Shot = new Spell("Kill Shot");
    Spell Explosive_Trap = new Spell("Explosive Trap");
    Spell Cobra_Shot = new Spell("Cobra Shot");
    Spell Immolation_Trap = new Spell("Immolation Trap");

    // BUFF & HELPING
    Spell Concussive_Shot = new Spell("Concussive Shot");
    Spell Aspect_of_the_Hawk = new Spell("Aspect of the Hawk");
    Spell Disengage = new Spell("Disengage");
    Spell Hunters_Mark = new Spell("Hunter's Mark");
    Spell Scatter_Shot = new Spell("Scatter Shot");	// 19503
    Spell Feign_Death = new Spell("Feign Death");	//	5384
    Spell Snake_Trap = new Spell("Snake Trap");
    Spell Ice_Trap = new Spell("Ice Trap");
    Spell Freezing_Trap = new Spell("Freezing Trap");
    Spell Trap_Launcher = new Spell("Trap Launcher");	//	77769
    Spell Rapid_Fire = new Spell("Rapid Fire");	//	3045
    Spell Misdirection = new Spell("Misdirection");
    Spell Deterrence = new Spell("Deterrence");	//	19263
    Spell Wing_Clip = new Spell("Wing Clip");

    // PET
    Spell Kill_Command = new Spell("Kill Command");
    Spell Mend_Pet = new Spell("Mend Pet");	//	136
    Spell Revive_Pet = new Spell("Revive Pet");	//	982
    Spell Call_Pet = new Spell("Call Pet 1");	//	883

    // TIMER
    Timer look = new Timer(0);
    Timer fighttimer = new Timer(0);
    Timer petheal = new Timer(0);
    Timer traplaunchertimer = new Timer(0);
    Timer disengagetimer = new Timer(0);
    Timer Serpent_Sting_debuff = new Timer(0);
    Timer mountchill = new Timer(0);

    // Profession & Racials
    Spell Arcane_Torrent = new Spell("Arcane Torrent");
    Spell Lifeblood = new Spell("Lifeblood");
    Spell Stoneform = new Spell("Stoneform");
    Spell Tailoring = new Spell("Tailoring");
    Spell Leatherworking = new Spell("Leatherworking");
    Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    Spell War_Stomp = new Spell("War Stomp");
    Spell Berserking = new Spell("Berserking");

    #endregion InitializeSpell

    public BeastMaster()
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

                    if (fighttimer.IsReady && ObjectManager.Target.HealthPercent > 90 && ObjectManager.Me.Target > 0 && ObjectManager.GetNumberAttackPlayer() < 2)
                    {
                        Logging.WriteFight(" - Target Evading - ");
                        break;
                    }
                }
            }
            if (ObjectManager.Me.IsMounted) mountchill = new Timer(2000);
            Thread.Sleep(350);
        }
    }

    public void pull()
    {

        if (hardmob())
            Logging.WriteFight(" -  Pull Hard Mob - ");
        else
            Logging.WriteFight(" -  Pull Easy Mob - ");
        fighttimer = new Timer(60000);

        if (Concussive_Shot.KnownSpell && Concussive_Shot.IsSpellUsable && Concussive_Shot.IsDistanceGood)
        {
            SpellManager.CastSpellByIdLUA(5116);
            // Concussive_Shot.Launch();
        }
    }

    public void buffoutfight()
    {

        if (Fight.InFight || ObjectManager.Me.IsDeadMe) return;

        pet();

        if (!ObjectManager.Me.HaveBuff(79640) &&
            ItemsManager.GetItemCountByIdLUA(58149) == 1)
        {
            Logging.WriteFight("Use Alchi Flask");
            Lua.RunMacroText("/use item:58149");
        }

        if (Aspect_of_the_Hawk.KnownSpell && Aspect_of_the_Hawk.IsSpellUsable &&
            !Aspect_of_the_Hawk.HaveBuff)
        {
            SpellManager.CastSpellByIdLUA(13165);
            // Aspect_of_the_Hawk.Launch();
        }

    }

    public void fight()
    {
        pet();
        selfheal();
        buffinfight();
        if (ObjectManager.GetNumberAttackPlayer() > 1) fighttimer = new Timer(60000);

        if (ObjectManager.GetNumberAttackPlayer() > 2 && Explosive_Trap.IsSpellUsable && Trap_Launcher.IsSpellUsable && Arcane_Shot.IsDistanceGood)
        {
            traplaunchertimer = new Timer(1100);
            Trap_Launcher.Launch();
            while (!traplaunchertimer.IsReady)
            {
                if (Explosive_Trap.KnownSpell && Explosive_Trap.IsSpellUsable)
                {
                    SpellManager.CastSpellByIDAndPosition(13813, ObjectManager.Target.Position);
                }
            }
        }

        if (ObjectManager.GetNumberAttackPlayer() < 2 && Immolation_Trap.IsSpellUsable && Trap_Launcher.IsSpellUsable && Arcane_Shot.IsDistanceGood &&
            !ObjectManager.Target.IsTargetingMe && ObjectManager.Target.HealthPercent > 70)
        {
            traplaunchertimer = new Timer(1100);
            Trap_Launcher.Launch();
            while (!traplaunchertimer.IsReady)
            {
                if (Immolation_Trap.KnownSpell && Immolation_Trap.IsSpellUsable)
                {
                    SpellManager.CastSpellByIDAndPosition(13795, ObjectManager.Target.Position);
                }
            }
        }

        if (ObjectManager.GetNumberAttackPlayer() < 2 && Immolation_Trap.IsSpellUsable && !Trap_Launcher.KnownSpell && Arcane_Shot.IsDistanceGood)
        {
            Immolation_Trap.Launch();
        }

        if (Kill_Shot.KnownSpell && Kill_Shot.IsSpellUsable && Kill_Shot.IsDistanceGood)
        {
            SpellManager.CastSpellByIdLUA(53351);
            // Kill_Shot.Launch();
        }

        if (Hunters_Mark.KnownSpell && Hunters_Mark.IsSpellUsable && Hunters_Mark.IsDistanceGood && !Hunters_Mark.TargetHaveBuff)
        {
            SpellManager.CastSpellByIdLUA(1130);
            // Hunters_Mark.Launch();
        }

        if ((ObjectManager.GetNumberAttackPlayer() > 2 || hardmob()) && Misdirection.KnownSpell && Misdirection.IsSpellUsable)
        {
            Lua.RunMacroText("/cast [@pet] Misdirection");
            Lua.RunMacroText("/cast [@pet] Irreführung");
            Lua.RunMacroText("/cast [@pet] Détournement");
            Lua.RunMacroText("/cast [@pet] Перенаправление");
            Lua.RunMacroText("/cast [@pet] Redirección");
            Lua.RunMacroText("/cast [@pet] Redirecionar");
        }

        if (Focus_Fire.KnownSpell && Focus_Fire.IsSpellUsable && ObjectManager.Pet.BuffStack(19615) == 5) // Frenzy Effect
        {
            SpellManager.CastSpellByIdLUA(82692);
            // Focus_Fire.Launch();
        }

        if (Concussive_Shot.KnownSpell && Concussive_Shot.IsSpellUsable && Concussive_Shot.IsDistanceGood && !ObjectManager.Target.HaveBuff(1978))
        {
            SpellManager.CastSpellByIdLUA(5116);
            // Concussive_Shot.Launch();
        }

        if (!ObjectManager.Target.HaveBuff(1978) && Serpent_Sting_debuff.IsReady && Arcane_Shot.IsDistanceGood)
        {
            Serpent_Sting_debuff = new Timer(2500);
            Serpent_Sting.Launch();
        }

        if (!ObjectManager.Target.HaveBuff(1978) && !Serpent_Sting_debuff.IsReady)
        {
            if (Kill_Shot.KnownSpell && Kill_Shot.IsSpellUsable && Kill_Shot.IsDistanceGood)
            {
                Kill_Shot.Launch();
            }
        }

        if ((ObjectManager.GetNumberAttackPlayer() > 1 || hardmob()) && Snake_Trap.IsSpellUsable && Arcane_Shot.IsDistanceGood && !ObjectManager.Target.GetMove && Trap_Launcher.KnownSpell)
        {
            traplaunchertimer = new Timer(1100);
            Trap_Launcher.Launch();
            while (!traplaunchertimer.IsReady)
            {
                if (Snake_Trap.KnownSpell && Snake_Trap.IsSpellUsable && Trap_Launcher.HaveBuff)
                {
                    SpellManager.CastSpellByIDAndPosition(34600, ObjectManager.Target.Position);
                }
            }
        }

        if (Freezing_Trap.KnownSpell && Freezing_Trap.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() > 1)
        {
            SpellManager.CastSpellByIdLUA(1499);
            // Freezing_Trap.Launch();
        }

        if (ObjectManager.Target.HaveBuff(1978))
        {
            if (Multi_Shot.KnownSpell && Multi_Shot.IsSpellUsable && Multi_Shot.IsDistanceGood && ObjectManager.GetNumberAttackPlayer() > 1)
            {
                SpellManager.CastSpellByIdLUA(2643);
                // Multi_Shot.Launch();
            }

            if (Kill_Command.KnownSpell && Kill_Command.IsSpellUsable)
            {
                SpellManager.CastSpellByIdLUA(34026);
                // Kill_Command.Launch();
            }

            if (Arcane_Shot.KnownSpell && Arcane_Shot.IsSpellUsable && Arcane_Shot.IsDistanceGood)
            {
                SpellManager.CastSpellByIdLUA(3044);
                // Arcane_Shot.Launch();
            }
        }

        if (ObjectManager.Me.BarTwoPercentage < 70 && ObjectManager.Target.HaveBuff(1978))
        {
            if (Steady_Shot.KnownSpell && Steady_Shot.IsSpellUsable && Steady_Shot.IsDistanceGood && !Cobra_Shot.KnownSpell)
            {
                SpellManager.CastSpellByIdLUA(56641);
                // Steady_Shot.Launch();
            }
            else if (Cobra_Shot.KnownSpell && Cobra_Shot.IsSpellUsable && Cobra_Shot.IsDistanceGood && ObjectManager.Target.HaveBuff(1978))
            {
                SpellManager.CastSpellByIdLUA(77767);
                // Cobra_Shot.Launch();
            }
        }

        if (Arcane_Torrent.KnownSpell && Arcane_Torrent.IsSpellUsable &&
            ObjectManager.Target.IsCast && ObjectManager.Target.GetDistance < 8)
        {
            Arcane_Torrent.Launch();
        }

    }

    private void pet()
    {

        if (ObjectManager.Me.IsMounted || !mountchill.IsReady) return;

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

        if (Mend_Pet.KnownSpell && Mend_Pet.IsSpellUsable && petheal.IsReady &&
            ObjectManager.Pet.Health > 0 && ObjectManager.Pet.HealthPercent < 70)
        {
            petheal = new Timer(9000);
            Mend_Pet.Launch();
        }

        if (Fight.InFight) Lua.RunMacroText("/petattack");

    }

    private void buffinfight()
    {

        if (!Fight.InFight) return;

        if ((ObjectManager.GetNumberAttackPlayer() > 1 || hardmob()) &&
            ObjectManager.Me.HealthPercent < 65 &&
            ObjectManager.Target.GetDistance < 5 &&
            Intimidation.KnownSpell && Intimidation.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(19577);
            // Intimidation.Launch();
        }

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

        if (Berserking.KnownSpell && Berserking.IsSpellUsable && Arcane_Shot.IsDistanceGood)
        {
            SpellManager.CastSpellByIdLUA(1454);
            // Berserking.Launch();
        }

        if (Rapid_Fire.KnownSpell && Rapid_Fire.IsSpellUsable && (hardmob() || ObjectManager.GetNumberAttackPlayer() > 2) && Arcane_Shot.IsDistanceGood)
        {
            Rapid_Fire.Launch();
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

        if (ObjectManager.Me.HealthPercent < 80 &&
            Spirit_Mend.KnownSpell && Spirit_Mend.IsSpellUsable)
        {
            Lua.RunMacroText("/target " + ObjectManager.Me.Name);
            SpellManager.CastSpellByIdLUA(90361);
            Lua.RunMacroText("/targetlasttarget");
        }

        if (Disengage.KnownSpell && Disengage.IsSpellUsable && Disengage.IsDistanceGood &&
            ObjectManager.Target.HealthPercent > 30 && ObjectManager.Target.GetDistance < 5)
        {
            disengagetimer = new Timer(2000);
            while (ObjectManager.Target.GetDistance < 5 && !disengagetimer.IsReady)
                if (Wing_Clip.KnownSpell && Wing_Clip.IsSpellUsable && Wing_Clip.IsDistanceGood && !Wing_Clip.TargetHaveBuff)
                {
                    SpellManager.CastSpellByIdLUA(2974);
                    // Wing_Clip.Launch();
                }
            SpellManager.CastSpellByIdLUA(781);
            // Disengage.Launch();
            if (Concussive_Shot.KnownSpell && Concussive_Shot.IsSpellUsable && Concussive_Shot.IsDistanceGood)
            {
                SpellManager.CastSpellByIdLUA(5116);
                // Concussive_Shot.Launch();
            }
            return;
        }

        if (ObjectManager.Target.GetDistance < 10 && ((Disengage.KnownSpell && !Disengage.IsSpellUsable) || !Disengage.KnownSpell))
        {
            disengagetimer = new Timer(5000);
            while (ObjectManager.Target.GetDistance < 10 || !disengagetimer.IsReady)
            {
                if (!Fight.InFight) return;
                Keyboard.DownKey(nManager.Wow.Memory.WowProcess.MainWindowHandle, "S");
                Thread.Sleep(100);

                if (Mend_Pet.KnownSpell && Mend_Pet.IsSpellUsable && petheal.IsReady &&
                    ObjectManager.Pet.Health > 0 && ObjectManager.Pet.HealthPercent < 70)
                {
                    petheal = new Timer(9000);
                    Mend_Pet.Launch();
                }

                if (Kill_Command.KnownSpell && Kill_Command.IsSpellUsable && Kill_Command.IsDistanceGood)
                {
                    SpellManager.CastSpellByIdLUA(34026);
                    // Kill_Command.Launch();
                }

                if (Wing_Clip.KnownSpell && Wing_Clip.IsSpellUsable && Wing_Clip.IsDistanceGood && !Wing_Clip.TargetHaveBuff)
                {
                    SpellManager.CastSpellByIdLUA(2974);
                    // Wing_Clip.Launch();
                }

                if (Raptor_Strike.KnownSpell && Raptor_Strike.IsSpellUsable && Raptor_Strike.IsDistanceGood)
                {
                    SpellManager.CastSpellByIdLUA(2973);
                    // Raptor_Strike.Launch();
                }

                if (Kill_Shot.KnownSpell && Kill_Shot.IsSpellUsable && Kill_Shot.IsDistanceGood)
                {
                    SpellManager.CastSpellByIdLUA(53351);
                    // Kill_Shot.Launch();
                }

                if (Arcane_Shot.KnownSpell && Arcane_Shot.IsSpellUsable && Arcane_Shot.IsDistanceGood)
                {
                    SpellManager.CastSpellByIdLUA(3044);
                    // Arcane_Shot.Launch();
                }

                if (Feign_Death.KnownSpell && Feign_Death.IsSpellUsable)
                {
                    Feign_Death.Launch();
                    Thread.Sleep(3000);
                }

                if (Freezing_Trap.KnownSpell && Freezing_Trap.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() > 1)
                {
                    SpellManager.CastSpellByIdLUA(1499);
                    // Freezing_Trap.Launch();
                }

                if (Scatter_Shot.KnownSpell && Scatter_Shot.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() > 1)
                {
                    Scatter_Shot.Launch();
                }

                if (ObjectManager.Me.HealthPercent < 30 && ObjectManager.Target.IsTargetingMe)
                {
                    if (!Feign_Death.IsSpellUsable && !Scatter_Shot.IsSpellUsable && Deterrence.KnownSpell && Deterrence.KnownSpell)
                    {
                        Deterrence.Launch();
                    }
                }

                Keyboard.DownKey(nManager.Wow.Memory.WowProcess.MainWindowHandle, "{SPACE}");
            }
        }

        if (Feign_Death.KnownSpell && Feign_Death.IsSpellUsable && ObjectManager.Me.HealthPercent < 15 && ObjectManager.Pet.Health > 10)
        {
            Feign_Death.Launch();
            Thread.Sleep(3000);
        }

        if (Feign_Death.KnownSpell && Feign_Death.IsSpellUsable && ObjectManager.Me.HealthPercent < 15 && (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0))
        {
            Feign_Death.Launch();
            Lua.RunMacroText("/cleartarget");
            Thread.Sleep(30000);
        }

    }

    public bool hardmob()
    {
        if (((ObjectManager.Target.MaxHealth * 100) / ObjectManager.Me.MaxHealth) > 130)
        {
            return true;
        }
        return false;
    }
}
#endregion