using System.Collections.Generic;
using System.Globalization;
using System.IO;
using meshReader.Game;
using meshReader.Game.ADT;
using meshReader.Game.WMO;
using meshDatabase.Database;
using System.Linq;
using SlimDX;
using RecastLayer;

namespace meshBuilder
{
    public class Geometry
    {
        public List<Vector3> Vertices { get; private set; }
        public List<Triangle<uint>> Triangles { get; private set; }

        public bool Transform { get; set; }

        public Geometry()
        {
            Vertices = new List<Vector3>(10000);
            Triangles = new List<Triangle<uint>>(10000);
        }

        public void CalculateBoundingBox(out float[] min, out float[] max)
        {
            min = new[] {float.MaxValue, float.MaxValue, float.MaxValue};
            max = new[] {float.MinValue, float.MinValue, float.MinValue};

            foreach (var vert in Vertices)
            {
                if (vert.X > max[0])
                    max[0] = vert.X;
                if (vert.X < min[0])
                    min[0] = vert.X;

                if (vert.Y > max[1])
                    max[1] = vert.Y;
                if (vert.Y < min[1])
                    min[1] = vert.Y;

                if (vert.Z > max[2])
                    max[2] = vert.Z;
                if (vert.Z < min[2])
                    min[2] = vert.Z;
            }
        }

        public void CalculateMinMaxHeight(out float min, out float max, float[] bbMin, float[] bbMax)
        {
            min = float.MaxValue;
            max = float.MinValue;

            foreach (var vert in Vertices)
            {
                if (Transform)
                {
                    if (vert.Y < min && vert.X >= bbMin[0] && vert.X <= bbMax[0] && vert.Z >= bbMin[2] && vert.Z <= bbMax[2])
                        min = vert.Y;
                    if (vert.Y > max && vert.X >= bbMin[0] && vert.X <= bbMax[0] && vert.Z >= bbMin[2] && vert.Z <= bbMax[2])
                        max = vert.Y;
                }
                else
                {
                    if (vert.Z < min && vert.X >= bbMin[0] && vert.X <= bbMax[0] && vert.Y >= bbMin[1] && vert.Y <= bbMax[1])
                        min = vert.Z;
                    if (vert.Z > max && vert.X >= bbMin[0] && vert.X <= bbMax[0] && vert.Y >= bbMin[1] && vert.Y <= bbMax[1])
                        max = vert.Z;
                }
            }
        }

        public void CreateDemoDump(string path)
        {
            using (var stream = new StreamWriter(path, false))
            {
                foreach (var vert in Vertices)
                {
                    stream.WriteLine("v " + vert.X.ToString(CultureInfo.InvariantCulture) + " " +
                                     vert.Y.ToString(CultureInfo.InvariantCulture) + " " +
                                     vert.Z.ToString(CultureInfo.InvariantCulture));
                }

                foreach (var tri in Triangles)
                {
                    stream.WriteLine("f " + (tri.V0 + 1) + " " + (tri.V1 + 1) + " " + (tri.V2 + 1));
                }
            }
        }

        public void AddData(IEnumerable<Vector3> verts, IEnumerable<Triangle<uint>> tris)
        {
            var vertOffset = (uint) Vertices.Count;
            foreach (var vert in verts)
            {
                Vertices.Add(Transform ? vert.ToRecast() : vert);
            }

            foreach (var tri in tris)
                Triangles.Add(new Triangle<uint>(tri.Type, tri.V0 + vertOffset, tri.V1 + vertOffset, tri.V2 + vertOffset));
        }

        public void AddDungeon(WorldModelRoot model, WorldModelHandler.WorldModelDefinition def)
        {
            var verts = new List<Vector3>();
            var tris = new List<Triangle<uint>>();
            WorldModelHandler.InsertModelGeometry(verts, tris, def, model);
            AddData(verts, tris);
        }

        public void AddGameObject(GameObject go)
        {
            var verts = new List<Vector3>();
            var tris = new List<Triangle<uint>>();
            WorldModelHandler.InsertGameObjectModelGeometry(verts, tris, GameObjectHelper.GetFullFileNameFromDisplayId(go.Model), go.Transformation /*, go.Rotations*/);
            AddData(verts, tris);
        }

        public void AddAllianceGunShip()
        {
            var verts = new List<Vector3>();
            var tris = new List<Triangle<uint>>();
            WorldModelHandler.GetAllianceGunshipModelGeometry(verts, tris, GameObjectHelper.GetFullFileNameFromDisplayId(8254));
            AddData(verts, tris);
        }

        public void AddAdt(ADT data)
        {
            foreach (var mc in data.MapChunks)
                AddData(mc.Vertices, mc.Triangles.Select(t => new Triangle<uint>(t.Type, t.V0, t.V1, t.V2)));

            if (data.DoodadHandler.Triangles != null)
                AddData(data.DoodadHandler.Vertices, data.DoodadHandler.Triangles);

            // terrain under water is marked now, so we can safely ignore the actual surface
            //if (data.LiquidHandler.Triangles != null)
            //    AddData(data.LiquidHandler.Vertices, data.LiquidHandler.Triangles);

            if (data.WorldModelHandler.Triangles != null)
                AddData(data.WorldModelHandler.Vertices, data.WorldModelHandler.Triangles);
        }

        public void GetRawData(out float[] vertices, out int[] triangles, out byte[] areas)
        {
            vertices = new float[Vertices.Count * 3];
            for (int i = 0; i < Vertices.Count; i++)
            {
                var vert = Vertices[i];
                vertices[(i * 3) + 0] = vert.X;
                vertices[(i * 3) + 1] = vert.Y;
                vertices[(i * 3) + 2] = vert.Z;
            }
            triangles = new int[Triangles.Count * 3];
            for (int i = 0; i < Triangles.Count; i++)
            {
                var tri = Triangles[i];
                triangles[(i * 3) + 0] = (int) tri.V0;
                triangles[(i * 3) + 1] = (int) tri.V1;
                triangles[(i * 3) + 2] = (int) tri.V2;
            }
            areas = new byte[Triangles.Count];
            for (int i = 0; i < Triangles.Count; i++)
            {
                switch (Triangles[i].Type)
                {
                    case TriangleType.Water:
                        areas[i] = (byte) PolyArea.Water;
                        break;

                    default:
                        areas[i] = (byte) PolyArea.Terrain;
                        break;
                }
            }
        }
    }
}