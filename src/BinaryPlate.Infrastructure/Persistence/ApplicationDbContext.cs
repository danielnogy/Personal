using BinaryPlate.Domain.Entities.SSM;

namespace BinaryPlate.Infrastructure.Persistence;

public sealed class ApplicationDbContext : IdentityDbContext<ApplicationUser,
                                                             ApplicationRole,
                                                             string,
                                                             ApplicationUserClaim,
                                                             ApplicationUserRole,
                                                             ApplicationUserLogin,
                                                             ApplicationRoleClaim,
                                                             ApplicationUserToken>, IApplicationDbContext
{
    #region Private Fields

    private readonly ITenantResolver _tenantResolver;
    private readonly Lazy<IConfiguration> _configuration;
    private readonly Lazy<IDbContextHelperService> _dbContextHelperService;

    #endregion Private Fields

    #region Public Constructors

    public ApplicationDbContext(IConfiguration configuration,
                                ITenantResolver tenantResolver,
                                IDbContextHelperService dbContextHelperService,
                                DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        _tenantResolver = tenantResolver;
        _configuration = new Lazy<IConfiguration>(() => configuration);
        _dbContextHelperService = new Lazy<IDbContextHelperService>(() => dbContextHelperService);
        Current = this;
    }

    #endregion Public Constructors

    #region Public Properties

    // Application identity entities.
    public override DbSet<ApplicationUserRole> UserRoles { get; set; }

    public override DbSet<ApplicationUserClaim> UserClaims { get; set; }
    public override DbSet<ApplicationUserLogin> UserLogins { get; set; }
    public override DbSet<ApplicationRoleClaim> RoleClaims { get; set; }
    public override DbSet<ApplicationUserToken> UserTokens { get; set; }
    public DbSet<ApplicationUserAttachment> ApplicationUserAttachments { get; set; }
    public DbSet<ApplicationPermission> ApplicationPermissions { get; set; }

    // Application configuration entities.
    public DbSet<UserSettings> UserSettings { get; set; }

    public DbSet<PasswordSettings> PasswordSettings { get; set; }
    public DbSet<LockoutSettings> LockoutSettings { get; set; }
    public DbSet<SignInSettings> SignInSettings { get; set; }
    public DbSet<TokenSettings> TokenSettings { get; set; }
    public DbSet<FileStorageSettings> FileStorageSettings { get; set; }

    // Application POC entities.
    public DbSet<Applicant> Applicants { get; set; }

    public DbSet<Reference> References { get; set; }

    // Application reporting entities.
    public DbSet<Report> Reports { get; set; }

    // Application tenant entity.
    public DbSet<Tenant> Tenants { get; set; }

    // DbContext-related properties.
    public DbContext Current { get; }

    #endregion Public Properties
    #region SSM
    public DbSet<Test> Tests { get; set; }
    public DbSet<TestResult> TestResults { get; set; }
    public DbSet<Material> Materials { get; set; }
    public DbSet<MaterialCategory> MaterialCategories { get; set; }
    public DbSet<TestMaterial> TestMaterials { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<QuestionCategory> QuestionCategories { get; set; }
    public DbSet<TestQuestion> TestQuestions { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Answer> Answers { get; set; }
    #endregion
    #region Private Properties

    // Gets the connection string for the host.
    private string HostConnection => _configuration.Value.GetConnectionString(ConnectionString.HostConnection);

    // Gets the connection string for a specific tenant.
    private string TenantConnection => _configuration.Value.GetConnectionString(ConnectionString.MultiTenantSeparateDbConnection);

    #endregion Private Properties

    #region Public Methods

    public async Task<bool> EnsureTenantDatabaseCreated()
    {
        // If the tenant mode is multi-tenant and separate database per tenant is used.
        if (_tenantResolver.TenantMode == TenantMode.MultiTenant && _tenantResolver.DataIsolationStrategy == DataIsolationStrategy.SeparateDatabasePerTenant)
        {
            // Ensure that the database is created.
            var ensureCreatedAsync = await Database.EnsureCreatedAsync();
            return ensureCreatedAsync;
        }
        return false;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        // Perform additional data management actions.
        await _dbContextHelperService.Value.PerformDataManagementActions(this);

        // Asynchronously saves changes to the underlying database and retrieves the total number of changes made.
        var totalChanges = await base.SaveChangesAsync(cancellationToken);

        // Returns the total number of changes made during the SaveChangesAsync operation.
        return totalChanges;
    }

    public bool TrySwitchToHostDatabase()
    {
        // Check if the application is in Multi-Tenant mode with Separate Database per Tenant strategy.
        if (_tenantResolver.TenantMode == TenantMode.MultiTenant && _tenantResolver.DataIsolationStrategy == DataIsolationStrategy.SeparateDatabasePerTenant)
        {
            // Set the database connection string to the host database.
            Database.SetConnectionString(HostConnection);

            // Returns true to indicate that the switch to the host database was successful.
            return true;
        }

        // Returns false as the application is not in the required Multi-Tenant mode or strategy.
        return false;
    }

    public bool TrySwitchToTenantDatabase(string tenantName)
    {
        // Check if the application is in Multi-Tenant mode with Separate Database per Tenant strategy.
        if (_tenantResolver.TenantMode == TenantMode.MultiTenant && _tenantResolver.DataIsolationStrategy == DataIsolationStrategy.SeparateDatabasePerTenant)
        {
            // Sets the database connection string to the specified tenant's database using a formatted connection string.
            Database.SetConnectionString(string.Format(TenantConnection, tenantName));

            // Returns true to indicate that the switch to the specified tenant's database was successful.
            return true;
        }

        // Returns false as the application is not in the required Multi-Tenant mode or strategy.
        return false;
    }

    public async Task<bool> TryRenameTenantDatabase(string currentDbName, string newDbName, CancellationToken cancellationToken)
    {
        // Check if the application is in Multi-Tenant mode with Separate Database per Tenant strategy.
        if (_tenantResolver.TenantMode == TenantMode.MultiTenant && _tenantResolver.DataIsolationStrategy == DataIsolationStrategy.SeparateDatabasePerTenant)
        {
            // Set the current database to single-user mode with immediate rollback.
            await Database.ExecuteSqlRawAsync($"ALTER DATABASE [{currentDbName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE", cancellationToken);

            // Rename the database.
            await Database.ExecuteSqlRawAsync($"ALTER DATABASE [{currentDbName}] MODIFY NAME = [{newDbName}]", cancellationToken);

            // Set the new database back to multi-user mode with immediate rollback.
            await Database.ExecuteSqlRawAsync($"ALTER DATABASE [{newDbName}] SET MULTI_USER WITH ROLLBACK IMMEDIATE", cancellationToken);

            // Returns true to indicate that the renaming of the specified tenant's database was successful.
            return true;
        }

        // Returns false as the application is not in the required Multi-Tenant mode or strategy.
        return false;
    }

    #endregion Public Methods

    #region Protected Methods

    protected override void OnConfiguring(DbContextOptionsBuilder contextOptionsBuilder)
    {
        // Sets the connection string for the current tenant.
        _tenantResolver.SetDbConnectionString(contextOptionsBuilder);

        // Calls the base method to configure other DbContext options.
        base.OnConfiguring(contextOptionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Test>()
            .HasMany(x => x.TestMaterials)
            .WithOne(x => x.Test)
            .HasForeignKey(x => x.TestId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Test>()
            .HasMany(x => x.TestResults)
            .WithOne(x => x.Test)
            .HasForeignKey(x => x.TestId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Test>()
           .HasMany(x => x.TestQuestions)
           .WithOne(x => x.Test)
           .HasForeignKey(x => x.TestId)
           .OnDelete(DeleteBehavior.Cascade);


        modelBuilder.Entity<Material>()
            .HasMany(x => x.TestMaterials)
            .WithOne(x => x.Material)
            .HasForeignKey(x => x.MaterialId)
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<Material>()
            .HasOne(x => x.MaterialCategory)
            .WithMany(x => x.Materials)
            .HasForeignKey(x => x.MaterialCategoryId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Question>()
            .HasMany(x => x.TestQuestions)
            .WithOne(x => x.Question)
            .HasForeignKey(x => x.QuestionId);
        modelBuilder.Entity<Question>()
            .HasOne(x => x.Category)
            .WithMany(x => x.Questions)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.NoAction);
       
        modelBuilder.Entity<Answer>()
            .HasOne(x => x.Question)
            .WithMany(x => x.Answers)
            .HasForeignKey(x => x.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Employee>()
            .HasMany(x=>x.TestResults)
            .WithOne(x => x.Employee)
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<Employee>()
            .HasOne(x=>x.Department)
            .WithMany(x => x.Employees)
            .HasForeignKey(x => x.DepartmentId)
            .OnDelete(DeleteBehavior.NoAction);

        

        // Configures entities based on multi-tenant mode.
        ConfigureEntitiesByTenantMode(modelBuilder);

        // Configures entities to support soft deletion.
        _dbContextHelperService.Value.ConfigureSoftDeletableEntities(modelBuilder);

        // Configures entities for support concurrency check.
        _dbContextHelperService.Value.ConfigureConcurrentEntities(modelBuilder);

        // Configures entities for storing settings in a separate schema.
        _dbContextHelperService.Value.ConfigureSettingsSchemaEntities(modelBuilder);

        // Calls the base method to continue with further configuration.
        base.OnModelCreating(modelBuilder);

        // Applies entity configurations from the current assembly.
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    #endregion Protected Methods

    #region Private Methods

    /// <summary>
    /// Configures entities in the database context based on the current tenant mode.
    /// </summary>
    /// <param name="modelBuilder">The model builder used for configuring entities.</param>
    private void ConfigureEntitiesByTenantMode(ModelBuilder modelBuilder)
    {
        // Depending on the tenant mode, configure the entities and set query filters.
        switch (_tenantResolver.TenantMode)
        {
            // For MultiTenant mode, configure multi-tenant entities and set query filters.
            case TenantMode.MultiTenant:
                _dbContextHelperService.Value.ConfigureMultiTenantsEntities(modelBuilder);
                SetQueryFilterOnMultiTenantsEntities(modelBuilder);
                break;

            // For SingleTenant mode, deconfigure IMustHaveTenant and IMayHaveTenant entities.
            case TenantMode.SingleTenant:
                _dbContextHelperService.Value.DeconfigureITenantEntities(modelBuilder);
                break;
        }

        // Set query filter on permission entity regardless of the tenant mode.
        SetQueryFilterOnPermissionEntity(modelBuilder);
    }

    /// <summary>
    /// Sets a query filter on the permission entity based on the current tenant.
    /// </summary>
    /// <param name="builder">The model builder to be configured.</param>
    private void SetQueryFilterOnPermissionEntity(ModelBuilder builder)
    {
        // Set the query filter for the ApplicationPermission entity, based on tenant or host visibility.
        builder.SetQueryFilter<ApplicationPermission>(p => p.TenantVisibility == !_tenantResolver.IsHostRequest || p.HostVisibility == _tenantResolver.IsHostRequest);
    }

    /// <summary>
    /// Sets query filters on multi-tenant entities based on the current tenant.
    /// </summary>
    /// <param name="builder">The model builder to be configured.</param>
    private void SetQueryFilterOnMultiTenantsEntities(ModelBuilder builder)
    {
        // Set the query filter for entities that may have a tenant ID.
        builder.SetQueryFilter<IMayHaveTenant>(p => p.TenantId == _tenantResolver.GetTenantId());

        // Set the query filter for entities that must have a tenant ID.
        builder.SetQueryFilter<IMustHaveTenant>(p => p.TenantId == _tenantResolver.GetTenantId());
    }

    #endregion Private Methods
}