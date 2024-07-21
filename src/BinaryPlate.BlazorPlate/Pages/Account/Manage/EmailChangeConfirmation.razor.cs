namespace BinaryPlate.BlazorPlate.Pages.Account.Manage;

public partial class EmailChangeConfirmation
{
    #region Public Properties

    [Parameter] public bool DisplayConfirmAccountLink { get; set; }
    [Parameter] public string EmailConfirmationUrl { get; set; }

    #endregion Public Properties

    #region Private Properties

    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private AuthenticationService AuthenticationService { get; set; }

    #endregion Private Properties
    
    #region Private Methods

    private async Task RedirectToEmailConfirmationUrl(string emailConfirmationUrl)
    {
        await AuthenticationService.Logout();

        var emailConfirmationUrlDecoded= WebUtility.UrlDecode(emailConfirmationUrl);

        NavigationManager.NavigateTo(emailConfirmationUrlDecoded);
    }

    #endregion Private Methods
}