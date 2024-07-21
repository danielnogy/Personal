namespace BinaryPlate.Infrastructure.Services.Reporting;

public class ReportDataProvider(IApplicationDbContext applicationDbContext) : IReportDataProvider
{
    #region Public Methods

    public async Task<List<ApplicantItem>> GetApplicants(GetApplicantsQuery request)
    {
        // Create a query to retrieve applicants from the application database context, including references.
        var query = applicationDbContext.Applicants.Include(a => a.References).AsQueryable();

        // Filter applicants based on the search text if provided.
        if (!string.IsNullOrWhiteSpace(request.SearchText))
        {
            query = query.Where(a =>
                                    a.FirstName.Contains(request.SearchText) ||
                                    a.LastName.Contains(request.SearchText) ||
                                    a.Ssn.ToString().Contains(request.SearchText)
                               );
        }

        // Apply sorting based on the provided SortBy parameter or default to sorting by FirstName and LastName.
        query = !string.IsNullOrWhiteSpace(request.SortBy)
            ? query.SortBy(request.SortBy)  // Ensure SortBy is a valid extension method
            : query.OrderBy(a => a.FirstName).ThenBy(a => a.LastName);

        // Execute the query and map the results to ApplicantItem entities.
        var applicantItems = await query.Select(q => ApplicantItem.MapFromEntity(q, true))
                                        .AsNoTracking()
                                        .ToListAsync();

        // Return the list of mapped ApplicantItem entities.
        return applicantItems;
    }

    #endregion Public Methods
}