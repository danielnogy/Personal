using BinaryPlate.BlazorPlate.Contracts.Consumers.SSM;
using BinaryPlate.BlazorPlate.Features.SSM.Materials.Commands.UpdateMaterial;
using BinaryPlate.BlazorPlate.Features.SSM.Materials.Queries.GetMaterialForEdit;
using BinaryPlate.BlazorPlate.Features.SSM.Materials.Queries.GetMaterials;
using Mapster;

namespace BinaryPlate.BlazorPlate.Pages.SSM.Materials
{
    public partial class EditMaterial
    {
        [Parameter] public int MaterialId { get; set; } = 0;
        public MaterialItem MaterialParameter { get; set; } = new();
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IMaterialsClient MaterialsClient { get; set; }
        [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }
        private EditContextApiExceptionFallback EditContextApiExceptionFallback { get; set; }
        private GetMaterialForEditResponse MaterialForEdit { get; set; } = new();
        private UpdateMaterialCommand UpdateMaterialCommand { get; set; } = new();
        protected override async Task OnInitializedAsync()
        {
            var responseWrapper = await MaterialsClient.GetMaterial(new GetMaterialForEditQuery
            {
                Id = MaterialId,
            });

            if (responseWrapper.IsSuccessStatusCode)
                MaterialParameter = responseWrapper.Payload.Adapt<MaterialItem>();
            else
                SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }
        public async Task SubmitForm()
        {
            MaterialParameter.Adapt(UpdateMaterialCommand);

            var responseWrapper = await MaterialsClient.UpdateMaterial(UpdateMaterialCommand);

            if (responseWrapper.IsSuccessStatusCode)
            {
                Snackbar.Add(responseWrapper.Payload, Severity.Success);
                NavigationManager.NavigateTo("/ssm/Materials");
            }
            else
            {
                EditContextApiExceptionFallback.PopulateFormErrors(responseWrapper.ApiErrorResponse);
                SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
            }
        }
    }
}