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
            case WoWClass.Rogue:

                Spell Blade_Flurry = new Spell("Blade Flurry");
                Spell Mutilate = new Spell("Mutilate");

                if (Blade_Flurry.KnownSpell)
                {
                    RogueCom ccROC = new RogueCom();
                }

                if (Mutilate.KnownSpell)
                {
                    RogueAssa ccROA = new RogueAssa();
                }

                else
                {
                    Rogue ccRO = new Rogue();
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
        Main.range = 3.6f; // Range

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
        Main.range = 3.6f; // Range

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

            else if (ObjectManager.Me.BarTwoPercentage > 35 && !Envenom.IsSpellUsable && Envenom.IsDistanceGood)
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
        Main.range = 3.6f; // Range

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

#endregion CustomClass
