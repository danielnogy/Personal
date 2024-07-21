namespace BinaryPlate.Application.Common.Contracts.Infrastructure.Reporting;

/// <summary>
/// This interface represents the contract for generating HTML application reports.
/// </summary>
public interface IReportingService
{
    #region Public Methods

    /// <summary>
    /// Generates a PDF report for a list of applicant items.
    /// </summary>
    /// <param name="applicantItems">The list of applicant items for the report.</param>
    /// <param name="textDirection">The text direction for the content in the generated PDF.</param>
    /// <param name="baseUri">The optional base URI for additional resources in the PDF.</param>
    /// <returns>A task representing the asynchronous operation and returns <see cref="FileMetaData"/>.</returns>
    Task<FileMetaData> GenerateApplicantsPdfReport(List<ApplicantItem> applicantItems, TextDirection textDirection, string baseUri = null);

    #endregion Public Methods
}