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
                Thread.Sleep(1000);
                Lua.LuaDoString("for i=1,GetNumGossipOptions() do local _,n=GetGossipOptions(); if n == \"trainer\" then SelectGossipOption(i) end end");
                Thread.Sleep(1000);
                Lua.LuaDoString("SetTrainerServiceTypeFilter(\"available\",1);");
                Lua.LuaDoString("SetTrainerServiceTypeFilter(\"unavailable\",0);");
                Lua.LuaDoString("SetTrainerServiceTypeFilter(\"used\",0);");
                Thread.Sleep(1000);
                Lua.LuaDoString("for i=0,GetNumTrainerServices(),1 do BuyTrainerService(1); end");
                Thread.Sleep(500);
                Lua.LuaDoString("CloseTrainer()");
            }
            catch (Exception exception)
            {
                Logging.WriteError("TrainingSpell(): " + exception);
            }
        }
    }
}
