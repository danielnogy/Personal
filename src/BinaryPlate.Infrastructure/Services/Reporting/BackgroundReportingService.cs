using BinaryPlate.Application.Features.POC.Applicants.Queries.ExportApplicants;

namespace BinaryPlate.Infrastructure.Services.Reporting;

public class BackgroundReportingService(ITenantResolver tenantResolver,
                                        IUtcDateTimeProvider utcDateTimeProvider,
                                        IApplicationDbContext dbContext,
                                        IHubNotificationService hubNotificationService,
                                        IReportDataProvider reportDataProvider,
                                        IReportingService reportingService) : IBackgroundReportingService
{
    #region Public Methods

    public async Task InitiateReport(ExportApplicantsQuery request, Guid reportId, string userNameIdentifier, Guid? tenantId = null, string tenantName = null)
    {
        // Set tenant information using the provided parameters.
        tenantResolver.SetTenantInfo(tenantId, tenantName);

        // Update the report status to pending and notify the user.
        await UpdateStatusAndNotify(reportId, ReportStatus.Pending, userNameIdentifier);

        // Set the initial status for the report.
        await SetInitialReportStatus(request, reportId);
    }

    public async Task<FileMetaData> ExportDataAsPdfInBackground(ExportApplicantsQuery request,
                                                                Guid reportId,
                                                                string userNameIdentifier,
                                                                TextDirection textDirection,
                                                                string baseUri,
                                                                Guid? tenantId = null,
                                                                string tenantName = null)
    {
        // Set tenant information using the provided parameters.
        tenantResolver.SetTenantInfo(tenantId, tenantName);

        // Create a new FileMetaData object to store information about the exported PDF file.
        FileMetaData fileMetaData = new();

        try
        {
            // Update the report status to in progress and notify the user.
            await UpdateStatusAndNotify(reportId, ReportStatus.InProgress, userNameIdentifier, fileMetaData);

            // Get applicant data from the report data provider.
            var applicantItems = await reportDataProvider.GetApplicants(new GetApplicantsQuery { SearchText = request.SearchText, SortBy = request.SortBy });

            // Generate the PDF file from the provided HTML template and the applicant data.
            fileMetaData = await reportingService.GenerateApplicantsPdfReport(applicantItems, textDirection, baseUri);

            // Update the report status to completed and notify the user.
            await UpdateStatusAndNotify(reportId, ReportStatus.Completed, userNameIdentifier, fileMetaData);
        }
        catch
        {
            // If an exception occurs, update the report status to failed and notify the user.
            await UpdateStatusAndNotify(reportId, ReportStatus.Failed, userNameIdentifier, fileMetaData);
        }

        // Return the FileMetaData object representing the exported PDF file.
        return fileMetaData;

    }

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Sets an initial pending status for a report based on the provided query and report ID.
    /// </summary>
    /// <param name="request">The dynamic request object containing query parameters.</param>
    /// <param name="reportId">The ID of the report.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task SetInitialReportStatus(dynamic request, Guid reportId)
    {
        // Add a new report with initial pending status to the database.
        dbContext.Reports.Add(new Report
        {
            Id = reportId,
            Title = "N/A",
            QueryString = $"SearchText:{request.SearchText ?? "All"}, SortBy:{request.SortBy}",
            Status = (int)ReportStatus.Pending
        });

        // Save changes to the database.
        await dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Updates the status of a report, notifies the issuer and viewers, and refreshes the reports for viewers.
    /// </summary>
    /// <param name="reportId">The ID of the report.</param>
    /// <param name="status">The new status of the report.</param>
    /// <param name="userNameIdentifier">The identifier of the user initiating the update.</param>
    /// <param name="fileMetaData">Optional file metadata associated with the report.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task UpdateStatusAndNotify(Guid reportId, ReportStatus status, string userNameIdentifier, FileMetaData fileMetaData = null)
    {
        // Wait briefly to simulate processing time.
        //await Task.Delay(1000);

        // Retrieve the report from the database.
        var report = await dbContext.Reports.Where(r => r.Id == reportId).FirstOrDefaultAsync();

        // If the report exists, update its status and associated file metadata.
        if (report != null)
        {
            report.Title = $"{utcDateTimeProvider.GetUtcNow().ToLongDateString()} {fileMetaData?.FileName}";
            report.Status = (int)status;
            report.ContentType = fileMetaData?.ContentType;
            report.FileName = fileMetaData?.FileName;
            report.FileUri = fileMetaData?.FileUri;

            // Save changes to the database.
            await dbContext.SaveChangesAsync();

            // Notify the issuer of the updated status.
            await hubNotificationService.NotifyReportIssuer(userNameIdentifier, fileMetaData, status);
        }

        // Refresh reports for viewers.
        await hubNotificationService.RefreshReportsViewer(userNameIdentifier);
    }

    #endregion Private Methods
}
