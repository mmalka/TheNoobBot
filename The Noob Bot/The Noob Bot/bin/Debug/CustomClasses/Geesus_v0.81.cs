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

    public void ShowConfiguration()
    {
        MessageBox.Show("No setting for this custom class.");
    }

    public void Initialize()
    {
        try
        {
            switch (ObjectManager.Me.WowClass)
            {
                case WoWClass.DeathKnight:
                    var Heart_Strike = new Spell("Heart Strike");
                    var Scourge_Strike = new Spell("Scourge Strike");
                    var Frost_Strike = new Spell("Frost Strike");
                    if (Heart_Strike.KnownSpell)
                    {
                        Logging.WriteFight("Blood Deathknight Found");
                        new Blood();
                    }
                    if (Scourge_Strike.KnownSpell)
                    {
                        Logging.WriteFight("Unholy Deathknight Found");
                        new Unholy();
                    }
                    if (Frost_Strike.KnownSpell)
                    {
                        Logging.WriteFight("Frost Deathknight Found");
                        new Frostdk();
                    }
                    if (!Heart_Strike.KnownSpell && !Scourge_Strike.KnownSpell && !Frost_Strike.KnownSpell)
                    {
                        Logging.WriteFight("Deathknight without Spec");
                        new Unholy();
                    }
                    break;
                case WoWClass.Mage:
                    var Summon_Water_Elemental = new Spell("Summon Water Elemental");
                    var Arcane_Barrage = new Spell("Arcane Barrage");
                    var Pyroblast = new Spell("Pyroblast");
                    if (Summon_Water_Elemental.KnownSpell)
                    {
                        Logging.WriteFight("Frost Mage Found");
                        new Frost();
                    }
                    if (Arcane_Barrage.KnownSpell)
                    {
                        Logging.WriteFight("Arcane Mage found");
                        new Arcane();
                    }
                    if (Pyroblast.KnownSpell)
                    {
                        Logging.WriteFight("Fire Mage found");
                        new Fire();
                    }
                    if (!Summon_Water_Elemental.KnownSpell && !Arcane_Barrage.KnownSpell && !Pyroblast.KnownSpell)
                    {
                        Logging.WriteFight("Mage without Spec");
                        new Frost();
                    }
                    break;
                case WoWClass.Warlock:
                    var Summon_Felguard = new Spell("Summon Felguard");
                    var Unstable_Affliction = new Spell("Unstable Affliction");
                    if (Unstable_Affliction.KnownSpell)
                    {
                        Logging.WriteFight("Affli Warlock Found");
                        new Affli();
                    }
                    if (Summon_Felguard.KnownSpell)
                    {
                        Logging.WriteFight("Demo Warlock Found");
                        new Demo();
                    }
                    if (!Unstable_Affliction.KnownSpell && !Summon_Felguard.KnownSpell)
                    {
                        Logging.WriteFight("Warlock without Spec");
                        new Demo();
                    }
                    break;
                case WoWClass.Druid:
                    var Starsurge = new Spell("Starsurge");
                    if (Starsurge.KnownSpell)
                    {
                        Logging.WriteFight("Balance Dudu Found");
                        new Balance();
                    }
                    if (!Starsurge.KnownSpell)
                    {
                        Logging.WriteFight("Dudu without Spec");
                        new Balance();
                    }
                    break;
                case WoWClass.Paladin:
                    break;
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
                case WoWClass.Rogue:
                    break;
                case WoWClass.Warrior:
                    var Mortal_Strike = new Spell("Mortal Strike");
                    if (Mortal_Strike.KnownSpell)
                    {
                        Logging.WriteFight("Arms Warrior Found");
                        new Arms();
                    }
                    if (!Mortal_Strike.KnownSpell)
                    {
                        Logging.WriteFight("Warrior without Spec");
                        new Arms();
                    }
                    break;
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
                    if (!Explosive_Shot.KnownSpell && !Aimed_Shot.KnownSpell)
                    {
                        Logging.WriteFight("Hunter without Spec");
                        new Survival();
                    }
                    break;
                default:
                    Dispose();
                    break;
            }
        }
        catch (Exception e) { }
    }

    public void Dispose()
    {
        Logging.WriteFight("Closing The Noob Bot CC");
        loop = false;
    }

}

public class Blood
{
    Int32 fail = 0;
    Int32 fail2 = 0;
    Int32 boltcount = 0;
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
    Spell Tailoring = new Spell("Tailoring");
    Spell Leatherworking = new Spell("Leatherworking");
    Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    Spell War_Stomp = new Spell("War Stomp");
    Spell Berserking = new Spell("Berserking");

    #endregion InitializeSpell

