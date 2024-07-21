namespace BinaryPlate.Infrastructure.Persistence;

public class HostDbSeedingStatusService(ICacheService cacheService) : IHostDbSeedingStatusService
{
    #region Public Methods

    public void UpdateHostDbSeedingStatus(DatabaseType databaseType)
    {
        // Update the cache value to indicate whether the host database is seeded or not.
        switch (databaseType)
        {
            // If switching to the host database, set the cache value to "false" (not seeded).
            case DatabaseType.HostDatabase:
                cacheService.Set("HostDbSeeded", "false", new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
                break;

            // If switching to the per-tenant database, set the cache value to "true" (seeded).
            case DatabaseType.PerTenantDatabase:
                cacheService.Set("HostDbSeeded", "true", new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove));
                break;
        }
    }

    public bool GetHostDbSeedingPendingStatus()
    {
        // Check if the host database seeding is pending or not and return the flag value.
        return cacheService.Get<string>("HostDbSeeded") == null || cacheService.Get<string>("HostDbSeeded") == "false";
    }

    #endregion Public Methods
}