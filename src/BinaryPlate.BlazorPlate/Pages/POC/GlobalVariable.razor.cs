namespace BinaryPlate.BlazorPlate.Pages.POC;

public partial class GlobalVariable
{
    #region Private Properties

    [Inject] private BreadcrumbService BreadcrumbService { get; set; }
    [Inject] private AppStateManager AppStateManager { get; set; }
    [Inject] private IDialogService DialogService { get; set; }

    #endregion Private Properties

    #region Protected Methods

    protected override void OnInitialized()
    {
        BreadcrumbService.SetBreadcrumbItems(new List<BreadcrumbItem>
        {
            new(Resource.Home, "/"),
            new(Resource.Proof_of_Concepts, "#", true),
            new(Resource.Global_Variable, "#", true)
        });

        AppStateManager.PlayAudioChanged += AppStateChanged;
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

    #endregion Private Methods
}