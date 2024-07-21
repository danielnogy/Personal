namespace BinaryPlate.BlazorPlate.Pages.Account;

public partial class TwoFactorAuthRedirect
{
    #region Private Properties

    [Inject] private NavigationManager NavigationManager { get; set; }

    #endregion Private Properties

    #region Private Methods

    private void RedirectTo2Fa()
    {
        NavigationManager.NavigateTo("/account/manage/index");
    }

    #endregion Private Methods
}