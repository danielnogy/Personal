using BinaryPlate.BlazorPlate.Services.Handlers;

namespace BinaryPlate.BlazorPlate.Extensions;

public static class HttpInterceptorExtensions
{
    #region Public Methods

    public static void AddHttpInterceptor(this IServiceCollection services, WebAssemblyHostBuilder builder)
    {
        services.AddScoped<DefaultHttpInterceptor>();
        services.AddHttpClient(NamedHttpClient.DefaultClient, client =>
        {
            client.DefaultRequestVersion = HttpVersion.Version30;
            client.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrLower;
            client.BaseAddress = new Uri(builder.Configuration.GetSection("BaseApiUrl").Value ?? throw new InvalidOperationException("Invalid BaseApiUrl."));
        }).AddHttpMessageHandler<DefaultHttpInterceptor>();
    }

    public static void AddAuthHttpInterceptor(this IServiceCollection services, WebAssemblyHostBuilder builder)
    {
        services.AddScoped<OAuthHttpInterceptor>();
        services.AddHttpClient(NamedHttpClient.OAuthClient, client =>
        {
            client.DefaultRequestVersion = HttpVersion.Version30;
            client.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrLower;
            client.BaseAddress = new Uri(builder.Configuration.GetSection("BaseApiUrl").Value ?? throw new InvalidOperationException("Invalid BaseApiUrl."));
        }).AddHttpMessageHandler<OAuthHttpInterceptor>();
    }
    public static void AddTwoFactorAuthHttpInterceptor(this IServiceCollection services, WebAssemblyHostBuilder builder)
    {
        services.AddScoped<TwoFactorAuthHttpInterceptor>();
        services.AddHttpClient(NamedHttpClient.TwoFactorAuthClient, client =>
        {
            client.DefaultRequestVersion = HttpVersion.Version30;
            client.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrLower;
            client.BaseAddress = new Uri(builder.Configuration.GetSection("BaseApiUrl").Value ?? throw new InvalidOperationException("Invalid BaseApiUrl."));
        }).AddHttpMessageHandler<TwoFactorAuthHttpInterceptor>();
    }
    public static void AddCustomHeadersHttpInterceptor(this IServiceCollection services, WebAssemblyHostBuilder builder)
    {
        services.AddScoped<CustomHeadersHttpInterceptor>();
        services.AddHttpClient(NamedHttpClient.CustomHeadersHttpClient, client =>
        {
            client.DefaultRequestVersion = HttpVersion.Version30;
            client.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrLower;
            client.BaseAddress = new Uri(builder.Configuration.GetSection("BaseApiUrl").Value ?? throw new InvalidOperationException("Invalid BaseApiUrl."));
        }).AddHttpMessageHandler<CustomHeadersHttpInterceptor>();
    }

    #endregion Public Methods
}