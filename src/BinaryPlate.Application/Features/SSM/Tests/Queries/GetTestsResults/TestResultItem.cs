using BinaryPlate.Domain.Entities.SSM;
using Mapster;

namespace BinaryPlate.Application.Features.SSM.Tests.Queries.GetTestsResults;

public class TestResultItem : AuditableDto
{
    #region Public Properties
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public int TestId { get; set; }
    public DateTime DateTaken { get; set; }
    public decimal Score { get; set; }

    #endregion Public Properties

    #region Public Methods

    public static TestResultItem MapFromEntity(TestResult testResult)
    {
        return testResult.Adapt<TestResultItem>();
    }

    #endregion Public Methods
}