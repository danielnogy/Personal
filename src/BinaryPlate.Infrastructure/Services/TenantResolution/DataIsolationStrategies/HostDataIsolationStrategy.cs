namespace BinaryPlate.Infrastructure.Services.TenantResolution.DataIsolationStrategies;

public class HostDataIsolationStrategy() : ITenantDataIsolationStrategy
{
    #region Public Methods

    public void SetDbConnectionString(DbContextOptionsBuilder contextOptionsBuilder, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        // Get the connection string for the host database from the application settings.
        var hostConnectionString = configuration.GetConnectionString(ConnectionString.HostConnection);

        // Configure the database context options with the host connection string.
        contextOptionsBuilder.UseSqlServer(hostConnectionString)
                             .EnableSensitiveDataLogging(true);
    }

    #endregion Public Methods
}