using BinaryPlate.BlazorPlate.Consumers.HttpClients.SSM;
using BinaryPlate.BlazorPlate.Contracts.Consumers.SSM;
using BinaryPlate.BlazorPlate.Features.SSM.MaterialCategories.Queries.GetMaterialCategoriess;
using BinaryPlate.BlazorPlate.Features.SSM.Materials.Queries.GetMaterials;
using BinaryPlate.BlazorPlate.Features.SSM.Tests.Commands.CreateTest.AddModels;
using BinaryPlate.BlazorPlate.Features.SSM.Tests.Commands.UpdateTest.EditModels;
using BinaryPlate.BlazorPlate.Features.SSM.Tests.Queries.GetTestsMaterials;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Navigations;
using Syncfusion.Blazor.Popups;

namespace BinaryPlate.BlazorPlate.Pages.SSM.Tests
{
    public partial class MaterialSelection
    {
        [Parameter] public bool Visible { get; set; }
        [Parameter] public int TestId { get; set; }
        [Parameter] public EventCallback<bool> VisibleChanged { get; set; }
        [Inject] private SfDialogService DialogService { get; set; }
        [Inject] public IMaterialsClient MaterialsClient { get; set; }
        [Inject] public IMaterialCategoriesClient MaterialCategoriesClient { get; set; }
        [Inject] TimerHelper TimerObject { get; set; }
        [Inject] public ITestsClient TestsClient { get; set; }

        #region TestMaterials
        [Parameter] public EventCallback<List<TestMaterialItemForAdd>> OnAddedTestMateiralsListChanged { get; set; }
        [Parameter] public EventCallback<List<TestMaterialItemForEdit>> OnModifiedTestMaterialsListChanged { get; set; }
        [Parameter] public EventCallback<List<int>> OnRemovedTestMaterialsListChanged { get; set; }
        public List<TestMaterialItemForAdd> AddedTestMaterialsList = new();
        public List<TestMaterialItemForEdit> ModifiedTestMaterialsList = new();
        public List<int> RemovedTestMaterialsList = new();

        #endregion
        public List<MaterialItem> SelectedMaterials { get; set; } = new();
        private List<MaterialItem> MaterialItems { get; set; } = new();
        private List<MaterialCategoryItem> MaterialCategoryItems { get; set; } = new();
        private SfGrid<MaterialItem> Grid { get; set; }
        private GetMaterialsQuery GetMaterialsQuery { get; set; } = new GetMaterialsQuery();
        private GetMaterialCategoriesQuery GetMaterialCategoriesQuery { get; set; } = new GetMaterialCategoriesQuery();
        private GetMaterialsResponse GetMaterialsResponse { get; set; } = new GetMaterialsResponse();
        private GetMaterialCategoriesResponse GetMaterialCategoriesResponse { get; set; } = new GetMaterialCategoriesResponse();
        private List<TestMaterialItem> TestMaterialItems { get; set; } = new();
        private List<int> TestMaterialItemsMaterialIds { get; set; } = new();
        private GetTestMaterialsQuery GetTestMaterialsQuery { get; set; } = new GetTestMaterialsQuery();


        public int TotalItems { get; set; } = 1;
        public int PageSize { get; set; } = 4;
        public SfPager Pager { get; set; }
        private int SelectedMaterialCategoryId { get; set; }
        private string SearchText { get; set; }

