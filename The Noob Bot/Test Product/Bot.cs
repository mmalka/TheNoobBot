using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using nManager.Wow.Class;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;

namespace Test_Product
{
    internal class Bot
    {
        /*private const string CurrentProfileName = "afk.xml";
        private static bool _xmlProfile = true;
        private static BattlegrounderProfile _currentProfile = new BattlegrounderProfile();*/

        public static bool Pulse()
        {
            var myConn =
                new MySqlConnection(
                    "server=over-game.eu; user id=thenoobbot_npcdb; password=4KJmBv5u3VRaVPwV; database=thenoobbot_npcdb;");
            myConn.Open();
            const string mySqlQuery =
                "SELECT ct.entry, ct.name, ct.subname, ct.faction_A, ct.faction_H, ct.npcflag, c.map, c.position_x, c.position_y, c.position_z " +
                "FROM `creature_template` AS ct, `creature` AS c " +
                "WHERE c.id = ct.entry && c.phaseMask =1 && c.spawnMask&15 && " +
                "( ! ( c.unit_flags &33554432 ) && ! ( c.unit_flags &262144 ) && ! ( c.unit_flags &524288 ) ) && " +
                "!(ct.npcflag &1) && " +
                // NPC without GOSSIP menu for now, we need to handle GOSSIP correctly in the bot first.
                "((ct.npcflag &64 && trainer_type=2) || ct.npcflag &128 || ct.npcflag &4096 || ct.npcflag &8192 || ct.npcflag &16384 || ct.npcflag &32768 || ct.npcflag &65536 || ct.npcflag &131072 || ct.npcflag &1048576 || ct.npcflag &2097152 || ct.npcflag &4194304 || ct.npcflag &8388608 || ct.npcflag &67108864) && " +
                "( ! ( ct.unit_flags &33554432 ) && ! ( ct.unit_flags &262144 ) && ! ( ct.unit_flags &524288 ) ) && ! ( ct.flags_extra &128 ) && " +
                "ct.entry NOT IN (SELECT `npc_entry` FROM (`creature_transport`)) " +
                "GROUP BY ct.entry;";
            var sqlCmd = new MySqlCommand(mySqlQuery, myConn);
            var result = sqlCmd.ExecuteReader();
            var newList = new List<Npc>();
            var currentFaction = new Npc.FactionType();
            while (result.Read())
            {
                if (UnitRelation.GetReaction(5, result.GetUInt32("faction_H")) ==
                    Reaction.Friendly &&
                    UnitRelation.GetReaction(3, result.GetUInt32("faction_A")) ==
                    Reaction.Friendly ||
                    UnitRelation.GetReaction(5, result.GetUInt32("faction_H")) ==
                    Reaction.Neutral)
                {
                    // neutral
                    currentFaction = Npc.FactionType.Neutral;
                }
                else if (UnitRelation.GetReaction(5, result.GetUInt32("faction_H")) ==
                         Reaction.Friendly)
                {
                    // horde
                    currentFaction = Npc.FactionType.Horde;
                }
                else if (UnitRelation.GetReaction(3, result.GetUInt32("faction_A")) ==
                         Reaction.Friendly)
                {
                    // alliance
                    currentFaction = Npc.FactionType.Alliance;
                }
                if (Convert.ToBoolean(result.GetUInt32("npcflag") & 64))
                {
                    var newtype = Npc.NpcType.None;
                    if (result.GetString("subname").Contains("Alchemy"))
                        newtype = Npc.NpcType.AlchemyTrainer;
                    else if (result.GetString("subname").Contains("Blacksmithing"))
                        newtype = Npc.NpcType.BlacksmithingTrainer;
                    else if (result.GetString("subname").Contains("Enchanting"))
                        newtype = Npc.NpcType.EnchantingTrainer;
                    else if (result.GetString("subname").Contains("Engineering"))
                        newtype = Npc.NpcType.EngineeringTrainer;
                    else if (result.GetString("subname").Contains("Herbalism"))
                        newtype = Npc.NpcType.HerbalismTrainer;
                    else if (result.GetString("subname").Contains("Inscription"))
                        newtype = Npc.NpcType.InscriptionTrainer;
                    else if (result.GetString("subname").Contains("Jewelcrafting"))
                        newtype = Npc.NpcType.JewelcraftingTrainer;
                    else if (result.GetString("subname").Contains("Leatherworking"))
                        newtype = Npc.NpcType.LeatherworkingTrainer;
                    else if (result.GetString("subname").Contains("Mining"))
                        newtype = Npc.NpcType.MiningTrainer;
                    else if (result.GetString("subname").Contains("Skinning"))
                        newtype = Npc.NpcType.SkinningTrainer;
                    else if (result.GetString("subname").Contains("Tailoring"))
                        newtype = Npc.NpcType.TailoringTrainer;
                    else if (result.GetString("subname").Contains("Archaeology"))
                        newtype = Npc.NpcType.ArchaeologyTrainer;
                    else if (result.GetString("subname").Contains("Cooking"))
                        newtype = Npc.NpcType.CookingTrainer;
                    else if (result.GetString("subname").Contains("First Aid"))
                        newtype = Npc.NpcType.FirstAidTrainer;
                    else if (result.GetString("subname").Contains("Fishing"))
                        newtype = Npc.NpcType.FishingTrainer;
                    else if (result.GetString("subname").Contains("Riding"))
                        newtype = Npc.NpcType.RidingTrainer;
                    if (newtype != Npc.NpcType.None)
                        newList.Add(new Npc
                            {
                                ContinentId = (ContinentId) result.GetUInt32("map"),
                                Entry = result.GetInt32("entry"),
                                Faction = currentFaction,
                                Name = result.GetString("name"),
                                Position =
                                    new Point(result.GetFloat("position_x"), result.GetFloat("position_y"),
                                              result.GetFloat("position_z")),
                                SelectGossipOption = 1,
                                Type = newtype
                            });
                }
                if (Convert.ToBoolean(result.GetUInt32("npcflag") & 128))
                {
                    newList.Add(new Npc
                        {
                            ContinentId = (ContinentId) result.GetUInt32("map"),
                            Entry = result.GetInt32("entry"),
                            Faction = currentFaction,
                            Name = result.GetString("name"),
                            Position =
                                new Point(result.GetFloat("position_x"), result.GetFloat("position_y"),
                                          result.GetFloat("position_z")),
                            SelectGossipOption = 1,
                            Type = Npc.NpcType.Vendor
                        });
                }
                if (Convert.ToBoolean(result.GetUInt32("npcflag") & 4096))
                {
                    newList.Add(new Npc
                        {
                            ContinentId = (ContinentId) result.GetUInt32("map"),
                            Entry = result.GetInt32("entry"),
                            Faction = currentFaction,
                            Name = result.GetString("name"),
                            Position =
                                new Point(result.GetFloat("position_x"), result.GetFloat("position_y"),
                                          result.GetFloat("position_z")),
                            SelectGossipOption = 1,
                            Type = Npc.NpcType.Repair
                        });
                }
                if (Convert.ToBoolean(result.GetUInt32("npcflag") & 8192))
                {
                    newList.Add(new Npc
                        {
                            ContinentId = (ContinentId) result.GetUInt32("map"),
                            Entry = result.GetInt32("entry"),
                            Faction = currentFaction,
                            Name = result.GetString("name"),
                            Position =
                                new Point(result.GetFloat("position_x"), result.GetFloat("position_y"),
                                          result.GetFloat("position_z")),
                            SelectGossipOption = 1,
                            Type = Npc.NpcType.FlightMaster
                        });
                }
                if (Convert.ToBoolean(result.GetUInt32("npcflag") & 16384))
                {
                    newList.Add(new Npc
                        {
                            ContinentId = (ContinentId) result.GetUInt32("map"),
                            Entry = result.GetInt32("entry"),
                            Faction = currentFaction,
                            Name = result.GetString("name"),
                            Position =
                                new Point(result.GetFloat("position_x"), result.GetFloat("position_y"),
                                          result.GetFloat("position_z")),
                            SelectGossipOption = 1,
                            Type = Npc.NpcType.SpiritHealer
                        });
                }
                if (Convert.ToBoolean(result.GetUInt32("npcflag") & 32768))
                {
                    newList.Add(new Npc
                        {
                            ContinentId = (ContinentId) result.GetUInt32("map"),
                            Entry = result.GetInt32("entry"),
                            Faction = currentFaction,
                            Name = result.GetString("name"),
                            Position =
                                new Point(result.GetFloat("position_x"), result.GetFloat("position_y"),
                                          result.GetFloat("position_z")),
                            SelectGossipOption = 1,
                            Type = Npc.NpcType.SpiritGuide
                        });
                }
                if (Convert.ToBoolean(result.GetUInt32("npcflag") & 65536))
                {
                    newList.Add(new Npc
                        {
                            ContinentId = (ContinentId) result.GetUInt32("map"),
                            Entry = result.GetInt32("entry"),
                            Faction = currentFaction,
                            Name = result.GetString("name"),
                            Position =
                                new Point(result.GetFloat("position_x"), result.GetFloat("position_y"),
                                          result.GetFloat("position_z")),
                            SelectGossipOption = 1,
                            Type = Npc.NpcType.Innkeeper
                        });
                }
                if (Convert.ToBoolean(result.GetUInt32("npcflag") & 131072))
                {
                    newList.Add(new Npc
                        {
                            ContinentId = (ContinentId) result.GetUInt32("map"),
                            Entry = result.GetInt32("entry"),
                            Faction = currentFaction,
                            Name = result.GetString("name"),
                            Position =
                                new Point(result.GetFloat("position_x"), result.GetFloat("position_y"),
                                          result.GetFloat("position_z")),
                            SelectGossipOption = 1,
                            Type = Npc.NpcType.Banker
                        });
                }
                if (Convert.ToBoolean(result.GetUInt32("npcflag") & 1048576))
                {
                    newList.Add(new Npc
                        {
                            ContinentId = (ContinentId) result.GetUInt32("map"),
                            Entry = result.GetInt32("entry"),
                            Faction = currentFaction,
                            Name = result.GetString("name"),
                            Position =
                                new Point(result.GetFloat("position_x"), result.GetFloat("position_y"),
                                          result.GetFloat("position_z")),
                            SelectGossipOption = 1,
                            Type = Npc.NpcType.Battlemaster
                        });
                }
                if (Convert.ToBoolean(result.GetUInt32("npcflag") & 2097152))
                {
                    newList.Add(new Npc
                        {
                            ContinentId = (ContinentId) result.GetUInt32("map"),
                            Entry = result.GetInt32("entry"),
                            Faction = currentFaction,
                            Name = result.GetString("name"),
                            Position =
                                new Point(result.GetFloat("position_x"), result.GetFloat("position_y"),
                                          result.GetFloat("position_z")),
                            SelectGossipOption = 1,
                            Type = Npc.NpcType.Auctioneer
                        });
                }
                if (Convert.ToBoolean(result.GetUInt32("npcflag") & 4194304))
                {
                    newList.Add(new Npc
                        {
                            ContinentId = (ContinentId) result.GetUInt32("map"),
                            Entry = result.GetInt32("entry"),
                            Faction = currentFaction,
                            Name = result.GetString("name"),
                            Position =
                                new Point(result.GetFloat("position_x"), result.GetFloat("position_y"),
                                          result.GetFloat("position_z")),
                            SelectGossipOption = 1,
                            Type = Npc.NpcType.StableMaster
                        });
                }
                if (Convert.ToBoolean(result.GetUInt32("npcflag") & 8388608))
                {
                    newList.Add(new Npc
                        {
                            ContinentId = (ContinentId) result.GetUInt32("map"),
                            Entry = result.GetInt32("entry"),
                            Faction = currentFaction,
                            Name = result.GetString("name"),
                            Position =
                                new Point(result.GetFloat("position_x"), result.GetFloat("position_y"),
                                          result.GetFloat("position_z")),
                            SelectGossipOption = 1,
                            Type = Npc.NpcType.GuildBanker
                        });
                }
                if (Convert.ToBoolean(result.GetUInt32("npcflag") & 67108864))
                {
                    newList.Add(new Npc
                        {
                            ContinentId = (ContinentId) result.GetUInt32("map"),
                            Entry = result.GetInt32("entry"),
                            Faction = currentFaction,
                            Name = result.GetString("name"),
                            Position =
                                new Point(result.GetFloat("position_x"), result.GetFloat("position_y"),
                                          result.GetFloat("position_z")),
                            SelectGossipOption = 1,
                            Type = Npc.NpcType.Mailbox
                        });
                }
            }
            NpcDB.BuildNewList(newList);
            newList.Clear();
            return result.HasRows;
            /*if (_xmlProfile)
            {
                _currentProfile = new BattlegrounderProfile();
                if (File.Exists(Application.StartupPath + "\\Profiles\\Battlegrounder\\" +
                                CurrentProfileName))
                {
                    _currentProfile =
                        XmlSerializer.Deserialize<BattlegrounderProfile>(Application.StartupPath +
                                                                         "\\Profiles\\Battlegrounder\\" +
                                                                         CurrentProfileName);
                    if (_currentProfile.IsValid())
                    {
                        _xmlProfile = false;
                        var listP = new List<Point>();
                        foreach (Point points in _currentProfile.BattlegrounderZones[0].Points)
                        {
                            Logging.Write("listP.Add(new Point((float) " + points.X + ", (float) " + points.Y +
                                          ", (float) " + points.Z + "));");
                        }
                    }
                }
                _xmlProfile = false;
            }
            else
            {
                var afkPointsWG = new List<Point>
                    {
                        new Point((float) 982.9947, (float) 1432.755, (float) 367.0161),
                        new Point((float) 982.5961, (float) 1430.552, (float) 367.0203),
                        new Point((float) 981.0853, (float) 1422.228, (float) 367.0353),
                        new Point((float) 979.8093, (float) 1420.264, (float) 367.125),
                        new Point((float) 977.5601, (float) 1419.583, (float) 367.2351),
                        new Point((float) 975.6661, (float) 1420.291, (float) 367.2938),
                        new Point((float) 974.4164, (float) 1422.272, (float) 367.3008),
                        new Point((float) 974.7516, (float) 1424.621, (float) 367.236),
                        new Point((float) 976.4982, (float) 1426.171, (float) 367.1609),
                        new Point((float) 978.4533, (float) 1426.769, (float) 367.1197),
                        new Point((float) 979.8653, (float) 1428.206, (float) 367.0862),
                        new Point((float) 980.1599, (float) 1430.545, (float) 367.085),
                        new Point((float) 978.8823, (float) 1432.508, (float) 367.1245),
                        new Point((float) 976.5683, (float) 1433.182, (float) 367.1879),
                        new Point((float) 974.4132, (float) 1432.19, (float) 367.2423),
                        new Point((float) 973.475, (float) 1430.416, (float) 367.2567),
                        new Point((float) 973.8593, (float) 1428.09, (float) 367.225),
                        new Point((float) 974.8751, (float) 1426.355, (float) 367.1908),
                        new Point((float) 974.887, (float) 1424.333, (float) 367.2365),
                        new Point((float) 973.7601, (float) 1422.663, (float) 367.2998),
                        new Point((float) 971.5264, (float) 1421.91, (float) 367.2932),
                        new Point((float) 969.6051, (float) 1422.564, (float) 367.29),
                        new Point((float) 967.8405, (float) 1424.193, (float) 367.29),
                        new Point((float) 965.8826, (float) 1424.633, (float) 367.29),
                        new Point((float) 963.7526, (float) 1423.664, (float) 367.2903),
                        new Point((float) 962.796, (float) 1421.892, (float) 367.2921),
                        new Point((float) 962.6562, (float) 1419.828, (float) 367.2935),
                        new Point((float) 961.345, (float) 1417.922, (float) 367.2947),
                        new Point((float) 959.0137, (float) 1417.887, (float) 367.2966),
                        new Point((float) 956.8498, (float) 1418.931, (float) 367.2975),
                        new Point((float) 955.9509, (float) 1420.734, (float) 367.2975),
                        new Point((float) 956.4103, (float) 1423.077, (float) 367.2961),
                        new Point((float) 958.2764, (float) 1424.542, (float) 367.2947),
                        new Point((float) 960.6763, (float) 1424.426, (float) 367.2933),
                        new Point((float) 962.3994, (float) 1422.773, (float) 367.2933),
                        new Point((float) 962.7288, (float) 1420.78, (float) 367.2933),
                        new Point((float) 963.53, (float) 1418.578, (float) 367.2933),
                        new Point((float) 965.6325, (float) 1418.027, (float) 367.2933),
                        new Point((float) 968.0612, (float) 1418.327, (float) 367.2918),
                        new Point((float) 972.0192, (float) 1419.052, (float) 367.2928),
                        new Point((float) 973.6957, (float) 1420.21, (float) 367.2976),
                        new Point((float) 974.4346, (float) 1422.105, (float) 367.299),
                        new Point((float) 1000.892, (float) 1590.354, (float) 334.4754),
                        new Point((float) 998.7656, (float) 1590.773, (float) 333.6605),
                        new Point((float) 996.4114, (float) 1591.811, (float) 333.1363),
                        new Point((float) 994.5696, (float) 1593.631, (float) 332.8185),
                        new Point((float) 993.3889, (float) 1595.947, (float) 332.6086),
                        new Point((float) 991.8482, (float) 1597.882, (float) 332.0692),
                        new Point((float) 989.6449, (float) 1599.211, (float) 331.5488),
                        new Point((float) 987.063, (float) 1599.671, (float) 331.2907),
                        new Point((float) 984.5206, (float) 1599.179, (float) 331.4182),
                        new Point((float) 982.3218, (float) 1597.811, (float) 331.7794),
                        new Point((float) 980.764, (float) 1595.763, (float) 332.2285),
                        new Point((float) 980.0327, (float) 1593.279, (float) 331.7405),
                        new Point((float) 980.2413, (float) 1590.681, (float) 331.3929),
                        new Point((float) 981.3694, (float) 1588.332, (float) 331.8931),
                        new Point((float) 983.701, (float) 1588.861, (float) 331.5182),
                        new Point((float) 985.2396, (float) 1590.974, (float) 331.9752),
                        new Point((float) 985.9928, (float) 1593.483, (float) 332.1008),
                        new Point((float) 988.2136, (float) 1595.562, (float) 332.2313),
                        new Point((float) 989.9907, (float) 1593.686, (float) 332.4543),
                        new Point((float) 991.316, (float) 1591.367, (float) 332.5291),
                        new Point((float) 993.5851, (float) 1590.047, (float) 332.9106),
                        new Point((float) 1083.667, (float) 1637.089, (float) 330.8144),
                        new Point((float) 1083.586, (float) 1639.103, (float) 330.8497),
                        new Point((float) 1083.35, (float) 1645.029, (float) 330.9558),
                        new Point((float) 1083.753, (float) 1647.572, (float) 330.7704),
                        new Point((float) 1084.91, (float) 1651.748, (float) 330.1223),
                        new Point((float) 1084.757, (float) 1654.269, (float) 329.9672),
                        new Point((float) 1082.184, (float) 1654.472, (float) 331.1662),
                        new Point((float) 1080.392, (float) 1654.345, (float) 332.3298),
                        new Point((float) 1078.13, (float) 1655.564, (float) 333.6623),
                        new Point((float) 1076.008, (float) 1657.102, (float) 335.154),
                        new Point((float) 1076.222, (float) 1659.428, (float) 334.6195),
                        new Point((float) 1077.378, (float) 1660.804, (float) 333.4773),
                        new Point((float) 1078.724, (float) 1662.041, (float) 332.2591),
                        new Point((float) 1080.882, (float) 1663.273, (float) 330.929),
                        new Point((float) 1086.319, (float) 1666.108, (float) 328.0335),
                        new Point((float) 1088.555, (float) 1667.19, (float) 327.0331),
                        new Point((float) 1090.437, (float) 1667.338, (float) 326.3665),
                        new Point((float) 1092.926, (float) 1666.578, (float) 325.6138),
                        new Point((float) 1097.058, (float) 1665.216, (float) 324.5133),
                        new Point((float) 1099.608, (float) 1664.83, (float) 323.8292),
                        new Point((float) 1101.58, (float) 1666.356, (float) 323.1497),
                        new Point((float) 1102.964, (float) 1668.618, (float) 322.6722),
                        new Point((float) 1104.042, (float) 1670.968, (float) 322.5992),
                        new Point((float) 1104.878, (float) 1673.381, (float) 322.6988),
                        new Point((float) 1104.728, (float) 1677.432, (float) 322.9559),
                        new Point((float) 1105.718, (float) 1679.822, (float) 323.4667),
                        new Point((float) 1106.909, (float) 1682.287, (float) 323.5345),
                        new Point((float) 1108.127, (float) 1684.57, (float) 323.8902),
                        new Point((float) 1111.055, (float) 1689.843, (float) 325.2816),
                        new Point((float) 1113.339, (float) 1690.557, (float) 326.4582),
                        new Point((float) 1115.069, (float) 1690.162, (float) 327.4368),
                        new Point((float) 1116.83, (float) 1689.46, (float) 329.3797),
                        new Point((float) 1118.294, (float) 1688.296, (float) 331.2055),
                        new Point((float) 1119.557, (float) 1687.813, (float) 332.7397),
                        new Point((float) 1117.677, (float) 1686.288, (float) 332.4156),
                        new Point((float) 1115.838, (float) 1685.411, (float) 331.0065),
                        new Point((float) 1113.145, (float) 1684.127, (float) 327.4233),
                        new Point((float) 1113.18, (float) 1681.718, (float) 327.4293),
                        new Point((float) 1115.39, (float) 1676.088, (float) 329.8867),
                        new Point((float) 1116.658, (float) 1673.753, (float) 329.7334),
                        new Point((float) 1118.083, (float) 1672.665, (float) 328.6056),
                        new Point((float) 1121.107, (float) 1670.882, (float) 327.5817),
                        new Point((float) 1122.307, (float) 1668.638, (float) 327.3739),
                        new Point((float) 1123.094, (float) 1666.156, (float) 326.8132),
                        new Point((float) 1123.896, (float) 1663.626, (float) 325.7647),
                        new Point((float) 1124.46, (float) 1661.848, (float) 324.7401),
                        new Point((float) 1126.99, (float) 1661.31, (float) 324.4391),
                        new Point((float) 1129.443, (float) 1662.358, (float) 325.1386),
                        new Point((float) 1131.801, (float) 1663.838, (float) 326.0385),
                        new Point((float) 1133.768, (float) 1665.443, (float) 326.9488),
                        new Point((float) 1135.067, (float) 1666.893, (float) 327.6869),
                        new Point((float) 1136.415, (float) 1669.195, (float) 328.7676),
                        new Point((float) 1138.456, (float) 1670.445, (float) 329.7826),
                        new Point((float) 1140.493, (float) 1668.871, (float) 329.8449),
                        new Point((float) 1143.809, (float) 1665.176, (float) 329.2135),
                        new Point((float) 1146.589, (float) 1664.938, (float) 329.7372),
                        new Point((float) 1149.24, (float) 1664.935, (float) 330.2859),
                        new Point((float) 1153.506, (float) 1665.031, (float) 331.2351),
                        new Point((float) 1156.308, (float) 1664.901, (float) 331.5777),
                        new Point((float) 1161.873, (float) 1665.798, (float) 333.2629),
                        new Point((float) 1162.771, (float) 1667.368, (float) 334.12),
                        new Point((float) 1164.027, (float) 1670.683, (float) 336.3004),
                        new Point((float) 1164.929, (float) 1672.187, (float) 337.4863),
                        new Point((float) 1166.353, (float) 1673.355, (float) 338.9576),
                        new Point((float) 1166.13, (float) 1671.353, (float) 337.7474),
                        new Point((float) 1165.588, (float) 1668.583, (float) 335.7238),
                        new Point((float) 1165.33, (float) 1666.653, (float) 334.975),
                        new Point((float) 1165.206, (float) 1664.053, (float) 334.2451),
                        new Point((float) 1167.318, (float) 1663.411, (float) 335.5632),
                        new Point((float) 1174.273, (float) 1663.895, (float) 337.1036),
                        new Point((float) 1176.921, (float) 1664.074, (float) 336.5154),
                        new Point((float) 1181.592, (float) 1662.477, (float) 333.4007),
                        new Point((float) 1181.989, (float) 1659.8, (float) 330.3744),
                        new Point((float) 1181.976, (float) 1658.013, (float) 329.2476),
                        new Point((float) 1182.08, (float) 1656.162, (float) 328.209),
                        new Point((float) 1184.629, (float) 1655.63, (float) 328.5196),
                        new Point((float) 1198.347, (float) 1640.93, (float) 327.6815),
                        new Point((float) 1197.455, (float) 1638.378, (float) 327.3329),
                        new Point((float) 1195.904, (float) 1636.142, (float) 327.0511),
                        new Point((float) 1194.378, (float) 1633.95, (float) 326.5312),
                        new Point((float) 1192.804, (float) 1631.688, (float) 325.484),
                        new Point((float) 1190.78, (float) 1628.778, (float) 323.3735),
                        new Point((float) 1189.715, (float) 1627.247, (float) 321.9185),
                        new Point((float) 1187.757, (float) 1624.434, (float) 318.6245),
                        new Point((float) 1186.145, (float) 1622.117, (float) 315.3091),
                        new Point((float) 1185.262, (float) 1620.215, (float) 312.8096),
                        new Point((float) 1187.515, (float) 1618.836, (float) 312.7059),
                        new Point((float) 1190.075, (float) 1618.019, (float) 312.695),
                        new Point((float) 1191.856, (float) 1616.099, (float) 312.5865),
                        new Point((float) 1194.393, (float) 1612.502, (float) 312.3923),
                        new Point((float) 1196.844, (float) 1611.801, (float) 312.5216),
                        new Point((float) 1197.442, (float) 1608.997, (float) 312.4576),
                        new Point((float) 1272.361, (float) 1322.163, (float) 314.1213),
                        new Point((float) 1273.87, (float) 1320.771, (float) 314.8628),
                        new Point((float) 1274.473, (float) 1318.835, (float) 315.4742),
                        new Point((float) 1276.474, (float) 1318.268, (float) 315.9791),
                        new Point((float) 1278.22, (float) 1316.448, (float) 316.5334),
                        new Point((float) 1279.746, (float) 1315.134, (float) 316.6008),
                        new Point((float) 1280.776, (float) 1313.358, (float) 316.5727),
                        new Point((float) 1280.056, (float) 1315.473, (float) 316.6031),
                        new Point((float) 1278.744, (float) 1317.306, (float) 316.4329),
                        new Point((float) 1280.315, (float) 1318.679, (float) 316.2097),
                        new Point((float) 1282.361, (float) 1317.846, (float) 316.4237),
                        new Point((float) 1283.984, (float) 1316.434, (float) 316.752),
                        new Point((float) 1285.008, (float) 1314.457, (float) 316.748),
                        new Point((float) 1285.224, (float) 1312.415, (float) 316.6688),
                        new Point((float) 1284.258, (float) 1310.431, (float) 316.1708),
                        new Point((float) 1282.952, (float) 1306.723, (float) 315.5386),
                        new Point((float) 1282.303, (float) 1304.534, (float) 315.3908),
                        new Point((float) 1281.383, (float) 1302.519, (float) 315.792),
                        new Point((float) 1279.763, (float) 1300.98, (float) 316.1241),
                        new Point((float) 1277.861, (float) 1300.325, (float) 316.4515),
                        new Point((float) 1279.959, (float) 1300.326, (float) 316.5832),
                        new Point((float) 1281.809, (float) 1301.572, (float) 316.7097),
                        new Point((float) 1288.642, (float) 1304.213, (float) 316.0458),
                        new Point((float) 1290.447, (float) 1305.252, (float) 316.4063),
                        new Point((float) 1292.624, (float) 1305.84, (float) 316.8867),
                        new Point((float) 1294.737, (float) 1306.239, (float) 317.3663),
                        new Point((float) 1297.032, (float) 1306.72, (float) 317.663),
                        new Point((float) 1295.502, (float) 1308.264, (float) 317.3368),
                        new Point((float) 1293.396, (float) 1309.027, (float) 317.0801),
                        new Point((float) 1293.011, (float) 1310.961, (float) 317.6697),
                        new Point((float) 1294.269, (float) 1312.565, (float) 318.5455),
                        new Point((float) 1295.864, (float) 1313.775, (float) 319.4164),
                        new Point((float) 1297.975, (float) 1313.842, (float) 319.8638),
                        new Point((float) 1299.689, (float) 1314.925, (float) 320.5795),
                        new Point((float) 1299.693, (float) 1313.051, (float) 319.8156),
                        new Point((float) 1299.693, (float) 1311.135, (float) 319.1144),
                        new Point((float) 1299.521, (float) 1309.026, (float) 318.375),
                        new Point((float) 1297.294, (float) 1308.952, (float) 317.8903),
                        new Point((float) 1296.021, (float) 1307.226, (float) 317.5197),
                        new Point((float) 1293.989, (float) 1307.803, (float) 317.0305),
                        new Point((float) 1292.566, (float) 1309.262, (float) 316.9726),
                        new Point((float) 1291.238, (float) 1311.2, (float) 317.3637),
                        new Point((float) 1292.463, (float) 1312.805, (float) 318.2063),
                        new Point((float) 1294.175, (float) 1313.697, (float) 318.9976),
                        new Point((float) 1296.29, (float) 1314.382, (float) 319.7507),
                        new Point((float) 1297.932, (float) 1315.789, (float) 320.6294),
                        new Point((float) 1299.009, (float) 1317.627, (float) 321.4706),
                        new Point((float) 1298.187, (float) 1319.591, (float) 321.7651),
                        new Point((float) 1296.286, (float) 1320.576, (float) 321.3784),
                        new Point((float) 1296.437, (float) 1322.699, (float) 321.1384),
                        new Point((float) 1297.75, (float) 1324.53, (float) 321.3198),
                        new Point((float) 1298.466, (float) 1326.635, (float) 321.4613),
                        new Point((float) 1299.659, (float) 1328.568, (float) 322.0152),
                        new Point((float) 1299.631, (float) 1330.815, (float) 322.1299),
                        new Point((float) 1297.782, (float) 1331.131, (float) 321.2201),
                        new Point((float) 1295.983, (float) 1330.917, (float) 320.2675),
                        new Point((float) 1294.235, (float) 1331.471, (float) 319.282),
                        new Point((float) 1292.372, (float) 1331.51, (float) 318.3777),
                        new Point((float) 1290.596, (float) 1331.897, (float) 317.3133),
                        new Point((float) 1288.999, (float) 1333.087, (float) 316.8307),
                        new Point((float) 1287.263, (float) 1334.599, (float) 316.3406),
                        new Point((float) 1286.999, (float) 1336.572, (float) 316.6422),
                        new Point((float) 1287.861, (float) 1338.318, (float) 317.3403),
                        new Point((float) 1289.241, (float) 1339.763, (float) 318.1519),
                        new Point((float) 1290.705, (float) 1340.857, (float) 318.9806),
                        new Point((float) 1295.75, (float) 1342.876, (float) 320.3156),
                        new Point((float) 1302.323, (float) 1344.641, (float) 320.9724),
                        new Point((float) 1304.421, (float) 1345.211, (float) 321.1412),
                        new Point((float) 1308.448, (float) 1347.246, (float) 322.3723),
                        new Point((float) 1309.387, (float) 1349.027, (float) 322.6274),
                        new Point((float) 1310.728, (float) 1350.703, (float) 322.4273),
                        new Point((float) 1312.383, (float) 1351.973, (float) 322.1263),
                        new Point((float) 1314.646, (float) 1353.127, (float) 322.1216),
                        new Point((float) 1316.47, (float) 1354.169, (float) 321.7124),
                        new Point((float) 1318.481, (float) 1354.918, (float) 321.5745),
                        new Point((float) 1320.717, (float) 1355.114, (float) 321.7266),
                        new Point((float) 1327.698, (float) 1358.35, (float) 321.127),
                        new Point((float) 1329.656, (float) 1359.334, (float) 320.7422),
                        new Point((float) 1331.949, (float) 1359.129, (float) 320.7488),
                        new Point((float) 1334.127, (float) 1358.832, (float) 321.1953),
                        new Point((float) 1336.31, (float) 1358.371, (float) 322.1201),
                        new Point((float) 1338.449, (float) 1358.634, (float) 322.7827)
                    };
            }
            return true;
            /*var t = nManager.Wow.ObjectManager.ObjectManager.Me.GetPowerByPowerType(PowerType.Energy);
            var tMax = nManager.Wow.ObjectManager.ObjectManager.Me.GetMaxPowerByPowerType(PowerType.Energy);

            var obj = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWGameObject(
                nManager.Wow.ObjectManager.ObjectManager.GetObjectWoWGameObject());
            var test = obj.GOType;


            var skill = nManager.Wow.Helpers.Skill.GetMaxValue(SkillLine.Fishing);
            //var tbreak = 1;

            //var idEquiped = nManager.Wow.ObjectManager.ObjectManager.Me.GetDescriptor<uint>(Descriptors.PlayerFields.VisibleItems + 15 * 2);
            /*
            DBC<DBCStruct.SpellRec> DBCSpell = new DBC<DBCStruct.SpellRec>((int)Addresses.DBC.spell);
            var sw = new StreamWriter(Application.StartupPath + "\\spell.txt", true, Encoding.UTF8);
            for (int i = 0; i < DBCSpell.MaxIndex - 1; i++)
            {
                if (DBCSpell.HasRow(i))
                {
                    var t = DBCSpell.GetRow(i);

                    sw.Write(t.SpellId + ";" + Memory.WowMemory.Memory.ReadUTF8String(t.Name) + Environment.NewLine);
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
            }
            
            return false;

            var t = new []
                        {
"###############################################################",
"#xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx#",
"##xxx##############################################x#########x#",
"####xx#############################################x#########x#",
"#####xx############################################x#########x#",
"######xx###########################################x###########",
"#######xx##########################################xxxxxxxxxxx#",
"########xx###################################################x#",
"#########xx##################################################x#",
"##########x##################################################x#",
"##xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx#########x#",
"##x#########xx###############################################x#",
"##x##########xx##############################################x#",
"##x###########xx#############################################x#",
"##x############xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx#########x#",
"##x#############xx#############################################",
"##x##############xx############################################",
"##xxx#############xx###########################################",
"###################xx##########################################",
"####################xx#########################################",
"#####################xx########################################",
"##x#######xxxxxxxxxxxxxx#######################################",
"##x#######x############xx######################################",
"##x#####xxx#############xx#####################################",
"##x######################xx####################################",
"##xxxxxxxx#x###x##########xx###################################",
"##x######x#x###############xxxxxxxxxxxxxxxxxxxxxxxxx###########",
"##x######xxxx#x#####x#######xx#################################",
"##x########x#################xx################################",
"##x########x##################xxxxxxxx#########################",
"##x########x###################x###############################",
"#xxxxxxxxx#xxxxxxxxxxxxxxxxxxxxxx##############################",
"###############################################################",
                        }; // <> = X = c ; ^ = y = l

            var r = "";

            foreach (var lr in t)
            {
                r += lr + Environment.NewLine;
            }

            var listP = new List<Point>();

            for (int l = 0; l <= t.Length-1; l++)
            {
                for (int c = 0; c <= t[l].ToCharArray().Length-1; c++)
                {
                    if (t[l].ToCharArray()[c].ToString() == "x")
                        listP.Add(new Point(c, l, 0));
                }
            }
            var time = nManager.Helpful.Others.Times;
            var path = nManager.Wow.Helpers.PathFinder.FindPath(new Point(-10, 31, 0), new Point(61, 14, 0), listP, 10f, 10f);
            time = nManager.Helpful.Others.Times - time;
            var n = 0;
            foreach (var point in path)
            {
                var l = t[(int)point.Y].ToCharArray();
                l[(int)point.X] = Convert.ToChar(n.ToString());
                t[(int)point.Y] = new String(l);
                n++;
                if (n > 9)
                    n = 1;
            }

            r += Environment.NewLine + Environment.NewLine + "Path count: " + path.Count + " | " + time + " ms" +  Environment.NewLine;
            foreach (var lr in t)
            {
                r += lr + Environment.NewLine;
            }

            nManager.Helpful.Others.WriteFile("TESTTEST.txt", r);
            */
        }

        public static
            void Dispose
            ()
        {
        }
    }
}