namespace BinaryPlate.Infrastructure.Services.TenantResolution.DataIsolationStrategies;

public class SeparateDataIsolationPerTenantStrategy : ITenantDataIsolationStrategy
{
    #region Public Methods

    public void SetDbConnectionString(DbContextOptionsBuilder contextOptionsBuilder, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        // Get the name of the current tenant from the HTTP context.
        var tenantName = httpContextAccessor.GetTenantName();

        // Construct the connection string for the separate database per tenant.
        var tenantConnection = string.Format(configuration.GetConnectionString(ConnectionString.MultiTenantSeparateDbConnection)
                                             ?? throw new InvalidOperationException("MultiTenantSeparateDbConnection is missing from the appsettings."),
                                             tenantName ?? throw new ArgumentNullException("Invalid database name."));

        // Configure the database context options with the tenant-specific connection string.
        contextOptionsBuilder.UseSqlServer(tenantConnection)
                             .EnableSensitiveDataLogging(true);
    }

    #endregion Public Methods
}