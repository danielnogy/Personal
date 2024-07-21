namespace BinaryPlate.BlazorPlate.Consumers.SignalRClients;

public class DashboardHubClient(AuthenticationService authenticationService,
                                 AccessTokenProvider accessTokenProvider,
                                 NavigationManager navigationManager,
                                 UrlProvider urlProvider,
                                 ISnackbar snackbar)
{
    #region Public Events

    public event Action<GetHeadlinesResponse> OnHeadlinesDataReceived;

    #endregion Public Events

    #region Public Properties

    public HubConnection HubConnection { get; set; }

    #endregion Public Properties

    #region Public Methods

    public async Task<bool> StartHubConnection()
    {
        if (!await authenticationService.IsUserAuthenticated())
            return false;

        if (ShouldInitializeHubConnection())
        {
            //snackbar.Add(Resource.Dashboard_Hub_is_being_initialized, Severity.Info);

            var subDomain = navigationManager.GetSubDomain();

            HubConnection = BuildHubConnection(subDomain);

            try
            {
                await HubConnection.StartAsync();

                if (HubConnection.State == HubConnectionState.Connected)
                {
                    snackbar.Add(Resource.Dashboard_Hub_is_now_connected, Severity.Success);
                    return true;
                }
            }
            catch (Exception ex)
            {
                HandleHubConnectionError(ex);
            }

            HubConnection.Closed += OnHubConnectionClosed;
        }

        return false;
    }

    public Task GetHeadlinesData(GetHeadlinesResponse response)
    {
        if (response == null)
            throw new ArgumentNullException(nameof(response));

        HubConnection.On<GetHeadlinesResponse>("SendHeadlinesData", (data) =>
                                                                    {
                                                                        response = data;
                                                                        // Emit the event to notify listeners
                                                                        OnHeadlinesDataReceived?.Invoke(data);
                                                                    });
        return Task.CompletedTask;
    }

    public async Task<bool> CloseHubConnection()
    {
        if (HubConnection is null)
            return true;

        switch (HubConnection.State)
        {
            case HubConnectionState.Connected:
                try
                {
                    await HubConnection.StopAsync();
                }
                finally
                {
                    await HubConnection.DisposeAsync();
                    //snackbar.Add(Resource.Dashboard_Hub_is_closed, Severity.Warning);
                }
                return true;

            case HubConnectionState.Disconnected:
                snackbar.Add(Resource.Dashboard_Hub_is_already_closed, Severity.Success);
                return true;
        }
        return false;
    }

    #endregion Public Methods

    #region Private Methods

    private bool ShouldInitializeHubConnection()
    {
        return HubConnection is null || HubConnection.State == HubConnectionState.Disconnected;
    }

    private HubConnection BuildHubConnection(string subDomain)
    {
        var hubConnectionBuilder = new HubConnectionBuilder();

        hubConnectionBuilder.WithUrl($"{urlProvider.BaseHubApiUrl}/Hubs/DashboardHub?Bp-Tenant={subDomain}&Accept-Language={CultureInfo.CurrentCulture}",
                                     options =>
                                     {
                                         options.AccessTokenProvider = async () => await accessTokenProvider.TryGetAccessToken();
                                     });

        return hubConnectionBuilder.Build();
    }

    private void HandleHubConnectionError(Exception ex)
    {
        snackbar.Add(string.Format(Resource.Unable_to_connect_to_the_dashboard_hub_due_to_an_error, ex.Message), Severity.Error);
    }

    private Task OnHubConnectionClosed(Exception exception)
    {
        snackbar.Add(Resource.Reporting_Hub_is_closed, Severity.Warning);

        return Task.CompletedTask;
    }

    #endregion Private Methods
}