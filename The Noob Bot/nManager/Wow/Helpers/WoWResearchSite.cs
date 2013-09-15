using System.Runtime.InteropServices;
using nManager.Wow.Patchables;

namespace nManager.Wow.Helpers
{
    internal class WoWResearchSite
    {
        private ResearchSiteDbcRecord _rSiteDBCRecord0;

        private static DBC<ResearchSiteDbcRecord> _rSiteDBC;

        private WoWResearchSite(string name)
        {
            if (_rSiteDBC == null)
                _rSiteDBC = new DBC<ResearchSiteDbcRecord>((int) Addresses.DBC.ResearchSite);
            for (int id = _rSiteDBC.MinIndex; id <= _rSiteDBC.MaxIndex; id++)
            {
                _rSiteDBCRecord0 = _rSiteDBC.GetRow(id);
                if (_rSiteDBCRecord0.Id == id)
                {
                    string temp = Name;
                    if (temp == name)
                    {
                        return;
                    }
                }
            }
            _rSiteDBCRecord0 = new ResearchSiteDbcRecord();
        }

        private WoWResearchSite(int reqId)
        {
            if (_rSiteDBC == null)
                _rSiteDBC = new DBC<ResearchSiteDbcRecord>((int) Addresses.DBC.ResearchSite);
            for (int id = _rSiteDBC.MinIndex; id <= _rSiteDBC.MaxIndex; id++)
            {
                _rSiteDBCRecord0 = _rSiteDBC.GetRow(id);
                if (_rSiteDBCRecord0.Id == id && id == reqId)
                {
                    return;
                }
            }
            _rSiteDBCRecord0 = new ResearchSiteDbcRecord();
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
            get { return _rSiteDBCRecord0; }
        }

        public string Name
        {
            get
            {
                return _rSiteDBC.String(_rSiteDBC.GetRowOffset((int) _rSiteDBCRecord0.Id) +
                                        _rSiteDBCRecord0.NameOffset +
                                        (uint) Marshal.OffsetOf(typeof (ResearchSiteDbcRecord), "NameOffset"));
            }
        }

        // I don't like to expose this, but did not find another way
        public int MinIndex
        {
            get
            {
                if (_rSiteDBC == null)
                    _rSiteDBC = new DBC<ResearchSiteDbcRecord>((int) Addresses.DBC.ResearchSite);
                return _rSiteDBC.MinIndex;
            }
        }

        public int MaxIndex
        {
            get
            {
                if (_rSiteDBC == null)
                    _rSiteDBC = new DBC<ResearchSiteDbcRecord>((int) Addresses.DBC.ResearchSite);
                return _rSiteDBC.MaxIndex;
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