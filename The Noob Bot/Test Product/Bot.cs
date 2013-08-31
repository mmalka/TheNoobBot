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

        public static bool Pulse()
        {
            try
            {
                /*
                var myConn =
                    new MySqlConnection(
                        "server=over-game.eu; user id=thenoobbot_npcdb; password=4KJmBv5u3VRaVPwV; database=thenoobbot_npcdb;");
                myConn.Open();
                const string npcquery =
                    "SELECT ct.entry, ct.name, ct.subname, ct.faction_A, ct.faction_H, ct.npcflag, c.map, c.position_x, c.position_y, c.position_z " +
                    "FROM `creature_template` AS ct, `creature` AS c " +
                    "WHERE c.id = ct.entry && c.phaseMask =1 && c.spawnMask&15 && `WDBVerified` = 15595 && " +
                    "( ! ( c.unit_flags &33554432 ) && ! ( c.unit_flags &262144 ) && ! ( c.unit_flags &524288 ) ) && " +
                    "!(ct.npcflag &1)" +
                    " && " +
                    // NPC without GOSSIP menu for now, we need to handle GOSSIP correctly in the bot first.
                    "((ct.npcflag &64 && trainer_type=2) || ct.npcflag &128 || ct.npcflag &4096 || ct.npcflag &8192 || ct.npcflag &16384 || ct.npcflag &32768 || ct.npcflag &65536 || ct.npcflag &131072 || ct.npcflag &1048576 || ct.npcflag &2097152 || ct.npcflag &4194304 || ct.npcflag &8388608 || ct.npcflag &67108864) && " +
                    "( ! ( ct.unit_flags &33554432 ) && ! ( ct.unit_flags &262144 ) && ! ( ct.unit_flags &524288 ) ) && ! ( ct.flags_extra &128 ) && " +
                    "ct.entry NOT IN (SELECT `npc_entry` FROM (`creature_transport`)) " +
                    "GROUP BY c.guid ORDER BY ct.entry;";
                var npccmd = new MySqlCommand(npcquery, myConn);
                MySqlDataReader npcresult = npccmd.ExecuteReader();
                var newList = new List<Npc>();
                var currentFaction = new Npc.FactionType();
                while (npcresult.Read())
                {
                    if ((UnitRelation.GetReaction(1, npcresult.GetUInt32("faction_A")) ==
                         Reaction.Friendly ||
                         UnitRelation.GetReaction(1, npcresult.GetUInt32("faction_A")) ==
                         Reaction.Neutral) &&
                        (UnitRelation.GetReaction(2, npcresult.GetUInt32("faction_H")) ==
                         Reaction.Friendly ||
                         UnitRelation.GetReaction(2, npcresult.GetUInt32("faction_H")) ==
                         Reaction.Neutral))
                    {
                        // neutral
                        currentFaction = Npc.FactionType.Neutral;
                    }
                    else if (UnitRelation.GetReaction(2, npcresult.GetUInt32("faction_H")) ==
                             Reaction.Friendly ||
                             UnitRelation.GetReaction(2, npcresult.GetUInt32("faction_H")) ==
                             Reaction.Neutral)
                    {
                        // horde
                        currentFaction = Npc.FactionType.Horde;
                    }
                    else if (UnitRelation.GetReaction(1, npcresult.GetUInt32("faction_A")) ==
                             Reaction.Friendly ||
                             UnitRelation.GetReaction(1, npcresult.GetUInt32("faction_A")) ==
                             Reaction.Neutral)
                    {
                        // alliance
                        currentFaction = Npc.FactionType.Alliance;
                    }
                    if (Others.ToBoolean(npcresult.GetUInt32("npcflag") & 64))
                    {
                        var newtype = Npc.NpcType.None;
                        if (npcresult.GetString("subname").Contains("Alchemy"))
                            newtype = Npc.NpcType.AlchemyTrainer;
                        else if (npcresult.GetString("subname").Contains("Blacksmithing"))
                            newtype = Npc.NpcType.BlacksmithingTrainer;
                        else if (npcresult.GetString("subname").Contains("Enchanting"))
                            newtype = Npc.NpcType.EnchantingTrainer;
                        else if (npcresult.GetString("subname").Contains("Engineering"))
                            newtype = Npc.NpcType.EngineeringTrainer;
                        else if (npcresult.GetString("subname").Contains("Herbalism"))
                            newtype = Npc.NpcType.HerbalismTrainer;
                        else if (npcresult.GetString("subname").Contains("Inscription"))
                            newtype = Npc.NpcType.InscriptionTrainer;
                        else if (npcresult.GetString("subname").Contains("Jewelcrafting"))
                            newtype = Npc.NpcType.JewelcraftingTrainer;
                        else if (npcresult.GetString("subname").Contains("Leatherworking"))
                            newtype = Npc.NpcType.LeatherworkingTrainer;
                        else if (npcresult.GetString("subname").Contains("Mining"))
                            newtype = Npc.NpcType.MiningTrainer;
                        else if (npcresult.GetString("subname").Contains("Skinning"))
                            newtype = Npc.NpcType.SkinningTrainer;
                        else if (npcresult.GetString("subname").Contains("Tailoring"))
                            newtype = Npc.NpcType.TailoringTrainer;
                        else if (npcresult.GetString("subname").Contains("Archaeology"))
                            newtype = Npc.NpcType.ArchaeologyTrainer;
                        else if (npcresult.GetString("subname").Contains("Cooking"))
                            newtype = Npc.NpcType.CookingTrainer;
                        else if (npcresult.GetString("subname").Contains("First Aid"))
                            newtype = Npc.NpcType.FirstAidTrainer;
                        else if (npcresult.GetString("subname").Contains("Fishing"))
                            newtype = Npc.NpcType.FishingTrainer;
                        else if (npcresult.GetString("subname").Contains("Riding"))
                            newtype = Npc.NpcType.RidingTrainer;
                        if (newtype != Npc.NpcType.None)
                            newList.Add(new Npc
                                {
                                    ContinentId = (ContinentId) npcresult.GetUInt32("map"),
                                    Entry = npcresult.GetInt32("entry"),
                                    Faction = currentFaction,
                                    Name = npcresult.GetString("name"),
                                    Position =
                                        new Point(npcresult.GetFloat("position_x"), npcresult.GetFloat("position_y"),
                                                  npcresult.GetFloat("position_z")),
                                    SelectGossipOption = 1,
                                    Type = newtype
                                });
                    }
                    if (Others.ToBoolean(npcresult.GetUInt32("npcflag") & 128))
                    {
                        newList.Add(new Npc
                            {
                                ContinentId = (ContinentId) npcresult.GetUInt32("map"),
                                Entry = npcresult.GetInt32("entry"),
                                Faction = currentFaction,
                                Name = npcresult.GetString("name"),
                                Position =
                                    new Point(npcresult.GetFloat("position_x"), npcresult.GetFloat("position_y"),
                                              npcresult.GetFloat("position_z")),
                                SelectGossipOption = 1,
                                Type = Npc.NpcType.Vendor
                            });
                    }
                    if (Others.ToBoolean(npcresult.GetUInt32("npcflag") & 4096))
                    {
                        newList.Add(new Npc
                            {
                                ContinentId = (ContinentId) npcresult.GetUInt32("map"),
                                Entry = npcresult.GetInt32("entry"),
                                Faction = currentFaction,
                                Name = npcresult.GetString("name"),
                                Position =
                                    new Point(npcresult.GetFloat("position_x"), npcresult.GetFloat("position_y"),
                                              npcresult.GetFloat("position_z")),
                                SelectGossipOption = 1,
                                Type = Npc.NpcType.Repair
                            });
                    }
                    if (Others.ToBoolean(npcresult.GetUInt32("npcflag") & 8192))
                    {
                        newList.Add(new Npc
                            {
                                ContinentId = (ContinentId) npcresult.GetUInt32("map"),
                                Entry = npcresult.GetInt32("entry"),
                                Faction = currentFaction,
                                Name = npcresult.GetString("name"),
                                Position =
                                    new Point(npcresult.GetFloat("position_x"), npcresult.GetFloat("position_y"),
                                              npcresult.GetFloat("position_z")),
                                SelectGossipOption = 1,
                                Type = Npc.NpcType.FlightMaster
                            });
                    }
                    if (Others.ToBoolean(npcresult.GetUInt32("npcflag") & 16384))
                    {
                        newList.Add(new Npc
                            {
                                ContinentId = (ContinentId) npcresult.GetUInt32("map"),
                                Entry = npcresult.GetInt32("entry"),
                                Faction = currentFaction,
                                Name = npcresult.GetString("name"),
                                Position =
                                    new Point(npcresult.GetFloat("position_x"), npcresult.GetFloat("position_y"),
                                              npcresult.GetFloat("position_z")),
                                SelectGossipOption = 1,
                                Type = Npc.NpcType.SpiritHealer
                            });
                    }
                    if (Others.ToBoolean(npcresult.GetUInt32("npcflag") & 32768))
                    {
                        newList.Add(new Npc
                            {
                                ContinentId = (ContinentId) npcresult.GetUInt32("map"),
                                Entry = npcresult.GetInt32("entry"),
                                Faction = currentFaction,
                                Name = npcresult.GetString("name"),
                                Position =
                                    new Point(npcresult.GetFloat("position_x"), npcresult.GetFloat("position_y"),
                                              npcresult.GetFloat("position_z")),
                                SelectGossipOption = 1,
                                Type = Npc.NpcType.SpiritGuide
                            });
                    }
                    if (Others.ToBoolean(npcresult.GetUInt32("npcflag") & 65536))
                    {
                        newList.Add(new Npc
                            {
                                ContinentId = (ContinentId) npcresult.GetUInt32("map"),
                                Entry = npcresult.GetInt32("entry"),
                                Faction = currentFaction,
                                Name = npcresult.GetString("name"),
                                Position =
                                    new Point(npcresult.GetFloat("position_x"), npcresult.GetFloat("position_y"),
                                              npcresult.GetFloat("position_z")),
                                SelectGossipOption = 1,
                                Type = Npc.NpcType.Innkeeper
                            });
                    }
                    if (Others.ToBoolean(npcresult.GetUInt32("npcflag") & 131072))
                    {
                        newList.Add(new Npc
                            {
                                ContinentId = (ContinentId) npcresult.GetUInt32("map"),
                                Entry = npcresult.GetInt32("entry"),
                                Faction = currentFaction,
                                Name = npcresult.GetString("name"),
                                Position =
                                    new Point(npcresult.GetFloat("position_x"), npcresult.GetFloat("position_y"),
                                              npcresult.GetFloat("position_z")),
                                SelectGossipOption = 1,
                                Type = Npc.NpcType.Banker
                            });
                    }
                    if (Others.ToBoolean(npcresult.GetUInt32("npcflag") & 1048576))
                    {
                        newList.Add(new Npc
                            {
                                ContinentId = (ContinentId) npcresult.GetUInt32("map"),
                                Entry = npcresult.GetInt32("entry"),
                                Faction = currentFaction,
                                Name = npcresult.GetString("name"),
                                Position =
                                    new Point(npcresult.GetFloat("position_x"), npcresult.GetFloat("position_y"),
                                              npcresult.GetFloat("position_z")),
                                SelectGossipOption = 1,
                                Type = Npc.NpcType.Battlemaster
                            });
                    }
                    if (Others.ToBoolean(npcresult.GetUInt32("npcflag") & 2097152))
                    {
                        newList.Add(new Npc
                            {
                                ContinentId = (ContinentId) npcresult.GetUInt32("map"),
                                Entry = npcresult.GetInt32("entry"),
                                Faction = currentFaction,
                                Name = npcresult.GetString("name"),
                                Position =
                                    new Point(npcresult.GetFloat("position_x"), npcresult.GetFloat("position_y"),
                                              npcresult.GetFloat("position_z")),
                                SelectGossipOption = 1,
                                Type = Npc.NpcType.Auctioneer
                            });
                    }
                    if (Others.ToBoolean(npcresult.GetUInt32("npcflag") & 4194304))
                    {
                        newList.Add(new Npc
                            {
                                ContinentId = (ContinentId) npcresult.GetUInt32("map"),
                                Entry = npcresult.GetInt32("entry"),
                                Faction = currentFaction,
                                Name = npcresult.GetString("name"),
                                Position =
                                    new Point(npcresult.GetFloat("position_x"), npcresult.GetFloat("position_y"),
                                              npcresult.GetFloat("position_z")),
                                SelectGossipOption = 1,
                                Type = Npc.NpcType.StableMaster
                            });
                    }
                    if (Others.ToBoolean(npcresult.GetUInt32("npcflag") & 8388608))
                    {
                        newList.Add(new Npc
                            {
                                ContinentId = (ContinentId) npcresult.GetUInt32("map"),
                                Entry = npcresult.GetInt32("entry"),
                                Faction = currentFaction,
                                Name = npcresult.GetString("name"),
                                Position =
                                    new Point(npcresult.GetFloat("position_x"), npcresult.GetFloat("position_y"),
                                              npcresult.GetFloat("position_z")),
                                SelectGossipOption = 1,
                                Type = Npc.NpcType.GuildBanker
                            });
                    }
                    if (Others.ToBoolean(npcresult.GetUInt32("npcflag") & 67108864))
                    {
                        newList.Add(new Npc
                            {
                                ContinentId = (ContinentId) npcresult.GetUInt32("map"),
                                Entry = npcresult.GetInt32("entry"),
                                Faction = currentFaction,
                                Name = npcresult.GetString("name"),
                                Position =
                                    new Point(npcresult.GetFloat("position_x"), npcresult.GetFloat("position_y"),
                                              npcresult.GetFloat("position_z")),
                                SelectGossipOption = 1,
                                Type = Npc.NpcType.Mailbox
                            });
                    }
                }
                npcresult.Close();
                const string goquery =
                    "SELECT `got`.entry, `got`.name, `got`.type, `got`.data0, `got`.faction, `go`.map, `go`.position_x, `go`.position_y, `go`.position_z " +
                    "FROM `gameobject_template` AS got, `gameobject` AS go " +
                    "WHERE `go`.id = `got`.entry && `go`.phaseMask =1 && `go`.spawnMask&15 && `WDBVerified` = 15595 && " +
                    "((( ! ( `got`.flags &1 ) && ! ( `got`.flags &2 ) && ! ( `got`.flags &4 ) && ! ( `got`.flags &8 ) && ! ( `got`.flags &64 ) && ! ( `got`.flags &72 ) && ! ( `got`.flags &1024 )) && " +
                    "`got`.type = 19) || (`type` = 8 && `data0` = 3) || (`type` = 8 && `data0` = 1552) || (`type` = 34)) " +
                    "GROUP BY `go`.guid ORDER BY `got`.entry;";
                var gocmd = new MySqlCommand(goquery, myConn);
                MySqlDataReader goresult = gocmd.ExecuteReader();
                while (goresult.Read())
                {
                    if (goresult.GetUInt32("faction") == 0)
                        currentFaction = Npc.FactionType.Neutral;
                    else if ((UnitRelation.GetReaction(1, goresult.GetUInt32("faction")) ==
                              Reaction.Friendly ||
                              UnitRelation.GetReaction(1, goresult.GetUInt32("faction")) ==
                              Reaction.Neutral) &&
                             (UnitRelation.GetReaction(2, goresult.GetUInt32("faction")) ==
                              Reaction.Friendly ||
                              UnitRelation.GetReaction(2, goresult.GetUInt32("faction")) ==
                              Reaction.Neutral))
                    {
                        currentFaction = Npc.FactionType.Neutral;
                    }
                    else if (UnitRelation.GetReaction(2, goresult.GetUInt32("faction")) ==
                             Reaction.Friendly ||
                             UnitRelation.GetReaction(2, goresult.GetUInt32("faction")) ==
                             Reaction.Neutral)
                    {
                        currentFaction = Npc.FactionType.Horde;
                    }
                    else if (UnitRelation.GetReaction(1, goresult.GetUInt32("faction")) ==
                             Reaction.Friendly ||
                             UnitRelation.GetReaction(1, goresult.GetUInt32("faction")) ==
                             Reaction.Neutral)
                    {
                        currentFaction = Npc.FactionType.Alliance;
                    }
                    if (goresult.GetUInt32("type") == 19)
                    {
                        newList.Add(new Npc
                            {
                                ContinentId = (ContinentId) goresult.GetUInt32("map"),
                                Entry = goresult.GetInt32("entry"),
                                Faction = currentFaction,
                                Name = goresult.GetString("name"),
                                Position =
                                    new Point(goresult.GetFloat("position_x"), goresult.GetFloat("position_y"),
                                              goresult.GetFloat("position_z")),
                                SelectGossipOption = 1,
                                Type = Npc.NpcType.Mailbox
                            });
                    }
                    else if (goresult.GetUInt32("type") == 8 && goresult.GetUInt32("data0") == 3)
                    {
                        newList.Add(new Npc
                            {
                                ContinentId = (ContinentId) goresult.GetUInt32("map"),
                                Entry = goresult.GetInt32("entry"),
                                Faction = currentFaction,
                                Name = goresult.GetString("name"),
                                Position =
                                    new Point(goresult.GetFloat("position_x"), goresult.GetFloat("position_y"),
                                              goresult.GetFloat("position_z")),
                                SelectGossipOption = 1,
                                Type = Npc.NpcType.SmeltingForge
                            });
                    }
                    else if (goresult.GetUInt32("type") == 8 && goresult.GetUInt32("data0") == 1552)
                    {
                        newList.Add(new Npc
                            {
                                ContinentId = (ContinentId) goresult.GetUInt32("map"),
                                Entry = goresult.GetInt32("entry"),
                                Faction = currentFaction,
                                Name = goresult.GetString("name"),
                                Position =
                                    new Point(goresult.GetFloat("position_x"), goresult.GetFloat("position_y"),
                                              goresult.GetFloat("position_z")),
                                SelectGossipOption = 1,
                                Type = Npc.NpcType.RuneForge
                            });
                    }
                    else if (goresult.GetUInt32("type") == 34)
                    {
                        newList.Add(new Npc
                            {
                                ContinentId = (ContinentId) goresult.GetUInt32("map"),
                                Entry = goresult.GetInt32("entry"),
                                Faction = currentFaction,
                                Name = goresult.GetString("name"),
                                Position =
                                    new Point(goresult.GetFloat("position_x"), goresult.GetFloat("position_y"),
                                              goresult.GetFloat("position_z")),
                                SelectGossipOption = 1,
                                Type = Npc.NpcType.GuildBanker
                            });
                    }
                }
                goresult.Close();
                NpcDB.BuildNewList(newList);
                newList.Clear();
                return goresult.HasRows;
                /*
                DBC<DBCStruct.SpellRec> DBCSpell = new DBC<DBCStruct.SpellRec>((int) Addresses.DBC.Spell);
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
                l[(int)point.X] = Others.ToChar(n.ToString());
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