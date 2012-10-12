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
                    #region DeathKnight Specialisation checking

                case WoWClass.DeathKnight:
                    var Heart_Strike = new Spell("Heart Strike");
                    var Vampiric_Blood = new Spell("Vampiric Blood");
                    var Scourge_Strike = new Spell("Scourge Strike");
                    var Dark_Transformation = new Spell("Dark Transformation");
                    var Frost_Strike = new Spell("Frost Strike");
                    var Howling_Blast = new Spell("Howling Blast");
                    if (Heart_Strike.KnownSpell || Vampiric_Blood.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            MessageBox.Show("There is no settings available for your Class/Specialisation.");
                        }
                        else
                        {
                            Logging.WriteFight("Loading Deathknight Blood class...");
                            new Deathknight_Blood();
                        }
                    }
                    else if (Scourge_Strike.KnownSpell || Dark_Transformation.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            MessageBox.Show("There is no settings available for your Class/Specialisation.");
                        }
                        else
                        {
                            Logging.WriteFight("Loading Deathknight Unholy class...");
                            new Deathknight_Unholy();
                        }
                    }
                    else if (Frost_Strike.KnownSpell || Howling_Blast.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            MessageBox.Show("There is no settings available for your Class/Specialisation.");
                        }
                        else
                        {
                            Logging.WriteFight("Loading Deathknight Frost class...");
                            new Deathknight_Frost();
                        }
                    }
                    else
                    {
                        if (ConfigOnly)
                        {
                            MessageBox.Show("There is no settings available for your Class/Specialisation.");
                        }
                        else
                        {
                            Logging.WriteFight("No specialisation detected.");
                            Logging.WriteFight("Loading Deathknight Apprentice class...");
                            new Deathknight_Apprentice();
                        }
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
    private readonly Spell ArcaneTorrent = new Spell("Arcane Torrent");
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
        if (ObjectManager.Target.HaveBuff(55078) && ObjectManager.Target.HaveBuff(55095) && Pestilence.KnownSpell
            && Pestilence.IsSpellUsable && Pestilence.IsDistanceGood && ObjectManager.GetNumberAttackPlayer() > 1 &&
            Timer_Pestilence.IsReady)
        {
            Pestilence.Launch();
            Timer_Pestilence = new Timer(1000 * 30);
            return;
        }
        if (ObjectManager.Target.HaveBuff(55078) && ObjectManager.Target.HaveBuff(55095) && Blood_Boil.IsSpellUsable &&
            Blood_Boil.IsDistanceGood && ObjectManager.GetNumberAttackPlayer() > 1 && !Timer_Pestilence.IsReady)
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
        if (ArcaneTorrent.KnownSpell && ArcaneTorrent.IsSpellUsable &&
            ObjectManager.Target.IsCast && ObjectManager.Target.GetDistance < 8)
        {
            ArcaneTorrent.Launch();
            return;
        }
        if (ObjectManager.Target.IsCast && Mind_Freeze.KnownSpell && Mind_Freeze.IsSpellUsable &&
            Mind_Freeze.IsDistanceGood)
        {
            Mind_Freeze.Launch();
            return;
        }
        if (ObjectManager.Target.IsCast && Strangulate.KnownSpell && Strangulate.IsSpellUsable &&
            Strangulate.IsDistanceGood)
        {
            Strangulate.Launch();
            return;
        }
    }
}

public class Deathknight_Blood
{
    private Spell AntiMagic_Shell = new Spell("Anti-Magic Shell");
    private Spell AntiMagic_Zone = new Spell("Anti-Magic Zone");
    private Spell Army_of_the_Dead = new Spell("Army of the Dead");
    private Spell Asphyxiate = new Spell("Asphyxiate");
    private Spell Blood_Boil = new Spell("Blood Boil");
    private Spell Blood_Plague = new Spell("Blood Plague");
    private Spell Blood_Presence = new Spell("Blood Presence");
    private Spell Blood_Strike = new Spell("Blood Strike");
    private Spell Blood_Tap = new Spell("Blood Tap");
    private Spell Bone_Shield = new Spell("Bone Shield");
    private Spell Chains_of_Ice = new Spell("Chains of Ice");
    private Spell Dancing_Rune_Weapon = new Spell("Dancing Rune Weapon");
    private Spell Dark_Command = new Spell("Dark Command");
    private Spell Dark_Simulacrum = new Spell("Dark Simulacrum");
    private Spell Deaths_Advance = new Spell("Death's Advance");
    private Spell Death_and_Decay = new Spell("Death and Decay");
    private Spell Death_Coil = new Spell("Death Coil");
    private Spell Death_Grip = new Spell("Death Grip");
    private Spell Death_Pact = new Spell("Death Pact");
    private Spell Death_Siphon = new Spell("Death Siphon");
    private Spell Death_Strike = new Spell("Death Strike");
    private Spell Empower_Rune_Weapon = new Spell("Empower Rune Weapon");
    private Spell Frost_Fever = new Spell("Frost Fever");
    private Spell Frost_Presence = new Spell("Frost Presence");
    private Spell Heart_Strike = new Spell("Heart Strike");
    private Spell Horn_of_Winter = new Spell("Horn of Winter");
    private Spell Icebound_Fortitude = new Spell("Icebound Fortitude");
    private Spell Icy_Touch = new Spell("Icy Touch");
    private Spell Lichborne = new Spell("Lichborne");
    private Spell Mind_Freeze = new Spell("Mind Freeze");
    private Spell Necrotic_Strike = new Spell("Necrotic Strike");
    private Spell Outbreak = new Spell("Outbreak");
    private Spell Path_of_Frost = new Spell("Path of Frost");
    private Spell Pestilence = new Spell("Pestilence");
    private Spell Plague_Leech = new Spell("Plague Leech");
    private Spell Plague_Strike = new Spell("Plague Strike");
    private Spell Raise_Ally = new Spell("Raise Ally");
    private Spell Raise_Dead = new Spell("Raise Dead");
    private Spell Remorseless_Winter = new Spell("Remorseless Winter");
    private Spell Rune_Strike = new Spell("Rune Strike");
    private Spell Rune_Tap = new Spell("Rune Tap");
    private Spell Soul_Reaper = new Spell("Soul Reaper");
    private Spell Strangulate = new Spell("Strangulate");
    private Spell Unholy_Blight = new Spell("Unholy Blight");
    private Spell Unholy_Presence = new Spell("Unholy Presence");
    private Spell Vampiric_Blood = new Spell("Vampiric Blood");

