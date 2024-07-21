namespace BinaryPlate.Infrastructure.Middleware.MultiTenancy;
public class MultiTenancyMiddleware(RequestDelegate next, TenantInterceptorFactory tenantInterceptorFactory)
{
    #region Private Fields

    private readonly RequestDelegate _next = next ?? throw new ArgumentNullException(nameof(next));

    #endregion Private Fields

    #region Public Methods

    public async Task InvokeAsync(HttpContext httpContext, IApplicationDbContext dbContext, ITenantResolver tenantResolver)
    {
        // Retrieves the appropriate tenant interceptor based on the current tenant mode.
        var tenantInterceptor = tenantInterceptorFactory.CreateInstance(tenantResolver.TenantMode);

        // Invokes the handling logic of the selected tenant interceptor, passing the HTTP context,
        // application database context, and tenant resolver for managing tenant-related information.
        tenantInterceptor.Handle(httpContext, dbContext, tenantResolver);

        // Invokes the next request delegate in the middleware pipeline and awaits the result.
        await _next(httpContext);

    }

    #endregion Public Methods
}