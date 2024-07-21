namespace BinaryPlate.Infrastructure.Contracts.MultiTenancy;

/// <summary>
/// This interface represents the contract for handling tenant-specific operations.
/// </summary>
public interface ITenantInterceptor
{
    #region Public Properties

    /// <summary>
    /// Gets the current tenant mode.
    /// </summary>
    TenantMode TenantMode { get; }

    #endregion Public Properties

    #region Public Methods

    /// <summary>
    /// Handles tenant-specific operations for the given <see cref="HttpContext"/>.
    /// </summary>
    /// <param name="httpContext">The <see cref="HttpContext"/> for the current request.</param>
    /// <param name="dbContext">The application database context.</param>
    /// <param name="tenantResolver">The tenant resolver service.</param>
    void Handle(HttpContext httpContext, IApplicationDbContext dbContext, ITenantResolver tenantResolver);

    #endregion Public Methods
}