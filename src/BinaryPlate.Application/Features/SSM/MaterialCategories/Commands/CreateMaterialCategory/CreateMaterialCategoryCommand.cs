using BinaryPlate.Domain.Entities.SSM;
using Mapster;

namespace BinaryPlate.Application.Features.SSM.MaterialCategories.Commands.CreateMaterialCategory;

public class CreateMaterialCategoryCommand : IRequest<Envelope<CreateMaterialCategoryResponse>>
{
    #region Public Properties

    public string Name { get; set; }
    public string Description { get; set; }

    #endregion Public Properties

    #region Public Methods

    public MaterialCategory MapToEntity()
    {
        return new MaterialCategory();
    }

    #endregion Public Methods

    #region Public Classes

    public class CreateMaterialCategoryCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<CreateMaterialCategoryCommand, Envelope<CreateMaterialCategoryResponse>>
    {
        #region Public Methods

        public async Task<Envelope<CreateMaterialCategoryResponse>> Handle(CreateMaterialCategoryCommand request, CancellationToken cancellationToken)
        {
            // Map the request to an entity.
            var materialCategory = request.Adapt<MaterialCategory>();

            // Add the materialCategory to the database context.
            await dbContext.MaterialCategories.AddAsync(materialCategory, cancellationToken);

            // Save changes to the database.
            await dbContext.SaveChangesAsync(cancellationToken);

            // Create a response with the materialCategory ID and a success message.
            var response = new CreateMaterialCategoryResponse
            {
                Id = materialCategory.Id,
                SuccessMessage = "Categoria creata cu succes" //Resource.MaterialCategory_has_been_created_successfully
            };

            // Return a result envelope with the response as the payload.
            return Envelope<CreateMaterialCategoryResponse>.Result.Ok(response);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}