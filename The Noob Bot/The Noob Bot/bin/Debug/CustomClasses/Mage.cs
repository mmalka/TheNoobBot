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
            case WoWClass.Mage:

                Spell Arcane_Barrage = new Spell("Arcane Barrage");
                Spell Pyroblast = new Spell("Pyroblast");
                Spell Summon_Water_Elemental = new Spell("Summon Water Elemental");

                // if (Arcane_Barrage.KnownSpell)
                // {
                //     MageArcane ccMAAR = new MageArcan();
                // }

                //if (Pyroblast.KnownSpell)
                // {
                //     MageFire ccMAFE = new MageFire();
                // }

                if (Summon_Water_Elemental.KnownSpell)
                {
                    MageFrost ccMAFR = new MageFrost();
                }

                // else
                // {
                //      Mage ccMA = new Mage();
                //  }

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

public class MageFrost
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

    public MageFrost()
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

#endregion CustomClass
