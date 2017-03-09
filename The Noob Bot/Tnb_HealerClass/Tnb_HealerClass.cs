/*
* HealerClass for TheNoobBot
* Credit : Vesper, Ryuichiro
* Thanks you !
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

public class Main : IHealerClass
{
    internal static float InternalRange = 30f;
    internal static bool InternalLoop = true;
    internal static float Version = 0.6f;

    #region IHealerClass Members

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
        Logging.WriteFight("Healing system stopped.");
        InternalLoop = false;
    }

    public void ShowConfiguration()
    {
        Directory.CreateDirectory(Application.StartupPath + "\\HealerClasses\\Settings\\");
        Initialize(true);
    }

    public void ResetConfiguration()
    {
        Directory.CreateDirectory(Application.StartupPath + "\\HealerClasses\\Settings\\");
        Initialize(true, true);
    }

    #endregion

    public void Initialize(bool configOnly, bool resetSettings = false)
    {
        try
        {
            if (!InternalLoop)
                InternalLoop = true;
            Logging.WriteFight("Loading healing system.");
            WoWSpecialization wowSpecialization = ObjectManager.Me.WowSpecialization(true);
            switch (ObjectManager.Me.WowClass)
            {
                    #region Non healer classes detection

                case WoWClass.DeathKnight:
                case WoWClass.Mage:
                case WoWClass.Warlock:
                case WoWClass.Rogue:
                case WoWClass.Warrior:
                case WoWClass.Hunter:
                case WoWClass.DemonHunter:

                    string error = "Class " + ObjectManager.Me.WowClass + " can't be a healer.";
                    if (configOnly)
                        MessageBox.Show(error);
                    Logging.WriteFight(error);
                    break;

                    #endregion

                    #region Druid Specialisation checking

                case WoWClass.Druid:

                    if (wowSpecialization == WoWSpecialization.DruidRestoration)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath + "\\HealerClasses\\Settings\\Druid_Restoration.xml";
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
                            InternalRange = 30f;
                            new DruidRestoration();
                        }
                    }
                    else
                    {
                        string druidNonRestauration = "Class " + ObjectManager.Me.WowClass + " can be a healer, but only in Restoration specialisation.";
                        if (configOnly)
                            MessageBox.Show(druidNonRestauration);
                        Logging.WriteFight(druidNonRestauration);
                    }
                    break;

                    #endregion

                    #region Paladin Specialisation checking

                case WoWClass.Paladin:

                    if (wowSpecialization == WoWSpecialization.PaladinHoly)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath + "\\HealerClasses\\Settings\\Paladin_Holy.xml";
                            var currentSetting = new PaladinHoly.PaladinHolySettings();
                            if (File.Exists(currentSettingsFile) && !resetSettings)
                            {
                                currentSetting = Settings.Load<PaladinHoly.PaladinHolySettings>(currentSettingsFile);
                            }
                            currentSetting.ToForm();
                            currentSetting.Save(currentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Paladin Holy Healer class...");
                            InternalRange = 30f;
                            new PaladinHoly();
                        }
                    }
                    else
                    {
                        string paladinNonHoly = "Class " + ObjectManager.Me.WowClass + " can be a healer, but only in Holy specialisation.";
                        if (configOnly)
                            MessageBox.Show(paladinNonHoly);
                        Logging.WriteFight(paladinNonHoly);
                    }
                    break;

                    #endregion

                    #region Shaman Specialisation checking

                case WoWClass.Shaman:

                    if (wowSpecialization == WoWSpecialization.ShamanRestoration)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath + "\\HealerClasses\\Settings\\Shaman_Restoration.xml";
                            var currentSetting = new ShamanRestoration.ShamanRestorationSettings();
                            if (File.Exists(currentSettingsFile) && !resetSettings)
                            {
                                currentSetting = Settings.Load<ShamanRestoration.ShamanRestorationSettings>(currentSettingsFile);
                            }
                            currentSetting.ToForm();
                            currentSetting.Save(currentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Shaman Restoration Healer class...");
                            InternalRange = 30f;
                            EquipmentAndStats.SetPlayerSpe(WoWSpecialization.ShamanRestoration);
                            new ShamanRestoration();
                        }
                    }
                    else
                    {
                        string shamanNonRestauration = "Class " + ObjectManager.Me.WowClass + " can be a healer, but only in Restoration specialisation.";
                        if (configOnly)
                            MessageBox.Show(shamanNonRestauration);
                        Logging.WriteFight(shamanNonRestauration);
                    }
                    break;

                    #endregion

                    #region Priest Specialisation checking

                case WoWClass.Priest:

                    if (wowSpecialization == WoWSpecialization.PriestDiscipline)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath + "\\HealerClasses\\Settings\\Priest_Discipline.xml";
                            var currentSetting = new PriestDiscipline.PriestDisciplineSettings();
                            if (File.Exists(currentSettingsFile) && !resetSettings)
                            {
                                currentSetting = Settings.Load<PriestDiscipline.PriestDisciplineSettings>(currentSettingsFile);
                            }
                            currentSetting.ToForm();
                            currentSetting.Save(currentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Priest Discipline Healer class...");
                            InternalRange = 30f;
                            new PriestDiscipline();
                        }
                    }
                    else if (wowSpecialization == WoWSpecialization.PriestHoly)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath + "\\HealerClasses\\Settings\\Priest_Holy.xml";
                            var currentSetting = new PriestHoly.PriestHolySettings();
                            if (File.Exists(currentSettingsFile) && !resetSettings)
                            {
                                currentSetting = Settings.Load<PriestHoly.PriestHolySettings>(currentSettingsFile);
                            }
                            currentSetting.ToForm();
                            currentSetting.Save(currentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Priest Holy Healer class...");
                            InternalRange = 30f;
                            new PriestHoly();
                        }
                    }
                    else
                    {
                        string priestNonHoly = "Class " + ObjectManager.Me.WowClass + " can be a healer, but only in Holy or Discipline specialisation.";
                        if (configOnly)
                            MessageBox.Show(priestNonHoly);
                        Logging.WriteFight(priestNonHoly);
                    }
                    break;

                    #endregion

                    #region Monk Specialisation checking

                case WoWClass.Monk:

                    if (wowSpecialization == WoWSpecialization.MonkMistweaver)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath + "\\HealerClasses\\Settings\\Monk_Mistweaver.xml";
                            var currentSetting = new MonkMistweaver.MonkMistweaverSettings();
                            if (File.Exists(currentSettingsFile) && !resetSettings)
                            {
                                currentSetting = Settings.Load<MonkMistweaver.MonkMistweaverSettings>(currentSettingsFile);
                            }
                            currentSetting.ToForm();
                            currentSetting.Save(currentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Monk Mistweaver Healer class...");
                            InternalRange = 30.0f;
                            new MonkMistweaver();
                        }
                    }
                    else
                    {
                        string monkNonMistweaver = "Class " + ObjectManager.Me.WowClass + " can be a healer, but only in Mistweaver specialisation.";
                        if (configOnly)
                            MessageBox.Show(monkNonMistweaver);
                        Logging.WriteFight(monkNonMistweaver);
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
        Logging.WriteFight("Healing system stopped.");
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
        Logging.WriteDebug("Loaded " + ObjectManager.Me.WowSpecialization() + " Healer Class " + Version.ToString("0.0###"));

        // Last field is intentionnally ommited because it's a backing field.
    }
}

#region Diagnostics

public static class Diagnostics
{
    private static Stopwatch stopwatch;
    private static string name;
    private static uint count;
    private static long timeMin;
    private static long timeMax;
    private static float timeAverage;

    public static void Init(string name)
    {
        Diagnostics.stopwatch = new Stopwatch();
        Diagnostics.name = name;
        Diagnostics.count = 0;
        Diagnostics.timeMin = long.MaxValue;
        Diagnostics.timeMax = long.MinValue;
        Diagnostics.timeAverage = 0;
        Logging.WriteDebug("Initialized Diagnostic Time for " + name);
    }

    public static void Summarize()
    {
        Logging.WriteDebug("Summarizing Diagnostics for '" + name + "' after " + count + " loops.");
        Logging.WriteDebug("Time = {Min: " + timeMin + ", Max: " + timeMax + ", Average: " + timeAverage + "}");
    }

    public static void Start()
    {
        stopwatch.Restart();
    }

    public static void Stop(bool log = true)
    {
        stopwatch.Stop();
        long ellapsedTime = stopwatch.ElapsedMilliseconds;
        timeMin = (ellapsedTime < timeMin) ? ellapsedTime : timeMin;
        timeMax = (ellapsedTime > timeMax) ? ellapsedTime : timeMax;
        timeAverage = ((timeAverage*count) + ellapsedTime)/++count;
        if (log)
        {
            Logging.WriteFileOnly("Time = {Ellapsed: " + ellapsedTime + ", Min: " + timeMin + ", Max: " + timeMax + ", Average: " + timeAverage + "} Loop: " + count);
        }
    }
}

#endregion

#region Druid

public class DruidRestoration
{
    private static DruidRestorationSettings MySettings = DruidRestorationSettings.GetSettings();

    #region General Timers & Variables

    private readonly WoWItem _firstTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET);
    private readonly WoWItem _secondTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET, 2);

    private bool CombatMode = true;

    private Timer DefensiveTimer = new Timer(0);

    private List<WoWPlayer> DamagedPlayers = new List<WoWPlayer>();
    private List<WoWPlayer> Tanks = new List<WoWPlayer>();
    private WoWPlayer Tank = ObjectManager.Me;
    private Timer ScanForTanksTimer = new Timer(0);
    private double PartyHpMedian;

    private WoWUnit Target = ObjectManager.Target;
    private WoWPlayer Me = ObjectManager.Me;

    #endregion

    #region Talents & Traits

    private readonly Spell TranquilMind = new Spell(189857); //Passive

    #endregion

    #region Professions & Racials

    //private readonly Spell ArcaneTorrent = new Spell("Arcane Torrent"); //No GCD
    private readonly Spell Berserking = new Spell("Berserking"); //No GCD
    private readonly Spell BloodFury = new Spell("Blood Fury"); //No GCD
    private readonly Spell Darkflight = new Spell("Darkflight"); //No GCD
    private readonly Spell GiftoftheNaaru = new Spell("Gift of the Naaru"); //No GCD
    private readonly Spell Stoneform = new Spell("Stoneform"); //No GCD
    private readonly Spell WarStomp = new Spell("War Stomp"); //No GCD

    #endregion

    #region Druid Buffs

    private readonly Spell CatForm = new Spell("Cat Form");
    private readonly Spell Flourish = new Spell("Flourish");
    private readonly Spell Incarnation = new Spell("Incarnation: Tree of Life");

    #endregion

    #region Offensive Spells

    private readonly Spell SolarWrath = new Spell("Solar Wrath");

    #endregion

    #region Artifact Spells

    private readonly Spell EssenceofGHanir = new Spell("Essence of G'Hanir");

    #endregion

    #region Defensive Spells

    //private readonly Spell EntanglingRoots = new Spell("Entangling Roots");
    private readonly Spell Ironbark = new Spell("Ironbark"); //No GCD
    //private readonly Spell MassEntanglement = new Spell("Mass Entanglement");
    private readonly Spell MightyBash = new Spell("Mighty Bash");
    //private readonly Spell Typhoon = new Spell("Typhoon");
    //private readonly Spell WildCharge = new Spell("Wild Charge"); //No GCD

    #endregion

    #region Healing Spells

    private readonly Spell CenarionWard = new Spell("Cenarion Ward");
    private readonly Spell Efflorescence = new Spell("Efflorescence");
    private Timer EfflorescenceTimer = new Timer(0);
    private readonly Spell FrenziedRegeneration = new Spell("Frenzied Regeneration");
    private readonly Spell HealingTouch = new Spell("Healing Touch");
    private readonly Spell Lifebloom = new Spell("Lifebloom");
    private readonly Spell Regrowth = new Spell("Regrowth");
    private readonly Spell Rejuvenation = new Spell("Rejuvenation");
    private readonly Spell Renewal = new Spell("Renewal"); //No GCD
    private readonly Spell Swiftmend = new Spell("Swiftmend");
    private readonly Spell Tranquility = new Spell("Tranquility");
    private readonly Spell WildGrowth = new Spell("Wild Growth");

    #endregion

    #region Utility Spells

    private readonly Spell Dash = new Spell(1850 /*"Dash"*/); //No GCD
    //private readonly Spell DisplacerBeast = new Spell("Displacer Beast");
    //private readonly Spell Prowl = new Spell("Prowl"); //No GCD

    #endregion

    public DruidRestoration()
    {
        Main.InternalRange = HealingTouch.MaxRangeFriend;
        MySettings = DruidRestorationSettings.GetSettings();
        Main.DumpCurrentSettings<DruidRestorationSettings>(MySettings);

        Diagnostics.Init("Restoration Druid");

        while (Main.InternalLoop)
        {
            try
            {
                if (!ObjectManager.Me.IsDeadMe)
                {
                    //Dismount in Combat
                    if (ObjectManager.Me.InCombat && ObjectManager.Me.IsMounted)
                        MountTask.DismountMount(true);

                    Target = ObjectManager.Target;

                    if (!ObjectManager.Me.IsMounted)
                    {
                        ScanParty();

                        if (Fight.InFight || PartyHpMedian < 90)
                            Combat();
                        else
                            Patrolling();
                    }
                }
                else
                    Thread.Sleep(800);
            }
            catch
            {
            }
            Thread.Sleep(200);
        }
        Diagnostics.Summarize();
    }

    // Scans Party and calculates class variables
    private void ScanParty()
    {
        //Prepare Tank Scan
        if (ScanForTanksTimer.IsReady && !Fight.InFight)
        {
            Logging.WriteFight("Scanning for Tanks:");
            Tanks.Clear();
        }

        //Setup Solo
        if (!Party.IsInGroup())
        {
            if (ScanForTanksTimer.IsReady && !Fight.InFight)
                Tanks.Add(Me);
            DamagedPlayers.Clear();
            DamagedPlayers.Add(Me);
            PartyHpMedian = Me.HealthPercent;
        }
        //Setup Group
        else
        {
            int alivePlayers = 0;
            PartyHpMedian = 0;

            try
            {
                Memory.WowMemory.GameFrameLock();

                DamagedPlayers.Clear();

                foreach (UInt128 playerInMyParty in Party.GetPartyPlayersGUID())
                {
                    if (playerInMyParty <= 0)
                        continue;
                    WoWObject obj = ObjectManager.GetObjectByGuid(playerInMyParty);
                    if (!obj.IsValid || obj.Type != WoWObjectType.Player)
                        continue;
                    var currentPlayer = new WoWPlayer(obj.GetBaseAddress);
                    if (!currentPlayer.IsValid || !currentPlayer.IsAlive)
                        continue;

                    //Calculate Class Variables
                    PartyHpMedian += currentPlayer.HealthPercent;
                    alivePlayers++;
                    if (currentPlayer.HealthPercent < 100)
                    {
                        DamagedPlayers.Add(currentPlayer);
                    }

                    //Add Tank
                    if (ScanForTanksTimer.IsReady && !Fight.InFight)
                    {
                        if (currentPlayer.GetUnitRole == WoWUnit.PartyRole.Tank)
                        {
                            Logging.WriteFight("Found Tank: " + currentPlayer.Name + "(" + currentPlayer.GetUnitId() + ")");
                            Tanks.Add(currentPlayer);
                        }
                        else
                            Logging.WriteFight(currentPlayer.Name + "(" + currentPlayer.GetUnitId() + ")");
                    }
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("private void ScanParty(): " + e);
            }
            finally
            {
                Memory.WowMemory.GameFrameUnLock();
            }
            PartyHpMedian /= alivePlayers;
            if (ScanForTanksTimer.IsReady && !Fight.InFight)
            {
                if (MySettings.HealSecondaryTank && Tanks.Count > 1)
                    Tank = Tanks[1];
                else if (Tanks.Count > 0)
                    Tank = Tanks[0];
                else
                    Tank = Me;
            }
        }

        //Restart Scan For Tanks Interval
        if (ScanForTanksTimer.IsReady)
            ScanForTanksTimer = new Timer(1000*60);
    }

    // For Movement Spells (always return after Casting)
    private void Patrolling()
    {
        //Log
        if (CombatMode)
        {
            Logging.WriteFight("Patrolling:");
            CombatMode = false;
        }

        if (Me.GetMove && !Usefuls.PlayerUsingVehicle)
        {
            if (MySettings.UseCatFormOOC && CatForm.IsSpellUsable && !CatForm.HaveBuff)
            {
                CatForm.Cast();
                return;
            }
        }
    }

    // For general InFight Behavior (only touch if you want to add a new method like PetManagement())
    private void Combat()
    {
        //Log
        if (!CombatMode)
        {
            Logging.WriteFight("Combat:");
            CombatMode = true;
        }

        Diagnostics.Start();
        try
        {
            if (MySettings.UseSelfHealing)
                SelfHealing();
            if (MySettings.UseDefensiveSpells)
                Defensive();
            if (MySettings.UseBuffsSpells)
                Buffs();
            if ((MySettings.UseTankHealing && TankHealing()) ||
                (MySettings.UseAOEHealing && AOEHealing()) ||
                (MySettings.UsePriorityHealing && PriorityHealing()))
                return;

            //Filler Damage
            if (!Me.GetMove && MySettings.UseSolarWrath && ObjectManager.Target.IsHostile &&
                SolarWrath.IsSpellUsable && SolarWrath.IsHostileDistanceGood)
            {
                SolarWrath.Cast();
                return;
            }
            else
            {
                Logging.WriteFileOnly(!Me.GetMove + ", " + MySettings.UseSolarWrath + ", " + Target.IsHostile + ", " + SolarWrath.IsSpellUsable + ", " + SolarWrath.IsHostileDistanceGood);
            }
            Logging.WriteFileOnly("Empty Loop");
        }
        finally
        {
            Diagnostics.Stop();
        }
    }

    // For Self-Healing (always return after Casting)
    private void SelfHealing()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            //Renewal 
            if (Me.HealthPercent < MySettings.UseRenewalBelowPercentage && Renewal.IsSpellUsable)
            {
                Renewal.Cast();
                return;
            }
            if (Me.HealthPercent < MySettings.UseFrenziedRegenerationBelowPercentage && FrenziedRegeneration.IsSpellUsable)
            {
                FrenziedRegeneration.Cast();
                return;
            }
        }
        catch (Exception e)
        {
            Logging.WriteError("private void SelfHealing(): " + e);
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    // For Defensive Spells (always return after Casting)
    private void Defensive()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            if (DefensiveTimer.IsReady || Me.HealthPercent < 20)
            {
                //Stun
                if (Target.IsStunnable && !Target.IsStunned)
                {
                    if (Me.HealthPercent < MySettings.UseMightyBashBelowPercentage && MightyBash.IsSpellUsable && MightyBash.IsHostileDistanceGood)
                    {
                        MightyBash.Cast();
                        return;
                    }
                    if (Me.HealthPercent < MySettings.UseWarStompBelowPercentage && WarStomp.IsSpellUsable && WarStomp.IsHostileDistanceGood)
                    {
                        WarStomp.Cast();
                        return;
                    }
                }
                //Mitigate Damage
                if (Me.HealthPercent < MySettings.UseStoneformBelowPercentage && Stoneform.IsSpellUsable)
                {
                    Stoneform.Cast();
                    return;
                }
            }
        }
        catch (Exception e)
        {
            Logging.WriteError("private void Defensive(): " + e);
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    // For Buffs (only return if a Cast triggered Global Cooldown)
    private void Buffs()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

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
            // Activate Incarnation: Tree of Life (Enhance upcoming Heal Spells)
            if ((PartyHpMedian < MySettings.UseIncarnationBelowPartyPercentage ||
                 Tank.HealthPercent < MySettings.UseIncarnationBelowTankPercentage) &&
                Incarnation.IsSpellUsable)
            {
                Incarnation.Cast();
            }
            // Activate Berserking (+15% Haste for 10 sec)
            if ((PartyHpMedian < MySettings.UseBerserkingBelowPartyPercentage ||
                 Tank.HealthPercent < MySettings.UseBerserkingBelowTankPercentage) &&
                Berserking.IsSpellUsable)
            {
                Berserking.Cast();
            }
            // Activate Essence of G'Hanir (Double healing from Hots for 8 seconds)
            if ((PartyHpMedian < MySettings.UseEssenceofGHanirBelowPartyPercentage ||
                 Tank.HealthPercent < MySettings.UseEssenceofGHanirBelowTankPercentage) &&
                EssenceofGHanir.IsSpellUsable)
            {
                EssenceofGHanir.Cast();
                return;
            }
            // Activate Flourish (Extends duration of all Hots by 6 seconds)
            /*if ((PartyHpMedian < MySettings.UseFlourishBelowPartyPercentage ||
                 PriorityPlayer.HealthPercent < MySettings.UseFlourishBelowTargetPercentage) && Flourish.IsSpellUsable)
            {
                Flourish.Cast(false, true, false, PriorityPlayer.GetUnitId());
                return;
            }*/
        }
        catch (Exception e)
        {
            Logging.WriteError("private void Buffs(): " + e);
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    // For Tank Healing (only return if a Cast triggered Global Cooldown)
    private bool TankHealing()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            Logging.WriteFileOnly("Tank Healing? " + Tank.Name + "[" + Tank.GetUnitId() + "]: Health =" + Tank.HealthPercent + ", Range = " + Tank.GetDistance + ", Lifebloom = " + Tank.UnitAura(Lifebloom.Ids, Me.Guid).AuraTimeLeftInMs + ", Rejuvenation = " + Tank.UnitAura(Rejuvenation.Ids, Me.Guid).AuraTimeLeftInMs + ", InCombat = " + Tank.InCombat);

            // Set Efflorescence
            if (MySettings.UseEfflorescenceAtMainTank && !Tank.GetMove && EfflorescenceTimer.IsReady && Efflorescence.IsSpellUsable &&
                CombatClass.InSpellRange(Tank, Efflorescence.MinRangeFriend, Efflorescence.MaxRangeFriend) && !Tank.HaveBuff(207386))
            {
                Logging.WriteFileOnly("Tank Healing: Efflorescence");
                Efflorescence.CastAtPosition(Tank.Position);
                EfflorescenceTimer = new Timer(1000*10 /*30*/);
                return true;
            }
            // Maintain Cenarion Ward
            if (Tank.HealthPercent < MySettings.UseCenarionWardBelowTankPercentage && CenarionWard.IsSpellUsable &&
                CombatClass.InSpellRange(Tank, CenarionWard.MinRangeFriend, CenarionWard.MaxRangeFriend) &&
                Tank.UnitAura(CenarionWard.Id, Me.Guid).AuraTimeLeftInMs < 5000)
            {
                Logging.WriteFileOnly("Tank Healing: Cenarion Ward");
                CenarionWard.CastOnUnitID(Tank.GetUnitId());
                return true;
            }
            // Maintain Lifebloom
            if (Tank.HealthPercent < MySettings.UseLifebloomBelowTankPercentage && Lifebloom.IsSpellUsable &&
                CombatClass.InSpellRange(Tank, Lifebloom.MinRangeFriend, Lifebloom.MaxRangeFriend) &&
                Tank.UnitAura(Lifebloom.Ids, ObjectManager.Me.Guid).AuraTimeLeftInMs < 5000)
            {
                Logging.WriteFileOnly("Tank Healing: Lifebloom");
                Lifebloom.CastOnUnitID(Tank.GetUnitId());
                return true;
            }
            // Maintain Rejuvenation
            if (Tank.HealthPercent < MySettings.UseRejuvenationBelowTankPercentage && Rejuvenation.IsSpellUsable &&
                CombatClass.InSpellRange(Tank, Rejuvenation.MinRangeFriend, Rejuvenation.MaxRangeFriend) &&
                Tank.UnitAura(Rejuvenation.Id, ObjectManager.Me.Guid).AuraTimeLeftInMs < 5000)
            {
                Logging.WriteFileOnly("Tank Healing: Rejuvenation");
                Rejuvenation.CastOnUnitID(Tank.GetUnitId());
                return true;
            }
        }
        catch (Exception e)
        {
            Logging.WriteError("private bool TankHealing(): " + e);
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
        return false;
    }

    // For AOE Healing (only return if a Cast triggered Global Cooldown)
    private bool AOEHealing()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            // Channel Tranquility
            if ((!Me.GetMove || Me.UnitAura(TranquilMind.Id).IsValid) && Tranquility.IsSpellUsable &&
                ObjectManager.GetFriendlyUnits().FindAll(unit => unit.HealthPercent < MySettings.UseTranquilityDamagedPlayerHealthThreshold && unit.GetDistance <= 40f).Count >= MySettings.UseTranquilityAtDamagedPlayerDensity)
            {
                Tranquility.Cast(true);
                return true;
            }

            // Cast Wild Growth
            if (WildGrowth.IsSpellUsable && ObjectManager.GetFriendlyUnits().FindAll(unit => unit.HealthPercent < MySettings.UseWildGrowthDamagedPlayerHealthThreshold && unit.GetDistance <= 30f).Count >= MySettings.UseWildGrowthAtDamagedPlayerDensity)
            {
                WildGrowth.CastOnSelf();
                return true;
            }

            Logging.WriteFileOnly("No AOE Heal casted. Tranquility would heal " + ObjectManager.GetFriendlyUnits().FindAll(unit => unit.HealthPercent < MySettings.UseTranquilityDamagedPlayerHealthThreshold && unit.GetDistance <= 40f).Count + " damaged players. Wild Growth would heal " + ObjectManager.GetFriendlyUnits().FindAll(unit => unit.HealthPercent < MySettings.UseWildGrowthDamagedPlayerHealthThreshold && unit.GetDistance <= 30f).Count + " damaged players.");
        }
        catch (Exception e)
        {
            Logging.WriteError("private bool AOEHealing(): " + e);
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
        return false;
    }

    // For Priority Healing (only return if a Cast triggered Global Cooldown)
    private bool PriorityHealing()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            //Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            DamagedPlayers = DamagedPlayers.OrderBy(x => x.HealthPercent).ToList();

            // Emergency Heals
            bool GiftoftheNaaruIsSpellUsable = GiftoftheNaaru.IsSpellUsable;
            bool IronbarkIsSpellUsable = Ironbark.IsSpellUsable;
            bool SwiftmendIsSpellUsable = Swiftmend.IsSpellUsable;
            foreach (WoWPlayer player in DamagedPlayers)
            {
                Logging.WriteFileOnly(player.Name + "[" + player.GetUnitId() + "]: Health =" + player.HealthPercent.ToString("0.0###") + ", Range = " + player.GetDistance + ", Regrowth = " + player.UnitAura(Regrowth.Ids, Me.Guid).AuraTimeLeftInMs + ", Rejuvenation = " + player.UnitAura(Rejuvenation.Ids, Me.Guid).AuraTimeLeftInMs);

                if (player.IsHostile || player.GetDistance > Swiftmend.MaxRangeFriend)
                    continue;

                //Gift of the Naaru
                if (player.HealthPercent < MySettings.UseGiftoftheNaaruBelowPercentage && GiftoftheNaaruIsSpellUsable &&
                    CombatClass.InSpellRange(player, GiftoftheNaaru.MinRangeFriend, GiftoftheNaaru.MaxRangeFriend))
                {
                    Logging.WriteFileOnly("Priority Healing: Gift of the Naaru");
                    GiftoftheNaaru.CastOnUnitID(player.GetUnitId());
                }

                //Ironbark 
                if (player.HealthPercent < MySettings.UseIronbarkBelowPercentage && IronbarkIsSpellUsable &&
                    CombatClass.InSpellRange(player, Ironbark.MinRangeFriend, Ironbark.MaxRangeFriend) &&
                    player.UnitAura(Ironbark.Id, Me.Guid).AuraTimeLeftInMs < 4000)
                {
                    Logging.WriteFileOnly("Priority Healing: Ironbark");
                    Ironbark.CastOnUnitID(player.GetUnitId());
                }

                // Cast Swiftmend
                if (player.HealthPercent < MySettings.UseSwiftmendBelowPercentage && SwiftmendIsSpellUsable &&
                    CombatClass.InSpellRange(player, Swiftmend.MinRangeFriend, Swiftmend.MaxRangeFriend))
                {
                    Logging.WriteFileOnly("Priority Healing: Swiftmend");
                    Swiftmend.CastOnUnitID(player.GetUnitId());
                    return true;
                }
            }

            // Instant Casts
            bool RejuvenationIsSpellUsable = Rejuvenation.IsSpellUsable;
            foreach (WoWPlayer player in DamagedPlayers)
            {
                // Maintain Rejuvenation
                if (player.HealthPercent < MySettings.UseRejuvenationBelowPercentage && RejuvenationIsSpellUsable &&
                    player.UnitAura(Rejuvenation.Ids, Me.Guid).AuraTimeLeftInMs < 5000)
                {
                    Logging.WriteFileOnly("Priority Healing: Rejuvenation");
                    Rejuvenation.CastOnUnitID(player.GetUnitId());
                    return true;
                }
            }

            // Non Instant Casts
            if (!Me.GetMove)
            {
                bool RegrowthIsSpellUsable = Regrowth.IsSpellUsable;
                bool HealingTouchIsSpellUsable = HealingTouch.IsSpellUsable;
                foreach (WoWPlayer player in DamagedPlayers)
                {
                    // Maintain Regrowth
                    if (!Me.GetMove && player.HealthPercent < MySettings.UseRegrowthBelowPercentage && RegrowthIsSpellUsable &&
                        player.UnitAura(Regrowth.Ids, Me.Guid).AuraTimeLeftInMs < 4000)
                    {
                        Logging.WriteFileOnly("Priority Healing: Regrowth");
                        Regrowth.CastOnUnitID(player.GetUnitId());
                        return true;
                    }
                }
                foreach (WoWPlayer player in DamagedPlayers)
                {
                    // Cast Healing Touch
                    if (!Me.GetMove && player.HealthPercent < MySettings.UseHealingTouchBelowPercentage &&
                        HealingTouchIsSpellUsable)
                    {
                        Logging.WriteFileOnly("Priority Healing: Healing Touch");
                        HealingTouch.Cast(false, false, false, player.GetUnitId());
                        //HealingTouch.CastOnUnitID(player.GetUnitId());
                        return true;
                    }
                }
            }
        }
        catch (Exception e)
        {
            Logging.WriteError("private bool PriorityHealing(): " + e);
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
        return false;
    }

    [Serializable]
    public class DruidRestorationSettings : Settings
    {
        /* General Settings */
        public bool HealSecondaryTank = false;

        /* Self Healing */
        public bool UseSelfHealing = true;
        public int UseRenewalBelowPercentage = 50;
        public int UseFrenziedRegenerationBelowPercentage = 0;

        /* Defensive Spells */
        public bool UseDefensiveSpells = false;
        public int UseMightyBashBelowPercentage = 50;
        public int UseWarStompBelowPercentage = 50;
        public int UseStoneformBelowPercentage = 50;
        //public bool UseEntanglingRoots = true;
        //public bool UseMassEntanglement = true;
        //public bool UseTyphoon = true;
        //public bool UseWildCharge = true;

        /* Buffs */
        public bool UseBuffsSpells = true;
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;
        public int UseIncarnationBelowPartyPercentage = 40;
        public int UseIncarnationBelowTankPercentage = 40;
        public int UseBerserkingBelowPartyPercentage = 50;
        public int UseBerserkingBelowTankPercentage = 25;
        public int UseEssenceofGHanirBelowPartyPercentage = 50;
        public int UseEssenceofGHanirBelowTankPercentage = 50;

        /* Tank Healing */
        public bool UseTankHealing = true;
        public bool UseEfflorescenceAtMainTank = true;
        public int UseCenarionWardBelowTankPercentage = 90;
        public int UseLifebloomBelowTankPercentage = 90;
        public int UseRejuvenationBelowTankPercentage = 100;

        /* AOE Healing */
        public bool UseAOEHealing = true;
        public int UseTranquilityAtDamagedPlayerDensity = 3;
        public int UseTranquilityDamagedPlayerHealthThreshold = 40;
        public int UseWildGrowthAtDamagedPlayerDensity = 4;
        public int UseWildGrowthDamagedPlayerHealthThreshold = 60;

        /* Priority Healing */
        public bool UsePriorityHealing = true;
        public int UseGiftoftheNaaruBelowPercentage = 20;
        public int UseIronbarkBelowPercentage = 20;
        public int UseSwiftmendBelowPercentage = 30;
        public int UseRejuvenationBelowPercentage = 100;
        public int UseRegrowthBelowPercentage = 80;
        public int UseHealingTouchBelowPercentage = 60;

        /* Offensive Spells */
        public bool UseSolarWrath = true;

        /* OOC Buffs */
        public bool UseCatFormOOC = false;
        //public bool UseDarkflightOOC = true;
        //public bool UseDashOOC = true;
        //public bool UseDisplacerBeastOOC = true;
        //public bool UseProwlOOC = true;

        public DruidRestorationSettings()
        {
            ConfigWinForm("Druid Restoration Settings");
            /* General Settings */
            AddControlInWinForm("Heal the secondary Tank", "HealSecondaryTank", "General Settings");

            /* Self Healing */
            AddControlInWinForm("Use Self Healing [False disables anything in this category]", "UseSelfHealing", "Self Healing");
            AddControlInWinForm("Cast Renewal", "UseRenewalBelowPercentage", "Self Healing", "BelowPercentage", "Life");
            AddControlInWinForm("Cast Frenzied Regeneration", "UseFrenziedRegenerationBelowPercentage", "Self Healing", "BelowPercentage", "Life");

            /* Defensive Spells */
            AddControlInWinForm("Use Defensive Spells [False disables anything in this category]", "UseDefensiveSpells", "Defensive Spells");
            AddControlInWinForm("Use Mighty Bash", "UseMightyBashBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use War Stomp", "UseWarStompBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Stoneform", "UseStoneformBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");

            /* Buffs */
            AddControlInWinForm("Use Buffs [False disables anything in this category]", "UseBuffsSpells", "Buffs");
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Buffs");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Buffs");
            AddControlInWinForm("Use Incarnation: Tree of Life", "UseIncarnationBelowPartyPercentage", "Buffs", "BelowPercentage", "Party Life");
            AddControlInWinForm("Use Incarnation: Tree of Life", "UseIncarnationBelowTankPercentage", "Buffs", "BelowPercentage", "Target Life");
            AddControlInWinForm("Use Berserking", "UseBerserkingBelowPartyPercentage", "Buffs", "BelowPercentage", "Party Life");
            AddControlInWinForm("Use Berserking", "UseBerserkingBelowTankPercentage", "Buffs", "BelowPercentage", "Target Life");
            AddControlInWinForm("Use Essence of G'Hanir", "UseEssenceofGHanirBelowPartyPercentage", "Buffs", "BelowPercentage", "Party Life");
            AddControlInWinForm("Use Essence of G'Hanir", "UseEssenceofGHanirBelowTankPercentage", "Buffs", "BelowPercentage", "Target Life");

            /* Tank Healing */
            AddControlInWinForm("Use Tank Healing [False disables anything in this category]", "UseTankHealing", "Tank Healing");
            AddControlInWinForm("Use Efflorescence", "UseEfflorescenceAtMainTank", "Tank Healing");
            AddControlInWinForm("Use Cenarion Ward", "UseCenarionWardBelowTankPercentage", "Tank Healing", "BelowPercentage", "Tank Life");
            AddControlInWinForm("Use Lifebloom", "UseLifebloomBelowTankPercentage", "Tank Healing", "BelowPercentage", "Tank Life");
            AddControlInWinForm("Use Rejuvenation", "UseRejuvenationBelowTankPercentage", "Tank Healing", "BelowPercentage", "Tank Life");

            /* AOE Healing */
            AddControlInWinForm("Use AOE Healing [False disables anything in this category]", "UseAOEHealing", "AOE Healing");
            AddControlInWinForm("Use Tranquility", "UseTranquilityAtDamagedPlayerDensity", "AOE Healing", "AtPercentage", "Damaged Players");
            AddControlInWinForm("Use Tranquility", "UseTranquilityDamagedPlayerHealthThreshold", "AOE Healing", "BelowPercentage", "Life Threshold for Damaged Players");
            AddControlInWinForm("Use Wild Growth", "UseWildGrowthAtDamagedPlayerDensity", "AOE Healing", "AtPercentage", "Damaged Players");
            AddControlInWinForm("Use Wild Growth", "UseWildGrowthDamagedPlayerHealthThreshold", "AOE Healing", "BelowPercentage", "Life Threshold for Damaged Players");

            /* Priority Healing */
            AddControlInWinForm("Use Priority Healing", "UsePriorityHealing", "Priority Healing");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaruBelowPercentage", "Priority Healing", "BelowPercentage", "Life");
            AddControlInWinForm("Use Ironbark", "UseIronbarkBelowPercentage", "Priority Healing", "BelowPercentage", "Life");
            AddControlInWinForm("Use Swiftmend", "UseSwiftmendBelowPercentage", "Priority Healing", "BelowPercentage", "Life");
            AddControlInWinForm("Use Rejuvenation", "UseRejuvenationBelowPercentage", "Priority Healing", "BelowPercentage", "Life");
            AddControlInWinForm("Use Regrowth", "UseRegrowthBelowPercentage", "Priority Healing", "BelowPercentage", "Life");
            AddControlInWinForm("Use Healing Touch", "UseHealingTouchBelowPercentage", "Priority Healing", "BelowPercentage", "Life");

            /* Offensive Spells */
            AddControlInWinForm("Use Solar Wrath", "UseSolarWrath", "Offensive Spells");

            /* OOC Buffs */
            AddControlInWinForm("Use Cat Form out of Combat", "UseCatFormOOC", "OOC Buffs");
        }

        public static DruidRestorationSettings CurrentSetting { get; set; }

        public static DruidRestorationSettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\HealerClasses\\Settings\\Druid_Restoration.xml";
            if (File.Exists(currentSettingsFile))
            {
                return
                    CurrentSetting = Load<DruidRestorationSettings>(currentSettingsFile);
            }
            return new DruidRestorationSettings();
        }
    }
}

