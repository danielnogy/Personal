using BinaryPlate.BlazorPlate.Consumers.SignalRClients;

namespace BinaryPlate.BlazorPlate.Shared;

public partial class Dashboard : IAsyncDisposable
{
    #region Private Fields

    private readonly EarningReport[] _earningReports =
    [
        new EarningReport
        {
            Name = "Lunees",
            Title = "Reactor Engineer",
            Avatar = "https://avatars2.githubusercontent.com/u/71094850?s=460&u=66c16f5bb7d27dc751f6759a82a3a070c8c7fe4b&v=4",
            Salary = "$0.99",
            Severity = Color.Success,
            SeverityTitle = Resource.Low
        },
        new EarningReport
        {
            Name = "Mikes-gh",
            Title = "Developer",
            Avatar = "https://avatars.githubusercontent.com/u/16208742?s=120&v=4",
            Salary = "$19.12K",
            Severity = Color.Secondary,
            SeverityTitle = Resource.Medium
        },
        new EarningReport
        {
            Name = "Garderoben",
            Title = "CSS Magician",
            Avatar = "https://avatars2.githubusercontent.com/u/10367109?s=460&amp;u=2abf95f9e01132e8e2915def42895ffe99c5d2c6&amp;v=4",
            Salary = "$1337",
            Severity = Color.Primary,
            SeverityTitle = Resource.High
        }
    ];

    #endregion Private Fields

    #region Private Properties

    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private DashboardHubClient DashboardHubClient { get; set; }
    [Inject] private BreadcrumbService BreadcrumbService { get; set; }
    [Inject] private IDashboardClient DashboardClient { get; set; }

    private GetHeadlinesResponse GetHeadlinesResponse { get; set; } = new();

    #endregion Private Properties

    #region Public Methods

    public async ValueTask DisposeAsync()
    {
        await DashboardHubClient.CloseHubConnection();
        DashboardHubClient.OnHeadlinesDataReceived -= HandleHeadlinesDataReceived;
    }

    #endregion Public Methods

    #region Protected Methods

    protected override async Task OnInitializedAsync()
    {
        BreadcrumbService.SetBreadcrumbItems([
            new BreadcrumbItem(Resource.Home, "/"),
            new BreadcrumbItem(Resource.Dashboard, "#", true)
        ]);

        var responseWrapper = await DashboardClient.GetHeadlinesData();

        if (responseWrapper.IsSuccessStatusCode)
        {
            GetHeadlinesResponse = responseWrapper.Payload;

            if (await DashboardHubClient.StartHubConnection())
            {
                await DashboardHubClient.GetHeadlinesData(GetHeadlinesResponse);
                DashboardHubClient.OnHeadlinesDataReceived += HandleHeadlinesDataReceived;
            }
        }
        else
        {
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }
    }

    #endregion Protected Methods

    #region Private Methods

    private void HandleHeadlinesDataReceived(GetHeadlinesResponse data)
    {
        GetHeadlinesResponse = data;
        InvokeAsync(StateHasChanged);
    }

    #endregion Private Methods
}