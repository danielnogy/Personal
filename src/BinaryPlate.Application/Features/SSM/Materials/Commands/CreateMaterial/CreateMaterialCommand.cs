using BinaryPlate.Domain.Entities.SSM;
using Mapster;

namespace BinaryPlate.Application.Features.SSM.Materials.Commands.CreateMaterial;

public class CreateMaterialCommand : IRequest<Envelope<CreateMaterialResponse>>
{
    #region Public Properties

    public string Title { get; set; }
    public string Description { get; set; }
    public string Url { get; set; }
    public int Type { get; set; }
    public int? MaterialCategoryId { get; set; }


    #endregion Public Properties

    #region Public Methods

    public Material MapToEntity()
    {
        return new Material();
    }

    #endregion Public Methods

    #region Public Classes

    public class CreateMaterialCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<CreateMaterialCommand, Envelope<CreateMaterialResponse>>
    {
        #region Public Methods

        public async Task<Envelope<CreateMaterialResponse>> Handle(CreateMaterialCommand request, CancellationToken cancellationToken)
        {
            // Map the request to an entity.
            var material = request.Adapt<Material>();

            // Add the material to the database context.
            await dbContext.Materials.AddAsync(material, cancellationToken);

            // Save changes to the database.
            await dbContext.SaveChangesAsync(cancellationToken);

            // Create a response with the material ID and a success message.
            var response = new CreateMaterialResponse
            {
                Id = material.Id,
                SuccessMessage = "Material creat cu succes" //Resource.Material_has_been_created_successfully
            };

            // Return a result envelope with the response as the payload.
            return Envelope<CreateMaterialResponse>.Result.Ok(response);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}