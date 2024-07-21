namespace BinaryPlate.BlazorPlate.Pages.Settings;

public partial class TokenSettings
{
    #region Private Properties

    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private IDialogService DialogService { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }
    [Inject] private BreadcrumbService BreadcrumbService { get; set; }
    [Inject] private IAppSettingsClient AppSettingsClient { get; set; }

    private EditContextApiExceptionFallback EditContextApiExceptionFallback { get; set; }
    private GetTokenSettingsForEditResponse TokenSettingsForEdit { get; set; } = new();
    private UpdateTokenSettingsCommand UpdateTokenSettingsCommand { get; set; }

    #endregion Private Properties

    #region Protected Methods

    protected override async Task OnInitializedAsync()
    {
        BreadcrumbService.SetBreadcrumbItems(new List<BreadcrumbItem>
        {
            new(Resource.Home, "/"),
            new(Resource.Settings, "#",true),
            new(Resource.Token_Settings, "#",true)
        });

        var responseWrapper = await AppSettingsClient.GetTokenSettings();

        if (responseWrapper.IsSuccessStatusCode)
        {
            TokenSettingsForEdit = responseWrapper.Payload;
            TokenSettingsForEdit.AccessTokenUoT = 0;
            TokenSettingsForEdit.RefreshTokenUoT = 0;
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
            if (TokenSettingsForEdit.AccessTokenTimeSpan != null)
            {
                var time = TokenSettingsForEdit.AccessTokenTimeSpan.Value;
                switch ((UnitOfTime)TokenSettingsForEdit.AccessTokenUoT)
                {
                    case UnitOfTime.Hours:
                        TokenSettingsForEdit.AccessTokenTimeSpan = TimeSpan.FromHours(time).TotalMinutes;
                        break;

                    case UnitOfTime.Days:
                        TokenSettingsForEdit.AccessTokenTimeSpan = TimeSpan.FromDays(time).TotalMinutes;
                        break;

                    case UnitOfTime.Month:
                        time *= 30;
                        TokenSettingsForEdit.AccessTokenTimeSpan = TimeSpan.FromDays(time).TotalMinutes;
                        break;

                    case UnitOfTime.Minutes:
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (TokenSettingsForEdit.RefreshTokenTimeSpan != null)
            {
                var time = TokenSettingsForEdit.RefreshTokenTimeSpan.Value;
                switch ((UnitOfTime)TokenSettingsForEdit.RefreshTokenUoT)
                {
                    case UnitOfTime.Hours:
                        TokenSettingsForEdit.RefreshTokenTimeSpan = TimeSpan.FromHours(time).TotalMinutes;
                        break;

                    case UnitOfTime.Days:
                        TokenSettingsForEdit.RefreshTokenTimeSpan = TimeSpan.FromDays(time).TotalMinutes;
                        break;

                    case UnitOfTime.Month:
                        time *= 30;
                        TokenSettingsForEdit.RefreshTokenTimeSpan = TimeSpan.FromDays(time).TotalMinutes;
                        break;

                    case UnitOfTime.Minutes:
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            UpdateTokenSettingsCommand = new UpdateTokenSettingsCommand
            {
                Id = TokenSettingsForEdit.Id,
                AccessTokenTimeSpan = TokenSettingsForEdit.AccessTokenTimeSpan,
                RefreshTokenUoT = TokenSettingsForEdit.RefreshTokenUoT,
                AccessTokenUoT = TokenSettingsForEdit.AccessTokenUoT,
                RefreshTokenTimeSpan = TokenSettingsForEdit.RefreshTokenTimeSpan,
                ConcurrencyStamp = TokenSettingsForEdit.ConcurrencyStamp
            };

            var responseWrapper = await AppSettingsClient.UpdateTokenSettings(UpdateTokenSettingsCommand);

            if (responseWrapper.IsSuccessStatusCode)
            {
                TokenSettingsForEdit.AccessTokenUoT = 0;
                TokenSettingsForEdit.RefreshTokenUoT = 0;

                TokenSettingsForEdit.Id = responseWrapper.Payload.Id;
                TokenSettingsForEdit.ConcurrencyStamp = responseWrapper.Payload.ConcurrencyStamp;

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