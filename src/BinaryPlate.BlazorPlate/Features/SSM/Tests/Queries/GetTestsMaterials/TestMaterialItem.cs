

namespace BinaryPlate.BlazorPlate.Features.SSM.Tests.Queries.GetTestsMaterials;

public class TestMaterialItem : AuditableDto
{
    #region Public Properties
    public int Id { get; set; }
    public int TestId { get; set; }
    public int MaterialId { get; set; }

    #endregion Public Properties

}