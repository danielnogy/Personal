using BinaryPlate.Application.Features.SSM.Tests.Queries.GetTestsQuestions;
using BinaryPlate.Domain.Entities.SSM;
using Mapster;

namespace BinaryPlate.Application.Features.SSM.Tests.Queries.GetTests;

public class TestItem : AuditableDto
{
    #region Public Properties

    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ConcurrencyStamp { get; set; }
    public List<TestQuestionItem> TestQuestions { get; set; } = new();

    #endregion Public Properties

    #region Public Methods

    public static TestItem MapFromEntity(Test test, bool withTestQuestions = false)
    {
        var testItem = test.Adapt<TestItem>();
        if (withTestQuestions)
            return testItem;

        testItem.TestQuestions = new();
        return testItem;
    }

    #endregion Public Methods
}