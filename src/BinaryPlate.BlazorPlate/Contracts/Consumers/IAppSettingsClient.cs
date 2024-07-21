namespace BinaryPlate.BlazorPlate.Contracts.Consumers;

/// <summary>
/// Provides methods for interacting with the application settings related to identity, file storage
/// and tokens.
/// </summary>
public interface IAppSettingsClient
{
    #region Public Methods

    /// <summary>
    /// Gets the identity settings for editing.
    /// </summary>
    /// <returns>An <see cref="GetIdentitySettingsForEditResponse"/>.</returns>
    Task<ApiResponseWrapper<GetIdentitySettingsForEditResponse>> GetIdentitySettings();

    /// <summary>
    /// Updates the identity settings.
    /// </summary>
    /// <param name="request">The request containing the updated identity settings.</param>
    /// <returns>An <see cref="UpdateIdentitySettingsResponse"/>.</returns>
    Task<ApiResponseWrapper<UpdateIdentitySettingsResponse>> UpdateIdentitySettings(UpdateIdentitySettingsCommand request);

    /// <summary>
    /// Gets the file storage settings for editing.
    /// </summary>
    /// <returns>A <see cref="GetFileStorageSettingsForEditResponse"/>.</returns>
    Task<ApiResponseWrapper<GetFileStorageSettingsForEditResponse>> GetFileStorageSettings();

    /// <summary>
    /// Updates the file storage settings.
    /// </summary>
    /// <param name="request">The request containing the updated file storage settings.</param>
    /// <returns>A <see cref="UpdateFileStorageSettingsResponse"/>.</returns>
    Task<ApiResponseWrapper<UpdateFileStorageSettingsResponse>> UpdateFileStorageSettings(UpdateFileStorageSettingsCommand request);

    /// <summary>
    /// Gets the token settings for editing.
    /// </summary>
    /// <returns>A <see cref="GetTokenSettingsForEditResponse"/>.</returns>
    Task<ApiResponseWrapper<GetTokenSettingsForEditResponse>> GetTokenSettings();

    /// <summary>
    /// Updates the token settings.
    /// </summary>
    /// <param name="request">The request containing the updated token settings.</param>
    /// <returns>A <see cref="UpdateTokenSettingsResponse"/>.</returns>
    Task<ApiResponseWrapper<UpdateTokenSettingsResponse>> UpdateTokenSettings(UpdateTokenSettingsCommand request);

    #endregion Public Methods
}