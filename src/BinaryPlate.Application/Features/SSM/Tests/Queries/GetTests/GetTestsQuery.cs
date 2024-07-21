namespace BinaryPlate.Application.Features.SSM.Tests.Queries.GetTests;

public class GetTestsQuery : FilterableQuery, IRequest<Envelope<GetTestsResponse>>
{
    #region Public Classes

    public class GetTestsQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetTestsQuery, Envelope<GetTestsResponse>>
    {
        #region Public Methods

        public async Task<Envelope<GetTestsResponse>> Handle(GetTestsQuery request, CancellationToken cancellationToken)
        {
            // Start with a query that retrieves all tests from the database.
            var query = dbContext.Tests.AsQueryable();

            // If a search text is provided, filter the tests by their first name or last name.
            if (!string.IsNullOrWhiteSpace(request.SearchText))
                query = query.Where(a => a.Title.Contains(request.SearchText));

            // If a sort by field is provided, sort the query by that field; otherwise, sort by
            // first name and then last name.

            query = !string.IsNullOrWhiteSpace(request.SortBy)
                ? query.SortBy(request.SortBy)
                : query.OrderBy(a => a.Title)/*.ThenBy(a => a.LastName)*/;

            // Convert the query to a paged list of test item DTOs.
            var testItems = await query.Select(q => TestItem.MapFromEntity(q, false))
                                            .AsNoTracking()
                                            .ToPagedListAsync(request.PageNumber, request.RowsPerPage);

            // Create an tests response DTO with the list of test item DTOs.
            var response = new GetTestsResponse
            {
                Tests = testItems
            };

            // Return a success response with the tests response DTO as the payload.
            return Envelope<GetTestsResponse>.Result.Ok(response);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}