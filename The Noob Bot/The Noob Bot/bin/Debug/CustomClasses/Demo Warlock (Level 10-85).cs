using System;
using System.Threading;
using System.Windows.Forms;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;
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
            case WoWClass.Warlock:

                var Conflagrate = new Spell("Conflagrate");
                var Unstable_Affliction = new Spell("Unstable Affliction");
                var Summon_Felguard = new Spell("Summon Felguard");

                if (Conflagrate.KnownSpell)
                {
                    var ccWADES = new WarlockDest();
                }

                if (Unstable_Affliction.KnownSpell)
                {
                    var ccWAAFF = new WarlockAffli();
                }

                if (Summon_Felguard.KnownSpell)
                {
                    var ccWADEM = new WarlockDemo();
                }

                else
                {
                    var ccWA = new Warlock();
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

public class WarlockDest
{
    #region InitializeSpell

    private Spell Bane_of_Doom = new Spell("Bane of Doom");
    private Spell Burning_Embers = new Spell("Burning Embers");
    private Spell Chaos_Bolt = new Spell("Chaos Bolt");
    private Spell Conflagrate = new Spell("Conflagrate");
    private Spell Corruption = new Spell("Corruption");
    private Spell Create_Healthstone = new Spell("Create Healthstone");
    private Spell Curse_of_Guldan = new Spell("Curse of Gul'dan");
    private Spell Curse_of_Tongues = new Spell("Curse of Tongues");
    private Spell Curse_of_Weakness = new Spell("Curse of Weakness");
    private Spell Curse_of_the_Elements = new Spell("Curse of the Elements");
    private Spell Death_Coil = new Spell("Death Coil");
    private Spell Demon_Armor = new Spell("Demon Armor");
    private Spell Demon_Soul = new Spell("Demon Soul");
    private Spell Drain_Life = new Spell("Drain Life");
    private Spell Fear = new Spell("Fear");
    private Spell Fel_Armor = new Spell("Fel Armor");
    private Spell Fel_Domination = new Spell("Fel Domination");
    private Spell Hand_of_Guldan = new Spell("Hand of Gul'dan");
    private Spell Health_Funnel = new Spell("Health Funnel");
    private Spell Howl_of_Terror = new Spell("Howl of Terror");
    private Spell Immolate = new Spell("Immolate");
    private Spell Immolation_Aura = new Spell("Immolation Aura");
    private Spell Incinerate = new Spell("Incinerate");
    private Spell Life_Tap = new Spell("Life Tap");
    private Spell Metamorphosis = new Spell("Metamorphosis");
    private Spell Seed_of_Corruption = new Spell("Seed of Corruption");
    private Spell Shadow_Bolt = new Spell("Shadow Bolt");
    private Spell Shadowburn = new Spell("Shadowburn");
    private Spell Shadowflame = new Spell("Shadowflame");
    private Spell Shadowfury = new Spell("Shadowfury");
    private Spell Soul_Fire = new Spell("Soul Fire");
    private Timer Soul_Fire_Timer = new Timer(0);
    private Spell Soul_Harvest = new Spell("Soul Harvest");
    private Spell Summon_Doomguard = new Spell("Summon Doomguard");
    private Spell Summon_Felguard = new Spell("Summon Felguard");
    private Spell Summon_Felguardd = new Spell("Summon Felguard");
    private Spell Summon_Imp = new Spell("Summon Imp");
    private Spell Summon_Infernal = new Spell("Summon Infernal");
    private Spell Summon_Voidwalker = new Spell("Summon Voidwalker");

    #endregion InitializeSpell

    public WarlockDest()
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
                    if (ObjectManager.Me.Target != lastTarget && ObjectManager.Target.GetDistance <= Main.range)
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
        Pet();

        Lua.RunMacroText("/petattack");
        Logging.WriteFight("Launch Pet Attack");
    }

    public void Patrolling()
    {
        Healthstone();

        if (ObjectManager.Me.BarTwoPercentage < 50 &&
            Life_Tap.KnownSpell && Life_Tap.IsDistanceGood && Life_Tap.IsSpellUsable)
        {
            Life_Tap.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 65 &&
            Soul_Harvest.KnownSpell && Soul_Harvest.IsSpellUsable)
        {
            ObjectManager.Me.forceIsCast = true;


            if (ObjectManager.Me.GetMove) // If the player moves
            {
                MovementManager.StopMove(); // Stop moving
                Thread.Sleep(2000);
            }

            Soul_Harvest.Launch();
            Thread.Sleep(9000);

            ObjectManager.Me.forceIsCast = false;
            return;
        }
    }

    public void Combat()
    {
        Pet();
        Heal();
        BuffCombat();
        Pet_Health();
        ArmorCheck();

        if (ObjectManager.GetNumberAttackPlayer() > 2 &&
            !Seed_of_Corruption.TargetHaveBuff && Seed_of_Corruption.TargetHaveBuff && Seed_of_Corruption.KnownSpell &&
            Seed_of_Corruption.IsSpellUsable)
        {
            Seed_of_Corruption.Launch();
            return;
        }

        if (ObjectManager.GetNumberAttackPlayer() > 2 &&
            Shadowfury.TargetHaveBuff && Shadowfury.KnownSpell && Shadowfury.IsSpellUsable)
        {
            Shadowfury.Launch();
            return;
        }

        /*if (ObjectManager.Target.PVP &&
            ObjectManager.Target.Priest &&
            Curse_of_Weakness.KnownSpell &&
            Curse_of_Weakness.IsDistanceGood &&
            Curse_of_Weakness.IsSpellUsable)
        {
            Curse_of_Weakness.Launch();          // For ACC, waiting for the detection caster ACC
            return;
        }*/

        /*if (ObjectManager.Target.PVP &&
            Curse_of_Tongues.KnownSpell &&
            Curse_of_Tongues.IsDistanceGood &&
            Curse_of_Tongues.IsSpellUsable)
        {
            Curse_of_Tongues.Launch();          // For CASTER, waiting for the detection caster ACC
            return;
        }*/

        if (ObjectManager.Target.PVP &&
            !Fear.TargetHaveBuff && !Howl_of_Terror.TargetHaveBuff &&
            Death_Coil.KnownSpell && Death_Coil.IsDistanceGood && Death_Coil.IsSpellUsable)
        {
            Death_Coil.Launch();
            return;
        }

        if (ObjectManager.Target.PVP &&
            !Howl_of_Terror.TargetHaveBuff && !Death_Coil.TargetHaveBuff && !Fear.TargetHaveBuff &&
            Fear.KnownSpell && Fear.IsDistanceGood && Fear.IsSpellUsable)
        {
            Fear.Launch();
            return;
        }

        if (ObjectManager.Target.PVP &&
            ObjectManager.Target.GetDistance <= 10 &&
            !Death_Coil.TargetHaveBuff && !Fear.TargetHaveBuff &&
            Howl_of_Terror.KnownSpell && Howl_of_Terror.IsDistanceGood && Howl_of_Terror.IsSpellUsable)
        {
            Howl_of_Terror.Launch();
            return;
        }

        if (!Curse_of_the_Elements.TargetHaveBuff && Curse_of_the_Elements.KnownSpell &&
            Curse_of_the_Elements.IsSpellUsable)
        {
            Curse_of_the_Elements.Launch();
            return;
        }

        if (Soul_Fire_Timer.IsReady && Soul_Fire.KnownSpell && Soul_Fire.IsDistanceGood && Soul_Fire.IsSpellUsable)
        {
            Soul_Fire.Launch();
            Soul_Fire_Timer = new Timer(1000*5);
            return;
        }

        if (!Immolate.TargetHaveBuff && Immolate.KnownSpell && Immolate.IsDistanceGood && Immolate.IsSpellUsable)
        {
            Immolate.Launch();
            return;
        }

        if (Conflagrate.KnownSpell && Conflagrate.IsDistanceGood && Conflagrate.IsSpellUsable)
        {
            Conflagrate.Launch();
            return;
        }

        if (!Bane_of_Doom.TargetHaveBuff && Bane_of_Doom.KnownSpell && Bane_of_Doom.IsDistanceGood &&
            Bane_of_Doom.IsSpellUsable)
        {
            Bane_of_Doom.Launch();
            return;
        }

        if (!Corruption.TargetHaveBuff && Corruption.KnownSpell && Corruption.IsDistanceGood && Corruption.IsSpellUsable)
        {
            Corruption.Launch();
            return;
        }

        if (Shadowflame.KnownSpell && Shadowflame.IsDistanceGood && Shadowflame.IsSpellUsable)
        {
            Shadowflame.Launch();
            return;
        }

        if (Chaos_Bolt.KnownSpell && Chaos_Bolt.IsDistanceGood && Chaos_Bolt.IsSpellUsable)
        {
            Chaos_Bolt.Launch();
            return;
        }

        if (Shadowburn.KnownSpell && Shadowburn.IsDistanceGood && Shadowburn.IsSpellUsable)
        {
            Shadowburn.Launch();
            return;
        }

        if (Incinerate.KnownSpell && Incinerate.IsDistanceGood && Incinerate.IsSpellUsable)
        {
            Incinerate.Launch();
            return;
        }

        if (ObjectManager.Me.BarTwoPercentage < 10)
        {
            Lua.RunMacroText("/cast shoot");
            return;
        }
    }

    private void BuffCombat()
    {
        if (Demon_Soul.KnownSpell && Demon_Soul.IsDistanceGood && Demon_Soul.IsSpellUsable)
        {
            Demon_Soul.Launch();
            return;
        }

        if (Summon_Infernal.KnownSpell && Summon_Infernal.IsDistanceGood && Summon_Infernal.IsSpellUsable)
        {
            SpellManager.CastSpellByIDAndPosition(1122, ObjectManager.Target.Position);
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.HealthPercent < 35 &&
            Drain_Life.KnownSpell && Drain_Life.IsDistanceGood && Drain_Life.IsSpellUsable)
        {
            Drain_Life.Launch();
            return;
        }

        if (ObjectManager.Me.BarTwoPercentage < 25 &&
            Life_Tap.KnownSpell && Life_Tap.IsDistanceGood && Life_Tap.IsSpellUsable)
        {
            Life_Tap.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 20 &&
            ItemsManager.GetItemCountByIdLUA(5512) == 1)
        {
            Lua.RunMacroText("/use item:5512");
            Logging.WriteFight("Use Healthstone");
            return;
        }
    }

    private void Healthstone()
    {
        if (!Fight.InFight &&
            ItemsManager.GetItemCountByIdLUA(5512) == 0 &&
            Create_Healthstone.KnownSpell)
        {
            Create_Healthstone.Launch();
            return;
        }
    }

    private void ArmorCheck()
    {
        if (Fel_Armor.KnownSpell &&
            !Fel_Armor.HaveBuff &&
            Fel_Armor.IsSpellUsable)
        {
            Fel_Armor.Launch();
        }
        else if (Demon_Armor.KnownSpell &&
                 !Fel_Armor.KnownSpell &&
                 !Demon_Armor.HaveBuff &&
                 Demon_Armor.IsSpellUsable)
        {
            Demon_Armor.Launch();
        }
    }

    private void Pet()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if ((ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0) &&
            !ObjectManager.Me.IsMounted && !ObjectManager.Me.IsDeadMe)
        {
            if (Summon_Imp.KnownSpell)
            {
                Summon_Imp.Launch();
                Thread.Sleep(1000);
            }
        }
    }

    private void Pet_Health()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (Health_Funnel.KnownSpell)
            if (ObjectManager.Pet.HealthPercent > 0 &&
                ObjectManager.Pet.HealthPercent < 50 &&
                Health_Funnel.IsSpellUsable)
            {
                Health_Funnel.Launch();
                while (ObjectManager.Me.IsCast)
                {
                    if (ObjectManager.Pet.HealthPercent > 80 || ObjectManager.Pet.IsDead)
                        break;

                    Thread.Sleep(100);
                }
            }
    }
}

