namespace BinaryPlate.BlazorPlate.Consumers.SignalRClients;

public class ReportingHubClient(AuthenticationService authenticationService,
                                 AccessTokenProvider accessTokenProvider,
                                 NavigationManager navigationManager,
                                 UrlProvider urlProvider,
                                 ISnackbar snackbar,
                                 IJSRuntime jsRuntime,
                                 IDialogService dialogService)
{
    #region Public Events

    public event Action ReportStatusReceived;

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
            //snackbar.Add(Resource.Reporting_Hub_is_being_initialized, Severity.Info);

            var subDomain = navigationManager.GetSubDomain();

            HubConnection = BuildHubConnection(subDomain);

            try
            {
                await HubConnection.StartAsync();

                if (HubConnection.State == HubConnectionState.Connected)
                {
                    snackbar.Add(Resource.Reporting_Hub_is_now_connected, Severity.Success);
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

                    snackbar.Add(Resource.Reporting_Hub_is_closed, Severity.Warning);
                }
                return true;

            case HubConnectionState.Disconnected:
                snackbar.Add(Resource.Reporting_Hub_is_already_closed, Severity.Success);
                return true;
        }
        return false;
    }

    public void NotifyReportIssuer()
    {
        HubConnection.On("NotifyReportIssuer", (Func<FileMetaData, ReportStatus, Task>)(async (fileMetaData, reportStatus) =>
        {
            switch (reportStatus)
            {
                case ReportStatus.Pending:
                    snackbar.Add(Resource.Your_report_is_being_initiated, Severity.Info);
                    break;

                case ReportStatus.InProgress:
                    snackbar.Add(Resource.Your_report_is_being_generated, Severity.Warning);
                    break;

                case ReportStatus.Completed:
                    snackbar.Add(string.Format(Resource.Your_report_0_is_ready_to_download, fileMetaData.FileName), Severity.Success);
                    await ShowDownloadFileDialogue(fileMetaData);
                    break;

                case ReportStatus.Failed:
                    snackbar.Add(Resource.Your_report_generation_has_failed, Severity.Error);
                    break;

                case ReportStatus.NotFound:
                    snackbar.Add(Resource.There_are_no_data_to_generate_report, Severity.Error);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(reportStatus), reportStatus, null);
            }
        }));
    }

    public void RefreshReportsViewer()
    {
        HubConnection.On("RefreshReportsViewer", () => ReportStatusReceived?.Invoke());
    }

    public async Task ShowDownloadFileDialogue(FileMetaData fileMetaData)
    {
        var dialogParameters = new DialogParameters
                               {
                                   {"ContentText", Resource.Your_report_is_ready_to_download},
                                   {"ButtonText", Resource.Download},
                                   {"Color", Color.Error}
                               };

        var dialogOptions = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

        var dialog = await dialogService.ShowAsync<GenericDialog>(Resource.Export, dialogParameters, dialogOptions);

        var dialogResult = await dialog.Result;

        if (!dialogResult.Canceled)
            await jsRuntime.InvokeVoidAsync("triggerFileDownload", fileMetaData.FileName, fileMetaData.FileUri);
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

        hubConnectionBuilder.WithUrl($"{urlProvider.BaseHubApiUrl}/Hubs/ReportingServicesHub?Bp-Tenant={subDomain}&Accept-Language={CultureInfo.CurrentCulture}",
                                     options =>
                                     {
                                         //options.Headers.Add("BP-Tenant", subDomain); //Doesn't Work
                                         //options.Headers.Add("Accept-Language", culture); //Doesn't Work.
                                         options.AccessTokenProvider = async () => await accessTokenProvider.TryGetAccessToken();
                                     });

        return hubConnectionBuilder.Build();
    }

    private void HandleHubConnectionError(Exception ex)
    {
        snackbar.Add(string.Format(Resource.Unable_to_connect_to_the_reporting_hub_due_to_an_error, ex.Message), Severity.Error);
    }

    private Task OnHubConnectionClosed(Exception exception)
    {
        snackbar.Add(Resource.Reporting_Hub_is_closed, Severity.Warning);

        return Task.CompletedTask;
    }

    #endregion Private Methods
}