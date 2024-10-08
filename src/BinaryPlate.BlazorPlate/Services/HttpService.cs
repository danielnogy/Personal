﻿namespace BinaryPlate.BlazorPlate.Services;

public class HttpService(IHttpClientFactory httpClientFactory) : IHttpService
{
    #region Public Properties

    public HttpCustomHeader HttpCustomHeader { get; private set; } = new();

    #endregion Public Properties

    #region Private Properties

    private static JsonSerializerOptions DefaultJsonSerializerOptions => new()
    {
        PropertyNameCaseInsensitive = true
    };

    #endregion Private Properties

    #region Public Methods

    public async Task<ApiResponseWrapper<TResponse>> Get<TResponse>(string url, string namedHttpClient = NamedHttpClient.DefaultClient)
    {
        using var httpClient = httpClientFactory.CreateClient(namedHttpClient);

        using var response = await httpClient.GetAsync(url).ConfigureAwait(false);

        return await ProcessResponse<TResponse>(response);
    }

    public async Task<ApiResponseWrapper<TResponse>> Get<TRequest, TResponse>(string url, TRequest data, string namedHttpClient = NamedHttpClient.DefaultClient)
    {
        var dataJson = JsonSerializer.Serialize(data);

        var stringContent = new StringContent(dataJson, Encoding.UTF8, "application/json");

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(url),
            Content = stringContent,
        };

        using var httpClient = httpClientFactory.CreateClient(namedHttpClient);

        using var response = await httpClient.SendAsync(request).ConfigureAwait(false);

        return await ProcessResponse<TResponse>(response);
    }

    public async Task<ApiResponseWrapper<TResponse>> Post<TRequest, TResponse>(string url, TRequest data, string namedHttpClient = NamedHttpClient.DefaultClient)
    {
        using var httpClient = httpClientFactory.CreateClient(namedHttpClient);

        var dataJson = JsonSerializer.Serialize(data);

        var stringContent = new StringContent(dataJson, Encoding.UTF8, "application/json");

        using var response = await httpClient.PostAsync(url, stringContent).ConfigureAwait(false);

        return await ProcessResponse<TResponse>(response);
    }

    public async Task<ApiResponseWrapper<TResponse>> Post<TResponse>(string url, string namedHttpClient = NamedHttpClient.DefaultClient)
    {
        using var httpClient = httpClientFactory.CreateClient(namedHttpClient);

        using var response = await httpClient.PostAsync(url, null).ConfigureAwait(false);

        return await ProcessResponse<TResponse>(response);
    }

    public async Task<ApiResponseWrapper<TResponse>> PostFormData<TRequest, TResponse>(string url, MultipartFormDataContent data, string namedHttpClient = NamedHttpClient.DefaultClient, CancellationToken cancellationToken = default)
    {
        using var httpClient = httpClientFactory.CreateClient(namedHttpClient);

        using var response = await httpClient.PostAsync(url, data, cancellationToken: cancellationToken).ConfigureAwait(false);

        return await ProcessResponse<TResponse>(response);
    }

    public async Task<ApiResponseWrapper<TResponse>> Put<TRequest, TResponse>(string url, TRequest data, string namedHttpClient = NamedHttpClient.DefaultClient)
    {
        using var httpClient = httpClientFactory.CreateClient(namedHttpClient);

        var dataJson = JsonSerializer.Serialize(data);

        var stringContent = new StringContent(dataJson, Encoding.UTF8, "application/json");

        using var response = await httpClient.PutAsync(url, stringContent);

        return await ProcessResponse<TResponse>(response);
    }

    public async Task<ApiResponseWrapper<TResponse>> PutFormData<TRequest, TResponse>(string url, MultipartFormDataContent data, string namedHttpClient = NamedHttpClient.DefaultClient, CancellationToken cancellationToken = default)
    {
        using var httpClient = httpClientFactory.CreateClient(namedHttpClient);

        using var response = await httpClient.PutAsync(url, data, cancellationToken: cancellationToken).ConfigureAwait(false);

        return await ProcessResponse<TResponse>(response);
    }

    public async Task<ApiResponseWrapper<TResponse>> Delete<TResponse>(string url, string namedHttpClient = NamedHttpClient.DefaultClient)
    {
        using var httpClient = httpClientFactory.CreateClient(namedHttpClient);

        using var response = await httpClient.DeleteAsync(url);

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