using BinaryPlate.Domain.Entities.SSM;
using Mapster;

namespace BinaryPlate.Application.Features.SSM.MaterialCategories.Queries.GetMaterialCategoryForEdit;

public class GetMaterialCategoryForEditResponse : AuditableDto
{
    #region Public Properties
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public string ConcurrencyStamp { get; set; }

    #endregion Public Properties

    #region Public Methods

    public static GetMaterialCategoryForEditResponse MapFromEntity(MaterialCategory materialCategory)
    {
        return materialCategory.Adapt<GetMaterialCategoryForEditResponse>();
    }

    #endregion Public Methods
}