public class WarlockDemo
{
    #region InitializeSpell

    private Spell Bane_of_Doom = new Spell("Bane of Doom");
    private Spell Burning_Embers = new Spell("Burning Embers");
    private Spell Chaos_Bolt = new Spell("Chaos Bolt");
    private Spell Conflagrate = new Spell("Conflagrate");
    private Spell Corruption = new Spell("Corruption");
    private Spell Create_Healthstone = new Spell("Create Healthstone");
    private Spell Curse_of_Guldan = new Spell("Curse of Gul'dan");
    private Spell Curse_of_Tongues = new Spell("Curse of Tongues");
    private Spell Curse_of_Weakness = new Spell("Curse of Weakness");
    private Spell Curse_of_the_Elements = new Spell("Curse of the Elements");
    private Spell Death_Coil = new Spell("Death Coil");
    private Spell Demon_Armor = new Spell("Demon Armor");
    private Spell Demon_Soul = new Spell("Demon Soul");
    private Spell Drain_Life = new Spell("Drain Life");
    private Spell Drain_Soul = new Spell("Drain Soul");
    private Spell Fear = new Spell("Fear");
    private Spell Fel_Armor = new Spell("Fel Armor");
    private Spell Fel_Domination = new Spell("Fel Domination");
    private Spell Hand_of_Guldan = new Spell("Hand of Gul'dan");
    private Spell Health_Funnel = new Spell("Health Funnel");
    private Spell Howl_of_Terror = new Spell("Howl of Terror");
    private Spell Immolate = new Spell("Immolate");
    private Spell Immolation_Aura = new Spell("Immolation Aura");
    private Spell Incinerate = new Spell("Incinerate");
    private Spell Life_Tap = new Spell("Life Tap");
    private Spell Metamorphosis = new Spell("Metamorphosis");
    private Spell Molten_Core = new Spell("Molten Core");
    private Spell Seed_of_Corruption = new Spell("Seed of Corruption");
    private Spell Shadow_Bolt = new Spell("Shadow Bolt");
    private Spell Shadowburn = new Spell("Shadowburn");
    private Spell Shadowflame = new Spell("Shadowflame");
    private Spell Shadowfury = new Spell("Shadowfury");
    private Spell Soul_Fire = new Spell("Soul Fire");
    private Timer Soul_Fire_Timer = new Timer(0);
    private Spell Soul_Harvest = new Spell("Soul Harvest");
    private Spell Summon_Doomguard = new Spell("Summon Doomguard");
    private Spell Summon_Felguard = new Spell("Summon Felguard");
    private Spell Summon_Felguardd = new Spell("Summon Felguard");
    private Spell Summon_Imp = new Spell("Summon Felguard");
    private Spell Summon_Infernal = new Spell("Summon Infernal");
    private Spell Summon_Voidwalker = new Spell("Summon Felguard");

