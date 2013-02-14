using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace meshReaderTest
{

    [TestFixture]
    public class Expansion01MeshTest : MeshTest
    {

        [TestFixtureSetUp]
        public void Initialize()
        {
            Initialize("S:\\meshReader\\Meshes\\Expansion01");
        }

        [Test]
        public void CitySilvermoon01()
        {
            TryPath(new Vector3(9335.6f, -7278f, 13.6f), new Vector3(9492f, -7279.4f, 14.3f));
        }

        [Test]
        public void CitySilvermoon02()
        {
            TryPath(new Vector3(9335.6f, -7278f, 13.6f), new Vector3(9551.5f, -7305.7f, 15.2f));
        }

        [Test]
        public void CitySilvermoon03()
        {
            TryPath(new Vector3(9335.6f, -7278f, 13.6f), new Vector3(9469f, -7353f, 23.5f));
        }

        [Test]
        public void CitySilvermoon04()
        {
            TryPath(new Vector3(9335.6f, -7278f, 13.6f), new Vector3(9941.5f, -7067.2f, 47.7f));
        }
        
    }

}