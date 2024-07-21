using BinaryPlate.Domain.Entities.SSM;
using Mapster;

namespace BinaryPlate.Application.Features.SSM.Materials.Commands.UpdateMaterial;

public class UpdateMaterialCommand : IRequest<Envelope<string>>
{
    #region Public Properties

    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Url { get; set; }
    public int Type { get; set; }
    public int? MaterialCategoryId { get; set; }
    public string ConcurrencyStamp { get; set; }



    #endregion Public Properties

    #region Public Methods

    public void MapToEntity(Material material)
    {
        if (material == null)
            throw new ArgumentNullException(nameof(material));
        
    }

    #endregion Public Methods

    #region Private Methods

    #endregion Private Methods

    #region Public Classes

    public class UpdateMaterialCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<UpdateMaterialCommand, Envelope<string>>
    {
        #region Public Methods

        public async Task<Envelope<string>> Handle(UpdateMaterialCommand request, CancellationToken cancellationToken)
        {
            
            // Load the material from the database context and include their answers.
            var material = await dbContext.Materials
                                            .Where(a => a.Id == request.Id)
                                            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            // Return an error response if the material is not found.
            if (material == null)
                return Envelope<string>.Result.NotFound("Incarcarea materialului a esuat");

            // Map the request to the loaded entity along with the related entities.
            request.Adapt(material);
            request.MapToEntity(material);


            // Update the entity in the database context.
            dbContext.Materials.Update(material);

            // Save changes to the database.
            await dbContext.SaveChangesAsync(cancellationToken);

            // Return a success response with a message.
            return Envelope<string>.Result.Ok("Material actualizat cu succes");
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}