    #endregion InitializeSpell

    public WarlockDemo()
    {
        Main.range = 23.0f; // Range

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
            Thread.Sleep(20);
        }
    }

    public void Pull()
    {
        Pet();

        Lua.RunMacroText("/petattack");
        Lua.RunMacroText("/cast axe toss");
        Logging.WriteFight("Launch Pet Attack");
    }

    public void Patrolling()
    {
        Healthstone();

        if (ObjectManager.Me.BarTwoPercentage < 50 &&
            Life_Tap.KnownSpell &&
            Life_Tap.IsDistanceGood &&
            Life_Tap.IsSpellUsable)
        {
            Life_Tap.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 65 &&
            Soul_Harvest.KnownSpell && Soul_Harvest.IsSpellUsable)
        {
            ObjectManager.Me.forceIsCast = true;


            if (ObjectManager.Me.GetMove) // If the player moves
            {
                MovementManager.StopMove(); // Stop moving
                Thread.Sleep(2000);
            }

            Soul_Harvest.Launch();
            Thread.Sleep(9000);

            ObjectManager.Me.forceIsCast = false;
            return;
        }
    }

    public void Combat()
    {
        Pet();
        Lua.RunMacroText("/cast Pursuit");
        Lua.RunMacroText("/cast Felstorm");
        Heal();
        BuffCombat();
        Pet_Health();
        ArmorCheck();

        if (ObjectManager.GetNumberAttackPlayer() > 2 &&
            !Seed_of_Corruption.TargetHaveBuff &&
            Seed_of_Corruption.TargetHaveBuff &&
            Seed_of_Corruption.KnownSpell &&
            Seed_of_Corruption.IsSpellUsable)
        {
            Seed_of_Corruption.Launch();
            return;
        }

        if (ObjectManager.GetNumberAttackPlayer() > 2 &&
            Shadowfury.TargetHaveBuff &&
            Shadowfury.KnownSpell &&
            Shadowfury.IsSpellUsable)
        {
            Shadowfury.Launch();
            return;
        }

        if (ObjectManager.Target.HealthPercent <= 40 &&
            !Drain_Soul.TargetHaveBuff &&
            Drain_Soul.KnownSpell &&
            Drain_Soul.IsDistanceGood &&
            Drain_Soul.IsSpellUsable)
        {
            Drain_Soul.Launch();
            return;
        }

        /*if (ObjectManager.Target.PVP &&
            ObjectManager.Target.Priest &&
            Curse_of_Weakness.KnownSpell &&
            Curse_of_Weakness.IsDistanceGood &&
            Curse_of_Weakness.IsSpellUsable)
        {
            Curse_of_Weakness.Launch();          // For ACC, waiting for the detection caster ACC
            return;
        }*/

        /*if (ObjectManager.Target.PVP &&
            Curse_of_Tongues.KnownSpell &&
            Curse_of_Tongues.IsDistanceGood &&
            Curse_of_Tongues.IsSpellUsable)
        {
            Curse_of_Tongues.Launch();          // For CASTER, waiting for the detection caster/ACC
            return;
        }*/

        if (ObjectManager.Target.PVP &&
            !Fear.TargetHaveBuff &&
            !Howl_of_Terror.TargetHaveBuff &&
            Death_Coil.KnownSpell &&
            Death_Coil.IsDistanceGood &&
            Death_Coil.IsSpellUsable)
        {
            Death_Coil.Launch();
            return;
        }

        if (ObjectManager.Target.PVP &&
            !Howl_of_Terror.TargetHaveBuff &&
            !Death_Coil.TargetHaveBuff &&
            !Fear.TargetHaveBuff &&
            Fear.KnownSpell &&
            Fear.IsDistanceGood &&
            Fear.IsSpellUsable)
        {
            Fear.Launch();
            return;
        }

        if (ObjectManager.Target.PVP &&
            ObjectManager.Target.GetDistance <= 10 &&
            !Death_Coil.TargetHaveBuff &&
            !Fear.TargetHaveBuff &&
            Howl_of_Terror.KnownSpell &&
            Howl_of_Terror.IsDistanceGood &&
            Howl_of_Terror.IsSpellUsable)
        {
            Howl_of_Terror.Launch();
            return;
        }

        if (!Curse_of_the_Elements.TargetHaveBuff &&
            Curse_of_the_Elements.KnownSpell &&
            Curse_of_the_Elements.IsSpellUsable)
        {
            Curse_of_the_Elements.Launch();
            return;
        }

        if (ObjectManager.Me.HaveBuff(71162) &&
            Incinerate.KnownSpell &&
            Incinerate.IsDistanceGood &&
            Incinerate.IsSpellUsable)
        {
            Incinerate.Launch();
            return;
        }

        if (!Bane_of_Doom.TargetHaveBuff &&
            Bane_of_Doom.KnownSpell &&
            Bane_of_Doom.IsDistanceGood &&
            Bane_of_Doom.IsSpellUsable)
        {
            Bane_of_Doom.Launch();
            return;
        }

        if (!Curse_of_Guldan.TargetHaveBuff &&
            Hand_of_Guldan.KnownSpell &&
            Hand_of_Guldan.IsDistanceGood &&
            Hand_of_Guldan.IsSpellUsable)
        {
            Hand_of_Guldan.Launch();
            return;
        }

        if (!Corruption.TargetHaveBuff &&
            Corruption.KnownSpell &&
            Corruption.IsDistanceGood &&
            Corruption.IsSpellUsable)
        {
            Corruption.Launch();
            return;
        }

        if (!Immolate.TargetHaveBuff &&
            Immolate.KnownSpell &&
            Immolate.IsDistanceGood &&
            Immolate.IsSpellUsable)
        {
            Immolate.Launch();
            return;
        }

        if (Shadow_Bolt.KnownSpell &&
            Shadow_Bolt.IsDistanceGood &&
            Shadow_Bolt.IsSpellUsable)
        {
            Shadow_Bolt.Launch();
            return;
        }

        if (ObjectManager.Me.BarTwoPercentage < 10)
        {
            Lua.RunMacroText("/cast shoot");
            return;
        }
    }

    private void BuffCombat()
    {
        if (Summon_Infernal.KnownSpell && Summon_Infernal.IsDistanceGood && Summon_Infernal.IsSpellUsable)
        {
            SpellManager.CastSpellByIDAndPosition(1122, ObjectManager.Target.Position);
        }

        if (Metamorphosis.KnownSpell && Metamorphosis.IsSpellUsable)
        {
            Metamorphosis.Launch();
        }

        if (!Metamorphosis.HaveBuff && Demon_Soul.KnownSpell && Demon_Soul.IsSpellUsable)
        {
            Demon_Soul.Launch();
        }

        if (ObjectManager.GetNumberAttackPlayer() > 2 &&
            Immolation_Aura.KnownSpell && Immolation_Aura.IsSpellUsable)
        {
            Immolation_Aura.Launch();
        }

        if (Summon_Doomguard.KnownSpell && Summon_Doomguard.IsSpellUsable)
        {
            Summon_Doomguard.Launch();
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.HealthPercent < 35 &&
            Drain_Life.KnownSpell && Drain_Life.IsDistanceGood && Drain_Life.IsSpellUsable)
        {
            Lua.RunMacroText("/use Soulburn");
            Drain_Life.Launch();
            return;
        }

        if (ObjectManager.Me.BarTwoPercentage < 25 &&
            Life_Tap.KnownSpell && Life_Tap.IsDistanceGood && Life_Tap.IsSpellUsable)
        {
            Life_Tap.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 33 &&
            ItemsManager.GetItemCountByIdLUA(5512) == 1)
        {
            Lua.RunMacroText("/use item:5512");
            Logging.WriteFight("Use Healthstone");
            return;
        }
    }

    private void Healthstone()
    {
        if (!Fight.InFight &&
            ItemsManager.GetItemCountByIdLUA(5512) == 0 &&
            Create_Healthstone.KnownSpell)
        {
            Create_Healthstone.Launch();
            return;
        }
    }

    private void ArmorCheck()
    {
        if (Fel_Armor.KnownSpell && !Fel_Armor.HaveBuff && Fel_Armor.IsSpellUsable)
        {
            Fel_Armor.Launch();
        }
        else if (Demon_Armor.KnownSpell && !Fel_Armor.KnownSpell && !Demon_Armor.HaveBuff && Demon_Armor.IsSpellUsable)
        {
            Demon_Armor.Launch();
        }
    }

    private void Pet()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if ((ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0) &&
            !ObjectManager.Me.IsMounted && !ObjectManager.Me.IsDeadMe)
        {
            if (Summon_Felguard.KnownSpell)
            {
                Lua.RunMacroText("/use Soulburn");
                Summon_Felguard.Launch();
            }
        }
    }

    private void Pet_Health()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (Health_Funnel.KnownSpell)
            if (ObjectManager.Pet.HealthPercent > 0 && ObjectManager.Pet.HealthPercent < 50 &&
                Health_Funnel.IsSpellUsable)
            {
                Health_Funnel.Launch();
                while (ObjectManager.Me.IsCast)
                {
                    if (ObjectManager.Pet.HealthPercent > 80 || ObjectManager.Pet.IsDead)
                        break;

                    Thread.Sleep(100);
                }
            }
    }
}

