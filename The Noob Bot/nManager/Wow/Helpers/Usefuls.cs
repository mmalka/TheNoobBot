using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;
using nManager.Helpful;
using nManager.Wow.Enums;
using nManager.Wow.Patchables;
using Timer = nManager.Helpful.Timer;

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
                MessageBox.Show(string.Format("{0}.", Translate.Get(Translate.Id.Please_select_exe_in_the_install_folder_of_the_game)));
                string path = Others.DialogBoxOpenFile("", "Profile files (Wow.exe)|Wow.exe");
                RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Blizzard Entertainment\\World of Warcraft") ??
                                  Registry.LocalMachine.CreateSubKey("SOFTWARE\\Blizzard Entertainment\\World of Warcraft");
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
        /// <param name="param"></param>
        /// <returns></returns>
        public static int LaunchWow(string param = "")
        {
            try
            {
                RegistryKey registre = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Blizzard Entertainment\World of Warcraft");
                if (registre == null)
                {
                    MakeWowRegistry();
                    LaunchWow();
                    return 0;
                }
                object val = registre.GetValue("InstallPath");
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
                Process proc = new Process {StartInfo = {FileName = val + "Wow.exe", Arguments = param}};
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
                try
                {
                    return
                        Memory.WowMemory.Memory.ReadUTF8String(Memory.WowProcess.WowModule +
                                                               (uint) Addresses.GameInfo.lastWowErrorMessage);
                }
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
                try
                {
                    return Memory.WowMemory.Memory.ReadByte(Memory.WowProcess.WowModule + (uint) Addresses.GameInfo.gameState) > 0;
                }
                catch (Exception exception)
                {
                    if (exception.ToString() == "Process is not open for read/write.")
                        Thread.Sleep(500);
                    Logging.WriteError("InGame: " + exception);
                    return false;
                }
            }
        }

        public static bool IsLoadingOrConnecting
        {
            get
            {
                try
                {
                    return
                        (Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule +
                                                         (uint) Addresses.GameInfo.isLoadingOrConnecting) != 0);
                }
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
                lock (typeof (Usefuls))
                {
                    try
                    {
                        string randomString = Others.GetRandomString(Others.Random(4, 10));
                        Lua.LuaDoString(randomString + " = GetMoney()");
                        return Others.ToInt32(Lua.GetLocalizedText(randomString));
                    }
                    catch (Exception e)
                    {
                        Logging.WriteError("GetMoneyCopper: " + e);
                        return 0;
                    }
                }
            }
        }

        public static bool IsFlyableArea
        {
            get
            {
                lock (typeof (Usefuls))
                {
                    try
                    {
                        string randomString = Others.GetRandomString(Others.Random(4, 10));
                        Lua.LuaDoString(randomString + " = IsFlyableArea()");
                        return Lua.GetLocalizedText(randomString) == "1";
                    }
                    catch (Exception e)
                    {
                        Logging.WriteError("IsFlyableArea: " + e);
                        return false;
                    }
                }
            }
        }

        public static bool IsOutdoors
        {
            get
            {
                lock (typeof (Usefuls))
                {
                    try
                    {
                        string randomString = Others.GetRandomString(Others.Random(4, 10));
                        Lua.LuaDoString(randomString + " = IsOutdoors()");
                        return Lua.GetLocalizedText(randomString) == "1";
                    }
                    catch (Exception e)
                    {
                        Logging.WriteError("IsOutdoors: " + e);
                        return false;
                    }
                }
            }
        }

        private static int _lastContainerNumFreeSlots;
        private static Timer _timerContainerNumFreeSlots = new Timer(0);

        public static int ContainerNumFreeSlots(BagType bagType)
        {
            int unspecified = 0;
            int quiver = 0;
            int ammoPouch = 0;
            int soulBag = 0;
            int leatherworkingBag = 0;
            int inscriptionBag = 0;
            int herbBag = 0;
            int enchantingBag = 0;
            int engineeringBag = 0;
            int keyring = 0;
            int gemBag = 0;
            int miningBag = 0;
            int unknown = 0;
            int vanityPets = 0;
            int lureBag = 0;
            string bag0FreeSlots = Others.GetRandomString(Others.Random(4, 10));
            string bag0Type = Others.GetRandomString(Others.Random(4, 10));
            string bag1FreeSlots = Others.GetRandomString(Others.Random(4, 10));
            string bag1Type = Others.GetRandomString(Others.Random(4, 10));
            string bag2FreeSlots = Others.GetRandomString(Others.Random(4, 10));
            string bag2Type = Others.GetRandomString(Others.Random(4, 10));
            string bag3FreeSlots = Others.GetRandomString(Others.Random(4, 10));
            string bag3Type = Others.GetRandomString(Others.Random(4, 10));
            string bag4FreeSlots = Others.GetRandomString(Others.Random(4, 10));
            string bag4Type = Others.GetRandomString(Others.Random(4, 10));
            string randomString = Others.GetRandomString(Others.Random(4, 10));
            Lua.LuaDoString(bag0FreeSlots + "," + bag0Type + " = GetContainerNumFreeSlots(0); " +
                            bag1FreeSlots + "," + bag1Type + " = GetContainerNumFreeSlots(1); " +
                            bag2FreeSlots + "," + bag2Type + " = GetContainerNumFreeSlots(2); " +
                            bag3FreeSlots + "," + bag3Type + " = GetContainerNumFreeSlots(3); " +
                            bag4FreeSlots + "," + bag4Type + " = GetContainerNumFreeSlots(4); " +
                            "if(" + bag1Type + " == nil) then " + bag1Type + " = 16777216 end " +
                            "if(" + bag2Type + " == nil) then " + bag2Type + " = 16777216 end " +
                            "if(" + bag3Type + " == nil) then " + bag3Type + " = 16777216 end " +
                            "if(" + bag4Type + " == nil) then " + bag4Type + " = 16777216 end " +
                            randomString + " = " + bag0FreeSlots + " .. \",\" .. " + bag0Type + " .. \";\" .. " + bag1FreeSlots + " .. \",\" .. " + bag1Type + " .. \";\" .. " +
                            bag2FreeSlots + " .. \",\" .. " + bag2Type + " .. \";\" .. " + bag3FreeSlots + " .. \",\" .. " + bag3Type + " .. \";\" .. " + bag4FreeSlots +
                            " .. \",\" .. " + bag4Type);
            string result = Lua.GetLocalizedText(randomString);
            if (!string.IsNullOrEmpty(result) && result.Contains(";"))
            {
                string[][] bags = result.Split(';').Select(s => s.Split(',')).ToArray();
                foreach (string[] t in bags)
                {
                    if (t.Count() <= 1)
                        continue;
                    if (t[0] == "0")
                        continue;
                    int currBagFreeSlots = Others.ToInt32(t[0]);
                    BagType currBagType = (BagType) Others.ToInt32(t[1]);
                    if (currBagType == BagType.Unspecified)
                    {
                        unspecified += currBagFreeSlots;
                        continue;
                    }
                    if (currBagType.HasFlag(BagType.None) || !currBagType.HasFlag(bagType))
                        continue;
                    switch (bagType)
                    {
                        case BagType.MiningBag:
                            miningBag += currBagFreeSlots;
                            break;
                        case BagType.HerbBag:
                            herbBag += currBagFreeSlots;
                            break;
                        case BagType.LeatherworkingBag:
                            leatherworkingBag += currBagFreeSlots;
                            break;
                        case BagType.GemBag:
                            gemBag += currBagFreeSlots;
                            break;
                        case BagType.EnchantingBag:
                            enchantingBag += currBagFreeSlots;
                            break;
                        case BagType.InscriptionBag:
                            inscriptionBag += currBagFreeSlots;
                            break;
                        case BagType.LureBag:
                            lureBag += currBagFreeSlots;
                            break;
                        case BagType.SoulBag:
                            soulBag += currBagFreeSlots;
                            break;
                        case BagType.VanityPets:
                            vanityPets += currBagFreeSlots;
                            break;
                        case BagType.Unknown:
                            unknown += currBagFreeSlots;
                            break;
                        case BagType.AmmoPouch:
                            ammoPouch += currBagFreeSlots;
                            break;
                        case BagType.Keyring:
                            keyring += currBagFreeSlots;
                            break;
                        case BagType.Quiver:
                            quiver += currBagFreeSlots;
                            break;
                        case BagType.EngineeringBag:
                            engineeringBag += currBagFreeSlots;
                            break;
                    }
                }
                switch (bagType)
                {
                    case BagType.Unspecified:
                        return unspecified;
                    case BagType.MiningBag:
                        return miningBag + unspecified;
                    case BagType.HerbBag:
                        return herbBag + unspecified;
                    case BagType.LeatherworkingBag:
                        return leatherworkingBag + unspecified;
                    case BagType.GemBag:
                        return gemBag + unspecified;
                    case BagType.EnchantingBag:
                        return enchantingBag + unspecified;
                    case BagType.InscriptionBag:
                        return inscriptionBag + unspecified;
                    case BagType.LureBag:
                        return lureBag + unspecified;
                    case BagType.SoulBag:
                        return soulBag + unspecified;
                    case BagType.VanityPets:
                        return vanityPets + unspecified;
                    case BagType.Unknown:
                        return unknown + unspecified;
                    case BagType.AmmoPouch:
                        return ammoPouch + unspecified;
                    case BagType.Keyring:
                        return keyring + unspecified;
                    case BagType.Quiver:
                        return quiver + unspecified;
                    case BagType.EngineeringBag:
                        return engineeringBag + unspecified;
                }
            }
            return 0;
        }

        public static int GetContainerNumFreeSlots
        {
            get
            {
                try
                {
                    lock (typeof (Usefuls))
                    {
                        if (!_timerContainerNumFreeSlots.IsReady)
                            return _lastContainerNumFreeSlots;

                        _timerContainerNumFreeSlots = new Timer(1000);
                        string randomString = Others.GetRandomString(Others.Random(4, 10));
                        string result = Lua.LuaDoString(
                            randomString + " = 0; for i = 0, 4 do if GetContainerNumFreeSlots(i) ~= nil then " +
                            randomString + " = " + randomString + " + GetContainerNumFreeSlots(i); end end  ",
                            randomString);
                        if (Regex.IsMatch(result, @"^[0-9]+$"))
                            _lastContainerNumFreeSlots = Others.ToInt32(result);
                        else
                            Logging.WriteError("GetContainerNumFreeSlots failed, \"" + result + "\" returned.");
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
                catch (Exception e)
                {
                    Logging.WriteError("GetContainerNumFreeSlots: " + e);
                    return 50;
                }
            }
        }

        private static int _lastHonorPoint;
        private static readonly Timer TimerHonorPoint = new Timer(1000);

        public static int GetHonorPoint
        {
            get
            {
                lock (typeof (Usefuls))
                {
                    try
                    {
                        if (!TimerHonorPoint.IsReady)
                            return _lastHonorPoint;

                        TimerHonorPoint.Reset();

                        string randomString = Others.GetRandomString(Others.Random(4, 10));
                        Lua.LuaDoString("_, " + randomString + " = GetCurrencyInfo(392);");

                        int t;
                        try
                        {
                            string localized = Lua.GetLocalizedText(randomString);
                            if (localized != null)
                                t = Others.ToInt32(localized);
                            else t = -1;
                        }
                        catch
                        {
                            t = -1;
                        }

                        if (t >= -1 || t <= 4000)
                            _lastHonorPoint = t;

                        return _lastHonorPoint;
                    }
                    catch (Exception e)
                    {
                        Logging.WriteError("GetHonorPoint: " + e);
                        return 0;
                    }
                }
            }
        }

        private static Timer _timePlayerUsingVehicle = new Timer(0);
        private static bool _lastResultPlayerUsingVehicle;

        public static bool PlayerUsingVehicle
        {
            get
            {
                lock (typeof (Usefuls))
                {
                    try
                    {
                        if (ObjectManager.ObjectManager.Me.InTransport)
                            return true;

                        if (!_timePlayerUsingVehicle.IsReady)
                            return _lastResultPlayerUsingVehicle;

                        string randomString = Others.GetRandomString(Others.Random(4, 10));
                        Lua.LuaDoString(randomString + " = UnitUsingVehicle(\"player\");");
                        _lastResultPlayerUsingVehicle = Lua.GetLocalizedText(randomString) == "1";
                        _timePlayerUsingVehicle = new Timer(500);
                        return _lastResultPlayerUsingVehicle;
                    }
                    catch (Exception e)
                    {
                        Logging.WriteError("PlayerUsingVehicle: " + e);
                        return false;
                    }
                }
            }
        }

        private static int _lastLatency;
        private static Timer _timerLatency = new Timer(0);

        public static int Latency
        {
            get
            {
                lock (typeof (Usefuls))
                {
                    try
                    {
                        if (!_timerLatency.IsReady)
                            return _lastLatency;

                        _timerLatency = new Timer(30*1000);
                        string randomString = Others.GetRandomString(Others.Random(4, 10));
                        Lua.LuaDoString("_, _, lagHome, lagWorld = GetNetStats(); " + randomString +
                                        " = lagHome + lagWorld");
                        _lastLatency = Others.ToInt32(Lua.GetLocalizedText(randomString)) / 2;
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

        public static bool MovementStatus(MovementFlags flags)
        {
            try
            {
                return (Convert.ToBoolean(
                    Memory.WowMemory.Memory.ReadInt(
                        Memory.WowMemory.Memory.ReadUInt(ObjectManager.ObjectManager.Me.GetBaseAddress +
                                                         (uint) Addresses.MovementFlagsOffsets.Offset1)
                        + (uint) Addresses.MovementFlagsOffsets.Offset2)
                    & (int) flags));
            }
            catch (Exception e)
            {
                Logging.WriteError("MovementStatus: " + e);
                return false;
            }
        }

        public static MovementFlags GetAllMovementStatus
        {
            get
            {
                return
                    (MovementFlags)
                    Memory.WowMemory.Memory.ReadInt(
                        Memory.WowMemory.Memory.ReadUInt(ObjectManager.ObjectManager.Me.GetBaseAddress + (uint) Addresses.MovementFlagsOffsets.Offset1) +
                        (uint) Addresses.MovementFlagsOffsets.Offset2);
            }
        }

        public static bool IsFalling
        {
            get { return MovementStatus(MovementFlags.Falling); }
        }

        public static bool IsSwimming
        {
            get { return MovementStatus(MovementFlags.Swimming); }
        }

        public static bool IsFlying
        {
            get { return MovementStatus(MovementFlags.Flying); }
        }

        public static string RealmName
        {
            get
            {
                try
                {
                    return Memory.WowMemory.Memory.ReadUTF8String(Memory.WowProcess.WowModule + (uint) Addresses.Login.realmName);
                }
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
                try
                {
                    return Memory.WowMemory.Memory.ReadUTF8String(Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + (uint) Addresses.GameInfo.zoneMap));
                }
                catch (Exception e)
                {
                    Logging.WriteError("MapZoneName: " + e);
                    return "";
                }
            }
        }

        public static string SubMapZoneName
        {
            get
            {
                try
                {
                    return Memory.WowMemory.Memory.ReadUTF8String(Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + (uint) Addresses.GameInfo.subZoneMap));
                }
                catch (Exception e)
                {
                    Logging.WriteError("SubMapZoneName: " + e);
                    return "";
                }
            }
        }

        public static string MapName
        {
            get
            {
                try
                {
                    return ((ContinentId) (Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule + (uint) Addresses.GameInfo.continentId))).ToString();
                }
                catch (Exception e)
                {
                    Logging.WriteError("MapName: " + e);
                    return "Azeroth";
                }
            }
        }

        public static string ContinentNameMpq
        {
            get
            {
                try
                {
                    int cId = Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule + (uint) Addresses.GameInfo.continentId);

                    string retS;
                    switch ((ContinentId) cId)
                    {
                        case Enums.ContinentId.PVPZone04:
                            retS = "PVPZone04";
                            break;
                        case Enums.ContinentId.PVPZone01:
                            retS = "PVPZone01";
                            break;
                        case Enums.ContinentId.Azeroth:
                            retS = "Azeroth";
                            break;
                        case Enums.ContinentId.NetherstormBG:
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
                        case Enums.ContinentId.Pandaria:
                            retS = "HawaiiMainLand";
                            break;
                        case Enums.ContinentId.NorthrendBG:
                            retS = "NorthrendBG";
                            break;
                        case Enums.ContinentId.IsleofConquest:
                            retS = "IsleofConquest";
                            break;
                        case Enums.ContinentId.CataclysmCTF:
                            retS = "CataclysmCTF";
                            break;
                        case Enums.ContinentId.Gilneas_BG_2:
                            retS = "Gilneas_BG_2";
                            break;
                        case Enums.ContinentId.PVPZone03:
                            retS = "PVPZone03";
                            break;
                        case Enums.ContinentId.ValleyOfPower:
                            retS = "ValleyOfPower";
                            break;
                        case Enums.ContinentId.DeathKnightStart:
                            retS = "DeathKnightStart";
                            break;
                        case Enums.ContinentId.SunwellPlateau:
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
                        case Enums.ContinentId.Gilneas:
                            retS = "Gilneas";
                            break;
                        case Enums.ContinentId.Gilneas2:
                            retS = "Gilneas2";
                            break;
                        case Enums.ContinentId.NewRaceStartZone:
                            retS = "NewRaceStartZone";
                            break;
                        case Enums.ContinentId.MoguIslandDailyArea:
                            retS = "MoguIslandDailyArea";
                            break;
                        case Enums.ContinentId.EyeoftheStorm2dot0:
                            retS = "EyeoftheStorm2.0";
                            break;
                        case Enums.ContinentId.GoldRushBG:
                            retS = "GoldRushBG";
                            break;
                        default:
                            retS = "MapId_" + cId;
                            break;
                    }
                    return retS;
                }
                catch (Exception e)
                {
                    Logging.WriteError("ContinentNameMpq: " + e);
                    return "Azeroth";
                }
            }
        }

        public static int ContinentId
        {
            get
            {
                try
                {
                    return Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule + (uint) Addresses.GameInfo.continentId);
                }
                catch (Exception e)
                {
                    Logging.WriteError("ContinentId: " + e);
                    return 0;
                }
            }
        }

        public static int AreaId
        {
            get
            {
                try
                {
                    return Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule + (uint) Addresses.GameInfo.AreaId);
                }
                catch (Exception e)
                {
                    Logging.WriteError("AreaId: " + e);
                    return 0;
                }
            }
        }

        public static uint WowVersion
        {
            get
            {
                try
                {
                    return Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + (uint) Addresses.GameInfo.buildWowVersion);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WowVersion: " + e);
                    return 0;
                }
            }
        }

        public static void DisMount()
        {
            try
            {
                Lua.RunMacroText(ObjectManager.ObjectManager.Me.HaveBuff(SpellManager.MountDruidId()) ? "/cancelform" : "/dismount");
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
                uint mask =
                    Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + (uint) Addresses.PlayerNameStore.nameStorePtr + (uint) Addresses.PlayerNameStore.nameMaskOffset);
                uint baseAddresse =
                    Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + (uint) Addresses.PlayerNameStore.nameStorePtr + (uint) Addresses.PlayerNameStore.nameBaseOffset);

                ulong shortGUID = guid & 0xffffffff;
                if (mask == 0xffffffff)
                    return "";

                uint offset = 12*(uint) (mask & shortGUID);
                uint current = Memory.WowMemory.Memory.ReadUInt(baseAddresse + offset + 8);
                offset = Memory.WowMemory.Memory.ReadUInt(baseAddresse + offset);

                if (current == 0 || (current & 0x1) == 0x1)
                    return "";

                uint testGUID = Memory.WowMemory.Memory.ReadUInt(current);

                while (testGUID != shortGUID)
                {
                    current = Memory.WowMemory.Memory.ReadUInt(current + offset + 4);
                    if (current == 0 || (current & 0x1) == 0x1)
                        return "";
                    testGUID = Memory.WowMemory.Memory.ReadUInt(current);
                    Thread.Sleep(5);
                }


                return Memory.WowMemory.Memory.ReadUTF8String(current + (uint) Addresses.PlayerNameStore.nameStringOffset);
            }
            catch (Exception e)
            {
                Logging.WriteError("GetPlayerName(ulong guid): " + e);
                return "";
            }
        }

        private static readonly List<int> AchievementsDoneCache = new List<int>();
        private static readonly List<int> AchievementsNotDoneCache = new List<int>();

        public static bool IsCompletedAchievement(int achievementId, bool meOnly = false)
        {
            if (AchievementsDoneCache.Contains(achievementId))
                return true;
            if (AchievementsNotDoneCache.Contains(achievementId))
                return false;
            string randomString = Others.GetRandomString(Others.Random(4, 10));
            string randomString2 = Others.GetRandomString(Others.Random(4, 10));
            string randomString3 = Others.GetRandomString(Others.Random(4, 10));
            string query;
            if (meOnly)
                query = "_, _, _, " + randomString2 + ", _, _, _, _, _, _, _, _, " + randomString3 + " = GetAchievementInfo(" + achievementId + "); if " + randomString2 + " and " + randomString3 + " then " + randomString + "=\"1\" else " + randomString + "=\"0\" end;";
            else
                query = "_, _, _, " + randomString2 + " = GetAchievementInfo(" + achievementId + "); if " + randomString2 + " then " + randomString + "=\"1\" else " + randomString + "=\"0\" end;";
            Lua.LuaDoString(query);
            string ret = Lua.GetLocalizedText(randomString);
            if (ret == "1")
            {
                AchievementsDoneCache.Add(achievementId);
                return true;
            }
            AchievementsNotDoneCache.Add(achievementId);
            return false;
        }

        private static readonly Object ThisLock = new Object();
        private static readonly Timer AfkTimer = new Timer(500);
        private static string _key;

        public static void UpdateLastHardwareAction()
        {
            lock (ThisLock)
            {
                if (!InGame || IsLoadingOrConnecting)
                {
                    Thread.Sleep(10);
                    return;
                }
                if (string.IsNullOrEmpty(_key))
                {
                    Thread.Sleep(10);
                    _key = Keybindings.GetAFreeKey(true);
                    AfkTimer.Reset();
                }
                if (!AfkTimer.IsReady) return;
                Keyboard.DownKey(Memory.WowProcess.MainWindowHandle, _key);
                Thread.Sleep(10);
                Keyboard.UpKey(Memory.WowProcess.MainWindowHandle, _key);
                AfkTimer.Reset();
            }
        }
    }
}