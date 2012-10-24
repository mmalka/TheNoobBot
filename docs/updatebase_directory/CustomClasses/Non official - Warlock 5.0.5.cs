/*
* CustomClass for TheNoobBot
* Credit : Rival, Geesus, Enelya, Marstor, Vesper, Neo2003, DreadLocks
* Thanks you !
*/

using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;
using Keybindings = nManager.Wow.Enums.Keybindings;
using Timer = nManager.Helpful.Timer;

public class Main : ICustomClass
{
    internal static float range = 30f;
    internal static bool loop = true;

    public float Range
    {
        get { return range; }
        set { range = value; }
    }

    public void Initialize()
    {
        Initialize(false);
    }

    public void Initialize(bool ConfigOnly)
    {
        try
        {
            Logging.WriteFight("Loading combat system.");

            switch (ObjectManager.Me.WowClass)
            {
                    #region Warlock Specialisation checking

                case WoWClass.Warlock:
                    var Warlock_Demonology_Spell = new Spell("Summon Felguard");
                    var Warlock_Affliction_Spell = new Spell("Unstable Affliction");
                    var Warlock_Destruction_Spell = new Spell("Conflagrate");

                    if (Warlock_Demonology_Spell.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Warlock_Demonology.xml";
                            Warlock_Demonology.WarlockDemonologySettings CurrentSetting;
                            CurrentSetting = new Warlock_Demonology.WarlockDemonologySettings();
                            if (System.IO.File.Exists(CurrentSettingsFile))
                            {
                                CurrentSetting =
                                    Settings.Load<Warlock_Demonology.WarlockDemonologySettings>(CurrentSettingsFile);
                            }
                            CurrentSetting.ToForm();
                            CurrentSetting.Save(CurrentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Warlock Demonology class...");
                            new Warlock_Demonology();
                        }
                        break;
                    }

                    else if (Warlock_Affliction_Spell.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Warlock_Affliction.xml";
                            Warlock_Affliction.WarlockAfflictionSettings CurrentSetting;
                            CurrentSetting = new Warlock_Affliction.WarlockAfflictionSettings();
                            if (System.IO.File.Exists(CurrentSettingsFile))
                            {
                                CurrentSetting =
                                    Settings.Load<Warlock_Affliction.WarlockAfflictionSettings>(CurrentSettingsFile);
                            }
                            CurrentSetting.ToForm();
                            CurrentSetting.Save(CurrentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Warlock Affliction class...");
                            new Warlock_Affliction();
                        }
                        break;
                    }

                    else if (Warlock_Destruction_Spell.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Warlock_Destruction.xml";
                            Warlock_Destruction.WarlockDestructionSettings CurrentSetting;
                            CurrentSetting = new Warlock_Destruction.WarlockDestructionSettings();
                            if (System.IO.File.Exists(CurrentSettingsFile))
                            {
                                CurrentSetting =
                                    Settings.Load<Warlock_Destruction.WarlockDestructionSettings>(CurrentSettingsFile);
                            }
                            CurrentSetting.ToForm();
                            CurrentSetting.Save(CurrentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Warlock Destruction class...");
                            new Warlock_Destruction();
                        }
                        break;
                    }

                    else
                    {
                        if (ConfigOnly)
                        {
                            MessageBox.Show("There is no settings available for your Class/Specialisation.");
                        }
                        else
                        {
                            Logging.WriteFight("Warlock without Spec");
                            new Warlock();
                        }
                        break;
                    }

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

    public void Dispose()
    {
        Logging.WriteFight("Combat system stopped.");
        loop = false;
    }

    public void ShowConfiguration()
    {
        Directory.CreateDirectory(Application.StartupPath + "\\CustomClasses\\Settings\\");
        Initialize(true);
    }
}

#region Warlock

public class Warlock_Demonology
{
    [Serializable]
    public class WarlockDemonologySettings : nManager.Helpful.Settings
    {
        /* Professions & Racials */
        public bool UseArcaneTorrent = true;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseLifeblood = true;
        public bool UseStoneform = true;
        public bool UseGiftoftheNaaru = true;
        public bool UseWarStomp = true;
        /* Warlock Buffs */
        public bool UseCurseofEnfeeblement = false;
        public bool UseCurseoftheElements = true;
        public bool UseDarkIntent = true;
        public bool UseGrimoireofSacrifice = true;
        public bool UseMetamorphosis = true;
        public bool UseSoulLink = true;
        public bool UseSoulstone = true;
        /* Offensive Spell */
        public bool UseCarrionSwarm = true;
        public bool UseCommandDemon = true;
        public bool UseCorruption = true;
        public bool UseDoom = true;
        public bool UseFelFlame = true;
        public bool UseHandofGuldan = true;
        public bool UseHarvestLife = true;
        public bool UseHellfire = true;
        public bool UseImmolationAura = true;
        public bool UseShadowBolt = true;
        public bool UseSoulFire = true;
        public bool UseSummonImp = false;
        public bool UseSummonVoidwalker = false;
        public bool UseSummonFelhunter = false;
        public bool UseSummonSuccubus = false;
        public bool UseSummonFelguard = true;
        public bool UseTouchofChaos = true;
        public bool UseVoidRay = true;
        /* Offensive Cooldown */
        public bool UseArchimondesVengeance = true;
        public bool UseDarkSoul = true;
        public bool UseGrimoireofService = true;
        public bool UseSummonDoomguard = true;
        public bool UseSummonInfernal = false;
        /* Defensive Cooldown */
        public bool UseDarkBargain = true;
        public bool UseHowlofTerror = true;
        public bool UseSacrificialPact = true;
        public bool UseShadowfury = true;
        public bool UseTwilightWard = true;
        public bool UseUnboundWill = true;
        public bool UseUnendingResolve = true;
        /* Healing Spell */
        public bool UseCreateHealthstone = true;
        public bool UseDarkRegeneration = true;
        public bool UseDrainLife = true;
        public bool UseHealthFunnel = true;
        public bool UseLifeTap = true;
        public bool UseMortalCoil = true;
        /* Game Settings */
        public bool UseLowCombat = true;
        public bool UseTrinket = true;
        public bool UseEngGlove = true;
        public bool UseAlchFlask = true;

        public WarlockDemonologySettings()
        {
            ConfigWinForm(new System.Drawing.Point(400, 400), "Warlock Demonology Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");
            AddControlInWinForm("Use Lifeblood", "UseLifeblood", "Professions & Racials");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            /* Warlock Buffs */
            AddControlInWinForm("Use Curse of Enfeeblement", "UseCurseofEnfeeblement", "Warlock Buffs");
            AddControlInWinForm("Use Curse of the Elements", "UseCurseoftheElements", "Warlock Buffs");
            AddControlInWinForm("Use Dark Intent", "UseDarkIntent", "Warlock Buffs");
            AddControlInWinForm("Use Grimoire of Sacrifice", "UseGrimoireofSacrifice", "Warlock Buffs");
            AddControlInWinForm("Use Metamorphosis", "UseMetamorphosis", "Warlock Buffs");
            AddControlInWinForm("Use Soul Link ", "UseSoulLink ", "Warlock Buffs");
            AddControlInWinForm("Use Soulstone", "UseSoulstone", "Warlock Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Carrion Swarm", "UseCarrionSwarm", "Offensive Spell");
            AddControlInWinForm("Use Command Demon", "UseCommandDemon", "Offensive Spell");
            AddControlInWinForm("Use Corruption", "UseCorruption", "Offensive Spell");
            AddControlInWinForm("Use Doom", "UseDoom", "Offensive Spell");
            AddControlInWinForm("Use Fel Flame", "UseFelFlame", "Offensive Spell");
            AddControlInWinForm("Use Hand of Guldan", "UseHandofGuldan", "Offensive Spell");
            AddControlInWinForm("Use Harvest Life", "UseHarvestLife", "Offensive Spell");
            AddControlInWinForm("Use Hellfire", "UseHellfire", "Offensive Spell");
            AddControlInWinForm("Use Immolation Aura", "UseImmolationAura", "Offensive Spell");
            AddControlInWinForm("Use Shadow Bolt", "UseShadowBolt", "Offensive Spell");
            AddControlInWinForm("Use Soul Fire", "UseSoulFire", "Offensive Spell");
            AddControlInWinForm("Use Summon Imp", "UseSummonImp", "Offensive Spell");
            AddControlInWinForm("Use Summon Voidwalker", "UseSummonVoidwalker", "Offensive Spell");
            AddControlInWinForm("Use Summon Felhunter", "UseSummonFelhunter", "Offensive Spell");
            AddControlInWinForm("Use Summon Succubus", "UseSummonSuccubus", "Offensive Spell");
            AddControlInWinForm("Use Summon Felguard", "UseSummonFelguard", "Offensive Spell");
            AddControlInWinForm("Use Touch of Chaos", "UseTouchofChaos", "Offensive Spell");
            AddControlInWinForm("Use Void Ray", "UseVoidRay", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use Archimonde's Vengeance", "UseArchimondesVengeance", "Offensive Cooldown");
            AddControlInWinForm("Use Dark Soul", "UseDarkSoul", "Offensive Cooldown");
            AddControlInWinForm("Use Grimoire of Service", "UseGrimoireofService", "Offensive Cooldown");
            AddControlInWinForm("Use Summon Doomguard", "UseSummonDoomguard", "Offensive Cooldown");
            AddControlInWinForm("Use Summon Infernal", "UseSummonInfernal", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Dark Bargain", "UseDarkBargain", "Defensive Cooldown");
            AddControlInWinForm("Use Howl of Terror", "UseHowlofTerror", "Defensive Cooldown");
            AddControlInWinForm("Use Sacrificial Pact", "UseSacrificialPact", "Defensive Cooldown");
            AddControlInWinForm("Use Shadowfury", "UseShadowfury", "Defensive Cooldown");
            AddControlInWinForm("Use Twilight Ward", "UseTwilightWard", "Defensive Cooldown");
            AddControlInWinForm("Use Unbound Will", "UseUnboundWill", "Defensive Cooldown");
            AddControlInWinForm("Use Unending Resolve", "UseUnendingResolve", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Create Healthstone", "UseCreateHealthstone", "Healing Spell");
            AddControlInWinForm("Use Dark Regeneration", "UseDarkRegeneration", "Healing Spell");
            AddControlInWinForm("Use Drain Life", "UseDrainLife", "Healing Spell");
            AddControlInWinForm("Use Health Funnel", "UseHealthFunnel", "Healing Spell");
            AddControlInWinForm("Use Life Tap", "UseLifeTap", "Healing Spell");
            AddControlInWinForm("Use Mortal Coil", "UseMortalCoil", "Healing Spell");
            /* Game Settings */
            AddControlInWinForm("Use Low Combat Settings", "UseLowCombat", "Game Settings");
            AddControlInWinForm("Use Trinket", "UseTrinket", "Game Settings");
            AddControlInWinForm("Use Engineering Gloves", "UseEngGlove", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
        }

        public static WarlockDemonologySettings CurrentSetting { get; set; }

        public static WarlockDemonologySettings GetSettings()
        {
            string CurrentSettingsFile = Application.StartupPath + "\\CustomClasses\\Settings\\Warlock_Demonology.xml";
            if (System.IO.File.Exists(CurrentSettingsFile))
            {
                return
                    CurrentSetting = Settings.Load<Warlock_Demonology.WarlockDemonologySettings>(CurrentSettingsFile);
            }
            else
            {
                return new Warlock_Demonology.WarlockDemonologySettings();
            }
        }
    }

    private readonly WarlockDemonologySettings MySettings = WarlockDemonologySettings.GetSettings();

    #region Professions & Racials

    private readonly Spell Arcane_Torrent = new Spell("Arcane Torrent");
    private readonly Spell Berserking = new Spell("Berserking");
    private readonly Spell Blood_Fury = new Spell("Blood Fury");
    private readonly Spell Lifeblood = new Spell("Lifeblood");
    private readonly Spell Stoneform = new Spell("Stoneform");
    private readonly Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    private readonly Spell War_Stomp = new Spell("War Stomp");
    private readonly Spell Engineering = new Spell("Engineering");
    private readonly Spell Alchemy = new Spell("Alchemy");

    #endregion

    #region Warlock Buffs

    private readonly Spell Curse_of_Enfeeblement = new Spell("Curse of Enfeeblement");
    private readonly Spell Curse_of_the_Elements = new Spell("Curse of the Elements");
    private readonly Spell Dark_Intent = new Spell("Dark Intent");
    private readonly Spell Grimoire_of_Sacrifice = new Spell("Grimoire of Sacrifice");
    private readonly Spell Metamorphosis = new Spell("Metamorphosis");
    private readonly Spell Soul_Link = new Spell("Soul Link");
    private readonly Spell Soulstone = new Spell("Soulstone");

    #endregion

    #region Offensive Spell

    private readonly Spell Carrion_Swarm = new Spell("Carrion Swarm");
    private readonly Spell Command_Demon = new Spell("Command Demon");
    private readonly Spell Corruption = new Spell("Corruption");
    private Timer Corruption_Timer = new Timer(0);
    private readonly Spell Doom = new Spell("Doom");
    private Timer Doom_Timer = new Timer(0);
    private readonly Spell Fel_Flame = new Spell("Fel Flame");
    private readonly Spell Hand_of_Guldan = new Spell("Hand of Gul'dan");
    private readonly Spell Harvest_Life = new Spell("Harvest Life");
    private readonly Spell Hellfire = new Spell("Hellfire");
    private readonly Spell Immolation_Aura = new Spell("Immolation Aura");
    private readonly Spell Shadow_Bolt = new Spell("Shadow Bolt");
    private readonly Spell Soul_Fire = new Spell("Soul Fire");
    private readonly Spell Summon_Imp = new Spell("Summon Imp");
    private readonly Spell Summon_Voidwalker = new Spell("Summon Voidwalker");
    private readonly Spell Summon_Felhunter = new Spell("Summon Felhunter");
    private readonly Spell Summon_Succubus = new Spell("Summon Succubus");
    private readonly Spell Summon_Felguard = new Spell("Summon Felguard");
    private readonly Spell Touch_of_Chaos = new Spell("Touch of Chaos");
    private readonly Spell Void_Ray = new Spell("Void Ray");

    #endregion

    #region Offensive Cooldown

    private readonly Spell Archimondes_Vengeance = new Spell("Archimonde's Vengeance");
    private readonly Spell Dark_Soul = new Spell("Dark Soul");
    private readonly Spell Grimoire_of_Service = new Spell("Grimoire of Service");
    private readonly Spell Summon_Doomguard = new Spell("Summon Doomguard");
    private readonly Spell Summon_Infernal = new Spell("Summon Infernal");

    #endregion

    #region Defensive Cooldown

    private readonly Spell Dark_Bargain = new Spell("Dark Bargain");
    private readonly Spell Howl_of_Terror = new Spell("Howl_of_Terror");
    private readonly Spell Sacrificial_Pact = new Spell("Sacrificial Pact");
    private readonly Spell Shadowfury = new Spell("Shadowfury");
    private readonly Spell Twilight_Ward = new Spell("Twilight Ward");
    private readonly Spell Unbound_Will = new Spell("Unbound Will");
    private readonly Spell Unending_Resolve = new Spell("Unending Resolve");

    #endregion

    #region Healing Spell

    private readonly Spell Create_Healthstone = new Spell("Create Healthstone");
    private readonly Spell Dark_Regeneration = new Spell("Dark Regeneration");
    private readonly Spell Drain_Life = new Spell("Drain Life");
    private readonly Spell Health_Funnel = new Spell("Health Funnel");
    private readonly Spell Life_Tap = new Spell("Life Tap");
    private readonly Spell Mortal_Coil = new Spell("Mortal Coil");

    #endregion

    private Timer OnCD = new Timer(0);
    private Timer Trinket_Timer = new Timer(0);
    private Timer Engineering_Timer = new Timer(0);
    private Timer AlchFlask_Timer = new Timer(0);
    public int LC = 0;

    public Warlock_Demonology()
    {
        Main.range = 30.0f;
        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            try
            {
                if (!ObjectManager.Me.IsMounted)
                {
                    if (Fight.InFight && ObjectManager.Me.Target > 0)
                    {
                        if (ObjectManager.Me.Target != lastTarget &&
                            (Doom.IsDistanceGood || Corruption.IsDistanceGood))
                        {
                            Pull();
                            lastTarget = ObjectManager.Me.Target;
                        }

                        if (ObjectManager.Target.Level < 70 && ObjectManager.Me.Level > 84
                            && MySettings.UseLowCombat)
                        {
                            LC = 1;
                            LowCombat();
                        }
                        else
                        {
                            LC = 0;
                            Combat();
                        }
                    }
                    else
                        Patrolling();
                }
            }
            catch
            {
            }
            Thread.Sleep(250);
        }
    }

    public void Pull()
    {
        if (Corruption.IsSpellUsable && Corruption.IsDistanceGood && Corruption.KnownSpell
            && MySettings.UseDoom && ObjectManager.Me.DemonicFury > 199)
        {
            if (Metamorphosis.KnownSpell && Metamorphosis.IsSpellUsable
                && MySettings.UseMetamorphosis && !Metamorphosis.HaveBuff)
            {
                Metamorphosis.Launch();
                Thread.Sleep(400);
                Corruption.Launch();
                Doom_Timer = new Timer(1000*60);
            }

            if (Metamorphosis.HaveBuff)
            {
                Thread.Sleep(2500);
                Metamorphosis.Launch();
            }
            return;
        }
    }

    public void Combat()
    {
        AvoidMelee();
        if (OnCD.IsReady)
            Defense_Cycle();
        Heal();
        Decast();
        Buff();
        DPS_Burst();
        DPS_Cycle();
    }

    public void LowCombat()
    {
        AvoidMelee();
        Heal();
        Defense_Cycle();
        Buff();

        if (ObjectManager.Me.BarTwoPercentage < 75 && Life_Tap.KnownSpell && Life_Tap.IsSpellUsable
            && MySettings.UseLifeTap)
        {
            Life_Tap.Launch();
            return;
        }
        else
        {
            if (Fel_Flame.IsDistanceGood && Fel_Flame.IsSpellUsable && Fel_Flame.KnownSpell
                && MySettings.UseFelFlame)
            {
                Fel_Flame.Launch();
                if (ObjectManager.Target.HealthPercent < 50 && ObjectManager.Target.HealthPercent > 0)
                {
                    Fel_Flame.Launch();
                    return;
                }
            }
        }

        if (Hellfire.IsSpellUsable && Hellfire.KnownSpell && Hellfire.IsDistanceGood
            && MySettings.UseHellfire)
        {
            Hellfire.Launch();
            Thread.Sleep(200);
            while (ObjectManager.Me.IsCast && ObjectManager.Target.HealthPercent > 0)
            {
                Thread.Sleep(200);
            }
            return;
        }
    }

    public void DPS_Burst()
    {
        if (MySettings.UseTrinket && Trinket_Timer.IsReady && ObjectManager.Target.GetDistance < 40)
        {
            Logging.WriteFight("Use Trinket 1.");
            Lua.RunMacroText("/use 13");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Logging.WriteFight("Use Trinket 2.");
            Lua.RunMacroText("/use 14");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Trinket_Timer = new Timer(1000*60*2);
            return;
        }
        else if (Berserking.IsSpellUsable && Berserking.KnownSpell && MySettings.UseBerserking
            && ObjectManager.Target.GetDistance < 40)
        {
            Berserking.Launch();
            return;
        }
        else if (Blood_Fury.IsSpellUsable && Blood_Fury.KnownSpell && MySettings.UseBloodFury
            && ObjectManager.Target.GetDistance < 40)
        {
            Blood_Fury.Launch();
            return;
        }
        else if (Lifeblood.IsSpellUsable && Lifeblood.KnownSpell && MySettings.UseLifeblood
            && ObjectManager.Target.GetDistance < 40)
        {
            Lifeblood.Launch();
            return;
        }
        else if (MySettings.UseEngGlove && Engineering.KnownSpell && Engineering_Timer.IsReady
            && ObjectManager.Target.GetDistance < 40)
        {
            Logging.WriteFight("Use Engineering Gloves.");
            Lua.RunMacroText("/use 10");
            Engineering_Timer = new Timer(1000*60);
            return;
        }
        else if (Dark_Soul.KnownSpell && Dark_Soul.IsSpellUsable
            && MySettings.UseDarkSoul && ObjectManager.Target.GetDistance < 40)
        {
            Dark_Soul.Launch();
            return;
        }
        else if (Summon_Doomguard.KnownSpell && Summon_Doomguard.IsSpellUsable
            && MySettings.UseSummonDoomguard && Summon_Doomguard.IsDistanceGood)
        {
            Summon_Doomguard.Launch();
            return;
        }
        else if (Summon_Infernal.KnownSpell && Summon_Infernal.IsSpellUsable
            && MySettings.UseSummonInfernal && Summon_Infernal.IsDistanceGood)
        {
            SpellManager.CastSpellByIDAndPosition(1122, ObjectManager.Target.Position);
            return;
        }
        else if (Archimondes_Vengeance.KnownSpell && Archimondes_Vengeance.IsSpellUsable
            && MySettings.UseArchimondesVengeance)
        {
            Archimondes_Vengeance.Launch();
            return;
        }
        else
        {
            if (Grimoire_of_Service.KnownSpell && Grimoire_of_Service.IsSpellUsable
                && MySettings.UseGrimoireofService && ObjectManager.Target.GetDistance < 40)
            {
                Grimoire_of_Service.Launch();
                return;
            }
        }
    }

    public void DPS_Cycle()
    {
        if (ObjectManager.Me.DemonicFury > 899 || (Doom_Timer.IsReady || !ObjectManager.Target.HaveBuff(603)))
        {
            if (ObjectManager.Me.DemonicFury > 199)
            {
                if (Corruption.KnownSpell && Corruption.IsSpellUsable && Corruption.IsDistanceGood
                    && MySettings.UseCorruption)
                {
                    Corruption.Launch();
                    Corruption_Timer = new Timer(1000*20);
                }

                if (MySettings.UseMetamorphosis)
                    MetamorphosisCombat();
                return;
            }
        }

        if (Metamorphosis.HaveBuff)
            MetamorphosisCombat();

        if (Curse_of_the_Elements.KnownSpell && Curse_of_the_Elements.IsSpellUsable && MySettings.UseCurseoftheElements
            && Curse_of_the_Elements.IsDistanceGood && !Curse_of_the_Elements.TargetHaveBuff)
        {
            Curse_of_the_Elements.Launch();
            return;
        }
        else if (Curse_of_Enfeeblement.KnownSpell && Curse_of_Enfeeblement.IsSpellUsable && MySettings.UseCurseofEnfeeblement
            && Curse_of_Enfeeblement.IsDistanceGood && !Curse_of_Enfeeblement.TargetHaveBuff && !MySettings.UseCurseoftheElements)
        {
            Curse_of_Enfeeblement.Launch();
            return;
        }
        else if (ObjectManager.Me.BarTwoPercentage < 75 && Life_Tap.KnownSpell && Life_Tap.IsSpellUsable
            && MySettings.UseLifeTap)
        {
            Life_Tap.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 4 && Harvest_Life.IsSpellUsable && Harvest_Life.KnownSpell
            && MySettings.UseHarvestLife && Harvest_Life.IsDistanceGood)
        {
            Harvest_Life.Launch();
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(200);
            }
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 2 && Command_Demon.IsSpellUsable && Command_Demon.KnownSpell
            && Command_Demon.IsDistanceGood && ObjectManager.Pet.Guid == 207 && ObjectManager.Pet.Health > 0
            && MySettings.UseCommandDemon)
        {
            Command_Demon.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 4 && Hellfire.IsSpellUsable && Hellfire.KnownSpell
            && MySettings.UseHellfire && ObjectManager.Target.GetDistance < 20 
            && (!Harvest_Life.KnownSpell || !MySettings.UseHarvestLife))
        {
            Hellfire.Launch();
            Thread.Sleep(200);
            while (ObjectManager.Me.IsCast && ObjectManager.Target.HealthPercent > 0)
            {
                Thread.Sleep(200);
            }
            return;
        }
        else if (Corruption.KnownSpell && Corruption.IsSpellUsable && Corruption.IsDistanceGood
            && MySettings.UseCorruption && (!Corruption.TargetHaveBuff || Corruption_Timer.IsReady))
        {
            Corruption.Launch();
            Corruption_Timer = new Timer(1000*20);
            return;
        }
        else if (Hand_of_Guldan.KnownSpell && Hand_of_Guldan.IsSpellUsable && Hand_of_Guldan.IsDistanceGood
            && MySettings.UseHandofGuldan && !ObjectManager.Target.HaveBuff(47960))
        {
            Hand_of_Guldan.Launch();
            return;
        }
        else if (Soul_Fire.KnownSpell && Soul_Fire.IsSpellUsable && Soul_Fire.IsDistanceGood 
            && MySettings.UseSoulFire && ObjectManager.Me.HaveBuff(122355))
        {
            Soul_Fire.Launch();
            return;
        }
        else
        {
            if (Shadow_Bolt.KnownSpell && Shadow_Bolt.IsSpellUsable && Shadow_Bolt.IsDistanceGood
                && MySettings.UseShadowBolt)
            {
                Shadow_Bolt.Launch();
                return;
            }
        }
    }

    public void MetamorphosisCombat()
    {
        while (ObjectManager.Me.DemonicFury > 100)
        {
            if (Metamorphosis.KnownSpell && Metamorphosis.IsSpellUsable && !Metamorphosis.HaveBuff)
            {
                Metamorphosis.Launch();
                Thread.Sleep(700);
            }

            if (ObjectManager.GetNumberAttackPlayer() > 2)
            {
                if (Hellfire.KnownSpell && Hellfire.IsSpellUsable && Metamorphosis.HaveBuff
                    && MySettings.UseImmolationAura && ObjectManager.Target.GetDistance < 20)
                {
                    Hellfire.Launch();
                    Thread.Sleep(200);
                }
                else if (Carrion_Swarm.IsSpellUsable && Carrion_Swarm.KnownSpell
                    && Metamorphosis.HaveBuff && ObjectManager.Target.GetDistance < 20)
                {
                    Carrion_Swarm.Launch();
                    Thread.Sleep(200);
                }
                else
                {
                    if (Fel_Flame.IsSpellUsable && Fel_Flame.KnownSpell && Fel_Flame.IsDistanceGood
                        && MySettings.UseVoidRay && Metamorphosis.HaveBuff)
                    {
                        Fel_Flame.Launch();
                        Thread.Sleep(200);
                    }
                }
            }

            else
            {
                if (Corruption.IsDistanceGood && Metamorphosis.HaveBuff
                    && Corruption.KnownSpell && Corruption.IsSpellUsable && MySettings.UseDoom
                    && (Doom_Timer.IsReady || !ObjectManager.Target.HaveBuff(603)))
                {
                    Corruption.Launch();
                    Doom_Timer = new Timer(1000*60);
                    Thread.Sleep(200);
                }

                if (Shadow_Bolt.KnownSpell && Shadow_Bolt.IsSpellUsable && Shadow_Bolt.IsDistanceGood
                    && MySettings.UseTouchofChaos && Metamorphosis.HaveBuff)
                {
                    Shadow_Bolt.Launch();
                    Thread.Sleep(200);
                }
            }
        }

        Thread.Sleep(700);
        if (Metamorphosis.HaveBuff)
        {
            Metamorphosis.Launch();
            return;
        }
        return;
    }

    public void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            Heal();
            Buff();
        }
    }

    private void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        Pet();

        if (!Dark_Intent.HaveBuff && Dark_Intent.KnownSpell && Dark_Intent.IsSpellUsable
            && MySettings.UseDarkIntent)
        {
            Dark_Intent.Launch();
            return;
        }
        else if (!Soul_Link.HaveBuff && Soul_Link.KnownSpell && Soul_Link.IsSpellUsable
            && MySettings.UseSoulLink && (ObjectManager.Pet.Health != 0 || ObjectManager.Pet.Guid != 0))
        {
            Soul_Link.Launch();
            return;
        }
        if (!Soulstone.HaveBuff && Soulstone.KnownSpell && Soulstone.IsSpellUsable
            && MySettings.UseSoulstone)
        {
            Soulstone.Launch();
            return;
        }
        else
        {
            if (ItemsManager.GetItemCountByIdLUA(5512) == 0 && Create_Healthstone.KnownSpell
                && Create_Healthstone.IsSpellUsable && MySettings.UseCreateHealthstone)
            {
                Logging.WriteFight(" - Create Healthstone - ");
                Thread.Sleep(200);
                Create_Healthstone.Launch();
                Thread.Sleep(200);
                while (ObjectManager.Me.IsCast)
                {
                    Thread.Sleep(200);
                }
            }
        }
    }

    private void Pet()
    {
        if (Health_Funnel.KnownSpell)
        {
            if (ObjectManager.Pet.HealthPercent > 0 && ObjectManager.Pet.HealthPercent < 50
                && Health_Funnel.IsSpellUsable && Health_Funnel.KnownSpell && MySettings.UseHealthFunnel)
            {
                Health_Funnel.Launch();
                while (ObjectManager.Me.IsCast)
                {
                    if (ObjectManager.Pet.HealthPercent > 85 || ObjectManager.Pet.IsDead)
                        break;
                    Thread.Sleep(100);
                }
            }
        }

        if (Grimoire_of_Sacrifice.KnownSpell && !Grimoire_of_Sacrifice.HaveBuff && Grimoire_of_Sacrifice.IsSpellUsable
            && MySettings.UseGrimoireofSacrifice)
        {
            if ((ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0))
            {
                Logging.WriteFight(" - PET DEAD - ");
                if (MySettings.UseSummonFelhunter && Summon_Felhunter.KnownSpell && Summon_Felhunter.IsSpellUsable)
                    Summon_Felhunter.Launch();
                else if (MySettings.UseSummonFelguard && Summon_Felguard.KnownSpell && Summon_Felguard.IsSpellUsable)
                    Summon_Felguard.Launch();
                else if (MySettings.UseSummonImp && Summon_Imp.KnownSpell && Summon_Imp.IsSpellUsable)
                    Summon_Imp.Launch();
                else if (MySettings.UseSummonVoidwalker && Summon_Voidwalker.KnownSpell && Summon_Voidwalker.IsSpellUsable)
                    Summon_Voidwalker.Launch();
                else
                {
                    if (MySettings.UseSummonSuccubus && Summon_Succubus.KnownSpell && Summon_Succubus.IsSpellUsable)
                        Summon_Succubus.Launch();
                }
                Thread.Sleep(200);
            }
            if ((ObjectManager.Pet.Health != 0 || ObjectManager.Pet.Guid != 0))
                Grimoire_of_Sacrifice.Launch();
            return;
        }
        else
        {
            if ((ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0) && !Grimoire_of_Sacrifice.KnownSpell)
            {
                Logging.WriteFight(" - PET DEAD - ");
                if (MySettings.UseSummonFelhunter && Summon_Felhunter.KnownSpell && Summon_Felhunter.IsSpellUsable)
                    Summon_Felhunter.Launch();
                else if (MySettings.UseSummonFelguard && Summon_Felguard.KnownSpell && Summon_Felguard.IsSpellUsable)
                    Summon_Felguard.Launch();
                else if (MySettings.UseSummonImp && Summon_Imp.KnownSpell && Summon_Imp.IsSpellUsable)
                    Summon_Imp.Launch();
                else if (MySettings.UseSummonVoidwalker && Summon_Voidwalker.KnownSpell && Summon_Voidwalker.IsSpellUsable)
                    Summon_Voidwalker.Launch();
                else
                {
                    if (MySettings.UseSummonSuccubus && Summon_Succubus.KnownSpell && Summon_Succubus.IsSpellUsable)
                        Summon_Succubus.Launch();
                }
                return;
            }
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (ObjectManager.Me.HealthPercent < 80 && Gift_of_the_Naaru.IsSpellUsable && Gift_of_the_Naaru.KnownSpell
            && MySettings.UseGiftoftheNaaru)
        {
            Gift_of_the_Naaru.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 65 && Dark_Regeneration.IsSpellUsable && Dark_Regeneration.KnownSpell
            && MySettings.UseDarkRegeneration)
        {
            Dark_Regeneration.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 75 && ItemsManager.GetItemCountByIdLUA(5512) > 0
            && MySettings.UseCreateHealthstone)
        {
            Logging.WriteFight("Use Healthstone.");
            nManager.Wow.Helpers.ItemsManager.UseItem("Healthstone");
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 85 && Mortal_Coil.IsSpellUsable && Mortal_Coil.KnownSpell
            && MySettings.UseMortalCoil && Mortal_Coil.IsDistanceGood)
        {
            Mortal_Coil.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Me.HealthPercent < 70 && Drain_Life.KnownSpell
                && MySettings.UseDrainLife && Drain_Life.IsDistanceGood && Drain_Life.IsSpellUsable)
            {
                Drain_Life.Launch();
                while (ObjectManager.Me.IsCast)
                {
                    Thread.Sleep(200);
                }
                return;
            }
        }
    }

    private void Defense_Cycle()
    {
        if (ObjectManager.Me.HealthPercent < 70 && MySettings.UseUnendingResolve
            && Unending_Resolve.KnownSpell && Unending_Resolve.IsSpellUsable)
        {
            Unending_Resolve.Launch();
            OnCD = new Timer(1000*8);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 20 && MySettings.UseHowlofTerror
            && Howl_of_Terror.KnownSpell && Howl_of_Terror.IsSpellUsable && ObjectManager.Target.GetDistance < 8)
        {
            Howl_of_Terror.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 40 && MySettings.UseDarkBargain
            && Dark_Bargain.KnownSpell && Dark_Bargain.IsSpellUsable)
        {
            Dark_Bargain.Launch();
            OnCD = new Timer(1000*8);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 95 && MySettings.UseSacrificialPact
            && Sacrificial_Pact.KnownSpell && Sacrificial_Pact.IsSpellUsable
            && (ObjectManager.Pet.Health != 0 || ObjectManager.Pet.Guid != 0))
        {
            Sacrificial_Pact.Launch();
            OnCD = new Timer(1000*10);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 90 && MySettings.UseShadowfury
            && Shadowfury.KnownSpell && Shadowfury.IsSpellUsable && ObjectManager.Target.GetDistance < 8)
        {
            Shadowfury.Launch();
            OnCD = new Timer(1000*3);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && War_Stomp.IsSpellUsable && War_Stomp.KnownSpell
            && MySettings.UseWarStomp)
        {
            War_Stomp.Launch();
            OnCD = new Timer(1000*2);
            return;
        }
        else
        {
            if (ObjectManager.Me.HealthPercent < 80 && Stoneform.IsSpellUsable && Stoneform.KnownSpell
                && MySettings.UseStoneform)
            {
                Stoneform.Launch();
                OnCD = new Timer(1000*8);
                return;
            }
        }
    }

    private void Decast()
    {
        if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && ObjectManager.Target.GetDistance < 8
            && Arcane_Torrent.KnownSpell && Arcane_Torrent.IsSpellUsable && MySettings.UseArcaneTorrent)
        {
            Arcane_Torrent.Launch();
            return;
        }
        else if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe
            && MySettings.UseTwilightWard && Twilight_Ward.KnownSpell && Twilight_Ward.IsSpellUsable)
        {
            Twilight_Ward.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && MySettings.UseSummonFelhunter
                && Command_Demon.IsSpellUsable && Command_Demon.KnownSpell && ObjectManager.Target.GetDistance < 40)
            {
                Command_Demon.Launch();
                return;
            }
        }
    }

    private void AvoidMelee()
    {
        if (ObjectManager.Target.GetDistance < 3 && ObjectManager.Target.InCombat)
        {
            nManager.Wow.Helpers.Keybindings.PressKeybindings(nManager.Wow.Enums.Keybindings.MOVEBACKWARD);
        }
    }
}

