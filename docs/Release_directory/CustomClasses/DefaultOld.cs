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
    internal static float range = 3.5f;
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
            Logging.WriteFight("Loading Custom Class");

            switch (ObjectManager.Me.WowClass)
            {
                case WoWClass.DeathKnight:
                    new Deathknight();
                    break;
                case WoWClass.Druid:
                    var mangle = new Spell("Mangle");
                    if (mangle.KnownSpell)
                    {
                        Logging.WriteFight("Druit Deral CC selected.");
                        new DruidDeral();
                    }
                    else
                    {
                        Logging.WriteFight("Druit Balance CC selected.");
                        new DruidBalance();
                    }
                    break;
                case WoWClass.Hunter:
                    var explosiveShot = new Spell("Explosive Shot");
                    var aimedShot = new Spell("Aimed Shot");

                    var callPet1 = new Spell("Call Pet 1");
                    var callPet2 = new Spell("Call Pet 2");
                    var callPet3 = new Spell("Call Pet 3");
                    var callPet4 = new Spell("Call Pet 4");
                    var callPet5 = new Spell("Call Pet 5");

                    if (callPet5.KnownSpell)
                        Hunter.CallPet = callPet5;
                    else if (callPet4.KnownSpell)
                        Hunter.CallPet = callPet4;
                    else if (callPet3.KnownSpell)
                        Hunter.CallPet = callPet3;
                    else if (callPet2.KnownSpell)
                        Hunter.CallPet = callPet2;
                    else
                        Hunter.CallPet = callPet1;

                    if (explosiveShot.KnownSpell)
                    {
                        new Hunter.HunterSurv();
                        Logging.WriteFight("Hunter CC Surv selected.");
                    }

                    if (aimedShot.KnownSpell)
                    {
                        new Hunter.HunterMark();
                        Logging.WriteFight("Hunter CC Mark selected.");
                    }

                    else
                    {
                        Logging.WriteFight("Hunter CC selected.");
                        new Hunter.HunterDefault();
                    }

                    break;
                case WoWClass.Mage:
                    Logging.WriteFight("Mage CC selected.");
                    new Mage();
                    break;
                case WoWClass.Paladin:
                    Logging.WriteFight("Paladin CC selected.");
                    new Paladin();
                    break;
                case WoWClass.Priest:
                    Logging.WriteFight("Priest CC selected.");
                    new Priest();
                    break;
                case WoWClass.Rogue:
                    Logging.WriteFight("Rogue CC selected.");
                    new Rogue();
                    break;
                case WoWClass.Shaman:
                    Logging.WriteFight("Shaman CC selected.");
                    new Shaman();
                    break;
                case WoWClass.Warlock:
                    Logging.WriteFight("Warlock CC selected.");
                    new Warlock();
                    break;
                case WoWClass.Warrior:
                    Logging.WriteFight("Warrior CC selected.");
                    new Warrior();
                    break;
                default:
                    Dispose();
                    break;
            }
        }
        catch {}
        Logging.WriteFight("Custom Class closed.");
    }

    public void Dispose()
    {
        Logging.WriteFight("Closing Custom Class");
        loop = false;
    }

    public void ShowConfiguration()
    {
        MessageBox.Show("No setting for this custom class");
    }
}

#region CustomClass

public class Paladin
{
    #region InitializeSpell

    private readonly Spell _avengingWrath = new Spell("Avenging Wrath");
    private readonly Spell _blessingOfMight = new Spell("Blessing of Might");
    private readonly Spell _consecration = new Spell("Consecration");
    private readonly Spell _crusaderStrike = new Spell("Crusader Strike");

    private readonly Spell _devotionAura = new Spell("Devotion Aura");

    private readonly Spell _divineLight = new Spell("Divine Light");


    private readonly Spell _divineProtection = new Spell("Divine Protection");
    private readonly Spell _divineStorm = new Spell("Divine Storm");
    private readonly Spell _flashOfLight = new Spell("Flash of Light");
    private readonly Spell _guardianOfAncientKings = new Spell("Guardian of Ancient Kings");
    private readonly Spell _hammerOfJustice = new Spell("Hammer of Justice");
    private readonly Spell _hammerOfWrath = new Spell("Hammer of Wrath");
    private readonly Spell _holyLight = new Spell("Holy Light");

    private readonly Spell _inquisition = new Spell("Inquisition");
    private readonly Spell _judgement = new Spell("Judgement");
    private readonly Spell _layOnHands = new Spell("Lay on Hands");
    private readonly Spell _retributionAura = new Spell("Retribution Aura");
    private readonly Spell _sealOfRighteousness = new Spell("Seal of Righteousness");
    private readonly Spell _sealOfTruth = new Spell("Seal of Truth");
    private readonly Spell _templarsVerdict = new Spell("Templar's Verdict");
    private readonly Spell _wordOfGlory = new Spell("Word of Glory");
    private readonly Spell _zealotry = new Spell("Zealotry");

    #endregion InitializeSpell

    public Paladin()
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
                        if (ObjectManager.Me.Target != lastTarget && _judgement.IsDistanceGood)
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
            Thread.Sleep(500);
        }
    }

    private void Pull()
    {
        JudgementManager();
    }

    private void Combat()
    {
        JudgementManager();

        Heal();

        Others();
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

        if (_sealOfTruth.KnownSpell)
        {
            if (!_sealOfTruth.HaveBuff && _sealOfTruth.IsSpellUsable)
            {
                _sealOfTruth.Launch();
            }
        }
        else if (_sealOfRighteousness.KnownSpell)
            if (!_sealOfRighteousness.HaveBuff && _sealOfRighteousness.IsSpellUsable)
            {
                {
                    _sealOfRighteousness.Launch();
                }
            }
    }

    private void Blessing()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (_blessingOfMight.KnownSpell && !_blessingOfMight.HaveBuff && _blessingOfMight.IsSpellUsable)
        {
            _blessingOfMight.Launch();
        }
    }

    private void JudgementManager()
    {
        if (_judgement.KnownSpell &&
            _judgement.IsDistanceGood &&
            _judgement.IsSpellUsable)
        {
            _judgement.Launch();
        }
    }

    private void Aura()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (_retributionAura.KnownSpell)
        {
            if (!_retributionAura.HaveBuff &&
                _retributionAura.IsSpellUsable)
            {
                _retributionAura.Launch();
            }
        }
        else if (_devotionAura.KnownSpell)
        {
            if (!_devotionAura.HaveBuff &&
                _devotionAura.IsSpellUsable)
            {
                _devotionAura.Launch();
            }
        }
    }

    private void Heal()
    {
        if (_layOnHands.KnownSpell &&
            ObjectManager.Me.HealthPercent > 0 &&
            ObjectManager.Me.HealthPercent < 20 &&
            ObjectManager.Me.Level < 70 &&
            _layOnHands.IsSpellUsable)
        {
            _layOnHands.Launch();
            return;
        }

        if (_flashOfLight.KnownSpell &&
            ObjectManager.Me.HealthPercent > 50 &&
            ObjectManager.Me.HealthPercent < 70 &&
            _flashOfLight.IsSpellUsable)
        {
            _flashOfLight.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent > 0 &&
            ObjectManager.Me.HealthPercent < 50)
        {
            if (_wordOfGlory.KnownSpell &&
                _wordOfGlory.IsSpellUsable)
            {
                _wordOfGlory.Launch();
                return;
            }
            if (_divineLight.KnownSpell &&
                _divineLight.IsSpellUsable)
            {
                _divineLight.Launch();
                return;
            }
            if (_holyLight.KnownSpell &&
                _holyLight.IsSpellUsable)
            {
                _holyLight.Launch();
            }
        }
    }

    private void Others()
    {
        if (_hammerOfWrath.KnownSpell &&
            _hammerOfWrath.IsDistanceGood &&
            _hammerOfWrath.IsSpellUsable)
        {
            _hammerOfWrath.Launch();
            return;
        }

        if (_avengingWrath.KnownSpell &&
            _avengingWrath.IsSpellUsable)
        {
            _avengingWrath.Launch();
            return;
        }

        if (_guardianOfAncientKings.KnownSpell &&
            _guardianOfAncientKings.IsSpellUsable)
        {
            _guardianOfAncientKings.Launch();
            return;
        }

        if (_zealotry.KnownSpell &&
            _zealotry.IsSpellUsable &&
            !_zealotry.HaveBuff)
        {
            _zealotry.Launch();
            return;
        }

        if (_inquisition.KnownSpell &&
            _inquisition.IsSpellUsable &&
            !_inquisition.HaveBuff)
        {
            _inquisition.Launch();
            return;
        }

        if (_templarsVerdict.KnownSpell &&
            _templarsVerdict.IsSpellUsable &&
            !_templarsVerdict.IsDistanceGood)
        {
            _templarsVerdict.Launch();
            return;
        }

        if (_crusaderStrike.KnownSpell &&
            _crusaderStrike.IsDistanceGood &&
            _crusaderStrike.IsSpellUsable)
        {
            _crusaderStrike.Launch();
            return;
        }

        if (_hammerOfJustice.KnownSpell &&
            _hammerOfJustice.IsDistanceGood &&
            _hammerOfJustice.IsSpellUsable)
        {
            _hammerOfJustice.Launch();
            return;
        }

        /*
        if (Exorcism.KnownSpell &&
            Exorcism.IsDistanceGood &&
            Exorcism.IsSpellUsable)
        {
            Exorcism.Launch();
            return;
        }
        */

        if (_divineStorm.KnownSpell &&
            ObjectManager.GetNumberAttackPlayer() >= 2 &&
            _divineStorm.IsSpellUsable)
        {
            _divineStorm.Launch();
            return;
        }

        if (_divineProtection.KnownSpell &&
            _divineProtection.IsDistanceGood &&
            _divineProtection.IsSpellUsable)
        {
            if (ObjectManager.GetNumberAttackPlayer() > 1)
            {
                _divineProtection.Launch();
                return;
            }
        }

        if (_consecration.KnownSpell &&
            _consecration.IsDistanceGood &&
            _consecration.IsSpellUsable &&
            ObjectManager.Me.BarTwoPercentage > 65)
        {
            if (ObjectManager.GetNumberAttackPlayer() > 1)
            {
                _consecration.Launch();
            }
        }
    }
}

