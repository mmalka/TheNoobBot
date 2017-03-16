if(nManager.Wow.Helpers.ItemsManager.GetItemCount(35792) >= 1)
	ItemsManager.UseItem(ItemsManager.GetItemNameById(35792));
	
Thread.Sleep(1000);
	
return nManager.Wow.Helpers.Quest.IsObjectiveCompleted(11999, 1, 1) || nManager.Wow.Helpers.Quest.GetLogQuestIsComplete(11999) || nManager.Wow.Helpers.Quest.GetQuestCompleted(11999);

