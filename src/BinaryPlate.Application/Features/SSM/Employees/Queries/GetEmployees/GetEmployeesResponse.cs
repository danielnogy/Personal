namespace BinaryPlate.Application.Features.SSM.Employees.Queries.GetEmployees;

public class GetEmployeesResponse
{
    #region Public Properties

    public PagedList<EmployeeItem> Employees { get; set; }

    #endregion Public Properties
}