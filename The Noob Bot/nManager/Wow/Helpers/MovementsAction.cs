using nManager.Helpful;

namespace nManager.Wow.Helpers
{
    public class MovementsAction
    {
        public static bool UseLUAToMove = false; // RECOMMANDED AS HELL until I understand why LUA don't wanna work as intended.

        public static void Jump()
        {
            Ascend(true);
            Ascend(false);
        }

        public static void Ascend(bool start)
        {
            Logging.WriteFileOnly("Ascend(" + start + ")");
            if (UseLUAToMove)
            {
                Lua.LuaDoString(start ? "JumpOrAscendStart();" : "AscendStop();");
            }
            else
            {
                if (start)
                    Keybindings.DownKeybindings(Enums.Keybindings.JUMP);
                else
                    Keybindings.UpKeybindings(Enums.Keybindings.JUMP);
            }
        }

        public static void Descend(bool start)
        {
            Logging.WriteFileOnly("Descend(" + start + ")");
            if (UseLUAToMove)
            {
                Lua.LuaDoString(start ? "SitStandOrDescendStart();" : "DescendStop();");
            }
            else
            {
                if (start)
                    Keybindings.DownKeybindings(Enums.Keybindings.SITORSTAND);
                else
                    Keybindings.UpKeybindings(Enums.Keybindings.SITORSTAND);
            }
        }

        public static void MoveBackward(bool start)
        {
            Logging.WriteFileOnly("MoveBackward(" + start + ")");
            if (UseLUAToMove)
            {
                Lua.LuaDoString(start ? "MoveBackwardStart();" : "MoveBackwardStop();");
            }
            else
            {
                if (start)
                    Keybindings.DownKeybindings(Enums.Keybindings.MOVEBACKWARD);
                else
                    Keybindings.UpKeybindings(Enums.Keybindings.MOVEBACKWARD);
            }
        }

        public static void MoveForward(bool start)
        {
            Logging.WriteFileOnly("MoveForward(" + start + ")");
            if (UseLUAToMove)
            {
                Lua.LuaDoString(start ? "MoveForwardStart();" : "MoveForwardStop();");
            }
            else
            {
                if (start)
                    Keybindings.DownKeybindings(Enums.Keybindings.MOVEFORWARD);
                else
                    Keybindings.UpKeybindings(Enums.Keybindings.MOVEFORWARD);
            }
        }

        public static void StrafeLeft(bool start)
        {
            Logging.WriteFileOnly("StrafeLeft(" + start + ")");
            if (UseLUAToMove)
            {
                Lua.LuaDoString(start ? "StrafeLeftStart();" : "StrafeLeftStop();");
            }
            else
            {
                if (start)
                    Keybindings.DownKeybindings(Enums.Keybindings.STRAFELEFT);
                else
                    Keybindings.UpKeybindings(Enums.Keybindings.STRAFELEFT);
            }
        }

        public static void StrafeRight(bool start)
        {
            Logging.WriteFileOnly("StrafeRight(" + start + ")");
            if (UseLUAToMove)
            {
                Lua.LuaDoString(start ? "StrafeRightStart();" : "StrafeRightStop();");
            }
            else
            {
                if (start)
                    Keybindings.DownKeybindings(Enums.Keybindings.STRAFERIGHT);
                else
                    Keybindings.UpKeybindings(Enums.Keybindings.STRAFERIGHT);
            }
        }
    }
}