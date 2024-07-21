namespace BinaryPlate.BlazorPlate.Models;

public abstract class NamedHttpClient
{
    #region Public Fields

    public const string OAuthClient = "OAuthClient";
    public const string DefaultClient = "DefaultClient";
    public const string TwoFactorAuthClient = "TwoFactorAuth";
    public const string CustomHeadersHttpClient = "CustomHeadersHttpClient";

    #endregion Public Fields
}