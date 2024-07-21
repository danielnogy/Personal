namespace BinaryPlate.Gateway.Models;

public class ApiResponseWrapper<T>(T payload, bool isSuccessStatusCode, HttpStatusCode statusCode)
{
    #region Public Constructors

    public ApiResponseWrapper(ApiErrorResponse apiErrorResponse, bool isSuccessStatusCode, HttpStatusCode statusCode) : this(default(T), isSuccessStatusCode, statusCode)
    {
        ApiErrorResponse = apiErrorResponse;
    }

    #endregion Public Constructors

    #region Public Properties

    public bool IsSuccessStatusCode { get; } = isSuccessStatusCode;
    public HttpStatusCode StatusCode { get; set; } = statusCode;
    public T Payload { get; } = payload;
    public ApiErrorResponse ApiErrorResponse { get; }

    #endregion Public Properties
}