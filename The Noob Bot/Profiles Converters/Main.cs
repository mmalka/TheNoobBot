using System;
using System.Linq;
using System.Windows.Forms;
using nManager.Helpful;
using Profiles_Converters;
using nManager.Products;
using Profiles_Converters.Converters;
using Quester.Profile;
using Quest = Profiles_Converters.Converters.Quest;

public class Main : IProduct
{
    #region IProduct Members

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
            /*var hbProfile = XmlSerializer.Deserialize<HBProfile>(Application.StartupPath + @"\[H - Quest] 85-86 The Jade Forest [Kick].xml");
            if (hbProfile.Items == null || !hbProfile.Items.Any())
            {
                return;
            }
            var count = hbProfile.Items.Length;
            if (count > hbProfile.ItemsElementName.Length)
                count = hbProfile.ItemsElementName.Length;
            var machin = new Quester.Profile.Quest();
            for (int i = 0; i < count; i++)
            {
                var name = hbProfile.ItemsElementName[i];
                var value = hbProfile.Items[i];
                Logging.Write(name + ": " + value);
                if (value.ToString().Contains("Profiles_Converters.Converters"))
                {
                    if (value is Vendors)
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
                    }
                    else if (value is Blackspots)
                    {
                        Blackspots blackspots = value as Blackspots;
                    }
                    else if (value is AvoidMobs)
                    {
                        AvoidMobs avoidMobs = value as AvoidMobs;
                    }
                    else if (value is Quest)
                    {
                        Quest quest = value as Quest;
                    }
                }
            }*/
            // subProfile = hbProfile;
            /*HBProfile hbQuest = new HBProfile();
            hbQuest.Vendors = new List<HBProfile.Vendor>
            {
                new HBProfile.Vendor {Id = 1, Name = "Vendor1", Type = "Food", X = 15, Y = -30, Z = 45.15}
            };
            hbQuest.Mailboxes = new List<HBProfile.Mailbox>
            {
                new HBProfile.Mailbox {X = 10, Y = 20.10, Z = 30.20}
            };
            hbQuest.Blackspots = new List<HBProfile.Blackspot>
            {
                new HBProfile.Blackspot
                {
                    Name = "Blackspot1",
                    X = -1,
                    Y = -1.2,
                    Z = 1.3,
                    Radius = 10,
                    Height = 20
                },
                new HBProfile.Blackspot
                {
                    Name = "Blackspot2",
                    X = 1,
                    Y = 1.2,
                    Z = -1.3,
                    Radius = 10,
                    Height = 20
                },
            };
            hbQuest.AvoidMobs = new List<HBProfile.Mob>()
            {
                new HBProfile.Mob
                {
                    Id = 1,
                    Name = "Mob1"
                }
            };
            hbQuest.Quest = new List<HBProfile.QuestTemplate>();
            var objectiveQuest1 = new List<HBProfile.QuestObjective>();
            objectiveQuest1.Add(new HBProfile.QuestObjective()
            {
                Hotspots = new List<HBProfile.Hotspot>
                {
                    new HBProfile.Hotspot
                    {
                        X = 10,
                        Y = 10,
                        Z = 20
                    },
                },
                KillCount = 20,
                MobId = 100,
                Type = HBProfile.ObjectiveType.ApplyBuff
            });
            objectiveQuest1.Add(new HBProfile.QuestObjective()
            {
                Hotspots = new List<HBProfile.Hotspot>
                {
                    new HBProfile.Hotspot
                    {
                        X = 20,
                        Y = 20,
                        Z = 10
                    },
                },
                KillCount = 20,
                MobId = 200,
                Type = HBProfile.ObjectiveType.KillMob
            });
            var objectiveQuest2 = new List<HBProfile.QuestObjective>();
            objectiveQuest2.Add(new HBProfile.QuestObjective()
            {
                Hotspots = new List<HBProfile.Hotspot>
                {
                    new HBProfile.Hotspot
                    {
                        X = 20,
                        Y = 20,
                        Z = 10
                    },
                },
                KillCount = 20,
                MobId = 200,
                Type = HBProfile.ObjectiveType.KillMob
            });
            hbQuest.Quest.Add(new HBProfile.QuestTemplate
            {
                Id = 32,
                Name = "Quest1",
                Objective = objectiveQuest1,
            });
            hbQuest.Quest.Add(
                new HBProfile.QuestTemplate
                {
                    Id = 23,
                    Name = "Quest2",
                    Objective = objectiveQuest2
                });
            XmlSerializer.Serialize(Application.StartupPath + @"\test.xml", hbQuest);*/
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
            MessageBox.Show(nManager.Translate.Get(nManager.Translate.Id.No_setting_for_this_product) + ".");
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

    private bool _isStarted;

    #endregion
}