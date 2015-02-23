using meshReader.Game.MDX;
using meshReader.Game.WMO;
using System.Collections.Generic;
using System;

namespace meshReader.Game.Caching
{

    public static class Cache
    {
        public static GenericCache<MDX.Model> Model = new GenericCache<Model>();
        public static GenericCache<WMO.WorldModelRoot> WorldModel = new GenericCache<WorldModelRoot>();
        //public static Dictionary<Tuple<int, int>, ADT.ADT> Adt = new Dictionary<Tuple<int, int>, ADT.ADT>();

        public static void Clear()
        {
            Model.Clear();
            WorldModel.Clear();
        }
    }

}