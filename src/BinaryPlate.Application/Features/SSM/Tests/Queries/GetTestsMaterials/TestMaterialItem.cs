using BinaryPlate.Domain.Entities.SSM;
using Mapster;

namespace BinaryPlate.Application.Features.SSM.Tests.Queries.GetTestsMaterials;

public class TestMaterialItem : AuditableDto
{
    #region Public Properties
    public int Id { get; set; }
    public int TestId { get; set; }
    public int MaterialId { get; set; }

    #endregion Public Properties

    #region Public Methods

    public static TestMaterialItem MapFromEntity(TestMaterial testMaterial)
    {
        return testMaterial.Adapt<TestMaterialItem>();
    }

    #endregion Public Methods
}