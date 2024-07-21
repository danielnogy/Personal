using BinaryPlate.BlazorPlate.Features.SSM.Tests.Commands.CreateTest.AddModels;


namespace BinaryPlate.BlazorPlate.Features.SSM.Tests.Commands.CreateTest;

public class CreateTestCommand 
{
    #region Public Properties

    public string Title { get; set; }
    public string Description { get; set; }

    public IReadOnlyList<TestQuestionItemForAdd> TestQuestionItems { get; set; } = new List<TestQuestionItemForAdd>();
    public IReadOnlyList<TestMaterialItemForAdd> TestMaterialItems { get; set; } = new List<TestMaterialItemForAdd>();
    public IReadOnlyList<TestResultItemForAdd> TestResultItems { get; set; } = new List<TestResultItemForAdd>();


    #endregion Public Properties

}