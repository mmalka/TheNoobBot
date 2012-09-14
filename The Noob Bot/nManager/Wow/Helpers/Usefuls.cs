using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;
using nManager.Helpful;
using nManager.Wow.Patchables;

namespace nManager.Wow.Helpers
{
    public class Usefuls
    {
        /// <summary>
        /// Make key registre Wow.
        /// </summary>
        /// <returns></returns>
        public static void MakeWowRegistry()
        {
            try
            {
                MessageBox.Show(Translate.Get(Translate.Id.Please_select_exe_in_the_install_folder_of_the_game)+".");
                string path = Others.DialogBoxOpenFile("", "Profile files (Wow.exe)|Wow.exe");
                RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Blizzard Entertainment\\World of Warcraft");
                if (key == null)
                    key = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Blizzard Entertainment\\World of Warcraft");
                if (key != null) key.SetValue("InstallPath", path.Replace("Wow.exe", ""), RegistryValueKind.String);
            }
            catch (Exception e)
            {
                Logging.WriteError("MakeWowRegistry(): " + e);
            }
        }



        /// <summary>
        /// Launch World Of Warcraft.
        /// </summary>
        /// <typeparam></typeparam>
        /// <param></param>
        /// <returns></returns>
        public static int LaunchWow(string param = "")
        {
            try
            {
                var registre = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Blizzard Entertainment\World of Warcraft");
                if (registre == null)
                {
                    MakeWowRegistry();
                    LaunchWow();
                    return 0;
                }
                var val = registre.GetValue("InstallPath");
                if (val == null)
                {
                    MakeWowRegistry();
                    LaunchWow();
                    return 0;
                }
                if (!File.Exists(val + "Wow.exe"))
                {
                    MakeWowRegistry();
                    LaunchWow();
                    return 0;
                }
                var proc = new Process { StartInfo = { FileName = val + "Wow.exe", Arguments = param } };
                proc.Start();
                return proc.Id;
            }
            catch (Exception e)
            {
                Logging.WriteError("LaunchWow(): " + e);
            }
            return 0;
        }

        public static string GetLastWowErrorMessage
        {
            get
            {
                try { return Memory.WowMemory.Memory.ReadUTF8String(Memory.WowProcess.WowModule + (uint)Addresses.GameInfo.lastWowErrorMessage); }
                catch (Exception exception)
                {
                    Logging.WriteError("GetLastWowErrorMessage: " + exception);
                    return "";
                }
            }
        }

        public static bool InGame
        {
            get
            {
                try { return Memory.WowMemory.Memory.ReadByte(Memory.WowProcess.WowModule + (uint)Addresses.GameInfo.gameState) > 0; }
                catch (Exception exception)
                {
                    Logging.WriteError("InGame: " + exception);
                    return false;
                }
            }
        }

        public static bool IsLoadingOrConnecting
        {
            get
            {
                try { return (Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule + (uint)Addresses.GameInfo.isLoadingOrConnecting) != 0); }
                catch (Exception e)
                {
                    Logging.WriteError("IsLoadingOrConnecting: " + e);
                    return false;
                }
            }
        }

        public static int GetMoneyCopper
        {
            get
            {
                lock (typeof(Usefuls))
                {
                    try
                    {
                        string randomString = Others.GetRandomString(Others.Random(4, 10));
                        Lua.LuaDoString(randomString + " = GetMoney()");
                        return Convert.ToInt32(Lua.GetLocalizedText(randomString));
                    }
                    catch (Exception e)
                    {
                        Logging.WriteError("GetMoneyCopper: " + e); return 0;
                    }
                }
            }
        }

        public static bool IsOutdoors
        {
            get
            {
                lock (typeof(Usefuls))
                {
                    try
                    {
                        string randomString = Others.GetRandomString(Others.Random(4, 10));
                        Lua.LuaDoString(randomString + " = IsOutdoors()");
                        return Lua.GetLocalizedText(randomString) == "1";
                    }
                    catch (Exception e)
                    {
                        Logging.WriteError("IsOutdoors: " + e); return false;
                    }
                }
            }
        }

