namespace BinaryPlate.Application.Features.MyTenant.Commands.DeleteMyTenant;

public class DeleteMyTenantCommand : IRequest<Envelope<string>>
{
    #region Public Properties

    public string Id { get; set; }

    #endregion Public Properties

    #region Public Classes

    public class DeleteMyTenantCommandHandler(IApplicationDbContext dbContext, ITenantResolver tenantResolver) : IRequestHandler<DeleteMyTenantCommand, Envelope<string>>
    {
        #region Public Methods

        public async Task<Envelope<string>> Handle(DeleteMyTenantCommand request, CancellationToken cancellationToken)
        {
            // Check if the ID is null or empty, or not a valid GUID.
            if (string.IsNullOrEmpty(request.Id) || !Guid.TryParse(request.Id, out var tenantId))
                return Envelope<string>.Result.BadRequest(Resource.Invalid_tenant_Id);

            // Switch the database context to the Host database (Tenant Administration Portal / TAP).
            dbContext.TrySwitchToHostDatabase();

            // Find the tenant with the provided Id.
            var tenant = await dbContext.Tenants.Where(t => t.Id == tenantId && t.Id == tenantResolver.GetTenantId()).FirstOrDefaultAsync(cancellationToken: cancellationToken);

            // If no tenant with the provided Id is found, return a Not Found response.
            if (tenant == null)
                return Envelope<string>.Result.NotFound(Resource.The_tenant_is_not_found);

            // Remove the tenant from the database.
            dbContext.Tenants.Remove(tenant);

            // Save the changes to the database.
            await dbContext.SaveChangesAsync(cancellationToken);

            // Return a successful response message.
            return Envelope<string>.Result.Ok(Resource.Tenant_has_been_deleted_successfully);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}