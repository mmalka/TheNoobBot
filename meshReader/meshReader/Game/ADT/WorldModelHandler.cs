using System.Collections.Generic;
using System.IO;
using System.Linq;
using meshReader.Game.Caching;
using meshReader.Game.MDX;
using meshReader.Game.WMO;
using meshReader.Helper;
using SlimDX;
using SlimMath;

namespace meshReader.Game.ADT
{
    
    public class WorldModelHandler : ObjectDataHandler
    {
        private readonly HashSet<uint> _drawn = new HashSet<uint>();
        private List<WorldModelDefinition> _definitions;
        private List<string> _paths;

        public List<Vector3> Vertices { get; private set; }
        public List<Triangle<uint>> Triangles { get; private set; }
        
        public WorldModelHandler(ADT source)
            : base(source)
        {
            if (!source.HasObjectData)
                return;

            ReadModelPaths();
            ReadDefinitions();
        }

        private bool IsSane
        {
            get
            {
                return _definitions != null && _paths != null;
            }
        }

        protected override void ProcessInternal(ChunkedData subChunks)
        {
            if (!IsSane)
                return;

            var wmoReferencesChunk = subChunks.GetChunkByName("MCRW");
            if (wmoReferencesChunk == null)
                return;
            var stream = wmoReferencesChunk.GetStream();
            if (stream.Length < wmoReferencesChunk.Offset + wmoReferencesChunk.Length)
                return;
            var reader = new BinaryReader(stream);
            var refCount = (int)(wmoReferencesChunk.Length / 4);
            for (int i = 0; i < refCount; i++)
            {
                int index = reader.ReadInt32();
                if (index < 0 || index >= _definitions.Count)
                    continue;

                var wmo = _definitions[index];

                if (_drawn.Contains(wmo.UniqueId))
                    continue;
                _drawn.Add(wmo.UniqueId);

                if (wmo.MwidIndex >= _paths.Count)
                    continue;

                var path = _paths[(int) wmo.MwidIndex];
                var model = Cache.WorldModel.Get(path);
                if (model == null)
                {
                    try
                    {
                        model = new WorldModelRoot(path);
                    }
                    catch { continue; }
                    Cache.WorldModel.Insert(path, model);
                }

                if (Vertices == null)
                    Vertices = new List<Vector3>(1000);
                if (Triangles == null)
                    Triangles = new List<Triangle<uint>>(1000);

                InsertModelGeometry(Vertices, Triangles, wmo, model);
            }
        }

        public static void InsertGameObjectModelGeometry(List<Vector3> vertices, List<Triangle<uint>> triangles, string m2path, Matrix transformation)
        {
            if (m2path.EndsWith(".WMO", System.StringComparison.InvariantCultureIgnoreCase))
            {
                var wmo = Cache.WorldModel.Get(m2path);
                if (wmo == null)
                {
                    try
                    {
                        wmo = new WorldModelRoot(m2path);
                    }
                    catch { }
                    Cache.WorldModel.Insert(m2path, wmo);
                    System.Console.WriteLine(m2path);
                    foreach (var group in wmo.Groups)
                    {
                        if ((group.Flags & 0x80) != 0) // Here is a group of triangles that mess-up the WMO
                            continue;
                        int vertOffset = vertices.Count;
                        if (group.Triangles != null)
                        {
                            bool one = false;
                            for (int i = 0; i < group.Triangles.Length; i++)
                            {
                                // only include collidable triangles
                                if ((group.TriangleFlags[i] & 0x04) == 0 && group.TriangleMaterials[i] != 0xFF)
                                    continue;
                                var tri = group.Triangles[i];
                                triangles.Add(new Triangle<uint>(TriangleType.Wmo, (uint)(tri.V0 + vertOffset),
                                                                 (uint)(tri.V1 + vertOffset),
                                                                 (uint)(tri.V2 + vertOffset)));
                                one = true;
                            }
                            if (one)
                                vertices.AddRange(group.Vertices.Select(vert => Vector3.TransformCoordinate(vert, transformation)));
                        }
                    }

                }
            }
            else // .M2
            {
                var model = Cache.Model.Get(m2path);
                if (model == null)
                {
                    model = new Model(m2path);
                    Cache.Model.Insert(m2path, model);
                }
                System.Console.WriteLine(m2path);
                if (!model.IsCollidable)
                    return;

                int vertOffset = vertices.Count;
                vertices.AddRange(model.Vertices.Select(vert => Vector3.TransformCoordinate(vert, transformation)));
                foreach (var tri in model.Triangles)
                    triangles.Add(new Triangle<uint>(TriangleType.Doodad, (uint)(tri.V0 + vertOffset),
                                                     (uint)(tri.V1 + vertOffset), (uint)(tri.V2 + vertOffset)));
            }
        }

