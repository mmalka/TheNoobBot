using System;
using System.Threading;
using nManager.Helpful;

namespace nManager.Wow.Helpers
{
    public class Trainer
    {
        public static void TrainingSpell()
        {
            try
            {
                Lua.RunMacroText("/click GossipTitleButton3");
                Thread.Sleep(1000);
                Lua.LuaDoString("LoadAddOn\"Blizzard_TrainerUI\" f=ClassTrainerTrainButton f.e = 0 if f:GetScript\"OnUpdate\" then f:SetScript(\"OnUpdate\", nil)else f:SetScript(\"OnUpdate\", function(f,e) f.e=f.e+e if f.e>.01 then f.e=0 f:Click() end end)end");
                Thread.Sleep(1000);
                Lua.RunMacroText("/click ClassTrainerFrameCloseButton");
            }
            catch (Exception exception)
            {
                Logging.WriteError("TrainingSpell(): " + exception);
            }
        }
    }
}
