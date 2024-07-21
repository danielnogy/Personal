namespace BinaryPlate.BlazorPlate.Consumers.HttpClients;

public class ApplicantsClient(IHttpService httpService) : IApplicantsClient
{
    #region Public Methods

    public async Task<ApiResponseWrapper<GetApplicantForEditResponse>> GetApplicant(GetApplicantForEditQuery request)
    {
        return await httpService.Post<GetApplicantForEditQuery, GetApplicantForEditResponse>("applicants/GetApplicant", request, namedHttpClient: NamedHttpClient.TwoFactorAuthClient);
    }

    public async Task<ApiResponseWrapper<GetApplicantReferencesResponse>> GetApplicantReferences(GetApplicantReferencesQuery getApplicantReferencesQuery)
    {
        return await httpService.Post<GetApplicantReferencesQuery, GetApplicantReferencesResponse>("applicants/GetApplicantReferences", getApplicantReferencesQuery);
    }

    public async Task<ApiResponseWrapper<GetApplicantsResponse>> GetApplicants(GetApplicantsQuery request)
    {
        return await httpService.Post<GetApplicantsQuery, GetApplicantsResponse>("applicants/GetApplicants", request, namedHttpClient: NamedHttpClient.TwoFactorAuthClient);
    }

    public async Task<ApiResponseWrapper<CreateApplicantResponse>> CreateApplicant(CreateApplicantCommand request)
    {
        return await httpService.Post<CreateApplicantCommand, CreateApplicantResponse>("applicants/CreateApplicant", request, namedHttpClient: NamedHttpClient.TwoFactorAuthClient);
    }

    public async Task<ApiResponseWrapper<string>> UpdateApplicant(UpdateApplicantCommand request)
    {
        return await httpService.Put<UpdateApplicantCommand, string>("applicants/UpdateApplicant", request, namedHttpClient: NamedHttpClient.TwoFactorAuthClient);
    }

    public async Task<ApiResponseWrapper<string>> DeleteApplicant(string id)
    {
        return await httpService.Delete<string>($"applicants/DeleteApplicant?id={id}", namedHttpClient: NamedHttpClient.TwoFactorAuthClient);
    }

    public async Task<ApiResponseWrapper<ExportApplicantsResponse>> ExportAsPdf(ExportApplicantsQuery request)
    {
        return await httpService.Post<ExportApplicantsQuery, ExportApplicantsResponse>("applicants/ExportAsPdf", request, namedHttpClient: NamedHttpClient.TwoFactorAuthClient);
    }

    #endregion Public Methods
}