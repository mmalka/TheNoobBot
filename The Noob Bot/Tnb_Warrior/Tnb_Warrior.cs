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
                    #region Warrior Specialisation checking

                case WoWClass.Warrior:

                    if (wowSpecialization == WoWSpecialization.WarriorArms || wowSpecialization == WoWSpecialization.None)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Warrior_Arms.xml";
                            var currentSetting = new WarriorArms.WarriorArmsSettings();
                            if (File.Exists(currentSettingsFile) && !resetSettings)
                            {
                                currentSetting = Settings.Load<WarriorArms.WarriorArmsSettings>(currentSettingsFile);
                            }
                            currentSetting.ToForm();
                            currentSetting.Save(currentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Warrior Arms Combat class...");
                            EquipmentAndStats.SetPlayerSpe(WoWSpecialization.WarriorArms);
                            new WarriorArms();
                        }
                        break;
                    }
                    if (wowSpecialization == WoWSpecialization.WarriorFury)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Warrior_Fury.xml";
                            var currentSetting = new WarriorFury.WarriorFurySettings();
                            if (File.Exists(currentSettingsFile) && !resetSettings)
                            {
                                currentSetting = Settings.Load<WarriorFury.WarriorFurySettings>(currentSettingsFile);
                            }
                            currentSetting.ToForm();
                            currentSetting.Save(currentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Warrior Fury Combat class...");
                            EquipmentAndStats.SetPlayerSpe(WoWSpecialization.WarriorFury);
                            new WarriorFury();
                        }
                        break;
                    }
                    if (wowSpecialization == WoWSpecialization.WarriorProtection)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Warrior_Protection.xml";
                            var currentSetting = new WarriorProtection.WarriorProtectionSettings();
                            if (File.Exists(currentSettingsFile) && !resetSettings)
                            {
                                currentSetting = Settings.Load<WarriorProtection.WarriorProtectionSettings>(currentSettingsFile);
                            }
                            currentSetting.ToForm();
                            currentSetting.Save(currentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Warrior Protection Combat class...");
                            EquipmentAndStats.SetPlayerSpe(WoWSpecialization.WarriorProtection);
                            new WarriorProtection();
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

#region Warrior

public class WarriorArms
{
    private static WarriorArmsSettings MySettings = WarriorArmsSettings.GetSettings();

    #region General Timers & Variables

    private readonly WoWItem _firstTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET);
    private readonly WoWItem _secondTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET, 2);

    private bool CombatMode = true;

    private Timer DefensiveTimer = new Timer(0);
    private Timer StunTimer = new Timer(0);

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

    #region Talents

    private readonly Spell DeadlyCalm = new Spell("Deadly Calm");
    private readonly Spell FervorofBattle = new Spell("Fervor of Battle");

    #endregion

    #region Buffs

    private readonly Spell ShatteredDefensesBuff = new Spell(209706);
    private readonly Spell StoneHeartBuff = new Spell(225947);

    #endregion

    #region Dots

    //private readonly Spell ColossusSmashDot = new Spell(208086);

    #endregion

    #region Artifact Spells

    private readonly Spell Warbreaker = new Spell("Warbreaker");
    private readonly Spell ShatteredDefensesTrait = new Spell(209574);

    #endregion

    #region Offensive Spells

    private readonly Spell Charge = new Spell("Charge"); //No GCD
    private readonly Spell Cleave = new Spell("Cleave");
    private readonly Spell ColossusSmash = new Spell("Colossus Smash");
    private readonly Spell Execute = new Spell("Execute");
    private readonly Spell FocusedRage = new Spell("Focused Rage"); //No GCD //TESTING Does Buffstack work for a Buff which is also a Talent?
    private readonly Spell MortalStrike = new Spell("Mortal Strike");
    private readonly Spell Overpower = new Spell("Overpower");
    private readonly Spell Rend = new Spell("Rend"); //TESTING: Create seperate Dot version of the Spell for checks
    private readonly Spell Shockwave = new Spell("Shockwave");
    private readonly Spell Slam = new Spell("Slam");
    private readonly Spell Whirlwind = new Spell("Whirlwind");

    #endregion

    #region Offensive Cooldowns

    private readonly Spell Avatar = new Spell("Avatar"); //No GCD
    private readonly Spell BattleCry = new Spell("Battle Cry"); //No GCD
    private readonly Spell Bladestorm = new Spell("Bladestorm");
    //private readonly Spell Ravager = new Spell("Ravager");

    #endregion

    #region Defensive Spells

    private readonly Spell CommandingShout = new Spell("Commanding Shout"); //No GCD
    private readonly Spell DefensiveStance = new Spell("Defensive Stance"); //No GCD
    private readonly Spell DiebytheSword = new Spell("Die by the Sword"); //No GCD
    private readonly Spell StormBolt = new Spell("Storm Bolt");

    #endregion

    #region Healing Spells

    private readonly Spell VictoryRush = new Spell("Victory Rush");

    #endregion

    #region Utility Spells

    //private readonly Spell BerserkerRage = new Spell("Berserker Rage"); //No GCD
    //private readonly Spell Hamstring = new Spell("Hamstring"); //No GCD
    //private readonly Spell HeroicLeap = new Spell("Heroic Leap"); //No GCD
    private readonly Spell HeroicThrow = new Spell("Heroic Throw");
    //private readonly Spell IntimidatingShout = new Spell("Intimidating Shout");
    private readonly Spell Pummel = new Spell("Pummel"); //No GCD
    private readonly Spell Taunt = new Spell("Taunt"); //No GCD

    #endregion

    public WarriorArms()
    {
        Main.InternalRange = ObjectManager.Me.GetCombatReach;
        Main.InternalAggroRange = Main.InternalRange;
        Main.InternalLightHealingSpell = null;
        MySettings = WarriorArmsSettings.GetSettings();
        Main.DumpCurrentSettings<WarriorArmsSettings>(MySettings);
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
        if (Defensive() || AggroManagement() || Offensive())
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
            //Victory Rush
            if (MySettings.UseVictoryRush && VictoryRush.IsSpellUsable &&
                VictoryRush.IsHostileDistanceGood && ObjectManager.Me.HealthPercent < 70)
            {
                VictoryRush.Cast();
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

            //Toggle Defensive Stance
            if (((ObjectManager.Me.HealthPercent < MySettings.UseDefensiveStanceBelowPercentage && !DefensiveStance.HaveBuff) ||
                 (ObjectManager.Me.HealthPercent >= MySettings.UseDefensiveStanceBelowPercentage && DefensiveStance.HaveBuff)) &&
                DefensiveStance.IsSpellUsable)
            {
                DefensiveStance.Cast();
            }
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
                    //Storm Bolt
                    if (ObjectManager.Me.HealthPercent < MySettings.UseStormBoltBelowPercentage &&
                        StormBolt.IsSpellUsable && StormBolt.IsHostileDistanceGood)
                    {
                        StormBolt.Cast();
                        StunTimer = new Timer(1000*4);
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
            }
            //Mitigate Damage in Emergency Situations
            //Die by the Sword
            if (ObjectManager.Me.HealthPercent < MySettings.UseDiebytheSwordBelowPercentage && DiebytheSword.IsSpellUsable)
            {
                DiebytheSword.Cast();
                DefensiveTimer = new Timer(1000*8);
                return true;
            }
            //Commanding Shout
            if (ObjectManager.Me.HealthPercent < MySettings.UseCommandingShoutBelowPercentage && CommandingShout.IsSpellUsable)
            {
                CommandingShout.Cast();
                DefensiveTimer = new Timer(1000*8);
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
            //Apply Avatar
            if (MySettings.UseAvatar && Avatar.IsSpellUsable)
            {
                Avatar.Cast();
            }
            //Apply Battle Cry
            if (MySettings.UseBattleCry && BattleCry.IsSpellUsable)
            {
                BattleCry.Cast();
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

            if (!ObjectManager.Target.IsTargetingMe)
                return false;

            //Cast Taunt when you are in a party and the target of your target is a low health player
            if (MySettings.UseTauntBelowToTPercentage > 0 && Taunt.IsSpellUsable &&
                Taunt.IsHostileDistanceGood)
            {
                WoWObject obj = ObjectManager.GetObjectByGuid(ObjectManager.Target.Target);
                if (obj.IsValid && obj.Type == WoWObjectType.Player &&
                    new WoWPlayer(obj.GetBaseAddress).HealthPercent < MySettings.UseTauntBelowToTPercentage)
                {
                    Taunt.Cast();
                    return true;
                }
            }
            //Cast Heroic Throw
            if (MySettings.UseHeroicThrow && HeroicThrow.IsSpellUsable && HeroicThrow.IsHostileDistanceGood)
            {
                HeroicThrow.Cast();
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

            //Special Rotation against 4 or more Targets
            if (ObjectManager.Me.GetUnitInSpellRange(8f) >= 4)
            {
                //Cast Warbreaker
                if (MySettings.UseWarbreaker && Warbreaker.IsSpellUsable)
                {
                    Warbreaker.Cast();
                    return;
                }
                //Cast Bladestorm when
                if (MySettings.UseBladestorm && Bladestorm.IsSpellUsable &&
                    //Colossus Smash Dot is active
                    ColossusSmash.TargetHaveBuffFromMe)
                {
                    Bladestorm.Cast();
                    return;
                }
                //Cast Shockwave when
                if (MySettings.UseShockwave && Shockwave.IsSpellUsable /*&&
                    //it will hit 3 or more Targets (redundant)
                    ObjectManager.Me.GetUnitInSpellRange(10f) >= 3*/)
                {
                    Shockwave.Cast();
                    return;
                }
                //Cast Cleave
                if (MySettings.UseCleave && Cleave.IsSpellUsable && HaveRage(10))
                {
                    Cleave.Cast();
                    return;
                }
                //Cast Whirlwind
                if (MySettings.UseWhirlwind && Whirlwind.IsSpellUsable && HaveRage(25))
                {
                    Whirlwind.Cast();
                    return;
                }
            }
            else
            {
                //Maintain Rend when
                if (MySettings.UseRend && Rend.IsSpellUsable &&
                    HaveRage(15) && Rend.IsHostileDistanceGood &&
                    ObjectManager.Target.AuraTimeLeft(Rend.Id, true) <= 1000*15/3 &&
                    //Colossus Smash Dot is absent
                    !ColossusSmash.TargetHaveBuffFromMe)
                {
                    Rend.Cast();
                    return;
                }
                //Cast Colossus Smash when
                if (MySettings.UseColossusSmash && ColossusSmash.IsSpellUsable && ColossusSmash.IsHostileDistanceGood &&
                    //Colossus Smash Dot is absent and Shattered Defenses Buff is not active.
                    !ColossusSmash.TargetHaveBuffFromMe && !ObjectManager.Me.UnitAura(ShatteredDefensesBuff.Ids, ObjectManager.Me.Guid).IsValid)
                {
                    ColossusSmash.Cast();
                    return;
                }
                //Cast Warbreaker when
                if (MySettings.UseWarbreaker && Warbreaker.IsSpellUsable && Warbreaker.IsHostileDistanceGood &&
                    //Colossus Smash Dot is absent and Shattered Defenses Buff is not active.
                    !ColossusSmash.TargetHaveBuffFromMe && !ObjectManager.Me.UnitAura(ShatteredDefensesBuff.Ids, ObjectManager.Me.Guid).IsValid)
                {
                    Warbreaker.Cast();
                    return;
                }
                //Cast Overpower (talented) when available.
                if (MySettings.UseOverpower && Overpower.IsSpellUsable &&
                    HaveRage(10) && Overpower.IsHostileDistanceGood)
                {
                    Overpower.Cast();
                    return;
                }
                //Cast Mortal Strike when
                if (MySettings.UseMortalStrike && MortalStrike.IsSpellUsable &&
                    HaveRage(20) && MortalStrike.IsHostileDistanceGood &&
                    //you have 3 Focused Rage Stacks
                    FocusedRage.BuffStack >= 3)
                {
                    MortalStrike.Cast();
                    return;
                }
                //Spend all Energy on Execute if possible.
                if (MySettings.UseExecute && Execute.IsSpellUsable && Execute.IsHostileDistanceGood &&
                    (ObjectManager.Target.HealthPercent < 20 || StoneHeartBuff.HaveBuff))
                {
                    if (HaveRage(40))
                    {
                        Execute.Cast();
                        return;
                    }
                }
                else
                {
                    //Cast Mortal Strike
                    if (MySettings.UseMortalStrike && MortalStrike.IsSpellUsable &&
                        HaveRage(20) && MortalStrike.IsHostileDistanceGood)
                    {
                        MortalStrike.Cast();
                        return;
                    }
                    //Cast Focused Rage when
                    if (MySettings.UseFocusedRage && FocusedRage.IsSpellUsable &&
                        HaveRage(15) && FocusedRage.IsHostileDistanceGood &&
                        //you have less than 3 Focused Rage Stacks
                        FocusedRage.BuffStack < 3)
                    {
                        FocusedRage.Cast();
                        return;
                    }
                    //Cast Whirlwind when
                    if (MySettings.UseWhirlwind && Whirlwind.IsSpellUsable &&
                        HaveRage(25) && Whirlwind.IsHostileDistanceGood &&
                        //you have the Fervor of Battle Talent or it hits multiple Targets
                        (FervorofBattle.HaveBuff || ObjectManager.Me.GetUnitInSpellRange(8f) > 1))
                    {
                        Whirlwind.Cast();
                        return;
                    }
                    //Cast Slam when
                    if (MySettings.UseSlam && Slam.IsSpellUsable &&
                        HaveRage(20) && Slam.IsHostileDistanceGood &&
                        //you don't have the Fervor of Battle Talent
                        !FervorofBattle.HaveBuff)
                    {
                        Slam.Cast();
                        return;
                    }
                }
            }

            //Cast Charge
            if (MySettings.UseCharge && Charge.IsSpellUsable && Charge.IsHostileDistanceGood)
            {
                Charge.Cast();
                return;
            }
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    //Check Rage costs
    private bool HaveRage(int cost)
    {
        return ObjectManager.Me.Rage >= cost || (DeadlyCalm.HaveBuff && BattleCry.HaveBuff);
    }

    #region Nested type: WarriorArmsSettings

    [Serializable]
    public class WarriorArmsSettings : Settings
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
        public bool UseWarbreaker = true;

        /* Offensive Spells */
        public bool UseCharge = true;
        public bool UseCleave = true;
        public bool UseColossusSmash = true;
        public bool UseExecute = true;
        public bool UseFocusedRage = true;
        public bool UseMortalStrike = true;
        public bool UseOverpower = true;
        public bool UseRend = true;
        public bool UseShockwave = true;
        public bool UseSlam = true;
        public bool UseWhirlwind = true;

        /* Offensive Cooldowns */
        public bool UseAvatar = true;
        public bool UseBattleCry = true;
        public bool UseBladestorm = true;

        /* Defensive Spells */
        public int UseCommandingShoutBelowPercentage = 50;
        public int UseDefensiveStanceBelowPercentage = 50;
        public int UseDiebytheSwordBelowPercentage = 30;
        public int UseStormBoltBelowPercentage = 50;

        /* Healing Spells */
        public bool UseVictoryRush = true;

        /* Utility Spells */
        public bool UseHeroicThrow = true;
        public int UseTauntBelowToTPercentage = 20;

        /* Game Settings */
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;

        public WarriorArmsSettings()
        {
            ConfigWinForm("Warrior Arms Settings");
            /* Professions & Racials */
            //AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Darkflight", "UseDarkflight", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaruBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            AddControlInWinForm("Use Stone Form", "UseStoneformBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            AddControlInWinForm("Use War Stomp", "UseWarStompBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            /* Artifact Spells */
            AddControlInWinForm("Use Warbreaker", "UseWarbreaker", "Artifact Spells");
            /* Offensive Spells */
            AddControlInWinForm("Use Charge", "UseCharge", "Offensive Spells");
            AddControlInWinForm("Use Cleave", "UseCleave", "Offensive Spells");
            AddControlInWinForm("Use Colossus Smash", "UseColossusSmash", "Offensive Spells");
            AddControlInWinForm("Use Execute", "UseExecute", "Offensive Spells");
            AddControlInWinForm("Use Focused Rage", "UseFocusedRage", "Offensive Spells");
            AddControlInWinForm("Use Mortal Strike", "UseMortalStrike", "Offensive Spells");
            AddControlInWinForm("Use Overpower", "UseOverpower", "Offensive Spells");
            AddControlInWinForm("Use Rend", "UseRend", "Offensive Spells");
            AddControlInWinForm("Use Shockwave", "UseShockwave", "Offensive Spells");
            AddControlInWinForm("Use Slam", "UseSlam", "Offensive Spells");
            AddControlInWinForm("Use Whirlwind", "UseWhirlwind", "Offensive Spells");
            /* Offensive Cooldowns */
            AddControlInWinForm("Use Avatar", "UseAvatar", "Offensive Cooldowns");
            AddControlInWinForm("Use Battle Cry", "UseBattleCry", "Offensive Cooldowns");
            AddControlInWinForm("Use Bladestorm", "UseBladestorm", "Offensive Cooldowns");
            /* Defensive Spells */
            AddControlInWinForm("Use Commanding Shout", "UseCommandingShoutBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Defensive Stance", "UseDefensiveStanceBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Die by the Sword", "UseDiebytheSwordBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Storm Bolt", "UseStormBoltBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            /* Healing Spells */
            AddControlInWinForm("Use Victory Rush", "UseVictoryRush", "Healing Spells");
            /* Utility Spells */
            AddControlInWinForm("Use Heroic Throw", "UseHeroicThrow", "Utility Spells");
            AddControlInWinForm("Use Taunt", "UseTauntBelowToTPercentage", "Utility Spells", "BelowPercentage", "Target of Target Life");
            /* Game Settings */
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");
        }

        public static WarriorArmsSettings CurrentSetting { get; set; }

        public static WarriorArmsSettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Warrior_Arms.xml";
            if (File.Exists(currentSettingsFile))
            {
                return
                    CurrentSetting = Load<WarriorArmsSettings>(currentSettingsFile);
            }
            return new WarriorArmsSettings();
        }
    }

    #endregion
}

public class WarriorProtection
{
    private static WarriorProtectionSettings MySettings = WarriorProtectionSettings.GetSettings();

    #region General Timers & Variables

    private readonly WoWItem _firstTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET);
    private readonly WoWItem _secondTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET, 2);

    private bool CombatMode = true;

    private Timer DefensiveTimer = new Timer(0);
    private Timer StunTimer = new Timer(0);

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

    #region Talents

    private readonly Spell BoomingVoice = new Spell(202743);
    private readonly Spell ImpendingVictory = new Spell(202168);
    private readonly Spell Ultimatum = new Spell(122509);
    private readonly Spell Vengeance = new Spell(202572);

    #endregion

    #region Buffs

    private readonly Spell UltimatumBuff = new Spell(122510);
    private readonly Spell VengeanceFocusedRageBuff = new Spell(202573);
    private readonly Spell VengeanceIgnorePainBuff = new Spell(202574);

    #endregion

    #region Artifact Spells

    private readonly Spell NeltharionsFury = new Spell("Neltharion's Fury");

    #endregion

    #region Offensive Spells

    private readonly Spell Charge = new Spell("Charge"); //No GCD
    private readonly Spell Devastate = new Spell("Devastate");
    private readonly Spell FocusedRage = new Spell("Focused Rage"); //No GCD
    private readonly Spell Revenge = new Spell("Revenge");
    private readonly Spell ShieldSlam = new Spell("Shield Slam");
    private Timer RevengeTimer = new Timer(0);
    private readonly Spell Shockwave = new Spell("Shockwave");
    private readonly Spell ThunderClap = new Spell("Thunder Clap");

    #endregion

    #region Offensive Cooldowns

    private readonly Spell Avatar = new Spell("Avatar"); //No GCD
    private readonly Spell BattleCry = new Spell("Battle Cry"); //No GCD

    #endregion

    #region Defensive Spells

    private readonly Spell DemoralizingShout = new Spell("Demoralizing Shout"); //No GCD
    private readonly Spell IgnorePain = new Spell("Ignore Pain"); //No GCD
    private readonly Spell LastStand = new Spell("Last Stand"); //No GCD
    private readonly Spell ShieldBlock = new Spell("Shield Block"); //No GCD
    private readonly Spell ShieldWall = new Spell("Shield Wall"); //No GCD
    private readonly Spell SpellReflection = new Spell("Spell Reflection"); //No GCD
    private readonly Spell StormBolt = new Spell("Storm Bolt");

    #endregion

    #region Healing Spells

    private readonly Spell VictoryRush = new Spell("Victory Rush");

    #endregion

    #region Utility Spells

    private readonly Spell BerserkerRage = new Spell("Berserker Rage"); //No GCD
    private readonly Spell HeroicLeap = new Spell("Heroic Leap"); //No GCD
    private readonly Spell HeroicThrow = new Spell("Heroic Throw");
    private readonly Spell Intercept = new Spell("Intercept"); //No GCD
    //private readonly Spell PiercingHowl = new Spell("Piercing Howl"); //No GCD
    //private readonly Spell IntimidatingShout = new Spell("Intimidating Shout");
    private readonly Spell Pummel = new Spell("Pummel"); //No GCD
    private readonly Spell Taunt = new Spell("Taunt"); //No GCD

    #endregion

    public WarriorProtection()
    {
        Main.InternalRange = ObjectManager.Me.GetCombatReach;
        Main.InternalAggroRange = Main.InternalRange;
        Main.InternalLightHealingSpell = null;
        MySettings = WarriorProtectionSettings.GetSettings();
        Main.DumpCurrentSettings<WarriorProtectionSettings>(MySettings);
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
        if (Defensive() || AggroManagement() || Offensive())
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
            //Victory Rush / Impending Victory
            if (MySettings.UseVictoryRush_ImpendingVictory && VictoryRush.IsSpellUsable &&
                VictoryRush.IsHostileDistanceGood && (ObjectManager.Me.HealthPercent < 70 ||
                                                      (ImpendingVictory.HaveBuff && ObjectManager.Me.Rage >= 10 &&
                                                       ObjectManager.Me.HealthPercent < 85)))
            {
                VictoryRush.Cast();
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
                    //Storm Bolt
                    if (ObjectManager.Me.HealthPercent < MySettings.UseStormBoltBelowPercentage &&
                        StormBolt.IsSpellUsable && StormBolt.IsHostileDistanceGood)
                    {
                        StormBolt.Cast();
                        StunTimer = new Timer(1000*4);
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
                //Demoralizing Shout
                if (ObjectManager.Me.HealthPercent < MySettings.UseDemoralizingShoutBelowPercentage &&
                    DemoralizingShout.IsSpellUsable && (!BoomingVoice.HaveBuff ||
                                                        (ObjectManager.Me.MaxRage - ObjectManager.Me.Rage) >= 50))
                {
                    DemoralizingShout.Cast();
                    DefensiveTimer = new Timer(1000*8);
                    return true;
                }
            }
            //Emergency Situations
            //Last Stand
            if (ObjectManager.Me.HealthPercent < MySettings.UseLastStandBelowPercentage && LastStand.IsSpellUsable)
            {
                LastStand.Cast();
            }
            //Shield Wall
            if (ObjectManager.Me.HealthPercent < MySettings.UseShieldWallBelowPercentage && ShieldWall.IsSpellUsable)
            {
                ShieldWall.Cast();
                DefensiveTimer = new Timer(1000*8);
                return true;
            }
            //Spell Reflection
            if (ObjectManager.Me.HealthPercent < MySettings.UseSpellReflectionBelowPercentage && SpellReflection.IsSpellUsable)
            {
                SpellReflection.Cast();
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
            //Apply Avatar
            if (MySettings.UseAvatar && Avatar.IsSpellUsable)
            {
                Avatar.Cast();
            }
            //Apply Battle Cry
            if (MySettings.UseBattleCry && BattleCry.IsSpellUsable)
            {
                BattleCry.Cast();
            }
            //Apply Focused Rage when you have a Ultimatum proc
            if (MySettings.UseFocusedRage && FocusedRage.IsSpellUsable &&
                FocusedRage.BuffStack < 3 && UltimatumBuff.HaveBuff)
            {
                FocusedRage.Cast();
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

            if (!ObjectManager.Target.IsTargetingMe)
                return false;

            //Cast Taunt when you are in a party and the target of your target is a low health player
            if (MySettings.UseTauntBelowToTPercentage > 0 && Taunt.IsSpellUsable &&
                Taunt.IsHostileDistanceGood)
            {
                if (Taunt.IsSpellUsable)
                {
                    WoWObject obj = ObjectManager.GetObjectByGuid(ObjectManager.Target.Target);
                    if (obj.IsValid && obj.Type == WoWObjectType.Player &&
                        new WoWPlayer(obj.GetBaseAddress).HealthPercent < MySettings.UseTauntBelowToTPercentage)
                    {
                        Taunt.Cast();
                        return true;
                    }
                }
                else
                {
                    //Cast Heroic Leap
                    if (MySettings.UseHeroicLeap && HeroicLeap.IsSpellUsable && HeroicLeap.IsHostileDistanceGood)
                    {
                        HeroicLeap.CastAtPosition(ObjectManager.Target.Position);
                        return true;
                    }
                }
            }
            //Cast Heroic Throw
            if (MySettings.UseHeroicThrow && HeroicThrow.IsSpellUsable && HeroicThrow.IsHostileDistanceGood)
            {
                HeroicThrow.Cast();
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

            //Neltharion's Fury
            if (ObjectManager.Me.HealthPercent < MySettings.UseNeltharionsFuryBelowPercentage &&
                NeltharionsFury.IsSpellUsable && NeltharionsFury.IsHostileDistanceGood)
            {
                NeltharionsFury.Cast();
                return;
            }
            //Shield Block
            if (ShieldBlock.GetSpellCharges > MySettings.UseShieldBlockAboveCharges &&
                ObjectManager.Me.Rage >= 10 && ShieldBlock.IsSpellUsable &&
                ShieldBlock.IsHostileDistanceGood)
            {
                ShieldBlock.Cast();
                return;
            }
            //Cast Intercept
            if (MySettings.UseIntercept && Intercept.IsSpellUsable &&
                ((ObjectManager.Target.IsHostile && Intercept.IsHostileDistanceGood) ||
                 Intercept.IsFriendDistanceGood))
            {
                Intercept.Cast();
                return;
            }

            //Multiple Target Rotation
            if (MySettings.UseMultipleTargetRotation)
            {
                //Cast Shockwave when
                if (MySettings.UseShockwave && Shockwave.IsSpellUsable &&
                    //it will hit 3 or more Targets
                    ObjectManager.Me.GetUnitInSpellRange(10f) >= 3)
                {
                    Shockwave.Cast();
                    return;
                }
                //Cast Thunder Clap when
                if (MySettings.UseThunderClap && ThunderClap.IsSpellUsable &&
                    //it will hit 3 or more Targets
                    ObjectManager.Me.GetUnitInSpellRange(8f) >= 3)
                {
                    ThunderClap.Cast();
                    return;
                }
                //Cast Revenge
                if (MySettings.UseRevenge && Revenge.IsSpellUsable &&
                    //it will hit multiple Targets
                    ObjectManager.Me.GetUnitInSpellRange(8f) > 1)
                {
                    Revenge.Cast();
                    return;
                }
            }

            //Cast Shield Slam
            if (MySettings.UseShieldSlam && ShieldSlam.IsSpellUsable && ShieldSlam.IsHostileDistanceGood)
            {
                ShieldSlam.Cast();
                RevengeTimer = new Timer(1000*7.5);
                return;
            }
            //Cast Revenge when Shield Slam has under 1.5 seconds left on its cooldown.
            if (MySettings.UseRevenge && Revenge.IsSpellUsable &&
                RevengeTimer.IsReady && Revenge.IsHostileDistanceGood)
            {
                Revenge.Cast();
                return;
            }
            //Cast Devastate
            if (MySettings.UseDevastate && Devastate.IsSpellUsable && Devastate.IsHostileDistanceGood)
            {
                Devastate.Cast();
                return;
            }
            //Apply Focused Rage
            if (ObjectManager.Me.Rage > MySettings.UseFocusedRageAboveRagePercentage &&
                ObjectManager.Me.Rage >= 30 && FocusedRage.IsSpellUsable && FocusedRage.BuffStack < 3 &&
                (!Vengeance.HaveBuff || !VengeanceIgnorePainBuff.HaveBuff))
            {
                FocusedRage.Cast();
                return;
            }
            //Apply Ignore Pain
            if (ObjectManager.Me.Rage > MySettings.UseIgnorePainAboveRagePercentage &&
                ObjectManager.Me.Rage >= 20 && IgnorePain.IsSpellUsable &&
                (!Vengeance.HaveBuff || !VengeanceFocusedRageBuff.HaveBuff))
            {
                IgnorePain.Cast();
                return;
            }

            //Cast Charge
            if (MySettings.UseCharge && Charge.IsSpellUsable && Charge.IsHostileDistanceGood)
            {
                Charge.Cast();
                return;
            }
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    #region Nested type: WarriorProtectionSettings

    [Serializable]
    public class WarriorProtectionSettings : Settings
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
        public int UseNeltharionsFuryBelowPercentage = 90;

        /* Offensive Spells */
        public bool UseCharge = true;
        public bool UseDevastate = true;
        public bool UseFocusedRage = true;
        public bool UseIntercept = true;
        public bool UseRevenge = true;
        public bool UseShieldSlam = true;
        public bool UseShockwave = true;
        public bool UseThunderClap = true;

        /* Offensive Cooldowns */
        public bool UseAvatar = true;
        public bool UseBattleCry = true;

        /* Defensive Spells */
        public int UseFocusedRageAboveRagePercentage = 60;
        public int UseIgnorePainAboveRagePercentage = 60;
        public int UseShieldBlockAboveCharges = 1;
        public int UseStormBoltBelowPercentage = 80;

        /* Defensive Cooldowns */
        public int UseDemoralizingShoutBelowPercentage = 90;
        public int UseLastStandBelowPercentage = 10;
        public int UseShieldWallBelowPercentage = 25;
        public int UseSpellReflectionBelowPercentage = 0;

        /* Healing Spells */
        public bool UseVictoryRush_ImpendingVictory = true;

        /* Utility Spells */
        public bool UseHeroicLeap = true;
        public bool UseHeroicThrow = true;
        public int UseTauntBelowToTPercentage = 20;

        /* Game Settings */
        public bool UseMultipleTargetRotation = true;
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;

        public WarriorProtectionSettings()
        {
            ConfigWinForm("Warrior Protection Settings");
            /* Professions & Racials */
            //AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Darkflight", "UseDarkflight", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaruBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            AddControlInWinForm("Use Stone Form", "UseStoneformBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            AddControlInWinForm("Use War Stomp", "UseWarStompBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            /* Artifact Spells */
            AddControlInWinForm("Use Neltharion's Fury", "UseNeltharionsFuryBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            /* Offensive Spells */
            AddControlInWinForm("Use Charge", "UseCharge", "Offensive Spells");
            AddControlInWinForm("Use Devastate", "UseDevastate", "Offensive Spells");
            AddControlInWinForm("Use Focused Rage", "UseFocusedRage", "Offensive Spells");
            AddControlInWinForm("Use Intercept", "UseIntercept", "Offensive Spells");
            AddControlInWinForm("Use Revenge", "UseRevenge", "Offensive Spells");
            AddControlInWinForm("Use Shield Slam", "UseShieldSlam", "Offensive Spells");
            AddControlInWinForm("Use Shockwave", "UseShockwave", "Offensive Spells");
            AddControlInWinForm("Use Thunder Clap", "UseThunderClap", "Offensive Spells");
            /* Offensive Cooldowns */
            AddControlInWinForm("Use Avatar", "UseAvatar", "Offensive Cooldowns");
            AddControlInWinForm("Use Battle Cry", "UseBattleCry", "Offensive Cooldowns");
            /* Defensive Spells */
            AddControlInWinForm("Use Focused Rage", "UseFocusedRageAboveRagePercentage", "Defensive Spells", "AbovePercentage", "Rage");
            AddControlInWinForm("Use Ignore Pain", "UseIgnorePainAboveRagePercentage", "Defensive Spells", "AbovePercentage", "Rage");
            AddControlInWinForm("Use Shield Block", "UseShieldBlockAboveCharges", "Defensive Spells", "AbovePercentage", "Charges");
            AddControlInWinForm("Use Storm Bolt", "UseStormBoltBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            /* Defensive Cooldowns */
            AddControlInWinForm("Use Demoralizing Shout", "UseDemoralizingShoutBelowPercentage", "Defensive Cooldowns", "BelowPercentage", "Life");
            AddControlInWinForm("Use Last Stand", "UseLastStandBelowPercentage", "Defensive Cooldowns", "BelowPercentage", "Life");
            AddControlInWinForm("Use Shield Wall", "UseShieldWallBelowPercentage", "Defensive Cooldowns", "BelowPercentage", "Life");
            AddControlInWinForm("Use Spell Reflection", "UseSpellReflectionBelowPercentage", "Defensive Cooldowns", "BelowPercentage", "Life");
            /* Healing Spells */
            AddControlInWinForm("Use Victory Rush", "UseVictoryRush", "Healing Spells");
            /* Utility Spells */
            AddControlInWinForm("Use Heroic Leap (only to reset Taunt CD)", "UseHeroicLeap", "Utility Spells");
            AddControlInWinForm("Use Heroic Throw", "UseHeroicThrow", "Utility Spells");
            AddControlInWinForm("Use Taunt", "UseTauntBelowToTPercentage", "Utility Spells", "BelowPercentage", "Target of Target Life");
            /* Game Settings */
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");
        }

        public static WarriorProtectionSettings CurrentSetting { get; set; }

        public static WarriorProtectionSettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Warrior_Protection.xml";
            if (File.Exists(currentSettingsFile))
            {
                return
                    CurrentSetting = Load<WarriorProtectionSettings>(currentSettingsFile);
            }
            return new WarriorProtectionSettings();
        }
    }

    #endregion
}

public class WarriorFury
{
    private static WarriorFurySettings MySettings = WarriorFurySettings.GetSettings();

    #region General Timers & Variables

    private readonly WoWItem _firstTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET);
    private readonly WoWItem _secondTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET, 2);

    private bool CombatMode = true;

    private Timer DefensiveTimer = new Timer(0);
    private Timer StunTimer = new Timer(0);

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

    #region Talents

    private readonly Spell Frenzy = new Spell(206313);
    private readonly Spell InnerRage = new Spell(215573);
    private readonly Spell Massacre = new Spell(206315);
    private readonly Spell Outburst = new Spell(206320);
    private readonly Spell WreckingBall = new Spell(215569);

    #endregion

    #region Buffs

    private readonly Spell EnrageBuff = new Spell(184362);
    private readonly Spell FrenzyBuff = new Spell(202539);
    private readonly Spell MassacreBuff = new Spell(206316);
    private readonly Spell MeatCleaverBuff = new Spell(85739);
    private readonly Spell StoneHeartBuff = new Spell(225947);
    private readonly Spell WreckingBallBuff = new Spell(215570);

    #endregion

    #region Artifact Spells

    private readonly Spell OdynsFury = new Spell("Odyn's Fury");
    private readonly Spell JuggernautTrait = new Spell(200875);
    private readonly Spell JuggernautBuff = new Spell(201009);

    #endregion

    #region Offensive Spells

    private readonly Spell Bloodbath = new Spell("Bloodbath"); //No GCD
    private readonly Spell Bloodthirst = new Spell("Bloodthirst");
    private readonly Spell Charge = new Spell("Charge"); //No GCD
    private readonly Spell DragonRoar = new Spell("Dragon Roar"); //TESTING Testing if HaveBuff does check for Talent or Buff
    private readonly Spell Execute = new Spell("Execute");
    private readonly Spell FuriousSlash = new Spell("Furious Slash");
    private readonly Spell RagingBlow = new Spell("Raging Blow");
    private readonly Spell Rampage = new Spell("Rampage");
    private readonly Spell Shockwave = new Spell("Shockwave");
    private readonly Spell Whirlwind = new Spell("Whirlwind");

    #endregion

    #region Offensive Cooldowns

    private readonly Spell Avatar = new Spell("Avatar"); //No GCD
    private readonly Spell BattleCry = new Spell("Battle Cry"); //No GCD
    private Timer BattleCryHalfCD = new Timer(0);
    private readonly Spell Bladestorm = new Spell("Bladestorm");
    //private readonly Spell Ravager = new Spell("Ravager");

    #endregion

    #region Defensive Spells

    private readonly Spell CommandingShout = new Spell("Commanding Shout"); //No GCD
    private readonly Spell EnragedRegeneration = new Spell("Enraged Regeneration"); //No GCD
    private readonly Spell StormBolt = new Spell("Storm Bolt");

    #endregion

    #region Healing Spells

    private readonly Spell VictoryRush = new Spell("Victory Rush");

    #endregion

    #region Utility Spells

    private readonly Spell BerserkerRage = new Spell("Berserker Rage"); //No GCD
    //&private readonly Spell HeroicLeap = new Spell("Heroic Leap"); //No GCD
    private readonly Spell HeroicThrow = new Spell("Heroic Throw");
    //private readonly Spell PiercingHowl = new Spell("Piercing Howl"); //No GCD
    //private readonly Spell IntimidatingShout = new Spell("Intimidating Shout");
    private readonly Spell Pummel = new Spell("Pummel"); //No GCD
    private readonly Spell Taunt = new Spell("Taunt"); //No GCD

    #endregion

    public WarriorFury()
    {
        Main.InternalRange = ObjectManager.Me.GetCombatReach;
        Main.InternalAggroRange = Main.InternalRange;
        Main.InternalLightHealingSpell = null;
        MySettings = WarriorFurySettings.GetSettings();
        Main.DumpCurrentSettings<WarriorFurySettings>(MySettings);
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
        if (Defensive() || AggroManagement() || Offensive())
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
            //Victory Rush
            if (MySettings.UseVictoryRush && VictoryRush.IsSpellUsable &&
                VictoryRush.IsHostileDistanceGood && ObjectManager.Me.HealthPercent < 70)
            {
                VictoryRush.Cast();
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
                    //Storm Bolt
                    if (ObjectManager.Me.HealthPercent < MySettings.UseStormBoltBelowPercentage &&
                        StormBolt.IsSpellUsable && StormBolt.IsHostileDistanceGood)
                    {
                        StormBolt.Cast();
                        StunTimer = new Timer(1000*4);
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
            }
            //Mitigate Damage in Emergency Situations
            //Enraged Regeneration
            if (ObjectManager.Me.HealthPercent < MySettings.UseEnragedRegenerationBelowPercentage && EnragedRegeneration.IsSpellUsable)
            {
                EnragedRegeneration.Cast();
                DefensiveTimer = new Timer(1000*8);
                return true;
            }
            //Commanding Shout
            if (ObjectManager.Me.HealthPercent < MySettings.UseCommandingShoutBelowPercentage && CommandingShout.IsSpellUsable)
            {
                CommandingShout.Cast();
                DefensiveTimer = new Timer(1000*8);
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
            //Apply Battle Cry but
            if (MySettings.UseBattleCry && BattleCry.IsSpellUsable &&
                //wait for Bloodbath if it's talented
                (!Bloodbath.KnownSpell || (MySettings.UseBloodbath && Bloodbath.IsSpellUsable)))
            {
                //Apply Dragon Roar
                if (MySettings.UseDragonRoar && DragonRoar.IsSpellUsable && DragonRoar.IsHostileDistanceGood)
                {
                    DragonRoar.Cast();
                    return true;
                }
                //Apply Bloodbath
                if (MySettings.UseBloodbath && Bloodbath.IsSpellUsable)
                    Bloodbath.Cast();
                //Apply Avatar
                if (MySettings.UseAvatar && Avatar.IsSpellUsable)
                    Avatar.Cast();
                //Apply Enrage with Outburst
                if (MySettings.UseOutburst && Outburst.HaveBuff &&
                    BerserkerRage.IsSpellUsable && !EnrageBuff.HaveBuff)
                    BerserkerRage.Cast();

                BattleCry.Cast();
                BattleCryHalfCD = new Timer(1000*30);
            }
            //Apply Bloodbath between Battle Cries
            if (MySettings.UseBloodbath && Bloodbath.IsSpellUsable && BattleCryHalfCD.IsReady)
            {
                Bloodbath.Cast();
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

            if (!ObjectManager.Target.IsTargetingMe)
                return false;

            //Cast Taunt when you are in a party and the target of your target is a low health player
            if (MySettings.UseTauntBelowToTPercentage > 0 && Taunt.IsSpellUsable &&
                Taunt.IsHostileDistanceGood)
            {
                WoWObject obj = ObjectManager.GetObjectByGuid(ObjectManager.Target.Target);
                if (obj.IsValid && obj.Type == WoWObjectType.Player &&
                    new WoWPlayer(obj.GetBaseAddress).HealthPercent < MySettings.UseTauntBelowToTPercentage)
                {
                    Taunt.Cast();
                    return true;
                }
            }
            //Cast Heroic Throw
            if (MySettings.UseHeroicThrow && HeroicThrow.IsSpellUsable && HeroicThrow.IsHostileDistanceGood)
            {
                HeroicThrow.Cast();
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

            //Massacre Rotation
            if (Massacre.HaveBuff && (ObjectManager.Target.HealthPercent < 20 || StoneHeartBuff.HaveBuff))
            {
                //Cast Execute without Enrage, and without Massacre procs.
                if (MySettings.UseExecute && Execute.IsSpellUsable && Execute.IsHostileDistanceGood &&
                    ObjectManager.Me.Rage >= 25 && !EnrageBuff.HaveBuff && !MassacreBuff.HaveBuff)
                {
                    Execute.Cast();
                    return;
                }
                //Cast Execute with Massacre procs.
                if (MySettings.UseRampage && Rampage.IsSpellUsable && Rampage.IsHostileDistanceGood &&
                    MassacreBuff.HaveBuff)
                {
                    Rampage.Cast();
                    return;
                }
                //Cast Raging Blow with Inner Rage taken.
                if (MySettings.UseRagingBlow && RagingBlow.IsSpellUsable && RagingBlow.IsHostileDistanceGood &&
                    InnerRage.HaveBuff)
                {
                    RagingBlow.Cast();
                    return;
                }
                //Cast Execute 
                if (MySettings.UseExecute && Execute.IsSpellUsable && Execute.IsHostileDistanceGood &&
                    ObjectManager.Me.Rage >= 25)
                {
                    Execute.Cast();
                    return;
                }
                //Cast Bloodthirst 
                if (MySettings.UseBloodthirst && Bloodthirst.IsSpellUsable && Bloodthirst.IsHostileDistanceGood)
                {
                    Bloodthirst.Cast();
                    return;
                }
            }

            //Cast Bladestorm when
            if (MySettings.UseBladestorm && Bladestorm.IsSpellUsable &&
                //it will hit multiple Targets
                ObjectManager.Me.GetUnitInSpellRange(8f) > 1)
            {
                Bladestorm.Cast();
                return;
            }
            //Cast Shockwave when
            if (MySettings.UseShockwave && Shockwave.IsSpellUsable &&
                //it will hit 3 or more Targets (redundant)
                ObjectManager.Me.GetUnitInSpellRange(10f) >= 3)
            {
                Shockwave.Cast();
                return;
            }
            //Apply Meat Cleaver Buff when
            if (MySettings.UseWhirlwind && Whirlwind.IsSpellUsable &&
                //you have multiple Targets
                ObjectManager.Me.GetUnitInSpellRange(8f) > 1 &&
                !MeatCleaverBuff.HaveBuff)
            {
                Whirlwind.Cast();
                return;
            }
            //Cast Rampage when
            if (MySettings.UseRampage && Rampage.IsSpellUsable && Rampage.IsHostileDistanceGood &&
                //Enrage Buff is absent or you have 100 Rage.
                (!EnrageBuff.HaveBuff || ObjectManager.Me.Rage >= 100))
            {
                //Apply Dragon Roar
                if (MySettings.UseDragonRoar && DragonRoar.IsSpellUsable && DragonRoar.IsHostileDistanceGood)
                {
                    DragonRoar.Cast();
                    return;
                }
                Rampage.Cast();
                return;
            }
            //Cast Bloodthirst when
            if (MySettings.UseBloodthirst && Bloodthirst.IsSpellUsable && Bloodthirst.IsHostileDistanceGood &&
                //Enrage Buff is absent.
                !EnrageBuff.HaveBuff)
            {
                Bloodthirst.Cast();
                return;
            }
            //Cast Odyn's Fury when
            if (MySettings.UseOdynsFury && OdynsFury.IsSpellUsable && OdynsFury.IsHostileDistanceGood &&
                //you have the Battle Cry or Avatar Buff.
                (BattleCry.HaveBuff || Avatar.HaveBuff))
            {
                OdynsFury.Cast();
                return;
            }
            //Maintain Frenzy
            if (MySettings.UseFuriousSlash && FuriousSlash.IsSpellUsable &&
                FuriousSlash.IsHostileDistanceGood && Frenzy.HaveBuff &&
                (FrenzyBuff.BuffStack < 3 || ObjectManager.Me.UnitAura(FrenzyBuff.Id).AuraTimeLeftInMs < 1000*10/3))
            {
                FuriousSlash.Cast();
                return;
            }
            //Cast Execute if possible.
            if (MySettings.UseExecute && Execute.IsSpellUsable && Execute.IsHostileDistanceGood &&
                ObjectManager.Me.Rage >= 25 && EnrageBuff.HaveBuff &&
                (ObjectManager.Target.HealthPercent < 20 || StoneHeartBuff.HaveBuff))
            {
                Execute.Cast();
                return;
            }
            //Cast Whirlwind when
            if (MySettings.UseWhirlwind && Whirlwind.IsSpellUsable && Whirlwind.IsHostileDistanceGood &&
                //you have the Wrecking Ball Buff or multiple Targets
                (!WreckingBallBuff.HaveBuff || ObjectManager.Me.GetUnitInSpellRange(8f) > 1))
            {
                Whirlwind.Cast();
                return;
            }
            //Cast Raging Blow
            if (MySettings.UseRagingBlow && RagingBlow.IsSpellUsable && RagingBlow.IsHostileDistanceGood &&
                (EnrageBuff.HaveBuff || InnerRage.HaveBuff))
            {
                RagingBlow.Cast();
                return;
            }
            //Cast Bloodthirst
            if (MySettings.UseBloodthirst && Bloodthirst.IsSpellUsable && Bloodthirst.IsHostileDistanceGood)
            {
                Bloodthirst.Cast();
                return;
            }
            //Cast Furious Slash
            if (MySettings.UseFuriousSlash && FuriousSlash.IsSpellUsable && FuriousSlash.IsHostileDistanceGood)
            {
                FuriousSlash.Cast();
                return;
            }

            //Cast Charge
            if (MySettings.UseCharge && Charge.IsSpellUsable && Charge.IsHostileDistanceGood)
            {
                Charge.Cast();
                return;
            }
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    #region Nested type: WarriorFurySettings

    [Serializable]
    public class WarriorFurySettings : Settings
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
        public bool UseOdynsFury = true;

        /* Offensive Spells */
        public bool UseBloodbath = true;
        public bool UseBloodthirst = true;
        public bool UseCharge = true;
        public bool UseDragonRoar = true;
        public bool UseExecute = true;
        public bool UseFuriousSlash = true;
        public bool UseRagingBlow = true;
        public bool UseRampage = true;
        public bool UseOutburst = true;
        public bool UseShockwave = true;
        public bool UseWhirlwind = true;

        /* Offensive Cooldowns */
        public bool UseAvatar = true;
        public bool UseBattleCry = true;
        public bool UseBladestorm = true;

        /* Defensive Spells */
        public int UseCommandingShoutBelowPercentage = 50;
        public int UseEnragedRegenerationBelowPercentage = 30;
        public int UseStormBoltBelowPercentage = 50;

        /* Healing Spells */
        public bool UseVictoryRush = true;

        /* Utility Spells */
        public bool UseHeroicThrow = true;
        public int UseTauntBelowToTPercentage = 20;

        /* Game Settings */
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;

        public WarriorFurySettings()
        {
            ConfigWinForm("Warrior Fury Settings");
            /* Professions & Racials */
            //AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Darkflight", "UseDarkflight", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaruBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            AddControlInWinForm("Use Stone Form", "UseStoneformBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            AddControlInWinForm("Use War Stomp", "UseWarStompBelowPercentage", "Professions & Racials", "BelowPercentage", "Life");
            /* Artifact Spells */
            AddControlInWinForm("Use Odyn's Fury", "UseOdynsFury", "Artifact Spells");
            /* Offensive Spells */
            AddControlInWinForm("Use Bloodbath", "UseBloodbath", "Offensive Spells");
            AddControlInWinForm("Use Bloodthirst", "UseBloodthirst", "Offensive Spells");
            AddControlInWinForm("Use Charge", "UseCharge", "Offensive Spells");
            AddControlInWinForm("Use Dragon Roar", "UseDragonRoar", "Offensive Spells");
            AddControlInWinForm("Use Execute", "UseExecute", "Offensive Spells");
            AddControlInWinForm("Use Furious Slash", "UseFuriousSlash", "Offensive Spells");
            AddControlInWinForm("Use Raging Blow", "UseRagingBlow", "Offensive Spells");
            AddControlInWinForm("Use Rampage", "UseRampage", "Offensive Spells");
            AddControlInWinForm("Use Outburst (Berserker Rage)", "UseOutburst", "Offensive Spells");
            AddControlInWinForm("Use Shockwave", "UseShockwave", "Offensive Spells");
            AddControlInWinForm("Use Whirlwind", "UseWhirlwind", "Offensive Spells");
            /* Offensive Cooldowns */
            AddControlInWinForm("Use Avatar", "UseAvatar", "Offensive Cooldowns");
            AddControlInWinForm("Use Battle Cry", "UseBattleCry", "Offensive Cooldowns");
            AddControlInWinForm("Use Bladestorm", "UseBladestorm", "Offensive Cooldowns");
            /* Defensive Spells */
            AddControlInWinForm("Use Commanding Shout", "UseCommandingShoutBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Enraged Regeneration", "UseEnragedRegenerationBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            AddControlInWinForm("Use Storm Bolt", "UseStormBoltBelowPercentage", "Defensive Spells", "BelowPercentage", "Life");
            /* Healing Spells */
            AddControlInWinForm("Use Victory Rush", "UseVictoryRush", "Healing Spells");
            /* Utility Spells */
            AddControlInWinForm("Use Heroic Throw", "UseHeroicThrow", "Utility Spells");
            AddControlInWinForm("Use Taunt", "UseTauntBelowToTPercentage", "Utility Spells", "BelowPercentage", "Target of Target Life");
            /* Game Settings */
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");
        }

        public static WarriorFurySettings CurrentSetting { get; set; }

        public static WarriorFurySettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Warrior_Fury.xml";
            if (File.Exists(currentSettingsFile))
            {
                return
                    CurrentSetting = Load<WarriorFurySettings>(currentSettingsFile);
            }
            return new WarriorFurySettings();
        }
    }

    #endregion
}

#endregion

// ReSharper restore ObjectCreationAsStatement
// ReSharper restore EmptyGeneralCatchClause