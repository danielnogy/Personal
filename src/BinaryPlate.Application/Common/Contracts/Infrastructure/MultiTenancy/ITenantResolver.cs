namespace BinaryPlate.Application.Common.Contracts.Infrastructure.MultiTenancy;

/// <summary>
/// This interface represents the contract for managing tenant-related information
/// such as tenant mode, data isolation strategy, and tenant-specific context.
/// </summary>
public interface ITenantResolver
{
    #region Public Properties

    /// <summary>
    /// Gets or sets a value indicating whether the current request is for the host (non-tenant) context.
    /// </summary>
    bool IsHostRequest { get; }

    /// <summary>
    /// Gets a value indicating whether the current request is for a tenant context.
    /// </summary>
    bool IsTenantRequest { get; }

    /// <summary>
    /// Gets a value indicating whether the current request is a tenant creation request initiated by the host.
    /// </summary>
    bool IsTenantCreationHostRequest { get; }

    /// <summary>
    /// Gets the tenant mode for the current request.
    /// </summary>
    TenantMode TenantMode { get; }

    /// <summary>
    /// Gets the data isolation strategy.
    /// </summary>
    DataIsolationStrategy DataIsolationStrategy { get; }

    #endregion Public Properties

    #region Public Methods

    /// <summary>
    /// Gets the ID of the current tenant.
    /// </summary>
    /// <returns>The ID of the current tenant, or null if no tenant ID has been set.</returns>
    Guid? GetTenantId();

    /// <summary>
    /// Gets the name of the current tenant.
    /// </summary>
    /// <returns>The name of the current tenant, or null if no tenant name has been set.</returns>
    string GetTenantName();

    /// <summary>
    /// Sets the tenant ID and name based on the provided values.
    /// </summary>
    /// <param name="tenantId">The ID of the tenant. Null indicates the request is from the host portal.</param>
    /// <param name="tenantName">The name of the tenant.</param>
    void SetTenantInfo(Guid? tenantId, string tenantName = "");

    /// <summary>
    /// Sets the connection string for the DbContextOptionsBuilder.
    /// </summary>
    /// <param name="contextOptionsBuilder">The DbContextOptionsBuilder to set the connection string for.</param>
    void SetDbConnectionString(DbContextOptionsBuilder contextOptionsBuilder);

    /// <summary>
    /// Throws an exception if the provided tenant ID is invalid.
    /// </summary>
    /// <param name="tenantId">The ID of the tenant to validate.</param>
    /// <param name="pathValue">The path value indicating the context of the request.</param>
    void ThrowExceptionIfTenantIsInvalid(Guid? tenantId, string pathValue);

    /// <summary>
    /// Gets the cache for the specified tenant.
    /// </summary>
    /// <param name="tenantName">The name of the tenant for which the cache should be retrieved.</param>
    /// <returns>The ID of the cached tenant.</returns>
    Guid GetCache(string tenantName);

    /// <summary>
    /// Sets the cache for the specified tenant.
    /// </summary>
    /// <param name="tenantId">The ID of the tenant.</param>
    /// <param name="tenantName">The name of the tenant.</param>
    void SetCache(Guid tenantId, string tenantName);

    /// <summary>
    /// Clears the cache for the specified tenant.
    /// </summary>
    /// <param name="tenantName">The name of the tenant for which the cache should be cleared.</param>
    void ClearCache(string tenantName);

    #endregion Public Methods
}