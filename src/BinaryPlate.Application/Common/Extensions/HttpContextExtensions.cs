using BinaryPlate.Application.Common.Constants;

namespace BinaryPlate.Application.Common.Extensions;

/// <summary>
/// Provides extension methods for accessing properties of the <see cref="HttpContext"/> class.
/// </summary>
///
public static class HttpContextExtensions
{
    #region Public Methods

    /// <summary>
    /// Gets the user ID from the <see cref="HttpContext"/>.
    /// </summary>
    /// <param name="httpContextAccessor">The <see cref="IHttpContextAccessor"/> instance.</param>
    /// <returns>The user ID as a string, or an empty string if it is not found.</returns>
    public static string GetUserId(this IHttpContextAccessor httpContextAccessor)
    {
        var userId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        return userId ?? string.Empty;
    }

    /// <summary>
    /// Determines if the user is authenticated in the <see cref="HttpContext"/>.
    /// </summary>
    /// <param name="httpContextAccessor">The <see cref="IHttpContextAccessor"/> instance.</param>
    /// <returns>True if the user is authenticated, otherwise false.</returns>
    public static bool IsUserAuthenticated(this IHttpContextAccessor httpContextAccessor)
    {
        return httpContextAccessor.HttpContext is { User.Identity.IsAuthenticated: true };
    }

    /// <summary>
    /// Determines if the request is a WebSocket request in the <see cref="HttpContext"/>.
    /// </summary>
    /// <param name="httpContextAccessor">The <see cref="IHttpContextAccessor"/> instance.</param>
    /// <returns>True if it is a WebSocket request, otherwise false.</returns>
    public static bool IsWebSocketRequest(this IHttpContextAccessor httpContextAccessor)
    {
        return httpContextAccessor.HttpContext != null && httpContextAccessor.HttpContext.Request.Headers.ContainsKey("Sec-WebSocket-Extensions");
    }

    /// <summary>
    /// Determines if the user is authenticated in the <see cref="HttpContext"/>.
    /// </summary>
    /// <param name="httpContext">The <see cref="HttpContext"/> instance.</param>
    /// <returns>True if the user is authenticated, otherwise false.</returns>
    public static bool IsUserAuthenticated(this HttpContext httpContext)
    {
        return httpContext is { User.Identity.IsAuthenticated: true };
    }

    /// <summary>
    /// Determines if the request is a WebSocket request in the <see cref="HttpContext"/>.
    /// </summary>
    /// <param name="httpContext">The <see cref="HttpContext"/> instance.</param>
    /// <returns>True if it is a WebSocket request, otherwise false.</returns>
    public static bool IsWebSocketRequest(this HttpContext httpContext)
    {
        return httpContext.Request.Headers.ContainsKey("Sec-WebSocket-Extensions");
    }

    /// <summary>
    /// Gets the user name from the <see cref="HttpContext"/>.
    /// </summary>
    /// <param name="httpContextAccessor">The <see cref="IHttpContextAccessor"/> instance.</param>
    /// <returns>The user name as a string.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the <see cref="UserManager{TUser}"/> instance is null.
    /// </exception>
    public static string GetUserName(this IHttpContextAccessor httpContextAccessor)
    {
        // Get the UserManager service from the request services
        var userManager = httpContextAccessor.HttpContext?.RequestServices.GetService<UserManager<ApplicationUser>>();

        // Check if the UserManager service is null, throw an exception if so
        if (userManager == null)
            throw new ArgumentException(nameof(userManager));

        // Get the user name from the UserManager based on the HttpContext User
        var userName = userManager.GetUserName(httpContextAccessor.HttpContext.User);

        // Return the obtained user name
        return userName;
    }

    /// <summary>
    /// Gets the selected user language from the <see cref="HttpContext"/>.
    /// </summary>
    /// <param name="httpContextAccessor">The <see cref="IHttpContextAccessor"/> instance.</param>
    /// <returns>The user language as a string.</returns>
    public static string GetUserLanguage(this IHttpContextAccessor httpContextAccessor)
    {
        // Retrieve the value of the "Accept-Language" header from the HttpContext Request
        var language = httpContextAccessor.HttpContext?.Request.Headers["Accept-Language"].ToString();

        // Return the obtained user language
        return language;
    }

    public static TextDirection GetUserLanguageDirection(this IHttpContextAccessor httpContextAccessor)
    {
        // Retrieve the value of the "Accept-Language" header from the HttpContext Request
        var language = httpContextAccessor.HttpContext?.Request.Headers["Accept-Language"].ToString();

        // Check if the language is one of the RTL languages
        if (!string.IsNullOrEmpty(language) && IsRtlLanguage(language))
            return TextDirection.Rtl; // Right-to-Left

        // Default to Left-to-Right for other languages
        return TextDirection.Ltr;
    }

    /// <summary>
    /// Extension method to get the name of the requested controller from the provided
    /// <see cref="IHttpContextAccessor"/> object.
    /// </summary>
    /// <param name="httpContextAccessor">The <see cref="IHttpContextAccessor"/> object.</param>
    /// <returns>The name of the requested controller.</returns>
    public static string GetControllerName(this IHttpContextAccessor httpContextAccessor)
    {
        // Check if the HttpContext is available
        if (httpContextAccessor.HttpContext == null)
            return string.Empty;

        // Retrieve the name of the requested controller from the RouteData
        var controllerName = httpContextAccessor.HttpContext.GetRouteData().Values["controller"]?.ToString();

        // Return the obtained controller name
        return controllerName;
    }

