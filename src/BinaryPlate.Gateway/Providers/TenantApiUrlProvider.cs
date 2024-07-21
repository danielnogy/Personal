namespace BinaryPlate.Gateway.Providers;

public class TenantUrlProvider(IOptionsSnapshot<UrlOptions> optionsSnapshot)
{
    #region Private Fields

    private readonly UrlOptions _optionsSnapshot = optionsSnapshot.Value;

    #endregion Private Fields

    #region Public Properties

    public string TenantUrl => _optionsSnapshot.TenantUrl;

    #endregion Public Properties
}