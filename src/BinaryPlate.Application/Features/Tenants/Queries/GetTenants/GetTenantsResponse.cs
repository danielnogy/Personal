namespace BinaryPlate.Application.Features.Tenants.Queries.GetTenants;

public class GetTenantsResponse
{
    #region Public Properties

    public PagedList<TenantItem> Tenants { get; set; }

    #endregion Public Properties
}