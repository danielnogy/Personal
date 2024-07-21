using BinaryPlate.BlazorPlate.Contracts.Consumers.SSM;
using BinaryPlate.BlazorPlate.Features.SSM.MaterialCategories.Commands.CreateMaterialCategory;
using BinaryPlate.BlazorPlate.Features.SSM.MaterialCategories.Queries.GetMaterialCategoriess;
using Mapster;

namespace BinaryPlate.BlazorPlate.Pages.SSM.MaterialCategories;
public partial class AddMaterialCategory
{
    [Inject] private IMaterialCategoriesClient MaterialCategoriesClient { get; set; }
    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    private EditContextApiExceptionFallback EditContextApiExceptionFallback { get; set; }
    private MaterialCategoryItem MaterialCategoryItem { get; set; } = new();
    private CreateMaterialCategoryCommand CreateMaterialCategoryCommand { get; set; } = new();

    private async Task SubmitForm()
    {
        MaterialCategoryItem.Adapt(CreateMaterialCategoryCommand);
        var responseWrapper = await MaterialCategoriesClient.CreateMaterialCategory(CreateMaterialCategoryCommand);
        if (responseWrapper.IsSuccessStatusCode)
        {
            Snackbar.Add(responseWrapper.Payload.SuccessMessage, Severity.Success);
            NavigationManager.NavigateTo("ssm/materialCategories");
        }
        else
        {
            EditContextApiExceptionFallback.PopulateFormErrors(responseWrapper.ApiErrorResponse);
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }
    }
}
