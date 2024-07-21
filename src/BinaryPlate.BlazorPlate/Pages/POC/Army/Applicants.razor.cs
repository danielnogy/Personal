using BinaryPlate.BlazorPlate.Consumers.SignalRClients;

namespace BinaryPlate.BlazorPlate.Pages.POC.Army;

public partial class Applicants
{
    #region Public Properties

    public int ActivePanelIndex { get; set; } = 0;

    #endregion Public Properties

    #region Private Properties

    [Inject] private AppStateManager AppStateManager { get; set; }
    [Inject] private IApplicantsClient ApplicantsClient { get; set; }
    [Inject] private BreadcrumbService BreadcrumbService { get; set; }
    [Inject] private IDialogService DialogService { get; set; }
    [Inject] private IJSRuntime JsRuntime { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }
    [Inject] private ReportingHubClient ReportingHubClient { get; set; }
    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    private string SearchString { get; set; }
    private bool IsHubConnectionClosed { get; set; }
    private GetApplicantsResponse GetApplicantsResponse { get; set; }
    private GetApplicantsQuery GetApplicantsQuery { get; set; } = new();
    private MudTable<ApplicantItem> Table { get; set; }

    #endregion Private Properties

    #region Public Methods

    public async Task CloseHubConnection()
    {
        ActivePanelIndex = 0;

        IsHubConnectionClosed = await ReportingHubClient.CloseHubConnection();
    }

    #endregion Public Methods

    #region Protected Methods

    protected override void OnInitialized()
    {
        BreadcrumbService.SetBreadcrumbItems(new List<BreadcrumbItem>
        {
            new(Resource.Home, "/"),
            new(Resource.Proof_of_Concepts, "#", true),
            new(Resource.Applicants, "#", true)
        });
    }

    #endregion Protected Methods

    #region Private Methods

    private void AddApplicant()
    {
        NavigationManager.NavigateTo("poc/army/addApplicant");
    }

    private void EditApplicant(string id)
    {
        NavigationManager.NavigateTo($"poc/army/editApplicant/{id}");
    }

    private void ViewApplicant(string id)
    {
        NavigationManager.NavigateTo($"poc/army/viewApplicant/{id}");
    }

    private async Task DeleteApplicant(string id)
    {
        var dialog = await DialogService.ShowAsync<DeleteConfirmationDialog>(Resource.Delete);

        var dialogResult = await dialog.Result;

        if (!dialogResult.Canceled)
        {
            var responseWrapper = await ApplicantsClient.DeleteApplicant(id);

            if (responseWrapper.IsSuccessStatusCode)
            {
                Snackbar.Add(responseWrapper.Payload, Severity.Success);
                await Table.ReloadServerData();
            }
            else
            {
                SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
            }
        }
    }

    private async Task<TableData<ApplicantItem>> ServerReload(TableState state)
    {
        ActivePanelIndex = 0;

        GetApplicantsQuery.SearchText = SearchString;

        GetApplicantsQuery.PageNumber = state.Page + 1;

        GetApplicantsQuery.RowsPerPage = state.PageSize;

        GetApplicantsQuery.SortBy = state.SortDirection == SortDirection.None ? string.Empty : $"{state.SortLabel} {state.SortDirection}";

        var responseWrapper = await ApplicantsClient.GetApplicants(GetApplicantsQuery);

        var tableData = new TableData<ApplicantItem>();

        if (responseWrapper.IsSuccessStatusCode)
        {
            if (responseWrapper.Payload != null)
                GetApplicantsResponse = responseWrapper.Payload;

            tableData = new TableData<ApplicantItem>
            {
                TotalItems = GetApplicantsResponse.Applicants.TotalRows,
                Items = GetApplicantsResponse.Applicants.Items
            };
        }
        else
        {
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }

        return tableData;
    }

    private void FilterApplicants(string searchString)
    {
        if (GetApplicantsResponse is null)
            return;

        SearchString = searchString;

        Table.ReloadServerData();
    }

    private async Task ExportApplicantAsPdfOnDemand()
    {
        var dialogParameters = new DialogParameters
            {
                {"ContentText", Resource.Exporting_data_may_take_a_while},
                {"ButtonText", Resource.ExportAsPdfOnDemand},
                {"Color", Color.Error}
            };

        var dialogOptions = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

        var dialog = await DialogService.ShowAsync<GenericDialog>(Resource.Export, dialogParameters, dialogOptions);

        var dialogResult = await dialog.Result;

        if (!dialogResult.Canceled)
        {
            var responseWrapper = await ApplicantsClient.ExportAsPdf(new ExportApplicantsQuery
            {
                SearchText = GetApplicantsQuery.SearchText,
                SortBy = GetApplicantsQuery.SortBy,
                IsOnDemand = true
            });

            if (responseWrapper.IsSuccessStatusCode)
            {
                Snackbar.Add(responseWrapper.Payload.SuccessMessage, Severity.Success);
                await JsRuntime.InvokeVoidAsync("triggerFileDownload", responseWrapper.Payload.FileName, responseWrapper.Payload.FileUri);
            }
            else
            {
                SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
            }
        }
    }

    private async Task ExportApplicantAsPdfInBackground()
    {
        ActivePanelIndex = 0;

        var dialogParameters = new DialogParameters
        {
            {"ContentText", Resource.Exporting_data_may_take_a_while},
            {"ButtonText", Resource.ExportAsPdfInBackground},
            {"Color", Color.Error}
        };

        if (ReportingHubClient.HubConnection.State == HubConnectionState.Connected)
        {
            var dialogOptions = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

            var dialog = await DialogService.ShowAsync<GenericDialog>(Resource.Export, dialogParameters, dialogOptions);

            var dialogResult = await dialog.Result;

            if (!dialogResult.Canceled)
            {
                await ReportingHubClient.HubConnection.SendAsync("ExportApplicantToPdf", new ExportApplicantsQuery
                {
                    SearchText = GetApplicantsQuery.SearchText,
                    SortBy = GetApplicantsQuery.SortBy,
                    IsOnDemand = false
                });

                ActivePanelIndex = 1;
            }
        }
        else
        {
            Snackbar.Add(Resource.Reporting_Hub_is_not_active, Severity.Warning);
        }
    }

    private async Task OnHubConnectionToggledChanged(bool toggled)
    {
        if (toggled)
            await CloseHubConnection();
        else
            await StartHubConnection();
    }

    private async Task StartHubConnection()
    {
        IsHubConnectionClosed = !await ReportingHubClient.StartHubConnection();
    }

    #endregion Private Methods
}