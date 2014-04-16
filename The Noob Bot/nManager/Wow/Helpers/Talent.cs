using System;
using System.Threading;
using System.Windows.Forms;
using nManager.Helpful;

namespace nManager.Wow.Helpers
{
    /// <summary>
    /// Talents
    /// </summary>
    public static class Talent
    {
        private static int GetNumUnspentTalents()
        {
            string res = Others.GetRandomString(Others.Random(4, 10));
            int numUnspent = Others.ToInt32(Lua.LuaDoString(res + " = GetNumUnspentTalents()", res));
            return numUnspent;
        }

        public static void DoTalents()
        {
            try
            {
                if (GetNumUnspentTalents() == 0)
                    return;
                if (Others.ExistFile(Application.StartupPath + "\\CombatClasses\\Talents\\" + ObjectManager.ObjectManager.Me.WowSpecialization() + ".talents.txt"))
                {
                    Lua.RunMacroText("/click PlayerTalentFrameCloseButton"); // Make sure it's already closed.
                    Thread.Sleep(400);
                    Lua.RunMacroText("/click TalentMicroButton");
                    string advised = Others.ReadFile(Application.StartupPath + "\\CombatClasses\\Talents\\" + ObjectManager.ObjectManager.Me.WowSpecialization() + ".talents.txt");
                    var talents = advised.Split('|');
                    foreach (string s in talents)
                    {
                        Lua.LuaDoString("PlayerTalentFrame_SelectTalent(" + s + ")");
                    }

                    Lua.RunMacroText("/click PlayerTalentFrameTalentsLearnButton");
                    Thread.Sleep(400);
                    Lua.RunMacroText("/click PlayerTalentFrameCloseButton");
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("DoTalents(): " + exception);
            }
        }
    }
}