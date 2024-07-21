﻿namespace BinaryPlate.BlazorPlate.Consumers.HttpClients;

public class DashboardClient(IHttpService httpService) : IDashboardClient
{
    #region Public Methods

    public async Task<ApiResponseWrapper<GetHeadlinesResponse>> GetHeadlinesData()
    {
        return await httpService.Post<GetHeadlinesResponse>("Dashboard/GetHeadlinesData");
    }

    #endregion Public Methods
}