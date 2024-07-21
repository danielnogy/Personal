namespace BinaryPlate.BlazorPlate.Services.Handlers;

public class TwoFactorAuthHttpInterceptor : DelegatingHandler
{
    #region Private Fields

    private readonly AppStateManager _appStateManager;
    private readonly NavigationManager _navigationManager;
    private readonly ReturnUrlProvider _returnUrlProvider;
    private readonly AccessTokenProvider _accessTokenProvider;
    private readonly RefreshTokenService _refreshTokenService;
    private readonly ILocalStorageService _localStorageService;
    private readonly AuthenticationStateProvider _authenticationStateProvider;

    private CancellationTokenSource _tokenSource;
    private string _local2FaUri;

    #endregion Private Fields

    #region Public Constructors

    public TwoFactorAuthHttpInterceptor(AppStateManager appStateManager,
                                        NavigationManager navigationManager,
                                        ReturnUrlProvider returnUrlProvider,
                                        AccessTokenProvider accessTokenProvider,
                                        RefreshTokenService refreshTokenService,
                                        ILocalStorageService localStorageService,
                                        AuthenticationStateProvider authenticationStateProvider)
    {
        _appStateManager = appStateManager;
        _navigationManager = navigationManager;
        _returnUrlProvider = returnUrlProvider;
        _accessTokenProvider = accessTokenProvider;
        _refreshTokenService = refreshTokenService;
        _localStorageService = localStorageService;
        _authenticationStateProvider = authenticationStateProvider;

        _tokenSource = new CancellationTokenSource();
        _appStateManager.TokenSourceChanged += OnAppStateManagerOnTokenSourceChanged;
    }

    #endregion Public Constructors

    #region Protected Methods

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        _appStateManager.OverlayVisible = true;

        _local2FaUri = GetBasePath(new Uri(request.RequestUri?.AbsoluteUri!).LocalPath);
        await AddHeadersToRequest(request);
        var response = await base.SendAsync(request, _tokenSource.Token);
        await ProcessResponse(cancellationToken, response);

        _appStateManager.OverlayVisible = false;

        return response;
    }

    private async Task ProcessResponse(CancellationToken cancellationToken, HttpResponseMessage response)
    {
        var uri = _navigationManager.Uri;

        switch (response.StatusCode)
        {
            case HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden:
                {
                    if (!(uri.Contains("401") || uri.Contains("404")))
                    {
                        await _returnUrlProvider.SetReturnUrl(uri);
                        _navigationManager.NavigateTo("/pages/error/401/true");
                    }

                    break;
                }
            case HttpStatusCode.Locked:
                {
                    if (!(uri.Contains("401") || uri.Contains("404")))
                    {
                        await _returnUrlProvider.SetReturnUrl(uri);
                        _navigationManager.NavigateTo("/pages/error/423");
                    }

                    break;
                }
            case HttpStatusCode.Redirect:
                {
                    await _localStorageService.SetItemAsync("Local2FaUri", _local2FaUri, cancellationToken);
                    if (!(uri.Contains("401") || uri.Contains("404")))
                    {
                        await _returnUrlProvider.SetReturnUrl(uri);
                        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
                        var user = authState.User;
                        _navigationManager.NavigateTo(user.Identity is { IsAuthenticated: true }
                                                          ? $"/account/loginWith2Fa/{user.Identity.Name}"
                                                          : "/pages/error/401/true");
                    }

                    break;
                }
        }
    }

    #endregion Protected Methods

    #region Private Methods

    private void OnAppStateManagerOnTokenSourceChanged(object obj, EventArgs args)
    {
        _tokenSource.Cancel();
        _tokenSource = new CancellationTokenSource();
    }

    private async Task AddHeadersToRequest(HttpRequestMessage request)
    {
        AddTenantHeader(request);
        await AddAuthorizationHeader(request);
        await AddTwoFactorAuthHeader(request);
        AddLanguageHeader(request);
    }

    private void AddTenantHeader(HttpRequestMessage request)
    {
        var subDomain = _navigationManager.GetSubDomain();
        if (subDomain != null)
            request.Headers.Add("BP-Tenant", subDomain);
    }

    private async Task AddAuthorizationHeader(HttpRequestMessage request)
    {
        var accessToken = await _accessTokenProvider.TryGetAccessToken();

        request.Headers.Authorization = new AuthenticationHeaderValue("bearer", accessToken);

        if (!string.IsNullOrWhiteSpace(accessToken))
        {
            var absolutePath = request.RequestUri?.AbsolutePath;
            if (absolutePath != null && !absolutePath.Contains("/api/account/"))
            {
                accessToken = await _refreshTokenService.TryRefreshToken();
                if (!string.IsNullOrEmpty(accessToken))
                    request.Headers.Authorization = new AuthenticationHeaderValue("bearer", accessToken);
            }
        }
    }

    private async Task AddTwoFactorAuthHeader(HttpRequestMessage request)
    {
        var requestedCachedUri = await _localStorageService.GetItemAsync<string>(_local2FaUri);
        if (requestedCachedUri != null)
            request.Headers.Add("TwoFactorAuthenticatedUri", requestedCachedUri);
    }

    private static void AddLanguageHeader(HttpRequestMessage request)
    {
        request.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue(CultureInfo.CurrentCulture.ToString()));
    }

    private string GetBasePath(string path)
    {
        var segments = path.Split('/');
        if (segments.Length <= 1)
            return path;
        var basePathSegments = segments.Take(segments.Length - 1);
        return string.Join('/', basePathSegments);
    }

    #endregion Private Methods
}