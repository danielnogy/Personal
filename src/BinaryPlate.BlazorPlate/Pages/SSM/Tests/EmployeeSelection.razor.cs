using BinaryPlate.BlazorPlate.Contracts.Consumers.SSM;
using BinaryPlate.BlazorPlate.Features.SSM.Employees.Queries.GetEmployees;
using BinaryPlate.BlazorPlate.Features.SSM.Tests.Commands.CreateTest.AddModels;
using BinaryPlate.BlazorPlate.Features.SSM.Tests.Commands.UpdateTest.EditModels;
using BinaryPlate.BlazorPlate.Features.SSM.Tests.Queries.GetQuestionForEdit;
using BinaryPlate.BlazorPlate.Features.SSM.Tests.Queries.GetTestsMaterials;
using BinaryPlate.BlazorPlate.Features.SSM.Tests.Queries.GetTestsResults;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Navigations;
using Syncfusion.Blazor.Popups;

namespace BinaryPlate.BlazorPlate.Pages.SSM.Tests
{
    public partial class EmployeeSelection:ComponentBase
    {
        [Parameter] public bool Visible { get; set; }
        [Parameter] public int TestId { get; set; }
        [Parameter] public EventCallback<bool> VisibleChanged { get; set; }
        [Inject] private SfDialogService DialogService { get; set; }
        [Inject] public IEmployeesClient EmployeesClient { get; set; }
        [Inject] public ITestsClient TestsClient { get; set; }
        [Inject] TimerHelper TimerObject { get; set; }

        #region TestMaterials
        [Parameter] public EventCallback<List<TestResultItemForAdd>> OnAddedTestResultsListChanged { get; set; }
        [Parameter] public EventCallback<List<TestResultItemForEdit>> OnModifiedTestResultsListChanged { get; set; }
        [Parameter] public EventCallback<List<int>> OnRemovedTestResultsListChanged { get; set; }
        public List<TestResultItemForAdd> AddedTestResultsList = new();
        public List<TestResultItemForEdit> ModifiedTestResultsList = new();
        public List<int> RemovedTestResultsList = new();

        #endregion
        public List<EmployeeItem> SelectedEmployees { get; set; } = new();
        private List<EmployeeItem> EmployeeItems { get; set; } = new();
        private SfGrid<EmployeeItem> Grid { get; set; }
        private GetEmployeesQuery GetEmployeesQuery { get; set; } = new ();
        private GetEmployeesResponse GetEmployeesResponse { get; set; } = new ();
        private List<TestResultItem> TestResultItems { get; set; } = new();
        private List<int> TestResultItemsResultIds { get; set; } = new();
        private GetTestResultsQuery GetTestResultsQuery { get; set; } = new ();


        public int TotalItems { get; set; } = 1;
        public int PageSize { get; set; } = 4;
        public SfPager Pager { get; set; }
        private int SelectedMaterialCategoryId { get; set; }
        private string SearchText { get; set; }

