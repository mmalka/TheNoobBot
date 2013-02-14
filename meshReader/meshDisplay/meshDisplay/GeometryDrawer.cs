using System.Collections.Generic;
using meshReader.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace meshDisplay
{

    public struct VertexPositionColorNormal : IVertexType
    {
        public Vector3 Position;
        public Color Color;
        public Vector3 Normal;

        public VertexPositionColorNormal(Vector3 position, Color color, Vector3 normal)
        {
            Position = position;
            Color = color;
            Normal = normal;
        }

        public VertexDeclaration VertexDeclaration
        {
            get
            {
                return
                    new VertexDeclaration(
                        new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                        new VertexElement(12, VertexElementFormat.Color, VertexElementUsage.Color, 0),
                        new VertexElement(16, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0));
            }
        }
    }

    public class GeometryDrawer
    {
        private VertexBuffer _vertexBuffer;
        private IndexBuffer _indexBuffer;
        private Microsoft.Xna.Framework.Game _game;

        private static void GenerateNormals(VertexPositionColorNormal[] vertices, int[] indices)
        {
            for (int i = 0; i < indices.Length / 3; i++)
            {
                Vector3 firstvec = vertices[indices[i * 3 + 1]].Position - vertices[indices[i * 3]].Position;
                Vector3 secondvec = vertices[indices[i * 3]].Position - vertices[indices[i * 3 + 2]].Position;
                Vector3 normal = Vector3.Cross(firstvec, secondvec);
                normal.Normalize();
                vertices[indices[i * 3]].Normal += normal;
                vertices[indices[i * 3 + 1]].Normal += normal;
                vertices[indices[i * 3 + 2]].Normal += normal;
            }

            for (int i = 0; i < vertices.Length; i++)
                vertices[i].Normal.Normalize();
        }

        public void Initialize(Microsoft.Xna.Framework.Game game, Color color, IEnumerable<IEnumerable<Vector3>> verticeLists, IEnumerable<IEnumerable<Triangle<byte>>> triangleLists)
        {
            int listCount = verticeLists.Count();
            var vertCount = verticeLists.Sum(l => l.Count());
            var triCount = triangleLists.Sum(l => l.Count());
            var verts = new Vector3[vertCount];
            var tris = new Triangle<uint>[triCount];
            int c = 0;
            int tc = 0;
            for (int i = 0; i < listCount; i++)
            {
                var vertList = verticeLists.ElementAt(i);
                var triList = triangleLists.ElementAt(i);
                var vertOffset = (uint)c;

                foreach (var vert in vertList)
                    verts[c++] = vert;

                foreach (var tri in triList)
                {
                    tris[tc++] = new Triangle<uint>(tri.Type, tri.V0 + vertOffset, tri.V1 + vertOffset, tri.V2 + vertOffset);
                }
            }
            Initialize(game, color, verts, tris);
        }

        public void Initialize(Microsoft.Xna.Framework.Game game, Color color, IEnumerable<Vector3> vertices, IEnumerable<Triangle<uint>> triangles)
        {
            _game = game;
            int vertCount = vertices.Count();
            int triCount = triangles.Count();
            _vertexBuffer = new VertexBuffer(game.GraphicsDevice, typeof(VertexPositionColorNormal), vertCount, BufferUsage.None);
            _indexBuffer = new IndexBuffer(game.GraphicsDevice, typeof(int), triCount * 3, BufferUsage.None);

            var data = new VertexPositionColorNormal[vertCount];
            int c = 0;
            foreach (var vert in vertices)
                data[c++] = new VertexPositionColorNormal(new Vector3(vert.Y, vert.Z, vert.X), color, new Vector3(0));

            var triData = new int[triCount*3];
            c = 0;
            foreach (var tri in triangles)
            {
                triData[c] = (int)tri.V0;
                triData[c + 1] = (int) tri.V1;
                triData[c + 2] = (int) tri.V2;
                c += 3;

                switch (tri.Type)
                {
                    case TriangleType.Water:
                        data[tri.V0].Color = Color.Blue;
                        data[tri.V1].Color = Color.Blue;
                        data[tri.V2].Color = Color.Blue;
                        break;
                }
            }

            GenerateNormals(data, triData);
            _vertexBuffer.SetData(data);
            _indexBuffer.SetData(triData);
        }

        public void Initialize(Microsoft.Xna.Framework.Game game, Color color, float[] verts, int[] tris)
        {
            _game = game;
            _vertexBuffer = new VertexBuffer(game.GraphicsDevice, typeof(VertexPositionColorNormal), verts.Length / 3, BufferUsage.None);
            _indexBuffer = new IndexBuffer(game.GraphicsDevice, typeof(int), tris.Length, BufferUsage.None);
            var data = new VertexPositionColorNormal[verts.Length/3];
            for (int i = 0; i < verts.Length / 3; i++)
                data[i] = new VertexPositionColorNormal(new Vector3(-verts[(i*3) + 0], verts[(i*3)+1], -verts[(i*3)+2]), color, new Vector3(0));
            GenerateNormals(data, tris);
            _vertexBuffer.SetData(data);
            _indexBuffer.SetData(tris);
        }

        public void Draw()
        {
            _game.GraphicsDevice.SetVertexBuffer(_vertexBuffer);
            _game.GraphicsDevice.Indices = _indexBuffer;
            _game.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, _vertexBuffer.VertexCount, 0, _indexBuffer.IndexCount / 3);
        }

    }

}