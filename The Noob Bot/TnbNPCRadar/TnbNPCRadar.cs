using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using nManager.Helpful;
using nManager.Plugins;
using nManager.Wow.Class;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;

#region Interface Implementation - Edition Expert only

public class Main : IPlugins
{
    private bool _checkFieldRunning;

    public bool Loop
    {
        get { return MyPluginClass.InternalLoop; }
        set { MyPluginClass.InternalLoop = value; }
    }

    public string Name
    {
        get { return MyPluginClass.Name; }
    }

    public string Author
    {
        get { return MyPluginClass.Author; }
    }

    public string Description
    {
        get { return MyPluginClass.Description; }
    }

    public string TargetVersion
    {
        get { return MyPluginClass.TargetVersion; }
    }

    public string Version
    {
        get { return MyPluginClass.Version; }
    }

    public bool IsStarted
    {
        get { return Loop && !_checkFieldRunning; }
    }

    public void Dispose()
    {
        Logging.WriteDebug(string.Format("The plugin {0} has stopped.", Name));
        Loop = false;
    }

    public void Initialize()
    {
        Logging.WriteDebug(string.Format("The plugin {0} is loading.", Name));
        Initialize(false);
    }

    public void ShowConfiguration()
    {
        // If you have a configuration form, please create/call MyPluginClass.ShowConfiguration();
        Logging.WriteDebug(string.Format("The plugin {0} don't implement any settings system.", Name));
        MessageBox.Show(string.Format("The plugin {0} don't need to be configured.", Name));
    }

    public void ResetConfiguration()
    {
        // If you have a configuration form, please create/call MyPluginClass.ResetConfiguration();
        Logging.WriteDebug(string.Format("The plugin {0} don't implement any settings system.", Name));
        MessageBox.Show(string.Format("The plugin {0} don't need to be configured.", Name));
    }

    public void CheckFields() // do not edit.
    {
        _checkFieldRunning = true;
        Loop = true;
        while (Loop)
        {
            Thread.Sleep(1000); // Don't do any action.
        }
    }

    public void Initialize(bool configOnly, bool resetSettings = false)
    {
        try
        {
            if (!configOnly && !resetSettings)
                Loop = true;
            MyPluginClass.Init();
        }
        catch (Exception e)
        {
            Logging.WriteError("IPlugins.Main.Initialize(bool configOnly, bool resetSettings = false): " + e);
        }
        if (Loop)
            Dispose();
    }
}

#endregion

#region Plugin core - Your plugin should be coded here

public static class MyPluginClass
{
    public static bool InternalLoop = true;
    public static string Author = "Vesper";
    public static string Name = "NPC Radar";
    public static string TargetVersion = "6.0.x";
    public static string Version = "1.0.0";

    public static string Description =
        "A background radar that constantly update/build the NPCs Database (Vendors, Repairers, QuestGivers).";

    private static readonly List<int> BlackListed = new List<int>(new[] {77789, 32638, 32639, 32641, 32642, 35642, 191605, 24780, 29561, 49586, 49588, 62822, 211006});

    public static void Init()
    {
        // Do some init stuff here.
        MainLoop();
    }

    public static void MainLoop()
    {
        while (InternalLoop)
        {
            PulseRadar();
            Thread.Sleep(650*2); // Every 2 ObjectManager refresh
            // Prevent corruptions while the game loads after a zone change
        }
    }

