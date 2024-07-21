using BinaryPlate.BlazorPlate.Consumers.SignalRClients;
using BinaryPlate.BlazorPlate.Providers;

namespace BinaryPlate.BlazorPlate.Pages.Reporting;

public partial class Reports : IAsyncDisposable
{
    #region Private Properties

    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; }
    [Inject] private ReportingHubClient ReportingHubClient { get; set; }
    [Inject] private AccessTokenProvider AccessTokenProvider { get; set; }
    [Inject] private UrlProvider UrlProvider { get; set; }
    [Inject] private BreadcrumbService BreadcrumbService { get; set; }
    [Inject] private IDialogService DialogService { get; set; }
    [Inject] private IJSRuntime Js { get; set; }
    [Inject] private IReportsClient ReportsClient { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }

    private string SearchString { get; set; }
    private GetReportsQuery GetReportsQuery { get; set; } = new();
    private GetReportForEditResponse GetReportForEditResponse { get; set; } = new();
    private GetReportsResponse GetReportsResponse { get; set; }
    private MudTable<ReportItem> Table { get; set; }
    private ReportStatus? SelectedReportStatus { get; set; }

    #endregion Private Properties

    #region Public Methods

    public async ValueTask DisposeAsync()
    {
        await ReportingHubClient.StartHubConnection();
    }

    #endregion Public Methods

    #region Protected Methods

    protected override async Task OnInitializedAsync()
    {
        BreadcrumbService.SetBreadcrumbItems(new List<BreadcrumbItem>
        {
            new(Resource.Home, "/"),
            new(Resource.Reporting, "#",true),
            new(Resource.Reports, "#", true)
        });

        var userIdentity = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Identity;

        if (userIdentity is { IsAuthenticated: true })
        {
            await ReportingHubClient.StartHubConnection();

            ReportingHubClient.RefreshReportsViewer();

            ReportingHubClient.ReportStatusReceived += HandleReportStatusReceived;
        }
    }

    #endregion Protected Methods

    #region Private Methods

    private void HandleReportStatusReceived()
    {
        Table.ReloadServerData();
    }

    private async Task DownloadReport(string reportFileName, string reportFileUri)
    {
        await Js.InvokeVoidAsync("triggerFileDownload", reportFileName, reportFileUri);
    }

    private void FilterReports(string searchString, ReportStatus? reportStatus)
    {
        SearchString = searchString;
        SelectedReportStatus = reportStatus;
        Table.ReloadServerData();
    }

    private string GetReportStatus(ReportStatus reportStatus)
    {
        switch (reportStatus)
        {
            case ReportStatus.Pending:
                return Resource.Pending;

            case ReportStatus.InProgress:
                return Resource.In_Progress;

            case ReportStatus.Completed:
                return Resource.Completed;

            case ReportStatus.Failed:
                return Resource.Failed;

            default:
                return Resource.All;
        }
    }

    private async Task<TableData<ReportItem>> ServerReload(TableState state)
    {
        GetReportsQuery.SearchText = SearchString;

        GetReportsQuery.SelectedReportStatus = SelectedReportStatus;

        GetReportsQuery.PageNumber = state.Page + 1;

        GetReportsQuery.RowsPerPage = state.PageSize;

        GetReportsQuery.SortBy = state.SortDirection == SortDirection.None
            ? string.Empty
            : $"{state.SortLabel} {state.SortDirection}";

        var responseWrapper = await ReportsClient.GetReports(GetReportsQuery);

        var tableData = new TableData<ReportItem>();

        if (responseWrapper.IsSuccessStatusCode)
        {
            if (responseWrapper.Payload != null)
                GetReportsResponse = responseWrapper.Payload;

            tableData = new TableData<ReportItem>
            {
                TotalItems = GetReportsResponse.Reports.TotalRows,
                Items = GetReportsResponse.Reports.Items
            };
        }
        else
        {
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }

        return tableData;
    }

    private async Task ViewReport(string id)
    {
        var responseWrapper = await ReportsClient.GetReport(new GetReportForEditQuery { Id = id });

        if (responseWrapper.IsSuccessStatusCode)
        {
            if (responseWrapper.Payload != null)
                GetReportForEditResponse = responseWrapper.Payload;

            var dialogParameters = new DialogParameters { ["ReportForEdit"] = GetReportForEditResponse };

            var dialogOptions = new DialogOptions
            {
                CloseButton = true,
                MaxWidth = MaxWidth.ExtraExtraLarge,
                CloseOnEscapeKey = true,
                FullScreen = true
            };

            await DialogService.ShowAsync<ViewReportDialog>(Resource.Report_Details, dialogParameters, dialogOptions);
        }
        else
        {
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }
    }

    #endregion Private Methods
}