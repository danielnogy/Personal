using BinaryPlate.BlazorPlate.Contracts.Consumers.SSM;
using BinaryPlate.BlazorPlate.Features.SSM.MaterialCategories.Commands.UpdateMaterialCategory;
using BinaryPlate.BlazorPlate.Features.SSM.MaterialCategories.Queries.GetMaterialCategoriess;
using BinaryPlate.BlazorPlate.Features.SSM.MaterialCategories.Queries.GetMaterialCategoryForEdit;
using Mapster;

namespace BinaryPlate.BlazorPlate.Pages.SSM.MaterialCategories
{
    public partial class EditMaterialCategory
    {
        [Parameter] public int MaterialCategoryId { get; set; } = 0;
        public MaterialCategoryItem MaterialCategoryParameter { get; set; } = new();
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IMaterialCategoriesClient MaterialCategoriesClient { get; set; }
        [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }
        private EditContextApiExceptionFallback EditContextApiExceptionFallback { get; set; }
        private GetMaterialCategoryForEditResponse MaterialCategoryForEdit { get; set; } = new();
        private UpdateMaterialCategoryCommand UpdateMaterialCategoryCommand { get; set; } = new();
        protected override async Task OnInitializedAsync()
        {
            var responseWrapper = await MaterialCategoriesClient.GetMaterialCategory(new GetMaterialCategoryForEditQuery
            {
                Id = MaterialCategoryId,
            });

            if (responseWrapper.IsSuccessStatusCode)
                MaterialCategoryParameter = responseWrapper.Payload.Adapt<MaterialCategoryItem>();
            else
                SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }
        public async Task SubmitForm()
        {
            MaterialCategoryParameter.Adapt(UpdateMaterialCategoryCommand);

            var responseWrapper = await MaterialCategoriesClient.UpdateMaterialCategory(UpdateMaterialCategoryCommand);

            if (responseWrapper.IsSuccessStatusCode)
            {
                Snackbar.Add(responseWrapper.Payload, Severity.Success);
                NavigationManager.NavigateTo("/ssm/MaterialCategories");
            }
            else
            {
                EditContextApiExceptionFallback.PopulateFormErrors(responseWrapper.ApiErrorResponse);
                SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
            }
        }
    }
}