namespace BinaryPlate.WebAPI.Hubs;
/// <summary>
/// SignalR Hub for handling reporting services and exporting data.
/// </summary>
[Authorize]
public class ReportingServicesHub(IBackgroundJobClient backgroundJob,
                                  ISignalRContextInfoProvider signalRContextInfoProvider,
                                  IBackgroundReportingService backgroundReportingService) : Hub
{
    /// <summary>
    /// Invoked when a connection to the hub is established.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public override Task OnConnectedAsync()
    {
        if (Context.UserIdentifier != null)
        {
            // Get the username associated with the connected user.
            var name = signalRContextInfoProvider.GetUserName(Context);

            // Add the user to the group associated with their username.
            Groups.AddToGroupAsync(Context.ConnectionId, name);
        }

        return base.OnConnectedAsync();
    }

    /// <summary>
    /// Initiates the process of exporting applicants' data to a PDF file.
    /// </summary>
    /// <param name="request">Query parameters for exporting applicants.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task ExportApplicantToPdf(ExportApplicantsQuery request)
    {
        // Retrieve tenant information using the SignalR context information provider.
        var tenantResolver = signalRContextInfoProvider.SetTenantIdViaTenantResolver(Context);

        // Generate a unique report identifier.
        var reportId = Guid.NewGuid();

        // Get the unique identifier of the user who initiated the request.
        var userNameIdentifier = signalRContextInfoProvider.GetUserNameIdentifier(Context);

        // Get the base URL associated with the context.
        var baseUrl = signalRContextInfoProvider.GetHostName(Context);

        // Retrieve the resolved tenant ID.
        var tenantId = tenantResolver.GetTenantId();

        // Retrieve the resolved tenant name.
        var tenantName = tenantResolver.GetTenantName();

        // Get the text direction based on the user's language in the context.
        var textDirection = signalRContextInfoProvider.GetUserLanguageDirection(Context);

        // Enqueue a background job to initiate the report.
        var pendingJob = backgroundJob.Enqueue<IBackgroundReportingService>(brs => backgroundReportingService.InitiateReport(request, reportId, userNameIdentifier, tenantId, tenantName));

        // Continue the job with exporting data as a PDF in the background.
        backgroundJob.ContinueJobWith<IBackgroundReportingService>(pendingJob, brs => backgroundReportingService.ExportDataAsPdfInBackground(request, reportId, userNameIdentifier, textDirection, baseUrl, tenantId, tenantName));

        await Task.CompletedTask;
    }
}