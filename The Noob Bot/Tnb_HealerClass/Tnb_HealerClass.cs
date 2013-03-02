/*
* CombatClass for TheNoobBot
* Credit : Rival, Geesus, Enelya, Marstor, Vesper, Neo2003, Dreadlocks
* Thanks you !
*/

using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using nManager.Helpful;
using nManager.Wow;
using nManager.Wow.Class;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;
using Keybindings = nManager.Wow.Helpers.Keybindings;
using Point = System.Drawing.Point;
using Timer = nManager.Helpful.Timer;

public class Main : IHealerClass
{
    internal static float range = 30f;
    internal static bool loop = true;

    #region IHealerClass Members

    public float Range
    {
        get { return range; }
        set { range = value; }
    }

    public void Initialize()
    {
        Initialize(false);
    }

    public void Dispose()
    {
        Logging.WriteFight("Healing system stopped.");
        loop = false;
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
            if (!loop)
                loop = true;
            Logging.WriteFight("Loading healing system.");
            switch (ObjectManager.Me.WowClass)
            {
                    #region Non healer classes detection

                case WoWClass.DeathKnight:
                case WoWClass.Mage:
                case WoWClass.Warlock:
                case WoWClass.Rogue:
                case WoWClass.Warrior:
                case WoWClass.Hunter:
                    string error = "Class " + ObjectManager.Me.WowClass + " can't be a healer.";
                    if (configOnly)
                        MessageBox.Show(error);
                    Logging.WriteFight(error);
                    break;

                    #endregion

                    #region Druid Specialisation checking

                case WoWClass.Druid:
                    var druidRestorationSpell = new Spell("Swiftmend");

                    if (druidRestorationSpell.KnownSpell)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath +
                                                         "\\CombatClasses\\Settings\\Druid_Restoration.xml";
                            var currentSetting = new Druid_Restoration.DruidRestorationSettings();
                            if (File.Exists(currentSettingsFile) && !resetSettings)
                            {
                                currentSetting =
                                    Settings.Load<Druid_Restoration.DruidRestorationSettings>(currentSettingsFile);
                            }
                            currentSetting.ToForm();
                            currentSetting.Save(currentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Druid Restoration Found");
                            range = 30f;
                            new Druid_Restoration();
                        }
                    }
                    else
                    {
                        string druidNonRestauration = "Class " + ObjectManager.Me.WowClass +
                                                      " can be a healer, but only in Restoration specialisation.";
                        if (configOnly)
                            MessageBox.Show(druidNonRestauration);
                        Logging.WriteFight(druidNonRestauration);
                    }
                    break;

                    #endregion

                    #region Paladin Specialisation checking

                case WoWClass.Paladin:
                    var paladinHolySpell = new Spell("Holy Shock");
                    if (paladinHolySpell.KnownSpell)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath +
                                                         "\\CombatClasses\\Settings\\Paladin_Holy.xml";
                            var currentSetting = new Paladin_Holy.PaladinHolySettings();
                            if (File.Exists(currentSettingsFile) && !resetSettings)
                            {
                                currentSetting = Settings.Load<Paladin_Holy.PaladinHolySettings>(currentSettingsFile);
                            }
                            currentSetting.ToForm();
                            currentSetting.Save(currentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Paladin Holy class...");
                            range = 30f;
                            new Paladin_Holy();
                        }
                    }
                    else
                    {
                        string paladinNonHoly = "Class " + ObjectManager.Me.WowClass +
                                                " can be a healer, but only in Holy specialisation.";
                        if (configOnly)
                            MessageBox.Show(paladinNonHoly);
                        Logging.WriteFight(paladinNonHoly);
                    }
                    break;

                    #endregion

                    #region Shaman Specialisation checking

                case WoWClass.Shaman:
                    var shamanRestorationSpell = new Spell("Riptide");

                    if (shamanRestorationSpell.KnownSpell)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath +
                                                         "\\CombatClasses\\Settings\\Shaman_Restoration.xml";
                            var currentSetting = new Shaman_Restoration.ShamanRestorationSettings();
                            if (File.Exists(currentSettingsFile) && !resetSettings)
                            {
                                currentSetting =
                                    Settings.Load<Shaman_Restoration.ShamanRestorationSettings>(currentSettingsFile);
                            }
                            currentSetting.ToForm();
                            currentSetting.Save(currentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Shaman Restoration class...");
                            range = 30f;
                            new Shaman_Restoration();
                        }
                    }
                    else
                    {
                        string shamanNonRestauration = "Class " + ObjectManager.Me.WowClass +
                                                       " can be a healer, but only in Restoration specialisation.";
                        if (configOnly)
                            MessageBox.Show(shamanNonRestauration);
                        Logging.WriteFight(shamanNonRestauration);
                    }
                    break;

                    #endregion

                    #region Priest Specialisation checking

                case WoWClass.Priest:
                    var priestDisciplineSpell = new Spell("Penance");
                    var priestHolySpell = new Spell("Holy Word: Chastise");
                    if (priestDisciplineSpell.KnownSpell)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath +
                                                         "\\CombatClasses\\Settings\\Priest_Discipline.xml";
                            var currentSetting = new Priest_Discipline.PriestDisciplineSettings();
                            if (File.Exists(currentSettingsFile) && !resetSettings)
                            {
                                currentSetting =
                                    Settings.Load<Priest_Discipline.PriestDisciplineSettings>(currentSettingsFile);
                            }
                            currentSetting.ToForm();
                            currentSetting.Save(currentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Priest Discipline class...");
                            range = 30f;
                            new Priest_Discipline();
                        }
                    }
                    else if (priestHolySpell.KnownSpell)
                    {
                        if (configOnly)
                        {
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CombatClasses\\Settings\\Priest_Holy.xml";
                            Priest_Holy.PriestHolySettings CurrentSetting;
                            CurrentSetting = new Priest_Holy.PriestHolySettings();
                            if (File.Exists(CurrentSettingsFile) && !resetSettings)
                            {
                                CurrentSetting =
                                    Settings.Load<Priest_Holy.PriestHolySettings>(CurrentSettingsFile);
                            }
                            CurrentSetting.ToForm();
                            CurrentSetting.Save(CurrentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Priest Holy class...");
                            range = 30f;
                            new Priest_Holy();
                        }
                    }
                    else
                    {
                        string priestNonHoly = "Class " + ObjectManager.Me.WowClass +
                                               " can be a healer, but only in Holy or Discipline specialisation.";
                        if (configOnly)
                            MessageBox.Show(priestNonHoly);
                        Logging.WriteFight(priestNonHoly);
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
        Logging.WriteFight("Healing system stopped.");
    }
}

#region Druid

public class Druid_Restoration
{
    private readonly string MoveBackward =
        Keybindings.GetKeyByAction(nManager.Wow.Enums.Keybindings.MOVEBACKWARD);

    private readonly string MoveForward =
        Keybindings.GetKeyByAction(nManager.Wow.Enums.Keybindings.MOVEFORWARD);

    private readonly DruidRestorationSettings _mySettings = DruidRestorationSettings.GetSettings();

    #region General Timers & Variables

    private Timer AlchFlask_Timer = new Timer(0);
    private Timer Engineering_Timer = new Timer(0);
    private Timer OnCD = new Timer(0);
    private Timer Trinket_Timer = new Timer(0);

    #endregion

    #region Professions & Racials

    private readonly Spell Alchemy = new Spell("Alchemy");
    private readonly Spell Arcane_Torrent = new Spell("Arcane Torrent");
    private readonly Spell Berserking = new Spell("Berserking");
    private readonly Spell Blood_Fury = new Spell("Blood Fury");
    private readonly Spell Engineering = new Spell("Engineering");
    private readonly Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    private readonly Spell Lifeblood = new Spell("Lifeblood");
    private readonly Spell Stoneform = new Spell("Stoneform");
    private readonly Spell War_Stomp = new Spell("War Stomp");

    #endregion

    #region Druid Buffs

    private readonly Spell Dash = new Spell("Dash");
    private readonly Spell Faerie_Fire = new Spell("Faerie Fire");
    private readonly Spell Mark_of_the_Wild = new Spell("Mark of the Wild");
    private readonly Spell Stampeding_Roar = new Spell("Stampeding Roar");

    #endregion

    #region Offensive Spell

    private readonly Spell Hurricane = new Spell("Hurricane");
    private readonly Spell Moonfire = new Spell("Moonfire");
    private readonly Spell Wrath = new Spell("Wrath");
    private Timer Moonfire_Timer = new Timer(0);

    #endregion

    #region Healing Cooldown

    private readonly Spell Force_of_Nature = new Spell("Force of Nature");
    private readonly Spell Incarnation = new Spell("Incarnation");

    #endregion

    #region Defensive Cooldown

    private readonly Spell Barkskin = new Spell("Barkskin");
    private readonly Spell Disorienting_Roar = new Spell("Disorienting Roar");
    private readonly Spell Entangling_Roots = new Spell("Entangling Roots");
    private readonly Spell Ironbark = new Spell("Ironbark");
    private readonly Spell Mass_Entanglement = new Spell("Mass Entanglement");
    private readonly Spell Mighty_Bash = new Spell("Mighty Bash");
    private readonly Spell Natures_Grasp = new Spell("Nature's Grasp");
    private readonly Spell Solar_Beam = new Spell("Solar Beam");
    private readonly Spell Typhoon = new Spell("Typhoon");
    private readonly Spell Ursols_Vortex = new Spell("Ursol's Vortex");
    private readonly Spell Wild_Charge = new Spell("Wild Charge");

    #endregion

    #region Healing Spell

    private readonly Spell Cenarion_Ward = new Spell("Cenarion Ward");
    private readonly Spell Healing_Touch = new Spell("Healing Touch");
    private readonly Spell Innervate = new Spell("Innervate");
    private readonly Spell Lifebloom = new Spell("Lifebloom");
    private readonly Spell Might_of_Ursoc = new Spell("Might of Ursoc");
    private readonly Spell Natures_Swiftness = new Spell("Nature's Swiftness");
    private readonly Spell Nourish = new Spell("Nourish");
    private readonly Spell Regrowth = new Spell("Regrowth");
    private readonly Spell Rejuvenation = new Spell("Rejuvenation");
    private readonly Spell Renewal = new Spell("Renewal");
    private readonly Spell Swiftmend = new Spell("Swiftmend");
    private readonly Spell Tranquility = new Spell("Tranquility");
    private readonly Spell Wild_Growth = new Spell("Wild Growth");
    private readonly Spell Wild_Mushroom = new Spell("Wild Mushroom");
    private readonly Spell Wild_Mushroom_Bloom = new Spell("Wild Mushroom: Bloom");
    private Timer Healing_Touch_Timer = new Timer(0);
    private Timer Nourish_Timer = new Timer(0);

    #endregion

    public Druid_Restoration()
    {
        Main.range = 30f;
        UInt64 lastTarget = 0;

        while (Main.loop)
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
                                && (Moonfire.IsDistanceGood || Wrath.IsDistanceGood))
                            {
                                Pull();
                                lastTarget = ObjectManager.Me.Target;
                            }

