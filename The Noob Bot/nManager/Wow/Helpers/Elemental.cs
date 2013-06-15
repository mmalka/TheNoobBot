using System;
using System.Windows.Forms;
using nManager.Helpful;

namespace nManager.Wow.Helpers
{
    public class Elemental
    {
        public static void AutoMakeElemental()
        {
            try
            {
                if (Others.ExistFile(Application.StartupPath + "\\Data\\autoMakeElementalMacro.txt"))
                {
                    string codeLua =
                        Others.ReadFile(Application.StartupPath + "\\Data\\autoMakeElementalMacro.txt")
                              .Replace(Environment.NewLine, " ");
                    Lua.LuaDoString(codeLua);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("AutoMakeElemental(): " + exception);
            }
        }
    }
}