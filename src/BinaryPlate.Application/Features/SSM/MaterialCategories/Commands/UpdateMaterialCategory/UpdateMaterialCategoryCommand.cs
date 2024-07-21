using BinaryPlate.Domain.Entities.SSM;
using Mapster;

namespace BinaryPlate.Application.Features.SSM.MaterialCategories.Commands.UpdateMaterialCategory;

public class UpdateMaterialCategoryCommand : IRequest<Envelope<string>>
{
    #region Public Properties

    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ConcurrencyStamp { get; set; }

    #endregion Public Properties

    #region Public Methods

    public void MapToEntity(MaterialCategory materialCategory)
    {
        if (materialCategory == null)
            throw new ArgumentNullException(nameof(materialCategory));
        
    }

    #endregion Public Methods

    #region Private Methods

    #endregion Private Methods

    #region Public Classes

    public class UpdateMaterialCategoryCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<UpdateMaterialCategoryCommand, Envelope<string>>
    {
        #region Public Methods

        public async Task<Envelope<string>> Handle(UpdateMaterialCategoryCommand request, CancellationToken cancellationToken)
        {
            
            // Load the materialCategory from the database context and include their answers.
            var materialCategory = await dbContext.MaterialCategories
                                            .Where(a => a.Id == request.Id)
                                            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            // Return an error response if the materialCategory is not found.
            if (materialCategory == null)
                return Envelope<string>.Result.NotFound("Incarcarea categoriei a esuat");

            // Map the request to the loaded entity along with the related entities.
            request.Adapt(materialCategory);
            request.MapToEntity(materialCategory);


            // Update the entity in the database context.
            dbContext.MaterialCategories.Update(materialCategory);

            // Save changes to the database.
            await dbContext.SaveChangesAsync(cancellationToken);

            // Return a success response with a message.
            return Envelope<string>.Result.Ok("Categorie actualizata cu succes");
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}