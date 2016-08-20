/*
* HealerClass for TheNoobBot
* Credit : Vesper
* Thanks you !
*/

using System.Collections.Generic;
using System.Reflection;
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
        // Logging.Write("Most interesting target to heal is " + lowestHPUnit.Name + " (GUID: " + lowestHPUnit.Guid + "), %HP: " + lowestHPUnit.HealthPercent);
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

    private WoWPlayer Tank = new WoWPlayer(0);
    private WoWPlayer LowestHpPlayer = new WoWPlayer(0);
    private int DamagedPlayers;
    private double PartyHpMedian;

    #endregion

    #region Shaman Buffs

    public readonly Spell Riptide = new Spell("Riptide");

    #endregion

    #region Offensive Cooldowns

    public readonly Spell Bloodlust = new Spell("Bloodlust"); //No GCD
    public readonly Spell Heroism = new Spell("Heroism"); //No GCD

    #endregion

    #region Defensive Cooldowns

    //public readonly Spell AncestralGuidance = new Spell("Ancestral Guidance");
    //public readonly Spell CloudburstTotem  = new Spell("Cloudburst Totem");
    //public readonly Spell EarthenShieldTotem  = new Spell("Earthen Shield Totem");
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
    //public readonly Spell HealingRain = new Spell("Healing Rain");
    public readonly Spell HealingSurge = new Spell("Healing Surge");
    public readonly Spell HealingWave = new Spell("Healing Wave");

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
                        }
                            //Setup Group
                        else
                        {
                            double lowestHp = 100;
                            int alivePlayers = 0;
                            PartyHpMedian = 0;
                            DamagedPlayers = 0;
                            foreach (UInt128 playerInMyParty in Party.GetPartyPlayersGUID())
                            {
                                if (playerInMyParty <= 0) continue;
                                WoWObject obj = ObjectManager.GetObjectByGuid(playerInMyParty);
                                if (!obj.IsValid || obj.Type != WoWObjectType.Player) continue;
                                var currentPlayer = new WoWPlayer(obj.GetBaseAddress);
                                if (!currentPlayer.IsValid || !currentPlayer.IsAlive) continue;

                                PartyHpMedian += currentPlayer.HealthPercent;
                                alivePlayers++;
                                if (currentPlayer.HealthPercent < 100)
                                    DamagedPlayers++;
                                //Setup LowestHpPlayer
                                if (currentPlayer.HealthPercent < lowestHp && HealerClass.InRange(currentPlayer))
                                {
                                    lowestHp = currentPlayer.HealthPercent;
                                    LowestHpPlayer = currentPlayer;
                                }
                                //Setup Tank
                                if (currentPlayer.GetUnitRole == WoWUnit.PartyRole.Tank && Tank != currentPlayer)
                                {
                                    Logging.WriteFight("New Tank: " + currentPlayer.Name);
                                    Tank = currentPlayer;
                                }
                            }
                            PartyHpMedian /= alivePlayers;

                            if (LowestHpPlayer.Guid > 0)
                            {
                                //Prioritize Tank
                                if (Tank != null && Tank.HealthPercent < 50)
                                    LowestHpPlayer = Tank;
                                //Prioritize Self
                                if (ObjectManager.Me.HealthPercent < 50 &&
                                    ObjectManager.Me.HealthPercent - 10 < LowestHpPlayer.HealthPercent)
                                {
                                    LowestHpPlayer = ObjectManager.Me;
                                }
                                //Setup Target
                                if (ObjectManager.Me.Target != LowestHpPlayer.Guid && HealerClass.InRange(LowestHpPlayer) && LowestHpPlayer.IsAlive)
                                {
                                    Logging.WriteFight("New Target: " + LowestHpPlayer.Name);
                                    Interact.InteractWith(LowestHpPlayer.GetBaseAddress);
                                }
                                else if (ObjectManager.Me.Target != ObjectManager.Me.Guid)
                                    Interact.InteractWith(ObjectManager.Me.GetBaseAddress);
                            }
                        }
                        Combat();
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

    private void Combat()
    {
        //Burst();	//GCD independent
        GCDCycle(); //GCD dependent
    }

    private void Burst()
    {
        //Offensive Cooldowns
        if (!ObjectManager.Me.HaveBuff(57724) &&
            Bloodlust.IsSpellUsable && MySettings.UseBloodlustHeroism && Bloodlust.KnownSpell)
        {
            Bloodlust.Cast();
        }
        if (!ObjectManager.Me.HaveBuff(57723) &&
            Heroism.IsSpellUsable && MySettings.UseBloodlustHeroism && Heroism.KnownSpell)
        {
            Heroism.Cast();
        }
    }

    private void GCDCycle()
    {
        Usefuls.SleepGlobalCooldown();
        Interact.InteractWith(ObjectManager.Me.GetBaseAddress);

        //Buffs
        if (!Riptide.TargetHaveBuff && ObjectManager.Target.HealthPercent <= MySettings.UseRiptideAtPercentage && //95
            Riptide.IsSpellUsable && MySettings.UseRiptide && Riptide.KnownSpell)
        {
            Riptide.Cast();
            return;
        }

        //Emergency Heals
        if (ObjectManager.Target.HealthPercent <= MySettings.UseHealingSurgeAtPercentage && //40
            //(!MySettings.UseHealingSurgeWithTidalWavesOnly || ObjectManager.Target.HaveBuff(000)) &&//true TODO Implement "Tidal Waves" BuffID
            HealingSurge.IsSpellUsable && MySettings.UseHealingSurge && HealingSurge.KnownSpell)
        {
            HealingSurge.Cast();
            return;
        }
        if (ObjectManager.Target.HealthPercent <= MySettings.UseSpiritLinkTotemAtPercentage && //20
            SpiritLinkTotemCooldown.IsReady && MySettings.UseSpiritLinkTotem && SpiritLinkTotem.KnownSpell)
        {
            SpellManager.CastSpellByIDAndPosition(SpiritLinkTotem.Id, ObjectManager.Target.Position);
            SpiritLinkTotemCooldown = new Timer(1000*180);
            return;
        }

        //Party Heals
        if (PartyHpMedian <= MySettings.UseHealingTideTotemAtPartyPercentage && //60
            HealingTideTotemCooldown.IsReady && MySettings.UseHealingTideTotem && HealingTideTotem.KnownSpell)
        {
            SpellManager.CastSpellByIDAndPosition(HealingTideTotem.Id, ObjectManager.Me.Position);
            HealingTideTotemCooldown = new Timer(1000*180);
            return;
        }
        if (PartyHpMedian <= MySettings.UseChainHealAtPartyPercentage && //80
            DamagedPlayers >= MySettings.UseChainHealAtDamagedPlayers && //4
            ChainHeal.IsSpellUsable && MySettings.UseChainHeal && ChainHeal.KnownSpell)
        {
            ChainHeal.Cast();
            return;
        }

        //OnCD
        if (PartyHpMedian <= MySettings.UseHealingStreamTotemAtPartyPercentage && //95
            HealingStreamTotemCooldown.IsReady && MySettings.UseHealingStreamTotem && HealingStreamTotem.KnownSpell)
        {
            HealingStreamTotem.Cast();
            HealingStreamTotemCooldown = new Timer(1000*30);
            return;
        }

        //Filler
        if (ObjectManager.Target.HealthPercent <= MySettings.UseHealingWaveAtPercentage && //95
            //(!MySettings.UseHealingWaveWithTidalWavesOnly || ObjectManager.Target.HaveBuff(000)) &&//true TODO Implement "Tidal Waves" BuffID
            HealingWave.IsSpellUsable && MySettings.UseHealingWave && HealingWave.KnownSpell)
        {
            HealingWave.Cast();
            return;
        }
    }

    #region Nested type: ShamanRestorationSettings

    [Serializable]
    public class ShamanRestorationSettings : Settings
    {
        /* Healing Cooldowns */
        public bool UseHealingStreamTotem = true;
        public int UseHealingStreamTotemAtPartyPercentage = 95;
        public bool UseHealingTideTotem = true;
        public int UseHealingTideTotemAtPartyPercentage = 40;
        public bool UseSpiritLinkTotem = true;
        public int UseSpiritLinkTotemAtPercentage = 20;
        /* Healing Spells */
        public bool UseChainHeal = true;
        public int UseChainHealAtDamagedPlayers = 4;
        public int UseChainHealAtPartyPercentage = 60;
        public bool UseHealingSurge = true;
        public bool UseHealingSurgeWithTidalWavesOnly = true;
        public int UseHealingSurgeAtPercentage = 40;
        public bool UseHealingWave = true;
        public bool UseHealingWaveWithTidalWavesOnly = true;
        public int UseHealingWaveAtPercentage = 95;
        public bool UseRiptide = true;
        public int UseRiptideAtPercentage = 95;
        /* Offensive Cooldowns */
        public bool UseBloodlustHeroism = false;

        public ShamanRestorationSettings()
        {
            ConfigWinForm("Shaman Restoration Settings");
            /* Healing Cooldowns */
            AddControlInWinForm("Use Healing Stream Totem", "UseHealingStreamTotem", "Healing Cooldowns");
            AddControlInWinForm("Use Healing Stream Totem At Party Wide Percentage", "UseHealingStreamTotemAtPartyPercentage", "Healing Cooldowns", "AtPercentage");
            AddControlInWinForm("Use Healing Tide Totem", "UseHealingTideTotem", "Healing Cooldowns");
            AddControlInWinForm("Use Healing Tide Totem At Party Wide Percentage", "UseHealingTideTotemAtPartyPercentage", "Healing Cooldowns", "AtPercentage");
            AddControlInWinForm("Use Spirit Link Totem", "UseSpiritLinkTotem", "Healing Cooldowns");
            AddControlInWinForm("Use Spirit Link Totem At Percentage", "UseSpiritLinkTotemAtPercentage", "Healing Cooldowns", "AtPercentage");
            /* Healing Spells */
            AddControlInWinForm("Use Chain Heal", "UseChainHeal", "Healing Spells");
            AddControlInWinForm("Use Chain Heal At Damaged Player Count", "UseChainHealAtDamagedPlayers", "Healing Spells", "AtPercentage");
            AddControlInWinForm("Use Chain Heal At Party Wide Percentage", "UseChainHealAtPartyPercentage", "Healing Spells", "AtPercentage");
            AddControlInWinForm("Use Healing Surge", "UseHealingSurge", "Healing Spells");
            AddControlInWinForm("Use Healing Surge Only With Tidal Waves Buff", "UseHealingSurgeWithTidalWavesOnly", "Healing Spells");
            AddControlInWinForm("Use Healing Surge At Percentage", "UseHealingSurgeAtPercentage", "Healing Spells", "AtPercentage");
            AddControlInWinForm("Use Healing Wave", "UseHealingWave", "Healing Spells");
            AddControlInWinForm("Use Healing Wave Only With Tidal Waves Buff", "UseHealingWaveWithTidalWavesOnly", "Healing Spells");
            AddControlInWinForm("Use Healing Wave At Percentage", "UseHealingWaveAtPercentage", "Healing Spells", "AtPercentage");
            AddControlInWinForm("Use Riptide", "UseRiptide", "Healing Spells");
            AddControlInWinForm("Use Riptide At Percentage", "UseRiptideAtPercentage", "Healing Spells", "AtPercentage");
            /* Offensive Cooldowns */
            AddControlInWinForm("Use Bloodlust / Heroism", "UseBloodlustHeroism", "Offensive Cooldowns");
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