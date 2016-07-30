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
                    #region DemonHunter Specialisation checking

                case WoWClass.DemonHunter:

                    if (wowSpecialization == WoWSpecialization.DemonHunterVengeance || wowSpecialization == WoWSpecialization.None)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\DemonHunter_Vengeance.xml";
                            var currentSetting = new DemonHunterVengeance.DemonHunterVengeanceSettings();
                            if (File.Exists(currentSettingsFile) && !resetSettings)
                            {
                                currentSetting = Settings.Load<DemonHunterVengeance.DemonHunterVengeanceSettings>(currentSettingsFile);
                            }
                            currentSetting.ToForm();
                            currentSetting.Save(currentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading DemonHunter Vengeance Combat class...");
                            EquipmentAndStats.SetPlayerSpe(WoWSpecialization.DemonHunterVengeance);
                            new DemonHunterVengeance();
                        }
                        break;
                    }
                    if (wowSpecialization == WoWSpecialization.DemonHunterHavoc)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\DemonHunter_Havor.xml";
                            var currentSetting = new DemonHunterHavoc.DemonHunterHavocSettings();
                            if (File.Exists(currentSettingsFile) && !resetSettings)
                            {
                                currentSetting = Settings.Load<DemonHunterHavoc.DemonHunterHavocSettings>(currentSettingsFile);
                            }
                            currentSetting.ToForm();
                            currentSetting.Save(currentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading DemonHunter Havor Combat class...");
                            EquipmentAndStats.SetPlayerSpe(WoWSpecialization.DemonHunterHavoc);
                            new DemonHunterHavoc();
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

#region DemonHunter

public class DemonHunterHavoc
{
    private static DemonHunterHavocSettings MySettings = DemonHunterHavocSettings.GetSettings();

    #region Professions & Racial

    public readonly Spell ArcaneTorrent = new Spell("Arcane Torrent");
    public readonly Spell GiftoftheNaaru = new Spell("Gift of the Naaru");

    private readonly WoWItem _firstTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET);
    private readonly WoWItem _secondTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET, 2);

    #endregion

    #region DemonHunter Seals & Buffs

    #endregion

    #region Offensive Spell

    #endregion

    #region Offensive Cooldown

    #endregion

    #region Defensive Cooldown

    #endregion

    #region Healing Spell

    #endregion

    public DemonHunterHavoc()
    {
        Main.InternalRange = ObjectManager.Me.GetCombatReach;
        MySettings = DemonHunterHavocSettings.GetSettings();
        Main.DumpCurrentSettings<DemonHunterHavocSettings>(MySettings);
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
                            if (ObjectManager.Me.Target != lastTarget /* && Judgment.IsHostileDistanceGood*/)
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

    private void Pull()
    {
        DPSBurst();
    }

    private void Combat()
    {
        if (MySettings.DoAvoidMelee)
            AvoidMelee();
        DefenseCycle();
        DPSCycle();
        Heal();
        DPSCycle();
        Buffs();
        DPSBurst();
        DPSCycle();
    }

    private void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            Heal();
        }
    }

    private void Buffs()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            if (MySettings.UseAlchFlask && !ObjectManager.Me.HaveBuff(79638) && !ObjectManager.Me.HaveBuff(79640) && !ObjectManager.Me.HaveBuff(79639)
                && !ItemsManager.IsItemOnCooldown(75525) && ItemsManager.GetItemCount(75525) > 0)
                ItemsManager.UseItem(75525);
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.ManaPercentage < 10)
        {
            if (ArcaneTorrent.KnownSpell && MySettings.UseArcaneTorrentForResource && ArcaneTorrent.IsSpellUsable)
            {
                ArcaneTorrent.Cast();
                return;
            }
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
    }

    private void DefenseCycle()
    {
    }

    private void DPSCycle()
    {
        Usefuls.SleepGlobalCooldown();
        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
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

    #region Nested type: DemonHunterHavocSettings

    [Serializable]
    public class DemonHunterHavocSettings : Settings
    {
        public bool DoAvoidMelee = false;
        public int DoAvoidMeleeDistance = 0;
        public bool UseAlchFlask = true;
        public bool UseArcaneTorrentForDecast = true;
        public int UseArcaneTorrentForDecastAtPercentage = 100;
        public bool UseArcaneTorrentForResource = true;
        public int UseArcaneTorrentForResourceAtPercentage = 80;
        public bool UseArdentDefender = true;
        public bool UseAvengersShield = true;
        public bool UseBerserking = true;
        public bool UseConsecration = true;
        public bool UseCrusaderStrike = true;
        public bool UseDivineHavor = true;
        public bool UseDivineShield = true;
        public bool UseFlashOfLight = true;
        public bool UseGiftoftheNaaru = true;
        public int UseGiftoftheNaaruAtPercentage = 80;
        public bool UseGuardianOfAncientKings = true;
        public bool UseHammerOfJustice = true;
        public bool UseHammerOfTheRighteous = true;
        public bool UseHammerOfWrath = true;
        public bool UseHandOfHavor = true;
        public bool UseHandOfPurity = true;
        public bool UseHolyAvenger = true;
        public bool UseHolyWrath = true;
        public bool UseJudgment = true;
        public bool UseLayOnHands = true;

        public bool UseSacredShield = true;
        public bool UseShieldOfTheRighteous = true;
        public bool UseStoneform = true;
        public int UseStoneformAtPercentage = 80;
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;
        public bool UseWarStomp = true;
        public int UseWarStompAtPercentage = 80;
        public bool UseWordOfGlory = true;

        public DemonHunterHavocSettings()
        {
            ConfigWinForm("DemonHunter Havor Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
            AddControlInWinForm("Use Arcane Torrent for Interrupt", "UseArcaneTorrentForDecast", "Professions & Racials", "AtPercentage");
            AddControlInWinForm("Use Arcane Torrent for Resource", "UseArcaneTorrentForResource", "Professions & Racials", "AtPercentage");

            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            /* DemonHunter Seals & Buffs */
            /* Offensive Spell */
            AddControlInWinForm("Use Shield of the Righteous", "UseShieldOfTheRighteous", "Offensive Spell");
            AddControlInWinForm("Use Consecration", "UseConsecration", "Offensive Spell");
            AddControlInWinForm("Use Avenger's Shield", "UseAvengersShield", "Offensive Spell");
            AddControlInWinForm("Use Hammer of Wrath", "UseHammerOfWrath", "Offensive Spell");
            AddControlInWinForm("Use Crusader Strike", "UseCrusaderStrike", "Offensive Spell");
            AddControlInWinForm("Use Hammer of the Righteous", "UseHammerOfTheRighteous", "Offensive Spell");
            AddControlInWinForm("Use Judgment", "UseJudgment", "Offensive Spell");
            AddControlInWinForm("Use Hammer of Justice", "UseHammerOfJustice", "Offensive Spell");
            AddControlInWinForm("Use Holy Wrath", "UseHolyWrath", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use Holy Avenger", "UseHolyAvenger", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Guardian of Ancient Kings", "UseGuardianOfAncientKings", "Defensive Cooldown");
            AddControlInWinForm("Use Ardent Defender", "UseArdentDefender", "Defensive Cooldown");
            AddControlInWinForm("Use Sacred Shield", "UseSacredShield", "Defensive Cooldown");
            AddControlInWinForm("Use Hand of Purity", "UseHandOfPurity", "Defensive Cooldown");
            AddControlInWinForm("Use Divine Havor", "UseDivineHavor", "Defensive Cooldown");
            AddControlInWinForm("Use Divine Shield", "UseDivineShield", "Defensive Cooldown");
            AddControlInWinForm("Use Hand of Havor", "UseHandOfHavor", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Flash of Light", "UseFlashOfLight", "Healing Spell");
            AddControlInWinForm("Use Lay on Hands", "UseLayOnHands", "Healing Spell");
            AddControlInWinForm("Use Word of Glory", "UseWordOfGlory", "Healing Spell");
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");
            AddControlInWinForm("Do avoid melee (Off Advised!!)", "DoAvoidMelee", "Game Settings");
            AddControlInWinForm("Avoid melee distance (1 to 4)", "DoAvoidMeleeDistance", "Game Settings");
        }

        public static DemonHunterHavocSettings CurrentSetting { get; set; }

        public static DemonHunterHavocSettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\DemonHunter_Havor.xml";
            if (File.Exists(currentSettingsFile))
            {
                return
                    CurrentSetting = Load<DemonHunterHavocSettings>(currentSettingsFile);
            }
            return new DemonHunterHavocSettings();
        }
    }

    #endregion
}

public class DemonHunterVengeance
{
    private static DemonHunterVengeanceSettings MySettings = DemonHunterVengeanceSettings.GetSettings();

    #region Professions & Racial

    public readonly Spell ArcaneTorrent = new Spell("Arcane Torrent");
    public readonly Spell GiftoftheNaaru = new Spell("Gift of the Naaru");

    private readonly WoWItem _firstTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET);
    private readonly WoWItem _secondTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET, 2);

    #endregion

    #region DemonHunter Seals & Buffs

    #endregion

    #region Offensive Spell

    #endregion

    #region Offensive Cooldown

    #endregion

    #region Defensive Cooldown

    #endregion

    #region Healing Spell

    #endregion

    #region Flask & Potion Management

