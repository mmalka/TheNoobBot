/*
* HealerClass for TheNoobBot
* Credit : Vesper, Ryuichiro
* Thanks you !
*/

using System.Collections.Generic;
using System.Reflection;
using nManager.Wow;
using nManager.Wow.Bot.Tasks;
using nManager.Wow.Class;
using Timer = nManager.Helpful.Timer;
// ReSharper disable EmptyGeneralCatchClause
// ReSharper disable ObjectCreationAsStatement
using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using nManager.Helpful;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;

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
    private readonly DruidRestorationSettings _mySettings = DruidRestorationSettings.GetSettings();

    public DruidRestoration()
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

    public void HealingBurst()
    {
    }

    #region Nested type: DruidRestorationSettings

    [Serializable]
    public class DruidRestorationSettings : Settings
    {
        public DruidRestorationSettings()
        {
            ConfigWinForm("Druid Restoration Settings");
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
    private readonly PaladinHolySettings _mySettings = PaladinHolySettings.GetSettings();

    public PaladinHoly()
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
        // Logging.Write("Most interesting lowestHpPlayer to heal is " + lowestHPUnit.Name + " (GUID: " + lowestHPUnit.Guid + "), %HP: " + lowestHPUnit.HealthPercent);
        // Logging.Write("Party has a HealthPercent median of " + partyHealthPercentMedian);
        if (ObjectManager.Target.HealthPercent < 40)
            HealBurst();
        HealCycle();
        Buffs();
    }

    private void Buffs()
    {
        if (!ObjectManager.Me.IsMounted)
            Blessing();
        Seal();
    }

    private void Seal()
    {
    }

    private void Blessing()
    {
    }

    private void HealBurst()
    {
    }

    private void HealCycle()
    {
    }

    #region Nested type: PaladinHolySettings

    [Serializable]
    public class PaladinHolySettings : Settings
    {
        public PaladinHolySettings()
        {
            ConfigWinForm("Paladin Protection Settings");
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

    private bool CombatMode = true;

    private WoWPlayer Tank = new WoWPlayer(0);
    private WoWPlayer Target = new WoWPlayer(0);
    private int DamagedPlayers;
    private double PartyHpMedian;

    #endregion

    #region Talents

    //public readonly Spell Deluge = new Spell("Deluge");
    //public readonly Spell HighTide = new Spell("High Tide");

    #endregion

    #region Shaman Buffs

    public readonly Spell Riptide = new Spell("Riptide");
    public readonly Spell GhostWolf = new Spell("Ghost Wolf");

    #endregion

    #region Offensive Cooldowns

    public readonly Spell Bloodlust = new Spell("Bloodlust"); //No GCD
    public readonly Spell Heroism = new Spell("Heroism"); //No GCD

    #endregion

    #region Defensive Cooldowns

    //public readonly Spell AncestralGuidance = new Spell("Ancestral Guidance");
    //public readonly Spell CloudburstTotem  = new Spell("Cloudburst Totem");
    public readonly Spell EarthenShieldTotem = new Spell("Earthen Shield Totem");
    private Timer EarthenShieldTotemCooldown = new Timer(0);
    public readonly Spell HealingStreamTotem = new Spell("Healing Stream Totem");
    private Timer HealingStreamTotemCooldown = new Timer(0);
    public readonly Spell HealingTideTotem = new Spell("Healing Tide Totem");
    private Timer HealingTideTotemCooldown = new Timer(0);
    public readonly Spell SpiritLinkTotem = new Spell("Spirit Link Totem");
    private Timer SpiritLinkTotemCooldown = new Timer(0);
    //public readonly Spell SpiritwalkersGrace = new Spell("Spiritwalker's Grace");//No GCD

    #endregion

    #region Healing Cooldown

    public readonly Spell ChainHeal = new Spell("Chain Heal");
    public readonly Spell HealingRain = new Spell("Healing Rain");
    public readonly Spell HealingSurge = new Spell("Healing Surge");
    public readonly Spell HealingWave = new Spell("Healing Wave");

    #endregion

    #region Utility Cooldowns

    public readonly Spell WindRushTotem = new Spell("Wind Rush Totem");

    #endregion

    public ShamanRestoration()
    {
        Main.InternalRange = 30f;
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

                                //Setup Target //TODO implement Casts that doesn't need to be Targeted first.
                                //if (ObjectManager.Me.Target != Target.Guid && HealerClass.InRange(Target) && Target.IsAlive)
                                //{
                                //    Logging.WriteFight("New Target: " + Target.Name);
                                //    Interact.InteractWith(Target.GetBaseAddress);
                                //}
                                //else if (ObjectManager.Me.Target != ObjectManager.Me.Guid)
                                //{
                                //    //Logging.WriteFight("New Target: " + ObjectManager.Me.Name);
                                //    //Interact.InteractWith(ObjectManager.Me.GetBaseAddress);
                                //}
                            }
                        }
                        if (Fight.InFight || PartyHpMedian < 90)
                        {
                            Combat();
                        }
                        else
                        {
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
        }
    }

    private void Test()
    {
        Logging.WriteDebug("HealingStreamTotem.IsSpellUsable == " + HealingStreamTotem.IsSpellUsable);
        Usefuls.SleepGlobalCooldown();
        try
        {
            Memory.WowMemory.GameFrameLock();

            if (PartyHpMedian <= MySettings.UseHealingStreamTotemAtPartyPercentage && HealingStreamTotem.IsSpellUsable &&
                MySettings.UseHealingStreamTotem && HealingStreamTotem.KnownSpell)
            {
                HealingStreamTotem.Cast();
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
                //Wind Rush Totem
                if (WindRushTotem.IsSpellUsable && MySettings.UseWindRushTotem)
                {
                    SpellManager.CastSpellByIDAndPosition(WindRushTotem.Id, ObjectManager.Me.Position);
                    return;
                }

                //Ghost Wolf
                if (!GhostWolf.HaveBuff && GhostWolf.IsSpellUsable && MySettings.UseGhostWolf)
                {
                    GhostWolf.Cast();
                }
            }
        }
    }

    private void Combat()
    {
        //Log
        if (!CombatMode)
        {
            Logging.WriteFight("Combat:");
            CombatMode = true;
        }

        Burst(); //GCD independent
        GCDCycle(); //GCD dependent
    }

    private void Burst()
    {
        //Offensive Cooldowns
        if (Bloodlust.IsSpellUsable && !ObjectManager.Me.HaveBuff(57724) && MySettings.UseBloodlustHeroism)
        {
            Bloodlust.Cast();
        }
        if (Heroism.IsSpellUsable && !ObjectManager.Me.HaveBuff(57723) && MySettings.UseBloodlustHeroism)
        {
            Heroism.Cast();
        }
    }

    private void GCDCycle()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            //Logging.WriteFight("Tank.GetUnitId() == " + Tank.GetUnitId());
            //Logging.WriteFight("Target.GetUnitId() == " + Target.GetUnitId());
            //Logging.WriteFight("------------------");
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

            //Heal Cooldowns
            if (Target.HealthPercent <= MySettings.UseSpiritLinkTotemBelowPercentage && SpiritLinkTotemCooldown.IsReady &&
                CombatClass.InSpellRange(Target, SpiritLinkTotem.MinRangeFriend, SpiritLinkTotem.MaxRangeFriend) &&
                MySettings.UseSpiritLinkTotem && SpiritLinkTotem.KnownSpell)
            {
                SpellManager.CastSpellByIDAndPosition(SpiritLinkTotem.Id, Target.Position);
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
                SpellManager.CastSpellByIDAndPosition(EarthenShieldTotem.Id, Target.Position);
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
        /* Shaman Buffs */
        public bool UseGhostWolf = true;
        /* Healing Buffs */
        public bool UseRiptide = true;
        public int UseRiptideBelowPercentage = 95;
        public int UseRiptideAtTankPercentage = 95;
        /* Healing Cooldowns */
        public bool UseEarthenShieldTotem = true;
        public int UseEarthenShieldTotemBelowPercentage = 95;
        public bool UseHealingStreamTotem = true;
        public int UseHealingStreamTotemAtPartyPercentage = 95;
        public bool UseHealingTideTotem = true;
        public int UseHealingTideTotemAtPartyPercentage = 40;
        public bool UseSpiritLinkTotem = true;
        public int UseSpiritLinkTotemBelowPercentage = 20;
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
        /* Utility Cooldowns */
        public bool UseWindRushTotem = true;
        /* Offensive Cooldowns */
        public bool UseBloodlustHeroism = false;
        /* Game Settings */
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;
        public int BurstDistance = 30;
        public int PrioritizeTankBelowPercentage = 40;
        public int PrioritizeMeBelowPercentage = 40;

        public ShamanRestorationSettings()
        {
            ConfigWinForm("Shaman Restoration Settings");
            /* Shaman Buffs */
            AddControlInWinForm("Use Ghost Wolf", "UseGhostWolf", "Shaman Buffs");
            /* Healing Buffs */
            AddControlInWinForm("Use Riptide", "UseRiptide", "Healing Buffs");
            AddControlInWinForm("Use Riptide", "UseRiptideBelowPercentage", "Healing Buffs", "BelowPercentage", "Life");
            AddControlInWinForm("Use Riptide On Tank\n(highest possible priority)", "UseRiptideAtTankPercentage", "Healing Buffs", "BelowPercentage", "Life");
            /* Healing Cooldowns */
            AddControlInWinForm("Use Earthen Shield Totem", "UseEarthenShieldTotem", "Healing Cooldowns");
            AddControlInWinForm("Use Earthen Shield Totem If Party Average", "UseEarthenShieldTotemBelowPercentage", "Healing Cooldowns", "BelowPercentage", "Life");
            AddControlInWinForm("Use Healing Stream Totem", "UseHealingStreamTotem", "Healing Cooldowns");
            AddControlInWinForm("Use Healing Stream Totem If Party Average", "UseHealingStreamTotemAtPartyPercentage", "Healing Cooldowns", "BelowPercentage", "Life");
            AddControlInWinForm("Use Healing Tide Totem", "UseHealingTideTotem", "Healing Cooldowns");
            AddControlInWinForm("Use Healing Tide Totem If Party Average", "UseHealingTideTotemAtPartyPercentage", "Healing Cooldowns", "BelowPercentage", "Life");
            AddControlInWinForm("Use Spirit Link Totem", "UseSpiritLinkTotem", "Healing Cooldowns");
            AddControlInWinForm("Use Spirit Link Totem", "UseSpiritLinkTotemBelowPercentage", "Healing Cooldowns", "BelowPercentage", "Life");
            /* Healing Spells */
            AddControlInWinForm("Use Healing Rain", "UseHealingRain", "Healing Spells");
            AddControlInWinForm("Use Healing Rain If Damaged Players stay together", "UseHealingRainAtPlayerDensity", "Healing Spells"); //TODO add AbovePercentage alternative
            AddControlInWinForm("Use Healing Rain If Party Average", "UseHealingRainAtPartyPercentage", "Healing Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Chain Heal", "UseChainHeal", "Healing Spells");
            AddControlInWinForm("Use Chain Heal If Damaged Players Count Reaches", "UseChainHealAtDamagedPlayers", "Healing Spells"); //TODO add AbovePercentage alternative
            AddControlInWinForm("Use Chain Heal If Party Average", "UseChainHealAtPartyPercentage", "Healing Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Healing Surge", "UseHealingSurge", "Healing Spells");
            AddControlInWinForm("Use Healing Surge Only While Tidal Waves Buff Is Active", "UseHealingSurgeWithTidalWavesOnly", "Healing Spells");
            AddControlInWinForm("Use Healing Surge", "UseHealingSurgeBelowPercentage", "Healing Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Healing Wave", "UseHealingWave", "Healing Spells");
            AddControlInWinForm("Use Healing Wave", "UseHealingWaveBelowPercentage", "Healing Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Healing Wave\nWhile Tidal Waves Buff Is Active", "UseHealingWaveWithTidalWavesBelowPercentage", "Healing Spells", "BelowPercentage", "Life");
            /* Utility Cooldowns */
            AddControlInWinForm("Use Wind Rush Totem", "UseWindRushTotem", "Utility Cooldowns");
            /* Offensive Cooldowns */
            AddControlInWinForm("Use Bloodlust / Heroism", "UseBloodlustHeroism", "Offensive Cooldowns");
            /* Game Settings */
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");
            AddControlInWinForm("Burst If Target is below Range", "BurstDistance", "Game Settings"); //TODO add BelowPercentage alternative
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