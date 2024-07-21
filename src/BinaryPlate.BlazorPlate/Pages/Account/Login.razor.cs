namespace BinaryPlate.BlazorPlate.Pages.Account;

public partial class Login
{
    #region Private Properties

    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private ReturnUrlProvider ReturnUrlProvider { get; set; }
    [Inject] private UserPasswordService UserPasswordService { get; set; }
    [Inject] private AuthenticationService AuthenticationService { get; set; }
    [Inject] private IAccountsClient AccountsClient { get; set; }
    private EditContextApiExceptionFallback EditContextApiExceptionFallback { get; set; }
    private LoginCommand LoginCommand { get; } = new();

    #endregion Private Properties

    #region Protected Methods

    protected override void OnInitialized()
    {
        LoginCommand.Email = "danielnagy112@yahoo.com";
        LoginCommand.Password = "1qazZAQ!";
    }

    #endregion Protected Methods

    #region Private Methods

    private async Task LoginUser()
    {
        var responseWrapper = await AccountsClient.Login(LoginCommand);

        if (responseWrapper.IsSuccessStatusCode)
        {
            switch (responseWrapper.Payload.RequiresTwoFactor)
            {
                case true:
                    Handle2Fa();
                    break;

                default:
                {
                    await AuthenticationService.Login(responseWrapper.Payload.AuthResponse);
                    var returnUrl = await ReturnUrlProvider.GetReturnUrl();
                    NavigationManager.NavigateTo(returnUrl);
                    break;
                }
            }
        }
        else
        {
            EditContextApiExceptionFallback.PopulateFormErrors(responseWrapper.ApiErrorResponse);
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }
    }

    private void Handle2Fa()
    {
        UserPasswordService.SetUserPassword(LoginCommand.Password);
        NavigationManager.NavigateTo($"account/loginWith2Fa/{LoginCommand.Email}");
    }

    #endregion Private Methods
}