public class Warlock_Destruction
{
    [Serializable]
    public class WarlockDestructionSettings : nManager.Helpful.Settings
    {
        /* Professions & Racials */
        public bool UseArcaneTorrent = true;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseLifeblood = true;
        public bool UseStoneform = true;
        public bool UseGiftoftheNaaru = true;
        public bool UseWarStomp = true;
        /* Warlock Buffs */
        public bool UseCurseofEnfeeblement = false;
        public bool UseCurseoftheElements = true;
        public bool UseDarkIntent = true;
        public bool UseGrimoireofSacrifice = true;
        public bool UseSoulLink = true;
        public bool UseSoulstone = true;
        /* Offensive Spell */
        public bool UseChaosBolt = true;
        public bool UseCommandDemon = true;
        public bool UseConflagrate = true;
        public bool UseFelFlame = true;
        public bool UseFireandBrimstone = true;
        public bool UseHarvestLife = true;
        public bool UseImmolate = true;
        public bool UseIncinerate = true;
        public bool UseRainofFire = true;
        public bool UseShadowburn = true;
        public bool UseSummonImp = false;
        public bool UseSummonVoidwalker = false;
        public bool UseSummonFelhunter = true;
        public bool UseSummonSuccubus = false;
        /* Offensive Cooldown */
        public bool UseArchimondesVengeance = true;
        public bool UseDarkSoul = true;
        public bool UseGrimoireofService = true;
        public bool UseSummonDoomguard = true;
        public bool UseSummonInfernal = false;
        /* Defensive Cooldown */
        public bool UseDarkBargain = true;
        public bool UseHowlofTerror = true;
        public bool UseSacrificialPact = true;
        public bool UseShadowfury = true;
        public bool UseTwilightWard = true;
        public bool UseUnboundWill = true;
        public bool UseUnendingResolve = true;
        /* Healing Spell */
        public bool UseCreateHealthstone = true;
        public bool UseDarkRegeneration = true;
        public bool UseDrainLife = true;
        public bool UseEmberTap = true;
        public bool UseFlamesofXoroth = true;
        public bool UseHealthFunnel = true;
        public bool UseLifeTap = true;
        public bool UseMortalCoil = true;
        /* Game Settings */
        public bool UseLowCombat = true;
        public bool UseTrinket = true;
        public bool UseEngGlove = true;
        public bool UseAlchFlask = true;

