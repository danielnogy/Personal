namespace BinaryPlate.Application.Features.MyTenant.Queries.GetMyTenant;

public class GetMyTenantForEditResponse : AuditableDto
{
    #region Public Properties

    public string Id { get; set; }
    public string Name { get; set; }

    #endregion Public Properties

    #region Public Methods

    public static GetMyTenantForEditResponse MapFromEntity(Tenant myTenant)
    {
        return new GetMyTenantForEditResponse
        {
            Id = myTenant.Id.ToString(),
            Name = myTenant.Name
        };
    }

    #endregion Public Methods
}