using Microsoft.Extensions.Caching.Memory;

namespace BinaryPlate.Application.Common.Contracts.Infrastructure;

/// <summary>
/// This interface represents the contract for caching data in memory.
/// </summary>
public interface ICacheService
{
    #region Public Methods

    /// <summary>
    /// Gets the cached value associated with the specified key.
    /// </summary>
    /// <typeparam name="T">The type of the cached value.</typeparam>
    /// <param name="key">The key of the cached value.</param>
    /// <returns>The cached value, or default(T) if the key is not found.</returns>
    T Get<T>(string key);

    /// <summary>
    /// Sets the value associated with the specified key in the cache.
    /// </summary>
    /// <typeparam name="T">The type of the value to cache.</typeparam>
    /// <param name="key">The key of the cached value.</param>
    /// <param name="value">The value to cache.</param>
    /// <param name="cacheEntryOptions">Options for the cache entry.</param>
    void Set<T>(string key, T value, MemoryCacheEntryOptions cacheEntryOptions = default);

    /// <summary>
    /// Removes the cached value associated with the specified key.
    /// </summary>
    /// <param name="key">The key of the cached value to remove.</param>
    void Remove(string key);

    #endregion Public Methods
}