#endregion

#region Paladin

public class PaladinHoly
{
    private static PaladinHolySettings MySettings = PaladinHolySettings.GetSettings();

    #region General Timers & Variables

    private readonly WoWItem _firstTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET);
    private readonly WoWItem _secondTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET, 2);

    private bool CombatMode = true;

    private Timer DefensiveTimer = new Timer(0);

    private List<WoWPlayer> Tanks = new List<WoWPlayer>();
    private Timer ScanForTanksTimer = new Timer(0);
    private WoWPlayer Target = new WoWPlayer(0);
    private WoWPlayer OldTarget = new WoWPlayer(0);
    private int DamagedPlayers;
    private double PartyHpMedian;

    #endregion

    #region Professions & Racial

    //private readonly Spell ArcaneTorrent = new Spell("Arcane Torrent"); //No GCD
    private readonly Spell Berserking = new Spell("Berserking"); //No GCD
    private readonly Spell BloodFury = new Spell("Blood Fury"); //No GCD
    private readonly Spell Darkflight = new Spell("Darkflight"); //No GCD
    private readonly Spell GiftoftheNaaru = new Spell("Gift of the Naaru"); //No GCD
    private readonly Spell Stoneform = new Spell("Stoneform"); //No GCD
    private readonly Spell WarStomp = new Spell("War Stomp"); //No GCD

    #endregion

    #region Talents

    private readonly Spell AuraofMercy = new Spell("Aura of Mercy"); //No GCD
    private readonly Spell AuraofSacrifice = new Spell("Aura of Sacrifice"); //No GCD
    private readonly Spell DevotionAura = new Spell("Devotion Aura"); //No GCD
    private readonly Spell JudgmentofLight = new Spell(183778);

    #endregion

    #region Buffs

    private readonly Spell Forbearance = new Spell(25771);

    #endregion

    #region Dot

    private readonly Spell JudgmentofLightDebuff = new Spell(196941);

    #endregion

    #region Artifact Spells

    private readonly Spell TyrsDeliverance = new Spell("Tyr's Deliverance");

    #endregion

    #region Offensive Spells

    private readonly Spell Judgment = new Spell("Judgment");

    #endregion

    #region Defensive Spells

    private readonly Spell BlessingofProtection = new Spell("Blessing of Protection");
    private readonly Spell BlessingofSacrifice = new Spell("Blessing of Sacrifice"); //No GCD
    private readonly Spell DivineProtection = new Spell(498);
    private readonly Spell DivineShield = new Spell("Divine Shield");

    #endregion

    #region Healing Buffs

    private readonly Spell BeaconofFaith = new Spell("Beacon of Faith");
    private readonly Spell BeaconofLight = new Spell("Beacon of Light");
    private readonly Spell BeaconofVirtue = new Spell("Beacon of Virtue");

    #endregion

    #region Healing Spells

    private readonly Spell BestowFaith = new Spell("Bestow Faith");
    private readonly Spell FlashofLight = new Spell("Flash of Light");
    private readonly Spell HolyLight = new Spell("Holy Light");
    private readonly Spell HolyPrism = new Spell("Holy Prism");
    private readonly Spell HolyShock = new Spell("Holy Shock");
    private readonly Spell LightofDawn = new Spell("Light of Dawn");
    private readonly Spell LightoftheMartyr = new Spell("Light of the Martyr");

    #endregion

    #region Healing Cooldowns

    private readonly Spell AvengingWrath = new Spell("Avenging Wrath"); //No GCD
    private readonly Spell AuraMastery = new Spell("Aura Mastery"); //No GCD
    private readonly Spell HolyAvenger = new Spell("Holy Avenger");
    private readonly Spell LayonHands = new Spell("Lay on Hands"); //No GCD
    private readonly Spell LightsHammer = new Spell("Light's Hammer");

    #endregion

    #region Utility Spells

    private readonly Spell BlessingofFreedom = new Spell("Blessing of Freedom");
    private readonly Spell DivineSteed = new Spell("Divine Steed");
    private readonly Spell Repentance = new Spell("Repentance");
    private readonly Spell RuleofLaw = new Spell("Rule of Law"); //No GCD

    #endregion

    public PaladinHoly()
    {
        Main.InternalRange = HolyLight.MaxRangeFriend;
        MySettings = PaladinHolySettings.GetSettings();
        Main.DumpCurrentSettings<PaladinHolySettings>(MySettings);

        while (Main.InternalLoop)
        {
            try
            {
                if (!ObjectManager.Me.IsDeadMe)
                {
                    //Dismount in Combat
                    if (ObjectManager.Me.InCombat && ObjectManager.Me.IsMounted)
                        MountTask.DismountMount(true);

                    if (!ObjectManager.Me.IsMounted)
                    {
                        //Prepare Tank Scan
                        if (ScanForTanksTimer.IsReady)
                        {
                            Logging.WriteFight("Scanning for Tanks:");
                            Tanks.Clear();
                        }

                        //Setup Solo
                        if (!Party.IsInGroup())
                        {
                            if (ScanForTanksTimer.IsReady)
                                Tanks.Add(ObjectManager.Me);
                            PartyHpMedian = ObjectManager.Me.HealthPercent;
                            DamagedPlayers = 1;
                            Target = ObjectManager.Me;
                        }
                        //Setup Group
                        else
                        {
                            double lowestHp = 100;
                            int alivePlayers = 0;
                            PartyHpMedian = 0;
                            DamagedPlayers = 0;
                            Target = ObjectManager.Me;

                            try
                            {
                                Memory.WowMemory.GameFrameLock();

                                foreach (UInt128 playerInMyParty in Party.GetPartyPlayersGUID())
                                {
                                    if (playerInMyParty <= 0)
                                        continue;
                                    WoWObject obj = ObjectManager.GetObjectByGuid(playerInMyParty);
                                    if (!obj.IsValid || obj.Type != WoWObjectType.Player)
                                        continue;
                                    var currentPlayer = new WoWPlayer(obj.GetBaseAddress);
                                    if (!currentPlayer.IsValid || !currentPlayer.IsAlive)
                                        continue;

                                    //Calculate Class Variables
                                    PartyHpMedian += currentPlayer.HealthPercent;
                                    alivePlayers++;
                                    if (currentPlayer.HealthPercent < 100)
                                        DamagedPlayers++;

                                    //Setup Target
                                    if (currentPlayer.HealthPercent < lowestHp &&
                                        HealerClass.InRange(currentPlayer))
                                    {
                                        lowestHp = currentPlayer.HealthPercent;
                                        Target = currentPlayer;
                                    }

                                    //Add Tank
                                    if (ScanForTanksTimer.IsReady)
                                    {
                                        if (currentPlayer.GetUnitRole == WoWUnit.PartyRole.Tank)
                                        {
                                            Logging.WriteFight("Found Tank: " + currentPlayer.Name + "(" + currentPlayer.GetUnitId() + ")");
                                            Tanks.Add(currentPlayer);
                                        }
                                        else
                                            Logging.WriteFight(currentPlayer.Name + "(" + currentPlayer.GetUnitId() + ")");
                                    }
                                }
                            }
                            finally
                            {
                                Memory.WowMemory.GameFrameUnLock();
                            }
                            PartyHpMedian /= alivePlayers;

                            //Prioritize Tank
                            if (!Tanks.Contains(Target))
                            {
                                double lowestTankHp = 100;
                                foreach (WoWPlayer tank in Tanks)
                                {
                                    if (tank.HealthPercent < MySettings.PrioritizeTankBelowPercentage &&
                                        tank.HealthPercent < lowestTankHp)
                                    {
                                        lowestTankHp = tank.HealthPercent;
                                        Target = tank;
                                    }
                                }
                            }

                            //Prioritize Me
                            if (ObjectManager.Me.HealthPercent < MySettings.PrioritizeMeBelowPercentage &&
                                Target != ObjectManager.Me)
                            {
                                Target = ObjectManager.Me;
                            }
                        }

                        //Restart Scan For Tanks Interval
                        if (ScanForTanksTimer.IsReady)
                            ScanForTanksTimer = new Timer(1000*60);

                        if (Fight.InFight || PartyHpMedian < 90)
                            Combat();
                        else
                            Patrolling();
                    }
                }
                else
                    Thread.Sleep(500);
            }
            catch
            {
            }
        }
    }

    // For Movement Spells (always return after Casting)
    private void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            //Log
            if (CombatMode)
            {
                Logging.WriteFight("Patrolling:");
                CombatMode = false;
            }

            if (ObjectManager.Me.GetMove && !Usefuls.PlayerUsingVehicle)
            {
                //Movement Buffs
                if (!Darkflight.HaveBuff && !DivineSteed.HaveBuff) //they don't stack
                {
                    if (MySettings.UseDarkflight && Darkflight.IsSpellUsable)
                    {
                        Darkflight.Cast();
                        return;
                    }
                    if (MySettings.UseDivineSteed && DivineSteed.IsSpellUsable)
                    {
                        DivineSteed.Cast();
                        return;
                    }
                }
            }
        }
    }

    // For general InFight Behavior (only touch if you want to add a new method like PetManagement())
    private void Combat()
    {
        //Log
        if (!CombatMode)
        {
            Logging.WriteFight("Combat:");
            CombatMode = true;
        }
        Healing();
        if (Defensive() || Offensive())
            return;
        Rotation();
    }

    // For Self-Healing Spells (always return after Casting)
    private bool Healing()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            //Gift of the Naaru
            if (ObjectManager.Me.HealthPercent < MySettings.UseGiftoftheNaaruBelowPercentage && GiftoftheNaaru.IsSpellUsable)
            {
                GiftoftheNaaru.Cast();
                return true;
            }
            return false;
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    // For Defensive Buffs and Livesavers (always return after Casting)
    private bool Defensive()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            if (DefensiveTimer.IsReady || ObjectManager.Me.HealthPercent < 20)
            {
                //Stun
                if (ObjectManager.Target.IsStunnable && !ObjectManager.Target.IsStunned)
                {
                    if (ObjectManager.Me.HealthPercent < MySettings.UseWarStompBelowPercentage && WarStomp.IsSpellUsable)
                    {
                        WarStomp.Cast();
                        return true;
                    }
                }
                //Mitigate Damage
                if (ObjectManager.Me.HealthPercent < MySettings.UseStoneformBelowPercentage && Stoneform.IsSpellUsable)
                {
                    Stoneform.Cast();
                    DefensiveTimer = new Timer(1000*8);
                    return true;
                }
                if (ObjectManager.Me.HealthPercent < MySettings.UseDivineProtectionBelowPercentage &&
                    DivineProtection.IsSpellUsable && !ObjectManager.Me.HaveBuff(Forbearance.Id))
                {
                    DivineProtection.CastOnSelf();
                    DefensiveTimer = new Timer(1000*8);
                    return false;
                }
            }
            //Mitigate Damage in Emergency Situations
            if (ObjectManager.Me.HealthPercent < MySettings.UseDivineShieldBelowPercentage &&
                DivineShield.IsSpellUsable && !ObjectManager.Me.HaveBuff(Forbearance.Id))
            {
                DivineShield.CastOnSelf();
                DefensiveTimer = new Timer(1000*8);
                return true;
            }
            return false;
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    // For Offensive Buffs (only return if a Cast triggered Global Cooldown)
    private bool Offensive()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

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
            if (MySettings.UseBerserking && Berserking.IsSpellUsable)
            {
                Berserking.Cast();
            }
            if (MySettings.UseBloodFury && BloodFury.IsSpellUsable)
            {
                BloodFury.Cast();
            }

            //Use Avenging Wrath when target has low health
            if (Target.HealthPercent < MySettings.UseAvengingWrathBelowTargetPercentage && AvengingWrath.IsSpellUsable)
            {
                AvengingWrath.Cast();
            }
            //Use Aura Mastery on cooldown, assuming the raid is sufficiently damaged to justify it.
            if (PartyHpMedian < MySettings.UseAuraMasteryBelowPartyPercentage && AuraMastery.IsSpellUsable)
            {
                AuraMastery.Cast();
            }
            return false;
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    // For Spots (always return after Casting)
    private void Rotation()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            //Log when Target switched
            if (OldTarget != (OldTarget = Target))
                Logging.WriteFight("new Target: " + Target.Name + "(" + Target.GetUnitId() + ")");
            //Use Lay on Hands to save dying Players
            if (Target.HealthPercent < MySettings.UseLayonHandsBelowTargetPercentage && LayonHands.IsSpellUsable &&
                CombatClass.InSpellRange(Target, LayonHands.MinRangeFriend, LayonHands.MaxRangeFriend) && !Target.HaveBuff(Forbearance.Id))
            {
                LayonHands.Cast(false, true, false, Target.GetUnitId());
                return;
            }
            //Keep Beacon of Light on the main (first) tank.
            if (MySettings.UseBeaconofLight && BeaconofLight.IsSpellUsable)
            {
                WoWPlayer mainTank = Tanks.First();
                if (CombatClass.InSpellRange(mainTank, BeaconofLight.MinRangeFriend, BeaconofLight.MaxRangeFriend) && !mainTank.HaveBuff(BeaconofLight.Ids))
                {
                    Logging.WriteFight("No active Beacon of Light, applying it to: " + mainTank.Name + "(" + mainTank.GetUnitId() + ")");
                    BeaconofLight.Cast(false, true, false, mainTank.GetUnitId());
                    return;
                }
            }
            //Keep Beacon of Faith on the side (second) tank.
            if (MySettings.UseBeaconofFaith && BeaconofFaith.IsSpellUsable)
            {
                WoWPlayer sideTank = Tanks[1] ?? ObjectManager.Me;
                if (CombatClass.InSpellRange(sideTank, BeaconofFaith.MinRangeFriend, BeaconofFaith.MaxRangeFriend) && !sideTank.HaveBuff(BeaconofFaith.Ids))
                {
                    Logging.WriteFight("No active Beacon of Faith, applying it to: " + sideTank.Name + "(" + sideTank.GetUnitId() + ")");
                    BeaconofFaith.Cast(false, true, false, sideTank.GetUnitId());
                    return;
                }
            }
            //Use Light of the Martyr to save dying Players
            if (Target.HealthPercent < MySettings.UseLightoftheMartyrBelowTargetPercentage &&
                ObjectManager.Me.HealthPercent > MySettings.UseLightoftheMartyrAbovePercentage && LightoftheMartyr.IsSpellUsable &&
                CombatClass.InSpellRange(Target, LightoftheMartyr.MinRangeFriend, LightoftheMartyr.MaxRangeFriend))
            {
                LightoftheMartyr.Cast(false, true, false, Target.GetUnitId());
                return;
            }
            //Use Holy Shock on cooldown.
            if (Target.HealthPercent < MySettings.UseHolyShockBelowTargetPercentage && HolyShock.IsSpellUsable &&
                CombatClass.InSpellRange(Target, HolyShock.MinRangeFriend, HolyShock.MaxRangeFriend))
            {
                //Holy Avenger Buff
                if (MySettings.UseHolyAvenger && HolyAvenger.IsSpellUsable)
                {
                    HolyAvenger.Cast();
                    return;
                }
                HolyShock.Cast(false, true, false, Target.GetUnitId());
                return;
            }
            //Use Blessing of Sacrifice for reducing the damage taken by the target.
            if (Target.HealthPercent < MySettings.UseBlessingofSacrificeBelowTankPercentage &&
                ObjectManager.Me.HealthPercent > MySettings.UseBlessingofSacrificeAbovePercentage && BlessingofSacrifice.IsSpellUsable &&
                CombatClass.InSpellRange(Target, BlessingofSacrifice.MinRangeFriend, BlessingofSacrifice.MaxRangeFriend))
            {
                BlessingofSacrifice.Cast(false, true, false, Target.GetUnitId());
                return;
            }
            //Use Bestow Faith on Tanks on cooldown (if you have taken this talent).
            if (BestowFaith.IsSpellUsable)
            {
                WoWPlayer lowestTank = Tanks.First();
                foreach (WoWPlayer tank in Tanks)
                {
                    if (tank.HealthPercent < lowestTank.HealthPercent)
                    {
                        lowestTank = tank;
                    }
                }
                if (lowestTank.HealthPercent < MySettings.UseBestowFaithBelowTankPercentage &&
                    CombatClass.InSpellRange(lowestTank, BestowFaith.MinRangeFriend, BestowFaith.MaxRangeFriend))
                {
                    BestowFaith.Cast(false, true, false, lowestTank.GetUnitId());
                    return;
                }
            }
            //Maintain the Judgment of Light debuff on the target (if you have taken this talent).
            if (MySettings.UseJudgment && Judgment.IsSpellUsable && ObjectManager.Target.IsHostile &&
                Judgment.IsHostileDistanceGood && !JudgmentofLight.HaveBuff && !JudgmentofLightDebuff.TargetHaveBuff)
            {
                Judgment.Cast();
                return;
            }
            //Use Light's Hammer during AoE raid damage.
            if (LightsHammer.IsSpellUsable && CombatClass.InSpellRange(Target, LightsHammer.MinRangeFriend, LightsHammer.MaxRangeFriend))
            {
                List<WoWPlayer> playersInRange = GetPlayersInRange(Target, 10f);
                int damagedPlayers = 0;
                float averageHealth = 0;
                foreach (WoWPlayer player in playersInRange)
                {
                    damagedPlayers += (player.HealthPercent < 100) ? 1 : 0;
                    averageHealth += player.HealthPercent;
                }
                averageHealth /= playersInRange.Count();
                if (damagedPlayers > MySettings.UseLightsHammerAtDamagedPlayer &&
                    averageHealth < MySettings.UseLightsHammerBelowHealth)
                {
                    LightsHammer.CastAtPosition(Target.Position);
                    return;
                }
            }
            //Use Tyr's Deliverance on cooldown, assuming the raid is sufficiently damaged to justify it.
            if (PartyHpMedian < MySettings.UseTyrsDeliveranceBelowPartyPercentage && TyrsDeliverance.IsSpellUsable && !ObjectManager.Me.GetMove)
            {
                List<WoWPlayer> playersInRange = GetPlayersInRange(ObjectManager.Target, 15f);
                foreach (WoWPlayer player in playersInRange)
                {
                    if (player.HealthPercent < 100)
                    {
                        TyrsDeliverance.Cast();
                        return;
                    }
                }
            }
            //Use Beacon of Virtue when multiple party members are taking damage
            if (BeaconofVirtue.IsSpellUsable && CombatClass.InSpellRange(Target, BeaconofVirtue.MinRangeFriend, BeaconofVirtue.MaxRangeFriend))
            {
                List<WoWPlayer> playersInRange = GetPlayersInRange(Target, 30f);
                int damagedPlayers = 0;
                float averageHealth = 0;
                foreach (WoWPlayer player in playersInRange)
                {
                    damagedPlayers += (player.HealthPercent < 100) ? 1 : 0;
                    averageHealth += player.HealthPercent;
                }
                averageHealth /= playersInRange.Count();
                if (damagedPlayers > MySettings.UseBeaconofVirtueAtDamagedPlayer &&
                    averageHealth < MySettings.UseBeaconofVirtueBelowHealth)
                {
                    BeaconofVirtue.Cast(false, true, false, Target.GetUnitId());
                    return;
                }
            }
            //Use Light of Dawn on cooldown, assuming it heals enough players to justify it.
            //TODO: Could cycle through all players of the party!
            if (LightofDawn.IsSpellUsable && CombatClass.InSpellRange(Target, 0, 10f))
            {
                List<WoWPlayer> playersInRange = GetPlayersInRange(Target, 10f);
                int damagedPlayers = 0;
                float averageHealth = 0;
                foreach (WoWPlayer player in playersInRange)
                {
                    damagedPlayers += (player.HealthPercent < 100) ? 1 : 0;
                    averageHealth += player.HealthPercent;
                }
                averageHealth /= playersInRange.Count();
                if (damagedPlayers > MySettings.UseLightofDawnAtDamagedPlayer &&
                    averageHealth < MySettings.UseLightofDawnBelowHealth)
                {
                    LightofDawn.Cast(false, true, false, Target.GetUnitId());
                    return;
                }
            }
            //Use Holy Prism on cooldown, assuming it heals enough players to justify it (if you have taken this talent).
            if (HolyPrism.IsSpellUsable && ObjectManager.Target.IsHostile && HolyPrism.IsHostileDistanceGood)
            {
                List<WoWPlayer> playersInRange = GetPlayersInRange(ObjectManager.Target, 15f);
                int damagedPlayers = 0;
                float averageHealth = 0;
                foreach (WoWPlayer player in playersInRange)
                {
                    damagedPlayers += (player.HealthPercent < 100) ? 1 : 0;
                    averageHealth += player.HealthPercent;
                }
                averageHealth /= playersInRange.Count();
                if (damagedPlayers > MySettings.UseHolyPrismAtDamagedPlayers &&
                    averageHealth < MySettings.UseHolyPrismBelowHealth)
                {
                    HolyPrism.Cast();
                    return;
                }
            }
            //Use Flash of Light for high damage
            if (Target.HealthPercent < MySettings.UseFlashofLightBelowTargetPercentage && FlashofLight.IsSpellUsable && !ObjectManager.Me.GetMove &&
                CombatClass.InSpellRange(Target, FlashofLight.MinRangeFriend, FlashofLight.MaxRangeFriend))
            {
                FlashofLight.Cast(false, true, false, Target.GetUnitId());
                return;
            }
            //Use Holy Light for low damage
            if (Target.HealthPercent < MySettings.UseHolyLightBelowTargetPercentage && HolyLight.IsSpellUsable && !ObjectManager.Me.GetMove &&
                CombatClass.InSpellRange(Target, HolyLight.MinRangeFriend, HolyLight.MaxRangeFriend) && AvengingWrath.HaveBuff)
            {
                HolyLight.Cast(false, true, false, Target.GetUnitId());
                return;
            }
            //Use Rule of Law when the target is out of range.
            if (MySettings.UseRuleofLaw && RuleofLaw.IsSpellUsable &&
                Target.GetDistance > 40 && Target.GetDistance <= 60)
            {
                RuleofLaw.Cast();
            }
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    //Get Players in Range around Unit
    private List<WoWPlayer> GetPlayersInRange(WoWUnit target, float range)
    {
        List<WoWPlayer> allPlayers = ObjectManager.GetObjectWoWPlayer();
        List<WoWPlayer> playerInSpellRange = new List<WoWPlayer>();
        foreach (WoWPlayer player in allPlayers)
            if (player.Position.DistanceTo(target.Position) <= range)
                playerInSpellRange.Add(player);
        return playerInSpellRange;
    }

    //Get Players in Range around Player
    private List<WoWPlayer> GetPlayersInRange(WoWPlayer target, float range)
    {
        List<WoWPlayer> allPlayers = ObjectManager.GetObjectWoWPlayer();
        List<WoWPlayer> playerInSpellRange = new List<WoWPlayer>();
        foreach (WoWPlayer player in allPlayers)
            if (player.Position.DistanceTo(target.Position) <= range)
                playerInSpellRange.Add(player);
        return playerInSpellRange;
    }

    #region Nested type: PaladinHolySettings

    [Serializable]
    public class PaladinHolySettings : Settings
    {
        /* Professions & Racials */
        //public bool UseArcaneTorrent = true;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseDarkflight = true;
        public int UseGiftoftheNaaruBelowPercentage = 50;
        public int UseStoneformBelowPercentage = 50;
        public int UseWarStompBelowPercentage = 50;

        /* Artifact Spells */
        public int UseTyrsDeliveranceBelowPartyPercentage = 50;

        /* Offensive Spells */
        public bool UseHolyShock = true;
        public bool UseJudgment = true;

        /* Defensive Spells */
        public int UseBlessingofSacrificeAbovePercentage = 80;
        public int UseBlessingofSacrificeBelowTankPercentage = 20;
        public int UseDivineProtectionBelowPercentage = 70;
        public int UseDivineShieldBelowPercentage = 10;

        /* Healing Buffs */
        public bool UseBeaconofFaith = true;
        public bool UseBeaconofLight = true;
        public int UseBeaconofVirtueAtDamagedPlayer = 3;
        public int UseBeaconofVirtueBelowHealth = 60;
        /* Healing Spells */
        public int UseBestowFaithBelowTankPercentage = 40;
        public int UseFlashofLightBelowTargetPercentage = 60;
        public int UseHolyLightBelowTargetPercentage = 90;
        public int UseHolyPrismAtDamagedPlayers = 3;
        public int UseHolyPrismBelowHealth = 60;
        public int UseHolyShockBelowTargetPercentage = 80;
        public int UseLightofDawnAtDamagedPlayer = 3;
        public int UseLightofDawnBelowHealth = 60;
        public int UseLightoftheMartyrAbovePercentage = 80;
        public int UseLightoftheMartyrBelowTargetPercentage = 20;
        /* Healing Cooldowns */
        public int UseAvengingWrathBelowTargetPercentage = 40;
        public int UseAuraMasteryBelowPartyPercentage = 40;
        public bool UseHolyAvenger = true;
        public int UseLayonHandsBelowTargetPercentage = 10;
        public int UseLightsHammerAtDamagedPlayer = 5;
        public int UseLightsHammerBelowHealth = 60;

        /* Utility Spells */
        public bool UseDivineSteed = true;
        public bool UseRuleofLaw = true;

        /* Game Settings */
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;
        public int PrioritizeTankBelowPercentage = 40;
        public int PrioritizeMeBelowPercentage = 40;

        public PaladinHolySettings()
        {
            ConfigWinForm("Paladin Protection Settings");
            /* Professions & Racials */
            //AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Darkflight", "UseDarkflight", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaruBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            AddControlInWinForm("Use Stone Form", "UseStoneformBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            AddControlInWinForm("Use War Stomp", "UseWarStompBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            /* Artifact Spells */
            AddControlInWinForm("Use Tyr's Deliverance", "UseTyrsDeliveranceBelowPartyPercentage", "Artifact Spells", "BelowPercentage", "Party Life");
            /* Offensive Spells */
            AddControlInWinForm("Use Holy Shock", "UseHolyShock", "Offensive Spells");
            AddControlInWinForm("Use Judgment", "UseJudgment", "Offensive Spells");
            /* Defensive Spells */
            AddControlInWinForm("Use Blessing of Sacrifice", "UseBlessingofSacrificeAbovePercentage", "Defensive Spells", "AbovePercentage", "Life");
            AddControlInWinForm("Use Blessing of Sacrifice", "UseBlessingofSacrificeBelowTankPercentage", "Defensive Spells", "BelowPercentage", "Tank Life");
            AddControlInWinForm("Use Divine Protection", "UseDivineProtectionBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Divine Shield", "UseDivineShieldBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            /* Healing Buffs */
            AddControlInWinForm("Use Beacon of Faith", "UseBeaconofFaith", "Healing Buffs");
            AddControlInWinForm("Use Beacon of Light", "UseBeaconofLight", "Healing Buffs");
            AddControlInWinForm("Use Beacon of Virtue", "UseBeaconofVirtueAtDamagedPlayer", "Healing Buffs", "AtPercentage", "Damaged Players");
            AddControlInWinForm("Use Beacon of Virtue", "UseBeaconofVirtueBelowHealth", "Healing Buffs", "BelowPercentage", "~Health of Damaged Players");
            /* Healing Spells */
            AddControlInWinForm("Use Bestow Faith", "UseBestowFaithBelowTankPercentage", "Healing Spells", "BelowPercentage", "Tank Life");
            AddControlInWinForm("Use Flash of Light", "UseFlashofLightBelowTargetPercentage", "Healing Spells", "BelowPercentage", "Target Life");
            AddControlInWinForm("Use Holy Light", "UseHolyLightBelowTargetPercentage", "Healing Spells", "BelowPercentage", "Target Life");
            AddControlInWinForm("Use Holy Prism", "UseHolyPrismAtDamagedPlayers", "Healing Spells", "AtPercentage", "Damaged Players");
            AddControlInWinForm("Use Holy Prism", "UseHolyPrismBelowHealth", "Healing Spells", "BelowPercentage", "~Health of Damaged Players");
            AddControlInWinForm("Use Holy Shock", "UseHolyShockBelowTargetPercentage", "Healing Spells", "BelowPercentage", "Target Life");
            AddControlInWinForm("Use Light of Dawn", "UseLightofDawnAtDamagedPlayer", "Healing Spells", "AtPercentage", "Damaged Players");
            AddControlInWinForm("Use Light of Dawn", "UseLightofDawnBelowHealth", "Healing Spells", "BelowPercentage", "~Health of Damaged Players");
            AddControlInWinForm("Use Light of the Martyr", "UseLightoftheMartyrAbovePercentage", "Healing Spells", "AbovePercentage", "Life");
            AddControlInWinForm("Use Light of the Martyr", "UseLightoftheMartyrBelowTargetPercentage", "Healing Spells", "BelowPercentage", "Target Life");
            /* Healing Cooldowns */
            AddControlInWinForm("Use Avenging Wrath", "UseAvengingWrathBelowTargetPercentage", "Healing Cooldowns", "BelowPercentage", "Target Life");
            AddControlInWinForm("Use Aura Mastery", "UseAuraMasteryBelowPartyPercentage", "Healing Cooldowns", "BelowPercentage", "Party Life");
            AddControlInWinForm("Use Holy Avenger", "UseHolyAvenger", "Healing Cooldowns");
            AddControlInWinForm("Use Lay on Hands", "UseLayonHandsBelowTargetPercentage", "Healing Cooldowns", "BelowPercentage", "Target Life");
            AddControlInWinForm("Use Light's Hammer", "UseLightsHammerAtDamagedPlayer", "Healing Cooldowns", "AtPercentage", "Damaged Players");
            AddControlInWinForm("Use Light's Hammer", "UseLightsHammerBelowHealth", "Healing Cooldowns", "BelowPercentage", "~Health of Damaged Players");
            /* Utility Spells */
            AddControlInWinForm("Use Divine Steed", "UseDivineSteed", "Utility Spells");
            AddControlInWinForm("Use Rule of Law", "UseRuleofLaw", "Utility Spells");
            /* Game Settings */
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");
            AddControlInWinForm("Prioritize Tank", "PrioritizeTankBelowPercentage", "Game Settings", "BelowPercentage");
            AddControlInWinForm("Prioritize Me", "PrioritizeMeBelowPercentage", "Game Settings", "BelowPercentage");
        }

        public static PaladinHolySettings CurrentSetting { get; set; }

        public static PaladinHolySettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\HealerClasses\\Settings\\Paladin_Holy.xml";
            if (File.Exists(currentSettingsFile))
            {
                return CurrentSetting = Load<PaladinHolySettings>(currentSettingsFile);
            }
            return new PaladinHolySettings();
        }
    }

    #endregion
}

