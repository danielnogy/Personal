﻿namespace BinaryPlate.Application.Common.Contracts.Infrastructure.Storage;

/// <summary>
/// This interface represents the contract for creating an instance of the required storage provider.
/// </summary>
public interface IStorageProvider
{
    #region Public Methods

    /// <summary>
    /// Invokes an instance of the file storage service based on the configured storage type.
    /// </summary>
    /// <returns>An instance of the file storage service.</returns>
    Task<IFileStorageService> InvokeStorageService();

    #endregion Public Methods
}