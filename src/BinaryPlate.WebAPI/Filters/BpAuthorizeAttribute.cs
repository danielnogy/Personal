namespace BinaryPlate.WebAPI.Filters;

/// <summary>
/// Custom authorization attribute used to check if the current user has permission to access a resource.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class BpAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    #region Public Properties

    public bool TwoFactorAuthRequired { get; set; }

    #endregion Public Properties

    #region Public Methods

    /// <summary>
    /// Checks if the current user has permission to access a resource.
    /// </summary>
    /// <param name="context">The context for the authorization filter.</param>
    public virtual void OnAuthorization(AuthorizationFilterContext context)
    {
        // Check if two-factor authentication is required
        if (TwoFactorAuthRequired)
        {
            // If two-factor authentication is required and the user is not authenticated with two-factor,
            // throw an exception to redirect the user to the two-factor authentication page
            if (IsUserTwoFactorEnabled(context.HttpContext.User) && !IsUserTwoFactorAuthenticated(context.HttpContext))
                throw new TwoFactorAuthRedirectionException();

            // If two-factor authentication is required and the user does not have it enabled,
            // throw an exception indicating that two-factor authentication is required
            //2FA bypass
            if (!IsUserTwoFactorEnabled(context.HttpContext.User) && 1==2)
                throw new TwoFactorAuthRequiredException();
        }

        // Define the httpContextAccessor.
        var httpContextAccessor = context.HttpContext.RequestServices.GetRequiredService<IHttpContextAccessor>();

        //var dbContext = context.HttpContext.RequestServices.GetRequiredService<IApplicationDbContext>();

        // Get the route data from the context.
        var routeData = (httpContextAccessor.HttpContext ?? throw new InvalidOperationException()).GetRouteData();

        //var areaName = routeData?.Values["area"]?.ToString();

        // Check if the user is a super admin.
        var isSuperAdmin = context.HttpContext.User.HasClaim(c => c is { Type: "IsSuperAdmin", Value: "true" });

        if (isSuperAdmin)
            return;

        // Get the controller and action names from the route data.
        var controllerName = routeData.Values["controller"]?.ToString();
        var actionName = routeData.Values["action"]?.ToString();

        // Combine the controller and action names to create the permission name.
        var permission = $"{controllerName}.{actionName}";

        // TODO: Uncomment this section if you want to check whether the anonymous user is allowed to access the resource or not.
        //var allowAnonymous = !await dbContext.ApplicationPermissions.AnyAsync(p => p.Name == permission);
        //if (allowAnonymous)
        //    return;

        // Check if the user is authenticated.
        if (context.HttpContext.User.Identity is not { IsAuthenticated: true })
            throw new UnauthorizedAccessException(string.Format(Resource.You_are_not_authorized, context.HttpContext.Request.Path));

        // Create a claim for the permission.
        var claim = new Claim("permissions", permission);

        // Check if the user has the required permission.
        if (!context.HttpContext.User.HasClaim(c => c.Type == claim.Type && c.Value == claim.Value))
            throw new ForbiddenAccessException(string.Format(Resource.You_are_forbidden, context.HttpContext.Request.Path));
    }

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Checks if the user has two-factor authentication enabled based on claims.
    /// </summary>
    /// <param name="user">The ClaimsPrincipal representing the user.</param>
    /// <returns>True if the user has two-factor authentication enabled; otherwise, false.</returns>
    private static bool IsUserTwoFactorEnabled(ClaimsPrincipal user)
    {
        // Retrieve the value of the "TwoFactorEnabled" claim from user claims
        var twoFactorEnabled = user.Claims.FirstOrDefault(c => c.Type == "TwoFactorEnabled")?.Value;

        // Return true if the user has two-factor authentication enabled, otherwise return false
        return twoFactorEnabled == "true";
    }

    /// <summary>
    /// Checks if the user has been authenticated with two-factor authentication for the current request.
    /// </summary>
    /// <param name="httpContext">The HttpContext representing the current HTTP request.</param>
    /// <returns>True if the user has been authenticated with two-factor authentication; otherwise, false.</returns>
    private static bool IsUserTwoFactorAuthenticated(HttpContext httpContext)
    {
        // Get the controller and action from the current route values.
        var controller = httpContext.GetRouteValue("controller");

        // Construct the requested URI based on the controller.
        var requestedUri = $"/api/{controller}";

        // Retrieve the value of the "TwoFactorAuthenticatedUri" header from the request.
        var twoFactorAuthenticatedUri = httpContext.Request.Headers["TwoFactorAuthenticatedUri"];

        // Compare the requested URI with the two-factor authenticated URI.
        var twoFactorAuthenticated = string.Equals(twoFactorAuthenticatedUri, requestedUri, StringComparison.OrdinalIgnoreCase);

        // Return the result indicating whether the user has been authenticated with two-factor authentication for the current request.
        return twoFactorAuthenticated;
    }

    #endregion Private Methods
}