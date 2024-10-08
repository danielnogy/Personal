﻿namespace BinaryPlate.BlazorPlate.Pages.POC.Localization.Properties;

public partial class CurrentCultureInfo
{
    #region Private Properties

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
                                                 new(Resource.Properties, "#", true),
                                                 new(Resource.Culture_Info, "#", true)
                                             });
    }

    #endregion Protected Methods

    #region Private Methods

    private async Task ShowTipsDialog()
    {
        await DialogService.ShowAsync<CultureInfoTipsDialog>();
    }

    #endregion Private Methods
}