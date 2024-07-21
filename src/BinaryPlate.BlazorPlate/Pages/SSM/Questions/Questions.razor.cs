using BinaryPlate.BlazorPlate.Contracts.Consumers.SSM;
using BinaryPlate.BlazorPlate.Features.SSM.Questions.Queries.GetQuestions;
using BinaryPlate.BlazorPlate.Services;
using BinaryPlate.BlazorPlate.Shared.SSM;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Navigations;
using Syncfusion.Blazor.Popups;
using DialogOptions = Syncfusion.Blazor.Popups.DialogOptions;

namespace BinaryPlate.BlazorPlate.Pages.SSM.Questions
{
    public partial class Questions
    {
        [Inject] public IQuestionsClient QuestionsClient { get; set; }
        [Inject] private BreadcrumbService BreadcrumbService { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private SfDialogService DialogService { get; set; }
        private GetQuestionsQuery GetQuestionsQuery { get; set; } = new GetQuestionsQuery();
        private GetQuestionsResponse GetQuestionsResponse { get; set;} = new GetQuestionsResponse();
        private List<QuestionItem> Items { get; set; }
        private GridComponent<QuestionItem> Grid { get; set; }
        private QuestionItem SelectedRecord { get; set; }
        private int PageCount { get; set; } = 2;
        private int TotalItems { get; set; } = 1;
        private string SearchText { get; set; } = "";

        protected override async Task OnInitializedAsync()
        {
            BreadcrumbService.SetBreadcrumbItems(new List<MudBlazor.BreadcrumbItem>
            {
                new("Acasa", "/"),
                new("SSM", "#", true),
                new("Intrebari", "#", true)
            });
        }
        private async Task<IEnumerable<QuestionItem>> ExcelDatasourceLoad()
        {
            GetQuestionsQuery.SearchText = SearchText;
            GetQuestionsQuery.PageNumber = Grid.Pager.CurrentPage;
            GetQuestionsQuery.RowsPerPage = -1;
            GetQuestionsQuery.SortBy = "";
            var responseWrapper = await QuestionsClient.GetQuestions(GetQuestionsQuery);
            if (responseWrapper.IsSuccessStatusCode)
            {
                if (responseWrapper.Payload != null)
                {
                    
                    GetQuestionsResponse = responseWrapper.Payload;
                    return GetQuestionsResponse.Questions.Items.AsEnumerable();
                }
            }
            return new List<QuestionItem>();
        }
        private async Task ServerLoad()
        {
            
            GetQuestionsQuery.SearchText = SearchText;
            GetQuestionsQuery.PageNumber = Grid.Pager.CurrentPage; 
            GetQuestionsQuery.RowsPerPage = Grid.Pager.PageSize; 
            GetQuestionsQuery.SortBy = "";

            var responseWrapper = await QuestionsClient.GetQuestions(GetQuestionsQuery);
            if (responseWrapper.IsSuccessStatusCode)
            {
                if (responseWrapper.Payload != null)
                {
                    TotalItems = responseWrapper.Payload.Questions.TotalRows;
                    GetQuestionsResponse = responseWrapper.Payload;
                    Items = GetQuestionsResponse.Questions.Items.ToList();
                    await Grid.Refresh();
                    StateHasChanged();

                }
            }
        }
       
        
        private async void Add()
        {
            NavigationManager.NavigateTo($"ssm/Questions/addQuestion");
        } 
        private async void Delete(QuestionItem selectedItem)
        {
            if(selectedItem != null)
            {
                await QuestionsClient.DeleteQuestion(selectedItem.Id);
                await ServerLoad();
            }
        }
        private void Edit(QuestionItem selectedItem)
        {
            if (selectedItem != null)
                NavigationManager.NavigateTo($"ssm/Questions/editQuestion/{selectedItem.Id}");
        }
        
    }
}
