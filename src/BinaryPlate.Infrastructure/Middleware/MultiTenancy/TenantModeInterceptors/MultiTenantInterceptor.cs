namespace BinaryPlate.Infrastructure.Middleware.MultiTenancy.TenantModeInterceptors;

public class MultiTenantInterceptor : ITenantInterceptor
{
    #region Public Properties

    public TenantMode TenantMode => TenantMode.MultiTenant;

    #endregion Public Properties

    #region Public Methods

    public void Handle(HttpContext httpContext, IApplicationDbContext dbContext, ITenantResolver tenantResolver)
    {
        // Get the name of the current tenant from the HTTP context.
        var tenantName = httpContext.GetTenantName();

        // Check if no tenant is specified.
        if (string.IsNullOrEmpty(tenantName))
        {
            // Set the tenant information to null if no tenant is specified.
            tenantResolver.SetTenantInfo(tenantId: null);
        }
        else
        {
            // Try to get the tenant ID from the cache or database.
            var tenantId = TryGetCachedTenantIdIfEnabled(dbContext, tenantResolver, tenantName);

            // Throw an exception if the tenant is invalid.
            tenantResolver.ThrowExceptionIfTenantIsInvalid(tenantId, httpContext.Request.Path.Value);

            // Set the tenant information based on the retrieved tenant ID and name.
            tenantResolver.SetTenantInfo(tenantId, tenantName);
        }
    }

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Tries to get the cached tenant ID if caching is enabled; otherwise, retrieves it from the database.
    /// </summary>
    /// <param name="dbContext">The application database context.</param>
    /// <param name="tenantResolver">The tenant resolver.</param>
    /// <param name="tenantName">The name of the tenant.</param>
    /// <returns>The ID of the tenant.</returns>
    private Guid? TryGetCachedTenantIdIfEnabled(IApplicationDbContext dbContext, ITenantResolver tenantResolver, string tenantName)
    {
        // Try to get the tenant ID from the cache.
        var tenantId = tenantResolver.GetCache(tenantName);

        // Check if the tenant ID is already in the cache.
        if (tenantId != Guid.Empty)
            return tenantId;

        // Variable to store the retrieved tenant information.
        Tenant tenant;

        // Check the tenant mode and data isolation strategy.
        switch (tenantResolver.TenantMode)
        {
            case TenantMode.MultiTenant when tenantResolver.DataIsolationStrategy == DataIsolationStrategy.SeparateDatabasePerTenant:
                {
                    // Set the database connection to the host connection to retrieve the tenant information.
                    dbContext.TrySwitchToHostDatabase();

                    // Retrieve the tenant information from the database
                    tenant = dbContext.Tenants.FirstOrDefault(t => t.Name == tenantName);

                    // Throw an exception if the tenant is not found.
                    if (tenant == null)
                        throw new Exception(Resource.Tenant_is_not_found);

                    // Set the database connection to the tenant-specific connection.
                    dbContext.TrySwitchToTenantDatabase(tenant.Name);
                    break;
                }
            default:
                // Retrieve the tenant information from the database in other tenant modes.
                tenant = dbContext.Tenants.FirstOrDefault(t => t.Name == tenantName);
                break;
        }

        // Check the retrieved tenant information.
        switch (tenant)
        {
            case { Enabled: true }:
                // Set the tenant ID in the cache and return it.
                tenantId = tenant.Id;
                tenantResolver.SetCache(tenant.Id, tenant.Name);
                break;

            case { Enabled: false }:
                // Throw an exception if the tenant is deactivated.
                throw new Exception(Resource.Tenant_is_deactivated);
            case null:
                // Throw an exception if the tenant is not found.
                throw new Exception(Resource.Tenant_is_not_found);
        }
        return tenantId;
    }

    #endregion Private Methods
}