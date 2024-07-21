using BinaryPlate.Domain.Entities.SSM;
using Mapster;

namespace BinaryPlate.Application.Features.SSM.Employees.Queries.GetEmployees;

public class EmployeeItem : AuditableDto
{
    #region Public Properties

    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Adress { get; set; }
    public int DepartmentId { get; set; }

    #endregion Public Properties

    #region Public Methods

    public static EmployeeItem MapFromEntity(Employee employee)
    {
        return employee.Adapt<EmployeeItem>();
         
    }

    #endregion Public Methods
}