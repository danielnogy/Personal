namespace BinaryPlate.BlazorPlate.Pages.Account;

public partial class Logout
{
    #region Private Properties

    [Inject] private AuthenticationService AuthenticationService { get; set; }
    [Inject] private AppStateManager AppStateManager { get; set; }
    [Inject] private IAccountsClient AccountsClient { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }

    #endregion Private Properties

    #region Protected Methods

    protected override async Task OnInitializedAsync()
    {
        AppStateManager.OverlayVisible = true;

        await AuthenticationService.Logout();

        AppStateManager.OverlayVisible = false;

        NavigationManager.NavigateTo("/account/login");
    }

    #endregion Protected Methods
}