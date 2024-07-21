using BinaryPlate.BlazorPlate.Contracts.Consumers.SSM;
using BinaryPlate.BlazorPlate.Features.SSM.Questions.Commands.CreateQuestion;
using BinaryPlate.BlazorPlate.Features.SSM.Questions.Commands.UpdateQuestion;
using BinaryPlate.BlazorPlate.Features.SSM.Questions.Queries.GetQuestionForEdit;
using BinaryPlate.BlazorPlate.Features.SSM.Questions.Queries.GetQuestions;
using Mapster;

namespace BinaryPlate.BlazorPlate.Pages.SSM.Questions
{
    public partial class EditQuestion
    {
        [Parameter] public int QuestionId { get; set; } = 0;
        public QuestionItem QuestionParameter { get; set; } = new();
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IQuestionsClient QuestionsClient { get; set; }
        [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }
        private EditContextApiExceptionFallback EditContextApiExceptionFallback { get; set; }
        private GetQuestionForEditResponse QuestionForEdit { get; set; } = new();
        private UpdateQuestionCommand UpdateQuestionCommand { get; set; } = new();

        List<AnswerItemForAdd> CreatedQuestionAnswers { get; set; }
        List<AnswerItemForEdit> ModifiedQuestionAnswers { get; set; }
        List<int> RemovedQuestionAnswers { get; set; }
        public  void RefreshAddedQuestionAnswers(List<AnswerItemForAdd> itemForAdds)
        {
            CreatedQuestionAnswers = itemForAdds;
        }
        public  void RefreshModifiedQuestionAnswers(List<AnswerItemForEdit> itemForEdits)
        {
            ModifiedQuestionAnswers = itemForEdits;
        }
        public  void RefreshRemovedQuestionAnswers(List<int> itemsToRemove)
        {
            RemovedQuestionAnswers = itemsToRemove;
        }


        protected override async Task OnInitializedAsync()
        {
            var responseWrapper = await QuestionsClient.GetQuestion(new GetQuestionForEditQuery
            {
                Id = QuestionId,
            });

            if (responseWrapper.IsSuccessStatusCode)
                QuestionParameter = responseWrapper.Payload.Adapt<QuestionItem>();
            else
                SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }

        public async Task SubmitForm()
        {
            QuestionParameter.Adapt(UpdateQuestionCommand);
            UpdateQuestionCommand.ModifiedAnswers = ModifiedQuestionAnswers;
            UpdateQuestionCommand.NewAnswers = CreatedQuestionAnswers;
            UpdateQuestionCommand.RemovedAnswers = RemovedQuestionAnswers;
            var responseWrapper = await QuestionsClient.UpdateQuestion(UpdateQuestionCommand);

            if (responseWrapper.IsSuccessStatusCode)
            {
                Snackbar.Add(responseWrapper.Payload, Severity.Success);
                NavigationManager.NavigateTo("/ssm/Questions");
            }
            else
            {
                EditContextApiExceptionFallback.PopulateFormErrors(responseWrapper.ApiErrorResponse);
                SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
            }
        }
    }
}