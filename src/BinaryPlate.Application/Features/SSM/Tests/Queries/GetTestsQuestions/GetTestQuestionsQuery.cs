namespace BinaryPlate.Application.Features.SSM.Tests.Queries.GetTestsQuestions;

public class GetTestQuestionsQuery : FilterableQuery, IRequest<Envelope<GetTestQuestionsResponse>>
{
    #region Public Properties

    public int TestId { get; set; }

    #endregion Public Properties

    public class GetTestQuestionsQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetTestQuestionsQuery, Envelope<GetTestQuestionsResponse>>
    {
        #region Public Methods

        #region Public Methods

        public async Task<Envelope<GetTestQuestionsResponse>> Handle(GetTestQuestionsQuery request, CancellationToken cancellationToken)
        {
            // Start with a query that retrieves all references for the specified test from the database.
            var query = dbContext.TestQuestions.Include(x=>x.Question).Where(a => a.TestId == request.TestId);

            // If a sort by field is provided, sort the query by that field; otherwise, sort by name.
            query = !string.IsNullOrWhiteSpace(request.SortBy)
                ? query.SortBy(request.SortBy)
                : query.OrderBy(a => a.Question.Text);

            // Convert the query to a paged list of test reference item DTOs.
            var testQuestionItems = await query.Select(q => TestQuestionItem.MapFromEntity(q)).
                                                      ToPagedListAsync(request.PageNumber, request.RowsPerPage);

            // Create an test references response DTO with the list of test reference item DTOs.
            var response = new GetTestQuestionsResponse
            {
                TestQuestions = testQuestionItems
            };

            // Return a success response with the test references response DTO as the payload.
            return Envelope<GetTestQuestionsResponse>.Result.Ok(response);
        }

        #endregion Public Methods
    }

    #endregion Public Methods
}