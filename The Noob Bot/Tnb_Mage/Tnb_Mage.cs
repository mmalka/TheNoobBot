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
                    #region Mage Specialisation checking

                case WoWClass.Mage:

                    if (wowSpecialization == WoWSpecialization.MageArcane)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Mage_Arcane.xml";
                            var currentSetting = new MageArcane.MageArcaneSettings();
                            if (File.Exists(currentSettingsFile) && !resetSettings)
                            {
                                currentSetting = Settings.Load<MageArcane.MageArcaneSettings>(currentSettingsFile);
                            }
                            currentSetting.ToForm();
                            currentSetting.Save(currentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Mage Arcane Combat class...");
                            InternalRange = 30.0f;
                            EquipmentAndStats.SetPlayerSpe(WoWSpecialization.MageArcane);
                            new MageArcane();
                        }
                        break;
                    }
                    if (wowSpecialization == WoWSpecialization.MageFire)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Mage_Fire.xml";
                            var currentSetting = new MageFire.MageFireSettings();
                            if (File.Exists(currentSettingsFile) && !resetSettings)
                            {
                                currentSetting = Settings.Load<MageFire.MageFireSettings>(currentSettingsFile);
                            }
                            currentSetting.ToForm();
                            currentSetting.Save(currentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Mage Fire Combat class...");
                            InternalRange = 30.0f;
                            EquipmentAndStats.SetPlayerSpe(WoWSpecialization.MageFire);
                            new MageFire();
                        }
                        break;
                    }
                    if (wowSpecialization == WoWSpecialization.MageFrost || wowSpecialization == WoWSpecialization.None)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Mage_Frost.xml";
                            var currentSetting = new MageFrost.MageFrostSettings();
                            if (File.Exists(currentSettingsFile) && !resetSettings)
                            {
                                currentSetting = Settings.Load<MageFrost.MageFrostSettings>(currentSettingsFile);
                            }
                            currentSetting.ToForm();
                            currentSetting.Save(currentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Mage Frost Combat class...");
                            InternalRange = 30.0f;
                            EquipmentAndStats.SetPlayerSpe(WoWSpecialization.MageFrost);
                            new MageFrost();
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

#region Mage

public class MageArcane
{
    private static MageArcaneSettings MySettings = MageArcaneSettings.GetSettings();

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

    private readonly Spell ArcaneCharge = new Spell(114664);

    #endregion

    #region Artifact Spells

    private readonly Spell MarkofAluneth = new Spell("Mark of Aluneth");

    #endregion

    #region Offensive Spells

    private readonly Spell ArcaneBarrage = new Spell("Arcane Barrage");
    private readonly Spell ArcaneBlast = new Spell("Arcane Blast");
    private readonly Spell ArcaneExplosion = new Spell("Arcane Explosion");
    private readonly Spell ArcaneMissiles = new Spell("Arcane Missiles");
    private readonly Spell ArcaneOrb = new Spell("Arcane Orb");
    private readonly Spell ChargedUp = new Spell("Charged Up");
    private readonly Spell NetherTempest = new Spell("Nether Tempest");
    private readonly Spell RuneofPower = new Spell("Rune of Power");
    private readonly Spell Supernova = new Spell("Supernova");

    #endregion

    #region Offensive Cooldowns

    private readonly Spell ArcanePower = new Spell("Arcane Power"); //No GCD
    private readonly Spell Evocation = new Spell("Evocation");
    private readonly Spell MirrorImage = new Spell("Mirror Image");
    private readonly Spell PresenceofMind = new Spell("Presence of Mind"); //No GCD
    private readonly Spell TimeWarp = new Spell("Time Warp"); //No GCD

    #endregion

    #region Defensive Spells

    private readonly Spell FrostNova = new Spell("Frost Nova");
    private readonly Spell GreaterInvisibility = new Spell("Greater Invisibility");
    private readonly Spell IceBarrier = new Spell("Ice Barrier");
    private readonly Spell IceBlock = new Spell("Ice Block");
    private readonly Spell Slow = new Spell("Slow");

    #endregion

    #region Utility Spells

    //private readonly Spell Blink = new Spell("Blink");
    //private readonly Spell Counterspell = new Spell("Counterspell");
    //private readonly Spell Displacement = new Spell("Displacement");
    private readonly Spell IceFloes = new Spell("Ice Floes");
    //private readonly Spell Polymorph = new Spell("Polymorph");
    //private readonly Spell Spellsteal = new Spell("Spellsteal");
    //private readonly Spell SlowFall = new Spell("Slow Fall");
    //private readonly Spell ConjureRefreshment = new Spell("Conjure Refreshment");

    #endregion

    public MageArcane()
    {
        Main.InternalRange = 39;
        Main.InternalAggroRange = 39;
        Main.InternalLightHealingSpell = null;
        MySettings = MageArcaneSettings.GetSettings();
        Main.DumpCurrentSettings<MageArcaneSettings>(MySettings);
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

                            if (ArcaneBlast.IsHostileDistanceGood)
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

        if (ObjectManager.Me.GetMove)
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
                //Ice Barrier
                if (ObjectManager.Me.HealthPercent < MySettings.UseIceBarrierBelowPercentage && IceBarrier.IsSpellUsable)
                {
                    IceBarrier.Cast();
                    return true;
                }
                //Frost Nova
                if (ObjectManager.Me.HealthPercent < MySettings.UseFrostNovaBelowPercentage && FrostNova.IsSpellUsable)
                {
                    FrostNova.Cast();
                    return true;
                }
                //Greater Invisibility
                if (ObjectManager.Me.HealthPercent < MySettings.UseGreaterInvisibilityBelowPercentage && GreaterInvisibility.IsSpellUsable)
                {
                    GreaterInvisibility.Cast();
                    return true;
                }
                //Slow
                if (ObjectManager.Me.HealthPercent < MySettings.UseSlowBelowPercentage && Slow.IsSpellUsable)
                {
                    Slow.Cast();
                    return true;
                }
            }
            //Mitigate Damage in Emergency Situations
            //Ice Block
            if (ObjectManager.Me.HealthPercent < MySettings.UseIceBlockBelowPercentage && IceBlock.IsSpellUsable)
            {
                IceBlock.Cast();
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
            //Cast Time Warp whenever available.
            if (MySettings.UseTimeWarp && TimeWarp.IsSpellUsable && !ObjectManager.Me.HaveBuff(80354))
            {
                TimeWarp.Cast();
            }
            //Cast Charged Up when you have no Arcane Charges.
            if (MySettings.UseChargedUp && ChargedUp.IsSpellUsable && ArcaneCharge.BuffStack == 0)
            {
                ChargedUp.Cast();
            }
            //Cast Evocation when you have low Mana and
            if (ObjectManager.Me.Mana < MySettings.UseEvocationBelowManaPercentage && Evocation.IsSpellUsable &&
                //you are not in the Burst Phase
                !ArcanePower.HaveBuff)
            {
                Evocation.Cast();
                return true;
            }
            //Cast Mirror Image when you have 4 Arcane Charges.
            if (MySettings.UseMirrorImage && MirrorImage.IsSpellUsable && ArcaneCharge.BuffStack == 4)
            {
                MirrorImage.Cast();
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

            //Use Ice Floes if you have to move //TODO: and have no instant casts to burn.
            if (ObjectManager.Me.GetMove && !IceFloes.HaveBuff)
            {
                //Cast Ice Floes
                if (MySettings.UseIceFloes && IceFloes.IsSpellUsable)
                {
                    IceFloes.Cast();
                    return;
                }
            }

            //Burn Phase
            //1. Ensure you have 4 Arcane Charges.
            if (ArcaneCharge.BuffStack == 4)
            {
                //2. Place Rune of Power (if talented).
                if (MySettings.UseRuneofPower && RuneofPower.IsSpellUsable && !RuneofPower.HaveBuff)
                {
                    RuneofPower.CastAtPosition(ObjectManager.Me.Position);
                    return;
                }
                //3. Activate Arcane Power 
                if (MySettings.UseArcanePower && ArcanePower.IsSpellUsable)
                {
                    ArcanePower.Cast();
                }
            }
            if (ArcanePower.HaveBuff)
            {
                //4. Cast Arcane Missiles whenever available.
                if (MySettings.UseArcaneMissiles && ArcaneMissiles.IsSpellUsable && ArcaneMissiles.IsHostileDistanceGood)
                {
                    ArcaneMissiles.Cast();
                    return;
                }
                //5. Cast Arcane Blast.
                if (MySettings.UseArcaneBlast && ArcaneBlast.IsSpellUsable && ArcaneBlast.IsHostileDistanceGood)
                {
                    ArcaneBlast.Cast();
                    return;
                }
                //6. Activate Presence of Mind (if talented) for the end of Arcane Power.
                if (MySettings.UsePresenceofMind && PresenceofMind.IsSpellUsable && ObjectManager.Target.UnitAura(ArcanePower.Ids).AuraTimeLeftInMs < 2000)
                {
                    PresenceofMind.Cast();
                    return;
                }
            }


            //Conserve Phase
            //1. Use Rune of Power if it has 2 charges.
            if (MySettings.UseRuneofPower && RuneofPower.IsSpellUsable && RuneofPower.GetSpellCharges == 2)
            {
                RuneofPower.CastAtPosition(ObjectManager.Me.Position);
                return;
            }
            //2. Cast Mark of Aluneth on cooldown and make sure Rune of Power is up.
            if (MySettings.UseMarkofAluneth && MarkofAluneth.IsSpellUsable && MarkofAluneth.IsHostileDistanceGood)
            {
                if (MySettings.UseRuneofPower && RuneofPower.IsSpellUsable && !RuneofPower.HaveBuff)
                {
                    RuneofPower.CastAtPosition(ObjectManager.Me.Position);
                    return;
                }
                MarkofAluneth.Cast();
                return;
            }
            //3. Cast Arcane Orb (if talented) and you currently have no Arcane Charges.
            if (MySettings.UseArcaneOrb && ArcaneOrb.IsSpellUsable && ArcaneOrb.IsHostileDistanceGood &&
                ArcaneCharge.BuffStack == 0)
            {
                ArcaneOrb.Cast();
                return;
            }
            //4.-5. Apply/Refresh Nether Tempest (if talented)
            if (MySettings.UseNetherTempest && NetherTempest.IsSpellUsable && NetherTempest.IsHostileDistanceGood &&
                //with 4 stacks of Arcane Charge and it has less than 4 seconds remaining.
                ArcaneCharge.BuffStack == 4 && ObjectManager.Target.UnitAura(NetherTempest.Ids).AuraTimeLeftInMs < 4000)
            {
                NetherTempest.Cast();
                return;
            }
            //6. Cast Arcane Missiles if
            if (MySettings.UseArcaneMissiles && ArcaneMissiles.IsSpellUsable && ArcaneMissiles.IsHostileDistanceGood &&
                //you have 3 charges of Arcane Missiles or
                (ArcaneMissiles.GetSpellCharges == 3 ||
                 //you are not at 100% Mana and you have 3 or more Arcane Charges.
                 (ObjectManager.Me.Mana < 100 && ArcaneCharge.BuffStack >= 3)))
            {
                ArcaneMissiles.Cast();
                return;
            }
            //7. Cast Supernova (if talented).
            if (MySettings.UseSupernova && Supernova.IsSpellUsable && Supernova.IsHostileDistanceGood)
            {
                Supernova.Cast();
                return;
            }
            //8. Cast Arcane Barrage if you need to regenerate Mana.
            if (ObjectManager.Me.Mana < MySettings.UseArcaneBarrageBelowManaPercentage &&
                ArcaneBarrage.IsSpellUsable && ArcaneBarrage.IsHostileDistanceGood)
            {
                ArcaneBarrage.Cast();
                return;
            }
            //9. Cast Arcane Explosion if you will hit 3 or more targets.
            if (MySettings.UseArcaneExplosion && ArcaneExplosion.IsSpellUsable &&
                ObjectManager.Me.GetUnitInSpellRange(10) >= 3)
            {
                ArcaneExplosion.Cast();
                return;
            }
            //10. Cast Arcane Blast.
            if (MySettings.UseArcaneBlast && ArcaneBlast.IsSpellUsable && ArcaneBlast.IsHostileDistanceGood)
            {
                ArcaneBlast.Cast();
                return;
            }
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    #region Nested type: MageArcaneSettings

    [Serializable]
    public class MageArcaneSettings : Settings
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
        public bool UseMarkofAluneth = true;

        /* Offensive Spells */
        public int UseArcaneBarrageBelowManaPercentage = 3;
        public bool UseArcaneBlast = true;
        public bool UseArcaneExplosion = true;
        public bool UseArcaneMissiles = true;
        public bool UseArcaneOrb = true;
        public bool UseChargedUp = true;
        public bool UseNetherTempest = true;
        public bool UseRuneofPower = true;
        public bool UseSupernova = true;

        /* Offensive Cooldowns */
        public bool UseArcanePower = true;
        public int UseEvocationBelowManaPercentage = 2;
        public bool UseMirrorImage = true;
        public bool UsePresenceofMind = true;
        public bool UseTimeWarp = true;

        /* Defensive Spells */
        public int UseFrostNovaBelowPercentage = 0;
        public int UseGreaterInvisibilityBelowPercentage = 0;
        public int UseIceBarrierBelowPercentage = 90;
        public int UseIceBlockBelowPercentage = 10;
        public int UseSlowBelowPercentage = 0;

        /* Utility Spells */
        public bool UseIceFloes = true;

        /* Game Settings */
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;

        public MageArcaneSettings()
        {
            ConfigWinForm("Mage Arcane Settings");
            /* Professions & Racials */
            //AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Darkflight", "UseDarkflight", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaruBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            AddControlInWinForm("Use Stone Form", "UseStoneformBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            AddControlInWinForm("Use War Stomp", "UseWarStompBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            /* Artifact Spells */
            AddControlInWinForm("Use Mark of Aluneth", "UseMarkofAluneth", "Artifact Spells");
            /* Offensive Spells */
            AddControlInWinForm("Use Arcane Barrage", "UseArcaneBarrageBelowManaPercentage", "Offensive Spells", "BelowPercentage", "Mana");
            AddControlInWinForm("Use Arcane Blast", "UseArcaneBlast", "Offensive Spells");
            AddControlInWinForm("Use Arcane Explosion", "UseArcaneExplosion", "Offensive Spells");
            AddControlInWinForm("Use Arcane Missiles", "UseArcaneMissiles", "Offensive Spells");
            AddControlInWinForm("Use Arcane Orb", "UseArcaneOrb", "Offensive Spells");
            AddControlInWinForm("Use Charged Up", "UseChargedUp", "Offensive Spells");
            AddControlInWinForm("Use Nether Tempest", "UseNetherTempest", "Offensive Spells");
            AddControlInWinForm("Use Rune of Power", "UseRuneofPower", "Offensive Spells");
            AddControlInWinForm("Use Supernova", "UseSupernova", "Offensive Spells");
            /* Offensive Cooldowns */
            AddControlInWinForm("Use Arcane Power", "UseArcanePower", "Offensive Cooldowns");
            AddControlInWinForm("Use Evocation", "UseEvocationBelowManaPercentage", "Offensive Cooldowns", "BelowPercentage", "Mana");
            AddControlInWinForm("Use Mirror Image", "UseMirrorImage", "Offensive Cooldowns");
            AddControlInWinForm("Use Presence of Mind", "UsePresenceofMind", "Offensive Cooldowns");
            AddControlInWinForm("Use Time Warp", "UseTimeWarp", "Offensive Cooldowns");
            /* Defensive Spells */
            AddControlInWinForm("Use Frost Nova", "UseFrostNovaBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Greater Invisibility", "UseGreaterInvisibilityBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Ice Barrier", "UseIceBarrierBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Ice Block", "UseIceBlockBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Slow", "UseSlowBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            /* Utility Spells */
            AddControlInWinForm("Use Ice Floes", "UseIceFloes", "Utility Spells");
            /* Game Settings */
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");
        }

        public static MageArcaneSettings CurrentSetting { get; set; }

        public static MageArcaneSettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Mage_Arcane.xml";
            if (File.Exists(currentSettingsFile))
            {
                return
                    CurrentSetting = Load<MageArcaneSettings>(currentSettingsFile);
            }
            return new MageArcaneSettings();
        }
    }

    #endregion
}

public class MageFire
{
    private static MageFireSettings MySettings = MageFireSettings.GetSettings();

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

    private readonly Spell HeatingUp = new Spell(48107);
    private readonly Spell HotStreak = new Spell(195283);

    #endregion

    #region Dots

    private readonly Spell Ignite = new Spell(12654);

    #endregion

    #region Artifact Spells

    private readonly Spell PhoenixsFlames = new Spell("Phoenix's Flames");

    #endregion

    #region Offensive Spells

    private readonly Spell BlastWave = new Spell("Blast Wave");
    private readonly Spell Cinderstorm = new Spell("Cinderstorm");
    private readonly Spell DragonsBreath = new Spell("Dragon's Breath");
    private readonly Spell Fireball = new Spell("Fireball");
    private readonly Spell FireBlast = new Spell("Fire Blast");
    private readonly Spell FlameOn = new Spell("Flame On");
    private readonly Spell Flamestrike = new Spell("Flamestrike");
    private readonly Spell LivingBomb = new Spell("Living Bomb");
    private readonly Spell Meteor = new Spell("Meteor");
    private readonly Spell Pyroblast = new Spell("Pyroblast");
    private readonly Spell RuneofPower = new Spell("Rune of Power");
    private readonly Spell Scorch = new Spell("Scorch");

    #endregion

    #region Offensive Cooldowns

    private readonly Spell Combustion = new Spell("Combustion"); //No GCD
    private readonly Spell MirrorImage = new Spell("Mirror Image");
    private readonly Spell TimeWarp = new Spell("Time Warp"); //No GCD

    #endregion

    #region Defensive Spells

    private readonly Spell FrostNova = new Spell("Frost Nova");
    private readonly Spell IceBarrier = new Spell("Ice Barrier");
    private readonly Spell IceBlock = new Spell("Ice Block");
    private readonly Spell Invisibility = new Spell("Invisibility");

    #endregion

    #region Utility Spells

    //private readonly Spell Blink = new Spell("Blink");
    //private readonly Spell Counterspell = new Spell("Counterspell");
    private readonly Spell IceFloes = new Spell("Ice Floes");
    //private readonly Spell Shimmer = new Spell("Shimmer");
    //private readonly Spell Spellsteal = new Spell("Spellsteal");
    //private readonly Spell SlowFall = new Spell("Slow Fall");
    //private readonly Spell ConjureRefreshment = new Spell("Conjure Refreshment");

    #endregion

    public MageFire()
    {
        Main.InternalRange = 39f;
        Main.InternalAggroRange = 39f;
        Main.InternalLightHealingSpell = null;
        MySettings = MageFireSettings.GetSettings();
        Main.DumpCurrentSettings<MageFireSettings>(MySettings);
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

        if (ObjectManager.Me.GetMove)
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
                //Ice Barrier
                if (ObjectManager.Me.HealthPercent < MySettings.UseIceBarrierBelowPercentage && IceBarrier.IsSpellUsable)
                {
                    IceBarrier.Cast();
                    return true;
                }
                //Frost Nova
                if (ObjectManager.Me.HealthPercent < MySettings.UseFrostNovaBelowPercentage && FrostNova.IsSpellUsable)
                {
                    FrostNova.Cast();
                    return true;
                }
                //Greater Invisibility
                if (ObjectManager.Me.HealthPercent < MySettings.UseInvisibilityBelowPercentage && Invisibility.IsSpellUsable)
                {
                    Invisibility.Cast();
                    return true;
                }
            }
            //Mitigate Damage in Emergency Situations
            //Ice Block
            if (ObjectManager.Me.HealthPercent < MySettings.UseIceBlockBelowPercentage && IceBlock.IsSpellUsable)
            {
                IceBlock.Cast();
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
            //Cast Time Warp whenever available.
            if (MySettings.UseTimeWarp && TimeWarp.IsSpellUsable && !ObjectManager.Me.HaveBuff(80354))
            {
                TimeWarp.Cast();
            }
            //Cast Mirror Image when you have at least 2 charges of Phoenix's Flames available
            if (MySettings.UseMirrorImage && MirrorImage.IsSpellUsable && PhoenixsFlames.GetSpellCharges >= 2)
            {
                MirrorImage.Cast();
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

            //13. Use Ice Floes or Cast Scorch if you have to move //TODO: and have no instant casts to burn.
            if (ObjectManager.Me.GetMove && !IceFloes.HaveBuff)
            {
                //Cast Ice Floes
                if (MySettings.UseIceFloes && IceFloes.IsSpellUsable)
                {
                    IceFloes.Cast();
                    return;
                }
                //Cast Scorch if have no charges of Ice Floes
                if (MySettings.UseIceFloes && Scorch.IsSpellUsable && Scorch.IsHostileDistanceGood)
                {
                    Scorch.Cast();
                    return;
                }
            }

            //1. Use Rune of Power when Combustion is off cooldown.
            if (MySettings.UseRuneofPower && RuneofPower.IsSpellUsable && Combustion.IsSpellUsable)
            {
                RuneofPower.CastAtPosition(ObjectManager.Me.Position);
                return;
            }
            //2. Use Combustion when it is off cooldown.  
            if (MySettings.UseCombustion && Combustion.IsSpellUsable &&
                //Ensure you have at least 2 charges of Phoenix's Flames available and
                PhoenixsFlames.GetSpellCharges >= 2 &&
                //Flame On is off-cooldown, if possible.
                FlameOn.IsSpellUsable)
            {
                Combustion.Cast();
                return;
            }
            //3. Use Rune of Power when it has 2 charges.
            if (MySettings.UseRuneofPower && RuneofPower.IsSpellUsable && RuneofPower.GetSpellCharges == 2)
            {
                RuneofPower.CastAtPosition(ObjectManager.Me.Position);
                return;
            }
            //4. Cast Phoenix's Flames when
            if (MySettings.UsePhoenixsFlames && PhoenixsFlames.IsSpellUsable && Combustion.IsHostileDistanceGood &&
                //it has 2 charges.
                PhoenixsFlames.GetSpellCharges >= 2)
            {
                PhoenixsFlames.Cast();
                return;
            }
            //5. Cast Phoenix's Flames when
            if (MySettings.UsePhoenixsFlames && PhoenixsFlames.IsSpellUsable && PhoenixsFlames.IsHostileDistanceGood &&
                //there are 3 or more targets stacked
                ObjectManager.Target.GetUnitInSpellRange(5f) >= 3)
            {
                PhoenixsFlames.Cast();
                return;
            }
            //6. Cast Flamestrike when
            if (MySettings.UseFlamestrike && Flamestrike.IsSpellUsable && Flamestrike.IsHostileDistanceGood &&
                //there are 3 or more targets stacked and
                ObjectManager.Target.GetUnitInSpellRange(8f) >= 3 &&
                //you are on a Hot Streak.
                HotStreak.HaveBuff)
            {
                Flamestrike.CastAtPosition(ObjectManager.Me.Position);
                return;
            }
            //7. Cast Pyroblast when
            if (MySettings.UsePyroblast && Pyroblast.IsSpellUsable && Pyroblast.IsHostileDistanceGood &&
                //you are on a Hot Streak.
                HotStreak.HaveBuff)
            {
                Pyroblast.Cast();
                return;
            }
            //8. Cast Meteor if it is talented and off cooldown.
            if (MySettings.UseMeteor && Meteor.IsSpellUsable && Meteor.IsHostileDistanceGood)
            {
                Meteor.CastAtPosition(ObjectManager.Me.Position);
                return;
            }
            //9. Living Bomb if talented and
            if (MySettings.UseLivingBomb && LivingBomb.IsSpellUsable && LivingBomb.IsHostileDistanceGood &&
                //there are 3 or more targets stacked and
                ObjectManager.Target.GetUnitInSpellRange(10f) >= 3 &&
                //TODO: they will all live for at least 12 seconds.
                true)
            {
                LivingBomb.Cast();
                return;
            }
            //10. Cast Flame On if talented and it is off cooldown and 
            if (MySettings.UseFlameOn && FlameOn.IsSpellUsable &&
                //you currently have no charges of Fire Blast.
                FireBlast.GetSpellCharges == 0)
            {
                FlameOn.Cast();
                return;
            }
            //11. Cast Dragon's Breath if it is off cooldown.
            if (MySettings.UseDragonsBreath && DragonsBreath.IsSpellUsable && DragonsBreath.IsHostileDistanceGood)
            {
                DragonsBreath.Cast();
                return;
            }
            //12. Cast Fire Blast when
            if (MySettings.UseFireBlast && FireBlast.IsSpellUsable && FireBlast.IsHostileDistanceGood &&
                //you are Heating Up.
                HeatingUp.HaveBuff)
            {
                FireBlast.Cast();
                return;
            }
            //12. Cast Fireball to generate Heating Up.
            if (MySettings.UseFireball && Fireball.IsSpellUsable && Fireball.IsHostileDistanceGood)
            {
                Fireball.Cast();
                return;
            }
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    #region Nested type: MageFireSettings

    [Serializable]
    public class MageFireSettings : Settings
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
        public bool UsePhoenixsFlames = true;

        /* Offensive Spells */
        public bool UseDragonsBreath = true;
        public bool UseFireball = true;
        public bool UseFireBlast = true;
        public bool UseFlameOn = true;
        public bool UseFlamestrike = true;
        public bool UseLivingBomb = true;
        public bool UseMeteor = true;
        public bool UsePyroblast = true;
        public bool UseRuneofPower = true;

        /* Offensive Cooldowns */
        public bool UseCombustion = true;
        public bool UseMirrorImage = true;
        public bool UseTimeWarp = true;

        /* Defensive Spells */
        public int UseFrostNovaBelowPercentage = 0;
        public int UseInvisibilityBelowPercentage = 0;
        public int UseIceBarrierBelowPercentage = 90;
        public int UseIceBlockBelowPercentage = 10;

        /* Utility Spells */
        public bool UseIceFloes = true;

        /* Game Settings */
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;

        public MageFireSettings()
        {
            ConfigWinForm("Mage Frost Settings");
            /* Professions & Racials */
            //AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Darkflight", "UseDarkflight", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaruBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            AddControlInWinForm("Use Stone Form", "UseStoneformBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            AddControlInWinForm("Use War Stomp", "UseWarStompBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            /* Artifact Spells */
            AddControlInWinForm("Use Phoenix's Flames", "UsePhoenixsFlames", "Artifact Spells");
            /* Offensive Spells */
            AddControlInWinForm("Use Dragon's Breath", "UseDragonsBreath", "Offensive Spells");
            AddControlInWinForm("Use Fireball", "UseFireball", "Offensive Spells");
            AddControlInWinForm("Use Fire Blast", "UseFireBlast", "Offensive Spells");
            AddControlInWinForm("Use Flame On", "UseFlameOn", "Offensive Spells");
            AddControlInWinForm("Use Flamestrike", "UseFlamestrike", "Offensive Spells");
            AddControlInWinForm("Use Living Bomb", "UseLivingBomb", "Offensive Spells");
            AddControlInWinForm("Use Meteor", "UseMeteor", "Offensive Spells");
            AddControlInWinForm("Use Pyroblast", "UsePyroblast", "Offensive Spells");
            AddControlInWinForm("Use Rune of Power", "UseRuneofPower", "Offensive Spells");
            /* Offensive Cooldowns */
            AddControlInWinForm("Use Combustion", "UseCombustion", "Offensive Cooldowns");
            AddControlInWinForm("Use Mirror Image", "UseMirrorImage", "Offensive Cooldowns");
            AddControlInWinForm("Use Time Warp", "UseTimeWarp", "Offensive Cooldowns");
            /* Defensive Spells */
            AddControlInWinForm("Use Frost Nova", "UseFrostNovaBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Invisibility", "UseInvisibilityBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Ice Barrier", "UseIceBarrierBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Ice Block", "UseIceBlockBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            /* Utility Spells */
            AddControlInWinForm("Use Ice Floes", "UseIceFloes", "Utility Spells");
            /* Game Settings */
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");
        }

        public static MageFireSettings CurrentSetting { get; set; }

        public static MageFireSettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Mage_Frost.xml";
            if (File.Exists(currentSettingsFile))
            {
                return
                    CurrentSetting = Load<MageFireSettings>(currentSettingsFile);
            }
            return new MageFireSettings();
        }
    }

    #endregion
}

public class MageFrost
{
    private static MageFrostSettings MySettings = MageFrostSettings.GetSettings();

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

    #region Talent

    private readonly Spell ArcticGale = new Spell("Arctic Gale");

    #endregion

    #region Buffs

    private readonly Spell BrainFreeze = new Spell(190447);
    private readonly Spell FingersofFrost = new Spell(112965);

    #endregion

    #region Dots

    private readonly Spell WintersChill = new Spell("Winter's Chill");

    #endregion

    #region Artifact Spells

    private readonly Spell Ebonbolt = new Spell("Ebonbolt");

    #endregion

    #region Offensive Spells

    private readonly Spell Blizzard = new Spell("Blizzard");
    private readonly Spell Flurry = new Spell("Flurry"); //No GCD
    private readonly Spell Frostbolt = new Spell("Frostbolt");
    private readonly Spell FrostBomb = new Spell("Frost Bomb");
    private readonly Spell FrozenTouch = new Spell("Frozen Touch");
    private readonly Spell IceLance = new Spell("Ice Lance");
    private readonly Spell IceNova = new Spell("Ice Nova");
    private readonly Spell GlacialSpike = new Spell("Glacial Spike");
    private readonly Spell RuneofPower = new Spell("Rune of Power");
    private readonly Spell WaterJet = new Spell("Water Jet");

    #endregion

    #region Offensive Cooldowns

    private readonly Spell FrozenOrb = new Spell("Frozen Orb");
    private readonly Spell IcyVeins = new Spell("Icy Veins"); //No GCD
    private readonly Spell MirrorImage = new Spell("Mirror Image");
    private readonly Spell RayofFrost = new Spell("Ray of Frost");
    private readonly Spell TimeWarp = new Spell("Time Warp"); //No GCD

    #endregion

    #region Defensive Spells

    private readonly Spell FrostNova = new Spell("Frost Nova");
    private readonly Spell IceBarrier = new Spell("Ice Barrier");
    private readonly Spell IceBlock = new Spell("Ice Block");
    private readonly Spell Invisibility = new Spell("Invisibility");

    #endregion

    #region Utility Spells

    //private readonly Spell Blink = new Spell("Blink");
    //private readonly Spell Counterspell = new Spell("Counterspell");
    private readonly Spell Freeze = new Spell("Freeze"); //No GCD
    private readonly Spell IceFloes = new Spell("Ice Floes");
    //private readonly Spell Polymorph = new Spell("Polymorph");
    //private readonly Spell Spellsteal = new Spell("Spellsteal");
    private readonly Spell SummonWaterElemental = new Spell("Summon Water Elemental");
    //private readonly Spell SlowFall = new Spell("Slow Fall");
    //private readonly Spell ConjureRefreshment = new Spell("Conjure Refreshment");

    #endregion

    public MageFrost()
    {
        Main.InternalRange = 39f;
        Main.InternalAggroRange = 39f;
        Main.InternalLightHealingSpell = null;
        MySettings = MageFrostSettings.GetSettings();
        Main.DumpCurrentSettings<MageFrostSettings>(MySettings);
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

        if (ObjectManager.Me.GetMove)
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
                //Ice Barrier
                if (ObjectManager.Me.HealthPercent < MySettings.UseIceBarrierBelowPercentage && IceBarrier.IsSpellUsable)
                {
                    IceBarrier.Cast();
                    return true;
                }
                //Frost Nova
                if (ObjectManager.Me.HealthPercent < MySettings.UseFrostNovaBelowPercentage && FrostNova.IsSpellUsable)
                {
                    FrostNova.Cast();
                    return true;
                }
                //Greater Invisibility
                if (ObjectManager.Me.HealthPercent < MySettings.UseInvisibilityBelowPercentage && Invisibility.IsSpellUsable)
                {
                    Invisibility.Cast();
                    return true;
                }
            }
            //Mitigate Damage in Emergency Situations
            //Ice Block
            if (ObjectManager.Me.HealthPercent < MySettings.UseIceBlockBelowPercentage && IceBlock.IsSpellUsable)
            {
                IceBlock.Cast();
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
            //Cast Summon Water Elemental if it's not alive
            if (MySettings.UseSummonWaterElemental && SummonWaterElemental.IsSpellUsable &&
                ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0 || !ObjectManager.Pet.IsValid)
            {
                Logging.WriteDebug("Pet: Health == " + ObjectManager.Pet.Health + ", Guid == " + ObjectManager.Pet.Guid + ", IsValid == " + ObjectManager.Pet.IsValid);
                SummonWaterElemental.Cast();
                return true;
            }
            //Cast Time Warp whenever available.
            if (MySettings.UseTimeWarp && TimeWarp.IsSpellUsable && !ObjectManager.Me.HaveBuff(80354))
            {
                TimeWarp.Cast();
            }
            //Cast Mirror Image when you have 2 charges of Fingers of Frost.
            if (MySettings.UseMirrorImage && MirrorImage.IsSpellUsable && FingersofFrost.GetSpellCharges == 2)
            {
                MirrorImage.Cast();
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

            //Use Ice Floes if you have to move //TODO: and have no instant casts to burn.
            if (ObjectManager.Me.GetMove && !IceFloes.HaveBuff)
            {
                //Cast Ice Floes
                if (MySettings.UseIceFloes && IceFloes.IsSpellUsable)
                {
                    IceFloes.Cast();
                    return;
                }
            }

            //11b. Cast Ice Lance if
            if (MySettings.UseIceLance && IceLance.IsSpellUsable && IceLance.IsHostileDistanceGood &&
                //the Winter's Chill Dot is on the Target
                WintersChill.TargetHaveBuff)
            {
                IceLance.Cast();
                return;
            }
            //12b. Cast Frostbolt if
            if (MySettings.UseFrostbolt && Frostbolt.IsSpellUsable && Frostbolt.IsHostileDistanceGood &&
                //the Water Jet Dot is on the Target and you have less than 2 charges of Fingers of Frost.
                WaterJet.TargetHaveBuff && FingersofFrost.GetSpellCharges < 2)
            {
                Frostbolt.Cast();
                return;
            }

            //1. Cast Rune of Power if talented and it is at 2 charges.
            if (MySettings.UseRuneofPower && RuneofPower.IsSpellUsable && RuneofPower.GetSpellCharges == 2)
            {
                RuneofPower.CastAtPosition(ObjectManager.Me.Position);
                return;
            }
            //2. Cast Icy Veins if it is off cooldown.
            if (MySettings.UseIcyVeins && IcyVeins.IsSpellUsable)
            {
                IcyVeins.Cast();
                return;
            }
            //3. Cast Ray of Frost if
            if (MySettings.UseRayofFrost && RayofFrost.IsSpellUsable && RayofFrost.IsHostileDistanceGood &&
                //your Rune of Power is down.
                RuneofPower.HaveBuff)
            {
                RayofFrost.Cast();
                return;
            }
            //4. Cast Rune of Power if talented and
            if (MySettings.UseRuneofPower && RuneofPower.IsSpellUsable && !RuneofPower.HaveBuff &&
                //you will deal high burst damage.
                (FingersofFrost.GetSpellCharges >= 2 || (FingersofFrost.GetSpellCharges >= 1 &&
                                                         Ebonbolt.IsSpellUsable) || FrozenOrb.IsSpellUsable))
            {
                RuneofPower.CastAtPosition(ObjectManager.Me.Position);
                return;
            }
            //5. Cast Frost Bomb if talented and
            if (MySettings.UseFrostBomb && FrostBomb.IsSpellUsable && FrostBomb.IsHostileDistanceGood &&
                //you will trigger it with 2 charges of Fingers of Frost.
                FingersofFrost.GetSpellCharges >= 2)
            {
                FrostBomb.Cast();
                return;
            }
            //6. Cast Ice Lance if
            if (MySettings.UseIceLance && IceLance.IsSpellUsable && IceLance.IsHostileDistanceGood &&
                //you have 2 charges of Fingers of Frost.
                FingersofFrost.GetSpellCharges >= 2)
            {
                IceLance.Cast();
                return;
            }
            //7. Cast Frozen Orb if it is off cooldown.
            if (MySettings.UseFrozenOrb && FrozenOrb.IsSpellUsable && FrozenOrb.IsHostileDistanceGood)
            {
                FrozenOrb.Cast();
                return;
            }
            //8. Cast Freeze from your Water Elemental if //TODO: check if we have to workaround to cast pet spells
            if (MySettings.UseFreeze && Freeze.IsSpellUsable &&
                // it will hit at least 2 adds 
                ObjectManager.Pet.GetUnitInSpellRange(8f) >= 2)
            {
                Freeze.Cast();
                return;
            }
            //9. Cast Frozen Touch if talented and
            if (MySettings.UseFrozenTouch && FrozenTouch.IsSpellUsable &&
                //you currently have no charges of Fingers of Frost.
                FingersofFrost.GetSpellCharges == 0)
            {
                FrozenTouch.Cast();
                return;
            }
            //10. Cast Ebonbolt if it is off cooldown and
            if (MySettings.UseEbonbolt && Ebonbolt.IsSpellUsable && Ebonbolt.IsHostileDistanceGood &&
                //you have 1 or less charges of Fingers of Frost.
                FingersofFrost.GetSpellCharges <= 2)
            {
                Ebonbolt.Cast();
                return;
            }
            //11. Cast Flurry if
            if (MySettings.UseFlurry && Flurry.IsSpellUsable && Flurry.IsHostileDistanceGood &&
                //Brain Freeze is active and you currently have no charges of Fingers of Frost.
                BrainFreeze.HaveBuff && FingersofFrost.GetSpellCharges == 0)
            {
                Flurry.Cast();
                return;
            }
            //12. Cast Water Jet from your Water Elemental if //TODO: check if we have to workaround to cast pet spells
            if (MySettings.UseFreeze && Freeze.IsSpellUsable &&
                //you have no charges of Fingers of Frost.
                FingersofFrost.GetSpellCharges == 0)
            {
                WaterJet.Cast();
                return;
            }
            //13. Cast Ice Nova if talented.
            if (MySettings.UseIceNova && IceNova.IsSpellUsable && IceNova.IsHostileDistanceGood)
            {
                IceNova.Cast();
            }
            //14. Cast Blizzard if
            if (MySettings.UseBlizzard && Blizzard.IsSpellUsable && Blizzard.IsHostileDistanceGood &&
                //more than 4 targets are present and within the AoE or
                ObjectManager.Target.GetUnitInSpellRange(8f) > 4 ||
                //you are talented into Arctic Gale.
                ArcticGale.HaveBuff)
            {
                Blizzard.Cast();
            }
            //15. Cast Ice Lance if
            if (MySettings.UseIceLance && IceLance.IsSpellUsable && IceLance.IsHostileDistanceGood &&
                //you have 1 charge of Fingers of Frost.
                FingersofFrost.GetSpellCharges == 1)
            {
                IceLance.Cast();
                return;
            }
            //16. Cast Glacial Spike if talented and available.
            if (MySettings.UseGlacialSpike && GlacialSpike.IsSpellUsable && GlacialSpike.IsHostileDistanceGood)
            {
                GlacialSpike.Cast();
            }
            //16. Cast Frostbolt as a filler spell.
            if (MySettings.UseFrostbolt && Frostbolt.IsSpellUsable && Frostbolt.IsHostileDistanceGood)
            {
                Frostbolt.Cast();
            }
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    #region Nested type: MageFrostSettings

    [Serializable]
    public class MageFrostSettings : Settings
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
        public bool UseEbonbolt = true;

        /* Offensive Spells */
        public bool UseBlizzard = true;
        public bool UseFlurry = true;
        public bool UseFreeze = true;
        public bool UseFrostbolt = true;
        public bool UseFrostBomb = true;
        public bool UseFrozenTouch = true;
        public bool UseGlacialSpike = true;
        public bool UseIceLance = true;
        public bool UseIceNova = true;
        public bool UseRuneofPower = true;

        /* Offensive Cooldowns */
        public bool UseFrozenOrb = true;
        public bool UseIcyVeins = true;
        public bool UseMirrorImage = true;
        public bool UseRayofFrost = true;
        public bool UseTimeWarp = true;

        /* Defensive Spells */
        public int UseFrostNovaBelowPercentage = 0;
        public int UseInvisibilityBelowPercentage = 0;
        public int UseIceBarrierBelowPercentage = 90;
        public int UseIceBlockBelowPercentage = 10;

        /* Utility Spells */
        public bool UseIceFloes = true;
        public bool UseSummonWaterElemental = true;

        /* Game Settings */
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;

        public MageFrostSettings()
        {
            ConfigWinForm("Mage Fire Settings");
            /* Professions & Racials */
            //AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Darkflight", "UseDarkflight", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaruBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            AddControlInWinForm("Use Stone Form", "UseStoneformBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            AddControlInWinForm("Use War Stomp", "UseWarStompBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            /* Artifact Spells */
            AddControlInWinForm("Use Ebonbolt", "UseEbonbolt", "Artifact Spells");
            /* Offensive Spells */
            AddControlInWinForm("Use Blizzard", "UseBlizzard", "Offensive Spells");
            AddControlInWinForm("Use Flurry", "UseFlurry", "Offensive Spells");
            AddControlInWinForm("Use Freeze", "UseFreeze", "Offensive Spells");
            AddControlInWinForm("Use Frostbolt", "UseFrostbolt", "Offensive Spells");
            AddControlInWinForm("Use Frost Bomb", "UseFrostBomb", "Offensive Spells");
            AddControlInWinForm("Use Frozen Touch", "UseFrozenTouch", "Offensive Spells");
            AddControlInWinForm("Use Glacial Spike", "UseGlacialSpike", "Offensive Spells");
            AddControlInWinForm("Use Ice Lance", "UseIceLance", "Offensive Spells");
            AddControlInWinForm("Use Ice Nova", "UseIceNova", "Offensive Spells");
            AddControlInWinForm("Use Rune of Power", "UseRuneofPower", "Offensive Spells");
            /* Offensive Cooldowns */
            AddControlInWinForm("Use Frozen Orb", "UseFrozenOrb", "Offensive Cooldowns");
            AddControlInWinForm("Use Icy Veins", "UseIcyVeins", "Offensive Cooldowns");
            AddControlInWinForm("Use Mirror Image", "UseMirrorImage", "Offensive Cooldowns");
            AddControlInWinForm("Use Ray of Frost", "UseRayofFrost", "Offensive Cooldowns");
            AddControlInWinForm("Use Time Warp", "UseTimeWarp", "Offensive Cooldowns");
            /* Defensive Spells */
            AddControlInWinForm("Use Frost Nova", "UseFrostNovaBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Invisibility", "UseInvisibilityBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Ice Barrier", "UseIceBarrierBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Ice Block", "UseIceBlockBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            /* Utility Spells */
            AddControlInWinForm("Use Ice Floes", "UseIceFloes", "Utility Spells");
            AddControlInWinForm("Use Summon Water Elemental", "UseSummonWaterElemental", "Utility Spells");
            /* Game Settings */
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");
        }

        public static MageFrostSettings CurrentSetting { get; set; }

        public static MageFrostSettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Mage_Fire.xml";
            if (File.Exists(currentSettingsFile))
            {
                return
                    CurrentSetting = Load<MageFrostSettings>(currentSettingsFile);
            }
            return new MageFrostSettings();
        }
    }

    #endregion
}

#endregion

// ReSharper restore ObjectCreationAsStatement
// ReSharper restore EmptyGeneralCatchClause