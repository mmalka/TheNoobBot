using System;
using System.Collections.Generic;
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
                    LoadList(); return _listNpc;
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
                    LoadList(); _listNpc = value;
                }
                catch (Exception ex)
                {
                    Logging.WriteError("NpcDB > ListNpc set: " + ex);
                }
            }
        }

        static void LoadList()
        {
            try
            {
                lock (typeof(NpcDB))
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
                AddNpcRange(new List<Npc> { npc });
            }
            catch (Exception ex)
            {
                Logging.WriteError("NpcDB > AddNpc(Npc npc): " + ex);
            }
        }

        public static void AddNpcRange(List<Npc> npcList)
        {
            try
            {
                LoadList();
                lock (typeof(NpcDB))
                {
                    foreach (var npc in npcList)
                    {
                        bool found = false;
                        foreach (var npc1 in ListNpc)
                        {

                            if (npc1.Position.DistanceTo(npc.Position) < 1 && npc1.Entry == npc.Entry)
                            {
                                found = true;
                                break;
                            }
                        }
                        if (!found)
                            ListNpc.Add(npc);
                    }

                    XmlSerializer.Serialize(Application.StartupPath + "\\Data\\NpcDB.xml", _listNpc);
                }

            }
            catch (Exception ex)
            {
                Logging.WriteError("NpcDB > AddNpcRange(List<Npc> npcList)): " + ex);
            }
        }

        public static Npc GetNpcNearby(Npc.NpcType type)
        {
            try
            {
                var faction = (Npc.FactionType)Enum.Parse(typeof(Npc.FactionType), ObjectManager.ObjectManager.Me.PlayerFaction);
                return GetNpcNearby(type, faction, (Enums.ContinentId)Usefuls.ContinentId,
                                    ObjectManager.ObjectManager.Me.Position, (!string.IsNullOrEmpty(nManagerSetting.CurrentSetting.FlyingMountName) && !Usefuls.IsSwimming));

            }
            catch (Exception ex)
            {
                Logging.WriteError("NpcDB > GetNpcNearby(Npc.NpcType type): " + ex);
                return new Npc();
            }
        }
        public static Npc GetNpcNearby(Npc.NpcType type, Npc.FactionType faction, Enums.ContinentId continentId, Point currentPosition, bool forceFlying = false)
        {
            try
            {
                var npcTemp = new Npc();
                foreach (var npc in ListNpc)
                {
                    if (npc.Position.Type.ToLower() != "flying" && forceFlying)
                        continue;

                    if ((npc.Faction == faction || npc.Faction == Npc.FactionType.Neutral) && npc.Type == type && npc.ContinentId == continentId)
                    {
                        if (npcTemp.Position.DistanceTo(currentPosition) > npc.Position.DistanceTo(currentPosition) || npcTemp.Position.X == 0)
                            if (npc.Position.DistanceTo(currentPosition) <= nManagerSetting.CurrentSetting.MaxDistanceToGoToMailboxesOrNPCs)
                                npcTemp = npc;
                    }
                }
                return npcTemp;
            }
            catch (Exception ex)
            {
                Logging.WriteError("NpcDB > GetNpcNearby(Npc.NpcType type, Npc.FactionType faction, Enums.ContinentId continentId, Point currentPosition): " + ex);
                return new Npc();
            }
        }
    }
}
