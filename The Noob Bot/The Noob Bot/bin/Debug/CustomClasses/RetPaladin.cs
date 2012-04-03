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
            case WoWClass.Paladin:

                Spell Templars_Verdict = new Spell("Templar's Verdict");

                if (Templars_Verdict.KnownSpell)
                {
                    RetPaladin ccDRF = new RetPaladin();
                    Logging.WriteFight("SadisticElysium Ret CC 1.0 Loaded.");
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

public class RetPaladin
{
    #region InitializeSpell

    Spell Seal_of_Command = new Spell("Seal of Command");
    Spell Seal_of_Truth = new Spell("Seal of Truth");
    Spell Blessing_of_Might = new Spell("Blessing of Might");
    Spell Judgement = new Spell("Judgement");
    Spell Retribution_Aura = new Spell("Retribution Aura");
    Spell Crusader_Aura = new Spell("Crusader Aura");
    Spell Devotion_Aura = new Spell("Devotion Aura");
    Spell Lay_on_Hands = new Spell("Lay on Hands");
    Spell Flash_of_Light = new Spell("Flash of Light");
    Spell Holy_Light = new Spell("Holy Light");
    Spell Exorcism = new Spell("Exorcism");
    Spell Crusader = new Spell("Crusader");
    Spell Crusader_Strike = new Spell("Crusader Strike");
    Spell Inquisition = new Spell("Inquisition");
    Spell Hand_of_Light = new Spell("Hand of Light");
    Spell Avenging_Wrath = new Spell("Avenging Wrath");
    Spell Hammer_of_Wrath = new Spell("Hammer of Wrath");
    Spell Consecration = new Spell("Consecration");
    Spell Word_of_Glory = new Spell("Word of Glory");
    Spell Divine_Protection = new Spell("Divine Protection");
    Spell Divine_Shield = new Spell("Divine Shield");
    Spell Hammer_of_Justice = new Spell("Hammer of Justice");
    Spell Divine_Storm = new Spell("Divine Storm");
    Spell Divine_Plea = new Spell("Divine Plea");
    Spell Zealotry = new Spell("Zealotry");
    Spell Guardian_of_Ancient_Kings = new Spell("Guardian of Ancient Kings");
    Spell Rebuke = new Spell("Rebuke");
    Spell Templars_Verdict = new Spell("Templar's Verdict");
    #endregion InitializeSpell

    public RetPaladin()
    {
        Main.range = 3.6f; // Range

        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            if (ObjectManager.Me.IsMounted && Crusader_Aura.KnownSpell && !Crusader_Aura.HaveBuff && Crusader_Aura.IsSpellUsable)
                Crusader_Aura.Launch();

            if (!ObjectManager.Me.IsMounted)
            {
                Patrolling();

                if (Fight.InFight && ObjectManager.Me.Target > 0)
                {
                    if (ObjectManager.Me.Target != lastTarget && Judgement.IsDistanceGood)
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
        Judgement.Launch();
    }

    public void Combat()
    {
        Heal();
        AvoidMelee();
        Interrupt();
        Shield();
        Others();
    }

    public void Patrolling()
    {

        if (!ObjectManager.Me.IsMounted)
        {
            Seal();
            Blessing();
            Aura();
            HolyLight();
        }
    }

    ////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////
    ///////////					Private Function				  //////////
    ////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////

    private void Seal()
    {

        if (Seal_of_Command.KnownSpell)
        {
            if (!Seal_of_Command.HaveBuff && Seal_of_Command.IsSpellUsable)
            {
                Seal_of_Command.Launch();
            }
        }
        else if (Seal_of_Truth.KnownSpell)
            if (!Seal_of_Truth.HaveBuff && Seal_of_Truth.IsSpellUsable)
            {
                {
                    Seal_of_Truth.Launch();
                }
            }
    }

    private void Blessing()
    {
        if (Blessing_of_Might.KnownSpell && !Blessing_of_Might.HaveBuff && Blessing_of_Might.IsSpellUsable)
        {
            Blessing_of_Might.Launch();
        }
    }

    private void HolyLight()
    {
        if (Holy_Light.KnownSpell && ObjectManager.Me.HealthPercent < 60 &&
            Holy_Light.IsSpellUsable && Crusader.HaveBuff)
        {
            Holy_Light.Launch();
            return;
        }
    }

    private void Aura()
    {

        if (Retribution_Aura.KnownSpell)
        {
            if (!Retribution_Aura.HaveBuff &&
            Retribution_Aura.IsSpellUsable)
            {
                Retribution_Aura.Launch();
            }
        }
        else if (Devotion_Aura.KnownSpell)
        {
            if (!Devotion_Aura.HaveBuff &&
            Devotion_Aura.IsSpellUsable)
            {
                Devotion_Aura.Launch();
            }
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (Lay_on_Hands.KnownSpell && ObjectManager.Me.HealthPercent > 0 && ObjectManager.Me.HealthPercent < 20 && Lay_on_Hands.IsSpellUsable)
        {
            Lay_on_Hands.Launch();
            return;
        }

        if (Flash_of_Light.KnownSpell && ObjectManager.Me.HealthPercent > 0 && ObjectManager.Me.HealthPercent < 50 && Flash_of_Light.IsSpellUsable)
        {
            Flash_of_Light.Launch();
            return;
        }
        if (Word_of_Glory.KnownSpell && ObjectManager.Me.HealthPercent > 0 && ObjectManager.Me.HealthPercent < 70 && Word_of_Glory.IsSpellUsable)
        {
            Word_of_Glory.Launch();
            return;
        }
        if (ObjectManager.Me.BarTwoPercentage < 50 && Divine_Plea.KnownSpell && Divine_Plea.IsSpellUsable)
        {
            Divine_Plea.Launch();
            return;
        }
    }

    private void Others()
    {
        if (ObjectManager.GetNumberAttackPlayer() >= 3 && Zealotry.KnownSpell && Zealotry.IsDistanceGood && Zealotry.IsSpellUsable)
        {
            Zealotry.Launch();
        }

        if (Avenging_Wrath.KnownSpell &&
            Avenging_Wrath.IsSpellUsable)
        {
            Avenging_Wrath.Launch();
        }

        if (Inquisition.KnownSpell &&
            Inquisition.IsSpellUsable &&
            !ObjectManager.Me.HaveBuff(84963) &&
            !ObjectManager.Me.HaveBuff(90174))
        {
            Inquisition.Launch();
            return;
        }

        if (ObjectManager.Target.HealthPercent <= 25 || Hammer_of_Wrath.KnownSpell &&
            Hammer_of_Wrath.IsDistanceGood &&
            Avenging_Wrath.HaveBuff &&
            Hammer_of_Wrath.IsSpellUsable)
        {
            Hammer_of_Wrath.Launch();
            return;
        }

        if (ObjectManager.Me.HaveBuff(59578) &&
            Exorcism.KnownSpell && Exorcism.IsDistanceGood && Exorcism.IsSpellUsable)
        {
            Exorcism.Launch();
            return;
        }

        if (Templars_Verdict.KnownSpell &&
            Templars_Verdict.IsDistanceGood &&
            Templars_Verdict.IsSpellUsable)
        {
            Templars_Verdict.Launch();
            return;
        }

        if (Judgement.KnownSpell &&
            Judgement.IsSpellUsable)
        {
            Judgement.Launch();
            return;
        }

        if (Divine_Storm.KnownSpell &&
            ObjectManager.GetNumberAttackPlayer() >= 2 &&
            Divine_Storm.IsSpellUsable)
        {
            Divine_Storm.Launch();
            return;
        }

        if (Guardian_of_Ancient_Kings.KnownSpell &&
            ObjectManager.GetNumberAttackPlayer() >= 3 &&
            Guardian_of_Ancient_Kings.IsSpellUsable)
        {
            Guardian_of_Ancient_Kings.Launch();
            return;
        }

        if (Crusader_Strike.KnownSpell &&
            Crusader_Strike.IsDistanceGood &&
            Crusader_Strike.IsSpellUsable)
        {
            Crusader_Strike.Launch();
            return;
        }

        if (Consecration.KnownSpell &&
            Consecration.IsDistanceGood &&
            Consecration.IsSpellUsable)
        {
            if (ObjectManager.GetNumberAttackPlayer() > 1)
                Consecration.Launch();
            return;
        }

        if (Divine_Protection.KnownSpell &&
            Divine_Protection.IsDistanceGood &&
            Divine_Protection.IsSpellUsable)
        {
            if (ObjectManager.GetNumberAttackPlayer() > 1)
                Divine_Protection.Launch();
        }

    }

    private void TargetMoving()
    {
        if (ObjectManager.Target.GetDistance >= 5 && Hammer_of_Justice.KnownSpell && Hammer_of_Justice.IsDistanceGood && Hammer_of_Justice.IsSpellUsable)
            Hammer_of_Justice.Launch();
    }

    private void Interrupt()
    {
        if (ObjectManager.Target.IsCast && Rebuke.KnownSpell && Rebuke.IsSpellUsable && Rebuke.IsDistanceGood)
            Rebuke.Launch();
    }

    private void Shield()
    {
        if (Divine_Shield.KnownSpell && ObjectManager.Me.HealthPercent > 0 && ObjectManager.Me.HealthPercent < 25 && Divine_Shield.IsSpellUsable)
            Divine_Shield.Launch();

        if (Divine_Protection.KnownSpell && ObjectManager.Me.HealthPercent > 0 && ObjectManager.Me.HealthPercent < 50 && Divine_Protection.IsSpellUsable)
            Divine_Protection.Launch();
    }

    private void AvoidMelee()
    {
        if (ObjectManager.Target.GetDistance < 1)
            Keyboard.DownKey(nManager.Wow.Memory.WowProcess.MainWindowHandle, "{DOWN}");
    }
}

#endregion CustomClass