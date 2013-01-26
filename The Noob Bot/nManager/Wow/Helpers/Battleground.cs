using System.Collections.Generic;
using System.Threading;
using nManager.Wow;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;
using nManager.Wow.Patchables;
using nManager.Wow.ObjectManager;

namespace nManager.Wow.Helpers
{
    public class Battleground
    {
        public static void JoinBattlegroundQueue(BattlegroundId id)
        {
            Memory.WowMemory.Memory.WriteUInt(Memory.WowProcess.WowModule + (uint)Addresses.Battleground.selectedBattlegroundId, (uint) id);
            Thread.Sleep(100);
            Lua.LuaDoString("JoinBattlefield(0);");
        }
        public static BattlegroundId GetSelectedBattlegroundId()
        {
            return (BattlegroundId)Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + (uint)Addresses.Battleground.selectedBattlegroundId);
        }
        public static int QueueingStatus()
        {
            var v1 = Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + (uint)Addresses.Battleground.statPvp);
            int v2 = (Memory.WowMemory.Memory.ReadByte(Memory.WowProcess.WowModule + (uint)Addresses.Battleground.statPvp) & 1);
            if (v1 == 0 || v2 > 0)
                return 0;

            return 1;
        }
        public static void AcceptBattlefieldPort(int index, bool accept)
        {
            Lua.LuaDoString("AcceptBattlefieldPort(" + index + "," + (accept ? 1 : 0) + ")");
        }
        public static void AcceptBattlefieldPortAll()
        {
            for (int i = 0; i <= 10; i++)
            {
                AcceptBattlefieldPort(i, true);
                Thread.Sleep(500);
            }
        }
        public static bool IsFinishBattleground()
        {
            return Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + (uint)Addresses.Battleground.pvpExitWindow) > 0;
        }
        public static void ExitBattleground()
        {
            Lua.LuaDoString("LeaveBattlefield()");
        }

        static readonly List<uint> PreparationId = new List<uint>();


        public static bool BattlegroundIsStarted()
        {
            try
            {
                if (PreparationId.Count <= 0)
                {
                    PreparationId.Add(44521);
                    PreparationId.Add(32728);
                    PreparationId.Add(32727);
                }
                return !ObjectManager.ObjectManager.Me.HaveBuff(PreparationId);
            }
            catch
            { }
            return false;

        }

        public static void JoinBattlefield(BattlegroundId type, bool asGroup = false)
        {
            if (type != BattlegroundId.None)
            {
                Lua.LuaDoString("for i = 1, GetNumBattlegroundTypes() do local _,_,_,_,id = GetBattlegroundInfo(i); if id == {0} then RequestBattlegroundInstanceInfo(i); end end");
                Lua.LuaDoString(string.Format("JoinBattlefield(1, {0})", asGroup ? "true" : "false"));
                Thread.Sleep(500);
            }
        }

        public static bool IsInBattleground()
        {
            if(GetCurrentBattleground() != BattlegroundId.None)
                return true;
            return false;
        }

        public static BattlegroundId GetCurrentBattleground()
        {
            switch ((ContinentId)Usefuls.ContinentId)
            {
                case ContinentId.PVPZone04:
                    return BattlegroundId.ArathiBasin;

                case ContinentId.NetherstormBG:
                    return BattlegroundId.EyeOfTheStorm;

                case ContinentId.PVPZone01:
                    return BattlegroundId.AlteracValley;

                case ContinentId.PVPZone03:
                    return BattlegroundId.WarsongGulch;

                case ContinentId.NorthrendBG:
                    return BattlegroundId.StrandOfTheAncients;

                case ContinentId.IsleofConquest:
                    return BattlegroundId.IsleOfConquest;

                case ContinentId.CataclysmCTF:
                    return BattlegroundId.TwinPeaks;

                case ContinentId.STV_Mine_BG:
                    return BattlegroundId.SilvershardMines;

                case ContinentId.Gilneas_BG_2:
                    return BattlegroundId.BattleForGilneas;

                case ContinentId.ValleyOfPower:
                    return BattlegroundId.TempleOfKotmogu;
            }
            return BattlegroundId.None;
        }

        public string NonLocalizedName
        {
            get
            {
                switch ((ContinentId)Usefuls.ContinentId)
                {
                    case ContinentId.PVPZone04:
                        return "Arathi Basin";

                    case ContinentId.NetherstormBG:
                        return "Eye of the Storm";

                    case ContinentId.PVPZone01:
                        return "Alterac Valley";

                    case ContinentId.PVPZone03:
                        return "Warsong Gulch";

                    case ContinentId.NorthrendBG:
                        return "Strand of the Ancients";

                    case ContinentId.IsleofConquest:
                        return "Isle of Conquest";

                    case ContinentId.CataclysmCTF:
                        return "Twin Peaks";

                    case ContinentId.STV_Mine_BG:
                        return "Silvershard Mines";

                    case ContinentId.Gilneas_BG_2:
                        return "Battle For Gilneas";

                    case ContinentId.ValleyOfPower:
                        return "Temple of Kotmogu";
                }
                return "";
            }
        }

    }
}