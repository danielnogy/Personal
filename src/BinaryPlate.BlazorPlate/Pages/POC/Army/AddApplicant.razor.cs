﻿namespace BinaryPlate.BlazorPlate.Pages.POC.Army;

public partial class AddApplicant
{
    #region Private Properties

    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }
    [Inject] private IApplicantsClient ApplicantsClient { get; set; }
    [Inject] private BreadcrumbService BreadcrumbService { get; set; }
    [Inject] private IDialogService DialogService { get; set; }

    private EditContextApiExceptionFallback EditContextApiExceptionFallback { get; set; }
    private CreateApplicantCommand CreateApplicantCommand { get; set; } = new();

    #endregion Private Properties

    #region Protected Methods

    protected override void OnInitialized()
    {
        BreadcrumbService.SetBreadcrumbItems(new List<BreadcrumbItem>
        {
            new(Resource.Home, "/"),
            new(Resource.Proof_of_Concepts, "#", true),
            new(Resource.Applicants, "poc/army/applicants"),
            new(Resource.Add_Applicant, "#", true)
        });

        CreateApplicantCommand.DateOfBirth = DateTime.Now.AddYears(-18);
    }

    #endregion Protected Methods

    #region Private Methods

    private async Task ShowTipsDialog()
    {
        await DialogService.ShowAsync<AddApplicantsTipsDialog>();
    }

    private async void AppStateChanged(object sender, EventArgs args)
    {
        await InvokeAsync(StateHasChanged);
    }

    private async Task SubmitForm()
    {
        var responseWrapper = await ApplicantsClient.CreateApplicant(CreateApplicantCommand);
        if (responseWrapper.IsSuccessStatusCode)
        {
            Snackbar.Add(responseWrapper.Payload.SuccessMessage, Severity.Success);
            NavigationManager.NavigateTo("poc/army/applicants");
        }
        else
        {
            EditContextApiExceptionFallback.PopulateFormErrors(responseWrapper.ApiErrorResponse);
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }
    }

    private void UpdateApplicantReferences(List<ReferenceItemForAdd> referenceItems)
    {
        CreateApplicantCommand.ReferenceItems = referenceItems;
    }

    #endregion Private Methods
}