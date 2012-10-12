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
                    #region Mage Specialisation checking

                case WoWClass.Mage:
                    var Summon_Water_Elemental = new Spell("Summon Water Elemental");
                    var Arcane_Barrage = new Spell("Arcane Barrage");
                    var Pyroblast = new Spell("Pyroblast");
                    if (Summon_Water_Elemental.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            MessageBox.Show("There is no settings available for your Class/Specialisation.");
                        }
                        else
                        {
                            Logging.WriteFight("Loading Mage Frost class...");
                            new Mage_Frost();
                        }
                    }
                    if (Arcane_Barrage.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            MessageBox.Show("There is no settings available for your Class/Specialisation.");
                        }
                        else
                        {
                            Logging.WriteFight("Loading Arcane Mage class...");
                            new Mage_Arcane();
                        }
                    }
                    if (Pyroblast.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            MessageBox.Show("There is no settings available for your Class/Specialisation.");
                        }
                        else
                        {
                            Logging.WriteFight("Loading Mage Fire class...");
                            new Mage_Fire();
                        }
                    }
                    if (!Summon_Water_Elemental.KnownSpell && !Arcane_Barrage.KnownSpell && !Pyroblast.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            MessageBox.Show("There is no settings available for your Class/Specialisation.");
                        }
                        else
                        {
                            Logging.WriteFight("No specialisation detected.");
                            Logging.WriteFight("Loading Mage frost class...");
                            new Mage_Frost();
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

#region Mage

public class Mage_Frost
{
    #region InitializeSpell

    private Spell Arcane_Brilliance = new Spell("Arcane Brilliance");
    private Spell Arcane_Explosion = new Spell("Arcane Explosion");
    private Spell Blink = new Spell("Blink");
    private Spell Blizzard = new Spell("Blizzard");
    private Spell Cold_Snap = new Spell("Cold Snap");
    private Spell Cone_of_Cold = new Spell("Cone of Cold");
    private Spell Conjure_Mana_Gem = new Spell("Conjure Mana Gem");
    private Spell Conjure_Refreshment = new Spell("Conjure Refreshment");
    private Spell Counterspell = new Spell("Counterspell");
    private Spell Deep_Freeze = new Spell("Deep Freeze");
    private Spell Evocation = new Spell("Evocation");
    private Spell Fire_Blast = new Spell("Fire Blast");
    private Spell Flamestrike = new Spell("Flamestrike");
    private Spell Freeze = new Spell("Freeze");
    private Spell Frost_Armor = new Spell("Frost Armor");
    private Spell Frost_Bomb = new Spell("Frost Bomb");
    private Spell Frost_Jaw = new Spell("Frost Jaw");
    private Spell Frost_Nova = new Spell("Frost Nova");
    private Spell Frostbolt = new Spell("Frostbolt");
    private Spell Frostfire_Bolt = new Spell("Frostfire Bolt");
    private Spell Frozen_Orb = new Spell("Frozen Orb");
    private Spell Ice_Barrier = new Spell("Ice Barrier");
    private Spell Ice_Block = new Spell("Ice Block");
    private Spell Ice_Floes = new Spell("Ice Floes");
    private Spell Ice_Lance = new Spell("Ice Lance");
    private Spell Icy_Veins = new Spell("Icy Veins");
    private Spell Invisibility = new Spell("Invisibility");
    private Spell Living_Bomb = new Spell("Living Bomb");
    private Spell Mage_Armor = new Spell("Mage Armor");
    private Spell Mirror_Image = new Spell("Mirror Image");
    private Spell Molten_Armor = new Spell("Molten Armor");
    private Spell Nether_Tempest = new Spell("Nether Tempest");
    private Spell Presence_of_Mind = new Spell("Presence of Mind");
    private Spell Remove_Curse = new Spell("Remove Curse");
    private Spell Ring_of_Frost = new Spell("Ring of Frost");
    private Spell Scorch = new Spell("Scorch");
    private Spell Slow_Fall = new Spell("Slow Fall");
    private Spell Summon_Water_Elemental = new Spell("Summon Water Elemental");
    private Spell Temporal_Shield = new Spell("Temporal Shield");
    private Spell Time_Warp = new Spell("Time Warp");

    // TIMER
    private Timer Eating_Timer = new Timer(0);
    private Timer Nether_Tempest_Timer = new Timer(0);
    private Timer Living_Bomb_Timer = new Timer(0);
    private Timer Frost_Bomb_Timer = new Timer(0);
    private Timer Freeze_Timer = new Timer(0);
    int StartFight = 1;

    // Profession & Racials
    private Spell Arcane_Torrent = new Spell("Arcane Torrent");
    private Spell Berserking = new Spell("Berserking");
    private Spell Blood_Fury = new Spell("Blood Fury");
    private Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    private Spell Lifeblood = new Spell("Lifeblood");
    private Spell Stoneform = new Spell("Stoneform");

    #endregion InitializeSpell

    public Mage_Frost()
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
        Pet();
        if (Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(65499) == 0 && // 85
            Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(43523) == 0 && // 84-80
            Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(43518) == 0 && // 79-74
            Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(65517) == 0 && // 73-64
            Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(65516) == 0 && // 63-54
            Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(65515) == 0 && // 53-44
            Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(65500) == 0) // 43-38
        {
            Conjure_Refreshment.Launch();
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(200);
            }
            return;
        }

        if (Evocation.KnownSpell && Evocation.IsSpellUsable &&
            ObjectManager.Me.HealthPercent < 40 ||
            ObjectManager.Me.ManaPercentage < 40)
        {
            Evocation.Launch();
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(200);
            }
            return;
        }

        if (Conjure_Mana_Gem.KnownSpell && ItemsManager.GetItemCountByIdLUA(36799) == 0)
        {
            Conjure_Mana_Gem.Launch();
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(200);
            }
            return;
        }

        if (Frost_Armor.KnownSpell && Frost_Armor.IsSpellUsable && !Frost_Armor.HaveBuff)
        {
            Frost_Armor.Launch();
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(200);
            }
            return;
        }

        if (Molten_Armor.KnownSpell && Molten_Armor.IsSpellUsable && !Molten_Armor.HaveBuff && !Frost_Armor.KnownSpell)
        {
            Molten_Armor.Launch();
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(200);
            }
            return;
        }

        if (ObjectManager.Me.HealthPercent < 40 || ObjectManager.Me.ManaPercentage < 40)
        {
            if (ItemsManager.GetItemCountByIdLUA(65499) > 0) // 85
            {
                Lua.RunMacroText("/use item:65499");
                Eating_Timer = new Timer(1000 * 15);
                while (!ObjectManager.Me.InCombat || !Eating_Timer.IsReady)
                {
                    Thread.Sleep(200);
                }
                return;
            }
            
            if (ItemsManager.GetItemCountByIdLUA(43523) > 0)// 84-80
            {
                Lua.RunMacroText("/use item:43523");
                Eating_Timer = new Timer(1000 * 15);
                while (!ObjectManager.Me.InCombat || !Eating_Timer.IsReady)
                {
                    Thread.Sleep(200);
                }
                return;
            }

            if (ItemsManager.GetItemCountByIdLUA(43518) > 0) // 79-74
            {
                Lua.RunMacroText("/use item:43518");
                Eating_Timer = new Timer(1000 * 15);
                while (!ObjectManager.Me.InCombat || !Eating_Timer.IsReady)
                {
                    Thread.Sleep(200);
                }
                return;
            }

            if (ItemsManager.GetItemCountByIdLUA(65517) > 0) // 73-64
            {
                Lua.RunMacroText("/use item:65499");
                Eating_Timer = new Timer(1000 * 15);
                while (!ObjectManager.Me.InCombat || !Eating_Timer.IsReady)
                {
                    Thread.Sleep(200);
                }
                return;
            }

            if (ItemsManager.GetItemCountByIdLUA(65516) > 0) // 63-54
            {
                Lua.RunMacroText("/use item:65516");
                Eating_Timer = new Timer(1000 * 15);
                while (!ObjectManager.Me.InCombat || !Eating_Timer.IsReady)
                {
                    Thread.Sleep(200);
                }
                return;
            }

            if (ItemsManager.GetItemCountByIdLUA(65515) > 0) // 53-44
            {
                Lua.RunMacroText("/use item:65515");
                Eating_Timer = new Timer(1000 * 15);
                while (!ObjectManager.Me.InCombat || !Eating_Timer.IsReady)
                {
                    Thread.Sleep(200);
                }
                return;
            }

            if (ItemsManager.GetItemCountByIdLUA(65500) > 0) // 43-38
            {
                Lua.RunMacroText("/use item:65500");
                Eating_Timer = new Timer(1000 * 15);
                while (!ObjectManager.Me.InCombat || !Eating_Timer.IsReady)
                {
                    Thread.Sleep(200);
                }
                return;
            }
        }
    }

    public void Pull()
    {
        /*if (Inferno_Blast.KnownSpell && Inferno_Blast.IsDistanceGood && Inferno_Blast.IsSpellUsable)
        {
            Inferno_Blast.Launch();
            return;
        }*/

        if (Freeze.IsDistanceGood && Freeze.KnownSpell && Freeze.IsDistanceGood)
        {
            SpellManager.CastSpellByIDAndPosition(33395, ObjectManager.Target.Position);
            if (Deep_Freeze.IsSpellUsable && Deep_Freeze.KnownSpell && Deep_Freeze.IsDistanceGood)
            {
                Thread.Sleep(200);
                Deep_Freeze.Launch();
            }
        }
        StartFight--;
        return;
    }

    public void Buff()
    {
        if (Arcane_Brilliance.KnownSpell && Arcane_Brilliance.IsSpellUsable &&
            !Arcane_Brilliance.HaveBuff)
        {
            Arcane_Brilliance.Launch();
            return;
        }
    }

    public void LowCombat()
    {
        AvoidMelee();
        Pet();
        Heal();
        Resistance();
        Buff();

        if (Ice_Lance.IsDistanceGood && Ice_Lance.KnownSpell && Ice_Lance.IsSpellUsable
            && ObjectManager.Me.HaveBuff(44544))
        {
            Ice_Lance.Launch();
            return;
        }

        if (Frostbolt.KnownSpell && Frostbolt.IsSpellUsable && Frostbolt.IsDistanceGood)
        {
            Frostbolt.Launch();
            return;
        }

        if (ObjectManager.Target.HealthPercent > 90)
        {
            if (Arcane_Explosion.KnownSpell && Arcane_Explosion.IsSpellUsable && Arcane_Explosion.IsDistanceGood)
            {
                Arcane_Explosion.Launch();
                return;
            }
        }
    }

    public void Combat()
    {
        AvoidMelee();
        Pet();
        Decast();
        Heal();
        Resistance();
        Buff();
        if (StartFight == 1)
            Pull();

        if (ObjectManager.GetNumberAttackPlayer() > 4 && Flamestrike.IsSpellUsable && Flamestrike.KnownSpell
            && Flamestrike.IsDistanceGood)
        {
            SpellManager.CastSpellByIDAndPosition(2120, ObjectManager.Target.Position);
            return;
        }

        if (ObjectManager.GetNumberAttackPlayer() > 4 && Arcane_Explosion.IsSpellUsable && Arcane_Explosion.KnownSpell
            && Arcane_Explosion.IsDistanceGood)
        {
            Arcane_Explosion.Launch();
            return;
        }

        if (Frozen_Orb.KnownSpell && Frozen_Orb.IsSpellUsable && Frozen_Orb.IsDistanceGood)
        {
            Frozen_Orb.Launch();
            return;
        }

        /*if (Freeze.KnownSpell && Freeze.IsSpellUsable && Freeze.IsDistanceGood)
        {
            SpellManager.CastSpellByIDAndPosition(33395, ObjectManager.Target.Position);
            return;
        }*/

        if (ObjectManager.Me.Level > 9 && Freeze_Timer.IsReady && (ObjectManager.Pet.Health != 0 || ObjectManager.Pet.Guid != 0))
        {
            SpellManager.CastSpellByIDAndPosition(33395, ObjectManager.Target.Position);
            Freeze_Timer = new Timer(1000 * 25);
            return;
        }

        if (Frostfire_Bolt.IsDistanceGood && Frostfire_Bolt.KnownSpell && Frostfire_Bolt.IsSpellUsable
            && ObjectManager.Me.HaveBuff(57761))
        {
            Frostfire_Bolt.Launch();
            return;
        }

        if (Ice_Lance.IsDistanceGood && Ice_Lance.KnownSpell && Ice_Lance.IsSpellUsable
            && ObjectManager.Me.HaveBuff(44544))
        {
            Ice_Lance.Launch();
            return;
        }

        if (Icy_Veins.KnownSpell && Icy_Veins.IsSpellUsable)
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

            Icy_Veins.Launch();
            return;
        }

        if (Frostbolt.KnownSpell && Frostbolt.IsSpellUsable && Frostbolt.IsDistanceGood
            && Presence_of_Mind.KnownSpell && Presence_of_Mind.IsSpellUsable
            && !ObjectManager.Me.HaveBuff(48108))
        {
            Presence_of_Mind.Launch();
            Thread.Sleep(200);
            Frostbolt.Launch();
            return;
        }

        if (Nether_Tempest.KnownSpell && Nether_Tempest.IsSpellUsable && Nether_Tempest.IsDistanceGood
            && (!Nether_Tempest.TargetHaveBuff || Nether_Tempest_Timer.IsReady))
        {
            Nether_Tempest.Launch();
            Nether_Tempest_Timer = new Timer(1000 * 9);
            return;
        }

        else if (Living_Bomb.KnownSpell && Living_Bomb.IsSpellUsable && Living_Bomb.IsDistanceGood
            && !Living_Bomb.TargetHaveBuff)
        {
            Living_Bomb.Launch();
            return;
        }

        else if (Frost_Bomb.KnownSpell && Frost_Bomb.IsSpellUsable && Frost_Bomb.IsDistanceGood)
        {
            Frost_Bomb.Launch();
            return;
        }

        else
        {
            if (ObjectManager.Me.Level > 74)
            {
                if (Living_Bomb_Timer.IsReady)
                {
                    Logging.WriteFight("Cast Living Bomb.");
                    Lua.RunMacroText("/cast Living Bomb");
                    Living_Bomb_Timer = new Timer(1000 * 12);
                }
                if (Frost_Bomb_Timer.IsReady)
                {
                    Logging.WriteFight("Cast Frost Bomb.");
                    Lua.RunMacroText("/cast Frost Bomb");
                    Frost_Bomb_Timer = new Timer(1000 * 8);
                }
                if (Nether_Tempest_Timer.IsReady)
                {
                    Logging.WriteFight("Cast Nether Tempest.");
                    Lua.RunMacroText("/cast Nether Tempest");
                    Nether_Tempest_Timer = new Timer(1000 * 9);
                }
            }
        }


        if (Mirror_Image.KnownSpell && Mirror_Image.IsSpellUsable)
        {
            Mirror_Image.Launch();
            return;
        }

        if (Time_Warp.KnownSpell && Time_Warp.IsSpellUsable && !Time_Warp.HaveBuff && !ObjectManager.Me.HaveBuff(80354))
        {
            Time_Warp.Launch();
            return;
        }

        if (Frostbolt.KnownSpell && Frostbolt.IsSpellUsable && Frostbolt.IsDistanceGood)
        {
            Frostbolt.Launch();
            return;
        }
    }

    private void Resistance()
    {
        if (ObjectManager.Me.HealthPercent < 95 && Ice_Barrier.IsSpellUsable && Ice_Barrier.KnownSpell
            && !ObjectManager.Me.HaveBuff(11426))
        {
            Ice_Barrier.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 95 && Temporal_Shield.IsSpellUsable && Temporal_Shield.KnownSpell
            && !ObjectManager.Me.HaveBuff(11426))
        {
            Temporal_Shield.Launch();
            return;
        }

        if (Deep_Freeze.KnownSpell && Deep_Freeze.IsSpellUsable && Deep_Freeze.IsDistanceGood
            && ObjectManager.Me.HealthPercent < 50)
        {
            Deep_Freeze.Launch();
            return;
        }

        if (Frost_Nova.KnownSpell && ObjectManager.Target.GetDistance < 6
            && ObjectManager.Me.HealthPercent < 50)
        {
            if (!Frost_Nova.IsSpellUsable && Cold_Snap.KnownSpell && Cold_Snap.IsSpellUsable)
            {
                Cold_Snap.Launch();
                Thread.Sleep(200);
            }

            if (Frost_Nova.IsSpellUsable)
            {
                Frost_Nova.Launch();
                if (Blink.KnownSpell && Blink.IsSpellUsable)
                {
                    Thread.Sleep(200);
                    Blink.Launch();
                    return;
                }
            }
            return;
        }

        if (Cone_of_Cold.KnownSpell && Cone_of_Cold.IsSpellUsable && ObjectManager.Target.GetDistance < 11
            && ObjectManager.Me.HealthPercent < 40 && Blink.KnownSpell && Blink.IsSpellUsable && !Frost_Nova.IsSpellUsable)
        {
            Cone_of_Cold.Launch();
            Thread.Sleep(200);
            Blink.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 80 && Stoneform.IsSpellUsable && Stoneform.KnownSpell)
        {
            Stoneform.Launch();
            return;
        }

        if (Ring_of_Frost.KnownSpell && Ring_of_Frost.IsSpellUsable &&
            ObjectManager.GetNumberAttackPlayer() > 2 &&
            ObjectManager.Target.GetDistance < 10)
        {
            SpellManager.CastSpellByIDAndPosition(113724, ObjectManager.Target.Position);
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(200);
            }
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.ManaPercentage < 40 &&
            ItemsManager.GetItemCountByIdLUA(36799) > 0)
        {
            Lua.RunMacroText("/use item:36799");
            return;
        }

        if (ObjectManager.Me.ManaPercentage < 70 && Arcane_Torrent.KnownSpell && Arcane_Torrent.IsSpellUsable)
        {
            Arcane_Torrent.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 80 &&
            Gift_of_the_Naaru.KnownSpell && Gift_of_the_Naaru.IsSpellUsable)
        {
            Gift_of_the_Naaru.Launch();
            return;
        }

        if (Evocation.KnownSpell && Evocation.IsSpellUsable &&
            ObjectManager.Me.HealthPercent > 0 &&
            ObjectManager.Me.HealthPercent < 30 ||
            ObjectManager.Me.ManaPercentage < 20)
        {
            Evocation.Launch();
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(200);
            }
            return;
        }
    }

    private void Decast()
    {
        if (ObjectManager.Target.IsCast &&
            ObjectManager.Target.IsTargetingMe &&
            Counterspell.KnownSpell && Counterspell.IsSpellUsable && Counterspell.IsDistanceGood)
        {
            Counterspell.Launch();
            return;
        }

        if (ObjectManager.Target.IsCast &&
            ObjectManager.Target.IsTargetingMe &&
            Frost_Jaw.KnownSpell && Frost_Jaw.IsSpellUsable && Frost_Jaw.IsDistanceGood)
        {
            Frost_Jaw.Launch();
            return;
        }
    }

    private void Pet()
    {
        if (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0)
        {
            Logging.WriteFight(" - PET DEAD - ");
            if (Summon_Water_Elemental.KnownSpell && Summon_Water_Elemental.IsSpellUsable)
            {
                Summon_Water_Elemental.Launch();
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

public class Mage_Arcane
{
    #region InitializeSpell

    // Arcane Only
    private Spell Arcane_Barrage = new Spell("Arcane Barrage");
    private Spell Arcane_Blast = new Spell("Arcane Blast");
    private Spell Arcane_Explosion = new Spell("Arcane Explosion");
    private Spell Presence_of_Mind = new Spell("Presence of Mind");
    private Spell Slow = new Spell("Slow");

    // Survive
    private Spell Mana_Shield = new Spell("Mana Shield");
    private Spell Mage_Ward = new Spell("Mage Ward");
    private Spell Ring_of_Frost = new Spell("Ring of Frost");
    private Spell Frost_Nova = new Spell("Frost Nova");
    private Spell Blink = new Spell("Blink");
    private Spell Counterspell = new Spell("Counterspell");
    private Spell Frostbolt = new Spell("Frostbolt");

    // DPS
    private Spell Frostfire_Bolt = new Spell("Frostfire Bolt");
    private Spell Fireball = new Spell("Fireball");
    private Spell Flame_Orb = new Spell("Flame Orb");
    private Spell Fire_Blast = new Spell("Fire Blast");
    private Spell Arcane_Missiles = new Spell("Arcane Missiles");

    // BUFF & HELPING
    private Spell Evocation = new Spell("Evocation");
    private Spell Conjure_Refreshment = new Spell("Conjure Refreshment");
    private Spell Arcane_Brilliance = new Spell("Arcane Brilliance");
    private Spell Remove_Curse = new Spell("Remove Curse");
    private Spell Mage_Armor = new Spell("Mage Armor");
    private Spell Molten_Armor = new Spell("Molten Armor");
    private Spell Conjure_Mana_Gem = new Spell("Conjure Mana Gem");

    // BIG CD
    private Spell Mirror_Image = new Spell("Mirror Image");
    private Spell Time_Warp = new Spell("Time Warp");
    private Spell Invisibility = new Spell("Invisibility");
    private Spell Ice_Block = new Spell("Ice Block");
    private Spell Cold_Snap = new Spell("Cold Snap");

    // TIMER
    private Timer freeze = new Timer(0);
    private Timer look = new Timer(0);
    private Timer fighttimer = new Timer(0);
    private Timer waitfordebuff = new Timer(0);

    // Profession & Racials
    private Spell ArcaneTorrent = new Spell("Arcane Torrent");
    private Spell Lifeblood = new Spell("Lifeblood");
    private Spell Stoneform = new Spell("Stoneform");
    private Spell Tailoring = new Spell("Tailoring");
    private Spell Leatherworking = new Spell("Leatherworking");
    private Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    private Spell War_Stomp = new Spell("War Stomp");
    private Spell Berserking = new Spell("Berserking");

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
        if (ObjectManager.Target.GetDistance > 25 && !Slow.KnownSpell)
            SpellManager.CastSpellByIdLUA(116); //Frostbolt.Launch();
    }

    public void buffoutfight()
    {
        if (Fight.InFight || ObjectManager.Me.IsDeadMe) return;

        if (Evocation.KnownSpell && Evocation.IsSpellUsable &&
            ObjectManager.Me.HealthPercent < 40 ||
            ObjectManager.Me.ManaPercentage < 40)
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

        if (Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(65499) == 0 && // 85
            Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(43523) == 0 && // 84-80
            Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(43518) == 0 && // 79-74
            Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(65517) == 0 && // 73-64
            Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(65516) == 0 && // 63-54
            Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(65515) == 0 && // 53-44
            Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(65500) == 0) // 43-38
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

        if (Mirror_Image.KnownSpell && Mirror_Image.IsSpellUsable && hardmob() ||
            ObjectManager.GetNumberAttackPlayer() > 1)
        {
            SpellManager.CastSpellByIdLUA(55342);
            // Mirror_Image.Launch();
        }

        if (Berserking.KnownSpell && Berserking.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(1454);
            // Berserking.Launch();
        }

        if (Arcane_Blast.KnownSpell && Arcane_Blast.IsDistanceGood && Arcane_Blast.IsSpellUsable &&
            ObjectManager.Target.GetDistance > 8)
        {
            SpellManager.CastSpellByIdLUA(30451);
            // Arcane_Blast.Launch();
            return;
        }

        if (Arcane_Blast.KnownSpell && Arcane_Blast.IsDistanceGood && Arcane_Blast.IsSpellUsable &&
            !Arcane_Missiles.IsSpellUsable && !Arcane_Barrage.IsSpellUsable && !Fire_Blast.IsSpellUsable)
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

        if (Slow.KnownSpell && Slow.IsDistanceGood && Slow.IsSpellUsable && !Slow.TargetHaveBuff &&
            ObjectManager.Target.IsTargetingMe)
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

        if (ArcaneTorrent.KnownSpell && ArcaneTorrent.IsSpellUsable &&
            ObjectManager.Target.IsCast && ObjectManager.Target.GetDistance < 8)
        {
            ArcaneTorrent.Launch();
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

        if (ObjectManager.Me.ManaPercentage < 40 &&
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
            ObjectManager.Me.ManaPercentage < 20)
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
        if (((ObjectManager.Target.MaxHealth*100)/ObjectManager.Me.MaxHealth) > 140)
        {
            return true;
        }
        return false;
    }
}

public class Mage_Fire
{
    #region InitializeSpell

    private Spell Arcane_Brilliance = new Spell("Arcane Brilliance");
    private Spell Arcane_Explosion = new Spell("Arcane Explosion");
    private Spell Blink = new Spell("Blink");
    private Spell Blizzard = new Spell("Blizzard");
    private Spell Cold_Snap = new Spell("Cold Snap");
    private Spell Combustion = new Spell("Combustion");
    private Spell Cone_of_Cold = new Spell("Cone of Cold");
    private Spell Conjure_Mana_Gem = new Spell("Conjure Mana Gem");
    private Spell Conjure_Refreshment = new Spell("Conjure Refreshment");
    private Spell Counterspell = new Spell("Counterspell");
    private Spell Deep_Freeze = new Spell("Deep Freeze");
    private Spell Dragons_Breath = new Spell("Dragon's Breath");
    private Spell Evocation = new Spell("Evocation");
    private Spell Fireball = new Spell("Fireball");
    private Spell Flamestrike = new Spell("Flamestrike");
    private Spell Frost_Armor = new Spell("Frost Armor");
    private Spell Frost_Bomb = new Spell("Frost Bomb");
    private Spell Frost_Jaw = new Spell("Frost Jaw");
    private Spell Frost_Nova = new Spell("Frost Nova");
    private Spell Frostfire_Bolt = new Spell("Frostfire Bolt");
    private Spell Ice_Barrier = new Spell("Ice Barrier");
    private Spell Ice_Block = new Spell("Ice Block");
    private Spell Ice_Floes = new Spell("Ice Floes");
    private Spell Ice_Lance = new Spell("Ice Lance");
    private Spell Inferno_Blast = new Spell("Inferno Blast");
    private Spell Invisibility = new Spell("Invisibility");
    private Spell Living_Bomb = new Spell("Living Bomb");
    private Spell Mage_Armor = new Spell("Mage Armor");
    private Spell Mirror_Image = new Spell("Mirror Image");
    private Spell Molten_Armor = new Spell("Molten Armor");
    private Spell Nether_Tempest = new Spell("Nether Tempest");
    private Spell Presence_of_Mind = new Spell("Presence of Mind");
    private Spell Pyroblast = new Spell("Pyroblast");
    private Spell Remove_Curse = new Spell("Remove Curse");
    private Spell Ring_of_Frost = new Spell("Ring of Frost");
    private Spell Scorch = new Spell("Scorch");
    private Spell Slow_Fall = new Spell("Slow Fall");
    private Spell Temporal_Shield = new Spell("Temporal Shield");
    private Spell Time_Warp = new Spell("Time Warp");

    // TIMER
    private Timer Eating_Timer = new Timer(0);
    private Timer Nether_Tempest_Timer = new Timer(0);
    private Timer Living_Bomb_Timer = new Timer(0);
    private Timer Frost_Bomb_Timer = new Timer(0);
    private Timer Inferno_Blast_Timer = new Timer(0);
    int StartFight = 1;

    // Profession & Racials
    private Spell Arcane_Torrent = new Spell("Arcane Torrent");
    private Spell Berserking = new Spell("Berserking");
    private Spell Blood_Fury = new Spell("Blood Fury");
    private Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    private Spell Lifeblood = new Spell("Lifeblood");
    private Spell Stoneform = new Spell("Stoneform");

    #endregion InitializeSpell

    public Mage_Fire()
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
        if (Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(65499) == 0 && // 85
            Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(43523) == 0 && // 84-80
            Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(43518) == 0 && // 79-74
            Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(65517) == 0 && // 73-64
            Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(65516) == 0 && // 63-54
            Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(65515) == 0 && // 53-44
            Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(65500) == 0) // 43-38
        {
            Conjure_Refreshment.Launch();
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(200);
            }
            return;
        }

        if (Evocation.KnownSpell && Evocation.IsSpellUsable &&
            ObjectManager.Me.HealthPercent < 40 ||
            ObjectManager.Me.ManaPercentage < 40)
        {
            Evocation.Launch();
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(200);
            }
            return;
        }

        if (Conjure_Mana_Gem.KnownSpell && ItemsManager.GetItemCountByIdLUA(36799) == 0)
        {
            Conjure_Mana_Gem.Launch();
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(200);
            }
        }

        if (Molten_Armor.KnownSpell && Molten_Armor.IsSpellUsable && !Molten_Armor.HaveBuff)
        {
            Molten_Armor.Launch();
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(200);
            }
        }

        if (ObjectManager.Me.HealthPercent < 40 || ObjectManager.Me.ManaPercentage < 40)
        {
            if (ItemsManager.GetItemCountByIdLUA(65499) > 0) // 85
            {
                Lua.RunMacroText("/use item:65499");
                Eating_Timer = new Timer(1000 * 15);
                while (!ObjectManager.Me.InCombat || !Eating_Timer.IsReady)
                {
                    Thread.Sleep(200);
                }
                return;
            }
            
            if (ItemsManager.GetItemCountByIdLUA(43523) > 0)// 84-80
            {
                Lua.RunMacroText("/use item:43523");
                Eating_Timer = new Timer(1000 * 15);
                while (!ObjectManager.Me.InCombat || !Eating_Timer.IsReady)
                {
                    Thread.Sleep(200);
                }
                return;
            }

            if (ItemsManager.GetItemCountByIdLUA(43518) > 0) // 79-74
            {
                Lua.RunMacroText("/use item:43518");
                Eating_Timer = new Timer(1000 * 15);
                while (!ObjectManager.Me.InCombat || !Eating_Timer.IsReady)
                {
                    Thread.Sleep(200);
                }
                return;
            }

            if (ItemsManager.GetItemCountByIdLUA(65517) > 0) // 73-64
            {
                Lua.RunMacroText("/use item:65499");
                Eating_Timer = new Timer(1000 * 15);
                while (!ObjectManager.Me.InCombat || !Eating_Timer.IsReady)
                {
                    Thread.Sleep(200);
                }
                return;
            }

            if (ItemsManager.GetItemCountByIdLUA(65516) > 0) // 63-54
            {
                Lua.RunMacroText("/use item:65516");
                Eating_Timer = new Timer(1000 * 15);
                while (!ObjectManager.Me.InCombat || !Eating_Timer.IsReady)
                {
                    Thread.Sleep(200);
                }
                return;
            }

            if (ItemsManager.GetItemCountByIdLUA(65515) > 0) // 53-44
            {
                Lua.RunMacroText("/use item:65515");
                Eating_Timer = new Timer(1000 * 15);
                while (!ObjectManager.Me.InCombat || !Eating_Timer.IsReady)
                {
                    Thread.Sleep(200);
                }
                return;
            }

            if (ItemsManager.GetItemCountByIdLUA(65500) > 0) // 43-38
            {
                Lua.RunMacroText("/use item:65500");
                Eating_Timer = new Timer(1000 * 15);
                while (!ObjectManager.Me.InCombat || !Eating_Timer.IsReady)
                {
                    Thread.Sleep(200);
                }
                return;
            }
        }
    }

    public void Pull()
    {
        /*if (Inferno_Blast.KnownSpell && Inferno_Blast.IsDistanceGood && Inferno_Blast.IsSpellUsable)
        {
            Inferno_Blast.Launch();
            return;
        }*/

        if (ObjectManager.Me.Level > 23 && Fireball.IsDistanceGood && Inferno_Blast_Timer.IsReady)
        {
            Logging.WriteFight("Cast Inferno Blast.");
            Lua.RunMacroText("/cast Inferno Blast");
            Inferno_Blast_Timer = new Timer(1000 * 8);
        }
        StartFight--;
            return;
    }

    public void Buff()
    {
        if (Arcane_Brilliance.KnownSpell && Arcane_Brilliance.IsSpellUsable &&
            !Arcane_Brilliance.HaveBuff)
        {
            Arcane_Brilliance.Launch();
            return;
        }
    }

    public void LowCombat()
    {
        AvoidMelee();
        Heal();
        Resistance();
        Buff();

        /*if (Inferno_Blast.KnownSpell && Inferno_Blast.IsSpellUsable && Inferno_Blast.IsDistanceGood)
        {
            Inferno_Blast.Launch();
            return;
        }*/
        if (Pyroblast.KnownSpell && Pyroblast.IsSpellUsable && Pyroblast.IsDistanceGood
            && ObjectManager.Me.HaveBuff(48108))
        {
            Pyroblast.Launch();
            return;
        }

        if (ObjectManager.Me.Level > 23 && Fireball.IsDistanceGood && Inferno_Blast_Timer.IsReady)
        {
            Logging.WriteFight("Cast Inferno Blast.");
            Lua.RunMacroText("/cast Inferno Blast");
            Inferno_Blast_Timer = new Timer(1000 * 8);
            return;
        }

        if (Fireball.KnownSpell && Fireball.IsSpellUsable && Fireball.IsDistanceGood)
        {
            Fireball.Launch();
            return;
        }

        if (ObjectManager.Target.HealthPercent > 90)
        {
            if (Arcane_Explosion.KnownSpell && Arcane_Explosion.IsSpellUsable && Arcane_Explosion.IsDistanceGood)
            {
                Arcane_Explosion.Launch();
                return;
            }
        }
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

        if (ObjectManager.GetNumberAttackPlayer() > 4 && Flamestrike.IsSpellUsable && Flamestrike.KnownSpell
            && Flamestrike.IsDistanceGood)
        {
            SpellManager.CastSpellByIDAndPosition(2120, ObjectManager.Target.Position);
            return;
        }

        if (ObjectManager.GetNumberAttackPlayer() > 4 && Arcane_Explosion.IsSpellUsable && Arcane_Explosion.KnownSpell
            && Arcane_Explosion.IsDistanceGood)
        {
            Arcane_Explosion.Launch();
            return;
        }

        if (Combustion.KnownSpell && Combustion.IsDistanceGood && Combustion.IsSpellUsable
            && ObjectManager.Target.HaveBuff(12654) && ObjectManager.Target.HaveBuff(11366))
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

            Combustion.Launch();
            return;
        }

        if (Pyroblast.KnownSpell && Pyroblast.IsSpellUsable && Pyroblast.IsDistanceGood
            && ObjectManager.Me.HaveBuff(48108))
        {
            Pyroblast.Launch();
            return;
        }

        if (Pyroblast.KnownSpell && Pyroblast.IsSpellUsable && Pyroblast.IsDistanceGood
            && Presence_of_Mind.KnownSpell && Presence_of_Mind.IsSpellUsable
            && !ObjectManager.Me.HaveBuff(48108))
        {
            Presence_of_Mind.Launch();
            Thread.Sleep(200);
            Pyroblast.Launch();
            return;
        }

        /*if (Inferno_Blast.KnownSpell && Inferno_Blast.IsSpellUsable && Inferno_Blast.IsDistanceGood
            && ObjectManager.Me.HaveBuff(48107))
        {
            Inferno_Blast.Launch();
            return;
        }*/

        if (ObjectManager.Me.Level > 23 && Fireball.IsDistanceGood && ObjectManager.Me.HaveBuff(48107) && Inferno_Blast_Timer.IsReady)
        {
            Logging.WriteFight("Cast Inferno Blast.");
            Lua.RunMacroText("/cast Inferno Blast");
            Inferno_Blast_Timer = new Timer(1000 * 8);
            return;
        }

        if (Nether_Tempest.KnownSpell && Nether_Tempest.IsSpellUsable && Nether_Tempest.IsDistanceGood
            && (!Nether_Tempest.TargetHaveBuff || Nether_Tempest_Timer.IsReady))
        {
            Nether_Tempest.Launch();
            Nether_Tempest_Timer = new Timer(1000 * 9);
            return;
        }

        else if (Living_Bomb.KnownSpell && Living_Bomb.IsSpellUsable && Living_Bomb.IsDistanceGood
            && !Living_Bomb.TargetHaveBuff)
        {
            Living_Bomb.Launch();
            return;
        }

        else if (Frost_Bomb.KnownSpell && Frost_Bomb.IsSpellUsable && Frost_Bomb.IsDistanceGood)
        {
            Frost_Bomb.Launch();
            return;
        }

        else
        {
            if (ObjectManager.Me.Level > 74)
            {
                if (Living_Bomb_Timer.IsReady)
                {
                    Logging.WriteFight("Cast Living Bomb.");
                    Lua.RunMacroText("/cast Living Bomb");
                    Living_Bomb_Timer = new Timer(1000 * 12);
                }
                if (Frost_Bomb_Timer.IsReady)
                {
                    Logging.WriteFight("Cast Frost Bomb.");
                    Lua.RunMacroText("/cast Frost Bomb");
                    Frost_Bomb_Timer = new Timer(1000 * 8);
                }
                if (Nether_Tempest_Timer.IsReady)
                {
                    Logging.WriteFight("Cast Nether Tempest.");
                    Lua.RunMacroText("/cast Nether Tempest");
                    Nether_Tempest_Timer = new Timer(1000 * 9);
                }
            }
        }


        if (Mirror_Image.KnownSpell && Mirror_Image.IsSpellUsable)
        {
            Mirror_Image.Launch();
            return;
        }
        if (Time_Warp.KnownSpell && Time_Warp.IsSpellUsable && !Time_Warp.HaveBuff && !ObjectManager.Me.HaveBuff(80354))
        {
            Time_Warp.Launch();
            return;
        }

        if (Fireball.KnownSpell && Fireball.IsSpellUsable && Fireball.IsDistanceGood)
        {
            Fireball.Launch();
            return;
        }
    }

    private void Resistance()
    {
        if (ObjectManager.Me.HealthPercent < 95 && Ice_Barrier.IsSpellUsable && Ice_Barrier.KnownSpell
            && !ObjectManager.Me.HaveBuff(11426))
        {
            Ice_Barrier.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 95 && Temporal_Shield.IsSpellUsable && Temporal_Shield.KnownSpell
            && !ObjectManager.Me.HaveBuff(11426))
        {
            Temporal_Shield.Launch();
            return;
        }

        if (Deep_Freeze.KnownSpell && Deep_Freeze.IsSpellUsable && Deep_Freeze.IsDistanceGood
            && ObjectManager.Me.HealthPercent < 50)
        {
            Deep_Freeze.Launch();
            return;
        }

        if (Frost_Nova.KnownSpell && ObjectManager.Target.GetDistance < 6
            && ObjectManager.Me.HealthPercent < 50)
        {
            if (!Frost_Nova.IsSpellUsable && Cold_Snap.KnownSpell && Cold_Snap.IsSpellUsable)
            {
                Cold_Snap.Launch();
                Thread.Sleep(200);
            }

            if (Frost_Nova.IsSpellUsable)
            {
                Frost_Nova.Launch();
                if (Blink.KnownSpell && Blink.IsSpellUsable)
                {
                    Thread.Sleep(200);
                    Blink.Launch();
                    return;
                }
            }
            return;
        }

        if (Cone_of_Cold.KnownSpell && Cone_of_Cold.IsSpellUsable && ObjectManager.Target.GetDistance < 11
            && ObjectManager.Me.HealthPercent < 40 && Blink.KnownSpell && Blink.IsSpellUsable && !Frost_Nova.IsSpellUsable)
        {
            Cone_of_Cold.Launch();
            Thread.Sleep(200);
            Blink.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 80 && Stoneform.IsSpellUsable && Stoneform.KnownSpell)
        {
            Stoneform.Launch();
            return;
        }

        if (Ring_of_Frost.KnownSpell && Ring_of_Frost.IsSpellUsable &&
            ObjectManager.GetNumberAttackPlayer() > 2 &&
            ObjectManager.Target.GetDistance < 10)
        {
            SpellManager.CastSpellByIDAndPosition(113724, ObjectManager.Target.Position);
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(200);
            }
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.ManaPercentage < 40 &&
            ItemsManager.GetItemCountByIdLUA(36799) > 0)
        {
            Lua.RunMacroText("/use item:36799");
            return;
        }

        if (ObjectManager.Me.ManaPercentage < 70 && Arcane_Torrent.KnownSpell && Arcane_Torrent.IsSpellUsable)
        {
            Arcane_Torrent.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 80 &&
            Gift_of_the_Naaru.KnownSpell && Gift_of_the_Naaru.IsSpellUsable)
        {
            Gift_of_the_Naaru.Launch();
            return;
        }

        if (Evocation.KnownSpell && Evocation.IsSpellUsable &&
            ObjectManager.Me.HealthPercent > 0 &&
            ObjectManager.Me.HealthPercent < 30 ||
            ObjectManager.Me.ManaPercentage < 20)
        {
            Evocation.Launch();
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(200);
            }
            return;
        }
    }

    private void Decast()
    {
        if (ObjectManager.Target.IsCast &&
            ObjectManager.Target.IsTargetingMe &&
            Counterspell.KnownSpell && Counterspell.IsSpellUsable && Counterspell.IsDistanceGood)
        {
            Counterspell.Launch();
            return;
        }

        if (ObjectManager.Target.IsCast &&
            ObjectManager.Target.IsTargetingMe &&
            Frost_Jaw.KnownSpell && Frost_Jaw.IsSpellUsable && Frost_Jaw.IsDistanceGood)
        {
            Frost_Jaw.Launch();
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

#endregion
