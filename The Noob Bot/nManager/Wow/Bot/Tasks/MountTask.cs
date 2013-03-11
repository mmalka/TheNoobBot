using System;
using System.Threading;
using System.Windows.Forms;
using nManager.Helpful;
using nManager.Wow.Helpers;
using nManager.Wow.Class;
using Timer = nManager.Helpful.Timer;

namespace nManager.Wow.Bot.Tasks
{
    public enum MountCapacity
    {
        Feet = 0,
        Swimm = 0,
        Ground = 1,
        Fly = 2,
    }

    public class MountTask
    {
        private static int _nbTry;
        private static int _noMountsInSettings;
        private static string _localizedAbysalMountName = string.Empty;
        private static bool startupCheck = true;

        public static MountCapacity GetMountCapacity()
        {
            string aquaMount = nManagerSetting.CurrentSetting.AquaticMountName;
            string groundMount = nManagerSetting.CurrentSetting.GroundMountName;
            string flyMount = nManagerSetting.CurrentSetting.FlyingMountName;

            if (startupCheck)
            {
                // 1st Check if mounts in general settings exist
                if (ObjectManager.ObjectManager.Me.Level >= 16 && groundMount != string.Empty && !SpellManager.ExistMountLUA(groundMount) &&
                    !SpellManager.ExistSpellBookLUA(groundMount))
                {
                    MessageBox.Show(Translate.Get(Translate.Id.This_mount_does_not_exist) + ": " + groundMount);
                    groundMount = string.Empty;
                }
                if (ObjectManager.ObjectManager.Me.Level >= 18 && aquaMount != string.Empty && !SpellManager.ExistMountLUA(aquaMount) &&
                    !SpellManager.ExistSpellBookLUA(aquaMount))
                {
                    MessageBox.Show(Translate.Get(Translate.Id.This_mount_does_not_exist) + ": " + aquaMount);
                    aquaMount = string.Empty;
                }
                if (ObjectManager.ObjectManager.Me.Level >= 58 && flyMount != string.Empty && !SpellManager.ExistMountLUA(flyMount) &&
                    !SpellManager.ExistSpellBookLUA(flyMount))
                {
                    MessageBox.Show(Translate.Get(Translate.Id.This_mount_does_not_exist) + ": " + flyMount);
                    flyMount = string.Empty;
                }
                if (ObjectManager.ObjectManager.Me.Level >= 60 && aquaMount != string.Empty && _localizedAbysalMountName == string.Empty)
                    _localizedAbysalMountName = Helpers.SpellManager.SpellListManager.SpellNameByIdExperimental(75207);

                startupCheck = false;
            }
            if (ObjectManager.ObjectManager.Me.Level < 16 || (groundMount == string.Empty && flyMount == string.Empty && aquaMount == string.Empty))
            {
                if (ObjectManager.ObjectManager.Me.Level >= 16 && _noMountsInSettings != 1)
                {
                    MessageBox.Show(Translate.Get(Translate.Id.No_mounts_in_settings));
                    _noMountsInSettings++;
                }
                return MountCapacity.Feet;
            }

            // Wherever we are if we have an aquatic mount and are swimming
            if (ObjectManager.ObjectManager.Me.Level >= 18 && Usefuls.IsSwimming && aquaMount != string.Empty)
            {
                // We are in Vashjir and the Abyssal Seahorse is selected
                if (aquaMount == _localizedAbysalMountName &&
                    (Usefuls.AreaId == 5146 || Usefuls.AreaId == 4815 ||
                     Usefuls.AreaId == 5145 || Usefuls.AreaId == 5144))
                {
                    return MountCapacity.Swimm;
                }
                else if (aquaMount != _localizedAbysalMountName)
                {
                    return MountCapacity.Swimm;
                }
            }

            if (ObjectManager.ObjectManager.Me.Level >= 58 && Usefuls.IsOutdoors)
            {
                if (flyMount != string.Empty && Usefuls.IsFlyableArea)
                {
                    Enums.ContinentId cont = (Enums.ContinentId) Usefuls.ContinentId;

                    // We are in Pandaria and with "Wisdom of the Four Winds" aura
                    Spell Wisdom4Winds = new Spell(115913);
                    if (cont == Enums.ContinentId.Pandaria &&
                        Wisdom4Winds.KnownSpell)
                        return MountCapacity.Fly;

                    // We are in Northfend with "Cold Weather Flying" aura
                    Spell ColdWeather = new Spell(54197);
                    if (cont == Enums.ContinentId.Northrend && ColdWeather.KnownSpell)
                        return MountCapacity.Fly;

                    // We are in Azeroth/Kalimdor/Deptholm with "Flight Master's License" aura
                    Spell FlightMasterLicense = new Spell(90267);
                    if ((cont == Enums.ContinentId.Azeroth || cont == Enums.ContinentId.Kalimdor ||
                         cont == Enums.ContinentId.Maelstrom) &&
                        FlightMasterLicense.KnownSpell)
                        return MountCapacity.Fly;

                    // We are in Outland and Expert Flying or better
                    Spell ExpertRider = new Spell(34090);
                    Spell ArtisanRider = new Spell(34091);
                    Spell MasterRider = new Spell(90265);
                    if (cont == Enums.ContinentId.Outland &&
                        (ExpertRider.KnownSpell || ArtisanRider.KnownSpell || MasterRider.KnownSpell))
                        return MountCapacity.Fly;

                    // More work to be done with spell 130487 = "Cloud Serpent Riding"
                }
                if (ObjectManager.ObjectManager.Me.Level >= 16 && groundMount != string.Empty)
                    return MountCapacity.Ground;
            }
            return MountCapacity.Feet;
        }

