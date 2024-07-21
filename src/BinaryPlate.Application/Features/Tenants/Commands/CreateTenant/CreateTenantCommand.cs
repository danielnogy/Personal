namespace BinaryPlate.Application.Features.Tenants.Commands.CreateTenant;

public class CreateTenantCommand : IRequest<Envelope<CreateTenantResponse>>
{
    #region Public Properties

    public string Name { get; set; }
    public bool Enabled { get; set; }

    #endregion Public Properties

    #region Public Methods

    public Tenant MapToEntity()
    {
        return new Tenant
        {
            Id = Guid.NewGuid(),
            Name = Name,
            Enabled = Enabled
        };
    }

    #endregion Public Methods
}

public class CreateTenantCommandHandler : IRequestHandler<CreateTenantCommand, Envelope<CreateTenantResponse>>
{
    #region Private Fields

    private readonly ITenantResolver _tenantResolver;
    private readonly IApplicationDbContext _dbContext;
    private readonly IAppSeederService _appSeederService;
    private readonly IOptions<IdentityOptions> _identityOptions;

    #endregion Private Fields

    #region Public Constructors

    public CreateTenantCommandHandler(ITenantResolver tenantResolver,
                                      IApplicationDbContext dbContext,
                                      IAppSeederService appSeederService,
                                      IOptions<IdentityOptions> identityOptions)
    {
        _tenantResolver = tenantResolver;
        _dbContext = dbContext;
        _appSeederService = appSeederService;
        _identityOptions = identityOptions;

        // Get the connection string for the "HostConnection".

        // Disable password complexity for new tenants.
        DisablePasswordComplexity();
    }

    #endregion Public Constructors

    #region Public Methods

    public async Task<Envelope<CreateTenantResponse>> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
    {
        // Determine the data isolation strategy based on the tenant resolver.
        switch (_tenantResolver.DataIsolationStrategy)
        {
            // Shared database strategy for all tenants.
            case DataIsolationStrategy.SharedDatabaseForAllTenants:
                return await CreateTenantViaSharedDatabaseStrategy(request, cancellationToken);

            // Separate database strategy for each tenant.
            case DataIsolationStrategy.SeparateDatabasePerTenant:
                return await CreateTenantViaSeparateDatabasePerTenantStrategy(request, cancellationToken);

            // Invalid argument.
            default:
                return Envelope<CreateTenantResponse>.Result.ServerError("Invalid argument.");
        }
    }

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Handles the creation of a new tenant using the shared database strategy.
    /// </summary>
    /// <param name="request">The create tenant command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An envelope containing the response for the create tenant operation.</returns>
    private async Task<Envelope<CreateTenantResponse>> CreateTenantViaSharedDatabaseStrategy(CreateTenantCommand request, CancellationToken cancellationToken)
    {
        // Check if the tenant with the same name already exists.
        var tenantExists = await _dbContext.Tenants.AnyAsync(t => t.Name == request.Name, cancellationToken: cancellationToken);

        if (tenantExists)
            return Envelope<CreateTenantResponse>.Result.ServerError(Resource.A_tenant_with_the_same_name_already_exists__Please_choose_a_different_name);

        // Map request to tenant entity.
        var tenant = request.MapToEntity();

        // Set the tenant ID in the tenant resolver.
        _tenantResolver.SetTenantInfo(tenant.Id);

        // Add the new tenant to the database.
        await _dbContext.Tenants.AddAsync(tenant, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        // Create a response object with the new tenant's ID and a success message.
        var response = new CreateTenantResponse
        {
            Id = tenant.Id,
            SuccessMessage = Resource.Tenant_has_been_created_successfully
        };

        // Seed tenant data with shared database strategy.
        var succeeded = await _appSeederService.SeedTenantWithSharedDatabaseStrategy();

        // Check if the identity result was successful.
        if (!succeeded)
            return Envelope<CreateTenantResponse>.Result.ServerError("Something went wrong upon seeding tenant's data.");

        // Return the tenant response.
        return Envelope<CreateTenantResponse>.Result.Ok(response);
    }

    /// <summary>
    /// Handles the creation of a new tenant using the separate database strategy.
    /// </summary>
    /// <param name="request">The create tenant command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An envelope containing the response for the create tenant operation.</returns>
    private async Task<Envelope<CreateTenantResponse>> CreateTenantViaSeparateDatabasePerTenantStrategy(CreateTenantCommand request, CancellationToken cancellationToken)
    {
        // Ensure that the tenant's database exists.
        var databaseCreated = await _dbContext.EnsureTenantDatabaseCreated();

        // Map the request to a tenant entity.
        var tenant = request.MapToEntity();

        // Set the tenant ID in the tenant resolver.
        _tenantResolver.SetTenantInfo(tenant.Id);

        // Seed tenant data with a separate database strategy if necessary.
        if (databaseCreated)
        {
            // Attempt to seed tenant data using a separate database strategy.
            var succeeded = await _appSeederService.SeedTenantWithSeparateDatabaseStrategy();

            // Check if seeding was not successful, and return a server error response if necessary.
            if (!succeeded)
                return Envelope<CreateTenantResponse>.Result.ServerError("Something went wrong upon seeding tenant's data.");
        }

        // Add tenant-specific information to the host database.
        await AddTenantInfoToHostDatabase(cancellationToken, tenant);

        // Create a response object with the new tenant's ID and a success message.
        var response = new CreateTenantResponse
        {
            Id = tenant.Id,
            SuccessMessage = Resource.Tenant_has_been_created_successfully
        };

        // Return the tenant response.
        return Envelope<CreateTenantResponse>.Result.Ok(response);
    }

    /// <summary>
    /// Adds information about a new tenant to the Host database.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token for asynchronous operations.</param>
    /// <param name="tenant">The tenant entity containing information to be added.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task AddTenantInfoToHostDatabase(CancellationToken cancellationToken, Tenant tenant)
    {
        // Set the connection string to the host database (Tenant Administration Portal / TAP).
        _dbContext.TrySwitchToHostDatabase();

        // Add the new tenant to the Host database.
        await _dbContext.Tenants.AddAsync(tenant, cancellationToken);

        // Save changes to persist the new tenant to the Host database.
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Disables password complexity requirements for new tenants.
    /// </summary>
    private void DisablePasswordComplexity()
    {
        _identityOptions.Value.Password.RequireDigit = false;
        _identityOptions.Value.Password.RequireLowercase = false;
        _identityOptions.Value.Password.RequireNonAlphanumeric = false;
        _identityOptions.Value.Password.RequireUppercase = false;
    }

    #endregion Private Methods
}