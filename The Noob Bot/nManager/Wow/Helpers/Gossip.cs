using System;
using System.Threading;
using nManager.Helpful;

namespace nManager.Wow.Helpers
{
    public class Gossip
    {
        public sealed class GossipOption
        {
            public static readonly GossipOption Banker = new GossipOption("Banker");
            public static readonly GossipOption BattleMaster = new GossipOption("BattleMaster");
            public static readonly GossipOption Binder = new GossipOption("Binder");
            public static readonly GossipOption Gossip = new GossipOption("Gossip");
            public static readonly GossipOption Healer = new GossipOption("Healer");
            public static readonly GossipOption Petition = new GossipOption("Petition");
            public static readonly GossipOption Tabard = new GossipOption("Tabard");
            public static readonly GossipOption Taxi = new GossipOption("Taxi");
            public static readonly GossipOption Trainer = new GossipOption("Trainer");
            public static readonly GossipOption Unlearn = new GossipOption("Unlearn");
            public static readonly GossipOption Vendor = new GossipOption("Vendor");

            private GossipOption(string value)
            {
                Value = value;
            }

            public string Value { get; private set; }

            public static implicit operator string(GossipOption go)
            {
                return go.Value;
            }
        }

        public static void TrainAllAvailableSpells()
        {
            try
            {
                if (SelectGossip(GossipOption.Trainer))
                {
                    Lua.LuaDoString("SetTrainerServiceTypeFilter(\"available\",1);");
                    Lua.LuaDoString("SetTrainerServiceTypeFilter(\"unavailable\",0);");
                    Lua.LuaDoString("SetTrainerServiceTypeFilter(\"used\",0);");
                    Thread.Sleep(1000);
                    Lua.LuaDoString("for i=0,GetNumTrainerServices(),1 do BuyTrainerService(1); end");
                    Thread.Sleep(500);
                    Lua.LuaDoString("CloseTrainer()");
                    Thread.Sleep(500);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("TrainingSpell(): " + exception);
            }
        }

        public static bool SelectGossip(GossipOption option)
        {
            string randomString = Others.GetRandomString(Others.Random(4, 10));
            Lua.LuaDoString(randomString + " = GetNumGossipOptions()");
            int nbGossip = Others.ToInt32(Lua.GetLocalizedText(randomString));
            if (nbGossip == 0) // There is no gossip, so let's assume we have the correct window open
                return true;

            string luaResultStr = Others.GetRandomString(Others.Random(4, 10));
            string luaTable = Others.GetRandomString(Others.Random(4, 10));
            string luaVarId = Others.GetRandomString(Others.Random(4, 10));
            string luaVarValue = Others.GetRandomString(Others.Random(4, 10));
            string luaCode = "local " + luaTable + " = { GetGossipOptions() } ";
            luaCode += luaResultStr + " = 0 ";
            luaCode += "for " + luaVarId + "," + luaVarValue + " in pairs(" + luaTable + ") do ";
            luaCode += "if string.lower(" + luaVarValue + ") == \"" + option.Value.ToLower() + "\" then " + luaResultStr + " = " + luaVarId + "/2 ";
            luaCode += "end end";

            Lua.LuaDoString(luaCode);
            string strOptionNumber = Lua.GetLocalizedText(luaResultStr);
            int optionNumber = Others.ToInt32(strOptionNumber);
            if (optionNumber == 0)
            {
                Logging.WriteError("No gossip option " + option + " available for this NPC");
                return false;
            }
            Lua.LuaDoString("SelectGossipOption(" + optionNumber + ")");
            Thread.Sleep(500 + Usefuls.Latency);
            return true;
        }

        public static void CloseGossip()
        {
            Lua.LuaDoString("CloseGossip()");
        }

        public static bool IsTaxiWindowOpen()
        {
            string randomString = Others.GetRandomString(Others.Random(4, 10));
            Lua.LuaDoString(randomString + " = NumTaxiNodes()");
            string result = Lua.GetLocalizedText(randomString);
            return Others.ToInt32(result) > 0;
        }

        public static void TakeTaxi(string px, string py)
        {
            string randomString = Others.GetRandomString(Others.Random(4, 10));
            Lua.LuaDoString(randomString + " = NumTaxiNodes()");
            int nbTaxiNode = Others.ToInt32(Lua.GetLocalizedText(randomString));
            for (int id = 1; id <= nbTaxiNode; id++)
            {
                string chkpx = Others.GetRandomString(Others.Random(4, 10));
                string chkpy = Others.GetRandomString(Others.Random(4, 10));
                Lua.LuaDoString(chkpx + "," + chkpy + " = TaxiNodePosition(" + id + ")");
                string retpx = Lua.GetLocalizedText(chkpx);
                string retpy = Lua.GetLocalizedText(chkpy);
                if (retpx == px && retpy == py)
                {
                    Lua.LuaDoString("TakeTaxiNode(" + id + ")");
                    break;
                }
            }
        }

        public static void ExportTaxiInfo()
        {
            if (nManager.Wow.ObjectManager.ObjectManager.Me.Target <= 0 || !nManager.Wow.ObjectManager.ObjectManager.Target.IsNpcFlightMaster)
                return;
            nManager.Helpful.Logging.WriteDebug("ExportTaxiInfo of NPC " + nManager.Wow.ObjectManager.ObjectManager.Target.Name + " (TaxiEntry: " + nManager.Wow.ObjectManager.ObjectManager.Target.Entry + ")");
            string randomString = nManager.Helpful.Others.GetRandomString(nManager.Helpful.Others.Random(4, 10));
            nManager.Wow.Helpers.Lua.LuaDoString(randomString + " = NumTaxiNodes()");
            int nbTaxiNode = nManager.Helpful.Others.ToInt32(nManager.Wow.Helpers.Lua.GetLocalizedText(randomString));
            if (nbTaxiNode <= 0)
            {
                nManager.Helpful.Logging.WriteDebug("No TaxiNodes found, make sure to have the window opened and to know at least one path.");
                return;
            }
            nManager.Helpful.Logging.WriteDebug("Found " + nbTaxiNode + " Taxi Path for this Flight Master.");
            for (int id = 1; id <= nbTaxiNode; id++)
            {
                string chkpx = nManager.Helpful.Others.GetRandomString(nManager.Helpful.Others.Random(4, 10));
                string chkpy = nManager.Helpful.Others.GetRandomString(nManager.Helpful.Others.Random(4, 10));
                string destName = nManager.Helpful.Others.GetRandomString(nManager.Helpful.Others.Random(4, 10));
                nManager.Wow.Helpers.Lua.LuaDoString(destName + " = TaxiNodeName(" + id + ");");
                nManager.Wow.Helpers.Lua.LuaDoString(chkpx + "," + chkpy + " = TaxiNodePosition(" + id + ");");
                string retpx = nManager.Wow.Helpers.Lua.GetLocalizedText(chkpx);
                string retpy = nManager.Wow.Helpers.Lua.GetLocalizedText(chkpy);
                string retdestName = nManager.Wow.Helpers.Lua.GetLocalizedText(destName);
                nManager.Helpful.Logging.WriteDebug("Slot " + id + ", Destination Name " + retdestName + ", px " + retpx + ", py " + retpy);
            }
        }
    }
}