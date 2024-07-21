using BinaryPlate.BlazorPlate.Contracts.Consumers.SSM;
using BinaryPlate.BlazorPlate.Features.SSM.QuestionCategories.Queries.GetQuestionCategoriess;
using BinaryPlate.BlazorPlate.Features.SSM.Questions.Commands.CreateQuestion;
using BinaryPlate.BlazorPlate.Features.SSM.Questions.Commands.UpdateQuestion;
using BinaryPlate.BlazorPlate.Features.SSM.Questions.Queries.GetQuestions;
using BinaryPlate.BlazorPlate.Shared.SSM;
using Syncfusion.Blazor.Popups;

namespace BinaryPlate.BlazorPlate.Pages.SSM.Questions
{
	public partial class QuestionsForm
    {
        #region Public Properties
        [Inject] private IFileUploadClient FileUploadClient { get; set; }
        [Parameter] public string HeaderText { get; set; }
        [Parameter] public int QuestionId { get; set; } = 0;
        [Parameter] public QuestionItem QuestionModel { get; set; } 
        [Parameter] public EventCallback<QuestionItem> QuestionModelChanged { get; set; } 
        [Parameter] public EventCallback SubmitForm { get; set; }
        [Parameter] public string BreadCrumbText { get; set; } = "Editare/Adaugare intrebare";


        List<AnswerItemForAdd> CreatedQuestionAnswers { get; set; }
        List<AnswerItemForEdit> ModifiedQuestionAnswers { get; set; }
        List<int> RemovedQuestionAnswers { get; set; }
        [Parameter]public EventCallback<List<AnswerItemForAdd>> CreatedQuestionAnswersChanged { get; set; }
        [Parameter]public EventCallback<List<AnswerItemForEdit>> ModifiedQuestionAnswersChanged { get; set; }
        [Parameter]public EventCallback<List<int>> RemovedQuestionAnswersChanged { get; set; }

        #endregion Public Properties

        #region Private Properties


        [Inject] private BreadcrumbService BreadcrumbService { get; set; }
        [Inject] private SfDialogService DialogService { get; set; }
        [Inject] private IQuestionsClient QuestionsClient { get; set; }
        [Inject] private IQuestionCategoriesClient CategoriesClient { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }
        [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
        private EditContextApiExceptionFallback EditContextApiExceptionFallback { get; set; }
        private GetQuestionCategoriesQuery GetQuestionCategories { get; set; } = new();
        private List<QuestionCategoryItem> QuestionCategoryItems { get; set; } = new();

        public async void RefreshAddedQuestionAnswers(List<AnswerItemForAdd> itemForAdds)
        {
            CreatedQuestionAnswers = itemForAdds;
            await CreatedQuestionAnswersChanged.InvokeAsync(CreatedQuestionAnswers);
        } 
        public async void RefreshModifiedQuestionAnswers(List<AnswerItemForEdit> itemForEdits) 
        {
            ModifiedQuestionAnswers = itemForEdits;
            await ModifiedQuestionAnswersChanged.InvokeAsync(ModifiedQuestionAnswers);
        }
        public async void RefreshRemovedQuestionAnswers(List<int> itemsToRemove )
        {
            RemovedQuestionAnswers = itemsToRemove;
            await RemovedQuestionAnswersChanged.InvokeAsync(RemovedQuestionAnswers);
        }

        #endregion Private Properties
        protected override async Task OnInitializedAsync()
        {
            BreadcrumbService.SetBreadcrumbItems(new List<MudBlazor.BreadcrumbItem>
        {
            new("Acasa", "/"),
            new("SSM", "#",true),
            new("Intrebari", "/ssm/Questions"),
            new(BreadCrumbText, "#", true)
        });
            await QuestionCategoriesLoad();
        }
        private async Task QuestionCategoriesLoad()
        {
            GetQuestionCategories.RowsPerPage = -1;
            var responseWrapper = await CategoriesClient.GetQuestionCategories(GetQuestionCategories);
            if (responseWrapper.IsSuccessStatusCode)
            {
                QuestionCategoryItems = responseWrapper.Payload.QuestionCategories.Items.ToList();
            }
        }
        private async Task OnValidSubmit()
        {
            var dialog = await DialogService.ConfirmAsync(
                QuestionId !=0 ?"Confirmati actualizarile facute?": "Confirmati inregistrearea noua?",
                "Confirmare");

            if (dialog)
            {
                await SubmitForm.InvokeAsync();
            }
        }
        

    }
    

}