/*
* HealerClass for TheNoobBot
* Credit : Vesper, Ryuichiro
* Thanks you !
*/

using System;
using System.Collections.Generic;
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

public class Main : IHealerClass
{
    internal static float InternalRange = 30f;
    internal static bool InternalLoop = true;

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

        // Last field is intentionnally ommited because it's a backing field.
    }
}

#region Druid

public class DruidRestoration
{
    private static DruidRestorationSettings MySettings = DruidRestorationSettings.GetSettings();

    #region General Timers & Variables

    private readonly WoWItem _firstTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET);
    private readonly WoWItem _secondTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET, 2);

    private bool CombatMode = true;

    private Timer DefensiveTimer = new Timer(0);
    private Timer StunTimer = new Timer(0);

    private WoWPlayer Tank = new WoWPlayer(0);
    private WoWPlayer Target = new WoWPlayer(0);
    private int DamagedPlayers;
    private double PartyHpMedian;

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
                                    if (playerInMyParty <= 0) continue;
                                    WoWObject obj = ObjectManager.GetObjectByGuid(playerInMyParty);
                                    if (!obj.IsValid || obj.Type != WoWObjectType.Player) continue;
                                    var currentPlayer = new WoWPlayer(obj.GetBaseAddress);
                                    if (!currentPlayer.IsValid || !currentPlayer.IsAlive) continue;

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

            if (ObjectManager.Me.GetMove)
            {
                //Movement Buffs
                if (!Darkflight.HaveBuff && !Dash.HaveBuff) //they don't stack
                {
                    if (MySettings.UseDarkflight && Darkflight.IsSpellUsable)
                    {
                        Darkflight.Cast();
                        return;
                    }
                    if (MySettings.UseDash && Dash.IsSpellUsable && (MySettings.UseCatFormOOC || CatForm.HaveBuff))
                    {
                        Dash.Cast();
                        return;
                    }
                }
                if (MySettings.UseCatFormOOC && CatForm.IsSpellUsable && !CatForm.HaveBuff)
                {
                    CatForm.Cast();
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
        if (Healing() || Defensive() || Offensive())
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
            //Renewal 
            if (Renewal.IsSpellUsable && Target.HealthPercent < MySettings.UseRenewalBelowTargetPercentage)
            {
                Renewal.Cast(false, true, false, Target.GetUnitId());
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

            if (StunTimer.IsReady && (DefensiveTimer.IsReady || ObjectManager.Me.HealthPercent < 20))
            {
                //Stun
                if (ObjectManager.Target.IsStunnable)
                {
                    if (MightyBash.IsSpellUsable && ObjectManager.Me.HealthPercent < MySettings.UseMightyBashBelowPercentage && MightyBash.IsHostileDistanceGood)
                    {
                        MightyBash.Cast();
                        StunTimer = new Timer(1000*5);
                        return true;
                    }
                    if (WarStomp.IsSpellUsable && ObjectManager.Me.HealthPercent < MySettings.UseWarStompBelowPercentage && WarStomp.IsHostileDistanceGood)
                    {
                        WarStomp.Cast();
                        StunTimer = new Timer(1000*2);
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
            //Frenzied Regeneration (shapeshift in Bear Form)
            if (FrenziedRegeneration.IsSpellUsable && ObjectManager.Me.HealthPercent < MySettings.UseFrenziedRegenerationBelowPercentage)
            {
                FrenziedRegeneration.CastOnSelf();
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

            //Ironbark 
            if (Target.HealthPercent < MySettings.UseIronbarkBelowTargetPercentage && Ironbark.IsSpellUsable &&
                CombatClass.InSpellRange(Target, Ironbark.MinRangeFriend, Ironbark.MaxRangeFriend) &&
                Target.UnitAura(Ironbark.Id, ObjectManager.Me.Guid).AuraTimeLeftInMs < 4000)
            {
                Ironbark.Cast(false, true, false, Target.GetUnitId());
                return;
            }

            //Efflorescence 
            if (Target.HealthPercent < MySettings.UseEfflorescenceBelowTargetPercentage && Efflorescence.IsSpellUsable &&
                CombatClass.InSpellRange(Target, Efflorescence.MinRangeFriend, Efflorescence.MaxRangeFriend) &&
                Target.GetPlayerInSpellRange(10f) > MySettings.UseEfflorescenceAbovePlayerDensity && EfflorescenceTimer.IsReady)
            {
                Efflorescence.CastAtPosition(Target.Position);
                EfflorescenceTimer = new Timer(1000*30);
                return;
            }

            //Heal Buffs on Tank
            if (CenarionWard.IsSpellUsable && Tank.HealthPercent < MySettings.UseCenarionWardBelowTankPercentage &&
                CombatClass.InSpellRange(Tank, CenarionWard.MinRangeFriend, CenarionWard.MaxRangeFriend) &&
                Tank.UnitAura(CenarionWard.Id, ObjectManager.Me.Guid).AuraTimeLeftInMs < 5000)
            {
                CenarionWard.Cast(false, true, false, Tank.GetUnitId());
                return;
            }
            if (Tank.HealthPercent < MySettings.UseLifebloomBelowTankPercentage && Lifebloom.IsSpellUsable &&
                CombatClass.InSpellRange(Tank, Lifebloom.MinRangeFriend, Lifebloom.MaxRangeFriend) &&
                Tank.UnitAura(Lifebloom.Id, ObjectManager.Me.Guid).AuraTimeLeftInMs < 5000)
            {
                Lifebloom.Cast(false, true, false, Tank.GetUnitId());
                return;
            }
            if (Tank.HealthPercent < MySettings.UseRejuvenationBelowTankPercentage && Rejuvenation.IsSpellUsable &&
                CombatClass.InSpellRange(Tank, Rejuvenation.MinRangeFriend, Rejuvenation.MaxRangeFriend) &&
                Tank.UnitAura(Rejuvenation.Id, ObjectManager.Me.Guid).AuraTimeLeftInMs < 5000)
            {
                Rejuvenation.Cast(false, true, false, Tank.GetUnitId());
                return;
            }

            //Emergency Heal
            if (Target.HealthPercent < MySettings.UseSwiftmendBelowTargetPercentage && Swiftmend.IsSpellUsable &&
                CombatClass.InSpellRange(Target, Swiftmend.MinRangeFriend, Swiftmend.MaxRangeFriend))
            {
                Swiftmend.Cast(false, true, false, Target.GetUnitId());
                return;
            }

            //AOE Heal
            if (PartyHpMedian < MySettings.UseTranquilityBelowPartyPercentage && Tranquility.IsSpellUsable &&
                Target.UnitAura(Tranquility.Id, ObjectManager.Me.Guid).AuraTimeLeftInMs < 2666 &&
                ObjectManager.Me.GetPlayerInSpellRange(40f) > MySettings.UseTranquilityAbovePlayerDensity)
            {
                Tranquility.Cast(false, true, false, Target.GetUnitId());
                return;
            }

            //Buff upcoming Healings
            if ((PartyHpMedian < MySettings.UseIncarnationBelowPartyPercentage ||
                 Target.HealthPercent < MySettings.UseIncarnationBelowTargetPercentage) && Incarnation.IsSpellUsable)
            {
                Incarnation.Cast(false, true, false, Target.GetUnitId());
                return;
            }

            //AOE Heal
            if (PartyHpMedian < MySettings.UseWildGrowthBelowPartyPercentage && WildGrowth.IsSpellUsable &&
                CombatClass.InSpellRange(Target, WildGrowth.MinRangeFriend, WildGrowth.MaxRangeFriend) &&
                Target.UnitAura(WildGrowth.Id, ObjectManager.Me.Guid).AuraTimeLeftInMs < 2333 &&
                Target.GetPlayerInSpellRange(30f) > MySettings.UseWildGrowthAbovePlayerDensity)
            {
                WildGrowth.Cast(false, true, false, Target.GetUnitId());
                return;
            }

            //Buff current Hots
            if ((PartyHpMedian < MySettings.UseEssenceofGHanirBelowPartyPercentage ||
                 Target.HealthPercent < MySettings.UseEssenceofGHanirBelowTargetPercentage) && EssenceofGHanir.IsSpellUsable)
            {
                EssenceofGHanir.Cast(false, true, false, Target.GetUnitId());
                return;
            }
            if ((PartyHpMedian < MySettings.UseFlourishBelowPartyPercentage ||
                 Target.HealthPercent < MySettings.UseFlourishBelowTargetPercentage) && Flourish.IsSpellUsable)
            {
                Flourish.Cast(false, true, false, Target.GetUnitId());
                return;
            }

            //Heal Buffs on Target
            if (Target.HealthPercent < MySettings.UseRejuvenationBelowTargetPercentage && Rejuvenation.IsSpellUsable &&
                CombatClass.InSpellRange(Target, Rejuvenation.MinRangeFriend, Rejuvenation.MaxRangeFriend) &&
                Target.UnitAura(Rejuvenation.Id, ObjectManager.Me.Guid).AuraTimeLeftInMs < 5000)
            {
                Rejuvenation.Cast(false, true, false, Target.GetUnitId());
                return;
            }
            if (Target.HealthPercent < MySettings.UseRegrowthBelowTargetPercentage && Regrowth.IsSpellUsable &&
                CombatClass.InSpellRange(Target, Regrowth.MinRangeFriend, Regrowth.MaxRangeFriend) &&
                Target.UnitAura(Regrowth.Id, ObjectManager.Me.Guid).AuraTimeLeftInMs < 4000)
            {
                Regrowth.Cast(false, true, false, Target.GetUnitId());
                return;
            }

            //Filler Heal
            if (Target.HealthPercent < MySettings.UseHealingTouchBelowTargetPercentage && HealingTouch.IsSpellUsable &&
                CombatClass.InSpellRange(Target, HealingTouch.MinRangeFriend, HealingTouch.MaxRangeFriend))
            {
                HealingTouch.Cast(false, true, false, Target.GetUnitId());
                return;
            }

            //Filler Damage
            if (MySettings.UseSolarWrath && SolarWrath.IsSpellUsable && SolarWrath.IsHostileDistanceGood)
            {
                SolarWrath.Cast();
                return;
            }
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    #region Nested type: DruidRestorationSettings

    [Serializable]
    public class DruidRestorationSettings : Settings
    {
        /* Professions & Racials */
        //public bool UseArcaneTorrent = true;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseDarkflight = true;
        public int UseGiftoftheNaaruBelowPercentage = 50;
        public int UseStoneformBelowPercentage = 50;
        public int UseWarStompBelowPercentage = 50;

        /* Druid Buffs */
        public bool UseCatFormOOC = false;
        public int UseFlourishBelowTargetPercentage = 30;
        public int UseFlourishBelowPartyPercentage = 30;
        public int UseIncarnationBelowTargetPercentage = 40;
        public int UseIncarnationBelowPartyPercentage = 40;

        /* Artifact Spells */
        public int UseEssenceofGHanirBelowTargetPercentage = 50;
        public int UseEssenceofGHanirBelowPartyPercentage = 50;

        /* Offensive Spells */
        public bool UseSolarWrath = true;

        /* Defensive Spells */
        public int UseIronbarkBelowTargetPercentage = 60;
        //public bool UseEntanglingRoots = true;
        //public bool UseMassEntanglement = true;
        public int UseMightyBashBelowPercentage = 25;
        //public bool UseTyphoon = true;
        //public bool UseWildCharge = true;

        /* Healing Spells */
        public int UseCenarionWardBelowTankPercentage = 90;
        public int UseEfflorescenceBelowTargetPercentage = 90;
        public int UseEfflorescenceAbovePlayerDensity = 2;
        public int UseFrenziedRegenerationBelowPercentage = 20;
        public int UseHealingTouchBelowTargetPercentage = 80;
        public int UseLifebloomBelowTankPercentage = 90;
        public int UseRegrowthBelowTargetPercentage = 60;
        public int UseRejuvenationBelowTargetPercentage = 80;
        public int UseRejuvenationBelowTankPercentage = 90;
        public int UseRenewalBelowTargetPercentage = 25;
        public int UseSwiftmendBelowTargetPercentage = 30;
        public int UseTranquilityBelowPartyPercentage = 30;
        public int UseTranquilityAbovePlayerDensity = 4;
        public int UseWildGrowthBelowPartyPercentage = 60;
        public int UseWildGrowthAbovePlayerDensity = 2;

        /* Utility Spells */
        public bool UseDash = true;
        //public bool UseDisplacerBeast = true;
        //public bool UseProwl = true;

        /* Game Settings */
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;
        public int PrioritizeTankBelowPercentage = 40;
        public int PrioritizeMeBelowPercentage = 40;

        public DruidRestorationSettings()
        {
            ConfigWinForm("Druid Restoration Settings");
            /* Professions & Racials */
            //AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Darkflight", "UseDarkflight", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaruBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            AddControlInWinForm("Use Stone Form", "UseStoneformBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            AddControlInWinForm("Use War Stomp", "UseWarStompBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            /* Druid Buffs */
            AddControlInWinForm("Use Cat Form out of Combat", "UseCatFormOOC", "Druid Buffs");
            AddControlInWinForm("Use Flourish", "UseFlourishBelowTargetPercentage", "Druid Buffs", "BelowPercentage", "Target Life");
            AddControlInWinForm("Use Flourish", "UseFlourishBelowPartyPercentage", "Druid Buffs", "BelowPercentage", "Party Life");
            AddControlInWinForm("Use Incarnation", "UseIncarnationBelowTargetPercentage", "Druid Buffs", "BelowPercentage", "Target Life");
            AddControlInWinForm("Use Incarnation", "UseIncarnationBelowPartyPercentage", "Druid Buffs", "BelowPercentage", "Party Life");
            /* Artifact Spells */
            AddControlInWinForm("Use Essence of G'Hanir", "UseEssenceofGHanirBelowTargetPercentage", "Artifact Spells", "BelowPercentage", "Target Life");
            AddControlInWinForm("Use Essence of G'Hanir", "UseEssenceofGHanirBelowPartyPercentage", "Artifact Spells", "BelowPercentage", "Party Life");
            /* Offensive Spells */
            AddControlInWinForm("Use Solar Wrath", "UseSolarWrath", "Offensive Spells");
            /* Defensive Spells */
            AddControlInWinForm("Use Ironbark", "UseIronbarkBelowTargetPercentage", "Defensive Spells", "BelowPercentage", "Target Life");
            AddControlInWinForm("Use Mighty Bash", "UseMightyBashBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            /* Healing Spells */
            AddControlInWinForm("Use Cenarion Ward", "UseCenarionWardBelowTankPercentage", "Healing Spells", "BelowPercentage", "Tank Life");
            AddControlInWinForm("Use Frenzied Regeneration", "UseFrenziedRegenerationBelowPercentage", "Healing Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Healing Touch", "UseHealingTouchBelowTargetPercentage", "Healing Spells", "BelowPercentage", "Target Life");
            AddControlInWinForm("Use Lifebloom", "UseLifebloomBelowTankPercentage", "Healing Spells", "BelowPercentage", "Tank Life");
            AddControlInWinForm("Use Regrowth", "UseRegrowthBelowTargetPercentage", "Healing Spells", "BelowPercentage", "Target Life");
            AddControlInWinForm("Use Rejuvenation", "UseRejuvenationBelowTargetPercentage", "Healing Spells", "BelowPercentage", "Target Life");
            AddControlInWinForm("Use Rejuvenation", "UseRejuvenationBelowTankPercentage", "Healing Spells", "BelowPercentage", "Tank Life");
            AddControlInWinForm("Use Renewal", "UseRenewalBelowTargetPercentage", "Healing Spells", "BelowPercentage", "Target Life");
            AddControlInWinForm("Use Swiftmend", "UseSwiftmendBelowTargetPercentage", "Healing Spells", "BelowPercentage", "Target Life");
            AddControlInWinForm("Use Tranquility", "UseTranquilityBelowPartyPercentage", "Healing Spells", "BelowPercentage", "Party Life");
            AddControlInWinForm("Use Tranquility", "UseTranquilityAbovePlayerDensity", "Healing Spells", "AbovePercentage", "Players");
            AddControlInWinForm("Use Wild Growth", "UseWildGrowthBelowPartyPercentage", "Healing Spells", "BelowPercentage", "Party Life");
            AddControlInWinForm("Use Wild Growth", "UseWildGrowthAbovePlayerDensity", "Healing Spells", "AbovePercentage", "Players");
            /* Utility Spells */
            AddControlInWinForm("Use Dash", "UseDash", "Utility Spells");
            /* Game Settings */
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");
            AddControlInWinForm("Prioritize Tank", "PrioritizeTankBelowPercentage", "Game Settings", "BelowPercentage");
            AddControlInWinForm("Prioritize Me", "PrioritizeMeBelowPercentage", "Game Settings", "BelowPercentage");
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

    #endregion
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
    private Timer StunTimer = new Timer(0);

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

    private readonly Spell AuraofMercy = new Spell("Aura of Mercy"); //No GCD
    private readonly Spell AuraofSacrifice = new Spell("Aura of Sacrifice"); //No GCD
    private readonly Spell DevotionAura = new Spell("Devotion Aura"); //No GCD

    #endregion

    #region Buffs

    private readonly Spell Forbearance = new Spell(25771);

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
    private readonly Spell DivineProtection = new Spell("Divine Protection");
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

            if (ObjectManager.Me.GetMove)
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
        if (Healing() || Defensive() || Offensive())
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

            if (StunTimer.IsReady && (DefensiveTimer.IsReady || ObjectManager.Me.HealthPercent < 20))
            {
                //Stun
                if (ObjectManager.Target.IsStunnable)
                {
                    if (WarStomp.IsSpellUsable && ObjectManager.Me.HealthPercent < MySettings.UseWarStompBelowPercentage && WarStomp.IsHostileDistanceGood)
                    {
                        WarStomp.Cast();
                        StunTimer = new Timer(1000*2);
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
                    DivineProtection.IsSpellUsable && !Target.HaveBuff(Forbearance.Id))
                {
                    DivineProtection.Cast();
                    DefensiveTimer = new Timer(1000*8);
                    return true;
                }
            }
            //Mitigate Damage in Emergency Situations
            if (ObjectManager.Me.HealthPercent < MySettings.UseDivineShieldBelowPercentage &&
                DivineShield.IsSpellUsable && !Target.HaveBuff(Forbearance.Id))
            {
                DivineShield.Cast();
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

            //Avenging Wrath Buff
            if (Target.HealthPercent < MySettings.UseAvengingWrathBelowTargetPercentage && AvengingWrath.IsSpellUsable)
            {
                AvengingWrath.Cast();
            }
            //Aura Mastery Buff
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

            //Beacon of Light on Tank
            if (MySettings.UseBeaconofLight && BeaconofLight.IsSpellUsable &&
                CombatClass.InSpellRange(Tank, BeaconofLight.MinRangeFriend, BeaconofLight.MaxRangeFriend) &&
                !Tank.HaveBuff(BeaconofLight.Ids))
            {
                BeaconofLight.Cast(false, true, false, Tank.GetUnitId());
            }
            //Beacon of Faith on Target
            if (MySettings.UseBeaconofFaith && BeaconofFaith.IsSpellUsable &&
                CombatClass.InSpellRange(Target, BeaconofFaith.MinRangeFriend, BeaconofFaith.MaxRangeFriend) &&
                !Target.HaveBuff(BeaconofLight.Ids) && !Target.HaveBuff(BeaconofFaith.Ids))
            {
                BeaconofFaith.Cast(false, true, false, Target.GetUnitId());
            }
            //Beacon of Faith on Target
            if (MySettings.UseBeaconofVirtue && BeaconofVirtue.IsSpellUsable &&
                CombatClass.InSpellRange(Target, BeaconofVirtue.MinRangeFriend, BeaconofVirtue.MaxRangeFriend) &&
                Target.GetPlayerInSpellRange(30f) > MySettings.UseBeaconofVirtueAbovePlayerDensity &&
                !Target.HaveBuff(BeaconofLight.Ids))
            {
                BeaconofVirtue.Cast(false, true, false, Target.GetUnitId());
            }

            //Lay on Hands on Target
            if (Target.HealthPercent < MySettings.UseLayonHandsBelowTargetPercentage && LayonHands.IsSpellUsable &&
                CombatClass.InSpellRange(Target, LayonHands.MinRangeFriend, LayonHands.MaxRangeFriend) && !Target.HaveBuff(Forbearance.Id))
            {
                LayonHands.Cast(false, true, false, Target.GetUnitId());
                return;
            }

            //Blessing of Sacrifice on Target
            if (Tank.HealthPercent < MySettings.UseBlessingofSacrificeBelowTankPercentage &&
                ObjectManager.Me.HealthPercent > MySettings.UseBlessingofSacrificeAbovePercentage && BlessingofSacrifice.IsSpellUsable &&
                CombatClass.InSpellRange(Tank, BlessingofSacrifice.MinRangeFriend, BlessingofSacrifice.MaxRangeFriend))
            {
                BlessingofSacrifice.Cast(false, true, false, Target.GetUnitId());
                return;
            }

            //Holy Shock on Target
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

            //Light of Dawn on Target
            if (Target.HealthPercent < MySettings.UseLightofDawnBelowTargetPercentage && LightofDawn.IsSpellUsable &&
                ((CombatClass.InSpellRange(Target, 0, 10f) && Target.GetPlayerInSpellRange(5f) > MySettings.UseLightofDawnAbovePlayerDensity) ||
                 ObjectManager.Me.GetPlayerInSpellRange(5f) > MySettings.UseLightofDawnAbovePlayerDensity) &&
                DamagedPlayers > MySettings.UseLightofDawnAboveDamagedPlayer)
            {
                LightofDawn.Cast(false, true, false, Target.GetUnitId());
                return;
            }

            //Light's Hammer on Target
            if (PartyHpMedian < MySettings.UseLightsHammerBelowPartyPercentage && LightsHammer.IsSpellUsable &&
                CombatClass.InSpellRange(Target, LightsHammer.MinRangeFriend, LightsHammer.MaxRangeFriend) &&
                Target.GetPlayerInSpellRange(10f) > MySettings.UseLightsHammerAbovePlayerDensity &&
                DamagedPlayers > MySettings.UseLightsHammerAboveDamagedPlayer)
            {
                LightsHammer.CastAtPosition(Target.Position);
                return;
            }

            //Holy Prism on Target
            if (PartyHpMedian < MySettings.UseHolyPrismBelowPartyPercentage && HolyPrism.IsSpellUsable &&
                CombatClass.InSpellRange(Target, HolyPrism.MinRangeFriend, HolyPrism.MaxRangeFriend) &&
                Target.GetPlayerInSpellRange(15f) > MySettings.UseHolyPrismAbovePlayerDensity &&
                DamagedPlayers > MySettings.UseHolyPrismAboveDamagedPlayer)
            {
                HolyPrism.Cast(false, true, false, Target.GetUnitId());
                return;
            }

            //Bestow Faith on Tank
            if (Tank.HealthPercent < MySettings.UseBestowFaithBelowTankPercentage && BestowFaith.IsSpellUsable &&
                CombatClass.InSpellRange(Tank, BestowFaith.MinRangeFriend, BestowFaith.MaxRangeFriend))
            {
                BestowFaith.Cast(false, true, false, Tank.GetUnitId());
                return;
            }

            //Tyr's Deliverance on Target
            if (PartyHpMedian < MySettings.UseTyrsDeliveranceBelowPartyPercentage && TyrsDeliverance.IsSpellUsable &&
                CombatClass.InSpellRange(Target, TyrsDeliverance.MinRangeFriend, TyrsDeliverance.MaxRangeFriend) &&
                Target.GetPlayerInSpellRange(15f) > MySettings.UseTyrsDeliveranceAbovePlayerDensity &&
                DamagedPlayers > MySettings.UseTyrsDeliveranceAboveDamagedPlayer)
            {
                TyrsDeliverance.Cast(false, true, false, Target.GetUnitId());
                return;
            }

            //Light of the Martyr on Target
            if (Target.HealthPercent < MySettings.UseLightoftheMartyrBelowTargetPercentage &&
                ObjectManager.Me.HealthPercent > MySettings.UseLightoftheMartyrAbovePercentage && LightoftheMartyr.IsSpellUsable &&
                CombatClass.InSpellRange(Target, LightoftheMartyr.MinRangeFriend, LightoftheMartyr.MaxRangeFriend))
            {
                LightoftheMartyr.Cast(false, true, false, Target.GetUnitId());
                return;
            }

            //Holy Light on Target when Avenging Wrath is up
            if (Target.HealthPercent < MySettings.UseHolyLightBelowTargetPercentage && HolyLight.IsSpellUsable &&
                CombatClass.InSpellRange(Target, HolyLight.MinRangeFriend, HolyLight.MaxRangeFriend) && AvengingWrath.HaveBuff)
            {
                HolyLight.Cast(false, true, false, Target.GetUnitId());
                return;
            }

            //Flash of Light on Target
            if (Target.HealthPercent < MySettings.UseFlashofLightBelowTargetPercentage && FlashofLight.IsSpellUsable &&
                CombatClass.InSpellRange(Target, FlashofLight.MinRangeFriend, FlashofLight.MaxRangeFriend))
            {
                FlashofLight.Cast(false, true, false, Target.GetUnitId());
                return;
            }

            //Holy Light on Target
            if (Target.HealthPercent < MySettings.UseHolyLightBelowTargetPercentage && HolyLight.IsSpellUsable &&
                CombatClass.InSpellRange(Target, HolyLight.MinRangeFriend, HolyLight.MaxRangeFriend))
            {
                HolyLight.Cast(false, true, false, Target.GetUnitId());
                return;
            }

            //Rule of Law Buff
            if (MySettings.UseRuleofLaw && RuleofLaw.IsSpellUsable)
            {
                RuleofLaw.Cast();
            }

            //Filler Damage
            if (MySettings.UseJudgment && Judgment.IsSpellUsable && Judgment.IsHostileDistanceGood &&
                !Judgment.TargetHaveBuff)
            {
                Judgment.Cast();
                return;
            }
            if (MySettings.UseHolyShock && HolyShock.IsSpellUsable && HolyShock.IsHostileDistanceGood)
            {
                HolyShock.Cast();
                return;
            }
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
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
        public int UseTyrsDeliveranceAbovePlayerDensity = 3;
        public int UseTyrsDeliveranceAboveDamagedPlayer = 3;

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
        public bool UseBeaconofVirtue = true;
        public int UseBeaconofVirtueAbovePlayerDensity = 2;
        /* Healing Spells */
        public int UseBestowFaithBelowTankPercentage = 40;
        public int UseFlashofLightBelowTargetPercentage = 60;
        public int UseHolyLightBelowTargetPercentage = 90;
        public int UseHolyPrismAbovePlayerDensity = 2;
        public int UseHolyPrismAboveDamagedPlayer = 2;
        public int UseHolyPrismBelowPartyPercentage = 60;
        public int UseHolyShockBelowTargetPercentage = 80;
        public int UseLightofDawnAbovePlayerDensity = 2;
        public int UseLightofDawnAboveDamagedPlayer = 2;
        public int UseLightofDawnBelowTargetPercentage = 60;
        public int UseLightoftheMartyrAbovePercentage = 80;
        public int UseLightoftheMartyrBelowTargetPercentage = 30;
        /* Healing Cooldowns */
        public int UseAvengingWrathBelowTargetPercentage = 40;
        public int UseAuraMasteryBelowPartyPercentage = 40;
        public bool UseHolyAvenger = true;
        public int UseLayonHandsBelowTargetPercentage = 10;
        public int UseLightsHammerAbovePlayerDensity = 2;
        public int UseLightsHammerAboveDamagedPlayer = 4;
        public int UseLightsHammerBelowPartyPercentage = 60;

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
            AddControlInWinForm("Use Essence of G'Hanir", "UseTyrsDeliveranceAbovePlayerDensity", "Artifact Spells", "AbovePercentage", "Players");
            AddControlInWinForm("Use Essence of G'Hanir", "UseTyrsDeliveranceAboveDamagedPlayer", "Artifact Spells", "AbovePercentage", "Damaged Players");
            AddControlInWinForm("Use Essence of G'Hanir", "UseTyrsDeliveranceBelowPartyPercentage", "Artifact Spells", "BelowPercentage", "Party Life");
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
            AddControlInWinForm("Use Beacon of Virtue", "UseBeaconofVirtue", "Healing Buffs");
            AddControlInWinForm("Use Beacon of Virtue", "UseBeaconofVirtueAbovePlayerDensity", "Healing Buffs", "AbovePercentage", "Players");
            /* Healing Spells */
            AddControlInWinForm("Use Bestow Faith", "UseBestowFaithBelowTankPercentage", "Healing Spells", "BelowPercentage", "Tank Life");
            AddControlInWinForm("Use Flash of Light", "UseFlashofLightBelowTargetPercentage", "Healing Spells", "BelowPercentage", "Target Life");
            AddControlInWinForm("Use Holy Light", "UseHolyLightBelowTargetPercentage", "Healing Spells", "BelowPercentage", "Target Life");
            AddControlInWinForm("Use Holy Prism", "UseHolyPrismAbovePlayerDensity", "Healing Spells", "AbovePercentage", "Players");
            AddControlInWinForm("Use Holy Prism", "UseHolyPrismAboveDamagedPlayer", "Healing Spells", "AbovePercentage", "Damaged Players");
            AddControlInWinForm("Use Holy Prism", "UseHolyPrismBelowPartyPercentage", "Healing Spells", "BelowPercentage", "Party Life");
            AddControlInWinForm("Use Holy Shock", "UseHolyShockBelowTargetPercentage", "Healing Spells", "BelowPercentage", "Target Life");
            AddControlInWinForm("Use Light of Dawn", "UseLightofDawnAbovePlayerDensity", "Healing Spells", "AbovePercentage", "Players");
            AddControlInWinForm("Use Light of Dawn", "UseLightofDawnAboveDamagedPlayer", "Healing Spells", "AbovePercentage", "Damaged Players");
            AddControlInWinForm("Use Light of Dawn", "UseLightofDawnBelowPartyPercentage", "Healing Spells", "BelowPercentage", "Party Life");
            AddControlInWinForm("Use Light of the Martyr", "UseLightoftheMartyrAbovePercentage", "Healing Spells", "AbovePercentage", "Life");
            AddControlInWinForm("Use Light of the Martyr", "UseLightoftheMartyrBelowTargetPercentage", "Healing Spells", "BelowPercentage", "Target Life");
            /* Healing Cooldowns */
            AddControlInWinForm("Use Avenging Wrath", "UseAvengingWrathBelowTargetPercentage", "Healing Cooldowns", "BelowPercentage", "Target Life");
            AddControlInWinForm("Use Aura Mastery", "UseAuraMasteryBelowPartyPercentage", "Healing Cooldowns", "BelowPercentage", "Party Life");
            AddControlInWinForm("Use Holy Avenger", "UseHolyAvenger", "Healing Cooldowns");
            AddControlInWinForm("Use Lay on Hands", "UseLayonHandsBelowTargetPercentage", "Healing Cooldowns", "BelowPercentage", "Target Life");
            AddControlInWinForm("Use Light's Hammer", "UseLightsHammerAbovePlayerDensity", "Healing Cooldowns", "AbovePercentage", "Players");
            AddControlInWinForm("Use Light's Hammer", "UseLightsHammerAboveDamagedPlayer", "Healing Cooldowns", "AbovePercentage", "Damaged Players");
            AddControlInWinForm("Use Light's Hammer", "UseLightsHammerBelowPartyPercentage", "Healing Cooldowns", "BelowPercentage", "Party Life");
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
    private Timer StunTimer = new Timer(0);

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

            if (ObjectManager.Me.GetMove)
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
        if (Healing() || Defensive() || Offensive())
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

            if (StunTimer.IsReady && (DefensiveTimer.IsReady || ObjectManager.Me.HealthPercent < 20))
            {
                //Stun
                if (ObjectManager.Target.IsStunnable)
                {
                    if (ObjectManager.Me.HealthPercent < MySettings.UseWarStompBelowPercentage && WarStomp.IsSpellUsable)
                    {
                        WarStomp.Cast();
                        StunTimer = new Timer(1000*2.5);
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