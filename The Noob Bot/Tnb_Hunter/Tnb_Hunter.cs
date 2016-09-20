/*
* CombatClass for TheNoobBot
* Credit : Vesper, Neo2003, Dreadlocks, Ryuichiro
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
                    #region Hunter Specialisation checking

                case WoWClass.Hunter:

                    if (wowSpecialization == WoWSpecialization.HunterMarksmanship)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Hunter_Marksmanship.xml";
                            var currentSetting = new HunterMarksmanship.HunterMarksmanshipSettings();
                            if (File.Exists(currentSettingsFile) && !resetSettings)
                            {
                                currentSetting = Settings.Load<HunterMarksmanship.HunterMarksmanshipSettings>(currentSettingsFile);
                            }
                            currentSetting.ToForm();
                            currentSetting.Save(currentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Hunter Marksmanship Combat class...");
                            InternalRange = 40.0f;
                            EquipmentAndStats.SetPlayerSpe(WoWSpecialization.HunterMarksmanship);
                            new HunterMarksmanship();
                        }
                        break;
                    }
                    if (wowSpecialization == WoWSpecialization.HunterSurvival)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Hunter_Survival.xml";
                            var currentSetting = new HunterSurvival.HunterSurvivalSettings();
                            if (File.Exists(currentSettingsFile) && !resetSettings)
                            {
                                currentSetting = Settings.Load<HunterSurvival.HunterSurvivalSettings>(currentSettingsFile);
                            }
                            currentSetting.ToForm();
                            currentSetting.Save(currentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Hunter Survival Combat class...");
                            InternalRange = 40.0f;
                            EquipmentAndStats.SetPlayerSpe(WoWSpecialization.HunterSurvival);
                            new HunterSurvival();
                        }
                        break;
                    }
                    if (wowSpecialization == WoWSpecialization.HunterBeastMastery || wowSpecialization == WoWSpecialization.None)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Hunter_BeastMastery.xml";
                            var currentSetting = new HunterBeastMastery.HunterBeastMasterySettings();
                            if (File.Exists(currentSettingsFile) && !resetSettings)
                            {
                                currentSetting = Settings.Load<HunterBeastMastery.HunterBeastMasterySettings>(currentSettingsFile);
                            }
                            currentSetting.ToForm();
                            currentSetting.Save(currentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Hunter BeastMastery Combat class...");
                            InternalRange = 40.0f;
                            EquipmentAndStats.SetPlayerSpe(WoWSpecialization.HunterBeastMastery);
                            new HunterBeastMastery();
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

#region Hunter

public class HunterMarksmanship
{
    private static HunterMarksmanshipSettings MySettings = HunterMarksmanshipSettings.GetSettings();

    #region General Timers & Variables

    private readonly WoWItem _firstTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET);
    private readonly WoWItem _secondTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET, 2);

    private bool CombatMode = true;

    private Timer CCTimer = new Timer(0);

    #endregion

    #region Talents

    private readonly Spell LoneWolf = new Spell("Lone Wolf");
    private readonly Spell NarrowEscape = new Spell("Narrow Escape");
    private readonly Spell Posthaste = new Spell("Posthaste");
    private readonly Spell SteadyFocus = new Spell("Steady Focus");

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

    #region Hunter Dots

    private readonly Spell HuntersMark = new Spell("Hunter's Mark");
    private readonly Spell Vulnerable = new Spell("Vulnerable");

    #endregion

    #region Hunter Pets

    private readonly Spell CallPet1 = new Spell("Call Pet 1");
    private readonly Spell CallPet2 = new Spell("Call Pet 2");
    private readonly Spell CallPet3 = new Spell("Call Pet 3");
    private readonly Spell CallPet4 = new Spell("Call Pet 4");
    private readonly Spell CallPet5 = new Spell("Call Pet 5");
    private readonly Spell Dismiss = new Spell("Dismiss Pet");
    private readonly Spell FeedPet = new Spell("Feed Pet");
    private readonly Spell MendPet = new Spell("Mend Pet");
    private readonly Spell RevivePet = new Spell("Revive Pet");

    #endregion

    #region Offensive Spell

    private readonly Spell AimedShot = new Spell("Aimed Shot");
    private readonly Spell ArcaneShot = new Spell("Arcane Shot");
    private readonly Spell MarkedShot = new Spell("Marked Shot");
    private readonly Spell MultiShot = new Spell("Multi-Shot");

    #endregion

    #region Legion Artifact

    private readonly Spell Windburst = new Spell("Windburst"); //No GCD

    #endregion

    #region Offensive Cooldown

    private readonly Spell AMurderofCrows = new Spell("A Murder of Crows");
    private readonly Spell Barrage = new Spell("Barrage");
    //private readonly Spell BlackArrow = new Spell("Black Arrow");
    //private readonly Spell ExplosiveShot = new Spell("Explosive Shot");
    private readonly Spell PiercingShot = new Spell("Piercing Shot");
    private readonly Spell Sidewinders = new Spell("Sidewinders");
    //private readonly Spell Sentinel = new Spell("Sentinel");
    private readonly Spell Trueshot = new Spell("Trueshot");
    //private readonly Spell Volley = new Spell("Volley");

    #endregion

    #region Defensive Cooldown

    private readonly Spell AspectoftheTurtle = new Spell("Aspect of the Turtle"); //No GCD
    private readonly Spell BindingShot = new Spell("Binding Shot");
    private readonly Spell BurstingShot = new Spell("Bursting Shot");
    //private readonly Spell Camouflage = new Spell("Camouflage"); //No GCD
    private readonly Spell ConcussiveShot = new Spell("Concussive Shot");
    //private readonly Spell CounterShot = new Spell("Counter Shot"); //No GCD
    private readonly Spell Disengage = new Spell("Disengage"); //No GCD
    private readonly Spell FeignDeath = new Spell("Feign Death"); //No GCD
    private readonly Spell Misdirection = new Spell("Misdirection"); //No GCD

    #endregion

    #region Healing Spell

    private readonly Spell Exhilaration = new Spell("Exhilaration");
    //private readonly Spell WyvernSting = new Spell("Wyvern Sting");

    #endregion

    #region Utility Spells

    private readonly Spell AspectoftheCheetah = new Spell("Aspect of the Cheetah"); //No GCD
    //private readonly Spell Flare = new Spell("Flare");

    #endregion

    public HunterMarksmanship()
    {
        Main.InternalRange = 39f;
        Main.InternalAggroRange = 39f;
        Main.InternalLightHealingSpell = null;
        MySettings = HunterMarksmanshipSettings.GetSettings();
        Main.DumpCurrentSettings<HunterMarksmanshipSettings>(MySettings);
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

                            if (ArcaneShot.IsHostileDistanceGood)
                                Combat();
                            else
                                Patrolling();
                        }
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
            Thread.Sleep(100);
        }
    }

    private void Patrolling()
    {
        if (!ObjectManager.Me.IsCast)
        {
            //Log
            if (CombatMode)
            {
                Logging.WriteFight("Patrolling:");
                CombatMode = false;
            }

            if (ObjectManager.Me.GetMove)
            {
                if (!Darkflight.HaveBuff && !AspectoftheCheetah.HaveBuff)
                {
                    if (MySettings.UseDarkflight && Darkflight.IsSpellUsable)
                    {
                        Darkflight.Cast();
                    }
                    else if (MySettings.UseAspectoftheCheetah && AspectoftheCheetah.IsSpellUsable)
                    {
                        AspectoftheCheetah.Cast();
                    }
                }
            }
            else
            {
                Pet();
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
        if (!ObjectManager.Me.IsCast)
        {
            Heal();
            if (Defensive()) return;
            Pet();
            BurstBuffs();
            GCDCycle();
        }
    }

    private void Heal()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            //Gift of the Naaru
            if (ObjectManager.Me.HealthPercent < MySettings.UseGiftoftheNaaruBelowPercentage && GiftoftheNaaru.IsSpellUsable)
            {
                GiftoftheNaaru.Cast();
                return;
            }
            //Exhilaration
            if (ObjectManager.Me.HealthPercent < MySettings.UseExhilarationBelowPercentage && Exhilaration.IsSpellUsable)
            {
                Exhilaration.Cast();
                return;
            }
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    private bool Defensive()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            //Defensive Cooldowns
            if (CCTimer.IsReady)
            {
                if (ObjectManager.Target.IsStunnable)
                {
                    if (WarStomp.IsSpellUsable && ObjectManager.Me.HealthPercent < MySettings.UseWarStompBelowPercentage)
                    {
                        WarStomp.Cast();
                        CCTimer = new Timer(1000*2);
                    }
                }
                if (Stoneform.IsSpellUsable && ObjectManager.Me.HealthPercent < MySettings.UseStoneformBelowPercentage)
                {
                    Stoneform.Cast();
                }
                if (AspectoftheTurtle.IsSpellUsable && ObjectManager.Me.HealthPercent < MySettings.UseAspectoftheTurtleBelowPercentage)
                {
                    AspectoftheTurtle.Cast();
                }
                if (Misdirection.IsSpellUsable && ObjectManager.Me.HealthPercent < MySettings.UseMisdirectionBelowPercentage &&
                    ObjectManager.Pet.IsAlive)
                {
                    Misdirection.CastOnUnitID("pet");
                }
                if (BurstingShot.IsSpellUsable && ObjectManager.Me.HealthPercent < MySettings.UseBurstingShotBelowPercentage && BurstingShot.IsHostileDistanceGood)
                {
                    BurstingShot.Cast();
                    CCTimer = new Timer(1000*4);
                    return true;
                }
                if (ConcussiveShot.IsSpellUsable && ObjectManager.Me.HealthPercent < MySettings.UseConcussiveShotBelowPercentage && ConcussiveShot.IsHostileDistanceGood)
                {
                    ConcussiveShot.Cast();
                    CCTimer = new Timer(1000*6);
                    return true;
                }
                if (Disengage.IsSpellUsable && ObjectManager.Me.HealthPercent < MySettings.UseDisengageBelowPercentage)
                {
                    MovementsAction.Jump();
                    Disengage.Cast();
                    if (NarrowEscape.HaveBuff || Posthaste.HaveBuff)
                        CCTimer = new Timer(1000*8);
                    return true;
                }
                if (BindingShot.IsSpellUsable && ObjectManager.Me.HealthPercent < MySettings.UseBindingShotBelowPercentage && BindingShot.IsHostileDistanceGood)
                {
                    BindingShot.Cast();
                    CCTimer = new Timer(1000*5);
                    return true;
                }
                if (FeignDeath.IsSpellUsable && ObjectManager.Me.HealthPercent < MySettings.UseFeignDeathBelowPercentage)
                {
                    FeignDeath.Cast();
                    Others.SafeSleep(5000);
                    if (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0 || !ObjectManager.Pet.IsValid)
                        return true;
                    Others.SafeSleep(5000);
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

    private void DismissPet()
    {
        if (MySettings.DismissOnCall && Dismiss.IsSpellUsable)
        {
            Dismiss.Cast();
            Others.SafeSleep(1500);
        }
    }

    private void Pet()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            if (!ObjectManager.Me.IsCast && !LoneWolf.HaveBuff)
            {
                if (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0 || !ObjectManager.Pet.IsValid)
                {
                    //Call Pet //TODO implement a check which one is active
                    switch (MySettings.UsePet)
                    {
                        case 1:
                            if (CallPet1.IsSpellUsable)
                            {
                                DismissPet();
                                CallPet1.Cast();
                                Others.SafeSleep(1000 + Usefuls.Latency);
                            }
                            break;

                        case 2:
                            if (CallPet2.IsSpellUsable)
                            {
                                DismissPet();
                                CallPet2.Cast();
                                Others.SafeSleep(1000 + Usefuls.Latency);
                            }
                            break;

                        case 3:
                            if (CallPet3.IsSpellUsable)
                            {
                                DismissPet();
                                CallPet3.Cast();
                                Others.SafeSleep(1000 + Usefuls.Latency);
                            }
                            break;

                        case 4:
                            if (CallPet4.IsSpellUsable)
                            {
                                DismissPet();
                                CallPet4.Cast();
                                Others.SafeSleep(1000 + Usefuls.Latency);
                            }
                            break;

                        case 5:
                            if (CallPet5.IsSpellUsable)
                            {
                                DismissPet();
                                CallPet5.Cast();
                                Others.SafeSleep(1000 + Usefuls.Latency);
                            }
                            break;

                        default:
                            break;
                    }

                    //Revive Pet
                    if (MySettings.UseRevivePet && RevivePet.IsSpellUsable && ObjectManager.Pet.IsValid)
                    {
                        if (MySettings.UseCombatRevive || !ObjectManager.Me.InCombat)
                        {
                            RevivePet.Cast();
                            return;
                        }
                    }
                }
                else
                {
                    if (ObjectManager.Pet.Health > 0)
                    {
                        if (!ObjectManager.Me.InCombat && ObjectManager.Pet.HealthPercent < MySettings.UseFeedPetBelowPercentage && FeedPet.IsSpellUsable)
                        {
                            FeedPet.Cast();
                            return;
                        }
                        if (ObjectManager.Pet.HealthPercent < MySettings.UseMendPetBelowPercentage && MendPet.IsSpellUsable)
                        {
                            MendPet.Cast();
                            return;
                        }
                    }
                }
            }
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    private void BurstBuffs()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            //Manage Pet Targeting
            //if (ObjectManager.Pet.IsAlive && ObjectManager.Pet.Target != ObjectManager.Me.Target)
            //{
            //    Lua.RunMacroText("/petattack");
            //    Logging.WriteFight("Cast Pet Attack");
            //}

            //Burst Buffs
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
            if (MySettings.UseTrueshot && Trueshot.IsSpellUsable)
            {
                Trueshot.Cast();
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

            //Procs
            if (MySettings.UseMarkedShot && MarkedShot.IsSpellUsable && MarkedShot.IsHostileDistanceGood)
            {
                MarkedShot.Cast();
                return;
            }

            //Cooldowns
            if (MySettings.UseBarrage && Barrage.IsSpellUsable && Barrage.IsHostileDistanceGood)
            {
                Barrage.Cast();
                return;
            }
            if (MySettings.UseWindburst && Windburst.IsSpellUsable && Windburst.IsHostileDistanceGood)
            {
                Windburst.Cast();
                return;
            }
            if (MySettings.UseAMurderofCrows && AMurderofCrows.IsSpellUsable && AMurderofCrows.IsHostileDistanceGood && !AMurderofCrows.TargetHaveBuff)
            {
                AMurderofCrows.Cast();
                return;
            }

            //Spend Focus
            if (MySettings.UseAimedShot && AimedShot.IsSpellUsable && AimedShot.IsHostileDistanceGood &&
                ObjectManager.Target.UnitAura(Vulnerable.Ids, ObjectManager.Me.Guid).AuraTimeLeftInMs > 2000)
            {
                AimedShot.Cast();
                return;
            }

            //Generate Focus
            if (MySettings.UseSidewinders && Sidewinders.IsSpellUsable && Sidewinders.IsHostileDistanceGood &&
                (HuntersMark.TargetHaveBuff || Sidewinders.GetSpellCharges == 2))
            {
                Sidewinders.Cast();
                return;
            }
            if (ObjectManager.Target.GetUnitInSpellRange(5f) > 1)
            {
                if (MySettings.UseMultiShot && MultiShot.IsSpellUsable && MultiShot.IsHostileDistanceGood)
                {
                    if (SteadyFocus.HaveBuff)
                    {
                        MultiShot.Cast();
                        Others.SafeSleep(1500 + Usefuls.Latency);
                        MultiShot.Cast();
                        Others.SafeSleep(1500 + Usefuls.Latency);
                    }
                    MultiShot.Cast();
                    return;
                }
            }
            else
            {
                if (MySettings.UseArcaneShot && ArcaneShot.IsSpellUsable && ArcaneShot.IsHostileDistanceGood)
                {
                    if (SteadyFocus.HaveBuff)
                    {
                        ArcaneShot.Cast();
                        Others.SafeSleep(1500 + Usefuls.Latency);
                        ArcaneShot.Cast();
                        Others.SafeSleep(1500 + Usefuls.Latency);
                    }
                    ArcaneShot.Cast();
                    return;
                }
            }
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    #region Nested type: HunterMarksmanshipSettings

    [Serializable]
    public class HunterMarksmanshipSettings : Settings
    {
        /* Professions & Racials */
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseDarkflight = true;
        public int UseGiftoftheNaaruBelowPercentage = 50;
        public int UseStoneformBelowPercentage = 50;
        public int UseWarStompBelowPercentage = 50;

        /* Pet */
        public bool DismissOnCall = true;
        public bool UseCombatRevive = true;
        public int UseFeedPetBelowPercentage = 0;
        public int UseMendPetBelowPercentage = 50;
        public int UsePet = 1;
        public bool UseRevivePet = true;

        /* Offensive Spells */
        public bool UseAimedShot = true;
        public bool UseArcaneShot = true;
        public bool UseMarkedShot = true;
        public bool UseMultiShot = true;

        /* Artifact Spells */
        public bool UseWindburst = true;

        /* Offensive Spells */
        public bool UseAMurderofCrows = true;
        public bool UseBarrage = true;
        public bool UseSidewinders = true;
        public bool UseTrueshot = true;

        /* Defensive Cooldowns */
        public int UseAspectoftheTurtleBelowPercentage = 35;
        public int UseBindingShotBelowPercentage = 75;
        public int UseBurstingShotBelowPercentage = 75;
        public int UseDisengageBelowPercentage = 75;
        public int UseConcussiveShotBelowPercentage = 75;
        public int UseFeignDeathBelowPercentage = 10;
        public int UseMisdirectionBelowPercentage = 75;

        /* Healing Spells */
        public int UseExhilarationBelowPercentage = 25;

        /* Utility Spells */
        public bool UseAspectoftheCheetah = true;

        /* Game Settings */
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;

        public HunterMarksmanshipSettings()
        {
            ConfigWinForm("Hunter Marksmanship Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Darkflight", "UseDarkflight", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaruBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            AddControlInWinForm("Use Stone Form", "UseStoneformBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            AddControlInWinForm("Use War Stomp", "UseWarStompBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            /* Pet */
            AddControlInWinForm("Dismiss pet before calling again", "DismissOnCall", "Pet");
            AddControlInWinForm("Use Pet (1,2,3,4,5)", "UsePet", "Pet");
            AddControlInWinForm("Use Pet in Slot 1", "UsePet1", "Pet");
            AddControlInWinForm("Use Pet in Slot 2", "UsePet2", "Pet");
            AddControlInWinForm("Use Pet in Slot 3", "UsePet3", "Pet");
            AddControlInWinForm("Use Pet in Slot 4", "UsePet4", "Pet");
            AddControlInWinForm("Use Pet in Slot 5", "UsePet5", "Pet");
            //AddControlInWinForm("Use Feed Pet", "UseFeedPetBelowPercentage", "Pet", "BelowPercentage", "Life");
            AddControlInWinForm("Use Mend Pet", "UseMendPetBelowPercentage", "Pet", "BelowPercentage", "Life");
            AddControlInWinForm("Use Revive Pet", "UseRevivePet", "Pet");
            /* Offensive Spell */
            AddControlInWinForm("Use Aimed Shot", "UseAimedShot", "Offensive Spell");
            AddControlInWinForm("Use Arcane Shot", "UseArcaneShot", "Offensive Spell");
            AddControlInWinForm("Use Marked Shot", "UseMarkedShot", "Offensive Spell");
            AddControlInWinForm("Use Multi-Shot", "UseMultiShot", "Offensive Spell");
            /* Artifact Spells */
            AddControlInWinForm("Use Windburst", "UseWindburst", "Artifact Spells");
            /* Offensive Cooldown */
            AddControlInWinForm("Use A Murder of Crows", "UseAMurderofCrows", "Offensive Cooldown");
            AddControlInWinForm("Use Barrage", "UseBarrage", "Offensive Cooldown");
            AddControlInWinForm("Use Sidewinders", "UseSidewinders", "Offensive Cooldown");
            AddControlInWinForm("Use Trueshot", "UseTrueshot", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Aspect of the Turtle", "UseAspectoftheTurtleBelowPercentage", "Defensive Cooldowns", "BelowPercentage", "Life");
            AddControlInWinForm("Use Binding Shot", "UseBindingShotBelowPercentage", "Defensive Cooldowns", "BelowPercentage", "Life");
            AddControlInWinForm("Use Bursting Shot", "UseBurstingShotBelowPercentage", "Defensive Cooldowns", "BelowPercentage", "Life");
            AddControlInWinForm("Use Disengage", "UseDisengageBelowPercentage", "Defensive Cooldowns", "BelowPercentage", "Life");
            AddControlInWinForm("Use Concussive Shot", "UseConcussiveShotBelowPercentage", "Defensive Cooldowns", "BelowPercentage", "Life");
            AddControlInWinForm("Use Feign Death", "UseFeignDeathBelowPercentage", "Defensive Cooldowns", "BelowPercentage", "Life");
            AddControlInWinForm("Use Misdirection", "UseMisdirectionBelowPercentage", "Defensive Cooldowns", "BelowPercentage", "Life");
            /* Healing Spell */
            AddControlInWinForm("Use Exhilaration", "UseExhilarationBelowPercentage", "Healing Spell", "BelowPercentage", "Life");
            /* Utility Spells */
            AddControlInWinForm("Use Aspect of the Cheetah", "UseAspectoftheCheetah", "Utility Spells");
            /* Game Settings */
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");
        }

        public static HunterMarksmanshipSettings CurrentSetting { get; set; }

        public static HunterMarksmanshipSettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Hunter_Marksmanship.xml";
            if (File.Exists(currentSettingsFile))
            {
                return
                    CurrentSetting = Load<HunterMarksmanshipSettings>(currentSettingsFile);
            }
            return new HunterMarksmanshipSettings();
        }
    }

    #endregion
}

