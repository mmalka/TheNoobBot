using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading;
using nManager.Products;
using nManager.Wow.Bot.States;
using nManager.Wow.ObjectManager;
using ObjectManager = nManager.Wow.ObjectManager.ObjectManager;
// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using nManager.Helpful;
using nManager.Wow;
using nManager.Wow.Class;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;
using nManager.Wow.Patchables;

// ReSharper restore RedundantUsingDirective

namespace Test_Product
{
    internal class Bot
    {
        /*private const string CurrentProfileName = "afk.xml";
        private static bool _xmlProfile = true;
        private static BattlegrounderProfile _currentProfile = new BattlegrounderProfile();*/
        private static readonly Thread RadarThread = new Thread(LaunchRadar) { Name = "RadarThread" };
        private static readonly Thread TaxiThread = new Thread(LaunchTaxi) { Name = "TaxiThread" };

        public static void LaunchRadar()
        {
            int d;
            var myConn = new MySqlConnection("server=192.168.10.222; user id=root; password=aabbcc; database=offydump;");
            myConn.Open();
            // Various mount repair, portable mailbox, repair robots, Guild Page...
            List<int> BlackListed = new List<int>(new int[] {32638, 32639, 32641, 32642, 35642, 191605, 24780, 29561, 49586, 49588, 62822, 211006});
            while (true)
            {
                Thread.Sleep(650 * 2); // Every 2 ObjectManager refresh
                // Prevent corruptions while the game loads after a zone change
                if (!Usefuls.InGame || Usefuls.IsLoadingOrConnecting)
                    continue;
                List<Npc> npcRadar = new List<Npc>();
                List<WoWGameObject> Mailboxes = ObjectManager.GetWoWGameObjectOfType(WoWGameObjectType.Mailbox);
                List<WoWUnit> Vendors = ObjectManager.GetWoWUnitVendor();
                List<WoWUnit> Repairers = ObjectManager.GetWoWUnitRepair();
                List<WoWUnit> Inkeepers = ObjectManager.GetWoWUnitInkeeper();
                List<WoWUnit> Trainers = ObjectManager.GetWoWUnitTrainer();
                List<WoWUnit> FlightMasters = ObjectManager.GetWoWUnitFlightMaster();
                List<WoWUnit> Auctioneers = ObjectManager.GetWoWUnitAuctioneer();
                List<WoWUnit> SpiritHealers = ObjectManager.GetWoWUnitSpiritHealer();
                List<WoWUnit> SpiritGuides = ObjectManager.GetWoWUnitSpiritGuide();
                List<WoWUnit> NpcMailboxes = ObjectManager.GetWoWUnitMailbox();
                List<Npc> npcRadarQuesters = new List<Npc>();
                List<WoWUnit> NpcQuesters = ObjectManager.GetWoWUnitQuester();
                List<WoWGameObject> ObjectQuesters = ObjectManager.GetWoWGameObjectOfType(WoWGameObjectType.Questgiver);
                List<WoWGameObject> Forges = ObjectManager.GetWoWGameObjectOfType(WoWGameObjectType.SpellFocus);

                List<WoWGameObject> AllGos = ObjectManager.GetObjectWoWGameObject();

                foreach (WoWGameObject go in AllGos)
                {
                    if (go.Entry != 0 && go.Name != "" && go.CreatedBy == 0)
                    {
                        string query = "SELECT entry FROM gameobject WHERE entry = " + go.Entry + " AND " +
                            "map = " + Usefuls.RealContinentId + " AND " +
                            "SQRT((x-" + go.Position.X + ")*(x-" + go.Position.X + ") + " +
                             "(y-" + go.Position.Y + ")*(y-" + go.Position.Y + ")) < 0.5;";
                        var cmd = new MySqlCommand(query, myConn);
                        MySqlDataReader result = cmd.ExecuteReader();
                        if (!result.HasRows && go.GOType != WoWGameObjectType.MoTransport)
                        {
                            result.Close();
                            Quaternion rotations = go.Rotations;
                            Matrix4 matrice = go.WorldMatrix;
                            query = "INSERT INTO gameobject (entry,map,x,y,z,o,r0,r1,r2,r3,m11,m12,m13,m14,m21,m22,m23,m24,m31,m32,m33,m34,m41,m42,m43,m44) VALUES (" + go.Entry + "," + Usefuls.RealContinentId + "," + go.Position.X + "," + go.Position.Y + "," + go.Position.Z + "," + go.Orientation + "," + rotations.X + "," + rotations.Y + "," + rotations.Z + "," + rotations.W + "," +
                                matrice.xx + "," + matrice.xy + "," + matrice.xz + "," + matrice.xw + "," + matrice.yx + "," + matrice.yy + "," + matrice.yz + "," + matrice.yw + "," + matrice.zx + "," + matrice.zy + "," + matrice.zz + "," + matrice.zw + "," + matrice.wx + "," + matrice.wy + "," + matrice.wz + "," + matrice.ww +");";
                            cmd = new MySqlCommand(query, myConn);
                            cmd.ExecuteNonQuery();
                        }
                        else
                            result.Close();
                        //bool newAdded = false;
                        query = "SELECT entry,questitem1 FROM gameobject_template WHERE entry = " + go.Entry + ";";
                        cmd = new MySqlCommand(query, myConn);
                        result = cmd.ExecuteReader();
                        if (!result.HasRows)
                        {
                            result.Close();
                            query = "INSERT IGNORE INTO gameobject_template (entry,type,name,iconname,castbarcaption,model,faction,flags,size";
                            for (uint i = 0; i < 32; i++)
                                query += ",data" + i;
                            query += ",questitem1,questitem2,questitem3,questitem4) VALUES (" + go.Entry + "," + (uint)go.GOType + ",'" + go.Name.Replace("'", "\\'") + "','" + go.IconName + "','" + go.CastBarCaption + "'," + go.DisplayId + "," + go.Faction + "," + (uint)go.GOFlags + "," + go.Size;
                            for (uint i = 0; i < 32; i++)
                                query += "," + go.Data(i);
                            query += "," + go.QuestItem1 + "," + go.QuestItem2 + "," + go.QuestItem3 + "," + go.QuestItem4;
                            query += ");";
                            cmd = new MySqlCommand(query, myConn);
                            cmd.ExecuteNonQuery();
                            //newAdded = true;
                        }
                        else
                        {
                            /*result.Read();
                            int questitem = result.GetInt32(1);*/
                            result.Close();
                            /*if (questitem > 1000000 || questitem < 0) // 1065353216
                            {
                                query = "UPDATE gameobject_template set size = " + go.Size + ", questitem1 = " + go.QuestItem1 + ", questitem2 = " + go.QuestItem2 + ", questitem3 = " + go.QuestItem3 + ", questitem4 = " + go.QuestItem4 + " WHERE entry =" + go.Entry + ";";
                                cmd = new MySqlCommand(query, myConn);
                                cmd.ExecuteNonQuery();
                            }*/
                        }
                        /*if (!newAdded && go.IconName != "")
                        {
                            query = "UPDATE gameobject_template set iconname='" + go.IconName + "' WHERE entry =" + go.Entry + ";";
                            cmd = new MySqlCommand(query, myConn);
                            cmd.ExecuteNonQuery();
                        }
                        if (!newAdded && go.CastBarCaption != "")
                        {
                            query = "UPDATE gameobject_template set castbarcaption='" + go.CastBarCaption + "' WHERE entry =" + go.Entry + ";";
                            cmd = new MySqlCommand(query, myConn);
                            cmd.ExecuteNonQuery();
                        }*/
                    }
                }

                foreach (WoWGameObject o in Mailboxes)
                {
                    if (o.CreatedBy != 0)
                        continue;
                    npcRadar.Add(new Npc
                    {
                        ContinentId = (ContinentId) Usefuls.ContinentId,
                        Entry = o.Entry,
                        Faction = UnitRelation.GetObjectRacialFaction(o.Faction),
                        Name = o.Name,
                        Position = o.Position,
                        Type = Npc.NpcType.Mailbox
                    });
                }
                foreach (WoWUnit n in Vendors)
                {
                    if (BlackListed.Contains(n.Entry))
                        continue;
                    npcRadar.Add(new Npc
                    {
                        ContinentId = (ContinentId) Usefuls.ContinentId,
                        Entry = n.Entry,
                        Faction = UnitRelation.GetObjectRacialFaction(n.Faction),
                        Name = n.Name,
                        Position = n.Position,
                        Type = Npc.NpcType.Vendor
                    });
                }
                foreach (WoWUnit n in Repairers)
                {
                    if (BlackListed.Contains(n.Entry))
                        continue;
                    npcRadar.Add(new Npc
                    {
                        ContinentId = (ContinentId) Usefuls.ContinentId,
                        Entry = n.Entry,
                        Faction = UnitRelation.GetObjectRacialFaction(n.Faction),
                        Name = n.Name,
                        Position = n.Position,
                        Type = Npc.NpcType.Repair
                    });
                }
                foreach (WoWUnit n in Inkeepers)
                {
                    npcRadar.Add(new Npc
                    {
                        ContinentId = (ContinentId) Usefuls.ContinentId,
                        Entry = n.Entry,
                        Faction = UnitRelation.GetObjectRacialFaction(n.Faction),
                        Name = n.Name,
                        Position = n.Position,
                        Type = Npc.NpcType.Innkeeper
                    });
                }
                foreach (WoWUnit n in Trainers)
                {
                    Npc.NpcType newtype;
                    if (n.SubName.Contains("Alchemy") || n.SubName.Contains("alchimistes"))
                        newtype = Npc.NpcType.AlchemyTrainer;
                    else if (n.SubName.Contains("Blacksmithing") || n.SubName.Contains("forgerons"))
                        newtype = Npc.NpcType.BlacksmithingTrainer;
                    else if (n.SubName.Contains("Enchanting") || n.SubName.Contains("enchanteurs"))
                        newtype = Npc.NpcType.EnchantingTrainer;
                    else if (n.SubName.Contains("Engineering") || n.SubName.Contains("ingénieurs"))
                        newtype = Npc.NpcType.EngineeringTrainer;
                    else if (n.SubName.Contains("Herbalism") || n.SubName.Contains("herboristes"))
                        newtype = Npc.NpcType.HerbalismTrainer;
                    else if (n.SubName.Contains("Inscription") || n.SubName.Contains("calligraphes"))
                        newtype = Npc.NpcType.InscriptionTrainer;
                    else if (n.SubName.Contains("Jewelcrafting") || n.SubName.Contains("joailliers"))
                        newtype = Npc.NpcType.JewelcraftingTrainer;
                    else if (n.SubName.Contains("Leatherworking") || n.SubName.Contains("travailleurs du cuir"))
                        newtype = Npc.NpcType.LeatherworkingTrainer;
                    else if (n.SubName.Contains("Mining") || n.SubName.Contains("mineurs"))
                        newtype = Npc.NpcType.MiningTrainer;
                    else if (n.SubName.Contains("Skinning") || n.SubName.Contains("dépeceurs"))
                        newtype = Npc.NpcType.SkinningTrainer;
                    else if (n.SubName.Contains("Tailoring") || n.SubName.Contains("tailleurs"))
                        newtype = Npc.NpcType.TailoringTrainer;
                    else if (n.SubName.Contains("Archaeology") || n.SubName.Contains("archéologues"))
                        newtype = Npc.NpcType.ArchaeologyTrainer;
                    else if (n.SubName.Contains("Cooking") || n.SubName.Contains("cuisiniers"))
                        newtype = Npc.NpcType.CookingTrainer;
                    else if (n.SubName.Contains("First Aid") || n.SubName.Contains("secouristes"))
                        newtype = Npc.NpcType.FirstAidTrainer;
                    else if (n.SubName.Contains("Fishing") || n.SubName.Contains("forgerons"))
                        newtype = Npc.NpcType.FishingTrainer;
                    else if (n.SubName.Contains("Riding") || n.SubName.Contains(" de vol") || n.SubName.Contains(" de monte"))
                        newtype = Npc.NpcType.RidingTrainer;
                    else
                        continue;
                    npcRadar.Add(new Npc
                    {
                        ContinentId = (ContinentId) Usefuls.ContinentId,
                        Entry = n.Entry,
                        Faction = UnitRelation.GetObjectRacialFaction(n.Faction),
                        Name = n.Name,
                        Position = n.Position,
                        Type = newtype
                    });
                }
                foreach (WoWUnit n in SpiritHealers)
                {
                    npcRadar.Add(new Npc
                    {
                        ContinentId = (ContinentId) Usefuls.ContinentId,
                        Entry = n.Entry,
                        Faction = UnitRelation.GetObjectRacialFaction(n.Faction),
                        Name = n.Name,
                        Position = n.Position,
                        Type = Npc.NpcType.SpiritHealer
                    });
                }
                foreach (WoWUnit n in SpiritGuides)
                {
                    npcRadar.Add(new Npc
                    {
                        ContinentId = (ContinentId) Usefuls.ContinentId,
                        Entry = n.Entry,
                        Faction = UnitRelation.GetObjectRacialFaction(n.Faction),
                        Name = n.Name,
                        Position = n.Position,
                        Type = Npc.NpcType.SpiritGuide
                    });
                }
                foreach (WoWUnit n in NpcMailboxes)
                {
                    npcRadar.Add(new Npc
                    {
                        ContinentId = (ContinentId) Usefuls.ContinentId,
                        Entry = n.Entry,
                        Faction = UnitRelation.GetObjectRacialFaction(n.Faction),
                        Name = n.Name,
                        Position = n.Position,
                        Type = Npc.NpcType.Mailbox
                    });
                }
                foreach (WoWUnit n in Auctioneers)
                {
                    npcRadar.Add(new Npc
                    {
                        ContinentId = (ContinentId) Usefuls.ContinentId,
                        Entry = n.Entry,
                        Faction = UnitRelation.GetObjectRacialFaction(n.Faction),
                        Name = n.Name,
                        Position = n.Position,
                        Type = Npc.NpcType.Auctioneer
                    });
                }
                foreach (WoWUnit n in NpcQuesters)
                {
                    npcRadarQuesters.Add(new Npc
                    {
                        ContinentId = (ContinentId) Usefuls.ContinentId,
                        Entry = n.Entry,
                        Faction = UnitRelation.GetObjectRacialFaction(n.Faction),
                        Name = n.Name,
                        Position = n.Position,
                        Type = Npc.NpcType.QuestGiver
                    });
                }
                foreach (WoWGameObject o in ObjectQuesters)
                {
                    npcRadarQuesters.Add(new Npc
                    {
                        ContinentId = (ContinentId) Usefuls.ContinentId,
                        Entry = o.Entry,
                        Faction = UnitRelation.GetObjectRacialFaction(o.Faction),
                        Name = o.Name,
                        Position = o.Position,
                        Type = Npc.NpcType.QuestGiver
                    });
                }
                foreach (WoWGameObject o in Forges)
                {
                    Npc.NpcType newtype;
                    switch (o.Data0)
                    {
                        case 3:
                            newtype = Npc.NpcType.SmeltingForge;
                            break;
                        case 1552:
                            newtype = Npc.NpcType.RuneForge;
                            break;
                        default:
                            continue;
                    }
                    npcRadar.Add(new Npc
                    {
                        ContinentId = (ContinentId) Usefuls.ContinentId,
                        Entry = o.Entry,
                        Faction = UnitRelation.GetObjectRacialFaction(o.Faction),
                        Name = o.Name,
                        Position = o.Position,
                        Type = newtype
                    });
                }
                d = NpcDB.AddNpcRange(npcRadar, true);
                if (d > 0)
                    Logging.Write("Found " + d + " new NPCs/Mailboxes in memory");
                d = AddQuesters(npcRadarQuesters, true);
                if (d == 1)
                    Logging.Write("Found " + d + " new Quest Giver in memory");
                else if (d > 1)
                    Logging.Write("Found " + d + " new Quest Givers in memory");
            }
        }

