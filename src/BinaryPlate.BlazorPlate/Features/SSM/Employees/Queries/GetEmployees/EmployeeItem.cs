namespace BinaryPlate.BlazorPlate.Features.SSM.Employees.Queries.GetEmployees;

public class EmployeeItem : AuditableDto
{
    #region Public Properties

    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Adress { get; set; }
    public int DepartmentId { get; set; }

    #endregion Public Properties


}