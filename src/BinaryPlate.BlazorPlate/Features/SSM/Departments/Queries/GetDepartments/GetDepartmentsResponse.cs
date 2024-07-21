namespace BinaryPlate.BlazorPlate.Features.SSM.Departments.Queries.GetDepartments;

public class GetDepartmentsResponse
{
    #region Public Properties

    public PagedList<DepartmentItem> Departments { get; set; }

    #endregion Public Properties
}