        public static void LaunchTaxi()
        {
            try
            {

            if (_availableTaxis == null)
                _availableTaxis = XmlSerializer.Deserialize<List<Taxi>>(Application.StartupPath + @"\Data\TaxiList.xml");
            if (_availableTaxiLinks == null)
                _availableTaxiLinks = XmlSerializer.Deserialize<List<TaxiLink>>(Application.StartupPath + @"\Data\TaxiLinks.xml");
            uint firstTaxiId = 0;
            while (true)
            {
                _availableTaxis = XmlSerializer.Deserialize<List<Taxi>>(Application.StartupPath + @"\Data\TaxiList.xml");
                _availableTaxiLinks = XmlSerializer.Deserialize<List<TaxiLink>>(Application.StartupPath + @"\Data\TaxiLinks.xml");
                if (IsTaxiOpen())
                {
                    if (firstTaxiId != 0 && firstTaxiId == ObjectManager.Me.Target.GetWoWId)
                    {
                        Logging.Write("The continent have been parsed !");
                        break;
                    }
                    if (firstTaxiId == 0)
                        firstTaxiId = ObjectManager.Me.Target.GetWoWId;
                    if (TaxiListContainsTaxiId(ObjectManager.Me.Target.GetWoWId))
                    {
                        Logging.WriteDebug("The taxi from NPC " + ObjectManager.Target.Name + " is already in our database.");
                        var myTaxi = GetTaxiFromTaxiId(ObjectManager.Me.Target.GetWoWId);
                        if (myTaxi.Faction != Npc.FactionType.Neutral && ObjectManager.Me.PlayerFaction != myTaxi.Faction.ToString())
                        {
                            for (int i = 0; i < _availableTaxis.Count; i++)
                            {
                                if (myTaxi.Id == _availableTaxis[i].Id)
                                {
                                    _availableTaxis[i].Faction = Npc.FactionType.Neutral;
                                }
                            }
                        }
                    }
                    else
                    {
                        Taxi localTaxi = new Taxi();
                        localTaxi.Id = ObjectManager.Me.Target.GetWoWId;
                        localTaxi.Position = ObjectManager.Target.Position;
                        string taxiInfo = ExtractCurrentTaxiInfo();
                        localTaxi.Name = taxiInfo.Split('#')[0];
                        localTaxi.ContinentId = Usefuls.ContinentId;
                        localTaxi.Xcoord = taxiInfo.Split('#')[1].Split('^')[0];
                        localTaxi.Ycoord = taxiInfo.Split('^')[1].Split('@')[0];
                        localTaxi.Faction = ObjectManager.Me.PlayerFaction == "Alliance" ? Npc.FactionType.Alliance : Npc.FactionType.Horde;
                        _availableTaxis.Add(localTaxi);
                        foreach (TaxiLink taxiLink in _availableTaxiLinks)
                        {
                            Application.DoEvents();
                            if (taxiLink.PointB == 0 && taxiLink.PointB_XY == localTaxi.Xcoord + localTaxi.Ycoord)
                            {
                                taxiLink.PointB = localTaxi.Id;
                                taxiLink.PointB_XY = "";
                            }
                        }
                    }

                    foreach (string ctaxi in ExtractDirectPathTaxiInfoList())
                    {
                        Application.DoEvents();
                        string taxiInfo = ctaxi;
                        Taxi localTaxi = new Taxi();
                        localTaxi.Name = taxiInfo.Split('#')[0];
                        localTaxi.ContinentId = Usefuls.ContinentId;
                        localTaxi.Xcoord = taxiInfo.Split('#')[1].Split('^')[0];
                        localTaxi.Ycoord = taxiInfo.Split('^')[1].Split('@')[0];
                        bool taxiExist = false;
                        Taxi taxiFound = new Taxi();
                        foreach (Taxi taxi in _availableTaxis)
                        {
                            Application.DoEvents();
                            if (taxi.Xcoord == localTaxi.Xcoord && taxi.Ycoord == localTaxi.Ycoord)
                            {
                                // this taxi exist in the list so we have its ID
                                taxiExist = true;
                                taxiFound = taxi;
                            }
                        }
                        bool found = false;
                        foreach (TaxiLink taxiLink in _availableTaxiLinks)
                        {
                            Application.DoEvents();
                            if (taxiExist && taxiLink.PointA == ObjectManager.Me.Target.GetWoWId && taxiLink.PointB == localTaxi.Id)
                            {
                                found = true;
                                break;
                            }
                            if (taxiLink.PointA == ObjectManager.Me.Target.GetWoWId && taxiLink.PointB_XY == localTaxi.Xcoord + localTaxi.Ycoord)
                            {
                                found = true;
                                break;
                            }
                            if (taxiExist && taxiLink.PointB == ObjectManager.Me.Target.GetWoWId && taxiLink.PointA == localTaxi.Id)
                            {
                                found = true;
                                break;
                            }
                        }
                        if (!found)
                        {
                            _availableTaxiLinks.Add(taxiExist
                                ? new TaxiLink { PointA = ObjectManager.Me.Target.GetWoWId, PointB = taxiFound.Id }
                                : new TaxiLink { PointA = ObjectManager.Me.Target.GetWoWId, PointB_XY = localTaxi.Xcoord + localTaxi.Ycoord });
                        }
                    }

                    XmlSerializer.Serialize(Application.StartupPath + @"\Data\TaxiList.xml", _availableTaxis);
                    XmlSerializer.Serialize(Application.StartupPath + @"\Data\TaxiLinks.xml", _availableTaxiLinks);
                    string nextHop = ExtractNextTaxiInfo();
                    Logging.Write("Taking taxi from " + ExtractCurrentTaxiInfo().Split('#')[0] + " to " + nextHop.Split('#')[0]);
                    Gossip.TakeTaxi(nextHop.Split('#')[1].Split('^')[0], nextHop.Split('^')[1].Split('@')[0]);
                    Thread.Sleep(1000);
                }
                if (ObjectManager.Me.OnTaxi)
                {
                    Travel.TravelPatientlybyTaxi(true);
                    Thread.Sleep(2000);
                    continue;
                }
                WoWUnit taxiUnit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitFlightMaster());
                var baseAddress = MovementManager.FindTarget(taxiUnit);
                if (MovementManager.InMovement)
                    continue;
                if (baseAddress > 0)
                {
                    Interact.InteractWith(baseAddress);
                    Thread.Sleep(500);
                    if (!Gossip.IsTaxiWindowOpen())
                    {
                        Gossip.SelectGossip(Gossip.GossipOption.Taxi);
                        Thread.Sleep(250 + Usefuls.Latency);
                    }
                }
                Thread.Sleep(200);
            }
            }
            catch (Exception e)
            {
                Logging.WriteDebug(e.ToString());
            }
        }
        public static int AddQuesters(List<Npc> npcqList, bool neutralIfPossible = false)
        {
            int count = 0;
            List<Npc> qesterList = XmlSerializer.Deserialize<List<Npc>>(Application.StartupPath + "\\Data\\QuestersDB.xml");
            if (qesterList == null)
                qesterList = new List<Npc>();
            for (int i = 0; i < npcqList.Count; i++)
            {
                Npc npc = npcqList[i];
                if (npc.Name == null || npc.Name == "")
                    continue;
                bool found = false;
                bool factionChange = false;
                Npc oldNpc = new Npc();
                for (int i2 = 0; i2 < qesterList.Count; i2++)
                {
                    Npc npc1 = qesterList[i2];
                    if (npc1.Entry == npc.Entry && npc1.Type == npc.Type && npc1.Position.DistanceTo(npc.Position) < 60)
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
                        return (x.Type < y.Type ? -1 : 1);
                    return (x.Entry < y.Entry ? -1 : 1);
                });
                XmlSerializer.Serialize(Application.StartupPath + "\\Data\\QuestersDB.xml", qesterList);
            }
            return count;
        }

        public static bool IsTaxiOpen()
        {
            Lua.LuaDoString("test = tostring(TaxiFrame and TaxiFrame:IsVisible())");
            return Lua.GetLocalizedText("test") == "true";
        }

        public static List<string> ExtractAllPathsTaxi()
        {
            string result = Others.GetRandomString(Others.Random(4, 10));
            Lua.LuaDoString(result + "= \"\"; nb = NumTaxiNodes(); for i=1,nb do n = TaxiNodeName(i); x,y = TaxiNodePosition(i); " + result + " = " + result +
                            ".. n .. \"#\" .. x .. \"^\" .. y  .. \"@\" .. GetNumRoutes(i) .. \"~\" .. TaxiNodeGetType(i) .. \"|\" end");
            string allPaths = Lua.GetLocalizedText(result);
            List<string> ListPaths = new List<String>();
            for (int i = 0; i < allPaths.Split('|').Length - 1; i++)
            {
                ListPaths.Add(allPaths.Split('|')[i]);
            }
            var listPathFinal = new List<string>();
            string lowerValue = "";
            while (ListPaths.Count > 0)
            {
                foreach (var listPath in ListPaths)
                {
                    if (lowerValue == "")
                        lowerValue = listPath;
                    else if (Others.ToSingle(listPath.Split('^')[1].Split('@')[0].Trim()) < Others.ToSingle(lowerValue.Split('^')[1].Split('@')[0].Trim()))
                    {
                        lowerValue = listPath;
                    }
                }
                ListPaths.Remove(lowerValue);
                listPathFinal.Add(lowerValue);
                lowerValue = "";
            }
            return listPathFinal; // listPathFinal;
        }

        public static string ExtractCurrentTaxiInfo()
        {
            var allPaths = ExtractAllPathsTaxi();
            for (int i = 0; i < allPaths.Count - 1; i++)
            {
                Application.DoEvents();
                string taxi = allPaths[i];
                string routes = taxi.Split('@')[1].Split('~')[0];
                string type = taxi.Split('~')[1];

                if (routes != "0" || type == "REACHABLE")
                    continue;
                if (type == "CURRENT")
                {
                    Logging.WriteDebug("Current taxi is : ");
                    Logging.WriteDebug(taxi);
                    return taxi;
                }
            }
            return "";
        }
        public static string FirstReachable = "";
        public static string ExtractNextTaxiInfo()
        {
            bool currentFound = false;
            var allPaths = ExtractAllPathsTaxi();
            for (int i = 0; i < allPaths.Count - 1; i++)
            {
                Application.DoEvents();
                string taxi = allPaths[i];
                string type = taxi.Split('~')[1];

                if (currentFound && type == "REACHABLE")
                    return taxi;

                if (type == "REACHABLE" && FirstReachable == "")
                    FirstReachable = taxi;
                if (FirstReachable == "")
                    FirstReachable = allPaths[0];
                if (type == "CURRENT")
                {
                    currentFound = true;
                }
            }
            return FirstReachable; // loop to the first taxi if current is the last
        }

        public static List<String> ExtractDirectPathTaxiInfoList()
        {
            Logging.WriteDebug("Begin ExtractDirectPathTaxiInfoList from NPC " + ObjectManager.Me.Target.GetWoWId);
            List<String> taxis = new List<String>();
            var allPaths = ExtractAllPathsTaxi();
            for (int i = 0; i < allPaths.Count - 1; i++)
            {
                Application.DoEvents();
                string taxi = allPaths[i];
                string routes = taxi.Split('@')[1].Split('~')[0];
                if (routes == "1") // always reachable or it would be "0" hop.
                {
                    Logging.WriteDebug(taxi);
                    taxis.Add(taxi);
                }
            }
            return taxis;
        }

        private static List<Taxi> _availableTaxis;
        private static List<TaxiLink> _availableTaxiLinks;

        private static bool TaxiListContainsTaxiId(uint id)
        {
            foreach (Taxi taxi in _availableTaxis)
            {
                Application.DoEvents();
                if (taxi.Id == id)
                    return true;
            }
            return false;
        }

        private static Taxi GetTaxiFromTaxiId(uint id)
        {
            foreach (Taxi taxi in _availableTaxis)
            {
                Application.DoEvents();
                if (taxi.Id == id)
                    return taxi;
            }
            return new Taxi();
        }

        private bool CheckForFrame(string FrameName)
        {
            string MyString = Others.GetRandomString(5);
            string LuaString = "if (" + FrameName + " and " + FrameName + ":IsVisible()) " +
                               MyString + " = \"true\" " +
                               "end";
            string res = Lua.LuaDoString(LuaString, MyString);
            return (res == "true");
        }

        private static bool TaxiLinksNeedsMoreCleanUp()
        {
            foreach (TaxiLink availableTaxiLink in _availableTaxiLinks)
            {
                bool found = false;
                if (availableTaxiLink.PointB == 0)
                    continue;
                foreach (TaxiLink taxiLink in _availableTaxiLinks)
                {
                    if (taxiLink.PointB == 0)
                        continue;
                    if (availableTaxiLink.PointA == taxiLink.PointA && availableTaxiLink.PointB == taxiLink.PointB)
                    {
                        if (found)
                        {
                            Logging.Write("PointA = " + taxiLink.PointA + " PointB = " + taxiLink.PointB);
                            Logging.Write("Simple Duplicated removed.");
                            _availableTaxiLinks.Remove(taxiLink);
                            return true;
                        }
                        found = true;
                    }
                    if (availableTaxiLink.PointA == taxiLink.PointB && availableTaxiLink.PointB == taxiLink.PointA)
                    {
                        if (found)
                        {
                            Logging.Write("PointA = " + taxiLink.PointA + " PointB = " + taxiLink.PointB);
                            Logging.Write("Inverted Duplicated removed.");
                            _availableTaxiLinks.Remove(taxiLink);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private static void DoTaxiLinksCleaning()
        {
            _availableTaxiLinks = XmlSerializer.Deserialize<List<TaxiLink>>(Application.StartupPath + @"\Data\TaxiLinks.xml");
            while (TaxiLinksNeedsMoreCleanUp())
            {
                Thread.Sleep(1);
                Application.DoEvents();
            }
            XmlSerializer.Serialize(Application.StartupPath + @"\Data\TaxiLinks.xml", _availableTaxiLinks);
        }

        private static void MergeFiles()
        {
            List<Npc> qesterListOriginal = XmlSerializer.Deserialize<List<Npc>>(Application.StartupPath + "\\Data\\- QuestersDB.xml");
            List<Npc> qesterListOther = XmlSerializer.Deserialize<List<Npc>>(Application.StartupPath + "\\Data\\QuestersDB.xml");
            List<Npc> qesterListResult = new List<Npc>();
            foreach (Npc quester in qesterListOriginal)
            {
                if (qesterListResult.Find(x => x.Entry == quester.Entry && x.Type == quester.Type && x.Position.DistanceTo(quester.Position) < 60) == null)
                    qesterListResult.Add(quester);
            }
            foreach (Npc quester in qesterListOther)
            {
                if (qesterListResult.Find(x => x.Entry == quester.Entry && x.Type == quester.Type && x.Position.DistanceTo(quester.Position) < 60) == null)
                    qesterListResult.Add(quester);
            }
            qesterListResult.Sort(delegate(Npc x, Npc y)
                {
                    if (x.Entry == y.Entry)
                        return (x.Type < y.Type ? -1 : 1);
                    return (x.Entry < y.Entry ? -1 : 1);
                });
            XmlSerializer.Serialize(Application.StartupPath + "\\Data\\QuestersDBnew.xml", qesterListResult);

            List<Npc> npcListOriginal = XmlSerializer.Deserialize<List<Npc>>(Application.StartupPath + "\\Data\\- NpcDB.xml");
            List<Npc> npcListOther = XmlSerializer.Deserialize<List<Npc>>(Application.StartupPath + "\\Data\\NpcDB.xml");
            List<Npc> npcListResult = new List<Npc>();
            foreach (Npc npc in npcListOriginal)
            {
                if (npcListResult.Find(x => x.Entry == npc.Entry && x.Type == npc.Type && x.Position.DistanceTo(npc.Position) < 30) == null)
                    npcListResult.Add(npc);
            }
            foreach (Npc npc in npcListOther)
            {
                if (npcListResult.Find(x => x.Entry == npc.Entry && x.Type == npc.Type && x.Position.DistanceTo(npc.Position) < 30) == null)
                    npcListResult.Add(npc);
            }
            npcListResult.Sort(delegate(Npc x, Npc y)
            {
                if (x.Entry == y.Entry)
                    return (x.Type < y.Type ? -1 : 1);
                return (x.Entry < y.Entry ? -1 : 1);
            });
            XmlSerializer.Serialize(Application.StartupPath + "\\Data\\NpcDBnew.xml", npcListResult);
        }

        public static bool Pulse()
        {
            try
            {
                // Update spell list
                //SpellManager.UpdateSpellBook();
                DoTaxiLinksCleaning();
                //MergeFiles();
                RadarThread.Start();
                TaxiThread.Start();

                while (TaxiThread.IsAlive)
                {
                    Application.DoEvents();
                    Thread.Sleep(100);
                }
                DoTaxiLinksCleaning();
                /*var sw = new StreamWriter(Application.StartupPath + "\\spell.txt", true, Encoding.UTF8);
                for (uint i = 1; i <= 200000; i += 2500)
                {
                    var listSpell = new List<uint>();
                    for (uint i2 = i; i2 < i + 2500; i2++)
                    {
                        listSpell.Add(i2);
                    }
                    SpellManager.SpellInfoCreateCache(listSpell);
                    foreach (uint u in listSpell)
                    {
                        Application.DoEvents();
                        var spellName = SpellManager.GetSpellInfo(u);
                        if (!string.IsNullOrEmpty(spellName.Name))
                            sw.Write(u + ";" + spellName.Name + Environment.NewLine);
                    }
                }
                sw.Close();
                //Cheat.AntiAfkPulse();
                /*
                //D3D9
                const int VMT_ENDSCENE = 42;
                using (var d3d = new Direct3D())
                {
                    using (var tmpDevice = new Device(d3d, 0, DeviceType.Hardware, IntPtr.Zero, CreateFlags.HardwareVertexProcessing, new PresentParameters() { BackBufferWidth = 1, BackBufferHeight = 1 }))
                    {
                        var EndScenePointer = nManager.Wow.Memory.WowMemory.Memory.ReadUInt(nManager.Wow.Memory.WowMemory.Memory.ReadUInt((uint)tmpDevice.ComPointer) + VMT_ENDSCENE * 4);
                    }
                }*/
            }
            catch (Exception exception)
            {
                Logging.WriteError("Test product: " + exception);
            }
            return false;
        }

        public static
            void Dispose
            ()
        {
        }
    }
}