        public WarlockDestructionSettings()
        {
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");
            AddControlInWinForm("Use Lifeblood", "UseLifeblood", "Professions & Racials");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            /* Warlock Buffs */
            AddControlInWinForm("Use Curse of Enfeeblement", "UseCurseofEnfeeblement", "Warlock Buffs");
            AddControlInWinForm("Use Curse of the Elements", "UseCurseoftheElements", "Warlock Buffs");
            AddControlInWinForm("Use Dark Intent", "UseDarkIntent", "Warlock Buffs");
            AddControlInWinForm("Use Grimoire of Sacrifice", "UseGrimoireofSacrifice", "Warlock Buffs");
            AddControlInWinForm("Use Soul Link ", "UseSoulLink ", "Warlock Buffs");
            AddControlInWinForm("Use Soulstone", "UseSoulstone", "Warlock Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Chaos Bolt", "UseChaosBolt", "Offensive Spell");
            AddControlInWinForm("Use Command Demon", "UseCommandDemon", "Offensive Spell");
            AddControlInWinForm("Use Conflagrate", "UseConflagrate", "Offensive Spell");
            AddControlInWinForm("Use Fel Flame", "UseFelFlame", "Offensive Spell");
            AddControlInWinForm("Use Fire and Brimstone", "UseFireandBrimstone", "Offensive Spell");
            AddControlInWinForm("Use Harvest Life", "UseHarvestLife", "Offensive Spell");
            AddControlInWinForm("Use Immolate", "UseImmolate", "Offensive Spell");
            AddControlInWinForm("Use Incinerate", "UseIncinerate", "Offensive Spell");
            AddControlInWinForm("Use Rain of Fire", "UseRainofFire", "Offensive Spell");
            AddControlInWinForm("Use Shadowburn", "UseShadowburn", "Offensive Spell");
            AddControlInWinForm("Use Summon Imp", "UseSummonImp", "Offensive Spell");
            AddControlInWinForm("Use Summon Voidwalker", "UseSummonVoidwalker", "Offensive Spell");
            AddControlInWinForm("Use Summon Felhunter", "UseSummonFelhunter", "Offensive Spell");
            AddControlInWinForm("Use Summon Succubus", "UseSummonSuccubus", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use Archimonde's Vengeance", "UseArchimondesVengeance", "Offensive Cooldown");
            AddControlInWinForm("Use Dark Soul", "UseDarkSoul", "Offensive Cooldown");
            AddControlInWinForm("Use Grimoire of Service", "UseGrimoireofService", "Offensive Cooldown");
            AddControlInWinForm("Use Summon Doomguard", "UseSummonDoomguard", "Offensive Cooldown");
            AddControlInWinForm("Use Summon Infernal", "UseSummonInfernal", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Dark Bargain", "UseDarkBargain", "Defensive Cooldown");
            AddControlInWinForm("Use Howl of Terror", "UseHowlofTerror", "Defensive Cooldown");
            AddControlInWinForm("Use Sacrificial Pact", "UseSacrificialPact", "Defensive Cooldown");
            AddControlInWinForm("Use Shadowfury", "UseShadowfury", "Defensive Cooldown");
            AddControlInWinForm("Use Twilight Ward", "UseTwilightWard", "Defensive Cooldown");
            AddControlInWinForm("Use Unbound Will", "UseUnboundWill", "Defensive Cooldown");
            AddControlInWinForm("Use Unending Resolve", "UseUnendingResolve", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Create Healthstone", "UseCreateHealthstone", "Healing Spell");
            AddControlInWinForm("Use Dark Regeneration", "UseDarkRegeneration", "Healing Spell");
            AddControlInWinForm("Use Drain Life", "UseDrainLife", "Healing Spell");
            AddControlInWinForm("Use Ember Tap", "UseEmberTap", "Healing Spell");
            AddControlInWinForm("Use Flames of Xoroth", "UseFlamesofXoroth", "Healing Spell");
            AddControlInWinForm("Use Health Funnel", "UseHealthFunnel", "Healing Spell");
            AddControlInWinForm("Use Life Tap", "UseLifeTap", "Healing Spell");
            AddControlInWinForm("Use Mortal Coil", "UseMortalCoil", "Healing Spell");
            /* Game Settings */
            AddControlInWinForm("Use Low Combat Settings", "UseLowCombat", "Game Settings");
            AddControlInWinForm("Use Trinket", "UseTrinket", "Game Settings");
            AddControlInWinForm("Use Engineering Gloves", "UseEngGlove", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
        }

        public static WarlockDestructionSettings CurrentSetting { get; set; }

        public static WarlockDestructionSettings GetSettings()
        {
            string CurrentSettingsFile = Application.StartupPath + "\\CustomClasses\\Settings\\Warlock_Destruction.xml";
            if (System.IO.File.Exists(CurrentSettingsFile))
            {
                return
                    CurrentSetting = Settings.Load<Warlock_Destruction.WarlockDestructionSettings>(CurrentSettingsFile);
            }
            else
            {
                return new Warlock_Destruction.WarlockDestructionSettings();
            }
        }
    }

    private readonly WarlockDestructionSettings MySettings = WarlockDestructionSettings.GetSettings();

    #region Professions & Racials

    private readonly Spell Arcane_Torrent = new Spell("Arcane Torrent");
    private readonly Spell Berserking = new Spell("Berserking");
    private readonly Spell Blood_Fury = new Spell("Blood Fury");
    private readonly Spell Lifeblood = new Spell("Lifeblood");
    private readonly Spell Stoneform = new Spell("Stoneform");
    private readonly Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    private readonly Spell War_Stomp = new Spell("War Stomp");
    private readonly Spell Engineering = new Spell("Engineering");
    private readonly Spell Alchemy = new Spell("Alchemy");

    #endregion

    #region Warlock Buffs

    private readonly Spell Curse_of_Enfeeblement = new Spell("Curse of Enfeeblement");
    private readonly Spell Curse_of_the_Elements = new Spell("Curse of the Elements");
    private readonly Spell Dark_Intent = new Spell("Dark Intent");
    private readonly Spell Grimoire_of_Sacrifice = new Spell("Grimoire of Sacrifice");
    private readonly Spell Soul_Link = new Spell("Soul Link");
    private readonly Spell Soulstone = new Spell("Soulstone");

    #endregion

    #region Offensive Spell

    private readonly Spell Chaos_Bolt = new Spell("Chaos Bolt");
    private readonly Spell Command_Demon = new Spell("Command Demon");
    private readonly Spell Conflagrate = new Spell("Conflagrate");
    private readonly Spell Corruption = new Spell("Corruption");
    private readonly Spell Fel_Flame = new Spell("Fel Flame");
    private readonly Spell Fire_and_Brimstone = new Spell("Fire and Brimstone");
    private readonly Spell Harvest_Life = new Spell("Harvest Life");
    private readonly Spell Immolate = new Spell("Immolate");
    private Timer Immolate_Timer = new Timer(0);
    private readonly Spell Incinerate = new Spell("Incinerate");
    private readonly Spell Rain_of_Fire = new Spell("Rain of Fire");
    private readonly Spell Shadow_Bolt = new Spell("Shadow Bolt");
    private readonly Spell Shadowburn = new Spell("Shadowburn");
    private readonly Spell Summon_Imp = new Spell("Summon Imp");
    private readonly Spell Summon_Voidwalker = new Spell("Summon Voidwalker");
    private readonly Spell Summon_Felhunter = new Spell("Summon Felhunter");
    private readonly Spell Summon_Succubus = new Spell("Summon Succubus");

    #endregion

    #region Offensive Cooldown

    private readonly Spell Archimondes_Vengeance = new Spell("Archimonde's Vengeance");
    private readonly Spell Dark_Soul = new Spell("Dark Soul");
    private readonly Spell Grimoire_of_Service = new Spell("Grimoire of Service");
    private readonly Spell Summon_Doomguard = new Spell("Summon Doomguard");
    private readonly Spell Summon_Infernal = new Spell("Summon Infernal");

    #endregion

    #region Defensive Cooldown

    private readonly Spell Dark_Bargain = new Spell("Dark Bargain");
    private readonly Spell Howl_of_Terror = new Spell("Howl_of_Terror");
    private readonly Spell Sacrificial_Pact = new Spell("Sacrificial Pact");
    private readonly Spell Shadowfury = new Spell("Shadowfury");
    private readonly Spell Twilight_Ward = new Spell("Twilight Ward");
    private readonly Spell Unbound_Will = new Spell("Unbound Will");
    private readonly Spell Unending_Resolve = new Spell("Unending Resolve");

    #endregion

    #region Healing Spell

    private readonly Spell Create_Healthstone = new Spell("Create Healthstone");
    private readonly Spell Dark_Regeneration = new Spell("Dark Regeneration");
    private readonly Spell Drain_Life = new Spell("Drain Life");
    private readonly Spell Ember_Tap = new Spell("Ember Tap");
    private readonly Spell Flames_of_Xoroth = new Spell("Flames of Xoroth");
    private readonly Spell Health_Funnel = new Spell("Health Funnel");
    private readonly Spell Life_Tap = new Spell("Life Tap");
    private readonly Spell Mortal_Coil = new Spell("Mortal Coil");

    #endregion

    private Timer OnCD = new Timer(0);
    private Timer Trinket_Timer = new Timer(0);
    private Timer Engineering_Timer = new Timer(0);
    private Timer AlchFlask_Timer = new Timer(0);
    public int LC = 0;

    public Warlock_Destruction()
    {
        Main.range = 30.0f;
        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            try
            {
                if (!ObjectManager.Me.IsMounted)
                {
                    if (Fight.InFight && ObjectManager.Me.Target > 0)
                    {
                        if (ObjectManager.Me.Target != lastTarget &&
                            (Curse_of_the_Elements.IsDistanceGood))
                        {
                            Pull();
                            lastTarget = ObjectManager.Me.Target;
                        }

                        if (ObjectManager.Target.Level < 70 && ObjectManager.Me.Level > 84
                            && MySettings.UseLowCombat)
                        {
                            LC = 1;
                            LowCombat();
                        }
                        else
                        {
                            LC = 0;
                            Combat();
                        }
                    }
                    else
                        Patrolling();
                }
            }
            catch
            {
            }
            Thread.Sleep(250);
        }
    }

    public void Pull()
    {
        if (Curse_of_the_Elements.KnownSpell && Curse_of_the_Elements.IsSpellUsable
            && Curse_of_the_Elements.IsDistanceGood && !Curse_of_the_Elements.TargetHaveBuff
            && MySettings.UseCurseoftheElements)
        {
            Curse_of_the_Elements.Launch();
            return;
        }
    }

    public void LowCombat()
    {
        AvoidMelee();
        Heal();
        Defense_Cycle();
        Buff();

        // Blizzard API Calls for Incinerate using Shadow Bolt Function
        if (Shadow_Bolt.KnownSpell && Shadow_Bolt.IsSpellUsable && Shadow_Bolt.IsDistanceGood
            && MySettings.UseIncinerate)
        {
            Shadow_Bolt.Launch();
            return;
        }

        if (ObjectManager.Target.HealthPercent < 50 && ObjectManager.Target.HealthPercent > 0)
        {
            if (Shadow_Bolt.KnownSpell && Shadow_Bolt.IsSpellUsable && Shadow_Bolt.IsDistanceGood
                && MySettings.UseIncinerate)
            {
                Shadow_Bolt.Launch();
                return;
            }
        }

        if (Rain_of_Fire.IsSpellUsable && Rain_of_Fire.KnownSpell && Rain_of_Fire.IsDistanceGood
            && MySettings.UseRainofFire)
        {
            SpellManager.CastSpellByIDAndPosition(5740, ObjectManager.Target.Position);
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(200);
            }
            return;
        }
    }

    public void DPS_Burst()
    {
        if (MySettings.UseTrinket && Trinket_Timer.IsReady && ObjectManager.Target.GetDistance < 30)
        {
            Logging.WriteFight("Use Trinket 1.");
            Lua.RunMacroText("/use 13");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Logging.WriteFight("Use Trinket 2.");
            Lua.RunMacroText("/use 14");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Trinket_Timer = new Timer(1000*60*2);
            return;
        }
        else if (Berserking.IsSpellUsable && Berserking.KnownSpell && MySettings.UseBerserking
            && ObjectManager.Target.GetDistance < 30)
        {
            Berserking.Launch();
            return;
        }
        else if (Blood_Fury.IsSpellUsable && Blood_Fury.KnownSpell && MySettings.UseBloodFury
            && ObjectManager.Target.GetDistance < 30)
        {
            Blood_Fury.Launch();
            return;
        }
        else if (Lifeblood.IsSpellUsable && Lifeblood.KnownSpell && MySettings.UseLifeblood
            && ObjectManager.Target.GetDistance < 30)
        {
            Lifeblood.Launch();
            return;
        }
        else if (MySettings.UseEngGlove && Engineering.KnownSpell && Engineering_Timer.IsReady
            && ObjectManager.Target.GetDistance < 30)
        {
            Logging.WriteFight("Use Engineering Gloves.");
            Lua.RunMacroText("/use 10");
            Engineering_Timer = new Timer(1000*60);
            return;
        }
        else if (Dark_Soul.KnownSpell && Dark_Soul.IsSpellUsable
            && MySettings.UseDarkSoul && ObjectManager.Target.GetDistance < 40)
        {
            Dark_Soul.Launch();
            return;
        }
        else if (Summon_Doomguard.KnownSpell && Summon_Doomguard.IsSpellUsable
            && MySettings.UseSummonDoomguard && Summon_Doomguard.IsDistanceGood)
        {
            Summon_Doomguard.Launch();
            return;
        }
        else if (Summon_Infernal.KnownSpell && Summon_Infernal.IsSpellUsable
            && MySettings.UseSummonInfernal && Summon_Infernal.IsDistanceGood)
        {
            SpellManager.CastSpellByIDAndPosition(1122, ObjectManager.Target.Position);
            return;
        }
        else if (Archimondes_Vengeance.KnownSpell && Archimondes_Vengeance.IsSpellUsable
            && MySettings.UseArchimondesVengeance)
        {
            Archimondes_Vengeance.Launch();
            return;
        }
        else
        {
            if (Grimoire_of_Service.KnownSpell && Grimoire_of_Service.IsSpellUsable
                && MySettings.UseGrimoireofService && ObjectManager.Target.GetDistance < 40)
            {
                Grimoire_of_Service.Launch();
                return;
            }
        }
    }

    public void Combat()
    {
        AvoidMelee();
        if (OnCD.IsReady)
            Defense_Cycle();
        Heal();
        Decast();
        Buff();
        DPS_Burst();
        DPS_Cycle();
    }

    public void DPS_Cycle()
    {
        if (Curse_of_the_Elements.KnownSpell && Curse_of_the_Elements.IsSpellUsable && MySettings.UseCurseoftheElements
            && Curse_of_the_Elements.IsDistanceGood && !Curse_of_the_Elements.TargetHaveBuff)
        {
            Curse_of_the_Elements.Launch();
            return;
        }
        else if (Curse_of_Enfeeblement.KnownSpell && Curse_of_Enfeeblement.IsSpellUsable && MySettings.UseCurseofEnfeeblement
            && Curse_of_Enfeeblement.IsDistanceGood && !Curse_of_Enfeeblement.TargetHaveBuff && !MySettings.UseCurseoftheElements)
        {
            Curse_of_Enfeeblement.Launch();
            return;
        }
        // Blizzard API Calls for Immolate using Corruption Function
        else if (ObjectManager.GetNumberAttackPlayer() > 4 && Fire_and_Brimstone.IsSpellUsable && Fire_and_Brimstone.KnownSpell
            && !ObjectManager.Target.HaveBuff(348) && Corruption.IsSpellUsable && Corruption.KnownSpell && Corruption.IsDistanceGood
            && MySettings.UseFireandBrimstone && MySettings.UseImmolate)
        {
            Fire_and_Brimstone.Launch();
            Thread.Sleep(200);
            Corruption.Launch();
            Immolate_Timer = new Timer(1000*12);
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 4 && ObjectManager.Target.HaveBuff(348)
            && MySettings.UseHarvestLife && Harvest_Life.KnownSpell && Harvest_Life.IsSpellUsable
            && Harvest_Life.IsDistanceGood)
        {
            Harvest_Life.Launch();
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(200);
            }
            return;
        }
        // Blizzard API Calls for Incinerate using Shadow Bolt Function
        else if (ObjectManager.GetNumberAttackPlayer() > 4 && Fire_and_Brimstone.IsSpellUsable && Fire_and_Brimstone.KnownSpell
            && Shadow_Bolt.KnownSpell && Shadow_Bolt.IsSpellUsable && Shadow_Bolt.IsDistanceGood
            && MySettings.UseFireandBrimstone && MySettings.UseIncinerate)
        {
            Fire_and_Brimstone.Launch();
            Thread.Sleep(200);
            Shadow_Bolt.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 4 && Rain_of_Fire.IsSpellUsable && Rain_of_Fire.KnownSpell
            && Rain_of_Fire.IsDistanceGood && MySettings.UseRainofFire)
        {
            SpellManager.CastSpellByIDAndPosition(5740, ObjectManager.Target.Position);
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(200);
            }
            return;
        }
        else if (Conflagrate.KnownSpell && Conflagrate.IsSpellUsable && Conflagrate.IsDistanceGood
            && MySettings.UseConflagrate)
        {
            Conflagrate.Launch();
            return;
        }
        else
        {
            if (Corruption.IsSpellUsable && Corruption.KnownSpell && Corruption.IsDistanceGood
                && MySettings.UseImmolate && !ObjectManager.Target.HaveBuff(348) || Immolate_Timer.IsReady)
            {
                Corruption.Launch();
                Immolate_Timer = new Timer(1000*12);
                return;
            }
        }

