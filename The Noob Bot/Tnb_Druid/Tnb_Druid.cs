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
                    #region Druid Specialisation checking

                case WoWClass.Druid:

                    if (wowSpecialization == WoWSpecialization.DruidFeral || wowSpecialization == WoWSpecialization.None)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Druid_Feral.xml";
                            var currentSetting = new DruidFeral.DruidFeralSettings();
                            if (File.Exists(currentSettingsFile) && !resetSettings)
                            {
                                currentSetting = Settings.Load<DruidFeral.DruidFeralSettings>(currentSettingsFile);
                            }
                            currentSetting.ToForm();
                            currentSetting.Save(currentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Druid Feral Found");
                            EquipmentAndStats.SetPlayerSpe(WoWSpecialization.DruidFeral);
                            new DruidFeral();
                        }
                        break;
                    }
                    if (wowSpecialization == WoWSpecialization.DruidGuardian)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Druid_Guardian.xml";
                            var currentSetting = new DruidGuardian.DruidGuardianSettings();
                            if (File.Exists(currentSettingsFile) && !resetSettings)
                            {
                                currentSetting = Settings.Load<DruidGuardian.DruidGuardianSettings>(currentSettingsFile);
                            }
                            currentSetting.ToForm();
                            currentSetting.Save(currentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Feral Guardian Found");
                            EquipmentAndStats.SetPlayerSpe(WoWSpecialization.DruidGuardian);
                            new DruidGuardian();
                        }
                        break;
                    }
                    if (wowSpecialization == WoWSpecialization.DruidBalance)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Druid_Balance.xml";
                            var currentSetting = new DruidBalance.DruidBalanceSettings();
                            if (File.Exists(currentSettingsFile) && !resetSettings)
                            {
                                currentSetting = Settings.Load<DruidBalance.DruidBalanceSettings>(currentSettingsFile);
                            }
                            currentSetting.ToForm();
                            currentSetting.Save(currentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Druid Balance Found");
                            InternalRange = 30.0f;
                            EquipmentAndStats.SetPlayerSpe(WoWSpecialization.DruidBalance);
                            new DruidBalance();
                        }
                        break;
                    }
                    if (wowSpecialization == WoWSpecialization.DruidRestoration)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Druid_Restoration.xml";
                            var currentSetting = new DruidRestoration.DruidRestorationSettings();
                            if (File.Exists(currentSettingsFile) && !resetSettings)
                            {
                                currentSetting = Settings.Load<DruidRestoration.DruidRestorationSettings>(currentSettingsFile);
                            }
                            currentSetting.ToForm();
                            currentSetting.Save(currentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Druid Restoration Found");
                            InternalRange = 30.0f;
                            EquipmentAndStats.SetPlayerSpe(WoWSpecialization.DruidRestoration);
                            new DruidRestoration();
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

#region Druid

public class DruidBalance
{
    private static DruidBalanceSettings MySettings = DruidBalanceSettings.GetSettings();

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

    public readonly Spell Berserking = new Spell("Berserking");
    public readonly Spell Darkflight = new Spell("Darkflight");
    public readonly Spell WarStomp = new Spell("War Stomp");

    #endregion

    #region Druid Buffs

    public readonly Spell BalanceofPower = new Spell("Balance of Power");
    public readonly Spell Dash = new Spell("Dash");
    public readonly Spell DisplacerBeast = new Spell("Displacer Beast");
    public readonly Spell Euphoria = new Spell("Euphoria");
    public readonly Spell LunarEmpowerment = new Spell(164547);
    public readonly Spell LunarPeak = new Spell(171743);
    public readonly Spell MarkoftheWild = new Spell("Mark of the Wild");
    public readonly Spell MoonfireDebuff = new Spell(164812);
    public readonly Spell MoonkinForm = new Spell("Moonkin Form");
    public readonly Spell SolarEmpowerment = new Spell(164545);
    public readonly Spell SolarPeak = new Spell(171744);
    public readonly Spell StampedingRoar = new Spell("Stampeding Roar");
    public readonly Spell SunfireDebuff = new Spell(164815);
    private Timer _moonfireTimer = new Timer(0);
    public bool PeakTracker = true;

    #endregion

    #region Offensive Spell

    public readonly Spell Hurricane = new Spell("Hurricane");
    public readonly Spell Moonfire = new Spell("Moonfire");
    public readonly Spell Starfall = new Spell("Starfall");
    public readonly Spell Starfire = new Spell("Starfire");
    public readonly Spell Starsurge = new Spell("Starsurge");
    public readonly Spell StellarFlare = new Spell("Stellar Flare");
    public readonly Spell Sunfire = new Spell(93402);
    public readonly Spell Wrath = new Spell("Wrath");
    private Timer _stellarFlareTimer = new Timer(0);

    #endregion

    #region Offensive Cooldown

    public readonly Spell CelestialAlignment = new Spell("Celestial Alignment");
    public readonly Spell ForceofNature = new Spell("Force of Nature");
    public readonly Spell Incarnation = new Spell("Incarnation: Chosen of Elune");

    #endregion

    #region Defensive Cooldown

    public readonly Spell Barkskin = new Spell("Barkskin");
    public readonly Spell EntanglingRoots = new Spell("Entangling Roots");
    public readonly Spell IncapacitatingRoar = new Spell("Incapacitating Roar");
    public readonly Spell MassEntanglement = new Spell("Mass Entanglement");
    public readonly Spell MightyBash = new Spell("Mighty Bash");
    public readonly Spell SolarBeam = new Spell("Solar Beam");
    public readonly Spell Typhoon = new Spell("Typhoon");
    public readonly Spell UrsolsVortex = new Spell("Ursol's Vortex");
    public readonly Spell WildCharge = new Spell("Wild Charge");
    public readonly Spell WildMushroom = new Spell("Wild Mushroom");

    #endregion

    #region Healing Spell

    public readonly Spell CenarionWard = new Spell("Cenarion Ward");
    public readonly Spell HealingTouch = new Spell("Healing Touch");
    public readonly Spell HeartoftheWild = new Spell("Heart of the Wild");
    public readonly Spell NaturesVigil = new Spell("Nature's Vigil");
    public readonly Spell Rejuvenation = new Spell("Rejuvenation");
    public readonly Spell Renewal = new Spell("Renewal");

    #endregion

    public DruidBalance()
    {
        Main.InternalRange = 39f;
        Main.InternalAggroRange = 39f;
        MySettings = DruidBalanceSettings.GetSettings();
        Main.DumpCurrentSettings<DruidBalanceSettings>(MySettings);
        UInt128 lastTarget = 0;
        LowHP();

        while (Main.InternalLoop)
        {
            try
            {
                if (!ObjectManager.Me.IsMounted)
                {
                    if (Fight.InFight && ObjectManager.Me.Target > 0)
                    {
                        if (ObjectManager.Me.Target != lastTarget
                            && (Wrath.IsHostileDistanceGood || Starfire.IsHostileDistanceGood))
                        {
                            Pull();
                            lastTarget = ObjectManager.Me.Target;
                        }

                        if (MySettings.UseLowCombat && (ObjectManager.Me.Level - ObjectManager.Target.Level >= MySettings.UseLowCombatAtPercentage))
                        {
                            if (CombatClass.InSpellRange(ObjectManager.Target, 0, Main.InternalRange))
                                LowCombat();
                        }
                        else
                        {
                            if (CombatClass.InSpellRange(ObjectManager.Target, 0, Main.InternalRange))
                                Combat();
                        }
                    }
                    if (!ObjectManager.Me.IsCast)
                        Patrolling();
                }
            }
            catch
            {
            }
            Thread.Sleep(100);
        }
    }

    private void LowHP()
    {
        if (MySettings.UseBarkskinAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseBarkskinAtPercentage;

        if (MySettings.UseIncapacitatingRoarAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseIncapacitatingRoarAtPercentage;

        if (MySettings.UseMassEntanglementAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseMassEntanglementAtPercentage;

        if (MySettings.UseMightyBashAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseMightyBashAtPercentage;

        if (MySettings.UseTyphoonAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseTyphoonAtPercentage;

        if (MySettings.UseUrsolsVortexAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseUrsolsVortexAtPercentage;

        if (MySettings.UseWarStompAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseWarStompAtPercentage;

        if (MySettings.UseSolarBeamAtPercentage > DecastHP)
            DecastHP = MySettings.UseSolarBeamAtPercentage;

        if (MySettings.UseCenarionWardAtPercentage > HealHP)
            HealHP = MySettings.UseCenarionWardAtPercentage;

        if (MySettings.UseHealingTouchAtPercentage > HealHP)
            HealHP = MySettings.UseHealingTouchAtPercentage;

        if (MySettings.UseHeartoftheWildAtPercentage > HealHP)
            HealHP = MySettings.UseHeartoftheWildAtPercentage;

        if (MySettings.UseNaturesVigilAtPercentage > HealHP)
            HealHP = MySettings.UseNaturesVigilAtPercentage;

        if (MySettings.UseRejuvenationAtPercentage > HealHP)
            HealHP = MySettings.UseRejuvenationAtPercentage;

        if (MySettings.UseRenewalAtPercentage > HealHP)
            HealHP = MySettings.UseRenewalAtPercentage;
    }

    private void Pull()
    {
        if (MySettings.UseStarfire && ObjectManager.Me.Eclipse <= 0 && Starfire.IsSpellUsable && Starfire.IsHostileDistanceGood)
        {
            Starfire.Cast();
            return;
        }
        if (MySettings.UseWrath && ObjectManager.Me.Eclipse > 0 && Wrath.IsSpellUsable && Wrath.IsHostileDistanceGood)
            Wrath.Cast();
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

        if (MySettings.UseStarsurge && Starsurge.GetSpellCharges > 0 && Starsurge.IsSpellUsable && Starsurge.IsHostileDistanceGood)
        {
            Starsurge.Cast();
            return;
        }
        if (MySettings.UseMoonfire && ObjectManager.Me.Eclipse <= 0 && !MoonfireDebuff.TargetHaveBuff && Moonfire.IsHostileDistanceGood && Moonfire.IsSpellUsable)
        {
            Moonfire.Cast();
            return;
        }
        if (MySettings.UseSunfire && ObjectManager.Me.Eclipse > 0 && !Sunfire.TargetHaveBuff && !SpellManager.IsSpellOnCooldown(164815) && Moonfire.IsHostileDistanceGood)
        {
            Sunfire.Cast();
            return;
        }
        if (MySettings.UseStarfall && Starfall.GetSpellCharges > 0 && Starfall.IsSpellUsable && ObjectManager.GetUnitInSpellRange(8 /*, ObjectManager.Target*/) > 1)
        {
            Starfall.Cast();
            return;
        }
        if (MySettings.UseHurricane && Hurricane.IsSpellUsable && ObjectManager.GetUnitInSpellRange(8 /*, ObjectManager.Target*/) > 3 && Hurricane.IsHostileDistanceGood)
        {
            SpellManager.CastSpellByIDAndPosition(16914, ObjectManager.Target.Position);
            return;
        }
        if (MySettings.UseStarfire && MoonfireDebuff.TargetHaveBuff && ObjectManager.Me.Eclipse < -30 && Starfire.IsSpellUsable && Starfire.IsHostileDistanceGood)
        {
            Starfire.Cast();
            return;
        }
        if (MySettings.UseWrath && Sunfire.TargetHaveBuff && ObjectManager.Me.Eclipse > 20 && Wrath.IsSpellUsable && Wrath.IsHostileDistanceGood)
            Wrath.Cast();
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

        if (ObjectManager.Me.HealthPercent <= DecastHP)
            Decast();

        DPSBurst();
        DPSCycle();
    }

    private void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (MySettings.UseDarkflight && Darkflight.KnownSpell && !Dash.HaveBuff && !StampedingRoar.HaveBuff && ObjectManager.Me.GetMove && !ObjectManager.Target.InCombat && Darkflight.IsSpellUsable)
            Darkflight.Cast();

        if (MySettings.UseDash && !Darkflight.HaveBuff && !StampedingRoar.HaveBuff && ObjectManager.Me.GetMove && !ObjectManager.Target.InCombat && Dash.IsSpellUsable)
            Dash.Cast();

        if (MySettings.UseDisplacerBeast && !ObjectManager.Me.InCombat && ObjectManager.Me.GetMove && !Dash.HaveBuff && !StampedingRoar.HaveBuff
            && DisplacerBeast.IsSpellUsable)
            DisplacerBeast.Cast();

        if (MySettings.UseMarkoftheWild && !MarkoftheWild.HaveBuff && MarkoftheWild.IsSpellUsable)
            MarkoftheWild.Cast();

        if (MySettings.UseStampedingRoar && !Darkflight.HaveBuff && !Dash.HaveBuff && ObjectManager.Me.GetMove && !ObjectManager.Target.InCombat && StampedingRoar.IsSpellUsable)
            StampedingRoar.Cast();

        if (MySettings.UseMoonkinForm && !MoonkinForm.HaveBuff && (!Dash.HaveBuff || !Darkflight.HaveBuff || ObjectManager.Target.InCombat) && MoonkinForm.KnownSpell)
        {
            if (ObjectManager.Me.IsCast)
                ObjectManager.Me.StopCast();

            Others.SafeSleep(1000);
            MoonkinForm.Cast();
        }

        if (MySettings.UseAlchFlask && !ObjectManager.Me.HaveBuff(79638) && !ObjectManager.Me.HaveBuff(79640) && !ObjectManager.Me.HaveBuff(79639)
            && !ItemsManager.IsItemOnCooldown(75525) && ItemsManager.GetItemCount(75525) > 0)
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
        if (MySettings.UseBarkskin && ObjectManager.Me.HealthPercent <= MySettings.UseBarkskinAtPercentage && Barkskin.IsSpellUsable)
        {
            if (ObjectManager.Me.IsCast)
                ObjectManager.Me.StopCast();

            Others.SafeSleep(1000);
            Barkskin.Cast();
            _onCd = new Timer(1000*12);
            return;
        }
        if (MySettings.UseIncapacitatingRoar && (ObjectManager.Me.HealthPercent <= MySettings.UseIncapacitatingRoarAtPercentage
                                                 || ObjectManager.GetUnitInSpellRange(10 /*, ObjectManager.Me*/) > 2) && IncapacitatingRoar.IsSpellUsable)
        {
            if (ObjectManager.Me.IsCast)
                ObjectManager.Me.StopCast();

            Others.SafeSleep(1000);
            IncapacitatingRoar.Cast();
            _onCd = new Timer(1000*3);

            if (MySettings.UseWildCharge && WildCharge.IsSpellUsable && ObjectManager.GetUnitInSpellRange(5 /*, ObjectManager.Me*/) > 0)
                WildCharge.Cast();
            else if (MySettings.UseDisplacerBeast && DisplacerBeast.IsSpellUsable && ObjectManager.GetUnitInSpellRange(5 /*, ObjectManager.Me*/) > 0)
                DisplacerBeast.Cast();
            else
            {
                Logging.WriteFight("Defensively Moving Away");
                var maxTimeTimer = new Timer(1000*3);
                MovementsAction.MoveBackward(true);
                if (ObjectManager.Target.InCombat && !maxTimeTimer.IsReady && IncapacitatingRoar.TargetHaveBuff && ObjectManager.GetUnitInSpellRange(5 /*, ObjectManager.Me*/) > 0)
                {
                    Logging.WriteFight("Defensively Moving Away");
                    Others.SafeSleep(3000);
                }
                MovementsAction.MoveBackward(false);
                if (maxTimeTimer.IsReady && ObjectManager.Target.InCombat && ObjectManager.GetUnitInSpellRange(5 /*, ObjectManager.Me*/) > 2)
                {
                    MovementsAction.MoveForward(true);
                    Others.SafeSleep(3000);
                    MovementsAction.MoveForward(false);
                    MovementManager.Face(ObjectManager.Target.Position);
                }
            }

            return;
        }
        if (MySettings.UseMassEntanglement && (ObjectManager.Me.HealthPercent <= MySettings.UseMassEntanglementAtPercentage
                                               || ObjectManager.GetUnitInSpellRange(10 /*, ObjectManager.Me*/) > 2) && MassEntanglement.IsSpellUsable)
        {
            if (ObjectManager.Me.IsCast)
                ObjectManager.Me.StopCast();

            Others.SafeSleep(1000);
            MassEntanglement.Cast();
            _onCd = new Timer(1000*3);

            if (MySettings.UseWildCharge && WildCharge.IsSpellUsable && ObjectManager.GetUnitInSpellRange(5 /*, ObjectManager.Me*/) > 0)
                WildCharge.Cast();
            else if (MySettings.UseDisplacerBeast && DisplacerBeast.IsSpellUsable && ObjectManager.GetUnitInSpellRange(5 /*, ObjectManager.Me*/) > 0)
                DisplacerBeast.Cast();
            else
            {
                Logging.WriteFight("Defensively Moving Away");
                var maxTimeTimer = new Timer(1000*3);
                MovementsAction.MoveBackward(true);
                if (ObjectManager.Target.GetDistance < 10 && ObjectManager.Target.InCombat && !maxTimeTimer.IsReady && MassEntanglement.TargetHaveBuff)
                {
                    Logging.WriteFight("Defensively Moving Away");
                    Others.SafeSleep(3000);
                }
                MovementsAction.MoveBackward(false);
                if (maxTimeTimer.IsReady && ObjectManager.Target.InCombat && ObjectManager.GetUnitInSpellRange(5 /*, ObjectManager.Me*/) > 2)
                {
                    MovementsAction.MoveForward(true);
                    Others.SafeSleep(3000);
                    MovementsAction.MoveForward(false);
                    MovementManager.Face(ObjectManager.Target.Position);
                }
            }

            return;
        }
        if (MySettings.UseMightyBash && ObjectManager.Me.HealthPercent <= MySettings.UseMightyBashAtPercentage && MightyBash.IsSpellUsable && MightyBash.IsHostileDistanceGood)
        {
            if (ObjectManager.Me.IsCast)
                ObjectManager.Me.StopCast();

            Others.SafeSleep(1000);
            MightyBash.Cast();
            _onCd = new Timer(1000*5);
            return;
        }
        if (MySettings.UseTyphoon && ObjectManager.Me.HealthPercent <= MySettings.UseTyphoonAtPercentage && Typhoon.IsSpellUsable && Typhoon.IsHostileDistanceGood)
        {
            if (ObjectManager.Me.IsCast)
                ObjectManager.Me.StopCast();

            Others.SafeSleep(1000);
            Typhoon.Cast();
            _onCd = new Timer(1000*6);
            return;
        }
        if (MySettings.UseUrsolsVortex && ObjectManager.Me.HealthPercent <= MySettings.UseUrsolsVortexAtPercentage && UrsolsVortex.IsSpellUsable
            && UrsolsVortex.IsHostileDistanceGood)
        {
            if (ObjectManager.Me.IsCast)
                ObjectManager.Me.StopCast();

            Others.SafeSleep(1000);
            SpellManager.CastSpellByIDAndPosition(102793, ObjectManager.Me.Position);

            if (MySettings.UseWildCharge && WildCharge.IsSpellUsable && ObjectManager.GetUnitInSpellRange(5 /*, ObjectManager.Me*/) > 0)
                WildCharge.Cast();
            else if (MySettings.UseDisplacerBeast && DisplacerBeast.IsSpellUsable && ObjectManager.GetUnitInSpellRange(5 /*, ObjectManager.Me*/) > 0)
                DisplacerBeast.Cast();
            else
            {
                Logging.WriteFight("Defensively Moving Away");
                var maxTimeTimer = new Timer(1000*4);
                MovementsAction.MoveBackward(true);
                while (ObjectManager.Target.GetDistance < 10 && ObjectManager.Target.InCombat && !maxTimeTimer.IsReady && UrsolsVortex.TargetHaveBuff)
                    Others.SafeSleep(300);
                MovementsAction.MoveBackward(false);
                if (maxTimeTimer.IsReady && ObjectManager.Target.GetDistance < 10 && ObjectManager.Target.InCombat && UrsolsVortex.TargetHaveBuff)
                {
                    MovementsAction.MoveForward(true);
                    Others.SafeSleep(3000);
                    MovementsAction.MoveForward(false);
                    MovementManager.Face(ObjectManager.Target.Position);
                }
            }

            return;
        }
        if (MySettings.UseWarStomp && ObjectManager.Me.HealthPercent <= MySettings.UseWarStompAtPercentage && WarStomp.IsSpellUsable
            && ObjectManager.GetUnitInSpellRange(WarStomp.MaxRangeHostile) > 0)
        {
            if (ObjectManager.Me.IsCast)
                ObjectManager.Me.StopCast();

            Others.SafeSleep(1000);
            WarStomp.Cast();
            _onCd = new Timer(1000*2);
        }
        if (MySettings.UseWildMushroom && ObjectManager.Target.GetMove && WildMushroom.IsSpellUsable && WildMushroom.IsHostileDistanceGood)
        {
            if (ObjectManager.Me.IsCast)
                ObjectManager.Me.StopCast();

            Others.SafeSleep(1000);
            SpellManager.CastSpellByIDAndPosition(88747, ObjectManager.Target.Position);
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (MySettings.UseCenarionWard && ObjectManager.Me.HealthPercent <= MySettings.UseCenarionWardAtPercentage && CenarionWard.IsSpellUsable)
        {
            CenarionWard.Cast();
            return;
        }
        if (MySettings.UseHealingTouch && ObjectManager.Me.HealthPercent <= MySettings.UseHealingTouchAtPercentage && HealingTouch.IsSpellUsable)
        {
            HealingTouch.Cast();
            return;
        }
        if (MySettings.UseHeartoftheWild && ObjectManager.Me.HealthPercent <= MySettings.UseHeartoftheWildAtPercentage && HeartoftheWild.IsSpellUsable)
        {
            HeartoftheWild.Cast();
            return;
        }
        if (MySettings.UseNaturesVigil && ObjectManager.Me.HealthPercent <= MySettings.UseNaturesVigilAtPercentage && NaturesVigil.IsSpellUsable)
        {
            NaturesVigil.Cast();
            return;
        }
        if (MySettings.UseRejuvenation && ObjectManager.Me.HealthPercent <= MySettings.UseHealingTouchAtPercentage && !Rejuvenation.HaveBuff && Rejuvenation.IsSpellUsable)
        {
            Rejuvenation.Cast();
            return;
        }
        if (MySettings.UseRenewal && ObjectManager.Me.HealthPercent <= MySettings.UseRenewalAtPercentage && Renewal.IsSpellUsable)
        {
            Renewal.Cast();
            return;
        }
    }

    private void Decast()
    {
        if (MySettings.UseSolarBeam && ObjectManager.Me.HealthPercent <= MySettings.UseSolarBeamAtPercentage
            && ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && SolarBeam.IsSpellUsable && SolarBeam.IsHostileDistanceGood)
        {
            if (MySettings.UseEntanglingRoots && EntanglingRoots.IsSpellUsable && EntanglingRoots.IsHostileDistanceGood)
            {
                EntanglingRoots.Cast();
                Others.SafeSleep(1000);
            }

            SpellManager.CastSpellByIDAndPosition(78675, ObjectManager.Target.Position);
        }
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
        if (MySettings.UseBerserking && Berserking.IsSpellUsable && Starfire.IsHostileDistanceGood)
            Berserking.Cast();

        if (MySettings.UseForceofNature && (ForceofNature.GetSpellCharges > 1 || (CelestialAlignment.HaveBuff && ForceofNature.GetSpellCharges > 0))
            && ForceofNature.IsSpellUsable && ForceofNature.IsHostileDistanceGood)
            ForceofNature.Cast();

        if (MySettings.UseIncarnation && Incarnation.IsSpellUsable && Starfire.IsHostileDistanceGood)
        {
            Incarnation.Cast();
            Others.SafeSleep(1000);
        }
        if (MySettings.UseCelestialAlignment && ObjectManager.Me.Eclipse < 0 && CelestialAlignment.IsSpellUsable && Starfire.IsHostileDistanceGood)
        {
            if (MySettings.UseIncarnation && Incarnation.KnownSpell && !Incarnation.HaveBuff && Incarnation.IsSpellUsable)
                return;
            else
            {
                CelestialAlignment.Cast();
                CelestialAlignmentCombat();
            }
        }
    }

    private void DPSCycle()
    {
        Usefuls.SleepGlobalCooldown();
        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            if (LunarPeak.HaveBuff)
                PeakTracker = false;

            if (SolarPeak.HaveBuff)
                PeakTracker = true;

            if (MySettings.UseMoonfire && Euphoria.KnownSpell && ObjectManager.Me.Eclipse <= 0 && (!MoonfireDebuff.TargetHaveBuff
                                                                                                   || (LunarPeak.HaveBuff && _moonfireTimer.IsReady)) && Moonfire.IsSpellUsable && Moonfire.IsHostileDistanceGood)
            {
                Moonfire.Cast();
                _moonfireTimer = new Timer(1000*30);
                return;
            }
            else if (MySettings.UseMoonfire && !Euphoria.KnownSpell && ObjectManager.Me.Eclipse <= 0 && (!MoonfireDebuff.TargetHaveBuff || (LunarPeak.HaveBuff && !BalanceofPower.KnownSpell))
                     && Moonfire.IsSpellUsable && Moonfire.IsHostileDistanceGood)
            {
                Moonfire.Cast();
                return;
            }

            if (MySettings.UseSunfire && Euphoria.KnownSpell && ObjectManager.Me.Eclipse > 0 && (!Sunfire.TargetHaveBuff
                                                                                                 || SolarPeak.HaveBuff) && !SpellManager.IsSpellOnCooldown(164815) && Moonfire.IsHostileDistanceGood)
            {
                Sunfire.Cast();
                return;
            }
            else if (MySettings.UseSunfire && !Euphoria.KnownSpell && ObjectManager.Me.Eclipse > 0 && (!Sunfire.TargetHaveBuff || (SolarPeak.HaveBuff && !BalanceofPower.KnownSpell)
                                                                                                       || ObjectManager.Me.Eclipse <= 20) && !SpellManager.IsSpellOnCooldown(164815) && Moonfire.IsHostileDistanceGood)
            {
                Sunfire.Cast();
                return;
            }

            if (MySettings.UseStellarFlare && _stellarFlareTimer.IsReady && (!StellarFlare.TargetHaveBuff || ObjectManager.Me.Eclipse == 0 || ObjectManager.Target.UnitAura(152221).AuraTimeLeftInMs < 3000)
                && StellarFlare.IsSpellUsable && StellarFlare.IsHostileDistanceGood)
            {
                StellarFlare.Cast();
                _stellarFlareTimer = new Timer(1000*16);
                return;
            }
            if (MySettings.UseStarsurge &&
                (Starsurge.GetSpellCharges > 2 || (Starsurge.GetSpellCharges > 0 && (MoonfireDebuff.TargetHaveBuff || ObjectManager.Me.Eclipse > 0) && (Sunfire.TargetHaveBuff || ObjectManager.Me.Eclipse <= 0))
                 && ((ObjectManager.Me.UnitAura(164547).AuraTimeLeftInMs < 1 && ObjectManager.Me.Eclipse <= 0) || ObjectManager.Me.UnitAura(164545).AuraTimeLeftInMs < 1 && ObjectManager.Me.Eclipse > 0))
                && Starsurge.IsSpellUsable && Starsurge.IsHostileDistanceGood)
            {
                Starsurge.Cast();
                return;
            }
            if (MySettings.UseStarfall && (Starfall.GetSpellCharges > 2 || (Starfall.GetSpellCharges > 0 && ObjectManager.Me.Eclipse <= 0))
                && Starfall.IsSpellUsable && ObjectManager.GetUnitInSpellRange(Starfall.MaxRangeHostile) > 1)
            {
                Starfall.Cast();
                return;
            }
            if (MySettings.UseStarfire && MoonfireDebuff.TargetHaveBuff && ((ObjectManager.Me.Eclipse <= 39 && PeakTracker)
                                                                            || (ObjectManager.Me.Eclipse <= -60 && !PeakTracker)) && Starfire.IsSpellUsable && Starfire.IsHostileDistanceGood)
            {
                Starfire.Cast(true, false, true);
                return;
            }
            if (MySettings.UseWrath && Sunfire.TargetHaveBuff && ((ObjectManager.Me.Eclipse > -59 && !PeakTracker)
                                                                  || ObjectManager.Me.Eclipse > 39 && PeakTracker) && Wrath.IsSpellUsable && Wrath.IsHostileDistanceGood)
            {
                Wrath.Cast(true, false, true);
                return;
            }
            if (ObjectManager.Me.Level < 10)
            {
                if (MySettings.UseMoonfire && !MoonfireDebuff.TargetHaveBuff && Moonfire.IsSpellUsable && Moonfire.IsHostileDistanceGood)
                {
                    Moonfire.Cast();
                    return;
                }

                if (MySettings.UseWrath && !MoonfireDebuff.TargetHaveBuff && Wrath.IsSpellUsable && Wrath.IsHostileDistanceGood)
                {
                    Wrath.Cast(true, false, true);
                    return;
                }
            }
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    public void CelestialAlignmentCombat()
    {
        while (CelestialAlignment.HaveBuff)
        {
            if (!MoonfireDebuff.TargetHaveBuff || !Sunfire.TargetHaveBuff || ObjectManager.Me.UnitAura(112071).AuraTimeLeftInMs < 1000)
                Moonfire.Cast();

            if (MySettings.UseStarsurge && Starsurge.GetSpellCharges > 0 && ObjectManager.Me.UnitAura(164547).AuraTimeLeftInMs < 1 && Starsurge.IsSpellUsable && Starsurge.IsHostileDistanceGood)
                Starsurge.Cast();

            if (MySettings.UseStarfire && MoonfireDebuff.TargetHaveBuff && (ObjectManager.Me.UnitAura(164547).AuraTimeLeftInMs > 0 || Starsurge.GetSpellCharges < 1)
                && Starfire.IsSpellUsable && Starfire.IsHostileDistanceGood)
                Starfire.Cast();

            Others.SafeSleep(1000);
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

    #region Nested type: DruidBalanceSettings

    [Serializable]
    public class DruidBalanceSettings : Settings
    {
        public bool DoAvoidMelee = false;
        public int DoAvoidMeleeDistance = 0;
        public bool UseAlchFlask = true;
        public bool UseBarkskin = true;
        public int UseBarkskinAtPercentage = 75;
        public bool UseBerserking = true;
        public bool UseCelestialAlignment = true;
        public bool UseCenarionWard = true;
        public int UseCenarionWardAtPercentage = 85;
        public bool UseDarkflight = true;
        public bool UseDash = true;
        public bool UseDisplacerBeast = true;
        public bool UseEntanglingRoots = true;
        public bool UseForceofNature = true;
        public bool UseHealingTouch = true;
        public int UseHealingTouchAtPercentage = 60;
        public bool UseHeartoftheWild = true;
        public int UseHeartoftheWildAtPercentage = 60;
        public bool UseHurricane = true;
        public bool UseIncapacitatingRoar = true;
        public int UseIncapacitatingRoarAtPercentage = 50;
        public bool UseIncarnation = true;
        public bool UseInnervate = true;
        public bool UseLowCombat = true;
        public int UseLowCombatAtPercentage = 15;
        public bool UseMarkoftheWild = true;
        public bool UseMassEntanglement = true;
        public int UseMassEntanglementAtPercentage = 50;
        public bool UseMightyBash = true;
        public int UseMightyBashAtPercentage = 80;
        public bool UseMoonfire = true;
        public bool UseMoonkinForm = true;
        public bool UseNaturesVigil = false;
        public int UseNaturesVigilAtPercentage = 80;
        public bool UseRejuvenation = true;
        public int UseRejuvenationAtPercentage = 80;
        public bool UseRenewal = true;
        public int UseRenewalAtPercentage = 65;
        public bool UseSolarBeam = true;
        public int UseSolarBeamAtPercentage = 95;
        public bool UseStampedingRoar = true;
        public bool UseStarfall = true;
        public bool UseStarfire = true;
        public bool UseStarsurge = true;
        public bool UseStellarFlare = true;
        public bool UseSunfire = true;
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;
        public bool UseTyphoon = true;
        public int UseTyphoonAtPercentage = 50;
        public bool UseUrsolsVortex = true;
        public int UseUrsolsVortexAtPercentage = 50;
        public bool UseWarStomp = true;
        public int UseWarStompAtPercentage = 80;
        public bool UseWildCharge = true;
        public bool UseWildMushroom = true;
        public bool UseWrath = true;

        public DruidBalanceSettings()
        {
            ConfigWinForm("Druid Balance Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Darkflight", "UseDarkflight", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            /* Druid Buffs */
            AddControlInWinForm("Use Dash", "UseDash", "Druid Buffs");
            AddControlInWinForm("Use Displacer Beast", "UseDisplacerBeast", "Druid Buffs");
            AddControlInWinForm("Use Mark of the Wild", "UseMarkoftheWild", "Druid Buffs");
            AddControlInWinForm("Use Moonkin Form", "UseMoonkinForm", "Druid Buffs");
            AddControlInWinForm("Use Stampeding Roar", "UseStampedingRoar", "Druid Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Hurricane", "UseHurricane", "Offensive Spell");
            AddControlInWinForm("Use Moonfire", "UseMoonfire", "Offensive Spell");
            AddControlInWinForm("Use Starfall", "UseStarfall", "Offensive Spell");
            AddControlInWinForm("Use Starfire", "UseStarfire", "Offensive Spell");
            AddControlInWinForm("Use Starsurge", "UseStarsurge", "Offensive Spell");
            AddControlInWinForm("Use Stellar Flare", "UseStellarFlare", "Offensive Spell");
            AddControlInWinForm("Use Sunfire", "UseSunfire", "Offensive Spell");
            AddControlInWinForm("Use Wrath", "UseWrath", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use Celestial Alignment", "UseCelestialAlignment", "Offensive Cooldown");
            AddControlInWinForm("Use Force of Nature", "UseForceofNature", "Offensive Cooldown");
            AddControlInWinForm("Use Incarnation: Chosen of Elune", "UseIncarnation", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Barkskin", "UseBarkskin", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Entangling Roots", "UseEntanglingRoots", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Incapacitating Roar", "UseIncapacitatingRoar", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Mass Entanglement", "UseMassEntanglement", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Mighty Bash", "UseMightyBash", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Solar Beam", "UseSolarBeam", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Typhoon", "UseTyphoon", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Ursol's Vortex", "UseUrsolsVortex", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Wild Charge", "UseWildCharge", "Defensive Cooldown");
            AddControlInWinForm("Use WildMushroom", "UseWildMushroom", "Offensive Spell");
            /* Healing Spell */
            AddControlInWinForm("Use Cenarion Ward", "UseCenarionWard", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Healing Touch", "UseHealingTouch", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Heart of the Wild", "UseHeartoftheWild", "Offensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Nature's Vigil", "UseNaturesVigil", "Offensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Rejuvenation", "UseRejuvenation", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Renewal", "UseRenewal", "Healing Spell", "AtPercentage");
            /* Game Settings */
            AddControlInWinForm("Use Low Combat Settings - Level Difference", "UseLowCombat", "Game Settings", "AtPercentage");
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
            AddControlInWinForm("Do avoid melee (Off Advised!!)", "DoAvoidMelee", "Game Settings");
            AddControlInWinForm("Avoid melee distance (1 to 4)", "DoAvoidMeleeDistance", "Game Settings");
        }

        public static DruidBalanceSettings CurrentSetting { get; set; }

        public static DruidBalanceSettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Druid_Balance.xml";
            if (File.Exists(currentSettingsFile))
            {
                return
                    CurrentSetting = Load<DruidBalanceSettings>(currentSettingsFile);
            }
            return new DruidBalanceSettings();
        }
    }

    #endregion
}

public class DruidFeral
{
    private static DruidFeralSettings MySettings = DruidFeralSettings.GetSettings();

    #region General Timers & Variables

    private readonly WoWItem _firstTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET);
    private readonly WoWItem _secondTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET, 2);
    public int DecastHP = 0;
    public int DefenseHP = 0;
    public int HealHP = 0;
    public int LC = 0;
    public uint CP;

    private Timer _onCd = new Timer(0);

    #endregion

    #region Professions & Racials

    public readonly Spell Berserking = new Spell("Berserking");
    public readonly Spell Darkflight = new Spell("Darkflight");
    public readonly Spell WarStomp = new Spell("War Stomp");

    #endregion

    #region Druid Buffs

    public readonly Spell Bloodtalons = new Spell("Bloodtalons");
    public readonly Spell CatForm = new Spell("Cat Form");
    public readonly Spell Clearcasting = new Spell(135700);
    public readonly Spell Dash = new Spell("Dash");
    public readonly Spell DisplacerBeast = new Spell("Displacer Beast");
    public readonly Spell FaerieFire = new Spell("Faerie Fire");
    public readonly Spell HeartoftheWild = new Spell("Heart of the Wild");
    public readonly Spell LunarInspiration = new Spell("Lunar Inspiration");
    public readonly Spell MarkoftheWild = new Spell("Mark of the Wild");
    public readonly Spell MoonfireDebuff = new Spell(155625);
    public readonly Spell PredatorySwiftness = new Spell(69369);
    public readonly Spell Prowl = new Spell("Prowl");
    public readonly Spell RakeDebuff = new Spell(155722);
    public readonly Spell SavageRoar = new Spell("Savage Roar");
    public readonly Spell SouloftheForest = new Spell("Soul of the Forest");
    public readonly Spell StampedingRoar = new Spell("Stampeding Roar");

    #endregion

    #region Offensive Spell

    public readonly Spell FerociousBite = new Spell("Ferocious Bite");
    public readonly Spell Moonfire = new Spell("Moonfire");
    public readonly Spell Rake = new Spell("Rake");
    public readonly Spell Rip = new Spell("Rip");
    public readonly Spell Shred = new Spell("Shred");
    public readonly Spell Swipe = new Spell("Swipe");
    public readonly Spell Thrash = new Spell("Thrash");

    #endregion

    #region Offensive Cooldown

    public readonly Spell Berserk = new Spell("Berserk");
    public readonly Spell ForceofNature = new Spell("Force of Nature");
    public readonly Spell Incarnation = new Spell("Incarnation: King of the Jungle");
    public readonly Spell TigersFury = new Spell("Tiger's Fury");

    #endregion

    #region Defensive Cooldown

    public readonly Spell FaerieSwarm = new Spell("Faerie Swarm");
    public readonly Spell IncapacitatingRoar = new Spell("Incapacitating Roar");
    public readonly Spell Maim = new Spell("Maim");
    public readonly Spell MassEntanglement = new Spell("Mass Entanglement");
    public readonly Spell MightyBash = new Spell("Mighty Bash");
    public readonly Spell SkullBash = new Spell("Skull Bash");
    public readonly Spell SurvivalInstincts = new Spell("Survival Instincts");
    public readonly Spell Typhoon = new Spell("Typhoon");
    public readonly Spell UrsolsVortex = new Spell("Ursol's Vortex");
    public readonly Spell WildCharge = new Spell("Wild Charge");

    #endregion

    #region Healing Spell

    public readonly Spell CenarionWard = new Spell("Cenarion Ward");
    public readonly Spell HealingTouch = new Spell("Healing Touch");
    public readonly Spell NaturesVigil = new Spell("Nature's Vigil");
    public readonly Spell Rejuvenation = new Spell("Rejuvenation");
    public readonly Spell Renewal = new Spell("Renewal");

    #endregion

    public DruidFeral()
    {
        Main.InternalRange = ObjectManager.Me.GetCombatReach;
        MySettings = DruidFeralSettings.GetSettings();
        Main.DumpCurrentSettings<DruidFeralSettings>(MySettings);
        UInt128 lastTarget = 0;

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
                                && (FaerieFire.IsHostileDistanceGood || Rake.IsHostileDistanceGood))
                            {
                                Pull();
                                lastTarget = ObjectManager.Me.Target;
                            }

                            if (MySettings.UseLowCombat && (ObjectManager.Me.Level - ObjectManager.Target.Level >= MySettings.UseLowCombatAtPercentage))
                            {
                                if (ObjectManager.Target.GetDistance < 30)
                                    LowCombat();
                            }
                            else
                            {
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
        if (MySettings.UseIncapacitatingRoarAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseIncapacitatingRoarAtPercentage;

        if (MySettings.UseMaimAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseMaimAtPercentage;

        if (MySettings.UseMassEntanglementAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseMassEntanglementAtPercentage;

        if (MySettings.UseMightyBashAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseMightyBashAtPercentage;

        if (MySettings.UseSurvivalInstinctsAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseSurvivalInstinctsAtPercentage;

        if (MySettings.UseTyphoonAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseTyphoonAtPercentage;

        if (MySettings.UseUrsolsVortexAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseUrsolsVortexAtPercentage;

        if (MySettings.UseWarStompAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseWarStompAtPercentage;

        if (MySettings.UseSkullBashAtPercentage > DecastHP)
            DecastHP = MySettings.UseSkullBashAtPercentage;

        if (MySettings.UseCenarionWardAtPercentage > HealHP)
            HealHP = MySettings.UseCenarionWardAtPercentage;

        if (MySettings.UseHealingTouchAtPercentage > HealHP)
            HealHP = MySettings.UseHealingTouchAtPercentage;

        if (MySettings.UseHeartoftheWildAtPercentage > HealHP)
            HealHP = MySettings.UseHeartoftheWildAtPercentage;

        if (MySettings.UseNaturesVigilAtPercentage > HealHP)
            HealHP = MySettings.UseNaturesVigilAtPercentage;

        if (MySettings.UseRejuvenationAtPercentage > HealHP)
            HealHP = MySettings.UseRejuvenationAtPercentage;

        if (MySettings.UseRenewalAtPercentage > HealHP)
            HealHP = MySettings.UseRenewalAtPercentage;
    }

    private void Pull()
    {
        if (MySettings.UseHealingTouch && Bloodtalons.KnownSpell && !ObjectManager.Me.InCombat && HealingTouch.IsSpellUsable)
            HealingTouch.Cast();

        if (MySettings.UseCatForm && !CatForm.HaveBuff)
            CatForm.Cast();

        if (MySettings.UseProwl && !ObjectManager.Me.InCombat && !Prowl.HaveBuff && Prowl.IsSpellUsable)
            Prowl.Cast();

        if (MySettings.UseWildCharge && WildCharge.IsSpellUsable && WildCharge.IsHostileDistanceGood && ObjectManager.Target.GetDistance > Main.InternalRange)
        {
            WildCharge.Cast();
            return;
        }
        if (MySettings.UseRake && (!RakeDebuff.TargetHaveBuff || ObjectManager.Target.UnitAura(155722).AuraTimeLeftInMs < 4500) && Rake.IsSpellUsable && Rake.IsHostileDistanceGood)
        {
            Rake.Cast();
            return;
        }

        if (MySettings.UseFaerieFire && FaerieFire.IsSpellUsable && FaerieFire.IsHostileDistanceGood)
            FaerieFire.Cast();
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

        if (MySettings.UseFerociousBite && ObjectManager.Me.ComboPoint > 4 && FerociousBite.IsSpellUsable && FerociousBite.IsHostileDistanceGood)
        {
            FerociousBite.Cast();
            return;
        }
        if (MySettings.UseShred && Shred.IsSpellUsable && Shred.IsHostileDistanceGood)
        {
            Shred.Cast();
            return;
        }

        if (MySettings.UseSwipe && Swipe.IsSpellUsable && ObjectManager.GetUnitInSpellRange(Swipe.MaxRangeHostile) > 2)
            Swipe.Cast();
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

        if (ObjectManager.Me.HealthPercent <= DecastHP)
            Decast();

        DPSBurst();
        DPSCycle();
    }

    private void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (MySettings.UseCatForm && !CatForm.HaveBuff && CatForm.KnownSpell)
            CatForm.Cast();

        if (MySettings.UseDarkflight && Darkflight.KnownSpell && !Dash.HaveBuff && !StampedingRoar.HaveBuff && ObjectManager.Me.GetMove && !ObjectManager.Target.InCombat && Darkflight.IsSpellUsable)
            Darkflight.Cast();

        if (MySettings.UseDash && !Darkflight.HaveBuff && !StampedingRoar.HaveBuff && ObjectManager.Me.GetMove && !ObjectManager.Target.InCombat && Dash.IsSpellUsable)
            Dash.Cast();

        if (MySettings.UseDisplacerBeast && !ObjectManager.Target.InCombat && ObjectManager.Me.GetMove && !Darkflight.HaveBuff && !Dash.HaveBuff && !StampedingRoar.HaveBuff && DisplacerBeast.IsSpellUsable)
            DisplacerBeast.Cast();

        if (MySettings.UseMarkoftheWild && !MarkoftheWild.HaveBuff && MarkoftheWild.IsSpellUsable)
            MarkoftheWild.Cast();

        if (MySettings.UseStampedingRoar && !Darkflight.HaveBuff && !Dash.HaveBuff && ObjectManager.Me.GetMove && !ObjectManager.Target.InCombat && StampedingRoar.IsSpellUsable)
            StampedingRoar.Cast();

        if (MySettings.UseAlchFlask && !ObjectManager.Me.HaveBuff(79638) && !ObjectManager.Me.HaveBuff(79640) && !ObjectManager.Me.HaveBuff(79639)
            && !ItemsManager.IsItemOnCooldown(75525) && ItemsManager.GetItemCount(75525) > 0)
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
        if (MySettings.UseFaerieFire && !FaerieFire.TargetHaveBuff && FaerieFire.IsSpellUsable && FaerieFire.IsHostileDistanceGood)
            FaerieFire.Cast();

        if (MySettings.UseFaerieSwarm && !FaerieSwarm.TargetHaveBuff && ObjectManager.Target.GetMove && FaerieSwarm.IsSpellUsable && FaerieSwarm.IsHostileDistanceGood)
            FaerieSwarm.Cast();

        if (MySettings.UseIncapacitatingRoar && (ObjectManager.Me.HealthPercent <= MySettings.UseIncapacitatingRoarAtPercentage
                                                 || ObjectManager.GetUnitInSpellRange(10 /*, ObjectManager.Me*/) > 2) && IncapacitatingRoar.IsSpellUsable)
        {
            Others.SafeSleep(1000);
            IncapacitatingRoar.Cast();
            _onCd = new Timer(1000*3);

            if (MySettings.UseWildCharge && WildCharge.IsSpellUsable && ObjectManager.GetUnitInSpellRange(5 /*, ObjectManager.Me*/) > 0)
                WildCharge.Cast();
            else if (MySettings.UseDisplacerBeast && DisplacerBeast.IsSpellUsable && ObjectManager.GetUnitInSpellRange(5 /*, ObjectManager.Me*/) > 0)
                DisplacerBeast.Cast();
            else
            {
                var maxTimeTimer = new Timer(1000*3);
                MovementsAction.MoveBackward(true);
                if (ObjectManager.Target.InCombat && !maxTimeTimer.IsReady && IncapacitatingRoar.TargetHaveBuff && ObjectManager.GetUnitInSpellRange(5 /*, ObjectManager.Me*/) > 0)
                {
                    Logging.WriteFight("Defensively Moving Away");
                    Others.SafeSleep(3000);
                }
                MovementsAction.MoveBackward(false);
                if (maxTimeTimer.IsReady && ObjectManager.Target.InCombat && ObjectManager.GetUnitInSpellRange(5 /*, ObjectManager.Me*/) > 2)
                {
                    MovementsAction.MoveForward(true);
                    Others.SafeSleep(3000);
                    MovementsAction.MoveForward(false);
                    MovementManager.Face(ObjectManager.Target.Position);
                }
            }

            return;
        }
        if (MySettings.UseMaim && ObjectManager.Me.HealthPercent <= MySettings.UseMaimAtPercentage && ObjectManager.Me.ComboPoint > 2 && Maim.IsSpellUsable && Maim.IsHostileDistanceGood)
        {
            CP = ObjectManager.Me.ComboPoint;
            Maim.Cast();
            _onCd = new Timer(1000*CP);
            return;
        }
        if (MySettings.UseMassEntanglement && (ObjectManager.Me.HealthPercent <= MySettings.UseMassEntanglementAtPercentage
                                               || ObjectManager.GetUnitInSpellRange(10 /*, ObjectManager.Me*/) > 2) && MassEntanglement.IsSpellUsable)
        {
            Others.SafeSleep(1000);
            MassEntanglement.Cast();
            _onCd = new Timer(1000*3);

            if (MySettings.UseWildCharge && WildCharge.IsSpellUsable && ObjectManager.GetUnitInSpellRange(5 /*, ObjectManager.Me*/) > 0)
                WildCharge.Cast();
            else if (MySettings.UseDisplacerBeast && DisplacerBeast.IsSpellUsable && ObjectManager.GetUnitInSpellRange(5 /*, ObjectManager.Me*/) > 0)
                DisplacerBeast.Cast();
            else
            {
                var maxTimeTimer = new Timer(1000*3);
                MovementsAction.MoveBackward(true);
                if (ObjectManager.Target.InCombat && !maxTimeTimer.IsReady && MassEntanglement.TargetHaveBuff && ObjectManager.GetUnitInSpellRange(5 /*, ObjectManager.Me*/) > 0)
                {
                    Logging.WriteFight("Defensively Moving Away");
                    Others.SafeSleep(3000);
                }
                MovementsAction.MoveBackward(false);
                if (maxTimeTimer.IsReady && ObjectManager.Target.InCombat && ObjectManager.GetUnitInSpellRange(5 /*, ObjectManager.Me*/) > 2)
                {
                    MovementsAction.MoveForward(true);
                    Others.SafeSleep(3000);
                    MovementsAction.MoveForward(false);
                    MovementManager.Face(ObjectManager.Target.Position);
                }
            }

            return;
        }
        if (MySettings.UseMightyBash && ObjectManager.Me.HealthPercent <= MySettings.UseMightyBashAtPercentage && MightyBash.IsSpellUsable && MightyBash.IsHostileDistanceGood)
        {
            MightyBash.Cast();
            _onCd = new Timer(1000*5);
            return;
        }
        if (MySettings.UseSurvivalInstincts && ObjectManager.Me.HealthPercent <= MySettings.UseSurvivalInstinctsAtPercentage && SurvivalInstincts.GetSpellCharges > 0
            && SurvivalInstincts.IsSpellUsable)
        {
            SurvivalInstincts.Cast();
            _onCd = new Timer(1000*6);
            return;
        }
        if (MySettings.UseTyphoon && ObjectManager.Me.HealthPercent <= MySettings.UseTyphoonAtPercentage && Typhoon.IsSpellUsable && Typhoon.IsHostileDistanceGood)
        {
            Typhoon.Cast();
            _onCd = new Timer(1000*6);
            return;
        }
        if (MySettings.UseUrsolsVortex && ObjectManager.Me.HealthPercent <= MySettings.UseUrsolsVortexAtPercentage && UrsolsVortex.IsSpellUsable
            && UrsolsVortex.IsHostileDistanceGood)
        {
            Others.SafeSleep(1000);
            SpellManager.CastSpellByIDAndPosition(102793, ObjectManager.Me.Position);

            if (MySettings.UseWildCharge && WildCharge.IsSpellUsable && ObjectManager.GetUnitInSpellRange(5 /*, ObjectManager.Me*/) > 0)
                WildCharge.Cast();
            else if (MySettings.UseDisplacerBeast && DisplacerBeast.IsSpellUsable && ObjectManager.GetUnitInSpellRange(5 /*, ObjectManager.Me*/) > 0)
                DisplacerBeast.Cast();
            else
            {
                var maxTimeTimer = new Timer(1000*3);
                MovementsAction.MoveBackward(true);
                if (ObjectManager.Target.InCombat && !maxTimeTimer.IsReady && UrsolsVortex.TargetHaveBuff && ObjectManager.GetUnitInSpellRange(5 /*, ObjectManager.Me*/) > 0)
                {
                    Logging.WriteFight("Defensively Moving Away");
                    Others.SafeSleep(3000);
                }
                MovementsAction.MoveBackward(false);
                if (maxTimeTimer.IsReady && ObjectManager.Target.InCombat && ObjectManager.GetUnitInSpellRange(5 /*, ObjectManager.Me*/) > 2)
                {
                    MovementsAction.MoveForward(true);
                    Others.SafeSleep(3000);
                    MovementsAction.MoveForward(false);
                    MovementManager.Face(ObjectManager.Target.Position);
                }
            }

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

        if (MySettings.UseCenarionWard && ObjectManager.Me.HealthPercent <= MySettings.UseCenarionWardAtPercentage && CenarionWard.IsSpellUsable)
        {
            CenarionWard.Cast();
            return;
        }
        if (MySettings.UseHealingTouch && ((PredatorySwiftness.HaveBuff && ObjectManager.Me.HealthPercent < 100)
                                           || (ObjectManager.Me.HealthPercent <= MySettings.UseHealingTouchAtPercentage && !ObjectManager.Me.InCombat)) && HealingTouch.IsSpellUsable)
        {
            HealingTouch.Cast();
            return;
        }
        if (MySettings.UseHeartoftheWild && ObjectManager.Me.HealthPercent <= MySettings.UseHeartoftheWildAtPercentage && HeartoftheWild.IsSpellUsable)
        {
            HeartoftheWild.Cast();
            return;
        }
        if (MySettings.UseNaturesVigil && ObjectManager.Me.HealthPercent <= MySettings.UseNaturesVigilAtPercentage && NaturesVigil.IsSpellUsable)
        {
            NaturesVigil.Cast();
            return;
        }
        if (MySettings.UseRejuvenation && ObjectManager.Me.HealthPercent <= MySettings.UseHealingTouchAtPercentage && !Rejuvenation.HaveBuff && Rejuvenation.IsSpellUsable)
        {
            Rejuvenation.Cast();
            return;
        }
        if (MySettings.UseRenewal && ObjectManager.Me.HealthPercent <= MySettings.UseRenewalAtPercentage && Renewal.IsSpellUsable)
            Renewal.Cast();
    }

    private void Decast()
    {
        if (MySettings.UseSkullBash && ObjectManager.Me.HealthPercent <= MySettings.UseSkullBashAtPercentage
            && ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && SkullBash.IsSpellUsable && SkullBash.IsHostileDistanceGood)
            SkullBash.Cast();
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
        if (MySettings.UseBerserking && Berserking.IsSpellUsable && Shred.IsHostileDistanceGood)
            Berserking.Cast();

        if (MySettings.UseForceofNature && (ForceofNature.GetSpellCharges > 1 || (TigersFury.HaveBuff && ForceofNature.GetSpellCharges > 0))
            && ForceofNature.IsSpellUsable && ForceofNature.IsHostileDistanceGood)
            ForceofNature.Cast();

        if (MySettings.UseIncarnation && Incarnation.IsSpellUsable && Shred.IsHostileDistanceGood)
        {
            Incarnation.Cast();
            Others.SafeSleep(1000);
        }

        if (MySettings.UseBerserk && Berserk.IsSpellUsable && Shred.IsHostileDistanceGood)
            Berserk.Cast();

        if (MySettings.UseTigersFury && ObjectManager.Me.Energy < 35 && TigersFury.IsSpellUsable && Shred.IsHostileDistanceGood)
            TigersFury.Cast();
    }

    private void DPSCycle()
    {
        Usefuls.SleepGlobalCooldown();
        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            if (MySettings.UseHealingTouch && Bloodtalons.KnownSpell && PredatorySwiftness.HaveBuff && HealingTouch.IsSpellUsable)
            {
                HealingTouch.Cast();
                return;
            }
            if (MySettings.UseThrash && ((Clearcasting.HaveBuff && !SouloftheForest.KnownSpell && !Bloodtalons.KnownSpell)
                                         || (ObjectManager.GetUnitInSpellRange(Thrash.MaxRangeHostile) > 2 && !Thrash.TargetHaveBuff))
                && Thrash.IsSpellUsable && Thrash.IsHostileDistanceGood)
            {
                Thrash.Cast();
                return;
            }
            if (MySettings.UseRake && (!RakeDebuff.TargetHaveBuff || ObjectManager.Target.UnitAura(155722).AuraTimeLeftInMs < 4500) && Rake.IsSpellUsable && Rake.IsHostileDistanceGood)
            {
                Rake.Cast();
                return;
            }
            if (MySettings.UseMoonfire && LunarInspiration.KnownSpell && (!MoonfireDebuff.TargetHaveBuff || ObjectManager.Target.UnitAura(155625).AuraTimeLeftInMs < 4200)
                && Moonfire.IsHostileDistanceGood && Moonfire.IsSpellUsable)
            {
                Moonfire.Cast();
                return;
            }
            if (MySettings.UseSavageRoar && (!SavageRoar.HaveBuff || ObjectManager.Me.UnitAura(52610).AuraTimeLeftInMs < 3000
                                             || (ObjectManager.Me.UnitAura(52610).AuraTimeLeftInMs < 12000 && ObjectManager.Me.ComboPoint > 4)) && SavageRoar.IsSpellUsable && SavageRoar.IsHostileDistanceGood)
            {
                SavageRoar.Cast();
                return;
            }
            if (MySettings.UseRip && (!Rip.TargetHaveBuff || (ObjectManager.Me.UnitAura(1079).AuraTimeLeftInMs < 7200 && ObjectManager.Me.ComboPoint > 4 && ObjectManager.Target.HealthPercent > 24))
                && Rip.IsSpellUsable && Rip.IsHostileDistanceGood)
            {
                Rip.Cast();
                return;
            }
            if (MySettings.UseFerociousBite && ObjectManager.Me.UnitAura(52610).AuraTimeLeftInMs > 3000
                && ((ObjectManager.Me.UnitAura(1079).AuraTimeLeftInMs > 7200 && ObjectManager.Me.ComboPoint > 4 && ObjectManager.Me.Energy > 49)
                    || (ObjectManager.Target.HealthPercent < 25 && ObjectManager.Me.UnitAura(1079).AuraTimeLeftInMs < 7200))
                && FerociousBite.IsSpellUsable && FerociousBite.IsHostileDistanceGood)
            {
                FerociousBite.Cast();
                return;
            }
            if (MySettings.UseSwipe && ObjectManager.Me.ComboPoint < 5 && Swipe.IsSpellUsable && ObjectManager.GetUnitInSpellRange(Swipe.MaxRangeHostile) > 2)
            {
                Swipe.Cast();
                return;
            }
            if (MySettings.UseShred && ObjectManager.Me.ComboPoint < 5 && Shred.IsSpellUsable && Shred.IsHostileDistanceGood)
            {
                Shred.Cast();
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

    #region Nested type: DruidFeralSettings

    [Serializable]
    public class DruidFeralSettings : Settings
    {
        public bool DoAvoidMelee = false;
        public int DoAvoidMeleeDistance = 0;
        public bool UseAlchFlask = true;
        public bool UseBerserk = true;
        public bool UseBerserking = true;
        public bool UseCatForm = true;
        public bool UseCenarionWard = true;
        public int UseCenarionWardAtPercentage = 85;
        public bool UseDarkflight = true;
        public bool UseDash = true;
        public bool UseDisplacerBeast = true;
        public bool UseFaerieFire = false;
        public bool UseFaerieSwarm = true;
        public bool UseFerociousBite = true;
        public bool UseForceofNature = true;
        public bool UseHealingTouch = true;
        public int UseHealingTouchAtPercentage = 80;
        public bool UseHeartoftheWild = true;
        public int UseHeartoftheWildAtPercentage = 80;
        public bool UseIncapacitatingRoar = true;
        public int UseIncapacitatingRoarAtPercentage = 50;
        public bool UseIncarnation = true;
        public bool UseLowCombat = true;
        public int UseLowCombatAtPercentage = 15;
        public bool UseMaim = true;
        public int UseMaimAtPercentage = 60;
        public bool UseMarkoftheWild = true;
        public bool UseMassEntanglement = true;
        public int UseMassEntanglementAtPercentage = 50;
        public bool UseMightyBash = true;
        public int UseMightyBashAtPercentage = 80;
        public bool UseMoonfire = true;
        public bool UseNaturesVigil = false;
        public int UseNaturesVigilAtPercentage = 80;
        public bool UseProwl = true;
        public bool UseRake = true;
        public bool UseRejuvenation = true;
        public int UseRejuvenationAtPercentage = 50;
        public bool UseRenewal = true;
        public int UseRenewalAtPercentage = 65;
        public bool UseRip = true;
        public bool UseSavageRoar = true;
        public bool UseShred = true;
        public bool UseSkullBash = true;
        public int UseSkullBashAtPercentage = 95;
        public bool UseStampedingRoar = true;
        public bool UseSurvivalInstincts = true;
        public int UseSurvivalInstinctsAtPercentage = 70;
        public bool UseSwipe = true;
        public bool UseThrash = true;
        public bool UseTigersFury = true;
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;
        public bool UseTyphoon = true;
        public int UseTyphoonAtPercentage = 50;
        public bool UseUrsolsVortex = true;
        public int UseUrsolsVortexAtPercentage = 50;
        public bool UseWarStomp = true;
        public int UseWarStompAtPercentage = 80;
        public bool UseWildCharge = true;

        public DruidFeralSettings()
        {
            ConfigWinForm("Druid Feral Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Darkflight", "UseDarkflight", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            /* Druid Buffs */
            AddControlInWinForm("Use Cat Form", "UseCatForm", "Druid Buffs");
            AddControlInWinForm("Use Dash", "UseDash", "Druid Buffs");
            AddControlInWinForm("Use Displacer Beast", "UseDisplacerBeast", "Druid Buffs");
            AddControlInWinForm("Use Faerie Fire", "UseFaerieFire", "Druid Buffs");
            AddControlInWinForm("Use Heart of the Wild", "UseHeartoftheWild", "Druid Buffs");
            AddControlInWinForm("Use Mark of the Wild", "UseMarkoftheWild", "Druid Buffs");
            AddControlInWinForm("Use Prowl", "UseProwl", "Druid Buffs");
            AddControlInWinForm("Use Savage Roar", "UseSavageRoar", "Druid Buffs");
            AddControlInWinForm("Use Stampeding Roar", "UseStampedingRoar", "Druid Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Ferocious Bite", "UseFerociousBite", "Offensive Spell");
            AddControlInWinForm("Use Moonfire", "UseMoonfire", "Offensive Spell");
            AddControlInWinForm("Use Rake", "UseRake", "Offensive Spell");
            AddControlInWinForm("Use Rip", "UseRip", "Offensive Spell");
            AddControlInWinForm("Use Shred", "UseShred", "Offensive Spell");
            AddControlInWinForm("Use Swipe", "UseSwipe", "Offensive Spell");
            AddControlInWinForm("Use Thrash", "UseThrash", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use Berserk", "UseBerserk", "Offensive Cooldown");
            AddControlInWinForm("Use Force of Nature", "UseForceofNature", "Offensive Cooldown");
            AddControlInWinForm("Use Incarnation: King of the Jungle", "UseIncarnation", "Offensive Cooldown");
            AddControlInWinForm("Use Tiger's Fury", "UseTigersFury", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Faerie Swarm", "UseFaerieSwarm", "Defensive Cooldown");
            AddControlInWinForm("Use Incapacitating Roar", "UseIncapacitatingRoar", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Maim ", "UseMaim ", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Mass Entanglement", "UseMassEntanglement", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Mighty Bash", "UseMightyBash", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Skull Bash", "UseSkullBash", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Survival Instincts", "UseSurvivalInstincts", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Typhoon", "UseTyphoon", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Ursol's Vortex", "UseUrsolsVortex", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Wild Charge", "UseWildCharge", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Cenarion Ward", "UseCenarionWard", "Healing Spell");
            AddControlInWinForm("Use Healing Touch", "UseHealingTouch", "Healing Spell");
            AddControlInWinForm("Use Heart of the Wild", "UseHeartoftheWild", "Offensive Cooldown");
            AddControlInWinForm("Use Nature's Vigil", "UseNaturesVigil", "Offensive Cooldown");
            AddControlInWinForm("Use Rejuvenation", "UseRejuvenation", "Healing Spell");
            AddControlInWinForm("Use Renewal", "UseRenewal", "Healing Spell");
            /* Game Settings */
            AddControlInWinForm("Use Low Combat Settings - Level Difference", "UseLowCombat", "Game Settings");
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
            AddControlInWinForm("Do avoid melee (Off Advised!!)", "DoAvoidMelee", "Game Settings");
            AddControlInWinForm("Avoid melee distance (1 to 4)", "DoAvoidMeleeDistance", "Game Settings");
        }

        public static DruidFeralSettings CurrentSetting { get; set; }

        public static DruidFeralSettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Druid_Feral.xml";
            if (File.Exists(currentSettingsFile))
            {
                return
                    CurrentSetting = Load<DruidFeralSettings>(currentSettingsFile);
            }
            return new DruidFeralSettings();
        }
    }

    #endregion
}

public class DruidRestoration
{
    private static DruidRestorationSettings MySettings = DruidRestorationSettings.GetSettings();

    #region General Timers & Variables

    private readonly WoWItem _firstTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET);
    private readonly WoWItem _secondTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET, 2);
    public int DefenseHP = 0;
    public int HealHP = 0;
    public int HealMP = 0;

    private Timer _onCd = new Timer(0);

    #endregion

    #region Professions & Racials

    public readonly Spell Berserking = new Spell("Berserking");
    public readonly Spell Darkflight = new Spell("Darkflight");
    public readonly Spell WarStomp = new Spell("War Stomp");

    #endregion

    #region Druid Buffs

    public readonly Spell Dash = new Spell("Dash");
    public readonly Spell DisplacerBeast = new Spell("Displacer Beast");
    public readonly Spell MarkoftheWild = new Spell("Mark of the Wild");
    public readonly Spell MoonfireDebuff = new Spell(164812);
    public readonly Spell StampedingRoar = new Spell("Stampeding Roar");

    #endregion

    #region Offensive Spell

    public readonly Spell HeartoftheWild = new Spell("Heart of the Wild");
    public readonly Spell Hurricane = new Spell("Hurricane");
    public readonly Spell Moonfire = new Spell("Moonfire");
    public readonly Spell Wrath = new Spell("Wrath");

    #endregion

    #region Healing Cooldown

    public readonly Spell ForceofNature = new Spell("Force of Nature");
    public readonly Spell Incarnation = new Spell("Incarnation: Tree of Life");
    public readonly Spell NaturesSwiftness = new Spell("Nature's Swiftness");
    public readonly Spell Tranquility = new Spell("Tranquility");

    #endregion

    #region Defensive Cooldown

    public readonly Spell Barkskin = new Spell("Barkskin");
    public readonly Spell IncapacitatingRoar = new Spell("Incapacitating Roar");
    public readonly Spell Ironbark = new Spell("Ironbark");
    public readonly Spell MassEntanglement = new Spell("Mass Entanglement");
    public readonly Spell MightyBash = new Spell("Mighty Bash");
    public readonly Spell Typhoon = new Spell("Typhoon");
    public readonly Spell UrsolsVortex = new Spell("Ursol's Vortex");

    #endregion

    #region Healing Spell

    public readonly Spell CenarionWard = new Spell("Cenarion Ward");
    public readonly Spell HealingTouch = new Spell("Healing Touch");
    public readonly Spell Lifebloom = new Spell("Lifebloom");
    public readonly Spell NaturesVigil = new Spell("Nature's Vigil");
    public readonly Spell Regrowth = new Spell("Regrowth");
    public readonly Spell Rejuvenation = new Spell("Rejuvenation");
    public readonly Spell Renewal = new Spell("Renewal");
    public readonly Spell Swiftmend = new Spell("Swiftmend");
    public readonly Spell WildGrowth = new Spell("Wild Growth");
    public readonly Spell WildMushroom = new Spell("Wild Mushroom");

    #endregion

    public DruidRestoration()
    {
        Main.InternalRange = 30.0f;
        Main.InternalAggroRange = 30f;
        MySettings = DruidRestorationSettings.GetSettings();
        Main.DumpCurrentSettings<DruidRestorationSettings>(MySettings);
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
                                && (Moonfire.IsHostileDistanceGood || Wrath.IsHostileDistanceGood))
                            {
                                Pull();
                                lastTarget = ObjectManager.Me.Target;
                            }

                            if (ObjectManager.Target.GetDistance <= 40f)
                                Combat();
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
        if (MySettings.UseBarkskinAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseBarkskinAtPercentage;

        if (MySettings.UseIncapacitatingRoarAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseIncapacitatingRoarAtPercentage;

        if (MySettings.UseIronbarkAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseIronbarkAtPercentage;

        if (MySettings.UseMassEntanglementAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseMassEntanglementAtPercentage;

        if (MySettings.UseMightyBashAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseMightyBashAtPercentage;

        if (MySettings.UseTyphoonAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseTyphoonAtPercentage;

        if (MySettings.UseUrsolsVortexAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseUrsolsVortexAtPercentage;

        if (MySettings.UseWarStompAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseWarStompAtPercentage;

        if (MySettings.UseCenarionWardAtPercentage > HealHP)
            HealHP = MySettings.UseCenarionWardAtPercentage;

        if (MySettings.UseHealingTouchAtPercentage > HealHP)
            HealHP = MySettings.UseHealingTouchAtPercentage;

        if (MySettings.UseHeartoftheWildAtPercentage > HealHP)
            HealHP = MySettings.UseHeartoftheWildAtPercentage;

        if (MySettings.UseLifebloomAtPercentage > HealHP)
            HealHP = MySettings.UseLifebloomAtPercentage;

        if (MySettings.UseNaturesVigilAtPercentage > HealHP)
            HealHP = MySettings.UseNaturesVigilAtPercentage;

        if (MySettings.UseRegrowthAtPercentage > HealHP)
            HealHP = MySettings.UseRegrowthAtPercentage;

        if (MySettings.UseRejuvenationAtPercentage > HealHP)
            HealHP = MySettings.UseRejuvenationAtPercentage;

        if (MySettings.UseSwiftmendAtPercentage > HealHP)
            HealHP = MySettings.UseSwiftmendAtPercentage;

        if (MySettings.UseRenewalAtPercentage > HealHP)
            HealHP = MySettings.UseRenewalAtPercentage;

        if (MySettings.UseWildGrowthAtPercentage > HealHP)
            HealHP = MySettings.UseWildGrowthAtPercentage;

        if (MySettings.UseWildMushroomAtPercentage > HealHP)
            HealHP = MySettings.UseWildMushroomAtPercentage;
    }

    private void Pull()
    {
        if (MySettings.UseWrath && Wrath.IsSpellUsable && Wrath.IsHostileDistanceGood)
        {
            Wrath.Cast();
            return;
        }

        if (MySettings.UseMoonfire && Moonfire.IsSpellUsable && Moonfire.IsHostileDistanceGood)
            Moonfire.Cast();
    }

    private void Combat()
    {
        Buff();

        if (MySettings.DoAvoidMelee)
            AvoidMelee();

        DPSCycle();

        if (_onCd.IsReady && ObjectManager.Me.HealthPercent <= DefenseHP)
            DefenseCycle();

        if (ObjectManager.Me.HealthPercent <= HealHP || ObjectManager.Me.ManaPercentage <= HealMP)
            Heal();

        HealingBurst();
        DPSCycle();
    }

    private void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (MySettings.UseDarkflight && Darkflight.KnownSpell && !Dash.HaveBuff && !StampedingRoar.HaveBuff && ObjectManager.Me.GetMove && !ObjectManager.Target.InCombat && Darkflight.IsSpellUsable)
            Darkflight.Cast();

        if (MySettings.UseDash && !Darkflight.HaveBuff && !StampedingRoar.HaveBuff && ObjectManager.Me.GetMove && !ObjectManager.Target.InCombat && Dash.IsSpellUsable)
            Dash.Cast();

        if (MySettings.UseDisplacerBeast && !ObjectManager.Me.InCombat && ObjectManager.Me.GetMove && !Dash.HaveBuff && !StampedingRoar.HaveBuff
            && DisplacerBeast.IsSpellUsable)
            DisplacerBeast.Cast();

        if (MySettings.UseMarkoftheWild && !MarkoftheWild.HaveBuff && MarkoftheWild.IsSpellUsable)
            MarkoftheWild.Cast();

        if (MySettings.UseStampedingRoar && !Darkflight.HaveBuff && !Dash.HaveBuff && ObjectManager.Me.GetMove && !ObjectManager.Target.InCombat && StampedingRoar.IsSpellUsable)
            StampedingRoar.Cast();

        if (MySettings.UseAlchFlask && !ObjectManager.Me.HaveBuff(79638) && !ObjectManager.Me.HaveBuff(79640) && !ObjectManager.Me.HaveBuff(79639)
            && !ItemsManager.IsItemOnCooldown(75525) && ItemsManager.GetItemCount(75525) > 0)
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
        if (MySettings.UseBarkskin && ObjectManager.Me.HealthPercent <= MySettings.UseBarkskinAtPercentage && Barkskin.IsSpellUsable)
        {
            if (ObjectManager.Me.IsCast)
                ObjectManager.Me.StopCast();

            Others.SafeSleep(1000);
            Barkskin.Cast();
            _onCd = new Timer(1000*12);
            return;
        }
        if (MySettings.UseIncapacitatingRoar && (ObjectManager.Me.HealthPercent <= MySettings.UseIncapacitatingRoarAtPercentage
                                                 || ObjectManager.GetUnitInSpellRange(10 /*, ObjectManager.Me*/) > 2) && IncapacitatingRoar.IsSpellUsable)
        {
            if (ObjectManager.Me.IsCast)
                ObjectManager.Me.StopCast();

            Others.SafeSleep(1000);
            IncapacitatingRoar.Cast();
            _onCd = new Timer(1000*3);

            if (MySettings.UseDisplacerBeast && DisplacerBeast.IsSpellUsable && ObjectManager.GetUnitInSpellRange(5 /*, ObjectManager.Me*/) > 0)
                DisplacerBeast.Cast();
            else
            {
                Logging.WriteFight("Defensively Moving Away");
                var maxTimeTimer = new Timer(1000*3);
                MovementsAction.MoveBackward(true);
                if (ObjectManager.Target.InCombat && !maxTimeTimer.IsReady && IncapacitatingRoar.TargetHaveBuff && ObjectManager.GetUnitInSpellRange(5 /*, ObjectManager.Me*/) > 0)
                {
                    Logging.WriteFight("Defensively Moving Away");
                    Others.SafeSleep(3000);
                }
                MovementsAction.MoveBackward(false);
                if (maxTimeTimer.IsReady && ObjectManager.Target.InCombat && ObjectManager.GetUnitInSpellRange(5 /*, ObjectManager.Me*/) > 2)
                {
                    MovementsAction.MoveForward(true);
                    Others.SafeSleep(3000);
                    MovementsAction.MoveForward(false);
                    MovementManager.Face(ObjectManager.Target.Position);
                }
            }

            return;
        }
        if (MySettings.UseIronbark && ObjectManager.Me.HealthPercent <= MySettings.UseIronbarkAtPercentage && Ironbark.IsSpellUsable)
        {
            Ironbark.Cast();
            _onCd = new Timer(1000*12);
            return;
        }
        if (MySettings.UseMassEntanglement && (ObjectManager.Me.HealthPercent <= MySettings.UseMassEntanglementAtPercentage
                                               || ObjectManager.GetUnitInSpellRange(10 /*, ObjectManager.Me*/) > 2) && MassEntanglement.IsSpellUsable)
        {
            if (ObjectManager.Me.IsCast)
                ObjectManager.Me.StopCast();

            Others.SafeSleep(1000);
            MassEntanglement.Cast();
            _onCd = new Timer(1000*3);

            if (MySettings.UseDisplacerBeast && DisplacerBeast.IsSpellUsable && ObjectManager.GetUnitInSpellRange(5 /*, ObjectManager.Me*/) > 0)
                DisplacerBeast.Cast();
            else
            {
                Logging.WriteFight("Defensively Moving Away");
                var maxTimeTimer = new Timer(1000*3);
                MovementsAction.MoveBackward(true);
                if (ObjectManager.Target.GetDistance < 10 && ObjectManager.Target.InCombat && !maxTimeTimer.IsReady && MassEntanglement.TargetHaveBuff)
                {
                    Logging.WriteFight("Defensively Moving Away");
                    Others.SafeSleep(3000);
                }
                MovementsAction.MoveBackward(false);
                if (maxTimeTimer.IsReady && ObjectManager.Target.InCombat && ObjectManager.GetUnitInSpellRange(5 /*, ObjectManager.Me*/) > 2)
                {
                    MovementsAction.MoveForward(true);
                    Others.SafeSleep(3000);
                    MovementsAction.MoveForward(false);
                    MovementManager.Face(ObjectManager.Target.Position);
                }
            }

            return;
        }
        if (MySettings.UseMightyBash && ObjectManager.Me.HealthPercent <= MySettings.UseMightyBashAtPercentage && MightyBash.IsSpellUsable && MightyBash.IsHostileDistanceGood)
        {
            if (ObjectManager.Me.IsCast)
                ObjectManager.Me.StopCast();

            Others.SafeSleep(1000);
            MightyBash.Cast();
            _onCd = new Timer(1000*5);
            return;
        }
        if (MySettings.UseTyphoon && ObjectManager.Me.HealthPercent <= MySettings.UseTyphoonAtPercentage && Typhoon.IsSpellUsable && Typhoon.IsHostileDistanceGood)
        {
            if (ObjectManager.Me.IsCast)
                ObjectManager.Me.StopCast();

            Others.SafeSleep(1000);
            Typhoon.Cast();
            _onCd = new Timer(1000*6);
            return;
        }
        if (MySettings.UseUrsolsVortex && ObjectManager.Me.HealthPercent <= MySettings.UseUrsolsVortexAtPercentage && UrsolsVortex.IsSpellUsable
            && UrsolsVortex.IsHostileDistanceGood)
        {
            if (ObjectManager.Me.IsCast)
                ObjectManager.Me.StopCast();

            Others.SafeSleep(1000);
            SpellManager.CastSpellByIDAndPosition(102793, ObjectManager.Me.Position);

            if (MySettings.UseDisplacerBeast && DisplacerBeast.IsSpellUsable && ObjectManager.GetUnitInSpellRange(5 /*, ObjectManager.Me*/) > 0)
                DisplacerBeast.Cast();
            else
            {
                Logging.WriteFight("Defensively Moving Away");
                var maxTimeTimer = new Timer(1000*4);
                MovementsAction.MoveBackward(true);
                while (ObjectManager.Target.GetDistance < 10 && ObjectManager.Target.InCombat && !maxTimeTimer.IsReady && UrsolsVortex.TargetHaveBuff)
                    Others.SafeSleep(300);
                MovementsAction.MoveBackward(false);
                if (maxTimeTimer.IsReady && ObjectManager.Target.GetDistance < 10 && ObjectManager.Target.InCombat && UrsolsVortex.TargetHaveBuff)
                {
                    MovementsAction.MoveForward(true);
                    Others.SafeSleep(3000);
                    MovementsAction.MoveForward(false);
                    MovementManager.Face(ObjectManager.Target.Position);
                }
            }

            return;
        }
        if (MySettings.UseWarStomp && ObjectManager.Me.HealthPercent <= MySettings.UseWarStompAtPercentage && WarStomp.IsSpellUsable
            && ObjectManager.GetUnitInSpellRange(WarStomp.MaxRangeHostile) > 0)
        {
            if (ObjectManager.Me.IsCast)
                ObjectManager.Me.StopCast();

            Others.SafeSleep(1000);
            WarStomp.Cast();
            _onCd = new Timer(1000*2);
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (MySettings.UseCenarionWard && ObjectManager.Me.HealthPercent <= MySettings.UseCenarionWardAtPercentage && CenarionWard.IsSpellUsable)
        {
            CenarionWard.Cast();
            return;
        }
        if (MySettings.UseHealingTouch && ObjectManager.Me.HealthPercent <= MySettings.UseHealingTouchAtPercentage && HealingTouch.IsSpellUsable)
        {
            HealingTouch.Cast();
            return;
        }
        if (MySettings.UseNaturesSwiftness && ObjectManager.Me.HealthPercent <= MySettings.UseHealingTouchAtPercentage && NaturesSwiftness.IsSpellUsable)
        {
            NaturesSwiftness.Cast();
            return;
        }
        if (MySettings.UseNaturesVigil && ObjectManager.Me.HealthPercent <= MySettings.UseNaturesVigilAtPercentage && NaturesVigil.IsSpellUsable)
        {
            NaturesVigil.Cast();
            return;
        }
        if (MySettings.UseRegrowth && ObjectManager.Me.HealthPercent <= MySettings.UseRegrowthAtPercentage && !Regrowth.HaveBuff && Regrowth.IsSpellUsable)
        {
            Regrowth.Cast();
            return;
        }
        if (MySettings.UseRejuvenation && ObjectManager.Me.HealthPercent <= MySettings.UseHealingTouchAtPercentage && !Rejuvenation.HaveBuff && Rejuvenation.IsSpellUsable)
        {
            Rejuvenation.Cast();
            return;
        }
        if (MySettings.UseRenewal && ObjectManager.Me.HealthPercent <= MySettings.UseRenewalAtPercentage && Renewal.IsSpellUsable)
        {
            Renewal.Cast();
            return;
        }
        if (MySettings.UseSwiftmend && ObjectManager.Me.HealthPercent <= MySettings.UseSwiftmendAtPercentage && Rejuvenation.HaveBuff && Swiftmend.IsSpellUsable)
        {
            Swiftmend.Cast();
            return;
        }
        if (MySettings.UseTranquility && ObjectManager.Me.HealthPercent <= MySettings.UseTranquilityAtPercentage && Tranquility.IsSpellUsable)
        {
            Tranquility.Cast(false, true);
            return;
        }
        if (MySettings.UseWildGrowth && ObjectManager.Me.HealthPercent <= MySettings.UseWildGrowthAtPercentage && !WildGrowth.HaveBuff && WildGrowth.IsSpellUsable)
        {
            WildGrowth.Cast();
            return;
        }
        if (MySettings.UseWildMushroom && ObjectManager.Me.HealthPercent <= MySettings.UseWildMushroomAtPercentage && !WildMushroom.HaveBuff && WildMushroom.IsSpellUsable)
        {
            SpellManager.CastSpellByIDAndPosition(145205, ObjectManager.Me.Position);
            return;
        }
    }

    public void HealingBurst()
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
        if (MySettings.UseBerserking && Berserking.IsSpellUsable && Wrath.IsHostileDistanceGood)
            Berserking.Cast();

        if (MySettings.UseForceofNature && (ForceofNature.GetSpellCharges > 1 || (Incarnation.HaveBuff && ForceofNature.GetSpellCharges > 0))
            && ForceofNature.IsSpellUsable && ForceofNature.IsHostileDistanceGood)
            ForceofNature.Cast();

        if (MySettings.UseHeartoftheWild && HeartoftheWild.IsSpellUsable && Wrath.IsHostileDistanceGood)
            HeartoftheWild.Cast();

        if (MySettings.UseIncarnation && Incarnation.IsSpellUsable && Wrath.IsHostileDistanceGood)
            Incarnation.Cast();
    }

    private void DPSCycle()
    {
        Usefuls.SleepGlobalCooldown();
        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            if (MySettings.UseMoonfire && !MoonfireDebuff.TargetHaveBuff && Moonfire.IsSpellUsable && Moonfire.IsHostileDistanceGood)
            {
                Moonfire.Cast();
                return;
            }
            if (MySettings.UseHurricane && Hurricane.IsSpellUsable && ObjectManager.GetUnitInSpellRange(8 /*, ObjectManager.Target*/) > 3 && Hurricane.IsHostileDistanceGood)
            {
                SpellManager.CastSpellByIDAndPosition(16914, ObjectManager.Target.Position);
                return;
            }
            if (MySettings.UseWrath && MoonfireDebuff.TargetHaveBuff && Wrath.IsSpellUsable && Wrath.IsHostileDistanceGood)
                Wrath.Cast(true, false, true);
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

    #region Nested type: DruidRestorationSettings

    [Serializable]
    public class DruidRestorationSettings : Settings
    {
        public bool DoAvoidMelee = false;
        public int DoAvoidMeleeDistance = 0;
        public bool UseAlchFlask = true;
        public bool UseBarkskin = true;
        public int UseBarkskinAtPercentage = 75;
        public bool UseBerserking = true;
        public bool UseCenarionWard = true;
        public int UseCenarionWardAtPercentage = 85;
        public bool UseDarkflight = true;
        public bool UseDash = true;
        public bool UseDisplacerBeast = true;
        public bool UseForceofNature = true;
        public bool UseHealingTouch = true;
        public int UseHealingTouchAtPercentage = 60;
        public bool UseHeartoftheWild = true;
        public int UseHeartoftheWildAtPercentage = 60;
        public bool UseHurricane = true;
        public bool UseIncapacitatingRoar = true;
        public int UseIncapacitatingRoarAtPercentage = 50;
        public bool UseIncarnation = true;
        public bool UseIronbark = true;
        public int UseIronbarkAtPercentage = 70;
        public bool UseLifebloom = true;
        public int UseLifebloomAtPercentage = 70;
        public bool UseMarkoftheWild = true;
        public bool UseMassEntanglement = true;
        public int UseMassEntanglementAtPercentage = 50;
        public bool UseMightyBash = true;
        public int UseMightyBashAtPercentage = 80;
        public bool UseMoonfire = true;
        public bool UseNaturesSwiftness = true;
        public bool UseNaturesVigil = false;
        public int UseNaturesVigilAtPercentage = 80;
        public bool UseRegrowth = true;
        public int UseRegrowthAtPercentage = 65;
        public bool UseRejuvenation = true;
        public int UseRejuvenationAtPercentage = 80;
        public bool UseRenewal = true;
        public int UseRenewalAtPercentage = 65;
        public bool UseStampedingRoar = true;
        public bool UseSwiftmend = true;
        public int UseSwiftmendAtPercentage = 75;
        public bool UseTranquility = true;
        public int UseTranquilityAtPercentage = 30;
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;
        public bool UseTyphoon = true;
        public int UseTyphoonAtPercentage = 50;
        public bool UseUrsolsVortex = true;
        public int UseUrsolsVortexAtPercentage = 50;
        public bool UseWarStomp = true;
        public int UseWarStompAtPercentage = 80;
        public bool UseWildGrowth = true;
        public int UseWildGrowthAtPercentage = 45;
        public bool UseWildMushroom = true;
        public int UseWildMushroomAtPercentage = 55;
        public bool UseWrath = true;

        public DruidRestorationSettings()
        {
            ConfigWinForm("Druid Restoration Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Darkflight", "UseDarkflight", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            /* Druid Buffs */
            AddControlInWinForm("Use Dash", "UseDash", "Druid Buffs");
            AddControlInWinForm("Use Displacer Beast", "UseDisplacerBeast", "Druid Buffs");
            AddControlInWinForm("Use Mark of the Wild", "UseMarkoftheWild", "Druid Buffs");
            AddControlInWinForm("Use Stampeding Roar", "UseStampedingRoar", "Druid Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Heart of the Wild", "UseHeartoftheWild", "Offensive Spell");
            AddControlInWinForm("Use Hurricane", "UseHurricane", "Offensive Spell");
            AddControlInWinForm("Use Moonfire", "UseMoonfire", "Offensive Spell");
            AddControlInWinForm("Use Wrath", "UseWrath", "Offensive Spell");
            /* Healing Cooldown */
            AddControlInWinForm("Use Force of Nature", "UseForceofNature", "Healing Cooldown");
            AddControlInWinForm("Use Incarnation: Tree of Life", "UseIncarnation", "Healing Cooldown");
            AddControlInWinForm("Use Nature's Swiftness", "UseNaturesSwiftness", "Healing Cooldown");
            AddControlInWinForm("Use Tranquility", "UseTranquility", "Healing Cooldown", "AtPercentage");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Barkskin", "UseBarkskin", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Incapacitating Roar", "UseIncapacitatingRoar", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Ironbark", "UseIronbark", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Mass Entanglement", "UseMassEntanglement", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Mighty Bash", "UseMightyBash", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Typhoon", "UseTyphoon", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Ursol's Vortex", "UseUrsolsVortex", "Defensive Cooldown", "AtPercentage");
            /* Healing Spell */
            AddControlInWinForm("Use Cenarion Ward", "UseCenarionWard", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Healing Touch", "UseHealingTouch", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Lifebloom", "UseLifebloom", "Offensive Spell", "AtPercentage");
            AddControlInWinForm("Use Nature's Vigil", "UseNaturesVigil", "Offensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Regrowth", "UseRegrowth", "Offensive Spell", "AtPercentage");
            AddControlInWinForm("Use Rejuvenation", "UseRejuvenation", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Renewal", "UseRenewal", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Swiftmend", "UseSwiftmend", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Wild Growth", "UseWildGrowth", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use WildMushroom", "UseWildMushroom", "Healing Spell", "AtPercentage");
            /* Game Settings */
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
            AddControlInWinForm("Do avoid melee (Off Advised!!)", "DoAvoidMelee", "Game Settings");
            AddControlInWinForm("Avoid melee distance (1 to 4)", "DoAvoidMeleeDistance", "Game Settings");
        }

        public static DruidRestorationSettings CurrentSetting { get; set; }

        public static DruidRestorationSettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Druid_Restoration.xml";
            if (File.Exists(currentSettingsFile))
            {
                return
                    CurrentSetting = Load<DruidRestorationSettings>(currentSettingsFile);
            }
            return new DruidRestorationSettings();
        }
    }

    #endregion
}

public class DruidGuardian
{
    private static DruidGuardianSettings MySettings = DruidGuardianSettings.GetSettings();

    #region General Timers & Variables

    private readonly WoWItem _firstTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET);
    private readonly WoWItem _secondTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET, 2);
    public int DecastHP = 0;
    public int DefenseHP = 0;
    public int HealHP = 0;

    private Timer _onCd = new Timer(0);

    #endregion

    #region Professions & Racials

    public readonly Spell Berserking = new Spell("Berserking");
    public readonly Spell Darkflight = new Spell("Darkflight");
    public readonly Spell WarStomp = new Spell("War Stomp");

    #endregion

    #region Druid Buffs

    public readonly Spell BearForm = new Spell("Bear Form");
    public readonly Spell Dash = new Spell("Dash");
    public readonly Spell DisplacerBeast = new Spell("Displacer Beast");
    public readonly Spell DreamofCenarius = new Spell(145162);
    public readonly Spell FaerieFire = new Spell("Faerie Fire");
    public readonly Spell MarkoftheWild = new Spell("Mark of the Wild");
    public readonly Spell StampedingRoar = new Spell("Stampeding Roar");
    public readonly Spell ToothandClaw = new Spell(135286);

    #endregion

    #region Offensive Spell

    public readonly Spell Growl = new Spell("Growl");
    public readonly Spell Lacerate = new Spell("Lacerate");
    public readonly Spell Mangle = new Spell("Mangle");
    public readonly Spell Maul = new Spell("Maul");
    public readonly Spell Pulverize = new Spell("Pulverize");
    public readonly Spell Thrash = new Spell("Thrash");

    #endregion

    #region Offensive Cooldown

    public readonly Spell Berserk = new Spell("Berserk");
    public readonly Spell ForceofNature = new Spell("Force of Nature");
    public readonly Spell Incarnation = new Spell("Incarnation: Son of Ursoc");

    #endregion

    #region Defensive Cooldown

    public readonly Spell Barkskin = new Spell("Barkskin");
    public readonly Spell BristlingFur = new Spell("Bristling Fur");
    public readonly Spell FaerieSwarm = new Spell("Faerie Swarm");
    public readonly Spell IncapacitatingRoar = new Spell("Incapacitating Roar");
    public readonly Spell MassEntanglement = new Spell("Mass Entanglement");
    public readonly Spell MightyBash = new Spell("Mighty Bash");
    public readonly Spell SavageDefense = new Spell("Savage Defense");
    public readonly Spell SkullBash = new Spell("Skull Bash");
    public readonly Spell SurvivalInstincts = new Spell("Survival Instincts");
    public readonly Spell Typhoon = new Spell("Typhoon");
    public readonly Spell UrsolsVortex = new Spell("Ursol's Vortex");
    public readonly Spell WildCharge = new Spell("Wild Charge");

    #endregion

    #region Healing Spell

    public readonly Spell CenarionWard = new Spell("Cenarion Ward");
    public readonly Spell FrenziedRegeneration = new Spell("Frenzied Regeneration");
    public readonly Spell HealingTouch = new Spell("Healing Touch");
    public readonly Spell HeartoftheWild = new Spell("Heart of the Wild");
    public readonly Spell NaturesVigil = new Spell("Nature's Vigil");
    public readonly Spell Rejuvenation = new Spell("Rejuvenation");
    public readonly Spell Renewal = new Spell("Renewal");

    #endregion

    public DruidGuardian()
    {
        Main.InternalRange = ObjectManager.Me.GetCombatReach;
        MySettings = DruidGuardianSettings.GetSettings();
        Main.DumpCurrentSettings<DruidGuardianSettings>(MySettings);
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
                                && (FaerieFire.IsHostileDistanceGood || Maul.IsHostileDistanceGood))
                            {
                                Pull();
                                lastTarget = ObjectManager.Me.Target;
                            }

                            if (ObjectManager.Target.GetDistance <= 40f)
                                Combat();
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
        if (MySettings.UseBarkskinAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseBarkskinAtPercentage;

        if (MySettings.UseBristlingFurAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseBristlingFurAtPercentage;

        if (MySettings.UseIncapacitatingRoarAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseIncapacitatingRoarAtPercentage;

        if (MySettings.UseMassEntanglementAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseMassEntanglementAtPercentage;

        if (MySettings.UseMightyBashAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseMightyBashAtPercentage;

        if (MySettings.UseSavageDefenseAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseSavageDefenseAtPercentage;

        if (MySettings.UseSurvivalInstinctsAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseSurvivalInstinctsAtPercentage;

        if (MySettings.UseTyphoonAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseTyphoonAtPercentage;

        if (MySettings.UseUrsolsVortexAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseUrsolsVortexAtPercentage;

        if (MySettings.UseWarStompAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseWarStompAtPercentage;

        if (MySettings.UseSkullBashAtPercentage > DecastHP)
            DecastHP = MySettings.UseSkullBashAtPercentage;

        if (MySettings.UseCenarionWardAtPercentage > HealHP)
            HealHP = MySettings.UseCenarionWardAtPercentage;

        if (MySettings.UseFrenziedRegenerationAtPercentage > HealHP)
            HealHP = MySettings.UseFrenziedRegenerationAtPercentage;

        if (MySettings.UseHealingTouchAtPercentage > HealHP)
            HealHP = MySettings.UseHealingTouchAtPercentage;

        if (MySettings.UseHeartoftheWildAtPercentage > HealHP)
            HealHP = MySettings.UseHeartoftheWildAtPercentage;

        if (MySettings.UseNaturesVigilAtPercentage > HealHP)
            HealHP = MySettings.UseNaturesVigilAtPercentage;

        if (MySettings.UseRejuvenationAtPercentage > HealHP)
            HealHP = MySettings.UseRejuvenationAtPercentage;

        if (MySettings.UseRenewalAtPercentage > HealHP)
            HealHP = MySettings.UseRenewalAtPercentage;
    }

    private void Pull()
    {
        if (BearForm.HaveBuff && MySettings.UseBearForm)
            BearForm.Cast();

        if (MySettings.UseWildCharge && WildCharge.IsSpellUsable && WildCharge.IsHostileDistanceGood && ObjectManager.Target.GetDistance > Main.InternalRange)
        {
            WildCharge.Cast();
            return;
        }
        if (MySettings.UseGrowl && Growl.IsSpellUsable && Growl.IsHostileDistanceGood)
        {
            Growl.Cast();
            return;
        }

        if (MySettings.UseFaerieFire && FaerieFire.IsSpellUsable && FaerieFire.IsHostileDistanceGood)
            FaerieFire.Cast();
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

        if (ObjectManager.Me.HealthPercent <= DecastHP)
            Decast();

        DPSBurst();
        DPSCycle();
    }

    private void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (MySettings.UseBearForm && !BearForm.HaveBuff && BearForm.KnownSpell)
            BearForm.Cast();

        if (MySettings.UseDarkflight && Darkflight.KnownSpell && !Dash.HaveBuff && !StampedingRoar.HaveBuff && ObjectManager.Me.GetMove && !ObjectManager.Target.InCombat && Darkflight.IsSpellUsable)
            Darkflight.Cast();

        if (MySettings.UseDash && !Darkflight.HaveBuff && !StampedingRoar.HaveBuff && ObjectManager.Me.GetMove && !ObjectManager.Target.InCombat && Dash.IsSpellUsable)
            Dash.Cast();

        if (MySettings.UseDisplacerBeast && !ObjectManager.Target.InCombat && ObjectManager.Me.GetMove && !Darkflight.HaveBuff && !Dash.HaveBuff && !StampedingRoar.HaveBuff && DisplacerBeast.IsSpellUsable)
            DisplacerBeast.Cast();

        if (MySettings.UseMarkoftheWild && !MarkoftheWild.HaveBuff && MarkoftheWild.IsSpellUsable)
            MarkoftheWild.Cast();

        if (MySettings.UseStampedingRoar && !Darkflight.HaveBuff && !Dash.HaveBuff && ObjectManager.Me.GetMove && !ObjectManager.Target.InCombat && StampedingRoar.IsSpellUsable)
            StampedingRoar.Cast();

        if (MySettings.UseAlchFlask && !ObjectManager.Me.HaveBuff(79638) && !ObjectManager.Me.HaveBuff(79640) && !ObjectManager.Me.HaveBuff(79639)
            && !ItemsManager.IsItemOnCooldown(75525) && ItemsManager.GetItemCount(75525) > 0)
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
        if (MySettings.UseBarkskin && ObjectManager.Me.HealthPercent <= MySettings.UseBarkskinAtPercentage && Barkskin.IsSpellUsable)
        {
            Barkskin.Cast();
            _onCd = new Timer(1000*12);
            return;
        }

        if (MySettings.UseFaerieFire && !FaerieFire.TargetHaveBuff && FaerieFire.IsSpellUsable && FaerieFire.IsHostileDistanceGood)
            FaerieFire.Cast();

        if (MySettings.UseFaerieSwarm && !FaerieSwarm.TargetHaveBuff && ObjectManager.Target.GetMove && FaerieSwarm.IsSpellUsable && FaerieSwarm.IsHostileDistanceGood)
            FaerieSwarm.Cast();

        if (MySettings.UseIncapacitatingRoar && (ObjectManager.Me.HealthPercent <= MySettings.UseIncapacitatingRoarAtPercentage
                                                 || ObjectManager.GetUnitInSpellRange(10 /*, ObjectManager.Me*/) > 2) && IncapacitatingRoar.IsSpellUsable)
        {
            Others.SafeSleep(1000);
            IncapacitatingRoar.Cast();
            _onCd = new Timer(1000*3);

            if (MySettings.UseDisplacerBeast && DisplacerBeast.IsSpellUsable && ObjectManager.GetUnitInSpellRange(5 /*, ObjectManager.Me*/) > 0)
                DisplacerBeast.Cast();
            else
            {
                var maxTimeTimer = new Timer(1000*3);
                MovementsAction.MoveBackward(true);
                if (ObjectManager.Target.InCombat && !maxTimeTimer.IsReady && IncapacitatingRoar.TargetHaveBuff && ObjectManager.GetUnitInSpellRange(5 /*, ObjectManager.Me*/) > 0)
                {
                    Logging.WriteFight("Defensively Moving Away");
                    Others.SafeSleep(3000);
                }
                MovementsAction.MoveBackward(false);
                if (maxTimeTimer.IsReady && ObjectManager.Target.InCombat && ObjectManager.GetUnitInSpellRange(5 /*, ObjectManager.Me*/) > 2)
                {
                    MovementsAction.MoveForward(true);
                    Others.SafeSleep(3000);
                    MovementsAction.MoveForward(false);
                    MovementManager.Face(ObjectManager.Target.Position);
                }
            }

            return;
        }
        if (MySettings.UseMassEntanglement && (ObjectManager.Me.HealthPercent <= MySettings.UseMassEntanglementAtPercentage
                                               || ObjectManager.GetUnitInSpellRange(10 /*, ObjectManager.Me*/) > 2) && MassEntanglement.IsSpellUsable)
        {
            Others.SafeSleep(1000);
            MassEntanglement.Cast();
            _onCd = new Timer(1000*3);

            if (MySettings.UseDisplacerBeast && DisplacerBeast.IsSpellUsable && ObjectManager.GetUnitInSpellRange(5 /*, ObjectManager.Me*/) > 0)
                DisplacerBeast.Cast();
            else
            {
                var maxTimeTimer = new Timer(1000*3);
                MovementsAction.MoveBackward(true);
                if (ObjectManager.Target.InCombat && !maxTimeTimer.IsReady && MassEntanglement.TargetHaveBuff && ObjectManager.GetUnitInSpellRange(5 /*, ObjectManager.Me*/) > 0)
                {
                    Logging.WriteFight("Defensively Moving Away");
                    Others.SafeSleep(3000);
                }
                MovementsAction.MoveBackward(false);
                if (maxTimeTimer.IsReady && ObjectManager.Target.InCombat && ObjectManager.GetUnitInSpellRange(5 /*, ObjectManager.Me*/) > 2)
                {
                    MovementsAction.MoveForward(true);
                    Others.SafeSleep(3000);
                    MovementsAction.MoveForward(false);
                    MovementManager.Face(ObjectManager.Target.Position);
                }
            }

            return;
        }
        if (MySettings.UseMightyBash && ObjectManager.Me.HealthPercent <= MySettings.UseMightyBashAtPercentage && MightyBash.IsSpellUsable && MightyBash.IsHostileDistanceGood)
        {
            MightyBash.Cast();
            _onCd = new Timer(1000*5);
            return;
        }
        if (MySettings.UseSavageDefense && ObjectManager.Me.HealthPercent <= MySettings.UseSavageDefenseAtPercentage && SavageDefense.GetSpellCharges > 0
            && !ObjectManager.Target.IsCast && SavageDefense.IsSpellUsable)
        {
            SavageDefense.Cast();
            _onCd = new Timer(1000*6);
            return;
        }
        if (MySettings.UseSurvivalInstincts && ObjectManager.Me.HealthPercent <= MySettings.UseSurvivalInstinctsAtPercentage && SurvivalInstincts.GetSpellCharges > 0
            && SurvivalInstincts.IsSpellUsable)
        {
            SurvivalInstincts.Cast();
            _onCd = new Timer(1000*6);
            return;
        }
        if (MySettings.UseTyphoon && ObjectManager.Me.HealthPercent <= MySettings.UseTyphoonAtPercentage && Typhoon.IsSpellUsable && Typhoon.IsHostileDistanceGood)
        {
            Typhoon.Cast();
            _onCd = new Timer(1000*6);
            return;
        }
        if (MySettings.UseUrsolsVortex && ObjectManager.Me.HealthPercent <= MySettings.UseUrsolsVortexAtPercentage && UrsolsVortex.IsSpellUsable
            && UrsolsVortex.IsHostileDistanceGood)
        {
            Others.SafeSleep(1000);
            SpellManager.CastSpellByIDAndPosition(102793, ObjectManager.Me.Position);

            if (MySettings.UseDisplacerBeast && DisplacerBeast.IsSpellUsable && ObjectManager.GetUnitInSpellRange(5 /*, ObjectManager.Me*/) > 0)
                DisplacerBeast.Cast();
            else
            {
                var maxTimeTimer = new Timer(1000*3);
                MovementsAction.MoveBackward(true);
                if (ObjectManager.Target.InCombat && !maxTimeTimer.IsReady && UrsolsVortex.TargetHaveBuff && ObjectManager.GetUnitInSpellRange(5 /*, ObjectManager.Me*/) > 0)
                {
                    Logging.WriteFight("Defensively Moving Away");
                    Others.SafeSleep(3000);
                }
                MovementsAction.MoveBackward(false);
                if (maxTimeTimer.IsReady && ObjectManager.Target.InCombat && ObjectManager.GetUnitInSpellRange(5 /*, ObjectManager.Me*/) > 2)
                {
                    MovementsAction.MoveForward(true);
                    Others.SafeSleep(3000);
                    MovementsAction.MoveForward(false);
                    MovementManager.Face(ObjectManager.Target.Position);
                }
            }

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

        if (MySettings.UseCenarionWard && ObjectManager.Me.HealthPercent <= MySettings.UseCenarionWardAtPercentage && CenarionWard.IsSpellUsable)
        {
            CenarionWard.Cast();
            return;
        }
        if (MySettings.UseFrenziedRegeneration && ObjectManager.Me.HealthPercent <= MySettings.UseFrenziedRegenerationAtPercentage
            && ObjectManager.Me.Rage > 39 && FrenziedRegeneration.IsSpellUsable)
        {
            FrenziedRegeneration.Cast();
            return;
        }
        if (MySettings.UseHealingTouch && ((DreamofCenarius.HaveBuff && ObjectManager.Me.HealthPercent < 100)
                                           || (ObjectManager.Me.HealthPercent <= MySettings.UseHealingTouchAtPercentage && !ObjectManager.Me.InCombat)) && HealingTouch.IsSpellUsable)
        {
            HealingTouch.Cast();
            return;
        }
        if (MySettings.UseHeartoftheWild && ObjectManager.Me.HealthPercent <= MySettings.UseHeartoftheWildAtPercentage && HeartoftheWild.IsSpellUsable)
        {
            HeartoftheWild.Cast();
            return;
        }
        if (MySettings.UseNaturesVigil && ObjectManager.Me.HealthPercent <= MySettings.UseNaturesVigilAtPercentage && NaturesVigil.IsSpellUsable)
        {
            NaturesVigil.Cast();
            return;
        }
        if (MySettings.UseRejuvenation && ObjectManager.Me.HealthPercent <= MySettings.UseHealingTouchAtPercentage && !Rejuvenation.HaveBuff && Rejuvenation.IsSpellUsable)
        {
            Rejuvenation.Cast();
            return;
        }
        if (MySettings.UseRenewal && ObjectManager.Me.HealthPercent <= MySettings.UseRenewalAtPercentage && Renewal.IsSpellUsable)
            Renewal.Cast();
    }

    private void Decast()
    {
        if (MySettings.UseSkullBash && ObjectManager.Me.HealthPercent <= MySettings.UseSkullBashAtPercentage
            && ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && SkullBash.IsSpellUsable && SkullBash.IsHostileDistanceGood)
            SkullBash.Cast();
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
        if (MySettings.UseBerserking && Berserking.IsSpellUsable && Maul.IsHostileDistanceGood)
            Berserking.Cast();

        if (MySettings.UseForceofNature && (ForceofNature.GetSpellCharges > 1 || (Berserk.HaveBuff && ForceofNature.GetSpellCharges > 0))
            && ForceofNature.IsSpellUsable && ForceofNature.IsHostileDistanceGood)
            ForceofNature.Cast();

        if (MySettings.UseIncarnation && Incarnation.IsSpellUsable && Maul.IsHostileDistanceGood)
        {
            Incarnation.Cast();
            Others.SafeSleep(1000);
        }

        if (MySettings.UseBerserk && Berserk.IsSpellUsable && Maul.IsHostileDistanceGood)
            Berserk.Cast();
    }

    private void DPSCycle()
    {
        Usefuls.SleepGlobalCooldown();
        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            if (MySettings.UseGrowl && !ObjectManager.Target.InCombat && Growl.IsSpellUsable && Growl.IsHostileDistanceGood)
            {
                Growl.Cast();
                return;
            }
            if (MySettings.UseMaul && (ToothandClaw.HaveBuff || ObjectManager.Me.RagePercentage > 90 || ObjectManager.Me.HealthPercent > 90)
                && Maul.IsSpellUsable && Maul.IsHostileDistanceGood)
            {
                Maul.Cast();
                return;
            }
            if (MySettings.UseMangle && Mangle.IsSpellUsable && Mangle.IsHostileDistanceGood)
            {
                Mangle.Cast();
                return;
            }
            if (MySettings.UseThrash && !Thrash.TargetHaveBuff && Thrash.IsSpellUsable && Thrash.IsHostileDistanceGood)
            {
                Thrash.Cast();
                return;
            }
            if (MySettings.UseLacerate && Lacerate.TargetBuffStack < 3 && !Incarnation.HaveBuff && Lacerate.IsSpellUsable && Lacerate.IsHostileDistanceGood)
            {
                Lacerate.Cast();
                return;
            }
            if (MySettings.UsePulverize && Lacerate.TargetBuffStack > 2 && Pulverize.IsSpellUsable && Pulverize.IsHostileDistanceGood)
                Pulverize.Cast();
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

    #region Nested type: DruidGuardianSettings

    [Serializable]
    public class DruidGuardianSettings : Settings
    {
        public bool DoAvoidMelee = false;
        public int DoAvoidMeleeDistance = 0;
        public bool UseAlchFlask = true;
        public bool UseBarkskin = true;
        public int UseBarkskinAtPercentage = 85;
        public bool UseBearForm = true;
        public bool UseBerserk = true;
        public bool UseBerserking = true;
        public bool UseBristlingFur = true;
        public int UseBristlingFurAtPercentage = 80;
        public bool UseCenarionWard = true;
        public int UseCenarionWardAtPercentage = 85;
        public bool UseDarkflight = true;
        public bool UseDash = true;
        public bool UseDisplacerBeast = true;
        public bool UseFaerieFire = false;
        public bool UseFaerieSwarm = true;
        public bool UseForceofNature = true;
        public bool UseFrenziedRegeneration = true;
        public int UseFrenziedRegenerationAtPercentage = 70;
        public bool UseGrowl = true;
        public bool UseHealingTouch = true;
        public int UseHealingTouchAtPercentage = 60;
        public bool UseHeartoftheWild = true;
        public int UseHeartoftheWildAtPercentage = 70;
        public bool UseIncapacitatingRoar = true;
        public int UseIncapacitatingRoarAtPercentage = 50;
        public bool UseIncarnation = true;
        public bool UseLacerate = true;
        public bool UseMangle = true;
        public bool UseMarkoftheWild = true;
        public bool UseMassEntanglement = true;
        public int UseMassEntanglementAtPercentage = 50;
        public bool UseMaul = true;
        public bool UseMightyBash = true;
        public int UseMightyBashAtPercentage = 80;
        public bool UseNaturesVigil = false;
        public int UseNaturesVigilAtPercentage = 80;
        public bool UsePulverize = true;
        public bool UseRejuvenation = true;
        public int UseRejuvenationAtPercentage = 50;
        public bool UseRenewal = true;
        public int UseRenewalAtPercentage = 65;
        public bool UseSavageDefense = true;
        public int UseSavageDefenseAtPercentage = 65;
        public bool UseSavageRoar = true;
        public bool UseSkullBash = true;
        public int UseSkullBashAtPercentage = 95;
        public bool UseStampedingRoar = true;
        public bool UseSurvivalInstincts = true;
        public int UseSurvivalInstinctsAtPercentage = 70;
        public bool UseThrash = true;
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;
        public bool UseTyphoon = true;
        public int UseTyphoonAtPercentage = 50;
        public bool UseUrsolsVortex = true;
        public int UseUrsolsVortexAtPercentage = 50;
        public bool UseWarStomp = true;
        public int UseWarStompAtPercentage = 80;
        public bool UseWildCharge = true;

        public DruidGuardianSettings()
        {
            ConfigWinForm("Druid Guardian Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Darkflight", "UseDarkflight", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            /* Druid Buffs */
            AddControlInWinForm("Use Bear Form", "UseBearForm", "Druid Buffs");
            AddControlInWinForm("Use Dash", "UseDash", "Druid Buffs");
            AddControlInWinForm("Use Displacer Beast", "UseDisplacerBeast", "Druid Buffs");
            AddControlInWinForm("Use Faerie Fire", "UseFaerieFire", "Druid Buffs");
            AddControlInWinForm("Use Mark of the Wild", "UseMarkoftheWild", "Druid Buffs");
            AddControlInWinForm("Use Stampeding Roar", "UseStampedingRoar", "Druid Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Growl", "UseGrowl", "Offensive Spell");
            AddControlInWinForm("Use Lacerate", "UseLacerate", "Offensive Spell");
            AddControlInWinForm("Use Mangle", "UseMangle", "Offensive Spell");
            AddControlInWinForm("Use Maul", "UseMaul", "Offensive Spell");
            AddControlInWinForm("Use Pulverize", "UsePulverize", "Offensive Spell");
            AddControlInWinForm("Use Thrash", "UseThrash", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use Berserk", "UseBerserk", "Offensive Cooldown");
            AddControlInWinForm("Use Force of Nature", "UseForceofNature", "Offensive Cooldown");
            AddControlInWinForm("Use Incarnation: Son of Ursoc", "UseIncarnation", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Barkskin", "UseBarkskin", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Bristling Fur", "UseBristlingFur", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Faerie Swarm", "UseFaerieSwarm", "Defensive Cooldown");
            AddControlInWinForm("Use Incapacitating Roar", "UseIncapacitatingRoar", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Mass Entanglement", "UseMassEntanglement", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Mighty Bash", "UseMightyBash", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Savage Defense", "UseSavageDefense", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Skull Bash", "UseSkullBash", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Survival Instincts", "UseSurvivalInstincts", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Typhoon", "UseTyphoon", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Ursol's Vortex", "UseUrsolsVortex", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Wild Charge", "UseWildCharge", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Cenarion Ward", "UseCenarionWard", "Healing Spell");
            AddControlInWinForm("Use Frenzied Regeneration", "UseFrenziedRegeneration", "Healing Spell");
            AddControlInWinForm("Use Healing Touch", "UseHealingTouch", "Healing Spell");
            AddControlInWinForm("Use Heart of the Wild", "UseHeartoftheWild", "Healing Spell");
            AddControlInWinForm("Use Natures Vigil", "UseNaturesVigil", "Healing Spell");
            AddControlInWinForm("Use Rejuvenation", "UseRejuvenation", "Healing Spell");
            AddControlInWinForm("Use Renewal", "UseRenewal", "Healing Spell");
            /* Game Settings */
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
            AddControlInWinForm("Do avoid melee (Off Advised!!)", "DoAvoidMelee", "Game Settings");
            AddControlInWinForm("Avoid melee distance (1 to 4)", "DoAvoidMeleeDistance", "Game Settings");
        }

        public static DruidGuardianSettings CurrentSetting { get; set; }

        public static DruidGuardianSettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Druid_Guardian.xml";
            if (File.Exists(currentSettingsFile))
            {
                return
                    CurrentSetting = Load<DruidGuardianSettings>(currentSettingsFile);
            }
            return new DruidGuardianSettings();
        }
    }

    #endregion
}

#endregion

// ReSharper restore ObjectCreationAsStatement
// ReSharper restore EmptyGeneralCatchClause