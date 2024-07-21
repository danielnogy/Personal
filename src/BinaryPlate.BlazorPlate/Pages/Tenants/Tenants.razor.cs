namespace BinaryPlate.BlazorPlate.Pages.Tenants;

public partial class Tenants
{
    #region Public Properties

    [CascadingParameter] public Task<AuthenticationState> AuthenticationState { get; set; }

    #endregion Public Properties

    #region Private Properties

    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }
    [Inject] private IDialogService DialogService { get; set; }
    [Inject] private BreadcrumbService BreadcrumbService { get; set; }
    [Inject] private ITenantsClient TenantsClient { get; set; }
    [Inject] private NavigationService NavigationService { get; set; }

    private string SearchString { get; set; }
    private MudTable<TenantItem> Table { get; set; }
    private GetTenantsQuery GetTenantsQuery { get; } = new();
    private GetTenantsResponse GetTenantsResponse { get; set; }

    #endregion Private Properties

    #region Protected Methods

    protected override void OnInitialized()
    {
        BreadcrumbService.SetBreadcrumbItems(new List<BreadcrumbItem>
        {
            new(Resource.Home, "/"),
            new("Tenants", "#",true)
        });
    }

    #endregion Protected Methods

    #region Private Methods

    private async Task<TableData<TenantItem>> ServerReload(TableState state)
    {
        GetTenantsQuery.SearchText = SearchString;

        GetTenantsQuery.PageNumber = state.Page + 1;

        GetTenantsQuery.RowsPerPage = state.PageSize;

        GetTenantsQuery.SortBy = state.SortDirection == SortDirection.None ? string.Empty : $"{state.SortLabel} {state.SortDirection}";

        var responseWrapper = await TenantsClient.GetTenants(GetTenantsQuery);

        var tableData = new TableData<TenantItem>();

        if (responseWrapper.IsSuccessStatusCode)
        {
            GetTenantsResponse = responseWrapper.Payload;
            tableData = new TableData<TenantItem> { TotalItems = GetTenantsResponse.Tenants.TotalRows, Items = GetTenantsResponse.Tenants.Items };
        }
        else
        {
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }
        return tableData;
    }

    private void FilterTenants(string searchString)
    {
        if (GetTenantsResponse is null)
            return;
        SearchString = searchString;
        Table.ReloadServerData();
    }

    private void AddTenant()
    {
        NavigationManager.NavigateTo("tenants/addTenant");
    }

    private void EditTenant(string id)
    {
        NavigationManager.NavigateTo($"tenants/editTenant/{id}");
    }

    private async Task DeleteTenant(string id)
    {
        var dialog = await DialogService.ShowAsync<DeleteConfirmationDialog>(Resource.Delete);

        var dialogResult = await dialog.Result;

        if (!dialogResult.Canceled)
        {
            var responseWrapper = await TenantsClient.DeleteTenant(id);

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

    #endregion Private Methods
}