    public Blood()
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
            Thread.Sleep(30);
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
            else
            {
                if (ObjectManager.Me.HealthPercent > 63 || !Lichborne.IsSpellUsable)
                {
                    gcd = new Timer(2000);
                    SpellManager.CastSpellByIdLUA(56815);
                    // Rune_Strike.Launch();
                    return;
                }
                return;
            }
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
            else if (!Every_Man_for_Himself.KnownSpell)
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
        else return false;

    }

}

public class Unholy
{
    Int32 fail = 0;
    Int32 fail2 = 0;
    Int32 boltcount = 0;
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

    public Unholy()
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
            Thread.Sleep(30);
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
            else if (!Every_Man_for_Himself.KnownSpell)
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
        else return false;

    }

}

public class Frostdk { }

public class Frost
{
    Int32 fail = 0;
    Int32 fail2 = 0;
    Int32 boltcount = 0;
    #region InitializeSpell

    // Frost Only
    Spell Summon_Water_Elemental = new Spell("Summon Water Elemental");
    Spell Ice_Barrier = new Spell("Ice Barrier");
    Spell Deep_Freeze = new Spell("Deep Freeze");
    Spell Frostbolt = new Spell("Frostbolt");
    Spell Ice_Lance = new Spell("Ice Lance");
    Spell Icy_Veins = new Spell("Icy Veins");
    Spell Freeze = new Spell("Freeze");
    Spell Cone_of_Cold = new Spell("Cone_of_Cold ");
    Spell Improved_Cone_of_Cold = new Spell("Improved Cone of Cold");

    // Survive
    Spell Mana_Shield = new Spell("Mana Shield");
    Spell Mage_Ward = new Spell("Mage Ward");
    Spell Ring_of_Frost = new Spell("Ring of Frost");
    Spell Frost_Nova = new Spell("Frost Nova");
    Spell Blink = new Spell("Blink");
    Spell Counterspell = new Spell("Counterspell");

    // DPS
    Spell Fireball = new Spell("Fireball");
    Spell Frostfire_Bolt = new Spell("Frostfire Bolt");
    Spell Flame_Orb = new Spell("Flame Orb");
    Spell Fire_Blast = new Spell("Fire Blast");
    Spell Arcane_Missiles = new Spell("Arcane Missiles");

    // BUFF & HELPING
    Spell Evocation = new Spell("Evocation");
    Spell Conjure_Refreshment = new Spell("Conjure Refreshment");
    Spell Arcane_Brilliance = new Spell("Arcane Brilliance");
    Spell Remove_Curse = new Spell("Remove Curse");
    Spell Molten_Armor = new Spell("Molten Armor");

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

