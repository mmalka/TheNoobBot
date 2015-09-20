using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using SlimDX;

namespace meshDatabase.Database
{

    public static class GameObjectHelper
    {
        private static DBC _gameobjectDisplayInfo;
        private static DBC _fileData;
        private static bool _initialized;
        private static IEnumerable<GameobjectDisplayInfoEntry> _displayInfoEntries;
        private static IEnumerable<FileDataEntry> _filaDataEntries;
        private static MySqlConnection _myConn;
        private static Dictionary<int, string> _cache;
    
        public static void Initialize()
        {
            if (_initialized)
                return;

            _cache = new Dictionary<int, string>(1000);
            _gameobjectDisplayInfo = MpqManager.GetDBC("GameobjectDisplayInfo");
            _displayInfoEntries = _gameobjectDisplayInfo.Records.Select(r => new GameobjectDisplayInfoEntry(r));
            _fileData = MpqManager.GetDBC("FileData");
            _filaDataEntries = _fileData.Records.Select(r => new FileDataEntry(r));
            _myConn = new MySqlConnection("server=192.168.10.222; user id=root; password=aabbcc; database=offydump;");
            _myConn.Open();

            /*List<MapEntry> titi = PhaseHelper.GetAllMaps();
            foreach (MapEntry e in titi)
            {
                string sqlcode = "INSERT INTO Maps VALUES (" + e.Id + ",'" + e.InternalName.Replace("'", "\\'") + "','" + e.Name.Replace("'", "\\'") + "',";
                sqlcode += (int)e.InstanceType + "," + (int)e.MapType + "," + e.PhaseParent + "," + e.Flags + ");";
                System.Console.WriteLine(sqlcode);
                var cmd = new MySqlCommand(sqlcode, _myConn);
                cmd.ExecuteNonQuery();
            }*/
            _initialized = true;
        }

        public static string GetFullFileNameFromDisplayId(int displayId)
        {
            Initialize();
            string ret;
            if (_cache.TryGetValue(displayId, out ret))
                return ret;
            var displayInfoEntry = _displayInfoEntries.Where(e => e.DisplayId == displayId).FirstOrDefault();
            if (displayInfoEntry == null)
                return string.Empty;

            var fileDataEntry = _filaDataEntries.Where(e => e.DataId == displayInfoEntry.DataId).FirstOrDefault();
            if (fileDataEntry == null)
                return string.Empty;
            ret = fileDataEntry.FilePath + fileDataEntry.FileName;
            _cache.Add(displayId, ret);
            return ret;
        }

        // Alliance Fief maps lvl1, 2, 3 then Horde ones
        public static bool IsFiefMap(int id)
        {
            return (id == 1158 || id == 1159 || id == 1331 || id == 1152 || id == 1153 || id == 1330);
        }