        if (ObjectManager.Target.HealthPercent < 20)
        {
            if (Shadowburn.KnownSpell && Shadowburn.IsSpellUsable && Shadowburn.IsDistanceGood
                && !ObjectManager.Me.HaveBuff(117828) && MySettings.UseShadowburn)
            {
                Shadowburn.Launch();
                return;
            }
        }
        else
        {
            if (Chaos_Bolt.KnownSpell && Chaos_Bolt.IsSpellUsable && Chaos_Bolt.IsDistanceGood
                && !ObjectManager.Me.HaveBuff(117828) && MySettings.UseChaosBolt)
            {
                Chaos_Bolt.Launch();
                return;
            }
        }

        if (Shadow_Bolt.KnownSpell && Shadow_Bolt.IsSpellUsable && Shadow_Bolt.IsDistanceGood
            && MySettings.UseIncinerate)
        {
            Shadow_Bolt.Launch();
            return;
        }
    }

    public void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            Heal();
            Buff();
        }
    }

    private void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        Pet();

        if (!Dark_Intent.HaveBuff && Dark_Intent.KnownSpell && Dark_Intent.IsSpellUsable
            && MySettings.UseDarkIntent)
        {
            Dark_Intent.Launch();
            return;
        }
        else if (!Soul_Link.HaveBuff && Soul_Link.KnownSpell && Soul_Link.IsSpellUsable
            && MySettings.UseSoulLink && (ObjectManager.Pet.Health != 0 || ObjectManager.Pet.Guid != 0))
        {
            Soul_Link.Launch();
            return;
        }
        if (!Soulstone.HaveBuff && Soulstone.KnownSpell && Soulstone.IsSpellUsable
            && MySettings.UseSoulstone)
        {
            Soulstone.Launch();
            return;
        }
        else
        {
            if (ItemsManager.GetItemCountByIdLUA(5512) == 0 && Create_Healthstone.KnownSpell
                && Create_Healthstone.IsSpellUsable && MySettings.UseCreateHealthstone)
            {
                Logging.WriteFight(" - Create Healthstone - ");
                Thread.Sleep(200);
                Create_Healthstone.Launch();
                Thread.Sleep(200);
                while (ObjectManager.Me.IsCast)
                {
                    Thread.Sleep(200);
                }
            }
        }
    }

    private void Pet()
    {
        if (Health_Funnel.KnownSpell)
        {
            if (ObjectManager.Pet.HealthPercent > 0 && ObjectManager.Pet.HealthPercent < 50
                && Health_Funnel.IsSpellUsable && Health_Funnel.KnownSpell && MySettings.UseHealthFunnel)
            {
                Health_Funnel.Launch();
                while (ObjectManager.Me.IsCast)
                {
                    if (ObjectManager.Pet.HealthPercent > 85 || ObjectManager.Pet.IsDead)
                        break;
                    Thread.Sleep(100);
                }
            }
        }

        if (Grimoire_of_Sacrifice.KnownSpell && !Grimoire_of_Sacrifice.HaveBuff && Grimoire_of_Sacrifice.IsSpellUsable
            && MySettings.UseGrimoireofSacrifice)
        {
            if ((ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0))
            {
                Logging.WriteFight(" - PET DEAD - ");
                if (Flames_of_Xoroth.KnownSpell && Flames_of_Xoroth.IsSpellUsable && MySettings.UseFlamesofXoroth)
                    Flames_of_Xoroth.Launch();
                else if (MySettings.UseSummonFelhunter && Summon_Felhunter.KnownSpell && Summon_Felhunter.IsSpellUsable)
                    Summon_Felhunter.Launch();
                else if (MySettings.UseSummonImp && Summon_Imp.KnownSpell && Summon_Imp.IsSpellUsable)
                    Summon_Imp.Launch();
                else if (MySettings.UseSummonVoidwalker && Summon_Voidwalker.KnownSpell && Summon_Voidwalker.IsSpellUsable)
                    Summon_Voidwalker.Launch();
                else
                {
                    if (MySettings.UseSummonSuccubus && Summon_Succubus.KnownSpell && Summon_Succubus.IsSpellUsable)
                        Summon_Succubus.Launch();
                }
                Thread.Sleep(200);
            }
            if ((ObjectManager.Pet.Health != 0 || ObjectManager.Pet.Guid != 0))
                Grimoire_of_Sacrifice.Launch();
            return;
        }
        else
        {
            if ((ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0) && !Grimoire_of_Sacrifice.KnownSpell)
            {
                Logging.WriteFight(" - PET DEAD - ");
                if (Flames_of_Xoroth.KnownSpell && Flames_of_Xoroth.IsSpellUsable && MySettings.UseFlamesofXoroth)
                    Flames_of_Xoroth.Launch();
                else if (MySettings.UseSummonFelhunter && Summon_Felhunter.KnownSpell && Summon_Felhunter.IsSpellUsable)
                    Summon_Felhunter.Launch();
                else if (MySettings.UseSummonImp && Summon_Imp.KnownSpell && Summon_Imp.IsSpellUsable)
                    Summon_Imp.Launch();
                else if (MySettings.UseSummonVoidwalker && Summon_Voidwalker.KnownSpell && Summon_Voidwalker.IsSpellUsable)
                    Summon_Voidwalker.Launch();
                else
                {
                    if (MySettings.UseSummonSuccubus && Summon_Succubus.KnownSpell && Summon_Succubus.IsSpellUsable)
                        Summon_Succubus.Launch();
                }
                return;
            }
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (ObjectManager.Me.HealthPercent < 80 && Gift_of_the_Naaru.IsSpellUsable && Gift_of_the_Naaru.KnownSpell
            && MySettings.UseGiftoftheNaaru)
        {
            Gift_of_the_Naaru.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 60 && Ember_Tap.IsSpellUsable && Ember_Tap.KnownSpell
            && MySettings.UseEmberTap)
        {
            Ember_Tap.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 65 && Dark_Regeneration.IsSpellUsable && Dark_Regeneration.KnownSpell
            && MySettings.UseDarkRegeneration)
        {
            Dark_Regeneration.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 75 && ItemsManager.GetItemCountByIdLUA(5512) > 0
            && MySettings.UseCreateHealthstone)
        {
            Logging.WriteFight("Use Healthstone.");
            nManager.Wow.Helpers.ItemsManager.UseItem("Healthstone");
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 85 && Mortal_Coil.IsSpellUsable && Mortal_Coil.KnownSpell
            && MySettings.UseMortalCoil && Mortal_Coil.IsDistanceGood)
        {
            Mortal_Coil.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Me.HealthPercent < 70 && Drain_Life.KnownSpell
                && MySettings.UseDrainLife && Drain_Life.IsDistanceGood && Drain_Life.IsSpellUsable)
            {
                Drain_Life.Launch();
                while (ObjectManager.Me.IsCast)
                {
                    Thread.Sleep(200);
                }
                return;
            }
        }
    }

    private void Defense_Cycle()
    {
        if (ObjectManager.Me.HealthPercent < 70 && MySettings.UseUnendingResolve
            && Unending_Resolve.KnownSpell && Unending_Resolve.IsSpellUsable)
        {
            Unending_Resolve.Launch();
            OnCD = new Timer(1000*8);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 20 && MySettings.UseHowlofTerror
            && Howl_of_Terror.KnownSpell && Howl_of_Terror.IsSpellUsable && ObjectManager.Target.GetDistance < 8)
        {
            Howl_of_Terror.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 40 && MySettings.UseDarkBargain
            && Dark_Bargain.KnownSpell && Dark_Bargain.IsSpellUsable)
        {
            Dark_Bargain.Launch();
            OnCD = new Timer(1000*8);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 95 && MySettings.UseSacrificialPact
            && Sacrificial_Pact.KnownSpell && Sacrificial_Pact.IsSpellUsable
            && (ObjectManager.Pet.Health != 0 || ObjectManager.Pet.Guid != 0))
        {
            Sacrificial_Pact.Launch();
            OnCD = new Timer(1000*10);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 90 && MySettings.UseShadowfury
            && Shadowfury.KnownSpell && Shadowfury.IsSpellUsable && ObjectManager.Target.GetDistance < 8)
        {
            Shadowfury.Launch();
            OnCD = new Timer(1000*3);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && War_Stomp.IsSpellUsable && War_Stomp.KnownSpell
            && MySettings.UseWarStomp)
        {
            War_Stomp.Launch();
            OnCD = new Timer(1000*2);
            return;
        }
        else
        {
            if (ObjectManager.Me.HealthPercent < 80 && Stoneform.IsSpellUsable && Stoneform.KnownSpell
                && MySettings.UseStoneform)
            {
                Stoneform.Launch();
                OnCD = new Timer(1000*8);
                return;
            }
        }
    }

    private void Decast()
    {
        if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && ObjectManager.Target.GetDistance < 8
            && Arcane_Torrent.KnownSpell && Arcane_Torrent.IsSpellUsable && MySettings.UseArcaneTorrent)
        {
            Arcane_Torrent.Launch();
            return;
        }
        else if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe
            && MySettings.UseTwilightWard && Twilight_Ward.KnownSpell && Twilight_Ward.IsSpellUsable)
        {
            Twilight_Ward.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && MySettings.UseSummonFelhunter
                && Command_Demon.IsSpellUsable && Command_Demon.KnownSpell && ObjectManager.Target.GetDistance < 40)
            {
                Command_Demon.Launch();
                return;
            }
        }
    }

    private void AvoidMelee()
    {
        if (ObjectManager.Target.GetDistance < 3 && ObjectManager.Target.InCombat)
        {
            nManager.Wow.Helpers.Keybindings.PressKeybindings(nManager.Wow.Enums.Keybindings.MOVEBACKWARD);
        }
    }
}

