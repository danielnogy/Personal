namespace BinaryPlate.BlazorPlate.Features.SSM.Departments.Commands.UpdateDepartment;

public class UpdateDepartmentCommand 
{
    #region Public Properties

    public int Id { get; set; }
    public string Name { get; set; }

    public string ConcurrencyStamp { get; set; }



    #endregion Public Properties

}