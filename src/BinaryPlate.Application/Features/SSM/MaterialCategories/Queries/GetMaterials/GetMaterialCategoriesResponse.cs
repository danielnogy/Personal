namespace BinaryPlate.Application.Features.SSM.MaterialCategories.Queries.GetMaterialCategories;

public class GetMaterialCategoriesResponse
{
    #region Public Properties

    public PagedList<MaterialCategoryItem> MaterialCategories { get; set; }

    #endregion Public Properties
}