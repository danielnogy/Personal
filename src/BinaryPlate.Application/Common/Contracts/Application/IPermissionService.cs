﻿namespace BinaryPlate.Application.Common.Contracts.Application;

/// <summary>
/// This interface represents the contract for managing permissions.
/// </summary>
public interface IPermissionService
{
    #region Public Methods

    /// <summary>
    /// Retrieves permissions on demand based on the specified query.
    /// </summary>
    /// <param name="request">The query specifying the permissions to retrieve.</param>
    /// <returns>An envelope containing the permissions response.</returns>
    Task<Envelope<GetPermissionsResponse>> GetPermissionsOnDemand(GetPermissionsQuery request);

    /// <summary>
    /// Retrieves all permissions.
    /// </summary>
    /// <returns>An envelope containing the permissions response.</returns>
    Task<Envelope<GetPermissionsResponse>> GetAllPermissions();

    /// <summary>
    /// Retrieves the list of non-excluded permissions for the specified user.
    /// </summary>
    /// <param name="user">The user to retrieve permissions for.</param>
    /// <returns>A list of permission items.</returns>
    Task<List<PermissionItem>> GetUserPermissionsExceptExcluded(ApplicationUser user);

    #endregion Public Methods
}