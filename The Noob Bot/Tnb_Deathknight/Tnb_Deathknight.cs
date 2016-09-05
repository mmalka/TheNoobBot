/*
* CombatClass for TheNoobBot
* Credit : Vesper, Neo2003, Dreadlocks
* Thanks you !
*/

using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using nManager.Helpful;
using nManager.Wow;
using nManager.Wow.Bot.Tasks;
using nManager.Wow.Class;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;
using Timer = nManager.Helpful.Timer;

// ReSharper disable EmptyGeneralCatchClause
// ReSharper disable ObjectCreationAsStatement

public class Main : ICombatClass
{
    internal static float InternalRange = 5.0f;
    internal static float InternalAggroRange = 5.0f;
    internal static bool InternalLoop = true;
    internal static Spell InternalLightHealingSpell;

    #region ICombatClass Members

    public float AggroRange
    {
        get { return InternalAggroRange; }
    }

    public Spell LightHealingSpell
    {
        get { return InternalLightHealingSpell; }
        set { InternalLightHealingSpell = value; }
    }

    public float Range
    {
        get { return InternalRange; }
        set { InternalRange = value; }
    }

    public void Initialize()
    {
        Initialize(false);
    }

    public void Dispose()
    {
        Logging.WriteFight("Combat system stopped.");
        InternalLoop = false;
    }

    public void ShowConfiguration()
    {
        Directory.CreateDirectory(Application.StartupPath + "\\CombatClasses\\Settings\\");
        Initialize(true);
    }

    public void ResetConfiguration()
    {
        Directory.CreateDirectory(Application.StartupPath + "\\CombatClasses\\Settings\\");
        Initialize(true, true);
    }

    #endregion

    public void Initialize(bool configOnly, bool resetSettings = false)
    {
        try
        {
            if (!InternalLoop)
                InternalLoop = true;
            Logging.WriteFight("Loading combat system.");
            WoWSpecialization wowSpecialization = ObjectManager.Me.WowSpecialization(true);
            switch (ObjectManager.Me.WowClass)
            {
                    #region DeathKnight Specialisation checking

                case WoWClass.DeathKnight:

                    if (wowSpecialization == WoWSpecialization.DeathknightBlood)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Deathknight_Blood.xml";
                            var currentSetting = new DeathknightBlood.DeathknightBloodSettings();
                            if (File.Exists(currentSettingsFile) && !resetSettings)
                            {
                                currentSetting = Settings.Load<DeathknightBlood.DeathknightBloodSettings>(currentSettingsFile);
                            }
                            currentSetting.ToForm();
                            currentSetting.Save(currentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Deathknight Blood Combat class...");
                            InternalRange = 5.0f;
                            EquipmentAndStats.SetPlayerSpe(WoWSpecialization.DeathknightBlood);
                            new DeathknightBlood();
                        }
                        break;
                    }
                    if (wowSpecialization == WoWSpecialization.DeathknightUnholy || wowSpecialization == WoWSpecialization.None)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Deathknight_Unholy.xml";
                            var currentSetting = new DeathknightUnholy.DeathknightUnholySettings();
                            if (File.Exists(currentSettingsFile) && !resetSettings)
                            {
                                currentSetting = Settings.Load<DeathknightUnholy.DeathknightUnholySettings>(currentSettingsFile);
                            }
                            currentSetting.ToForm();
                            currentSetting.Save(currentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Deathknight Unholy Combat class...");
                            InternalRange = 5.0f;
                            EquipmentAndStats.SetPlayerSpe(WoWSpecialization.DeathknightUnholy);
                            new DeathknightUnholy();
                        }
                        break;
                    }
                    if (wowSpecialization == WoWSpecialization.DeathknightFrost)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Deathknight_Frost.xml";
                            var currentSetting = new DeathknightFrost.DeathknightFrostSettings();
                            if (File.Exists(currentSettingsFile) && !resetSettings)
                            {
                                currentSetting = Settings.Load<DeathknightFrost.DeathknightFrostSettings>(currentSettingsFile);
                            }
                            currentSetting.ToForm();
                            currentSetting.Save(currentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Deathknight Frost Combat class...");
                            InternalRange = 5.0f;
                            EquipmentAndStats.SetPlayerSpe(WoWSpecialization.DeathknightFrost);
                            new DeathknightFrost();
                        }
                        break;
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

    internal static void DumpCurrentSettings<T>(object mySettings)
    {
        mySettings = mySettings is T ? (T) mySettings : default(T);
        BindingFlags bindingFlags = BindingFlags.Public |
                                    BindingFlags.NonPublic |
                                    BindingFlags.Instance |
                                    BindingFlags.Static;
        for (int i = 0; i < mySettings.GetType().GetFields(bindingFlags).Length - 1; i++)
        {
            FieldInfo field = mySettings.GetType().GetFields(bindingFlags)[i];
            Logging.WriteDebug(field.Name + " = " + field.GetValue(mySettings));
        }

        // Last field is intentionnally ommited because it's a backing field.
    }
}

#region Deathknight

public class DeathknightBlood
{
    private static DeathknightBloodSettings MySettings = DeathknightBloodSettings.GetSettings();

    #region General Timers & Variables

