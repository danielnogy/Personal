using BinaryPlate.Application.Common.Constants;

namespace BinaryPlate.WebAPI.Services.HubServices;

public class SignalRContextInfoProvider(IApplicationDbContext dbContext, ITenantResolver tenantResolver) : ISignalRContextInfoProvider
{
    #region Public Methods

    public string GetHostName(HubCallerContext hubCallerContext)
    {
        // Check if the given HubCallerContext is null, throw an ArgumentNullException if it is.
        ThrowExceptionIfNull(hubCallerContext);

        // Get the HttpContext from the given HubCallerContext and retrieve the request object from it.
        var httpContext = hubCallerContext.GetHttpContext()?.Request;

        // Return a formatted string with the Scheme and Host of the request object.
        return $"{httpContext?.Scheme}://{httpContext?.Host}";
    }

    public string GetUserNameIdentifier(HubCallerContext hubCallerContext)
    {
        // Check if the given HubCallerContext is null, throw an ArgumentNullException if it is.
        ThrowExceptionIfNull(hubCallerContext);

        // Return the value of the first claim whose type matches ClaimTypes.NameIdentifier.
        return hubCallerContext.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
    }

    public string GetUserName(HubCallerContext hubCallerContext)
    {
        // Check if the given HubCallerContext is null, throw an ArgumentNullException if it is.
        ThrowExceptionIfNull(hubCallerContext);

        // Check if the user is authenticated, throw an Exception if they are not.
        if (!hubCallerContext.User.IsAuthenticated())
            throw new Exception(Resource.You_are_not_authorized);

        // Split the username by the "@" symbol and return the first part.
        return hubCallerContext.User?.Identity?.Name?.Split("@")[0];
    }

    public string GetUserLanguage(HubCallerContext hubCallerContext)
    {
        // Check if the given HubCallerContext is null, throw an ArgumentNullException if it is.
        ThrowExceptionIfNull(hubCallerContext);

        HttpContext request = hubCallerContext.GetHttpContext();
        if (request != null && request.Request.Query.TryGetValue("Accept-Language", out var language))
            // Return the first language found in the "Accept-Language" header
            return language.FirstOrDefault();

        // Return null or a default value if the language is not found
        return null;
    }

    public TextDirection GetUserLanguageDirection(HubCallerContext hubCallerContext)
    {
        // Check if the given HubCallerContext is null, throw an ArgumentNullException if it is.
        ThrowExceptionIfNull(hubCallerContext);

        HttpContext request = hubCallerContext.GetHttpContext();
        if (request != null && request.Request.Query.TryGetValue("Accept-Language", out var language) && IsRtlLanguage(language.FirstOrDefault()))
            return TextDirection.Rtl;

        // Default to Left-to-Right for other languages
        return TextDirection.Ltr;
    }

    public (Guid? TenantId, string TenantName) GetTenantInfo(HubCallerContext hubCallerContext)
    {
        // Ensure the SignalR connection context is not null.
        ThrowExceptionIfNull(hubCallerContext);

        // Extract tenant name from the SignalR connection's query parameters.
        var tenantName = hubCallerContext.GetHttpContext()?.Request.Query["BP-Tenant"].ToString();

        // If the tenant name is null or empty, return null values.
        if (string.IsNullOrEmpty(tenantName))
            return (null, null);

        // Retrieve tenant information from the database based on the tenant name.
        var tenant = dbContext.Tenants.FirstOrDefault(t => t.Name == tenantName);

        // Create a tuple containing the tenant ID and tenant name.
        var tenantInfo = (tenant?.Id, tenantName);

        // Return the tenant information.
        return tenantInfo;
    }

    public ITenantResolver SetTenantIdViaTenantResolver(HubCallerContext hubCallerContext)
    {
        // Check if the given HubCallerContext is null, throw an ArgumentNullException if it is.
        ThrowExceptionIfNull(hubCallerContext);

        // Get the tenant information from the SignalR connection context using the GetTenantInfo method.
        var tenantInfo = GetTenantInfo(hubCallerContext);

        // Set the TenantId and TenantName using the TenantResolver.
        tenantResolver.SetTenantInfo(tenantInfo.TenantId, tenantInfo.TenantName);

        // Return the updated tenant resolver.
        return tenantResolver;
    }

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Determines if a language is right-to-left (RTL) based on its code.
    /// </summary>
    /// <param name="language">The language code to check.</param>
    /// <returns>True if the language is right-to-left (RTL), otherwise false.</returns>
    private static bool IsRtlLanguage(string language)
    {
        // Check if the language code starts with any RTL language code.
        return Language.RtlLanguageCodes.Any(language.StartsWith);
    }

    /// <summary>
    /// Throws an ArgumentNullException if the provided HubCallerContext is null.
    /// </summary>
    /// <param name="hubCallerContext">The HubCallerContext to check for null.</param>
    private void ThrowExceptionIfNull(HubCallerContext hubCallerContext)
    {
        // Throw an ArgumentNullException if the given HubCallerContext is null.
        if (hubCallerContext is null)
            throw new ArgumentNullException(nameof(hubCallerContext));
    }

    #endregion Private Methods
}