        private void OnRowSelected(RowSelectEventArgs<EmployeeItem> args)
        {
            try
            {
                if (!SelectedEmployees.Select(x => x.Id).Contains(args.Data.Id))
                {
                    SelectedEmployees.Add(args.Data);
                    if (!TestResultItemsResultIds.Contains(args.Data.Id) && TestId != 0)
                    {
                        AddedTestResultsList.Add(
                            new TestResultItemForAdd
                            {
                                EmployeeId = args.Data.Id,
                                TestId = TestId
                            });
                        OnAddedTestResultsListChanged.InvokeAsync(AddedTestResultsList);
                    }




                }
                if (TestResultItems.Any(x => x.EmployeeId == args.Data.Id))
                {
                    var selectedId = TestResultItems.FirstOrDefault(x => x.EmployeeId == args.Data.Id).Id;
                    if (RemovedTestResultsList.Contains(selectedId))
                    {
                        RemovedTestResultsList.Remove(selectedId);
                        OnRemovedTestResultsListChanged.InvokeAsync(RemovedTestResultsList);

                    }
                }
                StateHasChanged();
            }
            catch (Exception ex)
            {

                throw ex.InnerException;
            }
        }
        private void OnRowDeselected(RowDeselectEventArgs<EmployeeItem> args)
        {
            var itemToRemove = SelectedEmployees.FirstOrDefault(x => x.Id == args.Data.Id);
            if (itemToRemove != null)
            {
                SelectedEmployees.Remove(itemToRemove);
                if (TestResultItemsResultIds.Contains(args.Data.Id) && TestId != 0)
                {
                    RemovedTestResultsList.Add(TestResultItems.FirstOrDefault(x => x.EmployeeId == args.Data.Id).Id);
                    OnRemovedTestResultsListChanged.InvokeAsync(RemovedTestResultsList);
                }
                if (AddedTestResultsList.Select(x => x.EmployeeId).Contains(args.Data.Id))
                {
                    AddedTestResultsList.Remove(AddedTestResultsList.FirstOrDefault(x => x.EmployeeId == args.Data.Id));
                    OnAddedTestResultsListChanged.InvokeAsync(AddedTestResultsList);
                }
                StateHasChanged();
            }
        }
        private async Task DataBound()
        {
            if (SelectedEmployees != null)
            {
                var idsAlreadySelected = SelectedEmployees.Select(x => x.Id);
                if (idsAlreadySelected.Count() > 0)
                {
                    foreach (var item in EmployeeItems)
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
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await MaterialCategoriesLoad();
                if (TestId != 0)
                {
                    await TestMaterialsLoad();
                }

                TimerObject.OnElapsed += async () =>
                {
                    await MaterialsLoad();
                };
                TimerObject.SetTimer(600);
            }
        }
        private async Task TestMaterialsLoad()
        {
            GetTestResultsQuery.RowsPerPage = -1;
            GetTestResultsQuery.TestId = TestId;

            var responseWrapper = await TestsClient.GetTestResults(GetTestResultsQuery);
            if (responseWrapper.IsSuccessStatusCode)
            {
                var response = responseWrapper.Payload;
                TestResultItems = response.TestResults.Items.ToList();
                TestResultItemsResultIds = response.TestResults.Items.Select(x => x.EmployeeId).ToList();


            }
        }
        private async Task EmployeesLoad()
        {
            if (Grid != null)
                await Grid.ShowSpinnerAsync();
            while (Pager == null)
            {
                await Task.Delay(30);
            }
            PageSize = 4;

            GetEmployeesQuery.PageNumber = Pager.CurrentPage;
            GetEmployeesQuery.RowsPerPage = Pager.PageSize;
            GetEmployeesQuery.SearchText = SearchText;
            GetEmployeesQuery.SortBy = "";
            //if we want to see selected questions
            //if (SelectedMaterialCategoryId == -1)
            //{
            //    MaterialItems = SelectedMaterials
            //                        .Where(x =>
            //                            x.Title.Contains(!string.IsNullOrWhiteSpace(SearchText) ? SearchText : ""))
            //                        .ToList();
            //    TotalItems = SelectedMaterials.Count;
            //    PageSize = SelectedMaterials.Count;
            //    await Grid.Refresh();
            //    if (Grid != null)
            //        await Grid.HideSpinnerAsync();
            //    StateHasChanged();
            //    return;
            //}
            //DEPARTMENTS INSTEAD OF CATEGORIES
            var responseWrapper = await EmployeesClient.GetEmployees(GetEmployeesQuery);
            if (responseWrapper.IsSuccessStatusCode)
            {
                if (responseWrapper.Payload != null)
                {
                    TotalItems = responseWrapper.Payload.Materials.TotalRows;
                    GetEmployeesResponse = responseWrapper.Payload;
                    if (TestId != 0)
                    {
                        foreach (var item in TestResultItems)
                        {
                            if (GetEmployeesResponse.Employees.Items.Select(x => x.Id).Contains(item.EmployeeId))
                            {
                                SelectedEmployees.Add(GetEmployeesResponse.Employees.Items.FirstOrDefault(x => x.Id == item.EmployeeId));
                            }
                        }
                    }
                    EmployeeItems = GetEmployeesResponse.Employees.Items.ToList();
                    if (Grid != null)
                        await Grid.Refresh();
                    StateHasChanged();

                }
            }
            if (Grid != null)
                await Grid.HideSpinnerAsync();
        }

        //getdepartments 
        private async Task MaterialCategoriesLoad()
        {

            while (Pager == null)
            {
                await Task.Delay(30);
            }
            GetMaterialCategoriesQuery.RowsPerPage = -1;
            GetMaterialCategoriesQuery.SortBy = "";

            var responseWrapper = await MaterialCategoriesClient.GetMaterialCategories(GetMaterialCategoriesQuery);
            if (responseWrapper.IsSuccessStatusCode)
            {
                if (responseWrapper.Payload != null)
                {
                    //TotalItems = responseWrapper.Payload.Materials.TotalRows;
                    GetMaterialCategoriesResponse = responseWrapper.Payload;
                    MaterialCategoryItems = GetMaterialCategoriesResponse.MaterialCategories.Items.ToList();
                    MaterialCategoryItems.Add(new MaterialCategoryItem { Id = 0, Name = " Toate " });
                    MaterialCategoryItems.Add(new MaterialCategoryItem { Id = -1, Name = " Selectate " });
                    if (Grid != null)
                        await Grid.Refresh();
                    StateHasChanged();

                }
            }
        }
        private async void PageChangedHandler(PageChangedEventArgs args)
        {
            await MaterialsLoad();
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
        public List<MaterialItem> GetSelectedRecords()
        {
            return SelectedMaterials;
        }
        public void SetSelectedRecords(List<MaterialItem> items)
        {
            SelectedMaterials = items;
        }

        public void Dispose()
        {
            TimerObject.Dispose();
        }
    }
}