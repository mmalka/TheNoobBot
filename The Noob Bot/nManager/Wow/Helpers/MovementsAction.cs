using nManager.Helpful;

namespace nManager.Wow.Helpers
{
    public class MovementsAction
    {
        public static void Jump()
        {
            Ascend(true);
            Ascend(false);
        }

        public static void Ascend(bool start)
        {
            Logging.WriteFileOnly("Ascend("+ start +")");
            Lua.LuaDoString(start ? "JumpOrAscendStart();" : "AscendStop();");
        }

        public static void Descend(bool start)
        {
            Logging.WriteFileOnly("Descend(" + start + ")");
            Lua.LuaDoString(start ? "SitStandOrDescendStart();" : "DescendStop();");
        }

        public static void MoveBackward(bool start)
        {
            Logging.WriteFileOnly("MoveBackward(" + start + ")");
            Lua.LuaDoString(start ? "MoveBackwardStart();" : "MoveBackwardStop();");
        }

        public static void MoveForward(bool start)
        {
            Logging.WriteFileOnly("MoveForward(" + start + ")");
            Lua.LuaDoString(start ? "MoveForwardStart();" : "MoveForwardStop();");
        }

        public static void StrafeLeft(bool start)
        {
            Logging.WriteFileOnly("StrafeLeft(" + start + ")");
            Lua.LuaDoString(start ? "StrafeLeftStart();" : "StrafeLeftStop();");
        }

        public static void StrafeRight(bool start)
        {
            Logging.WriteFileOnly("StrafeRight(" + start + ")");
            Lua.LuaDoString(start ? "StrafeRightStart();" : "StrafeRightStop();");
        }
    }
}