    private readonly WoWItem _firstTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET);
    private readonly WoWItem _secondTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET, 2);
    public int DecastHP = 0;
    public int DefenseHP = 0;
    public int HealHP = 0;
    public int LC = 0;

    private Timer _onCd = new Timer(0);

    #endregion

    #region Professions & Racials

    public readonly Spell ArcaneTorrent = new Spell("Arcane Torrent");
    public readonly Spell Berserking = new Spell("Berserking");
    public readonly Spell BloodFury = new Spell("Blood Fury");
    public readonly Spell Darkflight = new Spell("Darkflight");
    public readonly Spell EveryManforHimself = new Spell("Every Man for Himself");
    public readonly Spell GiftoftheNaaru = new Spell("Gift of the Naaru");
    public readonly Spell Stoneform = new Spell("Stoneform");
    public readonly Spell WarStomp = new Spell("War Stomp");

    #endregion

    #region Deathknight Presence & Buffs

    public readonly Spell BloodPlague = new Spell(55078);
    public readonly Spell BloodPresence = new Spell("Blood Presence");
    public readonly Spell CrimsonScourge = new Spell(81141);
    public readonly Spell FrostFever = new Spell(55095);
    public readonly Spell FrostPresence = new Spell("Frost Presence");
    public readonly Spell HornofWinter = new Spell("Horn of Winter");
    public readonly Spell NecroticPlague = new Spell(152281);
    public readonly Spell PathofFrost = new Spell("Path of Frost");
    public readonly uint PathofFrostBuff = 3714;
    public readonly Spell UnholyPresence = new Spell("Unholy Presence");
    private Timer _pathofFrostBuffTimer = new Timer(0);

    #endregion

    #region Offensive Spell

    public readonly Spell BloodBoil = new Spell("Blood Boil");
    public readonly Spell DeathCoil = new Spell("Death Coil");
    public readonly Spell DeathStrike = new Spell("Death Strike");
    public readonly Spell DeathandDecay = new Spell("Death and Decay");
    public readonly Spell Defile = new Spell("Defile");
    public readonly Spell IcyTouch = new Spell("Icy Touch");
    public readonly Spell PlagueLeech = new Spell("Plague Leech");
    public readonly Spell PlagueStrike = new Spell("Plague Strike");
    public readonly Spell SoulReaper = new Spell("Soul Reaper");
    public readonly Spell UnholyBlight = new Spell("Unholy Blight");
    private Timer _defileTimer = new Timer(0);

    #endregion

    #region Offensive Cooldown

    public readonly Spell BloodTap = new Spell("Blood Tap");
    public readonly Spell BloodCharge = new Spell(114851);
    public readonly Spell BreathofSindragosa = new Spell("Breath of Sindragosa");
    public readonly Spell DancingRuneWeapon = new Spell("Dancing Rune Weapon");
    public readonly Spell DeathGrip = new Spell("Death Grip");
    public readonly Spell EmpowerRuneWeapon = new Spell("Empower Rune Weapon");
    public readonly Spell Outbreak = new Spell("Outbreak");

    #endregion

    #region Defensive Cooldown

    public readonly Spell AntiMagicShell = new Spell("Anti-Magic Shell");
    public readonly Spell AntiMagicZone = new Spell("Anti-Magic Zone");
    public readonly Spell ArmyoftheDead = new Spell("Army of the Dead");
    public readonly Spell Asphyxiate = new Spell("Asphyxiate");
    public readonly Spell BoneShield = new Spell("Bone Shield");
    public readonly Spell ChainsofIce = new Spell("Chains of Ice");
    public readonly Spell DeathsAdvance = new Spell("Death's Advance");
    public readonly Spell DesecratedGround = new Spell("Desecrated Ground");
    public readonly Spell GorefiendsGrasp = new Spell("Gorefriend's Grasp");
    public readonly Spell IceboundFortitude = new Spell("Icebound Fortitude");
    public readonly Spell MindFreeze = new Spell("Mind Freeze");
    public readonly Spell RemorselessWinter = new Spell("Remorseless Winter");
    public readonly Spell RuneTap = new Spell("Rune Tap");
    public readonly Spell Strangulate = new Spell("Strangulate");
    public readonly Spell VampiricBlood = new Spell("Vampiric Blood");

    #endregion

    #region Healing Spell

    public readonly Spell Conversion = new Spell("Conversion");
    public readonly Spell DeathPact = new Spell("Death Pact");
    public readonly Spell DeathSiphon = new Spell("Death Siphon");
    public readonly Spell Lichborne = new Spell("Lichborne");

    #endregion

    public DeathknightBlood()
    {
        Main.InternalRange = ObjectManager.Me.GetCombatReach;
        MySettings = DeathknightBloodSettings.GetSettings();
        Main.DumpCurrentSettings<DeathknightBloodSettings>(MySettings);
        UInt128 lastTarget = 0;
        LowHP();

        while (Main.InternalLoop)
        {
            try
            {
                if (!ObjectManager.Me.IsDeadMe)
                {
                    BuffPath();
                    if (!ObjectManager.Me.IsMounted)
                    {
                        if (Fight.InFight && ObjectManager.Me.Target > 0)
                        {
                            if (ObjectManager.Me.Target != lastTarget && (DeathGrip.IsHostileDistanceGood || IcyTouch.IsHostileDistanceGood))
                            {
                                Pull();
                                lastTarget = ObjectManager.Me.Target;
                            }

                            if (MySettings.UseLowCombat && (ObjectManager.Me.Level - ObjectManager.Target.Level >= MySettings.UseLowCombatAtPercentage))
                            {
                                LC = 1;
                                if (ObjectManager.Target.GetDistance < 30)
                                    LowCombat();
                            }
                            else
                            {
                                LC = 0;
                                if (ObjectManager.Target.GetDistance < 30)
                                    Combat();
                            }
                        }
                        if (!ObjectManager.Me.IsCast)
                            Patrolling();
                    }
                }
                else
                    Thread.Sleep(500);
            }
            catch
            {
            }
            Thread.Sleep(100);
        }
    }

    private void LowHP()
    {
        if (MySettings.UseAsphyxiateAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseAsphyxiateAtPercentage;

        if (MySettings.UseBoneShieldAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseBoneShieldAtPercentage;

        if (MySettings.UseIceboundFortitudeAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseIceboundFortitudeAtPercentage;

        if (MySettings.UseRemorselessWinterAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseRemorselessWinterAtPercentage;

        if (MySettings.UseRuneTapAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseRuneTapAtPercentage;

        if (MySettings.UseStoneformAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseStoneformAtPercentage;

        if (MySettings.UseWarStompAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseWarStompAtPercentage;

        if (MySettings.UseConversionAtPercentage > HealHP)
            HealHP = MySettings.UseConversionAtPercentage;

        if (MySettings.UseDeathPactAtPercentage > HealHP)
            HealHP = MySettings.UseDeathPactAtPercentage;

        if (MySettings.UseDeathSiphonAtPercentage > HealHP)
            HealHP = MySettings.UseDeathSiphonAtPercentage;

        if (MySettings.UseGiftoftheNaaruAtPercentage > HealHP)
            HealHP = MySettings.UseGiftoftheNaaruAtPercentage;

        if (MySettings.UseLichborneAtPercentage > HealHP)
            HealHP = MySettings.UseLichborneAtPercentage;

        if (MySettings.UseVampiricBloodAtPercentage > HealHP)
            HealHP = MySettings.UseVampiricBloodAtPercentage;

        if (MySettings.UseAntiMagicShellAtPercentage > DecastHP)
            DecastHP = MySettings.UseAntiMagicShellAtPercentage;

        if (MySettings.UseAntiMagicZoneAtPercentage > DecastHP)
            DecastHP = MySettings.UseAntiMagicZoneAtPercentage;

        if (MySettings.UseArcaneTorrentForDecastAtPercentage > DecastHP)
            DecastHP = MySettings.UseArcaneTorrentForDecastAtPercentage;

        if (MySettings.UseAsphyxiateAtPercentage > DecastHP)
            DecastHP = MySettings.UseAsphyxiateAtPercentage;

        if (MySettings.UseMindFreezeAtPercentage > DecastHP)
            DecastHP = MySettings.UseMindFreezeAtPercentage;

        if (MySettings.UseStrangulateAtPercentage > DecastHP)
            DecastHP = MySettings.UseStrangulateAtPercentage;
    }

    private void BuffPath()
    {
        if (MySettings.UsePathofFrost && !ObjectManager.Me.InCombat && _pathofFrostBuffTimer.IsReady
            && (!PathofFrost.HaveBuff || ObjectManager.Me.AuraIsActiveAndExpireInLessThanMs(PathofFrostBuff, 10000)) && PathofFrost.IsSpellUsable)
        {
            PathofFrost.Cast();
            _pathofFrostBuffTimer = new Timer(1000*10);
        }
    }

    private void Pull()
    {
        if (MySettings.UseDeathGrip && DeathGrip.IsSpellUsable && ObjectManager.Target.GetDistance > Main.InternalRange && DeathGrip.IsHostileDistanceGood)
        {
            DeathGrip.Cast();
            MovementManager.StopMove();
            return;
        }
        if (MySettings.UseIcyTouch && IcyTouch.IsSpellUsable && IcyTouch.IsHostileDistanceGood)
            IcyTouch.Cast();
    }

    private void LowCombat()
    {
        Buff();

        if (MySettings.DoAvoidMelee)
            AvoidMelee();

        if (_onCd.IsReady && ObjectManager.Me.HealthPercent <= DefenseHP)
            DefenseCycle();

        if (ObjectManager.Me.HealthPercent <= HealHP)
            Heal();

        if (MySettings.UseIcyTouch && IcyTouch.IsSpellUsable && IcyTouch.IsHostileDistanceGood)
        {
            IcyTouch.Cast();
            return;
        }
        if (MySettings.UseDeathCoil && DeathCoil.IsSpellUsable && DeathCoil.IsHostileDistanceGood)
        {
            DeathCoil.Cast();
            return;
        }
        if (MySettings.UsePlagueStrike && PlagueStrike.IsSpellUsable && PlagueStrike.IsHostileDistanceGood)
        {
            PlagueStrike.Cast();
            return;
        }
        if (MySettings.UseBloodBoil && BloodBoil.IsSpellUsable && BloodBoil.IsHostileDistanceGood)
            BloodBoil.Cast();
    }

    private void Combat()
    {
        Buff();

        if (MySettings.DoAvoidMelee)
            AvoidMelee();

        DPSCycle();

        if (_onCd.IsReady && ObjectManager.Me.HealthPercent <= DefenseHP)
            DefenseCycle();

        if (ObjectManager.Me.HealthPercent <= HealHP)
            Heal();

        if (ObjectManager.Me.HealthPercent <= DecastHP || (MySettings.UseChainsofIce && ObjectManager.Target.GetMove))
            Decast();

        DPSBurst();
        DPSCycle();
    }

    private void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (MySettings.UseBloodPresence && !BloodPresence.HaveBuff && (LC != 1 || (!MySettings.UseUnholyPresence && !MySettings.UseFrostPresence)) && BloodPresence.IsSpellUsable)
            BloodPresence.Cast();

        if (MySettings.UseDarkflight && Darkflight.KnownSpell && !DeathsAdvance.HaveBuff && ObjectManager.Me.GetMove && !ObjectManager.Target.InCombat && Darkflight.IsSpellUsable)
            Darkflight.Cast();

        if (MySettings.UseDeathsAdvance && !Darkflight.HaveBuff && ObjectManager.Me.GetMove && !ObjectManager.Target.InCombat && DeathsAdvance.IsSpellUsable)
            DeathsAdvance.Cast();

        if (MySettings.UseFrostPresence && !MySettings.UseUnholyPresence && LC == 1 && !FrostPresence.HaveBuff
            && ObjectManager.Me.HealthPercent > MySettings.UseBloodPresenceAtPercentage + 10 && FrostPresence.IsSpellUsable)
            FrostPresence.Cast();

        if (MySettings.UseHornofWinter && !HornofWinter.HaveBuff && HornofWinter.IsSpellUsable)
            HornofWinter.Cast();

        if (MySettings.UseUnholyPresence && LC == 1 && !UnholyPresence.HaveBuff && ObjectManager.Me.HealthPercent > MySettings.UseBloodPresenceAtPercentage + 10
            && UnholyPresence.IsSpellUsable)
            UnholyPresence.Cast();

        if (MySettings.UseAlchFlask && ItemsManager.GetItemCount(75525) > 0 && !ObjectManager.Me.HaveBuff(79638) && !ObjectManager.Me.HaveBuff(79640)
            && !ObjectManager.Me.HaveBuff(79639) && !ItemsManager.IsItemOnCooldown(75525))
            ItemsManager.UseItem(75525);
    }

    private void AvoidMelee()
    {
        if (ObjectManager.Target.GetDistance < MySettings.DoAvoidMeleeDistance && ObjectManager.Target.InCombat)
        {
            Logging.WriteFight("Too Close. Moving Back");
            var maxTimeTimer = new Timer(1000*2);
            MovementsAction.MoveBackward(true);
            while (ObjectManager.Target.GetDistance < 2 && ObjectManager.Target.InCombat && !maxTimeTimer.IsReady)
            {
                Others.SafeSleep(300);
            }
            MovementsAction.MoveBackward(false);
            if (maxTimeTimer.IsReady && ObjectManager.Target.GetDistance < 2 && ObjectManager.Target.InCombat)
            {
                MovementsAction.MoveForward(true);
                Others.SafeSleep(1000);
                MovementsAction.MoveForward(false);
                MovementManager.Face(ObjectManager.Target.Position);
            }
        }
    }

    private void DefenseCycle()
    {
        if (MySettings.UseEveryManforHimself && ObjectManager.Me.IsStunned && EveryManforHimself.IsSpellUsable)
            EveryManforHimself.Cast();

        if (MySettings.UseAsphyxiate && ObjectManager.Me.HealthPercent <= MySettings.UseAsphyxiateAtPercentage && Asphyxiate.IsSpellUsable
            && Asphyxiate.IsHostileDistanceGood)
        {
            Asphyxiate.Cast();
            _onCd = new Timer(1000*5);
            return;
        }
        if (MySettings.UseBoneShield && !BoneShield.HaveBuff && ObjectManager.Me.HealthPercent <= MySettings.UseBoneShieldAtPercentage
            && BoneShield.IsSpellUsable)
        {
            BoneShield.Cast();
            return;
        }
        if (MySettings.UseIceboundFortitude && ObjectManager.Me.HealthPercent <= MySettings.UseIceboundFortitudeAtPercentage
            && IceboundFortitude.IsSpellUsable)
        {
            IceboundFortitude.Cast();
            _onCd = new Timer(1000*12);
            return;
        }
        if (MySettings.UseRemorselessWinter && (ObjectManager.Me.HealthPercent <= MySettings.UseRemorselessWinterAtPercentage
                                                || ObjectManager.GetUnitInSpellRange(RemorselessWinter.MaxRangeHostile) > 1) && RemorselessWinter.IsSpellUsable)
        {
            RemorselessWinter.Cast();
            _onCd = new Timer(1000*8);
            return;
        }
        if (MySettings.UseRuneTap && ObjectManager.Me.HealthPercent <= MySettings.UseRuneTapAtPercentage && RuneTap.IsSpellUsable)
        {
            RuneTap.Cast();
            _onCd = new Timer(1000*3);
            return;
        }
        if (MySettings.UseStoneform && ObjectManager.Me.HealthPercent <= MySettings.UseStoneformAtPercentage && Stoneform.IsSpellUsable)
        {
            Stoneform.Cast();
            _onCd = new Timer(1000*8);
            return;
        }
        if (MySettings.UseWarStomp && ObjectManager.Me.HealthPercent <= MySettings.UseWarStompAtPercentage && WarStomp.IsSpellUsable
            && ObjectManager.GetUnitInSpellRange(WarStomp.MaxRangeHostile) > 0)
        {
            WarStomp.Cast();
            _onCd = new Timer(1000*2);
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (MySettings.UseBloodTapForHeal && BloodTap.IsSpellUsable && BloodCharge.BuffStack > 4 && ObjectManager.Me.HealthPercent <= HealHP
            && ObjectManager.Target.GetDistance < 30)
            BloodTap.Cast();

        if (MySettings.UseConversion && ObjectManager.Me.RunicPower > 10 && ObjectManager.Me.HealthPercent <= MySettings.UseConversionAtPercentage
            && Conversion.IsSpellUsable)
        {
            Conversion.Cast();
            while (ObjectManager.Me.IsCast && (ObjectManager.Me.RunicPower > 4 || ObjectManager.Me.HealthPercent < 100))
                Others.SafeSleep(200);
            return;
        }
        if (MySettings.UseDeathPact && ObjectManager.Me.HealthPercent <= MySettings.UseDeathPactAtPercentage && DeathPact.IsSpellUsable)
        {
            DeathPact.Cast();
            return;
        }
        if (MySettings.UseDeathSiphon && ObjectManager.Me.HealthPercent <= MySettings.UseDeathSiphonAtPercentage && DeathSiphon.IsSpellUsable && DeathSiphon.IsHostileDistanceGood)
        {
            DeathSiphon.Cast();
            return;
        }
        if (MySettings.UseGiftoftheNaaru && ObjectManager.Me.HealthPercent <= MySettings.UseGiftoftheNaaruAtPercentage && GiftoftheNaaru.IsSpellUsable)
        {
            GiftoftheNaaru.Cast();
            return;
        }
        if (MySettings.UseLichborne && ObjectManager.Me.HealthPercent <= MySettings.UseLichborneAtPercentage && ObjectManager.Me.RunicPower > 39 && Lichborne.IsSpellUsable)
        {
            Lichborne.Cast();
            return;
        }
        if (MySettings.UseVampiricBlood && ObjectManager.Me.HealthPercent <= MySettings.UseVampiricBloodAtPercentage && VampiricBlood.IsSpellUsable)
            VampiricBlood.Cast();
    }

    private void Decast()
    {
        if (MySettings.UseArcaneTorrentForDecast && ObjectManager.Me.HealthPercent <= MySettings.UseArcaneTorrentForDecastAtPercentage
            && ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && ArcaneTorrent.IsSpellUsable && ObjectManager.Target.GetDistance < 8)
        {
            ArcaneTorrent.Cast();
            return;
        }
        if (MySettings.UseAntiMagicShell && ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe
            && ObjectManager.Me.HealthPercent <= MySettings.UseAntiMagicShellAtPercentage && AntiMagicShell.IsSpellUsable)
        {
            AntiMagicShell.Cast();
            return;
        }
        if (MySettings.UseAntiMagicZone && ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe
            && ObjectManager.Me.HealthPercent <= MySettings.UseAntiMagicZoneAtPercentage && AntiMagicZone.IsSpellUsable)
        {
            SpellManager.CastSpellByIDAndPosition(51052, ObjectManager.Me.Position);
            return;
        }
        if (MySettings.UseAsphyxiate && ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe
            && ObjectManager.Me.HealthPercent <= MySettings.UseAsphyxiateAtPercentage && Asphyxiate.IsSpellUsable && Asphyxiate.IsHostileDistanceGood)
        {
            Asphyxiate.Cast();
            return;
        }
        if (MySettings.UseMindFreeze && ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe
            && ObjectManager.Me.HealthPercent <= MySettings.UseMindFreezeAtPercentage && MindFreeze.IsSpellUsable && MindFreeze.IsHostileDistanceGood)
        {
            MindFreeze.Cast();
            return;
        }
        if (MySettings.UseStrangulate && ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && (MySettings.UseStrangulate && ObjectManager.Me.HealthPercent <= MySettings.UseStrangulateAtPercentage
                                                                                                               || MySettings.UseAsphyxiate && ObjectManager.Me.HealthPercent <= MySettings.UseAsphyxiateAtPercentage) &&
            Strangulate.IsSpellUsable && Strangulate.IsHostileDistanceGood)
        {
            Strangulate.Cast();
            return;
        }
        if (MySettings.UseChainsofIce && ObjectManager.Target.GetMove && !ChainsofIce.TargetHaveBuff && ChainsofIce.IsHostileDistanceGood
            && ChainsofIce.IsSpellUsable)
            ChainsofIce.Cast();
    }

    private void DPSBurst()
    {
        if (MySettings.UseTrinketOne && !ItemsManager.IsItemOnCooldown(_firstTrinket.Entry) && ItemsManager.IsItemUsable(_firstTrinket.Entry))
        {
            ItemsManager.UseItem(_firstTrinket.Name);
            Logging.WriteFight("Use First Trinket Slot");
        }
        if (MySettings.UseTrinketTwo && !ItemsManager.IsItemOnCooldown(_secondTrinket.Entry) && ItemsManager.IsItemUsable(_secondTrinket.Entry))
        {
            ItemsManager.UseItem(_secondTrinket.Name);
            Logging.WriteFight("Use Second Trinket Slot");
        }
        if (MySettings.UseBerserking && Berserking.IsSpellUsable && ObjectManager.Target.GetDistance < 30)
            Berserking.Cast();

        if (MySettings.UseBloodFury && BloodFury.IsSpellUsable && ObjectManager.Target.GetDistance < 30)
            BloodFury.Cast();

        if (MySettings.UseBloodTapForDPS && BloodTap.IsSpellUsable && BloodCharge.BuffStack > 9 && ObjectManager.Target.GetDistance < 30)
            BloodTap.Cast();

        if (MySettings.UseDancingRuneWeapon && DancingRuneWeapon.IsSpellUsable && DancingRuneWeapon.IsHostileDistanceGood)
            DancingRuneWeapon.Cast();
    }

    private void DPSCycle()
    {
        Usefuls.SleepGlobalCooldown();
        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!
            if (Lichborne.HaveBuff && ObjectManager.Me.HealthPercent < 85 && DeathCoil.IsSpellUsable)
            {
                Lua.RunMacroText("/target Player");
                DeathCoil.Cast();
                return;
            }
            if (MySettings.UsePlagueLeech && MySettings.UseOutbreak && !NecroticPlague.KnownSpell && BloodPlague.TargetHaveBuff
                && FrostFever.TargetHaveBuff && Outbreak.IsSpellUsable && PlagueLeech.IsSpellUsable && !DeathStrike.IsSpellUsable && PlagueLeech.IsHostileDistanceGood)
            {
                PlagueLeech.Cast();
                Others.SafeSleep(1000);
                if (!BloodPlague.TargetHaveBuff && Outbreak.IsSpellUsable && Outbreak.IsHostileDistanceGood)
                    Outbreak.Cast();
                return;
            }

            if (MySettings.UseUnholyBlight && NecroticPlague.KnownSpell && (!NecroticPlague.TargetHaveBuff || ObjectManager.Target.AuraIsActiveAndExpireInLessThanMs(NecroticPlague.Id, 4000))
                && UnholyBlight.IsSpellUsable && ObjectManager.Target.GetDistance < 9)
            {
                UnholyBlight.Cast();
                return;
            }
            else if (MySettings.UseUnholyBlight && UnholyBlight.KnownSpell && (!BloodPlague.TargetHaveBuff || ObjectManager.Target.AuraIsActiveAndExpireInLessThanMs(BloodPlague.Id, 4000)
                                                                               || !FrostFever.TargetHaveBuff || ObjectManager.Target.AuraIsActiveAndExpireInLessThanMs(FrostFever.Id, 4000))
                     && UnholyBlight.IsSpellUsable && ObjectManager.Target.GetDistance < 9)
            {
                UnholyBlight.Cast();
                return;
            }

            if (MySettings.UseOutbreak && NecroticPlague.KnownSpell && (!NecroticPlague.TargetHaveBuff || ObjectManager.Target.AuraIsActiveAndExpireInLessThanMs(NecroticPlague.Id, 4000))
                && Outbreak.IsSpellUsable && Outbreak.IsHostileDistanceGood)
            {
                Outbreak.Cast();
                return;
            }
            else if (MySettings.UseOutbreak && Outbreak.KnownSpell && (!BloodPlague.TargetHaveBuff || ObjectManager.Target.AuraIsActiveAndExpireInLessThanMs(BloodPlague.Id, 4000)
                                                                       || !FrostFever.TargetHaveBuff || ObjectManager.Target.AuraIsActiveAndExpireInLessThanMs(FrostFever.Id, 4000))
                     && Outbreak.IsSpellUsable && Outbreak.IsHostileDistanceGood)
            {
                Outbreak.Cast();
                return;
            }

            if (MySettings.UseDefile && Defile.IsSpellUsable && Defile.IsHostileDistanceGood)
            {
                SpellManager.CastSpellByIDAndPosition(152280, ObjectManager.Target.Position);
                _defileTimer = new Timer(1000*30);
                return;
            }
            if (MySettings.UseBreathofSindragosa && ObjectManager.Me.RunicPower > 75 && BreathofSindragosa.IsSpellUsable
                && ObjectManager.Target.GetDistance < 11)
            {
                BreathofSindragosa.Cast();
                return;
            }
            if (MySettings.UseDeathStrike && DeathStrike.IsSpellUsable && DeathStrike.IsHostileDistanceGood)
            {
                if (Defile.KnownSpell && _defileTimer.IsReady)
                    return;
                DeathStrike.Cast();
                return;
            }
            if (MySettings.UseDeathCoil && DeathCoil.IsSpellUsable && DeathCoil.IsHostileDistanceGood)
            {
                if (MySettings.UseBreathofSindragosa && !BreathofSindragosa.IsSpellUsable)
                {
                    if ((MySettings.UseLichborne && ObjectManager.Me.HealthPercent <= MySettings.UseLichborneAtPercentage && Lichborne.IsSpellUsable)
                        || (MySettings.UseConversion && ObjectManager.Me.HealthPercent <= MySettings.UseConversionAtPercentage && Conversion.IsSpellUsable))
                        return;
                    DeathCoil.Cast();
                    return;
                }
                else if (!BreathofSindragosa.KnownSpell)
                {
                    if ((MySettings.UseLichborne && ObjectManager.Me.HealthPercent <= MySettings.UseLichborneAtPercentage && Lichborne.IsSpellUsable)
                        || (MySettings.UseConversion && ObjectManager.Me.HealthPercent <= MySettings.UseConversionAtPercentage && Conversion.IsSpellUsable))
                        return;
                    DeathCoil.Cast();
                    return;
                }
            }

            if (MySettings.UsePlagueStrike && NecroticPlague.KnownSpell && (!NecroticPlague.TargetHaveBuff || ObjectManager.Target.AuraIsActiveAndExpireInLessThanMs(NecroticPlague.Id, 4000))
                && PlagueStrike.IsSpellUsable && PlagueStrike.IsHostileDistanceGood)
            {
                PlagueStrike.Cast();
                return;
            }
            else if (MySettings.UsePlagueStrike && (!BloodPlague.TargetHaveBuff || ObjectManager.Target.AuraIsActiveAndExpireInLessThanMs(BloodPlague.Id, 4000))
                     && !Outbreak.IsSpellUsable && !UnholyBlight.IsSpellUsable && PlagueStrike.IsSpellUsable && PlagueStrike.IsHostileDistanceGood)
            {
                PlagueStrike.Cast();
                return;
            }

            if (MySettings.UseIcyTouch && NecroticPlague.KnownSpell && (!NecroticPlague.TargetHaveBuff || ObjectManager.Target.AuraIsActiveAndExpireInLessThanMs(NecroticPlague.Id, 4000))
                && IcyTouch.IsSpellUsable && IcyTouch.IsHostileDistanceGood)
            {
                IcyTouch.Cast();
                return;
            }
            else if (MySettings.UseIcyTouch && (!FrostFever.TargetHaveBuff || ObjectManager.Target.AuraIsActiveAndExpireInLessThanMs(FrostFever.Id, 4000))
                     && !Outbreak.IsSpellUsable && !UnholyBlight.IsSpellUsable && IcyTouch.IsSpellUsable && IcyTouch.IsHostileDistanceGood)
            {
                IcyTouch.Cast();
                return;
            }

            if (MySettings.UseGorefiendsGrasp && GorefiendsGrasp.IsSpellUsable && ObjectManager.GetUnitInSpellRange(GorefiendsGrasp.MaxRangeHostile) > 2)
            {
                GorefiendsGrasp.Cast();
                return;
            }
            /* CURRENTLY TOO WEAK FOR SINGLE TARGET OR AOE
            if (MySettings.UseDeathandDecay && !Defile.KnownSpell && DeathandDecay.IsSpellUsable && DeathandDecay.IsHostileDistanceGood)
            {
                SpellManager.CastSpellByIDAndPosition(43265, ObjectManager.Target.Position);
                return;
            }
             * */
            if (MySettings.UseArmyoftheDead && ArmyoftheDead.IsSpellUsable && ObjectManager.GetUnitInSpellRange(ArmyoftheDead.MaxRangeHostile) > 3)
            {
                ArmyoftheDead.Cast();
                return;
            }
            if (MySettings.UseSoulReaper && ObjectManager.Target.HealthPercent < 35 && SoulReaper.IsSpellUsable && !DeathStrike.IsSpellUsable
                && SoulReaper.IsHostileDistanceGood)
            {
                SoulReaper.Cast();
                return;
            }
            if (MySettings.UseBloodBoil && BloodBoil.IsSpellUsable && (!DeathStrike.IsSpellUsable || CrimsonScourge.HaveBuff) && BloodBoil.IsHostileDistanceGood)
            {
                BloodBoil.Cast();
                return;
            }
            if ((!MySettings.UseDeathStrike || !DeathStrike.KnownSpell) && MySettings.UsePlagueStrike && PlagueStrike.IsSpellUsable && PlagueStrike.IsHostileDistanceGood)
            {
                PlagueStrike.Cast();
                return;
            }
            if ((!MySettings.UseDeathStrike || !DeathStrike.KnownSpell) && MySettings.UseIcyTouch && IcyTouch.IsSpellUsable && IcyTouch.IsHostileDistanceGood)
            {
                IcyTouch.Cast();
                return;
            }
            if (MySettings.UseArcaneTorrentForResource && ObjectManager.Me.RunicPowerPercentage < 85 && ArcaneTorrent.IsSpellUsable)
            {
                ArcaneTorrent.Cast();
                return;
            }
            if (MySettings.UseEmpowerRuneWeapon && ObjectManager.Me.RunicPowerPercentage < 75 && EmpowerRuneWeapon.IsSpellUsable)
                EmpowerRuneWeapon.Cast();
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    private void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            Buff();
            Heal();
        }
    }

    #region Nested type: DeathknightBloodSettings

    [Serializable]
    public class DeathknightBloodSettings : Settings
    {
        public bool DoAvoidMelee = false;
        public int DoAvoidMeleeDistance = 0;
        public bool UseAlchFlask = true;
        public bool UseAntiMagicShell = true;
        public int UseAntiMagicShellAtPercentage = 95;
        public bool UseAntiMagicZone = true;
        public int UseAntiMagicZoneAtPercentage = 95;
        public bool UseArcaneTorrentForDecast = true;
        public int UseArcaneTorrentForDecastAtPercentage = 95;
        public bool UseArcaneTorrentForResource = true;
        public bool UseArmyoftheDead = true;
        public bool UseAsphyxiate = true;
        public int UseAsphyxiateAtPercentage = 90;
        public bool UseBerserking = true;
        public bool UseBloodBoil = true;
        public bool UseBloodFury = true;
        public bool UseBloodPresence = true;
        public int UseBloodPresenceAtPercentage = 50;
        public bool UseBloodTapForDPS = true;
        public bool UseBloodTapForHeal = true;
        public bool UseBreathofSindragosa = true;
        public bool UseBoneShield = true;
        public int UseBoneShieldAtPercentage = 100;
        public bool UseChainsofIce = false;
        public bool UseConversion = true;
        public int UseConversionAtPercentage = 45;
        public bool UseDancingRuneWeapon = true;
        public bool UseDarkflight = true;
        public bool UseDeathCoil = true;
        public bool UseDeathGrip = true;
        public bool UseDeathPact = true;
        public int UseDeathPactAtPercentage = 55;
        public bool UseDeathSiphon = true;
        public int UseDeathSiphonAtPercentage = 80;
        public bool UseDeathStrike = true;
        public bool UseDeathandDecay = true;
        public bool UseDefile = true;
        public bool UseDeathsAdvance = true;
        public bool UseEmpowerRuneWeapon = true;
        public bool UseEveryManforHimself = true;
        public bool UseFrostPresence = true;
        public bool UseGiftoftheNaaru = true;
        public int UseGiftoftheNaaruAtPercentage = 80;
        public bool UseGorefiendsGrasp = true;
        public bool UseHornofWinter = true;
        public bool UseIceboundFortitude = true;
        public int UseIceboundFortitudeAtPercentage = 80;
        public bool UseIcyTouch = true;
        public bool UseLichborne = true;
        public int UseLichborneAtPercentage = 45;
        public bool UseLowCombat = true;
        public int UseLowCombatAtPercentage = 15;
        public bool UseMindFreeze = true;
        public int UseMindFreezeAtPercentage = 100;
        public bool UseOutbreak = true;
        public bool UsePathofFrost = true;
        public bool UsePlagueLeech = true;
        public bool UsePlagueStrike = true;
        public bool UseRemorselessWinter = true;
        public int UseRemorselessWinterAtPercentage = 70;
        public bool UseRuneTap = true;
        public int UseRuneTapAtPercentage = 70;
        public bool UseSoulReaper = true;
        public bool UseStoneform = true;
        public int UseStoneformAtPercentage = 80;
        public bool UseStrangulate = true;
        public int UseStrangulateAtPercentage = 95;
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;
        public bool UseUnholyBlight = true;
        public bool UseUnholyPresence = true;
        public bool UseVampiricBlood = true;
        public int UseVampiricBloodAtPercentage = 70;
        public bool UseWarStomp = true;
        public int UseWarStompAtPercentage = 80;

        public DeathknightBloodSettings()
        {
            ConfigWinForm("Deathknight Blood Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent for Interrupt", "UseArcaneTorrentForDecast", "Professions && Racials", "AtPercentage");
            AddControlInWinForm("Use Arcane Torrent for Resource", "UseArcaneTorrentForResource", "Professions && Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions && Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions && Racials");
            AddControlInWinForm("Use Darkflight", "UseDarkflight", "Professions & Racials");
            AddControlInWinForm("Use Every Man for Himself", "UseEveryManforHimself", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions && Racials", "AtPercentage");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions && Racials", "AtPercentage");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions && Racials", "AtPercentage");
            /* Deathknight Presence & Buffs */
            AddControlInWinForm("Use Blood Presence", "UseBloodPresence", "Deathknight Presence && Buffs", "AtPercentage");
            AddControlInWinForm("Use Frost Presence", "UseFrostPresence", "Deathknight Presence && Buffs");
            AddControlInWinForm("Use Horn of Winter", "UseHornofWinter", "Deathknight Presence && Buffs");
            AddControlInWinForm("Use Path of Frost", "UsePathofFrost", "Deathknight Presence && Buffs");
            AddControlInWinForm("Use Unholy Presence", "UseUnholyPresence", "Deathknight Presence && Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Blood Boil", "UseBloodBoil", "Offensive Spell");
            AddControlInWinForm("Use Death Coil", "UseDeathCoil", "Offensive Spell");
            AddControlInWinForm("Use Death and Decay", "UseDeathandDecay", "Offensive Spell");
            AddControlInWinForm("Use Death Strike", "UseDeathStrike", "Offensive Spell");
            AddControlInWinForm("Use Defile", "UseDefile", "Offensive Spell");
            AddControlInWinForm("Use Icy Touch", "UseIcyTouch", "Offensive Spell");
            AddControlInWinForm("Use Plague Leech", "UsePlagueLeech", "Offensive Spell");
            AddControlInWinForm("Use Plague Strike", "UsePlagueStrike", "Offensive Spell");
            AddControlInWinForm("Use Soul Reaper", "UseSoulReaper", "Offensive Spell");
            AddControlInWinForm("Use Unholy Blight", "UseUnholyBlight", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use Blood Tap for DPS", "UseBloodTapForDPS", "Offensive Cooldown");
            AddControlInWinForm("Use Blood Tap for Healing", "UseBloodTapForHeal", "Offensive Cooldown");
            AddControlInWinForm("Use Breath of Sindragosa", "UseBreathofSindragosa", "Offensive Cooldown");
            AddControlInWinForm("Use Dancing Rune Weapon", "UseDancingRuneWeapon", "Offensive Cooldown");
            AddControlInWinForm("Use Death Grip", "UseDeathGrip", "Offensive Cooldown");
            AddControlInWinForm("Use Empower Rune Weapon", "UseEmpowerRuneWeapon", "Offensive Cooldown");
            AddControlInWinForm("Use Outbreak", "UseOutbreak", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Anti-Magic Shell", "UseAntiMagicShell", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Anti-Magic Zone", "UseAntiMagicZone", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Army of the Dead", "UseArmyoftheDead", "Defensive Cooldown");
            AddControlInWinForm("Use Asphyxiate", "UseAsphyxiate", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Bone Shield", "UseBoneShield", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Chains of Ice", "UseChainsofIce", "Defensive Cooldown");
            AddControlInWinForm("Use Death's Advance", "UseDeathsAdvance", "Defensive Cooldown");
            AddControlInWinForm("Use Gorefiend's Grasp", "UseGorefiendsGrasp", "Defensive Cooldown");
            AddControlInWinForm("Use Icebound Fortitude", "UseIceboundFortitude", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Mind Freeze", "UseMindFreeze", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Remorseless Winter", "UseRemorseless Winter", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Rune Tap", "UseRuneTap", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Strangulate", "UseStrangulate", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Vampiric Blood", "UseVampiricBlood", "Defensive Cooldown", "AtPercentage");
            /* Healing Spell */
            AddControlInWinForm("Use Conversion", "UseConversion", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Death Pact", "UseDeathPact", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Death Siphon", "UseDeathSiphon", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Lichborne", "UseLichborne", "Healing Spell", "AtPercentage");
            /* Game Settings */
            AddControlInWinForm("Use Low Combat Settings - Level Differnce", "UseLowCombat", "Game Settings", "AtPercentage");
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");
            AddControlInWinForm("Use Engineer Gloves", "UseEngGlove", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
            AddControlInWinForm("Do avoid melee (Off Advised!!)", "DoAvoidMelee", "Game Settings");
            AddControlInWinForm("Avoid melee distance (1 to 4)", "DoAvoidMeleeDistance", "Game Settings");
        }

        public static DeathknightBloodSettings CurrentSetting { get; set; }

        public static DeathknightBloodSettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Deathknight_Blood.xml";
            if (File.Exists(currentSettingsFile))
            {
                return
                    CurrentSetting = Load<DeathknightBloodSettings>(currentSettingsFile);
            }
            return new DeathknightBloodSettings();
        }
    }

    #endregion
}

public class DeathknightUnholy
{
    private static DeathknightUnholySettings MySettings = DeathknightUnholySettings.GetSettings();

    #region General Timers & Variables

    private readonly WoWItem _firstTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET);
    private readonly WoWItem _secondTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET, 2);
    public int DecastHP = 0;
    public int DefenseHP = 0;
    public int HealHP = 0;
    public int LC = 0;

    private Timer _onCd = new Timer(0);

    #endregion

    #region Professions & Racials

    public readonly Spell ArcaneTorrent = new Spell("Arcane Torrent");
    public readonly Spell Berserking = new Spell("Berserking");
    public readonly Spell BloodFury = new Spell("Blood Fury");
    public readonly Spell Darkflight = new Spell("Darkflight");
    public readonly Spell EveryManforHimself = new Spell("Every Man for Himself");
    public readonly Spell GiftoftheNaaru = new Spell("Gift of the Naaru");
    public readonly Spell Stoneform = new Spell("Stoneform");
    public readonly Spell WarStomp = new Spell("War Stomp");

    #endregion

    #region Deathknight Presence & Buffs

    public readonly Spell BloodPlague = new Spell(55078);
    public readonly Spell BloodPresence = new Spell("Blood Presence");
    public readonly Spell FrostFever = new Spell(55095);
    public readonly Spell FrostPresence = new Spell("Frost Presence");
    public readonly Spell HornofWinter = new Spell("Horn of Winter");
    public readonly Spell NecroticPlague = new Spell(152281);
    public readonly Spell PathofFrost = new Spell("Path of Frost");
    public readonly uint PathofFrostBuff = 3714;
    public readonly Spell ShadowInfusion = new Spell(91342);
    public readonly Spell SuddenDoom = new Spell(81340);
    public readonly Spell UnholyPresence = new Spell("Unholy Presence");
    private Timer _pathofFrostBuffTimer = new Timer(0);

    #endregion

    #region Offensive Spell

    public readonly Spell BloodBoil = new Spell("Blood Boil");
    public readonly Spell DeathandDecay = new Spell("Death and Decay");
    public readonly Spell DeathCoil = new Spell("Death Coil");
    public readonly Spell Defile = new Spell("Defile");
    public readonly Spell FesteringStrike = new Spell("Festering Strike");
    public readonly Spell IcyTouch = new Spell("Icy Touch");
    public readonly Spell PlagueLeech = new Spell("Plague Leech");
    public readonly Spell PlagueStrike = new Spell("Plague Strike");
    public readonly Spell ScourgeStrike = new Spell("Scourge Strike");
    public readonly Spell SoulReaper = new Spell("Soul Reaper");
    public readonly Spell UnholyBlight = new Spell("Unholy Blight");
    private Timer _defileTimer = new Timer(0);

    #endregion

    #region Offensive Cooldown

    public readonly Spell BloodTap = new Spell("Blood Tap");
    public readonly Spell BloodCharge = new Spell(114851);
    public readonly Spell BreathofSindragosa = new Spell("Breath of Sindragosa");
    public readonly Spell DarkTransformation = new Spell("Dark Transformation");
    public readonly Spell DeathGrip = new Spell("Death Grip");
    public readonly Spell EmpowerRuneWeapon = new Spell("Empower Rune Weapon");
    public readonly Spell Outbreak = new Spell("Outbreak");
    public readonly Spell RaiseDead = new Spell("Raise Dead");
    public readonly Spell SummonGargoyle = new Spell("Summon Gargoyle");

    #endregion

    #region Defensive Cooldown

    public readonly Spell AntiMagicShell = new Spell("Anti-Magic Shell");
    public readonly Spell AntiMagicZone = new Spell("Anti-Magic Zone");
    public readonly Spell ArmyoftheDead = new Spell("Army of the Dead");
    public readonly Spell Asphyxiate = new Spell("Asphyxiate");
    public readonly Spell ChainsofIce = new Spell("Chains of Ice");
    public readonly Spell DeathsAdvance = new Spell("Death's Advance");
    public readonly Spell DesecratedGround = new Spell("Desecrated Ground");
    public readonly Spell GorefiendsGrasp = new Spell("Gorefriend's Grasp");
    public readonly Spell IceboundFortitude = new Spell("Icebound Fortitude");
    public readonly Spell MindFreeze = new Spell("Mind Freeze");
    public readonly Spell RemorselessWinter = new Spell("Remorseless Winter");
    public readonly Spell Strangulate = new Spell("Strangulate");

    #endregion

    #region Healing Spell

    public readonly Spell Conversion = new Spell("Conversion");
    public readonly Spell DeathPact = new Spell("Death Pact");
    public readonly Spell DeathSiphon = new Spell("Death Siphon");
    public readonly Spell DeathStrike = new Spell("Death Strike");
    public readonly Spell Lichborne = new Spell("Lichborne");

    #endregion

    public DeathknightUnholy()
    {
        Main.InternalAggroRange = 29f;
        Main.InternalRange = ObjectManager.Me.GetCombatReach;
        MySettings = DeathknightUnholySettings.GetSettings();
        Main.DumpCurrentSettings<DeathknightUnholySettings>(MySettings);
        UInt128 lastTarget = 0;
        LowHP();

        while (Main.InternalLoop)
        {
            try
            {
                if (!ObjectManager.Me.IsDeadMe)
                {
                    BuffPath();
                    if (!ObjectManager.Me.IsMounted)
                    {
                        if (Fight.InFight && ObjectManager.Me.Target > 0)
                        {
                            if (ObjectManager.Me.Target != lastTarget && (DeathGrip.IsHostileDistanceGood || IcyTouch.IsHostileDistanceGood))
                            {
                                Pull();
                                lastTarget = ObjectManager.Me.Target;
                            }

                            if (MySettings.UseLowCombat && (ObjectManager.Me.Level - ObjectManager.Target.Level >= MySettings.UseLowCombatAtPercentage))
                            {
                                LC = 1;
                                if (ObjectManager.Target.GetDistance < Main.InternalAggroRange)
                                    LowCombat();
                            }
                            else
                            {
                                LC = 0;
                                if (ObjectManager.Target.GetDistance < Main.InternalAggroRange)
                                    Combat();
                            }
                        }
                        if (!ObjectManager.Me.IsCast)
                            Patrolling();
                    }
                }
                else
                    Thread.Sleep(500);
            }
            catch
            {
            }
            Thread.Sleep(100);
        }
    }

    private void LowHP()
    {
        if (MySettings.UseAsphyxiateAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseAsphyxiateAtPercentage;

        if (MySettings.UseIceboundFortitudeAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseIceboundFortitudeAtPercentage;

        if (MySettings.UseRemorselessWinterAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseRemorselessWinterAtPercentage;

        if (MySettings.UseStoneformAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseStoneformAtPercentage;

        if (MySettings.UseWarStompAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseWarStompAtPercentage;

        if (MySettings.UseConversionAtPercentage > HealHP)
            HealHP = MySettings.UseConversionAtPercentage;

        if (MySettings.UseDeathPactAtPercentage > HealHP)
            HealHP = MySettings.UseDeathPactAtPercentage;

        if (MySettings.UseDeathSiphonAtPercentage > HealHP)
            HealHP = MySettings.UseDeathSiphonAtPercentage;

        if (MySettings.UseDeathStrikeAtPercentage > HealHP)
            HealHP = MySettings.UseDeathStrikeAtPercentage;

        if (MySettings.UseGiftoftheNaaruAtPercentage > HealHP)
            HealHP = MySettings.UseGiftoftheNaaruAtPercentage;

        if (MySettings.UseLichborneAtPercentage > HealHP)
            HealHP = MySettings.UseLichborneAtPercentage;

        if (MySettings.UseAntiMagicShellAtPercentage > DecastHP)
            DecastHP = MySettings.UseAntiMagicShellAtPercentage;

        if (MySettings.UseAntiMagicZoneAtPercentage > DecastHP)
            DecastHP = MySettings.UseAntiMagicZoneAtPercentage;

        if (MySettings.UseArcaneTorrentForDecastAtPercentage > DecastHP)
            DecastHP = MySettings.UseArcaneTorrentForDecastAtPercentage;

        if (MySettings.UseAsphyxiateAtPercentage > DecastHP)
            DecastHP = MySettings.UseAsphyxiateAtPercentage;

        if (MySettings.UseMindFreezeAtPercentage > DecastHP)
            DecastHP = MySettings.UseMindFreezeAtPercentage;

        if (MySettings.UseStrangulateAtPercentage > DecastHP)
            DecastHP = MySettings.UseStrangulateAtPercentage;
    }

    private void BuffPath()
    {
        if (MySettings.UsePathofFrost && !ObjectManager.Me.InCombat && _pathofFrostBuffTimer.IsReady
            && (!PathofFrost.HaveBuff || ObjectManager.Me.AuraIsActiveAndExpireInLessThanMs(PathofFrostBuff, 10000)) && PathofFrost.IsSpellUsable)
        {
            PathofFrost.Cast();
            _pathofFrostBuffTimer = new Timer(1000*10);
        }
    }

    private void Pull()
    {
        if (ObjectManager.Pet.IsAlive)
        {
            Lua.RunMacroText("/petattack");
            Logging.WriteFight("Cast Pet Attack");
        }

        if (MySettings.UseDeathGrip && DeathGrip.IsSpellUsable && ObjectManager.Target.GetDistance > Main.InternalRange && DeathGrip.IsHostileDistanceGood)
        {
            DeathGrip.Cast();
            MovementManager.StopMove();
            return;
        }
        if (MySettings.UseIcyTouch && IcyTouch.IsSpellUsable && IcyTouch.IsHostileDistanceGood)
            IcyTouch.Cast();
    }

    private void LowCombat()
    {
        Buff();

        if (MySettings.DoAvoidMelee)
            AvoidMelee();

        if (_onCd.IsReady && ObjectManager.Me.HealthPercent <= DefenseHP)
            DefenseCycle();

        if (ObjectManager.Me.HealthPercent <= HealHP)
            Heal();

        if (MySettings.UseIcyTouch && IcyTouch.IsSpellUsable && IcyTouch.IsHostileDistanceGood)
        {
            IcyTouch.Cast();
            return;
        }
        if (MySettings.UseDeathCoil && DeathCoil.IsSpellUsable && DeathCoil.IsHostileDistanceGood)
        {
            DeathCoil.Cast();
            return;
        }
        if (MySettings.UseScourgeStrike && ScourgeStrike.IsSpellUsable && ScourgeStrike.IsHostileDistanceGood)
        {
            ScourgeStrike.Cast();
            return;
        }
        if (MySettings.UseBloodBoil && BloodBoil.IsSpellUsable && BloodBoil.IsHostileDistanceGood)
            BloodBoil.Cast();
    }

    private void Combat()
    {
        Buff();

        if (MySettings.DoAvoidMelee)
            AvoidMelee();

        DPSCycle();

        if (_onCd.IsReady && ObjectManager.Me.HealthPercent <= DefenseHP)
            DefenseCycle();

        if (ObjectManager.Me.HealthPercent <= HealHP)
            Heal();

        if (ObjectManager.Me.HealthPercent <= DecastHP || (MySettings.UseChainsofIce && ObjectManager.Target.GetMove))
            Decast();

        DPSBurst();
        DPSCycle();
    }

    private void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (MySettings.UseBloodPresence && !BloodPresence.HaveBuff && ObjectManager.Me.HealthPercent <= MySettings.UseBloodPresenceAtPercentage
            && BloodPresence.IsSpellUsable)
            BloodPresence.Cast();

        if (MySettings.UseDarkflight && Darkflight.KnownSpell && !DeathsAdvance.HaveBuff && ObjectManager.Me.GetMove && !ObjectManager.Target.InCombat && Darkflight.IsSpellUsable)
            Darkflight.Cast();

        if (MySettings.UseDeathsAdvance && !Darkflight.HaveBuff && ObjectManager.Me.GetMove && !ObjectManager.Target.InCombat && DeathsAdvance.IsSpellUsable)
            DeathsAdvance.Cast();

        if (MySettings.UseFrostPresence && !MySettings.UseUnholyPresence && !FrostPresence.HaveBuff
            && (ObjectManager.Me.HealthPercent > MySettings.UseBloodPresenceAtPercentage + 10 || !MySettings.UseBloodPresence) && FrostPresence.IsSpellUsable)
            FrostPresence.Cast();

        if (MySettings.UseHornofWinter && !HornofWinter.HaveBuff && HornofWinter.IsSpellUsable)
            HornofWinter.Cast();

        if (MySettings.UseRaiseDead && (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0) && RaiseDead.IsSpellUsable)
        {
            Logging.WriteFight(" - PET DEAD - ");
            Logging.WriteFight(" - SUMMONING PET - ");
            RaiseDead.Cast();
        }

        if (MySettings.UseUnholyPresence && !UnholyPresence.HaveBuff && (ObjectManager.Me.HealthPercent > MySettings.UseBloodPresenceAtPercentage + 10 || !MySettings.UseBloodPresence)
            && UnholyPresence.IsSpellUsable)
            UnholyPresence.Cast();

        if (MySettings.UseAlchFlask && ItemsManager.GetItemCount(75525) > 0 && !ObjectManager.Me.HaveBuff(79638) && !ObjectManager.Me.HaveBuff(79640) && !ObjectManager.Me.HaveBuff(79639)
            && !ItemsManager.IsItemOnCooldown(75525))
            ItemsManager.UseItem(75525);
    }

    private void AvoidMelee()
    {
        if (ObjectManager.Target.GetDistance < MySettings.DoAvoidMeleeDistance && ObjectManager.Target.InCombat)
        {
            Logging.WriteFight("Too Close. Moving Back");
            var maxTimeTimer = new Timer(1000*2);
            MovementsAction.MoveBackward(true);
            while (ObjectManager.Target.GetDistance < 2 && ObjectManager.Target.InCombat && !maxTimeTimer.IsReady)
            {
                Others.SafeSleep(300);
            }
            MovementsAction.MoveBackward(false);
            if (maxTimeTimer.IsReady && ObjectManager.Target.GetDistance < 2 && ObjectManager.Target.InCombat)
            {
                MovementsAction.MoveForward(true);
                Others.SafeSleep(1000);
                MovementsAction.MoveForward(false);
                MovementManager.Face(ObjectManager.Target.Position);
            }
        }
    }

    private void DefenseCycle()
    {
        if (MySettings.UseEveryManforHimself && ObjectManager.Me.IsStunned && EveryManforHimself.IsSpellUsable)
            EveryManforHimself.Cast();

        if (MySettings.UseAsphyxiate && ObjectManager.Me.HealthPercent <= MySettings.UseAsphyxiateAtPercentage && Asphyxiate.IsSpellUsable
            && Asphyxiate.IsHostileDistanceGood)
        {
            Asphyxiate.Cast();
            _onCd = new Timer(1000*5);
            return;
        }
        if (MySettings.UseIceboundFortitude && ObjectManager.Me.HealthPercent <= MySettings.UseIceboundFortitudeAtPercentage
            && IceboundFortitude.IsSpellUsable)
        {
            IceboundFortitude.Cast();
            _onCd = new Timer(1000*12);
            return;
        }
        if (MySettings.UseRemorselessWinter && (ObjectManager.Me.HealthPercent <= MySettings.UseRemorselessWinterAtPercentage
                                                || ObjectManager.GetUnitInSpellRange(RemorselessWinter.MaxRangeHostile) > 1) && RemorselessWinter.IsSpellUsable)
        {
            RemorselessWinter.Cast();
            _onCd = new Timer(1000*8);
            return;
        }
        if (MySettings.UseStoneform && ObjectManager.Me.HealthPercent <= MySettings.UseStoneformAtPercentage && Stoneform.IsSpellUsable)
        {
            Stoneform.Cast();
            _onCd = new Timer(1000*8);
            return;
        }
        if (MySettings.UseWarStomp && ObjectManager.Me.HealthPercent <= MySettings.UseWarStompAtPercentage && WarStomp.IsSpellUsable
            && ObjectManager.GetUnitInSpellRange(WarStomp.MaxRangeHostile) > 0)
        {
            WarStomp.Cast();
            _onCd = new Timer(1000*2);
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (MySettings.UseBloodTapForHeal && BloodTap.IsSpellUsable && BloodCharge.BuffStack > 4 && ObjectManager.Me.HealthPercent <= HealHP
            && ObjectManager.Target.GetDistance < 30)
            BloodTap.Cast();

        if (MySettings.UseConversion && ObjectManager.Me.RunicPower > 10 && ObjectManager.Me.HealthPercent <= MySettings.UseConversionAtPercentage
            && Conversion.IsSpellUsable)
        {
            Conversion.Cast();
            while (ObjectManager.Me.IsCast && (ObjectManager.Me.RunicPower > 4 || ObjectManager.Me.HealthPercent < 100))
                Others.SafeSleep(200);
            return;
        }
        if (MySettings.UseDeathPact && ObjectManager.Me.HealthPercent <= MySettings.UseDeathPactAtPercentage && DeathPact.IsSpellUsable)
        {
            DeathPact.Cast();
            return;
        }
        if (MySettings.UseDeathSiphon && ObjectManager.Me.HealthPercent <= MySettings.UseDeathSiphonAtPercentage && DeathSiphon.IsSpellUsable && DeathSiphon.IsHostileDistanceGood)
        {
            DeathSiphon.Cast();
            return;
        }
        if (MySettings.UseDeathStrike && ObjectManager.Target.IsValid && ObjectManager.Target.IsAlive
            && ObjectManager.Me.HealthPercent <= MySettings.UseDeathStrikeAtPercentage && DeathStrike.IsSpellUsable && DeathStrike.IsHostileDistanceGood)
        {
            DeathStrike.Cast();
            return;
        }
        if (MySettings.UseGiftoftheNaaru && ObjectManager.Me.HealthPercent <= MySettings.UseGiftoftheNaaruAtPercentage && GiftoftheNaaru.IsSpellUsable)
        {
            GiftoftheNaaru.Cast();
            return;
        }
        if (MySettings.UseLichborne && ObjectManager.Me.HealthPercent <= MySettings.UseLichborneAtPercentage && ObjectManager.Me.RunicPower > 39 && Lichborne.IsSpellUsable)
            Lichborne.Cast();
    }

    private void Decast()
    {
        if (MySettings.UseArcaneTorrentForDecast && ObjectManager.Me.HealthPercent <= MySettings.UseArcaneTorrentForDecastAtPercentage && ObjectManager.Target.IsCast
            && ObjectManager.Target.IsTargetingMe && ArcaneTorrent.IsSpellUsable && ObjectManager.Target.GetDistance < 8)
        {
            ArcaneTorrent.Cast();
            return;
        }
        if (MySettings.UseAntiMagicShell && ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe
            && ObjectManager.Me.HealthPercent <= MySettings.UseAntiMagicShellAtPercentage && AntiMagicShell.IsSpellUsable)
        {
            AntiMagicShell.Cast();
            return;
        }
        if (MySettings.UseAntiMagicZone && ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe
            && ObjectManager.Me.HealthPercent <= MySettings.UseAntiMagicZoneAtPercentage && AntiMagicZone.IsSpellUsable)
        {
            SpellManager.CastSpellByIDAndPosition(51052, ObjectManager.Me.Position);
            return;
        }
        if (MySettings.UseAsphyxiate && ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe
            && ObjectManager.Me.HealthPercent <= MySettings.UseAsphyxiateAtPercentage && Asphyxiate.IsSpellUsable && Asphyxiate.IsHostileDistanceGood)
        {
            Asphyxiate.Cast();
            return;
        }
        if (MySettings.UseMindFreeze && ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe
            && ObjectManager.Me.HealthPercent <= MySettings.UseMindFreezeAtPercentage && MindFreeze.IsSpellUsable && MindFreeze.IsHostileDistanceGood)
        {
            MindFreeze.Cast();
            return;
        }
        if (MySettings.UseStrangulate && ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && (MySettings.UseStrangulate && ObjectManager.Me.HealthPercent <= MySettings.UseStrangulateAtPercentage
                                                                                                               || MySettings.UseAsphyxiate && ObjectManager.Me.HealthPercent <= MySettings.UseAsphyxiateAtPercentage) &&
            Strangulate.IsSpellUsable && Strangulate.IsHostileDistanceGood)
        {
            Strangulate.Cast();
            return;
        }
        if (MySettings.UseChainsofIce && ObjectManager.Target.GetMove && !ChainsofIce.TargetHaveBuff && ChainsofIce.IsHostileDistanceGood && ChainsofIce.IsSpellUsable)
            ChainsofIce.Cast();
    }

    private void DPSBurst()
    {
        if (MySettings.UseTrinketOne && !ItemsManager.IsItemOnCooldown(_firstTrinket.Entry) && ItemsManager.IsItemUsable(_firstTrinket.Entry))
        {
            ItemsManager.UseItem(_firstTrinket.Name);
            Logging.WriteFight("Use First Trinket Slot");
        }
        if (MySettings.UseTrinketTwo && !ItemsManager.IsItemOnCooldown(_secondTrinket.Entry) && ItemsManager.IsItemUsable(_secondTrinket.Entry))
        {
            ItemsManager.UseItem(_secondTrinket.Name);
            Logging.WriteFight("Use Second Trinket Slot");
        }
        if (MySettings.UseBerserking && Berserking.IsSpellUsable && ObjectManager.Target.GetDistance < 30)
            Berserking.Cast();

        if (MySettings.UseBloodFury && BloodFury.IsSpellUsable && ObjectManager.Target.GetDistance < 30)
            BloodFury.Cast();

        if (MySettings.UseBloodTapForDPS && BloodTap.IsSpellUsable && BloodCharge.BuffStack > 9 && ObjectManager.Target.GetDistance < 30)
            BloodTap.Cast();

        if (MySettings.UseSummonGargoyle && SummonGargoyle.IsSpellUsable && ObjectManager.Target.GetDistance < 30)
            SummonGargoyle.Cast();
    }

    private void DPSCycle()
    {
        Usefuls.SleepGlobalCooldown();
        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            if (Lichborne.HaveBuff && ObjectManager.Me.HealthPercent < 85 && DeathCoil.IsSpellUsable)
            {
                Lua.RunMacroText("/target Player");
                DeathCoil.Cast();
                return;
            }
            if (MySettings.UsePlagueLeech && MySettings.UseOutbreak && !NecroticPlague.KnownSpell && BloodPlague.TargetHaveBuff
                && FrostFever.TargetHaveBuff && Outbreak.IsSpellUsable && PlagueLeech.IsSpellUsable && !FesteringStrike.IsSpellUsable && PlagueLeech.IsHostileDistanceGood)
            {
                PlagueLeech.Cast();
                Others.SafeSleep(1000);
                if (!BloodPlague.TargetHaveBuff && Outbreak.IsSpellUsable && Outbreak.IsHostileDistanceGood)
                    Outbreak.Cast();
                return;
            }

            if (MySettings.UseUnholyBlight && UnholyBlight.KnownSpell && NecroticPlague.KnownSpell && (!NecroticPlague.TargetHaveBuff || ObjectManager.Me.BuffStack(155159) < 15
                                                                                                       || ObjectManager.Target.AuraIsActiveAndExpireInLessThanMs(NecroticPlague.Id, 4000)) &&
                UnholyBlight.IsSpellUsable && ObjectManager.Target.GetDistance < 9)
            {
                UnholyBlight.Cast();
                return;
            }
            else if (MySettings.UseUnholyBlight && UnholyBlight.KnownSpell && (!BloodPlague.TargetHaveBuff || ObjectManager.Target.AuraIsActiveAndExpireInLessThanMs(BloodPlague.Id, 4000)
                                                                               || !FrostFever.TargetHaveBuff || ObjectManager.Target.AuraIsActiveAndExpireInLessThanMs(FrostFever.Id, 4000))
                     && UnholyBlight.IsSpellUsable && ObjectManager.Target.GetDistance < 9)
            {
                UnholyBlight.Cast();
                return;
            }

            if (MySettings.UseOutbreak && Outbreak.KnownSpell && NecroticPlague.KnownSpell && (!NecroticPlague.TargetHaveBuff || ObjectManager.Me.BuffStack(155159) < 15
                                                                                               || ObjectManager.Target.AuraIsActiveAndExpireInLessThanMs(NecroticPlague.Id, 4000)) && Outbreak.IsSpellUsable &&
                Outbreak.IsHostileDistanceGood)
            {
                Outbreak.Cast();
                return;
            }
            else if (MySettings.UseOutbreak && Outbreak.KnownSpell && (!BloodPlague.TargetHaveBuff || ObjectManager.Target.AuraIsActiveAndExpireInLessThanMs(BloodPlague.Id, 4000)
                                                                       || !FrostFever.TargetHaveBuff || ObjectManager.Target.AuraIsActiveAndExpireInLessThanMs(FrostFever.Id, 4000)) && Outbreak.IsSpellUsable &&
                     Outbreak.IsHostileDistanceGood)
            {
                Outbreak.Cast();
                return;
            }

            if (MySettings.UsePlagueStrike && NecroticPlague.KnownSpell && (!NecroticPlague.TargetHaveBuff || ObjectManager.Target.AuraIsActiveAndExpireInLessThanMs(NecroticPlague.Id, 4000))
                && PlagueStrike.IsSpellUsable && PlagueStrike.IsHostileDistanceGood)
            {
                PlagueStrike.Cast();
                return;
            }
            else if (MySettings.UsePlagueStrike && (!BloodPlague.TargetHaveBuff || ObjectManager.Target.AuraIsActiveAndExpireInLessThanMs(BloodPlague.Id, 4000))
                     && !Outbreak.IsSpellUsable && !UnholyBlight.IsSpellUsable && PlagueStrike.IsSpellUsable && PlagueStrike.IsHostileDistanceGood)
            {
                PlagueStrike.Cast();
                return;
            }

            if (MySettings.UseIcyTouch && NecroticPlague.KnownSpell && (!NecroticPlague.TargetHaveBuff || ObjectManager.Target.AuraIsActiveAndExpireInLessThanMs(NecroticPlague.Id, 4000))
                && IcyTouch.IsSpellUsable && IcyTouch.IsHostileDistanceGood)
            {
                IcyTouch.Cast();
                return;
            }
            else if (MySettings.UseIcyTouch && (!FrostFever.TargetHaveBuff || ObjectManager.Target.AuraIsActiveAndExpireInLessThanMs(FrostFever.Id, 4000))
                     && !Outbreak.IsSpellUsable && !UnholyBlight.IsSpellUsable && IcyTouch.IsSpellUsable && IcyTouch.IsHostileDistanceGood)
            {
                IcyTouch.Cast();
                return;
            }

            if (MySettings.UseDarkTransformation && ObjectManager.Me.BuffStack(91342) > 4 && DarkTransformation.IsSpellUsable)
            {
                DarkTransformation.Cast();
                return;
            }
            if (MySettings.UseGorefiendsGrasp && GorefiendsGrasp.IsSpellUsable && ObjectManager.GetUnitInSpellRange(GorefiendsGrasp.MaxRangeHostile) > 2)
            {
                GorefiendsGrasp.Cast();
                return;
            }
            if (MySettings.UseDeathandDecay && DeathandDecay.IsSpellUsable && ObjectManager.GetUnitInSpellRange(DeathandDecay.MaxRangeHostile) > 2)
            {
                SpellManager.CastSpellByIDAndPosition(43265, ObjectManager.Target.Position);
                return;
            }
            if (MySettings.UseArmyoftheDead && ArmyoftheDead.IsSpellUsable && ObjectManager.GetUnitInSpellRange(ArmyoftheDead.MaxRangeHostile) > 3)
            {
                ArmyoftheDead.Cast();
                return;
            }
            if (MySettings.UseBloodBoil && BloodBoil.IsSpellUsable && ObjectManager.GetUnitInSpellRange(BloodBoil.MaxRangeHostile) > 2)
            {
                BloodBoil.Cast();
                return;
            }
            if (MySettings.UseBreathofSindragosa && ObjectManager.Me.RunicPower > 75 && BreathofSindragosa.IsSpellUsable && ObjectManager.Target.GetDistance < 11)
            {
                BreathofSindragosa.Cast();
                return;
            }
            if (MySettings.UseSoulReaper && ObjectManager.Target.HealthPercent < 45
                && (ObjectManager.Me.HealthPercent > MySettings.UseDeathStrikeAtPercentage || !MySettings.UseDeathStrike)
                && SoulReaper.IsSpellUsable && SoulReaper.IsHostileDistanceGood)
            {
                SoulReaper.Cast();
                return;
            }
            if (MySettings.UseDefile && Defile.IsSpellUsable && Defile.IsHostileDistanceGood)
            {
                SpellManager.CastSpellByIDAndPosition(152280, ObjectManager.Target.Position);
                _defileTimer = new Timer(1000*30);
                return;
            }
            if (MySettings.UseScourgeStrike && ObjectManager.Me.RunicPowerPercentage < 90 && ScourgeStrike.IsSpellUsable && ScourgeStrike.IsHostileDistanceGood)
            {
                if (Defile.KnownSpell && _defileTimer.IsReady)
                    return;
                ScourgeStrike.Cast();
                return;
            }
            if ((!MySettings.UseScourgeStrike || !ScourgeStrike.KnownSpell) && MySettings.UsePlagueStrike && PlagueStrike.IsSpellUsable && PlagueStrike.IsHostileDistanceGood)
            {
                PlagueStrike.Cast();
                return;
            }
            if (MySettings.UseDeathCoil && (ObjectManager.Me.RunicPowerPercentage > 79 || SuddenDoom.HaveBuff
                                            || !ObjectManager.Pet.HaveBuff(63560)) && DeathCoil.IsSpellUsable && DeathCoil.IsHostileDistanceGood)
            {
                if (MySettings.UseBreathofSindragosa && BreathofSindragosa.KnownSpell && !BreathofSindragosa.IsSpellUsable)
                {
                    if ((MySettings.UseLichborne && ObjectManager.Me.HealthPercent <= MySettings.UseLichborneAtPercentage && Lichborne.IsSpellUsable)
                        || (MySettings.UseConversion && ObjectManager.Me.HealthPercent <= MySettings.UseConversionAtPercentage && Conversion.IsSpellUsable))
                        return;
                    DeathCoil.Cast();
                    return;
                }
                else if (!BreathofSindragosa.KnownSpell)
                {
                    if ((MySettings.UseLichborne && ObjectManager.Me.HealthPercent <= MySettings.UseLichborneAtPercentage && Lichborne.IsSpellUsable)
                        || (MySettings.UseConversion && ObjectManager.Me.HealthPercent <= MySettings.UseConversionAtPercentage && Conversion.IsSpellUsable))
                        return;
                    DeathCoil.Cast();
                    return;
                }
            }
            if (MySettings.UseFesteringStrike && ObjectManager.Me.RunicPowerPercentage < 90 && !ScourgeStrike.IsSpellUsable
                && FesteringStrike.IsSpellUsable && FesteringStrike.IsHostileDistanceGood)
            {
                if (MySettings.UseDeathStrike && ObjectManager.Me.HealthPercent <= MySettings.UseDeathStrikeAtPercentage && DeathStrike.IsSpellUsable)
                    return;
                FesteringStrike.Cast();
                return;
            }
            if (MySettings.UseDeathStrike && (!FesteringStrike.KnownSpell || !MySettings.UseFesteringStrike) && ObjectManager.Me.RunicPowerPercentage < 90 && !ScourgeStrike.IsSpellUsable
                && DeathStrike.IsSpellUsable && DeathStrike.IsHostileDistanceGood)
            {
                DeathStrike.Cast();
                return;
            }
            if (MySettings.UseArcaneTorrentForResource && ObjectManager.Me.RunicPowerPercentage < 85 && ArcaneTorrent.IsSpellUsable)
            {
                ArcaneTorrent.Cast();
                return;
            }
            if (MySettings.UseEmpowerRuneWeapon && ObjectManager.Me.RunicPowerPercentage < 75 && EmpowerRuneWeapon.IsSpellUsable)
                EmpowerRuneWeapon.Cast();
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    private void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            Buff();
            Heal();
        }
    }

    #region Nested type: DeathknightUnholySettings

    [Serializable]
    public class DeathknightUnholySettings : Settings
    {
        public bool DoAvoidMelee = false;
        public int DoAvoidMeleeDistance = 0;
        public bool UseAlchFlask = true;
        public bool UseAntiMagicShell = true;
        public int UseAntiMagicShellAtPercentage = 95;
        public bool UseAntiMagicZone = true;
        public int UseAntiMagicZoneAtPercentage = 95;
        public bool UseArcaneTorrentForDecast = true;
        public int UseArcaneTorrentForDecastAtPercentage = 95;
        public bool UseArcaneTorrentForResource = true;
        public bool UseArmyoftheDead = true;
        public bool UseAsphyxiate = true;
        public int UseAsphyxiateAtPercentage = 90;
        public bool UseBerserking = true;
        public bool UseBloodBoil = true;
        public bool UseBloodFury = true;
        public bool UseBloodPresence = true;
        public int UseBloodPresenceAtPercentage = 50;
        public bool UseBloodTapForDPS = true;
        public bool UseBloodTapForHeal = true;
        public bool UseBreathofSindragosa = true;
        public bool UseChainsofIce = false;
        public bool UseConversion = true;
        public int UseConversionAtPercentage = 45;
        public bool UseDarkflight = true;
        public bool UseDarkTransformation = true;
        public bool UseDeathCoil = true;
        public bool UseDeathGrip = true;
        public bool UseDeathPact = true;
        public int UseDeathPactAtPercentage = 55;
        public bool UseDeathSiphon = true;
        public int UseDeathSiphonAtPercentage = 80;
        public bool UseDeathStrike = true;
        public int UseDeathStrikeAtPercentage = 80;
        public bool UseDeathandDecay = true;
        public bool UseDefile = true;
        public bool UseDeathsAdvance = true;
        public bool UseEmpowerRuneWeapon = true;
        public bool UseEveryManforHimself = true;
        public bool UseFesteringStrike = true;
        public bool UseFrostPresence = true;
        public bool UseGiftoftheNaaru = true;
        public int UseGiftoftheNaaruAtPercentage = 80;
        public bool UseGorefiendsGrasp = true;
        public bool UseHornofWinter = true;
        public bool UseIceboundFortitude = true;
        public int UseIceboundFortitudeAtPercentage = 80;
        public bool UseIcyTouch = true;
        public bool UseLichborne = true;
        public int UseLichborneAtPercentage = 45;
        public bool UseLowCombat = true;
        public int UseLowCombatAtPercentage = 15;
        public bool UseMindFreeze = true;
        public int UseMindFreezeAtPercentage = 95;
        public bool UseOutbreak = true;
        public bool UsePathofFrost = true;
        public bool UsePlagueLeech = true;
        public bool UsePlagueStrike = true;
        public bool UseRaiseDead = true;
        public bool UseRemorselessWinter = true;
        public int UseRemorselessWinterAtPercentage = 70;
        public bool UseScourgeStrike = true;
        public bool UseSoulReaper = true;
        public bool UseStoneform = true;
        public int UseStoneformAtPercentage = 80;
        public bool UseStrangulate = true;
        public int UseStrangulateAtPercentage = 95;
        public bool UseSummonGargoyle = true;
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;
        public bool UseUnholyBlight = true;
        public bool UseUnholyPresence = true;
        public bool UseWarStomp = true;
        public int UseWarStompAtPercentage = 80;

        public DeathknightUnholySettings()
        {
            ConfigWinForm("Deathknight Unholy Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent for Interrupt", "UseArcaneTorrentForDecast", "Professions & Racials", "AtPercentage");
            AddControlInWinForm("Use Arcane Torrent for Resource", "UseArcaneTorrentForResource", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Darkflight", "UseDarkflight", "Professions & Racials");
            AddControlInWinForm("Use Every Man for Himself", "UseEveryManforHimself", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials", "AtPercentage");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials", "AtPercentage");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials", "AtPercentage");
            /* Deathknight Presence & Buffs */
            AddControlInWinForm("Use Frost Presence", "UseFrostPresence", "Deathknight Presence & Buffs");
            AddControlInWinForm("Use Blood Presence", "UseBloodPresence", "Deathknight Presence & Buffs", "AtPercentage");
            AddControlInWinForm("Use Horn of Winter", "UseHornofWinter", "Deathknight Presence & Buffs");
            AddControlInWinForm("Use Path of Frost", "UsePathofFrost", "Deathknight Presence & Buffs");
            AddControlInWinForm("Use Unholy Presence", "UseUnholyPresence", "Deathknight Presence & Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Blood Boil", "UseBloodBoil", "Offensive Spell");
            AddControlInWinForm("Use Dark Transformation", "UseDarkTransformation", "Offensive Spell");
            AddControlInWinForm("Use Death Coil", "UseDeathCoil", "Offensive Spell");
            AddControlInWinForm("Use Death and Decay", "UseDeathandDecay", "Offensive Spell");
            AddControlInWinForm("Use Defile", "UseDefile", "Offensive Spell");
            AddControlInWinForm("Use Festering Strike", "UseFesteringStrike", "Offensive Spell");
            AddControlInWinForm("Use Icy Touch", "UseIcyTouch", "Offensive Spell");
            AddControlInWinForm("Use Plague Leech", "UsePlagueLeech", "Offensive Spell");
            AddControlInWinForm("Use Plague Strike", "UsePlagueStrike", "Offensive Spell");
            AddControlInWinForm("Use Soul Reaper", "UseSoulReaper", "Offensive Spell");
            AddControlInWinForm("Use Scourge Strike", "UseScourgeStrike", "Offensive Spell");
            AddControlInWinForm("Use Unholy Blight", "UseUnholyBlight", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use Blood Tap for DPS", "UseBloodTapForDPS", "Offensive Cooldown");
            AddControlInWinForm("Use Blood Tap for Healing", "UseBloodTapForHeal", "Offensive Cooldown");
            AddControlInWinForm("Use Breath of Sindragosa", "UseBreathofSindragosa", "Offensive Cooldown");
            AddControlInWinForm("Use Death Grip", "UseDeathGrip", "Offensive Cooldown");
            AddControlInWinForm("Use Empower Rune Weapon", "UseEmpowerRuneWeapon", "Offensive Cooldown");
            AddControlInWinForm("Use Outbreak", "UseOutbreak", "Offensive Cooldown");
            AddControlInWinForm("Use Raise Dead", "UseRaiseDead", "Offensive Cooldown");
            AddControlInWinForm("Use Summon Gargoyle", "UseSummonGargoyle", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Anti-Magic Shell", "UseAntiMagicShell", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Anti-Magic Zone", "UseAntiMagicZone", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Army of the Dead", "UseArmyoftheDead", "Defensive Cooldown");
            AddControlInWinForm("Use Asphyxiate", "UseAsphyxiate", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Chains of Ice", "UseChainsofIce", "Defensive Cooldown");
            AddControlInWinForm("Use Death's Advance", "UseDeathsAdvance", "Defensive Cooldown");
            AddControlInWinForm("Use Gorefiend's Grasp", "UseGorefriendsGrasp", "Defensive Cooldown");
            AddControlInWinForm("Use Icebound Fortitude", "UseIceboundFortitude", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Mind Freeze", "UseMindFreeze", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Remorseless Winter", "UseRemorseless Winter", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Strangulate", "UseStrangulate", "Defensive Cooldown", "AtPercentage");
            /* Healing Spell */
            AddControlInWinForm("Use Conversion", "UseConversion", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Death Pact", "UseDeathPact", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Death Siphon", "UseDeathSiphon", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Death Strike", "UseDeathStrike", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Lichborne", "UseLichborne", "Healing Spell", "AtPercentage");
            /* Game Settings */
            AddControlInWinForm("Use Low Combat Settings - Level Difference", "UseLowCombat", "Game Settings", "AtPercentage");
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
            AddControlInWinForm("Do avoid melee (Off Advised!!)", "DoAvoidMelee", "Game Settings");
            AddControlInWinForm("Avoid melee distance (1 to 4)", "DoAvoidMeleeDistance", "Game Settings");
        }

        public static DeathknightUnholySettings CurrentSetting { get; set; }

        public static DeathknightUnholySettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Deathknight_Unholy.xml";
            if (File.Exists(currentSettingsFile))
            {
                return
                    CurrentSetting = Load<DeathknightUnholySettings>(currentSettingsFile);
            }
            return new DeathknightUnholySettings();
        }
    }

    #endregion
}

public class DeathknightFrost
{
    private static DeathknightFrostSettings MySettings = DeathknightFrostSettings.GetSettings();

    #region General Timers & Variables

    private readonly WoWItem _firstTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET);
    private readonly WoWItem _secondTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET, 2);
    public int DecastHP = 0;
    public int DefenseHP = 0;
    public int HealHP = 0;
    public int LC = 0;

    private Timer _onCd = new Timer(0);

    #endregion

    #region Professions & Racials

    public readonly Spell ArcaneTorrent = new Spell("Arcane Torrent");
    public readonly Spell Berserking = new Spell("Berserking");
    public readonly Spell BloodFury = new Spell("Blood Fury");
    public readonly Spell Darkflight = new Spell("Darkflight");
    public readonly Spell EveryManforHimself = new Spell("Every Man for Himself");
    public readonly Spell GiftoftheNaaru = new Spell("Gift of the Naaru");
    public readonly Spell Stoneform = new Spell("Stoneform");
    public readonly Spell WarStomp = new Spell("War Stomp");

    #endregion

    #region Deathknight Presence & Buffs

    public readonly Spell BloodPlague = new Spell(55078);
    public readonly Spell BloodPresence = new Spell("Blood Presence");
    public readonly Spell FreezingFog = new Spell(59052);
    public readonly Spell FrostFever = new Spell(55095);
    public readonly Spell FrostPresence = new Spell("Frost Presence");
    public readonly Spell HornofWinter = new Spell("Horn of Winter");
    public readonly Spell KillingMachine = new Spell(51124);
    public readonly Spell PathofFrost = new Spell("Path of Frost");
    public readonly uint PathofFrostBuff = 3714;
    public readonly Spell NecroticPlague = new Spell(152281);
    public readonly Spell UnholyPresence = new Spell("Unholy Presence");
    private Timer _pathofFrostBuffTimer = new Timer(0);

    #endregion

    #region Offensive Spell

    public readonly Spell BloodBoil = new Spell("Blood Boil");
    public readonly Spell BloodStrike = new Spell("Blood Strike");
    public readonly Spell DeathCoil = new Spell("Death Coil");
    public readonly Spell DeathandDecay = new Spell("Death and Decay");
    public readonly Spell FrostStrike = new Spell("Frost Strike");
    public readonly Spell HowlingBlast = new Spell("Howling Blast");
    public readonly Spell IcyTouch = new Spell("Icy Touch");
    public readonly Spell Obliterate = new Spell("Obliterate");
    public readonly Spell PlagueLeech = new Spell("Plague Leech");
    public readonly Spell PlagueStrike = new Spell("Plague Strike");
    public readonly Spell SoulReaper = new Spell("Soul Reaper");
    public readonly Spell UnholyBlight = new Spell("Unholy Blight");
    public readonly Spell Defile = new Spell("Defile");
    private Timer _defileTimer = new Timer(0);

    #endregion

    #region Offensive Cooldown

    public readonly Spell BloodTap = new Spell("Blood Tap");
    public readonly Spell BloodCharge = new Spell(114851);
    public readonly Spell DeathGrip = new Spell("Death Grip");
    public readonly Spell EmpowerRuneWeapon = new Spell("Empower Rune Weapon");
    public readonly Spell Outbreak = new Spell("Outbreak");
    public readonly Spell PillarofFrost = new Spell("Pillar of Frost");

    #endregion

    #region Defensive Cooldown

    public readonly Spell AntiMagicShell = new Spell("Anti-Magic Shell");
    public readonly Spell AntiMagicZone = new Spell("Anti-Magic Zone");
    public readonly Spell ArmyoftheDead = new Spell("Army of the Dead");
    public readonly Spell Asphyxiate = new Spell("Asphyxiate");
    public readonly Spell ChainsofIce = new Spell("Chains of Ice");
    public readonly Spell DeathsAdvance = new Spell("Death's Advance");
    public readonly Spell DesecratedGround = new Spell("Desecrated Ground");
    public readonly Spell GorefiendsGrasp = new Spell("Gorefriend's Grasp");
    public readonly Spell IceboundFortitude = new Spell("Icebound Fortitude");
    public readonly Spell MindFreeze = new Spell("Mind Freeze");
    public readonly Spell RemorselessWinter = new Spell("Remorseless Winter");
    public readonly Spell Strangulate = new Spell("Strangulate");

    #endregion

    #region Healing Spell

    public readonly Spell Conversion = new Spell("Conversion");
    public readonly Spell DeathPact = new Spell("Death Pact");
    public readonly Spell DeathSiphon = new Spell("Death Siphon");
    public readonly Spell DeathStrike = new Spell("Death Strike");
    public readonly Spell Lichborne = new Spell("Lichborne");

    #endregion

    public DeathknightFrost()
    {
        Main.InternalRange = ObjectManager.Me.GetCombatReach;
        MySettings = DeathknightFrostSettings.GetSettings();
        Main.DumpCurrentSettings<DeathknightFrostSettings>(MySettings);
        UInt128 lastTarget = 0;
        LowHP();

        while (Main.InternalLoop)
        {
            try
            {
                if (!ObjectManager.Me.IsDeadMe)
                {
                    BuffPath();
                    if (!ObjectManager.Me.IsMounted)
                    {
                        if (Fight.InFight && ObjectManager.Me.Target > 0)
                        {
                            if (ObjectManager.Me.Target != lastTarget && (DeathGrip.IsHostileDistanceGood || IcyTouch.IsHostileDistanceGood))
                            {
                                Pull();
                                lastTarget = ObjectManager.Me.Target;
                            }

                            if (MySettings.UseLowCombat && (ObjectManager.Me.Level - ObjectManager.Target.Level >= MySettings.UseLowCombatAtPercentage))
                            {
                                LC = 1;
                                if (ObjectManager.Target.GetDistance < 30)
                                    LowCombat();
                            }
                            else
                            {
                                LC = 0;
                                if (ObjectManager.Target.GetDistance < 30)
                                    Combat();
                            }
                        }
                        if (!ObjectManager.Me.IsCast)
                            Patrolling();
                    }
                }
                else
                    Thread.Sleep(500);
            }
            catch
            {
            }
            Thread.Sleep(100);
        }
    }

    private void LowHP()
    {
        if (MySettings.UseAsphyxiateAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseAsphyxiateAtPercentage;

        if (MySettings.UseIceboundFortitudeAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseIceboundFortitudeAtPercentage;

        if (MySettings.UseRemorselessWinterAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseRemorselessWinterAtPercentage;

        if (MySettings.UseStoneformAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseStoneformAtPercentage;

        if (MySettings.UseWarStompAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseWarStompAtPercentage;

        if (MySettings.UseConversionAtPercentage > HealHP)
            HealHP = MySettings.UseConversionAtPercentage;

        if (MySettings.UseDeathPactAtPercentage > HealHP)
            HealHP = MySettings.UseDeathPactAtPercentage;

        if (MySettings.UseDeathSiphonAtPercentage > HealHP)
            HealHP = MySettings.UseDeathSiphonAtPercentage;

        if (MySettings.UseDeathStrikeAtPercentage > HealHP)
            HealHP = MySettings.UseDeathStrikeAtPercentage;

        if (MySettings.UseGiftoftheNaaruAtPercentage > HealHP)
            HealHP = MySettings.UseGiftoftheNaaruAtPercentage;

        if (MySettings.UseLichborneAtPercentage > HealHP)
            HealHP = MySettings.UseLichborneAtPercentage;

        if (MySettings.UseAntiMagicShellAtPercentage > DecastHP)
            DecastHP = MySettings.UseAntiMagicShellAtPercentage;

        if (MySettings.UseAntiMagicZoneAtPercentage > DecastHP)
            DecastHP = MySettings.UseAntiMagicZoneAtPercentage;

        if (MySettings.UseArcaneTorrentForDecastAtPercentage > DecastHP)
            DecastHP = MySettings.UseArcaneTorrentForDecastAtPercentage;

        if (MySettings.UseAsphyxiateAtPercentage > DecastHP)
            DecastHP = MySettings.UseAsphyxiateAtPercentage;

        if (MySettings.UseMindFreezeAtPercentage > DecastHP)
            DecastHP = MySettings.UseMindFreezeAtPercentage;

        if (MySettings.UseStrangulateAtPercentage > DecastHP)
            DecastHP = MySettings.UseStrangulateAtPercentage;
    }

    private void BuffPath()
    {
        if (MySettings.UsePathofFrost && !ObjectManager.Me.InCombat && _pathofFrostBuffTimer.IsReady
            && (!PathofFrost.HaveBuff || ObjectManager.Me.AuraIsActiveAndExpireInLessThanMs(PathofFrostBuff, 10000)) && PathofFrost.IsSpellUsable)
        {
            PathofFrost.Cast();
            _pathofFrostBuffTimer = new Timer(1000*10);
        }
    }

    private void Pull()
    {
        if (MySettings.UseDeathGrip && DeathGrip.IsSpellUsable && ObjectManager.Target.GetDistance > Main.InternalRange && DeathGrip.IsHostileDistanceGood)
        {
            DeathGrip.Cast();
            MovementManager.StopMove();
            return;
        }
        if (MySettings.UseIcyTouch && IcyTouch.IsSpellUsable && IcyTouch.IsHostileDistanceGood)
            IcyTouch.Cast();
    }

    private void LowCombat()
    {
        Buff();

        if (MySettings.DoAvoidMelee)
            AvoidMelee();

        if (_onCd.IsReady && ObjectManager.Me.HealthPercent <= DefenseHP)
            DefenseCycle();

        if (ObjectManager.Me.HealthPercent <= HealHP)
            Heal();

        if (MySettings.UseHowlingBlast && HowlingBlast.IsSpellUsable && HowlingBlast.IsHostileDistanceGood)
        {
            HowlingBlast.Cast();
            return;
        }
        if (MySettings.UseFrostStrike && FrostStrike.IsSpellUsable && FrostStrike.IsHostileDistanceGood)
        {
            FrostStrike.Cast();
            return;
        }
        if (MySettings.UseDeathCoil && DeathCoil.IsSpellUsable && DeathCoil.IsHostileDistanceGood)
        {
            DeathCoil.Cast();
            return;
        }
        if (MySettings.UsePlagueStrike && PlagueStrike.IsSpellUsable && PlagueStrike.IsHostileDistanceGood)
        {
            PlagueStrike.Cast();
            return;
        }
        if (MySettings.UseBloodBoil && BloodBoil.IsSpellUsable && BloodBoil.IsHostileDistanceGood)
            BloodBoil.Cast();
    }

    private void Combat()
    {
        Buff();

        if (MySettings.DoAvoidMelee)
            AvoidMelee();

        DPSCycle();

        if (_onCd.IsReady && ObjectManager.Me.HealthPercent <= DefenseHP)
            DefenseCycle();

        if (ObjectManager.Me.HealthPercent <= HealHP)
            Heal();

        if (ObjectManager.Me.HealthPercent <= DecastHP || (MySettings.UseChainsofIce && ObjectManager.Target.GetMove))
            Decast();

        DPSBurst();
        DPSCycle();
    }

    private void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (MySettings.UseBloodPresence && !BloodPresence.HaveBuff && ObjectManager.Me.HealthPercent <= MySettings.UseBloodPresenceAtPercentage
            && BloodPresence.IsSpellUsable)
            BloodPresence.Cast();

        if (MySettings.UseDarkflight && Darkflight.KnownSpell && !DeathsAdvance.HaveBuff && ObjectManager.Me.GetMove && !ObjectManager.Target.InCombat && Darkflight.IsSpellUsable)
            Darkflight.Cast();

        if (MySettings.UseDeathsAdvance && !Darkflight.HaveBuff && ObjectManager.Me.GetMove && !ObjectManager.Target.InCombat && DeathsAdvance.IsSpellUsable)
            DeathsAdvance.Cast();

        if (MySettings.UseFrostPresence && LC != 1 && !FrostPresence.HaveBuff
            && (ObjectManager.Me.HealthPercent > MySettings.UseBloodPresenceAtPercentage + 10 || !MySettings.UseBloodPresence) && FrostPresence.IsSpellUsable)
            FrostPresence.Cast();

        if (MySettings.UseHornofWinter && !HornofWinter.HaveBuff && HornofWinter.IsSpellUsable)
            HornofWinter.Cast();

        if (MySettings.UseUnholyPresence && MySettings.UseLowCombat && !UnholyPresence.HaveBuff && LC == 1
            && (ObjectManager.Me.HealthPercent > MySettings.UseBloodPresenceAtPercentage + 10 || !MySettings.UseBloodPresence) && UnholyPresence.IsSpellUsable)
            UnholyPresence.Cast();

        if (MySettings.UseAlchFlask && ItemsManager.GetItemCount(75525) > 0 && !ObjectManager.Me.HaveBuff(79638) && !ObjectManager.Me.HaveBuff(79640)
            && !ObjectManager.Me.HaveBuff(79639) && !ItemsManager.IsItemOnCooldown(75525))
            ItemsManager.UseItem(75525);
    }

    private void AvoidMelee()
    {
        if (ObjectManager.Target.GetDistance < MySettings.DoAvoidMeleeDistance && ObjectManager.Target.InCombat)
        {
            Logging.WriteFight("Too Close. Moving Back");
            var maxTimeTimer = new Timer(1000*2);
            MovementsAction.MoveBackward(true);
            while (ObjectManager.Target.GetDistance < 2 && ObjectManager.Target.InCombat && !maxTimeTimer.IsReady)
            {
                Others.SafeSleep(300);
            }
            MovementsAction.MoveBackward(false);
            if (maxTimeTimer.IsReady && ObjectManager.Target.GetDistance < 2 && ObjectManager.Target.InCombat)
            {
                MovementsAction.MoveForward(true);
                Others.SafeSleep(1000);
                MovementsAction.MoveForward(false);
                MovementManager.Face(ObjectManager.Target.Position);
            }
        }
    }

    private void DefenseCycle()
    {
        if (MySettings.UseEveryManforHimself && ObjectManager.Me.IsStunned && EveryManforHimself.IsSpellUsable)
            EveryManforHimself.Cast();

        if (MySettings.UseAsphyxiate && ObjectManager.Me.HealthPercent <= MySettings.UseAsphyxiateAtPercentage && Asphyxiate.IsSpellUsable
            && Asphyxiate.IsHostileDistanceGood)
        {
            Asphyxiate.Cast();
            _onCd = new Timer(1000*5);
            return;
        }
        if (MySettings.UseIceboundFortitude && ObjectManager.Me.HealthPercent <= MySettings.UseIceboundFortitudeAtPercentage
            && IceboundFortitude.IsSpellUsable)
        {
            IceboundFortitude.Cast();
            _onCd = new Timer(1000*12);
            return;
        }
        if (MySettings.UseRemorselessWinter && (ObjectManager.Me.HealthPercent <= MySettings.UseRemorselessWinterAtPercentage
                                                || ObjectManager.GetUnitInSpellRange(RemorselessWinter.MaxRangeHostile) > 1) && RemorselessWinter.IsSpellUsable)
        {
            RemorselessWinter.Cast();
            _onCd = new Timer(1000*8);
            return;
        }
        if (MySettings.UseStoneform && ObjectManager.Me.HealthPercent <= MySettings.UseStoneformAtPercentage && Stoneform.IsSpellUsable)
        {
            Stoneform.Cast();
            _onCd = new Timer(1000*8);
            return;
        }
        if (MySettings.UseWarStomp && ObjectManager.Me.HealthPercent <= MySettings.UseWarStompAtPercentage && WarStomp.IsSpellUsable
            && ObjectManager.GetUnitInSpellRange(WarStomp.MaxRangeHostile) > 0)
        {
            WarStomp.Cast();
            _onCd = new Timer(1000*2);
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (MySettings.UseBloodTapForHeal && BloodTap.IsSpellUsable && BloodCharge.BuffStack > 4 && ObjectManager.Me.HealthPercent <= HealHP && ObjectManager.Target.GetDistance < 30)
            BloodTap.Cast();

        if (MySettings.UseConversion && ObjectManager.Me.RunicPower > 10 && ObjectManager.Me.HealthPercent <= MySettings.UseConversionAtPercentage
            && Conversion.IsSpellUsable)
        {
            Conversion.Cast();
            while (ObjectManager.Me.IsCast && (ObjectManager.Me.RunicPower > 4 || ObjectManager.Me.HealthPercent < 100))
                Others.SafeSleep(200);
            return;
        }
        if (MySettings.UseDeathPact && ObjectManager.Me.HealthPercent <= MySettings.UseDeathPactAtPercentage && DeathPact.IsSpellUsable)
        {
            DeathPact.Cast();
            return;
        }
        if (MySettings.UseDeathSiphon && ObjectManager.Me.HealthPercent <= MySettings.UseDeathSiphonAtPercentage && DeathSiphon.IsSpellUsable && DeathSiphon.IsHostileDistanceGood)
        {
            DeathSiphon.Cast();
            return;
        }
        if (MySettings.UseDeathStrike && ObjectManager.Target.IsValid && ObjectManager.Target.IsAlive
            && ObjectManager.Me.HealthPercent <= MySettings.UseDeathStrikeAtPercentage && DeathStrike.IsSpellUsable && DeathStrike.IsHostileDistanceGood)
        {
            DeathStrike.Cast();
            return;
        }
        if (MySettings.UseGiftoftheNaaru && ObjectManager.Me.HealthPercent <= MySettings.UseGiftoftheNaaruAtPercentage && GiftoftheNaaru.IsSpellUsable)
        {
            GiftoftheNaaru.Cast();
            return;
        }
        if (MySettings.UseLichborne && ObjectManager.Me.HealthPercent <= MySettings.UseLichborneAtPercentage && ObjectManager.Me.RunicPower > 39 && Lichborne.IsSpellUsable)
            Lichborne.Cast();
    }

    private void Decast()
    {
        if (MySettings.UseArcaneTorrentForDecast && ObjectManager.Me.HealthPercent <= MySettings.UseArcaneTorrentForDecastAtPercentage
            && ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && ArcaneTorrent.IsSpellUsable && ObjectManager.Target.GetDistance < 8)
        {
            ArcaneTorrent.Cast();
            return;
        }
        if (MySettings.UseAntiMagicShell && ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe
            && ObjectManager.Me.HealthPercent <= MySettings.UseAntiMagicShellAtPercentage && AntiMagicShell.IsSpellUsable)
        {
            AntiMagicShell.Cast();
            return;
        }
        if (MySettings.UseAntiMagicZone && ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe
            && ObjectManager.Me.HealthPercent <= MySettings.UseAntiMagicZoneAtPercentage && AntiMagicZone.IsSpellUsable)
        {
            SpellManager.CastSpellByIDAndPosition(51052, ObjectManager.Me.Position);
            return;
        }
        if (MySettings.UseAsphyxiate && ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe
            && ObjectManager.Me.HealthPercent <= MySettings.UseAsphyxiateAtPercentage && Asphyxiate.IsSpellUsable && Asphyxiate.IsHostileDistanceGood)
        {
            Asphyxiate.Cast();
            return;
        }
        if (MySettings.UseMindFreeze && ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe
            && ObjectManager.Me.HealthPercent <= MySettings.UseMindFreezeAtPercentage && MindFreeze.IsSpellUsable && MindFreeze.IsHostileDistanceGood)
        {
            MindFreeze.Cast();
            return;
        }
        if (MySettings.UseStrangulate && ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && (MySettings.UseStrangulate && ObjectManager.Me.HealthPercent <= MySettings.UseStrangulateAtPercentage
                                                                                                               || MySettings.UseAsphyxiate && ObjectManager.Me.HealthPercent <= MySettings.UseAsphyxiateAtPercentage) &&
            Strangulate.IsSpellUsable && Strangulate.IsHostileDistanceGood)
        {
            Strangulate.Cast();
            return;
        }
        if (MySettings.UseChainsofIce && ObjectManager.Target.GetMove && !ChainsofIce.TargetHaveBuff && ChainsofIce.IsHostileDistanceGood
            && ChainsofIce.IsSpellUsable)
            ChainsofIce.Cast();
    }

    private void DPSBurst()
    {
        if (MySettings.UseTrinketOne && !ItemsManager.IsItemOnCooldown(_firstTrinket.Entry) && ItemsManager.IsItemUsable(_firstTrinket.Entry))
        {
            ItemsManager.UseItem(_firstTrinket.Name);
            Logging.WriteFight("Use First Trinket Slot");
        }
        if (MySettings.UseTrinketTwo && !ItemsManager.IsItemOnCooldown(_secondTrinket.Entry) && ItemsManager.IsItemUsable(_secondTrinket.Entry))
        {
            ItemsManager.UseItem(_secondTrinket.Name);
            Logging.WriteFight("Use Second Trinket Slot");
        }
        if (MySettings.UseBerserking && Berserking.IsSpellUsable && ObjectManager.Target.GetDistance < 30)
            Berserking.Cast();

        if (MySettings.UseBloodFury && BloodFury.IsSpellUsable && ObjectManager.Target.GetDistance < 30)
            BloodFury.Cast();

        if (MySettings.UseBloodTapForDPS && BloodTap.IsSpellUsable && BloodCharge.BuffStack > 9 && ObjectManager.Target.GetDistance < 30)
            BloodTap.Cast();

        if (MySettings.UsePillarofFrost && PillarofFrost.IsSpellUsable && ObjectManager.Target.GetDistance < 30)
            PillarofFrost.Cast();
    }

    private void DPSCycle()
    {
        Usefuls.SleepGlobalCooldown();
        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            if (Lichborne.HaveBuff && ObjectManager.Me.HealthPercent < 85 && DeathCoil.IsSpellUsable)
            {
                Lua.RunMacroText("/target Player");
                DeathCoil.Cast();
                return;
            }
            if (MySettings.UsePlagueLeech && MySettings.UseOutbreak && !NecroticPlague.KnownSpell && BloodPlague.TargetHaveBuff
                && FrostFever.TargetHaveBuff && Outbreak.IsSpellUsable && PlagueLeech.IsSpellUsable && !Obliterate.IsSpellUsable && PlagueLeech.IsHostileDistanceGood)
            {
                PlagueLeech.Cast();
                Others.SafeSleep(1000);
                if (!BloodPlague.TargetHaveBuff && Outbreak.IsSpellUsable && Outbreak.IsHostileDistanceGood)
                    Outbreak.Cast();
                return;
            }

            if (MySettings.UseUnholyBlight && UnholyBlight.KnownSpell && NecroticPlague.KnownSpell &&
                (!NecroticPlague.TargetHaveBuff || ObjectManager.Target.AuraIsActiveAndExpireInLessThanMs(NecroticPlague.Id, 4000))
                && UnholyBlight.IsSpellUsable && ObjectManager.Target.GetDistance < 9)
            {
                UnholyBlight.Cast();
                return;
            }
            else if (MySettings.UseUnholyBlight && UnholyBlight.KnownSpell && (!BloodPlague.TargetHaveBuff || ObjectManager.Target.AuraIsActiveAndExpireInLessThanMs(BloodPlague.Id, 4000)
                                                                               || !FrostFever.TargetHaveBuff || ObjectManager.Target.AuraIsActiveAndExpireInLessThanMs(FrostFever.Id, 4000)) &&
                     UnholyBlight.IsSpellUsable && ObjectManager.Target.GetDistance < 9)
            {
                UnholyBlight.Cast();
                return;
            }

            if (MySettings.UseOutbreak && Outbreak.KnownSpell && NecroticPlague.KnownSpell && (!NecroticPlague.TargetHaveBuff || ObjectManager.Target.AuraIsActiveAndExpireInLessThanMs(NecroticPlague.Id, 4000))
                && Outbreak.IsSpellUsable && Outbreak.IsHostileDistanceGood)
            {
                Outbreak.Cast();
                return;
            }
            else if (MySettings.UseOutbreak && Outbreak.KnownSpell && (!BloodPlague.TargetHaveBuff || ObjectManager.Target.AuraIsActiveAndExpireInLessThanMs(BloodPlague.Id, 4000)
                                                                       || !FrostFever.TargetHaveBuff || ObjectManager.Target.AuraIsActiveAndExpireInLessThanMs(FrostFever.Id, 4000)) && Outbreak.IsSpellUsable &&
                     Outbreak.IsHostileDistanceGood)
            {
                Outbreak.Cast();
                return;
            }

            if (MySettings.UsePlagueStrike && NecroticPlague.KnownSpell && (!NecroticPlague.TargetHaveBuff || ObjectManager.Target.AuraIsActiveAndExpireInLessThanMs(NecroticPlague.Id, 4000))
                && PlagueStrike.IsSpellUsable && PlagueStrike.IsHostileDistanceGood)
            {
                PlagueStrike.Cast();
                return;
            }
            else if (MySettings.UsePlagueStrike && (!BloodPlague.TargetHaveBuff || ObjectManager.Target.AuraIsActiveAndExpireInLessThanMs(BloodPlague.Id, 4000))
                     && !Outbreak.IsSpellUsable && !UnholyBlight.IsSpellUsable && PlagueStrike.IsSpellUsable && PlagueStrike.IsHostileDistanceGood)
            {
                PlagueStrike.Cast();
                return;
            }

            if (MySettings.UseHowlingBlast && NecroticPlague.KnownSpell && (!NecroticPlague.TargetHaveBuff || ObjectManager.Target.AuraIsActiveAndExpireInLessThanMs(NecroticPlague.Id, 4000))
                && HowlingBlast.IsSpellUsable && HowlingBlast.IsHostileDistanceGood)
            {
                HowlingBlast.Cast();
                return;
            }
            else if (MySettings.UseHowlingBlast && (!FrostFever.TargetHaveBuff || ObjectManager.Target.AuraIsActiveAndExpireInLessThanMs(FrostFever.Id, 4000))
                     && !Outbreak.IsSpellUsable && !UnholyBlight.IsSpellUsable && HowlingBlast.IsSpellUsable && HowlingBlast.IsHostileDistanceGood)
            {
                HowlingBlast.Cast();
                return;
            }

            if (MySettings.UseIcyTouch && !MySettings.UseHowlingBlast && NecroticPlague.KnownSpell &&
                (!NecroticPlague.TargetHaveBuff || ObjectManager.Target.AuraIsActiveAndExpireInLessThanMs(NecroticPlague.Id, 4000))
                && IcyTouch.IsSpellUsable && IcyTouch.IsHostileDistanceGood)
            {
                IcyTouch.Cast();
                return;
            }
            else if (MySettings.UseIcyTouch && !MySettings.UseHowlingBlast && (!FrostFever.TargetHaveBuff || ObjectManager.Target.AuraIsActiveAndExpireInLessThanMs(FrostFever.Id, 4000))
                     && !Outbreak.IsSpellUsable && !UnholyBlight.IsSpellUsable && IcyTouch.IsSpellUsable && IcyTouch.IsHostileDistanceGood)
            {
                IcyTouch.Cast();
                return;
            }

            if (MySettings.UseGorefiendsGrasp && GorefiendsGrasp.IsSpellUsable && ObjectManager.GetUnitInSpellRange(GorefiendsGrasp.MaxRangeHostile) > 2)
            {
                GorefiendsGrasp.Cast();
                return;
            }
            if ((ObjectManager.GetUnitInSpellRange() > 2 && MySettings.UseDualWield) || (ObjectManager.GetUnitInSpellRange() > 3 && MySettings.UseTwoHander))
            {
                if (MySettings.UseDeathandDecay && NecroticPlague.KnownSpell && NecroticPlague.TargetHaveBuff && DeathandDecay.IsSpellUsable
                    && ObjectManager.GetUnitInSpellRange(DeathandDecay.MaxRangeHostile) > 2)
                {
                    SpellManager.CastSpellByIDAndPosition(43265, ObjectManager.Target.Position);
                    return;
                }
                if (MySettings.UseArmyoftheDead && ArmyoftheDead.IsSpellUsable && ObjectManager.GetUnitInSpellRange(ArmyoftheDead.MaxRangeHostile) > 3)
                {
                    ArmyoftheDead.Cast();
                    Others.SafeSleep(4000);
                    return;
                }
                if (MySettings.UseBloodBoil && BloodPlague.TargetHaveBuff && ObjectManager.Target.AuraIsActiveAndExpireInLessThanMs(BloodPlague.Id, 4000) && BloodBoil.IsSpellUsable
                    && BloodBoil.IsHostileDistanceGood)
                {
                    BloodBoil.Cast();
                    return;
                }
                if (MySettings.UseHowlingBlast && HowlingBlast.IsSpellUsable && HowlingBlast.IsHostileDistanceGood)
                {
                    HowlingBlast.Cast();
                    return;
                }
                if (MySettings.UseFrostStrike && FrostStrike.IsSpellUsable && FrostStrike.IsHostileDistanceGood)
                {
                    if ((MySettings.UseLichborne && ObjectManager.Me.HealthPercent <= MySettings.UseLichborneAtPercentage && Lichborne.IsSpellUsable)
                        || (MySettings.UseConversion && ObjectManager.Me.HealthPercent <= MySettings.UseConversionAtPercentage && Conversion.IsSpellUsable))
                        return;
                    FrostStrike.Cast();
                    return;
                }
            }

            if (MySettings.UseSoulReaper && ObjectManager.Target.HealthPercent < 35
                && (ObjectManager.Me.HealthPercent > MySettings.UseDeathStrikeAtPercentage || !MySettings.UseDeathStrike)
                && SoulReaper.IsSpellUsable && SoulReaper.IsHostileDistanceGood)
            {
                SoulReaper.Cast();
                return;
            }

            if (MySettings.UseDualWield)
            {
                if (MySettings.UseFrostStrike && (KillingMachine.HaveBuff || ObjectManager.Me.RunicPower > 88)
                    && FrostStrike.IsSpellUsable && FrostStrike.IsHostileDistanceGood)
                {
                    if ((MySettings.UseLichborne && ObjectManager.Me.HealthPercent <= MySettings.UseLichborneAtPercentage && Lichborne.IsSpellUsable)
                        || (MySettings.UseConversion && ObjectManager.Me.HealthPercent <= MySettings.UseConversionAtPercentage && Conversion.IsSpellUsable))
                        return;
                    FrostStrike.Cast();
                    return;
                }
                if (MySettings.UseDefile && Defile.IsSpellUsable && Defile.IsHostileDistanceGood)
                {
                    SpellManager.CastSpellByIDAndPosition(152280, ObjectManager.Target.Position);
                    _defileTimer = new Timer(1000*30);
                    return;
                }
                if (MySettings.UseObliterate && KillingMachine.HaveBuff && ObjectManager.Me.RunicPower < 40 && Obliterate.IsSpellUsable
                    && Obliterate.IsHostileDistanceGood)
                {
                    if (Defile.KnownSpell && _defileTimer.IsReady)
                        return;
                    if (MySettings.UseDeathStrike && ObjectManager.Me.HealthPercent <= MySettings.UseDeathStrikeAtPercentage && DeathStrike.IsSpellUsable)
                        return;
                    Obliterate.Cast();
                    return;
                }
                if (MySettings.UseHowlingBlast && ObjectManager.Me.RunicPowerPercentage < 90 && HowlingBlast.IsSpellUsable && HowlingBlast.IsHostileDistanceGood)
                {
                    HowlingBlast.Cast();
                    return;
                }
                if (!MySettings.UseHowlingBlast && MySettings.UseIcyTouch && ObjectManager.Me.RunicPowerPercentage < 90 && IcyTouch.IsSpellUsable && IcyTouch.IsHostileDistanceGood)
                {
                    IcyTouch.Cast();
                    return;
                }
            }

            if (MySettings.UseTwoHander)
            {
                if (MySettings.UseDefile && Defile.IsSpellUsable && Defile.IsHostileDistanceGood)
                {
                    SpellManager.CastSpellByIDAndPosition(152280, ObjectManager.Target.Position);
                    _defileTimer = new Timer(1000*30);
                    return;
                }
                if (MySettings.UseObliterate && KillingMachine.HaveBuff && Obliterate.IsSpellUsable && Obliterate.IsHostileDistanceGood)
                {
                    if (Defile.KnownSpell && _defileTimer.IsReady)
                        return;
                    else if (MySettings.UseDeathStrike && ObjectManager.Me.HealthPercent <= MySettings.UseDeathStrikeAtPercentage && DeathStrike.IsSpellUsable)
                        return;
                    Obliterate.Cast();
                    return;
                }
                if (MySettings.UseHowlingBlast && FreezingFog.HaveBuff && ObjectManager.Me.RunicPowerPercentage < 90
                    && HowlingBlast.IsSpellUsable && HowlingBlast.IsHostileDistanceGood)
                {
                    HowlingBlast.Cast();
                    return;
                }
                if (!MySettings.UseHowlingBlast && MySettings.UseIcyTouch && ObjectManager.Me.RunicPowerPercentage < 90 && IcyTouch.IsSpellUsable && IcyTouch.IsHostileDistanceGood)
                {
                    IcyTouch.Cast();
                    return;
                }
                if (MySettings.UseFrostStrike && (KillingMachine.HaveBuff || ObjectManager.Me.RunicPower > 76 || !Obliterate.IsSpellUsable)
                    && FrostStrike.IsSpellUsable && FrostStrike.IsHostileDistanceGood)
                {
                    if ((MySettings.UseLichborne && ObjectManager.Me.HealthPercent <= MySettings.UseLichborneAtPercentage && Lichborne.IsSpellUsable)
                        || (MySettings.UseConversion && ObjectManager.Me.HealthPercent <= MySettings.UseConversionAtPercentage && Conversion.IsSpellUsable))
                        return;
                    FrostStrike.Cast();
                    return;
                }
                if (MySettings.UseObliterate && Obliterate.IsSpellUsable && Obliterate.IsHostileDistanceGood)
                {
                    if (Defile.KnownSpell && _defileTimer.IsReady)
                        return;
                    else if (MySettings.UseDeathStrike && ObjectManager.Me.HealthPercent <= MySettings.UseDeathStrikeAtPercentage && DeathStrike.IsSpellUsable)
                        return;
                    Obliterate.Cast();
                    return;
                }
            }
            if (MySettings.UseArcaneTorrentForResource && ObjectManager.Me.RunicPowerPercentage < 85 && ArcaneTorrent.IsSpellUsable)
            {
                ArcaneTorrent.Cast();
                return;
            }
            if (MySettings.UseEmpowerRuneWeapon && ObjectManager.Me.RunicPowerPercentage < 75 && EmpowerRuneWeapon.IsSpellUsable)
                EmpowerRuneWeapon.Cast();
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    private void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            Buff();
            Heal();
        }
    }

    #region Nested type: DeathknightFrostSettings

    [Serializable]
    public class DeathknightFrostSettings : Settings
    {
        public bool DoAvoidMelee = false;
        public int DoAvoidMeleeDistance = 0;
        public bool UseAlchFlask = true;
        public bool UseAntiMagicShell = true;
        public int UseAntiMagicShellAtPercentage = 95;
        public bool UseAntiMagicZone = true;
        public int UseAntiMagicZoneAtPercentage = 95;
        public bool UseArcaneTorrentForDecast = true;
        public int UseArcaneTorrentForDecastAtPercentage = 95;
        public bool UseArcaneTorrentForResource = true;
        public bool UseArmyoftheDead = true;
        public bool UseAsphyxiate = true;
        public int UseAsphyxiateAtPercentage = 90;
        public bool UseBerserking = true;
        public bool UseBloodBoil = true;
        public bool UseBloodFury = true;
        public bool UseBloodPresence = true;
        public int UseBloodPresenceAtPercentage = 50;
        public bool UseBloodTapForDPS = true;
        public bool UseBloodTapForHeal = true;
        public bool UseChainsofIce = false;
        public bool UseConversion = true;
        public int UseConversionAtPercentage = 45;
        public bool UseDarkflight = true;
        public bool UseDeathCoil = true;
        public bool UseDeathGrip = true;
        public bool UseDeathPact = true;
        public int UseDeathPactAtPercentage = 55;
        public bool UseDeathSiphon = true;
        public int UseDeathSiphonAtPercentage = 80;
        public bool UseDeathStrike = true;
        public int UseDeathStrikeAtPercentage = 80;
        public bool UseDeathandDecay = true;
        public bool UseDefile = true;
        public bool UseDeathsAdvance = true;
        public bool UseDualWield = false;
        public bool UseEmpowerRuneWeapon = true;
        public bool UseEveryManforHimself = true;
        public bool UseFrostPresence = true;
        public bool UseFrostStrike = true;
        public bool UseGiftoftheNaaru = true;
        public int UseGiftoftheNaaruAtPercentage = 80;
        public bool UseGorefiendsGrasp = true;
        public bool UseHornofWinter = true;
        public bool UseHowlingBlast = true;
        public bool UseIceboundFortitude = true;
        public int UseIceboundFortitudeAtPercentage = 80;
        public bool UseIcyTouch = true;
        public bool UseLichborne = true;
        public int UseLichborneAtPercentage = 45;
        public bool UseLowCombat = true;
        public int UseLowCombatAtPercentage = 15;
        public bool UseMindFreeze = true;
        public int UseMindFreezeAtPercentage = 95;
        public bool UseObliterate = true;
        public bool UseOutbreak = true;
        public bool UsePathofFrost = true;
        public bool UsePillarofFrost = true;
        public bool UsePlagueLeech = true;
        public bool UsePlagueStrike = true;
        public bool UseRemorselessWinter = true;
        public int UseRemorselessWinterAtPercentage = 70;
        public bool UseSoulReaper = true;
        public bool UseStoneform = true;
        public int UseStoneformAtPercentage = 80;
        public bool UseStrangulate = true;
        public int UseStrangulateAtPercentage = 95;
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;
        public bool UseTwoHander = true;
        public bool UseUnholyBlight = true;
        public bool UseUnholyPresence = true;
        public bool UseWarStomp = true;
        public int UseWarStompAtPercentage = 80;

        public DeathknightFrostSettings()
        {
            ConfigWinForm("Deathknight Frost Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent for Interrupt", "UseArcaneTorrentForDecast", "Professions & Racials", "AtPercentage");
            AddControlInWinForm("Use Arcane Torrent for Resource", "UseArcaneTorrentForResource", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Darkflight", "UseDarkflight", "Professions & Racials");
            AddControlInWinForm("Use Every Man for Himself", "UseEveryManforHimself", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials", "AtPercentage");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials", "AtPercentage");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials", "AtPercentage");
            /* Deathknight Presence & Buffs */
            AddControlInWinForm("Use Frost Presence", "UseFrostPresence", "Deathknight Presence & Buffs");
            AddControlInWinForm("Use Blood Presence", "UseBloodPresence", "Deathknight Presence & Buffs", "AtPercentage");
            AddControlInWinForm("Use Horn of Winter", "UseHornofWinter", "Deathknight Presence & Buffs");
            AddControlInWinForm("Use Path of Frost", "UsePathofFrost", "Deathknight Presence & Buffs");
            AddControlInWinForm("Use Unholy Presence", "UseUnholyPresence", "Deathknight Presence & Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Blood Boil", "UseBloodBoil", "Offensive Spell");
            AddControlInWinForm("Use Death Coil", "UseDeathCoil", "Offensive Spell");
            AddControlInWinForm("Use Death and Decay", "UseDeathandDecay", "Offensive Spell");
            AddControlInWinForm("Use Defile", "UseDefile", "Offensive Spell");
            AddControlInWinForm("Use Frost Strike", "UseFrostStrike", "Offensive Spell");
            AddControlInWinForm("Use Howling Blast", "UseHowlingBlast", "Offensive Spell");
            AddControlInWinForm("Use Icy Touch", "UseIcyTouch", "Offensive Spell");
            AddControlInWinForm("Use Plague Leech", "UsePlagueLeech", "Offensive Spell");
            AddControlInWinForm("Use Plague Strike", "UsePlagueStrike", "Offensive Spell");
            AddControlInWinForm("Use Obliterate", "UseObliterate", "Offensive Spell");
            AddControlInWinForm("Use Soul Reaper", "UseSoulReaper", "Offensive Spell");
            AddControlInWinForm("Use Unholy Blight", "UseUnholyBlight", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use Blood Tap for DPS", "UseBloodTapForDPS", "Offensive Cooldown");
            AddControlInWinForm("Use Blood Tap for Heal", "UseBloodTapForHeal", "Offensive Cooldown");
            AddControlInWinForm("Use Death Grip", "UseDeathGrip", "Offensive Cooldown");
            AddControlInWinForm("Use Empower Rune Weapon", "UseEmpowerRuneWeapon", "Offensive Cooldown");
            AddControlInWinForm("Use Outbreak", "UseOutbreak", "Offensive Cooldown");
            AddControlInWinForm("Use Pillar of Frost", "UsePillarofFrost", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Anti-Magic Shell", "UseAntiMagicShell", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Anti-Magic Zone", "UseAntiMagicZone", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Army of the Dead", "UseArmyoftheDead", "Defensive Cooldown");
            AddControlInWinForm("Use Asphyxiate", "UseAsphyxiate", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Chains of Ice", "UseChainsofIce", "Defensive Cooldown");
            AddControlInWinForm("Use Death's Advance", "UseDeathsAdvance", "Defensive Cooldown");
            AddControlInWinForm("Use Gorefriend's Grasp", "UseGorefriendsGrasp", "Defensive Cooldown");
            AddControlInWinForm("Use Icebound Fortitude", "UseIceboundFortitude", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Mind Freeze", "UseMindFreeze", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Remorseless Winter", "UseRemorseless Winter", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Strangulate", "UseStrangulate", "Defensive Cooldown", "AtPercentage");
            /* Healing Spell */
            AddControlInWinForm("Use Conversion", "UseConversion", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Death Pact", "UseDeathPact", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Death Siphon", "UseDeathSiphon", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Death Strike", "UseDeathStrike", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Lichborne", "UseLichborne", "Healing Spell", "AtPercentage");
            /* Game Settings */
            AddControlInWinForm("Use Low Combat Settings - Level Difference", "UseLowCombat", "Game Settings", "AtPercentage");
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
            AddControlInWinForm("Use Dual Wield", "UseDualWield", "Game Settings");
            AddControlInWinForm("Use Two Hander", "UseTwoHander", "Game Settings");
            AddControlInWinForm("Do avoid melee (Off Advised!!)", "DoAvoidMelee", "Game Settings");
            AddControlInWinForm("Avoid melee distance (1 to 4)", "DoAvoidMeleeDistance", "Game Settings");
        }

        public static DeathknightFrostSettings CurrentSetting { get; set; }

        public static DeathknightFrostSettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Deathknight_Frost.xml";
            if (File.Exists(currentSettingsFile))
            {
                return
                    CurrentSetting = Load<DeathknightFrostSettings>(currentSettingsFile);
            }
            return new DeathknightFrostSettings();
        }
    }

    #endregion
}

#endregion

// ReSharper restore ObjectCreationAsStatement
// ReSharper restore EmptyGeneralCatchClause