public class WarlockAffli
{
    #region InitializeSpell

    private Spell Bane_of_Agony = new Spell("Bane of Agony");
    private Spell Bane_of_Doom = new Spell("Bane of Doom");
    private Spell Burning_Embers = new Spell("Burning Embers");
    private Spell Chaos_Bolt = new Spell("Chaos Bolt");
    private Spell Conflagrate = new Spell("Conflagrate");
    private Spell Corruption = new Spell("Corruption");
    private Spell Create_Healthstone = new Spell("Create Healthstone");
    private Spell Curse_of_Guldan = new Spell("Curse of Gul'dan");
    private Spell Curse_of_Tongues = new Spell("Curse of Tongues");
    private Spell Curse_of_Weakness = new Spell("Curse of Weakness");
    private Spell Curse_of_the_Elements = new Spell("Curse of the Elements");
    private Spell Death_Coil = new Spell("Death Coil");
    private Spell Demon_Armor = new Spell("Demon Armor");

    private Spell Demon_Soul = new Spell("Demon Soul");
    private Spell Drain_Life = new Spell("Drain Life");
    private Spell Drain_Soul = new Spell("Drain Soul");
    private Spell Fear = new Spell("Fear");
    private Spell Fel_Armor = new Spell("Fel Armor");
    private Spell Fel_Domination = new Spell("Fel Domination");
    private Spell Hand_of_Guldan = new Spell("Hand of Gul'dan");
    private Spell Haunt = new Spell("Haunt");
    private Timer Haunt_Timer = new Timer(0);
    private Spell Health_Funnel = new Spell("Health Funnel");
    private Spell Howl_of_Terror = new Spell("Howl of Terror");
    private Spell Immolate = new Spell("Immolate");
    private Spell Immolation_Aura = new Spell("Immolation Aura");
    private Spell Incinerate = new Spell("Incinerate");
    private Spell Life_Tap = new Spell("Life Tap");
    private Spell Metamorphosis = new Spell("Metamorphosis");
    private Spell Molten_Core = new Spell("Molten Core");
    private Spell Seed_of_Corruption = new Spell("Seed of Corruption");
    private Spell Shadow_Bolt = new Spell("Shadow Bolt");
    private Spell Shadow_Trance = new Spell("Shadow Trance");
    private Spell Shadowburn = new Spell("Shadowburn");
    private Spell Shadowflame = new Spell("Shadowflame");
    private Spell Shadowfury = new Spell("Shadowfury");
    private Spell Soul_Fire = new Spell("Soul Fire");
    private Spell Soul_Harvest = new Spell("Soul Harvest");
    private Spell Summon_Doomguard = new Spell("Summon Doomguard");
    private Spell Summon_Felguard = new Spell("Summon Felguard");
    private Spell Summon_Felguardd = new Spell("Summon Felguard");
    private Spell Summon_Felhunter = new Spell("Summon Felhunter");
    private Spell Summon_Imp = new Spell("Summon Imp");
    private Spell Summon_Infernal = new Spell("Summon Infernal");
    private Spell Summon_Voidwalker = new Spell("Summon Voidwalker");
    private Spell Unstable_Affliction = new Spell("Unstable Affliction");