public class Deathknight
{
    #region InitializeSpell

    private readonly Spell _bloodPlague = new Spell("Blood Plague");
    private readonly Spell _bloodStrike = new Spell("Blood Strike");
    private readonly Spell _deathGrip = new Spell("Death Grip");
    private readonly Spell _deathStrike = new Spell("Death Strike");
    private readonly Spell _frostEver = new Spell("Frost Ever");
    private readonly Spell _frostStrike = new Spell("Frost Strike");
    private readonly Spell _hornOfWinter = new Spell("Horn of Winter");
    private readonly Spell _icyTouch = new Spell("Icy Touch");
    private readonly Spell _plagueStrike = new Spell("Plague Strike");
    private readonly Spell _runeStrike = new Spell("Rune Strike");

    #endregion InitializeSpell

    public Deathknight()
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
                        if (ObjectManager.Me.Target != lastTarget && _icyTouch.IsDistanceGood)
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
            Thread.Sleep(500);
        }
    }

    public void Pull()
    {
        if (_deathGrip.KnownSpell &&
            ObjectManager.Target.GetDistance > 15 &&
            _deathGrip.IsSpellUsable &&
            _deathGrip.IsDistanceGood)
        {
            _deathGrip.Launch();
        }
        else if (_icyTouch.KnownSpell &&
                 _icyTouch.IsSpellUsable &&
                 _icyTouch.IsDistanceGood)
        {
            _icyTouch.Launch();
        }
    }

    private void Combat()
    {
        Buff();


        if (_frostStrike.KnownSpell &&
            _frostStrike.IsSpellUsable &&
            _frostStrike.IsDistanceGood)
        {
            _frostStrike.Launch();
        }

        if (!_frostEver.HaveBuff &&
            _icyTouch.KnownSpell &&
            _icyTouch.IsSpellUsable &&
            _icyTouch.IsDistanceGood)
        {
            _icyTouch.Launch();
            FastCheck();
        }

        if (!_bloodPlague.HaveBuff &&
            _plagueStrike.KnownSpell &&
            _plagueStrike.IsSpellUsable &&
            _plagueStrike.IsDistanceGood)
        {
            _plagueStrike.Launch();
            FastCheck();
        }

        if (_deathStrike.KnownSpell)
        {
            if (_deathStrike.IsSpellUsable &&
                _deathStrike.IsDistanceGood)
            {
                _deathStrike.Launch();
                FastCheck();
            }
        }
        else if (_bloodStrike.KnownSpell)
        {
            if (_bloodStrike.IsSpellUsable &&
                _bloodStrike.IsDistanceGood)
            {
                _bloodStrike.Launch();
                FastCheck();
            }
        }
    }

    private void Patrolling()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        Buff();
    }

    private void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (!_hornOfWinter.HaveBuff &&
            _hornOfWinter.KnownSpell &&
            _hornOfWinter.IsSpellUsable)
        {
            _hornOfWinter.Launch();
        }
    }

    private void FastCheck()
    {
        if (_runeStrike.KnownSpell &&
            _runeStrike.IsSpellUsable &&
            _runeStrike.IsDistanceGood)
        {
            _runeStrike.Launch();
        }
    }
}

public class Warrior
{
    #region InitializeSpell

    private readonly Spell _battleShout = new Spell("Battle Shout");
    private readonly Spell _battleStance = new Spell("Battle Stance");
    private readonly Spell _berserking = new Spell("Berserking");
    private readonly Spell _bloodrage = new Spell("Bloodrage");
    private readonly Spell _bloodthirst = new Spell("Bloodthirst");
    private readonly Spell _charge = new Spell("Charge");
    private readonly Spell _execute = new Spell("Execute");
    private readonly Spell _hamstring = new Spell("Hamstring");
    private readonly Spell _heroicStrike = new Spell("Heroic Strike");
    private readonly Spell _overpower = new Spell("Overpower");
    private readonly Spell _rend = new Spell("Rend");
    private readonly Spell _slam = new Spell("Slam");
    private readonly Spell _strike = new Spell("Strike");
    private readonly Spell _victoryRush = new Spell("Victory Rush");

    #endregion InitializeSpell

    public Warrior()
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
                        if (ObjectManager.Me.Target != lastTarget && _charge.IsDistanceGood)
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
            Thread.Sleep(500);
        }
    }

    public void Pull()
    {
        if (!_battleStance.HaveBuff)
            _battleStance.Launch();


        if (_charge.KnownSpell &&
            ObjectManager.Target.GetDistance > 15 &&
            _charge.IsSpellUsable &&
            _charge.IsDistanceGood)
        {
            _charge.Launch();
        }
    }

    private void Combat()
    {
        if (_strike.KnownSpell &&
            _strike.IsSpellUsable &&
            _strike.IsDistanceGood)
        {
            _strike.Launch();
        }

        if (_bloodrage.KnownSpell &&
            _bloodrage.IsSpellUsable &&
            _bloodrage.IsDistanceGood &&
            ObjectManager.Me.HealthPercent > 40)
        {
            _bloodrage.Launch();
        }

        if (_berserking.KnownSpell &&
            _berserking.IsSpellUsable &&
            _berserking.IsDistanceGood)
        {
            _berserking.Launch();
        }

        if (_bloodthirst.KnownSpell &&
            _bloodthirst.IsDistanceGood &&
            _bloodthirst.IsSpellUsable)
        {
            _bloodthirst.Launch();
        }

        if (_hamstring.KnownSpell &&
            _hamstring.IsSpellUsable &&
            _hamstring.IsDistanceGood &&
            !_hamstring.TargetHaveBuff)
        {
            _hamstring.Launch();
        }

        else if (_rend.KnownSpell &&
                 !_rend.TargetHaveBuff &&
                 _rend.IsSpellUsable &&
                 _rend.IsDistanceGood)
        {
            _rend.Launch();
        }

        if (_victoryRush.KnownSpell &&
            _victoryRush.IsSpellUsable &&
            _victoryRush.IsDistanceGood)
        {
            _victoryRush.Launch();
        }

        else if (_battleShout.KnownSpell &&
                 _battleShout.IsSpellUsable &&
                 !_battleShout.HaveBuff)
        {
            _battleShout.Launch();
        }

        else if (_overpower.KnownSpell &&
                 _overpower.IsSpellUsable &&
                 _overpower.IsDistanceGood)
        {
            _overpower.Launch();
        }

        else if (_execute.KnownSpell &&
                 _execute.IsSpellUsable &&
                 ObjectManager.Target.HealthPercent <= 20)
        {
            _execute.Launch();
        }

        else
        {
            if (_slam.KnownSpell)
            {
                if (_slam.IsSpellUsable && _slam.IsDistanceGood && ObjectManager.Me.BarTwoPercentage > 35)
                    _slam.Launch();
            }
            else
            {
                if (_heroicStrike.IsSpellUsable && _heroicStrike.IsDistanceGood &&
                    ObjectManager.Me.BarTwoPercentage > 35)
                    _heroicStrike.Launch();
            }
        }
    }

    private void Patrolling()
    {
    }
}

public class Mage
{
    #region InitializeSpell

    private readonly Spell _arcaneIntellect = new Spell("Arcane Intellect");
    private readonly Spell _chilled = new Spell("Chilled");
    //private readonly Spell _conjureFood = new Spell("Conjure Food");
    //private readonly Spell _conjureRefreshment = new Spell("Conjure Refreshment");
    //private readonly Spell _conjureWater = new Spell("Conjure Water");
    private readonly Spell _fireBlast = new Spell("Fire Blast");
    private readonly Spell _fireball = new Spell("Fireball");
    private readonly Spell _frostArmor = new Spell("Frost Armor");
    private readonly Spell _frostNova = new Spell("Frost Nova");
    private readonly Spell _frostbolt = new Spell("Frostbolt");
    private readonly Spell _frostfireBolt = new Spell("Frostfire Bolt");
    private readonly Spell _hotStreak = new Spell("Hot Streak");
    private readonly Spell _iceArmor = new Spell("Ice Armor");
    private readonly Spell _iceLance = new Spell("Ice Lance");
    private readonly Spell _icyVeins = new Spell("Icy Veins");
    private readonly Spell _presenceOfMind = new Spell("Presence of Mind");
    private readonly Spell _pyroblast = new Spell("Pyroblast");
    private readonly Spell _summonWaterElemental = new Spell("Summon Water Elemental");

    #endregion InitializeSpell

    public Mage()
    {
        Main.range = 28.0f; // Range
        bool postCombatUsed = true;
        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            try
            {
                if (!ObjectManager.Me.IsMounted)
                {
                    Patrolling();
                    if (!Fight.InFight && !postCombatUsed)
                    {
                        PostCombat();
                    }

                    if (Fight.InFight && ObjectManager.Me.Target > 0)
                    {
                        if (ObjectManager.Me.Target != lastTarget && NukeSpell().IsDistanceGood)
                        {
                            Pull();
                            lastTarget = ObjectManager.Me.Target;
                        }

                        Combat();
                        postCombatUsed = false;
                    }
                }
            }
            catch
            {
            }
            Thread.Sleep(500);
        }
    }

    public void Pull()
    {
        if (_summonWaterElemental.KnownSpell &&
            _summonWaterElemental.IsSpellUsable &&
            ObjectManager.Pet.Guid == 0)
        {
            _summonWaterElemental.Launch();
        }

        if (NukeSpell().IsSpellUsable &&
            NukeSpell().IsDistanceGood)
        {
            NukeSpell().Launch();
        }
    }

    private void Combat()
    {
        if (_presenceOfMind.KnownSpell &&
            _presenceOfMind.IsSpellUsable)
        {
            _presenceOfMind.Launch();

            if (_pyroblast.KnownSpell &&
                _pyroblast.IsSpellUsable &&
                _pyroblast.IsDistanceGood)
            {
                _pyroblast.Launch();
            }
        }

        if (NukeSpell().IsSpellUsable &&
            NukeSpell().IsDistanceGood)
        {
            NukeSpell().Launch();
        }

        if (_icyVeins.KnownSpell &&
            _icyVeins.IsSpellUsable)
        {
            _icyVeins.Launch();
        }

        if (_fireBlast.KnownSpell &&
            _fireBlast.IsSpellUsable &&
            _fireBlast.IsDistanceGood)
        {
            _fireBlast.Launch();
        }

        if (_pyroblast.KnownSpell &&
            _hotStreak.HaveBuff &&
            _pyroblast.IsSpellUsable)
        {
            _pyroblast.Launch();
        }

        if (_frostNova.KnownSpell &&
            _frostNova.IsSpellUsable &&
            _frostNova.IsDistanceGood)
        {
            nManager.Wow.Helpers.Keybindings.DownKeybindings(Keybindings.MOVEBACKWARD);
            Thread.Sleep(300);
            _frostNova.Launch();
            Thread.Sleep(1600);
            nManager.Wow.Helpers.Keybindings.UpKeybindings(Keybindings.MOVEBACKWARD);
        }

        if (_iceLance.KnownSpell &&
            _iceLance.IsDistanceGood &&
            _iceLance.IsSpellUsable)
        {
            if (_frostNova.HaveBuff ||
                _chilled.HaveBuff)
            {
                _iceLance.Launch();
            }
        }
    }

    private void PostCombat()
    {
    }

    public void Patrolling()
    {
        if (_iceArmor.KnownSpell)
        {
            if (!_iceArmor.HaveBuff &&
                _iceArmor.IsSpellUsable)
            {
                _iceArmor.Launch();
            }
        }
        else
        {
            if (!_frostArmor.HaveBuff &&
                _frostArmor.KnownSpell &&
                _frostArmor.IsSpellUsable)
            {
                _frostArmor.Launch();
            }
        }

        if (!_arcaneIntellect.HaveBuff &&
            _arcaneIntellect.KnownSpell &&
            _arcaneIntellect.IsSpellUsable)
        {
            _arcaneIntellect.Launch();
        }
    }

    private Spell NukeSpell()
    {
        if (_frostfireBolt.KnownSpell)
        {
            return _frostfireBolt;
        }
        if (_frostbolt.KnownSpell)
        {
            return _frostbolt;
        }
        return _fireball;
    }
}

