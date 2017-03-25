using System.Collections.Generic;
using System.IO;
using meshReader.Game.Caching;
using meshReader.Game.MDX;
using meshReader.Helper;
using SlimDX;

namespace meshReader.Game.ADT
{
    public class DoodadHandler : ObjectDataHandler
    {
        private readonly HashSet<uint> _drawn = new HashSet<uint>();
        private List<DoodadDefinition> _definitions;
        private List<string> _paths;

        public List<Vector3> Vertices { get; private set; }
        public List<Triangle<uint>> Triangles { get; private set; }

        public DoodadHandler(ADT adt)
            : base(adt)
        {
            if (!adt.HasObjectData)
                return;

            var mddf = adt.ObjectData.GetChunkByName("MDDF");
            if (mddf != null)
                ReadDoodadDefinitions(mddf);

            var mmid = adt.ObjectData.GetChunkByName("MMID");
            var mmdx = adt.ObjectData.GetChunkByName("MMDX");
            if (mmid != null && mmdx != null)
                ReadDoodadPaths(mmid, mmdx);
        }

        private bool IsSane
        {
            get { return _definitions != null && _paths != null; }
        }

        protected override void ProcessInternal(ChunkedData subChunks)
        {
            if (!IsSane)
                return;

            var doodadReferencesChunk = subChunks.GetChunkByName("MCRD");
            if (doodadReferencesChunk == null)
                return;
            var stream = doodadReferencesChunk.GetStream();
            var reader = new BinaryReader(stream);
            var refCount = (int) (doodadReferencesChunk.Length / 4);

            for (int i = 0; i < refCount; i++)
            {
                int index = reader.ReadInt32();
                if (index < 0 || index >= _definitions.Count)
                    break; //continue;

                var doodad = _definitions[index];

                if (_drawn.Contains(doodad.UniqueId))
                    continue;
                _drawn.Add(doodad.UniqueId);

                if (doodad.MmidIndex >= _paths.Count)
                    continue;

                var path = _paths[(int) doodad.MmidIndex];
                var model = Cache.Model.Get(path);
                if (model == null)
                {
                    model = new Model(path);
                    Cache.Model.Insert(path, model);
                }

                if (!model.IsCollidable)
                    continue;

                // some weak heuristic to save memory allocation time
                if (Vertices == null)
                    Vertices = new List<Vector3>((int) (refCount * model.Vertices.Length * 0.2));
                if (Triangles == null)
                    Triangles = new List<Triangle<uint>>((int) (refCount * model.Triangles.Length * 0.2));

                InsertModelGeometry(doodad, model);
            }
        }

        private void InsertModelGeometry(DoodadDefinition def, Model model)
        {
            var transformation = Transformation.GetTransformation(def);
            var vertOffset = (uint) Vertices.Count;
            foreach (var vert in model.Vertices)
                Vertices.Add(Vector3.TransformCoordinate(vert, transformation));
            foreach (var tri in model.Triangles)
                Triangles.Add(new Triangle<uint>(TriangleType.Doodad, tri.V0 + vertOffset, tri.V1 + vertOffset, tri.V2 + vertOffset));
        }

        // TODO: this is so fucking idiotic because data and id share the same stream
        private void ReadDoodadPaths(Chunk id, Chunk data)
        {
            int paths = (int) id.Length / 4;
            _paths = new List<string>(paths);
            for (int i = 0; i < paths; i++)
            {
                var r = new BinaryReader(id.GetStream());
                r.BaseStream.Seek(i * 4, SeekOrigin.Current);
                uint offset = r.ReadUInt32();
                var dataStream = data.GetStream();
                dataStream.Seek(offset + data.Offset, SeekOrigin.Begin);
                _paths.Add(dataStream.ReadCString());
            }
        }

        public class DoodadDefinition : Transformation.IDefinition
        {
            public uint MmidIndex;
            public uint UniqueId;
            public Vector3 Position { get; private set; }
            public Vector3 Rotation { get; private set; }
            public ushort DecimalScale;
            public ushort Flags;

            public float Scale
            {
                get { return DecimalScale / 1024.0f; }
            }

            public void Read(Stream s)
            {
                var r = new BinaryReader(s);
                MmidIndex = r.ReadUInt32();
                UniqueId = r.ReadUInt32();
                Position = Vector3Helper.Read(s);
                Rotation = Vector3Helper.Read(s);
                DecimalScale = r.ReadUInt16();
                Flags = r.ReadUInt16();
            }
        }

        private void ReadDoodadDefinitions(Chunk c)
        {
            int count = (int) c.Length / 36;
            _definitions = new List<DoodadDefinition>(count);

            var stream = c.GetStream();
            for (int i = 0; i < count; i++)
            {
                var def = new DoodadDefinition();
                def.Read(stream);
                _definitions.Add(def);
            }
        }
    }
}