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
                    #region Warlock Specialisation checking

                case WoWClass.Warlock:

                    if (wowSpecialization == WoWSpecialization.WarlockDemonology)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Warlock_Demonology.xml";
                            var currentSetting = new WarlockDemonology.WarlockDemonologySettings();
                            if (File.Exists(currentSettingsFile) && !resetSettings)
                            {
                                currentSetting = Settings.Load<WarlockDemonology.WarlockDemonologySettings>(currentSettingsFile);
                            }
                            currentSetting.ToForm();
                            currentSetting.Save(currentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Warlock Demonology Combat class...");
                            InternalRange = 40.0f;
                            EquipmentAndStats.SetPlayerSpe(WoWSpecialization.WarlockDemonology);
                            new WarlockDemonology();
                        }
                        break;
                    }
                    if (wowSpecialization == WoWSpecialization.WarlockAffliction || wowSpecialization == WoWSpecialization.None)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Warlock_Affliction.xml";
                            var currentSetting = new WarlockAffliction.WarlockAfflictionSettings();
                            if (File.Exists(currentSettingsFile) && !resetSettings)
                            {
                                currentSetting = Settings.Load<WarlockAffliction.WarlockAfflictionSettings>(currentSettingsFile);
                            }
                            currentSetting.ToForm();
                            currentSetting.Save(currentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Warlock Affliction Combat class...");
                            InternalRange = 40.0f;
                            EquipmentAndStats.SetPlayerSpe(WoWSpecialization.WarlockAffliction);
                            new WarlockAffliction();
                        }
                        break;
                    }
                    if (wowSpecialization == WoWSpecialization.WarlockDestruction)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Warlock_Destruction.xml";
                            var currentSetting = new WarlockDestruction.WarlockDestructionSettings();
                            if (File.Exists(currentSettingsFile) && !resetSettings)
                            {
                                currentSetting = Settings.Load<WarlockDestruction.WarlockDestructionSettings>(currentSettingsFile);
                            }
                            currentSetting.ToForm();
                            currentSetting.Save(currentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Warlock Destruction Combat class...");
                            InternalRange = 40.0f;
                            EquipmentAndStats.SetPlayerSpe(WoWSpecialization.WarlockDestruction);
                            new WarlockDestruction();
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

#region Warlock

public class WarlockDemonology
{
    private static WarlockDemonologySettings MySettings = WarlockDemonologySettings.GetSettings();

    #region General Timers & Variables

    private readonly WoWItem _firstTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET);
    private readonly WoWItem _secondTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET, 2);
    public int DecastHP = 0;
    public int DefenseHP = 0;
    public int HealHP = 0;
    public int HealMP = 0;
    public int LC = 0;

    private Timer _onCd = new Timer(0);

    #endregion

    #region Professions & Racials

    public readonly Spell ArcaneTorrent = new Spell("Arcane Torrent");
    public readonly Spell Berserking = new Spell("Berserking");
    public readonly Spell BloodFury = new Spell("Blood Fury");
    public readonly Spell Darkflight = new Spell("Darkflight");
    public readonly Spell EveryManforHimself = new Spell("Every Man for Himself");
    public readonly Spell GiftoftheNaaru = new Spell("Gift of the Naaru");
    public readonly Spell Stoneform = new Spell("Stoneform");
    public readonly Spell WarStomp = new Spell("War Stomp");

    #endregion

    #region Warlock Buffs

    public readonly Spell CorruptionDebuff = new Spell(146739);
    public readonly Spell BurningRush = new Spell("Burning Rush");
    public readonly Spell DarkIntent = new Spell("Dark Intent");
    public readonly Spell GrimoireofSupremacy = new Spell("Grimoire of Supremacy");
    public readonly Spell KilJaedensCunning = new Spell("Kil'Jaeden's Cunning");
    public readonly Spell Metamorphosis = new Spell("Metamorphosis");
    public readonly Spell MoltenCore = new Spell("Molten Core");
    public readonly Spell Soulstone = new Spell("Soulstone");
    public readonly Spell UnendingBreath = new Spell("Unending Breath");

    #endregion

    #region Offensive Spell

    public readonly Spell ChaosWave = new Spell(124916);
    public readonly Spell CommandDemon = new Spell("Command Demon");
    public readonly Spell Corruption = new Spell("Corruption");
    public readonly Spell DemonicServitude = new Spell("Demonic Servitude");
    public readonly Spell Demonbolt = new Spell("Demonbolt");
    public readonly Spell Doom = new Spell(603);
    public readonly Spell HandofGuldan = new Spell("Hand of Gul'dan");
    public readonly Spell Hellfire = new Spell("Hellfire");
    public readonly Spell ImmolationAura = new Spell(104025);
    public readonly Spell ShadowBolt = new Spell("Shadow Bolt");
    public readonly Spell SoulFire = new Spell("Soul Fire");
    public readonly Spell SummonFelguard = new Spell("Summon Felguard");
    public readonly Spell SummonFelhunter = new Spell("Summon Felhunter");
    public readonly Spell SummonFelImp = new Spell("Summon Fel Imp");
    public readonly Spell SummonImp = new Spell("Summon Imp");
    public readonly Spell SummonObserver = new Spell("Summon Observer");
    public readonly Spell SummonShivarra = new Spell("Summon Shivarra");
    public readonly Spell SummonSuccubus = new Spell("Summon Succubus");
    public readonly Spell SummonWrathguard = new Spell("Summon Wrathguard");
    public readonly Spell SummonVoidlord = new Spell("Summon Voidlord");
    public readonly Spell SummonVoidwalker = new Spell("Summon Voidwalker");
    public readonly Spell TouchofChaos = new Spell(103964);

    #endregion

    #region Offensive Cooldown

    public readonly Spell Cataclysm = new Spell("Cataclysm");
    public readonly Spell DarkSoulKnowledge = new Spell("Dark Soul: Knowledge");
    public readonly Spell GrimoireofService = new Spell("Grimoire of Service");
    public readonly Spell MannarothsFury = new Spell("Mannaroth's Fury");
    public readonly Spell SummonAbyssal = new Spell("Summon Abyssal");
    public readonly Spell SummonDoomguard = new Spell("Summon Doomguard");
    public readonly Spell SummonInfernal = new Spell("Summon Infernal");
    public readonly Spell SummonTerrorguard = new Spell(112927);

    #endregion

    #region Defensive Cooldown

    public readonly Spell BloodHorror = new Spell("Blood Horror");
    public readonly Spell DarkBargain = new Spell("Dark Bargain");
    public readonly Spell Fear = new Spell("Fear");
    public readonly Spell HowlofTerror = new Spell("HowlofTerror");
    public readonly Spell SacrificialPact = new Spell("Sacrificial Pact");
    public readonly Spell Shadowfury = new Spell("Shadowfury");
    public readonly Spell UnendingResolve = new Spell("Unending Resolve");
    private Timer _fearTimer = new Timer(0);

    #endregion

    #region Healing Spell

    public readonly Spell CreateHealthstone = new Spell("Create Healthstone");
    public readonly Spell DarkRegeneration = new Spell("Dark Regeneration");
    public readonly Spell DrainLife = new Spell("Drain Life");
    public readonly Spell HealthFunnel = new Spell("Health Funnel");
    public readonly Spell LifeTap = new Spell("Life Tap");
    public readonly Spell MortalCoil = new Spell("Mortal Coil");
    private Timer _healthstoneTimer = new Timer(0);

    #endregion

    public WarlockDemonology()
    {
        Main.InternalRange = 39f;
        Main.InternalAggroRange = 39f;
        MySettings = WarlockDemonologySettings.GetSettings();
        Main.DumpCurrentSettings<WarlockDemonologySettings>(MySettings);
        UInt128 lastTarget = 0;
        LowHP();

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
                                && (SoulFire.IsHostileDistanceGood || ShadowBolt.IsHostileDistanceGood))
                            {
                                Pull();
                                lastTarget = ObjectManager.Me.Target;
                            }

                            if (MySettings.UseLowCombat && (ObjectManager.Me.Level - ObjectManager.Target.Level >= MySettings.UseLowCombatAtPercentage))
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

    private void LowHP()
    {
        if (MySettings.UseBloodHorrorAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseBloodHorrorAtPercentage;

        if (MySettings.UseDarkBargainAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseDarkBargainAtPercentage;

        if (MySettings.UseFearAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseFearAtPercentage;

        if (MySettings.UseHowlofTerrorAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseHowlofTerrorAtPercentage;

        if (MySettings.UseSacrificialPactAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseSacrificialPactAtPercentage;

        if (MySettings.UseShadowfuryAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseShadowfuryAtPercentage;

        if (MySettings.UseStoneformAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseStoneformAtPercentage;

        if (MySettings.UseUnendingResolveAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseUnendingResolveAtPercentage;

        if (MySettings.UseWarStompAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseWarStompAtPercentage;

        if (MySettings.UseArcaneTorrentForDecastAtPercentage > DecastHP)
            DecastHP = MySettings.UseArcaneTorrentForDecastAtPercentage;

        if (MySettings.UseArcaneTorrentForResourceAtPercentage > HealMP)
            HealMP = MySettings.UseArcaneTorrentForResourceAtPercentage;

        if (MySettings.UseLifeTapAtPercentage > HealMP)
            HealMP = MySettings.UseLifeTapAtPercentage;

        if (MySettings.UseCreateHealthstoneAtPercentage > HealHP)
            HealHP = MySettings.UseCreateHealthstoneAtPercentage;

        if (MySettings.UseDarkRegenerationAtPercentage > HealHP)
            HealHP = MySettings.UseDarkRegenerationAtPercentage;

        if (MySettings.UseDrainLifeAtPercentage > HealHP)
            HealHP = MySettings.UseDrainLifeAtPercentage;

        if (MySettings.UseGiftoftheNaaruAtPercentage > HealHP)
            HealHP = MySettings.UseGiftoftheNaaruAtPercentage;

        if (MySettings.UseMortalCoilAtPercentage > HealHP)
            HealHP = MySettings.UseMortalCoilAtPercentage;
    }

    private void Pull()
    {
        if (ObjectManager.Pet.IsAlive)
        {
            Lua.RunMacroText("/petattack");
            Logging.WriteFight("Cast Pet Attack");
        }
        if (MySettings.UseSoulFire && SoulFire.IsSpellUsable && SoulFire.IsHostileDistanceGood)
        {
            SoulFire.Cast();
            return;
        }

        if (MySettings.UseShadowBolt && ShadowBolt.IsSpellUsable && ShadowBolt.IsHostileDistanceGood)
            ShadowBolt.Cast();
    }

    private void LowCombat()
    {
        Buff();

        if (MySettings.DoAvoidMelee)
            AvoidMelee();

        if (_onCd.IsReady && ObjectManager.Me.HealthPercent <= DefenseHP)
            DefenseCycle();

        if (ObjectManager.Me.HealthPercent <= HealHP || ObjectManager.Me.ManaPercentage <= HealMP)
            Heal();

        if (MySettings.UseHandofGuldan && !HandofGuldan.TargetHaveBuff && HandofGuldan.GetSpellCharges > 0 && HandofGuldan.IsSpellUsable && HandofGuldan.IsHostileDistanceGood)
        {
            HandofGuldan.Cast();
            return;
        }
        if (MySettings.UseCommandDemon && CommandDemon.IsSpellUsable && ShadowBolt.IsHostileDistanceGood)
        {
            CommandDemon.Cast();
            return;
        }
        if (MySettings.UseHellfire && Hellfire.IsSpellUsable && ObjectManager.Me.HealthPercent > MySettings.UseHellfireAbovePercentage
            && ObjectManager.GetUnitInSpellRange(Hellfire.MaxRangeHostile) > 4)
        {
            Hellfire.Cast();
            Others.SafeSleep(200);
            while (ObjectManager.Me.IsCast && ObjectManager.GetUnitInSpellRange(Hellfire.MaxRangeHostile) > 4 && ObjectManager.Me.HealthPercent > MySettings.UseHellfireAbovePercentage)
                Others.SafeSleep(200);
            return;
        }
        if (MySettings.UseShadowBolt && ShadowBolt.IsSpellUsable && ShadowBolt.IsHostileDistanceGood)
        {
            ShadowBolt.Cast();
            return;
        }
    }

    private void Combat()
    {
        Buff();

        if (MySettings.DoAvoidMelee)
            AvoidMelee();

        DPSCycle();

        if (_onCd.IsReady && ObjectManager.Me.HealthPercent <= DefenseHP)
            DefenseCycle();

        if (ObjectManager.Me.HealthPercent <= HealHP || ObjectManager.Me.ManaPercentage <= HealMP)
            Heal();

        if (ObjectManager.Me.HealthPercent <= DecastHP)
            Decast();

        DPSBurst();
        DPSCycle();
    }

    private void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        Pet();

        if (MySettings.UseBurningRush && !Darkflight.HaveBuff && !BurningRush.HaveBuff && ObjectManager.Me.GetMove && !ObjectManager.Target.InCombat && BurningRush.IsSpellUsable)
            BurningRush.Cast();

        if (MySettings.UseBurningRush && BurningRush.HaveBuff && (!ObjectManager.Me.GetMove || ObjectManager.Target.InCombat
                                                                  || ObjectManager.Me.HealthPercent < MySettings.UseBurningRushAbovePercentage) && BurningRush.IsSpellUsable)
            BurningRush.Cast();

        if (MySettings.UseCreateHealthstone && ItemsManager.GetItemCount(5512) == 0 && Usefuls.GetContainerNumFreeSlots > 0 && CreateHealthstone.IsSpellUsable)
        {
            Logging.WriteFight(" - Create Healthstone - ");
            CreateHealthstone.Cast();
            Others.SafeSleep(500);

            while (ObjectManager.Me.IsCast)
                Others.SafeSleep(200);
        }

        if (MySettings.UseDarkflight && Darkflight.KnownSpell && !BurningRush.HaveBuff && ObjectManager.Me.GetMove && !ObjectManager.Target.InCombat && Darkflight.IsSpellUsable)
            Darkflight.Cast();

        if (MySettings.UseDarkIntent && !DarkIntent.HaveBuff && DarkIntent.IsSpellUsable)
            DarkIntent.Cast();

        if (MySettings.UseSoulstone && !Soulstone.HaveBuff && Soulstone.IsSpellUsable)
        {
            Soulstone.Cast();
            Others.SafeSleep(500);

            while (ObjectManager.Me.IsCast)
                Others.SafeSleep(20);
        }

        if (MySettings.UseUnendingBreath && ObjectManager.Me.UnitAura(5697).AuraTimeLeftInMs < 1 && UnendingBreath.IsSpellUsable)
            UnendingBreath.Cast();

        if (MySettings.UseAlchFlask && !ObjectManager.Me.HaveBuff(79638) && !ObjectManager.Me.HaveBuff(79640) && !ObjectManager.Me.HaveBuff(79639)
            && !ItemsManager.IsItemOnCooldown(75525) && ItemsManager.GetItemCount(75525) > 0)
            ItemsManager.UseItem(75525);
    }

    private void Pet()
    {
        if (MySettings.UseHealthFunnel && ObjectManager.Pet.HealthPercent > 0 && HealthFunnel.IsSpellUsable
            && ObjectManager.Pet.HealthPercent <= MySettings.UseHealthFunnelAtPercentage)
        {
            HealthFunnel.Launch(true, false, true);
            Others.SafeSleep(500);

            while (ObjectManager.Me.IsCast && (ObjectManager.Pet.HealthPercent > 81 || ObjectManager.Pet.IsDead))
                Others.SafeSleep(20);

            if (ObjectManager.Me.IsCast)
                ObjectManager.Me.StopCast();
            return;
        }

        if (MySettings.UseSummonDoomguard && DemonicServitude.KnownSpell && (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0)
            && (SummonDoomguard.IsSpellUsable || SummonTerrorguard.IsSpellUsable))
        {
            Logging.WriteFight(" - PET DEAD - ");
            if (GrimoireofSupremacy.KnownSpell)
                SummonTerrorguard.Cast();
            else
                SummonDoomguard.Cast();
            Others.SafeSleep(500);

            while (ObjectManager.Me.IsCast)
                Others.SafeSleep(20);
        }
        else if (MySettings.UseSummonInfernal && DemonicServitude.KnownSpell && (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0)
                 && (SummonInfernal.IsSpellUsable || SummonAbyssal.IsSpellUsable))
        {
            Logging.WriteFight(" - PET DEAD - ");
            if (GrimoireofSupremacy.KnownSpell)
                SpellManager.CastSpellByIDAndPosition(140763, ObjectManager.Target.Position);
            else
                SpellManager.CastSpellByIDAndPosition(1122, ObjectManager.Target.Position);
            Others.SafeSleep(500);

            while (ObjectManager.Me.IsCast)
                Others.SafeSleep(20);
        }
        else if (MySettings.UseSummonFelguard && (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0) && (SummonFelguard.IsSpellUsable || SummonWrathguard.IsSpellUsable))
        {
            Logging.WriteFight(" - PET DEAD - ");
            if (GrimoireofSupremacy.KnownSpell)
                SummonWrathguard.Cast();
            else
                SummonFelguard.Cast();
            Others.SafeSleep(500);

            while (ObjectManager.Me.IsCast)
                Others.SafeSleep(20);
        }
        else if (MySettings.UseSummonFelhunter && (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0) && (SummonFelhunter.IsSpellUsable || SummonObserver.IsSpellUsable))
        {
            Logging.WriteFight(" - PET DEAD - ");
            if (GrimoireofSupremacy.KnownSpell)
                SummonObserver.Cast();
            else
                SummonFelhunter.Cast();
            Others.SafeSleep(500);

            while (ObjectManager.Me.IsCast)
                Others.SafeSleep(20);
        }
        else if (MySettings.UseSummonImp && (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0) && (SummonImp.IsSpellUsable || SummonFelImp.IsSpellUsable))
        {
            Logging.WriteFight(" - PET DEAD - ");
            if (GrimoireofSupremacy.KnownSpell)
                SummonFelImp.Cast();
            else
                SummonImp.Cast();
            Others.SafeSleep(500);

            while (ObjectManager.Me.IsCast)
                Others.SafeSleep(20);
        }
        else if (MySettings.UseSummonVoidwalker && (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0) && (SummonVoidwalker.IsSpellUsable || SummonVoidlord.IsSpellUsable))
        {
            Logging.WriteFight(" - PET DEAD - ");
            if (GrimoireofSupremacy.KnownSpell)
                SummonVoidlord.Cast();
            else
                SummonVoidwalker.Cast();
            Others.SafeSleep(500);

            while (ObjectManager.Me.IsCast)
                Others.SafeSleep(20);
        }
        else if (MySettings.UseSummonSuccubus && (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0) && (SummonSuccubus.IsSpellUsable || SummonShivarra.IsSpellUsable))
        {
            Logging.WriteFight(" - PET DEAD - ");
            if (GrimoireofSupremacy.KnownSpell)
                SummonShivarra.Cast();
            else
                SummonSuccubus.Cast();
            Others.SafeSleep(500);

            while (ObjectManager.Me.IsCast)
                Others.SafeSleep(20);
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
        if (MySettings.UseEveryManforHimself && ObjectManager.Me.IsStunned && EveryManforHimself.IsSpellUsable)
            EveryManforHimself.Cast();

        if (MySettings.UseBloodHorror && ObjectManager.Me.HealthPercent <= MySettings.UseBloodHorrorAtPercentage && BloodHorror.IsSpellUsable && ObjectManager.Target.GetDistance < 6)
        {
            BloodHorror.Cast();
            return;
        }
        if (MySettings.UseDarkBargain && ObjectManager.Me.HealthPercent <= MySettings.UseDarkBargainAtPercentage && DarkBargain.IsSpellUsable)
        {
            DarkBargain.Cast();
            _onCd = new Timer(1000*8);
            return;
        }
        if (MySettings.UseFear && ObjectManager.Me.HealthPercent <= MySettings.UseFearAtPercentage && _fearTimer.IsReady && Fear.IsSpellUsable)
        {
            Fear.Cast();
            _fearTimer = new Timer(1000*10);
            _onCd = new Timer(1000*2);
            return;
        }
        if (MySettings.UseHowlofTerror && ObjectManager.Me.HealthPercent <= MySettings.UseHowlofTerrorAtPercentage && HowlofTerror.IsSpellUsable
            && ObjectManager.GetUnitInSpellRange(HowlofTerror.MaxRangeHostile) > 0)
        {
            HowlofTerror.Cast();
            return;
        }
        if (MySettings.UseMeteorStrike && MySettings.UseSummonInfernal && ObjectManager.Me.HealthPercent <= MySettings.UseMeteorStrikeAtPercentage
            && (ObjectManager.Pet.Health != 0 || ObjectManager.Pet.Guid != 0) && CommandDemon.IsSpellUsable)
        {
            CommandDemon.Cast();
            return;
        }
        if (MySettings.UseSacrificialPact && ObjectManager.Me.HealthPercent <= MySettings.UseSacrificialPactAtPercentage && (ObjectManager.Pet.Health != 0 || ObjectManager.Pet.Guid != 0)
            && SacrificialPact.IsSpellUsable)
        {
            SacrificialPact.Cast();
            _onCd = new Timer(1000*10);
            return;
        }
        if (MySettings.UseShadowfury && ObjectManager.Me.HealthPercent <= MySettings.UseShadowfuryAtPercentage && Shadowfury.IsSpellUsable
            && Shadowfury.IsHostileDistanceGood)
        {
            SpellManager.CastSpellByIDAndPosition(30283, ObjectManager.Target.Position);
            _onCd = new Timer(1000*3);
            return;
        }
        if (MySettings.UseStoneform && ObjectManager.Me.HealthPercent <= MySettings.UseStoneformAtPercentage && Stoneform.IsSpellUsable)
        {
            Stoneform.Cast();
            _onCd = new Timer(1000*8);
            return;
        }
        if (MySettings.UseUnendingResolve && ObjectManager.Me.HealthPercent <= MySettings.UseUnendingResolveAtPercentage && UnendingResolve.IsSpellUsable)
        {
            UnendingResolve.Cast();
            _onCd = new Timer(1000*8);
            return;
        }
        if (MySettings.UseWarStomp && ObjectManager.Me.HealthPercent <= MySettings.UseWarStompAtPercentage && WarStomp.IsSpellUsable
            && ObjectManager.GetUnitInSpellRange(WarStomp.MaxRangeHostile) > 0)
        {
            WarStomp.Cast();
            _onCd = new Timer(1000*2);
            return;
        }
        if (MySettings.UseWhiplash && MySettings.UseSummonSuccubus && ObjectManager.Me.HealthPercent <= MySettings.UseWhiplashAtPercentage && CommandDemon.IsSpellUsable
            && ObjectManager.Target.GetDistance < 6)
        {
            CommandDemon.Cast();
            _onCd = new Timer(1000*2);
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (MySettings.UseCauterizeMaster && MySettings.UseSummonImp && ObjectManager.Me.HealthPercent <= MySettings.UseCauterizeMasterAtPercentage
            && (ObjectManager.Pet.Health != 0 || ObjectManager.Pet.Guid != 0) && CommandDemon.IsSpellUsable)
        {
            CommandDemon.Cast();
            return;
        }
        if (MySettings.UseCreateHealthstone && _healthstoneTimer.IsReady && ObjectManager.Me.HealthPercent <= MySettings.UseCreateHealthstoneAtPercentage
            && ItemsManager.GetItemCount(5512) > 0)
        {
            Logging.WriteFight("Use Healthstone.");
            ItemsManager.UseItem("Healthstone");
            _healthstoneTimer = new Timer(1000*60);
            return;
        }
        if (MySettings.UseDarkRegeneration && ObjectManager.Me.HealthPercent <= MySettings.UseDarkRegenerationAtPercentage && DarkRegeneration.IsSpellUsable)
        {
            DarkRegeneration.Cast();
            return;
        }
        if (MySettings.UseDrainLife && DrainLife.KnownSpell && DrainLife.IsHostileDistanceGood && DrainLife.IsSpellUsable
            && ObjectManager.Me.HealthPercent <= MySettings.UseDrainLifeAtPercentage)
        {
            DrainLife.Launch(true, false, true);
            while (ObjectManager.Me.IsCast && ObjectManager.Pet.HealthPercent < 96)
                Others.SafeSleep(20);

            if (ObjectManager.Me.IsCast)
                ObjectManager.Me.StopCast();
            return;
        }
        if (MySettings.UseGiftoftheNaaru && ObjectManager.Me.HealthPercent <= MySettings.UseGiftoftheNaaruAtPercentage && GiftoftheNaaru.IsSpellUsable)
        {
            GiftoftheNaaru.Cast();
            return;
        }
        if (MySettings.UseMortalCoil && ObjectManager.Me.HealthPercent <= MySettings.UseMortalCoilAtPercentage && MortalCoil.IsSpellUsable && MortalCoil.IsHostileDistanceGood)
        {
            MortalCoil.Cast();
            return;
        }
        if (MySettings.UseArcaneTorrentForResource && ObjectManager.Me.ManaPercentage <= MySettings.UseArcaneTorrentForResourceAtPercentage && ArcaneTorrent.IsSpellUsable)
        {
            ArcaneTorrent.Cast();
            return;
        }
        if (MySettings.UseLifeTap && ObjectManager.Me.ManaPercentage <= MySettings.UseLifeTapAtPercentage && LifeTap.IsSpellUsable)
            LifeTap.Cast();
    }

    private void Decast()
    {
        if (MySettings.UseArcaneTorrentForDecast && ObjectManager.Me.HealthPercent <= MySettings.UseArcaneTorrentForDecastAtPercentage
            && ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && ArcaneTorrent.IsSpellUsable && ObjectManager.Target.GetDistance < 8)
        {
            ArcaneTorrent.Cast();
            return;
        }
        if (MySettings.UseShadowLock && MySettings.UseSummonDoomguard && ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe
            && (ObjectManager.Pet.Health != 0 || ObjectManager.Pet.Guid != 0) && CommandDemon.IsSpellUsable && ShadowBolt.IsHostileDistanceGood)
        {
            CommandDemon.Cast();
            return;
        }

        if (MySettings.UseSpellLock && MySettings.UseSummonFelhunter && ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe
            && (ObjectManager.Pet.Health != 0 || ObjectManager.Pet.Guid != 0) && CommandDemon.IsSpellUsable && ShadowBolt.IsHostileDistanceGood)
            CommandDemon.Cast();
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
        if (MySettings.UseBerserking && Berserking.IsSpellUsable && ShadowBolt.IsHostileDistanceGood)
            Berserking.Cast();

        if (MySettings.UseBloodFury && BloodFury.IsSpellUsable && ShadowBolt.IsHostileDistanceGood)
            BloodFury.Cast();

        if (MySettings.UseDarkSoulKnowledge && !MySettings.UseMetamorphosis && !DarkSoulKnowledge.HaveBuff && DarkSoulKnowledge.IsSpellUsable && ShadowBolt.IsHostileDistanceGood)
            DarkSoulKnowledge.Cast();

        if (MySettings.UseSummonDoomguard && !DemonicServitude.KnownSpell && SummonDoomguard.IsSpellUsable && ObjectManager.GetUnitInSpellRange(Hellfire.MaxRangeHostile) < 8)
        {
            if (GrimoireofSupremacy.KnownSpell)
                SummonTerrorguard.Cast();
            else
                SummonDoomguard.Cast();
        }
        if (MySettings.UseSummonInfernal && !DemonicServitude.KnownSpell && SummonInfernal.IsSpellUsable && ObjectManager.GetUnitInSpellRange(Hellfire.MaxRangeHostile) > 7)
        {
            if (GrimoireofSupremacy.KnownSpell)
                SpellManager.CastSpellByIDAndPosition(140763, ObjectManager.Target.Position);
            else
                SpellManager.CastSpellByIDAndPosition(1122, ObjectManager.Target.Position);
        }

        if (MySettings.UseGrimoireofService && GrimoireofService.IsSpellUsable && ShadowBolt.IsHostileDistanceGood)
            GrimoireofService.Cast();
    }

    private void DPSCycle()
    {
        Usefuls.SleepGlobalCooldown();
        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            if (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0)
                Pet();

            if (MySettings.UseKilJaedensCunning && !KilJaedensCunning.HaveBuff && ObjectManager.Me.GetMove && KilJaedensCunning.IsSpellUsable && ShadowBolt.IsHostileDistanceGood)
                KilJaedensCunning.Cast();

            if (MySettings.UseMannarothsFury && MannarothsFury.IsSpellUsable && ObjectManager.GetUnitInSpellRange(Hellfire.MaxRangeHostile) > 2)
                MannarothsFury.Cast();

            if (MySettings.UseMetamorphosis && ObjectManager.Me.DemonicFury > 899 || !Doom.TargetHaveBuff || DarkSoulKnowledge.IsSpellUsable)
            {
                if (ObjectManager.Me.DemonicFury > 450)
                {
                    if (MySettings.UseCorruption && !CorruptionDebuff.TargetHaveBuff && Corruption.IsSpellUsable && Corruption.IsHostileDistanceGood)
                        Corruption.Cast();

                    if (MySettings.UseMetamorphosis && Metamorphosis.KnownSpell)
                    {
                        if (MySettings.UseCataclysm && Cataclysm.KnownSpell && (!Cataclysm.IsSpellUsable || ObjectManager.Me.DemonicFury < 950))
                            Others.SafeSleep(200);
                        else
                            MetamorphosisCombat();
                    }
                }
            }

            if (Metamorphosis.HaveBuff)
                MetamorphosisCombat();

            if (MySettings.UseCorruption && (!CorruptionDebuff.TargetHaveBuff || ObjectManager.Target.UnitAura(146739).AuraTimeLeftInMs < 5400) && Corruption.IsSpellUsable
                && Corruption.IsHostileDistanceGood)
            {
                Corruption.Cast();
                return;
            }
            if (MySettings.UseHandofGuldan && (!HandofGuldan.TargetHaveBuff || ObjectManager.Target.UnitAura(105174).AuraTimeLeftInMs < 3000) && HandofGuldan.GetSpellCharges > 0
                && HandofGuldan.IsSpellUsable && HandofGuldan.IsHostileDistanceGood)
            {
                HandofGuldan.Cast();
                return;
            }
            if (MySettings.UseSoulFire && ((MoltenCore.BuffStack > 0 && Demonbolt.KnownSpell) || MoltenCore.BuffStack > 4 || ObjectManager.Target.HealthPercent < 25)
                && SoulFire.IsSpellUsable && SoulFire.IsHostileDistanceGood && ObjectManager.GetUnitInSpellRange(Hellfire.MaxRangeHostile) < 8)
            {
                SoulFire.Cast();
                return;
            }
            if (MySettings.UseCommandDemon && MySettings.UseSummonFelguard && CommandDemon.IsSpellUsable && ObjectManager.Pet.Health > 0
                && ObjectManager.GetUnitInSpellRange(Hellfire.MaxRangeHostile) > 1)
            {
                CommandDemon.Cast();
                return;
            }
            if (MySettings.UseHellfire && Hellfire.IsSpellUsable && ObjectManager.Me.HealthPercent > MySettings.UseHellfireAbovePercentage && (ObjectManager.GetUnitInSpellRange(Hellfire.MaxRangeHostile) > 4
                                                                                                                                               || MannarothsFury.HaveBuff))
            {
                Hellfire.Cast();
                Others.SafeSleep(200);
                while (ObjectManager.Me.IsCast && ObjectManager.GetUnitInSpellRange(Hellfire.MaxRangeHostile) > 2 && ObjectManager.Me.HealthPercent > MySettings.UseHellfireAbovePercentage)
                    Others.SafeSleep(200);
                return;
            }
            if (MySettings.UseShadowBolt && ShadowBolt.IsSpellUsable && ShadowBolt.IsHostileDistanceGood)
                ShadowBolt.Cast();
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }

    private void MetamorphosisCombat()
    {
        while (ObjectManager.Me.DemonicFury > 100)
        {
            if (MySettings.UseMetamorphosis && !Metamorphosis.HaveBuff && Metamorphosis.IsSpellUsable)
                Metamorphosis.Cast();

            if (MySettings.UseDarkSoulKnowledge && !DarkSoulKnowledge.HaveBuff && DarkSoulKnowledge.IsSpellUsable && ShadowBolt.IsHostileDistanceGood)
                DarkSoulKnowledge.Cast();

            if (MySettings.UseCataclysm && Cataclysm.IsSpellUsable && Cataclysm.IsHostileDistanceGood)
                SpellManager.CastSpellByIDAndPosition(152108, ObjectManager.Target.Position);

            if (MySettings.UseDoom && (!Doom.TargetHaveBuff || ObjectManager.Target.UnitAura(603).AuraTimeLeftInMs < 18000)
                && !SpellManager.IsSpellOnCooldown(603) && Corruption.IsHostileDistanceGood)
            {
                if (Cataclysm.KnownSpell && Cataclysm.IsSpellUsable && Cataclysm.IsHostileDistanceGood)
                    SpellManager.CastSpellByIDAndPosition(152108, ObjectManager.Target.Position);
                else
                    Doom.Cast();
            }

            if (MySettings.UseCommandDemon && MySettings.UseSummonFelguard && CommandDemon.IsSpellUsable && ObjectManager.Pet.Health > 0
                && ObjectManager.GetUnitInSpellRange(Hellfire.MaxRangeHostile) > 1)
                CommandDemon.Cast();

            if (MySettings.UseImmolationAura && !ImmolationAura.HaveBuff && Hellfire.IsSpellUsable
                && !SpellManager.IsSpellOnCooldown(104025) && ObjectManager.GetUnitInSpellRange(Hellfire.MaxRangeHostile) > 2)
                ImmolationAura.Cast();

            if (MySettings.UseDemonbolt && Demonbolt.BuffStack < 4 && Demonbolt.IsSpellUsable && Demonbolt.IsHostileDistanceGood)
                Demonbolt.Cast();

            if (MySettings.UseChaosWave && DarkSoulKnowledge.HaveBuff && ChaosWave.GetSpellCharges > 0 && ObjectManager.Me.DemonicFury > 79
                && !SpellManager.IsSpellOnCooldown(124916) && HandofGuldan.IsHostileDistanceGood)
                ChaosWave.Cast();

            if (MySettings.UseTouchofChaos && (CorruptionDebuff.TargetHaveBuff && ObjectManager.Target.UnitAura(146739).AuraTimeLeftInMs < 5400
                                               || MoltenCore.BuffStack < 1) && ObjectManager.Me.DemonicFury > 39 && !SpellManager.IsSpellOnCooldown(103964) && ShadowBolt.IsHostileDistanceGood)
                TouchofChaos.Cast();

            if (MySettings.UseSoulFire && MoltenCore.BuffStack > 0 && SoulFire.IsSpellUsable && SoulFire.IsHostileDistanceGood && ObjectManager.GetUnitInSpellRange(Hellfire.MaxRangeHostile) < 8)
                SoulFire.Cast();

            Others.SafeSleep(1000);
        }

        Others.SafeSleep(1000);
        if (Metamorphosis.HaveBuff)
        {
            Metamorphosis.Cast();
            Others.SafeSleep(1000);
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

    #region Nested type: WarlockDemonologySettings

    [Serializable]
    public class WarlockDemonologySettings : Settings
    {
        public bool DoAvoidMelee = false;
        public int DoAvoidMeleeDistance = 0;
        public bool UseAlchFlask = true;
        public bool UseArcaneTorrentForDecast = true;
        public int UseArcaneTorrentForDecastAtPercentage = 95;
        public bool UseArcaneTorrentForResource = true;
        public int UseArcaneTorrentForResourceAtPercentage = 80;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseBloodHorror = true;
        public int UseBloodHorrorAtPercentage = 20;
        public bool UseBurningRush = true;
        public int UseBurningRushAbovePercentage = 50;
        public bool UseCataclysm = true;
        public bool UseCauterizeMaster = true;
        public int UseCauterizeMasterAtPercentage = 90;
        public bool UseChaosWave = true;
        public bool UseCommandDemon = true;
        public bool UseCorruption = true;
        public bool UseCreateHealthstone = true;
        public int UseCreateHealthstoneAtPercentage = 80;
        public bool UseDarkBargain = true;
        public int UseDarkBargainAtPercentage = 60;
        public bool UseDarkflight = true;
        public bool UseDarkIntent = true;
        public bool UseDarkRegeneration = true;
        public int UseDarkRegenerationAtPercentage = 70;
        public bool UseDarkSoulKnowledge = true;
        public bool UseDemonbolt = true;
        public bool UseDoom = true;
        public bool UseDrainLife = true;
        public int UseDrainLifeAtPercentage = 65;
        public bool UseEveryManforHimself = true;
        public bool UseFear = true;
        public int UseFearAtPercentage = 20;
        public bool UseGiftoftheNaaru = true;
        public int UseGiftoftheNaaruAtPercentage = 80;
        public bool UseGrimoireofService = true;
        public bool UseHandofGuldan = true;
        public bool UseHealthFunnel = true;
        public int UseHealthFunnelAtPercentage = 50;
        public bool UseHellfire = true;
        public int UseHellfireAbovePercentage = 50;
        public bool UseHowlofTerror = true;
        public int UseHowlofTerrorAtPercentage = 20;
        public bool UseImmolationAura = true;
        public bool UseKilJaedensCunning = true;
        public bool UseLifeTap = true;
        public int UseLifeTapAtPercentage = 70;
        public bool UseLowCombat = true;
        public int UseLowCombatAtPercentage = 15;
        public bool UseMannarothsFury = true;
        public bool UseMetamorphosis = true;
        public bool UseMeteorStrike = true;
        public int UseMeteorStrikeAtPercentage = 85;
        public bool UseMortalCoil = true;
        public int UseMortalCoilAtPercentage = 85;
        public bool UseSacrificialPact = true;
        public int UseSacrificialPactAtPercentage = 70;
        public bool UseShadowBolt = true;
        public bool UseShadowfury = true;
        public int UseShadowfuryAtPercentage = 90;
        public bool UseShadowLock = true;
        public int UseShadowLockAtPercentage = 95;
        public bool UseSoulFire = true;
        public bool UseSoulstone = true;
        public bool UseSpellLock = true;
        public int UseSpellLockAtPercentage = 95;
        public bool UseStoneform = true;
        public int UseStoneformAtPercentage = 80;
        public bool UseSummonDoomguard = true;
        public bool UseSummonFelguard = true;
        public bool UseSummonFelhunter = false;
        public bool UseSummonImp = false;
        public bool UseSummonInfernal = true;
        public bool UseSummonSuccubus = false;
        public bool UseSummonVoidwalker = false;
        public bool UseTouchofChaos = true;
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;
        public bool UseUnendingBreath = false;
        public bool UseUnendingResolve = true;
        public int UseUnendingResolveAtPercentage = 70;
        public bool UseVoidRay = true;
        public bool UseWarStomp = true;
        public int UseWarStompAtPercentage = 80;
        public bool UseWhiplash = true;
        public int UseWhiplashAtPercentage = 85;

        public WarlockDemonologySettings()
        {
            ConfigWinForm("Warlock Demonology Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent for Interrupt", "UseArcaneTorrentForDecast", "Professions & Racials", "AtPercentage");
            AddControlInWinForm("Use Arcane Torrent for Resource", "UseArcaneTorrentForResource", "Professions & Racials", "AtPercentage");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Darkflight", "UseDarkflight", "Professions & Racials");
            AddControlInWinForm("Use Every Man for Himself", "UseEveryManforHimself", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials", "AtPercentage");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials", "AtPercentage");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials", "AtPercentage");
            /* Warlock Buffs */
            AddControlInWinForm("Use Burning Rush", "UseBurningRush", "Warlock Buffs", "BelowPercentage");
            AddControlInWinForm("Use Dark Intent", "UseDarkIntent", "Warlock Buffs");
            AddControlInWinForm("Use Grimoire of Sacrifice", "UseGrimoireofSacrifice", "Warlock Buffs");
            AddControlInWinForm("Use Kil'Jaeden's Cunning", "UseKilJaedensCunning", "Warlock Buffs");
            AddControlInWinForm("Use Metamorphosis", "UseMetamorphosis", "Warlock Buffs");
            AddControlInWinForm("Use Soulstone", "UseSoulstone", "Warlock Buffs");
            AddControlInWinForm("Use Unending Breath", "UseUnendingBreath", "Warlock Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Chaos Wave", "UseChaosWave", "Offensive Spell");
            AddControlInWinForm("Use Command Demon", "UseCommandDemon", "Offensive Spell");
            AddControlInWinForm("Use Corruption", "UseCorruption", "Offensive Spell");
            AddControlInWinForm("Use Demonbolt", "UseDemonbolt", "Offensive Spell");
            AddControlInWinForm("Use Doom", "UseDoom", "Offensive Spell");
            AddControlInWinForm("Use Hand of Guldan", "UseHandofGuldan", "Offensive Spell");
            AddControlInWinForm("Use Hellfire", "UseHellfire", "Offensive Spell", "AbovePercentage");
            AddControlInWinForm("Use Immolation Aura", "UseImmolationAura", "Offensive Spell");
            AddControlInWinForm("Use Shadow Bolt", "UseShadowBolt", "Offensive Spell");
            AddControlInWinForm("Use Soul Fire", "UseSoulFire", "Offensive Spell");
            AddControlInWinForm("Use Summon Felguard", "UseSummonFelguard", "Offensive Spell");
            AddControlInWinForm("Use Summon Felhunter", "UseSummonFelhunter", "Offensive Spell");
            AddControlInWinForm("Use Summon Imp", "UseSummonImp", "Offensive Spell");
            AddControlInWinForm("Use Summon Succubus", "UseSummonSuccubus", "Offensive Spell");
            AddControlInWinForm("Use Summon Voidwalker", "UseSummonVoidwalker", "Offensive Spell");
            AddControlInWinForm("Use Touch of Chaos", "UseTouchofChaos", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use Cataclysm", "UseCataclysm", "Offensive Cooldown");
            AddControlInWinForm("Use Dark Soul: Knowledge", "UseDarkSoulKnowledge", "Offensive Cooldown");
            AddControlInWinForm("Use Grimoire of Service", "UseGrimoireofService", "Offensive Cooldown");
            AddControlInWinForm("Use Mannaroth's Fury", "UseMannarothsFury", "Offensive Cooldown");
            AddControlInWinForm("Use Summon Doomguard", "UseSummonDoomguard", "Offensive Cooldown");
            AddControlInWinForm("Use Summon Infernal", "UseSummonInfernal", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Blood Horror", "UseBloodHorror", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Dark Bargain", "UseDarkBargain", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Fear", "UseFear", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Howl of Terror", "UseHowlofTerror", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Meteor Strike", "UseMeteorStrike", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Sacrificial Pact", "UseSacrificialPact", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Shadowfury", "UseShadowfury", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Shadow Lock", "UseShadowLock", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Spell Lock", "UseSpellLock", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Unending Resolve", "UseUnendingResolve", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Whiplash", "UseWhiplash", "Defensive Cooldown", "AtPercentage");
            /* Healing Spell */
            AddControlInWinForm("Use Cauterize Master", "UseCauterizeMaster", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Create Healthstone", "UseCreateHealthstone", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Dark Regeneration", "UseDarkRegeneration", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Drain Life", "UseDrainLife", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Health Funnel", "UseHealthFunnel", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Life Tap", "UseLifeTap", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Mortal Coil", "UseMortalCoil", "Healing Spell", "AtPercentage");
            /* Game Settings */
            AddControlInWinForm("Use Low Combat Settings - At Level", "UseLowCombat", "Game Settings", "AtPercentage");
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
            AddControlInWinForm("Do avoid melee (Off Advised!!)", "DoAvoidMelee", "Game Settings");
            AddControlInWinForm("Avoid melee distance (1 to 4)", "DoAvoidMeleeDistance", "Game Settings");
        }

        public static WarlockDemonologySettings CurrentSetting { get; set; }

        public static WarlockDemonologySettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Warlock_Demonology.xml";
            if (File.Exists(currentSettingsFile))
            {
                return
                    CurrentSetting = Load<WarlockDemonologySettings>(currentSettingsFile);
            }
            return new WarlockDemonologySettings();
        }
    }

    #endregion
}

public class WarlockDestruction
{
    private static WarlockDestructionSettings MySettings = WarlockDestructionSettings.GetSettings();

    #region General Timers & Variables

    private readonly WoWItem _firstTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET);
    private readonly WoWItem _secondTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET, 2);
    public int DecastHP = 0;
    public int DefenseHP = 0;
    public int HealHP = 0;
    public int HealMP = 0;
    public int LC = 0;

    private Timer _onCd = new Timer(0);

    #endregion

    #region Professions & Racials

    public readonly Spell ArcaneTorrent = new Spell("Arcane Torrent");
    public readonly Spell Berserking = new Spell("Berserking");
    public readonly Spell BloodFury = new Spell("Blood Fury");
    public readonly Spell Darkflight = new Spell("Darkflight");
    public readonly Spell EveryManforHimself = new Spell("Every Man for Himself");
    public readonly Spell GiftoftheNaaru = new Spell("Gift of the Naaru");
    public readonly Spell Stoneform = new Spell("Stoneform");
    public readonly Spell WarStomp = new Spell("War Stomp");

    #endregion

    #region Warlock Buffs

    public readonly Spell BurningRush = new Spell("Burning Rush");
    public readonly Spell DarkIntent = new Spell("Dark Intent");
    public readonly Spell GrimoireofSacrifice = new Spell("Grimoire of Sacrifice");
    public readonly Spell GrimoireofSupremacy = new Spell("Grimoire of Supremacy");
    public readonly Spell ImmolateDebuff = new Spell(157736);
    public readonly Spell KilJaedensCunning = new Spell("Kil'Jaeden's Cunning");
    public readonly Spell RainofFireDebuff = new Spell(104232);
    public readonly Spell Soulstone = new Spell("Soulstone");
    public readonly Spell UnendingBreath = new Spell("Unending Breath");

    #endregion

    #region Offensive Spell

    public readonly Spell ChaosBolt = new Spell("Chaos Bolt");
    public readonly Spell CommandDemon = new Spell("Command Demon");
    public readonly Spell Conflagrate = new Spell("Conflagrate");
    public readonly Spell DemonicServitude = new Spell("Demonic Servitude");
    public readonly Spell Immolate = new Spell("Immolate");
    public readonly Spell Incinerate = new Spell("Incinerate");
    public readonly Spell RainofFire = new Spell("Rain of Fire");
    public readonly Spell Shadowburn = new Spell("Shadowburn");
    public readonly Spell SummonFelhunter = new Spell("Summon Felhunter");
    public readonly Spell SummonFelImp = new Spell("Summon Fel Imp");
    public readonly Spell SummonImp = new Spell("Summon Imp");
    public readonly Spell SummonObserver = new Spell("Summon Observer");
    public readonly Spell SummonShivarra = new Spell("Summon Shivarra");
    public readonly Spell SummonSuccubus = new Spell("Summon Succubus");
    public readonly Spell SummonVoidlord = new Spell("Summon Voidlord");
    public readonly Spell SummonVoidwalker = new Spell("Summon Voidwalker");

    #endregion

    #region Offensive Cooldown

    public readonly Spell Cataclysm = new Spell("Cataclysm");
    public readonly Spell DarkSoulInstability = new Spell("Dark Soul: Instability");
    public readonly Spell FireandBrimstone = new Spell("Fire and Brimstone");
    public readonly Spell GrimoireofService = new Spell("Grimoire of Service");
    public readonly Spell MannarothsFury = new Spell("Mannaroth's Fury");
    public readonly Spell SummonAbyssal = new Spell("Summon Abyssal");
    public readonly Spell SummonDoomguard = new Spell("Summon Doomguard");
    public readonly Spell SummonInfernal = new Spell("Summon Infernal");
    public readonly Spell SummonTerrorguard = new Spell(112927);

    #endregion

    #region Defensive Cooldown

    public readonly Spell BloodHorror = new Spell("Blood Horror");
    public readonly Spell DarkBargain = new Spell("Dark Bargain");
    public readonly Spell Fear = new Spell("Fear");
    public readonly Spell HowlofTerror = new Spell("HowlofTerror");
    public readonly Spell SacrificialPact = new Spell("Sacrificial Pact");
    public readonly Spell Shadowfury = new Spell("Shadowfury");
    public readonly Spell UnendingResolve = new Spell("Unending Resolve");
    private Timer _fearTimer = new Timer(0);

    #endregion

    #region Healing Spell

    public readonly Spell CreateHealthstone = new Spell("Create Healthstone");
    public readonly Spell DarkRegeneration = new Spell("Dark Regeneration");
    public readonly Spell EmberTap = new Spell("Ember Tap");
    public readonly Spell FlamesofXoroth = new Spell("Flames of Xoroth");
    public readonly Spell MortalCoil = new Spell("Mortal Coil");
    private Timer _healthstoneTimer = new Timer(0);

    #endregion

    public WarlockDestruction()
    {
        Main.InternalRange = 39f;
        Main.InternalAggroRange = 39f;
        MySettings = WarlockDestructionSettings.GetSettings();
        Main.DumpCurrentSettings<WarlockDestructionSettings>(MySettings);
        UInt128 lastTarget = 0;
        LowHP();

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
                                && (Immolate.IsHostileDistanceGood || ChaosBolt.IsHostileDistanceGood))
                            {
                                Pull();
                                lastTarget = ObjectManager.Me.Target;
                            }

                            if (MySettings.UseLowCombat && (ObjectManager.Me.Level - ObjectManager.Target.Level >= MySettings.UseLowCombatAtPercentage))
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

    private void LowHP()
    {
        if (MySettings.UseBloodHorrorAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseBloodHorrorAtPercentage;

        if (MySettings.UseDarkBargainAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseDarkBargainAtPercentage;

        if (MySettings.UseFearAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseFearAtPercentage;

        if (MySettings.UseHowlofTerrorAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseHowlofTerrorAtPercentage;

        if (MySettings.UseSacrificialPactAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseSacrificialPactAtPercentage;

        if (MySettings.UseShadowfuryAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseShadowfuryAtPercentage;

        if (MySettings.UseStoneformAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseStoneformAtPercentage;

        if (MySettings.UseUnendingResolveAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseUnendingResolveAtPercentage;

        if (MySettings.UseWarStompAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseWarStompAtPercentage;

        if (MySettings.UseArcaneTorrentForDecastAtPercentage > DecastHP)
            DecastHP = MySettings.UseArcaneTorrentForDecastAtPercentage;

        if (MySettings.UseArcaneTorrentForResourceAtPercentage > HealMP)
            HealMP = MySettings.UseArcaneTorrentForResourceAtPercentage;

        if (MySettings.UseCreateHealthstoneAtPercentage > HealHP)
            HealHP = MySettings.UseCreateHealthstoneAtPercentage;

        if (MySettings.UseDarkRegenerationAtPercentage > HealHP)
            HealHP = MySettings.UseDarkRegenerationAtPercentage;

        if (MySettings.UseEmberTapAtPercentage > HealHP)
            HealHP = MySettings.UseEmberTapAtPercentage;

        if (MySettings.UseGiftoftheNaaruAtPercentage > HealHP)
            HealHP = MySettings.UseGiftoftheNaaruAtPercentage;

        if (MySettings.UseMortalCoilAtPercentage > HealHP)
            HealHP = MySettings.UseMortalCoilAtPercentage;
    }

    private void Pull()
    {
        if (ObjectManager.Pet.IsAlive)
        {
            Lua.RunMacroText("/petattack");
            Logging.WriteFight("Cast Pet Attack");
        }
        if (MySettings.UseImmolate && Immolate.IsSpellUsable && Immolate.IsHostileDistanceGood)
        {
            Immolate.Cast();
            return;
        }

        if (MySettings.UseIncinerate && Incinerate.IsSpellUsable && Incinerate.IsHostileDistanceGood)
            Incinerate.Cast();
    }

    private void LowCombat()
    {
        Buff();

        if (MySettings.DoAvoidMelee)
            AvoidMelee();

        if (_onCd.IsReady && ObjectManager.Me.HealthPercent <= DefenseHP)
            DefenseCycle();

        if (ObjectManager.Me.HealthPercent <= HealHP || ObjectManager.Me.ManaPercentage <= HealMP)
            Heal();

        if (MySettings.UseConflagrate && Conflagrate.GetSpellCharges > 0 && Conflagrate.IsSpellUsable && Conflagrate.IsHostileDistanceGood)
        {
            Conflagrate.Cast();
            return;
        }
        if (MySettings.UseCommandDemon && CommandDemon.IsSpellUsable && Conflagrate.IsHostileDistanceGood)
        {
            CommandDemon.Cast();
            return;
        }
        if (MySettings.UseRainofFire && !RainofFireDebuff.TargetHaveBuff && RainofFire.IsSpellUsable && ObjectManager.GetUnitInSpellRange(8) > 2)
        {
            SpellManager.CastSpellByIDAndPosition(RainofFire.Id, ObjectManager.Target.Position);
            return;
        }
        if (MySettings.UseIncinerate && Incinerate.IsSpellUsable && Incinerate.IsHostileDistanceGood)
        {
            Incinerate.Cast();
            return;
        }
    }

    private void Combat()
    {
        Buff();

        if (MySettings.DoAvoidMelee)
            AvoidMelee();

        DPSCycle();

        if (_onCd.IsReady && ObjectManager.Me.HealthPercent <= DefenseHP)
            DefenseCycle();

        if (ObjectManager.Me.HealthPercent <= HealHP || ObjectManager.Me.ManaPercentage <= HealMP)
            Heal();

        if (ObjectManager.Me.HealthPercent <= DecastHP)
            Decast();

        DPSBurst();
        DPSCycle();
    }

    private void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        Pet();

        if (MySettings.UseBurningRush && !Darkflight.HaveBuff && !BurningRush.HaveBuff && ObjectManager.Me.GetMove && !ObjectManager.Target.InCombat && BurningRush.IsSpellUsable)
            BurningRush.Cast();

        if (MySettings.UseBurningRush && BurningRush.HaveBuff && (!ObjectManager.Me.GetMove || ObjectManager.Target.InCombat
                                                                  || ObjectManager.Me.HealthPercent < MySettings.UseBurningRushAbovePercentage) && BurningRush.IsSpellUsable)
            BurningRush.Cast();

        if (MySettings.UseCreateHealthstone && ItemsManager.GetItemCount(5512) == 0 && Usefuls.GetContainerNumFreeSlots > 0 && CreateHealthstone.IsSpellUsable)
        {
            Logging.WriteFight(" - Create Healthstone - ");
            CreateHealthstone.Cast();
            Others.SafeSleep(500);

            while (ObjectManager.Me.IsCast)
                Others.SafeSleep(200);
        }

        if (MySettings.UseDarkflight && Darkflight.KnownSpell && !BurningRush.HaveBuff && ObjectManager.Me.GetMove && !ObjectManager.Target.InCombat && Darkflight.IsSpellUsable)
            Darkflight.Cast();

        if (MySettings.UseDarkIntent && !DarkIntent.HaveBuff && DarkIntent.IsSpellUsable)
            DarkIntent.Cast();

        if (MySettings.UseSoulstone && !Soulstone.HaveBuff && Soulstone.IsSpellUsable)
        {
            Soulstone.Cast();
            Others.SafeSleep(500);

            while (ObjectManager.Me.IsCast)
                Others.SafeSleep(20);
        }

        if (MySettings.UseUnendingBreath && ObjectManager.Me.UnitAura(5697).AuraTimeLeftInMs < 1 && UnendingBreath.IsSpellUsable)
            UnendingBreath.Cast();

        if (MySettings.UseAlchFlask && !ObjectManager.Me.HaveBuff(79638) && !ObjectManager.Me.HaveBuff(79640) && !ObjectManager.Me.HaveBuff(79639)
            && !ItemsManager.IsItemOnCooldown(75525) && ItemsManager.GetItemCount(75525) > 0)
            ItemsManager.UseItem(75525);
    }

    private void Pet()
    {
        if (MySettings.UseFlamesofXoroth && (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0) && !GrimoireofSacrifice.HaveBuff && FlamesofXoroth.IsSpellUsable)
        {
            FlamesofXoroth.Cast();
            Logging.WriteFight(" - PET DEAD - ");
        }

        if (MySettings.UseSummonDoomguard && DemonicServitude.KnownSpell && (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0) && !GrimoireofSacrifice.HaveBuff
            && (SummonDoomguard.IsSpellUsable || SummonTerrorguard.IsSpellUsable))
        {
            Logging.WriteFight(" - PET DEAD - ");
            if (GrimoireofSupremacy.KnownSpell)
                SummonTerrorguard.Cast();
            else
                SummonDoomguard.Cast();
            Others.SafeSleep(500);

            while (ObjectManager.Me.IsCast)
                Others.SafeSleep(20);
        }
        else if (MySettings.UseSummonInfernal && DemonicServitude.KnownSpell && (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0) && !GrimoireofSacrifice.HaveBuff
                 && (SummonInfernal.IsSpellUsable || SummonAbyssal.IsSpellUsable))
        {
            Logging.WriteFight(" - PET DEAD - ");
            if (GrimoireofSupremacy.KnownSpell)
                SpellManager.CastSpellByIDAndPosition(140763, ObjectManager.Target.Position);
            else
                SpellManager.CastSpellByIDAndPosition(1122, ObjectManager.Target.Position);
            Others.SafeSleep(500);

            while (ObjectManager.Me.IsCast)
                Others.SafeSleep(20);
        }
        else if (MySettings.UseSummonFelhunter && (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0) && !GrimoireofSacrifice.HaveBuff
                 && (SummonFelhunter.IsSpellUsable || SummonObserver.IsSpellUsable))
        {
            Logging.WriteFight(" - PET DEAD - ");
            if (GrimoireofSupremacy.KnownSpell)
                SummonObserver.Cast();
            else
                SummonFelhunter.Cast();
            Others.SafeSleep(500);

            while (ObjectManager.Me.IsCast)
                Others.SafeSleep(20);
        }
        else if (MySettings.UseSummonImp && (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0) && !GrimoireofSacrifice.HaveBuff
                 && (SummonImp.IsSpellUsable || SummonFelImp.IsSpellUsable))
        {
            Logging.WriteFight(" - PET DEAD - ");
            if (GrimoireofSupremacy.KnownSpell)
                SummonFelImp.Cast();
            else
                SummonImp.Cast();
            Others.SafeSleep(500);

            while (ObjectManager.Me.IsCast)
                Others.SafeSleep(20);
        }
        else if (MySettings.UseSummonVoidwalker && (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0) && !GrimoireofSacrifice.HaveBuff
                 && (SummonVoidwalker.IsSpellUsable || SummonVoidlord.IsSpellUsable))
        {
            Logging.WriteFight(" - PET DEAD - ");
            if (GrimoireofSupremacy.KnownSpell)
                SummonVoidlord.Cast();
            else
                SummonVoidwalker.Cast();
            Others.SafeSleep(500);

            while (ObjectManager.Me.IsCast)
                Others.SafeSleep(20);
        }
        else if (MySettings.UseSummonSuccubus && (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0) && !GrimoireofSacrifice.HaveBuff
                 && (SummonSuccubus.IsSpellUsable || SummonShivarra.IsSpellUsable))
        {
            Logging.WriteFight(" - PET DEAD - ");
            if (GrimoireofSupremacy.KnownSpell)
                SummonShivarra.Cast();
            else
                SummonSuccubus.Cast();
            Others.SafeSleep(500);

            while (ObjectManager.Me.IsCast)
                Others.SafeSleep(20);
        }

        if (MySettings.UseGrimoireofSacrifice && !GrimoireofSacrifice.HaveBuff && (ObjectManager.Pet.Health != 0 || ObjectManager.Pet.Guid != 0) && GrimoireofSacrifice.IsSpellUsable)
            GrimoireofSacrifice.Cast();
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
        if (MySettings.UseEveryManforHimself && ObjectManager.Me.IsStunned && EveryManforHimself.IsSpellUsable)
            EveryManforHimself.Cast();

        if (MySettings.UseBloodHorror && ObjectManager.Me.HealthPercent <= MySettings.UseBloodHorrorAtPercentage && BloodHorror.IsSpellUsable && ObjectManager.Target.GetDistance < 6)
        {
            BloodHorror.Cast();
            return;
        }
        if (MySettings.UseDarkBargain && ObjectManager.Me.HealthPercent <= MySettings.UseDarkBargainAtPercentage && DarkBargain.IsSpellUsable)
        {
            DarkBargain.Cast();
            _onCd = new Timer(1000*8);
            return;
        }
        if (MySettings.UseFear && ObjectManager.Me.HealthPercent <= MySettings.UseFearAtPercentage && _fearTimer.IsReady && Fear.IsSpellUsable)
        {
            Fear.Cast();
            _fearTimer = new Timer(1000*10);
            _onCd = new Timer(1000*2);
            return;
        }
        if (MySettings.UseHowlofTerror && ObjectManager.Me.HealthPercent <= MySettings.UseHowlofTerrorAtPercentage && HowlofTerror.IsSpellUsable
            && ObjectManager.GetUnitInSpellRange(HowlofTerror.MaxRangeHostile) > 0)
        {
            HowlofTerror.Cast();
            return;
        }
        if (MySettings.UseMeteorStrike && MySettings.UseSummonInfernal && ObjectManager.Me.HealthPercent <= MySettings.UseMeteorStrikeAtPercentage
            && (ObjectManager.Pet.Health != 0 || ObjectManager.Pet.Guid != 0) && CommandDemon.IsSpellUsable)
        {
            CommandDemon.Cast();
            return;
        }
        if (MySettings.UseSacrificialPact && ObjectManager.Me.HealthPercent <= MySettings.UseSacrificialPactAtPercentage && (ObjectManager.Pet.Health != 0 || ObjectManager.Pet.Guid != 0)
            && SacrificialPact.IsSpellUsable)
        {
            SacrificialPact.Cast();
            _onCd = new Timer(1000*10);
            return;
        }
        if (MySettings.UseShadowfury && ObjectManager.Me.HealthPercent <= MySettings.UseShadowfuryAtPercentage && Shadowfury.IsSpellUsable
            && Shadowfury.IsHostileDistanceGood)
        {
            SpellManager.CastSpellByIDAndPosition(30283, ObjectManager.Target.Position);
            _onCd = new Timer(1000*3);
            return;
        }
        if (MySettings.UseStoneform && ObjectManager.Me.HealthPercent <= MySettings.UseStoneformAtPercentage && Stoneform.IsSpellUsable)
        {
            Stoneform.Cast();
            _onCd = new Timer(1000*8);
            return;
        }
        if (MySettings.UseUnendingResolve && ObjectManager.Me.HealthPercent <= MySettings.UseUnendingResolveAtPercentage && UnendingResolve.IsSpellUsable)
        {
            UnendingResolve.Cast();
            _onCd = new Timer(1000*8);
            return;
        }
        if (MySettings.UseWarStomp && ObjectManager.Me.HealthPercent <= MySettings.UseWarStompAtPercentage && WarStomp.IsSpellUsable
            && ObjectManager.GetUnitInSpellRange(WarStomp.MaxRangeHostile) > 0)
        {
            WarStomp.Cast();
            _onCd = new Timer(1000*2);
            return;
        }
        if (MySettings.UseWhiplash && MySettings.UseSummonSuccubus && ObjectManager.Me.HealthPercent <= MySettings.UseWhiplashAtPercentage && CommandDemon.IsSpellUsable
            && ObjectManager.Target.GetDistance < 6)
        {
            CommandDemon.Cast();
            _onCd = new Timer(1000*2);
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (MySettings.UseCauterizeMaster && MySettings.UseSummonImp && ObjectManager.Me.HealthPercent <= MySettings.UseCauterizeMasterAtPercentage
            && (ObjectManager.Pet.Health != 0 || ObjectManager.Pet.Guid != 0) && CommandDemon.IsSpellUsable)
        {
            CommandDemon.Cast();
            return;
        }
        if (MySettings.UseCreateHealthstone && _healthstoneTimer.IsReady && ObjectManager.Me.HealthPercent <= MySettings.UseCreateHealthstoneAtPercentage
            && ItemsManager.GetItemCount(5512) > 0)
        {
            Logging.WriteFight("Use Healthstone.");
            ItemsManager.UseItem("Healthstone");
            _healthstoneTimer = new Timer(1000*60);
            return;
        }
        if (MySettings.UseDarkRegeneration && ObjectManager.Me.HealthPercent <= MySettings.UseDarkRegenerationAtPercentage && DarkRegeneration.IsSpellUsable)
        {
            DarkRegeneration.Cast();
            return;
        }
        if (MySettings.UseEmberTap && ObjectManager.Me.HealthPercent <= MySettings.UseEmberTapAtPercentage && EmberTap.IsSpellUsable)
        {
            EmberTap.Cast();
            return;
        }
        if (MySettings.UseGiftoftheNaaru && ObjectManager.Me.HealthPercent <= MySettings.UseGiftoftheNaaruAtPercentage && GiftoftheNaaru.IsSpellUsable)
        {
            GiftoftheNaaru.Cast();
            return;
        }
        if (MySettings.UseMortalCoil && ObjectManager.Me.HealthPercent <= MySettings.UseMortalCoilAtPercentage && MortalCoil.IsSpellUsable && MortalCoil.IsHostileDistanceGood)
        {
            MortalCoil.Cast();
            return;
        }
        if (MySettings.UseArcaneTorrentForResource && ObjectManager.Me.ManaPercentage <= MySettings.UseArcaneTorrentForResourceAtPercentage && ArcaneTorrent.IsSpellUsable)
            ArcaneTorrent.Cast();
    }

    private void Decast()
    {
        if (MySettings.UseArcaneTorrentForDecast && ObjectManager.Me.HealthPercent <= MySettings.UseArcaneTorrentForDecastAtPercentage
            && ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && ArcaneTorrent.IsSpellUsable && ObjectManager.Target.GetDistance < 8)
        {
            ArcaneTorrent.Cast();
            return;
        }
        if (MySettings.UseShadowLock && MySettings.UseSummonDoomguard && ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe
            && (ObjectManager.Pet.Health != 0 || ObjectManager.Pet.Guid != 0) && CommandDemon.IsSpellUsable && Incinerate.IsHostileDistanceGood)
        {
            CommandDemon.Cast();
            return;
        }

        if (MySettings.UseSpellLock && MySettings.UseSummonFelhunter && ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe
            && (ObjectManager.Pet.Health != 0 || ObjectManager.Pet.Guid != 0) && CommandDemon.IsSpellUsable && Incinerate.IsHostileDistanceGood)
            CommandDemon.Cast();
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
        if (MySettings.UseBerserking && Berserking.IsSpellUsable && Incinerate.IsHostileDistanceGood)
            Berserking.Cast();

        if (MySettings.UseBloodFury && BloodFury.IsSpellUsable && Incinerate.IsHostileDistanceGood)
            BloodFury.Cast();

        if (MySettings.UseDarkSoulInstability && !DarkSoulInstability.HaveBuff && DarkSoulInstability.IsSpellUsable && Incinerate.IsHostileDistanceGood)
            DarkSoulInstability.Cast();

        if (MySettings.UseSummonDoomguard && !DemonicServitude.KnownSpell && (SummonDoomguard.IsSpellUsable || SummonTerrorguard.IsSpellUsable)
            && ObjectManager.GetUnitInSpellRange(8) < 8)
        {
            if (GrimoireofSupremacy.KnownSpell)
                SummonTerrorguard.Cast();
            else
                SummonDoomguard.Cast();
        }
        if (MySettings.UseSummonInfernal && !DemonicServitude.KnownSpell && (SummonInfernal.IsSpellUsable || SummonAbyssal.IsSpellUsable)
            && ObjectManager.GetUnitInSpellRange(8) > 7)
        {
            if (GrimoireofSupremacy.KnownSpell)
                SpellManager.CastSpellByIDAndPosition(140763, ObjectManager.Target.Position);
            else
                SpellManager.CastSpellByIDAndPosition(1122, ObjectManager.Target.Position);
        }

        if (MySettings.UseGrimoireofService && GrimoireofService.IsSpellUsable && Incinerate.IsHostileDistanceGood)
            GrimoireofService.Cast();
    }

    private void DPSCycle()
    {
        Usefuls.SleepGlobalCooldown();
        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            if (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0)
            {
                if (GrimoireofSacrifice.KnownSpell && GrimoireofSacrifice.HaveBuff)
                    Others.SafeSleep(20);
                else
                    Pet();
            }

            if (MySettings.UseKilJaedensCunning && !KilJaedensCunning.HaveBuff && ObjectManager.Me.GetMove && KilJaedensCunning.IsSpellUsable && Incinerate.IsHostileDistanceGood)
                KilJaedensCunning.Cast();

            if (MySettings.UseMannarothsFury && MannarothsFury.IsSpellUsable && ObjectManager.GetUnitInSpellRange(8) > 2)
                MannarothsFury.Cast();

            if (MySettings.UseFireandBrimstone && FireandBrimstone.IsSpellUsable && ObjectManager.GetUnitInSpellRange(8) > 5)
            {
                FireandBrimstone.Cast();
                Others.SafeSleep(1000);
            }
            if (MySettings.UseRainofFire && !RainofFireDebuff.TargetHaveBuff && RainofFire.IsSpellUsable && (ObjectManager.GetUnitInSpellRange(8) > 4
                                                                                                             || MannarothsFury.HaveBuff))
            {
                SpellManager.CastSpellByIDAndPosition(5740, ObjectManager.Target.Position);
                return;
            }
            if (MySettings.UseShadowburn && ObjectManager.Target.HealthPercent < 20 && (ObjectManager.Me.BurningEmbers > 35 || DarkSoulInstability.HaveBuff)
                && Shadowburn.IsSpellUsable && Shadowburn.IsHostileDistanceGood)
            {
                Shadowburn.Cast();
                return;
            }
            if (MySettings.UseImmolate && (!ImmolateDebuff.TargetHaveBuff || ObjectManager.Target.UnitAura(157736).AuraTimeLeftInMs < 4500)
                && Immolate.IsSpellUsable && Immolate.IsHostileDistanceGood)
            {
                if (Cataclysm.KnownSpell && Cataclysm.IsSpellUsable)
                    SpellManager.CastSpellByIDAndPosition(152108, ObjectManager.Target.Position);
                else
                    Immolate.Cast();

                return;
            }
            if (MySettings.UseConflagrate && Conflagrate.GetSpellCharges > 1 && Conflagrate.IsSpellUsable && Conflagrate.IsHostileDistanceGood)
            {
                Conflagrate.Cast();
                return;
            }
            if (MySettings.UseCataclysm && Cataclysm.IsSpellUsable && Cataclysm.IsHostileDistanceGood)
            {
                SpellManager.CastSpellByIDAndPosition(152108, ObjectManager.Target.Position);
                return;
            }
            if (MySettings.UseChaosBolt && (ObjectManager.Me.BurningEmbers > 35 || DarkSoulInstability.HaveBuff)
                && ChaosBolt.IsSpellUsable && ChaosBolt.IsHostileDistanceGood)
            {
                ChaosBolt.Cast();
                return;
            }
            if (MySettings.UseConflagrate && Conflagrate.GetSpellCharges > 0 && Conflagrate.IsSpellUsable && Conflagrate.IsHostileDistanceGood)
            {
                Conflagrate.Cast();
                return;
            }
            if (MySettings.UseIncinerate && Conflagrate.GetSpellCharges < 1 && ObjectManager.Me.BurningEmbers < 36 && Incinerate.IsSpellUsable
                && Incinerate.IsHostileDistanceGood)
                Incinerate.Cast();
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

    #region Nested type: WarlockDestructionSettings

    [Serializable]
    public class WarlockDestructionSettings : Settings
    {
        public bool DoAvoidMelee = false;
        public int DoAvoidMeleeDistance = 0;
        public bool UseAlchFlask = true;
        public bool UseArcaneTorrentForDecast = true;
        public int UseArcaneTorrentForDecastAtPercentage = 95;
        public bool UseArcaneTorrentForResource = true;
        public int UseArcaneTorrentForResourceAtPercentage = 80;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseBloodHorror = true;
        public int UseBloodHorrorAtPercentage = 20;
        public bool UseBurningRush = true;
        public int UseBurningRushAbovePercentage = 50;
        public bool UseCataclysm = true;
        public bool UseCauterizeMaster = true;
        public int UseCauterizeMasterAtPercentage = 90;
        public bool UseChaosBolt = true;
        public bool UseCommandDemon = true;
        public bool UseConflagrate = true;
        public bool UseCreateHealthstone = true;
        public int UseCreateHealthstoneAtPercentage = 80;
        public bool UseDarkBargain = true;
        public int UseDarkBargainAtPercentage = 60;
        public bool UseDarkflight = true;
        public bool UseDarkIntent = true;
        public bool UseDarkRegeneration = true;
        public int UseDarkRegenerationAtPercentage = 70;
        public bool UseDarkSoulInstability = true;
        public bool UseEmberTap = true;
        public int UseEmberTapAtPercentage = 65;
        public bool UseEveryManforHimself = true;
        public bool UseFear = true;
        public int UseFearAtPercentage = 20;
        public bool UseFireandBrimstone = true;
        public bool UseFlamesofXoroth = true;
        public bool UseGiftoftheNaaru = true;
        public int UseGiftoftheNaaruAtPercentage = 80;
        public bool UseGrimoireofSacrifice = true;
        public bool UseGrimoireofService = true;
        public bool UseHowlofTerror = true;
        public int UseHowlofTerrorAtPercentage = 20;
        public bool UseImmolate = true;
        public bool UseIncinerate = true;
        public bool UseKilJaedensCunning = true;
        public bool UseLowCombat = true;
        public int UseLowCombatAtPercentage = 15;
        public bool UseMannarothsFury = true;
        public bool UseMeteorStrike = true;
        public int UseMeteorStrikeAtPercentage = 85;
        public bool UseMortalCoil = true;
        public int UseMortalCoilAtPercentage = 85;
        public bool UseRainofFire = true;
        public bool UseSacrificialPact = true;
        public int UseSacrificialPactAtPercentage = 70;
        public bool UseShadowburn = true;
        public bool UseShadowfury = true;
        public int UseShadowfuryAtPercentage = 90;
        public bool UseShadowLock = true;
        public int UseShadowLockAtPercentage = 95;
        public bool UseSoulstone = true;
        public bool UseSpellLock = true;
        public int UseSpellLockAtPercentage = 95;
        public bool UseStoneform = true;
        public int UseStoneformAtPercentage = 80;
        public bool UseSummonDoomguard = true;
        public bool UseSummonFelhunter = true;
        public bool UseSummonImp = false;
        public bool UseSummonInfernal = false;
        public bool UseSummonSuccubus = false;
        public bool UseSummonVoidwalker = false;
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;
        public bool UseUnendingBreath = false;
        public bool UseUnendingResolve = true;
        public int UseUnendingResolveAtPercentage = 70;
        public bool UseWarStomp = true;
        public int UseWarStompAtPercentage = 80;
        public bool UseWhiplash = true;
        public int UseWhiplashAtPercentage = 85;

        public WarlockDestructionSettings()
        {
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent for Interrupt", "UseArcaneTorrentForDecast", "Professions & Racials", "AtPercentage");
            AddControlInWinForm("Use Arcane Torrent for Resource", "UseArcaneTorrentForResource", "Professions & Racials", "AtPercentage");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Darkflight", "UseDarkflight", "Professions & Racials");
            AddControlInWinForm("Use Every Man for Himself", "UseEveryManforHimself", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials", "AtPercentage");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials", "AtPercentage");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials", "AtPercentage");
            /* Warlock Buffs */
            AddControlInWinForm("Use Burning Rush", "UseBurningRush", "Warlock Buffs", "AbovePercentage");
            AddControlInWinForm("Use Dark Intent", "UseDarkIntent", "Warlock Buffs");
            AddControlInWinForm("Use Grimoire of Sacrifice", "UseGrimoireofSacrifice", "Warlock Buffs");
            AddControlInWinForm("Use Kil'Jaeden's Cunning", "UseKilJaedensCunning", "Warlock Buffs");
            AddControlInWinForm("Use Soulstone", "UseSoulstone", "Warlock Buffs");
            AddControlInWinForm("Use Unending Breath", "UseUnendingBreath", "Warlock Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Chaos Bolt", "UseChaosBolt", "Offensive Spell");
            AddControlInWinForm("Use Command Demon", "UseCommandDemon", "Offensive Spell");
            AddControlInWinForm("Use Conflagrate", "UseConflagrate", "Offensive Spell");
            AddControlInWinForm("Use Immolate", "UseImmolate", "Offensive Spell");
            AddControlInWinForm("Use Incinerate", "UseIncinerate", "Offensive Spell");
            AddControlInWinForm("Use Rain of Fire", "UseRainofFire", "Offensive Spell");
            AddControlInWinForm("Use Shadowburn", "UseShadowburn", "Offensive Spell");
            AddControlInWinForm("Use Summon Imp", "UseSummonImp", "Offensive Spell");
            AddControlInWinForm("Use Summon Voidwalker", "UseSummonVoidwalker", "Offensive Spell");
            AddControlInWinForm("Use Summon Felhunter", "UseSummonFelhunter", "Offensive Spell");
            AddControlInWinForm("Use Summon Succubus", "UseSummonSuccubus", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use Cataclysm", "UseCataclysm", "Offensive Cooldown");
            AddControlInWinForm("Use Dark Soul: Instability", "UseDarkSoulInstability", "Offensive Cooldown");
            AddControlInWinForm("Use Fire and Brimstone", "UseFireandBrimstone", "Offensive Cooldown");
            AddControlInWinForm("Use Grimoire of Service", "UseGrimoireofService", "Offensive Cooldown");
            AddControlInWinForm("Use Mannaroth's Fury", "UseMannarothsFury", "Offensive Cooldown");
            AddControlInWinForm("Use Summon Doomguard", "UseSummonDoomguard", "Offensive Cooldown");
            AddControlInWinForm("Use Summon Infernal", "UseSummonInfernal", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Blood Horror", "UseBloodHorror", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Dark Bargain", "UseDarkBargain", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Fear", "UseFear", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Howl of Terror", "UseHowlofTerror", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Sacrificial Pact", "UseSacrificialPact", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Shadowfury", "UseShadowfury", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Spell Lock", "UseSpellLock", "Offensive Spell", "AtPercentage");
            AddControlInWinForm("Use Shadow Lock", "UseShadowLock", "Offensive Spell", "AtPercentage");
            AddControlInWinForm("Use Unending Resolve", "UseUnendingResolve", "Defensive Cooldown", "AtPercentage");
            /* Healing Spell */
            AddControlInWinForm("Use Create Healthstone", "UseCreateHealthstone", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Dark Regeneration", "UseDarkRegeneration", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Ember Tap", "UseEmberTap", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Flames of Xoroth", "UseFlamesofXoroth", "Healing Spell");
            AddControlInWinForm("Use Mortal Coil", "UseMortalCoil", "Healing Spell", "AtPercentage");
            /* Game Settings */
            AddControlInWinForm("Use Low Combat Settings - Level Difference", "UseLowCombat", "Game Settings", "AtPercentage");
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
            AddControlInWinForm("Do avoid melee (Off Advised!!)", "DoAvoidMelee", "Game Settings");
            AddControlInWinForm("Avoid melee distance (1 to 4)", "DoAvoidMeleeDistance", "Game Settings");
        }

        public static WarlockDestructionSettings CurrentSetting { get; set; }

        public static WarlockDestructionSettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Warlock_Destruction.xml";
            if (File.Exists(currentSettingsFile))
            {
                return
                    CurrentSetting = Load<WarlockDestructionSettings>(currentSettingsFile);
            }
            return new WarlockDestructionSettings();
        }
    }

    #endregion
}

public class WarlockAffliction
{
    private static WarlockAfflictionSettings MySettings = WarlockAfflictionSettings.GetSettings();

    #region General Timers & Variables

    private readonly WoWItem _firstTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET);
    private readonly WoWItem _secondTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET, 2);
    public int DecastHP = 0;
    public int DefenseHP = 0;
    public int HealHP = 0;
    public int HealMP = 0;
    public int LC = 0;

    private Timer _onCd = new Timer(0);

    #endregion

    #region Professions & Racials

    public readonly Spell ArcaneTorrent = new Spell("Arcane Torrent");
    public readonly Spell Berserking = new Spell("Berserking");
    public readonly Spell BloodFury = new Spell("Blood Fury");
    public readonly Spell Darkflight = new Spell("Darkflight");
    public readonly Spell EveryManforHimself = new Spell("Every Man for Himself");
    public readonly Spell GiftoftheNaaru = new Spell("Gift of the Naaru");
    public readonly Spell Stoneform = new Spell("Stoneform");
    public readonly Spell WarStomp = new Spell("War Stomp");

    #endregion

    #region Warlock Buffs

    public readonly Spell BurningRush = new Spell("Burning Rush");
    public readonly Spell CorruptionDebuff = new Spell(146739);
    public readonly Spell DarkIntent = new Spell("Dark Intent");
    public readonly Spell GrimoireofSacrifice = new Spell("Grimoire of Sacrifice");
    public readonly Spell GrimoireofSupremacy = new Spell("Grimoire of Supremacy");
    public readonly Spell HauntingSpirits = new Spell(157698);
    public readonly Spell ImmolateDebuff = new Spell(157736);
    public readonly Spell KilJaedensCunning = new Spell("Kil'Jaeden's Cunning");
    public readonly Spell RainofFireDebuff = new Spell(104232);
    public readonly Spell SoulburnHaunt = new Spell("Soulburn: Haunt");
    public readonly Spell SoulShards = new Spell(117198);
    public readonly Spell Soulstone = new Spell("Soulstone");
    public readonly Spell UnendingBreath = new Spell("Unending Breath");

    #endregion

    #region Offensive Spell

    public readonly Spell Agony = new Spell("Agony");
    public readonly Spell CommandDemon = new Spell("Command Demon");
    public readonly Spell Corruption = new Spell("Corruption");
    public readonly Spell DemonicServitude = new Spell("Demonic Servitude");
    public readonly Spell DrainSoul = new Spell("Drain Soul");
    public readonly Spell Haunt = new Spell("Haunt");
    public readonly Spell SeedofCorruption = new Spell("Seed of Corruption");
    public readonly Spell SoulSwap = new Spell("Soul Swap");
    public readonly Spell SummonFelhunter = new Spell("Summon Felhunter");
    public readonly Spell SummonFelImp = new Spell("Summon Fel Imp");
    public readonly Spell SummonImp = new Spell("Summon Imp");
    public readonly Spell SummonObserver = new Spell("Summon Observer");
    public readonly Spell SummonShivarra = new Spell("Summon Shivarra");
    public readonly Spell SummonSuccubus = new Spell("Summon Succubus");
    public readonly Spell SummonVoidlord = new Spell("Summon Voidlord");
    public readonly Spell SummonVoidwalker = new Spell("Summon Voidwalker");
    public readonly Spell UnstableAffliction = new Spell("Unstable Affliction");
    private Timer _unstableAfflictionTimer = new Timer(0);
    private Timer _hauntTimer = new Timer(0);

    #endregion

    #region Offensive Cooldown

    public readonly Spell Cataclysm = new Spell("Cataclysm");
    public readonly Spell DarkSoulMisery = new Spell("Dark Soul: Misery");
    public readonly Spell FireandBrimstone = new Spell("Fire and Brimstone");
    public readonly Spell GrimoireofService = new Spell("Grimoire of Service");
    public readonly Spell MannarothsFury = new Spell("Mannaroth's Fury");
    public readonly Spell Soulburn = new Spell("Soulburn");
    public readonly Spell SummonAbyssal = new Spell("Summon Abyssal");
    public readonly Spell SummonDoomguard = new Spell("Summon Doomguard");
    public readonly Spell SummonInfernal = new Spell("Summon Infernal");
    public readonly Spell SummonTerrorguard = new Spell(112927);

    #endregion

    #region Defensive Cooldown

    public readonly Spell BloodHorror = new Spell("Blood Horror");
    public readonly Spell DarkBargain = new Spell("Dark Bargain");
    public readonly Spell Fear = new Spell("Fear");
    public readonly Spell HowlofTerror = new Spell("HowlofTerror");
    public readonly Spell SacrificialPact = new Spell("Sacrificial Pact");
    public readonly Spell Shadowfury = new Spell("Shadowfury");
    public readonly Spell UnendingResolve = new Spell("Unending Resolve");
    private Timer _fearTimer = new Timer(0);

    #endregion

    #region Healing Spell

    public readonly Spell CreateHealthstone = new Spell("Create Healthstone");
    public readonly Spell DarkRegeneration = new Spell("Dark Regeneration");
    public readonly Spell DrainLife = new Spell("Drain Life");
    public readonly Spell HealthFunnel = new Spell("Health Funnel");
    public readonly Spell LifeTap = new Spell("Life Tap");
    public readonly Spell MortalCoil = new Spell("Mortal Coil");
    private Timer _healthstoneTimer = new Timer(0);

    #endregion

    public WarlockAffliction()
    {
        Main.InternalRange = 39f;
        Main.InternalAggroRange = 39f;
        MySettings = WarlockAfflictionSettings.GetSettings();
        Main.DumpCurrentSettings<WarlockAfflictionSettings>(MySettings);
        UInt128 lastTarget = 0;
        LowHP();

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
                                && (UnstableAffliction.IsHostileDistanceGood || Agony.IsHostileDistanceGood))
                            {
                                Pull();
                                lastTarget = ObjectManager.Me.Target;
                            }

                            if (MySettings.UseLowCombat && (ObjectManager.Me.Level - ObjectManager.Target.Level >= MySettings.UseLowCombatAtPercentage))
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

    private void LowHP()
    {
        if (MySettings.UseBloodHorrorAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseBloodHorrorAtPercentage;

        if (MySettings.UseDarkBargainAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseDarkBargainAtPercentage;

        if (MySettings.UseFearAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseFearAtPercentage;

        if (MySettings.UseHowlofTerrorAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseHowlofTerrorAtPercentage;

        if (MySettings.UseSacrificialPactAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseSacrificialPactAtPercentage;

        if (MySettings.UseShadowfuryAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseShadowfuryAtPercentage;

        if (MySettings.UseStoneformAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseStoneformAtPercentage;

        if (MySettings.UseUnendingResolveAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseUnendingResolveAtPercentage;

        if (MySettings.UseWarStompAtPercentage > DefenseHP)
            DefenseHP = MySettings.UseWarStompAtPercentage;

        if (MySettings.UseArcaneTorrentForDecastAtPercentage > DecastHP)
            DecastHP = MySettings.UseArcaneTorrentForDecastAtPercentage;

        if (MySettings.UseArcaneTorrentForResourceAtPercentage > HealMP)
            HealMP = MySettings.UseArcaneTorrentForResourceAtPercentage;

        if (MySettings.UseLifeTapAtPercentage > HealMP)
            HealMP = MySettings.UseLifeTapAtPercentage;

        if (MySettings.UseCreateHealthstoneAtPercentage > HealHP)
            HealHP = MySettings.UseCreateHealthstoneAtPercentage;

        if (MySettings.UseDarkRegenerationAtPercentage > HealHP)
            HealHP = MySettings.UseDarkRegenerationAtPercentage;

        if (MySettings.UseDrainLifeAtPercentage > HealHP)
            HealHP = MySettings.UseDrainLifeAtPercentage;

        if (MySettings.UseGiftoftheNaaruAtPercentage > HealHP)
            HealHP = MySettings.UseGiftoftheNaaruAtPercentage;

        if (MySettings.UseMortalCoilAtPercentage > HealHP)
            HealHP = MySettings.UseMortalCoilAtPercentage;
    }

    private void Pull()
    {
        if (ObjectManager.Pet.IsAlive)
        {
            Lua.RunMacroText("/petattack");
            Logging.WriteFight("Cast Pet Attack");
        }
        if (!Agony.TargetHaveBuff && !Corruption.TargetHaveBuff && !UnstableAffliction.TargetHaveBuff && ObjectManager.Me.SoulShards > 199)
        {
            if (MySettings.UseSoulSwap && MySettings.UseSoulburn && Soulburn.IsSpellUsable && SoulSwap.IsSpellUsable && SoulSwap.IsHostileDistanceGood)
            {
                if (!Soulburn.HaveBuff)
                {
                    Soulburn.Cast();
                    Others.SafeSleep(1000);
                }

                SoulSwap.Cast();
            }
            return;
        }
        if (MySettings.UseUnstableAffliction && _unstableAfflictionTimer.IsReady && (!UnstableAffliction.TargetHaveBuff || ObjectManager.Target.UnitAura(30108).AuraTimeLeftInMs < 4200)
            && UnstableAffliction.IsSpellUsable && UnstableAffliction.IsHostileDistanceGood)
        {
            UnstableAffliction.Cast();
            _unstableAfflictionTimer = new Timer(1000*5);
        }
    }

    private void LowCombat()
    {
        Buff();

        if (MySettings.DoAvoidMelee)
            AvoidMelee();

        if (_onCd.IsReady && ObjectManager.Me.HealthPercent <= DefenseHP)
            DefenseCycle();

        if (ObjectManager.Me.HealthPercent <= HealHP || ObjectManager.Me.ManaPercentage <= HealMP)
            Heal();

        if (MySettings.UseHaunt && !Haunt.HaveBuff && Haunt.IsSpellUsable)
        {
            Haunt.Cast();
            return;
        }
        if (MySettings.UseCommandDemon && CommandDemon.IsSpellUsable && DrainSoul.IsHostileDistanceGood)
        {
            CommandDemon.Cast();
            return;
        }
        if (MySettings.UseSeedofCorruption && SeedofCorruption.IsSpellUsable && ObjectManager.GetUnitInSpellRange(10) > 3)
        {
            SeedofCorruption.Cast();
            return;
        }
        if (MySettings.UseDrainSoul && DrainSoul.IsSpellUsable && DrainSoul.IsHostileDistanceGood)
        {
            DrainSoul.Cast();
            return;
        }
    }

    private void Combat()
    {
        Buff();

        if (MySettings.DoAvoidMelee)
            AvoidMelee();

        DPSCycle();

        if (_onCd.IsReady && ObjectManager.Me.HealthPercent <= DefenseHP)
            DefenseCycle();

        if (ObjectManager.Me.HealthPercent <= HealHP || ObjectManager.Me.ManaPercentage <= HealMP)
            Heal();

        if (ObjectManager.Me.HealthPercent <= DecastHP)
            Decast();

        DPSBurst();
        DPSCycle();
    }

    private void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        Pet();

        if (MySettings.UseBurningRush && !Darkflight.HaveBuff && !BurningRush.HaveBuff && ObjectManager.Me.GetMove && !ObjectManager.Target.InCombat && BurningRush.IsSpellUsable)
            BurningRush.Cast();

        if (MySettings.UseBurningRush && BurningRush.HaveBuff && (!ObjectManager.Me.GetMove || ObjectManager.Target.InCombat
                                                                  || ObjectManager.Me.HealthPercent < MySettings.UseBurningRushAbovePercentage) && BurningRush.IsSpellUsable)
            BurningRush.Cast();

        if (MySettings.UseCreateHealthstone && ItemsManager.GetItemCount(5512) == 0 && Usefuls.GetContainerNumFreeSlots > 0 && CreateHealthstone.IsSpellUsable)
        {
            Logging.WriteFight(" - Create Healthstone - ");
            CreateHealthstone.Cast();
            Others.SafeSleep(500);

            while (ObjectManager.Me.IsCast)
                Others.SafeSleep(200);
        }

        if (MySettings.UseDarkflight && Darkflight.KnownSpell && !BurningRush.HaveBuff && ObjectManager.Me.GetMove && !ObjectManager.Target.InCombat && Darkflight.IsSpellUsable)
            Darkflight.Cast();

        if (MySettings.UseDarkIntent && !DarkIntent.HaveBuff && DarkIntent.IsSpellUsable)
            DarkIntent.Cast();

        if (MySettings.UseSoulstone && !Soulstone.HaveBuff && Soulstone.IsSpellUsable)
        {
            Soulstone.Cast();
            Others.SafeSleep(500);

            while (ObjectManager.Me.IsCast)
                Others.SafeSleep(20);
        }

        if (MySettings.UseUnendingBreath && ObjectManager.Me.UnitAura(5697).AuraTimeLeftInMs < 1 && UnendingBreath.IsSpellUsable)
            UnendingBreath.Cast();

        if (MySettings.UseAlchFlask && !ObjectManager.Me.HaveBuff(79638) && !ObjectManager.Me.HaveBuff(79640) && !ObjectManager.Me.HaveBuff(79639)
            && !ItemsManager.IsItemOnCooldown(75525) && ItemsManager.GetItemCount(75525) > 0)
            ItemsManager.UseItem(75525);
    }

    private void Pet()
    {
        if (MySettings.UseSummonDoomguard && DemonicServitude.KnownSpell && (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0) && !GrimoireofSacrifice.HaveBuff
            && (SummonDoomguard.IsSpellUsable || SummonTerrorguard.IsSpellUsable))
        {
            Logging.WriteFight(" - PET DEAD - ");
            if (GrimoireofSupremacy.KnownSpell)
                SummonTerrorguard.Cast();
            else
                SummonDoomguard.Cast();
            Others.SafeSleep(500);

            while (ObjectManager.Me.IsCast)
                Others.SafeSleep(20);
        }
        else if (MySettings.UseSummonInfernal && DemonicServitude.KnownSpell && (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0) && !GrimoireofSacrifice.HaveBuff
                 && (SummonInfernal.IsSpellUsable || SummonAbyssal.IsSpellUsable))
        {
            Logging.WriteFight(" - PET DEAD - ");
            if (GrimoireofSupremacy.KnownSpell)
                SpellManager.CastSpellByIDAndPosition(140763, ObjectManager.Target.Position);
            else
                SpellManager.CastSpellByIDAndPosition(1122, ObjectManager.Target.Position);
            Others.SafeSleep(500);

            while (ObjectManager.Me.IsCast)
                Others.SafeSleep(20);
        }
        else if (MySettings.UseSummonFelhunter && (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0) && !GrimoireofSacrifice.HaveBuff
                 && (SummonFelhunter.IsSpellUsable || SummonObserver.IsSpellUsable))
        {
            Logging.WriteFight(" - PET DEAD - ");
            if (GrimoireofSupremacy.KnownSpell)
                SummonObserver.Cast();
            else
                SummonFelhunter.Cast();
            Others.SafeSleep(500);

            while (ObjectManager.Me.IsCast)
                Others.SafeSleep(20);
        }
        else if (MySettings.UseSummonImp && (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0) && !GrimoireofSacrifice.HaveBuff
                 && (SummonImp.IsSpellUsable || SummonFelImp.IsSpellUsable))
        {
            Logging.WriteFight(" - PET DEAD - ");
            if (GrimoireofSupremacy.KnownSpell)
                SummonFelImp.Cast();
            else
                SummonImp.Cast();
            Others.SafeSleep(500);

            while (ObjectManager.Me.IsCast)
                Others.SafeSleep(20);
        }
        else if (MySettings.UseSummonVoidwalker && (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0) && !GrimoireofSacrifice.HaveBuff
                 && (SummonVoidwalker.IsSpellUsable || SummonVoidlord.IsSpellUsable))
        {
            Logging.WriteFight(" - PET DEAD - ");
            if (GrimoireofSupremacy.KnownSpell)
                SummonVoidlord.Cast();
            else
                SummonVoidwalker.Cast();
            Others.SafeSleep(500);

            while (ObjectManager.Me.IsCast)
                Others.SafeSleep(20);
        }
        else if (MySettings.UseSummonSuccubus && (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0) && !GrimoireofSacrifice.HaveBuff
                 && (SummonSuccubus.IsSpellUsable || SummonShivarra.IsSpellUsable))
        {
            Logging.WriteFight(" - PET DEAD - ");
            if (GrimoireofSupremacy.KnownSpell)
                SummonShivarra.Cast();
            else
                SummonSuccubus.Cast();
            Others.SafeSleep(500);

            while (ObjectManager.Me.IsCast)
                Others.SafeSleep(20);
        }

        if (MySettings.UseGrimoireofSacrifice && !GrimoireofSacrifice.HaveBuff && (ObjectManager.Pet.Health != 0 || ObjectManager.Pet.Guid != 0) && GrimoireofSacrifice.IsSpellUsable)
            GrimoireofSacrifice.Cast();
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
        if (MySettings.UseEveryManforHimself && ObjectManager.Me.IsStunned && EveryManforHimself.IsSpellUsable)
            EveryManforHimself.Cast();

        if (MySettings.UseBloodHorror && ObjectManager.Me.HealthPercent <= MySettings.UseBloodHorrorAtPercentage && BloodHorror.IsSpellUsable && ObjectManager.Target.GetDistance < 6)
        {
            BloodHorror.Cast();
            return;
        }
        if (MySettings.UseDarkBargain && ObjectManager.Me.HealthPercent <= MySettings.UseDarkBargainAtPercentage && DarkBargain.IsSpellUsable)
        {
            DarkBargain.Cast();
            _onCd = new Timer(1000*8);
            return;
        }
        if (MySettings.UseFear && ObjectManager.Me.HealthPercent <= MySettings.UseFearAtPercentage && _fearTimer.IsReady && Fear.IsSpellUsable)
        {
            Fear.Cast();
            _fearTimer = new Timer(1000*10);
            _onCd = new Timer(1000*2);
            return;
        }
        if (MySettings.UseHowlofTerror && ObjectManager.Me.HealthPercent <= MySettings.UseHowlofTerrorAtPercentage && HowlofTerror.IsSpellUsable
            && ObjectManager.GetUnitInSpellRange(HowlofTerror.MaxRangeHostile) > 0)
        {
            HowlofTerror.Cast();
            return;
        }
        if (MySettings.UseMeteorStrike && MySettings.UseSummonInfernal && ObjectManager.Me.HealthPercent <= MySettings.UseMeteorStrikeAtPercentage
            && (ObjectManager.Pet.Health != 0 || ObjectManager.Pet.Guid != 0) && CommandDemon.IsSpellUsable)
        {
            CommandDemon.Cast();
            return;
        }
        if (MySettings.UseSacrificialPact && ObjectManager.Me.HealthPercent <= MySettings.UseSacrificialPactAtPercentage && (ObjectManager.Pet.Health != 0 || ObjectManager.Pet.Guid != 0)
            && SacrificialPact.IsSpellUsable)
        {
            SacrificialPact.Cast();
            _onCd = new Timer(1000*10);
            return;
        }
        if (MySettings.UseShadowfury && ObjectManager.Me.HealthPercent <= MySettings.UseShadowfuryAtPercentage && Shadowfury.IsSpellUsable
            && Shadowfury.IsHostileDistanceGood)
        {
            SpellManager.CastSpellByIDAndPosition(30283, ObjectManager.Target.Position);
            _onCd = new Timer(1000*3);
            return;
        }
        if (MySettings.UseStoneform && ObjectManager.Me.HealthPercent <= MySettings.UseStoneformAtPercentage && Stoneform.IsSpellUsable)
        {
            Stoneform.Cast();
            _onCd = new Timer(1000*8);
            return;
        }
        if (MySettings.UseUnendingResolve && ObjectManager.Me.HealthPercent <= MySettings.UseUnendingResolveAtPercentage && UnendingResolve.IsSpellUsable)
        {
            UnendingResolve.Cast();
            _onCd = new Timer(1000*8);
            return;
        }
        if (MySettings.UseWarStomp && ObjectManager.Me.HealthPercent <= MySettings.UseWarStompAtPercentage && WarStomp.IsSpellUsable
            && ObjectManager.GetUnitInSpellRange(WarStomp.MaxRangeHostile) > 0)
        {
            WarStomp.Cast();
            _onCd = new Timer(1000*2);
            return;
        }
        if (MySettings.UseWhiplash && MySettings.UseSummonSuccubus && ObjectManager.Me.HealthPercent <= MySettings.UseWhiplashAtPercentage && CommandDemon.IsSpellUsable
            && ObjectManager.Target.GetDistance < 6)
        {
            CommandDemon.Cast();
            _onCd = new Timer(1000*2);
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (MySettings.UseCauterizeMaster && MySettings.UseSummonImp && ObjectManager.Me.HealthPercent <= MySettings.UseCauterizeMasterAtPercentage
            && (ObjectManager.Pet.Health != 0 || ObjectManager.Pet.Guid != 0) && CommandDemon.IsSpellUsable)
        {
            CommandDemon.Cast();
            return;
        }
        if (MySettings.UseCreateHealthstone && _healthstoneTimer.IsReady && ObjectManager.Me.HealthPercent <= MySettings.UseCreateHealthstoneAtPercentage
            && ItemsManager.GetItemCount(5512) > 0)
        {
            Logging.WriteFight("Use Healthstone.");
            ItemsManager.UseItem("Healthstone");
            _healthstoneTimer = new Timer(1000*60);
            return;
        }
        if (MySettings.UseDarkRegeneration && ObjectManager.Me.HealthPercent <= MySettings.UseDarkRegenerationAtPercentage && DarkRegeneration.IsSpellUsable)
        {
            DarkRegeneration.Cast();
            return;
        }
        if (MySettings.UseDrainLife && DrainLife.KnownSpell && DrainLife.IsHostileDistanceGood && DrainLife.IsSpellUsable
            && ObjectManager.Me.HealthPercent <= MySettings.UseDrainLifeAtPercentage)
        {
            DrainLife.Launch(true, false, true);
            while (ObjectManager.Me.IsCast && ObjectManager.Pet.HealthPercent < 96)
                Others.SafeSleep(20);

            if (ObjectManager.Me.IsCast)
                ObjectManager.Me.StopCast();
            return;
        }
        if (MySettings.UseGiftoftheNaaru && ObjectManager.Me.HealthPercent <= MySettings.UseGiftoftheNaaruAtPercentage && GiftoftheNaaru.IsSpellUsable)
        {
            GiftoftheNaaru.Cast();
            return;
        }
        if (MySettings.UseMortalCoil && ObjectManager.Me.HealthPercent <= MySettings.UseMortalCoilAtPercentage && MortalCoil.IsSpellUsable && MortalCoil.IsHostileDistanceGood)
        {
            MortalCoil.Cast();
            return;
        }
        if (MySettings.UseArcaneTorrentForResource && ObjectManager.Me.ManaPercentage <= MySettings.UseArcaneTorrentForResourceAtPercentage && ArcaneTorrent.IsSpellUsable)
        {
            ArcaneTorrent.Cast();
            return;
        }
        if (MySettings.UseLifeTap && ObjectManager.Me.ManaPercentage <= MySettings.UseLifeTapAtPercentage && LifeTap.IsSpellUsable)
            LifeTap.Cast();
    }

    private void Decast()
    {
        if (MySettings.UseArcaneTorrentForDecast && ObjectManager.Me.HealthPercent <= MySettings.UseArcaneTorrentForDecastAtPercentage
            && ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && ArcaneTorrent.IsSpellUsable && ObjectManager.Target.GetDistance < 8)
        {
            ArcaneTorrent.Cast();
            return;
        }
        if (MySettings.UseShadowLock && MySettings.UseSummonDoomguard && ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe
            && (ObjectManager.Pet.Health != 0 || ObjectManager.Pet.Guid != 0) && CommandDemon.IsSpellUsable && DrainSoul.IsHostileDistanceGood)
        {
            CommandDemon.Cast();
            return;
        }

        if (MySettings.UseSpellLock && MySettings.UseSummonFelhunter && ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe
            && (ObjectManager.Pet.Health != 0 || ObjectManager.Pet.Guid != 0) && CommandDemon.IsSpellUsable && DrainSoul.IsHostileDistanceGood)
            CommandDemon.Cast();
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
        if (MySettings.UseBerserking && Berserking.IsSpellUsable && DrainSoul.IsHostileDistanceGood)
            Berserking.Cast();

        if (MySettings.UseBloodFury && BloodFury.IsSpellUsable && DrainSoul.IsHostileDistanceGood)
            BloodFury.Cast();

        if (MySettings.UseDarkSoulMisery && !DarkSoulMisery.HaveBuff && DarkSoulMisery.IsSpellUsable && DrainSoul.IsHostileDistanceGood)
            DarkSoulMisery.Cast();

        if (MySettings.UseSummonDoomguard && !DemonicServitude.KnownSpell && (SummonDoomguard.IsSpellUsable || SummonTerrorguard.IsSpellUsable)
            && ObjectManager.GetUnitInSpellRange(10) < 8)
        {
            if (GrimoireofSupremacy.KnownSpell)
                SummonTerrorguard.Cast();
            else
                SummonDoomguard.Cast();
        }
        if (MySettings.UseSummonInfernal && !DemonicServitude.KnownSpell && (SummonInfernal.IsSpellUsable || SummonAbyssal.IsSpellUsable)
            && ObjectManager.GetUnitInSpellRange(10) > 7)
        {
            if (GrimoireofSupremacy.KnownSpell)
                SpellManager.CastSpellByIDAndPosition(140763, ObjectManager.Target.Position);
            else
                SpellManager.CastSpellByIDAndPosition(1122, ObjectManager.Target.Position);
        }

        if (MySettings.UseGrimoireofService && GrimoireofService.IsSpellUsable && DrainSoul.IsHostileDistanceGood)
            GrimoireofService.Cast();
    }

    private void DPSCycle()
    {
        Usefuls.SleepGlobalCooldown();
        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            if (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0)
            {
                if (GrimoireofSacrifice.KnownSpell && GrimoireofSacrifice.HaveBuff)
                    Others.SafeSleep(20);
                else
                    Pet();
            }

            if (MySettings.UseKilJaedensCunning && !KilJaedensCunning.HaveBuff && ObjectManager.Me.GetMove && KilJaedensCunning.IsSpellUsable && DrainSoul.IsHostileDistanceGood)
                KilJaedensCunning.Cast();

            if (MySettings.UseMannarothsFury && MannarothsFury.IsSpellUsable && ObjectManager.GetUnitInSpellRange(8) > 3)
                MannarothsFury.Cast();


            if (MySettings.UseCataclysm && Cataclysm.IsSpellUsable && Cataclysm.IsHostileDistanceGood && ObjectManager.GetUnitInSpellRange(10) > 3)
            {
                SpellManager.CastSpellByIDAndPosition(152108, ObjectManager.Target.Position);
                return;
            }
            if (MySettings.UseAgony && (!Agony.TargetHaveBuff || ObjectManager.Target.UnitAura(980).AuraTimeLeftInMs < 7200) && Agony.IsSpellUsable
                && Agony.IsHostileDistanceGood)
            {
                Agony.Cast();
                return;
            }
            if (MySettings.UseCorruption && (!CorruptionDebuff.TargetHaveBuff || ObjectManager.Target.UnitAura(146739).AuraTimeLeftInMs < 5400) && Corruption.IsSpellUsable
                && Corruption.IsHostileDistanceGood)
            {
                if (MySettings.UseSoulburn && ObjectManager.GetUnitInSpellRange(10) > 3)
                {
                    Soulburn.Cast();
                    Others.SafeSleep(1000);
                    SeedofCorruption.Cast();
                }
                else
                    Corruption.Cast();
                return;
            }
            if (MySettings.UseUnstableAffliction && _unstableAfflictionTimer.IsReady && (!UnstableAffliction.TargetHaveBuff || ObjectManager.Target.UnitAura(30108).AuraTimeLeftInMs < 4200)
                && UnstableAffliction.IsSpellUsable && UnstableAffliction.IsHostileDistanceGood)
            {
                UnstableAffliction.Cast();
                _unstableAfflictionTimer = new Timer(1000*5);
                return;
            }

            if (MySettings.UseHaunt && SoulburnHaunt.KnownSpell && Haunt.IsSpellUsable && Haunt.IsHostileDistanceGood)
            {
                if (MySettings.UseSoulburn && ObjectManager.Me.SoulShards > 199 && (!HauntingSpirits.HaveBuff || ObjectManager.Target.UnitAura(157698).AuraTimeLeftInMs < 9000)
                    && Soulburn.IsSpellUsable)
                {
                    Soulburn.Cast();
                    Others.SafeSleep(1000);
                }
                if (Soulburn.HaveBuff || ObjectManager.Me.SoulShards > 399 || (ObjectManager.Me.SoulShards > 299 && DarkSoulMisery.HaveBuff))
                    Haunt.Cast();
                return;
            }
            else if (MySettings.UseHaunt && _hauntTimer.IsReady && ((!Haunt.TargetHaveBuff && (ObjectManager.Me.SoulShards > 299 || DarkSoulMisery.HaveBuff))
                                                                    || ObjectManager.Me.SoulShards > 399) && Haunt.IsSpellUsable && Haunt.IsHostileDistanceGood)
            {
                Haunt.Cast();
                _hauntTimer = new Timer(1000*3);
                return;
            }

            if (MySettings.UseSeedofCorruption && !SeedofCorruption.TargetHaveBuff && SeedofCorruption.IsSpellUsable && ObjectManager.GetUnitInSpellRange(10) > 3)
            {
                SeedofCorruption.Cast();
                return;
            }
            if (MySettings.UseDrainSoul && DrainSoul.IsSpellUsable && DrainSoul.IsHostileDistanceGood && ObjectManager.GetUnitInSpellRange(10) < 4)
            {
                DrainSoul.Cast(true, false, true);
                Others.SafeSleep(500);
                while (ObjectManager.Me.IsCast && Agony.TargetHaveBuff && CorruptionDebuff.TargetHaveBuff && UnstableAffliction.TargetHaveBuff
                       && ObjectManager.Me.SoulShards < 400)
                    Others.SafeSleep(20);

                if (ObjectManager.Me.IsCast)
                    ObjectManager.Me.StopCast();
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

    #region Nested type: WarlockAfflictionSettings

    [Serializable]
    public class WarlockAfflictionSettings : Settings
    {
        public bool DoAvoidMelee = false;
        public int DoAvoidMeleeDistance = 0;
        public bool UseAgony = true;
        public bool UseAlchFlask = true;
        public bool UseArcaneTorrentForDecast = true;
        public int UseArcaneTorrentForDecastAtPercentage = 95;
        public bool UseArcaneTorrentForResource = true;
        public int UseArcaneTorrentForResourceAtPercentage = 80;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseBloodHorror = true;
        public int UseBloodHorrorAtPercentage = 20;
        public bool UseBurningRush = true;
        public int UseBurningRushAbovePercentage = 50;
        public bool UseCataclysm = true;
        public bool UseCauterizeMaster = true;
        public int UseCauterizeMasterAtPercentage = 90;
        public bool UseCommandDemon = true;
        public bool UseCorruption = true;
        public bool UseCreateHealthstone = true;
        public int UseCreateHealthstoneAtPercentage = 80;
        public bool UseDarkBargain = true;
        public int UseDarkBargainAtPercentage = 60;
        public bool UseDarkflight = true;
        public bool UseDarkIntent = true;
        public bool UseDarkRegeneration = true;
        public int UseDarkRegenerationAtPercentage = 70;
        public bool UseDarkSoulMisery = true;
        public bool UseDrainLife = true;
        public int UseDrainLifeAtPercentage = 70;
        public bool UseDrainSoul = true;
        public bool UseEveryManforHimself = true;
        public bool UseFear = true;
        public int UseFearAtPercentage = 20;
        public bool UseGiftoftheNaaru = true;
        public int UseGiftoftheNaaruAtPercentage = 80;
        public bool UseGrimoireofSacrifice = true;
        public bool UseGrimoireofService = true;
        public bool UseHaunt = true;
        public bool UseHealthFunnel = true;
        public int UseHealthFunnelAtPercentage = 50;
        public bool UseHowlofTerror = true;
        public int UseHowlofTerrorAtPercentage = 20;
        public bool UseKilJaedensCunning = true;
        public bool UseLifeTap = true;
        public int UseLifeTapAtPercentage = 70;
        public bool UseLowCombat = true;
        public int UseLowCombatAtPercentage = 15;
        public bool UseMannarothsFury = true;
        public bool UseMeteorStrike = true;
        public int UseMeteorStrikeAtPercentage = 85;
        public bool UseMortalCoil = true;
        public int UseMortalCoilAtPercentage = 85;
        public bool UseSacrificialPact = true;
        public int UseSacrificialPactAtPercentage = 70;
        public bool UseSeedofCorruption = true;
        public bool UseShadowfury = true;
        public int UseShadowfuryAtPercentage = 90;
        public bool UseShadowLock = true;
        public int UseShadowLockAtPercentage = 95;
        public bool UseSoulburn = true;
        public bool UseSoulstone = true;
        public bool UseSoulSwap = true;
        public bool UseSpellLock = true;
        public int UseSpellLockAtPercentage = 95;
        public bool UseStoneform = true;
        public int UseStoneformAtPercentage = 80;
        public bool UseSummonDoomguard = true;
        public bool UseSummonFelhunter = true;
        public bool UseSummonImp = false;
        public bool UseSummonInfernal = false;
        public bool UseSummonSuccubus = false;
        public bool UseSummonVoidwalker = false;
        public bool UseTrinketOne = true;
        public bool UseTrinketTwo = true;
        public bool UseUnendingBreath = false;
        public bool UseUnendingResolve = true;
        public int UseUnendingResolveAtPercentage = 70;
        public bool UseUnstableAffliction = true;
        public bool UseWarStomp = true;
        public int UseWarStompAtPercentage = 80;
        public bool UseWhiplash = true;
        public int UseWhiplashAtPercentage = 85;

        public WarlockAfflictionSettings()
        {
            ConfigWinForm("Warlock Affliction Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent for Interrupt", "UseArcaneTorrentForDecast", "Professions & Racials", "AtPercentage");
            AddControlInWinForm("Use Arcane Torrent for Resource", "UseArcaneTorrentForResource", "Professions & Racials", "AtPercentage");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Darkflight", "UseDarkflight", "Professions & Racials");
            AddControlInWinForm("Use Every Man for Himself", "UseEveryManforHimself", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials", "AtPercentage");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            /* Warlock Buffs */
            AddControlInWinForm("Use Dark Intent", "UseDarkIntent", "Warlock Buffs");
            AddControlInWinForm("Use Grimoire of Sacrifice", "UseGrimoireofSacrifice", "Warlock Buffs");
            AddControlInWinForm("Use Soulstone", "UseSoulstone", "Warlock Buffs");
            AddControlInWinForm("Use Unending Breath", "UseUnendingBreath", "Warlock Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Agony", "UseAgony", "Offensive Spell");
            AddControlInWinForm("Use Command Demon", "UseCommandDemon", "Offensive Spell");
            AddControlInWinForm("Use Corruption", "UseCorruption", "Offensive Spell");
            AddControlInWinForm("Use Drain Soul", "UseDrainSoul", "Offensive Spell");
            AddControlInWinForm("Use Haunt", "UseHaunt", "Offensive Spell");
            AddControlInWinForm("Use Seed of Corruption", "UseSeedofCorruption", "Offensive Spell");
            AddControlInWinForm("Use Soul Swap", "UseSoulSwap", "Offensive Spell");
            AddControlInWinForm("Use Soulburn", "UseSoulburn", "Offensive Spell");
            AddControlInWinForm("Use Summon Imp", "UseSummonImp", "Offensive Spell");
            AddControlInWinForm("Use Summon Voidwalker", "UseSummonVoidwalker", "Offensive Spell");
            AddControlInWinForm("Use Summon Felhunter", "UseSummonFelhunter", "Offensive Spell");
            AddControlInWinForm("Use Summon Succubus", "UseSummonSuccubus", "Offensive Spell");
            AddControlInWinForm("Use Unstable Affliction", "UseUnstableAffliction", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use Cataclysm", "UseCataclysm", "Offensive Cooldown");
            AddControlInWinForm("Use Dark Soul: Misery", "UseDarkSoulMisery", "Offensive Cooldown");
            AddControlInWinForm("Use Grimoire of Service", "UseGrimoireofService", "Offensive Cooldown");
            AddControlInWinForm("Use Kil'Jaeden's Cunning", "UseKilJaedensCunning", "Offensive Cooldown");
            AddControlInWinForm("Use Mannaroth's Fury", "UseMannarothsFury", "Offensive Cooldown");
            AddControlInWinForm("Use Summon Doomguard", "UseSummonDoomguard", "Offensive Cooldown");
            AddControlInWinForm("Use Summon Infernal", "UseSummonInfernal", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Blood Terror", "UseBloodTerror", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Burning Rush", "UseBurningRush", "Defensive Cooldown", "AbovePercentage");
            AddControlInWinForm("Use Dark Bargain", "UseDarkBargain", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Fear", "UseFear", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Howl of Terror", "UseHowlofTerror", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Meteor Strike", "UseMeteorStrike", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Sacrificial Pact", "UseSacrificialPact", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Shadowfury", "UseShadowfury", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Shadow Lock", "UseShadowLock", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Spell Lock", "UseSpellLock", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Unending Resolve", "UseUnendingResolve", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Whiplash", "UseWhiplash", "Defensive Cooldown", "AtPercentage");
            /* Healing Spell */
            AddControlInWinForm("Use Cauterize Master", "UseCauterizeMaster", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Create Healthstone", "UseCreateHealthstone", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Dark Regeneration", "UseDarkRegeneration", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Drain Life", "UseDrainLife", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Health Funnel", "UseHealthFunnel", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Life Tap", "UseLifeTap", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Mortal Coil", "UseMortalCoil", "Healing Spell", "AtPercentage");
            /* Game Settings */
            AddControlInWinForm("Use Low Combat Settings - Level Difference", "UseLowCombat", "Game Settings", "AtPercentage");
            AddControlInWinForm("Use Trinket One", "UseTrinketOne", "Game Settings");
            AddControlInWinForm("Use Trinket Two", "UseTrinketTwo", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
            AddControlInWinForm("Do avoid melee (Off Advised!!)", "DoAvoidMelee", "Game Settings");
            AddControlInWinForm("Avoid melee distance (1 to 4)", "DoAvoidMeleeDistance", "Game Settings");
        }

        public static WarlockAfflictionSettings CurrentSetting { get; set; }

        public static WarlockAfflictionSettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Warlock_Affliction.xml";
            if (File.Exists(currentSettingsFile))
            {
                return
                    CurrentSetting = Load<WarlockAfflictionSettings>(currentSettingsFile);
            }
            return new WarlockAfflictionSettings();
        }
    }

    #endregion
}

#endregion

// ReSharper restore ObjectCreationAsStatement
// ReSharper restore EmptyGeneralCatchClause