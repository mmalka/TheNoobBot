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
            return; // hack gameobject
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
            return (id == 1158 || id == 1331 || id == 1159 || id == 1152 || id == 1330 || id == 1153);
        }

        public static IEnumerable<GameObject> GetAllGameobjectInBoundingBox(float[] bmin, float[] bmax, int mapId)
        {
            Initialize();
            // to wow : -v.Z, -v.X, v.Y
            Vector3 bbmax = new Vector3(-bmin[2], -bmin[0], -bmin[1]);
            Vector3 bbmin = new Vector3(-bmax[2], -bmax[0], -bmax[1]);
            List<GameObject> returnResult = new List<GameObject>();
            string filter = (IsFiefMap(mapId) ? "gt.type in (1,2,5,19,44)" : "gt.type in (1,2,5,7,8,19,22,23,38,44,46)");
            //filter = "gt.type not in (0,38)";
            string query = "select go.entry, gt.model, " +
                "go.m11, go.m12, go.m13, go.m14, go.m21, go.m22, go.m23, go.m24, go.m31, go.m32, go.m33, go.m34, go.m41, go.m42, go.m43, go.m44, gt.type" +
                //", go.r0, go.r1, go.r2" +
                " from gameobject go left join gameobject_template gt on go.entry=gt.entry where go.map=" + mapId + (filter.Length != 0 ? " and " + filter : "") +
                " and go.x >= " + bbmin.X.ToString(System.Globalization.CultureInfo.InvariantCulture) + " and go.x <= " + bbmax.X.ToString(System.Globalization.CultureInfo.InvariantCulture) + " " +
                " and go.y >= " + bbmin.Y.ToString(System.Globalization.CultureInfo.InvariantCulture) + " and go.y <= " + bbmax.Y.ToString(System.Globalization.CultureInfo.InvariantCulture) + ";";
            //System.Console.WriteLine(query);
            MySqlDataReader result;
            MySqlCommand cmd;
            cmd = new MySqlCommand(query, _myConn);
            result = cmd.ExecuteReader();
            if (result.HasRows)
            {
                while (result.Read())
                {
                    GameObject go = new GameObject();
                    //go.Id = result.GetInt32(0);
                    //if (go.Id == 232409)
                    //    continue;
                    int model = result.GetInt32(1);
                    int type = result.GetInt32(18);
                    go.Model = model;
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
            // now fill all the plots with big constructions
            if (mapId == 1159 || mapId == 1331)
            {
                GameObject go = new GameObject();
                //go.Id = 225540; // Garrison - Alliance Mine 3 or 2
                go.Model = mapId == 1159 ? 14648 : 14647;
                Matrix m = new Matrix();
                m.M11 = 0.939693f;
                m.M21 = -0.34202f;
                m.M31 = 0f;
                m.M41 = 1845.08f;
                m.M12 = 0.34202f;
                m.M22 = 0.939693f;
                m.M32 = 0f;
                m.M42 = 146.274f;
                m.M13 = 0f;
                m.M23 = 0f;
                m.M33 = 1f;
                m.M43 = 53.4169f;
                m.M14 = 0f;
                m.M24 = 0f;
                m.M34 = 0f;
                m.M44 = 1f;
                go.Transformation = m;
                returnResult.Add(go);
            }
            if (mapId == 1159 || mapId == 1331)
            {
                GameObject go = new GameObject();
                //go.Id = 235991; // Garrison Building Farm V3
                go.Model = 20785;
                Matrix m = new Matrix();
                m.M11 = -0.906306f;
                m.M21 = -0.422623f;
                m.M31 = 0f;
                m.M41 = 1847.61f;
                m.M12 = 0.422623f;
                m.M22 = -0.906306f;
                m.M32 = 0f;
                m.M42 = 134.726f;
                m.M13 = 0f;
                m.M23 = 0f;
                m.M33 = 1f;
                m.M43 = 78.107f;
                m.M14 = 0f;
                m.M24 = 0f;
                m.M34 = 0f;
                m.M44 = 1f;
                go.Transformation = m;
                returnResult.Add(go);
            }
            if (mapId == 1159 || mapId == 1331)
            {
                GameObject go = new GameObject();
                //go.Id = 230989; // Garrison Building Alliance Fishing V3
                go.Model = 16435;
                Matrix m = new Matrix();
                m.M11 = -0.953716f;
                m.M21 = -0.300707f;
                m.M31 = 0f;
                m.M41 = 2031.59f;
                m.M12 = 0.300707f;
                m.M22 = -0.953716f;
                m.M32 = 0f;
                m.M42 = 174.441f;
                m.M13 = 0f;
                m.M23 = 0f;
                m.M33 = 1f;
                m.M43 = 84.366f;
                m.M14 = 0f;
                m.M24 = 0f;
                m.M34 = 0f;
                m.M44 = 1f;
                go.Transformation = m;
                returnResult.Add(go);
            }
            if (mapId == 1159) // 1st medium slot in garrison level 3 only
            {
                GameObject go = new GameObject();
                //go.Id = 224807; // Garrison Building Inn V3
                go.Model = 14617;
                Matrix m = new Matrix();
                m.M11 = -0.819149f;
                m.M21 = -0.57358f;
                m.M31 = 0f;
                m.M41 = 1893.34f;
                m.M12 = 0.57358f;
                m.M22 = -0.819149f;
                m.M32 = 0f;
                m.M42 = 185.168f;
                m.M13 = 0f;
                m.M23 = 0f;
                m.M33 = 1f;
                m.M43 = 78.0562f;
                m.M14 = 0f;
                m.M24 = 0f;
                m.M34 = 0f;
                m.M44 = 1f;
                go.Transformation = m;
                returnResult.Add(go);
            }
            if (mapId == 1159 || mapId == 1331) // 2nd medium slot in garrison level 3 and level 2
            {
                GameObject go = new GameObject();
                //go.Id = 224807; // Garrison Building Inn V3
                go.Model = 14617;
                Matrix m = new Matrix();
                m.M11 = 0.0871575f;
                m.M21 = 0.996194f;
                m.M31 = 0f;
                m.M41 = 1864.95f;
                m.M12 = -0.996194f;
                m.M22 = 0.0871575f;
                m.M32 = 0f;
                m.M42 = 320.208f;
                m.M13 = 0f;
                m.M23 = 0f;
                m.M33 = 1f;
                m.M43 = 81.6605f;
                m.M14 = 0f;
                m.M24 = 0f;
                m.M34 = 0f;
                m.M44 = 1f;
                go.Transformation = m;
                returnResult.Add(go);
            }
            if (mapId == 1159 || mapId == 1331 || mapId == 1158) // Big slot in garrison all levels
            {
                GameObject go = new GameObject();
                //go.Id = 224799; // Garrison Building Barracks V3
                go.Model = 14400;
                Matrix m = new Matrix();
                m.M11 = -0.933579f;
                m.M21 = -0.358373f;
                m.M31 = 0f;
                m.M41 = 1918.64f;
                m.M12 = 0.358373f;
                m.M22 = -0.933579f;
                m.M32 = 0f;
                m.M42 = 228.767f;
                m.M13 = 0f;
                m.M23 = 0f;
                m.M33 = 1f;
                m.M43 = 76.6396f;
                m.M14 = 0f;
                m.M24 = 0f;
                m.M34 = 0f;
                m.M44 = 1f;
                go.Transformation = m;
                returnResult.Add(go);
            }
            if (mapId == 1159) // 2nd Big slot in garrison level 3
            {
                GameObject go = new GameObject();
                //go.Id = 224799; // Garrison Building Barracks V3
                go.Model = 14400;
                Matrix m = new Matrix();
                m.M11 = 0.793354f;
                m.M21 = 0.60876f;
                m.M31 = 0f;
                m.M41 = 1814.32f;
                m.M12 = -0.60876f;
                m.M22 = 0.793354f;
                m.M32 = 0f;
                m.M42 = 286.394f;
                m.M13 = 0f;
                m.M23 = 0f;
                m.M33 = 1f;
                m.M43 = 76.6249f;
                m.M14 = 0f;
                m.M24 = 0f;
                m.M34 = 0f;
                m.M44 = 1f;
                go.Transformation = m;
                returnResult.Add(go);
            }
            if (mapId == 1159 || mapId == 1331 || mapId == 1158) // small slot in garrison all levels
            {
                GameObject go = new GameObject();
                go.Model = 19353; // Garrison Building Under Construction V1
                Matrix m = new Matrix();
                m.M11 = -0.325565f;
                m.M21 = -0.94552f;
                m.M31 = 0f;
                m.M41 = 1830.53f;
                m.M12 = 0.94552f;
                m.M22 = -0.325565f;
                m.M32 = 0f;
                m.M42 = 196.747f;
                m.M13 = 0f;
                m.M23 = 0f;
                m.M33 = 1f;
                m.M43 = 71.9859f;
                m.M14 = 0f;
                m.M24 = 0f;
                m.M34 = 0f;
                m.M44 = 1f;
                go.Transformation = m;
                returnResult.Add(go);
                GameObject go2 = new GameObject(go);
                Quaternion simpleRotation = new Quaternion(0, 0, 1, 0); // Z rotation
                Matrix m2 = Matrix.RotationQuaternion(simpleRotation);
                go2.Transformation = m2 * m;
                returnResult.Add(go2);
            }
            if (mapId == 1159 || mapId == 1331) // small slot in garrison level 3 and level 2
            {
                GameObject go = new GameObject();
                go.Model = 19353; // Garrison Building Under Construction V1
                Matrix m = new Matrix();
                m.M11 = 0.275637f;
                m.M21 = 0.961262f;
                m.M31 = 0f;
                m.M41 = 1819.58f;
                m.M12 = -0.961262f;
                m.M22 = 0.275637f;
                m.M32 = 0f;
                m.M42 = 231.281f;
                m.M13 = 0f;
                m.M23 = 0f;
                m.M33 = 1f;
                m.M43 = 72.174f;
                m.M14 = 0f;
                m.M24 = 0f;
                m.M34 = 0f;
                m.M44 = 1f;
                go.Transformation = m;
                returnResult.Add(go);
                GameObject go2 = new GameObject(go);
                Quaternion simpleRotation = new Quaternion(0, 0, 1, 0); // Z rotation
                Matrix m2 = Matrix.RotationQuaternion(simpleRotation);
                go2.Transformation = m2 * m;
                returnResult.Add(go2);
            }
            if (mapId == 1159) // small slot in garrison level 3 only
            {
                GameObject go = new GameObject();
                go.Model = 19353; // Garrison Building Under Construction V1
                Matrix m = new Matrix();
                m.M11 = -0.382683f;
                m.M21 = -0.92388f;
                m.M31 = 0f;
                m.M41 = 1804.33f;
                m.M12 = 0.92388f;
                m.M22 = -0.382683f;
                m.M32 = 0f;
                m.M42 = 189.146f;
                m.M13 = 0f;
                m.M23 = 0f;
                m.M33 = 1f;
                m.M43 = 70.0748f;
                m.M14 = 0f;
                m.M24 = 0f;
                m.M34 = 0f;
                m.M44 = 1f;
                go.Transformation = m;
                returnResult.Add(go);
                GameObject go2 = new GameObject(go);
                Quaternion simpleRotation = new Quaternion(0, 0, 1, 0); // Z rotation
                Matrix m2 = Matrix.RotationQuaternion(simpleRotation);
                go2.Transformation = m2 * m;
                returnResult.Add(go2);
            }

            /* Here I will add custom GOs to make the garden */
            if (mapId == 1159 || mapId == 1331)
            {
                // Lampe à gauche dans le gardin
                GameObject go = new GameObject();
                go.Model = 15246; // Mailbox
                Matrix m = new Matrix();
                m.M11 = -0.852638f;
                m.M21 = 0.522501f;
                m.M31 = 0f;
                m.M41 = 1829.552f;
                m.M12 = -0.522501f;
                m.M22 = -0.852638f;
                m.M32 = 0f;
                m.M42 = 147.0054f;
                m.M13 = 0f;
                m.M23 = 0f;
                m.M33 = 1f;
                m.M43 = 76.60423f;
                m.M14 = 0f;
                m.M24 = 0f;
                m.M34 = 0f;
                m.M44 = 1f;
                go.Transformation = m;
                returnResult.Add(go);
                GameObject go4 = new GameObject(go);
                m.M41 = 1830.246f;
                m.M42 = 146.2089f;
                m.M43 = 76.7339f;
                go4.Transformation = m;
                returnResult.Add(go4);

                // Lampe au fond du gardin
                GameObject go2 = new GameObject(go);
                m.M41 = 1812.812f;
                m.M42 = 146.6327f;
                m.M43 = 76.60424f;
                go2.Transformation = m;
                returnResult.Add(go2);
                GameObject go5 = new GameObject(go);
                m.M41 = 1811.82f;
                m.M42 = 145.952f;
                m.M43 = 76.60423f;
                go5.Transformation = m;
                returnResult.Add(go5);
                GameObject go6 = new GameObject(go);
                m.M41 = 1810.82f;
                m.M42 = 145.252f;
                m.M43 = 76.60423f;
                go6.Transformation = m;
                returnResult.Add(go6);

                // Puit
                GameObject go3 = new GameObject(go);
                go3.Model = 6404; // Seau de bonbons
                m.M11 = 0.0707997f;
                m.M21 = 0.997491f;
                m.M41 = 1810.76f;
                m.M12 = -0.997491f;
                m.M22 = 0.0707997f;
                m.M42 = 156.80f;
                m.M43 = 76.60424f;
                go3.Transformation = m;
                returnResult.Add(go3);
            }

            // Now horde part
            if (mapId == 1330 || mapId == 1153)
            {
                GameObject go = new GameObject();
                //go.Id = 230468; // Garrison Building Horde Mine V3 or V2
                go.Model = mapId == 1153 ? 18569 : 18568;
                Matrix m = new Matrix();
                m.M11 = 0.662621f;
                m.M21 = -0.748954f;
                m.M31 = 0f;
                m.M41 = 5399.83f;
                m.M12 = 0.748954f;
                m.M22 = 0.662621f;
                m.M32 = 0f;
                m.M42 = 4465.98f;
                m.M13 = 0f;
                m.M23 = 0f;
                m.M33 = 1f;
                m.M43 = 114.54f;
                m.M14 = 0f;
                m.M24 = 0f;
                m.M34 = 0f;
                m.M44 = 1f;
                go.Transformation = m;
                returnResult.Add(go);
            }
            if (mapId == 1330 || mapId == 1153)
            {
                GameObject go = new GameObject();
                //go.Id = 236449; // Garrison Building Horde Farm V3
                go.Model = 21880;
                Matrix m = new Matrix();
                m.M11 = -0.707104f;
                m.M21 = -0.70711f;
                m.M31 = 0f;
                m.M41 = 5415.37f;
                m.M12 = 0.70711f;
                m.M22 = -0.707104f;
                m.M32 = 0f;
                m.M42 = 4586.44f;
                m.M13 = 0f;
                m.M23 = 0f;
                m.M33 = 1f;
                m.M43 = 136.583f;
                m.M14 = 0f;
                m.M24 = 0f;
                m.M34 = 0f;
                m.M44 = 1f;
                go.Transformation = m;
                returnResult.Add(go);
            }
            if (mapId == 1330 || mapId == 1153)
            {
                GameObject go = new GameObject();
                //go.Id = 230990; // Garrison Building Horde Fishing V3
                go.Model = 16436;
                Matrix m = new Matrix();
                m.M11 = 0.17365f;
                m.M21 = 0.984807f;
                m.M31 = 0f;
                m.M41 = 5476.59f;
                m.M12 = -0.984807f;
                m.M22 = 0.17365f;
                m.M32 = 0f;
                m.M42 = 4622.71f;
                m.M13 = 0f;
                m.M23 = 0f;
                m.M33 = 1f;
                m.M43 = 134.45f;
                m.M14 = 0f;
                m.M24 = 0f;
                m.M34 = 0f;
                m.M44 = 1f;
                go.Transformation = m;
                returnResult.Add(go);
            }
            if (mapId == 1153 || mapId == 1330 || mapId == 1152) // Big slot in garrison all levels
            {
                GameObject go = new GameObject();
                //go.Id = 230414; // Garrison Building Horde Barracks V3
                go.Model = 18560;
                Matrix m = new Matrix();
                m.M11 = 0.587786f;
                m.M21 = -0.809016f;
                m.M31 = 0f;
                m.M41 = 5575.18f;
                m.M12 = 0.809016f;
                m.M22 = 0.587786f;
                m.M32 = 0f;
                m.M42 = 4459.67f;
                m.M13 = 0f;
                m.M23 = 0f;
                m.M33 = 1f;
                m.M43 = 130.368f;
                m.M14 = 0f;
                m.M24 = 0f;
                m.M34 = 0f;
                m.M44 = 1f;
                go.Transformation = m;
                returnResult.Add(go);
            }
            if (mapId == 1153) // 2nd Big slot in garrison level 3
            {
                GameObject go = new GameObject();
                //go.Id = 230414; // Garrison Building Horde Barracks V3
                go.Model = 18560;
                Matrix m = new Matrix();
                m.M11 = -0.317305f;
                m.M21 = -0.948324f;
                m.M31 = 0f;
                m.M41 = 5651.28f;
                m.M12 = 0.948324f;
                m.M22 = -0.317305f;
                m.M32 = 0f;
                m.M42 = 4441.27f;
                m.M13 = 0f;
                m.M23 = 0f;
                m.M33 = 1f;
                m.M43 = 130.525f;
                m.M14 = 0f;
                m.M24 = 0f;
                m.M34 = 0f;
                m.M44 = 1f;
                go.Transformation = m;
                returnResult.Add(go);
            }
            if (mapId == 1153) // Medium slot in garrison level 3 only
            {
                GameObject go = new GameObject();
                //go.Id = 230418; // Garrison Building Horde Inn V3
                go.Model = 18563;
                Matrix m = new Matrix();
                m.M11 = -0.688353f;
                m.M21 = -0.725376f;
                m.M31 = 0f;
                m.M41 = 5693.36f;
                m.M12 = 0.725376f;
                m.M22 = -0.688353f;
                m.M32 = 0f;
                m.M42 = 4471.9f;
                m.M13 = 0f;
                m.M23 = 0f;
                m.M33 = 1f;
                m.M43 = 130.525f;
                m.M14 = 0f;
                m.M24 = 0f;
                m.M34 = 0f;
                m.M44 = 1f;
                go.Transformation = m;
                returnResult.Add(go);
            }
            if (mapId == 1153 || mapId == 1330) // Medium slot in garrison level 3 and 2 ????
            {
                GameObject go = new GameObject();
                //go.Id = 230418; // Garrison Building Horde Inn V3
                go.Model = 18563;
                Matrix m = new Matrix();
                m.M11 = 0.999048f;
                m.M21 = 0.0436183f;
                m.M31 = 0f;
                m.M41 = 5525.88f;
                m.M12 = -0.0436183f;
                m.M22 = 0.999048f;
                m.M32 = 0f;
                m.M42 = 4523.57f;
                m.M13 = 0f;
                m.M23 = 0f;
                m.M33 = 1f;
                m.M43 = 131.718f;
                m.M14 = 0f;
                m.M24 = 0f;
                m.M34 = 0f;
                m.M44 = 1f;
                go.Transformation = m;
                returnResult.Add(go);
            }
            if (mapId == 1153) // Small slot level 3 only
            {
                GameObject go = new GameObject();
                //go.Id = 230442; // Garrison Building Horde Salvage Yard V3
                go.Model = 19355;
                Matrix m = new Matrix();
                m.M11 = 0.694659f;
                m.M21 = -0.719339f;
                m.M31 = 0f;
                m.M41 = 5617.8f;
                m.M12 = 0.719339f;
                m.M22 = 0.694659f;
                m.M32 = 0f;
                m.M42 = 4510.28f;
                m.M13 = 0f;
                m.M23 = 0f;
                m.M33 = 1f;
                m.M43 = 119.27f + 2.0f;
                m.M14 = 0f;
                m.M24 = 0f;
                m.M34 = 0f;
                m.M44 = 1f;
                go.Transformation = m;
                returnResult.Add(go);
                GameObject go2 = new GameObject(go);
                Quaternion simpleRotation = new Quaternion(0, 0, 1, 0); // Z rotation
                Matrix m2 = Matrix.RotationQuaternion(simpleRotation);
                go2.Transformation = m2 * m;
                returnResult.Add(go2);
            }
            if (mapId == 1153 || mapId == 1330) // Small slot level 2 and level 3
            {
                GameObject go = new GameObject();
                //go.Id = 230442; // Garrison Building Horde Salvage Yard V3
                go.Model = 19355;
                Matrix m = new Matrix();
                m.M11 = -0.92388f;
                m.M21 = 0.382683f;
                m.M31 = 0f;
                m.M41 = 5665.69f;
                m.M12 = -0.382683f;
                m.M22 = -0.92388f;
                m.M32 = 0f;
                m.M42 = 4549.27f;
                m.M13 = 0f;
                m.M23 = 0f;
                m.M33 = 1f;
                m.M43 = 119.27f + 2.0f;
                m.M14 = 0f;
                m.M24 = 0f;
                m.M34 = 0f;
                m.M44 = 1f;
                go.Transformation = m;
                returnResult.Add(go);
                GameObject go2 = new GameObject(go);
                Quaternion simpleRotation = new Quaternion(0, 0, 1, 0); // Z rotation
                Matrix m2 = Matrix.RotationQuaternion(simpleRotation);
                go2.Transformation = m2 * m;
                returnResult.Add(go2);
            }
            if (mapId == 1153 || mapId == 1330 || mapId == 1152) // Small slot all levels
            {
                GameObject go = new GameObject();
                //go.Id = 230442; // Garrison Building Horde Salvage Yard V3
                go.Model = 19355;
                Matrix m = new Matrix();
                m.M11 = -0.45399f;
                m.M21 = -0.891007f;
                m.M31 = 0f;
                m.M41 = 5645.12f;
                m.M12 = 0.891007f;
                m.M22 = -0.45399f;
                m.M32 = 0f;
                m.M42 = 4508.96f;
                m.M13 = 0f;
                m.M23 = 0f;
                m.M33 = 1f;
                m.M43 = 119.27f + 2.0f;
                m.M14 = 0f;
                m.M24 = 0f;
                m.M34 = 0f;
                m.M44 = 1f;
                go.Transformation = m;
                returnResult.Add(go);
                GameObject go2 = new GameObject(go);
                Quaternion simpleRotation = new Quaternion(0, 0, 1, 0); // Z rotation
                Matrix m2 = Matrix.RotationQuaternion(simpleRotation);
                go2.Transformation = m2 * m;
                returnResult.Add(go2);
            }
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
                    //go.Id = result.GetInt32(0);
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
