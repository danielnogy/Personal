namespace BinaryPlate.Application.Features.MyTenant.Commands.UpdateMyTenant;

public class UpdateMyTenantCommand : IRequest<Envelope<string>>
{
    #region Public Properties

    public string Id { get; set; }
    public string Name { get; set; }
    public string Subdomain { get; set; }
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidUntil { get; set; }

    #endregion Public Properties

    #region Public Methods

    public void MapToEntity(Tenant myTenant)
    {
        if (myTenant == null)
            throw new ArgumentNullException(nameof(myTenant));

        myTenant.Name = Name;
    }

    #endregion Public Methods

    #region Public Classes

    public class UpdateMyTenantCommandHandler(IApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor) : IRequestHandler<UpdateMyTenantCommand, Envelope<string>>
    {
        #region Public Methods

        public async Task<Envelope<string>> Handle(UpdateMyTenantCommand request, CancellationToken cancellationToken)
        {
            // Check if the ID is null or empty, or not a valid GUID.
            if (string.IsNullOrEmpty(request.Id) || !Guid.TryParse(request.Id, out var tenantId))
                return Envelope<string>.Result.BadRequest(Resource.Invalid_tenant_Id);

            // Switch the database context to the Host database (Tenant Administration Portal / TAP).
            dbContext.TrySwitchToHostDatabase();

            // Check if any tenant with the same name already exists in the database, excluding the
            // current tenant.
            var tenantExist = await dbContext.Tenants.AnyAsync(t => t.Id != tenantId && t.Name == request.Name, cancellationToken: cancellationToken);

            // Check if the tenant exists. If so, throw an exception.
            if (tenantExist)
                return Envelope<string>.Result.ServerError(Resource.A_tenant_with_the_same_name_already_exists__Please_choose_a_different_name);

            // Switch the database context to the tenant-specific database.
            dbContext.TrySwitchToTenantDatabase(httpContextAccessor.GetTenantName());

            // Get the current user's tenant ID based on their user ID retrieved from the HTTP context.
            var currentUserTenantId = await dbContext.Users.Where(u => u.Id == httpContextAccessor.GetUserId())
                                                           .Select(u => u.TenantId)
                                                           .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            // Switch the database context back to the Host database (Tenant Administration Portal / TAP).
            dbContext.TrySwitchToHostDatabase();

            // Get the tenant from the database.
            var myTenant = await dbContext.Tenants.Where(t => t.Id == tenantId && t.Id == currentUserTenantId)
                                                  .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            // Check if the tenant is null.
            if (myTenant == null)
                return Envelope<string>.Result.NotFound(Resource.Unable_to_load_tenant);

            // Store the current name of the tenant before any updates.
            var currentTenantName = myTenant.Name;

            // Map the update request to the tenant entity.
            request.MapToEntity(myTenant);

            // Update the tenant information, including database updates if necessary.
            await UpdateTenantInfo(myTenant, currentTenantName, cancellationToken);

            // Return a success message.
            return Envelope<string>.Result.Ok(Resource.Tenant_has_been_updated_successfully);
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Updates tenant information in the database and performs additional actions if necessary.
        /// </summary>
        /// <param name="myTenant">The tenant entity to be updated.</param>
        /// <param name="currentTenantName">The current name of the tenant before any updates.</param>
        /// <param name="cancellationToken">The cancellation token for asynchronous operations.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task UpdateTenantInfo(Tenant myTenant, string currentTenantName, CancellationToken cancellationToken)
        {
            // Update the tenant entity in the database context.
            dbContext.Tenants.Update(myTenant);

            // Save changes to persist the updated tenant to the database.
            await dbContext.SaveChangesAsync(cancellationToken);

            // Attempt to rename the tenant's database.
            await dbContext.TryRenameTenantDatabase(currentTenantName, myTenant.Name, cancellationToken);
        }

        #endregion Private Methods
    }

    #endregion Public Classes
}