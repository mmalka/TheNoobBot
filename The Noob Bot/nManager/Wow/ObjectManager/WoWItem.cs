using System;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.Helpers;
using nManager.Wow.Patchables;

namespace nManager.Wow.ObjectManager
{
    public class WoWItem : WoWObject
    {
        public WoWItem(uint address)
            : base(address)
        {
        }

        public override string Name
        {
            get
            {
                try
                {
                    lock (this)
                    {
                        string randomStringResult = Others.GetRandomString(Others.Random(4, 10));
                        Lua.LuaDoString(randomStringResult + ", _, _, _, _, _, _, _ = GetItemInfo(" + Entry + ")");
                        return Lua.GetLocalizedText(randomStringResult);
                    }
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWItem > Name: " + e);
                }
                return "";
            }
        }

        private ItemInfo _itemInfo;

        public ItemInfo GetItemInfo
        {
            get
            {
                try
                {
                    return _itemInfo ?? (_itemInfo = new ItemInfo(Entry));
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWItem > GetItemInfo: " + e);
                    return new ItemInfo(0);
                }
            }
        }

        public bool IsEquippableItem
        {
            get
            {
                try
                {
                    string sResult;
                    lock (this)
                    {
                        string randomStringResult = Others.GetRandomString(Others.Random(4, 10));
                        Lua.LuaDoString(randomStringResult + " = IsEquippableItem(" + Entry + ")");
                        sResult = Lua.GetLocalizedText(randomStringResult);
                    }

                    return (sResult != "");
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWItem > IsEquippableItem: " + e);
                    return false;
                }
            }
        }

        public string SpellName
        {
            get
            {
                try
                {
                    string randomString = Others.GetRandomString(Others.Random(4, 10));
                    Lua.LuaDoString(randomString + ",_ = GetItemSpell(" + Name + ")");
                    string sResult = Lua.GetLocalizedText(randomString);
                    if (sResult != string.Empty && sResult != "nil")
                        return sResult;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWItem > SpellName: " + exception);
                }
                return "";
            }
        }

        public ulong Owner
        {
            get { return GetDescriptor<ulong>(GetBaseAddress, (uint) Descriptors.ItemFields.Owner); }
        }

        public T GetDescriptor<T>(Descriptors.ItemFields field) where T : struct
        {
            try
            {
                return GetDescriptor<T>((uint) field);
            }
            catch (Exception e)
            {
                Logging.WriteError("WoWItem > GetDescriptor<T>(Descriptors.ItemFields field): " + e);
                return default(T);
            }
        }
    }
}