                            if (ObjectManager.Target.GetDistance < 41)
                                Combat();
                        }
                        else if (!ObjectManager.Me.IsCast)
                            Patrolling();
                    }
                }
                Thread.Sleep(500);
            }
            catch
            {
            }
            Thread.Sleep(250);
        }
    }

    private void Pull()
    {
        if (Moonfire.KnownSpell && Moonfire.IsDistanceGood && Moonfire.IsSpellUsable
            && _mySettings.UseMoonfire)
        {
            Moonfire.Launch();
            Moonfire_Timer = new Timer(1000*11);
            return;
        }
        else
        {
            if (Wrath.KnownSpell && Wrath.IsDistanceGood && Wrath.IsSpellUsable
                && _mySettings.UseWrath)
            {
                Wrath.Launch();
                return;
            }
        }
    }

    private void Combat()
    {
        Buff();
        AvoidMelee();
        if (OnCD.IsReady)
            Defense_Cycle();
        Heal();
        Decast();
        Healing_Burst();
        DPS_Cycle();
    }

    private void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (AlchFlask_Timer.IsReady && _mySettings.UseAlchFlask && Alchemy.KnownSpell
            && ItemsManager.GetItemCountByIdLUA(75525) == 1)
        {
            Logging.WriteFight("Use Alchi Flask");
            Lua.RunMacroText("/use item:75525");
            AlchFlask_Timer = new Timer(1000*60*60*2);
            return;
        }
        else if (Mark_of_the_Wild.KnownSpell && Mark_of_the_Wild.IsSpellUsable && !Mark_of_the_Wild.TargetHaveBuff
                 && _mySettings.UseMarkoftheWild)
        {
            Mark_of_the_Wild.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() == 0 && _mySettings.UseDash
                 && Dash.KnownSpell && Dash.IsSpellUsable && !Dash.TargetHaveBuff && !Stampeding_Roar.TargetHaveBuff
                 && ObjectManager.Me.GetMove)
        {
            Dash.Launch();
            return;
        }
        else
        {
            if (ObjectManager.GetNumberAttackPlayer() == 0 && _mySettings.UseStampedingRoar
                && Stampeding_Roar.KnownSpell && Stampeding_Roar.IsSpellUsable && !Dash.TargetHaveBuff
                && !Stampeding_Roar.TargetHaveBuff && ObjectManager.Me.GetMove)
            {
                Stampeding_Roar.Launch();
                return;
            }
        }
    }

    private void AvoidMelee()
    {
        if (ObjectManager.Target.GetDistance < 2 && ObjectManager.Target.InCombat)
        {
            Logging.WriteFight("Too Close. Moving Back");
            var MaxTime_Timer = new Timer(1000*2);
            Keyboard.DownKey(Memory.WowProcess.MainWindowHandle, MoveBackward);
            while (ObjectManager.Target.GetDistance < 2 && ObjectManager.Target.InCombat && !MaxTime_Timer.IsReady)
                Thread.Sleep(300);
            Keyboard.UpKey(Memory.WowProcess.MainWindowHandle, MoveBackward);
            if (MaxTime_Timer.IsReady && ObjectManager.Target.GetDistance < 2 && ObjectManager.Target.InCombat)
            {
                Keyboard.DownKey(Memory.WowProcess.MainWindowHandle, MoveForward);
                Thread.Sleep(1000);
                Keyboard.UpKey(Memory.WowProcess.MainWindowHandle, MoveForward);
                MovementManager.Face(ObjectManager.Target.Position);
            }
        }
    }

    private void Defense_Cycle()
    {
        if (ObjectManager.Me.HealthPercent <= _mySettings.UseStoneformAtPercentage && Stoneform.IsSpellUsable &&
            Stoneform.KnownSpell
            && _mySettings.UseStoneform)
        {
            Stoneform.Launch();
            OnCD = new Timer(1000*8);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && Barkskin.KnownSpell && Barkskin.IsSpellUsable
                 && _mySettings.UseBarkskin)
        {
            Barkskin.Launch();
            OnCD = new Timer(1000*12);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && Ironbark.KnownSpell && Ironbark.IsSpellUsable
                 && _mySettings.UseIronbark)
        {
            Ironbark.Launch();
            OnCD = new Timer(1000*12);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && Mighty_Bash.KnownSpell && Mighty_Bash.IsSpellUsable
                 && _mySettings.UseMightyBash && Mighty_Bash.IsDistanceGood)
        {
            Mighty_Bash.Launch();
            OnCD = new Timer(1000*5);
            return;
        }
        else if (Mass_Entanglement.KnownSpell && Mass_Entanglement.IsSpellUsable && Mass_Entanglement.IsDistanceGood
                 && _mySettings.UseMassEntanglement && ObjectManager.Me.HealthPercent < 80)
        {
            Mass_Entanglement.Launch();

            if (Wild_Charge.KnownSpell && Wild_Charge.IsDistanceGood && Wild_Charge.IsSpellUsable
                && _mySettings.UseWildCharge)
            {
                Thread.Sleep(200);
                Wild_Charge.Launch();
            }
            return;
        }
        else if (Ursols_Vortex.KnownSpell && Ursols_Vortex.IsSpellUsable && Ursols_Vortex.IsDistanceGood
                 && _mySettings.UseUrsolsVortex && ObjectManager.Me.HealthPercent < 80)
        {
            Ursols_Vortex.Launch();

            if (Wild_Charge.KnownSpell && Wild_Charge.IsDistanceGood && Wild_Charge.IsSpellUsable
                && _mySettings.UseWildCharge)
            {
                Thread.Sleep(200);
                Wild_Charge.Launch();
            }
            return;
        }
        else if (Natures_Grasp.KnownSpell && Natures_Grasp.IsSpellUsable
                 && ObjectManager.Target.IsCast && _mySettings.UseNaturesGrasp && ObjectManager.Me.HealthPercent < 80)
        {
            Natures_Grasp.Launch();

            if (Wild_Charge.KnownSpell && Wild_Charge.IsDistanceGood && Wild_Charge.IsSpellUsable
                && _mySettings.UseWildCharge)
            {
                Thread.Sleep(200);
                Wild_Charge.Launch();
            }
            return;
        }
        else if (Typhoon.KnownSpell && Typhoon.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() > 2
                 && ObjectManager.Target.GetDistance < 40 && ObjectManager.Me.HealthPercent < 70
                 && _mySettings.UseTyphoon)
        {
            Typhoon.Launch();
            return;
        }
        else if (Disorienting_Roar.KnownSpell && Disorienting_Roar.IsSpellUsable &&
                 ObjectManager.GetNumberAttackPlayer() > 2
                 && ObjectManager.Target.GetDistance < 10 && ObjectManager.Me.HealthPercent < 70
                 && _mySettings.UseDisorientingRoar)
        {
            Disorienting_Roar.Launch();
            OnCD = new Timer(1000*3);
            return;
        }
        else
        {
            if (ObjectManager.Me.HealthPercent <= _mySettings.UseWarStompAtPercentage && War_Stomp.IsSpellUsable &&
                War_Stomp.KnownSpell
                && _mySettings.UseWarStomp)
            {
                War_Stomp.Launch();
                OnCD = new Timer(1000*2);
                return;
            }
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (Arcane_Torrent.IsSpellUsable && Arcane_Torrent.KnownSpell &&
            ObjectManager.Me.HealthPercent <= _mySettings.UseArcaneTorrentForResourceAtPercentage
            && _mySettings.UseArcaneTorrentForResource)
        {
            Arcane_Torrent.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && Natures_Swiftness.IsSpellUsable && Natures_Swiftness.KnownSpell
                 && _mySettings.UseNaturesSwiftness && _mySettings.UseHealingTouch)
        {
            Natures_Swiftness.Launch();
            Thread.Sleep(400);
            Healing_Touch.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 70 && Renewal.IsSpellUsable && Renewal.KnownSpell
                 && _mySettings.UseRenewal)
        {
            Renewal.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 95 && !Fight.InFight && ObjectManager.GetNumberAttackPlayer() == 0
                 && Healing_Touch.IsSpellUsable && Healing_Touch.KnownSpell && _mySettings.UseHealingTouch)
        {
            Healing_Touch.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 90 && Cenarion_Ward.IsSpellUsable && Cenarion_Ward.KnownSpell
                 && _mySettings.UseCenarionWard)
        {
            Cenarion_Ward.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent <= _mySettings.UseGiftoftheNaaruAtPercentage &&
                 Gift_of_the_Naaru.IsSpellUsable && Gift_of_the_Naaru.KnownSpell
                 && _mySettings.UseGiftoftheNaaru)
        {
            Gift_of_the_Naaru.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 90 && Rejuvenation.IsSpellUsable && Rejuvenation.KnownSpell
                 && !Rejuvenation.TargetHaveBuff && _mySettings.UseRejuvenation)
        {
            Rejuvenation.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 60 && Regrowth.IsSpellUsable && Regrowth.KnownSpell
                 && !Regrowth.TargetHaveBuff && _mySettings.UseRegrowth)
        {
            Regrowth.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && Swiftmend.IsSpellUsable && Swiftmend.KnownSpell
                 && _mySettings.UseSwiftmend)
        {
            Swiftmend.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 50 && Wild_Growth.IsSpellUsable && Wild_Growth.KnownSpell
                 && _mySettings.UseWildGrowth)
        {
            Wild_Growth.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 40 && Healing_Touch.IsSpellUsable && Healing_Touch.KnownSpell
                 && Healing_Touch_Timer.IsReady && _mySettings.UseHealingTouch)
        {
            Healing_Touch.Launch();
            Healing_Touch_Timer = new Timer(1000*15);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 40 && Nourish.IsSpellUsable && Nourish.KnownSpell
                 && Nourish_Timer.IsReady && _mySettings.UseNourish && !_mySettings.UseHealingTouch)
        {
            Nourish.Launch();
            Nourish_Timer = new Timer(1000*15);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 35 && Might_of_Ursoc.IsSpellUsable && Might_of_Ursoc.KnownSpell
                 && _mySettings.UseMightofUrsoc)
        {
            Might_of_Ursoc.Launch();
            return;
        }
        else if (Wild_Mushroom.KnownSpell && Wild_Mushroom.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() > 3
                 && Wild_Mushroom_Bloom.KnownSpell && Wild_Mushroom.IsSpellUsable && _mySettings.UseWildMushroom
                 && ObjectManager.Me.HealthPercent < 80)
        {
            for (int i = 0; i < 3; i++)
            {
                SpellManager.CastSpellByIDAndPosition(88747, ObjectManager.Target.Position);
                Thread.Sleep(200);
            }

            Wild_Mushroom_Bloom.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Me.HealthPercent < 30 && Tranquility.IsSpellUsable && Tranquility.KnownSpell
                && _mySettings.UseTranquility)
            {
                Tranquility.Launch();
                while (ObjectManager.Me.IsCast)
                {
                    Thread.Sleep(100);
                    Thread.Sleep(100);
                }
                return;
            }
        }

        if (ObjectManager.Me.ManaPercentage < 50 && _mySettings.UseInnervate)
        {
            Innervate.Launch();
            return;
        }
    }

    private void Decast()
    {
        if (Arcane_Torrent.IsSpellUsable && Arcane_Torrent.KnownSpell && ObjectManager.Target.GetDistance < 8
            && ObjectManager.Me.HealthPercent <= _mySettings.UseArcaneTorrentForDecastAtPercentage
            && _mySettings.UseArcaneTorrentForDecast && ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe)
        {
            Arcane_Torrent.Launch();
            return;
        }
    }

    public void Healing_Burst()
    {
        if (_mySettings.UseTrinket && Trinket_Timer.IsReady && ObjectManager.Target.GetDistance < 30)
        {
            Logging.WriteFight("Use Trinket 1.");
            Lua.RunMacroText("/use 13");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Logging.WriteFight("Use Trinket 2.");
            Lua.RunMacroText("/use 14");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Trinket_Timer = new Timer(1000*60*2);
        }
        else if (Berserking.IsSpellUsable && Berserking.KnownSpell && ObjectManager.Target.GetDistance < 30
                 && _mySettings.UseBerserking)
            Berserking.Launch();
        else if (Blood_Fury.IsSpellUsable && Blood_Fury.KnownSpell && ObjectManager.Target.GetDistance < 30
                 && _mySettings.UseBloodFury)
            Blood_Fury.Launch();
        else if (Lifeblood.IsSpellUsable && Lifeblood.KnownSpell && ObjectManager.Target.GetDistance < 30
                 && _mySettings.UseLifeblood)
            Lifeblood.Launch();
        else if (Engineering_Timer.IsReady && Engineering.KnownSpell && ObjectManager.Target.GetDistance < 30
                 && _mySettings.UseEngGlove)
        {
            Logging.WriteFight("Use Engineering Gloves.");
            Lua.RunMacroText("/use 10");
            Engineering_Timer = new Timer(1000*60);
        }
        else if (Force_of_Nature.IsSpellUsable && Force_of_Nature.KnownSpell && Force_of_Nature.IsDistanceGood
                 && _mySettings.UseForceofNature)
        {
            SpellManager.CastSpellByIDAndPosition(106737, ObjectManager.Target.Position);
            return;
        }
        else
        {
            if (Incarnation.IsSpellUsable && Incarnation.KnownSpell && _mySettings.UseIncarnation
                && ObjectManager.Target.GetDistance < 30)
            {
                Incarnation.Launch();
                return;
            }
        }
    }

    private void DPS_Cycle()
    {
        if (Moonfire.KnownSpell && Moonfire.IsDistanceGood && Moonfire.IsSpellUsable
            && _mySettings.UseMoonfire && (!Moonfire.TargetHaveBuff || Moonfire_Timer.IsReady))
        {
            Moonfire.Launch();
            Moonfire_Timer = new Timer(1000*11);
            return;
        }
        else if (Hurricane.KnownSpell && Hurricane.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() > 2 &&
                 ObjectManager.Target.GetDistance < 30 && _mySettings.UseHurricane)
        {
            SpellManager.CastSpellByIDAndPosition(16914, ObjectManager.Target.Position);
            return;
        }
        else
        {
            if (Wrath.KnownSpell && Wrath.IsSpellUsable && Wrath.IsDistanceGood
                && _mySettings.UseWrath)
            {
                Wrath.Launch();
                return;
            }
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

    #region Nested type: DruidRestorationSettings

    [Serializable]
    public class DruidRestorationSettings : Settings
    {
        public bool UseAlchFlask = true;
        public bool UseArcaneTorrentForDecast = true;
        public int UseArcaneTorrentForDecastAtPercentage = 100;
        public bool UseArcaneTorrentForResource = true;
        public int UseArcaneTorrentForResourceAtPercentage = 80;
        public bool UseBarkskin = true;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseCenarionWard = true;
        public bool UseDash = true;
        public bool UseDisorientingRoar = true;
        public bool UseEngGlove = true;
        public bool UseEntanglingRoots = true;
        public bool UseFaerieFire = true;
        public bool UseForceofNature = true;
        public bool UseGiftoftheNaaru = true;
        public int UseGiftoftheNaaruAtPercentage = 80;
        public bool UseHealingTouch = true;
        public bool UseHurricane = true;
        public bool UseIncarnation = true;
        public bool UseInnervate = true;
        public bool UseIronbark = true;
        public bool UseLifeblood = true;
        public bool UseLifebloom = true;
        public bool UseLowCombat = true;
        public bool UseMarkoftheWild = true;
        public bool UseMassEntanglement = true;
        public bool UseMightofUrsoc = true;
        public bool UseMightyBash = true;
        public bool UseMoonfire = true;
        public bool UseNaturesGrasp = true;
        public bool UseNaturesSwiftness = true;
        public bool UseNourish = false;
        public bool UseRegrowth = true;
        public bool UseRejuvenation = true;
        public bool UseRenewal = true;
        public bool UseSolarBeam = true;
        public bool UseStampedingRoar = true;
        public bool UseStoneform = true;
        public int UseStoneformAtPercentage = 80;
        public bool UseSwiftmend = true;
        public bool UseTranquility = true;
        public bool UseTrinket = true;
        public bool UseTyphoon = true;
        public bool UseUrsolsVortex = true;
        public bool UseWarStomp = true;
        public int UseWarStompAtPercentage = 80;
        public bool UseWildCharge = true;
        public bool UseWildGrowth = true;
        public bool UseWildMushroom = false;
        public bool UseWrath = true;

        public DruidRestorationSettings()
        {
            ConfigWinForm(new Point(500, 400), "Druid Restoration Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent for Interrupt", "UseArcaneTorrentForDecast", "Professions & Racials",
                                "AtPercentage");
            AddControlInWinForm("Use Arcane Torrent for Resource", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");
            AddControlInWinForm("Use Lifeblood", "UseLifeblood", "Professions & Racials");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            /* Druid Buffs */
            AddControlInWinForm("Use Dash", "UseDash", "Druid Buffs");
            AddControlInWinForm("Use Faerie Fire", "UseFaerieFire", "Druid Buffs");
            AddControlInWinForm("Use Mark of the Wild", "UseMarkoftheWild", "Druid Buffs");
            AddControlInWinForm("Use Stampeding Roar", "UseStampedingRoar", "Druid Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Hurricane", "UseHurricane", "Offensive Spell");
            AddControlInWinForm("Use Moonfire", "UseMoonfire", "Offensive Spell");
            AddControlInWinForm("Use Wrath", "UseWrath", "Offensive Spell");
            /* Healing Cooldown */
            AddControlInWinForm("Use Force of Nature", "UseForceofNature", "Offensive Cooldown");
            AddControlInWinForm("Use Incarnation", "UseIncarnation", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Barkskin", "UseBarkskin", "Defensive Cooldown");
            AddControlInWinForm("Use Disorienting Roar", "UseDisorientingRoar", "Defensive Cooldown");
            AddControlInWinForm("Use Entangling Roots", "UseEntanglingRoots", "Defensive Cooldown");
            AddControlInWinForm("Use Ironbark", "UseIronbark", "Defensive Cooldown");
            AddControlInWinForm("Use Mass Entanglement", "UseMassEntanglement", "Defensive Cooldown");
            AddControlInWinForm("Use Mighty Bash", "UseMightyBash", "Defensive Cooldown");
            AddControlInWinForm("Use Nature's Grasp", "UseNaturesGrasp", "Defensive Cooldown");
            AddControlInWinForm("Use Solar Beam", "UseSolarBeam", "Defensive Cooldown");
            AddControlInWinForm("Use Typhoon", "UseTyphoon", "Defensive Cooldown");
            AddControlInWinForm("Use Ursol's Vortex", "UseUrsolsVortex", "Defensive Cooldown");
            AddControlInWinForm("Use Wild Charge", "UseWildCharge", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Cenarion Ward", "UseCenarionWard", "Healing Spell");
            AddControlInWinForm("Use Healing Touch", "UseHealingTouch", "Healing Spell");
            AddControlInWinForm("Use Innervate", "UseInnervate", "Healing Spell");
            AddControlInWinForm("Use Lifebloom", "UseLifebloom", "Offensive Spell");
            AddControlInWinForm("Use Might of Ursoc", "UseMightofUrsoc", "Healing Spell");
            AddControlInWinForm("Use Nature's Swiftness", "UseNaturesSwiftness", "Healing Spell");
            AddControlInWinForm("Use Nourish", "UseNourish", "Offensive Spell");
            AddControlInWinForm("Use Regrowth", "UseRegrowth", "Offensive Spell");
            AddControlInWinForm("Use Rejuvenation", "UseRejuvenation", "Healing Spell");
            AddControlInWinForm("Use Renewal", "UseRenewal", "Healing Spell");
            AddControlInWinForm("Use Swiftmend", "UseSwiftmend", "Offensive Spell");
            AddControlInWinForm("Use Tranquility", "UseTranquility", "Healing Spell");
            AddControlInWinForm("Use Wild Growth", "UseWildGrowth", "Offensive Spell");
            AddControlInWinForm("Use WildMushroom", "UseWildMushroom", "Offensive Spell");
            /* Game Settings */
            AddControlInWinForm("Use Low Combat Settings", "UseLowCombat", "Game Settings");
            AddControlInWinForm("Use Trinket", "UseTrinket", "Game Settings");
            AddControlInWinForm("Use Engineering Gloves", "UseEngGlove", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
        }

        public static DruidRestorationSettings CurrentSetting { get; set; }

        public static DruidRestorationSettings GetSettings()
        {
            string CurrentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Druid_Restoration.xml";
            if (File.Exists(CurrentSettingsFile))
            {
                return
                    CurrentSetting = Load<DruidRestorationSettings>(CurrentSettingsFile);
            }
            else
            {
                return new DruidRestorationSettings();
            }
        }
    }

    #endregion
}

#endregion

#region Paladin

public class Paladin_Holy
{
    private readonly string MoveBackward =
        Keybindings.GetKeyByAction(nManager.Wow.Enums.Keybindings.MOVEBACKWARD);

    private readonly string MoveForward =
        Keybindings.GetKeyByAction(nManager.Wow.Enums.Keybindings.MOVEFORWARD);

    private readonly PaladinHolySettings _mySettings = PaladinHolySettings.GetSettings();

    #region Professions & Racial

    private readonly Spell ArcaneTorrent = new Spell("Arcane Torrent");
    private readonly Spell Berserking = new Spell("Berserking");
    private readonly Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    private readonly Spell Lifeblood = new Spell("Lifeblood");
    private readonly Spell Stoneform = new Spell("Stoneform");
    private readonly Spell War_Stomp = new Spell("War Stomp");

    #endregion

    #region Paladin Seals & Buffs

    private readonly Spell BlessingOfKings = new Spell("Blessing of Kings");
    private readonly Spell BlessingOfMight = new Spell("Blessing of Might");
    private readonly Spell SealOfInsight = new Spell("Seal of Insight");
    private readonly Spell SealOfTheRighteousness = new Spell("Seal of Righteousness");
    private readonly Spell SealOfTruth = new Spell("Seal of Truth");

    #endregion

    #region Offensive Spell

    private readonly Spell Denounce = new Spell("Denounce");
    private readonly Spell HammerOfJustice = new Spell("Hammer of Justice");
    private readonly Spell HammerOfWrath = new Spell("Hammer of Wrath");
    private readonly Spell HolyShock = new Spell("Holy Shock");

    #endregion

    #region Offensive Cooldown

    private readonly Spell AvengingWrath = new Spell("Avenging Wrath");
    private readonly Spell DivineFavor = new Spell("Divine Favor");
    private readonly Spell HolyAvenger = new Spell("HolyAvenger");

    #endregion

    #region Defensive Cooldown

    private readonly Spell DevotionAura = new Spell("Devotion Aura");
    private readonly Spell DivineProtection = new Spell("Divine Protection");
    private readonly Spell DivineShield = new Spell("Divine Shield");
    private readonly Spell HandOfProtection = new Spell("Hand of Protection");
    private readonly Spell HandOfPurity = new Spell("Hand of Purity");
    private readonly Spell SacredShield = new Spell("Sacred Shield");

    #endregion

    #region Healing Spell

    private readonly Spell BeaconOfLight = new Spell("Beacon of Light");
    private readonly Spell DivineLight = new Spell("Divine Light");
    private readonly Spell DivinePlea = new Spell("Divine Plea");
    private readonly Spell FlashOfLight = new Spell("Flash of Light");
    private readonly Spell GlyphOfHarshWords = new Spell("Glyph of Harsh Words");
    private readonly Spell HolyLight = new Spell("Holy Light");
    private readonly Spell HolyRadiance = new Spell("Holy Radiance");
    private readonly Spell LayOnHands = new Spell("Lay on Hands");
    private readonly Spell WordOfGlory = new Spell("Word of Glory");

    #endregion

    public Paladin_Holy()
    {
        Main.range = 30f;

        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            try
            {
                if (!ObjectManager.Me.IsDead && !Usefuls.IsLoadingOrConnecting && Usefuls.InGame)
                {
                    if (!ObjectManager.Me.IsMounted)
                    {
                        if (Heal.IsHealing)
                        {
                            if (ObjectManager.Me.HealthPercent < 100 && !Party.IsInGroup())
                            {
                                if (ObjectManager.Me.Target != ObjectManager.Me.Guid)
                                    Interact.InteractGameObject(ObjectManager.Me.GetBaseAddress);
                            }
                            else if (Party.IsInGroup())
                            {
                                double lowestHp = 100;
                                var lowestHpPlayer = new WoWUnit(0);
                                foreach (var playerInMyParty in Party.GetPartyPlayersGUID())
                                {
                                    if (playerInMyParty <= 0) continue;
                                    var obj = ObjectManager.GetObjectByGuid(playerInMyParty);
                                    if (!obj.IsValid || obj.Type != WoWObjectType.Player)
                                        continue;
                                    var currentPlayer = new WoWPlayer(obj.GetBaseAddress);
                                    if (!currentPlayer.IsValid || currentPlayer.IsDead) continue;

                                    if (currentPlayer.HealthPercent < 100 && currentPlayer.HealthPercent < lowestHp)
                                    {
                                        lowestHp = currentPlayer.HealthPercent;
                                        lowestHpPlayer = currentPlayer;
                                    }
                                }
                                if (ObjectManager.Me.HealthPercent < 50 &&
                                    ObjectManager.Me.HealthPercent - 10 < lowestHp)
                                {
                                    lowestHpPlayer = ObjectManager.Me;
                                }
                                if (ObjectManager.Me.Target != lowestHpPlayer.Guid)
                                    Interact.InteractGameObject(lowestHpPlayer.GetBaseAddress);
                            }
                            else
                            {
                                Heal.IsHealing = false;
                                break;
                            }
                            HealingFight();
                        }
                        else if (!ObjectManager.Me.IsCast)
                            Patrolling();
                    }
                }
                Thread.Sleep(500);
            }
            catch
            {
            }
            Thread.Sleep(150);
        }
    }

    private void HealingFight()
    {
        //AvoidMelee();

        if (ObjectManager.Target.HealthPercent < 40) 
            Heal_Burst();

        Heal_Cycle();

        Buffs();
    }

    private void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            Blessing();
        }
        Seal();
    }

    private void Buffs()
    {
        if (!ObjectManager.Me.IsMounted)
            Blessing();
        Seal();
    }

    private void Seal()
    {
        if (SealOfInsight.KnownSpell && _mySettings.UseSealOfInsight)
        {
            if (!SealOfInsight.TargetHaveBuff && SealOfInsight.IsSpellUsable)
                SealOfInsight.Launch();
        }
        else if (SealOfTruth.KnownSpell && _mySettings.UseSealOfTruth)
        {
            if (!SealOfTruth.TargetHaveBuff && SealOfTruth.IsSpellUsable)
                SealOfTruth.Launch();
        }
        else if (SealOfTheRighteousness.KnownSpell && _mySettings.UseSealOfTheRighteousness)
        {
            if (!SealOfTheRighteousness.TargetHaveBuff && SealOfTheRighteousness.IsSpellUsable)
                SealOfTheRighteousness.Launch();
        }
    }

    private void Blessing()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (BlessingOfKings.KnownSpell && _mySettings.UseBlessingOfKings)
        {
            if (!BlessingOfKings.TargetHaveBuff && BlessingOfKings.IsSpellUsable)
                BlessingOfKings.Launch();
        }
        else if (BlessingOfMight.KnownSpell && _mySettings.UseBlessingOfMight)
        {
            if (!BlessingOfMight.TargetHaveBuff && BlessingOfMight.IsSpellUsable)
                BlessingOfMight.Launch();
        }
        if (BeaconOfLight.KnownSpell && _mySettings.UseBeaconOfLight)
        {
            if (!BeaconOfLight.TargetHaveBuff && BeaconOfLight.IsSpellUsable)
                BeaconOfLight.Launch();
        }
    }

    private void Heal_Burst()
    {
        if (DivineFavor.KnownSpell && DivineFavor.IsSpellUsable)
        {
            if (AvengingWrath.KnownSpell && AvengingWrath.IsSpellUsable && _mySettings.UseAvengingWrath)
            {
                AvengingWrath.Launch();
            }
            if (Lifeblood.KnownSpell && Lifeblood.IsSpellUsable && _mySettings.UseLifeblood)
            {
                Lifeblood.Launch();
            }
            if (HolyAvenger.KnownSpell && HolyAvenger.IsSpellUsable && _mySettings.UseHolyAvenger)
            {
                HolyAvenger.Launch();
            }
            if (_mySettings.UseDivineFavor)
                DivineFavor.Launch();
            return;
        }
        else if (Lifeblood.KnownSpell && Lifeblood.IsSpellUsable && _mySettings.UseLifeblood)
        {
            Lifeblood.Launch();
            return;
        }
    }

    private void Heal_Cycle()
    {
        if (!ObjectManager.Target.HaveBuff(25771))
        {
            if (_mySettings.UseDivineShield && ObjectManager.Target == ObjectManager.Me && DivineShield.KnownSpell && ObjectManager.Target.HealthPercent > 0 && ObjectManager.Target.HealthPercent <= 20 &&
                DivineShield.IsSpellUsable)
            {
                DivineShield.Launch();
                return;
            }
            if (LayOnHands.KnownSpell && ObjectManager.Target.HealthPercent > 0 && ObjectManager.Target.HealthPercent <= 20 &&
                LayOnHands.IsSpellUsable && _mySettings.UseLayOnHands)
            {
                LayOnHands.Launch();
                return;
            }
            if (HandOfProtection.KnownSpell && ObjectManager.Target.HealthPercent > 0 &&
                ObjectManager.Target.HealthPercent <= 20 &&
                HandOfProtection.IsSpellUsable && _mySettings.UseHandOfProtection)
            {
                HandOfProtection.Launch();
                return;
            }
        }
        if (ObjectManager.Target.ManaPercentage < 30)
        {
            if (ArcaneTorrent.KnownSpell && ArcaneTorrent.IsSpellUsable && _mySettings.UseArcaneTorrentForResource)
                ArcaneTorrent.Launch();
            if (DivinePlea.KnownSpell && DivinePlea.IsSpellUsable && _mySettings.UseHandOfProtection)
            {
                DivinePlea.Launch();
                return;
            }
        }
        if (ObjectManager.Target.HealthPercent > 0 && ObjectManager.Target.HealthPercent < 50)
        {
            if (WordOfGlory.KnownSpell && WordOfGlory.IsSpellUsable && _mySettings.UseWordOfGlory)
                WordOfGlory.Launch();
            if (DivineLight.KnownSpell && DivineLight.IsSpellUsable && _mySettings.UseDivineLight)
            {
                DivineLight.Launch();
                return;
            }
            if (FlashOfLight.KnownSpell && FlashOfLight.IsSpellUsable && _mySettings.UseFlashOfLight)
            {
                FlashOfLight.Launch();
                return;
            }
            if (HolyLight.KnownSpell && HolyLight.IsSpellUsable && _mySettings.UseHolyLight)
            {
                HolyLight.Launch();
                return;
            }
        }
        if (ObjectManager.Target.HealthPercent >= 0 && ObjectManager.Target.HealthPercent < 30)
        {
            if (WordOfGlory.KnownSpell && WordOfGlory.IsSpellUsable &&
                (!GlyphOfHarshWords.KnownSpell /* || cast on me */) && _mySettings.UseWordOfGlory)
                WordOfGlory.Launch();
            if (DivineProtection.KnownSpell && DivineProtection.IsSpellUsable && _mySettings.UseDivineProtection)
                DivineProtection.Launch();
            if (FlashOfLight.KnownSpell && FlashOfLight.IsSpellUsable && _mySettings.UseFlashOfLight)
            {
                FlashOfLight.Launch();
                return;
            }
            if (HolyLight.KnownSpell && HolyLight.IsSpellUsable && _mySettings.UseHolyLight)
            {
                HolyLight.Launch();
                return;
            }
            if (DivineLight.KnownSpell && DivineLight.IsSpellUsable && _mySettings.UseDivineLight)
            {
                DivineLight.Launch();
                return;
            }
        }
    }

    private void AvoidMelee()
    {
        /*if (ObjectManager.Target.GetDistance < 2 && ObjectManager.Target.InCombat)
        {
            Logging.WriteFight("Too Close. Moving Back");
            var MaxTime_Timer = new Timer(1000*2);
            Keyboard.DownKey(Memory.WowProcess.MainWindowHandle, MoveBackward);
            while (ObjectManager.Target.GetDistance < 2 && ObjectManager.Target.InCombat && !MaxTime_Timer.IsReady)
                Thread.Sleep(300);
            Keyboard.UpKey(Memory.WowProcess.MainWindowHandle, MoveBackward);
            if (MaxTime_Timer.IsReady && ObjectManager.Target.GetDistance < 2 && ObjectManager.Target.InCombat)
            {
                Keyboard.DownKey(Memory.WowProcess.MainWindowHandle, MoveForward);
                Thread.Sleep(1000);
                Keyboard.UpKey(Memory.WowProcess.MainWindowHandle, MoveForward);
                MovementManager.Face(ObjectManager.Target.Position);
            }
        }*/
    }

    #region Nested type: PaladinHolySettings

    [Serializable]
    public class PaladinHolySettings : Settings
    {
        public bool UseArcaneTorrentForDecast = true;
        public int UseArcaneTorrentForDecastAtPercentage = 100;
        public bool UseArcaneTorrentForResource = true;
        public int UseArcaneTorrentForResourceAtPercentage = 80;
        public bool UseAvengingWrath = true;
        public bool UseBeaconOfLight = true;
        public bool UseBerserking = true;
        public bool UseBlessingOfKings = true;
        public bool UseBlessingOfMight = true;
        public bool UseDenounce = true;
        public bool UseDevotionAura = true;
        public bool UseDivineFavor = true;
        public bool UseDivineLight = true;
        public bool UseDivinePlea = true;
        public bool UseDivineProtection = true;
        public bool UseDivineShield = true;
        public bool UseFlashOfLight = true;
        public bool UseGiftoftheNaaru = true;
        public int UseGiftoftheNaaruAtPercentage = 80;
        public bool UseHammerOfJustice = true;
        public bool UseHammerOfWrath = true;
        public bool UseHandOfProtection = true;
        public bool UseHandOfPurity = true;
        public bool UseHolyAvenger = true;
        public bool UseHolyLight = true;
        public bool UseHolyRadiance = true;
        public bool UseHolyShock = true;
        public bool UseLayOnHands = true;
        public bool UseLifeblood = true;
        public bool UseSacredShield = true;
        public bool UseSealOfInsight = true;
        public bool UseSealOfTheRighteousness = true;
        public bool UseSealOfTruth = true;
        public bool UseStoneform = true;
        public int UseStoneformAtPercentage = 80;
        public bool UseWarStomp = true;
        public int UseWarStompAtPercentage = 80;
        public bool UseWordOfGlory = true;

        public PaladinHolySettings()
        {
            ConfigWinForm(new Point(500, 400), "Paladin Protection Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent for Interrupt", "UseArcaneTorrentForDecast", "Professions & Racials",
                                "AtPercentage");
            AddControlInWinForm("Use Arcane Torrent for Resource", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Lifeblood", "UseLifeblood", "Professions & Racials");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            /* Paladin Seals & Buffs */
            AddControlInWinForm("Use Seal of the Righteousness", "UseSealOfTheRighteousness", "Paladin Seals & Buffs");
            AddControlInWinForm("Use Seal of Truth", "UseSealOfTruth", "Paladin Seals & Buffs");
            AddControlInWinForm("Use Seal of Insight", "UseSealOfInsight", "Paladin Seals & Buffs");
            AddControlInWinForm("Use Blessing of Might", "UseBlessingOfMight", "Paladin Seals & Buffs");
            AddControlInWinForm("Use Blessing of Kings", "UseBlessingOfKings", "Paladin Seals & Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Holy Shock", "UseHolyShock", "Offensive Spell");
            AddControlInWinForm("Use Denounce", "UseDenounce", "Offensive Spell");
            AddControlInWinForm("Use Hammer of Justice", "UseHammerOfJustice", "Offensive Spell");
            AddControlInWinForm("Use Hammer of Wrath", "UseHammerOfWrath", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use Divine Favor", "UseDivineFavor", "Offensive Cooldown");
            AddControlInWinForm("Use Holy Avenger", "UseHolyAvenger", "Offensive Cooldown");
            AddControlInWinForm("Use Avenging Wrath", "UseAvengingWrath", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Sacred Shield", "UseSacredShield", "Defensive Cooldown");
            AddControlInWinForm("Use Hand of Purity", "UseHandOfPurity", "Defensive Cooldown");
            AddControlInWinForm("Use Devotion Aura", "UseDevotionAura", "Defensive Cooldown");
            AddControlInWinForm("Use Divine Protection", "UseDivineProtection", "Defensive Cooldown");
            AddControlInWinForm("Use Divine Shield", "UseDivineShield", "Defensive Cooldown");
            AddControlInWinForm("Use Hand of Protection", "UseHandOfProtection", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Divine Plea", "UseDivinePlea", "Healing Spell");
            AddControlInWinForm("Use Divine Light", "UseDivineLight", "Healing Spell");
            AddControlInWinForm("Use Holy Radiance", "UseHolyRadiance", "Healing Spell");
            AddControlInWinForm("Use Flash of Light", "UseFlashOfLight", "Healing Spell");
            AddControlInWinForm("Use Holy Light", "UseHolyLight", "Healing Spell");
            AddControlInWinForm("Use Lay on Hands", "UseLayOnHands", "Healing Spell");
            AddControlInWinForm("Use Word of Glory", "UseWordOfGlory", "Healing Spell");
            AddControlInWinForm("Use Beacon of Light", "UseBeaconOfLight", "Healing Spell");
        }

        public static PaladinHolySettings CurrentSetting { get; set; }

        public static PaladinHolySettings GetSettings()
        {
            string CurrentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Paladin_Holy.xml";
            if (File.Exists(CurrentSettingsFile))
            {
                return CurrentSetting = Load<PaladinHolySettings>(CurrentSettingsFile);
            }
            return new PaladinHolySettings();
        }
    }

    #endregion
}

#endregion

#region Shaman

public class Shaman_Restoration
{
    private readonly string MoveBackward =
        Keybindings.GetKeyByAction(nManager.Wow.Enums.Keybindings.MOVEBACKWARD);

    private readonly string MoveForward =
        Keybindings.GetKeyByAction(nManager.Wow.Enums.Keybindings.MOVEFORWARD);

    private readonly ShamanRestorationSettings _mySettings = ShamanRestorationSettings.GetSettings();

    #region General Timers & Variables

    private Timer AlchFlask_Timer = new Timer(0);
    private Timer Engineering_Timer = new Timer(0);
    public int LC = 0;
    private Timer OnCD = new Timer(0);
    private Timer Trinket_Timer = new Timer(0);

    #endregion

    #region Professions & Racials

    private readonly Spell Alchemy = new Spell("Alchemy");
    private readonly Spell Arcane_Torrent = new Spell("Arcane Torrent");
    private readonly Spell Berserking = new Spell("Berserking");
    private readonly Spell Blood_Fury = new Spell("Blood Fury");
    private readonly Spell Engineering = new Spell("Engineering");
    private readonly Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    private readonly Spell Lifeblood = new Spell("Lifeblood");
    private readonly Spell Stoneform = new Spell("Stoneform");
    private readonly Spell War_Stomp = new Spell("War Stomp");

    #endregion

    #region Shaman Buffs

    private readonly Spell Earth_Shield = new Spell("Earth Shield");
    private readonly Spell Earthliving_Weapon = new Spell("Earthliving Weapon");
    private readonly Spell Flametongue_Weapon = new Spell("Flametongue Weapon");
    private readonly Spell Frostbrand_Weapon = new Spell("Frostbrand Weapon");
    private readonly Spell Ghost_Wolf = new Spell("Ghost Wolf");
    private readonly Spell Lightning_Shield = new Spell("Lightning Shield");
    private readonly Spell Rockbiter_Weapon = new Spell("Rockbiter Weapon");
    private readonly Spell Spiritwalkers_Grace = new Spell("Spiritwalker's Grace");
    private readonly Spell Water_Shield = new Spell("Water Shield");
    private readonly Spell Water_Walking = new Spell("Water Walking");
    private Timer Water_Walking_Timer = new Timer(0);

    #endregion

    #region Offensive Spell

    private readonly Spell Chain_Lightning = new Spell("Chain Lightning");
    private readonly Spell Earth_Shock = new Spell("Earth Shock");
    private readonly Spell Flame_Shock = new Spell("Flame Shock");
    private readonly Spell Frost_Shock = new Spell("Frost Shock");
    private readonly Spell Lava_Burst = new Spell("Lava Burst");
    private readonly Spell Lightning_Bolt = new Spell("Lightning Bolt");
    private readonly Spell Magma_Totem = new Spell("Magma Totem");
    private readonly Spell Primal_Strike = new Spell("Primal Strike");
    private readonly Spell Searing_Totem = new Spell("Searing Totem");
    private Timer Flame_Shock_Timer = new Timer(0);

    #endregion

    #region Offensive Cooldown

    private readonly Spell Ancestral_Swiftness = new Spell("Ancestral Swiftness");
    private readonly Spell Ascendance = new Spell("Ascendance");
    private readonly Spell Bloodlust = new Spell("Bloodlust");
    private readonly Spell Call_of_the_Elements = new Spell("Call of the Elements");
    private readonly Spell Earth_Elemental_Totem = new Spell("Earth Elemental Totem");
    private readonly Spell Elemental_Blast = new Spell("Elemental Blast");
    private readonly Spell Elemental_Mastery = new Spell("Elemental Mastery");
    private readonly Spell Fire_Elemental_Totem = new Spell("Fire Elemental Totem");
    private readonly Spell Heroism = new Spell("Heroism");
    private readonly Spell Stormlash_Totem = new Spell("Stormlash Totem");
    private readonly Spell Totemic_Projection = new Spell("Totemic Projection");
    private readonly Spell Unleash_Elements = new Spell("Unleash Elements");
    private readonly Spell Unleashed_Fury = new Spell("Unleashed Fury");

    #endregion

    #region Defensive Cooldown

    private readonly Spell Astral_Shift = new Spell("Astral Shift");
    private readonly Spell Capacitor_Totem = new Spell("Capacitor Totem");
    private readonly Spell Earthbind_Totem = new Spell("Earthbind Totem");
    private readonly Spell Grounding_Totem = new Spell("Grounding Totem");
    private readonly Spell Stone_Bulwark_Totem = new Spell("Stone Bulwark Totem");
    private readonly Spell Wind_Shear = new Spell("Wind Shear");

    #endregion

    #region Healing Spell

    private readonly Spell Ancestral_Guidance = new Spell("Ancestral Guidance");
    private readonly Spell Chain_Heal = new Spell("Chain Heal");
    private readonly Spell Greater_Healing_Wave = new Spell("Greater Healing Wave");
    private readonly Spell Healing_Rain = new Spell("Healing Rain");
    private readonly Spell Healing_Stream_Totem = new Spell("Healing Stream Totem");
    private readonly Spell Healing_Surge = new Spell("Healing Surge");
    private readonly Spell Healing_Tide_Totem = new Spell("Healing Tide Totem");
    private readonly Spell Healing_Wave = new Spell("Healing_Wave");
    private readonly Spell Mana_Tide_Totem = new Spell("Mana Tide Totem");
    private readonly Spell Riptide = new Spell("Riptide");
    private readonly Spell Spirit_Link_Totem = new Spell("Spirit Link Totem");
    private readonly Spell Totemic_Recall = new Spell("Totemic Recall");

    #endregion

    public Shaman_Restoration()
    {
        Main.range = 30f;
        UInt64 lastTarget = 0;

        while (Main.loop)
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
                                && (Flame_Shock.IsDistanceGood || Earth_Shock.IsDistanceGood))
                            {
                                Pull();
                                lastTarget = ObjectManager.Me.Target;
                            }

                            if (ObjectManager.Target.Level < 70 && ObjectManager.Me.Level > 84
                                && _mySettings.UseLowCombat)
                            {
                                LC = 1;
                                if (ObjectManager.Target.GetDistance < 41)
                                    LowCombat();
                            }
                            else
                            {
                                LC = 0;
                                if (ObjectManager.Target.GetDistance < 41)
                                    Combat();
                            }
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
            Thread.Sleep(150);
        }
    }

    private void Pull()
    {
        if (Totemic_Projection.KnownSpell && Totemic_Projection.IsSpellUsable && _mySettings.UseTotemicProjection)
            Totemic_Projection.Launch();

        if (Flame_Shock.KnownSpell && Flame_Shock.IsSpellUsable && Flame_Shock.IsDistanceGood
            && _mySettings.UseFlameShock && LC != 1)
        {
            Flame_Shock.Launch();
            return;
        }
        else
        {
            if (Earth_Shock.KnownSpell && Earth_Shock.IsSpellUsable && Earth_Shock.IsDistanceGood
                && _mySettings.UseEarthShock)
            {
                Earth_Shock.Launch();
                return;
            }
        }
    }

    private void LowCombat()
    {
        Buff();
        AvoidMelee();
        Defense_Cycle();
        Heal();

        if (Earth_Shock.KnownSpell && Earth_Shock.IsSpellUsable && Earth_Shock.IsDistanceGood
            && _mySettings.UseEarthShock)
        {
            Earth_Shock.Launch();
            return;
        }
        else if (Lava_Burst.KnownSpell && Lava_Burst.IsSpellUsable && Lava_Burst.IsDistanceGood
                 && _mySettings.UseLavaBurst)
        {
            Lava_Burst.Launch();
            return;
        }
        else if (Chain_Lightning.KnownSpell && Chain_Lightning.IsSpellUsable && Chain_Lightning.IsDistanceGood
                 && _mySettings.UseChainLightning)
        {
            Chain_Lightning.Launch();
            return;
        }
        else
        {
            if (Searing_Totem.KnownSpell && Searing_Totem.IsSpellUsable && _mySettings.UseSearingTotem
                && FireTotemReady() && !Searing_Totem.CreatedBySpellInRange(25) && ObjectManager.Target.GetDistance < 31)
            {
                Searing_Totem.Launch();
                return;
            }
        }

        if (Magma_Totem.KnownSpell && Magma_Totem.IsSpellUsable && ObjectManager.Target.GetDistance < 8
            && _mySettings.UseMagmaTotem && FireTotemReady())
        {
            Magma_Totem.Launch();
            return;
        }
    }

    private void Combat()
    {
        Buff();
        AvoidMelee();
        if (OnCD.IsReady)
            Defense_Cycle();
        Heal();
        Decast();
        DPS_Burst();
        DPS_Cycle();
    }

    private void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (Water_Walking.IsSpellUsable && Water_Walking.KnownSpell &&
            (!Water_Walking.TargetHaveBuff || Water_Walking_Timer.IsReady)
            && ObjectManager.GetNumberAttackPlayer() == 0 && !Fight.InFight && _mySettings.UseWaterWalking)
        {
            Water_Walking.Launch();
            Water_Walking_Timer = new Timer(1000*60*9);
            return;
        }
        else if ((ObjectManager.Me.ManaPercentage < 5 && Water_Shield.KnownSpell && Water_Shield.IsSpellUsable
                  && _mySettings.UseWaterShield && !Water_Shield.TargetHaveBuff)
                 || (!_mySettings.UseLightningShield && !_mySettings.UseEarthShield))
        {
            Water_Shield.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 50 && Earth_Shield.KnownSpell && Earth_Shield.IsSpellUsable
                 && _mySettings.UseEarthShield && !Earth_Shield.TargetHaveBuff && ObjectManager.Me.ManaPercentage > 15
                 || !_mySettings.UseLightningShield)
        {
            Earth_Shield.Launch();
            return;
        }
        else if (Lightning_Shield.KnownSpell && Lightning_Shield.IsSpellUsable && !Lightning_Shield.TargetHaveBuff
                 && _mySettings.UseLightningShield && ObjectManager.Me.ManaPercentage > 15
                 && ObjectManager.Me.HealthPercent > 70)
        {
            Lightning_Shield.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 0 && Spiritwalkers_Grace.IsSpellUsable
                 && Spiritwalkers_Grace.KnownSpell && _mySettings.UseSpiritwalkersGrace && ObjectManager.Me.GetMove)
        {
            Spiritwalkers_Grace.Launch();
            return;
        }
        else
        {
            if (Flametongue_Weapon.KnownSpell && Flametongue_Weapon.IsSpellUsable && !ObjectManager.Me.HaveBuff(10400)
                && _mySettings.UseFlametongueWeapon)
            {
                Flametongue_Weapon.Launch();
                return;
            }
            else if (Earthliving_Weapon.KnownSpell && Earthliving_Weapon.IsSpellUsable &&
                     !ObjectManager.Me.HaveBuff(52007)
                     && _mySettings.UseEarthlivingWeapon && !_mySettings.UseFlametongueWeapon)
            {
                Earthliving_Weapon.Launch();
                return;
            }
            else if (Frostbrand_Weapon.KnownSpell && Frostbrand_Weapon.IsSpellUsable &&
                     !ObjectManager.Me.HaveBuff(8034)
                     && _mySettings.UseFrostbrandWeapon && !_mySettings.UseFlametongueWeapon &&
                     !_mySettings.UseEarthlivingWeapon)
            {
                Frostbrand_Weapon.Launch();
                return;
            }
            else
            {
                if (Rockbiter_Weapon.KnownSpell && Rockbiter_Weapon.IsSpellUsable &&
                    !ObjectManager.Me.HaveBuff(36494)
                    && _mySettings.UseRockbiterWeapon && !_mySettings.UseFlametongueWeapon
                    && !_mySettings.UseFrostbrandWeapon && !_mySettings.UseEarthlivingWeapon)
                {
                    Rockbiter_Weapon.Launch();
                    return;
                }
            }
        }

        if (ObjectManager.GetNumberAttackPlayer() == 0 && Ghost_Wolf.IsSpellUsable && Ghost_Wolf.KnownSpell
            && _mySettings.UseGhostWolf && ObjectManager.Me.GetMove && !Ghost_Wolf.TargetHaveBuff
            && ObjectManager.Target.GetDistance > 50)
        {
            Ghost_Wolf.Launch();
            return;
        }
    }

    private void AvoidMelee()
    {
        if (ObjectManager.Target.GetDistance < 2 && ObjectManager.Target.InCombat)
        {
            Logging.WriteFight("Too Close. Moving Back");
            var MaxTime_Timer = new Timer(1000*2);
            Keyboard.DownKey(Memory.WowProcess.MainWindowHandle, MoveBackward);
            while (ObjectManager.Target.GetDistance < 2 && ObjectManager.Target.InCombat && !MaxTime_Timer.IsReady)
                Thread.Sleep(300);
            Keyboard.UpKey(Memory.WowProcess.MainWindowHandle, MoveBackward);
            if (MaxTime_Timer.IsReady && ObjectManager.Target.GetDistance < 2 && ObjectManager.Target.InCombat)
            {
                Keyboard.DownKey(Memory.WowProcess.MainWindowHandle, MoveForward);
                Thread.Sleep(1000);
                Keyboard.UpKey(Memory.WowProcess.MainWindowHandle, MoveForward);
                MovementManager.Face(ObjectManager.Target.Position);
            }
        }
    }

    private void Defense_Cycle()
    {
        if (ObjectManager.Me.HealthPercent < 50 && Capacitor_Totem.KnownSpell && Capacitor_Totem.IsSpellUsable
            && AirTotemReady() && _mySettings.UseCapacitorTotem)
        {
            Capacitor_Totem.Launch();
            OnCD = new Timer(1000*5);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 50 && Stone_Bulwark_Totem.KnownSpell &&
                 Stone_Bulwark_Totem.IsSpellUsable
                 && EarthTotemReady() && _mySettings.UseStoneBulwarkTotem)
        {
            Stone_Bulwark_Totem.Launch();
            OnCD = new Timer(1000*10);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 70 && Spirit_Link_Totem.KnownSpell &&
                 Spirit_Link_Totem.IsSpellUsable
                 && AirTotemReady() && _mySettings.UseSpiritLinkTotem)
        {
            Spirit_Link_Totem.Launch();
            OnCD = new Timer(1000*6);
            return;
        }
        else if (ObjectManager.Me.HealthPercent <= _mySettings.UseWarStompAtPercentage && War_Stomp.IsSpellUsable &&
                 War_Stomp.KnownSpell
                 && _mySettings.UseWarStomp)
        {
            War_Stomp.Launch();
            OnCD = new Timer(1000*2);
            return;
        }
        else if (ObjectManager.Me.HealthPercent <= _mySettings.UseStoneformAtPercentage && Stoneform.IsSpellUsable &&
                 Stoneform.KnownSpell
                 && _mySettings.UseStoneform)
        {
            Stoneform.Launch();
            OnCD = new Timer(1000*8);
            return;
        }
        else
        {
            if (ObjectManager.Me.HealthPercent < 70 && Astral_Shift.KnownSpell && Astral_Shift.IsSpellUsable
                && _mySettings.UseAstralShift)
            {
                Astral_Shift.Launch();
                OnCD = new Timer(1000*6);
                return;
            }
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (Arcane_Torrent.IsSpellUsable && Arcane_Torrent.KnownSpell &&
            ObjectManager.Me.HealthPercent <= _mySettings.UseArcaneTorrentForResourceAtPercentage
            && _mySettings.UseArcaneTorrentForResource)
        {
            Arcane_Torrent.Launch();
            return;
        }
        else if (ObjectManager.Me.ManaPercentage < 50 && Totemic_Recall.KnownSpell && Totemic_Recall.IsSpellUsable
                 && _mySettings.UseTotemicRecall && ObjectManager.GetNumberAttackPlayer() == 0 && !Fight.InFight
                 && TotemicRecallReady())
        {
            Totemic_Recall.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Me.ManaPercentage < 80 && Mana_Tide_Totem.KnownSpell && Mana_Tide_Totem.IsSpellUsable
                && _mySettings.UseManaTideTotem && WaterTotemReady())
            {
                Mana_Tide_Totem.Launch();
                return;
            }
        }

        if (ObjectManager.Me.HealthPercent < 95 && Healing_Surge.KnownSpell && Healing_Surge.IsSpellUsable
            && ObjectManager.GetNumberAttackPlayer() == 0 && !Fight.InFight && _mySettings.UseHealingSurge)
        {
            Healing_Surge.Launch();
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(200);
            }
            return;
        }
        else if (Healing_Surge.KnownSpell && Healing_Surge.IsSpellUsable && ObjectManager.Me.HealthPercent < 50
                 && _mySettings.UseHealingSurge)
        {
            Healing_Surge.Launch();
            return;
        }
        else if (Greater_Healing_Wave.KnownSpell && Greater_Healing_Wave.IsSpellUsable
                 && ObjectManager.Me.HealthPercent < 60 && _mySettings.UseGreaterHealingWave)
        {
            Greater_Healing_Wave.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent <= _mySettings.UseGiftoftheNaaruAtPercentage &&
                 Gift_of_the_Naaru.KnownSpell && Gift_of_the_Naaru.IsSpellUsable
                 && _mySettings.UseGiftoftheNaaru)
        {
            Gift_of_the_Naaru.Launch();
            return;
        }
        else if (Healing_Tide_Totem.KnownSpell && Healing_Tide_Totem.IsSpellUsable &&
                 ObjectManager.Me.HealthPercent < 70
                 && WaterTotemReady() && _mySettings.UseHealingTideTotem)
        {
            Healing_Tide_Totem.Launch();
            return;
        }
        else if (Ancestral_Guidance.KnownSpell && Ancestral_Guidance.IsSpellUsable &&
                 ObjectManager.Me.HealthPercent < 70
                 && _mySettings.UseAncestralGuidance)
        {
            Ancestral_Guidance.Launch();
            return;
        }
        else if (Chain_Heal.KnownSpell && Chain_Heal.IsSpellUsable && ObjectManager.Me.HealthPercent < 80
                 && _mySettings.UseChainHeal)
        {
            Chain_Heal.Launch();
            return;
        }
        else if (Healing_Stream_Totem.KnownSpell && Healing_Stream_Totem.IsSpellUsable &&
                 ObjectManager.Me.HealthPercent < 90
                 && WaterTotemReady() && _mySettings.UseHealingStreamTotem)
        {
            Healing_Stream_Totem.Launch();
            return;
        }
        else if (Riptide.KnownSpell && Riptide.IsSpellUsable && ObjectManager.Me.HealthPercent < 90
                 && _mySettings.UseRiptide && !Riptide.TargetHaveBuff)
        {
            Riptide.Launch();
            return;
        }
        else
        {
            if (Healing_Wave.KnownSpell && Healing_Wave.IsSpellUsable && ObjectManager.Me.HealthPercent < 80
                && _mySettings.UseHealingWave)
            {
                Healing_Wave.Launch();
                return;
            }
        }
    }

    private void Decast()
    {
        if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && _mySettings.UseWindShear
            && Wind_Shear.KnownSpell && Wind_Shear.IsSpellUsable && Wind_Shear.IsDistanceGood)
        {
            Wind_Shear.Launch();
            return;
        }
        else if (Arcane_Torrent.IsSpellUsable && Arcane_Torrent.KnownSpell && ObjectManager.Target.GetDistance < 8
                 && ObjectManager.Me.HealthPercent <= _mySettings.UseArcaneTorrentForDecastAtPercentage
                 && _mySettings.UseArcaneTorrentForDecast && ObjectManager.Target.IsCast &&
                 ObjectManager.Target.IsTargetingMe)
        {
            Arcane_Torrent.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && _mySettings.UseGroundingTotem
                && Grounding_Totem.KnownSpell && Grounding_Totem.IsSpellUsable && AirTotemReady())
            {
                Grounding_Totem.Launch();
                return;
            }
        }

        if (ObjectManager.Target.GetMove && !Frost_Shock.TargetHaveBuff && _mySettings.UseFrostShock
            && Frost_Shock.KnownSpell && Frost_Shock.IsSpellUsable && Frost_Shock.IsDistanceGood)
        {
            Frost_Shock.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Target.GetMove && _mySettings.UseEarthbindTotem && EarthTotemReady()
                && Earthbind_Totem.KnownSpell && Earthbind_Totem.IsSpellUsable && Earthbind_Totem.IsDistanceGood)
            {
                Earthbind_Totem.Launch();
                return;
            }
        }
    }

    private void DPS_Burst()
    {
        if (_mySettings.UseTrinket && Trinket_Timer.IsReady && ObjectManager.Target.GetDistance < 30)
        {
            Logging.WriteFight("Use Trinket 1.");
            Lua.RunMacroText("/use 13");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Logging.WriteFight("Use Trinket 2.");
            Lua.RunMacroText("/use 14");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Trinket_Timer = new Timer(1000*60*2);
        }
        else if (Berserking.IsSpellUsable && Berserking.KnownSpell && ObjectManager.Target.GetDistance < 30
                 && _mySettings.UseBerserking)
            Berserking.Launch();
        else if (Blood_Fury.IsSpellUsable && Blood_Fury.KnownSpell && ObjectManager.Target.GetDistance < 30
                 && _mySettings.UseBloodFury)
            Blood_Fury.Launch();
        else if (Lifeblood.IsSpellUsable && Lifeblood.KnownSpell && ObjectManager.Target.GetDistance < 30
                 && _mySettings.UseLifeblood)
            Lifeblood.Launch();
        else if (Engineering_Timer.IsReady && Engineering.KnownSpell && ObjectManager.Target.GetDistance < 30
                 && _mySettings.UseEngGlove)
        {
            Logging.WriteFight("Use Engineering Gloves.");
            Lua.RunMacroText("/use 10");
            Engineering_Timer = new Timer(1000*60);
        }
        else if (Unleash_Elements.KnownSpell && Unleash_Elements.IsSpellUsable && Unleashed_Fury.KnownSpell
                 && _mySettings.UseUnleashElements && Unleash_Elements.IsDistanceGood)
        {
            Unleash_Elements.Launch();
            return;
        }
        else if (Elemental_Blast.KnownSpell && Elemental_Blast.IsSpellUsable
                 && _mySettings.UseElementalBlast && Elemental_Blast.IsDistanceGood)
        {
            Elemental_Blast.Launch();
            return;
        }
        else if (Ascendance.KnownSpell && Ascendance.IsSpellUsable && ObjectManager.Me.HealthPercent < 80
                 && _mySettings.UseAscendance && ObjectManager.Target.GetDistance < 40)
        {
            Ascendance.Launch();
            return;
        }
        else if (Fire_Elemental_Totem.KnownSpell && Fire_Elemental_Totem.IsSpellUsable
                 && _mySettings.UseFireElementalTotem && ObjectManager.Target.GetDistance < 40)
        {
            Fire_Elemental_Totem.Launch();
            return;
        }
        else if (Stormlash_Totem.KnownSpell && AirTotemReady()
                 && _mySettings.UseStormlashTotem && ObjectManager.Target.GetDistance < 40)
        {
            if (!Stormlash_Totem.IsSpellUsable && _mySettings.UseCalloftheElements
                && Call_of_the_Elements.KnownSpell && Call_of_the_Elements.IsSpellUsable)
            {
                Call_of_the_Elements.Launch();
                Thread.Sleep(200);
            }

            if (Stormlash_Totem.IsSpellUsable)
                Stormlash_Totem.Launch();
            return;
        }
        else if (Bloodlust.KnownSpell && Bloodlust.IsSpellUsable && _mySettings.UseBloodlustHeroism
                 && ObjectManager.Target.GetDistance < 40 && !ObjectManager.Me.HaveBuff(57724))
        {
            Bloodlust.Launch();
            return;
        }

        else if (Heroism.KnownSpell && Heroism.IsSpellUsable && _mySettings.UseBloodlustHeroism
                 && ObjectManager.Target.GetDistance < 40 && !ObjectManager.Me.HaveBuff(57723))
        {
            Heroism.Launch();
            return;
        }
        else
        {
            if (Elemental_Mastery.KnownSpell && Elemental_Mastery.IsSpellUsable
                && !ObjectManager.Me.HaveBuff(2825) && _mySettings.UseElementalMastery
                && !ObjectManager.Me.HaveBuff(32182))
            {
                Elemental_Mastery.Launch();
                return;
            }
        }
    }

    private void DPS_Cycle()
    {
        if (Primal_Strike.KnownSpell && Primal_Strike.IsSpellUsable && Primal_Strike.IsDistanceGood
            && _mySettings.UsePrimalStrike && ObjectManager.Me.Level < 11)
        {
            Primal_Strike.Launch();
            return;
        }

        if (Earth_Elemental_Totem.KnownSpell && Earth_Elemental_Totem.IsSpellUsable
            && ObjectManager.GetNumberAttackPlayer() > 3 && _mySettings.UseEarthElementalTotem)
        {
            Earth_Elemental_Totem.Launch();
            return;
        }
        else if (Flame_Shock.IsSpellUsable && Flame_Shock.IsDistanceGood && Flame_Shock.KnownSpell
                 && _mySettings.UseFlameShock && (!Flame_Shock.TargetHaveBuff || Flame_Shock_Timer.IsReady))
        {
            Flame_Shock.Launch();
            Flame_Shock_Timer = new Timer(1000*27);
            return;
        }
        else if (Lava_Burst.KnownSpell && Lava_Burst.IsSpellUsable && Lava_Burst.IsDistanceGood
                 && _mySettings.UseLavaBurst && Flame_Shock.TargetHaveBuff)
        {
            Lava_Burst.Launch();
            return;
        }
        else if (Earth_Shock.IsSpellUsable && Earth_Shock.KnownSpell && Earth_Shock.IsDistanceGood
                 && _mySettings.UseEarthShock && Flame_Shock.TargetHaveBuff)
        {
            Earth_Shock.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 1 && Magma_Totem.KnownSpell
                 && Magma_Totem.IsSpellUsable && _mySettings.UseMagmaTotem
                 && !Fire_Elemental_Totem.CreatedBySpell)
        {
            Magma_Totem.Launch();
            return;
        }
        if (Searing_Totem.KnownSpell && Searing_Totem.IsSpellUsable && _mySettings.UseSearingTotem
            && FireTotemReady() && !Searing_Totem.CreatedBySpellInRange(25) && ObjectManager.Target.GetDistance < 31)
        {
            Searing_Totem.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 1 && Chain_Lightning.KnownSpell
                 && Chain_Lightning.IsSpellUsable && Chain_Lightning.IsDistanceGood
                 && _mySettings.UseChainLightning && !ObjectManager.Me.HaveBuff(77762))
        {
            if (Ancestral_Swiftness.KnownSpell && Ancestral_Swiftness.IsSpellUsable
                && _mySettings.UseAncestralSwiftness)
            {
                Ancestral_Swiftness.Launch();
                Thread.Sleep(200);
            }
            Chain_Lightning.Launch();
            return;
        }
        else
        {
            if (Lightning_Bolt.IsDistanceGood && Lightning_Bolt.KnownSpell && Lightning_Bolt.IsSpellUsable
                && _mySettings.UseLightningBolt && !ObjectManager.Me.HaveBuff(77762))
            {
                if (Ancestral_Swiftness.KnownSpell && Ancestral_Swiftness.IsSpellUsable
                    && _mySettings.UseAncestralSwiftness)
                {
                    Ancestral_Swiftness.Launch();
                    Thread.Sleep(200);
                }
                Lightning_Bolt.Launch();
                return;
            }
        }
    }

    private bool FireTotemReady()
    {
        if (Fire_Elemental_Totem.CreatedBySpell || Magma_Totem.CreatedBySpell)
            return false;
        return true;
    }

    private bool EarthTotemReady()
    {
        if (Earthbind_Totem.CreatedBySpell || Earth_Elemental_Totem.CreatedBySpell
            || Stone_Bulwark_Totem.CreatedBySpell)
            return false;
        return true;
    }

    private bool WaterTotemReady()
    {
        if (Healing_Stream_Totem.CreatedBySpell || Healing_Tide_Totem.CreatedBySpell
            || Mana_Tide_Totem.CreatedBySpell)
            return false;
        return true;
    }

    private bool AirTotemReady()
    {
        if (Capacitor_Totem.CreatedBySpell || Grounding_Totem.CreatedBySpell
            || Stormlash_Totem.CreatedBySpell || Spirit_Link_Totem.CreatedBySpell)
            return false;
        return true;
    }

    private bool TotemicRecallReady()
    {
        if (Fire_Elemental_Totem.CreatedBySpell)
            return false;
        else if (Earth_Elemental_Totem.CreatedBySpell)
            return false;
        else if (Searing_Totem.CreatedBySpell)
            return true;
        else if (FireTotemReady() && EarthTotemReady() && WaterTotemReady() && AirTotemReady())
            return false;
        else
            return true;
    }

    private void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            Buff();
            Heal();
        }
    }

    #region Nested type: ShamanRestorationSettings

    [Serializable]
    public class ShamanRestorationSettings : Settings
    {
        public bool UseAlchFlask = true;
        public bool UseAncestralGuidance = true;
        public bool UseAncestralSwiftness = true;
        public bool UseArcaneTorrentForDecast = true;
        public int UseArcaneTorrentForDecastAtPercentage = 100;
        public bool UseArcaneTorrentForResource = true;
        public int UseArcaneTorrentForResourceAtPercentage = 80;
        public bool UseAscendance = true;
        public bool UseAstralShift = true;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseBloodlustHeroism = true;
        public bool UseCalloftheElements = true;
        public bool UseCapacitorTotem = true;
        public bool UseChainHeal = false;
        public bool UseChainLightning = true;
        public bool UseEarthElementalTotem = true;
        public bool UseEarthShield = true;
        public bool UseEarthShock = true;
        public bool UseEarthbindTotem = false;
        public bool UseEarthlivingWeapon = true;
        public bool UseElementalBlast = true;
        public bool UseElementalMastery = true;
        public bool UseEngGlove = true;
        public bool UseFireElementalTotem = true;
        public bool UseFlameShock = true;
        public bool UseFlametongueWeapon = true;
        public bool UseFrostShock = false;
        public bool UseFrostbrandWeapon = false;
        public bool UseGhostWolf = true;
        public bool UseGiftoftheNaaru = true;
        public int UseGiftoftheNaaruAtPercentage = 80;
        public bool UseGreaterHealingWave = true;
        public bool UseGroundingTotem = true;
        public bool UseHealingRain = true;
        public bool UseHealingStreamTotem = true;
        public bool UseHealingSurge = true;
        public bool UseHealingTideTotem = true;
        public bool UseHealingWave = false;
        public bool UseLavaBurst = true;
        public bool UseLifeblood = true;
        public bool UseLightningBolt = true;
        public bool UseLightningShield = true;
        public bool UseLowCombat = true;
        public bool UseMagmaTotem = true;
        public bool UseManaTideTotem = true;
        public bool UsePrimalStrike = true;
        public bool UseRiptide = true;
        public bool UseRockbiterWeapon = false;
        public bool UseSearingTotem = true;
        public bool UseSpiritLinkTotem = true;
        public bool UseSpiritwalkersGrace = true;
        public bool UseStoneBulwarkTotem = true;
        public bool UseStoneform = true;
        public int UseStoneformAtPercentage = 80;
        public bool UseStormlashTotem = true;
        public bool UseTotemicProjection = true;
        public bool UseTotemicRecall = true;
        public bool UseTrinket = true;
        public bool UseUnleashElements = true;
        public bool UseWarStomp = true;
        public int UseWarStompAtPercentage = 80;
        public bool UseWaterShield = true;
        public bool UseWaterWalking = true;
        public bool UseWindShear = true;

        public ShamanRestorationSettings()
        {
            ConfigWinForm(new Point(500, 400), "Shaman Restoration Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent for Interrupt", "UseArcaneTorrentForDecast", "Professions & Racials",
                                "AtPercentage");
            AddControlInWinForm("Use Arcane Torrent for Resource", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");
            AddControlInWinForm("Use Lifeblood", "UseLifeblood", "Professions & Racials");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            /* Shaman Buffs */
            AddControlInWinForm("Use Earth Shield", "UseEarthShield", "Shaman Buffs");
            AddControlInWinForm("Use Earthliving Weapon", "UseEarthlivingWeapon", "Shaman Buffs");
            AddControlInWinForm("Use Flametongue Weapon", "UseFlametongueWeapon", "Shaman Buffs");
            AddControlInWinForm("Use Frostbrand Weapon", "UseFrostbrandWeapon", "Shaman Buffs");
            AddControlInWinForm("Use Ghost Wolf", "UseGhostWolf", "Shaman Buffs");
            AddControlInWinForm("Use Lightning Shield", "UseLightningShield", "Shaman Buffs");
            AddControlInWinForm("Use Rockbiter Weapon", "UseRockbiterWeapon", "Shaman Buffs");
            AddControlInWinForm("Use Spiritwalker's Grace", "UseSpiritwalkersGrace", "Shaman Buffs");
            AddControlInWinForm("Use Water Shield", "UseWaterShield", "Shaman Buffs");
            AddControlInWinForm("Use Water Walking", "UseWaterWalking", "Shaman Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Chain Lightning", "UseChainLightning", "Offensive Spell");
            AddControlInWinForm("Use Earth Shock", "UseEarthShock", "Offensive Spell");
            AddControlInWinForm("Use Flame Shock", "UseFlameShock", "Offensive Spell");
            AddControlInWinForm("Use Frost Shock", "UseFrostShock", "Offensive Spell");
            AddControlInWinForm("Use Lava Burst", "UseLavaBurst", "Offensive Spell");
            AddControlInWinForm("Use Lightning Bolt", "UseLightningBolt", "Offensive Spell");
            AddControlInWinForm("Use Magma Totem", "UseMagmaTotem", "Offensive Spell");
            AddControlInWinForm("Use Searing Totem", "UseSearingTotem", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use Ancestral Swiftness", "UseAncestralSwiftness", "Offensive Cooldown");
            AddControlInWinForm("Use Ascendance", "UseAscendance", "Offensive Cooldown");
            AddControlInWinForm("Use Bloodlust / Heroism", "UseBloodlustHeroism", "Offensive Cooldown");
            AddControlInWinForm("Use Call of the Elements", "UseCalloftheElements", "Offensive Cooldown");
            AddControlInWinForm("Use Earth Elemental Totem", "UseEarthElementalTotem", "Offensive Cooldown");
            AddControlInWinForm("Use Elemental Blast", "UseElementalBlast", "Offensive Cooldown");
            AddControlInWinForm("Use Elemental Mastery", "UseElementalMastery", "Offensive Cooldown");
            AddControlInWinForm("Use Fire Elemental Totem", "UseFireElementalTotem", "Offensive Cooldown");
            AddControlInWinForm("Use Stormlash Totem", "UseStormlashTotem", "Offensive Cooldown");
            AddControlInWinForm("Use Totemic Projection", "UseTotemicProjection", "Offensive Cooldown");
            AddControlInWinForm("Use Unleash Elements", "UseUnleashElements", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Astral Shift", "UseAstralShift", "Defensive Cooldown");
            AddControlInWinForm("Use Capacitor Totem", "UseCapacitorTotem", "Defensive Cooldown");
            AddControlInWinForm("Use Earthbind Totem", "UseEarthbindTotem", "Defensive Cooldown");
            AddControlInWinForm("Use Grounding Totem", "UseGroundingTotem", "Defensive Cooldown");
            AddControlInWinForm("Use StoneBulwark Totem", "UseStoneBulwarkTotem", "Defensive Cooldown");
            AddControlInWinForm("Use Wind Shear", "UseWindShear", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Ancestral Guidance", "UseAncestralGuidance", "Healing Spell");
            AddControlInWinForm("Use Chain Heal", "UseChainHeal", "Healing Spell");
            AddControlInWinForm("Use Greater Healing Wave", "UseGreaterHealingWave", "Healing Spell");
            AddControlInWinForm("Use Healing Rain", "UseHealingRain", "Healing Spell");
            AddControlInWinForm("Use Healing Surge", "UseHealingSurge", "Healing Spell");
            AddControlInWinForm("Use Healing Stream Totem", "UseHealingStream_Totem", "Healing Spell");
            AddControlInWinForm("Use Healing Tide Totem", "UsHealingTideTotem", "Healing Spell");
            AddControlInWinForm("Use Healing Wave", "UseHealingWave", "Healing Spell");
            AddControlInWinForm("Use Mana Tide Totem", "UseManaTideTotem", "Healing Spell");
            AddControlInWinForm("Use Riptide", "UseRiptide", "Healing Spell");
            AddControlInWinForm("Use Spirit Link Totem", "UseSpiritLinkTotem", "Healing Spell");
            AddControlInWinForm("Use Totemic Recall", "UseTotemicRecall", "Healing Spell");
            /* Game Settings */
            AddControlInWinForm("Use Low Combat Settings", "UseLowCombat", "Game Settings");
            AddControlInWinForm("Use Trinket", "UseTrinket", "Game Settings");
            AddControlInWinForm("Use Engineering Gloves", "UseEngGlove", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
        }

        public static ShamanRestorationSettings CurrentSetting { get; set; }

        public static ShamanRestorationSettings GetSettings()
        {
            string CurrentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Shaman_Restoration.xml";
            if (File.Exists(CurrentSettingsFile))
            {
                return
                    CurrentSetting = Load<ShamanRestorationSettings>(CurrentSettingsFile);
            }
            else
            {
                return new ShamanRestorationSettings();
            }
        }
    }

    #endregion
}

#endregion

#region Priest

public class Priest_Discipline
{
    private readonly string MoveBackward =
        Keybindings.GetKeyByAction(nManager.Wow.Enums.Keybindings.MOVEBACKWARD);

    private readonly string MoveForward =
        Keybindings.GetKeyByAction(nManager.Wow.Enums.Keybindings.MOVEFORWARD);

    private readonly PriestDisciplineSettings _mySettings = PriestDisciplineSettings.GetSettings();

    #region General Timers & Variables

    private Timer AlchFlask_Timer = new Timer(0);
    private Timer Engineering_Timer = new Timer(0);
    private Timer OnCD = new Timer(0);
    private Timer Trinket_Timer = new Timer(0);

    #endregion

    #region Professions and Racials

    private readonly Spell Alchemy = new Spell("Alchemy");
    private readonly Spell Arcane_Torrent = new Spell("Arcane Torrent");
    private readonly Spell Berserking = new Spell("Berserking");
    private readonly Spell Blood_Fury = new Spell("Blood Fury");
    private readonly Spell Engineering = new Spell("Engineering");
    private readonly Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    private readonly Spell Lifeblood = new Spell("Lifeblood");
    private readonly Spell Stoneform = new Spell("Stoneform");
    private readonly Spell War_Stomp = new Spell("War Stomp");

    #endregion

    #region Priest Buffs

    private readonly Spell Inner_Fire = new Spell("Inner Fire");
    private readonly Spell Inner_Will = new Spell("Inner Will");
    private readonly Spell Levitate = new Spell("Levitate");
    private readonly Spell Power_Word_Fortitude = new Spell("Power Word: Fortitude");
    private Timer Levitate_Timer = new Timer(0);

    #endregion

    #region Offensive Spell

    private readonly Spell Cascade = new Spell("Cascade");
    private readonly Spell Divine_Star = new Spell("Divine Star");
    private readonly Spell Halo = new Spell("Halo");
    private readonly Spell Mind_Sear = new Spell("Mind Sear");
    private readonly Spell Power_Word_Solace = new Spell("Power Word: Solace");
    private readonly Spell Shadow_Word_Death = new Spell("Shadow Word: Death");
    private readonly Spell Shadow_Word_Pain = new Spell("Shadow Word: Pain");
    private readonly Spell Smite = new Spell("Smite");
    private Timer Shadow_Word_Pain_Timer = new Timer(0);

    #endregion

    #region Healing Cooldown

    private readonly Spell Archangel = new Spell("Archangel");
    private readonly Spell Inner_Focus = new Spell("Inner Focus");
    private readonly Spell Power_Infusion = new Spell("Power Infusion");
    private readonly Spell Shadowfiend = new Spell("Shadowfiend");

    #endregion

    #region Defensive Cooldown

    private readonly Spell Pain_Suppression = new Spell("Pain Suppression");
    private readonly Spell Power_Word_Barrier = new Spell("Power Word: Barrier");
    private readonly Spell Power_Word_Shield = new Spell("Power Word: Shield");
    private readonly Spell Psychic_Scream = new Spell("Psychic Scream");
    private readonly Spell Psyfiend = new Spell("Psyfiend");
    private readonly Spell Spectral_Guise = new Spell("Spectral Guise");
    private readonly Spell Void_Tendrils = new Spell("Void Tendrils");

    #endregion

    #region Healing Spell

    private readonly Spell Desperate_Prayer = new Spell("Desperate Prayer");
    private readonly Spell Flash_Heal = new Spell("Flash Heal");
    private readonly Spell Greater_Heal = new Spell("Greater Heal");
    private readonly Spell Heal_Spell = new Spell("Heal");
    private readonly Spell Holy_Fire = new Spell("Holy Fire");
    private readonly Spell Hymn_of_Hope = new Spell("Hymn of Hope");
    private readonly Spell Penance = new Spell("Penance");
    private readonly Spell Prayer_of_Healing = new Spell("Prayer of Healing");
    private readonly Spell Prayer_of_Mending = new Spell("Prayer of Mending");
    private readonly Spell Renew = new Spell("Renew");
    private readonly Spell Spirit_Shell = new Spell("Spirit Shell");
    private Timer Renew_Timer = new Timer(0);

    #endregion

    public Priest_Discipline()
    {
        Main.range = 30f;
        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            try
            {
                if (!ObjectManager.Me.IsDead)
                {
                    if (!ObjectManager.Me.IsMounted)
                    {
                        Buff_Levitate();
                        if (Fight.InFight && ObjectManager.Me.Target > 0)
                        {
                            if (ObjectManager.Me.Target != lastTarget &&
                                (Holy_Fire.IsDistanceGood || Shadow_Word_Pain.IsDistanceGood))
                            {
                                Pull();
                                lastTarget = ObjectManager.Me.Target;
                            }
                            else
                            {
                                if (ObjectManager.Target.GetDistance < 41)
                                    Combat();
                            }
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
            Thread.Sleep(150);
        }
    }

    private void Buff_Levitate()
    {
        if (!Fight.InFight && Levitate.KnownSpell && Levitate.IsSpellUsable && _mySettings.UseLevitate
            && (!Levitate.TargetHaveBuff || Levitate_Timer.IsReady))
        {
            Levitate.Launch();
            Levitate_Timer = new Timer(1000*60*9);
        }
    }

    private void Pull()
    {
        if (Holy_Fire.IsSpellUsable && Holy_Fire.KnownSpell && Holy_Fire.IsDistanceGood
            && _mySettings.UseHolyFire)
        {
            Holy_Fire.Launch();
            return;
        }
        else
        {
            if (Shadow_Word_Pain.IsSpellUsable && Shadow_Word_Pain.KnownSpell && Shadow_Word_Pain.IsDistanceGood
                && _mySettings.UseShadowWordPain)
            {
                Shadow_Word_Pain.Launch();
                Shadow_Word_Pain_Timer = new Timer(1000*14);
                return;
            }
        }
    }

    private void Combat()
    {
        AvoidMelee();
        if (OnCD.IsReady)
            Defense_Cycle();
        Heal();
        Buff();
        Healing_Burst();
        DPS_Cycle();
    }

    private void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (Power_Word_Fortitude.KnownSpell && Power_Word_Fortitude.IsSpellUsable &&
            !Power_Word_Fortitude.TargetHaveBuff && _mySettings.UsePowerWordFortitude)
        {
            Power_Word_Fortitude.Launch();
            return;
        }
        else if (Inner_Fire.KnownSpell && Inner_Fire.IsSpellUsable && !Inner_Fire.TargetHaveBuff
                 && _mySettings.UseInnerFire)
        {
            Inner_Fire.Launch();
            return;
        }
        else if (Inner_Will.KnownSpell && Inner_Will.IsSpellUsable && !Inner_Will.TargetHaveBuff
                 && !_mySettings.UseInnerFire && _mySettings.UseInnerWill)
        {
            Inner_Will.Launch();
            return;
        }
        else
        {
            if (AlchFlask_Timer.IsReady && _mySettings.UseAlchFlask
                && ItemsManager.GetItemCountByIdLUA(75525) == 1)
            {
                Logging.WriteFight("Use Alchi Flask");
                Lua.RunMacroText("/use item:75525");
                AlchFlask_Timer = new Timer(1000*60*60*2);
            }
        }
    }

    private void AvoidMelee()
    {
        if (ObjectManager.Target.GetDistance < 2 && ObjectManager.Target.InCombat)
        {
            Logging.WriteFight("Too Close. Moving Back");
            var MaxTime_Timer = new Timer(1000*2);
            Keyboard.DownKey(Memory.WowProcess.MainWindowHandle, MoveBackward);
            while (ObjectManager.Target.GetDistance < 2 && ObjectManager.Target.InCombat && !MaxTime_Timer.IsReady)
                Thread.Sleep(300);
            Keyboard.UpKey(Memory.WowProcess.MainWindowHandle, MoveBackward);
            if (MaxTime_Timer.IsReady && ObjectManager.Target.GetDistance < 2 && ObjectManager.Target.InCombat)
            {
                Keyboard.DownKey(Memory.WowProcess.MainWindowHandle, MoveForward);
                Thread.Sleep(1000);
                Keyboard.UpKey(Memory.WowProcess.MainWindowHandle, MoveForward);
                MovementManager.Face(ObjectManager.Target.Position);
            }
        }
    }

    private void Defense_Cycle()
    {
        if (ObjectManager.Me.HealthPercent <= _mySettings.UsePsychicScreamAtPercentage && Psychic_Scream.IsSpellUsable &&
            Psychic_Scream.KnownSpell
            && _mySettings.UsePsychicScream)
        {
            Psychic_Scream.Launch();
            OnCD = new Timer(1000*8);
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() >= 2 &&
                 ObjectManager.Me.HealthPercent <= _mySettings.UseVoidTendrilsAtPercentage &&
                 Void_Tendrils.IsSpellUsable && Void_Tendrils.KnownSpell && _mySettings.UseVoidTendrils)
        {
            Void_Tendrils.Launch();
            OnCD = new Timer(1000*10);
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() >= 2 &&
                 ObjectManager.Me.HealthPercent <= _mySettings.UsePsyfiendAtPercentage &&
                 Psyfiend.IsSpellUsable && Psyfiend.KnownSpell && _mySettings.UsePsyfiend)
        {
            Psyfiend.Launch();
            OnCD = new Timer(1000*10);
            return;
        }
        else if (ObjectManager.Me.HealthPercent <= _mySettings.UseSpectralGuiseAtPercentage &&
                 Spectral_Guise.IsSpellUsable && Spectral_Guise.KnownSpell
                 && _mySettings.UseSpectralGuise)
        {
            if (Renew.KnownSpell && Renew.IsSpellUsable && _mySettings.UseRenew)
            {
                Renew.Launch();
                Thread.Sleep(1500);
            }
            Spectral_Guise.Launch();
            OnCD = new Timer(1000*3);
            return;
        }
        else if (ObjectManager.Me.HealthPercent <= _mySettings.UsePowerWordBarrierAtPercentage &&
                 Power_Word_Barrier.IsSpellUsable && Power_Word_Barrier.KnownSpell
                 && _mySettings.UsePowerWordBarrier)
        {
            SpellManager.CastSpellByIDAndPosition(62618, ObjectManager.Me.Position);
            OnCD = new Timer(1000*10);
            return;
        }
        else if (ObjectManager.Me.HealthPercent <= _mySettings.UsePainSuppressionAtPercentage &&
                 Pain_Suppression.IsSpellUsable && Pain_Suppression.KnownSpell
                 && _mySettings.UsePainSuppression)
        {
            Pain_Suppression.Launch();
            OnCD = new Timer(1000*8);
            return;
        }
        else if (ObjectManager.Me.HealthPercent <= _mySettings.UseStoneformAtPercentage &&
                 Stoneform.IsSpellUsable && Stoneform.KnownSpell
                 && _mySettings.UseStoneform)
        {
            Stoneform.Launch();
            OnCD = new Timer(1000*8);
            return;
        }
        else
        {
            if (ObjectManager.Me.HealthPercent <= _mySettings.UseWarStompAtPercentage &&
                War_Stomp.IsSpellUsable && War_Stomp.KnownSpell
                && _mySettings.UseWarStomp)
            {
                War_Stomp.Launch();
                OnCD = new Timer(1000*2);
                return;
            }
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.HealthPercent <= _mySettings.UseFlashHealNonCombatAtPercentage && !Fight.InFight &&
            ObjectManager.GetNumberAttackPlayer() == 0
            && Flash_Heal.KnownSpell && Flash_Heal.IsSpellUsable && _mySettings.UseFlashHealNonCombat)
        {
            Flash_Heal.Launch(false);
            return;
        }
        else if (ObjectManager.Me.HealthPercent <= _mySettings.UseInnerFocusAtPercentage && Inner_Focus.KnownSpell &&
                 Inner_Focus.IsSpellUsable
                 && _mySettings.UseInnerFocus && !Inner_Focus.TargetHaveBuff)
        {
            Inner_Focus.Launch();
            return;
        }
        else if (!Fight.InFight && ObjectManager.Me.ManaPercentage <= _mySettings.UseHymnofHopeAtPercentage &&
                 Hymn_of_Hope.KnownSpell
                 && Hymn_of_Hope.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() == 0 &&
                 _mySettings.UseHymnofHope)
        {
            Hymn_of_Hope.Launch(false);
            return;
        }
        else if (ObjectManager.Me.HealthPercent <= _mySettings.UseDesperatePrayerAtPercentage &&
                 Desperate_Prayer.KnownSpell && Desperate_Prayer.IsSpellUsable
                 && _mySettings.UseDesperatePrayer)
        {
            Desperate_Prayer.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent <= _mySettings.UseFlashHealInCombatAtPercentage &&
                 Flash_Heal.KnownSpell && Flash_Heal.IsSpellUsable
                 && _mySettings.UseFlashHealInCombat)
        {
            Flash_Heal.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent <= _mySettings.UseGreaterHealAtPercentage &&
                 Greater_Heal.KnownSpell && Greater_Heal.IsSpellUsable
                 && _mySettings.UseGreaterHeal)
        {
            Greater_Heal.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent <= _mySettings.UseGiftoftheNaaruAtPercentage &&
                 Gift_of_the_Naaru.IsSpellUsable && Gift_of_the_Naaru.KnownSpell
                 && _mySettings.UseGiftoftheNaaru)
        {
            Gift_of_the_Naaru.Launch();
            return;
        }
        else if (Power_Word_Shield.KnownSpell && Power_Word_Shield.IsSpellUsable
                 && !Power_Word_Shield.TargetHaveBuff && _mySettings.UsePowerWordShield
                 && !ObjectManager.Me.HaveBuff(6788) &&
                 ObjectManager.Me.HealthPercent <= _mySettings.UsePowerWordShieldAtPercentage
                 && (ObjectManager.GetNumberAttackPlayer() > 0 || ObjectManager.Me.GetMove))
        {
            Power_Word_Shield.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent <= _mySettings.UsePrayerofHealingAtPercentage &&
                 Prayer_of_Healing.KnownSpell && Prayer_of_Healing.IsSpellUsable
                 && _mySettings.UsePrayerofHealing)
        {
            Prayer_of_Healing.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent <= _mySettings.UsePrayerofMendingAtPercentage &&
                 Prayer_of_Mending.KnownSpell && Prayer_of_Mending.IsSpellUsable
                 && _mySettings.UsePrayerofMending)
        {
            Prayer_of_Mending.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent <= _mySettings.UseHealAtPercentage &&
                 Heal_Spell.KnownSpell && Heal_Spell.IsSpellUsable
                 && (_mySettings.UseHeal || !Greater_Heal.KnownSpell))
        {
            Heal_Spell.Launch();
            return;
        }
        else
        {
            if (Renew.KnownSpell && Renew.IsSpellUsable && !Renew.TargetHaveBuff &&
                ObjectManager.Me.HealthPercent <= _mySettings.UseRenewAtPercentage &&
                _mySettings.UseRenew)
            {
                Renew.Launch();
                return;
            }
        }
    }

    private void Healing_Burst()
    {
        if (_mySettings.UseTrinket && Trinket_Timer.IsReady && ObjectManager.Target.GetDistance < 30)
        {
            Logging.WriteFight("Use Trinket 1.");
            Lua.RunMacroText("/use 13");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Logging.WriteFight("Use Trinket 2.");
            Lua.RunMacroText("/use 14");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Trinket_Timer = new Timer(1000*60*2);
        }
        else if (Berserking.IsSpellUsable && Berserking.KnownSpell && ObjectManager.Target.GetDistance < 30
                 && _mySettings.UseBerserking)
            Berserking.Launch();
        else if (Blood_Fury.IsSpellUsable && Blood_Fury.KnownSpell && ObjectManager.Target.GetDistance < 30
                 && _mySettings.UseBloodFury)
            Blood_Fury.Launch();
        else if (Lifeblood.IsSpellUsable && Lifeblood.KnownSpell && ObjectManager.Target.GetDistance < 30
                 && _mySettings.UseLifeblood)
            Lifeblood.Launch();
        else if (Engineering_Timer.IsReady && Engineering.KnownSpell && ObjectManager.Target.GetDistance < 30
                 && _mySettings.UseEngGlove)
        {
            Logging.WriteFight("Use Engineering Gloves.");
            Lua.RunMacroText("/use 10");
            Engineering_Timer = new Timer(1000*60);
        }
        else if (Power_Infusion.IsSpellUsable && Power_Infusion.KnownSpell
                 && _mySettings.UsePowerInfusion && ObjectManager.Target.GetDistance < 40)
        {
            Power_Infusion.Launch();
            return;
        }
        else if (Archangel.IsSpellUsable && Archangel.KnownSpell && ObjectManager.Me.BuffStack(81661) > 4
                 && _mySettings.UseArchangel && ObjectManager.Target.GetDistance < 40)
        {
            Archangel.Launch();
            return;
        }
        else if (Spirit_Shell.IsSpellUsable && Spirit_Shell.KnownSpell && ObjectManager.Me.HealthPercent > 80
                 && _mySettings.UseSpiritShell && ObjectManager.Target.InCombat)
        {
            Spirit_Shell.Launch();
            return;
        }
        else
        {
            if (Shadowfiend.IsSpellUsable && Shadowfiend.KnownSpell && Shadowfiend.IsDistanceGood
                && _mySettings.UseShadowfiend)
            {
                Shadowfiend.Launch();
                return;
            }
        }
    }

    private void DPS_Cycle()
    {
        if (ObjectManager.Me.ManaPercentage < 80 && Arcane_Torrent.KnownSpell && Arcane_Torrent.IsSpellUsable
            && _mySettings.UseArcaneTorrent)
        {
            Arcane_Torrent.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 2 && Cascade.IsSpellUsable && Cascade.KnownSpell
                 && Cascade.IsDistanceGood && _mySettings.UseCascade)
        {
            Cascade.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 2 && Divine_Star.IsSpellUsable && Divine_Star.KnownSpell
                 && Divine_Star.IsDistanceGood && _mySettings.UseDivineStar)
        {
            Divine_Star.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 2 && Halo.IsSpellUsable && Halo.KnownSpell
                 && Halo.IsDistanceGood && _mySettings.UseHalo)
        {
            Halo.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 4 && Mind_Sear.IsSpellUsable && Mind_Sear.KnownSpell
                 && Mind_Sear.IsDistanceGood && !ObjectManager.Me.IsCast && _mySettings.UseMindSear)
        {
            Mind_Sear.Launch();
            return;
        }
        else if (Shadow_Word_Death.IsSpellUsable && Shadow_Word_Death.IsDistanceGood && Shadow_Word_Death.KnownSpell
                 && ObjectManager.Target.HealthPercent < 20 && _mySettings.UseShadowWordDeath)
        {
            Shadow_Word_Death.Launch();
            return;
        }
        else if (Shadow_Word_Pain.KnownSpell && Shadow_Word_Pain.IsSpellUsable
                 && Shadow_Word_Pain.IsDistanceGood && _mySettings.UseShadowWordPain
                 && (!Shadow_Word_Pain.TargetHaveBuff || Shadow_Word_Pain_Timer.IsReady))
        {
            Shadow_Word_Pain.Launch();
            Shadow_Word_Pain_Timer = new Timer(1000*14);
            return;
        }
        else if (Power_Word_Solace.KnownSpell && Power_Word_Solace.IsDistanceGood
                 && Power_Word_Solace.IsSpellUsable && _mySettings.UsePowerWordSolace
                 && ObjectManager.Me.ManaPercentage < 50)
        {
            Power_Word_Solace.Launch();
            return;
        }
        else if (Penance.IsSpellUsable && Penance.IsDistanceGood && Penance.KnownSpell
                 && _mySettings.UsePenance)
        {
            Penance.Launch();
            return;
        }
        else if (Holy_Fire.IsSpellUsable && Holy_Fire.IsDistanceGood && Holy_Fire.KnownSpell
                 && _mySettings.UseHolyFire)
        {
            Holy_Fire.Launch();
            return;
        }
        else if (Smite.IsSpellUsable && Smite.KnownSpell && Smite.IsDistanceGood
                 && _mySettings.UseSmite && Shadow_Word_Pain.TargetHaveBuff
                 && ObjectManager.GetNumberAttackPlayer() < 5)
        {
            Smite.Launch();
            return;
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

    #region Nested type: PriestDisciplineSettings

    [Serializable]
    public class PriestDisciplineSettings : Settings
    {
        public bool UseAlchFlask = true;
        public bool UseArcaneTorrent = true;
        public bool UseArchangel = true;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseCascade = true;
        public bool UseDesperatePrayer = true;
        public int UseDesperatePrayerAtPercentage = 65;
        public bool UseDivineStar = true;
        public bool UseEngGlove = true;
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
        public bool UseLifeblood = true;
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
        public bool UseTrinket = true;
        public bool UseVoidTendrils = true;
        public int UseVoidTendrilsAtPercentage = 35;
        public bool UseWarStomp = true;
        public int UseWarStompAtPercentage = 80;

        public PriestDisciplineSettings()
        {
            ConfigWinForm(new Point(500, 400), "Discipline Priest Settings");
            /* Professions and Racials */
            AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions and Racials");
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
            AddControlInWinForm("Use Trinket", "UseTrinket", "Game Settings");
            AddControlInWinForm("Use Engineering Gloves", "UseEngGlove", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
        }

        public static PriestDisciplineSettings CurrentSetting { get; set; }

        public static PriestDisciplineSettings GetSettings()
        {
            string CurrentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Priest_Discipline.xml";
            if (File.Exists(CurrentSettingsFile))
            {
                return
                    CurrentSetting = Load<PriestDisciplineSettings>(CurrentSettingsFile);
            }
            else
            {
                return new PriestDisciplineSettings();
            }
        }
    }

    #endregion
}

public class Priest_Holy
{
    private readonly string MoveBackward =
        Keybindings.GetKeyByAction(nManager.Wow.Enums.Keybindings.MOVEBACKWARD);

    private readonly string MoveForward =
        Keybindings.GetKeyByAction(nManager.Wow.Enums.Keybindings.MOVEFORWARD);

    private readonly PriestHolySettings _mySettings = PriestHolySettings.GetSettings();

    #region General Timers & Variables

    private Timer AlchFlask_Timer = new Timer(0);
    private Timer Engineering_Timer = new Timer(0);
    private Timer OnCD = new Timer(0);
    private Timer Trinket_Timer = new Timer(0);

    #endregion

    #region Professions and Racials

    private readonly Spell Alchemy = new Spell("Alchemy");
    private readonly Spell Arcane_Torrent = new Spell("Arcane Torrent");
    private readonly Spell Berserking = new Spell("Berserking");
    private readonly Spell Blood_Fury = new Spell("Blood Fury");
    private readonly Spell Engineering = new Spell("Engineering");
    private readonly Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    private readonly Spell Lifeblood = new Spell("Lifeblood");
    private readonly Spell Stoneform = new Spell("Stoneform");
    private readonly Spell War_Stomp = new Spell("War Stomp");

    #endregion

    #region Priest Buffs

    private readonly Spell Chakra_Chastise = new Spell("Chakra: Chastise");
    private readonly Spell Chakra_Sanctuary = new Spell("Chakra: Sanctuary");
    private readonly Spell Chakra_Serenity = new Spell("Chakra: Serenity");
    private readonly Spell Inner_Fire = new Spell("Inner Fire");
    private readonly Spell Inner_Will = new Spell("Inner Will");
    private readonly Spell Levitate = new Spell("Levitate");
    private readonly Spell Power_Word_Fortitude = new Spell("Power Word: Fortitude");
    private Timer Levitate_Timer = new Timer(0);

    #endregion

    #region Offensive Spell

    private readonly Spell Cascade = new Spell("Cascade");
    private readonly Spell Divine_Star = new Spell("Divine Star");
    private readonly Spell Halo = new Spell("Halo");
    private readonly Spell Holy_Word_Chastise = new Spell("Holy Word: Chastise");
    private readonly Spell Mind_Sear = new Spell("Mind Sear");
    private readonly Spell Power_Word_Solace = new Spell("Power Word: Solace");
    private readonly Spell Shadow_Word_Death = new Spell("Shadow Word: Death");
    private readonly Spell Shadow_Word_Pain = new Spell("Shadow Word: Pain");
    private readonly Spell Smite = new Spell("Smite");
    private Timer Shadow_Word_Pain_Timer = new Timer(0);

    #endregion

    #region Healing Cooldown

    private readonly Spell Divine_Hymn = new Spell("Divine Hymn");
    private readonly Spell Light_Well = new Spell("Light Well");
    private readonly Spell Power_Infusion = new Spell("Power Infusion");
    private readonly Spell Shadowfiend = new Spell("Shadowfiend");

    #endregion

    #region Defensive Cooldown

    private readonly Spell Guardian_Spirit = new Spell("Guardian Spirit");
    private readonly Spell Power_Word_Shield = new Spell("Power Word: Shield");
    private readonly Spell Psychic_Scream = new Spell("Psychic Scream");
    private readonly Spell Psyfiend = new Spell("Psyfiend");
    private readonly Spell Spectral_Guise = new Spell("Spectral Guise");
    private readonly Spell Void_Tendrils = new Spell("Void Tendrils");

    #endregion

    #region Healing Spell

    private readonly Spell Circle_of_Healing = new Spell("Circle of Healing");
    private readonly Spell Desperate_Prayer = new Spell("Desperate Prayer");
    private readonly Spell Flash_Heal = new Spell("Flash Heal");
    private readonly Spell Greater_Heal = new Spell("Greater Heal");
    private readonly Spell Heal_Spell = new Spell("Heal");
    private readonly Spell Holy_Fire = new Spell("Holy Fire");
    private readonly Spell Hymn_of_Hope = new Spell("Hymn of Hope");
    private readonly Spell Prayer_of_Healing = new Spell("Prayer of Healing");
    private readonly Spell Prayer_of_Mending = new Spell("Prayer of Mending");
    private readonly Spell Renew = new Spell("Renew");
    private Timer Renew_Timer = new Timer(0);

    #endregion

    public Priest_Holy()
    {
        Main.range = 30f;
        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            try
            {
                if (!ObjectManager.Me.IsDead)
                {
                    if (!ObjectManager.Me.IsMounted)
                    {
                        Buff_Levitate();
                        if (Fight.InFight && ObjectManager.Me.Target > 0)
                        {
                            if (ObjectManager.Me.Target != lastTarget &&
                                (Holy_Fire.IsDistanceGood || Shadow_Word_Pain.IsDistanceGood))
                            {
                                Pull();
                                lastTarget = ObjectManager.Me.Target;
                            }
                            else
                            {
                                if (ObjectManager.Target.GetDistance < 41)
                                    Combat();
                            }
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
            Thread.Sleep(150);
        }
    }

    private void Buff_Levitate()
    {
        if (!Fight.InFight && Levitate.KnownSpell && Levitate.IsSpellUsable && _mySettings.UseLevitate
            && (!Levitate.TargetHaveBuff || Levitate_Timer.IsReady))
        {
            Levitate.Launch();
            Levitate_Timer = new Timer(1000*60*9);
        }
    }

    private void Pull()
    {
        if (Holy_Fire.IsSpellUsable && Holy_Fire.KnownSpell && Holy_Fire.IsDistanceGood
            && _mySettings.UseHolyFire)
        {
            Holy_Fire.Launch();
            return;
        }
        else
        {
            if (Shadow_Word_Pain.IsSpellUsable && Shadow_Word_Pain.KnownSpell && Shadow_Word_Pain.IsDistanceGood
                && _mySettings.UseShadowWordPain)
            {
                Shadow_Word_Pain.Launch();
                Shadow_Word_Pain_Timer = new Timer(1000*14);
                return;
            }
        }
    }

    private void Combat()
    {
        AvoidMelee();
        if (OnCD.IsReady)
            Defense_Cycle();
        Heal();
        Buff();
        Healing_Burst();
        DPS_Cycle();
    }

    private void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (Power_Word_Fortitude.KnownSpell && Power_Word_Fortitude.IsSpellUsable &&
            !Power_Word_Fortitude.TargetHaveBuff && _mySettings.UsePowerWordFortitude)
        {
            Power_Word_Fortitude.Launch();
            return;
        }
        else if (Inner_Fire.KnownSpell && Inner_Fire.IsSpellUsable && !Inner_Fire.TargetHaveBuff
                 && _mySettings.UseInnerFire)
        {
            Inner_Fire.Launch();
            return;
        }
        else if (Inner_Will.KnownSpell && Inner_Will.IsSpellUsable && !Inner_Will.TargetHaveBuff
                 && !_mySettings.UseInnerFire && _mySettings.UseInnerWill)
        {
            Inner_Will.Launch();
            return;
        }
        else if (Chakra_Chastise.KnownSpell && Chakra_Chastise.IsSpellUsable && !Chakra_Chastise.TargetHaveBuff
                 && _mySettings.UseChakraChastise)
        {
            Chakra_Chastise.Launch();
            return;
        }
        else if (Chakra_Sanctuary.KnownSpell && Chakra_Sanctuary.IsSpellUsable && !Chakra_Sanctuary.TargetHaveBuff
                 && !_mySettings.UseChakraChastise && _mySettings.UseChakraSanctuary)
        {
            Chakra_Sanctuary.Launch();
            return;
        }
        else if (Chakra_Serenity.KnownSpell && Chakra_Serenity.IsSpellUsable && !Chakra_Serenity.TargetHaveBuff
                 && !_mySettings.UseChakraChastise && !_mySettings.UseChakraSanctuary && _mySettings.UseChakraSerenity)
        {
            Chakra_Serenity.Launch();
            return;
        }
        else
        {
            if (AlchFlask_Timer.IsReady && _mySettings.UseAlchFlask
                && ItemsManager.GetItemCountByIdLUA(75525) == 1)
            {
                Logging.WriteFight("Use Alchi Flask");
                Lua.RunMacroText("/use item:75525");
                AlchFlask_Timer = new Timer(1000*60*60*2);
            }
        }
    }

    private void AvoidMelee()
    {
        if (ObjectManager.Target.GetDistance < 2 && ObjectManager.Target.InCombat)
        {
            Logging.WriteFight("Too Close. Moving Back");
            var MaxTime_Timer = new Timer(1000*2);
            Keyboard.DownKey(Memory.WowProcess.MainWindowHandle, MoveBackward);
            while (ObjectManager.Target.GetDistance < 2 && ObjectManager.Target.InCombat && !MaxTime_Timer.IsReady)
                Thread.Sleep(300);
            Keyboard.UpKey(Memory.WowProcess.MainWindowHandle, MoveBackward);
            if (MaxTime_Timer.IsReady && ObjectManager.Target.GetDistance < 2 && ObjectManager.Target.InCombat)
            {
                Keyboard.DownKey(Memory.WowProcess.MainWindowHandle, MoveForward);
                Thread.Sleep(1000);
                Keyboard.UpKey(Memory.WowProcess.MainWindowHandle, MoveForward);
                MovementManager.Face(ObjectManager.Target.Position);
            }
        }
    }

    private void Defense_Cycle()
    {
        if (ObjectManager.Me.HealthPercent <= _mySettings.UsePsychicScreamAtPercentage && Psychic_Scream.IsSpellUsable &&
            Psychic_Scream.KnownSpell
            && _mySettings.UsePsychicScream)
        {
            Psychic_Scream.Launch();
            OnCD = new Timer(1000*8);
            return;
        }
        else if (ObjectManager.Me.HealthPercent <= _mySettings.UseGuardianSpiritAtPercentage && Guardian_Spirit.KnownSpell &&
                 Guardian_Spirit.IsSpellUsable
                 && _mySettings.UseGuardianSpirit)
        {
            Guardian_Spirit.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() >= 2 &&
                 ObjectManager.Me.HealthPercent <= _mySettings.UseVoidTendrilsAtPercentage &&
                 Void_Tendrils.IsSpellUsable && Void_Tendrils.KnownSpell && _mySettings.UseVoidTendrils)
        {
            Void_Tendrils.Launch();
            OnCD = new Timer(1000*10);
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() >= 2 &&
                 ObjectManager.Me.HealthPercent <= _mySettings.UsePsyfiendAtPercentage &&
                 Psyfiend.IsSpellUsable && Psyfiend.KnownSpell && _mySettings.UsePsyfiend)
        {
            Psyfiend.Launch();
            OnCD = new Timer(1000*10);
            return;
        }
        else if (ObjectManager.Me.HealthPercent <= _mySettings.UseSpectralGuiseAtPercentage &&
                 Spectral_Guise.IsSpellUsable && Spectral_Guise.KnownSpell
                 && _mySettings.UseSpectralGuise)
        {
            if (Renew.KnownSpell && Renew.IsSpellUsable && _mySettings.UseRenew)
            {
                Renew.Launch();
                Thread.Sleep(1500);
            }
            Spectral_Guise.Launch();
            OnCD = new Timer(1000*3);
            return;
        }
        else if (ObjectManager.Me.HealthPercent <= _mySettings.UseStoneformAtPercentage &&
                 Stoneform.IsSpellUsable && Stoneform.KnownSpell
                 && _mySettings.UseStoneform)
        {
            Stoneform.Launch();
            OnCD = new Timer(1000*8);
            return;
        }
        else
        {
            if (ObjectManager.Me.HealthPercent <= _mySettings.UseWarStompAtPercentage &&
                War_Stomp.IsSpellUsable && War_Stomp.KnownSpell
                && _mySettings.UseWarStomp)
            {
                War_Stomp.Launch();
                OnCD = new Timer(1000*2);
                return;
            }
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.HealthPercent <= _mySettings.UseFlashHealNonCombatAtPercentage && !Fight.InFight &&
            ObjectManager.GetNumberAttackPlayer() == 0
            && Flash_Heal.KnownSpell && Flash_Heal.IsSpellUsable && _mySettings.UseFlashHealNonCombat)
        {
            Flash_Heal.Launch(false);
            return;
        }
        else if (ObjectManager.Me.HealthPercent <= _mySettings.UseDivineHymnAtPercentage && Divine_Hymn.KnownSpell &&
                 Divine_Hymn.IsSpellUsable
                 && _mySettings.UseDivineHymn)
        {
            Divine_Hymn.Launch();
            return;
        }
        else if (!Fight.InFight && ObjectManager.Me.ManaPercentage <= _mySettings.UseHymnofHopeAtPercentage &&
                 Hymn_of_Hope.KnownSpell
                 && Hymn_of_Hope.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() == 0 &&
                 _mySettings.UseHymnofHope)
        {
            Hymn_of_Hope.Launch(false);
            return;
        }
        else if (ObjectManager.Me.HealthPercent <= _mySettings.UseDesperatePrayerAtPercentage &&
                 Desperate_Prayer.KnownSpell && Desperate_Prayer.IsSpellUsable
                 && _mySettings.UseDesperatePrayer)
        {
            Desperate_Prayer.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent <= _mySettings.UseFlashHealInCombatAtPercentage &&
                 Flash_Heal.KnownSpell && Flash_Heal.IsSpellUsable
                 && _mySettings.UseFlashHealInCombat)
        {
            Flash_Heal.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent <= _mySettings.UseGreaterHealAtPercentage &&
                 Greater_Heal.KnownSpell && Greater_Heal.IsSpellUsable
                 && _mySettings.UseGreaterHeal)
        {
            Greater_Heal.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent <= _mySettings.UseGiftoftheNaaruAtPercentage &&
                 Gift_of_the_Naaru.IsSpellUsable && Gift_of_the_Naaru.KnownSpell
                 && _mySettings.UseGiftoftheNaaru)
        {
            Gift_of_the_Naaru.Launch();
            return;
        }
        else if (Power_Word_Shield.KnownSpell && Power_Word_Shield.IsSpellUsable
                 && !Power_Word_Shield.TargetHaveBuff && _mySettings.UsePowerWordShield
                 && !ObjectManager.Me.HaveBuff(6788) &&
                 ObjectManager.Me.HealthPercent <= _mySettings.UsePowerWordShieldAtPercentage
                 && (ObjectManager.GetNumberAttackPlayer() > 0 || ObjectManager.Me.GetMove))
        {
            Power_Word_Shield.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent <= _mySettings.UsePrayerofHealingAtPercentage &&
                 Prayer_of_Healing.KnownSpell && Prayer_of_Healing.IsSpellUsable
                 && _mySettings.UsePrayerofHealing)
        {
            Prayer_of_Healing.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent <= _mySettings.UseCircleofHealingAtPercentage &&
                 Circle_of_Healing.KnownSpell && Circle_of_Healing.IsSpellUsable
                 && _mySettings.UseCircleofHealing)
        {
            SpellManager.CastSpellByIDAndPosition(34861, ObjectManager.Me.Position);
            return;
        }
        else if (ObjectManager.Me.HealthPercent <=
                 _mySettings.UsePrayerofMendingAtPercentage &&
                 Prayer_of_Mending.KnownSpell && Prayer_of_Mending.IsSpellUsable
                 && _mySettings.UsePrayerofMending)
        {
            Prayer_of_Mending.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent <= _mySettings.UseHealAtPercentage &&
                 Heal_Spell.KnownSpell && Heal_Spell.IsSpellUsable
                 && (_mySettings.UseHeal || !Greater_Heal.KnownSpell))
        {
            Heal_Spell.Launch();
            return;
        }
        else if (Light_Well.KnownSpell && Light_Well.IsSpellUsable &&
                 _mySettings.UseGlyphofLightspring
                 &&
                 ObjectManager.Me.HealthPercent <=
                 _mySettings.UseLightWellAtPercentage && _mySettings.UseLightWell)
        {
            SpellManager.CastSpellByIDAndPosition(724,
                                                  ObjectManager.Target
                                                               .Position);
            return;
        }
        else
        {
            if (Renew.KnownSpell && Renew.IsSpellUsable && !Renew.TargetHaveBuff &&
                ObjectManager.Me.HealthPercent <=
                _mySettings.UseRenewAtPercentage && _mySettings.UseRenew)
            {
                Renew.Launch();
                return;
            }
        }
    }

    private void Healing_Burst()
    {
        if (_mySettings.UseTrinket && Trinket_Timer.IsReady && ObjectManager.Target.GetDistance < 30)
        {
            Logging.WriteFight("Use Trinket 1.");
            Lua.RunMacroText("/use 13");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Logging.WriteFight("Use Trinket 2.");
            Lua.RunMacroText("/use 14");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Trinket_Timer = new Timer(1000*60*2);
        }
        else if (Berserking.IsSpellUsable && Berserking.KnownSpell && ObjectManager.Target.GetDistance < 30
                 && _mySettings.UseBerserking)
            Berserking.Launch();
        else if (Blood_Fury.IsSpellUsable && Blood_Fury.KnownSpell && ObjectManager.Target.GetDistance < 30
                 && _mySettings.UseBloodFury)
            Blood_Fury.Launch();
        else if (Lifeblood.IsSpellUsable && Lifeblood.KnownSpell && ObjectManager.Target.GetDistance < 30
                 && _mySettings.UseLifeblood)
            Lifeblood.Launch();
        else if (Engineering_Timer.IsReady && Engineering.KnownSpell && ObjectManager.Target.GetDistance < 30
                 && _mySettings.UseEngGlove)
        {
            Logging.WriteFight("Use Engineering Gloves.");
            Lua.RunMacroText("/use 10");
            Engineering_Timer = new Timer(1000*60);
        }
        else if (Power_Infusion.IsSpellUsable && Power_Infusion.KnownSpell
                 && _mySettings.UsePowerInfusion && ObjectManager.Target.GetDistance < 40)
        {
            Power_Infusion.Launch();
            return;
        }
        else
        {
            if (Shadowfiend.IsSpellUsable && Shadowfiend.KnownSpell && Shadowfiend.IsDistanceGood
                && _mySettings.UseShadowfiend)
            {
                Shadowfiend.Launch();
                return;
            }
        }
    }

    private void DPS_Cycle()
    {
        if (ObjectManager.Me.ManaPercentage < 80 && Arcane_Torrent.KnownSpell && Arcane_Torrent.IsSpellUsable
            && _mySettings.UseArcaneTorrent)
        {
            Arcane_Torrent.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 2 && Cascade.IsSpellUsable && Cascade.KnownSpell
                 && Cascade.IsDistanceGood && _mySettings.UseCascade)
        {
            Cascade.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 2 && Divine_Star.IsSpellUsable && Divine_Star.KnownSpell
                 && Divine_Star.IsDistanceGood && _mySettings.UseDivineStar)
        {
            Divine_Star.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 2 && Halo.IsSpellUsable && Halo.KnownSpell
                 && Halo.IsDistanceGood && _mySettings.UseHalo)
        {
            Halo.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 4 && Mind_Sear.IsSpellUsable && Mind_Sear.KnownSpell
                 && Mind_Sear.IsDistanceGood && !ObjectManager.Me.IsCast && _mySettings.UseMindSear)
        {
            Mind_Sear.Launch();
            return;
        }
        else if (Shadow_Word_Death.IsSpellUsable && Shadow_Word_Death.IsDistanceGood && Shadow_Word_Death.KnownSpell
                 && ObjectManager.Target.HealthPercent < 20 && _mySettings.UseShadowWordDeath)
        {
            Shadow_Word_Death.Launch();
            return;
        }
        else if (Shadow_Word_Pain.KnownSpell && Shadow_Word_Pain.IsSpellUsable
                 && Shadow_Word_Pain.IsDistanceGood && _mySettings.UseShadowWordPain
                 && (!Shadow_Word_Pain.TargetHaveBuff || Shadow_Word_Pain_Timer.IsReady))
        {
            Shadow_Word_Pain.Launch();
            Shadow_Word_Pain_Timer = new Timer(1000*14);
            return;
        }
        else if (Power_Word_Solace.KnownSpell && Power_Word_Solace.IsDistanceGood
                 && Power_Word_Solace.IsSpellUsable && _mySettings.UsePowerWordSolace
                 && ObjectManager.Me.ManaPercentage < 50)
        {
            Power_Word_Solace.Launch();
            return;
        }
        else if (Holy_Word_Chastise.IsSpellUsable && Holy_Word_Chastise.IsDistanceGood && Holy_Word_Chastise.KnownSpell
                 && _mySettings.UseHolyWordChastise)
        {
            Holy_Word_Chastise.Launch();
            return;
        }
        else if (Holy_Fire.IsSpellUsable && Holy_Fire.IsDistanceGood && Holy_Fire.KnownSpell
                 && _mySettings.UseHolyFire)
        {
            Holy_Fire.Launch();
            return;
        }
        else if (Smite.IsSpellUsable && Smite.KnownSpell && Smite.IsDistanceGood
                 && _mySettings.UseSmite && Shadow_Word_Pain.TargetHaveBuff
                 && ObjectManager.GetNumberAttackPlayer() < 5)
        {
            Smite.Launch();
            return;
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

    #region Nested type: PriestHolySettings

    [Serializable]
    public class PriestHolySettings : Settings
    {
        public bool UseAlchFlask = true;
        public bool UseArcaneTorrent = true;
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
        public bool UseEngGlove = true;
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
        public bool UseLifeblood = true;
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
        public bool UseTrinket = true;
        public bool UseVoidTendrils = true;
        public int UseVoidTendrilsAtPercentage = 35;
        public bool UseWarStomp = true;
        public int UseWarStompAtPercentage = 80;

        public PriestHolySettings()
        {
            ConfigWinForm(new Point(500, 400), "Holy Priest Settings");
            /* Professions and Racials */
            AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions and Racials");
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
            AddControlInWinForm("Use Trinket", "UseTrinket", "Game Settings");
            AddControlInWinForm("Use Engineering Gloves", "UseEngGlove", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
            AddControlInWinForm("Use Glyph of Lightspring", "UseGlyphofLightspring", "Game Settings");
        }

        public static PriestHolySettings CurrentSetting { get; set; }

        public static PriestHolySettings GetSettings()
        {
            string CurrentSettingsFile = Application.StartupPath + "\\CombatClasses\\Settings\\Priest_Holy.xml";
            if (File.Exists(CurrentSettingsFile))
            {
                return
                    CurrentSetting = Load<PriestHolySettings>(CurrentSettingsFile);
            }
            else
            {
                return new PriestHolySettings();
            }
        }
    }

    #endregion
}

#endregion