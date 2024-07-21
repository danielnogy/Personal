﻿namespace BinaryPlate.Application.Common.Contracts.Application;

/// <summary>
/// This interface represents the contract for authentication-related operations.
/// </summary>
public interface IAuthService
{
    #region Public Methods

    /// <summary>
    /// Performs a login operation based on the specified login command.
    /// </summary>
    /// <param name="request">The login command containing user credentials.</param>
    /// <returns>An envelope containing the login response.</returns>
    Task<Envelope<LoginResponse>> Login(LoginCommand request);

    /// <summary>
    /// Generates access and refresh tokens for the specified user.
    /// </summary>
    /// <param name="user">The user for whom to generate tokens.</param>
    /// <param name="hasPassword"></param>
    /// <returns>A tuple containing the access token and refresh token.</returns>
    Task<(string accessToken, string refreshToken)> GenerateAccessAndRefreshTokens(ApplicationUser user, bool hasPassword = true);

    #endregion Public Methods
}