using System.Collections.Generic;

namespace meshReader.Game.Caching
{
    public class GenericCache<T> where T : class
    {
        public const int FlushLimit = 1000;
        private readonly Dictionary<string, T> _items = new Dictionary<string, T>(1000);

        public void Insert(string key, T val)
        {
            if (_items.Count > FlushLimit)
                Clear();

            _items.Add(key, val);
        }

        public T Get(string key)
        {
            T ret;
            if (_items.TryGetValue(key, out ret))
                return ret;
            return null;
        }

        public void Clear()
        {
            _items.Clear();
        }
    }
}