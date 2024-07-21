namespace BinaryPlate.BlazorPlate.Services.Handlers;

public class OAuthHttpInterceptor(NavigationManager navigationManager) : DelegatingHandler
{
    #region Protected Methods

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
        await AddRequiredHeaders(request);
        return await base.SendAsync(request, cancellationToken);
    }

    #endregion Protected Methods

    #region Private Methods

    private Task AddRequiredHeaders(HttpRequestMessage request)
    {
        var subDomain = navigationManager.GetSubDomain();

        if (subDomain != null)
            request.Headers.Add("BP-Tenant", subDomain);

        return Task.CompletedTask;
    }

    #endregion Private Methods
}