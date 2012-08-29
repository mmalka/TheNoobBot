/*
*
 * CustomClass Developpement Kit by Vesper 
*
*/

/* First of all, we call all the dependencies */
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

/* We now declare the main Class */
public class Main : ICustomClass
{
    internal static float range = 3.5f; // This is the default range for every class if not specified
    internal static bool loop = true;

    public float Range
    {
        get { return range; }
        set { range = value; }
    }

    public void Initialize()
    {
        try
        {
            Logging.WriteFight("Loading combat system.");
            switch (ObjectManager.Me.WowClass)
            {
                #region Paladin Specialisation checking
                case WoWClass.Paladin:
                    var Retribution_Spell = new Spell("Templar's Verdict"); // Main DPS spell that does not exist in others specialisations.
                    var Protection_Spell = new Spell("Avenger's Shield"); // Main Tank spell that does not exist in others specialisations.
                    var Holy_Spell = new Spell("Holy Shock"); // Main Heal spell that does not exist in others specialisations.
                    if (Retribution_Spell.KnownSpell)
                    {
                        Logging.WriteFight("Loading Paladin Retribution class...");
                        new Paladin_Retribution();
                        break;
                    }
                    else if (Protection_Spell.KnownSpell)
                    {
                        Logging.WriteFight("Loading Paladin Protection class...");
                        new Paladin_Protection();
                        break;
                    }
                    else if (Holy_Spell.KnownSpell)
                    {
                        Logging.WriteFight("Loading Paladin Holy class...");
                        new Paladin_Holy();
                        // Only the Paladin Holy area is documented, I let you read the rest of the spec carefully, example Tank Def cd usage or DPS burst as ret.
                        break;
                    }
                    else
                    {
                        Logging.WriteFight("No specialisation detected."); 
                        /* If we don't find any of the three spell, it means we are probably low level, 
                        *  and then, we will just play like a retribution Paladin.
                         * The fact is : We will only use Crusader Strike & Judgement at low level
                        */
                        Logging.WriteFight("Loading Paladin Retribution class...");
                        new Paladin_Retribution();
                        break;
                    }             
                #endregion
                default:
                    Dispose(); // In that case, an issue made the bot unable to retrieve your class, so, we just close the Combat System.
                    break;
            }
        }
        catch { }
        Logging.WriteFight("Combat system stopped."); // We wont reach this line until an error happend in the Class we are using or if we are done with the class.
    }

    public void Dispose()
    {
        Logging.WriteFight("Combat system stopped.");
        loop = false;
    }

    public void ShowConfiguration()
    {
        MessageBox.Show("This CustomClass has no settings available."); // We don't setup any config GUI for now.
    }
}


#region Paladin
public class Paladin_Holy // We register the Paladin_Holy gameplay
{
    
    /**
      * We set up spell list, we should define the spell we will need later,
      * As you can see, we define the racial and professions based spells as well as others spells.
    **/
    #region Professions & Racial
    private readonly Spell Arcane_Torrent = new Spell("Arcane Torrent");
    private readonly Spell Lifeblood = new Spell("Lifeblood");
    private readonly Spell Stoneform = new Spell("Stoneform");
    private readonly Spell Tailoring = new Spell("Tailoring");
    private readonly Spell Leatherworking = new Spell("Leatherworking");
    private readonly Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    private readonly Spell War_Stomp = new Spell("War Stomp");
    private readonly Spell Berserking = new Spell("Berserking");
    #endregion

    #region Paladin Aura
    private readonly Spell DevotionAura = new Spell("Devotion Aura");
    private readonly Spell RetributionAura = new Spell("Retribution Aura");
    private readonly Spell ConcentrationAura = new Spell("Concentration Aura");
    private readonly Spell ResistanceAura = new Spell("Resistance Aura");
    private readonly Spell CrusaderAura = new Spell("Crusader Aura");
    #endregion

    #region Paladin Seals & Buffs
    private readonly Spell SealOfTheRighteousness = new Spell("Seal of Righteousness");
    private readonly Spell SealOfTruth = new Spell("Seal of Truth");
    private readonly Spell SealOfInsight = new Spell("Seal of Insight");
    private readonly Spell SealOfJustice = new Spell("Seal of Justice");
    private readonly Spell BlessingOfMight = new Spell("Blessing of Might");
    private readonly Spell BlessingOfKings = new Spell("Blessing of Kings");
    #endregion

    #region Offensive Spell
    private readonly Spell HammerOfJustice = new Spell("Hammer of Justice");
    private readonly Spell HammerOfWrath = new Spell("Hammer of Wrath");
    private readonly Spell Judgement = new Spell("Judgement");
    private readonly Spell HolyShock = new Spell("Holy Shock");
    private readonly Spell Exorcism = new Spell("Exorcism");
    #endregion

    #region Offensive Cooldown
    private readonly Spell Inquisition = new Spell("Inquisition");
    private readonly Spell GuardianOfAncientKings = new Spell("Guardian of Ancient Kings");
    Timer BurstTime = new Timer(0);
    private readonly Spell Zealotry = new Spell("Zealotry"); 
    private readonly Spell AvengingWrath = new Spell("Avenging Wrath");
    #endregion

    #region Defensive Cooldown
    private readonly Spell DivineProtection = new Spell("Divine Protection");
    private readonly Spell DivineShield = new Spell("Divine Shield");
    private readonly Spell HandOfProtection = new Spell("Hand Of Protection");
    // 25771 = Forbearance
    #endregion
        
