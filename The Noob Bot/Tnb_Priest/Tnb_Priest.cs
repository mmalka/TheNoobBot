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
                    #region Priest Specialisation checking

                case WoWClass.Priest:

                    if (wowSpecialization == WoWSpecialization.PriestShadow)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Priest_Shadow.xml";
                            var currentSetting = new PriestShadow.PriestShadowSettings();
                            if (File.Exists(currentSettingsFile) && !resetSettings)
                            {
                                currentSetting = Settings.Load<PriestShadow.PriestShadowSettings>(currentSettingsFile);
                            }
                            currentSetting.ToForm();
                            currentSetting.Save(currentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Priest Shadow Combat class...");
                            InternalRange = 30.0f;
                            EquipmentAndStats.SetPlayerSpe(WoWSpecialization.PriestShadow);
                            new PriestShadow();
                        }
                        break;
                    }
                    if (wowSpecialization == WoWSpecialization.PriestDiscipline || wowSpecialization == WoWSpecialization.None)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Priest_Discipline.xml";
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
                            Logging.WriteFight("Loading Priest Discipline Combat class...");
                            InternalRange = 30.0f;
                            EquipmentAndStats.SetPlayerSpe(WoWSpecialization.PriestDiscipline);
                            new PriestDiscipline();
                        }
                        break;
                    }
                    if (wowSpecialization == WoWSpecialization.PriestHoly)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Priest_Holy.xml";
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
                            Logging.WriteFight("Loading Priest Holy Combat class...");
                            InternalRange = 30.0f;
                            EquipmentAndStats.SetPlayerSpe(WoWSpecialization.PriestHoly);
                            new PriestHoly();
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

#region Priest

public class PriestShadow
{
    private static PriestShadowSettings MySettings = PriestShadowSettings.GetSettings();

    #region General Timers & Variables

    private readonly WoWItem _firstTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET);
    private readonly WoWItem _secondTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET, 2);
    public int LC = 0;

    private Timer _onCd = new Timer(0);

    #endregion

    #region Professions and Racials

    public readonly Spell Alchemy = new Spell("Alchemy");
    public readonly Spell ArcaneTorrent = new Spell("Arcane Torrent");
    public readonly Spell Berserking = new Spell("Berserking");
    public readonly Spell BloodFury = new Spell("Blood Fury");

    public readonly Spell GiftoftheNaaru = new Spell("Gift of the Naaru");

    public readonly Spell Stoneform = new Spell("Stoneform");
    public readonly Spell WarStomp = new Spell("War Stomp");

    #endregion

    #region Priest Buffs

    public readonly Spell InnerFire = new Spell("Inner Fire");
    public readonly Spell InnerWill = new Spell("Inner Will");
    public readonly Spell Levitate = new Spell("Levitate");
    public readonly Spell PowerWordFortitude = new Spell("Power Word: Fortitude");
    public readonly Spell Shadowform = new Spell("Shadowform");
    public readonly Spell ClarityofPower = new Spell("Clarity of Power");
    private Timer _levitateTimer = new Timer(0);

    #endregion

    #region Offensive Spell

    public readonly Spell Cascade = new Spell("Cascade");
    public readonly Spell DevouringPlague = new Spell("Devouring Plague");
    public readonly Spell DivineStar = new Spell("Divine Star");
    public readonly Spell Halo = new Spell("Halo");
    public readonly Spell MindBlast = new Spell("Mind Blast");
    public readonly Spell MindFlay = new Spell("Mind Flay");
    public readonly Spell MindSear = new Spell("Mind Sear");
    public readonly Spell MindSpike = new Spell("Mind Spike");
    public readonly Spell ShadowWordDeath = new Spell("Shadow Word: Death");
    public readonly Spell Insanity = new Spell("Insanity");
    public readonly Spell ShadowWordPain = new Spell("Shadow Word: Pain");
    public readonly Spell Smite = new Spell("Smite");
    public readonly Spell VampiricTouch = new Spell("Vampiric Touch");
    public readonly Spell VoidEntropy = new Spell("Void Entropy");
    private Timer _devouringPlagueTimer = new Timer(0);
    private Timer _shadowWordPainTimer = new Timer(0);
    private Timer _vampiricTouchTimer = new Timer(0);
    private Timer _voidEntropyTimer = new Timer(0);

    #endregion

    #region Offensive Cooldown

    public readonly Spell PowerInfusion = new Spell("Power Infusion");
    public readonly Spell Shadowfiend = new Spell("Shadowfiend");

    #endregion

    #region Defensive Cooldown

    public readonly Spell Dispersion = new Spell("Dispersion");
    public readonly Spell PowerWordShield = new Spell("Power Word: Shield");
    public readonly Spell PsychicHorror = new Spell("Psychic Horror");
    public readonly Spell PsychicScream = new Spell("Psychic Scream");
    public readonly Spell Psyfiend = new Spell("Psyfiend");
    public readonly Spell Silence = new Spell("Silence");
    public readonly Spell SpectralGuise = new Spell("Spectral Guise");
    public readonly Spell VoidTendrils = new Spell("Void Tendrils");

    #endregion

    #region Healing Spell

    public readonly Spell DesperatePrayer = new Spell("Desperate Prayer");
    public readonly Spell FlashHeal = new Spell("Flash Heal");
    public readonly Spell HymnofHope = new Spell("Hymn of Hope");
    public readonly Spell PrayerofMending = new Spell("Prayer of Mending");
    public readonly Spell Renew = new Spell("Renew");
    public readonly Spell VampiricEmbrace = new Spell("Vampiric Embrace");
