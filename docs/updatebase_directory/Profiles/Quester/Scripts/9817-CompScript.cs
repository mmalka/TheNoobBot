Point p = new Point(-239.0211f,5497.871f,25.79529f);

return ObjectManager.Me.Position.DistanceTo(p) <= 5f || nManager.Wow.Helpers.Quest.GetLogQuestId().Contains(9817) ||
 nManager.Wow.Helpers.Quest.GetLogQuestIsComplete(9817) 
 || nManager.Wow.Helpers.Quest.GetQuestCompleted(9817);