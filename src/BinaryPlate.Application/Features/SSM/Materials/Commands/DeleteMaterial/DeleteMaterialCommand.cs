namespace BinaryPlate.Application.Features.SSM.Materials.Commands.DeleteMaterial;

public class DeleteMaterialCommand : IRequest<Envelope<string>>
{
    #region Public Properties

    public int Id { get; set; }

    #endregion Public Properties

    #region Public Classes

    public class DeleteMaterialCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<DeleteMaterialCommand, Envelope<string>>
    {
        #region Public Methods

        public async Task<Envelope<string>> Handle(DeleteMaterialCommand request, CancellationToken cancellationToken)
        {
            // Find the material with the given ID.
            var material = await dbContext.Materials.Where(a => a.Id == request.Id).FirstOrDefaultAsync(cancellationToken: cancellationToken);

            // If no material found with the given ID, return 404 NotFound response.
            if (material == null)
                return Envelope<string>.Result.NotFound("Materialul nu a fost gasit");

            // Remove the material from the Materials table.
            dbContext.Materials.Remove(material);

            // Save the changes in the database.
            await dbContext.SaveChangesAsync(cancellationToken);

            // Return a success response.
            return Envelope<string>.Result.Ok("Materialul a fost sters cu succes");
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}