        public static bool onGroundMount()
        {
            return SpellManager.HaveBuffLua(nManagerSetting.CurrentSetting.GroundMountName);
        }

        public static bool onFlyMount()
        {
            return SpellManager.HaveBuffLua(nManagerSetting.CurrentSetting.FlyingMountName);
        }

        public static bool onAquaticMount()
        {
            return SpellManager.HaveBuffLua(nManagerSetting.CurrentSetting.AquaticMountName);
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
                if (ObjectManager.ObjectManager.Me.IsMounted && !onGroundMount())
                    DismountMount(stopMove);

                if (nManagerSetting.CurrentSetting.GroundMountName != "" && !ObjectManager.ObjectManager.Me.IsMounted &&
                    nManagerSetting.CurrentSetting.UseGroundMount && Usefuls.IsOutdoors)
                {
                    if (stopMove)
                        MovementManager.StopMove();
                    else
                        MovementManager.StopMoveTo();
                    Logging.Write("Mounting gound mount " + nManagerSetting.CurrentSetting.GroundMountName);

                    Thread.Sleep(250);
                    SpellManager.CastSpellByNameLUA(nManagerSetting.CurrentSetting.GroundMountName);
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
                if (ObjectManager.ObjectManager.Me.IsMounted && !onAquaticMount())
                    DismountMount(stopMove);

                if (!ObjectManager.ObjectManager.Me.IsMounted)
                {
                    if (stopMove)
                        MovementManager.StopMove();
                    else
                        MovementManager.StopMoveTo();
                    Logging.Write("Mounting aquatic mount " + nManagerSetting.CurrentSetting.AquaticMountName);

                    Thread.Sleep(250);
                    SpellManager.CastSpellByNameLUA(nManagerSetting.CurrentSetting.AquaticMountName);
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
                if (ObjectManager.ObjectManager.Me.IsMounted && !onFlyMount())
                    DismountMount(stopMove);

                if (!ObjectManager.ObjectManager.Me.IsMounted)
                {
                    if (stopMove)
                        MovementManager.StopMove();
                    else
                        MovementManager.StopMoveTo();
                    Thread.Sleep(100);
                    if (Usefuls.IsSwimming)
                    {
                        Logging.WriteNavigator("Going out of water");
                        MovementsAction.Ascend(true);
                        Thread.Sleep(1750);
                        MovementsAction.Ascend(false);
                    }
                    if (Usefuls.IsOutdoors)
                    {
                        if (stopMove)
                            MovementManager.StopMove();
                        else
                            MovementManager.StopMoveTo();
                        if (ObjectManager.ObjectManager.Me.InCombat)
                        {
                            return;
                        }
                        Thread.Sleep(250);
                        Logging.Write("Mounting fly mount " + nManagerSetting.CurrentSetting.FlyingMountName);
                        SpellManager.CastSpellByNameLUA(nManagerSetting.CurrentSetting.FlyingMountName);
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
                                var t = new Timer(1000); // 1 sec
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
            var t = new Timer(950);
            while (!Usefuls.IsFlying && !t.IsReady)
            {
                Thread.Sleep(50);
            }
            Thread.Sleep(150);
            MovementsAction.Ascend(false);
        }

        public static void Land()
        {
            Logging.WriteNavigator("Landing in progress.");
            MovementsAction.Descend(true);
            var t = new Timer(15000);
            while (Usefuls.IsFlying && !t.IsReady)
            {
                float z0 = ObjectManager.ObjectManager.Me.Position.Z;
                Thread.Sleep(100);
                if (z0 == ObjectManager.ObjectManager.Me.Position.Z)
                    t.ForceReady();
            }
            Thread.Sleep(150);
            MovementsAction.Descend(false);
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
                        if (Usefuls.IsFlying)
                            Land();
                        Usefuls.DisMount();
                        Thread.Sleep(300 + Usefuls.Latency);
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