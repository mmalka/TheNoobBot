using nManager.Helpful;

namespace nManager.Wow.Helpers
{
    public class MovementsAction
    {
        public static bool UseLUAToMove = false; // RECOMMANDED AS HELL until I understand why LUA don't wanna work as intended.

        public static void Jump()
        {
            Ascend(true, true);
        }

        public static void Ascend(bool start, bool redo = false)
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
            if (redo)
            {
                Ascend(!start);
            }
        }

        public static void Descend(bool start, bool redo = false)
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
            if (redo)
            {
                Descend(!start);
            }
        }

        public static void MoveBackward(bool start, bool redo = false)
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
            if (redo)
            {
                MoveBackward(!start);
            }
        }

        public static void MoveForward(bool start, bool redo = false)
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
            if (redo)
            {
                MoveForward(!start);
            }
        }

        public static void StrafeLeft(bool start, bool redo = false)
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
            if (redo)
            {
                StrafeLeft(!start);
            }
        }

        public static void StrafeRight(bool start, bool redo = false)
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
            if (redo)
            {
                StrafeRight(!start);
            }
        }
    }
}