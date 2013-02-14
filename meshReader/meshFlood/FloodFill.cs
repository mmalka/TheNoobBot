using System;
using System.Collections.Generic;
using DetourLayer;
using meshPather;
using Microsoft.Xna.Framework;

namespace meshFlood
{
    
    public class FloodFill
    {
        private readonly NavMeshQuery _query;
        private readonly QueryFilter _filter;
        private readonly Queue<uint> _open;

        public int Marked { get; private set; }
        public NavMesh Mesh { get; private set; }
        public bool[] Visited { get; private set; }
        public int VisitedMask { get; private set; }

        public FloodFill(NavMesh mesh)
        {
            Mesh = mesh;
            
            _query = new NavMeshQuery();
            if (!_query.Initialize(Mesh, 1024).HasSucceeded())
                throw new Exception("Failed to initialize navigation mesh query");

            _filter = new QueryFilter();

            var param = Mesh.Parameter;
            // detour guarantees these are 2^x
            var tileBits = (int) Math.Log(param.MaxTiles, 2);
            var polyBits = (int) Math.Log(param.MaxPolygons, 2);
            // we also don't care about salt, so create a mask to cut these off just in case
            Visited = new bool[1 << (tileBits + polyBits)];
            VisitedMask = (1 << (tileBits + polyBits)) - 1;

            _open = new Queue<uint>(100);
        }

        public void ExecuteFrom(Vector3 source)
        {
            var poly = FindPoly(source);
            if (poly == 0)
            {
                Console.WriteLine("Couldn't find polygon for " + source);
                return;
            }

            _open.Enqueue(poly);
            while (_open.Count > 0)
            {
                var p = _open.Dequeue();
                VisitPolygon(p);
            }
        }

        private uint FindPoly(Vector3 loc)
        {
            return _query.FindNearestPolygon(loc.ToFloatArray(), new[] {2.5f, 2.5f, 2.5f}, _filter);
        }

        private void VisitPolygon(uint polyRef)
        {
            if (IsVisited(polyRef))
                return;

            Poly poly;
            MeshTile tile;
            if (!Mesh.GetTileAndPolyByRef(polyRef, out tile, out poly).HasSucceeded())
                throw new Exception("Couldn't find polygon for reference " + polyRef);

            MarkVisited(polyRef);

            for (uint i = poly.FirstLink; i != Link.NullLink; i = tile.GetLink(i).Next)
            {
                var linkRef = tile.GetLink(i).Reference;
                if (IsVisited(linkRef))
                    continue;

                _open.Enqueue(linkRef);
            }
        }

        public bool IsVisited(uint polyRef)
        {
            return Visited[polyRef & VisitedMask];
        }

        private void MarkVisited(uint polyRef)
        {
            Visited[polyRef & VisitedMask] = true;
            Marked++;
        }

    }

}