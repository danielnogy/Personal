using BinaryPlate.BlazorPlate.Contracts.Consumers.SSM;
using BinaryPlate.BlazorPlate.Features.SSM.Departments.Queries.GetDepartments;
using BinaryPlate.BlazorPlate.Services;
using BinaryPlate.BlazorPlate.Shared.SSM;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Navigations;
using Syncfusion.Blazor.Popups;
using DialogOptions = Syncfusion.Blazor.Popups.DialogOptions;

namespace BinaryPlate.BlazorPlate.Pages.SSM.Departments
{
    public partial class Departments
    {
        [Inject] public IDepartmentsClient DepartmentsClient { get; set; }
        [Inject] private BreadcrumbService BreadcrumbService { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private SfDialogService DialogService { get; set; }
        private GetDepartmentsQuery GetDepartmentsQuery { get; set; } = new GetDepartmentsQuery();
        private GetDepartmentsResponse GetDepartmentsResponse { get; set;} = new GetDepartmentsResponse();
        private List<DepartmentItem> Items { get; set; }
        private GridComponent<DepartmentItem> Grid { get; set; }
        private DepartmentItem SelectedRecord { get; set; }
        private int PageCount { get; set; } = 2;
        private int TotalItems { get; set; } = 1;
        private string SearchText { get; set; } = "";

        protected override async Task OnInitializedAsync()
        {
            BreadcrumbService.SetBreadcrumbItems(new List<MudBlazor.BreadcrumbItem>
            {
                new("Acasa", "/"),
                new("SSM", "#", true),
                new("Departamente", "#", true)
            });
        }
        private async Task<IEnumerable<DepartmentItem>> ExcelDatasourceLoad()
        {
            GetDepartmentsQuery.SearchText = SearchText;
            GetDepartmentsQuery.PageNumber = Grid.Pager.CurrentPage;
            GetDepartmentsQuery.RowsPerPage = -1;
            GetDepartmentsQuery.SortBy = "";
            var responseWrapper = await DepartmentsClient.GetDepartments(GetDepartmentsQuery);
            if (responseWrapper.IsSuccessStatusCode)
            {
                if (responseWrapper.Payload != null)
                {
                    
                    GetDepartmentsResponse = responseWrapper.Payload;
                    return GetDepartmentsResponse.Departments.Items.AsEnumerable();
                }
            }
            return new List<DepartmentItem>();
        }
        private async Task ServerLoad()
        {
            
            GetDepartmentsQuery.SearchText = SearchText;
            GetDepartmentsQuery.PageNumber = Grid.Pager.CurrentPage; 
            GetDepartmentsQuery.RowsPerPage = Grid.Pager.PageSize; 
            GetDepartmentsQuery.SortBy = "";

            var responseWrapper = await DepartmentsClient.GetDepartments(GetDepartmentsQuery);
            if (responseWrapper.IsSuccessStatusCode)
            {
                if (responseWrapper.Payload != null)
                {
                    TotalItems = responseWrapper.Payload.Departments.TotalRows;
                    GetDepartmentsResponse = responseWrapper.Payload;
                    Items = GetDepartmentsResponse.Departments.Items.ToList();
                    await Grid.Refresh();
                    StateHasChanged();

                }
            }
        }
        

        private async void Add()
        {
            NavigationManager.NavigateTo($"ssm/Departments/addDepartment");
        } 
        private async void Delete(DepartmentItem selectedItem)
        {
            if(selectedItem != null)
            {
                await DepartmentsClient.DeleteDepartment(selectedItem.Id);
                await ServerLoad();
            }
        }
        private void Edit(DepartmentItem selectedItem)
        {
            if (selectedItem != null)
                NavigationManager.NavigateTo($"ssm/Departments/editDepartment/{selectedItem.Id}");
        }
        
    }
}
