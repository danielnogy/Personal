namespace BinaryPlate.Application.Common.Models.Persistence;

public class ConnectionString
{
    #region Public Fields

    public static readonly string HostConnection = "HostDbConnection";
    public static readonly string MultiTenantSeparateDbConnection = "MultiTenantSeparateDbConnection";
    public static readonly string MultiTenantSharedDbConnection = "MultiTenantSharedDbConnection";
    public static readonly string SingleTenantDbConnection = "SingleTenantDbConnection";
    public static readonly string HangfireDbConnection = "HangfireDbConnection";

    #endregion Public Fields
}