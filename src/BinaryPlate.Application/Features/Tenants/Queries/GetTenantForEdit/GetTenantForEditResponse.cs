namespace BinaryPlate.Application.Features.Tenants.Queries.GetTenantForEdit;

public class GetTenantForEditResponse : AuditableDto
{
    #region Public Properties

    public string Id { get; set; }
    public string Name { get; set; }
    public bool Enabled { get; set; }
    public string ConcurrencyStamp { get; set; }

    #endregion Public Properties

    #region Public Methods

    public static GetTenantForEditResponse MapFromEntity(Tenant tenant)
    {
        return new GetTenantForEditResponse
        {
            Id = tenant.Id.ToString(),
            Name = tenant.Name,
            Enabled = tenant.Enabled,
            ConcurrencyStamp = tenant.ConcurrencyStamp
        };
    }

    #endregion Public Methods
}