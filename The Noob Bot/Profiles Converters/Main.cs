using System;
using System.Collections.Generic;
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
            System.Diagnostics.Stopwatch timer = System.Diagnostics.Stopwatch.StartNew();
            var hbProfile = XmlSerializer.Deserialize<HBProfile>(Application.StartupPath + @"\[A - Quest] EK 12-58 [Kick].xml");
            if (hbProfile.Items == null || !hbProfile.Items.Any())
            {
                return;
            }
            int count = hbProfile.Items.Length;
            if (count > hbProfile.ItemsElementName.Length)
                count = hbProfile.ItemsElementName.Length;
            var tnbProfile = new QuesterProfile();
            var tnbTmpNpcList = new List<Npc>();
            var pickUpList = new List<KeyValuePair<int, uint>>(); // QuestId, Quest.PickUp
            var itemPickUpList = new List<KeyValuePair<int, uint>>(); // QuestId, Quest.ItemPickUp 
            var turnInList = new List<KeyValuePair<int, uint>>(); // QuestId, Quest.TurnIn
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
                            tnbProfile.Blackspots.Add(new QuesterBlacklistRadius
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
                        AvoidMobs avoidMobs = value as AvoidMobs;
                        foreach (var avoidMob in avoidMobs.Items)
                        {
                            tnbProfile.AvoidMobs.Add(new Npc()
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
                                var objective = qContent as ObjectiveMetaType;
                                if (objective.Type == ObjectiveTypeType.KillMob)
                                {
                                    int killCount = objective.KillCount != null ? Convert.ToInt32(objective.KillCount) : 0;
                                    int mobId = objective.MobId != null ? Convert.ToInt32(objective.MobId) : 0;
                                    var hotspotsList = new List<Point>();
                                    if (objective.Items == null)
                                        return;
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

                                    tmpQuest.Objectives.Add(tmpObjective);
                                }
                                else if (objective.Type == ObjectiveTypeType.CollectItem)
                                {
                                    int collectCount = objective.CollectCount != null ? Convert.ToInt32(objective.CollectCount) : 0;
                                    int collectItemId = objective.ItemId != null ? Convert.ToInt32(objective.ItemId) : 0;
                                    var hotspotsList = new List<Point>();
                                    if (objective.Items == null)
                                        return;
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
                                    tmpQuest.Objectives.Add(tmpObjective);
                                }
                            }
                        }
                        tnbProfile.Quests.Add(tmpQuest);
                    }
                    else
                    {
                        if (value is QuestOrderType)
                        {
                            var questOrder = value as QuestOrderType;
                            for (int j = 0; j < questOrder.Items.Length; j++)
                            {
                                var qOrder = questOrder.Items[j];
                                if (qOrder is PickUp)
                                {
                                    var pickUp = qOrder as PickUp;
                                    var tnbNpc = new Npc
                                    {
                                        Entry = (int) pickUp.GiverId,
                                        Name = pickUp.GiverName,
                                        Position = new Point {X = pickUp.X, Y = pickUp.Y, Z = pickUp.Z},
                                    };
                                    if (tnbNpc.Position.IsValid)
                                        tnbTmpNpcList.Add(tnbNpc);
                                    if (pickUp.GiverType == "Item")
                                        itemPickUpList.Add(new KeyValuePair<int, uint>((int) pickUp.QuestId, pickUp.GiverId));
                                    else
                                        pickUpList.Add(new KeyValuePair<int, uint>((int) pickUp.QuestId, pickUp.GiverId));
                                }
                                else if (qOrder is TurnIn)
                                {
                                    var turnIn = qOrder as TurnIn;
                                    var tnbNpc = new Npc
                                    {
                                        Entry = (int) turnIn.TurnInId,
                                        Name = turnIn.TurnInName,
                                        Position = new Point {X = turnIn.X, Y = turnIn.Y, Z = turnIn.Z},
                                    };
                                    if (tnbNpc.Position.IsValid)
                                        tnbTmpNpcList.Add(tnbNpc);
                                    turnInList.Add(new KeyValuePair<int, uint>((int) turnIn.QuestId, turnIn.TurnInId));
                                }
                            }
                        }
                    }
                }
            }
            if (pickUpList.Count > 0)
            {
                foreach (var tnbQuest in tnbProfile.Quests)
                {
                    foreach (var keyValuePair in pickUpList)
                    {
                        if (keyValuePair.Key == tnbQuest.Id && tnbQuest.PickUp == 0)
                        {
                            tnbQuest.PickUp = (int) keyValuePair.Value;
                        }
                    }
                }
            }
            if (itemPickUpList.Count > 0)
            {
                foreach (var tnbQuest in tnbProfile.Quests)
                {
                    foreach (var keyValuePair in itemPickUpList)
                    {
                        if (keyValuePair.Key == tnbQuest.Id && tnbQuest.ItemPickUp == 0)
                        {
                            tnbQuest.ItemPickUp = (int) keyValuePair.Value;
                        }
                    }
                }
            }
            if (turnInList.Count > 0)
            {
                foreach (var tnbQuest in tnbProfile.Quests)
                {
                    foreach (var keyValuePair in turnInList)
                    {
                        if (keyValuePair.Key == tnbQuest.Id && tnbQuest.TurnIn == 0)
                        {
                            tnbQuest.TurnIn = (int) keyValuePair.Value;
                        }
                    }
                }
            }
            if (tnbTmpNpcList.Count > 0)
            {
                foreach (var tmpNPC in tnbTmpNpcList)
                {
                    bool found = false;
                    foreach (var npc in tnbProfile.Questers)
                    {
                        if (npc.Entry == tmpNPC.Entry && npc.Position.X == tmpNPC.Position.X)
                            found = true;
                    }
                    if (!found)
                        tnbProfile.Questers.Add(tmpNPC);
                }
            }
            /*List<Quester.Profile.Quest> questsToRemove = new List<Quester.Profile.Quest>();
            foreach (Quester.Profile.Quest q in tnbProfile.Quests)
            {
                if (q.MinLevel == 0 || q.MaxLevel == 0 || q.QuestLevel == 0)
                {
                    var qInfo = WowHead.GetQuestObject(q.Id);
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
                    if (qInfo.Classs != 0)
                        q.ClassMask = qInfo.Classs;
                    Logging.Write("Update quest: " + q.Name + "(" + q.Id + "), minLevel = " + q.MinLevel + ", maxLevel = " + q.MaxLevel + ", raceMask = " + q.RaceMask + ", classMask = " + q.ClassMask);
                }
            }*/
            XmlSerializer.Serialize(Application.StartupPath + @"\test_TNB_Extract.xml", tnbProfile);
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

    #endregion
}