        public static void InsertModelGeometry(List<Vector3> vertices, List<Triangle<uint>> triangles, WorldModelDefinition def, WorldModelRoot root)
        {
            var transformation = Transformation.GetTransformation(def);

            foreach (var group in root.Groups)
            {
                if ((group.Flags & 0x80) != 0) // Here is a group of triangles that mess-up the WMO
                    continue;
                int vertOffset = vertices.Count;
                if (group.Triangles != null)
                {
                    bool one = false;
                    for (int i = 0; i < group.Triangles.Length; i++)
                    {
                        // only include collidable tris
                        if ((group.TriangleFlags[i] & 0x04) != 0 && group.TriangleMaterials[i] != 0xFF)
                            continue;
                        var tri = group.Triangles[i];
                        triangles.Add(new Triangle<uint>(TriangleType.Wmo, (uint)(tri.V0 + vertOffset),
                                                         (uint)(tri.V1 + vertOffset),
                                                         (uint)(tri.V2 + vertOffset)));
                        one = true;
                    }
                    if (one)
                        vertices.AddRange(group.Vertices.Select(vert => Vector3.TransformCoordinate(vert, transformation)));
                }
            }

            if (def.DoodadSet >= 0 && def.DoodadSet < root.DoodadSets.Count)
            {
                var set = root.DoodadSets[def.DoodadSet];
                var instances = new List<DoodadInstance>((int)set.CountInstances);
                for (uint i = set.FirstInstanceIndex; i < (set.CountInstances + set.FirstInstanceIndex); i++)
                {
                    if (i >= root.DoodadInstances.Count)
                        break;
                    instances.Add(root.DoodadInstances[(int)i]);
                }
                
                foreach (var instance in instances)
                {
                    var model = Cache.Model.Get(instance.File);
                    if (model == null)
                    {
                        model = new Model(instance.File);
                        Cache.Model.Insert(instance.File, model);
                    }

                    if (!model.IsCollidable)
                        continue;
                    var doodadTransformation = Transformation.GetWmoDoodadTransformation(instance, def);
                    int vertOffset = vertices.Count;
                    vertices.AddRange(model.Vertices.Select(vert => Vector3.TransformCoordinate(vert, doodadTransformation)));
                    foreach (var tri in model.Triangles)
                        triangles.Add(new Triangle<uint>(TriangleType.Wmo, (uint) (tri.V0 + vertOffset),
                                                         (uint) (tri.V1 + vertOffset), (uint) (tri.V2 + vertOffset)));
                }
            }

            foreach (var group in root.Groups)
            {
                if (!group.HasLiquidData)
                    continue;

                for (int y = 0; y < group.LiquidDataHeader.Height; y++)
                {
                    for (int x = 0; x < group.LiquidDataHeader.Width; x++)
                    {
                        if (!group.LiquidDataGeometry.ShouldRender(x, y))
                            continue;

                        var vertOffset = (uint)vertices.Count;
                        vertices.Add(GetLiquidVert(transformation, group.LiquidDataHeader.BaseLocation,
                                                   group.LiquidDataGeometry.HeightMap[x, y], x, y));
                        vertices.Add(GetLiquidVert(transformation, group.LiquidDataHeader.BaseLocation,
                                                   group.LiquidDataGeometry.HeightMap[x + 1, y], x + 1, y));
                        vertices.Add(GetLiquidVert(transformation, group.LiquidDataHeader.BaseLocation,
                                                   group.LiquidDataGeometry.HeightMap[x, y + 1], x, y + 1));
                        vertices.Add(GetLiquidVert(transformation, group.LiquidDataHeader.BaseLocation,
                                                   group.LiquidDataGeometry.HeightMap[x + 1, y + 1], x + 1, y + 1));

                        triangles.Add(new Triangle<uint>(TriangleType.Water, vertOffset, vertOffset + 2, vertOffset + 1));
                        triangles.Add(new Triangle<uint>(TriangleType.Water, vertOffset + 2, vertOffset + 3,
                                                         vertOffset + 1));

                    }
                }
            }
        }

