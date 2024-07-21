using BinaryPlate.Domain.Entities.SSM;
using Mapster;

namespace BinaryPlate.Application.Features.SSM.QuestionCategories.Commands.CreateQuestionCategory;

public class CreateQuestionCategoryCommand : IRequest<Envelope<CreateQuestionCategoryResponse>>
{
    #region Public Properties

    public string Name { get; set; }
    public string Description { get; set; }

    #endregion Public Properties

    #region Public Methods

    public QuestionCategory MapToEntity()
    {
        return new QuestionCategory();
    }

    #endregion Public Methods

    #region Public Classes

    public class CreateQuestionCategoryCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<CreateQuestionCategoryCommand, Envelope<CreateQuestionCategoryResponse>>
    {
        #region Public Methods

        public async Task<Envelope<CreateQuestionCategoryResponse>> Handle(CreateQuestionCategoryCommand request, CancellationToken cancellationToken)
        {
            // Map the request to an entity.
            var questionCategory = request.Adapt<QuestionCategory>();

            // Add the questionCategory to the database context.
            await dbContext.QuestionCategories.AddAsync(questionCategory, cancellationToken);

            // Save changes to the database.
            await dbContext.SaveChangesAsync(cancellationToken);

            // Create a response with the questionCategory ID and a success message.
            var response = new CreateQuestionCategoryResponse
            {
                Id = questionCategory.Id,
                SuccessMessage = "Categoria creata cu succes" //Resource.QuestionCategory_has_been_created_successfully
            };

            // Return a result envelope with the response as the payload.
            return Envelope<CreateQuestionCategoryResponse>.Result.Ok(response);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}