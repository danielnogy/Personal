using BinaryPlate.BlazorPlate.Features.SSM.Questions.Queries.GetQuestions;
using BinaryPlate.BlazorPlate.Features.SSM.Tests.Queries.GetTests;
using Syncfusion.Blazor.Popups;
using BinaryPlate.BlazorPlate.Features.SSM.Tests.Queries.GetTestsQuestions;
using Syncfusion.Blazor.Navigations;
using BinaryPlate.BlazorPlate.Features.SSM.Materials.Queries.GetMaterials;
using BinaryPlate.BlazorPlate.Features.SSM.Tests.Queries.GetTestsMaterials;
using System.Security.Authentication.ExtendedProtection;
using BinaryPlate.BlazorPlate.Features.SSM.Tests.Commands.CreateTest.AddModels;
using BinaryPlate.BlazorPlate.Features.SSM.Tests.Commands.UpdateTest.EditModels;
using BinaryPlate.BlazorPlate.Features.SSM.Employees.Queries.GetEmployees;
using BinaryPlate.BlazorPlate.Features.SSM.Tests.Queries.GetTestsResults;

namespace BinaryPlate.BlazorPlate.Pages.SSM.Tests
{
    public partial class TestsForm
    {
        [Parameter] public string HeaderText { get; set; }
        [Parameter] public TestItem TestModel { get; set; } = new();
        [Parameter] public int TestId { get; set; } = 0;
        [Parameter] public EventCallback SubmitForm { get; set; }
        [Parameter] public string BreadCrumbText { get; set; } = "Editare/Adaugare instruire";

        [Inject] private SfDialogService DialogService { get; set; }
        private EditContextApiExceptionFallback EditContextApiExceptionFallback { get; set; }
        private List<QuestionItem> QuestionItems { get; set; } = new();
        private List<QuestionItem> SelectedQuestions { get; set; } = new();
        private List<MaterialItem> SelectedMaterials { get; set; } = new();
        private List<EmployeeItem> SelectedEmployees { get; set; } = new();
        private QuestionSelection QuestionSelectionComponent { get; set; }
        private MaterialSelection MaterialSelectionComponent { get; set; }
        private EmployeeSelection EmployeeSelectionComponent { get; set; }
        #region TestQuestions 
        [Parameter] public EventCallback<List<TestQuestionItemForAdd>> OnAddedTestQuestionsListChanged { get; set; }
        [Parameter] public EventCallback<List<TestQuestionItemForEdit>> OnModifiedTestQuestionsListChanged { get; set; }
        [Parameter] public EventCallback<List<int>> OnRemovedTestQuestionsListChanged { get; set; }
        public List<TestQuestionItemForAdd> AddedTestQuestionsList { get; set; } = new();
        public List<TestQuestionItemForEdit> ModifiedTestQuestionsList { get; set; } = new();
        public List<int> RemovedTestQuestionsList { get; set; } = new();

        #endregion
        #region TestMaterials
        [Parameter] public EventCallback<List<TestMaterialItemForAdd>> OnAddedTestMaterialsListChanged { get; set; }
        [Parameter] public EventCallback<List<TestMaterialItemForEdit>> OnModifiedTestMaterialsListChanged { get; set; }
        [Parameter] public EventCallback<List<int>> OnRemovedTestMaterialsListChanged { get; set; }
        public List<TestMaterialItemForAdd> AddedTestMaterialsList { get; set; } = new();
        public List<TestMaterialItemForEdit> ModifiedTestMaterialsList { get; set; } = new();
        public List<int> RemovedTestMaterialsList { get; set; } = new();

        #endregion        
        #region TestResults
        [Parameter] public EventCallback<List<TestResultItemForAdd>> OnAddedTestResultsListChanged { get; set; }
        [Parameter] public EventCallback<List<TestResultItemForEdit>> OnModifiedTestResultsListChanged { get; set; }
        [Parameter] public EventCallback<List<int>> OnRemovedTestResultsListChanged { get; set; }
        public List<TestResultItemForAdd> AddedTestResultsList { get; set; } = new();
        public List<TestResultItemForEdit> ModifiedTestResultsList { get; set; } = new();
        public List<int> RemovedTestResultsList { get; set; } = new();
        #endregion
        public async void RefreshAddedTestQuestions(List<TestQuestionItemForAdd> itemForAdds)
        {
            await OnAddedTestQuestionsListChanged.InvokeAsync(itemForAdds);
        }
        public async void RefreshAddedTestMaterials(List<TestMaterialItemForAdd> itemForAdds)
        {
            await OnAddedTestMaterialsListChanged.InvokeAsync(itemForAdds);
        }
        public async void RefreshAddedTestResults(List<TestResultItemForAdd> itemForAdds)
        {
            await OnAddedTestResultsListChanged.InvokeAsync(itemForAdds);
        }
        //public async void RefreshModifiedQuestionAnswers(List<AnswerItemForEdit> itemForEdits)
        //{
        //    ModifiedQuestionAnswers = itemForEdits;
        //    await ModifiedQuestionAnswersChanged.InvokeAsync(ModifiedQuestionAnswers);
        //}
        public async void RefreshRemovedTestQuestions(List<int> itemsToRemove)
        {
            await OnRemovedTestQuestionsListChanged.InvokeAsync(itemsToRemove);
        }
        public async void RefreshRemovedTestMaterials(List<int> itemsToRemove)
        {
            await OnRemovedTestMaterialsListChanged.InvokeAsync(itemsToRemove);
        }
        public async void RefreshRemovedTestResults(List<int> itemsToRemove)
        {
            await OnRemovedTestResultsListChanged.InvokeAsync(itemsToRemove);
        }

        public void Select(SelectingEventArgs args)
        {
            if (args.IsSwiped)
            {
                args.Cancel = true;
            }
        }
        protected override async Task OnParametersSetAsync()
        {
            if (TestId !=0)
            {

            }
        }
        protected override async Task OnInitializedAsync()
        {
            //await QuestionCategoriesLoad();
            //SelectedQuestionCategoryId = QuestionCategories.Select(x => x.Id).FirstOrDefault();
            //await QuestionsLoad();
        }
        private void PopulateSelectedQuestionsInTest()
        {
            SelectedQuestions = QuestionSelectionComponent.GetSelectedRecords();
            foreach (var question in SelectedQuestions)
            {
                TestModel.TestQuestions.Add(new TestQuestionItem
                {
                    TestId = TestModel.Id,
                    QuestionId = question.Id
                });
            }
        }
        private void PopulateSelectedMaterialsInTest()
        {
            SelectedMaterials = MaterialSelectionComponent.GetSelectedRecords();
            foreach (var material in SelectedMaterials)
            {
                TestModel.TestMaterials.Add(new TestMaterialItem
                {
                    TestId = TestModel.Id,
                    MaterialId = material.Id
                });
            }
        }

        private void PopulateSelectedEmployeesInTest()
        {
            SelectedEmployees = EmployeeSelectionComponent.GetSelectedRecords();
            foreach (var employee in SelectedEmployees)
            {
                TestModel.TestResults.Add(new TestResultItem
                {
                    TestId = TestModel.Id,
                    EmployeeId = employee.Id
                });
            }
        }
        private async Task OnValidSubmit()
        {
            PopulateSelectedQuestionsInTest();
            PopulateSelectedMaterialsInTest();

            var dialog = await DialogService.ConfirmAsync(
                TestId != 0 ? "Confirmati actualizarile facute?" : "Confirmati inregistrearea noua?",
                "Confirmare");

            if (dialog)
            {
                await SubmitForm.InvokeAsync();
            }
        }
    }
}