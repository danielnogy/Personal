namespace BinaryPlate.Application.Features.SSM.MaterialCategories.Queries.GetMaterialCategoryForEdit;

public class GetMaterialCategoryForEditQuery : IRequest<Envelope<GetMaterialCategoryForEditResponse>>
{
    #region Public Properties

    public int Id { get; set; }

    #endregion Public Properties

    #region Public Classes

    public class GetMaterialCategoryForEditQueryHandler(IApplicationDbContext dbContext) : IRequestHandler<GetMaterialCategoryForEditQuery, Envelope<GetMaterialCategoryForEditResponse>>
    {
        #region Public Methods

        public async Task<Envelope<GetMaterialCategoryForEditResponse>> Handle(GetMaterialCategoryForEditQuery request, CancellationToken cancellationToken)
        {
            
            // Retrieve the materialCategory from the database using the ID.
            var materialCategory = await dbContext.MaterialCategories.Where(a => a.Id == request.Id).FirstOrDefaultAsync(cancellationToken: cancellationToken);

            // If the materialCategory is not found, return a not found response.
            if (materialCategory == null)
                return Envelope<GetMaterialCategoryForEditResponse>.Result.NotFound("Incarcarea categoriei a esuat");

            // Map the materialCategory entity to an materialCategory response DTO.
            var materialCategoryForEditResponse = GetMaterialCategoryForEditResponse.MapFromEntity(materialCategory);

            // Return a success response with the materialCategory response DTO as the payload.
            return Envelope<GetMaterialCategoryForEditResponse>.Result.Ok(materialCategoryForEditResponse);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}