        private static int _lastContainerNumFreeSlots;
        private static Helpful.Timer _timerContainerNumFreeSlots = new Helpful.Timer(0);
        public static int GetContainerNumFreeSlots
        {
            get
            {
                try
                {
                    lock (typeof(Usefuls))
                    {
                        if (!_timerContainerNumFreeSlots.IsReady)
                            return _lastContainerNumFreeSlots;

                        _timerContainerNumFreeSlots = new Helpful.Timer(1000);
                        string randomString = Others.GetRandomString(Others.Random(4, 10));
                        _lastContainerNumFreeSlots = Convert.ToInt32(Lua.LuaDoString(randomString + " = 0; for i = 0, 4 do if GetContainerNumFreeSlots(i) ~= nil then " + randomString + " = " + randomString + " + GetContainerNumFreeSlots(i); end end  ", randomString));
                        return _lastContainerNumFreeSlots;
                    }

                    /*
                    try
                    {
                        int nbBag = 4;
                        int BACKPACK_SLOT = 16;

                        int freeSlot = 0;

                        for (int b = 0; b <= nbBag - 1; b++)
                        {
                            ulong bagGuid = Memory.WowMemory.Memory.ReadUInt64(Process.Process.wowModule + (uint)Addresses.Container.EquippedBagGUID + (uint)(b * 0x8));

                            if (bagGuid > 0)
                            {
                                WoWContainer t = new WoWContainer(ObjectManager.GetObjectByGuid(bagGuid).GetBaseAddress);
                                if (t.GetBaseAddress > 0)
                                {
                                    freeSlot += t.NumberSlot;

                                    for (int s = 1; s <= t.NumberSlot; s++)
                                    {
                                        if (t.GetSlot(s) > 0)
                                            freeSlot--;
                                    }
                                }
                            }
                        }


                        //Backpack 
                        freeSlot += BACKPACK_SLOT;
                        for (int s = 0; s <= BACKPACK_SLOT - 1; s++)
                        {
                            if (ObjectManager.Me.GetDescriptor<ulong>(Descriptors.PlayerFields.PLAYER_FIELD_PACK_SLOT_1 + (s * 0x8)) > 0)
                                freeSlot--;
                        }

                        return freeSlot;

                    }
                    catch
                    {
                        return 0;
                    }
                     * */
                }
                catch (Exception e) { Logging.WriteError("GetContainerNumFreeSlots: " + e); return 50; }
            }
        }

        private static int _lastHonorPoint;
        private static Helpful.Timer _timerHonorPoint = new Helpful.Timer(0);
        public static int GetHonorPoint
        {
            get
            {
                lock (typeof(Usefuls))
                {
                    try
                    {
                        if (!_timerHonorPoint.IsReady)
                            return _lastHonorPoint;

                        _timerHonorPoint = new Helpful.Timer(1000);

                        string randomString = Others.GetRandomString(Others.Random(4, 10));
                        Lua.LuaDoString("_, " + randomString + " = GetCurrencyInfo(392);");

                        int t = -1;
                        try
                        {
                            t = Convert.ToInt32(Lua.GetLocalizedText(randomString));
                        }
                        catch {}

                        if (t >= 0 || t <= 4000)
                            _lastHonorPoint = t;

                    return _lastHonorPoint;
                    }
                    catch (Exception e) { Logging.WriteError("GetHonorPoint: " + e); return 0; }
                }
            }
        }
        static Helpful.Timer _timePlayerUsingVehicle = new Helpful.Timer(0);
        static private bool _lastResultPlayerUsingVehicle;
        public static bool PlayerUsingVehicle
        {
            get
            {
                lock (typeof(Usefuls))
                {
                    try
                    {
                        if (ObjectManager.ObjectManager.Me.InTransport)
                            return true;

                        if (!_timePlayerUsingVehicle.IsReady)
                            return _lastResultPlayerUsingVehicle;

                        string randomString = Others.GetRandomString(Others.Random(4, 10));
                        Lua.LuaDoString(randomString + " = UnitUsingVehicle(\"player\");");
                        _lastResultPlayerUsingVehicle = Convert.ToBoolean(Lua.GetLocalizedText(randomString) == "1");
                        _timePlayerUsingVehicle = new Helpful.Timer(500);
                        return _lastResultPlayerUsingVehicle;
                    }
                    catch (Exception e) { Logging.WriteError("PlayerUsingVehicle: " + e); return false; }
                }
            }
        }

