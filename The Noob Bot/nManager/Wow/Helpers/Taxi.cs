using nManager.Helpful;

namespace nManager.Wow.Helpers
{
    public class Taxi
    {
        public static bool IsTaxiWindowOpen()
        {
            string randomString = Others.GetRandomString(Others.Random(4, 10));
            Lua.LuaDoString(randomString + " = NumTaxiNodes()");
            string result = Lua.GetLocalizedText(randomString);
            return Others.ToInt32(result) > 0;
        }

        public static void FindAndOpenTaxiGossip()
        {
            string randomString = Others.GetRandomString(Others.Random(4, 10));
            string command = "i = 1 GossipOptions = GetGossipOptions() for txt, type in pairs(GossipOptions) do ";
            command += "if type == \"Taxi\" then " + randomString + " = i end i++ ";
            command += "end";
            Lua.LuaDoString(command);
            int index = Others.ToInt32(Lua.GetLocalizedText(randomString));
            Lua.LuaDoString("SelectGossipOption(" + index + ")");
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
    }
}