using BinaryPlate.Domain.Entities.SSM;
using Mapster;

namespace BinaryPlate.Application.Features.SSM.Departments.Queries.GetDepartments;

public class DepartmentItem : AuditableDto
{
    #region Public Properties

    public int Id { get; set; }
    public string Name { get; set; }


    #endregion Public Properties

    #region Public Methods

    public static DepartmentItem MapFromEntity(Department department)
    {
        return department.Adapt<DepartmentItem>();
         
    }

    #endregion Public Methods
}