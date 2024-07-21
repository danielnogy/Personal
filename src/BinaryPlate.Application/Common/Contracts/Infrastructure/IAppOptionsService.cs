namespace BinaryPlate.Application.Common.Contracts.Infrastructure;

/// <summary>
/// This interface represents the contract for accessing to the global configuration settings for the application stored in appsettings.json.
/// </summary>
public interface IAppOptionsService
{
    #region Public Methods

    /// <summary>
    /// Retrieves the options related to the client application.
    /// </summary>
    /// <returns>An instance of <see cref="AppClientOptions"/> containing client application configurations.</returns>
    AppClientOptions GetAppClientOptions();

    /// <summary>
    /// Retrieves the options related to JSON Web Tokens (JWT).
    /// </summary>
    /// <returns>An instance of <see cref="AppJwtOptions"/> containing JWT configurations.</returns>
    AppJwtOptions GetAppJwtOptions();

    /// <summary>
    /// Retrieves the options related to the Simple Mail Transfer Protocol (SMTP) configuration.
    /// </summary>
    /// <returns>An instance of <see cref="AppMailSenderOptions"/> containing SMTP configurations.</returns>
    AppMailSenderOptions GetAppMailSenderOptions();

    /// <summary>
    /// Retrieves the options related to the application's user settings.
    /// </summary>
    /// <returns>An instance of <see cref="AppUserOptions"/> containing user settings configurations.</returns>
    AppUserOptions GetAppUserOptions();

    /// <summary>
    /// Retrieves the options related to user account lockout policies.
    /// </summary>
    /// <returns>An instance of <see cref="AppLockoutOptions"/> containing lockout policies configurations.</returns>
    AppLockoutOptions GetAppLockoutOptions();

    /// <summary>
    /// Retrieves the options related to user account password policies.
    /// </summary>
    /// <returns>An instance of <see cref="AppPasswordOptions"/> containing password policies configurations.</returns>
    AppPasswordOptions GetAppPasswordOptions();

    /// <summary>
    /// Retrieves the options related to user account sign-in settings.
    /// </summary>
    /// <returns>An instance of <see cref="AppSignInOptions"/> containing sign-in settings configurations.</returns>
    AppSignInOptions GetAppSignInOptions();

    /// <summary>
    /// Retrieves the options related to user access tokens and refresh tokens.
    /// </summary>
    /// <returns>An instance of <see cref="AppTokenOptions"/> containing token configurations.</returns>
    AppTokenOptions GetAppTokenOptions();

    /// <summary>
    /// Retrieves the options related to file storage for the application.
    /// </summary>
    /// <returns>An instance of <see cref="AppFileStorageOptions"/> containing file storage configurations.</returns>
    AppFileStorageOptions GetAppFileStorageOptions();

    /// <summary>
    /// Retrieves the options related to application multi-tenancy configurations.
    /// </summary>
    /// <returns>An instance of <see cref="AppTenantOptions"/> containing multi-tenancy configurations.</returns>
    AppTenantOptions GetAppTenantOptions();

    /// <summary>
    /// Retrieves the options related to application exception handling configurations.
    /// </summary>
    /// <returns>An instance of <see cref="AppExceptionOptions"/> containing exception handling configurations.</returns>
    AppExceptionOptions GetAppExceptionOptions();

    /// <summary>
    /// Retrieves the subdomain associated with the current request.
    /// </summary>
    /// <returns>The subdomain as a string.</returns>
    string GetSubDomain();

    #endregion Public Methods
}