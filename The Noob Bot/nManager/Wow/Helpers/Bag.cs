using System;
using System.Collections.Generic;
using nManager.Helpful;
using nManager.Wow.ObjectManager;
using nManager.Wow.Patchables;

namespace nManager.Wow.Helpers
{
    public class Bag
    {
        public static List<WoWItem> GetBagItem()
        {
            try
            {
                string randomString = Others.GetRandomString(Others.Random(4, 10));

                List<WoWItem> bagItem = new List<WoWItem>();

                string luaCommand = "";
                luaCommand = luaCommand + "l=0 ";
                luaCommand = luaCommand + "" + randomString + " = \"\" ";
                luaCommand = luaCommand + "ItemLinkT = \"\" ";
                luaCommand = luaCommand + "for b=0,4 do ";
                luaCommand = luaCommand + "for s=1,22 do ";
                luaCommand = luaCommand + "l=GetContainerItemLink(b,s) ";
                luaCommand = luaCommand + "if l then ";
                luaCommand = luaCommand + "ItemLinkT = GetContainerItemLink(b,s) ";
                luaCommand = luaCommand + "" + randomString + " = " + randomString + " .. ItemLinkT .. \"^\" ";
                luaCommand = luaCommand + "end ";
                luaCommand = luaCommand + "end ";
                luaCommand = luaCommand + "end ";

                string listLinkItem;

                lock (typeof (Bag))
                {
                    Lua.LuaDoString(luaCommand);

                    listLinkItem = Lua.GetLocalizedText(randomString);
                }

                List<uint> itemId = new List<uint>();
                string[] linkItemArray = listLinkItem.Split('^');

                foreach (string sR in linkItemArray)
                {
                    if (sR != "")
                    {
                        try
                        {
                            itemId.Add(Others.ToUInt32(sR.Split(':')[1]));
                        }
                        catch (Exception exception)
                        {
                            Logging.WriteError("GetBagItem()#1: " + exception);
                        }
                    }
                }

                if (itemId.Count > 0)
                {
                    List<WoWItem> objects = ObjectManager.ObjectManager.GetObjectWoWItem();
                    List<int> emptyBlackList = new List<int>();
                    foreach (WoWItem o in objects)
                    {
                        try
                        {
                            if (o.Type == Enums.WoWObjectType.Item)
                            {
                                uint itemIdTemp = ObjectManager.ObjectManager.Me.GetDescriptor<uint>(o.GetBaseAddress,
                                                                                                    (uint)
                                                                                                    Descriptors
                                                                                                        .ObjectFields
                                                                                                        .Entry);
                                ulong itemGuidOwner = ObjectManager.ObjectManager.Me.GetDescriptor<ulong>(
                                    o.GetBaseAddress, (uint) Descriptors.ItemFields.Owner);

                                if (itemId.Contains(itemIdTemp) && itemGuidOwner == ObjectManager.ObjectManager.Me.Guid &&
                                    !emptyBlackList.Contains(o.Entry))
                                {
                                    bagItem.Add(o);
                                    emptyBlackList.Add(o.Entry);
                                }
                            }
                        }
                        catch (Exception exception)
                        {
                            Logging.WriteError("GetBagItem()#2: " + exception);
                        }
                    }
                }

                return bagItem;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetBagItem()#3: " + exception);
            }
            return new List<WoWItem>();
        }

        public static bool ItemIsInBag(string name)
        {
            try
            {
                string randomString = Others.GetRandomString(Others.Random(4, 10));

                string scriptLua = "local c,l,r,_=0 ";
                scriptLua = scriptLua + randomString + " = \"False\" ";
                scriptLua = scriptLua + "for b=0,4 do ";
                scriptLua = scriptLua + "for s=1,40 do  ";
                scriptLua = scriptLua + "local l=GetContainerItemLink(b,s) ";
                scriptLua = scriptLua + "if l then namei,_,r=GetItemInfo(l) ";
                scriptLua = scriptLua + "if namei == " + name + " then ";
                scriptLua = scriptLua + randomString + " = \"True\" ";
                scriptLua = scriptLua + " end ";
                scriptLua = scriptLua + "end ";
                scriptLua = scriptLua + "end ";
                scriptLua = scriptLua + "end ";


                lock (typeof (Bag))
                {
                    Lua.LuaDoString(scriptLua);
                    return Lua.GetLocalizedText(randomString) == "True";
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("ItemIsInBag(string name): " + exception);
                return false;
            }
        }

        public static int NumFreeSlots
        {
            get { return Usefuls.GetContainerNumFreeSlots; }
        }
    }
}