namespace BinaryPlate.BlazorPlate.Consumers.HttpClients;

public class TenantsClient(IHttpService httpService, AppStateManager appStateManager) : ITenantsClient
{
    #region Public Methods

    public async Task<ApiResponseWrapper<GetTenantForEditResponse>> GetTenant(GetTenantForEditQuery request)
    {
        return await httpService.Post<GetTenantForEditQuery, GetTenantForEditResponse>("tenants/GetTenant", request);
    }

    public async Task<ApiResponseWrapper<GetTenantsResponse>> GetTenants(GetTenantsQuery request)
    {
        return await httpService.Post<GetTenantsQuery, GetTenantsResponse>("tenants/GetTenants", request);
    }

    public async Task<ApiResponseWrapper<CreateTenantResponse>> CreateTenant(CreateTenantCommand request)
    {
        appStateManager.SetHttpCustomHeader(key: "BP-TenantCreatedByHost", value: request.Name);

        return await httpService.Post<CreateTenantCommand, CreateTenantResponse>("tenants/CreateTenant", request, namedHttpClient: NamedHttpClient.CustomHeadersHttpClient);
    }

    public async Task<ApiResponseWrapper<string>> UpdateTenant(UpdateTenantCommand request)
    {
        return await httpService.Put<UpdateTenantCommand, string>("tenants/UpdateTenant", request);
    }

    public async Task<ApiResponseWrapper<string>> DeleteTenant(string id)
    {
        return await httpService.Delete<string>($"tenants/DeleteTenant?id={id}");
    }

    #endregion Public Methods
}