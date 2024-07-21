namespace BinaryPlate.Application.Features.SSM.Tests.Queries.GetTestsMaterials;

public class GetTestMaterialsQuery : FilterableQuery, IRequest<Envelope<GetTestMaterialsResponse>>
{
    #region Public Properties

    public int TestId { get; set; }

    #endregion Public Properties

    public class GetTestMaterialsQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetTestMaterialsQuery, Envelope<GetTestMaterialsResponse>>
    {
        #region Public Methods

        #region Public Methods

        public async Task<Envelope<GetTestMaterialsResponse>> Handle(GetTestMaterialsQuery request, CancellationToken cancellationToken)
        {
            // Start with a query that retrieves all references for the specified test from the database.
            var query = dbContext.TestMaterials.Include(x=>x.Material).Where(a => a.TestId == request.TestId);

            // If a sort by field is provided, sort the query by that field; otherwise, sort by name.
            query = !string.IsNullOrWhiteSpace(request.SortBy)
                ? query.SortBy(request.SortBy)
                : query.OrderBy(a => a.Material.Id);

            // Convert the query to a paged list of test reference item DTOs.
            var testMaterialItems = await query.Select(q => TestMaterialItem.MapFromEntity(q)).
                                                      ToPagedListAsync(request.PageNumber, request.RowsPerPage);

            // Create an test references response DTO with the list of test reference item DTOs.
            var response = new GetTestMaterialsResponse
            {
                TestMaterials = testMaterialItems
            };

            // Return a success response with the test references response DTO as the payload.
            return Envelope<GetTestMaterialsResponse>.Result.Ok(response);
        }

        #endregion Public Methods
    }

    #endregion Public Methods
}