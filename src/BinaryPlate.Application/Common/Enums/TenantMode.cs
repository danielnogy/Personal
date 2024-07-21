namespace BinaryPlate.Application.Common.Enums;

/// <summary>
/// Represents the enumeration for specifying the tenant mode of the application.
/// </summary>
public enum TenantMode
{
    /// <summary>
    /// Indicates that the application is configured to only support a single tenant.
    /// </summary>
    SingleTenant = 1,

    /// <summary>
    /// Indicates that the application is configured to support multiple tenants.
    /// </summary>
    MultiTenant = 2,
}
