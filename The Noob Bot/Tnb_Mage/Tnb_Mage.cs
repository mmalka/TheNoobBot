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

    #region ICombatClass Members

    public float AggroRange
    {
        get { return InternalAggroRange; }
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
                    #region Mage Specialisation checking

                case WoWClass.Mage:

                    if (wowSpecialization == WoWSpecialization.MageArcane)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Mage_Arcane.xml";
                            var currentSetting = new MageArcane.MageArcaneSettings();
                            if (File.Exists(currentSettingsFile) && !resetSettings)
                            {
                                currentSetting = Settings.Load<MageArcane.MageArcaneSettings>(currentSettingsFile);
                            }
                            currentSetting.ToForm();
                            currentSetting.Save(currentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Mage Arcane Combat class...");
                            InternalRange = 30.0f;
                            EquipmentAndStats.SetPlayerSpe(WoWSpecialization.MageArcane);
                            new MageArcane();
                        }
                        break;
                    }
                    if (wowSpecialization == WoWSpecialization.MageFire)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Mage_Fire.xml";
                            var currentSetting = new MageFire.MageFireSettings();
                            if (File.Exists(currentSettingsFile) && !resetSettings)
                            {
                                currentSetting = Settings.Load<MageFire.MageFireSettings>(currentSettingsFile);
                            }
                            currentSetting.ToForm();
                            currentSetting.Save(currentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Mage Fire Combat class...");
                            InternalRange = 30.0f;
                            EquipmentAndStats.SetPlayerSpe(WoWSpecialization.MageFire);
                            new MageFire();
                        }
                        break;
                    }
                    if (wowSpecialization == WoWSpecialization.MageFrost || wowSpecialization == WoWSpecialization.None)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Mage_Frost.xml";
                            var currentSetting = new MageFrost.MageFrostSettings();
                            if (File.Exists(currentSettingsFile) && !resetSettings)
                            {
                                currentSetting = Settings.Load<MageFrost.MageFrostSettings>(currentSettingsFile);
                            }
                            currentSetting.ToForm();
                            currentSetting.Save(currentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Mage Frost Combat class...");
                            InternalRange = 30.0f;
                            EquipmentAndStats.SetPlayerSpe(WoWSpecialization.MageFrost);
                            new MageFrost();
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

#region Mage

public class MageArcane
{
    private static MageArcaneSettings MySettings = MageArcaneSettings.GetSettings();

    #region General Timers & Variables

    private readonly WoWItem _firstTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET);
    private readonly WoWItem _secondTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET, 2);
    public int DecastHP = 0;
    public int DefenseHP = 0;
    public int HealHP = 0;
    public int HealMP = 0;
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

    #endregion

    #region Mage Buffs

    public readonly Spell ArcaneBrilliance = new Spell("Arcane Brilliance");
    public readonly Spell ArcaneChargePassive = new Spell("Arcane Charge");
    public readonly Spell BlazingSpeed = new Spell("Blazing Speed");
    public readonly Spell Cauterize = new Spell(87024);
    public readonly Spell DalaranBrilliance = new Spell("Dalaran Brilliance");
    public readonly Spell IceFloes = new Spell("Ice Floes");
    public readonly Spell Hypothermia = new Spell(41425);
    public readonly Spell RuneofPower = new Spell("Rune of Power");
    public readonly Spell RuneofPowerBuff = new Spell(116014);

    #endregion

    #region Offensive Spell

    public readonly Spell ArcaneOrb = new Spell("Arcane Orb");
    public readonly Spell ArcaneBarrage = new Spell("Arcane Barrage");
    public readonly Spell ArcaneBlast = new Spell("Arcane Blast");
    public readonly Spell ArcaneExplosion = new Spell("Arcane Explosion");
    public readonly Spell ArcaneMissiles = new Spell("Arcane Missiles");
    public readonly Spell NetherTempest = new Spell("Nether Tempest");

    #endregion

    #region Offensive Cooldown

    public readonly Spell ArcanePower = new Spell("Arcane Power");
    public readonly Spell MirrorImage = new Spell("Mirror Image");
    public readonly Spell PresenceofMind = new Spell("Presence of Mind");
    public readonly Spell PrismaticCrystal = new Spell("Prismatic Crystal");
    public readonly Spell Supernova = new Spell("Supernova");
    public readonly Spell TimeWarp = new Spell("Time Warp");

    #endregion

    #region Defensive Cooldown

    public readonly Spell AlterTime = new Spell("Alter Time");
    public readonly Spell Blink = new Spell("Blink");
    public readonly Spell ColdSnap = new Spell("Cold Snap");
    public readonly Spell ConeofCold = new Spell("Cone of Cold");
    public readonly Spell Counterspell = new Spell("Counterspell");
    public readonly Spell Evanesce = new Spell("Evanesce");
    public readonly Spell FrostNova = new Spell("Frost Nova");
    public readonly Spell Frostjaw = new Spell("Frostjaw");
    public readonly Spell GreaterInvisibility = new Spell("Greater Invisibility");
    public readonly Spell IceBarrier = new Spell("Ice Barrier");
    public readonly Spell IceBlock = new Spell("Ice Block");
    public readonly Spell IceWard = new Spell("Ice Ward");
    public readonly Spell Invisibility = new Spell("Invisibility");
    public readonly Spell RingofFrost = new Spell("Ring of Frost");
    public readonly Spell Slow = new Spell("Slow");

    #endregion

    #region Healing Spell

    public readonly Spell ConjureRefreshment = new Spell("Conjure Refreshment");
    public readonly Spell Evocation = new Spell("Evocation");
    public readonly Spell GlobalCooldown = new Spell("Global Cooldown");
    private Timer _conjureRefreshmentTimer = new Timer(0);
    private bool _burnPhaseStarted = false;

    #endregion

    public MageArcane()
    {
        Main.InternalRange = 39f;
        Main.InternalAggroRange = 39f;
        MySettings = MageArcaneSettings.GetSettings();
        Main.DumpCurrentSettings<MageArcaneSettings>(MySettings);
        UInt128 lastTarget = 0;
        LowHP();

        while (Main.InternalLoop)
        {
            try
            {
                if (!ObjectManager.Me.IsDeadMe)
                {
                    if (!ObjectManager.Me.IsMounted)
                    {
                        if (Fight.InFight && ObjectManager.Me.Target > 0)
                        {
                            if (ObjectManager.Me.Target != lastTarget && (ArcaneBarrage.IsHostileDistanceGood || ArcaneBlast.IsHostileDistanceGood))
                            {
                                Pull();
                                lastTarget = ObjectManager.Me.Target;
                            }

                            if (MySettings.UseLowCombat && (ObjectManager.Me.Level - ObjectManager.Target.Level >= MySettings.UseLowCombatAtPercentage))
                            {
                                LC = 1;
                                if (CombatClass.InSpellRange(ObjectManager.Target, 0, Main.InternalRange))
                                    LowCombat();
                            }
                            else
                            {
                                LC = 0;
                                if (CombatClass.InSpellRange(ObjectManager.Target, 0, Main.InternalRange))
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
        if (MySettings.UseAlterTimeAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseAlterTimeAtPercentage;

        if (MySettings.UseConeofColdAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseConeofColdAtPercentage;

        if (MySettings.UseFrostNovaAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseFrostNovaAtPercentage;

        if (MySettings.UseEvanesceAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseEvanesceAtPercentage;

        if (MySettings.UseGreaterInvisibilityAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseGreaterInvisibilityAtPercentage;

        if (MySettings.UseIceBarrierAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseIceBarrierAtPercentage;

        if (MySettings.UseIceWardAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseIceWardAtPercentage;

        if (MySettings.UseInvisibilityAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseInvisibilityAtPercentage;

        if (MySettings.UseStoneformAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseStoneformAtPercentage;

        if (MySettings.UseArcaneTorrentForDecastAtPercentage > DecastHP)
            DecastHP = MySettings.UseArcaneTorrentForDecastAtPercentage;

        if (MySettings.UseCounterspellAtPercentage > DecastHP)
            DecastHP = MySettings.UseCounterspellAtPercentage;

        if (MySettings.UseFrostjawAtPercentage > DecastHP)
            DecastHP = MySettings.UseFrostjawAtPercentage;

        if (MySettings.UseArcaneTorrentForResourceAtPercentage > HealMP)
            HealMP = MySettings.UseArcaneTorrentForResourceAtPercentage;

        if (MySettings.UseColdSnapForHealAtPercentage > HealHP)
            HealHP = MySettings.UseColdSnapForHealAtPercentage;

        if (MySettings.UseGiftoftheNaaruAtPercentage > HealHP)
            HealHP = MySettings.UseGiftoftheNaaruAtPercentage;
    }

    private void Pull()
    {
        if (MySettings.UseRuneofPower && !RuneofPowerBuff.HaveBuff && RuneofPower.IsSpellUsable && ArcaneBlast.IsHostileDistanceGood)
            SpellManager.CastSpellByIDAndPosition(RuneofPower.Id, ObjectManager.Me.Position);

        if (MySettings.UseIceFloes && IceFloes.IsSpellUsable && ObjectManager.Me.GetMove)
            IceFloes.Cast();

        if (MySettings.UseArcaneBlast && ArcaneBlast.KnownSpell && ArcaneBlast.IsSpellUsable && ArcaneBlast.IsHostileDistanceGood)
            ArcaneBlast.Cast();
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

        if (MySettings.UseArcaneBarrage && ArcaneBarrage.IsSpellUsable && ArcaneBarrage.IsHostileDistanceGood)
        {
            ArcaneBarrage.Cast();
            return;
        }
        if (MySettings.UseArcaneMissiles && ArcaneMissiles.IsSpellUsable && ArcaneMissiles.IsHostileDistanceGood)
        {
            ArcaneMissiles.Cast();
            return;
        }
        if (MySettings.UseArcaneBlast && ArcaneBlast.IsSpellUsable && ArcaneBlast.IsHostileDistanceGood)
        {
            ArcaneBlast.Cast();
            return;
        }
        if (MySettings.UseArcaneExplosion && ArcaneExplosion.IsSpellUsable && ArcaneExplosion.IsHostileDistanceGood)
            ArcaneExplosion.Cast();
    }

    private void Combat()
    {
        Buff();

        if (MySettings.DoAvoidMelee)
            AvoidMelee();

        DPSCycle();

        if (_onCd.IsReady && (ObjectManager.Me.HealthPercent <= DefenseHP || ObjectManager.GetNumberAttackPlayer() > 2))
            DefenseCycle();

        if (ObjectManager.Me.HealthPercent <= HealHP || ObjectManager.Me.ManaPercentage <= HealMP)
            Heal();

        if (ObjectManager.Me.HealthPercent <= DecastHP || (MySettings.UseSlow && ObjectManager.Target.GetMove))
            Decast();

        DPSBurst();
        DPSCycle();
    }

    private void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (MySettings.UseArcaneBrilliance && !ArcaneBrilliance.HaveBuff && !DalaranBrilliance.HaveBuff && ArcaneBrilliance.IsSpellUsable)
            ArcaneBrilliance.Cast();

        if (MySettings.UseBlazingSpeed && !Darkflight.HaveBuff && ObjectManager.Me.GetMove && !ObjectManager.Target.InCombat && BlazingSpeed.IsSpellUsable)
            BlazingSpeed.Cast();

        if (MySettings.UseDarkflight && Darkflight.KnownSpell && !BlazingSpeed.HaveBuff && ObjectManager.Me.GetMove && !ObjectManager.Target.InCombat && Darkflight.IsSpellUsable)
            Darkflight.Cast();

        if (MySettings.UseIceBlock && !Evanesce.KnownSpell && Cauterize.HaveBuff && !Hypothermia.HaveBuff)
        {
            if (MySettings.UseColdSnapForDefence && !IceBlock.IsSpellUsable && ColdSnap.IsSpellUsable)
            {
                ColdSnap.Cast();
                Others.SafeSleep(1000);
            }
            if (IceBlock.IsSpellUsable)
            {
                IceBlock.Cast();
                _onCd = new Timer(1000*10);
                return;
            }
        }

        if (MySettings.UseAlchFlask && !ObjectManager.Me.HaveBuff(79638) && !ObjectManager.Me.HaveBuff(79640) && !ObjectManager.Me.HaveBuff(79639)
            && !ItemsManager.IsItemOnCooldown(75525) && ItemsManager.GetItemCount(75525) > 0)
            ItemsManager.UseItem(75525);

        if (MySettings.UseConjureRefreshment && _conjureRefreshmentTimer.IsReady
            && ItemsManager.GetItemCount(113509) == 0 // 100
            && ItemsManager.GetItemCount(80610) == 0 // 90
            && ItemsManager.GetItemCount(65499) == 0 // 85-89
            && ItemsManager.GetItemCount(43523) == 0 // 84-80
            && ItemsManager.GetItemCount(43518) == 0 // 79-74
            && ItemsManager.GetItemCount(65517) == 0 // 73-64
            && ItemsManager.GetItemCount(65516) == 0 // 63-54
            && ItemsManager.GetItemCount(65515) == 0 // 53-44
            && ItemsManager.GetItemCount(65500) == 0 // 43-38
            && ConjureRefreshment.IsSpellUsable)
        {
            ConjureRefreshment.Cast();
            _conjureRefreshmentTimer = new Timer(1000*60*10);
        }
    }

    private void AvoidMelee()
    {
        if (ObjectManager.Target.GetDistance < MySettings.DoAvoidMeleeDistance && ObjectManager.Target.InCombat)
        {
            Logging.WriteFight("Too Close. Moving Back");
            var maxTimeTimer = new Timer(1000*2);
            MovementsAction.MoveBackward(true);
            while (ObjectManager.Target.GetDistance < 2 && ObjectManager.Target.InCombat && !maxTimeTimer.IsReady)
                Others.SafeSleep(300);
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

        if (MySettings.UseAlterTime && !AlterTime.HaveBuff && ObjectManager.Me.HealthPercent <= MySettings.UseAlterTimeAtPercentage && AlterTime.IsSpellUsable)
        {
            AlterTime.Cast();
            return;
        }
        if (MySettings.UseBlink && (FrostNova.TargetHaveBuff || ConeofCold.TargetHaveBuff || Slow.TargetHaveBuff || Frostjaw.TargetHaveBuff) && Blink.IsSpellUsable
            && ObjectManager.Target.GetDistance < 5)
        {
            Blink.Cast();
            return;
        }
        if (MySettings.UseConeofCold && ObjectManager.Me.HealthPercent <= MySettings.UseConeofColdAtPercentage && ConeofCold.IsSpellUsable && ConeofCold.IsHostileDistanceGood)
        {
            ConeofCold.Cast();
            return;
        }
        if (MySettings.UseEvanesce && ObjectManager.Me.HealthPercent <= MySettings.UseEvanesceAtPercentage)
        {
            if (MySettings.UseColdSnapForDefence && !Evanesce.IsSpellUsable && ColdSnap.IsSpellUsable)
            {
                ColdSnap.Cast();
                Others.SafeSleep(1000);
            }
            if (Evanesce.IsSpellUsable)
            {
                Evanesce.Cast();
                _onCd = new Timer(1000*3);
            }
            return;
        }
        if (MySettings.UseFrostjaw && ObjectManager.Me.HealthPercent <= MySettings.UseFrostjawAtPercentage && Frostjaw.IsSpellUsable && ObjectManager.Target.GetDistance < 10)
        {
            Frostjaw.Cast();
            return;
        }
        if (MySettings.UseFrostNova && ObjectManager.Me.HealthPercent <= MySettings.UseFrostNovaAtPercentage && ObjectManager.GetUnitInSpellRange(RingofFrost.MaxRangeHostile) > 0)
        {
            if (MySettings.UseColdSnapForDefence && !FrostNova.IsSpellUsable && ColdSnap.IsSpellUsable)
            {
                ColdSnap.Cast();
                Others.SafeSleep(1000);
            }
            if (FrostNova.IsSpellUsable)
            {
                FrostNova.Cast();
                _onCd = new Timer(1000*8);
            }
            return;
        }
        if (MySettings.UseIceBarrier && !IceBarrier.HaveBuff && ObjectManager.Me.HealthPercent <= MySettings.UseIceBarrierAtPercentage && IceBarrier.IsSpellUsable)
        {
            IceBarrier.Cast();
            return;
        }
        if (MySettings.UseIceWard && ObjectManager.Me.HealthPercent <= MySettings.UseIceWardAtPercentage && IceWard.IsSpellUsable && ObjectManager.Target.GetDistance < 10)
        {
            IceWard.Cast();
            _onCd = new Timer(1000*5);
            return;
        }
        if (MySettings.UseInvisibility && ObjectManager.Me.HealthPercent <= MySettings.UseInvisibilityAtPercentage && Invisibility.IsSpellUsable)
        {
            Invisibility.Cast();
            Others.SafeSleep(8000);
            return;
        }
        if (MySettings.UseGreaterInvisibility && ObjectManager.Me.HealthPercent <= MySettings.UseGreaterInvisibilityAtPercentage && Invisibility.IsSpellUsable)
        {
            GreaterInvisibility.Cast();
            Others.SafeSleep(8000);
            return;
        }
        if (MySettings.UseRingofFrost && RingofFrost.IsSpellUsable && ObjectManager.GetUnitInSpellRange(RingofFrost.MaxRangeHostile) > 2)
        {
            SpellManager.CastSpellByIDAndPosition(113724, ObjectManager.Target.Position);
            _onCd = new Timer(1000*10);
            return;
        }
        if (MySettings.UseStoneform && ObjectManager.Me.HealthPercent <= MySettings.UseStoneformAtPercentage && Stoneform.IsSpellUsable)
        {
            Stoneform.Cast();
            _onCd = new Timer(1000*8);
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (MySettings.UseArcaneTorrentForResource && ObjectManager.Me.ManaPercentage <= MySettings.UseArcaneTorrentForResourceAtPercentage && ArcaneTorrent.IsSpellUsable)
        {
            ArcaneTorrent.Cast();
            return;
        }
        if (MySettings.UseGiftoftheNaaru && ObjectManager.Me.HealthPercent <= MySettings.UseGiftoftheNaaruAtPercentage && GiftoftheNaaru.IsSpellUsable)
        {
            GiftoftheNaaru.Cast();
            return;
        }
        if (MySettings.UseColdSnapForHeal && ObjectManager.Me.HealthPercent <= MySettings.UseColdSnapForHealAtPercentage && ColdSnap.IsSpellUsable)
        {
            ColdSnap.Cast();
            return;
        }
    }

    private void Decast()
    {
        if (MySettings.UseArcaneTorrentForDecast && ObjectManager.Me.HealthPercent <= MySettings.UseArcaneTorrentForDecastAtPercentage
            && ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && ArcaneTorrent.IsSpellUsable && ObjectManager.Target.GetDistance < 8)
        {
            ArcaneTorrent.Cast();
            return;
        }
        if (MySettings.UseCounterspell && ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && ObjectManager.Me.HealthPercent <= MySettings.UseCounterspellAtPercentage
            && Counterspell.IsSpellUsable && Counterspell.IsHostileDistanceGood)
        {
            Counterspell.Cast();
            return;
        }
        if (MySettings.UseFrostjaw && ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && ObjectManager.Me.HealthPercent <= MySettings.UseFrostjawAtPercentage
            && Frostjaw.IsSpellUsable && Frostjaw.IsHostileDistanceGood)
        {
            Frostjaw.Cast();
            return;
        }
        if (MySettings.UseSlow && !Slow.TargetHaveBuff && ObjectManager.Target.GetMove && Slow.IsSpellUsable && Slow.IsHostileDistanceGood)
            Slow.Cast();
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
        if (MySettings.UseBerserking && Berserking.IsSpellUsable && ArcaneBlast.IsHostileDistanceGood)
        {
            if (ObjectManager.Me.IsCast)
                ObjectManager.Me.StopCast();

            Others.SafeSleep(500);
            Berserking.Cast();
        }
        if (MySettings.UseBloodFury && BloodFury.IsSpellUsable && ArcaneBlast.IsHostileDistanceGood)
        {
            if (ObjectManager.Me.IsCast)
                ObjectManager.Me.StopCast();

            Others.SafeSleep(500);
            BloodFury.Cast();
        }
        if (MySettings.UseMirrorImage && MirrorImage.IsSpellUsable && ArcaneBlast.IsHostileDistanceGood)
        {
            if (ObjectManager.Me.IsCast)
                ObjectManager.Me.StopCast();

            Others.SafeSleep(500);
            MirrorImage.Cast();
        }
        if (MySettings.UseRuneofPower && !RuneofPowerBuff.HaveBuff && !ObjectManager.Me.GetMove && RuneofPower.IsSpellUsable && ArcaneBlast.IsHostileDistanceGood)
        {
            if (ObjectManager.Me.IsCast)
                ObjectManager.Me.StopCast();

            Others.SafeSleep(500);
            SpellManager.CastSpellByIDAndPosition(116011, ObjectManager.Me.Position);
        }
        if (MySettings.UsePresenceofMind && ArcaneChargePassive.BuffStack < 2 && PresenceofMind.IsSpellUsable && ArcaneBlast.IsHostileDistanceGood)
        {
            if (ObjectManager.Me.IsCast)
                ObjectManager.Me.StopCast();

            Others.SafeSleep(500);
            PresenceofMind.Cast();
        }
        if (MySettings.UsePrismaticCrystal && _burnPhaseStarted && PrismaticCrystal.IsSpellUsable && PrismaticCrystal.IsHostileDistanceGood)
        {
            if (ObjectManager.Me.IsCast)
                ObjectManager.Me.StopCast();

            Others.SafeSleep(500);
            SpellManager.CastSpellByIDAndPosition(152087, ObjectManager.Target.Position);
            Lua.RunMacroText("/target Prismatic Crystal");
        }
        if (MySettings.UseTimeWarp && !ObjectManager.Me.HaveBuff(80354) && !ObjectManager.Me.HaveBuff(57724) && !ObjectManager.Me.HaveBuff(57723) && !ObjectManager.Me.HaveBuff(95809)
            && TimeWarp.IsSpellUsable && ArcaneBlast.IsHostileDistanceGood)
        {
            if (ObjectManager.Me.IsCast)
                ObjectManager.Me.StopCast();

            Others.SafeSleep(500);
            TimeWarp.Cast();
        }
    }

    private void DPSCycle()
    {
        Usefuls.SleepGlobalCooldown();
        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            if (MySettings.UseEvocation && ArcaneChargePassive.BuffStack >= 4 && Evocation.IsSpellUsable && ArcaneBlast.IsHostileDistanceGood)
            {
                _burnPhaseStarted = true;
                if (MySettings.UseArcanePower && ArcanePower.IsSpellUsable)
                    ArcanePower.Cast();
            }
            if (ObjectManager.Me.ManaPercentage < 50)
            {
                _burnPhaseStarted = false;
                Evocation.Launch(true, false, true);
                Others.SafeSleep(500);
                while (ObjectManager.Me.ManaPercentage < 96 && ObjectManager.Me.IsCast)
                    Others.SafeSleep(20);

                if (ObjectManager.Me.IsCast)
                    ObjectManager.Me.StopCast();
                return;
            }
            if (MySettings.UseIceFloes && !IceFloes.HaveBuff && ObjectManager.Me.GetMove && IceFloes.IsSpellUsable && ArcaneBlast.IsHostileDistanceGood)
                IceFloes.Cast();

            if (MySettings.UseNetherTempest && ArcaneChargePassive.BuffStack == 4 && (!NetherTempest.TargetHaveBuff || ObjectManager.Target.AuraIsActiveAndExpireInLessThanMs(NetherTempest.Id, 3599))
                && NetherTempest.IsSpellUsable && NetherTempest.IsHostileDistanceGood)
            {
                NetherTempest.Cast();
                return;
            }
            if (MySettings.UseSupernova && Supernova.GetSpellCharges > 0 && Supernova.IsSpellUsable && Supernova.IsHostileDistanceGood)
            {
                Supernova.Cast();
                return;
            }
            if (MySettings.UseArcaneOrb && ArcaneChargePassive.BuffStack <= 1 && ArcaneOrb.IsSpellUsable && ArcaneOrb.IsHostileDistanceGood)
            {
                ArcaneOrb.Cast();
                return;
            }
            if (MySettings.UseConeofCold && MySettings.UseConeofColdGlyph && ConeofCold.IsSpellUsable && ObjectManager.GetUnitInSpellRange(ConeofCold.MaxRangeHostile) >= 5)
            {
                ConeofCold.Cast();
                return;
            }
            if (MySettings.UseArcaneExplosion && ArcaneChargePassive.BuffStack < 4 && ArcaneExplosion.IsSpellUsable && ObjectManager.GetUnitInSpellRange(ArcaneExplosion.MaxRangeHostile) >= 5)
            {
                ArcaneExplosion.Cast();
                return;
            }
            if (MySettings.UseArcaneMissiles && (ArcaneChargePassive.BuffStack >= 4 || ArcaneMissiles.GetSpellCharges >= 3) && ArcaneMissiles.IsSpellUsable && ArcaneMissiles.IsHostileDistanceGood)
            {
                ArcaneMissiles.Cast();
                return;
            }
            if (MySettings.UseArcaneMissiles && ArcaneChargePassive.BuffStack >= 4 && ArcaneMissiles.IsSpellUsable && ArcaneMissiles.IsHostileDistanceGood)
            {
                ArcaneMissiles.Cast();
                return;
            }
            if (MySettings.UseArcaneBlast && (ObjectManager.Me.ManaPercentage >= 93 || ArcaneChargePassive.BuffStack < 4 || _burnPhaseStarted) && ArcaneBlast.IsSpellUsable && ArcaneBlast.IsHostileDistanceGood)
            {
                ArcaneBlast.Launch(true, false, true);
                return;
            }
            if (MySettings.UseArcaneBarrage && !_burnPhaseStarted && ArcaneChargePassive.BuffStack >= 4 && ArcaneBarrage.IsSpellUsable && ArcaneBarrage.IsHostileDistanceGood)
            {
                ArcaneBarrage.Cast();
                return;
            }
            if (MySettings.UseArcaneExplosion && ObjectManager.Me.GetMove && ObjectManager.Me.AuraIsActiveAndExpireInLessThanMs(ArcaneChargePassive.Id, 3500)
                && ArcaneExplosion.IsSpellUsable && ObjectManager.GetUnitInSpellRange(ArcaneExplosion.MaxRangeHostile) >= 1)
            {
                ArcaneExplosion.Cast();
                return;
            }
            if (MySettings.UseArcaneBlast && (!MySettings.UseArcaneBarrage || !ArcaneBarrage.KnownSpell) && ArcaneBlast.IsSpellUsable && ArcaneBlast.IsHostileDistanceGood)
            {
                ArcaneBlast.Launch(true, false, true);
                return;
            }
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    private void Patrolling()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        Buff();
        Heal();
    }

    #region Nested type: MageArcaneSettings

    [Serializable]
    public class MageArcaneSettings : Settings
    {
        public bool DoAvoidMelee = false;
        public int DoAvoidMeleeDistance = 0;
        public bool UseAlchFlask = true;
        public bool UseAlterTime = true;
        public int UseAlterTimeAtPercentage = 80;
        public bool UseArcaneBarrage = true;
        public bool UseArcaneBlast = true;
        public bool UseArcaneBrilliance = true;
        public bool UseArcaneExplosion = true;
        public bool UseArcaneMissiles = true;
        public bool UseArcaneOrb = true;
        public bool UseArcanePower = true;
        public bool UseArcaneTorrentForDecast = true;
        public int UseArcaneTorrentForDecastAtPercentage = 95;
        public bool UseArcaneTorrentForResource = true;
        public int UseArcaneTorrentForResourceAtPercentage = 80;
        public bool UseBerserking = true;
        public bool UseBlazingSpeed = true;
        public bool UseBlink = true;
        public bool UseBloodFury = true;
        public bool UseColdSnapForDefence = true;
        public bool UseColdSnapForHeal = true;
        public int UseColdSnapForHealAtPercentage = 70;
        public bool UseConeofCold = true;
        public int UseConeofColdAtPercentage = 45;
        public bool UseConeofColdGlyph = false;
        public bool UseConjureRefreshment = true;
        public bool UseCounterspell = true;
        public int UseCounterspellAtPercentage = 95;
        public bool UseDarkflight = true;
        public bool UseEvanesce = true;
        public int UseEvanesceAtPercentage = 90;
        public bool UseEveryManforHimself = true;
        public bool UseEvocation = true;
        public bool UseFrostNova = true;
        public int UseFrostNovaAtPercentage = 50;
        public bool UseFrostjaw = true;
        public int UseFrostjawAtPercentage = 95;
        public bool UseGiftoftheNaaru = true;
        public int UseGiftoftheNaaruAtPercentage = 80;
        public bool UseGreaterInvisibility = true;
        public int UseGreaterInvisibilityAtPercentage = 10;
        public bool UseIceBarrier = true;
        public int UseIceBarrierAtPercentage = 95;
        public bool UseIceBlock = true;
        public bool UseIceFloes = true;
        public bool UseIceWard = true;
        public int UseIceWardAtPercentage = 45;
        public bool UseInvisibility = true;
        public int UseInvisibilityAtPercentage = 10;
        public bool UseLowCombat = true;
        public int UseLowCombatAtPercentage = 15;
        public bool UseMirrorImage = true;
        public bool UseNetherTempest = true;
        public bool UsePresenceofMind = true;
        public bool UsePrismaticCrystal = true;
        public bool UseRingofFrost = true;
        public bool UseRuneofPower = true;
        public bool UseScorch = true;
        public bool UseSlow = false;
        public bool UseStoneform = true;
        public int UseStoneformAtPercentage = 80;
        public bool UseSupernova = true;
        public bool UseTimeWarp = true;
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;

        public MageArcaneSettings()
        {
            ConfigWinForm("Mage Arcane Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent for Interrupt", "UseArcaneTorrentForDecast", "Professions & Racials", "AtPercentage");
            AddControlInWinForm("Use Arcane Torrent for Resource", "UseArcaneTorrentForResource", "Professions & Racials", "AtPercentage");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Darkflight", "UseDarkflight", "Professions & Racials");
            AddControlInWinForm("Use Every Man for Himself", "UseEveryManforHimself", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials", "AtPercentage");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials", "AtPercentage");
            /* Mage Buffs */
            AddControlInWinForm("Use Arcane Brilliance", "UseArcaneBrilliance", "Mage Buffs");
            AddControlInWinForm("Use Blazing Speed", "UseBlazingSpeed", "Mage Buffs");
            AddControlInWinForm("Use Ice Floes", "UseIceFloes", "Mage Buffs");
            AddControlInWinForm("Use Rune of Power", "UseRuneofPower", "Mage Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Arcane Barrage", "UseArcaneBarrage", "Offensive Spell");
            AddControlInWinForm("Use Arcane Blast", "UseArcaneBlast", "Offensive Spell");
            AddControlInWinForm("Use Arcane Explosion", "UseArcaneExplosion", "Offensive Spell");
            AddControlInWinForm("Use Arcane Missiles", "UseArcaneMissiles", "Offensive Spell");
            AddControlInWinForm("Use Arcane Orb", "UseArcaneOrb", "Offensive Spell");
            AddControlInWinForm("Use Nether Tempest", "UseNetherTempest", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use Arcane Power", "UseArcanePower", "Offensive Cooldown");
            AddControlInWinForm("Use Mirror Image", "UseMirrorImage", "Offensive Cooldown");
            AddControlInWinForm("Use Presence of Mind", "UsePresenceofMind", "Offensive Cooldown");
            AddControlInWinForm("Use Prismatic Crystal", "UsePrismaticCrystal", "Offensive Cooldown");
            AddControlInWinForm("Use Supernova", "UseSupernova", "Offensive Cooldown");
            AddControlInWinForm("Use Time Warp", "UseTimeWarp", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Alter Time", "UseAlterTime", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Blink", "UseBlink", "Defensive Cooldown");
            AddControlInWinForm("Use Cone of Cold", "UseConeofCold", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Cold Snap For Defence", "UseColdSnapForDefence", "Defensive Cooldown");
            AddControlInWinForm("Use Counterspell", "UseCounterspell", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Evanesce", "UseEvanesce", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Frostjaw", "UseFrostjaw", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Frost Nova", "UseFrostNova", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Greater Invisiblity", "UseGreaterInvisibility", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Ice Barrier", "UseIceBarrier", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Ice Block", "UseIceBlock", "Defensive Cooldown");
            AddControlInWinForm("Use Ice Ward", "UseIceWard", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Invisibility", "UseInvisibility", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Ring of Frost", "UseRingofFrost", "Defensive Cooldown");
            AddControlInWinForm("Use Slow", "UseSlow", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Cold Snap For Healing", "UseColdSnapForHeal", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Conjure Refreshment", "UseConjureRefreshment", "Healing Spell");
            AddControlInWinForm("Use Evocation", "UseEvocation", "Healing Spell");
            /* Game Settings */
            AddControlInWinForm("Use Cone of Cold Glyph", "UseConeofColdGlyph", "Game Settings");
            AddControlInWinForm("Use Low Combat - Level Difference", "UseLowCombat", "Game Settings", "AtPercentage");
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
            AddControlInWinForm("Do avoid melee (Off Advised!!)", "DoAvoidMelee", "Game Settings");
            AddControlInWinForm("Avoid melee distance (1 to 4)", "DoAvoidMeleeDistance", "Game Settings");
        }

        public static MageArcaneSettings CurrentSetting { get; set; }

        public static MageArcaneSettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Mage_Arcane.xml";
            if (File.Exists(currentSettingsFile))
            {
                return
                    CurrentSetting = Load<MageArcaneSettings>(currentSettingsFile);
            }
            return new MageArcaneSettings();
        }
    }

    #endregion
}

public class MageFrost
{
    private static MageFrostSettings MySettings = MageFrostSettings.GetSettings();

    #region General Timers & Variablesoffrostat

    private readonly WoWItem _firstTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET);
    private readonly WoWItem _secondTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET, 2);
    public int DecastHP = 0;
    public int DefenseHP = 0;
    public int HealHP = 0;
    public int HealMP = 0;
    public int LC = 0;

    private Timer _petAbilityTimer = new Timer(0);
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

    #endregion

    #region Mage Buffs

    public readonly Spell ArcaneBrilliance = new Spell("Arcane Brilliance");
    public readonly Spell BlazingSpeed = new Spell("Blazing Speed");
    public readonly Spell BrainFreeze = new Spell(57761);
    public readonly Spell Cauterize = new Spell(87024);
    public readonly Spell DalaranBrilliance = new Spell("Dalaran Brilliance");
    public readonly Spell FingersofFrost = new Spell(44544);
    public readonly Spell Hypothermia = new Spell(41425);
    public readonly Spell IceFloes = new Spell("Ice Floes");
    public readonly Spell RuneofPower = new Spell("Rune of Power");
    public readonly Spell RuneofPowerBuff = new Spell(116014);

    #endregion

    #region Offensive Spell

    public readonly Spell Blizzard = new Spell("Blizzard");
    public readonly Spell CometStorm = new Spell("Comet Storm");
    public readonly Spell ConeofCold = new Spell("Cone of Cold");
    public readonly Spell FireBlast = new Spell("Fire Blast");
    public readonly Spell Frostbolt = new Spell("Frostbolt");
    public readonly Spell FrostBomb = new Spell("Frost Bomb");
    public readonly Spell FrostfireBolt = new Spell("Frostfire Bolt");
    public readonly Spell IceLance = new Spell("Ice Lance");
    public readonly Spell IceNova = new Spell("Ice Nova");
    public readonly Spell WaterJet = new Spell(135029);
    public readonly Spell SummonWaterElemental = new Spell("Summon Water Elemental");
    private Timer _waterJetTimer = new Timer(0);

    #endregion

    #region Offensive Cooldown

    public readonly Spell FrozenOrb = new Spell("Frozen Orb");
    public readonly Spell IcyVeins = new Spell("Icy Veins");
    public readonly Spell MirrorImage = new Spell("Mirror Image");
    public readonly Spell PrismaticCrystal = new Spell("Prismatic Crystal");
    public readonly Spell TimeWarp = new Spell("Time Warp");
    private Timer _frozenOrbTimer = new Timer(0);

    #endregion

    #region Defensive Cooldown

    public readonly Spell AlterTime = new Spell("Alter Time");
    public readonly Spell Blink = new Spell("Blink");
    public readonly Spell Counterspell = new Spell("Counterspell");
    public readonly Spell DeepFreeze = new Spell("Deep Freeze");
    public readonly Spell Evanesce = new Spell("Evanesce");
    public readonly Spell Frostjaw = new Spell("Frostjaw");
    public readonly Spell FrostNova = new Spell("Frost Nova");
    public readonly Spell GreaterInvisibility = new Spell("Greater Invisibility");
    public readonly Spell IceBarrier = new Spell("Ice Barrier");
    public readonly Spell IceBlock = new Spell("Ice Block");
    public readonly Spell IceWard = new Spell("Ice Ward");
    public readonly Spell Invisibility = new Spell("Invisibility");
    public readonly Spell RingofFrost = new Spell("Ring of Frost");

    #endregion

    #region Healing Spell

    public readonly Spell ColdSnap = new Spell("Cold Snap");
    public readonly Spell ConjureRefreshment = new Spell("Conjure Refreshment");
    private Timer _conjureRefreshmentTimer = new Timer(0);

    #endregion

    public MageFrost()
    {
        Main.InternalRange = 39f;
        Main.InternalAggroRange = 39f;
        MySettings = MageFrostSettings.GetSettings();
        Main.DumpCurrentSettings<MageFrostSettings>(MySettings);
        UInt128 lastTarget = 0;
        LowHP();

        while (Main.InternalLoop)
        {
            try
            {
                if (!ObjectManager.Me.IsDeadMe)
                {
                    if (!ObjectManager.Me.IsMounted)
                    {
                        if (Fight.InFight && ObjectManager.Me.Target > 0)
                        {
                            if (ObjectManager.Me.Target != lastTarget && (Frostbolt.IsHostileDistanceGood || IceLance.IsHostileDistanceGood))
                            {
                                Pull();
                                lastTarget = ObjectManager.Me.Target;
                            }
                            if (MySettings.UseLowCombat && (ObjectManager.Me.Level - ObjectManager.Target.Level >= MySettings.UseLowCombatAtPercentage))
                            {
                                LC = 1;
                                if (CombatClass.InSpellRange(ObjectManager.Target, 0, Main.InternalRange))
                                    LowCombat();
                            }
                            else
                            {
                                LC = 0;
                                if (CombatClass.InSpellRange(ObjectManager.Target, 0, Main.InternalRange))
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
        if (MySettings.UseAlterTimeAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseAlterTimeAtPercentage;

        if (MySettings.UseConeofColdAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseConeofColdAtPercentage;

        if (MySettings.UseDeepFreezeAtPercentage < DefenseHP)
            DefenseHP = MySettings.UseDeepFreezeAtPercentage;

        if (MySettings.UseFrostNovaAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseFrostNovaAtPercentage;

        if (MySettings.UseEvanesceAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseEvanesceAtPercentage;

        if (MySettings.UseGreaterInvisibilityAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseGreaterInvisibilityAtPercentage;

        if (MySettings.UseIceBarrierAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseIceBarrierAtPercentage;

        if (MySettings.UseIceWardAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseIceWardAtPercentage;

        if (MySettings.UseInvisibilityAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseInvisibilityAtPercentage;

        if (MySettings.UseStoneformAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseStoneformAtPercentage;

        if (MySettings.UseArcaneTorrentForDecastAtPercentage > DecastHP)
            DecastHP = MySettings.UseArcaneTorrentForDecastAtPercentage;

        if (MySettings.UseCounterspellAtPercentage > DecastHP)
            DecastHP = MySettings.UseCounterspellAtPercentage;

        if (MySettings.UseFrostjawAtPercentage > DecastHP)
            DecastHP = MySettings.UseFrostjawAtPercentage;

        if (MySettings.UseArcaneTorrentForResourceAtPercentage > HealMP)
            HealMP = MySettings.UseArcaneTorrentForResourceAtPercentage;

        if (MySettings.UseColdSnapForHealAtPercentage > HealHP)
            HealHP = MySettings.UseColdSnapForHealAtPercentage;

        if (MySettings.UseGiftoftheNaaruAtPercentage > HealHP)
            HealHP = MySettings.UseGiftoftheNaaruAtPercentage;
    }

    private void Pull()
    {
        if (ObjectManager.Pet.IsAlive)
        {
            Lua.RunMacroText("/petattack");
            Logging.WriteFight("Cast Pet Attack");
        }
        if (MySettings.UseRuneofPower && !RuneofPowerBuff.HaveBuff && RuneofPower.IsSpellUsable && Frostbolt.IsHostileDistanceGood)
            SpellManager.CastSpellByIDAndPosition(RuneofPower.Id, ObjectManager.Me.Position);

        if (MySettings.UseIceFloes && IceFloes.IsSpellUsable && ObjectManager.Me.GetMove)
            IceFloes.Cast();

        if (MySettings.UseFrostbolt && Frostbolt.KnownSpell && Frostbolt.IsSpellUsable && Frostbolt.IsHostileDistanceGood)
            Frostbolt.Cast();

        if (MySettings.UseFreeze && _petAbilityTimer.IsReady && (ObjectManager.Pet.Health != 0 || ObjectManager.Pet.Guid != 0)
            && !ObjectManager.Target.IsBoss && Frostbolt.IsHostileDistanceGood)
        {
            SpellManager.CastSpellByIDAndPosition(33395, ObjectManager.Target.Position);
            _petAbilityTimer = new Timer(1000*25);
            Others.SafeSleep(1000);

            if (MySettings.UseDeepFreeze && DeepFreeze.IsSpellUsable && DeepFreeze.IsHostileDistanceGood)
                DeepFreeze.Cast();
        }
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

        if (MySettings.UseIceLance && IceLance.KnownSpell && FingersofFrost.HaveBuff && IceLance.IsSpellUsable && IceLance.IsHostileDistanceGood)
        {
            IceLance.Cast();
            return;
        }
        if (MySettings.UseFrostfireBolt && FrostfireBolt.KnownSpell && BrainFreeze.HaveBuff && FrostfireBolt.IsSpellUsable && FrostfireBolt.IsHostileDistanceGood)
        {
            FrostfireBolt.Cast();
            return;
        }
        if (MySettings.UseFrostbolt && Frostbolt.KnownSpell && Frostbolt.IsSpellUsable && Frostbolt.IsHostileDistanceGood)
        {
            Frostbolt.Cast();
            return;
        }
        if (MySettings.UseConeofCold && ConeofCold.IsSpellUsable && ConeofCold.IsHostileDistanceGood)
            ConeofCold.Cast();
    }

    private void Combat()
    {
        Buff();

        if (MySettings.DoAvoidMelee)
            AvoidMelee();

        DPSCycle();

        if (_onCd.IsReady && (ObjectManager.Me.HealthPercent <= DefenseHP || ObjectManager.GetNumberAttackPlayer() > 2))
            DefenseCycle();

        if (ObjectManager.Me.HealthPercent <= HealHP || ObjectManager.Me.ManaPercentage <= HealMP)
            Heal();

        if (ObjectManager.Me.HealthPercent <= DecastHP)
            Decast();

        DPSBurst();
        DPSCycle();
    }

    private void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (MySettings.UseArcaneBrilliance && !ArcaneBrilliance.HaveBuff && !DalaranBrilliance.HaveBuff && ArcaneBrilliance.IsSpellUsable)
            ArcaneBrilliance.Cast();

        if (MySettings.UseBlazingSpeed && !Darkflight.HaveBuff && ObjectManager.Me.GetMove && !ObjectManager.Target.InCombat && BlazingSpeed.IsSpellUsable)
            BlazingSpeed.Cast();

        if (MySettings.UseDarkflight && Darkflight.KnownSpell && !BlazingSpeed.HaveBuff && ObjectManager.Me.GetMove && !ObjectManager.Target.InCombat && Darkflight.IsSpellUsable)
            Darkflight.Cast();

        if (MySettings.UseIceBlock && !Evanesce.KnownSpell && Cauterize.HaveBuff && !Hypothermia.HaveBuff)
        {
            if (MySettings.UseColdSnapForDefence && !IceBlock.IsSpellUsable && ColdSnap.IsSpellUsable)
            {
                ColdSnap.Cast();
                Others.SafeSleep(1000);
            }
            if (IceBlock.IsSpellUsable)
            {
                IceBlock.Cast();
                _onCd = new Timer(1000*10);
                return;
            }
        }

        if (MySettings.UseSummonWaterElemental && (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0) && SummonWaterElemental.IsSpellUsable)
        {
            Logging.WriteFight(" - PET DEAD - ");
            SummonWaterElemental.Cast();
        }

        if (MySettings.UseAlchFlask && !ObjectManager.Me.HaveBuff(79638) && !ObjectManager.Me.HaveBuff(79640) && !ObjectManager.Me.HaveBuff(79639)
            && !ItemsManager.IsItemOnCooldown(75525) && ItemsManager.GetItemCount(75525) > 0)
            ItemsManager.UseItem(75525);

        if (MySettings.UseConjureRefreshment && _conjureRefreshmentTimer.IsReady
            && ItemsManager.GetItemCount(113509) == 0 // 100
            && ItemsManager.GetItemCount(80610) == 0 // 90
            && ItemsManager.GetItemCount(65499) == 0 // 85-89
            && ItemsManager.GetItemCount(43523) == 0 // 84-80
            && ItemsManager.GetItemCount(43518) == 0 // 79-74
            && ItemsManager.GetItemCount(65517) == 0 // 73-64
            && ItemsManager.GetItemCount(65516) == 0 // 63-54
            && ItemsManager.GetItemCount(65515) == 0 // 53-44
            && ItemsManager.GetItemCount(65500) == 0 // 43-38
            && ConjureRefreshment.IsSpellUsable)
        {
            ConjureRefreshment.Cast();
            _conjureRefreshmentTimer = new Timer(1000*60*10);
        }
    }

    private void AvoidMelee()
    {
        if (ObjectManager.Target.GetDistance < MySettings.DoAvoidMeleeDistance && ObjectManager.Target.InCombat)
        {
            Logging.WriteFight("Too Close. Moving Back");
            var maxTimeTimer = new Timer(1000*2);
            MovementsAction.MoveBackward(true);
            while (ObjectManager.Target.GetDistance < 2 && ObjectManager.Target.InCombat && !maxTimeTimer.IsReady)
                Others.SafeSleep(300);
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

        if (MySettings.UseAlterTime && !AlterTime.HaveBuff && ObjectManager.Me.HealthPercent <= MySettings.UseAlterTimeAtPercentage && AlterTime.IsSpellUsable)
        {
            AlterTime.Cast();
            return;
        }
        if (MySettings.UseBlink && (FrostNova.TargetHaveBuff || ConeofCold.TargetHaveBuff || Frostjaw.TargetHaveBuff) && Blink.IsSpellUsable
            && ObjectManager.Target.GetDistance < 5)
        {
            Blink.Cast();
            return;
        }
        if (MySettings.UseConeofCold && ObjectManager.Me.HealthPercent <= MySettings.UseConeofColdAtPercentage && ConeofCold.IsSpellUsable && ConeofCold.IsHostileDistanceGood)
        {
            ConeofCold.Cast();
            return;
        }
        if (MySettings.UseDeepFreeze && ObjectManager.Me.HealthPercent <= MySettings.UseDeepFreezeAtPercentage && DeepFreeze.IsSpellUsable && DeepFreeze.IsHostileDistanceGood)
        {
            DeepFreeze.Cast();
            _onCd = new Timer(1000*5);
            return;
        }
        if (MySettings.UseEvanesce && ObjectManager.Me.HealthPercent <= MySettings.UseEvanesceAtPercentage)
        {
            if (MySettings.UseColdSnapForDefence && !Evanesce.IsSpellUsable && ColdSnap.IsSpellUsable)
            {
                ColdSnap.Cast();
                Others.SafeSleep(1000);
            }
            if (Evanesce.IsSpellUsable)
            {
                Evanesce.Cast();
                _onCd = new Timer(1000*3);
            }
            return;
        }
        if (MySettings.UseFrostjaw && ObjectManager.Me.HealthPercent <= MySettings.UseFrostjawAtPercentage && Frostjaw.IsSpellUsable && ObjectManager.Target.GetDistance < 10)
        {
            Frostjaw.Cast();
            return;
        }
        if (MySettings.UseFrostNova && ObjectManager.Me.HealthPercent <= MySettings.UseFrostNovaAtPercentage && ObjectManager.GetUnitInSpellRange(RingofFrost.MaxRangeHostile) > 0)
        {
            if (MySettings.UseColdSnapForDefence && !FrostNova.IsSpellUsable && ColdSnap.IsSpellUsable)
            {
                ColdSnap.Cast();
                Others.SafeSleep(1000);
            }
            if (FrostNova.IsSpellUsable)
            {
                FrostNova.Cast();
                _onCd = new Timer(1000*8);
            }
            return;
        }
        if (MySettings.UseIceBarrier && !IceBarrier.HaveBuff && ObjectManager.Me.HealthPercent <= MySettings.UseIceBarrierAtPercentage && IceBarrier.IsSpellUsable)
        {
            IceBarrier.Cast();
            return;
        }
        if (MySettings.UseIceWard && ObjectManager.Me.HealthPercent <= MySettings.UseIceWardAtPercentage && IceWard.IsSpellUsable && ObjectManager.Target.GetDistance < 10)
        {
            IceWard.Cast();
            _onCd = new Timer(1000*5);
            return;
        }
        if (MySettings.UseInvisibility && ObjectManager.Me.HealthPercent <= MySettings.UseInvisibilityAtPercentage && Invisibility.IsSpellUsable)
        {
            Invisibility.Cast();
            Others.SafeSleep(8000);
            return;
        }
        if (MySettings.UseGreaterInvisibility && ObjectManager.Me.HealthPercent <= MySettings.UseGreaterInvisibilityAtPercentage && Invisibility.IsSpellUsable)
        {
            GreaterInvisibility.Cast();
            Others.SafeSleep(8000);
            return;
        }
        if (MySettings.UseRingofFrost && RingofFrost.IsSpellUsable && ObjectManager.GetUnitInSpellRange(RingofFrost.MaxRangeHostile) > 2)
        {
            SpellManager.CastSpellByIDAndPosition(113724, ObjectManager.Target.Position);
            _onCd = new Timer(1000*10);
            return;
        }
        if (MySettings.UseStoneform && ObjectManager.Me.HealthPercent <= MySettings.UseStoneformAtPercentage && Stoneform.IsSpellUsable)
        {
            Stoneform.Cast();
            _onCd = new Timer(1000*8);
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (MySettings.UseArcaneTorrentForResource && ObjectManager.Me.ManaPercentage <= MySettings.UseArcaneTorrentForResourceAtPercentage && ArcaneTorrent.IsSpellUsable)
        {
            ArcaneTorrent.Cast();
            return;
        }
        if (MySettings.UseGiftoftheNaaru && ObjectManager.Me.HealthPercent <= MySettings.UseGiftoftheNaaruAtPercentage && GiftoftheNaaru.IsSpellUsable)
        {
            GiftoftheNaaru.Cast();
            return;
        }
        if (MySettings.UseColdSnapForHeal && ObjectManager.Me.HealthPercent <= MySettings.UseColdSnapForHealAtPercentage && ColdSnap.IsSpellUsable)
        {
            ColdSnap.Cast();
            return;
        }
    }

    private void Decast()
    {
        if (MySettings.UseArcaneTorrentForDecast && ObjectManager.Me.HealthPercent <= MySettings.UseArcaneTorrentForDecastAtPercentage
            && ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && ArcaneTorrent.IsSpellUsable && ObjectManager.Target.GetDistance < 8)
        {
            ArcaneTorrent.Cast();
            return;
        }
        if (MySettings.UseCounterspell && ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && ObjectManager.Me.HealthPercent <= MySettings.UseCounterspellAtPercentage
            && Counterspell.IsSpellUsable && Counterspell.IsHostileDistanceGood)
        {
            Counterspell.Cast();
            return;
        }
        if (MySettings.UseFrostjaw && ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && ObjectManager.Me.HealthPercent <= MySettings.UseFrostjawAtPercentage
            && Frostjaw.IsSpellUsable && Frostjaw.IsHostileDistanceGood)
            Frostjaw.Cast();
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
        if (MySettings.UseBerserking && Berserking.IsSpellUsable && Frostbolt.IsHostileDistanceGood)
        {
            if (ObjectManager.Me.IsCast)
                ObjectManager.Me.StopCast();

            Others.SafeSleep(500);
            Berserking.Cast();
        }
        if (MySettings.UseBloodFury && BloodFury.IsSpellUsable && Frostbolt.IsHostileDistanceGood)
        {
            if (ObjectManager.Me.IsCast)
                ObjectManager.Me.StopCast();

            Others.SafeSleep(500);
            BloodFury.Cast();
        }
        if (MySettings.UseIcyVeins && IcyVeins.IsSpellUsable && Frostbolt.IsHostileDistanceGood)
        {
            if (ObjectManager.Me.IsCast)
                ObjectManager.Me.StopCast();

            Others.SafeSleep(500);
            IcyVeins.Cast();
        }
        if (MySettings.UseMirrorImage && MirrorImage.IsSpellUsable && Frostbolt.IsHostileDistanceGood)
        {
            if (ObjectManager.Me.IsCast)
                ObjectManager.Me.StopCast();

            Others.SafeSleep(1000);
            MirrorImage.Cast();
        }
        if (MySettings.UseRuneofPower && !RuneofPowerBuff.HaveBuff && !ObjectManager.Me.GetMove && RuneofPower.IsSpellUsable && Frostbolt.IsHostileDistanceGood)
        {
            if (ObjectManager.Me.IsCast)
                ObjectManager.Me.StopCast();

            Others.SafeSleep(500);
            SpellManager.CastSpellByIDAndPosition(116011, ObjectManager.Me.Position);
        }
        if (MySettings.UsePrismaticCrystal && PrismaticCrystal.IsSpellUsable && PrismaticCrystal.IsHostileDistanceGood)
        {
            if (ObjectManager.Me.IsCast)
                ObjectManager.Me.StopCast();

            Others.SafeSleep(500);
            SpellManager.CastSpellByIDAndPosition(152087, ObjectManager.Target.Position);
            Lua.RunMacroText("/target Prismatic Crystal");

            if (ObjectManager.Pet.IsAlive)
            {
                Lua.RunMacroText("/petattack");
                Logging.WriteFight("Cast Pet Attack");
            }
        }
        if (MySettings.UseTimeWarp && !ObjectManager.Me.HaveBuff(80354) && !ObjectManager.Me.HaveBuff(57724) && !ObjectManager.Me.HaveBuff(57723) && !ObjectManager.Me.HaveBuff(95809)
            && TimeWarp.IsSpellUsable && Frostbolt.IsHostileDistanceGood)
        {
            if (ObjectManager.Me.IsCast)
                ObjectManager.Me.StopCast();

            Others.SafeSleep(500);
            TimeWarp.Cast();
        }
    }

    private void DPSCycle()
    {
        Usefuls.SleepGlobalCooldown();
        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            if (MySettings.UseIceFloes && !IceFloes.HaveBuff && ObjectManager.Me.GetMove && IceFloes.IsSpellUsable && Frostbolt.IsHostileDistanceGood)
                IceFloes.Cast();

            if (MySettings.UseFrozenOrb && FrozenOrb.IsSpellUsable && FrozenOrb.IsHostileDistanceGood)
            {
                FrozenOrb.Cast();
                _frozenOrbTimer = new Timer(1000*10);
                return;
            }
            if (MySettings.UseCometStorm && !ObjectManager.Target.GetMove && CometStorm.IsSpellUsable && CometStorm.IsHostileDistanceGood)
            {
                CometStorm.Cast();
                return;
            }
            if (MySettings.UseFrostBomb && !FrostBomb.TargetHaveBuff && FingersofFrost.BuffStack > 1 && FrostBomb.IsSpellUsable && Frostbolt.IsHostileDistanceGood)
            {
                FrostBomb.Cast();
                return;
            }
            if (MySettings.UseFreeze && _petAbilityTimer.IsReady && _frozenOrbTimer.IsReady && (ObjectManager.Pet.Health != 0 || ObjectManager.Pet.Guid != 0)
                && FingersofFrost.BuffStack < 2 && !ObjectManager.Target.IsBoss && Frostbolt.IsHostileDistanceGood)
            {
                SpellManager.CastSpellByIDAndPosition(33395, ObjectManager.Target.Position);
                _petAbilityTimer = new Timer(1000*25);
                return;
            }
            if (MySettings.UseIceNova && IceNova.GetSpellCharges > 1 && IceNova.IsSpellUsable && IceNova.IsHostileDistanceGood)
            {
                IceNova.Cast();
                return;
            }
            if (FrostBomb.KnownSpell)
            {
                if (MySettings.UseIceLance && FingersofFrost.HaveBuff && (FingersofFrost.BuffStack > 1 || FrostBomb.TargetHaveBuff
                                                                          || ObjectManager.Me.UnitAura(44544).AuraTimeLeftInMs < 4000) && IceLance.IsSpellUsable && IceLance.IsHostileDistanceGood)
                {
                    IceLance.Cast();
                    return;
                }
            }
            else
            {
                if (MySettings.UseIceLance && FingersofFrost.HaveBuff && IceLance.IsSpellUsable && IceLance.IsHostileDistanceGood)
                {
                    IceLance.Cast();
                    return;
                }
            }
            if (MySettings.UseConeofCold && ConeofCold.IsSpellUsable && ObjectManager.GetUnitInSpellRange(ConeofCold.MaxRangeHostile) > 1)
            {
                ConeofCold.Cast();
                return;
            }
            if (MySettings.UseBlizzard && Blizzard.IsSpellUsable && ObjectManager.GetUnitInSpellRange(Blizzard.MaxRangeHostile) > 4)
            {
                SpellManager.CastSpellByIDAndPosition(10, ObjectManager.Target.Position);
                Others.SafeSleep(500);
                while (FingersofFrost.BuffStack < 2 && ObjectManager.Me.IsCast)
                    Others.SafeSleep(20);

                if (ObjectManager.Me.IsCast)
                    ObjectManager.Me.StopCast();
                return;
            }
            if (MySettings.UseFrostfireBolt && BrainFreeze.HaveBuff && FingersofFrost.BuffStack < 2 && FrostfireBolt.IsSpellUsable && FrostfireBolt.IsHostileDistanceGood)
            {
                FrostfireBolt.Cast();
                return;
            }
            if (MySettings.UseWaterJet && _petAbilityTimer.IsReady && _frozenOrbTimer.IsReady && ObjectManager.Me.Level > 99 && (ObjectManager.Pet.Health != 0 || ObjectManager.Pet.Guid != 0)
                && Frostbolt.IsHostileDistanceGood)
            {
                while (FingersofFrost.HaveBuff)
                    IceLance.Cast();

                WaterJet.Cast();
                _petAbilityTimer = new Timer(1000*25);
                _waterJetTimer = new Timer(1000*4);

                while (!_waterJetTimer.IsReady)
                {
                    if (MySettings.UseFrostbolt && Frostbolt.KnownSpell && Frostbolt.IsHostileDistanceGood && Frostbolt.IsSpellUsable)
                        Frostbolt.Cast();
                }
                return;
            }
            if (MySettings.UseIceNova && IceNova.GetSpellCharges > 0 && IceNova.IsSpellUsable && IceNova.IsHostileDistanceGood)
            {
                IceNova.Cast();
                return;
            }
            if (MySettings.UseFrostbolt && BrainFreeze.BuffStack < 2 && FingersofFrost.BuffStack < 2 && Frostbolt.IsSpellUsable && Frostbolt.IsHostileDistanceGood)
            {
                Frostbolt.Launch(true, false, true);
                return;
            }
            if (MySettings.UseFrostfireBolt && (!MySettings.UseFrostbolt || !Frostbolt.KnownSpell) && FrostfireBolt.IsSpellUsable && FrostfireBolt.IsHostileDistanceGood)
            {
                FrostfireBolt.Cast();
                return;
            }
            if (MySettings.UseFireBlast && !IceLance.KnownSpell && FireBlast.IsSpellUsable && FireBlast.IsHostileDistanceGood)
                FireBlast.Cast();
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    private void Patrolling()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        Buff();
        Heal();
    }

    #region Nested type: MageFrostSettings

    [Serializable]
    public class MageFrostSettings : Settings
    {
        public bool DoAvoidMelee = false;
        public int DoAvoidMeleeDistance = 0;
        public bool UseAlchFlask = true;
        public bool UseAlterTime = true;
        public int UseAlterTimeAtPercentage = 80;
        public bool UseArcaneBrilliance = true;
        public bool UseArcaneExplosion = true;
        public bool UseArcaneTorrentForDecast = true;
        public int UseArcaneTorrentForDecastAtPercentage = 95;
        public bool UseArcaneTorrentForResource = true;
        public int UseArcaneTorrentForResourceAtPercentage = 80;
        public bool UseBerserking = true;
        public bool UseBlazingSpeed = true;
        public bool UseBlink = true;
        public bool UseBlizzard = true;
        public bool UseBloodFury = true;
        public bool UseColdSnapForDefence = true;
        public bool UseColdSnapForHeal = true;
        public int UseColdSnapForHealAtPercentage = 70;
        public bool UseCometStorm = true;
        public bool UseConeofCold = true;
        public int UseConeofColdAtPercentage = 45;
        public bool UseConjureRefreshment = true;
        public bool UseCounterspell = true;
        public int UseCounterspellAtPercentage = 95;
        public bool UseDarkflight = true;
        public bool UseDeepFreeze = true;
        public int UseDeepFreezeAtPercentage = 50;
        public bool UseEvanesce = true;
        public int UseEvanesceAtPercentage = 50;
        public bool UseEveryManforHimself = true;
        public bool UseFireBlast = true;
        public bool UseFreeze = true;
        public bool UseFrostNova = true;
        public int UseFrostNovaAtPercentage = 50;
        public bool UseFrostbolt = true;
        public bool UseFrostBomb = true;
        public bool UseFrostfireBolt = true;
        public bool UseFrostjaw = true;
        public int UseFrostjawAtPercentage = 95;
        public bool UseFrozenOrb = true;
        public bool UseGiftoftheNaaru = true;
        public int UseGiftoftheNaaruAtPercentage = 80;
        public bool UseGreaterInvisibility = true;
        public int UseGreaterInvisibilityAtPercentage = 10;
        public bool UseIceBarrier = true;
        public int UseIceBarrierAtPercentage = 95;
        public bool UseIceBlock = true;
        public bool UseIceFloes = true;
        public bool UseIceLance = true;
        public bool UseIceNova = true;
        public bool UseIceWard = true;
        public int UseIceWardAtPercentage = 45;
        public bool UseIcyVeins = true;
        public bool UseInvisibility = true;
        public int UseInvisibilityAtPercentage = 10;
        public bool UseLowCombat = true;
        public int UseLowCombatAtPercentage = 15;
        public bool UseMirrorImage = true;
        public bool UsePrismaticCrystal = true;
        public bool UseRingofFrost = true;
        public bool UseRuneofPower = true;
        public bool UseStoneform = true;
        public int UseStoneformAtPercentage = 80;
        public bool UseSummonWaterElemental = true;
        public bool UseTimeWarp = true;
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;
        public bool UseWaterJet = true;

        public MageFrostSettings()
        {
            ConfigWinForm("Mage Frost Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent for Interrupt", "UseArcaneTorrentForDecast", "Professions & Racials", "AtPercentage");
            AddControlInWinForm("Use Arcane Torrent for Resource", "UseArcaneTorrentForResource", "Professions & Racials", "AtPercentage");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Darkflight", "UseDarkflight", "Professions & Racials");
            AddControlInWinForm("Use Every Man for Himself", "UseEveryManforHimself", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials", "AtPercentage");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials", "AtPercentage");
            /* Mage Buffs */
            AddControlInWinForm("Use Arcane Brilliance", "UseArcaneBrilliance", "Mage Buffs");
            AddControlInWinForm("Use Blazing Speed", "UseBlazingSpeed", "Mage Buffs");
            AddControlInWinForm("Use Ice Floes", "UseIceFloes", "Mage Buffs");
            AddControlInWinForm("Use Rune of Power", "UseRuneofPower", "Mage Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Blizzard", "UseBlizzard", "Offensive Spell");
            AddControlInWinForm("Use Comet Storm", "UseCometStorm", "Offensive Spell");
            AddControlInWinForm("Use Fire Blast", "UseFireBlast", "Offensive Spell");
            AddControlInWinForm("Use Frost Bomb", "UseFrostBomb", "Offensive Spell");
            AddControlInWinForm("Use Frostbolt", "UseFrostbolt", "Offensive Spell");
            AddControlInWinForm("Use Frostfire Bolt", "UseFrostfireBolt", "Offensive Spell");
            AddControlInWinForm("Use Ice Lance", "UseIceLance", "Offensive Spell");
            AddControlInWinForm("Use Ice Nova", "UseIceNova", "Offensive Spell");
            AddControlInWinForm("Use Pet Freeze Ability", "UseFreeze", "Offensive Spell");
            AddControlInWinForm("Use Pet Water Jet Ability", "UseWaterJet", "Offensive Spell");
            AddControlInWinForm("Use Summon Water Elemental", "UseSummonWaterElemental", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use Frozen Orb", "UseFrozenOrb", "Offensive Cooldown");
            AddControlInWinForm("Use Icy Veins", "UseIcyVeins", "Offensive Cooldown");
            AddControlInWinForm("Use Mirror Image", "UseMirrorImage", "Offensive Cooldown");
            AddControlInWinForm("Use Frost Bomb", "UseFrostBomb", "Offensive Cooldown");
            AddControlInWinForm("Use Prismatic Crystal", "UsePrismaticCrystal", "Offensive Cooldown");
            AddControlInWinForm("Use Time Warp", "UseTimeWarp", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Alter Time", "UseAlterTime", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Blink", "UseBlink", "Defensive Cooldown");
            AddControlInWinForm("Use Cold Snap For Defence", "UseColdSnapForDefence", "Defensive Cooldown");
            AddControlInWinForm("Use Cone of Cold", "UseConeofCold", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Counterspell", "UseCounterspell", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Deep Freeze", "UseDeepFreeze", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Evanesce", "UseEvanesce", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Frostjaw", "UseFrostjaw", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Frost Nova", "UseFrostNova", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use GreaterInvisibility", "UseGreaterInvisibility", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Ice Barrier", "UseIceBarrier", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Ice Block", "UseIceBlock", "Defensive Cooldown");
            AddControlInWinForm("Use Ice Ward", "UseIceWard", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Invisibility", "UseInvisibility", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Ring of Frost", "UseRingofFrost", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Cold Snap For Heal", "UseColdSnapForHeal", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Conjure Refreshment", "UseConjureRefreshment", "Healing Spell");
            /* Game Settings */
            AddControlInWinForm("Use Low Combat Settings", "UseLowCombat", "Game Settings");
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
            AddControlInWinForm("Do avoid melee (Off Advised!!)", "DoAvoidMelee", "Game Settings");
            AddControlInWinForm("Avoid melee distance (1 to 4)", "DoAvoidMeleeDistance", "Game Settings");
        }

        public static MageFrostSettings CurrentSetting { get; set; }

        public static MageFrostSettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Mage_Frost.xml";
            if (File.Exists(currentSettingsFile))
            {
                return
                    CurrentSetting = Load<MageFrostSettings>(currentSettingsFile);
            }
            return new MageFrostSettings();
        }
    }

    #endregion
}

public class MageFire
{
    private static MageFireSettings MySettings = MageFireSettings.GetSettings();

    #region General Timers & Variables

    private readonly WoWItem _firstTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET);
    private readonly WoWItem _secondTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET, 2);
    public int DecastHP = 0;
    public int DefenseHP = 0;
    public int HealHP = 0;
    public int HealMP = 0;
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

    #endregion

    #region Mage Buffs

    public readonly Spell ArcaneBrilliance = new Spell("Arcane Brilliance");
    public readonly Spell BlazingSpeed = new Spell("Blazing Speed");
    public readonly Spell Cauterize = new Spell(87024);
    public readonly Spell DalaranBrilliance = new Spell("Dalaran Brilliance");
    public readonly Spell HeatingUp = new Spell(48107);
    public readonly Spell Hypothermia = new Spell(41425);
    public readonly Spell IceFloes = new Spell("Ice Floes");
    public readonly Spell Ignite = new Spell(12654);
    public readonly Spell PyroblastBuff = new Spell(48108);
    public readonly Spell RuneofPower = new Spell("Rune of Power");
    public readonly Spell RuneofPowerBuff = new Spell(116014);

    #endregion

    #region Offensive Spell

    public readonly Spell DragonsBreath = new Spell("Dragon's Breath");
    public readonly Spell Fireball = new Spell("Fireball");
    public readonly Spell Flamestrike = new Spell("Flamestrike");
    public readonly Spell InfernoBlast = new Spell("Inferno Blast");
    public readonly Spell Pyroblast = new Spell("Pyroblast");
    public readonly Spell Scorch = new Spell("Scorch");

    #endregion

    #region Offensive Cooldown

    public readonly Spell BlastWave = new Spell("Blast Wave");
    public readonly Spell Combustion = new Spell("Combustion");
    public readonly Spell LivingBomb = new Spell("Living Bomb");
    public readonly Spell Meteor = new Spell("Meteor");
    public readonly Spell MirrorImage = new Spell("Mirror Image");
    public readonly Spell PrismaticCrystal = new Spell("Prismatic Crystal");
    public readonly Spell TimeWarp = new Spell("Time Warp");

    #endregion

    #region Defensive Cooldown

    public readonly Spell AlterTime = new Spell("Alter Time");
    public readonly Spell Blink = new Spell("Blink");
    public readonly Spell ConeofCold = new Spell("Cone of Cold");
    public readonly Spell Counterspell = new Spell("Counterspell");
    public readonly Spell Evanesce = new Spell("Evanesce");
    public readonly Spell Frostjaw = new Spell("Frostjaw");
    public readonly Spell FrostNova = new Spell("Frost Nova");
    public readonly Spell GreaterInvisibility = new Spell("Greater Invisibility");
    public readonly Spell IceBarrier = new Spell("Ice Barrier");
    public readonly Spell IceBlock = new Spell("Ice Block");
    public readonly Spell IceWard = new Spell("Ice Ward");
    public readonly Spell Invisibility = new Spell("Invisibility");
    public readonly Spell RingofFrost = new Spell("Ring of Frost");

    #endregion

    #region Healing Spell

    public readonly Spell ColdSnap = new Spell("Cold Snap");
    public readonly Spell ConjureRefreshment = new Spell("Conjure Refreshment");
    private Timer _conjureRefreshmentTimer = new Timer(0);

    #endregion

    public MageFire()
    {
        Main.InternalRange = 39f;
        Main.InternalAggroRange = 39f;
        MySettings = MageFireSettings.GetSettings();
        Main.DumpCurrentSettings<MageFireSettings>(MySettings);
        UInt128 lastTarget = 0;
        LowHP();

        while (Main.InternalLoop)
        {
            try
            {
                if (!ObjectManager.Me.IsDeadMe)
                {
                    if (!ObjectManager.Me.IsMounted)
                    {
                        if (Fight.InFight && ObjectManager.Me.Target > 0)
                        {
                            if (ObjectManager.Me.Target != lastTarget
                                && (Scorch.IsHostileDistanceGood || Pyroblast.IsHostileDistanceGood))
                            {
                                Pull();
                                lastTarget = ObjectManager.Me.Target;
                            }

                            if (MySettings.UseLowCombat && (ObjectManager.Me.Level - ObjectManager.Target.Level >= MySettings.UseLowCombatAtPercentage))
                            {
                                LC = 1;
                                if (CombatClass.InSpellRange(ObjectManager.Target, 0, Main.InternalRange))
                                    LowCombat();
                            }
                            else
                            {
                                LC = 0;
                                if (CombatClass.InSpellRange(ObjectManager.Target, 0, Main.InternalRange))
                                    Combat();
                            }
                        }
                        else
                        {
                            if (!ObjectManager.Me.IsCast)
                                Patrolling();
                        }
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
        if (MySettings.UseAlterTimeAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseAlterTimeAtPercentage;

        if (MySettings.UseConeofColdAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseConeofColdAtPercentage;

        if (MySettings.UseFrostNovaAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseFrostNovaAtPercentage;

        if (MySettings.UseEvanesceAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseEvanesceAtPercentage;

        if (MySettings.UseGreaterInvisibilityAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseGreaterInvisibilityAtPercentage;

        if (MySettings.UseIceBarrierAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseIceBarrierAtPercentage;

        if (MySettings.UseIceWardAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseIceWardAtPercentage;

        if (MySettings.UseInvisibilityAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseInvisibilityAtPercentage;

        if (MySettings.UseStoneformAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseStoneformAtPercentage;

        if (MySettings.UseArcaneTorrentForDecastAtPercentage > DecastHP)
            DecastHP = MySettings.UseArcaneTorrentForDecastAtPercentage;

        if (MySettings.UseCounterspellAtPercentage > DecastHP)
            DecastHP = MySettings.UseCounterspellAtPercentage;

        if (MySettings.UseFrostjawAtPercentage > DecastHP)
            DecastHP = MySettings.UseFrostjawAtPercentage;

        if (MySettings.UseArcaneTorrentForResourceAtPercentage > HealMP)
            HealMP = MySettings.UseArcaneTorrentForResourceAtPercentage;

        if (MySettings.UseColdSnapForHealAtPercentage > HealHP)
            HealHP = MySettings.UseColdSnapForHealAtPercentage;

        if (MySettings.UseGiftoftheNaaruAtPercentage > HealHP)
            HealHP = MySettings.UseGiftoftheNaaruAtPercentage;
    }

    private void Pull()
    {
        if (MySettings.UseRuneofPower && !RuneofPowerBuff.HaveBuff && RuneofPower.IsSpellUsable && Fireball.IsHostileDistanceGood)
            SpellManager.CastSpellByIDAndPosition(RuneofPower.Id, ObjectManager.Me.Position);

        if (MySettings.UseIceFloes && IceFloes.IsSpellUsable && ObjectManager.Me.GetMove)
            IceFloes.Cast();

        if (MySettings.UsePyroblast && PyroblastBuff.HaveBuff && Pyroblast.IsSpellUsable && Pyroblast.IsHostileDistanceGood)
        {
            Pyroblast.Cast();
            return;
        }
        if (MySettings.UseScorch && ObjectManager.Me.GetMove && Scorch.IsSpellUsable && Scorch.IsHostileDistanceGood)
        {
            Scorch.Cast();
            return;
        }
        if (MySettings.UseFireball && Fireball.IsSpellUsable && Fireball.IsHostileDistanceGood)
            Fireball.Cast();
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

        if (MySettings.UsePyroblast && PyroblastBuff.HaveBuff && Pyroblast.IsSpellUsable && Pyroblast.IsHostileDistanceGood)
        {
            Pyroblast.Cast();
            return;
        }
        if (MySettings.UseInfernoBlast && InfernoBlast.IsSpellUsable && InfernoBlast.IsHostileDistanceGood)
        {
            InfernoBlast.Cast();
            return;
        }
        if (MySettings.UseFireball && Fireball.IsSpellUsable && Fireball.IsHostileDistanceGood)
        {
            Fireball.Cast();
            return;
        }
        if (MySettings.UseFlamestrike && Flamestrike.IsSpellUsable && Flamestrike.IsHostileDistanceGood)
            SpellManager.CastSpellByIDAndPosition(2120, ObjectManager.Target.Position);
    }

    private void Combat()
    {
        Buff();

        if (MySettings.DoAvoidMelee)
            AvoidMelee();

        DPSCycle();

        if (_onCd.IsReady && (ObjectManager.Me.HealthPercent <= DefenseHP || ObjectManager.GetNumberAttackPlayer() > 2))
            DefenseCycle();

        if (ObjectManager.Me.HealthPercent <= HealHP || ObjectManager.Me.ManaPercentage <= HealMP)
            Heal();

        if (ObjectManager.Me.HealthPercent <= DecastHP)
            Decast();

        DPSBurst();
        DPSCycle();
    }

    private void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (MySettings.UseArcaneBrilliance && !ArcaneBrilliance.HaveBuff && !DalaranBrilliance.HaveBuff && ArcaneBrilliance.IsSpellUsable)
            ArcaneBrilliance.Cast();

        if (MySettings.UseBlazingSpeed && !Darkflight.HaveBuff && ObjectManager.Me.GetMove && !ObjectManager.Target.InCombat && BlazingSpeed.IsSpellUsable)
            BlazingSpeed.Cast();

        if (MySettings.UseDarkflight && Darkflight.KnownSpell && !BlazingSpeed.HaveBuff && ObjectManager.Me.GetMove && !ObjectManager.Target.InCombat && Darkflight.IsSpellUsable)
            Darkflight.Cast();

        if (MySettings.UseIceBlock && !Evanesce.KnownSpell && Cauterize.HaveBuff && !Hypothermia.HaveBuff)
        {
            if (MySettings.UseColdSnapForDefence && !IceBlock.IsSpellUsable && ColdSnap.IsSpellUsable)
            {
                ColdSnap.Cast();
                Others.SafeSleep(1000);
            }
            if (IceBlock.IsSpellUsable)
            {
                IceBlock.Cast();
                _onCd = new Timer(1000*10);
                return;
            }
        }

        if (MySettings.UseAlchFlask && !ObjectManager.Me.HaveBuff(79638) && !ObjectManager.Me.HaveBuff(79640) && !ObjectManager.Me.HaveBuff(79639)
            && !ItemsManager.IsItemOnCooldown(75525) && ItemsManager.GetItemCount(75525) > 0)
            ItemsManager.UseItem(75525);

        if (MySettings.UseConjureRefreshment && _conjureRefreshmentTimer.IsReady
            && ItemsManager.GetItemCount(113509) == 0 // 100
            && ItemsManager.GetItemCount(80610) == 0 // 90
            && ItemsManager.GetItemCount(65499) == 0 // 85-89
            && ItemsManager.GetItemCount(43523) == 0 // 84-80
            && ItemsManager.GetItemCount(43518) == 0 // 79-74
            && ItemsManager.GetItemCount(65517) == 0 // 73-64
            && ItemsManager.GetItemCount(65516) == 0 // 63-54
            && ItemsManager.GetItemCount(65515) == 0 // 53-44
            && ItemsManager.GetItemCount(65500) == 0 // 43-38
            && ConjureRefreshment.IsSpellUsable)
        {
            ConjureRefreshment.Cast();
            _conjureRefreshmentTimer = new Timer(1000*60*10);
        }
    }

    private void AvoidMelee()
    {
        if (ObjectManager.Target.GetDistance < MySettings.DoAvoidMeleeDistance && ObjectManager.Target.InCombat)
        {
            Logging.WriteFight("Too Close. Moving Back");
            var maxTimeTimer = new Timer(1000*2);
            MovementsAction.MoveBackward(true);
            while (ObjectManager.Target.GetDistance < 2 && ObjectManager.Target.InCombat && !maxTimeTimer.IsReady)
                Others.SafeSleep(300);
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

        if (MySettings.UseAlterTime && !AlterTime.HaveBuff && ObjectManager.Me.HealthPercent <= MySettings.UseAlterTimeAtPercentage && AlterTime.IsSpellUsable)
        {
            AlterTime.Cast();
            return;
        }
        if (MySettings.UseBlink && (FrostNova.TargetHaveBuff || ConeofCold.TargetHaveBuff || Frostjaw.TargetHaveBuff) && Blink.IsSpellUsable
            && ObjectManager.Target.GetDistance < 5)
        {
            Blink.Cast();
            return;
        }
        if (MySettings.UseConeofCold && ObjectManager.Me.HealthPercent <= MySettings.UseConeofColdAtPercentage && ConeofCold.IsSpellUsable && ConeofCold.IsHostileDistanceGood)
        {
            ConeofCold.Cast();
            return;
        }
        if (MySettings.UseEvanesce && ObjectManager.Me.HealthPercent <= MySettings.UseEvanesceAtPercentage)
        {
            if (MySettings.UseColdSnapForDefence && !Evanesce.IsSpellUsable && ColdSnap.IsSpellUsable)
            {
                ColdSnap.Cast();
                Others.SafeSleep(1000);
            }
            if (Evanesce.IsSpellUsable)
            {
                Evanesce.Cast();
                _onCd = new Timer(1000*3);
            }
            return;
        }
        if (MySettings.UseFrostjaw && ObjectManager.Me.HealthPercent <= MySettings.UseFrostjawAtPercentage && Frostjaw.IsSpellUsable && ObjectManager.Target.GetDistance < 10)
        {
            Frostjaw.Cast();
            return;
        }
        if (MySettings.UseFrostNova && ObjectManager.Me.HealthPercent <= MySettings.UseFrostNovaAtPercentage && ObjectManager.GetUnitInSpellRange(RingofFrost.MaxRangeHostile) > 0)
        {
            if (MySettings.UseColdSnapForDefence && !FrostNova.IsSpellUsable && ColdSnap.IsSpellUsable)
            {
                ColdSnap.Cast();
                Others.SafeSleep(1000);
            }
            if (FrostNova.IsSpellUsable)
            {
                FrostNova.Cast();
                _onCd = new Timer(1000*8);
            }
            return;
        }
        if (MySettings.UseIceBarrier && !IceBarrier.HaveBuff && ObjectManager.Me.HealthPercent <= MySettings.UseIceBarrierAtPercentage && IceBarrier.IsSpellUsable)
        {
            IceBarrier.Cast();
            return;
        }
        if (MySettings.UseIceWard && ObjectManager.Me.HealthPercent <= MySettings.UseIceWardAtPercentage && IceWard.IsSpellUsable && ObjectManager.Target.GetDistance < 10)
        {
            IceWard.Cast();
            _onCd = new Timer(1000*5);
            return;
        }
        if (MySettings.UseInvisibility && ObjectManager.Me.HealthPercent <= MySettings.UseInvisibilityAtPercentage && Invisibility.IsSpellUsable)
        {
            Invisibility.Cast();
            Others.SafeSleep(8000);
            return;
        }
        if (MySettings.UseGreaterInvisibility && ObjectManager.Me.HealthPercent <= MySettings.UseGreaterInvisibilityAtPercentage && Invisibility.IsSpellUsable)
        {
            GreaterInvisibility.Cast();
            Others.SafeSleep(8000);
            return;
        }
        if (MySettings.UseRingofFrost && RingofFrost.IsSpellUsable && ObjectManager.GetUnitInSpellRange(RingofFrost.MaxRangeHostile) > 2)
        {
            SpellManager.CastSpellByIDAndPosition(113724, ObjectManager.Target.Position);
            _onCd = new Timer(1000*10);
            return;
        }
        if (MySettings.UseStoneform && ObjectManager.Me.HealthPercent <= MySettings.UseStoneformAtPercentage && Stoneform.IsSpellUsable)
        {
            Stoneform.Cast();
            _onCd = new Timer(1000*8);
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (MySettings.UseArcaneTorrentForResource && ObjectManager.Me.ManaPercentage <= MySettings.UseArcaneTorrentForResourceAtPercentage && ArcaneTorrent.IsSpellUsable)
        {
            ArcaneTorrent.Cast();
            return;
        }
        if (MySettings.UseGiftoftheNaaru && ObjectManager.Me.HealthPercent <= MySettings.UseGiftoftheNaaruAtPercentage && GiftoftheNaaru.IsSpellUsable)
        {
            GiftoftheNaaru.Cast();
            return;
        }
        if (MySettings.UseColdSnapForHeal && ObjectManager.Me.HealthPercent <= MySettings.UseColdSnapForHealAtPercentage && ColdSnap.IsSpellUsable)
        {
            ColdSnap.Cast();
            return;
        }
    }

    private void Decast()
    {
        if (MySettings.UseArcaneTorrentForDecast && ObjectManager.Me.HealthPercent <= MySettings.UseArcaneTorrentForDecastAtPercentage
            && ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && ArcaneTorrent.IsSpellUsable && ObjectManager.Target.GetDistance < 8)
        {
            ArcaneTorrent.Cast();
            return;
        }
        if (MySettings.UseCounterspell && ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && ObjectManager.Me.HealthPercent <= MySettings.UseCounterspellAtPercentage
            && Counterspell.IsSpellUsable && Counterspell.IsHostileDistanceGood)
        {
            Counterspell.Cast();
            return;
        }
        if (MySettings.UseFrostjaw && ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && ObjectManager.Me.HealthPercent <= MySettings.UseFrostjawAtPercentage
            && Frostjaw.IsSpellUsable && Frostjaw.IsHostileDistanceGood)
            Frostjaw.Cast();
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
        if (MySettings.UseBerserking && Berserking.IsSpellUsable && Fireball.IsHostileDistanceGood)
        {
            if (ObjectManager.Me.IsCast)
                ObjectManager.Me.StopCast();

            Others.SafeSleep(500);
            Berserking.Cast();
        }
        if (MySettings.UseBloodFury && BloodFury.IsSpellUsable && Fireball.IsHostileDistanceGood)
        {
            if (ObjectManager.Me.IsCast)
                ObjectManager.Me.StopCast();

            Others.SafeSleep(500);
            BloodFury.Cast();
        }
        if (MySettings.UseMirrorImage && MirrorImage.IsSpellUsable && Fireball.IsHostileDistanceGood)
        {
            if (ObjectManager.Me.IsCast)
                ObjectManager.Me.StopCast();

            Others.SafeSleep(1000);
            MirrorImage.Cast();
        }
        if (MySettings.UseRuneofPower && !RuneofPowerBuff.HaveBuff && !ObjectManager.Me.GetMove && RuneofPower.IsSpellUsable && Fireball.IsHostileDistanceGood)
        {
            if (ObjectManager.Me.IsCast)
                ObjectManager.Me.StopCast();

            Others.SafeSleep(500);
            SpellManager.CastSpellByIDAndPosition(116011, ObjectManager.Me.Position);
        }
        if (MySettings.UsePrismaticCrystal && PrismaticCrystal.IsSpellUsable && PrismaticCrystal.IsHostileDistanceGood)
        {
            if (ObjectManager.Me.IsCast)
                ObjectManager.Me.StopCast();

            Others.SafeSleep(500);
            SpellManager.CastSpellByIDAndPosition(152087, ObjectManager.Target.Position);
            Lua.RunMacroText("/target Prismatic Crystal");
        }
        if (MySettings.UseTimeWarp && !ObjectManager.Me.HaveBuff(80354) && !ObjectManager.Me.HaveBuff(57724) && !ObjectManager.Me.HaveBuff(57723) && !ObjectManager.Me.HaveBuff(95809)
            && TimeWarp.IsSpellUsable && Fireball.IsHostileDistanceGood)
        {
            if (ObjectManager.Me.IsCast)
                ObjectManager.Me.StopCast();

            Others.SafeSleep(500);
            TimeWarp.Cast();
        }
        if (MySettings.UseMeteor && Meteor.IsSpellUsable && Meteor.IsHostileDistanceGood)
        {
            if (ObjectManager.Me.IsCast)
                ObjectManager.Me.StopCast();

            Others.SafeSleep(1000);
            Meteor.Cast();
        }
        if (MySettings.UseCombustion && Ignite.TargetHaveBuff && Combustion.IsSpellUsable && Combustion.IsHostileDistanceGood)
        {
            if (ObjectManager.Me.IsCast)
                ObjectManager.Me.StopCast();

            Others.SafeSleep(1000);
            Combustion.Cast();
        }
    }

    private void DPSCycle()
    {
        Usefuls.SleepGlobalCooldown();
        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            if (MySettings.UseIceFloes && !IceFloes.HaveBuff && ObjectManager.Me.GetMove && IceFloes.IsSpellUsable && Fireball.IsHostileDistanceGood)
                IceFloes.Cast();

            if (MySettings.UseFlamestrike && Flamestrike.IsSpellUsable && ObjectManager.GetUnitInSpellRange(Flamestrike.MaxRangeHostile) > 4)
            {
                SpellManager.CastSpellByIDAndPosition(2120, ObjectManager.Target.Position);
                return;
            }
            if (MySettings.UsePyroblast && ((PyroblastBuff.HaveBuff && HeatingUp.HaveBuff) || ObjectManager.Me.UnitAura(48108).AuraTimeLeftInMs < 4000)
                && Pyroblast.IsSpellUsable && Pyroblast.IsHostileDistanceGood)
            {
                Pyroblast.Cast();
                return;
            }
            if (MySettings.UseLivingBomb && !LivingBomb.TargetHaveBuff && LivingBomb.IsSpellUsable && LivingBomb.IsHostileDistanceGood)
            {
                LivingBomb.Cast();
                return;
            }
            if (MySettings.UseInfernoBlast && HeatingUp.HaveBuff && !PyroblastBuff.HaveBuff && InfernoBlast.IsSpellUsable && InfernoBlast.IsHostileDistanceGood)
            {
                if (ObjectManager.Me.IsCast)
                    ObjectManager.Me.StopCast();

                Others.SafeSleep(1000);
                InfernoBlast.Cast();
                return;
            }
            if (MySettings.UseBlastWave && BlastWave.GetSpellCharges > 0 && BlastWave.IsSpellUsable && BlastWave.IsHostileDistanceGood)
            {
                BlastWave.Cast();
                return;
            }
            if (MySettings.UseDragonsBreath && DragonsBreath.IsSpellUsable && ObjectManager.Target.GetDistance < 12)
            {
                DragonsBreath.Cast();
                return;
            }
            if (MySettings.UseScorch && ObjectManager.Me.GetMove && !IceFloes.HaveBuff && Scorch.IsSpellUsable && Scorch.IsHostileDistanceGood)
            {
                Scorch.Cast();
                return;
            }
            if (MySettings.UseFireball && !Ignite.HaveBuff && Fireball.IsSpellUsable && Fireball.IsHostileDistanceGood)
            {
                Fireball.Launch(true, false, true);
                Others.SafeSleep(500);
                while (!Ignite.HaveBuff && ObjectManager.Me.IsCast)
                    Others.SafeSleep(20);

                if (ObjectManager.Me.IsCast)
                    ObjectManager.Me.StopCast();
                return;
            }
            if (MySettings.UsePyroblast && (!MySettings.UseFireball || !Fireball.KnownSpell) && Pyroblast.IsSpellUsable && Pyroblast.IsHostileDistanceGood)
            {
                Pyroblast.Cast();
                return;
            }
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    private void Patrolling()
    {
        if (ObjectManager.Me.IsMounted) return;
        Buff();
        Heal();
    }

    #region Nested type: MageFireSettings

    [Serializable]
    public class MageFireSettings : Settings
    {
        public bool DoAvoidMelee = false;
        public int DoAvoidMeleeDistance = 0;
        public bool UseAlchFlask = true;
        public bool UseAlterTime = true;
        public int UseAlterTimeAtPercentage = 80;
        public bool UseArcaneBrilliance = true;
        public bool UseArcaneTorrentForDecast = true;
        public int UseArcaneTorrentForDecastAtPercentage = 95;
        public bool UseArcaneTorrentForResource = true;
        public int UseArcaneTorrentForResourceAtPercentage = 80;
        public bool UseBerserking = true;
        public bool UseBlastWave = true;
        public bool UseBlazingSpeed = true;
        public bool UseBlink = true;
        public bool UseBloodFury = true;
        public bool UseColdSnapForDefence = true;
        public bool UseColdSnapForHeal = true;
        public int UseColdSnapForHealAtPercentage = 70;
        public bool UseCombustion = true;
        public bool UseConeofCold = true;
        public int UseConeofColdAtPercentage = 45;
        public bool UseConjureRefreshment = true;
        public bool UseCounterspell = true;
        public int UseCounterspellAtPercentage = 95;
        public bool UseDarkflight = true;
        public bool UseDragonsBreath = true;
        public bool UseEvanesce = true;
        public int UseEvanesceAtPercentage = 50;
        public bool UseEveryManforHimself = true;
        public bool UseFireball = true;
        public bool UseFlamestrike = true;
        public bool UseFrostNova = true;
        public int UseFrostNovaAtPercentage = 50;
        public bool UseFrostjaw = true;
        public int UseFrostjawAtPercentage = 40;
        public bool UseFrozenOrb = true;
        public bool UseGiftoftheNaaru = true;
        public int UseGiftoftheNaaruAtPercentage = 80;
        public bool UseGreaterInvisibility = true;
        public int UseGreaterInvisibilityAtPercentage = 10;
        public bool UseIceBarrier = true;
        public int UseIceBarrierAtPercentage = 95;
        public bool UseIceBlock = true;
        public bool UseIceFloes = false;
        public bool UseIceWard = true;
        public int UseIceWardAtPercentage = 45;
        public bool UseInfernoBlast = true;
        public bool UseInvisibility = true;
        public int UseInvisibilityAtPercentage = 10;
        public bool UseLivingBomb = true;
        public bool UseLowCombat = true;
        public int UseLowCombatAtPercentage = 15;
        public bool UseMeteor = true;
        public bool UseMirrorImage = true;
        public bool UsePrismaticCrystal = true;
        public bool UsePyroblast = true;
        public bool UseRingofFrost = true;
        public bool UseRuneofPower = true;
        public bool UseScorch = true;
        public bool UseStoneform = true;
        public int UseStoneformAtPercentage = 80;
        public bool UseTimeWarp = true;
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;

        public MageFireSettings()
        {
            ConfigWinForm("Mage Fire Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent for Interrupt", "UseArcaneTorrentForDecast", "Professions & Racials", "AtPercentage");
            AddControlInWinForm("Use Arcane Torrent for Resource", "UseArcaneTorrentForResource", "Professions & Racials", "AtPercentage");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Darkflight", "UseDarkflight", "Professions & Racials");
            AddControlInWinForm("Use Every Man for Himself", "UseEveryManforHimself", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials", "AtPercentage");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials", "AtPercentage");
            /* Mage Buffs */
            AddControlInWinForm("Use Arcane Brilliance", "UseArcaneBrilliance", "Mage Buffs");
            AddControlInWinForm("Use Blazing Speed", "UseBlazingSpeed", "Mage Buffs");
            AddControlInWinForm("Use Ice Floes", "UseIceFloes", "Mage Buffs");
            AddControlInWinForm("Use Rune of Power", "UseRuneofPower", "Mage Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Dragon's Breath", "UseDragonsBreath", "Offensive Spell");
            AddControlInWinForm("Use Fireball", "UseFireball", "Offensive Spell");
            AddControlInWinForm("Use Flamestrike", "UseFlamestrike", "Offensive Spell");
            AddControlInWinForm("Use Inferno Blast", "UseInfernoBlast", "Offensive Spell");
            AddControlInWinForm("Use Pyroblast", "UsePyroblast", "Offensive Spell");
            AddControlInWinForm("Use Scorch", "UseScorch", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use Blast Wave", "UseBlastWave", "Offensive Cooldown");
            AddControlInWinForm("Use Combustion", "UseCombustion", "Offensive Cooldown");
            AddControlInWinForm("Use Living Bomb", "UseLivingBomb", "Offensive Cooldown");
            AddControlInWinForm("Use Meteor", "UseMeteor", "Offensive Cooldown");
            AddControlInWinForm("Use Mirror Image", "UseMirrorImage", "Offensive Cooldown");
            AddControlInWinForm("Use Prismatic Crystal", "UsePrismaticCrystal", "Offensive Cooldown");
            AddControlInWinForm("Use Time Warp", "UseTimeWarp", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Alter Time", "UseAlterTime", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Blink", "UseBlink", "Defensive Cooldown");
            AddControlInWinForm("Use Cold Snap", "UseColdSnap", "Defensive Cooldown");
            AddControlInWinForm("Use Cone of Cold", "UseConeofCold", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Counterspell", "UseCounterspell", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Evanesce", "UseEvanesce", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Frostjaw", "UseFrostjaw", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Frost Nova", "UseFrostNova", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Greater Invisibility", "UseGreaterInvisibility", "Defensive Cooldown");
            AddControlInWinForm("Use Ice Barrier", "UseIceBarrier", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Ice Block", "UseIceBlock", "Defensive Cooldown");
            AddControlInWinForm("Use Ice Ward", "UseIceWard", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Invisibility", "UseInvisibility", "Defensive Cooldown");
            AddControlInWinForm("Use Ring of Frost", "UseRingofFrost", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Cold SnapForHeal", "UseColdSnap", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Conjure Refreshment", "UseConjureRefreshment", "Healing Spell");
            /* Game Settings */
            AddControlInWinForm("Use Low Combat Settings", "UseLowCombat", "Game Settings");
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
            AddControlInWinForm("Do avoid melee (Off Advised!!)", "DoAvoidMelee", "Game Settings");
            AddControlInWinForm("Avoid melee distance (1 to 4)", "DoAvoidMeleeDistance", "Game Settings");
        }

        public static MageFireSettings CurrentSetting { get; set; }

        public static MageFireSettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Mage_Fire.xml";
            if (File.Exists(currentSettingsFile))
            {
                return
                    CurrentSetting = Load<MageFireSettings>(currentSettingsFile);
            }
            return new MageFireSettings();
        }
    }

    #endregion
}

#endregion

// ReSharper restore ObjectCreationAsStatement
// ReSharper restore EmptyGeneralCatchClause