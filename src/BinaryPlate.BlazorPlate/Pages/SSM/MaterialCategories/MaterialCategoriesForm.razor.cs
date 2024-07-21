using BinaryPlate.BlazorPlate.Contracts.Consumers.SSM;
using Syncfusion.Blazor.Popups;
using BinaryPlate.BlazorPlate.Features.SSM.MaterialCategories.Queries.GetMaterialCategoriess;

namespace BinaryPlate.BlazorPlate.Pages.SSM.MaterialCategories
{
    public partial class MaterialCategoriesForm
    {
        #region Public Properties

        [Parameter] public string HeaderText { get; set; }
        [Parameter] public int? MaterialCategoryId { get; set; } = 0;
        [Parameter] public MaterialCategoryItem MaterialCategoryModel { get; set; } 
        [Parameter] public EventCallback<MaterialCategoryItem> MaterialCategoryModelChanged { get; set; } 
        [Parameter] public EventCallback SubmitForm { get; set; }
        [Parameter] public string BreadCrumbText { get; set; } = "Editare/Adaugare departament";
        #endregion Public Properties

        #region Private Properties


        [Inject] private BreadcrumbService BreadcrumbService { get; set; }
        [Inject] private SfDialogService DialogService { get; set; }
        [Inject] private IMaterialCategoriesClient MaterialCategoriesClient { get; set; }
        [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
        private EditContextApiExceptionFallback EditContextApiExceptionFallback { get; set; }



        #endregion Private Properties
        protected override async Task OnInitializedAsync()
        {
            BreadcrumbService.SetBreadcrumbItems(new List<MudBlazor.BreadcrumbItem>
        {
            new("Acasa", "/"),
            new("SSM", "#",true),
            new("Categorii intrebari", "/ssm/MaterialCategories"),
            new(BreadCrumbText, "#", true)
        });
        }

        private async Task OnValidSubmit()
        {
            var dialog = await DialogService.ConfirmAsync(
                MaterialCategoryId !=0 ?"Confirmati actualizarile facute?": "Confirmati inregistrearea noua?",
                "Confirmare");

            if (dialog)
            {
                await SubmitForm.InvokeAsync();
            }
        }


    }
}