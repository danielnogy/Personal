namespace BinaryPlate.Gateway.Consumers;

public class TenantsClient(IHttpService httpService) : ITenantsClient
{
    #region Public Methods

    public async Task<ApiResponseWrapper<CreateTenantResponse>> CreateTenant(CreateTenantCommand request)
    {
        httpService.SetTenantHeader(value: request.Name);

        return await httpService.Post<CreateTenantCommand, CreateTenantResponse>("api/gateway/createTenant", request);
    }

    #endregion Public Methods
}