        private static int _lastLatency;
        private static Helpful.Timer _timerLatency = new Helpful.Timer(0);
        public static int Latency
        {
            get
            {
                lock (typeof(Usefuls))
                {
                    try
                    {
                        if (!_timerLatency.IsReady)
                            return _lastLatency;

                        _timerLatency = new Helpful.Timer(30 * 1000);
                        string randomString = Others.GetRandomString(Others.Random(4, 10));
                        Lua.LuaDoString("_, _, lagHome, lagWorld = GetNetStats (); " + randomString +
                                        " = lagHome + lagWorld");
                        _lastLatency = Convert.ToInt32(Lua.GetLocalizedText(randomString));
                        return _lastLatency;
                    }
                    catch (Exception e)
                    {
                        Logging.WriteError("Latency: " + e);
                        return 0;
                    }
                }
            }
        }

        public static bool IsSwimming
        {
            get
            {
                try
                {
                    return (Convert.ToBoolean(
                        Memory.WowMemory.Memory.ReadInt(
                        Memory.WowMemory.Memory.ReadUInt(ObjectManager.ObjectManager.Me.GetBaseAddress + (uint)Addresses.IsSwimming.offset1)
                        + (uint)Addresses.IsSwimming.offset2)
                        & (int)Addresses.IsSwimming.flag));
                }
                catch (Exception e)
                {
                    Logging.WriteError("IsSwimming: " + e); return false;
                }
            }
        }
        public static bool IsFlying
        {
            get
            {
                try
                {
                    return (Convert.ToBoolean(
                                                Memory.WowMemory.Memory.ReadInt(
                                                Memory.WowMemory.Memory.ReadUInt(ObjectManager.ObjectManager.Me.GetBaseAddress + (uint)Addresses.IsFlying.offset1)
                                                + (uint)Addresses.IsFlying.offset2)
                                                & (int)Addresses.IsFlying.flag));
                }
                catch (Exception e)
                {
                    Logging.WriteError("IsFlying: " + e); return false;
                }
            }
        }