    public Frost()
    {
        Main.range = 35.0f;
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
            Thread.Sleep(30);
        }
    }

    public void pull()
    {
        if (hardmob()) Logging.WriteFight(" -  Pull Hard Mob - ");
        if (!hardmob()) Logging.WriteFight(" -  Pull Easy Mob - ");
        if (Ice_Barrier.KnownSpell && Ice_Barrier.IsSpellUsable && !Ice_Barrier.HaveBuff && hardmob())
        {
            SpellManager.CastSpellByIdLUA(11426);
            // Ice_Barrier.Launch();
        }
        pet();
        Lua.RunMacroText("/petattack");
        fighttimer = new Timer(60000);
    }

    public void buffoutfight()
    {
        pet();
        if (Fight.InFight) return;

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

        if (Molten_Armor.KnownSpell && Molten_Armor.IsSpellUsable && !Molten_Armor.HaveBuff)
        {
            Molten_Armor.Launch();
        }

    }

    public void fight()
    {
        pet();
        selfheal();
        buffinfight();
        if (ObjectManager.GetNumberAttackPlayer() > 1) fighttimer = new Timer(60000);

        if (Mirror_Image.KnownSpell && Mirror_Image.IsSpellUsable && hardmob() || ObjectManager.GetNumberAttackPlayer() > 1)
        {
            SpellManager.CastSpellByIdLUA(55342);
            // Mirror_Image.Launch();
        }

        if (Deep_Freeze.KnownSpell && Deep_Freeze.IsSpellUsable && Deep_Freeze.IsDistanceGood && ObjectManager.Me.HaveBuff(44544) &&
            ((!hardmob() && ObjectManager.Target.HealthPercent > 50) || hardmob()))
        {
            SpellManager.CastSpellByIdLUA(44572);
            // Deep_Freeze.Launch();
        }

        if (Berserking.KnownSpell && Berserking.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(1454);
            // Berserking.Launch();
        }

        if (ObjectManager.Me.HaveBuff(44544) &&
           Ice_Lance.KnownSpell &&
           Ice_Lance.IsSpellUsable &&
           Ice_Lance.IsDistanceGood)
        {
            SpellManager.CastSpellByIdLUA(30455);
            // Ice_Lance.Launch();	
            return;
        }

        if (ObjectManager.Me.HaveBuff(44544) || Frost_Nova.TargetHaveBuff || Freeze.TargetHaveBuff || Deep_Freeze.TargetHaveBuff || Improved_Cone_of_Cold.TargetHaveBuff)
        {
            if (Deep_Freeze.KnownSpell && Deep_Freeze.IsSpellUsable && Deep_Freeze.IsDistanceGood)
            {
                SpellManager.CastSpellByIdLUA(44572);
                // Deep_Freeze.Launch();
            }
            else if (Ice_Lance.KnownSpell && Ice_Lance.IsSpellUsable && Deep_Freeze.IsDistanceGood)
            {
                SpellManager.CastSpellByIdLUA(30455);
                // Ice_Lance.Launch();	
                return;
            }
        }

        if (ObjectManager.Me.HaveBuff(57761) &&
           Frostfire_Bolt.KnownSpell &&
           Frostfire_Bolt.IsSpellUsable &&
           Frostfire_Bolt.IsDistanceGood)
        {
            SpellManager.CastSpellByIdLUA(44614);
            // Frostfire_Bolt.Launch();
            return;
        }

        if (freeze.IsReady && Frostbolt.IsDistanceGood && Fight.InFight)
        {
            SpellManager.CastSpellByIDAndPosition(33395, ObjectManager.Target.Position);
            freeze = new Timer(1000 * 25);
            return;
        }

        if (ObjectManager.Me.HealthPercent < 10 &&
            Ice_Lance.KnownSpell &&
            Ice_Lance.IsSpellUsable &&
            Ice_Lance.IsDistanceGood)
        {
            SpellManager.CastSpellByIdLUA(30455);
            // Ice_Lance.Launch();	
            return;
        }

        if (Counterspell.KnownSpell && Counterspell.IsSpellUsable && Counterspell.IsDistanceGood &&
            ObjectManager.Target.IsCast && ObjectManager.Target.HealthPercent > 30)
        {
            SpellManager.CastSpellByIdLUA(2139);
            // Counterspell.Launch();
            return;
        }

        if (Arcane_Torrent.KnownSpell && Arcane_Torrent.IsSpellUsable &&
            ObjectManager.Target.IsCast && ObjectManager.Target.GetDistance < 8)
        {
            Arcane_Torrent.Launch();
        }

        if (Frost_Nova.KnownSpell && Frost_Nova.IsSpellUsable && ObjectManager.Target.GetDistance < 5)
        {
            SpellManager.CastSpellByIdLUA(122);
            // Frost_Nova.Launch();
        }

        if (Cone_of_Cold.KnownSpell && Cone_of_Cold.IsSpellUsable &&
            ObjectManager.Target.GetDistance < 8 &&
            ObjectManager.GetNumberAttackPlayer() > 1)
        {
            SpellManager.CastSpellByIdLUA(120);
            // Cone_of_Cold.Launch();
        }

        if (Flame_Orb.KnownSpell && Flame_Orb.IsDistanceGood && Flame_Orb.IsSpellUsable && hardmob())
        {
            SpellManager.CastSpellByIdLUA(82731);
            // Flame_Orb.Launch();
        }

        if (Arcane_Missiles.KnownSpell && Arcane_Missiles.IsDistanceGood && Arcane_Missiles.IsSpellUsable)
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

        if (Frostbolt.KnownSpell && Frostbolt.IsDistanceGood && Frostbolt.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(116);
            // Frostbolt.Launch();
        }

        if (ObjectManager.Target.GetDistance < 8 &&
            Fire_Blast.KnownSpell &&
            Fire_Blast.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(2136);
            // Fire_Blast.Launch();
            return;
        }

        if (!Frostbolt.KnownSpell && Fireball.IsSpellUsable && Fireball.IsDistanceGood)
        {
            SpellManager.CastSpellByIdLUA(133);
            // Fireball.Launch();
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

        if (Icy_Veins.KnownSpell && Icy_Veins.IsSpellUsable && hardmob() || ObjectManager.GetNumberAttackPlayer() > 2)
        {
            SpellManager.CastSpellByIdLUA(12472);
            // Icy_Veins.Launch();
        }

        if (Time_Warp.KnownSpell && Time_Warp.IsSpellUsable &&
            !Time_Warp.HaveBuff && hardmob() && ObjectManager.GetNumberAttackPlayer() > 2)
        {
            SpellManager.CastSpellByIdLUA(80353);
            // Time_Warp.Launch();
        }

        if (Blink.KnownSpell && Blink.IsSpellUsable && Fight.InFight &&
            ObjectManager.Target.GetDistance < 5 && ObjectManager.Target.HealthPercent > 30)
        {
            SpellManager.CastSpellByIdLUA(1953);
            // Blink.Launch();	
        }

        if (ObjectManager.Target.GetDistance > 55 && Blink.KnownSpell && Blink.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(1953);
            // Blink.Launch();	
        }

        if (Cold_Snap.KnownSpell && Cold_Snap.IsSpellUsable && !Icy_Veins.IsSpellUsable && !Deep_Freeze.IsSpellUsable && !Ice_Barrier.IsSpellUsable && ObjectManager.Me.HealthPercent < 50)
        {
            Cold_Snap.Launch();
        }

    }

    private void selfheal()
    {

        if (Ice_Barrier.KnownSpell && Ice_Barrier.IsSpellUsable && !Ice_Barrier.HaveBuff && Fight.InFight &&
            (ObjectManager.Me.HealthPercent < 45 || hardmob()))
        {
            SpellManager.CastSpellByIdLUA(11426);
            // Ice_Barrier.Launch();
        }

        if (Mage_Ward.KnownSpell && Mage_Ward.IsSpellUsable && ObjectManager.Target.IsCast &&
            ObjectManager.Target.IsTargetingMe && !Counterspell.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(543);
            // Mage_Ward.Launch();
        }

        if (!Mana_Shield.HaveBuff && ObjectManager.Me.HealthPercent < 35 &&
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

    private void pet()
    {

        if (ObjectManager.Me.IsMounted || !mountchill.IsReady) return;

        if ((ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0) &&
            !ObjectManager.Me.IsMounted && !ObjectManager.Me.IsDeadMe)
        {
            Logging.WriteFight(" - PET DEAD - ");
            if (Summon_Water_Elemental.KnownSpell &&
                Summon_Water_Elemental.IsSpellUsable)
            {
                Summon_Water_Elemental.Launch();
            }
        }

    }

    public bool hardmob()
    {

        if (((ObjectManager.Target.MaxHealth * 100) / ObjectManager.Me.MaxHealth) > 110)
        {
            return true;
        }
        else return false;

    }

}

public class Arcane
{
    Int32 fail = 0;
    Int32 fail2 = 0;
    Int32 boltcount = 0;
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

    public Arcane()
    {
        Main.range = 40.0f;
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
            Thread.Sleep(30);
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
        else return false;

    }

}

public class Fire
{
    Int32 fail = 0;
    Int32 fail2 = 0;
    Int32 boltcount = 0;
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

    public Fire()
    {
        Main.range = 40.0f;
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
            Thread.Sleep(30);
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
        else return false;

    }

}

public class Demo
{
    Int32 fail = 0;
    Int32 fail2 = 0;
    Int32 boltcount = 0;
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
        Main.range = 40.0f;
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
            Thread.Sleep(30);
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
        else return false;
    }

}

public class Affli
{
    Int32 fail = 0;
    Int32 fail2 = 0;
    Int32 boltcount = 0;
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
        Main.range = 40.0f;
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
            Thread.Sleep(30);
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
        else return false;
    }

}

