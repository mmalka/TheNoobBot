using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using nManager.Wow.Enums;
using nManager.Wow.Patchables;

namespace nManager.Wow.Helpers
{
    class WoWResearchSite
    {
        private ResearchSiteDbcRecord rSiteDbcRecord_0;

        private static DBC<ResearchSiteDbcRecord> rSiteDbc;

        private WoWResearchSite(string name)
        {
            if (rSiteDbc == null)
                rSiteDbc = new DBC<ResearchSiteDbcRecord>((int)Addresses.DBC.ResearchSite);
            for (int id = rSiteDbc.MinIndex; id <= rSiteDbc.MaxIndex; id++)
            {
                rSiteDbcRecord_0 = rSiteDbc.GetRow((int)id);
                if (rSiteDbcRecord_0.Id == id)
                {
                    string temp = rSiteDbc.String(rSiteDbcRecord_0.NameOffset);
                    if (temp == name)
                    {
                        return;
                    }
                }
            }
        }

        private WoWResearchSite(int reqId)
        {
            if (rSiteDbc == null)
                rSiteDbc = new DBC<ResearchSiteDbcRecord>((int)Addresses.DBC.ResearchSite);
            for (int id = rSiteDbc.MinIndex; id <= rSiteDbc.MaxIndex; id++)
            {
                rSiteDbcRecord_0 = rSiteDbc.GetRow((int)id);
                if (rSiteDbcRecord_0.Id == id && id == reqId)
                {
                    return;
                }
            }
        }

        // Factory function
        public static WoWResearchSite FromName(string name)
        {
            return new WoWResearchSite(name);
        }

        public static WoWResearchSite FromId(int id)
        {
            return new WoWResearchSite(id);
        }

        public ResearchSiteDbcRecord Record
        {
            get { return this.rSiteDbcRecord_0; }
        }

        public string Name
        {
            get { return rSiteDbc.String(this.rSiteDbcRecord_0.NameOffset);  }
        }

        // I don't like to expose this, but did not find another way
        public int MinIndex
        {
            get
            {
                if (rSiteDbc == null)
                    rSiteDbc = new DBC<ResearchSiteDbcRecord>((int)Addresses.DBC.ResearchSite);
                return rSiteDbc.MinIndex;
            }
        }
        public int MaxIndex
        {
            get
            {
                if (rSiteDbc == null)
                    rSiteDbc = new DBC<ResearchSiteDbcRecord>((int)Addresses.DBC.ResearchSite);
                return rSiteDbc.MaxIndex;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ResearchSiteDbcRecord
        {
            public uint Id;
            public uint Map;
            public uint QuestIdPoint;
            public uint NameOffset;
            public uint Unk;
        }
    }
}