        private void OnRowSelected(RowSelectEventArgs<MaterialItem> args)
        {
            try
            {
                if (!SelectedMaterials.Select(x => x.Id).Contains(args.Data.Id))
                {
                    SelectedMaterials.Add(args.Data);
                    if (!TestMaterialItemsMaterialIds.Contains(args.Data.Id) && TestId != 0)
                    {
                        AddedTestMaterialsList.Add(
                            new TestMaterialItemForAdd
                            {
                                MaterialId = args.Data.Id,
                                TestId = TestId
                            });
                        OnAddedTestMateiralsListChanged.InvokeAsync(AddedTestMaterialsList);
                    }




                }
                if (TestMaterialItems.Any(x => x.MaterialId == args.Data.Id))
                {
                    var selectedId = TestMaterialItems.FirstOrDefault(x => x.MaterialId == args.Data.Id).Id;
                    if (RemovedTestMaterialsList.Contains(selectedId))
                    {
                        RemovedTestMaterialsList.Remove(selectedId);
                        OnRemovedTestMaterialsListChanged.InvokeAsync(RemovedTestMaterialsList);

                    }
                }
                StateHasChanged();
            }
            catch (Exception ex)
            {

                throw ex.InnerException;
            }
        }
        private void OnRowDeselected(RowDeselectEventArgs<MaterialItem> args)
        {
            var itemToRemove = SelectedMaterials.FirstOrDefault(x => x.Id == args.Data.Id);
            if (itemToRemove != null)
            {
                SelectedMaterials.Remove(itemToRemove);
                if (TestMaterialItemsMaterialIds.Contains(args.Data.Id) && TestId != 0)
                {
                    RemovedTestMaterialsList.Add(TestMaterialItems.FirstOrDefault(x => x.MaterialId == args.Data.Id).Id);
                    OnRemovedTestMaterialsListChanged.InvokeAsync(RemovedTestMaterialsList);
                }
                if (AddedTestMaterialsList.Select(x => x.MaterialId).Contains(args.Data.Id))
                {
                    AddedTestMaterialsList.Remove(AddedTestMaterialsList.FirstOrDefault(x => x.MaterialId == args.Data.Id));
                    OnAddedTestMateiralsListChanged.InvokeAsync(AddedTestMaterialsList);
                }
                StateHasChanged();
            }
        }
        private async Task DataBound()
        {
            if (SelectedMaterials != null)
            {
                var idsAlreadySelected = SelectedMaterials.Select(x => x.Id);
                if (idsAlreadySelected.Count() > 0)
                {
                    foreach (var item in MaterialItems)
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

                SelectedMaterialCategoryId = MaterialCategoryItems.Select(x => x.Id).FirstOrDefault();
                TimerObject.OnElapsed += async () =>
                {
                    await MaterialsLoad();
                };
                TimerObject.SetTimer(600);
            }
        }
        private async Task TestMaterialsLoad()
        {
            GetTestMaterialsQuery.RowsPerPage = -1;
            GetTestMaterialsQuery.TestId = TestId;

            var responseWrapper = await TestsClient.GetTestMaterials(GetTestMaterialsQuery);
            if (responseWrapper.IsSuccessStatusCode)
            {
                var response = responseWrapper.Payload;
                TestMaterialItems = response.TestMaterials.Items.ToList();
                TestMaterialItemsMaterialIds = response.TestMaterials.Items.Select(x => x.MaterialId).ToList();


            }
        }
        private async Task MaterialsLoad()
        {
            if (Grid != null)
                await Grid.ShowSpinnerAsync();
            while (Pager == null)
            {
                await Task.Delay(30);
            }
            PageSize = 4;

            GetMaterialsQuery.MaterialCategoryId = SelectedMaterialCategoryId;
            GetMaterialsQuery.PageNumber = Pager.CurrentPage;
            GetMaterialsQuery.RowsPerPage = Pager.PageSize;
            GetMaterialsQuery.SearchText = SearchText;
            GetMaterialsQuery.SortBy = "";
            //if we want to see selected questions
            if (SelectedMaterialCategoryId == -1)
            {
                MaterialItems = SelectedMaterials
                                    .Where(x =>
                                        x.Title.Contains(!string.IsNullOrWhiteSpace(SearchText) ? SearchText : ""))
                                    .ToList();
                TotalItems = SelectedMaterials.Count;
                PageSize = SelectedMaterials.Count;
                await Grid.Refresh();
                if (Grid != null)
                    await Grid.HideSpinnerAsync();
                StateHasChanged();
                return;
            }
            var responseWrapper = await MaterialsClient.GetMaterials(GetMaterialsQuery);
            if (responseWrapper.IsSuccessStatusCode)
            {
                if (responseWrapper.Payload != null)
                {
                    TotalItems = responseWrapper.Payload.Materials.TotalRows;
                    GetMaterialsResponse = responseWrapper.Payload;
                    if (TestId != 0)
                    {
                        foreach (var item in TestMaterialItems)
                        {
                            if (GetMaterialsResponse.Materials.Items.Select(x => x.Id).Contains(item.MaterialId))
                            {
                                SelectedMaterials.Add(GetMaterialsResponse.Materials.Items.FirstOrDefault(x => x.Id == item.MaterialId));
                            }
                        }
                    }
                    MaterialItems = GetMaterialsResponse.Materials.Items.ToList();
                    if (Grid != null)
                        await Grid.Refresh();
                    StateHasChanged();

                }
            }
            if (Grid != null)
                await Grid.HideSpinnerAsync();
        }
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