namespace BinaryPlate.Application.Features.SSM.Materials.Queries.GetMaterialForEdit;

public class GetMaterialForEditQuery : IRequest<Envelope<GetMaterialForEditResponse>>
{
    #region Public Properties

    public int Id { get; set; }

    #endregion Public Properties

    #region Public Classes

    public class GetMaterialForEditQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetMaterialForEditQuery, Envelope<GetMaterialForEditResponse>>
    {
        #region Public Methods

        public async Task<Envelope<GetMaterialForEditResponse>> Handle(GetMaterialForEditQuery request, CancellationToken cancellationToken)
        {
            
            // Retrieve the material from the database using the ID.
            var material = await dbContext.Materials.Where(a => a.Id == request.Id).FirstOrDefaultAsync(cancellationToken: cancellationToken);

            // If the material is not found, return a not found response.
            if (material == null)
                return Envelope<GetMaterialForEditResponse>.Result.NotFound("Incarcarea materialului a esuat");

            // Map the material entity to an material response DTO.
            var materialForEditResponse = GetMaterialForEditResponse.MapFromEntity(material);

            // Return a success response with the material response DTO as the payload.
            return Envelope<GetMaterialForEditResponse>.Result.Ok(materialForEditResponse);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}