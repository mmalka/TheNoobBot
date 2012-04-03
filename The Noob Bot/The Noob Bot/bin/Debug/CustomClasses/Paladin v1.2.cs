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

                Spell Avengers_Shield = new Spell("Avenger's Shield");
                Spell Templars_Verdict = new Spell("Templar's Verdict");
                Spell Holy_Shock = new Spell("Holy Shock");

                if (Avengers_Shield.KnownSpell)
                {
                    PaladinDef ccPALD = new PaladinDef();
                }

                if (Templars_Verdict.KnownSpell || Holy_Shock.KnownSpell)
                {
                    PaladinVind ccPALV = new PaladinVind();
                }

                if (!Templars_Verdict.KnownSpell && !Holy_Shock.KnownSpell && !Avengers_Shield.KnownSpell)
                {
                    Paladin ccPAL = new Paladin();
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

public class PaladinDef
{
    #region InitializeSpell

    Spell Seal_of_Command = new Spell("Seal of Command");
    Spell Seal_of_Righteousness = new Spell("Seal of Righteousness");
    Spell Seal_of_Insight = new Spell("Seal of Insight");
    Spell Seal_of_Truth = new Spell("Seal of Truth");

    Spell Blessing_of_Might = new Spell("Blessing of Might");

    Spell Judgement = new Spell("Judgement");

    Spell Retribution_Aura = new Spell("Retribution Aura");
    Spell Devotion_Aura = new Spell("Devotion Aura");
    Spell Crusader_Aura = new Spell("Crusader Aura");

    Spell Lay_on_Hands = new Spell("Lay on Hands");
    Spell Flash_of_Light = new Spell("Flash of Light");
    Spell Holy_Light = new Spell("Holy Light");

    Spell Exorcism = new Spell("Exorcism");
    Spell Crusader_Strike = new Spell("Crusader Strike");
    Spell Hammer_of_Wrath = new Spell("Hammer of Wrath");
    Spell Consecration = new Spell("Consecration");
    Spell Divine_Protection = new Spell("Divine Protection");
    Spell Divine_Shield = new Spell("Divine Shield");
    Spell Hammer_of_Justice = new Spell("Hammer of Justice");
    Spell Divine_Storm = new Spell("Divine Storm");
    Spell Avengers_Shield = new Spell("Avenger's Shield");
    Spell Templars_Verdict = new Spell("Templar's Verdict");
    Spell Holy_Shock = new Spell("Holy Shock");
    Spell Shield_of_the_Righteous = new Spell("Shield of the Righteous");
    Spell Sacred_Duty = new Spell("Sacred Duty");
    Spell Hammer_of_the_Righteous = new Spell("Hammer of the Righteous");
    Spell Holy_Wrath = new Spell("Holy Wrath");
    Spell Guardian_of_Ancient_Kings = new Spell("Guardian of Ancient Kings");
    Spell Ardent_Defender = new Spell("Ardent Defender");
    Spell Word_of_Glory = new Spell("Word of Glory");
    Spell Avenging_Wrath = new Spell("Avenging Wrath");
    Spell Inquisition = new Spell("Inquisition");
    Spell Divine_Plea = new Spell("Divine Plea");
    Spell Zealotry = new Spell("Zealotry");
    Spell Rebuke = new Spell("Rebuke");
    Timer Shield_of_the_Righteous_Timer = new Timer(0);
    Timer Inquisition_Timer = new Timer(0);

    #endregion InitializeSpell

    public PaladinDef()
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
                    if (ObjectManager.Me.Target != lastTarget && Avengers_Shield.IsDistanceGood)
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
        if (Avengers_Shield.KnownSpell && Avengers_Shield.IsSpellUsable)
            Avengers_Shield.Launch();

        if (Judgement.KnownSpell && Judgement.IsDistanceGood && Judgement.IsSpellUsable)
            Judgement.Launch();
    }

    public void Combat()
    {
        BuffCombat();
        AvoidMelee();
        Shield();
        Heal();
        TargetMoving();
        Decast();

        if (Seal_of_Insight.KnownSpell || Seal_of_Truth.KnownSpell || Seal_of_Righteousness.KnownSpell)
        {
            if (Seal_of_Insight.KnownSpell && !Seal_of_Insight.HaveBuff && Seal_of_Insight.IsSpellUsable && ObjectManager.Me.BarTwoPercentage < 25)
                Seal_of_Insight.Launch();

            if (Seal_of_Truth.KnownSpell && !Seal_of_Truth.HaveBuff && Seal_of_Truth.IsSpellUsable && ObjectManager.Me.BarTwoPercentage > 80)
                Seal_of_Truth.Launch();

            else if (!Seal_of_Truth.KnownSpell && Seal_of_Righteousness.KnownSpell && !Seal_of_Righteousness.HaveBuff && Seal_of_Righteousness.IsSpellUsable && ObjectManager.Me.BarTwoPercentage > 80)
                Seal_of_Righteousness.Launch();
        }

        if (ObjectManager.GetNumberAttackPlayer() <= 2 && Inquisition_Timer.IsReady && Inquisition.KnownSpell && Inquisition.IsSpellUsable)
        {
            Inquisition.Launch();
            Inquisition_Timer = new Timer(1000 * 12);
            return;
        }

        if (ObjectManager.GetNumberAttackPlayer() > 2 && Consecration.KnownSpell && Consecration.IsSpellUsable)
        {
            Consecration.Launch();
            return;
        }

        if (Crusader_Strike.KnownSpell && Crusader_Strike.IsDistanceGood && Crusader_Strike.IsSpellUsable)
        {
            if (ObjectManager.GetNumberAttackPlayer() > 2 && Hammer_of_the_Righteous.KnownSpell && Hammer_of_the_Righteous.IsDistanceGood && Hammer_of_the_Righteous.IsSpellUsable)
            {
                Hammer_of_the_Righteous.Launch();
                return;
            }

            else
            {
                Crusader_Strike.Launch();
                return;
            }
        }

        if (ObjectManager.GetNumberAttackPlayer() > 2 && Holy_Wrath.KnownSpell && Holy_Wrath.IsDistanceGood && Holy_Wrath.IsSpellUsable)
        {
            Lua.RunMacroText("/use 13");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Lua.RunMacroText("/use 14");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");

            Holy_Wrath.Launch();
            return;
        }

        if (Shield_of_the_Righteous_Timer.IsReady && Shield_of_the_Righteous.KnownSpell && Shield_of_the_Righteous.IsDistanceGood && Shield_of_the_Righteous.IsSpellUsable)
        {
            Shield_of_the_Righteous.Launch();
            Shield_of_the_Righteous_Timer = new Timer(1000 * 12);
            return;
        }

        if (Judgement.KnownSpell && Judgement.IsDistanceGood && Judgement.IsSpellUsable)
        {
            Judgement.Launch();
            return;
        }

        if (Avengers_Shield.KnownSpell && Avengers_Shield.IsSpellUsable)
        {
            Avengers_Shield.Launch();
            return;
        }

        if (Hammer_of_Wrath.KnownSpell && Hammer_of_Wrath.IsDistanceGood && Hammer_of_Wrath.IsSpellUsable)
        {
            Hammer_of_Wrath.Launch();
            return;
        }

    }

    private void BuffCombat()
    {
        if (Avenging_Wrath.KnownSpell && Avenging_Wrath.IsSpellUsable)
            Avenging_Wrath.Launch();
    }

    private void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (Blessing_of_Might.KnownSpell && !Blessing_of_Might.HaveBuff && Blessing_of_Might.IsSpellUsable)
            Blessing_of_Might.Launch();

        if (Retribution_Aura.KnownSpell && !Retribution_Aura.HaveBuff && Retribution_Aura.IsSpellUsable)
            Retribution_Aura.Launch();

        else if (Devotion_Aura.KnownSpell && !Devotion_Aura.HaveBuff && !Retribution_Aura.HaveBuff && Devotion_Aura.IsSpellUsable)
            Devotion_Aura.Launch();
    }

    private void Shield()
    {
        if (Guardian_of_Ancient_Kings.KnownSpell && ObjectManager.Me.HealthPercent > 0 && ObjectManager.Me.HealthPercent < 25 && Guardian_of_Ancient_Kings.IsSpellUsable)
            Guardian_of_Ancient_Kings.Launch();

        if (Divine_Shield.KnownSpell && ObjectManager.Me.HealthPercent > 0 && ObjectManager.Me.HealthPercent < 25 && Divine_Shield.IsSpellUsable)
            Divine_Shield.Launch();

        if (Divine_Protection.KnownSpell && ObjectManager.Me.HealthPercent > 0 && ObjectManager.Me.HealthPercent < 50 && Divine_Protection.IsSpellUsable)
            Divine_Protection.Launch();

        if (Ardent_Defender.KnownSpell && ObjectManager.Me.HealthPercent > 0 && ObjectManager.Me.HealthPercent < 5 && Ardent_Defender.IsSpellUsable)
            Ardent_Defender.Launch();
    }

    private void Heal()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (Lay_on_Hands.KnownSpell && ObjectManager.Me.HealthPercent > 0 && ObjectManager.Me.HealthPercent < 10 && Lay_on_Hands.IsSpellUsable)
        {
            Lay_on_Hands.Launch();
            return;
        }

        if (Flash_of_Light.KnownSpell && ObjectManager.Me.HealthPercent > 0 && ObjectManager.Me.HealthPercent < 50 && Flash_of_Light.IsSpellUsable)
            Flash_of_Light.Launch();

        if (Word_of_Glory.KnownSpell && ObjectManager.Me.HealthPercent > 0 && ObjectManager.Me.HealthPercent < 70 && Word_of_Glory.IsSpellUsable)
            Word_of_Glory.Launch();

        if (ObjectManager.Me.BarTwoPercentage < 50 && Divine_Plea.KnownSpell && Divine_Plea.IsSpellUsable)
            Divine_Plea.Launch();
    }

    private void TargetMoving()
    {
        if (ObjectManager.Target.GetDistance >= 5 && Hammer_of_Justice.KnownSpell && Hammer_of_Justice.IsDistanceGood && Hammer_of_Justice.IsSpellUsable)
            Hammer_of_Justice.Launch();
    }

    private void Decast()
    {
        if (ObjectManager.Target.IsCast && Rebuke.KnownSpell && Rebuke.IsSpellUsable && Rebuke.IsDistanceGood)
            Rebuke.Launch();
    }

    private void AvoidMelee()
    {
        if (ObjectManager.Target.GetDistance < 1)
            Keyboard.DownKey(nManager.Wow.Memory.WowProcess.MainWindowHandle, "{DOWN}");
    }
}

