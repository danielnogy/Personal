namespace BinaryPlate.Application.Common.Models.ApplicationOptions;

/// <summary>
/// Represents options for configuring app tenant functionality.
/// </summary>
public class AppTenantOptions
{
    #region Public Fields

    /// <summary>
    /// Gets the name of the configuration section where these tenant options can be found.
    /// </summary>
    public const string Section = "AppTenantOptions";

    #endregion Public Fields

    #region Public Properties

    /// <summary>
    /// Gets or sets the tenant mode that determines how the application handles multitenancy.
    /// </summary>
    public int TenantMode { get; set; }

    /// <summary>
    /// Gets or sets the data isolation strategy used for managing data separation between tenants.
    /// </summary>
    public int DataIsolationStrategy { get; set; }

    #endregion Public Properties
}