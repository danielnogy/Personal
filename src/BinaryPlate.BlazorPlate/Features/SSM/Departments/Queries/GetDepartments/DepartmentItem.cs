

namespace BinaryPlate.BlazorPlate.Features.SSM.Departments.Queries.GetDepartments;

public class DepartmentItem : AuditableDto
{
    #region Public Properties
    public int Id { get; set; }
    public string Name { get; set; }
    public string ConcurrencyStamp { get; set; }

    #endregion Public Properties
}