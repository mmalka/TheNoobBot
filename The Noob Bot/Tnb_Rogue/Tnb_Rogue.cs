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
                    #region Rogue Specialisation checking

                case WoWClass.Rogue:

                    if (wowSpecialization == WoWSpecialization.RogueOutlaw)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Rogue_Outlaw.xml";
                            var currentSetting = new RogueOutlaw.RogueOutlawSettings();
                            if (File.Exists(currentSettingsFile) && !resetSettings)
                            {
                                currentSetting = Settings.Load<RogueOutlaw.RogueOutlawSettings>(currentSettingsFile);
                            }
                            currentSetting.ToForm();
                            currentSetting.Save(currentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Rogue Outlaw Combat class...");
                            EquipmentAndStats.SetPlayerSpe(WoWSpecialization.RogueOutlaw);
                            new RogueOutlaw();
                        }
                        break;
                    }
                    if (wowSpecialization == WoWSpecialization.RogueAssassination || wowSpecialization == WoWSpecialization.None)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Rogue_Assassination.xml";
                            var currentSetting = new RogueAssassination.RogueAssassinationSettings();
                            if (File.Exists(currentSettingsFile) && !resetSettings)
                            {
                                currentSetting = Settings.Load<RogueAssassination.RogueAssassinationSettings>(currentSettingsFile);
                            }
                            currentSetting.ToForm();
                            currentSetting.Save(currentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Rogue Assassination Combat class...");
                            EquipmentAndStats.SetPlayerSpe(WoWSpecialization.RogueAssassination);
                            new RogueAssassination();
                        }
                        break;
                    }
                    if (wowSpecialization == WoWSpecialization.RogueSubtlety)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Rogue_Subtlety.xml";
                            var currentSetting = new RogueSubtlety.RogueSubtletySettings();
                            if (File.Exists(currentSettingsFile) && !resetSettings)
                            {
                                currentSetting = Settings.Load<RogueSubtlety.RogueSubtletySettings>(currentSettingsFile);
                            }
                            currentSetting.ToForm();
                            currentSetting.Save(currentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Rogue Subtlety Combat class...");
                            EquipmentAndStats.SetPlayerSpe(WoWSpecialization.RogueSubtlety);
                            new RogueSubtlety();
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

#region Rogue

public class RogueOutlaw
{
    private static RogueOutlawSettings MySettings = RogueOutlawSettings.GetSettings();

    #region General Timers & Variables

    private readonly WoWItem _firstTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET);
    private readonly WoWItem _secondTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET, 2);
    public uint CP = 0;
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

    #region Rogue Buffs

    public readonly Spell BladeFlurry = new Spell("Blade Flurry");
    public readonly Spell BurstofSpeed = new Spell("Burst of Speed");
    public readonly Spell CripplingPoison = new Spell("Crippling Poison");
    public readonly Spell DeadlyPoison = new Spell("Deadly Poison");
    public readonly Spell LeechingPoison = new Spell("Leeching Poison");
    public readonly Spell MindnumbingPoison = new Spell("Mind-numbing Poison");
    public readonly Spell ParalyticPoison = new Spell("Paralytic Poison");
    public readonly Spell SliceandDice = new Spell("Slice and Dice");
    public readonly Spell Sprint = new Spell("Sprint");
    public readonly Spell Stealth = new Spell("Stealth");
    public readonly Spell WoundPoison = new Spell("Wound Poison");
/*
        private Timer _sliceandDiceTimer = new Timer(0);
*/

    #endregion

    #region Offensive Spell

    public readonly Spell Ambush = new Spell("Ambush");
    public readonly Spell CrimsonTempest = new Spell("Crimson Tempest");
    public readonly Spell DeadlyThrow = new Spell("Deadly Throw");
    public readonly Spell Eviscerate = new Spell("Eviscerate");
    public readonly Spell ExposeArmor = new Spell("Expose Armor");
    public readonly Spell FanofKnives = new Spell("Fan of Knives");
    public readonly Spell Garrote = new Spell("Garrote");
    public readonly Spell RevealingStrike = new Spell("Revealing Strike");
    public readonly Spell Rupture = new Spell("Rupture");
    public readonly Spell Shiv = new Spell("Shiv");
    public readonly Spell ShurikenToss = new Spell("Shuriken Toss");
    public readonly Spell SinisterStrike = new Spell("Sinister Strike");
    public readonly Spell Throw = new Spell("Throw");
    private Timer _ruptureTimer = new Timer(0);

    #endregion

    #region Offensive Cooldown

    public readonly Spell AdrenalineRush = new Spell("Adrenaline Rush");
    public readonly Spell KillingSpree = new Spell("Killing Spree");
    public readonly Spell Redirect = new Spell("Redirect");
    public readonly Spell ShadowBlades = new Spell("Shadow Blades");
    public readonly Spell ShadowStep = new Spell("Shadow Step");
    public readonly Spell Vendetta = new Spell("Vendetta");

    #endregion

    #region Defensive Cooldown

    public readonly Spell CheapShot = new Spell("Cheap Shot");
    public readonly Spell CloakofShadows = new Spell("Cloak of Shadows");
    public readonly Spell CombatReadiness = new Spell("Combat Readiness");
    public readonly Spell Dismantle = new Spell("Dismantle");
    public readonly Spell Evasion = new Spell("Evasion");
    public readonly Spell Kick = new Spell("Kick");
    public readonly Spell KidneyShot = new Spell("Kidney Shot");
    public readonly Spell Preparation = new Spell("Preparation");
    public readonly Spell SmokeBomb = new Spell("Smoke Bomb");
    public readonly Spell Vanish = new Spell("Vanish");
