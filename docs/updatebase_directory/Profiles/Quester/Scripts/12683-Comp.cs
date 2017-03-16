if(ObjectManager.Me.HaveBuff(52307))
{
	if (ItemsManager.GetItemCount(39164) <= 0 || ItemsManager.IsItemOnCooldown(39164) || !ItemsManager.IsItemUsable(39164))
		return false;
	ItemsManager.UseItem(ItemsManager.GetItemNameById(39164));
}	

return nManager.Wow.Helpers.Quest.GetLogQuestIsComplete(12683) || nManager.Wow.Helpers.Quest.IsQuestFlaggedCompletedLUA(12683);