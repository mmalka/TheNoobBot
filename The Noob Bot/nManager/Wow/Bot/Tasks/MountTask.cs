using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using nManager.Helpful;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;
using nManager.Wow.Class;
using Timer = nManager.Helpful.Timer;

namespace nManager.Wow.Bot.Tasks
{
    public enum MountCapacity
    {
        Feet,
        Ground,
        Swimm,
        Fly,
    }

    public class MountTask
    {
        private static int _nbTry;
        private static int _noMountsInSettings;
        private static string _localizedAbysalMountName = string.Empty;
        private static bool _startupCheck = true;
        private static bool _wisdom4Winds;
        private static bool _coldWeather;
        private static bool _flightMasterLicense;
        private static bool _dreaneorFly;
        private static Spell _spellAquaMount = new Spell(0);
        private static Spell _spellGroundMount = new Spell(0);
        private static Spell _spellFlyMount = new Spell(0);
        private static string _aquaMount;
        private static string _groundMount;
        private static string _flyMount;
        public static bool SettingsHasChanged;
        private static Timer dismountTimer = new Timer(0);

        public static MountCapacity GetMountCapacity()
        {
            if (_startupCheck || SettingsHasChanged)
            {
                // 1st Check if mounts in general settings exist
                _aquaMount = nManagerSetting.CurrentSetting.AquaticMountName;
                _groundMount = nManagerSetting.CurrentSetting.GroundMountName;
                _flyMount = nManagerSetting.CurrentSetting.FlyingMountName;
                if (!string.IsNullOrEmpty(_aquaMount.Trim()))
                    _spellAquaMount = new Spell(_aquaMount);
                if (!string.IsNullOrEmpty(_groundMount.Trim()))
                    _spellGroundMount = new Spell(_groundMount);
                if (!string.IsNullOrEmpty(_flyMount.Trim()))
                    _spellFlyMount = new Spell(_flyMount);

                if (ObjectManager.ObjectManager.Me.Level >= 16 && _groundMount != string.Empty && nManagerSetting.CurrentSetting.UseGroundMount && !_spellGroundMount.KnownSpell)
                {
                    Others.ShowMessageBox(Translate.Get(Translate.Id.ThisGroundMountDoesNotExist) + _groundMount);
                    _groundMount = string.Empty;
                }
                if (ObjectManager.ObjectManager.Me.Level >= 18 && _aquaMount != string.Empty && !_spellAquaMount.KnownSpell)
                {
                    Others.ShowMessageBox(Translate.Get(Translate.Id.ThisAquaticMountDoesNotExist) + _aquaMount);
                    _aquaMount = string.Empty;
                }
                if (ObjectManager.ObjectManager.Me.Level >= 58 && _flyMount != string.Empty && !_spellFlyMount.KnownSpell)
                {
                    Others.ShowMessageBox(Translate.Get(Translate.Id.ThisFlyingMountDoesNotExist) + _flyMount);
                    _flyMount = string.Empty;
                }
                if (ObjectManager.ObjectManager.Me.Level >= 60 && _aquaMount != string.Empty && _localizedAbysalMountName == string.Empty)
                    _localizedAbysalMountName = SpellManager.GetSpellInfo(75207).Name;

                Spell wisdom4Winds = new Spell(115913);
                _wisdom4Winds = wisdom4Winds.KnownSpell;
                Spell coldWeather = new Spell(54197);
                _coldWeather = coldWeather.KnownSpell;
                Spell flightMasterLicense = new Spell(90267);
                _flightMasterLicense = flightMasterLicense.KnownSpell;
                _dreaneorFly = ObjectManager.ObjectManager.Me.Level >= 90 && Usefuls.IsCompletedAchievement(10018, false); // Draenor Pathfinder (account wide)

                _startupCheck = false;
                SettingsHasChanged = false;
            }
            if (ObjectManager.ObjectManager.Me.InCombatBlizzard)
                return MountCapacity.Feet;
            if (_groundMount == string.Empty && _flyMount == string.Empty && _aquaMount == string.Empty)
            {
                if (((ObjectManager.ObjectManager.Me.Level >= 20 && Skill.GetValue(SkillLine.Riding) > 0) || (ObjectManager.ObjectManager.Me.WowClass == WoWClass.Druid && ObjectManager.ObjectManager.Me.Level >= 16)) &&
                    _noMountsInSettings != 1)
                {
                    MessageBox.Show(Translate.Get(Translate.Id.No_mounts_in_settings));
                    _noMountsInSettings++;
                    return MountCapacity.Feet;
                }
            }

            if (ObjectManager.ObjectManager.Me.HaveBuff(ObjectManager.WoWUnit.CombatMount))
                return MountCapacity.Feet;

            // Wherever we are if we have an aquatic mount and are swimming
            if (((ObjectManager.ObjectManager.Me.Level >= 20 && Skill.GetValue(SkillLine.Riding) > 0) || (ObjectManager.ObjectManager.Me.WowClass == WoWClass.Druid && ObjectManager.ObjectManager.Me.Level >= 18)) &&
                Usefuls.IsSwimming &&
                _aquaMount != string.Empty)
            {
                // The Abyssal Seahorse is selected
                if (_aquaMount == _localizedAbysalMountName)
                {
                    // We are in Vashjir
                    //  Kelp'thar Forest       || Abyssal Depths         || Shimmering Expanse
                    if (Usefuls.AreaId == 4815 || Usefuls.AreaId == 5145 || Usefuls.AreaId == 5144)
                    {
                        return MountCapacity.Swimm;
                    }
                    // We are NOT in Vashjir
                    return MountCapacity.Feet;
                }
                else // (_aquaMount != _localizedAbysalMountName)
                {
                    return MountCapacity.Swimm;
                }
            }

            if (Usefuls.IsOutdoors)
            {
                if (ObjectManager.ObjectManager.Me.HaveBuff(178256)) // Panicked Rush
                    return MountCapacity.Feet; // Tannan Jungle Intro to leave to the docks.

                if ((ObjectManager.ObjectManager.Me.Level >= 60 || (ObjectManager.ObjectManager.Me.WowClass == WoWClass.Druid && ObjectManager.ObjectManager.Me.Level >= 58)) && _flyMount != string.Empty &&
                    Usefuls.IsFlyableArea)
                {
                    ContinentId cont = (ContinentId) Usefuls.ContinentId;

                    // We are in Draenor and we have the achievement
                    if (_dreaneorFly && (cont == ContinentId.Draenor || Usefuls.ContinentNameMpqByContinentId(Usefuls.ContinentId) == "TanaanJungle"))
                        return MountCapacity.Fly;

                    // We are in Pandaria and with "Wisdom of the Four Winds" aura
                    if (_wisdom4Winds && cont == ContinentId.Pandaria)
                        return MountCapacity.Fly;

                    // We are in Northfend with "Cold Weather Flying" aura
                    if (_coldWeather && cont == ContinentId.Northrend)
                        return MountCapacity.Fly;

                    // We are in Azeroth/Kalimdor/Deptholm with "Flight Master's License" aura
                    if (_flightMasterLicense &&
                        (cont == ContinentId.Azeroth || cont == ContinentId.Kalimdor ||
                         cont == ContinentId.Maelstrom))
                        return MountCapacity.Fly;

                    // We are in Outland and Expert Flying or better
                    Spell expertRider = new Spell(34090);
                    Spell artisanRider = new Spell(34091);
                    Spell masterRider = new Spell(90265);
                    if (cont == ContinentId.Outland &&
                        (expertRider.KnownSpell || artisanRider.KnownSpell || masterRider.KnownSpell))
                        return MountCapacity.Fly;

                    // More work to be done with spell 130487 = "Cloud Serpent Riding"
                }
                if (((ObjectManager.ObjectManager.Me.Level >= 20 && Skill.GetValue(SkillLine.Riding) > 0) || (ObjectManager.ObjectManager.Me.WowClass == WoWClass.Druid && ObjectManager.ObjectManager.Me.Level >= 16) ||
                     _spellGroundMount.KnownSpell && _spellGroundMount.Id == 179245 || _spellGroundMount.Id == 179244) && _groundMount != string.Empty)
                    return MountCapacity.Ground;
            }
            return MountCapacity.Feet;
        }

