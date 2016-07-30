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

    #region Hunter Buffs

    public readonly Spell AspectoftheHawk = new Spell("Aspect of the Hawk");
    public readonly Spell Camouflage = new Spell("Camouflage");
    public readonly Spell FeignDeath = new Spell("Feign Death");
    public readonly Spell HuntersMark = new Spell("Hunter's Mark");
    public readonly Spell Misdirection = new Spell("Misdirection");
    public readonly Spell SteadyFocus = new Spell("Steady Focus");

    #endregion

    #region Offensive Spell

    public readonly Spell AimedShot = new Spell("Aimed Shot");
    public readonly Spell ArcaneShot = new Spell("Arcane Shot");
    public readonly Spell CallPet1 = new Spell("Call Pet 1");
    public readonly Spell CallPet2 = new Spell("Call Pet 2");
    public readonly Spell CallPet3 = new Spell("Call Pet 3");
    public readonly Spell CallPet4 = new Spell("Call Pet 4");
    public readonly Spell CallPet5 = new Spell("Call Pet 5");
    public readonly Spell ChimeraShot = new Spell("Chimera Shot");
    public readonly Spell Dismiss = new Spell("Dismiss Pet");
    public readonly Spell ExplosiveTrap = new Spell("Explosive Trap");
    public readonly Spell KillShot = new Spell("Kill Shot");
    public readonly Spell MultiShot = new Spell("Multi-Shot");
    public readonly Spell SerpentSting = new Spell("Serpent Sting");
    public readonly Spell SteadyShot = new Spell("Steady Shot");
