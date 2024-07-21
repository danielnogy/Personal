namespace BinaryPlate.BlazorPlate.Features.MyTenant.Queries.GetTenantForEdit;

public class GetMyTenantForEditResponse : AuditableDto
{
    #region Public Properties

    public string Id { get; set; }
    public string Name { get; set; }

    #endregion Public Properties
}