using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms.VisualStyles;

namespace meshDatabase.Database
{
    public class TaxiData
    {
        public TaxiNode.TaxiNodesDb2Record From { get; private set; }
        public Dictionary<int, TaxiNode.TaxiNodesDb2Record> To { get; private set; }

        public TaxiData(TaxiNode.TaxiNodesDb2Record from, Dictionary<int, TaxiNode.TaxiNodesDb2Record> to)
        {
            From = from;
            To = to;
        }
    }

    public static class TaxiHelper
    {
        private static DB5Reader _taxiNodes;
        private static DB5Reader _taxiPath;
        private static bool _initialized;
        private static BinaryReader[] _cachedTaxiNodesRows;
        private static BinaryReader[] _cachedTaxiPathRows;
        private static TaxiNode.TaxiNodesDb2Record[] _cachedTaxiNodesRecords;
        private static TaxiPath.TaxiPathDb2Record[] _cachedTaxiPathRecords;

        public static DB5Reader TaxiNodesDBC
        {
            get
            {
                Initialize();
                return _taxiNodes;
            }
        }

        public static DB5Reader TaxiPathDBC
        {
            get
            {
                Initialize();
                return _taxiPath;
            }
        }

        public static void Initialize()
        {
            if (_initialized)
                return;

            //MpqManager.InitializeDBC();
            _taxiNodes = MpqManager.GetDB5("TaxiNodes");
            _taxiPath = MpqManager.GetDB5("TaxiPath");
            _cachedTaxiNodesRows = _taxiNodes.Rows.ToArray();
            _cachedTaxiPathRows = _taxiNodes.Rows.ToArray();

            _cachedTaxiPathRecords = new TaxiPath.TaxiPathDb2Record[_cachedTaxiPathRows.Length];
            for (int i = 0; i < _cachedTaxiPathRows.Length - 1; i++)
            {
                _cachedTaxiPathRecords[i] = DB5Reader.ByteToType<TaxiPath.TaxiPathDb2Record>(_cachedTaxiPathRows[i]);
            }
            _cachedTaxiNodesRecords = new TaxiNode.TaxiNodesDb2Record[_cachedTaxiNodesRows.Length];
            for (int i = 0; i < _cachedTaxiNodesRows.Length - 1; i++)
            {
                _cachedTaxiNodesRecords[i] = DB5Reader.ByteToType<TaxiNode.TaxiNodesDb2Record>(_cachedTaxiNodesRows[i]);
            }
            _initialized = true;
        }

        public static TaxiData GetTaxiData(TaxiNode.TaxiNodesDb2Record from)
        {
            var to = new Dictionary<int, TaxiNode.TaxiNodesDb2Record>();


            foreach (var record in _cachedTaxiPathRecords)
            {
                if (record.From != from.Id)
                    continue;
                var nodeRecord = GetNode(record.To);
                if (nodeRecord.IsValid())
                    to.Add(record.Id, nodeRecord);
            }
            return new TaxiData(from, to);
        }

        public static TaxiPath.TaxiPathDb2Record GetRecordById(int id)
        {
            Initialize();

            foreach (var record in _cachedTaxiPathRecords)
            {
                if (record.Id == id)
                    return record;
            }
            return new TaxiPath.TaxiPathDb2Record();
        }

        public static TaxiNode.TaxiNodesDb2Record GetNode(int id)
        {
            Initialize();

            foreach (var record in _cachedTaxiNodesRecords)
            {
                if (record.Id == id)
                    return record;
            }
            return new TaxiNode.TaxiNodesDb2Record();
        }

        public static List<TaxiNode.TaxiNodesDb2Record> GetNodesInBBox(int mapId, float[] bmin, float[] bmax)
        {
            Initialize();

            var ret = new List<TaxiNode.TaxiNodesDb2Record>(2);
            foreach (var record in _cachedTaxiNodesRecords)
            {
                var data = record;
                if (!data.IsValid())
                    continue;

                if (data.MapId != mapId)
                    continue;

                if (data.Location.X >= bmin[0] && data.Location.Y >= bmin[1] && data.Location.X <= bmax[0] && data.Location.Y <= bmax[1])
                    ret.Add(data);
            }
            return ret;
        }
    }
}