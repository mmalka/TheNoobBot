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
        private static void ReadAndRunTalent()
        {
            try
            {
                if (Others.ExistFile(Application.StartupPath + "\\CustomClasses\\Talents\\" +
                                     ObjectManager.ObjectManager.Me.WowClass + ".macro.txt"))
                {
                    Lua.RunMacroText("/click TalentMicroButton");
                    string macro =
                        Others.ReadFile(Application.StartupPath + "\\CustomClasses\\Talents\\" +
                                        ObjectManager.ObjectManager.Me.WowClass + ".macro.txt");

                    if (macro.Replace(" ", "") != "")
                        Lua.RunMacroText(macro);
                    else
                        return;

                    Lua.RunMacroText("/click PlayerTalentFrameLearnButton");
                    Thread.Sleep(400);
                    Lua.RunMacroText("/click StaticPopup1Button1");
                    Thread.Sleep(400);
                    Lua.RunMacroText("/click TalentMicroButton");
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("ReadAndRunTalent(): " + exception);
            }
        }

        /// <summary>
        /// Does the talents.
        /// </summary>
        public static void DoTalents()
        {
            try
            {
                ReadAndRunTalent();
            }
            catch (Exception exception)
            {
                Logging.WriteError("DoTalents(): " + exception);
            }
        }
    }
}