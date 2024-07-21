namespace BinaryPlate.Infrastructure.Middleware.MultiTenancy.TenantModeInterceptors;

public class TenantInterceptorFactory
{
    #region Public Methods

    public ITenantInterceptor CreateInstance(TenantMode tenantMode)
    {
        // Switch based on the specified TenantMode to determine the appropriate ITenantDataIsolationStrategy.
        switch (tenantMode)
        {
            // If the application is in SingleTenant mode, use the SingleTenantInterceptor.
            case TenantMode.SingleTenant:
                return new SingleTenantInterceptor();

            // If the application is in MultiTenant mode, use the MultiTenantInterceptor.
            case TenantMode.MultiTenant:
                return new MultiTenantInterceptor();

            // Throw an exception for unsupported tenant mode.
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    #endregion Public Methods
}