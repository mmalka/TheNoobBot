using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using nManager.Helpful;
using nManager.Wow.Bot.Tasks;
using nManager.Wow.Class;
using nManager.Wow.ObjectManager;

namespace nManager.Wow.Helpers
{
    public class Archaeology
    {
        private static List<Digsite> _allDigsiteZone = new List<Digsite>();

        public static void Initialize()
        {
            _allDigsiteZone = new List<Digsite>();
        }

        public static void ClearList()
        {
            try
            {
                _allDigsiteZone = new List<Digsite>();
            }
            catch (Exception e)
            {
                Logging.WriteError("Archaeology > ClearList(): " + e);
            }
        }

        public static List<uint> SurveyList = new List<uint> {10103, 10102, 10101};
        private static List<int> _archaeologyItemsFindList;

        public static List<int> ArchaeologyItemsFindList
        {
            get
            {
                try
                {
                    if (_archaeologyItemsFindList == null)
                    {
                        _archaeologyItemsFindList = new List<int>();
                        foreach (string i in Others.ReadFileAllLines(Application.StartupPath + "\\Data\\archaeologyFind.txt"))
                        {
                            if (!string.IsNullOrWhiteSpace(i))
                                _archaeologyItemsFindList.Add(Others.ToInt32(i));
                        }
                    }
                    return _archaeologyItemsFindList;
                }
                catch (Exception e)
                {
                    Logging.WriteError("Archaeology > ArchaeologyItemsFindList : " + e);
                    return new List<int>();
                }
            }
        }

        public static bool ForceReloadDigsites = false;

        public static List<Digsite> GetAllDigsitesZone()
        {
            try
            {
                if (_allDigsiteZone.Count <= 0 || ForceReloadDigsites)
                {
                    ForceReloadDigsites = false;
                    List<Digsite> listDigsitesZone = new List<Digsite>();

                    try
                    {
                        Logging.Write("Loading ArchaeologistDigsites.xml");

                        listDigsitesZone =
                            XmlSerializer.Deserialize<List<Digsite>>(Application.StartupPath +
                                                                     "\\Data\\ArchaeologistDigsites.xml");
                        listDigsitesZone = GenerateOrUpdate(listDigsitesZone);
                        XmlSerializer.Serialize(Application.StartupPath + "\\Data\\ArchaeologistDigsites.xml",
                            listDigsitesZone);
                        listDigsitesZone = listDigsitesZone.OrderByDescending(c => c.PriorityDigsites).ToList();

                        Logging.Write(listDigsitesZone.Count + " Archaeology Digsites Zones in the data base.");
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("ArchaeologistDigsites.xml: " + e);
                    }

                    _allDigsiteZone = listDigsitesZone;
                }

                return _allDigsiteZone;
            }
            catch (Exception e)
            {
                Logging.WriteError("Archaeology > GetAllDigsitesZone(): " + e);
            }
            return new List<Digsite>();
        }

        public static List<Digsite> GenerateOrUpdate(List<Digsite> listDigsitesZoneFromXML)
        {
            bool doUpdate = listDigsitesZoneFromXML != null;
            if (doUpdate)
                listDigsitesZoneFromXML.OrderByDescending(c => c.id).ToList();
            List<Digsite> fullList = new List<Digsite>();

            // Extracting the complete list from the DBC
            WoWResearchSite curRec = WoWResearchSite.FromId(0); // This record is invalid
            int MinIndex = curRec.MinIndex;
            int MaxIndex = curRec.MaxIndex;
            for (int i = MinIndex; i <= MaxIndex; i++)
            {
                curRec = WoWResearchSite.FromId(i);
                if (curRec.Record.Id != 0)
                {
                    Digsite curDigSite = new Digsite {id = (int) curRec.Record.Id, name = curRec.Name, PriorityDigsites = 1, Active = true};
                    fullList.Add(curDigSite);
                }
            }
            if (doUpdate)
            {
                List<Digsite> finalList = fullList.Concat(listDigsitesZoneFromXML)
                    .ToLookup(p => p.id)
                    .Select(g => g.Aggregate((p1, p2) => new Digsite
                    {
                        id = p1.id,
                        name = p1.name,
                        PriorityDigsites = p2.PriorityDigsites,
                        Active = p2.Active
                    })).ToList();
                return finalList;
            }
            return /*finalList = */ fullList;
        }

