namespace BinaryPlate.Application.Common.Models.ApplicationOptions;

/// <summary>
/// Represents a set of configuration options for the application.
/// </summary>
public class AppOptions
{
    #region Public Fields

    /// <summary>
    /// Gets the name of the configuration section where these options can be found.
    /// </summary>
    public const string Section = "AppOptions";

    #endregion Public Fields

    #region Public Properties

    /// <summary>
    /// Gets or sets the client options for the application.
    /// </summary>
    /// <returns>An instance of <see cref="AppClientOptions"/> containing client options.</returns>
    public AppClientOptions AppClientOptions { get; set; }

    /// <summary>
    /// Gets or sets the JWT (JSON Web Token) options for the application.
    /// </summary>
    /// <returns>An instance of <see cref="AppJwtOptions"/> containing JWT options.</returns>
    public AppJwtOptions AppJwtOptions { get; set; }

    /// <summary>
    /// Gets or sets the mail sender options for the application.
    /// </summary>
    /// <returns>An instance of <see cref="AppMailSenderOptions"/> containing mail sender options.</returns>
    public AppMailSenderOptions AppMailSenderOptions { get; set; }

    /// <summary>
    /// Gets or sets the identity options for the application.
    /// </summary>
    /// <returns>An instance of <see cref="AppIdentityOptions"/> containing identity options.</returns>
    public AppIdentityOptions AppIdentityOptions { get; set; }

    /// <summary>
    /// Gets or sets the token options for the application.
    /// </summary>
    /// <returns>An instance of <see cref="AppTokenOptions"/> containing token options.</returns>
    public AppTokenOptions AppTokenOptions { get; set; }

    /// <summary>
    /// Gets or sets the file storage options for the application.
    /// </summary>
    /// <returns>An instance of <see cref="AppFileStorageOptions"/> containing file storage options.</returns>
    public AppFileStorageOptions AppFileStorageOptions { get; set; }

    /// <summary>
    /// Gets or sets the tenant options for the application.
    /// </summary>
    /// <returns>An instance of <see cref="AppTenantOptions"/> containing tenant options.</returns>
    public AppTenantOptions AppTenantOptions { get; set; }

    /// <summary>
    /// Gets or sets the exception handling options for the application.
    /// </summary>
    /// <returns>An instance of <see cref="AppExceptionOptions"/> containing exception handling options.</returns>
    public AppExceptionOptions AppExceptionOptions { get; set; }

    #endregion Public Properties
}