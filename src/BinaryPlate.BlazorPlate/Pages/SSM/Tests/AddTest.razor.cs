using BinaryPlate.BlazorPlate.Consumers.HttpClients;
using BinaryPlate.BlazorPlate.Consumers.HttpClients.SSM;
using BinaryPlate.BlazorPlate.Contracts.Consumers.SSM;
using BinaryPlate.BlazorPlate.Features.SSM.Tests.Commands.CreateTest;
using BinaryPlate.BlazorPlate.Features.SSM.Tests.Commands.CreateTest.AddModels;
using BinaryPlate.BlazorPlate.Features.SSM.Tests.Commands.UpdateTest;
using BinaryPlate.BlazorPlate.Features.SSM.Tests.Queries.GetTests;
using Mapster;

namespace BinaryPlate.BlazorPlate.Pages.SSM.Tests;
public partial class AddTest
{
    [Inject] private ITestsClient TestsClient { get; set; }
    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    private EditContextApiExceptionFallback EditContextApiExceptionFallback { get; set; }
    private TestItem TestItem { get; set; } = new();
    private CreateTestCommand CreateTestCommand { get; set; } = new();



    private async Task SubmitForm()
    {
        TestItem.Adapt(CreateTestCommand);
        CreateTestCommand.TestQuestionItems = TestItem.TestQuestions.Adapt<List<TestQuestionItemForAdd>>();
        CreateTestCommand.TestMaterialItems = TestItem.TestMaterials.Adapt<List<TestMaterialItemForAdd>>();
        var responseWrapper = await TestsClient.CreateTest(CreateTestCommand);
        if (responseWrapper.IsSuccessStatusCode)
        {
            Snackbar.Add(responseWrapper.Payload.SuccessMessage, Severity.Success);
            NavigationManager.NavigateTo("ssm/tests");
        }
        else
        {
            EditContextApiExceptionFallback.PopulateFormErrors(responseWrapper.ApiErrorResponse);
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }
    }
}
