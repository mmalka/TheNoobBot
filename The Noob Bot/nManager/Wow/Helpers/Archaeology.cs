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
        private static List<Digsite> _allDigsiteZone;

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

        public static List<int> SurveyList = new List<int> {10103, 10102, 10101};
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

        public static List<Digsite> GetAllDigsitesZone()
        {
            try
            {
                // I don't know why it's null when called from DigSites List Management
                // since it's a static method so the class is initialized fully before the 1st call can be made
                if (_allDigsiteZone == null || _allDigsiteZone.Count <= 0)
                {
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

        public static List<Digsite> GetDigsitesZoneAvailable()
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
                        WoWResearchSite RowRSite = WoWResearchSite.FromName(dl.name);
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
                    Logging.Write("Digsite zone not found.");

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

        public static void SolveAllArtifact()
        {
            try
            {
                if (archaeologySpell == null)
                    archaeologySpell = new Spell("Archaeology");

                Lua.RunMacroText("/cast " + archaeologySpell.NameInGame);
                int j = 1;
                while (j <= 12)
                {
                    Lua.RunMacroText("/click ArchaeologyFrameSummaryButton");
                    Lua.RunMacroText("/click ArchaeologyFrameSummaryPageRace" + j);
                    Thread.Sleep(100);
                    Lua.RunMacroText("/click ArchaeologyFrameArtifactPageSolveFrameSolveButton");
                    Thread.Sleep(400);
                    if (ObjectManager.ObjectManager.Me.IsCast)
                    {
                        if (Usefuls.GetContainerNumFreeSlots >= 1)
                        {
                            j--;
                        }
                        while (ObjectManager.ObjectManager.Me.IsCast)
                        {
                            Thread.Sleep(100);
                        }
                    }
                    j++;
                }
                Lua.RunMacroText("/click ArchaeologyFrameCloseButton");
                if (ObjectManager.ObjectManager.Me.Level < 90 || Usefuls.GetContainerNumFreeSlots <= 0) return;
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
                        Thread.Sleep(500);
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
                        Thread.Sleep(500);
                    }
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("Archaeology >  solveAllArtifact(): " + e);
            }
        }

        private struct DigsitesZoneLua
        {
            public string name;
        }
    }
}