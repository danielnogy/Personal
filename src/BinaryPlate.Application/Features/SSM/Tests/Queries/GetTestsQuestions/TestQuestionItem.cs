using BinaryPlate.Domain.Entities.SSM;
using Mapster;

namespace BinaryPlate.Application.Features.SSM.Tests.Queries.GetTestsQuestions;

public class TestQuestionItem : AuditableDto
{
    #region Public Properties

    public int Id { get; set; }
    public int TestId { get; set; }
    public int QuestionId { get; set; }

    #endregion Public Properties

    #region Public Methods

    public static TestQuestionItem MapFromEntity(TestQuestion testQuestion)
    {
        return testQuestion.Adapt<TestQuestionItem>();
    }

    #endregion Public Methods
}