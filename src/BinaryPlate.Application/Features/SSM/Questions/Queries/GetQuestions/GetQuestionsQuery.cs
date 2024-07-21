namespace BinaryPlate.Application.Features.SSM.Questions.Queries.GetQuestions;

public class GetQuestionsQuery : FilterableQuery, IRequest<Envelope<GetQuestionsResponse>>
{
    public int QuestionCategoryId { get; set; }
    #region Public Classes

    public class GetQuestionsQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetQuestionsQuery, Envelope<GetQuestionsResponse>>
    {
        #region Public Methods

        public async Task<Envelope<GetQuestionsResponse>> Handle(GetQuestionsQuery request, CancellationToken cancellationToken)
        {
            // Start with a query that retrieves all questions from the database.
            var query = dbContext.Questions.AsQueryable();

            // If a search text is provided, filter the questions by their first name or last name.
            if (!string.IsNullOrWhiteSpace(request.SearchText))
                query = query.Where(a => a.Text.ToLower().Contains(request.SearchText.ToLower()));

            // If a sort by field is provided, sort the query by that field; otherwise, sort by
            // first name and then last name.
            if(request.QuestionCategoryId != 0)
                query = query.Where(a => a.CategoryId == request.QuestionCategoryId);

            query = !string.IsNullOrWhiteSpace(request.SortBy)
                ? query.SortBy(request.SortBy)
                : query.OrderBy(a => a.Text)/*.ThenBy(a => a.LastName)*/;

            // Convert the query to a paged list of question item DTOs.
            var answers = await query.Select(q => QuestionItem.MapFromEntity(q, false))
                                            .AsNoTracking()
                                            .ToPagedListAsync(request.PageNumber, request.RowsPerPage);

            // Create an questions response DTO with the list of question item DTOs.
            var response = new GetQuestionsResponse
            {
                Questions = answers
            };

            // Return a success response with the questions response DTO as the payload.
            return Envelope<GetQuestionsResponse>.Result.Ok(response);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}