public class PaladinVind
{
    #region InitializeSpell

    Spell Seal_of_Command = new Spell("Seal of Command");
    Spell Seal_of_Righteousness = new Spell("Seal of Righteousness");
    Spell Seal_of_Insight = new Spell("Seal of Insight");
    Spell Seal_of_Truth = new Spell("Seal of Truth");

    Spell Blessing_of_Might = new Spell("Blessing of Might");

    Spell Judgement = new Spell("Judgement");

    Spell Retribution_Aura = new Spell("Retribution Aura");
    Spell Devotion_Aura = new Spell("Devotion Aura");
    Spell Crusader_Aura = new Spell("Crusader Aura");

    Spell Lay_on_Hands = new Spell("Lay on Hands");
    Spell Flash_of_Light = new Spell("Flash of Light");
    Spell Holy_Light = new Spell("Holy Light");

    Spell Exorcism = new Spell("Exorcism");
    Spell Crusader_Strike = new Spell("Crusader Strike");
    Spell Hammer_of_Wrath = new Spell("Hammer of Wrath");
    Spell Consecration = new Spell("Consecration");
    Spell Divine_Protection = new Spell("Divine Protection");
    Spell Divine_Shield = new Spell("Divine Shield");
    Spell Hammer_of_Justice = new Spell("Hammer of Justice");
    Spell Divine_Storm = new Spell("Divine Storm");
    Spell Avengers_Shield = new Spell("Avenger's Shield");
    Spell Templars_Verdict = new Spell("Templar's Verdict");
    Spell Holy_Shock = new Spell("Holy Shock");
    Spell Shield_of_the_Righteous = new Spell("Shield of the Righteous");
    Spell Sacred_Duty = new Spell("Sacred Duty");
    Spell Hammer_of_the_Righteous = new Spell("Hammer of the Righteous");
    Spell Holy_Wrath = new Spell("Holy Wrath");
    Spell Guardian_of_Ancient_Kings = new Spell("Guardian of Ancient Kings");
    Spell Ardent_Defender = new Spell("Ardent Defender");
    Spell Word_of_Glory = new Spell("Word of Glory");
    Spell Avenging_Wrath = new Spell("Avenging Wrath");
    Spell Inquisition = new Spell("Inquisition");
    Spell The_Art_of_War = new Spell("The Art of War");
    Spell Divine_Plea = new Spell("Divine Plea");
    Spell Zealotry = new Spell("Zealotry");
    Spell Rebuke = new Spell("Rebuke");
    Timer Templars_Verdict_Timer = new Timer(0);