        public static bool OnGroundMount()
        {
            return _spellGroundMount.HaveBuff;
        }

        public static bool OnFlyMount()
        {
            return _spellFlyMount.HaveBuff;
        }

        public static bool OnAquaticMount()
        {
            return _spellAquaMount.HaveBuff;
        }

        public static void Mount(bool stopMove = true)
        {
            switch (GetMountCapacity())
            {
                case MountCapacity.Fly:
                    MountingFlyingMount(stopMove);
                    break;
                case MountCapacity.Swimm:
                    MountingAquaticMount(stopMove);
                    break;
                case MountCapacity.Ground:
                    MountingGroundMount(stopMove);
                    break;
                default:
                    return;
            }
        }

        public static void MountingGroundMount(bool stopMove = true)
        {
            try
            {
                if (ObjectManager.ObjectManager.Me.IsMounted && !OnGroundMount())
                    DismountMount(stopMove);

                if (nManagerSetting.CurrentSetting.GroundMountName != "" && !ObjectManager.ObjectManager.Me.IsMounted &&
                    nManagerSetting.CurrentSetting.UseGroundMount && Usefuls.IsOutdoors)
                {
                    if (stopMove)
                        MovementManager.StopMove();
                    else
                        MovementManager.StopMoveTo();
                    Logging.Write("Mounting gound mount " + _spellGroundMount.NameInGame);

                    Thread.Sleep(250);
                    SpellManager.CastSpellByNameLUA(_spellGroundMount.NameInGame);
                    Thread.Sleep(500 + Usefuls.Latency);
                    while (ObjectManager.ObjectManager.Me.IsCast)
                    {
                        Thread.Sleep(50);
                    }
                    Thread.Sleep(500);

                    if (!ObjectManager.ObjectManager.Me.IsMounted && Products.Products.IsStarted)
                    {
                        if (_nbTry >= 2)
                        {
                            MovementManager.UnStuck();
                        }
                        _nbTry++;
                    }
                    else
                    {
                        _nbTry = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError("MountTask > MountingGroundMount(): " + ex);
            }
        }

        public static void MountingAquaticMount(bool stopMove = true)
        {
            try
            {
                // don't check for druids or people always using the same mount at the cost of lower speed (turtle)
                if (nManagerSetting.CurrentSetting.AquaticMountName != nManagerSetting.CurrentSetting.GroundMountName &&
                    nManagerSetting.CurrentSetting.AquaticMountName != nManagerSetting.CurrentSetting.FlyingMountName)
                    MovementManager.SwimmingMountRecentlyTimer = new Timer(120000); // prevent move to location from checking !Swimming && OnAquaticMount for 120
                if (ObjectManager.ObjectManager.Me.IsMounted && !OnAquaticMount())
                    DismountMount(stopMove);

                if (!ObjectManager.ObjectManager.Me.IsMounted)
                {
                    if (stopMove)
                        MovementManager.StopMove();
                    else
                        MovementManager.StopMoveTo();
                    Logging.Write("Mounting aquatic mount " + _spellAquaMount.NameInGame);

                    Thread.Sleep(250);
                    SpellManager.CastSpellByNameLUA(_spellAquaMount.NameInGame);
                    Thread.Sleep(500 + Usefuls.Latency);
                    while (ObjectManager.ObjectManager.Me.IsCast)
                    {
                        Thread.Sleep(50);
                    }
                    Thread.Sleep(500);

                    if (!ObjectManager.ObjectManager.Me.IsMounted && Products.Products.IsStarted)
                    {
                        if (_nbTry >= 2)
                        {
                            MovementManager.UnStuck();
                        }
                        _nbTry++;
                    }
                    else
                    {
                        _nbTry = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError("MountTask > MountingAquaticMount(bool stopMove = true): " + ex);
            }
        }

        private static int tryMounting;

        public static void MountingFlyingMount(bool stopMove = true)
        {
            try
            {
                if (ObjectManager.ObjectManager.Me.IsMounted && (!OnFlyMount() && !ObjectManager.ObjectManager.Me.IsDeadMe))
                    DismountMount(stopMove);

                if (!ObjectManager.ObjectManager.Me.IsMounted)
                {
                    if (stopMove)
                        MovementManager.StopMove();
                    else
                        MovementManager.StopMoveTo();
                    Thread.Sleep(100);
                    if (Usefuls.IsSwimming)
                        Logging.WriteNavigator("Going out of water");
                    while (Usefuls.IsSwimming)
                    {
                        MovementsAction.Ascend(true);
                        Thread.Sleep(500);
                        MovementsAction.Ascend(false);
                    }
                    if (Usefuls.IsOutdoors && !ObjectManager.ObjectManager.Me.HaveBuff(SpellManager.DruidMountId()))
                    {
                        if (stopMove)
                            MovementManager.StopMove();
                        else
                            MovementManager.StopMoveTo();
                        if (ObjectManager.ObjectManager.Me.InCombat)
                        {
                            return;
                        }
                        while (ObjectManager.ObjectManager.Me.GetMove)
                            Thread.Sleep(50 + Usefuls.Latency); // to stop moving/falling
                        Logging.Write("Mounting fly mount " + _spellFlyMount.NameInGame);
                        SpellManager.CastSpellByNameLUA(_spellFlyMount.NameInGame);
                        if (ObjectManager.ObjectManager.Me.InCombat)
                        {
                            return;
                        }
                        Thread.Sleep(500 + Usefuls.Latency);
                        while (ObjectManager.ObjectManager.Me.IsCast)
                        {
                            Thread.Sleep(50);
                        }
                        if (ObjectManager.ObjectManager.Me.InCombat)
                        {
                            return;
                        }
                        Thread.Sleep(500);
                    }
                    if (!ObjectManager.ObjectManager.Me.IsMounted && Products.Products.IsStarted)
                    {
                        if (ObjectManager.ObjectManager.Me.InCombat)
                        {
                            return;
                        }
                        Thread.Sleep(500);
                        if (ObjectManager.ObjectManager.Me.InCombat)
                        {
                            return;
                        }
                        if (!ObjectManager.ObjectManager.Me.IsMounted && Products.Products.IsStarted)
                        {
                            if (tryMounting >= 3)
                            {
                                Logging.Write("Mounting failed");
                                MovementManager.UnStuckFly();
                            }
                            else
                            {
                                Timer t = new Timer(1000); // 1 sec
                                while (!t.IsReady && !Usefuls.IsOutdoors)
                                {
                                    if (ObjectManager.ObjectManager.Me.InCombat)
                                        return;
                                    Thread.Sleep(200);
                                }
                                tryMounting++;
                            }
                        }
                    }
                    if (ObjectManager.ObjectManager.Me.IsMounted)
                    {
                        tryMounting = 0;
                        Thread.Sleep(100);
                        Takeoff();
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError("MountTask > MountingFlyingMount(): " + ex);
            }
        }

        public static void Takeoff()
        {
            Logging.WriteNavigator("Take-off in progress.");
            MovementsAction.Ascend(true);
            Timer t = new Timer(950);
            while (!Usefuls.IsFlying && !t.IsReady)
            {
                Thread.Sleep(50);
            }
            Thread.Sleep(150);
            MovementsAction.Ascend(false);
        }

        public static void Land(bool useLuaToLand = false)
        {
            Logging.WriteNavigator("Landing in progress.");
            MovementsAction.Descend(true, false, useLuaToLand);
            Timer t = new Timer(60000);
            while (Usefuls.IsFlying && !t.IsReady)
            {
                float z0 = ObjectManager.ObjectManager.Me.Position.Z;
                Thread.Sleep(1000);
                if (z0 == ObjectManager.ObjectManager.Me.Position.Z)
                    t.ForceReady();
            }
            Logging.WriteDebug("Still flying after 1min of landing.");
            Thread.Sleep(150);
            MovementsAction.Descend(false, false, useLuaToLand);
        }

        public static bool JustDismounted()
        {
            return !dismountTimer.IsReady;
        }

        public static void DismountMount(bool stopMove = true)
        {
            try
            {
                if (Products.Products.IsStarted)
                {
                    if (stopMove)
                        MovementManager.StopMove();
                    else
                        MovementManager.StopMoveTo();
                    Thread.Sleep(200);

                    if (ObjectManager.ObjectManager.Me.IsMounted)
                    {
                        Logging.Write("Dismount in progress.");
                        bool flying = Usefuls.IsFlying;
                        if (flying)
                            Land();
                        Usefuls.DisMount();
                        if (flying)
                            dismountTimer = new Timer(5000);
                        Thread.Sleep(200 + Usefuls.Latency);
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError("MountTask > DismountMount(): " + ex);
            }
        }
    }
}