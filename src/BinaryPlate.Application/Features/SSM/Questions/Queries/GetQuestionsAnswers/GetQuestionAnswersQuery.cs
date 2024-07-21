namespace BinaryPlate.Application.Features.SSM.Questions.Queries.GetQuestionsAnswers;

public class GetQuestionAnswersQuery : FilterableQuery, IRequest<Envelope<GetAnswersResponse>>
{
    #region Public Properties

    public int QuestionId { get; set; }

    #endregion Public Properties

    public class GetAnswersQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetQuestionAnswersQuery, Envelope<GetAnswersResponse>>
    {
        #region Public Methods

        #region Public Methods

        public async Task<Envelope<GetAnswersResponse>> Handle(GetQuestionAnswersQuery request, CancellationToken cancellationToken)
        {
            // Start with a query that retrieves all references for the specified question from the database.
            var query = dbContext.Answers.Include(x=>x.Question).Where(a => a.QuestionId == request.QuestionId);

            // If a sort by field is provided, sort the query by that field; otherwise, sort by name.
            query = !string.IsNullOrWhiteSpace(request.SortBy)
                ? query.SortBy(request.SortBy)
                : query.OrderBy(a => a.Question.Text);

            // Convert the query to a paged list of question reference item DTOs.
            var answerItems = await query.Select(q => QuestionAnswerItem.MapFromEntity(q)).
                                                      ToPagedListAsync(request.PageNumber, request.RowsPerPage);

            // Create an question references response DTO with the list of question reference item DTOs.
            var response = new GetAnswersResponse
            {
                Answers = answerItems
            };

            // Return a success response with the question references response DTO as the payload.
            return Envelope<GetAnswersResponse>.Result.Ok(response);
        }

        #endregion Public Methods
    }

    #endregion Public Methods
}