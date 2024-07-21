namespace BinaryPlate.Infrastructure.Contracts.Persistence;

/// <summary>
/// Interface defining helper methods for configuring Entity Framework DbContext.
/// </summary>
public interface IDbContextHelperService
{
    #region Public Methods

    /// <summary>
    /// Configures entities to support concurrency check.
    /// </summary>
    void ConfigureConcurrentEntities(ModelBuilder builder);

    /// <summary>
    /// Configures entities related to multi-tenancy.
    /// </summary>
    void ConfigureMultiTenantsEntities(ModelBuilder builder);

    /// <summary>
    /// Configures entities for storing settings in a separate schema.
    /// </summary>
    void ConfigureSettingsSchemaEntities(ModelBuilder builder);

    /// <summary>
    /// Configures entities to support soft deletion.
    /// </summary>
    void ConfigureSoftDeletableEntities(ModelBuilder builder);

    /// <summary>
    /// Removes the configuration related to ITenant entities from the specified ModelBuilder.
    /// This method is used to undo or deconfigure any settings associated with ITenant entities.
    /// </summary>
    /// <param name="builder">The ModelBuilder to which the configuration changes will be applied.</param>
    void DeconfigureITenantEntities(ModelBuilder builder);


    /// <summary>
    /// Performs data management actions on the provided DbContext.
    /// </summary>
    /// <param name="context">The DbContext on which data management actions are to be performed.</param>
    Task PerformDataManagementActions(DbContext context);

    #endregion Public Methods
}