public class Warlock
{
    #region InitializeSpell

    private readonly Spell _corruption = new Spell("Corruption");
    private readonly Spell _curseOfAgony = new Spell("Curse of Agony");
    private readonly Spell _curseOfTheElements = new Spell("Curse of the Elements");
    private readonly Spell _demonArmor = new Spell("Demon Armor");
    private readonly Spell _demonSkin = new Spell("Demon Skin");
    private readonly Spell _demonicEmpowerment = new Spell("Demonic Empowerment");
    private readonly Spell _drainLife = new Spell("Drain Life");
    private readonly Spell _drainSoul = new Spell("Drain Soul");
    private readonly Spell _felArmor = new Spell("Fel Armor");
    private readonly Spell _felDomination = new Spell("Fel Domination");
    private readonly Spell _healthFunnel = new Spell("Health Funnel");
    private readonly Spell _immolate = new Spell("Immolate");
    private readonly Spell _lifeTap = new Spell("Life Tap");
    private readonly Spell _shadowBolt = new Spell("Shadow Bolt");
    private readonly Spell _shadowTrance = new Spell("Shadow Trance");
    private readonly Spell _summonFelguard = new Spell("Summon Felguard");
    private readonly Spell _summonImp = new Spell("Summon Imp");
    private readonly Spell _summonVoidwalker = new Spell("Summon Voidwalker");
    private readonly Spell _unstableAffliction = new Spell("Unstable Affliction");

    #endregion InitializeSpell

    public Warlock()
    {
        Main.range = 28.0f; // Range
        bool postCombatUsed = true;
        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            try
            {
                if (!ObjectManager.Me.IsMounted && !ObjectManager.Me.IsDeadMe)
                {
                    Patrolling();

                    if (!Fight.InFight && !postCombatUsed)
                    {
                        postCombatUsed = true;
                        PostCombat();
                    }

                    if (Fight.InFight && ObjectManager.Me.Target > 0)
                    {
                        if (ObjectManager.Me.Target != lastTarget && ObjectManager.Target.GetDistance <= Main.range)
                        {
                            Pull();
                            lastTarget = ObjectManager.Me.Target;
                        }

                        Combat();
                        postCombatUsed = false;
                    }
                }
            }
            catch
            {
            }
            Thread.Sleep(500);
        }
    }

    public void Pull()
    {
        Boolean hasDot = false;
        PetCheck();
        ArmorCheck();

        Lua.RunMacroText("/petattack");
        Logging.WriteFight("Launch Pet Attack");

        if (_demonicEmpowerment.KnownSpell &&
            _demonicEmpowerment.IsSpellUsable &&
            !_demonicEmpowerment.HaveBuff &&
            ObjectManager.Pet.Health > 0)
        {
            _demonicEmpowerment.Launch();
        }

        if (_immolate.KnownSpell &&
            _immolate.IsSpellUsable &&
            _immolate.IsDistanceGood)
        {
            _immolate.Launch();
            hasDot = true;
        }

        if (_curseOfTheElements.KnownSpell || _curseOfAgony.KnownSpell)
        {
            if (_curseOfTheElements.KnownSpell &&
                _curseOfTheElements.IsSpellUsable &&
                _curseOfTheElements.IsDistanceGood)
            {
                _curseOfTheElements.Launch();
                hasDot = true;
            }
            else if (_curseOfAgony.KnownSpell &&
                     _curseOfAgony.IsSpellUsable &&
                     _curseOfAgony.IsDistanceGood)
            {
                _curseOfAgony.Launch();
                hasDot = true;
            }
        }

        if (_corruption.KnownSpell &&
            _corruption.IsSpellUsable &&
            _corruption.IsDistanceGood)
        {
            _corruption.Launch();
            hasDot = true;
        }


        if (!hasDot)
        {
            if (_shadowBolt.KnownSpell &&
                _shadowBolt.IsSpellUsable &&
                _shadowBolt.IsDistanceGood)
            {
                _shadowBolt.Launch();
            }
        }
    }

    private void Combat()
    {
        PetCheck();
        LifeTap();
        PetHealth();
        AvoidMelee();

        if (_shadowTrance.HaveBuff)
        {
            if (_shadowBolt.KnownSpell &&
                _shadowBolt.IsSpellUsable &&
                _shadowBolt.IsDistanceGood)
            {
                _shadowBolt.Launch();
            }
            PetHealth();
        }

        if (_unstableAffliction.KnownSpell &&
            _unstableAffliction.IsSpellUsable &&
            _unstableAffliction.IsDistanceGood &&
            !ObjectManager.Target.HaveBuff(_unstableAffliction.Id))
        {
            _unstableAffliction.Launch();
            PetHealth();
        }

        if (_drainLife.KnownSpell &&
            _drainLife.IsDistanceGood &&
            ObjectManager.Me.HealthPercent < 70)
        {
            _drainLife.Launch();
            PetHealth();
        }
        else
        {
            if (_shadowBolt.KnownSpell &&
                _shadowBolt.IsSpellUsable &&
                _shadowBolt.IsDistanceGood)
            {
                _shadowBolt.Launch();
            }
            PetHealth();
        }

        if (_drainSoul.KnownSpell &&
            _drainSoul.IsDistanceGood &&
            ObjectManager.Target.HealthPercent < 25 &&
           ItemsManager.GetItemCountByIdLUA(6265) < 9)
        {
            _drainSoul.Launch();
            PetHealth();
        }
    }

    private void PostCombat()
    {
        LifeTap();
    }

    private void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            ArmorCheck();
        }
    }

    private void PetCheck()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if ((ObjectManager.Pet.Health == 0 ||
             ObjectManager.Pet.Guid == 0) && !ObjectManager.Me.IsMounted && !ObjectManager.Me.IsDeadMe)
        {
            if (_summonImp.KnownSpell)
            {
                Int32 nbSoulShard = ItemsManager.GetItemCountByIdLUA(6265); // Soul Shard

                if (nbSoulShard > 0 &&
                    (_summonVoidwalker.KnownSpell ||
                     _summonFelguard.KnownSpell))
                {
                    if (_summonFelguard.KnownSpell)
                    {
                        if (_felDomination.KnownSpell)
                        {
                            _felDomination.Launch();
                        }
                        _summonFelguard.Launch();
                        Thread.Sleep(1000);
                    }
                    else if (_summonVoidwalker.KnownSpell)
                    {
                        if (_felDomination.KnownSpell)
                        {
                            _felDomination.Launch();
                        }
                        _summonVoidwalker.Launch();
                        Thread.Sleep(1000);
                    }
                }
                else
                {
                    _summonImp.Launch();
                    Thread.Sleep(1000);
                }
            }
        }
    }

    private void PetHealth()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (_healthFunnel.KnownSpell)
            if (ObjectManager.Pet.HealthPercent > 0 &&
                ObjectManager.Pet.HealthPercent < 50 &&
                _healthFunnel.IsSpellUsable)
            {
                _healthFunnel.Launch();
                while (ObjectManager.Me.IsCast)
                {
                    if (ObjectManager.Pet.HealthPercent > 80 || ObjectManager.Pet.IsDead)
                        break;

                    Thread.Sleep(100);
                }
            }
    }

    private void LifeTap()
    {
        if (_lifeTap.KnownSpell)
            if (ObjectManager.Me.HealthPercent > 40 &&
                (ObjectManager.Me.BarTwoPercentage < ObjectManager.Me.HealthPercent))
            {
                while (ObjectManager.Me.HealthPercent > 35 &&
                       ObjectManager.Me.BarTwoPercentage < 65)
                {
                    _lifeTap.Launch();
                }
            }
    }

    private void ArmorCheck()
    {
        if (_felArmor.KnownSpell &&
            !_felArmor.HaveBuff &&
            _felArmor.IsSpellUsable)
        {
            _felArmor.Launch();
        }
        else if (_demonArmor.KnownSpell &&
                 !_felArmor.KnownSpell &&
                 !_demonArmor.HaveBuff &&
                 _demonArmor.IsSpellUsable)
        {
            _demonArmor.Launch();
        }
        else if (_demonSkin.KnownSpell &&
                 !_demonArmor.KnownSpell &&
                 !_felArmor.KnownSpell &&
                 !_demonSkin.HaveBuff &&
                 _demonSkin.IsSpellUsable)
        {
            _demonSkin.Launch();
        }
    }

    private void AvoidMelee()
    {
        if (ObjectManager.Target.GetDistance < 5 &&
            ObjectManager.Pet.IsAlive &&
            ObjectManager.Pet.Guid != 0 &&
            ObjectManager.Target.Target == ObjectManager.Pet.Guid)
        {
            nManager.Wow.Helpers.Keybindings.DownKeybindings(Keybindings.MOVEBACKWARD);
            Thread.Sleep(1500);
            nManager.Wow.Helpers.Keybindings.UpKeybindings(Keybindings.MOVEBACKWARD);
        }
    }
}