/*
        private Timer _renewTimer = new Timer(0);
*/

    #endregion

    public PriestShadow()
    {
        Main.InternalRange = 39f;
        Main.InternalAggroRange = 39f;
        MySettings = PriestShadowSettings.GetSettings();
        Main.DumpCurrentSettings<PriestShadowSettings>(MySettings);
        UInt128 lastTarget = 0;

        while (Main.InternalLoop)
        {
            try
            {
                if (!ObjectManager.Me.IsDead)
                {
                    if (!ObjectManager.Me.IsMounted)
                    {
                        BuffLevitate();
                        if (Fight.InFight && ObjectManager.Me.Target > 0)
                        {
                            if (ObjectManager.Me.Target != lastTarget &&
                                (Smite.IsHostileDistanceGood || ShadowWordPain.IsHostileDistanceGood))
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

    private void BuffLevitate()
    {
        if (!ObjectManager.Me.InCombat && Levitate.KnownSpell && Levitate.IsSpellUsable && MySettings.UseLevitate
            && (!Levitate.HaveBuff || _levitateTimer.IsReady))
        {
            Levitate.Cast();
            _levitateTimer = new Timer(1000*60*9);
        }
    }

    private void Pull()
    {
        if (DevouringPlague.IsSpellUsable && DevouringPlague.KnownSpell && DevouringPlague.IsHostileDistanceGood
            && ObjectManager.Me.ShadowOrbs == 3 && MySettings.UseDevouringPlague)
        {
            DevouringPlague.Cast();
            return;
        }
        if (ShadowWordPain.IsSpellUsable && ShadowWordPain.KnownSpell && ShadowWordPain.IsHostileDistanceGood
            && MySettings.UseShadowWordPain)
        {
            ShadowWordPain.Cast();
            _shadowWordPainTimer = new Timer(1000*14);
        }
    }

    private void LowCombat()
    {
        if (MySettings.DoAvoidMelee)
            AvoidMelee();
        Heal();
        DefenseCycle();
        Buff();

        if (DevouringPlague.IsSpellUsable && DevouringPlague.KnownSpell && DevouringPlague.IsHostileDistanceGood
            && ObjectManager.Me.ShadowOrbs == 3 && MySettings.UseDevouringPlague)
        {
            DevouringPlague.Cast();
            return;
        }
        if (MindSpike.KnownSpell && MindSpike.IsSpellUsable && MindSpike.IsHostileDistanceGood
            && MySettings.UseMindSpike)
        {
            MindSpike.Cast();
            if (ObjectManager.Target.HealthPercent < 50 && ObjectManager.Target.HealthPercent > 0)
            {
                MindSpike.Cast();
                return;
            }
            return;
        }
        if (MindSear.KnownSpell && MindSear.IsSpellUsable && MindSear.IsHostileDistanceGood
            && MySettings.UseMindSear)
        {
            MindSear.Cast();
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

        if (PowerWordFortitude.KnownSpell && PowerWordFortitude.IsSpellUsable &&
            !PowerWordFortitude.HaveBuff && MySettings.UsePowerWordFortitude)
        {
            PowerWordFortitude.Cast();
            return;
        }
        if (InnerFire.KnownSpell && InnerFire.IsSpellUsable && !InnerFire.HaveBuff
            && MySettings.UseInnerFire)
        {
            InnerFire.Cast();
            return;
        }
        if (InnerWill.KnownSpell && InnerWill.IsSpellUsable && !InnerWill.HaveBuff
            && !MySettings.UseInnerFire && MySettings.UseInnerWill)
        {
            InnerWill.Cast();
            return;
        }
        if (MySettings.UseAlchFlask && !ObjectManager.Me.HaveBuff(79638) && !ObjectManager.Me.HaveBuff(79640) && !ObjectManager.Me.HaveBuff(79639)
            && !ItemsManager.IsItemOnCooldown(75525) && ItemsManager.GetItemCount(75525) > 0)
        {
            ItemsManager.UseItem(75525);
            return;
        }
        if (!Shadowform.HaveBuff && Shadowform.KnownSpell && Shadowform.IsSpellUsable
            && MySettings.UseShadowform)
        {
            Shadowform.Cast();
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
        if (ObjectManager.Me.HealthPercent <= MySettings.UsePsychicScreamAtPercentage && PsychicScream.IsSpellUsable &&
            PsychicScream.KnownSpell
            && MySettings.UsePsychicScream)
        {
            PsychicScream.Cast();
            _onCd = new Timer(1000*8);
            return;
        }
        if (ObjectManager.Me.HealthPercent <= MySettings.UseDispersionHealthAtPercentage && Dispersion.KnownSpell &&
            Dispersion.IsSpellUsable
            && MySettings.UseDispersionHealth)
        {
            if (Renew.KnownSpell && Renew.IsSpellUsable && MySettings.UseRenew)
            {
                Renew.Cast();
                Others.SafeSleep(1500);
            }
            Dispersion.Cast();
            _onCd = new Timer(1000*6);
            return;
        }
        if (ObjectManager.GetNumberAttackPlayer() >= 2 &&
            ObjectManager.Me.HealthPercent <= MySettings.UseVoidTendrilsAtPercentage &&
            VoidTendrils.IsSpellUsable && VoidTendrils.KnownSpell && MySettings.UseVoidTendrils)
        {
            VoidTendrils.Cast();
            _onCd = new Timer(1000*10);
            return;
        }
        if (ObjectManager.GetNumberAttackPlayer() >= 2 &&
            ObjectManager.Me.HealthPercent <= MySettings.UsePsyfiendAtPercentage &&
            Psyfiend.IsSpellUsable && Psyfiend.KnownSpell && MySettings.UsePsyfiend)
        {
            Psyfiend.Cast();
            _onCd = new Timer(1000*10);
            return;
        }
        if (PowerWordShield.KnownSpell && PowerWordShield.IsSpellUsable
            && !PowerWordShield.HaveBuff && MySettings.UsePowerWordShield
            && !ObjectManager.Me.HaveBuff(6788) &&
            ObjectManager.Me.HealthPercent <= MySettings.UsePowerWordShieldAtPercentage
            && (ObjectManager.Me.InCombat || ObjectManager.Me.GetMove))
        {
            PowerWordShield.Cast();
            _onCd = new Timer(1000*6);
            return;
        }
        if (ObjectManager.Me.HealthPercent <= MySettings.UseSpectralGuiseAtPercentage &&
            SpectralGuise.IsSpellUsable && SpectralGuise.KnownSpell
            && MySettings.UseSpectralGuise)
        {
            if (Renew.KnownSpell && Renew.IsSpellUsable && MySettings.UseRenew)
            {
                Renew.Cast();
                Others.SafeSleep(1500);
            }
            SpectralGuise.Cast();
            _onCd = new Timer(1000*3);
            return;
        }
        if (ObjectManager.Me.HealthPercent <= MySettings.UseStoneformAtPercentage &&
            Stoneform.IsSpellUsable && Stoneform.KnownSpell
            && MySettings.UseStoneform)
        {
            Stoneform.Cast();
            _onCd = new Timer(1000*8);
            return;
        }
        if (ObjectManager.Me.HealthPercent <= MySettings.UseWarStompAtPercentage &&
            WarStomp.IsSpellUsable && WarStomp.KnownSpell
            && MySettings.UseWarStomp)
        {
            WarStomp.Cast();
            _onCd = new Timer(1000*2);
        }
    }

    private void Heal()
    {
        if (MySettings.UseArcaneTorrentForResource && ArcaneTorrent.KnownSpell && ArcaneTorrent.IsSpellUsable
            && ObjectManager.Me.ManaPercentage <= MySettings.UseArcaneTorrentForResourceAtPercentage)
        {
            ArcaneTorrent.Cast();
            return;
        }

        if (ObjectManager.Me.HealthPercent <= MySettings.UseFlashHealNonCombatAtPercentage &&
            !ObjectManager.Me.InCombat
            && FlashHeal.KnownSpell && FlashHeal.IsSpellUsable && MySettings.UseFlashHealNonCombat)
        {
            FlashHeal.Cast(false);
            return;
        }
        if (ObjectManager.Me.ManaPercentage <= MySettings.UseHymnofHopeAtPercentage &&
            HymnofHope.KnownSpell
            && HymnofHope.IsSpellUsable && !ObjectManager.Me.InCombat && MySettings.UseHymnofHope)
        {
            HymnofHope.Cast(false);
            return;
        }
        if (ObjectManager.Me.ManaPercentage <= MySettings.UseDispersionManaAtPercentage &&
            !ObjectManager.Me.InCombat
            && Dispersion.KnownSpell && Dispersion.IsSpellUsable && MySettings.UseDispersionMana)
        {
            Dispersion.Cast();
            return;
        }
        if (ObjectManager.Me.HealthPercent <= MySettings.UseDesperatePrayerAtPercentage &&
            DesperatePrayer.KnownSpell && DesperatePrayer.IsSpellUsable
            && MySettings.UseDesperatePrayer)
        {
            DesperatePrayer.Cast();
            return;
        }
        if (ObjectManager.Me.HealthPercent <= MySettings.UseFlashHealInCombatAtPercentage &&
            FlashHeal.KnownSpell && FlashHeal.IsSpellUsable
            && MySettings.UseFlashHealInCombat)
        {
            FlashHeal.Cast();
            return;
        }
        if (ObjectManager.Me.HealthPercent <= MySettings.UseGiftoftheNaaruAtPercentage &&
            GiftoftheNaaru.IsSpellUsable && GiftoftheNaaru.KnownSpell
            && MySettings.UseGiftoftheNaaru)
        {
            GiftoftheNaaru.Cast();
            return;
        }
        if (ObjectManager.Me.HealthPercent <= MySettings.UseVampiricEmbraceAtPercentage &&
            VampiricEmbrace.IsSpellUsable && VampiricEmbrace.KnownSpell
            && MySettings.UseVampiricEmbrace)
        {
            VampiricEmbrace.Cast();
            return;
        }
        if (ObjectManager.Me.HealthPercent <= MySettings.UsePrayerofMendingAtPercentage &&
            PrayerofMending.KnownSpell && PrayerofMending.IsSpellUsable
            && MySettings.UsePrayerofMending)
        {
            PrayerofMending.Cast();
            return;
        }
        if (Renew.KnownSpell && Renew.IsSpellUsable && !Renew.HaveBuff &&
            ObjectManager.Me.HealthPercent <= MySettings.UseRenewAtPercentage &&
            MySettings.UseRenew)
        {
            Renew.Cast();
        }
    }

    private void Decast()
    {
        if (MySettings.UseArcaneTorrentForDecast && ArcaneTorrent.KnownSpell && ObjectManager.Target.GetDistance < 8 && ArcaneTorrent.IsSpellUsable
            && ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe
            && ObjectManager.Me.HealthPercent <= MySettings.UseArcaneTorrentForDecastAtPercentage)
        {
            ArcaneTorrent.Cast();
            return;
        }
        if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && MySettings.UseSilence
            && Silence.KnownSpell && Silence.IsSpellUsable && Silence.IsHostileDistanceGood
            && ObjectManager.Target.HealthPercent <= MySettings.UseSilenceAtPercentage)
        {
            Silence.Cast();
            return;
        }
        if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && MySettings.UsePsychicHorror
            && PsychicHorror.KnownSpell && PsychicHorror.IsSpellUsable && PsychicHorror.IsHostileDistanceGood
            && ObjectManager.Target.HealthPercent <= MySettings.UsePsychicHorrorAtPercentage)
        {
            PsychicHorror.Cast();
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
        if (PowerInfusion.IsSpellUsable && PowerInfusion.KnownSpell
            && MySettings.UsePowerInfusion && ObjectManager.Target.GetDistance <= 40f)
        {
            PowerInfusion.Cast();
            return;
        }
        if (Shadowfiend.IsSpellUsable && Shadowfiend.KnownSpell && Shadowfiend.IsHostileDistanceGood
            && MySettings.UseShadowfiend)
        {
            Shadowfiend.Cast();
        }
    }

    private void DPSCycle()
    {
        Usefuls.SleepGlobalCooldown();
        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            if (ObjectManager.Me.ManaPercentage <= MySettings.UseArcaneTorrentForResourceAtPercentage && ArcaneTorrent.KnownSpell && ArcaneTorrent.IsSpellUsable
                && MySettings.UseArcaneTorrentForResource)
            {
                ArcaneTorrent.Cast();
                return;
            }
            if (ObjectManager.GetNumberAttackPlayer() > 2 && Cascade.IsSpellUsable && Cascade.KnownSpell
                && Cascade.IsHostileDistanceGood && MySettings.UseCascade)
            {
                Cascade.Cast();
                return;
            }
            if (ObjectManager.GetNumberAttackPlayer() > 2 && DivineStar.IsSpellUsable && DivineStar.KnownSpell
                && DivineStar.IsHostileDistanceGood && MySettings.UseDivineStar)
            {
                DivineStar.Cast();
                return;
            }
            if (ObjectManager.GetNumberAttackPlayer() > 2 && Halo.IsSpellUsable && Halo.KnownSpell
                && Halo.IsHostileDistanceGood && MySettings.UseHalo)
            {
                Halo.Cast();
                return;
            }
            if (ObjectManager.GetNumberAttackPlayer() > 4 && MindSear.IsSpellUsable && MindSear.KnownSpell
                && MindSear.IsHostileDistanceGood && !ObjectManager.Me.IsCast && MySettings.UseMindSear)
            {
                MindSear.Cast();
                return;
            }
            if (ShadowWordDeath.IsSpellUsable && ShadowWordDeath.IsHostileDistanceGood && ShadowWordDeath.KnownSpell
                && ObjectManager.Target.HealthPercent < 20 && MySettings.UseShadowWordDeath && ObjectManager.Me.ShadowOrbs < 5)
            {
                ShadowWordDeath.Cast();
                return;
            }
            if (ShadowWordPain.KnownSpell && ShadowWordPain.IsSpellUsable
                && ShadowWordPain.IsHostileDistanceGood && MySettings.UseShadowWordPain && !ClarityofPower.KnownSpell
                && (!ShadowWordPain.TargetHaveBuff || _shadowWordPainTimer.IsReady))
            {
                ShadowWordPain.Cast();
                _shadowWordPainTimer = new Timer(1000*15);
                return;
            }
            if (VampiricTouch.KnownSpell && VampiricTouch.IsSpellUsable
                && VampiricTouch.IsHostileDistanceGood && MySettings.UseVampiricTouch && !ClarityofPower.KnownSpell
                && (!VampiricTouch.TargetHaveBuff || _vampiricTouchTimer.IsReady))
            {
                VampiricTouch.Cast();
                _vampiricTouchTimer = new Timer(1000*12);
                return;
            }
            if (VoidEntropy.KnownSpell && VoidEntropy.IsSpellUsable
                && VoidEntropy.IsHostileDistanceGood && MySettings.UseVoidEntropy
                && (!VoidEntropy.TargetHaveBuff || _voidEntropyTimer.IsReady))
            {
                VoidEntropy.Cast();
                _voidEntropyTimer = new Timer(1000*57);
                return;
            }
            if (MindSpike.IsSpellUsable && MindSpike.IsHostileDistanceGood && MindSpike.KnownSpell &&
                ObjectManager.Me.HaveBuff(87160) && MySettings.UseMindSpike)
            {
                MindSpike.Cast();
                return;
            }
            if (DevouringPlague.KnownSpell && DevouringPlague.IsSpellUsable && DevouringPlague.IsHostileDistanceGood &&
                ObjectManager.Me.ShadowOrbs > 2 && MySettings.UseDevouringPlague
                && (!DevouringPlague.TargetHaveBuff || _devouringPlagueTimer.IsReady))
            {
                DevouringPlague.Cast();
                _devouringPlagueTimer = new Timer(1000*3);
                if (Insanity.KnownSpell)
                {
                    Others.SafeSleep(1000);
                    MindFlay.Cast();
                }
                return;
            }
            if (MindBlast.KnownSpell && MindBlast.IsSpellUsable && MindBlast.IsHostileDistanceGood
                && ObjectManager.Me.ShadowOrbs < 5 && MySettings.UseMindBlast)
            {
                MindBlast.Cast();
                return;
            }
            if (MindSpike.IsSpellUsable && MindSpike.IsHostileDistanceGood && MindSpike.KnownSpell &&
                (ObjectManager.Me.HaveBuff(87160) || ClarityofPower.KnownSpell) && MySettings.UseMindSpike)
            {
                MindSpike.Cast();
                return;
            }
            if (MySettings.UseMindFlay && MindFlay.KnownSpell && MindFlay.IsHostileDistanceGood && MindFlay.IsSpellUsable && !ObjectManager.Me.IsCast
                && (ShadowWordPain.TargetHaveBuff || !MySettings.UseShadowWordPain || !ShadowWordPain.KnownSpell) &&
                (VampiricTouch.TargetHaveBuff || !MySettings.UseVampiricTouch || !VampiricTouch.KnownSpell)
                && !ObjectManager.Me.HaveBuff(87160) && ObjectManager.GetNumberAttackPlayer() < 5
                && ObjectManager.Me.ShadowOrbs < 3 && !ClarityofPower.KnownSpell)
            {
                MindFlay.Cast();
                return;
            }
            // Blizzard API Calls for Mind Flay using Smite Function
            if (MySettings.UseMindFlay && Smite.KnownSpell && Smite.IsHostileDistanceGood && Smite.IsSpellUsable && !ObjectManager.Me.IsCast
                && (ShadowWordPain.TargetHaveBuff || !MySettings.UseShadowWordPain || !ShadowWordPain.KnownSpell) &&
                (VampiricTouch.TargetHaveBuff || !MySettings.UseVampiricTouch || !VampiricTouch.KnownSpell || !_vampiricTouchTimer.IsReady)
                && !ObjectManager.Me.HaveBuff(87160) && ObjectManager.GetNumberAttackPlayer() < 5
                && ObjectManager.Me.ShadowOrbs < 3 && !ClarityofPower.KnownSpell)
            {
                Smite.Cast();
            }

            if (Smite.KnownSpell && Smite.IsSpellUsable && Smite.IsHostileDistanceGood && ObjectManager.Me.Level < 10)
            {
                Smite.Cast();
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

    #region Nested type: PriestShadowSettings

    [Serializable]
    public class PriestShadowSettings : Settings
    {
        public bool DoAvoidMelee = false;
        public int DoAvoidMeleeDistance = 0;
        public bool UseAlchFlask = true;
        public bool UseArcaneTorrentForDecast = true;
        public int UseArcaneTorrentForDecastAtPercentage = 100;
        public bool UseArcaneTorrentForResource = true;
        public int UseArcaneTorrentForResourceAtPercentage = 80;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseCascade = true;
        public bool UseDesperatePrayer = true;
        public int UseDesperatePrayerAtPercentage = 65;
        public bool UseDevouringPlague = true;
        public bool UseDispersionHealth = true;
        public int UseDispersionHealthAtPercentage = 20;
        public bool UseDispersionMana = true;
        public int UseDispersionManaAtPercentage = 60;
        public bool UseDivineStar = true;

        public bool UseFlashHealInCombat = true;
        public int UseFlashHealInCombatAtPercentage = 60;
        public bool UseFlashHealNonCombat = true;
        public int UseFlashHealNonCombatAtPercentage = 95;
        public bool UseGiftoftheNaaru = true;
        public int UseGiftoftheNaaruAtPercentage = 80;
        public bool UseHalo = true;
        public bool UseHymnofHope = true;
        public int UseHymnofHopeAtPercentage = 40;
        public bool UseInnerFire = true;
        public bool UseInnerWill = false;
        public bool UseLevitate = false;

        public bool UseLowCombat = true;
        public bool UseMindBlast = true;
        public bool UseMindFlay = true;
        public bool UseMindSear = true;
        public bool UseMindSpike = true;
        public bool UsePowerInfusion = true;
        public bool UsePowerWordFortitude = true;
        public bool UsePowerWordShield = true;
        public int UsePowerWordShieldAtPercentage = 100;
        public bool UsePrayerofMending = true;
        public int UsePrayerofMendingAtPercentage = 50;
        public bool UsePsychicHorror = true;
        public int UsePsychicHorrorAtPercentage = 100;
        public bool UsePsychicScream = true;
        public int UsePsychicScreamAtPercentage = 20;
        public bool UsePsyfiend = true;
        public int UsePsyfiendAtPercentage = 35;
        public bool UseRenew = true;
        public int UseRenewAtPercentage = 90;
        public bool UseShadowWordDeath = true;
        public bool UseShadowWordInsanity = true;
        public bool UseShadowWordPain = true;
        public bool UseShadowfiend = true;
        public bool UseShadowform = true;
        public bool UseSilence = true;
        public int UseSilenceAtPercentage = 100;
        public bool UseSpectralGuise = true;
        public int UseSpectralGuiseAtPercentage = 70;
        public bool UseStoneform = true;
        public int UseStoneformAtPercentage = 80;
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;
        public bool UseVampiricEmbrace = true;
        public int UseVampiricEmbraceAtPercentage = 80;
        public bool UseVampiricTouch = true;
        public bool UseVoidEntropy = true;
        public bool UseVoidTendrils = true;
        public int UseVoidTendrilsAtPercentage = 35;
        public bool UseWarStomp = true;
        public int UseWarStompAtPercentage = 80;

        public PriestShadowSettings()
        {
            ConfigWinForm("Shadow Priest Settings");
            /* Professions and Racials */
            AddControlInWinForm("Use Arcane Torrent for Interrupt", "UseArcaneTorrentForDecast", "Professions & Racials", "AtPercentage");
            AddControlInWinForm("Use Arcane Torrent for Resource", "UseArcaneTorrentForResource", "Professions & Racials", "AtPercentage");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions and Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions and Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions and Racials", "AtPercentage");
            AddControlInWinForm("Use Lifeblood", "UseLifeblood", "Professions and Racials");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions and Racials", "AtPercentage");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions and Racials", "AtPercentage");
            /* Priest Buffs */
            AddControlInWinForm("Use Inner Fire", "UseInnerFire", "Priest Buffs");
            AddControlInWinForm("Use Inner Will", "UseInnerWill", "Priest Buffs");
            AddControlInWinForm("Use Levitate", "UseLevitate", "Priest Buffs");
            AddControlInWinForm("Use Power Word: Fortitude", "UsePowerWordFortitude", "Priest Buffs");
            AddControlInWinForm("Use Shadowform", "UseShadowform", "Priest Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Cascade", "UseCascade", "Offensive Spell");
            AddControlInWinForm("Use Devouring Plague", "UseDevoringPlague", "Offensive Spell");
            AddControlInWinForm("Use DivineStar", "UseDivineStar", "Offensive Spell");
            AddControlInWinForm("Use Halo", "UseHalo", "Offensive Spell");
            AddControlInWinForm("Use Mind Blast", "UseMindBlast", "Offensive Spell");
            AddControlInWinForm("Use Mind Flay", "UseMindFlay", "Offensive Spell");
            AddControlInWinForm("Use Mind Sear", "UseMindSear", "Offensive Spell");
            AddControlInWinForm("Use Mind Spike", "UseMindSpike", "Offensive Spell");
            AddControlInWinForm("Use Shadow Word: Death", "UseShadowWordDeath", "Offensive Spell");
            AddControlInWinForm("Use Shadow Word: Insanity", "UseShadowWordInsanity", "Offensive Spell");
            AddControlInWinForm("Use Shadow Word: Pain", "UseShadowWordPain", "Offensive Spell");
            AddControlInWinForm("Use Vampiric Touch", "UseVampiricTouch", "Offensive Spell");
            AddControlInWinForm("Use Void Entropy", "UseVoidEntropy", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use Power Infusion", "UsePowerInfusion", "Offensive Cooldown");
            AddControlInWinForm("Use Shadowfiend", "UseShadowfiend", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Dispersion when health low", "UseDispersionHealth", "Defensive Cooldown",
                "AtPercentage");
            AddControlInWinForm("Use Dispersion when mana low", "UseDispersionMana", "Defensive Cooldown",
                "AtPercentage");
            AddControlInWinForm("Use Power Word: Shield", "UsePowerWordShield", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Psychic Horror", "UsePsychicHorror", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Psychic Scream", "UsePsychicScream", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Psyfiend", "UsePsyfiend", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Silence", "UseSilence", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Spectral Guise", "UseSpectralGuise", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Void Tendrils", "UseVoidTendrils", "Defensive Cooldown", "AtPercentage");
            /* Healing Spell */
            AddControlInWinForm("Use Desperate Prayer", "UseDesperatePrayer", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Flash Heal for Regeneration after combat", "UseFlashHealNonCombat", "Healing Spell",
                "AtPercentage");
            AddControlInWinForm("Use Flash Heal during combat", "UseFlashHealInCombat", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Hymn of Hope", "UseHymnofHope", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Prayer of Mending", "UsePrayerofMending", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Renew", "UseRenew", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Vampiric Embrace", "UseVampiricEmbrace", "Healing Spell", "AtPercentage");
            /* Game Settings */
            AddControlInWinForm("Use Low Combat Settings", "UseLowCombat", "Game Settings");
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");

            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
            AddControlInWinForm("Do avoid melee (Off Advised!!)", "DoAvoidMelee", "Game Settings");
            AddControlInWinForm("Avoid melee distance (1 to 4)", "DoAvoidMeleeDistance", "Game Settings");
        }

        public static PriestShadowSettings CurrentSetting { get; set; }

        public static PriestShadowSettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Priest_Shadow.xml";
            if (File.Exists(currentSettingsFile))
            {
                return
                    CurrentSetting = Load<PriestShadowSettings>(currentSettingsFile);
            }
            return new PriestShadowSettings();
        }
    }

    #endregion
}

public class PriestDiscipline
{
    private static PriestDisciplineSettings MySettings = PriestDisciplineSettings.GetSettings();

    #region General Timers & Variables

    private readonly WoWItem _firstTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET);
    private readonly WoWItem _secondTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET, 2);

    private Timer _onCd = new Timer(0);

    #endregion

    #region Professions and Racials

    public readonly Spell Alchemy = new Spell("Alchemy");
    public readonly Spell ArcaneTorrent = new Spell("Arcane Torrent");
    public readonly Spell Berserking = new Spell("Berserking");
    public readonly Spell BloodFury = new Spell("Blood Fury");

    public readonly Spell GiftoftheNaaru = new Spell("Gift of the Naaru");

    public readonly Spell Stoneform = new Spell("Stoneform");
    public readonly Spell WarStomp = new Spell("War Stomp");

    #endregion

    #region Priest Buffs

    public readonly Spell InnerFire = new Spell("Inner Fire");
    public readonly Spell InnerWill = new Spell("Inner Will");
    public readonly Spell Levitate = new Spell("Levitate");
    public readonly Spell PowerWordFortitude = new Spell("Power Word: Fortitude");
    private Timer _levitateTimer = new Timer(0);

    #endregion

    #region Offensive Spell

    public readonly Spell Cascade = new Spell("Cascade");
    public readonly Spell DivineStar = new Spell("Divine Star");
    public readonly Spell Halo = new Spell("Halo");
    public readonly Spell MindSear = new Spell("Mind Sear");
    public readonly Spell PowerWordSolace = new Spell("Power Word: Solace");
    public readonly Spell ShadowWordDeath = new Spell("Shadow Word: Death");
    public readonly Spell ShadowWordPain = new Spell("Shadow Word: Pain");
    public readonly Spell Smite = new Spell("Smite");
    private Timer _shadowWordPainTimer = new Timer(0);

    #endregion

    #region Healing Cooldown

    public readonly Spell Archangel = new Spell("Archangel");
    public readonly Spell InnerFocus = new Spell("Inner Focus");
    public readonly Spell PowerInfusion = new Spell("Power Infusion");
    public readonly Spell Shadowfiend = new Spell("Shadowfiend");

    #endregion

    #region Defensive Cooldown

    public readonly Spell PainSuppression = new Spell("Pain Suppression");
    public readonly Spell PowerWordBarrier = new Spell("Power Word: Barrier");
    public readonly Spell PowerWordShield = new Spell("Power Word: Shield");
    public readonly Spell PsychicScream = new Spell("Psychic Scream");
    public readonly Spell Psyfiend = new Spell("Psyfiend");
    public readonly Spell SpectralGuise = new Spell("Spectral Guise");
    public readonly Spell VoidTendrils = new Spell("Void Tendrils");

    #endregion

    #region Healing Spell

    public readonly Spell DesperatePrayer = new Spell("Desperate Prayer");
    public readonly Spell FlashHeal = new Spell("Flash Heal");
    public readonly Spell GreaterHeal = new Spell("Greater Heal");
    public readonly Spell HealSpell = new Spell("Heal");
    public readonly Spell HolyFire = new Spell("Holy Fire");
    public readonly Spell HymnofHope = new Spell("Hymn of Hope");
    public readonly Spell Penance = new Spell("Penance");
    public readonly Spell PrayerofHealing = new Spell("Prayer of Healing");
    public readonly Spell PrayerofMending = new Spell("Prayer of Mending");
    public readonly Spell Renew = new Spell("Renew");
    public readonly Spell SpiritShell = new Spell("Spirit Shell");
/*
        private Timer _renewTimer = new Timer(0);
*/

    #endregion

    public PriestDiscipline()
    {
        Main.InternalRange = 30.0f;
        Main.InternalAggroRange = 30f;
        MySettings = PriestDisciplineSettings.GetSettings();
        Main.DumpCurrentSettings<PriestDisciplineSettings>(MySettings);
        UInt128 lastTarget = 0;

        while (Main.InternalLoop)
        {
            try
            {
                if (!ObjectManager.Me.IsDead)
                {
                    if (!ObjectManager.Me.IsMounted)
                    {
                        BuffLevitate();
                        if (Fight.InFight && ObjectManager.Me.Target > 0)
                        {
                            if (ObjectManager.Me.Target != lastTarget &&
                                (HolyFire.IsHostileDistanceGood || ShadowWordPain.IsHostileDistanceGood))
                            {
                                Pull();
                                lastTarget = ObjectManager.Me.Target;
                            }
                            else
                            {
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

    private void BuffLevitate()
    {
        if (!ObjectManager.Me.InCombat && Levitate.KnownSpell && Levitate.IsSpellUsable && MySettings.UseLevitate
            && (!Levitate.HaveBuff || _levitateTimer.IsReady))
        {
            Levitate.Cast();
            _levitateTimer = new Timer(1000*60*9);
        }
    }

    private void Pull()
    {
        if (HolyFire.IsSpellUsable && HolyFire.KnownSpell && HolyFire.IsHostileDistanceGood
            && MySettings.UseHolyFire)
        {
            HolyFire.Cast();
            return;
        }
        if (ShadowWordPain.IsSpellUsable && ShadowWordPain.KnownSpell && ShadowWordPain.IsHostileDistanceGood
            && MySettings.UseShadowWordPain)
        {
            ShadowWordPain.Cast();
            _shadowWordPainTimer = new Timer(1000*14);
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
        HealingBurst();
        DPSCycle();
    }

    private void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (PowerWordFortitude.KnownSpell && PowerWordFortitude.IsSpellUsable &&
            !PowerWordFortitude.HaveBuff && MySettings.UsePowerWordFortitude)
        {
            PowerWordFortitude.Cast();
            return;
        }
        if (InnerFire.KnownSpell && InnerFire.IsSpellUsable && !InnerFire.HaveBuff
            && MySettings.UseInnerFire)
        {
            InnerFire.Cast();
            return;
        }
        if (InnerWill.KnownSpell && InnerWill.IsSpellUsable && !InnerWill.HaveBuff
            && !MySettings.UseInnerFire && MySettings.UseInnerWill)
        {
            InnerWill.Cast();
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
        if (ObjectManager.Me.HealthPercent <= MySettings.UsePsychicScreamAtPercentage && PsychicScream.IsSpellUsable &&
            PsychicScream.KnownSpell
            && MySettings.UsePsychicScream)
        {
            PsychicScream.Cast();
            _onCd = new Timer(1000*8);
            return;
        }
        if (ObjectManager.GetNumberAttackPlayer() >= 2 &&
            ObjectManager.Me.HealthPercent <= MySettings.UseVoidTendrilsAtPercentage &&
            VoidTendrils.IsSpellUsable && VoidTendrils.KnownSpell && MySettings.UseVoidTendrils)
        {
            VoidTendrils.Cast();
            _onCd = new Timer(1000*10);
            return;
        }
        if (ObjectManager.GetNumberAttackPlayer() >= 2 &&
            ObjectManager.Me.HealthPercent <= MySettings.UsePsyfiendAtPercentage &&
            Psyfiend.IsSpellUsable && Psyfiend.KnownSpell && MySettings.UsePsyfiend)
        {
            Psyfiend.Cast();
            _onCd = new Timer(1000*10);
            return;
        }
        if (ObjectManager.Me.HealthPercent <= MySettings.UseSpectralGuiseAtPercentage &&
            SpectralGuise.IsSpellUsable && SpectralGuise.KnownSpell
            && MySettings.UseSpectralGuise)
        {
            if (Renew.KnownSpell && Renew.IsSpellUsable && MySettings.UseRenew)
            {
                Renew.Cast();
                Others.SafeSleep(1500);
            }
            SpectralGuise.Cast();
            _onCd = new Timer(1000*3);
            return;
        }
        if (ObjectManager.Me.HealthPercent <= MySettings.UsePowerWordBarrierAtPercentage &&
            PowerWordBarrier.IsSpellUsable && PowerWordBarrier.KnownSpell
            && MySettings.UsePowerWordBarrier)
        {
            SpellManager.CastSpellByIDAndPosition(62618, ObjectManager.Me.Position);
            _onCd = new Timer(1000*10);
            return;
        }
        if (ObjectManager.Me.HealthPercent <= MySettings.UsePainSuppressionAtPercentage &&
            PainSuppression.IsSpellUsable && PainSuppression.KnownSpell
            && MySettings.UsePainSuppression)
        {
            PainSuppression.Cast();
            _onCd = new Timer(1000*8);
            return;
        }
        if (ObjectManager.Me.HealthPercent <= MySettings.UseStoneformAtPercentage &&
            Stoneform.IsSpellUsable && Stoneform.KnownSpell
            && MySettings.UseStoneform)
        {
            Stoneform.Cast();
            _onCd = new Timer(1000*8);
            return;
        }
        if (ObjectManager.Me.HealthPercent <= MySettings.UseWarStompAtPercentage &&
            WarStomp.IsSpellUsable && WarStomp.KnownSpell
            && MySettings.UseWarStomp)
        {
            WarStomp.Cast();
            _onCd = new Timer(1000*2);
        }
    }

    private void Heal()
    {
        if (MySettings.UseArcaneTorrentForResource && ArcaneTorrent.KnownSpell && ArcaneTorrent.IsSpellUsable
            && ObjectManager.Me.ManaPercentage <= MySettings.UseArcaneTorrentForResourceAtPercentage)
        {
            ArcaneTorrent.Cast();
            return;
        }

        if (ObjectManager.Me.HealthPercent <= MySettings.UseFlashHealNonCombatAtPercentage &&
            !ObjectManager.Me.InCombat
            && FlashHeal.KnownSpell && FlashHeal.IsSpellUsable && MySettings.UseFlashHealNonCombat)
        {
            FlashHeal.Cast(false);
            return;
        }
        if (ObjectManager.Me.HealthPercent <= MySettings.UseInnerFocusAtPercentage && InnerFocus.KnownSpell &&
            InnerFocus.IsSpellUsable
            && MySettings.UseInnerFocus && !InnerFocus.HaveBuff)
        {
            InnerFocus.Cast();
            return;
        }
        if (ObjectManager.Me.ManaPercentage <= MySettings.UseHymnofHopeAtPercentage &&
            HymnofHope.KnownSpell
            && HymnofHope.IsSpellUsable && !ObjectManager.Me.InCombat &&
            MySettings.UseHymnofHope)
        {
            HymnofHope.Cast(false);
            return;
        }
        if (ObjectManager.Me.HealthPercent <= MySettings.UseDesperatePrayerAtPercentage &&
            DesperatePrayer.KnownSpell && DesperatePrayer.IsSpellUsable
            && MySettings.UseDesperatePrayer)
        {
            DesperatePrayer.Cast();
            return;
        }
        if (ObjectManager.Me.HealthPercent <= MySettings.UseFlashHealInCombatAtPercentage &&
            FlashHeal.KnownSpell && FlashHeal.IsSpellUsable
            && MySettings.UseFlashHealInCombat)
        {
            FlashHeal.Cast();
            return;
        }
        if (ObjectManager.Me.HealthPercent <= MySettings.UseGreaterHealAtPercentage &&
            GreaterHeal.KnownSpell && GreaterHeal.IsSpellUsable
            && MySettings.UseGreaterHeal)
        {
            GreaterHeal.Cast();
            return;
        }
        if (ObjectManager.Me.HealthPercent <= MySettings.UseGiftoftheNaaruAtPercentage &&
            GiftoftheNaaru.IsSpellUsable && GiftoftheNaaru.KnownSpell
            && MySettings.UseGiftoftheNaaru)
        {
            GiftoftheNaaru.Cast();
            return;
        }
        if (PowerWordShield.KnownSpell && PowerWordShield.IsSpellUsable
            && !PowerWordShield.HaveBuff && MySettings.UsePowerWordShield
            && !ObjectManager.Me.HaveBuff(6788) &&
            ObjectManager.Me.HealthPercent <= MySettings.UsePowerWordShieldAtPercentage
            && (ObjectManager.Me.InCombat || ObjectManager.Me.GetMove))
        {
            PowerWordShield.Cast();
            return;
        }
        if (ObjectManager.Me.HealthPercent <= MySettings.UsePrayerofHealingAtPercentage &&
            PrayerofHealing.KnownSpell && PrayerofHealing.IsSpellUsable
            && MySettings.UsePrayerofHealing)
        {
            PrayerofHealing.Cast();
            return;
        }
        if (ObjectManager.Me.HealthPercent <= MySettings.UsePrayerofMendingAtPercentage &&
            PrayerofMending.KnownSpell && PrayerofMending.IsSpellUsable
            && MySettings.UsePrayerofMending)
        {
            PrayerofMending.Cast();
            return;
        }
        if (ObjectManager.Me.HealthPercent <= MySettings.UseHealAtPercentage &&
            HealSpell.KnownSpell && HealSpell.IsSpellUsable
            && (MySettings.UseHeal || !GreaterHeal.KnownSpell))
        {
            HealSpell.Cast();
            return;
        }
        if (Renew.KnownSpell && Renew.IsSpellUsable && !Renew.HaveBuff &&
            ObjectManager.Me.HealthPercent <= MySettings.UseRenewAtPercentage &&
            MySettings.UseRenew)
        {
            Renew.Cast();
        }
    }

    private void Decast()
    {
        if (MySettings.UseArcaneTorrentForDecast && ArcaneTorrent.KnownSpell && ObjectManager.Target.GetDistance < 8 && ArcaneTorrent.IsSpellUsable
            && ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe
            && ObjectManager.Me.HealthPercent <= MySettings.UseArcaneTorrentForDecastAtPercentage)
        {
            ArcaneTorrent.Cast();
        }
    }

    private void HealingBurst()
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
        if (PowerInfusion.IsSpellUsable && PowerInfusion.KnownSpell
            && MySettings.UsePowerInfusion && ObjectManager.Target.GetDistance <= 40f)
        {
            PowerInfusion.Cast();
            return;
        }
        if (Archangel.IsSpellUsable && Archangel.KnownSpell && ObjectManager.Me.BuffStack(81661) > 4
            && MySettings.UseArchangel && ObjectManager.Target.GetDistance <= 40f)
        {
            Archangel.Cast();
            return;
        }
        if (SpiritShell.IsSpellUsable && SpiritShell.KnownSpell && ObjectManager.Me.HealthPercent > 80
            && MySettings.UseSpiritShell && ObjectManager.Target.InCombat)
        {
            SpiritShell.Cast();
            return;
        }
        if (Shadowfiend.IsSpellUsable && Shadowfiend.KnownSpell && Shadowfiend.IsHostileDistanceGood
            && MySettings.UseShadowfiend)
        {
            Shadowfiend.Cast();
        }
    }

    private void DPSCycle()
    {
        Usefuls.SleepGlobalCooldown();
        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            if (MySettings.UseArcaneTorrentForResource && ObjectManager.Me.ManaPercentage <= MySettings.UseArcaneTorrentForResourceAtPercentage && ArcaneTorrent.IsSpellUsable)
            {
                ArcaneTorrent.Cast();
                return;
            }
            if (MySettings.UseShadowWordPain && ShadowWordPain.IsSpellUsable && ShadowWordPain.IsHostileDistanceGood &&
                (!ShadowWordPain.TargetHaveBuff || ObjectManager.Target.AuraIsActiveAndExpireInLessThanMs(ShadowWordPain.Id, 2000)))
            {
                ShadowWordPain.Cast();
                return;
            }
            if (MySettings.UsePenance && Penance.IsSpellUsable && Penance.IsHostileDistanceGood)
            {
                Penance.Cast();
                return;
            }
            if (MySettings.UseSmite && Smite.IsSpellUsable && Smite.IsHostileDistanceGood)
            {
                Smite.Cast();
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

    #region Nested type: PriestDisciplineSettings

    [Serializable]
    public class PriestDisciplineSettings : Settings
    {
        public bool DoAvoidMelee = false;
        public int DoAvoidMeleeDistance = 0;
        public bool UseAlchFlask = true;
        public bool UseArcaneTorrentForDecast = true;
        public int UseArcaneTorrentForDecastAtPercentage = 100;
        public bool UseArcaneTorrentForResource = true;
        public int UseArcaneTorrentForResourceAtPercentage = 80;
        public bool UseArchangel = true;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseCascade = true;
        public bool UseDesperatePrayer = true;
        public int UseDesperatePrayerAtPercentage = 65;
        public bool UseDivineStar = true;

        public bool UseFlashHealInCombat = true;
        public int UseFlashHealInCombatAtPercentage = 60;
        public bool UseFlashHealNonCombat = true;
        public int UseFlashHealNonCombatAtPercentage = 95;
        public bool UseGiftoftheNaaru = true;
        public int UseGiftoftheNaaruAtPercentage = 80;
        public bool UseGreaterHeal = true;
        public int UseGreaterHealAtPercentage = 70;
        public bool UseHalo = true;
        public bool UseHeal = true;
        public int UseHealAtPercentage = 70;
        public bool UseHolyFire = true;
        public bool UseHymnofHope = true;
        public int UseHymnofHopeAtPercentage = 40;
        public bool UseInnerFire = true;
        public bool UseInnerFocus = true;
        public int UseInnerFocusAtPercentage = 90;
        public bool UseInnerWill = false;
        public bool UseLevitate = false;

        public bool UseMindSear = true;
        public bool UsePainSuppression = true;
        public int UsePainSuppressionAtPercentage = 70;
        public bool UsePenance = true;
        public bool UsePowerInfusion = true;
        public bool UsePowerWordBarrier = true;
        public int UsePowerWordBarrierAtPercentage = 60;
        public bool UsePowerWordFortitude = true;
        public bool UsePowerWordShield = true;
        public int UsePowerWordShieldAtPercentage = 100;
        public bool UsePowerWordSolace = true;
        public bool UsePrayerofHealing = false;
        public int UsePrayerofHealingAtPercentage = 50;
        public bool UsePrayerofMending = true;
        public int UsePrayerofMendingAtPercentage = 50;
        public bool UsePsychicScream = true;
        public int UsePsychicScreamAtPercentage = 20;
        public bool UsePsyfiend = true;
        public int UsePsyfiendAtPercentage = 35;
        public bool UseRenew = true;
        public int UseRenewAtPercentage = 90;
        public bool UseShadowWordDeath = true;
        public bool UseShadowWordPain = true;
        public bool UseShadowfiend = true;
        public bool UseSmite = true;
        public bool UseSpectralGuise = true;
        public int UseSpectralGuiseAtPercentage = 70;
        public bool UseSpiritShell = true;
        public bool UseStoneform = true;
        public int UseStoneformAtPercentage = 80;
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;
        public bool UseVoidTendrils = true;
        public int UseVoidTendrilsAtPercentage = 35;
        public bool UseWarStomp = true;
        public int UseWarStompAtPercentage = 80;

        public PriestDisciplineSettings()
        {
            ConfigWinForm("Discipline Priest Settings");
            /* Professions and Racials */
            AddControlInWinForm("Use Arcane Torrent for Interrupt", "UseArcaneTorrentForDecast", "Professions & Racials", "AtPercentage");
            AddControlInWinForm("Use Arcane Torrent for Resource", "UseArcaneTorrentForResource", "Professions & Racials", "AtPercentage");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions and Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions and Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions and Racials", "AtPercentage");
            AddControlInWinForm("Use Lifeblood", "UseLifeblood", "Professions and Racials");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions and Racials", "AtPercentage");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions and Racials", "AtPercentage");
            /* Priest Buffs */
            AddControlInWinForm("Use Inner Fire", "UseInnerFire", "Priest Buffs");
            AddControlInWinForm("Use Inner Will", "UseInnerWill", "Priest Buffs");
            AddControlInWinForm("Use Levitate", "UseLevitate", "Priest Buffs");
            AddControlInWinForm("Use Power Word: Fortitude", "UsePowerWordFortitude", "Priest Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Cascade", "UseCascade", "Offensive Spell");
            AddControlInWinForm("Use Divine Star", "Use Divine Star", "Offensive Spell");
            AddControlInWinForm("Use Halo", "UseHalo", "Offensive Spell");
            AddControlInWinForm("Use Holy Fire", "UseHolyFire", "Offensive Spell");
            AddControlInWinForm("Use Mind Sear", "UseMindSear", "Offensive Spell");
            AddControlInWinForm("Use Penance", "UsePenance", "Offensive Spell");
            AddControlInWinForm("Use Shadow Word: Death", "UseShadowWordDeath", "Offensive Spell");
            AddControlInWinForm("Use Shadow Word: Pain", "UseShadowWordPain", "Offensive Spell");
            AddControlInWinForm("Use Smite", "UseSmite", "Offensive Spell");
            /* Healing Cooldown */
            AddControlInWinForm("Use Archangel", "UseArchangel", "Healing Cooldown");
            AddControlInWinForm("Use Power Infusion", "UsePowerInfusion", "Healing Cooldown");
            AddControlInWinForm("Use Shadowfiend", "UseShadowfiend", "Healing Cooldown");
            AddControlInWinForm("Use Spirit Shell", "UseSpiritShell", "Healing Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Pain Suppression", "UsePainSuppression", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Power Word: Barrier", "UsePowerWordBarrier", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Power Word: Shield", "UsePowerWordShield", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Psychic Scream", "UsePsychicScream", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Psyfiend", "UsePsyfiend", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Spectral Guise", "UseSpectralGuise", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Void Tendrils", "UseVoidTendrils", "Defensive Cooldown", "AtPercentage");
            /* Healing Spell */
            AddControlInWinForm("Use Desperate Prayer", "UseDesperatePrayer", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Flash Heal for Regeneration after combat", "UseFlashHealNonCombat", "Healing Spell",
                "AtPercentage");
            AddControlInWinForm("Use Flash Heal during combat", "UseFlashHealInCombat", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Greater Heal", "UseGreaterHeal", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Heal", "UseHeal", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Hymn of Hope", "UseHymnofHope", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Inner Focus", "UseInnerFocus", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Prayer of Mending", "UsePrayerofMending", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Renew", "UseRenew", "Healing Spell", "AtPercentage");
            /* Game Settings */
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");

            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
            AddControlInWinForm("Do avoid melee (Off Advised!!)", "DoAvoidMelee", "Game Settings");
            AddControlInWinForm("Avoid melee distance (1 to 4)", "DoAvoidMeleeDistance", "Game Settings");
        }

        public static PriestDisciplineSettings CurrentSetting { get; set; }

        public static PriestDisciplineSettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Priest_Discipline.xml";
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
    private static PriestHolySettings MySettings = PriestHolySettings.GetSettings();

    #region General Timers & Variables

    private readonly WoWItem _firstTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET);
    private readonly WoWItem _secondTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET, 2);

    private Timer _onCd = new Timer(0);

    #endregion

    #region Professions and Racials

    public readonly Spell Alchemy = new Spell("Alchemy");
    public readonly Spell ArcaneTorrent = new Spell("Arcane Torrent");
    public readonly Spell Berserking = new Spell("Berserking");
    public readonly Spell BloodFury = new Spell("Blood Fury");

    public readonly Spell GiftoftheNaaru = new Spell("Gift of the Naaru");

    public readonly Spell Stoneform = new Spell("Stoneform");
    public readonly Spell WarStomp = new Spell("War Stomp");

    #endregion

    #region Priest Buffs

    public readonly Spell ChakraChastise = new Spell("Chakra: Chastise");
    public readonly Spell ChakraSanctuary = new Spell("Chakra: Sanctuary");
    public readonly Spell ChakraSerenity = new Spell("Chakra: Serenity");
    public readonly Spell InnerFire = new Spell("Inner Fire");
    public readonly Spell InnerWill = new Spell("Inner Will");
    public readonly Spell Levitate = new Spell("Levitate");
    public readonly Spell PowerWordFortitude = new Spell("Power Word: Fortitude");
    private Timer _levitateTimer = new Timer(0);

    #endregion

    #region Offensive Spell

    public readonly Spell Cascade = new Spell("Cascade");
    public readonly Spell DivineStar = new Spell("Divine Star");
    public readonly Spell Halo = new Spell("Halo");
    public readonly Spell HolyWordChastise = new Spell("Holy Word: Chastise");
    public readonly Spell MindSear = new Spell("Mind Sear");
    public readonly Spell PowerWordSolace = new Spell("Power Word: Solace");
    public readonly Spell ShadowWordDeath = new Spell("Shadow Word: Death");
    public readonly Spell ShadowWordPain = new Spell("Shadow Word: Pain");
    public readonly Spell Smite = new Spell("Smite");
    private Timer _shadowWordPainTimer = new Timer(0);

    #endregion

    #region Healing Cooldown

    public readonly Spell DivineHymn = new Spell("Divine Hymn");
    public readonly Spell LightWell = new Spell("Light Well");
    public readonly Spell PowerInfusion = new Spell("Power Infusion");
    public readonly Spell Shadowfiend = new Spell("Shadowfiend");

    #endregion

    #region Defensive Cooldown

    public readonly Spell GuardianSpirit = new Spell("Guardian Spirit");
    public readonly Spell PowerWordShield = new Spell("Power Word: Shield");
    public readonly Spell PsychicScream = new Spell("Psychic Scream");
    public readonly Spell Psyfiend = new Spell("Psyfiend");
    public readonly Spell SpectralGuise = new Spell("Spectral Guise");
    public readonly Spell VoidTendrils = new Spell("Void Tendrils");

    #endregion

    #region Healing Spell

    public readonly Spell CircleofHealing = new Spell("Circle of Healing");
    public readonly Spell DesperatePrayer = new Spell("Desperate Prayer");
    public readonly Spell FlashHeal = new Spell("Flash Heal");
    public readonly Spell GreaterHeal = new Spell("Greater Heal");
    public readonly Spell HealSpell = new Spell("Heal");
    public readonly Spell HolyFire = new Spell("Holy Fire");
    public readonly Spell HymnofHope = new Spell("Hymn of Hope");
    public readonly Spell PrayerofHealing = new Spell("Prayer of Healing");
    public readonly Spell PrayerofMending = new Spell("Prayer of Mending");
    public readonly Spell Renew = new Spell("Renew");
/*
        private Timer _renewTimer = new Timer(0);
*/

    #endregion

    public PriestHoly()
    {
        Main.InternalRange = 30.0f;
        Main.InternalAggroRange = 30.0f;
        MySettings = PriestHolySettings.GetSettings();
        Main.DumpCurrentSettings<PriestHolySettings>(MySettings);
        UInt128 lastTarget = 0;

        while (Main.InternalLoop)
        {
            try
            {
                if (!ObjectManager.Me.IsDead)
                {
                    if (!ObjectManager.Me.IsMounted)
                    {
                        BuffLevitate();
                        if (Fight.InFight && ObjectManager.Me.Target > 0)
                        {
                            if (ObjectManager.Me.Target != lastTarget &&
                                (HolyFire.IsHostileDistanceGood || ShadowWordPain.IsHostileDistanceGood))
                            {
                                Pull();
                                lastTarget = ObjectManager.Me.Target;
                            }
                            else
                            {
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

    private void BuffLevitate()
    {
        if (!ObjectManager.Me.InCombat && Levitate.KnownSpell && Levitate.IsSpellUsable && MySettings.UseLevitate
            && (!Levitate.HaveBuff || _levitateTimer.IsReady))
        {
            Levitate.Cast();
            _levitateTimer = new Timer(1000*60*9);
        }
    }

    private void Pull()
    {
        if (HolyFire.IsSpellUsable && HolyFire.KnownSpell && HolyFire.IsHostileDistanceGood
            && MySettings.UseHolyFire)
        {
            HolyFire.Cast();
            return;
        }
        if (ShadowWordPain.IsSpellUsable && ShadowWordPain.KnownSpell && ShadowWordPain.IsHostileDistanceGood
            && MySettings.UseShadowWordPain)
        {
            ShadowWordPain.Cast();
            _shadowWordPainTimer = new Timer(1000*14);
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
        HealingBurst();
        DPSCycle();
    }

    private void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (PowerWordFortitude.KnownSpell && PowerWordFortitude.IsSpellUsable &&
            !PowerWordFortitude.HaveBuff && MySettings.UsePowerWordFortitude)
        {
            PowerWordFortitude.Cast();
            return;
        }
        if (InnerFire.KnownSpell && InnerFire.IsSpellUsable && !InnerFire.HaveBuff
            && MySettings.UseInnerFire)
        {
            InnerFire.Cast();
            return;
        }
        if (InnerWill.KnownSpell && InnerWill.IsSpellUsable && !InnerWill.HaveBuff
            && !MySettings.UseInnerFire && MySettings.UseInnerWill)
        {
            InnerWill.Cast();
            return;
        }
        if (ChakraChastise.KnownSpell && ChakraChastise.IsSpellUsable && !ChakraChastise.HaveBuff
            && MySettings.UseChakraChastise)
        {
            ChakraChastise.Cast();
            return;
        }
        if (ChakraSanctuary.KnownSpell && ChakraSanctuary.IsSpellUsable && !ChakraSanctuary.HaveBuff
            && !MySettings.UseChakraChastise && MySettings.UseChakraSanctuary)
        {
            ChakraSanctuary.Cast();
            return;
        }
        if (ChakraSerenity.KnownSpell && ChakraSerenity.IsSpellUsable && !ChakraSerenity.HaveBuff
            && !MySettings.UseChakraChastise && !MySettings.UseChakraSanctuary && MySettings.UseChakraSerenity)
        {
            ChakraSerenity.Cast();
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
        if (ObjectManager.Me.HealthPercent <= MySettings.UsePsychicScreamAtPercentage && PsychicScream.IsSpellUsable &&
            PsychicScream.KnownSpell
            && MySettings.UsePsychicScream)
        {
            PsychicScream.Cast();
            _onCd = new Timer(1000*8);
            return;
        }
        if (ObjectManager.Me.HealthPercent <= MySettings.UseGuardianSpiritAtPercentage && GuardianSpirit.KnownSpell &&
            GuardianSpirit.IsSpellUsable
            && MySettings.UseGuardianSpirit)
        {
            GuardianSpirit.Cast();
            return;
        }
        if (ObjectManager.GetNumberAttackPlayer() >= 2 &&
            ObjectManager.Me.HealthPercent <= MySettings.UseVoidTendrilsAtPercentage &&
            VoidTendrils.IsSpellUsable && VoidTendrils.KnownSpell && MySettings.UseVoidTendrils)
        {
            VoidTendrils.Cast();
            _onCd = new Timer(1000*10);
            return;
        }
        if (ObjectManager.GetNumberAttackPlayer() >= 2 &&
            ObjectManager.Me.HealthPercent <= MySettings.UsePsyfiendAtPercentage &&
            Psyfiend.IsSpellUsable && Psyfiend.KnownSpell && MySettings.UsePsyfiend)
        {
            Psyfiend.Cast();
            _onCd = new Timer(1000*10);
            return;
        }
        if (ObjectManager.Me.HealthPercent <= MySettings.UseSpectralGuiseAtPercentage &&
            SpectralGuise.IsSpellUsable && SpectralGuise.KnownSpell
            && MySettings.UseSpectralGuise)
        {
            if (Renew.KnownSpell && Renew.IsSpellUsable && MySettings.UseRenew)
            {
                Renew.Cast();
                Others.SafeSleep(1500);
            }
            SpectralGuise.Cast();
            _onCd = new Timer(1000*3);
            return;
        }
        if (ObjectManager.Me.HealthPercent <= MySettings.UseStoneformAtPercentage &&
            Stoneform.IsSpellUsable && Stoneform.KnownSpell
            && MySettings.UseStoneform)
        {
            Stoneform.Cast();
            _onCd = new Timer(1000*8);
            return;
        }
        if (ObjectManager.Me.HealthPercent <= MySettings.UseWarStompAtPercentage &&
            WarStomp.IsSpellUsable && WarStomp.KnownSpell
            && MySettings.UseWarStomp)
        {
            WarStomp.Cast();
            _onCd = new Timer(1000*2);
        }
    }

    private void Heal()
    {
        if (MySettings.UseArcaneTorrentForResource && ArcaneTorrent.KnownSpell && ArcaneTorrent.IsSpellUsable
            && ObjectManager.Me.ManaPercentage <= MySettings.UseArcaneTorrentForResourceAtPercentage)
        {
            ArcaneTorrent.Cast();
            return;
        }

        if (ObjectManager.Me.HealthPercent <= MySettings.UseFlashHealNonCombatAtPercentage &&
            !ObjectManager.Me.InCombat
            && FlashHeal.KnownSpell && FlashHeal.IsSpellUsable && MySettings.UseFlashHealNonCombat)
        {
            FlashHeal.Cast(false);
            return;
        }
        if (ObjectManager.Me.HealthPercent <= MySettings.UseDivineHymnAtPercentage && DivineHymn.KnownSpell &&
            DivineHymn.IsSpellUsable
            && MySettings.UseDivineHymn)
        {
            DivineHymn.Cast();
            return;
        }
        if (ObjectManager.Me.ManaPercentage <= MySettings.UseHymnofHopeAtPercentage &&
            HymnofHope.KnownSpell
            && HymnofHope.IsSpellUsable && !ObjectManager.Me.InCombat &&
            MySettings.UseHymnofHope)
        {
            HymnofHope.Cast(false);
            return;
        }
        if (ObjectManager.Me.HealthPercent <= MySettings.UseDesperatePrayerAtPercentage &&
            DesperatePrayer.KnownSpell && DesperatePrayer.IsSpellUsable
            && MySettings.UseDesperatePrayer)
        {
            DesperatePrayer.Cast();
            return;
        }
        if (ObjectManager.Me.HealthPercent <= MySettings.UseFlashHealInCombatAtPercentage &&
            FlashHeal.KnownSpell && FlashHeal.IsSpellUsable
            && MySettings.UseFlashHealInCombat)
        {
            FlashHeal.Cast();
            return;
        }
        if (ObjectManager.Me.HealthPercent <= MySettings.UseGreaterHealAtPercentage &&
            GreaterHeal.KnownSpell && GreaterHeal.IsSpellUsable
            && MySettings.UseGreaterHeal)
        {
            GreaterHeal.Cast();
            return;
        }
        if (ObjectManager.Me.HealthPercent <= MySettings.UseGiftoftheNaaruAtPercentage &&
            GiftoftheNaaru.IsSpellUsable && GiftoftheNaaru.KnownSpell
            && MySettings.UseGiftoftheNaaru)
        {
            GiftoftheNaaru.Cast();
            return;
        }
        if (PowerWordShield.KnownSpell && PowerWordShield.IsSpellUsable
            && !PowerWordShield.HaveBuff && MySettings.UsePowerWordShield
            && !ObjectManager.Me.HaveBuff(6788) &&
            ObjectManager.Me.HealthPercent <= MySettings.UsePowerWordShieldAtPercentage
            && (ObjectManager.Me.InCombat || ObjectManager.Me.GetMove))
        {
            PowerWordShield.Cast();
            return;
        }
        if (ObjectManager.Me.HealthPercent <= MySettings.UsePrayerofHealingAtPercentage &&
            PrayerofHealing.KnownSpell && PrayerofHealing.IsSpellUsable
            && MySettings.UsePrayerofHealing)
        {
            PrayerofHealing.Cast();
            return;
        }
        if (ObjectManager.Me.HealthPercent <= MySettings.UseCircleofHealingAtPercentage &&
            CircleofHealing.KnownSpell && CircleofHealing.IsSpellUsable
            && MySettings.UseCircleofHealing)
        {
            SpellManager.CastSpellByIDAndPosition(34861, ObjectManager.Me.Position);
            return;
        }
        if (ObjectManager.Me.HealthPercent <=
            MySettings.UsePrayerofMendingAtPercentage &&
            PrayerofMending.KnownSpell && PrayerofMending.IsSpellUsable
            && MySettings.UsePrayerofMending)
        {
            PrayerofMending.Cast();
            return;
        }
        if (ObjectManager.Me.HealthPercent <= MySettings.UseHealAtPercentage &&
            HealSpell.KnownSpell && HealSpell.IsSpellUsable
            && (MySettings.UseHeal || !GreaterHeal.KnownSpell))
        {
            HealSpell.Cast();
            return;
        }
        if (LightWell.KnownSpell && LightWell.IsSpellUsable &&
            MySettings.UseGlyphofLightspring
            &&
            ObjectManager.Me.HealthPercent <=
            MySettings.UseLightWellAtPercentage && MySettings.UseLightWell)
        {
            SpellManager.CastSpellByIDAndPosition(724,
                ObjectManager.Target
                    .Position);
            return;
        }
        if (Renew.KnownSpell && Renew.IsSpellUsable && !Renew.HaveBuff &&
            ObjectManager.Me.HealthPercent <=
            MySettings.UseRenewAtPercentage && MySettings.UseRenew)
        {
            Renew.Cast();
        }
    }

    private void Decast()
    {
        if (MySettings.UseArcaneTorrentForDecast && ArcaneTorrent.KnownSpell && ObjectManager.Target.GetDistance < 8 && ArcaneTorrent.IsSpellUsable
            && ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe
            && ObjectManager.Me.HealthPercent <= MySettings.UseArcaneTorrentForDecastAtPercentage)
        {
            ArcaneTorrent.Cast();
        }
    }

    private void HealingBurst()
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
        if (PowerInfusion.IsSpellUsable && PowerInfusion.KnownSpell
            && MySettings.UsePowerInfusion && ObjectManager.Target.GetDistance <= 40f)
        {
            PowerInfusion.Cast();
            return;
        }
        if (Shadowfiend.IsSpellUsable && Shadowfiend.KnownSpell && Shadowfiend.IsHostileDistanceGood
            && MySettings.UseShadowfiend)
        {
            Shadowfiend.Cast();
        }
    }

    private void DPSCycle()
    {
        Usefuls.SleepGlobalCooldown();
        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            if (ObjectManager.Me.ManaPercentage <= MySettings.UseArcaneTorrentForResourceAtPercentage && ArcaneTorrent.KnownSpell && ArcaneTorrent.IsSpellUsable
                && MySettings.UseArcaneTorrentForResource)
            {
                ArcaneTorrent.Cast();
                return;
            }
            if (ObjectManager.GetNumberAttackPlayer() > 2 && Cascade.IsSpellUsable && Cascade.KnownSpell
                && Cascade.IsHostileDistanceGood && MySettings.UseCascade)
            {
                Cascade.Cast();
                return;
            }
            if (ObjectManager.GetNumberAttackPlayer() > 2 && DivineStar.IsSpellUsable && DivineStar.KnownSpell
                && DivineStar.IsHostileDistanceGood && MySettings.UseDivineStar)
            {
                DivineStar.Cast();
                return;
            }
            if (ObjectManager.GetNumberAttackPlayer() > 2 && Halo.IsSpellUsable && Halo.KnownSpell
                && Halo.IsHostileDistanceGood && MySettings.UseHalo)
            {
                Halo.Cast();
                return;
            }
            if (ObjectManager.GetNumberAttackPlayer() > 4 && MindSear.IsSpellUsable && MindSear.KnownSpell
                && MindSear.IsHostileDistanceGood && !ObjectManager.Me.IsCast && MySettings.UseMindSear)
            {
                MindSear.Cast();
                return;
            }
            if (ShadowWordDeath.IsSpellUsable && ShadowWordDeath.IsHostileDistanceGood && ShadowWordDeath.KnownSpell
                && ObjectManager.Target.HealthPercent < 20 && MySettings.UseShadowWordDeath)
            {
                ShadowWordDeath.Cast();
                return;
            }
            if (ShadowWordPain.KnownSpell && ShadowWordPain.IsSpellUsable
                && ShadowWordPain.IsHostileDistanceGood && MySettings.UseShadowWordPain
                && (!ShadowWordPain.TargetHaveBuff || _shadowWordPainTimer.IsReady))
            {
                ShadowWordPain.Cast();
                _shadowWordPainTimer = new Timer(1000*14);
                return;
            }
            if (PowerWordSolace.KnownSpell && PowerWordSolace.IsHostileDistanceGood
                && PowerWordSolace.IsSpellUsable && MySettings.UsePowerWordSolace
                && ObjectManager.Me.ManaPercentage < 50)
            {
                PowerWordSolace.Cast();
                return;
            }
            if (HolyWordChastise.IsSpellUsable && HolyWordChastise.IsHostileDistanceGood && HolyWordChastise.KnownSpell
                && MySettings.UseHolyWordChastise)
            {
                HolyWordChastise.Cast();
                return;
            }
            if (HolyFire.IsSpellUsable && HolyFire.IsHostileDistanceGood && HolyFire.KnownSpell
                && MySettings.UseHolyFire)
            {
                HolyFire.Cast();
                return;
            }
            if (Smite.IsSpellUsable && Smite.KnownSpell && Smite.IsHostileDistanceGood
                && MySettings.UseSmite && ShadowWordPain.TargetHaveBuff
                && ObjectManager.GetNumberAttackPlayer() < 5)
            {
                Smite.Cast();
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

    #region Nested type: PriestHolySettings

    [Serializable]
    public class PriestHolySettings : Settings
    {
        public bool DoAvoidMelee = false;
        public int DoAvoidMeleeDistance = 0;
        public bool UseAlchFlask = true;
        public bool UseArcaneTorrentForDecast = true;
        public int UseArcaneTorrentForDecastAtPercentage = 100;
        public bool UseArcaneTorrentForResource = true;
        public int UseArcaneTorrentForResourceAtPercentage = 80;
        public bool UseArchangel = true;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseCascade = true;
        public bool UseChakraChastise = true;
        public bool UseChakraSanctuary = false;
        public bool UseChakraSerenity = false;
        public bool UseCircleofHealing = false;
        public int UseCircleofHealingAtPercentage = 50;
        public bool UseDesperatePrayer = true;
        public int UseDesperatePrayerAtPercentage = 65;
        public bool UseDivineHymn = true;
        public int UseDivineHymnAtPercentage = 30;
        public bool UseDivineStar = true;

        public bool UseFlashHealInCombat = true;
        public int UseFlashHealInCombatAtPercentage = 60;
        public bool UseFlashHealNonCombat = true;
        public int UseFlashHealNonCombatAtPercentage = 95;
        public bool UseGiftoftheNaaru = true;
        public int UseGiftoftheNaaruAtPercentage = 80;
        public bool UseGlyphofLightspring = false;
        public bool UseGreaterHeal = true;
        public int UseGreaterHealAtPercentage = 70;
        public bool UseGuardianSpirit = true;
        public int UseGuardianSpiritAtPercentage = 20;
        public bool UseHalo = true;
        public bool UseHeal = true;
        public int UseHealAtPercentage = 70;
        public bool UseHolyFire = true;
        public bool UseHolyWordChastise = true;
        public bool UseHymnofHope = true;
        public int UseHymnofHopeAtPercentage = 40;
        public bool UseInnerFire = true;
        public bool UseInnerWill = false;
        public bool UseLevitate = false;

        public bool UseLightWell = true;
        public int UseLightWellAtPercentage = 95;
        public bool UseMindSear = true;
        public bool UsePowerInfusion = true;
        public bool UsePowerWordFortitude = true;
        public bool UsePowerWordShield = true;
        public int UsePowerWordShieldAtPercentage = 100;
        public bool UsePowerWordSolace = true;
        public bool UsePrayerofHealing = false;
        public int UsePrayerofHealingAtPercentage = 50;
        public bool UsePrayerofMending = true;
        public int UsePrayerofMendingAtPercentage = 50;
        public bool UsePsychicScream = true;
        public int UsePsychicScreamAtPercentage = 20;
        public bool UsePsyfiend = true;
        public int UsePsyfiendAtPercentage = 35;
        public bool UseRenew = true;
        public int UseRenewAtPercentage = 90;
        public bool UseShadowWordDeath = true;
        public bool UseShadowWordPain = true;
        public bool UseShadowfiend = true;
        public bool UseSmite = true;
        public bool UseSpectralGuise = true;
        public int UseSpectralGuiseAtPercentage = 70;
        public bool UseStoneform = true;
        public int UseStoneformAtPercentage = 80;
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;
        public bool UseVoidTendrils = true;
        public int UseVoidTendrilsAtPercentage = 35;
        public bool UseWarStomp = true;
        public int UseWarStompAtPercentage = 80;

        public PriestHolySettings()
        {
            ConfigWinForm("Holy Priest Settings");
            /* Professions and Racials */
            AddControlInWinForm("Use Arcane Torrent for Interrupt", "UseArcaneTorrentForDecast", "Professions & Racials", "AtPercentage");
            AddControlInWinForm("Use Arcane Torrent for Resource", "UseArcaneTorrentForResource", "Professions & Racials", "AtPercentage");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions and Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions and Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions and Racials", "AtPercentage");
            AddControlInWinForm("Use Lifeblood", "UseLifeblood", "Professions and Racials");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions and Racials", "AtPercentage");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions and Racials", "AtPercentage");
            /* Priest Buffs */
            AddControlInWinForm("Use Chakra: Chastise", "UseChakraChastise", "Priest Buffs");
            AddControlInWinForm("Use Chakra: Sanctuary", "UseChakraSanctuary", "Priest Buffs");
            AddControlInWinForm("Use Chakra: Serenity", "UseChakraSerenity", "Priest Buffs");
            AddControlInWinForm("Use Inner Fire", "UseInnerFire", "Priest Buffs");
            AddControlInWinForm("Use Inner Will", "UseInnerWill", "Priest Buffs");
            AddControlInWinForm("Use Levitate", "UseLevitate", "Priest Buffs");
            AddControlInWinForm("Use Power Word: Fortitude", "UsePowerWordFortitude", "Priest Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Cascade", "UseCascade", "Offensive Spell");
            AddControlInWinForm("Use Divine Star", "Use Divine Star", "Offensive Spell");
            AddControlInWinForm("Use Halo", "UseHalo", "Offensive Spell");
            AddControlInWinForm("Use Holy Fire", "UseHolyFire", "Offensive Spell");
            AddControlInWinForm("Use Holy Word: Chastise", "UseHolyWordChastise", "Offensive Spell");
            AddControlInWinForm("Use Mind Sear", "UseMindSear", "Offensive Spell");
            AddControlInWinForm("Use Shadow Word: Death", "UseShadowWordDeath", "Offensive Spell");
            AddControlInWinForm("Use Shadow Word: Pain", "UseShadowWordPain", "Offensive Spell");
            AddControlInWinForm("Use Smite", "UseSmite", "Offensive Spell");
            /* Healing Cooldown */
            AddControlInWinForm("Use Divine Hymn", "UseDivineHymn", "Healing Cooldown", "AtPercentage");
            AddControlInWinForm("Use Light Well", "UseLightWell", "Healing Cooldown", "AtPercentage");
            AddControlInWinForm("Use Power Infusion", "UsePowerInfusion", "Healing Cooldown");
            AddControlInWinForm("Use Shadowfiend", "UseShadowfiend", "Healing Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Guardian Spirit", "UseGuardianSpirit", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Power Word: Shield", "UsePowerWordShield", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Psychic Scream", "UsePsychicScream", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Psyfiend", "UsePsyfiend", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Spectral Guise", "UseSpectralGuise", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Void Tendrils", "UseVoidTendrils", "Defensive Cooldown", "AtPercentage");
            /* Healing Spell */
            AddControlInWinForm("Use Circle of Healing", "UseCircleofHealing", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Desperate Prayer", "UseDesperatePrayer", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Flash Heal for Regeneration after combat", "UseFlashHealNonCombat", "Healing Spell",
                "AtPercentage");
            AddControlInWinForm("Use Flash Heal during combat", "UseFlashHealInCombat", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Greater Heal", "UseGreaterHeal", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Heal", "UseHeal", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Hymn of Hope", "UseHymnofHope", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Prayer of Mending", "UsePrayerofMending", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Renew", "UseRenew", "Healing Spell");
            /* Game Settings */
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");

            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
            AddControlInWinForm("Use Glyph of Lightspring", "UseGlyphofLightspring", "Game Settings");
            AddControlInWinForm("Do avoid melee (Off Advised!!)", "DoAvoidMelee", "Game Settings");
            AddControlInWinForm("Avoid melee distance (1 to 4)", "DoAvoidMeleeDistance", "Game Settings");
        }

        public static PriestHolySettings CurrentSetting { get; set; }

        public static PriestHolySettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Priest_Holy.xml";
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

// ReSharper restore ObjectCreationAsStatement
// ReSharper restore EmptyGeneralCatchClause