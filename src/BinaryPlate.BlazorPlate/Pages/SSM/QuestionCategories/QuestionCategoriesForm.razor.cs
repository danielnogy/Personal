using BinaryPlate.BlazorPlate.Contracts.Consumers.SSM;
using Syncfusion.Blazor.Popups;
using BinaryPlate.BlazorPlate.Features.SSM.QuestionCategories.Queries.GetQuestionCategoriess;

namespace BinaryPlate.BlazorPlate.Pages.SSM.QuestionCategories
{
    public partial class QuestionCategoriesForm
    {
        #region Public Properties

        [Parameter] public string HeaderText { get; set; }
        [Parameter] public int QuestionCategoryId { get; set; } = 0;
        [Parameter] public QuestionCategoryItem QuestionCategoryModel { get; set; } 
        [Parameter] public EventCallback<QuestionCategoryItem> QuestionCategoryModelChanged { get; set; } 
        [Parameter] public EventCallback SubmitForm { get; set; }
        [Parameter] public string BreadCrumbText { get; set; } = "Editare/Adaugare departament";
        #endregion Public Properties

        #region Private Properties


        [Inject] private BreadcrumbService BreadcrumbService { get; set; }
        [Inject] private SfDialogService DialogService { get; set; }
        [Inject] private IQuestionCategoriesClient QuestionCategoriesClient { get; set; }
        [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
        private EditContextApiExceptionFallback EditContextApiExceptionFallback { get; set; }



        #endregion Private Properties
        protected override async Task OnInitializedAsync()
        {
            BreadcrumbService.SetBreadcrumbItems(new List<MudBlazor.BreadcrumbItem>
        {
            new("Acasa", "/"),
            new("SSM", "#",true),
            new("Categorii intrebari", "/ssm/QuestionCategories"),
            new(BreadCrumbText, "#", true)
        });
        }

        private async Task OnValidSubmit()
        {
            var dialog = await DialogService.ConfirmAsync(
                QuestionCategoryId !=0 ?"Confirmati actualizarile facute?": "Confirmati inregistrearea noua?",
                "Confirmare");

            if (dialog)
            {
                await SubmitForm.InvokeAsync();
            }
        }


    }
}