    private Timer Frost_Fever_Timer = new Timer(0);
    private Timer Path_of_Frost_Timer = new Timer(0);
    private Timer Pestilence_Timer = new Timer(0);
    private Timer Plague_Strike_Timer = new Timer(0);
    private Timer Dancing_Rune_Weapon_Timer = new Timer(0);

    // Profession & Racials

    private Spell Arcane_Torrent = new Spell("Arcane Torrent");
    private Spell Berserking = new Spell("Berserking");
    private Spell Blood_Fury = new Spell("Blood Fury");
    private Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    private Spell Lifeblood = new Spell("Lifeblood");
    private Spell Stoneform = new Spell("Stoneform");

    public int DRW = 1;

    public Deathknight_Blood()
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
        Grip();
    }

    private void Buff_path()
    {
        if (!Fight.InFight && !Path_of_Frost.HaveBuff && Path_of_Frost.KnownSpell && Path_of_Frost.IsSpellUsable)
        {
            Path_of_Frost.Launch();
            Path_of_Frost_Timer = new Timer(1000 * 60 * 9);
        }
        if (Path_of_Frost_Timer.IsReady)
        {
            Path_of_Frost.Launch();
            Path_of_Frost_Timer = new Timer(1000 * 60 * 9);
        }
    }

    public void LowCombat()
    {
        Unholy_Presence_Buff();
        AvoidMelee();
        Grip();
        Heal();
        Resistance();
        Buff();

        if (Icy_Touch.KnownSpell && Icy_Touch.IsSpellUsable && Icy_Touch.IsDistanceGood)
        {
            Icy_Touch.Launch();
            if (ObjectManager.Target.HealthPercent < 50 && ObjectManager.Target.HealthPercent > 0)
            {
                Icy_Touch.Launch();
                return;
            }
            return;
        }

        if (Blood_Boil.IsSpellUsable && Blood_Boil.IsSpellUsable && Blood_Boil.IsDistanceGood)
        {
            Blood_Boil.Launch();
            return;
        }

        if (Death_Coil.KnownSpell && Death_Coil.IsSpellUsable && Death_Coil.IsDistanceGood)
        {
            Death_Coil.Launch();
            return;
        }
    }

    public void Combat()
    {
        Blood_Presence_Buff();
        AvoidMelee();
        Grip();
        Decast();
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

        if (Unholy_Blight.KnownSpell && Unholy_Blight.IsSpellUsable && Unholy_Blight.IsDistanceGood)
        {
            Unholy_Blight.Launch();
            Plague_Strike_Timer = new Timer(1000 * 27);
            Frost_Fever_Timer = new Timer(1000 * 27);
            return;
        }

        if (Outbreak.KnownSpell && Outbreak.IsSpellUsable && Outbreak.IsDistanceGood && (Plague_Strike_Timer.IsReady
            || Frost_Fever_Timer.IsReady))
        {
            Outbreak.Launch();
            Plague_Strike_Timer = new Timer(1000 * 27);
            Frost_Fever_Timer = new Timer(1000 * 27);
            return;
        }

        if (!Outbreak.KnownSpell)
        {
            if (!Blood_Plague.TargetHaveBuff &&
                Plague_Strike.KnownSpell && Plague_Strike.IsSpellUsable && Plague_Strike.IsDistanceGood)
            {
                Plague_Strike.Launch();
                Plague_Strike_Timer = new Timer(1000 * 27);
                return;
            }

            if (!Frost_Fever.TargetHaveBuff &&
                Icy_Touch.KnownSpell && Icy_Touch.IsSpellUsable && Icy_Touch.IsDistanceGood)
            {
                Icy_Touch.Launch();
                Frost_Fever_Timer = new Timer(1000 * 27);
                return;
            }
        }

        if (Plague_Strike_Timer.IsReady && Frost_Fever_Timer.IsReady && Blood_Plague.TargetHaveBuff && Frost_Fever.TargetHaveBuff
            && Blood_Boil.IsSpellUsable && Blood_Boil.KnownSpell && Blood_Boil.IsDistanceGood)
        {
            Blood_Boil.Launch();
            Plague_Strike_Timer = new Timer(1000 * 27);
            Frost_Fever_Timer = new Timer(1000 * 27);
            return;
        }

        if (Plague_Strike_Timer.IsReady && Plague_Strike.KnownSpell && Plague_Strike.IsDistanceGood &&
            !Outbreak.IsSpellUsable && !Unholy_Blight.IsSpellUsable)
        {
            if (!Plague_Strike.IsSpellUsable && Blood_Tap.KnownSpell && Blood_Tap.IsSpellUsable)
            {
                Blood_Tap.Launch();
                Thread.Sleep(500);
            }

            if (Plague_Strike.IsSpellUsable)
            {
                Plague_Strike.Launch();
                Plague_Strike_Timer = new Timer(1000 * 27);
                return;
            }
        }

        if (Frost_Fever_Timer.IsReady && Icy_Touch.KnownSpell && Icy_Touch.IsDistanceGood &&
            !Outbreak.IsSpellUsable && !Unholy_Blight.IsSpellUsable)
        {
            if (!Icy_Touch.IsSpellUsable && Blood_Tap.KnownSpell && Blood_Tap.IsSpellUsable)
            {
                Blood_Tap.Launch();
                Thread.Sleep(500);
            }

            if (Icy_Touch.IsSpellUsable)
            {
                Icy_Touch.Launch();
                Frost_Fever_Timer = new Timer(1000 * 27);
                return;
            }
        }

        if (ObjectManager.GetNumberAttackPlayer() > 3 && Blood_Boil.IsSpellUsable && Blood_Boil.IsDistanceGood && Blood_Boil.KnownSpell)
        {
            Blood_Boil.Launch();
            if (Blood_Plague.TargetHaveBuff && Frost_Fever.TargetHaveBuff)
            {
                Plague_Strike_Timer = new Timer(1000 * 27);
                Frost_Fever_Timer = new Timer(1000 * 27);
            }
            return;
        }

        if (ObjectManager.GetNumberAttackPlayer() > 2 &&
            Death_and_Decay.KnownSpell && Death_and_Decay.IsSpellUsable && Death_and_Decay.IsDistanceGood)
        {
            SpellManager.CastSpellByIDAndPosition(43265, ObjectManager.Target.Position);
        }

        if (ObjectManager.GetNumberAttackPlayer() < 4 && ObjectManager.GetNumberAttackPlayer() > 1 && Heart_Strike.IsSpellUsable 
            && Heart_Strike.IsDistanceGood && Heart_Strike.KnownSpell)
        {
            Heart_Strike.Launch();
            return;
        }

        if (ObjectManager.GetNumberAttackPlayer() > 4 && Army_of_the_Dead.IsSpellUsable && Army_of_the_Dead.IsDistanceGood && Army_of_the_Dead.KnownSpell)
        {
            Army_of_the_Dead.Launch();
            Thread.Sleep(4000);
            return;
        }

        if (ObjectManager.Me.HaveBuff(81141))
        {
            Blood_Boil.Launch();
            if (Blood_Plague.TargetHaveBuff && Frost_Fever.TargetHaveBuff)
            {
                Plague_Strike_Timer = new Timer(1000 * 27);
                Frost_Fever_Timer = new Timer(1000 * 27);
            }
            return;
        }

        if (Soul_Reaper.KnownSpell && Soul_Reaper.IsDistanceGood && Soul_Reaper.IsSpellUsable
            && ObjectManager.Target.HealthPercent < 35 && ObjectManager.Me.HealthPercent > 90)
        {
            Soul_Reaper.Launch();
            return;
        }

        if (Death_Strike.IsSpellUsable && Death_Strike.IsDistanceGood && Death_Strike.KnownSpell)
        {
            Logging.WriteFight("Use Trinket 1.");
            Lua.RunMacroText("/use 13");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Logging.WriteFight("Use Trinket 2.");
            Lua.RunMacroText("/use 14");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");

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

            Death_Strike.Launch();
            return;
        }

        if (Dancing_Rune_Weapon_Timer.IsReady && DRW == 0)
            DRW++;

        if (DRW == 1 && Dancing_Rune_Weapon.IsSpellUsable && Dancing_Rune_Weapon.KnownSpell && Dancing_Rune_Weapon.IsDistanceGood)
        {
            Logging.WriteFight("Cast Dancing Rune Weapon.");
            Lua.RunMacroText("/cast Dancing Rune Weapon");
            Dancing_Rune_Weapon_Timer = new Timer(90000);
            DRW--;
        }

        if (Rune_Strike.IsSpellUsable && Rune_Strike.IsDistanceGood && Rune_Strike.KnownSpell && DRW == 0)
        {
            Rune_Strike.Launch();
            return;
        }

        if (Blood_Strike.IsSpellUsable && Blood_Strike.IsDistanceGood && Blood_Strike.KnownSpell)
        {
            Blood_Strike.Launch();
            return;
        }
        /*if (Heart_Strike.IsSpellUsable && Heart_Strike.IsDistanceGood && Heart_Strike.KnownSpell)
        {
            Heart_Strike.Launch();
            return;
        }*/

        if (Empower_Rune_Weapon.IsSpellUsable && Empower_Rune_Weapon.KnownSpell)
        {
            Empower_Rune_Weapon.Launch();
            return;
        }

        if (Arcane_Torrent.IsSpellUsable && Arcane_Torrent.KnownSpell)
        {
            Arcane_Torrent.Launch();
            return;
        }
    }

    public void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            Buff();
        }
    }

    private void Blood_Presence_Buff()
    {
        if (!Blood_Presence.HaveBuff && Blood_Presence.KnownSpell)
        {
            Blood_Presence.Launch();
            return;
        }
    }

    private void Unholy_Presence_Buff()
    {
        if (!Unholy_Presence.HaveBuff && Unholy_Presence.KnownSpell)
        {
            Unholy_Presence.Launch();
            return;
        }
    }

    private void Grip()
    {
        if (ObjectManager.Target.GetDistance > 5 &&
            Death_Grip.KnownSpell && Death_Grip.IsSpellUsable && Death_Grip.IsDistanceGood)
        {
            Death_Grip.Launch();
            MovementManager.StopMove();
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.HealthPercent < 80 && Gift_of_the_Naaru.IsSpellUsable && Gift_of_the_Naaru.KnownSpell)
        {
            Gift_of_the_Naaru.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 55 &&
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

        if (ObjectManager.Me.HealthPercent < 45 &&
            Lichborne.KnownSpell && Lichborne.IsSpellUsable &&
            Death_Coil.KnownSpell && Death_Coil.IsSpellUsable)
        {
            if (Lichborne.IsSpellUsable)
            {
                Lichborne.Launch();
                return;
            }
        }

        if (ObjectManager.Me.HealthPercent < 80 && Death_Siphon.KnownSpell && Death_Siphon.IsSpellUsable)
        {
            Death_Siphon.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 70)
        {
            if (Vampiric_Blood.KnownSpell && Vampiric_Blood.IsSpellUsable)
            {
                Vampiric_Blood.Launch();
                Thread.Sleep(200);
            }

            if (!Rune_Tap.IsSpellUsable && Blood_Tap.KnownSpell && Blood_Tap.IsSpellUsable)
            {
                Blood_Tap.Launch();
                Thread.Sleep(500);
            }

            if (Rune_Tap.KnownSpell && Rune_Tap.IsSpellUsable &&
                (Vampiric_Blood.HaveBuff || ObjectManager.Me.HaveBuff(81164)))
                Rune_Tap.Launch();
        }
    }

    private void Resistance()
    {
        if (!Bone_Shield.HaveBuff && Bone_Shield.KnownSpell && Bone_Shield.IsSpellUsable)
        {
            Bone_Shield.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 80 &&
            Icebound_Fortitude.KnownSpell && Icebound_Fortitude.IsSpellUsable)
        {
            Icebound_Fortitude.Launch();
        }

        if (ObjectManager.Me.HealthPercent < 80 && Stoneform.IsSpellUsable && Stoneform.KnownSpell)
        {
            Stoneform.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 70 || ObjectManager.GetNumberAttackPlayer() > 1
            && ObjectManager.Target.GetDistance < 8)
        {
            if (Remorseless_Winter.KnownSpell && Remorseless_Winter.IsSpellUsable)
            {
                Remorseless_Winter.Launch();
                return;
            }
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

public class Deathknight_Unholy
{
    #region InitializeSpell

    private Spell AntiMagic_Shell = new Spell("Anti-Magic Shell");
    private Spell AntiMagic_Zone = new Spell("Anti-Magic Zone");
    private Spell Army_of_the_Dead = new Spell("Army of the Dead");
    private Spell Asphyxiate = new Spell("Asphyxiate");
    private Spell Blood_Boil = new Spell("Blood Boil");
    private Spell Blood_Plague = new Spell("Blood Plague");
    private Spell Blood_Presence = new Spell("Blood Presence");
    private Spell Blood_Strike = new Spell("Blood Strike");
    private Spell Blood_Tap = new Spell("Blood Tap");
    private Spell Chains_of_Ice = new Spell("Chains of Ice");
    private Spell Dark_Simulacrum = new Spell("Dark Simulacrum");
    private Spell Dark_Transformation = new Spell("Dark Transformation");
    private Spell Deaths_Advance = new Spell("Death's Advance");
    private Spell Death_and_Decay = new Spell("Death and Decay");
    private Spell Death_Coil = new Spell("Death Coil");
    private Spell Death_Grip = new Spell("Death Grip");
    private Spell Death_Pact = new Spell("Death Pact");
    private Spell Death_Siphon = new Spell("Death Siphon");
    private Spell Death_Strike = new Spell("Death Strike");
    private Spell Empower_Rune_Weapon = new Spell("Empower Rune Weapon");
    private Spell Festering_Strike = new Spell("Festering Strike");
    private Spell Frost_Fever = new Spell("Frost Fever");
    private Spell Frost_Presence = new Spell("Frost Presence");
    private Spell Horn_of_Winter = new Spell("Horn of Winter");
    private Spell Icebound_Fortitude = new Spell("Icebound Fortitude");
    private Spell Icy_Touch = new Spell("Icy Touch");
    private Spell Lichborne = new Spell("Lichborne");
    private Spell Mind_Freeze = new Spell("Mind Freeze");
    private Spell Necrotic_Strike = new Spell("Necrotic Strike");
    private Spell Outbreak = new Spell("Outbreak");
    private Spell Path_of_Frost = new Spell("Path of Frost");
    private Spell Pestilence = new Spell("Pestilence");
    private Spell Plague_Leech = new Spell("Plague Leech");
    private Spell Plague_Strike = new Spell("Plague Strike");
    private Spell Raise_Ally = new Spell("Raise Ally");
    private Spell Raise_Dead = new Spell("Raise Dead");
    private Spell Remorseless_Winter = new Spell("Remorseless Winter");
    private Spell Soul_Reaper = new Spell("Soul Reaper");
    private Spell Scourge_Strike = new Spell("Scourge Strike");
    private Spell Strangulate = new Spell("Strangulate");
    private Spell Summon_Gargoyle = new Spell("Summon Gargoyle");
    private Spell Unholy_Blight = new Spell("Unholy Blight");
    private Spell Unholy_Frenzy = new Spell("Unholy Frenzy");
    private Spell Unholy_Presence = new Spell("Unholy Presence");

    private Timer Dark_Transformation_Timer = new Timer(0);
    private Timer Frost_Fever_Timer = new Timer(0);
    private Timer Path_of_Frost_Timer = new Timer(0);
    private Timer Pestilence_Timer = new Timer(0);
    private Timer Plague_Strike_Timer = new Timer(0);
    private Timer Summon_Gargoyle_Timer = new Timer(0);

    // Profession & Racials

    private Spell Arcane_Torrent = new Spell("Arcane Torrent");
    private Spell Berserking = new Spell("Berserking");
    private Spell Blood_Fury = new Spell("Blood Fury");
    private Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    private Spell Lifeblood = new Spell("Lifeblood");
    private Spell Stoneform = new Spell("Stoneform");

    public int SG = 1;
    public int DT = 1;

    #endregion InitializeSpell

    public Deathknight_Unholy()
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
        Grip();
    }

    private void Buff_path()
    {
        if (!Fight.InFight && !Path_of_Frost.HaveBuff && Path_of_Frost.KnownSpell && Path_of_Frost.IsSpellUsable)
        {
            Path_of_Frost.Launch();
            Path_of_Frost_Timer = new Timer(1000 * 60 * 9);
        }
        if (Path_of_Frost_Timer.IsReady)
        {
            Path_of_Frost.Launch();
            Path_of_Frost_Timer = new Timer(1000 * 60 * 9);
        }
    }

    public void LowCombat()
    {
        Unholy_Presence_Buff();
        AvoidMelee();
        Grip();
        Heal();
        Resistance();
        Buff();
        if (Icy_Touch.KnownSpell && Icy_Touch.IsSpellUsable && Icy_Touch.IsDistanceGood)
        {
            Icy_Touch.Launch();
            if (ObjectManager.Target.HealthPercent < 50 && ObjectManager.Target.HealthPercent > 0)
            {
                Icy_Touch.Launch();
                return;
            }
            return;
        }

        if (Death_Coil.KnownSpell && Death_Coil.IsSpellUsable && Death_Coil.IsDistanceGood)
        {
            Death_Coil.Launch();
            return;
        }

        if (Blood_Boil.IsSpellUsable && Blood_Boil.IsSpellUsable && Blood_Boil.IsDistanceGood)
        {
            Blood_Boil.Launch();
            return;
        }
    }

    public void Combat()
    {
        Unholy_Presence_Buff();
        Ghoul();
        AvoidMelee();
        Grip();
        Decast();
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

        if (Unholy_Blight.KnownSpell && Unholy_Blight.IsSpellUsable && Unholy_Blight.IsDistanceGood)
        {
            Unholy_Blight.Launch();
            Plague_Strike_Timer = new Timer(1000 * 27);
            Frost_Fever_Timer = new Timer(1000 * 27);
            return;
        }

        if (Outbreak.KnownSpell && Outbreak.IsSpellUsable && Outbreak.IsDistanceGood && (Plague_Strike_Timer.IsReady
            || Frost_Fever_Timer.IsReady))
        {
            Outbreak.Launch();
            Plague_Strike_Timer = new Timer(1000 * 27);
            Frost_Fever_Timer = new Timer(1000 * 27);
            return;
        }

        if (!Outbreak.KnownSpell)
        {
            if (!Blood_Plague.TargetHaveBuff &&
                Plague_Strike.KnownSpell && Plague_Strike.IsSpellUsable && Plague_Strike.IsDistanceGood)
            {
                Plague_Strike.Launch();
                Plague_Strike_Timer = new Timer(1000 * 27);
                return;
            }

            if (!Frost_Fever.TargetHaveBuff &&
                Icy_Touch.KnownSpell && Icy_Touch.IsSpellUsable && Icy_Touch.IsDistanceGood)
            {
                Icy_Touch.Launch();
                Frost_Fever_Timer = new Timer(1000 * 27);
                return;
            }
        }

        if (Plague_Strike_Timer.IsReady && Frost_Fever_Timer.IsReady && Blood_Plague.TargetHaveBuff && Frost_Fever.TargetHaveBuff)
        {
            Festering_Strike.Launch();
            Plague_Strike_Timer = new Timer(1000 * 27);
            Frost_Fever_Timer = new Timer(1000 * 27);
            return;
        }

        if (Plague_Strike_Timer.IsReady && Plague_Strike.KnownSpell && Plague_Strike.IsDistanceGood &&
            !Outbreak.IsSpellUsable && !Unholy_Blight.IsSpellUsable)
        {
            if (!Plague_Strike.IsSpellUsable && Blood_Tap.KnownSpell && Blood_Tap.IsSpellUsable)
            {
                Blood_Tap.Launch();
                Thread.Sleep(500);
            }

            if (Plague_Strike.IsSpellUsable)
            {
                Plague_Strike.Launch();
                Plague_Strike_Timer = new Timer(1000 * 27);
                return;
            }
        }

        if (Frost_Fever_Timer.IsReady && Icy_Touch.KnownSpell && Icy_Touch.IsDistanceGood &&
            !Outbreak.IsSpellUsable && !Unholy_Blight.IsSpellUsable)
        {
            if (!Icy_Touch.IsSpellUsable && Blood_Tap.KnownSpell && Blood_Tap.IsSpellUsable)
            {
                Blood_Tap.Launch();
                Thread.Sleep(500);
            }

            if (Icy_Touch.IsSpellUsable)
            {
                Icy_Touch.Launch();
                Frost_Fever_Timer = new Timer(1000 * 27);
                return;
            }
        }

        if (Unholy_Frenzy.IsSpellUsable && Unholy_Frenzy.KnownSpell)
        {
            Logging.WriteFight("Use Trinket 1.");
            Lua.RunMacroText("/use 13");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Logging.WriteFight("Use Trinket 2.");
            Lua.RunMacroText("/use 14");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");

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

            Unholy_Frenzy.Launch();
            return;
        }

        if (Summon_Gargoyle_Timer.IsReady && SG == 0)
            SG++;

        if (SG == 1 && Summon_Gargoyle.IsSpellUsable && Summon_Gargoyle.KnownSpell && Summon_Gargoyle.IsDistanceGood)
        {
            Logging.WriteFight("Use Trinket 1.");
            Lua.RunMacroText("/use 13");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Logging.WriteFight("Use Trinket 2.");
            Lua.RunMacroText("/use 14");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");

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

            Summon_Gargoyle.Launch();
            Summon_Gargoyle_Timer = new Timer(180000);
            SG--;
        }

        if (ObjectManager.GetNumberAttackPlayer() > 2 && Pestilence.IsSpellUsable && Pestilence.IsDistanceGood && Pestilence.KnownSpell)
        {
            Pestilence.Launch();
            return;
        }

        if (Dark_Transformation_Timer.IsReady && DT == 0)
            DT++;

        if (DT == 1 && Dark_Transformation.IsSpellUsable && Dark_Transformation.KnownSpell && Dark_Transformation.IsDistanceGood)
        {
            Logging.WriteFight("Use Trinket 1.");
            Lua.RunMacroText("/use 13");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Logging.WriteFight("Use Trinket 2.");
            Lua.RunMacroText("/use 14");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");

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

            Dark_Transformation.Launch();
            Dark_Transformation_Timer = new Timer(30000);
            DT--;
        }

        if (Dark_Transformation_Timer.IsReady && DT == 1 && SG == 0)
        {
            if (Death_Coil.KnownSpell && Death_Coil.IsSpellUsable && Death_Coil.IsDistanceGood)
            {
                Death_Coil.Launch();
                return;
            }
        }

        if (ObjectManager.GetNumberAttackPlayer() > 2 &&
            Death_and_Decay.KnownSpell && Death_and_Decay.IsSpellUsable && Death_and_Decay.IsDistanceGood)
        {
            SpellManager.CastSpellByIDAndPosition(43265, ObjectManager.Target.Position);
        }

        if (ObjectManager.GetNumberAttackPlayer() > 4 && Army_of_the_Dead.IsSpellUsable && Army_of_the_Dead.IsDistanceGood && Army_of_the_Dead.KnownSpell)
        {
            Army_of_the_Dead.Launch();
            Thread.Sleep(4000);
            return;
        }

        if (ObjectManager.GetNumberAttackPlayer() > 2 && Blood_Boil.IsSpellUsable && Blood_Boil.IsDistanceGood && Blood_Boil.KnownSpell)
        {
            Blood_Boil.Launch();
            return;
        }

        if (Soul_Reaper.KnownSpell && Soul_Reaper.IsDistanceGood && Soul_Reaper.IsSpellUsable
            && ObjectManager.Target.HealthPercent < 35)
        {
            Soul_Reaper.Launch();
            return;
        }

        if (ObjectManager.Me.RunicPowerPercentage < 90 &&
            Scourge_Strike.KnownSpell && Scourge_Strike.IsSpellUsable && Scourge_Strike.IsDistanceGood)
        {
            Scourge_Strike.Launch();
            return;
        }

        if (ObjectManager.Me.RunicPowerPercentage < 90 &&
            Festering_Strike.KnownSpell && Festering_Strike.IsSpellUsable && Festering_Strike.IsDistanceGood)
        {
            Festering_Strike.Launch();
            return;
        }

        if (ObjectManager.Me.RunicPowerPercentage >= 90 || ObjectManager.Me.HaveBuff(81340)
            && Death_Coil.KnownSpell && Death_Coil.IsSpellUsable && Death_Coil.IsDistanceGood)
        {
            if (Blood_Tap.IsSpellUsable && Blood_Tap.KnownSpell)
            {
                Blood_Tap.Launch();
                Thread.Sleep(500);
            }
            Death_Coil.Launch();
            return;
        }

        if (Horn_of_Winter.KnownSpell && Horn_of_Winter.IsSpellUsable)
        {
            Horn_of_Winter.Launch();
        }

        if (Empower_Rune_Weapon.IsSpellUsable && Empower_Rune_Weapon.KnownSpell)
        {
            Empower_Rune_Weapon.Launch();
            return;
        }

        if (Arcane_Torrent.IsSpellUsable && Arcane_Torrent.KnownSpell)
        {
            Arcane_Torrent.Launch();
            return;
        }
    }

    public void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            Buff();
        }
    }

    private void Unholy_Presence_Buff()
    {
        if (!Unholy_Presence.HaveBuff && Unholy_Presence.KnownSpell)
        {
            Unholy_Presence.Launch();
            return;
        }
    }

    private void Grip()
    {
        if (ObjectManager.Target.GetDistance > 5 &&
            Death_Grip.KnownSpell && Death_Grip.IsSpellUsable && Death_Grip.IsDistanceGood)
        {
            Death_Grip.Launch();
            MovementManager.StopMove();
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.HealthPercent < 80 && Gift_of_the_Naaru.IsSpellUsable && Gift_of_the_Naaru.KnownSpell)
        {
            Gift_of_the_Naaru.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 60 &&
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

        if (ObjectManager.Me.HealthPercent < 80 && Death_Siphon.KnownSpell && Death_Siphon.IsSpellUsable)
        {
            Death_Siphon.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 45 &&
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
        if (ObjectManager.Me.HealthPercent < 80 &&
            Icebound_Fortitude.KnownSpell && Icebound_Fortitude.IsSpellUsable)
        {
            Icebound_Fortitude.Launch();
        }

        if (ObjectManager.Me.HealthPercent < 80 && Stoneform.IsSpellUsable && Stoneform.KnownSpell)
        {
            Stoneform.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 70 || ObjectManager.GetNumberAttackPlayer() > 1
            && ObjectManager.Target.GetDistance < 8)
        {
            if (Remorseless_Winter.KnownSpell && Remorseless_Winter.IsSpellUsable)
            {
                Remorseless_Winter.Launch();
                return;
            }
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
    }

    private void Ghoul()
    {
        if ((ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0) &&
            !ObjectManager.Me.IsMounted && !ObjectManager.Me.IsDeadMe)
        {
            Logging.WriteFight(" - PET DEAD - ");
            if (Raise_Dead.KnownSpell && Raise_Dead.IsSpellUsable)
            {
                Logging.WriteFight(" - SUMMONING PET - ");
                Raise_Dead.Launch();
            }
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

public class Deathknight_Frost
{
    #region InitializeSpell

    private Spell AntiMagic_Shell = new Spell("Anti-Magic Shell");
    private Spell AntiMagic_Zone = new Spell("Anti-Magic Zone");
    private Spell Army_of_the_Dead = new Spell("Army of the Dead");
    private Spell Asphyxiate = new Spell("Asphyxiate");
    private Spell Blood_Boil = new Spell("Blood Boil");
    private Spell Blood_Plague = new Spell("Blood Plague");
    private Spell Blood_Presence = new Spell("Blood Presence");
    private Spell Blood_Strike = new Spell("Blood Strike");
    private Spell Blood_Tap = new Spell("Blood Tap");
    private Spell Chains_of_Ice = new Spell("Chains of Ice");
    private Spell Dark_Simulacrum = new Spell("Dark Simulacrum");
    private Spell Deaths_Advance = new Spell("Death's Advance");
    private Spell Death_and_Decay = new Spell("Death and Decay");
    private Spell Death_Coil = new Spell("Death Coil");
    private Spell Death_Grip = new Spell("Death Grip");
    private Spell Death_Pact = new Spell("Death Pact");
    private Spell Death_Siphon = new Spell("Death Siphon");
    private Spell Death_Strike = new Spell("Death Strike");
    private Spell Empower_Rune_Weapon = new Spell("Empower Rune Weapon");
    private Spell Frost_Fever = new Spell("Frost Fever");
    private Spell Frost_Presence = new Spell("Frost Presence");
    private Spell Frost_Strike = new Spell("Frost Strike");
    private Spell Horn_of_Winter = new Spell("Horn of Winter");
    private Spell Howling_Blast = new Spell("Howling Blast");
    private Spell Icebound_Fortitude = new Spell("Icebound Fortitude");
    private Spell Icy_Touch = new Spell("Icy Touch");
    private Spell Lichborne = new Spell("Lichborne");
    private Spell Mind_Freeze = new Spell("Mind Freeze");
    private Spell Necrotic_Strike = new Spell("Necrotic Strike");
    private Spell Obliterate = new Spell("Obliterate");
    private Spell Outbreak = new Spell("Outbreak");
    private Spell Path_of_Frost = new Spell("Path of Frost");
    private Spell Pestilence = new Spell("Pestilence");
    private Spell Pillar_of_Frost = new Spell("Pillar of Frost");
    private Spell Plague_Leech = new Spell("Plague Leech");
    private Spell Plague_Strike = new Spell("Plague Strike");
    private Spell Raise_Ally = new Spell("Raise Ally");
    private Spell Raise_Dead = new Spell("Raise Dead");
    private Spell Remorseless_Winter = new Spell("Remorseless Winter");
    private Spell Soul_Reaper = new Spell("Soul Reaper");
    private Spell Strangulate = new Spell("Strangulate");
    private Spell Unholy_Blight = new Spell("Unholy Blight");
    private Spell Unholy_Presence = new Spell("Unholy Presence");

    private Timer Frost_Fever_Timer = new Timer(0);
    private Timer Path_of_Frost_Timer = new Timer(0);
    private Timer Pestilence_Timer = new Timer(0);
    private Timer Plague_Strike_Timer = new Timer(0);

    // Profession & Racials

    private Spell Arcane_Torrent = new Spell("Arcane Torrent");
    private Spell Berserking = new Spell("Berserking");
    private Spell Blood_Fury = new Spell("Blood Fury");
    private Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    private Spell Lifeblood = new Spell("Lifeblood");
    private Spell Stoneform = new Spell("Stoneform");

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
        Grip();
    }

    private void Buff_path()
    {
        if (!Fight.InFight && !Path_of_Frost.HaveBuff && Path_of_Frost.KnownSpell && Path_of_Frost.IsSpellUsable)
        {
            Path_of_Frost.Launch();
            Path_of_Frost_Timer = new Timer(1000 * 60 * 9);
        }
        if (Path_of_Frost_Timer.IsReady)
        {
            Path_of_Frost.Launch();
            Path_of_Frost_Timer = new Timer(1000 * 60 * 9);
        }
    }

    public void LowCombat()
    {
        Unholy_Presence_Buff();
        AvoidMelee();
        Grip();
        Heal();
        Resistance();
        Buff();

        if (Howling_Blast.KnownSpell && Howling_Blast.IsSpellUsable && Howling_Blast.IsDistanceGood)
        {
            Howling_Blast.Launch();
            if (ObjectManager.Target.HealthPercent < 50 && ObjectManager.Target.HealthPercent > 0)
            {
                Howling_Blast.Launch();
                return;
            }
            return;
        }

        if (ObjectManager.Target.HealthPercent > 90)
        {
            if (Blood_Boil.KnownSpell && Blood_Boil.IsSpellUsable && Blood_Boil.IsDistanceGood)
            {
                Blood_Boil.Launch();
                return;
            }
        }

        if (Death_Coil.KnownSpell && Death_Coil.IsSpellUsable && Death_Coil.IsDistanceGood)
        {
            Death_Coil.Launch();
            return;
        }
    }

    public void Combat()
    {
        Frost_Presence_Buff();
        AvoidMelee();
        Grip();
        Decast();
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

        if (Unholy_Blight.KnownSpell && Unholy_Blight.IsSpellUsable && Unholy_Blight.IsDistanceGood)
        {
            Unholy_Blight.Launch();
            Plague_Strike_Timer = new Timer(1000 * 27);
            Frost_Fever_Timer = new Timer(1000 * 27);
            return;
        }

        if (Outbreak.KnownSpell && Outbreak.IsSpellUsable && Outbreak.IsDistanceGood && (Plague_Strike_Timer.IsReady
            || Frost_Fever_Timer.IsReady))
        {
            Outbreak.Launch();
            Plague_Strike_Timer = new Timer(1000 * 27);
            Frost_Fever_Timer = new Timer(1000 * 27);
            return;
        }

        if (!Outbreak.KnownSpell)
        {
            if (!Blood_Plague.TargetHaveBuff &&
                Plague_Strike.KnownSpell && Plague_Strike.IsSpellUsable && Plague_Strike.IsDistanceGood)
            {
                Plague_Strike.Launch();
                Plague_Strike_Timer = new Timer(1000 * 27);
                return;
            }

            if (!Frost_Fever.TargetHaveBuff &&
                Howling_Blast.KnownSpell && Howling_Blast.IsSpellUsable && Howling_Blast.IsDistanceGood)
            {
                Howling_Blast.Launch();
                Frost_Fever_Timer = new Timer(1000 * 27);
                return;
            }
        }

        if (Plague_Strike_Timer.IsReady && Plague_Strike.KnownSpell && Plague_Strike.IsDistanceGood &&
            !Outbreak.IsSpellUsable && !Unholy_Blight.IsSpellUsable)
        {
            if (!Plague_Strike.IsSpellUsable && Blood_Tap.KnownSpell && Blood_Tap.IsSpellUsable)
            {
                Blood_Tap.Launch();
                Thread.Sleep(500);
            }

            if (Plague_Strike.IsSpellUsable)
            {
                Plague_Strike.Launch();
                Plague_Strike_Timer = new Timer(1000 * 27);
                return;
            }
        }

        if (Frost_Fever_Timer.IsReady && Howling_Blast.KnownSpell && Howling_Blast.IsDistanceGood &&
            !Outbreak.IsSpellUsable && !Unholy_Blight.IsSpellUsable)
        {
            if (!Howling_Blast.IsSpellUsable && Blood_Tap.KnownSpell && Blood_Tap.IsSpellUsable)
            {
                Blood_Tap.Launch();
                Thread.Sleep(500);
            }

            if (Howling_Blast.IsSpellUsable)
            {
                Howling_Blast.Launch();
                Frost_Fever_Timer = new Timer(1000 * 27);
                return;
            }
        }

        if (Pillar_of_Frost.IsSpellUsable && Pillar_of_Frost.KnownSpell)
        {
            Logging.WriteFight("Use Trinket 1.");
            Lua.RunMacroText("/use 13");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Logging.WriteFight("Use Trinket 2.");
            Lua.RunMacroText("/use 14");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");

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

            Pillar_of_Frost.Launch();
            return;
        }

        if (ObjectManager.Me.HaveBuff(59052) && Howling_Blast.KnownSpell && Howling_Blast.IsDistanceGood)
        {
            Howling_Blast.Launch();
            Frost_Fever_Timer = new Timer(1000 * 27);
            return;
        }

        if (ObjectManager.GetNumberAttackPlayer() > 2 &&
            Death_and_Decay.KnownSpell && Death_and_Decay.IsSpellUsable && Death_and_Decay.IsDistanceGood)
        {
            SpellManager.CastSpellByIDAndPosition(43265, ObjectManager.Target.Position);
        }

        if (ObjectManager.GetNumberAttackPlayer() > 4 && Army_of_the_Dead.IsSpellUsable && Army_of_the_Dead.IsDistanceGood && Army_of_the_Dead.KnownSpell)
        {
            Army_of_the_Dead.Launch();
            Thread.Sleep(4000);
            return;
        }

        if (ObjectManager.Me.RunicPowerPercentage >= 90 &&
            Blood_Strike.KnownSpell && Blood_Strike.IsSpellUsable && Blood_Strike.IsDistanceGood)
        {
            Blood_Strike.Launch();
            return;
        }
        /*Frost_Strike.KnownSpell && Frost_Strike.IsSpellUsable && Frost_Strike.IsDistanceGood)
    {
        Frost_Strike.Launch();
        return;
    }*/
        if (Soul_Reaper.KnownSpell && Soul_Reaper.IsDistanceGood && Soul_Reaper.IsSpellUsable
            && ObjectManager.Target.HealthPercent < 35)
        {
            Soul_Reaper.Launch();
            return;
        }

        if (ObjectManager.Me.HaveBuff(51128) &&
            Blood_Strike.KnownSpell && Blood_Strike.IsSpellUsable && Blood_Strike.IsDistanceGood)
        {
            Blood_Strike.Launch();
            return;
        }
        /*Frost_Strike.KnownSpell && Frost_Strike.IsSpellUsable && Frost_Strike.IsDistanceGood)
    {
        Frost_Strike.Launch();
        return;
    }*/

        if (ObjectManager.Me.HaveBuff(51128) && Obliterate.KnownSpell && Obliterate.IsSpellUsable && Obliterate.IsDistanceGood)
        //&& ObjectManager.Me.runeReady(5) && ObjectManager.Me.runeReady(6))
        {
            if (ObjectManager.Me.HealthPercent < 55 &&
                Death_Strike.KnownSpell && Death_Strike.IsSpellUsable && Death_Strike.IsDistanceGood)
            {
                Death_Strike.Launch();
                return;
            }
            Obliterate.Launch();
            return;
        }

        if (Obliterate.KnownSpell && Obliterate.IsSpellUsable && Obliterate.IsDistanceGood)
        //&& ObjectManager.Me.runeReady(5) && ObjectManager.Me.runeReady(6))
        {
            if (ObjectManager.Me.HealthPercent < 55 &&
                Death_Strike.KnownSpell && Death_Strike.IsSpellUsable && Death_Strike.IsDistanceGood)
            {
                Death_Strike.Launch();
                return;
            }
            Obliterate.Launch();
            return;
        }

        if (Blood_Strike.KnownSpell && Blood_Strike.IsSpellUsable && Blood_Strike.IsDistanceGood)
        {
            Blood_Strike.Launch();
            return;
        }
        /*Frost_Strike.Launch();
        return;
    }*/

        /*if (Howling_Blast.KnownSpell && Howling_Blast.IsSpellUsable && Howling_Blast.IsDistanceGood)
        {
            Howling_Blast.Launch();
            Frost_Fever_Timer = new Timer(1000*27);
            return;
        }*/

        if (Horn_of_Winter.KnownSpell && Horn_of_Winter.IsSpellUsable)
        {
            Horn_of_Winter.Launch();
        }

        if (Empower_Rune_Weapon.IsSpellUsable && Empower_Rune_Weapon.KnownSpell)
        //!Frost_Strike.IsSpellUsable
        {
            Empower_Rune_Weapon.Launch();
            return;
        }

        if (Arcane_Torrent.IsSpellUsable && Arcane_Torrent.KnownSpell)
        {
            Arcane_Torrent.Launch();
            return;
        }
    }

    public void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            Buff();
        }
    }

    private void Frost_Presence_Buff()
    {
        if (!Frost_Presence.HaveBuff && Frost_Presence.KnownSpell)
        {
            Frost_Presence.Launch();
            return;
        }
    }

    private void Unholy_Presence_Buff()
    {
        if (!Unholy_Presence.HaveBuff && Unholy_Presence.KnownSpell)
        {
            Unholy_Presence.Launch();
            return;
        }
    }

    private void Grip()
    {
        if (ObjectManager.Target.GetDistance > 5 &&
            Death_Grip.KnownSpell && Death_Grip.IsSpellUsable && Death_Grip.IsDistanceGood)
        {
            Death_Grip.Launch();
            MovementManager.StopMove();
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.HealthPercent < 80 && Gift_of_the_Naaru.IsSpellUsable && Gift_of_the_Naaru.KnownSpell)
        {
            Gift_of_the_Naaru.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 60 &&
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

        if (ObjectManager.Me.HealthPercent < 80 && Death_Siphon.KnownSpell && Death_Siphon.IsSpellUsable)
        {
            Death_Siphon.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 45 &&
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
        if (ObjectManager.Me.HealthPercent < 80 &&
            Icebound_Fortitude.KnownSpell && Icebound_Fortitude.IsSpellUsable)
        {
            Icebound_Fortitude.Launch();
        }

        if (ObjectManager.Me.HealthPercent < 80 && Stoneform.IsSpellUsable && Stoneform.KnownSpell)
        {
            Stoneform.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 70 || ObjectManager.GetNumberAttackPlayer() > 1
            && ObjectManager.Target.GetDistance < 8)
        {
            if (Remorseless_Winter.KnownSpell && Remorseless_Winter.IsSpellUsable)
            {
                Remorseless_Winter.Launch();
                return;
            }
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