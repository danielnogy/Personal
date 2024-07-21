namespace BinaryPlate.BlazorPlate.Consumers.HttpClients;

public class FileUploadClient(IHttpService httpService) : IFileUploadClient
{
    #region Public Methods

    public async Task<ApiResponseWrapper<UploadFileResponse>> UploadFile(MultipartFormDataContent request)
    {
        return await httpService.PostFormData<MultipartFormDataContent, UploadFileResponse>("fileUpload/uploadFile", request);
    }

    #endregion Public Methods
}