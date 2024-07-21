using BinaryPlate.Domain.Entities.SSM;
using Mapster;

namespace BinaryPlate.Application.Features.SSM.Materials.Queries.GetMaterials;

public class MaterialItem : AuditableDto
{
    #region Public Properties
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Url { get; set; }
    public int Type { get; set; }
    public int? MaterialCategoryId { get; set; }
    #endregion Public Properties

    #region Public Methods

    public static MaterialItem MapFromEntity(Material material)
    {
        return material.Adapt<MaterialItem>();
         
    }

    #endregion Public Methods
}