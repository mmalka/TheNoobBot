using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using nManager.Helpful;
using nManager.Wow.Class;

namespace nManager.Wow.Helpers
{
    public class NpcDB
    {
        private static List<Npc> _listNpc;

        public static List<Npc> ListNpc
        {
            get
            {
                try
                {
                    LoadList();
                    return _listNpc;
                }
                catch (Exception ex)
                {
                    Logging.WriteError("NpcDB > ListNpc get: " + ex);
                    return new List<Npc>();
                }
            }
            set
            {
                try
                {
                    LoadList();
                    _listNpc = value;
                }
                catch (Exception ex)
                {
                    Logging.WriteError("NpcDB > ListNpc set: " + ex);
                }
            }
        }

        private static void LoadList()
        {
            try
            {
                lock (typeof (NpcDB))
                {
                    if (_listNpc == null)
                        _listNpc = XmlSerializer.Deserialize<List<Npc>>(Application.StartupPath + "\\Data\\NpcDB.xml");
                    if (_listNpc == null)
                        _listNpc = new List<Npc>();
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError("NpcDB > LoadList(): " + ex);
            }
        }

        public static void AddNpc(Npc npc)
        {
            try
            {
                AddNpcRange(new List<Npc> {npc});
            }
            catch (Exception ex)
            {
                Logging.WriteError("NpcDB > AddNpc(Npc npc): " + ex);
            }
        }

        public static void DelNpc(Npc npc)
        {
            try
            {
                lock (typeof (NpcDB))
                {
                    foreach (Npc npc1 in ListNpc)
                    {
                        if (npc1.Entry != npc.Entry || npc1.Type != npc.Type || !(npc1.Position.DistanceTo(npc.Position) < 1)) continue;
                        ListNpc.Remove(npc1);
                        break;
                    }
                    _listNpc.Sort(delegate(Npc x, Npc y)
                    {
                        if (x.Entry == y.Entry)
                            if (x.Position.X == y.Position.X)
                                return (x.Type < y.Type ? -1 : 1);
                            else
                                return (x.Position.X < y.Position.X ? -1 : 1);
                        return (x.Entry < y.Entry ? -1 : 1);
                    });
                    XmlSerializer.Serialize(Application.StartupPath + "\\Data\\NpcDB.xml", _listNpc);
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError("NpcDB > DelNpc(Npc npc): " + ex);
            }
        }

        public static int AddNpcRange(List<Npc> npcList, bool neutralIfPossible = false)
        {
            try
            {
                int count = 0;
                LoadList();
                lock (typeof (NpcDB))
                {
                    for (int i = 0; i < npcList.Count; i++)
                    {
                        Npc npc = npcList[i];
                        if (npc.Name == null || npc.Name == "")
                            continue;
                        bool found = false;
                        bool factionChange = false;
                        Npc oldNpc = new Npc();

                        for (int i2 = 0; i2 < ListNpc.Count; i2++)
                        {
                            Npc npc1 = ListNpc[i2];
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
                            ListNpc.Remove(oldNpc);
                            ListNpc.Add(npc);
                            count++;
                        }
                        else if (!found)
                        {
                            ListNpc.Add(npc);
                            count++;
                        }
                    }
                    if (count != 0)
                    {
                        _listNpc.Sort(delegate(Npc x, Npc y)
                        {
                            if (x.Entry == y.Entry)
                                if (x.Position.X == y.Position.X)
                                    return (x.Type < y.Type ? -1 : 1);
                                else
                                    return (x.Position.X < y.Position.X ? -1 : 1);
                            return (x.Entry < y.Entry ? -1 : 1);
                        });
                        XmlSerializer.Serialize(Application.StartupPath + "\\Data\\NpcDB.xml", _listNpc);
                    }
                    return count;
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError("NpcDB > AddNpcRange(List<Npc> npcList)): " + ex);
                return 0;
            }
        }

        public static void BuildNewList(List<Npc> npcList)
        {
            try
            {
                File.Delete(Application.StartupPath + "\\Data\\NpcDB.xml");
                ListNpc.Clear();
                lock (typeof (NpcDB))
                {
                    foreach (Npc npc in npcList)
                    {
                        ListNpc.Add(npc);
                    }
                    Logging.Write("List builded with " + ListNpc.Count() + "NPC inside.");
                    _listNpc.Sort(delegate(Npc x, Npc y)
                    {
                        if (x.Entry == y.Entry)
                            if (x.Position.X == y.Position.X)
                                return (x.Type < y.Type ? -1 : 1);
                            else
                                return (x.Position.X < y.Position.X ? -1 : 1);
                        return (x.Entry < y.Entry ? -1 : 1);
                    });
                    XmlSerializer.Serialize(Application.StartupPath + "\\Data\\NpcDB.xml", ListNpc);
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError("NpcDB > BuildNewList(List<Npc> npcList)): " + ex);
            }
        }

        public static Npc GetNpcNearby(Npc.NpcType type, bool ignoreRadiusSettings = false)
        {
            try
            {
                Npc.FactionType faction = (Npc.FactionType) Enum.Parse(typeof (Npc.FactionType), ObjectManager.ObjectManager.Me.PlayerFaction);
                return GetNpcNearby(type, faction, Usefuls.ContinentId, ObjectManager.ObjectManager.Me.Position, ignoreRadiusSettings);
            }
            catch (Exception ex)
            {
                Logging.WriteError("NpcDB > GetNpcNearby(Npc.NpcType type, bool ignoreRadiusSettings = false): " + ex);
                return new Npc();
            }
        }

        public static Npc GetNpcNearby(Npc.NpcType type, Npc.FactionType faction, int continentId, Point currentPosition, bool ignoreRadiusSettings = false)
        {
            try
            {
                Npc npcTemp = new Npc();
                foreach (Npc npc in ListNpc)
                {
                    if ((npc.Faction != faction && npc.Faction != Npc.FactionType.Neutral) || npc.Type != type || npc.ContinentIdInt != continentId)
                        continue;
                    if (!(npcTemp.Position.DistanceTo(currentPosition) > npc.Position.DistanceTo(currentPosition)) && npcTemp.Position.X != 0)
                        continue;
                    if (ignoreRadiusSettings || npc.Position.DistanceTo(currentPosition) <= nManagerSetting.CurrentSetting.MaxDistanceToGoToMailboxesOrNPCs)
                        npcTemp = npc;
                }
                return npcTemp;
            }
            catch (Exception ex)
            {
                Logging.WriteError(
                    "NpcDB > GetNpcNearby(Npc.NpcType type, Npc.FactionType faction, Enums.ContinentId continentId, Point currentPosition, bool ignoreRadiusSettings = false): " +
                    ex);
                return new Npc();
            }
        }

        public static Npc GetNpcByEntry(int entry)
        {
            foreach (Npc npc in ListNpc)
            {
                if (npc.Entry == entry)
                    return npc;
            }
            return new Npc();
        }
    }
}