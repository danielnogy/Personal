using BinaryPlate.BlazorPlate.Features.SSM.Tests.Queries.GetTestsMaterials;
using BinaryPlate.BlazorPlate.Features.SSM.Tests.Queries.GetTestsQuestions;
using BinaryPlate.BlazorPlate.Features.SSM.Tests.Queries.GetTestsResults;

namespace BinaryPlate.BlazorPlate.Features.SSM.Tests.Queries.GetTests;

public class TestItem : AuditableDto
{
    #region Public Properties

    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public List<TestQuestionItem> TestQuestions { get; set; } = new();
    public List<TestMaterialItem> TestMaterials { get; set; } = new();
    public List<TestResultItem> TestResults { get; set; } = new();
    public string ConcurrencyStamp { get; set; }
    #endregion Public Properties


}