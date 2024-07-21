using BinaryPlate.BlazorPlate.Consumers.HttpClients;
using BinaryPlate.BlazorPlate.Consumers.HttpClients.SSM;
using BinaryPlate.BlazorPlate.Contracts.Consumers.SSM;
using BinaryPlate.BlazorPlate.Features.SSM.Materials.Commands.CreateMaterial;
using BinaryPlate.BlazorPlate.Features.SSM.Materials.Queries.GetMaterials;
using Mapster;

namespace BinaryPlate.BlazorPlate.Pages.SSM.Materials;
public partial class AddMaterial
{
    [Inject] private IMaterialsClient MaterialsClient { get; set; }
    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    private EditContextApiExceptionFallback EditContextApiExceptionFallback { get; set; }
    private MaterialItem MaterialItem { get; set; } = new();
    private CreateMaterialCommand CreateMaterialCommand { get; set; } = new();

    private async Task SubmitForm()
    {
        MaterialItem.Adapt(CreateMaterialCommand);
        var responseWrapper = await MaterialsClient.CreateMaterial(CreateMaterialCommand);
        if (responseWrapper.IsSuccessStatusCode)
        {
            Snackbar.Add(responseWrapper.Payload.SuccessMessage, Severity.Success);
            NavigationManager.NavigateTo("ssm/materials");
        }
        else
        {
            EditContextApiExceptionFallback.PopulateFormErrors(responseWrapper.ApiErrorResponse);
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }
    }
}
