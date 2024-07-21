namespace BinaryPlate.Infrastructure.Services;

public class MemoryCacheService(IMemoryCache cache) : ICacheService
{
    #region Public Methods

    public T Get<T>(string key)
    {
        return cache.Get<T>(key);
    }

    public void Set<T>(string key, T value, MemoryCacheEntryOptions cacheEntryOptions = default)
    {
        cache.Set(key, value, cacheEntryOptions);
    }

    public void Remove(string key)
    {
        cache.Remove(key);
    }

    #endregion Public Methods
}