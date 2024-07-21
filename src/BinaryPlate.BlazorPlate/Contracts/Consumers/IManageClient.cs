﻿namespace BinaryPlate.BlazorPlate.Contracts.Consumers;

/// <summary>
/// Provides methods for managing the current logged in user account.
/// </summary>
public interface IManageClient
{
    #region Public Methods

    /// <summary>
    /// Gets the current user.
    /// </summary>
    /// <returns>A <see cref="GetCurrentUserForEditResponse"/>.</returns>
    Task<ApiResponseWrapper<GetCurrentUserForEditResponse>> GetUser();

    /// <summary>
    /// Gets the current user's avatar.
    /// </summary>
    /// <returns>A <see cref="GetUserAvatarForEditResponse"/>.</returns>
    Task<ApiResponseWrapper<GetUserAvatarForEditResponse>> GetUserAvatar();

    /// <summary>
    /// Updates the current user's profile.
    /// </summary>
    /// <param name="request">The user profile data to update.</param>
    /// <returns>A success message.</returns>
    Task<ApiResponseWrapper<string>> UpdateUserProfile(UpdateUserProfileCommand request);

    /// <summary>
    /// Updates the current user's avatar.
    /// </summary>
    /// <param name="request">The user avatar data to update.</param>
    /// <returns>A success message.</returns>
    Task<ApiResponseWrapper<string>> UpdateUserAvatar(UpdateUserAvatarCommand request);

    /// <summary>
    /// Changes the current user's email.
    /// </summary>
    /// <param name="request">The email data to change to.</param>
    /// <returns>A <see cref="ChangeEmailResponse"/>.</returns>
    Task<ApiResponseWrapper<ChangeEmailResponse>> ChangeEmail(ChangeEmailCommand request);

    /// <summary>
    /// Confirms the current user's email change.
    /// </summary>
    /// <param name="request">The confirmation data for the email change.</param>
    /// <returns>A <see cref="ChangeEmailResponse"/>.</returns>
    Task<ApiResponseWrapper<ChangeEmailResponse>> ConfirmEmailChange(ConfirmEmailChangeCommand request);

    /// <summary>
    /// Changes the current user's password.
    /// </summary>
    /// <param name="request">The password data to change to.</param>
    /// <returns>A <see cref="ChangePasswordResponse"/>.</returns>
    Task<ApiResponseWrapper<ChangePasswordResponse>> ChangePassword(ChangePasswordCommand request);

    /// <summary>
    /// Set a new password for the current user.
    /// </summary>
    /// <param name="request">The password data to change to.</param>
    /// <returns>A <see cref="SetPasswordResponse"/>.</returns>
    Task<ApiResponseWrapper<SetPasswordResponse>> SetPassword(SetPasswordCommand request);

    /// <summary>
    /// Gets the current user's 2-factor authentication state.
    /// </summary>
    /// <returns>A <see cref="GetTwoFactorAuthenticationStateResponse"/>.</returns>
    Task<ApiResponseWrapper<GetTwoFactorAuthenticationStateResponse>> Get2FaState();

    /// <summary>
    /// Retrieves the shared key and QR code URI for enabling two-factor authentication.
    /// </summary>
    /// <returns>A <see cref="LoadSharedKeyAndQrCodeUriResponse"/>.</returns>
    Task<ApiResponseWrapper<LoadSharedKeyAndQrCodeUriResponse>> LoadSharedKeyAndQrCodeUri();

    /// <summary>
    /// Enables two-factor authentication for the current user.
    /// </summary>
    /// <param name="request">
    /// The <see cref="EnableAuthenticatorCommand"/> containing the two-factor authentication code.
    /// </param>
    /// <returns>A <see cref="EnableAuthenticatorResponse"/>.</returns>
    Task<ApiResponseWrapper<EnableAuthenticatorResponse>> EnableAuthenticator(EnableAuthenticatorCommand request);

    /// <summary>
    /// Disables two-factor authentication for the current user.
    /// </summary>
    /// <returns>A success message.</returns>
    Task<ApiResponseWrapper<string>> Disable2Fa();

    /// <summary>
    /// Generates a set of recovery codes for the current user.
    /// </summary>
    /// <returns>A <see cref="GenerateRecoveryCodesResponse"/>.</returns>
    Task<ApiResponseWrapper<GenerateRecoveryCodesResponse>> GenerateRecoveryCodes();

    /// <summary>
    /// Checks the two-factor authentication state for the current user.
    /// </summary>
    /// <returns>A <see cref="GetUser2FaStateResponse"/>.</returns>
    Task<ApiResponseWrapper<GetUser2FaStateResponse>> CheckUser2FaState();

    /// <summary>
    /// Resets the authenticator app key for the current user.
    /// </summary>
    /// <returns>A <see cref="ResetAuthenticatorResponse"/>.</returns>
    Task<ApiResponseWrapper<ResetAuthenticatorResponse>> ResetAuthenticator();

    /// <summary>
    /// Downloads personal data for the current user.
    /// </summary>
    /// <returns>A <see cref="DownloadPersonalDataResponse"/>.</returns>
    Task<ApiResponseWrapper<DownloadPersonalDataResponse>> DownloadPersonalData();

    /// <summary>
    /// Deletes personal data for the current user.
    /// </summary>
    /// <param name="request">The <see cref="DeleteAccountCommand"/> containing the user's password.</param>
    /// <returns>A success message.</returns>
    Task<ApiResponseWrapper<string>> DeleteAccount(DeleteAccountCommand request);

    /// <summary>
    /// Determines whether the current user needs to enter their password to perform sensitive actions.
    /// </summary>
    /// <returns>
    /// A <see cref="bool"/> indicating whether the current user needs to enter their password.
    /// </returns>
    Task<ApiResponseWrapper<bool>> RequirePassword();

    #endregion Public Methods
}