public class HunterBeastMastery
{
    private static HunterBeastMasterySettings MySettings = HunterBeastMasterySettings.GetSettings();

    #region General Timers & Variables

    private readonly WoWItem _firstTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET);
    private readonly WoWItem _secondTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET, 2);

    private bool CombatMode = true;

    private Timer CCTimer = new Timer(0);

    #endregion

    #region Talents

    private readonly Spell BeastCleave = new Spell("Beast Cleave");
    private readonly Spell NarrowEscape = new Spell("Narrow Escape");
    private readonly Spell Posthaste = new Spell("Posthaste");

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

    #region Hunter Pets

    private readonly Spell CallPet1 = new Spell("Call Pet 1");
    private readonly Spell CallPet2 = new Spell("Call Pet 2");
    private readonly Spell CallPet3 = new Spell("Call Pet 3");
    private readonly Spell CallPet4 = new Spell("Call Pet 4");
    private readonly Spell CallPet5 = new Spell("Call Pet 5");
    private readonly Spell Dismiss = new Spell("Dismiss Pet");
    private readonly Spell FeedPet = new Spell("Feed Pet");
    private readonly Spell MendPet = new Spell("Mend Pet");
    private readonly Spell RevivePet = new Spell("Revive Pet");

    #endregion

    #region Offensive Spell

    private readonly Spell CobraShot = new Spell("Cobra Shot");
    private readonly Spell MultiShot = new Spell("Multi-Shot");

    #endregion

    #region Legion Artifact

    private readonly Spell TitansThunder = new Spell("Titan's Thunder"); //No GCD

    #endregion

    #region Offensive Cooldown

    private readonly Spell AMurderofCrows = new Spell("A Murder of Crows");
    private readonly Spell AspectoftheWild = new Spell("Aspect of the Wild"); //No GCD
    private readonly Spell Barrage = new Spell("Barrage");
    //private readonly Spell BlinkStrike = new Spell("Blink Strike");
    //private Timer BlinkStrikeCD = new Timer(0);
    private readonly Spell BestialWrath = new Spell("Bestial Wrath"); //No GCD
    private readonly Spell ChimeraShot = new Spell("Chimaera Shot");
    private readonly Spell DireBeast = new Spell("Dire Beast");
    private Timer DireBeastTimer = new Timer(0);
    private readonly Spell DireFrenzy = new Spell("Dire Frenzy");
    private readonly Spell KillCommand = new Spell("Kill Command");
    private readonly Spell Stampede = new Spell("Stampede"); //No GCD
    //private readonly Spell Volley = new Spell("Volley");

    #endregion

    #region Defensive Cooldown

    private readonly Spell AspectoftheTurtle = new Spell("Aspect of the Turtle"); //No GCD
    private readonly Spell BindingShot = new Spell("Binding Shot");
    private readonly Spell ConcussiveShot = new Spell("Concussive Shot");
    //private readonly Spell CounterShot = new Spell("Counter Shot"); //No GCD
    private readonly Spell Disengage = new Spell("Disengage"); //No GCD
    private readonly Spell Intimidation = new Spell("Intimidation");
    private readonly Spell FeignDeath = new Spell("Feign Death"); //No GCD
    private readonly Spell Misdirection = new Spell("Misdirection"); //No GCD

    #endregion

    #region Healing Spell

    private readonly Spell Exhilaration = new Spell("Exhilaration");

    #endregion

    #region Utility Spells

    private readonly Spell AspectoftheCheetah = new Spell("Aspect of the Cheetah"); //No GCD
    //private readonly Spell Flare = new Spell("Flare");
    //private readonly Spell WyvernSting = new Spell("Wyvern Sting");

    #endregion

    public HunterBeastMastery()
    {
        Main.InternalRange = 39f;
        Main.InternalAggroRange = 39f;
        Main.InternalLightHealingSpell = null;
        MySettings = HunterBeastMasterySettings.GetSettings();
        Main.DumpCurrentSettings<HunterBeastMasterySettings>(MySettings);
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

                            if (CobraShot.IsHostileDistanceGood)
                                Combat();
                            else
                                Patrolling();
                        }
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
            Thread.Sleep(100);
        }
    }

    private void Patrolling()
    {
        if (!ObjectManager.Me.IsCast)
        {
            //Log
            if (CombatMode)
            {
                Logging.WriteFight("Patrolling:");
                CombatMode = false;
            }

            if (ObjectManager.Me.GetMove)
            {
                if (!Darkflight.HaveBuff && !AspectoftheCheetah.HaveBuff)
                {
                    if (MySettings.UseDarkflight && Darkflight.IsSpellUsable)
                    {
                        Darkflight.Cast();
                    }
                    else if (MySettings.UseAspectoftheCheetah && AspectoftheCheetah.IsSpellUsable)
                    {
                        AspectoftheCheetah.Cast();
                    }
                }
            }
            else
            {
                Pet();
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
        if (!ObjectManager.Me.IsCast)
        {
            Heal();
            if (Defensive()) return;
            Pet();
            BurstBuffs();
            GCDCycle();
        }
    }

    private void Heal()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            //Gift of the Naaru
            if (ObjectManager.Me.HealthPercent < MySettings.UseGiftoftheNaaruBelowPercentage && GiftoftheNaaru.IsSpellUsable)
            {
                GiftoftheNaaru.Cast();
                return;
            }
            //Exhilaration
            if (ObjectManager.Me.HealthPercent < MySettings.UseExhilarationBelowPercentage && Exhilaration.IsSpellUsable)
            {
                Exhilaration.Cast();
                return;
            }
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    private bool Defensive()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            //Defensive Cooldowns
            if (CCTimer.IsReady)
            {
                if (ObjectManager.Target.IsStunnable)
                {
                    if (WarStomp.IsSpellUsable && ObjectManager.Me.HealthPercent < MySettings.UseWarStompBelowPercentage)
                    {
                        WarStomp.Cast();
                        CCTimer = new Timer(1000*2);
                    }
                    if (Intimidation.IsSpellUsable && ObjectManager.Me.HealthPercent < MySettings.UseIntimidationBelowPercentage)
                    {
                        Intimidation.Cast();
                        CCTimer = new Timer(1000*5);
                    }
                }
                if (Stoneform.IsSpellUsable && ObjectManager.Me.HealthPercent < MySettings.UseStoneformBelowPercentage)
                {
                    Stoneform.Cast();
                }
                if (AspectoftheTurtle.IsSpellUsable && ObjectManager.Me.HealthPercent < MySettings.UseAspectoftheTurtleBelowPercentage)
                {
                    AspectoftheTurtle.Cast();
                }
                if (Misdirection.IsSpellUsable && ObjectManager.Me.HealthPercent < MySettings.UseMisdirectionBelowPercentage &&
                    ObjectManager.Pet.IsAlive)
                {
                    Misdirection.CastOnUnitID("pet");
                }
                if (ConcussiveShot.IsSpellUsable && ObjectManager.Me.HealthPercent < MySettings.UseConcussiveShotBelowPercentage && ConcussiveShot.IsHostileDistanceGood)
                {
                    ConcussiveShot.Cast();
                    CCTimer = new Timer(1000*6);
                    return true;
                }
                if (Disengage.IsSpellUsable && ObjectManager.Me.HealthPercent < MySettings.UseDisengageBelowPercentage)
                {
                    MovementsAction.Jump();
                    Disengage.Cast();
                    if (NarrowEscape.HaveBuff || Posthaste.HaveBuff)
                        CCTimer = new Timer(1000*8);
                    return true;
                }
                if (BindingShot.IsSpellUsable && ObjectManager.Me.HealthPercent < MySettings.UseBindingShotBelowPercentage && BindingShot.IsHostileDistanceGood)
                {
                    BindingShot.Cast();
                    CCTimer = new Timer(1000*5);
                    return true;
                }
                if (FeignDeath.IsSpellUsable && ObjectManager.Me.HealthPercent < MySettings.UseFeignDeathBelowPercentage)
                {
                    FeignDeath.Cast();
                    Others.SafeSleep(5000);
                    if (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0 || !ObjectManager.Pet.IsValid)
                        return true;
                    Others.SafeSleep(5000);
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

    private void DismissPet()
    {
        if (MySettings.DismissOnCall && Dismiss.IsSpellUsable)
        {
            Dismiss.Cast();
            Others.SafeSleep(1500);
        }
    }

    private void Pet()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            if (!ObjectManager.Me.IsCast)
            {
                if (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0 || !ObjectManager.Pet.IsValid)
                {
                    //Call Pet //TODO implement a check which one is active
                    switch (MySettings.UsePet)
                    {
                        case 1:
                            if (CallPet1.IsSpellUsable)
                            {
                                DismissPet();
                                CallPet1.Cast();
                                Others.SafeSleep(1000 + Usefuls.Latency);
                            }
                            break;

                        case 2:
                            if (CallPet2.IsSpellUsable)
                            {
                                DismissPet();
                                CallPet2.Cast();
                                Others.SafeSleep(1000 + Usefuls.Latency);
                            }
                            break;

                        case 3:
                            if (CallPet3.IsSpellUsable)
                            {
                                DismissPet();
                                CallPet3.Cast();
                                Others.SafeSleep(1000 + Usefuls.Latency);
                            }
                            break;

                        case 4:
                            if (CallPet4.IsSpellUsable)
                            {
                                DismissPet();
                                CallPet4.Cast();
                                Others.SafeSleep(1000 + Usefuls.Latency);
                            }
                            break;

                        case 5:
                            if (CallPet5.IsSpellUsable)
                            {
                                DismissPet();
                                CallPet5.Cast();
                                Others.SafeSleep(1000 + Usefuls.Latency);
                            }
                            break;

                        default:
                            break;
                    }

                    //Revive Pet
                    if (MySettings.UseRevivePet && RevivePet.IsSpellUsable && ObjectManager.Pet.IsValid)
                    {
                        if (MySettings.UseCombatRevive || !ObjectManager.Me.InCombat)
                        {
                            RevivePet.Cast();
                            return;
                        }
                    }
                }
                else
                {
                    if (ObjectManager.Pet.Health > 0)
                    {
                        if (!ObjectManager.Me.InCombat && ObjectManager.Pet.HealthPercent < MySettings.UseFeedPetBelowPercentage && FeedPet.IsSpellUsable)
                        {
                            FeedPet.Cast();
                            return;
                        }
                        if (ObjectManager.Pet.HealthPercent < MySettings.UseMendPetBelowPercentage && MendPet.IsSpellUsable)
                        {
                            MendPet.Cast();
                            return;
                        }
                    }
                }
            }
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    private void BurstBuffs()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            //Manage Pet Targeting
            //if (ObjectManager.Pet.IsAlive && ObjectManager.Pet.Target != ObjectManager.Me.Target)
            //{
            //    Lua.RunMacroText("/petattack");
            //    Logging.WriteFight("Cast Pet Attack");
            //}

            //Burst Buffs
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
            if (MySettings.UseBestialWrath && BestialWrath.IsSpellUsable)
            {
                BestialWrath.Cast();
            }
            if (MySettings.UseAspectoftheWild && AspectoftheWild.IsSpellUsable)
            {
                AspectoftheWild.Cast();
            }
            if (MySettings.UseTitansThunder && TitansThunder.IsSpellUsable && (!DireBeastTimer.IsReady || ObjectManager.Pet.HaveBuff(DireFrenzy.Ids)))
            {
                TitansThunder.Cast();
            }
            if (MySettings.UseStampede && Stampede.IsSpellUsable && Stampede.IsHostileDistanceGood && BestialWrath.HaveBuff)
            {
                Stampede.Cast();
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

            //AOEs
            if (ObjectManager.Target.GetUnitInSpellRange(5f) > 1)
            {
                if (MySettings.UseMultiShot && MultiShot.IsSpellUsable && MultiShot.IsHostileDistanceGood && !ObjectManager.Pet.HaveBuff(BeastCleave.Ids))
                {
                    MultiShot.Cast();
                    return;
                }
                if (MySettings.UseBarrage && Barrage.IsSpellUsable && Barrage.IsHostileDistanceGood)
                {
                    Barrage.Cast();
                    return;
                }
            }

            //Cooldowns
            if (MySettings.UseAMurderofCrows && AMurderofCrows.IsSpellUsable && AMurderofCrows.IsHostileDistanceGood && !AMurderofCrows.TargetHaveBuff)
            {
                AMurderofCrows.Cast();
                return;
            }
            if (MySettings.UseKillCommand && KillCommand.IsSpellUsable && ObjectManager.Pet.Position.DistanceTo(ObjectManager.Target.Position) <= 25)
            {
                KillCommand.Cast();
                return;
            }
            if (MySettings.UseDireBeast && DireBeast.IsSpellUsable && DireBeast.IsHostileDistanceGood)
            {
                DireBeast.Cast();
                DireBeastTimer = new Timer(1000*8);
                return;
            }
            else if (MySettings.UseDireFrenzy && DireFrenzy.IsSpellUsable && DireFrenzy.IsHostileDistanceGood)
            {
                DireFrenzy.Cast();
                return;
            }
            if (MySettings.UseChimeraShot && ChimeraShot.IsSpellUsable && ChimeraShot.IsHostileDistanceGood)
            {
                ChimeraShot.Cast();
                return;
            }
            if (MySettings.UseBarrage && Barrage.IsSpellUsable && Barrage.IsHostileDistanceGood)
            {
                Barrage.Cast();
                return;
            }

            //Generate Focus
            if (ObjectManager.Me.Focus > MySettings.UseCobraShotAboveValue && CobraShot.IsSpellUsable &&
                CobraShot.IsHostileDistanceGood)
            {
                CobraShot.Cast();
                return;
            }
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    #region Nested type: HunterBeastMasterySettings

    [Serializable]
    public class HunterBeastMasterySettings : Settings
    {
        /* Professions & Racials */
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseDarkflight = true;
        public int UseGiftoftheNaaruBelowPercentage = 50;
        public int UseStoneformBelowPercentage = 50;
        public int UseWarStompBelowPercentage = 50;

        /* Pet */
        public bool DismissOnCall = true;
        public bool UseCombatRevive = true;
        public int UseFeedPetBelowPercentage = 0;
        public int UseMendPetBelowPercentage = 50;
        public int UsePet = 1;
        public bool UseRevivePet = true;

        /* Offensive Spells */
        public int UseCobraShotAboveValue = 90;
        public bool UseMultiShot = true;

        /* Artifact Spells */
        public bool UseTitansThunder = true;

        /* Offensive Spells */
        public bool UseAMurderofCrows = true;
        public bool UseAspectoftheWild = true;
        public bool UseBarrage = true;
        public bool UseBestialWrath = true;
        public bool UseChimeraShot = true;
        public bool UseDireBeast = true;
        public bool UseDireFrenzy = true;
        public bool UseKillCommand = true;
        public bool UseStampede = true;

        /* Defensive Cooldowns */
        public int UseAspectoftheTurtleBelowPercentage = 35;
        public int UseBindingShotBelowPercentage = 50;
        public int UseConcussiveShotBelowPercentage = 50;
        public int UseDisengageBelowPercentage = 50;
        public int UseIntimidationBelowPercentage = 50;
        public int UseFeignDeathBelowPercentage = 10;
        public int UseMisdirectionBelowPercentage = 75;

        /* Healing Spells */
        public int UseExhilarationBelowPercentage = 25;

        /* Utility Spells */
        public bool UseAspectoftheCheetah = true;

        /* Game Settings */
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;

        public HunterBeastMasterySettings()
        {
            ConfigWinForm("Hunter BeastMastery Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Darkflight", "UseDarkflight", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaruBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            AddControlInWinForm("Use Stone Form", "UseStoneformBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            AddControlInWinForm("Use War Stomp", "UseWarStompBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            /* Pet */
            AddControlInWinForm("Dismiss pet before calling again", "DismissOnCall", "Pet");
            AddControlInWinForm("Use Pet (1,2,3,4,5)", "UsePet", "Pet");
            AddControlInWinForm("Use Pet in Slot 1", "UsePet1", "Pet");
            AddControlInWinForm("Use Pet in Slot 2", "UsePet2", "Pet");
            AddControlInWinForm("Use Pet in Slot 3", "UsePet3", "Pet");
            AddControlInWinForm("Use Pet in Slot 4", "UsePet4", "Pet");
            AddControlInWinForm("Use Pet in Slot 5", "UsePet5", "Pet");
            //AddControlInWinForm("Use Feed Pet", "UseFeedPetBelowPercentage", "Pet", "BelowPercentage", "Life");
            AddControlInWinForm("Use Mend Pet", "UseMendPetBelowPercentage", "Pet", "BelowPercentage", "Life");
            AddControlInWinForm("Use Revive Pet", "UseRevivePet", "Pet");
            /* Offensive Spell */
            AddControlInWinForm("Use Cobra Shot", "UseCobraShotAboveValue", "Offensive Spell", "AbovePercentage", "Focus"); //TODO add AbovePercentage alternative
            AddControlInWinForm("Use Multi-Shot", "UseMultiShot", "Offensive Spell");
            /* Artifact Spells */
            AddControlInWinForm("Use Titan's Thunder", "UseTitansThunder", "Artifact Spells");
            /* Offensive Cooldown */
            AddControlInWinForm("Use A Murder of Crows", "UseAMurderofCrows", "Offensive Cooldown");
            AddControlInWinForm("Use Aspect of the Wild", "UseAspectoftheWild", "Offensive Cooldown");
            AddControlInWinForm("Use Barrage", "UseBarrage", "Offensive Cooldown");
            AddControlInWinForm("Use Bestial Wrath", "UseBestialWrath", "Offensive Cooldown");
            AddControlInWinForm("Use Chimera Shot", "UseChimeraShot", "Offensive Cooldown");
            AddControlInWinForm("Use Dire Beast", "UseDireBeast", "Offensive Cooldown");
            AddControlInWinForm("Use Dire Frenzy", "UseDireFrenzy", "Offensive Cooldown");
            AddControlInWinForm("Use Kill Command", "UseKillCommand", "Offensive Cooldown");
            AddControlInWinForm("Use Stampede", "UseStampede", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Aspect of the Turtle", "UseAspectoftheTurtleBelowPercentage", "Defensive Cooldowns", "BelowPercentage", "Life");
            AddControlInWinForm("Use Binding Shot", "UseBindingShotBelowPercentage", "Defensive Cooldowns", "BelowPercentage", "Life");
            AddControlInWinForm("Use Concussive Shot", "UseConcussiveShotBelowPercentage", "Defensive Cooldowns", "BelowPercentage", "Life");
            AddControlInWinForm("Use Disengage", "UseDisengageBelowPercentage", "Defensive Cooldowns", "BelowPercentage", "Life");
            AddControlInWinForm("Use Intimidation", "UseIntimidationBelowPercentage", "Defensive Cooldowns", "BelowPercentage", "Life");
            AddControlInWinForm("Use Feign Death", "UseFeignDeathBelowPercentage", "Defensive Cooldowns", "BelowPercentage", "Life");
            AddControlInWinForm("Use Misdirection", "UseMisdirectionBelowPercentage", "Defensive Cooldowns", "BelowPercentage", "Life");
            /* Healing Spell */
            AddControlInWinForm("Use Exhilaration", "UseExhilarationBelowPercentage", "Healing Spell", "BelowPercentage", "Life");
            /* Utility Spells */
            AddControlInWinForm("Use Aspect of the Cheetah", "UseAspectoftheCheetah", "Utility Spells");
            /* Game Settings */
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");
        }

        public static HunterBeastMasterySettings CurrentSetting { get; set; }

        public static HunterBeastMasterySettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Hunter_BeastMastery.xml";
            if (File.Exists(currentSettingsFile))
            {
                return
                    CurrentSetting = Load<HunterBeastMasterySettings>(currentSettingsFile);
            }
            return new HunterBeastMasterySettings();
        }
    }

    #endregion
}

public class HunterSurvival
{
    private static HunterSurvivalSettings MySettings = HunterSurvivalSettings.GetSettings();

    #region General Timers & Variables

    private readonly WoWItem _firstTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET);
    private readonly WoWItem _secondTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET, 2);

    private bool CombatMode = true;
    private uint StackMoongooseFury = 0;

    private Timer CCTimer = new Timer(0);

    #endregion

    #region Talents

    private readonly Spell WayoftheMokNathal = new Spell("Way of the Mok'Nathal");

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

    #region Hunter Pets

    private readonly Spell CallPet1 = new Spell("Call Pet 1");
    private readonly Spell CallPet2 = new Spell("Call Pet 2");
    private readonly Spell CallPet3 = new Spell("Call Pet 3");
    private readonly Spell CallPet4 = new Spell("Call Pet 4");
    private readonly Spell CallPet5 = new Spell("Call Pet 5");
    private readonly Spell Dismiss = new Spell("Dismiss Pet");
    private readonly Spell FeedPet = new Spell("Feed Pet");
    private readonly Spell MendPet = new Spell("Mend Pet");
    private readonly Spell RevivePet = new Spell("Revive Pet");

    #endregion

    #region Hunter Buffs

    private readonly Spell MokNathalTactics = new Spell("Mok'Nathal Tactics");

    #endregion

    #region Hunter Dots

    private readonly Spell MongooseFury = new Spell("Mongoose Fury");

    #endregion

    #region Offensive Spell

    //private readonly Spell Carve = new Spell("Carve");
    private readonly Spell RaptorStrike = new Spell("Raptor Strike");

    #endregion

    #region Legion Artifact

    private readonly Spell FuryoftheEagle = new Spell("Fury of the Eagle"); //No GCD

    #endregion

    #region Offensive Cooldown

    private readonly Spell AMurderofCrows = new Spell("A Murder of Crows");
    private readonly Spell AspectoftheEagle = new Spell("Aspect of the Eagle"); //No GCD
    //private readonly Spell Butchery = new Spell("Butchery");
    private readonly Spell DragonsfireGrenade = new Spell("Dragonsfire Grenade");
    private readonly Spell ExplosiveTrap = new Spell("Explosive Trap");
    private readonly Spell FlankingStrike = new Spell("Flanking Strike");
    private readonly Spell Harpoon = new Spell("Harpoon");
    private readonly Spell Lacerate = new Spell("Lacerate");
    private readonly Spell MongooseBite = new Spell("Mongoose Bite");
    private readonly Spell SnakeHunter = new Spell("Snake Hunter");
    private readonly Spell SpittingCobra = new Spell("Spitting Cobra");
    private readonly Spell ThrowingAxes = new Spell("Throwing Axes");

    #endregion

    #region Defensive Cooldown

    private readonly Spell AspectoftheTurtle = new Spell("Aspect of the Turtle"); //No GCD
    private readonly Spell FeignDeath = new Spell("Feign Death"); //No GCD
    //private readonly Spell Muzzle = new Spell("Muzzle");

    #endregion

    #region Healing Spell

    private readonly Spell Exhilaration = new Spell("Exhilaration");

    #endregion

    #region Utility Spells

    private readonly Spell AspectoftheCheetah = new Spell("Aspect of the Cheetah"); //No GCD
    //private readonly Spell CalTrops = new Spell("CalTrops");
    //private readonly Spell Flare = new Spell("Flare");
    //private readonly Spell FreezingTrap = new Spell("Freezing Trap");
    //private readonly Spell RangersNet = new Spell("Ranger's Net");
    //private readonly Spell SteelTrap = new Spell("Steel Trap");
    //private readonly Spell StickyBomb = new Spell("Sticky Bomb");
    //private readonly Spell TarTrap = new Spell("Tar Trap");
    //private readonly Spell WingClip = new Spell("Wing Clip");

    #endregion

    public HunterSurvival()
    {
        Main.InternalRange = 39f;
        Main.InternalAggroRange = 39f;
        Main.InternalLightHealingSpell = null;
        MySettings = HunterSurvivalSettings.GetSettings();
        Main.DumpCurrentSettings<HunterSurvivalSettings>(MySettings);
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
                            else
                                Patrolling();
                        }
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
            Thread.Sleep(100);
        }
    }

    private void Patrolling()
    {
        if (!ObjectManager.Me.IsCast)
        {
            //Log
            if (CombatMode)
            {
                Logging.WriteFight("Patrolling:");
                CombatMode = false;
            }

            if (ObjectManager.Me.GetMove)
            {
                if (!Darkflight.HaveBuff && !AspectoftheCheetah.HaveBuff)
                {
                    if (MySettings.UseDarkflight && Darkflight.IsSpellUsable)
                    {
                        Darkflight.Cast();
                    }
                    else if (MySettings.UseAspectoftheCheetah && AspectoftheCheetah.IsSpellUsable)
                    {
                        AspectoftheCheetah.Cast();
                    }
                }
            }
            else
            {
                Pet();
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
        if (!ObjectManager.Me.IsCast)
        {
            Heal();
            if (Defensive()) return;
            Pet();
            BurstBuffs();
            GCDCycle();
        }
    }

    private void Heal()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            //Gift of the Naaru
            if (ObjectManager.Me.HealthPercent < MySettings.UseGiftoftheNaaruBelowPercentage && GiftoftheNaaru.IsSpellUsable)
            {
                GiftoftheNaaru.Cast();
                return;
            }
            //Exhilaration
            if (ObjectManager.Me.HealthPercent < MySettings.UseExhilarationBelowPercentage && Exhilaration.IsSpellUsable)
            {
                Exhilaration.Cast();
                return;
            }
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    private bool Defensive()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            //Defensive Cooldowns
            if (CCTimer.IsReady)
            {
                if (ObjectManager.Target.IsStunnable)
                {
                    if (WarStomp.IsSpellUsable && ObjectManager.Me.HealthPercent < MySettings.UseWarStompBelowPercentage)
                    {
                        WarStomp.Cast();
                        CCTimer = new Timer(1000*2);
                    }
                }
                if (Stoneform.IsSpellUsable && ObjectManager.Me.HealthPercent < MySettings.UseStoneformBelowPercentage)
                {
                    Stoneform.Cast();
                }
                if (AspectoftheTurtle.IsSpellUsable && ObjectManager.Me.HealthPercent < MySettings.UseAspectoftheTurtleBelowPercentage)
                {
                    AspectoftheTurtle.Cast();
                }
                if (FeignDeath.IsSpellUsable && ObjectManager.Me.HealthPercent < MySettings.UseFeignDeathBelowPercentage)
                {
                    FeignDeath.Cast();
                    Others.SafeSleep(5000);
                    if (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0 || !ObjectManager.Pet.IsValid)
                        return true;
                    Others.SafeSleep(5000);
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

    private void DismissPet()
    {
        if (MySettings.DismissOnCall && Dismiss.IsSpellUsable)
        {
            Dismiss.Cast();
            Others.SafeSleep(1500);
        }
    }

    private void Pet()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            if (!ObjectManager.Me.IsCast)
            {
                if (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0 || !ObjectManager.Pet.IsValid)
                {
                    //Call Pet //TODO implement a check which one is active
                    switch (MySettings.UsePet)
                    {
                        case 1:
                            if (CallPet1.IsSpellUsable)
                            {
                                DismissPet();
                                CallPet1.Cast();
                                Others.SafeSleep(1000 + Usefuls.Latency);
                            }
                            break;

                        case 2:
                            if (CallPet2.IsSpellUsable)
                            {
                                DismissPet();
                                CallPet2.Cast();
                                Others.SafeSleep(1000 + Usefuls.Latency);
                            }
                            break;

                        case 3:
                            if (CallPet3.IsSpellUsable)
                            {
                                DismissPet();
                                CallPet3.Cast();
                                Others.SafeSleep(1000 + Usefuls.Latency);
                            }
                            break;

                        case 4:
                            if (CallPet4.IsSpellUsable)
                            {
                                DismissPet();
                                CallPet4.Cast();
                                Others.SafeSleep(1000 + Usefuls.Latency);
                            }
                            break;

                        case 5:
                            if (CallPet5.IsSpellUsable)
                            {
                                DismissPet();
                                CallPet5.Cast();
                                Others.SafeSleep(1000 + Usefuls.Latency);
                            }
                            break;

                        default:
                            break;
                    }

                    //Revive Pet
                    if (MySettings.UseRevivePet && RevivePet.IsSpellUsable && ObjectManager.Pet.IsValid)
                    {
                        if (MySettings.UseCombatRevive || !ObjectManager.Me.InCombat)
                        {
                            RevivePet.Cast();
                            return;
                        }
                    }
                }
                else
                {
                    if (ObjectManager.Pet.Health > 0)
                    {
                        if (!ObjectManager.Me.InCombat && ObjectManager.Pet.HealthPercent < MySettings.UseFeedPetBelowPercentage && FeedPet.IsSpellUsable)
                        {
                            FeedPet.Cast();
                            return;
                        }
                        if (ObjectManager.Pet.HealthPercent < MySettings.UseMendPetBelowPercentage && MendPet.IsSpellUsable)
                        {
                            MendPet.Cast();
                            return;
                        }
                    }
                }
            }
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    private void BurstBuffs()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            //Manage Pet Targeting
            //if (ObjectManager.Pet.IsAlive && ObjectManager.Pet.Target != ObjectManager.Me.Target)
            //{
            //    Lua.RunMacroText("/petattack");
            //    Logging.WriteFight("Cast Pet Attack");
            //}

            //Burst Buffs
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

            //Mongoose Logic
            if (MySettings.UseFuryoftheEagle && FuryoftheEagle.IsSpellUsable && FuryoftheEagle.IsHostileDistanceGood &&
                MongooseFury.BuffStack == 6)
            {
                FuryoftheEagle.Cast();
                return;
            }
            if (StackMoongooseFury == 0)
            {
                if (MongooseBite.GetSpellCharges == 3)
                {
                    Logging.WriteFight("Started stacking Moongoose Fury");
                    StackMoongooseFury = 1;
                }
            }
            else
            {
                if (StackMoongooseFury == 1)
                {
                    if (MySettings.UseAspectoftheEagle && AspectoftheEagle.IsSpellUsable)
                    {
                        StackMoongooseFury = 2;
                        AspectoftheEagle.Cast();
                    }
                    else if (MySettings.UseSnakeHunter && SnakeHunter.IsSpellUsable &&
                             MongooseFury.BuffStack <= 3 && MongooseBite.GetSpellCharges == 0)
                    {
                        StackMoongooseFury = 2;
                        SnakeHunter.Cast();
                        return;
                    }
                }
                if (MySettings.UseMongooseBite && MongooseBite.GetSpellCharges > 0 && MongooseFury.BuffStack < 6)
                {
                    MongooseBite.Cast();
                    return;
                }
                if (!MongooseFury.HaveBuff)
                {
                    Logging.WriteFight("Stopped stacking Moongoose Fury");
                    StackMoongooseFury = 0;
                    return;
                }
            }


            //High Priority Cooldowns
            if (MySettings.UseExplosiveTrap && ExplosiveTrap.IsSpellUsable && ExplosiveTrap.IsHostileDistanceGood)
            {
                ExplosiveTrap.Cast();
                return;
            }
            if (MySettings.UseDragonsfireGrenade && DragonsfireGrenade.IsSpellUsable && DragonsfireGrenade.IsHostileDistanceGood)
            {
                DragonsfireGrenade.Cast();
                return;
            }
            if (MySettings.UseAMurderofCrows && AMurderofCrows.IsSpellUsable && AMurderofCrows.IsHostileDistanceGood && !AMurderofCrows.TargetHaveBuff)
            {
                AMurderofCrows.Cast();
                return;
            }

            //Maintain Mok'Nathal Tactics
            if ((ObjectManager.Me.UnitAura(MokNathalTactics.Id, ObjectManager.Me.Guid).AuraTimeLeftInMs < 1 || MokNathalTactics.BuffStack < 4) &&
                MySettings.UseFlankingStrike && FlankingStrike.IsSpellUsable && FlankingStrike.IsHostileDistanceGood)
            {
                FlankingStrike.Cast();
                return;
            }
            //Maintain Lacerate
            if (MySettings.UseLacerate && Lacerate.IsSpellUsable && Lacerate.IsHostileDistanceGood && !Lacerate.TargetHaveBuff)
            {
                Lacerate.Cast();
                return;
            }

            //Low Priority Cooldowns
            if (MySettings.UseFlankingStrike && FlankingStrike.IsSpellUsable && FlankingStrike.IsHostileDistanceGood)
            {
                FlankingStrike.Cast();
                return;
            }
            if (MySettings.UseSpittingCobra && SpittingCobra.IsSpellUsable && SpittingCobra.IsHostileDistanceGood)
            {
                SpittingCobra.Cast();
                return;
            }
            if (MySettings.UseThrowingAxes && ThrowingAxes.IsSpellUsable && ThrowingAxes.IsHostileDistanceGood)
            {
                ThrowingAxes.Cast();
                return;
            }
            if (MySettings.UseHarpoon && Harpoon.IsSpellUsable && Harpoon.IsHostileDistanceGood)
            {
                Harpoon.Cast();
                CCTimer = new Timer(1000*3);
                return;
            }
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    #region Nested type: HunterSurvivalSettings

    [Serializable]
    public class HunterSurvivalSettings : Settings
    {
        /* Professions & Racials */
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseDarkflight = true;
        public int UseGiftoftheNaaruBelowPercentage = 50;
        public int UseStoneformBelowPercentage = 50;
        public int UseWarStompBelowPercentage = 50;

        /* Pet */
        public bool DismissOnCall = true;
        public bool UseCombatRevive = true;
        public int UseFeedPetBelowPercentage = 0;
        public int UseMendPetBelowPercentage = 50;
        public int UsePet = 1;
        public bool UseRevivePet = true;

        /* Offensive Spells */
        public bool UseRaptorStrike = true;

        /* Artifact Spells */
        public bool UseFuryoftheEagle = true;

        /* Offensive Spells */
        public bool UseAMurderofCrows = true;
        public bool UseAspectoftheEagle = true;
        public bool UseDragonsfireGrenade = true;
        public bool UseExplosiveTrap = true;
        public bool UseFlankingStrike = true;
        public bool UseHarpoon = true;
        public bool UseLacerate = true;
        public bool UseMongooseBite = true;
        public bool UseSnakeHunter = true;
        public bool UseSpittingCobra = true;
        public bool UseThrowingAxes = true;

        /* Defensive Cooldowns */
        public int UseAspectoftheTurtleBelowPercentage = 35;
        public int UseFeignDeathBelowPercentage = 10;

        /* Healing Spells */
        public int UseExhilarationBelowPercentage = 25;

        /* Utility Spells */
        public bool UseAspectoftheCheetah = true;

        /* Game Settings */
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;

        public HunterSurvivalSettings()
        {
            ConfigWinForm("Hunter Survival Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Darkflight", "UseDarkflight", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaruBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            AddControlInWinForm("Use Stone Form", "UseStoneformBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            AddControlInWinForm("Use War Stomp", "UseWarStompBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            /* Pet */
            AddControlInWinForm("Dismiss pet before calling again", "DismissOnCall", "Pet");
            AddControlInWinForm("Use Pet (1,2,3,4,5)", "UsePet", "Pet");
            AddControlInWinForm("Use Pet in Slot 1", "UsePet1", "Pet");
            AddControlInWinForm("Use Pet in Slot 2", "UsePet2", "Pet");
            AddControlInWinForm("Use Pet in Slot 3", "UsePet3", "Pet");
            AddControlInWinForm("Use Pet in Slot 4", "UsePet4", "Pet");
            AddControlInWinForm("Use Pet in Slot 5", "UsePet5", "Pet");
            //AddControlInWinForm("Use Feed Pet", "UseFeedPetBelowPercentage", "Pet", "BelowPercentage", "Life");
            AddControlInWinForm("Use Mend Pet", "UseMendPetBelowPercentage", "Pet", "BelowPercentage", "Life");
            AddControlInWinForm("Use Revive Pet", "UseRevivePet", "Pet");
            /* Offensive Spell */
            AddControlInWinForm("Use Raptor Strike", "UseRaptorStrike", "Offensive Spell");
            /* Artifact Spells */
            AddControlInWinForm("Use Fury of the Eagle", "UseFuryoftheEagle", "Artifact Spells");
            /* Offensive Cooldown */
            AddControlInWinForm("Use A Murder of Crows", "UseAMurderofCrows", "Offensive Cooldown");
            AddControlInWinForm("Use Aspect of the Eagle", "UseAspectoftheEagle", "Offensive Cooldown");
            AddControlInWinForm("Use Dragonsfire Grenade", "UseDragonsfireGrenade", "Offensive Cooldown");
            AddControlInWinForm("Use Explosive Trap", "UseExplosiveTrap", "Offensive Cooldown");
            AddControlInWinForm("Use Flanking Strike", "UseFlankingStrike", "Offensive Cooldown");
            AddControlInWinForm("Use Harpoon", "UseHarpoon", "Offensive Cooldown");
            AddControlInWinForm("Use Lacerate", "UseLacerate", "Offensive Cooldown");
            AddControlInWinForm("Use Mongoose Bite", "UseMongooseBite", "Offensive Cooldown");
            AddControlInWinForm("Use Snake Hunter", "UseSnakeHunter", "Offensive Cooldown");
            AddControlInWinForm("Use Spitting Cobra", "UseSpittingCobra", "Offensive Cooldown");
            AddControlInWinForm("Use Throwing Axes", "UseThrowingAxes", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Aspect of the Turtle", "UseAspectoftheTurtleBelowPercentage", "Defensive Cooldowns", "BelowPercentage", "Life");
            AddControlInWinForm("Use Feign Death", "UseFeignDeathBelowPercentage", "Defensive Cooldowns", "BelowPercentage", "Life");
            /* Healing Spell */
            AddControlInWinForm("Use Exhilaration", "UseExhilarationBelowPercentage", "Healing Spell", "BelowPercentage", "Life");
            /* Utility Spells */
            AddControlInWinForm("Use Aspect of the Cheetah", "UseAspectoftheCheetah", "Utility Spells");
            /* Game Settings */
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");
        }

        public static HunterSurvivalSettings CurrentSetting { get; set; }

        public static HunterSurvivalSettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Hunter_Survival.xml";
            if (File.Exists(currentSettingsFile))
            {
                return
                    CurrentSetting = Load<HunterSurvivalSettings>(currentSettingsFile);
            }
            return new HunterSurvivalSettings();
        }
    }

    #endregion
}

#endregion

// ReSharper restore ObjectCreationAsStatement
// ReSharper restore EmptyGeneralCatchClause