public class Shaman
{
    #region InitializeSpell

    private readonly Spell _ballLightning = new Spell("Ball Lightning");
    private readonly Spell _callOfTheElements = new Spell("Call of the Elements");
    private readonly Spell _earthShock = new Spell("Earth Shock");
    private readonly Spell _feralSpirit = new Spell("Feral Spirit");
    private readonly Spell _flametongueWeapon = new Spell("Flametongue Weapon");
    private readonly Spell _healingSurge = new Spell("Healing Surge");
    private readonly Spell _healingWave = new Spell("Healing Wave");
    private readonly Spell _lavaBurst = new Spell("Lava Burst");
    private readonly Spell _lavaLash = new Spell("Lava Lash");
    private readonly Spell _lightningBolt = new Spell("Lightning Bolt");
    private readonly Spell _searingTotem = new Spell("Searing Totem");
    private readonly Spell _stoneskinTotem = new Spell("Stoneskin Totem");

    private readonly Spell _stormstrike = new Spell("Stormstrike");
    private readonly Spell _totemicRecall = new Spell("Totemic Recall");
    private readonly Spell _unleashElements = new Spell("Unleash Elements");
    private readonly Spell _waterShield = new Spell("Water Shield");
    private readonly Spell _windfuryWeapon = new Spell("Windfury Weapon");
    private Timer _flametongueWeaponTimer = new Timer(0);
    private Timer _windfuryWeaponTimer = new Timer(0);

    #endregion InitializeSpell

    public Shaman()
    {
        Main.range = 23.0f; // Range

        UInt64 lastTarget = 0;
        bool postCombatUsed = true;

        while (Main.loop)
        {
            try
            {
                if (!ObjectManager.Me.IsMounted)
                {
                    Patrolling();

                    if (!Fight.InFight && !postCombatUsed)
                    {
                        postCombatUsed = true;
                        PostCombat();
                    }


                    if (Fight.InFight && ObjectManager.Me.Target > 0)
                    {
                        if (ObjectManager.Me.Target != lastTarget && ObjectManager.Target.GetDistance <= Main.range)
                        {
                            Pull();
                            lastTarget = ObjectManager.Me.Target;
                        }

                        Combat();
                        postCombatUsed = false;
                    }
                }
            }
            catch
            {
            }
            Thread.Sleep(500);
        }
    }

    private void Pull()
    {
        Buff();

        if (_callOfTheElements.KnownSpell)
        {
            if (_callOfTheElements.IsSpellUsable)
            {
                _callOfTheElements.Launch();
            }
        }
        else
        {
            if (_stoneskinTotem.KnownSpell &&
                _stoneskinTotem.IsSpellUsable)
            {
                _stoneskinTotem.Launch();
            }

            if (_searingTotem.IsSpellUsable &&
                _searingTotem.KnownSpell)
            {
                _searingTotem.Launch();
            }
        }
        _lightningBolt.Launch();
        CheckHeal();
    }

    private void Combat()
    {
        CheckHeal();

        if (_ballLightning.KnownSpell &&
            _ballLightning.IsSpellUsable &&
            _ballLightning.IsDistanceGood)
        {
            _ballLightning.Launch();
            CheckHeal();
        }

        if (_unleashElements.KnownSpell &&
            _unleashElements.IsSpellUsable &&
            _unleashElements.IsDistanceGood)
        {
            _unleashElements.Launch();
            CheckHeal();
        }

        if (_lavaLash.KnownSpell &&
            _lavaLash.IsSpellUsable &&
            _lavaLash.IsDistanceGood)
        {
            _lavaLash.Launch();
            CheckHeal();
        }

        if (_stormstrike.KnownSpell &&
            _stormstrike.IsSpellUsable &&
            _stormstrike.IsDistanceGood)
        {
            _stormstrike.Launch();
            CheckHeal();
        }

        if (_earthShock.KnownSpell &&
            _earthShock.IsSpellUsable &&
            _earthShock.IsDistanceGood)
        {
            _earthShock.Launch();
            CheckHeal();
        }

        if (_lavaBurst.KnownSpell &&
            _lavaBurst.IsSpellUsable &&
            _lavaBurst.IsDistanceGood)
        {
            _lavaBurst.Launch();
            CheckHeal();
        }

        if (_feralSpirit.KnownSpell &&
            _feralSpirit.IsSpellUsable &&
            _feralSpirit.IsDistanceGood)
        {
            _feralSpirit.Launch();
            CheckHeal();
        }

        if (_lightningBolt.KnownSpell &&
            _lightningBolt.IsSpellUsable &&
            _lightningBolt.IsDistanceGood)
        {
            _lightningBolt.Launch();
            CheckHeal();
        }
    }

    private void PostCombat()
    {
        if (_totemicRecall.KnownSpell &&
            _totemicRecall.IsSpellUsable)
        {
            _totemicRecall.Launch();
        }
    }

    private void Patrolling()
    {
        Buff();
    }

    private void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;


        if (_windfuryWeaponTimer.IsReady &&
            !_windfuryWeapon.HaveBuff &&
            _windfuryWeapon.KnownSpell &&
            _windfuryWeapon.IsSpellUsable)
        {
            _windfuryWeaponTimer = new Timer(1000 * 60 * 30);
            _windfuryWeapon.Launch();
        }

        if (ObjectManager.Me.IsMounted)
            return;

        if (_flametongueWeaponTimer.IsReady &&
            !_flametongueWeapon.HaveBuff &&
            _flametongueWeapon.KnownSpell &&
            _flametongueWeapon.IsSpellUsable)
        {
            _flametongueWeaponTimer = new Timer(1000 * 60 * 30);
            _flametongueWeapon.Launch();
        }

        if (!_waterShield.HaveBuff &&
            _waterShield.KnownSpell &&
            _waterShield.IsSpellUsable)
        {
            _waterShield.Launch();
        }
    }

    private void CheckHeal()
    {
        if (ObjectManager.Me.HealthPercent <= 50)
        {
            if (_healingSurge.KnownSpell &&
                _healingSurge.IsSpellUsable)
            {
                _healingSurge.Launch();
                Thread.Sleep(700);
            }
            else if (_healingWave.KnownSpell &&
                     _healingWave.IsSpellUsable)
            {
                _healingWave.Launch();
                Thread.Sleep(700);
            }
        }
    }
}

public class Priest
{
    #region InitializeSpell

    private readonly Spell _devouringPlague = new Spell("Devouring Plague");
    private readonly Spell _divineSpirit = new Spell("Divine Spirit");
    private readonly Spell _flashHeal = new Spell("Flash Heal");
    private readonly Spell _greaterHeal = new Spell("Greater Heal");
    private readonly Spell _heal = new Spell("Heal");
    private readonly Spell _holyFire = new Spell("Holy Fire");
    private readonly Spell _innerFire = new Spell("Inner Fire");
    private readonly Spell _lesserHeal = new Spell("Lesser Heal");
    private readonly Spell _mindBlast = new Spell("Mind Blast");
    private readonly Spell _mindFlay = new Spell("Mind Flay");
    private readonly Spell _powerWordFortitude = new Spell("Power Word: Fortitude");
    private readonly Spell _powerWordShield = new Spell("Power Word: Shield");
    private readonly Spell _shadowProtection = new Spell("Shadow Protection");
    private readonly Spell _shadowWordPain = new Spell("Shadow Word: Pain");
    private readonly Spell _shadowfiend = new Spell("Shadowfiend");
    private readonly Spell _shadowform = new Spell("Shadowform");
    private readonly Spell _smite = new Spell("Smite");
    private readonly Spell _vampiricEmbrace = new Spell("Vampiric Embrace");
    private readonly Spell _vampiricTouch = new Spell("Vampiric Touch");

    #endregion InitializeSpell

    public Priest()
    {
        Main.range = 23.0f; // Range

        UInt64 lastTarget = 0;
        bool postCombatUsed = true;

        while (Main.loop)
        {
            try
            {
                if (!ObjectManager.Me.IsMounted)
                {
                    Patrolling();

                    if (!Fight.InFight && !postCombatUsed)
                    {
                        postCombatUsed = true;
                        PostCombat();
                    }


                    if (Fight.InFight && ObjectManager.Me.Target > 0)
                    {
                        if (ObjectManager.Me.Target != lastTarget && ObjectManager.Target.GetDistance <= Main.range)
                        {
                            Pull();
                            lastTarget = ObjectManager.Me.Target;
                        }

                        Combat();
                        postCombatUsed = false;
                    }
                }
            }
            catch
            {
            }
            Thread.Sleep(500);
        }
    }

    private void Pull()
    {
        BuffShadowform();
        BuffPwShield();

        if (_vampiricTouch.KnownSpell &&
            _vampiricTouch.IsSpellUsable &&
            _vampiricTouch.IsDistanceGood)
        {
            _vampiricTouch.Launch();
        }
        else if (_holyFire.KnownSpell &&
                 _holyFire.IsSpellUsable &&
                 _holyFire.IsDistanceGood)
        {
            _holyFire.Launch();
        }
        else
        {
            if (_smite.KnownSpell &&
                _smite.IsSpellUsable &&
                _smite.IsDistanceGood)
            {
                _smite.Launch();
            }
        }


        if (_mindBlast.KnownSpell &&
            _mindBlast.IsSpellUsable)
        {
            _mindBlast.Launch();
        }
        else
        {
            if (_smite.KnownSpell &&
                _smite.IsSpellUsable &&
                _smite.IsDistanceGood)
            {
                _smite.Launch();
            }
        }
    }

