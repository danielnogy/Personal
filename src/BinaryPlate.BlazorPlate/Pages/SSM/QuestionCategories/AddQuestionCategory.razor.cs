using BinaryPlate.BlazorPlate.Contracts.Consumers.SSM;
using BinaryPlate.BlazorPlate.Features.SSM.QuestionCategories.Commands.CreateQuestionCategory;
using BinaryPlate.BlazorPlate.Features.SSM.QuestionCategories.Queries.GetQuestionCategoriess;
using Mapster;

namespace BinaryPlate.BlazorPlate.Pages.SSM.QuestionCategories;
public partial class AddQuestionCategory
{
    [Inject] private IQuestionCategoriesClient QuestionCategoriesClient { get; set; }
    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    private EditContextApiExceptionFallback EditContextApiExceptionFallback { get; set; }
    private QuestionCategoryItem QuestionCategoryItem { get; set; } = new();
    private CreateQuestionCategoryCommand CreateQuestionCategoryCommand { get; set; } = new();

    private async Task SubmitForm()
    {
        QuestionCategoryItem.Adapt(CreateQuestionCategoryCommand);
        var responseWrapper = await QuestionCategoriesClient.CreateQuestionCategory(CreateQuestionCategoryCommand);
        if (responseWrapper.IsSuccessStatusCode)
        {
            Snackbar.Add(responseWrapper.Payload.SuccessMessage, Severity.Success);
            NavigationManager.NavigateTo("ssm/questionCategories");
        }
        else
        {
            EditContextApiExceptionFallback.PopulateFormErrors(responseWrapper.ApiErrorResponse);
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }
    }
}