    #endregion InitializeSpell

    public WarlockAffli()
    {
        Main.range = 23.0f; // Range

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
            Thread.Sleep(20);
        }
    }

    public void Pull()
    {
        Pet();

        Lua.RunMacroText("/petattack");
        Logging.WriteFight("Launch Pet Attack");
    }

    public void Patrolling()
    {
        Healthstone();

        if (ObjectManager.Me.BarTwoPercentage < 50 &&
            Life_Tap.KnownSpell &&
            Life_Tap.IsDistanceGood &&
            Life_Tap.IsSpellUsable)
        {
            Life_Tap.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 65 &&
            Soul_Harvest.KnownSpell && Soul_Harvest.IsSpellUsable)
        {
            ObjectManager.Me.forceIsCast = true;


            if (ObjectManager.Me.GetMove) // If the player moves
            {
                MovementManager.StopMove(); // Stop moving
                Thread.Sleep(2000);
            }

            Soul_Harvest.Launch();
            Thread.Sleep(9000);

            ObjectManager.Me.forceIsCast = false;
            return;
        }
    }

    public void Combat()
    {
        Pet();
        Heal();
        BuffCombat();
        Pet_Health();
        ArmorCheck();

        if (ObjectManager.GetNumberAttackPlayer() > 2 &&
            !Seed_of_Corruption.TargetHaveBuff && Seed_of_Corruption.TargetHaveBuff && Seed_of_Corruption.KnownSpell &&
            Seed_of_Corruption.IsSpellUsable)
        {
            Seed_of_Corruption.Launch();
            return;
        }

        if (ObjectManager.GetNumberAttackPlayer() > 2 &&
            Shadowfury.TargetHaveBuff && Shadowfury.KnownSpell && Shadowfury.IsSpellUsable)
        {
            Shadowfury.Launch();
            return;
        }

        /*if (ObjectManager.Target.PVP &&
            ObjectManager.Target.Priest &&
            Curse_of_Weakness.KnownSpell &&
            Curse_of_Weakness.IsDistanceGood &&
            Curse_of_Weakness.IsSpellUsable)
        {
            Curse_of_Weakness.Launch();          // For ACC, waiting for the detection caster ACC
            return;
        }*/

        /*if (ObjectManager.Target.PVP &&
            Curse_of_Tongues.KnownSpell &&
            Curse_of_Tongues.IsDistanceGood &&
            Curse_of_Tongues.IsSpellUsable)
        {
            Curse_of_Tongues.Launch();          // For CASTER, waiting for the detection caster/ACC
            return;
        }*/

        if (ObjectManager.Target.PVP &&
            !Fear.TargetHaveBuff && !Howl_of_Terror.TargetHaveBuff &&
            Death_Coil.KnownSpell && Death_Coil.IsDistanceGood && Death_Coil.IsSpellUsable)
        {
            Death_Coil.Launch();
            return;
        }

        if (ObjectManager.Target.PVP &&
            !Howl_of_Terror.TargetHaveBuff && !Death_Coil.TargetHaveBuff && !Fear.TargetHaveBuff &&
            Fear.KnownSpell && Fear.IsDistanceGood && Fear.IsSpellUsable)
        {
            Fear.Launch();
            return;
        }

        if (ObjectManager.Target.PVP &&
            ObjectManager.Target.GetDistance <= 10 &&
            !Death_Coil.TargetHaveBuff && !Fear.TargetHaveBuff &&
            Howl_of_Terror.KnownSpell && Howl_of_Terror.IsDistanceGood && Howl_of_Terror.IsSpellUsable)
        {
            Howl_of_Terror.Launch();
            return;
        }

        if (ObjectManager.Me.HaveBuff(17941) &&
            Shadow_Bolt.KnownSpell && Shadow_Bolt.IsDistanceGood && Shadow_Bolt.IsSpellUsable)
        {
            Shadow_Bolt.Launch();
            return;
        }

        if (Haunt_Timer.IsReady && Haunt.KnownSpell && Haunt.IsDistanceGood && Haunt.IsSpellUsable)
        {
            Haunt.Launch();
            Haunt_Timer = new Timer(1000*10);
            return;
        }

        if (!Bane_of_Agony.TargetHaveBuff && Bane_of_Agony.KnownSpell && Bane_of_Agony.IsDistanceGood &&
            Bane_of_Agony.IsSpellUsable)
        {
            Bane_of_Agony.Launch();
            return;
        }

        if (!Corruption.TargetHaveBuff && Corruption.KnownSpell && Corruption.IsDistanceGood && Corruption.IsSpellUsable)
        {
            Corruption.Launch();
            return;
        }

        if (!Unstable_Affliction.TargetHaveBuff && Unstable_Affliction.KnownSpell && Unstable_Affliction.IsDistanceGood &&
            Unstable_Affliction.IsSpellUsable)
        {
            Unstable_Affliction.Launch();
            return;
        }

        if (ObjectManager.Target.HealthPercent <= 25 &&
            !Drain_Soul.TargetHaveBuff && Drain_Soul.KnownSpell && Drain_Soul.IsDistanceGood && Drain_Soul.IsSpellUsable)
        {
            Drain_Soul.Launch();
            return;
        }

        if (Demon_Soul.KnownSpell && Demon_Soul.IsSpellUsable)
        {
            Demon_Soul.Launch();
            return;
        }

        if (Drain_Life.KnownSpell && Drain_Life.IsDistanceGood && Drain_Life.IsSpellUsable)
        {
            Drain_Life.Launch();
            return;
        }

        if (ObjectManager.Me.BarTwoPercentage < 10)
        {
            Lua.RunMacroText("/cast shoot");
            return;
        }
    }

    private void BuffCombat()
    {
        if (Summon_Infernal.KnownSpell && Summon_Infernal.IsDistanceGood && Summon_Infernal.IsSpellUsable)
        {
            SpellManager.CastSpellByIDAndPosition(1122, ObjectManager.Target.Position);
        }

        if (Metamorphosis.KnownSpell && Metamorphosis.IsSpellUsable)
        {
            Metamorphosis.Launch();
        }

        if (!Metamorphosis.HaveBuff && Demon_Soul.KnownSpell && Demon_Soul.IsSpellUsable)
        {
            Demon_Soul.Launch();
        }

        if (ObjectManager.GetNumberAttackPlayer() > 2 &&
            Immolation_Aura.KnownSpell && Immolation_Aura.IsSpellUsable)
        {
            Immolation_Aura.Launch();
        }

        if (Summon_Doomguard.KnownSpell && Summon_Doomguard.IsSpellUsable)
        {
            Summon_Doomguard.Launch();
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.HealthPercent < 35 &&
            Drain_Life.KnownSpell && Drain_Life.IsDistanceGood && Drain_Life.IsSpellUsable)
        {
            Drain_Life.Launch();
            return;
        }

        if (ObjectManager.Me.BarTwoPercentage < 25 &&
            Life_Tap.KnownSpell && Life_Tap.IsDistanceGood && Life_Tap.IsSpellUsable)
        {
            Life_Tap.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 20 &&
            ItemsManager.GetItemCountByIdLUA(5512) == 1)
        {
            Lua.RunMacroText("/use item:5512");
            Logging.WriteFight("Use Healthstone");
            return;
        }
    }

    private void Healthstone()
    {
        if (!Fight.InFight &&
            ItemsManager.GetItemCountByIdLUA(5512) == 0 &&
            Create_Healthstone.KnownSpell)
        {
            Create_Healthstone.Launch();
            return;
        }
    }

    private void ArmorCheck()
    {
        if (Fel_Armor.KnownSpell && !Fel_Armor.HaveBuff && Fel_Armor.IsSpellUsable)
        {
            Fel_Armor.Launch();
        }
        else if (Demon_Armor.KnownSpell && !Fel_Armor.KnownSpell && !Demon_Armor.HaveBuff && Demon_Armor.IsSpellUsable)
        {
            Demon_Armor.Launch();
        }
    }

    private void Pet()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if ((ObjectManager.Pet.Health == 0 ||
             ObjectManager.Pet.Guid == 0) &&
            !ObjectManager.Me.IsMounted && !ObjectManager.Me.IsDeadMe)
        {
            if (Summon_Felhunter.KnownSpell)
            {
                Summon_Felhunter.Launch();
            }
            else if (!Summon_Felhunter.KnownSpell && Summon_Voidwalker.KnownSpell)
            {
                Summon_Voidwalker.Launch();
            }

            else if (!Summon_Felhunter.KnownSpell && !Summon_Voidwalker.KnownSpell && Summon_Imp.KnownSpell)
            {
                Summon_Imp.Launch();
            }
        }
    }

    private void Pet_Health()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (Health_Funnel.KnownSpell)
            if (ObjectManager.Pet.HealthPercent > 0 && ObjectManager.Pet.HealthPercent < 50 &&
                Health_Funnel.IsSpellUsable)
            {
                Health_Funnel.Launch();
                while (ObjectManager.Me.IsCast)
                {
                    if (ObjectManager.Pet.HealthPercent > 80 || ObjectManager.Pet.IsDead)
                        break;

                    Thread.Sleep(100);
                }
            }
    }
}

