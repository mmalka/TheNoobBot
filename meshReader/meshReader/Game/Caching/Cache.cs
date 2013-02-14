using meshReader.Game.MDX;
using meshReader.Game.WMO;

namespace meshReader.Game.Caching
{

    public static class Cache
    {
        public static GenericCache<MDX.Model> Model = new GenericCache<Model>();
        public static GenericCache<WMO.WorldModelRoot> WorldModel = new GenericCache<WorldModelRoot>();

        public static void Clear()
        {
            Model.Clear();
            WorldModel.Clear();
        }
    }

}