#endregion

#region Shaman

public class ShamanRestoration
{
    private static ShamanRestorationSettings MySettings = ShamanRestorationSettings.GetSettings();

    #region General Timers & Variables

    private readonly WoWItem _firstTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET);
    private readonly WoWItem _secondTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET, 2);

    private bool CombatMode = true;

    private Timer DefensiveTimer = new Timer(0);

    private WoWPlayer Tank = new WoWPlayer(0);
    private WoWPlayer Target = new WoWPlayer(0);
    private int DamagedPlayers;
    private double PartyHpMedian;

    #endregion

    #region Professions & Racial

    //private readonly Spell ArcaneTorrent = new Spell("Arcane Torrent"); //No GCD
    private readonly Spell Berserking = new Spell("Berserking"); //No GCD
    private readonly Spell BloodFury = new Spell("Blood Fury"); //No GCD
    private readonly Spell Darkflight = new Spell("Darkflight"); //No GCD
    private readonly Spell GiftoftheNaaru = new Spell("Gift of the Naaru"); //No GCD
    private readonly Spell Stoneform = new Spell("Stoneform"); //No GCD
    private readonly Spell WarStomp = new Spell("War Stomp"); //No GCD

    #endregion

    #region Talents

    //private readonly Spell Deluge = new Spell("Deluge");
    //private readonly Spell HighTide = new Spell("High Tide");

    #endregion

    #region Artifact Spells

    private readonly Spell GiftoftheQueen = new Spell("Gift of the Queen");

    #endregion

    #region Offensive Spells

    private readonly Spell Bloodlust = new Spell("Bloodlust"); //No GCD
    private readonly Spell Heroism = new Spell("Heroism"); //No GCD

    #endregion

    #region Healing Buffs

    private readonly Spell Riptide = new Spell("Riptide");

    #endregion

    #region Healing Spells

    private readonly Spell ChainHeal = new Spell("Chain Heal");
    private readonly Spell HealingRain = new Spell("Healing Rain");
    private readonly Spell HealingSurge = new Spell("Healing Surge");
    private readonly Spell HealingWave = new Spell("Healing Wave");

    #endregion

    #region Healing Cooldowns

    //private readonly Spell AncestralGuidance = new Spell("Ancestral Guidance");
    //private readonly Spell CloudburstTotem  = new Spell("Cloudburst Totem");
    private readonly Spell EarthenShieldTotem = new Spell("Earthen Shield Totem");
    private Timer EarthenShieldTotemCooldown = new Timer(0);
    private readonly Spell HealingStreamTotem = new Spell("Healing Stream Totem");
    private Timer HealingStreamTotemCooldown = new Timer(0);
    private readonly Spell HealingTideTotem = new Spell("Healing Tide Totem");
    private Timer HealingTideTotemCooldown = new Timer(0);
    private readonly Spell SpiritLinkTotem = new Spell("Spirit Link Totem");
    private Timer SpiritLinkTotemCooldown = new Timer(0);
    //private readonly Spell SpiritwalkersGrace = new Spell("Spiritwalker's Grace");//No GCD

    #endregion

    #region Utility Spells

    private readonly Spell GhostWolf = new Spell("Ghost Wolf");
    private readonly Spell WindRushTotem = new Spell("Wind Rush Totem");

    #endregion

    public ShamanRestoration()
    {
        Main.InternalRange = HealingSurge.MaxRangeFriend;
        MySettings = ShamanRestorationSettings.GetSettings();
        Main.DumpCurrentSettings<ShamanRestorationSettings>(MySettings);

        while (Main.InternalLoop)
        {
            try
            {
                if (!ObjectManager.Me.IsDeadMe)
                {
                    //Dismount in Combat
                    if (ObjectManager.Me.InCombat && ObjectManager.Me.IsMounted)
                        MountTask.DismountMount(true);

                    if (!ObjectManager.Me.IsMounted)
                    {
                        //Setup Solo
                        if (!Party.IsInGroup())
                        {
                            Tank = ObjectManager.Me;
                            PartyHpMedian = ObjectManager.Me.HealthPercent;
                            DamagedPlayers = 1;
                            Target = ObjectManager.Me;
                        }
                        //Setup Group
                        else
                        {
                            double lowestHp = 100;
                            int alivePlayers = 0;
                            PartyHpMedian = 0;
                            DamagedPlayers = 0;
                            Target = Tank;

                            try
                            {
                                Memory.WowMemory.GameFrameLock();

                                foreach (UInt128 playerInMyParty in Party.GetPartyPlayersGUID())
                                {
                                    if (playerInMyParty <= 0)
                                        continue;
                                    WoWObject obj = ObjectManager.GetObjectByGuid(playerInMyParty);
                                    if (!obj.IsValid || obj.Type != WoWObjectType.Player)
                                        continue;
                                    var currentPlayer = new WoWPlayer(obj.GetBaseAddress);
                                    if (!currentPlayer.IsValid || !currentPlayer.IsAlive)
                                        continue;

                                    //Calculate Class Variables
                                    PartyHpMedian += currentPlayer.HealthPercent;
                                    alivePlayers++;
                                    if (currentPlayer.HealthPercent < 100)
                                        DamagedPlayers++;

                                    //Setup Target
                                    if (currentPlayer.HealthPercent < lowestHp && HealerClass.InRange(currentPlayer))
                                    {
                                        lowestHp = currentPlayer.HealthPercent;
                                        Target = currentPlayer;
                                    }

                                    //Setup Tank
                                    if (currentPlayer.GetUnitRole == WoWUnit.PartyRole.Tank && Tank != currentPlayer)
                                    {
                                        Logging.WriteFight("New Tank: " + currentPlayer.Name);
                                        Tank = currentPlayer;
                                    }
                                }
                            }
                            finally
                            {
                                Memory.WowMemory.GameFrameUnLock();
                            }
                            PartyHpMedian /= alivePlayers;

                            if (Target.Guid > 0)
                            {
                                //Prioritize Me
                                if (Target != ObjectManager.Me && ObjectManager.Me.HealthPercent <= MySettings.PrioritizeMeBelowPercentage)
                                {
                                    Target = ObjectManager.Me;
                                }

                                //Prioritize Tank
                                if (Tank != null && Target != Tank &&
                                    (Tank.HealthPercent <= MySettings.PrioritizeTankBelowPercentage) || Target.HealthPercent == 100)
                                {
                                    Target = Tank;
                                }
                            }
                        }

                        if (Fight.InFight || PartyHpMedian < 90)
                            Combat();
                        else
                            Patrolling();
                    }
                }
                else
                    Thread.Sleep(500);
            }
            catch
            {
            }
        }
    }

    // For Movement Spells (always return after Casting)
    private void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            //Log
            if (CombatMode)
            {
                Logging.WriteFight("Patrolling:");
                CombatMode = false;
            }

            if (ObjectManager.Me.GetMove && !Usefuls.PlayerUsingVehicle)
            {
                //Movement Buffs
                if (!Darkflight.HaveBuff) // doesn't stack
                {
                    if (MySettings.UseDarkflight && Darkflight.IsSpellUsable) //they don't stack
                    {
                        Darkflight.Cast();
                        return;
                    }
                    if (WindRushTotem.IsSpellUsable && MySettings.UseWindRushTotem)
                    {
                        WindRushTotem.CastAtPosition(ObjectManager.Me.Position);
                        return;
                    }
                }

                //Ghost Wolf
                if (!GhostWolf.HaveBuff && GhostWolf.IsSpellUsable && MySettings.UseGhostWolf)
                {
                    GhostWolf.Cast();
                    return;
                }
            }
        }
    }

    // For general InFight Behavior (only touch if you want to add a new method like PetManagement())
    private void Combat()
    {
        //Log
        if (!CombatMode)
        {
            Logging.WriteFight("Combat:");
            CombatMode = true;
        }
        Healing();
        if (Defensive() || Offensive())
            return;
        Rotation();
    }

    // For Self-Healing Spells (always return after Casting)
    private bool Healing()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            //Gift of the Naaru
            if (ObjectManager.Me.HealthPercent < MySettings.UseGiftoftheNaaruBelowPercentage && GiftoftheNaaru.IsSpellUsable)
            {
                GiftoftheNaaru.Cast();
                return true;
            }
            return false;
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    // For Defensive Buffs and Livesavers (always return after Casting)
    private bool Defensive()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            if (DefensiveTimer.IsReady || ObjectManager.Me.HealthPercent < 20)
            {
                //Stun
                if (ObjectManager.Target.IsStunnable && !ObjectManager.Target.IsStunned)
                {
                    if (ObjectManager.Me.HealthPercent < MySettings.UseWarStompBelowPercentage && WarStomp.IsSpellUsable)
                    {
                        WarStomp.Cast();
                        return true;
                    }
                }
                //Mitigate Damage
                if (ObjectManager.Me.HealthPercent < MySettings.UseStoneformBelowPercentage && Stoneform.IsSpellUsable)
                {
                    Stoneform.Cast();
                    DefensiveTimer = new Timer(1000*8);
                    return true;
                }
            }
            //Mitigate Damage in Emergency Situations
            return false;
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    // For Offensive Buffs (only return if a Cast triggered Global Cooldown)
    private bool Offensive()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

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
            if (MySettings.UseBerserking && Berserking.IsSpellUsable)
            {
                Berserking.Cast();
            }
            if (MySettings.UseBloodFury && BloodFury.IsSpellUsable)
            {
                BloodFury.Cast();
            }
            //Bloodlust && Heroism
            if (Bloodlust.IsSpellUsable && !ObjectManager.Me.HaveBuff(57724) && MySettings.UseBloodlustHeroism)
            {
                Bloodlust.Cast();
            }
            if (Heroism.IsSpellUsable && !ObjectManager.Me.HaveBuff(57723) && MySettings.UseBloodlustHeroism)
            {
                Heroism.Cast();
            }
            return false;
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    // For the Ability Priority Logic
    private void Rotation()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            //Heal Buffs
            if ( /*!Tank.HaveBuff(61295)*/!Tank.UnitAura(61295, ObjectManager.Me.Guid).IsValid && Riptide.IsSpellUsable && Tank.HealthPercent <= MySettings.UseRiptideAtTankPercentage &&
                                          CombatClass.InSpellRange(Tank, Riptide.MinRangeFriend, Riptide.MaxRangeFriend) && MySettings.UseRiptide)
            {
                Riptide.Cast(false, true, false, Tank.GetUnitId());
                return;
            }
            if ( /*!Target.HaveBuff(61295)*/!Target.UnitAura(61295, ObjectManager.Me.Guid).IsValid && Riptide.IsSpellUsable && Target.HealthPercent <= MySettings.UseRiptideBelowPercentage &&
                                            CombatClass.InSpellRange(Target, Riptide.MinRangeFriend, Riptide.MaxRangeFriend) && MySettings.UseRiptide)
            {
                Riptide.Cast(false, true, false, Target.GetUnitId());
                return;
            }

            //Emergency Heals
            if (Target.HealthPercent <= MySettings.UseHealingSurgeBelowPercentage &&
                (!MySettings.UseHealingSurgeWithTidalWavesOnly || /*!Target.HaveBuff(53390)*/ !Target.UnitAura(53390, ObjectManager.Me.Guid).IsValid) &&
                CombatClass.InSpellRange(Target, HealingSurge.MinRangeFriend, HealingSurge.MaxRangeFriend) &&
                HealingSurge.IsSpellUsable && MySettings.UseHealingSurge)
            {
                HealingSurge.Cast(false, true, false, Target.GetUnitId());
                return;
            }

            //Artifact AOE Heal
            if (PartyHpMedian < MySettings.UseGiftoftheQueenBelowPartyPercentage && GiftoftheQueen.IsSpellUsable &&
                Target.GetPlayerInSpellRange(12f) > MySettings.UseGiftoftheQueenAbovePlayerDensity)
            {
                GiftoftheQueen.CastAtPosition(Target.Position);
                return;
            }

            //Heal Cooldowns
            if (Target.HealthPercent <= MySettings.UseSpiritLinkTotemBelowPercentage && SpiritLinkTotemCooldown.IsReady &&
                CombatClass.InSpellRange(Target, SpiritLinkTotem.MinRangeFriend, SpiritLinkTotem.MaxRangeFriend) &&
                MySettings.UseSpiritLinkTotem && SpiritLinkTotem.KnownSpell)
            {
                SpiritLinkTotem.CastAtPosition(Target.Position);
                SpiritLinkTotemCooldown = new Timer(1000*180);
                return;
            }
            if (PartyHpMedian <= MySettings.UseHealingTideTotemAtPartyPercentage && HealingTideTotemCooldown.IsReady &&
                MySettings.UseHealingTideTotem && HealingTideTotem.KnownSpell)
            {
                HealingTideTotem.Cast();
                HealingTideTotemCooldown = new Timer(1000*180);
                return;
            }
            if (HealingStreamTotemCooldown.IsReady && PartyHpMedian <= MySettings.UseHealingStreamTotemAtPartyPercentage &&
                MySettings.UseHealingStreamTotem && HealingStreamTotem.KnownSpell)
            {
                HealingStreamTotem.Cast();
                HealingStreamTotemCooldown = new Timer(1000*30);
                return;
            }
            if (EarthenShieldTotemCooldown.IsReady && Target.HealthPercent <= MySettings.UseEarthenShieldTotemBelowPercentage &&
                CombatClass.InSpellRange(Target, EarthenShieldTotem.MinRangeFriend, EarthenShieldTotem.MaxRangeFriend) &&
                MySettings.UseEarthenShieldTotem && EarthenShieldTotem.KnownSpell)
            {
                EarthenShieldTotem.CastAtPosition(Target.Position);
                EarthenShieldTotemCooldown = new Timer(1000*60);
                return;
            }
            if (Target.GetPlayerInSpellRange(10f) >= MySettings.UseHealingRainAtPlayerDensity &&
                PartyHpMedian <= MySettings.UseHealingRainAtPartyPercentage && HealingRain.IsSpellUsable &&
                CombatClass.InSpellRange(Target, HealingRain.MinRangeFriend, HealingRain.MaxRangeFriend) && MySettings.UseHealingRain)
            {
                //DEBUG
                Logging.Write("Target.GetPlayerInSpellRange(10f) == " + Target.GetPlayerInSpellRange(10f));

                HealingRain.Cast(false, true, false, Target.GetUnitId());
                return;
            }

            //Filler
            if (Target.HealthPercent <= MySettings.UseHealingWaveWithTidalWavesBelowPercentage && /*!Target.HaveBuff(53390)*/ !Target.UnitAura(53390, ObjectManager.Me.Guid).IsValid &&
                CombatClass.InSpellRange(Target, HealingWave.MinRangeFriend, HealingWave.MaxRangeFriend) &&
                HealingWave.IsSpellUsable && MySettings.UseHealingWave)
            {
                HealingWave.Cast(false, true, false, Target.GetUnitId());
                return;
            }
            if (DamagedPlayers >= MySettings.UseChainHealAtDamagedPlayers && PartyHpMedian <= MySettings.UseChainHealAtPartyPercentage &&
                CombatClass.InSpellRange(Target, ChainHeal.MinRangeFriend, ChainHeal.MaxRangeFriend) &&
                ChainHeal.IsSpellUsable && MySettings.UseChainHeal)
            {
                ChainHeal.Cast(false, true, false, Target.GetUnitId());
                return;
            }
            if (Target.HealthPercent <= MySettings.UseHealingWaveBelowPercentage &&
                CombatClass.InSpellRange(Target, HealingWave.MinRangeFriend, HealingWave.MaxRangeFriend) &&
                HealingWave.IsSpellUsable && MySettings.UseHealingWave)
            {
                HealingWave.Cast(false, true, false, Target.GetUnitId());
                return;
            }
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    #region Nested type: ShamanRestorationSettings

    [Serializable]
    public class ShamanRestorationSettings : Settings
    {
        /* Professions & Racials */
        //public bool UseArcaneTorrent = true;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseDarkflight = true;
        public int UseGiftoftheNaaruBelowPercentage = 50;
        public int UseStoneformBelowPercentage = 50;
        public int UseWarStompBelowPercentage = 50;

        /* Artifact Spells */
        public int UseGiftoftheQueenBelowPartyPercentage = 50;
        public int UseGiftoftheQueenAbovePlayerDensity = 3;

        /* Offensive Spells */
        public bool UseBloodlustHeroism = false;

        /* Healing Buffs */
        public bool UseRiptide = true;
        public int UseRiptideBelowPercentage = 95;
        public int UseRiptideAtTankPercentage = 95;
        /* Healing Spells */
        public bool UseChainHeal = true;
        public int UseChainHealAtDamagedPlayers = 4;
        public int UseChainHealAtPartyPercentage = 60;
        public bool UseHealingRain = true;
        public int UseHealingRainAtPlayerDensity = 3;
        public int UseHealingRainAtPartyPercentage = 60;
        public bool UseHealingSurge = true;
        public bool UseHealingSurgeWithTidalWavesOnly = true;
        public int UseHealingSurgeBelowPercentage = 40;
        public bool UseHealingWave = true;
        public int UseHealingWaveBelowPercentage = 95;
        public int UseHealingWaveWithTidalWavesBelowPercentage = 60;
        /* Healing Cooldowns */
        public bool UseEarthenShieldTotem = true;
        public int UseEarthenShieldTotemBelowPercentage = 95;
        public bool UseHealingStreamTotem = true;
        public int UseHealingStreamTotemAtPartyPercentage = 95;
        public bool UseHealingTideTotem = true;
        public int UseHealingTideTotemAtPartyPercentage = 40;
        public bool UseSpiritLinkTotem = true;
        public int UseSpiritLinkTotemBelowPercentage = 20;

        /* Utility Spells */
        public bool UseGhostWolf = true;
        public bool UseWindRushTotem = true;

        /* Game Settings */
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;
        public int PrioritizeTankBelowPercentage = 40;
        public int PrioritizeMeBelowPercentage = 40;

        public ShamanRestorationSettings()
        {
            ConfigWinForm("Shaman Restoration Settings");
            /* Professions & Racials */
            //AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Darkflight", "UseDarkflight", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaruBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            AddControlInWinForm("Use Stone Form", "UseStoneformBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            AddControlInWinForm("Use War Stomp", "UseWarStompBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            /* Artifact Spells */
            AddControlInWinForm("Use Gift of the Queen", "UseGiftoftheQueenBelowPartyPercentage", "Artifact Spells", "BelowPercentage", "Party Life");
            AddControlInWinForm("Use Gift of the Queen", "UseGiftoftheQueenAbovePlayerDensity", "Artifact Spells", "AbovePercentage", "Players");
            /* Offensive Spells */
            AddControlInWinForm("Use Bloodlust / Heroism", "UseBloodlustHeroism", "Offensive Spells");
            /* Healing Buffs */
            AddControlInWinForm("Use Riptide", "UseRiptide", "Healing Buffs");
            AddControlInWinForm("Use Riptide", "UseRiptideBelowPercentage", "Healing Buffs", "BelowPercentage", "Life");
            AddControlInWinForm("Use Riptide On Tank\n(highest possible priority)", "UseRiptideAtTankPercentage", "Healing Buffs", "BelowPercentage", "Life");
            /* Healing Spells */
            AddControlInWinForm("Use Healing Rain", "UseHealingRain", "Healing Spells");
            AddControlInWinForm("Use Healing Rain If Damaged Players stay together", "UseHealingRainAtPlayerDensity", "Healing Spells", "AbovePercentage", "Players"); //TODO add AbovePercentage alternative
            AddControlInWinForm("Use Healing Rain If Party Average", "UseHealingRainAtPartyPercentage", "Healing Spells", "BelowPercentage", "Party Life");
            AddControlInWinForm("Use Chain Heal", "UseChainHeal", "Healing Spells");
            AddControlInWinForm("Use Chain Heal If Damaged Players Count Reaches", "UseChainHealAtDamagedPlayers", "Healing Spells"); //TODO add AbovePercentage alternative
            AddControlInWinForm("Use Chain Heal If Party Average", "UseChainHealAtPartyPercentage", "Healing Spells", "BelowPercentage", "Party Life");
            AddControlInWinForm("Use Healing Surge", "UseHealingSurge", "Healing Spells");
            AddControlInWinForm("Use Healing Surge Only While Tidal Waves Buff Is Active", "UseHealingSurgeWithTidalWavesOnly", "Healing Spells");
            AddControlInWinForm("Use Healing Surge", "UseHealingSurgeBelowPercentage", "Healing Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Healing Wave", "UseHealingWave", "Healing Spells");
            AddControlInWinForm("Use Healing Wave", "UseHealingWaveBelowPercentage", "Healing Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Healing Wave\nWhile Tidal Waves Buff Is Active", "UseHealingWaveWithTidalWavesBelowPercentage", "Healing Spells", "BelowPercentage", "Life");
            /* Healing Cooldowns */
            AddControlInWinForm("Use Earthen Shield Totem", "UseEarthenShieldTotem", "Healing Cooldowns");
            AddControlInWinForm("Use Earthen Shield Totem If Party Average", "UseEarthenShieldTotemBelowPercentage", "Healing Cooldowns", "BelowPercentage", "Life");
            AddControlInWinForm("Use Healing Stream Totem", "UseHealingStreamTotem", "Healing Cooldowns");
            AddControlInWinForm("Use Healing Stream Totem If Party Average", "UseHealingStreamTotemAtPartyPercentage", "Healing Cooldowns", "BelowPercentage", "Party Life");
            AddControlInWinForm("Use Healing Tide Totem", "UseHealingTideTotem", "Healing Cooldowns");
            AddControlInWinForm("Use Healing Tide Totem If Party Average", "UseHealingTideTotemAtPartyPercentage", "Healing Cooldowns", "BelowPercentage", "Party Life");
            AddControlInWinForm("Use Spirit Link Totem", "UseSpiritLinkTotem", "Healing Cooldowns");
            AddControlInWinForm("Use Spirit Link Totem", "UseSpiritLinkTotemBelowPercentage", "Healing Cooldowns", "BelowPercentage", "Life");
            /* Utility Spells */
            AddControlInWinForm("Use Ghost Wolf", "UseGhostWolf", "Utility Spells");
            AddControlInWinForm("Use Wind Rush Totem", "UseWindRushTotem", "Utility Spells");
            /* Game Settings */
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");
            AddControlInWinForm("Prioritize Tank", "PrioritizeTankBelowPercentage", "Game Settings", "BelowPercentage");
            AddControlInWinForm("Prioritize Me", "PrioritizeMeBelowPercentage", "Game Settings", "BelowPercentage");
        }

        public static ShamanRestorationSettings CurrentSetting { get; set; }

        public static ShamanRestorationSettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\HealerClasses\\Settings\\Shaman_Restoration.xml";
            if (File.Exists(currentSettingsFile))
            {
                return
                    CurrentSetting = Load<ShamanRestorationSettings>(currentSettingsFile);
            }
            return new ShamanRestorationSettings();
        }
    }

    #endregion
}