public class Warlock_Affliction
{
    [Serializable]
    public class WarlockAfflictionSettings : nManager.Helpful.Settings
    {
        /* Professions & Racials */
        public bool UseArcaneTorrent = true;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseLifeblood = true;
        public bool UseStoneform = true;
        public bool UseGiftoftheNaaru = true;
        public bool UseWarStomp = true;
        /* Warlock Buffs */
        public bool UseCurseofEnfeeblement = false;
        public bool UseCurseofExhaustion = false;
        public bool UseCurseoftheElements = true;
        public bool UseDarkIntent = true;
        public bool UseGrimoireofSacrifice = true;
        public bool UseSoulLink = true;
        public bool UseSoulstone = true;
        /* Offensive Spell */
        public bool UseAgony = true;
        public bool UseCommandDemon = true;
        public bool UseCorruption = true;
        public bool UseDrainSoul= true;
        public bool UseFelFlame = true;
        public bool UseHarvestLife = true;
        public bool UseHaunt = true;
        public bool UseMaleficGrasp = true;
        public bool UseRainofFire = true;
        public bool UseSeedofCorruption = true;
        public bool UseShadowBolt = true;
        public bool UseSoulSwap = true;
        public bool UseSoulburn = true;
        public bool UseSummonImp = false;
        public bool UseSummonVoidwalker = false;
        public bool UseSummonFelhunter = true;
        public bool UseSummonSuccubus = false;
        public bool UseUnstableAffliction = true;
        /* Offensive Cooldown */
        public bool UseArchimondesVengeance = true;
        public bool UseDarkSoul = true;
        public bool UseGrimoireofService = true;
        public bool UseSummonDoomguard = true;
        public bool UseSummonInfernal = false;
        /* Defensive Cooldown */
        public bool UseDarkBargain = true;
        public bool UseHowlofTerror = true;
        public bool UseSacrificialPact = true;
        public bool UseShadowfury = true;
        public bool UseTwilightWard = true;
        public bool UseUnboundWill = true;
        public bool UseUnendingResolve = true;
        /* Healing Spell */
        public bool UseCreateHealthstone = true;
        public bool UseDarkRegeneration = true;
        public bool UseDrainLife = true;
        public bool UseHealthFunnel = true;
        public bool UseLifeTap = true;
        public bool UseMortalCoil = true;
        /* Game Settings */
        public bool UseLowCombat = true;
        public bool UseTrinket = true;
        public bool UseEngGlove = true;
        public bool UseAlchFlask = true;

