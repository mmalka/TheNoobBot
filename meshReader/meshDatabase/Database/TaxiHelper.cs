using System.Collections.Generic;

namespace meshDatabase.Database
{

    public class TaxiData
    {
        public TaxiNode From { get; private set; }
        public Dictionary<int, TaxiNode> To { get; private set; }

        public TaxiData(TaxiNode from, Dictionary<int, TaxiNode> to)
        {
            From = from;
            To = to;
        }
    }
    
    public static class TaxiHelper
    {
        private static DBC _taxiNodes;
        private static DBC _taxiPath;
        private static bool _initialized;

        public static DBC TaxiNodesDBC
        {
            get
            {
                Initialize();
                return _taxiNodes;
            }
        }

        public static DBC TaxiPathDBC
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

            MpqManager.InitializeDBC();
            _taxiNodes = MpqManager.GetDBC("TaxiNodes");
            _taxiPath = MpqManager.GetDBC("TaxiPath");
            _initialized = true;
        }

        public static TaxiData GetTaxiData(TaxiNode from)
        {
            var to = new Dictionary<int, TaxiNode>();
            foreach (var record in _taxiPath.Records)
            {
                var data = new TaxiPath(record);
                if (!data.IsValid || data.From != from.Id)
                    continue;

                var nodeRecord = _taxiNodes.GetRecordById(data.To);
                if (nodeRecord == null)
                    continue;
                to.Add(data.Id, new TaxiNode(nodeRecord));
            }

            return new TaxiData(from, to);
        }

        public static TaxiPath GetPath(int id)
        {
            Initialize();

            var record = _taxiPath.GetRecordById(id);
            if (record == null)
                return null;
            return new TaxiPath(record);
        }

        public static TaxiNode GetNode(int id)
        {
            Initialize();

            var record = _taxiNodes.GetRecordById(id);
            if (record == null)
                return null;
            return new TaxiNode(record);
        }

        public static List<TaxiNode> GetNodesInBBox(int mapId, float[] bmin, float[] bmax)
        {
            Initialize();

            var ret = new List<TaxiNode>(2);
            foreach (var record in _taxiNodes.Records)
            {
                var data = new TaxiNode(record);
                if (!data.IsValid)
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