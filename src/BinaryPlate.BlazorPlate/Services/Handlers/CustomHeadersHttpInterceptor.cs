namespace BinaryPlate.BlazorPlate.Services.Handlers;

public class CustomHeadersHttpInterceptor : DelegatingHandler
{
    #region Private Fields

    private readonly NavigationManager _navigationManager;
    private readonly ReturnUrlProvider _returnUrlProvider;
    private readonly RefreshTokenService _refreshTokenService;
    private readonly AccessTokenProvider _accessTokenProvider;
    private readonly AppStateManager _appStateManager;

    private CancellationTokenSource _tokenSource;

    #endregion Private Fields

    #region Public Constructors

    public CustomHeadersHttpInterceptor(NavigationManager navigationManager,
                                        ReturnUrlProvider returnUrlProvider,
                                        RefreshTokenService refreshTokenService,
                                        AccessTokenProvider accessTokenProvider,
                                        AppStateManager appStateManager)
    {
        _navigationManager = navigationManager;
        _returnUrlProvider = returnUrlProvider;
        _refreshTokenService = refreshTokenService;
        _accessTokenProvider = accessTokenProvider;
        _appStateManager = appStateManager;

        _tokenSource = new CancellationTokenSource();
        _appStateManager.TokenSourceChanged += OnAppStateManagerOnTokenSourceChanged;
    }

    #endregion Public Constructors

    #region Protected Methods

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        _appStateManager.OverlayVisible = true;

        await AddRequiredHeaders(request);
        AddCustomHeaders(request);
        var response = await base.SendAsync(request, _tokenSource.Token);

        if (response.StatusCode is HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden)
        {
            var url = _navigationManager.Uri;

            if (!(url.Contains("401") || url.Contains("404")))
            {
                await _returnUrlProvider.SetReturnUrl(url);
                _navigationManager.NavigateTo($"/pages/error/401/true");
            }
        }

        _appStateManager.OverlayVisible = false;

        return response;
    }

    #endregion Protected Methods

    #region Private Methods

    private void OnAppStateManagerOnTokenSourceChanged(object obj, EventArgs args)
    {
        _tokenSource.Cancel();
        _tokenSource = new CancellationTokenSource();
    }

    private async Task AddRequiredHeaders(HttpRequestMessage request)
    {
        var accessToken = await _accessTokenProvider.TryGetAccessToken();

        request.Headers.Authorization = new AuthenticationHeaderValue("bearer", accessToken);

        if (!string.IsNullOrWhiteSpace(accessToken))
        {
            var absPath = request.RequestUri?.AbsolutePath;
            if (absPath != null && !absPath.Contains("/api/account/"))
            {
                accessToken = await _refreshTokenService.TryRefreshToken();
                if (!string.IsNullOrEmpty(accessToken))
                    request.Headers.Authorization = new AuthenticationHeaderValue("bearer", accessToken);
            }
        }

        request.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue(CultureInfo.CurrentCulture.ToString()));
    }

    private void AddCustomHeaders(HttpRequestMessage request)
    {
        var httpCustomHeader = _appStateManager.GetHttpCustomHeader();

        if (httpCustomHeader is { Key: not null })
            request.Headers.Add(httpCustomHeader.Key, httpCustomHeader.Value);

        _appStateManager.ClearCustomHeader();
    }

    #endregion Private Methods
}