namespace BinaryPlate.Application.Features.Account.Commands.AuthenticateExternalLogin;

public class AuthenticateExternalLoginCommand : IRequest<Envelope<AuthenticateExternalLoginResponse>>
{
    #region Public Properties

    public string AuthenticationScheme { get; set; }

    #endregion Public Properties

    #region Public Classes

    public class ExternalLoginCallBackCommandCommandHandler(IHttpContextAccessor httpContextAccessor) : IRequestHandler<AuthenticateExternalLoginCommand, Envelope<AuthenticateExternalLoginResponse>>
    {
        #region Public Methods

        public async Task<Envelope<AuthenticateExternalLoginResponse>> Handle(AuthenticateExternalLoginCommand request, CancellationToken cancellationToken)
        {
            // Check if the HttpContextAccessor is null.
            if (httpContextAccessor.HttpContext is null)
                throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));

            // Authenticate the request using the specified authentication scheme.
            var authenticationResult = await httpContextAccessor.HttpContext.AuthenticateAsync(request.AuthenticationScheme);

            // Check if authentication succeeded.
            if (authenticationResult.Succeeded)
            {
                // Extract the claims from the authenticated principal.
                var claims = authenticationResult.Principal.Claims.ToList();

                // Extract the login information.
                var loginInfo = ExtractLoginInfo(authenticationResult.Principal, request.AuthenticationScheme);

                // Add the authentication method claim with the login provider.
                claims.AddRange(new List<Claim>
                                {
                                    new (ClaimTypes.AuthenticationMethod, loginInfo.LoginProvider)
                                });

                // Check if the user is a Twitter user and extract additional information.
                var isTwitterUser = CheckIfTwitterUserAndExtractInfo(request, claims, loginInfo);

                if (isTwitterUser)
                    // Add the additional claims for Twitter users.
                    claims.AddRange(new List<Claim>
                                    {
                                        new (ClaimTypes.Email, loginInfo.Email),
                                        new (ClaimTypes.GivenName, loginInfo.FirstName),
                                        new (ClaimTypes.Surname, loginInfo.LastName)
                                    });

                // Return the response with the claims.
                return Envelope<AuthenticateExternalLoginResponse>.Result.Ok(new AuthenticateExternalLoginResponse
                {
                    Claims = claims
                });
            }

            // Handle different authentication failure scenarios.
            if (authenticationResult.Failure != null)
                switch (authenticationResult.Failure)
                {
                    // Authentication failed due to token validation exception.
                    case SecurityTokenValidationException:
                        return Envelope<AuthenticateExternalLoginResponse>.Result.Unauthorized("Authentication failed. Please check your credentials.");

                    // Authentication failed due to general authentication exception.
                    case AuthenticationException:
                        return Envelope<AuthenticateExternalLoginResponse>.Result.Forbidden("Authentication failed. Please check your credentials.");
                }

            // Return a general error response if authentication failed.
            return Envelope<AuthenticateExternalLoginResponse>.Result.ServerError("Authentication failed. Please try again later.");
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Extracts login information from the provided principal.
        /// </summary>
        /// <param name="principal">The ClaimsPrincipal object.</param>
        /// <param name="authenticationScheme">The Authentication Scheme type.</param>
        /// <returns>The UserInfo object with extracted login information.</returns>
        private static UserInfo ExtractLoginInfo(ClaimsPrincipal principal, string authenticationScheme)
        {
            // Extract email, first name, last name, provider key, and display name from claims.
            return new UserInfo
            {
                Email = principal.Claims.Where(c => c.Type == ClaimTypes.Email)
                                        .Select(c => c.Value)
                                        .FirstOrDefault(),

                FirstName = principal.Claims.Where(c => c.Type == ClaimTypes.GivenName)
                                            .Select(c => c.Value)
                                            .FirstOrDefault(),

                LastName = principal.Claims.Where(c => c.Type == ClaimTypes.Surname)
                                           .Select(c => c.Value)
                                           .FirstOrDefault(),

                DisplayName = principal.Claims.Where(c => c.Type == ClaimTypes.Name)
                                              .Select(c => c.Value)
                                              .FirstOrDefault(),

                ProviderKey = principal.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                                              .Select(c => c.Value)
                                              .FirstOrDefault(),

                LoginProvider = authenticationScheme
            };
        }

        /// <summary>
        /// Checks if the authentication scheme is Twitter and extracts additional information if needed.
        /// </summary>
        /// <param name="request">The AuthenticateExternalLoginCommand object.</param>
        /// <param name="claims">The collection of claims.</param>
        /// <param name="loginInfo">The UserInfo object containing login information.</param>
        private bool CheckIfTwitterUserAndExtractInfo(AuthenticateExternalLoginCommand request, IReadOnlyCollection<Claim> claims, UserInfo loginInfo)
        {
            if (request.AuthenticationScheme == TwitterDefaults.AuthenticationScheme)
            {
                // Extract missing information for Twitter login.
                var (nameIdentifier, name) = ExtractMissingInfo(claims);
                loginInfo.FirstName ??= name;
                loginInfo.LastName ??= string.Empty;
                loginInfo.Email ??= $"{nameIdentifier}@{name}";

                return true;
            }

            return false;
        }

        /// <summary>
        /// Extracts missing information from the collection of claims.
        /// </summary>
        /// <param name="claims">The collection of claims.</param>
        /// <returns>A tuple containing the name identifier and name.</returns>
        private (string, string) ExtractMissingInfo(IReadOnlyCollection<Claim> claims)
        {
            // Extract name and name identifier from claims.
            var name = claims.Where(c => c.Type == ClaimTypes.Name)
                             .Select(c => c.Value)
                             .FirstOrDefault();

            var nameIdentifier = claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                                       .Select(c => c.Value)
                                       .FirstOrDefault();

            return (nameIdentifier, name);
        }

        #endregion Private Methods
    }

    #endregion Public Classes
}