public class Ele
{
    Int32 fail = 0;
    Int32 fail2 = 0;
    Int32 boltcount = 0;
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
        Main.range = 35.0f;
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
            Thread.Sleep(30);
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
        else return false;
    }

}

public class Balance
{
    Int32 firemode = 0;
    Int32 fail = 0;
    Int32 fail2 = 0;
    Int32 boltcount = 0;
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
        Main.range = 40.0f;
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
            Thread.Sleep(30);
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
        if (firemode == 0 || firemode == 1) return true;
        else return false;
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
        if (firemode == 2) return true;
        else return false;
    }

    public bool hardmob()
    {
        if (((ObjectManager.Target.MaxHealth * 100) / ObjectManager.Me.MaxHealth) > 110)
        {
            return true;
        }
        else return false;
    }

}

public class Survival
{
    Int32 fail = 0;
    Int32 fail2 = 0;
    Int32 boltcount = 0;
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
        Main.range = 40.0f;
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
            Thread.Sleep(30);
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
        else return false;

    }

}

public class Marks
{
    Int32 fail = 0;
    Int32 fail2 = 0;
    Int32 boltcount = 0;
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
        Main.range = 40.0f;
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
            Thread.Sleep(30);
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
        else return false;

    }

}


public class Shadow
{
    Int32 fail = 0;
    Int32 fail2 = 0;
    Int32 boltcount = 0;
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
        Main.range = 40f;
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
            Thread.Sleep(30);
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
        else return false;

    }

}

public class Arms
{
    Int32 fail = 0;
    Int32 fail2 = 0;
    Int32 boltcount = 0;
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
            Thread.Sleep(30);
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
        else return false;

    }

}