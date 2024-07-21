using BinaryPlate.Domain.Entities.SSM;
using Mapster;

namespace BinaryPlate.Application.Features.SSM.Departments.Queries.GetDepartmentForEdit;

public class GetDepartmentForEditResponse : AuditableDto
{
    #region Public Properties
    public int Id { get; set; }
    public string Name { get; set; }

    public string ConcurrencyStamp { get; set; }

    #endregion Public Properties

    #region Public Methods

    public static GetDepartmentForEditResponse MapFromEntity(Department department)
    {
        return department.Adapt<GetDepartmentForEditResponse>();
    }

    #endregion Public Methods
}