/*
* CombatClass for TheNoobBot
* Credit : Vesper, Neo2003, Ryuichiro
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
    internal static float Version = 1.001f;

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
                    #region Priest Specialisation checking

                case WoWClass.Priest:

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
        Logging.WriteDebug("Loaded " + ObjectManager.Me.WowSpecialization() + " " + ObjectManager.Me.WowClass + " Combat Class 1.0");

        // Last field is intentionnally ommited because it's a backing field.
    }
}

#region Priest

public class PriestDiscipline
{
    private static PriestDisciplineSettings MySettings = PriestDisciplineSettings.GetSettings();

    #region General Timers & Variables

    private readonly WoWItem _firstTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET);
    private readonly WoWItem _secondTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET, 2);

    private bool CombatMode = true;

    private Timer DefensiveTimer = new Timer(0);
    private Timer StunTimer = new Timer(0);

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

    #region Buffs

    private readonly Spell Atonement = new Spell("Atonement");

    #endregion

    #region Artifact Spells

    private readonly Spell LightsWrath = new Spell("Light's Wrath");

    #endregion

    #region Offensive Spells

    private readonly Spell DivineStar = new Spell("Divine Star");
    private readonly Spell Halo = new Spell("Halo");
    private readonly Spell Penance = new Spell("Penance");
    private readonly Spell PowerWordSolace = new Spell("Power Word: Solace");
    private readonly Spell PurgetheWicked = new Spell("Purge the Wicked");
    private readonly Spell Schism = new Spell("Schism");
    private readonly Spell ShadowWordPain = new Spell("Shadow Word: Pain");
    private readonly Spell Smite = new Spell("Smite");

    #endregion

    #region Offensive Cooldowns

    private readonly Spell Shadowfiend = new Spell("Shadowfiend");
    private readonly Spell Mindbender = new Spell("Mindbender");
    private readonly Spell PowerInfusion = new Spell("Power Infusion");

    #endregion

    #region Defensive Spells

    private readonly Spell ClarityofWill = new Spell("Clarity of Will");
    private readonly Spell Fade = new Spell("Fade"); //No GCD
    private readonly Spell PainSuppression = new Spell("Pain Suppression"); //No GCD
    private readonly Spell PowerWordBarrier = new Spell("Power Word: Barrier");
    private readonly Spell PowerWordShield = new Spell("Power Word: Shield");
    private readonly Spell Rapture = new Spell("Rapture"); //No GCD
    private readonly Spell ShiningForce = new Spell("Shining Force");

    #endregion

    #region Healing Spell

    private readonly Spell Plea = new Spell("Plea");
    private readonly Spell PowerWordRadiance = new Spell("Power Word: Radiance");
    private readonly Spell ShadowMend = new Spell("Shadow Mend");

    #endregion

    #region Utility Spells

    private readonly Spell AngelicFeather = new Spell("Angelic Feather");
    //private readonly Spell DispelMagic = new Spell("Dispel Magic");
    //private readonly Spell LeapofFaith = new Spell("Leap of Faith");
    //private readonly Spell MassDispel = new Spell("Mass Dispel");
    //private readonly Spell MindControl = new Spell("Mind Control");
    //private readonly Spell PsychicScream = new Spell("Psychic Scream");
    //private readonly Spell PurifyDisease = new Spell("Purify");
    //private readonly Spell ShackleUndead = new Spell("Shackle Undead");

    #endregion

    public PriestDiscipline()
    {
        Main.InternalRange = 39.0f;
        Main.InternalAggroRange = 39.0f;
        Main.InternalLightHealingSpell = Plea;
        MySettings = PriestDisciplineSettings.GetSettings();
        Main.DumpCurrentSettings<PriestDisciplineSettings>(MySettings);
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
                                lastTarget = ObjectManager.Me.Target;

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

    // For Movement Spells (always return after Casting)
    private void Patrolling()
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
            if (!Darkflight.HaveBuff && !AngelicFeather.HaveBuff) // doesn't stack
            {
                if (MySettings.UseDarkflight && Darkflight.IsSpellUsable)
                {
                    Darkflight.Cast();
                    return;
                }
                if (MySettings.UseAngelicFeather && AngelicFeather.IsSpellUsable)
                {
                    AngelicFeather.CastAtPosition(ObjectManager.Me.Position);
                    return;
                }
            }
        }
        else
        {
            //Self Heal for Damage Dealer
            if (nManager.Products.Products.ProductName == "Damage Dealer" && Main.InternalLightHealingSpell.IsSpellUsable &&
                ObjectManager.Me.HealthPercent < 90 && ObjectManager.Target.Guid == ObjectManager.Me.Guid)
            {
                Main.InternalLightHealingSpell.CastOnSelf();
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
                GiftoftheNaaru.CastOnSelf();
                return true;
            }
            //Cast Plea when
            if (ObjectManager.Me.HealthPercent < MySettings.UsePleaBelowPercentage && ShadowMend.IsSpellUsable &&
                //you are moving
                ObjectManager.Me.GetMove)
            {
                Plea.CastOnSelf();
                return true;
            }
            //Shadow Mend
            if (ObjectManager.Me.HealthPercent < MySettings.UseShadowMendBelowPercentage && ShadowMend.IsSpellUsable)
            {
                ShadowMend.CastOnSelf();
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
                        WarStomp.CastOnSelf();
                        StunTimer = new Timer(1000*2.5);
                        return true;
                    }
                }
                //Mitigate Damage
                if (ObjectManager.Me.HealthPercent < MySettings.UseStoneformBelowPercentage && Stoneform.IsSpellUsable)
                {
                    Stoneform.CastOnSelf();
                    DefensiveTimer = new Timer(1000*8);
                    return true;
                }
                //Shining Force
                if (ObjectManager.Me.HealthPercent < MySettings.UseShiningForceBelowPercentage && ShiningForce.IsSpellUsable &&
                    ObjectManager.Me.GetUnitInSpellRange(10f) >= 1)
                {
                    ShiningForce.CastOnSelf();
                    DefensiveTimer = new Timer(1000*3);
                    return true;
                }
                //Fade
                if (ObjectManager.Me.HealthPercent < MySettings.UseFadeBelowPercentage && Fade.IsSpellUsable)
                {
                    Fade.CastOnSelf();
                    DefensiveTimer = new Timer(1000*10);
                    return true;
                }
                //Power Word: Shield
                if (ObjectManager.Me.HealthPercent < MySettings.UsePowerWordShieldBelowPercentage &&
                    PowerWordShield.IsSpellUsable && !PowerWordShield.HaveBuff)
                {
                    //Rapture
                    if (ObjectManager.Me.HealthPercent < MySettings.UseRaptureBelowPercentage &&
                        Rapture.IsSpellUsable)
                    {
                        Rapture.CastOnSelf();
                    }
                    PowerWordShield.CastOnSelf();
                    return true;
                }
                //Clarity of Will
                if (ObjectManager.Me.HealthPercent < MySettings.UseClarityofWillBelowPercentage &&
                    ClarityofWill.IsSpellUsable && !ClarityofWill.HaveBuff)
                {
                    ClarityofWill.CastOnSelf();
                    return true;
                }
            }
            //Mitigate Damage in Emergency Situations
            //Pain Suppression
            if (ObjectManager.Me.HealthPercent < MySettings.UsePainSuppressionBelowPercentage && PainSuppression.IsSpellUsable)
            {
                PainSuppression.CastOnSelf();
                DefensiveTimer = new Timer(1000*8);
                return true;
            }
            //Power Word: Barrier
            if (ObjectManager.Me.HealthPercent < MySettings.UsePowerWordBarrierBelowPercentage &&
                PowerWordBarrier.IsSpellUsable)
            {
                PowerWordBarrier.CastOnSelf();
                DefensiveTimer = new Timer(1000*10);
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
            //Cast Power Infusion when Voidform is active
            if (MySettings.UsePowerInfusion && PowerInfusion.IsSpellUsable)
            {
                PowerInfusion.Cast();
            }
            //Cast Shadowfiend / Mindbender
            if (Mindbender.KnownSpell)
            {
                if (MySettings.UseMindbender && Mindbender.IsSpellUsable)
                {
                    Mindbender.Cast();
                    return true;
                }
            }
            else if (MySettings.UseShadowfiend && Shadowfiend.IsSpellUsable)
            {
                Shadowfiend.Cast();
                return true;
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

            // Apply Atonement to party
            if (Party.IsInGroup())
            {
                WoWPlayer atonementTarget = new WoWPlayer(0);
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

                    if (!currentPlayer.HaveBuff(Atonement.Ids) && currentPlayer.HealthPercent < 100)
                    {
                        atonementTarget = currentPlayer;
                        break;
                    }
                }
                if (atonementTarget.IsAlive)
                {
                    //1. Apply Power Word: Shield
                    if (MySettings.UsePowerWordShield && PowerWordShield.IsSpellUsable &&
                        PowerWordShield.IsFriendDistanceGood && !atonementTarget.HaveBuff(PowerWordShield.Ids))
                    {
                        //1a. Apply Rapture
                        if (MySettings.UseRapture && Rapture.IsSpellUsable)
                            Rapture.Cast();
                        PowerWordShield.Cast();
                        return;
                    }
                    //2. Cast Plea when
                    if (MySettings.UsePlea && Plea.IsSpellUsable && Plea.IsFriendDistanceGood &&
                        //you are moving
                        ObjectManager.Me.GetMove)
                    {
                        Plea.Cast();
                        return;
                    }
                    //3. Cast Power Word: Radiance
                    if (MySettings.UsePowerWordRadiance &&
                        PowerWordRadiance.IsSpellUsable && Plea.IsFriendDistanceGood &&
                        //it will affect 3 targets
                        ((ObjectManager.Target.GetPlayerInSpellRange(30f) + ObjectManager.Target.GetUnitInSpellRange(30f)) > 2))
                    {
                        PowerWordRadiance.Cast();
                        return;
                    }
                    //4. Cast Plea
                    if (MySettings.UsePlea &&
                        Plea.IsSpellUsable && Plea.IsFriendDistanceGood)
                    {
                        Plea.Cast();
                        return;
                    }
                }
            }


            //1. Cast Schism
            if (MySettings.UseSchism && Schism.IsSpellUsable && Schism.IsHostileDistanceGood)
            {
                Schism.Cast();
                return;
            }
            //2. Cast Penance
            if (MySettings.UsePenance && Penance.IsSpellUsable && Penance.IsHostileDistanceGood)
            {
                Penance.Cast();
                return;
            }
            //3. Cast Light's Wrath
            if (MySettings.UseLightsWrath && LightsWrath.IsSpellUsable && LightsWrath.IsHostileDistanceGood)
            {
                LightsWrath.Cast();
                return;
            }
            //4. Cast Shadow Word: Pain / Purge the Wicked when
            if (MySettings.UseShadowWordPain_PurgetheWicked && ShadowWordPain.IsSpellUsable && ShadowWordPain.IsHostileDistanceGood &&
                //the Dot isn't up
                (!ShadowWordPain.TargetHaveBuffFromMe && !PurgetheWicked.TargetHaveBuffFromMe))
            {
                ShadowWordPain.Cast();
                return;
            }
            //5. Cast Halo when
            if (MySettings.UseHalo && Halo.IsSpellUsable &&
                //you have mulitple targets
                ObjectManager.Target.GetUnitInSpellRange(30f) > 1)
            {
                Halo.Cast();
                return;
            }
            //6. Cast Power Word: Solace
            if (MySettings.UsePowerWordSolace && PowerWordSolace.IsSpellUsable && PowerWordSolace.IsHostileDistanceGood)
            {
                PowerWordSolace.Cast();
                return;
            }
            //7. Cast Smite
            if (MySettings.UseSmite && Smite.IsSpellUsable && Smite.IsHostileDistanceGood)
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

    #region Nested type: PriestDisciplineSettings

    [Serializable]
    public class PriestDisciplineSettings : Settings
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
        public bool UseLightsWrath = true;

        /* Offensive Spells */
        public bool UseHalo = true;
        public bool UsePenance = true;
        public bool UsePowerWordSolace = true;
        public bool UseSchism = true;
        public bool UseShadowWordPain_PurgetheWicked = true;
        public bool UseSmite = true;

        /* Offensive Cooldowns */
        public bool UseMindbender = true;
        public bool UsePowerInfusion = true;
        public bool UseShadowfiend = true;

        /* Atonement Spells */
        public bool UsePlea = true;
        public bool UsePowerWordRadiance = true;
        public bool UsePowerWordShield = true;
        public bool UseRapture = true;

        /* Defensive Spells */
        public int UseClarityofWillBelowPercentage = 40;
        public int UseRaptureBelowPercentage = 10;
        public int UseFadeBelowPercentage = 100; //No GCD
        public int UsePainSuppressionBelowPercentage = 20;
        public int UsePowerWordBarrierBelowPercentage = 60;
        public int UsePowerWordShieldBelowPercentage = 80;
        public int UseShiningForceBelowPercentage = 60;

        /* Healing Spells */
        public int UsePleaBelowPercentage = 40;
        public int UseShadowMendBelowPercentage = 40;

        /* Utility Spells */
        public bool UseAngelicFeather = true;

        /* Game Settings */
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;

        public PriestDisciplineSettings()
        {
            ConfigWinForm("Discipline Priest Settings");
            /* Professions & Racials */
            //AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Darkflight", "UseDarkflight", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaruBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            AddControlInWinForm("Use Stone Form", "UseStoneformBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            AddControlInWinForm("Use War Stomp", "UseWarStompBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            /* Artifact Spells */
            AddControlInWinForm("Use Lights Wrath", "UseLightsWrath", "Artifact Spells");
            /* Offensive Spells */
            AddControlInWinForm("Use Halo", "UseHalo", "Offensive Spells");
            AddControlInWinForm("Use Penance", "UsePenance", "Offensive Spells");
            AddControlInWinForm("Use Power Word: Solace", "UsePowerWordSolace", "Offensive Spells");
            AddControlInWinForm("Use Schism", "UseSchism", "Offensive Spells");
            AddControlInWinForm("Use Shadow Word: Pain & Purge the Wicked", "UseShadowWordPain_PurgetheWicked", "Offensive Spells");
            AddControlInWinForm("Use Smite", "UseSmite", "Offensive Spells");
            /* Offensive Cooldowns */
            AddControlInWinForm("Use Mindbender", "UseMindbender", "Offensive Cooldowns");
            AddControlInWinForm("Use Power Infusion", "UsePowerInfusion", "Offensive Cooldowns");
            AddControlInWinForm("Use Shadowfiend", "UseShadowfiend", "Offensive Cooldowns");
            /* Atonement Spells */
            AddControlInWinForm("Use Plea", "UsePlea", "Atonement Spells");
            AddControlInWinForm("Use Power Word: Radiance", "UsePowerWordRadiance", "Atonement Spells");
            AddControlInWinForm("Use Power Word: Shield", "UsePowerWordShield", "Atonement Spells");
            AddControlInWinForm("Use Rapture", "UseRapture", "Atonement Spells");
            /* Defensive Spells */
            AddControlInWinForm("Use Clarity of Will", "UseClarityofWillBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Rapture", "UseRaptureBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Fade", "UseFadeBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Pain Suppression", "UsePainSuppressionBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Power Word: Barrier", "UsePowerWordBarrierBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Power Word: Shield", "UsePowerWordShieldBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Shining Force", "UseShiningForceBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            /* Healing Spell */
            AddControlInWinForm("Use Plea", "UsePleaBelowPercentage", "Healing Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Shadow Mend", "UseShadowMendBelowPercentage", "Healing Spells", "BelowPercentage", "Life");
            /* Utility Spells */
            AddControlInWinForm("Use Angelic Feather", "UseAngelicFeather", "Utility Spells");
            /* Game Settings */
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");
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

    private bool CombatMode = true;

    private Timer DefensiveTimer = new Timer(0);
    private Timer StunTimer = new Timer(0);

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

    #region Artifact Spells

    //private readonly Spell LightofTuure = new Spell("Light of T'uure");

    #endregion

    #region Offensive Spells

    //private readonly Spell DivineStar = new Spell("Divine Star");
    private readonly Spell Halo = new Spell("Halo");
    private readonly Spell HolyFire = new Spell("Holy Fire");
    private readonly Spell HolyNova = new Spell("Holy Nova");
    private readonly Spell HolyWordChastise = new Spell("Holy Word: Chastise");
    private readonly Spell Smite = new Spell("Smite");

    #endregion

    #region Defensive Spells

    private readonly Spell Fade = new Spell("Fade"); //No GCD
    private readonly Spell GuardianSpirit = new Spell("Guardian Spirit"); //No GCD
    private readonly Spell ShiningForce = new Spell("Shining Force");

    #endregion

    #region Healing Spell

    //private readonly Spell DesperatePrayer = new Spell("Desperate Prayer");
    //private readonly Spell DivineHymn = new Spell("Divine Hymn");
    private readonly Spell FlashHeal = new Spell("Flash Heal");
    private readonly Spell Heal = new Spell("Heal");
    private readonly Spell HolyWordSerenity = new Spell("Holy Word: Serenity");
    //private readonly Spell HolyWordSanctify = new Spell("Holy Word: Sanctify");
    //private readonly Spell PrayerofHealing = new Spell("Prayer of Healing");
    //private readonly Spell PrayerofMending = new Spell("Prayer of Mending");
    private readonly Spell Renew = new Spell("Renew");

    #endregion

    #region Utility Spells

    private readonly Spell AngelicFeather = new Spell("Angelic Feather");
    //private readonly Spell DispelMagic = new Spell("Dispel Magic");
    //private readonly Spell LeapofFaith = new Spell("Leap of Faith");
    //private readonly Spell MassDispel = new Spell("Mass Dispel");
    //private readonly Spell MindControl = new Spell("Mind Control");
    //private readonly Spell PsychicScream = new Spell("Psychic Scream");
    //private readonly Spell PurifyDisease = new Spell("Purify Disease");
    //private readonly Spell ShackleUndead = new Spell("Shackle Undead");
    //private readonly Spell SymbolofHope = new Spell("Symbol of Hope");

    #endregion

    public PriestHoly()
    {
        Main.InternalRange = 30.0f;
        Main.InternalAggroRange = 30.0f;
        Main.InternalLightHealingSpell = Heal;
        MySettings = PriestHolySettings.GetSettings();
        Main.DumpCurrentSettings<PriestHolySettings>(MySettings);
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
                                lastTarget = ObjectManager.Me.Target;

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

    // For Movement Spells (always return after Casting)
    private void Patrolling()
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
            if (!Darkflight.HaveBuff && !AngelicFeather.HaveBuff) // doesn't stack
            {
                if (MySettings.UseDarkflight && Darkflight.IsSpellUsable)
                {
                    Darkflight.Cast();
                    return;
                }
                if (MySettings.UseAngelicFeather && AngelicFeather.IsSpellUsable)
                {
                    AngelicFeather.CastAtPosition(ObjectManager.Me.Position);
                    return;
                }
            }
        }
        else
        {
            //Self Heal for Damage Dealer
            if (nManager.Products.Products.ProductName == "Damage Dealer" && Main.InternalLightHealingSpell.IsSpellUsable &&
                ObjectManager.Me.HealthPercent < 90 && ObjectManager.Target.Guid == ObjectManager.Me.Guid)
            {
                Main.InternalLightHealingSpell.CastOnSelf();
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
                GiftoftheNaaru.CastOnSelf();
                return true;
            }
            //Renew
            if (ObjectManager.Me.HealthPercent < MySettings.UseRenewBelowPercentage && Renew.IsSpellUsable)
            {
                Renew.CastOnSelf();
                return true;
            }
            //Holy Word: Serenity
            if (ObjectManager.Me.HealthPercent < MySettings.UseHolyWordSerenityBelowPercentage && HolyWordSerenity.IsSpellUsable)
            {
                HolyWordSerenity.CastOnSelf();
                return true;
            }
            //Flash Heal
            if (ObjectManager.Me.HealthPercent < MySettings.UseFlashHealBelowPercentage && FlashHeal.IsSpellUsable)
            {
                FlashHeal.CastOnSelf();
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
                //Shining Force
                if (ObjectManager.Me.HealthPercent < MySettings.UseShiningForceBelowPercentage && ShiningForce.IsSpellUsable &&
                    ObjectManager.Me.GetUnitInSpellRange(10f) >= 1)
                {
                    ShiningForce.CastOnSelf();
                    DefensiveTimer = new Timer(1000*3);
                    return true;
                }
                //Fade
                if (ObjectManager.Me.HealthPercent < MySettings.UseFadeBelowPercentage && Fade.IsSpellUsable)
                {
                    Fade.Cast();
                    DefensiveTimer = new Timer(1000*10);
                    return true;
                }
            }
            //Mitigate Damage in Emergency Situations
            //Guardian Spirit
            if (ObjectManager.Me.HealthPercent < MySettings.UseGuardianSpiritBelowPercentage && GuardianSpirit.IsSpellUsable)
            {
                GuardianSpirit.CastOnSelf();
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

            //1. Apply Holy Fire
            if (MySettings.UseHolyFire && HolyFire.IsSpellUsable && HolyFire.IsHostileDistanceGood &&
                HolyFire.TargetBuffStack < 2)
            {
                HolyFire.Cast();
                return;
            }
            //2. Cast Halo when
            if (MySettings.UseHalo && Halo.IsSpellUsable &&
                //you have mulitple targets
                ObjectManager.Target.GetUnitInSpellRange(30f) > 1)
            {
                Halo.Cast();
                return;
            }
            //3. Cast Holy Nova when
            if (MySettings.UseHolyNova && HolyNova.IsSpellUsable &&
                //you have 3 or more targets
                ObjectManager.Target.GetUnitInSpellRange(12f) >= 3)
            {
                HolyNova.Cast();
                return;
            }
            //4. Cast Holy Word: Chastise
            if (MySettings.UseHolyWordChastise && HolyWordChastise.IsSpellUsable && HolyWordChastise.IsHostileDistanceGood)
            {
                HolyWordChastise.Cast();
                return;
            }
            //5. Cast Holy Word: Chastise
            if (MySettings.UseSmite && Smite.IsSpellUsable && Smite.IsHostileDistanceGood)
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

    #region Nested type: PriestHolySettings

    [Serializable]
    public class PriestHolySettings : Settings
    {
        /* Professions & Racials */
        //public bool UseArcaneTorrent = true;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseDarkflight = true;
        public int UseGiftoftheNaaruBelowPercentage = 50;
        public int UseStoneformBelowPercentage = 50;
        public int UseWarStompBelowPercentage = 50;

        /* Offensive Spells */
        public bool UseHalo = true;
        public bool UseHolyFire = true;
        public bool UseHolyNova = true;
        public bool UseHolyWordChastise = true;
        public bool UseSmite = true;

        /* Defensive Spells */
        public int UseFadeBelowPercentage = 100;
        public int UseGuardianSpiritBelowPercentage = 10;
        public int UseShiningForceBelowPercentage = 60;

        /* Healing Spells */
        public int UseFlashHealBelowPercentage = 25;
        public int UseHolyWordSerenityBelowPercentage = 50;
        public int UseRenewBelowPercentage = 90;

        /* Utility Spells */
        public bool UseAngelicFeather = true;

        /* Game Settings */
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;

        public PriestHolySettings()
        {
            ConfigWinForm("Holy Priest Settings");
            /* Professions & Racials */
            //AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Darkflight", "UseDarkflight", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaruBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            AddControlInWinForm("Use Stone Form", "UseStoneformBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            AddControlInWinForm("Use War Stomp", "UseWarStompBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            /* Offensive Spells */
            AddControlInWinForm("Use Halo", "UseHalo", "Offensive Spells");
            AddControlInWinForm("Use Holy Fire", "UseHolyFire", "Offensive Spells");
            AddControlInWinForm("Use Holy Nova", "UseHolyNova", "Offensive Spells");
            AddControlInWinForm("Use Holy Word: Chastise", "UseHolyWordChastise", "Offensive Spells");
            AddControlInWinForm("Use Smite", "UseSmite", "Offensive Spells");
            /* Defensive Spells */
            AddControlInWinForm("Use Fade", "UseFadeBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Guardian Spirit", "UseGuardianSpiritBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Shining Force", "UseShiningForceBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            /* Healing Spell */
            AddControlInWinForm("Use Flash Heal", "UseFlashHealBelowPercentage", "Healing Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Holy Word: Serenity", "UseHolyWordSerenityBelowPercentage", "Healing Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Renew", "UseRenewBelowPercentage", "Healing Spells", "BelowPercentage", "Life");
            /* Utility Spells */
            AddControlInWinForm("Use Angelic Feather", "UseAngelicFeather", "Utility Spells");
            /* Game Settings */
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");
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

public class PriestShadow
{
    private static PriestShadowSettings MySettings = PriestShadowSettings.GetSettings();

    #region General Timers & Variables

    private readonly WoWItem _firstTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET);
    private readonly WoWItem _secondTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET, 2);

    private bool CombatMode = true;

    private Timer DefensiveTimer = new Timer(0);
    private Timer StunTimer = new Timer(0);

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

    private readonly Spell LegacyoftheVoid = new Spell("Legacy of the Void");
    private readonly Spell ReaperofSouls = new Spell("Reaper of Souls");

    #endregion

    #region Buffs

    private readonly Spell LingeringInsanity = new Spell("Lingering Insanity");
    private readonly Spell Voidform = new Spell(194249);

    #endregion

    #region Artifact Spells

    private readonly Spell VoidTorrent = new Spell("Void Torrent");

    #endregion

    #region Offensive Spells

    private readonly Spell MindBlast = new Spell("Mind Blast");
    private readonly Spell MindFlay = new Spell("Mind Flay");
    private readonly Spell MindSear = new Spell("Mind Sear");
    private readonly Spell MindSpike = new Spell("Mind Spike");
    private readonly Spell ShadowCrash = new Spell("Shadow Crash");
    private readonly Spell ShadowWordDeath = new Spell("Shadow Word: Death");
    private readonly Spell ShadowWordPain = new Spell("Shadow Word: Pain");
    private readonly Spell ShadowWordVoid = new Spell("Shadow Word: Void");
    private readonly Spell VampiricTouch = new Spell("Vampiric Touch");
    private readonly Spell VoidBolt = new Spell(228266);
    private readonly Spell VoidEruption = new Spell("Void Eruption");

    #endregion

    #region Offensive Cooldowns

    private readonly Spell Mindbender = new Spell("Mindbender");
    private readonly Spell PowerInfusion = new Spell("Power Infusion"); //No GCD
    private readonly Spell Shadowfiend = new Spell("Shadowfiend");
    private readonly Spell SurrendertoMadness = new Spell("Surrender to Madness"); //No GCD

    #endregion

    #region Defensive Spells

    private readonly Spell Dispersion = new Spell("Dispersion");
    private readonly Spell Fade = new Spell("Fade"); //No GCD
    private readonly Spell PowerWordShield = new Spell("Power Word: Shield");

    #endregion

    #region Healing Spell

    private readonly Spell ShadowMend = new Spell("Shadow Mend");
    private readonly Spell VampiricEmbrace = new Spell("Vampiric Embrace"); //No GCD

    #endregion

    #region Utility Spells

    //private readonly Spell DispelMagic = new Spell("Dispel Magic");
    //private readonly Spell MassDispel = new Spell("Mass Dispel");
    //private readonly Spell MindControl = new Spell("Mind Control");
    //private readonly Spell PsychicScream = new Spell("Psychic Scream");
    //private readonly Spell PurifyDisease = new Spell("Purify Disease");
    //private readonly Spell ShackleUndead = new Spell("Shackle Undead");

    #endregion

    public PriestShadow()
    {
        Main.InternalRange = 39f;
        Main.InternalAggroRange = 39f;
        Main.InternalLightHealingSpell = ShadowMend; // TESTING
        MySettings = PriestShadowSettings.GetSettings();
        Main.DumpCurrentSettings<PriestShadowSettings>(MySettings);
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
                                lastTarget = ObjectManager.Me.Target;

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

    // For Movement Spells (always return after Casting)
    private void Patrolling()
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
                if (MySettings.UseDarkflight && Darkflight.IsSpellUsable)
                {
                    Darkflight.Cast();
                    return;
                }
            }
        }
        else
        {
            //Self Heal for Damage Dealer
            if (nManager.Products.Products.ProductName == "Damage Dealer" && Main.InternalLightHealingSpell.IsSpellUsable &&
                ObjectManager.Me.HealthPercent < 90 && ObjectManager.Target.Guid == ObjectManager.Me.Guid)
            {
                Main.InternalLightHealingSpell.CastOnSelf();
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
            //Shadow Mend
            if (ObjectManager.Me.HealthPercent < MySettings.UseShadowMendBelowPercentage && ShadowMend.IsSpellUsable)
            {
                ShadowMend.Cast();
                return true;
            }
            //Heal Party/Raid
            //Dispersion
            if (VampiricEmbrace.IsSpellUsable)
            {
                int alivePlayers = 0;
                float PartyHpMedian = 0;
                int DamagedPlayers = 0;
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
                    PartyHpMedian += currentPlayer.HealthPercent;
                    alivePlayers++;
                    if (currentPlayer.HealthPercent < 100)
                        DamagedPlayers++;
                }
                PartyHpMedian /= alivePlayers;
                //Both Settings have to be true
                if (PartyHpMedian < MySettings.UseVampiricEmbraceBelowPartyPercentage &&
                    DamagedPlayers > MySettings.UseVampiricEmbraceAboveDamagedPlayers)
                {
                    VampiricEmbrace.Cast();
                }
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
                //Fade
                if (ObjectManager.Me.HealthPercent < MySettings.UseFadeBelowPercentage && Fade.IsSpellUsable)
                {
                    Fade.Cast();
                    DefensiveTimer = new Timer(1000*10);
                    return true;
                }
                //Power Word: Shield
                if (ObjectManager.Me.HealthPercent < MySettings.UsePowerWordShieldBelowPercentage &&
                    PowerWordShield.IsSpellUsable && !PowerWordShield.HaveBuff)
                {
                    PowerWordShield.Cast();
                    return true;
                }
            }
            //Mitigate Damage in Emergency Situations
            //Dispersion
            if (ObjectManager.Me.HealthPercent < MySettings.UseDispersionBelowPercentage && Dispersion.IsSpellUsable)
            {
                Dispersion.Cast();
                DefensiveTimer = new Timer(1000*6);
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
            //Cast Power Infusion when Voidform is active
            if (MySettings.UsePowerInfusion && PowerInfusion.IsSpellUsable && ObjectManager.Me.UnitAura(Voidform.Id, ObjectManager.Me.Guid).IsValid)
            {
                PowerInfusion.Cast();
            }
            //Cast Surrender to Madness
            if (MySettings.UseSurrendertoMadness && SurrendertoMadness.IsSpellUsable)
            {
                SurrendertoMadness.Cast();
                return true;
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

            //Against more than 4 Stacked Targets
            if (ObjectManager.Target.GetUnitInSpellRange(10f) > 4)
            {
                //1. Cast Shadow Word: Death when
                if (MySettings.UseShadowWordDeath && ShadowWordDeath.IsSpellUsable &&
                    ShadowWordDeath.GetSpellCharges > 0 && ShadowWordDeath.IsHostileDistanceGood &&
                    //you have the Reaper of Souls Talent or
                    ((ReaperofSouls.HaveBuff && ObjectManager.Target.HealthPercent < 35) ||
                     //TODO: the target will die.
                     ObjectManager.Target.HealthPercent < 20))
                {
                    ShadowWordDeath.Cast();
                    return;
                }
                //2. Cast Mind Sear
                if (MySettings.UseMindSear && MindSear.IsSpellUsable && MindSear.IsHostileDistanceGood)
                {
                    MindSear.Cast();
                    return;
                }
            }

            //Use Void Eruption
            if (MySettings.UseVoidEruption && VoidEruption.IsSpellUsable &&
                ((LegacyoftheVoid.HaveBuff && ObjectManager.Me.Insanity >= 70) ||
                 ObjectManager.Me.Insanity >= 100) && !ObjectManager.Me.UnitAura(Voidform.Id, ObjectManager.Me.Guid).IsValid)
            {
                VoidEruption.Cast();
                return;
            }

            //1. Use Void Torrent when
            if (MySettings.UseVoidTorrent && VoidTorrent.IsSpellUsable && VoidTorrent.IsHostileDistanceGood &&
                //Voidform is active
                ObjectManager.Me.UnitAura(Voidform.Id, ObjectManager.Me.Guid).IsValid)
            {
                VoidTorrent.Cast();
                return;
            }
            //2. Use Void Bolt when
            if (MySettings.UseVoidBolt && VoidBolt.IsSpellUsable &&
                CombatClass.InSpellRange(ObjectManager.Target, 0, 40) &&
                //Voidform is active
                ObjectManager.Me.UnitAura(Voidform.Id, ObjectManager.Me.Guid).IsValid)
            {
                VoidEruption.Cast();
                return;
            }
            //3. Cast Shadowfiend / Mindbender when you have low Voidform stacks.
            if (Mindbender.KnownSpell)
            {
                if (MySettings.UseMindbender && Mindbender.IsSpellUsable &&
                    Voidform.BuffStack <= 50 && ObjectManager.Me.UnitAura(Voidform.Id, ObjectManager.Me.Guid).IsValid)
                {
                    Mindbender.Cast();
                    return;
                }
            }
            else if (MySettings.UseShadowfiend && Shadowfiend.IsSpellUsable &&
                     Voidform.BuffStack <= 50 && ObjectManager.Me.UnitAura(Voidform.Id, ObjectManager.Me.Guid).IsValid)
            {
                Shadowfiend.Cast();
                return;
            }
            //4. Cast Shadow Word: Death when you are in Void form and
            if (MySettings.UseShadowWordDeath && ShadowWordDeath.IsSpellUsable &&
                ShadowWordDeath.IsHostileDistanceGood &&
                ((ReaperofSouls.HaveBuff && ObjectManager.Target.HealthPercent < 35) ||
                 ObjectManager.Target.HealthPercent < 20) && ObjectManager.Me.UnitAura(Voidform.Id, ObjectManager.Me.Guid).IsValid &&
                //you have 2 charges or
                (ShadowWordDeath.GetSpellCharges >= 2 ||
                 //you will not fall out of Voidform in 2 seconds and Mind Blast is off cooldown.
                 (ObjectManager.Me.Insanity > 9*2 && MindBlast.IsSpellUsable)))
            {
                ShadowWordDeath.Cast();
                return;
            }
            //5. Cast Mind Blast
            if (MySettings.UseMindBlast && MindBlast.IsSpellUsable && MindBlast.IsHostileDistanceGood &&
                MindBlast.GetSpellCharges > 0)
            {
                MindBlast.Cast();
                return;
            }
            //6. Cast Shadowfiend / Mindbender when
            if (Mindbender.KnownSpell)
            {
                if (MySettings.UseMindbender && Mindbender.IsSpellUsable &&
                    //you have high Voidform stacks.
                    Voidform.BuffStack > 50)
                {
                    Mindbender.Cast();
                    return;
                }
            }
            else if (MySettings.UseShadowfiend && Shadowfiend.IsSpellUsable &&
                     //you have high Voidform stacks.
                     Voidform.BuffStack > 50)
            {
                Shadowfiend.Cast();
                return;
            }
            //7. Cast Shadow Crash
            if (MySettings.UseShadowCrash && ShadowCrash.IsSpellUsable && ShadowCrash.IsHostileDistanceGood)
            {
                ShadowCrash.CastAtPosition(ObjectManager.Target.Position);
                return;
            }
            //8. Cast Shadow Word: Void when
            if (MySettings.UseShadowWordVoid && ShadowWordVoid.IsSpellUsable && ShadowWordVoid.IsHostileDistanceGood &&
                //you have 3 charges or Voidform is active.
                (ShadowWordVoid.GetSpellCharges == 3 || ObjectManager.Me.UnitAura(Voidform.Id, ObjectManager.Me.Guid).IsValid))
            {
                ShadowWordVoid.CastAtPosition(ObjectManager.Target.Position);
                return;
            }
            //9. Apply Shadow Word: Pain when
            if (MySettings.UseShadowWordPain && ShadowWordPain.IsSpellUsable && ShadowWordPain.IsHostileDistanceGood &&
                //the Dot isn't up.
                !ShadowWordPain.TargetHaveBuffFromMe)
            {
                ShadowWordPain.Cast();
                return;
            }
            //10. Apply Vampiric Touch when
            if (MySettings.UseVampiricTouch && VampiricTouch.IsSpellUsable && VampiricTouch.IsHostileDistanceGood &&
                //the Dot isn't up.
                !VampiricTouch.TargetHaveBuffFromMe)
            {
                VampiricTouch.Cast();
                return;
            }
            //11. Cast Mind Sear when
            if (MySettings.UseMindSear && MindSear.IsSpellUsable && MindSear.IsHostileDistanceGood &&
                //you have multiple targets
                ObjectManager.Target.GetUnitInSpellRange(10f) > 1)
            {
                MindSear.Cast();
                return;
            }
            //12. Cast Mind Flay / Mind Spike
            if (MySettings.UseMindFlay_Spike && MindFlay.IsSpellUsable && MindFlay.IsHostileDistanceGood)
            {
                MindFlay.Cast();
                return;
            }
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    #region Nested type: PriestShadowSettings

    [Serializable]
    public class PriestShadowSettings : Settings
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
        public bool UseVoidTorrent = true;

        /* Offensive Spells */
        public bool UseMindBlast = true;
        public bool UseMindFlay_Spike = true;
        public bool UseMindSear = true;
        public bool UseShadowCrash = true;
        public bool UseShadowWordDeath = true;
        public bool UseShadowWordPain = true;
        public bool UseShadowWordVoid = true;
        public bool UseVampiricTouch = true;
        public bool UseVoidBolt = true;
        public bool UseVoidEruption = true;

        /* Offensive Cooldowns */
        public bool UseMindbender = true;
        public bool UsePowerInfusion = true;
        public bool UseShadowfiend = true;
        public bool UseSurrendertoMadness = false;

        /* Defensive Spells */
        public int UseDispersionBelowPercentage = 10;
        public int UseFadeBelowPercentage = 100; //No GCD
        public int UsePowerWordShieldBelowPercentage = 80;

        /* Healing Spells */
        public int UseShadowMendBelowPercentage = 0;
        public int UseVampiricEmbraceAboveDamagedPlayers = 3;
        public int UseVampiricEmbraceBelowPartyPercentage = 40;

        /* Game Settings */
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;

        public PriestShadowSettings()
        {
            ConfigWinForm("Shadow Priest Settings");
            /* Professions & Racials */
            //AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Darkflight", "UseDarkflight", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaruBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            AddControlInWinForm("Use Stone Form", "UseStoneformBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            AddControlInWinForm("Use War Stomp", "UseWarStompBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            /* Artifact Spells */
            AddControlInWinForm("Use Void Torrent", "UseVoidTorrent", "Artifact Spells");
            /* Offensive Spells */
            AddControlInWinForm("Use Mind Blast", "UseMindBlast", "Offensive Spells");
            AddControlInWinForm("Use Mind Flay & Spike", "UseMindFlay_Spike", "Offensive Spells");
            AddControlInWinForm("Use Mind Sear", "UseMindSear", "Offensive Spells");
            AddControlInWinForm("Use Shadow Crash", "UseShadowCrash", "Offensive Spells");
            AddControlInWinForm("Use Shadow Word: Death", "UseShadowWordDeath", "Offensive Spells");
            AddControlInWinForm("Use Shadow Word: Pain", "UseShadowWordPain", "Offensive Spells");
            AddControlInWinForm("Use Shadow Word: Void", "UseShadowWordVoid", "Offensive Spells");
            AddControlInWinForm("Use Vampiric Touch", "UseVampiricTouch", "Offensive Spells");
            AddControlInWinForm("Use Void Bolt", "UseVoidBolt", "Offensive Spells");
            AddControlInWinForm("Use Void Eruption", "UseVoidEruption", "Offensive Spells");
            /* Offensive Cooldowns */
            AddControlInWinForm("Use Mindbender", "UseMindbender", "Offensive Cooldowns");
            AddControlInWinForm("Use Power Infusion", "UsePowerInfusion", "Offensive Cooldowns");
            AddControlInWinForm("Use Shadowfiend", "UseShadowfiend", "Offensive Cooldowns");
            AddControlInWinForm("Use Surrender to Madness", "UseSurrendertoMadness", "Offensive Cooldowns");
            /* Defensive Spells */
            AddControlInWinForm("Use Dispersion", "UseDispersionBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Fade", "UseFadeBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Power Word: Shield", "UsePowerWordShieldBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            /* Healing Spell */
            AddControlInWinForm("Use Shadow Mend", "UseShadowMendBelowPercentage", "Healing Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Vampiric Embrace", "UseVampiricEmbraceAboveDamagedPlayers", "Healing Spells", "AbovePercentage", "Damaged Players");
            AddControlInWinForm("Use Vampiric Embrace", "UseVampiricEmbraceBelowPartyPercentage", "Healing Spells", "BelowPercentage", "Party Life");
            /* Game Settings */
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");
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

#endregion

// ReSharper restore ObjectCreationAsStatement
// ReSharper restore EmptyGeneralCatchClause