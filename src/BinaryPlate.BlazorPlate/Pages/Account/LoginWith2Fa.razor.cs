namespace BinaryPlate.BlazorPlate.Pages.Account;

public partial class LoginWith2Fa
{
    #region Public Properties

    [Parameter] public string UserName { get; set; }

    #endregion Public Properties

    #region Private Properties

    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private AuthenticationService AuthenticationService { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private ReturnUrlProvider ReturnUrlProvider { get; set; }
    [Inject] private UserPasswordService UserPasswordService { get; set; }
    [Inject] private IAccountsClient AccountsClient { get; set; }
    [Inject] private ILocalStorageService LocalStorageService { get; set; }

    private string RecoveryCodeUrl { get; set; }
    private EditContextApiExceptionFallback EditContextApiExceptionFallback { get; set; }
    private LoginWith2FaCommand LoginWith2FaCommand { get; } = new();

    #endregion Private Properties

    #region Protected Methods

    protected override void OnInitialized()
    {
        RecoveryCodeUrl = $"/account/LoginWithRecoveryCode/{UserName}";
    }

    #endregion Protected Methods

    #region Private Methods

    private async Task LoginWith2FaUser()
    {
        LoginWith2FaCommand.UserName = UserName;

        if (UserPasswordService.UserPasswordProvided())
            LoginWith2FaCommand.Password = UserPasswordService.GetUserPassword();

        var responseWrapper = await AccountsClient.LoginWith2Fa(LoginWith2FaCommand);

        if (responseWrapper.IsSuccessStatusCode)
        {
            await AuthenticationService.ReAuthenticate(responseWrapper.Payload.AuthResponse);
            await Handle2FaUri();
            var returnUrl = await ReturnUrlProvider.GetReturnUrl();
            NavigationManager.NavigateTo(returnUrl);
        }
        else
        {
            EditContextApiExceptionFallback.PopulateFormErrors(responseWrapper.ApiErrorResponse);
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }
    }

    private async Task Handle2FaUri()
    {
        var local2FaUri = await LocalStorageService.GetItemAsync<string>("Local2FaUri");
        if (local2FaUri != null)
        {
            await LocalStorageService.SetItemAsync(local2FaUri, local2FaUri);
            await LocalStorageService.RemoveItemAsync("Local2FaUri");
        }
    }

    #endregion Private Methods
}