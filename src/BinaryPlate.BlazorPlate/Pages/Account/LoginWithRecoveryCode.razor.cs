namespace BinaryPlate.BlazorPlate.Pages.Account;

public partial class LoginWithRecoveryCode
{
    #region Public Properties

    [Parameter] public string Username { get; set; }

    #endregion Public Properties

    #region Private Properties

    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private AuthenticationService AuthenticationService { get; set; }
    [Inject] private UserPasswordService UserPasswordService { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private ReturnUrlProvider ReturnUrlProvider { get; set; }
    [Inject] private ILocalStorageService LocalStorageService { get; set; }
    [Inject] private IAccountsClient AccountsClient { get; set; }

    private EditContextApiExceptionFallback EditContextApiExceptionFallback { get; set; }
    private LoginWithRecoveryCodeCommand LoginWithRecoveryCodeCommand { get; } = new();

    #endregion Private Properties

    #region Private Methods

    private async Task LoginWith2FaRecoveryCode()
    {
        LoginWithRecoveryCodeCommand.UserName = Username;

        if (UserPasswordService.UserPasswordProvided())
            LoginWithRecoveryCodeCommand.Password = UserPasswordService.GetUserPassword();

        var responseWrapper = await AccountsClient.LoginWithRecoveryCode(LoginWithRecoveryCodeCommand);

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