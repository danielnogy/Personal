using BinaryPlate.Domain.Entities.SSM;
using Mapster;

namespace BinaryPlate.Infrastructure;

public static class DependencyInjection
{
    #region Public Methods

    /// <summary>
    /// Configures the infrastructure services for the application.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <param name="configuration">
    /// The <see cref="IConfiguration"/> instance representing the appsettings.json file.
    /// </param>
    /// <param name="environment">
    /// The <see cref="IHostEnvironment"/> instance representing the current hosting environment.
    /// </param>
    /// <returns>The <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        // Add DbContext.
        services.AddDbContext<ApplicationDbContext>();

        // Add Hangfire.
        services.AddHangfire(globalConfiguration => globalConfiguration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(configuration.GetConnectionString(ConnectionString.HangfireDbConnection), new SqlServerStorageOptions
            {
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                QueuePollInterval = TimeSpan.Zero,
                UseRecommendedIsolationLevel = true,
                DisableGlobalLocks = true
            }));

        services.AddHangfireServer();

        // Add scoped DbContext.
        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

        // Add Identity.
        services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddErrorDescriber<LocalizedIdentityErrorDescriber>()
                .AddDefaultTokenProviders()
                .AddPasswordValidator<CustomPasswordValidator<ApplicationUser>>()
                .AddUserManager<ApplicationUserManager>()
                .AddRoleManager<ApplicationRoleManager>();
        //.AddUserStore<CustomUserStore>()
        //.AddRoleStore<CustomRoleStore>();

        // Replace default validators with multi-tenant validators.
        services.Replace(ServiceDescriptor.Scoped<IUserValidator<ApplicationUser>, MultiTenantUserValidator>());
        services.Replace(ServiceDescriptor.Scoped<IRoleValidator<ApplicationRole>, MultiTenantRoleValidator>());

        // TODO: Uncomment and configure the password hashing options if needed.
        //services.Configure<PasswordHasherOptions>(options =>
        //{
        // options.IterationCount = 10000;
        //});

        // TODO: Uncomment and configure the password hashing options if using BCryptPasswordHasher.
        //services.AddScoped<IPasswordHasher<ApplicationUser>, BCryptPasswordHasher<ApplicationUser>>();
        //services.Configure<BCryptPasswordHasherOptions>(options =>
        //{
        // options.WorkFactor = 10;
        // options.EnhancedEntropy = false;
        //});

        // X-CSRF-Token.
        services.AddAntiforgery(options =>
        {
            options.HeaderName = "X-XSRF-Token";
            options.SuppressXFrameOptionsHeader = false;
        });

        // Add HttpContextAccessor.
        services.AddHttpContextAccessor();

        // Add Cache Services.
        services.AddMemoryCache();
        services.AddSingleton<ICacheService, MemoryCacheService>();

        // Add Application settings options.
        services.AddAppSettings(configuration);

        // Add identity managers.
        services.AddScoped<UserManager<ApplicationUser>, ApplicationUserManager>();
        services.AddScoped<RoleManager<ApplicationRole>, ApplicationRoleManager>();

        // Add singleton services.
        services.AddSingleton<UtcDateTimeProvider>();

        // Add application services.
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<IReportDataProvider, ReportDataProvider>();
        services.AddScoped<IAppSettingsService, AppSettingsService>();

        // Add infrastructure services.
        services.AddScoped<IUtcDateTimeProvider>(provider => new FrozenUtcDateTimeProvider(provider.GetRequiredService<UtcDateTimeProvider>()));
        services.AddScoped<IdentityErrorDescriber, LocalizedIdentityErrorDescriber>();

        services.AddScoped<ITokenGeneratorService, TokenGeneratorService>();
        services.AddScoped<IAppOptionsService, AppOptionsService>();
        services.AddScoped<INotificationService, NotificationService>();

        services.AddScoped<IBackgroundReportingService, BackgroundReportingService>();
        services.AddScoped<IReportingService, ReportingService>();

        services.AddScoped<IStorageProvider, StorageProvider>();
        services.AddScoped<IStorageFactory, StorageFactory>();
        services.AddScoped<IFileStorageService, AzureStorageService>();
        services.AddScoped<IFileStorageService, OnPremisesStorageService>();

        services.AddTransient<DataIsolationStrategyFactory>();
        services.AddTransient<ITenantDataIsolationStrategy, HostDataIsolationStrategy>();
        services.AddTransient<ITenantDataIsolationStrategy, SeparateDataIsolationPerTenantStrategy>();
        services.AddTransient<ITenantDataIsolationStrategy, SharedDataIsolationForAllTenantsStrategy>();
        services.AddTransient<ITenantDataIsolationStrategy, SingleTenantDataIsolationStrategy>();

        services.AddTransient<TenantInterceptorFactory>();
        services.AddTransient<ITenantInterceptor, MultiTenantInterceptor>();
        services.AddTransient<ITenantInterceptor, SingleTenantInterceptor>();

        services.AddScoped<ITenantResolver, TenantResolver>();
        services.AddScoped<IDatabaseInitializerService, DatabaseInitializerService>();
        services.AddScoped<IHostDbSeedingStatusService, HostDbSeedingStatusService>();
        services.AddScoped<IAppSeederService, AppSeederService>();
        services.AddScoped<IPermissionScanner, PermissionScanner>();
        services.AddScoped<IDbContextHelperService, DbContextHelperService>();

        return services;
    }
    
    #endregion Public Methods
}