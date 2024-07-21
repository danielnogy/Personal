namespace BinaryPlate.Application.Features.MyTenant.Queries.GetMyTenant;

public class GetMyTenantForEditQuery : IRequest<Envelope<GetMyTenantForEditResponse>>
{
    #region Public Classes

    public class GetMyTenantForEditQueryHandler(IApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor) : IRequestHandler<GetMyTenantForEditQuery, Envelope<GetMyTenantForEditResponse>>
    {
        #region Public Methods

        public async Task<Envelope<GetMyTenantForEditResponse>> Handle(GetMyTenantForEditQuery request, CancellationToken cancellationToken)
        {
            // Get the current user's tenant ID based on their user ID retrieved from the HTTP context
            var currentUserTenantId = await dbContext.Users
                                                     .Where(u => u.Id == httpContextAccessor.GetUserId())
                                                     .Select(u => u.TenantId)
                                                     .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            // Switch the database context to the host database.
            dbContext.TrySwitchToHostDatabase();

            // Query the database for a tenant with the given ID.
            var myTenant = await dbContext.Tenants.Where(t => t.Id == currentUserTenantId).FirstOrDefaultAsync(cancellationToken);

            // If no tenant is found with the given ID, return a not found response.
            if (myTenant == null)
                return Envelope<GetMyTenantForEditResponse>.Result.NotFound(Resource.Unable_to_load_tenant);

            // Convert the tenant entity to a response DTO.
            var myTenantForEditResponse = GetMyTenantForEditResponse.MapFromEntity(myTenant);

            // Return a successful response with the converted myTenant DTO.
            return Envelope<GetMyTenantForEditResponse>.Result.Ok(myTenantForEditResponse);

        }

        #endregion Public Methods
    }

    #endregion Public Classes
}