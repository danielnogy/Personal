using BinaryPlate.BlazorPlate.Consumers.HttpClients;
using BinaryPlate.BlazorPlate.Consumers.HttpClients.SSM;
using BinaryPlate.BlazorPlate.Contracts.Consumers.SSM;
using BinaryPlate.BlazorPlate.Features.SSM.Questions.Commands.CreateQuestion;
using BinaryPlate.BlazorPlate.Features.SSM.Questions.Commands.UpdateQuestion;
using BinaryPlate.BlazorPlate.Features.SSM.Questions.Queries.GetQuestions;
using Mapster;

namespace BinaryPlate.BlazorPlate.Pages.SSM.Questions;
public partial class AddQuestion
{
    [Inject] private IQuestionsClient QuestionsClient { get; set; }
    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    private EditContextApiExceptionFallback EditContextApiExceptionFallback { get; set; }
    private QuestionItem QuestionItem { get; set; } = new();
    private CreateQuestionCommand CreateQuestionCommand { get; set; } = new();
    List<AnswerItemForAdd> CreatedQuestionAnswers { get; set; }
    public async void RefreshAddedQuestionAnswers(List<AnswerItemForAdd> itemForAdds)
    {
        CreatedQuestionAnswers = itemForAdds;
    }


    private async Task SubmitForm()
    {
        QuestionItem.Adapt(CreateQuestionCommand);
        CreateQuestionCommand.AnswerItems = CreatedQuestionAnswers;
        var responseWrapper = await QuestionsClient.CreateQuestion(CreateQuestionCommand);
        if (responseWrapper.IsSuccessStatusCode)
        {
            Snackbar.Add(responseWrapper.Payload.SuccessMessage, Severity.Success);
            NavigationManager.NavigateTo("ssm/questions");
        }
        else
        {
            EditContextApiExceptionFallback.PopulateFormErrors(responseWrapper.ApiErrorResponse);
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }
    }
}