        public static string RealmName
        {
            get
            {
                try { return Memory.WowMemory.Memory.ReadUTF8String(Memory.WowProcess.WowModule + (uint)Addresses.Login.realmName); }
                catch (Exception e)
                {
                    Logging.WriteError("RealmName: " + e);
                    return "";
                }
            }
        }
        public static string MapZoneName
        {
            get
            {
                try { return Memory.WowMemory.Memory.ReadUTF8String(Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + (uint)Addresses.GameInfo.zoneMap)); }
                catch (Exception e)
                {
                    Logging.WriteError("MapZoneName: " + e); return "";
                }
            }
        }
        public static string SubMapZoneName
        {
            get
            {
                try { return Memory.WowMemory.Memory.ReadUTF8String(Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + (uint)Addresses.GameInfo.subZoneMap)); }
                catch (Exception e)
                {
                    Logging.WriteError("SubMapZoneName: " + e); return "";
                }
            }
        }
        public static string MapName
        {
            get
            {
                try { return ((Enums.ContinentId)(Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule + (uint)Addresses.GameInfo.continentId))).ToString(); }
                catch (Exception e)
                {
                    Logging.WriteError("MapName: " + e); return "Azeroth";
                }
            }
        }
        public static string ContinentNameMpq
        {
            get
            {
                try
                {
                    int cId = Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule + (uint)Addresses.GameInfo.continentId);

                    string retS;
                    switch ((Enums.ContinentId)cId)
                    {
                        case Enums.ContinentId.AB:
                            retS = "PVPZone04";
                            break;
                        case Enums.ContinentId.AV:
                            retS = "PVPZone01";
                            break;
                        case Enums.ContinentId.Azeroth:
                            retS = "Azeroth";
                            break;
                        case Enums.ContinentId.EOTS:
                            retS = "NetherstormBG";
                            break;
                        case Enums.ContinentId.Kalimdor:
                            retS = "Kalimdor";
                            break;
                        case Enums.ContinentId.Northrend:
                            retS = "Northrend";
                            break;
                        case Enums.ContinentId.Outland:
                            retS = "Expansion01";
                            break;
                        case Enums.ContinentId.Pandarie:
                            retS = "HawaiiMainLand";
                            break;
                        case Enums.ContinentId.SOTA:
                            retS = "NorthrendBG";
                            break;
                        case Enums.ContinentId.IOC:
                            retS = "IsleofConquest";
                            break;
                        case Enums.ContinentId.TP:
                            retS = "CataclysmCTF";
                            break;
                        case Enums.ContinentId.BFG:
                            retS = "Gilneas_BG_2";
                            break;
                        case Enums.ContinentId.WSG:
                            retS = "PVPZone03";
                            break;
                        case Enums.ContinentId.Acherus:
                            retS = "DeathKnightStart";
                            break;
                        case Enums.ContinentId.Gilneas:
                            retS = "Gilneas2";
                            break;
                        case Enums.ContinentId.Sunwell:
                            retS = "SunwellPlateau";
                            break;
                        case Enums.ContinentId.DarkmoonFaire:
                            retS = "DarkmoonFaire";
                            break;
                        case Enums.ContinentId.TolBarad:
                            retS = "TolBarad";
                            break;
                        case Enums.ContinentId.Maelstrom:
                            retS = "Deephome";
                            break;
                        case Enums.ContinentId.LostIsles:
                            retS = "LostIsles";
                            break;
                        default:
                            retS = "MapId_" + cId;
                            break;
                    }
                    return retS;
                }
                catch (Exception e)
                {
                    Logging.WriteError("ContinentNameMpq: " + e); return "Azeroth";
                }
            }
        }
        public static int ContinentId
        {
            get
            {
                try { return Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule + (uint)Addresses.GameInfo.continentId); }
                catch (Exception e)
                {
                    Logging.WriteError("ContinentId: " + e); return 0;
                }
            }
        }
        public static int AreaId
        {
            get
            {
                try { return Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule + (uint)Addresses.GameInfo.AreaId); }
                catch (Exception e)
                {
                    Logging.WriteError("AreaId: " + e); return 0;
                }
            }
        }

        public static uint WowVersion
        {
            get
            {
                try { return Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + (uint)Addresses.GameInfo.buildWowVersion); }
                catch (Exception e)
                {
                    Logging.WriteError("WowVersion: " + e); return 0;
                }
            }
        }

        public static void DisMount()
        {
            try
            {
                if (ObjectManager.ObjectManager.Me.HaveBuff(SpellManager.MountDruidId()))
                    Lua.RunMacroText("/cancelform");
                else
                    Lua.RunMacroText("/dismount");
                
            }
            catch (Exception e)
            {
                Logging.WriteError("DisMount(): " + e);
            }
        }

        public static void EjectVehicle()
        {
            try
            {
                Lua.LuaDoString("VehicleExit();");
            }
            catch (Exception e)
            {
                Logging.WriteError("EjectVehicle(): " + e);
            }
        }

        public static void OpenAllBags()
        {
            try
            {
                Lua.LuaDoString("OpenAllBags();");
            }
            catch (Exception e)
            {
                Logging.WriteError("OpenAllBags(): " + e);
            }
        }

        public static void CloseAllBags()
        {
            try
            {
                Lua.LuaDoString("CloseAllBags();");
            }
            catch (Exception e)
            {
                Logging.WriteError("CloseAllBags(): " + e);
            }
        }

        public static string GetPlayerName(ulong guid)
        {
            try
            {
                var mask = Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + (uint)Addresses.PlayerNameStore.nameStorePtr + (uint)Addresses.PlayerNameStore.nameMaskOffset);
                var baseAddresse = Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + (uint)Addresses.PlayerNameStore.nameStorePtr + (uint)Addresses.PlayerNameStore.nameBaseOffset);

                var shortGUID = guid & 0xffffffff;
                if (mask == 0xffffffff)
                    return "";

                var offset = 12 * (uint)(mask & shortGUID);
                var current = Memory.WowMemory.Memory.ReadUInt(baseAddresse + offset + 8);
                offset = Memory.WowMemory.Memory.ReadUInt(baseAddresse + offset);

                if (current == 0 || (current & 0x1) == 0x1)
                    return "";

                var testGUID = Memory.WowMemory.Memory.ReadUInt(current);

                while (testGUID != shortGUID)
                {
                    current = Memory.WowMemory.Memory.ReadUInt(current + offset + 4);
                    if (current == 0 || (current & 0x1) == 0x1)
                        return "";
                    testGUID = Memory.WowMemory.Memory.ReadUInt(current);
                    Thread.Sleep(5);
                }

                return Memory.WowMemory.Memory.ReadUTF8String(current + (uint)Addresses.PlayerNameStore.nameStringOffset);
            }
            catch (Exception e)
            {
                Logging.WriteError("GetPlayerName(ulong guid): " + e);
                return "";
            }
        }
    }
}
