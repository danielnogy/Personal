﻿namespace BinaryPlate.BlazorPlate.Pages.Account;

public partial class RegisterConfirmation
{
    #region Public Properties

    [Parameter] public bool DisplayConfirmAccountLink { get; set; }
    [Parameter] public string EmailConfirmationUrl { get; set; }

    #endregion Public Properties

    #region Private Properties

    [Inject] private NavigationManager NavigationManager { get; set; }

    #endregion Private Properties

    #region Private Methods

    private void ConfirmAccount()
    {
        var emailConfirmationUrlDecoded = WebUtility.UrlDecode(EmailConfirmationUrl);

        NavigationManager.NavigateTo(emailConfirmationUrlDecoded);
    }

    #endregion Private Methods
}