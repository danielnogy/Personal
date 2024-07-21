namespace BinaryPlate.Infrastructure.Contracts.Identity;

/// <summary>
/// This interface represents the contract for generating application built-in permissions.
/// </summary>
public interface IPermissionScanner
{
    #region Public Methods

    /// <summary>
    /// Scans the built-in API endpoints to generate system permissions.
    /// </summary>
    Task InitializeDefaultPermissions();

    #endregion Public Methods
}