/*
        private Timer _dismantleTimer = new Timer(0);
*/

    #endregion

    #region Healing Spell

    public readonly Spell Recuperate = new Spell("Recuperate");

    #endregion

    public RogueOutlaw()
    {
        Main.InternalRange = ObjectManager.Me.GetCombatReach;
        Main.InternalLightHealingSpell = null;
        MySettings = RogueOutlawSettings.GetSettings();
        Main.DumpCurrentSettings<RogueOutlawSettings>(MySettings);
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
                                && (Throw.IsHostileDistanceGood || SinisterStrike.IsHostileDistanceGood))
                            {
                                Pull();
                                lastTarget = ObjectManager.Me.Target;
                            }

                            if (ObjectManager.Target.Level < 70 && ObjectManager.Me.Level > 84
                                && MySettings.UseLowCombat)
                            {
                                LC = 1;
                                if (ObjectManager.Target.GetDistance < 30)
                                    LowCombat();
                            }
                            else
                            {
                                LC = 0;
                                if (ObjectManager.Target.GetDistance < 30 && ObjectManager.Me.Level < 10)
                                    LowCombat();
                                else if (ObjectManager.Target.GetDistance < 30)
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
        if (Redirect.IsSpellUsable && Redirect.IsHostileDistanceGood && Redirect.KnownSpell
            && MySettings.UseRedirect && ObjectManager.Me.ComboPoint > 0)
        {
            Redirect.Cast();
            Others.SafeSleep(200);
        }

        if (((Stealth.KnownSpell && Stealth.IsSpellUsable && !Stealth.HaveBuff && MySettings.UseStealth)
             || Stealth.HaveBuff) && LC != 1)
        {
            if (!Stealth.HaveBuff)
            {
                Stealth.Cast();
                Others.SafeSleep(200);
            }

            if (ShadowStep.IsSpellUsable && ShadowStep.IsHostileDistanceGood && ShadowStep.KnownSpell
                && MySettings.UseShadowStep)
            {
                ShadowStep.Cast();
                Others.SafeSleep(200);
            }

            if (Garrote.IsSpellUsable && Garrote.IsHostileDistanceGood && Garrote.KnownSpell
                && MySettings.UseGarrote)
            {
                Garrote.Cast();
                return;
            }
            if (CheapShot.IsSpellUsable && CheapShot.IsHostileDistanceGood && CheapShot.KnownSpell
                && MySettings.UseCheapShot)
            {
                CheapShot.Cast();
                return;
            }
            return;
        }
        if (ShurikenToss.IsSpellUsable && ShurikenToss.IsHostileDistanceGood && ShurikenToss.KnownSpell
            && MySettings.UseShurikenToss && !MySettings.UseStealth)
        {
            ShurikenToss.Cast();
            return;
        }
        if (Throw.IsSpellUsable && Throw.IsHostileDistanceGood && Throw.KnownSpell
            && MySettings.UseThrow && !MySettings.UseStealth)
        {
            MovementManager.StopMove();
            Throw.Cast();
            Others.SafeSleep(1000);
        }
    }

    private void LowCombat()
    {
        Buff();
        if (MySettings.DoAvoidMelee)
            AvoidMelee();
        DefenseCycle();
        Heal();

        if (Throw.IsSpellUsable && Throw.IsHostileDistanceGood && Throw.KnownSpell && !ObjectManager.Target.InCombat
            && MySettings.UseThrow)
        {
            Throw.Cast();
            return;
        }

        if (Eviscerate.KnownSpell && Eviscerate.IsSpellUsable && Eviscerate.IsHostileDistanceGood && MySettings.UseEviscerate && ObjectManager.Me.ComboPoint >= 2)
        {
            Eviscerate.Cast();
            return;
        }
        if (RevealingStrike.KnownSpell && RevealingStrike.IsSpellUsable && RevealingStrike.IsHostileDistanceGood
            && MySettings.UseRevealingStrike)
        {
            RevealingStrike.Cast();
            return;
        }
        if (SinisterStrike.KnownSpell && SinisterStrike.IsSpellUsable && SinisterStrike.IsHostileDistanceGood
            && MySettings.UseSinisterStrike)
        {
            SinisterStrike.Cast();
            return;
        }
        if (SliceandDice.KnownSpell && SliceandDice.IsSpellUsable && SliceandDice.IsHostileDistanceGood
            && MySettings.UseSliceandDice && !SliceandDice.HaveBuff)
        {
            CP = ObjectManager.Me.ComboPoint;
            SliceandDice.Cast();
            //_sliceandDiceTimer = new Timer(1000*(6 + (CP*6)));
            return;
        }
        if (FanofKnives.KnownSpell && FanofKnives.IsSpellUsable && FanofKnives.IsHostileDistanceGood
            && MySettings.UseFanofKnives)
        {
            FanofKnives.Cast();
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

        if (MySettings.UseDeadlyPoison && DeadlyPoison.KnownSpell && DeadlyPoison.IsSpellUsable
            && !DeadlyPoison.HaveBuff)
        {
            DeadlyPoison.Cast();
            return;
        }
        if (!WoundPoison.HaveBuff && WoundPoison.KnownSpell && WoundPoison.IsSpellUsable
            && MySettings.UseWoundPoison && !DeadlyPoison.HaveBuff)
        {
            WoundPoison.Cast();
            return;
        }
        if (!LeechingPoison.HaveBuff && LeechingPoison.KnownSpell && LeechingPoison.IsSpellUsable
            && MySettings.UseLeechingPoison)
        {
            LeechingPoison.Cast();
            return;
        }
        if (!ParalyticPoison.HaveBuff && ParalyticPoison.KnownSpell && ParalyticPoison.IsSpellUsable
            && MySettings.UseParalyticPoison && !LeechingPoison.HaveBuff)
        {
            ParalyticPoison.Cast();
            return;
        }
        if (!CripplingPoison.HaveBuff && CripplingPoison.KnownSpell && CripplingPoison.IsSpellUsable
            && MySettings.UseCripplingPoison && !LeechingPoison.HaveBuff && ParalyticPoison.HaveBuff)
        {
            CripplingPoison.Cast();
            return;
        }
        if (!MindnumbingPoison.HaveBuff && MindnumbingPoison.KnownSpell && MindnumbingPoison.IsSpellUsable
            && MySettings.UseMindnumbingPoison && !CripplingPoison.HaveBuff && !ParalyticPoison.HaveBuff
            && !LeechingPoison.HaveBuff)
        {
            MindnumbingPoison.Cast();
            return;
        }
        if (!ObjectManager.Me.InCombat && BurstofSpeed.IsSpellUsable && BurstofSpeed.KnownSpell
            && MySettings.UseBurstofSpeed && ObjectManager.Me.GetMove)
        {
            BurstofSpeed.Cast();
            return;
        }
        if (!ObjectManager.Me.InCombat && Sprint.IsSpellUsable && Sprint.KnownSpell
            && MySettings.UseSprint && ObjectManager.Me.GetMove)
        {
            Sprint.Cast();
            return;
        }
        if (MySettings.UseAlchFlask && !ObjectManager.Me.HaveBuff(79638) && !ObjectManager.Me.HaveBuff(79640) && !ObjectManager.Me.HaveBuff(79639) &&
            !ItemsManager.IsItemOnCooldown(75525) && ItemsManager.GetItemCount(75525) > 0)
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
        if (ObjectManager.Me.HealthPercent <= 80 && !KidneyShot.TargetHaveBuff && KidneyShot.KnownSpell
            && KidneyShot.IsSpellUsable && KidneyShot.IsHostileDistanceGood && ObjectManager.Me.ComboPoint <= 3
            && Recuperate.HaveBuff && MySettings.UseKidneyShot)
        {
            CP = ObjectManager.Me.ComboPoint;
            KidneyShot.Cast();
            _onCd = new Timer(1000*CP);
            return;
        }
        if (ObjectManager.Me.HealthPercent <= 80 && Evasion.KnownSpell && Evasion.IsSpellUsable
            && MySettings.UseEvasion)
        {
            Evasion.Cast();
            _onCd = new Timer(1000*15);
            return;
        }
        if (ObjectManager.Me.HealthPercent <= 90 && CombatReadiness.KnownSpell && CombatReadiness.IsSpellUsable
            && MySettings.UseCombatReadiness)
        {
            CombatReadiness.Cast();
            _onCd = new Timer(1000*20);
            return;
        }
        if (ObjectManager.Me.HealthPercent <= 95 && Dismantle.KnownSpell && Dismantle.IsSpellUsable
            && MySettings.UseDismantle)
        {
            Dismantle.Cast();
            //_dismantleTimer = new Timer(1000*60);
            return;
        }
        if (ObjectManager.Me.HealthPercent <= MySettings.UseWarStompAtPercentage && WarStomp.IsSpellUsable &&
            WarStomp.KnownSpell
            && MySettings.UseWarStomp)
        {
            WarStomp.Cast();
            _onCd = new Timer(1000*2);
            return;
        }
        if (ObjectManager.Me.HealthPercent <= MySettings.UseStoneformAtPercentage && Stoneform.IsSpellUsable &&
            Stoneform.KnownSpell
            && MySettings.UseStoneform)
        {
            Stoneform.Cast();
            _onCd = new Timer(1000*8);
            return;
        }
        if (ObjectManager.GetNumberAttackPlayer() >= 3 && Vanish.KnownSpell && Vanish.IsSpellUsable
            && MySettings.UseVanish)
        {
            Vanish.Cast();
            Others.SafeSleep(5000);
            return;
        }
        if (ObjectManager.Me.HealthPercent <= 70 && Preparation.KnownSpell && Preparation.IsSpellUsable
            && MySettings.UsePreparation && !Evasion.IsSpellUsable)
        {
            Preparation.Cast();
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (ObjectManager.Me.HealthPercent <= MySettings.UseGiftoftheNaaruAtPercentage && GiftoftheNaaru.KnownSpell &&
            GiftoftheNaaru.IsSpellUsable
            && MySettings.UseGiftoftheNaaru)
        {
            GiftoftheNaaru.Cast();
            return;
        }
        if (!Recuperate.HaveBuff && ObjectManager.Me.ComboPoint > 1 && MySettings.UseRecuperate
            && ObjectManager.Me.HealthPercent <= 90 && Recuperate.KnownSpell && Recuperate.IsSpellUsable)
        {
            Recuperate.Cast();
        }
    }

    private void Decast()
    {
        if (ObjectManager.Target.IsCast && Kick.KnownSpell && Kick.IsSpellUsable
            && Kick.IsHostileDistanceGood && MySettings.UseKick && ObjectManager.Target.IsTargetingMe)
        {
            Kick.Cast();
            return;
        }
        if (ArcaneTorrent.IsSpellUsable && ArcaneTorrent.KnownSpell && ObjectManager.Target.GetDistance < 8
            && ObjectManager.Me.HealthPercent <= MySettings.UseArcaneTorrentForDecastAtPercentage
            && MySettings.UseArcaneTorrentForDecast && ObjectManager.Target.IsCast &&
            ObjectManager.Target.IsTargetingMe)
        {
            ArcaneTorrent.Cast();
            return;
        }
        if (ObjectManager.Target.IsCast && CloakofShadows.KnownSpell && CloakofShadows.IsSpellUsable
            && ObjectManager.Target.IsTargetingMe && MySettings.UseCloakofShadows)
        {
            CloakofShadows.Cast();
            return;
        }
        if (ObjectManager.Target.IsCast && SmokeBomb.KnownSpell && SmokeBomb.IsSpellUsable
            && ObjectManager.Target.IsTargetingMe && MySettings.UseSmokeBomb
            && !CloakofShadows.HaveBuff)
        {
            SmokeBomb.Cast();
            return;
        }
        if (ObjectManager.Me.HealthPercent <= 70 && Preparation.KnownSpell && Preparation.IsSpellUsable
            && MySettings.UsePreparation && !CloakofShadows.IsSpellUsable && ObjectManager.Target.IsCast
            && ObjectManager.Target.IsTargetingMe)
        {
            Preparation.Cast();
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
        if (Berserking.IsSpellUsable && Berserking.KnownSpell && ObjectManager.Target.GetDistance < 30
            && MySettings.UseBerserking)
        {
            Berserking.Cast();
            return;
        }
        if (BloodFury.IsSpellUsable && BloodFury.KnownSpell && ObjectManager.Target.GetDistance < 30
            && MySettings.UseBloodFury)
        {
            BloodFury.Cast();
            return;
        }
        if (AdrenalineRush.KnownSpell && AdrenalineRush.IsSpellUsable
            && MySettings.UseAdrenalineRush && ObjectManager.Target.GetDistance < 30)
        {
            AdrenalineRush.Cast();
            return;
        }
        if (KillingSpree.KnownSpell && KillingSpree.IsSpellUsable
            && MySettings.UseKillingSpree && ObjectManager.Target.GetDistance < 10
            && ObjectManager.Me.EnergyPercentage < 35)
        {
            KillingSpree.Cast();
            return;
        }
        if (ShadowBlades.KnownSpell && ShadowBlades.IsSpellUsable
            && MySettings.UseShadowBlades && ObjectManager.Target.GetDistance < 30)
        {
            ShadowBlades.Cast();
        }
    }

    private void DPSCycle()
    {
        Usefuls.SleepGlobalCooldown();
        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            if (Garrote.IsSpellUsable && Garrote.IsHostileDistanceGood && Garrote.KnownSpell
                && MySettings.UseGarrote && ObjectManager.Me.HaveBuff(115192))
            {
                Garrote.Cast();
                return;
            }
            if (Throw.IsSpellUsable && Throw.IsHostileDistanceGood && Throw.KnownSpell && !ObjectManager.Target.InCombat
                && MySettings.UseThrow)
            {
                Throw.Cast();
                return;
            }
            if (BladeFlurry.KnownSpell && BladeFlurry.IsSpellUsable && ObjectManager.Target.GetDistance < 10
                && MySettings.UseBladeFlurry && !BladeFlurry.HaveBuff && ObjectManager.GetNumberAttackPlayer() > 1)
            {
                BladeFlurry.Cast();
                return;
            }
            if (BladeFlurry.KnownSpell && BladeFlurry.IsSpellUsable && SinisterStrike.IsHostileDistanceGood
                && BladeFlurry.HaveBuff && ObjectManager.GetNumberAttackPlayer() < 2)
            {
                BladeFlurry.Cast();
                return;
            }
            if (Eviscerate.KnownSpell && Eviscerate.IsSpellUsable && Eviscerate.IsHostileDistanceGood
                && MySettings.UseEviscerate && ObjectManager.Me.ComboPoint > 4)
            {
                Eviscerate.Cast();
                return;
            }
            if (RevealingStrike.KnownSpell && RevealingStrike.IsSpellUsable && RevealingStrike.IsHostileDistanceGood
                && MySettings.UseRevealingStrike && !RevealingStrike.TargetHaveBuff)
            {
                RevealingStrike.Cast();
                return;
            }
            if (SliceandDice.KnownSpell && SliceandDice.IsSpellUsable && SliceandDice.IsHostileDistanceGood
                && MySettings.UseSliceandDice && !SliceandDice.HaveBuff)
            {
                CP = ObjectManager.Me.ComboPoint;
                SliceandDice.Cast();
                //_sliceandDiceTimer = new Timer(1000*(6 + (CP*6)));
                return;
            }
            if (Rupture.KnownSpell && Rupture.IsHostileDistanceGood && Rupture.IsSpellUsable
                && MySettings.UseRupture && (!Rupture.TargetHaveBuff || _ruptureTimer.IsReady))
            {
                CP = ObjectManager.Me.ComboPoint;
                Rupture.Cast();
                _ruptureTimer = new Timer(1000*(4 + (CP*4)));
                return;
            }
            if (ExposeArmor.IsSpellUsable && ExposeArmor.IsHostileDistanceGood && ExposeArmor.KnownSpell
                && MySettings.UseExposeArmor && !ObjectManager.Target.HaveBuff(113746))
            {
                ExposeArmor.Cast();
                return;
            }
            if (SinisterStrike.KnownSpell && SinisterStrike.IsSpellUsable && SinisterStrike.IsHostileDistanceGood
                && MySettings.UseSinisterStrike)
            {
                SinisterStrike.Cast();
                return;
            }
            if (ArcaneTorrent.IsSpellUsable && ArcaneTorrent.KnownSpell
                && MySettings.UseArcaneTorrentForResource)
            {
                ArcaneTorrent.Cast();
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

    #region Nested type: RogueOutlawSettings

    [Serializable]
    public class RogueOutlawSettings : Settings
    {
        public bool DoAvoidMelee = false;
        public int DoAvoidMeleeDistance = 0;
        public bool UseAdrenalineRush = true;
        public bool UseAlchFlask = true;
        public bool UseAmbush = true;
        public bool UseArcaneTorrentForDecast = true;
        public int UseArcaneTorrentForDecastAtPercentage = 100;
        public bool UseArcaneTorrentForResource = true;
        public bool UseBerserking = true;
        public bool UseBladeFlurry = true;
        public bool UseBloodFury = true;
        public bool UseBurstofSpeed = true;
        public bool UseCheapShot = true;
        public bool UseCloakofShadows = true;
        public bool UseCombatReadiness = true;
        public bool UseCrimsonTempest = true;
        public bool UseCripplingPoison = false;
        public bool UseDeadlyPoison = true;
        public bool UseDeadlyThrow = true;
        public bool UseDismantle = true;

        public bool UseEvasion = true;
        public bool UseEviscerate = true;
        public bool UseExposeArmor = false;
        public bool UseFanofKnives = true;
        public bool UseGarrote = true;
        public bool UseGiftoftheNaaru = true;
        public int UseGiftoftheNaaruAtPercentage = 80;
        public bool UseKick = true;
        public bool UseKidneyShot = true;
        public bool UseKillingSpree = true;
        public bool UseLeechingPoison = true;

        public bool UseLowCombat = true;
        public bool UseMindnumbingPoison = true;
        public bool UseParalyticPoison = false;
        public bool UsePreparation = true;
        public bool UseRecuperate = true;
        public bool UseRedirect = true;
        public bool UseRevealingStrike = true;
        public bool UseRupture = true;
        public bool UseShadowBlades = true;
        public bool UseShadowStep = true;
        public bool UseShiv = true;
        public bool UseShurikenToss = true;
        public bool UseSinisterStrike = true;
        public bool UseSliceandDice = true;
        public bool UseSmokeBomb = true;
        public bool UseSprint = true;
        public bool UseStealth = false;
        public bool UseStoneform = true;
        public int UseStoneformAtPercentage = 80;
        public bool UseThrow = true;
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;
        public bool UseVanish = true;
        public bool UseWarStomp = true;
        public int UseWarStompAtPercentage = 80;
        public bool UseWoundPoison = false;

        public RogueOutlawSettings()
        {
            ConfigWinForm("Rogue Outlaw Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent for Interrupt", "UseArcaneTorrentForDecast", "Professions & Racials", "AtPercentage");
            AddControlInWinForm("Use Arcane Torrent for Resource", "UseArcaneTorrentForResource", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");

            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            /* Rogue Buffs */
            AddControlInWinForm("Use Blade Flurry", "UseBladeFlurry", "Rogue Buffs");
            AddControlInWinForm("Use Burst of Speed", "UseBurstofSpeed", "Rogue Buffs");
            AddControlInWinForm("Use Crippling Poison", "UseCripplingPoison", "Rogue Buffs");
            AddControlInWinForm("Use Deadly Poison", "UseDeadlyPoison", "Rogue Buffs");
            AddControlInWinForm("Use Leeching Poison", "UseLeechingPoison", "Rogue Buffs");
            AddControlInWinForm("Use Mindnumbing Poison", "UseMindnumbingPoison", "Rogue Buffs");
            AddControlInWinForm("Use Paralytic Poison", "UseParalyticPoison", "Rogue Buffs");
            AddControlInWinForm("Use Slice and Dice", "UseSliceandDice", "Rogue Buffs");
            AddControlInWinForm("Use Sprint", "UseSprint", "Rogue Buffs");
            AddControlInWinForm("Use Stealth", "UseStealth", "Rogue Buffs");
            AddControlInWinForm("Use Wound Poison", "UseWoundPoison", "Rogue Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Ambush", "UseAmbush", "Offensive Spell");
            AddControlInWinForm("Use Crimson Tempest", "UseCrimsonTempest", "Offensive Spell");
            AddControlInWinForm("Use Deadly Throw", "UseDeadlyThrow", "Offensive Spell");
            AddControlInWinForm("Use Eviscerate", "UseEviscerate", "Offensive Spell");
            AddControlInWinForm("Use Expose Armor", "UseExposeArmor", "Offensive Spell");
            AddControlInWinForm("Use Fan of Knives", "UseFanofKnives", "Offensive Spell");
            AddControlInWinForm("Use Garrote", "UseGarrote", "Offensive Spell");
            AddControlInWinForm("Use Revealing Strike", "UseRevealingStrike", "Offensive Spell");
            AddControlInWinForm("Use Rupture", "UseRupture", "Offensive Spell");
            AddControlInWinForm("Use Shiv", "UseShiv", "Offensive Spell");
            AddControlInWinForm("Use Shuriken Toss", "UseShurikenToss", "Offensive Spell");
            AddControlInWinForm("Use Sinister Strike", "UseSinisterStrike", "Offensive Spell");
            AddControlInWinForm("Use Throw", "UseThrow", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use Adrenaline Rush", "UseAdrenalineRush", "Offensive Cooldown");
            AddControlInWinForm("Use Killing Spree", "UseKillingSpree", "Offensive Cooldown");
            AddControlInWinForm("Use Redirect", "UseRedirect", "Offensive Cooldown");
            AddControlInWinForm("Use Shadow Blades", "UseShadowBlades", "Offensive Cooldown");
            AddControlInWinForm("Use Shadow Step", "UseShadowStep", "Offensive Cooldown");
            AddControlInWinForm("Use Vendetta", "UseVendetta", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use CheapShot", "UseCheapShot", "Defensive Cooldown");
            AddControlInWinForm("Use CloakofShadows", "UseCloakofShadows", "Defensive Cooldown");
            AddControlInWinForm("Use CombatReadiness", "UseCombatReadiness", "Defensive Cooldown");
            AddControlInWinForm("Use Dismantle", "UseDismantle", "Defensive Cooldown");
            AddControlInWinForm("Use Evasion", "UseEvasion", "Defensive Cooldown");
            AddControlInWinForm("Use Kick", "UseKick", "Defensive Cooldown");
            AddControlInWinForm("Use KidneyShot", "UseKidneyShot", "Defensive Cooldown");
            AddControlInWinForm("Use Preparation", "UsePreparation", "Defensive Cooldown");
            AddControlInWinForm("Use SmokeBomb", "UseSmokeBomb", "Defensive Cooldown");
            AddControlInWinForm("Use Vanish", "UseVanish", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Recuperate", "UseRecuperate", "Healing Spell");
            /* Game Settings */
            AddControlInWinForm("Use Low Combat Settings", "UseLowCombat", "Game Settings");
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");

            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
            AddControlInWinForm("Do avoid melee (Off Advised!!)", "DoAvoidMelee", "Game Settings");
            AddControlInWinForm("Avoid melee distance (1 to 4)", "DoAvoidMeleeDistance", "Game Settings");
        }

        public static RogueOutlawSettings CurrentSetting { get; set; }

        public static RogueOutlawSettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Rogue_Combat.xml";
            if (File.Exists(currentSettingsFile))
            {
                return
                    CurrentSetting = Load<RogueOutlawSettings>(currentSettingsFile);
            }
            return new RogueOutlawSettings();
        }
    }

    #endregion
}

public class RogueSubtlety
{
    private static RogueSubtletySettings MySettings = RogueSubtletySettings.GetSettings();

    #region General Timers & Variables

    private readonly WoWItem _firstTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET);
    private readonly WoWItem _secondTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET, 2);
    public uint CP = 0;
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

    #region Rogue Buffs

    public readonly Spell BurstofSpeed = new Spell("Burst of Speed");
    public readonly Spell CripplingPoison = new Spell("Crippling Poison");
    public readonly Spell DeadlyPoison = new Spell("Deadly Poison");
    public readonly Spell LeechingPoison = new Spell("Leeching Poison");
    public readonly Spell MindnumbingPoison = new Spell("Mind-numbing Poison");
    public readonly Spell ParalyticPoison = new Spell("Paralytic Poison");
    public readonly Spell SliceandDice = new Spell("Slice and Dice");
    public readonly Spell Sprint = new Spell("Sprint");
    public readonly Spell Stealth = new Spell("Stealth");
    public readonly Spell WoundPoison = new Spell("Wound Poison");
/*
        private Timer _sliceandDiceTimer = new Timer(0);
*/

    #endregion

    #region Offensive Spell

    public readonly Spell Ambush = new Spell("Ambush");
    public readonly Spell CrimsonTempest = new Spell("Crimson Tempest");
    public readonly Spell DeadlyThrow = new Spell("Deadly Throw");
    public readonly Spell Eviscerate = new Spell("Eviscerate");
    public readonly Spell ExposeArmor = new Spell("Expose Armor");
    public readonly Spell FanofKnives = new Spell("Fan of Knives");
    public readonly Spell Garrote = new Spell("Garrote");
    public readonly Spell Hemorrhage = new Spell("Hemorrhage");
    public readonly Spell Rupture = new Spell("Rupture");
    public readonly Spell Shiv = new Spell("Shiv");
    public readonly Spell ShurikenToss = new Spell("Shuriken Toss");
    public readonly Spell SinisterStrike = new Spell("Sinister Strike");
    public readonly Spell Throw = new Spell("Throw");
    private Timer _ruptureTimer = new Timer(0);

    #endregion

    #region Offensive Cooldown

    public readonly Spell Premeditation = new Spell("Premeditation");
    public readonly Spell Redirect = new Spell("Redirect");
    public readonly Spell ShadowBlades = new Spell("Shadow Blades");
    public readonly Spell ShadowDance = new Spell("Shadow Dance");
    public readonly Spell ShadowStep = new Spell("Shadow Step");

    #endregion

    #region Defensive Cooldown

    public readonly Spell CheapShot = new Spell("Cheap Shot");
    public readonly Spell CloakofShadows = new Spell("Cloak of Shadows");
    public readonly Spell CombatReadiness = new Spell("Combat Readiness");
    public readonly Spell Dismantle = new Spell("Dismantle");
    public readonly Spell Evasion = new Spell("Evasion");
    public readonly Spell Kick = new Spell("Kick");
    public readonly Spell KidneyShot = new Spell("Kidney Shot");
    public readonly Spell Preparation = new Spell("Preparation");
    public readonly Spell SmokeBomb = new Spell("Smoke Bomb");
    public readonly Spell Vanish = new Spell("Vanish");
/*
        private Timer _dismantleTimer = new Timer(0);
*/

    #endregion

    #region Healing Spell

    public readonly Spell Recuperate = new Spell("Recuperate");

    #endregion

    public RogueSubtlety()
    {
        Main.InternalRange = ObjectManager.Me.GetCombatReach;
        Main.InternalLightHealingSpell = null;
        MySettings = RogueSubtletySettings.GetSettings();
        Main.DumpCurrentSettings<RogueSubtletySettings>(MySettings);
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
                                && (Throw.IsHostileDistanceGood || SinisterStrike.IsHostileDistanceGood))
                            {
                                Pull();
                                lastTarget = ObjectManager.Me.Target;
                            }

                            if (ObjectManager.Target.Level < 70 && ObjectManager.Me.Level > 84
                                && MySettings.UseLowCombat)
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

    private void Pull()
    {
        if (Redirect.IsSpellUsable && Redirect.IsHostileDistanceGood && Redirect.KnownSpell
            && MySettings.UseRedirect && ObjectManager.Me.ComboPoint > 0)
        {
            Redirect.Cast();
            Others.SafeSleep(200);
        }

        if (((Stealth.KnownSpell && Stealth.IsSpellUsable && !Stealth.HaveBuff && MySettings.UseStealth)
             || Stealth.HaveBuff) && LC != 1)
        {
            if (!Stealth.HaveBuff)
            {
                Stealth.Cast();
                Others.SafeSleep(200);
            }

            if (Premeditation.IsSpellUsable && Premeditation.IsHostileDistanceGood && Premeditation.KnownSpell
                && MySettings.UsePremeditation && ObjectManager.Me.ComboPoint == 0)
            {
                Premeditation.Cast();
                Others.SafeSleep(200);
            }

            if (ShadowStep.IsSpellUsable && ShadowStep.IsHostileDistanceGood && ShadowStep.KnownSpell
                && MySettings.UseShadowStep)
            {
                ShadowStep.Cast();
                Others.SafeSleep(200);
            }

            if (Garrote.IsSpellUsable && Garrote.IsHostileDistanceGood && Garrote.KnownSpell
                && MySettings.UseGarrote)
            {
                Garrote.Cast();
                return;
            }
            if (CheapShot.IsSpellUsable && CheapShot.IsHostileDistanceGood && CheapShot.KnownSpell
                && MySettings.UseCheapShot)
            {
                CheapShot.Cast();
                return;
            }
            return;
        }
        if (ShurikenToss.IsSpellUsable && ShurikenToss.IsHostileDistanceGood && ShurikenToss.KnownSpell
            && MySettings.UseShurikenToss && !MySettings.UseStealth)
        {
            ShurikenToss.Cast();
            return;
        }
        if (Throw.IsSpellUsable && Throw.IsHostileDistanceGood && Throw.KnownSpell
            && MySettings.UseThrow && !MySettings.UseStealth)
        {
            MovementManager.StopMove();
            Throw.Cast();
            Others.SafeSleep(1000);
        }
    }

    private void LowCombat()
    {
        Buff();
        if (MySettings.DoAvoidMelee)
            AvoidMelee();
        DefenseCycle();
        Heal();

        if (Throw.IsSpellUsable && Throw.IsHostileDistanceGood && Throw.KnownSpell && !ObjectManager.Target.InCombat
            && MySettings.UseThrow)
        {
            Throw.Cast();
            return;
        }

        if (Eviscerate.KnownSpell && Eviscerate.IsSpellUsable && Eviscerate.IsHostileDistanceGood
            && MySettings.UseEviscerate && ObjectManager.Me.ComboPoint > 4)
        {
            Eviscerate.Cast();
            return;
        }
        if (SliceandDice.KnownSpell && SliceandDice.IsSpellUsable && SliceandDice.IsHostileDistanceGood
            && MySettings.UseSliceandDice && !SliceandDice.HaveBuff)
        {
            CP = ObjectManager.Me.ComboPoint;
            SliceandDice.Cast();
            //_sliceandDiceTimer = new Timer(1000*(6 + (CP*6)));
            return;
        }
        // Blizzard API Calls for Hemorrhage using Sinister Strike Function
        if (SinisterStrike.KnownSpell && SinisterStrike.IsSpellUsable && SinisterStrike.IsHostileDistanceGood
            && MySettings.UseHemorrhage)
        {
            SinisterStrike.Cast();
            return;
        }
        if (FanofKnives.KnownSpell && FanofKnives.IsSpellUsable && FanofKnives.IsHostileDistanceGood
            && MySettings.UseFanofKnives)
        {
            FanofKnives.Cast();
        }
    }

    private void Combat()
    {
        Buff();
        if (MySettings.DoAvoidMelee)
            AvoidMelee();
        if (_onCd.IsReady)
            DefenseCycle();
        DPSCycle();
        Heal();
        Decast();
        DPSCycle();
        DPSBurst();
        DPSCycle();
    }

    private void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (MySettings.UseDeadlyPoison && DeadlyPoison.KnownSpell && DeadlyPoison.IsSpellUsable
            && !DeadlyPoison.HaveBuff)
        {
            DeadlyPoison.Cast();
            return;
        }
        if (!WoundPoison.HaveBuff && WoundPoison.KnownSpell && WoundPoison.IsSpellUsable
            && MySettings.UseWoundPoison && !DeadlyPoison.HaveBuff)
        {
            WoundPoison.Cast();
            return;
        }
        if (!LeechingPoison.HaveBuff && LeechingPoison.KnownSpell && LeechingPoison.IsSpellUsable
            && MySettings.UseLeechingPoison)
        {
            LeechingPoison.Cast();
            return;
        }
        if (!ParalyticPoison.HaveBuff && ParalyticPoison.KnownSpell && ParalyticPoison.IsSpellUsable
            && MySettings.UseParalyticPoison && !LeechingPoison.HaveBuff)
        {
            ParalyticPoison.Cast();
            return;
        }
        if (!CripplingPoison.HaveBuff && CripplingPoison.KnownSpell && CripplingPoison.IsSpellUsable
            && MySettings.UseCripplingPoison && !LeechingPoison.HaveBuff && ParalyticPoison.HaveBuff)
        {
            CripplingPoison.Cast();
            return;
        }
        if (!MindnumbingPoison.HaveBuff && MindnumbingPoison.KnownSpell && MindnumbingPoison.IsSpellUsable
            && MySettings.UseMindnumbingPoison && !CripplingPoison.HaveBuff && !ParalyticPoison.HaveBuff
            && !LeechingPoison.HaveBuff)
        {
            MindnumbingPoison.Cast();
            return;
        }
        if (!ObjectManager.Me.InCombat && BurstofSpeed.IsSpellUsable && BurstofSpeed.KnownSpell
            && MySettings.UseBurstofSpeed && ObjectManager.Me.GetMove)
        {
            BurstofSpeed.Cast();
            return;
        }
        if (!ObjectManager.Me.InCombat && Sprint.IsSpellUsable && Sprint.KnownSpell
            && MySettings.UseSprint && ObjectManager.Me.GetMove)
        {
            Sprint.Cast();
            return;
        }
        if (MySettings.UseAlchFlask && !ObjectManager.Me.HaveBuff(79638) && !ObjectManager.Me.HaveBuff(79640) && !ObjectManager.Me.HaveBuff(79639) &&
            !ItemsManager.IsItemOnCooldown(75525) && ItemsManager.GetItemCount(75525) > 0)
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
        if (ObjectManager.Me.HealthPercent <= 80 && !KidneyShot.TargetHaveBuff && KidneyShot.KnownSpell
            && KidneyShot.IsSpellUsable && KidneyShot.IsHostileDistanceGood && ObjectManager.Me.ComboPoint <= 3
            && Recuperate.HaveBuff && MySettings.UseKidneyShot)
        {
            CP = ObjectManager.Me.ComboPoint;
            KidneyShot.Cast();
            _onCd = new Timer(1000*CP);
            return;
        }
        if (ObjectManager.Me.HealthPercent <= 80 && Evasion.KnownSpell && Evasion.IsSpellUsable
            && MySettings.UseEvasion)
        {
            Evasion.Cast();
            _onCd = new Timer(1000*15);
            return;
        }
        if (ObjectManager.Me.HealthPercent <= 90 && CombatReadiness.KnownSpell && CombatReadiness.IsSpellUsable
            && MySettings.UseCombatReadiness)
        {
            CombatReadiness.Cast();
            _onCd = new Timer(1000*20);
            return;
        }
        if (ObjectManager.Me.HealthPercent <= 95 && Dismantle.KnownSpell && Dismantle.IsSpellUsable
            && MySettings.UseDismantle)
        {
            Dismantle.Cast();
            //_dismantleTimer = new Timer(1000*60);
            return;
        }
        if (ObjectManager.Me.HealthPercent <= MySettings.UseWarStompAtPercentage && WarStomp.IsSpellUsable &&
            WarStomp.KnownSpell
            && MySettings.UseWarStomp)
        {
            WarStomp.Cast();
            _onCd = new Timer(1000*2);
            return;
        }
        if (ObjectManager.Me.HealthPercent <= MySettings.UseStoneformAtPercentage && Stoneform.IsSpellUsable &&
            Stoneform.KnownSpell
            && MySettings.UseStoneform)
        {
            Stoneform.Cast();
            _onCd = new Timer(1000*8);
            return;
        }
        if (ObjectManager.GetNumberAttackPlayer() >= 3 && Vanish.KnownSpell && Vanish.IsSpellUsable
            && MySettings.UseVanish)
        {
            Vanish.Cast();
            Others.SafeSleep(5000);
            return;
        }
        if (ObjectManager.Me.HealthPercent <= 70 && Preparation.KnownSpell && Preparation.IsSpellUsable
            && MySettings.UsePreparation && !Evasion.IsSpellUsable)
        {
            Preparation.Cast();
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (ObjectManager.Me.HealthPercent <= MySettings.UseGiftoftheNaaruAtPercentage && GiftoftheNaaru.KnownSpell &&
            GiftoftheNaaru.IsSpellUsable
            && MySettings.UseGiftoftheNaaru)
        {
            GiftoftheNaaru.Cast();
            return;
        }
        if (!Recuperate.HaveBuff && ObjectManager.Me.ComboPoint > 1 && MySettings.UseRecuperate
            && ObjectManager.Me.HealthPercent <= 90 && Recuperate.KnownSpell && Recuperate.IsSpellUsable)
        {
            Recuperate.Cast();
        }
    }

    private void Decast()
    {
        if (ObjectManager.Target.IsCast && Kick.KnownSpell && Kick.IsSpellUsable
            && Kick.IsHostileDistanceGood && MySettings.UseKick && ObjectManager.Target.IsTargetingMe)
        {
            Kick.Cast();
            return;
        }
        if (ArcaneTorrent.IsSpellUsable && ArcaneTorrent.KnownSpell && ObjectManager.Target.GetDistance < 8
            && ObjectManager.Me.HealthPercent <= MySettings.UseArcaneTorrentForDecastAtPercentage
            && MySettings.UseArcaneTorrentForDecast && ObjectManager.Target.IsCast &&
            ObjectManager.Target.IsTargetingMe)
        {
            ArcaneTorrent.Cast();
            return;
        }
        if (ObjectManager.Target.IsCast && CloakofShadows.KnownSpell && CloakofShadows.IsSpellUsable
            && ObjectManager.Target.IsTargetingMe && MySettings.UseCloakofShadows)
        {
            CloakofShadows.Cast();
            return;
        }
        if (ObjectManager.Target.IsCast && SmokeBomb.KnownSpell && SmokeBomb.IsSpellUsable
            && ObjectManager.Target.IsTargetingMe && MySettings.UseSmokeBomb
            && !CloakofShadows.HaveBuff)
        {
            SmokeBomb.Cast();
            return;
        }

        if (ObjectManager.Me.HealthPercent <= 70 && Preparation.KnownSpell && Preparation.IsSpellUsable
            && MySettings.UsePreparation && !CloakofShadows.IsSpellUsable && ObjectManager.Target.IsCast
            && ObjectManager.Target.IsTargetingMe)
        {
            Preparation.Cast();
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
        if (Berserking.IsSpellUsable && Berserking.KnownSpell && ObjectManager.Target.GetDistance < 30
            && MySettings.UseBerserking)
        {
            Berserking.Cast();
            return;
        }
        if (BloodFury.IsSpellUsable && BloodFury.KnownSpell && ObjectManager.Target.GetDistance < 30
            && MySettings.UseBloodFury)
        {
            BloodFury.Cast();
            return;
        }
        if (ShadowDance.KnownSpell && ShadowDance.IsSpellUsable
            && MySettings.UseShadowDance && ObjectManager.Target.GetDistance < 10)
        {
            ShadowDance.Cast();
            return;
        }
        if (ShadowBlades.KnownSpell && ShadowBlades.IsSpellUsable
            && MySettings.UseShadowBlades && ObjectManager.Target.GetDistance < 30)
        {
            ShadowBlades.Cast();
        }
    }

    private void DPSCycle()
    {
        Usefuls.SleepGlobalCooldown();
        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            if (ObjectManager.Me.HaveBuff(115192) || ObjectManager.Me.HaveBuff(51713))
            {
                if (Garrote.IsSpellUsable && Garrote.IsHostileDistanceGood && Garrote.KnownSpell
                    && MySettings.UseGarrote && !ObjectManager.Target.HaveBuff(703))
                {
                    Garrote.Cast();
                    return;
                }
            }

            if (Throw.IsSpellUsable && Throw.IsHostileDistanceGood && Throw.KnownSpell && !ObjectManager.Target.InCombat
                && MySettings.UseThrow)
            {
                Throw.Cast();
                return;
            }

            if (Eviscerate.KnownSpell && Eviscerate.IsSpellUsable && Eviscerate.IsHostileDistanceGood
                && MySettings.UseEviscerate && ObjectManager.Me.ComboPoint > 4)
            {
                Eviscerate.Cast();
                return;
            }
            if (SliceandDice.KnownSpell && SliceandDice.IsSpellUsable && SliceandDice.IsHostileDistanceGood
                && MySettings.UseSliceandDice && !SliceandDice.HaveBuff)
            {
                CP = ObjectManager.Me.ComboPoint;
                SliceandDice.Cast();
                //_sliceandDiceTimer = new Timer(1000*(6 + (CP*6)));
                return;
            }
            if (Rupture.KnownSpell && Rupture.IsHostileDistanceGood && Rupture.IsSpellUsable
                && MySettings.UseRupture && (!Rupture.TargetHaveBuff || _ruptureTimer.IsReady))
            {
                CP = ObjectManager.Me.ComboPoint;
                Rupture.Cast();
                _ruptureTimer = new Timer(1000*(4 + (CP*4)));
                return;
            }
            if (ExposeArmor.IsSpellUsable && ExposeArmor.IsHostileDistanceGood && ExposeArmor.KnownSpell
                && MySettings.UseExposeArmor && !ObjectManager.Target.HaveBuff(113746))
            {
                ExposeArmor.Cast();
                return;
            }
            if (Hemorrhage.KnownSpell && Hemorrhage.IsSpellUsable && Hemorrhage.IsHostileDistanceGood
                && MySettings.UseHemorrhage)
            {
                Hemorrhage.Cast();
                return;
            }
            if (ArcaneTorrent.IsSpellUsable && ArcaneTorrent.KnownSpell
                && MySettings.UseArcaneTorrentForResource)
            {
                ArcaneTorrent.Cast();
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
            Buff();
            Heal();
        }
    }

    #region Nested type: RogueSubtletySettings

    [Serializable]
    public class RogueSubtletySettings : Settings
    {
        public bool DoAvoidMelee = false;
        public int DoAvoidMeleeDistance = 0;
        public bool UseAlchFlask = true;
        public bool UseAmbush = true;
        public bool UseArcaneTorrentForDecast = true;
        public int UseArcaneTorrentForDecastAtPercentage = 100;
        public bool UseArcaneTorrentForResource = true;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseBurstofSpeed = true;
        public bool UseCheapShot = true;
        public bool UseCloakofShadows = true;
        public bool UseCombatReadiness = true;
        public bool UseCrimsonTempest = true;
        public bool UseCripplingPoison = false;
        public bool UseDeadlyPoison = true;
        public bool UseDeadlyThrow = true;
        public bool UseDismantle = true;

        public bool UseEvasion = true;
        public bool UseEviscerate = true;
        public bool UseExposeArmor = false;
        public bool UseFanofKnives = true;
        public bool UseGarrote = true;
        public bool UseGiftoftheNaaru = true;
        public int UseGiftoftheNaaruAtPercentage = 80;
        public bool UseHemorrhage = true;
        public bool UseKick = true;
        public bool UseKidneyShot = true;
        public bool UseLeechingPoison = true;

        public bool UseLowCombat = true;
        public bool UseMindnumbingPoison = true;
        public bool UseParalyticPoison = false;
        public bool UsePremeditation = true;
        public bool UsePreparation = true;
        public bool UseRecuperate = true;
        public bool UseRedirect = true;
        public bool UseRupture = true;
        public bool UseShadowBlades = true;
        public bool UseShadowDance = true;
        public bool UseShadowStep = true;
        public bool UseShiv = true;
        public bool UseShurikenToss = true;
        public bool UseSliceandDice = true;
        public bool UseSmokeBomb = true;
        public bool UseSprint = true;
        public bool UseStealth = false;
        public bool UseStoneform = true;
        public int UseStoneformAtPercentage = 80;
        public bool UseThrow = true;
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;
        public bool UseVanish = true;
        public bool UseWarStomp = true;
        public int UseWarStompAtPercentage = 80;
        public bool UseWoundPoison = false;

        public RogueSubtletySettings()
        {
            ConfigWinForm("Rogue Subtlety Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent for Interrupt", "UseArcaneTorrentForDecast", "Professions & Racials", "AtPercentage");
            AddControlInWinForm("Use Arcane Torrent for Resource", "UseArcaneTorrentForResource", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");

            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            /* Rogue Buffs */
            AddControlInWinForm("Use Burst of Speed", "UseBurstofSpeed", "Rogue Buffs");
            AddControlInWinForm("Use Crippling Poison", "UseCripplingPoison", "Rogue Buffs");
            AddControlInWinForm("Use Deadly Poison", "UseDeadlyPoison", "Rogue Buffs");
            AddControlInWinForm("Use Leeching Poison", "UseLeechingPoison", "Rogue Buffs");
            AddControlInWinForm("Use Mindnumbing Poison", "UseMindnumbingPoison", "Rogue Buffs");
            AddControlInWinForm("Use Paralytic Poison", "UseParalyticPoison", "Rogue Buffs");
            AddControlInWinForm("Use Slice and Dice", "UseSliceandDice", "Rogue Buffs");
            AddControlInWinForm("Use Sprint", "UseSprint", "Rogue Buffs");
            AddControlInWinForm("Use Stealth", "UseStealth", "Rogue Buffs");
            AddControlInWinForm("Use Wound Poison", "UseWoundPoison", "Rogue Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Ambush", "UseAmbush", "Offensive Spell");
            AddControlInWinForm("Use Crimson Tempest", "UseCrimsonTempest", "Offensive Spell");
            AddControlInWinForm("Use Deadly Throw", "UseDeadlyThrow", "Offensive Spell");
            AddControlInWinForm("Use Expose Armor", "UseExposeArmor", "Offensive Spell");
            AddControlInWinForm("Use Fan of Knives", "UseFanofKnives", "Offensive Spell");
            AddControlInWinForm("Use Eviscerate", "UseEviscerate", "Offensive Spell");
            AddControlInWinForm("Use Garrote", "UseGarrote", "Offensive Spell");
            AddControlInWinForm("Use Hemorrhage", "UseHemorrhage", "Offensive Spell");
            AddControlInWinForm("Use Rupture", "UseRupture", "Offensive Spell");
            AddControlInWinForm("Use Shiv", "UseShiv", "Offensive Spell");
            AddControlInWinForm("Use Shuriken Toss", "UseShurikenToss", "Offensive Spell");
            AddControlInWinForm("Use Throw", "UseThrow", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use Premeditation", "UsePremeditation", "Offensive Cooldown");
            AddControlInWinForm("Use Redirect", "UseRedirect", "Offensive Cooldown");
            AddControlInWinForm("Use Shadow Blades", "UseShadowBlades", "Offensive Cooldown");
            AddControlInWinForm("Use Shadow Dance", "UseShadowDance", "Offensive Cooldown");
            AddControlInWinForm("Use Shadow Step", "UseShadowStep", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use CheapShot", "UseCheapShot", "Defensive Cooldown");
            AddControlInWinForm("Use CloakofShadows", "UseCloakofShadows", "Defensive Cooldown");
            AddControlInWinForm("Use CombatReadiness", "UseCombatReadiness", "Defensive Cooldown");
            AddControlInWinForm("Use Dismantle", "UseDismantle", "Defensive Cooldown");
            AddControlInWinForm("Use Evasion", "UseEvasion", "Defensive Cooldown");
            AddControlInWinForm("Use Kick", "UseKick", "Defensive Cooldown");
            AddControlInWinForm("Use KidneyShot", "UseKidneyShot", "Defensive Cooldown");
            AddControlInWinForm("Use Preparation", "UsePreparation", "Defensive Cooldown");
            AddControlInWinForm("Use SmokeBomb", "UseSmokeBomb", "Defensive Cooldown");
            AddControlInWinForm("Use Vanish", "UseVanish", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Recuperate", "UseRecuperate", "Healing Spell");
            /* Game Settings */
            AddControlInWinForm("Use Low Combat Settings", "UseLowCombat", "Game Settings");
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");

            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
            AddControlInWinForm("Do avoid melee (Off Advised!!)", "DoAvoidMelee", "Game Settings");
            AddControlInWinForm("Avoid melee distance (1 to 4)", "DoAvoidMeleeDistance", "Game Settings");
        }

        public static RogueSubtletySettings CurrentSetting { get; set; }

        public static RogueSubtletySettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Rogue_Subtlety.xml";
            if (File.Exists(currentSettingsFile))
            {
                return
                    CurrentSetting = Load<RogueSubtletySettings>(currentSettingsFile);
            }
            return new RogueSubtletySettings();
        }
    }

    #endregion
}

public class RogueAssassination
{
    private static RogueAssassinationSettings MySettings = RogueAssassinationSettings.GetSettings();

    #region General Timers & Variables

    private readonly WoWItem _firstTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET);
    private readonly WoWItem _secondTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET, 2);
    public uint CP = 0;
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

    #region Rogue Buffs

    public readonly Spell BurstofSpeed = new Spell("Burst of Speed");
    public readonly Spell CripplingPoison = new Spell("Crippling Poison");
    public readonly Spell DeadlyPoison = new Spell("Deadly Poison");
    public readonly Spell LeechingPoison = new Spell("Leeching Poison");
    public readonly Spell MindnumbingPoison = new Spell("Mind-numbing Poison");
    public readonly Spell ParalyticPoison = new Spell("Paralytic Poison");
    public readonly Spell SliceandDice = new Spell("Slice and Dice");
    public readonly Spell Sprint = new Spell("Sprint");
    public readonly Spell Stealth = new Spell("Stealth");
    public readonly Spell WoundPoison = new Spell("Wound Poison");
    private Timer _sliceandDiceTimer = new Timer(0);

    #endregion

    #region Offensive Spell

    public readonly Spell Ambush = new Spell("Ambush");
    public readonly Spell CrimsonTempest = new Spell("Crimson Tempest");
    public readonly Spell DeadlyThrow = new Spell("Deadly Throw");
    public readonly Spell Dispatch = new Spell("Dispatch");
    public readonly Spell Envenom = new Spell("Envenom");
    public readonly Spell Eviscerate = new Spell("Eviscerate");
    public readonly Spell ExposeArmor = new Spell("Expose Armor");
    public readonly Spell FanofKnives = new Spell("Fan of Knives");
    public readonly Spell Garrote = new Spell("Garrote");
    public readonly Spell Mutilate = new Spell("Mutilate");
    public readonly Spell Rupture = new Spell("Rupture");
    public readonly Spell Shiv = new Spell("Shiv");
    public readonly Spell ShurikenToss = new Spell("Shuriken Toss");
    public readonly Spell SinisterStrike = new Spell("Sinister Strike");
    public readonly Spell Throw = new Spell("Throw");
    private Timer _ruptureTimer = new Timer(0);

    #endregion

    #region Offensive Cooldown

    public readonly Spell Redirect = new Spell("Redirect");
    public readonly Spell ShadowBlades = new Spell("Shadow Blades");
    public readonly Spell ShadowStep = new Spell("Shadow Step");
    public readonly Spell Vendetta = new Spell("Vendetta");

    #endregion

    #region Defensive Cooldown

    public readonly Spell CheapShot = new Spell("Cheap Shot");
    public readonly Spell CloakofShadows = new Spell("Cloak of Shadows");
    public readonly Spell CombatReadiness = new Spell("Combat Readiness");
    public readonly Spell Dismantle = new Spell("Dismantle");
    public readonly Spell Evasion = new Spell("Evasion");
    public readonly Spell Kick = new Spell("Kick");
    public readonly Spell KidneyShot = new Spell("Kidney Shot");
    public readonly Spell Preparation = new Spell("Preparation");
    public readonly Spell SmokeBomb = new Spell("Smoke Bomb");
    public readonly Spell Vanish = new Spell("Vanish");
/*
        private Timer _dismantleTimer = new Timer(0);
*/

    #endregion

    #region Healing Spell

    public readonly Spell Recuperate = new Spell("Recuperate");

    #endregion

    public RogueAssassination()
    {
        Main.InternalRange = ObjectManager.Me.GetCombatReach;
        Main.InternalLightHealingSpell = null;
        MySettings = RogueAssassinationSettings.GetSettings();
        Main.DumpCurrentSettings<RogueAssassinationSettings>(MySettings);
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
                                && (Throw.IsHostileDistanceGood || SinisterStrike.IsHostileDistanceGood))
                            {
                                Pull();
                                lastTarget = ObjectManager.Me.Target;
                            }

                            if (ObjectManager.Target.Level < 70 && ObjectManager.Me.Level > 84
                                && MySettings.UseLowCombat)
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

    private void Pull()
    {
        if (Redirect.IsSpellUsable && Redirect.IsHostileDistanceGood && Redirect.KnownSpell
            && MySettings.UseRedirect && ObjectManager.Me.ComboPoint > 0)
        {
            Redirect.Cast();
            Others.SafeSleep(200);
        }

        if (((Stealth.KnownSpell && Stealth.IsSpellUsable && !Stealth.HaveBuff && MySettings.UseStealth)
             || Stealth.HaveBuff) && LC != 1)
        {
            if (!Stealth.HaveBuff)
            {
                Stealth.Cast();
                Others.SafeSleep(200);
            }

            if (ShadowStep.IsSpellUsable && ShadowStep.IsHostileDistanceGood && ShadowStep.KnownSpell
                && MySettings.UseShadowStep)
            {
                ShadowStep.Cast();
                Others.SafeSleep(200);
            }

            if (Garrote.IsSpellUsable && Garrote.IsHostileDistanceGood && Garrote.KnownSpell
                && MySettings.UseGarrote)
            {
                Garrote.Cast();
                return;
            }
            if (CheapShot.IsSpellUsable && CheapShot.IsHostileDistanceGood && CheapShot.KnownSpell
                && MySettings.UseCheapShot)
            {
                CheapShot.Cast();
                return;
            }
            return;
        }
        if (ShurikenToss.IsSpellUsable && ShurikenToss.IsHostileDistanceGood && ShurikenToss.KnownSpell
            && MySettings.UseShurikenToss && !MySettings.UseStealth)
        {
            ShurikenToss.Cast();
            return;
        }
        if (Throw.IsSpellUsable && Throw.IsHostileDistanceGood && Throw.KnownSpell
            && MySettings.UseThrow && !MySettings.UseStealth)
        {
            MovementManager.StopMove();
            Throw.Cast();
            Others.SafeSleep(1000);
        }
    }

    private void LowCombat()
    {
        Buff();
        if (MySettings.DoAvoidMelee)
            AvoidMelee();
        DefenseCycle();
        Heal();

        if (Throw.IsSpellUsable && Throw.IsHostileDistanceGood && Throw.KnownSpell && !ObjectManager.Target.InCombat
            && MySettings.UseThrow)
        {
            Throw.Cast();
            return;
        }
        // Blizzard API Calls for Envenom using Eviscerate Function
        if (Eviscerate.KnownSpell && Eviscerate.IsSpellUsable && Eviscerate.IsHostileDistanceGood
            && MySettings.UseEnvenom && (ObjectManager.Me.ComboPoint > 4
                                         || (SliceandDice.HaveBuff && _sliceandDiceTimer.IsReady)))
        {
            Eviscerate.Cast();
            if (SliceandDice.HaveBuff)
                _sliceandDiceTimer = new Timer(1000*(6 + (5*6)));
            return;
        }
        // Blizzard API Calls for Dispatch using Sinister Strike Function
        if (SinisterStrike.KnownSpell && SinisterStrike.IsSpellUsable && SinisterStrike.IsHostileDistanceGood
            && MySettings.UseDispatch)
        {
            SinisterStrike.Cast();
            return;
        }
        if (SliceandDice.KnownSpell && SliceandDice.IsSpellUsable && SliceandDice.IsHostileDistanceGood
            && MySettings.UseSliceandDice && !SliceandDice.HaveBuff)
        {
            CP = ObjectManager.Me.ComboPoint;
            SliceandDice.Cast();
            _sliceandDiceTimer = new Timer(1000*(6 + (CP*6)));
            return;
        }
        if (Mutilate.KnownSpell && Mutilate.IsSpellUsable && ObjectManager.Target.HealthPercent > 35
            && MySettings.UseMutilate && Mutilate.IsHostileDistanceGood)
        {
            Mutilate.Cast();
            return;
        }
        if (FanofKnives.KnownSpell && FanofKnives.IsSpellUsable && FanofKnives.IsHostileDistanceGood
            && MySettings.UseFanofKnives)
        {
            FanofKnives.Cast();
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

        if (MySettings.UseDeadlyPoison && DeadlyPoison.KnownSpell && DeadlyPoison.IsSpellUsable
            && !DeadlyPoison.HaveBuff)
        {
            DeadlyPoison.Cast();
            return;
        }
        if (!WoundPoison.HaveBuff && WoundPoison.KnownSpell && WoundPoison.IsSpellUsable
            && MySettings.UseWoundPoison && !DeadlyPoison.HaveBuff)
        {
            WoundPoison.Cast();
            return;
        }
        if (!LeechingPoison.HaveBuff && LeechingPoison.KnownSpell && LeechingPoison.IsSpellUsable
            && MySettings.UseLeechingPoison)
        {
            LeechingPoison.Cast();
            return;
        }
        if (!ParalyticPoison.HaveBuff && ParalyticPoison.KnownSpell && ParalyticPoison.IsSpellUsable
            && MySettings.UseParalyticPoison && !LeechingPoison.HaveBuff)
        {
            ParalyticPoison.Cast();
            return;
        }
        if (!CripplingPoison.HaveBuff && CripplingPoison.KnownSpell && CripplingPoison.IsSpellUsable
            && MySettings.UseCripplingPoison && !LeechingPoison.HaveBuff && ParalyticPoison.HaveBuff)
        {
            CripplingPoison.Cast();
            return;
        }
        if (!MindnumbingPoison.HaveBuff && MindnumbingPoison.KnownSpell && MindnumbingPoison.IsSpellUsable
            && MySettings.UseMindnumbingPoison && !CripplingPoison.HaveBuff && !ParalyticPoison.HaveBuff
            && !LeechingPoison.HaveBuff)
        {
            MindnumbingPoison.Cast();
            return;
        }
        if (!ObjectManager.Me.InCombat && BurstofSpeed.IsSpellUsable && BurstofSpeed.KnownSpell
            && MySettings.UseBurstofSpeed && ObjectManager.Me.GetMove)
        {
            BurstofSpeed.Cast();
            return;
        }
        if (!ObjectManager.Me.InCombat && Sprint.IsSpellUsable && Sprint.KnownSpell
            && MySettings.UseSprint && ObjectManager.Me.GetMove)
        {
            Sprint.Cast();
            return;
        }
        if (MySettings.UseAlchFlask && !ObjectManager.Me.HaveBuff(79638) && !ObjectManager.Me.HaveBuff(79640) && !ObjectManager.Me.HaveBuff(79639) &&
            !ItemsManager.IsItemOnCooldown(75525) && ItemsManager.GetItemCount(75525) > 0)
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
        if (ObjectManager.Me.HealthPercent <= 80 && !KidneyShot.TargetHaveBuff && KidneyShot.KnownSpell
            && KidneyShot.IsSpellUsable && KidneyShot.IsHostileDistanceGood && ObjectManager.Me.ComboPoint <= 3
            && Recuperate.HaveBuff && MySettings.UseKidneyShot)
        {
            CP = ObjectManager.Me.ComboPoint;
            KidneyShot.Cast();
            _onCd = new Timer(1000*CP);
            return;
        }
        if (ObjectManager.Me.HealthPercent <= 80 && Evasion.KnownSpell && Evasion.IsSpellUsable
            && MySettings.UseEvasion)
        {
            Evasion.Cast();
            _onCd = new Timer(1000*15);
            return;
        }
        if (ObjectManager.Me.HealthPercent <= 90 && CombatReadiness.KnownSpell && CombatReadiness.IsSpellUsable
            && MySettings.UseCombatReadiness)
        {
            CombatReadiness.Cast();
            _onCd = new Timer(1000*20);
            return;
        }
        if (ObjectManager.Me.HealthPercent <= 95 && Dismantle.KnownSpell && Dismantle.IsSpellUsable
            && MySettings.UseDismantle)
        {
            Dismantle.Cast();
            //_dismantleTimer = new Timer(1000*60);
            return;
        }
        if (ObjectManager.Me.HealthPercent <= MySettings.UseWarStompAtPercentage && WarStomp.IsSpellUsable &&
            WarStomp.KnownSpell
            && MySettings.UseWarStomp)
        {
            WarStomp.Cast();
            _onCd = new Timer(1000*2);
            return;
        }
        if (ObjectManager.Me.HealthPercent <= MySettings.UseStoneformAtPercentage && Stoneform.IsSpellUsable &&
            Stoneform.KnownSpell
            && MySettings.UseStoneform)
        {
            Stoneform.Cast();
            _onCd = new Timer(1000*8);
            return;
        }
        if (ObjectManager.GetNumberAttackPlayer() >= 3 && Vanish.KnownSpell && Vanish.IsSpellUsable
            && MySettings.UseVanish)
        {
            Vanish.Cast();
            Others.SafeSleep(5000);
            return;
        }
        if (ObjectManager.Me.HealthPercent <= 70 && Preparation.KnownSpell && Preparation.IsSpellUsable
            && MySettings.UsePreparation && !Evasion.IsSpellUsable)
        {
            Preparation.Cast();
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.IsMounted)
            return;
        if (ObjectManager.Me.HealthPercent <= MySettings.UseGiftoftheNaaruAtPercentage && GiftoftheNaaru.KnownSpell &&
            GiftoftheNaaru.IsSpellUsable
            && MySettings.UseGiftoftheNaaru)
        {
            GiftoftheNaaru.Cast();
            return;
        }
        if (!Recuperate.HaveBuff && ObjectManager.Me.ComboPoint > 1 && MySettings.UseRecuperate
            && ObjectManager.Me.HealthPercent <= 90 && Recuperate.KnownSpell && Recuperate.IsSpellUsable)
        {
            Recuperate.Cast();
        }
    }

    private void Decast()
    {
        if (ObjectManager.Target.IsCast && Kick.KnownSpell && Kick.IsSpellUsable
            && Kick.IsHostileDistanceGood && MySettings.UseKick && ObjectManager.Target.IsTargetingMe)
        {
            Kick.Cast();
            return;
        }
        if (ArcaneTorrent.IsSpellUsable && ArcaneTorrent.KnownSpell && ObjectManager.Target.GetDistance < 8
            && ObjectManager.Me.HealthPercent <= MySettings.UseArcaneTorrentForDecastAtPercentage
            && MySettings.UseArcaneTorrentForDecast && ObjectManager.Target.IsCast &&
            ObjectManager.Target.IsTargetingMe)
        {
            ArcaneTorrent.Cast();
            return;
        }
        if (ObjectManager.Target.IsCast && CloakofShadows.KnownSpell && CloakofShadows.IsSpellUsable
            && ObjectManager.Target.IsTargetingMe && MySettings.UseCloakofShadows)
        {
            CloakofShadows.Cast();
            return;
        }
        if (ObjectManager.Target.IsCast && SmokeBomb.KnownSpell && SmokeBomb.IsSpellUsable
            && ObjectManager.Target.IsTargetingMe && MySettings.UseSmokeBomb
            && !CloakofShadows.HaveBuff)
        {
            SmokeBomb.Cast();
            return;
        }
        if (ObjectManager.Me.HealthPercent <= 70 && Preparation.KnownSpell && Preparation.IsSpellUsable
            && MySettings.UsePreparation && !CloakofShadows.IsSpellUsable && ObjectManager.Target.IsCast
            && ObjectManager.Target.IsTargetingMe)
        {
            Preparation.Cast();
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
        if (Berserking.IsSpellUsable && Berserking.KnownSpell && ObjectManager.Target.GetDistance < 30
            && MySettings.UseBerserking)
        {
            Berserking.Cast();
            return;
        }
        if (BloodFury.IsSpellUsable && BloodFury.KnownSpell && ObjectManager.Target.GetDistance < 30
            && MySettings.UseBloodFury)
        {
            BloodFury.Cast();
            return;
        }
        if (Vendetta.KnownSpell && Vendetta.IsSpellUsable
            && MySettings.UseVendetta && Vendetta.IsHostileDistanceGood)
        {
            Vendetta.Cast();
            return;
        }
        if (ShadowBlades.KnownSpell && ShadowBlades.IsSpellUsable
            && MySettings.UseShadowBlades && ObjectManager.Target.GetDistance < 30)
        {
            ShadowBlades.Cast();
        }
    }

    private void DPSCycle()
    {
        Usefuls.SleepGlobalCooldown();
        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            if (Mutilate.KnownSpell && Mutilate.IsSpellUsable && MySettings.UseMutilate
                && Mutilate.IsHostileDistanceGood && MySettings.UseShadowFocus && !ObjectManager.Target.InCombat
                && (Stealth.HaveBuff || ObjectManager.Me.HaveBuff(115192)))
            {
                Mutilate.Cast();
                return;
            }

            if (Garrote.IsSpellUsable && Garrote.IsHostileDistanceGood && Garrote.KnownSpell
                && MySettings.UseGarrote && ObjectManager.Me.HaveBuff(115192))
            {
                Garrote.Cast();
                return;
            }

            if (Throw.IsSpellUsable && Throw.IsHostileDistanceGood && Throw.KnownSpell && !ObjectManager.Target.InCombat
                && MySettings.UseThrow)
            {
                Throw.Cast();
                return;
            }

            if (Eviscerate.KnownSpell && Eviscerate.IsSpellUsable && Eviscerate.IsHostileDistanceGood
                && MySettings.UseEnvenom && (ObjectManager.Me.ComboPoint > 4
                                             || (SliceandDice.HaveBuff && _sliceandDiceTimer.IsReady)))
            {
                Eviscerate.Cast();
                if (SliceandDice.HaveBuff)
                    _sliceandDiceTimer = new Timer(1000*(6 + (5*6)));
                return;
            }
            if (SinisterStrike.KnownSpell && SinisterStrike.IsSpellUsable && SinisterStrike.IsHostileDistanceGood
                && MySettings.UseDispatch)
            {
                SinisterStrike.Cast();
                return;
            }
            if (SliceandDice.KnownSpell && SliceandDice.IsSpellUsable && SliceandDice.IsHostileDistanceGood
                && MySettings.UseSliceandDice && !SliceandDice.HaveBuff)
            {
                CP = ObjectManager.Me.ComboPoint;
                SliceandDice.Cast();
                _sliceandDiceTimer = new Timer(1000*(6 + (CP*6)));
                return;
            }
            if (Rupture.KnownSpell && Rupture.IsHostileDistanceGood && Rupture.IsSpellUsable
                && MySettings.UseRupture && (!Rupture.TargetHaveBuff || _ruptureTimer.IsReady))
            {
                CP = ObjectManager.Me.ComboPoint;
                Rupture.Cast();
                _ruptureTimer = new Timer(1000*(4 + (CP*4)));
                return;
            }
            if (ExposeArmor.IsSpellUsable && ExposeArmor.IsHostileDistanceGood && ExposeArmor.KnownSpell
                && MySettings.UseExposeArmor && !ObjectManager.Target.HaveBuff(113746))
            {
                ExposeArmor.Cast();
                return;
            }
            if (Mutilate.KnownSpell && Mutilate.IsSpellUsable && ObjectManager.Target.HealthPercent > 35
                && MySettings.UseMutilate && Mutilate.IsHostileDistanceGood)
            {
                Mutilate.Cast();
                return;
            }
            if (ArcaneTorrent.IsSpellUsable && ArcaneTorrent.KnownSpell
                && MySettings.UseArcaneTorrentForResource)
            {
                ArcaneTorrent.Cast();
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
            Buff();
            Heal();
        }
    }

    #region Nested type: RogueAssassinationSettings

    [Serializable]
    public class RogueAssassinationSettings : Settings
    {
        public bool DoAvoidMelee = false;
        public int DoAvoidMeleeDistance = 0;
        public bool UseAlchFlask = true;
        public bool UseAmbush = true;
        public bool UseArcaneTorrentForDecast = true;
        public int UseArcaneTorrentForDecastAtPercentage = 100;
        public bool UseArcaneTorrentForResource = true;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseBurstofSpeed = true;
        public bool UseCheapShot = true;
        public bool UseCloakofShadows = true;
        public bool UseCombatReadiness = true;
        public bool UseCrimsonTempest = true;
        public bool UseCripplingPoison = false;
        public bool UseDeadlyPoison = true;
        public bool UseDeadlyThrow = true;
        public bool UseDismantle = true;
        public bool UseDispatch = true;

        public bool UseEnvenom = true;
        public bool UseEvasion = true;
        public bool UseExposeArmor = false;
        public bool UseFanofKnives = true;
        public bool UseGarrote = true;
        public bool UseGiftoftheNaaru = true;
        public int UseGiftoftheNaaruAtPercentage = 80;
        public bool UseKick = true;
        public bool UseKidneyShot = true;
        public bool UseLeechingPoison = true;

        public bool UseLowCombat = true;
        public bool UseMindnumbingPoison = true;
        public bool UseMutilate = true;
        public bool UseParalyticPoison = false;
        public bool UsePreparation = true;
        public bool UseRecuperate = true;
        public bool UseRedirect = true;
        public bool UseRupture = true;
        public bool UseShadowBlades = true;
        public bool UseShadowFocus = false;
        public bool UseShadowStep = true;
        public bool UseShiv = true;
        public bool UseShurikenToss = true;
        public bool UseSliceandDice = true;
        public bool UseSmokeBomb = true;
        public bool UseSprint = true;
        public bool UseStealth = false;
        public bool UseStoneform = true;
        public int UseStoneformAtPercentage = 80;
        public bool UseThrow = true;
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;
        public bool UseVanish = true;
        public bool UseVendetta = true;
        public bool UseWarStomp = true;
        public int UseWarStompAtPercentage = 80;
        public bool UseWoundPoison = false;

        public RogueAssassinationSettings()
        {
            ConfigWinForm("Rogue Assassination Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent for Interrupt", "UseArcaneTorrentForDecast", "Professions & Racials", "AtPercentage");
            AddControlInWinForm("Use Arcane Torrent for Resource", "UseArcaneTorrentForResource", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");

            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            /* Rogue Buffs */
            AddControlInWinForm("Use Burst of Speed", "UseBurstofSpeed", "Rogue Buffs");
            AddControlInWinForm("Use Crippling Poison", "UseCripplingPoison", "Rogue Buffs");
            AddControlInWinForm("Use Deadly Poison", "UseDeadlyPoison", "Rogue Buffs");
            AddControlInWinForm("Use Leeching Poison", "UseLeechingPoison", "Rogue Buffs");
            AddControlInWinForm("Use Mindnumbing Poison", "UseMindnumbingPoison", "Rogue Buffs");
            AddControlInWinForm("Use Paralytic Poison", "UseParalyticPoison", "Rogue Buffs");
            AddControlInWinForm("Use Slice and Dice", "UseSliceandDice", "Rogue Buffs");
            AddControlInWinForm("Use Sprint", "UseSprint", "Rogue Buffs");
            AddControlInWinForm("Use Stealth", "UseStealth", "Rogue Buffs");
            AddControlInWinForm("Use Wound Poison", "UseWoundPoison", "Rogue Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Ambush", "UseAmbush", "Offensive Spell");
            AddControlInWinForm("Use Crimson Tempest", "UseCrimsonTempest", "Offensive Spell");
            AddControlInWinForm("Use Deadly Throw", "UseDeadlyThrow", "Offensive Spell");
            AddControlInWinForm("Use Dispatch", "UseDispatch", "Offensive Spell");
            AddControlInWinForm("Use Envenom", "UseEnvenom", "Offensive Spell");
            AddControlInWinForm("Use Expose Armor", "UseExposeArmor", "Offensive Spell");
            AddControlInWinForm("Use Fan of Knives", "UseFanofKnives", "Offensive Spell");
            AddControlInWinForm("Use Garrote", "UseGarrote", "Offensive Spell");
            AddControlInWinForm("Use Mutilate", "UseMutilate", "Offensive Spell");
            AddControlInWinForm("Use Rupture", "UseRupture", "Offensive Spell");
            AddControlInWinForm("Use Shiv", "UseShiv", "Offensive Spell");
            AddControlInWinForm("Use Shuriken Toss", "UseShurikenToss", "Offensive Spell");
            AddControlInWinForm("Use Throw", "UseThrow", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use Redirect", "UseRedirect", "Offensive Cooldown");
            AddControlInWinForm("Use Shadow Blades", "UseShadowBlades", "Offensive Cooldown");
            AddControlInWinForm("Use Shadow Step", "UseShadowStep", "Offensive Cooldown");
            AddControlInWinForm("Use Vendetta", "UseVendetta", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use CheapShot", "UseCheapShot", "Defensive Cooldown");
            AddControlInWinForm("Use CloakofShadows", "UseCloakofShadows", "Defensive Cooldown");
            AddControlInWinForm("Use CombatReadiness", "UseCombatReadiness", "Defensive Cooldown");
            AddControlInWinForm("Use Dismantle", "UseDismantle", "Defensive Cooldown");
            AddControlInWinForm("Use Evasion", "UseEvasion", "Defensive Cooldown");
            AddControlInWinForm("Use Kick", "UseKick", "Defensive Cooldown");
            AddControlInWinForm("Use KidneyShot", "UseKidneyShot", "Defensive Cooldown");
            AddControlInWinForm("Use Preparation", "UsePreparation", "Defensive Cooldown");
            AddControlInWinForm("Use SmokeBomb", "UseSmokeBomb", "Defensive Cooldown");
            AddControlInWinForm("Use Vanish", "UseVanish", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Recuperate", "UseRecuperate", "Healing Spell");
            /* Game Settings */
            AddControlInWinForm("Use Low Combat Settings", "UseLowCombat", "Game Settings");
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");

            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
            AddControlInWinForm("Use Shadow Focus Talent?", "UseShadowFocus", "Game Settings");
            AddControlInWinForm("Do avoid melee (Off Advised!!)", "DoAvoidMelee", "Game Settings");
            AddControlInWinForm("Avoid melee distance (1 to 4)", "DoAvoidMeleeDistance", "Game Settings");
        }

        public static RogueAssassinationSettings CurrentSetting { get; set; }

        public static RogueAssassinationSettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Rogue_Assassination.xml";
            if (File.Exists(currentSettingsFile))
            {
                return
                    CurrentSetting = Load<RogueAssassinationSettings>(currentSettingsFile);
            }
            return new RogueAssassinationSettings();
        }
    }

    #endregion
}

#endregion

// ReSharper restore ObjectCreationAsStatement
// ReSharper restore EmptyGeneralCatchClause