    #region Healing Spell
    private readonly Spell DivinePlea = new Spell("Divine Plea");
    private readonly Spell DivineLight = new Spell("Divine Light");
    private readonly Spell HolyRadiance = new Spell("Holy Radiance");
    private readonly Spell FlashOfLight = new Spell("Flash of Light");
    private readonly Spell HolyLight = new Spell("Holy Light");
    private readonly Spell LayOnHands = new Spell("Lay on Hands");
    private readonly Spell WorldOfGlory = new Spell("Word of Glory");
    #endregion

    public Paladin_Holy()
    {
        Main.range = 20f; // The main range for the Holy Paladin is set to 20m

        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            try
            {
                if (!ObjectManager.Me.IsMounted)
                {
                    Patrolling(); // We first, execute the Patrolling function, see below.

                    if (Fight.InFight && ObjectManager.Me.Target > 0) // If we are in fight and we've got 1 target or more attacking us.
                    {
                        if (ObjectManager.Me.Target != lastTarget && Judgement.IsDistanceGood) // We use the Pull function only if the "lastTarget" is not defined yet.
                        {
                            Pull(); // We execute our Pull cycle. In the Holy Paladin case, is just pulling with a Judgement.
                            lastTarget = ObjectManager.Me.Target;
                        }

                        Combat(); // Since our target is clearly defined, we now use our combat mode, and we will loop this function.
                    }
                }
                else if (ObjectManager.Me.IsMounted)
                {
                    Patrolling(); // We are a paladin, so we can use Aura while mounted.
                }
            }
            catch
            {
            }
            Thread.Sleep(250); // GCD / Lag
        }
    }

    private void Pull()
    {
        if (Judgement.KnownSpell && Judgement.IsDistanceGood && Judgement.IsSpellUsable)
        {
            Judgement.Launch();
        }
    }

    private void Combat()
    {
        /**
        *
        * Here, we use our functions in the order we want.
        * As you see, we first use DPS abilities, then use Heal abilities, then use DPS CoolDown, then loop back to DPS.
        **/
        DPS_Cycle();

        Heal();

        DPS_Burst();
    }

    private void Patrolling()
    {
        /*
         You can use the "Patrolling" function (out of combat) to refresh your spell, seals, blessings, and, why not, Water Walk potion or spells (DK etc).
        */
        if (!ObjectManager.Me.IsMounted)
        {
            Seal();
            Blessing();
            Aura();
        }
        else if(ObjectManager.Me.IsMounted)
        {
            Aura(); // We may prefer Crusader Aura while mounted.
        }
    }

    private void Seal()
    {
        if (ObjectManager.Me.IsMounted)
            return;
        /* As we are currently a Holy paladin, we put priority over SealOfInsight (healing/regen on fight) */
        if (SealOfInsight.KnownSpell)
        {
            if (!SealOfInsight.HaveBuff && SealOfInsight.IsSpellUsable)
            {
                SealOfInsight.Launch();
            }
        }
        else if (SealOfTruth.KnownSpell)
        {
        /* If we don't have the Insight seal, then we use SealOfTruth, for a good DPS */
            if (!SealOfTruth.HaveBuff && SealOfTruth.IsSpellUsable)
            {
                SealOfTruth.Launch();
            }
        }
        else if (SealOfTheRighteousness.KnownSpell)
        {
        /**
        * If we don't have Insight nor Truth, we use the Righteousness seal (we should be low level)
        * As time of now, what you can clearly understand, is that we DONT need to make custom class for a specific level, 
        * because most of the "low level" spells are also used at max level, so only the max level spells wont be executed if we corretly check "Spell.KnownSpell".
        **/
            if (!SealOfTheRighteousness.HaveBuff && SealOfTheRighteousness.IsSpellUsable)
            {
                {
                    SealOfTheRighteousness.Launch();
                }
            }
        }
    }


    private void Blessing()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (BlessingOfMight.KnownSpell && !BlessingOfMight.HaveBuff && BlessingOfMight.IsSpellUsable)
        {
            BlessingOfMight.Launch(); // BoM have a better mana bonus (regen) than just getting 5% of INT.
        }
        elseif (BlessingOfKings.KnownSpell && !BlessingOfKings.HaveBuff && BlessingOfKings.IsSpellUsable)
        {
            BlessingOfKings.Launch();
        }
    }

    private void Aura()
    {
        if (ObjectManager.Me.IsMounted && !CrusaderAura.IsSpellUsable)
            return;
        else if (ObjectManager.Me.IsMounted && !CrusaderAura.HaveBuff && CrusaderAura.IsSpellUsable)
            CrusaderAura.Launch();
        else if (!ConcentrationAura.HaveBuff && ConcentrationAura.IsSpellUsable)
            ConcentrationAura.Launch();
        else if (!ConcentrationAura.HaveBuff && !RetributionAura.HaveBuff && RetributionAura.IsSpellUsable)
            RetributionAura.Launch();
        else if (!ConcentrationAura.HaveBuff && !DevotionAura.HaveBuff && !RetributionAura.HaveBuff && DevotionAura.IsSpellUsable)
            DevotionAura.Launch();
    }

    private void Heal()
    {
        if (DivineShield.KnownSpell && ObjectManager.Me.HealthPercent > 0 && ObjectManager.Me.HealthPercent <= 5 && !ObjectManager.Me.HaveBuff(25771) && DivineShield.IsSpellUsable)
        {
            DivineShield.Launch(); // If we are down to 5% life and we don't have the Forbearance buff, and we can use DivineShield, then we use it.
            return;
        }
        if (LayOnHands.KnownSpell && ObjectManager.Me.HealthPercent > 0 && ObjectManager.Me.HealthPercent <= 20 && !ObjectManager.Me.HaveBuff(25771) && LayOnHands.IsSpellUsable)
        {
            LayOnHands.Launch(); // If we are down to 20% life and we don't have the Forbearance buff, and we can use Lay On Hands, then we use it.
            return;
        }
        if (ObjectManager.Me.BarTwoPercentage < 10)
        {
            // If we are down 10% mana and we got the "Arcane_Torrent" racial spell, then we use it.
            if (Arcane_Torrent.KnownSpell && Arcane_Torrent.IsSpellUsable)
                Arcane_Torrent.Launch();
            if (DivinePlea.KnownSpell && DivinePlea.IsSpellUsable)
            {
                // As Arcane_Torrent is out of GCD, we instantly use DivinePlea right after.
                DivinePlea.Launch();
                return;
            }
        }
        if (ObjectManager.Me.HealthPercent > 0 && ObjectManager.Me.HealthPercent < 50)
        {
            // If we are down 50% life, we start healing, every time we "return", we close the "Heal" function, and continue looping to DPS_Burst/DPS functions, then come back here when reaching the Heal function again.
            if (WorldOfGlory.KnownSpell && WorldOfGlory.IsSpellUsable)
                WorldOfGlory.Launch(); // If WOG is usable, we use it, and instantly use another Heal spell.
            if (DivineLight.KnownSpell && DivineLight.IsSpellUsable) // The IsSpellUsable will be affected by mana left, our top priority is Divine Light as we are still confortable with 50% life.
            {
                DivineLight.Launch();
                return;
            }
            if (FlashOfLight.KnownSpell && FlashOfLight.IsSpellUsable)
            {
                FlashOfLight.Launch();
                return;
            }
            if (HolyLight.KnownSpell && HolyLight.IsSpellUsable)
            {
                HolyLight.Launch();
                return;
            }
        }
        if (ObjectManager.Me.HealthPercent >= 0 && ObjectManager.Me.HealthPercent < 30)
        {
            if (WorldOfGlory.KnownSpell && WorldOfGlory.IsSpellUsable)
                WorldOfGlory.Launch();
            if (DivineProtection.KnownSpell && DivineProtection.IsSpellUsable)
                DivineProtection.Launch();
            // We now use DivineProtection (20% def) if we reach 30% or less life, and this time, our top priority is flashofLight for its casting speed and heal amount.
            if (FlashOfLight.KnownSpell && FlashOfLight.IsSpellUsable)
            {
                FlashOfLight.Launch();
                return;
            }
            if (HolyLight.KnownSpell && HolyLight.IsSpellUsable)
            {
                HolyLight.Launch();
                return;
            }
            if (DivineLight.KnownSpell && DivineLight.IsSpellUsable)
            {
                DivineLight.Launch();
                return;
            }
        }
    }
    private void DPS_Burst()
    {
        if (!Inquisition.HaveBuff && Inquisition.KnownSpell && Inquisition.IsSpellUsable && ObjectManager.Me.HolyPower == 3)
        {
            Inquisition.Launch(); // We will use Inquisition (as long as our Holy Power are not consumed by World of Glory, so, not so much time)
        }
        if (AvengingWrath.KnownSpell && AvengingWrath.IsSpellUsable)
        {
            AvengingWrath.Launch(); // We don't return after use of Inquisition, it's always better to use all the offensive technic at the same time.
        }
        
        return;
    }
    private void DPS_Cycle()
    {
        if (HammerOfJustice.KnownSpell && HammerOfJustice.IsDistanceGood && HammerOfJustice.IsSpellUsable)
        {
           // TODO : If target can be stun, if not, it will be a pure loss of DPS.
            HammerOfJustice.Launch(); // When the target reach us, we will be forced to avoid damage, so we use Stun.
            return;
        }
        if (HolyShock.KnownSpell && HolyShock.IsDistanceGood && HolyShock.IsSpellUsable)
        {
            HolyShock.Launch();
            return;
        }
        if (Exorcism.KnownSpell && Exorcism.IsDistanceGood && Exorcism.IsSpellUsable)
        {
            Exorcism.Launch();
            return;
        }
        if (Judgement.KnownSpell && Judgement.IsDistanceGood && Judgement.IsSpellUsable)
        {
            Judgement.Launch(); // It will be cast only if we are getting mana issue.
            return;
        }
    }
}

