namespace BinaryPlate.Gateway.Services;

public class HttpService(HttpClient httpClient) : IHttpService
{
    #region Public Properties

    public TenantHeader TenantHeader { get; private set; } = new();

    #endregion Public Properties

    #region Private Properties

    private static JsonSerializerOptions DefaultJsonSerializerOptions => new()
    {
        PropertyNameCaseInsensitive = true
    };

    #endregion Private Properties

    #region Public Methods

    public void SetTenantHeader(string value)
    {
        TenantHeader = new TenantHeader() { Key = "BP-TenantCreatedByGateway", Value = value };
    }

    public async Task<ApiResponseWrapper<TResponse>> Post<TRequest, TResponse>(string url, TRequest data)
    {
        httpClient.DefaultRequestHeaders.Clear();

        httpClient.DefaultRequestHeaders.Add("Accept-Language", CultureInfo.CurrentCulture.ToString());

        httpClient.DefaultRequestHeaders.Add(TenantHeader.Key, TenantHeader.Value);

        var dataJson = JsonSerializer.Serialize(data);

        var stringContent = new StringContent(dataJson, Encoding.UTF8, "application/json");

        using var response = await httpClient.PostAsync(url, stringContent);

        return await ProcessResponse<TResponse>(response);
    }

    #endregion Public Methods

    #region Private Methods

    private static async Task<ApiResponseWrapper<TResponse>> ProcessResponse<TResponse>(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            var responseDeserialized = await response.Content.ReadFromJsonAsync<SuccessResult<TResponse>>(DefaultJsonSerializerOptions).ConfigureAwait(false);

            if (responseDeserialized != null)
                return new ApiResponseWrapper<TResponse>(responseDeserialized.Payload, response.IsSuccessStatusCode, response.StatusCode);

            throw new ArgumentNullException($"ProcessResponse: {nameof(responseDeserialized)} cannot be NULL.");
        }
        else
        {
            var responseDeserialized = await response.Content.ReadFromJsonAsync<ApiErrorResponse>(DefaultJsonSerializerOptions).ConfigureAwait(false);

            return new ApiResponseWrapper<TResponse>(responseDeserialized, response.IsSuccessStatusCode, response.StatusCode);
        }
    }

    #endregion Private Methods
}