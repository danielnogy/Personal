namespace BinaryPlate.BlazorPlate.Pages.Account;

public partial class Register
{
    #region Private Properties

    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private ReturnUrlProvider ReturnUrlProvider { get; set; }
    [Inject] private IAccountsClient AccountsClient { get; set; }
    [Inject] private AuthenticationService AuthenticationService { get; set; }

    private bool PasswordVisibility { get; set; }
    private string PasswordInputIcon { get; set; } = Icons.Material.Filled.VisibilityOff;
    private InputType PasswordInput { get; set; } = InputType.Password;
    private EditContextApiExceptionFallback EditContextApiExceptionFallback { get; set; }
    private RegisterCommand RegisterCommand { get; } = new();

    #endregion Private Properties

    #region Private Methods

    private void TogglePasswordVisibility()
    {
        if (PasswordVisibility)
        {
            PasswordVisibility = false;
            PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
            PasswordInput = InputType.Password;
        }
        else
        {
            PasswordVisibility = true;
            PasswordInputIcon = Icons.Material.Filled.Visibility;
            PasswordInput = InputType.Text;
        }
    }

    private async Task RegisterUser()
    {
        var responseWrapper = await AccountsClient.Register(RegisterCommand);

        if (responseWrapper.IsSuccessStatusCode)
        {
            if (responseWrapper.Payload.RequireConfirmedAccount)
            {
                var confirmationUrlEncoded = responseWrapper.Payload.DisplayConfirmAccountLink 
                    ? $"account/registerConfirmation/{responseWrapper.Payload.DisplayConfirmAccountLink}/{WebUtility.UrlEncode(responseWrapper.Payload.EmailConfirmationUrl)}" 
                    : "account/registerConfirmation";

                NavigationManager.NavigateTo(confirmationUrlEncoded);
            }
            else
            {
                await AuthenticationService.Login(responseWrapper.Payload.AuthResponse);
                var returnUrl = await ReturnUrlProvider.GetReturnUrl();
                await ReturnUrlProvider.RemoveReturnUrl();
                NavigationManager.NavigateTo(returnUrl, forceLoad: true);
            }
        }
        else
        {
            EditContextApiExceptionFallback.PopulateFormErrors(responseWrapper.ApiErrorResponse);
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }
    }

    #endregion Private Methods
}