    /// <summary>
    /// Gets the tenant name from the <see cref="HttpContext"/>.
    /// </summary>
    /// <param name="httpContextAccessor">The <see cref="IHttpContextAccessor"/> instance.</param>
    /// <returns>
    /// The tenant name as a string, or an empty string if it is not found. If the
    /// "BP-TenantCreatedByGateway" header is present, returns its value. Otherwise, returns the
    /// value of the "BP-Tenant" header.
    /// </returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static string GetTenantName(this IHttpContextAccessor httpContextAccessor)
    {
        // Check if the HttpContext is available
        if (httpContextAccessor.HttpContext == null)
            return string.Empty;

        // Retrieve the value of the Bp-Tenant header created by the host owner through
        // the Tenant Portal Client App.
        var tenantName = httpContextAccessor.HttpContext.Request.Headers["BP-Tenant"];
        if (tenantName.Count != 0 && !string.IsNullOrWhiteSpace(tenantName))
            return tenantName;

        // Retrieve the value of the Bp-TenantByGatewayClient header created by an external user through the Tenant
        // Gateway Client App.
        var tenantNameCreatedByGateway = httpContextAccessor.HttpContext.Request.Headers["BP-TenantCreatedByGateway"];
        // Check if the Bp-TenantByGatewayClient header has a value and is not empty
        if (tenantNameCreatedByGateway.Count != 0 && !string.IsNullOrWhiteSpace(tenantNameCreatedByGateway))
            return tenantNameCreatedByGateway;

        var tenantNameCreatedByHost = httpContextAccessor.HttpContext.Request.Headers["BP-TenantCreatedByHost"];
        // Check if the Bp-TenantByHostClient header has a value and is not empty
        if (tenantNameCreatedByHost.Count != 0 && !string.IsNullOrWhiteSpace(tenantNameCreatedByHost))
            return tenantNameCreatedByHost;

        return string.Empty;
    }

    /// <summary>
    /// Gets the tenant name from the <see cref="HttpContext"/>.
    /// </summary>
    /// <param name="httpContext">The <see cref="HttpContext"/> instance.</param>
    /// <returns>The tenant name as a string.</returns>
    public static string GetTenantName(this HttpContext httpContext)
    {
        var tenantName = httpContext.Request.Headers["BP-Tenant"];
        return tenantName;
    }

    /// <summary>
    /// Check if the current HTTP request indicates a host-created tenant request.
    /// </summary>
    /// <param name="httpContextAccessor">The IHttpContextAccessor instance.</param>
    /// <returns>True if the request is a host-created tenant request; otherwise, false.</returns>
    public static bool IsTenantCreationHostRequest(this IHttpContextAccessor httpContextAccessor)
    {
        // Check if the "BP-TenantCreatedByHost" header exists in the request headers and return false if the
        // header is not present, or if it has no values.
        return httpContextAccessor.HttpContext != null
               && httpContextAccessor.HttpContext.Request.Headers.TryGetValue("BP-TenantCreatedByHost", out var values)
               && values.Any();
    }

    /// <summary>
    /// Gets the tenant name from user claims in the <see cref="HttpContext"/>.
    /// </summary>
    /// <param name="httpContext">The <see cref="HttpContext"/> instance.</param>
    /// <returns>The tenant name as a string, or an empty string if it is not found.</returns>
    public static string GetTenantNameFromUserClaims(this HttpContext httpContext)
    {
        return httpContext?.User.Claims.FirstOrDefault(c => c.Type == "TenantName")?.Value ?? string.Empty;
    }

    /// <summary>
    /// Gets the client application host name from the <see cref="HttpContext"/>.
    /// </summary>
    /// <param name="httpContextAccessor">The <see cref="IHttpContextAccessor"/> instance.</param>
    /// <returns>The client application host name as a string.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static string GetClientAppHostName(this IHttpContextAccessor httpContextAccessor)
    {
        // Check if the HttpContext is available
        if (httpContextAccessor.HttpContext == null)
            throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));

        // Get the application options service from the request services
        var appOptionsService = httpContextAccessor.HttpContext.RequestServices.GetRequiredService<IAppOptionsService>();

        // Get the tenant resolver service from the request services
        var tenantResolver = httpContextAccessor.HttpContext.RequestServices.GetRequiredService<ITenantResolver>();

        // Retrieve client application options
        var clientAppOptions = appOptionsService.GetAppClientOptions();

        // Determine the client app hostname based on the current tenant mode
        return tenantResolver.TenantMode switch
        {
            TenantMode.MultiTenant => tenantResolver.IsHostRequest ? clientAppOptions.SingleTenantHostName
                // Use multi-tenant hostname format with subdomain if not the host
                : string.Format(clientAppOptions.MultiTenantHostName, appOptionsService.GetSubDomain()),
            // Use single-tenant hostname if not in multi-tenant mode
            _ => clientAppOptions.SingleTenantHostName
        };
    }

    /// <summary>
    /// Gets the base URI from the <see cref="HttpContext"/>.
    /// </summary>
    /// <param name="httpContextAccessor">The <see cref="IHttpContextAccessor"/> instance.</param>
    /// <returns>The base URI as a string.</returns>
    public static string GetBaseUri(this IHttpContextAccessor httpContextAccessor)
    {
        return $"{httpContextAccessor.HttpContext?.Request.Scheme}://{httpContextAccessor.HttpContext?.Request.Host}";
    }

    #endregion Public Methods

    #region Private Methods

    private static bool IsRtlLanguage(string language)
    {
        return Language.RtlLanguageCodes.Any(language.StartsWith);
    }

    #endregion Private Methods
}