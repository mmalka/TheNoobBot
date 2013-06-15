using System;
using nManager.Helpful;
using nManager.Wow.Helpers;

namespace nManager.Wow.Class
{
    /// <summary>
    /// Item Information Class
    /// </summary>
    public class ItemInfo
    {
        public string ItemEquipLoc { get; private set; }
        public int ItemLevel { get; private set; }
        public string ItemLink { get; private set; }
        public int ItemMinLevel { get; private set; }
        public string ItemName { get; private set; }
        public int ItemRarity { get; private set; }
        public int ItemSellPrice { get; private set; }
        public int ItemStackCount { get; private set; }
        public string ItemSubType { get; private set; }
        public string ItemTexture { get; private set; }
        public string ItemType { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemInfo"/> class.
        /// </summary>
        /// <param name="entryId">The entry id.</param>
        public ItemInfo(int entryId)
        {
            try
            {
                string sResult;
                lock (this)
                {
                    string randomString = Others.GetRandomString(Others.Random(4, 10));
                    Lua.LuaDoString(
                        "itemName, itemLink, itemRarity, itemLevel, itemMinLevel, itemType, itemSubType, itemStackCount, itemEquipLoc, itemTexture, itemSellPrice = GetItemInfo(" +
                        entryId + ") " + randomString +
                        " = itemName .. \"^\" .. itemLink .. \"^\" .. itemRarity .. \"^\" .. itemLevel .. \"^\" .. itemMinLevel .. \"^\" .. itemType .. \"^\" .. itemSubType .. \"^\" .. itemStackCount .. \"^\" .. itemEquipLoc .. \"^\" .. itemTexture .. \"^\" .. itemSellPrice");
                    sResult = Lua.GetLocalizedText(randomString);
                }
                string[] itemInfoArray = sResult.Split(Convert.ToChar("^"));

                ItemName = itemInfoArray[0];
                ItemLink = itemInfoArray[1];
                ItemRarity = Others.ToInt32(itemInfoArray[2]);
                ItemLevel = Others.ToInt32(itemInfoArray[3]);
                ItemMinLevel = Others.ToInt32(itemInfoArray[4]);
                ItemType = itemInfoArray[5];
                ItemSubType = itemInfoArray[6];
                ItemStackCount = Others.ToInt32(itemInfoArray[7]);
                ItemEquipLoc = itemInfoArray[8];
                ItemTexture = itemInfoArray[9];
                ItemSellPrice = Others.ToInt32(itemInfoArray[10]);
            }
            catch (Exception exception)
            {
                Logging.WriteError("ItemInfo(int entryId): " + exception);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemInfo"/> class.
        /// </summary>
        /// <param name="nameItem">The name item.</param>
        public ItemInfo(string nameItem)
        {
            try
            {
                string sResult;
                lock (this)
                {
                    string randomString = Others.GetRandomString(Others.Random(4, 10));
                    Lua.LuaDoString(
                        "itemName, itemLink, itemRarity, itemLevel, itemMinLevel, itemType, itemSubType, itemStackCount, itemEquipLoc, itemTexture, itemSellPrice = GetItemInfo(\"" +
                        nameItem + "\")  " + randomString +
                        " = itemName .. \"^\" .. itemLink .. \"^\" .. itemRarity .. \"^\" .. itemLevel .. \"^\" .. itemMinLevel .. \"^\" .. itemType .. \"^\" .. itemSubType .. \"^\" .. itemStackCount .. \"^\" .. itemEquipLoc .. \"^\" .. itemTexture .. \"^\" .. itemSellPrice");
                    sResult = Lua.GetLocalizedText(randomString);
                }
                string[] intemInfoArray = sResult.Split(Convert.ToChar("^"));

                ItemName = intemInfoArray[0];
                ItemLink = intemInfoArray[1];
                ItemRarity = Others.ToInt32(intemInfoArray[2]);
                ItemLevel = Others.ToInt32(intemInfoArray[3]);
                ItemMinLevel = Others.ToInt32(intemInfoArray[4]);
                ItemType = intemInfoArray[5];
                ItemSubType = intemInfoArray[6];
                ItemStackCount = Others.ToInt32(intemInfoArray[7]);
                ItemEquipLoc = intemInfoArray[8];
                ItemTexture = intemInfoArray[9];
                ItemSellPrice = Others.ToInt32(intemInfoArray[10]);
            }
            catch (Exception exception)
            {
                Logging.WriteError("ItemInfo(string nameItem): " + exception);
            }
        }
    }
}