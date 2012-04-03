using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using nManager.Helpful;
using nManager.Wow.Class;

namespace nManager.Wow.Helpers
{
    public class Archaeology
    {
        static List<Digsite> _allDigsiteZone = new List<Digsite>();

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

        public static List<int> SurveyList = new List<int> { 10103, 10102, 10101};
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
                        foreach (var i in Others.ReadFileAllLines(Application.StartupPath + "\\Data\\archaeologyFind.txt"))
                        {
                            try
                            {
                                _archaeologyItemsFindList.Add(Convert.ToInt32(i));
                            }
                            catch
                            {
                            }
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
                if (_allDigsiteZone.Count <= 0)
                {
                    var listDigsitesZone = new List<Digsite>();

                    try
                    {
                        Logging.Write("Loading ArchaeologistDigsites.xml");

                        listDigsitesZone = XmlSerializer.Deserialize<List<Digsite>>(Application.StartupPath + "\\Data\\ArchaeologistDigsites.xml");

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

        public static List<Digsite> GetDigsitesZoneAvailable()
        {
            try
            {
                GetAllDigsitesZone();
                var resultList = new List<Digsite>();
                var continentId = (uint)Usefuls.ContinentId;

                var digsitesZoneLua = GetDigsitesZoneLua();

                if (digsitesZoneLua.Count > 0)
                {
                    foreach (DigsitesZoneLua dl in digsitesZoneLua)
                    {
                        bool zonefound = false;
                        for (var i = 0; i <= _allDigsiteZone.Count - 1; i++)
                        {
                            if (_allDigsiteZone[i].px == dl.px && _allDigsiteZone[i].py == dl.py && _allDigsiteZone[i].continentId == continentId && _allDigsiteZone[i].Active)
                            {
                                var dt = _allDigsiteZone[i];

                                try
                                {
                                    if (_allDigsiteZone[i].name != dl.name)
                                    {
                                        _allDigsiteZone[i].name = dl.name;
                                        XmlSerializer.Serialize(
                                            Application.StartupPath + "\\Data\\ArchaeologistDigsites.xml",
                                            _allDigsiteZone);
                                    }
                                }
                                catch (Exception)
                                {
                                }

                                dt.name = dl.name;
                                resultList.Add(dt);
                                zonefound = true;
                                Logging.Write("Digsite zone found: Name: " + dl.name + " - Distance =" + dt.position.DistanceTo(ObjectManager.ObjectManager.Me.Position));
                                break;
                            }
                        }
                        if (!zonefound)
                            Logging.Write("Digsite zone not found in database: Name=" + dl.name + " px=" + dl.px + " py=" + dl.py);
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
                var continentId = (uint)Usefuls.ContinentId;

                if (digsitesZoneLua.Count > 0)
                {
                    foreach (DigsitesZoneLua dl in digsitesZoneLua)
                    {
                        if (digsitesZone.px == dl.px && digsitesZone.py == dl.py && digsitesZone.continentId == continentId)
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
        static List<DigsitesZoneLua> GetDigsitesZoneLua()
        {
            try
            {
                string randomString = Others.GetRandomString(Others.Random(4, 10));
                var resultList = new List<DigsitesZoneLua>();

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
                luaCommand = luaCommand + "		" + randomString + " = " + randomString + " .. name .. '" + separator + "' .. px .. '" + separator + "' .. py .. '" + separatorDigsites + "' ";
                luaCommand = luaCommand + "	end ";
                luaCommand = luaCommand + "end ";

                string resultLua;
                lock (typeof(Archaeology))
                {
                    Lua.LuaDoString(luaCommand);
                    resultLua = Lua.GetLocalizedText(randomString);
                }

                if (resultLua.Replace(" ", "").Length > 0)
                {
                    try
                    {
                        string[] sDigsitesArray = resultLua.Split(Convert.ToChar(separatorDigsites));

                        foreach (string s in sDigsitesArray)
                        {
                            if (s.Replace(" ", "").Length > 0)
                            {
                                try
                                {
                                    string[] sDigsites = s.Split(Convert.ToChar(separator));
                                    var tDigsitesZoneLua = new DigsitesZoneLua
                                    {
                                        name = sDigsites[0],
                                        px = sDigsites[1],
                                        py = sDigsites[2]
                                    };
                                    resultList.Add(tDigsitesZoneLua);
                                }
                                catch { }
                            }
                        }
                    }
                    catch { }
                }

                return resultList;
            }
            catch (Exception e)
            {
                Logging.WriteError("Archaeology > GetDigsitesZoneLua()#3: " + e);
                return new List<DigsitesZoneLua>();
            }
        }

        static Spell archaeologySpell;
        public static void SolveAllArtifact()
        {
            try
            {
                if (archaeologySpell == null)
                    archaeologySpell = new Spell("Archaeology");

                for (int i = 1; i <= 10; i++)
                {
                    Lua.RunMacroText("/cast " + archaeologySpell.NameInGame);
                    Lua.RunMacroText("/click ArchaeologyFrameSummaryButton");
                    Lua.RunMacroText("/click ArchaeologyFrameSummaryPageRace" + i);
                    Lua.RunMacroText("/click ArchaeologyFrameArtifactPageSolveFrameSolveButton");

                    Thread.Sleep(500);
                    if (ObjectManager.ObjectManager.Me.IsCast)
                    {
                        if (Usefuls.GetContainerNumFreeSlots >= 1)
                        {
                            i--;
                        }
                        while (ObjectManager.ObjectManager.Me.IsCast)
                        {
                            Thread.Sleep(100);
                        }
                    }

                    Lua.RunMacroText("/click ArchaeologyFrameCloseButton");
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("Archaeology >  solveAllArtifact(): " + e);
            }
        }

        struct DigsitesZoneLua
        {
            public string name;
            public string px;
            public string py;
        }
    }
}
