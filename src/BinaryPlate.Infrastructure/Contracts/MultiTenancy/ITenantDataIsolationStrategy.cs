namespace BinaryPlate.Infrastructure.Contracts.MultiTenancy;

/// <summary>
/// Represents the contract for setting the database connection string based on the current tenant and data isolation strategy.
/// </summary>
public interface ITenantDataIsolationStrategy
{
    #region Public Methods

    /// <summary>
    /// Sets the database connection string in the provided <see cref="DbContextOptionsBuilder"/> based on the current tenant.
    /// </summary>
    /// <param name="contextOptionsBuilder">The <see cref="DbContextOptionsBuilder"/> to configure the database context options.</param>
    /// <param name="configuration">The <see cref="IConfiguration"/> containing application settings.</param>
    /// <param name="httpContextAccessor">The <see cref="IHttpContextAccessor"/> to access the current HTTP context.</param>
    void SetDbConnectionString(DbContextOptionsBuilder contextOptionsBuilder, IConfiguration configuration, IHttpContextAccessor httpContextAccessor);

    #endregion Public Methods
}