/*
* CombatClass for TheNoobBot
* Credit : Vesper, Neo2003, Dreadlocks, Ryuichiro
* Thanks you !
*/

using System;
using System.Linq;
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
                    #region Shaman Specialisation checking

                case WoWClass.Shaman:

                    if (wowSpecialization == WoWSpecialization.ShamanEnhancement)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Shaman_Enhancement.xml";
                            var currentSetting = new ShamanEnhancement.ShamanEnhancementSettings();
                            if (File.Exists(currentSettingsFile) && !resetSettings)
                            {
                                currentSetting = Settings.Load<ShamanEnhancement.ShamanEnhancementSettings>(currentSettingsFile);
                            }
                            currentSetting.ToForm();
                            currentSetting.Save(currentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Shaman Enhancement Combat class...");
                            EquipmentAndStats.SetPlayerSpe(WoWSpecialization.ShamanEnhancement);
                            new ShamanEnhancement();
                        }
                        break;
                    }
                    if (wowSpecialization == WoWSpecialization.ShamanElemental || wowSpecialization == WoWSpecialization.None)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Shaman_Elemental.xml";
                            var currentSetting = new ShamanElemental.ShamanElementalSettings();
                            if (File.Exists(currentSettingsFile) && !resetSettings)
                            {
                                currentSetting = Settings.Load<ShamanElemental.ShamanElementalSettings>(currentSettingsFile);
                            }
                            currentSetting.ToForm();
                            currentSetting.Save(currentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Shaman Elemental Combat class...");
                            InternalRange = 30.0f;
                            EquipmentAndStats.SetPlayerSpe(WoWSpecialization.ShamanElemental);
                            new ShamanElemental();
                        }
                        break;
                    }
                    if (wowSpecialization == WoWSpecialization.ShamanRestoration)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Shaman_Restoration.xml";
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
                            Logging.WriteFight("Loading Shaman Restoration Combat class...");
                            InternalRange = 30.0f;
                            EquipmentAndStats.SetPlayerSpe(WoWSpecialization.ShamanRestoration);
                            new ShamanRestoration();
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

#region Shaman

public class ShamanEnhancement
{
    private static ShamanEnhancementSettings MySettings = ShamanEnhancementSettings.GetSettings();

    #region General Timers & Variables

