namespace BinaryPlate.BlazorPlate.Features.SSM.Employees.Commands.UpdateEmployee;

public class UpdateEmployeeCommand 
{
    #region Public Properties

    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Adress { get; set; }
    public int DepartmentId { get; set; }
    public string ConcurrencyStamp { get; set; }



    #endregion Public Properties




}