#endregion

#region Priest

public class PriestDiscipline
{
    private readonly PriestDisciplineSettings _mySettings = PriestDisciplineSettings.GetSettings();

    public PriestDiscipline()
    {
        Main.InternalRange = 30f;

        while (Main.InternalLoop)
        {
            try
            {
                Thread.Sleep(100);
                if (ObjectManager.Me.IsDead || Usefuls.IsLoading || !Usefuls.InGame || ObjectManager.Me.IsMounted)
                    continue;

                List<WoWUnit> healingList = ObjectManager.GetFriendlyUnits();

                if (healingList.Count == 1)
                {
                    if (ObjectManager.Me.HealthPercent < 100)
                        HealingFight(ObjectManager.Me);
                    continue;
                }
                double partyPercentHealthMedian = 0;
                double lowestHp = 100;
                var lowestHpPlayer = new WoWUnit(0);
                foreach (WoWUnit currentPlayer in healingList)
                {
                    if (!currentPlayer.IsAlive || !currentPlayer.IsValid || !HealerClass.InRange(currentPlayer))
                        continue;
                    partyPercentHealthMedian += currentPlayer.HealthPercent;

                    if (currentPlayer.HealthPercent < 100 && currentPlayer.HealthPercent < lowestHp && HealerClass.InRange(currentPlayer))
                    {
                        lowestHp = currentPlayer.HealthPercent;
                        lowestHpPlayer = currentPlayer;
                    }
                }
                if (lowestHpPlayer.Guid > 0)
                {
                    if (lowestHpPlayer.Guid != ObjectManager.Me.Guid && ObjectManager.Me.HealthPercent < 100 && lowestHpPlayer.HealthPercent + 10 < ObjectManager.Me.HealthPercent)
                    {
                        lowestHpPlayer = ObjectManager.Me;
                        // If the lowest healthpercent available in my party is not me and I have only 10% more HP than him.
                        // Prioritize me instead. So selfish!
                    }
                }
                partyPercentHealthMedian = partyPercentHealthMedian/healingList.Count;
                // use partyPercentHealthMedian in the HealingFight() code may be useful.
                if (!lowestHpPlayer.IsValid || !lowestHpPlayer.IsAlive)
                    continue;
                if (lowestHpPlayer.HealthPercent >= 100 && partyPercentHealthMedian >= 100)
                    continue;
                HealingFight(lowestHpPlayer, partyPercentHealthMedian);
            }
            catch
            {
            }
        }
    }

