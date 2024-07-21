namespace BinaryPlate.Infrastructure.Extensions;

public static class MultiTenancyMiddlewareExtension
{
    #region Public Methods

    /// <summary>
    /// Adds multi-tenancy capabilities to the request pipeline.
    /// </summary>
    /// <param name="builder">The application builder.</param>
    /// <returns>The application builder.</returns>
    public static IApplicationBuilder UseMultiTenancy(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<MultiTenancyMiddleware>();
    }

    #endregion Public Methods
}