        public WarlockAfflictionSettings()
        {
            ConfigWinForm(new System.Drawing.Point(400, 400), "Warlock Affliction Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");
            AddControlInWinForm("Use Lifeblood", "UseLifeblood", "Professions & Racials");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            /* Warlock Buffs */
            AddControlInWinForm("Use Curse of Enfeeblement", "UseCurseofEnfeeblement", "Warlock Buffs");
            AddControlInWinForm("Use Curse of Exhaustion", "UseCurseofExhaustion", "Warlock Buffs");
            AddControlInWinForm("Use Curse of the Elements", "UseCurseoftheElements", "Warlock Buffs");
            AddControlInWinForm("Use Dark Intent", "UseDarkIntent", "Warlock Buffs");
            AddControlInWinForm("Use Grimoire of Sacrifice", "UseGrimoireofSacrifice", "Warlock Buffs");
            AddControlInWinForm("Use Soul Link ", "UseSoulLink ", "Warlock Buffs");
            AddControlInWinForm("Use Soulstone", "UseSoulstone", "Warlock Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Agony", "UseAgony", "Offensive Spell");
            AddControlInWinForm("Use Command Demon", "UseCommandDemon", "Offensive Spell");
            AddControlInWinForm("Use Corruption", "UseCorruption", "Offensive Spell");
            AddControlInWinForm("Use Drain Soul", "UseDrainSoul", "Offensive Spell");
            AddControlInWinForm("Use Fel Flame", "UseFelFlame", "Offensive Spell");
            AddControlInWinForm("Use Harvest Life", "UseHarvestLife", "Offensive Spell");
            AddControlInWinForm("Use Haunt", "UseHaunt", "Offensive Spell");
            AddControlInWinForm("Use Malefic Grasp", "UseMaleficGrasp", "Offensive Spell");
            AddControlInWinForm("Use Rain of Fire", "UseRainofFire", "Offensive Spell");
            AddControlInWinForm("Use Seed of Corruption", "UseSeedofCorruption", "Offensive Spell");
            AddControlInWinForm("Use Shadow Bolt", "UseShadowBolt", "Offensive Spell");
            AddControlInWinForm("Use Soul Swap", "UseSoulSwap", "Offensive Spell");
            AddControlInWinForm("Use Soulburn", "UseSoulburn", "Offensive Spell");
            AddControlInWinForm("Use Summon Imp", "UseSummonImp", "Offensive Spell");
            AddControlInWinForm("Use Summon Voidwalker", "UseSummonVoidwalker", "Offensive Spell");
            AddControlInWinForm("Use Summon Felhunter", "UseSummonFelhunter", "Offensive Spell");
            AddControlInWinForm("Use Summon Succubus", "UseSummonSuccubus", "Offensive Spell");
            AddControlInWinForm("Use Unstable Affliction", "UseUnstableAffliction", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use Archimonde's Vengeance", "UseArchimondesVengeance", "Offensive Cooldown");
            AddControlInWinForm("Use Dark Soul", "UseDarkSoul", "Offensive Cooldown");
            AddControlInWinForm("Use Grimoire of Service", "UseGrimoireofService", "Offensive Cooldown");
            AddControlInWinForm("Use Summon Doomguard", "UseSummonDoomguard", "Offensive Cooldown");
            AddControlInWinForm("Use Summon Infernal", "UseSummonInfernal", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Dark Bargain", "UseDarkBargain", "Defensive Cooldown");
            AddControlInWinForm("Use Howl of Terror", "UseHowlofTerror", "Defensive Cooldown");
            AddControlInWinForm("Use Sacrificial Pact", "UseSacrificialPact", "Defensive Cooldown");
            AddControlInWinForm("Use Shadowfury", "UseShadowfury", "Defensive Cooldown");
            AddControlInWinForm("Use Twilight Ward", "UseTwilightWard", "Defensive Cooldown");
            AddControlInWinForm("Use Unbound Will", "UseUnboundWill", "Defensive Cooldown");
            AddControlInWinForm("Use Unending Resolve", "UseUnendingResolve", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Create Healthstone", "UseCreateHealthstone", "Healing Spell");
            AddControlInWinForm("Use Dark Regeneration", "UseDarkRegeneration", "Healing Spell");
            AddControlInWinForm("Use Drain Life", "UseDrainLife", "Healing Spell");
            AddControlInWinForm("Use Health Funnel", "UseHealthFunnel", "Healing Spell");
            AddControlInWinForm("Use Life Tap", "UseLifeTap", "Healing Spell");
            AddControlInWinForm("Use Mortal Coil", "UseMortalCoil", "Healing Spell");
            /* Game Settings */
            AddControlInWinForm("Use Low Combat Settings", "UseLowCombat", "Game Settings");
            AddControlInWinForm("Use Trinket", "UseTrinket", "Game Settings");
            AddControlInWinForm("Use Engineering Gloves", "UseEngGlove", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
        }

        public static WarlockAfflictionSettings CurrentSetting { get; set; }

        public static WarlockAfflictionSettings GetSettings()
        {
            string CurrentSettingsFile = Application.StartupPath + "\\CustomClasses\\Settings\\Warlock_Affliction.xml";
            if (System.IO.File.Exists(CurrentSettingsFile))
            {
                return
                    CurrentSetting = Settings.Load<Warlock_Affliction.WarlockAfflictionSettings>(CurrentSettingsFile);
            }
            else
            {
                return new Warlock_Affliction.WarlockAfflictionSettings();
            }
        }
    }

    private readonly WarlockAfflictionSettings MySettings = WarlockAfflictionSettings.GetSettings();

    #region Professions & Racials

    private readonly Spell Arcane_Torrent = new Spell("Arcane Torrent");
    private readonly Spell Berserking = new Spell("Berserking");
    private readonly Spell Blood_Fury = new Spell("Blood Fury");
    private readonly Spell Lifeblood = new Spell("Lifeblood");
    private readonly Spell Stoneform = new Spell("Stoneform");
    private readonly Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    private readonly Spell War_Stomp = new Spell("War Stomp");
    private readonly Spell Engineering = new Spell("Engineering");
    private readonly Spell Alchemy = new Spell("Alchemy");

    #endregion

    #region Warlock Buffs

    private readonly Spell Curse_of_Enfeeblement = new Spell("Curse of Enfeeblement");
    private readonly Spell Curse_of_Exhaustion = new Spell("Curse of Exhaustion");
    private readonly Spell Curse_of_the_Elements = new Spell("Curse of the Elements");
    private readonly Spell Dark_Intent = new Spell("Dark Intent");
    private readonly Spell Grimoire_of_Sacrifice = new Spell("Grimoire of Sacrifice");
    private readonly Spell Soul_Link = new Spell("Soul Link");
    private readonly Spell Soulstone = new Spell("Soulstone");

    #endregion

    #region Offensive Spell

    private readonly Spell Agony = new Spell("Agony");
    private Timer Agony_Timer = new Timer(0);
    private readonly Spell Command_Demon = new Spell("Command Demon");
    private readonly Spell Corruption = new Spell("Corruption");
    private Timer Corruption_Timer = new Timer(0);
    private readonly Spell Drain_Soul = new Spell("Drain Soul");
    private readonly Spell Fel_Flame = new Spell("Fel Flame");
    private readonly Spell Harvest_Life = new Spell("Harvest Life");
    private readonly Spell Haunt = new Spell("Haunt");
    private readonly Spell Malefic_Grasp = new Spell("Malefic Grasp");
    private readonly Spell Rain_of_Fire = new Spell("Rain of Fire");
    private readonly Spell Seed_of_Corruption = new Spell("Seed of Corruption");
    private readonly Spell Shadow_Bolt = new Spell("Shadow Bolt");
    private readonly Spell Soul_Swap = new Spell("Soul Swap");
    private readonly Spell Soulburn = new Spell("Soulburn");
    private readonly Spell Summon_Imp = new Spell("Summon Imp");
    private readonly Spell Summon_Voidwalker = new Spell("Summon Voidwalker");
    private readonly Spell Summon_Felhunter = new Spell("Summon Felhunter");
    private readonly Spell Summon_Succubus = new Spell("Summon Succubus");
    private readonly Spell Summon_Felguard = new Spell("Summon Felguard");
    private readonly Spell Unstable_Affliction = new Spell("Unstable Affliction");
    private Timer Unstable_Affliction_Timer = new Timer(0);

    #endregion

    #region Offensive Cooldown

    private readonly Spell Archimondes_Vengeance = new Spell("Archimonde's Vengeance");
    private readonly Spell Dark_Soul = new Spell("Dark Soul");
    private readonly Spell Grimoire_of_Service = new Spell("Grimoire of Service");
    private readonly Spell Summon_Doomguard = new Spell("Summon Doomguard");
    private readonly Spell Summon_Infernal = new Spell("Summon Infernal");
    
    #endregion

    #region Defensive Cooldown

    private readonly Spell Dark_Bargain = new Spell("Dark Bargain");
    private readonly Spell Howl_of_Terror = new Spell("Howl_of_Terror");
    private readonly Spell Sacrificial_Pact = new Spell("Sacrificial Pact");
    private readonly Spell Shadowfury = new Spell("Shadowfury");
    private readonly Spell Twilight_Ward = new Spell("Twilight Ward");
    private readonly Spell Unbound_Will = new Spell("Unbound Will");
    private readonly Spell Unending_Resolve = new Spell("Unending Resolve");

    #endregion

    #region Healing Spell

    private readonly Spell Create_Healthstone = new Spell("Create Healthstone");
    private readonly Spell Dark_Regeneration = new Spell("Dark Regeneration");
    private readonly Spell Drain_Life = new Spell("Drain Life");
    private readonly Spell Health_Funnel = new Spell("Health Funnel");
    private readonly Spell Life_Tap = new Spell("Life Tap");
    private readonly Spell Mortal_Coil = new Spell("Mortal Coil");

    #endregion

    private Timer OnCD = new Timer(0);
    private Timer Trinket_Timer = new Timer(0);
    private Timer Engineering_Timer = new Timer(0);
    private Timer AlchFlask_Timer = new Timer(0);
    public int LC = 0;

    public Warlock_Affliction()
    {
        Main.range = 30.0f;
        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            try
            {
                if (!ObjectManager.Me.IsMounted)
                {
                    if (Fight.InFight && ObjectManager.Me.Target > 0)
                    {
                        if (ObjectManager.Me.Target != lastTarget &&
                            (Soul_Swap.IsDistanceGood || Agony.IsDistanceGood))
                        {
                            Pull();
                            lastTarget = ObjectManager.Me.Target;
                        }

                        if (ObjectManager.Target.Level < 70 && ObjectManager.Me.Level > 84
                            && MySettings.UseLowCombat)
                        {
                            LC = 1;
                            LowCombat();
                        }
                        else
                        {
                            LC = 0;
                            Combat();
                        }
                    }
                    else
                        Patrolling();
                }
            }
            catch
            {
            }
            Thread.Sleep(250);
        }
    }

    public void Pull()
    {
        if (!Agony.TargetHaveBuff && !Corruption.TargetHaveBuff && !Unstable_Affliction.TargetHaveBuff)
        {
            if (Soulburn.IsSpellUsable && Soulburn.KnownSpell && Soul_Swap.IsSpellUsable && Soul_Swap.KnownSpell
                && Soul_Swap.IsDistanceGood && MySettings.UseSoulSwap && MySettings.UseSoulburn)
            {
                if (!Soulburn.HaveBuff)
                {
                    Soulburn.Launch();
                    Thread.Sleep(200);
                }
                Soul_Swap.Launch();
                Agony_Timer = new Timer(1000*20);
                Corruption_Timer = new Timer(1000*15);
                Unstable_Affliction_Timer = new Timer(1000*10);
            }
        }
        return;
    }

    public void Combat()
    {
        AvoidMelee();
        if (OnCD.IsReady)
            Defense_Cycle();
        Heal();
        Decast();
        Buff();
        DPS_Burst();
        DPS_Cycle();
    }

    public void LowCombat()
    {
        AvoidMelee();
        Heal();
        Defense_Cycle();
        Buff();

        if (ObjectManager.Me.BarTwoPercentage < 75 && Life_Tap.KnownSpell && Life_Tap.IsSpellUsable
            && MySettings.UseLifeTap)
        {
            Life_Tap.Launch();
            return;
        }

        if (Malefic_Grasp.KnownSpell && Malefic_Grasp.IsSpellUsable && Malefic_Grasp.IsDistanceGood
            && MySettings.UseMaleficGrasp)
        {
            Malefic_Grasp.Launch();
            Thread.Sleep(200);
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(200);
            }
        }

        if (ObjectManager.Target.HealthPercent < 50 && ObjectManager.Target.HealthPercent > 0)
        {
            if (Malefic_Grasp.KnownSpell && Malefic_Grasp.IsSpellUsable && Malefic_Grasp.IsDistanceGood
                && MySettings.UseMaleficGrasp)
            {
                Malefic_Grasp.Launch();
                Thread.Sleep(200);
                while (ObjectManager.Me.IsCast)
                {
                    Thread.Sleep(200);
                }
            }
        }

        if (ObjectManager.Target.HealthPercent > 90)
        {
            if (Rain_of_Fire.IsSpellUsable && Rain_of_Fire.KnownSpell && Rain_of_Fire.IsDistanceGood
                && MySettings.UseRainofFire)
            {
                SpellManager.CastSpellByIDAndPosition(5740, ObjectManager.Target.Position);
                while (ObjectManager.Me.IsCast)
                {
                    Thread.Sleep(200);
                }
                return;
            }
        }
        return;
    }

    public void DPS_Burst()
    {
        if (MySettings.UseTrinket && Trinket_Timer.IsReady && ObjectManager.Target.GetDistance < 40)
        {
            Logging.WriteFight("Use Trinket 1.");
            Lua.RunMacroText("/use 13");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Logging.WriteFight("Use Trinket 2.");
            Lua.RunMacroText("/use 14");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Trinket_Timer = new Timer(1000*60*2);
            return;
        }
        else if (Berserking.IsSpellUsable && Berserking.KnownSpell && MySettings.UseBerserking
            && ObjectManager.Target.GetDistance < 40)
        {
            Berserking.Launch();
            return;
        }
        else if (Blood_Fury.IsSpellUsable && Blood_Fury.KnownSpell && MySettings.UseBloodFury
            && ObjectManager.Target.GetDistance < 40)
        {
            Blood_Fury.Launch();
            return;
        }
        else if (Lifeblood.IsSpellUsable && Lifeblood.KnownSpell && MySettings.UseLifeblood
            && ObjectManager.Target.GetDistance < 40)
        {
            Lifeblood.Launch();
            return;
        }
        else if (MySettings.UseEngGlove && Engineering.KnownSpell && Engineering_Timer.IsReady
            && ObjectManager.Target.GetDistance < 40)
        {
            Logging.WriteFight("Use Engineering Gloves.");
            Lua.RunMacroText("/use 10");
            Engineering_Timer = new Timer(1000*60);
            return;
        }
        else if (Dark_Soul.KnownSpell && Dark_Soul.IsSpellUsable
            && MySettings.UseDarkSoul && ObjectManager.Target.GetDistance < 40)
        {
            Dark_Soul.Launch();
            return;
        }
        else if (Summon_Doomguard.KnownSpell && Summon_Doomguard.IsSpellUsable
            && MySettings.UseSummonDoomguard && Summon_Doomguard.IsDistanceGood)
        {
            Summon_Doomguard.Launch();
            return;
        }
        else if (Summon_Infernal.KnownSpell && Summon_Infernal.IsSpellUsable
            && MySettings.UseSummonInfernal && Summon_Infernal.IsDistanceGood)
        {
            SpellManager.CastSpellByIDAndPosition(1122, ObjectManager.Target.Position);
            return;
        }
        else if (Archimondes_Vengeance.KnownSpell && Archimondes_Vengeance.IsSpellUsable
            && MySettings.UseArchimondesVengeance)
        {
            Archimondes_Vengeance.Launch();
            return;
        }
        else
        {
            if (Grimoire_of_Service.KnownSpell && Grimoire_of_Service.IsSpellUsable
                && MySettings.UseGrimoireofService && ObjectManager.Target.GetDistance < 40)
            {
                Grimoire_of_Service.Launch();
                return;
            }
        }
    }

    public void DPS_Cycle()
    {
        if (Curse_of_the_Elements.KnownSpell && Curse_of_the_Elements.IsSpellUsable && MySettings.UseCurseoftheElements
            && Curse_of_the_Elements.IsDistanceGood && !Curse_of_the_Elements.TargetHaveBuff)
        {
            Curse_of_the_Elements.Launch();
            return;
        }
        else if (Curse_of_Enfeeblement.KnownSpell && Curse_of_Enfeeblement.IsSpellUsable && MySettings.UseCurseofEnfeeblement
            && Curse_of_Enfeeblement.IsDistanceGood && !Curse_of_Enfeeblement.TargetHaveBuff && !MySettings.UseCurseoftheElements)
        {
            Curse_of_Enfeeblement.Launch();
            return;
        }
        else if (Curse_of_Exhaustion.KnownSpell && Curse_of_Exhaustion.IsSpellUsable && MySettings.UseCurseofExhaustion
            && Curse_of_Exhaustion.IsDistanceGood && !Curse_of_Exhaustion.TargetHaveBuff && !MySettings.UseCurseoftheElements
            && !MySettings.UseCurseofEnfeeblement)
        {
            Curse_of_Exhaustion.Launch();
            return;
        }
        else if (ObjectManager.Me.BarTwoPercentage < 75 && Life_Tap.KnownSpell && Life_Tap.IsSpellUsable
            && MySettings.UseLifeTap)
        {
            Life_Tap.Launch();
            return;
        }
        else if (ObjectManager.Target.HealthPercent < 20)
        {
            if (Drain_Soul.KnownSpell && Drain_Soul.IsSpellUsable && MySettings.UseDrainSoul && Drain_Soul.IsDistanceGood)
            {
                Drain_Soul.Launch();
                while (ObjectManager.Me.IsCast && !Agony_Timer.IsReady && !Corruption_Timer.IsReady
                    && !Unstable_Affliction_Timer.IsReady)
                {
                    Thread.Sleep(200);
                }
            }

            if (Agony_Timer.IsReady || Corruption_Timer.IsReady || Unstable_Affliction_Timer.IsReady)
            {
                if (Soulburn.IsSpellUsable && Soulburn.KnownSpell && Soul_Swap.IsSpellUsable && Soul_Swap.KnownSpell
                    && Soul_Swap.IsDistanceGood && MySettings.UseSoulburn && MySettings.UseSoulSwap)
                {
                    Soulburn.Launch();
                    Thread.Sleep(200);
                    Soul_Swap.Launch();
                    Agony_Timer = new Timer(1000*20);
                    Corruption_Timer = new Timer(1000*15);
                    Unstable_Affliction_Timer = new Timer(1000*10);
                }
            }
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 4 && Soulburn.IsSpellUsable && Soulburn.KnownSpell && !Corruption.TargetHaveBuff
            && Seed_of_Corruption.IsSpellUsable && Seed_of_Corruption.KnownSpell && Seed_of_Corruption.IsDistanceGood
            && MySettings.UseSoulburn && MySettings.UseSeedofCorruption)
        {
            Soulburn.Launch();
            Thread.Sleep(200);
            Seed_of_Corruption.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 4 && Harvest_Life.IsSpellUsable && Harvest_Life.KnownSpell
            && Harvest_Life.IsDistanceGood && MySettings.UseHarvestLife)
        {
            Harvest_Life.Launch();
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(200);
            }
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 4 && Rain_of_Fire.IsSpellUsable && Rain_of_Fire.KnownSpell
            && Rain_of_Fire.IsDistanceGood && MySettings.UseRainofFire)
        {
            SpellManager.CastSpellByIDAndPosition(5740, ObjectManager.Target.Position);
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(200);
            }
            return;
        }
        else if (Agony.KnownSpell && Agony.IsSpellUsable && Agony.IsDistanceGood && MySettings.UseAgony
            && (!Agony.TargetHaveBuff || Agony_Timer.IsReady))
        {
            Agony.Launch();
            Agony_Timer = new Timer(1000*20);
            return;
        }
        else if (Corruption.KnownSpell && Corruption.IsSpellUsable && Corruption.IsDistanceGood
            && MySettings.UseCorruption && (!Corruption.TargetHaveBuff || Corruption_Timer.IsReady))
        {
            Corruption.Launch();
            Corruption_Timer = new Timer(1000*15);
            return;
        }
        else if (Unstable_Affliction.KnownSpell && Unstable_Affliction.IsSpellUsable && Unstable_Affliction.IsDistanceGood
            && MySettings.UseUnstableAffliction && (!Unstable_Affliction.TargetHaveBuff || Unstable_Affliction_Timer.IsReady))
        {
            Unstable_Affliction.Launch();
            Unstable_Affliction_Timer = new Timer(1000*10);
            return;
        }
        else if (Haunt.KnownSpell && Haunt.IsSpellUsable && Haunt.IsDistanceGood && !Haunt.TargetHaveBuff
            && MySettings.UseHaunt)
        {
            Haunt.Launch();
            return;
        }
        // Blizzard API Calls for Malefic Grasp using Shadow Bolt Function
        else
        {
            if (!ObjectManager.Me.IsCast && Shadow_Bolt.KnownSpell && Shadow_Bolt.IsSpellUsable
                && !Agony_Timer.IsReady && !Corruption_Timer.IsReady && !Unstable_Affliction_Timer.IsReady
                && Shadow_Bolt.IsDistanceGood && MySettings.UseMaleficGrasp)
            {
                Shadow_Bolt.Launch();
                return;
            }
        }
    }

