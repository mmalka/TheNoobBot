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
            var hbProfile = XmlSerializer.Deserialize<HBProfile>(Application.StartupPath + @"\[H - Quest] 85-86 The Jade Forest [Kick].xml");
            if (hbProfile.Items == null || !hbProfile.Items.Any())
            {
                return;
            }
            int count = hbProfile.Items.Length;
            if (count > hbProfile.ItemsElementName.Length)
                count = hbProfile.ItemsElementName.Length;
            var tnbProfile = new QuesterProfile();
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
                        foreach (blackspotType blackspot in blackspots.Blackspot)
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
                                Entry = Others.ToInt32(avoidMob.Entry), Name = avoidMob.Name
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
                            if (qContent is objectiveMetaType)
                            {
                                var objective = qContent as objectiveMetaType;
                                if (objective.Type == objectiveTypeType.KillMob)
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
                                        foreach (point3dType hot in o.Hotspot)
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
                                    tnbProfile.Quests.Add(tmpQuest);
                                }
                                else if (objective.Type == objectiveTypeType.CollectItem)
                                {
                                    int collectCount = objective.CollectCount != null ? Convert.ToInt32(objective.CollectCount) : 0;
                                    int collectItemId = objective.ItemId != null ? Convert.ToInt32(objective.ItemId) : 0;
                                    var hotspotsList = new List<Point>();
                                    if (objective.Items == null)
                                        return;
                                    var mobList = new List<int>();
                                    for (int i2 = 0; i2 < objective.Items.Length; i2++)
                                    {
                                        if (objective.Items[i2] == null)
                                            continue;
                                        if (objective.Items[i2] is Hotspots && objective.ItemsElementName[i2] == ItemsChoiceType3.Hotspots)
                                        {
                                            var o = objective.Items[i2] as Hotspots;
                                            if (o != null && o.Hotspot == null)
                                                continue;
                                            foreach (point3dType hot in o.Hotspot)
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
                                            foreach (object mob in o.Items)
                                            {
                                                if (mob is mobObjectiveType)
                                                {
                                                    var mo = mob as mobObjectiveType;
                                                    if (mo.Id != null)
                                                        mobList.Add(Others.ToInt32(mo.Id));
                                                }
                                                else 
                                                    return;
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
                                    tmpObjective.Entry.AddRange(mobList);
                                        tmpQuest.Objectives.Add(tmpObjective);
                                        tnbProfile.Quests.Add(tmpQuest);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            XmlSerializer.Serialize(Application.StartupPath + @"\test_TNB_Extract.xml", tnbProfile);
            XmlSerializer.Serialize(Application.StartupPath + @"\test_HB_ReExtract.xml", hbProfile);
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