    private void HealingFight(WoWUnit lowestHPUnit, double partyHealthPercentMedian = 100)
    {
        if (ObjectManager.Me.Target != lowestHPUnit.Guid)
            Interact.InteractWith(lowestHPUnit.GetBaseAddress);
        DefenseCycle();
        Buff();
        HealingBurst();
        HealCycle();
    }

    private void Buff()
    {
    }

    private void DefenseCycle()
    {
    }

    private void HealCycle()
    {
    }

    private void HealingBurst()
    {
    }

    #region Nested type: PriestDisciplineSettings

    [Serializable]
    public class PriestDisciplineSettings : Settings
    {
        public PriestDisciplineSettings()
        {
            ConfigWinForm("Discipline Priest Settings");
        }

        public static PriestDisciplineSettings CurrentSetting { get; set; }

        public static PriestDisciplineSettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\HealerClasses\\Settings\\Priest_Discipline.xml";
            if (File.Exists(currentSettingsFile))
            {
                return
                    CurrentSetting = Load<PriestDisciplineSettings>(currentSettingsFile);
            }
            return new PriestDisciplineSettings();
        }
    }

    #endregion
}

public class PriestHoly
{
    private readonly PriestHolySettings _mySettings = PriestHolySettings.GetSettings();

    public PriestHoly()
    {
        Main.InternalRange = 30f;

        while (Main.InternalLoop)
        {
            try
            {
                Thread.Sleep(100);
                if (ObjectManager.Me.IsDead || Usefuls.IsLoading || !Usefuls.InGame || ObjectManager.Me.IsMounted)
                    continue;

                List<WoWUnit> healingList = ObjectManager.GetFriendlyUnits();

                if (healingList.Count == 1)
                {
                    if (ObjectManager.Me.HealthPercent < 100)
                        HealingFight(ObjectManager.Me);
                    continue;
                }
                double partyPercentHealthMedian = 0;
                double lowestHp = 100;
                var lowestHpPlayer = new WoWUnit(0);
                foreach (WoWUnit currentPlayer in healingList)
                {
                    if (!currentPlayer.IsAlive || !currentPlayer.IsValid || !HealerClass.InRange(currentPlayer))
                        continue;
                    partyPercentHealthMedian += currentPlayer.HealthPercent;

                    if (currentPlayer.HealthPercent < 100 && currentPlayer.HealthPercent < lowestHp && HealerClass.InRange(currentPlayer))
                    {
                        lowestHp = currentPlayer.HealthPercent;
                        lowestHpPlayer = currentPlayer;
                    }
                }
                if (lowestHpPlayer.Guid > 0)
                {
                    if (lowestHpPlayer.Guid != ObjectManager.Me.Guid && ObjectManager.Me.HealthPercent < 100 && lowestHpPlayer.HealthPercent + 10 < ObjectManager.Me.HealthPercent)
                    {
                        lowestHpPlayer = ObjectManager.Me;
                        // If the lowest healthpercent available in my party is not me and I have only 10% more HP than him.
                        // Prioritize me instead. So selfish!
                    }
                }
                partyPercentHealthMedian = partyPercentHealthMedian/healingList.Count;
                // use partyPercentHealthMedian in the HealingFight() code may be useful.
                if (!lowestHpPlayer.IsValid || !lowestHpPlayer.IsAlive)
                    continue;
                if (lowestHpPlayer.HealthPercent >= 100 && partyPercentHealthMedian >= 100)
                    continue;
                HealingFight(lowestHpPlayer, partyPercentHealthMedian);
            }
            catch
            {
            }
        }
    }

