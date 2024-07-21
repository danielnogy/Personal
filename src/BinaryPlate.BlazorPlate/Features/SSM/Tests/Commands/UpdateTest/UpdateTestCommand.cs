using BinaryPlate.BlazorPlate.Features.SSM.Tests.Commands.CreateTest.AddModels;
using BinaryPlate.BlazorPlate.Features.SSM.Tests.Commands.UpdateTest.EditModels;

namespace BinaryPlate.BlazorPlate.Features.SSM.Tests.Commands.UpdateTest;

public class UpdateTestCommand 
{
    #region Public Properties

    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }

    public string ConcurrencyStamp { get; set; }
    //TestQuestion
    public List<TestQuestionItemForAdd> NewTestQuestions { get; set; } = new();
    public List<TestQuestionItemForEdit> ModifiedTestQuestions { get; set; } = new();
    public List<int> RemovedTestQuestions { get; set; } = new();

    //TestMaterial
    public List<TestMaterialItemForAdd> NewTestMaterials { get; set; } = new();
    public List<TestMaterialItemForEdit> ModifiedTestMaterials { get; set; } = new();
    public List<int> RemovedTestMaterials { get; set; } = new();

    //TestResult
    public List<TestResultItemForAdd> NewTestResults { get; set; } = new();
    public List<TestResultItemForEdit> ModifiedTestResults { get; set; } = new();
    public List<int> RemovedTestResults { get; set; } = new();

    #endregion Public Properties


}