    #endregion InitializeSpell

    public PaladinVind()
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
            Thread.Sleep(350);
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
        if (Judgement.KnownSpell && Judgement.IsDistanceGood && Judgement.IsSpellUsable)
            Judgement.Launch();
    }

    public void Combat()
    {
        BuffCombat();
        AvoidMelee();
        Shield();
        Heal();
        TargetMoving();
        Decast();
        Heal();

        if (Seal_of_Insight.KnownSpell || Seal_of_Truth.KnownSpell || Seal_of_Righteousness.KnownSpell)
        {
            if (Seal_of_Insight.KnownSpell && !Seal_of_Insight.HaveBuff && Seal_of_Insight.IsSpellUsable && ObjectManager.Me.BarTwoPercentage < 25)
                Seal_of_Insight.Launch();

            if (Seal_of_Truth.KnownSpell && !Seal_of_Truth.HaveBuff && Seal_of_Truth.IsSpellUsable && ObjectManager.Me.BarTwoPercentage > 80)
                Seal_of_Truth.Launch();

            else if (!Seal_of_Truth.KnownSpell && Seal_of_Righteousness.KnownSpell && !Seal_of_Righteousness.HaveBuff && Seal_of_Righteousness.IsSpellUsable && ObjectManager.Me.BarTwoPercentage > 80)
                Seal_of_Righteousness.Launch();
        }

        if (Inquisition.KnownSpell && Inquisition.IsSpellUsable)
        {
            Inquisition.Launch();
            return;
        }

        if (ObjectManager.Target.GetDistance >= 5 && Avenging_Wrath.KnownSpell && Avenging_Wrath.IsSpellUsable)
        {
            Avenging_Wrath.Launch();
            return;
        }

        if (Hammer_of_Wrath.KnownSpell && Hammer_of_Wrath.IsDistanceGood && Hammer_of_Wrath.IsSpellUsable)
        {
            Hammer_of_Wrath.Launch();
            return;
        }

        if (The_Art_of_War.TargetHaveBuff && Exorcism.KnownSpell && Exorcism.IsDistanceGood && Exorcism.IsSpellUsable)
        {
            Exorcism.Launch();
            return;
        }

        if (Templars_Verdict_Timer.IsReady && Templars_Verdict.KnownSpell && Templars_Verdict.IsDistanceGood && Templars_Verdict.IsSpellUsable)
        {
            Lua.RunMacroText("/use 13");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Lua.RunMacroText("/use 14");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");

            Templars_Verdict.Launch();
            Templars_Verdict_Timer = new Timer(1000 * 12);
            return;
        }

        if (Crusader_Strike.KnownSpell && Crusader_Strike.IsDistanceGood && Crusader_Strike.IsSpellUsable)
        {
            if (ObjectManager.GetNumberAttackPlayer() >= 3 && Divine_Storm.KnownSpell && Divine_Storm.IsDistanceGood && Divine_Storm.IsSpellUsable)
            {
                Divine_Storm.Launch();
                return;
            }

            else
            {
                Crusader_Strike.Launch();
                return;
            }

        }

        if (Judgement.KnownSpell && Judgement.IsDistanceGood && Judgement.IsSpellUsable)
        {
            Judgement.Launch();
            return;
        }

        if (Holy_Wrath.KnownSpell && Holy_Wrath.IsDistanceGood && Holy_Wrath.IsSpellUsable)
        {
            Holy_Wrath.Launch();
            return;
        }

        if (Zealotry.KnownSpell && Zealotry.IsDistanceGood && Zealotry.IsSpellUsable)
        {
            Zealotry.Launch();
            return;
        }
    }

    private void BuffCombat()
    {
        if (Avenging_Wrath.KnownSpell && Avenging_Wrath.IsSpellUsable)
            Avenging_Wrath.Launch();
    }

    private void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (Blessing_of_Might.KnownSpell && !Blessing_of_Might.HaveBuff && Blessing_of_Might.IsSpellUsable)
            Blessing_of_Might.Launch();

        if (Retribution_Aura.KnownSpell && !Retribution_Aura.HaveBuff && Retribution_Aura.IsSpellUsable)
            Retribution_Aura.Launch();

        else if (Devotion_Aura.KnownSpell && !Devotion_Aura.HaveBuff && !Retribution_Aura.HaveBuff && Devotion_Aura.IsSpellUsable)
            Devotion_Aura.Launch();
    }

    private void Shield()
    {
        if (Guardian_of_Ancient_Kings.KnownSpell && ObjectManager.Me.HealthPercent > 0 && ObjectManager.Me.HealthPercent < 25 && Guardian_of_Ancient_Kings.IsSpellUsable)
            Guardian_of_Ancient_Kings.Launch();

        if (Divine_Shield.KnownSpell && ObjectManager.Me.HealthPercent > 0 && ObjectManager.Me.HealthPercent < 25 && Divine_Shield.IsSpellUsable)
            Divine_Shield.Launch();

        if (Divine_Protection.KnownSpell && ObjectManager.Me.HealthPercent > 0 && ObjectManager.Me.HealthPercent < 50 && Divine_Protection.IsSpellUsable)
            Divine_Protection.Launch();

        if (Ardent_Defender.KnownSpell && ObjectManager.Me.HealthPercent > 0 && ObjectManager.Me.HealthPercent < 5 && Ardent_Defender.IsSpellUsable)
            Ardent_Defender.Launch();
    }

    private void Heal()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (Lay_on_Hands.KnownSpell && ObjectManager.Me.HealthPercent > 0 && ObjectManager.Me.HealthPercent < 10 && Lay_on_Hands.IsSpellUsable)
        {
            Lay_on_Hands.Launch();
            return;
        }

        if (Flash_of_Light.KnownSpell && ObjectManager.Me.HealthPercent > 0 && ObjectManager.Me.HealthPercent < 50 && Flash_of_Light.IsSpellUsable)
            Flash_of_Light.Launch();

        if (Word_of_Glory.KnownSpell && ObjectManager.Me.HealthPercent > 0 && ObjectManager.Me.HealthPercent < 70 && Word_of_Glory.IsSpellUsable)
            Word_of_Glory.Launch();

        if (ObjectManager.Me.BarTwoPercentage < 50 && Divine_Plea.KnownSpell && Divine_Plea.IsSpellUsable)
            Divine_Plea.Launch();
    }

    private void TargetMoving()
    {
        if (ObjectManager.Target.GetDistance >= 5 && Hammer_of_Justice.KnownSpell && Hammer_of_Justice.IsDistanceGood && Hammer_of_Justice.IsSpellUsable)
            Hammer_of_Justice.Launch();
    }

    private void Decast()
    {
        if (ObjectManager.Target.IsCast && Rebuke.KnownSpell && Rebuke.IsSpellUsable && Rebuke.IsDistanceGood)
            Rebuke.Launch();
    }

    private void AvoidMelee()
    {
        if (ObjectManager.Target.GetDistance < 1)
            Keyboard.DownKey(nManager.Wow.Memory.WowProcess.MainWindowHandle, "{DOWN}");
    }

}