    private void Combat()
    {
        BuffPwShield();
        BuffInnerFire();
        BuffVampiricEmbrace();

        if (!_shadowWordPain.TargetHaveBuff &&
            _shadowWordPain.KnownSpell &&
            _shadowWordPain.IsSpellUsable &&
            ObjectManager.Target.HealthPercent > 10 &&
            _shadowWordPain.IsDistanceGood)
        {
            _shadowWordPain.Launch();
            HealCheck();
            Thread.Sleep(1000);
        }

        if (!_devouringPlague.TargetHaveBuff &&
            _devouringPlague.KnownSpell &&
            _devouringPlague.IsSpellUsable &&
            ObjectManager.Target.HealthPercent > 10 &&
            _devouringPlague.IsDistanceGood)
        {
            _devouringPlague.Launch();
            HealCheck();
        }

        if (_shadowfiend.KnownSpell &&
            ObjectManager.Target.HealthPercent > 10 &&
            ObjectManager.Me.BarTwoPercentage < 40 &&
            _shadowfiend.IsSpellUsable &&
            _shadowfiend.IsDistanceGood)
        {
            _shadowfiend.Launch();
            HealCheck();
        }

        if (_mindBlast.KnownSpell &&
            _mindBlast.IsSpellUsable &&
            _mindBlast.IsDistanceGood)
        {
            _mindBlast.Launch();
        }
        else if (_mindFlay.KnownSpell &&
                 _mindFlay.IsSpellUsable &&
                 _mindFlay.IsDistanceGood)
        {
            _mindFlay.Launch();
        }

        if (!_mindFlay.KnownSpell && !_mindFlay.IsSpellUsable)
        {
            if (_smite.KnownSpell &&
                _smite.IsSpellUsable &&
                _smite.IsDistanceGood)
            {
                _smite.Launch();
            }
        }

        HealCheck();
    }

    private void PostCombat()
    {
    }

    private void Patrolling()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        BuffShadowform();
        BuffInnerFire();
        BuffPwFortitude();
        BuffVampiricEmbrace();
        BuffDivineSpirit();
        BuffShadowProtection();
    }

    private void BuffShadowform()
    {
        if (ObjectManager.Me.IsMounted)
            return;
        if (!_shadowform.HaveBuff &&
            _shadowform.KnownSpell &&
            _shadowform.IsSpellUsable)
        {
            _shadowform.Launch();
        }
    }

    private void BuffInnerFire()
    {
        if (ObjectManager.Me.IsMounted)
            return;
        if (!_innerFire.HaveBuff &&
            _innerFire.KnownSpell &&
            _innerFire.IsSpellUsable)
        {
            _innerFire.Launch();
        }
    }

    private void BuffPwFortitude()
    {
        if (ObjectManager.Me.IsMounted)
            return;
        if (!_powerWordFortitude.HaveBuff &&
            _powerWordFortitude.KnownSpell &&
            _powerWordFortitude.IsSpellUsable)
        {
            _powerWordFortitude.Launch();
        }
    }

    private void BuffVampiricEmbrace()
    {
        if (ObjectManager.Me.IsMounted)
            return;
        if (!_vampiricEmbrace.HaveBuff &&
            _vampiricEmbrace.KnownSpell &&
            _vampiricEmbrace.IsSpellUsable)
        {
            _vampiricEmbrace.Launch();
        }
    }

    private void BuffDivineSpirit()
    {
        if (ObjectManager.Me.IsMounted)
            return;
        if (!_divineSpirit.HaveBuff &&
            _divineSpirit.KnownSpell &&
            _divineSpirit.IsSpellUsable)
        {
            _divineSpirit.Launch();
        }
    }

    private void BuffShadowProtection()
    {
        if (ObjectManager.Me.IsMounted)
            return;
        if (!_shadowProtection.HaveBuff &&
            _shadowProtection.KnownSpell &&
            _shadowProtection.IsSpellUsable)
        {
            _shadowProtection.Launch();
        }
    }

    private void BuffPwShield()
    {
        if (ObjectManager.Me.IsMounted)
            return;
        if (!_powerWordShield.HaveBuff &&
            _powerWordShield.KnownSpell &&
            _powerWordShield.IsSpellUsable)
        {
            _powerWordShield.Launch();
            Thread.Sleep(1000);
        }
    }

    private void HealCheck()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (ObjectManager.Me.HealthPercent > 50 &&
            ObjectManager.Me.HealthPercent < 70)
        {
            if (_flashHeal.KnownSpell)
            {
                _flashHeal.Launch();
            }
            else
            {
                _lesserHeal.Launch();
            }

            Thread.Sleep(1500);
        }
        else if (ObjectManager.Me.HealthPercent > 0 &&
                 ObjectManager.Me.HealthPercent <= 50)
        {
            if (_greaterHeal.KnownSpell && _greaterHeal.IsSpellUsable)
            {
                _greaterHeal.Launch();
            }
            else if (_heal.KnownSpell && _heal.IsSpellUsable)
            {
                _heal.Launch();
            }
            else if (_lesserHeal.IsSpellUsable)
            {
                _lesserHeal.Launch();
            }

            Thread.Sleep(1500);
        }
    }
}

public class Hunter
{
    public static Spell CallPet;

    #region Nested type: HunterDefault

    public class HunterDefault
    {
        #region InitializeSpell

        private readonly Spell _arcaneShot = new Spell("Arcane Shot");
        private readonly Spell _aspectOfTheFox = new Spell("Aspect of the Fox");
        private readonly Spell _aspectOfTheHawk = new Spell("Aspect of the Hawk");
        private readonly Spell _feignDeath = new Spell("Feign Death");
        private readonly Spell _freezingTrap = new Spell("Freezing Trap");
        private readonly Spell _hunterSMark = new Spell("Hunter's Mark");
        private readonly Spell _killCommand = new Spell("Kill Command");
        private readonly Spell _mendPet = new Spell("Mend Pet");
        private readonly Spell _raptorStrike = new Spell("Raptor Strike");
        private readonly Spell _revivePet = new Spell("Revive Pet");
        private readonly Spell _serpentSting = new Spell("Serpent Sting");
        private readonly Spell _steadyShot = new Spell("Steady Shot");

        #endregion InitializeSpell

        private Int32 _lastHealTick;

        public HunterDefault()
        {
            Main.range = 23.0f; // Range

            UInt64 lastTarget = 0;
            bool postCombatUsed = true;
            bool haveBeenPooled = false;

            while (Main.loop)
            {
                try
                {
                    if (!ObjectManager.Me.IsMounted)
                    {
                        Patrolling();
                        if (!Fight.InFight)
                            haveBeenPooled = false;

                        if (!Fight.InFight && !postCombatUsed)
                        {
                            postCombatUsed = true;
                            PostCombat();
                        }


                        if (Fight.InFight && ObjectManager.Me.Target > 0)
                        {
                            if (ObjectManager.Me.Target != lastTarget && ObjectManager.Target.GetDistance <= Main.range)
                            {
                                Pull();
                                lastTarget = ObjectManager.Me.Target;
                                haveBeenPooled = true;
                            }
                            if (haveBeenPooled)
                                Combat();
                            postCombatUsed = false;
                        }
                    }
                }
                catch
                {
                }
                Thread.Sleep(500);
            }
        }

        public void Pull()
        {
            SummonPet();

            if (ObjectManager.Pet.IsAlive)
            {
                Lua.LuaDoString("PetDefensiveMode();");
            }

            if (ObjectManager.Pet.IsAlive)
            {
                Lua.RunMacroText("/petattack");
                Logging.WriteFight("Launch Pet Attack");
            }

            if (!_hunterSMark.HaveBuff &&
                _hunterSMark.KnownSpell)
            {
                _hunterSMark.Launch();
            }
            Lua.RunMacroText("/petattack");

            _serpentSting.Launch();
            if (
                _serpentSting.KnownSpell &&
                _serpentSting.IsSpellUsable &&
                _serpentSting.IsDistanceGood)
            {
                _serpentSting.Launch();
            }
        }

        private void Combat()
        {
            SummonPet();
            HealPet();
            AvoidMelee();

            if (_arcaneShot.KnownSpell &&
                _arcaneShot.IsSpellUsable &&
                _arcaneShot.IsDistanceGood)
            {
                _arcaneShot.Launch();
                HealPet();
                AvoidMelee();
                Life();
            }

            if (_killCommand.KnownSpell &&
                _killCommand.IsSpellUsable &&
                _killCommand.IsDistanceGood)
            {
                _killCommand.Launch();
                HealPet();
                AvoidMelee();
                Life();
            }

            if (_raptorStrike.KnownSpell &&
                _raptorStrike.IsSpellUsable &&
                _raptorStrike.IsDistanceGood)
            {
                _raptorStrike.Launch();
                HealPet();
                AvoidMelee();
                Life();
            }

            if (_steadyShot.KnownSpell &&
                _steadyShot.IsSpellUsable &&
                _steadyShot.IsDistanceGood)
            {
                _steadyShot.Launch();
                HealPet();
                AvoidMelee();
                Life();
            }
        }

        private void PostCombat()
        {
        }

        private void Patrolling()
        {
            if (!ObjectManager.Me.IsMounted)
            {
                CheckAura();
            }
        }

        private void SummonPet()
        {
            if (!ObjectManager.Me.IsCast)
            {
                if (CallPet.KnownSpell &&
                    _revivePet.KnownSpell &&
                    !ObjectManager.Me.IsMounted)
                {
                    if (!ObjectManager.Pet.IsAlive ||
                        ObjectManager.Pet.Guid == 0)
                    {
                        Thread.Sleep(1000);
                        if (!ObjectManager.Me.IsCast && !ObjectManager.Me.IsMounted)
                        {
                            CallPet.Launch();
                            Thread.Sleep(1000);

                            if (!ObjectManager.Pet.IsAlive)
                            {
                                _revivePet.Launch();
                                Thread.Sleep(1000);
                            }
                        }
                    }
                }
            }
        }

        private void CheckAura()
        {
            if (_aspectOfTheFox.KnownSpell &&
                _aspectOfTheHawk.KnownSpell)
            {
                if (ObjectManager.Me.BarTwoPercentage >= 80 && !_aspectOfTheHawk.HaveBuff)
                {
                    _aspectOfTheHawk.Launch();
                }
                else if (ObjectManager.Me.BarTwoPercentage <= 25 && !_aspectOfTheFox.HaveBuff)
                {
                    _aspectOfTheFox.Launch();
                }
                else if (!_aspectOfTheFox.HaveBuff &&
                         !_aspectOfTheHawk.HaveBuff)
                {
                    _aspectOfTheHawk.Launch();
                }
            }
        }