    public void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            Heal();
            Buff();
        }
    }

    private void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        Pet();

        if (!Dark_Intent.HaveBuff && Dark_Intent.KnownSpell && Dark_Intent.IsSpellUsable
            && MySettings.UseDarkIntent)
        {
            Dark_Intent.Launch();
            return;
        }
        else if (!Soul_Link.HaveBuff && Soul_Link.KnownSpell && Soul_Link.IsSpellUsable
            && MySettings.UseSoulLink && (ObjectManager.Pet.Health != 0 || ObjectManager.Pet.Guid != 0))
        {
            Soul_Link.Launch();
            return;
        }
        if (!Soulstone.HaveBuff && Soulstone.KnownSpell && Soulstone.IsSpellUsable
            && MySettings.UseSoulstone)
        {
            Soulstone.Launch();
            return;
        }
        else
        {
            if (ItemsManager.GetItemCountByIdLUA(5512) == 0 && Create_Healthstone.KnownSpell
                && Create_Healthstone.IsSpellUsable && MySettings.UseCreateHealthstone)
            {
                Logging.WriteFight(" - Create Healthstone - ");
                Thread.Sleep(200);
                Create_Healthstone.Launch();
                Thread.Sleep(200);
                while (ObjectManager.Me.IsCast)
                {
                    Thread.Sleep(200);
                }
            }
        }
    }

    private void Pet()
    {
        if (Health_Funnel.KnownSpell)
        {
            if (ObjectManager.Pet.HealthPercent > 0 && ObjectManager.Pet.HealthPercent < 50
                && Health_Funnel.IsSpellUsable && Health_Funnel.KnownSpell && MySettings.UseHealthFunnel)
            {
                Health_Funnel.Launch();
                while (ObjectManager.Me.IsCast)
                {
                    if (ObjectManager.Pet.HealthPercent > 85 || ObjectManager.Pet.IsDead)
                        break;
                    Thread.Sleep(100);
                }
            }
        }

        if (Grimoire_of_Sacrifice.KnownSpell && !Grimoire_of_Sacrifice.HaveBuff && Grimoire_of_Sacrifice.IsSpellUsable
            && MySettings.UseGrimoireofSacrifice)
        {
            if ((ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0))
            {
                Logging.WriteFight(" - PET DEAD Summon to Sacrifice - ");
                if (MySettings.UseSummonFelhunter && Summon_Felhunter.KnownSpell && Summon_Felhunter.IsSpellUsable)
                    Summon_Felhunter.Launch();
                else if (MySettings.UseSummonImp && Summon_Imp.KnownSpell && Summon_Imp.IsSpellUsable)
                    Summon_Imp.Launch();
                else if (MySettings.UseSummonVoidwalker && Summon_Voidwalker.KnownSpell && Summon_Voidwalker.IsSpellUsable)
                    Summon_Voidwalker.Launch();
                else
                {
                    if (MySettings.UseSummonSuccubus && Summon_Succubus.KnownSpell && Summon_Succubus.IsSpellUsable)
                        Summon_Succubus.Launch();
                }
                Thread.Sleep(200);
            }
            if ((ObjectManager.Pet.Health != 0 || ObjectManager.Pet.Guid != 0))
                Grimoire_of_Sacrifice.Launch();
            return;
        }
        else
        {
            if ((ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0) && !Grimoire_of_Sacrifice.KnownSpell)
            {
                Logging.WriteFight(" - PET DEAD - ");
                if (MySettings.UseSummonFelhunter && Summon_Felhunter.KnownSpell && Summon_Felhunter.IsSpellUsable)
                    Summon_Felhunter.Launch();
                else if (MySettings.UseSummonImp && Summon_Imp.KnownSpell && Summon_Imp.IsSpellUsable)
                    Summon_Imp.Launch();
                else if (MySettings.UseSummonVoidwalker && Summon_Voidwalker.KnownSpell && Summon_Voidwalker.IsSpellUsable)
                    Summon_Voidwalker.Launch();
                else
                {
                    if (MySettings.UseSummonSuccubus && Summon_Succubus.KnownSpell && Summon_Succubus.IsSpellUsable)
                        Summon_Succubus.Launch();
                }
                return;
            }
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (ObjectManager.Me.HealthPercent < 80 && Gift_of_the_Naaru.IsSpellUsable && Gift_of_the_Naaru.KnownSpell
            && MySettings.UseGiftoftheNaaru)
        {
            Gift_of_the_Naaru.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 65 && Dark_Regeneration.IsSpellUsable && Dark_Regeneration.KnownSpell
            && MySettings.UseDarkRegeneration)
        {
            Dark_Regeneration.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 75 && ItemsManager.GetItemCountByIdLUA(5512) > 0
            && MySettings.UseCreateHealthstone)
        {
            Logging.WriteFight("Use Healthstone.");
            nManager.Wow.Helpers.ItemsManager.UseItem("Healthstone");
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 85 && Mortal_Coil.IsSpellUsable && Mortal_Coil.KnownSpell
            && MySettings.UseMortalCoil && Mortal_Coil.IsDistanceGood)
        {
            Mortal_Coil.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Me.HealthPercent < 70 && Drain_Life.KnownSpell
                && MySettings.UseDrainLife && Drain_Life.IsDistanceGood && Drain_Life.IsSpellUsable)
            {
                Drain_Life.Launch();
                while (ObjectManager.Me.IsCast)
                {
                    Thread.Sleep(200);
                }
                return;
            }
        }
    }

    private void Defense_Cycle()
    {
        if (ObjectManager.Me.HealthPercent < 70 && MySettings.UseUnendingResolve
            && Unending_Resolve.KnownSpell && Unending_Resolve.IsSpellUsable)
        {
            Unending_Resolve.Launch();
            OnCD = new Timer(1000*8);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 20 && MySettings.UseHowlofTerror
            && Howl_of_Terror.KnownSpell && Howl_of_Terror.IsSpellUsable && ObjectManager.Target.GetDistance < 8)
        {
            Howl_of_Terror.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 40 && MySettings.UseDarkBargain
            && Dark_Bargain.KnownSpell && Dark_Bargain.IsSpellUsable)
        {
            Dark_Bargain.Launch();
            OnCD = new Timer(1000*8);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 95 && MySettings.UseSacrificialPact
            && Sacrificial_Pact.KnownSpell && Sacrificial_Pact.IsSpellUsable
            && (ObjectManager.Pet.Health != 0 || ObjectManager.Pet.Guid != 0))
        {
            Sacrificial_Pact.Launch();
            OnCD = new Timer(1000*10);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 90 && MySettings.UseShadowfury
            && Shadowfury.KnownSpell && Shadowfury.IsSpellUsable && ObjectManager.Target.GetDistance < 8)
        {
            Shadowfury.Launch();
            OnCD = new Timer(1000*3);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && War_Stomp.IsSpellUsable && War_Stomp.KnownSpell
            && MySettings.UseWarStomp)
        {
            War_Stomp.Launch();
            OnCD = new Timer(1000*2);
            return;
        }
        else
        {
            if (ObjectManager.Me.HealthPercent < 80 && Stoneform.IsSpellUsable && Stoneform.KnownSpell
                && MySettings.UseStoneform)
            {
                Stoneform.Launch();
                OnCD = new Timer(1000*8);
                return;
            }
        }
    }

    private void Decast()
    {
        if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && ObjectManager.Target.GetDistance < 8
            && Arcane_Torrent.KnownSpell && Arcane_Torrent.IsSpellUsable && MySettings.UseArcaneTorrent)
        {
            Arcane_Torrent.Launch();
            return;
        }
        else if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe
            && MySettings.UseTwilightWard && Twilight_Ward.KnownSpell && Twilight_Ward.IsSpellUsable)
        {
            Twilight_Ward.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && MySettings.UseSummonFelhunter
                && Command_Demon.IsSpellUsable && Command_Demon.KnownSpell && ObjectManager.Target.GetDistance < 40)
            {
                Command_Demon.Launch();
                return;
            }
        }
    }

    private void AvoidMelee()
    {
        if (ObjectManager.Target.GetDistance < 3 && ObjectManager.Target.InCombat)
        {
            nManager.Wow.Helpers.Keybindings.PressKeybindings(nManager.Wow.Enums.Keybindings.MOVEBACKWARD);
        }
    }
}