        public static IEnumerable<GameObject> GetAllGameobjectInBoundingBox(float[] bmin, float[] bmax, int mapId)
        {
            Initialize();
            // to wow : -v.Z, -v.X, -v.Y
            Vector3 bbmax = new Vector3(-bmin[2], -bmin[0], -bmin[1]);
            Vector3 bbmin = new Vector3(-bmax[2], -bmax[0], -bmax[1]);
            List<GameObject> returnResult = new List<GameObject>();
            string filter = (IsFiefMap(mapId) ? "gt.type in (1,2,5,19,44)": "gt.type in (1,2,5,7,8,19,22,23,43)");
            string query = "select go.entry, gt.model, " +
                "go.m11, go.m12, go.m13, go.m14, go.m21, go.m22, go.m23, go.m24, go.m31, go.m32, go.m33, go.m34, go.m41, go.m42, go.m43, go.m44"+
                //", go.r0, go.r1, go.r2" +
                " from gameobject go left join gameobject_template gt on go.entry=gt.entry where go.map=" + mapId + (filter.Length != 0 ? " and " + filter : "") +
                " and go.x >= " + bbmin[0].ToString(System.Globalization.CultureInfo.InvariantCulture) + " and go.x <= " + bbmax[0].ToString(System.Globalization.CultureInfo.InvariantCulture) + " " +
                " and go.y >= " + bbmin[1].ToString(System.Globalization.CultureInfo.InvariantCulture) + " and go.y <= " + bbmax[1].ToString(System.Globalization.CultureInfo.InvariantCulture) + ";";
            //System.Console.WriteLine(query);
            var cmd = new MySqlCommand(query, _myConn);
            MySqlDataReader result = cmd.ExecuteReader();
            if (result.HasRows)
            {
                while (result.Read())
                {
                    GameObject go = new GameObject();
                    go.Id = result.GetInt32(0);
                    go.Model = result.GetInt32(1);
                    Matrix m = new Matrix();
                    // I reflect the matrix because it's bad, reflect is on this side: \
                    m.M11 = result.GetFloat(2);
                    m.M21 = result.GetFloat(3);
                    m.M31 = result.GetFloat(4);
                    m.M41 = result.GetFloat(5);
                    m.M12 = result.GetFloat(6);
                    m.M22 = result.GetFloat(7);
                    m.M32 = result.GetFloat(8);
                    m.M42 = result.GetFloat(9);
                    m.M13 = result.GetFloat(10);
                    m.M23 = result.GetFloat(11);
                    m.M33 = result.GetFloat(12);
                    m.M43 = result.GetFloat(13);
                    m.M14 = result.GetFloat(14);
                    m.M24 = result.GetFloat(15);
                    m.M34 = result.GetFloat(16);
                    m.M44 = result.GetFloat(17);
                    go.Transformation = m;
                    returnResult.Add(go);
                }
            }
            result.Close();
            return returnResult;
        }

        public static IEnumerable<GameObject> GetAllGameobjectInMap(int mapId)
        {
            Initialize();
            List<GameObject> returnResult = new List<GameObject>();
            string filter = "gt.type in (1,2,5,7,8,19,22,23,43)";
            string query = "select go.entry, gt.model, " +
                "go.m11, go.m12, go.m13, go.m14, go.m21, go.m22, go.m23, go.m24, go.m31, go.m32, go.m33, go.m34, go.m41, go.m42, go.m43, go.m44" +
                //", go.r0, go.r1, go.r2" +
                " from gameobject go left join gameobject_template gt on go.entry=gt.entry where go.map=" + mapId + (filter.Length != 0 ? " and " + filter : "") + ";";
            //System.Console.WriteLine(query);
            var cmd = new MySqlCommand(query, _myConn);
            MySqlDataReader result = cmd.ExecuteReader();
            if (result.HasRows)
            {
                while (result.Read())
                {
                    GameObject go = new GameObject();
                    go.Id = result.GetInt32(0);
                    go.Model = result.GetInt32(1);
                    Matrix m = new Matrix();
                    // I reflect the matrix because it's bad, reflect is on this side: \
                    m.M11 = result.GetFloat(2);
                    m.M21 = result.GetFloat(3);
                    m.M31 = result.GetFloat(4);
                    m.M41 = result.GetFloat(5);
                    m.M12 = result.GetFloat(6);
                    m.M22 = result.GetFloat(7);
                    m.M32 = result.GetFloat(8);
                    m.M42 = result.GetFloat(9);
                    m.M13 = result.GetFloat(10);
                    m.M23 = result.GetFloat(11);
                    m.M33 = result.GetFloat(12);
                    m.M43 = result.GetFloat(13);
                    m.M14 = result.GetFloat(14);
                    m.M24 = result.GetFloat(15);
                    m.M34 = result.GetFloat(16);
                    m.M44 = result.GetFloat(17);
                    go.Transformation = m;
                    returnResult.Add(go);
                }
            }
            result.Close();
            return returnResult;
        }
    }
}
