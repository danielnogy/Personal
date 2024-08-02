using BinaryPlate.BlazorPlate.Contracts.Consumers.SSM;
using BinaryPlate.BlazorPlate.Features.SSM.QuestionCategories.Queries.GetQuestionCategoriess;
using BinaryPlate.BlazorPlate.Features.SSM.Questions.Queries.GetQuestions;
using BinaryPlate.BlazorPlate.Features.SSM.Tests.Commands.CreateTest.AddModels;
using BinaryPlate.BlazorPlate.Features.SSM.Tests.Commands.UpdateTest.EditModels;
using BinaryPlate.BlazorPlate.Features.SSM.Tests.Queries.GetTestsQuestions;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Navigations;
using Syncfusion.Blazor.Popups;

namespace BinaryPlate.BlazorPlate.Pages.SSM.Tests
{
    public partial class QuestionSelection
    {
        [Parameter] public bool Visible { get; set; }
        [Parameter] public int TestId { get; set; }
        [Parameter] public EventCallback<bool> VisibleChanged { get; set; }

        #region TestQuestions 
        [Parameter] public EventCallback<List<TestQuestionItemForAdd>> OnAddedTestQuestionsListChanged { get; set; }
        [Parameter] public EventCallback<List<TestQuestionItemForEdit>> OnModifiedTestQuestionsListChanged { get; set; }
        [Parameter] public EventCallback<List<int>> OnRemovedTestQuestionsListChanged { get; set; }
        public List<TestQuestionItemForAdd> AddedTestQuestionsList = new();
        public List<TestQuestionItemForEdit> ModifiedTestQuestionsList = new();
        public List<int> RemovedTestQuestionsList = new();

        #endregion

        public List<QuestionItem> SelectedQuestions { get; set; } = new();
        [Inject] private SfDialogService DialogService { get; set; }
        [Inject] public IQuestionsClient QuestionsClient { get; set; }
        [Inject] public ITestsClient TestsClient { get; set; }
        [Inject] public IQuestionCategoriesClient QuestionCategoriesClient { get; set; }
        [Inject] TimerHelper TimerObject { get; set; }
        private List<QuestionItem> QuestionItems { get; set; } = new();
        private SfGrid<QuestionItem> Grid { get; set; }
        private List<QuestionCategoryItem> QuestionCategories { get; set; } = new();
        private List<TestQuestionItem> TestQuestionItems { get; set; } = new();
        private List<int> TestQuestionItemsQuestionIds { get; set; } = new();
        private GetQuestionsQuery GetQuestionsQuery { get; set; } = new GetQuestionsQuery();
        private GetTestQuestionsQuery GetTestQuestionsQuery { get; set; } = new GetTestQuestionsQuery();
        private GetQuestionsResponse GetQuestionsResponse { get; set; } = new GetQuestionsResponse();
        private GetQuestionCategoriesQuery GetQuestionCategoriesQuery { get; set; } = new();
        private GetQuestionCategoriesResponse GetQuestionCategoriesResponse { get; set; } = new();


        public int TotalItems { get; set; } = 1;
        public int PageSize { get; set; } = 4; 
        public SfPager Pager { get; set; }
        private int SelectedQuestionCategoryId { get; set; }
        private string SearchText { get; set; }

        private void OnRowSelected(RowSelectEventArgs<QuestionItem> args)
        {
            try
            {
                if (!SelectedQuestions.Select(x => x.Id).Contains(args.Data.Id))
                {
                    SelectedQuestions.Add(args.Data);
                    if (!TestQuestionItemsQuestionIds.Contains(args.Data.Id) && TestId != 0)
                    {
                        AddedTestQuestionsList.Add(
                            new TestQuestionItemForAdd
                            {
                                QuestionId = args.Data.Id,
                                TestId = TestId
                            });
                        OnAddedTestQuestionsListChanged.InvokeAsync(AddedTestQuestionsList);
                    }

                    

                    
                }
                if (TestQuestionItems.Any(x => x.QuestionId == args.Data.Id))
                {
                    var selectedId = TestQuestionItems.FirstOrDefault(x => x.QuestionId == args.Data.Id).Id;
                    if (RemovedTestQuestionsList.Contains(selectedId))
                    {
                        RemovedTestQuestionsList.Remove(selectedId);
                        OnRemovedTestQuestionsListChanged.InvokeAsync(RemovedTestQuestionsList);

                    }
                }
                StateHasChanged();
            }
            catch (Exception ex)
            {

                throw ex.InnerException;
            }
            
        }
        private void OnRowDeselected(RowDeselectEventArgs<QuestionItem> args)
        {
            var itemToRemove = SelectedQuestions.FirstOrDefault(x => x.Id == args.Data.Id);
            if (itemToRemove != null)
            {
                SelectedQuestions.Remove(itemToRemove);
                if (TestQuestionItemsQuestionIds.Contains(args.Data.Id) && TestId != 0)
                {
                    RemovedTestQuestionsList.Add(TestQuestionItems.FirstOrDefault(x=>x.QuestionId == args.Data.Id).Id);
                    OnRemovedTestQuestionsListChanged.InvokeAsync(RemovedTestQuestionsList);
                }
                if(AddedTestQuestionsList.Select(x=>x.QuestionId).Contains(args.Data.Id))
                {
                    AddedTestQuestionsList.Remove(AddedTestQuestionsList.FirstOrDefault(x=>x.QuestionId==args.Data.Id));
                    OnAddedTestQuestionsListChanged.InvokeAsync(AddedTestQuestionsList);
                }
                StateHasChanged();
            }
        }
        private async Task DataBound()
        {
            if (SelectedQuestions != null)
            {
                var idsAlreadySelected = SelectedQuestions.Select(x => x.Id).ToList();
                if (idsAlreadySelected.Count > 0)
                {
                    foreach (var item in QuestionItems)
                    {
                        if (idsAlreadySelected.Contains(item.Id))
                        {
                            await Grid.SelectRowAsync((await Grid.GetRowIndexByPrimaryKeyAsync(item.Id)));
                        }
                    }
                }
            }

        }
        private async Task ToolbarClickHandler(Syncfusion.Blazor.Navigations.ClickEventArgs args)
        {
            var selectedRecords = await Grid.GetSelectedRecordsAsync();

            if (args.Item.Id == "add")
            {

            }
        }

