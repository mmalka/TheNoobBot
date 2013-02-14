using System;
using meshPather;
using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace meshReaderTest
{
    
    public abstract class DungeonMeshTest
    {
        protected Pather Pather;

        protected void Initialize(string dungeon)
        {
            Pather = new Pather(dungeon);
            Assert.IsTrue(Pather.IsDungeon);
        }

        protected void TryPath(Vector3 start, Vector3 end)
        {
            var result = Pather.FindPath(start, end);
            Assert.IsNotNull(result);
            Assert.Greater(result.Count, 0);
            // make sure we didn't get an incomplete path
            Assert.Less((end - result[result.Count - 1].Location).Length(), 5f);

            foreach (var hop in result)
                Console.WriteLine("X: " + hop.Location.X + " Y: " + hop.Location.Y + " Z: " + hop.Location.Z);
            Console.WriteLine("Memory: " + (Pather.MemoryPressure / 1024) + "kB");
        }
    }

}