﻿namespace BinaryPlate.BlazorPlate.Pages.Settings;

public partial class FileStorageSettings
{
    #region Public Properties

    public bool StorageType { get; set; }

    #endregion Public Properties

    #region Private Properties

    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private IDialogService DialogService { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }
    [Inject] private BreadcrumbService BreadcrumbService { get; set; }
    [Inject] private IAppSettingsClient AppSettingsClient { get; set; }

    private bool FileStorageSettingsLoaded { get; set; }
    private EditContextApiExceptionFallback EditContextApiExceptionFallback { get; set; }
    private UpdateFileStorageSettingsCommand UpdateFileStorageSettingsCommand { get; set; }
    private GetFileStorageSettingsForEditResponse FileStorageSettingsForEdit { get; set; } = new();

    #endregion Private Properties

    #region Public Methods

    public void OnToggledChanged(bool toggled)
    {
        // Because variable is not two-way bound, we need to update it manually.
        StorageType = toggled;
        FileStorageSettingsForEdit.StorageType = Convert.ToInt32(StorageType);
    }

    #endregion Public Methods

    #region Protected Methods

    protected override async Task OnInitializedAsync()
    {
        BreadcrumbService.SetBreadcrumbItems(new List<BreadcrumbItem>
        {
            new(Resource.Home, "/"),
            new(Resource.Settings, "#",true),
            new(Resource.File_Storage_Settings, "#",true)
        });

        var responseWrapper = await AppSettingsClient.GetFileStorageSettings();

        if (responseWrapper.IsSuccessStatusCode)
        {
            FileStorageSettingsForEdit = responseWrapper.Payload;
            StorageType = Convert.ToBoolean(FileStorageSettingsForEdit.StorageType);
            FileStorageSettingsLoaded = true;
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
            UpdateFileStorageSettingsCommand = new UpdateFileStorageSettingsCommand
            {
                Id = FileStorageSettingsForEdit.Id,
                StorageType = FileStorageSettingsForEdit.StorageType,
                ConcurrencyStamp = FileStorageSettingsForEdit.ConcurrencyStamp
            };

            var responseWrapper = await AppSettingsClient.UpdateFileStorageSettings(UpdateFileStorageSettingsCommand);

            if (responseWrapper.IsSuccessStatusCode)
            {
                FileStorageSettingsForEdit.Id = responseWrapper.Payload.Id;
                FileStorageSettingsForEdit.ConcurrencyStamp = responseWrapper.Payload.ConcurrencyStamp;

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