        public static List<Digsite> GetDigsitesZoneAvailable(string NameForSecond = null)
        {
            try
            {
                GetAllDigsitesZone();
                List<Digsite> resultList = new List<Digsite>();
                List<DigsitesZoneLua> digsitesZoneLua = GetDigsitesZoneLua();

                if (digsitesZoneLua.Count > 0)
                {
                    foreach (DigsitesZoneLua dl in digsitesZoneLua)
                    {
                        bool zonefound = false;
                        WoWResearchSite RowRSite;
                        if (NameForSecond == dl.name)
                            RowRSite = WoWResearchSite.FromName(dl.name, true);
                        else
                            RowRSite = WoWResearchSite.FromName(dl.name);
                        WoWQuestPOIPoint qPOI = WoWQuestPOIPoint.FromSetId(RowRSite.Record.QuestIdPoint);
                        for (int i = 0; i <= _allDigsiteZone.Count - 1; i++)
                        {
                            if (_allDigsiteZone[i].id == RowRSite.Record.Id && _allDigsiteZone[i].Active)
                            {
                                resultList.Add(_allDigsiteZone[i]);
                                Logging.Write("Digsite zone found: Name: " + dl.name + " - Distance = " +
                                              qPOI.Center.DistanceTo2D(ObjectManager.ObjectManager.Me.Position));
                                zonefound = true;
                            }
                        }
                        if (!zonefound)
                            Logging.Write("Digsite zone not found in database: Name=" + dl.name);
                    }
                }
                else
                {
                    Logging.Write("No digsites zones found, make sure 'Show Digsites' is activated in your map window.");
                    Thread.Sleep(500);
                }

                return resultList;
            }
            catch (Exception e)
            {
                Logging.WriteError("Archaeology > GetDigsitesZoneAvailable(): " + e);
                return new List<Digsite>();
            }
        }

