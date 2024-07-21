namespace BinaryPlate.Infrastructure.Services.TenantResolution;

public class TenantResolver : ITenantResolver
{
    #region Private Fields

    private readonly Lazy<ICacheService> _cacheService;
    private readonly Lazy<IConfiguration> _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly Lazy<ITenantDataIsolationStrategy> _tenantDataIsolationStrategy;
    private readonly AppTenantOptions _appTenantOptions;

    #endregion Private Fields

    #region Public Constructors

    public TenantResolver(ICacheService cacheService,
                          IConfiguration configuration,
                          IAppOptionsService appOptionsService,
                          IHttpContextAccessor httpContextAccessor,
                          IHostDbSeedingStatusService hostDbSeedingStatusService,
                          DataIsolationStrategyFactory dataIsolationStrategyFactory)
    {
        _cacheService = new Lazy<ICacheService>(() => cacheService);
        _configuration = new Lazy<IConfiguration>(() => configuration);
        _appTenantOptions = appOptionsService.GetAppTenantOptions();
        _httpContextAccessor = httpContextAccessor;

        _tenantDataIsolationStrategy = new Lazy<ITenantDataIsolationStrategy>(() => dataIsolationStrategyFactory.CreateInstance(TenantMode,
                                                                                                                                DataIsolationStrategy,
                                                                                                                                hostDbSeedingStatusService,
                                                                                                                                IsTenantRequest,
                                                                                                                                IsHostRequest));
    }

    #endregion Public Constructors

    #region Public Properties

    public Guid? TenantId { get; private set; }
    public string TenantName { get; private set; }
    public bool IsTenantRequest => !string.IsNullOrWhiteSpace(_httpContextAccessor.GetTenantName());
    public bool IsHostRequest => TenantId is null && !IsTenantRequest;
    public bool IsTenantCreationHostRequest => _httpContextAccessor.IsTenantCreationHostRequest();
    public TenantMode TenantMode => (TenantMode)_appTenantOptions.TenantMode;
    public DataIsolationStrategy DataIsolationStrategy => (DataIsolationStrategy)_appTenantOptions.DataIsolationStrategy;

    #endregion Public Properties

    #region Public Methods

    public Guid? GetTenantId() => TenantId;

    public string GetTenantName() => TenantName;

    public void SetTenantInfo(Guid? tenantId, string tenantName = "")
    {
        TenantId = tenantId;
        TenantName = tenantName;
    }

    public void SetDbConnectionString(DbContextOptionsBuilder contextOptionsBuilder)
    {
        // Throws an exception if the application is in single tenant mode and the request is for a specific tenant.
        ThrowExceptionOnSingleTenantMode();

        // Calls the SetDbConnectionString method on the lazy-initialized _tenantDataIsolationStrategy.
        _tenantDataIsolationStrategy.Value.SetDbConnectionString(contextOptionsBuilder, _configuration.Value, _httpContextAccessor);
    }

    public Guid GetCache(string tenantName) => _cacheService.Value.Get<Guid>($"tenant_{tenantName}");

    public void SetCache(Guid tenantId, string tenantName) => _cacheService.Value.Set($"tenant_{tenantName}",
                                                                                      tenantId,
                                                                                      new MemoryCacheEntryOptions { Priority = CacheItemPriority.NeverRemove });

    public void ClearCache(string tenantName) => _cacheService.Value.Remove($"tenant_{tenantName}");

    /// <summary>
    /// Throws an exception if the tenant is invalid based on specified conditions.
    /// </summary>
    /// <param name="tenantId">The tenant ID to check.</param>
    /// <param name="pathValue">The path value to check.</param>
    public void ThrowExceptionIfTenantIsInvalid(Guid? tenantId, string pathValue)
    {
        // Checks if the path is not empty, the tenant ID is empty, and the path does not contain specific values.
        if (!string.IsNullOrEmpty(pathValue) && tenantId == Guid.Empty && !pathValue.Contains("hangfire") && !pathValue.Contains("/Hubs/"))
            // Throws an exception with the message indicating an invalid tenant name.
            throw new Exception(Resource.Invalid_tenant_name);
    }

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Throws an exception if accessing the tenant data platform is not allowed in single-tenant mode.
    /// </summary>
    private void ThrowExceptionOnSingleTenantMode()
    {
        // Checks if the request is for a tenant and the application is in single-tenant mode.
        if (IsTenantRequest && TenantMode == TenantMode.SingleTenant)
            // Throws an InvalidOperationException with a message indicating the restriction.
            throw new InvalidOperationException(Resource.Accessing_the_TDP_is_not_possible_in_single_tenant_mode);
    }

    #endregion Private Methods
}