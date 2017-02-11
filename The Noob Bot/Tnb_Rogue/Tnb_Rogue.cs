/*
* CombatClass for TheNoobBot
* Credit : Vesper, Neo2003, Dreadlocks, Ryuichiro
* Thanks you !
*/

using System;
using System.IO;
using System.Linq;
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
    internal static float Version = 1.0f;

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
        Logging.WriteDebug("Loaded " + ObjectManager.Me.WowSpecialization() + " Combat Class " + Version.ToString("0.0###"));

        // Last field is intentionnally ommited because it's a backing field.
    }
}

#region Rogue

public class RogueAssassination
{
    private static RogueAssassinationSettings MySettings = RogueAssassinationSettings.GetSettings();

    #region General Timers & Variables

    private readonly WoWItem _firstTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET);
    private readonly WoWItem _secondTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET, 2);

    private bool CombatMode = true;

    private Timer DefensiveTimer = new Timer(0);

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

    private readonly Spell DeeperStratagem = new Spell("Deeper Stratagem");
    private readonly Spell ElaboratePlanning = new Spell(193640);
    private readonly Spell Nightstalker = new Spell("Nightstalker");
    private readonly Spell Subterfuge = new Spell("Subterfuge");

    #endregion

    #region Buffs

    private readonly Spell AgonizingPoison = new Spell("Agonizing Poison"); //No GCD
    private readonly Spell CripplingPoison = new Spell("Crippling Poison"); //No GCD
    private readonly Spell DeadlyPoison = new Spell("Deadly Poison"); //No GCD
    private readonly Spell LeechingPoison = new Spell("Leeching Poison"); //No GCD
    private readonly Spell ElaboratePlanningBuff = new Spell(193641);

    #endregion

    #region Artifact Spells

    private readonly Spell Kingsbane = new Spell("Kingsbane");
    private Timer KingsbaneTimer = new Timer(0);

    #endregion

    #region Offensive Spells

    private readonly Spell DeathfromAbove = new Spell("Death from Above");
    private readonly Spell Envenom = new Spell("Envenom");
    private readonly Spell Exsanguinate = new Spell("Exsanguinate");
    private bool GarroteHasExsanguinateBuff = false;
    private bool RuptureHasExsanguinateBuff = false;
    private readonly Spell FanofKnives = new Spell("Fan of Knives");
    private readonly Spell Garrote = new Spell("Garrote");
    private readonly Spell Mutilate = new Spell("Mutilate");
    private readonly Spell Hemorrhage = new Spell("Hemorrhage");
    private readonly Spell Rupture = new Spell("Rupture");
    private readonly Spell SinisterStrike = new Spell("Sinister Strike");

    #endregion

    #region Offensive Cooldowns

    private readonly Spell MarkedforDeath = new Spell("Marked for Death"); //No GCD
    private readonly Spell Vanish = new Spell("Vanish"); //No GCD
    private readonly Spell Vendetta = new Spell("Vendetta"); //No GCD

    #endregion

    #region Defensive Spells

    private readonly Spell CloakofShadows = new Spell("Cloak of Shadows"); //No GCD
    private readonly Spell Evasion = new Spell("Evasion"); //No GCD
    private readonly Spell Feint = new Spell("Feint"); //No GCD
    private readonly Spell TricksoftheTrade = new Spell("Tricks of the Trade"); //No GCD

    #endregion

    #region Healing Spell

    private readonly Spell CrimsonVial = new Spell("Crimson Vial");

    #endregion

    #region Utility Spells

    private readonly Spell CheapShot = new Spell("Cheap Shot");
    //private readonly Spell Distract = new Spell("Distract");
    //private readonly Spell Kick = new Spell("Kick"); //No GCD
    private readonly Spell KidneyShot = new Spell("Kidney Shot");
    //private readonly Spell Sap = new Spell("Sap");
    //private readonly Spell Shadowstep = new Spell("Shadowstep"); //No GCD
    private readonly Spell Sprint = new Spell("Sprint"); //No GCD
    private readonly Spell StealthBuff = new Spell("Stealth"); //No GCD

    #endregion

    public RogueAssassination()
    {
        Main.InternalRange = ObjectManager.Me.GetCombatReach;
        Main.InternalAggroRange = Main.InternalRange;
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
            if (!Darkflight.HaveBuff && !Sprint.HaveBuff) // doesn't stack
            {
                if (MySettings.UseDarkflight && Darkflight.IsSpellUsable)
                {
                    Darkflight.Cast();
                    return;
                }
                if (MySettings.UseSprint && Sprint.IsSpellUsable)
                {
                    Sprint.Cast();
                    return;
                }
            }
            //Stealth
            if (MySettings.UseStealth && StealthBuff.IsSpellUsable && !StealthBuff.HaveBuff)
            {
                StealthBuff.Cast();
                return;
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

        if (StealthBuff.HaveBuff)
            Stealth();
        else
        {
            Healing();
            Defensive();
            AggroManagement();
            Offensive();
            Rotation();
        }
    }

    private void Stealth()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            //1. Cast Rupture when
            if (MySettings.UseRupture && Rupture.IsSpellUsable && Rupture.IsHostileDistanceGood &&
                //you have max combo points and you have the Nightstalker Talent
                GetFreeComboPoints() == 0 && Nightstalker.HaveBuff)
            {
                Rupture.Cast();
                return;
            }
            //2. Cast Garrote when
            if (MySettings.UseGarrote && Garrote.IsSpellUsable && Garrote.IsHostileDistanceGood &&
                //you have the Nightstalker or Subterfuge Talent
                !Garrote.TargetHaveBuffFromMe && (Nightstalker.HaveBuff || Subterfuge.HaveBuff))
            {
                Garrote.Cast();
                return;
            }
            //3. Cast Mutilate.
            if (MySettings.UseMutilate && Mutilate.IsSpellUsable && Mutilate.IsHostileDistanceGood
                //you have the Nightstalker
                && Nightstalker.HaveBuff)
            {
                Mutilate.Cast();
                return;
            }
            //4. Cast Cheap Shot when
            if (MySettings.UseCheapShot && CheapShot.IsSpellUsable && CheapShot.IsHostileDistanceGood &&
                //the target is stunnable
                ObjectManager.Target.IsStunnable && !ObjectManager.Target.IsStunned)
            {
                CheapShot.Cast();
                return;
            }
            //5. Cast Garrote
            if (MySettings.UseGarrote && Garrote.IsSpellUsable && Garrote.IsHostileDistanceGood &&
                !Garrote.TargetHaveBuffFromMe)
            {
                Garrote.Cast();
                return;
            }
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
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
            //Crimson Vial
            if (ObjectManager.Me.HealthPercent < MySettings.UseCrimsonVialBelowPercentage &&
                CrimsonVial.IsSpellUsable && ObjectManager.Me.Energy >= 30)
            {
                CrimsonVial.Cast();
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

            if (!ObjectManager.Target.IsStunned && (DefensiveTimer.IsReady || ObjectManager.Me.HealthPercent < 20))
            {
                //Stun
                if (ObjectManager.Target.IsStunnable)
                {
                    if (ObjectManager.Me.HealthPercent < MySettings.UseWarStompBelowPercentage && WarStomp.IsSpellUsable)
                    {
                        WarStomp.Cast();
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
                if (ObjectManager.Me.HealthPercent < MySettings.UseFeintBelowPercentage &&
                    Feint.IsSpellUsable && ObjectManager.Me.Energy >= 20)
                {
                    Feint.Cast();
                    DefensiveTimer = new Timer(1000*5);
                    return true;
                }
            }
            //Mitigate Damage in Emergency Situations
            //Cloak of Shadows
            if (ObjectManager.Me.HealthPercent < MySettings.UseCloakofShadowsBelowPercentage && CloakofShadows.IsSpellUsable)
            {
                CloakofShadows.Cast();
                DefensiveTimer = new Timer(1000*5);
                return true;
            }
            //Evasion
            if (ObjectManager.Me.HealthPercent < MySettings.UseEvasionBelowPercentage && Evasion.IsSpellUsable)
            {
                Evasion.Cast();
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

    // For Spots (always return after Casting)
    private bool AggroManagement()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            //Cast Tricks of the Trade on Tank when
            if (ObjectManager.Me.HealthPercent < MySettings.UseTricksoftheTradeBelowPercentage && TricksoftheTrade.IsSpellUsable
                //you are in a Group and
                && Party.IsInGroup())
            {
                WoWPlayer tank = new WoWPlayer(0);
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
                    if (currentPlayer.GetUnitRole == WoWUnit.PartyRole.Tank && tank != currentPlayer)
                        tank = currentPlayer;
                }
                //the Tank has more than 20% Health
                if (tank.HealthPercent > 20 && CombatClass.InSpellRange(tank, TricksoftheTrade.MinRangeFriend, TricksoftheTrade.MaxRangeFriend))
                    TricksoftheTrade.Cast(false, true, false, tank.GetUnitId());
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

            //Apply Poisons
            if (!AgonizingPoison.HaveBuff)
            {
                if (MySettings.UseAgonizingPoison && AgonizingPoison.IsSpellUsable)
                {
                    AgonizingPoison.CastOnSelf();
                }
                if (MySettings.UseDeadlyPoison && DeadlyPoison.IsSpellUsable && !DeadlyPoison.HaveBuff)
                {
                    DeadlyPoison.CastOnSelf();
                }
            }
            if (!LeechingPoison.HaveBuff)
            {
                if (MySettings.UseLeechingPoison && LeechingPoison.IsSpellUsable)
                {
                    LeechingPoison.CastOnSelf();
                }
                if (MySettings.UseCripplingPoison && CripplingPoison.IsSpellUsable && !CripplingPoison.HaveBuff)
                {
                    CripplingPoison.CastOnSelf();
                }
            }

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

            //Cast Kidney Shot when you have X combo points and
            if (ObjectManager.Me.ComboPoint > MySettings.UseKidneyShotAboveComboPoints && KidneyShot.IsSpellUsable
                && ObjectManager.Me.Energy >= 25 && KidneyShot.IsHostileDistanceGood &&
                //the target is stunnable and not stunned
                ObjectManager.Target.IsStunnable && !ObjectManager.Target.IsStunned)
            {
                KidneyShot.Cast();
                return;
            }
            //Cast Exsanguinate when
            if (MySettings.UseExsanguinate && Exsanguinate.IsSpellUsable && Exsanguinate.IsHostileDistanceGood &&
                //Kingsbane Dot is up
                Kingsbane.TargetHaveBuff)
            {
                GarroteHasExsanguinateBuff = true;
                RuptureHasExsanguinateBuff = true;
                Exsanguinate.Cast();
                return;
            }
            //Cast Marked for Death (when talented) when
            if (MySettings.UseMarkedforDeath && MarkedforDeath.IsSpellUsable && MarkedforDeath.IsHostileDistanceGood &&
                //you have 5 free combo points.
                GetFreeComboPoints() >= 5)
            {
                MarkedforDeath.Cast();
                return;
            }

            //1. Maintain Rupture when
            if (MySettings.UseRupture && Rupture.IsSpellUsable && Rupture.IsHostileDistanceGood &&
                //you have 3+ combo points and Rupture isn't on the target.
                ObjectManager.Me.ComboPoint >= 3 && !Rupture.TargetHaveBuffFromMe)
            {
                //it won't reset Exsanguinate
                if (!Rupture.TargetHaveBuffFromMe || !RuptureHasExsanguinateBuff)
                {
                    RuptureHasExsanguinateBuff = false;
                    Rupture.Cast();
                    return;
                }
            }
            //2. Activate Vendetta on the primary target when it is off cooldown.
            if (MySettings.UseVendetta && Vendetta.IsSpellUsable && Vendetta.IsHostileDistanceGood)
            {
                Vendetta.Cast();
            }
            //3. Activate Vanish when you aren't in stealth and
            if (MySettings.UseVanish && Vanish.IsSpellUsable && !StealthBuff.HaveBuff &&
                //you have max combo points
                GetFreeComboPoints() == 0) // && !SpellManager.GetAllSpellsOnCooldown.Exists(entry => entry.SpellId == 1856))
            {
                Vanish.Cast();
                return;
            }
            //1b. Maintain Rupture when
            if (MySettings.UseRupture && Rupture.IsSpellUsable && Rupture.IsHostileDistanceGood &&
                //you have max combo points and it has 8 or less seconds remaining
                GetFreeComboPoints() == 0 && ObjectManager.Target.UnitAura(Rupture.Ids, ObjectManager.Me.Guid).AuraTimeLeftInMs <= 8000)
            {
                //it won't reset Exsanguinate
                if (!Rupture.TargetHaveBuffFromMe || !RuptureHasExsanguinateBuff)
                {
                    RuptureHasExsanguinateBuff = false;
                    Rupture.Cast();
                    return;
                }
            }
            //4. Maintain Garrote Dot when it is off cooldown and
            if (MySettings.UseGarrote && Garrote.IsSpellUsable && Garrote.IsHostileDistanceGood &&
                //it has 6 or less seconds remaining
                ObjectManager.Target.UnitAura(Garrote.Ids, ObjectManager.Me.Guid).AuraTimeLeftInMs <= 6000)
            {
                //it won't reset Exsanguinate
                if (!Garrote.TargetHaveBuffFromMe || !GarroteHasExsanguinateBuff)
                {
                    GarroteHasExsanguinateBuff = false;
                    Garrote.Cast();
                    return;
                }
            }
            //5. Cast Kingsbane when it is off cooldown.
            if (MySettings.UseKingsbane && Kingsbane.IsSpellUsable && Kingsbane.IsHostileDistanceGood)
            {
                Kingsbane.Cast();
                return;
            }
            //Cast Death from Above when
            if (MySettings.UseDeathfromAbove && DeathfromAbove.IsSpellUsable && DeathfromAbove.IsHostileDistanceGood &&
                //you have max combo points and Elaborate Planning isn't up and
                GetFreeComboPoints() == 0 && !ObjectManager.Me.UnitAura(ElaboratePlanningBuff.Id).IsValid &&
                //Envenom isn't up
                !Envenom.HaveBuff)
            {
                DeathfromAbove.Cast();
                return;
            }
            //6. Cast Envenom when
            if (MySettings.UseEnvenom && Envenom.IsSpellUsable && Envenom.IsHostileDistanceGood &&
                //you have max combo points and Elaborate Planning isn't up and
                GetFreeComboPoints() == 0 && !ObjectManager.Me.UnitAura(ElaboratePlanningBuff.Id).IsValid &&
                //Envenom isn't up
                !Envenom.HaveBuff)
            {
                Envenom.Cast();
                return;
            }
            //7. Cast Fan of Knives when it hits 3+ targets 
            if (MySettings.UseFanofKnives && FanofKnives.IsSpellUsable && ObjectManager.Me.GetUnitInSpellRange(10f) >= 3)
            {
                FanofKnives.Cast();
                return;
            }
            //8. Cast Mutilate.
            if (MySettings.UseMutilate && Mutilate.IsSpellUsable && Mutilate.IsHostileDistanceGood)
            {
                Mutilate.Cast();
                return;
            }
            //9. Cast Sinister Strike.
            if (MySettings.UseMutilate && !Mutilate.KnownSpell && SinisterStrike.IsSpellUsable && SinisterStrike.IsHostileDistanceGood)
            {
                SinisterStrike.Cast();
                return;
            }
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    // Checks free combo points before capping
    private int GetFreeComboPoints()
    {
        return ((DeeperStratagem.HaveBuff) ? 6 : 5) - (int) ObjectManager.Me.ComboPoint;
    }

    #region Nested type: RogueAssassinationSettings

    [Serializable]
    public class RogueAssassinationSettings : Settings
    {
        /* Professions & Racials */
        //public bool UseArcaneTorrent = true;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseDarkflight = true;
        public int UseGiftoftheNaaruBelowPercentage = 50;
        public int UseStoneformBelowPercentage = 50;
        public int UseWarStompBelowPercentage = 50;

        /* Poisons */
        public bool UseAgonizingPoison = true;
        public bool UseCripplingPoison = true;
        public bool UseDeadlyPoison = true;
        public bool UseLeechingPoison = true;

        /* Artifact Spells */
        public bool UseKingsbane = true;

        /* Offensive Spells */
        public bool UseDeathfromAbove = true;
        public bool UseEnvenom = true;
        public bool UseExsanguinate = true;
        public bool UseFanofKnives = true;
        public bool UseGarrote = true;
        public bool UseMutilate = true;
        public bool UseRupture = true;

        /* Offensive Cooldowns */
        public bool UseMarkedforDeath = true;
        public bool UseVanish = true;
        public bool UseVendetta = true;

        /* Defensive Spells */
        public int UseCloakofShadowsBelowPercentage = 0;
        public int UseEvasionBelowPercentage = 10;
        public int UseFeintBelowPercentage = 0;
        public int UseTricksoftheTradeBelowPercentage = 100;

        /* Healing Spells */
        public int UseCrimsonVialBelowPercentage = 70;

        /* Utility Spells */
        public bool UseCheapShot = true;
        public int UseKidneyShotAboveComboPoints = 10;
        public bool UseSprint = true;
        public bool UseStealth = true;

        /* Game Settings */
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;

        public RogueAssassinationSettings()
        {
            ConfigWinForm("Rogue Assassination Settings");
            /* Professions & Racials */
            //AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Darkflight", "UseDarkflight", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaruBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            AddControlInWinForm("Use Stone Form", "UseStoneformBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            AddControlInWinForm("Use War Stomp", "UseWarStompBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            /* Poisons */
            AddControlInWinForm("Use Agonizing Poison", "UseAgonizingPoison", "Poisons");
            AddControlInWinForm("Use Deadly Poison", "UseDeadlyPoison", "Poisons");
            AddControlInWinForm("Use Crippling Poison", "UseCripplingPoison", "Poisons");
            AddControlInWinForm("Use Leeching Poison", "UseLeechingPoison", "Poisons");
            /* Artifact Spells */
            AddControlInWinForm("Use Kingsbane", "UseKingsbane", "Artifact Spells");
            /* Offensive Spells */
            AddControlInWinForm("Use Death from Above", "UseDeathfromAbove", "Offensive Spells");
            AddControlInWinForm("Use Envenom", "UseEnvenom", "Offensive Spells");
            AddControlInWinForm("Use Exsanguinate", "UseExsanguinate", "Offensive Spells");
            AddControlInWinForm("Use Fan of Knives", "UseFanofKnives", "Offensive Spells");
            AddControlInWinForm("Use Garrote", "UseGarrote", "Offensive Spells");
            AddControlInWinForm("Use Mutilate", "UseMutilate", "Offensive Spells");
            AddControlInWinForm("Use Rupture", "UseRupture", "Offensive Spells");
            /* Offensive Cooldowns */
            AddControlInWinForm("Use Marked for Death", "UseMarkedforDeath", "Offensive Cooldowns");
            AddControlInWinForm("Use Vanish", "UseVanish", "Offensive Cooldowns");
            AddControlInWinForm("Use Vendetta", "UseVendetta", "Offensive Cooldowns");
            /* Defensive Spells */
            AddControlInWinForm("Use Cloak of Shadows", "UseCloakofShadowsBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Evasion", "UseEvasionBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Feint", "UseFeintBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Tricks of the Trade", "UseTricksoftheTradeBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            /* Healing Spell */
            AddControlInWinForm("Use Crimson Vial", "UseCrimsonVialBelowPercentage", "Healing Spells", "BelowPercentage", "Life");
            /* Utility Spells */
            AddControlInWinForm("Use Cheap Shot", "UseCheapShot", "Utility Spells");
            AddControlInWinForm("Use Kidney Shot", "UseKidneyShotAboveComboPoints", "Offensive Spells", "AbovePercentage", "Combo Points");
            AddControlInWinForm("Use Sprint", "UseSprint", "Utility Spells");
            AddControlInWinForm("Use Stealth", "UseStealth", "Utility Spells");
            /* Game Settings */
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");
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

public class RogueOutlaw
{
    private static RogueOutlawSettings MySettings = RogueOutlawSettings.GetSettings();

    #region General Timers & Variables

    private readonly WoWItem _firstTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET);
    private readonly WoWItem _secondTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET, 2);

    private bool CombatMode = true;

    private Timer DefensiveTimer = new Timer(0);

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

    private readonly Spell DeeperStratagem = new Spell("Deeper Stratagem");

    #endregion

    #region Buffs

    private readonly Spell Broadsides = new Spell(193356);
    private readonly Spell BuriedTreasure = new Spell(199600);
    private readonly Spell GrandMelee = new Spell(193358);
    private readonly Spell JollyRoger = new Spell(199603);
    private readonly Spell Opportunity = new Spell(195627);
    private readonly Spell SharkInfestedWaters = new Spell(193357);
    private readonly Spell TrueBearing = new Spell(193359);

    #endregion

    #region Artifact Spells

    private readonly Spell CurseoftheDreadblades = new Spell("Curse of the Dreadblades");

    #endregion

    #region Offensive Spells

    private readonly Spell BetweentheEyes = new Spell("Between the Eyes");
    private readonly Spell BladeFlurry = new Spell("Blade Flurry");
    private readonly Spell DeathfromAbove = new Spell("Death from Above");
    private readonly Spell GhostlyStrike = new Spell("Ghostly Strike");
    private readonly Spell PistolShot = new Spell("Pistol Shot");
    private readonly Spell RolltheBones = new Spell("Roll the Bones");
    private readonly Spell RunThrough = new Spell("Run Through");
    private readonly Spell SaberSlash = new Spell("Saber Slash");
    private readonly Spell SliceandDice = new Spell("Slice and Dice");

    #endregion

    #region Offensive Cooldowns

    private readonly Spell AdrenalineRush = new Spell("Adrenaline Rush"); //No GCD
    private readonly Spell CannonballBarrage = new Spell("Cannonball Barrage");
    private readonly Spell KillingSpree = new Spell("Killing Spree");
    private readonly Spell MarkedforDeath = new Spell("Marked for Death"); //No GCD
    private readonly Spell Vanish = new Spell("Vanish"); //No GCD

    #endregion

    #region Defensive Spells

    private readonly Spell CloakofShadows = new Spell("Cloak of Shadows"); //No GCD
    private readonly Spell Feint = new Spell("Feint"); //No GCD
    private readonly Spell Riposte = new Spell("Riposte"); //No GCD
    private readonly Spell TricksoftheTrade = new Spell("Tricks of the Trade"); //No GCD

    #endregion

    #region Healing Spell

    private readonly Spell CrimsonVial = new Spell("Crimson Vial");

    #endregion

    #region Utility Spells

    private readonly Spell CheapShot = new Spell("Cheap Shot");
    //private readonly Spell Distract = new Spell("Distract");
    private readonly Spell GrapplingHook = new Spell("Grappling Hook");
    //private readonly Spell Kick = new Spell("Kick"); //No GCD
    //private readonly Spell Parley = new Spell("Parley");
    //private readonly Spell Sap = new Spell("Sap");
    private readonly Spell Sprint = new Spell("Sprint"); //No GCD
    private readonly Spell StealthBuff = new Spell("Stealth"); //No GCD

    #endregion

    public RogueOutlaw()
    {
        Main.InternalRange = ObjectManager.Me.GetCombatReach;
        Main.InternalAggroRange = Main.InternalRange;
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
            if (!Darkflight.HaveBuff && !Sprint.HaveBuff) // doesn't stack
            {
                if (MySettings.UseDarkflight && Darkflight.IsSpellUsable)
                {
                    Darkflight.Cast();
                    return;
                }
                if (MySettings.UseSprint && Sprint.IsSpellUsable)
                {
                    Sprint.Cast();
                    return;
                }
            }
            //Stealth
            if (MySettings.UseStealth && StealthBuff.IsSpellUsable && !StealthBuff.HaveBuff)
            {
                StealthBuff.Cast();
                return;
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
            //Crimson Vial
            if (ObjectManager.Me.HealthPercent < MySettings.UseCrimsonVialBelowPercentage &&
                CrimsonVial.IsSpellUsable && ObjectManager.Me.Energy >= 30)
            {
                CrimsonVial.Cast();
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

            if (!ObjectManager.Target.IsStunned && (DefensiveTimer.IsReady || ObjectManager.Me.HealthPercent < 20))
            {
                //Stun
                if (ObjectManager.Target.IsStunnable)
                {
                    if (ObjectManager.Me.HealthPercent < MySettings.UseWarStompBelowPercentage && WarStomp.IsSpellUsable)
                    {
                        WarStomp.Cast();
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
                if (ObjectManager.Me.HealthPercent < MySettings.UseFeintBelowPercentage &&
                    Feint.IsSpellUsable && ObjectManager.Me.Energy >= 20)
                {
                    Feint.Cast();
                    DefensiveTimer = new Timer(1000*5);
                    return true;
                }
            }
            //Mitigate Damage in Emergency Situations
            //Cloak of Shadows
            if (ObjectManager.Me.HealthPercent < MySettings.UseCloakofShadowsBelowPercentage && CloakofShadows.IsSpellUsable)
            {
                CloakofShadows.Cast();
                DefensiveTimer = new Timer(1000*5);
                return true;
            }
            //Riposte
            if (ObjectManager.Me.HealthPercent < MySettings.UseRiposteBelowPercentage && Riposte.IsSpellUsable)
            {
                Riposte.Cast();
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
            //Cast Vanish
            if (MySettings.UseVanish && Vanish.IsSpellUsable)
            {
                Vanish.Cast();
                return true;
            }
            //Toggle Blade Flurry when
            if (MySettings.UseBladeFlurry && BladeFlurry.IsSpellUsable &&
                //it is disabled and multiple enemies are stacked
                ((!BladeFlurry.HaveBuff && ObjectManager.Me.GetUnitInSpellRange(5f) > 1) ||
                 //it is enabled and there aren't multiple enmies stacked
                 (BladeFlurry.HaveBuff && ObjectManager.Me.GetUnitInSpellRange(5f) <= 1)))
            {
                BladeFlurry.Cast();
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

            //Cast Cheap Shot when
            if (MySettings.UseCheapShot && CheapShot.IsSpellUsable &&
                ObjectManager.Me.Energy >= 40 && CheapShot.IsHostileDistanceGood &&
                //the target is stunnable and you are stealthed
                ObjectManager.Target.IsStunnable && StealthBuff.HaveBuff)
            {
                CheapShot.Cast();
                return;
            }
            //1. Apply Roll the Bones when
            if (MySettings.UseRolltheBones && RolltheBones.IsSpellUsable &&
                ObjectManager.Me.Energy >= 25 &&
                //you have max combo points and
                GetFreeComboPoints() == 0 &&
                //you don't have the True Bearing Buff and less then 2 different Roll the Bones Buffs
                !TrueBearing.HaveBuff && GetRolltheBonesBuffs() < 2)
            {
                RolltheBones.Cast();
                return;
            }
            //2. Maintain Ghostly Strike when
            if (MySettings.UseGhostlyStrike && GhostlyStrike.IsSpellUsable &&
                ObjectManager.Me.Energy >= 30 && GhostlyStrike.IsHostileDistanceGood &&
                !GhostlyStrike.TargetHaveBuff)
            {
                GhostlyStrike.Cast();
                return;
            }
            //3. Activate Adrenaline Rush
            if (MySettings.UseAdrenalineRush && AdrenalineRush.IsSpellUsable)
            {
                AdrenalineRush.Cast();
                return;
            }
            //4. Activate Curse of the Dreadblades
            if (MySettings.UseCurseoftheDreadblades && CurseoftheDreadblades.IsSpellUsable)
            {
                CurseoftheDreadblades.Cast();
                return;
            }
            //5. Cast Marked for Death (when talented) when
            if (MySettings.UseMarkedforDeath && MarkedforDeath.IsSpellUsable && MarkedforDeath.IsHostileDistanceGood &&
                //you have 5 free combo points.
                GetFreeComboPoints() >= 5)
            {
                MarkedforDeath.Cast();
                return;
            }
            //6. Cast Death from Above (when talented) when
            if (MySettings.UseDeathfromAbove && DeathfromAbove.IsSpellUsable &&
                ObjectManager.Me.Energy >= 25 && DeathfromAbove.IsHostileDistanceGood &&
                //you have max combo points and
                GetFreeComboPoints() == 0 &&
                //Adrenaline Rush is not active.
                !AdrenalineRush.HaveBuff)
            {
                DeathfromAbove.Cast();
                return;
            }
            //7. Cast Run Through when
            if (MySettings.UseRunThrough && RunThrough.IsSpellUsable &&
                ObjectManager.Me.Energy >= 35 && RunThrough.IsHostileDistanceGood &&
                //you have max combo points
                GetFreeComboPoints() == 0)
            {
                RunThrough.Cast();
                return;
            }
            //8. Cast Between the Eyes when
            if (MySettings.UseBetweentheEyes && BetweentheEyes.IsSpellUsable &&
                ObjectManager.Me.Energy >= 35 && BetweentheEyes.IsHostileDistanceGood &&
                //you have max combo points
                GetFreeComboPoints() == 0)
            {
                BetweentheEyes.Cast();
                return;
            }
            //9. Cast Pistol Shot when
            if (MySettings.UsePistolShot && PistolShot.IsSpellUsable &&
                ObjectManager.Me.Energy >= 40 && PistolShot.IsHostileDistanceGood &&
                //your combo points aren't capping and
                GetFreeComboPoints() > 0 &&
                //you have an Opportunity proc.
                Opportunity.HaveBuff)
            {
                PistolShot.Cast();
                return;
            }
            //10. Cast Saber Slash
            if (MySettings.UseSaberSlash && SaberSlash.IsSpellUsable &&
                ObjectManager.Me.Energy >= 50 && SaberSlash.IsHostileDistanceGood)
            {
                SaberSlash.Cast();
                return;
            }
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    // Checks number of Roll the Bones Buffs
    private int GetRolltheBonesBuffs()
    {
        uint[] idBuffs = {Broadsides.Id, BuriedTreasure.Id, GrandMelee.Id, JollyRoger.Id, SharkInfestedWaters.Id, TrueBearing.Id};
        int buffs = 0;
        for (int i = 0; i < ObjectManager.Me.UnitAuras.Auras.Count; i++)
            if (idBuffs.Contains(ObjectManager.Me.UnitAuras.Auras[i].AuraSpellId))
                buffs++;
        return buffs;
    }

    // Checks free combo points before capping
    private int GetFreeComboPoints()
    {
        return ((DeeperStratagem.HaveBuff) ? 6 : 5) - (int) ObjectManager.Me.ComboPoint;
    }

    #region Nested type: RogueOutlawSettings

    [Serializable]
    public class RogueOutlawSettings : Settings
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
        public bool UseCurseoftheDreadblades = true;

        /* Offensive Spells */
        public bool UseBetweentheEyes = true;
        public bool UseBladeFlurry = true;
        public bool UseDeathfromAbove = true;
        public bool UseGhostlyStrike = true;
        public bool UsePistolShot = true;
        public bool UseRolltheBones = true;
        public bool UseRunThrough = true;
        public bool UseSaberSlash = true;

        /* Offensive Cooldowns */
        public bool UseAdrenalineRush = true;
        public bool UseMarkedforDeath = true;
        public bool UseVanish = true;

        /* Defensive Spells */
        public int UseCloakofShadowsBelowPercentage = 0;
        public int UseFeintBelowPercentage = 0;
        public int UseRiposteBelowPercentage = 10;
        public int UseTricksoftheTradeBelowPercentage = 100;

        /* Healing Spells */
        public int UseCrimsonVialBelowPercentage = 70;

        /* Utility Spells */
        public bool UseCheapShot = true;
        public bool UseSprint = true;
        public bool UseStealth = true;

        /* Game Settings */
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;

        public RogueOutlawSettings()
        {
            ConfigWinForm("Rogue Outlaw Settings");
            /* Professions & Racials */
            //AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Darkflight", "UseDarkflight", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaruBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            AddControlInWinForm("Use Stone Form", "UseStoneformBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            AddControlInWinForm("Use War Stomp", "UseWarStompBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            /* Artifact Spells */
            AddControlInWinForm("Use Curse of the Dreadblades", "UseCurseoftheDreadblades", "Artifact Spells");
            /* Offensive Spells */
            AddControlInWinForm("Use Between the Eyes", "UseBetweentheEyes", "Offensive Spells");
            AddControlInWinForm("Use Blade Flurry", "UseBladeFlurry", "Offensive Spells");
            AddControlInWinForm("Use Death from Above", "UseDeathfromAbove", "Offensive Spells");
            AddControlInWinForm("Use Ghostly Strike", "UseGhostlyStrike", "Offensive Spells");
            AddControlInWinForm("Use Pistol Shot", "UsePistolShot", "Offensive Spells");
            AddControlInWinForm("Use Roll the Bones", "UseRolltheBones", "Offensive Spells");
            AddControlInWinForm("Use Run Through", "UseRunThrough", "Offensive Spells");
            AddControlInWinForm("Use Saber Slash", "UseSaberSlash", "Offensive Spells");
            /* Offensive Cooldowns */
            AddControlInWinForm("Use Adrenaline Rush", "UseAdrenalineRush", "Offensive Cooldowns");
            AddControlInWinForm("Use Marked for Death", "UseMarkedforDeath", "Offensive Cooldowns");
            AddControlInWinForm("Use Vanish", "UseVanish", "Offensive Cooldowns");
            /* Defensive Spells */
            AddControlInWinForm("Use Cloak of Shadows", "UseCloakofShadowsBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Feint", "UseFeintBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Riposte", "UseRiposteBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Tricks of the Trade", "UseTricksoftheTradeBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            /* Healing Spell */
            AddControlInWinForm("Use Crimson Vial", "UseCrimsonVialBelowPercentage", "Healing Spells", "BelowPercentage", "Life");
            /* Utility Spells */
            AddControlInWinForm("Use Cheap Shot", "UseCheapShot", "Utility Spells");
            AddControlInWinForm("Use Sprint", "UseSprint", "Utility Spells");
            AddControlInWinForm("Use Stealth", "UseStealth", "Utility Spells");
            /* Game Settings */
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");
        }

        public static RogueOutlawSettings CurrentSetting { get; set; }

        public static RogueOutlawSettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Rogue_Outlaw.xml";
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

    private bool CombatMode = true;

    private Timer DefensiveTimer = new Timer(0);

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

    private readonly Spell DeeperStratagem = new Spell("Deeper Stratagem");

    #endregion

    #region Artifact Spells

    private readonly Spell GoremawsBite = new Spell("Goremaw's Bite");

    #endregion

    #region Offensive Spells

    private readonly Spell Backstab = new Spell("Backstab");
    private readonly Spell DeathfromAbove = new Spell("Death from Above");
    private readonly Spell EnvelopingShadows = new Spell("Enveloping Shadows");
    private readonly Spell Eviscerate = new Spell("Eviscerate");
    private readonly Spell Gloomblade = new Spell("Gloomblade");
    private readonly Spell Nightblade = new Spell("Nightblade");
    private readonly Spell Shadowstrike = new Spell("Shadowstrike");
    private readonly Spell ShurikenStorm = new Spell("Shuriken Storm");
    private readonly Spell SymbolsofDeath = new Spell("Symbols of Death"); //No GCD

    #endregion

    #region Offensive Cooldowns

    private readonly Spell MarkedforDeath = new Spell("Marked for Death"); //No GCD
    private readonly Spell ShadowBlades = new Spell("Shadow Blades"); //No GCD
    private readonly Spell ShadowDance = new Spell("Shadow Dance"); //No GCD
    private readonly Spell Vanish = new Spell("Vanish"); //No GCD

    #endregion

    #region Defensive Spells

    private readonly Spell CloakofShadows = new Spell("Cloak of Shadows"); //No GCD
    private readonly Spell Evasion = new Spell("Evasion"); //No GCD
    private readonly Spell Feint = new Spell("Feint"); //No GCD
    private readonly Spell TricksoftheTrade = new Spell("Tricks of the Trade"); //No GCD

    #endregion

    #region Healing Spell

    private readonly Spell CrimsonVial = new Spell("Crimson Vial");

    #endregion

    #region Utility Spells

    //private readonly Spell CheapShot = new Spell("Cheap Shot");
    //private readonly Spell Distract = new Spell("Distract");
    //private readonly Spell Kick = new Spell("Kick"); //No GCD
    private readonly Spell KidneyShot = new Spell("Kidney Shot");
    //private readonly Spell Sap = new Spell("Sap");
    //private readonly Spell Shadowstep = new Spell("Shadowstep"); //No GCD
    private readonly Spell Sprint = new Spell("Sprint"); //No GCD
    private readonly Spell StealthBuff = new Spell("Stealth"); //No GCD

    #endregion

    public RogueSubtlety()
    {
        Main.InternalRange = ObjectManager.Me.GetCombatReach;
        Main.InternalAggroRange = Main.InternalRange;
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
            if (!Darkflight.HaveBuff && !Sprint.HaveBuff) // doesn't stack
            {
                if (MySettings.UseDarkflight && Darkflight.IsSpellUsable)
                {
                    Darkflight.Cast();
                    return;
                }
                if (MySettings.UseSprint && Sprint.IsSpellUsable)
                {
                    Sprint.Cast();
                    return;
                }
            }
            //Stealth
            if (MySettings.UseStealth && StealthBuff.IsSpellUsable && !StealthBuff.HaveBuff)
            {
                StealthBuff.Cast();
                return;
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
        Defensive();
        AggroManagement();
        Offensive();
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
            //Crimson Vial
            if (ObjectManager.Me.HealthPercent < MySettings.UseCrimsonVialBelowPercentage &&
                CrimsonVial.IsSpellUsable && ObjectManager.Me.Energy >= 30)
            {
                CrimsonVial.Cast();
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

            if (!ObjectManager.Target.IsStunned && (DefensiveTimer.IsReady || ObjectManager.Me.HealthPercent < 20))
            {
                //Stun
                if (ObjectManager.Target.IsStunnable)
                {
                    if (ObjectManager.Me.HealthPercent < MySettings.UseWarStompBelowPercentage && WarStomp.IsSpellUsable)
                    {
                        WarStomp.Cast();
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
                if (ObjectManager.Me.HealthPercent < MySettings.UseFeintBelowPercentage &&
                    Feint.IsSpellUsable && ObjectManager.Me.Energy >= 20)
                {
                    Feint.Cast();
                    DefensiveTimer = new Timer(1000*5);
                    return true;
                }
            }
            //Mitigate Damage in Emergency Situations
            //Cloak of Shadows
            if (ObjectManager.Me.HealthPercent < MySettings.UseCloakofShadowsBelowPercentage && CloakofShadows.IsSpellUsable)
            {
                CloakofShadows.Cast();
                DefensiveTimer = new Timer(1000*5);
                return true;
            }
            //Evasion
            if (ObjectManager.Me.HealthPercent < MySettings.UseEvasionBelowPercentage && Evasion.IsSpellUsable)
            {
                Evasion.Cast();
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

    // For Spots (always return after Casting)
    private bool AggroManagement()
    {
        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            //Cast Tricks of the Trade on Tank when
            if (ObjectManager.Me.HealthPercent < MySettings.UseTricksoftheTradeBelowPercentage && TricksoftheTrade.IsSpellUsable
                //you are in a Group and
                && Party.IsInGroup())
            {
                WoWPlayer tank = new WoWPlayer(0);
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
                    if (currentPlayer.GetUnitRole == WoWUnit.PartyRole.Tank && tank != currentPlayer)
                        tank = currentPlayer;
                }
                //the Tank has more than 20% Health
                if (tank.HealthPercent > 20 && CombatClass.InSpellRange(tank, TricksoftheTrade.MinRangeFriend, TricksoftheTrade.MaxRangeFriend))
                    TricksoftheTrade.Cast(false, true, false, tank.GetUnitId());
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
            //Apply Symbols of Death
            if (MySettings.UseSymbolsofDeath && SymbolsofDeath.IsSpellUsable &&
                ObjectManager.Me.Energy >= 35 && !SymbolsofDeath.HaveBuff)
            {
                SymbolsofDeath.Cast();
                return true;
            }
            //Cast Vanish when you aren't in stealth
            if (MySettings.UseVanish && Vanish.IsSpellUsable && !StealthBuff.HaveBuff)
            {
                Vanish.Cast();
                return true;
            }
            //Apply Shadow Dance when you aren't in stealth and
            if (MySettings.UseShadowDance && ShadowDance.IsSpellUsable && !StealthBuff.HaveBuff &&
                //it has 2 or more charges
                ShadowDance.GetSpellCharges >= 2)
            {
                ShadowDance.Cast();
            }
            //Cast Shadow Blades when
            if (MySettings.UseShadowBlades && ShadowBlades.IsSpellUsable &&
                //you have 4 or more free combo points.
                GetFreeComboPoints() >= 4)
            {
                ShadowBlades.Cast();
                return true;
            }
            //Cast Marked for Death (when talented) when
            if (MySettings.UseMarkedforDeath && MarkedforDeath.IsSpellUsable && MarkedforDeath.IsHostileDistanceGood &&
                //you have 5 free combo points.
                GetFreeComboPoints() >= 5)
            {
                MarkedforDeath.Cast();
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

            //Cast Kidney Shot when you have X combo points and
            if (ObjectManager.Me.ComboPoint > MySettings.UseKidneyShotAboveComboPoints && KidneyShot.IsSpellUsable &&
                ObjectManager.Me.Energy >= 25 && KidneyShot.IsHostileDistanceGood &&
                //the target is stunnable and probably not stunned
                ObjectManager.Target.IsStunnable && !ObjectManager.Target.IsStunned)
            {
                KidneyShot.Cast();
                return;
            }
            //Cast Goremaw's Bite when
            if (MySettings.UseGoremawsBite && GoremawsBite.IsSpellUsable && GoremawsBite.IsHostileDistanceGood &&
                //you have 3 or more free combo points and
                GetFreeComboPoints() >= 3 &&
                //you aren't capping energy
                ObjectManager.Me.EnergyPercentage < 100)
            {
                GoremawsBite.Cast();
                return;
            }

            //Generate combo points if they aren't capping.
            if (GetFreeComboPoints() > 0)
            {
                //Cast Shuriken Storm when
                if (MySettings.UseShurikenStorm && ShurikenStorm.IsSpellUsable && ShurikenStorm.IsHostileDistanceGood &&
                    //you haves 2 or more targets
                    ObjectManager.Me.GetUnitInSpellRange(10f) >= 2)
                {
                    ShurikenStorm.Cast();
                    return;
                }
                //Cast Shadowstrike (only usable in Stelth or Shadow Dance)
                if (MySettings.UseShadowstrike && Shadowstrike.IsSpellUsable &&
                    ObjectManager.Me.Energy >= 40 && Shadowstrike.IsHostileDistanceGood)
                {
                    Shadowstrike.Cast();
                    return;
                }
                //Cast Gloomblade/Backstap
                if (MySettings.UseGloomblade && Gloomblade.IsSpellUsable &&
                    ObjectManager.Me.Energy >= 35 && Gloomblade.IsHostileDistanceGood)
                {
                    Gloomblade.Cast();
                    return;
                }
                else if (MySettings.UseBackstap && Backstab.IsSpellUsable &&
                         ObjectManager.Me.Energy >= 35 && Backstab.IsHostileDistanceGood)
                {
                    Backstab.Cast();
                    return;
                }
            }
                //Spend combo points if they are capping.
            else
            {
                //Apply Enveloping Shadows when
                if (MySettings.UseEnvelopingShadows && EnvelopingShadows.IsSpellUsable && EnvelopingShadows.IsHostileDistanceGood &&
                    //you don't have the Buff
                    EnvelopingShadows.HaveBuff)
                {
                    EnvelopingShadows.Cast();
                    return;
                }
                //Apply Nightblade when
                if (MySettings.UseNightblade && Nightblade.IsSpellUsable &&
                    ObjectManager.Me.Energy >= 25 && Nightblade.IsHostileDistanceGood &&
                    //your target don't have the Dot
                    !Nightblade.TargetHaveBuffFromMe)
                {
                    Nightblade.Cast();
                    return;
                }
                //Cast Eviscerate
                if (MySettings.UseEviscerate && Eviscerate.IsSpellUsable &&
                    ObjectManager.Me.Energy >= 35 && Eviscerate.IsHostileDistanceGood)
                {
                    Eviscerate.Cast();
                    return;
                }
                //4. Cast Death from Above
                if (MySettings.UseDeathfromAbove && DeathfromAbove.IsSpellUsable &&
                    ObjectManager.Me.Energy >= 25 && DeathfromAbove.IsHostileDistanceGood)
                {
                    DeathfromAbove.Cast();
                    return;
                }
            }
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    // Checks free combo points before capping
    private int GetFreeComboPoints()
    {
        return ((DeeperStratagem.HaveBuff) ? 6 : 5) - (int) ObjectManager.Me.ComboPoint;
    }

    #region Nested type: RogueSubtletySettings

    [Serializable]
    public class RogueSubtletySettings : Settings
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
        public bool UseGoremawsBite = true;

        /* Offensive Spells */
        public bool UseBackstap = true;
        public bool UseDeathfromAbove = true;
        public bool UseEnvelopingShadows = true;
        public bool UseEviscerate = true;
        public bool UseGloomblade = true;
        public bool UseNightblade = true;
        public bool UseShadowstrike = true;
        public bool UseShurikenStorm = true;

        /* Offensive Cooldowns */
        public bool UseMarkedforDeath = true;
        public bool UseShadowBlades = true;
        public bool UseShadowDance = true;
        public bool UseSymbolsofDeath = true;
        public bool UseVanish = true;

        /* Defensive Spells */
        public int UseCloakofShadowsBelowPercentage = 0;
        public int UseEvasionBelowPercentage = 10;
        public int UseFeintBelowPercentage = 0;
        public int UseTricksoftheTradeBelowPercentage = 100;

        /* Healing Spells */
        public int UseCrimsonVialBelowPercentage = 70;

        /* Utility Spells */
        //public bool UseCheapShot = true;
        public int UseKidneyShotAboveComboPoints = 10;
        public bool UseSprint = true;
        public bool UseStealth = true;

        /* Game Settings */
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;

        public RogueSubtletySettings()
        {
            ConfigWinForm("Rogue Subtlety Settings");
            /* Professions & Racials */
            //AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Darkflight", "UseDarkflight", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaruBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            AddControlInWinForm("Use Stone Form", "UseStoneformBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            AddControlInWinForm("Use War Stomp", "UseWarStompBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            /* Artifact Spells */
            AddControlInWinForm("Use Goremaw's Bite", "UseGoremawsBite", "Artifact Spells");
            /* Offensive Spells */
            AddControlInWinForm("Use Backstap", "UseBackstap", "Offensive Spells");
            AddControlInWinForm("Use Death from Above", "UseDeathfromAbove", "Offensive Spells");
            AddControlInWinForm("Use Enveloping Shadows", "UseEnvelopingShadows", "Offensive Spells");
            AddControlInWinForm("Use Eviscerate", "UseEviscerate", "Offensive Spells");
            AddControlInWinForm("Use Gloomblade", "UseGloomblade", "Offensive Spells");
            AddControlInWinForm("Use Nightblade", "UseNightblade", "Offensive Spells");
            AddControlInWinForm("Use Shadowstrike", "UseShadowstrike", "Offensive Spells");
            AddControlInWinForm("Use Shuriken Storm", "UseShurikenStorm", "Offensive Spells");
            /* Offensive Cooldowns */
            AddControlInWinForm("Use Marked for Death", "UseMarkedforDeath", "Offensive Cooldowns");
            AddControlInWinForm("Use Shadow Blades", "UseShadowBlades", "Offensive Cooldowns");
            AddControlInWinForm("Use Shadow Dance", "UseShadowDance", "Offensive Cooldowns");
            AddControlInWinForm("Use Symbols of Death", "UseSymbolsofDeath", "Offensive Cooldowns");
            AddControlInWinForm("Use Vanish", "UseVanish", "Offensive Cooldowns");
            /* Defensive Spells */
            AddControlInWinForm("Use Cloak of Shadows", "UseCloakofShadowsBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Evasios", "UseEvasiosBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Feint", "UseFeintBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Tricks of the Trade", "UseTricksoftheTradeBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            /* Healing Spell */
            AddControlInWinForm("Use Crimson Vial", "UseCrimsonVialBelowPercentage", "Healing Spells", "BelowPercentage", "Life");
            /* Utility Spells */
            //AddControlInWinForm("Use Cheap Shot", "UseCheapShot", "Utility Spells");
            AddControlInWinForm("Use Kidney Shot", "UseKidneyShotAboveComboPoints", "Offensive Spells", "AbovePercentage", "Combo Points");
            AddControlInWinForm("Use Sprint", "UseSprint", "Utility Spells");
            AddControlInWinForm("Use Stealth", "UseStealth", "Utility Spells");
            /* Game Settings */
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");
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

#endregion

// ReSharper restore ObjectCreationAsStatement
// ReSharper restore EmptyGeneralCatchClause