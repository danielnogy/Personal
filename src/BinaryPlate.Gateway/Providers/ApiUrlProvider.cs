namespace BinaryPlate.Gateway.Providers;

public class ApiUrlProvider(IOptions<UrlOptions> optionsSnapshot) : IApiUrlProvider
{
    #region Private Fields

    private readonly UrlOptions _optionsSnapshot = optionsSnapshot.Value;

    #endregion Private Fields

    #region Public Properties

    public string BaseUrl => _optionsSnapshot.BaseApiUrl;

    #endregion Public Properties
}