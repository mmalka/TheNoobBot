using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using nManager;
using nManager.Helpful;
using nManager.Products;
using nManager.Wow.Class;
using Profiles_Converters;
using Profiles_Converters.Converters;
using Profiles_Converters.WebParser;
using Quester.Profile;
using Quest = Profiles_Converters.Converters.Quest;

public class Main : IProduct
{
    #region IProduct Members

    private readonly List<KeyValuePair<int, uint>> _itemPickUpList = new List<KeyValuePair<int, uint>>(); // QuestId, Quest.ItemPickUp 
    private readonly List<KeyValuePair<int, uint>> _pickUpList = new List<KeyValuePair<int, uint>>(); // QuestId, Quest.PickUp
    private readonly QuesterProfile _tnbProfile = new QuesterProfile();
    private readonly List<Npc> _tnbTmpNpcList = new List<Npc>();
    private readonly List<KeyValuePair<int, uint>> _turnInList = new List<KeyValuePair<int, uint>>(); // QuestId, Quest.TurnIn
    private bool _isStarted;
    private MainForm formMain;

    public void Initialize()
    {
        try
        {
            Others.ProductStatusLog(Products.ProductName, 1);
        }
        catch (Exception e)
        {
            Logging.WriteError("Profiles Converters > Main > Initialize(): " + e);
        }
    }

    public void Dispose()
    {
        try
        {
            Stop();
            Others.ProductStatusLog(Products.ProductName, 2);
        }
        catch (Exception e)
        {
            Logging.WriteError("Profiles Converters > Main > Dispose(): " + e);
        }
    }

