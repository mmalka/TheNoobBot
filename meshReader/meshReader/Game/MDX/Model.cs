using System;
using System.IO;
using meshDatabase;
using meshReader.Helper;
using SlimDX;

namespace meshReader.Game.MDX
{
    
    public class Model
    {
        private readonly Stream _stream;

        public ModelHeader Header { get; private set; }
        public Vector3[] Vertices { get; private set; }
        public Vector3[] Normals { get; private set; }
        public Triangle<ushort>[] Triangles { get; private set; }

        public bool IsCollidable { get; private set;}

        public Model(string path)
        {
            _stream = MpqManager.GetFile(FixModelPath(path));
            //if (_stream == null) // required for file list
            //    return;
            Header = new ModelHeader();
            Header.Read(_stream);

            if (Header.OffsetBoundingNormals > 0 && Header.OffsetBoundingVertices > 0 &&
                Header.OffsetBoundingTriangles > 0 && Header.BoundingRadius > 0.0f)
            {
                IsCollidable = true;
                ReadVertices(_stream);
                ReadBoundingNormals(_stream);
                ReadBoundingTriangles(_stream);
            }
        }

        private static string FixModelPath(string path)
        {
            if (path.EndsWith(".M2", StringComparison.InvariantCultureIgnoreCase))
                return path;

            return Path.ChangeExtension(path, ".M2");
        }

        private void ReadVertices(Stream s)
        {
            s.Seek(Header.OffsetBoundingVertices, SeekOrigin.Begin);

            Vertices = new Vector3[Header.CountBoundingVertices];
            for (int i = 0; i < Header.CountBoundingVertices; i++)
            {
                Vertices[i] = Vector3Helper.Read(s);
            }
        }

        private void ReadBoundingTriangles(Stream s)
        {
            s.Seek(Header.OffsetBoundingTriangles, SeekOrigin.Begin);

            var r = new BinaryReader(s);
            Triangles = new Triangle<ushort>[Header.CountBoundingTriangles/3];
            for (int i = 0; i < Header.CountBoundingTriangles/3; i++)
                Triangles[i] = new Triangle<ushort>(TriangleType.Doodad, r.ReadUInt16(), r.ReadUInt16(), r.ReadUInt16());
        }

        private void ReadBoundingNormals(Stream s)
        {
            s.Seek(Header.OffsetBoundingNormals, SeekOrigin.Begin);

            Normals = new Vector3[Header.CountBoundingNormals];
            for (int i = 0; i < Header.CountBoundingNormals; i++)
            {
                Normals[i] = Vector3Helper.Read(s);
            }
        }
    }

}