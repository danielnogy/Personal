using BinaryPlate.BlazorPlate.Contracts.Consumers.SSM;
using BinaryPlate.BlazorPlate.Features.SSM.Questions.Commands.CreateQuestion;
using BinaryPlate.BlazorPlate.Features.SSM.Questions.Commands.UpdateQuestion;
using BinaryPlate.BlazorPlate.Features.SSM.Questions.Queries.GetQuestionsAnswers;
using Mapster;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Navigations;
using Syncfusion.Blazor.Popups;

namespace BinaryPlate.BlazorPlate.Pages.SSM.Questions
{
    public partial class Answers
    {
        [Inject] IQuestionsClient QuestionsClient { get; set; }
        [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
        [Inject] SfDialogService DialogService { get; set; }

        public List<AnswerItemForAdd> AddedQuestionAnswersList = new();
        public List<AnswerItemForEdit> ModifiedQuestionAnswersList = new();
        public List<int> RemovedQuestionAnswersList = new();

        [Parameter] public int QuestionId { get; set; }
        private SfGrid<QuestionAnswerItem> Grid { get; set; }
        private List<QuestionAnswerItem> QuestionAnswers { get; set; } = new();
        [Parameter] public EventCallback<List<AnswerItemForAdd>> OnAddedQuestionAnswersListChanged { get; set; }
        [Parameter] public EventCallback<List<AnswerItemForEdit>> OnModifiedQuestionAnswersListChanged { get; set; }
        [Parameter] public EventCallback<List<int>> OnRemovedQuestionAnswersListChanged { get; set; }
        public SfPager Pager { get; set; }
        private int PagerPageCount { get; set; } = 2;
        private int TotalItems { get; set; } = 1;
        private GetQuestionAnswersQuery GetQuestionAnswersQuery { get; set; } = new();
        private GetQuestionAnswersResponse GetQuestionAnswersResponse { get; set; }
        protected override async Task OnInitializedAsync()
        {
            await ServerLoad();
        }
        private async Task ServerLoad()
        {
            if (QuestionId != 0)
            {
                GetQuestionAnswersQuery.QuestionId = QuestionId;

                GetQuestionAnswersQuery.RowsPerPage = -1;

                var responseWrapper = await QuestionsClient.GetQuestionAnswers(GetQuestionAnswersQuery);

                var tableData = new TableData<QuestionAnswerItem>();

                if (responseWrapper.IsSuccessStatusCode)
                {
                    GetQuestionAnswersResponse = responseWrapper.Payload;
                    QuestionAnswers = GetQuestionAnswersResponse.Answers.Items.ToList();
                    TotalItems = GetQuestionAnswersResponse.Answers.TotalRows;
                    await Grid.Refresh();
                }
                else
                {
                    SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
                }
            }

        }
        private async void PageChangedHandler(PageChangedEventArgs args)
        {
            await ServerLoad();
            StateHasChanged();
        }
        private async Task ToolbarClickHandler(Syncfusion.Blazor.Navigations.ClickEventArgs args)
        {
            var selectedRecords = await Grid.GetSelectedRecordsAsync();
            if (selectedRecords.Count == 0
                && (
                new List<string>() { "edit", "delete" }).Contains(args.Item.Id)
                )
            {
                await DialogService.AlertAsync("Selectati o inregistrare pentru a efectua operatiunea!", "Atentie!");

            }

            if (args.Item.Id == "add")
            {
                await Grid.AddRecordAsync();
            }
            if (args.Item.Id == "edit")
            {
                await Grid.StartEditAsync();
            }
            if (args.Item.Id == "delete")
            {
                await Grid.DeleteRecordAsync();
                //await ServerLoad.InvokeAsync();
            }
        }
        private async Task OnActionBegin(ActionEventArgs<QuestionAnswerItem> args)
        {

        }
        private async Task OnActionComplete(ActionEventArgs<QuestionAnswerItem> args)
        {
            if (QuestionId != 0)
            {
                if (args.Action == "Add")
                {
                    if (args.RequestType == Syncfusion.Blazor.Grids.Action.Save)
                    {
                        args.Data.QuestionId = QuestionId;

                        //generate temporary Id
                        args.Data.Id = OthersHelper.GetRandomIntExcluding(QuestionAnswers.Select(x=>x.Id).ToList(),1,int.MaxValue);
                        AddedQuestionAnswersList.Add(args.Data.Adapt<AnswerItemForAdd>());
                        await OnAddedQuestionAnswersListChanged.InvokeAsync(AddedQuestionAnswersList);
                        await Grid.Refresh();
                    }
                }
                if (args.Action == "Edit")
                {
                    if (args.RequestType == Syncfusion.Blazor.Grids.Action.Save)
                    {
                        IsNewlyAddedAnswerModified(args.Data);
                        IsCurrentAnswerModified(args.Data);
                        await OnModifiedQuestionAnswersListChanged.InvokeAsync(ModifiedQuestionAnswersList);
                        await Grid.Refresh();
                    }
                }
                if (args.RequestType == Syncfusion.Blazor.Grids.Action.Delete)
                {
                    RemoveAddedQuestionAnswer(args.Data.Id);
                    await RemoveCurrentQuestionAnswer(args.Data.Id);
                    await Grid.Refresh();
                }
            }
            else
            {
                if ((args.RequestType == Syncfusion.Blazor.Grids.Action.Save && (args.Action == "Add" || args.Action == "Edit")) || args.RequestType == Syncfusion.Blazor.Grids.Action.Delete)
                {
                    var datasource = QuestionAnswers.Select(x => x.MapToAdd(x)).ToList();
                    await OnAddedQuestionAnswersListChanged.InvokeAsync(datasource);
                    await Grid.Refresh();
                }
            }
        }
        private void IsCurrentAnswerModified(QuestionAnswerItem questionAnswerModified)
        {
            var modifiedReferenceIndex =
                ModifiedQuestionAnswersList.FindIndex(item => item.Id == questionAnswerModified.Id);

            if (modifiedReferenceIndex >= 0)
                ModifiedQuestionAnswersList[modifiedReferenceIndex] = questionAnswerModified.Adapt<AnswerItemForEdit>();
            else
                ModifiedQuestionAnswersList.Add(questionAnswerModified.Adapt<AnswerItemForEdit>());

            var referencesResponseIndex = QuestionAnswers.FindIndex(item => item.Id == questionAnswerModified.Id);

            if (referencesResponseIndex >= 0)
                QuestionAnswers[referencesResponseIndex] = questionAnswerModified;
        }

        private void IsNewlyAddedAnswerModified(QuestionAnswerItem AnswerItemToBeModified)
        {
            var modifiedNewlyAddedAnswerIndex =
                AddedQuestionAnswersList.FindIndex(item => item.Id == AnswerItemToBeModified.Id);
            if (modifiedNewlyAddedAnswerIndex >= 0)
                AddedQuestionAnswersList[modifiedNewlyAddedAnswerIndex] = AnswerItemToBeModified.Adapt<AnswerItemForAdd>();
        }
        private void RemoveAddedQuestionAnswer(int id)
        {
            var addedQuestionAnswerObj = AddedQuestionAnswersList.FirstOrDefault(item => item.Id == id);

            if (addedQuestionAnswerObj is not null)
                AddedQuestionAnswersList.Remove(addedQuestionAnswerObj);

            var answersResponseObj = QuestionAnswers.FirstOrDefault(item => item.Id == id);

            if (answersResponseObj is not null)
                QuestionAnswers.Remove(answersResponseObj);
        }

        private async Task RemoveCurrentQuestionAnswer(int id)
        {
            var modifiedQuestionAnswerObj = ModifiedQuestionAnswersList.FirstOrDefault(item => item.Id == id);

            if (modifiedQuestionAnswerObj is not null)
                ModifiedQuestionAnswersList.Remove(modifiedQuestionAnswerObj);

            var answersResponseObj = QuestionAnswers.FirstOrDefault(item => item.Id == id);

            if (answersResponseObj is not null)
                QuestionAnswers.Remove(answersResponseObj);

            RemovedQuestionAnswersList.Add(id);

            await OnRemovedQuestionAnswersListChanged.InvokeAsync(RemovedQuestionAnswersList);
        }
    }
}