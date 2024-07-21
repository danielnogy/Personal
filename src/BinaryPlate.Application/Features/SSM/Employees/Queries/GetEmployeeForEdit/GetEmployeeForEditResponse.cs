using BinaryPlate.Domain.Entities.SSM;
using Mapster;

namespace BinaryPlate.Application.Features.SSM.Employees.Queries.GetEmployeeForEdit;

public class GetEmployeeForEditResponse : AuditableDto
{
    #region Public Properties
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Adress { get; set; }
    public int DepartmentId { get; set; }
    public string ConcurrencyStamp { get; set; }

    #endregion Public Properties

    #region Public Methods

    public static GetEmployeeForEditResponse MapFromEntity(Employee employee)
    {
        return employee.Adapt<GetEmployeeForEditResponse>();
    }

    #endregion Public Methods
}