namespace BinaryPlate.Gateway.Pages;

public partial class Index
{
    #region Private Properties

    [Inject] private ITenantsClient TenantsClient { get; set; }
    [Inject] private TenantUrlProvider TenantApiUrlProvider { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }

    private MudForm Form { get; set; }
    private bool Success { get; set; }
    private bool SubmitButtonDisabled { get; set; } = true;
    private bool ShowProgress { get; set; }
    private string TenantUrl { get; set; }
    private string InitialTenantName { get; set; }
    private string FinalTenantName { get; set; }

    private CreateTenantCommand CreateTenantCommand { get; } = new();

    #endregion Private Properties

    #region Private Methods

    private void BuildTenantUrl(string tenantName)
    {
        tenantName = tenantName.ReplaceSpaceAndSpecialCharsWithDashes().ToLower();
        InitialTenantName = tenantName;
        var postfix = $"{DateTime.Now:yyyyMMddHHmmssfff}";
        FinalTenantName = string.IsNullOrWhiteSpace(tenantName) ? postfix : $"{tenantName}-{postfix}";
        TenantUrl = string.Format(TenantApiUrlProvider.TenantUrl, FinalTenantName);
    }

    private string TenantCharLengthValidator(string tenantName)
    {
        if (!string.IsNullOrEmpty(InitialTenantName) && InitialTenantName.ToCharArray().Length >= 20)
            return "Tenant name is too long!";

        SubmitButtonDisabled = false;
        return null;
    }

    private async Task SubmitForm()
    {
        SubmitButtonDisabled = true;

        ShowProgress = true;

        CreateTenantCommand.Name = FinalTenantName;

        var responseWrapper = await TenantsClient.CreateTenant(CreateTenantCommand);

        if (responseWrapper.IsSuccessStatusCode)
        {
            NavigationManager.NavigateTo($"{TenantUrl}/account/login");
        }
        else
        {
            var exceptionResult = responseWrapper.ApiErrorResponse;
            SnackbarApiExceptionProvider.ShowErrors(exceptionResult);
            SubmitButtonDisabled = false;
            ShowProgress = false;
        }
    }

    #endregion Private Methods
}