public class Paladin
{
    #region InitializeSpell

    Spell Seal_of_Command = new Spell("Seal of Command");
    Spell Seal_of_Righteousness = new Spell("Seal of Righteousness");
    Spell Seal_of_Insight = new Spell("Seal of Insight");

    Spell Blessing_of_Might = new Spell("Blessing of Might");

    Spell Judgement = new Spell("Judgement");

    Spell Retribution_Aura = new Spell("Retribution Aura");
    Spell Devotion_Aura = new Spell("Devotion Aura");

    Spell Lay_on_Hands = new Spell("Lay on Hands");
    Spell Flash_of_Light = new Spell("Flash of Light");
    Spell Holy_Light = new Spell("Holy Light");

    Spell Exorcism = new Spell("Exorcism");
    Spell Crusader_Strike = new Spell("Crusader Strike");
    Spell Hammer_of_Wrath = new Spell("Hammer of Wrath");
    Spell Consecration = new Spell("Consecration");
    Spell Divine_Protection = new Spell("Divine Protection");
    Spell Divine_Shield = new Spell("Divine Shield");
    Spell Hammer_of_Justice = new Spell("Hammer of Justice");
    Spell Divine_Storm = new Spell("Divine Storm");
    Spell Avengers_Shield = new Spell("Avenger's Shield");
    Spell Templars_Verdict = new Spell("Templar's Verdict");
    Spell Holy_Shock = new Spell("Holy Shock");
    Spell Shield_of_the_Righteous = new Spell("Shield of the Righteous");
    Spell Sacred_Duty = new Spell("Sacred Duty");
    Spell Hammer_of_the_Righteous = new Spell("Hammer of the Righteous");
    Spell Holy_Wrath = new Spell("Holy Wrath");
    Spell Guardian_of_Ancient_Kings = new Spell("Guardian of Ancient Kings");
    Spell Ardent_Defender = new Spell("Ardent Defender");
    Spell Word_of_Glory = new Spell("Word of Glory");
    Spell Avenging_Wrath = new Spell("Avenging Wrath");
    Spell Inquisition = new Spell("Inquisition");
    Spell The_Art_of_War = new Spell("The Art of War");
    Spell Divine_Plea = new Spell("Divine Plea");
    Spell Zealotry = new Spell("Zealotry");
    Spell Rebuke = new Spell("Rebuke");