        private void HealPet()
        {
            if (ObjectManager.Pet.Health > 0 &&
                ObjectManager.Pet.HealthPercent < 50 &&
                (Environment.TickCount - _lastHealTick) > (15 * 1000))
            {
                _lastHealTick = Environment.TickCount;
                _mendPet.Launch();
            }
        }

        private void AvoidMelee()
        {
            if (ObjectManager.Target.GetDistance < 5 &&
                ObjectManager.Pet.IsAlive &&
                ObjectManager.Pet.Guid != 0 &&
                ObjectManager.Target.Target == ObjectManager.Pet.Guid)
            {
                nManager.Wow.Helpers.Keybindings.DownKeybindings(Keybindings.MOVEBACKWARD);
                Thread.Sleep(1500);
                nManager.Wow.Helpers.Keybindings.UpKeybindings(Keybindings.MOVEBACKWARD);
            }
        }

        private void Life()
        {
            if (ObjectManager.Me.Health > 0 &&
                ObjectManager.Me.HealthPercent < 40 &&
                _freezingTrap.KnownSpell &&
                _freezingTrap.IsSpellUsable &&
                _feignDeath.KnownSpell &&
                _feignDeath.IsSpellUsable)
            {
                _freezingTrap.Launch();
                _feignDeath.Launch();
            }
        }
    }

    #endregion

    #region Nested type: HunterMark

    public class HunterMark
    {
        #region InitializeSpell

        private readonly Spell _aimedShot = new Spell("Aimed Shot");
        private readonly Spell _arcaneShot = new Spell("Arcane Shot");
        private readonly Spell _aspectOfTheFox = new Spell("Aspect of the Fox");
        private readonly Spell _aspectOfTheHawk = new Spell("Aspect of the Hawk");
        private readonly Spell _cobraShot = new Spell("Cobra Shot");
        private readonly Spell _feignDeath = new Spell("Feign Death");
        private readonly Spell _freezingTrap = new Spell("Freezing Trap");
        private readonly Spell _hunterSMark = new Spell("Hunter's Mark");
        private readonly Spell _killCommand = new Spell("Kill Command");
        private readonly Spell _mendPet = new Spell("Mend Pet");
        private readonly Spell _raptorStrike = new Spell("Raptor Strike");
        private readonly Spell _revivePet = new Spell("Revive Pet");
        private readonly Spell _serpentSting = new Spell("Serpent Sting");
        private readonly Spell _steadyShot = new Spell("Steady Shot");

        #endregion InitializeSpell

        private Int32 _lastHealTick;

        public HunterMark()
        {
            Main.range = 23.0f; // Range

            UInt64 lastTarget = 0;
            bool postCombatUsed = true;
            bool haveBeenPooled = false;

            while (Main.loop)
            {
                try
                {
                    if (!ObjectManager.Me.IsMounted)
                    {
                        Patrolling();
                        if (!Fight.InFight)
                            haveBeenPooled = false;

                        if (!Fight.InFight && !postCombatUsed)
                        {
                            postCombatUsed = true;
                            PostCombat();
                        }


                        if (Fight.InFight && ObjectManager.Me.Target > 0)
                        {
                            if (ObjectManager.Me.Target != lastTarget && ObjectManager.Target.GetDistance <= Main.range)
                            {
                                Pull();
                                lastTarget = ObjectManager.Me.Target;
                                haveBeenPooled = true;
                            }
                            if (haveBeenPooled)
                                Combat();
                            postCombatUsed = false;
                        }
                    }
                }
                catch
                {
                }
                Thread.Sleep(500);
            }
        }

        private void Pull()
        {
            SummonPet();

            if (ObjectManager.Pet.IsAlive)
            {
                Lua.LuaDoString("PetDefensiveMode();");
            }

            if (ObjectManager.Pet.IsAlive)
            {
                Lua.RunMacroText("/petattack");
                Logging.WriteFight("Launch Pet Attack");
            }

            if (!_hunterSMark.HaveBuff &&
                _hunterSMark.KnownSpell)
            {
                _hunterSMark.Launch();
            }
            Lua.RunMacroText("/petattack");

            _serpentSting.Launch();
            if (
                _serpentSting.KnownSpell &&
                _serpentSting.IsSpellUsable &&
                _serpentSting.IsDistanceGood)
            {
                _serpentSting.Launch();
            }
        }

        private void Combat()
        {
            SummonPet();
            HealPet();
            AvoidMelee();

            if (_arcaneShot.KnownSpell &&
                _arcaneShot.IsSpellUsable &&
                _arcaneShot.IsDistanceGood)
            {
                _arcaneShot.Launch();
                HealPet();
                AvoidMelee();
                Life();
            }

            if (_killCommand.KnownSpell &&
                _killCommand.IsSpellUsable &&
                _killCommand.IsDistanceGood)
            {
                _killCommand.Launch();
                HealPet();
                AvoidMelee();
                Life();
            }

            if (_raptorStrike.KnownSpell &&
                _raptorStrike.IsSpellUsable &&
                _raptorStrike.IsDistanceGood)
            {
                _raptorStrike.Launch();
                HealPet();
                AvoidMelee();
                Life();
            }

            if (_aimedShot.KnownSpell &&
                _aimedShot.IsSpellUsable &&
                _aimedShot.IsDistanceGood)
            {
                _aimedShot.Launch();
                HealPet();
                AvoidMelee();
                Life();
            }

            if (_cobraShot.KnownSpell &&
                _cobraShot.IsSpellUsable &&
                _cobraShot.IsDistanceGood)
            {
                _cobraShot.Launch();
                HealPet();
                AvoidMelee();
                Life();
            }

            else if (_steadyShot.KnownSpell &&
                     _steadyShot.IsSpellUsable &&
                     _steadyShot.IsDistanceGood)
            {
                _steadyShot.Launch();
                HealPet();
                AvoidMelee();
                Life();
            }
        }

        private void PostCombat()
        {
        }

        private void Patrolling()
        {
            if (!ObjectManager.Me.IsMounted)
            {
                CheckAura();
            }
        }

        private void SummonPet()
        {
            if (!ObjectManager.Me.IsCast)
            {
                if (CallPet.KnownSpell &&
                    _revivePet.KnownSpell &&
                    !ObjectManager.Me.IsMounted)
                {
                    if (!ObjectManager.Pet.IsAlive ||
                        ObjectManager.Pet.Guid == 0)
                    {
                        Thread.Sleep(1000);
                        if (!ObjectManager.Me.IsCast && !ObjectManager.Me.IsMounted)
                        {
                            CallPet.Launch();
                            Thread.Sleep(1000);

                            if (!ObjectManager.Pet.IsAlive)
                            {
                                _revivePet.Launch();
                                Thread.Sleep(1000);
                            }
                        }
                    }
                }
            }
        }

        private void CheckAura()
        {
            if (_aspectOfTheFox.KnownSpell &&
                _aspectOfTheHawk.KnownSpell)
            {
                if (ObjectManager.Me.BarTwoPercentage >= 80 && !_aspectOfTheHawk.HaveBuff)
                {
                    _aspectOfTheHawk.Launch();
                }
                else if (ObjectManager.Me.BarTwoPercentage <= 25 && !_aspectOfTheFox.HaveBuff)
                {
                    _aspectOfTheFox.Launch();
                }
                else if (!_aspectOfTheFox.HaveBuff &&
                         !_aspectOfTheHawk.HaveBuff)
                {
                    _aspectOfTheHawk.Launch();
                }
            }
        }

        private void HealPet()
        {
            if (ObjectManager.Pet.Health > 0 &&
                ObjectManager.Pet.HealthPercent < 50 &&
                (Environment.TickCount - _lastHealTick) > (15 * 1000))
            {
                _lastHealTick = Environment.TickCount;
                _mendPet.Launch();
            }
        }

        private void AvoidMelee()
        {
            if (ObjectManager.Target.GetDistance < 5 &&
                ObjectManager.Pet.IsAlive &&
                ObjectManager.Pet.Guid != 0 &&
                ObjectManager.Target.Target == ObjectManager.Pet.Guid)
            {
                nManager.Wow.Helpers.Keybindings.DownKeybindings(Keybindings.MOVEBACKWARD);
                Thread.Sleep(1500);
                nManager.Wow.Helpers.Keybindings.UpKeybindings(Keybindings.MOVEBACKWARD);
            }
        }

        private void Life()
        {
            if (ObjectManager.Me.Health > 0 &&
                ObjectManager.Me.HealthPercent < 40 &&
                _freezingTrap.KnownSpell &&
                _freezingTrap.IsSpellUsable &&
                _feignDeath.KnownSpell &&
                _feignDeath.IsSpellUsable)
            {
                _freezingTrap.Launch();
                _feignDeath.Launch();
            }
        }
    }

    #endregion

    #region Nested type: HunterSurv

    public class HunterSurv
    {
        #region InitializeSpell

        private readonly Spell _arcaneShot = new Spell("Arcane Shot");
        private readonly Spell _aspectOfTheFox = new Spell("Aspect of the Fox");
        private readonly Spell _aspectOfTheHawk = new Spell("Aspect of the Hawk");
        private readonly Spell _cobraShot = new Spell("Cobra Shot");
        private readonly Spell _explosiveShot = new Spell("Explosive Shot");
        private readonly Spell _feignDeath = new Spell("Feign Death");
        private readonly Spell _freezingTrap = new Spell("Freezing Trap");
        private readonly Spell _hunterSMark = new Spell("Hunter's Mark");
        private readonly Spell _killCommand = new Spell("Kill Command");
        private readonly Spell _mendPet = new Spell("Mend Pet");
        private readonly Spell _raptorStrike = new Spell("Raptor Strike");
        private readonly Spell _revivePet = new Spell("Revive Pet");
        private readonly Spell _serpentSting = new Spell("Serpent Sting");
        private readonly Spell _steadyShot = new Spell("Steady Shot");

        #endregion InitializeSpell

        private Int32 _lastHealTick;

