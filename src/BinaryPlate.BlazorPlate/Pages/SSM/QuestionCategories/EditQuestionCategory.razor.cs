using BinaryPlate.BlazorPlate.Contracts.Consumers.SSM;
using BinaryPlate.BlazorPlate.Features.SSM.QuestionCategories.Commands.UpdateQuestionCategory;
using BinaryPlate.BlazorPlate.Features.SSM.QuestionCategories.Queries.GetQuestionCategoriess;
using BinaryPlate.BlazorPlate.Features.SSM.QuestionCategories.Queries.GetQuestionCategoryForEdit;
using Mapster;

namespace BinaryPlate.BlazorPlate.Pages.SSM.QuestionCategories
{
    public partial class EditQuestionCategory
    {
        [Parameter] public int QuestionCategoryId { get; set; } = 0;
        public QuestionCategoryItem QuestionCategoryParameter { get; set; } = new();
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IQuestionCategoriesClient QuestionCategoriesClient { get; set; }
        [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }
        private EditContextApiExceptionFallback EditContextApiExceptionFallback { get; set; }
        private GetQuestionCategoryForEditResponse QuestionCategoryForEdit { get; set; } = new();
        private UpdateQuestionCategoryCommand UpdateQuestionCategoryCommand { get; set; } = new();
        protected override async Task OnInitializedAsync()
        {
            var responseWrapper = await QuestionCategoriesClient.GetQuestionCategory(new GetQuestionCategoryForEditQuery
            {
                Id = QuestionCategoryId,
            });

            if (responseWrapper.IsSuccessStatusCode)
                QuestionCategoryParameter = responseWrapper.Payload.Adapt<QuestionCategoryItem>();
            else
                SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }
        public async Task SubmitForm()
        {
            QuestionCategoryParameter.Adapt(UpdateQuestionCategoryCommand);

            var responseWrapper = await QuestionCategoriesClient.UpdateQuestionCategory(UpdateQuestionCategoryCommand);

            if (responseWrapper.IsSuccessStatusCode)
            {
                Snackbar.Add(responseWrapper.Payload, Severity.Success);
                NavigationManager.NavigateTo("/ssm/QuestionCategories");
            }
            else
            {
                EditContextApiExceptionFallback.PopulateFormErrors(responseWrapper.ApiErrorResponse);
                SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
            }
        }
    }
}