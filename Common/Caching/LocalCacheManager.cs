using Common.Logging;

namespace Common.Caching
{
    public class LocalCacheManager : ICacheManager
    {
        private readonly ILog _logger;

        // These providers return Dictionary<string, T> and are used to reload the cache of a group
        private readonly Dictionary<string, Func<dynamic>> _providers = new Dictionary<string, Func<dynamic>>();

        private readonly object _cacheLock = new object();
        private readonly Dictionary<string, Cache<dynamic, dynamic>> _cache;

        public LocalCacheManager(ILog logger)
        {
            _logger = logger;
            _cache = new Dictionary<string, Cache<dynamic, dynamic>>();
        }

        public void DefineReload<TKey, TValue>(string groupKey, Func<Dictionary<TKey, TValue>> provider)
        {
            _providers[groupKey] = provider;
        }

        public bool Delete<TKey>(string groupKey, TKey key)
        {
            lock (_cacheLock)
            {
                if (_cache.TryGetValue(groupKey, out Cache<dynamic, dynamic> value))
                {
                    return value.Remove(key);
                }
                return false;
            }
        }

        public Task<bool> DeleteAsync<TKey>(string groupKey, TKey key)
        {
            return Task.FromResult(Delete(groupKey, key));
        }

        public bool Exists<TKey>(string groupKey, TKey key)
        {
            lock (_cacheLock)
            {
                if (_cache.TryGetValue(groupKey, out Cache<dynamic, dynamic> value))
                {
                    return value.Contains(key);
                }
                return false;
            }
        }

        public Task<bool> ExistsAsync<TKey>(string groupKey, TKey key)
        {
            return Task.FromResult(Exists(groupKey, key));
        }

        public TValue Get<TKey, TValue>(string groupKey, TKey key)
        {
            if (!GroupExists(groupKey))
            {
                Reload<TKey, TValue>(groupKey);
            }

            lock (_cacheLock)
            {
                if (_cache.TryGetValue(groupKey, out Cache<dynamic, dynamic> value)
                    && value.TryGetValue(key, out dynamic val))
                {
                    return (TValue)val;
                }
                return default;
            }
        }

        public async Task<TValue> GetAsync<TKey, TValue>(string groupKey, TKey key)
        {
            if (!GroupExists(groupKey))
            {
                await ReloadAsync<TKey, TValue>(groupKey);
            }

            return await Task.FromResult(Get<TKey, TValue>(groupKey, key));
        }

        public HashSet<TKey> GetKeys<TKey, TValue>(string groupKey)
        {
            if (!GroupExists(groupKey))
            {
                Reload<TKey, TValue>(groupKey);
            }

            lock (_cacheLock)
            {
                if (_cache.TryGetValue(groupKey, out Cache<dynamic, dynamic> value))
                {
                    return value.Data.Select(x => (TKey)x.Key).ToHashSet();
                }
                return new HashSet<TKey>();
            }
        }

        public Dictionary<TKey, TValue> GetGroup<TKey, TValue>(string groupKey)
        {
            if (!GroupExists(groupKey))
            {
                Reload<TKey, TValue>(groupKey);
            }

            lock (_cacheLock)
            {
                if (_cache.TryGetValue(groupKey, out Cache<dynamic, dynamic> value))
                {
                    return value.Data.ToDictionary(x => (TKey)x.Key, x => (TValue)x.Value);
                }
                return new Dictionary<TKey, TValue>();
            }
        }

        public async Task<Dictionary<TKey, TValue>> GetGroupAsync<TKey, TValue>(string groupKey)
        {
            if (!GroupExists(groupKey))
            {
                await ReloadAsync<TKey, TValue>(groupKey);
            }

            return await Task.FromResult(GetGroup<TKey, TValue>(groupKey));
        }

        public Dictionary<TKey, TValue> GetSet<TKey, TValue>(string groupKey, IEnumerable<TKey> keys)
        {
            if (!GroupExists(groupKey))
            {
                Reload<TKey, TValue>(groupKey);
            }

            HashSet<TKey> keysHash = keys.ToHashSet();
            lock (_cacheLock)
            {
                if (_cache.TryGetValue(groupKey, out Cache<dynamic, dynamic> value))
                {
                    return value.Data.Where(x => keysHash.Contains(x.Key)).ToDictionary(x => (TKey)x.Key, x => (TValue)x.Value);
                }
                return new Dictionary<TKey, TValue>();
            }
        }

        public async Task<Dictionary<TKey, TValue>> GetSetAsync<TKey, TValue>(string groupKey, IEnumerable<TKey> keys)
        {
            if (!GroupExists(groupKey))
            {
                await ReloadAsync<TKey, TValue>(groupKey);
            }

            return await Task.FromResult(GetSet<TKey, TValue>(groupKey, keys));
        }

        public bool GroupDelete(string groupKey)
        {
            lock (_cacheLock)
            {
                if (_cache.ContainsKey(groupKey))
                {
                    return _cache.Remove(groupKey);
                }
                return false;
            }
        }

        public Task<bool> GroupDeleteAsync(string groupKey)
        {
            return Task.FromResult(GroupDelete(groupKey));
        }

        public bool GroupExists(string groupKey)
        {
            lock (_cacheLock)
            {
                return _cache.ContainsKey(groupKey);
            }
        }

        public Task<bool> GroupExistsAsync(string groupKey)
        {
            return Task.FromResult(GroupExists(groupKey));
        }

        public bool Reload<TKey, TValue>(string groupKey)
        {
            if (!_providers.TryGetValue(groupKey, out Func<dynamic> provider))
            {
                throw new ArgumentException($"No provider set for group '{groupKey}'");
            }

            Dictionary<TKey, TValue> values = provider();
            return Reload(groupKey, values);
        }

        public bool Reload<TKey, TValue>(string groupKey, Dictionary<TKey, TValue> values)
        {
            lock (_cacheLock)
            {
                _cache[groupKey] = new Cache<dynamic, dynamic>(values.ToDictionary(x => (dynamic)x.Key, x => (dynamic)x.Value));
                return true;
            }
        }

        public Task<bool> ReloadAsync<TKey, TValue>(string groupKey)
        {
            return Task.FromResult(Reload<TKey, TValue>(groupKey));
        }

        public Task<bool> ReloadAsync<TKey, TValue>(string groupKey, Dictionary<TKey, TValue> values)
        {
            return Task.FromResult(Reload(groupKey, values));
        }

        public void Set<TKey, TValue>(string groupKey, TKey key, TValue value)
        {
            lock (_cacheLock)
            {
                if (!_cache.TryGetValue(groupKey, out Cache<dynamic, dynamic> cache))
                {
                    cache = new Cache<dynamic, dynamic>();
                    _cache[groupKey] = cache;
                }
                cache.AddOrUpdate(key, value);
            }
        }

        public Task SetAsync<TKey, TValue>(string groupKey, TKey key, TValue value)
        {
            Set(groupKey, key, value);
            return Task.CompletedTask;
        }
    }
}
