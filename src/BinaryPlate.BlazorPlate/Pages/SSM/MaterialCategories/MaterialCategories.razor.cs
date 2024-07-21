using BinaryPlate.BlazorPlate.Contracts.Consumers.SSM;
using BinaryPlate.BlazorPlate.Features.SSM.MaterialCategories.Queries.GetMaterialCategoriess;
using BinaryPlate.BlazorPlate.Shared.SSM;
using Syncfusion.Blazor.Popups;

namespace BinaryPlate.BlazorPlate.Pages.SSM.MaterialCategories
{
    public partial class MaterialCategories
    {
        [Inject] public IMaterialCategoriesClient MaterialCategoriesClient { get; set; }
        [Inject] private BreadcrumbService BreadcrumbService { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private SfDialogService DialogService { get; set; }
        private GetMaterialCategoriesQuery GetMaterialCategoriesQuery { get; set; } = new GetMaterialCategoriesQuery();
        private GetMaterialCategoriesResponse GetMaterialCategoriesResponse { get; set;} = new GetMaterialCategoriesResponse();
        private List<MaterialCategoryItem> Items { get; set; }
        private GridComponent<MaterialCategoryItem> Grid { get; set; }
        private MaterialCategoryItem SelectedRecord { get; set; }
        private int PageCount { get; set; } = 2;
        private int TotalItems { get; set; } = 1;
        private string SearchText { get; set; } = "";

        protected override async Task OnInitializedAsync()
        {
            BreadcrumbService.SetBreadcrumbItems(new List<MudBlazor.BreadcrumbItem>
            {
                new("Acasa", "/"),
                new("SSM", "#", true),
                new("Categorii intrebari", "#", true)
            });
        }
        private async Task<IEnumerable<MaterialCategoryItem>> ExcelDatasourceLoad()
        {
            GetMaterialCategoriesQuery.SearchText = SearchText;
            GetMaterialCategoriesQuery.PageNumber = Grid.Pager.CurrentPage;
            GetMaterialCategoriesQuery.RowsPerPage = -1;
            GetMaterialCategoriesQuery.SortBy = "";
            var responseWrapper = await MaterialCategoriesClient.GetMaterialCategories(GetMaterialCategoriesQuery);
            if (responseWrapper.IsSuccessStatusCode)
            {
                if (responseWrapper.Payload != null)
                {
                    
                    GetMaterialCategoriesResponse = responseWrapper.Payload;
                    return GetMaterialCategoriesResponse.MaterialCategories.Items.AsEnumerable();
                }
            }
            return new List<MaterialCategoryItem>();
        }
        private async Task ServerLoad()
        {
            
            GetMaterialCategoriesQuery.SearchText = SearchText;
            GetMaterialCategoriesQuery.PageNumber = Grid.Pager.CurrentPage; 
            GetMaterialCategoriesQuery.RowsPerPage = Grid.Pager.PageSize; 
            GetMaterialCategoriesQuery.SortBy = "";

            var responseWrapper = await MaterialCategoriesClient.GetMaterialCategories(GetMaterialCategoriesQuery);
            if (responseWrapper.IsSuccessStatusCode)
            {
                if (responseWrapper.Payload != null)
                {
                    TotalItems = responseWrapper.Payload.MaterialCategories.TotalRows;
                    GetMaterialCategoriesResponse = responseWrapper.Payload;
                    Items = GetMaterialCategoriesResponse.MaterialCategories.Items.ToList();
                    await Grid.Refresh();
                    StateHasChanged();

                }
            }
        }
       
        
        private async void Add()
        {
            NavigationManager.NavigateTo($"ssm/MaterialCategories/addMaterialCategory");
        } 
        private async void Delete(MaterialCategoryItem selectedItem)
        {
            if(selectedItem != null)
            {
                await MaterialCategoriesClient.DeleteMaterialCategory(selectedItem.Id);
                ServerLoad();
            }
        }
        private void Edit(MaterialCategoryItem selectedItem)
        {
            if (selectedItem != null)
                NavigationManager.NavigateTo($"ssm/MaterialCategories/editMaterialCategory/{selectedItem.Id}");
        }
        
    }
}
