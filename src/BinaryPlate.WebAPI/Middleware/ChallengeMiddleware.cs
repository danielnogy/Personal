namespace BinaryPlate.WebAPI.Middleware;

public class ChallengeMiddleware(RequestDelegate requestDelegate)
{
    #region Private Fields

    private readonly RequestDelegate _request = requestDelegate ?? throw new ArgumentNullException(nameof(requestDelegate));

    #endregion Private Fields

    #region Public Methods

    public async Task InvokeAsync(HttpContext context, ITenantResolver tenantResolver)
    {
        // Throws an ArgumentNullException if the context is null.
        if (context == null)
            throw new ArgumentNullException(nameof(context));

        // Enforces tenant access control.
        EnforceTenantAccessControl(context, tenantResolver);

        // Invokes the next request delegate in the pipeline and awaits the result.
        await _request(context);

        // Checks the HTTP response status code.
        switch (context.Response.StatusCode)
        {
            // If the status code is 401 (Unauthorized), throws an UnauthorizedAccessException with a message indicating the user is not authorized.
            case 401:
                throw new UnauthorizedAccessException(string.Format(Resource.You_are_not_authorized, context.Request.GetDisplayUrl().Split('?')[0]));

            // If the status code is 403 (Forbidden), throws a ForbiddenAccessException with a message indicating the user is forbidden.
            case 403:
                throw new ForbiddenAccessException(string.Format(Resource.You_are_forbidden, context.Request.PathBase));
        }
    }

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Enforces tenant access control based on the provided HTTP context and tenant resolver.
    /// </summary>
    /// <param name="httpContext">The current HTTP context.</param>
    /// <param name="tenantResolver">The tenant resolver.</param>
    private static void EnforceTenantAccessControl(HttpContext httpContext, ITenantResolver tenantResolver)
    {
        // Check if the application is not in MultiTenant mode, or it's not a tenant-specific request, or it's a tenant creation host request,
        // skip further processing.
        if (tenantResolver.TenantMode != TenantMode.MultiTenant || !tenantResolver.IsTenantRequest || tenantResolver.IsTenantCreationHostRequest)
            return;

        // Check if the user is authenticated and if the request is not a WebSocket request,
        // skip further processing.
        if (!httpContext.IsUserAuthenticated() || httpContext.IsWebSocketRequest())
            return;

        // Retrieve the tenant name from the request header.
        var tenantNameHeader = httpContext.GetTenantName();

        // Retrieve the tenant name from the user claims.
        var userTenantNameClaim = httpContext.GetTenantNameFromUserClaims();

        // Compare the tenant names from the request header and user claims.
        if (tenantNameHeader != userTenantNameClaim)
            // If the tenant names do not match, throw an UnauthorizedAccessException.
            throw new UnauthorizedAccessException(Resource.You_are_not_authorized_to_access_resources_related_to_other_tenants);
    }

    #endregion Private Methods
}