        protected override async Task OnInitializedAsync()
        {



        }

        protected override async Task OnParametersSetAsync()
        {
            
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await QuestionCategoriesLoad();
                if (TestId != 0)
                {
                    await TestQuestionsLoad();
                }

                SelectedQuestionCategoryId = QuestionCategories.Select(x => x.Id).FirstOrDefault();
                TimerObject.OnElapsed += async () =>
                {
                    await QuestionsLoad();
                };
                TimerObject.SetTimer(600);
            }
        }
        private async Task TestQuestionsLoad()
        {
            GetTestQuestionsQuery.RowsPerPage = -1;
            GetTestQuestionsQuery.TestId = TestId;

            var responseWrapper = await TestsClient.GetTestQuestions(GetTestQuestionsQuery);
            if (responseWrapper.IsSuccessStatusCode)
            {
                var response = responseWrapper.Payload;
                TestQuestionItems = response.TestQuestions.Items.ToList();
                TestQuestionItemsQuestionIds = response.TestQuestions.Items.Select(x => x.QuestionId).ToList();


            }
        }
        private async Task QuestionsLoad()
        {
            if (Grid != null)
                await Grid.ShowSpinnerAsync();
            while (Pager == null)
            {
                await Task.Delay(30);
            }
            PageSize = 4;

            GetQuestionsQuery.QuestionCategoryId = SelectedQuestionCategoryId;
            GetQuestionsQuery.PageNumber = Pager.CurrentPage;
            GetQuestionsQuery.RowsPerPage = Pager.PageSize;
            GetQuestionsQuery.SearchText = SearchText;
            GetQuestionsQuery.SortBy = "";
            //if we want to see selected questions
            if (SelectedQuestionCategoryId == -1)
            {
                QuestionItems = SelectedQuestions
                                    .Where(x =>
                                        x.Text.Contains(!string.IsNullOrWhiteSpace(SearchText) ? SearchText : ""))
                                    .ToList();
                TotalItems = SelectedQuestions.Count;
                PageSize = SelectedQuestions.Count;
                await Grid.Refresh();
                if (Grid != null)
                    await Grid.HideSpinnerAsync();
                StateHasChanged();
                return;
            }
            var responseWrapper = await QuestionsClient.GetQuestions(GetQuestionsQuery);
            if (responseWrapper.IsSuccessStatusCode)
            {
                if (responseWrapper.Payload != null)
                {
                    TotalItems = responseWrapper.Payload.Questions.TotalRows;
                    GetQuestionsResponse = responseWrapper.Payload;
                    if (TestId != 0)
                    {
                        foreach (var item in TestQuestionItems)
                        {
                            if(GetQuestionsResponse.Questions.Items.Select(x=>x.Id).Contains(item.QuestionId))
                            {
                                SelectedQuestions.Add(GetQuestionsResponse.Questions.Items.FirstOrDefault(x => x.Id == item.QuestionId));
                            }
                        }
                    }
                    QuestionItems = GetQuestionsResponse.Questions.Items.ToList();
                    if (Grid != null)
                        await Grid.Refresh();
                    StateHasChanged();

                }
            }
            if (Grid != null)
                await Grid.HideSpinnerAsync();
        }
        private async Task QuestionCategoriesLoad()
        {

            while (Pager == null)
            {
                await Task.Delay(30);
            }
            GetQuestionCategoriesQuery.RowsPerPage = -1;
            GetQuestionCategoriesQuery.SortBy = "";

            var responseWrapper = await QuestionCategoriesClient.GetQuestionCategories(GetQuestionCategoriesQuery);
            if (responseWrapper.IsSuccessStatusCode)
            {
                if (responseWrapper.Payload != null)
                {
                    //TotalItems = responseWrapper.Payload.Questions.TotalRows;
                    GetQuestionCategoriesResponse = responseWrapper.Payload;
                    QuestionCategories = GetQuestionCategoriesResponse.QuestionCategories.Items.ToList();
                    QuestionCategories.Add(new QuestionCategoryItem { Id = 0, Name = " Toate " });
                    QuestionCategories.Add(new QuestionCategoryItem { Id = -1, Name = " Selectate " });
                    if (Grid != null)
                        await Grid.Refresh();
                    StateHasChanged();

                }
            }
        }
        private async void PageChangedHandler(PageChangedEventArgs args)
        {
            await QuestionsLoad();
            StateHasChanged();
        }
        private async void InputHandler(Microsoft.AspNetCore.Components.ChangeEventArgs args)
        {
            if (args.Value != null)
            {
                SearchText = (string)args.Value;
                TimerObject.StopTimer();
                TimerObject.StartTimer();
            }


        }
        public List<QuestionItem> GetSelectedRecords()
        {
            return SelectedQuestions;
        }
        public void SetSelectedRecords(List<QuestionItem> items)
        {
            SelectedQuestions = items;
        }

        public void Dispose()
        {
                TimerObject.Dispose();
        }
    }
}