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
        Feet,
        Ground,
        Swimm,
        Fly,
    }

    public class MountTask
    {
        private static int _nbTry;

        public static MountCapacity GetMountCapacity()
        {
            string aquaMount = nManagerSetting.CurrentSetting.AquaticMountName;
            string groundMount = nManagerSetting.CurrentSetting.GroundMountName;
            string flyMount = nManagerSetting.CurrentSetting.FlyingMountName;

            // 1st Check if mounts in general settings exist
            if (groundMount != string.Empty && !SpellManager.ExistMountLUA(groundMount) && !SpellManager.ExistSpellBookLUA(groundMount))
            {
                MessageBox.Show(Translate.Get(Translate.Id.This_mount_does_not_exist) + ": " + groundMount);
                groundMount = string.Empty;
            }
            if (aquaMount != string.Empty && !SpellManager.ExistMountLUA(aquaMount) && !SpellManager.ExistSpellBookLUA(aquaMount))
            {
                MessageBox.Show(Translate.Get(Translate.Id.This_mount_does_not_exist) + ": " + aquaMount);
                aquaMount = string.Empty;
            }
            if (flyMount != string.Empty && !SpellManager.ExistMountLUA(flyMount) && !SpellManager.ExistSpellBookLUA(flyMount))
            {
                MessageBox.Show(Translate.Get(Translate.Id.This_mount_does_not_exist) + ": " + flyMount);
                flyMount = string.Empty;
            }
            System.Collections.Generic.List<uint> aquaMountId = SpellManager.SpellListManager.SpellIdByName(aquaMount);

            // The Abyssal Seahorse is selected
            if (aquaMount != string.Empty)
            {   // We are in Vashjir
                if (aquaMountId.Count > 0 && aquaMountId[0] == 75207 &&
                    (Usefuls.AreaId == 5146 || Usefuls.AreaId == 4815 ||
                     Usefuls.AreaId == 5145 || Usefuls.AreaId == 5144))
                {   // extra verification for caves and boats
                    Spell abyssal = new Spell("Abyssal Seahorse");
                    if (abyssal.KnownSpell && abyssal.IsSpellUsable)
                        return MountCapacity.Swimm;
                }
            }

            // Wherever we are if we have an aquatic mount and are swimming
            if (aquaMount != string.Empty && Usefuls.IsSwimming &&
                !(aquaMountId.Count > 0 && aquaMountId[0] == 75207))
                return MountCapacity.Swimm;

            nManager.Wow.Enums.ContinentId cont = (nManager.Wow.Enums.ContinentId)Usefuls.ContinentId;

            if (Usefuls.IsOutdoors)
            {
                if (flyMount != string.Empty && Usefuls.IsFlyableArea)
                {
                    // We are in Outland and Expert Flying or better
                    Spell ExpertRider = new Spell(34090);
                    Spell ArtisanRider = new Spell(34091);
                    Spell MasterRider = new Spell(90265);
                    if (cont == Enums.ContinentId.Outland &&
                        (ExpertRider.KnownSpell || ArtisanRider.KnownSpell || MasterRider.KnownSpell))
                        return MountCapacity.Fly;

                    // We are in Northfend with "Cold Weather Flying" aura
                    Spell ColdWeather = new Spell(54197);
                    if (cont == Enums.ContinentId.Northrend && ColdWeather.KnownSpell)
                        return MountCapacity.Fly;

                    // We are in Azeroth/Kalimdor/Deptholm with "Flight Master's License" aura
                    Spell FlightMasterLicense = new Spell(90267);
                    if ((cont == Enums.ContinentId.Azeroth || cont == Enums.ContinentId.Kalimdor || cont == Enums.ContinentId.Maelstrom) &&
                        FlightMasterLicense.KnownSpell)
                        return MountCapacity.Fly;

                    // We are in Pandaria and with "Wisdom of the Four Winds" aura
                    Spell Wisdom4Winds = new Spell(115913);
                    if (cont == Enums.ContinentId.Pandaria &&
                        Wisdom4Winds.KnownSpell)
                        return MountCapacity.Fly;

                    // More work to be done with spell 130487 = "Cloud Serpent Riding"
                }
                if (groundMount != string.Empty)
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

                if (!ObjectManager.ObjectManager.Me.IsMounted && nManagerSetting.CurrentSetting.useGroundMount && Usefuls.IsOutdoors)
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

        private static Timer TimerMount;
        private static int tryMounting;

        public static void MountingFlyingMount(bool stopMove = true)
        {
            try
            {
                if (TimerMount != null)
                    if (!TimerMount.IsReady)
                        return;
                TimerMount = new Timer(1*300);

                if (ObjectManager.ObjectManager.Me.IsMounted && !onFlyMount() && !Usefuls.IsFlying)
                    DismountMount(stopMove);

                if (!ObjectManager.ObjectManager.Me.IsMounted)
                {
                    if (stopMove)
                        MovementManager.StopMove();
                    else
                        MovementManager.StopMoveTo();
                    Logging.Write("Mounting fly mount " + nManagerSetting.CurrentSetting.FlyingMountName);
                    Thread.Sleep(100);
                    if (Usefuls.IsSwimming)
                    {
                        Keybindings.DownKeybindings(Enums.Keybindings.JUMP);
                        Thread.Sleep(1750);
                        Keybindings.UpKeybindings(Enums.Keybindings.JUMP);
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
                        SpellManager.CastSpellByNameLUA(nManagerSetting.CurrentSetting.FlyingMountName);
                        if (ObjectManager.ObjectManager.Me.InCombat)
                        {
                            return;
                        }
                        Thread.Sleep(500 + Usefuls.Latency);
                        while (ObjectManager.ObjectManager.Me.IsCast && !ObjectManager.ObjectManager.Me.InCombat)
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
                            if (ObjectManager.ObjectManager.Me.InCombat)
                            {
                                return;
                            }
                            if (tryMounting >= 3)
                            {
                                Logging.Write("Mounting failed");
                                MovementManager.UnStuckFly();
                            }
                            else
                            {
                                var t = new Timer(1000);
                                while (!t.IsReady &&
                                       !Usefuls.IsOutdoors)
                                {
                                    if (ObjectManager.ObjectManager.Me.InCombat)
                                        return;
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
            Keybindings.DownKeybindings(Enums.Keybindings.JUMP);
            var t = new Timer(850);
            while (!Usefuls.IsFlying && !t.IsReady)
            {
                Thread.Sleep(50);
            }
            Thread.Sleep(150);
            Keybindings.UpKeybindings(Enums.Keybindings.JUMP);
        }

        public static void Land()
        {
            Keybindings.DownKeybindings(Enums.Keybindings.SITORSTAND);
            var t = new Timer(15000);
            while (Usefuls.IsFlying && !t.IsReady)
            {
                Thread.Sleep(50);
            }
            Thread.Sleep(150);
            Keybindings.UpKeybindings(Enums.Keybindings.SITORSTAND);
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
                        Logging.Write("Dismount");
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
