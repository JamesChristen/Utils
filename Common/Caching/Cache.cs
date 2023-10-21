namespace Common.Caching
{
    internal class Cache<K, V>
    {
        private readonly object _lock = new object();
        private Dictionary<K, V> _cache = new Dictionary<K, V>();

        public Cache()
        {
        }

        public Cache(Dictionary<K, V> cache)
        {
            _cache = cache ?? new Dictionary<K, V>();
        }

        public bool HasValues
        {
            get
            {
                lock (_lock)
                {
                    return _cache.Count > 0;
                }
            }
        }

        public IEnumerable<V> Values
        {
            get
            {
                lock (_lock)
                {
                    return new List<V>(_cache.Values);
                }
            }
        }

        public Dictionary<K, V> Data
        {
            get
            {
                lock (_lock)
                {
                    return new Dictionary<K, V>(_cache);
                }
            }
        }

        public bool Contains(K key)
        {
            lock (_lock)
            {
                return _cache.ContainsKey(key);
            }
        }

        public V Get(K key, bool throwIfNotFound = false)
        {
            if (_cache.TryGetValue(key, out V value))
            {
                return value;
            }
            else if (throwIfNotFound)
            {
                throw new ArgumentException($"No item with key {key} in cache");
            }
            return default;
        }

        public bool TryGetValue(K key, out V value)
        {
            return _cache.TryGetValue(key, out value);
        }

        public void Add(K key, V value)
        {
            lock (_lock)
            {
                if (_cache.ContainsKey(key))
                {
                    throw new ArgumentException($"Item with key {key} already in cache");
                }
                _cache.Add(key, value);
            }
        }

        public void AddOrUpdate(K key, V value)
        {
            lock (_lock)
            {
                _cache[key] = value;
            }
        }

        public void AddOrUpdate(Dictionary<K, V> dict)
        {
            if (dict == null)
            {
                return;
            }

            lock (_lock)
            {
                foreach (KeyValuePair<K, V> pair in dict)
                {
                    _cache[pair.Key] = pair.Value;
                }
            }
        }

        public void Reload(Dictionary<K, V> cache)
        {
            lock (_lock)
            {
                _cache = new Dictionary<K, V>(cache);
            }
        }

        public bool Remove(K key)
        {
            lock (_lock)
            {
                return _cache.Remove(key);
            }
        }

        public void Clear()
        {
            lock (_lock)
            {
                _cache.Clear();
            }
        }
    }
}
