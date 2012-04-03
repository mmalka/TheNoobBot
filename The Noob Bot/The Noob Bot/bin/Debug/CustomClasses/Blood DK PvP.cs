using System;
using System.Threading;
using System.Windows.Forms;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;

public class Main : ICustomClass
{
    #region Initialize_CustomClass

    internal static float range = 3.5f;
    internal static bool loop = true;

    public float Range
    {
        get { return range; }
        set { range = value; }
    }

    public void ShowConfiguration()
    {
        MessageBox.Show("No setting for this custom class.");
    }

    public void Initialize()
    {
        new Deathknight();
        Logging.WriteFight("Death Knight Custom Class loaded successfully.");
    }

    #endregion Initialize_CustomClass

    #region Dispose_CustomClass

    public void Dispose()
    {
        Logging.WriteFight("Closing CC");
        loop = false;
    }

    #endregion Dispose_CustomClass
}

#region CustomClass

public class Deathknight
{
    #region InitializeSpell

    private readonly Spell _bloodPlague = new Spell("Blood Plague");
    private readonly Spell _deathGrip = new Spell("Death Grip");
    private readonly Spell _deathStrike = new Spell("Death Strike");
    private readonly Spell _frostFever = new Spell("Frost Fever");
    private readonly Spell _hornOfWinter = new Spell("Horn of Winter");
    private readonly Spell _boneShield = new Spell("Bone Shield");
    private readonly Spell _icyTouch = new Spell("Icy Touch");
    private readonly Spell _plagueStrike = new Spell("Plague Strike");
    private readonly Spell _runeStrike = new Spell("Rune Strike");
    private readonly Spell _bloodBoil = new Spell("Blood Boil");
    private readonly Spell _outbreak = new Spell("Outbreak");
    private readonly Spell _chainsOfIce = new Spell("Chains of Ice");
    private readonly Spell _vampiricBlood = new Spell("Vampiric Blood");
    private readonly Spell _runeTap = new Spell("Rune Tap");
    private readonly Spell _iceboundFortitude = new Spell("Icebound Fortitude");
    private readonly Spell _bloodPlauge = new Spell("Blood Plague");
    #endregion InitializeSpell

    public Deathknight()
    {
        Main.range = 3.6f;

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
        else if (_outbreak.KnownSpell &&
                 _outbreak.IsSpellUsable &&
                 _outbreak.IsDistanceGood)
        {
            _outbreak.Launch();
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

        if (!_frostFever.HaveBuff &&
            !_bloodPlauge.HaveBuff &&
            _outbreak.KnownSpell &&
            _outbreak.IsSpellUsable &&
            _outbreak.IsDistanceGood)
        {
            _outbreak.Launch();
            FastCheck();
        }

        if (!_frostFever.HaveBuff &&
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

        if (!_chainsOfIce.HaveBuff &&
            _chainsOfIce.KnownSpell &&
            _chainsOfIce.IsSpellUsable &&
            _chainsOfIce.IsDistanceGood)
        {
            _chainsOfIce.Launch();
            FastCheck();
        }

        if (_deathStrike.KnownSpell &&
            _deathStrike.IsSpellUsable &&
            _deathStrike.IsDistanceGood)
        {
            _deathStrike.Launch();
            FastCheck();
        }

        if (_bloodBoil.KnownSpell &&
            _bloodBoil.IsSpellUsable &&
            _bloodBoil.IsDistanceGood)
        {
            _bloodBoil.Launch();
            FastCheck();
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

        if (!_boneShield.HaveBuff &&
            _boneShield.KnownSpell &&
            _boneShield.IsSpellUsable)
        {
            _boneShield.Launch();
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

        int hp = ObjectManager.Me.Health / ObjectManager.Me.MaxHealth;

        if (hp < 0.41)
        {
            if (_vampiricBlood.IsSpellUsable)
            {
                _vampiricBlood.Launch();
            }

            if (_runeTap.IsSpellUsable)
            {
                _runeTap.Launch();
            }

            if (_iceboundFortitude.IsSpellUsable)
            {
                _iceboundFortitude.Launch();
            }
        }
    }
}

#endregion CustomClass