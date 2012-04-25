using System;
using System.Threading;
using System.Windows.Forms;
using nManager.Helpful;
using nManager.Wow.Helpers;
using nManager.Wow.Class;
using Timer = nManager.Helpful.Timer;

namespace nManager.Wow.Bot.Tasks
{
    public class MountTask
    {
        private static int _nbTry;

        public static void MountingGroundMount(bool stopMove = true)
        {
            try
            {
                if (nManagerSetting.CurrentSetting.GroundMountName == string.Empty)
                {
                    //    Logging.Write("Please configure your Ground mount in General settings");
                    //    MessageBox.Show("Please configure your Ground mount in General settings");
                    return;
                }

                if (!SpellManager.ExistMountLUA(nManagerSetting.CurrentSetting.GroundMountName) && !SpellManager.SpellUsableLUA(nManagerSetting.CurrentSetting.GroundMountName))
                {
                    Logging.Write("The mount \"" + nManagerSetting.CurrentSetting.GroundMountName + "\" does not exist !");
                    if (stopMove)
                        MovementManager.StopMove();
                    else
                        MovementManager.StopMoveTo();
                    MessageBox.Show(Translate.Get(Translate.Id.This_mount_does_not_exist) + ": " + nManagerSetting.CurrentSetting.GroundMountName);
                    nManager.Products.Products.ProductStop();
                    return;
                }

                if (ObjectManager.ObjectManager.Me.IsMounted && !SpellManager.HaveBuffLua(nManagerSetting.CurrentSetting.GroundMountName))
                    DismountMount(stopMove);

                if (!ObjectManager.ObjectManager.Me.IsMounted && nManagerSetting.CurrentSetting.useGroundMount && Usefuls.IsOutdoors)
                {
                    if (stopMove)
                        MovementManager.StopMove();
                    else
                        MovementManager.StopMoveTo();
                    Logging.Write("Mounting " + nManagerSetting.CurrentSetting.GroundMountName);

                    Thread.Sleep(500);
                    SpellManager.CastSpellByNameLUA(nManagerSetting.CurrentSetting.GroundMountName);
                    Thread.Sleep(1000);
                    Thread.Sleep(Usefuls.Latency);
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
                if (nManagerSetting.CurrentSetting.AquaticMountName == string.Empty || (nManager.Wow.Helpers.Usefuls.AreaId != 5146 
                     && nManager.Wow.Helpers.Usefuls.AreaId != 4815
                     && nManager.Wow.Helpers.Usefuls.AreaId != 5145
                     && nManager.Wow.Helpers.Usefuls.AreaId != 5144 && SpellManager.SpellListManager.SpellIdByName(nManagerSetting.CurrentSetting.AquaticMountName)[0] == 75207))
                {
                    return;
                }

                if (!SpellManager.ExistMountLUA(nManagerSetting.CurrentSetting.AquaticMountName) && !SpellManager.SpellUsableLUA(nManagerSetting.CurrentSetting.AquaticMountName))
                {
                    Logging.Write("The mount \"" + nManagerSetting.CurrentSetting.AquaticMountName + "\" does not exist !");
                    if (stopMove)
                        MovementManager.StopMove();
                    else
                        MovementManager.StopMoveTo();
                    MessageBox.Show(Translate.Get(Translate.Id.This_mount_does_not_exist) + ": " + nManagerSetting.CurrentSetting.AquaticMountName);
                    nManager.Products.Products.ProductStop();
                    return;
                }

                if (ObjectManager.ObjectManager.Me.IsMounted && !SpellManager.HaveBuffLua(nManagerSetting.CurrentSetting.AquaticMountName))
                    DismountMount(stopMove);

                if (!ObjectManager.ObjectManager.Me.IsMounted)
                {
                    if (stopMove)
                        MovementManager.StopMove();
                    else
                        MovementManager.StopMoveTo();
                    Logging.Write("Mounting " + nManagerSetting.CurrentSetting.AquaticMountName);

                    Thread.Sleep(500);
                    SpellManager.CastSpellByNameLUA(nManagerSetting.CurrentSetting.AquaticMountName);
                    Thread.Sleep(1000);
                    Thread.Sleep(Usefuls.Latency);
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
                if (nManager.Wow.Helpers.Usefuls.AreaId == 5389
                     || nManager.Wow.Helpers.Usefuls.AreaId == 5095)
                {
                    MountingGroundMount(false);
                    return;
                }
                if (TimerMount != null)
                    if (!TimerMount.IsReady)
                        return;
                TimerMount = new Timer(1*300);

                if (nManagerSetting.CurrentSetting.FlyingMountName == string.Empty)
                {
                    Logging.Write("Please configure your Fly mount in General settings");
                    if (stopMove)
                        MovementManager.StopMove();
                    else
                        MovementManager.StopMoveTo();
                    MessageBox.Show(Translate.Get(Translate.Id.Please_configure_your_Fly_mount_in_General_settings));
                    nManager.Products.Products.ProductStop();
                    return;
                }

                if (!SpellManager.ExistMountLUA(nManagerSetting.CurrentSetting.FlyingMountName) && !SpellManager.SpellUsableLUA(nManagerSetting.CurrentSetting.FlyingMountName))
                {
                    Logging.Write("The mount \"" + nManagerSetting.CurrentSetting.FlyingMountName + "\" does not exist !");
                    if (stopMove)
                        MovementManager.StopMove();
                    else
                        MovementManager.StopMoveTo();
                    MessageBox.Show(Translate.Get(Translate.Id.This_mount_does_not_exist) + ": " + nManagerSetting.CurrentSetting.FlyingMountName);
                    nManager.Products.Products.ProductStop();
                    return;
                }
                if (ObjectManager.ObjectManager.Me.IsMounted && !SpellManager.HaveBuffLua(nManagerSetting.CurrentSetting.FlyingMountName) && !Usefuls.IsFlying)
                    DismountMount(stopMove);

                if (!ObjectManager.ObjectManager.Me.IsMounted)
                {
                    if (stopMove)
                        MovementManager.StopMove();
                    else
                        MovementManager.StopMoveTo();
                    Logging.Write("Mounting " + nManagerSetting.CurrentSetting.FlyingMountName);
                    Thread.Sleep(100);
                    if (Usefuls.IsSwimming)
                    {
                        Keybindings.DownKeybindings(Enums.Keybindings.JUMP);
                        Thread.Sleep(2000);
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
                        Thread.Sleep(800);
                        Thread.Sleep(Usefuls.Latency);
                        while (ObjectManager.ObjectManager.Me.IsCast && !ObjectManager.ObjectManager.Me.InCombat)
                        {
                            Thread.Sleep(50);
                        }
                        if (ObjectManager.ObjectManager.Me.InCombat)
                        {
                            return;
                        }
                        Thread.Sleep(800);
                    }
                    if (!ObjectManager.ObjectManager.Me.IsMounted && Products.Products.IsStarted)
                    {
                        if (ObjectManager.ObjectManager.Me.InCombat)
                        {
                            return;
                        }
                        Thread.Sleep(700);
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
                        Keybindings.DownKeybindings(Enums.Keybindings.JUMP);
                        var t = new Timer(500);
                        while (!Usefuls.IsFlying && !t.IsReady)
                        {
                            Thread.Sleep(30);
                        }
                        Thread.Sleep(100);
                        Keybindings.UpKeybindings(Enums.Keybindings.JUMP);
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError("MountTask > MountingFlyingMount(): " + ex);
            }
        }


        public static void DismountMount(bool stopMove = true, bool stand = true)
        {
            try
            {
                if (ObjectManager.ObjectManager.Me.IsMounted && Products.Products.IsStarted)
                {
                    Logging.Write("Dismount");
                    if (stopMove)
                        MovementManager.StopMove();
                    else
                        MovementManager.StopMoveTo();
                    Thread.Sleep(200);

                    if (Usefuls.IsFlying && stand)
                    {
                        Keybindings.DownKeybindings(Enums.Keybindings.SITORSTAND);
                        var t = new Timer(15500);
                        while (Usefuls.IsFlying && !t.IsReady)
                        {
                            Thread.Sleep(50);
                        }
                        Keybindings.UpKeybindings(Enums.Keybindings.SITORSTAND);
                        Thread.Sleep(10);
                    }

                    Usefuls.DisMount();
                    Thread.Sleep(500);
                    Thread.Sleep(Usefuls.Latency);
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError("MountTask > DismountMount(): " + ex);
            }
        }
    }
}
