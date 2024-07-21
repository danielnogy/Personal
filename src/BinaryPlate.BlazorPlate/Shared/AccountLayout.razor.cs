namespace BinaryPlate.BlazorPlate.Shared;

public partial class AccountLayout
{
    #region Private Properties

    private bool IsDarkMode { get; set; } = true;
    private bool IsRightToLeft { get; set; }
    private MudTheme DefaultTheme { get; set; } = new BpAdminDashboardTheme();
    [Inject] private NavigationManager NavigationManager { get; set; }

    [Inject] private AppStateManager AppStateManager { get; set; }

    #endregion Private Properties

    #region Protected Methods

    protected override void OnInitialized()
    {
        var culture = CultureInfo.CurrentCulture;

        IsRightToLeft = culture.TextInfo.IsRightToLeft;

        NavigationManager.LocationChanged += (obj, nav) => { StateHasChanged(); };

        AppStateManager.LoaderOverlayChanged += (obj, nav) => { StateHasChanged(); };
    }

    protected void DarkModeToggle()
    {
        IsDarkMode = !IsDarkMode;
    }

    #endregion Protected Methods
}