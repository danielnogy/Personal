namespace BinaryPlate.Infrastructure.Services.TenantResolution.DataIsolationStrategies;

public class DataIsolationStrategyFactory
{
    #region Public Methods

    public ITenantDataIsolationStrategy CreateInstance(TenantMode tenantMode, DataIsolationStrategy dataIsolationStrategy, IHostDbSeedingStatusService hostDbSeedingStatusService, bool isTenantRequest, bool isHostRequest)
    {
        // Switch based on the specified TenantMode to determine the appropriate ITenantDataIsolationStrategy.
        switch (tenantMode)
        {
            // For MultiTenant mode
            case TenantMode.MultiTenant:
                switch (dataIsolationStrategy)
                {
                    // If SeparateDatabasePerTenant strategy and the request is for the host or there is a pending host database seeding status, use HostDataIsolationStrategy.
                    case DataIsolationStrategy.SeparateDatabasePerTenant when isHostRequest || hostDbSeedingStatusService.GetHostDbSeedingPendingStatus():
                        return new HostDataIsolationStrategy();

                    // If SeparateDatabasePerTenant strategy and the request is for a specific tenant, use SeparateDataIsolationPerTenantStrategy.
                    case DataIsolationStrategy.SeparateDatabasePerTenant when isTenantRequest:
                        return new SeparateDataIsolationPerTenantStrategy();

                    // If SharedDatabaseForAllTenants strategy, use SharedDataIsolationForAllTenantsStrategy.
                    case DataIsolationStrategy.SharedDatabaseForAllTenants:
                        return new SharedDataIsolationForAllTenantsStrategy();

                    // Throw an exception for unsupported data isolation strategy.
                    default:
                        throw new ArgumentOutOfRangeException();
                }

            // For SingleTenant mode, use SingleTenantDataIsolationStrategy.
            case TenantMode.SingleTenant:
                return new SingleTenantDataIsolationStrategy();

            // Throw an exception for unsupported tenant mode.
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    #endregion Public Methods
}