        public HunterSurv()
        {
            Main.range = 23.0f; // Range

            UInt64 lastTarget = 0;
            bool postCombatUsed = true;
            bool HaveBeenPooled = false;

            while (Main.loop)
            {
                try
                {
                    if (!ObjectManager.Me.IsMounted)
                    {
                        Patrolling();
                        if (!Fight.InFight)
                            HaveBeenPooled = false;

                        if (!Fight.InFight && !postCombatUsed)
                        {
                            postCombatUsed = true;
                            PostCombat();
                        }


                        if (Fight.InFight && ObjectManager.Me.Target > 0)
                        {
                            if (ObjectManager.Me.Target != lastTarget && ObjectManager.Target.GetDistance <= Main.range)
                            {
                                Pull();
                                lastTarget = ObjectManager.Me.Target;
                                HaveBeenPooled = true;
                            }
                            if (HaveBeenPooled)
                                Combat();
                            postCombatUsed = false;
                        }
                    }
                }
                catch
                {
                }
                Thread.Sleep(500);
            }
        }

        public void Pull()
        {
            SummonPet();

            if (ObjectManager.Pet.IsAlive)
            {
                Lua.LuaDoString("PetDefensiveMode();");
            }

            if (ObjectManager.Pet.IsAlive)
            {
                Lua.RunMacroText("/petattack");
                Logging.WriteFight("Launch Pet Attack");
            }

            if (!_hunterSMark.HaveBuff &&
                _hunterSMark.KnownSpell)
            {
                _hunterSMark.Launch();
            }
            Lua.RunMacroText("/petattack");

            _serpentSting.Launch();
            if (
                _serpentSting.KnownSpell &&
                _serpentSting.IsSpellUsable &&
                _serpentSting.IsDistanceGood)
            {
                _serpentSting.Launch();
            }
        }

        private void Combat()
        {
            SummonPet();
            HealPet();
            AvoidMelee();

            if (_arcaneShot.KnownSpell &&
                _arcaneShot.IsSpellUsable &&
                _arcaneShot.IsDistanceGood)
            {
                _arcaneShot.Launch();
                HealPet();
                AvoidMelee();
                Life();
            }

            if (_raptorStrike.KnownSpell &&
                _raptorStrike.IsSpellUsable &&
                _raptorStrike.IsDistanceGood)
            {
                _raptorStrike.Launch();
                HealPet();
                AvoidMelee();
                Life();
            }

            if (_killCommand.KnownSpell &&
                _killCommand.IsSpellUsable &&
                _killCommand.IsDistanceGood)
            {
                _killCommand.Launch();
                HealPet();
                AvoidMelee();
                Life();
            }

            if (_explosiveShot.KnownSpell &&
                _explosiveShot.IsSpellUsable &&
                _explosiveShot.IsDistanceGood)
            {
                _explosiveShot.Launch();
                HealPet();
                AvoidMelee();
                Life();
            }

            if (_cobraShot.KnownSpell &&
                _cobraShot.IsSpellUsable &&
                _cobraShot.IsDistanceGood)
            {
                _cobraShot.Launch();
                HealPet();
                AvoidMelee();
                Life();
            }

            else if (_steadyShot.KnownSpell &&
                     _steadyShot.IsSpellUsable &&
                     _steadyShot.IsDistanceGood)
            {
                _steadyShot.Launch();
                HealPet();
                AvoidMelee();
                Life();
            }
        }

        private void PostCombat()
        {
        }

        private void Patrolling()
        {
            if (!ObjectManager.Me.IsMounted)
            {
                CheckAura();
            }
        }

        private void SummonPet()
        {
            if (!ObjectManager.Me.IsCast)
            {
                if (CallPet.KnownSpell &&
                    _revivePet.KnownSpell &&
                    !ObjectManager.Me.IsMounted)
                {
                    if (!ObjectManager.Pet.IsAlive ||
                        ObjectManager.Pet.Guid == 0)
                    {
                        Thread.Sleep(1000);
                        if (!ObjectManager.Me.IsCast && !ObjectManager.Me.IsMounted)
                        {
                            CallPet.Launch();
                            Thread.Sleep(1000);

                            if (!ObjectManager.Pet.IsAlive)
                            {
                                _revivePet.Launch();
                                Thread.Sleep(1000);
                            }
                        }
                    }
                }
            }
        }

        private void CheckAura()
        {
            if (_aspectOfTheFox.KnownSpell &&
                _aspectOfTheHawk.KnownSpell)
            {
                if (ObjectManager.Me.BarTwoPercentage >= 80 && !_aspectOfTheHawk.HaveBuff)
                {
                    _aspectOfTheHawk.Launch();
                }
                else if (ObjectManager.Me.BarTwoPercentage <= 25 && !_aspectOfTheFox.HaveBuff)
                {
                    _aspectOfTheFox.Launch();
                }
                else if (!_aspectOfTheFox.HaveBuff &&
                         !_aspectOfTheHawk.HaveBuff)
                {
                    _aspectOfTheHawk.Launch();
                }
            }
        }

        private void HealPet()
        {
            if (ObjectManager.Pet.Health > 0 &&
                ObjectManager.Pet.HealthPercent < 50 &&
                (Environment.TickCount - _lastHealTick) > (15 * 1000))
            {
                _lastHealTick = Environment.TickCount;
                _mendPet.Launch();
            }
        }

        private void AvoidMelee()
        {
            if (ObjectManager.Target.GetDistance < 5 &&
                ObjectManager.Pet.IsAlive &&
                ObjectManager.Pet.Guid != 0 &&
                ObjectManager.Target.Target == ObjectManager.Pet.Guid)
            {
                nManager.Wow.Helpers.Keybindings.DownKeybindings(Keybindings.MOVEBACKWARD);
                Thread.Sleep(1500);
                nManager.Wow.Helpers.Keybindings.UpKeybindings(Keybindings.MOVEBACKWARD);
            }
        }

        private void Life()
        {
            if (ObjectManager.Me.Health > 0 &&
                ObjectManager.Me.HealthPercent < 40 &&
                _freezingTrap.KnownSpell &&
                _freezingTrap.IsSpellUsable &&
                _feignDeath.KnownSpell &&
                _feignDeath.IsSpellUsable)
            {
                _freezingTrap.Launch();
                _feignDeath.Launch();
            }
        }
    }

    #endregion
}

public class Rogue
{
    #region InitializeSpell

    private readonly Spell _adrenalineRush = new Spell("Adrenaline Rush");
    private readonly Spell _bladeFlurry = new Spell("Blade Flurry");
    private readonly Spell _cheapShot = new Spell("Cheap Shot");
    private readonly Spell _evasion = new Spell("Evasion");
    private readonly Spell _eviscerate = new Spell("Eviscerate");
    private readonly Spell _pickPocket = new Spell("Pick Pocket");
    private readonly Spell _premeditation = new Spell("Premeditation");
    private readonly Spell _shadowstep = new Spell("Shadowstep");
    private readonly Spell _sinisterStrike = new Spell("Sinister Strike");
    private readonly Spell _stealth = new Spell("Stealth");

    #endregion InitializeSpell

    public Rogue()
    {
        Main.range = 3.6f; // Range

        UInt64 lastTarget = 0;
        bool postCombatUsed = true;

        while (Main.loop)
        {
            try
            {
                if (!ObjectManager.Me.IsMounted)
                {
                    Patrolling();

                    if (!Fight.InFight && !postCombatUsed)
                    {
                        postCombatUsed = true;
                        PostCombat();
                    }


                    if (Fight.InFight && ObjectManager.Me.Target > 0)
                    {
                        if (ObjectManager.Me.Target != lastTarget && _premeditation.IsDistanceGood)
                        {
                            Pull();
                            lastTarget = ObjectManager.Me.Target;
                        }

                        Combat();
                        postCombatUsed = false;
                    }
                }
            }
            catch
            {
            }
            Thread.Sleep(500);
        }
    }

    private void Pull()
    {
        if (_stealth.KnownSpell &&
            !_stealth.HaveBuff)
        {
            if (ObjectManager.Target.Target != ObjectManager.Me.Guid &&
                _stealth.IsSpellUsable)
            {
                // RANGE = 18
                _stealth.Launch();

                if (_premeditation.KnownSpell &&
                    _premeditation.IsSpellUsable &&
                    _premeditation.IsDistanceGood)
                {
                    _premeditation.Launch();
                }

                if (_shadowstep.KnownSpell &&
                    _shadowstep.IsSpellUsable &&
                    _shadowstep.IsDistanceGood)
                {
                    _shadowstep.Launch();
                }

                //RANGE = 3

                if (_pickPocket.KnownSpell &&
                    _pickPocket.IsSpellUsable &&
                    ObjectManager.Target.Target == 0 &&
                    ObjectManager.Target.CreatureTypeTarget == "Humanoid")
                {
                    // RANGE = 2
                    _pickPocket.Launch();
                }

                if (_cheapShot.KnownSpell &&
                    _cheapShot.IsSpellUsable &&
                    _cheapShot.IsDistanceGood)
                {
                    _cheapShot.Launch();
                }
                else
                {
                    _sinisterStrike.Launch();
                }
            }
        }
    }

    private void Combat()
    {
        if (_sinisterStrike.KnownSpell && _sinisterStrike.IsSpellUsable && _sinisterStrike.IsDistanceGood)
            _sinisterStrike.Launch();

        if (ObjectManager.Target.HealthPercent < 50 &&
            ObjectManager.Me.ComboPoint >= 3 &&
            _eviscerate.IsSpellUsable &&
            _eviscerate.IsDistanceGood)
            _eviscerate.Launch();

        if (_evasion.KnownSpell &&
            _evasion.IsSpellUsable &&
            ObjectManager.Me.HealthPercent < 60)
            _evasion.Launch();

        if (_bladeFlurry.KnownSpell &&
            _bladeFlurry.IsSpellUsable &&
            ObjectManager.Target.HealthPercent > 50)
            _bladeFlurry.Launch();

        if (_adrenalineRush.KnownSpell &&
            _adrenalineRush.IsSpellUsable &&
            ObjectManager.Me.BarTwoPercentage < 50)
            _adrenalineRush.Launch();
    }

    private void PostCombat()
    {
    }

    private void Patrolling()
    {
    }
}

public class DruidBalance
{
    #region InitializeSpell