    private readonly WoWItem _firstTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET);
    private readonly WoWItem _secondTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET, 2);

    private bool CombatMode = true;

    private Timer StunCD = new Timer(0);

    #endregion

    #region Talents

    public readonly Spell CrashingStorm = new Spell("Crashing Storm");
    public readonly Spell Hailstorm = new Spell("Hailstorm");
    //public readonly Spell HotHand = new Spell("Hot Hand");
    //public readonly Spell EmpoweredStormlash = new Spell("Empowered Stormlash");
    //public readonly Spell Landslide = new Spell("Landslide");
    //public readonly Spell Overcharge = new Spell("Overcharge");
    //public readonly Spell Tempest = new Spell("Tempest");

    #endregion

    #region Professions & Racials

    //public readonly Spell ArcaneTorrent = new Spell("Arcane Torrent"); //No GCD
    public readonly Spell Berserking = new Spell("Berserking");
    public readonly Spell BloodFury = new Spell("Blood Fury"); //No GCD
    //public readonly Spell GiftoftheNaaru = new Spell("Gift of the Naaru"); //No GCD
    public readonly Spell Stoneform = new Spell("Stoneform"); //No GCD
    public readonly Spell WarStomp = new Spell("War Stomp");

    #endregion

    #region Shaman Buffs

    public readonly Spell Boulderfist = new Spell("Boulderfist");
    public readonly Spell Flametongue = new Spell("Flametongue");
    public readonly Spell Frostbrand = new Spell("Frostbrand");
    public readonly Spell GhostWolf = new Spell("Ghost Wolf");

    #endregion

    #region Offensive Spells

    public readonly Spell CrashLightning = new Spell("Crash Lightning");
    public readonly Spell FeralLunge = new Spell("Feral Lunge");
    public readonly Spell FeralSpirit = new Spell("Feral Spirit");
    public readonly Spell LavaLash = new Spell("Lava Lash");
    public readonly Spell LightningBolt = new Spell("Lightning Bolt");
    public readonly Spell Rockbiter = new Spell("Rockbiter");
    public readonly Spell Stormstrike = new Spell("Stormstrike");

    #endregion

    #region Offensive Cooldowns

    public readonly Spell Bloodlust = new Spell("Bloodlust"); //No GCD
    public readonly Spell Heroism = new Spell("Heroism"); //No GCD

    #endregion

    #region Defensive Cooldowns

    public readonly Spell AstralShift = new Spell("Astral Shift"); //No GCD
    public readonly Spell LightningSurgeTotem = new Spell("Lightning Surge Totem");

    #endregion

    #region Utility Cooldowns

    public readonly Spell WindRushTotem = new Spell("Wind Rush Totem");

    #endregion

    #region Healing Spells

    public readonly Spell HealingSurge = new Spell("Healing Surge");

    #endregion

    public ShamanEnhancement()
    {
        Main.InternalRange = ObjectManager.Me.GetCombatReach;
        MySettings = ShamanEnhancementSettings.GetSettings();
        Main.DumpCurrentSettings<ShamanEnhancementSettings>(MySettings);
        UInt128 lastTarget = 0;

        //Test();
        //ShowMethods(typeof(Spell));

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
                            if (ObjectManager.Me.Target != lastTarget)
                            {
                                Logging.Write("New Target == " + ObjectManager.Target.Name);
                                lastTarget = ObjectManager.Me.Target;
                            }

                            if (CombatClass.InSpellRange(ObjectManager.Target, 0, 40))
                                Combat();
                            else if (!ObjectManager.Me.IsCast)
                                Patrolling();
                        }
                        else if (!ObjectManager.Me.IsCast)
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

    private void Test()
    {
        Usefuls.SleepGlobalCooldown();
        try
        {
            Memory.WowMemory.GameFrameLock();

            Logging.Write("Hailstorm.HaveBuff == " + Hailstorm.HaveBuff);
            Logging.Write("CrashingStorm.HaveBuff == " + CrashingStorm.HaveBuff);


            //Test: Stormbringer Proc preventing GCD of Stormstrike
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    private static void ShowMethods(Type type)
    {
        foreach (var method in type.GetMethods())
        {
            var parameters = method.GetParameters();
            var parameterDescriptions = string.Join
                (", ", method.GetParameters()
                    .Select(x => x.ParameterType + " " + x.Name)
                    .ToArray());

            Logging.Write(method.Name + "(" + parameterDescriptions + ") [color=#0000ff]returns[/color] " + method.ReturnType);
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

            if (!ObjectManager.Me.GetMove)
            {
                //Healing Surge
                if (ObjectManager.Me.HealthPercent <= MySettings.UseHealingSurgeBelowPercentageOutOfCombat &&
                    HealingSurge.IsSpellUsable && MySettings.UseHealingSurge)
                {
                    HealingSurge.CastOnSelf();
                    return;
                }
            }
            else
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

        //Oneshot
        if (MySettings.TryOneshot && Oneshot())
            return;

        //Combat
        if (CombatClass.InSpellRange(ObjectManager.Target, 0, MySettings.BurstDistance))
            Burst(); //GCD independent
        GCDCycle(); //GCD dependent
    }

    private bool Oneshot()
    {
        Usefuls.SleepGlobalCooldown();
        try
        {
            Memory.WowMemory.GameFrameLock();

            int leveldif = (int) ObjectManager.Me.Level - (int) ObjectManager.Target.Level;

            if (ObjectManager.Target.HealthPercent == 100 && leveldif > MySettings.TryOneshotLevelDifference)
            {
                Logging.WriteFight("Target is " + leveldif + " Levels lower. Try Onehot!");

                //Charge
                if (FeralLunge.IsHostileDistanceGood && FeralLunge.IsSpellUsable && MySettings.UseFeralLunge)
                {
                    FeralLunge.Cast();
                    return true;
                }

                //Close Range
                if (Boulderfist.KnownSpell)
                {
                    if (Boulderfist.IsSpellUsable && Boulderfist.IsHostileDistanceGood && MySettings.UseBoulderfist)
                    {
                        Boulderfist.Cast();
                        return true;
                    }
                }
                else if (Rockbiter.IsHostileDistanceGood && Rockbiter.IsSpellUsable && MySettings.UseRockbiter)
                {
                    Rockbiter.Cast();
                    return true;
                }

                //Long Range
                if (LightningBolt.IsHostileDistanceGood && LightningBolt.IsSpellUsable && MySettings.UseLightningBolt)
                {
                    LightningBolt.Cast();
                    return true;
                }
            }
            return false;
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    private void Burst()
    {
        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            //Defensive Buffs
            if (ObjectManager.Me.HealthPercent < MySettings.UseAstralShiftBelowPercentage &&
                AstralShift.IsSpellUsable && MySettings.UseAstralShift)
            {
                AstralShift.Cast();
            }
            if (StunCD.IsReady)
            {
                if (ObjectManager.Me.HealthPercent < MySettings.UseStoneformBelowPercentage &&
                    Stoneform.IsSpellUsable && MySettings.UseStoneform)
                {
                    Stoneform.Cast();
                }
            }

            //Trinkets
            if (!ItemsManager.IsItemOnCooldown(_firstTrinket.Entry) && ItemsManager.IsItemUsable(_firstTrinket.Entry) && MySettings.UseTrinketOne)
            {
                ItemsManager.UseItem(_firstTrinket.Name);
                Logging.WriteFight("Use First Trinket Slot");
            }
            if (!ItemsManager.IsItemOnCooldown(_secondTrinket.Entry) && ItemsManager.IsItemUsable(_secondTrinket.Entry) && MySettings.UseTrinketTwo)
            {
                ItemsManager.UseItem(_secondTrinket.Name);
                Logging.WriteFight("Use Second Trinket Slot");
            }

            //Offensive Buffs
            if (BloodFury.IsSpellUsable && MySettings.UseBloodFury)
            {
                BloodFury.Cast();
            }

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
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    private void GCDCycle()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            //Stuns
            if (StunCD.IsReady)
            {
                if (ObjectManager.Me.HealthPercent < MySettings.UseLightningSurgeTotemBelowPercentage &&
                    LightningSurgeTotem.IsSpellUsable && MySettings.UseLightningSurgeTotem)
                {
                    SpellManager.CastSpellByIDAndPosition(LightningSurgeTotem.Id, ObjectManager.Target.Position);
                    StunCD = new Timer(1000*7);
                    return;
                }
                if (ObjectManager.Me.HealthPercent < MySettings.UseWarStompBelowPercentage &&
                    WarStomp.IsSpellUsable && MySettings.UseWarStomp)
                {
                    WarStomp.Cast();
                    StunCD = new Timer(1000*2);
                    return;
                }
            }

            //Heals
            if (ObjectManager.Me.HealthPercent < MySettings.UseHealingSurgeBelowPercentage &&
                (!MySettings.UseHealingSurgeInstantOnly || ObjectManager.Me.Maelstrom >= 20) &&
                HealingSurge.IsSpellUsable && MySettings.UseHealingSurge)
            {
                HealingSurge.CastOnSelf();
                return;
            }

            //Offensive Buffs
            if (!Boulderfist.HaveBuff && Boulderfist.IsSpellUsable &&
                Boulderfist.IsHostileDistanceGood && MySettings.UseBoulderfist)
            {
                Boulderfist.Cast();
                return;
            }
            if (!Frostbrand.HaveBuff && Frostbrand.IsSpellUsable &&
                Frostbrand.IsHostileDistanceGood && Hailstorm.HaveBuff &&
                MySettings.UseFrostbrand)
            {
                Frostbrand.Cast();
                return;
            }
            if (ObjectManager.Me.Maelstrom < 130 && Boulderfist.GetSpellCharges == 2 &&
                Boulderfist.IsSpellUsable && Boulderfist.IsHostileDistanceGood &&
                MySettings.UseBoulderfist)
            {
                Boulderfist.Cast();
                return;
            }
            if (!Flametongue.HaveBuff && Flametongue.IsSpellUsable &&
                Flametongue.IsHostileDistanceGood && MySettings.UseFlametongue)
            {
                Flametongue.Cast();
                return;
            }
            if (Berserking.IsSpellUsable && CombatClass.InSpellRange(ObjectManager.Target, 0, MySettings.BurstDistance) &&
                MySettings.UseBerserking)
            {
                Berserking.Cast();
                return;
            }

            //Offensive Cooldowns
            if (FeralSpirit.IsSpellUsable && CombatClass.InSpellRange(ObjectManager.Target, 0, MySettings.BurstDistance) &&
                MySettings.UseFeralSpirit)
            {
                FeralSpirit.Cast();
                return;
            }
            if (ObjectManager.GetUnitInSpellRange(5f) > 2 && CrashLightning.IsHostileDistanceGood &&
                CrashLightning.IsSpellUsable && MySettings.UseCrashLightning)
            {
                CrashLightning.Cast();
                return;
            }
            if (Stormstrike.IsSpellUsable && Stormstrike.IsHostileDistanceGood &&
                MySettings.UseStormstrike)
            {
                Stormstrike.Cast();
                return;
            }
            if (CrashLightning.IsSpellUsable && CrashLightning.IsHostileDistanceGood &&
                CrashingStorm.HaveBuff && MySettings.UseCrashLightning)
            {
                CrashLightning.Cast();
                return;
            }

            //Offensive Buffs (Pandemic)
            if (ObjectManager.Me.UnitAura(Frostbrand.Ids).AuraTimeLeftInMs < 45000 &&
                Frostbrand.IsSpellUsable && Frostbrand.IsHostileDistanceGood &&
                Hailstorm.HaveBuff && MySettings.UseFrostbrand)
            {
                Frostbrand.Cast();
                return;
            }
            if (ObjectManager.Me.UnitAura(Flametongue.Ids).AuraTimeLeftInMs < 48000 &&
                Flametongue.IsSpellUsable && Flametongue.IsHostileDistanceGood && MySettings.UseFlametongue)
            {
                Flametongue.Cast();
                return;
            }

            //Offensive Spells
            if (ObjectManager.Me.Maelstrom > 110 && LavaLash.IsSpellUsable &&
                LavaLash.IsHostileDistanceGood && MySettings.UseLavaLash)
            {
                LavaLash.Cast();
                return;
            }
            if (FeralLunge.IsHostileDistanceGood && FeralLunge.IsSpellUsable && MySettings.UseFeralLunge)
            {
                //Logging.WriteFight("IsHostileDistanceGood == " + FeralLunge.IsHostileDistanceGood);
                //Logging.WriteFight("Target.GetDistance == " + ObjectManager.Target.GetDistance);
                //Logging.WriteFight("InSpellRange(MinRangeHostile, MaxRangeHostile) == " + CombatClass.InSpellRange(ObjectManager.Target, FeralLunge.MinRangeHostile, FeralLunge.MaxRangeHostile));
                //Logging.WriteFight("InSpellRange(8, 25) == " + CombatClass.InSpellRange(ObjectManager.Target, 8, 25));
                //Logging.WriteFight("MinRangeHostile == " + FeralLunge.MinRangeHostile);
                //Logging.WriteFight("MaxRangeHostile == " + FeralLunge.MaxRangeHostile);
                FeralLunge.Cast();
                return;
            }
            if (Boulderfist.KnownSpell)
            {
                if (Boulderfist.IsSpellUsable && Boulderfist.IsHostileDistanceGood && MySettings.UseBoulderfist)
                {
                    Boulderfist.Cast();
                    return;
                }
            }
            else if (Rockbiter.IsHostileDistanceGood && Rockbiter.IsSpellUsable && MySettings.UseRockbiter)
            {
                Rockbiter.Cast();
                return;
            }
            if (Flametongue.IsHostileDistanceGood && Flametongue.IsSpellUsable && MySettings.UseFlametongue)
            {
                Flametongue.Cast();
                return;
            }

            if (CombatClass.InSpellRange(ObjectManager.Target, 5f, LightningBolt.MaxRangeHostile) &&
                LightningBolt.IsSpellUsable && MySettings.UseLightningBolt)
            {
                LightningBolt.Cast();
                return;
            }
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    #region Nested type: ShamanEnhancementSettings

    [Serializable]
    public class ShamanEnhancementSettings : Settings
    {
        /* Professions & Racials */
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseStoneform = true;
        public int UseStoneformBelowPercentage = 80;
        public bool UseWarStomp = true;
        public int UseWarStompBelowPercentage = 80;
        /* Shaman Buffs */
        public bool UseBoulderfist = true;
        public bool UseFlametongue = true;
        public bool UseFrostbrand = true;
        public bool UseGhostWolf = true;
        /* Offensive Spells */
        public bool UseCrashLightning = true;
        public bool UseFeralLunge = true;
        public bool UseLavaLash = true;
        public bool UseLightningBolt = true;
        public bool UseRockbiter = true;
        public bool UseStormstrike = true;
        /* Offensive Cooldowns */
        public bool UseBloodlustHeroism = false;
        public bool UseFeralSpirit = true;
        /* Defensive Cooldowns */
        public bool UseAstralShift = true;
        public int UseAstralShiftBelowPercentage = 80;
        public bool UseLightningSurgeTotem = true;
        public int UseLightningSurgeTotemBelowPercentage = 80;
        /* Utility Cooldowns */
        public bool UseWindRushTotem = true;
        /* Healing Spells */
        public bool UseHealingSurge = true;
        public bool UseHealingSurgeInstantOnly = true;
        public int UseHealingSurgeBelowPercentage = 50;
        public int UseHealingSurgeBelowPercentageOutOfCombat = 95;
        /* Game Settings */
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;
        public int BurstDistance = 30;
        public bool TryOneshot = true;
        public int TryOneshotLevelDifference = 20;

        public ShamanEnhancementSettings()
        {
            ConfigWinForm("Shaman Enhancement Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use Stoneform", "UseStoneformBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStompBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            /* Shaman Buffs */
            AddControlInWinForm("Use Boulderfist", "UseBoulderfist", "Shaman Buffs");
            AddControlInWinForm("Use Flametongue", "UseFlametongue", "Shaman Buffs");
            AddControlInWinForm("Use Frostbrand", "UseFrostbrand", "Shaman Buffs");
            AddControlInWinForm("Use Ghost Wolf", "UseGhostWolf", "Shaman Buffs");
            /* Offensive Spells */
            AddControlInWinForm("Use Crash Lightning", "UseCrashLightning", "Offensive Spells");
            AddControlInWinForm("Use Feral Lunge", "UseFeralLunge", "Offensive Cooldowns");
            AddControlInWinForm("Use Lightning Bolt", "UseLightningBolt", "Offensive Spells");
            AddControlInWinForm("Use Lava Lash", "UseLavaLash", "Offensive Spells");
            AddControlInWinForm("Use Rockbiter", "UseRockbiter", "Offensive Spells");
            AddControlInWinForm("Use Stormstrike", "UseStormstrike", "Offensive Spells");
            /* Offensive Cooldowns */
            AddControlInWinForm("Use Bloodlust / Heroism", "UseBloodlustHeroism", "Offensive Cooldowns");
            AddControlInWinForm("Use Feral Spirit", "UseFeralSpirit", "Offensive Cooldowns");
            /* Defensive Cooldowns */
            AddControlInWinForm("Use Astral", "UseAstralShift", "Defensive Cooldowns");
            AddControlInWinForm("Use Astral Shift", "UseAstralShiftBelowPercentage", "Defensive Cooldowns", "BelowPercentage", "Life");
            AddControlInWinForm("Use Lightning Surge Totem", "UseLightningSurgeTotem", "Defensive Cooldowns");
            AddControlInWinForm("Use Lightning Surge Totem", "UseLightningSurgeTotemBelowPercentage", "Defensive Cooldowns", "BelowPercentage", "Life");
            /* Utility Cooldowns */
            AddControlInWinForm("Use Wind Rush Totem", "UseWindRushTotem", "Utility Cooldowns");
            /* Healing Spells */
            AddControlInWinForm("Use Healing Surge", "UseHealingSurge", "Healing Spells");
            AddControlInWinForm("Use Healing Surge Only With Maelstrom In Fight", "UseHealingSurgeInstantOnly", "Healing Spells");
            AddControlInWinForm("Use Healing Surge while In Fight", "UseHealingSurgeBelowPercentage", "Healing Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Healing Surge while Out Of Fight", "UseHealingSurgeBelowPercentageOutOfCombat", "Healing Spells", "BelowPercentage", "Life");
            /* Game Settings */
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");
            AddControlInWinForm("Burst if Target is below Range", "BurstDistance", "Game Settings"); //TODO add BelowPercentage alternative
            AddControlInWinForm("Try Oneshotting Enemies", "TryOneshot", "Game Settings");
            AddControlInWinForm("Try Oneshotting Enemies if Level Difference is above", "TryOneshotLevelDifference", "Game Settings"); //TODO add AbovePercentage alternative
        }

        public static ShamanEnhancementSettings CurrentSetting { get; set; }

        public static ShamanEnhancementSettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Shaman_Enhancement.xml";
            if (File.Exists(currentSettingsFile))
            {
                return
                    CurrentSetting = Load<ShamanEnhancementSettings>(currentSettingsFile);
            }
            return new ShamanEnhancementSettings();
        }
    }

    #endregion
}

public class ShamanRestoration
{
    private static ShamanRestorationSettings MySettings = ShamanRestorationSettings.GetSettings();

    #region General Timers & Variables

    private readonly WoWItem _firstTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET);
    private readonly WoWItem _secondTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET, 2);
    public int LC = 0;

    private Timer _onCd = new Timer(0);

    #endregion

    #region Professions & Racials

    public readonly Spell Alchemy = new Spell("Alchemy");
    public readonly Spell ArcaneTorrent = new Spell("Arcane Torrent");
    public readonly Spell Berserking = new Spell("Berserking");
    public readonly Spell BloodFury = new Spell("Blood Fury");
    public readonly Spell GiftoftheNaaru = new Spell("Gift of the Naaru");
    public readonly Spell Stoneform = new Spell("Stoneform");
    public readonly Spell WarStomp = new Spell("War Stomp");

    #endregion

    #region Shaman Buffs

    public readonly Spell EarthShield = new Spell("Earth Shield");
    public readonly Spell EarthlivingWeapon = new Spell("Earthliving Weapon");
    public readonly Spell Flametongue = new Spell("Flametongue Weapon");
    public readonly Spell Frostbrand = new Spell("Frostbrand Weapon");
    public readonly Spell GhostWolf = new Spell("Ghost Wolf");
    public readonly Spell LightningShield = new Spell("Lightning Shield");
    public readonly Spell RockbiterWeapon = new Spell("Rockbiter Weapon");
    public readonly Spell SpiritwalkersGrace = new Spell("Spiritwalker's Grace");
    public readonly Spell WaterShield = new Spell("Water Shield");
    public readonly Spell WaterWalking = new Spell("Water Walking");
    private Timer _waterWalkingTimer = new Timer(0);

    #endregion

    #region Offensive Spells

    public readonly Spell ChainLightning = new Spell("Chain Lightning");
    public readonly Spell EarthShock = new Spell("Earth Shock");
    public readonly Spell FlameShock = new Spell("Flame Shock");
    public readonly Spell FrostShock = new Spell("Frost Shock");
    public readonly Spell LavaBurst = new Spell("Lava Burst");
    public readonly Spell LightningBolt = new Spell("Lightning Bolt");
    public readonly Spell MagmaTotem = new Spell("Magma Totem");
    public readonly Spell PrimalStrike = new Spell("Primal Strike");
    public readonly Spell SearingTotem = new Spell("Searing Totem");
    private Timer _flameShockTimer = new Timer(0);

    #endregion

    #region Offensive Cooldowns

    public readonly Spell AncestralSwiftness = new Spell("Ancestral Swiftness");
    public readonly Spell Ascendance = new Spell("Ascendance");
    public readonly Spell Bloodlust = new Spell("Bloodlust");
    public readonly Spell CalloftheElements = new Spell("Call of the Elements");
    public readonly Spell EarthElementalTotem = new Spell("Earth Elemental Totem");
    public readonly Spell ElementalBlast = new Spell("Elemental Blast");
    public readonly Spell ElementalMastery = new Spell("Elemental Mastery");
    public readonly Spell FireElementalTotem = new Spell("Fire Elemental Totem");
    public readonly Spell Heroism = new Spell("Heroism");
    public readonly Spell StormlashTotem = new Spell("Stormlash Totem");
    public readonly Spell TotemicProjection = new Spell("Totemic Projection");
    public readonly Spell UnleashElements = new Spell("Unleash Elements");
    public readonly Spell UnleashedFury = new Spell("Unleashed Fury");

    #endregion

    #region Defensive Cooldowns

    public readonly Spell AstralShift = new Spell("Astral Shift");
    public readonly Spell CapacitorTotem = new Spell("Capacitor Totem");
    public readonly Spell EarthbindTotem = new Spell("Earthbind Totem");
    public readonly Spell GroundingTotem = new Spell("Grounding Totem");
    public readonly Spell StoneBulwarkTotem = new Spell("Stone Bulwark Totem");
    public readonly Spell WindShear = new Spell("Wind Shear");

    #endregion

    #region Healing Spells

    public readonly Spell AncestralGuidance = new Spell("Ancestral Guidance");
    public readonly Spell ChainHeal = new Spell("Chain Heal");
    public readonly Spell GreaterHealingWave = new Spell("Greater Healing Wave");
    public readonly Spell HealingRain = new Spell("Healing Rain");
    public readonly Spell HealingStreamTotem = new Spell("Healing Stream Totem");
    public readonly Spell HealingSurge = new Spell("Healing Surge");
    public readonly Spell HealingTideTotem = new Spell("Healing Tide Totem");
    public readonly Spell HealingWave = new Spell("HealingWave");
    public readonly Spell ManaTideTotem = new Spell("Mana Tide Totem");
    public readonly Spell Riptide = new Spell("Riptide");
    public readonly Spell SpiritLinkTotem = new Spell("Spirit Link Totem");
    public readonly Spell TotemicRecall = new Spell("Totemic Recall");

    #endregion

    public ShamanRestoration()
    {
        Main.InternalRange = 30.0f;
        Main.InternalAggroRange = 30f;
        MySettings = ShamanRestorationSettings.GetSettings();
        Main.DumpCurrentSettings<ShamanRestorationSettings>(MySettings);
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
                                && (FlameShock.IsHostileDistanceGood || LightningBolt.IsHostileDistanceGood))
                            {
                                Pull();
                                lastTarget = ObjectManager.Me.Target;
                            }

                            if (ObjectManager.Target.Level < 70 && ObjectManager.Me.Level > 84
                                && MySettings.UseLowCombat)
                            {
                                LC = 1;
                                if (ObjectManager.Target.GetDistance <= 40f)
                                    LowCombat();
                            }
                            else
                            {
                                LC = 0;
                                if (ObjectManager.Target.GetDistance <= 40f)
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

    private void Pull()
    {
        if (TotemicProjection.KnownSpell && TotemicProjection.IsSpellUsable && MySettings.UseTotemicProjection)
            TotemicProjection.Cast();

        if (FlameShock.KnownSpell && FlameShock.IsSpellUsable && FlameShock.IsHostileDistanceGood
            && MySettings.UseFlameShock && LC != 1)
        {
            FlameShock.Cast();
            return;
        }
        if (EarthShock.KnownSpell && EarthShock.IsSpellUsable && EarthShock.IsHostileDistanceGood
            && MySettings.UseEarthShock)
        {
            EarthShock.Cast();
        }
    }

    private void LowCombat()
    {
        Buff();
        if (MySettings.DoAvoidMelee)
            AvoidMelee();
        DefenseCycle();
        Heal();

        if (EarthShock.KnownSpell && EarthShock.IsSpellUsable && EarthShock.IsHostileDistanceGood
            && MySettings.UseEarthShock)
        {
            EarthShock.Cast();
            return;
        }
        if (LavaBurst.KnownSpell && LavaBurst.IsSpellUsable && LavaBurst.IsHostileDistanceGood
            && MySettings.UseLavaBurst)
        {
            LavaBurst.Cast();
            return;
        }
        if (ChainLightning.KnownSpell && ChainLightning.IsSpellUsable && ChainLightning.IsHostileDistanceGood
            && MySettings.UseChainLightning)
        {
            ChainLightning.Cast();
            return;
        }
        if (SearingTotem.KnownSpell && SearingTotem.IsSpellUsable && MySettings.UseSearingTotem
            && FireTotemReady() && !SearingTotem.CreatedBySpellInRange(25) && ObjectManager.Target.GetDistance < 31)
        {
            SearingTotem.Cast();
            return;
        }
        if (MagmaTotem.KnownSpell && MagmaTotem.IsSpellUsable && ObjectManager.Target.GetDistance < 8
            && MySettings.UseMagmaTotem && FireTotemReady())
        {
            MagmaTotem.Cast();
        }
    }

    private void Combat()
    {
        Buff();
        DPSBurst();
        if (MySettings.DoAvoidMelee)
            AvoidMelee();
        DPSCycle();
        Decast();
        if (_onCd.IsReady)
            DefenseCycle();
        Heal();
    }

    private void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (WaterWalking.IsSpellUsable && WaterWalking.KnownSpell &&
            (!WaterWalking.HaveBuff || _waterWalkingTimer.IsReady)
            && !ObjectManager.Me.InCombat && MySettings.UseWaterWalking)
        {
            WaterWalking.CastOnSelf();
            _waterWalkingTimer = new Timer(1000*60*9);
            return;
        }
        if (MySettings.UseWaterShield && !WaterShield.HaveBuff && WaterShield.KnownSpell && WaterShield.IsSpellUsable &&
            (!MySettings.UseLightningShield && !MySettings.UseEarthShield || ObjectManager.Me.ManaPercentage < 5))
        {
            WaterShield.CastOnSelf();
            return;
        }
        if (MySettings.UseEarthShield && !MySettings.UseLightningShield && !EarthShield.HaveBuff && EarthShield.KnownSpell && ObjectManager.Me.HealthPercent < 50 && EarthShield.IsSpellUsable)
        {
            EarthShield.Cast();
            return;
        }
        if (MySettings.UseLightningShield && !MySettings.UseEarthShield && (ObjectManager.Me.ManaPercentage > 10 || !MySettings.UseWaterShield) && LightningShield.KnownSpell && LightningShield.IsSpellUsable &&
            !LightningShield.HaveBuff)
        {
            LightningShield.CastOnSelf();
            return;
        }
        if (ObjectManager.Me.InCombat && SpiritwalkersGrace.IsSpellUsable
            && SpiritwalkersGrace.KnownSpell && MySettings.UseSpiritwalkersGrace && ObjectManager.Me.GetMove)
        {
            SpiritwalkersGrace.Cast();
            return;
        }
        if (Flametongue.KnownSpell && Flametongue.IsSpellUsable && !ObjectManager.Me.HaveBuff(10400)
            && MySettings.UseFlametongue)
        {
            Flametongue.Cast();
            return;
        }
        if (EarthlivingWeapon.KnownSpell && EarthlivingWeapon.IsSpellUsable &&
            !ObjectManager.Me.HaveBuff(52007)
            && MySettings.UseEarthlivingWeapon && !MySettings.UseFlametongue)
        {
            EarthlivingWeapon.Cast();
            return;
        }
        if (Frostbrand.KnownSpell && Frostbrand.IsSpellUsable &&
            !ObjectManager.Me.HaveBuff(8034)
            && MySettings.UseFrostbrand && !MySettings.UseFlametongue &&
            !MySettings.UseEarthlivingWeapon)
        {
            Frostbrand.Cast();
            return;
        }
        if (RockbiterWeapon.KnownSpell && RockbiterWeapon.IsSpellUsable &&
            !ObjectManager.Me.HaveBuff(36494)
            && MySettings.UseRockbiterWeapon && !MySettings.UseFlametongue
            && !MySettings.UseFrostbrand && !MySettings.UseEarthlivingWeapon)
        {
            RockbiterWeapon.Cast();
            return;
        }

        if (MountTask.GetMountCapacity() == MountCapacity.Ground && !ObjectManager.Me.InCombat && GhostWolf.IsSpellUsable && GhostWolf.KnownSpell
            && MySettings.UseGhostWolf && ObjectManager.Me.GetMove && !GhostWolf.HaveBuff
            && ObjectManager.Target.GetDistance > 50)
        {
            GhostWolf.Cast();
            return;
        }

        if (MySettings.UseAlchFlask && !ObjectManager.Me.HaveBuff(79638) && !ObjectManager.Me.HaveBuff(79640) && !ObjectManager.Me.HaveBuff(79639)
            && !ItemsManager.IsItemOnCooldown(75525) && ItemsManager.GetItemCount(75525) > 0)
        {
            ItemsManager.UseItem(75525);
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
        if (ObjectManager.Me.HealthPercent < 50 && CapacitorTotem.KnownSpell && CapacitorTotem.IsSpellUsable
            && AirTotemReady() && MySettings.UseCapacitorTotem)
        {
            CapacitorTotem.Cast();
            _onCd = new Timer(1000*5);
            return;
        }
        if (ObjectManager.Me.HealthPercent < 50 && StoneBulwarkTotem.KnownSpell &&
            StoneBulwarkTotem.IsSpellUsable
            && EarthTotemReady() && MySettings.UseStoneBulwarkTotem)
        {
            StoneBulwarkTotem.Cast();
            _onCd = new Timer(1000*10);
            return;
        }
        if (ObjectManager.Me.HealthPercent < 70 && SpiritLinkTotem.KnownSpell &&
            SpiritLinkTotem.IsSpellUsable
            && AirTotemReady() && MySettings.UseSpiritLinkTotem)
        {
            SpiritLinkTotem.Cast();
            _onCd = new Timer(1000*6);
            return;
        }
        if (ObjectManager.Me.HealthPercent <= MySettings.UseWarStompBelowPercentage && WarStomp.IsSpellUsable &&
            WarStomp.KnownSpell
            && MySettings.UseWarStomp)
        {
            WarStomp.Cast();
            _onCd = new Timer(1000*2);
            return;
        }
        if (ObjectManager.Me.HealthPercent <= MySettings.UseStoneformBelowPercentage && Stoneform.IsSpellUsable &&
            Stoneform.KnownSpell
            && MySettings.UseStoneform)
        {
            Stoneform.Cast();
            _onCd = new Timer(1000*8);
            return;
        }
        if (ObjectManager.Me.HealthPercent < 70 && AstralShift.KnownSpell && AstralShift.IsSpellUsable
            && MySettings.UseAstralShift)
        {
            AstralShift.Cast();
            _onCd = new Timer(1000*6);
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (ArcaneTorrent.IsSpellUsable && ArcaneTorrent.KnownSpell &&
            ObjectManager.Me.ManaPercentage <= MySettings.UseArcaneTorrentForResourceBelowPercentage
            && MySettings.UseArcaneTorrentForResource)
        {
            ArcaneTorrent.Cast();
            return;
        }
        if (ObjectManager.Me.ManaPercentage < 50 && TotemicRecall.KnownSpell && TotemicRecall.IsSpellUsable
            && MySettings.UseTotemicRecall && !ObjectManager.Me.InCombat
            && TotemicRecallReady())
        {
            TotemicRecall.Cast();
            return;
        }
        if (ObjectManager.Me.ManaPercentage < 80 && ManaTideTotem.KnownSpell && ManaTideTotem.IsSpellUsable
            && MySettings.UseManaTideTotem && WaterTotemReady())
        {
            ManaTideTotem.Cast();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 95 && HealingSurge.KnownSpell && HealingSurge.IsSpellUsable
            && !ObjectManager.Me.InCombat && MySettings.UseHealingSurge)
        {
            HealingSurge.Cast();
            while (ObjectManager.Me.IsCast)
            {
                Others.SafeSleep(200);
            }
            return;
        }
        if (HealingSurge.KnownSpell && HealingSurge.IsSpellUsable && ObjectManager.Me.HealthPercent < 50
            && MySettings.UseHealingSurge)
        {
            HealingSurge.Cast();
            return;
        }
        if (GreaterHealingWave.KnownSpell && GreaterHealingWave.IsSpellUsable
            && ObjectManager.Me.HealthPercent < 60 && MySettings.UseGreaterHealingWave)
        {
            GreaterHealingWave.Cast();
            return;
        }
        if (ObjectManager.Me.HealthPercent <= MySettings.UseGiftoftheNaaruBelowPercentage &&
            GiftoftheNaaru.KnownSpell && GiftoftheNaaru.IsSpellUsable
            && MySettings.UseGiftoftheNaaru)
        {
            GiftoftheNaaru.Cast();
            return;
        }
        if (HealingTideTotem.KnownSpell && HealingTideTotem.IsSpellUsable &&
            ObjectManager.Me.HealthPercent < 70
            && WaterTotemReady() && MySettings.UseHealingTideTotem)
        {
            HealingTideTotem.Cast();
            return;
        }
        if (AncestralGuidance.KnownSpell && AncestralGuidance.IsSpellUsable &&
            ObjectManager.Me.HealthPercent < 70
            && MySettings.UseAncestralGuidance)
        {
            AncestralGuidance.Cast();
            return;
        }
        if (ChainHeal.KnownSpell && ChainHeal.IsSpellUsable && ObjectManager.Me.HealthPercent < 80
            && MySettings.UseChainHeal)
        {
            ChainHeal.Cast();
            return;
        }
        if (HealingStreamTotem.KnownSpell && HealingStreamTotem.IsSpellUsable &&
            ObjectManager.Me.HealthPercent < 90
            && WaterTotemReady() && MySettings.UseHealingStreamTotem)
        {
            HealingStreamTotem.Cast();
            return;
        }
        if (Riptide.KnownSpell && Riptide.IsSpellUsable && ObjectManager.Me.HealthPercent < 90
            && MySettings.UseRiptide && !Riptide.HaveBuff)
        {
            Riptide.Cast();
            return;
        }
        if (HealingWave.KnownSpell && HealingWave.IsSpellUsable && ObjectManager.Me.HealthPercent < 80
            && MySettings.UseHealingWave)
        {
            HealingWave.Cast();
        }
    }

    private void Decast()
    {
        if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && MySettings.UseWindShear
            && WindShear.KnownSpell && WindShear.IsSpellUsable && WindShear.IsHostileDistanceGood)
        {
            WindShear.Cast();
            return;
        }
        if (ArcaneTorrent.IsSpellUsable && ArcaneTorrent.KnownSpell && ObjectManager.Target.GetDistance < 8
            && ObjectManager.Me.HealthPercent <= MySettings.UseArcaneTorrentForDecastBelowPercentage
            && MySettings.UseArcaneTorrentForDecast && ObjectManager.Target.IsCast &&
            ObjectManager.Target.IsTargetingMe)
        {
            ArcaneTorrent.Cast();
            return;
        }
        if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && MySettings.UseGroundingTotem
            && GroundingTotem.KnownSpell && GroundingTotem.IsSpellUsable && AirTotemReady())
        {
            GroundingTotem.Cast();
            return;
        }

        if (ObjectManager.Target.GetMove && !FrostShock.TargetHaveBuff && MySettings.UseFrostShock
            && FrostShock.KnownSpell && FrostShock.IsSpellUsable && FrostShock.IsHostileDistanceGood)
        {
            FrostShock.Cast();
            return;
        }
        if (ObjectManager.Target.GetMove && MySettings.UseEarthbindTotem && EarthTotemReady()
            && EarthbindTotem.KnownSpell && EarthbindTotem.IsSpellUsable && EarthbindTotem.IsHostileDistanceGood)
        {
            EarthbindTotem.Cast();
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
            return;
        }
        if (Berserking.IsSpellUsable && Berserking.KnownSpell && ObjectManager.Target.GetDistance <= 40f
            && MySettings.UseBerserking)
        {
            Berserking.Cast();
            return;
        }
        if (BloodFury.IsSpellUsable && BloodFury.KnownSpell && ObjectManager.Target.GetDistance <= 40f
            && MySettings.UseBloodFury)
        {
            BloodFury.Cast();
            return;
        }
        if (UnleashElements.KnownSpell && UnleashElements.IsSpellUsable && UnleashedFury.KnownSpell
            && MySettings.UseUnleashElements && UnleashElements.IsHostileDistanceGood)
        {
            UnleashElements.Cast();
            return;
        }
        if (ElementalBlast.KnownSpell && ElementalBlast.IsSpellUsable
            && MySettings.UseElementalBlast && ElementalBlast.IsHostileDistanceGood)
        {
            ElementalBlast.Cast();
            return;
        }
        if (Ascendance.KnownSpell && Ascendance.IsSpellUsable && ObjectManager.Me.HealthPercent < 80
            && MySettings.UseAscendance && ObjectManager.Target.GetDistance <= 40f)
        {
            Ascendance.Cast();
            return;
        }
        if (FireElementalTotem.KnownSpell && FireElementalTotem.IsSpellUsable
            && MySettings.UseFireElementalTotem && ObjectManager.Target.GetDistance <= 40f)
        {
            FireElementalTotem.Cast();
            return;
        }
        if (StormlashTotem.KnownSpell && AirTotemReady()
            && MySettings.UseStormlashTotem && ObjectManager.Target.GetDistance <= 40f)
        {
            if (!StormlashTotem.IsSpellUsable && MySettings.UseCalloftheElements
                && CalloftheElements.KnownSpell && CalloftheElements.IsSpellUsable)
            {
                CalloftheElements.Cast();
                Others.SafeSleep(200);
            }

            if (StormlashTotem.IsSpellUsable)
                StormlashTotem.Cast();
            return;
        }
        if (Bloodlust.KnownSpell && Bloodlust.IsSpellUsable && MySettings.UseBloodlustHeroism
            && ObjectManager.Target.GetDistance <= 40f && !ObjectManager.Me.HaveBuff(57724))
        {
            Bloodlust.Cast();
            return;
        }
        if (Heroism.KnownSpell && Heroism.IsSpellUsable && MySettings.UseBloodlustHeroism
            && ObjectManager.Target.GetDistance <= 40f && !ObjectManager.Me.HaveBuff(57723))
        {
            Heroism.Cast();
            return;
        }
        if (ElementalMastery.KnownSpell && ElementalMastery.IsSpellUsable
            && !ObjectManager.Me.HaveBuff(2825) && MySettings.UseElementalMastery
            && !ObjectManager.Me.HaveBuff(32182))
        {
            ElementalMastery.Cast();
        }
    }

    private void DPSCycle()
    {
        Usefuls.SleepGlobalCooldown();
        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            if (PrimalStrike.KnownSpell && PrimalStrike.IsSpellUsable && PrimalStrike.IsHostileDistanceGood
                && MySettings.UsePrimalStrike && ObjectManager.Me.Level < 11)
            {
                PrimalStrike.Cast();
                return;
            }

            if (EarthElementalTotem.KnownSpell && EarthElementalTotem.IsSpellUsable
                && ObjectManager.GetNumberAttackPlayer() > 3 && MySettings.UseEarthElementalTotem)
            {
                EarthElementalTotem.Cast();
                return;
            }
            if (FlameShock.IsSpellUsable && FlameShock.IsHostileDistanceGood && FlameShock.KnownSpell
                && MySettings.UseFlameShock && (!FlameShock.TargetHaveBuff || _flameShockTimer.IsReady))
            {
                FlameShock.Cast();
                _flameShockTimer = new Timer(1000*27);
                return;
            }
            if (LavaBurst.KnownSpell && LavaBurst.IsSpellUsable && LavaBurst.IsHostileDistanceGood
                && MySettings.UseLavaBurst && FlameShock.TargetHaveBuff)
            {
                LavaBurst.Cast();
                return;
            }
            if (EarthShock.IsSpellUsable && EarthShock.KnownSpell && EarthShock.IsHostileDistanceGood
                && MySettings.UseEarthShock && FlameShock.TargetHaveBuff)
            {
                EarthShock.Cast();
                return;
            }
            if (ObjectManager.GetNumberAttackPlayer() > 1 && MagmaTotem.KnownSpell
                && MagmaTotem.IsSpellUsable && MySettings.UseMagmaTotem
                && !FireElementalTotem.CreatedBySpell)
            {
                MagmaTotem.Cast();
                return;
            }
            if (SearingTotem.KnownSpell && SearingTotem.IsSpellUsable && MySettings.UseSearingTotem
                && FireTotemReady() && !SearingTotem.CreatedBySpellInRange(25) && ObjectManager.Target.GetDistance < 31)
            {
                SearingTotem.Cast();
                return;
            }
            if (ObjectManager.GetNumberAttackPlayer() > 1 && ChainLightning.KnownSpell
                && ChainLightning.IsSpellUsable && ChainLightning.IsHostileDistanceGood
                && MySettings.UseChainLightning && !ObjectManager.Me.HaveBuff(77762))
            {
                if (AncestralSwiftness.KnownSpell && AncestralSwiftness.IsSpellUsable
                    && MySettings.UseAncestralSwiftness)
                {
                    AncestralSwiftness.Cast();
                    Others.SafeSleep(200);
                }
                ChainLightning.Cast();
                return;
            }
            if (LightningBolt.IsHostileDistanceGood && LightningBolt.KnownSpell && LightningBolt.IsSpellUsable
                && MySettings.UseLightningBolt && !ObjectManager.Me.HaveBuff(77762))
            {
                if (AncestralSwiftness.KnownSpell && AncestralSwiftness.IsSpellUsable
                    && MySettings.UseAncestralSwiftness)
                {
                    AncestralSwiftness.Cast();
                    Others.SafeSleep(200);
                }
                LightningBolt.Cast();
            }
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    private bool FireTotemReady()
    {
        return !FireElementalTotem.CreatedBySpell && !MagmaTotem.CreatedBySpell;
    }

    private bool EarthTotemReady()
    {
        return !EarthbindTotem.CreatedBySpell && !EarthElementalTotem.CreatedBySpell && !StoneBulwarkTotem.CreatedBySpell;
    }

    private bool WaterTotemReady()
    {
        return !HealingStreamTotem.CreatedBySpell && !HealingTideTotem.CreatedBySpell && !ManaTideTotem.CreatedBySpell;
    }

    private bool AirTotemReady()
    {
        return !CapacitorTotem.CreatedBySpell && !GroundingTotem.CreatedBySpell && !StormlashTotem.CreatedBySpell && !SpiritLinkTotem.CreatedBySpell;
    }

    private bool TotemicRecallReady()
    {
        if (FireElementalTotem.CreatedBySpell)
            return false;
        if (EarthElementalTotem.CreatedBySpell)
            return false;
        if (SearingTotem.CreatedBySpell)
            return true;
        if (FireTotemReady() && EarthTotemReady() && WaterTotemReady() && AirTotemReady())
            return false;
        return true;
    }

    private void Patrolling()
    {
        if (ObjectManager.Me.IsMounted) return;
        Buff();
        Heal();
    }

    #region Nested type: ShamanRestorationSettings

    [Serializable]
    public class ShamanRestorationSettings : Settings
    {
        public bool DoAvoidMelee = false;
        public int DoAvoidMeleeDistance = 0;
        public bool UseAlchFlask = true;
        public bool UseAncestralGuidance = true;
        public bool UseAncestralSwiftness = true;
        public bool UseArcaneTorrentForDecast = true;
        public int UseArcaneTorrentForDecastBelowPercentage = 100;
        public bool UseArcaneTorrentForResource = true;
        public int UseArcaneTorrentForResourceBelowPercentage = 80;
        public bool UseAscendance = true;
        public bool UseAstralShift = true;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseBloodlustHeroism = true;
        public bool UseCalloftheElements = true;
        public bool UseCapacitorTotem = true;
        public bool UseChainHeal = false;
        public bool UseChainLightning = true;
        public bool UseEarthElementalTotem = true;
        public bool UseEarthShield = true;
        public bool UseEarthShock = true;
        public bool UseEarthbindTotem = false;
        public bool UseEarthlivingWeapon = true;
        public bool UseElementalBlast = true;
        public bool UseElementalMastery = true;

        public bool UseFireElementalTotem = true;
        public bool UseFlameShock = true;
        public bool UseFlametongue = true;
        public bool UseFrostShock = false;
        public bool UseFrostbrand = false;
        public bool UseGhostWolf = true;
        public bool UseGiftoftheNaaru = true;
        public int UseGiftoftheNaaruBelowPercentage = 80;
        public bool UseGreaterHealingWave = true;
        public bool UseGroundingTotem = true;
        public bool UseHealingRain = true;
        public bool UseHealingStreamTotem = true;
        public bool UseHealingSurge = true;
        public bool UseHealingTideTotem = true;
        public bool UseHealingWave = false;
        public bool UseLavaBurst = true;

        public bool UseLightningBolt = true;
        public bool UseLightningShield = true;
        public bool UseLowCombat = true;
        public bool UseMagmaTotem = true;
        public bool UseManaTideTotem = true;
        public bool UsePrimalStrike = true;
        public bool UseRiptide = true;
        public bool UseRockbiterWeapon = false;
        public bool UseSearingTotem = true;
        public bool UseSpiritLinkTotem = true;
        public bool UseSpiritwalkersGrace = true;
        public bool UseStoneBulwarkTotem = true;
        public bool UseStoneform = true;
        public int UseStoneformBelowPercentage = 80;
        public bool UseStormlashTotem = true;
        public bool UseTotemicProjection = true;
        public bool UseTotemicRecall = true;
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;
        public bool UseUnleashElements = true;
        public bool UseWarStomp = true;
        public int UseWarStompBelowPercentage = 80;
        public bool UseWaterShield = true;
        public bool UseWaterWalking = true;
        public bool UseWindShear = true;

        public ShamanRestorationSettings()
        {
            ConfigWinForm("Shaman Restoration Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent for Interrupt", "UseArcaneTorrentForDecast", "Professions & Racials", "BelowPercentage");
            AddControlInWinForm("Use Arcane Torrent for Resource", "UseArcaneTorrentForResource", "Professions & Racials", "BelowPercentage");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");

            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            /* Shaman Buffs */
            AddControlInWinForm("Use Earth Shield", "UseEarthShield", "Shaman Buffs");
            AddControlInWinForm("Use Earthliving Weapon", "UseEarthlivingWeapon", "Shaman Buffs");
            AddControlInWinForm("Use Flametongue Weapon", "UseFlametongue", "Shaman Buffs");
            AddControlInWinForm("Use Frostbrand Weapon", "UseFrostbrand", "Shaman Buffs");
            AddControlInWinForm("Use Ghost Wolf", "UseGhostWolf", "Shaman Buffs");
            AddControlInWinForm("Use Lightning Shield", "UseLightningShield", "Shaman Buffs");
            AddControlInWinForm("Use Rockbiter Weapon", "UseRockbiterWeapon", "Shaman Buffs");
            AddControlInWinForm("Use Spiritwalker's Grace", "UseSpiritwalkersGrace", "Shaman Buffs");
            AddControlInWinForm("Use Water Shield", "UseWaterShield", "Shaman Buffs");
            AddControlInWinForm("Use Water Walking", "UseWaterWalking", "Shaman Buffs");
            /* Offensive Spells */
            AddControlInWinForm("Use Chain Lightning", "UseChainLightning", "Offensive Spells");
            AddControlInWinForm("Use Earth Shock", "UseEarthShock", "Offensive Spells");
            AddControlInWinForm("Use Flame Shock", "UseFlameShock", "Offensive Spells");
            AddControlInWinForm("Use Frost Shock", "UseFrostShock", "Offensive Spells");
            AddControlInWinForm("Use Lava Burst", "UseLavaBurst", "Offensive Spells");
            AddControlInWinForm("Use Lightning Bolt", "UseLightningBolt", "Offensive Spells");
            AddControlInWinForm("Use Magma Totem", "UseMagmaTotem", "Offensive Spells");
            AddControlInWinForm("Use Searing Totem", "UseSearingTotem", "Offensive Spells");
            /* Offensive Cooldowns */
            AddControlInWinForm("Use Ancestral Swiftness", "UseAncestralSwiftness", "Offensive Cooldowns");
            AddControlInWinForm("Use Ascendance", "UseAscendance", "Offensive Cooldowns");
            AddControlInWinForm("Use Bloodlust / Heroism", "UseBloodlustHeroism", "Offensive Cooldowns");
            AddControlInWinForm("Use Call of the Elements", "UseCalloftheElements", "Offensive Cooldowns");
            AddControlInWinForm("Use Earth Elemental Totem", "UseEarthElementalTotem", "Offensive Cooldowns");
            AddControlInWinForm("Use Elemental Blast", "UseElementalBlast", "Offensive Cooldowns");
            AddControlInWinForm("Use Elemental Mastery", "UseElementalMastery", "Offensive Cooldowns");
            AddControlInWinForm("Use Fire Elemental Totem", "UseFireElementalTotem", "Offensive Cooldowns");
            AddControlInWinForm("Use Stormlash Totem", "UseStormlashTotem", "Offensive Cooldowns");
            AddControlInWinForm("Use Totemic Projection", "UseTotemicProjection", "Offensive Cooldowns");
            AddControlInWinForm("Use Unleash Elements", "UseUnleashElements", "Offensive Cooldowns");
            /* Defensive Cooldowns */
            AddControlInWinForm("Use Astral Shift", "UseAstralShift", "Defensive Cooldowns");
            AddControlInWinForm("Use Capacitor Totem", "UseCapacitorTotem", "Defensive Cooldowns");
            AddControlInWinForm("Use Earthbind Totem", "UseEarthbindTotem", "Defensive Cooldowns");
            AddControlInWinForm("Use Grounding Totem", "UseGroundingTotem", "Defensive Cooldowns");
            AddControlInWinForm("Use StoneBulwark Totem", "UseStoneBulwarkTotem", "Defensive Cooldowns");
            AddControlInWinForm("Use Wind Shear", "UseWindShear", "Defensive Cooldowns");
            /* Healing Spells */
            AddControlInWinForm("Use Ancestral Guidance", "UseAncestralGuidance", "Healing Spells");
            AddControlInWinForm("Use Chain Heal", "UseChainHeal", "Healing Spells");
            AddControlInWinForm("Use Greater Healing Wave", "UseGreaterHealingWave", "Healing Spells");
            AddControlInWinForm("Use Healing Rain", "UseHealingRain", "Healing Spells");
            AddControlInWinForm("Use Healing Surge", "UseHealingSurge", "Healing Spells");
            AddControlInWinForm("Use Healing Stream Totem", "UseHealingStreamTotem", "Healing Spells");
            AddControlInWinForm("Use Healing Tide Totem", "UsHealingTideTotem", "Healing Spells");
            AddControlInWinForm("Use Healing Wave", "UseHealingWave", "Healing Spells");
            AddControlInWinForm("Use Mana Tide Totem", "UseManaTideTotem", "Healing Spells");
            AddControlInWinForm("Use Riptide", "UseRiptide", "Healing Spells");
            AddControlInWinForm("Use Spirit Link Totem", "UseSpiritLinkTotem", "Healing Spells");
            AddControlInWinForm("Use Totemic Recall", "UseTotemicRecall", "Healing Spells");
            /* Game Settings */
            AddControlInWinForm("Use Low Combat Settings", "UseLowCombat", "Game Settings");
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");

            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
            AddControlInWinForm("Do avoid melee (Off Advised!!)", "DoAvoidMelee", "Game Settings");
            AddControlInWinForm("Avoid melee distance (1 to 4)", "DoAvoidMeleeDistance", "Game Settings");
        }

        public static ShamanRestorationSettings CurrentSetting { get; set; }

        public static ShamanRestorationSettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Shaman_Restoration.xml";
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

public class ShamanElemental
{
    private static ShamanElementalSettings MySettings = ShamanElementalSettings.GetSettings();

    #region General Timers & Variables

    private readonly WoWItem _firstTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET);
    private readonly WoWItem _secondTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET, 2);

    private bool CombatMode = true;

    private Timer StunCD = new Timer(0);

    #endregion

    #region Talents

    public readonly Spell PrimalElementalist = new Spell("Primal Elementalist");

    #endregion

    #region Professions & Racials

    //public readonly Spell ArcaneTorrent = new Spell("Arcane Torrent"); //No GCD
    //public readonly Spell Berserking = new Spell("Berserking");
    public readonly Spell BloodFury = new Spell("Blood Fury"); //No GCD
    //public readonly Spell GiftoftheNaaru = new Spell("Gift of the Naaru"); //No GCD
    public readonly Spell Stoneform = new Spell("Stoneform"); //No GCD
    public readonly Spell WarStomp = new Spell("War Stomp");

    #endregion

    #region Shaman Buffs

    public readonly Spell GhostWolf = new Spell("Ghost Wolf");

    #endregion

    #region Offensive Spells

    public readonly Spell ChainLightning = new Spell("Chain Lightning");
    public readonly Spell EarthShock = new Spell("Earth Shock");
    public readonly Spell Earthquake = new Spell("Earthquake");
    private Timer EarthquakeCooldown = new Timer(0);
    public readonly Spell FlameShock = new Spell("Flame Shock");
    public readonly Spell LavaBurst = new Spell("Lava Burst");
    public readonly Spell LightningBolt = new Spell("Lightning Bolt");
    public readonly Spell TotemMastery = new Spell("Totem Mastery");

    #endregion

    #region Offensive Cooldowns

    public readonly Spell Ascendance = new Spell("Ascendance"); //No GCD
    public readonly Spell Bloodlust = new Spell("Bloodlust"); //No GCD
    public readonly Spell ElementalMastery = new Spell("Elemental Mastery"); //No GCD
    public readonly Spell FireElemental = new Spell("Fire Elemental");
    public readonly Spell Heroism = new Spell("Heroism"); //No GCD

    #endregion

    #region Defensive Cooldowns

    public readonly Spell AstralShift = new Spell("Astral Shift"); //No GCD
    public readonly Spell LightningSurgeTotem = new Spell("Lightning Surge Totem");

    #endregion

    #region Utility Cooldowns

    public readonly Spell WindRushTotem = new Spell("Wind Rush Totem");

    #endregion

    #region Healing Spells

    public readonly Spell HealingSurge = new Spell("Healing Surge");

    #endregion

    public ShamanElemental()
    {
        Main.InternalRange = 39f;
        Main.InternalAggroRange = 39f;
        MySettings = ShamanElementalSettings.GetSettings();
        Main.DumpCurrentSettings<ShamanElementalSettings>(MySettings);
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
                            if (ObjectManager.Me.Target != lastTarget)
                            {
                                lastTarget = ObjectManager.Me.Target;
                            }

                            if (CombatClass.InSpellRange(ObjectManager.Target, 0, 40))
                                Combat();
                            else if (!ObjectManager.Me.IsCast)
                                Patrolling();
                        }
                        else if (!ObjectManager.Me.IsCast)
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

            if (!ObjectManager.Me.GetMove)
            {
                //Healing Surge
                if (ObjectManager.Me.HealthPercent <= MySettings.UseHealingSurgeBelowPercentageOutOfCombat &&
                    HealingSurge.IsSpellUsable && MySettings.UseHealingSurge)
                {
                    HealingSurge.CastOnSelf();
                    return;
                }
            }
            else
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

        //Oneshot
        if (MySettings.TryOneshot && Oneshot())
            return;

        //Combat
        if (ObjectManager.Target.HealthPercent > MySettings.BurstAbovePercentage)
            Burst(); //GCD independent
        GCDCycle(); //GCD dependent
    }

    private bool Oneshot()
    {
        Usefuls.SleepGlobalCooldown();
        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            int leveldif = (int) ObjectManager.Me.Level - (int) ObjectManager.Target.Level;

            if (ObjectManager.Target.HealthPercent == 100 && leveldif > MySettings.TryOneshotLevelDifference)
            {
                Logging.WriteFight("Target is " + leveldif + " Levels lower. Try Onehot!");

                //Multiple Targets
                if (ObjectManager.Target.GetUnitCountInRange(10f) > 1 && ChainLightning.IsHostileDistanceGood &&
                    ChainLightning.IsSpellUsable && MySettings.UseChainLightning)
                {
                    ChainLightning.Cast();
                    return true;
                }

                //Single Target
                if (EarthShock.IsHostileDistanceGood && EarthShock.IsSpellUsable && MySettings.UseEarthShock)
                {
                    EarthShock.Cast();
                    return true;
                }
                if (LavaBurst.IsSpellUsable && LavaBurst.IsHostileDistanceGood && MySettings.UseLavaBurst)
                {
                    LavaBurst.Cast();
                    return true;
                }
                if (LightningBolt.IsHostileDistanceGood && LightningBolt.IsSpellUsable && MySettings.UseLightningBolt)
                {
                    LightningBolt.Cast();
                    return true;
                }
            }
            return false;
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    private void Burst()
    {
        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            //Defensive Buffs
            if (ObjectManager.Me.HealthPercent < MySettings.UseAstralShiftBelowPercentage &&
                AstralShift.IsSpellUsable && MySettings.UseAstralShift)
            {
                AstralShift.Cast();
            }
            if (StunCD.IsReady)
            {
                if (ObjectManager.Me.HealthPercent < MySettings.UseStoneformBelowPercentage &&
                    Stoneform.IsSpellUsable && MySettings.UseStoneform)
                {
                    Stoneform.Cast();
                }
            }

            //Trinkets
            if (!ItemsManager.IsItemOnCooldown(_firstTrinket.Entry) && ItemsManager.IsItemUsable(_firstTrinket.Entry) && MySettings.UseTrinketOne)
            {
                ItemsManager.UseItem(_firstTrinket.Name);
                Logging.WriteFight("Use First Trinket Slot");
            }
            if (!ItemsManager.IsItemOnCooldown(_secondTrinket.Entry) && ItemsManager.IsItemUsable(_secondTrinket.Entry) && MySettings.UseTrinketTwo)
            {
                ItemsManager.UseItem(_secondTrinket.Name);
                Logging.WriteFight("Use Second Trinket Slot");
            }

            //Offensive Cooldowns
            if (Bloodlust.IsSpellUsable && !ObjectManager.Me.HaveBuff(57724) && MySettings.UseBloodlustHeroism)
            {
                Bloodlust.Cast();
            }
            if (Heroism.IsSpellUsable && !ObjectManager.Me.HaveBuff(57723) && MySettings.UseBloodlustHeroism)
            {
                Heroism.Cast();
            }
            if (Ascendance.IsSpellUsable && MySettings.UseAscendance)
            {
                Ascendance.Cast();
            }
            if (ElementalMastery.IsSpellUsable && MySettings.UseElementalMastery)
            {
                ElementalMastery.Cast();
            }
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    private void GCDCycle()
    {
        Usefuls.SleepGlobalCooldown();
        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            //Stuns
            if (StunCD.IsReady)
            {
                if (ObjectManager.Me.HealthPercent < MySettings.UseLightningSurgeTotemBelowPercentage &&
                    LightningSurgeTotem.IsSpellUsable && MySettings.UseLightningSurgeTotem)
                {
                    SpellManager.CastSpellByIDAndPosition(LightningSurgeTotem.Id, ObjectManager.Target.Position);
                    StunCD = new Timer(1000*7);
                    return;
                }
                if (ObjectManager.Me.HealthPercent < MySettings.UseWarStompBelowPercentage &&
                    WarStomp.IsSpellUsable && MySettings.UseWarStomp)
                {
                    WarStomp.Cast();
                    StunCD = new Timer(1000*2);
                    return;
                }
            }

            //Heal
            if (ObjectManager.Me.HealthPercent < MySettings.UseHealingSurgeBelowPercentage &&
                HealingSurge.IsSpellUsable && MySettings.UseHealingSurge)
            {
                HealingSurge.CastOnSelf();
                return;
            }

            //Totem Mastery
            if ((!ObjectManager.Me.HaveBuff(210659) || !ObjectManager.Me.HaveBuff(210652) ||
                 !ObjectManager.Me.HaveBuff(202192) || !ObjectManager.Me.HaveBuff(210658)) &&
                MySettings.UseTotemMastery)
            {
                TotemMastery.Cast();
                return;
            }

            //Single Target Rotation
            if (!FlameShock.TargetHaveBuffFromMe && FlameShock.IsHostileDistanceGood &&
                /* ObjectManager.Target.HealthPercent >= 5 && */ FlameShock.IsSpellUsable &&
                MySettings.UseFlameShock)
            {
                FlameShock.Cast();
                return;
            }
            if (FireElemental.IsSpellUsable && ObjectManager.Target.HealthPercent > MySettings.BurstAbovePercentage &&
                MySettings.UseFireElemental)
            {
                FireElemental.Cast();
                return;
            }
            if (ObjectManager.Me.Maelstrom == ObjectManager.Me.MaxMaelstrom && EarthShock.IsHostileDistanceGood &&
                EarthShock.IsSpellUsable && MySettings.UseEarthShock)
            {
                EarthShock.Cast();
                return;
            }
            if (LavaBurst.IsSpellUsable && LavaBurst.IsHostileDistanceGood && MySettings.UseLavaBurst)
            {
                LavaBurst.Cast();
                return;
            }
            if (ObjectManager.Target.GetUnitCountInRange(8f) > 2 && Earthquake.IsHostileDistanceGood &&
                EarthquakeCooldown.IsReady && MySettings.UseEarthquake)
            {
                Earthquake.Cast();
                EarthquakeCooldown = new Timer(1000*10);
                return;
            }
            if (ObjectManager.Me.Maelstrom > 90 && EarthShock.IsHostileDistanceGood &&
                EarthShock.IsSpellUsable && MySettings.UseEarthShock)
            {
                EarthShock.Cast();
                return;
            }
            if (ObjectManager.Target.GetUnitCountInRange(8f) > 1 && ChainLightning.IsHostileDistanceGood &&
                ChainLightning.IsSpellUsable && MySettings.UseChainLightning)
            {
                ChainLightning.Cast();
                return;
            }
            if (LightningBolt.IsHostileDistanceGood && LightningBolt.IsSpellUsable && MySettings.UseLightningBolt)
            {
                LightningBolt.Cast();
                return;
            }
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    #region Nested type: ShamanElementalSettings

    [Serializable]
    public class ShamanElementalSettings : Settings
    {
        /* Professions & Racials */
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseStoneform = true;
        public int UseStoneformBelowPercentage = 80;
        public bool UseWarStomp = true;
        public int UseWarStompBelowPercentage = 80;
        /* Shaman Buffs */
        public bool UseGhostWolf = true;
        /* Offensive Spells */
        public bool UseChainLightning = true;
        public bool UseEarthShock = true;
        public bool UseEarthquake = true;
        public bool UseElementalBlast = true;
        public bool UseFlameShock = true;
        public bool UseLavaBurst = true;
        public bool UseLightningBolt = true;
        /* Offensive Cooldowns */
        public bool UseAscendance = true;
        public bool UseBloodlustHeroism = false;
        public bool UseElementalMastery = true;
        public bool UseFireElemental = true;
        public bool UseTotemMastery = true;
        /* Defensive Cooldowns */
        public bool UseAstralShift = true;
        public int UseAstralShiftBelowPercentage = 80;
        public bool UseLightningSurgeTotem = true;
        public int UseLightningSurgeTotemBelowPercentage = 80;
        /* Utility Cooldowns */
        public bool UseWindRushTotem = true;
        /* Healing Spells */
        public bool UseHealingSurge = true;
        public int UseHealingSurgeBelowPercentage = 50;
        public int UseHealingSurgeBelowPercentageOutOfCombat = 90;
        /* Game Settings */
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;
        public int BurstAbovePercentage = 10;
        public bool TryOneshot = true;
        public int TryOneshotLevelDifference = 20;

        public ShamanElementalSettings()
        {
            ConfigWinForm("Shaman Elemental Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use Stoneform", "UseStoneformBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStompBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            /* Shaman Buffs */
            AddControlInWinForm("Use Ghost Wolf", "UseGhostWolf", "Shaman Buffs");
            /* Offensive Spells */
            AddControlInWinForm("Use Chain Lightning", "UseChainLightning", "Offensive Spells");
            AddControlInWinForm("Use Earth Shock", "UseEarthShock", "Offensive Spells");
            AddControlInWinForm("Use Earthquake", "UseEarthquake", "Offensive Spells");
            AddControlInWinForm("Use Elemental Blast", "UseElementalBlast", "Offensive Spells");
            AddControlInWinForm("Use Flame Shock", "UseFlameShock", "Offensive Spells");
            AddControlInWinForm("Use Lava Burst", "UseLavaBurst", "Offensive Spells");
            AddControlInWinForm("Use Lightning Bolt", "UseLightningBolt", "Offensive Spells");
            /* Offensive Cooldowns */
            AddControlInWinForm("Use Ascendance", "UseAscendance", "Offensive Cooldowns");
            AddControlInWinForm("Use Bloodlust / Heroism", "UseBloodlustHeroism", "Offensive Cooldowns");
            AddControlInWinForm("Use Elemental Mastery", "UseElementalMastery", "Offensive Cooldowns");
            AddControlInWinForm("Use Fire Elemental", "UseFireElemental", "Offensive Cooldowns");
            AddControlInWinForm("Use Totem Mastery", "UseTotemMastery", "Offensive Cooldowns");
            /* Defensive Cooldowns */
            AddControlInWinForm("Use Astral", "UseAstralShift", "Defensive Cooldowns");
            AddControlInWinForm("Use Astral Shift", "UseAstralShiftBelowPercentage", "Defensive Cooldowns", "BelowPercentage", "Life");
            AddControlInWinForm("Use Lightning Surge Totem", "UseLightningSurgeTotem", "Defensive Cooldowns");
            AddControlInWinForm("Use Lightning Surge Totem", "UseLightningSurgeTotemBelowPercentage", "Defensive Cooldowns", "BelowPercentage", "Life");
            /* Utility Cooldowns */
            AddControlInWinForm("Use Wind Rush Totem", "UseWindRushTotem", "Utility Cooldowns");
            /* Healing Spells */
            AddControlInWinForm("Use Healing Surge", "UseHealingSurge", "Healing Spells");
            AddControlInWinForm("Use Healing Surge while In Fight", "UseHealingSurgeBelowPercentage", "Healing Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Healing Surge while Out Of Fight", "UseHealingSurgeBelowPercentageOutOfCombat", "Healing Spells", "BelowPercentage", "Life");
            /* Game Settings */
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");
            AddControlInWinForm("Burst if Target", "BurstAbovePercentage", "Game Settings", "AbovePercentage", "Life");
            AddControlInWinForm("Try Oneshotting Enemies", "TryOneshot", "Game Settings");
            AddControlInWinForm("Try Oneshotting Enemies if Level Difference is above", "TryOneshotLevelDifference", "Game Settings"); //TODO add AbovePercentage alternative
        }

        public static ShamanElementalSettings CurrentSetting { get; set; }

        public static ShamanElementalSettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Shaman_Elemental.xml";
            if (File.Exists(currentSettingsFile))
            {
                return
                    CurrentSetting = Load<ShamanElementalSettings>(currentSettingsFile);
            }
            return new ShamanElementalSettings();
        }
    }

    #endregion
}

#endregion

// ReSharper restore ObjectCreationAsStatement
// ReSharper restore EmptyGeneralCatchClause