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
                    #region Warlock Specialisation checking

                case WoWClass.Warlock:
                    var Summon_Felguard = new Spell("Summon Felguard");
                    var Unstable_Affliction = new Spell("Unstable Affliction");
                    var Conflagrate = new Spell("Conflagrate");
                    if (Unstable_Affliction.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            MessageBox.Show("There is no settings available for your Class/Specialisation.");
                        }
                        else
                        {
                            Logging.WriteFight("Loading Affliction Warlock class...");
                            new Affli();
                        }
                    }
                    else if (Summon_Felguard.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            MessageBox.Show("There is no settings available for your Class/Specialisation.");
                        }
                        else
                        {
                            Logging.WriteFight("Loading Demonology Warlock class...");
                            new Demo();
                        }
                    }
                    else if (Conflagrate.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            MessageBox.Show("There is no settings available for your Class/Specialisation.");
                        }
                        else
                        {
                            Logging.WriteFight("Loading Destruction Warlock class...");
                            new Destro();
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
                            Logging.WriteFight("Loading Demonology Warlock class...");
                            new Demo();
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

#region Warlock

public class Demo
{
    #region InitializeSpell

    private Spell Banish = new Spell("Banish");
    private Spell Carrion_Swarm = new Spell("Carrion Swarm");
    private Spell Command_Demon = new Spell("Command Demon");
    private Spell Corruption = new Spell("Corruption");
    private Spell Create_Healthstone = new Spell("Create Healthstone");
    private Spell Create_Soulwell = new Spell("Create Soulwell");
    private Spell Curse_of_Enfeeblement = new Spell("Curse of Enfeeblement");
    private Spell Curse_of_the_Elements = new Spell("Curse of the Elements");
    private Spell Dark_Apotheosis = new Spell("Dark Apotheosis");
    private Spell Dark_Bargain = new Spell("Dark Bargain");
    private Spell Dark_Intent = new Spell("Dark Intent");
    private Spell Dark_Regeneration = new Spell("Dark Regeneration");
    private Spell Dark_Soul_Knowledge = new Spell("Dark Soul: Knowledge");
    private Spell Demonic_Circle_Summon = new Spell("Demonic Circle: Summon");
    private Spell Demonic_Circle_Teleport = new Spell("Demonic Circle: Teleport");
    private Spell Demonic_Leap = new Spell("Demonic Leap");
    private Spell Doom = new Spell("Doom");
    private Spell Drain_Life = new Spell("Drain Life");
    private Spell Enslave_Demon = new Spell("Enslave Demon");
    private Spell Eye_of_Kilrogg = new Spell("Eye of Kilrogg");
    private Spell Fear = new Spell("Fear");
    private Spell Fel_Flame = new Spell("Fel Flame");
    private Spell Grimoire_of_Sacrifice = new Spell("Grimoire of Sacrifice");
    private Spell Grimoire_of_Service = new Spell("Grimoire of Service");
    private Spell Hand_of_Guldan = new Spell("Hand of Gul'dan");
    private Spell Harvest_Life = new Spell("Harvest Life");
    private Spell Health_Funnel = new Spell("Health Funnel");
    private Spell Hellfire = new Spell("Hellfire");
    private Spell Life_Tap = new Spell("Life Tap");
    private Spell Metamorphosis = new Spell("Metamorphosis");
    private Spell Mortal_Coil = new Spell("Mortal Coil");
    private Spell Ritual_of_Summoning = new Spell("Ritual of Summoning");
    private Spell Shadow_Bolt = new Spell("Shadow Bolt");
    private Spell Soul_Fire = new Spell("Soul Fire");
    private Spell Soulshatter = new Spell("Soulshatter");
    private Spell Soulstone = new Spell("Soulstone");
    private Spell Summon_Abysall = new Spell("Summon Abyssal");
    private Spell Summon_Fel_Imp = new Spell("Fel Imp");
    private Spell Summon_Imp = new Spell("Summon Imp");
    private Spell Summon_Voidwalker = new Spell("Summon Voidwalker");
    private Spell Summon_Voidlord = new Spell("Summon_Voidlord");
    private Spell Summon_Felhunter = new Spell("Summon Felhunter");
    private Spell Summon_Observer = new Spell("Summon Observer");
    private Spell Summon_Shivarra = new Spell("Summon Shivarra");
    private Spell Summon_Succubus = new Spell("Summon Succubus");
    private Spell Summon_Felguard = new Spell("Summon Felguard");
    private Spell Summon_Wrathguard = new Spell("Summon Wrathguard");
    private Spell Summon_Doomguard = new Spell("Summon Doomguard");
    private Spell Summon_Infernal = new Spell("Summon Infernal");
    private Spell Summon_Terrorguard = new Spell("Summon Terrorguard");
    private Spell Touch_of_Chaos = new Spell("Touch of Chaos");
    private Spell Twilight_Ward = new Spell("Twilight Ward");
    private Spell Unbound_Will = new Spell("Unbound Will");
    private Spell Unending_Breath = new Spell("Unending Breath");
    private Spell Unending_Resolve = new Spell("Unending Resolve");
    private Spell Void_Ray = new Spell("Void Ray");

    private Timer Doom_Timer = new Timer(0);
    private Timer Corruption_Timer = new Timer(0);
    private Timer Dark_Soul_Knowledge_Timer = new Timer(0);
    private Timer Grimoire_of_Service_Timer = new Timer(0);
    int Meta = 0;
    int StartFight = 1;

    // profession & racials

    private Spell Arcane_Torrent = new Spell("Arcane Torrent");
    private Spell Berserking = new Spell("Berserking");
    private Spell Blood_Fury = new Spell("Blood Fury");
    private Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    private Spell Lifeblood = new Spell("Lifeblood");
    private Spell Stoneform = new Spell("Stoneform");

    #endregion InitializeSpell

    public Demo()
    {
        Main.range = 30.0f;
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
            Thread.Sleep(400);
        }
    }

    public void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            Buff();
        }
    }

    public void Buff()
    {
        pet();

        if (!Dark_Intent.HaveBuff && Dark_Intent.KnownSpell)
        {
            Dark_Intent.Launch();
        }

        if (ItemsManager.GetItemCountByIdLUA(5512) == 0 && Create_Healthstone.KnownSpell &&
            Create_Healthstone.IsSpellUsable)
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
    }

    public void Pull()
    {
        if (ObjectManager.Me.Level > 35)
        {
            if (Doom_Timer.IsReady && ObjectManager.Me.DemonicFury > 199)
            {
                if (Metamorphosis.KnownSpell && Metamorphosis.IsSpellUsable && Shadow_Bolt.IsDistanceGood
                    && !ObjectManager.Me.HaveBuff(103958))
                {
                    Metamorphosis.Launch();
                    //Meta = 1;
                    Thread.Sleep(400);
                    //if (Doom.KnownSpell && Doom.IsSpellUsable && Doom.IsDistanceGood)
                    //{
                    Logging.WriteFight("Cast Doom.");
                    Lua.RunMacroText("/cast Doom");
                    Doom_Timer = new Timer(1000 * 60);
                    //}
                }
                if (Metamorphosis.KnownSpell && Metamorphosis.IsSpellUsable && ObjectManager.Me.HaveBuff(103958))
                {
                    Thread.Sleep(2500);
                    Metamorphosis.Launch();
                }
                //Meta = 0;
                StartFight--;
                return;
            }
        }
    }

    public void LowCombat()
    {
        AvoidMelee();
        Heal();
        Resistance();
        Buff();

        if (ObjectManager.Me.BarTwoPercentage < 75 && Life_Tap.KnownSpell && Life_Tap.IsSpellUsable)
        {
            Life_Tap.Launch();
            return;
        }
        if (Shadow_Bolt.IsDistanceGood)
        {
            Logging.WriteFight("Cast Fel Flame.");
            Lua.RunMacroText("/cast Fel Flame");
            if (ObjectManager.Target.HealthPercent < 50 && ObjectManager.Target.HealthPercent > 0)
            {
                Logging.WriteFight("Cast Fel Flame.");
                Lua.RunMacroText("/cast Fel Flame");
                return;
            }
        }

        if (ObjectManager.Target.HealthPercent > 90)
        {
            if (Hellfire.IsSpellUsable && Hellfire.KnownSpell && Hellfire.IsDistanceGood)
            {
                //Logging.WriteFight("Cast Hellfire.");
                //Lua.RunMacroText("/cast Hellfire");
                Hellfire.Launch();
                Thread.Sleep(200);
                while (ObjectManager.Me.IsCast && ObjectManager.Target.HealthPercent > 0)
                {
                    Thread.Sleep(100);
                    Thread.Sleep(100);
                }
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
        if (ObjectManager.Me.DemonicFury > 899 || Doom_Timer.IsReady)
        {
            if (ObjectManager.Me.DemonicFury > 199)
            {
                if (Corruption.KnownSpell && Corruption.IsSpellUsable && Corruption.IsDistanceGood)
                {
                    Corruption.Launch();
                    Corruption_Timer = new Timer(1000 * 20);
                }
                MetamorphosisCombat();
                return;
            }
        }

        if (ObjectManager.Me.HaveBuff(103958))
            MetamorphosisCombat();

        if (Curse_of_the_Elements.KnownSpell && Curse_of_the_Elements.IsSpellUsable
            && Curse_of_the_Elements.IsDistanceGood && !Curse_of_the_Elements.TargetHaveBuff)
        {
            Curse_of_the_Elements.Launch();
            return;
        }

        if (ObjectManager.Me.BarTwoPercentage < 75 && Life_Tap.KnownSpell && Life_Tap.IsSpellUsable)
        {
            Life_Tap.Launch();
            return;
        }

        //if (Dark_Soul_Knowledge.KnownSpell && Dark_Soul_Knowledge.IsSpellUsable)
        if (Dark_Soul_Knowledge_Timer.IsReady)
        {
            //Dark_Soul_Knowledge.Launch();
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
            if (ObjectManager.Me.Level > 83)
            {
                Logging.WriteFight("Cast Dark Soul: Knowledge.");
                Lua.RunMacroText("/cast Dark Soul: Knowledge");
            }
            Dark_Soul_Knowledge_Timer = new Timer(1000 * 60 * 2);
            Logging.WriteFight("Use Trinket 1.");
            Lua.RunMacroText("/use 13");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Logging.WriteFight("Use Trinket 2.");
            Lua.RunMacroText("/use 14");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            return;
        }

        if (Summon_Terrorguard.KnownSpell && Summon_Terrorguard.IsSpellUsable && Summon_Terrorguard.IsDistanceGood)
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
            Summon_Terrorguard.Launch();
            return;
        }

        if (Summon_Doomguard.KnownSpell && Summon_Doomguard.IsSpellUsable && Summon_Doomguard.IsDistanceGood)
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
            Summon_Doomguard.Launch();
            return;
        }

        if (Grimoire_of_Service_Timer.IsReady && (ObjectManager.Me.Level > 19))
        {
            Logging.WriteFight("Cast Grimoire of Service.");
            Lua.RunMacroText("/cast Grimoire of Service");
            Grimoire_of_Service_Timer = new Timer(1000 * 60 * 2);
            return;
        }

        if (ObjectManager.GetNumberAttackPlayer() > 4 && Harvest_Life.IsSpellUsable && Harvest_Life.KnownSpell
            && Harvest_Life.IsDistanceGood)
        {
            Harvest_Life.Launch();
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(200);
            }
            return;
        }

        if (ObjectManager.GetNumberAttackPlayer() > 2 && Command_Demon.IsSpellUsable && Command_Demon.KnownSpell
            && Command_Demon.IsDistanceGood)
        {
            //Logging.WriteFight("Cast Command Demon.");
            //Lua.RunMacroText("/cast Command Demon");
            Command_Demon.Launch();
            return;
        }

        if (ObjectManager.GetNumberAttackPlayer() > 4 && Hellfire.IsSpellUsable && Hellfire.KnownSpell
            && Hellfire.IsDistanceGood && !Harvest_Life.KnownSpell)
        {
            //Logging.WriteFight("Cast Hellfire.");
            //Lua.RunMacroText("/cast Hellfire");
            Hellfire.Launch();
            Thread.Sleep(200);
            while (ObjectManager.Me.IsCast && ObjectManager.Target.HealthPercent > 0)
            {
                Thread.Sleep(100);
                Thread.Sleep(100);
            }
            return;
        }

        if (Corruption.KnownSpell && Corruption.IsSpellUsable && Corruption.IsDistanceGood
            && (!Corruption.TargetHaveBuff || Corruption_Timer.IsReady))
        {
            Corruption.Launch();
            Corruption_Timer = new Timer(1000 * 20);
            return;
        }

        if (Hand_of_Guldan.KnownSpell && Hand_of_Guldan.IsSpellUsable && Hand_of_Guldan.IsDistanceGood
            && !ObjectManager.Target.HaveBuff(47960))
        {
            Hand_of_Guldan.Launch();
            return;
        }

        if (Soul_Fire.KnownSpell && Soul_Fire.IsSpellUsable && Soul_Fire.IsDistanceGood && ObjectManager.Me.HaveBuff(122355))
        {
            Soul_Fire.Launch();
            return;
        }

        if (Shadow_Bolt.KnownSpell && Shadow_Bolt.IsSpellUsable && Shadow_Bolt.IsDistanceGood)
        {
            Shadow_Bolt.Launch();
            return;
        }
    }

    public void MetamorphosisCombat()
    {
        while (ObjectManager.Me.DemonicFury > 100)
        {
            if (Metamorphosis.KnownSpell && Metamorphosis.IsSpellUsable && !ObjectManager.Me.HaveBuff(103958))
            {
                Metamorphosis.Launch();
                Thread.Sleep(700);
                //Meta = 1;
            }

            if (ObjectManager.GetNumberAttackPlayer() > 2)
            {
                if (Carrion_Swarm.IsSpellUsable && Carrion_Swarm.KnownSpell && Carrion_Swarm.IsDistanceGood
                    && ObjectManager.Me.HaveBuff(103958))
                {
                    if (ObjectManager.Me.Level > 61)
                    {
                        Logging.WriteFight("Cast Immolation Aura.");
                        Lua.RunMacroText("/cast Immolation Aura");
                        Thread.Sleep(200);
                    }
                    //Logging.WriteFight("Cast Carrion Swarm.");
                    //Lua.RunMacroText("/cast Carrion Swarm");
                    Carrion_Swarm.Launch();
                }
                else
                    return;

                //if (Void_Ray.IsSpellUsable && Void_Ray.KnownSpell && Void_Ray.IsDistanceGood)
                //{
                Thread.Sleep(200);
                if (ObjectManager.Me.Level > 84 && Shadow_Bolt.IsDistanceGood && ObjectManager.Me.HaveBuff(103958))
                {
                    Logging.WriteFight("Cast Void Ray.");
                    Lua.RunMacroText("/cast Void Ray");
                }
                else
                    return;
                //}
            }

            else
            {
                if (ObjectManager.Me.Level > 35)
                {
                    if (Shadow_Bolt.IsDistanceGood && ObjectManager.Me.HaveBuff(103958)
                        && (Doom_Timer.IsReady || !ObjectManager.Target.HaveBuff(124913)))
                    {
                        Logging.WriteFight("Cast Doom.");
                        Lua.RunMacroText("/cast Doom");
                        Doom_Timer = new Timer(1000 * 60);
                    }
                    else
                        return;
                }

                //if (Touch_of_Chaos.KnownSpell && Touch_of_Chaos.IsSpellUsable && Touch_of_Chaos.IsDistanceGood)
                //{
                Thread.Sleep(200);
                if (ObjectManager.Me.Level > 24 && Shadow_Bolt.IsDistanceGood && ObjectManager.Me.HaveBuff(103958))
                {
                    Logging.WriteFight("Cast Touch of Chaos.");
                    Lua.RunMacroText("/cast Touch of Chaos");
                }
                else
                    return;
                //}
            }
        }

        Thread.Sleep(700);
        if (ObjectManager.Me.HaveBuff(103958))
        {
            Metamorphosis.Launch();
            //Meta = 0;
            return;
        }
        return;
    }

    private void Heal()
    {
        if (ObjectManager.Me.HealthPercent < 80 && Gift_of_the_Naaru.IsSpellUsable && Gift_of_the_Naaru.KnownSpell)
        {
            Gift_of_the_Naaru.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 75 && ItemsManager.GetItemCountByIdLUA(5512) > 0)
        {
            Logging.WriteFight("Use Healthstone.");
            nManager.Wow.Helpers.ItemsManager.UseItem("Healthstone");
            return;
        }

        if (ObjectManager.Me.HealthPercent < 65 && Dark_Regeneration.IsSpellUsable && Dark_Regeneration.KnownSpell)
        {
            Dark_Regeneration.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 35 && Drain_Life.KnownSpell &&
            Drain_Life.IsDistanceGood && Drain_Life.IsSpellUsable)
        {
            //SpellManager.CastSpellByIdLUA(689);
            Drain_Life.Launch();
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(200);
            }
            return;
        }
    }

    private void Resistance()
    {
        if (ObjectManager.Me.HealthPercent < 70 &&
            Unending_Resolve.KnownSpell && Unending_Resolve.IsSpellUsable)
        {
            Unending_Resolve.Launch();
        }

        if (ObjectManager.Me.HealthPercent < 40 &&
            Dark_Bargain.KnownSpell && Dark_Bargain.IsSpellUsable)
        {
            Dark_Bargain.Launch();
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
            Twilight_Ward.KnownSpell && Twilight_Ward.IsSpellUsable)
        {
            Twilight_Ward.Launch();
        }
    }

    private void pet()
    {
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

        if (Grimoire_of_Sacrifice.KnownSpell && !Grimoire_of_Sacrifice.HaveBuff && Grimoire_of_Sacrifice.IsSpellUsable)
        {
            if ((ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0))
            {
                Logging.WriteFight(" - PET DEAD - ");
                Summon_Felhunter.Launch();
                Thread.Sleep(200);
            }
            Grimoire_of_Sacrifice.Launch();
        }

        if ((ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0))
        {
            Logging.WriteFight(" - PET DEAD - ");
            if (Summon_Wrathguard.KnownSpell && Summon_Wrathguard.IsSpellUsable)
            {
                Summon_Wrathguard.Launch();
                return;
            }
            else
            {
                Summon_Felguard.Launch();
                return;
            }
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

public class Affli
{
    #region InitializeSpell

    private Spell Agony = new Spell("Agony");
    private Spell Banish = new Spell("Banish");
    private Spell Command_Demon = new Spell("Command Demon");
    private Spell Corruption = new Spell("Corruption");
    private Spell Create_Healthstone = new Spell("Create Healthstone");
    private Spell Create_Soulwell = new Spell("Create Soulwell");
    private Spell Curse_of_Enfeeblement = new Spell("Curse of Enfeeblement");
    private Spell Curse_of_Exhaustion = new Spell("Curse of Exhaustion");
    private Spell Curse_of_the_Elements = new Spell("Curse of the Elements");
    private Spell Dark_Bargain = new Spell("Dark Bargain");
    private Spell Dark_Intent = new Spell("Dark Intent");
    private Spell Dark_Regeneration = new Spell("Dark Regeneration");
    private Spell Dark_Soul_Misery = new Spell("Dark Soul: Misery");
    private Spell Demonic_Circle_Summon = new Spell("Demonic Circle: Summon");
    private Spell Demonic_Circle_Teleport = new Spell("Demonic Circle: Teleport");
    private Spell Drain_Life = new Spell("Drain Life");
    private Spell Drain_Soul = new Spell("Drain Soul");
    private Spell Enslave_Demon = new Spell("Enslave Demon");
    private Spell Eye_of_Kilrogg = new Spell("Eye of Kilrogg");
    private Spell Fear = new Spell("Fear");
    private Spell Fel_Flame = new Spell("Fel Flame");
    private Spell Grimoire_of_Sacrifice = new Spell("Grimoire of Sacrifice");
    private Spell Grimoire_of_Service = new Spell("Grimoire of Service");
    private Spell Harvest_Life = new Spell("Harvest Life");
    private Spell Haunt = new Spell("Haunt");
    private Spell Health_Funnel = new Spell("Health Funnel");
    private Spell Life_Tap = new Spell("Life Tap");
    private Spell Malefic_Grasp = new Spell("Malefic Grasp");
    private Spell Mortal_Coil = new Spell("Mortal Coil");
    private Spell Rain_of_Fire = new Spell("Rain of Fire");
    private Spell Ritual_of_Summoning = new Spell("Ritual of Summoning");
    private Spell Seed_of_Corruption = new Spell("Seed of Corruption");
    private Spell Shadow_Bolt = new Spell("Shadow Bolt");
    private Spell Soul_Swap = new Spell("Soul Swap");
    private Spell Soulburn = new Spell("Soulburn");
    private Spell Soulshatter = new Spell("Soulshatter");
    private Spell Soulstone = new Spell("Soulstone");
    private Spell Summon_Imp = new Spell("Summon Imp");
    private Spell Summon_Voidwalker = new Spell("Summon Voidwalker");
    private Spell Summon_Felhunter = new Spell("Summon Felhunter");
    private Spell Summon_Succubus = new Spell("Summon Succubus");
    private Spell Summon_Doomguard = new Spell("Summon Doomguard");
    private Spell Summon_Terrorguard = new Spell("Summon Terrorguard");
    private Spell Summon_Infernal = new Spell("Summon Infernal");
    private Spell Twilight_Ward = new Spell("Twilight Ward");
    private Spell Unbound_Will = new Spell("Unbound Will");
    private Spell Unending_Breath = new Spell("Unending Breath");
    private Spell Unending_Resolve = new Spell("Unending Resolve");
    private Spell Unstable_Affliction = new Spell("Unstable Affliction");

    private Timer Agony_Timer = new Timer(0);
    private Timer Corruption_Timer = new Timer(0);
    private Timer Unstable_Affliction_Timer = new Timer(0);
    private Timer Dark_Soul_Misery_Timer = new Timer(0);
    private Timer Grimoire_of_Service_Timer = new Timer(0);

    // profession & racials

    private Spell Arcane_Torrent = new Spell("Arcane Torrent");
    private Spell Berserking = new Spell("Berserking");
    private Spell Blood_Fury = new Spell("Blood Fury");
    private Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    private Spell Lifeblood = new Spell("Lifeblood");
    private Spell Stoneform = new Spell("Stoneform");

    #endregion InitializeSpell

    public Affli()
    {
        Main.range = 30.0f;
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
            Thread.Sleep(400);
        }
    }

    public void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            Buff();
        }
    }

    public void Buff()
    {
        pet();

        if (!Dark_Intent.HaveBuff && Dark_Intent.KnownSpell)
        {
            Dark_Intent.Launch();
        }

        if (ItemsManager.GetItemCountByIdLUA(5512) == 0 && Create_Healthstone.KnownSpell &&
            Create_Healthstone.IsSpellUsable)
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
    }

    public void Pull()
    {
        if (!Agony.TargetHaveBuff && !Corruption.TargetHaveBuff && !Unstable_Affliction.TargetHaveBuff)
        {
            if (Soulburn.IsSpellUsable && Soulburn.KnownSpell && Soul_Swap.IsSpellUsable && Soul_Swap.KnownSpell
                && Soul_Swap.IsDistanceGood)
            {
                Soulburn.Launch();
                Thread.Sleep(200);
                Soul_Swap.Launch();
            }
            Agony_Timer = new Timer(1000 * 20);
            Corruption_Timer = new Timer(1000*15);
            Unstable_Affliction_Timer = new Timer(1000 * 10);
        }
    }

    public void LowCombat()
    {
        AvoidMelee();
        Heal();
        Resistance();
        Buff();

        if (ObjectManager.Me.BarTwoPercentage < 75 && Life_Tap.KnownSpell && Life_Tap.IsSpellUsable)
        {
            Life_Tap.Launch();
            return;
        }
        //SpellManager.CastSpellByIdLUA(103103);
        //Malefic_Grasp.Launch();
        Logging.WriteFight("Cast Malefic Grasp.");
        Lua.RunMacroText("/cast Malefic Grasp");
        Thread.Sleep(200);
        while (ObjectManager.Me.IsCast)
        {
            Thread.Sleep(100);
            Thread.Sleep(100);
        }
        
        if (ObjectManager.Target.HealthPercent < 50 && ObjectManager.Target.HealthPercent > 0)
        {
            Logging.WriteFight("Cast Malefic Grasp.");
            Lua.RunMacroText("/cast Malefic Grasp");
            Thread.Sleep(200);
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(100);
                Thread.Sleep(100);
            }
            return;
        }

        if (ObjectManager.Target.HealthPercent > 90)
        {
            if (Rain_of_Fire.IsSpellUsable && Rain_of_Fire.KnownSpell && Rain_of_Fire.IsDistanceGood)
            {
                //Rain_of_Fire.Launch();
                SpellManager.CastSpellByIDAndPosition(5740, ObjectManager.Target.Position);
                while (ObjectManager.Me.IsCast)
                {
                    Thread.Sleep(200);
                }
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
        Pull();

        if (Curse_of_the_Elements.KnownSpell && Curse_of_the_Elements.IsSpellUsable
            && Curse_of_the_Elements.IsDistanceGood && !Curse_of_the_Elements.TargetHaveBuff)
        {
            Curse_of_the_Elements.Launch();
            return;
        }

        if (ObjectManager.Me.BarTwoPercentage < 75 && Life_Tap.KnownSpell && Life_Tap.IsSpellUsable)
        {
            Life_Tap.Launch();
            return;
        }

        //if (Dark_Soul_Misery.KnownSpell && Dark_Soul_Misery.IsSpellUsable)
        if (Dark_Soul_Misery_Timer.IsReady)
        {
            //Dark_Soul_Misery.Launch();
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
            if (ObjectManager.Me.Level > 83)
            {
                Logging.WriteFight("Cast Dark Soul: Misery.");
                Lua.RunMacroText("/cast Dark Soul: Misery");
            }
            Dark_Soul_Misery_Timer = new Timer(1000 * 60 * 2);
            Logging.WriteFight("Use Trinket 1.");
            Lua.RunMacroText("/use 13");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Logging.WriteFight("Use Trinket 2.");
            Lua.RunMacroText("/use 14");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            return;
        }

        if (Summon_Terrorguard.KnownSpell && Summon_Terrorguard.IsSpellUsable && Summon_Terrorguard.IsDistanceGood)
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
            Summon_Terrorguard.Launch();
            return;
        }

        if (Summon_Doomguard.KnownSpell && Summon_Doomguard.IsSpellUsable && Summon_Doomguard.IsDistanceGood)
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
            Summon_Doomguard.Launch();
            return;
        }

        if (Grimoire_of_Service_Timer.IsReady && (ObjectManager.Me.Level > 19))
        {
            Logging.WriteFight("Cast Grimoire of Service.");
            Lua.RunMacroText("/cast Grimoire of Service");
            Grimoire_of_Service_Timer = new Timer(1000 * 60 * 2);
            return;
        }

        if (ObjectManager.Target.HealthPercent < 20)
        {
            Drain_Soul.Launch();
            while (ObjectManager.Me.IsCast && !Agony_Timer.IsReady && !Corruption_Timer.IsReady
                && !Unstable_Affliction_Timer.IsReady)
            {
                Thread.Sleep(200);
            }
            if (Agony_Timer.IsReady || Corruption_Timer.IsReady || Unstable_Affliction_Timer.IsReady)
            {
                if (Soulburn.IsSpellUsable && Soulburn.KnownSpell && Soul_Swap.IsSpellUsable && Soul_Swap.KnownSpell
                && Soul_Swap.IsDistanceGood)
                {
                    Soulburn.Launch();
                    Thread.Sleep(200);
                    Soul_Swap.Launch();
                }
                Agony_Timer = new Timer(1000 * 20);
                Corruption_Timer = new Timer(1000 * 15);
                Unstable_Affliction_Timer = new Timer(1000 * 10);
            }
            return;
        }

        if (ObjectManager.GetNumberAttackPlayer() > 4 && Soulburn.IsSpellUsable && Soulburn.KnownSpell && !Corruption.TargetHaveBuff
            && Seed_of_Corruption.IsSpellUsable && Seed_of_Corruption.KnownSpell && Seed_of_Corruption.IsDistanceGood)
        {
            Soulburn.Launch();
            Thread.Sleep(200);
            Seed_of_Corruption.Launch();
            return;
        }

        if (ObjectManager.GetNumberAttackPlayer() > 4 && Harvest_Life.IsSpellUsable && Harvest_Life.KnownSpell
            && Harvest_Life.IsDistanceGood)
        {
            Harvest_Life.Launch();
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(200);
            }
            return;
        }

        if (ObjectManager.GetNumberAttackPlayer() > 4 && Rain_of_Fire.IsSpellUsable && Rain_of_Fire.KnownSpell
            && Rain_of_Fire.IsDistanceGood)
        {
            //Rain_of_Fire.Launch();
            SpellManager.CastSpellByIDAndPosition(5740, ObjectManager.Target.Position);
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(200);
            }
            return;
        }

        if (Agony.KnownSpell && Agony.IsSpellUsable && Agony.IsDistanceGood
            && (!Agony.TargetHaveBuff || Agony_Timer.IsReady))
        {
            Agony.Launch();
            Agony_Timer = new Timer(1000 * 20);
            return;
        }

        if (Corruption.KnownSpell && Corruption.IsSpellUsable && Corruption.IsDistanceGood
            && (!Corruption.TargetHaveBuff || Corruption_Timer.IsReady))
        {
            Corruption.Launch();
            Corruption_Timer = new Timer(1000 * 15);
            return;
        }

        if (Unstable_Affliction.KnownSpell && Unstable_Affliction.IsSpellUsable && Unstable_Affliction.IsDistanceGood
            && (!Unstable_Affliction.TargetHaveBuff || Unstable_Affliction_Timer.IsReady))
        {
            Unstable_Affliction.Launch();
            Unstable_Affliction_Timer = new Timer(1000 * 10);
            return;
        }

        if (Haunt.KnownSpell && Haunt.IsSpellUsable && Haunt.IsDistanceGood && !Haunt.TargetHaveBuff)
        {
            Haunt.Launch();
            return;
        }

        //if (Malefic_Grasp.KnownSpell && Malefic_Grasp.IsSpellUsable && Malefic_Grasp.IsDistanceGood)
        if (!ObjectManager.Me.IsCast && !Agony_Timer.IsReady && !Corruption_Timer.IsReady && !Unstable_Affliction_Timer.IsReady
            && ObjectManager.Me.Level > 41)
        {
            //SpellManager.CastSpellByIdLUA(103103);
            //Malefic_Grasp.Launch();
            Logging.WriteFight("Cast Malefic Grasp.");
            Lua.RunMacroText("/cast Malefic Grasp");
            return;
        }
        else
        {
            if (!ObjectManager.Me.IsCast && Shadow_Bolt.KnownSpell && Shadow_Bolt.IsSpellUsable
                && Shadow_Bolt.IsDistanceGood)
            {
                Shadow_Bolt.Launch();
                return;
            }
        }
        return;
    }

    private void Heal()
    {
        if (ObjectManager.Me.HealthPercent < 80 && Gift_of_the_Naaru.IsSpellUsable && Gift_of_the_Naaru.KnownSpell)
        {
            Gift_of_the_Naaru.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 75 && ItemsManager.GetItemCountByIdLUA(5512) > 0)
        {
            Logging.WriteFight("Use Healthstone.");
            nManager.Wow.Helpers.ItemsManager.UseItem("Healthstone");
            return;
        }

        if (ObjectManager.Me.HealthPercent < 65 && Dark_Regeneration.IsSpellUsable && Dark_Regeneration.KnownSpell)
        {
            Dark_Regeneration.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 35 && Drain_Life.KnownSpell &&
            Drain_Life.IsDistanceGood && Drain_Life.IsSpellUsable)
        {
            //SpellManager.CastSpellByIdLUA(689);
            Drain_Life.Launch();
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(200);
            }
            return;
        }
    }

    private void Resistance()
    {
        if (ObjectManager.Me.HealthPercent < 70 &&
            Unending_Resolve.KnownSpell && Unending_Resolve.IsSpellUsable)
        {
            Unending_Resolve.Launch();
        }

        if (ObjectManager.Me.HealthPercent < 40 &&
            Dark_Bargain.KnownSpell && Dark_Bargain.IsSpellUsable)
        {
            Dark_Bargain.Launch();
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
            Twilight_Ward.KnownSpell && Twilight_Ward.IsSpellUsable)
        {
            Twilight_Ward.Launch();
        }

        if (ObjectManager.Target.IsCast &&
            ObjectManager.Target.IsTargetingMe &&
            Command_Demon.KnownSpell && Command_Demon.IsSpellUsable && Command_Demon.IsDistanceGood)
        {
            Command_Demon.Launch();
        }
    }

    private void pet()
    {
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

        if (Grimoire_of_Sacrifice.KnownSpell && !Grimoire_of_Sacrifice.HaveBuff && Grimoire_of_Sacrifice.IsSpellUsable)
        {
            if ((ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0))
            {
                Logging.WriteFight(" - PET DEAD - ");
                if (Soulburn.KnownSpell && Soulburn.IsSpellUsable)
                {
                    Soulburn.Launch();
                }
                Summon_Felhunter.Launch();
                Thread.Sleep(200);
            }
            Grimoire_of_Sacrifice.Launch();
        }

        if ((ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0) && !Grimoire_of_Sacrifice.KnownSpell)
        {
            Logging.WriteFight(" - PET DEAD - ");
            if (Summon_Felhunter.KnownSpell && Summon_Felhunter.IsSpellUsable)
            {
                Summon_Felhunter.Launch();
                return;
            }
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

public class Destro
{
    #region InitializeSpell

    private Spell Banish = new Spell("Banish");
    private Spell Chaos_Bolt = new Spell("Chaos Bolt");
    private Spell Command_Demon = new Spell("Command Demon");
    private Spell Conflagrate = new Spell("Conflagrate");
    private Spell Create_Healthstone = new Spell("Create Healthstone");
    private Spell Create_Soulwell = new Spell("Create Soulwell");
    private Spell Curse_of_Enfeeblement = new Spell("Curse of Enfeeblement");
    private Spell Curse_of_the_Elements = new Spell("Curse of the Elements");
    private Spell Dark_Bargain = new Spell("Dark Bargain");
    private Spell Dark_Intent = new Spell("Dark Intent");
    private Spell Dark_Regeneration = new Spell("Dark_Regeneration");
    private Spell Dark_Soul_Instability = new Spell("Dark Soul: Instability");
    private Spell Demonic_Circle_Summon = new Spell("Demonic Circle: Summon");
    private Spell Demonic_Circle_Teleport = new Spell("Demonic Circle: Teleport");
    private Spell Drain_Life = new Spell("Drain Life");
    private Spell Ember_Tap = new Spell("Ember Tap");
    private Spell Enslave_Demon = new Spell("Enslave Demon");
    private Spell Eye_of_Kilrogg = new Spell("Eye of Kilrogg");
    private Spell Fear = new Spell("Fear");
    private Spell Fel_Flame = new Spell("Fel Flame");
    private Spell Fire_and_Brimstone = new Spell("Fire and Brimstone");
    private Spell Flames_of_Xoroth = new Spell("Flames of Xoroth");
    private Spell Grimoire_of_Sacrifice = new Spell("Grimoire of Sacrifice");
    private Spell Grimoire_of_Service = new Spell("Grimoire of Service");
    private Spell Harvest_Life = new Spell("Harvest Life");
    private Spell Havoc = new Spell("Havoc");
    private Spell Health_Funnel = new Spell("Health Funnel");
    private Spell Immolate = new Spell("Immolate");
    //Bugged Immolate -> Corruption
    private Spell Corruption = new Spell("Corruption");
    private Spell Incinerate = new Spell("Incinerate");
    //Bugged Incinerate -> Corruption
    private Spell Shadow_Bolt = new Spell("Shadow Bolt");
    private Spell Mortal_Coil = new Spell("Mortal Coil");
    private Spell Rain_of_Fire = new Spell("Rain of Fire");
    private Spell Ritual_of_Summoning = new Spell("Ritual of Summoning");
    private Spell Shadowburn = new Spell("Shadowburn");
    private Spell Soulshatter = new Spell("Soulshatter");
    private Spell Soulstone = new Spell("Soulstone");
    private Spell Summon_Imp = new Spell("Summon Imp");
    private Spell Summon_Voidwalker = new Spell("Summon Voidwalker");
    private Spell Summon_Felhunter = new Spell("Summon Felhunter");
    private Spell Summon_Succubus = new Spell("Summon Succubus");
    private Spell Summon_Doomguard = new Spell("Summon Doomguard");
    private Spell Summon_Terrorguard = new Spell("Summon Terrorguard");
    private Spell Summon_Infernal = new Spell("Summon Infernal");
    private Spell Twilight_Ward = new Spell("Twilight Ward");
    private Spell Unbound_Will = new Spell("Unbound Will");
    private Spell Unending_Breath = new Spell("Unending Breath");
    private Spell Unending_Resolve = new Spell("Unending Resolve");
    private Spell Unstable_Affliction = new Spell("Unstable Affliction");

    private Timer Immolate_Timer = new Timer(0);
    private Timer Dark_Soul_Instability_Timer = new Timer(0);
    private Timer Grimoire_of_Service_Timer = new Timer(0);

    // profession & racials

    private Spell Arcane_Torrent = new Spell("Arcane Torrent");
    private Spell Berserking = new Spell("Berserking");
    private Spell Blood_Fury = new Spell("Blood Fury");
    private Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    private Spell Lifeblood = new Spell("Lifeblood");
    private Spell Stoneform = new Spell("Stoneform");

    #endregion InitializeSpell

    public Destro()
    {
        Main.range = 30.0f;
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
            Thread.Sleep(400);
        }
    }

    public void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            Buff();
        }
    }

    public void Buff()
    {
        pet();

        if (!Dark_Intent.HaveBuff && Dark_Intent.KnownSpell)
        {
            Dark_Intent.Launch();
        }

        if (ItemsManager.GetItemCountByIdLUA(5512) == 0 && Create_Healthstone.KnownSpell &&
            Create_Healthstone.IsSpellUsable)
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
    }

    public void Pull()
    {
        if (Curse_of_the_Elements.KnownSpell && Curse_of_the_Elements.IsSpellUsable
            && Curse_of_the_Elements.IsDistanceGood && !Curse_of_the_Elements.TargetHaveBuff)
        {
            Curse_of_the_Elements.Launch();
            return;
        }
    }

    public void LowCombat()
    {
        AvoidMelee();
        Heal();
        Resistance();
        Buff();

        if (Shadow_Bolt.KnownSpell && Shadow_Bolt.IsSpellUsable && Shadow_Bolt.IsDistanceGood)
        {
            Shadow_Bolt.Launch();
        }
        //Logging.WriteFight("Cast Incinerate.");
        //Lua.RunMacroText("/cast Incinerate");
        if (ObjectManager.Target.HealthPercent < 50 && ObjectManager.Target.HealthPercent > 0)
        {
            //Logging.WriteFight("Cast Incinerate.");
            //Lua.RunMacroText("/cast Incinerate");
            if (Shadow_Bolt.KnownSpell && Shadow_Bolt.IsSpellUsable && Shadow_Bolt.IsDistanceGood)
            {
                Shadow_Bolt.Launch();
                return;
            }
        }
        if (Rain_of_Fire.IsSpellUsable && Rain_of_Fire.KnownSpell && Rain_of_Fire.IsDistanceGood)
        {
            //Rain_of_Fire.Launch();
            SpellManager.CastSpellByIDAndPosition(5740, ObjectManager.Target.Position);
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(200);
            }
            return;
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
        Pull();

        if (Curse_of_the_Elements.KnownSpell && Curse_of_the_Elements.IsSpellUsable
            && Curse_of_the_Elements.IsDistanceGood && !Curse_of_the_Elements.TargetHaveBuff)
        {
            Curse_of_the_Elements.Launch();
            return;
        }

        //if (Dark_Soul_Instability.KnownSpell && Dark_Soul_Instability.IsSpellUsable)
        if (Dark_Soul_Instability_Timer.IsReady)
        {
            //Dark_Soul_Instability.Launch();
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
            if (ObjectManager.Me.Level > 83)
            {
                Logging.WriteFight("Cast Dark Soul: Instability.");
                Lua.RunMacroText("/cast Dark Soul: Instability");
            }
            Dark_Soul_Instability_Timer = new Timer(1000 * 60 * 2);
            Logging.WriteFight("Use Trinket 1.");
            Lua.RunMacroText("/use 13");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Logging.WriteFight("Use Trinket 2.");
            Lua.RunMacroText("/use 14");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            return;
        }

        if (Summon_Terrorguard.KnownSpell && Summon_Terrorguard.IsSpellUsable && Summon_Terrorguard.IsDistanceGood)
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
            Summon_Terrorguard.Launch();
            return;
        }

        if (Summon_Doomguard.KnownSpell && Summon_Doomguard.IsSpellUsable && Summon_Doomguard.IsDistanceGood)
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
            Summon_Doomguard.Launch();
            return;
        }

        /*if (Grimoire_of_Service.KnownSpell && Grimoire_of_Service.IsSpellUsable)
        {
            Grimoire_of_Service.Launch();
            return;
        }*/
        if (Grimoire_of_Service_Timer.IsReady && (ObjectManager.Me.Level > 19))
        {
            Logging.WriteFight("Cast Grimoire of Service.");
            Lua.RunMacroText("/cast Grimoire of Service");
            Grimoire_of_Service_Timer = new Timer(1000 * 60 * 2);
            return;
        }

        if (ObjectManager.GetNumberAttackPlayer() > 4 && Fire_and_Brimstone.IsSpellUsable && Fire_and_Brimstone.KnownSpell
            && !Corruption.TargetHaveBuff && Corruption.IsSpellUsable && Corruption.KnownSpell && Corruption.IsDistanceGood)
        {
            Fire_and_Brimstone.Launch();
            Thread.Sleep(200);
            //Immolate.Launch();
            //Logging.WriteFight("Cast Immolate.");
            //Lua.RunMacroText("/cast Immolate");
            Corruption.Launch();
            Immolate_Timer = new Timer(1000 * 12);
            return;
        }

        if (ObjectManager.GetNumberAttackPlayer() > 4 && ObjectManager.Target.HaveBuff(348))
        {
            //Harvest_Life.Launch();
            Logging.WriteFight("Cast Harvest Life.");
            Lua.RunMacroText("/cast Harvest Life");
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(200);
            }
            return;
        }

        if (ObjectManager.GetNumberAttackPlayer() > 4 && Fire_and_Brimstone.IsSpellUsable && Fire_and_Brimstone.KnownSpell
            && Shadow_Bolt.KnownSpell && Shadow_Bolt.IsSpellUsable && Shadow_Bolt.IsDistanceGood)
        {
            Fire_and_Brimstone.Launch();
            Thread.Sleep(200);
            //Incinerate.Launch();
            //Logging.WriteFight("Cast Incinerate.");
            //Lua.RunMacroText("/cast Incinerate");
            Shadow_Bolt.Launch();
            return;
        }

        if (ObjectManager.GetNumberAttackPlayer() > 4 && Rain_of_Fire.IsSpellUsable && Rain_of_Fire.KnownSpell
            && Rain_of_Fire.IsDistanceGood)
        {
            //Rain_of_Fire.Launch();
            SpellManager.CastSpellByIDAndPosition(5740, ObjectManager.Target.Position);
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(200);
            }
            return;
        }

        if (Conflagrate.KnownSpell && Conflagrate.IsSpellUsable && Conflagrate.IsDistanceGood)
        {
            Conflagrate.Launch();
            return;
        }

        if (/*!Corruption.TargetHaveBuff || */Immolate_Timer.IsReady)
        {
            //Immolate.Launch();
            //Logging.WriteFight("Cast Immolate.");
            //Lua.RunMacroText("/cast Immolate");
            Corruption.Launch();
            Immolate_Timer = new Timer(1000 * 12);
            return;
        }

        if (ObjectManager.Target.HealthPercent < 20)
        {
            if (Shadowburn.KnownSpell && Shadowburn.IsSpellUsable && Shadowburn.IsDistanceGood
                && !ObjectManager.Me.HaveBuff(117828))
            {
                Shadowburn.Launch();
                return;
            }
        }

        else
        {
            if (Chaos_Bolt.KnownSpell && Chaos_Bolt.IsSpellUsable && Chaos_Bolt.IsDistanceGood
                && !ObjectManager.Me.HaveBuff(117828))
            {
                Chaos_Bolt.Launch();
                return;
            }
        }

        if (Shadow_Bolt.KnownSpell && Shadow_Bolt.IsSpellUsable && Shadow_Bolt.IsDistanceGood)
        {
            Shadow_Bolt.Launch();
            return;
        }
        //Logging.WriteFight("Cast Incinerate.");
        //Lua.RunMacroText("/cast Incinerate");
        //return;
    }

    private void Heal()
    {
        if (ObjectManager.Me.HealthPercent < 80 && Gift_of_the_Naaru.IsSpellUsable && Gift_of_the_Naaru.KnownSpell)
        {
            Gift_of_the_Naaru.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 75 && ItemsManager.GetItemCountByIdLUA(5512) > 0)
        {
            Logging.WriteFight("Using Healthstone.");
            nManager.Wow.Helpers.ItemsManager.UseItem("Healthstone");
            return;
        }

        if (ObjectManager.Me.HealthPercent < 65 && Dark_Regeneration.IsSpellUsable && Dark_Regeneration.KnownSpell)
        {
            Dark_Regeneration.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 50 && Ember_Tap.IsSpellUsable && Ember_Tap.KnownSpell)
        {
            Ember_Tap.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 35 && Drain_Life.KnownSpell &&
            Drain_Life.IsDistanceGood && Drain_Life.IsSpellUsable)
        {
            //SpellManager.CastSpellByIdLUA(689);
            Drain_Life.Launch();
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(200);
            }
            return;
        }
    }

    private void Resistance()
    {
        if (ObjectManager.Me.HealthPercent < 70 &&
            Unending_Resolve.KnownSpell && Unending_Resolve.IsSpellUsable)
        {
            Unending_Resolve.Launch();
        }

        if (ObjectManager.Me.HealthPercent < 40 &&
            Dark_Bargain.KnownSpell && Dark_Bargain.IsSpellUsable)
        {
            Dark_Bargain.Launch();
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
            Twilight_Ward.KnownSpell && Twilight_Ward.IsSpellUsable)
        {
            Twilight_Ward.Launch();
        }

        if (ObjectManager.Target.IsCast &&
            ObjectManager.Target.IsTargetingMe &&
            Command_Demon.KnownSpell && Command_Demon.IsSpellUsable && Command_Demon.IsDistanceGood)
        {
            Command_Demon.Launch();
        }
    }

    private void pet()
    {
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

        if (Grimoire_of_Sacrifice.KnownSpell && !Grimoire_of_Sacrifice.HaveBuff && Grimoire_of_Sacrifice.IsSpellUsable)
        {
            if ((ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0))
            {
                Logging.WriteFight(" - PET DEAD - ");
                if (Flames_of_Xoroth.KnownSpell && Flames_of_Xoroth.IsSpellUsable)
                {
                    Flames_of_Xoroth.Launch();
                    Thread.Sleep(200);
                }
                Summon_Felhunter.Launch();
                Thread.Sleep(200);
            }
            Grimoire_of_Sacrifice.Launch();
        }

        if ((ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0) && !Grimoire_of_Sacrifice.KnownSpell)
        {
            Logging.WriteFight(" - PET DEAD - ");
            if (Flames_of_Xoroth.KnownSpell && Flames_of_Xoroth.IsSpellUsable)
            {
                Flames_of_Xoroth.Launch();
                Thread.Sleep(200);
            }
            if (Summon_Felhunter.KnownSpell && Summon_Felhunter.IsSpellUsable)
            {
                Summon_Felhunter.Launch();
                return;
            }
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
