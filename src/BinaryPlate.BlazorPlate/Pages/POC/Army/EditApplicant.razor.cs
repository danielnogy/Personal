namespace BinaryPlate.BlazorPlate.Pages.POC.Army;

public partial class EditApplicant
{
    #region Public Properties

    [Parameter] public string ApplicantId { get; set; }

    public List<ReferenceItemForAdd> AddedApplicantReferencesList { get; set; } = new();
    public List<ReferenceItemForEdit> ModifiedApplicantReferencesList { get; set; } = new();
    public List<string> RemovedApplicantReferencesList { get; set; } = new();

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
            new(Resource.Applicants, "/poc/army/applicants"),
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
                ConcurrencyStamp = ApplicantForEdit.ConcurrencyStamp,
                NewApplicantReferences = AddedApplicantReferencesList,
                ModifiedApplicantReferences = ModifiedApplicantReferencesList,
                RemovedApplicantReferences = RemovedApplicantReferencesList
            };
            var responseWrapper = await ApplicantsClient.UpdateApplicant(UpdateApplicantCommand);

            if (responseWrapper.IsSuccessStatusCode)
            {
                Snackbar.Add(responseWrapper.Payload, Severity.Success);
                NavigationManager.NavigateTo("poc/army/applicants");
            }
            else
            {
                EditContextApiExceptionFallback.PopulateFormErrors(responseWrapper.ApiErrorResponse);
                SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
            }
        }
    }

    private void RefreshNewApplicantReferencesList(List<ReferenceItemForAdd> referenceItems)
    {
        AddedApplicantReferencesList = referenceItems;
    }

    private void RefreshModifiedApplicantReferencesList(List<ReferenceItemForEdit> referenceItems)
    {
        ModifiedApplicantReferencesList = referenceItems;
    }

    private void RefreshRemovedApplicantReferencesList(List<string> referenceItemsIds)
    {
        RemovedApplicantReferencesList = referenceItemsIds;
    }

    #endregion Private Methods
}