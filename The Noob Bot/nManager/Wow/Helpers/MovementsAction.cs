namespace nManager.Wow.Helpers
{
    public class MovementsAction
    {
        public static void Jump()
        {
            Lua.LuaDoString("JumpOrAscendStart();");
            Lua.LuaDoString("AscendStop();");
        }

        public static void Ascend(bool start)
        {
            Lua.LuaDoString(start ? "JumpOrAscendStart();" : "AscendStop();");
        }

        public static void Descend(bool start)
        {
            Lua.LuaDoString(start ? "SitStandOrDescendStart();" : "DescendStop();");
        }

        public static void MoveBackward(bool start)
        {
            Lua.LuaDoString(start ? "MoveBackwardStart();" : "MoveBackwardStop();");
        }

        public static void MoveForward(bool start)
        {
            Lua.LuaDoString(start ? "MoveForwardStart();" : "MoveForwardStop();");
        }

        public static void StrafeLeft(bool start)
        {
            Lua.LuaDoString(start ? "StrafeLeftStart();" : "StrafeLeftStop();");
        }

        public static void StrafeRight(bool start)
        {
            Lua.LuaDoString(start ? "StrafeRightStart();" : "StrafeRightStop();");
        }
    }
}