using BinaryPlate.BlazorPlate.Contracts.Consumers.SSM;
using BinaryPlate.BlazorPlate.Features.SSM.QuestionCategories.Queries.GetQuestionCategoriess;
using BinaryPlate.BlazorPlate.Shared.SSM;
using Syncfusion.Blazor.Popups;

namespace BinaryPlate.BlazorPlate.Pages.SSM.QuestionCategories
{
    public partial class QuestionCategories
    {
        [Inject] public IQuestionCategoriesClient QuestionCategoriesClient { get; set; }
        [Inject] private BreadcrumbService BreadcrumbService { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private SfDialogService DialogService { get; set; }
        private GetQuestionCategoriesQuery GetQuestionCategoriesQuery { get; set; } = new GetQuestionCategoriesQuery();
        private GetQuestionCategoriesResponse GetQuestionCategoriesResponse { get; set;} = new GetQuestionCategoriesResponse();
        private List<QuestionCategoryItem> Items { get; set; }
        private GridComponent<QuestionCategoryItem> Grid { get; set; }
        private QuestionCategoryItem SelectedRecord { get; set; }
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
        private async Task<IEnumerable<QuestionCategoryItem>> ExcelDatasourceLoad()
        {
            GetQuestionCategoriesQuery.SearchText = SearchText;
            GetQuestionCategoriesQuery.PageNumber = Grid.Pager.CurrentPage;
            GetQuestionCategoriesQuery.RowsPerPage = -1;
            GetQuestionCategoriesQuery.SortBy = "";
            var responseWrapper = await QuestionCategoriesClient.GetQuestionCategories(GetQuestionCategoriesQuery);
            if (responseWrapper.IsSuccessStatusCode)
            {
                if (responseWrapper.Payload != null)
                {
                    
                    GetQuestionCategoriesResponse = responseWrapper.Payload;
                    return GetQuestionCategoriesResponse.QuestionCategories.Items.AsEnumerable();
                }
            }
            return new List<QuestionCategoryItem>();
        }
        private async Task ServerLoad()
        {
            
            GetQuestionCategoriesQuery.SearchText = SearchText;
            GetQuestionCategoriesQuery.PageNumber = Grid.Pager.CurrentPage; 
            GetQuestionCategoriesQuery.RowsPerPage = Grid.Pager.PageSize; 
            GetQuestionCategoriesQuery.SortBy = "";

            var responseWrapper = await QuestionCategoriesClient.GetQuestionCategories(GetQuestionCategoriesQuery);
            if (responseWrapper.IsSuccessStatusCode)
            {
                if (responseWrapper.Payload != null)
                {
                    TotalItems = responseWrapper.Payload.QuestionCategories.TotalRows;
                    GetQuestionCategoriesResponse = responseWrapper.Payload;
                    Items = GetQuestionCategoriesResponse.QuestionCategories.Items.ToList();
                    await Grid.Refresh();
                    StateHasChanged();

                }
            }
        }
       
        
        private async void Add()
        {
            NavigationManager.NavigateTo($"ssm/QuestionCategories/addQuestionCategory");
        } 
        private async void Delete(QuestionCategoryItem selectedItem)
        {
            if(selectedItem != null)
            {
                await QuestionCategoriesClient.DeleteQuestionCategory(selectedItem.Id);
                ServerLoad();
            }
        }
        private void Edit(QuestionCategoryItem selectedItem)
        {
            if (selectedItem != null)
                NavigationManager.NavigateTo($"ssm/QuestionCategories/editQuestionCategory/{selectedItem.Id}");
        }
        
    }
}
