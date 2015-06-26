using System.Runtime.InteropServices;
using nManager.Wow.Patchables;

namespace nManager.Wow.Helpers
{
    public class WoWResearchSite
    {
        private ResearchSiteDb2Record _rSiteDB2Record0;

        private static DB2<ResearchSiteDb2Record> _rSiteDB2;

        private static void init()
        {
            if (_rSiteDB2 == null)
                _rSiteDB2 = new DB2<ResearchSiteDb2Record>((int)Addresses.DBC.ResearchSite);
        }

        private WoWResearchSite(string name, bool SecondOne = false)
        {
            bool second = false;
            init();
            for (int id = _rSiteDB2.MinIndex; id <= _rSiteDB2.MaxIndex; id++)
            {
                _rSiteDB2Record0 = _rSiteDB2.GetRow(id);
                if (_rSiteDB2Record0.Id == id)
                {
                    string temp = Name;
                    if (temp == name)
                    {
                        if (SecondOne && !second)
                            second = true;
                        else
                            return;
                    }
                }
            }
            _rSiteDB2Record0 = new ResearchSiteDb2Record();
        }

        private WoWResearchSite(int reqId)
        {
            init();
            for (int id = _rSiteDB2.MinIndex; id <= _rSiteDB2.MaxIndex; id++)
            {
                _rSiteDB2Record0 = _rSiteDB2.GetRow(id);
                if (_rSiteDB2Record0.Id == id && id == reqId)
                {
                    return;
                }
            }
            _rSiteDB2Record0 = new ResearchSiteDb2Record();
        }

        // Factory function
        public static WoWResearchSite FromName(string name, bool secondOne = false)
        {
            return new WoWResearchSite(name, secondOne);
        }

        public static WoWResearchSite FromId(int id)
        {
            return new WoWResearchSite(id);
        }

        public ResearchSiteDb2Record Record
        {
            get { return _rSiteDB2Record0; }
        }

        public string Name
        {
            get
            {
                return _rSiteDB2.String(_rSiteDB2.GetRowOffset((int) _rSiteDB2Record0.Id) +
                                        _rSiteDB2Record0.NameOffset +
                                        (uint) Marshal.OffsetOf(typeof (ResearchSiteDb2Record), "NameOffset"));
            }
        }

        // I don't like to expose this, but did not find another way
        public int MinIndex
        {
            get
            {
                init();
                return _rSiteDB2.MinIndex;
            }
        }

        public int MaxIndex
        {
            get
            {
                init();
                return _rSiteDB2.MaxIndex;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ResearchSiteDb2Record
        {
            public uint Id;
            public uint Map;
            public uint QuestIdPoint;
            public uint NameOffset;
            public uint Unk;
        }
    }
}