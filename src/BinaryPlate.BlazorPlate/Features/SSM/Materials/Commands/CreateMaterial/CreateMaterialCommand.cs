namespace BinaryPlate.BlazorPlate.Features.SSM.Materials.Commands.CreateMaterial;

public class CreateMaterialCommand 
{
    #region Public Properties

    public string Title { get; set; }
    public string Description { get; set; }
    public string Url { get; set; }
    public MaterialTypeEnum Type { get; set; } = MaterialTypeEnum.Video;

    public int? MaterialCategoryId { get; set; }
    #endregion Public Properties
}