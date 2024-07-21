namespace BinaryPlate.BlazorPlate.Consumers.HttpClients;

public class ReportsClient(IHttpService httpService) : IReportsClient
{
    #region Public Methods

    public async Task<ApiResponseWrapper<GetReportForEditResponse>> GetReport(GetReportForEditQuery request)
    {
        return await httpService.Post<GetReportForEditQuery, GetReportForEditResponse>("reports/GetReport", request);
    }

    public async Task<ApiResponseWrapper<GetReportsResponse>> GetReports(GetReportsQuery request)
    {
        return await httpService.Post<GetReportsQuery, GetReportsResponse>("reports/GetReports", request);
    }

    #endregion Public Methods
}