public class Paladin_Protection
{
    #region Professions & Racial
    private readonly Spell Arcane_Torrent = new Spell("Arcane Torrent");
    private readonly Spell Lifeblood = new Spell("Lifeblood");
    private readonly Spell Stoneform = new Spell("Stoneform");
    private readonly Spell Tailoring = new Spell("Tailoring");
    private readonly Spell Leatherworking = new Spell("Leatherworking");
    private readonly Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    private readonly Spell War_Stomp = new Spell("War Stomp");
    private readonly Spell Berserking = new Spell("Berserking");
    #endregion

    #region Paladin Aura
    private readonly Spell DevotionAura = new Spell("Devotion Aura");
    private readonly Spell RetributionAura = new Spell("Retribution Aura");
    private readonly Spell ResistanceAura = new Spell("Resistance Aura");
    private readonly Spell CrusaderAura = new Spell("Crusader Aura");
    #endregion

    #region Paladin Seals & Buffs
    private readonly Spell SealOfTheRighteousness = new Spell("Seal of Righteousness");
    private readonly Spell SealOfTruth = new Spell("Seal of Truth");
    private readonly Spell SealOfJustice = new Spell("Seal of Justice");
    private readonly Spell BlessingOfMight = new Spell("Blessing of Might");
    private readonly Spell BlessingOfKings = new Spell("Blessing of Kings");
    #endregion

