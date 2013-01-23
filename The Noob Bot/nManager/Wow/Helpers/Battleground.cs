using System.Collections.Generic;
using System.Threading;
using nManager.Wow;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;
using nManager.Wow.Patchables;
using nManager.Wow.ObjectManager;

namespace wManager.Wow.Helpers
{
    public class Battleground
    {
        public static void JointBattleGroundQueue(BattleGroundID id)
        {
            Memory.WowMemory.Memory.WriteUInt(Memory.WowProcess.WowModule + (uint)Addresses.Battleground.selectedBattleGroundID, (uint) id);
            Thread.Sleep(100);
            Lua.LuaDoString("JoinBattlefield(0);");
        }
        public static BattleGroundID GetSelectedBattleGroundID()
        {
            return (BattleGroundID)Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + (uint)Addresses.Battleground.selectedBattleGroundID);
        }
        public static int StatBattleGround()
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
        public static bool IsFinishBattleGround()
        {
            return Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + (uint)Addresses.Battleground.pvpExitWindow) > 0;
        }
        public static void ExitBattleGround()
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
                return !ObjectManager.Me.HaveBuff(PreparationId);
            }
            catch
            { }
            return false;

        }

        public static void JoinBattlefield(BattleGroundID type, bool asGroup = false)
        {
            if (type != BattleGroundID.None)
            {
                Lua.LuaDoString("for i = 1, GetNumBattlegroundTypes() do local _,_,_,_,id = GetBattlegroundInfo(i); if id == {0} then RequestBattlegroundInstanceInfo(i); end end");
                Lua.LuaDoString(string.Format("JoinBattlefield(1, {0})", asGroup ? "true" : "false"));
                Thread.Sleep(500);
            }
        }

        public static bool IsInBattleground()
        {
            return GetCurrentBattleground() != BattleGroundID.None;
        }

        public static BattleGroundID GetCurrentBattleground()
        {
            switch ((ContinentId)Usefuls.ContinentId)
            {
                case ContinentId.PVPZone04:
                    return BattleGroundID.AB;

                case ContinentId.NetherstormBG:
                    return BattleGroundID.EotS;

                case ContinentId.PVPZone01:
                    return BattleGroundID.AV;

                case ContinentId.PVPZone03:
                    return BattleGroundID.WSG;

                case ContinentId.NorthrendBG:
                    return BattleGroundID.SotA;

                case ContinentId.IsleofConquest:
                    return BattleGroundID.IoC;

                case ContinentId.CataclysmCTF:
                    return BattleGroundID.TP;

                case ContinentId.STV_Mine_BG:
                    return BattleGroundID.SM;

                case ContinentId.Gilneas_BG_2:
                    return BattleGroundID.BfG;

                case ContinentId.ValleyOfPower:
                    return BattleGroundID.ToK;
            }
            return BattleGroundID.None;
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