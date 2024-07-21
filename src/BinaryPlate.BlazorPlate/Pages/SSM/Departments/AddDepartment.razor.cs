using BinaryPlate.BlazorPlate.Consumers.HttpClients;
using BinaryPlate.BlazorPlate.Consumers.HttpClients.SSM;
using BinaryPlate.BlazorPlate.Contracts.Consumers.SSM;
using BinaryPlate.BlazorPlate.Features.SSM.Departments.Commands.CreateDepartment;
using BinaryPlate.BlazorPlate.Features.SSM.Departments.Queries.GetDepartments;
using Mapster;

namespace BinaryPlate.BlazorPlate.Pages.SSM.Departments;
public partial class AddDepartment
{
    [Inject] private IDepartmentsClient DepartmentsClient { get; set; }
    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    private EditContextApiExceptionFallback EditContextApiExceptionFallback { get; set; }
    private DepartmentItem DepartmentItem { get; set; } = new();
    private CreateDepartmentCommand CreateDepartmentCommand { get; set; } = new();

    private async Task SubmitForm()
    {
        DepartmentItem.Adapt(CreateDepartmentCommand);
        var responseWrapper = await DepartmentsClient.CreateDepartment(CreateDepartmentCommand);
        if (responseWrapper.IsSuccessStatusCode)
        {
            Snackbar.Add(responseWrapper.Payload.SuccessMessage, Severity.Success);
            NavigationManager.NavigateTo("ssm/departments");
        }
        else
        {
            EditContextApiExceptionFallback.PopulateFormErrors(responseWrapper.ApiErrorResponse);
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }
    }
}
