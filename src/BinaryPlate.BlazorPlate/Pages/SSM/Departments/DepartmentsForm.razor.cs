using BinaryPlate.BlazorPlate.Contracts.Consumers.SSM;
using BinaryPlate.BlazorPlate.Features.SSM.Departments.Queries.GetDepartmentForEdit;
using BinaryPlate.BlazorPlate.Features.SSM.Departments.Queries.GetDepartments;
using Mapster;
using Syncfusion.Blazor.Popups;

namespace BinaryPlate.BlazorPlate.Pages.SSM.Departments
{
    public partial class DepartmentsForm
    {
        #region Public Properties

        [Parameter] public string HeaderText { get; set; }
        [Parameter] public int DepartmentId { get; set; } = 0;
        [Parameter] public DepartmentItem DepartmentModel { get; set; } 
        [Parameter] public EventCallback<DepartmentItem> DepartmentModelChanged { get; set; } 
        [Parameter] public EventCallback SubmitForm { get; set; }
        [Parameter] public string BreadCrumbText { get; set; } = "Editare/Adaugare departament";
        #endregion Public Properties

        #region Private Properties


        [Inject] private BreadcrumbService BreadcrumbService { get; set; }
        [Inject] private SfDialogService DialogService { get; set; }
        [Inject] private IDepartmentsClient DepartmentsClient { get; set; }
        [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
        private EditContextApiExceptionFallback EditContextApiExceptionFallback { get; set; }



        #endregion Private Properties
        protected override async Task OnInitializedAsync()
        {
            BreadcrumbService.SetBreadcrumbItems(new List<MudBlazor.BreadcrumbItem>
        {
            new("Acasa", "/"),
            new("SSM", "#",true),
            new("Departamente", "/ssm/Departments"),
            new(BreadCrumbText, "#", true)
        });
        }

        private async Task OnValidSubmit()
        {
            var dialog = await DialogService.ConfirmAsync(
                DepartmentId !=0 ?"Confirmati actualizarile facute?": "Confirmati inregistrearea noua?",
                "Confirmare");

            if (dialog)
            {
                await SubmitForm.InvokeAsync();
            }
        }


    }
}