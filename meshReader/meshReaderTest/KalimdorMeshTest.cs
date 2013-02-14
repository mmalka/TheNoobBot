using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace meshReaderTest
{

    [TestFixture]
    public class KalidmorMeshTest : MeshTest
    {

        [TestFixtureSetUp]
        public void Initialize()
        {
            Initialize("Kalimdor");
        }

        [Test]
        public void OrcStartToOrgrimmar()
        {
            TryPath(new Vector3(-604.2f, -4215.5f, 38.6f), new Vector3(1397.2f, -4367.8f, 25.3f));
        }

        [Test]
        public void OrcStartToCrossroads()
        {
            TryPath(new Vector3(-604.2f, -4215.5f, 38.6f), new Vector3(-448.6f, -2641.4f, 95.5f));
        }

        [Test]
        public void Orgrimmar01()
        {
            TryPath(new Vector3(1379.4f, -4370.25f, 26f), new Vector3(1659f, -4348.3f, 64.5f));
        }

        [Test]
        public void Orgrimmar02()
        {
            TryPath(new Vector3(1379.4f, -4370.25f, 26f), new Vector3(1779.3f, -3993f, 53.4f));
        }

        [Test]
        public void Orgrimmar03()
        {
            TryPath(new Vector3(1379.4f, -4370.25f, 26f), new Vector3(1879.4f, -4286.6f, 23.4f));
        }

        [Test]
        public void Orgrimmar04()
        {
            TryPath(new Vector3(1779.3f, -3993f, 53.4f), new Vector3(1879.4f, -4286.6f, 23.4f));
        }

        [Test]
        public void Orgrimmar05()
        {
            TryPath(new Vector3(1379.4f, -4370.25f, 26f), new Vector3(1688.5f, -3905.2f, 51.3f));
        }
    }

}