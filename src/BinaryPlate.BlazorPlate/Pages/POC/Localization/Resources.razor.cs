namespace BinaryPlate.BlazorPlate.Pages.POC.Localization;

public partial class Resources
{
    #region Private Properties

    [Inject] private LocalizationService LocalizationService { get; set; }
    [Inject] private BreadcrumbService BreadcrumbService { get; set; }
    [Inject] private IDialogService DialogService { get; set; }


    #endregion Private Properties

    #region Protected Methods

    protected override void OnInitialized()
    {
        BreadcrumbService.SetBreadcrumbItems(new List<BreadcrumbItem>
        {
            new(Resource.Home, "/"),
            new(Resource.Proof_of_Concepts, "#", true),
            new(Resource.Localization, "#", true),
            new(Resource.Resources, "#", true)
        });
    }

    #endregion Protected Methods

    #region Private Methods

    private async Task ShowTipsDialog()
    {
        await DialogService.ShowAsync<ResourcesTipsDialog>();
    }

    #endregion Private Methods
}