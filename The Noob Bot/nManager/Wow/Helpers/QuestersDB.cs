using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using nManager.Helpful;
using nManager.Wow.Class;

namespace nManager.Wow.Helpers
{
    public class QuestersDB
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
                    Logging.WriteError("QuestersDB > ListNpc get: " + ex);
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
                    Logging.WriteError("QuestersDB > ListNpc set: " + ex);
                }
            }
        }

        private static void LoadList()
        {
            try
            {
                lock (typeof(QuestersDB))
                {
                    if (_listNpc == null)
                        _listNpc = XmlSerializer.Deserialize<List<Npc>>(Application.StartupPath + "\\Data\\QuestersDB.xml");
                    if (_listNpc == null)
                        _listNpc = new List<Npc>();
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError("QuestersDB > LoadList(): " + ex);
            }
        }

        public static void AddNpc(Npc npc)
        {
            try
            {
                AddNpcRange(new List<Npc> { npc });
            }
            catch (Exception ex)
            {
                Logging.WriteError("QuestersDB > AddNpc(Npc npc): " + ex);
            }
        }

        public static void DelNpc(Npc npc)
        {
            try
            {
                lock (typeof(QuestersDB))
                {
                    foreach (Npc npc1 in ListNpc)
                    {
                        if (npc1.Entry != npc.Entry || npc1.Type != npc.Type || !(npc1.Position.DistanceTo(npc.Position) < 1)) continue;
                        ListNpc.Remove(npc1);
                        break;
                    }
                    _listNpc.Sort(delegate(Npc x, Npc y) { return (x.Entry < y.Entry ? -1 : 1); });
                    XmlSerializer.Serialize(Application.StartupPath + "\\Data\\QuestersDB.xml", _listNpc);
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError("QuestersDB > DelNpc(Npc npc): " + ex);
            }
        }

        public static int AddNpcRange(List<Npc> npcList, bool neutralIfPossible = false)
        {
            try
            {
                int count = 0;
                LoadList();
                lock (typeof(QuestersDB))
                {
                    for (int i = 0; i < npcList.Count; i++)
                    {
                        Npc npc = npcList[i];
                        if (string.IsNullOrEmpty(npc.Name))
                            continue;
                        bool found = false;
                        bool factionChange = false;
                        Npc oldNpc = new Npc();

                        for (int i2 = 0; i2 < ListNpc.Count; i2++)
                        {
                            Npc npc1 = ListNpc[i2];
                            if (npc1.Entry == npc.Entry && npc1.Type == npc.Type && npc1.Position.DistanceTo(npc.Position) < 30)
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
                        _listNpc.Sort(delegate(Npc x, Npc y) { return (x.Entry < y.Entry ? -1 : 1); });
                        XmlSerializer.Serialize(Application.StartupPath + "\\Data\\QuestersDB.xml", _listNpc);
                    }
                    return count;
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError("QuestersDB > AddNpcRange(List<Npc> npcList)): " + ex);
                return 0;
            }
        }

        public static void BuildNewList(List<Npc> npcList)
        {
            try
            {
                File.Delete(Application.StartupPath + "\\Data\\QuestersDB.xml");
                ListNpc.Clear();
                lock (typeof(QuestersDB))
                {
                    foreach (Npc npc in npcList)
                    {
                        ListNpc.Add(npc);
                    }
                    Logging.Write("List builded with " + ListNpc.Count() + "NPC inside.");
                    _listNpc.Sort(delegate(Npc x, Npc y) { return (x.Entry < y.Entry ? -1 : 1); });
                    XmlSerializer.Serialize(Application.StartupPath + "\\Data\\QuestersDB.xml", ListNpc);
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError("QuestersDB > BuildNewList(List<Npc> npcList)): " + ex);
            }
        }

        public static Npc GetNpcNearby(Npc.NpcType type, bool ignoreRadiusSettings = false)
        {
            try
            {
                Npc.FactionType faction = (Npc.FactionType)Enum.Parse(typeof(Npc.FactionType), ObjectManager.ObjectManager.Me.PlayerFaction);
                return GetNpcNearby(type, faction, (Enums.ContinentId)Usefuls.ContinentId, ObjectManager.ObjectManager.Me.Position, ignoreRadiusSettings);
            }
            catch (Exception ex)
            {
                Logging.WriteError("QuestersDB > GetNpcNearby(Npc.NpcType type, bool ignoreRadiusSettings = false): " + ex);
                return new Npc();
            }
        }

        public static Npc GetNpcNearby(Npc.NpcType type, Npc.FactionType faction, Enums.ContinentId continentId, Point currentPosition, bool ignoreRadiusSettings = false)
        {
            try
            {
                Npc npcTemp = new Npc();
                foreach (Npc npc in ListNpc)
                {
                    if ((npc.Faction != faction && npc.Faction != Npc.FactionType.Neutral) || npc.Type != type || npc.ContinentId != continentId)
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
                    "QuestersDB > GetNpcNearby(Npc.NpcType type, Npc.FactionType faction, Enums.ContinentId continentId, Point currentPosition, bool ignoreRadiusSettings = false): " +
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