namespace BinaryPlate.BlazorPlate.Features.SSM.MaterialCategories.Queries.GetMaterialCategoriess;

public class MaterialCategoryItem : AuditableDto
{
    #region Public Properties

    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    #endregion Public Properties

}