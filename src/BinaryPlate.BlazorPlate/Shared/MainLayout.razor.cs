using BinaryPlate.BlazorPlate.Consumers.SignalRClients;

namespace BinaryPlate.BlazorPlate.Shared;

public partial class MainLayout
{
    #region Protected Properties

    [Inject] protected BreadcrumbService BreadcrumbService { get; set; }
    [Inject] protected AppStateManager AppStateManager { get; set; }
    [Inject] protected ISnackbar Snackbar { get; set; }
    [Inject] protected NavigationManager NavigationManager { get; set; }
    [Inject] protected ReportingHubClient ReportingHubClient { get; set; }

    #endregion Protected Properties

    #region Private Properties

    private bool DrawerOpen { get; set; } = true;
    private bool IsRightToLeft { get; set; }
    private bool IsDarkMode { get; set; } = false;
    private MudTheme DefaultTheme { get; set; } = new BpAdminDashboardTheme();

    #endregion Private Properties

    #region Protected Methods

    protected override async Task OnInitializedAsync()
    {
        var culture = CultureInfo.CurrentCulture;

        IsRightToLeft = culture.TextInfo.IsRightToLeft;

        BreadcrumbService.BreadcrumbChanged += (obj, nav) => { StateHasChanged(); };

        AppStateManager.LoaderOverlayChanged += (obj, nav) => { StateHasChanged(); };

        Snackbar.Configuration.PositionClass = !IsRightToLeft ? Defaults.Classes.Position.BottomRight : Defaults.Classes.Position.BottomLeft;

        if (await ReportingHubClient.StartHubConnection())
            ReportingHubClient.NotifyReportIssuer();
    }

    protected void DrawerToggle()
    {
        DrawerOpen = !DrawerOpen;
    }

    protected void DarkModeToggle()
    {
        IsDarkMode = !IsDarkMode;
    }

    #endregion Protected Methods
}