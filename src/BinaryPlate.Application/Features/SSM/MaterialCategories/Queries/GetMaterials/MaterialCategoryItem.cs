using BinaryPlate.Domain.Entities.SSM;
using Mapster;

namespace BinaryPlate.Application.Features.SSM.MaterialCategories.Queries.GetMaterialCategories;

public class MaterialCategoryItem : AuditableDto
{
    #region Public Properties

    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    #endregion Public Properties

    #region Public Methods

    public static MaterialCategoryItem MapFromEntity(MaterialCategory materialCategory)
    {
        return materialCategory.Adapt<MaterialCategoryItem>();
    }

    #endregion Public Methods
}