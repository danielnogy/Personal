namespace BinaryPlate.Application.Common.Constants;

/// <summary>
/// Represents various connection strings used in the application.
/// </summary>
public class ConnectionString
{
    #region Public Fields

    /// <summary>
    /// Gets the connection string for the host database.
    /// </summary>
    public static readonly string HostConnection = "HostDbConnection";

    /// <summary>
    /// Gets the connection string for multi-tenant separate databases.
    /// </summary>
    public static readonly string MultiTenantSeparateDbConnection = "MultiTenantSeparateDbConnection";

    /// <summary>
    /// Gets the connection string for multi-tenant shared database.
    /// </summary>
    public static readonly string MultiTenantSharedDbConnection = "MultiTenantSharedDbConnection";

    /// <summary>
    /// Gets the connection string for single-tenant database.
    /// </summary>
    public static readonly string SingleTenantDbConnection = "SingleTenantDbConnection";

    /// <summary>
    /// Gets the connection string for Hangfire database.
    /// </summary>
    public static readonly string HangfireDbConnection = "HangfireDbConnection";

    #endregion Public Fields
}