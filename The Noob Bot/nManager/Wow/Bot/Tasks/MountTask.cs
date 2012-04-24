using System;
using System.Threading;
using System.Windows.Forms;
using nManager.Helpful;
using nManager.Wow.Helpers;
using Timer = nManager.Helpful.Timer;
using nManager.Wow.Class;

namespace nManager.Wow.Bot.Tasks
{
    public class MountTask
    {
        private static int _nbTry;

        public static void MountingMount(bool stopMove = true)
        {
            try
            {
                if (nManagerSetting.CurrentSetting.groundName == string.Empty)
                {
                    //    Logging.Write("Please configure your Ground mount in General settings");
                    //    MessageBox.Show("Please configure your Ground mount in General settings");
                    return;
                }

                var groundSpell = new Spell(nManagerSetting.CurrentSetting.groundName);
                if (groundSpell.Ids.Count != 0)
                {
                    Logging.Write("The mount \"" + nManagerSetting.CurrentSetting.groundName + "\" does not exist !");
                    if (stopMove)
                        MovementManager.StopMove();
                    else
                        MovementManager.StopMoveTo();
                    MessageBox.Show(Translate.Get(Translate.Id.This_mount_does_not_exist) + ": " + nManagerSetting.CurrentSetting.groundName);
                    nManager.Products.Products.ProductStop();
                    return;
                }

                if (ObjectManager.ObjectManager.Me.IsMounted && !SpellManager.HaveBuffLua(nManagerSetting.CurrentSetting.groundName))
                    DismountMount(stopMove);

                if (!ObjectManager.ObjectManager.Me.IsMounted && nManagerSetting.CurrentSetting.useGroundMount && Usefuls.IsOutdoors)
                {
                    if (stopMove)
                        MovementManager.StopMove();
                    else
                        MovementManager.StopMoveTo();
                    Logging.Write("Mounting " + nManagerSetting.CurrentSetting.groundName);

                    Thread.Sleep(500);
                    SpellManager.CastSpellByNameLUA(nManagerSetting.CurrentSetting.groundName);
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
                Logging.WriteError("MountTask > MountingMount(): " + ex);
            }
        }

        public static void MountingAquaticMount(bool stopMove = true)
        {
            try
            {
                if (nManagerSetting.CurrentSetting.aquaticName == string.Empty || (nManager.Wow.Helpers.Usefuls.AreaId != 5146 
                     && nManager.Wow.Helpers.Usefuls.AreaId != 4815
                     && nManager.Wow.Helpers.Usefuls.AreaId != 5145
                     && nManager.Wow.Helpers.Usefuls.AreaId != 5144 && SpellManager.SpellListManager.SpellIdByName(nManagerSetting.CurrentSetting.aquaticName)[0] == 75207))
                {
                    return;
                }

                var aquaticSpell = new Spell(nManagerSetting.CurrentSetting.aquaticName);
                if (aquaticSpell.Ids.Count != 0)
                {
                    Logging.Write("The mount \"" + nManagerSetting.CurrentSetting.aquaticName + "\" does not exist !");
                    if (stopMove)
                        MovementManager.StopMove();
                    else
                        MovementManager.StopMoveTo();
                    MessageBox.Show(Translate.Get(Translate.Id.This_mount_does_not_exist) + ": " + nManagerSetting.CurrentSetting.aquaticName);
                    nManager.Products.Products.ProductStop();
                    return;
                }

                if (ObjectManager.ObjectManager.Me.IsMounted && !SpellManager.HaveBuffLua(nManagerSetting.CurrentSetting.aquaticName))
                    DismountMount(stopMove);

                if (!ObjectManager.ObjectManager.Me.IsMounted)
                {
                    if (stopMove)
                        MovementManager.StopMove();
                    else
                        MovementManager.StopMoveTo();
                    Logging.Write("Mounting " + nManagerSetting.CurrentSetting.aquaticName);

                    Thread.Sleep(500);
                    SpellManager.CastSpellByNameLUA(nManagerSetting.CurrentSetting.aquaticName);
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
        public static void MountingFlyMount(bool stopMove = true)
        {
            try
            {
                if (nManager.Wow.Helpers.Usefuls.AreaId == 5389
                     || nManager.Wow.Helpers.Usefuls.AreaId == 5095)
                {
                    MountingMount(false);
                    return;
                }
                if (TimerMount != null)
                    if (!TimerMount.IsReady)
                        return;
                TimerMount = new Timer(1*300);

                if (nManagerSetting.CurrentSetting.flyingMountName == string.Empty)
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

                var flyingMountSpell = new Spell(nManagerSetting.CurrentSetting.flyingMountName);
                if (flyingMountSpell.Ids.Count != 0)
                {
                    Logging.Write("The mount \"" + nManagerSetting.CurrentSetting.flyingMountName + "\" does not exist !");
                    if (stopMove)
                        MovementManager.StopMove();
                    else
                        MovementManager.StopMoveTo();
                    MessageBox.Show(Translate.Get(Translate.Id.This_mount_does_not_exist) + ": " + nManagerSetting.CurrentSetting.flyingMountName);
                    nManager.Products.Products.ProductStop();
                    return;
                }

                if (ObjectManager.ObjectManager.Me.IsMounted && !SpellManager.HaveBuffLua(nManagerSetting.CurrentSetting.flyingMountName) && !Usefuls.IsFlying)
                    DismountMount(stopMove);

                if (!ObjectManager.ObjectManager.Me.IsMounted)
                {
                    if (stopMove)
                        MovementManager.StopMove();
                    else
                        MovementManager.StopMoveTo();
                    Logging.Write("Mounting " + nManagerSetting.CurrentSetting.flyingMountName);
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
                        SpellManager.CastSpellByNameLUA(nManagerSetting.CurrentSetting.flyingMountName);
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
                Logging.WriteError("MountTask > MountingFlyMount(): " + ex);
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