    #region Offensive Spell
    private readonly Spell HammerOfTheRighteous = new Spell("Hammer of the Righteous");
    private readonly Spell ShieldOfTheRighteous = new Spell("Shield of the Righteous");
    private readonly Spell AvengersShield = new Spell("Avenger's Shield");
    private readonly Spell CrusaderStrike = new Spell("Crusader Strike");
    private readonly Spell Consecration = new Spell("Consecration");
    private readonly Spell HolyWrath = new Spell("Holy Wrath");
    private readonly Spell Judgement = new Spell("Judgement");
    private readonly Spell HammerOfJustice = new Spell("Hammer of Justice");
    private readonly Spell HammerOfWrath = new Spell("Hammer of Wrath");
    private readonly Spell Exorcism = new Spell("Exorcism");
    #endregion

    #region Offensive Cooldown
    private readonly Spell AvengingWrath = new Spell("Avenging Wrath");
    private readonly Spell Inquisition = new Spell("Inquisition");
    #endregion

    #region Defensive Cooldown
    Timer OnCD = new Timer(0);
    private readonly Spell GuardianOfAncientKings = new Spell("Guardian of Ancient Kings");
    private readonly Spell HolyShield = new Spell("Holy Shield");
    private readonly Spell ArdentDefender = new Spell("Ardent Defender");
    private readonly Spell DivineGuardian = new Spell("Divine Guardian");
    private readonly Spell DivineProtection = new Spell("Divine Protection");
    private readonly Spell DivineShield = new Spell("Divine Shield");
    private readonly Spell HandOfProtection = new Spell("Hand Of Protection");
    #endregion
    
    #region Healing Spell
    private readonly Spell DivinePlea = new Spell("Divine Plea");
    private readonly Spell DivineLight = new Spell("Divine Light");
    private readonly Spell FlashOfLight = new Spell("Flash of Light");
    private readonly Spell HolyLight = new Spell("Holy Light");
    private readonly Spell LayOnHands = new Spell("Lay on Hands");
    private readonly Spell WorldOfGlory = new Spell("Word of Glory");
    #endregion

