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
            case WoWClass.Shaman:

                Spell Thunderstorm = new Spell("Thunderstorm");
                Spell Lava_Lash = new Spell("Lava Lash");
                Spell Earth_Shield = new Spell("Earth Shield");

                if (Thunderstorm.KnownSpell || Earth_Shield.KnownSpell)
                {
                    ShamanElem ccSHEL = new ShamanElem();
                }

                if (Lava_Lash.KnownSpell)
                {
                    ShamanEnhan ccSHEN = new ShamanEnhan();
                }

                else
                {
                    Shaman ccSH = new Shaman();
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

public class ShamanElem
{
    #region InitializeSpell

    Spell Thunderstorm = new Spell("Thunderstorm");
    Spell Lava_Lash = new Spell("Lava Lash");
    Spell Earth_Shield = new Spell("Earth Shield");
    Spell Flame_Shock = new Spell("Flame Shock");
    Spell Lava_Burst = new Spell("Lava Burst");
    Spell Lightning_Bolt = new Spell("Lightning Bolt");
    Spell Earth_Shock = new Spell("Earth Shock");
    Spell Chain_Lightning = new Spell("Chain Lightning");
    Spell Call_of_the_Elements = new Spell("Call of the Elements");
    Spell Searing_Totem = new Spell("Searing Totem");
    Spell Totem_of_Tranquil_Mind = new Spell("Totem of Tranquil Mind");
    Spell Fire_Elemental_Totem = new Spell("Fire Elemental Totem");
    Spell Mana_Spring_Totem = new Spell("Mana Spring Totem");
    Spell Lightning_Shield = new Spell("Lightning Shield");
    Spell Flametongue_Weapon = new Spell("Flametongue Weapon");
    Spell Flametongue_Totem = new Spell("Flametongue Totem");
    Spell Wind_Shear = new Spell("Wind Shear");
    Spell Water_Shield = new Spell("Water Shield");
    Spell Healing_Surge = new Spell("Healing Surge");
    Spell Healing_Wave = new Spell("Healing Wave");
    Timer Flametongue_Weapon_Timer = new Timer(0);
    Spell Earth_Elemental_Totem = new Spell("Earth Elemental Totem");
    Spell Grounding_Totem = new Spell("Grounding Totem");
    Spell Elemental_Mastery = new Spell("Elemental Mastery");
    Spell Bloodlust = new Spell("Bloodlust");
    Spell Heroism = new Spell("Heroism");
    Spell Stoneskin_Totem = new Spell("Stoneskin Totem");
    Spell Wrath_of_Air_Totem = new Spell("Wrath of Air Totem");

    Timer Elem_totem_Timer = new Timer(0);
    #endregion InitializeSpell

    public ShamanElem()
    {
        Main.range = 20.0f; // Range

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

    public void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            Buff();
            Heal();
        }
    }

    public void Pull()
    {
        Totem();
        Buff();
    }

    public void Combat()
    {
        AvoidMelee();
        Heal();
        Buff();
        Decast();
        BuffCombat();

        if (!Flame_Shock.TargetHaveBuff && Flame_Shock.KnownSpell && Flame_Shock.IsDistanceGood && Flame_Shock.IsSpellUsable)
        {
            Flame_Shock.Launch();
            return;
        }

        if (Flame_Shock.TargetHaveBuff && Lava_Burst.KnownSpell && Lava_Burst.IsDistanceGood && Lava_Burst.IsSpellUsable)
        {
            Lava_Burst.Launch();
            return;
        }

        if (Earth_Shock.KnownSpell && Earth_Shock.IsDistanceGood && Earth_Shock.IsSpellUsable)
        {
            Earth_Shock.Launch();
            return;
        }

        if (ObjectManager.GetNumberAttackPlayer() > 2 &&
            Chain_Lightning.KnownSpell && Chain_Lightning.IsDistanceGood && Chain_Lightning.IsSpellUsable)
        {
            Chain_Lightning.Launch();
            return;
        }

        if (Lightning_Bolt.KnownSpell && Lightning_Bolt.IsDistanceGood && Lightning_Bolt.IsSpellUsable)
        {
            Lightning_Bolt.Launch();
            return;
        }
    }

    private void Buff()
    {
        if (!Lightning_Shield.HaveBuff && !Water_Shield.HaveBuff && Lightning_Shield.KnownSpell && Lightning_Shield.IsSpellUsable)
            Lightning_Shield.Launch();

        if (ObjectManager.Me.BarTwoPercentage < 30 && !Water_Shield.HaveBuff && Water_Shield.KnownSpell && Water_Shield.IsSpellUsable)
            Water_Shield.Launch();

        if (Flametongue_Weapon_Timer.IsReady && Flametongue_Weapon.KnownSpell && Flametongue_Weapon.IsSpellUsable)
        {
            Flametongue_Weapon.Launch();
            Flametongue_Weapon_Timer = new Timer(1000 * 1800);
        }
    }

    private void Totem()
    {
        if (Call_of_the_Elements.KnownSpell && Call_of_the_Elements.IsSpellUsable)
        {
            Call_of_the_Elements.Launch();
        }

        else
        {
            if ((!Elem_totem_Timer.IsReady && !Bloodlust.IsSpellUsable) && Stoneskin_Totem.IsSpellUsable && Stoneskin_Totem.KnownSpell)
            {
                Stoneskin_Totem.Launch();
                Thread.Sleep(1000);
            }

            if (Flametongue_Totem.IsSpellUsable && Flametongue_Totem.KnownSpell)
            {
                Totem_of_Tranquil_Mind.Launch();
                Thread.Sleep(1000);
            }

            if (Wrath_of_Air_Totem.IsSpellUsable && Wrath_of_Air_Totem.KnownSpell)
            {
                Wrath_of_Air_Totem.Launch();
                Thread.Sleep(1000);
            }

            if (Mana_Spring_Totem.IsSpellUsable && Mana_Spring_Totem.KnownSpell)
            {
                Mana_Spring_Totem.Launch();
                Thread.Sleep(1000);
            }
        }
    }

    private void Heal()
    {
        if (Healing_Surge.KnownSpell && ObjectManager.Me.HealthPercent > 0 && ObjectManager.Me.HealthPercent < 50 &&
            Healing_Surge.IsSpellUsable)
        {
            Healing_Surge.Launch();
        }

        if (!Healing_Surge.KnownSpell && Healing_Wave.KnownSpell && ObjectManager.Me.HealthPercent > 0 && ObjectManager.Me.HealthPercent < 50 &&
            Healing_Wave.IsSpellUsable)
        {
            Healing_Wave.Launch();
        }
    }

    private void BuffCombat()
    {
        if (Elemental_Mastery.KnownSpell && Elemental_Mastery.IsSpellUsable && Elemental_Mastery.IsDistanceGood)
        {
            Elemental_Mastery.Launch();
        }

        if (Heroism.KnownSpell && Heroism.IsSpellUsable && Heroism.IsDistanceGood)
        {
            Heroism.Launch();
        }

        if (Elem_totem_Timer.IsReady && Bloodlust.KnownSpell && Bloodlust.IsSpellUsable && Bloodlust.IsDistanceGood)
        {
            if (Earth_Elemental_Totem.KnownSpell && Earth_Elemental_Totem.IsSpellUsable)
            {
                Earth_Elemental_Totem.Launch();
                Elem_totem_Timer = new Timer(1000 * 122);
                Thread.Sleep(1000);
            }

            Bloodlust.Launch();
        }

        Lua.RunMacroText("/use 13");
        Lua.RunMacroText("/use 14");
    }

    private void Decast()
    {
        if (ObjectManager.Target.IsCast && Wind_Shear.KnownSpell && Wind_Shear.IsSpellUsable && Wind_Shear.IsDistanceGood)
        {
            Wind_Shear.Launch();
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

public class ShamanEnhan
{
    #region InitializeSpell

    Spell Thunderstorm = new Spell("Thunderstorm");
    Spell Lava_Lash = new Spell("Lava Lash");
    Spell Earth_Shield = new Spell("Earth Shield");
    Spell Flame_Shock = new Spell("Flame Shock");
    Spell Lava_Burst = new Spell("Lava Burst");
    Spell Lightning_Bolt = new Spell("Lightning Bolt");
    Spell Earth_Shock = new Spell("Earth Shock");
    Spell Chain_Lightning = new Spell("Chain Lightning");
    Spell Call_of_the_Elements = new Spell("Call of the Elements");
    Spell Searing_Totem = new Spell("Searing Totem");
    Spell Totem_of_Tranquil_Mind = new Spell("Totem of Tranquil Mind");
    Spell Fire_Elemental_Totem = new Spell("Fire Elemental Totem");
    Spell Mana_Spring_Totem = new Spell("Mana Spring Totem");
    Spell Lightning_Shield = new Spell("Lightning Shield");
    Spell Flametongue_Weapon = new Spell("Flametongue Weapon");
    Spell Wind_Shear = new Spell("Wind Shear");
    Spell Water_Shield = new Spell("Water Shield");
    Spell Healing_Surge = new Spell("Healing Surge");
    Spell Healing_Wave = new Spell("Healing Wave");
    Spell Primal_Strike = new Spell("Primal Strike");

    Spell Earth_Elemental_Totem = new Spell("Earth Elemental Totem");
    Spell Grounding_Totem = new Spell("Grounding Totem");
    Spell Unleash_Flame = new Spell("Unleash Flame");
    Spell Unleash_Elements = new Spell("Unleash Elements");
    Spell Maelstrom_Weapon = new Spell("Maelstrom Weapon");
    Spell Stormstrike = new Spell("Stormstrike");
    Spell Feral_Spirit = new Spell("Feral Spirit");
    Spell Shamanistic_Rage = new Spell("Shamanistic Rage");
    Spell Windfury_Totem = new Spell("Windfury Totem");
    Spell Bloodlust = new Spell("Bloodlust");
    Spell Heroism = new Spell("Heroism");
    Spell Windfury_Weapon = new Spell("Windfury Weapon");
    Spell Frostbrand_Weapon = new Spell("Frostbrand Weapon");

    Timer Windfury_Weapon_Timer = new Timer(0);
    Timer Frostbrand_Weapon_Timer = new Timer(0);
    Timer Flametongue_Weapon_Timer = new Timer(0);
    #endregion InitializeSpell

    public ShamanEnhan()
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
        Totem();
        Buff();
    }

    public void Combat()
    {
        AvoidMelee();
        Heal();
        Buff();
        Decast();
        BuffCombat();

        if (Lava_Lash.KnownSpell &&
            Lava_Lash.IsDistanceGood &&
            Lava_Lash.IsSpellUsable)
        {
            Lava_Lash.Launch();
            return;
        }

        if (Unleash_Flame.HaveBuff &&
            Flame_Shock.KnownSpell &&
            Flame_Shock.IsDistanceGood &&
            Flame_Shock.IsSpellUsable)
        {
            Flame_Shock.Launch();
            return;
        }

        if (Unleash_Elements.KnownSpell &&
            Unleash_Elements.IsDistanceGood &&
            Unleash_Elements.IsSpellUsable)
        {
            Unleash_Elements.Launch();
            return;
        }

        if (Stormstrike.KnownSpell &&
            Stormstrike.IsDistanceGood &&
            Stormstrike.IsSpellUsable)
        {
            Stormstrike.Launch();
            return;
        }

        if (Earth_Shock.KnownSpell &&
            Earth_Shock.IsDistanceGood &&
            Earth_Shock.IsSpellUsable)
        {
            Earth_Shock.Launch();
            return;
        }

        if (Feral_Spirit.KnownSpell &&
            Feral_Spirit.IsDistanceGood &&
            Feral_Spirit.IsSpellUsable)
        {
            Feral_Spirit.Launch();
            return;
        }

        if (ObjectManager.Me.Level >= 55 &&
            !Flame_Shock.KnownSpell &&
            Primal_Strike.KnownSpell &&
            Primal_Strike.IsDistanceGood &&
            Primal_Strike.IsSpellUsable)
        {
            Primal_Strike.Launch();
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

    private void Buff()
    {

        if (!Lightning_Shield.HaveBuff &&
            !Water_Shield.HaveBuff &&
            Lightning_Shield.KnownSpell &&
            Lightning_Shield.IsSpellUsable)
        {
            Lightning_Shield.Launch();
        }

        if (ObjectManager.Me.BarTwoPercentage < 30 &&
            !Water_Shield.HaveBuff &&
            Water_Shield.KnownSpell &&
            Water_Shield.IsSpellUsable)
        {
            Water_Shield.Launch();
        }

        if (Flametongue_Weapon.KnownSpell)
        {

            if (Windfury_Weapon_Timer.IsReady &&
                Windfury_Weapon.KnownSpell &&
                Windfury_Weapon.IsSpellUsable)
            {
                Windfury_Weapon.Launch();
                Thread.Sleep(1000);
                Windfury_Weapon.Launch();
                Windfury_Weapon_Timer = new Timer(1000 * 1800);
            }

            if (!Windfury_Weapon.KnownSpell &&
                Frostbrand_Weapon_Timer.IsReady &&
                Frostbrand_Weapon.KnownSpell &&
                Frostbrand_Weapon.IsSpellUsable)
            {
                Frostbrand_Weapon.Launch();
                Thread.Sleep(1000);
                Frostbrand_Weapon.Launch();
                Frostbrand_Weapon_Timer = new Timer(1000 * 1800);
            }

            else if (!Frostbrand_Weapon.KnownSpell &&
                     Flametongue_Weapon_Timer.IsReady &&
                     Flametongue_Weapon.IsSpellUsable)
            {
                Flametongue_Weapon.Launch();
                Thread.Sleep(1000);
                Flametongue_Weapon.Launch();
                Flametongue_Weapon_Timer = new Timer(1000 * 1800);
            }

        }

    }

    private void Totem()
    {

        if (Call_of_the_Elements.KnownSpell &&
            Call_of_the_Elements.IsSpellUsable)
        {
            Call_of_the_Elements.Launch();
        }

        else
        {

            if (Searing_Totem.IsSpellUsable &&
                Searing_Totem.KnownSpell)
            {
                Searing_Totem.Launch();
                Thread.Sleep(1000);
            }

            if (Grounding_Totem.KnownSpell &&
                Grounding_Totem.IsSpellUsable)
            {
                Grounding_Totem.Launch();
                Thread.Sleep(1000);
            }

            if (Fire_Elemental_Totem.IsSpellUsable &&
                Fire_Elemental_Totem.KnownSpell)
            {
                Fire_Elemental_Totem.Launch();
                Thread.Sleep(1000);
            }

            else if (!Fire_Elemental_Totem.KnownSpell &&
                     Earth_Elemental_Totem.KnownSpell &&
                     Earth_Elemental_Totem.IsSpellUsable)
            {
                Earth_Elemental_Totem.Launch();
                Thread.Sleep(1000);
            }

            if (Windfury_Totem.IsSpellUsable &&
                Windfury_Totem.KnownSpell)
            {
                Windfury_Totem.Launch();
                Thread.Sleep(1000);
            }

        }
    }

    private void Heal()
    {
        if (Healing_Surge.KnownSpell && ObjectManager.Me.HealthPercent > 0 && ObjectManager.Me.HealthPercent < 50 &&
            Healing_Surge.IsSpellUsable)
        {
            Healing_Surge.Launch();
        }

        if (!Healing_Surge.KnownSpell && Healing_Wave.KnownSpell && ObjectManager.Me.HealthPercent > 0 && ObjectManager.Me.HealthPercent < 50 &&
            Healing_Wave.IsSpellUsable)
        {
            Healing_Wave.Launch();
        }
    }

    private void BuffCombat()
    {

        if (Shamanistic_Rage.KnownSpell &&
            Shamanistic_Rage.IsSpellUsable &&
            Shamanistic_Rage.IsDistanceGood)
        {
            Shamanistic_Rage.Launch();
        }

        if (Heroism.KnownSpell &&
            Heroism.IsSpellUsable &&
            Heroism.IsDistanceGood)
        {
            Heroism.Launch();
        }

        if (Bloodlust.KnownSpell &&
            Bloodlust.IsSpellUsable &&
            Bloodlust.IsDistanceGood)
        {
            Bloodlust.Launch();
        }

        Lua.RunMacroText("/use 13");
        Lua.RunMacroText("/use 14");

    }

    private void Decast()
    {
        if (ObjectManager.Target.IsCast &&
            Wind_Shear.KnownSpell &&
            Wind_Shear.IsSpellUsable &&
            Wind_Shear.IsDistanceGood)
        {
            Wind_Shear.Launch();
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

public class Shaman
{
    #region InitializeSpell

    Spell Thunderstorm = new Spell("Thunderstorm");
    Spell Lava_Lash = new Spell("Lava Lash");
    Spell Earth_Shield = new Spell("Earth Shield");
    Spell Flame_Shock = new Spell("Flame Shock");
    Spell Lava_Burst = new Spell("Lava Burst");
    Spell Lightning_Bolt = new Spell("Lightning Bolt");
    Spell Earth_Shock = new Spell("Earth Shock");
    Spell Chain_Lightning = new Spell("Chain Lightning");
    Spell Call_of_the_Elements = new Spell("Call of the Elements");
    Spell Searing_Totem = new Spell("Searing Totem");
    Spell Totem_of_Tranquil_Mind = new Spell("Totem of Tranquil Mind");
    Spell Fire_Elemental_Totem = new Spell("Fire Elemental Totem");
    Spell Mana_Spring_Totem = new Spell("Mana Spring Totem");
    Spell Lightning_Shield = new Spell("Lightning Shield");
    Spell Flametongue_Weapon = new Spell("Flametongue Weapon");
    Spell Wind_Shear = new Spell("Wind Shear");
    Spell Water_Shield = new Spell("Water Shield");
    Spell Healing_Surge = new Spell("Healing Surge");
    Spell Healing_Wave = new Spell("Healing Wave");

    Spell Earth_Elemental_Totem = new Spell("Earth Elemental Totem");
    Spell Grounding_Totem = new Spell("Grounding Totem");
    Spell Unleash_Flame = new Spell("Unleash Flame");
    Spell Unleash_Elements = new Spell("Unleash Elements");
    Spell Maelstrom_Weapon = new Spell("Maelstrom Weapon");
    Spell Stormstrike = new Spell("Stormstrike");
    Spell Feral_Spirit = new Spell("Feral Spirit");
    Spell Shamanistic_Rage = new Spell("Shamanistic Rage");
    Spell Windfury_Totem = new Spell("Windfury Totem");
    Spell Bloodlust = new Spell("Bloodlust");
    Spell Heroism = new Spell("Heroism");
    Spell Windfury_Weapon = new Spell("Windfury Weapon");
    Spell Frostbrand_Weapon = new Spell("Frostbrand Weapon");
    Spell Strength_of_Earth_Totem = new Spell("Strength of Earth Totem");
    Spell Primal_Strike = new Spell("Primal Strike");

    Timer Windfury_Weapon_Timer = new Timer(0);
    Timer Frostbrand_Weapon_Timer = new Timer(0);
    Timer Flametongue_Weapon_Timer = new Timer(0);

    #endregion InitializeSpell

    public Shaman()
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
        Totem();
        Buff();
    }

    public void Combat()
    {

        AvoidMelee();
        Heal();
        Buff();

        if (Earth_Shock.KnownSpell &&
            Earth_Shock.IsDistanceGood &&
            Earth_Shock.IsSpellUsable)
        {
            Earth_Shock.Launch();
            return;
        }

        if (Primal_Strike.KnownSpell &&
            Primal_Strike.IsDistanceGood &&
            Primal_Strike.IsSpellUsable)
        {
            Primal_Strike.Launch();
            return;
        }

        if (Lightning_Bolt.KnownSpell &&
            Lightning_Bolt.IsDistanceGood &&
            Lightning_Bolt.IsSpellUsable)
        {
            Lightning_Bolt.Launch();
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

    private void Buff()
    {

        if (!Lightning_Shield.HaveBuff &&
            !Water_Shield.HaveBuff &&
            Lightning_Shield.KnownSpell &&
            Lightning_Shield.IsSpellUsable)
        {
            Lightning_Shield.Launch();
        }

        if (Windfury_Weapon_Timer.IsReady &&
            Windfury_Weapon.KnownSpell &&
            Windfury_Weapon.IsSpellUsable)
        {
            Windfury_Weapon.Launch();
            Thread.Sleep(1000);
            Windfury_Weapon.Launch();
            Windfury_Weapon_Timer = new Timer(1000 * 1800);
        }

    }

    private void Totem()
    {

        if (Strength_of_Earth_Totem.KnownSpell &&
            Strength_of_Earth_Totem.IsSpellUsable)
        {
            Strength_of_Earth_Totem.Launch();
        }

    }

    private void Heal()
    {

        if (Healing_Wave.KnownSpell &&
            ObjectManager.Me.HealthPercent < 80 &&
            Healing_Wave.IsSpellUsable)
        {
            Healing_Wave.Launch();
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

#endregion CustomClass
