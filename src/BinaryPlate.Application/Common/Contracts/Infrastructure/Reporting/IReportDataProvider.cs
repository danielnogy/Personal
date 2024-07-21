namespace BinaryPlate.Application.Common.Contracts.Infrastructure.Reporting;

/// <summary>
/// This interface represents the contract for fetching report data.
/// </summary>
public interface IReportDataProvider
{
    #region Public Methods


    /// <summary>
    /// Retrieves a list of applicants based on the specified query.
    /// </summary>
    /// <param name="request">The query parameters for retrieving applicants.</param>
    /// <returns>A task that represents the asynchronous operation and returns a list of <see cref="ApplicantItem"/>.</returns>
    Task<List<ApplicantItem>> GetApplicants(GetApplicantsQuery request);


    #endregion Public Methods
}