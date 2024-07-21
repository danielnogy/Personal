namespace BinaryPlate.BlazorPlate.Pages.Settings;

public partial class IdentitySettings
{
    #region Private Properties

    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private IDialogService DialogService { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }
    [Inject] private BreadcrumbService BreadcrumbService { get; set; }
    [Inject] private IAppSettingsClient AppSettingsClient { get; set; }

    private EditContextApiExceptionFallback EditContextApiExceptionFallback { get; set; }
    private GetIdentitySettingsForEditResponse IdentitySettingsForEdit { get; set; } = new();
    private UpdateIdentitySettingsCommand UpdateIdentitySettingsCommand { get; set; }

    #endregion Private Properties

    #region Protected Methods

    protected override async Task OnInitializedAsync()
    {
        BreadcrumbService.SetBreadcrumbItems(new List<BreadcrumbItem>
        {
            new(Resource.Home, "/"),
            new(Resource.Settings, "#",true),
            new(Resource.Identity_Settings, "#",true)
        });

        var responseWrapper = await AppSettingsClient.GetIdentitySettings();

        if (responseWrapper.IsSuccessStatusCode)
        {
            var identitySettingsForEdit = responseWrapper.Payload;
            IdentitySettingsForEdit = identitySettingsForEdit;
        }
        else
        {
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }
    }

    #endregion Protected Methods

    #region Private Methods

    private async Task SubmitForm()
    {
        var dialog = await DialogService.ShowAsync<SaveConfirmationDialog>(Resource.Confirm);

        var dialogResult = await dialog.Result;

        if (!dialogResult.Canceled)
        {
            UpdateIdentitySettingsCommand = new UpdateIdentitySettingsCommand
            {
                UserSettingsModel = IdentitySettingsForEdit.UserSettingsForEdit.MapToCommand(),
                LockoutSettingsModel = IdentitySettingsForEdit.LockoutSettingsForEdit.MapToCommand(),
                PasswordSettingsModel = IdentitySettingsForEdit.PasswordSettingsForEdit.MapToCommand(),
                SignInSettingsModel = IdentitySettingsForEdit.SignInSettingsForEdit.MapToCommand(),
            };

            var responseWrapper = await AppSettingsClient.UpdateIdentitySettings(UpdateIdentitySettingsCommand);

            if (responseWrapper.IsSuccessStatusCode)
            {
                IdentitySettingsForEdit.UserSettingsForEdit.Id = responseWrapper.Payload.UserSettingsId;
                IdentitySettingsForEdit.LockoutSettingsForEdit.Id = responseWrapper.Payload.LockoutSettingsId;
                IdentitySettingsForEdit.PasswordSettingsForEdit.Id = responseWrapper.Payload.PasswordSettingsId;
                IdentitySettingsForEdit.SignInSettingsForEdit.Id = responseWrapper.Payload.SignInSettingsId;

                IdentitySettingsForEdit.UserSettingsForEdit.ConcurrencyStamp = responseWrapper.Payload.UserSettingsConcurrencyStamp;
                IdentitySettingsForEdit.LockoutSettingsForEdit.ConcurrencyStamp = responseWrapper.Payload.LockoutSettingsConcurrencyStamp;
                IdentitySettingsForEdit.PasswordSettingsForEdit.ConcurrencyStamp = responseWrapper.Payload.PasswordSettingsConcurrencyStamp;
                IdentitySettingsForEdit.SignInSettingsForEdit.ConcurrencyStamp = responseWrapper.Payload.SignInSettingsConcurrencyStamp;

                Snackbar.Add(responseWrapper.Payload.SuccessMessage, Severity.Success);
            }
            else
            {
                EditContextApiExceptionFallback.PopulateFormErrors(responseWrapper.ApiErrorResponse);
                SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
            }
        }
    }

    #endregion Private Methods
}