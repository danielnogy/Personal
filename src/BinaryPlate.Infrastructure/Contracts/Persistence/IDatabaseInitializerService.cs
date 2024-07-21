namespace BinaryPlate.Infrastructure.Contracts.Persistence;

/// <summary>
/// This interface represents the contract for initializing databases.
/// </summary>
public interface IDatabaseInitializerService
{
    #region Public Methods

    /// <summary>
    /// Creates a single-tenant database.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task CreateSingleTenantDatabase();

    /// <summary>
    /// Initializes a multi-tenant database with the specified data isolation strategy.
    /// </summary>
    /// <param name="dataIsolationStrategy">The data isolation strategy for the database.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task InitializeMultiTenantDatabase(DataIsolationStrategy dataIsolationStrategy);

    #endregion Public Methods
}