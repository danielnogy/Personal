namespace BinaryPlate.Application.Features.SSM.Questions.Queries.GetQuestionForEdit;

public class GetQuestionForEditQuery : IRequest<Envelope<GetQuestionForEditResponse>>
{
    #region Public Properties

    public int Id { get; set; }

    #endregion Public Properties

    #region Public Classes

    public class GetQuestionForEditQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetQuestionForEditQuery, Envelope<GetQuestionForEditResponse>>
    {
        #region Public Methods

        public async Task<Envelope<GetQuestionForEditResponse>> Handle(GetQuestionForEditQuery request, CancellationToken cancellationToken)
        {
            
            // Retrieve the question from the database using the ID.
            var question = await dbContext.Questions.Where(a => a.Id == request.Id).FirstOrDefaultAsync(cancellationToken: cancellationToken);

            // If the question is not found, return a not found response.
            if (question == null)
                return Envelope<GetQuestionForEditResponse>.Result.NotFound("Incarcarea intrebarii a esuat");

            // Map the question entity to an question response DTO.
            var questionForEditResponse = GetQuestionForEditResponse.MapFromEntity(question);

            // Return a success response with the question response DTO as the payload.
            return Envelope<GetQuestionForEditResponse>.Result.Ok(questionForEditResponse);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}