/*
        private Timer _serpentStingTimer = new Timer(0);
*/

    #endregion

    #region Offensive Cooldown

    public readonly Spell AMurderofCrows = new Spell("A Murder of Crows");
    public readonly Spell Barrage = new Spell("Barrage");
    public readonly Spell BlinkStrike = new Spell("Blink Strike");
    public readonly Spell DireBeast = new Spell("Dire Beast");
    public readonly Spell Fervor = new Spell("Fervor");
    public readonly Spell GlaiveToss = new Spell("Glaive Toss");
    public readonly Spell LynxRush = new Spell("Lynx Rush");
    public readonly Spell Powershot = new Spell("Powershot");
    public readonly Spell RapidFire = new Spell("Rapid Fire");
    public readonly Spell Readiness = new Spell("Readiness");
    public readonly Spell Stampede = new Spell("Stampede");
    private Timer _direBeastTimer = new Timer(0);

    #endregion

    #region Defensive Cooldown

    public readonly Spell BindingShot = new Spell("Binding Shot");
    public readonly Spell ConcussiveShot = new Spell("Concussive Shot");
    public readonly Spell Deterrance = new Spell("Deterrance");
    public readonly Spell Disengage = new Spell("Disengage");
    public readonly Spell FreezingTrap = new Spell("Freezing Trap");
    public readonly Spell IceTrap = new Spell("Ice Trap");
    public readonly Spell ScatterShot = new Spell("Scatter Shot");
    public readonly Spell SilencingShot = new Spell("Silencing Shot");
    public readonly Spell WyvernSting = new Spell("Wyvern Sting");

    #endregion

    #region Healing Spell

    public readonly Spell Exhilaration = new Spell("Exhilaration");
    public readonly Spell FeedPet = new Spell("Feed Pet");
    public readonly Spell MendPet = new Spell("Mend Pet");
    public readonly Spell RevivePet = new Spell("Revive Pet");
    private Timer _mendPetTimer = new Timer(0);

    #endregion

    public HunterMarksmanship()
    {
        Main.InternalRange = 39f;
        Main.InternalAggroRange = 39f;
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
                            if (ObjectManager.Me.Target != lastTarget
                                && ArcaneShot.IsHostileDistanceGood)
                            {
                                Pull();
                                lastTarget = ObjectManager.Me.Target;
                            }

                            if (ObjectManager.Target.Level < 70 && ObjectManager.Me.Level > 84
                                && MySettings.UseLowCombat)
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

    private void Pull()
    {
        if (HuntersMark.KnownSpell && HuntersMark.IsSpellUsable && MySettings.UseHuntersMark
            && HuntersMark.IsHostileDistanceGood && !HuntersMark.TargetHaveBuff && LC != 1)
            HuntersMark.Cast();

        if (ObjectManager.Pet.IsAlive)
        {
            Lua.RunMacroText("/petattack");
            Logging.WriteFight("Cast Pet Attack");
        }

        if (ObjectManager.Pet.IsAlive && MySettings.UseMisdirection && Misdirection.KnownSpell
            && Misdirection.IsSpellUsable)
        {
            Misdirection.CastOnUnitID("pet");
        }

        if (SerpentSting.KnownSpell && SerpentSting.IsSpellUsable && SerpentSting.IsHostileDistanceGood
            && MySettings.UseSerpentSting)
        {
            SerpentSting.Cast();
        }
    }

    private void LowCombat()
    {
        Buff();
        if (MySettings.DoAvoidMelee)
            AvoidMelee();
        DefenseCycle();
        Heal();

        if (GlaiveToss.KnownSpell && GlaiveToss.IsSpellUsable && GlaiveToss.IsHostileDistanceGood
            && MySettings.UseGlaiveToss)
        {
            GlaiveToss.Cast();
            return;
        }
        if (ArcaneShot.IsSpellUsable && ArcaneShot.IsHostileDistanceGood && ArcaneShot.KnownSpell
            && MySettings.UseArcaneShot)
        {
            ArcaneShot.Cast();
            return;
        }
        if (SteadyShot.KnownSpell && SteadyShot.IsSpellUsable && SteadyShot.IsHostileDistanceGood
            && MySettings.UseSteadyShot)
        {
            SteadyShot.Cast();
            return;
        }
        if (ExplosiveTrap.KnownSpell && ExplosiveTrap.IsSpellUsable && ExplosiveTrap.IsHostileDistanceGood
            && MySettings.UseExplosiveTrap)
        {
            ExplosiveTrap.Cast();
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

        Pet();

        if (MySettings.UseAspectoftheHawk && AspectoftheHawk.KnownSpell && AspectoftheHawk.IsSpellUsable
            && !AspectoftheHawk.HaveBuff && !ObjectManager.Me.HaveBuff(109260))
        {
            AspectoftheHawk.Cast();
            return;
        }

        if (MySettings.UseCamouflage && Camouflage.KnownSpell && Camouflage.IsSpellUsable && !Camouflage.HaveBuff
            && !ObjectManager.Me.InCombat)
        {
            Camouflage.Cast();
            return;
        }

        if (MySettings.UseAlchFlask && !ObjectManager.Me.HaveBuff(79638) && !ObjectManager.Me.HaveBuff(79640) && !ObjectManager.Me.HaveBuff(79639)
            && !ItemsManager.IsItemOnCooldown(75525) && ItemsManager.GetItemCount(75525) > 0)
            ItemsManager.UseItem(75525);
    }

    private void DismissPet()
    {
        if (MySettings.DismissOnCall)
        {
            if (!ObjectManager.Me.IsCast && (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0 || !ObjectManager.Pet.IsValid) && Dismiss.KnownSpell && Dismiss.IsSpellUsable)
            {
                Dismiss.Cast();
                Others.SafeSleep(1500);
            }
        }
    }

    private void Pet()
    {
        if (MountTask.JustDismounted())
            return;
        if (MySettings.UsePet1 && !ObjectManager.Me.IsCast && (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0 || !ObjectManager.Pet.IsValid) && CallPet1.KnownSpell && CallPet1.IsSpellUsable)
        {
            DismissPet();
            CallPet1.Cast();
            Others.SafeSleep(1000);
        }
        else if (MySettings.UsePet2 && !ObjectManager.Me.IsCast && (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0 || !ObjectManager.Pet.IsValid) && CallPet2.KnownSpell && CallPet2.IsSpellUsable)
        {
            DismissPet();
            CallPet2.Cast();
            Others.SafeSleep(1000);
        }
        else if (MySettings.UsePet3 && !ObjectManager.Me.IsCast && (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0 || !ObjectManager.Pet.IsValid) && CallPet3.KnownSpell && CallPet3.IsSpellUsable)
        {
            DismissPet();
            CallPet3.Cast();
            Others.SafeSleep(1000);
        }
        else if (MySettings.UsePet4 && !ObjectManager.Me.IsCast && (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0 || !ObjectManager.Pet.IsValid) && CallPet4.KnownSpell && CallPet4.IsSpellUsable)
        {
            DismissPet();
            CallPet4.Cast();
            Others.SafeSleep(1000);
        }
        else
        {
            if (MySettings.UsePet5 && !ObjectManager.Me.IsCast && (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0 || !ObjectManager.Pet.IsValid) && CallPet5.KnownSpell && CallPet5.IsSpellUsable)
            {
                DismissPet();
                CallPet5.Cast();
                Others.SafeSleep(1000);
            }
        }

        if (!ObjectManager.Me.IsCast && (!ObjectManager.Pet.IsAlive || ObjectManager.Pet.Guid == 0)
            && RevivePet.KnownSpell && RevivePet.IsSpellUsable && MySettings.UseRevivePet
            && MySettings.UseCombatRevive && ObjectManager.Target.HealthPercent > 10 && ObjectManager.Me.InCombat)
        {
            RevivePet.Cast();
            Others.SafeSleep(1000);
            return;
        }
        if (!ObjectManager.Me.IsCast && !ObjectManager.Pet.IsAlive
            && RevivePet.KnownSpell && RevivePet.IsSpellUsable && MySettings.UseRevivePet
            && !ObjectManager.Me.InCombat)
        {
            RevivePet.Cast();
            Others.SafeSleep(1000);
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
        if (ObjectManager.Me.HealthPercent < 20 && MySettings.UseFeignDeath
            && FeignDeath.KnownSpell && FeignDeath.IsSpellUsable)
        {
            FeignDeath.Cast();
            Others.SafeSleep(5000);
            if (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0)
                return;
            Others.SafeSleep(5000);
            return;
        }
        if (ObjectManager.Me.HealthPercent < 50 && MySettings.UseDeterrance
            && Deterrance.KnownSpell && Deterrance.IsSpellUsable)
        {
            Deterrance.Cast();
            Others.SafeSleep(200);
            return;
        }
        if (MySettings.UseFreezingTrap && ObjectManager.GetNumberAttackPlayer() > 1 && FreezingTrap.KnownSpell
            && FreezingTrap.IsSpellUsable && ObjectManager.Target.GetDistance > 10)
        {
            FreezingTrap.Cast();
            return;
        }
        if (ObjectManager.Me.HealthPercent < 80 && MySettings.UseIceTrap
            && IceTrap.KnownSpell && IceTrap.IsSpellUsable && ObjectManager.Target.GetDistance < 10
            && Disengage.KnownSpell && Disengage.IsSpellUsable && MySettings.UseDisengage)
        {
            IceTrap.Cast();
            Others.SafeSleep(1000);
            MovementsAction.Jump();
            Disengage.Cast();
            return;
        }
        if (ObjectManager.Me.HealthPercent < 80 && MySettings.UseConcussiveShot
            && ConcussiveShot.KnownSpell && ConcussiveShot.IsSpellUsable && ConcussiveShot.IsHostileDistanceGood
            && Disengage.KnownSpell && Disengage.IsSpellUsable && MySettings.UseDisengage)
        {
            ConcussiveShot.Cast();
            Others.SafeSleep(1000);
            MovementsAction.Jump();
            Disengage.Cast();
            return;
        }
        if (ObjectManager.Me.HealthPercent < 80 && MySettings.UseBindingShot
            && BindingShot.KnownSpell && BindingShot.IsSpellUsable && BindingShot.IsHostileDistanceGood
            && Disengage.KnownSpell && Disengage.IsSpellUsable && MySettings.UseDisengage)
        {
            BindingShot.Cast();
            Others.SafeSleep(1000);
            MovementsAction.Jump();
            Disengage.Cast();
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
        if (Exhilaration.KnownSpell && Exhilaration.IsSpellUsable
            && MySettings.UseExhilaration && ObjectManager.Me.HealthPercent < 70)
        {
            Exhilaration.Cast();
            return;
        }
        if (ObjectManager.Pet.Health > 0 && ObjectManager.Pet.HealthPercent < 50
            && FeedPet.KnownSpell && FeedPet.IsSpellUsable && MySettings.UseFeedPet
            && !ObjectManager.Me.InCombat)
        {
            FeedPet.Cast();
            return;
        }
        if (ObjectManager.Pet.Health > 0 && ObjectManager.Pet.HealthPercent < 80
            && MendPet.KnownSpell && MendPet.IsSpellUsable && MySettings.UseMendPet
            && _mendPetTimer.IsReady)
        {
            MendPet.Cast();
            _mendPetTimer = new Timer(1000*10);
        }
    }

    private void Decast()
    {
        if (ArcaneTorrent.IsSpellUsable && ArcaneTorrent.KnownSpell && ObjectManager.Target.GetDistance < 8
            && ObjectManager.Me.HealthPercent <= MySettings.UseArcaneTorrentForDecastAtPercentage
            && MySettings.UseArcaneTorrentForDecast && ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe)
        {
            ArcaneTorrent.Cast();
            return;
        }
        if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && SilencingShot.IsHostileDistanceGood
            && SilencingShot.KnownSpell && SilencingShot.IsSpellUsable && MySettings.UseSilencingShot)
        {
            SilencingShot.Cast();
            return;
        }
        if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && ScatterShot.IsHostileDistanceGood
            && ScatterShot.KnownSpell && ScatterShot.IsSpellUsable && MySettings.UseScatterShot)
        {
            ScatterShot.Cast();
            return;
        }
        if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && MySettings.UseWyvernSting
            && WyvernSting.KnownSpell && WyvernSting.IsSpellUsable && WyvernSting.IsHostileDistanceGood)
        {
            WyvernSting.Cast();
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
        if (AimedShot.KnownSpell && AimedShot.IsSpellUsable && AimedShot.IsHostileDistanceGood
            && MySettings.UseAimedShot && ObjectManager.Me.HaveBuff(82926))
        {
            AimedShot.Cast();
            return;
        }
        if (MySettings.UseSteadyShot && SteadyShot.KnownSpell && SteadyShot.IsHostileDistanceGood && SteadyShot.IsSpellUsable
            && SteadyFocus.KnownSpell && !ObjectManager.Me.HaveBuff(53220))
        {
            SteadyShot.Cast();
            return;
        }
        if (AMurderofCrows.KnownSpell && AMurderofCrows.IsSpellUsable && AMurderofCrows.IsHostileDistanceGood
            && MySettings.UseAMurderofCrows && !AMurderofCrows.TargetHaveBuff)
        {
            AMurderofCrows.Cast();
            return;
        }
        if (Barrage.KnownSpell && Barrage.IsSpellUsable && MySettings.UseBarrage && Barrage.IsHostileDistanceGood)
        {
            Barrage.Cast();
            return;
        }
        if (BlinkStrike.KnownSpell && BlinkStrike.IsSpellUsable && ObjectManager.Pet.IsAlive
            && MySettings.UseBlinkStrike && ObjectManager.Target.GetDistance <= 40f)
        {
            BlinkStrike.Cast();
            return;
        }
        if (DireBeast.KnownSpell && DireBeast.IsSpellUsable && MySettings.UseDireBeast
            && DireBeast.IsHostileDistanceGood && _direBeastTimer.IsReady)
        {
            DireBeast.Cast();
            _direBeastTimer = new Timer(1000*15);
            return;
        }
        if (Fervor.KnownSpell && Fervor.IsSpellUsable && ObjectManager.Me.Focus < 50
            && MySettings.UseFervor)
        {
            Fervor.Cast();
            return;
        }
        if (GlaiveToss.KnownSpell && GlaiveToss.IsSpellUsable && MySettings.UseGlaiveToss &&
            GlaiveToss.IsHostileDistanceGood)
        {
            GlaiveToss.Cast();
            return;
        }
        if (LynxRush.KnownSpell && LynxRush.IsSpellUsable && MySettings.UseLynxRush &&
            ObjectManager.Target.GetDistance <= 40f)
        {
            LynxRush.Cast();
            return;
        }
        if (Powershot.KnownSpell && Powershot.IsSpellUsable && MySettings.UsePowershot &&
            Powershot.IsHostileDistanceGood)
        {
            Powershot.Cast();
            return;
        }
        if (Stampede.KnownSpell && Stampede.IsSpellUsable && MySettings.UseStampede &&
            Stampede.IsHostileDistanceGood)
        {
            Stampede.Cast();
            return;
        }
        if (RapidFire.KnownSpell && RapidFire.IsSpellUsable && MySettings.UseRapidFire
            && ObjectManager.Target.GetDistance <= 40f)
        {
            RapidFire.Cast();
            return;
        }
        if (Readiness.KnownSpell && Readiness.IsSpellUsable && MySettings.UseReadiness)
        {
            Readiness.Cast();
        }
    }

    private void DPSCycle()
    {
        Usefuls.SleepGlobalCooldown();
        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            if (SerpentSting.IsSpellUsable && SerpentSting.IsHostileDistanceGood && SerpentSting.KnownSpell
                && MySettings.UseSerpentSting && !SerpentSting.TargetHaveBuff)
            {
                SerpentSting.Cast();
                //_serpentStingTimer = new Timer(1000*12);
                return;
            }
            if (ChimeraShot.KnownSpell && ChimeraShot.IsSpellUsable && ChimeraShot.IsHostileDistanceGood
                && MySettings.UseChimeraShot)
            {
                ChimeraShot.Cast();
                //_serpentStingTimer = new Timer(1000*12);
                return;
            }
            if (KillShot.KnownSpell && KillShot.IsSpellUsable && KillShot.IsHostileDistanceGood
                && MySettings.UseKillShot)
            {
                KillShot.Cast();
                return;
            }
            if (AimedShot.KnownSpell && AimedShot.IsSpellUsable && AimedShot.IsHostileDistanceGood
                && MySettings.UseAimedShot && ObjectManager.Me.HaveBuff(82926))
            {
                AimedShot.Cast();
                return;
            }
            if (MySettings.UseSteadyShot && SteadyShot.KnownSpell && SteadyShot.IsHostileDistanceGood && SteadyShot.IsSpellUsable
                && SteadyFocus.KnownSpell && !ObjectManager.Me.HaveBuff(53220))
            {
                SteadyShot.Cast();
                return;
            }
            if (ObjectManager.GetNumberAttackPlayer() > 3 && MySettings.UseMultiShot && MultiShot.KnownSpell && MultiShot.IsHostileDistanceGood
                && MultiShot.IsSpellUsable)
            {
                MultiShot.Cast();
                return;
            }
            if (ObjectManager.GetNumberAttackPlayer() > 3 && MySettings.UseMultiShot && MySettings.UseSteadyShot && SteadyShot.KnownSpell && SteadyShot.IsHostileDistanceGood &&
                SteadyShot.IsSpellUsable && ObjectManager.Me.FocusPercentage < 40)
            {
                SteadyShot.Cast();
                return;
            }
            if (ArcaneShot.KnownSpell && ArcaneShot.IsSpellUsable && ArcaneShot.IsHostileDistanceGood && MySettings.UseArcaneShot
                && (ObjectManager.Me.FocusPercentage > 64 || ObjectManager.Me.Level < 3))
            {
                ArcaneShot.Cast();
                return;
            }
            if (ArcaneTorrent.IsSpellUsable && ArcaneTorrent.KnownSpell && MySettings.UseArcaneTorrentForResource)
            {
                ArcaneTorrent.Cast();
                return;
            }
            if (SteadyShot.KnownSpell && SteadyShot.IsSpellUsable && SteadyShot.IsHostileDistanceGood
                && MySettings.UseSteadyShot && ObjectManager.Me.FocusPercentage < 80)
            {
                SteadyShot.Cast();
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

    #region Nested type: HunterMarksmanshipSettings

    [Serializable]
    public class HunterMarksmanshipSettings : Settings
    {
        public bool DismissOnCall = true;
        public bool DoAvoidMelee = false;
        public int DoAvoidMeleeDistance = 0;
        public bool UseAMurderofCrows = true;
        public bool UseAimedShot = true;
        public bool UseAlchFlask = true;
        public bool UseArcaneShot = true;
        public bool UseArcaneTorrentForDecast = true;
        public int UseArcaneTorrentForDecastAtPercentage = 100;
        public bool UseArcaneTorrentForResource = true;
        public bool UseAspectoftheHawk = true;
        public bool UseBarrage = true;
        public bool UseBerserking = true;
        public bool UseBindingShot = true;
        public bool UseBlinkStrike = true;
        public bool UseBloodFury = true;
        public bool UseCamouflage = false;
        public bool UseChimeraShot = true;
        public bool UseCombatRevive = true;
        public bool UseConcussiveShot = true;
        public bool UseDeterrance = true;
        public bool UseDireBeast = true;
        public bool UseDisengage = true;

        public bool UseExhilaration = true;
        public bool UseExplosiveTrap = true;
        public bool UseFeedPet = true;
        public bool UseFeignDeath = true;
        public bool UseFervor = true;
        public bool UseFreezingTrap = true;
        public bool UseGiftoftheNaaru = true;
        public int UseGiftoftheNaaruAtPercentage = 80;
        public bool UseGlaiveToss = true;
        public bool UseHuntersMark = true;
        public bool UseIceTrap = true;
        public bool UseKillShot = true;

        public bool UseLowCombat = true;
        public bool UseLynxRush = true;
        public bool UseMendPet = true;
        public bool UseMisdirection = true;
        public bool UseMultiShot = true;
        public bool UsePet1 = true;
        public bool UsePet2 = false;
        public bool UsePet3 = false;
        public bool UsePet4 = false;
        public bool UsePet5 = false;
        public bool UsePowershot = true;
        public bool UseRapidFire = true;
        public bool UseReadiness = true;
        public bool UseRevivePet = true;
        public bool UseScatterShot = true;
        public bool UseSerpentSting = true;
        public bool UseSilencingShot = true;
        public bool UseStampede = true;
        public bool UseSteadyShot = true;
        public bool UseStoneform = true;
        public int UseStoneformAtPercentage = 80;
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;
        public bool UseWarStomp = true;
        public int UseWarStompAtPercentage = 80;
        public bool UseWyvernSting = true;

        public HunterMarksmanshipSettings()
        {
            ConfigWinForm("Hunter Marksmanship Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent for Interrupt", "UseArcaneTorrentForDecast", "Professions & Racials", "AtPercentage");
            AddControlInWinForm("Use Arcane Torrent for Resource", "UseArcaneTorrentForResource", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");

            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            /* Hunter Buffs */
            AddControlInWinForm("Use Aspect of the Hawk", "UseAspectoftheHawk", "Hunter Buffs");
            AddControlInWinForm("Use Camouflage", "UseCamouflage", "Hunter Buffs");
            AddControlInWinForm("Use Feign Death", "UseFeignDeath", "Hunter Buffs");
            AddControlInWinForm("Use Hunter's Mark", "UseHuntersMark", "Hunter Buffs");
            AddControlInWinForm("Use Misdirection", "UseMisdirection", "Hunter Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Aimed Shot", "UseAimedShot", "Offensive Spell");
            AddControlInWinForm("Use Arcane Shot", "UseArcaneShot", "Offensive Spell");
            AddControlInWinForm("Dismiss pet before calling again", "DismissOnCall", "Offensive Spell");
            AddControlInWinForm("Use Pet in Slot 1", "UsePet1", "Offensive Spell");
            AddControlInWinForm("Use Pet in Slot 2", "UsePet2", "Offensive Spell");
            AddControlInWinForm("Use Pet in Slot 3", "UsePet3", "Offensive Spell");
            AddControlInWinForm("Use Pet in Slot 4", "UsePet4", "Offensive Spell");
            AddControlInWinForm("Use Pet in Slot 5", "UsePet5", "Offensive Spell");
            AddControlInWinForm("Use Chimera Shot", "UseChimeraShot", "Offensive Spell");
            AddControlInWinForm("Use Explosive Trap", "UseExplosiveTrap", "Offensive Spell");
            AddControlInWinForm("Use KillShot", "UseKillShot", "Offensive Spell");
            AddControlInWinForm("Use Multi-Shot", "UseMultiShot", "Offensive Spell");
            AddControlInWinForm("Use Serpent Sting", "UseSerpentSting", "Offensive Spell");
            AddControlInWinForm("Use Steady Shot", "UseSteadyShot", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use A Murder of Crows", "UseAMurderofCrows", "Offensive Cooldown");
            AddControlInWinForm("Use Barrage", "UseBarrage", "Offensive Cooldown");
            AddControlInWinForm("Use Blink Strike", "UseBlinkStrike", "Offensive Cooldown");
            AddControlInWinForm("Use Dire Beast", "UseDireBeast", "Offensive Cooldown");
            AddControlInWinForm("Use Fervor", "UseFervor", "Offensive Cooldown");
            AddControlInWinForm("Use Glaive Toss", "UseGlaiveToss", "Offensive Cooldown");
            AddControlInWinForm("Use Lynx Rush", "UseLynxRush", "Offensive Cooldown");
            AddControlInWinForm("Use Powershot", "UsePowershot", "Offensive Cooldown");
            AddControlInWinForm("Use Rapid Fire", "UseRapidFire", "Offensive Cooldown");
            AddControlInWinForm("Use Readiness", "UseReadiness", "Offensive Cooldown");
            AddControlInWinForm("Use Stampede", "UseStampede", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Binding Shot", "UseBindingShot", "Defensive Cooldown");
            AddControlInWinForm("Use Concussive Shot", "UseConcussiveShot", "Defensive Cooldown");
            AddControlInWinForm("Use Deterrance", "UseDeterrance", "Defensive Cooldown");
            AddControlInWinForm("Use Disengage", "UseDisengage", "Defensive Cooldown");
            AddControlInWinForm("Use Freezing Trap", "UseFreezingTrap", "Defensive Cooldown");
            AddControlInWinForm("Use Ice Trap", "UseIceTrap", "Defensive Cooldown");
            AddControlInWinForm("Use Scatter Shot", "UseScatterShot", "Defensive Cooldown");
            AddControlInWinForm("Use Silencing Shot", "UseSilencingShot", "Defensive Cooldown");
            AddControlInWinForm("Use Wyvern Sting", "UseWyvernSting", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Exhilaration", "UseExhilaration", "Healing Spell");
            AddControlInWinForm("Use Feed Pet", "UseFeedPet", "Healing Spell");
            AddControlInWinForm("Use Mend Pet", "UseMendPet", "Healing Spell");
            AddControlInWinForm("Use Revive Pet", "UseRevivePet", "Healing Spell");
            /* Game Settings */
            AddControlInWinForm("Use Low Combat Settings", "UseLowCombat", "Game Settings");
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");

            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
            AddControlInWinForm("Use Revive Pet in Combat", "UseCombatRevive", "Game Settings");
            AddControlInWinForm("Do avoid melee (Off Advised!!)", "DoAvoidMelee", "Game Settings");
            AddControlInWinForm("Avoid melee distance (1 to 4)", "DoAvoidMeleeDistance", "Game Settings");
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
    public int LC = 0;
    private Timer _ancientHysteriaTimer = new Timer(0);
    private Timer _burrowAttackTimer = new Timer(0);

    private Timer _froststormBreathTimer = new Timer(0);
    private Timer _onCd = new Timer(0);
    private Timer _spiritMendTimer = new Timer(0);

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

    #region Hunter Buffs

    public readonly Spell Camouflage = new Spell("Camouflage");
    public readonly Spell FeignDeath = new Spell("Feign Death");
    public readonly Spell HuntersMark = new Spell("Hunter's Mark");
    public readonly Spell Misdirection = new Spell("Misdirection");

    #endregion

    #region Offensive Spell

    public readonly Spell ArcaneShot = new Spell("Arcane Shot");
    public readonly Spell CallPet1 = new Spell("Call Pet 1");
    public readonly Spell CallPet2 = new Spell("Call Pet 2");
    public readonly Spell CallPet3 = new Spell("Call Pet 3");
    public readonly Spell CallPet4 = new Spell("Call Pet 4");
    public readonly Spell CallPet5 = new Spell("Call Pet 5");
    public readonly Spell CobraShot = new Spell("Cobra Shot");
    public readonly Spell Dismiss = new Spell("Dismiss Pet");
    public readonly Spell ExplosiveTrap = new Spell("Explosive Trap");
    public readonly Spell KillCommand = new Spell("Kill Command");
    public readonly Spell KillShot = new Spell("Kill Shot");
    public readonly Spell MultiShot = new Spell("Multi-Shot");
    public readonly Spell SteadyShot = new Spell("Steady Shot");

    #endregion

    #region Offensive Cooldown

    public readonly Spell AMurderofCrows = new Spell("A Murder of Crows");
    public readonly Spell Barrage = new Spell("Barrage");
    public readonly Spell BestialWrath = new Spell("Bestial Wrath");
    public readonly Spell BlinkStrike = new Spell("Blink Strike");
    public readonly Spell DireBeast = new Spell("Dire Beast");
    public readonly Spell Fervor = new Spell("Fervor");
    public readonly Spell FocusFire = new Spell("Focus Fire");
    public readonly Spell GlaiveToss = new Spell("Glaive Toss");
    public readonly Spell LynxRush = new Spell("Lynx Rush");
    public readonly Spell Powershot = new Spell("Powershot");
    public readonly Spell Stampede = new Spell("Stampede");
    private Timer _direBeastTimer = new Timer(0);

    #endregion

    #region Defensive Cooldown

    public readonly Spell BindingShot = new Spell("Binding Shot");
    public readonly Spell ConcussiveShot = new Spell("Concussive Shot");
    public readonly Spell Deterrance = new Spell("Deterrance");
    public readonly Spell Disengage = new Spell("Disengage");
    public readonly Spell FreezingTrap = new Spell("Freezing Trap");
    public readonly Spell IceTrap = new Spell("Ice Trap");
    public readonly Spell Intimidation = new Spell("Intimidation");
    public readonly Spell ScatterShot = new Spell("Scatter Shot");
    public readonly Spell SilencingShot = new Spell("Silencing Shot");

    #endregion

    #region Healing Spell

    public readonly Spell Exhilaration = new Spell("Exhilaration");
    public readonly Spell FeedPet = new Spell("Feed Pet");
    public readonly Spell MendPet = new Spell("Mend Pet");
    public readonly Spell RevivePet = new Spell("Revive Pet");
    private Timer _mendPetTimer = new Timer(0);

    #endregion

    public HunterBeastMastery()
    {
        Main.InternalRange = 39f;
        Main.InternalAggroRange = 39f;
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
                            if (ObjectManager.Me.Target != lastTarget
                                && ArcaneShot.IsHostileDistanceGood)
                            {
                                Pull();
                                lastTarget = ObjectManager.Me.Target;
                            }

                            if (ObjectManager.Target.Level < 70 && ObjectManager.Me.Level > 84
                                && MySettings.UseLowCombat)
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

    private void Pull()
    {
        if (HuntersMark.KnownSpell && HuntersMark.IsSpellUsable && MySettings.UseHuntersMark
            && HuntersMark.IsHostileDistanceGood && !HuntersMark.TargetHaveBuff && LC != 1)
            HuntersMark.Cast();

        if (ObjectManager.Pet.IsAlive)
        {
            Lua.RunMacroText("/petattack");
            Logging.WriteFight("Cast Pet Attack");
        }

        if (ObjectManager.Pet.IsAlive && MySettings.UseMisdirection && Misdirection.KnownSpell
            && Misdirection.IsSpellUsable)
        {
            Misdirection.CastOnUnitID("pet");
        }
    }

    private void LowCombat()
    {
        Buff();
        if (MySettings.DoAvoidMelee)
            AvoidMelee();
        DefenseCycle();
        Heal();

        if (GlaiveToss.KnownSpell && MySettings.UseGlaiveToss && GlaiveToss.IsHostileDistanceGood
            && GlaiveToss.IsSpellUsable)
        {
            GlaiveToss.Cast();
            return;
        }
        if (ArcaneShot.KnownSpell && MySettings.UseArcaneShot && ArcaneShot.IsHostileDistanceGood
            && ArcaneShot.IsSpellUsable)
        {
            ArcaneShot.Cast();
            return;
        }
        if (ExplosiveTrap.KnownSpell && MySettings.UseExplosiveTrap && ExplosiveTrap.IsHostileDistanceGood
            && ExplosiveTrap.IsSpellUsable)
        {
            ExplosiveTrap.Cast();
            return;
        }
        if (CobraShot.KnownSpell && MySettings.UseCobraShot && CobraShot.IsHostileDistanceGood
            && CobraShot.IsSpellUsable)
        {
            CobraShot.Cast();
            return;
        }
        if (SteadyShot.KnownSpell && (!CobraShot.KnownSpell || !MySettings.UseCobraShot) && SteadyShot.IsHostileDistanceGood
            && SteadyShot.IsSpellUsable)
        {
            SteadyShot.Cast();
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

        Pet();

        if (MySettings.UseCamouflage && Camouflage.KnownSpell && Camouflage.IsSpellUsable && !Camouflage.HaveBuff
            && !ObjectManager.Me.InCombat)
        {
            Camouflage.Cast();
            return;
        }

        if (MySettings.UseAlchFlask && !ObjectManager.Me.HaveBuff(79638) && !ObjectManager.Me.HaveBuff(79640) && !ObjectManager.Me.HaveBuff(79639)
            && !ItemsManager.IsItemOnCooldown(75525) && ItemsManager.GetItemCount(75525) > 0)
            ItemsManager.UseItem(75525);
    }

    private void DismissPet()
    {
        if (MySettings.DismissOnCall)
        {
            if (!ObjectManager.Me.IsCast && (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0 || !ObjectManager.Pet.IsValid) && Dismiss.KnownSpell && Dismiss.IsSpellUsable)
            {
                Dismiss.Cast();
                Others.SafeSleep(1500);
            }
        }
    }

    private void Pet()
    {
        if (MountTask.JustDismounted())
            return;
        if (MySettings.UsePet1 && !ObjectManager.Me.IsCast && (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0 || !ObjectManager.Pet.IsValid) && CallPet1.KnownSpell && CallPet1.IsSpellUsable)
        {
            DismissPet();
            CallPet1.Cast();
            Others.SafeSleep(1000);
        }
        else if (MySettings.UsePet2 && !ObjectManager.Me.IsCast && (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0 || !ObjectManager.Pet.IsValid) && CallPet2.KnownSpell && CallPet2.IsSpellUsable)
        {
            DismissPet();
            CallPet2.Cast();
            Others.SafeSleep(1000);
        }
        else if (MySettings.UsePet3 && !ObjectManager.Me.IsCast && (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0 || !ObjectManager.Pet.IsValid) && CallPet3.KnownSpell && CallPet3.IsSpellUsable)
        {
            DismissPet();
            CallPet3.Cast();
            Others.SafeSleep(1000);
        }
        else if (MySettings.UsePet4 && !ObjectManager.Me.IsCast && (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0 || !ObjectManager.Pet.IsValid) && CallPet4.KnownSpell && CallPet4.IsSpellUsable)
        {
            DismissPet();
            CallPet4.Cast();
            Others.SafeSleep(1000);
        }
        else
        {
            if (MySettings.UsePet5 && !ObjectManager.Me.IsCast && (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0 || !ObjectManager.Pet.IsValid) && CallPet5.KnownSpell && CallPet5.IsSpellUsable)
            {
                DismissPet();
                CallPet5.Cast();
                Others.SafeSleep(1000);
            }
        }

        if (!ObjectManager.Me.IsCast && (!ObjectManager.Pet.IsAlive || ObjectManager.Pet.Guid == 0)
            && RevivePet.KnownSpell && RevivePet.IsSpellUsable && MySettings.UseRevivePet
            && MySettings.UseCombatRevive && ObjectManager.Target.HealthPercent > 10 && ObjectManager.Me.InCombat)
        {
            RevivePet.Cast();
            Others.SafeSleep(1000);
            return;
        }
        if (!ObjectManager.Me.IsCast && !ObjectManager.Pet.IsAlive
            && RevivePet.KnownSpell && RevivePet.IsSpellUsable && MySettings.UseRevivePet
            && !ObjectManager.Me.InCombat)
        {
            RevivePet.Cast();
            Others.SafeSleep(1000);
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
        if (ObjectManager.Me.HealthPercent < 20 && MySettings.UseFeignDeath
            && FeignDeath.KnownSpell && FeignDeath.IsSpellUsable)
        {
            FeignDeath.Cast();
            Others.SafeSleep(5000);
            if (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0 || !ObjectManager.Pet.IsValid)
                return;
            Others.SafeSleep(5000);
            return;
        }
        if (ObjectManager.Me.HealthPercent < 50 && MySettings.UseDeterrance
            && Deterrance.KnownSpell && Deterrance.IsSpellUsable)
        {
            Deterrance.Cast();
            Others.SafeSleep(200);
            return;
        }
        if (MySettings.UseFreezingTrap && ObjectManager.GetNumberAttackPlayer() > 1 && FreezingTrap.KnownSpell
            && FreezingTrap.IsSpellUsable && ObjectManager.Target.GetDistance > 10)
        {
            FreezingTrap.Cast();
            return;
        }
        if (ObjectManager.Me.HealthPercent < 80 && MySettings.UseIceTrap
            && IceTrap.KnownSpell && IceTrap.IsSpellUsable && ObjectManager.Target.GetDistance < 10
            && Disengage.KnownSpell && Disengage.IsSpellUsable && MySettings.UseDisengage)
        {
            IceTrap.Cast();
            Others.SafeSleep(1000);
            MovementsAction.Jump();
            Disengage.Cast();
            return;
        }
        if (ObjectManager.Me.HealthPercent < 80 && MySettings.UseConcussiveShot
            && ConcussiveShot.KnownSpell && ConcussiveShot.IsSpellUsable && ConcussiveShot.IsHostileDistanceGood
            && Disengage.KnownSpell && Disengage.IsSpellUsable && MySettings.UseDisengage)
        {
            ConcussiveShot.Cast();
            Others.SafeSleep(1000);
            MovementsAction.Jump();
            Disengage.Cast();
            return;
        }
        if (ObjectManager.Me.HealthPercent < 80 && MySettings.UseBindingShot
            && BindingShot.KnownSpell && BindingShot.IsSpellUsable && BindingShot.IsHostileDistanceGood
            && Disengage.KnownSpell && Disengage.IsSpellUsable && MySettings.UseDisengage)
        {
            BindingShot.Cast();
            Others.SafeSleep(1000);
            MovementsAction.Jump();
            Disengage.Cast();
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
        if (Intimidation.IsSpellUsable && Intimidation.KnownSpell && MySettings.UseIntimidation
            && (ObjectManager.Me.HealthPercent < 80 || ObjectManager.Pet.Health < 80))
        {
            Intimidation.Cast();
            _onCd = new Timer(1000*3);
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (ObjectManager.Me.HealthPercent < 85 && ObjectManager.Pet.IsAlive && MySettings.UseSpiritBeastPet && _spiritMendTimer.IsReady)
        {
            Logging.WriteFight("Cast Spirit Mend.");
            Lua.RunMacroText("/target Player");
            Others.SafeSleep(200);
            Lua.RunMacroText("/cast Spirit Mend");
            _spiritMendTimer = new Timer(1000*40);
            return;
        }
        if (ObjectManager.Me.HealthPercent <= MySettings.UseGiftoftheNaaruAtPercentage && GiftoftheNaaru.KnownSpell && GiftoftheNaaru.IsSpellUsable
            && MySettings.UseGiftoftheNaaru)
        {
            GiftoftheNaaru.Cast();
            return;
        }
        if (Exhilaration.KnownSpell && Exhilaration.IsSpellUsable
            && MySettings.UseExhilaration && ObjectManager.Me.HealthPercent < 70)
        {
            Exhilaration.Cast();
            return;
        }
        if (ObjectManager.Pet.Health > 0 && ObjectManager.Pet.HealthPercent < 50
            && FeedPet.KnownSpell && FeedPet.IsSpellUsable && MySettings.UseFeedPet
            && !ObjectManager.Me.InCombat)
        {
            FeedPet.Cast();
            return;
        }
        if (ObjectManager.Pet.Health > 0 && ObjectManager.Pet.HealthPercent < 80
            && MendPet.KnownSpell && MendPet.IsSpellUsable && MySettings.UseMendPet
            && _mendPetTimer.IsReady)
        {
            MendPet.Cast();
            _mendPetTimer = new Timer(1000*10);
        }
    }

    private void Decast()
    {
        if (ArcaneTorrent.IsSpellUsable && ArcaneTorrent.KnownSpell && ObjectManager.Target.GetDistance < 8
            && ObjectManager.Me.HealthPercent <= MySettings.UseArcaneTorrentForDecastAtPercentage
            && MySettings.UseArcaneTorrentForDecast && ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe)
        {
            ArcaneTorrent.Cast();
            return;
        }
        if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && SilencingShot.IsHostileDistanceGood
            && SilencingShot.KnownSpell && SilencingShot.IsSpellUsable && MySettings.UseSilencingShot)
        {
            SilencingShot.Cast();
            return;
        }
        if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && ScatterShot.IsHostileDistanceGood
            && ScatterShot.KnownSpell && ScatterShot.IsSpellUsable && MySettings.UseScatterShot)
        {
            ScatterShot.Cast();
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
        if (Berserking.IsSpellUsable && Berserking.KnownSpell && ObjectManager.Target.GetDistance <= 40f && MySettings.UseBerserking)
        {
            Berserking.Cast();
            return;
        }
        if (BloodFury.IsSpellUsable && BloodFury.KnownSpell && ObjectManager.Target.GetDistance <= 40f && MySettings.UseBloodFury)
        {
            BloodFury.Cast();
            return;
        }
        if (AMurderofCrows.KnownSpell && AMurderofCrows.IsSpellUsable && AMurderofCrows.IsHostileDistanceGood
            && MySettings.UseAMurderofCrows && !AMurderofCrows.TargetHaveBuff)
        {
            AMurderofCrows.Cast();
            return;
        }
        if (Barrage.KnownSpell && Barrage.IsSpellUsable && MySettings.UseBarrage && Barrage.IsHostileDistanceGood)
        {
            Barrage.Cast();
            return;
        }
        if (BlinkStrike.KnownSpell && BlinkStrike.IsSpellUsable && ObjectManager.Pet.IsAlive
            && MySettings.UseBlinkStrike && ObjectManager.Target.GetDistance <= 40f)
        {
            BlinkStrike.Cast();
            return;
        }
        if (DireBeast.KnownSpell && DireBeast.IsSpellUsable && MySettings.UseDireBeast
            && DireBeast.IsHostileDistanceGood && _direBeastTimer.IsReady)
        {
            DireBeast.Cast();
            _direBeastTimer = new Timer(1000*15);
            return;
        }
        if (Fervor.KnownSpell && Fervor.IsSpellUsable && ObjectManager.Me.Focus < 50
            && MySettings.UseFervor)
        {
            Fervor.Cast();
            return;
        }
        if (GlaiveToss.KnownSpell && GlaiveToss.IsSpellUsable && MySettings.UseGlaiveToss &&
            GlaiveToss.IsHostileDistanceGood)
        {
            GlaiveToss.Cast();
            return;
        }
        if (LynxRush.KnownSpell && LynxRush.IsSpellUsable && MySettings.UseLynxRush &&
            ObjectManager.Target.GetDistance <= 40f)
        {
            LynxRush.Cast();
            return;
        }
        if (Powershot.KnownSpell && Powershot.IsSpellUsable && MySettings.UsePowershot &&
            Powershot.IsHostileDistanceGood)
        {
            Powershot.Cast();
            return;
        }
        if (Stampede.KnownSpell && Stampede.IsSpellUsable && MySettings.UseStampede &&
            Stampede.IsHostileDistanceGood)
        {
            Stampede.Cast();
            return;
        }
        if (BestialWrath.KnownSpell && BestialWrath.IsSpellUsable && MySettings.UseBestialWrath
            && ObjectManager.Target.GetDistance <= 40f)
        {
            BestialWrath.Cast();
            return;
        }
        if (MySettings.UseCoreHoundPet && ObjectManager.Target.GetDistance <= 40f
            && _ancientHysteriaTimer.IsReady && ObjectManager.Me.HaveBuff(95809)
            && ObjectManager.Pet.IsAlive && !BestialWrath.HaveBuff)
        {
            Lua.RunMacroText("/cast Ancient Hysteria");
            Logging.WriteFight("Cast Core Hound Pet Ancient Hysteria");
            _ancientHysteriaTimer = new Timer(1000*60*6);
            return;
        }
        if (ObjectManager.Pet.BuffStack(19623) == 5 && FocusFire.IsSpellUsable &&
            FocusFire.KnownSpell
            && MySettings.UseFocusFire)
        {
            FocusFire.Cast();
        }
    }

    private void DPSCycle()
    {
        Usefuls.SleepGlobalCooldown();
        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            if (KillShot.KnownSpell && KillShot.IsSpellUsable && KillShot.IsHostileDistanceGood
                && MySettings.UseKillShot)
            {
                KillShot.Cast();
                return;
            }
            if (ObjectManager.GetNumberAttackPlayer() > 2 && MySettings.UseMultiShot)
            {
                if (MultiShot.KnownSpell && MultiShot.IsSpellUsable && MultiShot.IsHostileDistanceGood)
                {
                    MultiShot.Cast();
                    return;
                }
                if (MySettings.UseChimeraPet && ObjectManager.Target.GetDistance < 10
                    && ObjectManager.Pet.Guid == 780 && ObjectManager.Pet.Focus > 29
                    && _froststormBreathTimer.IsReady && ObjectManager.Pet.IsAlive)
                {
                    Lua.RunMacroText("/cast Froststorm Breath");
                    Logging.WriteFight("Cast Chimera Pet AoE");
                    _froststormBreathTimer = new Timer(1000*8);
                    return;
                }
                if (MySettings.UseWormPet && ObjectManager.Target.GetDistance < 10
                    && ObjectManager.Pet.Guid == 784 && ObjectManager.Pet.Focus > 29
                    && _burrowAttackTimer.IsReady && ObjectManager.Pet.IsAlive)
                {
                    Lua.RunMacroText("/cast Burrow Attack");
                    Logging.WriteFight("Cast Worm Pet AoE");
                    _burrowAttackTimer = new Timer(1000*20);
                }
                return;
            }
            if (KillCommand.KnownSpell && KillCommand.IsSpellUsable && KillCommand.IsHostileDistanceGood
                && MySettings.UseKillCommand && ObjectManager.Target.GetDistance <= 40f)
            {
                KillCommand.Cast();
                return;
            }
            if (ArcaneShot.KnownSpell && ArcaneShot.IsSpellUsable && ArcaneShot.IsHostileDistanceGood
                && MySettings.UseArcaneShot && ObjectManager.Me.FocusPercentage > 59)
            {
                ArcaneShot.Cast();
                return;
            }
            if (ArcaneTorrent.IsSpellUsable && ArcaneTorrent.KnownSpell
                && MySettings.UseArcaneTorrentForResource)
            {
                ArcaneTorrent.Cast();
                return;
            }
            if (CobraShot.KnownSpell && CobraShot.IsSpellUsable && CobraShot.IsHostileDistanceGood
                && MySettings.UseCobraShot && ObjectManager.Me.FocusPercentage < 60)
            {
                CobraShot.Cast();
                return;
            }
            if (SteadyShot.KnownSpell && SteadyShot.IsSpellUsable && SteadyShot.IsHostileDistanceGood && (ObjectManager.Me.FocusPercentage < 60 || !CobraShot.KnownSpell || !MySettings.UseCobraShot))
            {
                SteadyShot.Cast();
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

    #region Nested type: HunterBeastMasterySettings

    [Serializable]
    public class HunterBeastMasterySettings : Settings
    {
        public bool DismissOnCall = true;
        public bool DoAvoidMelee = false;
        public int DoAvoidMeleeDistance = 0;
        public bool UseAMurderofCrows = true;
        public bool UseAlchFlask = true;
        public bool UseArcaneShot = true;
        public bool UseArcaneTorrentForDecast = true;
        public int UseArcaneTorrentForDecastAtPercentage = 100;
        public bool UseArcaneTorrentForResource = true;
        public bool UseBarrage = true;
        public bool UseBerserking = true;
        public bool UseBestialWrath = true;
        public bool UseBindingShot = true;
        public bool UseBlinkStrike = true;
        public bool UseBloodFury = true;
        public bool UseCamouflage = false;
        public bool UseChimeraPet = false;
        public bool UseCobraShot = true;
        public bool UseCombatRevive = true;
        public bool UseConcussiveShot = true;
        public bool UseCoreHoundPet = false;
        public bool UseDeterrance = true;
        public bool UseDireBeast = true;
        public bool UseDisengage = true;

        public bool UseExhilaration = true;
        public bool UseExplosiveTrap = true;
        public bool UseFeedPet = true;
        public bool UseFeignDeath = true;
        public bool UseFervor = true;
        public bool UseFocusFire = false;
        public bool UseFreezingTrap = true;
        public bool UseGiftoftheNaaru = true;
        public int UseGiftoftheNaaruAtPercentage = 80;
        public bool UseGlaiveToss = true;
        public bool UseHuntersMark = true;
        public bool UseIceTrap = true;
        public bool UseIntimidation = true;
        public bool UseKillCommand = true;
        public bool UseKillShot = true;

        public bool UseLowCombat = true;
        public bool UseLynxRush = true;
        public bool UseMendPet = true;
        public bool UseMisdirection = true;
        public bool UseMultiShot = true;
        public bool UsePet1 = true;
        public bool UsePet2 = false;
        public bool UsePet3 = false;
        public bool UsePet4 = false;
        public bool UsePet5 = false;
        public bool UsePowershot = true;
        public bool UseRevivePet = true;
        public bool UseScatterShot = true;
        public bool UseSilencingShot = true;
        public bool UseSpiritBeastPet = false;
        public bool UseStampede = true;
        public bool UseStoneform = true;
        public int UseStoneformAtPercentage = 80;
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;
        public bool UseWarStomp = true;
        public int UseWarStompAtPercentage = 80;
        public bool UseWormPet = false;

        public HunterBeastMasterySettings()
        {
            ConfigWinForm("Hunter BeastMastery Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent for Interrupt", "UseArcaneTorrentForDecast", "Professions & Racials", "AtPercentage");
            AddControlInWinForm("Use Arcane Torrent for Resource", "UseArcaneTorrentForResource", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");

            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            /* Hunter Buffs */
            AddControlInWinForm("Use Camouflage", "UseCamouflage", "Hunter Buffs");
            AddControlInWinForm("Use Feign Death", "UseFeignDeath", "Hunter Buffs");
            AddControlInWinForm("Use Hunter's Mark", "UseHuntersMark", "Hunter Buffs");
            AddControlInWinForm("Use Misdirection", "UseMisdirection", "Hunter Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Arcane Shot", "UseArcaneShot", "Offensive Spell");
            AddControlInWinForm("Dismiss pet before calling again", "DismissOnCall", "Offensive Spell");
            AddControlInWinForm("Use Pet in Slot 1", "UsePet1", "Offensive Spell");
            AddControlInWinForm("Use Pet in Slot 2", "UsePet2", "Offensive Spell");
            AddControlInWinForm("Use Pet in Slot 3", "UsePet3", "Offensive Spell");
            AddControlInWinForm("Use Pet in Slot 4", "UsePet4", "Offensive Spell");
            AddControlInWinForm("Use Pet in Slot 5", "UsePet5", "Offensive Spell");
            AddControlInWinForm("Use Cobra Shot", "UseCobraShot", "Offensive Spell");
            AddControlInWinForm("Use Explosive Trap", "UseExplosiveTrap", "Offensive Spell");
            AddControlInWinForm("Use Kill Command", "UseKillCommand", "Offensive Spell");
            AddControlInWinForm("Use KillShot", "UseKillShot", "Offensive Spell");
            AddControlInWinForm("Use Multi-Shot", "UseMultiShot", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use A Murder of Crows", "UseAMurderofCrows", "Offensive Cooldown");
            AddControlInWinForm("Use Barrage", "UseBarrage", "Offensive Cooldown");
            AddControlInWinForm("Use Bestial Wrath", "UseBestialWrath", "Offensive Cooldown");
            AddControlInWinForm("Use Blink Strike", "UseBlinkStrike", "Offensive Cooldown");
            AddControlInWinForm("Use Dire Beast", "UseDireBeast", "Offensive Cooldown");
            AddControlInWinForm("Use Fervor", "UseFervor", "Offensive Cooldown");
            AddControlInWinForm("Use Focus Fire", "UseFocusFire", "Offensive Cooldown");
            AddControlInWinForm("Use Glaive Toss", "UseGlaiveToss", "Offensive Cooldown");
            AddControlInWinForm("Use Lynx Rush", "UseLynxRush", "Offensive Cooldown");
            AddControlInWinForm("Use Powershot", "UsePowershot", "Offensive Cooldown");
            AddControlInWinForm("Use Stampede", "UseStampede", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Binding Shot", "UseBindingShot", "Defensive Cooldown");
            AddControlInWinForm("Use Concussive Shot", "UseConcussiveShot", "Defensive Cooldown");
            AddControlInWinForm("Use Deterrance", "UseDeterrance", "Defensive Cooldown");
            AddControlInWinForm("Use Disengage", "UseDisengage", "Defensive Cooldown");
            AddControlInWinForm("Use Freezing Trap", "UseFreezingTrap", "Defensive Cooldown");
            AddControlInWinForm("Use Ice Trap", "UseIceTrap", "Defensive Cooldown");
            AddControlInWinForm("Use Intimidation", "UseIntimidation", "Defensive Cooldown");
            AddControlInWinForm("Use Scatter Shot", "UseScatterShot", "Defensive Cooldown");
            AddControlInWinForm("Use Silencing Shot", "UseSilencingShot", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Exhilaration", "UseExhilaration", "Healing Spell");
            AddControlInWinForm("Use Feed Pet", "UseFeedPet", "Healing Spell");
            AddControlInWinForm("Use Mend Pet", "UseMendPet", "Healing Spell");
            AddControlInWinForm("Use Revive Pet", "UseRevivePet", "Healing Spell");
            /* Game Settings */
            AddControlInWinForm("Use Low Combat Settings", "UseLowCombat", "Game Settings");
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");

            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
            AddControlInWinForm("Use Core Hound Pet", "UseCoreHoundPet", "Game Settings");
            AddControlInWinForm("Use Worm Pet", "UseWormPet", "Game Settings");
            AddControlInWinForm("Use Chimera Pet", "UseChimeraPet", "Game Settings");
            AddControlInWinForm("Use Spirit Beast Pet", "UseSpiritBeastPet", "Game Settings");
            AddControlInWinForm("Use Revive Pet in Combat", "UseCombatRevive", "Game Settings");
            AddControlInWinForm("Do avoid melee (Off Advised!!)", "DoAvoidMelee", "Game Settings");
            AddControlInWinForm("Avoid melee distance (1 to 4)", "DoAvoidMeleeDistance", "Game Settings");
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

    #region Hunter Buffs

    public readonly Spell AspectoftheHawk = new Spell("Aspect of the Hawk");
    public readonly Spell Camouflage = new Spell("Camouflage");
    public readonly Spell FeignDeath = new Spell("Feign Death");
    public readonly Spell Misdirection = new Spell("Misdirection");
    public readonly Spell SerpentSting = new Spell("Serpent Sting");
    public readonly Spell TrapCaster = new Spell("Trap Caster");

    #endregion

    #region Offensive Spell

    public readonly Spell ArcaneShot = new Spell("Arcane Shot");
    public readonly Spell BlackArrow = new Spell("Black Arrow");
    public readonly Spell CallPet1 = new Spell("Call Pet 1");
    public readonly Spell CallPet2 = new Spell("Call Pet 2");
    public readonly Spell CallPet3 = new Spell("Call Pet 3");
    public readonly Spell CallPet4 = new Spell("Call Pet 4");
    public readonly Spell CallPet5 = new Spell("Call Pet 5");
    public readonly Spell CobraShot = new Spell("Cobra Shot");
    public readonly Spell Dismiss = new Spell("Dismiss Pet");
    public readonly Spell ExplosiveShot = new Spell("Explosive Shot");
    public readonly Spell ExplosiveTrap = new Spell("Explosive Trap");
    public readonly Spell KillShot = new Spell("Kill Shot");
    public readonly Spell MultiShot = new Spell("Multi-Shot");
    public readonly Spell SteadyShot = new Spell("Steady Shot");
    private Timer _serpentStingTimer = new Timer(0);

    #endregion

    #region Offensive Cooldown

    public readonly Spell AMurderofCrows = new Spell("A Murder of Crows");
    public readonly Spell Barrage = new Spell("Barrage");
    public readonly Spell BlinkStrike = new Spell("Blink Strike");
    public readonly Spell DireBeast = new Spell("Dire Beast");
    public readonly Spell Fervor = new Spell("Fervor");
    public readonly Spell GlaiveToss = new Spell("Glaive Toss");
    public readonly Spell LynxRush = new Spell("Lynx Rush");
    public readonly Spell Powershot = new Spell("Powershot");
    public readonly Spell Stampede = new Spell("Stampede");
    private Timer _direBeastTimer = new Timer(0);

    #endregion

    #region Defensive Cooldown

    public readonly Spell BindingShot = new Spell("Binding Shot");
    public readonly Spell ConcussiveShot = new Spell("Concussive Shot");
    public readonly Spell Deterrance = new Spell("Deterrance");
    public readonly Spell Disengage = new Spell("Disengage");
    public readonly Spell FreezingTrap = new Spell("Freezing Trap");
    public readonly Spell IceTrap = new Spell("Ice Trap");
    public readonly Spell ScatterShot = new Spell("Scatter Shot");
    public readonly Spell SilencingShot = new Spell("Silencing Shot");
    public readonly Spell WyvernSting = new Spell("Wyvern Sting");

    #endregion

    #region Healing Spell

    public readonly Spell Exhilaration = new Spell("Exhilaration");
    public readonly Spell FeedPet = new Spell("Feed Pet");
    public readonly Spell MendPet = new Spell("Mend Pet");
    public readonly Spell RevivePet = new Spell("Revive Pet");
    private Timer _mendPetTimer = new Timer(0);

    #endregion

    public HunterSurvival()
    {
        Main.InternalRange = 39f;
        Main.InternalAggroRange = 39f;
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
                            if (ObjectManager.Me.Target != lastTarget
                                && ArcaneShot.IsHostileDistanceGood)
                            {
                                Pull();
                                lastTarget = ObjectManager.Me.Target;
                            }

                            if (MySettings.UseLowCombat &&
                                ((ObjectManager.Target.Level < 70 && ObjectManager.Me.Level > 84)))
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

    private void UseTrap(Spell trap)
    {
        if (TrapCaster.HaveBuff)
            SpellManager.CastSpellByIDAndPosition(trap.Id, ObjectManager.Target.Position);
        else
            trap.Cast();
    }

    private void Pull()
    {
        if (ObjectManager.Pet.IsAlive)
        {
            Lua.RunMacroText("/petattack");
            Logging.WriteFight("Cast Pet Attack");
        }

        if (ObjectManager.Pet.IsAlive && MySettings.UseMisdirection && Misdirection.KnownSpell
            && Misdirection.IsSpellUsable)
        {
            Misdirection.CastOnUnitID("pet");
        }

        if (ArcaneShot.KnownSpell && ArcaneShot.IsHostileDistanceGood && MySettings.UseArcaneShot &&
            ArcaneShot.IsSpellUsable)
        {
            ArcaneShot.Cast();
        }
    }

    private void LowCombat()
    {
        Buff();
        if (MySettings.DoAvoidMelee)
            AvoidMelee();
        DefenseCycle();
        Heal();

        if (GlaiveToss.KnownSpell && MySettings.UseGlaiveToss && GlaiveToss.IsHostileDistanceGood &&
            GlaiveToss.IsSpellUsable)
        {
            GlaiveToss.Cast();
            return;
        }
        if (Barrage.KnownSpell && ObjectManager.GetNumberAttackPlayer() > 2 && MySettings.UseBarrage && Barrage.IsHostileDistanceGood && Barrage.IsSpellUsable)
        {
            Barrage.Cast();
            return;
        }
        if (!SerpentSting.TargetHaveBuff || _serpentStingTimer.IsReady)
        {
            if (MultiShot.KnownSpell && MySettings.UseMultiShot && MultiShot.IsHostileDistanceGood && ObjectManager.GetNumberAttackPlayer() >= 3 && MultiShot.IsSpellUsable)
            {
                _serpentStingTimer = new Timer(1000*12);
                MultiShot.Cast();
                return;
            }
            if (ArcaneShot.KnownSpell && MySettings.UseArcaneShot && ArcaneShot.IsHostileDistanceGood && ArcaneShot.IsSpellUsable)
            {
                _serpentStingTimer = new Timer(1000*12);
                ArcaneShot.Cast();
                return;
            }
        }
        if (CobraShot.KnownSpell && MySettings.UseCobraShot && CobraShot.IsHostileDistanceGood && CobraShot.IsSpellUsable)
        {
            CobraShot.Cast();
            return;
        }
        if (SteadyShot.KnownSpell && SteadyShot.IsHostileDistanceGood && ObjectManager.Me.FocusPercentage < 60
            && (!CobraShot.KnownSpell || !MySettings.UseCobraShot) && SteadyShot.IsSpellUsable)
        {
            SteadyShot.Cast();
            return;
        }
        if (ExplosiveTrap.KnownSpell && MySettings.UseExplosiveTrap && ExplosiveTrap.IsHostileDistanceGood
            && ExplosiveTrap.IsSpellUsable)
        {
            UseTrap(ExplosiveTrap);
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

        Pet();

        if (MySettings.UseAspectoftheHawk && AspectoftheHawk.KnownSpell && AspectoftheHawk.IsSpellUsable
            && !AspectoftheHawk.HaveBuff && !ObjectManager.Me.HaveBuff(109260))
        {
            AspectoftheHawk.Cast();
            return;
        }

        if (MySettings.UseCamouflage && Camouflage.KnownSpell && Camouflage.IsSpellUsable && !Camouflage.HaveBuff
            && !ObjectManager.Me.InCombat)
        {
            Camouflage.Cast();
            return;
        }

        if (MySettings.UseAlchFlask && !ObjectManager.Me.HaveBuff(79638) && !ObjectManager.Me.HaveBuff(79640) && !ObjectManager.Me.HaveBuff(79639)
            && !ItemsManager.IsItemOnCooldown(75525) && ItemsManager.GetItemCount(75525) > 0)
            ItemsManager.UseItem(75525);
    }

    private void DismissPet()
    {
        if (MySettings.DismissOnCall)
        {
            if (!ObjectManager.Me.IsCast && (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0 || !ObjectManager.Pet.IsValid) && Dismiss.KnownSpell && Dismiss.IsSpellUsable)
            {
                Dismiss.Cast();
                Others.SafeSleep(1500);
            }
        }
    }

    private void Pet()
    {
        if (MountTask.JustDismounted())
            return;
        if (MySettings.UsePet1 && !ObjectManager.Me.IsCast && (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0 || !ObjectManager.Pet.IsValid) && CallPet1.KnownSpell && CallPet1.IsSpellUsable)
        {
            DismissPet();
            CallPet1.Cast();
            Others.SafeSleep(1000);
        }
        else if (MySettings.UsePet2 && !ObjectManager.Me.IsCast && (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0 || !ObjectManager.Pet.IsValid) && CallPet2.KnownSpell && CallPet2.IsSpellUsable)
        {
            DismissPet();
            CallPet2.Cast();
            Others.SafeSleep(1000);
        }
        else if (MySettings.UsePet3 && !ObjectManager.Me.IsCast && (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0 || !ObjectManager.Pet.IsValid) && CallPet3.KnownSpell && CallPet3.IsSpellUsable)
        {
            DismissPet();
            CallPet3.Cast();
            Others.SafeSleep(1000);
        }
        else if (MySettings.UsePet4 && !ObjectManager.Me.IsCast && (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0 || !ObjectManager.Pet.IsValid) && CallPet4.KnownSpell && CallPet4.IsSpellUsable)
        {
            DismissPet();
            CallPet4.Cast();
            Others.SafeSleep(1000);
        }
        else
        {
            if (MySettings.UsePet5 && !ObjectManager.Me.IsCast && (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0 || !ObjectManager.Pet.IsValid) && CallPet5.KnownSpell && CallPet5.IsSpellUsable)
            {
                DismissPet();
                CallPet5.Cast();
                Others.SafeSleep(1000);
            }
        }

        if (!ObjectManager.Me.IsCast && (!ObjectManager.Pet.IsAlive || ObjectManager.Pet.Guid == 0)
            && RevivePet.KnownSpell && RevivePet.IsSpellUsable && MySettings.UseRevivePet
            && MySettings.UseCombatRevive && ObjectManager.Target.HealthPercent > 10 && ObjectManager.Me.InCombat)
        {
            RevivePet.Cast();
            Others.SafeSleep(1000);
            return;
        }
        if (!ObjectManager.Me.IsCast && !ObjectManager.Pet.IsAlive
            && RevivePet.KnownSpell && RevivePet.IsSpellUsable && MySettings.UseRevivePet
            && !ObjectManager.Me.InCombat)
        {
            RevivePet.Cast();
            Others.SafeSleep(1000);
        }
    }

    private void AvoidMelee()
    {
        if (ObjectManager.Target.GetDistance < MySettings.DoAvoidMeleeDistance && ObjectManager.Target.InCombat)
        {
            if (Disengage.KnownSpell && MySettings.UseDisengage && Disengage.IsSpellUsable)
            {
                Logging.WriteFight("Too Close. Using disengage");
                Disengage.Cast();
                MovementManager.Face(ObjectManager.Target.Position);
            }
        }
    }

    private void DefenseCycle()
    {
        if (ObjectManager.Me.HealthPercent < 20 && MySettings.UseFeignDeath
            && FeignDeath.KnownSpell && FeignDeath.IsSpellUsable)
        {
            FeignDeath.Cast();
            Others.SafeSleep(5000);
            if (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0 || !ObjectManager.Pet.IsValid)
                return;
            Others.SafeSleep(5000);
            return;
        }
        if (Deterrance.KnownSpell && MySettings.UseDeterrance && ObjectManager.Me.HealthPercent < 50
            && Deterrance.IsSpellUsable)
        {
            Deterrance.Cast();
            Others.SafeSleep(200);
            return;
        }
        if (MySettings.UseFreezingTrap && FreezingTrap.KnownSpell && ObjectManager.GetNumberAttackPlayer() > 1
            && ObjectManager.Target.GetDistance > 10 && FreezingTrap.IsSpellUsable)
        {
            UseTrap(FreezingTrap);
            return;
        }
        if (IceTrap.KnownSpell && MySettings.UseIceTrap && Disengage.KnownSpell && MySettings.UseDisengage
            && ObjectManager.Me.HealthPercent < 80 && ObjectManager.Target.GetDistance < 10
            && IceTrap.IsSpellUsable && Disengage.IsSpellUsable)
        {
            UseTrap(IceTrap);
            Others.SafeSleep(1000);
            MovementsAction.Jump();
            Disengage.Cast();
            return;
        }
        if (Disengage.KnownSpell && MySettings.UseDisengage && ConcussiveShot.KnownSpell
            && MySettings.UseConcussiveShot && ConcussiveShot.IsHostileDistanceGood
            && ObjectManager.Me.HealthPercent < 80 && ConcussiveShot.IsSpellUsable && Disengage.IsSpellUsable)
        {
            ConcussiveShot.Cast();
            Others.SafeSleep(1000);
            MovementsAction.Jump();
            Disengage.Cast();
            return;
        }
        if (BindingShot.KnownSpell && MySettings.UseBindingShot && Disengage.KnownSpell && MySettings.UseDisengage
            && ObjectManager.Me.HealthPercent < 80 && BindingShot.IsHostileDistanceGood
            && Disengage.IsSpellUsable && BindingShot.IsSpellUsable)
        {
            BindingShot.Cast();
            Others.SafeSleep(1000);
            MovementsAction.Jump();
            Disengage.Cast();
            return;
        }
        if (ObjectManager.Me.HealthPercent <= MySettings.UseWarStompAtPercentage && WarStomp.KnownSpell &&
            WarStomp.IsSpellUsable
            && MySettings.UseWarStomp)
        {
            WarStomp.Cast();
            _onCd = new Timer(1000*2);
            return;
        }
        if (ObjectManager.Me.HealthPercent <= MySettings.UseStoneformAtPercentage && Stoneform.KnownSpell &&
            Stoneform.IsSpellUsable
            && MySettings.UseStoneform)
        {
            Stoneform.Cast();
            _onCd = new Timer(1000*8);
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (ObjectManager.Me.HealthPercent <= MySettings.UseGiftoftheNaaruAtPercentage && GiftoftheNaaru.KnownSpell
            && MySettings.UseGiftoftheNaaru && GiftoftheNaaru.IsSpellUsable)
        {
            GiftoftheNaaru.Cast();
            return;
        }
        if (Exhilaration.KnownSpell && MySettings.UseExhilaration && ObjectManager.Me.HealthPercent < 70
            && Exhilaration.IsSpellUsable)
        {
            Exhilaration.Cast();
            return;
        }
        if (FeedPet.KnownSpell && MySettings.UseFeedPet && !ObjectManager.Me.InCombat
            && ObjectManager.Pet.Health > 0 && ObjectManager.Pet.HealthPercent < 50 && FeedPet.IsSpellUsable)
        {
            FeedPet.Cast();
            return;
        }
        if (MySettings.UseMendPet && _mendPetTimer.IsReady && MendPet.KnownSpell && ObjectManager.Pet.Health > 0
            && ObjectManager.Pet.HealthPercent < 80 && MendPet.IsSpellUsable)
        {
            MendPet.Cast();
            _mendPetTimer = new Timer(1000*10);
        }
    }

    private void Decast()
    {
        if (ArcaneTorrent.IsSpellUsable && ArcaneTorrent.KnownSpell && ObjectManager.Target.GetDistance < 8
            && ObjectManager.Me.HealthPercent <= MySettings.UseArcaneTorrentForDecastAtPercentage
            && MySettings.UseArcaneTorrentForDecast && ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe)
        {
            ArcaneTorrent.Cast();
            return;
        }
        if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && SilencingShot.IsHostileDistanceGood
            && SilencingShot.KnownSpell && SilencingShot.IsSpellUsable && MySettings.UseSilencingShot)
        {
            SilencingShot.Cast();
            return;
        }
        if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && ScatterShot.IsHostileDistanceGood
            && ScatterShot.KnownSpell && ScatterShot.IsSpellUsable && MySettings.UseScatterShot)
        {
            ScatterShot.Cast();
            return;
        }
        if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && MySettings.UseWyvernSting
            && WyvernSting.KnownSpell && WyvernSting.IsSpellUsable && WyvernSting.IsHostileDistanceGood)
        {
            WyvernSting.Cast();
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
        if (MySettings.UseBerserking && Berserking.KnownSpell && ObjectManager.Target.GetDistance <= 40f
            && Berserking.IsSpellUsable)
        {
            Berserking.Cast();
            return;
        }
        if (MySettings.UseBloodFury && BloodFury.KnownSpell && ObjectManager.Target.GetDistance <= 40f
            && BloodFury.IsSpellUsable)
        {
            BloodFury.Cast();
            return;
        }
        if (AMurderofCrows.KnownSpell && MySettings.UseAMurderofCrows && AMurderofCrows.IsHostileDistanceGood
            && AMurderofCrows.IsSpellUsable && !AMurderofCrows.TargetHaveBuff)
        {
            AMurderofCrows.Cast();
            return;
        }
        if (Barrage.KnownSpell && MySettings.UseBarrage && Barrage.IsHostileDistanceGood && Barrage.IsSpellUsable)
        {
            Barrage.Cast();
            return;
        }
        if (BlinkStrike.KnownSpell && MySettings.UseBlinkStrike && ObjectManager.Pet.IsAlive
            && ObjectManager.Target.GetDistance <= 40f && BlinkStrike.IsSpellUsable)
        {
            BlinkStrike.Cast();
            return;
        }
        if (DireBeast.KnownSpell && MySettings.UseDireBeast && _direBeastTimer.IsReady
            && DireBeast.IsHostileDistanceGood && DireBeast.IsSpellUsable)
        {
            DireBeast.Cast();
            _direBeastTimer = new Timer(1000*15);
            return;
        }
        if (Fervor.KnownSpell && MySettings.UseFervor && ObjectManager.Me.Focus < 50
            && Fervor.IsSpellUsable)
        {
            Fervor.Cast();
            return;
        }
        if (GlaiveToss.KnownSpell && MySettings.UseGlaiveToss && GlaiveToss.IsHostileDistanceGood
            && GlaiveToss.IsSpellUsable)
        {
            GlaiveToss.Cast();
            return;
        }
        if (LynxRush.KnownSpell && MySettings.UseLynxRush && ObjectManager.Target.GetDistance <= 40f
            && LynxRush.IsSpellUsable)
        {
            LynxRush.Cast();
            return;
        }
        if (Powershot.KnownSpell && MySettings.UsePowershot && Powershot.IsHostileDistanceGood
            && Powershot.IsSpellUsable)
        {
            Powershot.Cast();
            return;
        }
        if (Stampede.KnownSpell && MySettings.UseStampede && Stampede.IsHostileDistanceGood
            && Stampede.IsSpellUsable)
        {
            Stampede.Cast();
        }
    }

    private void DPSCycle()
    {
        Usefuls.SleepGlobalCooldown();
        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            if (ArcaneShot.KnownSpell && MySettings.UseArcaneShot && !SerpentSting.TargetHaveBuff
                && ArcaneShot.IsHostileDistanceGood && ArcaneShot.IsSpellUsable)
            {
                ArcaneShot.Cast();
                _serpentStingTimer = new Timer(1000*12);
                return;
            }
            if (ArcaneShot.KnownSpell && MySettings.UseArcaneShot && _serpentStingTimer.IsReady
                && ArcaneShot.IsHostileDistanceGood && ArcaneShot.IsSpellUsable)
            {
                ArcaneShot.Cast();
                _serpentStingTimer = new Timer(1000*12);
                return;
            }
            if (KillShot.KnownSpell && MySettings.UseKillShot && KillShot.IsHostileDistanceGood
                && KillShot.IsSpellUsable)
            {
                KillShot.Cast();
                return;
            }
            if (ObjectManager.GetNumberAttackPlayer() > 4 && MySettings.UseMultiShot && MySettings.UseExplosiveTrap
                && MySettings.UseExplosiveShot)
            {
                if (MultiShot.KnownSpell && MultiShot.IsHostileDistanceGood && MultiShot.IsSpellUsable)
                {
                    MultiShot.Cast();
                    return;
                }
                if (ExplosiveTrap.KnownSpell && ObjectManager.Target.GetDistance < 10 && ExplosiveTrap.IsSpellUsable)
                {
                    UseTrap(ExplosiveTrap);
                    return;
                }
                if (ExplosiveShot.KnownSpell && ExplosiveShot.IsHostileDistanceGood && ExplosiveShot.IsSpellUsable)
                {
                    ExplosiveShot.Cast();
                    return;
                }
                return;
            }
            if (ExplosiveTrap.KnownSpell && MySettings.UseExplosiveTrap && ObjectManager.Target.GetDistance < 10
                && ObjectManager.GetNumberAttackPlayer() < 4 && ObjectManager.GetNumberAttackPlayer() > 1
                && ExplosiveTrap.IsSpellUsable)
            {
                UseTrap(ExplosiveTrap);
                return;
            }
            if (BlackArrow.KnownSpell && MySettings.UseBlackArrow && BlackArrow.IsHostileDistanceGood
                && BlackArrow.IsSpellUsable)
            {
                BlackArrow.Cast();
                return;
            }
            if (ExplosiveShot.KnownSpell && MySettings.UseExplosiveShot && ExplosiveShot.IsHostileDistanceGood
                && ExplosiveShot.IsSpellUsable)
            {
                ExplosiveShot.Cast();
                return;
            }
            if (MultiShot.KnownSpell && MySettings.UseMultiShot && MultiShot.IsHostileDistanceGood
                && ObjectManager.Me.FocusPercentage > 79
                && ObjectManager.GetNumberAttackPlayer() < 4 && ObjectManager.GetNumberAttackPlayer() > 1
                && MultiShot.IsSpellUsable)
            {
                MultiShot.Cast();
                return;
            }
            if (ArcaneShot.KnownSpell && MySettings.UseArcaneShot && ArcaneShot.IsHostileDistanceGood
                && ObjectManager.Me.FocusPercentage > 79 && ArcaneShot.IsSpellUsable)
            {
                ArcaneShot.Cast();
                return;
            }
            if (ArcaneTorrent.KnownSpell && MySettings.UseArcaneTorrentForResource
                && ArcaneTorrent.IsSpellUsable)
            {
                ArcaneTorrent.Cast();
                return;
            }
            if (CobraShot.KnownSpell && MySettings.UseCobraShot && CobraShot.IsHostileDistanceGood
                && ObjectManager.Me.FocusPercentage < 80 && CobraShot.IsSpellUsable)
            {
                CobraShot.Cast();
                return;
            }
            if (SteadyShot.KnownSpell && SteadyShot.IsHostileDistanceGood
                && ObjectManager.Me.FocusPercentage < 60 && (!CobraShot.KnownSpell || !MySettings.UseCobraShot)
                && SteadyShot.IsSpellUsable)
            {
                SteadyShot.Cast();
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

    #region Nested type: HunterSurvivalSettings

    [Serializable]
    public class HunterSurvivalSettings : Settings
    {
        public bool DismissOnCall = true;
        public bool DoAvoidMelee = false;
        public int DoAvoidMeleeDistance = 0;
        public bool UseAMurderofCrows = true;
        public bool UseAlchFlask = true;
        public bool UseArcaneShot = true;
        public bool UseArcaneTorrentForDecast = true;
        public int UseArcaneTorrentForDecastAtPercentage = 100;
        public bool UseArcaneTorrentForResource = true;
        public bool UseAspectoftheHawk = true;
        public bool UseBarrage = true;
        public bool UseBerserking = true;
        public bool UseBindingShot = true;
        public bool UseBlackArrow = true;
        public bool UseBlinkStrike = true;
        public bool UseBloodFury = true;
        public bool UseCamouflage = false;
        public bool UseCobraShot = true;
        public bool UseCombatRevive = true;
        public bool UseConcussiveShot = true;
        public bool UseDeterrance = true;
        public bool UseDireBeast = true;
        public bool UseDisengage = true;

        public bool UseExhilaration = true;
        public bool UseExplosiveShot = true;
        public bool UseExplosiveTrap = true;
        public bool UseFeedPet = true;
        public bool UseFeignDeath = true;
        public bool UseFervor = true;
        public bool UseFreezingTrap = true;
        public bool UseGiftoftheNaaru = true;
        public int UseGiftoftheNaaruAtPercentage = 80;
        public bool UseGlaiveToss = true;
        public bool UseHuntersMark = true;
        public bool UseIceTrap = true;
        public bool UseKillShot = true;

        public bool UseLowCombat = true;
        public bool UseLynxRush = true;
        public bool UseMendPet = true;
        public bool UseMisdirection = true;
        public bool UseMultiShot = true;
        public bool UsePet1 = true;
        public bool UsePet2 = false;
        public bool UsePet3 = false;
        public bool UsePet4 = false;
        public bool UsePet5 = false;
        public bool UsePowershot = true;
        public bool UseRevivePet = true;
        public bool UseScatterShot = true;
        public bool UseSerpentSting = true;
        public bool UseSilencingShot = true;
        public bool UseStampede = true;
        public bool UseStoneform = true;
        public int UseStoneformAtPercentage = 80;
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;
        public bool UseWarStomp = true;
        public int UseWarStompAtPercentage = 80;
        public bool UseWyvernSting = true;

        public HunterSurvivalSettings()
        {
            ConfigWinForm("Hunter Survival Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent for Interrupt", "UseArcaneTorrentForDecast", "Professions & Racials", "AtPercentage");
            AddControlInWinForm("Use Arcane Torrent for Resource", "UseArcaneTorrentForResource", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");

            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            /* Hunter Buffs */
            AddControlInWinForm("Use Aspect of the Hawk", "UseAspectoftheHawk", "Hunter Buffs");
            AddControlInWinForm("Use Camouflage", "UseCamouflage", "Hunter Buffs");
            AddControlInWinForm("Use Feign Death", "UseFeignDeath", "Hunter Buffs");
            AddControlInWinForm("Use Hunter's Mark", "UseHuntersMark", "Hunter Buffs");
            AddControlInWinForm("Use Misdirection", "UseMisdirection", "Hunter Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Arcane Shot", "UseArcaneShot", "Offensive Spell");
            AddControlInWinForm("Use Black Arrow", "UseBlackArrow", "Offensive Spell");
            AddControlInWinForm("Dismiss pet before calling again", "DismissOnCall", "Offensive Spell");
            AddControlInWinForm("Use Pet in Slot 1", "UsePet1", "Offensive Spell");
            AddControlInWinForm("Use Pet in Slot 2", "UsePet2", "Offensive Spell");
            AddControlInWinForm("Use Pet in Slot 3", "UsePet3", "Offensive Spell");
            AddControlInWinForm("Use Pet in Slot 4", "UsePet4", "Offensive Spell");
            AddControlInWinForm("Use Pet in Slot 5", "UsePet5", "Offensive Spell");
            AddControlInWinForm("Use Cobra Shot", "UseCobraShot", "Offensive Spell");
            AddControlInWinForm("Use Explosive Shot", "UseExplosiveShot", "Offensive Spell");
            AddControlInWinForm("Use Explosive Trap", "UseExplosiveTrap", "Offensive Spell");
            AddControlInWinForm("Use KillShot", "UseKillShot", "Offensive Spell");
            AddControlInWinForm("Use Multi-Shot", "UseMultiShot", "Offensive Spell");
            AddControlInWinForm("Use Serpent Sting", "UseSerpentSting", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use A Murder of Crows", "UseAMurderofCrows", "Offensive Cooldown");
            AddControlInWinForm("Use Barrage", "UseBarrage", "Offensive Cooldown");
            AddControlInWinForm("Use Blink Strike", "UseBlinkStrike", "Offensive Cooldown");
            AddControlInWinForm("Use Dire Beast", "UseDireBeast", "Offensive Cooldown");
            AddControlInWinForm("Use Fervor", "UseFervor", "Offensive Cooldown");
            AddControlInWinForm("Use Glaive Toss", "UseGlaiveToss", "Offensive Cooldown");
            AddControlInWinForm("Use Lynx Rush", "UseLynxRush", "Offensive Cooldown");
            AddControlInWinForm("Use Powershot", "UsePowershot", "Offensive Cooldown");
            AddControlInWinForm("Use Rapid Fire", "UseRapidFire", "Offensive Cooldown");
            AddControlInWinForm("Use Readiness", "UseReadiness", "Offensive Cooldown");
            AddControlInWinForm("Use Stampede", "UseStampede", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Binding Shot", "UseBindingShot", "Defensive Cooldown");
            AddControlInWinForm("Use Concussive Shot", "UseConcussiveShot", "Defensive Cooldown");
            AddControlInWinForm("Use Deterrance", "UseDeterrance", "Defensive Cooldown");
            AddControlInWinForm("Use Disengage", "UseDisengage", "Defensive Cooldown");
            AddControlInWinForm("Use Freezing Trap", "UseFreezingTrap", "Defensive Cooldown");
            AddControlInWinForm("Use Ice Trap", "UseIceTrap", "Defensive Cooldown");
            AddControlInWinForm("Use Scatter Shot", "UseScatterShot", "Defensive Cooldown");
            AddControlInWinForm("Use Silencing Shot", "UseSilencingShot", "Defensive Cooldown");
            AddControlInWinForm("Use Wyvern Sting", "UseWyvernSting", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Exhilaration", "UseExhilaration", "Healing Spell");
            AddControlInWinForm("Use Feed Pet", "UseFeedPet", "Healing Spell");
            AddControlInWinForm("Use Mend Pet", "UseMendPet", "Healing Spell");
            AddControlInWinForm("Use Revive Pet", "UseRevivePet", "Healing Spell");
            /* Game Settings */
            AddControlInWinForm("Use Low Combat Settings", "UseLowCombat", "Game Settings");
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");

            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
            AddControlInWinForm("Use Revive Pet in Combat", "UseCombatRevive", "Game Settings");
            AddControlInWinForm("Do avoid melee (Off Advised!!)", "DoAvoidMelee", "Game Settings");
            AddControlInWinForm("Avoid melee distance (1 to 4)", "DoAvoidMeleeDistance", "Game Settings");
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