    public void Start()
    {
        try
        {
            Stopwatch timer = Stopwatch.StartNew();
            var hbProfile = XmlSerializer.Deserialize<HBProfile>(Application.StartupPath + @"\[A - Quest] 85-86 The Jade Forest [Kick].xml");
            if (hbProfile.Items == null || !hbProfile.Items.Any())
            {
                return;
            }
            int count = hbProfile.Items.Length;
            if (count > hbProfile.ItemsElementName.Length)
                count = hbProfile.ItemsElementName.Length;
            for (int i = 0; i < count; i++)
            {
                ItemsChoiceType2 name = hbProfile.ItemsElementName[i];
                object value = hbProfile.Items[i];
                Logging.Write(name + ": " + value);
                if (value.ToString().Contains("Profiles_Converters.Converters"))
                {
                    /*if (value is Vendors)
                    {
                        Vendors vendors = value as Vendors;
                        foreach (var vendor in vendors.Items)
                        {
                            Logging.Write(vendor.Entry + ";" + vendor.Name + ";" + vendor.Nav + ";" + vendor.Type + ";" + vendor.X + ";" + vendor.Y + ";" + vendor.Z);
                        }
                    }
                    else if (value is Mailboxes)
                    {
                        Mailboxes mailboxes = value as Mailboxes;
                        foreach (var mailbox in mailboxes.Items)
                        {
                            Logging.Write(mailbox.Nav + ";" + mailbox.X + ";" + mailbox.Y + ";" + mailbox.Z);
                        }
                    }
                    else */
                    if (value is Blackspots)
                    {
                        var blackspots = value as Blackspots;
                        foreach (BlackspotType blackspot in blackspots.Blackspot)
                        {
                            _tnbProfile.Blackspots.Add(new QuesterBlacklistRadius
                            {
                                Position = new Point {X = blackspot.X, Y = blackspot.Y, Z = blackspot.Z, Type = ""},
                                Radius = blackspot.Radius
                            });
                            //Logging.Write(blackspot.X + ";" + blackspot.Y + ";" + blackspot.Z + ";" + blackspot.Radius + ";" + blackspot.Height);
                        }
                    }
                        // avoidmob dosn't gather a valid Entry yet.
                    else if (value is AvoidMobs)
                    {
                        var avoidMobs = value as AvoidMobs;
                        foreach (MobType avoidMob in avoidMobs.Items)
                        {
                            _tnbProfile.AvoidMobs.Add(new Npc
                            {
                                Entry = (int) avoidMob.Id,
                                Name = avoidMob.Name
                            });
                            //Logging.Write(avoidMob.Entry + ";" + avoidMob.Name);
                        }
                    }
                    else if (value is Quest && (value as Quest).Id != null) // if id = null, then we are parsing the QuestOrder.
                    {
                        var quest = value as Quest;
                        var tmpQuest = new Quester.Profile.Quest();
                        tmpQuest.Id = quest.Id != null ? Others.ToInt32(quest.Id) : 0;
                        tmpQuest.Name = quest.Name;
                        Logging.Write(quest.Id + ";" + quest.Name);
                        if (quest.Items.Length <= 0)
                            return;
                        foreach (object qContent in quest.Items)
                        {
                            if (qContent is ObjectiveMetaType)
                            {
                                tmpQuest.Objectives.Add(getValidObjective(qContent as ObjectiveMetaType));
                            }
                        }
                        bool found = false;
                        foreach (Quester.Profile.Quest tmpQ in _tnbProfile.Quests)
                        {
                            if (tmpQ.Id == tmpQuest.Id)
                            {
                                found = true;
                                break;
                            }
                        }
                        if (!found)
                            _tnbProfile.Quests.Add(tmpQuest);
                    }
                    else
                    {
                        if (value is QuestOrderType)
                        {
                            var questOrder = value as QuestOrderType;
                            AnalyzeDeeper(questOrder);
                        }
                    }
                }
            }
            if (_pickUpList.Count > 0)
            {
                foreach (Quester.Profile.Quest tnbQuest in _tnbProfile.Quests)
                {
                    foreach (var keyValuePair in _pickUpList)
                    {
                        if (keyValuePair.Key == tnbQuest.Id && tnbQuest.PickUp == 0)
                        {
                            tnbQuest.PickUp = (int) keyValuePair.Value;
                        }
                    }
                }
            }
            if (_itemPickUpList.Count > 0)
            {
                foreach (Quester.Profile.Quest tnbQuest in _tnbProfile.Quests)
                {
                    foreach (var keyValuePair in _itemPickUpList)
                    {
                        if (keyValuePair.Key == tnbQuest.Id && tnbQuest.ItemPickUp == 0)
                        {
                            tnbQuest.ItemPickUp = (int) keyValuePair.Value;
                        }
                    }
                }
            }
            if (_turnInList.Count > 0)
            {
                foreach (Quester.Profile.Quest tnbQuest in _tnbProfile.Quests)
                {
                    foreach (var keyValuePair in _turnInList)
                    {
                        if (keyValuePair.Key == tnbQuest.Id && tnbQuest.TurnIn == 0)
                        {
                            tnbQuest.TurnIn = (int) keyValuePair.Value;
                        }
                    }
                }
            }
            if (_tnbTmpNpcList.Count > 0)
            {
                foreach (Npc tmpNPC in _tnbTmpNpcList)
                {
                    bool found = false;
                    foreach (Npc npc in _tnbProfile.Questers)
                    {
                        if (npc.Entry == tmpNPC.Entry && npc.Position.X == tmpNPC.Position.X)
                            found = true;
                    }
                    if (!found)
                        _tnbProfile.Questers.Add(tmpNPC);
                }
            }
            var questsToRemove = new List<Quester.Profile.Quest>();
            foreach (Quester.Profile.Quest q in _tnbProfile.Quests)
            {
                if (q.MinLevel == 0 || q.MaxLevel == 0 || q.QuestLevel == 0)
                {
                    WowHead.QuestInfo qInfo = WowHead.GetQuestObject(q.Id);
                    if (!qInfo.IsValid)
                    {
                        questsToRemove.Add(q);
                        continue;
                    }
                    q.MinLevel = qInfo.ReqMinLevel;
                    q.MaxLevel = qInfo.ReqMaxLevel;
                    q.QuestLevel = qInfo.Level;
                    if (qInfo.Race != 0)
                        q.RaceMask = qInfo.Race;
                    else if (qInfo.Side != 0 && q.RaceMask == 0)
                    {
                        if (qInfo.Side == 1)
                        {
                            q.RaceMask = 16778317;
                        }
                        else if (qInfo.Side == 2)
                        {
                            q.RaceMask = 33555378; 
                        }
                        else if (qInfo.Side == 3)
                        {
                            q.RaceMask = 50333695; // or not set
                        }
                    }
                    if (qInfo.Classs != 0)
                        q.ClassMask = qInfo.Classs;
                    Logging.Write("Update quest: " + q.Name + "(" + q.Id + "), questLevel = " + q.QuestLevel + ", minLevel = " + q.MinLevel + ", maxLevel = " + q.MaxLevel + ", raceMask = " + q.RaceMask + ", classMask = " + q.ClassMask);
                }
            }
            XmlSerializer.Serialize(Application.StartupPath + @"\test_TNB_Extract.xml", _tnbProfile);
            XmlSerializer.Serialize(Application.StartupPath + @"\test_HB_ReExtract.xml", hbProfile);
            MessageBox.Show(timer.ElapsedMilliseconds.ToString());
            formMain = new MainForm();
            formMain.Show();
            _isStarted = true;
            Others.ProductStatusLog(Products.ProductName, 4);
        }
        catch (Exception e)
        {
            Logging.WriteError("Profiles Converters > Main > Start(): " + e);
        }
    }

