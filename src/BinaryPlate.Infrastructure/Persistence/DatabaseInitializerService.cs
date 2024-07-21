namespace BinaryPlate.Infrastructure.Persistence;

public class DatabaseInitializerService(IApplicationDbContext dbContext,
                                        IAppSeederService identitySeeder,
                                        IHostDbSeedingStatusService hostDbSeedingStatusService,
                                        IConfiguration configuration) : IDatabaseInitializerService
{
    #region Public Methods

    public async Task CreateSingleTenantDatabase()
    {
        // Ensure the database is created.
        await dbContext.Database.EnsureCreatedAsync();

        // Seed the database based on the specified tenant mode.
        var tenantMode = configuration.GetValue<TenantMode>($"{AppOptions.Section}:{AppTenantOptions.Section}:TenantMode");
        // Seed the database based on the specified tenant mode.
        await ApplicationDbContextSeeder.SeedAsync(identitySeeder, tenantMode);
    }

    public async Task InitializeMultiTenantDatabase(DataIsolationStrategy dataIsolationStrategy)
    {
        // Execute the appropriate initialization method based on the data isolation strategy.
        switch (dataIsolationStrategy)
        {
            // For a shared database used by all tenants.
            case DataIsolationStrategy.SharedDatabaseForAllTenants:
                await InitializeSharedDatabaseForAllTenants();
                break;

            // For a separate database per tenant.
            case DataIsolationStrategy.SeparateDatabasePerTenant:
                await InitializeHostDatabase();
                break;
        }
    }

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Initializes the host database in a multi-tenant environment.
    /// </summary>
    private async Task InitializeHostDatabase()
    {
        // Ensure the database is created.
        await dbContext.Database.EnsureCreatedAsync();

        // Switch to the host database for initialization.
        hostDbSeedingStatusService.UpdateHostDbSeedingStatus(DatabaseType.HostDatabase);

        // Seed the host database based on the specified tenant mode.
        var tenantMode = configuration.GetValue<TenantMode>($"{AppOptions.Section}:{AppTenantOptions.Section}:TenantMode");
        await ApplicationDbContextSeeder.SeedAsync(identitySeeder, tenantMode);

        // Switch back to the per-tenant database.
        hostDbSeedingStatusService.UpdateHostDbSeedingStatus(DatabaseType.PerTenantDatabase);
    }

    /// <summary>
    /// Initializes a shared database for all tenants in a multi-tenant environment.
    /// </summary>
    private async Task InitializeSharedDatabaseForAllTenants()
    {
        // Ensure the database is created.
        await dbContext.Database.EnsureCreatedAsync();

        // Seed the shared database based on the specified tenant mode.
        var tenantMode = configuration.GetValue<TenantMode>($"{AppOptions.Section}:{AppTenantOptions.Section}:TenantMode");
        // Seed the database based on the specified tenant mode.
        await ApplicationDbContextSeeder.SeedAsync(identitySeeder, tenantMode);
    }

    #endregion Private Methods
}