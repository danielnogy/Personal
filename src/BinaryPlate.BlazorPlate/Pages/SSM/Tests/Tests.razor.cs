using BinaryPlate.BlazorPlate.Contracts.Consumers.SSM;
using BinaryPlate.BlazorPlate.Features.SSM.Tests.Queries.GetTests;
using BinaryPlate.BlazorPlate.Shared.SSM;
using Syncfusion.Blazor.Popups;

namespace BinaryPlate.BlazorPlate.Pages.SSM.Tests
{
    public partial class Tests
    {
        [Inject] public ITestsClient TestsClient { get; set; }
        [Inject] private BreadcrumbService BreadcrumbService { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private SfDialogService DialogService { get; set; }
        private GetTestsQuery GetTestsQuery { get; set; } = new GetTestsQuery();
        private GetTestsResponse GetTestsResponse { get; set; } = new GetTestsResponse();
        private List<TestItem> Items { get; set; }
        private GridComponent<TestItem> Grid { get; set; }
        private TestItem SelectedRecord { get; set; }
        private int PageCount { get; set; } = 2;
        private int TotalItems { get; set; } = 1;
        private string SearchText { get; set; } = "";

        protected override async Task OnInitializedAsync()
        {
            BreadcrumbService.SetBreadcrumbItems(new List<MudBlazor.BreadcrumbItem>
            {
                new("Acasa", "/"),
                new("SSM", "#", true),
                new("Instruiri", "#", true)
            });
        }
        private async Task<IEnumerable<TestItem>> ExcelDatasourceLoad()
        {
            GetTestsQuery.SearchText = SearchText;
            GetTestsQuery.PageNumber = Grid.Pager.CurrentPage;
            GetTestsQuery.RowsPerPage = -1;
            GetTestsQuery.SortBy = "";
            var responseWrapper = await TestsClient.GetTests(GetTestsQuery);
            if (responseWrapper.IsSuccessStatusCode)
            {
                if (responseWrapper.Payload != null)
                {

                    GetTestsResponse = responseWrapper.Payload;
                    return GetTestsResponse.Tests.Items.AsEnumerable();
                }
            }
            return new List<TestItem>();
        }
        private async Task ServerLoad()
        {

            GetTestsQuery.SearchText = SearchText;
            GetTestsQuery.PageNumber = Grid.Pager.CurrentPage;
            GetTestsQuery.RowsPerPage = Grid.Pager.PageSize;
            GetTestsQuery.SortBy = "";

            var responseWrapper = await TestsClient.GetTests(GetTestsQuery);
            if (responseWrapper.IsSuccessStatusCode)
            {
                if (responseWrapper.Payload != null)
                {
                    TotalItems = responseWrapper.Payload.Tests.TotalRows;
                    GetTestsResponse = responseWrapper.Payload;
                    Items = GetTestsResponse.Tests.Items.ToList();
                    await Grid.Refresh();
                    StateHasChanged();

                }
            }
        }
        private async void Add()
        {
            NavigationManager.NavigateTo($"ssm/Tests/addTest");
        }
        private async void Delete(TestItem selectedItem)
        {
            if (selectedItem != null)
            {
                await TestsClient.DeleteTest(selectedItem.Id);
                await ServerLoad();
            }
        }
        private void Edit(TestItem selectedItem)
        {
            if (selectedItem != null)
                NavigationManager.NavigateTo($"ssm/Tests/editTest/{selectedItem.Id}");
        }
    }
}