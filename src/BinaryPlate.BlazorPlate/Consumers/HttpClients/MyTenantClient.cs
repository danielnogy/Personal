namespace BinaryPlate.BlazorPlate.Consumers.HttpClients;

public class MyTenantClient(IHttpService httpService) : IMyTenantClient
{
    #region Public Methods

    public async Task<ApiResponseWrapper<GetMyTenantForEditResponse>> GetTenant(GetMyTenantForEditQuery request)
    {
        return await httpService.Post<GetMyTenantForEditQuery, GetMyTenantForEditResponse>("MyTenant/GetTenant", request);
    }

    public async Task<ApiResponseWrapper<string>> UpdateTenant(UpdateMyTenantCommand request)
    {
        return await httpService.Put<UpdateMyTenantCommand, string>("MyTenant/UpdateTenant", request);
    }

    #endregion Public Methods
}