    public void Stop()
    {
        try
        {
            if (formMain != null)
                formMain.Dispose();
            _isStarted = false;
            Others.ProductStatusLog(Products.ProductName, 6);
        }
        catch (Exception e)
        {
            Logging.WriteError("Profiles Converters > Main > Stop(): " + e);
        }
    }

    public void Settings()
    {
        try
        {
            MessageBox.Show(Translate.Get(Translate.Id.No_setting_for_this_product) + ".");
            Others.ProductStatusLog(Products.ProductName, 7);
        }
        catch (Exception e)
        {
            Logging.WriteError("Profiles Converters > Main > Settings(): " + e);
        }
    }

    public bool IsStarted
    {
        get { return _isStarted; }
    }

    private QuestObjective getValidObjective(ObjectiveMetaType qContent)
    {
        ObjectiveMetaType objective = qContent;
        if (objective.Type == ObjectiveTypeType.KillMob)
        {
            int killCount = objective.KillCount != null ? Convert.ToInt32(objective.KillCount) : 0;
            int mobId = objective.MobId != null ? Convert.ToInt32(objective.MobId) : 0;
            var hotspotsList = new List<Point>();
            if (objective.Items == null)
                return new QuestObjective();
            for (int i2 = 0; i2 < objective.Items.Length; i2++)
            {
                if (!(objective.Items[i2] is Hotspots) && objective.ItemsElementName[i2] == ItemsChoiceType3.Hotspots)
                    continue;
                var o = objective.Items[i2] as Hotspots;
                if (o == null || o.Hotspot == null)
                    continue;
                foreach (Point3DType hot in o.Hotspot)
                {
                    var tnbPoint = new Point {X = hot.X, Y = hot.Y, Z = hot.Z};
                    hotspotsList.Add(tnbPoint);
                }
            }
            var tmpObjective = new QuestObjective
            {
                Count = killCount,
                Entry = new List<int> {mobId,},
                Objective = Objective.KillMob,
                Hotspots = hotspotsList,
            };

            return tmpObjective;
        }
        if (objective.Type == ObjectiveTypeType.CollectItem)
        {
            int collectCount = objective.CollectCount != null ? Convert.ToInt32(objective.CollectCount) : 0;
            int collectItemId = objective.ItemId != null ? Convert.ToInt32(objective.ItemId) : 0;
            var hotspotsList = new List<Point>();
            if (objective.Items == null)
                return new QuestObjective();
            var mobList = new List<int>();
            var objList = new List<int>();
            var vndList = new List<int>();
            for (int i2 = 0; i2 < objective.Items.Length; i2++)
            {
                if (objective.Items[i2] == null)
                    continue;
                if (objective.Items[i2] is Hotspots && objective.ItemsElementName[i2] == ItemsChoiceType3.Hotspots)
                {
                    var o = objective.Items[i2] as Hotspots;
                    if (o != null && o.Hotspot == null)
                        continue;
                    foreach (Point3DType hot in o.Hotspot)
                    {
                        var tnbPoint = new Point {X = hot.X, Y = hot.Y, Z = hot.Z};
                        hotspotsList.Add(tnbPoint);
                    }
                }
                else if (objective.Items[i2] is CollectFrom || objective.ItemsElementName[i2] != ItemsChoiceType3.CollectFrom)
                {
                    var o = objective.Items[i2] as CollectFrom;
                    if (o == null || o.Items == null || o.Items.Length <= 0)
                        continue;
                    foreach (object typeUnit in o.Items)
                    {
                        if (typeUnit is MobObjectiveType)
                        {
                            var mob = typeUnit as MobObjectiveType;
                            if (mob.Id != null)
                                mobList.Add(Others.ToInt32(mob.Id));
                        }
                        if (typeUnit is GameObjectType)
                        {
                            var obj = typeUnit as GameObjectType;
                            if (obj.Id != null)
                                objList.Add(Others.ToInt32(obj.Id));
                        }
                        if (typeUnit is VendorObjectiveType)
                        {
                            var vnd = typeUnit as VendorObjectiveType;
                            if (vnd.Id != null)
                                vndList.Add(Others.ToInt32(vnd.Id));
                        }
                    }
                }
            }
            var tmpObjective = new QuestObjective
            {
                CollectCount = collectCount,
                CollectItemId = collectItemId,
                Entry = new List<int>(),
                Objective = Objective.KillMob,
                Hotspots = hotspotsList,
            };
            if (objList.Count > 0 && mobList.Count <= 0)
            {
                tmpObjective.Entry = objList;
                tmpObjective.Objective = Objective.PickUpObject;
            }
            else if (vndList.Count > 0 && mobList.Count <= 0)
            {
                tmpObjective.Entry = vndList;
                tmpObjective.Objective = Objective.BuyItem;
            }
            else
            {
                tmpObjective.Entry = mobList;
                tmpObjective.Objective = Objective.KillMob;
            }
            return tmpObjective;
        }
        return new QuestObjective();
    }

