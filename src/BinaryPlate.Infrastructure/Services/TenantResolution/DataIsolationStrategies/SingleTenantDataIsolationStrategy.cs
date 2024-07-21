namespace BinaryPlate.Infrastructure.Services.TenantResolution.DataIsolationStrategies;

public class SingleTenantDataIsolationStrategy : ITenantDataIsolationStrategy
{
    #region Public Methods

    public void SetDbConnectionString(DbContextOptionsBuilder contextOptionsBuilder, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        // Use the single-tenant database connection string from the configuration.
        contextOptionsBuilder.UseSqlServer(configuration.GetConnectionString(ConnectionString.SingleTenantDbConnection))
                             .EnableSensitiveDataLogging(false);

    }

    #endregion Public Methods
}