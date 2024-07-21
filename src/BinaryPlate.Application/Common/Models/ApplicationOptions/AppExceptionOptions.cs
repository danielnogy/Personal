namespace BinaryPlate.Application.Common.Models.ApplicationOptions;

/// <summary>
/// Represents the options for handling application exceptions.
/// </summary>
public class AppExceptionOptions
{
    #region Public Fields

    /// <summary>
    /// Gets the name of the section in the configuration file where these options can be found.
    /// </summary>
    public const string Section = "AppExceptionOptions";

    #endregion Public Fields

    #region Public Properties

    /// <summary>
    /// Gets or sets a value indicating whether detailed exception information is enabled.
    /// </summary>
    public bool DetailedExceptionEnabled { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether logging for exceptions is enabled.
    /// </summary>
    public bool LoggingEnabled { get; set; }

    #endregion Public Properties
}