public class Warlock
{
    #region InitializeSpell

    private Spell Conflagrate = new Spell("Conflagrate");
    private Spell Corruption = new Spell("Corruption");
    private Spell Create_Healthstone = new Spell("Create Healthstone");
    private Spell Dark_Regeneration = new Spell("Dark Regeneration");
    private Spell Drain_Life = new Spell("Drain Life");
    private Spell Fear = new Spell("Fear");
    private Spell Harvest_Life = new Spell("Harvest Life");
    private Spell Health_Funnel = new Spell("Health Funnel");
    private Spell Shadow_Bolt = new Spell("Shadow Bolt");
    private Spell Soul_Fire = new Spell("Soul Fire");
    private Spell Summon_Imp = new Spell("Summon Imp");
    private Spell Summon_Voidwalker = new Spell("Summon Voidwalker");
    private Spell Unstable_Affliction = new Spell("Unstable Affliction");

    private Timer Corruption_Timer = new Timer(0);
    private Timer Unstable_Affliction_Timer = new Timer(0);
    private Timer Trink_Timer = new Timer(0);
    private Timer Suffering_Timer = new Timer(0);

    // profession & racials

    private Spell Arcane_Torrent = new Spell("Arcane Torrent");
    private Spell Berserking = new Spell("Berserking");
    private Spell Blood_Fury = new Spell("Blood Fury");
    private Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    private Spell Lifeblood = new Spell("Lifeblood");
    private Spell Stoneform = new Spell("Stoneform");

    #endregion InitializeSpell

    public Warlock()
    {
        Main.range = 30.0f;
        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            if (!ObjectManager.Me.IsMounted)
            {
                Patrolling();

                if (Fight.InFight && ObjectManager.Me.Target > 0)
                {
                    if (ObjectManager.Me.Target != lastTarget)
                    {
                        lastTarget = ObjectManager.Me.Target;
                    }
                    Combat();
                }
            }
            Thread.Sleep(400);
        }
    }

    public void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            Buff();
        }
    }

    public void Buff()
    {
        Pet();

        if (ItemsManager.GetItemCountByIdLUA(5512) == 0 && Create_Healthstone.KnownSpell &&
            Create_Healthstone.IsSpellUsable)
        {
            Logging.WriteFight(" - Create Healthstone - ");
            Thread.Sleep(200);
            Create_Healthstone.Launch();
            Thread.Sleep(200);
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(200);
            }
        }
    }

    public void Combat()
    {
        AvoidMelee();
        Heal();
        Resistance();
        Buff();

        if (Trink_Timer.IsReady)
        {
            if (Berserking.IsSpellUsable && Berserking.KnownSpell)
            {
                Berserking.Launch();
            }

            if (Blood_Fury.IsSpellUsable && Blood_Fury.KnownSpell)
            {
                Blood_Fury.Launch();
            }

            if (Lifeblood.IsSpellUsable && Lifeblood.KnownSpell)
            {
                Lifeblood.Launch();
            }

            Logging.WriteFight("Use Trinket 1.");
            Lua.RunMacroText("/use 13");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Logging.WriteFight("Use Trinket 2.");
            Lua.RunMacroText("/use 14");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Trink_Timer = new Timer(1000 * 60 * 2);
            return;
        }

        if (ObjectManager.GetNumberAttackPlayer() > 4 && Harvest_Life.IsSpellUsable && Harvest_Life.KnownSpell
            && Harvest_Life.IsDistanceGood)
        {
            Harvest_Life.Launch();
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(200);
            }
            return;
        }

        if (Conflagrate.KnownSpell && Conflagrate.IsSpellUsable && Conflagrate.IsDistanceGood)
        {
            Conflagrate.Launch();
            return;
        }

        if (Corruption.KnownSpell && Corruption.IsSpellUsable && Corruption.IsDistanceGood
            && (!Corruption.TargetHaveBuff || Corruption_Timer.IsReady))
        {
            Corruption.Launch();
            Corruption_Timer = new Timer(1000 * 15);
            return;
        }

        if (Unstable_Affliction.KnownSpell && Unstable_Affliction.IsSpellUsable && Unstable_Affliction.IsDistanceGood
            && (!Unstable_Affliction.TargetHaveBuff || Unstable_Affliction_Timer.IsReady))
        {
            Unstable_Affliction.Launch();
            Unstable_Affliction_Timer = new Timer(1000 * 10);
            return;
        }

        if (Soul_Fire.KnownSpell && Soul_Fire.IsSpellUsable && Soul_Fire.IsDistanceGood && ObjectManager.Me.HaveBuff(122355))
        {
            Soul_Fire.Launch();
            return;
        }

        if (Shadow_Bolt.KnownSpell && Shadow_Bolt.IsSpellUsable && Shadow_Bolt.IsDistanceGood)
        {
            Shadow_Bolt.Launch();
            return;
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.HealthPercent < 80 && Gift_of_the_Naaru.IsSpellUsable && Gift_of_the_Naaru.KnownSpell)
        {
            Gift_of_the_Naaru.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 75 && ItemsManager.GetItemCountByIdLUA(5512) > 0)
        {
            Logging.WriteFight("Use Healthstone.");
            nManager.Wow.Helpers.ItemsManager.UseItem("Healthstone");
            return;
        }

        if (ObjectManager.Me.HealthPercent < 65 && Dark_Regeneration.IsSpellUsable && Dark_Regeneration.KnownSpell)
        {
            Dark_Regeneration.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 35 && Drain_Life.KnownSpell &&
            Drain_Life.IsDistanceGood && Drain_Life.IsSpellUsable)
        {
            //SpellManager.CastSpellByIdLUA(689);
            Drain_Life.Launch();
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(200);
            }
            return;
        }
    }

    private void Resistance()
    {
        if (ObjectManager.Me.HealthPercent < 80 && Stoneform.IsSpellUsable && Stoneform.KnownSpell)
        {
            Stoneform.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 90 && Summon_Voidwalker.KnownSpell && ObjectManager.Me.Level > 14
            && Suffering_Timer.IsReady)
        {
            Logging.WriteFight("Cast Voidwalker: Suffering.");
            Lua.RunMacroText("/cast Voidwalker: Suffering");
            Suffering_Timer = new Timer(1000 * 10);
            return;
        }
    }

    private void Pet()
    {
        if (Health_Funnel.KnownSpell)
        {
            if (ObjectManager.Pet.HealthPercent > 0 && ObjectManager.Pet.HealthPercent < 50 &&
                Health_Funnel.IsSpellUsable)
            {
                SpellManager.CastSpellByIdLUA(755);
                // Health_Funnel.Launch();
                while (ObjectManager.Me.IsCast)
                {
                    if (ObjectManager.Pet.HealthPercent > 85 || ObjectManager.Pet.IsDead)
                        break;
                    Thread.Sleep(100);
                }
            }
        }

        if (Summon_Voidwalker.KnownSpell)
        {
            if (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0)
            {
                Logging.WriteFight(" - PET DEAD - ");
                if (Summon_Voidwalker.KnownSpell && Summon_Voidwalker.IsSpellUsable)
                {
                    Summon_Voidwalker.Launch();
                    return;
                }
            }
        }

        else
        {
            if (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0)
            {
                Logging.WriteFight(" - PET DEAD - ");
                if (Summon_Imp.KnownSpell && Summon_Imp.IsSpellUsable)
                {
                    Summon_Imp.Launch();
                    return;
                }
            }
        }
    }

    private void AvoidMelee()
    {
        if (ObjectManager.Target.GetDistance < 1)
        {
            Keyboard.DownKey(nManager.Wow.Memory.WowProcess.MainWindowHandle, "{DOWN}");
        }
    }
}

#endregion