    public void AnalyzeDeeper(object obj)
    {
        object[] items = {};
        // <If Condition="HasQuest(26761) &amp;&amp; IsQuestCompleted(26761)">
        If If = null;
        While While = null;
        if (obj is QuestOrderType)
        {
            var questOrder = obj as QuestOrderType;
            items = questOrder.Items;
        }
        else if (obj is If)
        {
            If = obj as If;
            items = If.Items;
        }
        else if (obj is While)
        {
            While = obj as While;
            items = While.Items;
        }
        if (!(obj is QuestOrderType) && !(obj is If) && !(obj is While))
            return;
        for (int j = 0; j < items.Length; j++)
        {
            object qOrder = items[j];
            if (qOrder is PickUp)
            {
                var pickUp = qOrder as PickUp;
                CreateEmptyQuestFromPickUp(pickUp);
                var tnbNpc = new Npc
                {
                    Entry = (int) pickUp.GiverId,
                    Name = pickUp.GiverName,
                    Position = new Point {X = pickUp.X, Y = pickUp.Y, Z = pickUp.Z},
                };
                if (tnbNpc.Position.IsValid)
                {
                    _tnbTmpNpcList.Add(tnbNpc);
                }
                if (pickUp.GiverType == "Item")
                    _itemPickUpList.Add(new KeyValuePair<int, uint>((int) pickUp.QuestId, pickUp.GiverId));
                else
                    _pickUpList.Add(new KeyValuePair<int, uint>((int) pickUp.QuestId, pickUp.GiverId));
            }
            else if (qOrder is TurnIn)
            {
                var turnIn = qOrder as TurnIn;
                CreateEmptyQuestFromTurnIn(turnIn);
                var tnbNpc = new Npc
                {
                    Entry = (int) turnIn.TurnInId,
                    Name = turnIn.TurnInName,
                    Position = new Point {X = turnIn.X, Y = turnIn.Y, Z = turnIn.Z},
                };
                if (tnbNpc.Position.IsValid)
                    _tnbTmpNpcList.Add(tnbNpc);
                _turnInList.Add(new KeyValuePair<int, uint>((int) turnIn.QuestId, turnIn.TurnInId));
            }
            else if (qOrder is If)
            {
                var qOrderIf = qOrder as If;
                if (qOrderIf.Items == null)
                    continue;
                AnalyzeDeeper(qOrderIf);
            }
            else if (qOrder is While)
            {
                var qOrderWhile = qOrder as While;
                if (qOrderWhile.Items == null)
                    continue;
                AnalyzeDeeper(qOrderWhile);
            }
            else if (qOrder is CustomBehavior)
            {
                var emptyObjective = new QuestObjective();
                var qOrderCustom = qOrder as CustomBehavior;
                if (qOrderCustom.QuestId == 0)
                {
                    if (If != null)
                    {
                        qOrderCustom.QuestId = GetQuestIdFromCondition(If.Condition);
                    }
                    else if (While != null)
                    {
                        qOrderCustom.QuestId = GetQuestIdFromCondition(While.Condition);
                    }
                }
                if (qOrderCustom.File == "MoveTo")
                {
                    emptyObjective = new QuestObjective {Objective = Objective.MoveTo, Position = new Point {X = (float) qOrderCustom.X, Y = (float) qOrderCustom.Y, Z = (float) qOrderCustom.Z}};
                }
                else if (qOrderCustom.File == "WaitTimer")
                {
                    emptyObjective = new QuestObjective {Objective = Objective.Wait, WaitMs = (int) qOrderCustom.WaitTime};
                }
                else if (qOrderCustom.File == "")
                {
                    // more else/if to add with a valid .File argument here.
                }
                if (emptyObjective.Objective != Objective.None)
                {
                    foreach (Quester.Profile.Quest q in _tnbProfile.Quests)
                    {
                        if (q.Id == qOrderCustom.QuestId)
                            q.Objectives.Add(emptyObjective);
                    }
                }
            }
            else if (qOrder is ObjectiveMetaType)
            {
            }
        }
    }

