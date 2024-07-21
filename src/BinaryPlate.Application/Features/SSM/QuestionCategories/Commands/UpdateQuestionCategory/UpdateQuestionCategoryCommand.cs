using BinaryPlate.Domain.Entities.SSM;
using Mapster;

namespace BinaryPlate.Application.Features.SSM.QuestionCategories.Commands.UpdateQuestionCategory;

public class UpdateQuestionCategoryCommand : IRequest<Envelope<string>>
{
    #region Public Properties

    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ConcurrencyStamp { get; set; }

    #endregion Public Properties

    #region Public Methods

    public void MapToEntity(QuestionCategory questionCategory)
    {
        if (questionCategory == null)
            throw new ArgumentNullException(nameof(questionCategory));
        
    }

    #endregion Public Methods

    #region Private Methods

    #endregion Private Methods

    #region Public Classes

    public class UpdateQuestionCategoryCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<UpdateQuestionCategoryCommand, Envelope<string>>
    {
        #region Public Methods

        public async Task<Envelope<string>> Handle(UpdateQuestionCategoryCommand request, CancellationToken cancellationToken)
        {
            
            // Load the questionCategory from the database context and include their answers.
            var questionCategory = await dbContext.QuestionCategories
                                            .Where(a => a.Id == request.Id)
                                            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            // Return an error response if the questionCategory is not found.
            if (questionCategory == null)
                return Envelope<string>.Result.NotFound("Incarcarea categoriei a esuat");

            // Map the request to the loaded entity along with the related entities.
            request.Adapt(questionCategory);
            request.MapToEntity(questionCategory);


            // Update the entity in the database context.
            dbContext.QuestionCategories.Update(questionCategory);

            // Save changes to the database.
            await dbContext.SaveChangesAsync(cancellationToken);

            // Return a success response with a message.
            return Envelope<string>.Result.Ok("Categorie actualizata cu succes");
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}