    #endregion InitializeSpell

    public Paladin()
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
                    if (ObjectManager.Me.Target != lastTarget && Judgement.IsDistanceGood)
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
        if (Judgement.KnownSpell && Judgement.IsDistanceGood && Judgement.IsSpellUsable)
            Judgement.Launch();
    }

    public void Combat()
    {
        AvoidMelee();
        Heal();
        TargetMoving();
        Heal();

        if (Crusader_Strike.KnownSpell && Crusader_Strike.IsDistanceGood && Crusader_Strike.IsSpellUsable)
        {
            Lua.RunMacroText("/use 13");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Lua.RunMacroText("/use 14");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");

            Crusader_Strike.Launch();
        }

        if (Judgement.KnownSpell && Judgement.IsDistanceGood && Judgement.IsSpellUsable)
            Judgement.Launch();
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
        if (ObjectManager.Me.IsMounted)
            return;

        if (Seal_of_Righteousness.KnownSpell && !Seal_of_Righteousness.HaveBuff && Seal_of_Righteousness.IsSpellUsable)
            Seal_of_Righteousness.Launch();

        if (Devotion_Aura.KnownSpell && !Devotion_Aura.HaveBuff && Devotion_Aura.IsSpellUsable)
            Devotion_Aura.Launch();
    }

    private void Heal()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (Lay_on_Hands.KnownSpell && ObjectManager.Me.HealthPercent > 0 && ObjectManager.Me.HealthPercent < 10 && Lay_on_Hands.IsSpellUsable)
        {
            Lay_on_Hands.Launch();
            return;
        }

        if (Flash_of_Light.KnownSpell && ObjectManager.Me.HealthPercent > 0 && ObjectManager.Me.HealthPercent < 50 && Flash_of_Light.IsSpellUsable)
            Flash_of_Light.Launch();

        if (Word_of_Glory.KnownSpell && ObjectManager.Me.HealthPercent > 0 && ObjectManager.Me.HealthPercent < 70 && Word_of_Glory.IsSpellUsable)
            Word_of_Glory.Launch();

        if (ObjectManager.Me.BarTwoPercentage < 50 && Divine_Plea.KnownSpell && Divine_Plea.IsSpellUsable)
            Divine_Plea.Launch();
    }

    private void TargetMoving()
    {
        if (ObjectManager.Target.GetDistance >= 5 && Hammer_of_Justice.KnownSpell && Hammer_of_Justice.IsDistanceGood && Hammer_of_Justice.IsSpellUsable)
            Hammer_of_Justice.Launch();
    }

    private void AvoidMelee()
    {
        if (ObjectManager.Target.GetDistance < 1)
            Keyboard.DownKey(nManager.Wow.Memory.WowProcess.MainWindowHandle, "{DOWN}");
    }
}

#endregion CustomClass