    public Paladin_Protection()
    {
        Main.range = 3.6f; // Range

        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            try
            {
                if (!ObjectManager.Me.IsMounted)
                {
                    Patrolling();

                    if (Fight.InFight && ObjectManager.Me.Target > 0)
                    {
                        if (ObjectManager.Me.Target != lastTarget && Exorcism.IsDistanceGood)
                        {
                            Pull();
                            lastTarget = ObjectManager.Me.Target;
                        }

                        Combat();
                    }
                }
            }
            catch
            {
            }
            Thread.Sleep(250);
        }
    }

    private void Pull()
    {
        if (DivinePlea.KnownSpell && DivinePlea.IsSpellUsable)
        {
            DivinePlea.Launch();
            Thread.Sleep(250);
        }
        if (Inquisition.KnownSpell && Inquisition.IsSpellUsable)
        {
            Inquisition.Launch();
            Thread.Sleep(250);
        }
        if (AvengingWrath.KnownSpell && AvengingWrath.IsSpellUsable)
        {
            AvengingWrath.Launch();
        }
        if (Exorcism.KnownSpell && Exorcism.IsDistanceGood && Exorcism.IsSpellUsable)
        {
            Exorcism.Launch();
            Thread.Sleep(250);
        }
        if (Judgement.KnownSpell && Judgement.IsDistanceGood && Judgement.IsSpellUsable)
        {
            Judgement.Launch();
            Thread.Sleep(250);
        }
        if (AvengersShield.KnownSpell && AvengersShield.IsDistanceGood && AvengersShield.IsSpellUsable)
        {
            AvengersShield.Launch();
        }
    }

    private void Combat()
    {
        if (OnCD.IsReady)
            Defense_Cycle();

        DPS_Cycle();

        Heal();

        DPS_Burst();
    }

    private void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            Seal();
            Blessing();
            Aura();
        }
    }

    private void Seal()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (SealOfTruth.KnownSpell)
        {
            if (!SealOfTruth.HaveBuff && SealOfTruth.IsSpellUsable)
            {
                SealOfTruth.Launch();
            }
        }
        else if (SealOfTheRighteousness.KnownSpell)
            if (!SealOfTheRighteousness.HaveBuff && SealOfTheRighteousness.IsSpellUsable)
            {
                {
                    SealOfTheRighteousness.Launch();
                }
            }
    }

    private void Blessing()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (BlessingOfMight.KnownSpell && !BlessingOfMight.HaveBuff && BlessingOfMight.IsSpellUsable)
        {
            BlessingOfMight.Launch();
        }
    }

    private void Aura()
    {
        if (ObjectManager.Me.IsMounted && !CrusaderAura.IsSpellUsable)
            return;
        else if (ObjectManager.Me.IsMounted && !CrusaderAura.HaveBuff && CrusaderAura.IsSpellUsable)
            CrusaderAura.Launch();
        else if (!DevotionAura.HaveBuff && DevotionAura.IsSpellUsable)
            DevotionAura.Launch();
        else if (!DevotionAura.HaveBuff && !RetributionAura.HaveBuff && RetributionAura.IsSpellUsable)
            RetributionAura.Launch();
    }

    private void Heal()
    {
        if (DivineShield.KnownSpell && ObjectManager.Me.HealthPercent > 0 && ObjectManager.Me.HealthPercent <= 5 && DivineShield.IsSpellUsable && !ObjectManager.Me.HaveBuff(25771))
        {
            DivineShield.Launch();
            return;
        }
        if (LayOnHands.KnownSpell && ObjectManager.Me.HealthPercent > 0 && ObjectManager.Me.HealthPercent <= 20 && LayOnHands.IsSpellUsable && !ObjectManager.Me.HaveBuff(25771))
        {
            LayOnHands.Launch();
            return;
        }
        if (ObjectManager.Me.BarTwoPercentage < 10)
        {
            if (Arcane_Torrent.KnownSpell && Arcane_Torrent.IsSpellUsable)
                Arcane_Torrent.Launch();
            if (DivinePlea.KnownSpell && DivinePlea.IsSpellUsable)
            {
                DivinePlea.Launch();
                return;
            }
        }
        if (ObjectManager.Me.HealthPercent > 0 && ObjectManager.Me.HealthPercent < 50)
        {
            if (WorldOfGlory.KnownSpell && WorldOfGlory.IsSpellUsable)
                WorldOfGlory.Launch();
            if (DivineLight.KnownSpell && DivineLight.IsSpellUsable)
            {
                DivineLight.Launch();
                return;
            }
            if (FlashOfLight.KnownSpell && FlashOfLight.IsSpellUsable)
            {
                FlashOfLight.Launch();
                return;
            }
            if (HolyLight.KnownSpell && HolyLight.IsSpellUsable)
            {
                HolyLight.Launch();
                return;
            }
        }
        if (ObjectManager.Me.HealthPercent >= 0 && ObjectManager.Me.HealthPercent < 30)
        {
            if (WorldOfGlory.KnownSpell && WorldOfGlory.IsSpellUsable)
                WorldOfGlory.Launch();
            if (DivineProtection.KnownSpell && DivineProtection.IsSpellUsable)
                DivineProtection.Launch();
            if (FlashOfLight.KnownSpell && FlashOfLight.IsSpellUsable)
            {
                FlashOfLight.Launch();
                return;
            }
            if (HolyLight.KnownSpell && HolyLight.IsSpellUsable)
            {
                HolyLight.Launch();
                return;
            }
            if (DivineLight.KnownSpell && DivineLight.IsSpellUsable)
            {
                DivineLight.Launch();
                return;
            }
        }
    }
    private void DPS_Burst()
    {
        if (AvengingWrath.KnownSpell && AvengingWrath.IsSpellUsable && ObjectManager.Me.HolyPower == 3)
        {
            AvengingWrath.Launch();
            if (!Inquisition.HaveBuff && Inquisition.KnownSpell && Inquisition.IsSpellUsable)
            {
                Inquisition.Launch();
            }
            return;
        }
    }
    private void Defense_Cycle()
    {
        if (HolyShield.KnownSpell && HolyShield.IsSpellUsable)
        {
            HolyShield.Launch();
            OnCD = new Timer(1000 * 10);
            return;
        }
        if (HammerOfJustice.KnownSpell && HammerOfJustice.IsSpellUsable)
        {
            HammerOfJustice.Launch();
            OnCD = new Timer(1000 * 6);
            return;
        }
        if (DivineShield.KnownSpell && DivineShield.IsSpellUsable && !ObjectManager.Me.HaveBuff(25771))
        {
            DivineShield.Launch();
            OnCD = new Timer(1000 * 8);
            return;
        }
        if (DivineProtection.KnownSpell && DivineProtection.IsSpellUsable)
        {
            DivineProtection.Launch();
            OnCD = new Timer(1000 * 10);
            return;
        }
        if (GuardianOfAncientKings.KnownSpell && GuardianOfAncientKings.IsSpellUsable)
        {
            GuardianOfAncientKings.Launch();
            OnCD = new Timer(1000 * 12);
            return;
        }
        if (ArdentDefender.KnownSpell && ArdentDefender.IsSpellUsable)
        {
            ArdentDefender.Launch();
            OnCD = new Timer(1000 * 10);
            return;
        }
        if (LayOnHands.KnownSpell && LayOnHands.IsSpellUsable && !ObjectManager.Me.HaveBuff(25771))
        {
            LayOnHands.Launch();
            OnCD = new Timer(1000 * 5);
            return;
        }
        if (WorldOfGlory.KnownSpell && WorldOfGlory.IsSpellUsable)
        {
            WorldOfGlory.Launch();
            OnCD = new Timer(1000 * 5);
            return;
        }
        if (HandOfProtection.KnownSpell && HandOfProtection.IsSpellUsable && !ObjectManager.Me.HaveBuff(25771))
        {
            HandOfProtection.Launch();
            OnCD = new Timer(1000 * 8);
            return;
        }
    }
    private void DPS_Cycle()
    {
        /*if (HammerOfJustice.KnownSpell && HammerOfJustice.IsDistanceGood && HammerOfJustice.IsSpellUsable)
        {
           // TODO : If target can be stun, if not, it will be a pure loss of DPS.
            HammerOfJustice.Launch();
            return;
        }*/
        if (ShieldOfTheRighteous.KnownSpell && ShieldOfTheRighteous.IsSpellUsable && ShieldOfTheRighteous.IsDistanceGood && (ObjectManager.Me.HaveBuff(90174) || ObjectManager.Me.HolyPower == 3))
        {
            ShieldOfTheRighteous.Launch();
            return;
        }
        if (ObjectManager.GetNumberAttackPlayer() >= 3 && !ObjectManager.Me.HaveBuff(90174) && ObjectManager.Me.HolyPower != 3)
        {
            if (HammerOfTheRighteous.KnownSpell && HammerOfTheRighteous.IsDistanceGood && HammerOfTheRighteous.IsSpellUsable)
            {
                HammerOfTheRighteous.Launch();
                return;
            }
        }
        else
        {
            if (CrusaderStrike.KnownSpell && CrusaderStrike.IsDistanceGood && CrusaderStrike.IsSpellUsable && !ObjectManager.Me.HaveBuff(90174) && ObjectManager.Me.HolyPower != 3)
            {
                CrusaderStrike.Launch();
                return;
            }
        }
        if (AvengersShield.KnownSpell && AvengersShield.IsDistanceGood && AvengersShield.IsSpellUsable && !ObjectManager.Me.HaveBuff(90174) && ObjectManager.Me.HolyPower != 3)
        {
            AvengersShield.Launch();
            return;
        }
        if (HammerOfWrath.KnownSpell && HammerOfWrath.IsDistanceGood && HammerOfWrath.IsSpellUsable && !ObjectManager.Me.HaveBuff(90174) && ObjectManager.Me.HolyPower != 3)
        {
            HammerOfWrath.Launch();
            return;
        }
        if (Judgement.KnownSpell && Judgement.IsDistanceGood && Judgement.IsSpellUsable && !ObjectManager.Me.HaveBuff(90174) && ObjectManager.Me.HolyPower != 3)
        {
            Judgement.Launch();
            return;
        }
        if (Consecration.KnownSpell && Consecration.IsSpellUsable && ObjectManager.Me.BarTwoPercentage > 50 && !ObjectManager.Me.HaveBuff(90174) && ObjectManager.Me.HolyPower != 3 && !Judgement.IsSpellUsable && !CrusaderStrike.IsSpellUsable)
        {
            Consecration.Launch();
            return;
        }
        if (HolyWrath.KnownSpell && HolyWrath.IsSpellUsable && !ObjectManager.Me.HaveBuff(90174) && ObjectManager.Me.HolyPower != 3 && !Judgement.IsSpellUsable && !CrusaderStrike.IsSpellUsable)
        {
            HolyWrath.Launch();
            return;
        }
    }
}

