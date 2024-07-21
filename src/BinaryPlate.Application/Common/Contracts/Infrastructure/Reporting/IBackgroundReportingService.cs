namespace BinaryPlate.Application.Common.Contracts.Infrastructure.Reporting;

/// <summary>
/// This interface represents the contract for exporting application-specific reports using background job.
/// </summary>
public interface IBackgroundReportingService
{
    #region Public Methods

    /// <summary>
    /// Initiates a background report generation.
    /// </summary>
    /// <param name="request">The export applicants query.</param>
    /// <param name="reportId">The unique identifier for the report.</param>
    /// <param name="userNameIdentifier">The user name identifier initiating the report.</param>
    /// <param name="tenantId">The optional tenant identifier.</param>
    /// <param name="tenantName">The optional tenant name.</param>
    /// <returns>A task representing the asynchronous initiation of the report.</returns>
    Task InitiateReport(ExportApplicantsQuery request, Guid reportId, string userNameIdentifier, Guid? tenantId = null, string tenantName = null);

    /// <summary>
    /// Exports data as a PDF in the background.
    /// </summary>
    /// <param name="request">The export applicants query.</param>
    /// <param name="reportId">The unique identifier for the report.</param>
    /// <param name="userNameIdentifier">The user name identifier initiating the export.</param>
    /// <param name="textDirection">The text direction for the exported content.</param>
    /// <param name="baseUri">The base URI for the exported content.</param>
    /// <param name="tenantId">The optional tenant identifier.</param>
    /// <param name="tenantName">The optional tenant name.</param>
    /// <returns>A task representing the asynchronous export of data as a PDF in the background.</returns>
    Task<FileMetaData> ExportDataAsPdfInBackground(ExportApplicantsQuery request,
                                                   Guid reportId,
                                                   string userNameIdentifier,
                                                   TextDirection textDirection,
                                                   string baseUri,
                                                   Guid? tenantId = null,
                                                   string tenantName = null);

    #endregion Public Methods
}