namespace BinaryPlate.Application.Features.SSM.Tests.Queries.GetTestsResults;

public class GetTestResultsQuery : FilterableQuery, IRequest<Envelope<GetTestResultsResponse>>
{
    #region Public Properties

    public int TestId { get; set; }

    #endregion Public Properties

    public class GetTestResultsQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetTestResultsQuery, Envelope<GetTestResultsResponse>>
    {
        #region Public Methods

        #region Public Methods

        public async Task<Envelope<GetTestResultsResponse>> Handle(GetTestResultsQuery request, CancellationToken cancellationToken)
        {
            // Start with a query that retrieves all references for the specified test from the database.
            var query = dbContext.TestResults.Include(x=>x.Employee).Where(a => a.TestId == request.TestId);

            // If a sort by field is provided, sort the query by that field; otherwise, sort by name.
            query = !string.IsNullOrWhiteSpace(request.SortBy)
                ? query.SortBy(request.SortBy)
                : query.OrderBy(a => a.Employee.Name);

            // Convert the query to a paged list of test reference item DTOs.
            var testResultItems = await query.Select(q => TestResultItem.MapFromEntity(q)).
                                                      ToPagedListAsync(request.PageNumber, request.RowsPerPage);

            // Create an test references response DTO with the list of test reference item DTOs.
            var response = new GetTestResultsResponse
            {
                TestResults = testResultItems
            };

            // Return a success response with the test references response DTO as the payload.
            return Envelope<GetTestResultsResponse>.Result.Ok(response);
        }

        #endregion Public Methods
    }

    #endregion Public Methods
}