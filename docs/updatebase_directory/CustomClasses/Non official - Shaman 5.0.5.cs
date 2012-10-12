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
                    #region Shaman Specialisation checking

                case WoWClass.Shaman:
                    var Thunderstorm = new Spell("Thunderstorm");
                    var Lava_Lash = new Spell("Lava Lash");
                    if (Thunderstorm.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            MessageBox.Show("There is no settings available for your Class/Specialisation.");
                        }
                        else
                        {
                            Logging.WriteFight("Ele Shaman Found");
                            new Ele();
                        }
                    }

                    else if (Lava_Lash.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            MessageBox.Show("There is no settings available for your Class/Specialisation.");
                        }
                        else
                        {
                            Logging.WriteFight("Enh Shaman Found");
                            new Enh();
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
                            Logging.WriteFight("Shaman without Spec");
                            new Ele();
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

#region Shaman

public class Ele
{
    #region InitializeSpell

    private Spell Ancestral_Spirit = new Spell("Ancestral Spirit");
    private Spell Astral_Recall = new Spell("Astral Recall");
    private Spell Astral_Shift = new Spell("Astral Shift");
    private Spell Ancestral_Guidance = new Spell("Ancestral Guidance");
    private Spell Ancestral_Swiftness = new Spell("Ancestral Swiftness");
    private Spell Bind_Elemental = new Spell("Bind Elemental");
    private Spell Bloodlust = new Spell("Bloodlust");
    private Spell Call_of_the_Elements = new Spell("Call of the Elements");
    private Spell Capacitor_Totem = new Spell("Capacitor Totem");
    private Spell Chain_Heal = new Spell("Chain Heal");
    private Spell Chain_Lightning = new Spell("Chain Lightning");
    private Spell Cleanse_Spirit = new Spell("Cleanse Spirit");
    private Spell Earthquake = new Spell("Earthquake");
    private Spell Earth_Elemental_Totem = new Spell("Earth Elemental Totem");
    private Spell Earth_Shock = new Spell("Earth Shock");
    private Spell Earthbind_Totem = new Spell("Earthbind Totem");
    private Spell Elemental_Mastery = new Spell("Elemental Mastery");
    private Spell Fire_Elemental_Totem = new Spell("Fire Elemental Totem");
    private Spell Flame_Shock = new Spell("Flame Shock");
    private Spell Flametongue_Weapon = new Spell("Flametongue Weapon");
    private Spell Frostbrand_Weapon = new Spell("Frostbrand Weapon");
    private Spell Frost_Shock = new Spell("Frost Shock");
    private Spell Ghost_Wolf = new Spell("Ghost Wolf");
    private Spell Grounding_Totem = new Spell("Grounding Totem");
    private Spell Healing_Rain = new Spell("Healing Rain");
    private Spell Healing_Surge = new Spell("Healing Surge");
    private Spell Healing_Stream_Totem = new Spell("Healing Stream Totem");
    private Spell Healing_Tide_Totem = new Spell("Healing Tide Totem");
    private Spell Heroism = new Spell("Heroism");
    private Spell Hex = new Spell("Hex");
    private Spell Lava_Burst = new Spell("Lava Burst");
    private Spell Lightning_Bolt = new Spell("Lightning Bolt");
    private Spell Lightning_Shield = new Spell("Lightning Shield");
    private Spell Magma_Totem = new Spell("Magma Totem");
    private Spell Primal_Strike = new Spell("Primal Strike");
    private Spell Purge = new Spell("Purge");
    private Spell Rockbiter_Weapon = new Spell("Rockbiter Weapon");
    private Spell Searing_Totem = new Spell("Searing Totem");
    private Spell Spiritwalkers_Grace = new Spell("Spiritwalker's Grace");
    private Spell Stone_Bulwark_Totem = new Spell("Stone Bulwark Totem");
    private Spell Stormlash_Totem = new Spell("Stormlash Totem");
    private Spell Thunderstorm = new Spell("Thunderstorm");
    private Spell Totemic_Recall = new Spell("Totemic Recall");
    private Spell Tremor_Totem = new Spell("Tremor Totem");
    private Spell Unleash_Elements = new Spell("Unleash Elements");
    private Spell Water_Shield = new Spell("Water Shield");
    private Spell Water_Walking = new Spell("Water Walking");
    private Spell Windwalk_Totem = new Spell("Windwalk Totem");
    private Spell Wind_Shear = new Spell("Wind Shear");

    // TIMER
    private Timer Flame_Shock_Timer = new Timer(0);
    private Timer Water_Walking_Timer = new Timer(0);
    private Timer Flametongue_Weapon_Timer = new Timer(0);
    private Timer Air_Totem_Timer = new Timer(0);
    private Timer Earth_Totem_Timer = new Timer(0);
    private Timer Fire_Totem_Timer = new Timer(0);
    private Timer Water_Totem_Timer = new Timer(0);
    int StartFight = 1;
    int LSCounter = 0;

    // Profession & Racials
    private Spell Arcane_Torrent = new Spell("Arcane Torrent");
    private Spell Berserking = new Spell("Berserking");
    private Spell Blood_Fury = new Spell("Blood Fury");
    private Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    private Spell Lifeblood = new Spell("Lifeblood");
    private Spell Stoneform = new Spell("Stoneform");

    #endregion InitializeSpell

    public Ele()
    {
        Main.range = 30.0f;
        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            OutCombatBuff();
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

    public void OutCombatBuff()
    {
        if (Water_Walking.IsSpellUsable && Water_Walking.KnownSpell && (!Water_Walking.HaveBuff || Water_Walking_Timer.IsReady)
            && ObjectManager.Target.IsDead)
        {
            Logging.WriteFight("OutofCombat Water Walking.");
            Water_Walking.Launch();
            Water_Walking_Timer = new Timer(1000 * 60 * 9);
            return;
        }

        if (ObjectManager.Me.HealthPercent < 100 && Healing_Surge.KnownSpell && Healing_Surge.IsSpellUsable
            && ObjectManager.Target.IsDead)
        {
            Logging.WriteFight("OutofCombat Healing Surge.");
            Healing_Surge.Launch();
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(200);
            }
            return;
        }
    }

    public void Patrolling()
    {
        if (Flametongue_Weapon.KnownSpell && Flametongue_Weapon.IsSpellUsable && Flametongue_Weapon_Timer.IsReady)
        {
            Flametongue_Weapon.Launch();
            Flametongue_Weapon_Timer = new Timer(1000 * 60 * 60);
            return;
        }

        if (!ObjectManager.Me.IsMounted)
        {
            Buff();
        }
    }

    public void Pull()
    {
        if (Flame_Shock.KnownSpell && Flame_Shock.IsSpellUsable && Flame_Shock.IsDistanceGood)
        {
            Flame_Shock.Launch();
            return;
        }
        StartFight--;
    }

    public void Buff()
    {
        if (ObjectManager.Me.BarTwoPercentage < 15 && Water_Shield.KnownSpell && Water_Shield.IsSpellUsable)
        {
            if (Water_Shield.KnownSpell && Water_Shield.IsSpellUsable && !Water_Shield.HaveBuff)
            {
                Water_Shield.Launch();
                return;
            }
        }

        else
        {
            if (Lightning_Shield.KnownSpell && Lightning_Shield.IsSpellUsable && !Lightning_Shield.HaveBuff)
            {
                Lightning_Shield.Launch();
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

        if (Earth_Shock.KnownSpell && Earth_Shock.IsSpellUsable && Earth_Shock.IsDistanceGood)
        {
            Earth_Shock.Launch();
            return;
        }

        if (Lava_Burst.KnownSpell && Lava_Burst.IsSpellUsable && Lava_Burst.IsDistanceGood)
        {
            Lava_Burst.Launch();
            return;
        }

        if (Chain_Lightning.KnownSpell && Chain_Lightning.IsSpellUsable && Chain_Lightning.IsDistanceGood)
        {
            Chain_Lightning.Launch();
            return;
        }

        if (Searing_Totem.KnownSpell && Searing_Totem.IsSpellUsable && Fire_Totem_Timer.IsReady)
        {
            Searing_Totem.Launch();
            Fire_Totem_Timer = new Timer(1000 * 57);
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
        if (StartFight == 1)
            Pull();

        if (ObjectManager.Me.BarTwoPercentage < 70 && Arcane_Torrent.KnownSpell && Arcane_Torrent.IsSpellUsable)
        {
            Arcane_Torrent.Launch();
            return;
        }

        if (ObjectManager.Me.BarTwoPercentage < 80 && Thunderstorm.KnownSpell && Thunderstorm.IsSpellUsable)
        {
            Thunderstorm.Launch();
            return;
        }

        if (Stormlash_Totem.KnownSpell && Stormlash_Totem.IsSpellUsable && !ObjectManager.Me.HaveBuff(8178) 
            && Air_Totem_Timer.IsReady)
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
            Stormlash_Totem.Launch();
            Air_Totem_Timer = new Timer(1000 * 10);
            return;
        }
        
        if (Call_of_the_Elements.KnownSpell && Call_of_the_Elements.IsSpellUsable && !Stormlash_Totem.IsSpellUsable
            && !ObjectManager.Me.HaveBuff(120676))
        {
            Call_of_the_Elements.Launch();
            return;
        }

        if (Heroism.KnownSpell && Heroism.IsSpellUsable && !ObjectManager.Me.HaveBuff(57724))
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
            Heroism.Launch();
            return;
        }

        if (Bloodlust.KnownSpell && Bloodlust.IsSpellUsable && !ObjectManager.Me.HaveBuff(57724))
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
            Bloodlust.Launch();
            return;
        }

        if (Elemental_Mastery.KnownSpell && Elemental_Mastery.IsSpellUsable &&
            !ObjectManager.Me.HaveBuff(2825))
        {
            Elemental_Mastery.Launch();
            return;
        }

        if (Earth_Elemental_Totem.KnownSpell && Earth_Elemental_Totem.IsSpellUsable &&
            ObjectManager.GetNumberAttackPlayer() > 3)
        {
            Earth_Elemental_Totem.Launch();
            Earth_Totem_Timer = new Timer(1000 * 60);
            return;
        }

        if (Thunderstorm.KnownSpell && Thunderstorm.IsSpellUsable && ObjectManager.Target.GetDistance < 10
            && ObjectManager.GetNumberAttackPlayer() > 5)
        {
            Thunderstorm.Launch();
            return;
        }

        if (Earthquake.KnownSpell && Earthquake.IsSpellUsable && Earthquake.IsDistanceGood
            && ObjectManager.GetNumberAttackPlayer() > 5)
        {
            SpellManager.CastSpellByIDAndPosition(61882, ObjectManager.Target.Position);
            return;
        }

        if (Fire_Elemental_Totem.KnownSpell && Fire_Elemental_Totem.IsSpellUsable)
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
            Fire_Elemental_Totem.Launch();
            Fire_Totem_Timer = new Timer(1000 * 60);
            return;
        }

        if (Flame_Shock.IsSpellUsable && Flame_Shock.IsDistanceGood && Flame_Shock.KnownSpell
            && (!Flame_Shock.TargetHaveBuff || Flame_Shock_Timer.IsReady))
        {
            Flame_Shock.Launch();
            Flame_Shock_Timer = new Timer(1000 * 27);
            return;
        }

        if (Lava_Burst.KnownSpell && Lava_Burst.IsSpellUsable && Lava_Burst.IsDistanceGood && Flame_Shock.TargetHaveBuff)
        {
            Lava_Burst.Launch();
            return;
        }

        if (Earth_Shock.IsSpellUsable && Earth_Shock.KnownSpell && Earth_Shock.IsDistanceGood
            && LSCounter > 5)
        {
            if (Flame_Shock.KnownSpell && (!Flame_Shock.TargetHaveBuff || Flame_Shock_Timer.IsReady)) 
                return;
            Earth_Shock.Launch();
            LSCounter = 0;
            return;
        }

        if (ObjectManager.GetNumberAttackPlayer() > 1)
        {
            if (Magma_Totem.KnownSpell && Magma_Totem.IsSpellUsable && Fire_Totem_Timer.IsReady)
            {
                Magma_Totem.Launch();
                Fire_Totem_Timer = new Timer(1000 * 57);
                return;
            }
        }

        else
        {
            if (Searing_Totem.KnownSpell && Searing_Totem.IsSpellUsable && Fire_Totem_Timer.IsReady)
            {
                Searing_Totem.Launch();
                Fire_Totem_Timer = new Timer(1000 * 57);
                return;
            }
        }

        if (ObjectManager.GetNumberAttackPlayer() > 1)
        {
            if (Chain_Lightning.KnownSpell && Chain_Lightning.IsSpellUsable && Chain_Lightning.IsDistanceGood)
            {
                if (Ancestral_Swiftness.KnownSpell && Ancestral_Swiftness.IsSpellUsable)
                {
                    Ancestral_Swiftness.Launch();
                    Thread.Sleep(200);
                }
                Chain_Lightning.Launch();
                LSCounter += 2;
                return;
            }
        }

        else
        {
            if (Lightning_Bolt.IsDistanceGood && Lightning_Bolt.KnownSpell && Lightning_Bolt.IsSpellUsable)
            {
                if (Ancestral_Swiftness.KnownSpell && Ancestral_Swiftness.IsSpellUsable)
                {
                    Ancestral_Swiftness.Launch();
                    Thread.Sleep(200);
                }
                Lightning_Bolt.Launch();
                LSCounter++;
                return;
            }
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.HealthPercent < 80 && Gift_of_the_Naaru.KnownSpell && Gift_of_the_Naaru.IsSpellUsable)
        {
            Gift_of_the_Naaru.Launch();
            return;
        }

        if (Healing_Stream_Totem.KnownSpell && Healing_Stream_Totem.IsSpellUsable && ObjectManager.Me.HealthPercent < 90
            && Water_Totem_Timer.IsReady)
        {
            Healing_Stream_Totem.Launch();
            Water_Totem_Timer = new Timer(1000 * 15);
            return;
        }

        if (Healing_Tide_Totem.KnownSpell && Healing_Tide_Totem.IsSpellUsable && ObjectManager.Me.HealthPercent < 70
            && Water_Totem_Timer.IsReady)
        {
            Healing_Tide_Totem.Launch();
            Water_Totem_Timer = new Timer(1000 * 15);
            return;
        }

        if (Ancestral_Guidance.KnownSpell && Ancestral_Guidance.IsSpellUsable && ObjectManager.Me.HealthPercent < 70)
        {
            Ancestral_Guidance.Launch();
            return;
        }

        if (Healing_Surge.KnownSpell && Healing_Surge.IsSpellUsable && ObjectManager.Me.HealthPercent < 50)
        {
            Healing_Surge.Launch();
            return;
        }
    }

    private void Resistance()
    {
        if (ObjectManager.Me.HealthPercent < 50 && Capacitor_Totem.KnownSpell && Capacitor_Totem.IsSpellUsable
            && Air_Totem_Timer.IsReady)
        {
            Capacitor_Totem.Launch();
            Air_Totem_Timer = new Timer(1000 * 5);
            return;
        }

        if (ObjectManager.Me.HealthPercent < 70 && Astral_Shift.KnownSpell && Astral_Shift.IsSpellUsable)
        {
            Astral_Shift.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 50 && Stone_Bulwark_Totem.KnownSpell && Stone_Bulwark_Totem.IsSpellUsable
            && Earth_Totem_Timer.IsReady)
        {
            Stone_Bulwark_Totem.Launch();
            Earth_Totem_Timer = new Timer(1000 * 30);
            return;
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
            Wind_Shear.KnownSpell && Wind_Shear.IsSpellUsable && Wind_Shear.IsDistanceGood)
        {
            Wind_Shear.Launch();
            return;
        }
        
        if (ObjectManager.Target.IsCast &&
            ObjectManager.Target.IsTargetingMe &&
            Grounding_Totem.KnownSpell && Grounding_Totem.IsSpellUsable && Air_Totem_Timer.IsReady)
        {
            Grounding_Totem.Launch();
            Air_Totem_Timer = new Timer(1000 * 15);
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

public class Enh
{
    #region InitializeSpell

    private Spell Ancestral_Spirit = new Spell("Ancestral Spirit");
    private Spell Astral_Recall = new Spell("Astral Recall");
    private Spell Astral_Shift = new Spell("Astral Shift");
    private Spell Ancestral_Guidance = new Spell("Ancestral Guidance");
    private Spell Ancestral_Swiftness = new Spell("Ancestral Swiftness");
    private Spell Bind_Elemental = new Spell("Bind Elemental");
    private Spell Bloodlust = new Spell("Bloodlust");
    private Spell Call_of_the_Elements = new Spell("Call of the Elements");
    private Spell Capacitor_Totem = new Spell("Capacitor Totem");
    private Spell Chain_Heal = new Spell("Chain Heal");
    private Spell Chain_Lightning = new Spell("Chain Lightning");
    private Spell Cleanse_Spirit = new Spell("Cleanse Spirit");
    private Spell Earth_Elemental_Totem = new Spell("Earth Elemental Totem");
    private Spell Earth_Shock = new Spell("Earth Shock");
    private Spell Earthbind_Totem = new Spell("Earthbind Totem");
    private Spell Elemental_Mastery = new Spell("Elemental Mastery");
    private Spell Feral_Spirit = new Spell("Feral Spirit");
    private Spell Fire_Elemental_Totem = new Spell("Fire Elemental Totem");
    private Spell Fire_Nova = new Spell("Fire Nova");
    private Spell Flame_Shock = new Spell("Flame Shock");
    private Spell Flametongue_Weapon = new Spell("Flametongue Weapon");
    private Spell Frostbrand_Weapon = new Spell("Frostbrand Weapon");
    private Spell Frost_Shock = new Spell("Frost Shock");
    private Spell Ghost_Wolf = new Spell("Ghost Wolf");
    private Spell Grounding_Totem = new Spell("Grounding Totem");
    private Spell Healing_Rain = new Spell("Healing Rain");
    private Spell Healing_Surge = new Spell("Healing Surge");
    private Spell Healing_Stream_Totem = new Spell("Healing Stream Totem");
    private Spell Healing_Tide_Totem = new Spell("Healing Tide Totem");
    private Spell Heroism = new Spell("Heroism");
    private Spell Hex = new Spell("Hex");
    private Spell Lava_Lash = new Spell("Lava Lash");
    private Spell Lightning_Bolt = new Spell("Lightning Bolt");
    private Spell Lightning_Shield = new Spell("Lightning Shield");
    private Spell Magma_Totem = new Spell("Magma Totem");
    private Spell Maelstrom_Weapon = new Spell("Maelstrom Weapon");
    private Spell Primal_Strike = new Spell("Primal Strike");
    private Spell Purge = new Spell("Purge");
    private Spell Rockbiter_Weapon = new Spell("Rockbiter Weapon");
    private Spell Searing_Totem = new Spell("Searing Totem");
    private Spell Shamanistic_Rage = new Spell("Shamanistic Rage");
    private Spell Spirit_Walk = new Spell("Spirit Walk");
    private Spell Spiritwalkers_Grace = new Spell("Spiritwalker's Grace");
    private Spell Stone_Bulwark_Totem = new Spell("Stone Bulwark Totem");
    private Spell Stormlash_Totem = new Spell("Stormlash Totem");
    private Spell Stormstrike = new Spell("Stormstrike");
    private Spell Totemic_Recall = new Spell("Totemic Recall");
    private Spell Tremor_Totem = new Spell("Tremor Totem");
    private Spell Unleash_Elements = new Spell("Unleash Elements");
    private Spell Water_Shield = new Spell("Water Shield");
    private Spell Water_Walking = new Spell("Water Walking");
    private Spell Windfury_Weapon = new Spell("Windfury Weapon");
    private Spell Windwalk_Totem = new Spell("Windwalk Totem");
    private Spell Wind_Shear = new Spell("Wind Shear");

    // TIMER
    private Timer Flame_Shock_Timer = new Timer(0);
    private Timer Water_Walking_Timer = new Timer(0);
    private Timer Flametongue_Weapon_Timer = new Timer(0);
    private Timer Windfury_Weapon_Timer = new Timer(0);
    private Timer Air_Totem_Timer = new Timer(0);
    private Timer Earth_Totem_Timer = new Timer(0);
    private Timer Fire_Totem_Timer = new Timer(0);
    private Timer Water_Totem_Timer = new Timer(0);
    int StartFight = 1;

    // Profession & Racials
    private Spell Arcane_Torrent = new Spell("Arcane Torrent");
    private Spell Berserking = new Spell("Berserking");
    private Spell Blood_Fury = new Spell("Blood Fury");
    private Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    private Spell Lifeblood = new Spell("Lifeblood");
    private Spell Stoneform = new Spell("Stoneform");

    #endregion InitializeSpell

    public Enh()
    {
        Main.range = 30.0f;
        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            OutCombatBuff();
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

    public void OutCombatBuff()
    {
        if (Water_Walking.IsSpellUsable && Water_Walking.KnownSpell && (!Water_Walking.HaveBuff || Water_Walking_Timer.IsReady)
            && ObjectManager.Target.IsDead)
        {
            Logging.WriteFight("OutofCombat Water Walking.");
            Water_Walking.Launch();
            Water_Walking_Timer = new Timer(1000 * 60 * 9);
            return;
        }

        if (ObjectManager.Me.HealthPercent < 100 && Healing_Surge.KnownSpell && Healing_Surge.IsSpellUsable
            && ObjectManager.Target.IsDead)
        {
            Logging.WriteFight("OutofCombat Healing Surge.");
            Healing_Surge.Launch();
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(200);
            }
            return;
        }
    }

    public void Patrolling()
    {
        if (Windfury_Weapon.KnownSpell && Windfury_Weapon.IsSpellUsable && Windfury_Weapon_Timer.IsReady)
        {
            Windfury_Weapon.Launch();
            Windfury_Weapon_Timer = new Timer(1000 * 60 * 60);
            return;
        }

        if (Flametongue_Weapon.KnownSpell && Flametongue_Weapon.IsSpellUsable && Flametongue_Weapon_Timer.IsReady)
        {
            Flametongue_Weapon.Launch();
            Flametongue_Weapon_Timer = new Timer(1000 * 60 * 60);
            return;
        }

        if (!ObjectManager.Me.IsMounted)
        {
            Buff();
        }
    }

    public void Pull()
    {
        if (Earth_Shock.KnownSpell && Earth_Shock.IsSpellUsable && Earth_Shock.IsDistanceGood)
        {
            Earth_Shock.Launch();
            return;
        }
        StartFight--;
    }

    public void Buff()
    {
        if (ObjectManager.Me.BarTwoPercentage < 15 && Water_Shield.KnownSpell && Water_Shield.IsSpellUsable)
        {
            if (Water_Shield.KnownSpell && Water_Shield.IsSpellUsable && !Water_Shield.HaveBuff)
            {
                Water_Shield.Launch();
                return;
            }
        }

        else
        {
            if (Lightning_Shield.KnownSpell && Lightning_Shield.IsSpellUsable && !Lightning_Shield.HaveBuff)
            {
                Lightning_Shield.Launch();
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
        if (StartFight == 1)
            Pull();

        if (Earth_Shock.KnownSpell && Earth_Shock.IsSpellUsable && Earth_Shock.IsDistanceGood)
        {
            Earth_Shock.Launch();
            return;
        }

        if (Stormstrike.KnownSpell && Stormstrike.IsSpellUsable && Stormstrike.IsDistanceGood)
        {
            Stormstrike.Launch();
            return;
        }

        if (Lava_Lash.KnownSpell && Lava_Lash.IsSpellUsable && Lava_Lash.IsDistanceGood)
        {
            Lava_Lash.Launch();
            return;
        }

        if (Searing_Totem.KnownSpell && Searing_Totem.IsSpellUsable && Fire_Totem_Timer.IsReady)
        {
            Searing_Totem.Launch();
            Fire_Totem_Timer = new Timer(1000 * 57);
            return;
        }

        if (Chain_Lightning.KnownSpell && Chain_Lightning.IsSpellUsable && Chain_Lightning.IsDistanceGood)
        {
            Chain_Lightning.Launch();
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

        if (ObjectManager.Me.BarTwoPercentage < 70 && Arcane_Torrent.KnownSpell && Arcane_Torrent.IsSpellUsable)
        {
            Arcane_Torrent.Launch();
            return;
        }

        if (Stormlash_Totem.KnownSpell && Stormlash_Totem.IsSpellUsable && !ObjectManager.Me.HaveBuff(8178)
            && Air_Totem_Timer.IsReady)
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
            Stormlash_Totem.Launch();
            Air_Totem_Timer = new Timer(1000 * 10);
            return;
        }

        if (Call_of_the_Elements.KnownSpell && Call_of_the_Elements.IsSpellUsable && !Stormlash_Totem.IsSpellUsable
            && !ObjectManager.Me.HaveBuff(120676))
        {
            Call_of_the_Elements.Launch();
            return;
        }

        if (Feral_Spirit.KnownSpell && Feral_Spirit.IsSpellUsable)
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
            Feral_Spirit.Launch();
            return;
        }

        if (Heroism.KnownSpell && Heroism.IsSpellUsable && !ObjectManager.Me.HaveBuff(57724))
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
            Heroism.Launch();
            return;
        }

        if (Bloodlust.KnownSpell && Bloodlust.IsSpellUsable && !ObjectManager.Me.HaveBuff(57724))
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
            Bloodlust.Launch();
            return;
        }

        if (Elemental_Mastery.KnownSpell && Elemental_Mastery.IsSpellUsable &&
            !ObjectManager.Me.HaveBuff(2825))
        {
            Elemental_Mastery.Launch();
            return;
        }

        if (Earth_Elemental_Totem.KnownSpell && Earth_Elemental_Totem.IsSpellUsable &&
            ObjectManager.GetNumberAttackPlayer() > 3)
        {
            Earth_Elemental_Totem.Launch();
            Earth_Totem_Timer = new Timer(1000 * 60);
            return;
        }

        if (Fire_Elemental_Totem.KnownSpell && Fire_Elemental_Totem.IsSpellUsable)
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
            Fire_Elemental_Totem.Launch();
            Fire_Totem_Timer = new Timer(1000 * 60);
            return;
        }

        if (ObjectManager.GetNumberAttackPlayer() > 5)
        {
            if (Magma_Totem.KnownSpell && Magma_Totem.IsSpellUsable && Fire_Totem_Timer.IsReady)
            {
                Magma_Totem.Launch();
                Fire_Totem_Timer = new Timer(1000 * 57);
                return;
            }
        }

        else
        {
            if (Searing_Totem.KnownSpell && Searing_Totem.IsSpellUsable && Fire_Totem_Timer.IsReady)
            {
                Searing_Totem.Launch();
                Fire_Totem_Timer = new Timer(1000 * 57);
                return;
            }
        }

        if (ObjectManager.GetNumberAttackPlayer() > 2)
        {
            if (Chain_Lightning.KnownSpell && Chain_Lightning.IsSpellUsable && Chain_Lightning.IsDistanceGood
                && Maelstrom_Weapon.BuffStack == 5)
            {
                Chain_Lightning.Launch();
                return;
            }
        }

        else
        {
            if (Lightning_Bolt.IsDistanceGood && Lightning_Bolt.KnownSpell && Lightning_Bolt.IsSpellUsable
                && Maelstrom_Weapon.BuffStack == 5)
            {
                Lightning_Bolt.Launch();
                return;
            }
        }

        if (Flame_Shock.IsSpellUsable && Flame_Shock.IsDistanceGood && Flame_Shock.KnownSpell
            && (!Flame_Shock.TargetHaveBuff || Flame_Shock_Timer.IsReady))
        {
            if (Unleash_Elements.KnownSpell && Unleash_Elements.IsSpellUsable && Unleash_Elements.IsDistanceGood)
            {
                Unleash_Elements.Launch();
                Thread.Sleep(200);
            }
            Flame_Shock.Launch();
            Flame_Shock_Timer = new Timer(1000 * 27);
            return;
        }

        if (Fire_Nova.KnownSpell && Fire_Nova.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() > 2)
        {
            Fire_Nova.Launch();
            return;
        }

        if (Primal_Strike.KnownSpell && Primal_Strike.IsSpellUsable && Primal_Strike.IsDistanceGood)
        {
            Primal_Strike.Launch();
            return;
        }

        /*if (Stormstrike.KnownSpell && Stormstrike.IsSpellUsable && Stormstrike.IsDistanceGood)
        {
            Stormstrike.Launch();
            return;
        }*/

        if (Lava_Lash.KnownSpell && Lava_Lash.IsSpellUsable && Lava_Lash.IsDistanceGood)
        {
            Lava_Lash.Launch();
            return;
        }

        if (Unleash_Elements.KnownSpell && Unleash_Elements.IsSpellUsable && Unleash_Elements.IsDistanceGood)
        {
            Unleash_Elements.Launch();
            Thread.Sleep(200);
        }

        if (Earth_Shock.IsSpellUsable && Earth_Shock.KnownSpell && Earth_Shock.IsDistanceGood)
        {
            if (Flame_Shock.KnownSpell && (!Flame_Shock.TargetHaveBuff || Flame_Shock_Timer.IsReady))
                return;
            Earth_Shock.Launch();
            return;
        }

        if (ObjectManager.GetNumberAttackPlayer() > 2)
        {
            if (Chain_Lightning.KnownSpell && Chain_Lightning.IsSpellUsable && Chain_Lightning.IsDistanceGood)
            {
                if (Ancestral_Swiftness.KnownSpell && Ancestral_Swiftness.IsSpellUsable)
                {
                    Ancestral_Swiftness.Launch();
                    Thread.Sleep(200);
                }
                Chain_Lightning.Launch();
                return;
            }
        }

        else
        {
            if (Lightning_Bolt.IsDistanceGood && Lightning_Bolt.KnownSpell && Lightning_Bolt.IsSpellUsable)
            {
                if (Ancestral_Swiftness.KnownSpell && Ancestral_Swiftness.IsSpellUsable)
                {
                    Ancestral_Swiftness.Launch();
                    Thread.Sleep(200);
                }
                Lightning_Bolt.Launch();
                return;
            }
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.HealthPercent < 80 && Gift_of_the_Naaru.KnownSpell && Gift_of_the_Naaru.IsSpellUsable)
        {
            Gift_of_the_Naaru.Launch();
            return;
        }

        if (Healing_Stream_Totem.KnownSpell && Healing_Stream_Totem.IsSpellUsable && ObjectManager.Me.HealthPercent < 90
            && Water_Totem_Timer.IsReady)
        {
            Healing_Stream_Totem.Launch();
            Water_Totem_Timer = new Timer(1000 * 15);
            return;
        }

        if (Healing_Tide_Totem.KnownSpell && Healing_Tide_Totem.IsSpellUsable && ObjectManager.Me.HealthPercent < 70
            && Water_Totem_Timer.IsReady)
        {
            Healing_Tide_Totem.Launch();
            Water_Totem_Timer = new Timer(1000 * 15);
            return;
        }

        if (Ancestral_Guidance.KnownSpell && Ancestral_Guidance.IsSpellUsable && ObjectManager.Me.HealthPercent < 70)
        {
            Ancestral_Guidance.Launch();
            return;
        }

        if (Healing_Surge.KnownSpell && Healing_Surge.IsSpellUsable && ObjectManager.Me.HealthPercent < 50)
        {
            Healing_Surge.Launch();
            return;
        }
    }

    private void Resistance()
    {

        if (ObjectManager.Me.HealthPercent < 80 && Shamanistic_Rage.IsSpellUsable && Shamanistic_Rage.KnownSpell)
        {
            Shamanistic_Rage.Launch();
            return;
        }
        if (ObjectManager.Me.HealthPercent < 50 && Capacitor_Totem.KnownSpell && Capacitor_Totem.IsSpellUsable
            && Air_Totem_Timer.IsReady)
        {
            Capacitor_Totem.Launch();
            Air_Totem_Timer = new Timer(1000 * 5);
            return;
        }

        if (ObjectManager.Me.HealthPercent < 70 && Astral_Shift.KnownSpell && Astral_Shift.IsSpellUsable)
        {
            Astral_Shift.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 50 && Stone_Bulwark_Totem.KnownSpell && Stone_Bulwark_Totem.IsSpellUsable
            && Earth_Totem_Timer.IsReady)
        {
            Stone_Bulwark_Totem.Launch();
            Earth_Totem_Timer = new Timer(1000 * 30);
            return;
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
            Wind_Shear.KnownSpell && Wind_Shear.IsSpellUsable && Wind_Shear.IsDistanceGood)
        {
            Wind_Shear.Launch();
            return;
        }

        if (ObjectManager.Target.IsCast &&
            ObjectManager.Target.IsTargetingMe &&
            Grounding_Totem.KnownSpell && Grounding_Totem.IsSpellUsable && Air_Totem_Timer.IsReady)
        {
            Grounding_Totem.Launch();
            Air_Totem_Timer = new Timer(1000 * 15);
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
