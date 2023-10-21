namespace Common.Caching
{
    public interface ICacheManager
    {
        TValue Get<TKey, TValue>(string groupKey, TKey key);
        Task<TValue> GetAsync<TKey, TValue>(string groupKey, TKey key);

        Dictionary<TKey, TValue> GetSet<TKey, TValue>(string groupKey, IEnumerable<TKey> keys);
        Task<Dictionary<TKey, TValue>> GetSetAsync<TKey, TValue>(string groupKey, IEnumerable<TKey> keys);

        HashSet<TKey> GetKeys<TKey, TValue>(string groupKey);
        Dictionary<TKey, TValue> GetGroup<TKey, TValue>(string groupKey);
        Task<Dictionary<TKey, TValue>> GetGroupAsync<TKey, TValue>(string groupKey);

        void Set<TKey, TValue>(string groupKey, TKey key, TValue value);
        Task SetAsync<TKey, TValue>(string groupKey, TKey key, TValue value);

        bool Reload<TKey, TValue>(string groupKey);
        Task<bool> ReloadAsync<TKey, TValue>(string groupKey);
        bool Reload<TKey, TValue>(string groupKey, Dictionary<TKey, TValue> values);
        Task<bool> ReloadAsync<TKey, TValue>(string groupKey, Dictionary<TKey, TValue> values);
        void DefineReload<TKey, TValue>(string groupKey, Func<Dictionary<TKey, TValue>> provider);

        bool Exists<TKey>(string groupKey, TKey key);
        Task<bool> ExistsAsync<TKey>(string groupKey, TKey key);
        bool GroupExists(string groupKey);
        Task<bool> GroupExistsAsync(string groupKey);

        bool Delete<TKey>(string groupKey, TKey key);
        Task<bool> DeleteAsync<TKey>(string groupKey, TKey key);
        bool GroupDelete(string groupKey);
        Task<bool> GroupDeleteAsync(string groupKey);
    }
}