        public static bool DigsiteZoneIsAvailable(Digsite digsitesZone)
        {
            try
            {
                List<DigsitesZoneLua> digsitesZoneLua = GetDigsitesZoneLua();
                if (digsitesZoneLua.Count > 0)
                {
                    foreach (DigsitesZoneLua dl in digsitesZoneLua)
                    {
                        if (digsitesZone.name == dl.name)
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("Archaeology > DigsiteZoneIsAvailable(DigsitesZone digsitesZone): " + e);
            }
            return false;
        }

        private static List<DigsitesZoneLua> GetDigsitesZoneLua()
        {
            try
            {
                string randomString = Others.GetRandomString(Others.Random(4, 10));
                List<DigsitesZoneLua> resultList = new List<DigsitesZoneLua>();

                const string separatorDigsites = "#";
                const string separator = "^";

                string luaCommand = "";

                luaCommand = luaCommand + randomString + " = '' ";
                luaCommand = luaCommand + "SetMapToCurrentZone() ";
                luaCommand = luaCommand + "continent = GetCurrentMapContinent() ";
                luaCommand = luaCommand + "SetMapZoom(continent) ";
                luaCommand = luaCommand + "local totalPOIs = GetNumMapLandmarks() ";
                luaCommand = luaCommand + "for index = 1 , totalPOIs do ";
                luaCommand = luaCommand + "	local name, description, textureIndex, px, py = GetMapLandmarkInfo(index) ";
                luaCommand = luaCommand + "	if textureIndex == 177 then ";
                luaCommand = luaCommand + "		" + randomString + " = " + randomString + " .. name .. '" + separatorDigsites + "' ";
                luaCommand = luaCommand + "	end ";
                luaCommand = luaCommand + "end ";

                string resultLua;
                lock (typeof (Archaeology))
                {
                    Lua.LuaDoString(luaCommand);
                    resultLua = Lua.GetLocalizedText(randomString);
                }

                if (resultLua.Replace(" ", "").Length > 0)
                {
                    try
                    {
                        string[] sDigsitesArray = resultLua.Split(Others.ToChar(separatorDigsites));

                        foreach (string s in sDigsitesArray)
                        {
                            if (s.Replace(" ", "").Length > 0)
                            {
                                try
                                {
                                    string[] sDigsites = s.Split(Others.ToChar(separator));
                                    DigsitesZoneLua tDigsitesZoneLua = new DigsitesZoneLua
                                    {
                                        name = sDigsites[0],
                                    };
                                    resultList.Add(tDigsitesZoneLua);
                                }
                                catch
                                {
                                }
                            }
                        }
                    }
                    catch
                    {
                    }
                }

                return resultList;
            }
            catch (Exception e)
            {
                Logging.WriteError("Archaeology > GetDigsitesZoneLua()#3: " + e);
                return new List<DigsitesZoneLua>();
            }
        }

        private static Spell archaeologySpell;

        public static int SolveAllArtifact(bool useKeystone)
        {
            try
            {
                int nbSolved = 0;
                if (archaeologySpell == null)
                    archaeologySpell = new Spell("Archaeology");

                Lua.RunMacroText("/cast " + archaeologySpell.NameInGame);
                int j = 1;
                while (j <= 15)
                {
                    Lua.RunMacroText("/click ArchaeologyFrameSummaryButton");
                    if (j == 13)
                    {
                        Lua.RunMacroText("/click ArchaeologyFrameSummarytButton");
                        Lua.RunMacroText("/click ArchaeologyFrameSummaryPageNextPageButton");
                    }
                    if (j > 12)
                        Lua.RunMacroText("/click ArchaeologyFrameSummaryPageRace" + (j - 12));
                    else
                        Lua.RunMacroText("/click ArchaeologyFrameSummaryPageRace" + j);
                    Thread.Sleep(200 + Usefuls.Latency);
                    if (useKeystone)
                    {
                        bool moreToSocket;
                        do
                        {
                            moreToSocket = false;
                            string randomString1 = Others.GetRandomString(Others.Random(4, 10));
                            string randomString2 = Others.GetRandomString(Others.Random(4, 10));
                            Lua.LuaDoString(randomString1 + " = SocketItemToArtifact(); if " + randomString1 + " then " + randomString2 + "=\"1\" else " + randomString2 + "=\"0\" end");
                            if (Lua.GetLocalizedText(randomString2) == "1")
                            {
                                moreToSocket = true;
                                Thread.Sleep(250 + Usefuls.Latency);
                            }
                        } while (moreToSocket);
                    }
                    string randomString3 = Others.GetRandomString(Others.Random(4, 10));
                    string randomString4 = Others.GetRandomString(Others.Random(4, 10));
                    Lua.LuaDoString(randomString3 + " = CanSolveArtifact(); if " + randomString3 + " then " + randomString4 + "=\"1\" else " + randomString4 + "=\"0\" end");
                    bool canSolve = Lua.GetLocalizedText(randomString4) == "1";
                    if (canSolve)
                    {
                        nbSolved++;
                        Lua.RunMacroText("/click ArchaeologyFrameArtifactPageSolveFrameSolveButton");
                        if (Usefuls.GetContainerNumFreeSlots >= 1)
                            j--;
                        Thread.Sleep(2050 + Usefuls.Latency); // Spell cast time is 2.0secs
                    }
                    j++;
                    Thread.Sleep(200 + Usefuls.Latency);
                }
                Lua.RunMacroText("/click ArchaeologyFrameCloseButton");
                if (ObjectManager.ObjectManager.Me.Level < 90 || Usefuls.GetContainerNumFreeSlots <= 0) return nbSolved;
                for (int i = 79896; i <= 79917; i++)
                {
                    if (i == 79906 || i == 79907)
                        continue;
                    WoWItem item = ObjectManager.ObjectManager.GetWoWItemById(i);
                    if (item == null || !item.IsValid || ItemsManager.IsItemOnCooldown(i) ||
                        !ItemsManager.IsItemUsable(i)) continue;
                    while (ItemsManager.GetItemCount(i) > 0)
                    {
                        MountTask.DismountMount();
                        ItemsManager.UseItem(i);
                        Thread.Sleep(1550 + Usefuls.Latency); // Spell cast time is 1.5secs
                    }
                }
                for (int i = 95375; i <= 95382; i++)
                {
                    WoWItem item = ObjectManager.ObjectManager.GetWoWItemById(i);
                    if (item == null || !item.IsValid || ItemsManager.IsItemOnCooldown(i) ||
                        !ItemsManager.IsItemUsable(i)) continue;
                    while (ItemsManager.GetItemCount(i) > 0)
                    {
                        MountTask.DismountMount();
                        ItemsManager.UseItem(i);
                        Thread.Sleep(1550 + Usefuls.Latency); // Spell cast time is 1.5secs
                    }
                }
                return nbSolved;
            }
            catch (Exception e)
            {
                Logging.WriteError("Archaeology >  solveAllArtifact(): " + e);
                return 0;
            }
        }

        private struct DigsitesZoneLua
        {
            public string name;
        }
    }
}