    // Various mount repair, portable mailbox, repair robots, Guild Page...
    public static void PulseRadar()
    {
        if (!Usefuls.InGame || Usefuls.IsLoading)
            return;
        var npcRadar = new List<Npc>();
        List<WoWGameObject> Mailboxes = ObjectManager.GetWoWGameObjectOfType(WoWGameObjectType.Mailbox);
        List<WoWUnit> Vendors = ObjectManager.GetWoWUnitVendor();
        List<WoWUnit> Repairers = ObjectManager.GetWoWUnitRepair();
        List<WoWUnit> NpcMailboxes = ObjectManager.GetWoWUnitMailbox();
        var npcRadarQuesters = new List<Npc>();
        List<WoWUnit> NpcQuesters = ObjectManager.GetWoWUnitQuester();
        List<WoWGameObject> ObjectQuesters = ObjectManager.GetWoWGameObjectOfType(WoWGameObjectType.Questgiver);

        foreach (WoWGameObject o in Mailboxes)
        {
            if (o.CreatedBy != 0)
                continue;
            npcRadar.Add(new Npc
            {
                ContinentIdInt = Usefuls.ContinentId,
                Entry = o.Entry,
                Faction = UnitRelation.GetObjectRacialFaction(o.Faction),
                Name = o.Name,
                Position = o.Position,
                Type = Npc.NpcType.Mailbox
            });
        }
        foreach (WoWUnit n in Vendors)
        {
            if (BlackListed.Contains(n.Entry) || n.CreatedBy != 0)
                continue;
            npcRadar.Add(new Npc
            {
                ContinentIdInt = Usefuls.ContinentId,
                Entry = n.Entry,
                Faction = UnitRelation.GetObjectRacialFaction(n.Faction),
                Name = n.Name,
                Position = n.Position,
                Type = Npc.NpcType.Vendor
            });
        }
        foreach (WoWUnit n in Repairers)
        {
            if (BlackListed.Contains(n.Entry) || n.CreatedBy != 0)
                continue;
            npcRadar.Add(new Npc
            {
                ContinentIdInt = Usefuls.ContinentId,
                Entry = n.Entry,
                Faction = UnitRelation.GetObjectRacialFaction(n.Faction),
                Name = n.Name,
                Position = n.Position,
                Type = Npc.NpcType.Repair
            });
        }
        foreach (WoWUnit n in NpcMailboxes)
        {
            if (BlackListed.Contains(n.Entry) || n.CreatedBy != 0)
                continue;
            npcRadar.Add(new Npc
            {
                ContinentIdInt = Usefuls.ContinentId,
                Entry = n.Entry,
                Faction = UnitRelation.GetObjectRacialFaction(n.Faction),
                Name = n.Name,
                Position = n.Position,
                Type = Npc.NpcType.Mailbox
            });
        }
        foreach (WoWUnit n in NpcQuesters)
        {
            if (BlackListed.Contains(n.Entry) || n.CreatedBy != 0)
                continue;
            npcRadarQuesters.Add(new Npc
            {
                ContinentIdInt = Usefuls.ContinentId,
                Entry = n.Entry,
                Faction = UnitRelation.GetObjectRacialFaction(n.Faction),
                Name = n.Name,
                Position = n.Position,
                Type = Npc.NpcType.QuestGiver
            });
        }
        foreach (WoWGameObject o in ObjectQuesters)
        {
            if (o.CreatedBy != 0)
                continue;
            npcRadarQuesters.Add(new Npc
            {
                ContinentIdInt = Usefuls.ContinentId,
                Entry = o.Entry,
                Faction = UnitRelation.GetObjectRacialFaction(o.Faction),
                Name = o.Name,
                Position = o.Position,
                Type = Npc.NpcType.QuestGiver
            });
        }
        int d = NpcDB.AddNpcRange(npcRadar, true);
        if (d == 1)
            Logging.Write("Found " + d + " new NPC/Mailbox in memory");
        else if (d > 1)
            Logging.Write("Found " + d + " new NPCs/Mailboxes in memory");
        d = AddQuesters(npcRadarQuesters, true);
        if (d == 1)
            Logging.Write("Found " + d + " new Quest Giver in memory");
        else if (d > 1)
            Logging.Write("Found " + d + " new Quest Givers in memory");
    }


    public static int AddQuesters(List<Npc> npcqList, bool neutralIfPossible = false)
    {
        int count = 0;
        var qesterList = XmlSerializer.Deserialize<List<Npc>>(Application.StartupPath + "\\Data\\QuestersDB.xml");
        if (qesterList == null)
            qesterList = new List<Npc>();
        for (int i = 0; i < npcqList.Count; i++)
        {
            Npc npc = npcqList[i];
            if (string.IsNullOrEmpty(npc.Name))
                continue;
            bool found = false;
            bool factionChange = false;
            var oldNpc = new Npc();
            for (int i2 = 0; i2 < qesterList.Count; i2++)
            {
                Npc npc1 = qesterList[i2];
                if (npc1.Entry == npc.Entry && npc1.Type == npc.Type && npc1.Position.DistanceTo(npc.Position) < 75)
                {
                    found = true;
                    if (npc1.Faction != npc.Faction && npc1.Faction != Npc.FactionType.Neutral)
                    {
                        if (neutralIfPossible)
                            npc.Faction = Npc.FactionType.Neutral;
                        oldNpc = npc1;
                        factionChange = true;
                    }
                    break;
                }
            }
            if (found && factionChange)
            {
                qesterList.Remove(oldNpc);
                qesterList.Add(npc);
                count++;
            }
            else if (!found)
            {
                qesterList.Add(npc);
                count++;
            }
        }
        if (count != 0)
        {
            qesterList.Sort(delegate(Npc x, Npc y)
            {
                if (x.Entry == y.Entry)
                    if (x.Position.X == y.Position.X)
                        return (x.Type < y.Type ? -1 : 1);
                    else
                        return (x.Position.X < y.Position.X ? -1 : 1);
                return (x.Entry < y.Entry ? -1 : 1);
            });
            XmlSerializer.Serialize(Application.StartupPath + "\\Data\\QuestersDB.xml", qesterList);
        }
        return count;
    }
}

#endregion