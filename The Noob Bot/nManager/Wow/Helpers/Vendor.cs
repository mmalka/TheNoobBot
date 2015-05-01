using System;
using System.Collections.Generic;
using System.Linq;
using nManager.Helpful;

namespace nManager.Wow.Helpers
{
    public class Vendor
    {
        public static void RepairAllItems()
        {
            try
            {
                Lua.LuaDoString("RepairAllItems()");
            }
            catch (Exception e)
            {
                Logging.WriteError("RepairAllItems(): " + e);
            }
        }

        public static void SellItems(List<String> itemSell, List<string> itemNoSell,
            List<Enums.WoWItemQuality> itemQuality)
        {
            try
            {
                string syntaxSellItem = itemSell.Aggregate("", (current, s) => current + " or namei == \"" + s + "\" ");

                string syntaxQualityItem = itemQuality.Aggregate(" 1 == 2 ",
                    (current, s) => current + " or r == " + (uint) s + " ");

                string syntaxNoSellItem = "";
                string syntaxNoSellItemEnd = "";
                if (itemNoSell.Count > 0)
                {
                    syntaxNoSellItemEnd = " end ";
                    syntaxNoSellItem = itemNoSell.Aggregate(" if ",
                        (current, s) => current + " and namei ~= \"" + s + "\" ");
                    syntaxNoSellItem = syntaxNoSellItem.Replace("if  and", "if ");
                    syntaxNoSellItem = syntaxNoSellItem + " then ";
                }

                string scriptLua = "";

                scriptLua = scriptLua + "local c,l,r,_=0 ";

                scriptLua = scriptLua + "for b=0,4 do ";
                scriptLua = scriptLua + "for s=1,GetContainerNumSlots(b) do  ";
                scriptLua = scriptLua + "local l=GetContainerItemLink(b,s) ";
                scriptLua = scriptLua + "if l then namei,_,r=GetItemInfo(l) ";
                scriptLua = scriptLua + "if " + syntaxQualityItem + " " + syntaxSellItem + " then ";
                scriptLua = scriptLua + syntaxNoSellItem;
                scriptLua = scriptLua + " UseContainerItem(b,s)c=c+1 ";
                scriptLua = scriptLua + syntaxNoSellItemEnd;
                scriptLua = scriptLua + " end ";
                scriptLua = scriptLua + "end ";
                scriptLua = scriptLua + "end ";
                scriptLua = scriptLua + "end ";

                Lua.LuaDoString(scriptLua);
                System.Threading.Thread.Sleep(30000);
            }
            catch (Exception e)
            {
                Logging.WriteError(
                    "SellItems(List<String> itemSell, List<string> itemNoSell, List<Enums.WoWItemQuality> itemQuality): " +
                    e);
            }
        }

        public static void BuyItem(string name, int number)
        {
            try
            {
                Lua.LuaDoString(
                    "function buy(n,q) for i=1,100 do if n==GetMerchantItemInfo(i) then BuyMerchantItem(i,q) end end end name = \"" +
                    name + "\" buy (name," + number + ")");
            }
            catch (Exception e)
            {
                Logging.WriteError("BuyItem(string name, int number): " + e);
            }
        }
    }
}