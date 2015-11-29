using System;
using System.Collections.Generic;
using System.Windows.Forms;
using nManager.Wow.Helpers;

namespace nManager.Helpful
{
    public class Garrison
    {
        public enum BuildingID
        {
            DwarvenBunkerWarMill1 = 8,
            DwarvenBunkerWarMill2 = 9,
            DwarvenBunkerWarMill3 = 10,
            Barn1 = 24,
            Barn2 = 25,
            Barracks1 = 26,
            Barracks2 = 27,
            Barracks3 = 28,
            HerbGarden1 = 29,
            LunarfallInnFrostwallTavern1 = 34,
            LunarfallInnFrostwallTavern2 = 35,
            LunarfallInnFrostwallTavern3 = 36,
            MageTowerSpiritLodge1 = 37,
            MageTowerSpiritLodge2 = 38,
            MageTowerSpiritLodge3 = 39,
            LumberMill1 = 40,
            LumberMill2 = 41,
            Menagerie1 = 42,
            Storehouse1 = 51,
            SalvageYard1 = 52,
            TheForge1 = 60,
            LunarfallExcavationFrostwallMines1 = 61,
            LunarfallExcavationFrostwallMines2 = 62,
            LunarfallExcavationFrostwallMines3 = 63,
            FishingShack1 = 64,
            Stables1 = 65,
            Stables2 = 66,
            Stables3 = 67,
            AlchemyLab1 = 76,
            TheTannery1 = 90,
            EngineeringWorks1 = 91,
            EnchantersStudy1 = 93,
            TailoringEmporium1 = 94,
            ScribesQuarters1 = 95,
            GemBoutique1 = 96,
            TradingPost1 = 111,
            TheForge2 = 117,
            TheForge3 = 118,
            AlchemyLab2 = 119,
            AlchemyLab3 = 120,
            TheTannery2 = 121,
            TheTannery3 = 122,
            EngineeringWorks2 = 123,
            EngineeringWorks3 = 124,
            EnchantersStudy2 = 125,
            EnchantersStudy3 = 126,
            TailoringEmporium2 = 127,
            TailoringEmporium3 = 128,
            ScribesQuarters2 = 129,
            ScribesQuarters3 = 130,
            GemBoutique2 = 131,
            GemBoutique3 = 132,
            Barn3 = 133,
            FishingShack2 = 134,
            FishingShack3 = 135,
            HerbGarden2 = 136,
            HerbGarden3 = 137,
            LumberMill3 = 138,
            SalvageYard2 = 140,
            SalvageYard3 = 141,
            Storehouse2 = 142,
            Storehouse3 = 143,
            TradingPost2 = 144,
            TradingPost3 = 145,
            GladiatorsSanctum1 = 159,
            GladiatorsSanctum2 = 160,
            GladiatorsSanctum3 = 161,
            GnomishGearworksGoblinWorkshop1 = 162,
            GnomishGearworksGoblinWorkshop2 = 163,
            GnomishGearworksGoblinWorkshop3 = 164,
            Menagerie2 = 167,
            Menagerie3 = 168,
        }

        private static List<int> _garrisonMapIdList;

        public static List<int> GarrisonMapIdList
        {
            get
            {
                try
                {
                    if (_garrisonMapIdList == null)
                    {
                        _garrisonMapIdList = new List<int>();
                        foreach (string i in Others.ReadFileAllLines(Application.StartupPath + "\\Data\\garrisonMapIdList.txt"))
                        {
                            if (!String.IsNullOrWhiteSpace(i))
                                _garrisonMapIdList.Add(Others.ToInt32(i));
                        }
                    }
                    return _garrisonMapIdList;
                }
                catch (Exception e)
                {
                    Logging.WriteError("Usefuls.GarrisonMapIdList : " + e);
                    return new List<int>();
                }
            }
        }

        public static int GetGarrisonMineLevel
        {
            get
            {
                List<BuildingID> listOwnedBuildings = GetBuildingOwnedList();
                if (listOwnedBuildings.Contains(BuildingID.LunarfallExcavationFrostwallMines1))
                    return 1;
                if (listOwnedBuildings.Contains(BuildingID.LunarfallExcavationFrostwallMines2))
                    return 2;
                if (listOwnedBuildings.Contains(BuildingID.LunarfallExcavationFrostwallMines3))
                    return 3;
                return 0;
            }
        }

        public static int GetGarrisonGardenLevel
        {
            get
            {
                List<BuildingID> listOwnedBuildings = GetBuildingOwnedList();
                if (listOwnedBuildings.Contains(BuildingID.HerbGarden1))
                    return 1;
                if (listOwnedBuildings.Contains(BuildingID.HerbGarden2))
                    return 2;
                if (listOwnedBuildings.Contains(BuildingID.HerbGarden3))
                    return 3;
                return 0;
            }
        }

        public static int GetGarrisonLevel()
        {
            string randomString = Others.GetRandomString(Others.Random(4, 10));
            Lua.LuaDoString(randomString + " = C_Garrison.GetGarrisonInfo()");
            string ret = Lua.GetLocalizedText(randomString);
            return Others.ToInt32(ret);
        }

        public static List<BuildingID> GetBuildingOwnedList()
        {
            try
            {
                var listOwnedBuildings = new List<BuildingID>();
                Lua.LuaDoString(
                    "mygb = \"\"; " +
                    "local buildings = C_Garrison.GetBuildings() " +
                    " for i = 1, #buildings do " +
                    "  mygb =  mygb.. tostring(buildings[i].buildingID) .. \"|\" end ");
                foreach (string value in Lua.GetLocalizedText("mygb").Split('|'))
                {
                    int tValue = Others.ToInt32(value);
                    if (tValue > 0)
                        listOwnedBuildings.Add((BuildingID) tValue);
                }
                return listOwnedBuildings;
            }
            catch (Exception e)
            {
                Logging.WriteError("public static List<BuildingID> GetBuildingOwnedList(): " + e);
            }
            return new List<BuildingID>();
        }

        public static void DumpAllOwnedBuildingsRanks()
        {
            foreach (BuildingID source in GetBuildingOwnedList())
            {
                int rank = GetBuildingRank(source);
                Logging.Write("GBuilding = " + source + ", rank = " + rank);
            }
        }

        public static int GetBuildingRank(BuildingID buildingId)
        {
            // buildingID, buildingName, texturePrefix, icon, description, rank = C_Garrison.GetBuildingInfo(buildingID)
            //
            try
            {
                string result = Others.GetRandomString(Others.Random(4, 10));
                Lua.LuaDoString("_, _, _, _, _, " + result + " = C_Garrison.GetBuildingInfo(" + (int) buildingId + ");");
                return Others.ToInt32(Lua.GetLocalizedText(result));
            }
            catch (Exception e)
            {
                Logging.WriteError("public int GetBuildingRank(int buildingId): " + e);
            }
            return 0;
        }
    }
}