using System;
using meshPather;
using SlimDX;
using NUnit.Framework;

namespace meshReaderTest
{
    public abstract class MeshTest
    {
        protected Pather Pather;

        protected void Initialize(string continent)
        {
            Pather = new Pather(continent);
            Assert.IsFalse(Pather.IsDungeon);
        }

        protected double TryHugePath(Vector3 start, Vector3 end, out System.Collections.Generic.List<Hop> path)
        {
            TryPath(start, end, out path, true);
            if (path == null || path.Count == 0)
            {
                path.Add(new Hop {Type = HopType.Waypoint, Location = start});
                return 0;
            }
            float diff = (end - path[path.Count - 1].Location).Length();
            float ndiff = 5f;
            while (ndiff < diff)
            {
                int limit = (int) (path.Count * 0.80f);

                System.Collections.Generic.List<Hop> path2;
                TryPath(path[limit].Location, end, out path2, true);

                ndiff = (end - path2[path2.Count - 1].Location).Length();
                if (ndiff < diff)
                {
                    for (int j = path.Count - 1; j > limit; j--)
                    {
                        path.RemoveAt(j);
                    }
                    foreach (Hop p in path2)
                        path.Add(p);
                    diff = ndiff;
                    ndiff = 5f;
                }
            }
            double length = 0;
            for (int i = 0; i < path.Count - 1; i++)
                length += (path[i].Location - path[i + 1].Location).Length();
            return length;
        }

        protected double TryPath(Vector3 start, Vector3 end)
        {
            System.Collections.Generic.List<Hop> discard;
            return TryPath(start, end, out discard, false);
        }

        protected double TryPath(Vector3 start, Vector3 end, out System.Collections.Generic.List<Hop> hops, bool acceptIncomplete)
        {
            //Pather.LoadAllTiles();
            var result = Pather.FindPath(start, end);
            if (result == null || result.Count == 0)
            {
                hops = new System.Collections.Generic.List<Hop>();
                return 0;
            }
            //Assert.IsNotNull(result);
            //Assert.Greater(result.Count, 0);

            // make sure we didn't get an incomplete path
            if (!acceptIncomplete)
                Assert.Less((end - result[result.Count - 1].Location).Length(), 5f);

            double length = 0;
            for (int i = 0; i < result.Count - 1; i++)
                length += (result[i].Location - result[i + 1].Location).Length();

            Console.WriteLine("Total distance flying : " + (end - start).Length() + ", walking : " + length);
            Console.WriteLine("Distance to end : " + (end - result[result.Count - 1].Location).Length());
/*            foreach (var hop in result)
            {
                float tx, ty;
                Pather.GetTileByLocation(hop.Location.ToRecast().ToFloatArray(), out tx, out ty);
                Console.WriteLine("TX: " + tx + " TY: " + ty + " X: " + hop.Location.X + " Y: " + hop.Location.Y + " Z: " + hop.Location.Z);
            }*/
            Console.WriteLine("Number of hops : " + result.Count);
            Console.WriteLine("Memory: " + (Pather.MemoryPressure / 1024 / 1024) + "MB");

            hops = result;
            return length;
        }
    }
}