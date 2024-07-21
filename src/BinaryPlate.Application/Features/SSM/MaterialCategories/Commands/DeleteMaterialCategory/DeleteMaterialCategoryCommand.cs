namespace BinaryPlate.Application.Features.SSM.MaterialCategories.Commands.DeleteMaterialCategory;

public class DeleteMaterialCategoryCommand : IRequest<Envelope<string>>
{
    #region Public Properties

    public int Id { get; set; }

    #endregion Public Properties

    #region Public Classes

    public class DeleteMaterialCategoryCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<DeleteMaterialCategoryCommand, Envelope<string>>
    {
        #region Public Methods

        public async Task<Envelope<string>> Handle(DeleteMaterialCategoryCommand request, CancellationToken cancellationToken)
        {
            // Find the questionCategory with the given ID.
            var questionCategory = await dbContext.MaterialCategories.Where(a => a.Id == request.Id).FirstOrDefaultAsync(cancellationToken: cancellationToken);

            // If no questionCategory found with the given ID, return 404 NotFound response.
            if (questionCategory == null)
                return Envelope<string>.Result.NotFound("Categoria nu a fost gasita");

            // Remove the questionCategory from the MaterialCategories table.
            dbContext.MaterialCategories.Remove(questionCategory);

            // Save the changes in the database.
            await dbContext.SaveChangesAsync(cancellationToken);

            // Return a success response.
            return Envelope<string>.Result.Ok("Categoria a fost stearsa cu succes");
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}