public class Paladin_Retribution
{
    #region Professions & Racial
    private readonly Spell Arcane_Torrent = new Spell("Arcane Torrent");
    private readonly Spell Lifeblood = new Spell("Lifeblood");
    private readonly Spell Stoneform = new Spell("Stoneform");
    private readonly Spell Tailoring = new Spell("Tailoring");
    private readonly Spell Leatherworking = new Spell("Leatherworking");
    private readonly Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    private readonly Spell War_Stomp = new Spell("War Stomp");
    private readonly Spell Berserking = new Spell("Berserking");
    #endregion

    #region Paladin Aura
    private readonly Spell DevotionAura = new Spell("Devotion Aura");
    private readonly Spell RetributionAura = new Spell("Retribution Aura");
    private readonly Spell ResistanceAura = new Spell("Resistance Aura");
    private readonly Spell CrusaderAura = new Spell("Crusader Aura");
    #endregion

    #region Paladin Seals & Buffs
    private readonly Spell SealOfTheRighteousness = new Spell("Seal of Righteousness");
    private readonly Spell SealOfTruth = new Spell("Seal of Truth");
    private readonly Spell SealOfJustice = new Spell("Seal of Justice");
    private readonly Spell BlessingOfMight = new Spell("Blessing of Might");
    private readonly Spell BlessingOfKings = new Spell("Blessing of Kings");
    #endregion

    #region Offensive Spell
    private readonly Spell TemplarsVerdict = new Spell("Templar's Verdict");
    private readonly Spell HammerOfJustice = new Spell("Hammer of Justice");
    private readonly Spell DivineStorm = new Spell("Divine Storm");
    private readonly Spell HammerOfWrath = new Spell("Hammer of Wrath");
    private readonly Spell CrusaderStrike = new Spell("Crusader Strike");
    private readonly Spell Consecration = new Spell("Consecration");
    private readonly Spell HolyWrath = new Spell("Holy Wrath");
    private readonly Spell Judgement = new Spell("Judgement");
    private readonly Spell Exorcism = new Spell("Exorcism");
    #endregion

    #region Offensive Cooldown
    private readonly Spell Inquisition = new Spell("Inquisition");
    Timer InquisitionToUseInPriotiy = new Timer(0);
    private readonly Spell GuardianOfAncientKings = new Spell("Guardian of Ancient Kings");
    Timer BurstTime = new Timer(0);
    private readonly Spell Zealotry = new Spell("Zealotry"); 
    private readonly Spell AvengingWrath = new Spell("Avenging Wrath");
    #endregion

    #region Defensive Cooldown
    private readonly Spell DivineProtection = new Spell("Divine Protection");
    private readonly Spell DivineShield = new Spell("Divine Shield");
    private readonly Spell HandOfProtection = new Spell("Hand Of Protection");
    // 25771 = Forbearance
    #endregion
        