        private static Vector3 GetLiquidVert(Matrix transformation, Vector3 basePosition, float height, int x, int y)
        {
            if (System.Math.Abs(height) > 0.5f)
                basePosition.Z = 0.0f;

            return Vector3.TransformCoordinate(basePosition + new Vector3(x * Constant.UnitSize, y * Constant.UnitSize, height),
                                     transformation);
        }

        private void ReadDefinitions()
        {
            var chunk = Source.ObjectData.GetChunkByName("MODF");
            if (chunk == null)
                return;

            const int definitionSize = 64;
            var definitionCount = (int) (chunk.Length/definitionSize);
            _definitions = new List<WorldModelDefinition>(definitionCount);
            var stream = chunk.GetStream();
            for (int i = 0; i < definitionCount; i++)
                _definitions.Add(WorldModelDefinition.Read(stream));
        }

        // TODO: this is so fucking idiotic because data and id share the same stream. also stolen from DoodadHandler - need better synergy
        private void ReadModelPaths()
        {
            var mwid0 = Source.ObjectData.GetChunkByName("MWID");
            var mwmo0 = Source.ObjectData.GetChunkByName("MWMO");
            if (!(mwid0 == null || mwmo0 == null))
            {
                var paths = (int)(mwid0.Length / 4);
                _paths = new List<string>(paths);
                for (int i = 0; i < paths; i++)
                {
                    var r = new BinaryReader(mwid0.GetStream());
                    r.BaseStream.Seek(i * 4, SeekOrigin.Current);
                    uint offset = r.ReadUInt32();
                    var dataStream = mwmo0.GetStream();
                    dataStream.Seek(offset + mwmo0.Offset, SeekOrigin.Begin);
                    _paths.Add(dataStream.ReadCString());
                }
            }
        }

        public class WorldModelDefinition : Transformation.IDefinition
        {
            public uint MwidIndex;
            public uint UniqueId;
            public Vector3 Position { get; private set; }
            public Vector3 Rotation { get; private set; }
            public Vector3 UpperExtents;
            public Vector3 LowerExtents;
            public ushort Flags;
            public ushort DoodadSet;
            public ushort NameSet;

            public float Scale { get { return 1.0f; } }

            public static WorldModelDefinition Read(Stream s)
            {
                var r = new BinaryReader(s);
                var ret = new WorldModelDefinition
                              {
                                  MwidIndex = r.ReadUInt32(),
                                  UniqueId = r.ReadUInt32(),
                                  Position = Vector3Helper.Read(s),
                                  Rotation = Vector3Helper.Read(s),
                                  UpperExtents = Vector3Helper.Read(s),
                                  LowerExtents = Vector3Helper.Read(s),
                                  Flags = r.ReadUInt16(),
                                  DoodadSet = r.ReadUInt16(),
                                  NameSet = r.ReadUInt16()
                              };
                // discard some padding
                r.ReadUInt16();
                //r.ReadUInt32();
                return ret;
            }
        }

    }

}