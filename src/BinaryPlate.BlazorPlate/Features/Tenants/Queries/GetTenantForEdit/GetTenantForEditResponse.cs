namespace BinaryPlate.BlazorPlate.Features.Tenants.Queries.GetTenantForEdit;

public class GetTenantForEditResponse : AuditableDto
{
    #region Public Properties

    public string Id { get; set; }
    public string Name { get; set; }
    public bool Enabled { get; set; }
    public string ConcurrencyStamp { get; set; }

    #endregion Public Properties
}