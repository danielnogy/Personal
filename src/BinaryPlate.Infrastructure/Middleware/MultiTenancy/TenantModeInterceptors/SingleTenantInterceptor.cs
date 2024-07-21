namespace BinaryPlate.Infrastructure.Middleware.MultiTenancy.TenantModeInterceptors;

public class SingleTenantInterceptor : ITenantInterceptor
{
    #region Public Properties

    public TenantMode TenantMode => TenantMode.SingleTenant;

    #endregion Public Properties

    #region Public Methods

    public void Handle(HttpContext httpContext, IApplicationDbContext dbContext, ITenantResolver tenantResolver)
    {
        // Handles setting the tenant information for SingleTenant mode to an empty GUID.
        HandleSingleTenantMode(tenantResolver);
    }

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Sets the tenant information for SingleTenant mode to an empty GUID.
    /// </summary>
    /// <param name="tenantResolver">The tenant resolver instance.</param>
    private static void HandleSingleTenantMode(ITenantResolver tenantResolver)
    {
        tenantResolver.SetTenantInfo(Guid.Empty);
    }

    #endregion Private Methods
}