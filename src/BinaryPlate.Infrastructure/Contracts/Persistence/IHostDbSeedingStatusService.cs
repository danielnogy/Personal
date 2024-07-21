namespace BinaryPlate.Infrastructure.Contracts.Persistence;

/// <summary>
/// This interface represents the contract for managing seeding status of the host database.
/// </summary>
public interface IHostDbSeedingStatusService
{
    #region Public Methods

    /// <summary>
    /// Updates the seeding status for the host database based on the specified target.
    /// </summary>
    /// <param name="databaseType">The target database to switch to.</param>
    void UpdateHostDbSeedingStatus(DatabaseType databaseType);

    /// <summary>
    /// Checks if there is a pending seeding operation for the host database.
    /// </summary>
    /// <returns>True if a seeding operation for the host database is pending; otherwise, false.</returns>
    bool GetHostDbSeedingPendingStatus();

    #endregion Public Methods
}