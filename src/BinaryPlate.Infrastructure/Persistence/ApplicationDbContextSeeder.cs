namespace BinaryPlate.Infrastructure.Persistence;

public static class ApplicationDbContextSeeder
{
    #region Public Methods

    public static async Task SeedAsync(IAppSeederService appSeederService, TenantMode tenantMode)
    {
        // Seeds the application data based on the tenant mode.
        switch (tenantMode)
        {
            case TenantMode.SingleTenant:
                // Seed the application database for single tenant mode.
                await appSeederService.SeedSingleTenantModeDatabase();
                break;

            case TenantMode.MultiTenant:
                // Seed the host application database for multi-tenant mode.
                await appSeederService.SeedHostDatabase();
                break;

            default:
                // Throw an exception for unsupported tenant mode.
                throw new ArgumentOutOfRangeException(nameof(tenantMode), tenantMode, Resource.Unsupported_tenant_mode);
        }
    }

    #endregion Public Methods
}