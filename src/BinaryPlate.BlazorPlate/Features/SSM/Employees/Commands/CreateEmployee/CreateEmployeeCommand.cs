namespace BinaryPlate.BlazorPlate.Features.SSM.Employees.Commands.CreateEmployee;

public class CreateEmployeeCommand 
{
    #region Public Properties

    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Adress { get; set; }
    public int DepartmentId { get; set; }

    #endregion Public Properties



}