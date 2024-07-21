namespace BinaryPlate.Application.Common.Enums;

/// <summary>
/// Represents the enumeration for specifying the data isolation strategy.
/// </summary>
public enum DataIsolationStrategy
{
    /// <summary>
    /// Indicates that a shared database strategy is used for all tenants.
    /// </summary>
    SharedDatabaseForAllTenants = 1,

    /// <summary>
    /// Indicates that a separate database strategy is used for each tenant.
    /// </summary>
    SeparateDatabasePerTenant = 2
}