public class Warlock
{
    #region InitializeSpell

    private Spell Bane_of_Agony = new Spell("Bane of Agony");
    private Spell Bane_of_Doom = new Spell("Bane of Doom");
    private Spell Burning_Embers = new Spell("Burning Embers");
    private Spell Chaos_Bolt = new Spell("Chaos Bolt");
    private Spell Conflagrate = new Spell("Conflagrate");
    private Spell Corruption = new Spell("Corruption");
    private Spell Create_Healthstone = new Spell("Create Healthstone");
    private Spell Curse_of_Guldan = new Spell("Curse of Gul'dan");
    private Spell Curse_of_Tongues = new Spell("Curse of Tongues");
    private Spell Curse_of_Weakness = new Spell("Curse of Weakness");
    private Spell Curse_of_the_Elements = new Spell("Curse of the Elements");
    private Spell Death_Coil = new Spell("Death Coil");
    private Spell Demon_Armor = new Spell("Demon Armor");

    private Spell Demon_Soul = new Spell("Demon Soul");
    private Spell Drain_Life = new Spell("Drain Life");
    private Spell Drain_Soul = new Spell("Drain Soul");
    private Spell Fear = new Spell("Fear");
    private Spell Fel_Armor = new Spell("Fel Armor");
    private Spell Fel_Domination = new Spell("Fel Domination");
    private Spell Hand_of_Guldan = new Spell("Hand of Gul'dan");
    private Spell Haunt = new Spell("Haunt");
    private Timer Haunt_Timer = new Timer(0);
    private Spell Health_Funnel = new Spell("Health Funnel");
    private Spell Howl_of_Terror = new Spell("Howl of Terror");
    private Spell Immolate = new Spell("Immolate");
    private Spell Immolation_Aura = new Spell("Immolation Aura");
    private Spell Incinerate = new Spell("Incinerate");
    private Spell Life_Tap = new Spell("Life Tap");
    private Spell Metamorphosis = new Spell("Metamorphosis");
    private Spell Molten_Core = new Spell("Molten Core");
    private Spell Seed_of_Corruption = new Spell("Seed of Corruption");
    private Spell Shadow_Bolt = new Spell("Shadow Bolt");
    private Spell Shadow_Trance = new Spell("Shadow Trance");
    private Spell Shadowburn = new Spell("Shadowburn");
    private Spell Shadowflame = new Spell("Shadowflame");
    private Spell Shadowfury = new Spell("Shadowfury");
    private Spell Soul_Fire = new Spell("Soul Fire");
    private Spell Soul_Harvest = new Spell("Soul Harvest");
    private Spell Summon_Doomguard = new Spell("Summon Doomguard");
    private Spell Summon_Felguard = new Spell("Summon Felguard");
    private Spell Summon_Felguardd = new Spell("Summon Felguard");
    private Spell Summon_Felhunter = new Spell("Summon Felhunter");
    private Spell Summon_Imp = new Spell("Summon Imp");
    private Spell Summon_Infernal = new Spell("Summon Infernal");
    private Spell Summon_Voidwalker = new Spell("Summon Voidwalker");
    private Spell Unstable_Affliction = new Spell("Unstable Affliction");

