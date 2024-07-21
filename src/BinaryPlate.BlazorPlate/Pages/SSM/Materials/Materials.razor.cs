using BinaryPlate.BlazorPlate.Contracts.Consumers.SSM;
using BinaryPlate.BlazorPlate.Features.SSM.Materials.Queries.GetMaterials;
using BinaryPlate.BlazorPlate.Services;
using BinaryPlate.BlazorPlate.Shared.SSM;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Navigations;
using Syncfusion.Blazor.Popups;
using DialogOptions = Syncfusion.Blazor.Popups.DialogOptions;

namespace BinaryPlate.BlazorPlate.Pages.SSM.Materials
{
    public partial class Materials
    {
        [Inject] public IMaterialsClient MaterialsClient { get; set; }
        [Inject] private BreadcrumbService BreadcrumbService { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private SfDialogService DialogService { get; set; }
        private GetMaterialsQuery GetMaterialsQuery { get; set; } = new GetMaterialsQuery();
        private GetMaterialsResponse GetMaterialsResponse { get; set;} = new GetMaterialsResponse();
        private List<MaterialItem> Items { get; set; }
        private GridComponent<MaterialItem> Grid { get; set; }
        private MaterialItem SelectedRecord { get; set; }
        private int PageCount { get; set; } = 2;
        private int TotalItems { get; set; } = 1;
        private string SearchText { get; set; } = "";

        protected override async Task OnInitializedAsync()
        {
            BreadcrumbService.SetBreadcrumbItems(new List<MudBlazor.BreadcrumbItem>
            {
                new("Acasa", "/"),
                new("SSM", "#", true),
                new("Materiale", "#", true)
            });
        }
        private async Task<IEnumerable<MaterialItem>> ExcelDatasourceLoad()
        {
            GetMaterialsQuery.SearchText = SearchText;
            GetMaterialsQuery.PageNumber = Grid.Pager.CurrentPage;
            GetMaterialsQuery.RowsPerPage = -1;
            GetMaterialsQuery.SortBy = "";
            var responseWrapper = await MaterialsClient.GetMaterials(GetMaterialsQuery);
            if (responseWrapper.IsSuccessStatusCode)
            {
                if (responseWrapper.Payload != null)
                {
                    
                    GetMaterialsResponse = responseWrapper.Payload;
                    return GetMaterialsResponse.Materials.Items.AsEnumerable();
                }
            }
            return new List<MaterialItem>();
        }
        private async Task ServerLoad()
        {
            
            GetMaterialsQuery.SearchText = SearchText;
            GetMaterialsQuery.PageNumber = Grid.Pager.CurrentPage; 
            GetMaterialsQuery.RowsPerPage = Grid.Pager.PageSize; 
            GetMaterialsQuery.SortBy = "";

            var responseWrapper = await MaterialsClient.GetMaterials(GetMaterialsQuery);
            if (responseWrapper.IsSuccessStatusCode)
            {
                if (responseWrapper.Payload != null)
                {
                    TotalItems = responseWrapper.Payload.Materials.TotalRows;
                    GetMaterialsResponse = responseWrapper.Payload;
                    Items = GetMaterialsResponse.Materials.Items.ToList();
                    await Grid.Refresh();
                    StateHasChanged();

                }
            }
        }
       
        
        private async void Add()
        {
            NavigationManager.NavigateTo($"ssm/Materials/addMaterial");
        } 
        private async void Delete(MaterialItem selectedItem)
        {
            if(selectedItem != null)
            {
                await MaterialsClient.DeleteMaterial(selectedItem.Id);
                await ServerLoad();
            }
        }
        private void Edit(MaterialItem selectedItem)
        {
            if (selectedItem != null)
                NavigationManager.NavigateTo($"ssm/Materials/editMaterial/{selectedItem.Id}");
        }
        
    }
}
