

namespace BinaryPlate.BlazorPlate.Features.SSM.Tests.Queries.GetTestsQuestions;

public class TestQuestionItem : AuditableDto
{
    #region Public Properties

    public int Id { get; set; }
    public int TestId { get; set; }
    public int QuestionId { get; set; }

    #endregion Public Properties


}