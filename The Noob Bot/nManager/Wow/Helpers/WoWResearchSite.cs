using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using nManager.Helpful;

namespace nManager.Wow.Helpers
{
    public class WoWResearchSite
    {
        private ResearchSiteDb2Record _rSiteDB2Record0;

        private static DB5Reader _rSiteDB2;
        private static ResearchSiteDb2Record[] _cachedRecords;
        private static BinaryReader[] _cachedResearchSiteRows;

        private static void init()
        {
            if (_rSiteDB2 == null)
            {
                var mDefinitions = DBFilesClient.Load(Application.StartupPath + @"\Data\DBFilesClient\dblayout.xml");
                var definitions = mDefinitions.Tables.Where(t => t.Name == "ResearchSite");

                if (!definitions.Any())
                {
                    definitions = mDefinitions.Tables.Where(t => t.Name == Path.GetFileName("ResearchSite"));
                }
                if (definitions.Count() == 1)
                {
                    var table = definitions.First();
                    _rSiteDB2 = DBReaderFactory.GetReader(Application.StartupPath + @"\Data\DBFilesClient\ResearchSite.db2", table) as DB5Reader;
                    if (_cachedResearchSiteRows == null || _cachedRecords == null)
                    {
                        if (_rSiteDB2 != null)
                        {
                            _cachedResearchSiteRows = _rSiteDB2.Rows.ToArray();
                            _cachedRecords = new ResearchSiteDb2Record[_cachedResearchSiteRows.Length];
                            for (int i = 0; i < _cachedResearchSiteRows.Length - 1; i++)
                            {
                                _cachedRecords[i] = DB5Reader.ByteToType<ResearchSiteDb2Record>(_cachedResearchSiteRows[i]);
                            }
                        }
                    }
                    Logging.Write(_rSiteDB2.FileName + " loaded with " + _rSiteDB2.RecordsCount + " entries.");
                }
                else
                {
                    Logging.Write("DB2 ResearchSite not read-able.");
                }
            }
        }

        private WoWResearchSite(string name, bool SecondOne = false)
        {
            bool second = false;
            init();
            ResearchSiteDb2Record tempResearchSiteDb2Record = new ResearchSiteDb2Record();
            bool found = false;
            for (int i = 0; i < _rSiteDB2.RecordsCount - 1; i++)
            {
                tempResearchSiteDb2Record = _cachedRecords[i];
                if (tempResearchSiteDb2Record.Name() == name && tempResearchSiteDb2Record.Map == Usefuls.ContinentId)
                {
                    if (SecondOne && !second)
                        second = true;
                    else
                    {
                        found = true;
                        break;
                    }
                }
            }
            _rSiteDB2Record0 = found ? tempResearchSiteDb2Record : new ResearchSiteDb2Record();
        }

        public static List<ResearchSiteDb2Record> ExtractAllDigsites()
        {
            List<ResearchSiteDb2Record> digSites = new List<ResearchSiteDb2Record>();
            init();
            ResearchSiteDb2Record tempResearchSiteDb2Record = new ResearchSiteDb2Record();

            for (int i = 0; i < _rSiteDB2.RecordsCount - 1; i++)
            {
                tempResearchSiteDb2Record = _cachedRecords[i];
                digSites.Add(tempResearchSiteDb2Record);
            }

            return digSites;
        }

        private WoWResearchSite(int reqId)
        {
            init();
            ResearchSiteDb2Record tempResearchSiteDb2Record = new ResearchSiteDb2Record();
            bool found = false;
            for (int i = 0; i < _rSiteDB2.RecordsCount - 1; i++)
            {
                tempResearchSiteDb2Record = _cachedRecords[i];
                if (tempResearchSiteDb2Record.Id == reqId)
                {
                    found = true;
                    break;
                }
            }
            _rSiteDB2Record0 = found ? tempResearchSiteDb2Record : new ResearchSiteDb2Record();
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
            get { return _rSiteDB2Record0.Name(); }
        }

        // I don't like to expose this, but did not find another way
        public int MinIndex
        {
            get
            {
                init();
                return _rSiteDB2.MinId;
            }
        }

        public int MaxIndex
        {
            get
            {
                init();
                return _rSiteDB2.MaxId;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ResearchSiteDb2Record
        {
            public uint Id;
            public uint QuestIdPoint;
            public uint NameOffset;
            public ushort Map;
            public byte textureIndex; // 177
            public byte Unk2;

            public string Name()
            {
                string strValue;
                if (_rSiteDB2.StringTable != null && _rSiteDB2.StringTable.TryGetValue((int) NameOffset, out strValue))
                {
                    return strValue;
                }
                return "";
            }
        }
    }
}