    private uint GetQuestIdFromCondition(string condition)
    {
        //HasQuest(26322) &amp;&amp; !IsQuestCompleted(26322)
        const string beginning = "HasQuest(";
        const string end = ")";
        int start = condition.IndexOf(beginning, StringComparison.Ordinal) + beginning.Length;
        if (start <= 0 || start == beginning.Length - 1)
            return 0;
        int stop = condition.IndexOf(end, start, StringComparison.Ordinal);
        string result = condition.Substring(start, stop - start);
        uint questId = Others.ToUInt32(result);
        if (condition.Contains("HasQuest(" + questId + ")") && condition.Contains("!IsQuestCompleted(" + questId + ")"))
        {
            return questId;
        }
        if (condition.Contains("HasQuest(" + questId + ")") && !condition.Contains("IsQuestCompleted(" + questId + ")"))
        {
            return questId;
        }
        return questId; // Tmp value, we probably don't want the QuestId from HasQuest here.
    }

    public void CreateEmptyQuestFromTurnIn(TurnIn turnin)
    {
        bool found = false;
        var emptyQuest = new Quester.Profile.Quest {Id = (int) turnin.QuestId, Name = turnin.QuestName, TurnIn = (int) turnin.TurnInId};
        for (int i = 0; i < _tnbProfile.Quests.Count; i++)
        {
            Quester.Profile.Quest q = _tnbProfile.Quests[i];
            if (q.Id == turnin.QuestId)
            {
                found = true;
                break;
            }
        }
        if (!found)
            _tnbProfile.Quests.Add(emptyQuest);
    }

    public void CreateEmptyQuestFromPickUp(PickUp pickUp)
    {
        bool found = false;
        var emptyQuest = new Quester.Profile.Quest {Id = (int) pickUp.QuestId, Name = pickUp.QuestName};
        if (pickUp.GiverType == "Item")
            emptyQuest.ItemPickUp = (int) pickUp.GiverId;
        else
            emptyQuest.PickUp = (int) pickUp.GiverId;
        for (int i = 0; i < _tnbProfile.Quests.Count; i++)
        {
            Quester.Profile.Quest q = _tnbProfile.Quests[i];
            if (q.Id == pickUp.QuestId)
            {
                found = true;
                break;
            }
        }
        if (!found)
            _tnbProfile.Quests.Add(emptyQuest);
    }

    #endregion
}