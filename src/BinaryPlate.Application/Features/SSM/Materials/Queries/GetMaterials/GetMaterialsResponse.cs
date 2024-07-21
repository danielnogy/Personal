namespace BinaryPlate.Application.Features.SSM.Materials.Queries.GetMaterials;

public class GetMaterialsResponse
{
    #region Public Properties

    public PagedList<MaterialItem> Materials { get; set; }

    #endregion Public Properties
}