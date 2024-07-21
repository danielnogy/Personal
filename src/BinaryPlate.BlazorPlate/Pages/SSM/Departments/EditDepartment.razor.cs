using BinaryPlate.BlazorPlate.Contracts.Consumers.SSM;
using BinaryPlate.BlazorPlate.Features.SSM.Departments.Commands.UpdateDepartment;
using BinaryPlate.BlazorPlate.Features.SSM.Departments.Queries.GetDepartmentForEdit;
using BinaryPlate.BlazorPlate.Features.SSM.Departments.Queries.GetDepartments;
using Mapster;

namespace BinaryPlate.BlazorPlate.Pages.SSM.Departments
{
    public partial class EditDepartment
    {
        [Parameter] public int DepartmentId { get; set; } = 0;
        public DepartmentItem DepartmentParameter { get; set; } = new();
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IDepartmentsClient DepartmentsClient { get; set; }
        [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }
        private EditContextApiExceptionFallback EditContextApiExceptionFallback { get; set; }
        private GetDepartmentForEditResponse DepartmentForEdit { get; set; } = new();
        private UpdateDepartmentCommand UpdateDepartmentCommand { get; set; } = new();
        protected override async Task OnInitializedAsync()
        {
            var responseWrapper = await DepartmentsClient.GetDepartment(new GetDepartmentForEditQuery
            {
                Id = DepartmentId,
            });

            if (responseWrapper.IsSuccessStatusCode)
                DepartmentParameter = responseWrapper.Payload.Adapt<DepartmentItem>();
            else
                SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }
        public async Task SubmitForm()
        {
            DepartmentParameter.Adapt(UpdateDepartmentCommand);

            var responseWrapper = await DepartmentsClient.UpdateDepartment(UpdateDepartmentCommand);

            if (responseWrapper.IsSuccessStatusCode)
            {
                Snackbar.Add(responseWrapper.Payload, Severity.Success);
                NavigationManager.NavigateTo("/ssm/Departments");
            }
            else
            {
                EditContextApiExceptionFallback.PopulateFormErrors(responseWrapper.ApiErrorResponse);
                SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
            }
        }
    }
}