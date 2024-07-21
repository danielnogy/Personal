﻿namespace BinaryPlate.BlazorPlate.Pages.POC.Authorization.ServerSideAuthorization;

public partial class EditApplicant
{
    #region Public Properties

    [Parameter] public string ApplicantId { get; set; }

    #endregion Public Properties

    #region Private Properties

    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private IDialogService DialogService { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }
    [Inject] private BreadcrumbService BreadcrumbService { get; set; }
    [Inject] private IApplicantsClient ApplicantsClient { get; set; }

    private EditContextApiExceptionFallback EditContextApiExceptionFallback { get; set; }
    private GetApplicantForEditResponse ApplicantForEdit { get; set; } = new();
    private UpdateApplicantCommand UpdateApplicantCommand { get; set; }

    #endregion Private Properties

    #region Protected Methods

    protected override async Task OnInitializedAsync()
    {
        BreadcrumbService.SetBreadcrumbItems(new List<BreadcrumbItem>
        {
            new(Resource.Home, "/"),
            new(Resource.Proof_of_Concepts, "#", true),
            new(Resource.Authorization, "#", true),
            new(Resource.Server_Side_Authorization, "#", true),
            new(Resource.Applicants, "/poc/authorization/serverSideAuthorization/applicants"),
            new(Resource.Edit_Applicant, "#", true)
        });

        var responseWrapper = await ApplicantsClient.GetApplicant(new GetApplicantForEditQuery
        {
            Id = ApplicantId,
        });

        if (responseWrapper.IsSuccessStatusCode)
            ApplicantForEdit = responseWrapper.Payload;
        else
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
    }

    #endregion Protected Methods

    #region Private Methods

    private async Task SubmitForm()
    {
        var dialog = await DialogService.ShowAsync<SaveConfirmationDialog>(Resource.Confirm);

        var dialogResult = await dialog.Result;

        if (!dialogResult.Canceled)
        {
            UpdateApplicantCommand = new UpdateApplicantCommand
            {
                Id = ApplicantForEdit.Id,
                Ssn = ApplicantForEdit.Ssn,
                FirstName = ApplicantForEdit.FirstName,
                LastName = ApplicantForEdit.LastName,
                DateOfBirth = ApplicantForEdit.DateOfBirth,
                Height = ApplicantForEdit.Height,
                Weight = ApplicantForEdit.Weight,
            };
            var responseWrapper = await ApplicantsClient.UpdateApplicant(UpdateApplicantCommand);

            if (responseWrapper.IsSuccessStatusCode)
            {
                Snackbar.Add(responseWrapper.Payload, Severity.Success);
                NavigationManager.NavigateTo("poc/authorization/serverSideAuthorization/applicants");
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