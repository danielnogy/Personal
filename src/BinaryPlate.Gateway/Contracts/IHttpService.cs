namespace BinaryPlate.Gateway.Contracts;

public interface IHttpService
{
    #region Public Methods

    Task<ApiResponseWrapper<TResponse>> Post<TRequest, TResponse>(string url, TRequest data);

    void SetTenantHeader(string value);

    #endregion Public Methods
}