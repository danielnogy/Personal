namespace BinaryPlate.BlazorPlate.Features.SSM.Materials.Queries.GetMaterialForEdit;

public class GetMaterialForEditResponse
{
    #region Public Properties
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Url { get; set; }
    public MaterialTypeEnum Type { get; set; } = MaterialTypeEnum.Video;
    public int? MaterialCategoryId { get; set; }
    public string ConcurrencyStamp { get; set; }

    #endregion Public Properties

}