/*
        private readonly int _combatPotion = ItemsManager.GetIdByName(MySettings.CombatPotion);
*/
    private readonly int _flaskOrBattleElixir = ItemsManager.GetItemIdByName(MySettings.FlaskOrBattleElixir);
    private readonly int _guardianElixir = ItemsManager.GetItemIdByName(MySettings.GuardianElixir);

/*
        private readonly WoWItem _hands = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_HAND);
        private readonly int _teasureFindingPotion = ItemsManager.GetIdByName(MySettings.TeasureFindingPotion);
        private readonly int _wellFedBuff = ItemsManager.GetIdByName(MySettings.WellFedBuff);
*/

    #endregion

    public DemonHunterVengeance()
    {
        Main.InternalRange = ObjectManager.Me.GetCombatReach;
        MySettings = DemonHunterVengeanceSettings.GetSettings();
        Main.DumpCurrentSettings<DemonHunterVengeanceSettings>(MySettings);
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
                            if (ObjectManager.Me.Target != lastTarget /* && Reckoning.IsHostileDistanceGood*/)
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

    private void Pull()
    {
    }

    private void Combat()
    {
        Buffs();
        DPSBurst();
        if (MySettings.DoAvoidMelee)
            AvoidMelee();
        DPSCycle();
        Heal();
    }

    private void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            if (MySettings.UseFlaskOrBattleElixir && MySettings.FlaskOrBattleElixir != string.Empty)
                if (!SpellManager.HaveBuffLua(ItemsManager.GetItemSpell(MySettings.FlaskOrBattleElixir)) &&
                    !ItemsManager.IsItemOnCooldown(_flaskOrBattleElixir) &&
                    ItemsManager.IsItemUsable(_flaskOrBattleElixir))
                    ItemsManager.UseItem(MySettings.FlaskOrBattleElixir);
            if (MySettings.UseGuardianElixir && MySettings.GuardianElixir != string.Empty)
                if (!SpellManager.HaveBuffLua(ItemsManager.GetItemSpell(MySettings.GuardianElixir)) &&
                    !ItemsManager.IsItemOnCooldown(_guardianElixir) && ItemsManager.IsItemUsable(_guardianElixir))
                    ItemsManager.UseItem(MySettings.GuardianElixir);
            Blessing();
            Heal();
        }
    }

    private void Buffs()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            if (MySettings.UseFlaskOrBattleElixir && MySettings.FlaskOrBattleElixir != string.Empty)
                if (!SpellManager.HaveBuffLua(ItemsManager.GetItemSpell(MySettings.FlaskOrBattleElixir)) &&
                    !ItemsManager.IsItemOnCooldown(_flaskOrBattleElixir) &&
                    ItemsManager.IsItemUsable(_flaskOrBattleElixir))
                    ItemsManager.UseItem(MySettings.FlaskOrBattleElixir);
            if (MySettings.UseGuardianElixir && MySettings.GuardianElixir != string.Empty)
                if (!SpellManager.HaveBuffLua(ItemsManager.GetItemSpell(MySettings.GuardianElixir)) &&
                    !ItemsManager.IsItemOnCooldown(_guardianElixir) && ItemsManager.IsItemUsable(_guardianElixir))
                    ItemsManager.UseItem(MySettings.GuardianElixir);
            Blessing();

            if (MySettings.UseAlchFlask && !ObjectManager.Me.HaveBuff(79638) && !ObjectManager.Me.HaveBuff(79640) && !ObjectManager.Me.HaveBuff(79639)
                && !ItemsManager.IsItemOnCooldown(75525) && ItemsManager.GetItemCount(75525) > 0)
                ItemsManager.UseItem(75525);
        }
    }

    private void Blessing()
    {
        if (ObjectManager.Me.IsMounted)
            return;
        Usefuls.SleepGlobalCooldown();
    }

    private void Heal()
    {
    }

    private void DPSBurst()
    {
    }

    private void DPSCycle()
    {
        Usefuls.SleepGlobalCooldown();
        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
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

    #region Nested type: DemonHunterVengeanceSettings

    [Serializable]
    public class DemonHunterVengeanceSettings : Settings
    {
        public string CombatPotion = "Potion of Mogu Power";
        public bool DoAvoidMelee = false;
        public int DoAvoidMeleeDistance = 0;
        public string FlaskOrBattleElixir = "Flask of Winter's Bite";
        public string GuardianElixir = "";
        public bool RefreshWeakenedBlows = true;
        public string TeasureFindingPotion = "Potion of Luck";
        public bool UseAlchFlask = true;
        public bool UseArcaneTorrentForDecast = true;
        public int UseArcaneTorrentForDecastAtPercentage = 100;
        public bool UseArcaneTorrentForResource = true;
        public int UseArcaneTorrentForResourceAtPercentage = 80;
        public bool UseAvengingWrath = true;
        public bool UseBerserking = true;
        public bool UseGreaterBlessingOfKings = true;
        public bool UseGreaterBlessingOfMight = true;
        public bool UseGreaterBlessingOfWisdom = true;
        public bool UseCombatPotion = false;
        public bool UseCrusaderStrike = true;
        public bool UseBladeOfWrath = true;
        public bool UseBladeOfJustice = true;
        public bool UseDivineHammer = true;
        public bool UseDivineHavor = true;
        public bool UseDivineShield = true;
        public bool UseDivineStorm = true;
        public bool UseExecutionSentence = true;
        public bool UseJusticarsVengeance = true;
        public bool UseFlashOfLight = true;
        public bool UseFlaskOrBattleElixir = false;
        public bool UseGiftoftheNaaru = true;
        public int UseGiftoftheNaaruAtPercentage = 80;
        public bool UseGuardianElixir = false;
        public bool UseHammerOfJustice = true;
        public bool UseHandOfHavor = false;
        public bool UseHolyAvenger = true;
        public bool UseJudgment = true;
        public bool UseLayOnHands = true;

        public bool UseReckoning = true;
        public bool UseSacredShield = true;
        public bool UseStoneform = true;
        public int UseStoneformAtPercentage = 80;
        public bool UseTeasureFindingPotion = false;
        public bool UseTemplarsVerdict = true;
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;
        public bool UseWarStomp = true;
        public int UseWarStompAtPercentage = 80;
        public bool UseWellFedBuff = false;
        public bool UseWordOfGlory = true;

        public string WellFedBuff = "Sleeper Sushi";

        public DemonHunterVengeanceSettings()
        {
            ConfigWinForm("DemonHunter Vengeance Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent for Interrupt", "UseArcaneTorrentForDecast", "Professions & Racials", "AtPercentage");
            AddControlInWinForm("Use Arcane Torrent for Resource", "UseArcaneTorrentForResource", "Professions & Racials", "AtPercentage");

            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            /* DemonHunter Seals & Buffs */
            AddControlInWinForm("Use Greater Blessing of Might", "UseGreaterBlessingOfMight", "DemonHunter Blessings");
            AddControlInWinForm("Use Greater Blessing of Kings", "UseGreaterBlessingOfKings", "DemonHunter Blessings");
            AddControlInWinForm("Use Greater Blessing of Wisdom", "UseGreaterBlessingOfWisdom", "DemonHunter Blessings");
            /* Offensive Spell */
            AddControlInWinForm("Use Templar's Verdict", "UseTemplarsVerdict", "Offensive Spell");
            AddControlInWinForm("Use Justicar's Vengeance", "UseJusticarsVengeance", "Offensive Spell");
            AddControlInWinForm("Use Divine Storm", "UseDivineStorm", "Offensive Spell");
            AddControlInWinForm("Use Crusader Strike", "UseCrusaderStrike", "Offensive Spell");
            AddControlInWinForm("Use Blade of Wrath", "UseBladeOfWrath", "Offensive Spell");
            AddControlInWinForm("Use Blade of Justice", "UseBladeOfJustice", "Offensive Spell");
            AddControlInWinForm("Use Divine Hammer", "UseDivineHammer", "Offensive Spell");
            AddControlInWinForm("Use Judgment", "UseJudgment", "Offensive Spell");
            AddControlInWinForm("Use Hammer of Justice", "UseHammerOfJustice", "Offensive Spell");
            AddControlInWinForm("Use Execution Sentence", "UseExecutionSentence", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use Avenging Wrath", "UseAvengingWrath", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Reckoning", "UseReckoning", "Defensive Cooldown");
            AddControlInWinForm("Use Divine Havor", "UseDivineHavor", "Defensive Cooldown");
            AddControlInWinForm("Use Sacred Shield", "UseSacredShield", "Defensive Cooldown");
            AddControlInWinForm("Use Divine Shield", "UseDivineShield", "Defensive Cooldown");
            AddControlInWinForm("Use Hand of Havor", "UseHandOfHavor", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Flash of Light", "UseFlashOfLight", "Healing Spell");
            AddControlInWinForm("Use Lay on Hands", "UseLayOnHands", "Healing Spell");
            AddControlInWinForm("Use Word of Glory", "UseWordOfGlory", "Healing Spell");
            /* Flask & Potion Management */
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");
            AddControlInWinForm("Use Flask or Battle Elixir", "UseFlaskOrBattleElixir", "Flask & Potion Management");
            AddControlInWinForm("Flask or Battle Elixir Name", "FlaskOrBattleElixir", "Flask & Potion Management");
            AddControlInWinForm("Use Guardian Elixir", "UseGuardianElixir", "Flask & Potion Management");
            AddControlInWinForm("Guardian Elixir Name", "GuardianElixir", "Flask & Potion Management");
            AddControlInWinForm("Use Combat Potion", "UseCombatPotion", "Flask & Potion Management");
            AddControlInWinForm("Combat Potion Name", "CombatPotion", "Flask & Potion Management");
            AddControlInWinForm("Use Teasure Finding Potion", "UseTeasureFindingPotion", "Flask & Potion Management");
            AddControlInWinForm("Teasure Finding Potion Name", "TeasureFindingPotion", "Flask & Potion Management");
            AddControlInWinForm("Use Well Fed Buff", "UseWellFedBuff", "Flask & Potion Management");
            AddControlInWinForm("Well Fed Buff Name", "WellFedBuff", "Flask & Potion Management");
            AddControlInWinForm("Do avoid melee (Off Advised!!)", "DoAvoidMelee", "Game Settings");
            AddControlInWinForm("Avoid melee distance (1 to 4)", "DoAvoidMeleeDistance", "Game Settings");
        }

        public static DemonHunterVengeanceSettings CurrentSetting { get; set; }

        public static DemonHunterVengeanceSettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\DemonHunter_Vengeance.xml";
            if (File.Exists(currentSettingsFile))
            {
                return
                    CurrentSetting = Load<DemonHunterVengeanceSettings>(currentSettingsFile);
            }
            return new DemonHunterVengeanceSettings();
        }
    }

    #endregion
}

#endregion

// ReSharper restore ObjectCreationAsStatement
// ReSharper restore EmptyGeneralCatchClause