    #endregion InitializeSpell

    public Warlock()
    {
        Main.range = 23.0f; // Range

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
            Thread.Sleep(20);
        }
    }

    public void Pull()
    {
    }

    public void Patrolling()
    {
        Pet();
        Healthstone();

        if (ObjectManager.Me.BarTwoPercentage < 50 &&
            Life_Tap.KnownSpell &&
            Life_Tap.IsDistanceGood &&
            Life_Tap.IsSpellUsable)
        {
            Life_Tap.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 65 &&
            Soul_Harvest.KnownSpell && Soul_Harvest.IsSpellUsable)
        {
            Soul_Harvest.Launch();
            return;
        }
    }

    public void Combat()
    {
        Heal();
        BuffCombat();
        Pet_Health();
        ArmorCheck();

        if (!Corruption.TargetHaveBuff &&
            Corruption.KnownSpell &&
            Corruption.IsDistanceGood &&
            Corruption.IsSpellUsable)
        {
            Corruption.Launch();
            return;
        }

        if (!Immolate.TargetHaveBuff &&
            Immolate.KnownSpell &&
            Immolate.IsDistanceGood &&
            Immolate.IsSpellUsable)
        {
            Immolate.Launch();
            return;
        }

        if (Shadow_Bolt.KnownSpell &&
            Shadow_Bolt.IsDistanceGood &&
            Shadow_Bolt.IsSpellUsable)
        {
            Shadow_Bolt.Launch();
            return;
        }

        if (Drain_Life.KnownSpell &&
            Drain_Life.IsDistanceGood &&
            Drain_Life.IsSpellUsable)
        {
            Drain_Life.Launch();
            return;
        }

        if (ObjectManager.Target.HealthPercent <= 25 &&
            !Drain_Soul.TargetHaveBuff &&
            Drain_Soul.KnownSpell &&
            Drain_Soul.IsDistanceGood &&
            Drain_Soul.IsSpellUsable)
        {
            Drain_Soul.Launch();
            return;
        }

        if (ObjectManager.Me.BarTwoPercentage < 10)
        {
            Lua.RunMacroText("/cast shoot");
            return;
        }
    }

    private void BuffCombat()
    {
        /*if (Summon_Infernal.KnownSpell &&
            Summon_Infernal.IsDistanceGood && // As for DnD of DK
            Summon_Infernal.IsSpellUsable)
        {
            Summon_Infernal.Launch();
            return;
        }*/

        if (Metamorphosis.KnownSpell &&
            Metamorphosis.IsSpellUsable)
        {
            Metamorphosis.Launch();
        }

        if (!Metamorphosis.HaveBuff &&
            Demon_Soul.KnownSpell &&
            Demon_Soul.IsSpellUsable)
        {
            Demon_Soul.Launch();
        }

        if (ObjectManager.GetNumberAttackPlayer() > 2 &&
            Immolation_Aura.KnownSpell &&
            Immolation_Aura.IsSpellUsable)
        {
            Immolation_Aura.Launch();
        }

        if (Summon_Doomguard.KnownSpell &&
            Summon_Doomguard.IsSpellUsable)
        {
            Summon_Doomguard.Launch();
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.HealthPercent < 35 &&
            Drain_Life.KnownSpell &&
            Drain_Life.IsDistanceGood &&
            Drain_Life.IsSpellUsable)
        {
            Drain_Life.Launch();
            return;
        }

        if (ObjectManager.Me.BarTwoPercentage < 25 &&
            Life_Tap.KnownSpell &&
            Life_Tap.IsDistanceGood &&
            Life_Tap.IsSpellUsable)
        {
            Life_Tap.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 20 &&
            ItemsManager.GetItemCountByIdLUA(5512) == 1)
        {
            Lua.RunMacroText("/use item:5512");
            Logging.WriteFight("Use Healthstone");
            return;
        }
    }

    private void Healthstone()
    {
        if (!Fight.InFight &&
            ItemsManager.GetItemCountByIdLUA(5512) == 0 &&
            Create_Healthstone.KnownSpell)
        {
            Create_Healthstone.Launch();
            return;
        }
    }

    private void ArmorCheck()
    {
        if (Fel_Armor.KnownSpell &&
            !Fel_Armor.HaveBuff &&
            Fel_Armor.IsSpellUsable)
        {
            Fel_Armor.Launch();
        }
        else if (Demon_Armor.KnownSpell &&
                 !Fel_Armor.KnownSpell &&
                 !Demon_Armor.HaveBuff &&
                 Demon_Armor.IsSpellUsable)
        {
            Demon_Armor.Launch();
        }
    }

    private void Pet()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if ((ObjectManager.Pet.Health == 0 ||
             ObjectManager.Pet.Guid == 0) &&
            !ObjectManager.Me.IsMounted && !ObjectManager.Me.IsDeadMe)
        {
            if (Summon_Felhunter.KnownSpell)
            {
                Summon_Felhunter.Launch();
            }
            else if (!Summon_Felhunter.KnownSpell && Summon_Voidwalker.KnownSpell)
            {
                Summon_Voidwalker.Launch();
            }

            else if (!Summon_Felhunter.KnownSpell &&
                     !Summon_Voidwalker.KnownSpell &&
                     Summon_Imp.KnownSpell)
            {
                Summon_Imp.Launch();
            }
        }
    }

    private void Pet_Health()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (Health_Funnel.KnownSpell)
            if (ObjectManager.Pet.HealthPercent > 0 &&
                ObjectManager.Pet.HealthPercent < 50 &&
                Health_Funnel.IsSpellUsable)
            {
                Health_Funnel.Launch();
                while (ObjectManager.Me.IsCast)
                {
                    if (ObjectManager.Pet.HealthPercent > 80 || ObjectManager.Pet.IsDead)
                        break;

                    Thread.Sleep(100);
                }
            }
    }
}

#endregion CustomClass