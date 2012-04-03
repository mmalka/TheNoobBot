using System;
using nManager.Helpful;
using nManager.Wow.Class;


namespace nManager.Wow.Helpers
{
    public static class Smelting
    {
        public static void Pulse()
        {
            try
            {
                var macro =
                   "numTrade = GetNumTradeSkills(); " +
                   "firstTrade = GetFirstTradeSkill(); " +
                   "while numTrade>=firstTrade do " +
                   "  skillName, skillType, numAvailable, isExpanded, altVerb, numSkillUps = GetTradeSkillInfo(numTrade); " +
                   "  if numAvailable > 0 then " +
                   "    SelectTradeSkill(numTrade); " +
                   "    RunMacroText(\"/click TradeSkillCreateAllButton\"); " +
                   "    return; " +
                   "  end " +
                   "  numTrade = numTrade - 1; " +
                   "end";
                Lua.LuaDoString(macro);
            }
            catch (Exception exception)
            {
                Logging.WriteError("Smelting > Pulse(): " + exception);
            }
        }

        public static void OpenSmeltingWindow()
        {
            try
            {
                var smeltingSpell = new Spell("Smelting");
                if (!smeltingSpell.KnownSpell)
                    return;

                var macro =
                    "RunMacroText(\"/click TradeSkillFrameCloseButton\"); " +
                    "CastSpellByName(\"" + smeltingSpell.NameInGame + "\"); ";
                    Lua.LuaDoString(macro);
            }
            catch (Exception exception)
            {
                Logging.WriteError("Smelting > OpenSmeltingWindow(): " + exception);
                return;
            }
        }

        public static void CloseSmeltingWindow()
        {
            try
            {
                var macro =
                "RunMacroText(\"/click TradeSkillFrameCloseButton\"); ";
                Lua.LuaDoString(macro);
            }
            catch (Exception exception)
            {
                Logging.WriteError("Smelting > OpenSmeltingWindow(): " + exception);
                return;
            }
        }

        public static bool NeedRun(bool openWindow = true)
        {
            try
            {
                var smeltingSpell = new Spell("Smelting");
                if (!smeltingSpell.KnownSpell)
                    return false;

                var macro = "";

                if (openWindow)
                {
                    macro = macro + "RunMacroText(\"/click TradeSkillFrameCloseButton\"); " +
                    "CastSpellByName(\"" + smeltingSpell.NameInGame + "\");";
                }
                macro = macro +
                        "needSmelting = \"\"; " +
                        "numTrade = GetNumTradeSkills(); " +
                        "firstTrade = GetFirstTradeSkill(); " +
                        "while numTrade>=firstTrade do " +
                        "  skillName, skillType, numAvailable, isExpanded, altVerb, numSkillUps = GetTradeSkillInfo(numTrade); " +
                        "       if numAvailable > 0 then " +
                        "         needSmelting = \"true\"; " +
                        "         numTrade =  firstTrade; " +
                        "       end " +
                        "  numTrade = numTrade - 1; " +
                        "end ";
                if (openWindow)
                    macro = macro + "RunMacroText(\"/click TradeSkillFrameCloseButton\"); ";

                Lua.LuaDoString(macro);
                if (Lua.GetLocalizedText("needSmelting") == "true")
                    return true;
            }
            catch (Exception exception)
            {
                Logging.WriteError("Smelting > NeedRun(): " + exception);
            }
            return false;
        }
    }
}
