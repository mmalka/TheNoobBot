using System;
using System.Collections.Generic;
using nManager.Helpful;
using nManager.Wow.Class;

namespace nManager.Wow.Helpers
{
    public static class Milling
    {
        public static void Pulse(List<string> items)
        {
            try
            {
                var spell = new Spell("Milling");
                if (!spell.KnownSpell)
                    return;

                var itemArray = "";
                foreach (var i in items)
                {
                    if (!string.IsNullOrWhiteSpace(i) && i.Contains("'"))
                    {
                        items.Add(i.Replace("'", "’"));
                    }
                    if (ItemsManager.GetItemCountByNameLUA(i) >= 5)
                    {
                        if (!string.IsNullOrEmpty(itemArray))
                            itemArray = itemArray + ", ";
                        itemArray = itemArray + "\"" + i + "\"";
                    }
                }
                if (string.IsNullOrEmpty(itemArray))
                {
                    Logging.Write("Milling interrupted, no items founds from the list, check that the names are correctly typed and that you have at least 5 of them.");
                    return;
                }
                var macro =
                    "myTable = {" + itemArray + "} " +
                    "for key,value in pairs(myTable) do " +
                    "	itemsToMil = value " +
                    "	_, itemLink = GetItemInfo(itemsToMil) " +
                    "	_, _, Color, Ltype, Id, Enchant, Gem1, Gem2, Gem3, Gem4, Suffix, Unique, LinkLvl, Name = string.find(itemLink, \"|?c?f?f?(%x*)|?H?([^:]*):?(%d+):?(%d*):?(%d*):?(%d*):?(%d*):?(%d*):?(%-?%d*):?(%-?%d*):?(%d*)|?h?%[?([^%[%]]*)%]?|?h?|?r?\") " +
                    "	fisrtI = -1 " +
                    "	fisrtJ = -1 " +
                    "	if tonumber(Id) > 0 then " +
                    "		for i=0,4 do " +
                    "			 for j=1,GetContainerNumSlots(i)do " +
                    "				idT = GetContainerItemID(i,j) " +
                    "				 if tonumber(Id) == idT then " +
                    "					_, itemCount, _, _, _ = GetContainerItemInfo(i,j); " +
                    "					if tonumber(itemCount) >=5 then " +
                    "						CastSpellByName(\"" + spell.NameInGame + "\"); " +
                    "						UseContainerItem(i,j) " +
                    "					end " +
                    "				 end " +
                    "			 end " +
                    "		end " +
                    "	end " +
                    "end";

                Lua.LuaDoString(macro);
            }
            catch (Exception exception)
            {
                Logging.WriteError("Milling > Pulse(List<string> items): " + exception);
            }
        }

        public static bool NeedRun(List<string> items)
        {
            try
            {
                var spell = new Spell("Milling");
                if (!spell.KnownSpell)
                    return false;

                var itemArray = "";
                foreach (var i in items)
                {
                    if (!string.IsNullOrWhiteSpace(i) && i.Contains("'"))
                    {
                        items.Add(i.Replace("'", "’"));
                    }
                    if (ItemsManager.GetItemCountByNameLUA(i) >= 5)
                    {
                        if (!string.IsNullOrEmpty(itemArray))
                            itemArray = itemArray + ", ";
                        itemArray = itemArray + "\"" + i + "\"";
                    }
                }
                if (string.IsNullOrEmpty(itemArray))
                {
                    Logging.Write("Milling interrupted, no items founds from the list, check that the names are correctly typed and that you have at least 5 of them.");
                    return false;
                }
                var macro =
                    "myTable = {" + itemArray + "} " +
                    "needRun = \"false\" " +
                    "for key,value in pairs(myTable) do " +
                    "	itemsToMil = value " +
                    "	_, itemLink = GetItemInfo(itemsToMil) " +
                    "	_, _, Color, Ltype, Id, Enchant, Gem1, Gem2, Gem3, Gem4, Suffix, Unique, LinkLvl, Name = string.find(itemLink, \"|?c?f?f?(%x*)|?H?([^:]*):?(%d+):?(%d*):?(%d*):?(%d*):?(%d*):?(%d*):?(%-?%d*):?(%-?%d*):?(%d*)|?h?%[?([^%[%]]*)%]?|?h?|?r?\") " +
                    "	fisrtI = -1 " +
                    "	fisrtJ = -1 " +
                    "	if tonumber(Id) > 0 then " +
                    "		for i=0,4 do " +
                    "			 for j=1,GetContainerNumSlots(i)do " +
                    "				idT = GetContainerItemID(i,j) " +
                    "				 if tonumber(Id) == idT then " +
                    "					_, itemCount, _, _, _ = GetContainerItemInfo(i,j); " +
                    "					if tonumber(itemCount) >=5 then " +
                    "						needRun = \"true\"" +
                    "						return " +
                    "					end " +
                    "				 end " +
                    "			 end " +
                    "		end " +
                    "	end " +
                    "end";
                Lua.LuaDoString(macro);
                return Lua.GetLocalizedText("needRun") == "true";
            }
            catch (Exception exception)
            {
                Logging.WriteError("Milling > NeedRun(List<string> items): " + exception);
                return false;
            }
        }
    }
}