    private void HealingFight(WoWUnit lowestHPUnit, double partyHealthPercentMedian = 100)
    {
        if (ObjectManager.Me.Target != lowestHPUnit.Guid)
            Interact.InteractWith(lowestHPUnit.GetBaseAddress);
        DefenseCycle();
        Buff();
        HealingBurst();
        HealCycle();
    }

    private void Buff()
    {
    }

    private void DefenseCycle()
    {
    }

    private void HealCycle()
    {
    }

    private void HealingBurst()
    {
    }

    #region Nested type: PriestHolySettings

    [Serializable]
    public class PriestHolySettings : Settings
    {
        public PriestHolySettings()
        {
            ConfigWinForm("Holy Priest Settings");
        }

        public static PriestHolySettings CurrentSetting { get; set; }

        public static PriestHolySettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\HealerClasses\\Settings\\Priest_Holy.xml";
            if (File.Exists(currentSettingsFile))
            {
                return
                    CurrentSetting = Load<PriestHolySettings>(currentSettingsFile);
            }
            return new PriestHolySettings();
        }
    }

    #endregion
}

#endregion

#region Monk

public class MonkMistweaver
{
    private readonly MonkMistweaverSettings _mySettings = MonkMistweaverSettings.GetSettings();

    public MonkMistweaver()
    {
        Main.InternalRange = 30f;

        while (Main.InternalLoop)
        {
            try
            {
                Thread.Sleep(100);
                if (ObjectManager.Me.IsDead || Usefuls.IsLoading || !Usefuls.InGame || ObjectManager.Me.IsMounted)
                    continue;

                List<WoWUnit> healingList = ObjectManager.GetFriendlyUnits();

                if (healingList.Count == 1)
                {
                    if (ObjectManager.Me.HealthPercent < 100)
                        HealingFight(ObjectManager.Me);
                    continue;
                }
                double partyPercentHealthMedian = 0;
                double lowestHp = 100;
                var lowestHpPlayer = new WoWUnit(0);
                foreach (WoWUnit currentPlayer in healingList)
                {
                    if (!currentPlayer.IsAlive || !currentPlayer.IsValid || !HealerClass.InRange(currentPlayer))
                        continue;
                    partyPercentHealthMedian += currentPlayer.HealthPercent;

                    if (currentPlayer.HealthPercent < 100 && currentPlayer.HealthPercent < lowestHp && HealerClass.InRange(currentPlayer))
                    {
                        lowestHp = currentPlayer.HealthPercent;
                        lowestHpPlayer = currentPlayer;
                    }
                }
                if (lowestHpPlayer.Guid > 0)
                {
                    if (lowestHpPlayer.Guid != ObjectManager.Me.Guid && ObjectManager.Me.HealthPercent < 100 && lowestHpPlayer.HealthPercent + 10 < ObjectManager.Me.HealthPercent)
                    {
                        lowestHpPlayer = ObjectManager.Me;
                        // If the lowest healthpercent available in my party is not me and I have only 10% more HP than him.
                        // Prioritize me instead. So selfish!
                    }
                }
                partyPercentHealthMedian = partyPercentHealthMedian/healingList.Count;
                // use partyPercentHealthMedian in the HealingFight() code may be useful.
                if (!lowestHpPlayer.IsValid || !lowestHpPlayer.IsAlive)
                    continue;
                if (lowestHpPlayer.HealthPercent >= 100 && partyPercentHealthMedian >= 100)
                    continue;
                HealingFight(lowestHpPlayer, partyPercentHealthMedian);
            }
            catch
            {
            }
        }
    }

    private void HealingFight(WoWUnit lowestHPUnit, double partyHealthPercentMedian = 100)
    {
        if (ObjectManager.Me.Target != lowestHPUnit.Guid)
            Interact.InteractWith(lowestHPUnit.GetBaseAddress);
        Buff();
        DefenseCycle();
        Decast();
        HealingBurst();
        HealCycle();
    }

    private void Buff()
    {
    }

    private void DefenseCycle()
    {
    }

    private void HealCycle()
    {
    }

    private void Decast()
    {
    }

    private void HealingBurst()
    {
    }

    #region Nested type: MonkMistweaverSettings

    [Serializable]
    public class MonkMistweaverSettings : Settings
    {
        public MonkMistweaverSettings()
        {
            ConfigWinForm("Mistweaver Monk Settings");
        }

        public static MonkMistweaverSettings CurrentSetting { get; set; }

        public static MonkMistweaverSettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\HealerClasses\\Settings\\Monk_Mistweaver.xml";
            if (File.Exists(currentSettingsFile))
            {
                return
                    CurrentSetting = Load<MonkMistweaverSettings>(currentSettingsFile);
            }
            return new MonkMistweaverSettings();
        }
    }

    #endregion
}

#endregion

// ReSharper restore ObjectCreationAsStatement