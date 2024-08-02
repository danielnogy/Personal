using BinaryPlate.BlazorPlate.Contracts.Consumers.SSM;
using BinaryPlate.BlazorPlate.Features.SSM.Tests.Commands.CreateTest.AddModels;
using BinaryPlate.BlazorPlate.Features.SSM.Tests.Commands.UpdateTest;
using BinaryPlate.BlazorPlate.Features.SSM.Tests.Commands.UpdateTest.EditModels;
using BinaryPlate.BlazorPlate.Features.SSM.Tests.Queries.GetQuestionForEdit;
using BinaryPlate.BlazorPlate.Features.SSM.Tests.Queries.GetTests;
using Mapster;

namespace BinaryPlate.BlazorPlate.Pages.SSM.Tests;
public partial class EditTest
{
    [Parameter] public int TestId { get; set; }
    [Parameter] public TestItem TestModel { get; set; } = new();
    [Inject] ITestsClient TestsClient { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }

    private EditContextApiExceptionFallback EditContextApiExceptionFallback { get; set; }
    private GetTestForEditResponse TestForEdit { get; set; } = new();
    private UpdateTestCommand UpdateTestCommand { get; set; } = new();
    [Inject] private ISnackbar Snackbar { get; set; }

    List<TestQuestionItemForAdd> CreatedTestQuestions { get; set; } = new();
    List<TestQuestionItemForEdit> ModifiedTestQuestions { get; set; } = new();
    List<int> RemovedTestQuestions { get; set; }= new();
    protected override async Task OnInitializedAsync()
    {
        var responseWrapper = await TestsClient.GetTest(new GetTestForEditQuery
        {
            Id = TestId
        });

        if (responseWrapper.IsSuccessStatusCode)
            TestModel = responseWrapper.Payload.Adapt<TestItem>();
        else
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
    }
    public void RefreshAddedTestQuestions(List<TestQuestionItemForAdd> itemForAdds)
    {
        CreatedTestQuestions = itemForAdds;
    }
    public void RefreshModifiedTestQuestions(List<TestQuestionItemForEdit> itemForEdits)
    {
        ModifiedTestQuestions = itemForEdits;
    }
    public void RefreshRemovedTestQuestions(List<int> itemsToRemove)
    {
        RemovedTestQuestions = itemsToRemove;
    }
    public async Task SubmitForm()
    {
        TestModel.Adapt(UpdateTestCommand);
        UpdateTestCommand.ModifiedTestQuestions = ModifiedTestQuestions;
        UpdateTestCommand.NewTestQuestions = CreatedTestQuestions;
        UpdateTestCommand.RemovedTestQuestions = RemovedTestQuestions;
        var responseWrapper = await TestsClient.UpdateTest(UpdateTestCommand);

        if (responseWrapper.IsSuccessStatusCode)
        {
            Snackbar.Add(responseWrapper.Payload, Severity.Success);
            NavigationManager.NavigateTo("/ssm/Tests");
        }
        else
        {
            EditContextApiExceptionFallback.PopulateFormErrors(responseWrapper.ApiErrorResponse);
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }
    }
}
