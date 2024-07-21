namespace BinaryPlate.BlazorPlate.Features.SSM.Tests.Queries.GetTestsResults;

public class TestResultItem : AuditableDto
{
    #region Public Properties
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public int TestId { get; set; }
    public DateTime DateTaken { get; set; }
    public decimal Score { get; set; }

    #endregion Public Properties

}