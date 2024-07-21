namespace BinaryPlate.Application.Common.Contracts.Infrastructure;

/// <summary>
/// This interface represents the contract for seeding the application's database with initial data.
/// </summary>

public interface IAppSeederService
{
    #region Public Methods

    /// <summary>
    /// Seeds the tenant database using a shared database strategy.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation and indicating whether the seeding was successful.</returns>
    Task<bool> SeedTenantWithSharedDatabaseStrategy();

    /// <summary>
    /// Seeds the tenant database using a separate database strategy.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation and indicating whether the seeding was successful.</returns>
    Task<bool> SeedTenantWithSeparateDatabaseStrategy();

    /// <summary>
    /// Seeds the database in single-tenant mode.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation and indicating whether the seeding was successful.</returns>
    Task<bool> SeedSingleTenantModeDatabase();

    /// <summary>
    /// Seeds the host database.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation and indicating whether the seeding was successful.</returns>
    Task<bool> SeedHostDatabase();

    #endregion Public Methods
}