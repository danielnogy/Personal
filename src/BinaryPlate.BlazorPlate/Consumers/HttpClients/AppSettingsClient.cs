namespace BinaryPlate.BlazorPlate.Consumers.HttpClients;

public class AppSettingsClient(IHttpService httpService) : IAppSettingsClient
{
    #region Public Methods

    public async Task<ApiResponseWrapper<GetIdentitySettingsForEditResponse>> GetIdentitySettings()
    {
        return await httpService.Get<GetIdentitySettingsForEditResponse>("AppSettings/GetIdentitySettings");
    }

    public async Task<ApiResponseWrapper<UpdateIdentitySettingsResponse>> UpdateIdentitySettings(UpdateIdentitySettingsCommand request)
    {
        return await httpService.Put<UpdateIdentitySettingsCommand, UpdateIdentitySettingsResponse>("AppSettings/UpdateIdentitySettings", request);
    }

    public async Task<ApiResponseWrapper<GetFileStorageSettingsForEditResponse>> GetFileStorageSettings()
    {
        return await httpService.Get<GetFileStorageSettingsForEditResponse>("AppSettings/GetFileStorageSettings");
    }

    public async Task<ApiResponseWrapper<UpdateFileStorageSettingsResponse>> UpdateFileStorageSettings(UpdateFileStorageSettingsCommand request)
    {
        return await httpService.Put<UpdateFileStorageSettingsCommand, UpdateFileStorageSettingsResponse>("AppSettings/UpdateFileStorageSettings", request);
    }

    public async Task<ApiResponseWrapper<GetTokenSettingsForEditResponse>> GetTokenSettings()
    {
        return await httpService.Get<GetTokenSettingsForEditResponse>("AppSettings/GetTokenSettings");
    }

    public async Task<ApiResponseWrapper<UpdateTokenSettingsResponse>> UpdateTokenSettings(UpdateTokenSettingsCommand request)
    {
        return await httpService.Put<UpdateTokenSettingsCommand, UpdateTokenSettingsResponse>("AppSettings/UpdateTokenSettings", request);
    }

    #endregion Public Methods
}