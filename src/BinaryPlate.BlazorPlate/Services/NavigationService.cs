namespace BinaryPlate.BlazorPlate.Services;

public class NavigationService(IJSRuntime jsRuntime, NavigationManager navigationManager) 
{
    #region Public Methods

    public async Task NavigateToUrlAsync(string url, bool openInNewTab)
    {
        if (openInNewTab)
            await jsRuntime.InvokeVoidAsync("open", url, "_blank");
        else
            await jsRuntime.InvokeVoidAsync("open", url);
    }

    public async Task NavigateToTenantPortalAsync(string tenantName, bool redirectToRegistration, bool openInNewTab)
    {
        var url = navigationManager.BaseUri.Replace("//", $"//{tenantName}.");

        if (redirectToRegistration)
            url = $"{url}account/register";

        if (openInNewTab)
            await jsRuntime.InvokeVoidAsync("open", url, "_blank");
        else
            await jsRuntime.InvokeVoidAsync("open", url);
    }

    #endregion Public Methods
}