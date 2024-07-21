namespace BinaryPlate.Infrastructure.Services.TenantResolution.DataIsolationStrategies;

public class SharedDataIsolationForAllTenantsStrategy : ITenantDataIsolationStrategy
{
    #region Public Methods

    public void SetDbConnectionString(DbContextOptionsBuilder contextOptionsBuilder, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        // Use the shared database connection string from the configuration.
        contextOptionsBuilder.UseSqlServer(configuration.GetConnectionString(ConnectionString.MultiTenantSharedDbConnection))
                             .EnableSensitiveDataLogging(true);
    }

    #endregion Public Methods
}