    #region Healing Spell
    private readonly Spell DivinePlea = new Spell("Divine Plea");
    private readonly Spell DivineLight = new Spell("Divine Light");
    private readonly Spell FlashOfLight = new Spell("Flash of Light");
    private readonly Spell HolyLight = new Spell("Holy Light");
    private readonly Spell LayOnHands = new Spell("Lay on Hands");
    private readonly Spell WorldOfGlory = new Spell("Word of Glory");
    #endregion

    public Paladin_Retribution()
    {
        Main.range = 3.6f; // Range

        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            try
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
            }
            catch
            {
            }
            Thread.Sleep(250);
        }
    }

    private void Pull()
    {
        if (Judgement.KnownSpell && Judgement.IsDistanceGood && Judgement.IsSpellUsable)
        {
            Judgement.Launch();
        }
    }

    private void Combat()
    {
        DPS_Cycle();

        Heal();

        DPS_Burst();
    }

    private void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            Seal();
            Blessing();
            Aura();
        }
    }

    private void Seal()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (SealOfTruth.KnownSpell)
        {
            if (!SealOfTruth.HaveBuff && SealOfTruth.IsSpellUsable)
            {
                SealOfTruth.Launch();
            }
        }
        else if (SealOfTheRighteousness.KnownSpell)
            if (!SealOfTheRighteousness.HaveBuff && SealOfTheRighteousness.IsSpellUsable)
            {
                {
                    SealOfTheRighteousness.Launch();
                }
            }
    }

    private void Blessing()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (BlessingOfMight.KnownSpell && !BlessingOfMight.HaveBuff && BlessingOfMight.IsSpellUsable)
        {
            BlessingOfMight.Launch();
        }
    }

    private void Aura()
    {
        if (ObjectManager.Me.IsMounted && !CrusaderAura.IsSpellUsable)
            return;
        else if (ObjectManager.Me.IsMounted && !CrusaderAura.HaveBuff && CrusaderAura.IsSpellUsable)
            CrusaderAura.Launch();
        else if (!RetributionAura.HaveBuff && RetributionAura.IsSpellUsable)
            RetributionAura.Launch();
        else if (!DevotionAura.HaveBuff && !RetributionAura.HaveBuff && DevotionAura.IsSpellUsable)
            DevotionAura.Launch();
    }

    private void Heal()
    {
        if (DivineShield.KnownSpell && ObjectManager.Me.HealthPercent > 0 && ObjectManager.Me.HealthPercent <= 5 && !ObjectManager.Me.HaveBuff(25771) && DivineShield.IsSpellUsable)
        {
            DivineShield.Launch();
            return;
        }
        if (LayOnHands.KnownSpell && ObjectManager.Me.HealthPercent > 0 && ObjectManager.Me.HealthPercent <= 20 && !ObjectManager.Me.HaveBuff(25771) && LayOnHands.IsSpellUsable)
        {
            LayOnHands.Launch();
            return;
        }
        if (ObjectManager.Me.BarTwoPercentage < 10)
        {
            if (Arcane_Torrent.KnownSpell && Arcane_Torrent.IsSpellUsable)
                Arcane_Torrent.Launch();
            if (DivinePlea.KnownSpell && DivinePlea.IsSpellUsable)
            {
                DivinePlea.Launch();
                return;
            }
        }
        if (ObjectManager.Me.HealthPercent > 0 && ObjectManager.Me.HealthPercent < 50)
        {
            if (WorldOfGlory.KnownSpell && WorldOfGlory.IsSpellUsable)
                WorldOfGlory.Launch();
            if (DivineLight.KnownSpell && DivineLight.IsSpellUsable)
            {
                DivineLight.Launch();
                return;
            }
            if (FlashOfLight.KnownSpell && FlashOfLight.IsSpellUsable)
            {
                FlashOfLight.Launch();
                return;
            }
            if (HolyLight.KnownSpell && HolyLight.IsSpellUsable)
            {
                HolyLight.Launch();
                return;
            }
        }
        if (ObjectManager.Me.HealthPercent >= 0 && ObjectManager.Me.HealthPercent < 30)
        {
            if (WorldOfGlory.KnownSpell && WorldOfGlory.IsSpellUsable)
                WorldOfGlory.Launch();
            if (DivineProtection.KnownSpell && DivineProtection.IsSpellUsable)
                DivineProtection.Launch();
            if (FlashOfLight.KnownSpell && FlashOfLight.IsSpellUsable)
            {
                FlashOfLight.Launch();
                return;
            }
            if (HolyLight.KnownSpell && HolyLight.IsSpellUsable)
            {
                HolyLight.Launch();
                return;
            }
            if (DivineLight.KnownSpell && DivineLight.IsSpellUsable)
            {
                DivineLight.Launch();
                return;
            }
        }
    }
    private void DPS_Burst()
    {
        if (GuardianOfAncientKings.HaveBuff || !GuardianOfAncientKings.IsSpellUsable)
        {
            if (((GuardianOfAncientKings.HaveBuff && BurstTime.IsReady) || !GuardianOfAncientKings.IsSpellUsable) && Zealotry.KnownSpell && Zealotry.IsSpellUsable && ObjectManager.Me.HolyPower == 3)
            {
                Zealotry.Launch();
                Thread.Sleep(250);
                if ((!Inquisition.HaveBuff || InquisitionToUseInPriotiy.IsReady) && Inquisition.KnownSpell && Inquisition.IsSpellUsable)
                {
                    Inquisition.Launch();
                    InquisitionToUseInPriotiy = new Timer(1000 * (12 * 3 - 6));
                }
                AvengingWrath.Launch();
                return;
            }
            if (!Zealotry.KnownSpell && AvengingWrath.KnownSpell && AvengingWrath.IsSpellUsable)
            {
                AvengingWrath.Launch();
                return;
            }
        }
        else
            if (GuardianOfAncientKings.KnownSpell && GuardianOfAncientKings.IsSpellUsable && Zealotry.IsSpellUsable)
            {
                GuardianOfAncientKings.Launch();
                BurstTime = new Timer(1000 * 6);
                return;
            }
    }
    private void DPS_Cycle()
    {
        /*if (HammerOfJustice.KnownSpell && HammerOfJustice.IsDistanceGood && HammerOfJustice.IsSpellUsable)
        {
           // TODO : If target can be stun, if not, it will be a pure loss of DPS.
            HammerOfJustice.Launch();
            return;
        }*/
        if (Inquisition.KnownSpell && (!Inquisition.HaveBuff || InquisitionToUseInPriotiy.IsReady) && Inquisition.IsSpellUsable && (ObjectManager.Me.HaveBuff(90174) || ObjectManager.Me.HolyPower == 3))
        {
            if (Zealotry.IsSpellUsable && (GuardianOfAncientKings.HaveBuff || !GuardianOfAncientKings.IsSpellUsable))
            {
                DPS_Burst();
                return;
            }
            else
                Inquisition.Launch();
            InquisitionToUseInPriotiy = new Timer(1000 * (12 * 3 - 6));
            return;
        }
        if (TemplarsVerdict.KnownSpell && Inquisition.HaveBuff && TemplarsVerdict.IsSpellUsable && TemplarsVerdict.IsDistanceGood && (ObjectManager.Me.HaveBuff(90174) || ObjectManager.Me.HolyPower == 3))
        {
            TemplarsVerdict.Launch();
            return;
        }
        if (!Zealotry.HaveBuff && ObjectManager.GetNumberAttackPlayer() >= 3 && !ObjectManager.Me.HaveBuff(90174) && ObjectManager.Me.HolyPower != 3)
        {
            if (DivineStorm.KnownSpell && DivineStorm.IsDistanceGood && DivineStorm.IsSpellUsable)
            {
                DivineStorm.Launch();
                return;
            }
        }
        else
        {
            if (CrusaderStrike.KnownSpell && CrusaderStrike.IsDistanceGood && CrusaderStrike.IsSpellUsable && !ObjectManager.Me.HaveBuff(90174) && ObjectManager.Me.HolyPower != 3)
            {
                CrusaderStrike.Launch();
                return;
            }
        }
        if (Exorcism.KnownSpell && Exorcism.IsDistanceGood && Exorcism.IsSpellUsable && ObjectManager.Me.HaveBuff(59578) && !ObjectManager.Me.HaveBuff(90174) && ObjectManager.Me.HolyPower != 3 && Inquisition.HaveBuff)
        {
            Exorcism.Launch();
            return;
        }
        if (HammerOfWrath.KnownSpell && HammerOfWrath.IsDistanceGood && HammerOfWrath.IsSpellUsable && !ObjectManager.Me.HaveBuff(90174) && ObjectManager.Me.HolyPower != 3 && Inquisition.HaveBuff)
        {
            HammerOfWrath.Launch();
            return;
        }
        if (!Zealotry.HaveBuff && ObjectManager.GetNumberAttackPlayer() >= 3 && !ObjectManager.Me.HaveBuff(90174) && ObjectManager.Me.HolyPower != 3)
        {
            if (DivineStorm.KnownSpell && DivineStorm.IsDistanceGood && DivineStorm.IsSpellUsable)
            {
                DivineStorm.Launch();
                return;
            }
        }
        else
        {
            if (CrusaderStrike.KnownSpell && CrusaderStrike.IsDistanceGood && CrusaderStrike.IsSpellUsable && !ObjectManager.Me.HaveBuff(90174) && ObjectManager.Me.HolyPower != 3)
            {
                CrusaderStrike.Launch();
                return;
            }
        }
        if (Judgement.KnownSpell && Judgement.IsDistanceGood && Judgement.IsSpellUsable && !ObjectManager.Me.HaveBuff(90174) && ObjectManager.Me.HolyPower != 3)
        {
            Judgement.Launch();
            return;
        }
        if (HolyWrath.KnownSpell && HolyWrath.IsSpellUsable && !ObjectManager.Me.HaveBuff(90174) && ObjectManager.Me.HolyPower != 3 && !Judgement.IsSpellUsable && !CrusaderStrike.IsSpellUsable && !Zealotry.HaveBuff && Inquisition.HaveBuff)
        {
            HolyWrath.Launch();
            return;
        }
        if (Consecration.KnownSpell && Consecration.IsSpellUsable && ObjectManager.Me.BarTwoPercentage > 50 && !ObjectManager.Me.HaveBuff(90174) && ObjectManager.Me.HolyPower != 3 && !Judgement.IsSpellUsable && !CrusaderStrike.IsSpellUsable && !Zealotry.HaveBuff && Inquisition.HaveBuff)
        {
            Consecration.Launch();
            return;
        }
    }
}
#endregion