    private readonly Spell _entanglingRoots = new Spell("Entangling Roots");
    private readonly Spell _faerieFire = new Spell("Faerie Fire");
    private readonly Spell _healingTouch = new Spell("Healing Touch");
    private readonly Spell _innervate = new Spell("Innervate");
    private readonly Spell _insectSwarm = new Spell("Insect Swarm");
    private readonly Spell _markOfTheWild = new Spell("Mark of the Wild");
    private readonly Spell _moonfire = new Spell("Moonfire");
    private readonly Spell _regrowth = new Spell("Regrowth");
    private readonly Spell _starfire = new Spell("Starfire");
    private readonly Spell _thorns = new Spell("Thorns");
    private readonly Spell _wrath = new Spell("Wrath");

    #endregion InitializeSpell

    public DruidBalance()
    {
        {
            Main.range = 26.0f; // Range

            UInt64 lastTarget = 0;
            bool postCombatUsed = true;

            while (Main.loop)
            {
                try
                {
                    if (!ObjectManager.Me.IsMounted)
                    {
                        Patrolling();

                        if (!Fight.InFight && !postCombatUsed)
                        {
                            postCombatUsed = true;
                            PostCombat();
                        }


                        if (Fight.InFight && ObjectManager.Me.Target > 0)
                        {
                            if (ObjectManager.Me.Target != lastTarget && ObjectManager.Target.GetDistance <= Main.range)
                            {
                                Pull();
                                lastTarget = ObjectManager.Me.Target;
                            }

                            Combat();
                            postCombatUsed = false;
                        }
                    }
                }
                catch
                {
                }
                Thread.Sleep(500);
            }
        }
    }

    private void Pull()
    {
        BuffMarkoftheWild();
        BuffThorns();

        if (_starfire.KnownSpell &&
            _starfire.IsSpellUsable &&
            _starfire.IsDistanceGood)
        {
            _starfire.Launch();
        }
        else if (_wrath.KnownSpell &&
                 _wrath.IsSpellUsable &&
                 _wrath.IsDistanceGood)
        {
            _wrath.Launch();
        }

        if (_entanglingRoots.KnownSpell &&
            _entanglingRoots.IsSpellUsable &&
            _entanglingRoots.IsDistanceGood)
        {
            _entanglingRoots.Launch();
        }

        if (_moonfire.KnownSpell &&
            _moonfire.IsSpellUsable &&
            _moonfire.IsDistanceGood)
        {
            _moonfire.Launch();
        }
    }

    private void Combat()
    {
        if (_faerieFire.IsSpellUsable &&
            _faerieFire.KnownSpell &&
            _faerieFire.IsDistanceGood &&
            !ObjectManager.Target.HaveBuff(91565))
        {
            _faerieFire.Launch();
            CheckHeal();
            CheckMana();
            CheckHeal2();
        }

        if (_insectSwarm.IsSpellUsable &&
            _insectSwarm.KnownSpell &&
            _insectSwarm.IsDistanceGood &&
            !ObjectManager.Target.HaveBuff(_insectSwarm.Id))
        {
            _insectSwarm.Launch();
            CheckHeal();
            CheckHeal2();
            CheckMana();
        }

        if (_wrath.KnownSpell &&
            _wrath.IsSpellUsable &&
            _wrath.IsDistanceGood)
        {
            _wrath.Launch();
            CheckHeal();
            CheckHeal2();
            CheckMana();
        }
    }

    private void PostCombat()
    {
    }

    private void Patrolling()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        BuffMarkoftheWild();
    }

    private void BuffMarkoftheWild()
    {
        if (!ObjectManager.Me.HaveBuff(79061) &&
            _markOfTheWild.KnownSpell &&
            _markOfTheWild.IsSpellUsable)
        {
            _markOfTheWild.Launch();
        }
    }

    private void BuffThorns()
    {
        if (!_thorns.HaveBuff &&
            _thorns.KnownSpell &&
            _thorns.IsSpellUsable)
        {
            _thorns.Launch();
        }
    }

    private void CheckHeal()
    {
        if (_healingTouch.KnownSpell &&
            _healingTouch.IsSpellUsable &&
            ObjectManager.Me.HealthPercent <= 50)
        {
            _healingTouch.Launch();
            CheckHeal();
            CheckHeal2();
            CheckMana();
            Thread.Sleep(1000);
        }
    }

    private void CheckMana()
    {
        if (_innervate.KnownSpell &&
            _innervate.IsSpellUsable &&
            ObjectManager.Me.BarTwoPercentage <= 40)
        {
            _innervate.Launch();
            CheckHeal();
            CheckHeal2();
            CheckMana();
        }
    }

    private void CheckHeal2()
    {
        if (_regrowth.KnownSpell &&
            _regrowth.IsSpellUsable &&
            ObjectManager.Me.HealthPercent <= 60)
        {
            _regrowth.Launch();
            CheckHeal();
            CheckHeal2();
            CheckMana();
        }
    }
}

public class DruidDeral
{
    #region InitializeSpell

    private readonly Spell _bash = new Spell("Bash");
    private readonly Spell _bearForm = new Spell("Bear Form");
    private readonly Spell _catForm = new Spell("Cat Form");
    private readonly Spell _claw = new Spell("Claw");
    private readonly Spell _direBearForm = new Spell("Dire Bear Form");
    private readonly Spell _faerieFireFeral = new Spell("Faerie Fire (Feral)");
    private readonly Spell _ferociousBite = new Spell("Ferocious Bite");
    private readonly Spell _healingTouch = new Spell("Healing Touch");
    private readonly Spell _markOfTheWild = new Spell("Mark of the Wild");
    private readonly Spell _maul = new Spell("Maul");
    private readonly Spell _rip = new Spell("Rip");
    private readonly Spell _thorns = new Spell("Thorns");

    #endregion InitializeSpell

    public DruidDeral()
    {
        Main.range = 3.6f; // Range

        UInt64 lastTarget = 0;
        bool postCombatUsed = true;

        while (Main.loop)
        {
            try
            {
                if (!ObjectManager.Me.IsMounted)
                {
                    Patrolling();

                    if (!Fight.InFight && !postCombatUsed)
                    {
                        postCombatUsed = true;
                        PostCombat();
                    }


                    if (Fight.InFight && ObjectManager.Me.Target > 0)
                    {
                        if (ObjectManager.Me.Target != lastTarget && ObjectManager.Target.GetDistance <= Main.range)
                        {
                            Pull();
                            lastTarget = ObjectManager.Me.Target;
                        }

                        Combat();
                        postCombatUsed = false;
                    }
                }
            }
            catch
            {
            }
            Thread.Sleep(500);
        }
    }

    private void Pull()
    {
        Buff_Thorns();
        BuffMarkoftheWild();

        CheckForm();

        if (_faerieFireFeral.KnownSpell &&
            _faerieFireFeral.IsSpellUsable &&
            _faerieFireFeral.IsDistanceGood &&
            !ObjectManager.Target.HaveBuff(91565))
        {
            _faerieFireFeral.Launch();
        }
    }

    private void Combat()
    {
        CheckForm();

        if (_faerieFireFeral.KnownSpell &&
            _faerieFireFeral.IsSpellUsable &&
            _faerieFireFeral.IsDistanceGood &&
            !ObjectManager.Target.HaveBuff(_faerieFireFeral.Id))
        {
            _faerieFireFeral.Launch();
            CheckHeal();
        }

        if (GuessForm() == "bear")
        {
            if (_maul.KnownSpell &&
                _maul.IsSpellUsable &&
                _maul.IsDistanceGood)
            {
                CheckForm();
                _maul.Launch();
                CheckHeal();
            }

            if (_bash.KnownSpell &&
                _bash.IsSpellUsable &&
                _bash.IsDistanceGood)
            {
                CheckForm();
                _bash.Launch();
                CheckHeal();
            }
        }

        if (GuessForm() == "cat")
        {
            if (ObjectManager.Me.ComboPoint < 3)
            {
                if (_claw.KnownSpell &&
                    _claw.IsSpellUsable &&
                    _claw.IsDistanceGood)
                {
                    CheckForm();
                    _claw.Launch();
                    CheckHeal();
                }
            }
            else if (ObjectManager.Me.ComboPoint >= 3)
            {
                if (_rip.KnownSpell &&
                    !_ferociousBite.KnownSpell &&
                    _rip.IsSpellUsable &&
                    _rip.IsDistanceGood)
                {
                    CheckForm();
                    _rip.Launch();
                    CheckHeal();
                }

                if (_ferociousBite.KnownSpell &&
                    _ferociousBite.IsSpellUsable &&
                    _ferociousBite.IsDistanceGood)
                {
                    CheckForm();
                    _ferociousBite.Launch();
                    CheckHeal();
                }
            }
        }
    }

    private void PostCombat()
    {
    }

    private void Patrolling()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        BuffMarkoftheWild();
    }

    private void BuffMarkoftheWild()
    {
        if (!ObjectManager.Me.HaveBuff(79061) &&
            _markOfTheWild.KnownSpell &&
            _markOfTheWild.IsSpellUsable)
        {
            _markOfTheWild.Launch();
        }
    }

    private void Buff_Thorns()
    {
        if (!_thorns.HaveBuff &&
            _thorns.KnownSpell &&
            _thorns.IsSpellUsable)
        {
            _thorns.Launch();
        }
    }

    private void CheckHeal()
    {
        if (_healingTouch.KnownSpell &&
            _healingTouch.IsSpellUsable &&
            ObjectManager.Me.HealthPercent <= 50)
        {
            _healingTouch.Launch();
            Thread.Sleep(1000);
        }
    }

    private String GuessForm()
    {
        if (_catForm.KnownSpell)
        {
            return "cat";
        }
        if (_bearForm.KnownSpell || _direBearForm.KnownSpell)
        {
            return "bear";
        }

        return String.Empty;
    }

    private void CheckForm()
    {
        if (!ObjectManager.Me.IsMounted && !ObjectManager.Me.IsDeadMe)
        {
            if (_catForm.KnownSpell)
            {
                if (!_catForm.HaveBuff)
                {
                    _catForm.Launch();
                }
            }
            else if (_bearForm.KnownSpell || _direBearForm.KnownSpell)
            {
                if (!_bearForm.HaveBuff &&
                    _direBearForm.KnownSpell)
                {
                    _direBearForm.Launch();
                }
                else if (!_bearForm.HaveBuff &&
                         _bearForm.KnownSpell)
                {
                    _bearForm.Launch();
                }
            }
        }
    }
}

#endregion CustomClass
