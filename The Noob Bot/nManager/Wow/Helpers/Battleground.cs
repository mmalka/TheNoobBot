using System.Collections.Generic;
using System.Threading;
using nManager.Wow.Enums;
using nManager.Wow.Patchables;

namespace nManager.Wow.Helpers
{
    public class Battleground
    {
        public static void SetPVPRoles()
        {
            Lua.LuaDoString("SetPVPRoles(0, 0, 1);");
            // Tank, Heal, DPS
            // We need to allow the bot to heal in BG with a settings, then we can tick Heal as well.
        }
        public static void JoinBattlegroundQueue(BattlegroundId id)
        {
            SetPVPRoles();
            Lua.LuaDoString("JoinBattlefield(" + (uint) id + ");");
        }

        public static int QueueingStatus()
        {
            var v1 =
                Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + (uint) Addresses.Battleground.statPvp);
            int v2 =
                (Memory.WowMemory.Memory.ReadByte(Memory.WowProcess.WowModule + (uint) Addresses.Battleground.statPvp) &
                 1);
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
            return
                Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule +
                                                 (uint) Addresses.Battleground.pvpExitWindow) > 0;
        }

        public static void ExitBattleground()
        {
            Lua.LuaDoString("LeaveBattlefield()");
        }

        private static readonly List<uint> PreparationId = new List<uint>();


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
            {
            }
            return false;
        }

        public static void JoinBattlefield(BattlegroundId type, bool asGroup = false)
        {
            if (type == BattlegroundId.None) return;
            Lua.LuaDoString(
                "for i = 1, GetNumBattlegroundTypes() do local _,_,_,_,id = GetBattlegroundInfo(i); if id == {0} then RequestBattlegroundInstanceInfo(i); end end");
            Lua.LuaDoString(string.Format("JoinBattlefield(1, {0})", asGroup ? "true" : "false"));
            Thread.Sleep(500);
        }

        public static bool IsInBattleground()
        {
            return GetCurrentBattleground() != BattlegroundId.None;
        }

        public static BattlegroundId GetCurrentBattleground()
        {
            switch ((ContinentId) Usefuls.ContinentId)
            {
                case ContinentId.PVPZone04:
                    return BattlegroundId.ArathiBasin;

                case ContinentId.NetherstormBG:
                    return BattlegroundId.EyeoftheStorm;

                case ContinentId.PVPZone01:
                    return BattlegroundId.AlteracValley;

                case ContinentId.PVPZone03:
                    return BattlegroundId.WarsongGulch;

                case ContinentId.NorthrendBG:
                    return BattlegroundId.StrandoftheAncients;

                case ContinentId.IsleofConquest:
                    return BattlegroundId.IsleofConquest;

                case ContinentId.CataclysmCTF:
                    return BattlegroundId.TwinPeaks;

                case ContinentId.STV_Mine_BG:
                    return BattlegroundId.SilvershardMines;

                case ContinentId.Gilneas_BG_2:
                    return BattlegroundId.BattleforGilneas;

                case ContinentId.ValleyOfPower:
                    return BattlegroundId.TempleofKotmogu;

                case ContinentId.GoldRushBG:
                    return BattlegroundId.DeepwindGorge;
            }
            return BattlegroundId.None;
        }

        public string NonLocalizedName
        {
            get
            {
                switch ((ContinentId) Usefuls.ContinentId)
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