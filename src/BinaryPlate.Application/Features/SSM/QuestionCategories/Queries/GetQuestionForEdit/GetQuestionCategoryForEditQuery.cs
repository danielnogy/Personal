namespace BinaryPlate.Application.Features.SSM.QuestionCategories.Queries.GetQuestionCategoryForEdit;

public class GetQuestionCategoryForEditQuery : IRequest<Envelope<GetQuestionCategoryForEditResponse>>
{
    #region Public Properties

    public int Id { get; set; }

    #endregion Public Properties

    #region Public Classes

    public class GetQuestionCategoryForEditQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetQuestionCategoryForEditQuery, Envelope<GetQuestionCategoryForEditResponse>>
    {
        #region Public Methods

        public async Task<Envelope<GetQuestionCategoryForEditResponse>> Handle(GetQuestionCategoryForEditQuery request, CancellationToken cancellationToken)
        {
            
            // Retrieve the questionCategory from the database using the ID.
            var questionCategory = await dbContext.QuestionCategories.Where(a => a.Id == request.Id).FirstOrDefaultAsync(cancellationToken: cancellationToken);

            // If the questionCategory is not found, return a not found response.
            if (questionCategory == null)
                return Envelope<GetQuestionCategoryForEditResponse>.Result.NotFound("Incarcarea categoriei a esuat");

            // Map the questionCategory entity to an questionCategory response DTO.
            var questionCategoryForEditResponse = GetQuestionCategoryForEditResponse.MapFromEntity(questionCategory);

            // Return a success response with the questionCategory response DTO as the payload.
            return Envelope<GetQuestionCategoryForEditResponse>.Result.Ok(questionCategoryForEditResponse);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}